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
        public static uint CurrentGump()
        {
            return World.Player.CurrentGumpI;
        }
        public static bool HasGump()
        {
            return World.Player.HasGump;
        }

        public static void WaitForGump(uint gumpid, int delay) // Delay in MS
        {
             int subdelay = delay;
             while (World.Player.HasGump != true && World.Player.CurrentGumpI != gumpid && subdelay > 0)
             {
                 Thread.Sleep(2);
                 subdelay -= 2;
             }
        }
        public static void SendAction(uint gumpid, int buttonid) 
        {
            int[] nullswitch = new int[0];
            GumpTextEntry[] nullentries = new GumpTextEntry[0];
            ClientCommunication.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            ClientCommunication.SendToServer(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, nullswitch, nullentries));
            World.Player.HasGump = false;
            World.Player.CurrentGumpStrings.Clear();
            World.Player.CurrentGumpI = 0;
        }
      /*  public static void SendAdvancedAction(uint gumpid, int buttonid, int[] switchs, GumpTextEntry[] entries)
        {
            ClientCommunication.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            ClientCommunication.SendToServer(new GumpResponse(World.Player.CurrentGumpS, (uint)gumpid, buttonid, switchs, entries));
            World.Player.HasGump = false;
            World.Player.CurrentGumpStrings.Clear();
        }*/
        public static string LastGumpGetLine(int line)
        {
            if (line > World.Player.CurrentGumpStrings.Count)
            {
                Misc.SendMessage("Script Error: LastGumpGetLine: Text line (" + line + ") not exist");
                return "";
            }
            else
            {
                return World.Player.CurrentGumpStrings[line];
            }
        }
        public static List<string> LastGumpGetLineList()
        {
            return World.Player.CurrentGumpStrings;
        }
        public static bool LastGumpTextExist(string text)
        {
            foreach (string stext in World.Player.CurrentGumpStrings)
                if (stext.Contains(text))
                    return true;
            return false;
        }

        public static bool LastGumpTextExistByLine(int line, string text)
        {
            if (line > World.Player.CurrentGumpStrings.Count)
            {
                Misc.SendMessage("Script Error: LastGumpTextExistByLine: Text line (" + line + ") not exist");
                return false;
            }
            else
            {
                if (World.Player.CurrentGumpStrings[line].Contains(text))
                    return true;
                else
                    return false;
            }
        }
	}
}
