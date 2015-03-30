using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Assistant;
using RazorEnhanced;

namespace RazorEnhanced.UI
{
	public partial class EnhancedAutolootEditItemProps : Form
	{
		private const string m_Title = "Enhanced Autoloot Edit Item";

		private string m_List;
		private int m_Index;
		private AutoLoot.AutoLootItem m_Item;

		public EnhancedAutolootEditItemProps(string list, int index, AutoLoot.AutoLootItem item)
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;

			this.m_List = list;
			this.m_Index = index;
			this.m_Item = item;
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			RazorEnhanced.AutoLoot.RefreshItems();
			this.Close();
		}

		private void razorButton3_Click(object sender, EventArgs e)
		{
			for (int i = m_Item.Properties.Count - 1; i >= 0; i--)
			{
				AutoLoot.AutoLootItem.Property prop = m_Item.Properties[i];
				if (comboboxProp.Text == prop.Name)
				{
					m_Item.Properties.RemoveAt(i);
				}
			}

			RefreshGUI();
		}

		private void RefreshGUI()
		{
			listViewProps.Items.Clear();

			foreach (AutoLoot.AutoLootItem.Property prop in m_Item.Properties)
			{
				ListViewItem listitem = new ListViewItem(prop.Name);
				listitem.SubItems.Add(prop.Minimum.ToString());
				listitem.SubItems.Add(prop.Maximum.ToString());
				listViewProps.Items.Add(listitem);
			}

			RazorEnhanced.Settings.AutoLoot.ItemReplace(m_List, m_Index, m_Item);
		}

		private void EnhancedAutolootEditItemProps_Load(object sender, EventArgs e)
		{
			tMax.Text = "1";
			tMin.Text = "1";

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
			comboboxProp.Items.Add("Nighr Sight");
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

			lName.Text = m_Item.Name;
			lGraphics.Text = "0x" + m_Item.Graphics.ToString("X4");
			lColor.Text = "0x" + m_Item.Color.ToString("X4");

			RefreshGUI();
		}

		private void bAddProp_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int Min = 0;
			int Max = 0;
			if (comboboxProp.Text == "")
			{
				MessageBox.Show("Props name is not valid.",
				"Props name Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			try
			{
				Min = Convert.ToInt32(tMin.Text);
			}
			catch
			{
				MessageBox.Show("Minimum props value in not valid.",
				"Props minimum value error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			try
			{
				Max = Convert.ToInt32(tMax.Text);
			}
			catch
			{
				MessageBox.Show("Maxumum props value in not valid.",
				"Props Maxumum value error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			if (!fail)
			{
				RazorEnhanced.AutoLoot.AddPropToItem(m_List, m_Index, m_Item, comboboxProp.Text, Min, Max);
				RefreshGUI();
			}
		}
	}
}
