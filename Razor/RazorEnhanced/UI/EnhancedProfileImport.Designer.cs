namespace RazorEnhanced.UI
{
	partial class EnhancedProfileImport
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedProfileImport));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.profilename = new RazorEnhanced.UI.RazorTextBox();
			this.close = new RazorEnhanced.UI.RazorButton();
			this.profileimport = new RazorEnhanced.UI.RazorButton();
			this.label1 = new System.Windows.Forms.Label();
			this.cloneNameLabel = new System.Windows.Forms.Label();
			this.profilefilepathTextBox = new RazorEnhanced.UI.RazorTextBox();
			this.chosefileButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// profilename
			// 
			this.profilename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.profilename.BackColor = System.Drawing.Color.White;
			this.profilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.profilename.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.profilename.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			this.profilename.Location = new System.Drawing.Point(88, 31);
			this.profilename.Name = "profilename";
			this.profilename.Size = new System.Drawing.Size(210, 20);
			this.profilename.TabIndex = 0;
			// 
			// close
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
			this.close.ColorTable = office2010BlueTheme;
			this.close.Location = new System.Drawing.Point(39, 64);
			this.close.Name = "close";
			this.close.Size = new System.Drawing.Size(75, 23);
			this.close.TabIndex = 2;
			this.close.Text = "Close";
			this.close.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.close.UseVisualStyleBackColor = true;
			this.close.Click += new System.EventHandler(this.close_Click);
			// 
			// profileimport
			// 
			this.profileimport.ColorTable = office2010BlueTheme;
			this.profileimport.Location = new System.Drawing.Point(196, 64);
			this.profileimport.Name = "profileimport";
			this.profileimport.Size = new System.Drawing.Size(75, 23);
			this.profileimport.TabIndex = 3;
			this.profileimport.Text = "Import";
			this.profileimport.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.profileimport.UseVisualStyleBackColor = true;
			this.profileimport.Click += new System.EventHandler(this.profileadd_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "New Name:";
			// 
			// cloneNameLabel
			// 
			this.cloneNameLabel.AutoSize = true;
			this.cloneNameLabel.Location = new System.Drawing.Point(12, 9);
			this.cloneNameLabel.Name = "cloneNameLabel";
			this.cloneNameLabel.Size = new System.Drawing.Size(58, 13);
			this.cloneNameLabel.TabIndex = 6;
			this.cloneNameLabel.Text = "Profile File:";
			// 
			// profilefilepathTextBox
			// 
			this.profilefilepathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.profilefilepathTextBox.BackColor = System.Drawing.Color.White;
			this.profilefilepathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.profilefilepathTextBox.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.profilefilepathTextBox.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			this.profilefilepathTextBox.Location = new System.Drawing.Point(88, 7);
			this.profilefilepathTextBox.Name = "profilefilepathTextBox";
			this.profilefilepathTextBox.ReadOnly = true;
			this.profilefilepathTextBox.Size = new System.Drawing.Size(183, 20);
			this.profilefilepathTextBox.TabIndex = 7;
			// 
			// chosefileButton
			// 
			this.chosefileButton.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
			this.chosefileButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.chosefileButton.Location = new System.Drawing.Point(278, 7);
			this.chosefileButton.Name = "chosefileButton";
			this.chosefileButton.Size = new System.Drawing.Size(20, 20);
			this.chosefileButton.TabIndex = 8;
			this.chosefileButton.UseVisualStyleBackColor = true;
			this.chosefileButton.Click += new System.EventHandler(this.chosefileButton_Click);
			// 
			// EnhancedProfileImport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 97);
			this.Controls.Add(this.chosefileButton);
			this.Controls.Add(this.profilefilepathTextBox);
			this.Controls.Add(this.cloneNameLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.profileimport);
			this.Controls.Add(this.close);
			this.Controls.Add(this.profilename);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedProfileImport";
			this.Text = "Enhanced Profile Import";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private RazorTextBox profilename;
		private RazorButton close;
		private RazorButton profileimport;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label cloneNameLabel;
		private RazorTextBox profilefilepathTextBox;
		private System.Windows.Forms.Button chosefileButton;

	}
}