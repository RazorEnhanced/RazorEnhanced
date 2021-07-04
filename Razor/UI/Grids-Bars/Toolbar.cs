using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal Label LocationToolBarLabel { get { return locationToolBarLabel; } }
		internal RazorCheckBox LockToolBarCheckBox { get { return lockToolBarCheckBox; } }
		internal RazorCheckBox AutoopenToolBarCheckBox { get { return autoopenToolBarCheckBox; } }
		internal RazorComboBox ToolBoxCountComboBox { get { return toolboxcountComboBox; } }
		internal RazorComboBox ToolBoxStyleComboBox { get { return toolboxstyleComboBox; } }
		internal RazorComboBox ToolBoxSizeComboBox { get { return toolboxsizeComboBox; } }
		internal RazorCheckBox ShowHitsToolBarCheckBox { get { return showhitsToolBarCheckBox; } }
        internal RazorCheckBox ShowTitheToolBarCheckBox { get { return showtitheToolBarCheckBox; } }

        internal RazorCheckBox ShowStaminaToolBarCheckBox { get { return showstaminaToolBarCheckBox; } }
		internal RazorCheckBox ShowManaToolBarCheckBox { get { return showmanaToolBarCheckBox; } }
		internal RazorCheckBox ShowWeightToolBarCheckBox { get { return showweightToolBarCheckBox; } }
		internal RazorCheckBox ShowFollowerToolBarCheckBox { get { return showfollowerToolBarCheckBox; } }
		internal Label ToolBoxSlotsLabel { get { return toolbarslot_label; } }

		// ---------------- TOOLBAR START ----------------
		private void openToolBarButton_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ToolBar.Open();
		}

		private void closeToolBarButton_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ToolBar.Close();
		}

		private void lockToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (lockToolBarCheckBox.Focused)
			{
				RazorEnhanced.ToolBar.LockUnlock();
				RazorEnhanced.Settings.General.WriteBool("LockToolBarCheckBox", lockToolBarCheckBox.Checked);
				if (RazorEnhanced.ToolBar.ToolBarForm != null)
					RazorEnhanced.ToolBar.ToolBarForm.Show();
			}
		}

		private void autoopenToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (autoopenToolBarCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("AutoopenToolBarCheckBox", autoopenToolBarCheckBox.Checked);
		}

		private void toolboxcountComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (toolboxcomboupdate)
				return;

			int index = toolboxcountComboBox.SelectedIndex;
			RazorEnhanced.ToolBar.ToolBarItem item = RazorEnhanced.Settings.Toolbar.ReadSelectedItem(index);

			toolboxcountNameTextBox.Text = item.Name;
			toolboxcountHueTextBox.Text = "0x" + item.Color.ToString("X4");
			toolboxcountGraphTextBox.Text = "0x" + item.Graphics.ToString("X4");
			toolboxcountHueWarningCheckBox.Checked = item.Warning;
			toolboxcountWarningTextBox.Text = item.WarningLimit.ToString();

			if (toolboxcountComboBox.Focused)
				RazorEnhanced.ToolBar.Open();
		}

		private bool toolboxcomboupdate = false;
		private void toolboxcountNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (toolboxcountNameTextBox.Focused)
			{
				toolboxcomboupdate = true;
				if (toolboxcountComboBox.SelectedIndex != -1)
					toolboxcountComboBox.Items[toolboxcountComboBox.SelectedIndex] = "Slot " + toolboxcountComboBox.SelectedIndex + ": " + toolboxcountNameTextBox.Text;
				toolboxcomboupdate = false;
			}
		}
		private void toolboxcountNameTextBox_Leave(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
			RazorEnhanced.ToolBar.Open();
		}

		private void toolboxcountGraphTextBox_TextChanged(object sender, EventArgs e)
		{
			if (toolboxcountGraphTextBox.Focused)
			{
				RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
				RazorEnhanced.ToolBar.UpdatePanelImage();
				RazorEnhanced.ToolBar.UpdateCount();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void toolboxcountHueTextBox_TextChanged(object sender, EventArgs e)
		{
			if (toolboxcountHueTextBox.Focused)
			{
				RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
				RazorEnhanced.ToolBar.UpdatePanelImage();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void toolboxcountHueWarningCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (toolboxcountHueWarningCheckBox.Focused)
				RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
		}

		private void toolboxcountWarningTextBox_TextChanged(object sender, EventArgs e)
		{
			if (toolboxcountWarningTextBox.Focused)
				RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
		}

		private void toolboxcountClearButton_Click(object sender, EventArgs e)
		{
			int index = toolboxcountComboBox.SelectedIndex;
			toolboxcountNameTextBox.Text = "Empty";
			toolboxcountGraphTextBox.Text = "0x0000";
			toolboxcountHueTextBox.Text = "0x0000";
			toolboxcountHueWarningCheckBox.Checked = false;
			toolboxcountWarningTextBox.Text = "0";
			RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
			RazorEnhanced.ToolBar.UptateToolBarComboBox(index);
			RazorEnhanced.ToolBar.UpdatePanelImage();
			RazorEnhanced.ToolBar.UpdateCount();
		}

		private void toolboxcountTargetButton_Click(object sender, EventArgs e)
		{
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(ToolBarItemTarget_Callback));
		}

		private void ToolBarItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			this.Invoke((MethodInvoker)delegate
			{
				int index = toolboxcountComboBox.SelectedIndex;
				Assistant.Item item = Assistant.World.FindItem(serial);

				if (item == null)
					return;

				if (item.Serial.IsItem)
				{
					toolboxcountNameTextBox.Text = item.Name;
					int itemgraph = item.ItemID;
					toolboxcountGraphTextBox.Text = itemgraph.ToString("X4");
					toolboxcountHueTextBox.Text = item.Hue.ToString("X4");
					RazorEnhanced.Settings.Toolbar.UpdateItem(toolboxcountComboBox.SelectedIndex, toolboxcountNameTextBox.Text, toolboxcountGraphTextBox.Text, toolboxcountHueTextBox.Text, toolboxcountHueWarningCheckBox.Checked, toolboxcountWarningTextBox.Text);
					RazorEnhanced.ToolBar.UptateToolBarComboBox(index);
					RazorEnhanced.ToolBar.UpdatePanelImage();
					RazorEnhanced.ToolBar.UpdateCount();
				}
			});
		}

		private void toolboxstyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (toolboxstyleComboBox.Text != "TitleBar")
			{
				toolboxsizeComboBox.Enabled = toolbar_trackBar.Enabled = true;
		 		Assistant.Client.Instance.SetTitleStr(""); // Restore titlebar standard
			}
			else
				toolboxsizeComboBox.Enabled = toolbar_trackBar.Enabled = false;

			if (toolboxstyleComboBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteString("ToolBoxStyleComboBox", toolboxstyleComboBox.Text);

				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void toolboxsizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (toolboxsizeComboBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteString("ToolBoxSizeComboBox", toolboxsizeComboBox.Text);
				if (toolboxsizeComboBox.SelectedItem.ToString() == "Big")
				{
					Int32.TryParse(toolbarslot_label.Text, out int slot);
					if (slot == 0)
						slot = 2;

					if (slot % 2 != 0)
					{
						slot++;
						toolbarslot_label.Text = slot.ToString();
						RazorEnhanced.Settings.General.WriteInt("ToolBoxSlotsTextBox", slot);
					}
				}
				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void toolbaraddslotButton_Click(object sender, EventArgs e)
		{
			int slot = RazorEnhanced.Settings.General.ReadInt("ToolBoxSlotsTextBox");
			if (toolboxsizeComboBox.SelectedItem.ToString() == "Big")
			{
				slot += 2;
			}
			else
				slot += 1;

			toolbarslot_label.Text = slot.ToString();
			RazorEnhanced.Settings.General.WriteInt("ToolBoxSlotsTextBox", slot);
			RazorEnhanced.ToolBar.UptateToolBarComboBox(toolboxcountComboBox.SelectedIndex, slot);
			RazorEnhanced.ToolBar.Close();
			RazorEnhanced.ToolBar.Open();
		}

		private void toolbarremoveslotButton_Click(object sender, EventArgs e)
		{
			int slot = RazorEnhanced.Settings.General.ReadInt("ToolBoxSlotsTextBox");
			if (toolboxsizeComboBox.SelectedItem.ToString() == "Big")
				if (slot - 2 < 2)
					slot = 2;
				else
					slot -= 2;
			else
				if (slot - 1 < 1)
				slot = 1;
			else
				slot -= 1;

			toolbarslot_label.Text = slot.ToString();
			RazorEnhanced.Settings.General.WriteInt("ToolBoxSlotsTextBox", slot);
			RazorEnhanced.ToolBar.UptateToolBarComboBox(toolboxcountComboBox.SelectedIndex, slot);
			RazorEnhanced.ToolBar.Close();
			RazorEnhanced.ToolBar.Open();
		}

		private void showhitsToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showhitsToolBarCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ShowHitsToolBarCheckBox", showhitsToolBarCheckBox.Checked);
				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

        private void showtitheToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showtitheToolBarCheckBox.Focused)
            {
                RazorEnhanced.Settings.General.WriteBool("ShowTitheToolBarCheckBox", showtitheToolBarCheckBox.Checked);
                RazorEnhanced.ToolBar.Close();
                RazorEnhanced.ToolBar.Open();
            }
        }

        private void showstaminaToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showstaminaToolBarCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ShowStaminaToolBarCheckBox", showstaminaToolBarCheckBox.Checked);
				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void showmanaToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showmanaToolBarCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ShowManaToolBarCheckBox", showmanaToolBarCheckBox.Checked);
				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void showweightToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showweightToolBarCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ShowWeightToolBarCheckBox", showweightToolBarCheckBox.Checked);
				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void showfollowerToolBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showfollowerToolBarCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ShowFollowerToolBarCheckBox", showfollowerToolBarCheckBox.Checked);
				RazorEnhanced.ToolBar.Close();
				RazorEnhanced.ToolBar.Open();
			}
		}

		private void toolbar_trackBar_Scroll(object sender, EventArgs e)
		{
			int o = toolbar_trackBar.Value;

			if (toolbar_trackBar.Focused)
			{
				RazorEnhanced.Settings.General.WriteInt("ToolBarOpacity", o);
				if (RazorEnhanced.ToolBar.ToolBarForm != null)
				{
					RazorEnhanced.ToolBar.ToolBarForm.Show();
					RazorEnhanced.ToolBar.ToolBarForm.Opacity = ((double)o) / 100.0;
				}
			}

			toolbar_opacity_label.Text = String.Format("{0}%", o);
		}

		private void timerupdatestatus_Tick(object sender, EventArgs e)
		{
			UpdateRazorStatus();
			UpdateScriptGrid();
			if (toolboxstyleComboBox.Text != "TitleBar")
				RazorEnhanced.ToolBar.UpdateCount();
			SpellGrid.UpdateSAIcon();
		}

		private void timertitlestatusbar_Tick(object sender, EventArgs e)
		{
			if (Initializing || !Client.Running)
				return;

			if (!Assistant.Client.Instance.Ready || World.Player == null)
				return;

			if (toolboxstyleComboBox.Text == "TitleBar")
				TitleBar.UpdateTitleBar();
		}
	}
}
