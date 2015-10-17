namespace Assistant.Map
{
	partial class MapWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.uoMapControl = new Assistant.Map.UOMapControl();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.SuspendLayout();
			// 
			// uoMapControl
			// 
			this.uoMapControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.uoMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uoMapControl.Location = new System.Drawing.Point(0, 0);
			this.uoMapControl.Name = "uoMapControl";
			this.uoMapControl.Size = new System.Drawing.Size(284, 261);
			this.uoMapControl.TabIndex = 0;
			this.uoMapControl.Text = "UOAM2 -  --  - [0°0\'N 0°0\'W | (X: 0, Y: 0)]";
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
			// 
			// MapWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.uoMapControl);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "MapWindow";
			this.Text = "UO Positioning System";
			this.Deactivate += new System.EventHandler(this.MapWindow_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapWindow_FormClosing);
			this.Move += new System.EventHandler(this.MapWindow_Move);
			this.Resize += new System.EventHandler(this.MapWindow_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private UOMapControl uoMapControl;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
	}
}