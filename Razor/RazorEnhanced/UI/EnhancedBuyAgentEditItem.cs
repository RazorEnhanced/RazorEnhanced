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
	public partial class EnhancedBuyAgentEditItem : Form
	{
		private const string m_Title = "Enhanced Buy Edit Item";
        private ListView BuylistView;
        private List<RazorEnhanced.BuyAgent.BuyItem> BuyItemList;
        private int IndexEdit;
        public EnhancedBuyAgentEditItem(ListView PBuylistView, List<RazorEnhanced.BuyAgent.BuyItem> PBuyItemList, int PIndexEdit)
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;
            BuylistView = PBuylistView;
            BuyItemList = PBuyItemList;
            IndexEdit = PIndexEdit;
		}


        private void EnhancedSellManualAdd_Load(object sender, EventArgs e)
        {
            tName.Text = BuyItemList[IndexEdit].Name;
            tGraphics.Text = "0x" + BuyItemList[IndexEdit].Graphics.ToString("X4");
            tAmount.Text = BuyItemList[IndexEdit].Amount.ToString();
        }



        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bAddItem_Click(object sender, EventArgs e)
        {
            bool fail = false;
            int Graphics = 0 ;
            int Amount =0 ;
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
            
            if (!fail)
            {
                RazorEnhanced.BuyAgent.ModifyItemToList(tName.Text, Graphics, Amount, BuylistView, BuyItemList, IndexEdit);
                RazorEnhanced.Settings.SaveBuyItemList(Assistant.Engine.MainWindow.BuyListSelect.SelectedItem.ToString(), BuyItemList);
                this.Close();
            }

        }
	}
}
