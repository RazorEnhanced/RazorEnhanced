using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Assistant.Map
{
	internal class MapNetworkOut
	{
		internal class SendCoord
		{
			private int m_X;
			public int X { get { return m_X; } }

			private int m_Y;
			public int Y { get { return m_Y; } }

			private int m_Map;
			public int Map { get { return m_Map; } }

			public SendCoord(int x, int y, int map)
			{
				m_X = x;
				m_Y = y;
				m_Map = map;
			}
		}

		internal class SendStat
		{
			private int m_Hits;
			public int Hits { get { return m_Hits; } }

			private int m_Stamina;
			public int Stamina { get { return m_Stamina; } }

			private int m_Mana;
			public int Mana { get { return m_Mana; } }

			private int m_HitsMax;
			public int HitsMax { get { return m_HitsMax; } }

			private int m_StaminaMax;
			public int StaminaMax { get { return m_StaminaMax; } }

			private int m_ManaMax;
			public int ManaMax { get { return m_ManaMax; } }

			public SendStat(int hits, int stamina, int mana, int hitsmax, int staminamax, int manamax)
			{
				m_Hits = hits;
				m_Stamina = stamina;
				m_Mana = mana;
				m_HitsMax = hitsmax;
				m_StaminaMax = staminamax;
				m_ManaMax = manamax;
			}
		}

		internal class SendDeathPoint
		{
			private int m_X;
			public int X { get { return m_X; } }

			private int m_Y;
			public int Y { get { return m_Y; } }

			private int m_Map;
			public int Map { get { return m_Map; } }

			public SendDeathPoint(int x, int y, int map)
			{
				m_X = x;
				m_Y = y;
				m_Map = map;
			}
		}

		internal class SendPanic
		{
			private int m_X;
			public int X { get { return m_X; } }

			private int m_Y;
			public int Y { get { return m_Y; } }

			private int m_Map;
			public int Map { get { return m_Map; } }

			public SendPanic(int x, int y, int map)
			{
				m_X = x;
				m_Y = y;
				m_Map = map;
			}
		}

		internal class SendChatMessage
		{
			private int m_Lenght;
			public int Lenght { get { return m_Lenght; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private string m_Msg;
			public string Msg { get { return m_Msg; } }

			public SendChatMessage(int lenght, int color, string msg)
			{
				m_Lenght = lenght;
				m_Color = color;
				m_Msg = msg;
			}
		}

		internal static ConcurrentQueue<SendCoord> SendCoordQueue = new ConcurrentQueue<SendCoord>();
		internal static ConcurrentQueue<SendStat> SendStatQueue = new ConcurrentQueue<SendStat>();
		internal static ConcurrentQueue<short> SendFlagQueue = new ConcurrentQueue<short>();
		internal static ConcurrentQueue<SendChatMessage> SendChatMessageQueue = new ConcurrentQueue<SendChatMessage>();
		internal static ConcurrentQueue<string> SendReplyColorQueue = new ConcurrentQueue<string>();
		internal static ConcurrentQueue<string> SendRequestColorQueue = new ConcurrentQueue<string>();

		internal static bool SendPanicFlag = false;
		internal static bool SendDeathPointFlag = false;
		internal static SendPanic SendPanicData;
		internal static SendDeathPoint SendDeathPointData;
		internal static bool LastDead = false;

		internal static void OutThreadExec()
		{
			SendCoordQueue = new ConcurrentQueue<SendCoord>();
			SendStatQueue = new ConcurrentQueue<SendStat>();
			SendFlagQueue = new ConcurrentQueue<short>();
			SendChatMessageQueue = new ConcurrentQueue<SendChatMessage>();
			SendPanicFlag = false;
			SendDeathPointFlag = false;
			LastDead = false;
			SendPanicData = new SendPanic(0, 0, 0);
			SendDeathPointData = new SendDeathPoint(0, 0, 0);

			Byte[] outStream;

			// Invio stato alla connessione effettuata
			if (World.Player != null)
			{
				MapNetworkOut.SendCoordQueue.Enqueue(new MapNetworkOut.SendCoord(World.Player.Position.X, World.Player.Position.Y, World.Player.Map));
				MapNetworkOut.SendStatQueue.Enqueue(new MapNetworkOut.SendStat(World.Player.Hits, World.Player.Stam, World.Player.Mana, World.Player.HitsMax, World.Player.StamMax, World.Player.ManaMax));
				if (World.Player.Hits == 0)
					MapNetworkOut.SendFlagQueue.Enqueue(4);
				else if (World.Player.Poisoned)
					MapNetworkOut.SendFlagQueue.Enqueue(1);
				else
					MapNetworkOut.SendFlagQueue.Enqueue(0);
			}

			while (MapNetwork.OutThreadFlag)
			{
				try
				{
					// Processo send coordinate
					if (SendCoordQueue.Count > 0)
					{
						SendCoord coordtosend;
						SendCoordQueue.TryDequeue(out coordtosend);
						if (coordtosend != null)
						{
							List<byte> data = new List<byte>();
							data.Add(0x0);
							data.Add(0x2);
							data.AddRange(BitConverter.GetBytes((short)coordtosend.X));
							data.AddRange(BitConverter.GetBytes((short)coordtosend.Y));
							data.Add((byte)coordtosend.Map);
							outStream = data.ToArray();
							MapNetwork.serverStream.Write(outStream, 0, outStream.Length);
							MapNetwork.serverStream.Flush();
							data.Clear();
						}
					}

					// Processo send stats
					if (SendStatQueue.Count > 0)
					{
						SendStat statstosend;
						SendStatQueue.TryDequeue(out statstosend);
						if (statstosend != null)
						{
							List<byte> data = new List<byte>();
							data.Add(0x0);
							data.Add(0x5);
							data.AddRange(BitConverter.GetBytes((short)statstosend.Hits));
							data.AddRange(BitConverter.GetBytes((short)statstosend.Stamina));
							data.AddRange(BitConverter.GetBytes((short)statstosend.Mana));
							data.AddRange(BitConverter.GetBytes((short)statstosend.HitsMax));
							data.AddRange(BitConverter.GetBytes((short)statstosend.StaminaMax));
							data.AddRange(BitConverter.GetBytes((short)statstosend.ManaMax));

							outStream = data.ToArray();
							MapNetwork.serverStream.Write(outStream, 0, outStream.Length);
							MapNetwork.serverStream.Flush();
							data.Clear();
						}
					}

					// Processo send Flags
					if (SendFlagQueue.Count > 0)
					{
						short flagtosend;
						SendFlagQueue.TryDequeue(out flagtosend);
						List<byte> data = new List<byte>();
						data.Add(0x0);
						data.Add(0x7);
						data.AddRange(BitConverter.GetBytes((short)flagtosend));

						outStream = data.ToArray();
						MapNetwork.serverStream.Write(outStream, 0, outStream.Length);
						MapNetwork.serverStream.Flush();
						data.Clear();
					}

					// Processo send DeathPoint
					if (SendDeathPointFlag)
					{
						if (SendDeathPointData != null)
						{
							List<byte> data = new List<byte>();
							data.Add(0x0);
							data.Add(0x9);
							data.AddRange(BitConverter.GetBytes((short)SendDeathPointData.X));
							data.AddRange(BitConverter.GetBytes((short)SendDeathPointData.Y));
							data.Add((byte)SendDeathPointData.Map);

							outStream = data.ToArray();
							MapNetwork.serverStream.Write(outStream, 0, outStream.Length);
							MapNetwork.serverStream.Flush();

							SendDeathPointFlag = false;
							data.Clear();
						}
					}

					// Processo send Panic
					if (SendPanicFlag)
					{
						if (SendPanicData != null)
						{
							List<byte> data = new List<byte>();
							data.Add(0x0);
							data.Add(0xB);
							data.AddRange(BitConverter.GetBytes((short)SendPanicData.X));
							data.AddRange(BitConverter.GetBytes((short)SendPanicData.Y));
							data.Add((byte)SendPanicData.Map);

							outStream = data.ToArray();
							MapNetwork.serverStream.Write(outStream, 0, outStream.Length);
							MapNetwork.serverStream.Flush();

							SendPanicFlag = false;
							data.Clear();
						}
					}

					// Processo send Chat
					if (SendChatMessageQueue.Count > 0)
					{
						SendChatMessage msgtosend;
						SendChatMessageQueue.TryDequeue(out msgtosend);
						if (msgtosend != null)
						{
							List<byte> data = new List<byte>();
							data.Add(0x0);
							data.Add(0xD);
							data.AddRange(BitConverter.GetBytes((short)msgtosend.Lenght));
							data.AddRange(BitConverter.GetBytes(msgtosend.Color));
							data.AddRange(Encoding.Default.GetBytes(msgtosend.Msg));

							outStream = data.ToArray();
							MapNetwork.serverStream.Write(outStream, 0, outStream.Length);
							MapNetwork.serverStream.Flush();
							data.Clear();
						}
					}
				}
				catch (Exception ex)
				{
					MapNetwork.OutThreadFlag = false;
					MapNetwork.Disconnect();
				}
			}
			MapNetwork.AddLog("Write Thread Exit");
		}
	}
}