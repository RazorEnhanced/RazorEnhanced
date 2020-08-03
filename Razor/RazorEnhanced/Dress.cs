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
	public class Dress
	{

		internal class DressItemNew	 : ListAbleItem
		{
			[JsonProperty("Layer")]
			internal Layer Layer { get; set;}

			[JsonProperty("Name")]
			internal string Name { get; set;}

			[JsonProperty("Serial")]
			internal int Serial { get; set; }

			[JsonProperty("Selected")]
			internal bool Selected { get; set;}

			public DressItemNew(string name, Layer layer, int serial, bool selected)
			{
				Name = name;
				Layer = layer;
				Serial = serial;
				Selected = selected;
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
			[JsonProperty("Selected")]
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
			if (!Client.Running)
				return;

			Engine.MainWindow.SafeAction(s => s.DressLogBox.Items.Add(addlog));
			Engine.MainWindow.SafeAction(s => s.DressLogBox.SelectedIndex = s.DressLogBox.Items.Count - 1);
			if (Assistant.Engine.MainWindow.DressLogBox.Items.Count > 300)
				Engine.MainWindow.SafeAction(s => s.DressLogBox.Items.Clear());
		}

		private static int
			m_dressdelay;
		internal static int DressDelay
		{
			get { return m_dressdelay; }

			set
			{
				m_dressdelay = value;
				Engine.MainWindow.SafeAction(s => s.DressDragDelay.Text = value.ToString());
			}
		}

		private static int m_dressbag;
		internal static int DressBag
		{
			get { return m_dressbag; }

			set
			{
				m_dressbag = value;
				Engine.MainWindow.SafeAction(s => s.DressBagLabel.Text = "0x" + value.ToString("X8"));

			}
		}

		private static bool m_dressconflict;
		internal static bool DressConflict
		{
			get { return m_dressconflict; }

			set
			{
				m_dressconflict = value;
				Engine.MainWindow.SafeAction(s => s.DressCheckBox.Checked = value);
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
			List<DressList> lists = Settings.Dress.ListsRead();

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
			List<DressList> lists = Settings.Dress.ListsRead();
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
			if (World.Player == null) // non loggato
			{
				AddLog("You are not logged in game!");
				return;
			}

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

		internal static int UndressEngine(List<Dress.DressItemNew> items, int mseconds, int undressbagserial)
		{
			try
			{
				if (RazorEnhanced.Settings.General.ReadBool("UO3dEquipUnEquip"))
				{
					List<ushort> layertoundress = new List<ushort>();
					foreach (Dress.DressItemNew item in items)
					{
						//Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(l);
						layertoundress.Add((ushort)item.Layer);
					}
					RazorEnhanced.Dress.AddLog("UnDressing...");
					Assistant.Client.Instance.SendToServerWait(new UnEquipItemMacro(layertoundress));
				}
				else
				{
					foreach (DressItemNew item in items)
					{
						if (!item.Selected)
							continue;

						if (World.FindItem(item.Serial) == null)
							continue;

						Assistant.Item itemonlayer = Assistant.World.Player.GetItemOnLayer(World.FindItem(item.Serial).Layer);
						if (itemonlayer != null && itemonlayer.Serial == item.Serial)
							RazorEnhanced.Items.Move(item.Serial, undressbagserial, 0);
						RazorEnhanced.Dress.AddLog("Item 0x" + item.Serial.ToString("X8") + " on layer: " + item.Layer.ToString() + " undressed!");
						Thread.Sleep(mseconds);
					}
				}
				RazorEnhanced.Dress.AddLog("Finish!");
				if (Assistant.Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Enhanced Dress: Finish!", 945, true);
				Assistant.Engine.MainWindow.UndressFinishWork();

			}
			catch { }
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
			try
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

				UndressEngine(items, m_dressdelay, m_dressbag);
			}
			catch { }
		}

		private static Thread m_UndressThread;

		internal static void UndressStart()
		{
			// We have an existing thread that we need to examine and determine disposition
			// Cannot do delays or WaitSleepJoin because it will lock up UI
			if (m_UndressThread != null)
			{
				// We can only allow 1 thread to run at a time otherwise it will cause conflicts.
				switch (m_UndressThread.ThreadState)
				{
					case ThreadState.Aborted:
					case ThreadState.Unstarted:
					case ThreadState.Stopped:

					// Calculated risk here; usually unsafe and can leak memory, but we will accept risk here
					case ThreadState.AbortRequested:
					case ThreadState.StopRequested:
						m_UndressThread = null;
						break;

					default:
						// If thread is running or in WaitSleepJoin, make user wait and try again
						return;
				}
			}

			if (m_UndressThread == null)
			{
				try
				{
					m_UndressThread = new Thread(Dress.UndressEngine);
					m_UndressThread.Start();
				}
				catch { }
			}
		}

		// Dress

		internal static void DressEngine(List<Dress.DressItemNew> items, int mseconds, int undressbagserial, bool conflict)
		{
			try
			{
				if (RazorEnhanced.Settings.General.ReadBool("UO3dEquipUnEquip"))
				{
                    // Problem with uo3d is the serveuo servers don't swap 1hand/2hand properly
                    // but OSI does, so if delay is 0 let OSI swap fast otherwise handle udress for weapons
                    if (m_dressdelay == 0)
                    {
                        List<uint> itemserial = new List<uint>();
                        Assistant.Item lefth = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
                        Assistant.Item righth = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);

                        foreach (DressItemNew item in items)
                        {
                            var existingItem = Assistant.World.Player.GetItemOnLayer(item.Layer);
                            if (existingItem == null || item.Serial != existingItem.Serial)
                            {
                                itemserial.Add((uint)item.Serial);
                            }
                        }
                        if (itemserial.Count > 0)
                        {
                            RazorEnhanced.Dress.AddLog("Dressing...");
                            Assistant.Client.Instance.SendToServerWait(new EquipItemMacro(itemserial));
                        }
                    }
                    else
                    {
                        // This logic is terrible .. The plan is
                        // If your swapping weapons, and new weapon 1hand/2hand not equal to new 1/hand/2hand
                        // then undress the existing weapons first
                        List<uint> itemserial = new List<uint>();
                        Assistant.Item lefth = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
                        Assistant.Item righth = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);

                        bool dropWeaponL = false;
                        bool twoHandLeft = false;
                        bool dropWeaponR = false;
                        //Assistant.Item newLeft = null;
                        foreach (DressItemNew item in items)
                        {
                            var existingItem = Assistant.World.Player.GetItemOnLayer(item.Layer);
                            if (existingItem == null || item.Serial != existingItem.Serial)
                            {
                                itemserial.Add((uint)item.Serial);
                            }
                            if (item.Layer == Layer.LeftHand)
                            {
                                if (lefth == null || item.Serial != lefth.Serial)
                                {
                                    twoHandLeft = Assistant.World.FindItem(item.Serial).IsTwoHanded;
                                }
                            }

                            if (item.Layer == Layer.LeftHand && lefth != null && item.Serial != lefth.Serial)
                            {
                                dropWeaponL = true;
                            }
                            if (item.Layer == Layer.RightHand && righth != null && item.Serial != righth.Serial)
                            {
                                dropWeaponR = true;
                            }
                        }

                        List<ushort> dropLayer = new List<ushort>();
                        if (dropWeaponL || twoHandLeft)
                        {
                            dropLayer.Add((ushort)Layer.LeftHand);
                        }
                        if (dropWeaponR || twoHandLeft)
                        {
                            dropLayer.Add((ushort)Layer.RightHand);
                        }
                        if (dropLayer.Count > 0)
                        {
                            Assistant.Client.Instance.SendToServerWait(new UnEquipItemMacro(dropLayer));
                            Thread.Sleep(m_dressdelay);
                        }

                        if (itemserial.Count > 0)
                        {
                            RazorEnhanced.Dress.AddLog("Dressing...");
                            Assistant.Client.Instance.SendToServerWait(new EquipItemMacro(itemserial));
                        }
                    }
				}
				else
				{
					foreach (DressItemNew item in items)
					{
						if (!item.Selected)
							continue;

						if (item.Name == "UNDRESS")          // Caso undress slot
						{
							Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(item.Layer);

							if (itemtomove == null)
								continue;

							if (!itemtomove.Movable)
								continue;

							RazorEnhanced.Dress.AddLog("Item 0x" + itemtomove.Serial.Value.ToString("X8") + " on Layer: " + item.Layer.ToString() + " undressed!");
							RazorEnhanced.Items.Move(itemtomove.Serial, undressbagserial, 0);
							Thread.Sleep(mseconds);
						}
						else
						{
							if (World.FindItem(item.Serial) == null)
								continue;

							if (conflict)       // Caso abilitato controllo conflitto
							{
								Assistant.Item itemonlayer = Assistant.World.Player.GetItemOnLayer(World.FindItem(item.Serial).Layer);
								if (itemonlayer != null)
									if (itemonlayer.Serial == item.Serial)
										continue;

								if (World.FindItem(item.Serial).Layer == Layer.RightHand || World.FindItem(item.Serial).Layer == Layer.LeftHand)        // Check armi per controlli twohand
								{
									Assistant.Item lefth = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
									Assistant.Item righth = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);

									if (Assistant.World.FindItem(item.Serial).IsTwoHanded)
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

								Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(item.Layer);
								if (itemtomove != null)
								{
									if (itemtomove.Serial == item.Serial)
										continue;

									if (!itemtomove.Movable)
										continue;

									RazorEnhanced.Dress.AddLog("Item 0x" + itemtomove.Serial.Value.ToString("X8") + " on Layer: " + item.Layer.ToString() + " undressed!");
									RazorEnhanced.Items.Move(itemtomove.Serial, undressbagserial, 0);
									Thread.Sleep(mseconds);
									RazorEnhanced.Player.EquipItem(item.Serial);
									RazorEnhanced.Dress.AddLog("Item 0x" + item.Serial.ToString("X8") + " Equipped on layer: " + item.Layer.ToString());
									Thread.Sleep(mseconds);
								}
								else
								{
									RazorEnhanced.Player.EquipItem(item.Serial);
									RazorEnhanced.Dress.AddLog("Item 0x" + item.Serial.ToString("X8") + " Equipped on layer: " + item.Layer.ToString());
									Thread.Sleep(mseconds);
								}
							}
							else
							{
								Assistant.Item itemtomove = Assistant.World.Player.GetItemOnLayer(item.Layer);
								if (itemtomove != null)
									continue;

								RazorEnhanced.Dress.AddLog("Item 0x" + item.Serial.ToString("X8") + " Equipped on layer: " + item.Layer.ToString());
								RazorEnhanced.Player.EquipItem(item.Serial);
								Thread.Sleep(mseconds);
							}
						}
					}
				}
				RazorEnhanced.Dress.AddLog("Finish!");
				if (Assistant.Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Enhanced Dress: Finish!", 945, true);
				Assistant.Engine.MainWindow.UndressFinishWork();
			}

			catch { }
		}

		internal static void DressEngine()
		{
			try
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

				DressEngine(items, m_dressdelay, m_dressbag, m_dressconflict);
			}
			catch { }
		}

		private static Thread m_DressThread;

		internal static void DressStart()
		{
			// We have an existing thread that we need to examine and determine disposition
			// Cannot do delays or WaitSleepJoin because it will lock up UI
			if (m_DressThread != null)
			{
				// We can only allow 1 thread to run at a time otherwise it will cause conflicts.
				switch (m_DressThread.ThreadState)
				{
					case ThreadState.Aborted:
					case ThreadState.Unstarted:
					case ThreadState.Stopped:

					// Calculated risk here; usually unsafe and can leak memory, but we will accept risk here
					case ThreadState.AbortRequested:
					case ThreadState.StopRequested:
						m_DressThread = null;
						break;

					default:
						// If thread is running or in WaitSleepJoin, make user wait and try again
						return;
				}
			}
			if (m_DressThread == null)
			{
				m_DressThread = new Thread(Dress.DressEngine);
				m_DressThread.Start();
			}
		}

		internal static void ForceStop()
		{
			if (m_DressThread != null)
			{
				try
				{
					m_DressThread.Abort();
				}
				catch
				{ }
			}
			if (m_UndressThread != null)
			{
				try
				{
					m_UndressThread.Abort();
				}
				catch
				{ }
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

		public static void ChangeList(string listName)
		{
			if (!UpdateListParam(listName))
			{
				Scripts.SendMessageScriptError("Script Error: Dress.ChangeList: Scavenger list: " + listName + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true) // Se è in esecuzione forza stop change list e restart
				{
					Engine.MainWindow.SafeAction(s => s.DressStopButton.PerformClick());
					Engine.MainWindow.SafeAction(s => s.DressListSelect.SelectedIndex = s.DressListSelect.Items.IndexOf(listName));  // change list
					Engine.MainWindow.SafeAction(s => s.DressExecuteButton.PerformClick());
				}
				else
				{
					Engine.MainWindow.SafeAction(s => s.DressListSelect.SelectedIndex = s.DressListSelect.Items.IndexOf(listName));  // change list
				}
			}
		}

		internal static bool UpdateListParam(string listName)
		{
			if (Settings.Dress.ListExists(listName))
			{
				Settings.Dress.ListDetailsRead(listName, out int bag, out int delay, out bool conflict);
				DressListName = listName;
				DressBag = bag;
				DressDelay = delay;
				DressConflict = conflict;
				return true;
			}
			return false;
		}
	}
}
