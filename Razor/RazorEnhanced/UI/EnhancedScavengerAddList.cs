using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace RazorEnhanced.UI
{
	public partial class EnhancedScavengerAddList : Form
	{
		private const string m_Title = "Enhanced Scavenger Add Item List";


		public EnhancedScavengerAddList()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void EnhancedAutolootAddItemList_Load(object sender, EventArgs e)
		{
		}

		private void scavegercloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void scavegeraddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (scavengerListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(scavengerListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = scavengerListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.Scavenger.ListExists(newList))
				fail = true;

			if (fail)
			{
				MessageBox.Show("Invalid list name!",
				"Invalid list name!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}
			else
			{
				Scavenger.AddList(newList);
				this.Close();
			}
		}
	}
}
