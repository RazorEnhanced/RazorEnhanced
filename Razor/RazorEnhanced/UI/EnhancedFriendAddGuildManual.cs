using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedFriendAddGuildManual : Form
	{
		private const string m_Title = "Enhanced Friend Manual Add Player";

		public EnhancedFriendAddGuildManual()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title; ;
		}

		private void EnhancedFriendManualAdd_Load(object sender, EventArgs e)
		{
			tName.Text = "New Guild";
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Friend.RefreshGuilds();
			this.Close();
		}

		private void bAddPlayer_Click(object sender, EventArgs e)
		{
			if (tName.Text == null)
			{
				MessageBox.Show("Guild name is not valid.",
				"Guild name Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
			}
			else
			{
				RazorEnhanced.Friend.AddGuildToList(tName.Text);
				this.Close();
			}
		}
	}
}