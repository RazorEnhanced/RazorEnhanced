using RazorEnhanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        internal CheckBox CaptureMibsCheckBox { get { return captureMibsCheckBox; } }
        internal TextBox MibPathTextBox { get { return mibPathTextBox; } }
        internal CheckBox AddXYToGumpCheckBox { get { return addXYToGumpCheckBox; } }
        internal CheckBox CaptureTmapsCheckBox { get { return captureTmapsCheckBox; } }
        internal TextBox TmapPathTextBox { get { return tmapPathTextBox; } }
        internal CheckBox DisplayXYUnderMapCheckBox { get { return displayXYUnderMapCheckBox; } }
        internal ListBox MibGumpIdListBox { get { return mibGumpIdListBox; } }

        private static readonly List<uint> m_MibGumpIds = new List<uint>();
        internal static List<uint> MibGumpIds { get { return m_MibGumpIds; } }

        private const uint DefaultMibGumpId = 1426736667;

        private void captureMibsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (captureMibsCheckBox.Focused)
                RazorEnhanced.Settings.General.WriteBool("CaptureMibsCheckBox", captureMibsCheckBox.Checked);
        }

        private void mibSetPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog
            {
                Description = "Select MIB Capture Folder",
                SelectedPath = RazorEnhanced.Settings.General.ReadString("MibCapturePath"),
                ShowNewFolderButton = true
            };

            if (folder.ShowDialog(this) == DialogResult.OK)
            {
                RazorEnhanced.Settings.General.WriteString("MibCapturePath", folder.SelectedPath);
                mibPathTextBox.Text = folder.SelectedPath;
            }
        }

        private void mibClearPathButton_Click(object sender, EventArgs e)
        {
            mibPathTextBox.Text = "";
            RazorEnhanced.Settings.General.WriteString("MibCapturePath", "");
        }

        private void addXYToGumpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (addXYToGumpCheckBox.Focused)
                RazorEnhanced.Settings.General.WriteBool("AddXYToGumpCheckBox", addXYToGumpCheckBox.Checked);
        }

        private void mibGumpIdAddButton_Click(object sender, EventArgs e)
        {
            string text = mibGumpIdTextBox.Text.Trim();
            if (string.IsNullOrEmpty(text))
                return;

            uint gumpId;
            if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (!uint.TryParse(text.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out gumpId))
                    return;
            }
            else
            {
                if (!uint.TryParse(text, out gumpId))
                    return;
            }

            if (!m_MibGumpIds.Contains(gumpId))
            {
                m_MibGumpIds.Add(gumpId);
                mibGumpIdListBox.Items.Add(gumpId.ToString());
                SaveMibGumpIds();
            }
            mibGumpIdTextBox.Text = "";
        }

        private void mibGumpIdRemoveMenuItem_Click(object sender, EventArgs e)
        {
            if (mibGumpIdListBox.SelectedIndex < 0)
                return;

            string selected = mibGumpIdListBox.SelectedItem.ToString();
            if (uint.TryParse(selected, out uint gumpId))
            {
                m_MibGumpIds.Remove(gumpId);
            }
            mibGumpIdListBox.Items.RemoveAt(mibGumpIdListBox.SelectedIndex);
            SaveMibGumpIds();
        }

        internal void LoadMibGumpIds()
        {
            m_MibGumpIds.Clear();
            mibGumpIdListBox.Items.Clear();

            string stored = RazorEnhanced.Settings.General.ReadString("MibGumpIds");
            if (string.IsNullOrEmpty(stored))
            {
                m_MibGumpIds.Add(DefaultMibGumpId);
                mibGumpIdListBox.Items.Add(DefaultMibGumpId.ToString());
                SaveMibGumpIds();
            }
            else
            {
                string[] parts = stored.Split(',');
                foreach (string part in parts)
                {
                    if (uint.TryParse(part.Trim(), out uint id))
                    {
                        if (!m_MibGumpIds.Contains(id))
                        {
                            m_MibGumpIds.Add(id);
                            mibGumpIdListBox.Items.Add(id.ToString());
                        }
                    }
                }
                if (m_MibGumpIds.Count == 0)
                {
                    m_MibGumpIds.Add(DefaultMibGumpId);
                    mibGumpIdListBox.Items.Add(DefaultMibGumpId.ToString());
                    SaveMibGumpIds();
                }
            }
        }

        private void SaveMibGumpIds()
        {
            string value = string.Join(",", m_MibGumpIds.Select(id => id.ToString()));
            RazorEnhanced.Settings.General.WriteString("MibGumpIds", value);
        }

        private void captureTmapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (captureTmapsCheckBox.Focused)
                RazorEnhanced.Settings.General.WriteBool("CaptureTmapsCheckBox", captureTmapsCheckBox.Checked);
        }

        private void displayXYUnderMapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (displayXYUnderMapCheckBox.Focused)
                RazorEnhanced.Settings.General.WriteBool("DisplayXYUnderMapCheckBox", displayXYUnderMapCheckBox.Checked);
        }

        private void tmapSetPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog
            {
                Description = "Select Treasure Map Capture Folder",
                SelectedPath = RazorEnhanced.Settings.General.ReadString("TmapCapturePath"),
                ShowNewFolderButton = true
            };

            if (folder.ShowDialog(this) == DialogResult.OK)
            {
                RazorEnhanced.Settings.General.WriteString("TmapCapturePath", folder.SelectedPath);
                tmapPathTextBox.Text = folder.SelectedPath;
            }
        }

        private void tmapClearPathButton_Click(object sender, EventArgs e)
        {
            tmapPathTextBox.Text = "";
            RazorEnhanced.Settings.General.WriteString("TmapCapturePath", "");
        }
    }
}
