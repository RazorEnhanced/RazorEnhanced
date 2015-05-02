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

        internal static int TargetSerial
        {
            get
            {
                int serial = 0;
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealtargetLabel.Text, out serial)));
                return serial;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetLabel.Text = "0x" + value.ToString("X8")));
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
                int ID = 0;
                try
                {
                   ID = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text, 16);
                }
                catch
                { }
                return ID;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text = "0x" + value.ToString("X4")));
            }
        }
        internal static int CustomColor
        {
            get
            {
                int color = 0;
                try
                {
                    color = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text, 16);
                }
                catch
                { }
                return color;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text = "0x" + value.ToString("X4")));
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
                Assistant.Engine.MainWindow.BandageHealhpTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealhpTextBox.Text = value.ToString()));
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

        internal static void LoadSettings()
        {
            bool BandageHealcountdownCheckBox = false;
            string BandageHealtargetComboBox = "Self";
            int BandageHealtargetLabel = 0;
            bool BandageHealcustomCheckBox = false;
            int BandageHealcustomIDTextBox = 0;
            int BandageHealcustomcolorTextBox = 0;
            bool BandageHealdexformulaCheckBox = false;
            int BandageHealdelayTextBox = 0;
            int BandageHealhpTextBox = 0;
            bool BandageHealpoisonCheckBox = false;
            bool BandageHealmortalCheckBox = false;
            bool BandageHealhiddedCheckBox = false;

            RazorEnhanced.Settings.General.AssistantBandageHealLoadAll(out BandageHealcountdownCheckBox, out BandageHealtargetComboBox, out BandageHealtargetLabel, out BandageHealcustomCheckBox, out BandageHealcustomIDTextBox, out BandageHealcustomcolorTextBox, out BandageHealdexformulaCheckBox, out BandageHealdelayTextBox, out BandageHealhpTextBox, out BandageHealpoisonCheckBox, out BandageHealmortalCheckBox, out BandageHealhiddedCheckBox);
           
            Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Self");
            Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Target");

            ShowCountdown = BandageHealcountdownCheckBox;
            HiddenBlock = BandageHealhiddedCheckBox;
            MortalBlock = BandageHealmortalCheckBox;
            PoisonBlock = BandageHealpoisonCheckBox;
            HpLimit = BandageHealhpTextBox;
            CustomDelay = BandageHealdelayTextBox;
            CustomDexFormula = BandageHealdexformulaCheckBox;
            if (CustomDexFormula)
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = false;
            else
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = true;

            CustomColor = BandageHealcustomcolorTextBox;
            CustomID = BandageHealcustomIDTextBox;
            CustomCheckBox = BandageHealcustomCheckBox;
            if (CustomCheckBox)
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = true;
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = true;
            }
            else
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = false;
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = false;
            }

            TargetSerial = BandageHealtargetLabel;
            TargetType = BandageHealtargetComboBox;
            if (TargetType == "Target")
            {
                Assistant.Engine.MainWindow.BandageHealsettargetButton.Enabled = true;
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Enabled = true;
            }
            else
            {
                Assistant.Engine.MainWindow.BandageHealsettargetButton.Enabled = false;
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Enabled = false;
            }
        }
	}
}
