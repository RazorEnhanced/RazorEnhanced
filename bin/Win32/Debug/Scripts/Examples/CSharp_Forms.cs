//C#
using System;
using System.Windows.Forms;
using System.Drawing;


//using Assistant;
using System.Runtime.InteropServices;
using System.ComponentModel;



namespace RazorEnhanced
{
    public class Script
    {
        private Form form;
        private Button button1;

        private void Log(object messageString)
        {
            Misc.SendMessage(messageString, 201);
        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            this.form.Dispose();
            this.form = null;
        }

        private void OnResizeForm(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Misc.SendMessage("Hello, my name is " + Player.Name);
        }

        private void CreateForm()
        {
            this.form = new Form();
            this.form.Text = "Forms 101 - " + Player.Name;
            this.form.HelpButton = false;
            this.form.MinimizeBox = true;
            this.form.MaximizeBox = false;
            this.form.Width = 750;
            this.form.Height = 750;
            //this.form.BackColor = Color.Black;
            this.form.FormBorderStyle = FormBorderStyle.Sizable;
            this.form.StartPosition = FormStartPosition.CenterScreen;
            this.form.Opacity = 100;
            this.form.FormClosed += new FormClosedEventHandler(this.OnFormClosed);
            this.form.Resize += new EventHandler(this.OnResizeForm);
            this.form.TopMost = true;
            this.form.ActiveControl = null;
            this.form.Visible = true;
            button1 = new Button();
            button1.Size = new Size(80, 40);
            button1.Location = new Point(30, 30);
            button1.Text = "Click me";
            this.form.Controls.Add(button1);
            button1.Click += new EventHandler(button1_Click);
            
            this.form.Show();
        }

        public void Run()
        {
            CreateForm();
            while (form != null)
            {
                Application.DoEvents();
            }
        }
    }
}