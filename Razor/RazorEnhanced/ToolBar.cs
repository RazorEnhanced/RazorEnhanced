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

        internal static void UpdateHits(int maxhits, int hits)
        {
            int percent = (int)(hits * 100 / (maxhits == 0 ? (ushort)1 : maxhits));

            Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBarWindows.labelTextHits.Text = "Hits: " + hits.ToString() + " / " + maxhits.ToString();
                Assistant.Engine.MainWindow.ToolBarWindows.labelBarHits.Size = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.ToolBarWindows.labelBarHits.BackColor = GetColor(percent);
            });
        }

        internal static void UpdateStam(int maxstam, int stam)
        {
            int percent = (int)(stam * 100 / (maxstam == 0 ? (ushort)1 : maxstam));

            Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBarWindows.labelTextStamina.Text = "Stam: " + stam.ToString() + " / " + maxstam.ToString();
                Assistant.Engine.MainWindow.ToolBarWindows.labelBarStamina.Size = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.ToolBarWindows.labelBarStamina.BackColor = GetColor(percent);
            });
        }
        internal static void UpdateMana(int maxmana, int mana)
        {
            int percent = (int)(mana * 100 / (maxmana == 0 ? (ushort)1 : maxmana));

            Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBarWindows.labelTextMana.Text = "Mana: " + mana.ToString() + " / " + maxmana.ToString();
                Assistant.Engine.MainWindow.ToolBarWindows.labelBarMana.Size = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.ToolBarWindows.labelBarMana.BackColor = GetColor(percent);
            });
        }

        internal static void UpdateWeight(int maxweight, int weight)
        {
            Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBarWindows.labelWeight.Text = "Weight: " + weight.ToString() + " / " + maxweight.ToString();
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
                    if (Assistant.Engine.MainWindow.ToolBarWindows == null)
                    {
                        Assistant.Engine.MainWindow.ToolBarWindows = new RazorEnhanced.UI.EnhancedToolbar();
                        Assistant.Engine.MainWindow.ToolBarWindows.Show();
                        Assistant.Engine.MainWindow.ToolBarWindows.Location = new System.Drawing.Point(Settings.General.ReadInt("PosXToolBar"), Settings.General.ReadInt("PosYToolBar"));
                    }
                    else
                    {
                        Assistant.Engine.MainWindow.ToolBarWindows.Show();
                        Assistant.Engine.MainWindow.ToolBarWindows.Location = new System.Drawing.Point(Settings.General.ReadInt("PosXToolBar"), Settings.General.ReadInt("PosYToolBar"));
                    }
                    UpdatePanelImage();
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
            if (Assistant.Engine.MainWindow.ToolBarWindows == null)
                return;

            List<RazorEnhanced.ToolBar.ToolBarItem> items = RazorEnhanced.Settings.Toolbar.ReadItems();

            if (items[0].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel1.BackgroundImage = Ultima.Art.GetStatic(items[0].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel1.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel1count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel1.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel1.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel1.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel1.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel1count.Text = "";
            }

            if (items[1].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel2.BackgroundImage = Ultima.Art.GetStatic(items[1].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel2.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel2count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel2.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel2.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel2.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel2.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel2count.Text = "";
            }

            if (items[2].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel3.BackgroundImage = Ultima.Art.GetStatic(items[2].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel3.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel3count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel3.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel3.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel3.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel3.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel3count.Text = "";
            }

            if (items[3].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel4.BackgroundImage = Ultima.Art.GetStatic(items[3].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel4.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel4count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel4.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel4.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel4.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel4.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel4count.Text = "";
            }

            if (items[4].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel5.BackgroundImage = Ultima.Art.GetStatic(items[4].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel5.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel5count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel5.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel5.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel5.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel5.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel5count.Text = "";
            }

            if (items[5].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel6.BackgroundImage = Ultima.Art.GetStatic(items[5].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel6.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel6count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel6.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel6.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel6.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel6.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel6count.Text = "";
            }

            if (items[6].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel7.BackgroundImage = Ultima.Art.GetStatic(items[6].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel7.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel7count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel7.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel7.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel7.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel7.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel7count.Text = "";
            }

            if (items[7].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel8.BackgroundImage = Ultima.Art.GetStatic(items[7].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel8.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel8count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel8.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel8.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel8.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel8.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel8count.Text = "";
            }

            if (items[8].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel9.BackgroundImage = Ultima.Art.GetStatic(items[8].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel9.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel9count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel9.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel9.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel9.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel9.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel9count.Text = "";
            }

            if (items[9].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel10.BackgroundImage = Ultima.Art.GetStatic(items[9].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel10.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel10count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel10.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel10.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel10.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel10.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel10count.Text = "";
            }

            if (items[10].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel11.BackgroundImage = Ultima.Art.GetStatic(items[10].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel11.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel11count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel11.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel11.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel11.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel11.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel11count.Text = "";
            }

            if (items[11].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel12.BackgroundImage = Ultima.Art.GetStatic(items[11].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel12.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel12count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel12.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel12.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel12.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel12.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel12count.Text = "";
            }

            if (items[12].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel13.BackgroundImage = Ultima.Art.GetStatic(items[12].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel13.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel13count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel13.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel13.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel13.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel13.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel13count.Text = "";
            }

            if (items[13].Graphics != 0)
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel14.BackgroundImage = Ultima.Art.GetStatic(items[13].Graphics);
                Assistant.Engine.MainWindow.ToolBarWindows.panel14.Enabled = true;
                Assistant.Engine.MainWindow.ToolBarWindows.panel14count.Text = "0";
                Assistant.Engine.MainWindow.ToolBarWindows.panel14.BackColor = SystemColors.Control;
            }
            else
            {
                Assistant.Engine.MainWindow.ToolBarWindows.panel14.BackgroundImage = null;
                Assistant.Engine.MainWindow.ToolBarWindows.panel14.BackColor = Color.DarkGray;
                Assistant.Engine.MainWindow.ToolBarWindows.panel14.Enabled = false;
                Assistant.Engine.MainWindow.ToolBarWindows.panel14count.Text = "";
            }
        }

        internal static void UpdateAll()
        {
            if (Assistant.World.Player != null && Assistant.Engine.MainWindow.ToolBarWindows != null)
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
            if (Assistant.World.Player != null && Assistant.Engine.MainWindow.ToolBarWindows != null)
            {
                List<RazorEnhanced.ToolBar.ToolBarItem> items = RazorEnhanced.Settings.Toolbar.ReadItems();

                if (items[0].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[0].Graphics, items[0].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel1count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel1count.Text = amount.ToString();
                        if (items[0].Warning)
                        {
                            if (amount <= items[0].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel1.BackColor = Color.Orange;
                                if (amount < oldamount )
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[0].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel1.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[1].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[1].Graphics, items[1].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel2count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel2count.Text = amount.ToString();
                        if (items[1].Warning)
                        {
                            if (amount <= items[1].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel2.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[1].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel2.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[2].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[2].Graphics, items[2].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel3count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel3count.Text = amount.ToString();
                        if (items[2].Warning)
                        {
                            if (amount <= items[2].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel3.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[2].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel3.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[3].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[3].Graphics, items[3].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel4count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel4count.Text = amount.ToString();
                        if (items[3].Warning)
                        {
                            if (amount <= items[3].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel4.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[3].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel4.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[4].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[4].Graphics, items[4].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel5count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel5count.Text = amount.ToString();
                        if (items[4].Warning)
                        {
                            if (amount <= items[4].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel5.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[4].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel5.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[5].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[5].Graphics, items[5].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel6count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel6count.Text = amount.ToString();
                        if (items[5].Warning)
                        {
                            if (amount <= items[5].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel6.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[5].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel6.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[6].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[6].Graphics, items[6].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel7count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel7count.Text = amount.ToString();
                        if (items[6].Warning)
                        {
                            if (amount <= items[6].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel7.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[6].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel7.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[7].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[7].Graphics, items[7].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel8count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel8count.Text = amount.ToString();
                        if (items[7].Warning)
                        {
                            if (amount <= items[7].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel8.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[7].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel8.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[8].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[8].Graphics, items[8].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel9count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel9count.Text = amount.ToString();
                        if (items[8].Warning)
                        {
                            if (amount <= items[8].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel9.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[8].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel9.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[9].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[9].Graphics, items[9].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel10count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel10count.Text = amount.ToString();
                        if (items[9].Warning)
                        {
                            if (amount <= items[9].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel10.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[9].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel10.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[10].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[10].Graphics, items[10].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel11count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel11count.Text = amount.ToString();
                        if (items[10].Warning)
                        {
                            if (amount <= items[10].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel11.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[10].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel11.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[11].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[11].Graphics, items[11].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel12count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel12count.Text = amount.ToString();
                        if (items[11].Warning)
                        {
                            if (amount <= items[11].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel12.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[11].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel12.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[12].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[12].Graphics, items[12].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel13count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel13count.Text = amount.ToString();
                        if (items[12].Warning)
                        {
                            if (amount <= items[12].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel13.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[12].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel13.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
                if (items[13].Graphics != 0)
                {
                    Assistant.Engine.MainWindow.ToolBarWindows.BeginInvoke((MethodInvoker)delegate
                    {
                        int amount = RazorEnhanced.Items.BackpackCount(items[13].Graphics, items[13].Color);
                        int oldamount = Convert.ToInt32(Assistant.Engine.MainWindow.ToolBarWindows.panel14count.Text);
                        Assistant.Engine.MainWindow.ToolBarWindows.panel14count.Text = amount.ToString();
                        if (items[13].Warning)
                        {
                            if (amount <= items[13].WarningLimit)
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel14.BackColor = Color.Orange;
                                if (amount < oldamount)
                                {
                                    RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[13].Name + " under limit left: " + amount.ToString());
                                }
                            }
                            else
                            {
                                Assistant.Engine.MainWindow.ToolBarWindows.panel14.BackColor = SystemColors.Control;
                            }
                        }
                    });
                }
            }
        }


        //////////////// Load settings ////////////////
        internal static void LoadSettings()
        {
            Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Clear();
            bool LockToolBarCheckBox = false;
            bool AutoopenToolBarCheckBox = false;
            int PosXToolBar = 10;
            int PosYToolBar = 10;

            RazorEnhanced.Settings.General.EnhancedToolBarLoadAll(out LockToolBarCheckBox, out AutoopenToolBarCheckBox, out PosXToolBar, out PosYToolBar);

            Assistant.Engine.MainWindow.LockToolBarCheckBox.Checked = LockToolBarCheckBox;
            Assistant.Engine.MainWindow.AutoopenToolBarCheckBox.Checked = AutoopenToolBarCheckBox;
            Assistant.Engine.MainWindow.LocationToolBarLabel.Text = "X: " + PosXToolBar + " - Y:" + PosYToolBar;
            Assistant.Engine.ToolBarX = PosXToolBar;
            Assistant.Engine.ToolBarY = PosYToolBar;

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
