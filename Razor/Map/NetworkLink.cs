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


namespace Assistant.MapUO
{
	internal class MapNetwork
	{
        internal static bool OnClosing = false;
        internal static bool Connected = false;
        internal static TcpClient clientSocket = new TcpClient();
        internal static NetworkStream serverStream;
        internal static bool InThreadFlag = true;
        internal static bool OutThreadFlag = true;
        internal static Thread ConnThread;
        internal static Thread InThread;
        internal static Thread OutThread;
        internal static int port;
        internal static List<MapNetworkIn.UserData> UData;

        internal static void AddLog(string addlog)
        {
            if (!OnClosing)
            {
                Assistant.Engine.MainWindow.MapLogListBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLogListBox.Items.Add(addlog)));
                Assistant.Engine.MainWindow.MapLogListBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLogListBox.SelectedIndex = Assistant.Engine.MainWindow.MapLogListBox.Items.Count - 1));
                if (Assistant.Engine.MainWindow.MapLogListBox.Items.Count > 300)
                    Assistant.Engine.MainWindow.MapLogListBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLogListBox.Items.Clear()));
            }
        }

		internal static void TryConnect()
        {
            if (Resolve(RazorEnhanced.Settings.General.ReadString("MapServerAddressTextBox")) == IPAddress.None)
            {
                AddLog("- Invalid Map server address");
                return;
            }

            try
            {
                port = Convert.ToInt32(RazorEnhanced.Settings.General.ReadString("MapServerPortTextBox"));
            }
            catch
            {
                AddLog("- Invalid Map server port");
                return;
            }

            if (Resolve(RazorEnhanced.Settings.General.ReadString("MapServerAddressTextBox")) == IPAddress.None)
            {
                AddLog("- Invalid Map server address");
                return;
            }

            if (RazorEnhanced.Settings.General.ReadString("MapLinkUsernameTextBox") == "")
            {
                AddLog("- Invalid Username");
                return;
            }

            if (RazorEnhanced.Settings.General.ReadString("MapLinkPasswordTextBox") == "")
            {
                AddLog("- Invalid Password");
                return;
            }

            LockItem();
            AddLog("- Start connection Thread");
            ConnThread = new Thread(ConnThreadExec);
            ConnThread.Start();
        }

        internal static void ConnThreadExec()
        {
            try
            {
                clientSocket = new TcpClient();
                AddLog("- Try to Connect");
                clientSocket.Connect(RazorEnhanced.Settings.General.ReadString("MapServerAddressTextBox"), port);
                AddLog("- Connected");
                Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.Text = "ONLINE"));
                Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.ForeColor = Color.Green));
                
                // Invia dati login
                List<byte> data = new List<byte>();
                data.Add(0x0);
                data.Add(0x1);
                data.Add((byte)RazorEnhanced.Settings.General.ReadString("MapLinkUsernameTextBox").Length);
                data.Add((byte)RazorEnhanced.Settings.General.ReadString("MapLinkPasswordTextBox").Length);
                data.AddRange(Encoding.Default.GetBytes(RazorEnhanced.Settings.General.ReadString("MapLinkUsernameTextBox")));
                data.AddRange(Encoding.Default.GetBytes(RazorEnhanced.Settings.General.ReadString("MapLinkPasswordTextBox")));
                Byte[] sendBytes = data.ToArray();
                serverStream = clientSocket.GetStream();
                serverStream.Write(sendBytes, 0, sendBytes.Length);
                serverStream.Flush();

                // Risposta login
                byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                serverStream.Read(bytesFrom, 0, (clientSocket.ReceiveBufferSize));
                

                byte[] PacketIDByte = new byte[2] { bytesFrom[0], bytesFrom[1] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(PacketIDByte);
                short packedID = BitConverter.ToInt16(PacketIDByte, 0);
                if (packedID == 2)
                {
                    if (bytesFrom[2] == 1)
                    {
                        UData = new List<MapNetworkIn.UserData>();
                        serverStream.Flush();
                        AddLog("- Login Succesfull");
                        AddLog("- Start Read Thread");
                        InThreadFlag = true;
                        InThread = new Thread(MapNetworkIn.InThreadExec);
                        InThread.Start();
                        AddLog("- Start Write Thread");
                        OutThreadFlag = true;
                        OutThread = new Thread(MapNetworkOut.OutThreadExec);
                        OutThread.Start();
                        Connected = true;
                    }
                    else
                    {
                        AddLog("- Wrong user or password");
                        Disconnect();
                    }
                }
                else
                {
                    AddLog("- Unknow server response");
                    Disconnect();
                }
            }
            catch
            {
                AddLog("- Fail to Connect");
                UnLockItem();
            }
            AddLog("- Connection Thread End");
        }

        internal static void Disconnect()
        {
            try
            {
                UData.Clear();
            }
            catch { }
            
            InThreadFlag = false;
            OutThreadFlag = false;
            Connected = false;

            try
            {
                clientSocket.GetStream().Close();
            }
            catch { }

            try
            {
                clientSocket.Close();
            }
            catch { }
            if (!OnClosing)
            {
                UnLockItem();
                Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.Text = "OFFLINE"));
                Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.ForeColor = Color.Red));
            }
        }

        internal static void LockItem()
        {
            Assistant.Engine.MainWindow.MapConnectButton.Invoke(new Action(() => Assistant.Engine.MainWindow.MapConnectButton.Enabled = false));
            Assistant.Engine.MainWindow.MapServerAddressTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerAddressTextBox.Enabled = false));
            Assistant.Engine.MainWindow.MapServerPortTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerPortTextBox.Enabled = false));
            Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Enabled = false));
            Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Enabled = false));
        }
        internal static void UnLockItem()
        {
            Assistant.Engine.MainWindow.MapConnectButton.Invoke(new Action(() => Assistant.Engine.MainWindow.MapConnectButton.Enabled = true));
            Assistant.Engine.MainWindow.MapServerAddressTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerAddressTextBox.Enabled = true));
            Assistant.Engine.MainWindow.MapServerPortTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerPortTextBox.Enabled = true));
            Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Enabled = true));
            Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Enabled = true));
        }

        private static IPAddress Resolve(string addr)
        {
            IPAddress ipAddr = IPAddress.None;

            if (addr == null || addr == string.Empty)
                return ipAddr;

            try
            {
                ipAddr = IPAddress.Parse(addr);
            }
            catch
            {
                try
                {
                    IPHostEntry iphe = Dns.GetHostEntry(addr);

                    if (iphe.AddressList.Length > 0)
                        ipAddr = iphe.AddressList[iphe.AddressList.Length - 1];
                }
                catch
                {
                }
            }

            return ipAddr;
        }
	}
}
