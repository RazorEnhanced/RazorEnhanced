using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedProfileImport : Form
	{
		private const string m_Title = "Enhanced Profile Import";

		public EnhancedProfileImport()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
		}

		private void close_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void chosefileButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Profiles",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				profilefilepathTextBox.Text = od.FileName;
			}
		}
	}
}
