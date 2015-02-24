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

        public EnhancedItemInspector(Assistant.Item ItemTarg)
		{
			InitializeComponent();
            // general
            lName.Text = ItemTarg.Name;
            lSerial.Text = ItemTarg.Serial.ToString();
            lItemID.Text = ItemTarg.ItemID.ToString();
            lColor.Text = ItemTarg.Hue.ToString();
            lPosition.Text = ItemTarg.Position.ToString();
            // Details
            lContainer.Text = ItemTarg.Container.ToString();
            lRootContainer.Text = ItemTarg.RootContainer.ToString();
            lAmount.Text = ItemTarg.Amount.ToString();
            lLayer.Text = ItemTarg.Layer.ToString();
            lOwned.Text = "Utilita?";
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
