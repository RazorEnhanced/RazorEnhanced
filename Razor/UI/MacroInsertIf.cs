using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Assistant.Macros;
using RazorEnhanced.UI;

namespace Assistant
{
	/// <summary>
	/// Summary description for MacroInsertIf.
	/// </summary>
	public class MacroInsertIf : System.Windows.Forms.Form
	{
		private Macro m_Macro;
		private int m_Idx;
		private MacroAction m_Action;

        private RazorButton insert;
        private RazorTextBox txtAmount;
        private RazorButton cancel;
        private RazorComboBox varList;
        private RazorComboBox opList;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MacroInsertIf(Macro m, int idx)
		{
			m_Macro = m;
			m_Idx = idx;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			foreach (Counter c in Counter.List)
				varList.Items.Add(c.Name);
		}

		public MacroInsertIf(MacroAction a)
		{
			m_Action = a;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			foreach (Counter c in Counter.List)
				varList.Items.Add(c.Name);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            RazorEnhanced.UI.Office2010BlueTheme office2010Blue = new RazorEnhanced.UI.Office2010BlueTheme();
            this.insert = new RazorEnhanced.UI.RazorButton();
            this.txtAmount = new RazorEnhanced.UI.RazorTextBox();
            this.varList = new RazorEnhanced.UI.RazorComboBox();
            this.cancel = new RazorEnhanced.UI.RazorButton();
            this.opList = new RazorEnhanced.UI.RazorComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // insert
            // 
            office2010Blue.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Blue.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Blue.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            office2010Blue.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            office2010Blue.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Blue.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Blue.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue.TextColor = System.Drawing.Color.White;
            this.insert.ColorTable = office2010Blue;
            this.insert.Location = new System.Drawing.Point(38, 42);
            this.insert.Name = "insert";
            this.insert.Size = new System.Drawing.Size(92, 27);
            this.insert.TabIndex = 0;
            this.insert.Text = "&Insert";
            this.insert.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.insert.Click += new System.EventHandler(this.insert_Click);
            // 
            // txtAmount
            // 
            this.txtAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.txtAmount.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.txtAmount.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.txtAmount.Location = new System.Drawing.Point(181, 10);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Padding = new System.Windows.Forms.Padding(1);
            this.txtAmount.Size = new System.Drawing.Size(96, 22);
            this.txtAmount.TabIndex = 7;
            // 
            // varList
            // 
            this.varList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.varList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.varList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.varList.Items.AddRange(new object[] {
            "Hits",
            "Mana",
            "Stamina",
            "Poisoned",
            "SysMessage",
            "Weight",
            "Mounted",
            "R Hand Empty",
            "L Hand Empty"});
            this.varList.Location = new System.Drawing.Point(29, 9);
            this.varList.Name = "varList";
            this.varList.Size = new System.Drawing.Size(96, 25);
            this.varList.TabIndex = 8;
            this.varList.SelectedIndexChanged += new System.EventHandler(this.varList_SelectedIndexChanged);
            // 
            // cancel
            // 
            this.cancel.ColorTable = office2010Blue;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(151, 42);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(91, 27);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "&Cancel";
            this.cancel.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // opList
            // 
            this.opList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.opList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.opList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.opList.Items.AddRange(new object[] {
            "<=",
            ">="});
            this.opList.Location = new System.Drawing.Point(127, 9);
            this.opList.Name = "opList";
            this.opList.Size = new System.Drawing.Size(48, 25);
            this.opList.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "If:";
            // 
            // MacroInsertIf
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(284, 77);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.opList);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.varList);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.insert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MacroInsertIf";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert If...";
            this.Load += new System.EventHandler(this.MacroInsertIf_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void insert_Click(object sender, System.EventArgs e)
		{
			MacroAction a = null;

			try
			{
				if (varList.SelectedIndex == (int)IfAction.IfVarType.SysMessage)
					a = new IfAction((IfAction.IfVarType)varList.SelectedIndex, txtAmount.Text);
				else if (varList.SelectedIndex >= (int)IfAction.IfVarType.BeginCountersMarker)
					a = new IfAction(IfAction.IfVarType.Counter, (sbyte)opList.SelectedIndex, Utility.ToInt32(txtAmount.Text, 0), varList.SelectedItem as string);
				else
					a = new IfAction((IfAction.IfVarType)varList.SelectedIndex, (sbyte)opList.SelectedIndex, Utility.ToInt32(txtAmount.Text, 0));
			}
			catch
			{
				return;
			}

			if (m_Action == null)
				m_Macro.Insert(m_Idx + 1, a);
			else
				m_Action.Parent.Convert(m_Action, a);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void MacroInsertIf_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames(this);

			if (m_Action is IfAction)
			{
				try { varList.SelectedIndex = (int)((IfAction)m_Action).Variable; }
				catch { }
				try { opList.SelectedIndex = (int)((IfAction)m_Action).Op; }
				catch { }
				try
				{
					if (varList.SelectedIndex != 3 && (varList.SelectedIndex <= 5 || varList.SelectedIndex >= (int)IfAction.IfVarType.BeginCountersMarker))
						txtAmount.Text = ((IfAction)m_Action).Value.ToString();
				}
				catch
				{
				}

				if (((IfAction)m_Action).Counter != null && ((IfAction)m_Action).Variable == IfAction.IfVarType.Counter)
					try { varList.SelectedItem = ((IfAction)m_Action).Counter; }
					catch { }
			}
		}

		private void varList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				opList.Visible = varList.SelectedIndex < 3 || varList.SelectedIndex == 5 || varList.SelectedIndex >= (int)IfAction.IfVarType.BeginCountersMarker;
				txtAmount.Visible = varList.SelectedIndex != 3 && (varList.SelectedIndex <= 5 || varList.SelectedIndex >= (int)IfAction.IfVarType.BeginCountersMarker);
			}
			catch
			{
			}

			if (!opList.Visible)
			{
				if (txtAmount.Visible)
				{
					varList.Size = new System.Drawing.Size(80, 21);

					txtAmount.Location = new System.Drawing.Point(104, 9);
					txtAmount.Size = new System.Drawing.Size(120, 20);
				}
				else
				{
					varList.Size = new System.Drawing.Size(200, 21);
				}
			}
			else
			{
				varList.Size = new System.Drawing.Size(80, 21);

				txtAmount.Location = new System.Drawing.Point(144, 9);
				txtAmount.Size = new System.Drawing.Size(80, 20);
			}
		}
	}
}
