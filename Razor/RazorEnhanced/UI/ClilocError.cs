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
    internal partial class ClilocError : Form
    {
        static bool ShownOnce = false;
        internal ClilocError(string msg, string link)
        {
            InitializeComponent();
            message.Text = msg;
            discordFAQLink.Links.Add(0, discordFAQLink.Text.Length, discordFAQLink.Text);
            discordFAQLink.LinkClicked += new LinkLabelLinkClickedEventHandler(showLink);
            okButton.Click += new EventHandler(OkButton_Click);
        }
        public static DialogResult Show(string message, string link)
        {
            if (ShownOnce)
                return DialogResult.OK;
            using (var form = new ClilocError(message, link))
            {
                ShownOnce = true;
                return form.ShowDialog();
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
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

