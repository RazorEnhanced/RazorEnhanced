using System;
using System.Runtime.InteropServices;
using RazorEnhanced;
using System.Drawing;
using System.Timers;

namespace Assistant
{
	internal class TitleBar
	{

		private static IntPtr _hThemes;
		private static IntPtr ClientHandle { get; set; }

		private static bool init = false;
		private static void Init(IntPtr clienthwnd)
		{
			ClientHandle = clienthwnd;
			_hThemes = Native.LoadLibrary("UXTHEME.dll");
			init = true;
		}

		private static volatile bool m_drawing = false;

		internal static void Draw(object sender, ElapsedEventArgs e)
		{
			if (Assistant.World.Player == null)
				return;

			if (m_drawing)
				return;

			m_drawing = true;

			Check();
		

			Native.WindowPlacement place = new Native.WindowPlacement();
			Native.RECT rect = new Native.RECT();
			IntPtr hdc = Native.GetWindowDC(ClientHandle);
			Native.GetWindowPlacement(ClientHandle, ref place);
			Native.GetWindowRect(ClientHandle, out rect);
			rect.Top = Native.GetSystemMetrics(Native.SystemMetric.SM_CYFRAME);
			rect.Bottom = rect.Top + Native.GetSystemMetrics(Native.SystemMetric.SM_CYCAPTION);
			rect.Right = (rect.Right - rect.Left) - (4 * Native.GetSystemMetrics(Native.SystemMetric.SM_CXSIZE) + Native.GetSystemMetrics(Native.SystemMetric.SM_CXFRAME));
			rect.Left = Native.GetSystemMetrics(Native.SystemMetric.SM_CXSIZEFRAME) + Native.GetSystemMetrics(Native.SystemMetric.SM_CXSMICON) + 5;
			if (_hThemes != IntPtr.Zero)
			{
				IntPtr hthemes = Native.OpenThemeData(ClientHandle, "WINDOW");
				DrawBar(hthemes, ClientHandle, rect, hdc, place.showCmd == 3);
				Native.CloseThemeData(hthemes);
			}
			else
			{
				rect.Left += Native.GetSystemMetrics(Native.SystemMetric.SM_CXFRAME);
				DrawBar(IntPtr.Zero, ClientHandle, rect, hdc, place.showCmd == 3);
			}

			Native.ReleaseDC(ClientHandle, hdc);
			m_drawing = false;
		}

