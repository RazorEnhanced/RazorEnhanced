using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal Label SellBagLabel { get { return sellBagLabel; } }
		internal RazorCheckBox SellCheckBox { get { return sellEnableCheckBox; } }
		internal ListBox SellLogBox { get { return sellLogBox; } }
		internal RazorComboBox SellListSelect { get { return sellListSelect; } }
		internal DataGridView VendorSellGridView { get { return vendorsellGridView; } }

		private void sellListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			SellAgent.UpdateListParam(sellListSelect.Text);

			if (sellListSelect.Focused && sellListSelect.Text != String.Empty)
			{
				Settings.SellAgent.ListUpdate(sellListSelect.Text, RazorEnhanced.SellAgent.SellBag, true);
				SellAgent.AddLog("Sell Agent list changed to: " + sellListSelect.Text);
			}

			SellAgent.InitGrid();
		}

		private void sellAddList_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 5;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(5).Show();
		}

		private void sellCloneListButton_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 14;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(14).Show();
		}

		private void sellRemoveList_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != String.Empty)
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Vendor Sell list: " + sellListSelect.Text, "Delete Vendor Sell List?", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.SellAgent.AddLog("Sell Agent list " + sellListSelect.Text + " removed!");
					RazorEnhanced.SellAgent.RemoveList(sellListSelect.Text);
				}
			}
		}

		private void sellAddTarget_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(SellAgentItemTarget_Callback));
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void SellAgentItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item sellItem = Assistant.World.FindItem(serial);
			if (sellItem != null && sellItem.Serial.IsItem)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Sell Agent item added: " + sellItem.ToString(), false);
				RazorEnhanced.SellAgent.AddLog("Sell Agent item added: " + sellItem.ToString());
				this.Invoke((MethodInvoker)delegate { RazorEnhanced.SellAgent.AddItemToList(sellItem.Name, sellItem.ItemID, 999, sellItem.Hue); });
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid target", false);
				RazorEnhanced.SellAgent.AddLog("Invalid target");
			}
		}

		private void sellEnableCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (World.Player == null)  // offline
			{
				if (sellEnableCheckBox.Checked)
				{
					sellEnableCheckBox.Checked = false;
					SellAgent.AddLog("You are not logged in game!");
				}
				return;
			}

			if (sellListSelect.Text == String.Empty) // Nessuna lista
			{
				if (sellEnableCheckBox.Checked)
				{
					sellEnableCheckBox.Checked = false;
					SellAgent.AddLog("Item list not selected!");
				}
				return;
			}

			if (sellEnableCheckBox.Checked)
			{
				Assistant.Item bag = Assistant.World.FindItem(SellAgent.SellBag);

				if (bag != null && (bag.RootContainer != World.Player || !bag.IsContainer))
				{
					SellAgent.AddLog("Invalid or not accessible Container!");
					if (showagentmessageCheckBox.Checked)
						Misc.SendMessage("Invalid or not accessible Container!", false);
					sellEnableCheckBox.Checked = false;
				}
				else
				{
					sellListSelect.Enabled = false;
					sellAddListButton.Enabled = false;
					sellRemoveListButton.Enabled = false;
					sellCloneListButton.Enabled = false;
					SellAgent.AddLog("Apply item list " + sellListSelect.SelectedItem.ToString() + " filter ok!");
					if (showagentmessageCheckBox.Checked)
						Misc.SendMessage("Apply item list " + sellListSelect.SelectedItem.ToString() + " filter ok!", false);
					SellAgent.EnableSellFilter();
				}
			}
			else
			{
				sellListSelect.Enabled = true;
				sellAddListButton.Enabled = true;
				sellRemoveListButton.Enabled = true;
				sellCloneListButton.Enabled = true;
				if (sellListSelect.Text != String.Empty)
				{
					RazorEnhanced.SellAgent.AddLog("Remove item list " + sellListSelect.SelectedItem.ToString() + " filter ok!");
					if (showagentmessageCheckBox.Checked)
						RazorEnhanced.Misc.SendMessage("Remove item list " + sellListSelect.SelectedItem.ToString() + " filter ok!", false);
				}
			}
		}


		private void sellSetBag_Click(object sender, EventArgs e)
		{
			SellAgentSetBag();
		}

		internal void SellAgentSetBag()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select Sell bag.", false);

			if (sellListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(sellBagTarget_Callback));
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void sellBagTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item sellBag = Assistant.World.FindItem(serial);

			if (sellBag == null)
				return;

			if (sellBag != null && sellBag.Serial.IsItem && sellBag.IsContainer && sellBag.RootContainer == Assistant.World.Player)
			{
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Container set to: " + sellBag.ToString(), false);
				SellAgent.AddLog("Container set to: " + sellBag.ToString());
				SellAgent.SellBag = (int)sellBag.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Invalid container, set backpack", false);
				SellAgent.AddLog("Invalid container, set backpack");
				SellAgent.SellBag = (int)World.Player.Backpack.Serial.Value;
			}

			this.Invoke((MethodInvoker)delegate {
				RazorEnhanced.Settings.SellAgent.ListUpdate(sellListSelect.Text, serial, true);
				RazorEnhanced.SellAgent.RefreshLists();
			});
		}

		private void vendorsellGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = vendorsellGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 4)
			{
				cell.Value = Utility.FormatDatagridColorCell(cell);
			}
			else if (e.ColumnIndex == 3)
			{
				cell.Value = Utility.FormatDatagridAmountCell(cell, false);
			}
			else if (e.ColumnIndex == 2)
			{
				cell.Value = Utility.FormatDatagridItemIDCell(cell);
			}
			RazorEnhanced.SellAgent.CopyTable();
		}

		private void vendorsellGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = false;
			e.Row.Cells[1].Value = "New Item";
			e.Row.Cells[2].Value = "0x0000";
			e.Row.Cells[3].Value = 9999;
			e.Row.Cells[4].Value = "0x0000";
		}
	}
}
