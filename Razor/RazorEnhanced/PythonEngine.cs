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
                Runtime.Globals.SetVariable(module.Key, module.Value);
                Engine.GetBuiltinModule().SetVariable(module.Key, module.Value);
            }
            
        }

        }

        public void SetTrace(TracebackDelegate tracebackDelegate)
        {
            Engine.SetTrace(tracebackDelegate);
        }

        public void SetStdout(Action<string> stdoutWriter)
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
