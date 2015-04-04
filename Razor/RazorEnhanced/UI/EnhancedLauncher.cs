using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Assistant;

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
			List<RazorEnhanced.Shard> shards;
			RazorEnhanced.Settings.Shards.Read(out shards);

			foreach (Shard shard in shards)
			{
				if (shard.Selected)
				{
					shardlistCombobox.SelectedIndex = shardlistCombobox.Items.IndexOf(shard.Description);
					clientPathLabel.Text = shard.ClientPath;
					clientFolderLabel.Text = shard.ClientFolder;
					hostLabel.Text = shard.Host;
					portLabel.Text = shard.Port.ToString();
					patchEnc.Checked = shard.PatchEnc;
					osiEnc.Checked = shard.OSIEnc;
				}
			}

			if (shardlistCombobox.SelectedIndex == -1)
				groupBox2.Enabled = false;
		}

		private void UpdateGUI()
		{
			int port = 2593;
			Int32.TryParse(portLabel.Text, out port);

			RazorEnhanced.Settings.Shards.Update(shardlistCombobox.Text, clientPathLabel.Text, clientFolderLabel.Text, hostLabel.Text, port, patchEnc.Checked, osiEnc.Checked, true);

			List<RazorEnhanced.Shard> shards;
			RazorEnhanced.Settings.Shards.Read(out shards);

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
			EnhancedLauncherAddShard AddShard = new EnhancedLauncherAddShard();
			AddShard.TopMost = true;
			AddShard.Show();
			RefreshGUI();
		}

		private void shardlistCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshGUI();
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
			FolderBrowserDialog folder = new FolderBrowserDialog();

			folder.Description = "Select a UO Data Directory...";

			folder.ShowNewFolderButton = false;
			if (folder.ShowDialog() == DialogResult.OK)
			{
				clientFolderLabel.Text = folder.SelectedPath;
				UpdateGUI();
			}
		}

		private void bNameCopy_Click(object sender, EventArgs e)
		{
			if (openFile.ShowDialog(this) == DialogResult.OK)
			{
				clientPathLabel.Text = openFile.FileName;
				UpdateGUI();
			}
		}

		private void okay_Click(object sender, EventArgs e)
		{
			UpdateGUI();

			// Genero Login.cfg
			StreamWriter login;
			bool start = true;
			int port = 0;
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
				start = false;
			}

			// check client
			if (!File.Exists(clientPathLabel.Text))
			{
				MessageBox.Show("Client file location is not accessible or not exist!",
				"Client Error!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				start = false;
			}

			// check data folder
			if (!Directory.Exists(clientFolderLabel.Text))
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

		private void razorButton2_Click(object sender, EventArgs e)
		{
			if (shardlistCombobox.Text != "")
			{
				RazorEnhanced.Settings.Shards.Delete(shardlistCombobox.Text);
			}
		}
	}
}
