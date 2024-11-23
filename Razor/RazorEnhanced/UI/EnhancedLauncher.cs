using AutoUpdaterDotNET;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
    internal partial class EnhancedLauncher : Form
    {
        private const string m_Title = "Welcome to Razor Enhanced";

        public EnhancedLauncher()
        {
            InitializeComponent();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                this.AutoScaleMode = AutoScaleMode.Font;
            MaximizeBox = false;
            this.Text = m_Title;
        }

        private void RefreshGUI()
        {
            var shards = RazorEnhanced.Shard.Read();
            clientFolderLabel.Text = clientPathLabel.Text = String.Empty;
            m_Tip.SetToolTip(clientFolderLabel, clientFolderLabel.Text);

            foreach (Shard shard in shards)
            {
                if (shard.Selected)
                {
                    shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shard.Description);
                    clientPathLabel.Text = shard.ClientPath;
                    m_Tip.SetToolTip(clientPathLabel, clientPathLabel.Text);
                    clientFolderLabel.Text = shard.ClientFolder;
                    m_Tip.SetToolTip(clientFolderLabel, clientFolderLabel.Text);
                    cuoClientLabel.Text = shard.CUOClient;
                    m_Tip.SetToolTip(cuoClientLabel, cuoClientLabel.Text);
                    hostLabel.Text = shard.Host;
                    portLabel.Text = shard.Port.ToString();
                    patchEnc.Checked = shard.PatchEnc;
                    osiEnc.Checked = shard.OSIEnc;
                }
            }

            if (shardlistCombobox.SelectedIndex == -1)
                groupBox2.Enabled = false;

            if (Directory.Exists(clientFolderLabel.Text))
            {
                if (File.Exists(clientPathLabel.Text))
                {
                    launch.Enabled = true;
                }
                if (File.Exists(cuoClientLabel.Text))
                {
                    launchCUO.Enabled = true;
                }
            }
            else
            {
                launch.Enabled = false;
                launchCUO.Enabled = false;
            }

            var ip = Assistant.Client.Resolve(hostLabel.Text);
            if (ip == null || ip == System.Net.IPAddress.None || Convert.ToInt32(portLabel.Text) == 0)
            {
                Shards.ShowLauncher = true;
                launchCUO.Enabled = false;
                launch.Enabled = false;
            }
        }

        internal void UpdateGUI()
        {
            portLabel.Text = portLabel.Text.Replace(" ", "");
            uint port;
            UInt32.TryParse(portLabel.Text, out port);

            hostLabel.Text = hostLabel.Text.Replace(" ", "");

            Shard.Update(shardlistCombobox.Text, clientPathLabel.Text, clientFolderLabel.Text, cuoClientLabel.Text, hostLabel.Text, port, patchEnc.Checked, osiEnc.Checked, true);

            var shards = Shard.Read();

            shardlistCombobox.Items.Clear();
            foreach (Shard shard in shards)
            {
                shardlistCombobox.Items.Add(shard.Description);
            }

            RefreshGUI();
        }

        private void EnhancedLauncher_Load(object sender, EventArgs e)
        {
            DateTime reminderDate = DateTime.MinValue;
            string reminderPath = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced.reminder.json");
            if (File.Exists(reminderPath))
            {
                reminderDate = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(File.ReadAllText(reminderPath));
            }
            else
            {
                reminderDate = DateTime.MinValue;
            }
            if (DateTime.Now > reminderDate)
            {
                // run update check .. removed AutoUpdater
                AutoUpdater.Start("https://raw.githubusercontent.com/jsebold666/razorenhanced.github.io/main/RazorEnhancedAutoUpdater.xml");
            }
            UpdateGUI();
        }

        private void RazorButton1_Click(object sender, EventArgs e)
        {
            EnhancedAgentAddList addshard = new(9);
            addshard.FormClosed += new FormClosedEventHandler(Addshard_refresh);
            addshard.TopMost = true;
            addshard.Show();
        }

        private void Addshard_refresh(object sender, EventArgs e)
        {
            //int port;
            //Int32.TryParse(portLabel.Text, out port);

            var shards = RazorEnhanced.Shard.Read();

            shardlistCombobox.Items.Clear();
            foreach (Shard shard in shards)
            {
                shardlistCombobox.Items.Add(shard.Description);
            }

            RefreshGUI();
            groupBox2.Enabled = true;
        }

        private void ShardlistCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RazorEnhanced.Shard.UpdateLast(shardlistCombobox.Text);
            var shards = RazorEnhanced.Shard.Read();

            foreach (Shard shard in shards)
            {
                if (shard.Selected)
                {
                    shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shard.Description);
                    clientPathLabel.Text = shard.ClientPath;
                    m_Tip.SetToolTip(clientPathLabel, clientPathLabel.Text);
                    clientFolderLabel.Text = shard.ClientFolder;
                    m_Tip.SetToolTip(clientFolderLabel, clientFolderLabel.Text);
                    cuoClientLabel.Text = shard.CUOClient;
                    m_Tip.SetToolTip(cuoClientLabel, cuoClientLabel.Text);
                    hostLabel.Text = shard.Host;
                    portLabel.Text = shard.Port.ToString();
                    patchEnc.Checked = shard.PatchEnc;
                    osiEnc.Checked = shard.OSIEnc;
                }
            }

            if (File.Exists(cuoClientLabel.Text))
            {
                launchCUO.Enabled = true;
            }
            else
            {
                launchCUO.Enabled = false;
            }

            if (shardlistCombobox.SelectedIndex == -1)
            {
                groupBox2.Enabled = false;
            }
            else
            {
                groupBox2.Enabled = true;
            }

            if (Directory.Exists(clientFolderLabel.Text) && File.Exists(clientPathLabel.Text))
                launch.Enabled = true;
            else
                launch.Enabled = false;

            if (File.Exists(cuoClientLabel.Text))
            {
                launchCUO.Enabled = true;
            }
            else
            {
                launchCUO.Enabled = false;
            }

        }

        private void PatchEncy_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void OsiEnc_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void ServeraddressT_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void ServerportT_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new()
            {
                Description = "Select a UO Data Directory...",
                ShowNewFolderButton = false
            };
            if (folder.ShowDialog() == DialogResult.OK)
            {
                clientFolderLabel.Text = folder.SelectedPath;
                UpdateGUI();
            }
        }

        private void BNameCopy_Click(object sender, EventArgs e)
        {
            this.openclientlocation.FileName = "client.exe";
            this.openclientlocation.Filter = "Executable Files|Client.exe|Executable Files|*.exe";
            openclientlocation.RestoreDirectory = true;
            if (openclientlocation.ShowDialog(this) == DialogResult.OK)
            {
                clientPathLabel.Text = openclientlocation.FileName;
                m_Tip.SetToolTip(clientPathLabel, clientPathLabel.Text);
                clientFolderLabel.Text = Path.GetDirectoryName(openclientlocation.FileName);
                UpdateGUI();
            }
        }

        private void Okay_Click(object sender, EventArgs e)
        {

            UpdateGUI();
            var shards = RazorEnhanced.Shard.Read();
            Shard selected = shards.FirstOrDefault(s => s.Selected);
            if (selected != null)
            {
                selected.StartTypeSelected = Shard.StartType.OSI;
                Shard.Update(selected.Description, selected.ClientPath,
                             selected.ClientFolder, selected.CUOClient,
                             selected.Host, selected.Port, selected.PatchEnc,
                             selected.OSIEnc, selected.Selected, selected.StartTypeSelected);
            }

            // Genero Login.cfg
            StreamWriter login;
            string LoginString = "LoginServer=" + hostLabel.Text + "," + portLabel.Text;
            string logincfgpath = clientPathLabel.Text.Substring(0, clientPathLabel.Text.LastIndexOf("\\") + 1);

            try
            {
                if (!File.Exists(logincfgpath + "login.cfg"))
                {
                    login = new StreamWriter(logincfgpath + "login.cfg");
                }
                else
                {
                    File.Delete(logincfgpath + "login.cfg");
                    login = new StreamWriter(logincfgpath + "login.cfg");
                }
                login.WriteLine(LoginString);
                login.Close();
            }
            catch (Exception ex)
            {
                var dialogResult = RazorEnhanced.UI.RE_MessageBox.Show("Login Config Failure",
                        $"Unable to read login file: {logincfgpath + "login.cfg"}\r\nError:\r\n{ex}",
                        ok: "Ok", no: null, cancel: null, backColor: null);

                this.Close();
            }
        }


        private void LaunchCUO_Click(object sender, EventArgs e)
        {
            UpdateGUI();
            var shards = RazorEnhanced.Shard.Read();
            Shard selected = shards.FirstOrDefault(s => s.Selected);
            if (selected != null)
            {
                selected.StartTypeSelected = Shard.StartType.CUO;
                Shard.Update(selected.Description, selected.ClientPath,
                             selected.ClientFolder, selected.CUOClient,
                             selected.Host, selected.Port, selected.PatchEnc,
                             selected.OSIEnc, selected.Selected, selected.StartTypeSelected);
            }

            // Need to fix up settuings.json
            //StreamWriter login;
            //string LoginString = "LoginServer=" + hostLabel.Text + "," + portLabel.Text;
            //string logincfgpath = cuoClientLabel.Text.Substring(0, cuoClientLabel.Text.LastIndexOf("\\") + 1);
        }


        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RazorButton2_Click(object sender, EventArgs e)
        {
            if (shardlistCombobox.Text != String.Empty)
            {
                RazorEnhanced.Shard.Delete(shardlistCombobox.Text);
            }
            UpdateGUI();
        }

        private void Checkupdatebutton_Click(object sender, EventArgs e)
        {
            // AutoUpdater
            AutoUpdater.Start("https://raw.githubusercontent.com/jsebold666/razorenhanced.github.io/main/RazorEnhancedAutoUpdater.xml");
        }

        private void CuoClient_Click(object sender, EventArgs e)
        {
            this.openclientlocation.FileName = "ClassicUO.exe";
            this.openclientlocation.Filter = "Executable Files|ClassicUO.exe|Executable Files|*.exe";
            openclientlocation.RestoreDirectory = true;
            if (openclientlocation.ShowDialog(this) == DialogResult.OK)
            {
                cuoClientLabel.Text = openclientlocation.FileName;
                m_Tip.SetToolTip(cuoClientLabel, cuoClientLabel.Text);
            }
            UpdateGUI();
        }
    }
}
