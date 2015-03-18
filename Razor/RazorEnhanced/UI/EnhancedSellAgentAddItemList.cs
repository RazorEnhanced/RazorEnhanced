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
	public partial class EnhancedSellAgentAddItemList : Form
	{
        private const string m_Title = "Enhanced Sell Add Item List";


        public EnhancedSellAgentAddItemList()
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;           
		}


        private void sellcloseItemList_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selladdItemList_Click(object sender, EventArgs e)
        {
            bool fail = false;
            string NuovaLootList = "";

            if (sellagentListToAdd.Text == "")
                fail = true;

            if (!Regex.IsMatch(sellagentListToAdd.Text, "^[a-zA-Z0-9_]+$"))
                fail = true;

            NuovaLootList = sellagentListToAdd.Text.ToLower();
            for (int i = 0; i < Assistant.Engine.MainWindow.SellListSelect.Items.Count; i++)
            {
                if (NuovaLootList == Assistant.Engine.MainWindow.SellListSelect.GetItemText(Assistant.Engine.MainWindow.SellListSelect.Items[i]))
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
                Assistant.Engine.MainWindow.SellListSelect.Items.Add(NuovaLootList);
                Assistant.Engine.MainWindow.SellListSelect.SelectedIndex = Assistant.Engine.MainWindow.SellListSelect.Items.IndexOf(NuovaLootList);

                List<string> SellSettingItemList = new List<string>();

                for (int i = 0; i < Assistant.Engine.MainWindow.SellListSelect.Items.Count; i++)
                {
                    if (Assistant.Engine.MainWindow.SellListSelect.Items[i].ToString() != "Default")
                        SellSettingItemList.Add(Assistant.Engine.MainWindow.SellListSelect.Items[i].ToString());
                }
                RazorEnhanced.Settings.SaveSellGeneral(SellSettingItemList, NuovaLootList);
                this.Close();
            }
        }
	}
}
