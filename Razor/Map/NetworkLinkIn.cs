using System;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Linq;

namespace Assistant.MapUO
{
	internal class MapNetworkIn
	{
        public class UserData
        {
            private string m_nome;
            public string Nome
            {
                get { return m_nome; }
                set { m_nome = value; }
            }

            private short m_x;
            public short X
            {
                get { return m_x; }
                set { m_x = value; }
            }

            private short m_y;
            public short Y
            {
                get { return m_y; }
                set { m_y = value; }
            }

            private short m_facet;
            public short Facet
            {
                get { return m_facet; }
                set { m_facet = value; }
            }

            private short m_deathPointX;
            public short DeathPointX
            {
                get { return m_deathPointX; }
                set { m_deathPointX = value; }
            }

            private short m_deathPointY;
            public short DeathPointY
            {
                get { return m_deathPointY; }
                set { m_deathPointY = value; }
            }

            private short m_deathPointFacet;
            public short DeathPointFacet
            {
                get { return m_deathPointFacet; }
                set { m_deathPointFacet = value; }
            }

            private short m_panicPointX;
            public short PanicPointX
            {
                get { return m_panicPointX; }
                set { m_panicPointX = value; }
            }

            private short m_panicPointY;
            public short PanicPointY
            {
                get { return m_panicPointY; }
                set { m_panicPointY = value; }
            }

            private short m_panicPointFacet;
            public short PanicPointFacet
            {
                get { return m_panicPointFacet; }
                set { m_panicPointFacet = value; }
            }

            private short m_hits;
            public short Hits
            {
                get { return m_hits; }
                set { m_hits = value; }
            }

            private short m_hitsmax;
            public short HitsMax
            {
                get { return m_hitsmax; }
                set { m_hitsmax = value; }
            }

            private short m_stamina;
            public short Stamina
            {
                get { return m_stamina; }
                set { m_stamina = value; }
            }

            private short m_staminamax;
            public short StaminaMax
            {
                get { return m_staminamax; }
                set { m_staminamax = value; }
            }

            private short m_mana;
            public short Mana
            {
                get { return m_mana; }
                set { m_mana = value; }
            }

            private short m_manamax;
            public short ManaMax
            {
                get { return m_manamax; }
                set { m_manamax = value; }
            }

            private short m_flag;
            public short Flag
            {
                get { return m_flag; }
                set { m_flag = value; }
            }
            public UserData(string nome, short x, short y, short facet, short deathpointx, short deathpointy, short deathpointfacet, short panicpointx, short panicpointy, short panicpointfacet, short hits, short hitsmax, short stamina, short staminamax, short mana, short manamax, short flag)
			{
                m_nome = nome;
                m_x = x;
                m_y = y;
                m_facet = facet;
                m_deathPointX = deathpointx;
                m_deathPointY = deathpointy;
                m_deathPointFacet = deathpointfacet;
                m_panicPointX = panicpointx;
                m_panicPointY = panicpointy;
                m_panicPointFacet = panicpointfacet;
                m_hits = hits;
                m_hitsmax = hitsmax;
                m_stamina = stamina;
                m_staminamax = staminamax;
                m_mana = mana;
                m_manamax = manamax;
                m_flag = flag;
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
                        case 3:         // Ricevuta risposta da send
                            {
                                MapNetworkOut.SendSleep = false;
                                MapNetwork.serverStream.Flush();
                                break;
                            }
                        case 4:     // Coords update
                            {
                                byte[] XByte = new byte[2] {bytesFrom[2], bytesFrom[3]};
                                byte[] YByte = new byte[2] {bytesFrom[4], bytesFrom[5]};
                                short x = BitConverter.ToInt16(XByte, 0);
                                short y = BitConverter.ToInt16(YByte, 0);
                                short map = bytesFrom[6];
                                short userlenght = bytesFrom[7];
                                string username = Encoding.Default.GetString(bytesFrom, 8, userlenght);
                                MapNetwork.serverStream.Flush();

                                UpdateUserCoords(x, y, map, username);

                                if (MapWindow.uoMapControlstatic != null)
                                    MapWindow.uoMapControlstatic.Invoke(new Action(() => MapWindow.uoMapControlstatic.FullUpdate()));
                                
                                MapNetwork.AddLog("DEBUG: coords: " + x + " - " + y + " - " + map);

                                break;
                            }   
                        case 6:         // stats update
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

                                if (MapWindow.uoMapControlstatic != null)
                                    MapWindow.uoMapControlstatic.Invoke(new Action(() => MapWindow.uoMapControlstatic.FullUpdate()));

                                MapNetwork.AddLog("DEBUG: stats: " + hits + " - " + stamina + " - " + mana);

                                break;
                            }
                        case 8:         // Flags update
                            {
                                short flag = bytesFrom[2];
                                short userlenght = bytesFrom[3];
                                string username = Encoding.Default.GetString(bytesFrom, 4, userlenght);
                                MapNetwork.serverStream.Flush();

                                UpdateUserFlag(flag, username);

                                MapNetwork.AddLog("DEBUG: Flag: " + flag);

                                break;
                            }
                        case 10:            // Death point
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
                        case 12:            // Panic point
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
                        case 14:        // Chat message
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
                        case 16:        // another user logger
                            {
                                short userlenght = bytesFrom[2];
                                string username = Encoding.Default.GetString(bytesFrom, 3, userlenght);
                                MapNetwork.serverStream.Flush();

                                UpdateAddUser(username);

                                ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 33, 3, Language.CliLocName, "System", "[MAP-LINK] " + username + ": Logged In"));

