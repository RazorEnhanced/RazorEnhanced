using RazorEnhanced;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal TextBox HotKeyTextBox { get { return hotkeytextbox; } }
		internal TreeView HotKeyTreeView { get { return hotkeytreeView; } }
		internal Label HotKeyKeyMasterLabel { get { return hotkeyKeyMasterLabel; } }
		internal Label HotKeyStatusLabel { get { return hotkeyStatusLabel; } }
		internal TextBox HotKeyKeyMasterTextBox { get { return hotkeyKeyMasterTextBox; } }

		private void hotkeySetButton_Click(object sender, EventArgs e)
		{
			if (hotkeytreeView.SelectedNode != null && hotkeytreeView.SelectedNode.Name != null && hotkeytextbox.Text != String.Empty && hotkeytextbox.Text != "None")
			{
				if (hotkeytreeView.SelectedNode.Name == String.Empty)
				{
					return;
				}
				if (hotkeytreeView.SelectedNode.Parent.Name != null && hotkeytreeView.SelectedNode.Parent.Name == "TList")
					RazorEnhanced.HotKey.UpdateTargetKey(hotkeytreeView.SelectedNode, hotkeypassCheckBox.Checked);     // Aggiorno hotkey target
				else if (hotkeytreeView.SelectedNode.Parent.Name != null && hotkeytreeView.SelectedNode.Parent.Name == "SList")
				{
					RazorEnhanced.HotKey.UpdateScriptKey(hotkeytreeView.SelectedNode, hotkeypassCheckBox.Checked);     // Aggiorno hotkey Script
				}
				else if (hotkeytreeView.SelectedNode.Parent.Name != null && hotkeytreeView.SelectedNode.Parent.Name == "DList")
				{
					RazorEnhanced.HotKey.UpdateDressKey(hotkeytreeView.SelectedNode, hotkeypassCheckBox.Checked);     // Aggiorno hotkey Dress List
				}
				else
					RazorEnhanced.HotKey.UpdateKey(hotkeytreeView.SelectedNode, hotkeypassCheckBox.Checked);
			}
		}

		private void hotkeyClearButton_Click(object sender, EventArgs e)
		{
			if (hotkeytreeView.SelectedNode != null && hotkeytreeView.SelectedNode.Name != null)
			{
				if (hotkeytreeView.SelectedNode.Name == String.Empty)
				{
					return;
				}
				if (hotkeytreeView.SelectedNode.Parent.Name != null)
					RazorEnhanced.HotKey.ClearKey(hotkeytreeView.SelectedNode, hotkeytreeView.SelectedNode.Parent.Name);
				else
					RazorEnhanced.HotKey.ClearKey(hotkeytreeView.SelectedNode, "General");
			}
			hotkeytextbox.Text = Keys.None.ToString();
		}

		private void hotkeytreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (hotkeytreeView.SelectedNode != null && hotkeytreeView.SelectedNode.Name != null)
			{
				Keys k = Keys.None;
				RazorEnhanced.Settings.HotKey.FindKeyGui(hotkeytreeView.SelectedNode.Name, out k, out bool passkey);
				hotkeytextbox.Text = HotKey.KeyString(k);
				hotkeypassCheckBox.Checked = passkey;
				hotkeytextbox.LastKey = Keys.None;
				HotKey.NormalKey = k;
			}
		}

		private void hotkeyMasterSetButton_Click(object sender, EventArgs e)
		{
			if (hotkeyKeyMasterTextBox.Text != String.Empty && hotkeyKeyMasterTextBox.Text != "None")
			{
				RazorEnhanced.HotKey.UpdateMaster();
				hotkeyKeyMasterTextBox.Text = String.Empty;
			}
		}

		private void hotkeyMasterClearButton_Click(object sender, EventArgs e)
		{
			RazorEnhanced.HotKey.ClearMasterKey();
		}

		private void hotkeyEnableButton_Click(object sender, EventArgs e)
		{
			Assistant.Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Enable";
			RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", true);
			if (World.Player != null)
				RazorEnhanced.Misc.SendMessage("HotKey: ENABLED", 168, false);
		}

		private void hotkeyDisableButton_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", false);
			Assistant.Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Disable";
			if (World.Player != null)
				RazorEnhanced.Misc.SendMessage("HotKey: DISABLED", 37, false);
		}

		private void HotKey_MouseRoll(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Delta > 0)
				RazorEnhanced.HotKey.KeyDown((Keys)502 | Control.ModifierKeys);
			else if (e.Delta < 0)
				RazorEnhanced.HotKey.KeyDown((Keys)501 | Control.ModifierKeys);
		}

		private void HotKey_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
				RazorEnhanced.HotKey.KeyDown((Keys)500 | Control.ModifierKeys);
			else if (e.Button == MouseButtons.XButton1)
				RazorEnhanced.HotKey.KeyDown((Keys)503 | Control.ModifierKeys);
			else if (e.Button == MouseButtons.XButton2)
				RazorEnhanced.HotKey.KeyDown((Keys)504 | Control.ModifierKeys);
		}

	}
}
