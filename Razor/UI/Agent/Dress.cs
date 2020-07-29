using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal CheckBox DressCheckBox { get { return dressConflictCheckB; } }
		internal ListView DressListView { get { return dressListView; } }
		internal ListBox DressLogBox { get { return dressLogBox; } }
		internal RazorAgentNumOnlyTextBox DressDragDelay { get { return dressDragDelay; } }
		internal ComboBox DressListSelect { get { return dressListSelect; } }
		internal Label DressBagLabel { get { return dressBagLabel; } }

		internal Button DressExecuteButton { get { return dressExecuteButton; } }
		internal Button UnDressExecuteButton { get { return undressExecuteButton; } }
		internal Button DressStopButton { get { return dressStopButton; } }

		private void dressListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Dress.UpdateListParam(dressListSelect.Text);

			if (dressListSelect.Focused && dressListSelect.Text != String.Empty)
			{
				Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
				RazorEnhanced.Dress.AddLog("Dress list changed to: " + dressListSelect.Text);
			}

			RazorEnhanced.Dress.RefreshItems();
		}

		private void dressAddListB_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 6;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(6).Show();
		}

		private void dressRemoveListB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Dress list: " + dressListSelect.Text, "Delete Dress List?", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Dress.AddLog("Dress list " + dressListSelect.Text + " removed!");
					RazorEnhanced.Dress.DressBag = 0;
					RazorEnhanced.Dress.DressDelay = 100;
					RazorEnhanced.Dress.DressConflict = false;
					RazorEnhanced.Dress.RemoveList(dressListSelect.Text);
					HotKey.Init();
				}
			}
		}

		private void dressDragDelay_Leave(object sender, EventArgs e)
		{
			if (dressDragDelay.Text == String.Empty)
				dressDragDelay.Text = "100";

			RazorEnhanced.Dress.DressDelay = Convert.ToInt32(dressDragDelay.Text);

			RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
			RazorEnhanced.Dress.RefreshLists();
		}

		private void dressConflictCheckB_CheckedChanged(object sender, EventArgs e)
		{
			if (dressConflictCheckB.Focused)
			{
				RazorEnhanced.Dress.DressConflict = dressConflictCheckB.Checked;
				RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
				RazorEnhanced.Dress.RefreshLists();
			}
		}

		private void dressReadB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
				RazorEnhanced.Dress.ReadPlayerDress();
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void dressSetBagB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(DressItemContainerTarget_Callback));
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void DressItemContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item dressBag = Assistant.World.FindItem(serial);

			if (dressBag == null)
				return;

			if (dressBag != null && dressBag.Serial.IsItem && dressBag.IsContainer)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Undress Container set to: " + dressBag.ToString(), false);
				RazorEnhanced.Dress.AddLog("Undress Container set to: " + dressBag.ToString());
				RazorEnhanced.Dress.DressBag = (int)dressBag.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid Undress Container, set backpack", false);
				RazorEnhanced.Dress.AddLog("Invalid Undress Container, set backpack");
				RazorEnhanced.Dress.DressBag = (int)World.Player.Backpack.Serial.Value;
			}

			this.Invoke((MethodInvoker)delegate {
				RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
				RazorEnhanced.Dress.RefreshLists();
			});
		}

		private void dressRemoveB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
			{
				if (dressListView.SelectedItems.Count == 1)
				{
					int index = dressListView.SelectedItems[0].Index;
					string selection = dressListSelect.Text;

					if (RazorEnhanced.Settings.Dress.ListExists(selection))
					{
						List<Dress.DressItemNew> items = Settings.Dress.ItemsRead(selection);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.Dress.ItemDelete(selection, items[index]);
							RazorEnhanced.Dress.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void dressClearListB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
			{
				string selection = dressListSelect.Text;
				RazorEnhanced.Settings.Dress.ClearList(selection);
				RazorEnhanced.Dress.RefreshItems();
			}
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void dresslistView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (dressListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.Dress.UpdateSelectedItems(item.Index);
			}
		}

		private void dressAddTargetB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(DressItemTarget_Callback));
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void DressItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item dressItem = Assistant.World.FindItem(serial);
			if (dressItem != null && dressItem.Serial.IsItem)
				this.Invoke((MethodInvoker)delegate { RazorEnhanced.Dress.AddItemByTarger(dressItem); });
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid target", false);
				RazorEnhanced.Dress.AddLog("Invalid target");
			}
		}

		private void dressAddManualB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != String.Empty)
			{
				EnhancedDressAddUndressLayer ManualAddLayer = new EnhancedDressAddUndressLayer
				{
					TopMost = true
				};
				ManualAddLayer.Show();
			}
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void razorButton10_Click(object sender, EventArgs e)
		{
			UndressStart();
		}

		internal void UndressStart()
		{
			if (World.Player == null) // non loggato
			{
				RazorEnhanced.Dress.AddLog("You are not logged in game!");
				UndressFinishWork();
				return;
			}

			if (dressListSelect.Text == String.Empty)
			{
				RazorEnhanced.Dress.AddLog("Item List not selected!");
				UndressFinishWork();
				return;
			}

			UndressStartWork();
			RazorEnhanced.Dress.UndressStart();
			RazorEnhanced.Organizer.AddLog("Undress Engine Start...");

			if (showagentmessageCheckBox.Checked)
				RazorEnhanced.Misc.SendMessage("UNDRESS: Engine Start...", false);
		}

		private delegate void UndressFinishWorkCallback();

		internal void UndressFinishWork()
		{
			if (dressConflictCheckB.InvokeRequired ||
				dressExecuteButton.InvokeRequired ||
				undressExecuteButton.InvokeRequired ||
				dressAddListB.InvokeRequired ||
				dressRemoveListB.InvokeRequired ||
				dressStopButton.InvokeRequired ||
				organizerDragDelay.InvokeRequired)
			{
				UndressFinishWorkCallback d = new UndressFinishWorkCallback(UndressFinishWork);
				this.Invoke(d, null);
			}
			else
			{
				dressStopButton.Enabled = false;
				dressConflictCheckB.Enabled = true;
				dressExecuteButton.Enabled = true;
				undressExecuteButton.Enabled = true;
				dressAddListB.Enabled = true;
				dressRemoveListB.Enabled = true;
				dressDragDelay.Enabled = true;
			}
		}

		private delegate void UndressStartWorkCallback();

		internal void UndressStartWork()
		{
			if (dressConflictCheckB.InvokeRequired ||
				dressExecuteButton.InvokeRequired ||
				undressExecuteButton.InvokeRequired ||
				dressAddListB.InvokeRequired ||
				dressRemoveListB.InvokeRequired ||
				dressStopButton.InvokeRequired ||
				organizerDragDelay.InvokeRequired)
			{
				UndressStartWorkCallback d = new UndressStartWorkCallback(UndressStartWork);
				this.Invoke(d, null);
			}
			else
			{
				dressStopButton.Enabled = true;
				dressConflictCheckB.Enabled = false;
				dressExecuteButton.Enabled = false;
				undressExecuteButton.Enabled = false;
				dressAddListB.Enabled = false;
				dressRemoveListB.Enabled = false;
				dressDragDelay.Enabled = false;
			}
		}

		private void dressExecuteButton_Click(object sender, EventArgs e)
		{
			DressStart();
		}

		internal void DressStart()
		{
			if (World.Player == null) // non loggato
			{
				RazorEnhanced.Dress.AddLog("You are not logged in game!");
				UndressFinishWork();
				return;
			}

			if (dressListSelect.Text == String.Empty)
			{
				RazorEnhanced.Dress.AddLog("Item List not selected!");
				UndressFinishWork();
				return;
			}

			UndressStartWork();
			RazorEnhanced.Dress.DressStart();
			RazorEnhanced.Organizer.AddLog("Dress Engine Start...");

			if (showagentmessageCheckBox.Checked)
				RazorEnhanced.Misc.SendMessage("DRESS: Engine Start...", false);
		}

		private void dressStopButton_Click(object sender, EventArgs e)
		{
			DressStop();
		}

		internal void DressStop()
		{
			RazorEnhanced.Dress.ForceStop();

			RazorEnhanced.Dress.AddLog("Dress / Undress Engine force stop...");
			if (showagentmessageCheckBox.Checked)
				RazorEnhanced.Misc.SendMessage("DRESS/UNDRESS: Engine force stop...", false);
			UndressFinishWork();
		}
	}
}
