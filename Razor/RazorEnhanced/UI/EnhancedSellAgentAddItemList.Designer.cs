namespace RazorEnhanced.UI
{
    partial class EnhancedSellAgentAddItemList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedSellAgentAddItemList));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.sellagentListToAdd = new RazorEnhanced.UI.RazorTextBox();
			this.sellagentcloseItemList = new RazorEnhanced.UI.RazorButton();
			this.sellagentaddItemList = new RazorEnhanced.UI.RazorButton();
			this.SuspendLayout();
			// 
			// sellagentListToAdd
			// 
			this.sellagentListToAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.sellagentListToAdd.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.sellagentListToAdd.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			this.sellagentListToAdd.Location = new System.Drawing.Point(12, 12);
			this.sellagentListToAdd.Name = "sellagentListToAdd";
			this.sellagentListToAdd.Padding = new System.Windows.Forms.Padding(1);
			this.sellagentListToAdd.Size = new System.Drawing.Size(286, 20);
			this.sellagentListToAdd.TabIndex = 0;
			// 
			// sellagentcloseItemList
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
			this.sellagentcloseItemList.ColorTable = office2010BlueTheme1;
			this.sellagentcloseItemList.Location = new System.Drawing.Point(39, 41);
			this.sellagentcloseItemList.Name = "sellagentcloseItemList";
			this.sellagentcloseItemList.Size = new System.Drawing.Size(75, 23);
			this.sellagentcloseItemList.TabIndex = 2;
			this.sellagentcloseItemList.Text = "Close";
			this.sellagentcloseItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.sellagentcloseItemList.UseVisualStyleBackColor = true;
			this.sellagentcloseItemList.Click += new System.EventHandler(this.sellcloseItemList_Click);
			// 
			// sellagentaddItemList
			// 
			this.sellagentaddItemList.ColorTable = office2010BlueTheme1;
			this.sellagentaddItemList.Location = new System.Drawing.Point(196, 41);
			this.sellagentaddItemList.Name = "sellagentaddItemList";
			this.sellagentaddItemList.Size = new System.Drawing.Size(75, 23);
			this.sellagentaddItemList.TabIndex = 3;
			this.sellagentaddItemList.Text = "Add";
			this.sellagentaddItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.sellagentaddItemList.UseVisualStyleBackColor = true;
			this.sellagentaddItemList.Click += new System.EventHandler(this.selladdItemList_Click);
			// 
			// EnhancedSellAgentAddItemList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 74);
			this.Controls.Add(this.sellagentaddItemList);
			this.Controls.Add(this.sellagentcloseItemList);
			this.Controls.Add(this.sellagentListToAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedSellAgentAddItemList";
			this.Text = "Enhanced Sell Add Item List";
			this.Load += new System.EventHandler(this.EnhancedSellAgentAddItemList_Load);
			this.ResumeLayout(false);

		}

		#endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private RazorTextBox sellagentListToAdd;
        private RazorButton sellagentcloseItemList;
        private RazorButton sellagentaddItemList;

    }
}