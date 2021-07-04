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

				m_proplist = (List<AutoLoot.AutoLootItem.Property>)row.Cells[5].Value;
            }
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			Close();
		}

		internal static List<string> m_default_prop = new List<string>
		{
			"Balanced",
			"Cold Resist",
			"Damage Increase",
			"Defense Chance Increase",
			"Dexterity Bonus",
			"Energy Resists",
			"Faster Cast Recovery",
			"Enhance Potion",
			"Energy Damage",
			"Poison Damage",
			"Fire Damage",
			"Cold Damage",
			"Physical Damage",
			"Faster Casting",
			"Gold Increase",
			"Fire Resist",
			"Hit Chance Increase",
			"Hit Energy Area",
			"Hit Dispel",
			"Hit Cold Area",
			"Hit Fire Area",
			"Hit Fireball",
			"Hit Life Leech",
			"Hit Point Increase",
			"Hit Point Regeneration",
			"Hit Stamina Leech",
			"Hit Poison Area",
			"Hit Physical Area",
			"Hit Mana Leech",
			"Hit Magic Arrow",
			"Hit Lower Defense",
			"Hit Lower Attack",
			"Hit Lightning",
			"Hit Harm",
			"Intelligence Bonus",
			"Lower Mana Cost",
			"Lower Reagent Cost",
			"Lower Requirements",
			"Luck",
			"Mana Increase",
			"Mana Regeneration",
			"Physical Resist",
			"Poison Resist",
			"Night Sight",
			"Spell Channeling",
			"Spell Damage Increase",
			"Splintering Weapon",
			"Stamina Increase",
			"Stamina Regeneration",
			"Swing Speed Increase",
			"Velocity",
			"Balanced",
			"Self Repair",
			"Reflect Physical Damage",
			"Night Sight",
			"Mage Armor",
			"Swing Speed Increase",
			"Strength Bonus",
			"Water Elemental Slayer",
			"Troll Slayer",
			"Undead Slayer",
			"Terathan Slayer",
			"Spider Slayer",
			"Snow Elemental Slayer",
			"Snake Slayer",
			"Scorpion Slayer",
			"Reptile Slayer",
			"Repond Slayer",
			"Poison Elemental Slayer",
			"Orc Slayer",
			"Ophidian Slayer",
			"Ogre Slayer",
			"Lizardman Slayer",
			"Gargoyle Slayer",
			"Fire Elemental Slayer",
			"Elemental Slayer",
			"Earth Elemental Slayer",
			"Dragon Slayer",
			"Demon Slayer",
			"Blood Elemental Slayer",
			"Arachnid Slayer",
			"Air Elemental Slayer",
			"Magic Arrow Charges",
			"Lightning Charges",
			"Healing Charges",
			"Harm Charges",
			"Greater Healing Charges",
			"Fireball Charges",
		};

		private void EnhancedScavengerEditItemProps_Load(object sender, EventArgs e)
		{
			comboboxProp.DataSource = m_default_prop;
			lName.Text = m_name;
			lGraphics.Text = m_graphics;
            lColor.Text = m_color;

			int color = 0;
			int itemid = 0;
			if (m_graphics != "All")
				itemid = Convert.ToInt32(m_graphics, 16);

            if (m_color != "All")
				color = Convert.ToInt32(m_color, 16);

			if (m_proplist != null)
				foreach (AutoLoot.AutoLootItem.Property prop in m_proplist)
				{
					autolootpropGridView.Rows.Add(new object[] { prop.Name, prop.Minimum.ToString(), prop.Maximum.ToString()});
				}

			// Immagine
			Bitmap m_itemimage = Ultima.Art.GetStatic(itemid);
			{
				if (m_itemimage != null && color > 0)
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
				if (cell.Value != null)
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
				if (e.RowIndex != -1)
				{
					autolootpropGridView.Rows[e.RowIndex].Selected = true;
					agentrowindex = e.RowIndex;
					autolootpropGridView.CurrentCell = autolootpropGridView.Rows[e.RowIndex].Cells[1];
					contextMenuStrip1.Show(autolootpropGridView, e.Location);
					contextMenuStrip1.Show(Cursor.Position);
				}
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
				string propname = string.Empty;
				if (row.Cells[0].Value != null)
					propname = row.Cells[0].Value.ToString();

				propslist.Add(new AutoLoot.AutoLootItem.Property(propname, min ,max));
            }
			m_row.Cells[5].Value = propslist;
			AutoLoot.CopyTable();
		}

		private void bAddProp_Click(object sender, EventArgs e)
		{

			if (comboboxProp.Text != String.Empty)
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
