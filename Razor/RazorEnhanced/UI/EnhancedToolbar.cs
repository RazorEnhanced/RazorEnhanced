using System;
using System.Drawing;
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
			Assistant.Engine.MainWindow.ToolBarWindows = null;
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
						Assistant.Engine.ToolBarX = pt.X;
						Assistant.Engine.ToolBarY = pt.Y;
					}
				}
			}
		}

		private void EnhancedToolbar_Load(object sender, EventArgs e)
		{
			ToolBar.UpdateAll();
		}
	}
}