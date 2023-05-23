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

namespace RazorEnhanced
{
    public enum ScriptLanguage
    {
        UNKNOWN,
        PYTHON,
        CSHARP,
        UOSTEAM,
    }
    public class EnhancedScript
    {
        private string m_Filename="";
        private string m_Fullpath="";
        private string m_Text = "";
        
        private bool m_Wait;
        private bool m_Loop;
        private bool m_AutoStart;
        private bool m_Editor;
        private bool m_Run;

        private Thread m_Thread;

        internal EnhancedScriptEngine m_ScriptEngine;
        internal bool StopMessage;
        internal bool StartMessage;
        internal DateTime LastModified;

        private readonly object m_Lock = new object();
        
        internal EnhancedScript(string filename, string text, bool wait, bool loop, bool run, bool autostart, bool editor)
        {
            m_Filename = filename;
            

            m_Text = text;
            m_Wait = wait;
            m_Loop = loop;
            m_Run = run;
            m_AutoStart = autostart;
            m_Editor = editor;

            StartMessage = true;
            StopMessage = false;
            LastModified = DateTime.MinValue;


            m_Thread = new Thread(AsyncStart);
            m_ScriptEngine = new EnhancedScriptEngine(this, true);
        }

        //Methods

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


        internal void Start()
        {
            if (IsRunning || !IsUnstarted)
                return;

            try
            {
                //EventManager.Instance.Unsubscribe(m_Thread); 
                m_Thread.Start();
                while (!m_Thread.IsAlive)
                {
                }

                m_Run = true;
            }
            catch { }
        }

        private void AsyncStart()
        {
            if (World.Player == null) return;

            try
            {
                //EventManager.Instance.Unsubscribe(m_Thread);
                m_ScriptEngine.Run();
            }
            catch (Exception ex)
            {
                Stop();
                Misc.SendMessage($"EnhancedScript:AsyncStart:{ex.Message}", 138);
            }
        }

        internal void Stop()
        {
            if (!IsStopped)
                try
                {
                    if (m_Thread.ThreadState != ThreadState.AbortRequested)
                    {
                        //EventManager.Instance.Unsubscribe(m_Thread);
                        m_Thread.Abort();
                    }
                }
                catch { }
        }

        internal void Reset()
        {
            m_Thread = new Thread(AsyncStart);
            m_Run = false;
        }


        //Properties

        internal string Status
        {
            get
            {
                switch (m_Thread.ThreadState)
                {
                    case ThreadState.Aborted:
                    case ThreadState.AbortRequested:
                        return "Stopping";

                    case ThreadState.WaitSleepJoin:
                    case ThreadState.Running:
                        return "Running";

                    default:
                        return "Stopped";
                }
            }
        }


        internal Thread Thread { get => m_Thread; }

        internal EnhancedScriptEngine ScriptEngine { get => m_ScriptEngine; }

        internal string Filename {
            get{ lock (m_Lock) { return m_Filename; } }
            set{ lock (m_Lock) { 
                m_Filename = value;
                m_Fullpath = Scripts.GetFullPathForScript(m_Filename);
            }}
        }

        internal string Fullpath
        {
            get { lock (m_Lock) { return m_Fullpath; } }
            set { lock (m_Lock) {
                m_Fullpath = value;
                m_Filename = Path.GetFileName(m_Fullpath);
            }}
        }



        internal string Text{
            get{lock (m_Lock){return m_Text;}}
            set{lock (m_Lock){m_Text = value;}}
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
        
        internal bool Editor{
            get{lock (m_Lock){return m_Editor;}}
        }
        
        internal bool Run{
            get{lock (m_Lock){return m_Run;}}
            set{lock (m_Lock){m_Run = value;}}
        }


       

        internal bool IsRunning
        {
            get
            {
                lock (m_Lock)
                {
                    if ((m_Thread.ThreadState & (ThreadState.Unstarted | ThreadState.Stopped)) == 0)
                        //if (m_Thread.ThreadState == ThreadState.Running || m_Thread.ThreadState == ThreadState.WaitSleepJoin || m_Thread.ThreadState == ThreadState.AbortRequested)
                        return true;
                    else
                        return false;
                }
            }
        }

        internal bool IsStopped
        {
            get
            {
                lock (m_Lock)
                {
                    if ((m_Thread.ThreadState & ThreadState.Stopped) != 0 || (m_Thread.ThreadState & ThreadState.Aborted) != 0)
                        //if (m_Thread.ThreadState == ThreadState.Stopped || m_Thread.ThreadState == ThreadState.Aborted)
                        return true;
                    else
                        return false;
                }
            }
        }

        internal bool IsUnstarted
        {
            get
            {
                lock (m_Lock)
                {
                    if ((m_Thread.ThreadState & ThreadState.Unstarted) != 0)
                        //  if (m_Thread.ThreadState == ThreadState.Unstarted)
                        return true;
                    else
                        return false;
                }
            }
        }

        
    }


    




