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
	public partial class EnhancedAutolootEditItem : Form
	{
		private const string m_Title = "Enhanced Autoloot Edit Item";
        private ListView AutolootlistView;
        private List<RazorEnhanced.AutoLoot.AutoLootItem> AutoLootItemList;
        private int IndexEdit;
        public EnhancedAutolootEditItem(ListView PAutolootlistView, List<RazorEnhanced.AutoLoot.AutoLootItem> PAutoLootItemList, int PIndexEdit)
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;
            AutolootlistView = PAutolootlistView;
            AutoLootItemList = PAutoLootItemList;
            IndexEdit = PIndexEdit;
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void EnhancedAutolootManualAdd_Load(object sender, EventArgs e)
        {
            tName.Text=AutoLootItemList[IndexEdit].Name;
            tGraphics.Text = "0x" + AutoLootItemList[IndexEdit].Graphics.ToString("X4");
            tColor.Text = "0x" + AutoLootItemList[IndexEdit].Color.ToString("X4");
        }



        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bAddItem_Click(object sender, EventArgs e)
        {
            bool fail = false;
            int Graphics = 0 ;
            int Color =0 ;
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

            if (tColor.Text == "-1")
                Color = -1;
            else
            {
                try
                {

                    Color = Convert.ToInt32(tColor.Text, 16);
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
                AutoLootItemList.RemoveAt(IndexEdit);
                RazorEnhanced.AutoLoot.InsertItemToList(tName.Text, Graphics, Color, AutolootlistView, AutoLootItemList, IndexEdit);
                RazorEnhanced.Settings.SaveAutoLootItemList(AutoLootItemList);
                this.Close();

            }

        }
	}
}
