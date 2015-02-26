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
            Assistant.ObjectPropertyList ItemTargOPL = ItemTarg.ObjPropList;
			// general
			lSerial.Text = "0x" + ItemTarg.Serial.Value.ToString("X8");
			lItemID.Text = "0x" + ItemTarg.ItemID.Value.ToString("X4");
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
		
			for (int i = 0; i < ItemTargOPL.Content.Count; i++) // Skip sul nome :)
			{
				Assistant.ObjectPropertyList.OPLEntry ent = (Assistant.ObjectPropertyList.OPLEntry)ItemTargOPL.Content[i];
				int number = ent.Number;
				string args = ent.Args;
				string content;
				if (args == null)
					content = Assistant.Language.GetCliloc(number);
				else
					content = Assistant.Language.ClilocFormat(ent.Number, ent.Args);
                if (i == 0)
                {
                    if (content.IndexOf("#") != -1)
                        lName.Text =SubClilocSearch(content);
                    else
                        lName.Text = content;
                }
                else
                {
                    if (content.IndexOf("#") != -1)
                        listBox1.Items.Add(SubClilocSearch(content));
                    else
                        listBox1.Items.Add(content);
                }
			}
		}
        private string SubClilocSearch(string Text)
        {
            int CutPoint = Text.IndexOf("#");
            string Number = "";
            string Merged = "";
            string CutPart1 = "";
            string CutPart2 = "";
            for (int i = 0; i <= Text.Length - 1; i++)
            {
                if (i < CutPoint)
                    CutPart1 = CutPart1 + Text[i];
                else if (i >= CutPoint + 8)
                    CutPart2 = CutPart2 + Text[i];
                else if (i > CutPoint && i < CutPoint + 8)
                    Number = Number + Text[i];
            }
            try
            {
                Merged = CutPart1 + Assistant.Language.GetCliloc(Convert.ToInt32(Number)) + CutPart2;
            }
            catch (FormatException e)
            {
            }

            return Merged;
            
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
