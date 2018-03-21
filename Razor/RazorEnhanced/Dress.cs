using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class Dress
	{
		[Serializable]
		internal class DressItem // Vecchia struttura da rimuovere fra qualche versione. rimasta per permetere conversione settings
		{
			private int m_Layer;
			internal int Layer { get { return m_Layer; } }

			private string m_Name;
			internal string Name { get { return m_Name; } }

			private int m_serial;
			internal int Serial { get { return m_serial; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public DressItem(string name, int layer, int serial, bool selected)
			{
				m_Name = name;
				m_Layer = layer;
				m_serial = serial;
				m_Selected = selected;
			}
		}

		[Serializable]
		internal class DressItemNew
		{
			private Layer m_Layer;
			internal Layer Layer { get { return m_Layer; } }

			private string m_Name;
			internal string Name { get { return m_Name; } }

			private int m_serial;
			internal int Serial { get { return m_serial; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public DressItemNew(string name, Layer layer, int serial, bool selected)
			{
				m_Name = name;
				m_Layer = layer;
				m_serial = serial;
				m_Selected = selected;
			}
		}

		internal class DressList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Delay;
			internal int Delay { get { return m_Delay; } }

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			private bool m_Conflict;
			internal bool Conflict { get { return m_Conflict; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public DressList(string description, int delay, int bag, bool conflict, bool selected)
			{
				m_Description = description;
				m_Delay = delay;
				m_Bag = bag;
				m_Conflict = conflict;
				m_Selected = selected;
			}
		}

		internal static void AddLog(string addlog)
		{
			if (!Engine.Running)
				return;

			Engine.MainWindow.DressLogBox.Invoke(new Action(() => Engine.MainWindow.DressLogBox.Items.Add(addlog)));
			Engine.MainWindow.DressLogBox.Invoke(new Action(() => Engine.MainWindow.DressLogBox.SelectedIndex = Engine.MainWindow.DressLogBox.Items.Count - 1));
			if (Assistant.Engine.MainWindow.DressLogBox.Items.Count > 300)
				Assistant.Engine.MainWindow.DressLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.DressLogBox.Items.Clear()));
		}

		private static int m_dressdelay;
		internal static int DressDelay
		{
			get { return m_dressdelay; }

			set
			{
				m_dressdelay = value;
				Assistant.Engine.MainWindow.DressDragDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.DressDragDelay.Text = value.ToString()));
			}
		}

		private static int m_dressbag;
		internal static int DressBag
		{
			get { return m_dressbag; }

			set
			{
				m_dressbag = value;
				Assistant.Engine.MainWindow.DressBagLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.DressBagLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		private static bool m_dressconflict;
		internal static bool DressConflict
		{
			get { return m_dressconflict; }

			set
			{
				m_dressconflict = value;
				Assistant.Engine.MainWindow.DressCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.DressCheckBox.Checked = value));
			}
		}

		private static string m_dresslistname;
		internal static string DressListName
		{
			get { return m_dresslistname; }
			set { m_dresslistname = value; }
		}

		internal static void RefreshLists()
		{
			RazorEnhanced.Settings.Dress.ListsRead(out List<DressList> lists);

			if (lists.Count == 0)
				Assistant.Engine.MainWindow.DressListView.Items.Clear();

			DressList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.DressListSelect.Text)
				return;

			Assistant.Engine.MainWindow.DressListSelect.Items.Clear();
			foreach (DressList l in lists)
			{
				Assistant.Engine.MainWindow.DressListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.DressListSelect.SelectedIndex = Assistant.Engine.MainWindow.DressListSelect.Items.IndexOf(l.Description);
					DressDelay = l.Delay;
					DressBag = l.Bag;
					DressConflict = l.Conflict;
				}
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.Dress.ListInsert(newList, RazorEnhanced.Dress.DressDelay, (int)0, false);
			RazorEnhanced.Dress.RefreshLists();
			RazorEnhanced.Dress.RefreshItems();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.Dress.ListExists(list))
			{
				RazorEnhanced.Settings.Dress.ListDelete(list);
			}

			RazorEnhanced.Dress.RefreshLists();
			RazorEnhanced.Dress.RefreshItems();
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<DressItemNew> items = RazorEnhanced.Settings.Dress.ItemsRead(DressListName);

			if (items.Count != Assistant.Engine.MainWindow.DressListView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.DressListView.Items[i];
			DressItemNew old = items[i];

			if (lvi != null && old != null)
			{
				DressItemNew item = new Dress.DressItemNew(old.Name, old.Layer, old.Serial, lvi.Checked);
				RazorEnhanced.Settings.Dress.ItemReplace(RazorEnhanced.Dress.DressListName, i, item);
			}
		}

		internal static void RefreshItems()
		{
			RazorEnhanced.Settings.Dress.ListsRead(out List<DressList> lists);

			Assistant.Engine.MainWindow.DressListView.Items.Clear();
			foreach (DressList l in lists)
			{
				if (!l.Selected)
					continue;

				List<Dress.DressItemNew> items = RazorEnhanced.Settings.Dress.ItemsRead(l.Description);

				foreach (DressItemNew item in items)
				{
					ListViewItem listitem = new ListViewItem
					{
						Checked = item.Selected
					};
					listitem.SubItems.Add(item.Layer.ToString());
					if (item.Name != "UNDRESS")
					{
						listitem.SubItems.Add(item.Name);
						listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
					}
					else
					{
						listitem.SubItems.Add("UNDRESS");
						listitem.SubItems.Add("UNDRESS");
					}
					Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
				}
			}
		}

		internal static void ReadPlayerDress()
		{
			RazorEnhanced.Settings.Dress.ItemClear(Assistant.Engine.MainWindow.DressListSelect.Text);

			foreach (Layer l in LayerList)
			{
				Assistant.Item layeritem = Assistant.World.Player.GetItemOnLayer(l);
				if (layeritem == null) // slot vuoto
					continue;

				RazorEnhanced.Dress.DressItemNew itemtoinsert = new DressItemNew(layeritem.Name, l, layeritem.Serial, true);
				RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
			}
		
			RazorEnhanced.Dress.RefreshItems();
		}

		internal static void AddItemByTarger(Assistant.Item dressItem)
		{
			if (dressItem.Layer != Layer.Invalid)
			{
				RazorEnhanced.Dress.DressItemNew toinsert = new RazorEnhanced.Dress.DressItemNew(dressItem.Name, dressItem.Layer, dressItem.Serial, true);
				RazorEnhanced.Settings.Dress.ItemInsertByLayer(Assistant.Engine.MainWindow.DressListSelect.Text, toinsert);
				RazorEnhanced.Dress.RefreshItems();
			}
			else
				Misc.SendMessage("This item not have valid layer", false);
		}

		// Undress

		internal static int UndressEngine(int mseconds, int undressbagserial)
		{
			List<int> itemtoundress = new List<int>();
			List<ushort> layertoundress = new List<ushort>();

			foreach (Layer l in LayerList)
			{
				Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(l);
				if (itemtomove != null && itemtomove.Movable)
				{
					layertoundress.Add((ushort)l);
					itemtoundress.Add(itemtomove.Serial);
				}
			}

			if (itemtoundress.Count > 0 )
			{
				if (RazorEnhanced.Settings.General.ReadBool("UO3dEquipUnEquip"))
				{
					RazorEnhanced.Dress.AddLog("UnDressing...");
					ClientCommunication.SendToServerWait(new UnEquipItemMacro(layertoundress));
				}
				else
				{
					foreach (int serial in itemtoundress)
					{
						Dress.AddLog("Item 0x" + serial.ToString("X8") + " undressed!");
						Items.Move(serial, undressbagserial, 0);
						Thread.Sleep(mseconds);
					}
				}

			}
			Dress.AddLog("Finish!");
			if (Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
				Misc.SendMessage("Enhanced UnDress: Finish!", false);
			Engine.MainWindow.UndressFinishWork();
			return 0;
		}

		// Layer List
		internal static List<Layer> LayerList = new List<Layer>
		{
			Layer.RightHand,
			Layer.LeftHand,
			Layer.Shoes,
			Layer.Pants,
			Layer.Shirt,
			Layer.Head,
			Layer.Gloves,
			Layer.Ring,
			Layer.Neck,
			Layer.Waist,
			Layer.InnerTorso,
			Layer.Bracelet,
			Layer.MiddleTorso,
			Layer.Earrings,
			Layer.Arms,
			Layer.Cloak,
			Layer.OuterTorso,
			Layer.OuterLegs,
			Layer.InnerLegs,
			Layer.Talisman
		};

		internal static void UndressEngine()
		{
			// Check bag
			Assistant.Item bag = Assistant.World.FindItem(m_dressbag);
			if (bag == null)
			{
				if (Assistant.Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
					Misc.SendMessage("Dress: Invalid Bag, Switch to backpack", true);
				AddLog("Invalid Bag, Switch to backpack");
				DressBag = (int)World.Player.Backpack.Serial.Value;
			}

			int exit = UndressEngine(m_dressdelay, m_dressbag);
		}

		private static Thread m_UndressThread;

		internal static void UndressStart()
		{
			if (m_UndressThread == null ||
						(m_UndressThread != null && m_UndressThread.ThreadState != ThreadState.Running &&
						m_UndressThread.ThreadState != ThreadState.Unstarted &&
						m_UndressThread.ThreadState != ThreadState.WaitSleepJoin)
					)
			{
				m_UndressThread = new Thread(Dress.UndressEngine);
				m_UndressThread.Start();
			}
		}

		// Dress

		internal static int DressEngine(List<Dress.DressItemNew> items, int mseconds, int undressbagserial, bool conflict)
		{
			if (RazorEnhanced.Settings.General.ReadBool("UO3dEquipUnEquip"))
			{
				List<uint> itemserial = new List<uint>();

				foreach (DressItemNew oggettolista in items)
					itemserial.Add((uint)oggettolista.Serial);

				if (itemserial.Count > 0)
				{
					RazorEnhanced.Dress.AddLog("Dressing...");
					ClientCommunication.SendToServerWait(new EquipItemMacro(itemserial));
				}
			}
			else
			{
				foreach (DressItemNew oggettolista in items)
				{
					if (!oggettolista.Selected)
						continue;

					if (oggettolista.Name == "UNDRESS")          // Caso undress slot
					{
						Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(oggettolista.Layer);

						if (itemtomove == null)
							continue;

						if (!itemtomove.Movable)
							continue;

						RazorEnhanced.Dress.AddLog("Item 0x" + itemtomove.Serial.Value.ToString("X8") + " on Layer: " + oggettolista.Layer.ToString() + " undressed!");
						RazorEnhanced.Items.Move(itemtomove.Serial, undressbagserial, 0);
						Thread.Sleep(mseconds);
					}
					else
					{
						if (World.FindItem(oggettolista.Serial) == null)
							continue;

						if (conflict)       // Caso abilitato controllo conflitto
						{
							Assistant.Item itemonlayer = Assistant.World.Player.GetItemOnLayer(World.FindItem(oggettolista.Serial).Layer);
							if (itemonlayer != null)
								if (itemonlayer.Serial == oggettolista.Serial)
									continue;

							if (World.FindItem(oggettolista.Serial).Layer == Layer.RightHand || World.FindItem(oggettolista.Serial).Layer == Layer.LeftHand)        // Check armi per controlli twohand
							{
								Assistant.Item lefth = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
								Assistant.Item righth = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);

								if (Assistant.World.FindItem(oggettolista.Serial).IsTwoHanded)
								{
									if (lefth != null && lefth.Movable)
									{
										RazorEnhanced.Dress.AddLog("Item 0x" + lefth.Serial.Value.ToString("X8") + " on Layer: LeftHand undressed!");
										RazorEnhanced.Items.Move(lefth.Serial, undressbagserial, 0);
										Thread.Sleep(mseconds);
									}
									if (righth != null && righth.Movable)
									{
										RazorEnhanced.Dress.AddLog("Item 0x" + righth.Serial.Value.ToString("X8") + " on Layer: RightHand undressed!");
										RazorEnhanced.Items.Move(righth.Serial, undressbagserial, 0);
										Thread.Sleep(mseconds);
									}
								}
								else if ((lefth != null && lefth.IsTwoHanded) || (righth != null && righth.IsTwoHanded))
								{
									if (lefth != null && lefth.Movable)
									{
										RazorEnhanced.Dress.AddLog("Item 0x" + lefth.Serial.Value.ToString("X8") + " on Layer: LeftHand undressed!");
										RazorEnhanced.Items.Move(lefth.Serial, undressbagserial, 0);
										Thread.Sleep(mseconds);
									}
									if (righth != null && righth.Movable)
									{
										RazorEnhanced.Dress.AddLog("Item 0x" + righth.Serial.Value.ToString("X8") + " on Layer: RightHand undressed!");
										RazorEnhanced.Items.Move(righth.Serial, undressbagserial, 0);
										Thread.Sleep(mseconds);
									}
								}
							}

							Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(oggettolista.Layer);
							if (itemtomove != null)
							{
								if (itemtomove.Serial == oggettolista.Serial)
									continue;

								if (!itemtomove.Movable)
									continue;

								RazorEnhanced.Dress.AddLog("Item 0x" + itemtomove.Serial.Value.ToString("X8") + " on Layer: " + oggettolista.Layer.ToString() + " undressed!");
								RazorEnhanced.Items.Move(itemtomove.Serial, undressbagserial, 0);
								Thread.Sleep(mseconds);
								RazorEnhanced.Player.EquipItem(oggettolista.Serial);
								RazorEnhanced.Dress.AddLog("Item 0x" + oggettolista.Serial.ToString("X8") + " Equipped on layer: " + oggettolista.Layer.ToString());
								Thread.Sleep(mseconds);
							}
							else
							{
								RazorEnhanced.Player.EquipItem(oggettolista.Serial);
								RazorEnhanced.Dress.AddLog("Item 0x" + oggettolista.Serial.ToString("X8") + " Equipped on layer: " + oggettolista.Layer.ToString());
								Thread.Sleep(mseconds);
							}
						}
						else
						{
							Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(oggettolista.Layer);
							if (itemtomove != null)
								continue;

							RazorEnhanced.Dress.AddLog("Item 0x" + oggettolista.Serial.ToString("X8") + " Equipped on layer: " + oggettolista.Layer.ToString());
							RazorEnhanced.Player.EquipItem(oggettolista.Serial);
							Thread.Sleep(mseconds);
						}
					}
				}
			}
			RazorEnhanced.Dress.AddLog("Finish!");
			if (Assistant.Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
				RazorEnhanced.Misc.SendMessage("Enhanced Dress: Finish!", 945, true);
			Assistant.Engine.MainWindow.UndressFinishWork();
			return 0;
		}

		internal static void DressEngine()
		{
			List<Dress.DressItemNew> items = Settings.Dress.ItemsRead(Dress.DressListName);

			// Check bag
			Assistant.Item bag = Assistant.World.FindItem(m_dressbag);
			if (bag == null)
			{
				if (Assistant.Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
					Misc.SendMessage("Dress: Invalid Bag, Switch to backpack", 945, true);
				AddLog("Invalid Bag, Switch to backpack");
				DressBag = (int)World.Player.Backpack.Serial.Value;
			}

			int exit = DressEngine(items, m_dressdelay, m_dressbag, m_dressconflict);
		}

		private static Thread m_DressThread;

		internal static void DressStart()
		{
			if (m_DressThread == null ||
						(m_DressThread != null && m_DressThread.ThreadState != ThreadState.Running &&
						m_DressThread.ThreadState != ThreadState.Unstarted &&
						m_DressThread.ThreadState != ThreadState.WaitSleepJoin)
					)
			{
				m_DressThread = new Thread(Dress.DressEngine);
				m_DressThread.Start();
			}
		}

		internal static void ForceStop()
		{
			if (m_DressThread != null && m_DressThread.ThreadState != ThreadState.Stopped)
			{
				m_DressThread.Abort();
			}
			if (m_UndressThread != null && m_UndressThread.ThreadState != ThreadState.Stopped)
			{
				m_UndressThread.Abort();
			}
		}

		// Funzioni da script

		public static bool DressStatus()
		{
			if (m_DressThread != null && m_DressThread.ThreadState != ThreadState.Stopped)
				return true;
			else
				return false;
		}

		public static bool UnDressStatus()
		{
			if (m_UndressThread != null && m_UndressThread.ThreadState != ThreadState.Stopped)
				return true;
			else
				return false;
		}

		public static void DressFStart()
		{
			if (Assistant.Engine.MainWindow.DressExecuteButton.Enabled == true)
				Assistant.Engine.MainWindow.DressStart();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Dress.DressFStart: Dress already running");
			}
		}

		public static void UnDressFStart()
		{
			if (Assistant.Engine.MainWindow.UnDressExecuteButton.Enabled == true)
				Assistant.Engine.MainWindow.UndressStart();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Dress.UnDressFStart: Undress already running");
			}
		}

		public static void DressFStop()
		{
			if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
				Assistant.Engine.MainWindow.DressStop();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Dress.DressFStop: Dress not running");
			}
		}

		public static void UnDressFStop()
		{
			if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
				Assistant.Engine.MainWindow.DressStop();
			else
			{
				Scripts.SendMessageScriptError("Script Error: Dress.DressFStop: UnDress not running");
			}
		}

		public static void ChangeList(string nomelista)
		{
			if (!UpdateListParam(nomelista))
			{
				Scripts.SendMessageScriptError("Script Error: Dress.ChangeList: Scavenger list: " + nomelista + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.DressStopButton.Invoke(new Action(() => Assistant.Engine.MainWindow.DressStopButton.PerformClick()));
					Assistant.Engine.MainWindow.DressListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.DressListSelect.SelectedIndex = Assistant.Engine.MainWindow.DressListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.DressExecuteButton.Invoke(new Action(() => Assistant.Engine.MainWindow.DressExecuteButton.PerformClick()));
				}
				else
				{
					Assistant.Engine.MainWindow.DressListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.DressListSelect.SelectedIndex = Assistant.Engine.MainWindow.DressListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}

		internal static bool UpdateListParam(string nomelista)
		{
			if (Settings.Dress.ListExists(nomelista))
			{
				Settings.Dress.ListDetailsRead(nomelista, out int bag, out int delay, out bool conflict);
				DressListName = nomelista;
				DressBag = bag;
				DressDelay = delay;
				DressConflict = conflict;
				return true;
			}
			return false;
		}
	}
}