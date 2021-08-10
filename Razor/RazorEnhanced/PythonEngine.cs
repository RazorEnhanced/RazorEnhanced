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
        public ScriptEngine engine { get;  }
		public ScriptScope scope { get; }


        public PythonEngine() {
			engine = Python.CreateEngine();


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

            engine.SetSearchPaths(paths);

            // Add also defult IronPython 3.4 installlation folder, if present
            if (System.IO.Directory.Exists(@"C:\Program Files\IronPython 3.4"))
            {
                paths.Add(@"C:\Program Files\IronPython 3.4");
                paths.Add(@"C:\Program Files\IronPython 3.4\Lib"); 
            	paths.Add(@"C:\Program Files\IronPython 3.4\DLLs");
                paths.Add(@"C:\Program Files\IronPython 3.4\Scripts");
            }

			engine.GetBuiltinModule().SetVariable("Misc", new RazorEnhanced.Misc());
			engine.GetBuiltinModule().SetVariable("Items", new RazorEnhanced.Items());
			engine.GetBuiltinModule().SetVariable("Mobiles", new RazorEnhanced.Mobiles());
			engine.GetBuiltinModule().SetVariable("Player", new RazorEnhanced.Player());
			engine.GetBuiltinModule().SetVariable("Spells", new RazorEnhanced.Spells());
			engine.GetBuiltinModule().SetVariable("Gumps", new RazorEnhanced.Gumps());
			engine.GetBuiltinModule().SetVariable("Journal", new RazorEnhanced.Journal());
			engine.GetBuiltinModule().SetVariable("Target", new RazorEnhanced.Target());
			engine.GetBuiltinModule().SetVariable("Statics", new RazorEnhanced.Statics());

			engine.GetBuiltinModule().SetVariable("AutoLoot", new RazorEnhanced.AutoLoot());
			engine.GetBuiltinModule().SetVariable("Scavenger", new RazorEnhanced.Scavenger());
			engine.GetBuiltinModule().SetVariable("SellAgent", new RazorEnhanced.SellAgent());
			engine.GetBuiltinModule().SetVariable("BuyAgent", new RazorEnhanced.BuyAgent());
			engine.GetBuiltinModule().SetVariable("Organizer", new RazorEnhanced.Organizer());
			engine.GetBuiltinModule().SetVariable("Dress", new RazorEnhanced.Dress());
			engine.GetBuiltinModule().SetVariable("Friend", new RazorEnhanced.Friend());
			engine.GetBuiltinModule().SetVariable("Restock", new RazorEnhanced.Restock());
			engine.GetBuiltinModule().SetVariable("BandageHeal", new RazorEnhanced.BandageHeal());
			engine.GetBuiltinModule().SetVariable("PathFinding", new RazorEnhanced.PathFinding());
			engine.GetBuiltinModule().SetVariable("DPSMeter", new RazorEnhanced.DPSMeter());
			engine.GetBuiltinModule().SetVariable("Timer", new RazorEnhanced.Timer());
			engine.GetBuiltinModule().SetVariable("Vendor", new RazorEnhanced.Vendor());

			//Setup main script symbols, automatically imported for convenience
			scope = engine.CreateScope();
		}

		/*Dalamar: BEGIN*/
		public void Execute(String text) {
			if (text == null) return;

			ScriptSource m_Source = this.engine.CreateScriptSourceFromString(text);
			if (m_Source == null) return;

			//EXECUTE
			//USE: PythonCompilerOptions in order to initialize Python modules correctly, without it the Python env is half broken
			PythonCompilerOptions pco = (PythonCompilerOptions) this.engine.GetCompilerOptions(this.scope);
			pco.ModuleName = "__main__";
			pco.Module |= ModuleOptions.Initialize;
			CompiledCode compiled = m_Source.Compile(pco);
			compiled.Execute(this.scope);

			//DONT USE: Execute directly, unless you are not planning to import external modules.
			//m_Source.Execute(m_Scope);

		}
		/*Dalamar: END*/
	}
}
