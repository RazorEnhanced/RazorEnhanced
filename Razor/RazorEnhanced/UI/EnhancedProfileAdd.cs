using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedProfileAdd : Form
	{
		private const string m_Title = "Enhanced Profile Add";

		public EnhancedProfileAdd()
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
				RazorEnhanced.Profiles.Add(newprofile);
				RazorEnhanced.Profiles.SetLast(newprofile);
				RazorEnhanced.Profiles.Refresh();
				RazorEnhanced.Profiles.ProfileChange(newprofile);
				this.Close();
			}
		}

		private void EnhancedProfileAdd_Load(object sender, EventArgs e)
		{
		}
	}
}