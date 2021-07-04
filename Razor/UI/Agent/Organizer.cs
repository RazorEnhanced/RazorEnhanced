using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal RazorAgentNumOnlyTextBox OrganizerDragDelay { get { return organizerDragDelay; } }
		internal Label OrganizerSourceLabel { get { return organizerSourceLabel; } }
		internal Label OrganizerDestinationLabel { get { return organizerDestinationLabel; } }
		internal ListBox OrganizerLogBox { get { return organizerLogBox; } }
		internal DataGridView OrganizerDataGridView { get { return organizerdataGridView; } }
		internal RazorComboBox OrganizerListSelect { get { return organizerListSelect; } }
		internal Button OrganizerExecute { get { return organizerExecuteButton; } }
		internal Button OrganizerStop { get { return organizerStopButton; } }

		private void organizerAddList_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 3;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(3).Show();
		}

		private void organizerCloneListB_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 12;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(12).Show();
		}

		private void organizerRemoveList_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != String.Empty)
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Organizer list: " + organizerListSelect.Text, "Delete Organizer List?", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Organizer.AddLog("Organizer list " + organizerListSelect.Text + " removed!");
					RazorEnhanced.Organizer.OrganizerSource = 0;
					RazorEnhanced.Organizer.OrganizerDestination = 0;
					RazorEnhanced.Organizer.OrganizerDelay = 100;
					RazorEnhanced.Organizer.RemoveList(organizerListSelect.Text);
				}
			}
		}

		private void organizerSetSource_Click(object sender, EventArgs e)
		{
			OrganizerSetSource();
		}

		internal void OrganizerSetSource()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select Source container", false);

			if (organizerListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OrganizerSourceContainerTarget_Callback));
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}
        private bool AcceptibleOrganizerTarget(Assistant.Item organizerBag)
        {
            if (organizerBag.ItemID == 0x2259)
                return true;

            return organizerBag.Serial.IsItem && organizerBag.IsContainer;
        }

        private void OrganizerSourceContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item organizerBag = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (organizerBag == null)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Source Container, set backpack", false);
				RazorEnhanced.Organizer.AddLog("Invalid Source Container, set backpack");
				RazorEnhanced.Organizer.OrganizerSource = (int)World.Player.Backpack.Serial.Value;
				return;
			}

			if (organizerBag != null && AcceptibleOrganizerTarget(organizerBag))
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Source Container set to: " + organizerBag.ToString(), false);
				RazorEnhanced.Organizer.AddLog("Source Container set to: " + organizerBag.ToString());
				RazorEnhanced.Organizer.OrganizerSource = (int)organizerBag.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Source Container, set backpack", false);
				RazorEnhanced.Organizer.AddLog("Invalid Source Container, set backpack");
				RazorEnhanced.Organizer.OrganizerSource = (int)World.Player.Backpack.Serial.Value;
			}

			this.Invoke((MethodInvoker)delegate {
				RazorEnhanced.Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, serial, RazorEnhanced.Organizer.OrganizerDestination, true);
				RazorEnhanced.Organizer.RefreshLists();
			});
		}

		private void organizerSetDestination_Click(object sender, EventArgs e)
		{
			OrganizerSetDestination();
		}

		internal void OrganizerSetDestination()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select destination container", false);

			if (organizerListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OrganizerDestinationContainerTarget_Callback));
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void OrganizerDestinationContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item organizerBag = Assistant.World.FindItem((Assistant.Serial)((uint)serial));

			if (organizerBag == null)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Destination Container, set backpack", false);
				RazorEnhanced.Organizer.AddLog("Invalid Destination Container, set backpack");
				RazorEnhanced.Organizer.OrganizerDestination = (int)World.Player.Backpack.Serial.Value;
				return;
			}

			if (organizerBag != null && AcceptibleOrganizerTarget(organizerBag))
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Destination Container set to: " + organizerBag.ToString(), false);
				RazorEnhanced.Organizer.AddLog("Destination Container set to: " + organizerBag.ToString());
				RazorEnhanced.Organizer.OrganizerDestination = (int)organizerBag.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Destination Container, set backpack", false);
				RazorEnhanced.Organizer.AddLog("Invalid Destination Container, set backpack");
				RazorEnhanced.Organizer.OrganizerDestination = (int)World.Player.Backpack.Serial.Value;
			}

			this.Invoke((MethodInvoker)delegate {
				RazorEnhanced.Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, RazorEnhanced.Organizer.OrganizerSource, serial, true);
				RazorEnhanced.Organizer.RefreshLists();
			});
		}

		private void organizerListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			Organizer.UpdateListParam(organizerListSelect.Text);

			if (organizerListSelect.Focused && organizerListSelect.Text != String.Empty)
			{
				Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, RazorEnhanced.Organizer.OrganizerSource, RazorEnhanced.Organizer.OrganizerDestination, true);
				Organizer.AddLog("Organizer list changed to: " + organizerListSelect.Text);
			}

			Organizer.InitGrid();
		}

		private void organizerAddTarget_Click(object sender, EventArgs e)
		{
			OrganizerAddItem();
		}

		internal void OrganizerAddItem()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select item to add in Organizer list", false);

			if (organizerListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OrganizerItemTarget_Callback));
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void OrganizerItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item organizerItem = Assistant.World.FindItem(serial);
			if (organizerItem != null && organizerItem.Serial.IsItem)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Organizer item added: " + organizerItem.ToString(), false);
				RazorEnhanced.Organizer.AddLog("Organizer item added: " + organizerItem.ToString());
				this.Invoke((MethodInvoker)delegate { RazorEnhanced.Organizer.AddItemToList(organizerItem.Name, organizerItem.ItemID, organizerItem.Hue); });
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid target", false);
				RazorEnhanced.Organizer.AddLog("Invalid target");
			}
		}
		private void organizerDragDelay_Leave(object sender, EventArgs e)
		{
			if (organizerDragDelay.Text == String.Empty)
				organizerDragDelay.Text = "100";

			Organizer.OrganizerDelay = Convert.ToInt32(organizerDragDelay.Text);

			Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, RazorEnhanced.Organizer.OrganizerSource, RazorEnhanced.Organizer.OrganizerDestination, true);
			Organizer.RefreshLists();
		}

		private void organizerExecute_Click(object sender, EventArgs e)
		{
			OrganizerStartExec();
		}

		internal void OrganizerStartExec()
		{
			if (World.Player == null)  // offline
			{
				Organizer.AddLog("You are not logged in game!");
				return;
			}

			if (organizerListSelect.Text == String.Empty) // Nessuna lista
			{
				Organizer.AddLog("Item list not selected!");
				return;
			}

			Organizer.Start();
			Organizer.AddLog("Organizer Engine Start...");
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("ORGANIZER: Engine Start...", false);

			OrganizerStartWork();
		}

		private void organizerStop_Click(object sender, EventArgs e)
		{
			OrganizerStopExec();
		}

		internal void OrganizerStopExec()
		{
			Organizer.ForceStop();

			Organizer.AddLog("Organizer Engine force stop...");
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("ORGANIZER: Organizer Engine force stop...", false);
			OrganizerFinishWork();
		}

		private delegate void OrganizerStartWorkCallback();

		internal void OrganizerStartWork()
		{
			if (organizerStopButton.InvokeRequired ||
				organizerExecuteButton.InvokeRequired ||
				organizerListSelect.InvokeRequired ||
				organizerAddListB.InvokeRequired ||
				organizerRemoveListB.InvokeRequired ||
				organizerDragDelay.InvokeRequired)
			{
				OrganizerStartWorkCallback d = new OrganizerStartWorkCallback(OrganizerStartWork);
				this.Invoke(d, null);
			}
			else
			{
				organizerStopButton.Enabled = true;
				organizerExecuteButton.Enabled = false;
				organizerListSelect.Enabled = false;
				organizerAddListB.Enabled = false;
				organizerRemoveListB.Enabled = false;
				organizerCloneListB.Enabled = false;
				organizerDragDelay.Enabled = false;
			}
		}

		private delegate void OrganizerFinishWorkCallback();

		internal void OrganizerFinishWork()
		{
			if (organizerStopButton.InvokeRequired ||
				organizerExecuteButton.InvokeRequired ||
				organizerListSelect.InvokeRequired ||
				organizerAddListB.InvokeRequired ||
				organizerRemoveListB.InvokeRequired ||
				organizerDragDelay.InvokeRequired)
			{
				OrganizerFinishWorkCallback d = new OrganizerFinishWorkCallback(OrganizerFinishWork);
				this.Invoke(d, null);
			}
			else
			{
				organizerStopButton.Enabled = false;
				organizerExecuteButton.Enabled = true;
				organizerListSelect.Enabled = true;
				organizerAddListB.Enabled = true;
				organizerRemoveListB.Enabled = true;
				organizerCloneListB.Enabled = true;
				organizerDragDelay.Enabled = true;
			}
		}

		private void organizerdataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = organizerdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 3)
			{
				cell.Value = Utility.FormatDatagridColorCell(cell);
			}
			else if (e.ColumnIndex == 4)
			{
				cell.Value = Utility.FormatDatagridAmountCell(cell, true);
			}
			else if (e.ColumnIndex == 2)
			{
				cell.Value = Utility.FormatDatagridItemIDCell(cell);
			}
			RazorEnhanced.Organizer.CopyTable();
		}

		private void organizerdataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = false;
			e.Row.Cells[1].Value = "New Item";
			e.Row.Cells[2].Value = "0x0000";
			e.Row.Cells[3].Value = "0x0000";
			e.Row.Cells[4].Value = "1";
		}
	}
}
