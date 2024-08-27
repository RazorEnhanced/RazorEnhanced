namespace RazorEnhanced.UI
{
    partial class EnhancedDressAddUndressLayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedDressAddUndressLayer));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.layerlist = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.bAddItem = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.layerlist);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(221, 69);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layer";
            // 
            // layerlist
            // 
            this.layerlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.layerlist.FormattingEnabled = true;
            this.layerlist.Location = new System.Drawing.Point(51, 19);
            this.layerlist.Name = "layerlist";
            this.layerlist.Size = new System.Drawing.Size(151, 22);
            this.layerlist.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Layer: ";
            // 
            // bClose
            // 
            this.bClose.Location = new System.Drawing.Point(246, 48);
            this.bClose.Margin = new System.Windows.Forms.Padding(2);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(57, 20);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bAddItem
            // 
            this.bAddItem.Location = new System.Drawing.Point(246, 24);
            this.bAddItem.Margin = new System.Windows.Forms.Padding(2);
            this.bAddItem.Name = "bAddItem";
            this.bAddItem.Size = new System.Drawing.Size(57, 20);
            this.bAddItem.TabIndex = 2;
            this.bAddItem.Text = "Add";
            this.bAddItem.UseVisualStyleBackColor = true;
            this.bAddItem.Click += new System.EventHandler(this.bAddItem_Click);
            // 
            // EnhancedDressAddUndressLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 88);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bAddItem);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnhancedDressAddUndressLayer";
            this.Text = "Enhanced Dress Add Clear Layer";
            this.Load += new System.EventHandler(this.EnhancedDressAddUndressLayer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bAddItem;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.ComboBox layerlist;
        private System.Windows.Forms.Label label1;

    }
}