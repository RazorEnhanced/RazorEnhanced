using System;
using System.Windows.Forms;

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

			switch (mobileTarg.Direction & Assistant.Direction.Mask)
			{
				case Assistant.Direction.North: lDirection.Text = "North"; break;
				case Assistant.Direction.South: lDirection.Text = "South"; break;
				case Assistant.Direction.West: lDirection.Text = "West"; break;
				case Assistant.Direction.East: lDirection.Text = "East"; break;
				case Assistant.Direction.Right: lDirection.Text = "Right"; break;
				case Assistant.Direction.Left: lDirection.Text = "Left"; break;
				case Assistant.Direction.Down: lDirection.Text = "Down"; break;
				case Assistant.Direction.Up: lDirection.Text = "Up"; break;
				default: lDirection.Text = "Undefined"; break;
			}

			if (mobileTarg.Poisoned)
				lFlagPoisoned.Text = "Yes";
			else
				lFlagPoisoned.Text = "No";

			if (mobileTarg.Warmode)
				lFlagWar.Text = "Yes";
			else
				lFlagWar.Text = "No";

			if (mobileTarg.Visible)
				lFlagHidden.Text = "No";
			else
				lFlagHidden.Text = "Yes";

			if (mobileTarg.IsGhost)
				lFlagGhost.Text = "Yes";
			else
				lFlagGhost.Text = "No";

			if (mobileTarg.Blessed)		// Yellow Hits
				lFlagBlessed.Text = "Yes";
			else
				lFlagBlessed.Text = "No";

			if (mobileTarg.Paralized)     
				lFlagParalized.Text = "Yes";
			else
				lFlagParalized.Text = "No";

			for (int i = 0; i < mobileTarg.ObjPropList.Content.Count; i++)
			{
				Assistant.ObjectPropertyList.OPLEntry ent = mobileTarg.ObjPropList.Content[i];
				if (i == 0)
					lName.Text = ent.ToString();
				string content = ent.ToString();
				listBoxAttributes.Items.Add(content);
			}

			if (mobileTarg.ObjPropList.Content.Count == 0)
			{
				lName.Text = mobileTarg.Name.ToString();
				listBoxAttributes.Items.Add("No Props Readed!");
			}
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			this.Close();
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