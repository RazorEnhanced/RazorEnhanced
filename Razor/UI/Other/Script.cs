using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Assistant.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        //private DataTable scriptTable;
        // ---------------------------------------------------------------------- UTILS ----------------------------------------------------------------------
        internal static ListViewItem ScriptToTableRow(EnhancedScript script, int index)
        {
            ListViewItem listitem = new ListViewItem();
            listitem.Text = script.Filename;
            listitem.ToolTipText = script.Fullpath;
            var row = listitem.SubItems;

            var text_status = script.Status;
            var text_loop = script.Loop ? "Yes" : "No";
            var text_autostart = script.AutoStart ? "Yes" : "No";
            var text_wait = script.Wait ? "Yes" : "No";
            var text_hotkey = script.HotKey.ToString();
            var text_hotkeypass = script.HotKeyPass ? "Yes" : "No";
            var text_index = Convert.ToString(index);

            row.Add(text_status);
            row.Add(text_loop);
            row.Add(text_autostart);
            row.Add(text_wait);
            row.Add(text_hotkey);
            row.Add(text_hotkeypass);
            row.Add(text_index);
            
            return listitem;
        }


        internal static ScriptListView GetCurrentAllScriptsTab()
        {
            var selectedTab = Engine.MainWindow.AllScriptsTab.SelectedTab;
            if (selectedTab.Controls.Count == 0) return null;

            return (ScriptListView)selectedTab.Controls[0];
        }

        internal static List<Scripts.ScriptItem> GetScriptsCurrentTab()
        {
            ScriptListView scriptListView = GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return null;

            if (scriptListView.SelectedItems.Count != 1) // Selezione multipla o mancata
                return null;
            

            List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
            { list = Scripts.PyScripts; }
            else if (scriptListView.Name == "uosScriptListView")
            { list = Scripts.UosScripts; }
            else if (scriptListView.Name == "csScriptListView")
            { list = Scripts.CsScripts; }

            return list;
        }

        internal static TabControl GetAllScriptsTab()
        {
            return Engine.MainWindow.AllScriptsTab;
        }


        internal static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                        .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                        .ToUpperInvariant();
        }

        // ---------------------------------------------------------------------- LOAD/REFRESH ----------------------------------------------------------------------
        internal void ReloadScriptTable()
        {
            RazorEnhanced.Settings.Save();
            LoadAndInitializeScripts();
            RazorEnhanced.HotKey.Init();
        }

        internal static void LoadScriptList(System.Collections.Generic.List<Scripts.ScriptItem> scripts, ScriptListView scriptlistView)
        {
            foreach (Scripts.ScriptItem item in scripts)
            {
                var script = EnhancedScript.FromScriptItem(item);
                if (script != null) {
                
                    var listitem = ScriptToTableRow(script, item.Position);
                    scriptlistView.Items.Add(listitem);
                }
                else
                {
                    // 17/08/2020 Removed loading not exist script file
                    // 2/15/2021 added back to avoid hotkey issue,
                    //    removed bad file entries at load in settings so should never happen
                    ListViewItem listitem = new ListViewItem();
                    listitem.Text = "File Not Found";
                    listitem.SubItems.Add("Error");
                    listitem.SubItems.Add("No");
                    listitem.SubItems.Add("No");
                    listitem.SubItems.Add("No");
                    listitem.SubItems.Add(HotKey.KeyString(item.Hotkey));
                    listitem.SubItems.Add("No");
                    listitem.SubItems.Add("0");
                    scriptlistView.Items.Add(listitem);
                }
                
            }

        }

        private void LoadAndInitializeScripts()
        {
            //EnhancedScript.ClearAll();
            
            // Save current selected index
            int currentSelectionIndex = 0;
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;
            if (scriptListView.SelectedIndices.Count > 0)
            {
                currentSelectionIndex = scriptListView.SelectedIndices[0];
            }

            // Empty all the controls
            TabControl scriptsTab = MainForm.GetAllScriptsTab();
            if (scriptsTab == null)
                return;
            foreach (TabPage tabPage in scriptsTab.Controls)
            {
                ScriptListView listView = (RazorEnhanced.UI.ScriptListView)tabPage.Controls[0];
                listView.BeginUpdate();
                listView.Items.Clear();
            }

            LoadScriptList(RazorEnhanced.Scripts.PyScripts, pyScriptListView);
            LoadScriptList(RazorEnhanced.Scripts.UosScripts, uosScriptListView);
            LoadScriptList(RazorEnhanced.Scripts.CsScripts, csScriptListView);

            foreach (TabPage tabPage in scriptsTab.Controls)
            {
                ScriptListView listView = (RazorEnhanced.UI.ScriptListView)tabPage.Controls[0];
                listView.EndUpdate();
            }

            if (scriptListView.Items.Count > currentSelectionIndex)
            {
                var selected = scriptListView.Items[currentSelectionIndex];
                selected.Selected = true;
            }

        }
        
        internal void UpdateScriptGrid()
        {

            if (tabs.SelectedTab != AllScripts)
                return;

            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            if (scriptListView.Items.Count > 0)
            {
                scriptListView.BeginUpdate();
                try
                {
                    foreach (ListViewItem litem in scriptListView.Items)
                    {
                        string filename = litem.Text;
                        EnhancedScript script = EnhancedScript.Service.Search(filename);
                        {
                            if (script != null)
                            {
                                string status = script.Status;
                                if (status == "Stopped")
                                {
                                    litem.BackColor = SystemColors.Window;
                                    if (scriptSearchTextBox.Text != String.Empty)
                                    {
                                        if (litem.Text.ToLower().Contains(scriptSearchTextBox.Text.ToLower()))
                                        {
                                            litem.ForeColor = Color.Blue; // Set highlight color
                                        }
                                        else
                                        {
                                            litem.ForeColor = Color.LightGray;
                                        }
                                    }
                                    else
                                    {
                                        litem.ForeColor = SystemColors.WindowText;
                                    }
                                }
                                else
                                {

                                    if (scriptSearchTextBox.Text != String.Empty)
                                    {
                                        if (litem.Text.ToLower().Contains(scriptSearchTextBox.Text.ToLower()))
                                        {
                                            litem.ForeColor = Color.Blue; // Set highlight color
                                            litem.BackColor = Color.DarkGreen;
                                        }
                                        else
                                        {
                                            litem.ForeColor = Color.LightGray;
                                            litem.BackColor = Color.Green;
                                        }
                                    }
                                    else
                                    {
                                        litem.ForeColor = Color.White;
                                        litem.BackColor = Color.DarkGreen;
                                    }
                                }
                                litem.SubItems[1].Text = status;
                            }
                        }
                    }
                }
                finally
                {
                    scriptListView.EndUpdate();
                }
            }

        }




        // ---------------------------------------------------------------------- MOVE ----------------------------------------------------------------------
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptGridMoveUp();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptGridMoveDown();
        }

        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = "0";
            if (InputBox("Move to Index", "Index:", ref value) == DialogResult.OK)
            {
                try
                {
                    ScriptGridMoveTo(Convert.ToInt32(value));
                }
                catch (Exception)
                { }
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonMove = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonMove.Text = "Move";
            buttonCancel.Text = "Cancel";
            buttonMove.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonMove.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonMove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonMove, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonMove;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }


        private void ScriptGridMoveUp()
        {
            if (sorted) // No move script index if user have place some different ordering
                return;

            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null) return;

            List<Scripts.ScriptItem> list = GetScriptsCurrentTab();
            if (list == null || list.Count == 0) return;

            
            int rowCount = scriptListView.Items.Count;
            int index = scriptListView.SelectedItems[0].Index;

            if (index == 0) // include the header row
            {
                return;
            }
            int location = index - 1;

            if (location < 0 || location >= rowCount)
                return;

            var item = list[index];
            list.RemoveAt(index);
            item.Position = location;
            list.Insert(location, item);

            ReloadScriptTable();
            scriptListView.Items[location].Selected = true;
        }

        private void ScriptGridMoveDown()
        {
            if (sorted) // No move script index if user have place some different ordering
                return;

            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            List<Scripts.ScriptItem> list = GetScriptsCurrentTab();
            if (list == null || list.Count == 0) return;

     
            int rowCount = scriptListView.Items.Count;
            int index = scriptListView.SelectedItems[0].Index;
            int location = index + 1;

            if (location < 0 || location >= rowCount)
                return;

            var item = list[index];
            list.RemoveAt(index);
            item.Position = location;
            list.Insert(location, item);

            ReloadScriptTable();
            scriptListView.Items[location].Selected = true;
            
        }

        private void ScriptGridMoveTo(int location)
        {
            if (sorted) // No move script index if user have place some different ordering
                return;

            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            List<Scripts.ScriptItem> list = GetScriptsCurrentTab();
            if (list == null || list.Count == 0) return;
            int rowCount = scriptListView.Items.Count;
            int index = scriptListView.SelectedItems[0].Index;

            if (location >= rowCount) { location = rowCount - 1; }
            if (location < 0) { location = 0; }

                
            var item = list[index];
            list.RemoveAt(index);
            item.Position = location;
            list.Insert(location, item);

            ReloadScriptTable();
            scriptListView.Items[location].Selected = true;
            
        }

        
        

        // ---------------------------------------------------------------------- RUN/STOP/REFRERSH ----------------------------------------------------------------------


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
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            System.Collections.Generic.List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
                list = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                list = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                list = Scripts.CsScripts;
            if (list == null)
                return;

            if (list.Count > 0 && scriptListView.SelectedItems.Count == 1)
            {
                string filename = scriptListView.SelectedItems[0].Text;
                EnhancedScript script = EnhancedScript.Service.Search(filename);
                if (script != null)
                {
                    if (run)
                    {
                        script.Start();
                    }
                    else { 
                        script.Stop();
                    }
                }           
            }
        }      

        
        private void buttonScriptRefresh_Click(object sender, EventArgs e)
        {
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            System.Collections.Generic.List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
                list = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                list = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                list = Scripts.CsScripts;
            if (list == null)
                return;

            if (list.Count > 0 && scriptListView.SelectedItems.Count == 1)
            {
                int index = scriptListView.SelectedItems[0].Index;
                string scriptname = list[index].Filename;
                string fullpath = list[index].FullPath;
                EnhancedScript script = EnhancedScript.Service.Search(fullpath);
                if (script != null)
                {
                    bool isRunning = script.IsRunning;

                    if (isRunning)
                        script.Stop();
                    script.Load();
                    script.LastModified = DateTime.MinValue;
                    if (isRunning)
                        script.Start();
                }
            }
        }



        // ---------------------------------------------------------------------- ADD\REMOVE ----------------------------------------------------------------------
        private void AddScriptInGrid()
        {
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            openFileDialogscript.Filter = "Script Files|";  // *.py; *.uos; *.txt; *.cs

            System.Collections.Generic.List<Scripts.ScriptItem> list = null;           
            if (scriptListView.Name == "pyScriptListView")
            {
                list = Scripts.PyScripts;
                openFileDialogscript.Filter += "*.py;";
            }
            if (scriptListView.Name == "uosScriptListView")
            {
                list = Scripts.UosScripts;
                openFileDialogscript.Filter += "*.uos;";
            }
            if (scriptListView.Name == "csScriptListView")
            {
                list = Scripts.CsScripts;
                openFileDialogscript.Filter += "*.cs;";
            }
            if (list == null)
                return;

            DialogResult result = openFileDialogscript.ShowDialog();

            if (result == DialogResult.OK) // Test result.
            {
                string filename = Path.GetFileName(openFileDialogscript.FileName);
                string scriptPath = NormalizePath(openFileDialogscript.FileName.Substring(0, openFileDialogscript.FileName.LastIndexOf("\\") + 1));

                Scripts.ScriptItem script = Scripts.FindScript(filename);
                if (script == null)
                {
                    var scriptItem = new Scripts.ScriptItem();
                    scriptItem.Filename = filename;
                    scriptItem.Status = "Idle";
                    scriptItem.Loop = false;
                    scriptItem.Wait = false;
                    scriptItem.AutoStart = false;
                    scriptItem.HotKeyPass = false;
                    scriptItem.Hotkey = Keys.None;
                    scriptItem.FullPath = Path.Combine(scriptPath.ToLower(), filename);
                    EnhancedScript.FromScriptItem(scriptItem);
                    Scripts.UpdateScriptItems();
                    ReloadScriptTable();
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


        


        private void RemoveScriptInGrid()
        {
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            System.Collections.Generic.List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
                list = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                list = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                list = Scripts.CsScripts;
            if (list == null)
                return;

            if (list.Count > 0 && scriptListView.SelectedItems.Count == 1)
            {
                RunCurrentScript(false);

                int removeIndex = -1;
                for (int i=0; i< list.Count; i++)
                {
                    Scripts.ScriptItem item = list[i];
                    if (item.Filename == scriptListView.SelectedItems[0].Text)
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex >= 0)
                {
                    var item = list[removeIndex];
                    var script = EnhancedScript.FromScriptItem(item);
                    if (script != null)
                    {
                        EnhancedScript.Service.RemoveScript(script);
                        Scripts.UpdateScriptItems();
                    }
                }
                ReloadScriptTable();
                HotKey.Init();
            }
        }
        private void buttonScriptRemove_Click(object sender, EventArgs e)
        {
            RemoveScriptInGrid();
        }

        // ---------------------------------------------------------------------- HOTKEYS ----------------------------------------------------------------------

        internal void UpdateScriptGridKey()
        {
            int i = 0;

            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            scriptListView.BeginUpdate();
            DataTable scriptTable = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];
            //int index = 0;
            foreach (DataRow row in scriptTable.Rows)
            {
                bool passkey = (bool)row["HotKeyPass"];
                Keys key = (Keys)Convert.ToInt32(row["HotKey"]);
                scriptListView.Items[i].SubItems[5].Text = HotKey.KeyString(key);
                if (passkey)
                    scriptListView.Items[i].SubItems[6].Text = "Yes";
                else
                    scriptListView.Items[i].SubItems[6].Text = "No";
                i++;
            }
            scriptListView.EndUpdate();
        }

        // ---------------------------------------------------------------------- CONTEXT MENU ----------------------------------------------------------------------
        // right click on the script list
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

        private void buttonScriptTo_Click(object sender, EventArgs e)
        {
            moveToToolStripMenuItem_Click(sender, e);
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
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            System.Collections.Generic.List<Scripts.ScriptItem> scriptTable = null;
            if (scriptListView.Name == "pyScriptListView")
                scriptTable = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                scriptTable = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                scriptTable = Scripts.CsScripts;
            if (scriptTable == null)
                return;

            string fullPath = null;

            if (scriptTable.Count > 0 && scriptListView.SelectedItems.Count == 1)
            {
                string scriptname = scriptListView.SelectedItems[0].Text;
                Scripts.ScriptItem scriptItem = Scripts.FindScript(scriptname);
                if (scriptItem != null) 
                    fullPath = scriptItem.FullPath;
            }
            if (fullPath != null)
                EnhancedScriptEditor.Init(fullPath);
        }

        private void scriptSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            if (scriptSearchTextBox.Focused)
            {
                for (int i = 0; i < scriptListView.Items.Count; i++)
                {
                    scriptListView.Items[i].ForeColor = SystemColors.WindowText; // Decolor old search

                    if (scriptSearchTextBox.Text != String.Empty)
                    {
                        if (scriptListView.Items[i].Text.ToLower().Contains(scriptSearchTextBox.Text.ToLower()))
                        {
                            scriptListView.EnsureVisible(i);
                            scriptListView.Items[i].ForeColor = Color.Blue; // Set highlight color
                        }
                        else
                        {
                            scriptListView.Items[i].ForeColor = Color.LightGray;
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
            ScriptListView scriptListView = (ScriptListView)sender;
            if (scriptListView == null)
                return;
            if (scriptListView.SelectedItems.Count == 1)
            {
                string filename = scriptListView.SelectedItems[0].Text;
                string fullPathName = Scripts.GetFullPathForScript(filename);

                if (scriptListView.SelectedItems[0].SubItems[2].Text == "Yes")
                    scriptloopmodecheckbox.Checked = true;
                else
                    scriptloopmodecheckbox.Checked = false;

                if (scriptListView.SelectedItems[0].SubItems[3].Text == "Yes")
                    scriptautostartcheckbox.Checked = true;
                else
                    scriptautostartcheckbox.Checked = false;

                if (scriptListView.SelectedItems[0].SubItems[4].Text == "Yes")
                    scriptwaitmodecheckbox.Checked = true;
                else
                    scriptwaitmodecheckbox.Checked = false;
            }
        }

        private void ScriptGridAutoStartAtLogin(bool stripmenu)
        {
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            if (scriptListView.SelectedItems.Count != 1) // Selezione multipla o mancata
                return;

            if (stripmenu)
                scriptautostartcheckbox.Checked = !scriptautostartcheckbox.Checked;

            if (!scriptautostartcheckbox.Focused)
                return;


            System.Collections.Generic.List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
                list = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                list = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                list = Scripts.CsScripts;
            if (list == null)
                return;

            if (list == null || list.Count == 0) return;

            var selectedRow = scriptListView.SelectedItems[0];
            Scripts.ScriptItem item = Scripts.FindScript(selectedRow.Text);
            if (item == null) return;

            var script = EnhancedScript.FromScriptItem(item);
            if (script == null) return;

            item.AutoStart = scriptautostartcheckbox.Checked;
            script.AutoStart = scriptautostartcheckbox.Checked;
            
            if (scriptautostartcheckbox.Checked)
                scriptListView.SelectedItems[0].SubItems[3].Text = "Yes";
            else
                scriptListView.SelectedItems[0].SubItems[3].Text = "No";

            //ReloadScriptTable();
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
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            if (scriptListView.SelectedItems.Count != 1) // Selezione multipla o mancata
                return;

            if (stripmenu)
                scriptloopmodecheckbox.Checked = !scriptloopmodecheckbox.Checked;

            if (!scriptloopmodecheckbox.Focused)
                return;


            System.Collections.Generic.List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
                list = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                list = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                list = Scripts.CsScripts;

            if (list == null || list.Count == 0 ) return;

            var selectedRow = scriptListView.SelectedItems[0];
            Scripts.ScriptItem item = Scripts.FindScript(selectedRow.Text);
            if (item == null) return;

            var script = EnhancedScript.FromScriptItem(item);
            if (script == null) return;
            
            script.Loop = scriptloopmodecheckbox.Checked;
            item.Loop = scriptloopmodecheckbox.Checked;
            
            
            if (scriptloopmodecheckbox.Checked)
                scriptListView.SelectedItems[0].SubItems[2].Text = "Yes";
            else
                scriptListView.SelectedItems[0].SubItems[2].Text = "No";
            
            //ReloadScriptTable();    
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
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            if (scriptListView.SelectedItems.Count != 1) // Selezione multipla o mancata
                return;

            if (stripmenu)
                scriptwaitmodecheckbox.Checked = !scriptwaitmodecheckbox.Checked;

            if (!scriptwaitmodecheckbox.Focused)
                return;

            System.Collections.Generic.List<Scripts.ScriptItem> list = null;
            if (scriptListView.Name == "pyScriptListView")
                list = Scripts.PyScripts;
            if (scriptListView.Name == "uosScriptListView")
                list = Scripts.UosScripts;
            if (scriptListView.Name == "csScriptListView")
                list = Scripts.CsScripts;

            if (list == null || list.Count == 0) return;

            var selectedRow = scriptListView.SelectedItems[0];
            Scripts.ScriptItem item = Scripts.FindScript(selectedRow.Text);
            if (item == null) return;

            var script = EnhancedScript.FromScriptItem(item);
            if (script == null) return;

            script.Wait = scriptwaitmodecheckbox.Checked;
            item.Wait = scriptwaitmodecheckbox.Checked;

            if (scriptwaitmodecheckbox.Checked)
                scriptListView.SelectedItems[0].SubItems[4].Text = "Yes";
            else
                scriptListView.SelectedItems[0].SubItems[4].Text = "No";

            //ReloadScriptTable();
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
            ScriptListView scriptListView = MainForm.GetCurrentAllScriptsTab();
            if (scriptListView == null)
                return;

            sorted = true;
            // Get the new sorting column.
            ColumnHeader new_sorting_column = scriptListView.Columns[e.Column];

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
            scriptListView.ListViewItemSorter =
                new ListViewComparer(e.Column, sort_order);

            // Sort.
            scriptListView.Sort();
        }
    }

    public class ListViewComparer : System.Collections.IComparer
    {
        private readonly int ColumnNumber;
        private readonly SortOrder SortOrder;

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
