using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


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
			if (RazorEnhanced.Shard.Exists(newList))
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
				RazorEnhanced.Shard.Insert(newList, "Not set", "Not Set", "0.0.0.0", "0", false, false);
				this.Close();
			}
		}
	}
}
