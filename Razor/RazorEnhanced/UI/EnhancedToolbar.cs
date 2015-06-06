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
            if (this.Focused)
            {
                if (Assistant.Engine.MainWindow.LockToolBarCheckBox.Checked)
                {
                    this.Location = new Point(RazorEnhanced.Settings.General.ReadInt("PosXToolBar"), RazorEnhanced.Settings.General.ReadInt("PosYToolBar"));
                }
                else
                {
                    System.Drawing.Point pt = this.Location;
                    if (this.WindowState != FormWindowState.Minimized)
                    {
                        Assistant.Engine.MainWindow.LocationToolBarLabel.Text = "X: " + pt.X + " - Y:" + pt.Y;
                        RazorEnhanced.Settings.General.WriteInt("PosXToolBar", pt.X);
                        RazorEnhanced.Settings.General.WriteInt("PosYToolBar", pt.Y);
                    }
                }
            }
        }

        private void EnhancedToolbar_Load(object sender, EventArgs e)
        {
            Assistant.Engine.MainWindow.ToolBarOpen = true;
            ToolBar.UpdateAll();
        }
	}
}
