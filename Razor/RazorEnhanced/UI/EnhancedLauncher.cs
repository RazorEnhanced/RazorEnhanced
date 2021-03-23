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
        }

        internal void UpdateGUI()
        {
            portLabel.Text = portLabel.Text.Replace(" ", "");
            int port = 2593;
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

        private void razorButton1_Click(object sender, EventArgs e)
        {
            EnhancedAgentAddList addshard = new EnhancedAgentAddList(9);
            addshard.FormClosed += new FormClosedEventHandler(addshard_refresh);
            addshard.TopMost = true;
            addshard.Show();
        }

        private void addshard_refresh(object sender, EventArgs e)
        {
            int port = 2593;
            Int32.TryParse(portLabel.Text, out port);

            RazorEnhanced.Shard.Read(out List<Shard> shards);

            shardlistCombobox.Items.Clear();
            foreach (Shard shard in shards)
            {
                shardlistCombobox.Items.Add(shard.Description);
            }

            RefreshGUI();
            groupBox2.Enabled = true;
        }

        private void shardlistCombobox_SelectedIndexChanged(object sender, EventArgs e)
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

        private void patchEncy_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void OsiEnc_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void serveraddressT_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void serverportT_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void bNameCopy_Click(object sender, EventArgs e)
        {
            openclientlocation.RestoreDirectory = true;
            if (openclientlocation.ShowDialog(this) == DialogResult.OK)
            {
                clientPathLabel.Text = openclientlocation.FileName;
                clientFolderLabel.Text = Path.GetDirectoryName(openclientlocation.FileName);
                UpdateGUI();
            }
        }



        private void okay_Click(object sender, EventArgs e)
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


        private void launchCUO_Click(object sender, EventArgs e)
        {
            UpdateGUI();

            // Need to fix up settuings.json
            StreamWriter login;
            string LoginString = "LoginServer=" + hostLabel.Text + "," + portLabel.Text;
            string logincfgpath = cuoClientLabel.Text.Substring(0, cuoClientLabel.Text.LastIndexOf("\\") + 1);
        }


        private void quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void razorButton2_Click(object sender, EventArgs e)
        {
            if (shardlistCombobox.Text != String.Empty)
            {
                RazorEnhanced.Shard.Delete(shardlistCombobox.Text);
            }
            UpdateGUI();
        }

        private void checkupdatebutton_Click(object sender, EventArgs e)
        {
            // AutoUpdater
            AutoUpdater.ReportErrors = true;
            AutoUpdater.Start("http://www.RazorEnhanced.net/download/RazorEnhancedAutoUpdater.xml");
        }

        private void cuoClient_Click(object sender, EventArgs e)
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
