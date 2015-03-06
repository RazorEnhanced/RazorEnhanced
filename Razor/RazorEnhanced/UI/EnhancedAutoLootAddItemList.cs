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
            string NuovaLootList = "";

            if (autolootListToAdd.Text == "")
                fail = true;

            if (!Regex.IsMatch(autolootListToAdd.Text, "^[a-zA-Z0-9_]+$"))
                fail = true;

            NuovaLootList = autolootListToAdd.Text.ToLower();
            for (int i = 0; i < Assistant.Engine.MainWindow.AutolootListSelect.Items.Count; i++)
            {
                if (NuovaLootList == Assistant.Engine.MainWindow.AutolootListSelect.GetItemText(Assistant.Engine.MainWindow.AutolootListSelect.Items[i]))
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
                Assistant.Engine.MainWindow.AutolootListSelect.Items.Add(NuovaLootList);
                Assistant.Engine.MainWindow.AutolootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutolootListSelect.Items.IndexOf(NuovaLootList);

                // TODO procedure di save
                this.Close();
            }
        }
	}
}
