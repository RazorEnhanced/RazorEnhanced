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

            public SellItem(string name, int graphics, int amount)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_amount = amount;
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
                Assistant.Engine.MainWindow.SellListView.Items.Add(listitem);
            }
        }

        internal static void ModifyItemToList(string name, int graphics, int amount, ListView sellListView, List<SellItem> sellItemList, int indexToInsert)
        {
            sellItemList.RemoveAt(indexToInsert);                                                       // rimuove
            sellItemList.Insert(indexToInsert, new SellItem(name, graphics, amount));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveSellItemList(Assistant.Engine.MainWindow.SellListSelect.SelectedItem.ToString(), sellItemList);
            RazorEnhanced.SellAgent.RefreshList(sellItemList);
        }

        internal static void AddItemToList(string name, int graphics, int amount, ListView sellListView, List<SellItem> sellItemList)
        {
            sellItemList.Add(new SellItem(name, graphics, amount));
            RazorEnhanced.Settings.SaveSellItemList(Assistant.Engine.MainWindow.SellListSelect.SelectedItem.ToString(), sellItemList);
            RazorEnhanced.SellAgent.RefreshList(sellItemList);
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.SellLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.SellLogBox.SelectedIndex = Assistant.Engine.MainWindow.SellLogBox.Items.Count - 1));
        }
        internal static void EnableSellFilter(List<SellItem> BuyItemList)
        {

        }
        internal static void DisableSellFilter()
        {

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

            public BuyItem(string name, int graphics, int amount)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_amount = amount;
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
                Assistant.Engine.MainWindow.BuyListView.Items.Add(listitem);
            }
        }

        internal static void ModifyItemToList(string name, int graphics, int amount, ListView buylListView, List<BuyItem> buyItemList, int indexToInsert)
        {
            buyItemList.RemoveAt(indexToInsert);                                                       // rimuove
            buyItemList.Insert(indexToInsert, new BuyItem(name, graphics, amount));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveBuyItemList(Assistant.Engine.MainWindow.BuyListSelect.SelectedItem.ToString(), buyItemList);
            RazorEnhanced.BuyAgent.RefreshList(buyItemList);
        }

        internal static void AddItemToList(string name, int graphics, int amount, ListView buyListView, List<BuyItem> buyItemList)
        {
            buyItemList.Add(new BuyItem(name, graphics, amount));
            RazorEnhanced.Settings.SaveBuyItemList(Assistant.Engine.MainWindow.BuyListSelect.SelectedItem.ToString(), buyItemList);
            RazorEnhanced.BuyAgent.RefreshList(buyItemList);
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.BuyLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BuyLogBox.SelectedIndex = Assistant.Engine.MainWindow.BuyLogBox.Items.Count - 1));
        }

        internal static void EnableBuyFilter(List<BuyItem> BuyItemList)
        {
            
        }
        internal static void DisableBuyFilter()
        {

        }
    }
}