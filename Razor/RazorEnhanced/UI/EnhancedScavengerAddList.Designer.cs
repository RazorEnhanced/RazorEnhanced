namespace RazorEnhanced.UI
{
	partial class EnhancedScavengerAddList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedScavengerAddList));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.scavengerListToAdd = new RazorEnhanced.UI.RazorTextBox();
			this.scavengercloseItemList = new RazorEnhanced.UI.RazorButton();
			this.scavengeraddItemList = new RazorEnhanced.UI.RazorButton();
			this.SuspendLayout();
			// 
			// scavengerListToAdd
			// 
			this.scavengerListToAdd.Location = new System.Drawing.Point(12, 12);
			this.scavengerListToAdd.Name = "scavengerListToAdd";
			this.scavengerListToAdd.Padding = new System.Windows.Forms.Padding(1);
			this.scavengerListToAdd.Size = new System.Drawing.Size(286, 20);
			this.scavengerListToAdd.TabIndex = 0;
			// 
			// scavengercloseItemList
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
			this.scavengercloseItemList.ColorTable = office2010BlueTheme;
			this.scavengercloseItemList.Location = new System.Drawing.Point(39, 41);
			this.scavengercloseItemList.Name = "scavengercloseItemList";
			this.scavengercloseItemList.Size = new System.Drawing.Size(75, 23);
			this.scavengercloseItemList.TabIndex = 2;
			this.scavengercloseItemList.Text = "Close";
			this.scavengercloseItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.scavengercloseItemList.UseVisualStyleBackColor = true;
			this.scavengercloseItemList.Click += new System.EventHandler(this.scavegercloseItemList_Click);
			// 
			// scavengeraddItemList
			// 
			this.scavengeraddItemList.ColorTable = office2010BlueTheme;
			this.scavengeraddItemList.Location = new System.Drawing.Point(196, 41);
			this.scavengeraddItemList.Name = "scavengeraddItemList";
			this.scavengeraddItemList.Size = new System.Drawing.Size(75, 23);
			this.scavengeraddItemList.TabIndex = 3;
			this.scavengeraddItemList.Text = "Add";
			this.scavengeraddItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.scavengeraddItemList.UseVisualStyleBackColor = true;
			this.scavengeraddItemList.Click += new System.EventHandler(this.scavegeraddItemList_Click);
			// 
			// EnhancedScavengerAddItemList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 74);
			this.Controls.Add(this.scavengeraddItemList);
			this.Controls.Add(this.scavengercloseItemList);
			this.Controls.Add(this.scavengerListToAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedScavengerAddItemList";
			this.Text = "Enhanced Scavenger Add Item List";
			this.Load += new System.EventHandler(this.EnhancedAutolootAddItemList_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private RazorTextBox scavengerListToAdd;
		private RazorButton scavengercloseItemList;
		private RazorButton scavengeraddItemList;

	}
}