using System;
using System.Globalization;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
    public class ScriptListView : ListView
    {
        public ScriptListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014)
            {
                m.Msg = (int)IntPtr.Zero;
            }
            base.WndProc(ref m);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScriptListView
            // 
            this.Font = new System.Drawing.Font("Arial", 8F);
            this.ResumeLayout(false);

        }
    }

    public class RazorHotKeyTextBox : TextBox
    {
        public RazorHotKeyTextBox()
        {

        }
        internal Keys LastKey = Keys.None;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (LastKey != keyData)
                RazorEnhanced.HotKey.KeyDown(keyData);
            LastKey = keyData;
            return true;
        }
    }
    public class RazorAgentNumOnlyTextBox : TextBox
    {
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo fi = CultureInfo.CurrentCulture.NumberFormat;

            string c = e.KeyChar.ToString();
            if (char.IsDigit(c, 0))
            {
                return;
            }

            // copy/paste
            if (((e.KeyChar == 22) || (e.KeyChar == 3))
                && ((ModifierKeys & Keys.Control) == Keys.Control))
                return;

            if (e.KeyChar == '\b')
                return;

            e.Handled = true;
        }
        public RazorAgentNumOnlyTextBox()
        { }
    }

    public class RazorAgentNumHexTextBox : TextBox
    {
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo fi = CultureInfo.CurrentCulture.NumberFormat;

            string c = e.KeyChar.ToString();
            if (char.IsDigit(c, 0))
            {
                return;
            }

            switch (c)
            {
                case "x":
                    return;
                case "X":
                    {
                        e.KeyChar = 'x';
                        return;
                    }
                case "A":
                    return;
                case "a":
                    {
                        e.KeyChar = 'A';
                        return;
                    }
                case "B":
                    return;
                case "b":
                    {
                        e.KeyChar = 'B';
                        return;
                    }
                case "C":
                    return;
                case "c":
                    {
                        e.KeyChar = 'C';
                        return;
                    }
                case "D":
                    return;
                case "d":
                    {
                        e.KeyChar = 'D';
                        return;
                    }
                case "E":
                    return;
                case "e":
                    {
                        e.KeyChar = 'E';
                        return;
                    }
                case "F":
                    return;
                case "f":
                    {
                        e.KeyChar = 'F';
                        return;
                    }
            }

            // copy/paste
            if (((e.KeyChar == 22) || (e.KeyChar == 3))
                && ((ModifierKeys & Keys.Control) == Keys.Control))
                return;

            if (e.KeyChar == '\b')
                return;

            e.Handled = true;
        }

        public RazorAgentNumHexTextBox()
        { }
    }
}