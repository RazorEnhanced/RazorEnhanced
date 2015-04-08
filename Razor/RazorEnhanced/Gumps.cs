using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using Assistant;

namespace RazorEnhanced
{
	public class Gumps 
	{
        public static int CurrentGump()
        {
            return (int)World.Player.CurrentGumpS;
        }
        public static void WaitForGump(int gumpid, int delay) // Delay in MS
        {
             int subdelay = delay;
             while (World.Player.CurrentGumpS !=gumpid || subdelay < 0)
                {
                    Thread.Sleep(2);
                    subdelay -= 2;
                }
             RazorEnhanced.Misc.SendMessage("OK");
        }
	}
}
