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
                    ListViewItem listitem = new ListViewItem();

                    foreach (DressItem item in items)
                    {
                        if (item.Layer == 0)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("RightHand");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 1)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("LeftHand");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 2)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Shoes");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 3)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Pants");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 4)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Shirt");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 5)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Head");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 6)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Gloves");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 7)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Ring");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 8)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Neck");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 9)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Waist");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 10)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("InnerTorso");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 11)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Bracelet");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 12)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("MiddleTorso");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 13)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Earrings");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 14)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Arms");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 15)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("Cloak");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 16)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("OuterTorso");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 17)
                        {
                            listitem.Checked = item.Selected;
                            listitem.SubItems.Add("OuterLegs");
                            listitem.SubItems.Add(item.Name);
                            listitem.SubItems.Add("0x" + item.Serial.ToString("X8"));
                            Assistant.Engine.MainWindow.DressListView.Items.Add(listitem);
                            continue;
                        }
                        if (item.Layer == 18)
                        {
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

    }
}