		private static Font m_standard_font = new Font("Arial", 9, FontStyle.Regular);
		private static Brush m_normal_font_color = Brushes.Black;
		private static Brush m_warning_font_color = Brushes.Red;
		private static Brush m_poison_font_color = Brushes.GreenYellow;
		private static Brush m_mortal_font_color = Brushes.Violet;
		private static Brush m_paral_font_color = Brushes.Yellow;
		private static SolidBrush m_bar_fill_brush = new SolidBrush(Color.Green);
		private static Pen m_bar_pen = new Pen(Color.Green, 1);
		private static void DrawBar(IntPtr htheme, IntPtr hWnd, Native.RECT orig, IntPtr hOutDC, bool ismaximixed, bool isactive = true)
		{
			Native.RECT rect = new Native.RECT();
			Native.RECT window = new Native.RECT();

			Native.GetWindowRect(hWnd, out window);
			rect.Left = rect.Top = 0;
			rect.Right = window.Right - window.Left;
			rect.Bottom = orig.Bottom - orig.Top;

			IntPtr hdc = Native.CreateCompatibleDC(hOutDC);

			IntPtr hbmp = Native.CreateCompatibleBitmap(hOutDC, rect.Right, rect.Bottom);
			Native.SelectObject(hdc, hbmp);

			bool needrefill = true;

			if (htheme != IntPtr.Zero)
			{
				int modtop = Native.GetSystemMetrics(Native.SystemMetric.SM_CYFRAME);
				rect.Top -= modtop;
				needrefill = Native.DrawThemeBackground(htheme, hdc, 1, isactive ? 1 : 2, ref rect, IntPtr.Zero) != 0;
				rect.Top += modtop;

				//if (ismaximixed)
				// {
				//     needrefill = Native.DrawThemeBackground(htheme, hdc, 5, isactive ? 1 : 2, ref rect, IntPtr.Zero) != 0;
				// }
				// else
				// {
				//     int modtop = Native.GetSystemMetrics(Native.SystemMetric.SM_CYFRAME);
				//     rect.Top -= modtop;
				//     needrefill = Native.DrawThemeBackground(htheme, hdc, 1, isactive ? 1 : 2, ref rect, IntPtr.Zero) != 0;
				//     rect.Top += modtop;
				// }
			}

			if (needrefill)
				Native.FillRect(hdc, ref rect, Native.GetSysColorBrush(isactive ? 2 : 3));
			int offset = 30;

			using (Graphics g = Graphics.FromHdc(hdc))
			{
				int percenthits = (int)(World.Player.Hits * 100 / (World.Player.HitsMax == 0 ? (ushort)1 : World.Player.HitsMax));
				int percentstam = (int)(World.Player.Stam * 100 / (World.Player.StamMax == 0 ? (ushort)1 : World.Player.StamMax));
				int percentmana = (int)(World.Player.Mana * 100 / (World.Player.ManaMax == 0 ? (ushort)1 : World.Player.ManaMax));

				// Nome
				if (World.Player.Poisoned)
					g.DrawString(World.Player.Name, m_standard_font, m_poison_font_color, offset, 6);
				else if (World.Player.Paralized)
					g.DrawString(World.Player.Name, m_standard_font, m_paral_font_color, offset, 6);
				else if (World.Player.Blessed)
					g.DrawString(World.Player.Name, m_standard_font, m_mortal_font_color, offset, 6);
				else
					g.DrawString(World.Player.Name, m_standard_font, m_normal_font_color, offset, 6);

				offset += MeasureDisplayStringWidth(g, World.Player.Name, m_standard_font);

				offset += 5; // Spaziatura da nome a barre

				// hits
				g.DrawRectangle(m_bar_pen, new Rectangle(offset,6, 71, 5)); 
				m_bar_fill_brush.Color = RazorEnhanced.ToolBar.GetColor(percenthits);
				g.FillRectangle(m_bar_fill_brush, new Rectangle(offset+1, 7, (int)(percenthits / 1.4), 4));

				// mana
				g.DrawRectangle(m_bar_pen, new Rectangle(offset, 11, 71, 5)); 
				m_bar_fill_brush.Color = RazorEnhanced.ToolBar.GetColor(percentmana);
				g.FillRectangle(m_bar_fill_brush, new Rectangle(offset + 1, 12, (int)(percentmana / 1.4) , 4));

				// stam
				g.DrawRectangle(m_bar_pen, new Rectangle(offset, 16, 71, 5)); 
				m_bar_fill_brush.Color = RazorEnhanced.ToolBar.GetColor(percentstam);
				g.FillRectangle(m_bar_fill_brush, new Rectangle(offset + 1, 17, (int)(percentstam / 1.4), 4));

				offset += 5 + 71; // Spaziatura da barre a valori stringa stat

				// Hits
				if (Settings.General.ReadBool("ShowHitsToolBarCheckBox"))
				{
					string hits = "H: " + World.Player.Hits + "/" + World.Player.HitsMax;
					g.DrawString(hits, m_standard_font, m_normal_font_color, offset, 6);
					offset += MeasureDisplayStringWidth(g, hits, m_standard_font) + 2;
				}

				// Mana
				if (Settings.General.ReadBool("ShowManaToolBarCheckBox"))
				{
					string mana = "M: " + World.Player.Mana + "/" + World.Player.ManaMax;
					g.DrawString(mana, m_standard_font, m_normal_font_color, offset, 6);
					offset += MeasureDisplayStringWidth(g, mana, m_standard_font) + 2;
				}

				// Stam
				if (Settings.General.ReadBool("ShowStaminaToolBarCheckBox"))
				{
					string stam = "S: " + World.Player.Stam + "/" + World.Player.StamMax;
					g.DrawString(stam, m_standard_font, m_normal_font_color, offset, 6);
					offset += MeasureDisplayStringWidth(g, stam, m_standard_font) + 2;
				}

				// Follower
				if (Settings.General.ReadBool("ShowFollowerToolBarCheckBox"))
				{
					string follower = "F: " + World.Player.Followers + "/" + World.Player.FollowersMax;
					g.DrawString(follower, m_standard_font, m_normal_font_color, offset, 6);
					offset += MeasureDisplayStringWidth(g, follower, m_standard_font) + 2;
				}

				// Weight
				if (Settings.General.ReadBool("ShowWeightToolBarCheckBox"))
				{
					string weight = "W: " + World.Player.Weight + "/" + World.Player.MaxWeight;
					if (World.Player.Weight >= World.Player.MaxWeight)
						g.DrawString(weight, m_standard_font, m_warning_font_color, offset, 6);
					else
						g.DrawString(weight, m_standard_font, m_normal_font_color, offset, 6);
					offset += MeasureDisplayStringWidth(g, weight, m_standard_font) + 2;

				}

				offset += 5; // Spazio fra stat e item
				
				// Contatori item
				foreach (RazorEnhanced.ToolBar.ToolBarItem item in Settings.Toolbar.ReadItems())
				{
					if (item.Graphics == 0)
						continue;

					using (Bitmap m_itemimage = GetImage(item.Graphics, item.Color))
					{
						if (m_itemimage.Height > 15)
						{
							g.DrawImage(m_itemimage, offset, 8, 15, 15);
							offset += 15;
						}
						else
						{
							g.DrawImage(m_itemimage, offset, 8, m_itemimage.Width, m_itemimage.Height);
							offset += m_itemimage.Width;
						}
					}

					int amount = Items.BackpackCount(item.Graphics, item.Color);
					if (item.Warning && amount <= item.WarningLimit)
					{
						g.DrawString(": "+amount.ToString(), m_standard_font, m_warning_font_color, offset, 6);
					}
					else
					{
						g.DrawString(": " + amount.ToString(), m_standard_font, m_normal_font_color, offset, 6);
					}

					offset += MeasureDisplayStringWidth(g, ": " + amount.ToString(), m_standard_font) + 5;
				}
			}

			Native.BitBlt(hOutDC, orig.Left, orig.Top, orig.Right - orig.Left, orig.Bottom - orig.Top - 1, hdc, orig.Left, 0, 0x00cc0020);
			Native.DeleteDC(hdc);
			Native.DeleteObject(hbmp);
		}

