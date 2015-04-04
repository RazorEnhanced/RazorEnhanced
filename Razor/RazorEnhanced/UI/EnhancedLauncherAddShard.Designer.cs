namespace RazorEnhanced.UI
{
    partial class EnhancedLauncherAddShard
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
            RazorEnhanced.UI.Office2010BlueTheme RazorButton = new RazorEnhanced.UI.Office2010BlueTheme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedLauncherAddShard));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.shardAdd = new RazorEnhanced.UI.RazorTextBox();
            this.autolootcloseItemList = new RazorEnhanced.UI.RazorButton();
            this.autolootaddItemList = new RazorEnhanced.UI.RazorButton();
            this.SuspendLayout();
            // 
            // shardAdd
            // 
            this.shardAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shardAdd.BackColor = System.Drawing.Color.White;
            this.shardAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shardAdd.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.shardAdd.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.shardAdd.Location = new System.Drawing.Point(12, 12);
            this.shardAdd.Name = "shardAdd";
            this.shardAdd.Size = new System.Drawing.Size(286, 20);
            this.shardAdd.TabIndex = 0;
            // 
            // autolootcloseItemList
            // 
            RazorButton.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            RazorButton.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            RazorButton.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            RazorButton.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            RazorButton.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            RazorButton.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            RazorButton.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            RazorButton.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            RazorButton.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            RazorButton.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            RazorButton.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            RazorButton.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            RazorButton.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            RazorButton.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            RazorButton.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            RazorButton.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            RazorButton.TextColor = System.Drawing.Color.White;
            this.autolootcloseItemList.ColorTable = RazorButton;
            this.autolootcloseItemList.Location = new System.Drawing.Point(39, 41);
            this.autolootcloseItemList.Name = "autolootcloseItemList";
            this.autolootcloseItemList.Size = new System.Drawing.Size(75, 23);
            this.autolootcloseItemList.TabIndex = 2;
            this.autolootcloseItemList.Text = "Close";
            this.autolootcloseItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootcloseItemList.UseVisualStyleBackColor = true;
            this.autolootcloseItemList.Click += new System.EventHandler(this.autolootcloseItemList_Click);
            // 
            // autolootaddItemList
            // 
            this.autolootaddItemList.ColorTable = RazorButton;
            this.autolootaddItemList.Location = new System.Drawing.Point(196, 41);
            this.autolootaddItemList.Name = "autolootaddItemList";
            this.autolootaddItemList.Size = new System.Drawing.Size(75, 23);
            this.autolootaddItemList.TabIndex = 3;
            this.autolootaddItemList.Text = "Add";
            this.autolootaddItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootaddItemList.UseVisualStyleBackColor = true;
            this.autolootaddItemList.Click += new System.EventHandler(this.autolootaddItemList_Click);
            // 
            // EnhancedLauncherAddShard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 74);
            this.Controls.Add(this.autolootaddItemList);
            this.Controls.Add(this.autolootcloseItemList);
            this.Controls.Add(this.shardAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnhancedLauncherAddShard";
            this.Text = "Enhanced Launcher Add Shard";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private RazorTextBox shardAdd;
        private RazorButton autolootcloseItemList;
        private RazorButton autolootaddItemList;

    }
}