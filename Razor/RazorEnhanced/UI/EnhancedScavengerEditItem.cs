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
    public partial class EnhancedScavengerEditItem : Form
	{
		private const string m_Title = "Enhanced Scavenger Edit Item";
        private ListView ScavengerListView;
        private List<RazorEnhanced.Scavenger.ScavengerItem> ScavengerItemList;
        private int IndexEdit;
        public EnhancedScavengerEditItem(ListView PScavengerListView, List<RazorEnhanced.Scavenger.ScavengerItem> PScavengerItemList, int PIndexEdit)
		{
			InitializeComponent();
            MaximizeBox = false;
			this.Text = m_Title;
            ScavengerListView = PScavengerListView;
            ScavengerItemList = PScavengerItemList;
            IndexEdit = PIndexEdit;
		}

        private void EnhancedScavengerManualAdd_Load(object sender, EventArgs e)
        {
            tName.Text = ScavengerItemList[IndexEdit].Name;
            tGraphics.Text = "0x" + ScavengerItemList[IndexEdit].Graphics.ToString("X4");
            if (ScavengerItemList[IndexEdit].Color == -1)
                tColor.Text = "-1";
            else
                tColor.Text = "0x" + ScavengerItemList[IndexEdit].Color.ToString("X4");
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
                RazorEnhanced.Scavenger.ModifyItemToList(tName.Text, Graphics, Color, ScavengerListView, ScavengerItemList, IndexEdit);
                RazorEnhanced.Settings.SaveScavengerItemList(Assistant.Engine.MainWindow.ScavengerListSelect.SelectedItem.ToString(), ScavengerItemList);
                this.Close();
            }

        }
	}
}
