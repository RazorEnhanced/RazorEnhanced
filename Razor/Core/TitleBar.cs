using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using RazorEnhanced;
<<<<<<< HEAD
using System.Drawing;
using System.Timers;
using System.Threading;
=======


>>>>>>> parent of a583e70... Fix titlebar drawing image for client > 7.0.35
namespace Assistant
{
	internal class TitleBar
	{
		private static string EncodeColorStat(int val, int max)
		{
<<<<<<< HEAD
			ClientHandle = clienthwnd;
			_hThemes = Native.LoadLibrary("UXTHEME.dll");
			init = true;
		}

		internal static void Draw()
		{
			while (true)
			{
				if (Assistant.World.Player == null)
				{
					Thread.Sleep(50);
					return;
				}

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
				Thread.Sleep(50);
			}
=======
			double perc = ((double)val) / ((double)max);

			if (perc <= 0.25)
				return String.Format(" ~#FF0000{0}~#~", val);
			else if (perc <= 0.75)
				return String.Format(" ~#FFFF00{0}~#~", val);
			else
				return val.ToString();
>>>>>>> parent of a583e70... Fix titlebar drawing image for client > 7.0.35
		}

		private static StringBuilder m_TBBuilder = new StringBuilder();
		internal static void UpdateTitleBar()
		{
			m_TBBuilder.Remove(0, m_TBBuilder.Length);
			StringBuilder sb = m_TBBuilder;

			sb.Append(String.Format("~#{0:X6}{1}~#~ ", World.Player.GetNotorietyColor() & 0x00FFFFFF, World.Player.Name)); // Nome Player

			// Blocco barre
			string statStr = String.Format("{0}{1:X2}{2:X2}{3:X2}", 
			(int)(World.Player.GetStatusCode()),
			(int)(World.Player.HitsMax == 0 ? 0 : (double)World.Player.Hits / World.Player.HitsMax * 99),
			(int)(World.Player.ManaMax == 0 ? 0 : (double)World.Player.Mana / World.Player.ManaMax * 99),
			(int)(World.Player.StamMax == 0 ? 0 : (double)World.Player.Stam / World.Player.StamMax * 99));
			sb.Append(String.Format("~SL{0}", statStr));

			// Hits
			if (Settings.General.ReadBool("ShowHitsToolBarCheckBox"))
			{
				if (World.Player.Poisoned)
					sb.Append(String.Format("H:~#FF8000{0}~#~/{1} ", World.Player.Hits, World.Player.HitsMax));
				else
					sb.Append(String.Format("H:{0}/{1} ", EncodeColorStat(World.Player.Hits, World.Player.HitsMax), World.Player.HitsMax));
			}

			// Mana
			if (Settings.General.ReadBool("ShowManaToolBarCheckBox"))
				sb.Append(String.Format("M:{0}/{1} ", EncodeColorStat(World.Player.Mana, World.Player.ManaMax), World.Player.ManaMax));

			// Stam
			if (Settings.General.ReadBool("ShowStaminaToolBarCheckBox"))
				sb.Append(String.Format("S:{0}/{1} ", EncodeColorStat(World.Player.Stam, World.Player.StamMax), World.Player.StamMax));

			// Follower
			if (Settings.General.ReadBool("ShowFollowerToolBarCheckBox"))
				sb.Append(String.Format("F:{0}/{1} ", World.Player.Followers, World.Player.FollowersMax));

			// Weight
			if (Settings.General.ReadBool("ShowWeightToolBarCheckBox"))
			{
				if (World.Player.Weight >= World.Player.MaxWeight)
					sb.Append(String.Format("W:~#FF0000{0}~#~/{1} ", World.Player.Weight, World.Player.MaxWeight));
				else
					sb.Append(String.Format("W:{0}/{1} ", World.Player.Weight, World.Player.MaxWeight));
			}

			sb.Append("  ");

			List <RazorEnhanced.ToolBar.ToolBarItem> items = Settings.Toolbar.ReadItems();

			foreach (RazorEnhanced.ToolBar.ToolBarItem item in items)
			{
				if (item.Graphics == 0)
					continue;

				StringBuilder sbitem = new StringBuilder();
				sbitem.AppendFormat("~I{0:X4}", item.Graphics);
				if (item.Color > 0 && item.Color < 0xFFFF)
					sbitem.Append(item.Color.ToString("X4"));
				else
					sbitem.Append('~');
				sbitem.Append(": ");

				int amount = Items.BackpackCount(item.Graphics, item.Color);
				if (item.Warning && amount <= item.WarningLimit)
				{
					sbitem.AppendFormat("~#FF0000{0}~#~", amount);
				}
				else
					sbitem.Append(amount.ToString());

				sb.Append(String.Format("{0} ",sbitem.ToString()));
			}

<<<<<<< HEAD
		// Timer update
		private static Thread m_update_titlebar;
		internal static void Start()
		{
			m_update_titlebar = new Thread(new ThreadStart(Draw));
			m_update_titlebar.Start();
		}
		internal static void Stop()
		{
			try
			{
				m_update_titlebar.Abort();
			}
			catch { }
=======
			ClientCommunication.SetTitleStr(sb.ToString());
>>>>>>> parent of a583e70... Fix titlebar drawing image for client > 7.0.35
		}
	}
}