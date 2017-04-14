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

		public class SerialToGrab
		{
			private int m_corpseserial;
			public int CorpseSerial { get { return m_corpseserial; } }

			private int m_itemserial;
			public int ItemSerial { get { return m_itemserial; } }

			public SerialToGrab(int itemserial, int corpseserial)
			{
				m_corpseserial = corpseserial;
				m_itemserial = itemserial;
			}
		}

		internal static ConcurrentQueue<SerialToGrab> SerialToGrabList = new ConcurrentQueue<SerialToGrab>();

		internal class AutoLootList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Delay;
			internal int Delay { get { return m_Delay; } }

			private int m_Range;
			internal int Range { get { return m_Range; } }

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			private bool m_Noopencorpse;
			internal bool NoOpenCorpse { get { return m_Noopencorpse; } }

			public AutoLootList(string description, int delay, int bag, bool selected, bool noopencorpse, int range)
			{
				m_Description = description;
				m_Delay = delay;
				m_Bag = bag;
				m_Selected = selected;
				m_Noopencorpse = noopencorpse;
				m_Range = range;
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

		internal static int MaxRange
		{
			get
			{
				int range = 2;
				Assistant.Engine.MainWindow.AutoLootTextBoxMaxRange.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.AutoLootTextBoxMaxRange.Text, out range)));
				return range;
			}

			set
			{
				Assistant.Engine.MainWindow.AutoLootTextBoxMaxRange.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoLootTextBoxMaxRange.Text = value.ToString()));
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

		internal static void InitGrid()
		{
			List<AutoLootList> lists;
			RazorEnhanced.Settings.AutoLoot.ListsRead(out lists);

			Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Clear();

			foreach (AutoLootList l in lists)
			{
				if (l.Selected)
				{
					List<AutoLoot.AutoLootItem> items;
					RazorEnhanced.Settings.AutoLoot.ItemsRead(l.Description, out items);

					foreach (AutoLootItem item in items)
					{
						string color = "All";
						if (item.Color != -1)
							color = "0x" + item.Color.ToString("X4");

						Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, "0x" + item.Graphics.ToString("X4"), color, item.Properties });
					}

					break;
				}
			}
		}

		internal static void CopyTable()
		{
			Settings.AutoLoot.ClearList(Assistant.Engine.MainWindow.AutoLootListSelect.Text); // Rimuove vecchi dati dal save

			foreach (DataGridViewRow row in Assistant.Engine.MainWindow.AutoLootDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[3].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[3].Value, 16);

				bool check = false;
				bool.TryParse(row.Cells[0].Value.ToString(), out check);

				if (row.Cells[4].Value != null)
					Settings.AutoLoot.ItemInsert(Assistant.Engine.MainWindow.AutoLootListSelect.Text, new AutoLootItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, (List<AutoLootItem.Property>)row.Cells[4].Value));
				else
					Settings.AutoLoot.ItemInsert(Assistant.Engine.MainWindow.AutoLootListSelect.Text, new AutoLootItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, new List<AutoLootItem.Property>()));
			}

			Settings.Save(); // Salvo dati
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.AutoLoot.ListInsert(newList, RazorEnhanced.AutoLoot.AutoLootDelay, (int)0, RazorEnhanced.AutoLoot.NoOpenCorpse, RazorEnhanced.AutoLoot.MaxRange);

			RazorEnhanced.AutoLoot.RefreshLists();
			RazorEnhanced.AutoLoot.InitGrid();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.AutoLoot.ListExists(list))
			{
				RazorEnhanced.Settings.AutoLoot.ListDelete(list);
			}

			RazorEnhanced.AutoLoot.RefreshLists();
			RazorEnhanced.AutoLoot.InitGrid();
		}

		internal static void AddItemToList(string name, int graphics, int color)
		{
			Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Add(new object[] { "False", name, "0x" + graphics.ToString("X4"), "0x" + color.ToString("X4"), new List<AutoLootItem.Property>() });
			CopyTable();
		}

		private static void RefreshCorpse(Item corpo)
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
				RazorEnhanced.Item m_sharedcont = null;
				RazorEnhanced.Item m_OSIcont = null;

				RefreshCorpse(corpo);

				foreach (RazorEnhanced.Item oggettoContenuto in corpo.Contains)
				{
					// Blocco shared
					if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0) // Attende l'arrivo delle props
					{
						if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties[0].ToString() == "Instanced loot container")  // Rilevato backpack possibile shared loot verifico props UODREAMS
						{
							m_sharedcont = oggettoContenuto;
							break;
						}
					}
					if (oggettoContenuto.IsCorpse)  // Rilevato contenitore OSI
					{
						m_OSIcont = oggettoContenuto;
						break;
					}
				}

				RazorEnhanced.Item m_cont = null;

				if (m_sharedcont != null)
					m_cont = m_sharedcont;
				else if (m_OSIcont != null)
					m_cont = m_OSIcont;
				else
					m_cont = corpo;

				foreach (AutoLootItem autoLootItem in autoLootList)
				{
					if (!autoLootItem.Selected)
						continue;

					foreach (RazorEnhanced.Item oggettoContenuto in m_cont.Contains)
					{
						if (DragDropManager.HoldingItem)
							continue;

						if (autoLootItem.Color == -1)          // Colore ALL
						{
							if (oggettoContenuto.ItemID != autoLootItem.Graphics)
								continue;
							GrabItem(autoLootItem, oggettoContenuto, corpo.Serial);
						}
						else
						{
							if (oggettoContenuto.ItemID != autoLootItem.Graphics || oggettoContenuto.Hue != autoLootItem.Color)
								continue;
							GrabItem(autoLootItem, oggettoContenuto, corpo.Serial);
						}
					}
				}
			}

			return 0;
		}

		internal static void GrabItem(AutoLootItem autoLoootItem, Item oggettoContenuto, int corpseserial)
		{
			foreach (SerialToGrab item in SerialToGrabList)
				if (item.ItemSerial == oggettoContenuto.Serial)
					return;

			if (!oggettoContenuto.Movable || !oggettoContenuto.Visible)
				return;

			SerialToGrab data = new SerialToGrab(oggettoContenuto.Serial, corpseserial);

			if (autoLoootItem.Properties.Count > 0) // Item con props
			{
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
					SerialToGrabList.Enqueue(data);
			}
			else // Item Senza props
				SerialToGrabList.Enqueue(data);
		}

		internal static void AutoRun()
		{
			// Genero filtro per corpi
			Items.Filter corpseFilter = new Items.Filter
			{
				RangeMax = MaxRange,
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
			AutoLoot.SerialToGrabList = new ConcurrentQueue<SerialToGrab>();
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