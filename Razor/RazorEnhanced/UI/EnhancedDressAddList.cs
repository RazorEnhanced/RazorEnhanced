using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedDressAddList : Form
	{
		private const string m_Title = "Enhanced Dress Add List";

		public EnhancedDressAddList()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void dresscloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void dressaddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (dressListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(dressListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = dressListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.Dress.ListExists(newList))
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
				Dress.AddList(newList);
				this.Close();
			}
		}
	}
}