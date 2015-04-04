namespace RazorEnhanced.UI
{
    partial class EnhancedLauncher
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
            RazorEnhanced.UI.office2010BlueTheme office2010BlueTheme1 = new RazorEnhanced.UI.office2010BlueTheme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedLauncher));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.removeShard = new RazorEnhanced.UI.RazorButton();
            this.addShard = new RazorEnhanced.UI.RazorButton();
            this.shardlistCombobox = new RazorEnhanced.UI.RazorComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.OsiEnc = new RazorEnhanced.UI.RazorCheckBox();
            this.patchEncy = new RazorEnhanced.UI.RazorCheckBox();
            this.serverportT = new RazorEnhanced.UI.RazorTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.serveraddressT = new RazorEnhanced.UI.RazorTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.uofolderL = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bNameCopy = new System.Windows.Forms.Button();
            this.clientlocationL = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.quit = new RazorEnhanced.UI.RazorButton();
            this.okay = new RazorEnhanced.UI.RazorButton();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shard Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.removeShard);
            this.groupBox1.Controls.Add(this.addShard);
            this.groupBox1.Controls.Add(this.shardlistCombobox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 59);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shard Selection";
            // 
            // removeShard
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
            this.removeShard.ColorTable = office2010BlueTheme1;
            this.removeShard.Location = new System.Drawing.Point(390, 19);
            this.removeShard.Name = "removeShard";
            this.removeShard.Size = new System.Drawing.Size(75, 23);
            this.removeShard.TabIndex = 3;
            this.removeShard.Text = "Remove";
            this.removeShard.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.removeShard.UseVisualStyleBackColor = true;
            this.removeShard.Click += new System.EventHandler(this.razorButton2_Click);
            // 
            // addShard
            // 
            this.addShard.ColorTable = office2010BlueTheme1;
            this.addShard.Location = new System.Drawing.Point(309, 19);
            this.addShard.Name = "addShard";
            this.addShard.Size = new System.Drawing.Size(75, 23);
            this.addShard.TabIndex = 2;
            this.addShard.Text = "Add";
            this.addShard.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.addShard.UseVisualStyleBackColor = true;
            this.addShard.Click += new System.EventHandler(this.razorButton1_Click);
            // 
            // shardlistCombobox
            // 
            this.shardlistCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shardlistCombobox.FormattingEnabled = true;
            this.shardlistCombobox.Location = new System.Drawing.Point(82, 19);
            this.shardlistCombobox.Name = "shardlistCombobox";
            this.shardlistCombobox.Size = new System.Drawing.Size(221, 24);
            this.shardlistCombobox.TabIndex = 1;
            this.shardlistCombobox.SelectedIndexChanged += new System.EventHandler(this.shardlistCombobox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OsiEnc);
            this.groupBox2.Controls.Add(this.patchEncy);
            this.groupBox2.Controls.Add(this.serverportT);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.serveraddressT);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.uofolderL);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.bNameCopy);
            this.groupBox2.Controls.Add(this.clientlocationL);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 152);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shard Config";
            // 
            // OsiEnc
            // 
            this.OsiEnc.Location = new System.Drawing.Point(6, 118);
            this.OsiEnc.Name = "OsiEnc";
            this.OsiEnc.Size = new System.Drawing.Size(140, 21);
            this.OsiEnc.TabIndex = 15;
            this.OsiEnc.Text = "Use OSI Encryption";
            this.OsiEnc.CheckedChanged += new System.EventHandler(this.OsiEnc_CheckedChanged);
            // 
            // patchEncy
            // 
            this.patchEncy.BackColor = System.Drawing.SystemColors.Control;
            this.patchEncy.Location = new System.Drawing.Point(6, 92);
            this.patchEncy.Name = "patchEncy";
            this.patchEncy.Size = new System.Drawing.Size(140, 21);
            this.patchEncy.TabIndex = 14;
            this.patchEncy.Text = "Patch client encryption";
            this.patchEncy.UseVisualStyleBackColor = false;
            this.patchEncy.CheckedChanged += new System.EventHandler(this.patchEncy_CheckedChanged);
            // 
            // serverportT
            // 
            this.serverportT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverportT.BackColor = System.Drawing.Color.White;
            this.serverportT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverportT.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.serverportT.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.serverportT.Location = new System.Drawing.Point(339, 66);
            this.serverportT.Name = "serverportT";
            this.serverportT.Size = new System.Drawing.Size(48, 20);
            this.serverportT.TabIndex = 13;
            this.serverportT.TextChanged += new System.EventHandler(this.serverportT_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(309, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Port:";
            // 
            // serveraddressT
            // 
            this.serveraddressT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serveraddressT.BackColor = System.Drawing.Color.White;
            this.serveraddressT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serveraddressT.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.serveraddressT.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.serveraddressT.Location = new System.Drawing.Point(94, 66);
            this.serveraddressT.Name = "serveraddressT";
            this.serveraddressT.Size = new System.Drawing.Size(208, 20);
            this.serveraddressT.TabIndex = 11;
            this.serveraddressT.TextChanged += new System.EventHandler(this.serveraddressT_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Server Address:";
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(6, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 20);
            this.button1.TabIndex = 9;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uofolderL
            // 
            this.uofolderL.AutoSize = true;
            this.uofolderL.ForeColor = System.Drawing.Color.Red;
            this.uofolderL.Location = new System.Drawing.Point(118, 46);
            this.uofolderL.Name = "uofolderL";
            this.uofolderL.Size = new System.Drawing.Size(43, 13);
            this.uofolderL.TabIndex = 8;
            this.uofolderL.Text = "Not Set";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "UO Folder:";
            // 
            // bNameCopy
            // 
            this.bNameCopy.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
            this.bNameCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bNameCopy.Location = new System.Drawing.Point(6, 20);
            this.bNameCopy.Name = "bNameCopy";
            this.bNameCopy.Size = new System.Drawing.Size(20, 20);
            this.bNameCopy.TabIndex = 6;
            this.bNameCopy.UseVisualStyleBackColor = true;
            this.bNameCopy.Click += new System.EventHandler(this.bNameCopy_Click);
            // 
            // clientlocationL
            // 
            this.clientlocationL.AutoSize = true;
            this.clientlocationL.ForeColor = System.Drawing.Color.Red;
            this.clientlocationL.Location = new System.Drawing.Point(118, 23);
            this.clientlocationL.Name = "clientlocationL";
            this.clientlocationL.Size = new System.Drawing.Size(43, 13);
            this.clientlocationL.TabIndex = 5;
            this.clientlocationL.Text = "Not Set";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Client Location:";
            // 
            // quit
            // 
            this.quit.ColorTable = office2010BlueTheme1;
            this.quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quit.Location = new System.Drawing.Point(279, 236);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(72, 20);
            this.quit.TabIndex = 9;
            this.quit.Text = "Exit";
            this.quit.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.quit.Click += new System.EventHandler(this.quit_Click);
            // 
            // okay
            // 
            this.okay.ColorTable = office2010BlueTheme1;
            this.okay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okay.Location = new System.Drawing.Point(157, 236);
            this.okay.Name = "okay";
            this.okay.Size = new System.Drawing.Size(72, 20);
            this.okay.TabIndex = 8;
            this.okay.Text = "Launch";
            this.okay.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.okay.Click += new System.EventHandler(this.okay_Click);
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "exe";
            this.openFile.FileName = "client.exe";
            this.openFile.Filter = "Executable Files|*.exe";
            this.openFile.Title = "Select Client";
            // 
            // EnhancedLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 267);
            this.Controls.Add(this.quit);
            this.Controls.Add(this.okay);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnhancedLauncher";
            this.Text = "Welcom to Razor Enhanced";
            this.Load += new System.EventHandler(this.EnhancedLauncher_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private RazorButton removeShard;
        private RazorButton addShard;
        private System.Windows.Forms.Label clientlocationL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bNameCopy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label uofolderL;
        private System.Windows.Forms.Label label5;
        private RazorTextBox serverportT;
        private System.Windows.Forms.Label label7;
        private RazorTextBox serveraddressT;
        private System.Windows.Forms.Label label6;
        private RazorCheckBox OsiEnc;
        private RazorCheckBox patchEncy;
        private RazorButton quit;
        private RazorButton okay;
        private System.Windows.Forms.GroupBox groupBox2;
        public RazorComboBox shardlistCombobox;
        private System.Windows.Forms.OpenFileDialog openFile;
    


    }
}