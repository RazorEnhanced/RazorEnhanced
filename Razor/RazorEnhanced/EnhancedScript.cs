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
        internal bool StopMessage { get; set; }
        internal bool StartMessage { get; set; }

        internal DateTime FileChangeDate { get; set; }

        internal EnhancedScriptEngine m_ScriptEngine;
        internal EnhancedScriptEngine ScriptEngine { get => m_ScriptEngine; }


        internal EnhancedScript(string filename, string text, bool wait, bool loop, bool run, bool autostart)
        {
            StartMessage = true;
            StopMessage = false;
            m_Filename = filename;
            DateTime lastModified = DateTime.MinValue;
            FileChangeDate = lastModified;
            m_Text = text;
            m_Wait = wait;
            m_Loop = loop;
            m_Run = run;
            m_AutoStart = autostart;
            m_Thread = new Thread(AsyncStart);
            m_ScriptEngine = new EnhancedScriptEngine(this);
            m_ScriptEngine.Load(); //preload script
        }

        internal Thread Thread { get => m_Thread; }

        internal void Start()
        {
            if (IsRunning || !IsUnstarted)
                return;

            try
            {
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

        private readonly string m_Filename;
        internal string Filename
        {
            get
            {
                lock (m_Lock)
                {
                    return m_Filename;
                }
            }
        }

        private string m_Text;
        internal string Text
        {
            get
            {
                lock (m_Lock)
                {
                    return m_Text;
                }
            }
        }

        private Thread m_Thread;

        private readonly bool m_Wait;
        internal bool Wait
        {
            get
            {
                lock (m_Lock)
                {
                    return m_Wait;
                }
            }
        }

        private bool m_Loop;
        internal bool Loop
        {
            get
            {
                lock (m_Lock)
                {
                    return m_Loop;
                }
            }
            set
            {
                lock (m_Lock)
                {
                    m_Loop = value;
                }
            }
        }

        private bool m_AutoStart;
        internal bool AutoStart
        {
            get
            {
                lock (m_Lock)
                {
                    return m_AutoStart;
                }
            }
            set
            {
                lock (m_Lock)
                {
                    m_AutoStart = value;
                }
            }
        }

        private bool m_Run;
        internal bool Run
        {
            get
            {
                lock (m_Lock)
                {
                    return m_Run;
                }
            }
            set
            {
                lock (m_Lock)
                {
                    m_Run = value;
                }
            }
        }

        private readonly object m_Lock = new object();

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
        private bool m_Editor = false;

        private string m_Fullpath = "";
        private string m_Soruce = "";



        public PythonEngine pyEngine;
        public CSharpEngine csEngine;
        public UOSteamEngine uosEngine;

        public Assembly csProgram;
        public UOSScript uosProgram;

        public TracebackDelegate pyTraceback;
        public Action<string> sendStdout;
        public Action<string> sendStderr;


        public EnhancedScriptEngine(EnhancedScript script, bool autoLoad = true)
        {
            m_Script = script;
            m_Fullpath = Scripts.GetFullPathForScript(m_Script.Filename);
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
            sendStdout = stdoutWriter;
        }
        public void SetStderr(Action<string> stderrWriter)
        {
            sendStderr = stderrWriter;
        }

        public ScriptLanguage SetLanguage(ScriptLanguage language = ScriptLanguage.UNKNOWN)
        {
            string ext = Path.GetExtension(m_Fullpath).ToLower();

            m_Language = language;
            if (m_Language == ScriptLanguage.UNKNOWN)
            {
                switch (ext)
                {
                    case ".py": m_Language = ScriptLanguage.PYTHON; break;
                    case ".cs": m_Language = ScriptLanguage.CSHARP; break;
                    case ".uos": m_Language = ScriptLanguage.UOSTEAM; break;
                }
            }

            return m_Language;
        }

        ///<summary>
        /// Load the script and bring the specifict engine state at one step before execution.
        /// </summary>
        public bool Load()
        {
            try
            {
                switch (m_Language)
                {
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
            if (!File.Exists(m_Fullpath)) return false;

            try
            {
                pyEngine = new PythonEngine();

                //Clear path hooks (why?)
                var pc = HostingHelpers.GetLanguageContext(pyEngine.Engine) as PythonContext;
                var hooks = pc.SystemState.Get__dict__()["path_hooks"] as PythonDictionary;
                if (hooks != null) { hooks.Clear(); }

                //Load text
                var content = File.ReadAllText(m_Fullpath);
                pyEngine.Load(content);

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
            DateTime lastModified = System.IO.File.GetLastWriteTime(m_Fullpath);
            if (m_Script.FileChangeDate < lastModified)
            {
                LoadPython();
                // FileChangeDate update must be the last line of threads will messup (ex: mousewheel hotkeys)
                m_Script.FileChangeDate = System.IO.File.GetLastWriteTime(m_Fullpath);
            }

            if (pyTraceback != null)
            {
                pyEngine.Engine.SetTrace(pyTraceback);
            }
            if (sendStderr != null)
            {
                pyEngine.SetStderr(sendStderr);
            }
            if (sendStdout != null)
            {
                pyEngine.SetStdout(sendStdout);
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
                compileErrors = csEngine.CompileFromFile(m_Fullpath, true, out List<string> compileMessages, out Assembly assembly);
                if (!compileErrors)
                {
                    csProgram = assembly;
                }
                foreach (string str in compileMessages)
                {
                    Misc.SendMessage(str);
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
            var text = System.IO.File.ReadAllLines(m_Fullpath);
            if ((text[0].Substring(0, 2) == "//") && text[0].Length < 5)
            {
                string message = "WARNING: // header for UOS scripts is going to be deprecated. Please use //UOS instead";
                Misc.SendMessage(message);
            }
            uosProgram = uosEngine.Load(m_Fullpath);

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
            string display_error = ex.Message;
            if (pyEngine != null && pyEngine.Engine != null)
            {
                display_error = pyEngine.Engine.GetService<ExceptionOperations>().FormatException(ex);
            }
            var errorMessage = display_error.Replace("\n", " | ");

            SendMessageScriptError("ERROR " + m_Script.Filename + ": " + errorMessage);

            if (!logError) { return; }
            StringBuilder log = new StringBuilder();
            log.Append(Environment.NewLine + "============================ START REPORT ============================" + Environment.NewLine);

            DateTime dt = DateTime.Now;
            log.Append("---> Time: " + String.Format("{0:F}", dt) + Environment.NewLine);
            log.Append(Environment.NewLine);

            if (ex is SyntaxErrorException)
            {
                SyntaxErrorException se = ex as SyntaxErrorException;
                log.Append("----> Syntax Error:" + Environment.NewLine);
                log.Append("-> LINE: " + se.Line + Environment.NewLine);
                log.Append("-> COLUMN: " + se.Column + Environment.NewLine);
                log.Append("-> SEVERITY: " + se.Severity + Environment.NewLine);
                log.Append("-> MESSAGE: " + se.Message + Environment.NewLine);
            }
            else
            {

                log.Append("----> Generic Error:" + Environment.NewLine);
                log.Append(display_error);
            }

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
