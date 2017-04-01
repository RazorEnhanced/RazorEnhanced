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
			this.shardAdd.Location = new System.Drawing.Point(12, 12);
			this.shardAdd.Name = "shardAdd";
			this.shardAdd.Size = new System.Drawing.Size(286, 20);
			this.shardAdd.TabIndex = 0;
			// 
			// autolootcloseItemList
			// 
			this.autolootcloseItemList.Location = new System.Drawing.Point(39, 41);
			this.autolootcloseItemList.Name = "autolootcloseItemList";
			this.autolootcloseItemList.Size = new System.Drawing.Size(75, 23);
			this.autolootcloseItemList.TabIndex = 2;
			this.autolootcloseItemList.Text = "Close";
			this.autolootcloseItemList.UseVisualStyleBackColor = true;
			this.autolootcloseItemList.Click += new System.EventHandler(this.autolootcloseItemList_Click);
			// 
			// autolootaddItemList
			// 
			this.autolootaddItemList.Location = new System.Drawing.Point(196, 41);
			this.autolootaddItemList.Name = "autolootaddItemList";
			this.autolootaddItemList.Size = new System.Drawing.Size(75, 23);
			this.autolootaddItemList.TabIndex = 3;
			this.autolootaddItemList.Text = "Add";
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