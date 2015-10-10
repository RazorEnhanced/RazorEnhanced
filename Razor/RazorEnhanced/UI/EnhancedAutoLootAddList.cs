using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace RazorEnhanced.UI
{
	public partial class EnhancedAutoLootAddList : Form
	{
		private const string m_Title = "Enhanced Autoloot Add List";

		public EnhancedAutoLootAddList()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void EnhancedAutolootAddItemList_Load(object sender, EventArgs e)
		{
		}

		private void autolootcloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void autolootaddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (autolootListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(autolootListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = autolootListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.AutoLoot.ListExists(newList))
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
				AutoLoot.AddList(newList);
				this.Close();
			}
		}
	}
}
