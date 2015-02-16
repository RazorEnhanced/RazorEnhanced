using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using RazorEnhanced.UI;

namespace Assistant
{
	/// <summary>
	/// Summary description for HueEntry.
	/// </summary>
	public class HueEntry : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private XTextBox hueNum;
		private XButton inGame;
		private System.Windows.Forms.Label preview;
		private XButton okay;
		private XButton cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private int m_Hue;

		public delegate void HueEntryCallback(int hue);
		public static HueEntryCallback Callback = null;

		public int Hue { get { return m_Hue; } }

		public HueEntry()
			: this(0)
		{
		}

		public HueEntry(int hue)
		{
			m_Hue = hue;
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
			Colortable colortable1 = new Colortable();
			Colortable colortable2 = new Colortable();
			Colortable colortable3 = new Colortable();
			this.label1 = new System.Windows.Forms.Label();
			this.hueNum = new XTextBox();
			this.inGame = new XButton();
			this.preview = new System.Windows.Forms.Label();
			this.okay = new XButton();
			this.cancel = new XButton();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "Hue Number:";
			// 
			// hueNum
			// 
			this.hueNum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.hueNum.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			this.hueNum.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			this.hueNum.Location = new System.Drawing.Point(91, 5);
			this.hueNum.Name = "hueNum";
			this.hueNum.Padding = new System.Windows.Forms.Padding(1);
			this.hueNum.Size = new System.Drawing.Size(60, 22);
			this.hueNum.TabIndex = 1;
			this.hueNum.TextChanged += new System.EventHandler(this.hueNum_TextChanged);
			// 
			// inGame
			// 
			colortable1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			colortable1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			colortable1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			colortable1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			colortable1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
			colortable1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
			colortable1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			colortable1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			colortable1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
			colortable1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
			colortable1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			colortable1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			colortable1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
			colortable1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
			colortable1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			colortable1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			colortable1.TextColor = System.Drawing.Color.White;
			this.inGame.ColorTable = colortable1;
			this.inGame.Location = new System.Drawing.Point(5, 32);
			this.inGame.Name = "inGame";
			this.inGame.Size = new System.Drawing.Size(149, 23);
			this.inGame.TabIndex = 2;
			this.inGame.Text = "Select in Game";
			this.inGame.Theme = Theme.MSOffice2010_BLUE;
			this.inGame.Click += new System.EventHandler(this.inGame_Click);
			// 
			// preview
			// 
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.preview.Location = new System.Drawing.Point(5, 60);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(149, 23);
			this.preview.TabIndex = 3;
			this.preview.Text = "Preview";
			this.preview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// okay
			// 
			colortable2.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			colortable2.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			colortable2.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			colortable2.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			colortable2.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
			colortable2.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
			colortable2.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			colortable2.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			colortable2.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
			colortable2.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
			colortable2.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			colortable2.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			colortable2.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
			colortable2.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
			colortable2.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			colortable2.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			colortable2.TextColor = System.Drawing.Color.White;
			this.okay.ColorTable = colortable2;
			this.okay.Location = new System.Drawing.Point(12, 92);
			this.okay.Name = "okay";
			this.okay.Size = new System.Drawing.Size(62, 23);
			this.okay.TabIndex = 4;
			this.okay.Text = "&Okay";
			this.okay.Theme = Theme.MSOffice2010_BLUE;
			this.okay.Click += new System.EventHandler(this.okay_Click);
			// 
			// cancel
			// 
			colortable3.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			colortable3.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			colortable3.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			colortable3.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			colortable3.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
			colortable3.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
			colortable3.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
			colortable3.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
			colortable3.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
			colortable3.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
			colortable3.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
			colortable3.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
			colortable3.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
			colortable3.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
			colortable3.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			colortable3.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
			colortable3.TextColor = System.Drawing.Color.White;
			this.cancel.ColorTable = colortable3;
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(84, 92);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(62, 23);
			this.cancel.TabIndex = 5;
			this.cancel.Text = "Cancel";
			this.cancel.Theme = Theme.MSOffice2010_BLUE;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// HueEntry
			// 
			this.AcceptButton = this.okay;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(161, 122);
			this.ControlBox = false;
			this.Controls.Add(this.hueNum);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.okay);
			this.Controls.Add(this.preview);
			this.Controls.Add(this.inGame);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HueEntry";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Hue";
			this.Load += new System.EventHandler(this.HueEntry_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void hueNum_TextChanged(object sender, System.EventArgs e)
		{
			SetPreview(Utility.ToInt32(hueNum.Text, 0) & 0x3FFF);
		}

		public const int TextHueIDX = 30;
		private void SetPreview(int hue)
		{
			if (hue > 0 && hue < 3000)
				preview.BackColor = Ultima.Hues.GetHue(hue - 1).GetColor(TextHueIDX);
			else
				preview.BackColor = Color.Black;
			preview.ForeColor = (preview.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);
		}

		private void HueResp(int hue)
		{
			hue &= 0x3FFF;
			SetPreview(hue);
			hueNum.Text = hue.ToString();
			Callback = null;

			//Engine.MainWindow.ShowMe();
			this.Hide();
			this.SendToBack();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
			this.Show();
		}

		private void inGame_Click(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			Callback = new HueEntryCallback(HueResp);
			ClientCommunication.SendToClient(new HuePicker());
			World.Player.SendMessage(MsgLevel.Force, LocString.SelHue);
		}

		private void okay_Click(object sender, System.EventArgs e)
		{
			m_Hue = Utility.ToInt32(hueNum.Text, 0);
			this.DialogResult = DialogResult.OK;
			this.Close();
			Callback = null;
		}

		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
			Callback = null;
		}

		private void HueEntry_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames(this);

			SetPreview(m_Hue);
			hueNum.Text = m_Hue.ToString();
		}
	}
}
