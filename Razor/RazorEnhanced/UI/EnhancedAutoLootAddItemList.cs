using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;


namespace RazorEnhanced.UI
{
	public partial class EnhancedAutolootAddItemList : Form
	{
		private const string m_Title = "Enhanced Autoloot Add Item List";


		public EnhancedAutolootAddItemList()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void EnhancedAutolootAddItemList_Load(object sender, EventArgs e)
		{

		}

		private void autolootcloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void autolootaddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string nuovaLootList = "";

			if (autolootListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(autolootListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			nuovaLootList = autolootListToAdd.Text.ToLower();
			for (int i = 0; i < Assistant.Engine.MainWindow.AutolootListSelect.Items.Count; i++)
			{
				if (nuovaLootList == Assistant.Engine.MainWindow.AutolootListSelect.GetItemText(Assistant.Engine.MainWindow.AutolootListSelect.Items[i]))
					fail = true;
			}

			if (fail)
			{
				MessageBox.Show("Invalid list name!",
				"Invalid list name!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}
			else
			{
				Assistant.Engine.MainWindow.AutolootListSelect.Items.Add(nuovaLootList);
				Assistant.Engine.MainWindow.AutolootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutolootListSelect.Items.IndexOf(nuovaLootList);

				List<string> lootSettingItemList = new List<string>();

				for (int i = 0; i < Assistant.Engine.MainWindow.AutolootListSelect.Items.Count; i++)
				{
					if (Assistant.Engine.MainWindow.AutolootListSelect.Items[i].ToString() != "Default")
						lootSettingItemList.Add(Assistant.Engine.MainWindow.AutolootListSelect.Items[i].ToString());
				}
				RazorEnhanced.Settings.SaveAutoLootGeneral(Assistant.Engine.MainWindow.AutoLootDelay, lootSettingItemList, nuovaLootList, Assistant.Engine.MainWindow.AutoLootBag);
				this.Close();
			}
		}
	}
}
