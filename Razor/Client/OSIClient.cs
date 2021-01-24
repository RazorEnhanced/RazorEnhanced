using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Linq;

namespace Assistant
{

	internal unsafe sealed class OSIClient : Client
	{
		private const int WM_CUSTOMTITLE = WM_USER + 2;

		internal enum UONetMessage
		{
			Send = 1,
			Recv = 2,
			Ready = 3,
			NotReady = 4,
			Connect = 5,
			Disconnect = 6,
			KeyDown = 7,
			Mouse = 8,
			Activate = 9,
			Focus = 10,
			Close = 11,
			StatBar = 12,
			NotoHue = 13,
			DLL_Error = 14,
			DeathMsg = 15,
			OpenRPV = 18,
			SetGameSize = 19,
			FindData = 20,
			SmartCPU = 21,
			Negotiate = 22,
			SetMapHWnd = 23,
			DwmFree = 25
		}

		// uoa = user+3

		private enum InitError
		{
			SUCCESS,
			NO_UOWND,
			NO_TID,
			NO_HOOK,
			NO_SHAREMEM,
			LIB_DISABLED,
			NO_PATCH,
			NO_MEMCOPY,
			INVALID_PARAMS,

			UNKNOWN,
		}

		private const int SHARED_BUFF_SIZE = 524288; // 262144; // 250k

		[StructLayout(LayoutKind.Explicit, Size = 8 + SHARED_BUFF_SIZE)]
		private struct Buffer
		{
			[FieldOffset(0)]
			internal int Length;

			[FieldOffset(4)]
			internal int Start;

			[FieldOffset(8)]
			internal byte Buff0;
		}


		internal static string GetWindowsUserName()
		{
			int len = 1024;
			StringBuilder sb = new StringBuilder(len);
			return DLLImport.Win.GetUserNameA(sb, &len) != 0 ? sb.ToString() : "";
		}

		private static ConcurrentQueue<Packet> m_SendQueue;
		private static ConcurrentQueue<Packet> m_RecvQueue;

		private static volatile bool m_QueueRecv;
		private static volatile bool m_QueueSend;
		private static volatile bool m_ScriptWaitSend;
		private static volatile bool m_ScriptWaitRecv;

		private static Buffer* m_InRecv;

		private static Buffer* m_OutRecv;
		private static Buffer* m_InSend;
		private static Buffer* m_OutSend;
		private unsafe static byte* m_TitleStr;
		private static Mutex CommMutex;

		private static Process ClientProc;

        private static bool m_Ready = false;
		public override bool Ready { get { return m_Ready; } }

		public override Process ClientProcess { get { return ClientProc; } }

		public override bool ClientRunning
		{
			get
			{
				try
				{
					return ClientProc != null && !ClientProc.HasExited;
				}
				catch
				{
					return ClientProc != null && Assistant.Client.Instance.GetWindowHandle() != IntPtr.Zero;
				}
			}
		}

