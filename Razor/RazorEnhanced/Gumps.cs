using Assistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RazorEnhanced
{
    /// <summary>
    /// The Gumps class is used to read and interact with in-game gumps, via scripting.
    /// 
    /// NOTE
    /// ----
    /// During development of scripts that involves interecting with Gumps, is often needed to know gumpids and buttonids.
    /// For this purpose, can be particularly usefull to use *Inspect Gumps* and *Record*, top right, in the internal RE script editor.
    /// </summary>
    
	public class Gumps
	{
        /// <summary>
        /// Close a specific Gump.
        /// </summary>
        /// <param name="gumpid">ID of the gump</param>
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

        /// <summary>
        /// Clean current status of Gumps.
        /// </summary>
		public static void ResetGump()
		{
			World.Player.HasGump = false;
			World.Player.CurrentGumpStrings.Clear();
			World.Player.CurrentGumpTile.Clear();
			World.Player.CurrentGumpI = 0;
		}


        /// <summary>
        /// Return the ID of most recent, still open Gump.
        /// </summary>
        /// <returns>ID of gump.</returns>
		public static uint CurrentGump()
		{
			return World.Player.CurrentGumpI;
		}

        /// <summary>
        /// Get status if have a gump open or not.
        /// </summary>
        /// <returns>True: There is a Gump open - False: otherwise.</returns>
		public static bool HasGump()
		{
			return World.Player.HasGump;
		}

        /// <summary>
        /// Waits for a specific Gump to appear, for a maximum amount of time. If gumpid is 0 it will match any Gump.
        /// </summary>
        /// <param name="gumpid">ID of the gump. (0: any)</param>
        /// <param name="delay">Maximum wait, in milliseconds.</param>
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

        /// <summary>
        /// Send a Gump response by gumpid and buttonid.
        /// </summary>
        /// <param name="gumpid">ID of the gump.</param>
        /// <param name="buttonid">ID of the Button to press.</param>
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

        //AutoDoc concatenates description coming from Overloaded methods
        /// <summary>
        /// This method can also be used only Switches, without Text fileds.
        /// </summary>
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

        //AutoDoc concatenates description coming from Overloaded methods
        /// <summary>
        /// This method can also be used only Text fileds, without Switches.
        /// </summary>
		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> textlist_id, List<string> textlist_str)
		{
			List<int> switchs = new List<int>();
			SendAdvancedAction(gumpid, buttonid, switchs, textlist_id, textlist_str);
		}

        /// <summary>
        /// Send a Gump response, with gumpid and buttonid and advanced switch in gumps. 
        /// This function is intended for more complex Gumps, with not only Buttons, but also Switches, CheckBoxes and Text fileds.
        /// </summary>
        /// <param name="gumpid">ID of the gump.</param>
        /// <param name="buttonid">ID of the Button.</param>
        /// <param name="switchlist_id">List of ID of ON/Active switches. (empty: all Switches OFF)</param>
        /// <param name="textlist_id">List of ID of Text fileds. (empty: all text fileds empty )</param>
        /// <param name="textlist_str">List of the contents of the Text fields, provided in the same order as textlist_id.</param>
		public static void SendAdvancedAction(uint gumpid, int buttonid, List<int> switchlist_id, List<int> textlist_id, List<string> textlist_str)
		{
			if (textlist_id.Count == textlist_str.Count)
			{
				int i = 0;
				GumpTextEntry[] entries = new GumpTextEntry[textlist_id.Count];

				foreach (int entry in textlist_id)
				{
					GumpTextEntry entrie = new GumpTextEntry(0, string.Empty);
					entrie.EntryID = (ushort)entry;
					entrie.Text = textlist_str[i];
					entries[i] = entrie;
                    i++;
				}

				if (gumpid == 0)
				{
			 		Assistant.Client.Instance.SendToClientWait(new CloseGump(World.Player.CurrentGumpI));
			 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, buttonid, switchlist_id.ToArray(), entries));
				}
				else
				{
			 		Assistant.Client.Instance.SendToClientWait(new CloseGump(gumpid));
			 		Assistant.Client.Instance.SendToServerWait(new GumpResponse(World.Player.CurrentGumpS, gumpid, buttonid, switchlist_id.ToArray(), entries));
				}

				World.Player.HasGump = false;
				World.Player.CurrentGumpStrings.Clear();
			}
			else
			{
				Scripts.SendMessageScriptError("Script Error: SendAdvancedAction: entryID and entryS lenght not match");
			}
		}

        /// <summary>
        /// Get a specific line from the most recent and still open Gump. Filter by line number.
        /// </summary>
        /// <param name="line_num">Number of the line.</param>
        /// <returns>Text content of the line. (empty: line not found)</returns>
		public static string LastGumpGetLine(int line_num)
		{
			try
			{
				if (line_num > World.Player.CurrentGumpStrings.Count)
				{
					Scripts.SendMessageScriptError("Script Error: LastGumpGetLine: Text line (" + line_num + ") not exist");
					return "";
				}
				else
				{
					return World.Player.CurrentGumpStrings[line_num];
				}
			}
			catch
			{
				return "";
			}
		}

        /// <summary>
        /// Get all text from the most recent and still open Gump.
        /// </summary>
        /// <returns>Text of the gump.</returns>
		public static List<string> LastGumpGetLineList()
		{
			return World.Player.CurrentGumpStrings;
		}

        /// <summary>
        /// Search for text inside the most recent and still open Gump.
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <returns>True: Text found in active Gump - False: otherwise</returns>
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

        /// <summary>
        /// Search for text, in a spacific line of the most recent and still open Gump.
        /// </summary>
        /// <param name="line_num">Number of the line.</param>
        /// <param name="text">Text to search.</param>
        /// <returns></returns>
		public static bool LastGumpTextExistByLine(int line_num, string text)
		{
			try
			{
				if (line_num > World.Player.CurrentGumpStrings.Count)
				{
					Scripts.SendMessageScriptError("Script Error: LastGumpTextExistByLine: Text line (" + line_num + ") not exist");
					return false;
				}
				else
				{
					return World.Player.CurrentGumpStrings[line_num].Contains(text);
				}
			}
			catch
			{
				return false;
			}
		}

        /// <summary>
        /// Get the Raw Data of the most recent and still open Gump.
        /// </summary>
        /// <returns>Raw Data of the gump.</returns>
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


        /// <summary>
        /// Get the Raw Text of the most recent and still open Gump.
        /// </summary>
        /// <returns>List of Raw Text.</returns>
		public static List<string> LastGumpRawText()
		{
			try
			{
				return new List<string>(World.Player.CurrentGumpRawText);
			}
			catch
			{
				return new List<string>();
			}
		}

        /// <summary>
        /// Get the list of Gump Tile (! this documentation is a stub !) 
        /// </summary>
        /// <returns>List of Gump Tile.</returns>
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
