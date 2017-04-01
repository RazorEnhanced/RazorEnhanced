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
		// 8- Friend
		// 9- Shard

		int m_agentid = 0;
		public EnhancedAgentAddList(int agentid)
		{
			InitializeComponent();
			MaximizeBox = false;
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
			}
		}

		private void EnhancedAgentCloseItemList_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void EnhancedAgentAddList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (AgentListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(AgentListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = AgentListToAdd.Text.ToLower();

			switch (m_agentid)
			{
				case 1:
					if (Settings.AutoLoot.ListExists(newList))
						fail = true;
					break;
				case 2:
					if (Settings.Scavenger.ListExists(newList))
						fail = true;
					break;
				case 3:
					if (Settings.Organizer.ListExists(newList))
						fail = true;
					break;
				case 4:
					if (Settings.BuyAgent.ListExists(newList))
						fail = true;
					break;
				case 5:
					if (Settings.SellAgent.ListExists(newList))
						fail = true;
					break;
				case 6:
					if (Settings.Dress.ListExists(newList))
						fail = true;
					break;
				case 7:
					if (Settings.Friend.ListExists(newList))
						fail = true;
					break;
				case 8:
					if (Settings.Restock.ListExists(newList))
						fail = true;
					break;
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
				fail = true;
			}
			else
			{
				switch (m_agentid)
				{
					case 1:
						AutoLoot.AddList(newList);
						break;
					case 2:
						Scavenger.AddList(newList);
						break;
					case 3:
						Organizer.AddList(newList);
						break;
					case 4:
						BuyAgent.AddList(newList);
						break;
					case 5:
						SellAgent.AddList(newList);
						break;
					case 6:
						Dress.AddList(newList);
						break;
					case 7:
						Friend.AddList(newList);
						break;
					case 8:
						Restock.AddList(newList);
						break;
					case 9:
						RazorEnhanced.Shard.Insert(newList, "Not set", "Not Set", "0.0.0.0", "0", false, false);
						break;
				}

				Close();
			}
		}
	}
}