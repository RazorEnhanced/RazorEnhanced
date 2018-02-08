using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedProfileImport : Form
	{
		private const string m_Title = "Enhanced Profile Import";

		public EnhancedProfileImport()
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

			if (!File.Exists(profilefilepathTextBox.Text))
				fail = true;

			if (fail)
			{
				MessageBox.Show("Invalid file or profile name",
				"Enhanced Profiles",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
			}
			else
			{
				RazorEnhanced.ImportExport.ImportProfiles(newprofile, profilefilepathTextBox.Text);
				this.Close();
			}
		}

		private void chosefileButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Profiles",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				profilefilepathTextBox.Text = od.FileName;
			}
		}
	}
}