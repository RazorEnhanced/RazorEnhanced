using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace RazorEnhanced.UI
{
	public partial class EnhancedAutolootEditItemProps : Form
	{
		private List<AutoLoot.AutoLootItem.Property> m_proplist = new List<AutoLoot.AutoLootItem.Property>();
		private string m_name;
		private string m_graphics;
		private string m_color;
		DataGridViewRow m_row;
		public EnhancedAutolootEditItemProps(ref DataGridViewRow row)
		{
			m_row = row;
            InitializeComponent();
			MaximizeBox = false;
			if (row != null)
			{
				m_name = (string)row.Cells[1].Value;
				m_graphics = (string)row.Cells[2].Value;
				m_color = (string)row.Cells[3].Value;

				m_proplist = (List<AutoLoot.AutoLootItem.Property>)row.Cells[4].Value;
            }
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void EnhancedScavengerEditItemProps_Load(object sender, EventArgs e)
		{
			// Popola combobox props
			comboboxProp.Items.Add("Balanced");
			comboboxProp.Items.Add("Cold Resist");
			comboboxProp.Items.Add("Damage Increase");
			comboboxProp.Items.Add("Defense Chance Increase");
			comboboxProp.Items.Add("Dexterity Bonus");
			comboboxProp.Items.Add("Energy Resists");
			comboboxProp.Items.Add("Faster Cast Recovery");
			comboboxProp.Items.Add("Enhance Potion");
			comboboxProp.Items.Add("Energy Damage");
			comboboxProp.Items.Add("Poison Damage");
			comboboxProp.Items.Add("Fire Damage");
			comboboxProp.Items.Add("Cold Damage");
			comboboxProp.Items.Add("Physical Damage");
			comboboxProp.Items.Add("Faster Casting");
			comboboxProp.Items.Add("Gold Increase");
			comboboxProp.Items.Add("Fire Resist");
			comboboxProp.Items.Add("Hit Chance Increase");
			comboboxProp.Items.Add("Hit Energy Area");
			comboboxProp.Items.Add("Hit Dispel");
			comboboxProp.Items.Add("Hit Cold Area");
			comboboxProp.Items.Add("Hit Fire Area");
			comboboxProp.Items.Add("Hit Fireball");
			comboboxProp.Items.Add("Hit Life Leech");
			comboboxProp.Items.Add("Hit Point Increase");
			comboboxProp.Items.Add("Hit Point Regeneration");
			comboboxProp.Items.Add("Hit Stamina Leech");
			comboboxProp.Items.Add("Hit Poison Area");
			comboboxProp.Items.Add("Hit Physical Area");
			comboboxProp.Items.Add("Hit Mana Leech");
			comboboxProp.Items.Add("Hit Magic Arrow");
			comboboxProp.Items.Add("Hit Lower Defence");
			comboboxProp.Items.Add("Hit Lower Attack");
			comboboxProp.Items.Add("Hit Lightning");
			comboboxProp.Items.Add("Hit Harm");
			comboboxProp.Items.Add("Intelligence Bonus");
			comboboxProp.Items.Add("Lower Mana Cost");
			comboboxProp.Items.Add("Lower Reagent Cost");
			comboboxProp.Items.Add("Lower Requirements");
			comboboxProp.Items.Add("Luck");
			comboboxProp.Items.Add("Mana Increase");
			comboboxProp.Items.Add("Mana Regeneration");
			comboboxProp.Items.Add("Physical Resist");
			comboboxProp.Items.Add("Poison Resist");
			comboboxProp.Items.Add("Night Sight");
			comboboxProp.Items.Add("Spell Channeling");
			comboboxProp.Items.Add("Spell Damage Increase");
			comboboxProp.Items.Add("Splintering Weapon");
			comboboxProp.Items.Add("Stamina Increase");
			comboboxProp.Items.Add("Stamina Regeneration");
			comboboxProp.Items.Add("Swing Speed Increase");
			comboboxProp.Items.Add("Velocity");
			comboboxProp.Items.Add("Balanced");
			comboboxProp.Items.Add("Self Repair");
			comboboxProp.Items.Add("Reflect Physical Damage");
			comboboxProp.Items.Add("Night Sight");
			comboboxProp.Items.Add("Mage Armor");
			comboboxProp.Items.Add("Swing Speed Increase");
			comboboxProp.Items.Add("Strenght Bonus");
			comboboxProp.Items.Add("Water Elemental Slayer");
			comboboxProp.Items.Add("Troll Slayer");
			comboboxProp.Items.Add("Undead Slayer");
			comboboxProp.Items.Add("Terathan Slayer");
			comboboxProp.Items.Add("Spider Slayer");
			comboboxProp.Items.Add("Snow Elemental Slayer");
			comboboxProp.Items.Add("Snake Slayer");
			comboboxProp.Items.Add("Scorpion Slayer");
			comboboxProp.Items.Add("Reptile Slayer");
			comboboxProp.Items.Add("Repond Slayer");
			comboboxProp.Items.Add("Poison Elemental Slayer");
			comboboxProp.Items.Add("Orc Slayer");
			comboboxProp.Items.Add("Ophidian Slayer");
			comboboxProp.Items.Add("Ogre Slayer");
			comboboxProp.Items.Add("Lizardman Slayer");
			comboboxProp.Items.Add("Gargoyle Slayer");
			comboboxProp.Items.Add("Fire Elemental Slayer");
			comboboxProp.Items.Add("Elemental Slayer");
			comboboxProp.Items.Add("Earth Elemental Slayer");
			comboboxProp.Items.Add("Dragon Slayer");
			comboboxProp.Items.Add("Demon Slayer");
			comboboxProp.Items.Add("Blood Elemental Slayer");
			comboboxProp.Items.Add("Arachnid Slayer");
			comboboxProp.Items.Add("Air Elemental Slayer");
			comboboxProp.Items.Add("Magic Arrow Charges");
			comboboxProp.Items.Add("Lightning Charges");
			comboboxProp.Items.Add("Healing Charges");
			comboboxProp.Items.Add("Harm Charges");
			comboboxProp.Items.Add("Greater Healing Charges");
			comboboxProp.Items.Add("Fireball Charges");

			lName.Text = m_name;
			lGraphics.Text = m_graphics;
            lColor.Text = m_color;

			int color = 0;
            int itemid = Convert.ToInt32(m_graphics, 16);
            if (m_color != "All")
				color = Convert.ToInt32(m_color, 16);

			if (m_proplist != null)
				foreach (AutoLoot.AutoLootItem.Property prop in m_proplist)
				{
					autolootpropGridView.Rows.Add(new object[] { prop.Name, prop.Minimum.ToString(), prop.Maximum.ToString()});
				}

			// Immagine
			Bitmap m_itemimage = new Bitmap(Ultima.Art.GetStatic(itemid));
			{
				if (color > 0)
				{
					bool onlyHueGrayPixels = (color & 0x8000) != 0;
					color = (color & 0x3FFF) - 1;
					Ultima.Hue m_hue = Ultima.Hues.GetHue(color);
					m_hue.ApplyTo(m_itemimage, onlyHueGrayPixels);
				}
				imagepanel.BackgroundImage = m_itemimage;
			}

		}

		private void autolootpropGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = autolootpropGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
			{
				int propvalue = 0;
				Int32.TryParse(cell.Value.ToString(), out propvalue);

				if (propvalue < 0 || propvalue > 999)
					propvalue =0;

				cell.Value = propvalue.ToString();
			}

			SaveData();
		}

		private void autolootpropGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				autolootpropGridView.Rows[e.RowIndex].Selected = true;
				agentrowindex = e.RowIndex;
				autolootpropGridView.CurrentCell = autolootpropGridView.Rows[e.RowIndex].Cells[1];
				contextMenuStrip1.Show(autolootpropGridView, e.Location);
				contextMenuStrip1.Show(Cursor.Position);
			}
		}

		private void autolootpropGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.ThrowException = false;
			e.Cancel = false;
		}

		private void autolootpropGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[0].Value = "New Prop";
			e.Row.Cells[1].Value = "0";
            e.Row.Cells[2].Value = "0";
        }

		private void SaveData()
		{
			List<AutoLoot.AutoLootItem.Property> propslist = new List<AutoLoot.AutoLootItem.Property>();
			foreach (DataGridViewRow row in autolootpropGridView.Rows)
			{
				if (row.IsNewRow)
					continue;
				int min = Convert.ToInt32((string)row.Cells[1].Value);
				int max = Convert.ToInt32((string)row.Cells[2].Value);
                propslist.Add(new AutoLoot.AutoLootItem.Property(row.Cells[0].Value.ToString(),min ,max));
            }
			m_row.Cells[4].Value = propslist;
			Scavenger.CopyTable();
		}

		private void bAddProp_Click(object sender, EventArgs e)
		{

			if (comboboxProp.Text != "")
			{
				autolootpropGridView.Rows.Add(new object[] { comboboxProp.Text, "1", "1" });
				SaveData();
			}
		}

		// ----------------- GESTIONE MENU TENDINA -------------------

		private static int agentrowindex = 0;

		private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!autolootpropGridView.Rows[agentrowindex].IsNewRow)
			{
				autolootpropGridView.Rows.RemoveAt(agentrowindex);
				SaveData();
			}
		}

		// ----------------- END GESTIONE MENU TENDINA -------------------
	}
}