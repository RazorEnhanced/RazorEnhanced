using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class TargetGUI
	{
		[Serializable]
		public class TargetGUIObject
		{
			private string m_Selector;
			public string Selector { get { return m_Selector; } }

			private Mobiles.Filter m_Filter;
			public Mobiles.Filter Filter { get { return m_Filter; } }

			public TargetGUIObject(string selector, Mobiles.Filter filter)
			{
				m_Selector = selector;
				m_Filter = filter;
			}
		}

		public class TargetGUIObjectList
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private TargetGUIObject m_TargetObject;
			public TargetGUIObject TargetObject { get { return m_TargetObject; } }

			public TargetGUIObjectList(string name, TargetGUIObject targetobject)
			{
				m_Name = name;
				m_TargetObject = targetobject;
			}
		}

		internal static void RefreshTarget()
		{
			Assistant.Engine.MainWindow.TargetListView.Items.Clear();
			List<TargetGUI.TargetGUIObjectList> list = Settings.Target.ReadAll();
			foreach (TargetGUIObjectList target in list)
			{
				ListViewItem listitem = new ListViewItem();

				// Target ID
				listitem.SubItems.Add(target.Name);

				// Body List
				if (target.TargetObject.Filter.Bodies.Count > 0)
				{
					string bodylist = "";
					foreach (int body in target.TargetObject.Filter.Bodies)
					{
						bodylist = bodylist + body.ToString("X4") + " - ";
					}
					listitem.SubItems.Add(bodylist);
				}
				else
				{
					listitem.SubItems.Add("*");
				}

				// Target Name
				if (target.TargetObject.Filter.Name != "")
					listitem.SubItems.Add(target.TargetObject.Filter.Name);
				else
					listitem.SubItems.Add("*");

				// Hue List
				if (target.TargetObject.Filter.Hues.Count > 0)
				{
					string huelist = "";
					foreach (int hue in target.TargetObject.Filter.Hues)
					{
						huelist = huelist + "0x" + hue.ToString("X4") + " - ";
					}
					listitem.SubItems.Add(huelist);
				}
				else
				{
					listitem.SubItems.Add("*");
				}

				// Min Range
				if (target.TargetObject.Filter.RangeMin != -1)
					listitem.SubItems.Add(target.TargetObject.Filter.RangeMin.ToString());
				else
					listitem.SubItems.Add("*");

				// Max Range
				if (target.TargetObject.Filter.RangeMax != -1)
					listitem.SubItems.Add(target.TargetObject.Filter.RangeMax.ToString());
				else
					listitem.SubItems.Add("*");

				// Poisoned
				if (target.TargetObject.Filter.Poisoned == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.Poisoned == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Blessed
				if (target.TargetObject.Filter.Blessed == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.Blessed == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Human
				if (target.TargetObject.Filter.IsHuman == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.IsHuman == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Ghost
				if (target.TargetObject.Filter.IsGhost == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.IsGhost == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Female
				if (target.TargetObject.Filter.Female == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.Female == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Warmode
				if (target.TargetObject.Filter.Warmode == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.Warmode == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Friend
				if (target.TargetObject.Filter.Friend == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.Friend == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// Paralized
				if (target.TargetObject.Filter.Paralized == 1)
					listitem.SubItems.Add("X");
				else if (target.TargetObject.Filter.Paralized == -1)
					listitem.SubItems.Add("*");
				else
					listitem.SubItems.Add("-");

				// NotoColor list
				if (target.TargetObject.Filter.Notorieties.Count > 0)
				{
					string notolist = "";
					foreach (int noto in target.TargetObject.Filter.Notorieties)
					{
						notolist = notolist + GetNotoString((byte)noto) + " - ";
					}
					listitem.SubItems.Add(notolist);
				}
				else
				{
					listitem.SubItems.Add("*");
				}

				// Selector
				listitem.SubItems.Add(target.TargetObject.Selector);

				Assistant.Engine.MainWindow.TargetListView.Items.Add(listitem);
			}
		}

		internal static byte GetNotoByte(string notoname)
		{
			switch (notoname)
			{
				case "Innocent":
					return 0x01;

				case "Ally":
					return 0x02;

				case "Can be attacked":
					return 0x03;

				case "Criminal":
					return 0x04;

				case "Enemy":
					return 0x05;

				case "Murderer":
					return 0x06;

				case "Invulnerable":
					return 0x07;

				default:
					return 0x00;
			}
		}

		internal static string GetNotoString(byte notobyte)
		{
			switch (notobyte)
			{
				case 0x01:
					return "Innocent";

				case 0x02:
					return "Ally";

				case 0x03:
					return "Can be attacked";

				case 0x04:
					return "Criminal";

				case 0x05:
					return "Enemy";

				case 0x06:
					return "Murderer";

				case 0x07:
					return "Invulnerable";

				default:
					return "Null";
			}
		}
	}
}