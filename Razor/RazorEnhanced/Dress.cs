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
    }
}
