using Assistant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class Scavenger
	{
		[Serializable]
		public class ScavengerItem
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

			public ScavengerItem(string name, int graphics, int color, bool selected, List<Property> properties)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_Selected = selected;
				m_Properties = properties;
			}
		}

		internal class ScavengerList
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

			public ScavengerList(string description, int delay, int bag, bool selected, int range)
			{
				m_Description = description;
				m_Delay = delay;
				m_Bag = bag;
				m_Selected = selected;
				m_Range = range;
			}
		}

		private static bool m_AutoMode;

		internal static bool AutoMode
		{
			get { return m_AutoMode; }
			set { m_AutoMode = value; }
		}

		internal static string ScavengerListName
		{
			get
			{
				return Assistant.Engine.MainWindow.ScavengerListSelect.Text;
			}

			set
			{
				Assistant.Engine.MainWindow.ScavengerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerListSelect.Text = value));
			}
		}

		internal static int ScavengerDelay
		{
			get
			{
				Int32.TryParse(Assistant.Engine.MainWindow.ScavengerDragDelay.Text, out int delay);
				return delay;
			}

			set
			{
				Assistant.Engine.MainWindow.ScavengerDragDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerDragDelay.Text = value.ToString()));
			}
		}

		internal static int MaxRange
		{
			get
			{
				Int32.TryParse(Assistant.Engine.MainWindow.ScavengerRange.Text, out int range);
				return range;
			}

			set
			{
				Assistant.Engine.MainWindow.ScavengerRange.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerRange.Text = value.ToString()));
			}
		}

		internal static int ScavengerBag
		{
			get
			{
				int serialBag = 0;

				try
				{
					serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.ScavengerContainerLabel.Text, 16);
				}
				catch
				{ }

				return serialBag;
			}

			set
			{
				Assistant.Engine.MainWindow.ScavengerContainerLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerContainerLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (!Assistant.Engine.Running)
				return;

			Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.SelectedIndex = Assistant.Engine.MainWindow.ScavengerLogBox.Items.Count - 1));
			if (Assistant.Engine.MainWindow.ScavengerLogBox.Items.Count > 300)
				Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.Items.Clear()));
		}

		internal static void RefreshLists()
		{
			RazorEnhanced.Settings.Scavenger.ListsRead(out List<ScavengerList> lists);

			ScavengerList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.ScavengerListSelect.Text)
				return;

			Assistant.Engine.MainWindow.ScavengerListSelect.Items.Clear();
			foreach (ScavengerList l in lists)
			{
				Assistant.Engine.MainWindow.ScavengerListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = Assistant.Engine.MainWindow.ScavengerListSelect.Items.IndexOf(l.Description);
					ScavengerDelay = l.Delay;
					ScavengerBag = l.Bag;
				}
			}
		}

		internal static void InitGrid()
		{
			RazorEnhanced.Settings.Scavenger.ListsRead(out List<ScavengerList> lists);

			Assistant.Engine.MainWindow.ScavengerDataGridView.Rows.Clear();

			foreach (ScavengerList l in lists)
			{
				if (l.Selected)
				{
					List<Scavenger.ScavengerItem> items = Settings.Scavenger.ItemsRead(l.Description);

					foreach (ScavengerItem item in items)
					{
						string color = "All";
						if (item.Color != -1)
							color = "0x" + item.Color.ToString("X4");

						Assistant.Engine.MainWindow.ScavengerDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, "0x" + item.Graphics.ToString("X4"), color, item.Properties });
					}

					break;
				}
			}
		}

		internal static void CopyTable()
		{
			Settings.Scavenger.ClearList(Assistant.Engine.MainWindow.ScavengerListSelect.Text); // Rimuove vecchi dati dal save

			foreach (DataGridViewRow row in Assistant.Engine.MainWindow.ScavengerDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[3].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[3].Value, 16);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				if (row.Cells[4].Value != null)
					Settings.Scavenger.ItemInsert(Assistant.Engine.MainWindow.ScavengerListSelect.Text, new ScavengerItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, (List<ScavengerItem.Property>)row.Cells[4].Value));
				else
					Settings.Scavenger.ItemInsert(Assistant.Engine.MainWindow.ScavengerListSelect.Text, new ScavengerItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, new List<ScavengerItem.Property>()));
			}

			Settings.Save(); // Salvo dati
		}

		internal static void AddList(string newList)
		{
			Settings.Scavenger.ListInsert(newList, ScavengerDelay, 0, MaxRange);

			RefreshLists();
			InitGrid();
		}

		internal static void RemoveList(string list)
		{
			if (Settings.Scavenger.ListExists(list))
			{
				Settings.Scavenger.ListDelete(list);
			}

			RefreshLists();
			InitGrid();
		}

		internal static void AddItemToList(string name, int graphics, int color)
		{
			Assistant.Engine.MainWindow.ScavengerDataGridView.Rows.Add(new object[] { "False", name, "0x" + graphics.ToString("X4"), "0x" + color.ToString("X4"), new List<ScavengerItem.Property>()});
			CopyTable();
		}

		internal static void Engine(List<ScavengerItem> scavengerItemList, int mseconds, Items.Filter filter)
		{
			List<Item> itemsOnGround = RazorEnhanced.Items.ApplyFilter(filter);
			if (ClientCommunication.ServerEncrypted)
			{
				Items.Filter osifilter = filter;
				osifilter.Movable = false;
                itemsOnGround.AddRange(RazorEnhanced.Items.ApplyFilter(osifilter));
            }

			foreach (ScavengerItem scavengerItem in scavengerItemList)
			{
				if (World.Player.IsGhost)
				{
					ResetIgnore();
					Thread.Sleep(2000);
					return;
				}

				if (!scavengerItem.Selected)
					continue;

				foreach (RazorEnhanced.Item itemGround in itemsOnGround)
				{
					if (DragDropManager.HoldingItem)
						continue;

					if (scavengerItem.Color == -1)          // Colore ALL
					{
						if (itemGround.ItemID == scavengerItem.Graphics)
						{
							GrabItem(scavengerItem, itemGround);
						}
					}
					else
					{
						if (itemGround.ItemID == scavengerItem.Graphics && itemGround.Hue == scavengerItem.Color)
						{
							GrabItem(scavengerItem, itemGround);
						}
					}
				}
			}
		}

		internal static void GrabItem(ScavengerItem scavengerItem, Item itemGround)
		{
			if (DragDropManager.ScavengerSerialToGrab.Contains(itemGround.Serial))
				return;

			if (scavengerItem.Properties.Count > 0) // Item con props
			{
				Items.WaitForProps(itemGround, 1000);

				bool propsOk = false;
				foreach (ScavengerItem.Property props in scavengerItem.Properties) // Scansione e verifica props
				{
					int propsSuItemDaLootare = RazorEnhanced.Items.GetPropValue(itemGround, props.Name);
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
					DragDropManager.ScavengerSerialToGrab.Enqueue(itemGround.Serial);
			}
			else // Item Senza props
			{
				DragDropManager.ScavengerSerialToGrab.Enqueue(itemGround.Serial);
			}
		}

		public static void ResetIgnore()
		{
			DragDropManager.ScavengerSerialToGrab = new ConcurrentQueue<int>();
		}

		private static Items.Filter m_itemfilter = new Items.Filter
		{
			Movable = true,
			OnGround = 1,
			Enabled = true
		};

		internal static void AutoRun()
		{
			if (!Assistant.Engine.Running)
				return;

			// Genero filtro item
			m_itemfilter.RangeMax = MaxRange;

			// Check bag
			Assistant.Item bag = Assistant.World.FindItem(ScavengerBag);
			if (bag != null)
			{
				if (bag.RootContainer != World.Player)
				{
					if (Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						Misc.SendMessage("Scavenger: Invalid Bag, Switch to backpack", 945);
					AddLog("Invalid Bag, Switch to backpack");
					ScavengerBag = (int)World.Player.Backpack.Serial.Value;
				}
			}
			else
			{
				if (Settings.General.ReadBool("ShowMessageFieldCheckBox"))
					Misc.SendMessage("Scavenger: Invalid Bag, Switch to backpack", 945);
				AddLog("Invalid Bag, Switch to backpack");
				ScavengerBag = (int)World.Player.Backpack.Serial.Value;
			}

			Engine(Settings.Scavenger.ItemsRead(ScavengerListName), ScavengerDelay, m_itemfilter);
		}

		// Funzioni da script
		public static void RunOnce(List<ScavengerItem> scavengerList, int mseconds, Items.Filter filter)
		{
			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: Scavenger.Start: Scavenger already running");
			}
			else
			{
				Engine(scavengerList, mseconds, filter);
			}
		}

		public static void Start()
		{
			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: Scavenger.Start: Scavenger already running");
			}
			else
				Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = true));
		}

		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: Scavenger.Stop: Scavenger already sleeping");
			}
			else
				Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.ScavengerCheckBox.Checked;
		}

		public static void ChangeList(string nomelista)
		{
			if (!Assistant.Engine.MainWindow.ScavengerListSelect.Items.Contains(nomelista))
			{
				Scripts.SendMessageScriptError("Script Error: Scavenger.ChangeList: Scavenger list: " + nomelista + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false));
					Assistant.Engine.MainWindow.ScavengerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = Assistant.Engine.MainWindow.ScavengerListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = true));
				}
				else
				{
					Assistant.Engine.MainWindow.ScavengerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = Assistant.Engine.MainWindow.ScavengerListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}
	}
}