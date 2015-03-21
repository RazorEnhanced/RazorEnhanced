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

			public OrganizerItem(string name, int graphics, int color, int amount)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_amount = amount;
			}
		}

		internal static int ItemDragDelay
		{
			get
			{
				return Assistant.Engine.MainWindow.OrganizerDragDelay;
			}
		}
		internal static void AddLog(string addlog)
		{
			Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.SelectedIndex = Assistant.Engine.MainWindow.OrganizerLogBox.Items.Count - 1));
		}
		internal static void RefreshList(List<OrganizerItem> organizerItemList)
		{
			Assistant.Engine.MainWindow.OrganizerListView.Items.Clear();
			foreach (OrganizerItem item in organizerItemList)
			{
				ListViewItem listitem = new ListViewItem();
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
		internal static void ModifyItemToList(string name, int graphics, int color, int amount, ListView oranizerListView, List<OrganizerItem> organizerItemList, int indexToInsert)
		{
			organizerItemList.RemoveAt(indexToInsert);                                                       // rimuove
			organizerItemList.Insert(indexToInsert, new OrganizerItem(name, graphics, color, amount));     // inserisce al posto di prima
			RazorEnhanced.Settings.SaveOrganizerItemList(Assistant.Engine.MainWindow.OrganizerListSelect.SelectedItem.ToString(), organizerItemList, Assistant.Engine.MainWindow.OrganizerSourceBag.Value, Assistant.Engine.MainWindow.OrganizerDestinationBag.Value);
			RazorEnhanced.Organizer.RefreshList(organizerItemList);
		}
		internal static void AddItemToList(string name, int graphics, int color, int amount, ListView organizerlistView, List<OrganizerItem> organizerItemList)
		{
			organizerItemList.Add(new OrganizerItem(name, graphics, color, amount));
			RazorEnhanced.Settings.SaveOrganizerItemList(Assistant.Engine.MainWindow.OrganizerListSelect.SelectedItem.ToString(), organizerItemList, Assistant.Engine.MainWindow.OrganizerSourceBag.Value, Assistant.Engine.MainWindow.OrganizerDestinationBag.Value);
			RazorEnhanced.Organizer.RefreshList(organizerItemList);
		}

		internal static int Engine(List<OrganizerItem> organizerItemList, int mseconds, Item SourceBag, Item destinationBag)
		{
			// Apre le bag per item contenuti
			Items.UseItem(SourceBag);
			Items.WaitForContents(SourceBag, 1500);
			Items.UseItem(destinationBag);
			Items.WaitForContents(destinationBag, 1500);

			// Inizia scansione 
			foreach (RazorEnhanced.Item oggettoContenuto in SourceBag.Contains)
			{
				foreach (OrganizerItem oggettoDaLista in organizerItemList)
				{
					if (oggettoContenuto.ItemID == oggettoDaLista.Graphics && oggettoContenuto.Hue == oggettoDaLista.Color)     // Verifico il match fra colore e grafica
					{
						// Controllo amount e caso -1
						if (oggettoDaLista.Amount == -1) // Sposta senza contare
						{
							RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Found Move amount: " + oggettoContenuto.Amount);
							RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, 0);
							Thread.Sleep(mseconds);
						}
						else   // Caso con limite quantita'
						{
							int AmountContenuto = Items.ContainerCount(destinationBag, oggettoDaLista.Graphics, oggettoDaLista.Color);      // Calcolo oggetti gia' presenti nel container di destinazione
							if (AmountContenuto < oggettoDaLista.Amount)          // Controlla la differenza se mancano item al totale amount procede
							{
								if ((AmountContenuto - oggettoDaLista.Amount) <= oggettoContenuto.Amount)     // Caso che lo stack da spostare sia minore del limite di oggetti 
								{
									RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Found Move amount: " + oggettoContenuto.Amount);
									RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, 0);
									RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Destination bag amount: " + (AmountContenuto + oggettoContenuto.Amount));
									Thread.Sleep(mseconds);
								}
								else  // Caso che lo stack sia superiore (sposta solo un blocco)
								{
									RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Found Move amount: " + (oggettoContenuto.Amount - AmountContenuto));
									RazorEnhanced.Items.Move(oggettoContenuto, destinationBag, (oggettoContenuto.Amount - AmountContenuto));
									RazorEnhanced.Organizer.AddLog("- Item (0x" + oggettoContenuto.ItemID.ToString("X4") + ") Destination bag amount: " + (AmountContenuto + (oggettoContenuto.Amount - AmountContenuto)));
									Thread.Sleep(mseconds);
								}
							}
						}

					}
				}
			}
			return 0;
		}

		internal static void Engine()
		{
			Assistant.Item sourceBag = Assistant.World.FindItem((Assistant.Serial)Assistant.Engine.MainWindow.OrganizerSourceBag);
			Assistant.Item destinationBag = Assistant.World.FindItem((Assistant.Serial)Assistant.Engine.MainWindow.OrganizerDestinationBag);

			if (sourceBag == null || destinationBag == null)
				return;

			RazorEnhanced.Item razorSource = new RazorEnhanced.Item(sourceBag);
			RazorEnhanced.Item razorDestination = new RazorEnhanced.Item(destinationBag);

			int exit = Engine(Assistant.Engine.MainWindow.OrganizerItemList, Assistant.Engine.MainWindow.OrganizerDragDelay, razorSource, razorDestination);
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

	}
}
