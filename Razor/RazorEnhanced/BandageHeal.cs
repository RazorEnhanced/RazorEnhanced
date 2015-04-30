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
    public class BandageHeal
	{
        internal static void AddLog(string addlog)
        {
            Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Engine.MainWindow.BandageHealLogBox.Items.Add(addlog)));
            Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Engine.MainWindow.BandageHealLogBox.SelectedIndex = Engine.MainWindow.BandageHealLogBox.Items.Count - 1));
            if (Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count > 300)
                Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Clear()));
        }
        internal static string TargetType
        {
            get
            {
                return (string)Assistant.Engine.MainWindow.BandageHealtargetComboBox.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealtargetComboBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text = value));
            }
        }

        internal static string TargetSerial
        {
            get
            {
                return (string)Assistant.Engine.MainWindow.BandageHealtargetLabel.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.BandageHealtargetLabel.Text));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetLabel.Text = value));
            }
        }
        internal static bool CustomCheckBox 
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Checked = value));
            }
        }

        internal static int CustomID
        {
            get
            {
                int ID = 0x0001;
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text, out ID)));
                return ID;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text = value.ToString("X8")));
            }
        }
        internal static int CustomColor
        {
            get
            {
                int ID = 0x0001;
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text, out ID)));
                return ID;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text = value.ToString("X8")));
            }
        }
        internal static int CustomDelay
        {
            get
            {
                int delay = 1000;
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealdelayTextBox.Text, out delay)));
                return delay;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealdelayTextBox.Text = value.ToString()));
            }
        }

        internal static bool CustomDexFormula
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Checked = value));
            }
        }

        internal static int HpLimit
        {
            get
            {
                int hplimit = 100;
                Assistant.Engine.MainWindow.BandageHealhpTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealhpTextBox.Text, out hplimit)));
                if (hplimit > 100)
                {
                    HpLimit = 100;
                    return hplimit;
                }
                else
                    return hplimit;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text = value.ToString("X8")));
            }
        }

        internal static bool PoisonBlock
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Checked = value));
            }
        }
        internal static bool MortalBlock
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Checked = value));
            }
        }
        internal static bool HiddenBlock
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Checked = value));
            }
        }

        internal static bool ShowCountdown
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Checked = value));
            }
        }

	}
}