		static OSIClient()
		{
			Client.IsOSI = true;
			m_SendQueue = new ConcurrentQueue<Packet>();
			m_RecvQueue = new ConcurrentQueue<Packet>();
			//m_NextCmdID = 1425u;
			m_ClientEnc = false;
			m_ServerEnc = false;
			m_CalTimer = null;
			m_CalibrateNow = new TimerCallback(CalibrateNow);
			m_CalPos = Point2D.Zero;
			m_DwmTimer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTick), null, TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0));
		}

		private static System.Threading.Timer m_DwmTimer;

		private static void OnTick(object state)
		{
			if (!m_Ready)
			{
				return;
			}
			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.DwmFree, IntPtr.Zero);
		}

		public override void SetMapWndHandle(Form mapWnd)
		{
			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.SetMapHWnd, mapWnd.Handle);
		}

		public override void RequestStatbarPatch(bool preAOS)
		{
			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.StatBar, preAOS ? (IntPtr)1 : IntPtr.Zero);
		}

		public override void SetCustomNotoHue(int hue)
		{
			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.NotoHue, (IntPtr)hue);
		}
		public override void RunUI()
		{
			Engine.MainWnd = new MainForm();
            //if (!Assistant.Client.IsOSI)
            //{
            //    Engine.MainWnd.g  generalTab.Controls  smartCPU .Enabled = false;
            //    this.smartCPU.Text = "Smart CPU not available with Classic UO";
            //}
            Application.Run(Engine.MainWnd);
		}
		public override RazorEnhanced.Shard SelectShard(List<RazorEnhanced.Shard> shards)
		{
			return shards.FirstOrDefault(s => s.Selected);
		}


		public override void SetSmartCPU(bool enabled)
		{
			if (enabled)
				try { 	Assistant.Client.Instance.ClientProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal; }
				catch { }

			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.SmartCPU, (IntPtr)(enabled ? 1 : 0));
		}

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public new static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public override void SetGameSize(int x, int y)
        {
            //const int HWND_TOP = 0;
            //const short SWP_NOMOVE = 0x0002;
            //const short SWP_NOZORDER = 0x0004;

            DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.SetGameSize, (IntPtr)((x & 0xFFFF) | ((y & 0xFFFF) << 16)));
            // resizes the game size, not the internal size, so have to exit and restart unless I find smarter way
            //if (x != 0)
            //{
            //    SetWindowPos(Assistant.Client.Instance.GetWindowHandle(),
            //        HWND_TOP,
            //        0, 0,
            //        x, y,
            //        SWP_NOMOVE | SWP_NOZORDER);
            //}
        }

    public override Loader_Error LaunchClient(string client)
		{
			string dll = Path.Combine(Assistant.Engine.RootPath, "Crypt.dll");
			uint pid = 0;
			Loader_Error err = (Loader_Error)DLLImport.Razor.Load(client, dll, "OnAttach", null, 0, out pid);

			if (err == Loader_Error.SUCCESS)
			{
				try
				{
					ClientProc = Process.GetProcessById((int)pid);
					if (ClientProc != null && !RazorEnhanced.Settings.General.ReadBool("SmartCPU"))
						ClientProc.PriorityClass = (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), RazorEnhanced.Settings.General.ReadString("ClientPrio"), true);
				}
				catch
				{
				}
			}

			return ClientProc == null ? Loader_Error.UNKNOWN_ERROR : err;
		}

		private static bool m_ClientEnc = false;
		public override bool ClientEncrypted { get { return m_ClientEnc; } set { m_ClientEnc = value; } }

		private static bool m_ServerEnc = false;
		public override bool ServerEncrypted { get { return m_ServerEnc; } set { m_ServerEnc = value; } }

		public override bool InstallHooks(IntPtr mainWindow)
		{
			InitError error;
			int flags = 0;

			if (RazorEnhanced.Settings.General.ReadBool("Negotiate"))
				flags |= 0x04;

			if (ClientEncrypted)
				flags |= 0x08;

			if (Assistant.Client.Instance.ServerEncrypted)
				flags |= 0x10;

			//ClientProc.WaitForInputIdle();
			DLLImport.Razor.WaitForWindow(ClientProc.Id);

			error = (InitError)DLLImport.Razor.InstallLibrary(mainWindow, ClientProc.Id, flags);

			if (error != InitError.SUCCESS)
			{
				FatalInit(error);
				return false;
			}

			byte* baseAddr = (byte*)DLLImport.Razor.GetSharedAddress().ToPointer();

			// ZIPPY REV 80
			/*m_OutFwd = (Buffer*)baseAddr;
			m_InRecv = (Buffer*)(baseAddr+sizeof(Buffer)*1);
			m_OutRecv = (Buffer*)(baseAddr+sizeof(Buffer)*2);
			m_InSend = (Buffer*)(baseAddr+sizeof(Buffer)*3);
			m_OutSend = (Buffer*)(baseAddr+sizeof(Buffer)*4);
			m_TitleStr = (byte*)(baseAddr+sizeof(Buffer)*5);*/

			m_InRecv = (Buffer*)baseAddr;
			m_OutRecv = (Buffer*)(baseAddr + sizeof(Buffer));
			m_InSend = (Buffer*)(baseAddr + sizeof(Buffer) * 2);
			m_OutSend = (Buffer*)(baseAddr + sizeof(Buffer) * 3);
			m_TitleStr = baseAddr + sizeof(Buffer) * 4;

			DLLImport.Razor.SetServer(m_ServerIP, m_ServerPort);

			CommMutex = new Mutex {SafeWaitHandle = (new SafeWaitHandle(DLLImport.Razor.GetCommMutex(), true))};

			try
			{
				string path = Ultima.Files.GetFilePath("art.mul");
				if (!string.IsNullOrEmpty(path))
					DLLImport.Razor.SetDataPath(Path.GetDirectoryName(path));
				else
					DLLImport.Razor.SetDataPath(Path.GetDirectoryName(Ultima.Files.Directory));
			}
			catch
			{
				DLLImport.Razor.SetDataPath("");
			}

			if (RazorEnhanced.Settings.General.ReadBool("OldStatBar"))
			 	Assistant.Client.Instance.RequestStatbarPatch(true);

			return true;
		}

		private static uint m_ServerIP;
		private static ushort m_ServerPort;

		public override void SetConnectionInfo(IPAddress addr, int port)
		{
			byte[] ipBytes = addr.GetAddressBytes();
			uint ip = (uint)ipBytes[3] << 24;
			ip += (uint)ipBytes[2] << 16;
			ip += (uint)ipBytes[1] << 8;
			ip += (uint)ipBytes[0];
			m_ServerIP = ip;

			m_ServerPort = (ushort)port;
		}

		public override void SetNegotiate(bool negotiate)
		{
			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.Negotiate, (IntPtr)(negotiate ? 1 : 0));
		}

		public override bool Attach(int pid)
		{
			ClientProc = null;
			ClientProc = Process.GetProcessById(pid);
			return ClientProc != null;
		}

		public override void Close()
		{
			base.Close();
			DLLImport.Razor.Shutdown(true);
			if (ClientProc != null && !ClientProc.HasExited)
				ClientProc.CloseMainWindow();
			ClientProc = null;
		}



		private static void CalibrateNow()
		{
			m_CalTimer = null;

			if (World.Player == null)
				return;

			PlayerData.ExternalZ = false;

			Point3D pos = World.Player.Position;

			if (pos != Point3D.Zero && m_CalPos == pos)
			{
				DLLImport.Razor.CalibratePosition(pos.X, pos.Y, pos.Z);
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.1));
			}

			m_CalPos = Point2D.Zero;

			PlayerData.ExternalZ = true;
		}

		internal static Timer m_CalTimer = null;
		private static TimerCallback m_CalibrateNow = new TimerCallback(CalibrateNow);
		private static Point2D m_CalPos = Point2D.Zero;

		public override void BeginCalibratePosition()
		{
			if (World.Player == null || DLLImport.Razor.IsCalibrated())
				return;

			if (m_CalTimer != null)
				m_CalTimer.Stop();

			m_CalPos = new Point2D(World.Player.Position);

			m_CalTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(0.5), m_CalibrateNow);
			m_CalTimer.Start();
		}

		private static void FatalInit(InitError error)
		{
			StringBuilder sb = new StringBuilder(Language.GetString(LocString.InitError));
			sb.AppendFormat("{0}\n", error);
			sb.Append(Language.GetString((int)(LocString.InitError + (int)error)));

			//MessageBox.Show(Engine.ActiveWindow, sb.ToString(), "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			MessageBox.Show(sb.ToString(), "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		internal void OnLogout()
		{
			OnLogout(true);
		}

		private void OnLogout(bool fake)
		{
			if (!fake)
			{
				PacketHandlers.Party.Clear();

				Engine.MainWindow.UpdateTitle();
				// Felix Fix
				//foreach (WndRegEnt t in m_WndReg)
				//	DLLImport.Win.PostMessage((IntPtr)((WndRegEnt)t).Handle, (uint)Assistant.UOAssist.UOAMessage.LOGOUT, IntPtr.Zero, IntPtr.Zero);
				m_ConnectionStart = DateTime.MinValue;
			}

		 	Assistant.Client.Instance.SetTitleStr(""); // Restore titlebar standard

			if (World.Player != null)
			{
				// Stop forzato di tutti i thread agent
				RazorEnhanced.AutoLoot.AutoMode = false;
				RazorEnhanced.Scavenger.AutoMode = false;
				RazorEnhanced.BandageHeal.AutoMode = false;

				if (RazorEnhanced.Scripts.Timer != null)
					RazorEnhanced.Scripts.Timer.Close();

				if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
					Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false;

				if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
					Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = false;

				if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
					Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false;

				if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true)
					Assistant.Engine.MainWindow.OrganizerStop.PerformClick();

				if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
					Assistant.Engine.MainWindow.DressStopButton.PerformClick();

				if (Assistant.Engine.MainWindow.RestockStop.Enabled == true)
					Assistant.Engine.MainWindow.RestockStop.PerformClick();

				if (Assistant.Engine.MainWindow.SellCheckBox.Checked == true)
					Assistant.Engine.MainWindow.SellCheckBox.Checked = false;

				if (Assistant.Engine.MainWindow.BuyCheckBox.Checked == true)
					Assistant.Engine.MainWindow.BuyCheckBox.Checked = false;

				if (RazorEnhanced.ToolBar.ToolBarForm != null)
					RazorEnhanced.ToolBar.ToolBarForm.Close();

				if (RazorEnhanced.SpellGrid.SpellGridForm != null)
					RazorEnhanced.SpellGrid.SpellGridForm.Close();

				//Stop video recorder
				Assistant.MainForm.StopVideoRecorder();
			}

			PlayerData.ExternalZ = false;
			World.Player = null;
			PlayerData.FastWalkKey = 0;
			World.Items.Clear();
			World.Mobiles.Clear();
			ActionQueue.Stop();
			StealthSteps.Unhide();

			PacketHandlers.Party.Clear();
			PacketHandlers.IgnoreGumps.Clear();


		}

		[DllImport("user32.dll")]
		static new extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public override bool OnMessage(MainForm razor, uint wParam, int lParam)
		{
			bool retVal = true;
			bool m_hidetoolbar = false;
			bool m_hidespellgrid = false;

			switch ((UONetMessage)(wParam & 0xFFFF))
			{
				case UONetMessage.OpenRPV:
					{
						if (Engine.MainWindow != null)
						{
							StringBuilder sb = new StringBuilder(256);
							if (DLLImport.Win.GlobalGetAtomName((ushort)lParam, sb, 256) == 0)
								return false;

							BringToFront(Assistant.Client.Instance.GetWindowHandle());
							Engine.MainWindow.ShowMe();
						}
						break;
					}
				case UONetMessage.Ready: //Patch status
					if (lParam == (int)InitError.NO_MEMCOPY)
					{
						if (MessageBox.Show(Engine.ActiveWindow, Language.GetString(LocString.NoMemCpy), "No Client MemCopy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
						{
							m_Ready = false;
							ClientProc = null;
							Engine.MainWindow.CanClose = true;
							Engine.MainWindow.Close();
							break;
						}
					}

					try
					{
						DLLImport.Razor.SetDataPath(Ultima.Files.Directory);
					}
					catch
					{
						DLLImport.Razor.SetDataPath("");
					}
					UoMod.InjectUoMod();
					m_Ready = true;
					break;

				case UONetMessage.NotReady:
					m_Ready = false;
					FatalInit((InitError)lParam);
					ClientProc = null;
					Engine.MainWindow.CanClose = true;
					Engine.MainWindow.Close();
					break;

				// Network events
				case UONetMessage.Recv:
					OnRecv();
					break;

				case UONetMessage.Send:
					OnSend();
					break;

				case UONetMessage.Connect:
					m_ConnectionStart = DateTime.UtcNow;
					try
					{
						m_LastConnection = new IPAddress((uint)lParam);
					}
					catch
					{
					}
					break;

				case UONetMessage.Disconnect:
					OnLogout(false);
					break;

				case UONetMessage.Close:
					OnLogout();
					ClientProc = null;
					Engine.MainWindow.CanClose = true;
					Engine.MainWindow.Close();
					break;

				// Hot Keys
				case UONetMessage.Mouse:
					RazorEnhanced.HotKey.OnMouse((ushort)(lParam & 0xFFFF), (short)(lParam >> 16));
					break;

				case UONetMessage.KeyDown:
					//retVal = HotKey.OnKeyDown(lParam);
					retVal = RazorEnhanced.HotKey.KeyDown((Keys)(lParam));
					break;

				// Activation Tracking
				case UONetMessage.Activate:
					if ((lParam & 0x0000FFFF) == 0 && (lParam & 0xFFFF0000) != 0)
					{
						if (RazorEnhanced.ToolBar.ToolBarForm != null)
							RazorEnhanced.ToolBar.ToolBarForm.Hide();

						if (RazorEnhanced.SpellGrid.SpellGridForm != null)
							RazorEnhanced.SpellGrid.SpellGridForm.Hide();
					}
					else
					{
						if (lParam == 0 || lParam == 2097153) //2097153 riduzione a icona da win10 non perde il focus ma riduce
						{
							if (RazorEnhanced.ToolBar.ToolBarForm != null)
							{
								if (Cursor.Position.X >= RazorEnhanced.ToolBar.ToolBarForm.Location.X && Cursor.Position.X <= (RazorEnhanced.ToolBar.ToolBarForm.Location.X + RazorEnhanced.ToolBar.ToolBarForm.Width) && Cursor.Position.Y >= RazorEnhanced.ToolBar.ToolBarForm.Location.Y && Cursor.Position.Y <= (RazorEnhanced.ToolBar.ToolBarForm.Location.Y + RazorEnhanced.ToolBar.ToolBarForm.Height))
									break;
								m_hidetoolbar = true;
							}
							if (RazorEnhanced.SpellGrid.SpellGridForm != null)
							{
								if (Cursor.Position.X >= RazorEnhanced.SpellGrid.SpellGridForm.Location.X && Cursor.Position.X <= (RazorEnhanced.SpellGrid.SpellGridForm.Location.X + RazorEnhanced.SpellGrid.SpellGridForm.Width) && Cursor.Position.Y >= RazorEnhanced.SpellGrid.SpellGridForm.Location.Y && Cursor.Position.Y <= (RazorEnhanced.SpellGrid.SpellGridForm.Location.Y + RazorEnhanced.SpellGrid.SpellGridForm.Height))
									break;
								m_hidespellgrid = true;
							}

							if (m_hidetoolbar)
								RazorEnhanced.ToolBar.ToolBarForm.Hide();
							if (m_hidespellgrid)
								RazorEnhanced.SpellGrid.SpellGridForm.Hide();
						}
						else
						{
							if (RazorEnhanced.ToolBar.ToolBarForm != null)
								DLLImport.Win.ShowWindow(RazorEnhanced.ToolBar.ToolBarForm.Handle, 8);

							if (RazorEnhanced.SpellGrid.SpellGridForm != null)
								DLLImport.Win.ShowWindow(RazorEnhanced.SpellGrid.SpellGridForm.Handle, 8);

							DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
						}
					}

					/*if ( Config.GetBool( "AlwaysOnTop" ) )
					{
						if ( (lParam&0x0000FFFF) == 0 && (lParam&0xFFFF0000) != 0 && razor.WindowState != FormWindowState.Minimized && razor.Visible )
						{// if uo is deactivating and minimized and we are not minimized
							if ( !razor.ShowInTaskbar && razor.Visible )
								razor.Hide();
							razor.WindowState = FormWindowState.Minimized;
							m_LastActivate = DateTime.Now;
						}
						else if ( (lParam&0x0000FFFF) != 0 && (lParam&0xFFFF0000) != 0 && razor.WindowState != FormWindowState.Normal )
						{ // is UO is activating and minimized and we are minimized
							if ( m_LastActivate+TimeSpan.FromSeconds( 0.2 ) < DateTime.Now )
							{
								if ( !razor.ShowInTaskbar && !razor.Visible )
									razor.Show();
								razor.WindowState = FormWindowState.Normal;
								//SetForegroundWindow( FindUOWindow() );
							}
							m_LastActivate = DateTime.Now;
						}
					}*/
					break;

				case UONetMessage.Focus:
					if (RazorEnhanced.Settings.General.ReadBool("AlwaysOnTop"))
					{
						if (lParam != 0 && !razor.TopMost)
						{
							razor.TopMost = true;
							DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
						}
						else if (lParam == 0 && razor.TopMost)
						{
							razor.TopMost = false;
							razor.SendToBack();
						}
					}

					break;

				case UONetMessage.DLL_Error:
					{
						string error = "Unknown";
						switch ((UONetMessage)lParam)
						{
							case UONetMessage.StatBar:
								error = "Unable to patch status bar.";
								break;
						}

						MessageBox.Show(Engine.ActiveWindow, "An Error has occured : \n" + error, "Error Reported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
					}

				//case UONetMessage.FindData:
				//	FindData.Message((wParam & 0xFFFF0000) >> 16, lParam);
				//	break;

				// Unknown
				default:
					//MessageBox.Show(Engine.ActiveWindow, "Unknown message from uo client\n" + ((int)wParam).ToString(), "Error?");
					break;
			}

			return retVal;
		}


		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct CopyData
		{
			public int dwData;
			public int cbDAta;
			public IntPtr lpData;
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct Position
		{
			public ushort x;
			public ushort y;
			public ushort z;
		};

		public enum UONetMessageCopyData
		{
			Position = 1,
		}

		public override bool OnCopyData(IntPtr wparam, IntPtr lparam)
		{
			CopyData copydata = (CopyData)Marshal.PtrToStructure(lparam, typeof(CopyData));

			switch ((UONetMessageCopyData)copydata.dwData)
			{
				case UONetMessageCopyData.Position:
					if (World.Player != null)
					{
						Position pos = (Position)Marshal.PtrToStructure(copydata.lpData, typeof(Position));
						Point3D pt = new Point3D();

						pt.X = pos.x;
						pt.Y = pos.y;
						pt.Z = pos.z;

						World.Player.Position = pt;
					}

					return true;
			}

			return false;
		}

		public override unsafe void SendToServer(Packet p)
		{
			if (!m_Ready)
				return;


            if (m_QueueSend || m_ScriptWaitSend)
			{
				m_SendQueue.Enqueue(p);
			}
			else
			{
				m_ScriptWaitSend = true;
				ForceSendToServer(p);
				m_ScriptWaitSend = false;
			}
        }

		public override unsafe void SendToServerWait(Packet p)
		{
			if (!m_Ready)
				return;

			DateTime exittime = DateTime.Now + TimeSpan.FromSeconds(2);
			while (m_ScriptWaitSend || m_QueueSend)
			{
				if (DateTime.Now > exittime)
				{
					break;
				}
			}

			m_ScriptWaitSend = true;
			if (!m_QueueSend)
			{
				ForceSendToServer(p);
			}
			else
			{
				m_SendQueue.Enqueue(p);
			}
			m_ScriptWaitSend = false;
		}

		public override unsafe void SendToServer(PacketReader pr)
		{
			if (!m_Ready)
				return;

			SendToServer(MakePacketFrom(pr));
		}

		public override unsafe void SendToClientWait(Packet p)
		{
			if (!m_Ready || p.Length <= 0)
				return;

			DateTime exittime = DateTime.Now + TimeSpan.FromSeconds(2);
			while (m_ScriptWaitRecv || m_QueueRecv)
			{
				if (DateTime.Now > exittime)
				{
					break;
				}
			}

			m_ScriptWaitRecv = true;
			if (!m_QueueRecv)
			{
				ForceSendToClient(p);
			}
			else
			{
				m_RecvQueue.Enqueue(p);
			}
			m_ScriptWaitRecv = false;
		}


		public override void SendToClient(Packet p)
		{
			if (!m_Ready || p.Length <= 0)
				return;

            if (m_QueueRecv || m_ScriptWaitRecv)
			{
				m_RecvQueue.Enqueue(p);
			}
			else
			{
				m_ScriptWaitRecv = true;
				ForceSendToClient(p);
				m_ScriptWaitRecv = false;
			}
		}


		public override void ForceSendToClient(Packet p)
		{
			byte[] data = p.Compile();

			try  // AbandonedMutexException
			{
				CommMutex.WaitOne();
				fixed (byte* ptr = data)
				{
					//Packet.Log(PacketPath.RazorToClient, ptr, data.Length);
					CopyToBuffer(m_OutRecv, ptr, data.Length);
				}
			}
			catch { }

			CommMutex.ReleaseMutex();
		}

		public override void ForceSendToServer(Packet p)
		{
			if (p == null || p.Length <= 0)
				return;

			byte[] data = p.Compile();

			try  // AbandonedMutexException
			{
				CommMutex.WaitOne();
				InitSendFlush();
				fixed (byte* ptr = data)
				{
					//Packet.Log(PacketPath.RazorToServer, ptr, data.Length);
					CopyToBuffer(m_OutSend, ptr, data.Length);
				}
			}
			catch { }
			CommMutex.ReleaseMutex();
		}

		public override void InitSendFlush()
		{
			if (m_OutSend->Length == 0)
				DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_UONETEVENT, (IntPtr)UONetMessage.Send, IntPtr.Zero);
		}

		private void CopyToBuffer(Buffer* buffer, byte* data, int len)
		{
			//if ( buffer->Length + buffer->Start + len >= SHARED_BUFF_SIZE )
			//	throw new NullReferenceException( String.Format( "Buffer OVERFLOW in CopyToBuffer [{0} + {1}] <- {2}", buffer->Start, buffer->Length, len ) );

			/*		IntPtr to = (IntPtr)(&buffer->Buff0 + buffer->Start + buffer->Length);
					IntPtr from = (IntPtr)data;
					DLLImport.Win.memcpy(to, from, new UIntPtr((uint)len));
					buffer->Length += len;*/

			DLLImport.Win.memcpy((&buffer->Buff0) + buffer->Start + buffer->Length, data, len);
			buffer->Length += len;
		}


		private unsafe void HandleComm(Buffer* inBuff, Buffer* outBuff, ConcurrentQueue<Packet> queue, PacketPath path)
		{
			CommMutex.WaitOne();
			while (inBuff->Length > 0)
			{
				byte* buff = (&inBuff->Buff0) + inBuff->Start;

				int len = DLLImport.Razor.GetPacketLength(buff, inBuff->Length);
				if (len > inBuff->Length || len <= 0)
					break;

				inBuff->Start += len;
				inBuff->Length -= len;

				bool viewer = false;
				bool filter = false;

				switch (path)
				{
					case PacketPath.ClientToServer:
						viewer = PacketHandler.HasClientViewer(buff[0]);
						filter = PacketHandler.HasClientFilter(buff[0]);
						break;

					case PacketPath.ServerToClient:
						viewer = PacketHandler.HasServerViewer(buff[0]);
						filter = PacketHandler.HasServerFilter(buff[0]);
						break;
				}

				Packet p = null;
				PacketReader pr = null;
				if (viewer)
				{
					pr = new PacketReader(buff, len, DLLImport.Razor.IsDynLength(buff[0]));
					if (filter)
						p = MakePacketFrom(pr);
				}
				else if (filter)
				{
					byte[] temp = new byte[len];
					fixed (byte* ptr = temp)
						DLLImport.Win.memcpy(ptr, buff, len);
					p = new Packet(temp, len, DLLImport.Razor.IsDynLength(buff[0]));

					/*byte[] temp = new byte[len];
					fixed (byte* ptr = temp)
					{
						IntPtr to = (IntPtr)ptr;
						IntPtr from = (IntPtr)buff;
						DLLImport.Win.memcpy(to, from, new UIntPtr((uint)len));
					}
					p = new Packet(temp, len, DLLImport.Razor.IsDynLength(buff[0]));
					*/

				}

				bool blocked = false;
				switch (path)
				{
					// yes it should be this way
					case PacketPath.ClientToServer:
						{
							blocked = PacketHandler.OnClientPacket(buff[0], pr, p);
							break;
						}
					case PacketPath.ServerToClient:
						{
							blocked = PacketHandler.OnServerPacket(buff[0], pr, p);
							break;
						}
				}


				if (filter)
				{
					byte[] data = p.Compile();
					if (data[0] == 0xc8 || data[0] == 0x73 || data[0] == 0xbf || data[0] == 0xdc)
					{
					}
					else
					{
						//int something = len;
						//Debug.WriteLine("Packet id 0x{0:X}", data[0]);
					}

					fixed (byte* ptr = data)
					{
						Packet.Log(path, ptr, data.Length, blocked);
						if (!blocked)
							CopyToBuffer(outBuff, ptr, data.Length);
					}
				}
				else
				{
					//Packet.Log(path, buff, len, blocked);
					if (!blocked)
						CopyToBuffer(outBuff, buff, len);
				}

				while (queue.Count > 0)
				{
					if (!queue.TryDequeue(out p))
						continue;

					byte[] data = p.Compile();
					fixed (byte* ptr = data)
					{
						CopyToBuffer(outBuff, ptr, data.Length);
						//Packet.Log((PacketPath)(((int)path) + 1), ptr, data.Length);
					}
				}
			}
			CommMutex.ReleaseMutex();
		}

		private void OnRecv()
		{
			m_ScriptWaitRecv = true;
			m_QueueRecv = true;
			HandleComm(m_InRecv, m_OutRecv, m_RecvQueue, PacketPath.ServerToClient);
			m_QueueRecv = false;
			m_ScriptWaitRecv = false;
        }

		private void OnSend()
		{
			m_ScriptWaitSend = true;
			m_QueueSend = true;
			HandleComm(m_InSend, m_OutSend, m_SendQueue, PacketPath.ClientToServer);
			m_QueueSend = false;
			m_ScriptWaitSend = false;
        }

		[DllImport("Crypt.dll")]
		internal static unsafe extern void CalibratePosition(uint x, uint y, uint z);

		public override void SetPosition(uint x, uint y, uint z, byte dir)
		{
			CalibratePosition(x, y, z);
		}

		[DllImport("user32.dll")]
		internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		public void KeyPress(int keyCode)
		{
            const uint WM_KEYDOWN = 0x100;
            //const uint WM_KEYUP = 0x101;
			SendMessage(FindUOWindow(), WM_KEYDOWN, (IntPtr)keyCode, (IntPtr)1);
		}

		[DllImport("Crypt.dll")]
		internal static unsafe extern string GetUOVersion();
		public override string GetClientVersion()
		{
			IntPtr version = DLLImport.Razor.GetUOVersion();
			string str = Marshal.PtrToStringAnsi(version);
			return str;
		}
		public override string GetUoFilePath()
		{
			return ConfigurationManager.AppSettings["UODataDir"];
		}

		[DllImport("Crypt.dll")]
		internal static unsafe extern IntPtr FindUOWindow();
		public override IntPtr GetWindowHandle()
		{
			return FindUOWindow();
		}

		[DllImport("Crypt.dll")]
		internal static unsafe extern uint TotalIn();
		public override uint TotalDataIn()
		{
			return TotalIn();
		}

		[DllImport("Crypt.dll")]
		internal static unsafe extern uint TotalOut();
		public override uint TotalDataOut()
		{
			return TotalOut();
		}
		private enum KeyboardDir
		{
			North = 0x21, //page up
			Right = 0x27, // right
			East = 0x22, // page down
			Down = 0x28, // down
			South = 0x23, // end
			Left = 0x25, // left
			West = 0x24, // home
			Up = 0x26, // up
		}

		internal override void RequestMove(Direction m_Dir)
		{
            int direction;

			switch (m_Dir)
			{
				case Direction.Down:
					direction = (int)KeyboardDir.Down;
					break;
				case Direction.East:
					direction = (int)KeyboardDir.East;
					break;
				case Direction.Left:
					direction = (int)KeyboardDir.Left;
					break;
				case Direction.North:
					direction = (int)KeyboardDir.North;
					break;
				case Direction.Right:
					direction = (int)KeyboardDir.Right;
					break;
				case Direction.South:
					direction = (int)KeyboardDir.South;
					break;
				case Direction.Up:
					direction = (int)KeyboardDir.Up;
					break;
				case Direction.West:
					direction = (int)KeyboardDir.West;
					break;
				default:
					direction = (int)KeyboardDir.Up;
					break;
			}

			KeyPress(direction);

        }
		public override void PathFindTo(Assistant.Point3D location)
		{
            // Uses EasyUO to do the move
            if (RazorEnhanced.UoWarper.UODLLHandleClass == null)
            {
                RazorEnhanced.UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();
            }

            if (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
            {
                while (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
                {
                    Thread.Sleep(50);
                }
            }
            RazorEnhanced.UoWarper.UODLLHandleClass.Pathfind (location.X, location.Y, location.Z);
		}

		// Titlebar
		private static string m_LastStr = string.Empty;

		public override void SetTitleStr(string str)
		{
			if (m_LastStr == str)
				return;

			m_LastStr = str;
			byte[] copy = System.Text.Encoding.ASCII.GetBytes(str);
			int clen = copy.Length;
			if (clen >= 512)
				clen = 511;

			CommMutex.WaitOne();
			if (clen > 0)
			{
				fixed (byte* array = copy)
					DLLImport.Win.memcpy(m_TitleStr, array, clen);
			}
			*(m_TitleStr + clen) = 0;
			CommMutex.ReleaseMutex();

			DLLImport.Win.PostMessage(Assistant.Client.Instance.GetWindowHandle(), WM_CUSTOMTITLE, IntPtr.Zero, IntPtr.Zero);
		}

	}
}
