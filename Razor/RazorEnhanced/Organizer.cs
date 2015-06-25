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
	public class Organizer
	{
		[Serializable]
		public class OrganizerItem
		{

			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private int m_amount;
			public int Amount { get { return m_amount; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public OrganizerItem(string name, int graphics, int color, int amount, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_amount = amount;
				m_Selected = selected;
			}
		}

		internal class OrganizerList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Delay;
			internal int Delay { get { return m_Delay; } }

			private int m_Source;
			internal int Source { get { return m_Source; } }

			private int m_Destination;
			internal int Destination { get { return m_Destination; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public OrganizerList(string description, int delay, int source, int destination, bool selected)
			{
				m_Description = description;
				m_Delay = delay;
				m_Source = source;
				m_Destination = destination;
				  m_Selected = selected;
			}
		}

		internal static string OrganizerListName
		{
			get
			{
				return (string)Assistant.Engine.MainWindow.OrganizerListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.OrganizerListSelect.Text));
			}

			set
			{
				Assistant.Engine.MainWindow.OrganizerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerListSelect.Text = value));
			}
		}

		internal static int OrganizerDelay
		{
			get
			{
				int delay = 100;
				Assistant.Engine.MainWindow.OrganizerDragDelay.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.OrganizerDragDelay.Text, out delay)));
				return delay;
			}

			set
			{
				Assistant.Engine.MainWindow.OrganizerDragDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerDragDelay.Text = value.ToString()));
			}
		}

		internal static int OrganizerSource
		{
			get
			{
				int serialBag = 0;

				try
				{
					serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.OrganizerSourceLabel.Text, 16);
                }
                catch
                {
                }

				return serialBag;
			}

			set
			{
                Assistant.Engine.MainWindow.OrganizerSourceLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerSourceLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static int OrganizerDestination
		{
			get
			{
				int serialBag = 0;

				try
				{
                    serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.OrganizerDestinationLabel.Text, 16);
                }
                catch
                {
                }

				return serialBag;
			}

			set
			{
                Assistant.Engine.MainWindow.OrganizerDestinationLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerDestinationLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static void AddLog(string addlog)
		{
			Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.SelectedIndex = Assistant.Engine.MainWindow.OrganizerLogBox.Items.Count - 1));
            if (Assistant.Engine.MainWindow.OrganizerLogBox.Items.Count >300)
                Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.Items.Clear()));
		}

		internal static void RefreshLists()
		{
			List<OrganizerList> lists;
			RazorEnhanced.Settings.Organizer.ListsRead(out lists);

            if (lists.Count == 0)
                Assistant.Engine.MainWindow.OrganizerListView.Items.Clear();

			OrganizerList selectedList = lists.Where(l => l.Selected).FirstOrDefault();
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.OrganizerListSelect.Text)
				return;

			Assistant.Engine.MainWindow.OrganizerListSelect.Items.Clear();
			foreach (OrganizerList l in lists)
			{
				Assistant.Engine.MainWindow.OrganizerListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = Assistant.Engine.MainWindow.OrganizerListSelect.Items.IndexOf(l.Description);
					OrganizerDelay = l.Delay;
					OrganizerSource = l.Source;
					OrganizerDestination = l.Destination;
				}
			}
		}

		internal static void RefreshItems()
		{
			List<OrganizerList> lists;
			RazorEnhanced.Settings.Organizer.ListsRead(out lists);

			Assistant.Engine.MainWindow.OrganizerListView.Items.Clear();
			foreach (OrganizerList l in lists)
			{
				if (l.Selected)
				{
					List<Organizer.OrganizerItem> items;
					RazorEnhanced.Settings.Organizer.ItemsRead(l.Description, out items);

					foreach (OrganizerItem item in items)
					{
						ListViewItem listitem = new ListViewItem();

						listitem.Checked = item.Selected;

						listitem.SubItems.Add(item.Name);
						listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));

						if (item.Color == -1)
							listitem.SubItems.Add("All");
						else
                            listitem.SubItems.Add("0x" + item.Color.ToString("X4"));

						if (item.Amount == -1)
							listitem.SubItems.Add("All");
						else
							listitem.SubItems.Add(item.Amount.ToString());

						Assistant.Engine.MainWindow.OrganizerListView.Items.Add(listitem);
					}
				}
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<OrganizerItem> items;
			RazorEnhanced.Settings.Organizer.ItemsRead(OrganizerListName, out items);

			if (items.Count != Assistant.Engine.MainWindow.OrganizerListView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.OrganizerListView.Items[i];
			OrganizerItem old = items[i];

			if (lvi != null && old != null)
			{
				OrganizerItem item = new Organizer.OrganizerItem(old.Name, old.Graphics, old.Color, old.Amount, lvi.Checked);
				RazorEnhanced.Settings.Organizer.ItemReplace(RazorEnhanced.Organizer.OrganizerListName, i, item);
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.Organizer.ListInsert(newList, RazorEnhanced.Organizer.OrganizerDelay, 0, 0);

			RazorEnhanced.Organizer.RefreshLists();
			RazorEnhanced.Organizer.RefreshItems();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.Organizer.ListExists(list))
			{
				RazorEnhanced.Settings.Organizer.ListDelete(list);
			}

			RazorEnhanced.Organizer.RefreshLists();
			RazorEnhanced.Organizer.RefreshItems();
		}

		internal static void AddItemToList(string name, int graphics, int amount, int color)
		{
			OrganizerItem item = new OrganizerItem(name, graphics, color, amount, false);

			string selection = Assistant.Engine.MainWindow.OrganizerListSelect.Text;

			if (RazorEnhanced.Settings.Organizer.ListExists(selection))
			{
				if (!RazorEnhanced.Settings.Organizer.ItemExists(selection, item))
					RazorEnhanced.Settings.Organizer.ItemInsert(selection, item);
			}

			RazorEnhanced.Organizer.RefreshItems();
		}

		internal static void ModifyItemInList(string name, int graphics, int color, int amount, bool selected, OrganizerItem old, int index)
		{
			OrganizerItem item = new OrganizerItem(name, graphics, color, amount, selected);

			string selection = Assistant.Engine.MainWindow.OrganizerListSelect.Text;

            if (RazorEnhanced.Settings.Organizer.ListExists(selection))
			{
				if (RazorEnhanced.Settings.Organizer.ItemExists(selection, old))
					RazorEnhanced.Settings.Organizer.ItemReplace(selection, index, item);
			}

            RazorEnhanced.Organizer.RefreshItems();
		}

        private static bool ColorCheck(int colorDaLista, int colorDaItem)
        {
            if (colorDaLista == -1) // Wildcard colore
                return true;
            else
                if (colorDaLista == colorDaItem) // Match OK
                    return true;
                else // Match fallito
                    return false;
        }

        internal static int Engine(List<OrganizerItem> organizerItemList, int mseconds, int sourceBagserail, int destinationBagserial)
		{
            Item sourceBag = Items.FindBySerial(sourceBagserail);
            Item destinationBag = Items.FindBySerial(destinationBagserial);

			// Apre le bag per item contenuti
            RazorEnhanced.Organizer.AddLog("- Refresh Source Container");
            Items.UseItem(sourceBag);
            Thread.Sleep(mseconds);
            RazorEnhanced.Organizer.AddLog("- Refresh Destination Container");
			Items.UseItem(destinationBag);
            Thread.Sleep(mseconds);

			// Inizia scansione 
			foreach (RazorEnhanced.Item oggettoContenuto in sourceBag.Contains)
			{
				foreach (OrganizerItem oggettoDaLista in organizerItemList)
				{
                    if (!oggettoDaLista.Selected)
                        continue;

                    if (oggettoContenuto.ItemID == oggettoDaLista.Graphics && ColorCheck(oggettoDaLista.Color, oggettoContenuto.Hue))     // Verifico il match fra colore e grafica
					{
						// Controllo amount e caso -1
						if (oggettoDaLista.Amount == -1) // Sposta senza contare
						{
                            RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount in Source container: " + oggettoContenuto.Amount);
                            RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount to move: All ");
							RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, 0);
							Thread.Sleep(mseconds);
						}
						else   // Caso con limite quantita'
						{
                            if (oggettoContenuto.Amount <= oggettoDaLista.Amount)     // Caso che lo stack da spostare sia minore del limite di oggetti 
							{
                                RazorEnhanced.Organizer.AddLog("n");

                                RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount in Source container: " + oggettoContenuto.Amount);
                                RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount to move " + oggettoDaLista.Amount);
								RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, 0);
								Thread.Sleep(mseconds);
							}
							else  // Caso che lo stack sia superiore (sposta solo un blocco)
							{
                                RazorEnhanced.Organizer.AddLog("s");

                                RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount in Source container: " + oggettoContenuto.Amount);
                                RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount to move " + oggettoDaLista.Amount);
                                RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, oggettoDaLista.Amount);
								Thread.Sleep(mseconds);
							}	
						}
					}
				}
			}

            RazorEnhanced.Organizer.AddLog("Finish!");
            RazorEnhanced.Misc.SendMessage("Enhanced Organizer: Finish!");
            Assistant.Engine.MainWindow.OrganizerFinishWork();
			return 0;
		}

		internal static void Engine()
		{
            // Check Bag
            Assistant.Item sbag = Assistant.World.FindItem(OrganizerSource);
            if (sbag == null)
            {
                Misc.SendMessage("Organizer: Invalid Source Bag");
                AddLog("Invalid Source Bag");
                Assistant.Engine.MainWindow.OrganizerFinishWork();
                return;
            }
            Assistant.Item dbag = Assistant.World.FindItem(OrganizerDestination);
            if (dbag == null)
            {
                Misc.SendMessage("Organizer: Invalid Destination Bag");
                AddLog("Invalid Destination Bag");
                Assistant.Engine.MainWindow.OrganizerFinishWork();
                return;
            }

			List<Organizer.OrganizerItem> items;
			string list = Organizer.OrganizerListName;
			RazorEnhanced.Settings.Organizer.ItemsRead(list, out items);

            int exit = Engine(items, OrganizerDelay, OrganizerSource, OrganizerDestination);
		}

		private static Thread m_OrganizerThread;

		internal static void Start()
		{
			if (m_OrganizerThread == null ||
						(m_OrganizerThread != null && m_OrganizerThread.ThreadState != ThreadState.Running &&
						m_OrganizerThread.ThreadState != ThreadState.Unstarted &&
						m_OrganizerThread.ThreadState != ThreadState.WaitSleepJoin)
					)
			{
				m_OrganizerThread = new Thread(Organizer.Engine);
				m_OrganizerThread.Start();
			}

		}

		internal static void ForceStop()
		{
			if (m_OrganizerThread != null && m_OrganizerThread.ThreadState != ThreadState.Stopped)
			{
				m_OrganizerThread.Abort();
			}
		}
        // Funzioni da script

        public static void FStart()
        {
            if (Assistant.Engine.MainWindow.OrganizerExecute.Enabled == true)
                Assistant.Engine.MainWindow.OrganizerExecute.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerExecute.PerformClick()));
            else
                Misc.SendMessage("Script Error: Organizer.FStart: Organizer already running");
        }

        public static void FStop()
        {
            if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true)
                Assistant.Engine.MainWindow.OrganizerStop.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerStop.PerformClick()));
            else
                Misc.SendMessage("Script Error: Organizer.FStart: Organizer not running");
        }

        public static bool Status()
        {
            if (m_OrganizerThread != null && m_OrganizerThread.ThreadState != ThreadState.Stopped)
                return true;
            else
                return false;
        }
        public static void ChangeList(string nomelista)
        {
            bool ListaOK = false;
            for (int i = 0; i < Assistant.Engine.MainWindow.OrganizerListSelect.Items.Count; i++)
            {
                if (nomelista == Assistant.Engine.MainWindow.OrganizerListSelect.GetItemText(Assistant.Engine.MainWindow.OrganizerListSelect.Items[i]))
                    ListaOK = true;
            }
            if (!ListaOK)
                Misc.SendMessage("Script Error: Organizer.ChangeList: Organizer list: " + nomelista + " not exist");
            else
            {
                if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true) // Se è in esecuzione forza stop cambio lista e restart
                {
                    Assistant.Engine.MainWindow.OrganizerStop.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerStop.PerformClick()));
                    Assistant.Engine.MainWindow.OrganizerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = Assistant.Engine.MainWindow.OrganizerListSelect.Items.IndexOf(nomelista)));  // cambio lista
                    Assistant.Engine.MainWindow.OrganizerExecute.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerExecute.PerformClick()));
                }
                else
                {
                    Assistant.Engine.MainWindow.OrganizerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = Assistant.Engine.MainWindow.OrganizerListSelect.Items.IndexOf(nomelista)));  // cambio lista
                }
            }
        }
	}
}
