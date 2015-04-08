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
        public static void SendAction(int gumpid, int buttonid) 
        {
            int[] nullswitch = new int[0];
            GumpTextEntry[] nullentries = new GumpTextEntry[0];
            ClientCommunication.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            ClientCommunication.SendToServer(new GumpResponse(World.Player.CurrentGumpS, (uint)gumpid, buttonid, nullswitch, nullentries));
            World.Player.HasGump = false;
            World.Player.CurrentGumpStrings.Clear();
        }
        public static void SendAdvancedAction(int gumpid, int buttonid, int[] switchs, GumpTextEntry[] entries)
        {
            ClientCommunication.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            ClientCommunication.SendToServer(new GumpResponse(World.Player.CurrentGumpS, (uint)gumpid, buttonid, switchs, entries));
            World.Player.HasGump = false;
            World.Player.CurrentGumpStrings.Clear();
        }
	}
}
