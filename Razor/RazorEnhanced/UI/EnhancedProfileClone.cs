using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedProfileClone : Form
	{
		private const string m_Title = "Enhanced Profile Clone";

		public EnhancedProfileClone()
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
			string newprofileName = String.Empty;

			if (profilename.Text == String.Empty)
				fail = true;

			if (!Regex.IsMatch(profilename.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newprofileName = profilename.Text.ToLower();
			if (RazorEnhanced.Profiles.Exist(newprofileName))
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
				string oldprofileName = RazorEnhanced.Profiles.LastUsed();
				////////////////////
				string dest = Path.Combine(Assistant.Engine.RootPath, "Profiles", newprofileName);
				System.IO.Directory.CreateDirectory(dest);

				string src = Path.Combine(Assistant.Engine.RootPath, "Profiles", oldprofileName);

				try
				{
					DirectoryInfo d = new DirectoryInfo(src);
					FileInfo[] Files = d.GetFiles("*");
					foreach (FileInfo file in Files)
					{
						File.Copy(file.FullName, Path.Combine(dest, file.Name), true);
					}
					RazorEnhanced.Profiles.Add(newprofileName);
					RazorEnhanced.Profiles.Refresh();
				}
				catch
				{
					MessageBox.Show("Error during cloning profile!",
					"Enhanced Profiles",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
				}

				this.Close();
			}
		}

		private void EnhancedProfileAdd_Load(object sender, EventArgs e)
		{
			cloneNameLabel.Text = "Old Name: " + RazorEnhanced.Profiles.LastUsed();
		}
	}
}