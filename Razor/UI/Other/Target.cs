using JsonData;
using RazorEnhanced;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		string lasttargetselected = string.Empty;

		private void EnableTargetGUI()
		{
			groupBox44.Enabled = groupBox45.Enabled = groupBox48.Enabled = groupBox57.Enabled = groupBox46.Enabled =
			groupBox56.Enabled = groupBox55.Enabled = targetTestButton.Enabled = targetsaveButton.Enabled = true;
		}

		private void DisableTargetGUI()
		{
			groupBox44.Enabled = groupBox45.Enabled = groupBox48.Enabled = groupBox57.Enabled = groupBox46.Enabled =
			groupBox56.Enabled = groupBox55.Enabled = targetTestButton.Enabled = targetsaveButton.Enabled = false;
		}

		private void targetlistBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (targetlistBox.SelectedItem == null || targetlistBox.SelectedItem.ToString() == lasttargetselected)
				return;

			EnableTargetGUI();

			lasttargetselected = targetlistBox.SelectedItem.ToString();
			TargetGUI m_targ = Settings.Target.TargetRead(targetlistBox.SelectedItem.ToString());

			targethueGridView.Rows.Clear();
			targetbodydataGridView.Rows.Clear();

			// Selector
			targetSelectorComboBox.Text = m_targ.TargetGuiObject.Selector;

			//Name
			targetNameTextBox.Text = m_targ.TargetGuiObject.Filter.Name;

			// Flags
			if (m_targ.TargetGuiObject.Filter.Poisoned == -1)
				poisonedBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.Poisoned == 1)
				poisonedOn.Checked = true;
			else
				poisonedOff.Checked = true;

			if (m_targ.TargetGuiObject.Filter.Blessed == -1)
				blessedBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.Blessed == 1)
				blessedOn.Checked = true;
			else
				blessedOff.Checked = true;

			if (m_targ.TargetGuiObject.Filter.IsHuman == -1)
				humanBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.IsHuman == 1)
				humanOn.Checked = true;
			else
				humanOff.Checked = true;

			if (m_targ.TargetGuiObject.Filter.IsGhost == -1)
				ghostBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.IsGhost == 1)
				ghostOn.Checked = true;
			else
				ghostOff.Checked = true;

			if (m_targ.TargetGuiObject.Filter.Warmode == -1)
				warmodeBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.Warmode == 1)
				warmodeOn.Checked = true;
			else
				warmodeOff.Checked = true;

			if (m_targ.TargetGuiObject.Filter.Friend == -1)
				friendBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.Friend == 1)
				friendOn.Checked = true;
			else
				friendOff.Checked = true;

			if (m_targ.TargetGuiObject.Filter.Paralized == -1)
				paralizedBoth.Checked = true;
			else if (m_targ.TargetGuiObject.Filter.Paralized == 1)
				paralizedOn.Checked = true;
			else
				paralizedOff.Checked = true;

			// min max range
			targetRangeMaxTextBox.Text = m_targ.TargetGuiObject.Filter.RangeMax.ToString();
			targetRangeMinTextBox.Text = m_targ.TargetGuiObject.Filter.RangeMin.ToString();

			// Body ID
			if (m_targ.TargetGuiObject.Filter.Bodies.Count > 0)
			{
				targetbodyCheckBox.Checked = targetbodydataGridView.Enabled = true;
				foreach (int bodyid in m_targ.TargetGuiObject.Filter.Bodies)
					targetbodydataGridView.Rows.Add(new object[] { "0x" + bodyid.ToString("X4") });
			}
			else
				targetbodyCheckBox.Checked = targetbodydataGridView.Enabled = false;

			// Hue
			if (m_targ.TargetGuiObject.Filter.Hues.Count > 0)
			{
				targetcoloCheckBox.Checked = targethueGridView.Enabled = true;
				foreach (int hue in m_targ.TargetGuiObject.Filter.Hues)
					targethueGridView.Rows.Add(new object[] { "0x" + hue.ToString("X4") });
			}
			else
				targetcoloCheckBox.Checked = targethueGridView.Enabled = false;

			// noto
			targetBlueCheckBox.Checked = targetGreenCheckBox.Checked = targetGreyCheckBox.Checked = targetCriminalCheckBox.Checked =
			targetOrangeCheckBox.Checked = targetRedCheckBox.Checked = targetYellowCheckBox.Checked = false;

			foreach (int noto in m_targ.TargetGuiObject.Filter.Notorieties)
			{
				switch (noto)
				{
					case 0x01:
						targetBlueCheckBox.Checked = true;
						break;
					case 0x02:
						targetGreenCheckBox.Checked = true;
						break;
					case 0x03:
						targetGreyCheckBox.Checked = true;
						break;
					case 0x04:
						targetCriminalCheckBox.Checked = true;
						break;
					case 0x05:
						targetOrangeCheckBox.Checked = true;
						break;
					case 0x06:
						targetRedCheckBox.Checked = true;
						break;
					case 0x07:
						targetYellowCheckBox.Checked = true;
						break;
				}
			}
		}

		private void targetbodyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			targetbodydataGridView.Enabled = targetbodyCheckBox.Checked;
		}

		private void targetcoloCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			targethueGridView.Enabled = targetcoloCheckBox.Checked;
		}

		private void targetbodydataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = targetbodydataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 0)
				cell.Value = Utility.FormatDatagridItemIDCell(cell);
		}

		private void targetfilter_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = "0x0000";
		}

		private void targethueGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = targethueGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 0)
				cell.Value = Utility.FormatDatagridItemIDCell(cell);
		}

		private void SaveTarget()
		{
			List<int> bodylist = new List<int>();
			List<int> huelist = new List<int>();
			List<byte> notolist = new List<byte>();
			int maxrange = -1;
			int minrange = -1;

			// body list
			if (targetbodyCheckBox.Checked)
				foreach (DataGridViewRow row in targetbodydataGridView.Rows)
				{
					if (row.IsNewRow)
						continue;
					bodylist.Add(Convert.ToInt32(row.Cells[0].Value.ToString(), 16));
				}

			// hue list
			if (targetcoloCheckBox.Checked)
				foreach (DataGridViewRow row in targethueGridView.Rows)
				{
					if (row.IsNewRow)
						continue;
					huelist.Add(Convert.ToInt32(row.Cells[0].Value.ToString(), 16));
				}

			// max range
			if (Int32.TryParse(targetRangeMaxTextBox.Text, out maxrange))
			{
				if (maxrange < -1)
					maxrange = -1;
			}
			else
			{
				maxrange = -1;
				targetRangeMaxTextBox.Text = "-1";
			}

			// min range
			if (Int32.TryParse(targetRangeMinTextBox.Text, out minrange))
			{
				if (minrange < -1)
					minrange = -1;
			}
			else
			{
				minrange = -1;
				targetRangeMinTextBox.Text = "-1";
			}

			// notocolor

			if (targetBlueCheckBox.Checked)
				notolist.Add(0x01);

			if (targetGreenCheckBox.Checked)
				notolist.Add(0x02);

			if (targetGreyCheckBox.Checked)
				notolist.Add(0x03);

			if (targetCriminalCheckBox.Checked)
				notolist.Add(0x04);

			if (targetOrangeCheckBox.Checked)
				notolist.Add(0x05);

			if (targetRedCheckBox.Checked)
				notolist.Add(0x06);

			if (targetYellowCheckBox.Checked)
				notolist.Add(0x07);

			// Genero filtro da salvare
			Mobiles.Filter filtertosave = new Mobiles.Filter
			{
				Enabled = true,
				Bodies = bodylist,
				Name = targetNameTextBox.Text,
				Hues = huelist,
				RangeMax = maxrange,
				RangeMin = minrange
			};

			if (poisonedBoth.Checked)
				filtertosave.Poisoned = -1;
			else if (poisonedOn.Checked)
				filtertosave.Poisoned = 1;
			else
				filtertosave.Poisoned = 0;

			if (blessedBoth.Checked)
				filtertosave.Blessed = -1;
			else if (blessedOn.Checked)
				filtertosave.Blessed = 1;
			else
				filtertosave.Blessed = 0;

			if (humanBoth.Checked)
				filtertosave.IsHuman = -1;
			else if (humanOn.Checked)
				filtertosave.IsHuman = 1;
			else
				filtertosave.IsHuman = 0;

			if (ghostBoth.Checked)
				filtertosave.IsGhost = -1;
			else if (ghostOn.Checked)
				filtertosave.IsGhost = 1;
			else
				filtertosave.IsGhost = 0;

			filtertosave.Female = -1;

			if (warmodeBoth.Checked)
				filtertosave.Warmode = -1;
			else if (warmodeOn.Checked)
				filtertosave.Warmode = 1;
			else
				filtertosave.Warmode = 0;

			if (friendBoth.Checked)
				filtertosave.Friend = -1;
			else if (friendOn.Checked)
				filtertosave.Friend = 1;
			else
				filtertosave.Friend = 0;

			if (paralizedBoth.Checked)
				filtertosave.Paralized = -1;
			else if (paralizedOn.Checked)
				filtertosave.Paralized = 1;
			else
				filtertosave.Paralized = 0;

			filtertosave.Notorieties = notolist;

			// Genero struttura da salvare
			TargetGUI targettosave = new TargetGUI();
			targettosave.TargetGuiObject.Selector = targetSelectorComboBox.Text;
			targettosave.TargetGuiObject.Filter = Filter.FromMobileFilter(filtertosave);
			targettosave.Name = targetlistBox.SelectedItem.ToString();
			RazorEnhanced.Settings.Target.TargetReplace(targetlistBox.SelectedItem.ToString(), targettosave);

		}
		private void targetsaveButton_Click(object sender, EventArgs e)
		{
			SaveTarget();
		}

		private void targetaddButton_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newtargetid = String.Empty;

			if (targetaddTextBox.Text == String.Empty)
				fail = true;

			if (!System.Text.RegularExpressions.Regex.IsMatch(targetaddTextBox.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newtargetid = targetaddTextBox.Text.ToLower();
			if (RazorEnhanced.Settings.Target.TargetExist(newtargetid))
				fail = true;

			if (fail)
			{
				MessageBox.Show("Invalid Target ID!",
				"Invalid Target ID!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				return;
			}
			Mobiles.Filter filtertoadd = new Mobiles.Filter
			{
				Enabled = true
			};

			TargetGUI targettoadd = new TargetGUI();
			targettoadd.TargetGuiObject.Selector = "Nearest";
			targettoadd.TargetGuiObject.Filter = Filter.FromMobileFilter(filtertoadd);
			targettoadd.Name = newtargetid;

			// Salvo struttura
			RazorEnhanced.Settings.Target.TargetAdd(newtargetid, targettoadd, Keys.None, true);
			Filter.RefreshTargetShortCut(targetlistBox);
			targetlistBox.SelectedIndex = targetlistBox.Items.Count - 1;
			RazorEnhanced.HotKey.Init();
		}

		private void targetremoveButton_Click(object sender, EventArgs e)
		{
			if (targetlistBox.SelectedItem == null || string.IsNullOrEmpty(targetlistBox.SelectedItem.ToString()))
				return;

			RazorEnhanced.Settings.Target.TargetDelete(targetlistBox.SelectedItem.ToString());
			Filter.RefreshTargetShortCut(targetlistBox);
			if (targetlistBox.Items.Count > 0)
				targetlistBox.SelectedIndex = 0;
			else
			{
				lasttargetselected = string.Empty;
				DisableTargetGUI();
				targetlistBox.SelectedIndex = -1;
			}
			RazorEnhanced.HotKey.Init();
		}

		private void targetTestButton_Click(object sender, EventArgs e)
		{
			if (targetlistBox.SelectedItem == null || string.IsNullOrEmpty(targetlistBox.SelectedItem.ToString()))
				return;

			SaveTarget();
			RazorEnhanced.Target.SetLastTargetFromListHotKey(targetlistBox.SelectedItem.ToString());
		}

		private void targetChoseBody_Click(object sender, EventArgs e)
		{
			if (targetbodydataGridView.Enabled)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(targetbuttonChoseBody));
		}

		private void targetbuttonChoseBody(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Mobile mob = Assistant.World.FindMobile(serial);
			if (mob != null)
				targetbodydataGridView.Invoke((MethodInvoker)delegate { targetbodydataGridView.Rows.Add(new object[] { "0x" + mob.Body.ToString("X4") }); });
		}

		private void targetChoseHue_Click(object sender, EventArgs e)
		{
			if (targethueGridView.Enabled)
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(targetbuttonChoseBodyHue));
		}

		private void targetbuttonChoseBodyHue(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Mobile mob = Assistant.World.FindMobile(serial);
			if (mob != null)
				targethueGridView.Invoke((MethodInvoker)delegate { targethueGridView.Rows.Add(new object[] { "0x" + mob.Hue.ToString("X4") }); });

		}
	}
}
