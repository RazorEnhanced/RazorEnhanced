namespace RazorEnhanced.UI
{
	partial class EnhancedAgentAddList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedAgentAddList));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.AgentListToAdd = new RazorEnhanced.UI.RazorTextBox();
			this.agentcloseItemList = new RazorEnhanced.UI.RazorButton();
			this.agentaddItemList = new RazorEnhanced.UI.RazorButton();
			this.SuspendLayout();
			// 
			// AgentListToAdd
			// 
			this.AgentListToAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.AgentListToAdd.BackColor = System.Drawing.Color.White;
			this.AgentListToAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AgentListToAdd.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.AgentListToAdd.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			this.AgentListToAdd.Location = new System.Drawing.Point(12, 12);
			this.AgentListToAdd.Name = "AgentListToAdd";
			this.AgentListToAdd.Size = new System.Drawing.Size(286, 20);
			this.AgentListToAdd.TabIndex = 0;
			// 
			// agentcloseItemList
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
			this.agentcloseItemList.ColorTable = office2010BlueTheme1;
			this.agentcloseItemList.Location = new System.Drawing.Point(39, 41);
			this.agentcloseItemList.Name = "agentcloseItemList";
			this.agentcloseItemList.Size = new System.Drawing.Size(75, 23);
			this.agentcloseItemList.TabIndex = 2;
			this.agentcloseItemList.Text = "Close";
			this.agentcloseItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.agentcloseItemList.UseVisualStyleBackColor = true;
			this.agentcloseItemList.Click += new System.EventHandler(this.EnhancedAgentCloseItemList_Click);
			// 
			// agentaddItemList
			// 
			this.agentaddItemList.ColorTable = office2010BlueTheme1;
			this.agentaddItemList.Location = new System.Drawing.Point(196, 41);
			this.agentaddItemList.Name = "agentaddItemList";
			this.agentaddItemList.Size = new System.Drawing.Size(75, 23);
			this.agentaddItemList.TabIndex = 3;
			this.agentaddItemList.Text = "Add";
			this.agentaddItemList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
			this.agentaddItemList.UseVisualStyleBackColor = true;
			this.agentaddItemList.Click += new System.EventHandler(this.EnhancedAgentAddList_Click);
			// 
			// EnhancedAgentAddList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 74);
			this.Controls.Add(this.agentaddItemList);
			this.Controls.Add(this.agentcloseItemList);
			this.Controls.Add(this.AgentListToAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnhancedAgentAddList";
			this.Text = "Agent add List";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private RazorTextBox AgentListToAdd;
		private RazorButton agentcloseItemList;
		private RazorButton agentaddItemList;

	}
}