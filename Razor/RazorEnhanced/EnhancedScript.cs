using Assistant;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Utils;
using RazorEnhanced.UOS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static RazorEnhanced.Scripts;

namespace RazorEnhanced
{
    public enum ScriptLanguage
    {
        UNKNOWN,
        PYTHON,
        CSHARP,
        UOSTEAM,
    }
    public class EnhancedScriptService { 
        public readonly static EnhancedScriptService Instance = new EnhancedScriptService();

        private  readonly ConcurrentDictionary<string, EnhancedScript> m_ScriptList = new ConcurrentDictionary<string, EnhancedScript>();

        internal List<EnhancedScript> ScriptList() 
        {
            var retList = new List<EnhancedScript>();
            foreach (var script in m_ScriptList.Values)
            {
                retList.Add(script);
            }
            retList.Sort(delegate (EnhancedScript s1, EnhancedScript s2)
            {
                return s1.Position.CompareTo(s2.Position);
            });
            return retList;
        }

        internal List<EnhancedScript> ScriptListEditor() { return ScriptList().Where(script => script.Editor).ToList(); }
        internal List<EnhancedScript> ScriptListTab() { return ScriptList().Where(script => script != null && !script.Editor && script.Exist).ToList(); }
        internal List<EnhancedScript> ScriptListTabPy() { return ScriptListTab().Where(script => script.Language == ScriptLanguage.PYTHON).ToList(); }
        internal List<EnhancedScript> ScriptListTabCs() { return ScriptListTab().Where(script => script.Language == ScriptLanguage.CSHARP).ToList(); }
        internal List<EnhancedScript> ScriptListTabUos() { return ScriptListTab().Where(script => script.Language == ScriptLanguage.UOSTEAM).ToList(); }

        public event Action<EnhancedScript, bool> OnScriptListChanged;

        internal bool AddScript(EnhancedScript script)
        {
            bool result = true;
            if (m_ScriptList.ContainsKey(script.Fullpath))
            {
                m_ScriptList[script.Fullpath] = script;
            }
            else
            {
                result = m_ScriptList.TryAdd(script.Fullpath, script);
            }
            OnScriptListChanged?.Invoke(script, result);
            return result;
        }
        internal bool RemoveScript(EnhancedScript script)
        {
            bool result = true;
            if (m_ScriptList.ContainsKey(script.Fullpath))
            {
                result = m_ScriptList.TryRemove(script.Fullpath, out _);
            }
            OnScriptListChanged?.Invoke(script, !result);
            return result;
        }

        internal EnhancedScript CurrentScript()
        {
            return Search(Thread.CurrentThread);
        }


        internal EnhancedScript Search(string filename, bool editor = false)
        {
            foreach (var script in ScriptList())
            {
                if (!editor && script.Editor) { continue; }
                if (script.Fullpath.ToLower() == filename.ToLower())
                {
                    return script;
                }
                if (script.Filename.ToLower() == filename.ToLower())
                {
                    return script;
                }
            }
            return null;
        }

        internal EnhancedScript Search(Thread thread)
        {
            foreach (var script in ScriptList())
            {
                if (script.Thread == null) { continue; }
                if (script.Thread.Equals(thread))
                {
                    return script;
                }
            }
            return null;
        }
        internal void ClearAll()
        {
            StopAll();
            m_ScriptList.Clear();
        }

        internal void StopAll()
        {
            foreach (var script in ScriptList())
            {
                script.Stop();
            }
        }
    }

    public class EnhancedScript
    {
        public readonly static EnhancedScriptService Service = EnhancedScriptService.Instance;

        private ScriptItem m_ScriptItem;
        private string m_Fullpath="";
        private System.IO.FileSystemWatcher m_Watcher;
        private string m_Text = "";
        
        private bool m_Wait;
        private bool m_Loop;
        private bool m_AutoStart;

        private ScriptLanguage m_Language = ScriptLanguage.UNKNOWN;
        private bool m_Preload;
        private bool m_Editor;
        private Keys m_Hotkey;
        private bool m_HotKeyPass;
        private int m_Position;

        private Thread m_Thread;

        internal EnhancedScriptEngine m_ScriptEngine;
        internal bool StartMessage;
        internal bool StopMessage;
        internal DateTime LastModified;

