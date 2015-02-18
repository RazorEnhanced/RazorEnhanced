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
	/// Summary description for MacroInsertWait.
	/// </summary>
	public class MacroInsertWait : System.Windows.Forms.Form
	{
		private Macro m_Macro;
		private int m_Idx;
		private MacroAction m_Action;

		private XButton insert;
		private XRadioButton radioPause;
		private XTextBox pause;
		private System.Windows.Forms.Label label1;
		private XRadioButton radioGump;
		private XRadioButton radioStat;
		private XTextBox statAmount;
		private XButton cancel;
		private XRadioButton radioTarg;
		private XComboBox statList;
		private XComboBox statOpList;
		private XRadioButton radioMenu;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MacroInsertWait(Macro m, int idx)
		{
			m_Macro = m;
			m_Idx = idx;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public MacroInsertWait(MacroAction a)
		{
			m_Action = a;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            RazorEnhanced.UI.Office2010Blue office2010Blue = new RazorEnhanced.UI.Office2010Blue();
            this.insert = new RazorEnhanced.UI.XButton();
            this.radioPause = new RazorEnhanced.UI.XRadioButton();
            this.pause = new RazorEnhanced.UI.XTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioGump = new RazorEnhanced.UI.XRadioButton();
            this.radioTarg = new RazorEnhanced.UI.XRadioButton();
            this.radioStat = new RazorEnhanced.UI.XRadioButton();
            this.statAmount = new RazorEnhanced.UI.XTextBox();
            this.statList = new RazorEnhanced.UI.XComboBox();
            this.cancel = new RazorEnhanced.UI.XButton();
            this.statOpList = new RazorEnhanced.UI.XComboBox();
            this.radioMenu = new RazorEnhanced.UI.XRadioButton();
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
            this.insert.Location = new System.Drawing.Point(12, 134);
            this.insert.Name = "insert";
            this.insert.Size = new System.Drawing.Size(97, 28);
            this.insert.TabIndex = 0;
            this.insert.Text = "&Insert";
            this.insert.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.insert.Click += new System.EventHandler(this.insert_Click);
            // 
            // radioPause
            // 
            this.radioPause.Location = new System.Drawing.Point(10, 5);
            this.radioPause.Name = "radioPause";
            this.radioPause.Size = new System.Drawing.Size(91, 23);
            this.radioPause.TabIndex = 1;
            this.radioPause.Text = "Pause for:";
            this.radioPause.CheckedChanged += new System.EventHandler(this.radioPause_CheckedChanged);
            // 
            // pause
            // 
            this.pause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.pause.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.pause.Enabled = false;
            this.pause.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.pause.Location = new System.Drawing.Point(101, 5);
            this.pause.Name = "pause";
            this.pause.Padding = new System.Windows.Forms.Padding(1);
            this.pause.Size = new System.Drawing.Size(48, 22);
            this.pause.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(154, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "milliseconds";
            // 
            // radioGump
            // 
            this.radioGump.Location = new System.Drawing.Point(10, 28);
            this.radioGump.Name = "radioGump";
            this.radioGump.Size = new System.Drawing.Size(115, 23);
            this.radioGump.TabIndex = 4;
            this.radioGump.Text = "Wait for Gump";
            // 
            // radioTarg
            // 
            this.radioTarg.Location = new System.Drawing.Point(10, 74);
            this.radioTarg.Name = "radioTarg";
            this.radioTarg.Size = new System.Drawing.Size(120, 23);
            this.radioTarg.TabIndex = 5;
            this.radioTarg.Text = "Wait for Target";
            // 
            // radioStat
            // 
            this.radioStat.Location = new System.Drawing.Point(10, 97);
            this.radioStat.Name = "radioStat";
            this.radioStat.Size = new System.Drawing.Size(76, 23);
            this.radioStat.TabIndex = 6;
            this.radioStat.Text = "Wait for ";
            this.radioStat.CheckedChanged += new System.EventHandler(this.radioStat_CheckedChanged);
            // 
            // statAmount
            // 
            this.statAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.statAmount.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.statAmount.Enabled = false;
            this.statAmount.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.statAmount.Location = new System.Drawing.Point(221, 98);
            this.statAmount.Name = "statAmount";
            this.statAmount.Padding = new System.Windows.Forms.Padding(1);
            this.statAmount.Size = new System.Drawing.Size(48, 22);
            this.statAmount.TabIndex = 7;
            // 
            // statList
            // 
            this.statList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.statList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statList.Enabled = false;
            this.statList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.statList.Items.AddRange(new object[] {
            "Hits",
            "Mana",
            "Stamina"});
            this.statList.Location = new System.Drawing.Point(86, 97);
            this.statList.Name = "statList";
            this.statList.Size = new System.Drawing.Size(77, 25);
            this.statList.TabIndex = 8;
            // 
            // cancel
            // 
            this.cancel.ColorTable = office2010Blue;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(171, 134);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(97, 28);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "&Cancel";
            this.cancel.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // statOpList
            // 
            this.statOpList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.statOpList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statOpList.Enabled = false;
            this.statOpList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.statOpList.Items.AddRange(new object[] {
            "<=",
            ">="});
            this.statOpList.Location = new System.Drawing.Point(165, 97);
            this.statOpList.Name = "statOpList";
            this.statOpList.Size = new System.Drawing.Size(53, 25);
            this.statOpList.TabIndex = 11;
            // 
            // radioMenu
            // 
            this.radioMenu.Location = new System.Drawing.Point(10, 51);
            this.radioMenu.Name = "radioMenu";
            this.radioMenu.Size = new System.Drawing.Size(211, 23);
            this.radioMenu.TabIndex = 12;
            this.radioMenu.Text = "Wait for old-style Menu/Dialog";
            // 
            // MacroInsertWait
            // 
            this.AcceptButton = this.insert;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(276, 180);
            this.ControlBox = false;
            this.Controls.Add(this.radioMenu);
            this.Controls.Add(this.statOpList);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.statList);
            this.Controls.Add(this.statAmount);
            this.Controls.Add(this.radioStat);
            this.Controls.Add(this.radioTarg);
            this.Controls.Add(this.radioGump);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.radioPause);
            this.Controls.Add(this.insert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MacroInsertWait";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert Wait/Pause...";
            this.Load += new System.EventHandler(this.MacroInsertWait_Load);
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

			if (radioPause.Checked)
				a = new PauseAction(TimeSpan.FromMilliseconds(Utility.ToInt32(pause.Text, 1000)));
			else if (radioGump.Checked)
				a = new WaitForGumpAction(0);
			else if (radioMenu.Checked)
				a = new WaitForMenuAction(0);
			else if (radioTarg.Checked)
				a = new WaitForTargetAction();
			else if (radioStat.Checked && statList.SelectedIndex >= 0 && statList.SelectedIndex < 3 && statOpList.SelectedIndex >= 0 && statOpList.SelectedIndex < statOpList.Items.Count)
				a = new WaitForStatAction((IfAction.IfVarType)statList.SelectedIndex, (byte)statOpList.SelectedIndex, Utility.ToInt32(statAmount.Text, 0));

			if (a != null)
			{
				if (m_Action == null)
					m_Macro.Insert(m_Idx + 1, a);
				else
					m_Action.Parent.Convert(m_Action, a);
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void radioStat_CheckedChanged(object sender, System.EventArgs e)
		{
			statList.Enabled = statAmount.Enabled = statOpList.Enabled = radioStat.Checked;
		}

		private void radioPause_CheckedChanged(object sender, System.EventArgs e)
		{
			pause.Enabled = radioPause.Checked;
		}

		private void MacroInsertWait_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames(this);

			radioPause.Checked = m_Action is PauseAction;
			radioGump.Checked = m_Action is WaitForGumpAction;
			radioMenu.Checked = m_Action is WaitForMenuAction;
			radioTarg.Checked = m_Action is WaitForTargetAction;
			radioStat.Checked = m_Action is WaitForStatAction;

			if (radioPause.Checked)
			{
				pause.Text = ((int)((PauseAction)m_Action).Timeout.TotalMilliseconds).ToString();
			}
			else if (radioStat.Checked)
			{
				statList.SelectedIndex = (int)((WaitForStatAction)m_Action).Stat;
				statOpList.SelectedIndex = (int)((WaitForStatAction)m_Action).Op;
				statAmount.Text = ((WaitForStatAction)m_Action).Amount.ToString();
			}
		}
	}
}
