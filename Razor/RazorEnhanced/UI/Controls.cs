using System;
using System.ComponentModel;
using System.Drawing;
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
	}

	public class RazorRadioButton : RadioButton
	{
		public RazorRadioButton()
		{
		}
	}

	public class RazorTextBox : TextBox
	{
		public RazorTextBox()
		{
		
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
			if ((((int)e.KeyChar == 22) || ((int)e.KeyChar == 3))
				&& ((ModifierKeys & Keys.Control) == Keys.Control))
				return;

			if (e.KeyChar == '\b')
				return;

			e.Handled = true;
		}


		public RazorAgentNumOnlyTextBox()
		{


		}
	}

	public class RazorComboBox : ComboBox
	{
		public RazorComboBox()
		{
		}	
	}

	public class RazorCheckBox : CheckBox
	{
		public RazorCheckBox()
		{
		}
	}

	public partial class RazorButton : Button
	{

		public RazorButton()
		{
		}
	}
}