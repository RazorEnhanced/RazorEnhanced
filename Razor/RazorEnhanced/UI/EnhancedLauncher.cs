using System;
using System.Collections.Generic;
using System.IO;
using AutoUpdaterDotNET;
using System.Linq;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
    internal partial class EnhancedLauncher : Form
    {
        private const string m_Title = "Welcome to Razor Enhanced";

        public EnhancedLauncher()
        {
            InitializeComponent();
            MaximizeBox = false;
            this.Text = m_Title;
        }

        private void RefreshGUI()
        {
            RazorEnhanced.Shard.Read(out List<Shard> shards);
            clientFolderLabel.Text = clientPathLabel.Text = String.Empty;

            foreach (Shard shard in shards)
            {
                if (shard.Selected)
                {
                    shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shard.Description);
                    clientPathLabel.Text = shard.ClientPath;
                    clientFolderLabel.Text = shard.ClientFolder;
                    cuoClientLabel.Text = shard.CUOClient;
                    hostLabel.Text = shard.Host;
                    portLabel.Text = shard.Port.ToString();
                    patchEnc.Checked = shard.PatchEnc;
                    osiEnc.Checked = shard.OSIEnc;
                }
            }
            
            if (shardlistCombobox.SelectedIndex == -1)
                groupBox2.Enabled = false;

            if (Directory.Exists(clientFolderLabel.Text) && File.Exists(clientPathLabel.Text))
            {
                okay.Enabled = true;
                if (File.Exists(cuoClientLabel.Text))
                {
                    launchCUO.Enabled = true;
                }
            }
            else
            {
                okay.Enabled = false;
                launchCUO.Enabled = false;
            }

            var ip = Assistant.Client.Resolve(hostLabel.Text);
            if (ip == null || ip == System.Net.IPAddress.None || Convert.ToInt32(portLabel.Text) == 0)
            {
                RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                launchCUO.Enabled = false;
                okay.Enabled = false;
            }
        }

        internal void UpdateGUI()
        {
            portLabel.Text = portLabel.Text.Replace(" ", "");
            int port;
            Int32.TryParse(portLabel.Text, out port);

            hostLabel.Text = hostLabel.Text.Replace(" ", "");

            Shard.Update(shardlistCombobox.Text, clientPathLabel.Text, clientFolderLabel.Text, cuoClientLabel.Text, hostLabel.Text, port, patchEnc.Checked, osiEnc.Checked, true);

            Shard.Read(out List<Shard> shards);

            shardlistCombobox.Items.Clear();
            foreach (Shard shard in shards)
            {
                shardlistCombobox.Items.Add(shard.Description);
            }

            RefreshGUI();
        }

        private void EnhancedLauncher_Load(object sender, EventArgs e)
        {
            RefreshGUI();
        }

        private void RazorButton1_Click(object sender, EventArgs e)
        {
            EnhancedAgentAddList addshard = new EnhancedAgentAddList(9);
            addshard.FormClosed += new FormClosedEventHandler(Addshard_refresh);
            addshard.TopMost = true;
            addshard.Show();
        }

        private void Addshard_refresh(object sender, EventArgs e)
        {
            //int port;
            //Int32.TryParse(portLabel.Text, out port);

            RazorEnhanced.Shard.Read(out List<Shard> shards);

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
            RazorEnhanced.Shard.Read(out List<Shard> shards);

            foreach (Shard shard in shards)
            {
                if (shard.Selected)
                {
                    shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shard.Description);
                    clientPathLabel.Text = shard.ClientPath;
                    clientFolderLabel.Text = shard.ClientFolder;
                    cuoClientLabel.Text = shard.CUOClient;
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
                okay.Enabled = true;
            else
                okay.Enabled = false;

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
            FolderBrowserDialog folder = new FolderBrowserDialog
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
            openclientlocation.RestoreDirectory = true;
            if (openclientlocation.ShowDialog(this) == DialogResult.OK)
            {
                clientPathLabel.Text = openclientlocation.FileName;
                clientFolderLabel.Text = Path.GetDirectoryName(openclientlocation.FileName);
                UpdateGUI();
            }
        }



        private void Okay_Click(object sender, EventArgs e)
        {

            UpdateGUI();

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
                MessageBox.Show("Error to access to login.cfg: " + ex,
                "Launch Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                this.Close();
            }
        }


        private void LaunchCUO_Click(object sender, EventArgs e)
        {
            UpdateGUI();

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
            AutoUpdater.Start("https://raw.githubusercontent.com/RazorEnhanced/razorenhanced.github.io/main/RazorEnhancedAutoUpdater.xml");
        }

        private void CuoClient_Click(object sender, EventArgs e)
        {
            openclientlocation.RestoreDirectory = true;
            if (openclientlocation.ShowDialog(this) == DialogResult.OK)
            {
                cuoClientLabel.Text = openclientlocation.FileName;
            }
            UpdateGUI();
        }
    }
}
