using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class SellAgent
	{
		[Serializable]
		public class SellAgentItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_amount;
			public int Amount { get { return m_amount; } }

			private int m_color;
			public int Color { get { return m_color; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public SellAgentItem(string name, int graphics, int amount, int color, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_amount = amount;
				m_color = color;
				m_Selected = selected;
			}
		}

		internal class SellAgentList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public SellAgentList(string description, int bag, bool selected)
			{
				m_Description = description;
				m_Bag = bag;
				m_Selected = selected;
			}
		}

		internal static string SellListName
		{
			get
			{
				return (string)Assistant.Engine.MainWindow.SellListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.SellListSelect.Text));
			}

			set
			{
				Assistant.Engine.MainWindow.SellListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.SellListSelect.Text = value));
			}
		}

		internal static int SellBag
		{
			get
			{
				int serialBag = 0;

				try
				{
					serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.SellBagLabel.Text, 16);
				}
				catch
				{ }

				return serialBag;
			}

			set
			{
				Assistant.Engine.MainWindow.SellBagLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.SellBagLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (Engine.Running)
			{
				Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.Items.Add(addlog)));
				Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.SelectedIndex = Assistant.Engine.MainWindow.SellLogBox.Items.Count - 1));
				if (Assistant.Engine.MainWindow.SellLogBox.Items.Count > 300)
					Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.Items.Clear()));
			}
		}

		internal static void RefreshLists()
		{
			List<SellAgentList> lists;
			RazorEnhanced.Settings.SellAgent.ListsRead(out lists);

			if (lists.Count == 0)
				Assistant.Engine.MainWindow.SellListView.Items.Clear();

			SellAgentList selectedList = lists.Where(l => l.Selected).FirstOrDefault();
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.SellListSelect.Text)
				return;

			Assistant.Engine.MainWindow.SellListSelect.Items.Clear();
			foreach (SellAgentList l in lists)
			{
				Assistant.Engine.MainWindow.SellListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.SellListSelect.SelectedIndex = Assistant.Engine.MainWindow.SellListSelect.Items.IndexOf(l.Description);
					SellBag = l.Bag;
				}
			}
		}

		internal static void RefreshItems()
		{
			List<SellAgentList> lists;
			RazorEnhanced.Settings.SellAgent.ListsRead(out lists);

			Assistant.Engine.MainWindow.SellListView.Items.Clear();
			foreach (SellAgentList l in lists)
			{
				if (l.Selected)
				{
					List<SellAgent.SellAgentItem> items;
					RazorEnhanced.Settings.SellAgent.ItemsRead(l.Description, out items);

					foreach (SellAgentItem item in items)
					{
						ListViewItem listitem = new ListViewItem();

						listitem.Checked = item.Selected;

						listitem.SubItems.Add(item.Name);
						listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));

						listitem.SubItems.Add(item.Amount.ToString());

						if (item.Color == -1)
							listitem.SubItems.Add("All");
						else
							listitem.SubItems.Add("0x" + item.Color.ToString("X4"));

						Assistant.Engine.MainWindow.SellListView.Items.Add(listitem);
					}
				}
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<SellAgentItem> items;
			RazorEnhanced.Settings.SellAgent.ItemsRead(SellListName, out items);

			if (items.Count != Assistant.Engine.MainWindow.SellListView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.SellListView.Items[i];
			SellAgentItem old = items[i];

			if (lvi != null && old != null)
			{
				SellAgentItem item = new SellAgent.SellAgentItem(old.Name, old.Graphics, old.Amount, old.Color, lvi.Checked);
				RazorEnhanced.Settings.SellAgent.ItemReplace(RazorEnhanced.SellAgent.SellListName, i, item);
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.SellAgent.ListInsert(newList, 0);

			RazorEnhanced.SellAgent.RefreshLists();
			RazorEnhanced.SellAgent.RefreshItems();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.SellAgent.ListExists(list))
			{
				RazorEnhanced.Settings.SellAgent.ListDelete(list);
			}

			RazorEnhanced.SellAgent.RefreshLists();
			RazorEnhanced.SellAgent.RefreshItems();
		}

		internal static void AddItemToList(string name, int graphics, int amount, int color)
		{
			SellAgentItem item = new SellAgentItem(name, graphics, amount, color, false);

			string selection = Assistant.Engine.MainWindow.SellListSelect.Text;

			if (RazorEnhanced.Settings.SellAgent.ListExists(selection))
			{
				if (!RazorEnhanced.Settings.SellAgent.ItemExists(selection, item))
					RazorEnhanced.Settings.SellAgent.ItemInsert(selection, item);
			}

			RazorEnhanced.SellAgent.RefreshItems();
		}

		internal static void ModifyItemInList(string name, int graphics, int amount, int color, bool selected, SellAgentItem old, int index)
		{
			SellAgentItem item = new SellAgentItem(name, graphics, amount, color, selected);

			string selection = Assistant.Engine.MainWindow.SellListSelect.Text;

			if (RazorEnhanced.Settings.SellAgent.ListExists(selection))
			{
				if (RazorEnhanced.Settings.SellAgent.ItemExists(selection, old))
					RazorEnhanced.Settings.SellAgent.ItemReplace(selection, index, item);
			}

			RazorEnhanced.SellAgent.RefreshItems();
		}

		internal static void EnableSellFilter()
		{
			PacketHandler.RegisterServerToClientViewer(0x9E, new PacketViewerCallback(OnVendorSell));
		}

		private static bool ColorCheck(int colorDaLista, ushort colorDaVendor)
		{
			if (colorDaLista == -1)         // Wildcard colore
				return true;
			else
				if (colorDaLista == colorDaVendor)      // Match OK
				return true;
			else            // Match fallito
				return false;
		}

		private static void OnVendorSell(PacketReader pvSrc, PacketHandlerEventArgs args)
		{
			if (!Assistant.Engine.MainWindow.SellCheckBox.Checked) // Filtro disabilitato
				return;

			Assistant.Item bag = new Assistant.Item(SellBag);
			if (bag == null) // Verifica HotBag
			{
				AddLog("Invalid or not accessible Container");
				return;
			}

			int total = 0;
			uint serial = pvSrc.ReadUInt32();

			Assistant.Mobile vendor = Assistant.World.FindMobile(serial);
			if (vendor == null)
				Assistant.World.AddMobile(vendor = new Assistant.Mobile(serial));

			int count = pvSrc.ReadUInt16();
			int sold = 0;

			List<Assistant.SellListItem> list = new List<Assistant.SellListItem>(count); // Lista item checkati per vendita (non so dove sia dichiarata)
			List<RazorEnhanced.SellAgent.SellAgentItem> templist = new List<RazorEnhanced.SellAgent.SellAgentItem>(); // Lista temporanea per controlli amount

			AddLog("Container: 0x" + SellBag.ToString("X8"));

			for (int i = 0; i < count; i++) // Scansione item in lista menu vendor
			{
				uint ser = pvSrc.ReadUInt32();
				ushort gfx = pvSrc.ReadUInt16();
				ushort hue = pvSrc.ReadUInt16();
				ushort amount = pvSrc.ReadUInt16();
				ushort price = pvSrc.ReadUInt16();
				pvSrc.ReadString(pvSrc.ReadUInt16()); //name

				Assistant.Item item = Assistant.World.FindItem(ser);

				List<SellAgent.SellAgentItem> items;
				RazorEnhanced.Settings.SellAgent.ItemsRead(SellListName, out items);

				foreach (SellAgentItem sellItem in items) // Scansione item presenti in lista agent item
				{
					if (!sellItem.Selected)
						continue;

					if (gfx == sellItem.Graphics && (item != null && item != bag && item.IsChildOf(bag)) && RazorEnhanced.SellAgent.ColorCheck(sellItem.Color, hue)) // match sulla grafica fra lista agent e lista vendor e hotbag
					{
						int amountLeft = 60000;
						int index = 0;
						bool alreadySold = false;

						for (int y = 0; y < templist.Count; y++) // Controllo che non ho gia venduto item simili
						{
							if (templist[y].Graphics == gfx && RazorEnhanced.SellAgent.ColorCheck(templist[y].Color, hue))
							{
								alreadySold = true;
								amountLeft = templist[y].Amount;
								index = y;
							}
						}

						if (amountLeft == 60000) // Valore limite e inizzializzazione
							amountLeft = sellItem.Amount;

						if (amountLeft > 0) // Controlla se mancano da vendere
						{
							if (alreadySold) // Gia venduto oggetto stessa grafica
							{
								AddLog("Item match: 0x" + sellItem.Graphics.ToString("X4") + " - Amount: " + sellItem.Amount);
								if (amount < amountLeft)        // In caso che quella listata nel vendor sia minore di quella che voglio vendere vendo il massimo possibile
								{
									int amountTemp = amountLeft - amount;
									list.Add(new SellListItem(ser, amount));            // Lista processo vendita
									templist.RemoveAt(index);
									templist.Insert(index, new SellAgentItem(sellItem.Name, gfx, amountTemp, sellItem.Color, sellItem.Selected));
									total += amount * price;
									sold += amount;
								}
								else // Caso che quella listata nel vendor sia maggiore vendo solo quella mancante
								{
									list.Add(new SellListItem(ser, Convert.ToUInt16(amountLeft)));  // Lista processo vendita
									templist.RemoveAt(index);
									templist.Insert(index, new SellAgentItem(sellItem.Name, gfx, 0, sellItem.Color, sellItem.Selected));
									total += amountLeft * price;
									sold += amountLeft;
								}
							}
							else // Mai venduto oggetto stessa grafica
							{
								AddLog("Item match: 0x" + sellItem.Graphics.ToString("X4") + " - Amount: " + sellItem.Amount);
								if (amount < sellItem.Amount) // In caso che quella listata nel vendor sia minore di quella che voglio vendere vendo il massimo possibile
								{
									list.Add(new SellListItem(ser, amount));  // Lista processo vendita
									templist.Add(new SellAgentItem(sellItem.Name, gfx, (sellItem.Amount - amount), sellItem.Color, sellItem.Selected));
									total += amount * price;
									sold += amount;
								}
								else // Caso che quella listata nel vendor sia maggiore vendo solo quella mancante
								{
									list.Add(new SellListItem(ser, Convert.ToUInt16(sellItem.Amount)));  // Lista processo vendita
									templist.Add(new SellAgentItem(sellItem.Name, gfx, 0, sellItem.Color, sellItem.Selected));
									total += sellItem.Amount * price;
									sold += sellItem.Amount;
								}
							}
						}
					}
				}
			}

			if (list.Count > 0)
			{
				ClientCommunication.SendToServer(new VendorSellResponse(vendor, list));
				AddLog("Sold " + sold.ToString() + " items for " + total.ToString() + " gold coins");
				World.Player.SendMessage("Enhanced Sell Agent: sold " + sold.ToString() + " items for " + total.ToString() + " gold coins");
				args.Block = true;
			}
		}

		// Funzioni da script
		public static void Enable()
		{
			if (!ClientCommunication.AllowBit(FeatureBit.SellAgent))
			{
				Scripts.SendMessageScriptError("SellAgent Not Allowed!");
				return;
			}

			if (Assistant.Engine.MainWindow.SellCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: Sell.Enable: Filter alredy enabled");
			}
			else
				Assistant.Engine.MainWindow.SellCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellCheckBox.Checked = true));
		}

		public static void Disable()
		{
			if (Assistant.Engine.MainWindow.SellCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: Sell.Disable: Filter alredy disabled");
			}
			else
				Assistant.Engine.MainWindow.SellCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.SellCheckBox.Checked;
		}

		public static void ChangeList(string nomelista)
		{
			if (!Assistant.Engine.MainWindow.SellListSelect.Items.Contains(nomelista))
            {
				Scripts.SendMessageScriptError("Script Error: Sell.ChangeList: Scavenger list: " + nomelista + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.SellCheckBox.Checked == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.SellCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellCheckBox.Checked = false));
					Assistant.Engine.MainWindow.SellListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.SellListSelect.SelectedIndex = Assistant.Engine.MainWindow.SellListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.SellCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellCheckBox.Checked = true));
				}
				else
				{
					Assistant.Engine.MainWindow.SellListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.SellListSelect.SelectedIndex = Assistant.Engine.MainWindow.SellListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}
	}

	public class BuyAgent
	{
		[Serializable]
		public class BuyAgentItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Amount;
			public int Amount { get { return m_Amount; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public BuyAgentItem(string name, int graphics, int amount, int color, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Amount = amount;
				m_Color = color;
				m_Selected = selected;
			}
		}

		internal class BuyAgentList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

			public BuyAgentList(string description, bool selected)
			{
				m_Description = description;
				m_Selected = selected;
			}
		}

		internal static string BuyListName
		{
			get
			{
				return (string)Assistant.Engine.MainWindow.BuyListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.BuyListSelect.Text));
			}

			set
			{
				Assistant.Engine.MainWindow.BuyListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyListSelect.Text = value));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (Engine.Running)
			{
				Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.Items.Add(addlog)));
				Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.SelectedIndex = Assistant.Engine.MainWindow.BuyLogBox.Items.Count - 1));
				if (Assistant.Engine.MainWindow.BuyLogBox.Items.Count > 300)
					Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.Items.Clear()));
			}
		}

		internal static void RefreshLists()
		{
			List<BuyAgentList> lists;
			RazorEnhanced.Settings.BuyAgent.ListsRead(out lists);

			if (lists.Count == 0)
				Assistant.Engine.MainWindow.BuyListView.Items.Clear();

			BuyAgentList selectedList = lists.Where(l => l.Selected).FirstOrDefault();
			if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.BuyListSelect.Text)
				return;

			Assistant.Engine.MainWindow.BuyListSelect.Items.Clear();
			foreach (BuyAgentList l in lists)
			{
				Assistant.Engine.MainWindow.BuyListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Assistant.Engine.MainWindow.BuyListSelect.SelectedIndex = Assistant.Engine.MainWindow.BuyListSelect.Items.IndexOf(l.Description);
				}
			}
		}

		internal static void RefreshItems()
		{
			List<BuyAgentList> lists;
			RazorEnhanced.Settings.BuyAgent.ListsRead(out lists);

			Assistant.Engine.MainWindow.BuyListView.Items.Clear();
			foreach (BuyAgentList l in lists)
			{
				if (l.Selected)
				{
					List<BuyAgent.BuyAgentItem> items;
					RazorEnhanced.Settings.BuyAgent.ItemsRead(l.Description, out items);

					foreach (BuyAgentItem item in items)
					{
						ListViewItem listitem = new ListViewItem();

						listitem.Checked = item.Selected;

						listitem.SubItems.Add(item.Name);
						listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));

						listitem.SubItems.Add(item.Amount.ToString());

						if (item.Color == -1)
							listitem.SubItems.Add("All");
						else
							listitem.SubItems.Add("0x" + item.Color.ToString("X4"));

						Assistant.Engine.MainWindow.BuyListView.Items.Add(listitem);
					}
				}
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<BuyAgentItem> items;
			RazorEnhanced.Settings.BuyAgent.ItemsRead(BuyListName, out items);

			if (items.Count != Assistant.Engine.MainWindow.BuyListView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.BuyListView.Items[i];
			BuyAgentItem old = items[i];

			if (lvi != null && old != null)
			{
				BuyAgentItem item = new BuyAgent.BuyAgentItem(old.Name, old.Graphics, old.Amount, old.Color, lvi.Checked);
				RazorEnhanced.Settings.BuyAgent.ItemReplace(RazorEnhanced.BuyAgent.BuyListName, i, item);
			}
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.BuyAgent.ListInsert(newList);

			RazorEnhanced.BuyAgent.RefreshLists();
			RazorEnhanced.BuyAgent.RefreshItems();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.BuyAgent.ListExists(list))
			{
				RazorEnhanced.Settings.BuyAgent.ListDelete(list);
			}

			RazorEnhanced.BuyAgent.RefreshLists();
			RazorEnhanced.BuyAgent.RefreshItems();
		}

		internal static void AddItemToList(string name, int graphics, int amount, int color)
		{
			BuyAgentItem item = new BuyAgentItem(name, graphics, amount, color, false);

			string selection = Assistant.Engine.MainWindow.BuyListSelect.Text;

			if (RazorEnhanced.Settings.BuyAgent.ListExists(selection))
			{
				if (!RazorEnhanced.Settings.BuyAgent.ItemExists(selection, item))
					RazorEnhanced.Settings.BuyAgent.ItemInsert(selection, item);
			}

			RazorEnhanced.BuyAgent.RefreshItems();
		}

		internal static void ModifyItemInList(string name, int graphics, int amount, int color, bool selected, BuyAgentItem old, int index)
		{
			BuyAgentItem item = new BuyAgentItem(name, graphics, amount, color, selected);

			string selection = Assistant.Engine.MainWindow.BuyListSelect.Text;

			if (RazorEnhanced.Settings.BuyAgent.ListExists(selection))
			{
				if (RazorEnhanced.Settings.BuyAgent.ItemExists(selection, old))
					RazorEnhanced.Settings.BuyAgent.ItemReplace(selection, index, item);
			}

			RazorEnhanced.BuyAgent.RefreshItems();
		}

		internal static void EnableBuyFilter()
		{
			PacketHandler.RegisterServerToClientViewer(0x24, new PacketViewerCallback(DisplayBuy));
		}

		private static bool ColorCheck(int colorDaLista, ushort colorDaVendor)
		{
			if (colorDaLista == -1) // Wildcard colore
				return true;
			else
				if (colorDaLista == colorDaVendor) // Match OK
				return true;
			else  // Match fallito
				return false;
		}

		private static void DisplayBuy(PacketReader p, PacketHandlerEventArgs args)
		{
			if (!Assistant.Engine.MainWindow.BuyCheckBox.Checked) // Filtro disabilitato
				return;

			Assistant.Serial serial = p.ReadUInt32();
			ushort gump = p.ReadUInt16();

			Assistant.Mobile vendor = Assistant.World.FindMobile(serial);
			if (vendor == null)
				return;

			Assistant.Item pack = vendor.GetItemOnLayer(Layer.ShopBuy);
			if (pack == null || pack.Contains == null || pack.Contains.Count <= 0)
				return;

			int total = 0;
			int cost = 0;
			List<Assistant.VendorBuyItem> buyList = new List<Assistant.VendorBuyItem>(); // Lista definita altrove (non rimuovere se si fa pulizia in giro)

			for (int i = 0; i < pack.Contains.Count; i++) // Scan item lista oggetti in vendita
			{
				Assistant.Item item = (Assistant.Item)pack.Contains[i];
				if (item == null)
					continue;

				List<BuyAgent.BuyAgentItem> items;
				RazorEnhanced.Settings.BuyAgent.ItemsRead(BuyListName, out items);

				foreach (BuyAgentItem buyItem in items) // Scansione item presenti in lista agent item
				{
					if (!buyItem.Selected)
						continue;

					if (buyItem.Graphics == item.ItemID && RazorEnhanced.BuyAgent.ColorCheck(buyItem.Color, item.Hue)) // Verifica match fra lista e oggetti presenti del vendor
					{
						if (item.Amount >= buyItem.Amount) // Caso che il vendor abbia piu' item di quelli richiesti
						{
							AddLog("Item match: 0x" + buyItem.Graphics.ToString("X4") + " - Amount: " + item.Amount + " - Buyed: " + buyItem.Amount);
							buyList.Add(new VendorBuyItem(item.Serial, buyItem.Amount, item.Price));
							total += buyItem.Amount;
							cost += item.Price * buyItem.Amount;
						}
						else // Caso che il vendor ne abbia di meno (Li compro tutti)
						{
							AddLog("Item match: 0x" + buyItem.Graphics.ToString("X4") + " - Amount: " + item.Amount + " - Buyed: " + item.Amount);
							buyList.Add(new VendorBuyItem(item.Serial, item.Amount, item.Price));
							total += item.Amount;
							cost += item.Price * item.Amount;
						}
					}
				}
			}

			if (buyList.Count > 0)
			{
				args.Block = true;
				ClientCommunication.SendToServer(new VendorBuyResponse(serial, buyList));
				AddLog("Bought " + total.ToString() + " items for " + cost.ToString() + " gold coins");
				World.Player.SendMessage("Enhanced Buy Agent: bought " + total.ToString() + " items for " + cost.ToString() + " gold coins");
			}
		}

		// Funzioni da script
		public static void Enable()
		{
			if (!ClientCommunication.AllowBit(FeatureBit.BuyAgent))
			{
				Scripts.SendMessageScriptError("BuyAgent Not Allowed!");
				return;
			}

			if (Assistant.Engine.MainWindow.BuyCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: Buy.Enable: Filter alredy enabled");
			}
			else
				Assistant.Engine.MainWindow.BuyCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyCheckBox.Checked = true));
		}

		public static void Disable()
		{
			if (Assistant.Engine.MainWindow.BuyCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: Buy.Disable: Filter alredy disabled");
			}
			else
				Assistant.Engine.MainWindow.BuyCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.BuyCheckBox.Checked;
		}

		public static void ChangeList(string nomelista)
		{
			if (!Assistant.Engine.MainWindow.BuyListSelect.Items.Contains(nomelista))
			{
				Scripts.SendMessageScriptError("Script Error: Buy.ChangeList: Scavenger list: " + nomelista + " not exist");
			}
			else
			{
				if (Assistant.Engine.MainWindow.BuyCheckBox.Checked == true) // Se è in esecuzione forza stop cambio lista e restart
				{
					Assistant.Engine.MainWindow.BuyCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyCheckBox.Checked = false));
					Assistant.Engine.MainWindow.BuyListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyListSelect.SelectedIndex = Assistant.Engine.MainWindow.BuyListSelect.Items.IndexOf(nomelista)));  // cambio lista
					Assistant.Engine.MainWindow.BuyCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyCheckBox.Checked = true));
				}
				else
				{
					Assistant.Engine.MainWindow.BuyListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyListSelect.SelectedIndex = Assistant.Engine.MainWindow.BuyListSelect.Items.IndexOf(nomelista)));  // cambio lista
				}
			}
		}
	}
}