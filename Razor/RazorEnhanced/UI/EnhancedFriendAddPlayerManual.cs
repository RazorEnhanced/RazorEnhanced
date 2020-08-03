using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedFriendAddPlayerManual : Form
	{
		private const string m_Title = "Enhanced Friend Manual Add Player";

		public EnhancedFriendAddPlayerManual()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title; ;
		}

		private void EnhancedFriendManualAdd_Load(object sender, EventArgs e)
		{
			tName.Text = "New Player";
			tSerial.Text = "0x00000000";
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Friend.RefreshPlayers();
			this.Close();
		}

		private void bAddPlayer_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int serial = 0;
			if (tName.Text == null)
			{
				MessageBox.Show("Player name is not valid.",
				"Player name Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			try
			{
				serial = Convert.ToInt32(tSerial.Text, 16);
			}
			catch
			{
				MessageBox.Show("Player Serial is not valid.",
				"Player Serial Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			if (!fail)
			{
				RazorEnhanced.Friend.AddPlayerToList(tName.Text, serial);
				this.Close();
			}
		}
	}
}