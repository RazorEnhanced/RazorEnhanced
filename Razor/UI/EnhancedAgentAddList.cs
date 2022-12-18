using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
    public partial class EnhancedAgentAddList : Form
    {
        // Agent ID
        // 1- Autoloot
        // 2- Scavenger
        // 3- Organizer
        // 4- Buy
        // 5- Sell
        // 6- Dress
        // 7- Friend
        // 8- Restock
        // 9- Shard
        // 10 - Clone autoloot
        // 11 - Clone Scavenger
        // 12 - Clone Organizer
        // 13- Clone Buy
        // 14- Clone Sell
        // 17- Clone Restock

        private int m_agentid = 0;

        internal int AgentID { get { return m_agentid; } set { SetAgent(value); } }

        internal void SetAgent(int agentid)
        {
            m_agentid = agentid;
            switch (agentid)
            {
                case 1:
                    Text = "Autoloot Add List";
                    break;
                case 2:
                    Text = "Scavenger Add List";
                    break;
                case 3:
                    Text = "Organizer Add List";
                    break;
                case 4:
                    Text = "Buy Add List";
                    break;
                case 5:
                    Text = "Sell Add List";
                    break;
                case 6:
                    Text = "Dress Add List";
                    break;
                case 7:
                    Text = "Friend Add List";
                    break;
                case 8:
                    Text = "Restock Add List";
                    break;
                case 9:
                    Text = "Launcher Add Shard";
                    break;
                case 10:
                    Text = "Autoloot Clone List";
                    break;
                case 11:
                    Text = "Scavenger Clone List";
                    break;
                case 12:
                    Text = "Organizer Clone List";
                    break;
                case 13:
                    Text = "Buy Clone List";
                    break;
                case 14:
                    Text = "Sell Clone List";
                    break;
                case 17:
                    Text = "Restock Clone List";
                    break;
            }
        }
        public EnhancedAgentAddList(int agentid)
        {
            InitializeComponent();
            MaximizeBox = false;
            SetAgent(agentid);
        }

        private void EnhancedAgentCloseItemList_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EnhancedAgentAddList_Click(object sender, EventArgs e)
        {
            bool fail = false;
            if (AgentListToAdd.Text == String.Empty)
                fail = true;

            if (!Regex.IsMatch(AgentListToAdd.Text, "^[ a-zA-Z0-9_]+$"))
                fail = true;

            string newList = AgentListToAdd.Text.ToLower();

            switch (m_agentid)
            {
                case 9:
                    if (RazorEnhanced.Shard.Exists(newList))
                        fail = true;
                    break;
            }


            if (fail)
            {
                MessageBox.Show("Invalid list name!",
                "Invalid list name!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }
            else
            {
                switch (m_agentid)
                {
                    case 9:
                        RazorEnhanced.Shard.Insert(newList, "Not set", "Not Set", "Optional", "0.0.0.0", 0, false, false, false);
                        break;
                }

                Close();
            }
        }
    }
}
