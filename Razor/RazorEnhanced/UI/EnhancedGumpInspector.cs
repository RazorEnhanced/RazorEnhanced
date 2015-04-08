using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;


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
            EnhancedGumpInspectorListBox = new System.Windows.Forms.ListBox();
            EnhancedGumpInspectorListBox.FormattingEnabled = true;
            EnhancedGumpInspectorListBox.Location = new System.Drawing.Point(6, 19);
            EnhancedGumpInspectorListBox.Name = "listBox1";
            EnhancedGumpInspectorListBox.Size = new System.Drawing.Size(484, 342);
            EnhancedGumpInspectorListBox.TabIndex = 3;
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
            string selected = EnhancedGumpInspectorListBox.SelectedItem.ToString();
            Clipboard.SetText(selected.Substring(selected.IndexOf(':') + 1));
        }


	}

}
