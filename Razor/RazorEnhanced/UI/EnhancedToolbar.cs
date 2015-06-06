using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedToolbar : Form
	{
        // Hits
        internal string LabelTextHits
        {
            set { labelTextHits.Text = value; }
        }
        internal System.Drawing.Size LabelBarHitsSize
        {
            set { labelBarHits.Size = value; }
        }
        internal System.Drawing.Color LabelBarHitsColor
        {
            set { labelBarHits.BackColor = value; }
        }

        // Stam
        internal string LabelTextStam
        {
            set { labelTextStamina.Text = value; }
        }
        internal System.Drawing.Size LabelBarStamSize
        {
            set { labelBarStamina.Size = value; }
        }
        internal System.Drawing.Color LabelBarStamColor
        {
            set { labelBarStamina.BackColor = value; }
        }

        // Mana
        internal string LabelTextMana
        {
            set { labelTextMana.Text = value; }
        }
        internal System.Drawing.Size LabelBarManaSize
        {
            set { labelBarMana.Size = value; }
        }
        internal System.Drawing.Color LabelBarManaColor
        {
            set { labelBarMana.BackColor = value; }
        }

        // Weight
        internal string LabelWeight
        {
            set { labelWeight.Text = value; }
        }

		public EnhancedToolbar()
		{
            this.FormClosed += new FormClosedEventHandler(EnhancedToolbar_close);
            this.Move += new System.EventHandler(this.EnhancedToolbar_Move);
			InitializeComponent();
		}

        private void EnhancedToolbar_close(object sender, EventArgs e)
        {
            Assistant.Engine.MainWindow.ToolBarOpen = false;
            Assistant.Engine.MainWindow.ToolBar = null;
        }

        private void EnhancedToolbar_Move(object sender, System.EventArgs e)
        {
            System.Drawing.Point pt = this.Location;
            if (this.WindowState != FormWindowState.Minimized )
            {
                Assistant.Engine.MainWindow.LocationToolBarLabel.Text = "X: " + pt.X + " - Y:" + pt.Y;
                RazorEnhanced.Settings.General.WriteInt("PosXToolBar", pt.X);
                RazorEnhanced.Settings.General.WriteInt("PosYToolBar", pt.Y);
            }
        }

        private void EnhancedToolbar_Load(object sender, EventArgs e)
        {
            Assistant.Engine.MainWindow.ToolBarOpen = true;
            ToolBar.UpdateAll();
        }

        private void updateToolBarTimer_Tick(object sender, EventArgs e)
        {
            ToolBar.UpdateAll();
        }

	}
}
