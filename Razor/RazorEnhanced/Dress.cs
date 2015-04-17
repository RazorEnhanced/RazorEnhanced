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
    public class Dress
    {
        [Serializable]
        public class DressItem
        {
            private int m_Layer;
            public int Layer { get { return m_Layer; } }

            private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_serial;
            public int Serial { get { return m_serial; } }

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
            Engine.MainWindow.DressLogBox.Invoke(new Action(() => Engine.MainWindow.DressLogBox.Items.Add(addlog)));
            Engine.MainWindow.DressLogBox.Invoke(new Action(() => Engine.MainWindow.DressLogBox.SelectedIndex = Engine.MainWindow.DressLogBox.Items.Count - 1));
            if (Assistant.Engine.MainWindow.DressLogBox.Items.Count > 300)
                Assistant.Engine.MainWindow.DressLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.DressLogBox.Items.Clear()));
        }

        internal static int DressDelay
        {
            get
            {
                int delay = 100;
                Assistant.Engine.MainWindow.DressDragDelay.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.DressDragDelay.Text, out delay)));
                return delay;
            }
            set
            {
                Assistant.Engine.MainWindow.DressDragDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.DressDragDelay.Text = value.ToString()));
            }
        }

        internal static int DressBag
        {
            get
            {
                int serialBag = 0;

                try
                {
                    serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.DressBagLabel.Text, 16);

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
                Assistant.Engine.MainWindow.DressBagLabel.Text = "0x" + value.ToString("X8");
            }
        }
        internal static bool DressConflict
        {
            get
            {
                return Assistant.Engine.MainWindow.DressCheckBox.Checked;
            }
            set
            {
                Assistant.Engine.MainWindow.DressCheckBox.Checked = value;
            }
        }
        internal static string DressListName
        {
            get
            {
                return (string)Assistant.Engine.MainWindow.DressListSelect.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.DressListSelect.Text));
            }

            set
            {
                Assistant.Engine.MainWindow.DressListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.DressListSelect.Text = value));
            }
        }

        internal static void RefreshLists()
        {
            List<DressList> lists;
            RazorEnhanced.Settings.Dress.ListsRead(out lists);

            DressList selectedList = lists.Where(l => l.Selected).FirstOrDefault();
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

        internal static void UpdateSelectedItems()
        {
            List<DressItem> items;
            RazorEnhanced.Settings.Dress.ItemsRead(DressListName, out items);

            if (items.Count != Assistant.Engine.MainWindow.DressListView.Items.Count)
            {
                return;
            }

            for (int i = 0; i < Assistant.Engine.MainWindow.DressListView.Items.Count; i++)
            {
                ListViewItem lvi = Assistant.Engine.MainWindow.DressListView.Items[i];
                DressItem old = items[i];

                if (lvi != null && old != null)
                {
                    DressItem item = new Dress.DressItem(old.Name, old.Layer, old.Serial, lvi.Checked);
                    RazorEnhanced.Settings.Dress.ItemReplace(RazorEnhanced.Dress.DressListName, i, item);
                }
            }
        }

        internal static void RefreshItems()
        {
            List<DressList> lists;
            RazorEnhanced.Settings.Dress.ListsRead(out lists);

            Assistant.Engine.MainWindow.DressListView.Items.Clear();
            foreach (DressList l in lists)
            {
                if (l.Selected)
                {
                    List<Dress.DressItem> items;
                    RazorEnhanced.Settings.Dress.ItemsRead(l.Description, out items);
                    

                    foreach (DressItem item in items)
                    {
                        if (item.Layer == 0)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("RightHand");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 1)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("LeftHand");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 2)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Shoes");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 3)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Pants");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 4)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Shirt");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 5)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Head");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 6)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Gloves");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 7)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Ring");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 8)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Neck");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 9)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Waist");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 10)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("InnerTorso");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 11)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Bracelet");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 12)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("MiddleTorso");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 13)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Earrings");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 14)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Arms");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 15)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Cloak");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 16)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("OuterTorso");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 17)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("OuterLegs");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 18)
                        {
                            ListViewItem listitem = new ListViewItem();
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("InnerLegs");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                    }        
                }
            }
        }

        internal static void ReadPlayerDress()
        {
            RazorEnhanced.Settings.Dress.ItemClear(Assistant.Engine.MainWindow.DressListSelect.Text);

            Assistant.Item layeritem = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 0, layeritem.Serial,true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 1, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Shoes);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 2, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Pants);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 3, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Shirt);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 4, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Head);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 5, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Gloves);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 6, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Ring);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 7, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Neck);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 8, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Waist);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 9, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.InnerTorso);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 10, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Bracelet);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 11, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.MiddleTorso);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 12, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Earrings);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 13, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Arms);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 14, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Cloak);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 15, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.OuterTorso);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 16, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.OuterLegs);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 17, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.InnerLegs);
            if (layeritem != null)
            {
                RazorEnhanced.Dress.DressItem itemtoinsert = new DressItem(layeritem.Name, 18, layeritem.Serial, true);
                RazorEnhanced.Settings.Dress.ItemInsert(Assistant.Engine.MainWindow.DressListSelect.Text, itemtoinsert);
            }

            RazorEnhanced.Dress.RefreshItems();
        }
    }
}
