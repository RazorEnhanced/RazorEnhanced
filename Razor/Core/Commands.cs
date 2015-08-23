using System;
using System.Text;
using System.Collections.Generic;

namespace Assistant
{
	internal class Commands
	{
		public static void Initialize()
		{
			//Command.Register("AddUseOnce", new CommandCallback(AddUseOnce));
			Command.Register("Time", new CommandCallback(Time));
			Command.Register("Where", new CommandCallback(Where));
			Command.Register("Ping", new CommandCallback(Ping));
			Command.Register("ReduceCPU", new CommandCallback(ReNice));
			Command.Register("ReNice", new CommandCallback(ReNice));
			Command.Register("Help", new CommandCallback(Command.ListCommands));
			Command.Register("Echo", new CommandCallback(Echo));
			Command.Register("GetSerial", new CommandCallback(GetSerial));
			Command.Register("RPVInfo", new CommandCallback(GetRPVInfo));

		}

		private static void GetRPVInfo(string[] param)
		{
			if (PacketPlayer.CurrentOpenedInfo == null || PacketPlayer.CurrentOpenedInfo == "")
				return;

			ClientCommunication.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Current PacketVideo File Information:"));
			ClientCommunication.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", PacketPlayer.CurrentOpenedInfo));
		}

		private static void GetSerial(string[] param)
		{
			if (PacketPlayer.Playing)
			{
				ClientCommunication.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a player to get their serial number."));
				ClientCommunication.ForceSendToClient(new Target(Targeting.LocalTargID, false));
			}
		}

		private static void Echo(string[] param)
		{
			StringBuilder sb = new StringBuilder("Note To Self: ");
			for (int i = 0; i < param.Length; i++)
				sb.Append(param[i]);
			ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3, Language.CliLocName, "System", sb.ToString()));
		}

	/*	private static void AddUseOnce(string[] param)
		{
			string use = Language.GetString(LocString.UseOnce);
			foreach (Agent agent in Agent.List)
			{
				if (agent.Name == use)
				{
					agent.OnButtonPress(1);
					break;
				}
			}
		}*/

		private static void ReNice(string[] param)
		{
			try
			{
				System.Diagnostics.ProcessPriorityClass prio;
				if (param.Length < 1)
					prio = System.Diagnostics.ProcessPriorityClass.BelowNormal;
				else
					prio = (System.Diagnostics.ProcessPriorityClass)Enum.Parse(typeof(System.Diagnostics.ProcessPriorityClass), param[0], true);

				ClientCommunication.ClientProcess.PriorityClass = prio;
				World.Player.SendMessage(MsgLevel.Force, LocString.PrioSet, prio);
			}
			catch (Exception e)
			{
				World.Player.SendMessage(MsgLevel.Force, LocString.PrioSet, String.Format("Error: {0}", e.Message));
			}
		}

		private static void Time(string[] param)
		{
			World.Player.SendMessage(MsgLevel.Force, LocString.CurTime, DateTime.Now.ToString("MM/dd/yy HH:mm:ss.f"));
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
				case 0x7F:
					mapStr = "Internal";
					break;
				default:
					mapStr = String.Format("Unknown (#{0})", World.Player.Map);
					break;
			}
			World.Player.SendMessage(MsgLevel.Force, LocString.CurLoc, World.Player.Position, mapStr);
#if DEBUG
			World.Player.SendMessage(MsgLevel.Debug, "Cal? {0} - CalcZ = {1} (Extern = {2})", ClientCommunication.IsCalibrated(), World.Player.CalcZ, PlayerData.ExternalZ);
#endif
		}

		private static void Ping(string[] param)
		{
			int count = 5;
			if (param.Length > 0)
				count = Utility.ToInt32(param[0], 5);
			Assistant.Ping.StartPing(count);
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
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (string cmd in m_List.Keys)
			{
				sb.Append(cmd);
				sb.Append(" ");
			}
			World.Player.SendMessage(MsgLevel.Force, LocString.CommandList);
			World.Player.SendMessage(MsgLevel.Force, sb.ToString());
		}

		internal static void Register(string cmd, CommandCallback callback)
		{
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
				keys = new List<ushort>();
				keys.Add((ushort)value);

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

			text = text.Trim();

			if (text.Length > 0)
			{
                // Enanched Map Chat
                if (MapUO.MapNetwork.Connected)
                    if (text.StartsWith(RazorEnhanced.Settings.General.ReadString("MapChatPrefixTextBox")))
                    {
                        string message = text.Replace(RazorEnhanced.Settings.General.ReadString("MapChatPrefixTextBox"), "");
                        MapUO.MapNetworkOut.SendChatMessageQueue.Enqueue(new MapUO.MapNetworkOut.SendChatMessage(message.Length, RazorEnhanced.Settings.General.ReadInt("MapChatColor"), message));
                        args.Block = true;
                    }

				if (text[0] == '-')
				{
					text = text.Substring(1);
					string[] split = text.Split(' ', '\t');
					CommandCallback call = (CommandCallback)m_List[split[0]];
					if (call != null)
					{
						string[] param = new String[split.Length - 1];
						for (int i = 0; i < param.Length; i++)
							param[i] = split[i + 1];
						call(param);

						args.Block = true;
					}
				}
			}
		}
	}
}

