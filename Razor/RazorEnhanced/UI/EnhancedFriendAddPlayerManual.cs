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
                var dialogResult = RazorEnhanced.UI.RE_MessageBox.Show("Invalid Player Name",
                        $"Empty player name is invalid\r\nSpecify a name",
                        ok: "Ok", no: null, cancel: null, backColor: null);
                fail = true;
            }

            try
            {
                serial = Convert.ToInt32(tSerial.Text, 16);
            }
            catch
            {
                var dialogResult = RazorEnhanced.UI.RE_MessageBox.Show("Invalid Player Serial",
                        $"Player serial: {tSerial.Text} is invalid",
                        ok: "Ok", no: null, cancel: null, backColor: null);

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