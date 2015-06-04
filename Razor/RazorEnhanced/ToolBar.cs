using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assistant;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
    public class ToolBar
    {
        internal static void UpdateHits(int maxhits, int hits)
        {
            int percent = (int)(hits * 100 / (maxhits == 0 ? (ushort)1 : maxhits));

            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate {
                Assistant.Engine.MainWindow.ToolBar.LabelTextHits = "Hits: " + maxhits.ToString() + " / " + hits.ToString();
                Assistant.Engine.MainWindow.ToolBar.LabelBarHitsSize = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.ToolBar.LabelBarHitsColor = GetColor(percent);
            });
        }

        internal static void UpdateStam(int maxstam, int stam)
        {
            int percent = (int)(stam * 100 / (maxstam == 0 ? (ushort)1 : maxstam));

            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBar.LabelTextStam = "Stam: " + maxstam.ToString() + " / " + stam.ToString();
                Assistant.Engine.MainWindow.ToolBar.LabelBarStamSize = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.ToolBar.LabelBarStamColor = GetColor(percent);
            });
        }
        internal static void UpdateMana(int maxmana, int mana)
        {
            int percent = (int)(mana * 100 / (maxmana == 0 ? (ushort)1 : maxmana));

            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBar.LabelTextMana = "Mana: " + maxmana.ToString() + " / " + mana.ToString();
                Assistant.Engine.MainWindow.ToolBar.LabelBarManaSize = new System.Drawing.Size(percent, 10);
                Assistant.Engine.MainWindow.ToolBar.LabelBarManaColor = GetColor(percent);
            });
        }

        internal static void UpdateWeight(int maxweight, int weight)
        {
            Assistant.Engine.MainWindow.ToolBar.BeginInvoke((MethodInvoker)delegate
            {
                Assistant.Engine.MainWindow.ToolBar.LabelWeight = "Weight: " + weight.ToString() + " / " + maxweight.ToString();
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
                    Assistant.Engine.MainWindow.ToolBar.Location = new System.Drawing.Point(Settings.General.ReadInt("PosXToolBar"), Settings.General.ReadInt("PosYToolBar"));
                    Assistant.Engine.MainWindow.ToolBar.Show();
                }
                else
                {
                    Assistant.Engine.MainWindow.ToolBar.Show();
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
        }
    }   
}