        private readonly object m_Lock = new object();

        public event Action<EnhancedScript, bool> OnLoad;
        public event Action<EnhancedScript> OnStart;
        public event Action<EnhancedScript> OnStop;
        public event Action<EnhancedScript, string> OnError;
        public event Action<EnhancedScript, string> OnOutput;
        
        

        public static ScriptLanguage ExtToLanguage(string extenstion)
        {
            switch (extenstion)
            {
                case ".py": return ScriptLanguage.PYTHON;
                case ".cs": return ScriptLanguage.CSHARP;
                case ".uos": return ScriptLanguage.UOSTEAM;
                default: return ScriptLanguage.UNKNOWN;
            }
        }

        public static string LanguageToExt(ScriptLanguage language)
        {
            switch (language)
            {
                case ScriptLanguage.PYTHON: return ".py";
                case ScriptLanguage.CSHARP: return ".cs";
                case ScriptLanguage.UOSTEAM: return ".uos";
                default: return ".txt";
            }
        }

        public static string TempFilename(ScriptLanguage language)
        {
            var ext = EnhancedScript.LanguageToExt(language);
            var dir = Misc.ScriptDirectory();
            var filename = "Untitled";
            var count = 1;
            string tempname;
            while (count < 10000)
            {
                tempname = Path.Combine(dir, filename + count + ext);
                if (!File.Exists(tempname) && Service.Search(tempname) == null)
                {
                    return tempname;
                }
                count++;
            }
            return Path.Combine(dir, filename + count + ext);
        }
        public static EnhancedScript FromScriptItem(ScriptItem item)
        {
            var preload = true;
            var editor = false;
            var script = FromFile(item.FullPath, item.Wait, item.Loop, item.Hotkey, item.HotKeyPass, item.AutoStart, item.Position, preload, editor);
            if (script == null) return null;
            script.ScriptItem = item;
            script.Position = item.Position;
            return script;
        }

        public static EnhancedScript FromFile(string fullpath, bool wait = false, bool loop = false, Keys hotkey = Keys.None, bool hotkeyPass = false, bool autostart = false, int position = -1, bool preload = true, bool editor = true)
        {
            if (!File.Exists(fullpath)) { return null; }

            var script = Service.Search(fullpath);
            if (script!=null){ return script; }

            script = new EnhancedScript(fullpath, "", wait, loop, hotkey, hotkeyPass, autostart, position, preload, editor);
            script.Load();
            return script;
        }

        public static EnhancedScript FromText(string content, ScriptLanguage language=ScriptLanguage.UNKNOWN, bool wait = false, bool loop = false, Keys hotkey=Keys.None, bool hotkeyPass=false, bool run = false, int position=-1, bool autostart = false, bool preload = true)
        {
            var filename = EnhancedScript.TempFilename(language);
            EnhancedScript script = new EnhancedScript(filename, content, wait, loop, hotkey, hotkeyPass, autostart, position, preload, true);
            script.SetLanguage(language);
            return script;
        }


        // beginning of instance related, non-static methods/constructors/propeerties
        internal EnhancedScript(string fullpath, string text, bool wait, bool loop, Keys hotkey, bool hotkeyPass, bool autostart, int position, bool preload, bool editor)
        {

            m_Fullpath = fullpath;
            
            m_Text = text;
            m_Wait = wait;
            m_Loop = loop;
            m_Hotkey = hotkey;
            m_HotKeyPass = hotkeyPass;
            m_AutoStart = autostart;
            

            m_Preload = preload;
            m_Editor = editor;
            

            StartMessage = true;
            StopMessage = false;
            LastModified = DateTime.MinValue;

            m_Watcher = new FileSystemWatcher(Path.GetDirectoryName(fullpath));
            m_Watcher.Filter = Path.GetFileName(fullpath);
            m_Watcher.NotifyFilter = NotifyFilters.LastWrite;
            m_Watcher.Changed += new FileSystemEventHandler(ScriptChanged);
            m_Watcher.EnableRaisingEvents = true;

            AttemptAddDirectoryWatcher(fullpath);

            m_ScriptEngine = new EnhancedScriptEngine(this, m_Preload);
            Add();

            if (autostart) { Start(); }
        }

        public bool Add()
        {
            return Service.AddScript(this);
        }

