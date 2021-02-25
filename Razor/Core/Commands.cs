using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;
using RazorEnhanced.UI;
using Assistant.UI;
using Newtonsoft.Json;

namespace Assistant
{
	internal class Commands
	{
		public static void Initialize()
		{
			Command.Register("where", new CommandCallback(Where));
			Command.Register("ping", new CommandCallback(Ping));
            Command.Register("pping", new CommandCallback(Packet_Ping));
            Command.Register("reducecpu", new CommandCallback(ReNice));
			Command.Register("renice", new CommandCallback(ReNice));
			Command.Register("help", new CommandCallback(Command.ListCommands));
			Command.Register("listcommand", new CommandCallback(Command.ListCommands));
			Command.Register("echo", new CommandCallback(Echo));
			Command.Register("getserial", new CommandCallback(GetSerial));
			Command.Register("inspect", new CommandCallback(GetInfo));
			Command.Register("inspectgumps", new CommandCallback(InspectGumps));
			Command.Register("inspectalias", new CommandCallback(InspectAlias));
			Command.Register("playscript", new CommandCallback(PlayScript));
			Command.Register("hideitem", new CommandCallback(HideItem));
			Command.Register("drop", new CommandCallback(DropItem));
		}

		private static void GetSerial(string[] param)
		{
		 	Assistant.Client.Instance.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a player or item to get their serial number."));
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(GetSerialTarget_Callback));
		}

