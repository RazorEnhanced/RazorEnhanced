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
		 		Assistant.Client.Instance.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));
			else
		 		Assistant.Client.Instance.SendToClientWait(new CloseGump(gumpid));

			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpTile.Clear();
			World.Player.CurrentGumpI = 0;
		}

		public static void ResetGump()
		{
			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpTile.Clear();
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

			if (gumpid == 0)
			{
		 		Assistant.Client.Instance.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));
		 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, nullswitch, nullentries));
			}
			else
			{
		 		Assistant.Client.Instance.SendToClientWait(new CloseGump(gumpid));
		 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, nullswitch, nullentries));
			}

			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpTile.Clear();
			World.Player.CurrentGumpI = 0;
		}

		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> switchs)
		{
			GumpTextEntry[] entries = new GumpTextEntry[0];

			if (gumpid == 0)
			{
		 		Assistant.Client.Instance.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));
		 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, switchs.ToArray(), entries));
			}
			else
			{
		 		Assistant.Client.Instance.SendToClientWait(new CloseGump(gumpid));
		 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, switchs.ToArray(), entries));
			}

			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpTile.Clear();
		}
		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> entryID, List<string> entryS)
		{
			List<int> switchs = new List<int>();
			SendAdvancedAction(gumpid, buttonid, switchs, entryID, entryS);
		}

		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> switchs, List<int> entryID, List<string> entryS)
		{
			if (entryID.Count == entryS.Count)
			{
				int i = 0;
				GumpTextEntry[] entries = new GumpTextEntry[entryID.Count];

				foreach (int entry in entryID)
				{
					GumpTextEntry entrie = new GumpTextEntry(0, string.Empty);
					entrie.EntryID = (ushort)entry;
					entrie.Text = entryS[i];
					entries[i] = entrie;
                    i++;
				}

				if (gumpid == 0)
				{
			 		Assistant.Client.Instance.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));
			 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, switchs.ToArray(), entries));
				}
				else
				{
			 		Assistant.Client.Instance.SendToClientWait(new CloseGump(gumpid));
			 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, switchs.ToArray(), entries));
				}

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

		public static List<int> LastGumpTile()
		{
			try
			{
				return World.Player.CurrentGumpTile;
			}
			catch
			{
				return new List<int>();
			}
		}
	}
}
