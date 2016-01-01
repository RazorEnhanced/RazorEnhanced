using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedSellAgentAddList : Form
	{
		private const string m_Title = "Enhanced Sell Add Item List";

		public EnhancedSellAgentAddList()
		{
			InitializeComponent();

			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void sellcloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void selladdItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (sellagentListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(sellagentListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = sellagentListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.SellAgent.ListExists(newList))
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
				SellAgent.AddList(newList);
				this.Close();
			}
		}

		private void EnhancedSellAgentAddItemList_Load(object sender, EventArgs e)
		{
		}
	}
}