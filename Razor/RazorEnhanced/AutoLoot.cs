using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

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

                private int m_Maximum;
                public int Maximum { get { return m_Maximum; } }

                public Property(string name, int minimum, int maximum)
				{
					m_Name = name;
					m_Minimum = minimum;
                    m_Maximum = maximum;
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


        internal static int AutolootBagSerial
        {
            get
            {
                return Convert.ToInt16(Assistant.Engine.MainWindow.AutoLootContainerLabel.Text, 16);
            }
        }

        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.AutoLootLogBox.Items.Add(addlog);
            Assistant.Engine.MainWindow.AutoLootLogBox.SelectedIndex = Assistant.Engine.MainWindow.AutoLootLogBox.Items.Count - 1;
        }

		internal static void RefreshList(List<AutoLootItem> AutoLootItemList)
		{
			Assistant.Engine.MainWindow.AutoLootListView.Items.Clear();
			foreach (AutoLootItem item in AutoLootItemList)
			{
				ListViewItem listitem = new ListViewItem();
				listitem.SubItems.Add(item.Name);
				listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));
                if (item.Color == -1)
                    listitem.SubItems.Add("All");
                else
				    listitem.SubItems.Add("0x" + item.Color.ToString("X4"));
			//	Assistant.Engine.MainWindow.AutoLootListView.Items.Add(listitem);
			}
		}

		internal static void AddItemToList(string Name, int Graphics, int Color, ListView AutolootlistView, List<AutoLootItem> AutoLootItemList)
		{
			List<AutoLootItem.Property> PropsList = new List<AutoLootItem.Property>();
			AutoLootItemList.Add(new AutoLootItem(Name, Graphics, Color, PropsList));
			RazorEnhanced.AutoLoot.RefreshList(AutoLootItemList);
            RazorEnhanced.Settings.SaveAutoLootItemList(AutoLootItemList);
		}

        internal static void ModifyItemToList(string Name, int Graphics, int Color, ListView AutolootlistView, List<AutoLootItem> AutoLootItemList, int IndexToInsert)
        {
            List<AutoLootItem.Property> PropsList = AutoLootItemList[IndexToInsert].Properties;             // salva vecchie prop
            AutoLootItemList.RemoveAt(IndexToInsert);                                                       // rimuove
            AutoLootItemList.Insert(IndexToInsert, new AutoLootItem(Name, Graphics, Color, PropsList));     // inserisce al posto di prima
            RazorEnhanced.AutoLoot.RefreshList(AutoLootItemList);
            RazorEnhanced.Settings.SaveAutoLootItemList(AutoLootItemList);
        }

        internal static void RefreshPropListView(ListView AutolootlistViewProp, List<AutoLootItem> AutoLootItemList, int IndexToInsert)
        {
            AutolootlistViewProp.Items.Clear();
            List<AutoLootItem.Property> PropsList = AutoLootItemList[IndexToInsert].Properties;             // legge props correnti
            foreach (AutoLootItem.Property props in PropsList)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.Add(props.Name);
                listitem.SubItems.Add(props.Minimum.ToString());
                listitem.SubItems.Add(props.Maximum.ToString());
                AutolootlistViewProp.Items.Add(listitem);
            }
        }

        internal static void InsertPropToItem(string Name, int Graphics, int Color, ListView AutolootlistViewProp, List<AutoLootItem> AutoLootItemList, int IndexToInsert, string PropName, int PropMin, int PropMax)
        {
            AutolootlistViewProp.Items.Clear();
            List<AutoLootItem.Property> PropsToAdd = new List<AutoLootItem.Property>();
            AutoLootItemList[IndexToInsert].Properties.Add(new AutoLootItem.Property(PropName, PropMin, PropMax));
            RazorEnhanced.AutoLoot.RefreshPropListView(AutolootlistViewProp, AutoLootItemList, IndexToInsert);
            RazorEnhanced.Settings.SaveAutoLootItemList(AutoLootItemList);
        }

        public static void Engine()
        {
            Item LootBag;
            List<RazorEnhanced.Item> IgnoreItem = new List<RazorEnhanced.Item>();
            List<RazorEnhanced.Item> ItemCorpi = new List<RazorEnhanced.Item>();
            
            // Genero filtro per corpi
            RazorEnhanced.Items.Filter FiltroCorpi = new RazorEnhanced.Items.Filter();
            FiltroCorpi.Range = 2;
            FiltroCorpi.Movable = false;
            //FiltroCorpi.IsCorpse = true;
            FiltroCorpi.OnGround = true;

           // LootBag = RazorEnhanced.Items.FindBySerial(RazorEnhanced.AutoLoot.AutolootBagSerial);
            //RazorEnhanced.AutoLoot.AddLog("- LootBag :" + LootBag.ToString());
            RazorEnhanced.AutoLoot.AddLog("- Cerco corpi....");
            ItemCorpi = RazorEnhanced.Items.ApplyFilter(FiltroCorpi);
            foreach (RazorEnhanced.Item Corpo in ItemCorpi)
            {
                RazorEnhanced.AutoLoot.AddLog("- Loot dal corpo:" + Corpo.Serial.ToString());
                Thread.Sleep(2000);
                RazorEnhanced.AutoLoot.AddLog("- Passo al corpo successvivo");
            }
        }

	}
}       