using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal TextBox ScreenPath { get { return screenPath; } }

		private void screenAutoCap_CheckedChanged(object sender, System.EventArgs e)
		{
			if (screenAutoCap.Focused)
				RazorEnhanced.Settings.General.WriteBool("AutoCap", screenAutoCap.Checked);
		}

		private void setScnPath_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folder = new FolderBrowserDialog
			{
				Description = Language.GetString(LocString.SelSSFolder),
				SelectedPath = RazorEnhanced.Settings.General.ReadString("CapPath"),
				ShowNewFolderButton = true
			};

			if (folder.ShowDialog(this) == DialogResult.OK)
			{
				RazorEnhanced.Settings.General.WriteString("CapPath", folder.SelectedPath);
				screenPath.Text = folder.SelectedPath;

				ReloadScreenShotsList();
			}
		}

		internal void ReloadScreenShotsList()
		{
			if (tabs.SelectedTab != screenshotTab) // No force screen update in not showing tab
				return;

			ScreenCapManager.DisplayTo(screensList);
			if (screenPrev.Image != null)
			{
				screenPrev.Image.Dispose();
				screenPrev.Image = null;
			}
		}

		private void radioFull_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioFull.Checked)
			{
				radioUO.Checked = false;
				RazorEnhanced.Settings.General.WriteBool("CapFullScreen", true);
			}
		}

		private void radioUO_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioUO.Checked)
			{
				radioFull.Checked = false;
				RazorEnhanced.Settings.General.WriteBool("CapFullScreen", false);
			}
		}

		private void screensList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!screensList.Focused)
				return;

			if (screenPrev.Image != null)
			{
				screenPrev.Image.Dispose();
				screenPrev.Image = null;
			}

			if (screensList.SelectedIndex == -1)
				return;

			string file = Path.Combine(RazorEnhanced.Settings.General.ReadString("CapPath"), screensList.SelectedItem.ToString());
			if (!File.Exists(file))
			{
				MessageBox.Show(this, Language.Format(LocString.FileNotFoundA1, file), "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				screensList.Items.RemoveAt(screensList.SelectedIndex);
				screensList.SelectedIndex = -1;
				return;
			}

			using (Stream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				screenPrev.Image = Image.FromStream(reader);
			}
		}

		private void screensList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.Clicks == 1)
			{
				ContextMenu menu = new ContextMenu();
				menu.MenuItems.Add("Delete", new EventHandler(DeleteScreenCap));
				if (screensList.SelectedIndex == -1)
					menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;
				menu.MenuItems.Add("Delete ALL", new EventHandler(ClearScreensDirectory));
				menu.Show(screensList, new Point(e.X, e.Y));
			}
		}

		private void DeleteScreenCap(object sender, System.EventArgs e)
		{
			int sel = screensList.SelectedIndex;
			if (sel == -1)
				return;

			string file = Path.Combine(RazorEnhanced.Settings.General.ReadString("CapPath"), (string)screensList.SelectedItem);
			if (MessageBox.Show(this, Language.Format(LocString.DelConf, file), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			screensList.SelectedIndex = -1;
			if (screenPrev.Image != null)
			{
				screenPrev.Image.Dispose();
				screenPrev.Image = null;
			}

			try
			{
				File.Delete(file);
				screensList.Items.RemoveAt(sel);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Unable to Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			ReloadScreenShotsList();
		}

		private void ClearScreensDirectory(object sender, System.EventArgs e)
		{
			string dir = RazorEnhanced.Settings.General.ReadString("CapPath");
			if (MessageBox.Show(this, Language.Format(LocString.Confirm, dir), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			string[] files = Directory.GetFiles(dir, "*.jpg");
			StringBuilder sb = new StringBuilder();
			int failed = 0;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					File.Delete(files[i]);
				}
				catch
				{
					sb.AppendFormat("{0}\n", files[i]);
					failed++;
				}
			}

			if (failed > 0)
				MessageBox.Show(this, Language.Format(LocString.FileDelError, failed, failed != 1 ? "s" : String.Empty, sb.ToString()), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			ReloadScreenShotsList();
		}

		private void capNow_Click(object sender, System.EventArgs e)
		{
			ScreenCapManager.CaptureNow();
		}

		private void dispTime_CheckedChanged(object sender, System.EventArgs e)
		{
			if (dispTime.Focused)
				RazorEnhanced.Settings.General.WriteBool("CapTimeStamp", dispTime.Checked);
		}
		private void screenPath_TextChanged(object sender, System.EventArgs e)
		{
			RazorEnhanced.Settings.General.WriteString("CapPath", screenPath.Text);
		}

		private void imgFmt_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (imgFmt.SelectedIndex != -1)
				RazorEnhanced.Settings.General.WriteString("ImageFormat", imgFmt.SelectedItem.ToString());
			else
				RazorEnhanced.Settings.General.WriteString("ImageFormat", "jpg");
		}

		private void screenPrev_Click(object sender, System.EventArgs e)
		{
			if (screensList.SelectedItem is String file)
			{
				string tostart = Path.Combine(RazorEnhanced.Settings.General.ReadString("CapPath"), file);
				if (File.Exists(tostart))
					return;

				try
				{
					Process.Start(tostart);
				}
				catch { }
			}
		}
	}
}
