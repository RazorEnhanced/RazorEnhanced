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

                return RazorEnhanced.Item.FindBySerial(SerialBag);
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

        internal static void AddItemToList(string name, int graphics, int color, ListView AutolootlistView, List<AutoLootItem> autoLootItemList)
        {
            List<AutoLootItem.Property> propsList = new List<AutoLootItem.Property>();
            autoLootItemList.Add(new AutoLootItem(name, graphics, color, propsList));
            RazorEnhanced.Settings.SaveAutoLootItemList(Assistant.Engine.MainWindow.AutolootListSelect.SelectedItem.ToString(), autoLootItemList);
            RazorEnhanced.AutoLoot.RefreshList(autoLootItemList);
        }

        internal static void ModifyItemToList(string name, int graphics, int color, ListView autolootlistView, List<AutoLootItem> autoLootItemList, int indexToInsert)
        {
            List<AutoLootItem.Property> PropsList = autoLootItemList[indexToInsert].Properties;             // salva vecchie prop
            autoLootItemList.RemoveAt(indexToInsert);                                                       // rimuove
            autoLootItemList.Insert(indexToInsert, new AutoLootItem(name, graphics, color, PropsList));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveAutoLootItemList(Assistant.Engine.MainWindow.AutolootListSelect.SelectedItem.ToString(), autoLootItemList);
            RazorEnhanced.AutoLoot.RefreshList(autoLootItemList);
        }

        internal static void RefreshPropListView(ListView autolootlistViewProp, List<AutoLootItem> autoLootItemList, int indexToInsert)
        {
            autolootlistViewProp.Items.Clear();
            List<AutoLootItem.Property> PropsList = autoLootItemList[indexToInsert].Properties;             // legge props correnti
            foreach (AutoLootItem.Property props in PropsList)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.Add(props.Name);
                listitem.SubItems.Add(props.Minimum.ToString());
                listitem.SubItems.Add(props.Maximum.ToString());
                autolootlistViewProp.Items.Add(listitem);
            }
            RazorEnhanced.Settings.SaveAutoLootItemList(Assistant.Engine.MainWindow.AutolootListSelect.SelectedItem.ToString(), autoLootItemList);
        }

        internal static void InsertPropToItem(string name, int graphics, int Color, ListView autolootlistViewProp, List<AutoLootItem> autoLootItemList, int indexToInsert, string propName, int propMin, int propMax)
        {
            autolootlistViewProp.Items.Clear();
            List<AutoLootItem.Property> PropsToAdd = new List<AutoLootItem.Property>();
            autoLootItemList[indexToInsert].Properties.Add(new AutoLootItem.Property(propName, propMin, propMax));
            RazorEnhanced.Settings.SaveAutoLootItemList(Assistant.Engine.MainWindow.AutolootListSelect.SelectedItem.ToString(), autoLootItemList);
        }

		private static bool m_Auto;
		internal static bool Auto
		{
			get { return m_Auto; }
			set
			{
				if (m_Auto == value)
					return;

				m_Auto = value;
			}
		}

		internal static ArrayList m_IgnoreCorpiList = new ArrayList();

		public static void ResetIgnore()
		{
			m_IgnoreCorpiList.Clear();
		}

		internal static int Engine(List<AutoLootItem> autoLootList, int milliseconds, Item.Filter filter)
		{
			ArrayList corpi = RazorEnhanced.Item.ApplyFilter(filter);

			foreach (RazorEnhanced.Item corpo in corpi)
			{
				if (World.Player.Weight - 20 > World.Player.MaxWeight)
				{
					RazorEnhanced.AutoLoot.AddLog("- Max weight reached, Wait untill free some space");
					RazorEnhanced.Misc.SendMessage("AUTOLOOT: Max weight reached, Wait untill free some space");
					return -1;
				}

					RazorEnhanced.AutoLoot.AddLog("- Loot dal corpo:" + corpo.Serial.ToString("X8"));
					RazorEnhanced.Item.WaitForContents(corpo, 5000);
					RazorEnhanced.AutoLoot.AddLog("- Item Nel corpo: " + corpo.Contains.Count.ToString());

					foreach (RazorEnhanced.Item oggettoContenuto in corpo.Contains)
					{
                        if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Movable == true)
                        {
                            RazorEnhanced.AutoLoot.AddLog("- Detected Shard Loot: " + oggettoContenuto.Serial.ToString());
                            RazorEnhanced.Item.WaitForContents(oggettoContenuto, 5000);
                            foreach (RazorEnhanced.Item oggettoContenutoShard in oggettoContenuto.Contains)
                            {
                                // Blocco Shard
                                foreach (AutoLootItem autoLoootItem in autoLootList)
                                {
                                    if (oggettoContenutoShard.ItemID == autoLoootItem.Graphics)
                                    {
                                        if (Utility.DistanceSqrt(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(corpo.Position.X, corpo.Position.Y)) <= 3)
                                        {
                                            if (autoLoootItem.Properties.Count > 0) // Item con props
                                            {
                                                RazorEnhanced.AutoLoot.AddLog("- Item Match found scan props");

                                                bool propsOK = false;
                                                foreach (AutoLootItem.Property props in autoLoootItem.Properties) // Scansione e verifica props
                                                {
                                                    int PropsSuItemDaLootare = RazorEnhanced.Item.GetPropByString(oggettoContenutoShard, props.Name);
                                                    if (PropsSuItemDaLootare >= props.Minimum && PropsSuItemDaLootare <= props.Maximum)
                                                    {
                                                        propsOK = true;
                                                    }
                                                    else
                                                    {
                                                        propsOK = false;
                                                        break;                      // alla prima fallita esce non ha senso controllare le altre
                                                    }
                                                }

                                                if (propsOK) // Tutte le props match OK
                                                {
                                                    RazorEnhanced.AutoLoot.AddLog("- Props Match ok... Looting");
                                                    RazorEnhanced.Item.Move(oggettoContenutoShard, RazorEnhanced.AutoLoot.AutolootBag, 0);
                                                    Thread.Sleep(milliseconds);
                                                }
                                                else
                                                {
                                                    RazorEnhanced.AutoLoot.AddLog("- Props Match fail!");
                                                }
                                            }
                                            else // Item Senza props     
                                            {
                                                RazorEnhanced.AutoLoot.AddLog("- Item Match found... Looting");
                                                RazorEnhanced.Item.Move(oggettoContenutoShard, RazorEnhanced.AutoLoot.AutolootBag, 0);
                                                Thread.Sleep(milliseconds);
                                            }
                                        }
                                    }
                                }
                                //fine Blocco shaded
                            }
                        }
                           
						foreach (AutoLootItem autoLoootItem in autoLootList)
						{
							if (oggettoContenuto.ItemID == autoLoootItem.Graphics)
							{
								if (Utility.DistanceSqrt(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(corpo.Position.X, corpo.Position.Y)) <= 3)
								{
									if (autoLoootItem.Properties.Count > 0) // Item con props
									{
										RazorEnhanced.AutoLoot.AddLog("- Item Match found scan props");

										bool propsOK = false;
										foreach (AutoLootItem.Property props in autoLoootItem.Properties) // Scansione e verifica props
										{
											int PropsSuItemDaLootare = RazorEnhanced.Item.GetPropByString(oggettoContenuto, props.Name);
											if (PropsSuItemDaLootare >= props.Minimum && PropsSuItemDaLootare <= props.Maximum)
											{
												propsOK = true;
											}
											else
											{
												propsOK = false;
												break;                      // alla prima fallita esce non ha senso controllare le altre
											}
										}

										if (propsOK) // Tutte le props match OK
										{
											RazorEnhanced.AutoLoot.AddLog("- Props Match ok... Looting");
											RazorEnhanced.Item.Move(oggettoContenuto, RazorEnhanced.AutoLoot.AutolootBag, 0);
											Thread.Sleep(milliseconds);
										}
										else
										{
											RazorEnhanced.AutoLoot.AddLog("- Props Match fail!");
										}
									}
									else // Item Senza props     
									{
										RazorEnhanced.AutoLoot.AddLog("- Item Match found... Looting");
										RazorEnhanced.Item.Move(oggettoContenuto, RazorEnhanced.AutoLoot.AutolootBag, 0);
										Thread.Sleep(milliseconds);
									}
								}
							}
						}
					}
			}

			return 0;
		}

		public static int Run(List<AutoLootItem> autoLootList, int milliseconds, Item.Filter filter)
		{
			int result = Int32.MinValue;
			result=	AutoLoot.Engine(autoLootList, milliseconds, filter);
			return result;
		}

		public static int Run()
		{
			    // Genero filtro per corpi
				Item.Filter corpseFilter = new Item.Filter();
				corpseFilter.RangeMax = 3;
				corpseFilter.Movable = false;
				corpseFilter.IsCorpse = true;
				corpseFilter.OnGround = true;
				corpseFilter.Enabled = true;

				int exit =Run(Assistant.Engine.MainWindow.AutoLootItemList, Assistant.Engine.MainWindow.AutoLootDelayLabel, corpseFilter);
				return exit;
		}
	}
}
