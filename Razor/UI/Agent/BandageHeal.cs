using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;
using Assistant.UI;


namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal ListBox BandageHealLogBox { get { return bandagehealLogBox; } }
		internal RazorCheckBox BandageHealenableCheckBox { get { return bandagehealenableCheckBox; } }
		internal RazorComboBox BandageHealtargetComboBox { get { return bandagehealtargetComboBox; } }
		internal Label BandageHealtargetLabel { get { return bandagehealtargetLabel; } }
		internal RazorCheckBox BandageHealcustomCheckBox { get { return bandagehealcustomCheckBox; } }
		internal RazorAgentNumHexTextBox BandageHealcustomIDTextBox { get { return bandagehealcustomIDTextBox; } }
		internal RazorAgentNumHexTextBox BandageHealcustomcolorTextBox { get { return bandagehealcustomcolorTextBox; } }
		internal RazorCheckBox BandageHealdexformulaCheckBox { get { return bandagehealdexformulaCheckBox; } }
		internal RazorAgentNumOnlyTextBox BandageHealdelayTextBox { get { return bandagehealdelayTextBox; } }
		internal RazorAgentNumOnlyTextBox BandageHealhpTextBox { get { return bandagehealhpTextBox; } }
        internal RazorAgentNumOnlyTextBox BandageHealMaxRangeTextBox { get { return bandagehealmaxrangeTextBox; } }
        internal RazorCheckBox BandageHealpoisonCheckBox { get { return bandagehealpoisonCheckBox; } }
		internal RazorCheckBox BandageHealmortalCheckBox { get { return bandagehealmortalCheckBox; } }
		internal RazorCheckBox BandageHealhiddedCheckBox { get { return bandagehealhiddedCheckBox; } }
		internal RazorCheckBox BandageHealcountdownCheckBox { get { return bandagehealcountdownCheckBox; } }
		internal RazorCheckBox BandageHealUseText { get { return bandagehealusetext; } }
        internal RazorTextBox BandageHealUseTextContent { get { return bandagehealusetextContent; } }
        internal RazorTextBox BandageHealUseTextSelfContent { get { return bandagehealusetextSelfContent; } }

        internal RazorCheckBox BandageHealUseTarget { get { return bandagehealusetarget; } }
        internal RazorButton BandageHealsettargetButton { get { return bandagehealsettargetButton; } }
		internal RazorCheckBox BandageHealAutostartCheckBox { get { return bandagehealAutostartCheckBox; } }

		private void bandagehealenableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (World.Player != null)
			{
				if (bandagehealenableCheckBox.Checked)
				{
					RazorEnhanced.BandageHeal.AutoMode = true;
					RazorEnhanced.BandageHeal.AddLog("BANDAGE HEAL: Engine Start...");
					if (showagentmessageCheckBox.Checked)
						RazorEnhanced.Misc.SendMessage("BANDAGE HEAL: Engine Start...", false);
				}
				else
				{
					// Stop BANDAGEHEAL
					RazorEnhanced.BandageHeal.AutoMode = false;
					if (showagentmessageCheckBox.Checked)
						RazorEnhanced.Misc.SendMessage("BANDAGE HEAL: Engine Stop...", false);
					RazorEnhanced.BandageHeal.AddLog("BANDAGE HEAL: Engine Stop...");
				}
			}
			else
			{
				bandagehealenableCheckBox.Checked = false;
				RazorEnhanced.BandageHeal.AddLog("You are not logged in game!");
			}

			if (bandagehealenableCheckBox.Checked)
				groupBox6.Enabled = false;
			else
				groupBox6.Enabled = true;
		}
		private void bandagehealAutostartCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealAutostartCheckBox.Focused)
				Settings.General.WriteBool("BandageHealAutostartCheckBox", bandagehealAutostartCheckBox.Checked);
		}
		private void bandagehealtargetComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (BandageHealtargetComboBox.Text == "Target")
			{
				bandagehealsettargetButton.Enabled = true;
				bandagehealtargetLabel.Enabled = true;
			}
			else
			{
				bandagehealsettargetButton.Enabled = false;
				bandagehealtargetLabel.Enabled = false;
			}

			RazorEnhanced.Settings.General.WriteString("BandageHealtargetComboBox", bandagehealtargetComboBox.Text);
		}

		private void bandagehealsettargetButton_Click(object sender, EventArgs e)
		{
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(BandageHeakMobileTarget_Callback));
		}

		private void BandageHeakMobileTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Mobile mobile = Assistant.World.FindMobile(serial);

			if (mobile == null)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Target!", false);
				RazorEnhanced.BandageHeal.AddLog("Invalid Target!");
				return;
			}

			if (mobile.Serial.IsMobile)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Bandage Heal target set to: " + mobile.Name, false);
				RazorEnhanced.BandageHeal.AddLog("Bandage Heal target set to: " + mobile.Name);
				BandageHeal.TargetSerial = mobile.Serial;
				RazorEnhanced.Settings.General.WriteInt("BandageHealtargetLabel", mobile.Serial);
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Target!", false);
				RazorEnhanced.Scavenger.AddLog("Invalid Target!");
			}
		}

		private void bandagehealcustomCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealcustomCheckBox.Checked)
			{
				bandagehealcustomIDTextBox.Enabled = true;
				bandagehealcustomcolorTextBox.Enabled = true;
			}
			else
			{
				bandagehealcustomIDTextBox.Enabled = false;
				bandagehealcustomcolorTextBox.Enabled = false;
			}

			if (bandagehealcustomCheckBox.Focused)
				Settings.General.WriteBool("BandageHealcustomCheckBox", bandagehealcustomCheckBox.Checked);
		}

		private void bandagehealcustomIDTextBox_Leave(object sender, EventArgs e)
		{
			int id = -1;
			try
			{
				id = Convert.ToInt32(bandagehealcustomIDTextBox.Text, 16);
			}
			catch { }

			if (bandagehealcustomIDTextBox.Text == String.Empty || id == -1)
			{
				id = 0;
				bandagehealcustomIDTextBox.Text = "0x0000";
			}

			RazorEnhanced.BandageHeal.CustomID = id;
			RazorEnhanced.Settings.General.WriteInt("BandageHealcustomIDTextBox", id);
		}

		private void bandagehealcustomcolorTextBox_Leave(object sender, EventArgs e)
		{
			int color = -1;
			try
			{
				color = Convert.ToInt32(bandagehealcustomcolorTextBox.Text, 16);
			}
			catch { }

			if (bandagehealcustomcolorTextBox.Text == String.Empty || color == -1)
			{
				color = 0;
				bandagehealcustomcolorTextBox.Text = "0x0000";
			}

			RazorEnhanced.BandageHeal.CustomColor = color;
			RazorEnhanced.Settings.General.WriteInt("BandageHealcustomcolorTextBox", color);
		}

		private void bandagehealdexformulaCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealdexformulaCheckBox.Checked)
				bandagehealdelayTextBox.Enabled = false;
			else
				bandagehealdelayTextBox.Enabled = true;

			if (bandagehealdexformulaCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BandageHealdexformulaCheckBox", bandagehealdexformulaCheckBox.Checked);
		}

		private void bandagehealpoisonCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealpoisonCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BandageHealpoisonCheckBox", bandagehealpoisonCheckBox.Checked);
		}

		private void bandagehealmortalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealmortalCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BandageHealmortalCheckBox", bandagehealmortalCheckBox.Checked);
		}

		private void bandagehealhiddedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealhiddedCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BandageHealhiddedCheckBox", bandagehealhiddedCheckBox.Checked);
		}

		private void bandagehealcountdownCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bandagehealcountdownCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BandageHealcountdownCheckBox", bandagehealcountdownCheckBox.Checked);
		}

		private void bandagehealdelayTextBox_Leave(object sender, EventArgs e)
		{
			if (bandagehealdelayTextBox.Text == String.Empty)
				bandagehealdelayTextBox.Text = "1000";

			BandageHeal.CustomDelay = Convert.ToInt32(bandagehealdelayTextBox.Text);
			Settings.General.WriteInt("BandageHealdelayTextBox", BandageHeal.CustomDelay);
		}

		private void bandagehealhpTextBox_Leave(object sender, EventArgs e)
		{
			if (bandagehealhpTextBox.Text == String.Empty)
				bandagehealhpTextBox.Text = "100";

			BandageHeal.HpLimit = Convert.ToInt32(bandagehealhpTextBox.Text);
			Settings.General.WriteInt("BandageHealhpTextBox", BandageHeal.HpLimit);
		}



        private void bandagehealusetext_CheckedChanged(object sender, EventArgs e)
        {
            if (bandagehealusetext.Focused)
            {
                Settings.General.WriteBool("BandageHealUseText", bandagehealusetext.Checked);
                Engine.MainWindow.SafeAction(s => { s.SetBandSelfState(); });
            }
        }
        private void bandagehealusetext_Content_Leave(object sender, EventArgs e)
        {
            if (bandagehealusetextContent.Text == String.Empty)
                bandagehealusetextContent.Text = "[band";

            Settings.General.WriteString("BandageHealUseTextContent", bandagehealusetextContent.Text);

        }
        private void bandagehealusetextSelf_Content_Leave(object sender, EventArgs e)
        {
            if (bandagehealusetextSelfContent.Text == String.Empty)
                bandagehealusetextSelfContent.Text = "[bandself";

            Settings.General.WriteString("BandageHealUseTextSelfContent", bandagehealusetextSelfContent.Text);

        }

        private void bandagehealusetarget_CheckedChanged(object sender, EventArgs e)
		{
            if (bandagehealusetarget.Focused)
            {
                Settings.General.WriteBool("BandageHealUseTarget", bandagehealusetarget.Checked);
                bandagehealusetext.Enabled = !bandagehealusetarget.Checked;
                BandageHealUseTextSelfContent.Enabled = BandageHealUseTextContent.Enabled = bandagehealusetext.Checked && bandagehealusetext.Enabled;
            }

        }

		private void bandagehealmaxrangeTextBox_Leave(object sender, EventArgs e)
		{
			if (bandagehealmaxrangeTextBox.Text == String.Empty)
				bandagehealmaxrangeTextBox.Text = "1";

			BandageHeal.MaxRange = Convert.ToInt32(bandagehealmaxrangeTextBox.Text);
			Settings.General.WriteInt("BandageHealMaxRangeTextBox", BandageHeal.MaxRange);
		}
	}
}
