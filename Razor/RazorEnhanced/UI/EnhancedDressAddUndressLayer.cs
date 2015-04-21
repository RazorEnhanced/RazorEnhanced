using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Assistant;

namespace RazorEnhanced.UI
{
    public partial class EnhancedDressAddUndressLayer : Form
	{
        private const string m_Title = "Enhanced Dress Add Clear Layer";

		public EnhancedDressAddUndressLayer()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title; ;
		}


		private void bClose_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Dress.RefreshItems();
			this.Close();
		}

		private void bAddItem_Click(object sender, EventArgs e)
		{
            if (layerlist.Text != "")
            {
                RazorEnhanced.Dress.DressItem toinsert = new RazorEnhanced.Dress.DressItem("UNDRESS", RazorEnhanced.Dress.LayerStringToInt(layerlist.Text), 0, true);
                RazorEnhanced.Settings.Dress.ItemInsertByLayer(Assistant.Engine.MainWindow.DressListSelect.Text, toinsert);
                RazorEnhanced.Dress.RefreshItems();
                this.Close();
            }
		}

        private void EnhancedDressAddUndressLayer_Load(object sender, EventArgs e)
        {
            layerlist.Items.Add("RightHand");
            layerlist.Items.Add("LeftHand");
            layerlist.Items.Add("Shoes");
            layerlist.Items.Add("Pants");
            layerlist.Items.Add("Shirt");
            layerlist.Items.Add("Head");
            layerlist.Items.Add("Gloves");
            layerlist.Items.Add("Ring");
            layerlist.Items.Add("Neck");
            layerlist.Items.Add("Waist");
            layerlist.Items.Add("InnerTorso");
            layerlist.Items.Add("Bracelet");
            layerlist.Items.Add("MiddleTorso");
            layerlist.Items.Add("Earrings");
            layerlist.Items.Add("Arms");
            layerlist.Items.Add("Cloak");
            layerlist.Items.Add("OuterTorso");
            layerlist.Items.Add("OuterLegs");
            layerlist.Items.Add("InnerLegs");
            layerlist.Items.Add("UpperRight");
        }
	}
}
