using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal Label GridLocationLabel { get { return gridlocation_label; } }
		internal RazorCheckBox GridLockCheckBox { get { return gridlock_CheckBox; } }
		internal RazorCheckBox GridOpenLoginCheckBox { get { return gridopenlogin_CheckBox; } }
		internal Label GridVSlotLabel { get { return gridvslot_textbox; } }
		internal Label GridHSlotLabel { get { return gridhslot_textbox; } }
		internal RazorComboBox GridSlotComboBox { get { return gridslot_ComboBox; } }
		internal RazorComboBox GridGroupComboBox { get { return gridgroup_ComboBox; } }
		internal RazorComboBox GridBorderComboBox { get { return gridborder_ComboBox; } }
		internal RazorComboBox GridScriptComboBox { get { return gridscript_ComboBox; } }

		private void gridopen_button_Click(object sender, EventArgs e)
		{
			RazorEnhanced.SpellGrid.Open();
		}

		private void gridclose_button_Click(object sender, EventArgs e)
		{
			RazorEnhanced.SpellGrid.Close();
		}

		private void gridlock_CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (gridlock_CheckBox.Focused)
			{
				SpellGrid.LockUnlock();
				Settings.General.WriteBool("LockGridCheckBox", gridlock_CheckBox.Checked);
			}
		}

		private void gridopenlogin_CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (gridopenlogin_CheckBox.Focused)
				Settings.General.WriteBool("GridOpenLoginCheckBox", gridopenlogin_CheckBox.Checked);
		}

		private void gridslot_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = gridslot_ComboBox.SelectedIndex;
			SpellGrid.SpellGridItem item = Settings.SpellGrid.ReadSelectedItem(index);
			gridgroup_ComboBox.SelectedIndex = gridgroup_ComboBox.Items.IndexOf(item.Group);
			if (item.Group != "Empty")
			{
				gridborder_ComboBox.Enabled = true;

				gridborder_ComboBox.SelectedIndex = gridborder_ComboBox.Items.IndexOf(item.Color.Name);
				if (item.Group != "Script")
				{
					gridspell_ComboBox.SelectedIndex = gridspell_ComboBox.Items.IndexOf(item.Spell);
					gridspell_ComboBox.Enabled = true;
					gridscript_ComboBox.Enabled = false;
					gridscript_ComboBox.SelectedIndex = -1;
				}
				else
				{
					gridscript_ComboBox.Enabled = true;
					gridscript_ComboBox.SelectedIndex = gridscript_ComboBox.Items.IndexOf(item.Spell);
					gridspell_ComboBox.Enabled = false;
					gridspell_ComboBox.SelectedIndex = -1;
				}
			}
			else
			{
				Settings.SpellGrid.UpdateItem(gridslot_ComboBox.SelectedIndex, gridgroup_ComboBox.Text, gridspell_ComboBox.Text, System.Drawing.Color.Transparent);
				gridspell_ComboBox.SelectedIndex = -1;
				gridborder_ComboBox.SelectedIndex = -1;
				gridspell_ComboBox.Enabled = false;
				gridborder_ComboBox.Enabled = false;
			}

			if (gridslot_ComboBox.Focused)
				SpellGrid.Open();
		}

		private void gridgroup_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (gridgroup_ComboBox.Text)
			{
				case "Empty":
					{
						gridspell_ComboBox.DataSource = null;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = false;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = -1;
						gridscript_ComboBox.Enabled = false;
						if (gridgroup_ComboBox.Focused)
							SpellGrid.Close();
						break;
					}
				case "Magery":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconMagery.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Abilities":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconAbilities.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Mastery":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconMastery.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Bushido":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconBushido.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Chivalry":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconChivalry.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Necromancy":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconNecromancy.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Ninjitsu":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconNinjitsu.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Mysticism":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconMysticism.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Spellweaving":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SpellIconSpellweaving.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Skills":
					{
						gridspell_ComboBox.DataSource = SpellGrid.SkillsIcon.Keys.ToList();
						gridscript_ComboBox.Enabled = false;
						gridspell_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = 0;
						break;
					}
				case "Script":
					{
						gridscript_ComboBox.Enabled = gridborder_ComboBox.Enabled = true;
						gridspell_ComboBox.Enabled = false;
						gridspell_ComboBox.SelectedIndex = gridborder_ComboBox.SelectedIndex = -1;
						break;
					}
				default:
					break;
			}
			if (gridgroup_ComboBox.Focused)
			{
				UpdateGridItem();
				SpellGrid.Open();
			}
		}

		private void gridspell_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (gridspell_ComboBox.Focused)
			{
				gridborder_ComboBox.SelectedIndex = 0;
				UpdateGridItem();
				SpellGrid.Open();
			}
		}

		private void gridborder_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (gridborder_ComboBox.Focused)
			{
				UpdateGridItem();
				SpellGrid.Open();
			}
		}

		private void UpdateGridItem()
		{
			Color c = Color.Transparent;
			if (gridborder_ComboBox.SelectedItem != null)
				c = Color.FromName(gridborder_ComboBox.SelectedItem.ToString());

			if (gridgroup_ComboBox.Text != "Script")
				Settings.SpellGrid.UpdateItem(gridslot_ComboBox.SelectedIndex, gridgroup_ComboBox.Text, gridspell_ComboBox.Text, c);
			else
				Settings.SpellGrid.UpdateItem(gridslot_ComboBox.SelectedIndex, gridgroup_ComboBox.Text, gridscript_ComboBox.Text, c);
		}

		private void gridscript_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (gridscript_ComboBox.Focused)
			{
				UpdateGridItem();
				SpellGrid.Open();
			}
		}

		private void gridvslotadd_button_Click(object sender, EventArgs e)
		{
			if (SpellGrid.VSlot + SpellGrid.HSlot == 99)
				return;

			int slot = RazorEnhanced.Settings.General.ReadInt("GridVSlot");
			slot += 1;

			RazorEnhanced.Settings.General.WriteInt("GridVSlot", slot);
			RazorEnhanced.SpellGrid.VSlot = slot;
			gridvslot_textbox.Text = slot.ToString();
			RazorEnhanced.SpellGrid.UpdateBox();
			RazorEnhanced.SpellGrid.Close();
			RazorEnhanced.SpellGrid.Open();
		}

		private void gridvslotremove_button_Click(object sender, EventArgs e)
		{
			int slot = RazorEnhanced.Settings.General.ReadInt("GridVSlot");
			if (slot - 1 > 0)
			{
				slot -= 1;
				RazorEnhanced.Settings.General.WriteInt("GridVSlot", slot);
				RazorEnhanced.SpellGrid.VSlot = slot;
				gridvslot_textbox.Text = slot.ToString();
				RazorEnhanced.SpellGrid.UpdateBox();
				RazorEnhanced.SpellGrid.Close();
				RazorEnhanced.SpellGrid.Open();
			}
		}

		private void gridhslotadd_button_Click(object sender, EventArgs e)
		{
			if (SpellGrid.VSlot + SpellGrid.HSlot == 99)
				return;

			int slot = RazorEnhanced.Settings.General.ReadInt("GridHSlot");
			slot += 1;

			RazorEnhanced.Settings.General.WriteInt("GridHSlot", slot);
			RazorEnhanced.SpellGrid.HSlot = slot;
			gridhslot_textbox.Text = slot.ToString();
			RazorEnhanced.SpellGrid.UpdateBox();
			RazorEnhanced.SpellGrid.Close();
			RazorEnhanced.SpellGrid.Open();
		}

		private void gridhslotremove_button_Click(object sender, EventArgs e)
		{
			int slot = RazorEnhanced.Settings.General.ReadInt("GridHSlot");
			if (slot - 1 > 0)
			{
				slot -= 1;
				RazorEnhanced.Settings.General.WriteInt("GridHSlot", slot);
				RazorEnhanced.SpellGrid.HSlot = slot;
				gridhslot_textbox.Text = slot.ToString();
				RazorEnhanced.SpellGrid.UpdateBox();
				RazorEnhanced.SpellGrid.Close();
				RazorEnhanced.SpellGrid.Open();
			}
		}

		private void spellgrid_trackBar_Scroll(object sender, EventArgs e)
		{
			int o = spellgrid_trackBar.Value;

			if (spellgrid_trackBar.Focused)
			{
				RazorEnhanced.Settings.General.WriteInt("GridOpacity", o);
				if (RazorEnhanced.SpellGrid.SpellGridForm != null)
				{
					RazorEnhanced.SpellGrid.SpellGridForm.Opacity = ((double)o) / 100.0;
					RazorEnhanced.SpellGrid.SpellGridForm.Show();
				}
			}

			spellgrid_opacity_label.Text = String.Format("{0}%", o);
		}
	}
}
