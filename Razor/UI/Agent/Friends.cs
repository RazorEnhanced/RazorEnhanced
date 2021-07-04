using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal ListBox FriendLogBox { get { return friendLogBox; } }
		internal ListView FriendListView { get { return friendlistView; } }
		internal ComboBox FriendListSelect { get { return friendListSelect; } }
		internal RazorCheckBox FriendPartyCheckBox { get { return friendPartyCheckBox; } }
		internal RazorCheckBox FriendAttackCheckBox { get { return friendAttackCheckBox; } }
		internal RazorCheckBox FriendIncludePartyCheckBox { get { return friendIncludePartyCheckBox; } }
		internal RazorCheckBox FriendSLCheckBox { get { return SLfriendCheckBox; } }
		internal RazorCheckBox FriendTBCheckBox { get { return TBfriendCheckBox; } }
		internal RazorCheckBox FriendCOMCheckBox { get { return COMfriendCheckBox; } }
		internal RazorCheckBox FriendMINCheckBox { get { return MINfriendCheckBox; } }
		internal ListView FriendGuildListView { get { return friendguildListView; } }

		private void friendButtonAddList_Click(object sender, EventArgs e)
		{
			foreach (Form f in Application.OpenForms)
			{
				if (f is EnhancedAgentAddList af)
				{
					af.AgentID = 7;
					af.Focus();
					return;
				}
			}
			new EnhancedAgentAddList(7).Show();
		}

		private void friendButtonRemoveList_Click(object sender, EventArgs e)
		{
			if (friendListSelect.Text != String.Empty)
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Friend list: " + friendListSelect.Text, "Delete Friend List?", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Friend.AddLog("Friends list " + friendListSelect.Text + " removed!");
					RazorEnhanced.Friend.AutoacceptParty = false;
					RazorEnhanced.Friend.IncludeParty = false;
					RazorEnhanced.Friend.PreventAttack = false;
					RazorEnhanced.Friend.RemoveList(friendListSelect.Text);
				}
			}
		}

		private void friendPartyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (friendPartyCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void friendAttackCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (friendAttackCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void friendIncludePartyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (friendIncludePartyCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void friendListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{

			RazorEnhanced.Settings.Friend.ListDetailsRead(friendListSelect.Text, out bool includeparty, out bool preventattack, out bool autoacceptparty, out bool slfriend, out bool tbfriend, out bool comfriend, out bool minfriend);
			RazorEnhanced.Friend.IncludeParty = includeparty;
			RazorEnhanced.Friend.PreventAttack = preventattack;
			RazorEnhanced.Friend.AutoacceptParty = autoacceptparty;
			RazorEnhanced.Friend.SLFriend = slfriend;
			RazorEnhanced.Friend.TBFriend = tbfriend;
			RazorEnhanced.Friend.COMFriend = comfriend;
			RazorEnhanced.Friend.MINFriend = minfriend;

			RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
			RazorEnhanced.Friend.RefreshPlayers();
			RazorEnhanced.Friend.RefreshGuilds();

			if (friendListSelect.Text != String.Empty)
				RazorEnhanced.Friend.AddLog("Friends list changed to: " + friendListSelect.Text);
		}

		private void friendGuildListView_Checked(object sender, ItemCheckedEventArgs e)
		{
			if (friendguildListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.Friend.UpdateSelectedGuild(item.Index);
			}
		}

		private void friendlistView_PlayerChecked(object sender, ItemCheckedEventArgs e)
		{
			if (friendlistView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.Friend.UpdateSelectedPlayer(item.Index);
			}
		}

		private void friendAddTargetButton_Click(object sender, EventArgs e)
		{
			Friend.AddFriendTarget();
		}

		private void friendRemoveButton_Click(object sender, EventArgs e)
		{
			if (friendListSelect.Text != String.Empty)
			{
				if (friendlistView.SelectedItems.Count == 1)
				{
					int index = friendlistView.SelectedItems[0].Index;
					string selection = friendListSelect.Text;

					if (RazorEnhanced.Settings.Friend.ListExists(selection))
					{
						RazorEnhanced.Settings.Friend.PlayersRead(selection, out List<Friend.FriendPlayer> players);
						if (index <= players.Count - 1)
						{
							RazorEnhanced.Settings.Friend.PlayerDelete(selection, players[index]);
							RazorEnhanced.Friend.RefreshPlayers();
						}
					}
				}
			}
			else
				RazorEnhanced.Friend.AddLog("Friends list not selected!");
		}

		private void friendAddButton_Click(object sender, EventArgs e)
		{
			if (friendListSelect.Text != String.Empty)
			{
				EnhancedFriendAddPlayerManual ManualAddPlayer = new EnhancedFriendAddPlayerManual
				{
					TopMost = true
				};
				ManualAddPlayer.Show();
			}
			else
				RazorEnhanced.Friend.AddLog("Friends list not selected!");
		}

		private void FriendGuildAddButton_Click(object sender, EventArgs e)
		{
			if (friendListSelect.Text != String.Empty)
			{
				EnhancedFriendAddGuildManual ManualAddGuild = new EnhancedFriendAddGuildManual
				{
					TopMost = true
				};
				ManualAddGuild.Show();
			}
			else
				RazorEnhanced.Friend.AddLog("Friends list not selected!");
		}

		private void SLfriendCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (SLfriendCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void COMfriendCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (COMfriendCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void TBfriendCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (TBfriendCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void MINfriendCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (MINfriendCheckBox.Focused)
				RazorEnhanced.Settings.Friend.ListUpdate(friendListSelect.Text, RazorEnhanced.Friend.IncludeParty, RazorEnhanced.Friend.PreventAttack, RazorEnhanced.Friend.AutoacceptParty, Friend.SLFriend, Friend.TBFriend, Friend.COMFriend, Friend.MINFriend, true);
		}

		private void FriendGuildRemoveButton_Click(object sender, EventArgs e)
		{
			if (friendListSelect.Text != String.Empty)
			{
				if (friendguildListView.SelectedItems.Count == 1)
				{
					int index = friendguildListView.SelectedItems[0].Index;
					string selection = friendListSelect.Text;

					if (RazorEnhanced.Settings.Friend.ListExists(selection))
					{
						RazorEnhanced.Settings.Friend.GuildRead(selection, out List<Friend.FriendGuild> guilds);
						if (index <= guilds.Count - 1)
						{
							RazorEnhanced.Settings.Friend.GuildDelete(selection, guilds[index]);
							RazorEnhanced.Friend.RefreshGuilds();
						}
					}
				}
			}
			else
				RazorEnhanced.Friend.AddLog("Friends list not selected!");
		}
	}
}
