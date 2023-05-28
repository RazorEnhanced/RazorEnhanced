using Assistant;
using Assistant.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RazorEnhanced
{
    internal class Profiles
    {
        private static readonly string m_Save = "RazorEnhanced.NewProfiles";
        private static readonly string m_OldSave = "RazorEnhanced.profiles";
        private static List<Profile> m_Dataset;

        internal class PlayerEntry
        {
            public PlayerEntry(string playerName)
            {
                PlayerName = playerName;
                PlayerSerial = 0;
            }

            [JsonProperty]
            internal string PlayerName { get; set; }

            [JsonProperty]
            internal int PlayerSerial { get; set; }

        }

        public class Profile
        {
            public Profile(string name)
            {
                Name = name;
                Last = false;
                Players = new List<PlayerEntry>();
            }
            public string Name { get; set; }
            public bool Last { get; set; }
            public List<PlayerEntry> Players  { get; set; }

            public void Add(string playername, int serial)
            {
                PlayerEntry playerEntry = new PlayerEntry(playername);
                playerEntry.PlayerSerial = serial;
                Players.Add(playerEntry);
            }

            public void Remove(string playerName)
            {
                Players.RemoveAll(profileEntry => profileEntry.PlayerName == playerName);
            }
            public bool Contains(string playerName)
            {
                foreach (var profileEntry in Players)
                {
                    if (profileEntry.PlayerName == playerName)
                        return true;
                }
                return false;
            }

            public void Remove(int playerSerial)
            {
                Players.RemoveAll(profileEntry => profileEntry.PlayerSerial == playerSerial);
            }

            public bool Contains(int playerSerial)
            {
                foreach (var profileEntry in Players)
                {
                    if (profileEntry.PlayerSerial == playerSerial)
                        return true;
                }
                return false;
            }

        }

        internal static void Load(bool try_backup = true)
        {
            if (m_Dataset != null)
                return;

            string profileDir = Path.Combine(Assistant.Engine.RootPath, "Profiles");
            string filename = Path.Combine(profileDir, m_Save);
            string oldFilename = Path.Combine(profileDir, m_OldSave);
            string backup = Path.Combine(Assistant.Engine.RootPath, "Backup", m_Save);

            if (File.Exists(filename))
            {
                try
                {
                    m_Dataset = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Profile>>(File.ReadAllText(filename));
                    File.Copy(filename, backup, true);
                    return;
                }
                catch
                {
                    if (try_backup)
                    {
                        MessageBox.Show("Error loading " + m_Save + ", Try to restore from backup!");
                        File.Copy(backup, filename, true);
                        Load(false);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("All failed!, recovering from Profile names, linked characters will be lost");
                    }
                }
            }


            int countFoundProfiles = 0;
            string[] directorNames = Directory.GetDirectories(profileDir);
            m_Dataset = new List<Profile>();

            if (directorNames.Length == 0)
            {
                Profile defaultProfile = new Profile("default");
                defaultProfile.Last = true;
                m_Dataset.Add(defaultProfile);
            }
            else
            {
                foreach (string directorName in directorNames)
                {
                    Profile profile = new Profile(Path.GetFileName(directorName));
                    m_Dataset.Add(profile);
                }
                SetLast(directorNames[0]); // default first one as last
                if (File.Exists(oldFilename))
                {
                    var old_Dataset = new DataSet();
                    try
                    {
                        old_Dataset = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(File.ReadAllText(oldFilename));
                        foreach (DataRow row in old_Dataset.Tables["PROFILES"].Rows)
                        {
                            if ((bool)row["Last"])
                                SetLast((string)row["Name"]);
                        }
                        foreach (DataRow row in old_Dataset.Tables["PROFILES"].Rows)
                        {
                            try
                            {
                                string profileName = (string)row["Name"];
                                string playerName = (string)row["PlayerName"];
                                int serial = (int)(Int64)row["PlayerSerial"];
                                Link(serial, profileName, playerName);
                            }
                            catch
                            { }
                        }
                    }
                    catch
                    { }
                }
            }

        }

        // Funzioni di accesso al salvataggio
        internal static List<string> ReadAll()
        {
            List<string> profilelist = new List<string>();

            foreach (var profile in m_Dataset)
            {
                profilelist.Add(profile.Name);
            }

            return profilelist;
        }

        internal static string LastUsed()
        {
            foreach (var profile in m_Dataset)
            {
                if (profile.Last)
                {
                    return profile.Name;
                }
            }

            return "default";
        }

        internal static void SetLast(string name)
        {
            for (int i=0; i < m_Dataset.Count; i++)
            {
                if (m_Dataset[i].Name == name)
                {
                    m_Dataset[i].Last = true;
                } else
                {
                    m_Dataset[i].Last = false;
                }
            }

            Save();
        }

        internal static void Add(string name)
        {
            Profile newProfile = new Profile(name);
            newProfile.Last = true;
            m_Dataset.Add(newProfile);

            Save();
        }

        internal static void Delete(string name)
        {
            m_Dataset.RemoveAll(profile => profile.Name == name);
            Save();
        }

        internal static bool Exist(string name)
        {
            foreach (var profile in m_Dataset)
            {
                if (profile.Name == name)
                    return true;
            }
            return false;
        }

        internal static string IsLinked(int serial)
        {
            foreach (var profile in m_Dataset)
            {
                if (profile.Contains(serial))
                    return profile.Name;
            }
            return null;           
        }

        internal static string GetLinkName(string profilename)
        {
            string addComma = "";
            string allLinkNames = "";
            foreach (var profile in m_Dataset)
            {
                if (profile.Name == profilename)
                {
                    foreach (var profileEntry in profile.Players)
                    {
                        allLinkNames += (addComma + profileEntry.PlayerName);
                        addComma = ",";
                    }
                    return allLinkNames;
                }
            }
            return "None";
        }

        internal static void Link(int serial, string profileName, string playername)
        {
            foreach (var profile in m_Dataset)
            {
                if (profile.Contains(serial))
                {
                    profile.Remove(serial);
                    break;
                }
            }
            foreach (var profile in m_Dataset)  // Linko nuovo profilo
            {
                if (profile.Name == profileName)
                {
                    profile.Add(playername, serial);
                    break;
                }
            }
            Save();
        }

        internal static void UnLink(string profileName, int serial)
        {
            foreach (var profile in m_Dataset)
            {
                
                if (profile.Name == profileName)
                {
                    profile.Remove(serial);
                    break;
                }
            }
            Save();
        }

        internal static void Rename(string oldname, string newname)
        {
            for (int i = 0; i < m_Dataset.Count; i++)
            {
                if (m_Dataset[i].Name == oldname)
                {
                    m_Dataset[i].Name = newname;
                    break;
                }
            }
            Save();
            try
            {
                string oldDirectory = Path.Combine(Assistant.Engine.RootPath, "Profiles", oldname);
                string newDirectory = Path.Combine(Assistant.Engine.RootPath, "Profiles", newname);
                System.IO.Directory.Move(oldDirectory, newDirectory);
            }
            catch
            { }
        }

        // Funzioni richiamate dalla gui

        internal static void Refresh()
        {
            Assistant.Engine.MainWindow.ProfilesComboBox.Items.Clear();
            List<string> profilelist = ReadAll();
            foreach (string profilename in profilelist)
            {
                Assistant.Engine.MainWindow.ProfilesComboBox.Items.Add(profilename);
            }

            Assistant.Engine.MainWindow.ProfilesComboBox.SelectedIndex = Assistant.Engine.MainWindow.ProfilesComboBox.Items.IndexOf(LastUsed());
        }

        internal static void ProfileChange(string name)
        {
            // Salvo parametri di uscita
            RazorEnhanced.Settings.General.SaveExitData();
            PasswordMemory.ProfileChangeInit();
            Assistant.Engine.MainWindow.Initializing = true;

            // Stop timer script
            if (RazorEnhanced.Scripts.Timer != null)
                RazorEnhanced.Scripts.Timer.Close();

            // Stop forzato di tutti i thread agent
            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
                Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
                Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true)
                Assistant.Engine.MainWindow.OrganizerStop.PerformClick();

            if (Assistant.Engine.MainWindow.BuyCheckBox.Checked == true)
                Assistant.Engine.MainWindow.BuyCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.BuyCompleteCheckBox.Checked == true)
                Assistant.Engine.MainWindow.BuyCompleteCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.SellCheckBox.Checked == true)
                Assistant.Engine.MainWindow.SellCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
                Assistant.Engine.MainWindow.DressStopButton.PerformClick();

            if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
                Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
                Assistant.Engine.MainWindow.DressStopButton.PerformClick();

            // Stop filtri
            if (Assistant.Engine.MainWindow.AutoCarverCheckBox.Enabled == true)
                Assistant.Engine.MainWindow.AutoCarverCheckBox.Checked = false;

            if (Assistant.Engine.MainWindow.MobFilterCheckBox.Enabled == true)
                Assistant.Engine.MainWindow.MobFilterCheckBox.Checked = false;

            // Stop video recorder
            Assistant.MainForm.StopVideoRecorder();

            // Svuoto logbox e reset select index
        //  Assistant.Engine.MainWindow.AutoLootLogBox.Items.Clear();
            AutoLoot.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.ScavengerLogBox.Items.Clear();
            Scavenger.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.OrganizerLogBox.Items.Clear();
            Organizer.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.SellLogBox.Items.Clear();
            SellAgent.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.BuyLogBox.Items.Clear();
            BuyAgent.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.DressLogBox.Items.Clear();
            Dress.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.FriendLogBox.Items.Clear();
            Friend.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.RestockLogBox.Items.Clear();
            Restock.AddLog("Profile Changed!");

            Assistant.Engine.MainWindow.BandageHealLogBox.Items.Clear();
            BandageHeal.AddLog("Profile Changed!");

            // Cambio file
            //if (name == "default")
            //  RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced.settings";
            //else
            //  RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced." + name + ".settings";

            // Rimuovo cache password e disabilito vecchi filtri
            Assistant.Filters.Filter.DisableAll();

            // Chiuto toolbar
            if (RazorEnhanced.ToolBar.ToolBarForm != null)
                RazorEnhanced.ToolBar.ToolBarForm.Close();

            // Chiuto toolbar
            if (RazorEnhanced.SpellGrid.SpellGridForm != null)
                RazorEnhanced.SpellGrid.SpellGridForm.Close();

            // Carico save profilo
            RazorEnhanced.Settings.Load(name);

            // Abilito patch UOMod
            UoMod.ProfileChange();

            // Refresh list
            Assistant.Engine.MainWindow.AutoLootListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.SellListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.BuyListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.DressListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.FriendListSelect.SelectedIndex = -1;
            Assistant.Engine.MainWindow.RestockListSelect.SelectedIndex = -1;

            // Reinizzializzo razor
            Engine.MainWindow.SafeAction(s => s.LoadSettings());
            //Assistant.Engine.MainWindow.LoadSettings();

            // Riapro toollbar se le condizioni lo permettono
            if (RazorEnhanced.Settings.General.ReadBool("AutoopenToolBarCheckBox"))
                RazorEnhanced.ToolBar.Open();

            // Riapro la spellgrid se le condizioni lo permettono
            if (RazorEnhanced.Settings.General.ReadBool("GridOpenLoginCheckBox"))
                RazorEnhanced.SpellGrid.Open();

            Assistant.Engine.MainWindow.Initializing = false;
            SetLast(name);

            PasswordMemory.ProfileChangeEnd();

            if (World.Player != null) // Reinit script timer se cambio profilo avvene da loggati
                RazorEnhanced.Scripts.Init();
        }

        internal static void Save()
        {
            try
            {
                string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", m_Save);
                File.WriteAllText(filename, Newtonsoft.Json.JsonConvert.SerializeObject(m_Dataset, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing " + m_Save + ": " + ex);
            }
        }
    }
}
