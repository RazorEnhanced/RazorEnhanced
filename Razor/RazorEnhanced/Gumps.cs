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
            int currentgump = 0;
            try
            {
                currentgump = Convert.ToInt32(World.Player.CurrentGumpI);
            }
            catch
            { }

            return currentgump;
        }
        public static bool HasGump()
        {
            return World.Player.HasGump;
        }

        public static void WaitForGump(int gumpid, int delay) // Delay in MS
        {
             int subdelay = delay;
             while (CurrentGump() != gumpid && subdelay > 0)
             {
                 Thread.Sleep(2);
                 subdelay -= 2;
             }
        }
	}
}
