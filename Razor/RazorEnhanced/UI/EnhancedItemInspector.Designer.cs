using IronPython.Modules;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedItemInspector));
            this.bNameCopy = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imagepanel = new System.Windows.Forms.Panel();
            this.lName = new System.Windows.Forms.TextBox();
            this.lSerial = new System.Windows.Forms.TextBox();
            this.lItemID = new System.Windows.Forms.TextBox();
            this.lColor = new System.Windows.Forms.TextBox();
            this.lPosition = new System.Windows.Forms.TextBox();
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
            this.lContainer = new System.Windows.Forms.TextBox();
            this.lOwned = new System.Windows.Forms.TextBox();
            this.lLayer = new System.Windows.Forms.TextBox();
            this.lAmount = new System.Windows.Forms.TextBox();
            this.lRootContainer = new System.Windows.Forms.TextBox();
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
            this.listBoxAttributes = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.visibleflaglabel = new System.Windows.Forms.Label();
            this.groudflaglabel = new System.Windows.Forms.Label();
            this.twohandflaglabel = new System.Windows.Forms.Label();
            this.containerflaglabel = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.movableflaglabel = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.potionflaglabel = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.multiflaglabel = new System.Windows.Forms.Label();
            this.doorflaglabel = new System.Windows.Forms.Label();
            this.corpseflaglabel = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.razorButton1 = new RazorEnhanced.UI.RazorButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // bNameCopy
            // 
            this.bNameCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bNameCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bNameCopy.FlatAppearance.BorderSize = 0;
            this.bNameCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNameCopy.Location = new System.Drawing.Point(10, 32);
            this.bNameCopy.Name = "bNameCopy";
            this.bNameCopy.Size = new System.Drawing.Size(20, 20);
            this.bNameCopy.TabIndex = 0;
            this.bNameCopy.UseVisualStyleBackColor = true;
            this.bNameCopy.Click += new System.EventHandler(this.bNameCopy_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imagepanel);
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
            // imagepanel
            // 
            this.imagepanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imagepanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imagepanel.Location = new System.Drawing.Point(178, 55);
            this.imagepanel.Name = "imagepanel";
            this.imagepanel.Size = new System.Drawing.Size(132, 100);
            this.imagepanel.TabIndex = 18;
            // 
            // lName
            // 
            this.lName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lName.Location = new System.Drawing.Point(97, 36);
            this.lName.Name = "lName";
            this.lName.ReadOnly = true;
            this.lName.Size = new System.Drawing.Size(213, 13);
            this.lName.TabIndex = 17;
            this.lName.Text = "null";
            // 
            // lSerial
            // 
            this.lSerial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lSerial.Location = new System.Drawing.Point(97, 64);
            this.lSerial.Name = "lSerial";
            this.lSerial.ReadOnly = true;
            this.lSerial.Size = new System.Drawing.Size(66, 13);
            this.lSerial.TabIndex = 16;
            this.lSerial.Text = "0x00000000";
            // 
            // lItemID
            // 
            this.lItemID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lItemID.Location = new System.Drawing.Point(97, 96);
            this.lItemID.Name = "lItemID";
            this.lItemID.ReadOnly = true;
            this.lItemID.Size = new System.Drawing.Size(42, 13);
            this.lItemID.TabIndex = 15;
            this.lItemID.Text = "0x0000";
            // 
            // lColor
            // 
            this.lColor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lColor.Location = new System.Drawing.Point(97, 124);
            this.lColor.Name = "lColor";
            this.lColor.ReadOnly = true;
            this.lColor.Size = new System.Drawing.Size(42, 13);
            this.lColor.TabIndex = 14;
            this.lColor.Text = "0x0000";
            // 
            // lPosition
            // 
            this.lPosition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lPosition.Location = new System.Drawing.Point(97, 154);
            this.lPosition.Name = "lPosition";
            this.lPosition.ReadOnly = true;
            this.lPosition.Size = new System.Drawing.Size(102, 13);
            this.lPosition.TabIndex = 13;
            this.lPosition.Text = "0, 0, 0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Position:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Color:";
            // 
            // bPositionCopy
            // 
            this.bPositionCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bPositionCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bPositionCopy.FlatAppearance.BorderSize = 0;
            this.bPositionCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPositionCopy.Location = new System.Drawing.Point(10, 152);
            this.bPositionCopy.Name = "bPositionCopy";
            this.bPositionCopy.Size = new System.Drawing.Size(20, 20);
            this.bPositionCopy.TabIndex = 6;
            this.bPositionCopy.UseVisualStyleBackColor = true;
            this.bPositionCopy.Click += new System.EventHandler(this.bPositionCopy_Click);
            // 
            // bColorCopy
            // 
            this.bColorCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bColorCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bColorCopy.FlatAppearance.BorderSize = 0;
            this.bColorCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bColorCopy.Location = new System.Drawing.Point(10, 122);
            this.bColorCopy.Name = "bColorCopy";
            this.bColorCopy.Size = new System.Drawing.Size(20, 20);
            this.bColorCopy.TabIndex = 5;
            this.bColorCopy.UseVisualStyleBackColor = true;
            this.bColorCopy.Click += new System.EventHandler(this.bColorCopy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "ItemID";
            // 
            // bItemIdCopy
            // 
            this.bItemIdCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bItemIdCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bItemIdCopy.FlatAppearance.BorderSize = 0;
            this.bItemIdCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bItemIdCopy.Location = new System.Drawing.Point(10, 92);
            this.bItemIdCopy.Name = "bItemIdCopy";
            this.bItemIdCopy.Size = new System.Drawing.Size(20, 20);
            this.bItemIdCopy.TabIndex = 4;
            this.bItemIdCopy.UseVisualStyleBackColor = true;
            this.bItemIdCopy.Click += new System.EventHandler(this.bItemIdCopy_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Serial:";
            // 
            // bSerialCopy
            // 
            this.bSerialCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bSerialCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bSerialCopy.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.bSerialCopy.FlatAppearance.BorderSize = 0;
            this.bSerialCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSerialCopy.Location = new System.Drawing.Point(10, 62);
            this.bSerialCopy.Name = "bSerialCopy";
            this.bSerialCopy.Size = new System.Drawing.Size(20, 20);
            this.bSerialCopy.TabIndex = 2;
            this.bSerialCopy.UseVisualStyleBackColor = true;
            this.bSerialCopy.Click += new System.EventHandler(this.bSerialCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
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
            this.lContainer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lContainer.Location = new System.Drawing.Point(147, 34);
            this.lContainer.Name = "lContainer";
            this.lContainer.ReadOnly = true;
            this.lContainer.Size = new System.Drawing.Size(66, 13);
            this.lContainer.TabIndex = 18;
            this.lContainer.Text = "0x00000000";
            // 
            // lOwned
            // 
            this.lOwned.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lOwned.Location = new System.Drawing.Point(147, 154);
            this.lOwned.Name = "lOwned";
            this.lOwned.ReadOnly = true;
            this.lOwned.Size = new System.Drawing.Size(21, 13);
            this.lOwned.TabIndex = 12;
            this.lOwned.Text = "No";
            // 
            // lLayer
            // 
            this.lLayer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lLayer.Location = new System.Drawing.Point(147, 126);
            this.lLayer.Name = "lLayer";
            this.lLayer.ReadOnly = true;
            this.lLayer.Size = new System.Drawing.Size(79, 13);
            this.lLayer.TabIndex = 11;
            this.lLayer.Text = "0";
            // 
            // lAmount
            // 
            this.lAmount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lAmount.Location = new System.Drawing.Point(147, 94);
            this.lAmount.Name = "lAmount";
            this.lAmount.ReadOnly = true;
            this.lAmount.Size = new System.Drawing.Size(13, 13);
            this.lAmount.TabIndex = 10;
            this.lAmount.Text = "0";
            // 
            // lRootContainer
            // 
            this.lRootContainer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lRootContainer.Location = new System.Drawing.Point(147, 64);
            this.lRootContainer.Name = "lRootContainer";
            this.lRootContainer.ReadOnly = true;
            this.lRootContainer.Size = new System.Drawing.Size(66, 13);
            this.lRootContainer.TabIndex = 9;
            this.lRootContainer.Text = "0x00000000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Owned:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Layer:";
            // 
            // bOwnedCopy
            // 
            this.bOwnedCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bOwnedCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bOwnedCopy.FlatAppearance.BorderSize = 0;
            this.bOwnedCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOwnedCopy.Location = new System.Drawing.Point(10, 152);
            this.bOwnedCopy.Name = "bOwnedCopy";
            this.bOwnedCopy.Size = new System.Drawing.Size(20, 20);
            this.bOwnedCopy.TabIndex = 6;
            this.bOwnedCopy.UseVisualStyleBackColor = true;
            this.bOwnedCopy.Click += new System.EventHandler(this.bOwnedCopy_Click);
            // 
            // bLayerCopy
            // 
            this.bLayerCopy.BackColor = System.Drawing.SystemColors.Control;
            this.bLayerCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bLayerCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bLayerCopy.FlatAppearance.BorderSize = 0;
            this.bLayerCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLayerCopy.Location = new System.Drawing.Point(10, 122);
            this.bLayerCopy.Name = "bLayerCopy";
            this.bLayerCopy.Size = new System.Drawing.Size(20, 20);
            this.bLayerCopy.TabIndex = 5;
            this.bLayerCopy.UseVisualStyleBackColor = false;
            this.bLayerCopy.Click += new System.EventHandler(this.bLayerCopy_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Amount:";
            // 
            // bAmountCopy
            // 
            this.bAmountCopy.BackColor = System.Drawing.SystemColors.Control;
            this.bAmountCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bAmountCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bAmountCopy.FlatAppearance.BorderSize = 0;
            this.bAmountCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAmountCopy.Location = new System.Drawing.Point(10, 92);
            this.bAmountCopy.Name = "bAmountCopy";
            this.bAmountCopy.Size = new System.Drawing.Size(20, 20);
            this.bAmountCopy.TabIndex = 4;
            this.bAmountCopy.UseVisualStyleBackColor = false;
            this.bAmountCopy.Click += new System.EventHandler(this.bAmountCopy_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(39, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Root Container:";
            // 
            // bRContainerCopy
            // 
            this.bRContainerCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bRContainerCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bRContainerCopy.FlatAppearance.BorderSize = 0;
            this.bRContainerCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRContainerCopy.Location = new System.Drawing.Point(10, 62);
            this.bRContainerCopy.Name = "bRContainerCopy";
            this.bRContainerCopy.Size = new System.Drawing.Size(20, 20);
            this.bRContainerCopy.TabIndex = 2;
            this.bRContainerCopy.UseVisualStyleBackColor = true;
            this.bRContainerCopy.Click += new System.EventHandler(this.bRContainerCopy_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(39, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Container:";
            // 
            // bContainerCopy
            // 
            this.bContainerCopy.BackgroundImage = global::Assistant.Properties.Resources.RoundButton;
            this.bContainerCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bContainerCopy.FlatAppearance.BorderSize = 0;
            this.bContainerCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bContainerCopy.Location = new System.Drawing.Point(10, 32);
            this.bContainerCopy.Name = "bContainerCopy";
            this.bContainerCopy.Size = new System.Drawing.Size(20, 20);
            this.bContainerCopy.TabIndex = 0;
            this.bContainerCopy.UseVisualStyleBackColor = true;
            this.bContainerCopy.Click += new System.EventHandler(this.bContainerCopy_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxAttributes);
            this.groupBox3.Location = new System.Drawing.Point(467, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 373);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Attributes";
            // 
            // listBoxAttributes
            // 
            this.listBoxAttributes.FormattingEnabled = true;
            this.listBoxAttributes.Location = new System.Drawing.Point(12, 19);
            this.listBoxAttributes.Name = "listBoxAttributes";
            this.listBoxAttributes.Size = new System.Drawing.Size(289, 329);
            this.listBoxAttributes.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.visibleflaglabel);
            this.groupBox4.Controls.Add(this.groudflaglabel);
            this.groupBox4.Controls.Add(this.twohandflaglabel);
            this.groupBox4.Controls.Add(this.containerflaglabel);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.movableflaglabel);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.potionflaglabel);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.multiflaglabel);
            this.groupBox4.Controls.Add(this.doorflaglabel);
            this.groupBox4.Controls.Add(this.corpseflaglabel);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(334, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(127, 302);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Flags";
            // 
            // visibleflaglabel
            // 
            this.visibleflaglabel.AutoSize = true;
            this.visibleflaglabel.Location = new System.Drawing.Point(87, 273);
            this.visibleflaglabel.Name = "visibleflaglabel";
            this.visibleflaglabel.Size = new System.Drawing.Size(13, 13);
            this.visibleflaglabel.TabIndex = 36;
            this.visibleflaglabel.Text = "0";
            // 
            // groudflaglabel
            // 
            this.groudflaglabel.AutoSize = true;
            this.groudflaglabel.Location = new System.Drawing.Point(87, 243);
            this.groudflaglabel.Name = "groudflaglabel";
            this.groudflaglabel.Size = new System.Drawing.Size(13, 13);
            this.groudflaglabel.TabIndex = 35;
            this.groudflaglabel.Text = "0";
            // 
            // twohandflaglabel
            // 
            this.twohandflaglabel.AutoSize = true;
            this.twohandflaglabel.Location = new System.Drawing.Point(87, 213);
            this.twohandflaglabel.Name = "twohandflaglabel";
            this.twohandflaglabel.Size = new System.Drawing.Size(13, 13);
            this.twohandflaglabel.TabIndex = 34;
            this.twohandflaglabel.Text = "0";
            // 
            // containerflaglabel
            // 
            this.containerflaglabel.AutoSize = true;
            this.containerflaglabel.Location = new System.Drawing.Point(87, 35);
            this.containerflaglabel.Name = "containerflaglabel";
            this.containerflaglabel.Size = new System.Drawing.Size(13, 13);
            this.containerflaglabel.TabIndex = 33;
            this.containerflaglabel.Text = "0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 273);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(40, 13);
            this.label19.TabIndex = 32;
            this.label19.Text = "Visible:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 243);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(45, 13);
            this.label18.TabIndex = 31;
            this.label18.Text = "Ground:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 213);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 30;
            this.label15.Text = "Two Hand:";
            // 
            // movableflaglabel
            // 
            this.movableflaglabel.AutoSize = true;
            this.movableflaglabel.Location = new System.Drawing.Point(87, 183);
            this.movableflaglabel.Name = "movableflaglabel";
            this.movableflaglabel.Size = new System.Drawing.Size(13, 13);
            this.movableflaglabel.TabIndex = 29;
            this.movableflaglabel.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 183);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 13);
            this.label17.TabIndex = 28;
            this.label17.Text = "Movable:";
            // 
            // potionflaglabel
            // 
            this.potionflaglabel.AutoSize = true;
            this.potionflaglabel.Location = new System.Drawing.Point(87, 153);
            this.potionflaglabel.Name = "potionflaglabel";
            this.potionflaglabel.Size = new System.Drawing.Size(13, 13);
            this.potionflaglabel.TabIndex = 27;
            this.potionflaglabel.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 153);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "Potion:";
            // 
            // multiflaglabel
            // 
            this.multiflaglabel.AutoSize = true;
            this.multiflaglabel.Location = new System.Drawing.Point(87, 123);
            this.multiflaglabel.Name = "multiflaglabel";
            this.multiflaglabel.Size = new System.Drawing.Size(13, 13);
            this.multiflaglabel.TabIndex = 25;
            this.multiflaglabel.Text = "0";
            // 
            // doorflaglabel
            // 
            this.doorflaglabel.AutoSize = true;
            this.doorflaglabel.Location = new System.Drawing.Point(87, 93);
            this.doorflaglabel.Name = "doorflaglabel";
            this.doorflaglabel.Size = new System.Drawing.Size(13, 13);
            this.doorflaglabel.TabIndex = 24;
            this.doorflaglabel.Text = "0";
            // 
            // corpseflaglabel
            // 
            this.corpseflaglabel.AutoSize = true;
            this.corpseflaglabel.Location = new System.Drawing.Point(87, 63);
            this.corpseflaglabel.Name = "corpseflaglabel";
            this.corpseflaglabel.Size = new System.Drawing.Size(13, 13);
            this.corpseflaglabel.TabIndex = 23;
            this.corpseflaglabel.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 123);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(32, 13);
            this.label14.TabIndex = 21;
            this.label14.Text = "Multi:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "Door:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Corpse:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Container:";
            // 
            // razorButton1
            // 
            this.razorButton1.Location = new System.Drawing.Point(361, 397);
            this.razorButton1.Name = "razorButton1";
            this.razorButton1.Size = new System.Drawing.Size(75, 23);
            this.razorButton1.TabIndex = 10;
            this.razorButton1.Text = "Close";
            this.razorButton1.UseVisualStyleBackColor = true;
            this.razorButton1.Click += new System.EventHandler(this.razorButton1_Click);
            // 
            // EnhancedItemInspector
            // 
            this.ClientSize = new System.Drawing.Size(798, 423);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.razorButton1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnhancedItemInspector";
            this.Text = "Enhanced Item Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnhancedItemInspector_FormClosing);
            this.Load += new System.EventHandler(this.EnhancedItemInspector_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
		private System.Windows.Forms.ListBox listBoxAttributes;
		private RazorButton razorButton1;
		private System.Windows.Forms.Button bSerialCopy;
		private System.Windows.Forms.TextBox lName;
		private System.Windows.Forms.TextBox lSerial;
		private System.Windows.Forms.TextBox lItemID;
		private System.Windows.Forms.TextBox lColor;
		private System.Windows.Forms.TextBox lPosition;
		private System.Windows.Forms.TextBox lContainer;
		private System.Windows.Forms.TextBox lOwned;
		private System.Windows.Forms.TextBox lLayer;
		private System.Windows.Forms.TextBox lAmount;
		private System.Windows.Forms.TextBox lRootContainer;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label movableflaglabel;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label potionflaglabel;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label multiflaglabel;
		private System.Windows.Forms.Label doorflaglabel;
		private System.Windows.Forms.Label corpseflaglabel;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label containerflaglabel;
		private System.Windows.Forms.Label visibleflaglabel;
		private System.Windows.Forms.Label groudflaglabel;
		private System.Windows.Forms.Label twohandflaglabel;
		private System.Windows.Forms.Panel imagepanel;
    }
}