using System;
using System.IO;
using System.Linq;
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
                {
                    SerialBag = World.Player.Backpack.Serial;
                    return RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
                }
                else
                {
                    Item bag = RazorEnhanced.Items.FindBySerial(SerialBag);
                    if (bag.RootContainer != World.Player)
                        return RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
                    else
                        return bag;
                }
			}
		}

		internal static int ItemDragDelay
		{
			get
			{
				return Assistant.Engine.MainWindow.AutoLootDelay;
			}
		}

		internal static void AddLog(string addlog)
		{
			Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.SelectedIndex = Assistant.Engine.MainWindow.AutoLootLogBox.Items.Count - 1));
		}


		internal static void RefreshList(List<AutoLootItem> autoLootItemList)
		{
			Assistant.Engine.MainWindow.AutoLootListView.Items.Clear();
			foreach (AutoLootItem item in autoLootItemList)
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
			set { m_Auto = value; }
		}

		internal static Queue<Item> m_IgnoreCorpiQueue = new Queue<Item>();

		internal static int Engine(List<AutoLootItem> autoLootList, int mseconds, Items.Filter filter)
		{
			List<Item> corpi = RazorEnhanced.Items.ApplyFilter(filter);
			bool giaAperto = false;

			foreach (RazorEnhanced.Item corpo in corpi)
			{
				if (World.Player.Weight - 20 > World.Player.MaxWeight)
				{
					RazorEnhanced.AutoLoot.AddLog("- Max weight reached, Wait untill free some space");
					RazorEnhanced.Misc.SendMessage("AUTOLOOT: Max weight reached, Wait untill free some space");
					return -1;
				}

				// Apertura forzata 1 solo volta (necessaria in caso di corpi uccisi precedentemente da altri fuori schermata, in quanto vengono flaggati come updated anche se non realmente)
				foreach (RazorEnhanced.Item corpoIgnorato in m_IgnoreCorpiQueue)
				{
					if (corpoIgnorato.Serial == corpo.Serial)
						giaAperto = true;
				}
				if (!giaAperto)
				{
					RazorEnhanced.AutoLoot.AddLog("- Force Open: 0x" + corpo.Serial.ToString("X8"));
					Items.UseItem(corpo);
					m_IgnoreCorpiQueue.Enqueue(corpo);
					if (m_IgnoreCorpiQueue.Count > 50)
						m_IgnoreCorpiQueue.Dequeue();
				}

				RazorEnhanced.Items.WaitForContents(corpo, 1500);

				foreach (RazorEnhanced.Item oggettoContenuto in corpo.Contains)
				{
					// Blocco shared
					if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0) // Attende l'arrivo delle props
					{
						if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties[0].ToString() == "Instanced loot container")  // Rilevato backpack possibile shared loot verifico props
						{
							RazorEnhanced.Items.WaitForContents(oggettoContenuto, 1500);
							foreach (RazorEnhanced.Item oggettoContenutoShard in oggettoContenuto.Contains)
							{

								foreach (AutoLootItem autoLootItem in autoLootList)
								{
                                    if (autoLootItem.Color == -1)
                                    {
                                        if (oggettoContenutoShard.ItemID == autoLootItem.Graphics)
                                        {
                                            GrabItem(autoLootItem, oggettoContenuto, corpo, mseconds);
                                        }
                                    }
                                    else
                                    {
                                        if (oggettoContenutoShard.ItemID == autoLootItem.Graphics && oggettoContenutoShard.Hue == autoLootItem.Color)
                                        {
                                            GrabItem(autoLootItem, oggettoContenuto, corpo, mseconds);
                                        }
                                    }
								}
							}
						}
					}
					//fine Blocco shared

					foreach (AutoLootItem autoLootItem in autoLootList)
					{
                        if (autoLootItem.Color == -1)          // Colore ALL
                        {
                            if (oggettoContenuto.ItemID == autoLootItem.Graphics)
                            {
                                bool grabItem = true;
                                if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0)  // se zaino Attende l'arrivo delle props
                                    if (oggettoContenuto.Properties[0].ToString() == "Instanced loot container") // Controllo in caso siano presenti backpack nella lista di item interessati al loot
                                        grabItem = false;

                                if (grabItem)
                                {
                                    GrabItem(autoLootItem, oggettoContenuto, corpo, mseconds);
                                }
                            }
                        }
                        else
                        {
                            if (oggettoContenuto.ItemID == autoLootItem.Graphics && oggettoContenuto.Hue == autoLootItem.Color)
                            {
                                bool grabItem = true;
                                if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0)  // se zaino Attende l'arrivo delle props
                                    if (oggettoContenuto.Properties[0].ToString() == "Instanced loot container") // Controllo in caso siano presenti backpack nella lista di item interessati al loot
                                        grabItem = false;

                                if (grabItem)
                                {
                                    GrabItem(autoLootItem, oggettoContenuto, corpo, mseconds);
                                }
                            }
                        }
					}
				}
			}

			return 0;
		}

        internal static void GrabItem(AutoLootItem autoLoootItem, Item oggettoContenuto, Item corpo, int mseconds)
        {
            if (Utility.DistanceSqrt(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(corpo.Position.X, corpo.Position.Y)) <= 3)
            {
                if (autoLoootItem.Properties.Count > 0) // Item con props
                {
                    RazorEnhanced.AutoLoot.AddLog("- Item Match found scan props");

                    bool propsOK = false;
                    foreach (AutoLootItem.Property props in autoLoootItem.Properties) // Scansione e verifica props
                    {
                        int PropsSuItemDaLootare = RazorEnhanced.Items.GetPropByString(oggettoContenuto, props.Name);
                        if (PropsSuItemDaLootare >= props.Minimum && PropsSuItemDaLootare <= props.Maximum)
                        {
                            propsOK = true;
                        }
                        else
                        {
                            propsOK = false;
                            break; // alla prima fallita esce non ha senso controllare le altre
                        }
                    }

                    if (propsOK) // Tutte le props match OK
                    {
                        RazorEnhanced.AutoLoot.AddLog("- Item Match found (0x" + oggettoContenuto.Serial.ToString("X8") + ") ... Looting");
                        RazorEnhanced.Items.Move(oggettoContenuto, RazorEnhanced.AutoLoot.AutolootBag, 0);
                        Thread.Sleep(mseconds);
                    }
                    else
                    {
                        RazorEnhanced.AutoLoot.AddLog("- Props Match fail!");
                    }
                }
                else // Item Senza props     
                {
                    RazorEnhanced.AutoLoot.AddLog("- Item Match found (0x" + oggettoContenuto.Serial.ToString("X8") + ") ... Looting");
                    RazorEnhanced.Items.Move(oggettoContenuto, RazorEnhanced.AutoLoot.AutolootBag, 0);
                    Thread.Sleep(mseconds);
                }
            }
        }

		internal static void Engine()
		{
			int exit = Int32.MinValue;

			// Genero filtro per corpi
			Items.Filter corpseFilter = new Items.Filter();
			corpseFilter.RangeMax = 2;
			corpseFilter.Movable = false;
			corpseFilter.IsCorpse = true;
			corpseFilter.OnGround = true;
			corpseFilter.Enabled = true;

			exit = Engine(Assistant.Engine.MainWindow.AutoLootItemList, Assistant.Engine.MainWindow.AutoLootDelay , corpseFilter);
		}

		// Funzioni di controllo da script
		public static void ResetIgnore()
		{
			m_IgnoreCorpiQueue.Clear();
		}

		public static int RunOnce(List<AutoLootItem> autoLootList, int mseconds, Items.Filter filter)
		{
			int exit = Int32.MinValue;

			if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
			{
				Misc.SendMessage("Script Error: Autoloot.Start: Autoloot already running");
			}
			else
			{
                exit = Engine(autoLootList, mseconds, filter);
			}

			return exit;
		}

		public static void Start()
		{
			if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
				Misc.SendMessage("Script Error: Autoloot.Start: Autoloot already running");
			else
				Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = true));
		}

		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == false)
				Misc.SendMessage("Script Error: Autoloot.Stop: Autoloot already sleeping");
			else
				Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.AutolootCheckBox.Checked;
		}

		public static void ChangeList(string nomelista)
		{
			bool ListaOK = false;
			for (int i = 0; i < Assistant.Engine.MainWindow.AutolootListSelect.Items.Count; i++)
			{
				if (nomelista == Assistant.Engine.MainWindow.AutolootListSelect.GetItemText(Assistant.Engine.MainWindow.AutolootListSelect.Items[i]))
					ListaOK = true;
			}
			if (!ListaOK)
				Misc.SendMessage("Script Error: Autoloot.ChangeList: Autoloot list: " + nomelista + " not exist");
			else
			{
				if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false));
					Assistant.Engine.MainWindow.AutolootListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutolootListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = true));
				}
				else
				{
					Assistant.Engine.MainWindow.AutolootListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutolootListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}
	}
}