                                MapNetwork.AddLog("User: " + username + " Logged In!");

                                break;
                            }
                        case 18:        // another user logoff
                            {
                                short userlenght = bytesFrom[2];
                                string username = Encoding.Default.GetString(bytesFrom, 3, userlenght);
                                MapNetwork.serverStream.Flush();
                                
                                UpdateRemoveUser(username);

                                ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 33, 3, Language.CliLocName, "System", "[MAP-LINK] " + username + ": Logged Out"));

                                MapNetwork.AddLog("User: " + username + " Logged Out!");
                                break;
                            }           
                    }

                }
                catch 
                {
                    MapNetwork.Disconnect();
                }
            }
            MapNetwork.AddLog("Read Thread Exit");
        }
        private static void UpdateUserCoords(short x, short y, short map, string username)
        {
            var obj = MapNetwork.UData.FirstOrDefault(List => List.Nome == username);
            if (obj != null)
            {
                obj.X = x;
                obj.Y = y;
                obj.Facet = map;
            }
            else
                MapNetwork.UData.Add(new MapNetworkIn.UserData(username, x, y, map, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }
        private static void UpdateUserHits(short hits, short stamina, short mana, short hitsmax, short staminamax, short manamax, string username)
        {
            var obj = MapNetwork.UData.FirstOrDefault(List => List.Nome == username);
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
                MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, hits, stamina, mana, hitsmax, staminamax, manamax, 0));
        }
        private static void UpdateUserFlag(short flag, string username)
        {
            var obj = MapNetwork.UData.FirstOrDefault(List => List.Nome == username);
            if (obj != null)
            {
                obj.Flag = flag;
            }
            else
                MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, flag));
        }

        private static void UpdateUserDeathPoint(short x, short y, short map, string username)
        {
            var obj = MapNetwork.UData.FirstOrDefault(List => List.Nome == username);
            if (obj != null)
            {
                obj.DeathPointX = x;
                obj.DeathPointY = y;
                obj.DeathPointFacet = map;
            }
            else
                MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, x, y, map, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        private static void UpdateUserPanicPoint(short x, short y, short map, string username)
        {
            var obj = MapNetwork.UData.FirstOrDefault(List => List.Nome == username);
            if (obj != null)
            {
                obj.PanicPointX = x;
                obj.PanicPointY = y;
                obj.PanicPointFacet = map;
            }
            else
                MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, x, y, map, 0, 0, 0, 0, 0, 0, 0));
        }

        private static void UpdateAddUser(string username)
        {
            bool found = false;
            foreach (MapNetworkIn.UserData data in MapNetwork.UData)
            {
                if (data.Nome == username)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                MapNetwork.UData.Add(new MapNetworkIn.UserData(username, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        private static void UpdateRemoveUser(string username)
        {
            int i = 0;
            bool found = false;
            foreach (MapNetworkIn.UserData data in MapNetwork.UData)
            {
                if (data.Nome == username)
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
