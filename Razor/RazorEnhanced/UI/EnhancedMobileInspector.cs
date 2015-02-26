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
	internal partial class EnhancedMobileInspector : Form
	{
		private const string m_Title = "Enhanced Mobile Inspect";

		internal EnhancedMobileInspector(Assistant.Mobile MobileTarg)
		{
			InitializeComponent();
			// general
            Assistant.ObjectPropertyList MobileTargOPL = MobileTarg.ObjPropList;
			lName.Text = MobileTarg.Name.ToString();
			lSerial.Text = "0x" + MobileTarg.Serial.Value.ToString("X8");
			lMobileID.Text = "0x" + MobileTarg.Body.ToString("X4");
			lColor.Text = MobileTarg.Hue.ToString();
			lPosition.Text = MobileTarg.Position.ToString();
			// Details
			if (MobileTarg.Female)
				lSex.Text = "Female";
			else
				lSex.Text = "Male";
			lHits.Text = MobileTarg.Hits.ToString();
			lMaxHits.Text = MobileTarg.Hits.ToString();
			lNotoriety.Text = MobileTarg.Notoriety.ToString();
			lDirection.Text = MobileTarg.Direction.ToString();

			if (MobileTarg.Poisoned)
				lFlagPoisoned.Text = "Yes";
			else
				lFlagPoisoned.Text = "No";

			if (MobileTarg.Warmode)
				lFlagWar.Text = "Yes";
			else
				lFlagWar.Text = "No";

			if (MobileTarg.Visible)
				lFlagWar.Text = "No";
			else
				lFlagWar.Text = "Yes";

			if (MobileTarg.IsGhost)
				lFlagGhost.Text = "Yes";
			else
				lFlagGhost.Text = "No";

			if (MobileTarg.Blessed)
				lFlagBlessed.Text = "Yes";
			else
				lFlagBlessed.Text = "No";


            for (int i = 0; i < MobileTargOPL.Content.Count; i++) 
            {
                Assistant.ObjectPropertyList.OPLEntry ent = (Assistant.ObjectPropertyList.OPLEntry)MobileTargOPL.Content[i];
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
                        lName.Text = SubClilocSearch(content);
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
            catch
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
			Clipboard.SetText(lMobileID.Text);
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
			Clipboard.SetText(lSex.Text);
		}

		private void bRContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lHits.Text);
		}

		private void bAmountCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMaxHits.Text);
		}

		private void bLayerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lNotoriety.Text);
		}

		private void bOwnedCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lDirection.Text);
		}
	}
}
