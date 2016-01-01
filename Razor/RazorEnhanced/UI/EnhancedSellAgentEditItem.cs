using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedSellAgentEditItem : Form
	{
		private const string m_Title = "Enhanced Sell Edit Item";

		private string m_List;
		private SellAgent.SellAgentItem m_Item;
		private int m_Index;

		public EnhancedSellAgentEditItem(string list, int index, SellAgent.SellAgentItem item)
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;

			m_List = list;
			m_Index = index;
			m_Item = item;
		}

		private void EnhancedSellManualAdd_Load(object sender, EventArgs e)
		{
			tName.Text = m_Item.Name;
			tGraphics.Text = "0x" + m_Item.Graphics.ToString("X4");
			tAmount.Text = m_Item.Amount.ToString();
			tHue.Text = m_Item.Color.ToString();
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void bAddItem_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int graphics = 0;
			int amount = 0;
			int hue = 0;
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
				graphics = Convert.ToInt32(tGraphics.Text, 16);
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

			try
			{
				amount = Convert.ToInt32(tAmount.Text);
			}
			catch
			{
				MessageBox.Show("Item Amount is not valid.",
				"Item Amount Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			if (tHue.Text == "-1")
				hue = -1;
			else
			{
				try
				{
					hue = Convert.ToInt32(tHue.Text, 16);
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
				RazorEnhanced.SellAgent.ModifyItemInList(tName.Text, graphics, amount, hue, m_Item.Selected, m_Item, m_Index);
				RazorEnhanced.SellAgent.RefreshItems();
				this.Close();
			}
		}
	}
}