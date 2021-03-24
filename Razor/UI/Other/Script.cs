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
	public partial class MainForm : System.Windows.Forms.Form
	{
		private DataTable scriptTable;

		private static string LoadFromFile(string filename, bool wait, bool loop, bool run, bool autostart)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(filename);
			string fullpath = Path.Combine(Assistant.Engine.RootPath, "Scripts", filename);
			string text = null;

			if (File.Exists(fullpath))
			{
				//text = File.ReadAllText(fullpath);
			}
			else
			{
				return "ERROR: file not found";
			}

			Scripts.EnhancedScript script = new Scripts.EnhancedScript(filename, text, wait, loop, run, autostart);
			//string result = script.Create(null);

			//if (result == "Created")
            if (true)
			{
				Scripts.EnhancedScripts.TryAdd(filename, script);
			}
			//else
			//{
			//	status = "ERROR: " + result;
			//}

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
				Keys key = (Keys)Convert.ToInt32(row["HotKey"]);
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
                    // 17/08/2020 Removed loading not exist script file
                    // 2/15/2021 added back to avoid hotkey issue,
                    //    removed bad file entries at load in settings so should never happen
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
			if (sorted) // No move script index if user have place some different ordering
				return;

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
			if (sorted) // No move script index if user have place some different ordering
				return;

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
				Keys key = (Keys)Convert.ToInt32(row["HotKey"]);
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

		internal static string NormalizePath(string path)
		{
			return Path.GetFullPath(new Uri(path).LocalPath)
						.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
						.ToUpperInvariant();
		}


		private void AddScriptInGrid()
		{
            openFileDialogscript.Filter = "Script Files|*.py;*.uos;*.txt";
            DialogResult result = openFileDialogscript.ShowDialog();

            if (result == DialogResult.OK) // Test result.
            {
                string filename = Path.GetFileName(openFileDialogscript.FileName);
                string scriptPath = NormalizePath(openFileDialogscript.FileName.Substring(0, openFileDialogscript.FileName.LastIndexOf("\\") + 1));
                string razorPath = NormalizePath(Path.Combine(Assistant.Engine.RootPath, "Scripts"));

                if (scriptPath.Equals(razorPath, StringComparison.OrdinalIgnoreCase))
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
					string fullpath = Path.Combine(Assistant.Engine.RootPath, "Scripts",
						scriptname);

                    if (File.Exists(fullpath) && Scripts.EnhancedScripts.ContainsKey(scriptname))
                    {
                        //string text = File.ReadAllText(fullpath);
                        //bool loop = script.Loop;
                        //bool wait = script.Wait;
                        //bool run = script.Run;
                        //bool autostart = script.AutoStart;
                        bool isRunning = script.IsRunning;

                        if (isRunning)
                            script.Stop();

                        //Scripts.EnhancedScript reloaded = new Scripts.EnhancedScript(scriptname, text, wait,
                        //	loop, run, autostart);
                        //reloaded.Create(null);
                        Scripts.EnhancedScripts[scriptname].FileChangeDate = DateTime.MinValue;

                        if (isRunning)
                            script.Start();
                    }
				}
			}
		}

		private void RemoveScriptInGrid()
		{
			if (scriptTable != null && scriptTable.Rows.Count > 0 && scriptlistView.SelectedItems.Count == 1)
			{
				RunCurrentScript(false);

				foreach (DataRow row in scriptTable.Rows)
				{
					if ((string)row["Filename"] == scriptlistView.SelectedItems[0].SubItems[1].Text)
					{
						row.Delete();
						break;
					}
				}

				scriptlistView.SelectedItems[0].Remove();

				Settings.Save();
				HotKey.Init();
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
				//fullPath = (Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf("\\") + 1) + "Scripts\\") + filename;
				fullPath = Path.Combine(Assistant.Engine.RootPath, "Scripts", filename);
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
				scriptautostartcheckbox.Checked = !scriptautostartcheckbox.Checked;

			if (!scriptautostartcheckbox.Focused)
				return;

			foreach (DataRow row in scriptTable.Rows)
			{
				if ((string)row["Filename"] == scriptlistView.SelectedItems[0].SubItems[1].Text)
					row["AutoStart"] = scriptautostartcheckbox.Checked;
			}

			if (scriptautostartcheckbox.Checked)
				scriptlistView.SelectedItems[0].SubItems[4].Text = "Yes";
			else
				scriptlistView.SelectedItems[0].SubItems[4].Text = "No";

			//ReloadScriptTable();
			Settings.Save();
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
				scriptloopmodecheckbox.Checked = !scriptloopmodecheckbox.Checked;

			if (!scriptloopmodecheckbox.Focused)
				return;

			foreach (DataRow row in scriptTable.Rows)
			{
				if ((string)row["Filename"] == scriptlistView.SelectedItems[0].SubItems[1].Text)
					row["Loop"] = scriptloopmodecheckbox.Checked;
			}

			if (scriptloopmodecheckbox.Checked)
				scriptlistView.SelectedItems[0].SubItems[3].Text = "Yes";
			else
				scriptlistView.SelectedItems[0].SubItems[3].Text = "No";

			ReloadScriptTable();
			//Settings.Save();
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
				scriptwaitmodecheckbox.Checked = !scriptwaitmodecheckbox.Checked;

			if (!scriptwaitmodecheckbox.Focused)
				return;

			foreach (DataRow row in scriptTable.Rows)
			{
				if ((string)row["Filename"] == scriptlistView.SelectedItems[0].SubItems[1].Text)
					row["Wait"] = scriptwaitmodecheckbox.Checked;
			}

			if (scriptwaitmodecheckbox.Checked)
				scriptlistView.SelectedItems[0].SubItems[5].Text = "Yes";
			else
				scriptlistView.SelectedItems[0].SubItems[5].Text = "No";

			ReloadScriptTable();
			//Settings.Save();
		}

		private void scriptwaitmodecheckbox_CheckedChanged(object sender, EventArgs e)
		{
			ScriptGridWaitBeforeInterrupt(false);
		}

		private void waitBeforeInterruptToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptGridWaitBeforeInterrupt(true);
		}

		private ColumnHeader SortingColumn = null;
		private bool sorted = false;
		private void scriptlistView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			sorted = true;
			// Get the new sorting column.
			ColumnHeader new_sorting_column = scriptlistView.Columns[e.Column];

			// Figure out the new sorting order.
			System.Windows.Forms.SortOrder sort_order;
			if (SortingColumn == null)
			{
				// New column. Sort ascending.
				sort_order = SortOrder.Ascending;
			}
			else
			{
				// See if this is the same column.
				if (new_sorting_column == SortingColumn)
				{
					// Same column. Switch the sort order.
					if (SortingColumn.Text.StartsWith("> "))
					{
						sort_order = SortOrder.Descending;
					}
					else
					{
						sort_order = SortOrder.Ascending;
					}
				}
				else
				{
					// New column. Sort ascending.
					sort_order = SortOrder.Ascending;
				}

				// Remove the old sort indicator.
				SortingColumn.Text = SortingColumn.Text.Substring(2);
			}

			// Display the new sort order.
			SortingColumn = new_sorting_column;
			if (sort_order == SortOrder.Ascending)
			{
				SortingColumn.Text = "> " + SortingColumn.Text;
			}
			else
			{
				SortingColumn.Text = "< " + SortingColumn.Text;
			}

			// Create a comparer.
			scriptlistView.ListViewItemSorter =
				new ListViewComparer(e.Column, sort_order);

			// Sort.
			scriptlistView.Sort();
		}
	}

	public class ListViewComparer : System.Collections.IComparer
	{
		private int ColumnNumber;
		private SortOrder SortOrder;

		public ListViewComparer(int column_number,
			SortOrder sort_order)
		{
			ColumnNumber = column_number;
			SortOrder = sort_order;
		}

		// Compare two ListViewItems.
		public int Compare(object object_x, object object_y)
		{
			// Get the objects as ListViewItems.
			ListViewItem item_x = object_x as ListViewItem;
			ListViewItem item_y = object_y as ListViewItem;

			// Get the corresponding sub-item values.
			string string_x;
			if (item_x.SubItems.Count <= ColumnNumber)
			{
				string_x = "";
			}
			else
			{
				string_x = item_x.SubItems[ColumnNumber].Text;
			}

			string string_y;
			if (item_y.SubItems.Count <= ColumnNumber)
			{
				string_y = "";
			}
			else
			{
				string_y = item_y.SubItems[ColumnNumber].Text;
			}

			// Compare them.
			int result;
			double double_x, double_y;
			if (double.TryParse(string_x, out double_x) &&
				double.TryParse(string_y, out double_y))
			{
				// Treat as a number.
				result = double_x.CompareTo(double_y);
			}
			else
			{
				DateTime date_x, date_y;
				if (DateTime.TryParse(string_x, out date_x) &&
					DateTime.TryParse(string_y, out date_y))
				{
					// Treat as a date.
					result = date_x.CompareTo(date_y);
				}
				else
				{
					// Treat as a string.
					result = string_x.CompareTo(string_y);
				}
			}

			// Return the correct result depending on whether
			// we're sorting ascending or descending.
			if (SortOrder == SortOrder.Ascending)
			{
				return result;
			}
			else
			{
				return -result;
			}
		}
	}
}
