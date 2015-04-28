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
	public partial class EnhancedRestockAddList : Form
	{
        private const string m_Title = "Enhanced Restock Add Item List";

        public EnhancedRestockAddList()
		{
			InitializeComponent();
            MaximizeBox = false;

			this.Text = m_Title;           
		}

        private void restockcloseItemList_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void restockaddItemList_Click(object sender, EventArgs e)
        {
            bool fail = false;
            string newList = "";

            if (restockListToAdd.Text == "")
                fail = true;

            if (!Regex.IsMatch(restockListToAdd.Text, "^[a-zA-Z0-9_]+$"))
                fail = true;

			newList = restockListToAdd.Text.ToLower();
			if (RazorEnhanced.Settings.Restock.ListExists(newList))
				fail = true;

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
                Restock.AddList(newList);
				this.Close();
            }
        }

	}
}