    public class EnhancedScriptEngine
    {
        private EnhancedScript m_Script;
        private ScriptLanguage m_Language = ScriptLanguage.UNKNOWN;
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
            var lang = SetLanguage();
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


        public ScriptLanguage SetLanguage(ScriptLanguage language = ScriptLanguage.UNKNOWN)
        {
            if (language != ScriptLanguage.UNKNOWN) { m_Language = language; }
            if (m_Language == ScriptLanguage.UNKNOWN)
            {
                string ext = Path.GetExtension(m_Script.Fullpath).ToLower();
                m_Language = EnhancedScript.ExtToLanguage(ext);
            }

            return m_Language;
        }

        public ScriptLanguage GetLanguage()
        {
            if (m_Language == ScriptLanguage.UNKNOWN) { SetLanguage(); }
            return m_Language;
        }

        ///<summary>
        /// Load the script and bring the specifict engine state at one step before execution.
        /// </summary>
        public bool Load()
        {
            SetLanguage();
            try
            {
                switch (m_Language)
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
            if (!m_Loaded)
            {
                Load();
            }

            try
            {
                switch (m_Language)
                {
                    default:
                    case ScriptLanguage.PYTHON: return RunPython();
                    case ScriptLanguage.CSHARP: return RunCSharp();
                    case ScriptLanguage.UOSTEAM: return RunUOSteam();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
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
                var content = m_Script.Text;
                if (content == "" && File.Exists(m_Script.Fullpath)){ 
                    content = File.ReadAllText(m_Script.Fullpath); 
                }
                pyEngine.Load(content, m_Script.Fullpath);

                //get list of imported files (?hopefully?)
                var filenames = pyEngine.Compiled.Engine.GetModuleFilenames();
                //TODO: get all loaded files and add file monitor to all of them.
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }



            return true;
        }



        private bool RunPython()
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
            pyEngine.Execute();
            return true;
        }

        // ----------------------------------------- CSHARP -----------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        private bool LoadCSharp()
        {
            bool compileErrors = true;
            try
            {
                csEngine = CSharpEngine.Instance;
                compileErrors = csEngine.CompileFromFile(m_Script.Fullpath, true, out List<string> compileMessages, out Assembly assembly);
                if (!compileErrors)
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
                HandleException(ex);
                return false;
            }


            return !compileErrors;
        }



        private bool RunCSharp()
        {
            try
            {
                csEngine.Execute(csProgram);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
            return true;
        }

        // ----------------------------------------- UOS -----------------------------

        private bool LoadUOSteam()
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
                HandleException(ex);
                return false;
            }
            return true;
        }

        // ----------------------------------------- Exceptions & Log -----------------------------
        private void HandleException(Exception ex)
        {
            var exType = ex.GetType();

            // All good
            if (exType is System.Threading.ThreadAbortException) { return; }
            // sys.exit()
            if (exType is IronPython.Runtime.Exceptions.SystemExitException)
            {
                m_Script.Stop();
                return;
            }

            OutputException(ex, Scripts.ScriptErrorLog);
        }

        private void OutputException(Exception ex, bool logError = false)
        {
            String message = "";
            if (ex is SyntaxErrorException)
            {
                SyntaxErrorException se = ex as SyntaxErrorException;
                message += "Syntax Error:" + Environment.NewLine;
                message += "- LINE: " + se.Line + Environment.NewLine;
                message += "- COLUMN: " + se.Column + Environment.NewLine;
                message += "- SEVERITY: " + se.Severity + Environment.NewLine;
                message += "- MESSAGE: " + se.Message + Environment.NewLine;
            }
            else
            {
                message += "Generic Error:";
                ExceptionOperations eo = pyEngine.Engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                message += Regex.Replace(error.Trim(), "\n\n", "\n");     //remove empty lines
            }
            SendError("ERROR " + m_Script.Filename + ": " + message);

            if (logError) {
                LogException(message);
            }
        }
        private void LogException(string message)
        {
            
            StringBuilder log = new StringBuilder();
            log.Append(Environment.NewLine);
            log.Append("============================ START REPORT ============================");
            log.Append(Environment.NewLine);
            DateTime dt = DateTime.Now;
            log.Append("---> Time: " + String.Format("{0:F}", dt) + Environment.NewLine);
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
