using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

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
        internal static RazorEnhanced.Item AutolootBag
        {
            get
            {
                int SerialBag = Convert.ToInt32(Assistant.Engine.MainWindow.AutoLootContainerLabel.Text, 16);
                
                if (SerialBag == 0)
                    SerialBag = World.Player.Backpack.Serial;    
                
                return RazorEnhanced.Items.FindBySerial(SerialBag);
            }
        }

        internal static int ItemDragDelay
        {
            get
            {
                return Assistant.Engine.MainWindow.AutoLootDelayLabel;
            }
        }

        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.SelectedIndex = Assistant.Engine.MainWindow.AutoLootLogBox.Items.Count - 1));
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
				Assistant.Engine.MainWindow.AutoLootListView.Items.Add(listitem);
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
            ArrayList ItemCorpi = new ArrayList();
            ArrayList IgnoreListCorpi = new ArrayList();
            bool Skip = false;
            bool EngineStop = false;

            // Genero filtro per corpi
            RazorEnhanced.Items.Filter FiltroCorpi = new RazorEnhanced.Items.Filter();
            FiltroCorpi.RangeMax = 3;
            FiltroCorpi.Movable = false;
            FiltroCorpi.IsCorpse = true;
            FiltroCorpi.OnGround = true;
            FiltroCorpi.Enabled = true;

            while (!EngineStop)
            {
                //RazorEnhanced.AutoLoot.AddLog("- Cerco corpi....");
                ItemCorpi = RazorEnhanced.Items.ApplyFilter(FiltroCorpi);
                foreach (RazorEnhanced.Item Corpo in ItemCorpi)
                {
                    foreach (RazorEnhanced.Item CorpiIgnorati in IgnoreListCorpi)
                    {
                        if (CorpiIgnorati.Serial == Corpo.Serial) // Corpo ingorato
                        {
                            Skip = true;
                            //RazorEnhanced.AutoLoot.AddLog("- Corpo Ignorato skipp:" + Corpo.Serial.ToString());
                        }
                    }
                    if (!Skip)
                    {
                        RazorEnhanced.AutoLoot.AddLog("- Loot dal corpo:" + Corpo.Serial.ToString("X8"));
                        //RazorEnhanced.Items.UseItem(Corpo);
                        //RazorEnhanced.AutoLoot.AddLog("- Apro corpo e attendo item response");
                        RazorEnhanced.Item.WaitForContents(Corpo, 5000);
                        RazorEnhanced.AutoLoot.AddLog("- Item Nel corpo: " + Corpo.Contains.Count.ToString());
                        foreach (RazorEnhanced.Item OggettiContenuti in Corpo.Contains)
                        {
                            foreach (AutoLootItem ItemDaLista in Assistant.Engine.MainWindow.AutoLootItemList)
                            {
                                if (OggettiContenuti.ItemID == ItemDaLista.Graphics)
                                {
                                    if (Utility.DistanceSqrt(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(Corpo.Position.X, Corpo.Position.Y)) <= 3)
                                    {
                                        if (ItemDaLista.Properties.Count > 0)               // Item con props
                                        {
                                            RazorEnhanced.AutoLoot.AddLog("- Item Match found scan props");
                                            bool PropsOK = false;
                                            foreach (AutoLootItem.Property PropsDaLista in ItemDaLista.Properties)      // Scansione e verifica props
                                            {
                                                int PropsSuItemDaLootare = RazorEnhanced.Items.GetPropByString(OggettiContenuti, PropsDaLista.Name);                                               
                                                if (PropsSuItemDaLootare >= PropsDaLista.Minimum && PropsSuItemDaLootare <= PropsDaLista.Maximum)
                                                {
                                                    PropsOK = true;
                                                }
                                                else
                                                {
                                                    PropsOK = false;
                                                    break;                      // alla prima fallita esce non ha senso controllare le altre
                                                }        
                                            }
                                            if (PropsOK) // Tutte le props match OK
                                            {
                                                RazorEnhanced.AutoLoot.AddLog("- Props Match ok... Looting");
                                                RazorEnhanced.Items.Move(OggettiContenuti, RazorEnhanced.AutoLoot.AutolootBag, 0);
                                                Thread.Sleep(RazorEnhanced.AutoLoot.ItemDragDelay);
                                            }
                                            else
                                            {
                                                RazorEnhanced.AutoLoot.AddLog("- Props Match fail!");
                                            }
                                        }
                                        else        // Item Senza props     
                                        {
                                            RazorEnhanced.AutoLoot.AddLog("- Item Match found... Looting");
                                            RazorEnhanced.Items.Move(OggettiContenuti, RazorEnhanced.AutoLoot.AutolootBag, 0);
                                            Thread.Sleep(RazorEnhanced.AutoLoot.ItemDragDelay);
                                        }
                                    }
                                }

                            }
                        }
                        // Ignoro corpo
                        IgnoreListCorpi.Add(Corpo);
                       // RazorEnhanced.AutoLoot.AddLog("- Corpo Ignorato: " + Corpo.Serial.ToString());
                       // RazorEnhanced.AutoLoot.AddLog("- Lista ignore:" + IgnoreListCorpi.Count);
                       // Thread.Sleep(1000); // Da levare dopo test
                    }
                    Skip = false;
                   // RazorEnhanced.AutoLoot.AddLog("- Passo al corpo successvivo");
                }
                //  Thread.Sleep(1000); // Da levare dopo test delay fra corpi
                while (World.Player.Weight -20 > World.Player.ManaMax)
                {
                    Thread.Sleep(10000);
                    RazorEnhanced.AutoLoot.AddLog("- Max weight reached, Wait untill free some space");
                    RazorEnhanced.Misc.SendMessage("AUTOLOOT: Max weight reached, Wait untill free some space");
                }
            }
        }
	}
}       
