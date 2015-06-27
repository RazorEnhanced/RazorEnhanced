using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;


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
            string newprofile = "";

			if (profilename.Text == "")
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
                string oldprofilepath;
                if (RazorEnhanced.Profiles.LastUsed() == "default")
                    oldprofilepath = Path.Combine(Directory.GetCurrentDirectory(), "RazorEnhanced.settings");
                else
                    oldprofilepath = Path.Combine(Directory.GetCurrentDirectory(), "RazorEnhanced." + RazorEnhanced.Profiles.LastUsed() + ".settings");
                string newprofilepath = Path.Combine(Directory.GetCurrentDirectory(), "RazorEnhanced." + newprofile + ".settings");

                if (File.Exists(oldprofilepath))
                {
                    File.Copy(oldprofilepath, newprofilepath, true);
                    RazorEnhanced.Profiles.Add(newprofile);
                    RazorEnhanced.Profiles.Refresh();
                }
                else
                {
                    MessageBox.Show("Error during clonig profile!",
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
