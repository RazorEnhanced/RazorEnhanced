using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Assistant.Map
{
	internal class MapNetwork
	{
		internal enum Direction
		{
			North = 0,
			Right = 1,
			East = 2,
			Down = 3,
			South = 4,
			Left = 5,
			West = 6,
			Up = 7,
			Mask = Up
		}

		internal class MapName
		{
			public const string Felucca = "Felucca";
			public const string Trammel = "Trammel";
			public const string Ilshenar = "Ilshenar";
			public const string Malas = "Malas";
			public const string Tokuno = "Tokuno";
			public const string TerMur = "TerMur";
			public const string Custom = "Custom";
			public const string FelTram = "Fel/Tram";
		}

		internal class PlayerFlag
		{
			public const string Poison = "C";
			public const string MortalStrike = "D";
			public const string Paralyze = "A";
			public const string Death = "K"; // invented;
		}

		internal class PointN
		{
			public const string Death = "_DEATH_";
			public const string Marker = "_MARKER_";
		}

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
			//if (Engine.Running)
			//{
			//	Assistant.Engine.MainWindow.MapLogListBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLogListBox.Items.Add(addlog)));
			//	Assistant.Engine.MainWindow.MapLogListBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLogListBox.SelectedIndex = Assistant.Engine.MainWindow.MapLogListBox.Items.Count - 1));
			//	if (Assistant.Engine.MainWindow.MapLogListBox.Items.Count > 300)
			//		Assistant.Engine.MainWindow.MapLogListBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLogListBox.Items.Clear()));
			//}
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
				//Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.Text = "ONLINE"));
				//Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.ForeColor = Color.Green));

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

						if (bytesFrom[3] == 1)
						{
							AddLog("- Server require a datafile download");
							short URLlenght = bytesFrom[4];
							string URL = Encoding.Default.GetString(bytesFrom, 5, URLlenght);
							WebClient webClient = new WebClient();
							AddLog("- Start Download...");
							webClient.DownloadFile(URL, Path.GetDirectoryName(Application.ExecutablePath) + "//mapdata.zip");
							AddLog("- Download Done!");
							Decompress();
						}

						AddLog("- Start Parsing Datafile");
						ClearAll();
						MapIcon.ParseImageFile();
						MapIcon.IconTreasurePFList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\TreasurePF.def");
						MapIcon.IconTreasureList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\Treasure.def");
						MapIcon.IconTokunoIslandsList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\TokunoIslands.def");
						MapIcon.IconStealablesList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\Stealables.def");
						MapIcon.IconRaresList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\Rares.def");
						MapIcon.IconPersonalList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\Personal.def");
						MapIcon.IconOldHavenList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\OldHaven.def");
						MapIcon.IconNewHavenList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\NewHaven.def");
						MapIcon.IconMLList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\ML.def");
						MapIcon.IconDungeonsList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\Dungeons.def");
						MapIcon.IconcommonList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\common.def");
						MapIcon.IconAtlasList = MapIcon.ParseDataFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Definitions\\Atlas.def");

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
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				AddLog("- Fail to Connect");
				UnLockItem();
			}
			AddLog("- Connection Thread End");
		}

		private static void Decompress()
		{
			ClearAll();

			if (Directory.Exists("Icon\\"))
			{
				string[] fileEntries = Directory.GetFiles("Icon\\");
				foreach (string fileName in fileEntries)
				{
					if (fileName.EndsWith(".tmp", StringComparison.Ordinal))
					{
						File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + fileName);
					}
				}
			}

			if (Directory.Exists("Definitions\\"))
			{
				string[] fileEntries = Directory.GetFiles("Definitions\\");
				foreach (string fileName in fileEntries)
				{
					if (fileName.EndsWith(".tmp", StringComparison.Ordinal))
					{
						File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + fileName);
					}
				}
			}

			string filename = Path.GetDirectoryName(Application.ExecutablePath) + "\\mapdata.zip";
			AddLog("- Start Decompress Data file: " + filename);
			try
			{
				ZipFile zip = ZipFile.Read(filename);
				zip.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath), ExtractExistingFileAction.OverwriteSilently);
				AddLog("- Done Decompress Data file: " + filename);
				zip.Dispose();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				AddLog("- Fail Decompress Data file: " + filename);
			}
		}

		internal static void ClearAll()
		{
			UData = new List<MapNetworkIn.UserData>();
			MapIcon.IconImage.Clear();
			MapIcon.IconTreasurePFList.Clear();
			MapIcon.IconTreasureList.Clear();
			MapIcon.IconTokunoIslandsList.Clear();
			MapIcon.IconStealablesList.Clear();
			MapIcon.IconRaresList.Clear();
			MapIcon.IconPersonalList.Clear();
			MapIcon.IconPersonalList.Clear();
			MapIcon.IconOldHavenList.Clear();
			MapIcon.IconNewHavenList.Clear();
			MapIcon.IconMLList.Clear();
			MapIcon.IconDungeonsList.Clear();
			MapIcon.IconcommonList.Clear();
			MapIcon.IconAtlasList.Clear();
			MapIcon.AllListOfBuilds.Clear();
			Region.RegionLists.Clear();
		}

		internal static void Init()
		{
			ClearAll();
			MapIcon.ParseImageFile();
			MapIcon.IconTreasurePFList = MapIcon.ParseDataFile("TreasurePF.def");
			MapIcon.IconTreasureList = MapIcon.ParseDataFile("Treasure.def");
			MapIcon.IconTokunoIslandsList = MapIcon.ParseDataFile("TokunoIslands.def");
			MapIcon.IconStealablesList = MapIcon.ParseDataFile("Stealables.def");
			MapIcon.IconRaresList = MapIcon.ParseDataFile("Rares.def");
			MapIcon.IconPersonalList = MapIcon.ParseDataFile("Personal.def");
			MapIcon.IconOldHavenList = MapIcon.ParseDataFile("OldHaven.def");
			MapIcon.IconNewHavenList = MapIcon.ParseDataFile("NewHaven.def");
			MapIcon.IconMLList = MapIcon.ParseDataFile("ML.def");
			MapIcon.IconDungeonsList = MapIcon.ParseDataFile("Dungeons.def");
			MapIcon.IconcommonList = MapIcon.ParseDataFile("common.def");
			MapIcon.IconAtlasList = MapIcon.ParseDataFile("Atlas.def");

			MapIcon.AllListOfBuilds.Add(MapIcon.IconAtlasList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconcommonList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconDungeonsList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconMLList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconNewHavenList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconOldHavenList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconPersonalList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconRaresList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconTokunoIslandsList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconTreasureList);
			MapIcon.AllListOfBuilds.Add(MapIcon.IconTreasurePFList);

			Region.Load("guardlines.def");
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

			//if (Engine.Running)
			//{
			//	UnLockItem();
			//	Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.Text = "OFFLINE"));
			//	Assistant.Engine.MainWindow.MapLinkStatusLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkStatusLabel.ForeColor = Color.Red));
			//}
		}

		internal static void LockItem()
		{
			//if (Engine.Running)
			//{
			//	Assistant.Engine.MainWindow.MapConnectButton.Invoke(new Action(() => Assistant.Engine.MainWindow.MapConnectButton.Enabled = false));
			//	Assistant.Engine.MainWindow.MapServerAddressTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerAddressTextBox.Enabled = false));
			//	Assistant.Engine.MainWindow.MapServerPortTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerPortTextBox.Enabled = false));
			//	Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Enabled = false));
			//	Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Enabled = false));
			//}
		}

		internal static void UnLockItem()
		{
			//if (Engine.Running)
			//{
			//	Assistant.Engine.MainWindow.MapConnectButton.Invoke(new Action(() => Assistant.Engine.MainWindow.MapConnectButton.Enabled = true));
			//	Assistant.Engine.MainWindow.MapServerAddressTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerAddressTextBox.Enabled = true));
			//	Assistant.Engine.MainWindow.MapServerPortTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapServerPortTextBox.Enabled = true));
			//	Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkUsernameTextBox.Enabled = true));
			//	Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.MapLinkPasswordTextBox.Enabled = true));
			//}
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