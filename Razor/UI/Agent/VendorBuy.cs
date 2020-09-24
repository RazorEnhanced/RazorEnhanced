using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal RazorCheckBox BuyCheckBox { get { return buyEnableCheckBox; } }
		internal ListBox BuyLogBox { get { return buyLogBox; } }
		internal RazorComboBox BuyListSelect { get { return buyListSelect; } }
		internal RazorCheckBox BuyCompareNameCheckBox { get { return buyCompareNameCheckBox; } }
		internal DataGridView VendorBuyDataGridView { get { return vendorbuydataGridView; } }

		private void buyListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (buyListSelect.Text != String.Empty)
			{
				BuyAgent.UpdateListParam(buyListSelect.Text);

				if (buyListSelect.Focused)
				{
					Settings.BuyAgent.ListUpdate(buyListSelect.Text, RazorEnhanced.BuyAgent.CompareName, true);

					BuyAgent.AddLog("Buy Agent list changed to: " + buyListSelect.Text);
				}
			}

			RazorEnhanced.BuyAgent.InitGrid();
		}

		private void buyAddList_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 4;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(4).Show();
		}

		private void buyCloneButton_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 13;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(13).Show();
		}

		private void buyRemoveList_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != String.Empty)
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Vendor Buy list: " + buyListSelect.Text, "Delete Vendor Buy List?", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.BuyAgent.AddLog("Buy Agent list " + buyListSelect.Text + " removed!");
					RazorEnhanced.BuyAgent.RemoveList(buyListSelect.Text);
				}
			}
		}

		private void buyAddTarget_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != String.Empty)
			{
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(BuyAgentItemTarget_Callback));
			}
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}

		private void BuyAgentItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item buyItem = Assistant.World.FindItem(serial);
			if (buyItem != null && buyItem.Serial.IsItem)
			{
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Buy Agent item added: " + buyItem.ToString(), false);
				BuyAgent.AddLog("Buy Agent item added: " + buyItem.ToString());
				this.Invoke((MethodInvoker)delegate { RazorEnhanced.BuyAgent.AddItemToList(buyItem.Name, buyItem.ItemID, 999, buyItem.Hue); });
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Invalid target", false);
				BuyAgent.AddLog("Invalid target");
			}
		}

		private void buyEnableCheckB_CheckedChanged(object sender, EventArgs e)
		{
			if (World.Player == null)  // offline
			{
				buyEnableCheckBox.Checked = false;
				BuyAgent.AddLog("You are not logged in game!");
				return;
			}

			if (buyListSelect.Text == String.Empty) // Nessuna lista
			{
				buyEnableCheckBox.Checked = false;
				BuyAgent.AddLog("Item list not selected!");
				return;
			}

			if (buyEnableCheckBox.Checked)
			{
				buyListSelect.Enabled = false;
				buyAddListButton.Enabled = false;
				buyRemoveListButton.Enabled = false;
				buyCloneButton.Enabled = false;
				BuyAgent.AddLog("Apply item list " + buyListSelect.SelectedItem.ToString() + " filter ok!");
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Apply item list " + buyListSelect.SelectedItem.ToString() + " filter ok!", false);
				BuyAgent.EnableBuyFilter();
			}
			else
			{
				buyListSelect.Enabled = true;
				buyAddListButton.Enabled = true;
				buyRemoveListButton.Enabled = true;
				buyCloneButton.Enabled = true;
				BuyAgent.AddLog("Remove item list " + buyListSelect.SelectedItem.ToString() + " filter ok!");
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Remove item list " + buyListSelect.SelectedItem.ToString() + " filter ok!", false);
			}
		}

		private void vendorbuydataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = vendorbuydataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

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
			RazorEnhanced.BuyAgent.CopyTable();
		}

		private void vendorbuydataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = false;
			e.Row.Cells[1].Value = "New Item";
			e.Row.Cells[2].Value = "0x0000";
			e.Row.Cells[3].Value = 999;
			e.Row.Cells[4].Value = "0x0000";
		}

		private void buyCompareNameCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (buyCompareNameCheckBox.Focused)
			{
				Settings.BuyAgent.ListUpdate(buyListSelect.Text, buyCompareNameCheckBox.Checked, true);
				BuyAgent.CompareName = buyCompareNameCheckBox.Checked;
			}
		}
	}
}
