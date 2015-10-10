namespace RazorEnhanced.UI
{
	partial class EnhancedDressAddUndressLayer
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
			RazorEnhanced.UI.Office2010BlueTheme office2010BlueTheme1 = new RazorEnhanced.UI.Office2010BlueTheme();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedDressAddUndressLayer));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.layerlist = new RazorEnhanced.UI.RazorComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.bClose = new RazorEnhanced.UI.RazorButton();
			this.bAddItem = new RazorEnhanced.UI.RazorButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.layerlist);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(9, 10);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(221, 64);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Layer";
			// 
			// layerlist
			// 
			this.layerlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.layerlist.FormattingEnabled = true;
			this.layerlist.Location = new System.Drawing.Point(51, 18);
			this.layerlist.Name = "layerlist";
			this.layerlist.Size = new System.Drawing.Size(151, 24);
			this.layerlist.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Layer: ";
			// 
			// bClose
			// 
			office2010BlueTheme1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			office2010BlueTheme1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			office2010BlueTheme1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			office2010BlueTheme1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			office2010BlueTheme1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
			office2010BlueTheme1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
			office2010BlueTheme1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			office2010BlueTheme1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			office2010BlueTheme1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
			office2010BlueTheme1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
			office2010BlueTheme1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			office2010BlueTheme1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			office2010BlueTheme1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
			office2010BlueTheme1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
			office2010BlueTheme1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			office2010BlueTheme1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			office2010BlueTheme1.TextColor = System.Drawing.Color.White;
			this.bClose.ColorTable = office2010BlueTheme1;
			this.bClose.Location = new System.Drawing.Point(246, 45);
			this.bClose.Margin = new System.Windows.Forms.Padding(2);
			this.bClose.Name = "bClose";
			this.bClose.Size = new System.Drawing.Size(57, 19);
			this.bClose.TabIndex = 3;
			this.bClose.Text = "Close";
			this.bClose.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.bClose.UseVisualStyleBackColor = true;
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// bAddItem
			// 
			this.bAddItem.ColorTable = office2010BlueTheme1;
			this.bAddItem.Location = new System.Drawing.Point(246, 22);
			this.bAddItem.Margin = new System.Windows.Forms.Padding(2);
			this.bAddItem.Name = "bAddItem";
			this.bAddItem.Size = new System.Drawing.Size(57, 19);
			this.bAddItem.TabIndex = 2;
			this.bAddItem.Text = "Add";
			this.bAddItem.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.bAddItem.UseVisualStyleBackColor = true;
			this.bAddItem.Click += new System.EventHandler(this.bAddItem_Click);
			// 
			// EnhancedDressAddUndressLayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(314, 82);
			this.Controls.Add(this.bClose);
			this.Controls.Add(this.bAddItem);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedDressAddUndressLayer";
			this.Text = "Enhanced Dress Add Clear Layer";
			this.Load += new System.EventHandler(this.EnhancedDressAddUndressLayer_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.GroupBox groupBox1;
		private RazorButton bAddItem;
		private RazorButton bClose;
		private RazorComboBox layerlist;
		private System.Windows.Forms.Label label1;

	}
}