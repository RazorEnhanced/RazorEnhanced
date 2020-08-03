using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal RazorCheckBox AutolootCheckBox { get { return autoLootCheckBox; } }
		internal RazorAgentNumOnlyTextBox AutolootLabelDelay { get { return autoLootTextBoxDelay; } }
		internal RazorAgentNumOnlyTextBox AutoLootTextBoxMaxRange { get { return autoLootTextBoxMaxRange; } }
		internal Label AutoLootContainerLabel { get { return autolootContainerLabel; } }
		internal ListBox AutoLootLogBox { get { return autolootLogBox; } }
		internal RazorComboBox AutoLootListSelect { get { return autolootListSelect; } }
		internal CheckBox AutoLootNoOpenCheckBox { get { return autoLootnoopenCheckBox; } }
		internal DataGridView AutoLootDataGridView { get { return autolootdataGridView; } }
		internal RazorCheckBox AutolootAutostartCheckBox { get { return autolootautostartCheckBox; } }

		private void autolootautostartCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (autolootautostartCheckBox.Focused)
				Settings.General.WriteBool("AutolootAutostartCheckBox", autolootautostartCheckBox.Checked);
		}

		private void autolootContainerButton_Click(object sender, EventArgs e)
		{
			AutolootSetBag();
		}

		internal void AutolootSetBag()
		{
			if (showagentmessageCheckBox.Checked)
				RazorEnhanced.Misc.SendMessage("Autoloot Select Loot Bag", false);

			if (autolootListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(autoLootSetContainerTarget_Callback));
			else
				AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootSetContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item autoLootBag = Assistant.World.FindItem(serial);

			if (autoLootBag == null)
				return;

			if (autoLootBag != null && autoLootBag.Serial.IsItem && autoLootBag.IsContainer && autoLootBag.RootContainer == Assistant.World.Player)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Autoloot Container set to: " + autoLootBag.ToString(), false);
				RazorEnhanced.AutoLoot.AddLog("Autoloot Container set to: " + autoLootBag.ToString());
				AutoLoot.AutoLootBag = (int)autoLootBag.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Autoloot Container, set backpack", false);
				RazorEnhanced.AutoLoot.AddLog("Invalid Autoloot Container, set backpack");
				AutoLoot.AutoLootBag = (int)World.Player.Backpack.Serial.Value;
			}
			BeginInvoke((MethodInvoker)delegate {
				RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, RazorEnhanced.AutoLoot.AutoLootDelay, serial, true, RazorEnhanced.AutoLoot.NoOpenCorpse, RazorEnhanced.AutoLoot.MaxRange);
				RazorEnhanced.AutoLoot.RefreshLists();
			});
		}

		private void autoLootAddItemTarget_Click(object sender, EventArgs e)
		{
			AutolootAddItem();
		}

		internal void AutolootAddItem()
		{
			if (showagentmessageCheckBox.Checked)
				RazorEnhanced.Misc.SendMessage("Select item to add in Autoloot list", false);

			if (autolootListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(autoLootItemTarget_Callback));
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item autoLootItem = Assistant.World.FindItem(serial);
			if (autoLootItem != null && autoLootItem.Serial.IsItem)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Autoloot item added: " + autoLootItem.ToString(), false);
				RazorEnhanced.AutoLoot.AddLog("Autoloot item added: " + autoLootItem.ToString());
				this.Invoke((MethodInvoker)delegate { RazorEnhanced.AutoLoot.AddItemToList(autoLootItem.Name, autoLootItem.ItemID, autoLootItem.Hue); });
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid target", false);
				RazorEnhanced.AutoLoot.AddLog("Invalid target");
			}
		}

		private void autoLootItemProps_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != String.Empty)
			{
				if (autolootdataGridView.CurrentCell == null)
					return;

				DataGridViewRow row = autolootdataGridView.Rows[autolootdataGridView.CurrentCell.RowIndex];
				EnhancedAutolootEditItemProps editProp = new EnhancedAutolootEditItemProps(ref row)
				{
					TopMost = true
				};
				editProp.Show();
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (World.Player == null)  // offline
			{
				if (autoLootCheckBox.Checked)
				{
					AutoLoot.AddLog("You are not logged in game!");
					autoLootCheckBox.Checked = false;
				}
				return;
			}

			if (autolootListSelect.Text == String.Empty) // Nessuna lista
			{
				if (autoLootCheckBox.Checked)
				{
					autoLootCheckBox.Checked = false;
					AutoLoot.AddLog("Item list not selected!");
				}
				return;
			}

			if (autoLootCheckBox.Checked)
			{
				autolootListSelect.Enabled = false;
				autolootButtonAddList.Enabled = false;
				autoLootButtonRemoveList.Enabled = false;
				autoLootButtonListClone.Enabled = false;
				autoLootTextBoxDelay.Enabled = false;
				autoLootTextBoxMaxRange.Enabled = false;

				AutoLoot.ResetIgnore();
				AutoLoot.AutoMode = true;
				AutoLoot.AddLog("Autoloot Engine Start...");
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("AUTOLOOT: Engine Start...", false);
			}
			else
			{
				autolootListSelect.Enabled = true;
				autolootButtonAddList.Enabled = true;
				autoLootButtonRemoveList.Enabled = true;
				autoLootButtonListClone.Enabled = true;
				autoLootTextBoxDelay.Enabled = true;
				autoLootTextBoxMaxRange.Enabled = true;

				// Stop autoloot
				AutoLoot.AutoMode = false;
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("AUTOLOOT: Engine Stop...", false);
				AutoLoot.AddLog("Autoloot Engine Stop...");
			}
		}


		private void autoLootListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			AutoLoot.UpdateListParam(autolootListSelect.Text);

			if (autolootListSelect.Focused && autolootListSelect.Text != String.Empty)
			{
				Settings.AutoLoot.ListUpdate(autolootListSelect.Text, AutoLoot.AutoLootDelay, AutoLoot.AutoLootBag, true, AutoLoot.NoOpenCorpse, AutoLoot.MaxRange);
				AutoLoot.AddLog("Autoloot list changed to: " + autolootListSelect.Text);
			}

			AutoLoot.InitGrid();
		}
		private void autoLootnoopenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (autoLootnoopenCheckBox.Focused)
			{
				AutoLoot.NoOpenCorpse = autoLootnoopenCheckBox.Checked;
				RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, AutoLoot.AutoLootDelay, AutoLoot.AutoLootBag, true, AutoLoot.NoOpenCorpse, AutoLoot.MaxRange);
				RazorEnhanced.AutoLoot.RefreshLists();
			}
		}

		private void autoLootButtonAddList_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 1;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(1).Show();
		}
		private void autoLootButtonListClone_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 10;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(10).Show();
		}

		private void autoLootButtonRemoveList_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != String.Empty)
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure to delete this AutoLoot list: " + autolootListSelect.Text, "Delete AutoLoot List?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.AutoLoot.AddLog("Autoloot list " + autolootListSelect.Text + " removed!");
					RazorEnhanced.AutoLoot.AutoLootBag = 0;
					RazorEnhanced.AutoLoot.AutoLootDelay = 100;
					RazorEnhanced.AutoLoot.NoOpenCorpse = false;
					RazorEnhanced.AutoLoot.MaxRange = 1;
					RazorEnhanced.AutoLoot.RemoveList(autolootListSelect.Text);
					autolootListSelect.SelectedIndex = -1;

				}
			}
		}

		private void autoLootTextBoxDelay_Leave(object sender, EventArgs e)
		{

			if (autoLootTextBoxDelay.Text == String.Empty)
				autoLootTextBoxDelay.Text = "100";

			AutoLoot.AutoLootDelay = Convert.ToInt32(autoLootTextBoxDelay.Text);

			RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, AutoLoot.AutoLootDelay, AutoLoot.AutoLootBag, true, AutoLoot.NoOpenCorpse, AutoLoot.MaxRange);
			RazorEnhanced.AutoLoot.RefreshLists();
		}

		private void autoLootTextBoxMaxRange_Leave(object sender, EventArgs e)
		{
			if (autoLootTextBoxMaxRange.Text == String.Empty)
				autoLootTextBoxMaxRange.Text = "0";

			AutoLoot.MaxRange = Convert.ToInt32(autoLootTextBoxMaxRange.Text);

			RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, AutoLoot.AutoLootDelay, AutoLoot.AutoLootBag, true, AutoLoot.NoOpenCorpse, AutoLoot.MaxRange);
			RazorEnhanced.AutoLoot.RefreshLists();
		}

		private void autolootdataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = autolootdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 3)
			{
				cell.Value = Utility.FormatDatagridColorCell(cell);
			}
			else if (e.ColumnIndex == 2)
			{
				cell.Value = Utility.FormatDatagridItemIDCellAutoLoot(cell);
			}
			RazorEnhanced.AutoLoot.CopyTable();
		}

		private void autolootdataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = false;
			e.Row.Cells[1].Value = "New Item";
			e.Row.Cells[2].Value = "0x0000";
			e.Row.Cells[3].Value = "0x0000";
			e.Row.Cells[4].Value = null;
		}
	}
}
