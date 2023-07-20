using Assistant;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
 
using Microsoft.Scripting;
using static RazorEnhanced.Scripts;
using UOSScript = RazorEnhanced.UOScript.Script;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Linq;
using RazorEnhanced.UOScript;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Management.Instrumentation;

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

        internal List<EnhancedScript> ScriptList() { return m_ScriptList.Values.ToList(); }
        internal List<EnhancedScript> ScriptListEditor() { return m_ScriptList.Values.Where(script => script.Editor).ToList(); }
        internal List<EnhancedScript> ScriptListTab() { return m_ScriptList.Values.Where(script => !script.Editor && script.Exist).ToList(); }
        internal List<EnhancedScript> ScriptListTabPy() { return ScriptListTab().Where(script => script.Language == ScriptLanguage.PYTHON).ToList(); }
        internal List<EnhancedScript> ScriptListTabCs() { return ScriptListTab().Where(script => script.Language == ScriptLanguage.CSHARP).ToList(); }
        internal List<EnhancedScript> ScriptListTabUos() { return ScriptListTab().Where(script => script.Language == ScriptLanguage.UOSTEAM).ToList(); }


        internal bool AddScript(EnhancedScript script)
        {
            if (m_ScriptList.ContainsKey(script.Fullpath))
            {
                m_ScriptList[script.Fullpath] = script;
                return true;
            }
            else
            {
                return m_ScriptList.TryAdd(script.Fullpath, script);
            }
        }
        internal bool RemoveScript(EnhancedScript script)
        {
            if (m_ScriptList.ContainsKey(script.Fullpath))
            {
                return m_ScriptList.TryRemove(script.Fullpath, out _);
            }
            return true;
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
        private string m_Text = "";
        
        private bool m_Wait;
        private bool m_Loop;
        private bool m_AutoStart;

        private ScriptLanguage m_Language = ScriptLanguage.UNKNOWN;
        private bool m_Preload;
        private bool m_Editor;
        private Keys m_Hotkey;
        private bool m_HotKeyPass;

        private Thread m_Thread;

        internal EnhancedScriptEngine m_ScriptEngine;
        internal bool StopMessage;
        internal bool StartMessage;
        internal DateTime LastModified;

        private readonly object m_Lock = new object();



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
            var script = FromFile(item.FullPath, item.Wait, item.Loop, item.Hotkey, item.HotKeyPass, item.AutoStart, preload, editor);
            script.ScriptItem = item;
            return script;
        }

        public static EnhancedScript FromFile(string fullpath, bool wait = false, bool loop = false, Keys hotkey = Keys.None, bool hotkeyPass = false, bool autostart = false, bool preload = true, bool editor = true)
        {
            if (!File.Exists(fullpath)) { return null; }

            var script = Service.Search(fullpath);
            if (script!=null){ return script; }

            script = new EnhancedScript(fullpath, "", wait, loop, hotkey, hotkeyPass, autostart, preload, editor);

            script.Load();
            return script;
        }

        public static EnhancedScript FromText(string content, ScriptLanguage language=ScriptLanguage.UNKNOWN, bool wait = false, bool loop = false, Keys hotkey=Keys.None, bool hotkeyPass=false, bool run = false, bool autostart = false, bool preload = true)
        {
            var filename = EnhancedScript.TempFilename(language);
            EnhancedScript script = new EnhancedScript(filename, content, wait, loop, hotkey, hotkeyPass, autostart, preload, true);
            script.SetLanguage(language);
            return script;
        }


        // beginning of instance related, non-static methods/constructors/propeerties
        internal EnhancedScript(string fullpath, string text, bool wait, bool loop, Keys hotkey, bool hotkeyPass, bool autostart, bool preload, bool editor)
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
            if (m_ScriptItem == null)
            {
                m_ScriptItem = new ScriptItem()
                {
                    Loop = m_Loop,
                    Hotkey = m_Hotkey,
                    AutoStart = m_AutoStart,
                    Filename = Filename,
                    FullPath = m_Fullpath,
                    HotKeyPass = m_HotKeyPass,
                    Wait = m_Wait,
                };
            }
            return m_ScriptItem;
        }


        public bool Load(bool force=false)
        {
            string content;
            /* included in the catch? dirty ? 
            if (!File.Exists(Fullpath)) {
                m_Text = "";
                return false; 
            }
            */

            try{
                content = File.ReadAllText(Fullpath);
                LastModified = DateTime.Now;
            } catch (Exception e) {
                Misc.SendMessage("ERROR:EnhancedScript:Load: " + e.Message, 178);
                m_Text = "";
                return false;
            }

            if (force || content != m_Text ) { 
                m_Text = content;
                if (m_Preload) { 
                    m_ScriptEngine.Load();
                }
            }
            return true;
        }

        public bool Save(){
            try {
                File.WriteAllText(Fullpath, Text);
                LastModified = DateTime.Now;
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


        internal void Start()
        {
            //if (IsRunning || !IsUnstarted)
            if (IsRunning) return;

            try
            {
                //EventManager.Instance.Unsubscribe(m_Thread); 
                if (m_Thread != null) { Stop(); }
                m_Thread = new Thread(AsyncStart);
                m_Thread.Start();
                while (!m_Thread.IsAlive){ Misc.Pause(1); }

                //m_Run = true;
            }
            catch { }
        }

        private void AsyncStart()
        {
            do
            {
                if (World.Player == null) return;

                try
                {
                    //EventManager.Instance.Unsubscribe(m_Thread);
                    m_ScriptEngine.Run();
                }
                catch (ThreadAbortException ex) { return; }
                catch (Exception ex)
                {
                    Misc.SendMessage($"EnhancedScript:AsyncStart:{ex.GetType()}:\n{ex.Message}", 138);
                }
                
                Misc.Pause(1);
            } while (Loop);
        }

        internal void Stop()
        {
            if (IsRunning) { 
                try
                {
                    if (m_Thread.ThreadState != ThreadState.AbortRequested)
                    {
                        //EventManager.Instance.Unsubscribe(m_Thread);
                        m_Thread.Abort();
                        m_Thread = null;
                    }
                }
                catch { }
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
                        return "Run";


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
            set { lock (m_Lock) { m_Fullpath = value; } }
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
        }

        
        internal bool Loop{
            get{lock (m_Lock){return m_Loop;}}
            set{lock (m_Lock){m_Loop = value;}}
        }

        
        internal bool AutoStart{
            get{lock (m_Lock){return m_AutoStart;}}
            set{lock (m_Lock){m_AutoStart = value;}}
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
            set { lock (m_Lock) { m_Hotkey = value; } }
        }

        internal bool HotKeyPass
        {
            get { lock (m_Lock) { return m_HotKeyPass; } }
            set { lock (m_Lock) { m_HotKeyPass = value; } }
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

    }

// --------------------------------------------- ENHANCED SCRIPT ENGINE ----------------------------------------------------

    public class EnhancedScriptEngine
    {
        private EnhancedScript m_Script;
        private bool m_Loaded = false;
        
        public PythonEngine pyEngine;
        public CSharpEngine csEngine;
        public UOSteamEngine uosEngine;

        public Assembly csProgram;                                                    
        public UOSScript uosProgram;

        public TracebackDelegate pyTraceback;
        public Action<string> m_StdoutWriter;
        public Action<string> m_StderrWriter;


        


        public EnhancedScriptEngine(EnhancedScript script, bool autoLoad = true)
        {
            m_Script = script;
            var lang = script.SetLanguage();
            if (autoLoad && lang != ScriptLanguage.UNKNOWN)
            {
                Load();
            }
        }

        public void SetTracebackPython(TracebackDelegate traceFunc)
        {
            pyTraceback = traceFunc;
        }

        public void SetStdout(Action<string> stdoutWriter)
        {
            m_StdoutWriter = stdoutWriter;
        }
        public void SetStderr(Action<string> stderrWriter)
        {
            m_StderrWriter = stderrWriter;
        }


        public void SendOutput(string message)
        {
            //Misc.SendMessage(message);
            SendMessageScriptError(message);
            if (m_StdoutWriter != null) {
                m_StdoutWriter(message);
            }
        }
        public void SendError(string message)
        {
            SendMessageScriptError(message, 138);
            if (m_StderrWriter != null) {
                m_StderrWriter(message);
            } else if (m_StdoutWriter != null) {
                m_StdoutWriter(message);
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
            Misc.SendMessage($"ThreadID: {Thread.CurrentThread.ManagedThreadId}", 147);
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
                    content = File.ReadAllText(m_Script.Fullpath);
                }
                if (content == null || content == "") { 
                    return false; } 

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



        private bool RunPython()
        {
            try
            {
                DateTime lastModified = System.IO.File.GetLastWriteTime(m_Script.Fullpath);
                if (m_Script.LastModified < lastModified)
                {
                    LoadPython();
                    // FileChangeDate update must be the last line of threads will messup (ex: mousewheel hotkeys)
                    m_Script.LastModified = System.IO.File.GetLastWriteTime(m_Script.Fullpath);
                }

                if (pyTraceback != null)
                {
                    pyEngine.Engine.SetTrace(pyTraceback);
                }
                if (m_StderrWriter != null)
                {
                    pyEngine.SetStderr(m_StderrWriter);
                }
                if (m_StdoutWriter != null)
                {
                    pyEngine.SetStdout(m_StdoutWriter);
                }
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
            uosProgram = null;
            try
            {
                uosEngine = UOSteamEngine.Instance;
                // Using // only will be deprecated instead of //UOS
                var text = System.IO.File.ReadAllLines(m_Script.Fullpath);
                if ((text[0].Substring(0, 2) == "//") && text[0].Length < 5)
                {
                    string message = "WARNING: // header for UOS scripts is going to be deprecated. Please use //UOS instead";
                    SendOutput(message);
                }

                uosProgram = uosEngine.Load(m_Script.Fullpath);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
            return uosProgram != null;

        }

        private bool RunUOSteam()
        {
            try
            {
                uosEngine.Execute(uosProgram);
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
                File.AppendAllText(Assistant.Engine.RootPath + "\\" + m_Script.Filename + ".ERROR", log.ToString());
            }
            catch { }
            log.Clear();
        }





    }


}
