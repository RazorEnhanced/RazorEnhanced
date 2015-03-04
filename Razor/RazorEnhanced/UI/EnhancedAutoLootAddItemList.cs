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
        private RazorComboBox Autolootlist;

        public EnhancedAutolootAddItemList(RazorComboBox PAutolootlist)
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;
            Autolootlist = PAutolootlist;
            
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
            if (autolootListToAdd.Text == "")
                fail = true;

            if (!Regex.IsMatch(autolootListToAdd.Text, "^[a-zA-Z0-9_]+$"))
                fail = true;

            if (fail)
            {
                MessageBox.Show("List name can be only letter and number",
                "List name can be only letter and number",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                fail = true;
            }
            else
            {
                // TODO add list
            }
        }
	}
}
