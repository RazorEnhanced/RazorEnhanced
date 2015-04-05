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
	public partial class EnhancedLauncherAddShard : Form
	{
		private const string m_Title = "Enhanced Launcher Add Shard";


        public EnhancedLauncherAddShard()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void autolootcloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void autolootaddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (shardAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(shardAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = shardAdd.Text.ToLower();
            if (RazorEnhanced.Settings.Shards.Exists(newList))
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
                RazorEnhanced.Settings.Shards.Insert(newList, "Not set", "Not Set", "0.0.0.0", "0", false, false);            
                this.Close();
			}
		}
	}
}
