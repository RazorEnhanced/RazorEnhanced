using System;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedItemInspector : Form
	{
		private const string m_Title = "Enhanced Item Inspect";
		private Thread m_ProcessInfo;
		private Assistant.Item m_itemTarg;

		internal EnhancedItemInspector(Assistant.Item itemTarg)
		{
			InitializeComponent();
			MaximizeBox = false;
			m_itemTarg = itemTarg;
			// general
			lSerial.Text = "0x" + itemTarg.Serial.Value.ToString("X8");
			lItemID.Text = "0x" + itemTarg.ItemID.Value.ToString("X4");
			lColor.Text = "0x" + itemTarg.Hue.ToString("X4");
			lPosition.Text = itemTarg.Position.ToString();
			// Details
			Assistant.PlayerData tempdata;
			Assistant.Item tempdata2;
			if (itemTarg.Container is Assistant.PlayerData)
			{
				tempdata = (Assistant.PlayerData)itemTarg.Container;
				lContainer.Text = tempdata.Serial.ToString();
			}
			if (itemTarg.Container is Assistant.Item)
			{
				tempdata2 = (Assistant.Item)itemTarg.Container;
				lContainer.Text = tempdata2.Serial.ToString();
			}

			if (itemTarg.RootContainer is Assistant.PlayerData)
			{
				tempdata = (Assistant.PlayerData)itemTarg.RootContainer;
				lRootContainer.Text = tempdata.Serial.ToString();
				if (tempdata.Serial == Assistant.World.Player.Serial)
					lOwned.Text = "Yes";
			}
			if (itemTarg.RootContainer is Assistant.Item)
			{
				tempdata2 = (Assistant.Item)itemTarg.RootContainer;
				lRootContainer.Text = tempdata2.Serial.ToString();
				if (tempdata2.Serial == Assistant.World.Player.Backpack.Serial)
					lOwned.Text = "Yes";
			}

			lAmount.Text = itemTarg.Amount.ToString();
			lLayer.Text = itemTarg.Layer.ToString();

			// Attributes
			m_ProcessInfo = new Thread(ProcessInfoThread);
			m_ProcessInfo.Start();
		}

		private void ProcessInfoThread()
		{
			if (m_itemTarg != null)
			{
				Items.WaitForProps(m_itemTarg.Serial, 1000);

				if (m_itemTarg.ObjPropList.Content.Count > 0)
				{
					for (int i = 0; i < m_itemTarg.ObjPropList.Content.Count; i++)
					{
						Assistant.ObjectPropertyList.OPLEntry ent = m_itemTarg.ObjPropList.Content[i];
						if (i == 0)
							if (ent.ToString() == null)
								lName.Invoke(new Action(() => lName.Text = m_itemTarg.Name.ToString()));
							else
								lName.Invoke(new Action(() => lName.Text = ent.ToString()));
						string content = ent.ToString();
						listBoxAttributes.Invoke(new Action(() => listBoxAttributes.Items.Add(content)));
					}
				}
				else
				{
					lName.Invoke(new Action(() => lName.Text = m_itemTarg.Name.ToString()));
					listBoxAttributes.Invoke(new Action(() => listBoxAttributes.Items.Add("No Props Readed!")));
				}
			}
		}
        private void razorButton1_Click(object sender, EventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }

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

		private void EnhancedItemInspector_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }
		}
	}
}