using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Assistant;

namespace RazorEnhanced.UI
{
	public partial class EnhancedTargetAdd : Form
	{
		private const string m_Title = "Enhanced Target Add";

        public EnhancedTargetAdd()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title; ;
		}


        private void EnhancedrgetAdd_Load(object sender, EventArgs e)
		{
            tAddBody.Text = "0x0000";
            tAddHue.Text = "0x0000";
            tRangeMax.Text = "-1";
            tRangeMin.Text = "-1";
            
            SelectorComboBox.Items.Add("Random");
            SelectorComboBox.Items.Add("Nearest");
            SelectorComboBox.Items.Add("Farthest");
            SelectorComboBox.Items.Add("Weakest");
            SelectorComboBox.Items.Add("Strongest");

            notocolorComboBox.Items.Add("Innocent");
            notocolorComboBox.Items.Add("Ally");
            notocolorComboBox.Items.Add("Can be attacked");
            notocolorComboBox.Items.Add("Criminal");
            notocolorComboBox.Items.Add("Enemy");
            notocolorComboBox.Items.Add("Murderer");
            notocolorComboBox.Items.Add("Invulnerable");
		}

        private void bodyaddButton_Click(object sender, EventArgs e)
        {
            int body =0;
            try
            {
                body = Convert.ToInt32(tAddBody.Text, 16);
                if (body > 0)
                {
                    if (!bodylistBox.Items.Contains("0x" + body.ToString("X4")))
                    {
                        bodylistBox.Items.Add("0x" + body.ToString("X4"));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Body ID is not valid.",
                "Body ID is not valid.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                tAddBody.Text = "0x0000";
            }
        }

        private void hueaddButton_Click(object sender, EventArgs e)
        {
            int hue = 0;
            try
            {
                hue = Convert.ToInt32(tAddHue.Text, 16);
                if (!huelistBox.Items.Contains("0x" + hue.ToString("X4")))
                {
                    huelistBox.Items.Add("0x" + hue.ToString("X4"));
                }
            }
            catch
            {
                MessageBox.Show("Hue number is not valid.",
                "Hue number is not valid.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                tAddHue.Text = "0x0000";
            }
        }

        private void notoaddButton_Click(object sender, EventArgs e)
        {
            if (notocolorComboBox.Text != "")
            {
                if (!notolistBox.Items.Contains(notocolorComboBox.Text))
                {
                    notolistBox.Items.Add(notocolorComboBox.Text);
                }
            }
        }

        private void bodyremoveButton_Click(object sender, EventArgs e)
        {
            if (bodylistBox.SelectedIndex >= 0)
                bodylistBox.Items.RemoveAt(bodylistBox.SelectedIndex);            
        }

        private void hueremoveButton_Click(object sender, EventArgs e)
        {
            if (huelistBox.SelectedIndex >= 0)
                huelistBox.Items.RemoveAt(huelistBox.SelectedIndex);     
        }

        private void notoremoveButton_Click(object sender, EventArgs e)
        {
            if (notolistBox.SelectedIndex >= 0)
                notolistBox.Items.RemoveAt(notolistBox.SelectedIndex);     
        }

	}
}
