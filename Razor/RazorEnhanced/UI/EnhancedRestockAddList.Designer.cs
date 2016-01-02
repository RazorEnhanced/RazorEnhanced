namespace RazorEnhanced.UI
{
	partial class EnhancedRestockAddList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedRestockAddList));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.restockListToAdd = new RazorEnhanced.UI.RazorTextBox();
			this.restockcloseItemList = new RazorEnhanced.UI.RazorButton();
			this.restockaddItemList = new RazorEnhanced.UI.RazorButton();
			this.SuspendLayout();
			// 
			// restockListToAdd
			// 
			this.restockListToAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.restockListToAdd.BackColor = System.Drawing.Color.White;
			this.restockListToAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.restockListToAdd.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.restockListToAdd.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			this.restockListToAdd.Location = new System.Drawing.Point(12, 12);
			this.restockListToAdd.Name = "restockListToAdd";
			this.restockListToAdd.Size = new System.Drawing.Size(286, 20);
			this.restockListToAdd.TabIndex = 0;
			// 
			// restockcloseItemList
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
			this.restockcloseItemList.ColorTable = office2010BlueTheme;
			this.restockcloseItemList.Location = new System.Drawing.Point(39, 41);
			this.restockcloseItemList.Name = "restockcloseItemList";
			this.restockcloseItemList.Size = new System.Drawing.Size(75, 23);
			this.restockcloseItemList.TabIndex = 2;
			this.restockcloseItemList.Text = "Close";
			this.restockcloseItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.restockcloseItemList.UseVisualStyleBackColor = true;
			this.restockcloseItemList.Click += new System.EventHandler(this.restockcloseItemList_Click);
			// 
			// restockaddItemList
			// 
			this.restockaddItemList.ColorTable = office2010BlueTheme;
			this.restockaddItemList.Location = new System.Drawing.Point(196, 41);
			this.restockaddItemList.Name = "restockaddItemList";
			this.restockaddItemList.Size = new System.Drawing.Size(75, 23);
			this.restockaddItemList.TabIndex = 3;
			this.restockaddItemList.Text = "Add";
			this.restockaddItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.restockaddItemList.UseVisualStyleBackColor = true;
			this.restockaddItemList.Click += new System.EventHandler(this.restockaddItemList_Click);
			// 
			// EnhancedRestockAddList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 74);
			this.Controls.Add(this.restockaddItemList);
			this.Controls.Add(this.restockcloseItemList);
			this.Controls.Add(this.restockListToAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedRestockAddList";
			this.Text = "Enhanced Restock Add Item List";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private RazorTextBox restockListToAdd;
		private RazorButton restockcloseItemList;
		private RazorButton restockaddItemList;

	}
}