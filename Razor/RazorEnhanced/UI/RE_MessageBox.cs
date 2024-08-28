using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
    // Implemented our own message box because the standard font was 
    // unchangable, and didnt work on linux
    internal partial class RE_MessageBox : Form
    {

        // remove close button
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x200;  // CS_NOCLOSE
                return cp;
            }
        }


        internal RE_MessageBox(string title, string msg, string link = null,
            string ok = "Ok", string no = "No", string cancel="Cancel",
            Color? backColor = null)
        {
            InitializeComponent();

            // disable min/max
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ControlBox = false;

            this.okButton.Text = ok;
            okButton.Click += new EventHandler(OkButton_Click);

            if (no == null)
                this.noButton.Visible = false;
            else
            {
                this.noButton.Visible = true;
                this.noButton.Text = no;
                noButton.Click += new EventHandler(NoButton_Click);
            }

            if (cancel == null)
            {  
                this.cancelButton.Visible = false;
            }
            else {
                this.cancelButton.Visible = true;
                this.cancelButton.Text = cancel;
                cancelButton.Click += new EventHandler(CancelButton_Click);
            }

            if (backColor == null)
                this.BackColor = Color.WhiteSmoke;
            else
                this.BackColor = (Color)backColor;

            this.Text = title;
            message.Text = msg;
            using (Graphics g = this.CreateGraphics())
            {
                // Measure the size of the text in the label
                SizeF textSize = g.MeasureString(message.Text, message.Font);

                // Calculate the new width for the form, considering padding and border
                int newWidth = Math.Max((int)textSize.Width + message.Left + 20, 367); // Add some extra space                                                                        
                int newHeight = Math.Max((int)textSize.Height + message.Left + 120, 157); // Add some extra space


                // Set the label's width to match the text size
                message.Width = (int)textSize.Width;

                // Adjust the form's width to fit the label
                this.Width = newWidth;
                this.Height = newHeight;
            }



            if (link == null)
            {
                webLink.Visible = false;
            }
            else
            {
                webLink.Visible = true;
                webLink.Links.Add(0, webLink.Text.Length, webLink.Text);
                webLink.LinkClicked += new LinkLabelLinkClickedEventHandler(showLink);
            }
        }

        public static DialogResult Show(string title, string message, string link=null,
            string ok = "Ok", string no = "", string cancel = "Cancel",
            Color? backColor = null)
        {
            using (var form = new RE_MessageBox(title, message, link, ok, no, cancel, backColor))
            {                
                return form.ShowDialog();
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (okButton.Text == "Yes")
                this.DialogResult = DialogResult.Yes;
            else
                this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void NoButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void showLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the URL in the default web browser
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Link.LinkData.ToString(),
                UseShellExecute = true
            });
        }

    }
}

