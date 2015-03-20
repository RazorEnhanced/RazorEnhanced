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
using System.IO;
using Assistant;


namespace RazorEnhanced.UI
{
	public partial class EnhancedBuyAgentManualAdd : Form
	{
		private const string m_Title = "Enhanced Buy Manual Add Item";
        private ListView BuylistView;
        private List<RazorEnhanced.BuyAgent.BuyItem> BuyItemList;
        public EnhancedBuyAgentManualAdd(ListView PBuylistView, List<RazorEnhanced.BuyAgent.BuyItem> PBuyItemList)
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;
            BuylistView = PBuylistView;
            BuyItemList = PBuyItemList;
		}


        private void EnhancedBuyManualAdd_Load(object sender, EventArgs e)
        {
            tName.Text = "New Item";
            tAmount.Text = "999";
            tGraphics.Text = "0x0000";
            tHue.Text = "0x0000";
        }



        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bAddItem_Click(object sender, EventArgs e)
        {
            bool fail = false;
            int Graphics = 0;
            int Amount = 0;
            int Hue = 0;
            if (tName.Text == null)
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
                Graphics = Convert.ToInt32(tGraphics.Text, 16); 
            }
            catch
            {
                MessageBox.Show("Item Graphics is not valid.",
                "Item Graphics Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                fail = true;
            }

            try
            {

                Amount = Convert.ToInt32(tAmount.Text);
            }
            catch
            {
                MessageBox.Show("Item Amount is not valid.",
                "Item Amount Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                fail = true;
            }

            if (tHue.Text == "-1")
                Hue = -1;
            else
            {
                try
                {

                    Hue = Convert.ToInt32(tHue.Text, 16);
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
            }

            if (!fail)
            {
                RazorEnhanced.BuyAgent.AddItemToList(tName.Text, Graphics, Amount, Hue, BuylistView, BuyItemList);
                this.Close();
            }

        }
	}
}
