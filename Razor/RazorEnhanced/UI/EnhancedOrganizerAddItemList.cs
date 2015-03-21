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
	public partial class EnhancedOrganizerAddItemList : Form
	{
        private const string m_Title = "Enhanced Organizer Add Item List";
        

        public EnhancedOrganizerAddItemList()
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;           
		}

        private void EnhancedAutolootAddItemList_Load(object sender, EventArgs e)
        {

        }

        private void organizercloseItemList_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void organizeraddItemList_Click(object sender, EventArgs e)
        {
            bool fail = false;
            string nuovaOrganizerList = "";

            if (organizerListToAdd.Text == "")
                fail = true;

            if (!Regex.IsMatch(organizerListToAdd.Text, "^[a-zA-Z0-9_]+$"))
                fail = true;

            nuovaOrganizerList = organizerListToAdd.Text.ToLower();
            for (int i = 0; i < Assistant.Engine.MainWindow.OrganizerListSelect.Items.Count; i++)
            {
                if (nuovaOrganizerList == Assistant.Engine.MainWindow.OrganizerListSelect.GetItemText(Assistant.Engine.MainWindow.OrganizerListSelect.Items[i]))
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
                Assistant.Engine.MainWindow.OrganizerListSelect.Items.Add(nuovaOrganizerList);
                Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = Assistant.Engine.MainWindow.OrganizerListSelect.Items.IndexOf(nuovaOrganizerList);

                List<string> OrganizerSettingItemList = new List<string>();

                for (int i = 0; i < Assistant.Engine.MainWindow.OrganizerListSelect.Items.Count; i++)
                {
                    if (Assistant.Engine.MainWindow.OrganizerListSelect.Items[i].ToString() != "Default")
                        OrganizerSettingItemList.Add(Assistant.Engine.MainWindow.OrganizerListSelect.Items[i].ToString());
                }
                RazorEnhanced.Settings.SaveOrganizerGeneral(Assistant.Engine.MainWindow.OrganizerDragDelay, OrganizerSettingItemList, nuovaOrganizerList);
                this.Close();
            }
        }
	}
}
