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
	public partial class EnhancedScavengerAddItemList : Form
	{
        private const string m_Title = "Enhanced Scavenger Add Item List";


        public EnhancedScavengerAddItemList()
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;           
		}

        private void EnhancedAutolootAddItemList_Load(object sender, EventArgs e)
        {

        }

        private void scavegercloseItemList_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void scavegeraddItemList_Click(object sender, EventArgs e)
        {
            bool fail = false;
            string NuovaScavengerList = "";

            if (scavengerListToAdd.Text == "")
                fail = true;

            if (!Regex.IsMatch(scavengerListToAdd.Text, "^[a-zA-Z0-9_]+$"))
                fail = true;

            NuovaScavengerList = scavengerListToAdd.Text.ToLower();
            for (int i = 0; i < Assistant.Engine.MainWindow.ScavengerListSelect.Items.Count; i++)
            {
                if (NuovaScavengerList == Assistant.Engine.MainWindow.ScavengerListSelect.GetItemText(Assistant.Engine.MainWindow.ScavengerListSelect.Items[i]))
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
                Assistant.Engine.MainWindow.ScavengerListSelect.Items.Add(NuovaScavengerList);
                Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = Assistant.Engine.MainWindow.ScavengerListSelect.Items.IndexOf(NuovaScavengerList);

                List<string> ScavengerSettingItemList = new List<string>();

                for (int i = 0; i < Assistant.Engine.MainWindow.ScavengerListSelect.Items.Count; i++)
                {
                    if (Assistant.Engine.MainWindow.ScavengerListSelect.Items[i].ToString() != "Default")
                        ScavengerSettingItemList.Add(Assistant.Engine.MainWindow.ScavengerListSelect.Items[i].ToString());
                }
                RazorEnhanced.Settings.SaveScavengerGeneral(Assistant.Engine.MainWindow.ScavengerDragDelay.ToString(), ScavengerSettingItemList, NuovaScavengerList);
                this.Close();
            }
        }
	}
}
