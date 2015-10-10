namespace RazorEnhanced.UI
{
	partial class EnhancedAutoLootAddList
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
			RazorEnhanced.UI.Office2010BlueTheme office2010BlueTheme = new RazorEnhanced.UI.Office2010BlueTheme();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedAutoLootAddList));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.autolootListToAdd = new RazorEnhanced.UI.RazorTextBox();
			this.autolootcloseItemList = new RazorEnhanced.UI.RazorButton();
			this.autolootaddItemList = new RazorEnhanced.UI.RazorButton();
			this.SuspendLayout();
			// 
			// autolootListToAdd
			// 
			this.autolootListToAdd.Location = new System.Drawing.Point(12, 12);
			this.autolootListToAdd.Name = "autolootListToAdd";
			this.autolootListToAdd.Padding = new System.Windows.Forms.Padding(1);
			this.autolootListToAdd.Size = new System.Drawing.Size(286, 20);
			this.autolootListToAdd.TabIndex = 0;
			// 
			// autolootcloseItemList
			// 
			office2010BlueTheme.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			office2010BlueTheme.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			office2010BlueTheme.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			office2010BlueTheme.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			office2010BlueTheme.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
			office2010BlueTheme.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
			office2010BlueTheme.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			office2010BlueTheme.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			office2010BlueTheme.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
			office2010BlueTheme.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
			office2010BlueTheme.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			office2010BlueTheme.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			office2010BlueTheme.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
			office2010BlueTheme.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
			office2010BlueTheme.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			office2010BlueTheme.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			office2010BlueTheme.TextColor = System.Drawing.Color.White;
			this.autolootcloseItemList.ColorTable = office2010BlueTheme;
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
			this.autolootaddItemList.ColorTable = office2010BlueTheme;
			this.autolootaddItemList.Location = new System.Drawing.Point(196, 41);
			this.autolootaddItemList.Name = "autolootaddItemList";
			this.autolootaddItemList.Size = new System.Drawing.Size(75, 23);
			this.autolootaddItemList.TabIndex = 3;
			this.autolootaddItemList.Text = "Add";
			this.autolootaddItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.autolootaddItemList.UseVisualStyleBackColor = true;
			this.autolootaddItemList.Click += new System.EventHandler(this.autolootaddItemList_Click);
			// 
			// EnhancedAutolootAddItemList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 74);
			this.Controls.Add(this.autolootaddItemList);
			this.Controls.Add(this.autolootcloseItemList);
			this.Controls.Add(this.autolootListToAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedAutolootAddItemList";
			this.Text = "Enhanced Autoloot Add Item List";
			this.Load += new System.EventHandler(this.EnhancedAutolootAddItemList_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private RazorTextBox autolootListToAdd;
		private RazorButton autolootcloseItemList;
		private RazorButton autolootaddItemList;

	}
}