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
    public class SellAgent
    {
        [Serializable]
        public class SellItem
        {
            private string m_Name;
            public string Name { get { return m_Name; } }

            private int m_Graphics;
            public int Graphics { get { return m_Graphics; } }

            private int m_amount;
            public int Amount { get { return m_amount; } }

            private int m_color;
            public int Color { get { return m_color; } }

            public SellItem(string name, int graphics, int amount, int color)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_amount = amount;
                m_color = color;
            }
        }

        public class SellTempItem
        {
            private int m_Graphics;
            public int Graphics { get { return m_Graphics; } }

            private int m_amountleft;
            public int AmountLeft { get { return m_amountleft; } }

            private int m_color;
            public int Color { get { return m_color; } }

            public SellTempItem(int graphics, int amountleft, int color)
            {
                m_Graphics = graphics;
                m_amountleft = amountleft;
                m_color = color;
            }
        }

        internal static void RefreshList(List<SellItem> SellItemList)
        {
            Assistant.Engine.MainWindow.SellListView.Items.Clear();
            foreach (SellItem item in SellItemList)
            {
                ListViewItem listitem = new ListViewItem();
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

        internal static void ModifyItemToList(string name, int graphics, int amount, int color, ListView sellListView, List<SellItem> sellItemList, int indexToInsert)
        {
            sellItemList.RemoveAt(indexToInsert);                                                       // rimuove
            sellItemList.Insert(indexToInsert, new SellItem(name, graphics, amount, color));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveSellItemList(Assistant.Engine.MainWindow.SellListSelect.SelectedItem.ToString(), sellItemList, Assistant.Engine.MainWindow.SellBagLabel.Text);
            RazorEnhanced.SellAgent.RefreshList(sellItemList);
        }

        internal static void AddItemToList(string name, int graphics, int amount, int color, ListView sellListView, List<SellItem> sellItemList)
        {
            sellItemList.Add(new SellItem(name, graphics, amount, color));
            RazorEnhanced.Settings.SaveSellItemList(Assistant.Engine.MainWindow.SellListSelect.SelectedItem.ToString(), sellItemList, Assistant.Engine.MainWindow.SellBagLabel.Text);
            RazorEnhanced.SellAgent.RefreshList(sellItemList);
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.SelectedIndex = Assistant.Engine.MainWindow.SellLogBox.Items.Count - 1));
        }
        internal static void EnableSellFilter()
        {
            PacketHandler.RegisterServerToClientViewer(0x9E, new PacketViewerCallback(OnVendorSell));
        }

        internal static Assistant.Item HotBag
        {
            get
            {
                Assistant.Item HotBag = null;
                int SerialHotBag = Convert.ToInt32(Assistant.Engine.MainWindow.SellBagLabel.Text, 16);

                if (SerialHotBag == 0)
                {
                    return null;
                }
                else
                {
                    HotBag = Assistant.World.FindItem(SerialHotBag);
                    if (HotBag.RootContainer != World.Player || !HotBag.IsContainer)
                        return null;
                    else
                        return HotBag;
                }
            }
        }

        private static bool ColorCheck(int ColorDaLista, ushort ColorDaVendor)
        {
            if (ColorDaLista == -1)         // Wildcard colore
                return true;
            else
                if (ColorDaLista == ColorDaVendor)      // Match OK
                    return true;
                else            // Match fallito
                    return false;
        }

        private static void OnVendorSell(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.SellCheckBox.Checked)          // Filtro disabilitato
                return;

            if (SellAgent.HotBag == null)         // Verifica HotBag
            {
                AddLog("Invalid or not accessible HotBag");
                return;
            }

            int total = 0;
            uint serial = pvSrc.ReadUInt32();
            Assistant.Mobile vendor = Assistant.World.FindMobile(serial);
            if (vendor == null)
                Assistant.World.AddMobile(vendor = new Assistant.Mobile(serial));
            int count = pvSrc.ReadUInt16();
            int sold = 0;

           
            List<Assistant.SellListItem> list = new List<Assistant.SellListItem>(count);                // Lista item checkati per vendita (non so dove sia dichiarata)
            List<RazorEnhanced.SellAgent.SellTempItem> templist = new List<RazorEnhanced.SellAgent.SellTempItem>();         // Lista temporanea per controlli amount

            AddLog("HotBag: 0x" + HotBag.Serial.Value.ToString("X8"));

            for (int i = 0; i < count; i++)         // Scansione item in lista menu vendor
            {
                uint ser = pvSrc.ReadUInt32();
                ushort gfx = pvSrc.ReadUInt16();
                ushort hue = pvSrc.ReadUInt16();
                ushort amount = pvSrc.ReadUInt16();
                ushort price = pvSrc.ReadUInt16();
                pvSrc.ReadString(pvSrc.ReadUInt16()); //name

                Assistant.Item item = Assistant.World.FindItem(ser);

                foreach (SellItem SellItemList in Assistant.Engine.MainWindow.SellItemList) // Scansione item presenti in lista agent item 
                {
                    if (gfx == SellItemList.Graphics && (item != null && item != HotBag && item.IsChildOf(HotBag)) && RazorEnhanced.SellAgent.ColorCheck(SellItemList.Color, hue))                   // match sulla grafica fra lista agent e lista vendor e hotbag              
                    {
                        int AmountLefta = 60000;
                        int Index = 0;
                        bool GiaVenduto = false;

                        for (int y = 0; y < templist.Count; y++)            // Controllo che non ho gia venduto item simili
                        {
                            if (templist[y].Graphics == gfx && RazorEnhanced.SellAgent.ColorCheck(templist[y].Color, hue))
                            {
                                GiaVenduto = true;
                                AmountLefta = templist[y].AmountLeft;
                                Index = y;
                            }
                        }

                        if (AmountLefta == 60000)                      // Valore limite e inizzializzazione
                            AmountLefta = SellItemList.Amount;

                        if (AmountLefta > 0) // Controlla se mancano da vendere
                        {
                            if (GiaVenduto)  // Gia venduto oggetto stessa grafica
                            {
                                AddLog("Item match: 0x" + SellItemList.Graphics.ToString("X4") + " - Amount: " + SellItemList.Amount + " - Left: " + AmountLefta);
                                if (amount < AmountLefta)        // In caso che quella listata nel vendor sia minore di quella che voglio vendere vendo il massimo possibile
                                {
                                    int Amountleft = AmountLefta - amount;
                                    list.Add(new SellListItem(ser, amount));            // Lista processo vendita
                                    templist.RemoveAt(Index);
                                    templist.Insert(Index, new SellTempItem(gfx, Amountleft, SellItemList.Color));
                                    total += amount * price;
                                    sold += amount;
                                }
                                else               // Caso che quella listata nel vendor sia maggiore vendo solo quella mancante 
                                {
                                    list.Add(new SellListItem(ser, Convert.ToUInt16(AmountLefta)));  // Lista processo vendita
                                    templist.RemoveAt(Index);
                                    templist.Insert(Index, new SellTempItem(gfx, 0, SellItemList.Color));
                                    total += AmountLefta * price;
                                    sold += AmountLefta;
                                }
                            }
                            else     // Mai venduto oggetto stessa grafica
                            {
                                AddLog("Item match: 0x" + SellItemList.Graphics.ToString("X4") + " - Amount: " + SellItemList.Amount + " - Left: " + SellItemList.Amount);
                                if (amount < SellItemList.Amount)        // In caso che quella listata nel vendor sia minore di quella che voglio vendere vendo il massimo possibile
                                {
                                    list.Add(new SellListItem(ser, amount));  // Lista processo vendita
                                    templist.Add(new SellTempItem(gfx, (SellItemList.Amount - amount), SellItemList.Color));
                                    total += amount * price;
                                    sold += amount;
                                }
                                else               // Caso che quella listata nel vendor sia maggiore vendo solo quella mancante 
                                {
                                    list.Add(new SellListItem(ser, Convert.ToUInt16(SellItemList.Amount)));  // Lista processo vendita
                                    templist.Add(new SellTempItem(gfx, 0, SellItemList.Color));
                                    total += SellItemList.Amount * price;
                                    sold += SellItemList.Amount;
                                }
                            }
                        }

                    }
                }
                        
                                            
            }

            if (list.Count > 0)
            {
                ClientCommunication.SendToServer(new VendorSellResponse(vendor, list));
                AddLog("Sell " + sold.ToString() + "items for " + total.ToString() + " gold coin");
                World.Player.SendMessage("Enhanced Sell Agent: Sell " + sold.ToString() + " items for " + total.ToString() + " gold coin");
                args.Block = true;
            }
        }
    }

    public class BuyAgent
    {
        [Serializable]
        public class BuyItem
        {
            private string m_Name;
            public string Name { get { return m_Name; } }

            private int m_Graphics;
            public int Graphics { get { return m_Graphics; } }

            private int m_amount;
            public int Amount { get { return m_amount; } }

            private int m_color;
            public int Color { get { return m_color; } }

            public BuyItem(string name, int graphics, int amount, int color)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_amount = amount;
                m_color = color;
            }
        }
        internal static void RefreshList(List<BuyItem> BuyItemList)
        {
            Assistant.Engine.MainWindow.BuyListView.Items.Clear();
            foreach (BuyItem item in BuyItemList)
            {
                ListViewItem listitem = new ListViewItem();
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

        internal static void ModifyItemToList(string name, int graphics, int amount, int color, ListView buylListView, List<BuyItem> buyItemList, int indexToInsert)
        {
            buyItemList.RemoveAt(indexToInsert);                                                       // rimuove
            buyItemList.Insert(indexToInsert, new BuyItem(name, graphics, amount, color));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveBuyItemList(Assistant.Engine.MainWindow.BuyListSelect.SelectedItem.ToString(), buyItemList);
            RazorEnhanced.BuyAgent.RefreshList(buyItemList);
        }

        internal static void AddItemToList(string name, int graphics, int amount, int color, ListView buyListView, List<BuyItem> buyItemList)
        {
            buyItemList.Add(new BuyItem(name, graphics, amount, color));
            RazorEnhanced.Settings.SaveBuyItemList(Assistant.Engine.MainWindow.BuyListSelect.SelectedItem.ToString(), buyItemList);
            RazorEnhanced.BuyAgent.RefreshList(buyItemList);
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.SelectedIndex = Assistant.Engine.MainWindow.BuyLogBox.Items.Count - 1));
        }

        internal static void EnableBuyFilter()
        {
            PacketHandler.RegisterServerToClientViewer(0x24, new PacketViewerCallback(DisplayBuy));
        }

        private static bool ColorCheck(int ColorDaLista, ushort ColorDaVendor)
        {
            if (ColorDaLista == -1)         // Wildcard colore
                return true;
            else
                if (ColorDaLista == ColorDaVendor)      // Match OK
                    return true;
                else            // Match fallito
                    return false;
        }

        private static void DisplayBuy(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.BuyCheckBox.Checked)          // Filtro disabilitato
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
            List<Assistant.VendorBuyItem> buyList = new List<Assistant.VendorBuyItem>();                // Lista definita altrove (non rimuovere se si fa pulizia in giro)

            for (int i = 0; i < pack.Contains.Count; i++)                       // Scan item lista oggetti in vendita
            {
                Assistant.Item item = (Assistant.Item)pack.Contains[i];
                if (item == null)
                    continue;

                foreach (BuyItem BuyItemList in Assistant.Engine.MainWindow.BuyItemList) // Scansione item presenti in lista agent item 
                {
                    if (BuyItemList.Graphics == item.ItemID && RazorEnhanced.BuyAgent.ColorCheck(BuyItemList.Color, item.Hue))                  // Verifica match fra lista e oggetti presenti del vendor
                    { 
                        if (item.Amount >= BuyItemList.Amount)          // Caso che il vendor abbia piu' item di quelli richiesti
                        {
                            AddLog("Item match: 0x" + BuyItemList.Graphics.ToString("X4") + " - Amount: " + item.Amount + " - Buyed: " + BuyItemList.Amount);
                            buyList.Add(new VendorBuyItem(item.Serial, BuyItemList.Amount, item.Price));
                            total += BuyItemList.Amount;
                            cost += item.Price * BuyItemList.Amount;
                        }
                        else                // Caso che il vendor ne abbia di meno (Li compro tutti)
                        {
                            AddLog("Item match: 0x" + BuyItemList.Graphics.ToString("X4") + " - Amount: " + item.Amount + " - Buyed: " + item.Amount);
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
                AddLog("Buy " + total.ToString() + "items for " + cost.ToString() + " gold coin");
                World.Player.SendMessage("Enhanced Buy Agent: Buy " + total.ToString() + " items for " + cost.ToString() + " gold coin");
            }
        }

    }
}