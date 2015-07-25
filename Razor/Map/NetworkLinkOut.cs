using System;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Reflection;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

namespace Assistant.MapUO
{
	internal class MapNetworkOut
	{
        internal static bool SendSleep = false;
        public class SendCoord
        {    
            private int m_x;
            public int X { get { return m_x; } }

            private int m_y;
            public int Y { get { return m_y; } }

            private int m_map;
            public int Map { get { return m_map; } }
            public SendCoord(int x, int y, int map)
            {
                m_x = x;
                m_y = y;
                m_map = map;
            }
        }

        public class SendStat
        {
            private int m_hits;
            public int Hits { get { return m_hits; } }

            private int m_stamina;
            public int Stamina { get { return m_stamina; } }

            private int m_mana;
            public int Mana { get { return m_mana; } }

            private int m_hitsmax;
            public int HitsMax { get { return m_hitsmax; } }

            private int m_staminamax;
            public int StaminaMax { get { return m_staminamax; } }

            private int m_manamax;
            public int ManaMax { get { return m_manamax; } }
            public SendStat(int hits, int stamina, int mana, int hitsmax, int staminamax, int manamax)
            {
                m_hits = hits;
                m_stamina = stamina;
                m_mana = mana;
                m_hitsmax = hitsmax;
                m_staminamax = staminamax;
                m_manamax = manamax;
            }
        }

        internal static ConcurrentQueue<SendCoord> SendCoordQueue = new ConcurrentQueue<SendCoord>();
        internal static ConcurrentQueue<SendStat> SendStatQueue = new ConcurrentQueue<SendStat>();
        internal static void OutThreadExec()
        {
            SendCoordQueue = new ConcurrentQueue<SendCoord>();
            SendStatQueue = new ConcurrentQueue<SendStat>();

            Byte[] outStream;
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
                            Sleep(); // Attende risposta dall'altro thread di packet ricevuto
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
                            Sleep(); // Attende risposta dall'altro thread di packet ricevuto
                        }
                    }
                }
                catch
                {
                    MapNetwork.OutThreadFlag = false;
                    MapNetwork.Disconnect();
                }

            }
            MapNetwork.AddLog("Write Thread Exit");
        }

        private static void Sleep()
        {
            SendSleep = true;
            int timeout = 1000;
            while (SendSleep || timeout < 0)
            {
                Thread.Sleep(1);
                timeout -= 1;
            }
        }
       
	}
}
