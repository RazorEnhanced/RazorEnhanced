using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Assistant.Map
{
	internal partial class MapWindow : Form
	{
		internal const int WM_NCLBUTTONDOWN = 0xA1;
		internal const int HT_CAPTION = 0x2;

		internal static UOMapControl MapControl;
		internal static MapWindow MapWindowForm;
		internal static ContextMenuStrip MapContextMenu;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll")]
		internal static extern bool ReleaseCapture();

		public MapWindow()
		{
			InitializeComponent();

			MapControl = this.uoMapControl;
			MapWindowForm = this;
			MapContextMenu = this.contextMenuStrip;

			this.Location = new Point(RazorEnhanced.Settings.General.ReadInt("MapX"), RazorEnhanced.Settings.General.ReadInt("MapY"));
			this.ClientSize = new Size(RazorEnhanced.Settings.General.ReadInt("MapW"), RazorEnhanced.Settings.General.ReadInt("MapH"));
			Assistant.Engine.MapWindowX = RazorEnhanced.Settings.General.ReadInt("MapX");
			Assistant.Engine.MapWindowY = RazorEnhanced.Settings.General.ReadInt("MapY");
			Assistant.Engine.MapWindowH = RazorEnhanced.Settings.General.ReadInt("MapH");
			Assistant.Engine.MapWindowW = RazorEnhanced.Settings.General.ReadInt("MapW");

			if (this.Location.X < -10 || this.Location.Y < -10)
				this.Location = Point.Empty;

			if (this.Width < 50)
				this.Width = 50;
			if (this.Height < 50)
				this.Height = 50;

			ClientCommunication.SetMapWndHandle(this);	
		}

		internal class MapMenuItem : MenuItem
		{
			internal MapMenuItem(System.String text, System.EventHandler onClick)
				: base(text, onClick)
			{
				Tag = null;
			}
		}

		private void FocusChange(object sender, System.EventArgs e)
		{
			if (sender != null)
			{
				MapMenuItem mItem = sender as MapMenuItem;

				if (mItem != null)
				{
					Serial s = (Serial)mItem.Tag;
					Mobile m = World.FindMobile(s);
					MapControl.FocusMobile = m;
					MapControl.FullUpdate();
				}
			}
		}
		public static void Initialize()
		{
			new ReqPartyLocTimer().Start();
		}

		internal static void ToggleMap()
		{
			if (World.Player != null && Engine.MainWindow != null)
			{
				if (Engine.MainWindow.MapWindow == null)
				{
					Engine.MainWindow.MapWindow = new Assistant.Map.MapWindow();
					Engine.MainWindow.MapWindow.Show();
					Engine.MainWindow.MapWindow.BringToFront();
				}
				else
				{
					if (Engine.MainWindow.MapWindow.Visible)
					{
						Engine.MainWindow.MapWindow.Hide();
						Engine.MainWindow.BringToFront();
						ClientCommunication.BringToFront(ClientCommunication.FindUOWindow());
					}
					else
					{
						Engine.MainWindow.MapWindow.Show();
						Engine.MainWindow.MapWindow.BringToFront();
						Engine.MainWindow.MapWindow.TopMost = true;
						ClientCommunication.SetMapWndHandle(Engine.MainWindow.MapWindow);
					}
				}
			}
		}

		internal void CheckLocalUpdate(Mobile mob)
		{
			if (mob.InParty)
				MapControl.FullUpdate();
		}

		private class ReqPartyLocTimer : Timer
		{
			internal ReqPartyLocTimer()
				: base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
			{
			}

			protected override void OnTick()
			{
				// never send this packet to encrypted servers (could lead to OSI detecting razor)
				if (ClientCommunication.ServerEncrypted)
				{
					Stop();
					return;
				}

				if (Engine.MainWindow == null || Engine.MainWindow.MapWindow == null || !Engine.MainWindow.MapWindow.Visible)
					return; // don't bother when the map window isnt visible

				if (World.Player != null && PacketHandlers.Party.Count > 0)
				{
					if (PacketHandlers.SpecialPartySent > PacketHandlers.SpecialPartyReceived)
					{
						// If we sent more than we received then the server stopped responding
						// in that case, wait a long while before trying again
						PacketHandlers.SpecialPartySent = PacketHandlers.SpecialPartyReceived = 0;
						this.Interval = TimeSpan.FromSeconds(5.0);
						return;
					}
					else
					{
						this.Interval = TimeSpan.FromSeconds(1.0);
					}

					bool send = false;
					foreach (Serial s in PacketHandlers.Party)
					{
						Mobile m = World.FindMobile(s);

						if (m == World.Player)
							continue;

						if (m == null || Utility.Distance(World.Player.Position, m.Position) > World.Player.VisRange || !m.Visible)
						{
							send = true;
							break;
						}
					}

					if (send)
					{
						PacketHandlers.SpecialPartySent++;
						ClientCommunication.SendToServer(new QueryPartyLocs());
					}
				}
				else
				{
					this.Interval = TimeSpan.FromSeconds(1.0);
				}
			}
		}

		private void RequestPartyLocations()
		{
			if (World.Player != null && PacketHandlers.Party.Count > 0)
				ClientCommunication.SendToServer(new QueryPartyLocs());
		}

		internal void UpdateMap()
		{
			ClientCommunication.SetMapWndHandle(this);
			MapControl.UpdateMap();
		}

		internal void PlayerMoved()
		{
			if (this.Visible && MapControl != null)
				MapControl.FullUpdate();
		}

		private void MapWindow_Resize(object sender, System.EventArgs e)
		{
			MapControl.Height = this.Height;
			MapControl.Width = this.Width;

			if (this.Width < 50)
				this.Width = 50;
			if (this.Height < 50)
				this.Height = 50;

			this.Refresh();

			Assistant.Engine.MapWindowH = this.Height;
			Assistant.Engine.MapWindowW = this.Width;
		}

		private void MapWindow_Move(object sender, System.EventArgs e)
		{
			MapControl.Location = this.Location;
			Assistant.Engine.MapWindowX = this.Location.X;
			Assistant.Engine.MapWindowY = this.Location.Y;
		}

		private void MapWindow_Deactivate(object sender, System.EventArgs e)
		{
			if (this.TopMost)
				this.TopMost = false;
		}

		private void MapWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Assistant.Engine.Running)
			{
				e.Cancel = true;
				this.Hide();
				Engine.MainWindow.BringToFront();
				ClientCommunication.BringToFront(ClientCommunication.FindUOWindow());
			}
		}
	}
}
