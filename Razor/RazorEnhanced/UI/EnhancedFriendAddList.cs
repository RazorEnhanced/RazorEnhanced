using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedFriendAddList : Form
	{
		private const string m_Title = "Enhanced Friends Add List";

		public EnhancedFriendAddList()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void friendclosePlayerList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void friendaddPlayerList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (friendsListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(friendsListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = friendsListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.Friend.ListExists(newList))
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
				Friend.AddList(newList);
				this.Close();
			}
		}
	}
}