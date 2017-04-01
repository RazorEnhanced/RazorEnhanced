using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
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
			List<RazorEnhanced.Shard> shards;
			RazorEnhanced.Shard.Read(out shards);

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

			if (Directory.Exists(clientFolderLabel.Text) && File.Exists(clientPathLabel.Text))
				okay.Enabled = true;
			else
				okay.Enabled = false;
		}

		internal void UpdateGUI()
		{
			int port = 2593;
			Int32.TryParse(portLabel.Text, out port);

			RazorEnhanced.Shard.Update(shardlistCombobox.Text, clientPathLabel.Text, clientFolderLabel.Text, hostLabel.Text, port, patchEnc.Checked, osiEnc.Checked, true);

			List<RazorEnhanced.Shard> shards;
			RazorEnhanced.Shard.Read(out shards);

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

			List<RazorEnhanced.Shard> shards;
			RazorEnhanced.Shard.Read(out shards);

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
			List<RazorEnhanced.Shard> shards;
			RazorEnhanced.Shard.UpdateLast(shardlistCombobox.Text);
			RazorEnhanced.Shard.Read(out shards);

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
			groupBox2.Enabled = true;
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
			openclientlocation.RestoreDirectory = true;
			if (openclientlocation.ShowDialog(this) == DialogResult.OK)
			{
				clientPathLabel.Text = openclientlocation.FileName;
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

		private void quit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void razorButton2_Click(object sender, EventArgs e)
		{
			if (shardlistCombobox.Text != "")
			{
				RazorEnhanced.Shard.Delete(shardlistCombobox.Text);
			}
			UpdateGUI();
		}

		private void checkupdatebutton_Click(object sender, EventArgs e)
		{
			WebClient client = new WebClient();
			try // Try catch in caso che il server sia irraggiungibile
			{
				string reply = client.DownloadString("http://razorenhanced.org/download/version.dat");

				if (reply != Assembly.GetEntryAssembly().GetName().Version.ToString())
				{
					DialogResult dialogResult = MessageBox.Show("New Version of Razor Enhanced is avaibale! Want open webpage for download it?", "New Version Available", MessageBoxButtons.YesNo);
					if (dialogResult == DialogResult.Yes)
					{
						System.Diagnostics.Process.Start("http://www.razorenhanced.org/");
					}
				}
				else
				{
					DialogResult dialogResult = MessageBox.Show("You already have latest version of Razor Enhanced", "No New Update", MessageBoxButtons.OK);
				}
			}
			catch (Exception ex)
			{
				DialogResult dialogResult = MessageBox.Show("Connection on version check server has failed. " + ex, "Fail to connecto", MessageBoxButtons.OK);
			}
		}
	}
}