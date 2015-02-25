using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RazorEnhanced.UI
{
	public partial class EnhancedItemInspector : Form
	{
		private const string m_Title = "Enhanced Item Inspect";

        internal EnhancedItemInspector(Assistant.Item ItemTarg)
		{
			InitializeComponent();
            // general
            lName.Text = ItemTarg.Name.ToString(); 
            lSerial.Text = "0x"+ItemTarg.Serial.Value.ToString("X8");
            lItemID.Text = "0x"+ItemTarg.ItemID.Value.ToString("X4");
            lColor.Text = ItemTarg.Hue.ToString();
            lPosition.Text = ItemTarg.Position.ToString();
            // Details
            Assistant.PlayerData tempdata;
            Assistant.Item tempdata2;
            if (ItemTarg.Container is Assistant.PlayerData)
            {
                tempdata = (Assistant.PlayerData)ItemTarg.Container;
                lContainer.Text = tempdata.Serial.ToString();
            }
            if (ItemTarg.Container is Assistant.Item)
            {
                tempdata2 = (Assistant.Item)ItemTarg.Container;
                lContainer.Text = tempdata2.Serial.ToString();
            }

            if (ItemTarg.RootContainer is Assistant.PlayerData)
            {
                tempdata = (Assistant.PlayerData)ItemTarg.RootContainer;
                lRootContainer.Text = tempdata.Serial.ToString();
                if (tempdata.Serial == Assistant.World.Player.Serial)
                    lOwned.Text = "Yes";    
            }
            if (ItemTarg.RootContainer is Assistant.Item)
            {
                tempdata2 = (Assistant.Item)ItemTarg.RootContainer;
                lRootContainer.Text = tempdata2.Serial.ToString();
                if (tempdata2.Serial == Assistant.World.Player.Backpack.Serial)
                    lOwned.Text = "Yes";
            }

            lAmount.Text = ItemTarg.Amount.ToString();
            lLayer.Text = ItemTarg.Layer.ToString();
           

            
            // Attributes
            // TODO
		}

        private void razorButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lName_Click(object sender, EventArgs e)
        {

        }

        private void EnhancedItemInspect_Load(object sender, EventArgs e)
        {
            
        }

        private void bNameCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lName.Text);
        }

        private void bSerialCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lSerial.Text);
        }

        private void bItemIdCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lItemID.Text);
        }

        private void bColorCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lColor.Text);
        }

        private void bPositionCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lPosition.Text);
        }

        private void bContainerCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lContainer.Text);
        }

        private void bRContainerCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lRootContainer.Text);
        }

        private void bAmountCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lAmount.Text);
        }

        private void bLayerCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lLayer.Text);
        }

        private void bOwnedCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lOwned.Text);
        }
	}
}
