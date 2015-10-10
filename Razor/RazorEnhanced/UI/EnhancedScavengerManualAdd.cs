using System;
using System.Windows.Forms;


namespace RazorEnhanced.UI
{
	public partial class EnhancedScavengerManualAdd : Form
	{
		private const string m_Title = "Enhanced Scavenger Manual Add Item";

		public EnhancedScavengerManualAdd()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;
		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void EnhancedScavengerManualAdd_Load(object sender, EventArgs e)
		{
			tName.Text = "New Item";
			tColor.Text = "0x0000";
			tGraphics.Text = "0x0000";
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void bAddItem_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int graphics = 0;
			int color = 0;
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

			if (tColor.Text == "-1")
				color = -1;
			else
			{
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
			}

			if (!fail)
			{
				RazorEnhanced.Scavenger.AddItemToList(tName.Text, graphics, color);
				this.Close();
			}

		}
	}
}
