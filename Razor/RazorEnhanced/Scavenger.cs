using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

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

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public ScavengerList(string description, int delay, int bag, bool selected)
			{
				m_Description = description;
				m_Delay = delay;
				m_Bag = bag;
				m_Selected = selected;
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
				return (string)Assistant.Engine.MainWindow.ScavengerListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.ScavengerListSelect.Text));
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
				int delay = 100;
				Assistant.Engine.MainWindow.ScavengerDragDelay.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.ScavengerDragDelay.Text, out delay)));
				return delay;
			}

			set
			{
				Assistant.Engine.MainWindow.ScavengerDragDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerDragDelay.Text = value.ToString()));
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
			Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.SelectedIndex = Assistant.Engine.MainWindow.ScavengerLogBox.Items.Count - 1));
            if (Assistant.Engine.MainWindow.ScavengerLogBox.Items.Count > 300)
                Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.Items.Clear()));
		}

		internal static void RefreshLists()
		{
			List<ScavengerList> lists;
			RazorEnhanced.Settings.Scavenger.ListsRead(out lists);

            if (lists.Count == 0)
                Assistant.Engine.MainWindow.ScavengerListView.Items.Clear();

			ScavengerList selectedList = lists.Where(l => l.Selected).FirstOrDefault();
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

		internal static void RefreshItems()
		{
			List<ScavengerList> lists;
			RazorEnhanced.Settings.Scavenger.ListsRead(out lists);

			Assistant.Engine.MainWindow.ScavengerListView.Items.Clear();
			foreach (ScavengerList l in lists)
			{
				if (l.Selected)
				{
					List<Scavenger.ScavengerItem> items;
					RazorEnhanced.Settings.Scavenger.ItemsRead(l.Description, out items);

					foreach (ScavengerItem item in items)
					{
						ListViewItem listitem = new ListViewItem();

						listitem.Checked = item.Selected;

						listitem.SubItems.Add(item.Name);
						listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));

						if (item.Color == -1)
							listitem.SubItems.Add("All");
						else
							listitem.SubItems.Add("0x" + item.Color.ToString("X4"));

						Assistant.Engine.MainWindow.ScavengerListView.Items.Add(listitem);
					}
				}
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<ScavengerItem> items;
			RazorEnhanced.Settings.Scavenger.ItemsRead(ScavengerListName, out items);

			if (items.Count != Assistant.Engine.MainWindow.ScavengerListView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.ScavengerListView.Items[i];
			ScavengerItem old = items[i];

			if (lvi != null && old != null)
			{
				ScavengerItem item = new Scavenger.ScavengerItem(old.Name, old.Graphics, old.Color, lvi.Checked, old.Properties);
				RazorEnhanced.Settings.Scavenger.ItemReplace(RazorEnhanced.Scavenger.ScavengerListName, i, item);
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.Scavenger.ListInsert(newList, RazorEnhanced.Scavenger.ScavengerDelay, 0);

			RazorEnhanced.Scavenger.RefreshLists();
			RazorEnhanced.Scavenger.RefreshItems();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.Scavenger.ListExists(list))
			{
				RazorEnhanced.Settings.Scavenger.ListDelete(list);
			}

			RazorEnhanced.Scavenger.RefreshLists();
			RazorEnhanced.Scavenger.RefreshItems();
		}

		internal static void AddItemToList(string name, int graphics, int color)
		{
			List<ScavengerItem.Property> propsList = new List<ScavengerItem.Property>();
			ScavengerItem item = new ScavengerItem(name, graphics, color, false, propsList);

			string selection = Assistant.Engine.MainWindow.ScavengerListSelect.Text;

			if (RazorEnhanced.Settings.Scavenger.ListExists(selection))
			{
				if (!RazorEnhanced.Settings.Scavenger.ItemExists(selection, item))
					RazorEnhanced.Settings.Scavenger.ItemInsert(selection, item);
			}

			RazorEnhanced.Scavenger.RefreshItems();
		}

		internal static void ModifyItemInList(string name, int graphics, int color, bool selected, ScavengerItem old, int index)
		{
			List<ScavengerItem.Property> propsList = old.Properties;
			ScavengerItem item = new ScavengerItem(name, graphics, color, selected, propsList);

			string selection = Assistant.Engine.MainWindow.ScavengerListSelect.Text;

			if (RazorEnhanced.Settings.Scavenger.ListExists(selection))
			{
				if (RazorEnhanced.Settings.Scavenger.ItemExists(selection, old))
					RazorEnhanced.Settings.Scavenger.ItemReplace(selection, index, item);
			}

			RazorEnhanced.Scavenger.RefreshItems();
		}

		internal static void AddPropToItem(string list, int index, Scavenger.ScavengerItem item, string propName, int propMin, int propMax)
		{
			ScavengerItem.Property prop = new ScavengerItem.Property(propName, propMin, propMax);
			item.Properties.Add(prop);
			RazorEnhanced.Settings.Scavenger.ItemReplace(list, index, item);
		}

		internal static int Engine(List<ScavengerItem> scavengerItemList, int mseconds, Items.Filter filter)
		{
			List<Item> itemsOnGround = RazorEnhanced.Items.ApplyFilter(filter);

			foreach (RazorEnhanced.Item itemGround in itemsOnGround)
			{
                if (World.Player.IsGhost)
                {
                    Thread.Sleep(2000);
                    return 0;
                }

				if (World.Player.Weight - 20 > World.Player.MaxWeight)      // Controllo peso
				{
					RazorEnhanced.Scavenger.AddLog("- Max weight reached, Wait untill free some space");
					RazorEnhanced.Misc.SendMessage("SCAVENGER: Max weight reached, Wait untill free some space");
                    Thread.Sleep(2000);
					return -1;
				}

				foreach (ScavengerItem scavengerItem in scavengerItemList)
				{
					if (!scavengerItem.Selected)
						continue;

					if (scavengerItem.Color == -1)          // Colore ALL
					{
						if (itemGround.ItemID == scavengerItem.Graphics)
						{
							GrabItem(scavengerItem, itemGround, mseconds);
						}
					}
					else
					{
						if (itemGround.ItemID == scavengerItem.Graphics && itemGround.Hue == scavengerItem.Color)
						{
							GrabItem(scavengerItem, itemGround, mseconds);
						}
					}
				}

			}

			return 0;
		}

		internal static void GrabItem(ScavengerItem scavengerItem, Item itemGround, int mseconds)
		{
            if (!itemGround.Movable || !itemGround.Visible)
                return;

			if (Utility.DistanceSqrt(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(itemGround.Position.X, itemGround.Position.Y)) <= 3)
			{
				if (scavengerItem.Properties.Count > 0) // Item con props
				{
					RazorEnhanced.Scavenger.AddLog("- Item Match found scan props");

					bool propsOK = false;
					foreach (ScavengerItem.Property props in scavengerItem.Properties) // Scansione e verifica props
					{
						int PropsSuItemDaLootare = RazorEnhanced.Items.GetPropByString(itemGround, props.Name);
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
						RazorEnhanced.Scavenger.AddLog("- Item Match found (0x" + itemGround.Serial.ToString("X8") + ") ... Grabbing");
						RazorEnhanced.Item bag = RazorEnhanced.Items.FindBySerial(ScavengerBag);
						if (bag != null)
						{
							RazorEnhanced.Items.Move(itemGround, bag, 0);
							Thread.Sleep(mseconds);
						}
					}
					else
					{
						RazorEnhanced.Scavenger.AddLog("- Props Match fail!");
					}
				}
				else // Item Senza props     
				{
					RazorEnhanced.Scavenger.AddLog("- Item Match found (0x" + itemGround.Serial.ToString("X8") + ") ... Grabbing");
					RazorEnhanced.Item bag = RazorEnhanced.Items.FindBySerial(ScavengerBag);
					if (bag != null)
					{
						RazorEnhanced.Items.Move(itemGround, bag, 0);
						Thread.Sleep(mseconds);
					}
				}
			}
		}

		internal static void Engine()
		{
            int exit = Int32.MinValue;

			// Genero filtro item
			Items.Filter itemFilter = new Items.Filter();
			itemFilter.RangeMax = 2;
			itemFilter.Movable = true;
			itemFilter.OnGround = true;
			itemFilter.Enabled = true;

            // Check bag
            Assistant.Item bag = Assistant.World.FindItem(ScavengerBag);
            if (bag != null)
            {
                if (bag.RootContainer != World.Player)
                {
                    Misc.SendMessage("Scavenger: Invalid Bag, Switch to backpack");
                    AddLog("Invalid Bag, Switch to backpack");
                    ScavengerBag = (int)World.Player.Backpack.Serial.Value;
                    RazorEnhanced.Settings.Scavenger.ListUpdate(ScavengerListName, RazorEnhanced.Scavenger.ScavengerDelay, (int)World.Player.Backpack.Serial.Value, true);
                }
            }
            else
            {
                Misc.SendMessage("Scavenger: Invalid Bag, Switch to backpack");
                AddLog("Invalid Bag, Switch to backpack");
                ScavengerBag = (int)World.Player.Backpack.Serial.Value;
                RazorEnhanced.Settings.Scavenger.ListUpdate(ScavengerListName, RazorEnhanced.Scavenger.ScavengerDelay, (int)World.Player.Backpack.Serial.Value, true);
            }

			List<Scavenger.ScavengerItem> items;
			string list = Scavenger.ScavengerListName;
			RazorEnhanced.Settings.Scavenger.ItemsRead(list, out items);

			exit = Engine(items, ScavengerDelay, itemFilter);
		}

		// Funzioni da script
		public static int RunOnce(List<ScavengerItem> scavengerList, int mseconds, Items.Filter filter)
		{
			int exit = Int32.MinValue;

			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
			{
				Misc.SendMessage("Script Error: Scavenger.Start: Scavenger already running");
			}
			else
			{
				exit = Engine(scavengerList, mseconds, filter);
			}

			return exit;
		}

		public static void Start()
		{
			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
				Misc.SendMessage("Script Error: Scavenger.Start: Scavenger already running");
			else
				Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = true));
		}

		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == false)
				Misc.SendMessage("Script Error: Scavenger.Stop: Scavenger already sleeping");
			else
				Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.ScavengerCheckBox.Checked;
		}

		public static void ChangeList(string nomelista)
		{
			bool ListaOK = false;
			for (int i = 0; i < Assistant.Engine.MainWindow.ScavengerListSelect.Items.Count; i++)
			{
				if (nomelista == Assistant.Engine.MainWindow.ScavengerListSelect.GetItemText(Assistant.Engine.MainWindow.ScavengerListSelect.Items[i]))
					ListaOK = true;
			}
			if (!ListaOK)
				Misc.SendMessage("Script Error: Scavenger.ChangeList: Scavenger list: " + nomelista + " not exist");
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