        public bool Remove() { 
            return Service.RemoveScript(this);
        }

        public ScriptItem ToScriptItem()
        {
            return m_ScriptItem;
        }

        public bool Load(bool force = false)
        {
            string content = m_Text ?? "";
            try
            {
                if (this.Exist)
                {
                    LastModified = File.GetLastWriteTime(Fullpath);
                    lock (EnhancedScriptEngine.IoLock)
                    {
                        content = ReadAllTextWithoutLocking(Fullpath);
                    }
                }
                else {
                    Misc.SendMessage("file not found:" + this.Filename, 178);
                }
            }
            catch (Exception e)
            {
                Misc.SendMessage("ERROR:EnhancedScript:Load: " + e.Message, 178);
                OnLoad?.Invoke(this, false);
                return false;
            }


            if (force || content != m_Text)
            {
                m_Text = content;
                InitEngine();
            }
            OnLoad?.Invoke(this,true);
            return true;
        }

        public bool InitEngine()
        {
            if (m_Text.Trim() == "") {
                return false;
            }
            if (m_Preload)
            {
                return m_ScriptEngine.Load();
            }
            return true;
        }



        public bool Save(){
            try {
                lock (EnhancedScriptEngine.IoLock)
                {
                    File.WriteAllText(Fullpath, Text);
                }
                LastModified = File.GetLastWriteTime(Fullpath);
                Load(true);
            } catch (Exception e) {
                Misc.SendMessage("ERROR:EnhancedScript:Save: " + e.Message, 178);
                return false;
            }
            return true;
        }

        //Methods



        public ScriptLanguage SetLanguage(ScriptLanguage language = ScriptLanguage.UNKNOWN)
        {    
            m_Language = ( language != ScriptLanguage.UNKNOWN ) ? language : GuessLanguage(); 
            return m_Language;
        }

        public ScriptLanguage GetLanguage()
        {
            if (m_Language == ScriptLanguage.UNKNOWN) {
                m_Language = GuessLanguage();
            }
            return m_Language;
        }

        public ScriptLanguage GuessLanguage()
        {
            
            if (m_Language == ScriptLanguage.UNKNOWN && HasValidPath)
            {
                string ext = Path.GetExtension(Fullpath).ToLower();
                m_Language = ExtToLanguage(ext);
            }
            //Dalamar
            //TODO: guess language based on content ?
            if (m_Language == ScriptLanguage.UNKNOWN)
            {
                m_Language = ScriptLanguage.PYTHON;
            }
            return m_Language;
        }

        static private Object ScriptStateLock = new Object();

        internal void Start()
        {
            //if (IsRunning || !IsUnstarted)
            if (IsRunning) return;

            lock (ScriptStateLock)
            {
                try
                {
                    //EventManager.Instance.Unsubscribe(m_Thread); 
                    if (m_Thread != null) { Stop(); }
                    m_Thread = new Thread(AsyncStart);
                    m_Thread.Start();
                    //while (!m_Thread.IsAlive){ Misc.Pause(1); }

                    //m_Run = true;
                }
                catch (Exception e)
                {
                    OnError.Invoke(this, e.Message);
                }
            }
        }

        private void AsyncStart()
        {
            do
            {
                if (World.Player == null) return;
              
                try
                {
                    //EventManager.Instance.Unsubscribe(m_Thread);
                    OnStart?.Invoke(this);
                    m_ScriptEngine.Run();
                    OnStop?.Invoke(this);
                }
                catch (ThreadAbortException ex) {
                    OnStop?.Invoke(this);
                    return; 
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, ex.Message);
                    Misc.SendMessage($"EnhancedScript:AsyncStart:{ex.GetType()}:\n{ex.Message}", 138);
                }
                
                Misc.Pause(1);
            } while (Loop);
        }

        internal void Stop()
        {
            if (IsRunning) {
                lock (ScriptStateLock)
                {
                    try
                    {
                        if (m_Thread.ThreadState != ThreadState.AbortRequested)
                        {
                            //EventManager.Instance.Unsubscribe(m_Thread);
                            //m_Thread.Interrupt();                            
                            m_Thread.Abort();
                            m_Thread.Join();
                            m_Thread = null;
                        }
                    }
                    catch { }
                }
            }
        }

        internal void Reset()
        {
            Stop();
            //m_Thread = new Thread(AsyncStart);
            //m_Run = false;
        }


