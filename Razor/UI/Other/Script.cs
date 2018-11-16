using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace Assistant
{
	internal partial class MainForm : System.Windows.Forms.Form
	{
		private DataTable scriptTable;

		private static string LoadFromFile(string filename, bool wait, bool loop, bool run, bool autostart)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(filename);
			string fullpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Scripts", filename);
			string text = null;

			if (File.Exists(fullpath))
			{
				text = File.ReadAllText(fullpath);
			}
			else
			{
				return "ERROR: file not found";
			}

			Scripts.EnhancedScript script = new Scripts.EnhancedScript(filename, text, wait, loop, run, autostart);
			string result = script.Create(null);

			if (result == "Created")
			{
				Scripts.EnhancedScripts.TryAdd(filename, script);
			}
			else
			{
				status = "ERROR: " + result;
			}

			return status;
		}

		private void LoadAndInitializeScripts()
		{
			foreach (Scripts.EnhancedScript script in Scripts.EnhancedScripts.Values.ToList())
			{
				script.Stop();
				script.Reset();
			}

			Scripts.EnhancedScripts.Clear();

			scriptlistView.BeginUpdate();
			scriptlistView.Items.Clear();

			DataTable scriptTable = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];

			foreach (DataRow row in scriptTable.Rows)
			{
				string filename = (string)row["Filename"];
				bool wait = (bool)row["Wait"];
				bool loop = (bool)row["Loop"];
				string status = (string)row["Status"];
				bool passkey = (bool)row["HotKeyPass"];
				Keys key = (Keys)row["HotKey"];
				bool autostart = (bool)row["AutoStart"];

				bool run = false;
				if (status == "Running")
					run = true;

				string result = LoadFromFile(filename, wait, loop, run, autostart);

				if (result == "Loaded")
				{
					ListViewItem listitem = new ListViewItem();

					listitem.SubItems.Add(filename);

					listitem.SubItems.Add(status);

					if (loop)
						listitem.SubItems.Add("Yes");
					else
						listitem.SubItems.Add("No");

					if (autostart)
						listitem.SubItems.Add("Yes");
					else
						listitem.SubItems.Add("No");

					if (wait)
						listitem.SubItems.Add("Yes");
					else
						listitem.SubItems.Add("No");


					listitem.SubItems.Add(HotKey.KeyString(key));

					if (passkey)
						listitem.SubItems.Add("Yes");
					else
						listitem.SubItems.Add("No");

					scriptlistView.Items.Add(listitem);

					row["Status"] = "Stopped";
				}
				else
				{
					ListViewItem listitem = new ListViewItem();

					listitem.SubItems.Add("File Not Found");

					listitem.SubItems.Add("Error");

					listitem.SubItems.Add("No");

					listitem.SubItems.Add("No");

					listitem.SubItems.Add("No");

					listitem.SubItems.Add(HotKey.KeyString(key));

					listitem.SubItems.Add("No");

					scriptlistView.Items.Add(listitem);

					row["Status"] = "Error";
				}
			}
			scriptlistView.EndUpdate();
		}

		private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridMoveUp();
		}

		private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridMoveDown();
		}
		private void ScriptGridMoveDown()
		{
			if (scriptTable != null && scriptTable.Rows.Count > 0 && scriptlistView.SelectedItems.Count == 1)
			{
				int rowCount = scriptlistView.Items.Count;
				int index = scriptlistView.SelectedItems[0].Index;

				if (index >= rowCount - 1)
				{
					return;
				}

				DataRow newRow = scriptTable.NewRow();
				// We "clone" the row
				newRow.ItemArray = scriptTable.Rows[index + 1].ItemArray;
				// We remove the old and insert the new
				scriptTable.Rows.RemoveAt(index + 1);
				scriptTable.Rows.InsertAt(newRow, index);
				ReloadScriptTable();
				scriptlistView.Items[index + 1].Selected = true;
			}
		}

		private void ScriptGridMoveUp()
		{
			if (scriptTable != null && scriptTable.Rows.Count > 0 && scriptlistView.SelectedItems.Count == 1)
			{
				int rowCount = scriptlistView.Items.Count;
				int index = scriptlistView.SelectedItems[0].Index;

				if (index == 0) // include the header row
				{
					return;
				}

				DataRow newRow = scriptTable.NewRow();
				// We "clone" the row
				newRow.ItemArray = scriptTable.Rows[index - 1].ItemArray;
				// We remove the old and insert the new
				scriptTable.Rows.RemoveAt(index - 1);
				scriptTable.Rows.InsertAt(newRow, index);
				ReloadScriptTable();
				scriptlistView.Items[index - 1].Selected = true;
			}
		}

		internal void ReloadScriptTable()
		{
			RazorEnhanced.Settings.Save();
			LoadAndInitializeScripts();
			RazorEnhanced.HotKey.Init();
		}

		internal void UpdateScriptGridKey()
		{
			int i = 0;
			scriptlistView.BeginUpdate();
			DataTable scriptTable = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];
			foreach (DataRow row in scriptTable.Rows)
			{
				bool passkey = (bool)row["HotKeyPass"];
				Keys key = (Keys)row["HotKey"];
				scriptlistView.Items[i].SubItems[6].Text = HotKey.KeyString(key);
				if (passkey)
					scriptlistView.Items[i].SubItems[7].Text = "Yes";
				else
					scriptlistView.Items[i].SubItems[7].Text = "No";
				i++;
			}
			scriptlistView.EndUpdate();
		}

		internal void UpdateScriptGrid()
		{

			if (tabs.SelectedTab != scriptingTab)
				return;

			if (scriptlistView.Items.Count > 0)
			{
				scriptlistView.BeginUpdate();
				try
				{
					foreach (ListViewItem litem in scriptlistView.Items)
					{
						string filename = litem.SubItems[1].Text;
						Scripts.EnhancedScript script = Scripts.Search(filename);
						{
							if (script != null)
							{
								litem.SubItems[2].Text = script.Status;
							}
						}
					}
				}
				finally
				{
					scriptlistView.EndUpdate();
				}
			}

		}

		private void playToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RunCurrentScript(true);
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RunCurrentScript(false);
		}

		private void RunCurrentScript(bool run)
		{
			if (scriptlistView.SelectedItems.Count == 1)
			{
				string filename = scriptlistView.SelectedItems[0].SubItems[1].Text;
				Scripts.EnhancedScript script = Scripts.Search(filename);
				if (script != null)
				{
					script.Run = run;
				}
			}
		}

		private void AddScriptInGrid()
		{
			DialogResult result = openFileDialogscript.ShowDialog();

			if (result == DialogResult.OK) // Test result.
			{
				string filename = Path.GetFileName(openFileDialogscript.FileName);
				string scriptPath = openFileDialogscript.FileName.Substring(0, openFileDialogscript.FileName.LastIndexOf("\\") + 1).ToLower();
				string razorPath = (Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf("\\") + 1) + "Scripts\\").ToLower();

				if (scriptPath == razorPath)
				{
					Scripts.EnhancedScript script = Scripts.Search(filename);
					if (script == null)
					{
						scriptTable.Rows.Add(filename, Properties.Resources.red, "Idle", false, false, false, Keys.None, false);
						ReloadScriptTable();
					}
				}
				else
				{
					MessageBox.Show("Error, Script file must be in Scripts folder!");
				}
			}
		}

		private void addToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddScriptInGrid();
		}

		private void buttonScriptAdd_Click(object sender, EventArgs e)
		{
			AddScriptInGrid();
		}

		private void buttonScriptRefresh_Click(object sender, EventArgs e)
		{
			if (scriptTable != null && scriptTable.Rows.Count > 0 && scriptlistView.SelectedItems.Count == 1)
			{
				DataRow row = scriptTable.Rows[scriptlistView.SelectedItems[0].Index];
				string scriptname = row[0].ToString();
				Scripts.EnhancedScript script = Scripts.Search(scriptname);
				if (script != null)
				{
					string fullpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Scripts",
						scriptname);

					if (File.Exists(fullpath) && Scripts.EnhancedScripts.ContainsKey(scriptname))
					{
						string text = File.ReadAllText(fullpath);
						bool loop = script.Loop;
						bool wait = script.Wait;
						bool run = script.Run;
						bool autostart = script.AutoStart;
						bool isRunning = script.IsRunning;

						if (isRunning)
							script.Stop();

						Scripts.EnhancedScript reloaded = new Scripts.EnhancedScript(scriptname, text, wait,
							loop, run, autostart);
						reloaded.Create(null);
						Scripts.EnhancedScripts[scriptname] = reloaded;

						if (isRunning)
							reloaded.Start();
					}
				}
			}
		}

		private void RemoveScriptInGrid()
		{
			if (scriptTable != null && scriptTable.Rows.Count > 0 && scriptlistView.SelectedItems.Count == 1)
			{
				DataRow row = scriptTable.Rows[scriptlistView.SelectedItems[0].Index];
				RunCurrentScript(false);
				scriptTable.Rows.Remove(row);
				ReloadScriptTable();
			}
		}
		private void buttonScriptRemove_Click(object sender, EventArgs e)
		{
			RemoveScriptInGrid();
		}

		private void scriptlistView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				scriptgridMenuStrip.Show(Cursor.Position);
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveScriptInGrid();
		}

		private void buttonScriptDown_Click(object sender, EventArgs e)
		{
			ScriptGridMoveDown();
		}

		private void buttonScriptUp_Click(object sender, EventArgs e)
		{
			ScriptGridMoveUp();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridOpen();
		}

		private void buttonScriptEditorNew_Click(object sender, EventArgs e)
		{
			EnhancedScriptEditor.Init(null); // Open clear editor
		}

		private void ScriptGridOpen()
		{
			string fullPath = null;

			if (scriptTable != null && scriptTable.Rows.Count > 0 && scriptlistView.SelectedItems.Count == 1)
			{
				string filename = scriptlistView.SelectedItems[0].SubItems[1].Text;
				fullPath = (Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf("\\") + 1) + "Scripts\\") + filename;
			}
			if (fullPath != null)
				EnhancedScriptEditor.Init(fullPath);
		}

		private void scriptSearchTextBox_TextChanged(object sender, EventArgs e)
		{
			if (scriptSearchTextBox.Focused)
			{

				for (int i = 0; i < scriptlistView.Items.Count; i++)
				{
					scriptlistView.Items[i].ForeColor = SystemColors.WindowText; // Decolor old search

					if (scriptSearchTextBox.Text != String.Empty)
					{
						if (scriptlistView.Items[i].SubItems[1].Text.ToLower().Contains(scriptSearchTextBox.Text.ToLower()))
						{
							scriptlistView.EnsureVisible(i);
							scriptlistView.Items[i].ForeColor = Color.Blue; // Set highlight color
						}
					}
				}
			}
		}

		private void buttonOpenEditor_Click(object sender, EventArgs e)
		{
			ScriptGridOpen();
		}

		private void buttonScriptPlay_Click(object sender, EventArgs e)
		{
			RunCurrentScript(true);
		}

		private void buttonScriptStop_Click(object sender, EventArgs e)
		{
			RunCurrentScript(false);
		}

		private void textBoxEngineDelay_TextChanged(object sender, EventArgs e)
		{
			Int32.TryParse(textBoxDelay.Text, out int millliseconds);
			TimeSpan delay = TimeSpan.FromMilliseconds(millliseconds);
			Scripts.TimerDelay = delay;
		}

		private void showscriptmessageCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showscriptmessageCheckBox.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowScriptMessageCheckBox", showscriptmessageCheckBox.Checked);
		}

		private void scripterrorlogCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (scripterrorlogCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ScriptErrorLog", scripterrorlogCheckBox.Checked);
				Scripts.ScriptErrorLog = scripterrorlogCheckBox.Checked;
			}
		}
		private void scriptshowStartStopCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (scriptshowStartStopCheckBox.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("ScriptStartStopMessage", scriptshowStartStopCheckBox.Checked);
				Scripts.ScriptStartStopMessage = scriptshowStartStopCheckBox.Checked;
			}
		}

		private void scriptlistView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (scriptlistView.SelectedItems.Count == 1)
			{
				scriptfilelabel.Text = "File: " + scriptlistView.SelectedItems[0].SubItems[1].Text;

				if (scriptlistView.SelectedItems[0].SubItems[3].Text == "Yes")
					scriptloopmodecheckbox.Checked = true;
				else
					scriptloopmodecheckbox.Checked = false;

				if (scriptlistView.SelectedItems[0].SubItems[4].Text == "Yes")
					scriptautostartcheckbox.Checked = true;
				else
					scriptautostartcheckbox.Checked = false;

				if (scriptlistView.SelectedItems[0].SubItems[5].Text == "Yes")
					scriptwaitmodecheckbox.Checked = true;
				else
					scriptwaitmodecheckbox.Checked = false;
			}
		}

		private void ScriptGridAutoStartAtLogin(bool stripmenu)
		{
			if (scriptlistView.SelectedItems.Count != 1) // Selezione multipla o mancata
				return;

			if (stripmenu)
			{
				scriptTable.Rows[scriptlistView.SelectedItems[0].Index]["AutoStart"] = !scriptautostartcheckbox.Checked;
				scriptautostartcheckbox.Checked = !scriptautostartcheckbox.Checked;
				ReloadScriptTable();
			}
			else
			{
				if (!scriptautostartcheckbox.Focused)
					return;

				scriptTable.Rows[scriptlistView.SelectedItems[0].Index]["AutoStart"] = scriptautostartcheckbox.Checked;
			}

			ReloadScriptTable();
		}

		private void scriptautostartcheckbox_CheckedChanged(object sender, EventArgs e)
		{
			ScriptGridAutoStartAtLogin(false);
		}

		private void autoStartAtLoginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridAutoStartAtLogin(true);
		}

		private void ScriptGridLoopMode(bool stripmenu)
		{
			if (scriptlistView.SelectedItems.Count != 1) // Selezione multipla o mancata
				return;

			if (stripmenu)
			{
				scriptTable.Rows[scriptlistView.SelectedItems[0].Index]["Loop"] = !scriptloopmodecheckbox.Checked;
				scriptloopmodecheckbox.Checked = !scriptloopmodecheckbox.Checked;
				ReloadScriptTable();
			}
			else
			{
				if (!scriptloopmodecheckbox.Focused)
					return;

				scriptTable.Rows[scriptlistView.SelectedItems[0].Index]["Loop"] = scriptloopmodecheckbox.Checked;
			}

			ReloadScriptTable();
		}

		private void scriptloopmodecheckbox_CheckedChanged(object sender, EventArgs e)
		{
			ScriptGridLoopMode(false);
		}

		private void loopModeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridLoopMode(true);
		}

		private void ScriptGridWaitBeforeInterrupt(bool stripmenu)
		{
			if (scriptlistView.SelectedItems.Count != 1) // Selezione multipla o mancata
				return;

			if (stripmenu)
			{
				scriptTable.Rows[scriptlistView.SelectedItems[0].Index]["Wait"] = !scriptwaitmodecheckbox.Checked;
				scriptwaitmodecheckbox.Checked = !scriptwaitmodecheckbox.Checked;
				ReloadScriptTable();
			}
			else
			{
				if (!scriptwaitmodecheckbox.Focused)
					return;

				scriptTable.Rows[scriptlistView.SelectedItems[0].Index]["Wait"] = scriptwaitmodecheckbox.Checked;
			}

			ReloadScriptTable();
		}

		private void scriptwaitmodecheckbox_CheckedChanged(object sender, EventArgs e)
		{
			ScriptGridWaitBeforeInterrupt(false);
		}

		private void waitBeforeInterruptToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridWaitBeforeInterrupt(true);
		}
	}
}
