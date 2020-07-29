using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedProfileRename : Form
	{
		private const string m_Title = "Enhanced Profile Rename";

		public EnhancedProfileRename()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void close_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void profileadd_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newprofile = String.Empty;

			if (profilename.Text == String.Empty)
				fail = true;

			if (!Regex.IsMatch(profilename.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newprofile = profilename.Text.ToLower();
			if (RazorEnhanced.Profiles.Exist(newprofile))
				fail = true;

			if (fail)
			{
				MessageBox.Show("Invalid profile name!",
				"Enhanced Profiles",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
			}
			else
			{
				RazorEnhanced.Profiles.Rename(RazorEnhanced.Profiles.LastUsed(), newprofile);
				RazorEnhanced.Profiles.Refresh();

				this.Close();
			}
		}

		private void EnhancedProfileAdd_Load(object sender, EventArgs e)
		{
			oldNameLabel.Text = "Old Name: " + RazorEnhanced.Profiles.LastUsed();
		}
	}
}