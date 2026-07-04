using Assistant.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
    public partial class MainForm
    {
        internal void LoadAgentStatusGumpSettings()
        {
            bool enabled = RazorEnhanced.Settings.General.ReadBool("GumpStatusEnabled");
            gumpStatusEnabledCheckBox.Checked = enabled;

            int x = RazorEnhanced.Settings.General.ReadInt("GumpStatusX");
            int y = RazorEnhanced.Settings.General.ReadInt("GumpStatusY");
            gumpStatusXTextBox.Text = x.ToString();
            gumpStatusYTextBox.Text = y.ToString();

            bool vertical = RazorEnhanced.Settings.General.ReadBool("AgentGumpVertical");
            gumpOrientationComboBox.SelectedIndex = vertical ? 1 : 0;

            if (enabled)
                RazorEnhanced.AgentStatusGump.RequestStart();
            else
                RazorEnhanced.AgentStatusGump.RequestStop();
        }

        private void GumpStatusEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = gumpStatusEnabledCheckBox.Checked;
            RazorEnhanced.Settings.General.WriteBool("GumpStatusEnabled", enabled);
            if (enabled)
                RazorEnhanced.AgentStatusGump.RequestStart();
            else
                RazorEnhanced.AgentStatusGump.RequestStop();
        }

        private void GumpStatusXTextBox_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(gumpStatusXTextBox.Text, out int x))
            {
                RazorEnhanced.AgentStatusGump.SetPosition(x,
                    int.TryParse(gumpStatusYTextBox.Text, out int y) ? y : 200);
            }
            else
            {
                gumpStatusXTextBox.Text = RazorEnhanced.Settings.General.ReadInt("GumpStatusX").ToString();
            }
        }

        private void GumpStatusYTextBox_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(gumpStatusYTextBox.Text, out int y))
            {
                RazorEnhanced.AgentStatusGump.SetPosition(
                    int.TryParse(gumpStatusXTextBox.Text, out int x) ? x : 200, y);
            }
            else
            {
                gumpStatusYTextBox.Text = RazorEnhanced.Settings.General.ReadInt("GumpStatusY").ToString();
            }
        }

        private void GumpOrientationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gumpOrientationComboBox.Focused)
            {
                bool vertical = gumpOrientationComboBox.SelectedIndex == 1;
                RazorEnhanced.Settings.General.WriteBool("AgentGumpVertical", vertical);
                RazorEnhanced.AgentStatusGump.Refresh();
            }
        }

        private void GumpPickLocationButton_Click(object sender, EventArgs e)
        {
            RazorEnhanced.AgentStatusGump.PickPosition((x, y) =>
            {
                this.SafeAction(s =>
                {
                    s.gumpStatusXTextBox.Text = x.ToString();
                    s.gumpStatusYTextBox.Text = y.ToString();
                });
            });
        }
    }
}