        //Properties

        internal string Status
        {
            get
            {
                if (m_Thread == null) { return "Stopped"; }
                switch (m_Thread.ThreadState)
                {
                    
                    case ThreadState.AbortRequested:
                        return "Stopping";

                    case ThreadState.WaitSleepJoin:
                    case ThreadState.Running:
                        return "Running";


                    default:
                    case ThreadState.Unstarted:
                    case ThreadState.Aborted:
                        return "Stopped";
                }
            }
        }

        
        internal Thread Thread { get => m_Thread; }

        internal EnhancedScriptEngine ScriptEngine { get => m_ScriptEngine; }

        internal string Filename
        {
            get { lock (m_Lock) { return Path.GetFileName(m_Fullpath); } }
        }

        internal string Ext
        {
            get { lock (m_Lock) { return Path.GetExtension(m_Fullpath); } }
        }

        internal string Fullpath
        {
            get { lock (m_Lock) { return m_Fullpath; } }
            set { 
                lock (m_Lock) { m_Fullpath = value; }
                if (m_ScriptItem != null){
                    m_ScriptItem.FullPath = m_Fullpath;
                    m_ScriptItem.Filename = Filename;
                }
            }
        }

        internal ScriptItem ScriptItem
        {
            get { lock (m_Lock) { return ToScriptItem(); } }
            set { lock (m_Lock) { 
                    m_ScriptItem = value;
                    m_Fullpath = value.FullPath;
                } }
        }

        internal bool HasValidPath
        {
            get { 
                lock (m_Lock) {
                    if (m_Fullpath == null || m_Fullpath == "") { return false; }
                    if (Filename == null || Filename == "") { return false; }
                    var basedir = Path.GetDirectoryName(m_Fullpath);
                    if (!Directory.Exists(basedir)) { return false; }
                    return true;
                } }
        }

        internal bool Exist
        {
            get { lock (m_Lock) { return File.Exists(m_Fullpath); } }
        }



        internal string Text
        {
            get { lock (m_Lock) { return m_Text; } }
            set { lock (m_Lock) { m_Text = value; } }
        }


        internal ScriptLanguage Language
        {
            get { lock (m_Lock) { return GetLanguage(); } }
            set { lock (m_Lock) { SetLanguage(value); } }
        }
        


        internal bool Wait{
            get{lock (m_Lock){return m_Wait;}}
            set {
                lock (m_Lock) { m_Wait = value; }
                if (m_ScriptItem != null)
                {
                    m_ScriptItem.Wait = m_Wait;
                }
            }
        }

        
        internal bool Loop{
            get{lock (m_Lock){return m_Loop;}}
            set{lock (m_Lock){m_Loop = value;}
                if (m_ScriptItem != null)
                {
                    m_ScriptItem.Loop = m_Loop;
                }
            }
        }

        
        internal bool AutoStart{
            get{lock (m_Lock){return m_AutoStart;}}
            set{lock (m_Lock){m_AutoStart = value;}
                if (m_ScriptItem != null)
                {
                    m_ScriptItem.AutoStart = m_AutoStart;
                }
            }
        }

        internal bool Editor
        {
            get { lock (m_Lock) { return m_Editor; } }
            set { lock (m_Lock) { m_Editor = value; } }
        }
        internal bool Preload
        {
            get { lock (m_Lock) { return m_Preload; } }
            set { lock (m_Lock) { m_Preload = value; } }
        }

        internal Keys HotKey
        {
            get { lock (m_Lock) { return m_Hotkey; } }
            set { lock (m_Lock) { m_Hotkey = value; }
                if (m_ScriptItem != null)
                {
                    m_ScriptItem.Hotkey = m_Hotkey;
                }
            }
        }

        internal bool HotKeyPass
        {
            get { lock (m_Lock) { return m_HotKeyPass; } }
            set
            {
                lock (m_Lock) { m_HotKeyPass = value; }
                if (m_ScriptItem != null)
                {
                    m_ScriptItem.HotKeyPass = m_HotKeyPass;
                }
            }
        }

        internal int Position
        {
            get { lock (m_Lock) { return m_Position; } }
            set
            {
                lock (m_Lock) { m_Position = value; }
                if (m_ScriptItem != null)
                {
                    m_ScriptItem.Position = m_Position;
                }
            }
        }

        


