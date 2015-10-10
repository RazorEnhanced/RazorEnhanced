using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace RazorEnhanced.UI
{
	public partial class EnhancedOrganizerAddList : Form
	{
		private const string m_Title = "Enhanced Organizer Add Item List";

		public EnhancedOrganizerAddList()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;
		}

		private void EnhancedAutolootAddItemList_Load(object sender, EventArgs e)
		{
		}

		private void organizercloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void organizeraddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (organizerListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(organizerListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = organizerListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.Organizer.ListExists(newList))
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
				Organizer.AddList(newList);
				this.Close();
			}
		}
	}
}
