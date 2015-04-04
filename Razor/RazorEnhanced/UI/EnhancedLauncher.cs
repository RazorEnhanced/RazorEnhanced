using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.IO;
using Assistant;


namespace RazorEnhanced.UI
{
    
	internal partial class EnhancedLauncher : Form
	{

        
		private const string m_Title = "Welcom to Razor Enhanced";
        public EnhancedLauncher()
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;

		}

        private void EnhancedLauncher_Load(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            List<string> shardlist = new List<string>();
            RazorEnhanced.Settings.LauncherShardList(out shardlist);

            for (int i = 0; i < shardlist.Count; i++)               // Carico elenco shard 
            {
                shardlistCombobox.Items.Add(shardlist[i]);
            }

            string shardselected = RazorEnhanced.Settings.LauncherShardSelected(); // Carico ultimo usato   
            if (shardselected !="")
                shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shardselected);
        }


        private void razorButton1_Click(object sender, EventArgs e)
        {
            EnhancedLauncherAddShard AddShard = new EnhancedLauncherAddShard();
            AddShard.TopMost = true;
            AddShard.Show();
        }

        private void shardlistCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = true;
            string uoclient = "Not Set";
            string uofolder = "Not Set";
            string shardhost = "0.0.0.0";
            string shardport = "0";
            bool patchenc = false;
            bool osienc = false;
            RazorEnhanced.Settings.LauncherShardData(shardlistCombobox.Text, out uoclient, out uofolder, out shardhost, out shardport, out patchenc, out osienc);
            clientlocationL.Text = uoclient;
            uofolderL.Text = uofolder;
            serveraddressT.Text = shardhost;
            serverportT.Text = shardport;
            patchEncy.Checked = patchenc;
            OsiEnc.Checked = osienc;
            RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
        }

        private void patchEncy_CheckedChanged(object sender, EventArgs e)
        {
            RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
        }

        private void OsiEnc_CheckedChanged(object sender, EventArgs e)
        {
            RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
        }

        private void serveraddressT_TextChanged(object sender, EventArgs e)
        {
            RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
        }

        private void serverportT_TextChanged(object sender, EventArgs e)
        {
            RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();

            folder.Description = "Select a UO Data Directory...";

            folder.ShowNewFolderButton = false;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                uofolderL.Text =  folder.SelectedPath;
                RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
            }
        }

        private void bNameCopy_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog(this) == DialogResult.OK)
            {
                clientlocationL.Text = openFile.FileName;
                RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
            }
        }

        private void okay_Click(object sender, EventArgs e)
        {
            RazorEnhanced.Settings.LauncherShardInsert(shardlistCombobox.Text, clientlocationL.Text, uofolderL.Text, serveraddressT.Text, serverportT.Text, patchEncy.Checked, OsiEnc.Checked);
            // Genero Login.cfg
            StreamWriter login;
            bool start = true;
            int port = 0;
            string LoginString = "LoginServer=" + serveraddressT.Text + "," + serverportT.Text;
            string logincfgpath = clientlocationL.Text.Substring(0, clientlocationL.Text.LastIndexOf("\\") + 1);
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
                MessageBox.Show("Error to access to login.cfg: "+ex,
                "Launch Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                start = false;
            }
            // check port
            try
            {
                port = Convert.ToInt32(serverportT.Text);
            }
            catch
            {
                MessageBox.Show("Server port is not valid!.",
                "Server port is not valid!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                start = false;
            }

            // check client
            if (!File.Exists(clientlocationL.Text))
            {
                MessageBox.Show("Client file location is not accessible or not exist!",
                "Client Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                start = false;
            }

            // check data folder
            if (!Directory.Exists(uofolderL.Text))
            {
                MessageBox.Show("Data folder location is not accessible or not exist!",
                "Uo data Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                start = false;
            }
            if (start)
            {
                // avvio
            }
            
        }

        private void quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        internal void RefreshCombo()
        {
            shardlistCombobox.Items.Clear();

            List<string> shardlist = new List<string>();
            RazorEnhanced.Settings.LauncherShardList(out shardlist);

            for (int i = 0; i < shardlist.Count; i++)               // Carico elenco shard 
            {
                shardlistCombobox.Items.Add(shardlist[i]);
            }

            string shardselected = RazorEnhanced.Settings.LauncherShardSelected(); // Carico ultimo usato se presente
            if (shardselected != "")
                shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shardselected);
        }

        private void razorButton2_Click(object sender, EventArgs e)
        {
            if (shardlistCombobox.Text != "")
            {
                RazorEnhanced.Settings.LauncherShardDelete(shardlistCombobox.Text);
                groupBox2.Enabled = false;
                clientlocationL.Text = "Not Set";
                uofolderL.Text = "Not Set";
                serveraddressT.Text = "0.0.0.0";
                serverportT.Text = "0";
                patchEncy.Checked = false;
                OsiEnc.Checked = false;
                shardlistCombobox.Items.Clear();

                List<string> shardlist = new List<string>();
                RazorEnhanced.Settings.LauncherShardList(out shardlist);

                for (int i = 0; i < shardlist.Count; i++)               // Carico elenco shard 
                {
                    shardlistCombobox.Items.Add(shardlist[i]);
                }

                string shardselected = RazorEnhanced.Settings.LauncherShardSelected(); // Carico ultimo usato se presente
                if (shardselected != "")
                    shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shardselected);
            }

        }

      
	}
}
