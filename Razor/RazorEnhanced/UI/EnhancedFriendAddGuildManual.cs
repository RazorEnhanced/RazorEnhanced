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
                var dialogResult = RazorEnhanced.UI.RE_MessageBox.Show("Invalid Guild Name",
                        $"Empty guild name is invalid\r\nSpecify a name",
                        ok: "Ok", no: null, cancel: null, backColor: null);
            }
            else
            {
                RazorEnhanced.Friend.AddGuildToList(tName.Text);
                this.Close();
            }
        }
    }
}