using Assistant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class AutoLoot
	{
		private static Queue<int> m_IgnoreCorpseList = new Queue<int>();

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

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			private List<Property> m_Properties;
			public List<Property> Properties { get { return m_Properties; } }

			public AutoLootItem(string name, int graphics, int color, bool selected, List<Property> properties)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_Selected = selected;
				m_Properties = properties;
			}
		}

		internal class AutoLootList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Delay;
			internal int Delay { get { return m_Delay; } }

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			private bool m_Noopencorpse;
			internal bool NoOpenCorpse { get { return m_Noopencorpse; } }

			public AutoLootList(string description, int delay, int bag, bool selected, bool noopencorpse)
			{
				m_Description = description;
				m_Delay = delay;
				m_Bag = bag;
				m_Selected = selected;
				m_Noopencorpse = noopencorpse;
            }
		}

		private static bool m_AutoMode;

		internal static bool AutoMode
		{
			get { return m_AutoMode; }
			set { m_AutoMode = value; }
		}

		internal static string AutoLootListName
		{
			get
			{
				return (string)Assistant.Engine.MainWindow.AutoLootListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.AutoLootListSelect.Text));
			}

			set
			{
				Assistant.Engine.MainWindow.AutoLootListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootListSelect.Text = value));
			}
		}

		internal static int AutoLootDelay
		{
			get
			{
				int delay = 100;
				Assistant.Engine.MainWindow.AutolootLabelDelay.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.AutolootLabelDelay.Text, out delay)));
				return delay;
			}

			set
			{
				Assistant.Engine.MainWindow.AutolootLabelDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootLabelDelay.Text = value.ToString()));
			}
		}

		internal static int AutoLootBag
		{
			get
			{
				int serialBag = 0;

				try
				{
					serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.AutoLootContainerLabel.Text, 16);
				}
				catch
				{ }

				return serialBag;
			}

			set
			{
				Assistant.Engine.MainWindow.AutoLootContainerLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootContainerLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool NoOpenCorpse
		{
			get
			{
				return Assistant.Engine.MainWindow.AutoLootNoOpenCheckBox.Checked;
			}

			set
			{
				Assistant.Engine.MainWindow.AutoLootNoOpenCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootNoOpenCheckBox.Checked = value));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (!Assistant.Engine.Running)
				return;

			Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.SelectedIndex = Assistant.Engine.MainWindow.AutoLootLogBox.Items.Count - 1));
			if (Assistant.Engine.MainWindow.AutoLootLogBox.Items.Count > 300)
				Assistant.Engine.MainWindow.AutoLootLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootLogBox.Items.Clear()));
		}

		internal static void RefreshLists()
		{
			List<AutoLootList> lists;
			RazorEnhanced.Settings.AutoLoot.ListsRead(out lists);

			if (lists.Count == 0)
				Assistant.Engine.MainWindow.AutoLootListView.Items.Clear();

			AutoLootList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.AutoLootListSelect.Text)
				return;

			Assistant.Engine.MainWindow.AutoLootListSelect.Items.Clear();
			foreach (AutoLootList l in lists)
			{
				Assistant.Engine.MainWindow.AutoLootListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.AutoLootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutoLootListSelect.Items.IndexOf(l.Description);
					AutoLootDelay = l.Delay;
					AutoLootBag = l.Bag;
					NoOpenCorpse = l.NoOpenCorpse;
				}
			}
		}

		internal static void RefreshItems()
		{
			List<AutoLootList> lists;
			RazorEnhanced.Settings.AutoLoot.ListsRead(out lists);

			Assistant.Engine.MainWindow.AutoLootListView.Items.Clear();
			foreach (AutoLootList l in lists)
			{
				if (l.Selected)
				{
					List<AutoLoot.AutoLootItem> items;
					RazorEnhanced.Settings.AutoLoot.ItemsRead(l.Description, out items);

					foreach (AutoLootItem item in items)
					{
						ListViewItem listitem = new ListViewItem();

						listitem.Checked = item.Selected;

						listitem.SubItems.Add(item.Name);
						listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));

						if (item.Color == -1)
							listitem.SubItems.Add("All");
						else
							listitem.SubItems.Add("0x" + item.Color.ToString("X4"));

						Assistant.Engine.MainWindow.AutoLootListView.Items.Add(listitem);
					}
				}
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<AutoLootItem> items;
			RazorEnhanced.Settings.AutoLoot.ItemsRead(AutoLootListName, out items);

			if (items.Count != Assistant.Engine.MainWindow.AutoLootListView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.AutoLootListView.Items[i];
			AutoLootItem old = items[i];

			if (lvi != null && old != null)
			{
				AutoLootItem item = new AutoLoot.AutoLootItem(old.Name, old.Graphics, old.Color, lvi.Checked, old.Properties);
				RazorEnhanced.Settings.AutoLoot.ItemReplace(RazorEnhanced.AutoLoot.AutoLootListName, i, item);
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.AutoLoot.ListInsert(newList, RazorEnhanced.AutoLoot.AutoLootDelay, (int)0, RazorEnhanced.AutoLoot.NoOpenCorpse);

			RazorEnhanced.AutoLoot.RefreshLists();
			RazorEnhanced.AutoLoot.RefreshItems();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.AutoLoot.ListExists(list))
			{
				RazorEnhanced.Settings.AutoLoot.ListDelete(list);
			}

			RazorEnhanced.AutoLoot.RefreshLists();
			RazorEnhanced.AutoLoot.RefreshItems();
		}

		internal static void AddItemToList(string name, int graphics, int color)
		{
			List<AutoLootItem.Property> propsList = new List<AutoLootItem.Property>();
			AutoLootItem item = new AutoLootItem(name, graphics, color, false, propsList);

			string selection = Assistant.Engine.MainWindow.AutoLootListSelect.Text;

			if (RazorEnhanced.Settings.AutoLoot.ListExists(selection))
			{
				if (!RazorEnhanced.Settings.AutoLoot.ItemExists(selection, item))
					RazorEnhanced.Settings.AutoLoot.ItemInsert(selection, item);
			}

			RazorEnhanced.AutoLoot.RefreshItems();
		}

		internal static void ModifyItemInList(string name, int graphics, int color, bool selected, AutoLootItem old, int index)
		{
			List<AutoLootItem.Property> propsList = old.Properties;
			AutoLootItem item = new AutoLootItem(name, graphics, color, selected, propsList);

			string selection = Assistant.Engine.MainWindow.AutoLootListSelect.Text;

			if (RazorEnhanced.Settings.AutoLoot.ListExists(selection))
			{
				if (RazorEnhanced.Settings.AutoLoot.ItemExists(selection, old))
					RazorEnhanced.Settings.AutoLoot.ItemReplace(selection, index, item);
			}

			RazorEnhanced.AutoLoot.RefreshItems();
		}

		internal static void AddPropToItem(string list, int index, AutoLoot.AutoLootItem item, string propName, int propMin, int propMax)
		{
			AutoLootItem.Property prop = new AutoLootItem.Property(propName, propMin, propMax);
			item.Properties.Add(prop);
			RazorEnhanced.Settings.AutoLoot.ItemReplace(list, index, item);
		}

		internal static int Engine(List<AutoLootItem> autoLootList, int mseconds, Items.Filter filter)
		{
			List<Item> corpi = RazorEnhanced.Items.ApplyFilter(filter);

			if (World.Player.IsGhost)
			{
				Thread.Sleep(2000);
				ResetIgnore();
				return 0;
			}

			foreach (RazorEnhanced.Item corpo in corpi)
			{
				if (!NoOpenCorpse)
				{
					if (!m_IgnoreCorpseList.Contains(corpo.Serial))
					{
						DragDropManager.AutoLootSerialCorpseRefresh.Enqueue(corpo.Serial);
						m_IgnoreCorpseList.Enqueue(corpo.Serial);
					}

					if (m_IgnoreCorpseList.Count > 100)
						m_IgnoreCorpseList.Dequeue();
				}

				foreach (RazorEnhanced.Item oggettoContenuto in corpo.Contains)
				{
					// Blocco shared
					if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0) // Attende l'arrivo delle props
					{
						if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties[0].ToString() == "Instanced loot container")  // Rilevato backpack possibile shared loot verifico props
						{
							foreach (RazorEnhanced.Item oggettoContenutoShard in oggettoContenuto.Contains)
							{
								foreach (AutoLootItem autoLootItem in autoLootList)
								{
									if (!autoLootItem.Selected)
										continue;

									if (autoLootItem.Color == -1)
									{
										if (oggettoContenutoShard.ItemID == autoLootItem.Graphics)
										{
											GrabItem(autoLootItem, oggettoContenutoShard, mseconds);
										}
									}
									else
									{
										if (oggettoContenutoShard.ItemID == autoLootItem.Graphics && oggettoContenutoShard.Hue == autoLootItem.Color)
										{
											GrabItem(autoLootItem, oggettoContenutoShard, mseconds);
										}
									}
								}
							}
						}
					}
					//fine Blocco shared

					foreach (AutoLootItem autoLootItem in autoLootList)
					{
						if (!autoLootItem.Selected)
							continue;

						if (autoLootItem.Color == -1)          // Colore ALL
						{
							if (oggettoContenuto.ItemID != autoLootItem.Graphics)
								continue;

							bool grabItem = true;
							if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0)  // se zaino Attende l'arrivo delle props
								if (oggettoContenuto.Properties[0].ToString() == "Instanced loot container") // Controllo in caso siano presenti backpack nella lista di item interessati al loot
									grabItem = false;

							if (grabItem)
							{
								GrabItem(autoLootItem, oggettoContenuto, mseconds);
							}
						}
						else
						{
							if (oggettoContenuto.ItemID != autoLootItem.Graphics || oggettoContenuto.Hue != autoLootItem.Color)
								continue;

							bool grabItem = true;
							if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0)  // se zaino Attende l'arrivo delle props
								if (oggettoContenuto.Properties[0].ToString() == "Instanced loot container") // Controllo in caso siano presenti backpack nella lista di item interessati al loot
									grabItem = false;

							if (grabItem)
							{
								GrabItem(autoLootItem, oggettoContenuto, mseconds);
							}
						}
					}
				}
			}

			return 0;
		}

		internal static void GrabItem(AutoLootItem autoLoootItem, Item oggettoContenuto, int mseconds)
		{
			if (!oggettoContenuto.Movable || !oggettoContenuto.Visible)
				return;

			if (DragDropManager.AutoLootSerialToGrab.Contains(oggettoContenuto.Serial))
				return;

			if (autoLoootItem.Properties.Count > 0) // Item con props
			{
				RazorEnhanced.AutoLoot.AddLog("- Item Match found scan props");
				Items.WaitForProps(oggettoContenuto, 1000);

				bool propsOk = false;
				foreach (AutoLootItem.Property props in autoLoootItem.Properties) // Scansione e verifica props
				{
					int propsSuItemDaLootare = RazorEnhanced.Items.GetPropValue(oggettoContenuto, props.Name);
					if (propsSuItemDaLootare >= props.Minimum && propsSuItemDaLootare <= props.Maximum)
					{
						propsOk = true;
					}
					else
					{
						propsOk = false;
						break; // alla prima fallita esce non ha senso controllare le altre
					}
				}

				if (propsOk) // Tutte le props match OK
				{
					DragDropManager.AutoLootSerialToGrab.Enqueue(oggettoContenuto.Serial);
				}
				else
				{
					RazorEnhanced.AutoLoot.AddLog("- Props Match fail!");
				}
			}
			else // Item Senza props
			{
				DragDropManager.AutoLootSerialToGrab.Enqueue(oggettoContenuto.Serial);
			}
		}

		internal static void AutoRun()
		{
			// Genero filtro per corpi
			Items.Filter corpseFilter = new Items.Filter
			{
				RangeMax = 2,
				Movable = false,
				IsCorpse = 1,
				OnGround = 1,
				Enabled = true
			};

			// Check bag
			Assistant.Item bag = Assistant.World.FindItem(AutoLootBag);
			if (bag != null)
			{
				if (bag.RootContainer != World.Player)
				{
					if (Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						Misc.SendMessage("Autoloot: Invalid Bag, Switch to backpack", 945);
					AddLog("Invalid Bag, Switch to backpack");
					AutoLootBag = (int)World.Player.Backpack.Serial.Value;
				}
			}
			else
			{
				if (Settings.General.ReadBool("ShowMessageFieldCheckBox"))
					Misc.SendMessage("Autoloot: Invalid Bag, Switch to backpack", 945);
				AddLog("Invalid Bag, Switch to backpack");
				AutoLootBag = (int)World.Player.Backpack.Serial.Value;
			}

			List<AutoLoot.AutoLootItem> items;
			string list = AutoLoot.AutoLootListName;
			RazorEnhanced.Settings.AutoLoot.ItemsRead(list, out items);
			Engine(items, AutoLootDelay, corpseFilter);
		}

		// Funzioni di controllo da script
		public static void ResetIgnore()
		{
			m_IgnoreCorpseList.Clear();
			DragDropManager.AutoLootSerialToGrab = new ConcurrentQueue<int>();
			DragDropManager.AutoLootSerialCorpseRefresh = new ConcurrentQueue<int>();
			Scavenger.ResetIgnore();
		}

		public static int RunOnce(List<AutoLootItem> autoLootList, int mseconds, Items.Filter filter)
		{
			int exit = int.MinValue;

			if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
			{
					Scripts.SendMessageScriptError("Script Error: Autoloot.Start: Autoloot already running");
			}
			else
			{
				exit = Engine(autoLootList, mseconds, filter);
			}

			return exit;
		}

		public static void Start()
		{
			if (!ClientCommunication.AllowBit(FeatureBit.AutolootAgent))
			{
				Scripts.SendMessageScriptError("Autoloot Not Allowed!");
				return;
			}

				if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
			{
					Scripts.SendMessageScriptError("Script Error: Autoloot.Start: Autoloot already running");
			}
			else
				Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = true));
		}

		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: Autoloot.Stop: Autoloot already sleeping");
			}
			else
				Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.AutolootCheckBox.Checked;
		}

		public static void ChangeList(string nomelista)
		{
			if (!Assistant.Engine.MainWindow.AutoLootListSelect.Items.Contains(nomelista))
			{
				Scripts.SendMessageScriptError("Script Error: Autoloot.ChangeList: Autoloot list: " + nomelista + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false));
					Assistant.Engine.MainWindow.AutoLootListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutoLootListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.AutolootCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.AutolootCheckBox.Checked = true));
				}
				else
				{
					Assistant.Engine.MainWindow.AutoLootListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutoLootListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}
	}
}