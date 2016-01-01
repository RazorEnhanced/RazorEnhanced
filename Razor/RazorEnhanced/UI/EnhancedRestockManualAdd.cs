using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedRestockManualAdd : Form
	{
		private const string m_Title = "Enhanced Organizer Manual Add Item";

		public EnhancedRestockManualAdd()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;
		}

		private void EnhancedOrganizerManualAdd_Load(object sender, EventArgs e)
		{
			tName.Text = "New Item";
			tColor.Text = "0x0000";
			tGraphics.Text = "0x0000";
			tAmount.Text = "0";
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Restock.RefreshItems();
			this.Close();
		}

		private void bAddItem_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int graphics = 0;
			int color = 0;
			int amount = 0;

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

			try
			{
				color = Convert.ToInt32(tColor.Text, 16);
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

			if (!fail)
			{
				RazorEnhanced.Restock.AddItemToList(tName.Text, graphics, amount, color);
				this.Close();
			}
		}
	}
}