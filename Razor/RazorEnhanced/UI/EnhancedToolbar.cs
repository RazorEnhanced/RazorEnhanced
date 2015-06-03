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

		public EnhancedToolbar()
		{
            this.FormClosed += new FormClosedEventHandler(EnhancedToolbar_close);
			InitializeComponent();
		}

        private void EnhancedToolbar_close(object sender, EventArgs e)
        {
            Assistant.Engine.MainWindow.ToolBarOpen = false;
        }

        private void EnhancedToolbar_Load(object sender, EventArgs e)
        {
            Assistant.Engine.MainWindow.ToolBarOpen = true;

            // Carico parametri del char quando apro la barra
            if (Assistant.World.Player != null)         
            {
                RazorEnhanced.ToolBar.UpdateHits(Assistant.World.Player.HitsMax, Assistant.World.Player.Hits);
            }
        }
	}
}
