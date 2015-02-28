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

		internal EnhancedMobileInspector(Assistant.Mobile mobileTarg)
		{
			InitializeComponent();
			MaximizeBox = false;
			// general
			lName.Text = mobileTarg.Name.ToString();
			lSerial.Text = "0x" + mobileTarg.Serial.Value.ToString("X8");
			lMobileID.Text = "0x" + mobileTarg.Body.ToString("X4");
			lColor.Text = mobileTarg.Hue.ToString();
			lPosition.Text = mobileTarg.Position.ToString();
			// Details
			if (mobileTarg.Female)
				lSex.Text = "Female";
			else
				lSex.Text = "Male";
			lHits.Text = mobileTarg.Hits.ToString();
			lMaxHits.Text = mobileTarg.Hits.ToString();
			lNotoriety.Text = mobileTarg.Notoriety.ToString();
			lDirection.Text = mobileTarg.Direction.ToString();

			if (mobileTarg.Poisoned)
				lFlagPoisoned.Text = "Yes";
			else
				lFlagPoisoned.Text = "No";

			if (mobileTarg.Warmode)
				lFlagWar.Text = "Yes";
			else
				lFlagWar.Text = "No";

			if (mobileTarg.Visible)
				lFlagWar.Text = "No";
			else
				lFlagWar.Text = "Yes";

			if (mobileTarg.IsGhost)
				lFlagGhost.Text = "Yes";
			else
				lFlagGhost.Text = "No";

			if (mobileTarg.Blessed)
				lFlagBlessed.Text = "Yes";
			else
				lFlagBlessed.Text = "No";

			foreach (Assistant.ObjectPropertyList.OPLEntry ent in mobileTarg.ObjPropList.Content)
			{
				int number = ent.Number;
				string args = Assistant.Language.ParseSubCliloc(ent.Args);

				string content;
				if (args == null)
					content = Assistant.Language.GetCliloc(number);
				else
					content = Assistant.Language.ClilocFormat(ent.Number, args);

				listBoxAttributes.Items.Add(content);
			}
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
