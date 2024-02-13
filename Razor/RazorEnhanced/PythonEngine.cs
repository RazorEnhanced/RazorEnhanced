using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronPython.Runtime;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using IronPython.Compiler;
using System.IO;

namespace RazorEnhanced
{
    // --------------------------------------------------------- PythonEngine --------------------------------------------------------
    public class PythonEngine
    {


        public Dictionary<string, object> Modules;

        public ScriptRuntime Runtime;
        public ScriptEngine Engine { get;  }
        public ScriptScope Scope { get; set; }
        public String Text { get; set; }
        public String FilePath { get; set; }
        public ScriptSource Source { get; set; }
        public CompiledCode Compiled { get; set; }
        public PythonCompilerOptions CompilerOptions { get; set; }

        

        public PythonEngine() {
            Runtime = IronPython.Hosting.Python.CreateRuntime();
            Engine = IronPython.Hosting.Python.GetEngine(Runtime);
            
            //Paths for IronPython 3.4
            var paths = new List<string>();
            var basepath = Assistant.Engine.RootPath;
            // IronPython 3.4 add some default absolute paths: ./, ./Lib, ./DLLs
            // When run via CUO the paths are messed up, so we ditch the default ones and put the correct ones.
            // Order matters:
            // 1- ./Script/
            paths.Add(Misc.CurrentScriptDirectory());
            // 2- ./Lib/
            paths.Add(Path.Combine(basepath, "Lib"));
            // 3- ./
            paths.Add(basepath);

            Engine.SetSearchPaths(paths);

            // Add also defult IronPython 3.4 installlation folder, if present
            if (System.IO.Directory.Exists(@"C:\Program Files\IronPython 3.4"))
            {
                paths.Add(@"C:\Program Files\IronPython 3.4");
                paths.Add(@"C:\Program Files\IronPython 3.4\Lib"); 
                paths.Add(@"C:\Program Files\IronPython 3.4\DLLs");
                paths.Add(@"C:\Program Files\IronPython 3.4\Scripts");
            }

            //RE Modules list
            Modules = new Dictionary<string, object>{
                { "Misc", new RazorEnhanced.Misc() },
                { "Items", new RazorEnhanced.Items() },
                { "Mobiles", new RazorEnhanced.Mobiles() },
                { "Player", new RazorEnhanced.Player() },
                { "Spells", new RazorEnhanced.Spells() },
                { "Gumps", new RazorEnhanced.Gumps() },
                { "Journal", new RazorEnhanced.Journal() },
                { "Target", new RazorEnhanced.Target() },
                { "Statics", new RazorEnhanced.Statics() },
                { "Sound", new RazorEnhanced.Sound() },
                { "CUO", new RazorEnhanced.CUO() },
                { "AutoLoot", new RazorEnhanced.AutoLoot() },
                { "Scavenger", new RazorEnhanced.Scavenger() },
                { "SellAgent", new RazorEnhanced.SellAgent() },
                { "BuyAgent", new RazorEnhanced.BuyAgent() },
                { "Organizer", new RazorEnhanced.Organizer() },
                { "Dress", new RazorEnhanced.Dress() },
                { "Friend", new RazorEnhanced.Friend() },
                { "Restock", new RazorEnhanced.Restock() },
                { "BandageHeal", new RazorEnhanced.BandageHeal() },
                { "PathFinding", new RazorEnhanced.PathFinding() },
                { "DPSMeter", new RazorEnhanced.DPSMeter() },
                { "Timer", new RazorEnhanced.Timer() },
                { "Trade", new RazorEnhanced.Trade() },
                { "Vendor", new RazorEnhanced.Vendor() },
                { "PacketLogger", new RazorEnhanced.PacketLogger() },
                { "Events", new RazorEnhanced.Events() }
            };

            //Setup builtin modules and scope
            foreach (var module in Modules) {
                Runtime.Globals.SetVariable(module.Key, module.Value);
                Engine.GetBuiltinModule().SetVariable(module.Key, module.Value);
            }
            
        }
        
        ~PythonEngine() { 

        }

        public void SetTrace(TracebackDelegate tracebackDelegate)
        {
            Engine.SetTrace(tracebackDelegate);
        }

        public void SetStdout(Action<string> stdoutWriter)
        {
            PythonWriter outputWriter = new PythonWriter(stdoutWriter);
            Engine.Runtime.IO.SetOutput(outputWriter, Encoding.ASCII);
        }

        public void SetStderr(Action<string> stderrWriter)
        {
            PythonWriter errorWriter = new PythonWriter(stderrWriter);
            Engine.Runtime.IO.SetErrorOutput(errorWriter, Encoding.ASCII);
        }

        public dynamic Call(PythonFunction function, params object[] args) {
            try { 
                return Engine.Operations.Invoke(function, args);
            } catch {
                return null;
            }
        }

        /*
        public void Register(PythonFunction function, OnLogPacketDataCallBack callback)
        {
            Thread thread = Thread.CurrentThread;
            if (!m_Callbacks.ContainsKey(thread))
            {
                m_Callbacks[thread] = new Dictionary<int, HashSet<OnLogPacketDataCallBack>>();
            }
            if (!m_Callbacks[thread].ContainsKey(packetID))
            {
                m_Callbacks[thread][packetID] = new HashSet<OnLogPacketDataCallBack>();
            }
            m_Callbacks[thread][packetID].Add(callback);
        }
        */



        public bool Load(String text, String path = null)
        {
            Source = null;
            Compiled = null;
            Scope = null;
            if (Engine == null) { return false; }

            //CACHE (should we?)
            Text = text;
            FilePath = path;

            //LOAD code as text
            if (text == null) { return false; } // no text
            Source = Engine.CreateScriptSourceFromString(text, path);
            if (Source == null) { return false; }

            //COMPILE with OPTIONS
            //PythonCompilerOptions in order to initialize Python modules correctly, without it the Python env is half broken
            Scope = Engine.CreateScope();

            CompilerOptions = (PythonCompilerOptions)Engine.GetCompilerOptions(Scope);
            CompilerOptions.ModuleName = "__main__";
            CompilerOptions.Module |= ModuleOptions.Initialize;
            CompilerOptions.Optimized = true;
            
            Compiled = Source.Compile(CompilerOptions);
            if (Compiled == null) { return false; }
            
            return true;
        }
        
        
        public bool Execute() {
            //EXECUTE
            if (Scope == null) { return false; }
            else if (Compiled == null) { return false; }
            else if (Source == null)   { return false; }

            Journal journal = Modules["Journal"] as Journal;
            journal.Active = true;
            Compiled.Execute(Scope);
            journal.Active = false;

            return true;

            //DONT USE
            //Execute directly, unless you are not planning to import external modules.
            //Source.Execute(m_Scope);
        }
    }


    // --------------------------------------------------------- PythonWriter --------------------------------------------------------
    public class PythonWriter : MemoryStream
    {
        internal Action<string> m_action;

        public PythonWriter(Action<string> stdoutWriter)
            : base()
        {
            m_action = stdoutWriter;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (m_action != null)
                m_action(System.Text.Encoding.ASCII.GetString(buffer));
            base.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            if (m_action != null)
                m_action(System.Text.Encoding.ASCII.GetString(buffer));
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            if (m_action != null)
                m_action(value.ToString());
            base.WriteByte(value);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return base.BeginWrite(buffer, offset, count, callback, state);
        }
    }
}
