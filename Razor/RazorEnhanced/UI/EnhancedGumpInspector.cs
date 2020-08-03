using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedGumpInspector : Form
	{
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private RazorButton close;
		private RazorButton clear;
		private const string m_Title = "Enhanced Gump Inspector";
		public static System.Windows.Forms.ListBox EnhancedGumpInspectorListBox;

		public EnhancedGumpInspector()
		{
			InitializeComponent();
			MaximizeBox = false;
			this.Text = m_Title;
			EnhancedGumpInspectorListBox = new ListBox
			{
				FormattingEnabled = true,
				Location = new System.Drawing.Point(6, 19),
				Name = "listBox1",
				Size = new System.Drawing.Size(484, 342),
				TabIndex = 3
			};
			this.groupBox1.Controls.Add(EnhancedGumpInspectorListBox);
		}

		private void closeGumpInspector_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void EnhancedGumpInspector_Load(object sender, EventArgs e)
		{
			Assistant.Engine.MainWindow.GumpInspectorEnable = true;
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			EnhancedGumpInspectorListBox.Items.Clear();
		}

		private void razorButton1_Click_1(object sender, EventArgs e)
		{
			if (EnhancedGumpInspectorListBox.SelectedItem != null)
			{
				string selected = EnhancedGumpInspectorListBox.SelectedItem.ToString();
				Assistant.Utility.ClipBoardCopy(selected.Substring(selected.IndexOf(':') + 1));
			}
		}
	}
}