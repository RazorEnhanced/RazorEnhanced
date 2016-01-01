using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assistant.Map
{
	internal class MapNetworkIn
	{
		internal class UserData
		{
			private string m_Name;

			public string Name
			{
				get { return m_Name; }
				set { m_Name = value; }
			}

			private short m_X;

			public short X
			{
				get { return m_X; }
				set { m_X = value; }
			}

			private short m_Y;

			public short Y
			{
				get { return m_Y; }
				set { m_Y = value; }
			}

			private short m_Facet;

			public short Facet
			{
				get { return m_Facet; }
				set { m_Facet = value; }
			}

			private short m_DeathPointX;

			public short DeathPointX
			{
				get { return m_DeathPointX; }
				set { m_DeathPointX = value; }
			}

			private short m_DeathPointY;

			public short DeathPointY
			{
				get { return m_DeathPointY; }
				set { m_DeathPointY = value; }
			}

			private short m_DeathPointFacet;

			public short DeathPointFacet
			{
				get { return m_DeathPointFacet; }
				set { m_DeathPointFacet = value; }
			}

			private short m_PanicPointX;

			public short PanicPointX
			{
				get { return m_PanicPointX; }
				set { m_PanicPointX = value; }
			}

			private short m_PanicPointY;

			public short PanicPointY
			{
				get { return m_PanicPointY; }
				set { m_PanicPointY = value; }
			}

			private short m_PanicPointFacet;

			public short PanicPointFacet
			{
				get { return m_PanicPointFacet; }
				set { m_PanicPointFacet = value; }
			}

			private short m_Hits;

			public short Hits
			{
				get { return m_Hits; }
				set { m_Hits = value; }
			}

			private short m_Hitsmax;

			public short HitsMax
			{
				get { return m_Hitsmax; }
				set { m_Hitsmax = value; }
			}

			private short m_Stamina;

			public short Stamina
			{
				get { return m_Stamina; }
				set { m_Stamina = value; }
			}

			private short m_Staminamax;

			public short StaminaMax
			{
				get { return m_Staminamax; }
				set { m_Staminamax = value; }
			}

			private short m_Mana;

			public short Mana
			{
				get { return m_Mana; }
				set { m_Mana = value; }
			}

			private short m_ManaMax;

			public short ManaMax
			{
				get { return m_ManaMax; }
				set { m_ManaMax = value; }
			}

			private short m_Flags;

			public short Flag
			{
				get { return m_Flags; }
				set { m_Flags = value; }
			}

			private Color m_Color;

			public Color Color
			{
				get { return m_Color; }
				set { m_Color = value; }
			}

			public UserData(string name, short x, short y, short facet, short deathpointx, short deathpointy, short deathpointfacet, short panicpointx, short panicpointy, short panicpointfacet, short hits, short hitsmax, short stamina, short staminamax, short mana, short manamax, short flag, Color color)
			{
				m_Name = name;
				m_X = x;
				m_Y = y;
				m_Facet = facet;
				m_DeathPointX = deathpointx;
				m_DeathPointY = deathpointy;
				m_DeathPointFacet = deathpointfacet;
				m_PanicPointX = panicpointx;
				m_PanicPointY = panicpointy;
				m_PanicPointFacet = panicpointfacet;
				m_Hits = hits;
				m_Hitsmax = hitsmax;
				m_Stamina = stamina;
				m_Staminamax = staminamax;
				m_Mana = mana;
				m_ManaMax = manamax;
				m_Flags = flag;
				m_Color = color;
			}
		}

		internal static void InThreadExec()
		{
			while (MapNetwork.InThreadFlag)
			{
				try
				{
					byte[] bytesFrom = new byte[MapNetwork.clientSocket.ReceiveBufferSize];
					MapNetwork.serverStream.Read(bytesFrom, 0, (MapNetwork.clientSocket.ReceiveBufferSize));

					byte[] PacketIDByte = new byte[2] { bytesFrom[0], bytesFrom[1] };
					if (BitConverter.IsLittleEndian)
						Array.Reverse(PacketIDByte);
					short packedID = BitConverter.ToInt16(PacketIDByte, 0);
					switch (packedID)
					{
						// Ricevuta risposta da send
						case 3:
							{
								MapNetwork.serverStream.Flush();
								break;
							}
						// Coords update
						case 4:
							{
								byte[] XByte = new byte[2] { bytesFrom[2], bytesFrom[3] };
								byte[] YByte = new byte[2] { bytesFrom[4], bytesFrom[5] };
								short x = BitConverter.ToInt16(XByte, 0);
								short y = BitConverter.ToInt16(YByte, 0);
								short map = bytesFrom[6];
								short userlenght = bytesFrom[7];
								string username = Encoding.Default.GetString(bytesFrom, 8, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateUserCoords(x, y, map, username);

								if (MapWindow.MapControl != null)
									MapWindow.MapControl.Invoke(new Action(() => MapWindow.MapControl.FullUpdate()));

								MapNetwork.AddLog("DEBUG: coords: " + x + " - " + y + " - " + map);

								break;
							}
						// stats update
						case 6:
							{
								byte[] hitsByte = new byte[2] { bytesFrom[2], bytesFrom[3] };
								byte[] staminaByte = new byte[2] { bytesFrom[4], bytesFrom[5] };
								byte[] manaByte = new byte[2] { bytesFrom[6], bytesFrom[7] };
								byte[] hitsmaxByte = new byte[2] { bytesFrom[8], bytesFrom[9] };
								byte[] staminamaxByte = new byte[2] { bytesFrom[10], bytesFrom[11] };
								byte[] manamaxByte = new byte[2] { bytesFrom[12], bytesFrom[13] };
								short hits = BitConverter.ToInt16(hitsByte, 0);
								short stamina = BitConverter.ToInt16(staminaByte, 0);
								short mana = BitConverter.ToInt16(manaByte, 0);
								short hitsmax = BitConverter.ToInt16(hitsmaxByte, 0);
								short staminamax = BitConverter.ToInt16(staminamaxByte, 0);
								short manamax = BitConverter.ToInt16(manamaxByte, 0);
								short userlenght = bytesFrom[14];
								string username = Encoding.Default.GetString(bytesFrom, 15, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateUserHits(hits, stamina, mana, hitsmax, staminamax, manamax, username);

								if (MapWindow.MapControl != null)
									MapWindow.MapControl.Invoke(new Action(() => MapWindow.MapControl.FullUpdate()));

								MapNetwork.AddLog("DEBUG: stats: " + hits + " - " + stamina + " - " + mana);

								break;
							}
						// Flags update
						case 8:
							{
								short flag = bytesFrom[2];
								short userlenght = bytesFrom[3];
								string username = Encoding.Default.GetString(bytesFrom, 4, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateUserFlag(flag, username);

								MapNetwork.AddLog("DEBUG: Flag: " + flag);

								break;
							}
						// Death point
						case 10:
							{
								byte[] XByte = new byte[2] { bytesFrom[2], bytesFrom[3] };
								byte[] YByte = new byte[2] { bytesFrom[4], bytesFrom[5] };
								short x = BitConverter.ToInt16(XByte, 0);
								short y = BitConverter.ToInt16(YByte, 0);
								short map = bytesFrom[6];
								short userlenght = bytesFrom[7];
								string username = Encoding.Default.GetString(bytesFrom, 8, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateUserDeathPoint(x, y, map, username);

								MapNetwork.AddLog("DEBUG: Death: " + x + " - " + y + " - " + map);

								ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 33, 3, Language.CliLocName, "System", "[MAP-LINK] " + username + ": Died at coords: " + x + " - " + y + " facet: " + map));

								break;
							}
						// Panic point
						case 12:
							{
								byte[] XByte = new byte[2] { bytesFrom[2], bytesFrom[3] };
								byte[] YByte = new byte[2] { bytesFrom[4], bytesFrom[5] };
								short x = BitConverter.ToInt16(XByte, 0);
								short y = BitConverter.ToInt16(YByte, 0);
								short map = bytesFrom[6];
								short userlenght = bytesFrom[7];
								string username = Encoding.Default.GetString(bytesFrom, 8, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateUserPanicPoint(x, y, map, username);

								MapNetwork.AddLog("DEBUG: Panic: " + x + " - " + y + " - " + map);

								break;
							}
						// Chat message
						case 14:
							{
								byte[] msg_lenghtByte = new byte[2] { bytesFrom[2], bytesFrom[3] };
								byte[] msg_colorByte = new byte[4] { bytesFrom[4], bytesFrom[5], bytesFrom[6], bytesFrom[7] };
								Array.Reverse(msg_colorByte);
								short msg_lenght = BitConverter.ToInt16(msg_lenghtByte, 0);
								int msg_color = BitConverter.ToInt32(msg_colorByte, 0);
								short userlenght = bytesFrom[8];
								string msg = Encoding.Default.GetString(bytesFrom, 9, msg_lenght);
								string user = Encoding.Default.GetString(bytesFrom, 9 + msg_lenght, userlenght);
								MapNetwork.serverStream.Flush();

								ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, msg_color, 3, Language.CliLocName, "System", "[MAP-CHAT] " + user + ": " + msg));

								MapNetwork.AddLog("DEBUG: Chat message: " + msg + " - Col: " + msg_color.ToString());

								break;
							}
						// another user logger
						case 16:
							{
								short userlenght = bytesFrom[2];
								string username = Encoding.Default.GetString(bytesFrom, 3, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateAddUser(username);

								ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 33, 3, Language.CliLocName, "System", "[MAP-LINK] " + username + ": Logged In"));

								MapNetwork.AddLog("User: " + username + " Logged In!");

								break;
							}
						// another user logoff
						case 18:
							{
								short userlenght = bytesFrom[2];
								string username = Encoding.Default.GetString(bytesFrom, 3, userlenght);
								MapNetwork.serverStream.Flush();

								UpdateRemoveUser(username);

								ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 33, 3, Language.CliLocName, "System", "[MAP-LINK] " + username + ": Logged Out"));

								MapNetwork.AddLog("User: " + username + " Logged Out!");
								break;
							}
						// user request colot
						case 20:
							{
								int userlenght = bytesFrom[2];
								string username = Encoding.UTF8.GetString(bytesFrom, 3, userlenght);
								MapNetwork.serverStream.Flush();
								MapNetworkOut.SendRequestColorQueue.Enqueue(username);
								MapNetwork.AddLog(username + " richiede il mio colore");
								break;
							}
						// user send color
						case 22:
							{
								int userlenght = bytesFrom[2];
								string username = Encoding.UTF8.GetString(bytesFrom, 3, userlenght);
								int startByteCol = userlenght + 3;
								byte[] mex_color = new byte[4] { bytesFrom[startByteCol], bytesFrom[startByteCol + 1], bytesFrom[startByteCol + 2], bytesFrom[startByteCol + 3] };
								string mex_color_trad = "#" + BitConverter.ToString(mex_color).Replace("-", "");
								Color finalColor = ColorTranslator.FromHtml(mex_color_trad);
								MapNetwork.serverStream.Flush();
								UpdateUserColor(finalColor, username);
								MapNetwork.AddLog(username + " ha inviato il suo colore");
								break;
							}
					}
				}
				catch (Exception ex)
				{
					MapNetwork.Disconnect();
				}
			}
			MapNetwork.AddLog("Read Thread Exit");
		}

		private static void UpdateUserCoords(short x, short y, short map, string username)
		{
			var obj = MapNetwork.UData.FirstOrDefault(List => List.Name == username);
			if (obj != null)
			{
				obj.X = x;
				obj.Y = y;
				obj.Facet = map;
			}
			else
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, x, y, map, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Color.White));
		}

		private static void UpdateUserHits(short hits, short stamina, short mana, short hitsmax, short staminamax, short manamax, string username)
		{
			var obj = MapNetwork.UData.FirstOrDefault(List => List.Name == username);
			if (obj != null)
			{
				obj.Hits = hits;
				obj.Stamina = stamina;
				obj.Mana = mana;
				obj.HitsMax = hitsmax;
				obj.StaminaMax = staminamax;
				obj.ManaMax = manamax;
			}
			else
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, hits, stamina, mana, hitsmax, staminamax, manamax, 0, Color.White));
		}

		private static void UpdateUserFlag(short flag, string username)
		{
			var obj = MapNetwork.UData.FirstOrDefault(List => List.Name == username);
			if (obj != null)
			{
				obj.Flag = flag;
			}
			else
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, flag, Color.White));
		}

		private static void UpdateUserDeathPoint(short x, short y, short map, string username)
		{
			var obj = MapNetwork.UData.FirstOrDefault(List => List.Name == username);
			if (obj != null)
			{
				obj.DeathPointX = x;
				obj.DeathPointY = y;
				obj.DeathPointFacet = map;
			}
			else
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, x, y, map, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Color.White));
		}

		private static void UpdateUserPanicPoint(short x, short y, short map, string username)
		{
			var obj = MapNetwork.UData.FirstOrDefault(List => List.Name == username);
			if (obj != null)
			{
				obj.PanicPointX = x;
				obj.PanicPointY = y;
				obj.PanicPointFacet = map;
			}
			else
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, x, y, map, 0, 0, 0, 0, 0, 0, 0, Color.White));
		}

		private static void UpdateUserColor(Color col, string username)
		{
			MapNetworkIn.UserData obj = MapNetwork.UData.FirstOrDefault(u => u.Name == username);

			if (obj != null)
				obj.Color = col;
			else
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, col));

			if (username != RazorEnhanced.Settings.General.ReadString("MapLinkUsernameTextBox") && username != MapNetwork.PointN.Death && username != MapNetwork.PointN.Marker)
			{
				// if (Finder.finderControlstatic IsNot Nothing Then Finder.finderControlstatic.Invoke(New Action(Sub() Finder.ListViewPlayers.FindItemWithText(username).SubItems(7).Text = obj.Color.ToString))
			}
		}

		private static void UpdateAddUser(string username)
		{
			bool found = false;
			foreach (MapNetworkIn.UserData data in MapNetwork.UData)
			{
				if (data.Name == username)
				{
					found = true;
					break;
				}
			}

			if (!found)
				MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Color.White));
		}

		private static void UpdateRemoveUser(string username)
		{
			int i = 0;
			bool found = false;
			foreach (MapNetworkIn.UserData data in MapNetwork.UData)
			{
				if (data.Name == username)
				{
					found = true;
					break;
				}
				i++;
			}
			if (found)
				MapNetwork.UData.RemoveAt(i);
		}
	}
}