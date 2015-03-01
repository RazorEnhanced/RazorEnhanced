using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using PaxScript.Net;
using System.IO;
using Assistant;


namespace RazorEnhanced.UI
{
	public partial class EnhancedAutolootManualAdd : Form
	{
		private const string m_Title = "Enhanced Autoloot Manual Add Item";

        public EnhancedAutolootManualAdd()
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;


		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void EnhancedAutolootManualAdd_Load(object sender, EventArgs e)
        {
            tName.Text = "New Item";
            tColor.Text = "0x0000";
            tGraphycs.Text = "0x0000";
        }



        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bAddItem_Click(object sender, EventArgs e)
        {
            bool fail = false;
            int Graphycs;
            int Color;
            if (tName != null)
            {
                MessageBox.Show("Item name is not valid.",
                "Item name Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                fail = true;
            }

            try
            {
                Graphycs = Convert.ToInt32(tGraphycs); 
            }
            catch
            {
                MessageBox.Show("Item Graphycs is not valid.",
                "Item Graphycs Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                fail = true;
            }

            try
            {
                Color = Convert.ToInt32(tColor);
            }
            catch
            {
                MessageBox.Show("Item Color is not valid.",
                "Item Color Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                fail = true;
            }

            if (!fail)
            {
                // TODO add to list
                this.Close();
            }

        }
	}
}
