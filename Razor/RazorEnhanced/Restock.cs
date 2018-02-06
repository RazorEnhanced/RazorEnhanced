using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class Restock
	{
		[Serializable]
		public class RestockItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private int m_amountlimit;
			public int AmountLimit { get { return m_amountlimit; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public RestockItem(string name, int graphics, int color, int amountlimit, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_amountlimit = amountlimit;
				m_Selected = selected;
			}
		}

		internal class RestockList
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

			public RestockList(string description, int delay, int source, int destination, bool selected)
			{
				m_Description = description;
				m_Delay = delay;
				m_Source = source;
				m_Destination = destination;
				m_Selected = selected;
			}
		}

		internal static string RestockListName
		{
			get
			{
				return (string)Assistant.Engine.MainWindow.RestockListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.RestockListSelect.Text));
			}

			set
			{
				Assistant.Engine.MainWindow.RestockListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockListSelect.Text = value));
			}
		}

		internal static int RestockDelay
		{
			get
			{
				int delay = 100;
				Assistant.Engine.MainWindow.RestockDragDelay.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.RestockDragDelay.Text, out delay)));
				return delay;
			}

			set
			{
				Assistant.Engine.MainWindow.RestockDragDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockDragDelay.Text = value.ToString()));
			}
		}

		internal static int RestockSource
		{
			get
			{
				int serialBag = 0;

				try
				{
					serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.RestockSourceLabel.Text, 16);
				}
				catch
				{ }

				return serialBag;
			}

			set
			{
				Assistant.Engine.MainWindow.RestockSourceLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockSourceLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static int RestockDestination
		{
			get
			{
				int serialBag = 0;

				try
				{
					serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.RestockDestinationLabel.Text, 16);
				}
				catch
				{ }

				return serialBag;
			}

			set
			{
				Assistant.Engine.MainWindow.RestockDestinationLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockDestinationLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (!Assistant.Engine.Running)
				return;

			Assistant.Engine.MainWindow.RestockLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.RestockLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockLogBox.SelectedIndex = Assistant.Engine.MainWindow.RestockLogBox.Items.Count - 1));
			if (Assistant.Engine.MainWindow.RestockLogBox.Items.Count > 300)
				Assistant.Engine.MainWindow.RestockLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockLogBox.Items.Clear()));
		}

		internal static void RefreshLists()
		{
			List<RestockList> lists;
			RazorEnhanced.Settings.Restock.ListsRead(out lists);

			RestockList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.RestockListSelect.Text)
				return;

			Assistant.Engine.MainWindow.RestockListSelect.Items.Clear();
			foreach (RestockList l in lists)
			{
				Assistant.Engine.MainWindow.RestockListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.RestockListSelect.SelectedIndex = Assistant.Engine.MainWindow.RestockListSelect.Items.IndexOf(l.Description);
					RestockDelay = l.Delay;
					RestockSource = l.Source;
					RestockDestination = l.Destination;
				}
			}
		}

		internal static void CopyTable()
		{
			Settings.Restock.ClearList(Assistant.Engine.MainWindow.RestockListSelect.Text); // Rimuove vecchi dati dal save

			foreach (DataGridViewRow row in Assistant.Engine.MainWindow.RestockDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[3].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[3].Value, 16);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.Restock.ItemInsert(Assistant.Engine.MainWindow.RestockListSelect.Text, new RestockItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, Convert.ToInt32((string)row.Cells[4].Value), check));
			}

			Settings.Save(); // Salvo dati
		}

		internal static void InitGrid()
		{
			List<RestockList> lists;
			RazorEnhanced.Settings.Restock.ListsRead(out lists);

			Assistant.Engine.MainWindow.RestockDataGridView.Rows.Clear();

			foreach (RestockList l in lists)
			{
				if (l.Selected)
				{
					List<Restock.RestockItem> items = Settings.Restock.ItemsRead(l.Description);

					foreach (RestockItem item in items)
					{
						string color = "All";
						if (item.Color != -1)
							color = "0x" + item.Color.ToString("X4");

						Assistant.Engine.MainWindow.RestockDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, "0x" + item.Graphics.ToString("X4"), color, item.AmountLimit.ToString() });
					}

					break;
				}
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.Restock.ListInsert(newList, RazorEnhanced.Restock.RestockDelay, 0, 0);

			RazorEnhanced.Restock.RefreshLists();
			RazorEnhanced.Restock.InitGrid();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.Restock.ListExists(list))
			{
				RazorEnhanced.Settings.Restock.ListDelete(list);
			}

			RazorEnhanced.Restock.RefreshLists();
			RazorEnhanced.Restock.InitGrid();
		}

		internal static void AddItemToList(string name, int graphics, int color)
		{
			Assistant.Engine.MainWindow.RestockDataGridView.Rows.Add(new object[] { "False", name, "0x" + graphics.ToString("X4"), "0x" + color.ToString("X4"), "1" });
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

		internal static int Engine(List<RestockItem> restockItemList, int mseconds, int sourceBagserial, int destinationBagserial)
		{
			Item sourceBag = Items.FindBySerial(sourceBagserial);
			Item destinationBag = Items.FindBySerial(destinationBagserial);

			// Check if container is updated
			RazorEnhanced.Organizer.AddLog("- Refresh Source Container");
			Items.WaitForContents(sourceBag, 1000);
			Thread.Sleep(mseconds);

			foreach (RazorEnhanced.Item oggettoContenuto in sourceBag.Contains)
			{
				foreach (RestockItem oggettoDaLista in restockItemList)
				{
					if (!oggettoDaLista.Selected)
						continue;

					if (oggettoContenuto.ItemID != oggettoDaLista.Graphics || !ColorCheck(oggettoDaLista.Color, oggettoContenuto.Hue))
						continue;

					int amountpresente = RazorEnhanced.Items.ContainerCount(destinationBag.Serial, oggettoDaLista.Graphics, oggettoDaLista.Color);
					AddLog("Detected:" + amountpresente + " Item: 0x" + oggettoDaLista.Graphics.ToString("X4") + " on destination bag.");
					int left = oggettoDaLista.AmountLimit - amountpresente;
					if (left > 0)
					{
						AddLog("Left:" + left + " Item: 0x" + oggettoDaLista.Graphics.ToString("X4") + " on destination bag.");
						if (oggettoContenuto.Amount > left)
						{
							AddLog("Moving:" + left + " Item: 0x" + oggettoDaLista.Graphics.ToString("X4") + " to destination bag.");
							RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, left);
							Thread.Sleep(mseconds);
						}
						else
						{
							AddLog("Moving:" + oggettoContenuto.Amount + " Item: 0x" + oggettoDaLista.Graphics.ToString("X4") + " to destination bag.");
							RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, 0);
							Thread.Sleep(mseconds);
						}
					}
					else
					{
						AddLog("Item: 0x" + oggettoDaLista.Graphics.ToString("X4") + " limit reached.");
					}
				}
			}

			RazorEnhanced.Restock.AddLog("Finish!");
			if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
				RazorEnhanced.Misc.SendMessage("Enhanced Restock: Finish!", 945);
			Assistant.Engine.MainWindow.RestockFinishWork();
			return 0;
		}

		internal static void Engine()
		{
			// Check Bag
			Assistant.Item sbag = Assistant.World.FindItem(RestockSource);
			if (sbag == null)
			{
				if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
					Misc.SendMessage("Restock: Invalid Source Bag", 945);
				AddLog("Invalid Source Bag");
				Assistant.Engine.MainWindow.RestockFinishWork();
				return;
			}
			Assistant.Item dbag = Assistant.World.FindItem(RestockDestination);
			if (dbag == null)
			{
				if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
					Misc.SendMessage("Restock: Invalid Destination Bag", 945);
				AddLog("Invalid Destination Bag");
				Assistant.Engine.MainWindow.RestockFinishWork();
				return;
			}

			int exit = Engine(Settings.Restock.ItemsRead(RestockListName), RestockDelay, RestockSource, RestockDestination);
		}

		private static Thread m_RestockThread;

		internal static void Start()
		{
			if (m_RestockThread == null ||
						(m_RestockThread != null && m_RestockThread.ThreadState != ThreadState.Running &&
						m_RestockThread.ThreadState != ThreadState.Unstarted &&
						m_RestockThread.ThreadState != ThreadState.WaitSleepJoin)
					)
			{
				RazorEnhanced.DragDropManager.HoldingItem = false;
				m_RestockThread = new Thread(Restock.Engine);
				m_RestockThread.Start();
			}
		}

		internal static void ForceStop()
		{
			if (m_RestockThread != null && m_RestockThread.ThreadState != ThreadState.Stopped)
			{
				RazorEnhanced.DragDropManager.HoldingItem = false;
				m_RestockThread.Abort();
			}
		}

		// Funzioni da script

		public static void FStart()
		{
			if (Assistant.Engine.MainWindow.RestockExecute.Enabled == true)
				Assistant.Engine.MainWindow.RestockStartExec();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Restock.FStart: Restock already running");
			}
		}

		public static void FStop()
		{
			if (Assistant.Engine.MainWindow.RestockExecute.Enabled == true)
				Assistant.Engine.MainWindow.RestockStopExec();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Restock.FStart: Restock not running");
			}
		}

		public static bool Status()
		{
			if (m_RestockThread != null && m_RestockThread.ThreadState != ThreadState.Stopped)
				return true;
			else
				return false;
		}

		public static void ChangeList(string nomelista)
		{
			if (!Assistant.Engine.MainWindow.RestockListSelect.Items.Contains(nomelista))
			{
				Scripts.SendMessageScriptError("Script Error: Restock.ChangeList: Restock list: " + nomelista + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.RestockStop.Enabled == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.RestockStop.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockStop.PerformClick()));
					Assistant.Engine.MainWindow.RestockListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockListSelect.SelectedIndex = Assistant.Engine.MainWindow.RestockListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.RestockExecute.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockExecute.PerformClick()));
				}
				else
				{
					Assistant.Engine.MainWindow.RestockListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockListSelect.SelectedIndex = Assistant.Engine.MainWindow.RestockListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}
	}
}