        internal bool IsRunning
        {
            get
            {
                lock (m_Lock)
                {
                    if (m_Thread == null) { return false; }
                    if (  (m_Thread.ThreadState & 
                          ( ThreadState.Unstarted | 
                            ThreadState.Stopped | 
                            ThreadState.Aborted | 
                            ThreadState.StopRequested | 
                            ThreadState.AbortRequested) ) == 0)
                        //if (m_Thread.ThreadState == ThreadState.Running || m_Thread.ThreadState == ThreadState.WaitSleepJoin || m_Thread.ThreadState == ThreadState.AbortRequested)
                        return true;
                    else
                        return false;
                }
            }
        }

        public bool Suspended
        {
            get { return ScriptEngine.Suspended; }
            set { ScriptEngine.Suspended = value; }
        }
        public void Suspend()
        {
            ScriptEngine.Suspend();
        }

        public void Resume()
        {
            ScriptEngine.Resume();
        }


    }

// --------------------------------------------- ENHANCED SCRIPT ENGINE ----------------------------------------------------

    public class EnhancedScriptEngine
    {
        private EnhancedScript m_Script;
        private bool m_Loaded = false;

        static internal Object IoLock = new Object();

        public PythonEngine pyEngine;
        public CSharpEngine csEngine;
        public UOSteamEngine uosEngine;

        public Assembly csProgram;                                                    

        private TracebackDelegate m_pyTraceback;
        private UOSTracebackDelegate m_uosTraceback;
        private Action<string> m_stdoutWriter;
        private Action<string> m_stderrWriter;

        private bool m_Suspended = false;
        private ManualResetEvent m_SuspendedMutex;
        

        public EnhancedScriptEngine(EnhancedScript script, bool autoLoad = true)
        {
            m_SuspendedMutex = new ManualResetEvent(!m_Suspended);

            m_Script = script;
            var lang = script.SetLanguage();
            if (autoLoad && lang != ScriptLanguage.UNKNOWN)
            {
                Load();
            }
        }

        public bool Suspended
        { 
            get { return m_Suspended; }
            set { 
                if (value) { Suspend();} 
                else { Resume(); }
            } 
        }
        public void Suspend()
        {
            if (m_Script.Language == ScriptLanguage.CSHARP) {
                Misc.SendMessage("WARNING: Script Suspend is not supported by c# scripts.");
                return;
            }
            m_Suspended = true;
            m_SuspendedMutex.Reset();
        }

        public void Resume()
        {
            if (m_Script.Language == ScriptLanguage.CSHARP)
            {
                Misc.SendMessage("WARNING: Script Resume is not supported by c# scripts.");
                return;
            }
            m_Suspended = false;
            m_SuspendedMutex.Set();
        }



        public void SetTracebackPython(TracebackDelegate traceFunc)
        {
            m_pyTraceback = traceFunc;
        }

        public void SetTracebackUOS(UOSTracebackDelegate traceFunc)
        {
            m_uosTraceback = traceFunc;
        }

        public void SetStdout(Action<string> stdoutWriter)
        {
            m_stdoutWriter = stdoutWriter;
        }
        public void SetStderr(Action<string> stderrWriter)
        {
            m_stderrWriter = stderrWriter;
        }


        public void SendOutput(string message)
        {
            //Misc.SendMessage(message);
            SendMessageScriptError(message);
            if (m_stdoutWriter != null) {
                m_stdoutWriter(message);
            }
        }
        public void SendError(string message)
        {
            SendMessageScriptError(message, 138);
            if (m_stderrWriter != null) {
                m_stderrWriter(message);
            } else if (m_stdoutWriter != null) {
                m_stdoutWriter(message);
            }
        }


        ///<summary>
        /// Load the script and bring the specifict engine state at one step before execution.
        /// </summary>
        public bool Load()
        {
            if (m_Script.Text.Trim() == "") { 
                //what if there are only comments but no actual code ? 
                m_Loaded = false;
                return false;
            }

            m_Script.SetLanguage();
            try
            {
                switch (m_Script.Language)
                {
                    default:
                    case ScriptLanguage.PYTHON: m_Loaded = LoadPython(); break;
                    case ScriptLanguage.CSHARP: m_Loaded = LoadCSharp(); break;
                    case ScriptLanguage.UOSTEAM: m_Loaded = LoadUOSteam(); break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return m_Loaded;
        }

        ///<summary>
        /// Run the script.
        /// </summary>
        public bool Run()
        {
            if (!m_Loaded && !Load()) { return false; } // not loaded, and fail automatic loading.
            DateTime lastModified = File.GetLastWriteTime(m_Script.Fullpath);
            if (m_Script.LastModified < lastModified)
            {
                Load();
                // FileChangeDate update must be the last line of threads will messup (ex: mousewheel hotkeys)
                m_Script.LastModified = lastModified;
            }

            bool result;
            try {
                switch (m_Script.Language) {
                    default:
                    case ScriptLanguage.PYTHON: result = RunPython(); break;
                    case ScriptLanguage.CSHARP: result = RunCSharp(); break;
                    case ScriptLanguage.UOSTEAM: result = RunUOSteam(); break;
                }
            } catch (Exception ex) {
                result = HandleException(ex);
            }

            return result;
        }

        // ----------------------------------------- PYTHON -----------------------------

        private bool LoadPython()
        {

            try
            {
                pyEngine = new PythonEngine();

                //Clear path hooks (why?)
                var pc = HostingHelpers.GetLanguageContext(pyEngine.Engine) as PythonContext;
                var hooks = pc.SystemState.Get__dict__()["path_hooks"] as PythonDictionary;
                if (hooks != null) { hooks.Clear(); }

                //Load text
                var content = m_Script.Text ?? "";
                if (content == "" && File.Exists(m_Script.Fullpath))
                {
                    lock (IoLock)
                    {
                        content = ReadAllTextWithoutLocking(m_Script.Fullpath);
                    }
                }
                if (content == null || content == "") { 
                    return false; 
                } 

                if (!pyEngine.Load(content, m_Script.Fullpath)) {
                    return false;
                }

                //get list of imported files (?hopefully?)
                var filenames = pyEngine.Compiled.Engine.GetModuleFilenames();
                //TODO: get all loaded files and add file monitor to all of them.
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

            return true;
        }

        private TracebackDelegate TracebackPython(TraceBackFrame frame, string result, object payload)
        {
            if (m_pyTraceback != null)
            {
                m_pyTraceback = m_pyTraceback.Invoke(frame, result, payload);
            }

            SuspendCheck();
            return TracebackPython;
        }

        private bool TracebackUOS(UOS.Script script, UOS.ASTNode node, UOS.Scope scope)
        {
            if (m_uosTraceback != null)
            {
                return m_uosTraceback.Invoke(script, node, scope);
            }
            SuspendCheck();
            return true;
        }
        private void SuspendCheck() {
            if (Suspended) m_SuspendedMutex.WaitOne();
        }
        
        private bool RunPython()
        {
            try
            {
                pyEngine.SetTrace(TracebackPython);

                pyEngine.SetStderr(
                    (string message) => {
                        Misc.SendMessage(message,178);
                        if (m_stderrWriter == null) return;
                        m_stderrWriter.Invoke(message);
                    }
                );

                pyEngine.SetStdout(
                    (string message) => {
                        Misc.SendMessage(message);
                        if (m_stdoutWriter == null) return;
                        m_stdoutWriter.Invoke(message);
                    }
                );

                return pyEngine.Execute();
            } catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // ----------------------------------------- CSHARP -----------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        private bool LoadCSharp()
        {
            csProgram = null;
            bool result;
            try
            {
                csEngine = CSharpEngine.Instance;
                result = !csEngine.CompileFromFile(m_Script.Fullpath, true, out List<string> compileMessages, out Assembly assembly);
                if (result)
                {
                    csProgram = assembly;
                }
                foreach (string errorMessage in compileMessages)
                {
                    SendError(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }


            return result;
        }



        private bool RunCSharp()
        {
            try
            {
                csEngine.Execute(csProgram);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
            return true;
        }

        // ----------------------------------------- UOS -----------------------------

        private bool LoadUOSteam()
        {
            try
            {
                uosEngine = new UOSteamEngine();
                // Using // only will be deprecated instead of //UOS

                var content = m_Script.Text ?? "";
                if (content == "" && File.Exists(m_Script.Fullpath))
                {
                    lock (IoLock)
                    {
                        content = ReadAllTextWithoutLocking(m_Script.Fullpath);
                    }
                }
                if (content == null || content == "")
                {
                    return false;
                }

                /*
                var text = System.IO.File.ReadAllLines(m_Script.Fullpath);
                if ((text[0].Substring(0, 2) == "//") && text[0].Length < 5)
                {
                    string message = "WARNING: // header for UOS scripts is going to be deprecated. Please use //UOS instead";
                    SendOutput(message);
                }
                */

                

                uosEngine.SetStderr(
                    (string message) => {
                        Misc.SendMessage(message, 178);
                        if (m_stderrWriter == null) return;
                        m_stderrWriter.Invoke(message);
                    }
                );

                uosEngine.SetStdout(
                    (string message) => {
                        Misc.SendMessage(message);
                        if (m_stdoutWriter == null) return;
                        m_stdoutWriter.Invoke(message);
                    }
                );


                return uosEngine.Load(content, m_Script.Fullpath);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private bool RunUOSteam()
        {
            try
            {
                uosEngine.SetTrace(TracebackUOS);

                uosEngine.Execute();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
            return true;
        }

        // ----------------------------------------- Exceptions & Log -----------------------------
        public bool HandleException(Exception ex)
        {
            
            var exceptionType = ex.GetType();
            
            // GRACEFUL/SILENT EXIT
            if (exceptionType == typeof(ThreadAbortException)) { return true; } // thread stopped: All good
            if (exceptionType == typeof(SystemExitException)) { // sys.exit() or end of script
                return true;
            }

            // ERROR/VERBOSE EXIT
            var message = "";
            if (exceptionType == typeof(SyntaxErrorException))
            {
                SyntaxErrorException se = ex as SyntaxErrorException;
                message += "Syntax Error:" + Environment.NewLine;
                message += "- LINE: " + se.Line + Environment.NewLine;
                message += "- COLUMN: " + se.Column + Environment.NewLine;
                message += "- SEVERITY: " + se.Severity + Environment.NewLine;
                message += "- MESSAGE: " + se.Message + Environment.NewLine;
            }
            else if (
                exceptionType == typeof(UOSScriptError) ||
                exceptionType == typeof(UOSSyntaxError) ||
                exceptionType == typeof(UOSRuntimeError) ||
                exceptionType == typeof(UOSArgumentError))
            {
                UOSScriptError uos_se = ex as UOSScriptError;
                message += "\n"+uos_se.Message;
                //message += "- LINE: " + uos_se.LineNumber + Environment.NewLine;
                //message += "- CONTENT: " + uos_se.Line + Environment.NewLine;
                //message += "- LEXEME: " + uos_se.Node?.Lexeme??"" + Environment.NewLine;
                //message += "- MESSAGE: " + uos_se.Message + Environment.NewLine;
            }
            else if (m_Script.Language == ScriptLanguage.PYTHON)
            {
                message += "Python Error:";
                ExceptionOperations eo = pyEngine.Engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                message += Regex.Replace(error.Trim(), "\n\n", "\n");     //remove empty lines
            } else {
                message += "Generic Error:";
                message += Regex.Replace(ex.Message.Trim(), "\n\n", "\n");     //remove empty lines
            }

            m_Script.Stop();
            OutputException(message);
            return false;
        }

        private void OutputException(string message)
        {
            SendError("ERROR " + m_Script.Filename + ": " + message);
            if (Scripts.ScriptErrorLog) {
                LogException(message);
            }
        }
        private void LogException(string message)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Environment.NewLine);
            log.Append("============================ START REPORT ============================");
            log.Append(Environment.NewLine);
            log.Append("---> Time: " + String.Format("{0:F}", DateTime.Now) + Environment.NewLine);
            log.Append(Environment.NewLine);
            log.Append(message);
            log.Append(Environment.NewLine);
            log.Append("============================ END REPORT ============================");
            log.Append(Environment.NewLine);

            // For prevent crash in case of file are busy or inaccessible
            try
            {
                lock (IoLock)
                {
                    File.AppendAllText(Assistant.Engine.RootPath + "\\" + m_Script.Filename + ".ERROR", log.ToString());
                }
            }
            catch { }
            log.Clear();
        }       
    }
}
