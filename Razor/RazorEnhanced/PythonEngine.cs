using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronPython.Runtime;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;

namespace RazorEnhanced
{
    class PythonEngine
    {
        public ScriptEngine engine { get;  }
		public ScriptScope scope { get; }


        public PythonEngine() {
			engine = Python.CreateEngine();

            var paths = engine.GetSearchPaths();
            paths.Add(Misc.CurrentScriptDirectory());
			engine.SetSearchPaths(paths);

			engine.Runtime.Globals.SetVariable("Misc", new RazorEnhanced.Misc());
			engine.Runtime.Globals.SetVariable("Items", new RazorEnhanced.Items());
			engine.Runtime.Globals.SetVariable("Mobiles", new RazorEnhanced.Mobiles());
			engine.Runtime.Globals.SetVariable("Player", new RazorEnhanced.Player());
			engine.Runtime.Globals.SetVariable("Spells", new RazorEnhanced.Spells());
			engine.Runtime.Globals.SetVariable("Gumps", new RazorEnhanced.Gumps());
			engine.Runtime.Globals.SetVariable("Journal", new RazorEnhanced.Journal());
			engine.Runtime.Globals.SetVariable("Target", new RazorEnhanced.Target());
			engine.Runtime.Globals.SetVariable("Statics", new RazorEnhanced.Statics());

			engine.Runtime.Globals.SetVariable("AutoLoot", new RazorEnhanced.AutoLoot());
			engine.Runtime.Globals.SetVariable("Scavenger", new RazorEnhanced.Scavenger());
			engine.Runtime.Globals.SetVariable("SellAgent", new RazorEnhanced.SellAgent());
			engine.Runtime.Globals.SetVariable("BuyAgent", new RazorEnhanced.BuyAgent());
			engine.Runtime.Globals.SetVariable("Organizer", new RazorEnhanced.Organizer());
			engine.Runtime.Globals.SetVariable("Dress", new RazorEnhanced.Dress());
			engine.Runtime.Globals.SetVariable("Friend", new RazorEnhanced.Friend());
			engine.Runtime.Globals.SetVariable("Restock", new RazorEnhanced.Restock());
			engine.Runtime.Globals.SetVariable("BandageHeal", new RazorEnhanced.BandageHeal());
			engine.Runtime.Globals.SetVariable("PathFinding", new RazorEnhanced.PathFinding());
			engine.Runtime.Globals.SetVariable("DPSMeter", new RazorEnhanced.DPSMeter());
			engine.Runtime.Globals.SetVariable("Timer", new RazorEnhanced.Timer());
			engine.Runtime.Globals.SetVariable("Vendor", new RazorEnhanced.Vendor());


			//Setup main script symbols, automatically imported for convenience
			//scope = GetRazorScope(engine);
			scope = engine.Runtime.Globals;
		}

	}
}
