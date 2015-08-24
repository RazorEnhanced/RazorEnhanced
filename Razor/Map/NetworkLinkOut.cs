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
        public class SendDeathPoint
        {
            private int m_x;
            public int X { get { return m_x; } }

            private int m_y;
            public int Y { get { return m_y; } }

            private int m_map;
            public int Map { get { return m_map; } }
            public SendDeathPoint(int x, int y, int map)
            {
                m_x = x;
                m_y = y;
                m_map = map;
            }
        }

        public class SendPanic
        {
            private int m_x;
            public int X { get { return m_x; } }

            private int m_y;
            public int Y { get { return m_y; } }

            private int m_map;
            public int Map { get { return m_map; } }
            public SendPanic(int x, int y, int map)
            {
                m_x = x;
                m_y = y;
                m_map = map;
            }
        }

        public class SendChatMessage
        {
            private int m_msg_lenght;
            public int Lenght { get { return m_msg_lenght; } }

            private int m_color;
            public int Color { get { return m_color; } }

            private string m_msg;
            public string Msg { get { return m_msg; } }
            public SendChatMessage(int msg_lenght, int color, string msg)
            {
                m_msg_lenght = msg_lenght;
                m_color = color;
                m_msg = msg;
            }
        }

        internal static ConcurrentQueue<SendCoord> SendCoordQueue = new ConcurrentQueue<SendCoord>();
        internal static ConcurrentQueue<SendStat> SendStatQueue = new ConcurrentQueue<SendStat>();
        internal static ConcurrentQueue<short> SendFlagQueue = new ConcurrentQueue<short>();
        internal static ConcurrentQueue<SendChatMessage> SendChatMessageQueue = new ConcurrentQueue<SendChatMessage>();

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
            SendPanicData = new SendPanic(0,0,0);
            SendDeathPointData = new SendDeathPoint(0, 0, 0);

            Byte[] outStream;

            // Invio stato alla connessione effettuata
            if (World.Player != null)
            {
                MapNetworkOut.SendCoordQueue.Enqueue(new MapUO.MapNetworkOut.SendCoord(World.Player.Position.X, World.Player.Position.Y, World.Player.Map));
                MapNetworkOut.SendStatQueue.Enqueue(new MapUO.MapNetworkOut.SendStat(World.Player.Hits, World.Player.Stam, World.Player.Mana, World.Player.HitsMax, World.Player.StamMax, World.Player.ManaMax));
                if (World.Player.Hits == 0)
                    MapNetworkOut.SendFlagQueue.Enqueue(4);
                else if (World.Player.Poisoned)
                    MapUO.MapNetworkOut.SendFlagQueue.Enqueue(1);
                else
                    MapUO.MapNetworkOut.SendFlagQueue.Enqueue(0);
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
                        Sleep(); // Attende risposta dall'altro thread di packet ricevuto

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
                            Sleep(); // Attende risposta dall'altro thread di packet ricevuto
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
                            Sleep(); // Attende risposta dall'altro thread di packet ricevuto
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
