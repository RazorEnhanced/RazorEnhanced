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
	public partial class EnhancedProfileAdd : Form
	{
		private const string m_Title = "Enhanced Add Profile";

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
                "Invalid profile name!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
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
	}
}
