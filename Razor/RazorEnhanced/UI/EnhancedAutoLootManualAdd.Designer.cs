namespace RazorEnhanced.UI
{
    partial class EnhancedAutolootManualAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedAutolootManualAdd));
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tColor = new RazorEnhanced.UI.RazorTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tGraphycs = new RazorEnhanced.UI.RazorTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tName = new RazorEnhanced.UI.RazorTextBox();
            this.bAddItem = new RazorEnhanced.UI.RazorButton();
            this.bClose = new RazorEnhanced.UI.RazorButton();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tColor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tGraphycs);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 131);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Item";
            // 
            // tColor
            // 
            this.tColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.tColor.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.tColor.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.tColor.Location = new System.Drawing.Point(78, 88);
            this.tColor.Name = "tColor";
            this.tColor.Padding = new System.Windows.Forms.Padding(1);
            this.tColor.Size = new System.Drawing.Size(77, 22);
            this.tColor.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Color:";
            // 
            // tGraphycs
            // 
            this.tGraphycs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.tGraphycs.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.tGraphycs.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.tGraphycs.Location = new System.Drawing.Point(78, 56);
            this.tGraphycs.Name = "tGraphycs";
            this.tGraphycs.Padding = new System.Windows.Forms.Padding(1);
            this.tGraphycs.Size = new System.Drawing.Size(77, 22);
            this.tGraphycs.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Graphycs:";
            // 
            // tName
            // 
            this.tName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.tName.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.tName.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.tName.Location = new System.Drawing.Point(78, 26);
            this.tName.Name = "tName";
            this.tName.Padding = new System.Windows.Forms.Padding(1);
            this.tName.Size = new System.Drawing.Size(175, 22);
            this.tName.TabIndex = 1;
            // 
            // bAddItem
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
            this.bAddItem.ColorTable = office2010BlueTheme1;
            this.bAddItem.Location = new System.Drawing.Point(321, 39);
            this.bAddItem.Name = "bAddItem";
            this.bAddItem.Size = new System.Drawing.Size(76, 23);
            this.bAddItem.TabIndex = 2;
            this.bAddItem.Text = "Add";
            this.bAddItem.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.bAddItem.UseVisualStyleBackColor = true;
            this.bAddItem.Click += new System.EventHandler(this.bAddItem_Click);
            // 
            // bClose
            // 
            this.bClose.ColorTable = office2010BlueTheme1;
            this.bClose.Location = new System.Drawing.Point(321, 94);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(76, 23);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "Close";
            this.bClose.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(161, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Set -1 for all color";
            // 
            // EnhancedAutolootManualAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 157);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bAddItem);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnhancedAutolootManualAdd";
            this.Text = "Enhanced Autoloot Manual AddItem";
            this.Load += new System.EventHandler(this.EnhancedAutolootManualAdd_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private RazorTextBox tColor;
        private System.Windows.Forms.Label label3;
        private RazorTextBox tGraphycs;
        private System.Windows.Forms.Label label2;
        private RazorTextBox tName;
        private RazorButton bAddItem;
        private RazorButton bClose;
        private System.Windows.Forms.Label label4;

    }
}