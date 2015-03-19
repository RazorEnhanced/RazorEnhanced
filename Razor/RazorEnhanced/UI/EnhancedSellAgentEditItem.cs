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
	public partial class EnhancedSellAgentEditItem : Form
	{
		private const string m_Title = "Enhanced Sell Edit Item";
        private ListView SelllistView;
        private List<RazorEnhanced.SellAgent.SellItem> SellItemList;
        private int IndexEdit;
        public EnhancedSellAgentEditItem(ListView PSelllistView, List<RazorEnhanced.SellAgent.SellItem> PSellItemList, int PIndexEdit)
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;
            SelllistView = PSelllistView;
            SellItemList = PSellItemList;
            IndexEdit = PIndexEdit;
		}


        private void EnhancedSellManualAdd_Load(object sender, EventArgs e)
        {
            tName.Text = SellItemList[IndexEdit].Name;
            tGraphics.Text = "0x" + SellItemList[IndexEdit].Graphics.ToString("X4");
            tAmount.Text = SellItemList[IndexEdit].Amount.ToString();
            tHue.Text = SellItemList[IndexEdit].Color.ToString();
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
                RazorEnhanced.SellAgent.ModifyItemToList(tName.Text, Graphics, Amount, Hue, SelllistView, SellItemList, IndexEdit);
                RazorEnhanced.Settings.SaveSellItemList(Assistant.Engine.MainWindow.SellListSelect.SelectedItem.ToString(), SellItemList, Assistant.Engine.MainWindow.SellBagLabel.Text);
                this.Close();
            }

        }
	}
}
