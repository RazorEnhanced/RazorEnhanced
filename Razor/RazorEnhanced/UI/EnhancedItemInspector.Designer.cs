namespace RazorEnhanced.UI
{
    partial class EnhancedItemInspector
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            RazorEnhanced.UI.Office2010BlueTheme office2010BlueTheme = new RazorEnhanced.UI.Office2010BlueTheme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedItemInspector));
            this.bNameCopy = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lName = new System.Windows.Forms.Label();
            this.lSerial = new System.Windows.Forms.Label();
            this.lItemID = new System.Windows.Forms.Label();
            this.lColor = new System.Windows.Forms.Label();
            this.lPosition = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bPositionCopy = new System.Windows.Forms.Button();
            this.bColorCopy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.bItemIdCopy = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bSerialCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lContainer = new System.Windows.Forms.Label();
            this.lOwned = new System.Windows.Forms.Label();
            this.lLayer = new System.Windows.Forms.Label();
            this.lAmount = new System.Windows.Forms.Label();
            this.lRootContainer = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bOwnedCopy = new System.Windows.Forms.Button();
            this.bLayerCopy = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.bAmountCopy = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.bRContainerCopy = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.bContainerCopy = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.razorButton1 = new RazorEnhanced.UI.RazorButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bNameCopy
            // 
            this.bNameCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bNameCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bNameCopy.Location = new System.Drawing.Point(8, 30);
            this.bNameCopy.Name = "bNameCopy";
            this.bNameCopy.Size = new System.Drawing.Size(25, 25);
            this.bNameCopy.TabIndex = 0;
            this.bNameCopy.UseVisualStyleBackColor = true;
            this.bNameCopy.Click += new System.EventHandler(this.bNameCopy_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lName);
            this.groupBox1.Controls.Add(this.lSerial);
            this.groupBox1.Controls.Add(this.lItemID);
            this.groupBox1.Controls.Add(this.lColor);
            this.groupBox1.Controls.Add(this.lPosition);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.bPositionCopy);
            this.groupBox1.Controls.Add(this.bColorCopy);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.bItemIdCopy);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bSerialCopy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.bNameCopy);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 181);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Location = new System.Drawing.Point(97, 36);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(30, 17);
            this.lName.TabIndex = 17;
            this.lName.Text = "null";
            this.lName.Click += new System.EventHandler(this.lName_Click);
            // 
            // lSerial
            // 
            this.lSerial.AutoSize = true;
            this.lSerial.Location = new System.Drawing.Point(97, 64);
            this.lSerial.Name = "lSerial";
            this.lSerial.Size = new System.Drawing.Size(86, 17);
            this.lSerial.TabIndex = 16;
            this.lSerial.Text = "0x00000000";
            // 
            // lItemID
            // 
            this.lItemID.AutoSize = true;
            this.lItemID.Location = new System.Drawing.Point(97, 96);
            this.lItemID.Name = "lItemID";
            this.lItemID.Size = new System.Drawing.Size(54, 17);
            this.lItemID.TabIndex = 15;
            this.lItemID.Text = "0x0000";
            // 
            // lColor
            // 
            this.lColor.AutoSize = true;
            this.lColor.Location = new System.Drawing.Point(97, 124);
            this.lColor.Name = "lColor";
            this.lColor.Size = new System.Drawing.Size(54, 17);
            this.lColor.TabIndex = 14;
            this.lColor.Text = "0x0000";
            // 
            // lPosition
            // 
            this.lPosition.AutoSize = true;
            this.lPosition.Location = new System.Drawing.Point(97, 154);
            this.lPosition.Name = "lPosition";
            this.lPosition.Size = new System.Drawing.Size(48, 17);
            this.lPosition.TabIndex = 13;
            this.lPosition.Text = "0, 0, 0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Position:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Color:";
            // 
            // bPositionCopy
            // 
            this.bPositionCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bPositionCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bPositionCopy.Location = new System.Drawing.Point(8, 150);
            this.bPositionCopy.Name = "bPositionCopy";
            this.bPositionCopy.Size = new System.Drawing.Size(25, 25);
            this.bPositionCopy.TabIndex = 6;
            this.bPositionCopy.UseVisualStyleBackColor = true;
            this.bPositionCopy.Click += new System.EventHandler(this.bPositionCopy_Click);
            // 
            // bColorCopy
            // 
            this.bColorCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bColorCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bColorCopy.Location = new System.Drawing.Point(8, 120);
            this.bColorCopy.Name = "bColorCopy";
            this.bColorCopy.Size = new System.Drawing.Size(25, 25);
            this.bColorCopy.TabIndex = 5;
            this.bColorCopy.UseVisualStyleBackColor = true;
            this.bColorCopy.Click += new System.EventHandler(this.bColorCopy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "ItemID";
            // 
            // bItemIdCopy
            // 
            this.bItemIdCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bItemIdCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bItemIdCopy.Location = new System.Drawing.Point(8, 90);
            this.bItemIdCopy.Name = "bItemIdCopy";
            this.bItemIdCopy.Size = new System.Drawing.Size(25, 25);
            this.bItemIdCopy.TabIndex = 4;
            this.bItemIdCopy.UseVisualStyleBackColor = true;
            this.bItemIdCopy.Click += new System.EventHandler(this.bItemIdCopy_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Serial:";
            // 
            // bSerialCopy
            // 
            this.bSerialCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bSerialCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bSerialCopy.Location = new System.Drawing.Point(8, 60);
            this.bSerialCopy.Name = "bSerialCopy";
            this.bSerialCopy.Size = new System.Drawing.Size(25, 25);
            this.bSerialCopy.TabIndex = 2;
            this.bSerialCopy.UseVisualStyleBackColor = true;
            this.bSerialCopy.Click += new System.EventHandler(this.bSerialCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lContainer);
            this.groupBox2.Controls.Add(this.lOwned);
            this.groupBox2.Controls.Add(this.lLayer);
            this.groupBox2.Controls.Add(this.lAmount);
            this.groupBox2.Controls.Add(this.lRootContainer);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.bOwnedCopy);
            this.groupBox2.Controls.Add(this.bLayerCopy);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.bAmountCopy);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.bRContainerCopy);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.bContainerCopy);
            this.groupBox2.Location = new System.Drawing.Point(12, 199);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 187);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Item Details";
            // 
            // lContainer
            // 
            this.lContainer.AutoSize = true;
            this.lContainer.Location = new System.Drawing.Point(147, 34);
            this.lContainer.Name = "lContainer";
            this.lContainer.Size = new System.Drawing.Size(86, 17);
            this.lContainer.TabIndex = 18;
            this.lContainer.Text = "0x00000000";
            // 
            // lOwned
            // 
            this.lOwned.AutoSize = true;
            this.lOwned.Location = new System.Drawing.Point(147, 154);
            this.lOwned.Name = "lOwned";
            this.lOwned.Size = new System.Drawing.Size(26, 17);
            this.lOwned.TabIndex = 12;
            this.lOwned.Text = "No";
            // 
            // lLayer
            // 
            this.lLayer.AutoSize = true;
            this.lLayer.Location = new System.Drawing.Point(147, 126);
            this.lLayer.Name = "lLayer";
            this.lLayer.Size = new System.Drawing.Size(16, 17);
            this.lLayer.TabIndex = 11;
            this.lLayer.Text = "0";
            // 
            // lAmount
            // 
            this.lAmount.AutoSize = true;
            this.lAmount.Location = new System.Drawing.Point(147, 94);
            this.lAmount.Name = "lAmount";
            this.lAmount.Size = new System.Drawing.Size(16, 17);
            this.lAmount.TabIndex = 10;
            this.lAmount.Text = "0";
            // 
            // lRootContainer
            // 
            this.lRootContainer.AutoSize = true;
            this.lRootContainer.Location = new System.Drawing.Point(147, 64);
            this.lRootContainer.Name = "lRootContainer";
            this.lRootContainer.Size = new System.Drawing.Size(86, 17);
            this.lRootContainer.TabIndex = 9;
            this.lRootContainer.Text = "0x00000000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Owned:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "Layer:";
            // 
            // bOwnedCopy
            // 
            this.bOwnedCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bOwnedCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bOwnedCopy.Location = new System.Drawing.Point(8, 150);
            this.bOwnedCopy.Name = "bOwnedCopy";
            this.bOwnedCopy.Size = new System.Drawing.Size(25, 25);
            this.bOwnedCopy.TabIndex = 6;
            this.bOwnedCopy.UseVisualStyleBackColor = true;
            this.bOwnedCopy.Click += new System.EventHandler(this.bOwnedCopy_Click);
            // 
            // bLayerCopy
            // 
            this.bLayerCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bLayerCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bLayerCopy.Location = new System.Drawing.Point(8, 120);
            this.bLayerCopy.Name = "bLayerCopy";
            this.bLayerCopy.Size = new System.Drawing.Size(25, 25);
            this.bLayerCopy.TabIndex = 5;
            this.bLayerCopy.UseVisualStyleBackColor = true;
            this.bLayerCopy.Click += new System.EventHandler(this.bLayerCopy_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Amount:";
            // 
            // bAmountCopy
            // 
            this.bAmountCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bAmountCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bAmountCopy.Location = new System.Drawing.Point(8, 90);
            this.bAmountCopy.Name = "bAmountCopy";
            this.bAmountCopy.Size = new System.Drawing.Size(25, 25);
            this.bAmountCopy.TabIndex = 4;
            this.bAmountCopy.UseVisualStyleBackColor = true;
            this.bAmountCopy.Click += new System.EventHandler(this.bAmountCopy_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(39, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Root Container:";
            // 
            // bRContainerCopy
            // 
            this.bRContainerCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bRContainerCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bRContainerCopy.Location = new System.Drawing.Point(8, 60);
            this.bRContainerCopy.Name = "bRContainerCopy";
            this.bRContainerCopy.Size = new System.Drawing.Size(25, 25);
            this.bRContainerCopy.TabIndex = 2;
            this.bRContainerCopy.UseVisualStyleBackColor = true;
            this.bRContainerCopy.Click += new System.EventHandler(this.bRContainerCopy_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(39, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "Container:";
            // 
            // bContainerCopy
            // 
            this.bContainerCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bContainerCopy.Image = global::Assistant.Properties.Resources.RoundButton;
            this.bContainerCopy.Location = new System.Drawing.Point(8, 30);
            this.bContainerCopy.Name = "bContainerCopy";
            this.bContainerCopy.Size = new System.Drawing.Size(25, 25);
            this.bContainerCopy.TabIndex = 0;
            this.bContainerCopy.UseVisualStyleBackColor = true;
            this.bContainerCopy.Click += new System.EventHandler(this.bContainerCopy_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.Location = new System.Drawing.Point(348, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 373);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Attributes";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(12, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(289, 340);
            this.listBox1.TabIndex = 0;
            // 
            // razorButton1
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
            this.razorButton1.ColorTable = office2010BlueTheme;
            this.razorButton1.Location = new System.Drawing.Point(307, 392);
            this.razorButton1.Name = "razorButton1";
            this.razorButton1.Size = new System.Drawing.Size(75, 23);
            this.razorButton1.TabIndex = 10;
            this.razorButton1.Text = "Close";
            this.razorButton1.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButton1.UseVisualStyleBackColor = true;
            this.razorButton1.Click += new System.EventHandler(this.razorButton1_Click);
            // 
            // EnhancedItemInspector
            // 
            this.ClientSize = new System.Drawing.Size(676, 423);
            this.Controls.Add(this.razorButton1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnhancedItemInspector";
            this.Text = "Enhanced Item Inspector";
            this.Load += new System.EventHandler(this.EnhancedItemInspect_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button bNameCopy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bPositionCopy;
        private System.Windows.Forms.Button bColorCopy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bItemIdCopy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bOwnedCopy;
        private System.Windows.Forms.Button bLayerCopy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bAmountCopy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button bRContainerCopy;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button bContainerCopy;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox1;
        private RazorButton razorButton1;
        private System.Windows.Forms.Button bSerialCopy;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lSerial;
        private System.Windows.Forms.Label lItemID;
        private System.Windows.Forms.Label lColor;
        private System.Windows.Forms.Label lPosition;
        private System.Windows.Forms.Label lContainer;
        private System.Windows.Forms.Label lOwned;
        private System.Windows.Forms.Label lLayer;
        private System.Windows.Forms.Label lAmount;
        private System.Windows.Forms.Label lRootContainer;


    }
}