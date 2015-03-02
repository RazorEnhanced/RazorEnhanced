using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class AutoLoot
	{
		[Serializable]
		public class AutoLootItem
		{
			[Serializable]
			public class Property
			{
				private string m_Name;
				public string Name { get { return m_Name; } }

				private int m_Minimum;
				public int Minimum { get { return m_Minimum; } }

				public Property(string name, int minimum)
				{
					m_Name = name;
					m_Minimum = minimum;
				}
			}

			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private List<Property> m_Properties;
			public List<Property> Properties { get { return m_Properties; } }

			public AutoLootItem(string name, int graphics, int color, List<Property> properties)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_Properties = properties;
			}
		}

		internal static void RefreshList(ListView AutolootlistView, List<AutoLootItem> AutoLootItemList)
		{
			AutolootlistView.Items.Clear();
			foreach (AutoLootItem item in AutoLootItemList)
			{
				ListViewItem listitem = new ListViewItem();
				listitem.SubItems.Add(item.Name);
				listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));
				listitem.SubItems.Add("0x" + item.Color.ToString("X4"));
				AutolootlistView.Items.Add(listitem);
			}
		}

		internal static void AddItemToList(string Name, int Graphics, int Color, ListView AutolootlistView, List<AutoLootItem> AutoLootItemList)
		{
			List<AutoLootItem.Property> PropsList = new List<AutoLootItem.Property>();
			AutoLootItemList.Add(new AutoLootItem(Name, Graphics, Color, PropsList));
			RazorEnhanced.AutoLoot.RefreshList(AutolootlistView, AutoLootItemList);
		}
	}
}