		private static void GetSerialTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
		 	Assistant.Client.Instance.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Serial: 0x" + serial.Value.ToString("X8")));
		}

		private static void InspectAlias(string[] param)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedObjectInspector af)
				{
					af.Focus();
					return;
				}
			}
			new EnhancedObjectInspector().Show();
		}
		private static void GetInfo(string[] param)
		{
		 	Assistant.Client.Instance.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a player or item to open object inspect."));
			Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback( GetInfoTarget_Callback));
		}

		internal static void GetInfoTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			if (loc) // Target on ground or static
			{
				Engine.MainWindow.SafeAction(s =>
				{
					RazorEnhanced.UI.EnhancedStaticInspector inspector = new RazorEnhanced.UI.EnhancedStaticInspector(pt);
					inspector.TopMost = true;
					inspector.Show();
				});
			}
			else  // Target item or mobile
			{
				Assistant.Item assistantItem = Assistant.World.FindItem(serial);
				if (assistantItem != null && assistantItem.Serial.IsItem)
				{
					Engine.MainWindow.SafeAction(s =>
					{
						RazorEnhanced.UI.EnhancedItemInspector inspector = new RazorEnhanced.UI.EnhancedItemInspector(assistantItem);
						inspector.TopMost = true;
						inspector.Show();
					});
				}
				else
				{
					Assistant.Mobile assistantMobile = Assistant.World.FindMobile(serial);
					if (assistantMobile != null && assistantMobile.Serial.IsMobile)
					{
						Assistant.Engine.MainWindow.SafeAction(s => {
							RazorEnhanced.UI.EnhancedMobileInspector inspector = new RazorEnhanced.UI.EnhancedMobileInspector(assistantMobile);
							inspector.TopMost = true;
							inspector.Show();
						});
					}
				}
			}
		}

		private static void DropItem(string[] param)
		{
		 	Assistant.Client.Instance.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target item to Drop at feet."));
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(RazorEnhanced.HotKey.dropitemTarget_Callback));
		}

		private static void HideItem(string[] param)
		{
		 	Assistant.Client.Instance.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a item to hide."));
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(HideItem_Callback));
		}

		internal static void HideItem_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			RazorEnhanced.Items.Hide(serial);
		}

		private static void Echo(string[] param)
		{
			StringBuilder sb = new StringBuilder("Note To Self: ");
			foreach (string t in param)
				sb.Append(t);
		 	Assistant.Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3, Language.CliLocName, "System", sb.ToString()));
		}

		private static void ReNice(string[] param)
		{
			try
			{
				System.Diagnostics.ProcessPriorityClass prio;
				if (param.Length < 1)
					prio = System.Diagnostics.ProcessPriorityClass.BelowNormal;
				else
					prio = (System.Diagnostics.ProcessPriorityClass)Enum.Parse(typeof(System.Diagnostics.ProcessPriorityClass), param[0], true);

			 	Assistant.Client.Instance.ClientProcess.PriorityClass = prio;
				World.Player.SendMessage(MsgLevel.Force, LocString.PrioSet, prio);
			}
			catch (Exception e)
			{
				World.Player.SendMessage(MsgLevel.Force, LocString.PrioSet, String.Format("Error: {0}", e.Message));
			}
		}
		private static void InspectGumps(string[] param)
		{
			EnhancedScriptEditor.InspectGumps();
		}

		private static void Where(string[] param)
		{
			string mapStr;
			switch (World.Player.Map)
			{
				case 0:
					mapStr = "Felucca";
					break;

				case 1:
					mapStr = "Trammel";
					break;

				case 2:
					mapStr = "Ilshenar";
					break;

				case 3:
					mapStr = "Malas";
					break;

				case 4:
					mapStr = "Tokuno";
					break;

				case 5:
					mapStr = "Ter Mur";
					break;

				case 0x7F:
					mapStr = "Internal";
					break;

				default:
					mapStr = String.Format("Unknown (#{0})", World.Player.Map);
					break;
			}
			World.Player.SendMessage(MsgLevel.Force, LocString.CurLoc, World.Player.Position, mapStr);
		}

        internal static void Packet_Ping(string[] param)
        {
            int num_packets = 5;
            if (param.Length > 0)
            {
                try
                {
                    num_packets = int.Parse(param[0]);
                }
                catch (Exception e)
                {
                    num_packets = 5;
                }
            }
            Assistant.Ping.StartPing(num_packets);
        }

        internal static void Ping(string[] param)
		{
            Packet_Ping(param);
			new Thread(() =>
			{
				int max = int.MinValue;
                int min = int.MaxValue;
				int total = 0;

				System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
				PingOptions options = new PingOptions();
				options.DontFragment = true;

				// Create a buffer of 32 bytes of data to be transmitted.
				string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				int timeout = 1000;
				RazorEnhanced.Misc.SendMessage("Address: " + Assistant.Client.Instance.LastConnection.ToString(), 33, false);
                const int NumPings = 5;
				for (int i = 0; i < NumPings; i++)
				{
					PingReply reply = pingSender.Send(Assistant.Client.Instance.LastConnection, timeout, buffer, options);
					if (reply.Status == IPStatus.Success)
					{
						total += (int)reply.RoundtripTime;
                        RazorEnhanced.Misc.SendMessage("- RoundTrip time: " + reply.RoundtripTime +"ms", 33, false);
						if (reply.RoundtripTime > max)
							max = (int)reply.RoundtripTime;
						if (reply.RoundtripTime < min)
							min = (int)reply.RoundtripTime;
					}
					else
					if (reply.Status == IPStatus.Success)
					{
						RazorEnhanced.Misc.SendMessage("Network Ping Failed", 33, false);
					}
				}
				if (max == int.MinValue)
					RazorEnhanced.Misc.SendMessage("Network Server not respond to ping request", 33, false);
				else
					RazorEnhanced.Misc.SendMessage("Max: " + max + "ms - Avg: " + (total / NumPings).ToString() + "ms - Min: " + min + "ms", 33, false);


			}).Start();
		}

		private static void PlayScript(string[] param)
		{
			if (param == null || param.Length == 0)
				return;

			string scriptname = String.Empty;
			scriptname = param[0];

			if (param.Length > 1)
			{
				for (int i = 1; i < param.Length; i++)
				{
					scriptname = scriptname + " " + param[i];
				}
			}

			RazorEnhanced.Scripts.EnhancedScript script = RazorEnhanced.Scripts.Search(scriptname);
			if (script != null)
			{
				if (script.Run)
					script.Stop();
				else
					script.Run = true;
			}
			else
				RazorEnhanced.Misc.SendMessage("PlayScript: Script not exist",33, false);
		}
	}

	internal delegate void CommandCallback(string[] param);

	internal class Command
	{
		private static Dictionary<string, CommandCallback> m_List;

		static Command()
		{
			m_List = new Dictionary<string, CommandCallback>();
			PacketHandler.RegisterClientToServerFilter(0xAD, new PacketFilterCallback(OnSpeech));
		}

		internal static void ListCommands(string[] param)
		{
			RazorEnhanced.Misc.SendMessage("Command List:", 33, false);
			string suffix = "<";
			if (Client.IsOSI)
				suffix = "-";

			foreach (string cmd in m_List.Keys)
			{
				RazorEnhanced.Misc.SendMessage(suffix + cmd, 33, false);
			}
		}

		internal static void Register(string cmd, CommandCallback callback)
		{
			if (!m_List.ContainsKey(cmd))
				m_List.Add(cmd, callback);
		}

		internal static CommandCallback FindCommand(string cmd)
		{
			CommandCallback callback;
			m_List.TryGetValue(cmd, out callback);
			return callback;
		}

		internal static void RemoveCommand(string cmd)
		{
			m_List.Remove(cmd);
		}

		internal static Dictionary<string, CommandCallback> List { get { return m_List; } }

		internal static void OnSpeech(Packet pvSrc, PacketHandlerEventArgs args)
		{
			MessageType type = (MessageType)pvSrc.ReadByte();
			ushort hue = pvSrc.ReadUInt16();
			ushort font = pvSrc.ReadUInt16();
			string lang = pvSrc.ReadString(4);
			string text = "";
			List<ushort> keys = null;
			long txtOffset = 0;

			World.Player.SpeechHue = hue;

			if ((type & MessageType.Encoded) != 0)
			{
				int value = pvSrc.ReadInt16();
				int count = (value & 0xFFF0) >> 4;
				keys = new List<ushort> { (ushort)value };

				for (int i = 0; i < count; ++i)
				{
					if ((i & 1) == 0)
					{
						keys.Add(pvSrc.ReadByte());
					}
					else
					{
						keys.Add(pvSrc.ReadByte());
						keys.Add(pvSrc.ReadByte());
					}
				}

				txtOffset = pvSrc.Position;
				text = pvSrc.ReadUTF8StringSafe();
				type &= ~MessageType.Encoded;
			}
			else
			{
				txtOffset = pvSrc.Position;
				text = pvSrc.ReadUnicodeStringSafe();
			}

			if (RazorEnhanced.ScriptRecorder.OnRecord)
			{
				RazorEnhanced.ScriptRecorder.Record_UnicodeSpeech(type, text, hue);
			}

			text = text.Trim();

			if (text.Length <= 1)
				return;

			if (Client.IsOSI)
			{
				if (text[0] == '-' && text[1] != '-')
				{
					args.Block = ExecCommand(text);
				}
				else if (text[0] == '-' && text[1] == '-')
				{
					args.Block = true;
					Assistant.UOAssist.PostTextSend(text.Substring(2));
				}
			}
			else
			{
				if (text[0] == '<')
				{
					args.Block = ExecCommand(text);
				}
			}
		}

		private static bool ExecCommand(string text)
		{
			text = text.Substring(1).ToLower();
			string[] split = text.Split(' ', '\t');
			if (m_List.ContainsKey(split[0]))
			{
				CommandCallback call = (CommandCallback)m_List[split[0]];
				if (call != null)
				{
					string[] param = new String[split.Length - 1];
					for (int i = 0; i < param.Length; i++)
						param[i] = split[i + 1];
					call(param);

					return true;
				}
			}
			return false;
		}
	}
}
