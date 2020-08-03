using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal RazorCheckBox BlockPartyInviteCheckBox { get { return blockpartyinviteCheckBox; } }
		internal RazorCheckBox BlockTradeRequestCheckBox { get { return blocktraderequestCheckBox; } }
		internal RazorCheckBox ShowStaticFieldCheckBox { get { return showstaticfieldCheckBox; } }
		internal RazorCheckBox FlagsHighlightCheckBox { get { return flagsHighlightCheckBox; } }
		internal RazorCheckBox HighlightTargetCheckBox { get { return highlighttargetCheckBox; } }
		internal RazorCheckBox AutoCarverCheckBox { get { return autocarverCheckBox; } }
		internal RazorCheckBox BoneCutterCheckBox { get { return bonecutterCheckBox; } }
		internal RazorCheckBox MobFilterCheckBox { get { return mobfilterCheckBox; } }
		internal Label AutoCarverBladeLabel { get { return autocarverbladeLabel; } }
		internal Label BoneBladeLabel { get { return bonebladeLabel; } }
		internal RazorAgentNumOnlyTextBox RemountDelay { get { return remountdelay; } }
		internal RazorAgentNumOnlyTextBox RemountEDelay { get { return remountedelay; } }
		internal Label RemountSerialLabel { get { return remountseriallabel; } }
		internal RazorCheckBox RemountCheckbox { get { return remountcheckbox; } }
		internal RazorCheckBox ShowHeadTargetCheckBox { get { return showheadtargetCheckBox; } }
		internal RazorCheckBox BlockHealPoisonCheckBox { get { return blockhealpoisonCheckBox; } }
		internal RazorCheckBox ColorFlagsHighlightCheckBox { get { return colorflagsHighlightCheckBox; } }
		internal RazorCheckBox BlockMiniHealCheckBox { get { return blockminihealCheckBox; } }
		internal RazorCheckBox BlockBigHealCheckBox { get { return blockbighealCheckBox; } }
		internal RazorCheckBox BlockChivalryHealCheckBox { get { return blockchivalryhealCheckBox; } }
		internal RazorCheckBox ShowMessageFieldCheckBox { get { return showmessagefieldCheckBox; } }
		internal RazorCheckBox ShowAgentMessageCheckBox { get { return showagentmessageCheckBox; } }
		internal RazorCheckBox ColorFlagsSelfHighlightCheckBox { get { return colorflagsselfHighlightCheckBox; } }

		private void autocarverrazorButton_Click(object sender, EventArgs e)
		{
			AutoCarverSetBlade();
		}

		internal void AutoCarverSetBlade()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select Auto Carver Blade", false);

			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(autocarverbladeTarget_Callback));
		}

		private void autocarverbladeTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item blade = Assistant.World.FindItem(serial);

			if (blade == null)
				return;

			if (blade != null && blade.Serial.IsItem && blade.RootContainer == Assistant.World.Player)
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("AutoCarve Blade Set to: " + blade.ToString(), false);
				RazorEnhanced.Filters.AutoCarverBlade = (int)blade.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Invalid AutoCarve Blade", false);
				RazorEnhanced.Filters.AutoCarverBlade = 0;
			}

			RazorEnhanced.Settings.General.WriteInt("AutoCarverBladeLabel", RazorEnhanced.Filters.AutoCarverBlade);
		}

		private void boneCutterrazorButton_Click(object sender, EventArgs e)
		{
			BoneCutterSetBlade();
		}
		internal void BoneCutterSetBlade()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select Bone Cutter Blade", false);

			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(bonecutterbladeTarget_Callback));
		}

		private void bonecutterbladeTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item blade = Assistant.World.FindItem(serial);

			if (blade == null)
				return;

			if (blade != null && blade.Serial.IsItem && blade.RootContainer == Assistant.World.Player)
			{
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("BoneCutter Blade Set to: " + blade.ToString(), false);
				RazorEnhanced.Filters.BoneCutterBlade = (int)blade.Serial.Value;
			}
			else
			{
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("Invalid BoneCutter Blade", false);
				RazorEnhanced.Filters.BoneCutterBlade = 0;
			}

			Settings.General.WriteInt("BoneBladeLabel", RazorEnhanced.Filters.BoneCutterBlade);
		}

		private void autocarverCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (autocarverCheckBox.Checked)
			{
				RazorEnhanced.Filters.AutoCarver = true;
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("AutoCarver Engine Start...", false);
			}
			else
			{
				RazorEnhanced.Filters.AutoCarver = false;
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("AutoCarver Engine Stop...", false);
			}

			if (autocarverCheckBox.Focused)
				Settings.General.WriteBool("AutoCarverCheckBox", autocarverCheckBox.Checked);
		}

		private void bonecutterCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bonecutterCheckBox.Checked)
			{
				RazorEnhanced.Filters.BoneCutter = true;
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("BoneCutter Engine Start...", false);
			}
			else
			{
				RazorEnhanced.Filters.BoneCutter = false;
				if (showagentmessageCheckBox.Checked)
					Misc.SendMessage("BoneCutter Engine Stop...", false);
			}

			if (bonecutterCheckBox.Focused)
				Settings.General.WriteBool("BoneCutterCheckBox", bonecutterCheckBox.Checked);
		}

		private void highlighttargetCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (highlighttargetCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("HighlightTargetCheckBox", highlighttargetCheckBox.Checked);
		}

		private void flagsHighlightCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (flagsHighlightCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("FlagsHighlightCheckBox", flagsHighlightCheckBox.Checked);
		}

		private void showstaticfieldCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showstaticfieldCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowStaticFieldCheckBox", showstaticfieldCheckBox.Checked);
		}

		private void blocktraderequestCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (blocktraderequestCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockTradeRequestCheckBox", blocktraderequestCheckBox.Checked);
		}

		private void blockpartyinviteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (blockpartyinviteCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockPartyInviteCheckBox", blockpartyinviteCheckBox.Checked);
		}

		private void showheadtargetCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showheadtargetCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowHeadTargetCheckBox", showheadtargetCheckBox.Checked);
		}

		private void blockhealpoisonCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (blockhealpoisonCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockHealPoison", blockhealpoisonCheckBox.Checked);
		}

		private void colorflagsHighlightCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (colorflagsHighlightCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("ColorFlagsHighlightCheckBox", colorflagsHighlightCheckBox.Checked);
		}

		private void colorflagsselfHighlightCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (colorflagsselfHighlightCheckBox.Focused)
			{
				if (World.Player != null)
				{
					if (colorflagsselfHighlightCheckBox.Checked)

						RazorEnhanced.Filters.ApplyColor(World.Player);
					else
						RazorEnhanced.Filters.Decolorize(World.Player);
				}
				RazorEnhanced.Settings.General.WriteBool("ColorFlagsSelfHighlightCheckBox", colorflagsselfHighlightCheckBox.Checked);
			}
		}

		private void blockminihealCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (blockminihealCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockMiniHealCheckBox", blockminihealCheckBox.Checked);
		}

		private void blockbighealCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (blockbighealCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockBigHealCheckBox", blockbighealCheckBox.Checked);
		}

		private void blockchivalryhealCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (blockchivalryhealCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockChivalryHealCheckBox", blockchivalryhealCheckBox.Checked);
		}

		private void showmessagefieldCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showmessagefieldCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowMessageFieldCheckBox", showmessagefieldCheckBox.Checked);
		}

		private void showagentmessageCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showagentmessageCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowAgentMessageCheckBox", showagentmessageCheckBox.Checked);
		}

		private void mobfilterCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			graphfilterdatagrid.Enabled = !mobfilterCheckBox.Checked;
			RazorEnhanced.Filters.ReloadGraphFilterData(); // Ricarico tabella in memoria
			if (showagentmessageCheckBox.Checked)
			{
				if (mobfilterCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("Graphics changer filter: ENABLED!", false);
				else
					RazorEnhanced.Misc.SendMessage("Graphics changer filter: DISABLED!", false);
			}

			if (mobfilterCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("MobFilterCheckBox", mobfilterCheckBox.Checked);
		}

		private void graphfilterdatagrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = graphfilterdatagrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 3)
			{
				cell.Value = Utility.FormatDatagridColorGraphCell(cell);
			}
			else if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
			{
				cell.Value = Utility.FormatDatagridItemIDCell(cell);
			}
			RazorEnhanced.Filters.CopyGraphTable();
		}

		private void graphfilterdatagrid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = false;
			e.Row.Cells[1].Value = "0x0000";
			e.Row.Cells[2].Value = "0x0000";
			e.Row.Cells[3].Value = "No Change";
		}

		private void remountdelay_Leave(object sender, EventArgs e)
		{
			if (remountdelay.Text == String.Empty)
				remountdelay.Text = "100";

			RazorEnhanced.Filters.AutoRemountDelay = Convert.ToInt32(remountdelay.Text);
			Settings.General.WriteInt("MountDelay", RazorEnhanced.Filters.AutoRemountDelay);
		}

		private void remountedelay_Leave(object sender, EventArgs e)
		{
			if (remountedelay.Text == String.Empty)
				remountedelay.Text = "100";

			RazorEnhanced.Filters.AutoRemountEDelay = Convert.ToInt32(remountedelay.Text);
			Settings.General.WriteInt("EMountDelay", RazorEnhanced.Filters.AutoRemountEDelay);
		}

		private void remountcheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (remountcheckbox.Checked)
			{
				RazorEnhanced.Filters.AutoModeRemount = true;
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("AutoRemount Engine Start...", false);
			}
			else
			{
				RazorEnhanced.Filters.AutoModeRemount = false;
				if (showagentmessageCheckBox.Checked)
					RazorEnhanced.Misc.SendMessage("AutoRemount Engine Stop...", false);
			}

			if (remountcheckbox.Focused)
				RazorEnhanced.Settings.General.WriteBool("RemountCheckbox", remountcheckbox.Checked);
		}

		private void remountsetbutton_Click(object sender, EventArgs e)
		{
			AutoRemountSetMount();
		}

		internal void AutoRemountSetMount()
		{
			if (showagentmessageCheckBox.Checked)
				Misc.SendMessage("Select mount or item.", false);

			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(remountSetMountTarget_Callback));
		}

		private void remountSetMountTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			if (serial != 0)
			{
				RazorEnhanced.Filters.AutoRemountSerial = serial;
				RazorEnhanced.Settings.General.WriteInt("MountSerial", serial);
			}
		}
	}
}
