using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.IO;
using Assistant;


namespace RazorEnhanced.UI
{
	public partial class EnhancedGraphFilterAdd : Form
	{
		private const string m_Title = "Enhanced Graph Filter";

        public EnhancedGraphFilterAdd()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;
		}

		private void EnhancedOrganizerManualAdd_Load(object sender, EventArgs e)
		{

			tGraphicsNew.Text = "0x0000";
			tGraphicsReal.Text = "0x0000";
		}



		private void bClose_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Filters.RefreshLists();
			this.Close();
		}


		private void bAddItem_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int graphicsreal = 0;
            int graphicsnew = 0;
			
			try
			{
                graphicsreal = Convert.ToInt32(tGraphicsReal.Text, 16);
			}
			catch
			{
				fail = true;
			}

			try
			{
                graphicsnew = Convert.ToInt32(tGraphicsNew.Text, 16);
			}
			catch
			{
				fail = true;
			}

	        if (RazorEnhanced.Settings.GraphFilter.Exist(graphicsreal))
                fail = true;

			if (!fail)
			{
                RazorEnhanced.Settings.GraphFilter.Insert(graphicsreal, graphicsnew);
			}
            else
            {
                MessageBox.Show("Graphics number is not valid or already filtered",
                "Graphics number error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }
            RazorEnhanced.Filters.RefreshLists();
            this.Close();

		}


	}
}
