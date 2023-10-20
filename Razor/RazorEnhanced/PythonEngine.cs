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
    class PythonEngine
    {
        public Dictionary<string, object> Modules;
        public ScriptEngine Engine { get;  }
        public ScriptScope Scope { get; set; }
        public String Text { get; set; }
        public String FilePath { get; set; }
        public ScriptSource Source { get; set; }
        public CompiledCode Compiled { get; set; }
        public PythonCompilerOptions CompilerOptions { get; set; }

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

        public PythonEngine(Action<string> stdoutWriter) {
            var runtime = IronPython.Hosting.Python.CreateRuntime();
            if (stdoutWriter != null)
            {
                PythonWriter outputWriter = new PythonWriter(stdoutWriter);
                runtime.IO.SetErrorOutput(outputWriter, Encoding.ASCII);
                runtime.IO.SetOutput(outputWriter, Encoding.ASCII);
            }
            Engine = IronPython.Hosting.Python.GetEngine(runtime);



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
            Modules = new Dictionary<string, object>();
            Modules.Add("Misc", new RazorEnhanced.Misc());

            Modules.Add("Items", new RazorEnhanced.Items());
            Modules.Add("Mobiles", new RazorEnhanced.Mobiles());
            Modules.Add("Player", new RazorEnhanced.Player());
            Modules.Add("Spells", new RazorEnhanced.Spells());
            Modules.Add("Gumps", new RazorEnhanced.Gumps());
            Modules.Add("Journal", new RazorEnhanced.Journal());
            Modules.Add("Target", new RazorEnhanced.Target());
            Modules.Add("Statics", new RazorEnhanced.Statics());
            Modules.Add("Sound", new RazorEnhanced.Sound());
            Modules.Add("CUO", new RazorEnhanced.CUO());
            Modules.Add("AutoLoot", new RazorEnhanced.AutoLoot());
            Modules.Add("Scavenger", new RazorEnhanced.Scavenger());
            Modules.Add("SellAgent", new RazorEnhanced.SellAgent());
            Modules.Add("BuyAgent", new RazorEnhanced.BuyAgent());
            Modules.Add("Organizer", new RazorEnhanced.Organizer());
            Modules.Add("Dress", new RazorEnhanced.Dress());
            Modules.Add("Friend", new RazorEnhanced.Friend());
            Modules.Add("Restock", new RazorEnhanced.Restock());
            Modules.Add("BandageHeal", new RazorEnhanced.BandageHeal());
            Modules.Add("PathFinding", new RazorEnhanced.PathFinding());
            Modules.Add("DPSMeter", new RazorEnhanced.DPSMeter());
            Modules.Add("Timer", new RazorEnhanced.Timer());
            Modules.Add("Trade", new RazorEnhanced.Trade());
            Modules.Add("Vendor", new RazorEnhanced.Vendor());
            Modules.Add("PacketLogger", new RazorEnhanced.PacketLogger());

            //Setup builtin modules and scope
            foreach (var module in Modules) {
                Engine.Runtime.Globals.SetVariable(module.Key, module.Value);
                Engine.GetBuiltinModule().SetVariable(module.Key, module.Value);
            }
            Scope = Engine.CreateScope();

            CompilerOptions = (PythonCompilerOptions)Engine.GetCompilerOptions(Scope);
            CompilerOptions.ModuleName = "__main__";
            CompilerOptions.Module |= ModuleOptions.Initialize;
        }

        public void Execute(String text, String path=null)
        {
            if (Engine == null) return;

            //CACHE (should we?)
            Text = text;
            FilePath = path;

            //LOAD code as text
            if (text == null) return; // no text
            Source = Engine.CreateScriptSourceFromString(text, path);
            if (Source == null) return;

            //COMPILE with OPTIONS
            //PythonCompilerOptions in order to initialize Python modules correctly, without it the Python env is half broken
            Compiled = Source.Compile(CompilerOptions);

            //EXECUTE
            Journal journal = Modules["Journal"] as Journal;
            journal.Active = true;
            Scope = Engine.CreateScope();
            Compiled.Execute(Scope);
            journal.Active = false;

            //DONT USE
            //Execute directly, unless you are not planning to import external modules.
            //Source.Execute(m_Scope);
        }
    }
}
