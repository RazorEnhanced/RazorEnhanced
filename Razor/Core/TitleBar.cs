using System;
using System.Collections.Generic;
using System.Text;
using RazorEnhanced;


namespace Assistant
{
	internal class TitleBar
	{
		private static string EncodeColorStat(int val, int max)
		{
			double perc = ((double)val) / ((double)max);

			if (perc <= 0.25)
				return String.Format(" ~#FF0000{0}~#~", val);
			else if (perc <= 0.75)
				return String.Format(" ~#FFFF00{0}~#~", val);
			else
				return val.ToString();
		}

		private static StringBuilder m_TBBuilder = new StringBuilder();
		internal static void UpdateTitleBar()
		{
			if (!Assistant.Client.Instance.Ready || World.Player == null)
				return;

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
			if (Engine.MainWindow.ShowHitsToolBarCheckBox.Checked)
			{
				if (World.Player.Poisoned)
					sb.Append(String.Format("H:~#FF8000{0}~#~/{1} ", World.Player.Hits, World.Player.HitsMax));
				else
					sb.Append(String.Format("H:{0}/{1} ", EncodeColorStat(World.Player.Hits, World.Player.HitsMax), World.Player.HitsMax));
			}

			// Mana
			if (Engine.MainWindow.ShowManaToolBarCheckBox.Checked)
				sb.Append(String.Format("M:{0}/{1} ", EncodeColorStat(World.Player.Mana, World.Player.ManaMax), World.Player.ManaMax));

			// Stam
			if (Engine.MainWindow.ShowStaminaToolBarCheckBox.Checked)
				sb.Append(String.Format("S:{0}/{1} ", EncodeColorStat(World.Player.Stam, World.Player.StamMax), World.Player.StamMax));

			// Follower
			if (Engine.MainWindow.ShowFollowerToolBarCheckBox.Checked)
				sb.Append(String.Format("F:{0}/{1} ", World.Player.Followers, World.Player.FollowersMax));

			// Weight
			if (Engine.MainWindow.ShowWeightToolBarCheckBox.Checked)
			{
				if (World.Player.Weight >= World.Player.MaxWeight)
					sb.Append(String.Format("W:~#FF0000{0}~#~/{1} ", World.Player.Weight, World.Player.MaxWeight));
				else
					sb.Append(String.Format("W:{0}/{1} ", World.Player.Weight, World.Player.MaxWeight));
			}

            // Tithe
            if (Engine.MainWindow.ShowTitheToolBarCheckBox.Checked)
            {
                    sb.Append(String.Format("T:{0} ", World.Player.Tithe));
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

			Assistant.Client.Instance.SetTitleStr(sb.ToString());
		}
	}
}
