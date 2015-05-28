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
