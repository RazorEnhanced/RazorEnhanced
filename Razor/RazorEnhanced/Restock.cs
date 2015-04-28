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

                    if (serialBag == 0)
                    {
                        serialBag = (int)World.Player.Backpack.Serial.Value;
                    }
                    else
                    {
                        Item bag = RazorEnhanced.Items.FindBySerial(serialBag);
                        if (bag == null)
                            serialBag = (int)World.Player.Backpack.Serial.Value;
                        else
                            serialBag = bag.Serial;
                    }
                }
                catch
                {
                }

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

                    if (serialBag == 0)
                    {
                        serialBag = (int)World.Player.Backpack.Serial.Value;
                    }
                    else
                    {
                        Item bag = RazorEnhanced.Items.FindBySerial(serialBag);
                        if (bag == null)
                            serialBag = (int)World.Player.Backpack.Serial.Value;
                        else
                            serialBag = bag.Serial;
                    }
                }
                catch
                {
                }

                return serialBag;
            }

            set
            {
                Assistant.Engine.MainWindow.RestockDestinationLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockDestinationLabel.Text = "0x" + value.ToString("X8")));
            }
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.RestockLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.RestockLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockLogBox.SelectedIndex = Assistant.Engine.MainWindow.RestockLogBox.Items.Count - 1));
            if (Assistant.Engine.MainWindow.RestockLogBox.Items.Count > 300)
                Assistant.Engine.MainWindow.RestockLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.RestockLogBox.Items.Clear()));
        }

        internal static void RefreshLists()
        {
            List<RestockList> lists;
            RazorEnhanced.Settings.Restock.ListsRead(out lists);

            RestockList selectedList = lists.Where(l => l.Selected).FirstOrDefault();
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

        internal static void RefreshItems()
        {
            List<RestockList> lists;
            RazorEnhanced.Settings.Restock.ListsRead(out lists);

            Assistant.Engine.MainWindow.RestockListView.Items.Clear();
            foreach (RestockList l in lists)
            {
                if (l.Selected)
                {
                    List<Restock.RestockItem> items;
                    RazorEnhanced.Settings.Restock.ItemsRead(l.Description, out items);

                    foreach (RestockItem item in items)
                    {
                        ListViewItem listitem = new ListViewItem();

                        listitem.Checked = item.Selected;

                        listitem.SubItems.Add(item.Name);
                        listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));

                        if (item.Color == -1)
                            listitem.SubItems.Add("All");
                        else
                            listitem.SubItems.Add("0x" + item.Color.ToString("X4"));


                        listitem.SubItems.Add(item.AmountLimit.ToString());

                        Assistant.Engine.MainWindow.RestockListView.Items.Add(listitem);
                    }
                }
            }
        }

        internal static void UpdateSelectedItems(int i)
        {
            List<RestockItem> items;
            RazorEnhanced.Settings.Restock.ItemsRead(RestockListName, out items);

            if (items.Count != Assistant.Engine.MainWindow.RestockListView.Items.Count)
            {
                return;
            }

            ListViewItem lvi = Assistant.Engine.MainWindow.RestockListView.Items[i];
            RestockItem old = items[i];

            if (lvi != null && old != null)
            {
                RestockItem item = new Restock.RestockItem(old.Name, old.Graphics, old.Color, old.AmountLimit, lvi.Checked);
                RazorEnhanced.Settings.Restock.ItemReplace(RazorEnhanced.Restock.RestockListName, i, item);
            }
        }

        internal static void AddList(string newList)
        {
            RazorEnhanced.Settings.Restock.ListInsert(newList, RazorEnhanced.Restock.RestockDelay, 0, 0);

            RazorEnhanced.Restock.RefreshLists();
            RazorEnhanced.Restock.RefreshItems();
        }

        internal static void RemoveList(string list)
        {
            if (RazorEnhanced.Settings.Restock.ListExists(list))
            {
                RazorEnhanced.Settings.Restock.ListDelete(list);
            }

            RazorEnhanced.Restock.RefreshLists();
            RazorEnhanced.Restock.RefreshItems();
        }

        internal static void AddItemToList(string name, int graphics, int amountlimit, int color)
        {
            RestockItem item = new RestockItem(name, graphics, color, amountlimit, false);

            string selection = Assistant.Engine.MainWindow.RestockListSelect.Text;

            if (RazorEnhanced.Settings.Restock.ListExists(selection))
            {
                if (!RazorEnhanced.Settings.Restock.ItemExists(selection, item))
                    RazorEnhanced.Settings.Restock.ItemInsert(selection, item);
            }

            RazorEnhanced.Restock.RefreshItems();
        }

        internal static void ModifyItemInList(string name, int graphics, int color, int amountlimit, bool selected, RestockItem old, int index)
        {
            RestockItem item = new RestockItem(name, graphics, color, amountlimit, selected);

            string selection = Assistant.Engine.MainWindow.RestockListSelect.Text;

            if (RazorEnhanced.Settings.Restock.ListExists(selection))
            {
                if (RazorEnhanced.Settings.Restock.ItemExists(selection, old))
                    RazorEnhanced.Settings.Restock.ItemReplace(selection, index, item);
            }

            RazorEnhanced.Restock.RefreshItems();
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
    }
}
