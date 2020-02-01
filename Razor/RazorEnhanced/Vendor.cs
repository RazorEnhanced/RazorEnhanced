using Assistant;
using Assistant.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class SellAgent
	{
		private static string m_listname;
		private static int m_sellbag;

		[Serializable]
		public class SellAgentItem : ListAbleItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_amount;
			public int Amount { get { return m_amount; } }

			private int m_color;
			public int Color { get { return m_color; } }


			[JsonProperty("Selected")]
			internal bool Selected { get; set; }

			public SellAgentItem(string name, int graphics, int amount, int color, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_amount = amount;
				m_color = color;
				Selected = selected;
			}
		}

		internal class SellAgentList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			[JsonProperty("Selected")]
			internal bool Selected { get; set;}

			public SellAgentList(string description, int bag, bool selected)
			{
				m_Description = description;
				m_Bag = bag;
				Selected = selected;
			}
		}

		internal static string SellListName
		{
			get { return m_listname; }
			set { m_listname = value; }
		}

		internal static int SellBag
		{
			get { return m_sellbag; }

			set
			{
				m_sellbag = value;
				Assistant.Engine.MainWindow.SafeAction(s => s.SellBagLabel.Text = "0x" + value.ToString("X8"));
			}
		}

		internal static void AddLog(string addlog)
		{
			if (Client.Running)
			{
				Engine.MainWindow.SafeAction(s => s.SellLogBox.Items.Add(addlog));
				Engine.MainWindow.SafeAction(s => s.SellLogBox.SelectedIndex = s.SellLogBox.Items.Count - 1);
				if (Engine.MainWindow.SellLogBox.Items.Count > 300)
					Engine.MainWindow.SafeAction(s => s.SellLogBox.Items.Clear());
			}
		}

		internal static void RefreshLists()
		{
			List<SellAgentList> lists = Settings.SellAgent.ListsRead();

			SellAgentList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Engine.MainWindow.SellListSelect.Text)
				return;

			Engine.MainWindow.SellListSelect.Items.Clear();
			foreach (SellAgentList l in lists)
			{
				Engine.MainWindow.SellListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					Engine.MainWindow.SellListSelect.SelectedIndex = Engine.MainWindow.SellListSelect.Items.IndexOf(l.Description);
					SellBag = l.Bag;
					m_listname = l.Description;
				}
			}
		}

		internal static void CopyTable()
		{
			Settings.SellAgent.ClearList(Engine.MainWindow.SellListSelect.Text); // Rimuove vecchi dati dal save

			foreach (DataGridViewRow row in Engine.MainWindow.VendorSellGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[4].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[4].Value, 16);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.SellAgent.ItemInsert(Engine.MainWindow.SellListSelect.Text, new SellAgentItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), Convert.ToInt32(row.Cells[3].Value), color, check));
			}

			Settings.Save(); // Salvo dati
		}

        internal static void InitGrid()
		{
			List<SellAgentList> lists = Settings.SellAgent.ListsRead();

			Engine.MainWindow.VendorSellGridView.Rows.Clear();

			foreach (SellAgentList l in lists)
			{
				if (l.Selected)
				{
					List<SellAgent.SellAgentItem> items = Settings.SellAgent.ItemsRead(l.Description);

                    foreach (SellAgentItem item in items)
					{
						string color = "All";
						if (item.Color != -1)
							color = "0x" + item.Color.ToString("X4");

						Engine.MainWindow.VendorSellGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, "0x"+item.Graphics.ToString("X4"), item.Amount, color });
					}

					break;
				}
			}
		}
		internal static void CloneList(string newList)
		{
			RazorEnhanced.Settings.SellAgent.ListInsert(newList, SellBag);

			foreach (DataGridViewRow row in Engine.MainWindow.VendorSellGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[4].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[4].Value, 16);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.SellAgent.ItemInsert(newList, new SellAgentItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), Convert.ToInt32(row.Cells[3].Value), color, check));
			}

			Settings.Save(); // Salvo dati

			RazorEnhanced.SellAgent.RefreshLists();
			RazorEnhanced.SellAgent.InitGrid();
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.SellAgent.ListInsert(newList, 0);

			RazorEnhanced.SellAgent.RefreshLists();
			RazorEnhanced.SellAgent.InitGrid();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.SellAgent.ListExists(list))
			{
				RazorEnhanced.Settings.SellAgent.ListDelete(list);
			}

			RazorEnhanced.SellAgent.RefreshLists();
			RazorEnhanced.SellAgent.InitGrid();
		}

		internal static void AddItemToList(string name, int graphics, int amount, int color)
		{
			Engine.MainWindow.VendorSellGridView.Rows.Add(new object[] { "True", name, "0x" + graphics.ToString("X4"), amount, "0x" + color.ToString("X4") });
			CopyTable();
		}

		internal static void EnableSellFilter()
		{
			PacketHandler.RegisterServerToClientViewer(0x9E, new PacketViewerCallback(OnVendorSell));
		}

		private static bool ColorCheck(int colorDaLista, ushort colorDaVendor)
		{
			if (colorDaLista == -1)         // Wildcard colore
				return true;
			if (colorDaLista == colorDaVendor)      // Match OK
				return true;
			return false;
		}

		private static void OnVendorSell(PacketReader pvSrc, PacketHandlerEventArgs args)
		{
			if (!Engine.MainWindow.SellCheckBox.Checked) // Filtro disabilitato
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

			if (count == 0) // Il vendor non compra nulla
				return;

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

				List<SellAgent.SellAgentItem> items = Settings.SellAgent.ItemsRead(SellListName);

				foreach (SellAgentItem sellItem in items) // Scansione item presenti in lista agent item
				{
					if (!sellItem.Selected)
						continue;

					if (gfx != sellItem.Graphics || (item == null || item == bag || !item.IsChildOf(bag)) || !RazorEnhanced.SellAgent.ColorCheck(sellItem.Color, hue))
						continue;

					int amountLeft = int.MaxValue;
					int index = 0;
					bool alreadySold = false;

					for (int y = 0; y < templist.Count; y++) // Controllo che non ho gia venduto item simili
					{
						if (templist[y].Graphics != gfx || !RazorEnhanced.SellAgent.ColorCheck(templist[y].Color, hue))
							continue;

						alreadySold = true;
						amountLeft = templist[y].Amount;
						index = y;
					}

					if (amountLeft == int.MaxValue) // Valore limite e inizzializzazione
						amountLeft = sellItem.Amount;

					if (amountLeft <= 0)
						continue;

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

			if (list.Count <= 0)
				return;

	 		Assistant.Client.Instance.SendToServer(new VendorSellResponse(vendor, list));
			AddLog("Sold " + sold.ToString() + " items for " + total.ToString() + " gold coins");
			string message = "Enhanced Sell Agent: sold " + sold.ToString() + " items for " + total.ToString() + " gold coins";
			World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(message, "System", 1, "Vendor", vendor.Serial));          // Journal buffer
			World.Player.SendMessage(message);
			args.Block = true;
		}

		// Funzioni da script
		public static void Enable()
		{
			if (!Assistant.Client.Instance.AllowBit(FeatureBit.SellAgent))
			{
				Scripts.SendMessageScriptError("SellAgent Not Allowed!");
				return;
			}

			if (Engine.MainWindow.SellCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: Sell.Enable: Filter alredy enabled");
			}
			else
				Assistant.Engine.MainWindow.SafeAction(s => s.SellCheckBox.Checked = true);
		}

		public static void Disable()
		{
			if (Engine.MainWindow.SellCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: Sell.Disable: Filter alredy disabled");
			}
			else
				Assistant.Engine.MainWindow.SafeAction(s => s.SellCheckBox.Checked = false);
		}

		public static bool Status()
		{
			return Engine.MainWindow.SellCheckBox.Checked;
		}

		public static void ChangeList(string listName)
		{
			if (!UpdateListParam(listName))
            {
				Scripts.SendMessageScriptError("Script Error: Sell.ChangeList: Scavenger list: " + listName + " not exist");
			}
			else
			{
				if (Engine.MainWindow.SellCheckBox.Checked == true) // Se è in esecuzione forza stop change list e restart
				{
					Assistant.Engine.MainWindow.SafeAction(s => s.SellCheckBox.Checked = false);
					Assistant.Engine.MainWindow.SafeAction(s => s.SellListSelect.SelectedIndex = Engine.MainWindow.SellListSelect.Items.IndexOf(listName));  // change list
					Assistant.Engine.MainWindow.SafeAction(s => s.SellCheckBox.Checked = true);
				}
				else
				{
					Assistant.Engine.MainWindow.SafeAction(s => s.SellListSelect.SelectedIndex = Engine.MainWindow.SellListSelect.Items.IndexOf(listName));  // change list
				}
			}
		}
		internal static bool UpdateListParam(string listName)
		{
			if (Settings.SellAgent.ListExists(listName))
			{
				SellAgent.SellBag = Settings.SellAgent.BagRead(listName);
				SellAgent.SellListName = listName;
				return true;
			}
			return false;
		}
	}

	public class BuyAgent
	{
		private static string m_listname;

		[Serializable]
		public class BuyAgentItem  : ListAbleItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Amount;
			public int Amount { get { return m_Amount; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			[JsonProperty("Selected")]
			internal bool Selected { get; set; }

			public BuyAgentItem(string name, int graphics, int amount, int color, bool selected)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Amount = amount;
				m_Color = color;
				Selected = selected;
			}
		}

		internal class BuyAgentList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			[JsonProperty("Selected")]
			internal bool Selected { get; set;}

			public BuyAgentList(string description, bool selected)
			{
				m_Description = description;
				Selected = selected;
			}
		}

		internal static string BuyListName
		{
			get { return m_listname; }
			set { m_listname = value; }
		}

		internal static void AddLog(string addlog)
		{
			if (Client.Running)
			{
				Engine.MainWindow.SafeAction(s => s.BuyLogBox.Items.Add(addlog));
				Engine.MainWindow.SafeAction(s => s.BuyLogBox.SelectedIndex = s.BuyLogBox.Items.Count - 1);
				if (Engine.MainWindow.BuyLogBox.Items.Count > 300)
					Engine.MainWindow.SafeAction(s => s.BuyLogBox.Items.Clear());
			}
		}

		internal static void RefreshLists()
		{
			List<BuyAgentList> lists = Settings.BuyAgent.ListsRead();

			BuyAgentList selectedList = lists.FirstOrDefault(l => l.Selected);
			if (selectedList != null && selectedList.Description == Engine.MainWindow.BuyListSelect.Text)
				return;

			Engine.MainWindow.BuyListSelect.Items.Clear();
			foreach (BuyAgentList l in lists)
			{
				Engine.MainWindow.BuyListSelect.Items.Add(l.Description);

				if (l.Selected)
				{
					m_listname = l.Description;
					Engine.MainWindow.BuyListSelect.SelectedIndex = Engine.MainWindow.BuyListSelect.Items.IndexOf(l.Description);
				}
			}
		}

		internal static void CopyTable()
		{
			Settings.BuyAgent.ClearList(Engine.MainWindow.BuyListSelect.Text); // Rimuove vecchi dati dal save

			foreach (DataGridViewRow row in Engine.MainWindow.VendorBuyDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[4].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[4].Value, 16);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.BuyAgent.ItemInsert(Engine.MainWindow.BuyListSelect.Text, new BuyAgentItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), Convert.ToInt32(row.Cells[3].Value), color, check));
			}

			Settings.Save(); // Salvo dati
		}

		internal static void InitGrid()
		{
			List<BuyAgentList> lists = Settings.BuyAgent.ListsRead();

			Engine.MainWindow.VendorBuyDataGridView.Rows.Clear();

			foreach (BuyAgentList l in lists)
			{
				if (l.Selected)
				{
					List<BuyAgentItem> items = Settings.BuyAgent.ItemsRead(l.Description);

					foreach (BuyAgentItem item in items)
					{
						string color = "All";
						if (item.Color != -1)
							color = "0x" + item.Color.ToString("X4");

						Engine.MainWindow.VendorBuyDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, "0x" + item.Graphics.ToString("X4"), item.Amount, color });
					}

					break;
				}
			}
		}

		internal static void CloneList(string newList)
		{
			RazorEnhanced.Settings.BuyAgent.ListInsert(newList);

			foreach (DataGridViewRow row in Engine.MainWindow.VendorBuyDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				int color = 0;
				if ((string)row.Cells[4].Value == "All")
					color = -1;
				else
					color = Convert.ToInt32((string)row.Cells[4].Value, 16);

				bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

				Settings.BuyAgent.ItemInsert(newList, new BuyAgentItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), Convert.ToInt32(row.Cells[3].Value), color, check));
			}

			Settings.Save(); // Salvo dati

			RazorEnhanced.BuyAgent.RefreshLists();
			RazorEnhanced.BuyAgent.InitGrid();
		}

		internal static void AddList(string newList)
		{
			RazorEnhanced.Settings.BuyAgent.ListInsert(newList);

			RazorEnhanced.BuyAgent.RefreshLists();
			RazorEnhanced.BuyAgent.InitGrid();
		}

		internal static void RemoveList(string list)
		{
			if (RazorEnhanced.Settings.BuyAgent.ListExists(list))
			{
				RazorEnhanced.Settings.BuyAgent.ListDelete(list);
			}

			RazorEnhanced.BuyAgent.RefreshLists();
			RazorEnhanced.BuyAgent.InitGrid();
		}

		internal static void AddItemToList(string name, int graphics, int amount, int color)
		{
			Engine.MainWindow.VendorBuyDataGridView.Rows.Add(new object[] { "True", name, "0x" + graphics.ToString("X4"), amount, "0x" + color.ToString("X4") });
			CopyTable();
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
			if (!Engine.MainWindow.BuyCheckBox.Checked) // Filtro disabilitato
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

			for (int i = 0; i < pack.Contains.Count; i++)
			{
				if (pack.Contains[i] == null)
					continue;

				List<BuyAgent.BuyAgentItem> items = Settings.BuyAgent.ItemsRead(m_listname);

				foreach (BuyAgentItem buyItem in items) // Scansione item presenti in lista agent item
				{
					if (!buyItem.Selected)
						continue;

					if (buyItem.Graphics != pack.Contains[i].ItemID || !RazorEnhanced.BuyAgent.ColorCheck(buyItem.Color, pack.Contains[i].Hue))
						continue;

					if (pack.Contains[i].Amount >= buyItem.Amount) // Caso che il vendor abbia piu' item di quelli richiesti
					{
						AddLog("Item match: 0x" + buyItem.Graphics.ToString("X4") + " - Amount: " + pack.Contains[i].Amount + " - Buyed: " + buyItem.Amount);
						buyList.Add(new VendorBuyItem(pack.Contains[i].Serial, buyItem.Amount, pack.Contains[i].Price));
						total += buyItem.Amount;
						cost += pack.Contains[i].Price * buyItem.Amount;
					}
					else // Caso che il vendor ne abbia di meno (Li compro tutti)
					{
						AddLog("Item match: 0x" + buyItem.Graphics.ToString("X4") + " - Amount: " + pack.Contains[i].Amount + " - Buyed: " + pack.Contains[i].Amount);
						buyList.Add(new VendorBuyItem(pack.Contains[i].Serial, pack.Contains[i].Amount, pack.Contains[i].Price));
						total += pack.Contains[i].Amount;
						cost += pack.Contains[i].Price * pack.Contains[i].Amount;
					}
				}
			}

			if (buyList.Count <= 0)
				return;

			args.Block = true;
	 		Assistant.Client.Instance.SendToServer(new VendorBuyResponse(serial, buyList));

			string message = "Enhanced Buy Agent: bought " + total.ToString() + " items for " + cost.ToString() + " gold coins";
			World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(message, "System", 1, "Vendor", vendor.Serial));          // Journal buffer
			World.Player.SendMessage(message);
			AddLog("Bought " + total.ToString() + " items for " + cost.ToString() + " gold coins");
		}

		// Funzioni da script
		public static void Enable()
		{
			if (!Assistant.Client.Instance.AllowBit(FeatureBit.BuyAgent))
			{
				Scripts.SendMessageScriptError("BuyAgent Not Allowed!");
				return;
			}

			if (Engine.MainWindow.BuyCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: Buy.Enable: Filter alredy enabled");
			}
			else
				Assistant.Engine.MainWindow.SafeAction(s => s.BuyCheckBox.Checked = true);
		}

		public static void Disable()
		{
			if (Engine.MainWindow.BuyCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: Buy.Disable: Filter alredy disabled");
			}
			else
				Engine.MainWindow.SafeAction(s => s.BuyCheckBox.Checked = false);
		}

		public static bool Status()
		{
			return Engine.MainWindow.BuyCheckBox.Checked;
		}

		public static void ChangeList(string listName)
		{
			if (!Engine.MainWindow.BuyListSelect.Items.Contains(listName))
			{
				Scripts.SendMessageScriptError("Script Error: Buy.ChangeList: Scavenger list: " + listName + " not exist");
			}
			else
			{
				m_listname = listName;
				if (Engine.MainWindow.BuyCheckBox.Checked == true) // Se è in esecuzione forza stop change list e restart
				{
					Assistant.Engine.MainWindow.SafeAction(s => s.BuyCheckBox.Checked = false);
					Assistant.Engine.MainWindow.SafeAction(s => s.BuyListSelect.SelectedIndex = Engine.MainWindow.BuyListSelect.Items.IndexOf(listName));  // change list
					Assistant.Engine.MainWindow.SafeAction(s => s.BuyCheckBox.Checked = true);
				}
				else
				{
					Assistant.Engine.MainWindow.SafeAction(s => s.BuyListSelect.SelectedIndex = s.BuyListSelect.Items.IndexOf(listName));  // change list
				}
			}
		}
	}
}