		private static void Check()
		{
			if (!init)
				Init(ClientCommunication.FindUOWindow());

			int policy = (int)Native.DWMNCRenderingPolicy.Disabled;
			Native.DwmSetWindowAttribute(ClientHandle, (int)Native.DwmWindowAttribute.DWMWA_NCRENDERING_POLICY, ref policy, Marshal.SizeOf(policy));
		}
		
		private static int MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
		{
			StringFormat format = new StringFormat();
			RectangleF rect = new RectangleF(0, 0, 1000, 1000);
			CharacterRange[] ranges = {new CharacterRange(0, text.Length) };
			Region[] regions = new Region[1];

			format.SetMeasurableCharacterRanges(ranges);

			regions = graphics.MeasureCharacterRanges(text, font, rect, format);
			rect = regions[0].GetBounds(graphics);

			return (int)(rect.Right + 1.0f);
		}

		private static Bitmap GetImage(int itemid, int color)
		{
			Bitmap itemimage = new Bitmap(Ultima.Art.GetStatic(itemid));
			if (color > 0)
			{
				bool onlyHueGrayPixels = (color & 0x8000) != 0;
				color = (color & 0x3FFF) - 1;
				Ultima.Hue m_hue = Ultima.Hues.GetHue(color);
				m_hue.ApplyTo(itemimage, onlyHueGrayPixels);
			}
			itemimage = RazorEnhanced.ToolBar.CropImage(itemimage);
			return itemimage;
		}

		// Timer update
		internal static System.Timers.Timer UpdateTimer;
		internal static void Start()
		{
			m_drawing = false;
			UpdateTimer = new System.Timers.Timer(100);
			UpdateTimer.Elapsed += new ElapsedEventHandler(Draw);
			UpdateTimer.Enabled = true;
			UpdateTimer.Start();
		}
		internal static void Stop()
		{
			m_drawing = false;
			if (UpdateTimer != null)
				UpdateTimer.Dispose();
		}
	}
}