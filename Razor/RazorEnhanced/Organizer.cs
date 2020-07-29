using Assistant;
using Assistant.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class Organizer
	{
		private static int m_dragdelay;
		private static int m_sourcebag;
		private static int m_destinationbag;
		private static string m_organizerlist;

		[Serializable]
		public class OrganizerItem : ListAbleItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private int m_amount;
			public int Amount { get { return m_amount; } }

			[JsonProperty("Selected")]
			internal bool Selected { get; set; }

			public OrganizerItem(string name, int graphics, int color, int amount, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_amount = amount;
				Selected = selected;
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
			get { return m_organizerlist; }
			set { m_organizerlist = value; }
		}

		internal static int OrganizerDelay
		{
			get { return m_dragdelay; }

			set
			{
				m_dragdelay = value;
				Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerDragDelay.Text = value.ToString());
			}
		}

		internal static int OrganizerSource
		{
			get { return m_sourcebag; }

			set
			{
				m_sourcebag = value;
				Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerSourceLabel.Text = "0x" + value.ToString("X8"));
			}
		}

		internal static int OrganizerDestination
		{
			get { return m_destinationbag; }

			set
			{
				m_destinationbag = value;
				Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerDestinationLabel.Text = "0x" + value.ToString("X8"));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (!Client.Running)
				return;

			Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerLogBox.Items.Add(addlog));
			Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerLogBox.SelectedIndex = s.OrganizerLogBox.Items.Count - 1);
			if (Assistant.Engine.MainWindow.OrganizerLogBox.Items.Count > 300)
				Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerLogBox.Items.Clear());
		}

		internal static void RefreshLists()
		{
			List<OrganizerList> lists = Settings.Organizer.ListsRead();

			OrganizerList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.OrganizerListSelect.Text)
				return;

			Assistant.Engine.MainWindow.OrganizerListSelect.Items.Clear();
			foreach (OrganizerList l in lists)
			{
				Assistant.Engine.MainWindow.OrganizerListSelect.Items.Add(l.Description);

				if (!l.Selected)
					continue;

				Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = Assistant.Engine.MainWindow.OrganizerListSelect.Items.IndexOf(l.Description);
				OrganizerDelay = l.Delay;
				OrganizerSource = l.Source;
				OrganizerDestination = l.Destination;
				OrganizerListName = l.Description;
			}
		}

		internal static void CopyTable()
		{
			Settings.Organizer.ClearList(Assistant.Engine.MainWindow.OrganizerListSelect.Text); // Rimuove vecchi dati dal save

			foreach (DataGridViewRow row in Assistant.Engine.MainWindow.OrganizerDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[3].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[3].Value, 16);

				int amount = 0;
				if ((string)row.Cells[4].Value == "All")
					amount = -1;
				else
					amount = Convert.ToInt32((string)row.Cells[4].Value);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.Organizer.ItemInsert(Assistant.Engine.MainWindow.OrganizerListSelect.Text, new OrganizerItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, amount, check));
			}

			Settings.Save(); // Salvo dati
		}

		internal static void InitGrid()
		{
			List<OrganizerList> lists = Settings.Organizer.ListsRead();

			Assistant.Engine.MainWindow.OrganizerDataGridView.Rows.Clear();

			foreach (OrganizerList l in lists)
			{
				if (l.Selected)
				{
					List<Organizer.OrganizerItem> items = Settings.Organizer.ItemsRead(l.Description);

					foreach (OrganizerItem item in items)
					{
						string color = "All";
						if (item.Color != -1)
							color = "0x" + item.Color.ToString("X4");

						string amount = "All";
						if (item.Amount != -1)
							amount = item.Amount.ToString();

						Assistant.Engine.MainWindow.OrganizerDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, "0x" + item.Graphics.ToString("X4"), color, amount });
					}

					break;
				}
			}
		}
		internal static void CloneList(string newList)
		{
			RazorEnhanced.Settings.Organizer.ListInsert(newList, RazorEnhanced.Organizer.OrganizerDelay, OrganizerSource, OrganizerDestination);

			foreach (DataGridViewRow row in Assistant.Engine.MainWindow.OrganizerDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[3].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[3].Value, 16);

				int amount = 0;
				if ((string)row.Cells[4].Value == "All")
					amount = -1;
				else
					amount = Convert.ToInt32((string)row.Cells[4].Value);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.Organizer.ItemInsert(newList, new OrganizerItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, amount, check));
			}

			Settings.Save(); // Salvo dati
			RazorEnhanced.Organizer.RefreshLists();
			RazorEnhanced.Organizer.InitGrid();
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.Organizer.ListInsert(newList, RazorEnhanced.Organizer.OrganizerDelay, 0, 0);

			RazorEnhanced.Organizer.RefreshLists();
			RazorEnhanced.Organizer.InitGrid();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.Organizer.ListExists(list))
			{
				RazorEnhanced.Settings.Organizer.ListDelete(list);
			}

			RazorEnhanced.Organizer.RefreshLists();
			RazorEnhanced.Organizer.InitGrid();
		}

		internal static void AddItemToList(string name, int graphics, int color)
		{
			Assistant.Engine.MainWindow.OrganizerDataGridView.Rows.Add(new object[] { "True", name, "0x" + graphics.ToString("X4"), "0x" + color.ToString("X4"), "All" });
			CopyTable();
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

			// Check if container is updated
			RazorEnhanced.Organizer.AddLog("- Refresh Source Container");
			Items.WaitForContents(sourceBag, 1000);
			Thread.Sleep(mseconds);

			// Inizia scansione
			foreach (RazorEnhanced.Item oggettoContenuto in sourceBag.Contains)
			{
				foreach (OrganizerItem oggettoDaLista in organizerItemList)
				{
					if (!oggettoDaLista.Selected)
						continue;

					if (oggettoContenuto.ItemID != oggettoDaLista.Graphics || !ColorCheck(oggettoDaLista.Color, oggettoContenuto.Hue))
						continue;

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
							RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount in Source container: " + oggettoContenuto.Amount);
							RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount to move " + oggettoDaLista.Amount);
							RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, 0);
							Thread.Sleep(mseconds);
						}
						else  // Caso che lo stack sia superiore (sposta solo un blocco)
						{
							RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount in Source container: " + oggettoContenuto.Amount);
							RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Amount to move " + oggettoDaLista.Amount);
							RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, oggettoDaLista.Amount);
							Thread.Sleep(mseconds);
						}
					}
				}
			}

			RazorEnhanced.Organizer.AddLog("Finish!");
			if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
				RazorEnhanced.Misc.SendMessage("Enhanced Organizer: Finish!", 945, true);
			Assistant.Engine.MainWindow.OrganizerFinishWork();
			return 0;
		}


        public static void RunOnce(string organizerName, int sourceBag, int destBag, int dragDelay)
        {
            // Check Bag
            if (sourceBag == -1)
            {
                sourceBag = m_sourcebag;
            }
            Assistant.Item sbag = Assistant.World.FindItem(sourceBag);
            if (sbag == null)
            {
                AddLog("Invalid Source Bag");
                return;
            }

            if (destBag == -1)
            {
                destBag = m_destinationbag;
            }
            Assistant.Item dbag = Assistant.World.FindItem(destBag);
            if (dbag == null)
            {
                AddLog("Invalid Destination Bag");
                return;
            }

            if (dragDelay == -1)
            {
                dragDelay = m_dragdelay;
            }

            List<RazorEnhanced.Organizer.OrganizerItem>  organizerList = Settings.Organizer.ItemsRead(organizerName);

            int exit = Engine(organizerList, dragDelay, sourceBag, destBag);
        }

            internal static void Engine()
		{
			// Check Bag
			Assistant.Item sbag = Assistant.World.FindItem(m_sourcebag);
			if (sbag == null)
			{
				if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
					Misc.SendMessage("Organizer: Invalid Source Bag", 945, true);
				AddLog("Invalid Source Bag");
				Assistant.Engine.MainWindow.OrganizerFinishWork();
				return;
			}
			Assistant.Item dbag = Assistant.World.FindItem(m_destinationbag);
			if (dbag == null)
			{
				if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
					Misc.SendMessage("Organizer: Invalid Destination Bag", 945, true);
				AddLog("Invalid Destination Bag");
				Assistant.Engine.MainWindow.OrganizerFinishWork();
				return;
			}

			int exit = Engine(Settings.Organizer.ItemsRead(m_organizerlist), m_dragdelay, m_sourcebag, m_destinationbag);
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
			if (Status())
			{
				m_OrganizerThread.Abort();
			}
		}

		// Funzioni da script
		public static void FStart()
		{
			if (Assistant.Engine.MainWindow.OrganizerExecute.Enabled == true)
				Assistant.Engine.MainWindow.OrganizerStartExec();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Organizer.FStart: Organizer already running");
			}
		}

		public static void FStop()
		{
			if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true)
				Assistant.Engine.MainWindow.OrganizerStopExec();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Organizer.FStart: Organizer not running");
			}
		}

		public static bool Status()
		{
			if (m_OrganizerThread != null && ((m_OrganizerThread.ThreadState & ThreadState.Running) != 0 || (m_OrganizerThread.ThreadState & ThreadState.WaitSleepJoin) != 0 || (m_OrganizerThread.ThreadState & ThreadState.AbortRequested) != 0))
				return true;
			else
				return false;
		}

		public static void ChangeList(string listName)
		{
			if (!UpdateListParam(listName))
			{
				Scripts.SendMessageScriptError("Script Error: Organizer.ChangeList: Organizer list: " + listName + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true) // Se è in esecuzione forza stop change list e restart
				{
					Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerStop.PerformClick());
					Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerListSelect.SelectedIndex = Assistant.Engine.MainWindow.OrganizerListSelect.Items.IndexOf(listName));  // change list
					Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerExecute.PerformClick());
				}
				else
				{
					Assistant.Engine.MainWindow.SafeAction(s => s.OrganizerListSelect.SelectedIndex = s.OrganizerListSelect.Items.IndexOf(listName));  // change list
				}
			}
		}

		internal static bool UpdateListParam(string listName)
		{
			if (Settings.Organizer.ListExists(listName))
			{
				Settings.Organizer.ListDetailsRead(listName, out int bagsource, out int bagdestination, out int delay);
				OrganizerDelay = delay;
				OrganizerSource = bagsource;
				OrganizerDestination = bagdestination;
				OrganizerListName = listName;
				return true;
			}
			return false;
		}
	}
}
