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
            Modules.Add("Vendor", new RazorEnhanced.Vendor());

            //Setup builtin modules and scope
            foreach (var module in Modules) {
                engine.Runtime.Globals.SetVariable(module.Key, module.Value);
                engine.GetBuiltinModule().SetVariable(module.Key, module.Value);
            }
            scope = engine.CreateScope();

        }
    }
}
