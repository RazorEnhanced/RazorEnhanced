using Assistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RazorEnhanced
{
	public class Gumps
	{
		public static void CloseGump(uint gumpid)
		{
			if (gumpid == 0)
				ClientCommunication.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));
			else
				ClientCommunication.SendToClientWait(new CloseGump(gumpid));

			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpI = 0;
		}

		public static void ResetGump()
		{
			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpI = 0;
		}

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
			if (gumpid == 0)
			{
				while (World.Player.HasGump != true && subdelay > 0)
				{
					Thread.Sleep(2);
					subdelay -= 2;
				}
			}
			else
			{
				while (World.Player.HasGump != true && World.Player.CurrentGumpI != gumpid && subdelay > 0)
				{
					Thread.Sleep(2);
					subdelay -= 2;
				}
			}

			
		}

		public static void SendAction(uint gumpid, int buttonid)
		{
			int[] nullswitch = new int[0];
			GumpTextEntry[] nullentries = new GumpTextEntry[0];

			ClientCommunication.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));

			if (gumpid == 0)
				ClientCommunication.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, nullswitch, nullentries));
			else
				ClientCommunication.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, nullswitch, nullentries));

			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpI = 0;
		}

		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> switchs)
		{
			GumpTextEntry[] entries = new GumpTextEntry[0];

			ClientCommunication.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));

			if (gumpid == 0)
				ClientCommunication.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, switchs.ToArray(), entries));
			else
				ClientCommunication.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, switchs.ToArray(), entries));

			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
		}

		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> switchs, List<int> entryID, List<string> entryS)
		{
			if (entryID.Count == entryS.Count)
			{
				int i = 0;
				GumpTextEntry[] entries = new GumpTextEntry[entryID.Count];
				GumpTextEntry entrie = null;
				foreach (int entry in entryID)
				{
					entrie.EntryID = (ushort)entry;
					entrie.Text = entryS[i];
					entries[i] = entrie;
                }

				ClientCommunication.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));

				if (gumpid == 0)
					ClientCommunication.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, switchs.ToArray(), entries));
				else
					ClientCommunication.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, switchs.ToArray(), entries));

				World.Player.HasGump = false;
				World.Player.CurrentGumpStrings.Clear();
			}
			else
			{
				Scripts.SendMessageScriptError("Script Error: SendAdvancedAction: entryID and entryS lenght not match");
			}
		}

		public static string LastGumpGetLine(int line)
		{
			try
			{
				if (line > World.Player.CurrentGumpStrings.Count)
				{
					Scripts.SendMessageScriptError("Script Error: LastGumpGetLine: Text line (" + line + ") not exist");
					return "";
				}
				else
				{
					return World.Player.CurrentGumpStrings[line];
				}
			}
			catch
			{
				return "";
			}
		}

		public static List<string> LastGumpGetLineList()
		{
			return World.Player.CurrentGumpStrings;
		}

		public static bool LastGumpTextExist(string text)
		{
			try
			{
				return World.Player.CurrentGumpStrings.Any(stext => stext.Contains(text));
			}
			catch
			{
				return false;
			}
		}

		public static bool LastGumpTextExistByLine(int line, string text)
		{
			try
			{
				if (line > World.Player.CurrentGumpStrings.Count)
				{
					Scripts.SendMessageScriptError("Script Error: LastGumpTextExistByLine: Text line (" + line + ") not exist");
					return false;
				}
				else
				{
					return World.Player.CurrentGumpStrings[line].Contains(text);
				}
			}
			catch
			{
				return false;
			}
		}

		public static string LastGumpRawData()
		{
			try
			{
				return World.Player.CurrentGumpRawData;
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}