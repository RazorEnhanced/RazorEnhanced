namespace RazorEnhanced.UI
{
    partial class ClilocError
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
            this.okButton = new System.Windows.Forms.Button();
            this.discordFAQLink = new System.Windows.Forms.LinkLabel();
            this.message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(12, 79);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 25);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // discordFAQLink
            // 
            this.discordFAQLink.AutoSize = true;
            this.discordFAQLink.Location = new System.Drawing.Point(12, 43);
            this.discordFAQLink.Name = "discordFAQLink";
            this.discordFAQLink.Size = new System.Drawing.Size(85, 14);
            this.discordFAQLink.TabIndex = 1;
            this.discordFAQLink.TabStop = true;
            this.discordFAQLink.Text = "discord FAQ link";
            this.discordFAQLink.Visible = false;
            // 
            // message
            // 
            this.message.AutoSize = true;
            this.message.Location = new System.Drawing.Point(12, 10);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(78, 14);
            this.message.TabIndex = 2;
            this.message.Text = "cliloc message";
            // 
            // ClilocError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(351, 117);
            this.Controls.Add(this.message);
            this.Controls.Add(this.discordFAQLink);
            this.Controls.Add(this.okButton);
            this.Font = new System.Drawing.Font("Arial", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClilocError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClilocError";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.LinkLabel discordFAQLink;
        private System.Windows.Forms.Label message;
    }
}