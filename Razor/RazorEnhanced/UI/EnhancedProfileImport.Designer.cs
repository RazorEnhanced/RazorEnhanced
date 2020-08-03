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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedProfileImport));
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.profilename = new RazorEnhanced.UI.RazorTextBox();
			this.close = new RazorEnhanced.UI.RazorButton();
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
			this.profilename.Location = new System.Drawing.Point(88, 31);
			this.profilename.Name = "profilename";
			this.profilename.Size = new System.Drawing.Size(210, 20);
			this.profilename.TabIndex = 0;
			//
			// close
			//
			this.close.Location = new System.Drawing.Point(39, 64);
			this.close.Name = "close";
			this.close.Size = new System.Drawing.Size(75, 23);
			this.close.TabIndex = 2;
			this.close.Text = "Close";
			this.close.UseVisualStyleBackColor = true;
			this.close.Click += new System.EventHandler(this.close_Click);
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
		//private RazorButton profileimport;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label cloneNameLabel;
		private RazorTextBox profilefilepathTextBox;
		private System.Windows.Forms.Button chosefileButton;

	}
}
