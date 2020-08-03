using Assistant;
using Assistant.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace RazorEnhanced
{
    public class Friend
    {
        public class FriendPlayer : ListAbleItem
        {
            private string m_Name;
            public string Name { get { return m_Name; } }

            private int m_Serial;
            public int Serial { get { return m_Serial; } }

            private bool m_Selected;
            [JsonProperty("Selected")]
            internal bool Selected { get { return m_Selected; } }

            public FriendPlayer(string name, int serial, bool selected)
            {
                m_Name = name;
                m_Serial = serial;
                m_Selected = selected;
            }
        }

        [Serializable]
        public class FriendGuild : ListAbleItem
        {
            private string m_Name;
            public string Name { get { return m_Name; } }

            private bool m_Selected;
            [JsonProperty("Selected")]
            internal bool Selected { get { return m_Selected; } }

            public FriendGuild(string name, bool selected)
            {
                m_Name = name;
                m_Selected = selected;
            }
        }

        internal class FriendList
        {
            private string m_Description;
            internal string Description { get { return m_Description; } }

            private bool m_AutoacceptParty;
            internal bool AutoacceptParty { get { return m_AutoacceptParty; } }

            private bool m_PreventAttack;
            internal bool PreventAttack { get { return m_PreventAttack; } }

            private bool m_IncludeParty;
            internal bool IncludeParty { get { return m_IncludeParty; } }

            private bool m_SLFriend;
            internal bool SLFriend { get { return m_SLFriend; } }

            private bool m_TBFriend;
            internal bool TBFriend { get { return m_TBFriend; } }

            private bool m_COMFriend;
            internal bool COMFriend { get { return m_COMFriend; } }

            private bool m_MINFriend;
            internal bool MINFRiend { get { return m_MINFriend; } }

            private bool m_Selected;
            [JsonProperty("Selected")]
            internal bool Selected { get { return m_Selected; } }

            public FriendList(string description, bool autoacceptparty, bool preventattack, bool includeparty, bool slfriend, bool tbfriend, bool comfriend, bool minfriend, bool selected)
            {
                m_Description = description;
                m_AutoacceptParty = autoacceptparty;
                m_PreventAttack = preventattack;
                m_IncludeParty = includeparty;
                m_SLFriend = slfriend;
                m_TBFriend = tbfriend;
                m_COMFriend = comfriend;
                m_MINFriend = minfriend;
                m_Selected = selected;
            }
        }

        internal static void AddLog(string addlog)
        {
            if (!Client.Running)
                return;

            Engine.MainWindow.SafeAction(s => s.FriendLogBox.Items.Add(addlog));
            Engine.MainWindow.SafeAction(s => s.FriendLogBox.SelectedIndex = s.FriendLogBox.Items.Count - 1);
            if (Engine.MainWindow.FriendLogBox.Items.Count > 300)
                Engine.MainWindow.SafeAction(s => s.FriendLogBox.Items.Clear());
        }

        internal static bool IncludeParty
        {
            get
            {
                return Engine.MainWindow.FriendIncludePartyCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendIncludePartyCheckBox.Checked = value;
            }
        }

        internal static bool PreventAttack
        {
            get
            {
                return Engine.MainWindow.FriendAttackCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendAttackCheckBox.Checked = value;
            }
        }

        internal static bool AutoacceptParty
        {
            get
            {
                return Engine.MainWindow.FriendPartyCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendPartyCheckBox.Checked = value;
            }
        }

        internal static bool SLFriend
        {
            get
            {
                return Engine.MainWindow.FriendSLCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendSLCheckBox.Checked = value;
            }
        }

        internal static bool TBFriend
        {
            get
            {
                return Engine.MainWindow.FriendTBCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendTBCheckBox.Checked = value;
            }
        }

        internal static bool COMFriend
        {
            get
            {
                return Engine.MainWindow.FriendCOMCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendCOMCheckBox.Checked = value;
            }
        }

        internal static bool MINFriend
        {
            get
            {
                return Engine.MainWindow.FriendMINCheckBox.Checked;
            }
            set
            {
                Engine.MainWindow.FriendMINCheckBox.Checked = value;
            }
        }

        internal static string FriendListName
        {
            get
            {
                if (Engine.MainWindow.FriendListSelect.InvokeRequired)
                    return (string)Engine.MainWindow.FriendListSelect.Invoke(new Func<string>(() => Engine.MainWindow.FriendListSelect.Text));
                else
                    return Engine.MainWindow.FriendListSelect.Text;
            }

            set
            {
                Engine.MainWindow.FriendListSelect.Text = value;
            }
        }


        internal static void RefreshLists()
        {
            List<FriendList> lists = Settings.Friend.ListsRead();

            if (lists.Count == 0)
            {
                Engine.MainWindow.FriendListView.Items.Clear();
                Engine.MainWindow.FriendAttackCheckBox.Checked = false;
                Engine.MainWindow.FriendIncludePartyCheckBox.Checked = false;
                Engine.MainWindow.FriendPartyCheckBox.Checked = false;
                Engine.MainWindow.FriendSLCheckBox.Checked = false;
                Engine.MainWindow.FriendTBCheckBox.Checked = false;
                Engine.MainWindow.FriendCOMCheckBox.Checked = false;
                Engine.MainWindow.FriendMINCheckBox.Checked = false;

            }

            FriendList selectedList = lists.FirstOrDefault(l => l.Selected);
            if (selectedList != null && selectedList.Description == Engine.MainWindow.FriendListSelect.Text)
                return;

            Engine.MainWindow.FriendListSelect.Items.Clear();
            foreach (FriendList l in lists)
            {
                Engine.MainWindow.FriendListSelect.Items.Add(l.Description);

                if (!l.Selected)
                    continue;

                Engine.MainWindow.FriendListSelect.SelectedIndex = Engine.MainWindow.FriendListSelect.Items.IndexOf(l.Description);
                IncludeParty = l.IncludeParty;
                PreventAttack = l.PreventAttack;
                AutoacceptParty = l.AutoacceptParty;
                SLFriend = l.SLFriend;
                TBFriend = l.TBFriend;
                COMFriend = l.COMFriend;
                MINFriend = l.MINFRiend;
            }
        }

        internal static void RefreshPlayers()
        {
            List<FriendList> lists = Settings.Friend.ListsRead();

            Engine.MainWindow.SafeAction(s => s.FriendListView.Items.Clear());

            foreach (FriendList l in lists)
            {
                if (!l.Selected)
                    continue;

                RazorEnhanced.Settings.Friend.PlayersRead(l.Description, out List<FriendPlayer> players);

                foreach (FriendPlayer player in players)
                {
                    ListViewItem listitem = new ListViewItem
                    {
                        Checked = player.Selected
                    };

                    listitem.SubItems.Add(player.Name);
                    listitem.SubItems.Add("0x" + player.Serial.ToString("X8"));

                    Engine.MainWindow.SafeAction(s => s.FriendListView.Items.Add(listitem));
                }
            }
        }

        internal static void RefreshGuilds()
        {
            List<FriendList> lists = Settings.Friend.ListsRead();

            Engine.MainWindow.FriendGuildListView.Items.Clear();
            foreach (FriendList l in lists)
            {
                if (!l.Selected)
                    continue;

                RazorEnhanced.Settings.Friend.GuildRead(l.Description, out List<FriendGuild> guilds);

                foreach (FriendGuild guild in guilds)
                {
                    ListViewItem listitem = new ListViewItem
                    {
                        Checked = guild.Selected
                    };

                    listitem.SubItems.Add(guild.Name);

                    Engine.MainWindow.FriendGuildListView.Items.Add(listitem);
                }
            }
        }

        internal static void AddList(string newList)
        {
            RazorEnhanced.Settings.Friend.ListInsert(newList, false, false, false, false, false, false,false);

            RazorEnhanced.Friend.RefreshLists();
            RazorEnhanced.Friend.RefreshPlayers();
        }

        internal static void RemoveList(string list)
        {
            if (RazorEnhanced.Settings.Friend.ListExists(list))
            {
                RazorEnhanced.Settings.Friend.ListDelete(list);
            }

            RazorEnhanced.Friend.RefreshLists();
            RazorEnhanced.Friend.RefreshPlayers();
        }

        internal static void AddPlayerToList(string name, int serial)
        {
            string selection = Engine.MainWindow.FriendListSelect.Text;
            FriendPlayer player = new FriendPlayer(name, serial, true);

            if (RazorEnhanced.Settings.Friend.ListExists(selection))
            {
                if (!RazorEnhanced.Settings.Friend.PlayerExists(selection, player))
                {
                    if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
                        RazorEnhanced.Misc.SendMessage("Friend added: " + name, false);
                    RazorEnhanced.Friend.AddLog("Friend added: " + name);
                    RazorEnhanced.Settings.Friend.PlayerInsert(selection, player);
                    RazorEnhanced.Friend.RefreshPlayers();
                }
                else
                {
                    if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
                        RazorEnhanced.Misc.SendMessage(name + " is already in friend list", false);
                    RazorEnhanced.Friend.AddLog(name + " is already in friend list");
                }
            }
        }

        internal static void AddGuildToList(string name)
        {
            string selection = Engine.MainWindow.FriendListSelect.Text;
            FriendGuild guild = new FriendGuild(name, true);

            if (RazorEnhanced.Settings.Friend.ListExists(selection))
            {
                if (!RazorEnhanced.Settings.Friend.GuildExists(selection, name))
                    RazorEnhanced.Settings.Friend.GuildInsert(selection, guild);
            }
            RazorEnhanced.Friend.RefreshGuilds();
        }

        internal static void UpdateSelectedGuild(int i)
        {
            RazorEnhanced.Settings.Friend.GuildRead(FriendListName, out List<RazorEnhanced.Friend.FriendGuild> guilds);
            if (guilds.Count != Engine.MainWindow.FriendGuildListView.Items.Count)
            {
                return;
            }
            ListViewItem lvi = Engine.MainWindow.FriendGuildListView.Items[i];
            FriendGuild old = guilds[i];
            if (lvi != null && old != null)
            {
                FriendGuild guild = new Friend.FriendGuild(old.Name, lvi.Checked);
                RazorEnhanced.Settings.Friend.GuildReplace(RazorEnhanced.Friend.FriendListName, i, guild);
            }

        }

            internal static void UpdateSelectedPlayer(int i)
        {
            RazorEnhanced.Settings.Friend.PlayersRead(FriendListName, out List<FriendPlayer> players);

            if (players.Count != Engine.MainWindow.FriendListView.Items.Count)
            {
                return;
            }

            ListViewItem lvi = Engine.MainWindow.FriendListView.Items[i];
            FriendPlayer old = players[i];

            if (lvi != null && old != null)
            {
                FriendPlayer player = new Friend.FriendPlayer(old.Name, old.Serial, lvi.Checked);
                RazorEnhanced.Settings.Friend.PlayerReplace(RazorEnhanced.Friend.FriendListName, i, player);
            }
        }

        public static bool IsFriend(int serial)
        {
            RazorEnhanced.Settings.Friend.PlayersRead(Friend.FriendListName, out List<FriendPlayer> players);
            foreach (FriendPlayer player in players)        // Ricerca nella friend list normale
            {
                if (!player.Selected)
                    continue;

                if (player.Serial == serial)
                    return true;
            }

            if (Friend.IncludeParty && PacketHandlers.Party.Contains(serial))            // Ricerco nel party se attiva l'opzione
                return true;


            if (Engine.MainWindow.FriendSLCheckBox.Checked)
            {
                if (GetFaction("SL", serial))
                    return true;
            }

            if (Engine.MainWindow.FriendTBCheckBox.Checked)
            {
                if (GetFaction("TB", serial))
                    return true;
            }

            if (Engine.MainWindow.FriendCOMCheckBox.Checked)
            {
                if (GetFaction("CoM", serial))
                    return true;
            }

            if (Engine.MainWindow.FriendMINCheckBox.Checked)
            {
                if (GetFaction("MiN", serial))
                    return true;
            }

            List<Friend.FriendGuild> guilds = new List<FriendGuild>();
            RazorEnhanced.Settings.Friend.GuildRead(Friend.FriendListName, out guilds);
            foreach (FriendGuild guild in guilds)
            {
                if (!guild.Selected)
                    continue;

                if (GetGuild(guild.Name, serial))
                    return true;
            }
            return false;
        }
        public static void AddPlayer(string selection, string name, int serial)
        {
            FriendPlayer player = new FriendPlayer(name, serial, true);

            if (RazorEnhanced.Settings.Friend.ListExists(selection))
            {
                if (!RazorEnhanced.Settings.Friend.PlayerExists(selection, player))
                {
                    if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
                        RazorEnhanced.Misc.SendMessage("Friend added: " + name, false);
                    RazorEnhanced.Friend.AddLog("Friend added: " + name);
                    RazorEnhanced.Settings.Friend.PlayerInsert(selection, player);
                    RazorEnhanced.Friend.RefreshPlayers();
                }
                else
                {
                    if (Settings.General.ReadBool("ShowAgentMessageCheckBox"))
                        RazorEnhanced.Misc.SendMessage(name + " is already in friend list", false);
                    RazorEnhanced.Friend.AddLog(name + " is already in friend list");
                }
            }
            return;
        }


            private static bool GetFaction(string name, int serial)
        {
            Assistant.Mobile target = Assistant.World.FindMobile(serial);

            if (target == null)
                return false;

            if (target.ObjPropList.Content.Count <= 0)
                return false;

            string firstProp = target.ObjPropList.Content[0].ToString();
            if (firstProp.Contains(string.Format("[{0}]", name)))
                return true;
            return false;
        }


        private static bool GetGuild(string name, int serial)
        {
            Assistant.Mobile target = Assistant.World.FindMobile(serial);

            if (target == null)
                return false;

            if (target.ObjPropList.Content.Count > 0)
            {
                string firstProp = target.ObjPropList.Content[0].ToString();
                if (firstProp.Contains(string.Format("[{0}]", name)))
                    return true;
            }
            return false;
        }


        public static void ChangeList(string nameList)
        {
            if (!Engine.MainWindow.FriendListSelect.Items.Contains(nameList))
            {
                Scripts.SendMessageScriptError("Script Error: Friend.ChangeList: Friend List: " + nameList + " not exist");
            }
            else
            {
                Engine.MainWindow.SafeAction(s => s.FriendListSelect.SelectedIndex = s.FriendListSelect.Items.IndexOf(nameList));

            }
        }

        public static List<int> GetList(string nameList)
        {
            List<int> friendserials = new List<int>();
            if (!Engine.MainWindow.FriendListSelect.Items.Contains(nameList))
            {
                Scripts.SendMessageScriptError("Script Error: Friend.GetList: Friend List: " + nameList + " not exist");
            }
            else
            {
                RazorEnhanced.Settings.Friend.PlayersRead(nameList, out List<FriendPlayer> players);
                foreach (FriendPlayer player in players)
                    friendserials.Add(player.Serial);
            }
            return friendserials;
        }


        // Fiend target callback
        public static void AddFriendTarget()
        {
            if (Engine.MainWindow.FriendListSelect.Text == string.Empty)
            {
                Friend.AddLog("Friends list not selected!");
            }
            else
            {
                Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(FriendPlayerTarget_Callback));
            }
        }

        private static void FriendPlayerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
        {
            Assistant.Mobile friendplayer = World.FindMobile(serial);
            if (friendplayer != null && friendplayer.Serial.IsMobile && friendplayer.Serial != World.Player.Serial)
            {
                Engine.MainWindow.SafeAction(s => { Friend.AddPlayerToList(friendplayer.Name, friendplayer.Serial); });
            }
            else
            {
                if (Engine.MainWindow.ShowAgentMessageCheckBox.Checked)
                    Misc.SendMessage("Invalid target", false);
                Friend.AddLog("Invalid target");
            }
        }
    }
}
