using Assistant;
using Assistant.UI;
using IronPython.Runtime.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;
using static IronPython.Modules.PythonWeakRef;

namespace RazorEnhanced.UI
{
    public partial class EnhancedGridContainerViewer: Form
    {
        private List<Thread> PropThreads = new List<Thread>();
        private readonly Assistant.Item m_itemTarg;

        internal EnhancedGridContainerViewer(Assistant.Item itemTarg)
        {
            InitializeComponent();
            MaximizeBox = false;
            m_itemTarg = itemTarg;
        }

        private void EnhancedGridContainerViewer_Load(object sender, EventArgs e)
        {
            if (m_itemTarg == null)
                this.Close();

            btnRefresh_Click(btnRefresh, null);
        }

        private void EnhancedGridContainerViewer_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = false;
        }


        private void EnhancedGridContainerViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Thread th in PropThreads)
            {
                try
                {
                    th.Abort();
                }
                catch { }
            }

        }

        private void PropertyLoadThread(object obj)
        {
            var tuple = (Tuple<Assistant.Item, ListViewItem>)obj;

            Assistant.Item childitem = tuple.Item1;
            ListViewItem vi = tuple.Item2;

            Items.WaitForProps(childitem.Serial, 1000);

            if (childitem.ObjPropList.Content.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < childitem.ObjPropList.Content.Count; i++)
                {
                    Assistant.ObjectPropertyList.OPLEntry ent = childitem.ObjPropList.Content[i];
                    sb.AppendLine(ent.ToString());
                }
                vi.SubItems[1].Text = sb.ToString();
            }

            PropThreads.Remove(Thread.CurrentThread);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            PropThreads.Clear();

            foreach (var childitem in m_itemTarg.Contains)
            {
                ListViewItem vi = new ListViewItem(childitem.Name.ToString());
                // Immagine
                Bitmap m_childitemimage = Ultima.Art.GetStatic(childitem.TypeID);
                {
                    if (m_childitemimage != null && childitem.Hue > 0)
                    {
                        int hue = childitem.Hue;
                        bool onlyHueGrayPixels = (hue & 0x8000) != 0;
                        hue = (hue & 0x3FFF) - 1;
                        Ultima.Hue m_hue = Ultima.Hues.GetHue(hue);
                        m_hue.ApplyTo(m_childitemimage, onlyHueGrayPixels);
                    }
                    
                    imageList1.Images.Add(childitem.Serial.ToString(), m_childitemimage);
                    vi.ImageKey = childitem.Serial.ToString();
                }
                vi.SubItems.Add("");
                vi.SubItems.Add(childitem.Serial.ToString());
                vi.SubItems.Add(childitem.TypeID.ToString());
                vi.SubItems.Add(childitem.Hue.ToString());
                vi.SubItems.Add(childitem.Amount.ToString());

                // Attributes
                Thread PropertyThread = new Thread(PropertyLoadThread);
                PropertyThread.Start(System.Tuple.Create(childitem, vi));
                PropThreads.Add(PropertyThread);

                listView1.Items.Add(vi);
            }

            toolStripStatusLabel1.Text = m_itemTarg.Contains.Count.ToString();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.FocusedItem == null)
                return;

            string serial = listView1.FocusedItem.SubItems[2].Text;
            Assistant.Item assistantItem = Assistant.World.FindItem(Serial.Parse(serial));

            EnhancedItemInspector enhancedItemInspector = new(assistantItem);
            enhancedItemInspector.Show();
            enhancedItemInspector.BringToFront();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            listBoxAttributes.Items.Clear();

            toolStripStatusLabel3.Text = listView1.SelectedItems.Count.ToString();

            int sum = 0;
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (int.TryParse(item.SubItems[5].Text, out int value))
                {
                    sum += value;
                }
            }
            toolStripStatusLabel5.Text = sum.ToString();

            if (e.Item.Selected)
            {
                string props = e.Item.SubItems[1].Text;
                foreach (string prop in props.Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    listBoxAttributes.Items.Add(prop);
                }
            }
        }

        private void btnSortName_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem)?.Tag?.ToString() != "ASC")
            {
                (sender as ToolStripMenuItem).Tag = "ASC";
                listView1.ListViewItemSorter = new ListViewItemComparer(0, SortOrder.Ascending);
            }
            else
            {
                (sender as ToolStripMenuItem).Tag = "DESC";
                listView1.ListViewItemSorter = new ListViewItemComparer(0, SortOrder.Descending);
            }
        }

        private void btnSortSerial_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem)?.Tag?.ToString() != "ASC")
            {
                (sender as ToolStripMenuItem).Tag = "ASC";
                listView1.ListViewItemSorter = new ListViewItemComparer(2, SortOrder.Ascending);
            }
            else
            {
                (sender as ToolStripMenuItem).Tag = "DESC";
                listView1.ListViewItemSorter = new ListViewItemComparer(2, SortOrder.Descending);
            }
        }

        private void btnSortType_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem)?.Tag?.ToString() != "ASC")
            {
                (sender as ToolStripMenuItem).Tag = "ASC";
                listView1.ListViewItemSorter = new ListViewItemComparer(3, SortOrder.Ascending);
            }
            else
            {
                (sender as ToolStripMenuItem).Tag = "DESC";
                listView1.ListViewItemSorter = new ListViewItemComparer(3, SortOrder.Descending);
            }
        }

        private void btnSortHue_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem)?.Tag?.ToString() != "ASC")
            {
                (sender as ToolStripMenuItem).Tag = "ASC";
                listView1.ListViewItemSorter = new ListViewItemComparer(4, SortOrder.Ascending);
            }
            else
            {
                (sender as ToolStripMenuItem).Tag = "DESC";
                listView1.ListViewItemSorter = new ListViewItemComparer(4, SortOrder.Descending);
            }
        }

        private void btnSortAmount_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem)?.Tag?.ToString() != "ASC")
            {
                (sender as ToolStripMenuItem).Tag = "ASC";
                listView1.ListViewItemSorter = new ListViewItemComparer(5, SortOrder.Ascending);
            }
            else
            {
                (sender as ToolStripMenuItem).Tag = "DESC";
                listView1.ListViewItemSorter = new ListViewItemComparer(5, SortOrder.Descending);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void listBoxAttributes_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) 
                return;

            e.DrawBackground();

            int spacing = 5;

            string text = listBoxAttributes.Items[e.Index].ToString();
            e.Graphics.DrawString(text, e.Font, Brushes.White, e.Bounds.Left, e.Bounds.Top + spacing);

            e.DrawFocusRectangle();
        }

        private void useItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem == null)
                return;

            string serial = listView1.FocusedItem.SubItems[2].Text;
            RazorEnhanced.Items.UseItem(Serial.Parse(serial));
        }

        private void moveToBackpackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                int serial = int.Parse(item.SubItems[2].Text);
                RazorEnhanced.Items.Move(serial, World.Player.Backpack.Serial, -1);
            }
        }

        private void moveToContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback((loc, serial, pt, itemid) =>
            {
                if (loc)
                {
                }
                else
                {
                    Assistant.Item assistantItem = Assistant.World.FindItem(serial);
                    if (assistantItem != null && assistantItem.Serial.IsItem && assistantItem.IsContainer)
                    {
                        Engine.MainWindow.SafeAction(s =>
                        {
                            foreach (ListViewItem item in listView1.SelectedItems)
                            {
                                int serial = int.Parse(item.SubItems[2].Text);
                                RazorEnhanced.Items.Move(serial, assistantItem.Serial, -1);
                            }
                        });
                    }
                }
            }));
        }

        private void moveToBankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                int serial = int.Parse(item.SubItems[2].Text);
                RazorEnhanced.Items.Move(serial, Player.Bank.Serial, -1);
            }
        }

        private void moveToGroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback((loc, serial, pt, itemid) =>
            {
                if (loc)
                {
                    Engine.MainWindow.SafeAction(s =>
                    {
                        foreach (ListViewItem item in listView1.SelectedItems)
                        {
                            int serial = int.Parse(item.SubItems[2].Text);
                            RazorEnhanced.Items.MoveOnGround(serial, -1, pt.X, pt.Y, pt.Z);
                        }
                    });
                }
            }));
        }

    }

    // Add this class definition at the end of the file or in a separate file
    public class ListViewItemComparer : IComparer
    {
        private int columnIndex;
        private SortOrder sortOrder;

        public ListViewItemComparer(int columnIndex, SortOrder sortOrder)
        {
            this.columnIndex = columnIndex;
            this.sortOrder = sortOrder;
        }

        public int Compare(object x, object y)
        {
            ListViewItem itemX = (ListViewItem)x;
            ListViewItem itemY = (ListViewItem)y;

            // 비교할 컬럼의 텍스트 가져오기
            string textX = itemX.SubItems[columnIndex].Text;
            string textY = itemY.SubItems[columnIndex].Text;

            // 오름차순 또는 내림차순에 따라 비교
            int result = string.Compare(textX, textY);

            // 내림차순이면 비교 결과 반전
            if (sortOrder == SortOrder.Descending)
            {
                result = -result;
            }

            return result;
        }
    }
}
