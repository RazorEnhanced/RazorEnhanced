using System;
using System.IO;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using RazorEnhanced;

namespace RazorEnhanced
{
	public class Filters
	{
        internal static int BoneCutterBlade
        {
            get
            {
                try
                {
                    int serialblade = Convert.ToInt32(Assistant.Engine.MainWindow.BoneBladeLabel.Text, 16);
                    if (serialblade == 0)
                        return 0;

                    Item blade = RazorEnhanced.Items.FindBySerial(serialblade);
                    if (blade != null && blade.RootContainer == World.Player)
                       return blade.Serial;
                    else
                        return 0;
                }
                catch 
                {
                    return 0;
                }
            }

            set
            {
                Assistant.Engine.MainWindow.BoneBladeLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BoneBladeLabel.Text = "0x" + value.ToString("X8")));
            }
        }

        internal static int AutoCarverBlade
        {
            get
            {
                try
                {
                    int serialblade = Convert.ToInt32(Assistant.Engine.MainWindow.AutoCarverBladeLabel.Text, 16);
                    if (serialblade == 0)
                        return 0;

                    Item blade = RazorEnhanced.Items.FindBySerial(serialblade);
                    if (blade != null && blade.RootContainer == World.Player)
                        return blade.Serial;
                    else
                        return 0;
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                Assistant.Engine.MainWindow.AutoCarverBladeLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoCarverBladeLabel.Text = "0x" + value.ToString("X8")));
            }
        }

        internal static void ProcessMessage(Assistant.Mobile m)
        {
            if (m.Serial == World.Player.Serial)      // Skip Self
                return;

            if (Assistant.Engine.MainWindow.FlagsHighlightCheckBox.Checked)
            {
                if (m.Poisoned)
                    RazorEnhanced.Mobiles.Message(m.Serial, 10, "[Poisoned]");
                if (m.IsGhost)
                    RazorEnhanced.Mobiles.Message(m.Serial, 10, "[Dead]");
            }

            if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
            {
                if (Targeting.IsLastTarget(m))
                    RazorEnhanced.Mobiles.Message(m.Serial, 10, "*[Target]*");
            }

        }

	}
}
