using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assistant;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace RazorEnhanced
{
    public class ToolBar
    {
        [Serializable]
        public class ToolBarItem
        {
            private string m_Name;
            public string Name { get { return m_Name; } }

            private int m_Graphics;
            public int Graphics { get { return m_Graphics; } }

            private int m_Color;
            public int Color { get { return m_Color; } }

            private bool m_Warning;
            internal bool Warning { get { return m_Warning; } }

            private int m_WarningLimit;
            public int WarningLimit { get { return m_WarningLimit; } }

            public ToolBarItem(string name, int graphics, int color, bool warning, int warninglimit)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_Color = color;
                m_Warning = warning;
                m_WarningLimit = warninglimit;
            }
        }

        internal static List<RazorEnhanced.ToolBar.ToolBarItem> ItemCache;
        internal static void UpdateItemCache()
        {
            ItemCache = RazorEnhanced.Settings.Toolbar.ReadItems();
        }

        internal static void UpdateHits(int maxhits, int hits)
        {
            int percent = (int)(hits * 100 / (maxhits == 0 ? (ushort)1 : maxhits));

            Assistant.Engine.MainWindow.enhancedToolbar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.enhancedToolbar.labelTextHits.Text = "Hits: " + maxhits.ToString() + " / " + hits.ToString();
                Assistant.Engine.MainWindow.enhancedToolbar.labelBarHits.Size = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.enhancedToolbar.labelBarHits.BackColor = GetColor(percent);
            });
        }

        internal static void UpdateStam(int maxstam, int stam)
        {
            int percent = (int)(stam * 100 / (maxstam == 0 ? (ushort)1 : maxstam));

            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.enhancedToolbar.labelTextStamina.Text = "Stam: " + maxstam.ToString() + " / " + stam.ToString();
                Assistant.Engine.MainWindow.enhancedToolbar.labelBarStamina.Size = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.enhancedToolbar.labelBarStamina.BackColor = GetColor(percent);
            });
        }
        internal static void UpdateMana(int maxmana, int mana)
        {
            int percent = (int)(mana * 100 / (maxmana == 0 ? (ushort)1 : maxmana));

            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.enhancedToolbar.labelTextMana.Text = "Mana: " + maxmana.ToString() + " / " + mana.ToString();
                Assistant.Engine.MainWindow.enhancedToolbar.labelBarMana.Size = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.enhancedToolbar.labelBarMana.BackColor = GetColor(percent);
            });
        }

        internal static void UpdateWeight(int maxweight, int weight)
        {
            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.enhancedToolbar.labelWeight.Text = "Weight: " + weight.ToString() + " / " + maxweight.ToString();
            });
        }

        private static Color GetColor(int percent)
        {
            if (percent <= 10)
                return Color.DarkViolet;
            else if (percent > 10 && percent <= 30)
                return Color.DarkRed;
            else if (percent > 30 && percent <= 50)
                return Color.DarkOrange;
            else if (percent > 50 && percent <= 70)
                return Color.Goldenrod;
            else if (percent > 70 && percent <= 90)
                return Color.Gold;
            else
                return Color.ForestGreen;
        }

        internal static void Open()
        {
            if (Assistant.World.Player != null)
            {
                if (Assistant.Engine.MainWindow.ToolBar == null)
                {
                    Assistant.Engine.MainWindow.ToolBar = new RazorEnhanced.UI.EnhancedToolbar();
                    Assistant.Engine.MainWindow.ToolBar.Show();
                    Assistant.Engine.MainWindow.ToolBar.Location = new System.Drawing.Point(Settings.General.ReadInt("PosXToolBar"), Settings.General.ReadInt("PosYToolBar"));
                }
                else
                {
                    Assistant.Engine.MainWindow.ToolBar.Show();
                    Assistant.Engine.MainWindow.ToolBar.Location = new System.Drawing.Point(Settings.General.ReadInt("PosXToolBar"), Settings.General.ReadInt("PosYToolBar"));
                }
                UpdatePanelImage();
                UpdateItemCache();
            }
        }

        internal static void UptateToolBarComboBox(int index)
        {
            List<RazorEnhanced.ToolBar.ToolBarItem> items = RazorEnhanced.Settings.Toolbar.ReadItems();
            Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Clear();
            int i = 0;
            foreach (RazorEnhanced.ToolBar.ToolBarItem item in items)
            {
                Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Add("Slot " + i + ": " + item.Name);
                i++;
            }
            Assistant.Engine.MainWindow.ToolBoxCountComboBox.SelectedIndex = index;
        }

        internal static void UpdatePanelImage()
        {
            List<RazorEnhanced.ToolBar.ToolBarItem> items = RazorEnhanced.Settings.Toolbar.ReadItems();

            if (items[0].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel1.BackgroundImage = Ultima.Art.GetStatic(items[0].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel1.Enabled = true;
            }
            else
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel1.BackgroundImage = null;
                Assistant.Engine.MainWindow.enhancedToolbar.panel1.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.enhancedToolbar.panel1.Enabled = false;
            }

            if (items[1].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel2.BackgroundImage = Ultima.Art.GetStatic(items[1].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel2.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel2.Enabled = false;

            if (items[2].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel3.BackgroundImage = Ultima.Art.GetStatic(items[2].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel3.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel3.Enabled = false;

            if (items[3].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel4.BackgroundImage = Ultima.Art.GetStatic(items[3].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel4.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel4.Enabled = false;

            if (items[4].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel5.BackgroundImage = Ultima.Art.GetStatic(items[4].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel5.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel5.Enabled = false;

            if (items[5].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel6.BackgroundImage = Ultima.Art.GetStatic(items[5].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel6.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel6.Enabled = false;

            if (items[6].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel7.BackgroundImage = Ultima.Art.GetStatic(items[6].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel7.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel7.Enabled = false;

            if (items[7].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel8.BackgroundImage = Ultima.Art.GetStatic(items[7].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel8.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel8.Enabled = false;

            if (items[8].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel9.BackgroundImage = Ultima.Art.GetStatic(items[8].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel9.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel9.Enabled = false;

            if (items[9].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel10.BackgroundImage = Ultima.Art.GetStatic(items[9].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel10.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel10.Enabled = false;

            if (items[10].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel11.BackgroundImage = Ultima.Art.GetStatic(items[10].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel11.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel11.Enabled = false;

            if (items[11].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel12.BackgroundImage = Ultima.Art.GetStatic(items[11].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel12.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel12.Enabled = false;

            if (items[12].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel13.BackgroundImage = Ultima.Art.GetStatic(items[12].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel13.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel13.Enabled = false;

            if (items[13].Graphics != 0)
            {
                Assistant.Engine.MainWindow.enhancedToolbar.panel14.BackgroundImage = Ultima.Art.GetStatic(items[13].Graphics);
                Assistant.Engine.MainWindow.enhancedToolbar.panel14.Enabled = true;
            }
            else
                Assistant.Engine.MainWindow.enhancedToolbar.panel14.Enabled = false;
        }

        internal static void UpdateAll()
        {
            if (Assistant.World.Player != null && Assistant.Engine.MainWindow.ToolBarOpen)
            {
                RazorEnhanced.ToolBar.UpdateHits(Assistant.World.Player.HitsMax, Assistant.World.Player.Hits);
                RazorEnhanced.ToolBar.UpdateStam(Assistant.World.Player.StamMax, Assistant.World.Player.Stam);
                RazorEnhanced.ToolBar.UpdateMana(Assistant.World.Player.ManaMax, Assistant.World.Player.Mana);
                RazorEnhanced.ToolBar.UpdateWeight(Assistant.World.Player.MaxWeight, Assistant.World.Player.Weight);               
            }

            UpdateCount();

        }

        internal static void UpdateCount()
        {
            if (Assistant.World.Player != null && Assistant.Engine.MainWindow.ToolBarOpen)
            {
                List<RazorEnhanced.ToolBar.ToolBarItem> items = ItemCache;

                if (items[0].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
                    {
                        Assistant.Engine.MainWindow.enhancedToolbar.panel1count.Text = RazorEnhanced.Items.ContainerCount(World.Player.Backpack.Serial, items[0].Graphics, items[0].Color).ToString();
                        Assistant.Engine.MainWindow.enhancedToolbar.panel2count.Text = RazorEnhanced.Items.ContainerCount(World.Player.Backpack.Serial, items[1].Graphics, items[1].Color).ToString();
                    });
                }
            }
        }


        //////////////// Load settings ////////////////
        internal static void LoadSettings()
        {
            bool LockToolBarCheckBox = false;
            bool AutoopenToolBarCheckBox = false;
            int PosXToolBar = 10;
            int PosYToolBar = 10;

            RazorEnhanced.Settings.General.EnhancedToolBarLoadAll(out LockToolBarCheckBox, out AutoopenToolBarCheckBox, out PosXToolBar, out PosYToolBar);

            Assistant.Engine.MainWindow.LockToolBarCheckBox.Checked = LockToolBarCheckBox;
            Assistant.Engine.MainWindow.AutoopenToolBarCheckBox.Checked = AutoopenToolBarCheckBox;
            Assistant.Engine.MainWindow.LocationToolBarLabel.Text = "X: " + PosXToolBar + " - Y:" + PosYToolBar;

            List<RazorEnhanced.ToolBar.ToolBarItem> items = RazorEnhanced.Settings.Toolbar.ReadItems();

            int i = 0;
            foreach (RazorEnhanced.ToolBar.ToolBarItem item in items)
            {
                Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Add("Slot " + i + ": " + item.Name);
                i++;
            }
            Assistant.Engine.MainWindow.ToolBoxCountComboBox.SelectedIndex = 0;
        }
    }   
}
