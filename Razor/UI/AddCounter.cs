using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Assistant
{
	/// <summary>
	/// Summary description for AddCounter.
	/// </summary>
	public class AddCounter : System.Windows.Forms.Form
	{
       
		private System.Windows.Forms.Label label1;
        private RazorUIMod.XTextBox name;
		private System.Windows.Forms.Label label2;
        private RazorUIMod.XTextBox format;
		private System.Windows.Forms.Label label3;
        private RazorUIMod.XTextBox itemid;
		private System.Windows.Forms.Label label4;
        private RazorUIMod.XTextBox hue;
        private RazorUIMod.XButton Add;
        private RazorUIMod.XButton cancel;
        private RazorUIMod.XButton target;
        private RazorUIMod.XButton delete;
        private RazorUIMod.XCheckBox dispImg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddCounter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public AddCounter( Counter c ) : this()
		{
			name.Text = c.Name;
			format.Text = c.Format;
			itemid.Text = c.ItemID.ToString();
			hue.Text = c.Hue.ToString();
			dispImg.Checked = c.DisplayImage;

			delete.Visible = true;
			this.Text = "Edit Counter";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            RazorUIMod.Colortable colortable1 = new RazorUIMod.Colortable();
            RazorUIMod.Colortable colortable2 = new RazorUIMod.Colortable();
            RazorUIMod.Colortable colortable3 = new RazorUIMod.Colortable();
            RazorUIMod.Colortable colortable4 = new RazorUIMod.Colortable();
            this.label1 = new System.Windows.Forms.Label();
            this.name = new RazorUIMod.XTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.format = new RazorUIMod.XTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.itemid = new RazorUIMod.XTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.hue = new RazorUIMod.XTextBox();
            this.Add = new RazorUIMod.XButton();
            this.cancel = new RazorUIMod.XButton();
            this.target = new RazorUIMod.XButton();
            this.delete = new RazorUIMod.XButton();
            this.dispImg = new RazorUIMod.XCheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // name
            // 
            this.name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.name.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.name.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.name.Location = new System.Drawing.Point(58, 9);
            this.name.Name = "name";
            this.name.Padding = new System.Windows.Forms.Padding(1);
            this.name.Size = new System.Drawing.Size(81, 22);
            this.name.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(178, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Format:";
            // 
            // format
            // 
            this.format.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.format.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.format.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.format.Location = new System.Drawing.Point(235, 9);
            this.format.Name = "format";
            this.format.Padding = new System.Windows.Forms.Padding(1);
            this.format.Size = new System.Drawing.Size(53, 22);
            this.format.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Item ID:";
            // 
            // itemid
            // 
            this.itemid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.itemid.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.itemid.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.itemid.Location = new System.Drawing.Point(58, 37);
            this.itemid.Name = "itemid";
            this.itemid.Padding = new System.Windows.Forms.Padding(1);
            this.itemid.Size = new System.Drawing.Size(52, 22);
            this.itemid.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(139, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Color (Any: -1):";
            // 
            // hue
            // 
            this.hue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.hue.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.hue.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.hue.Location = new System.Drawing.Point(235, 37);
            this.hue.Name = "hue";
            this.hue.Padding = new System.Windows.Forms.Padding(1);
            this.hue.Size = new System.Drawing.Size(53, 22);
            this.hue.TabIndex = 7;
            // 
            // Add
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
            this.Add.ColorTable = colortable1;
            this.Add.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Add.Location = new System.Drawing.Point(10, 97);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(67, 23);
            this.Add.TabIndex = 8;
            this.Add.Text = "&Okay";
            this.Add.Theme = RazorUIMod.Theme.MSOffice2010_BLUE;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // cancel
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
            this.cancel.ColorTable = colortable2;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(86, 97);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(68, 23);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "Cancel";
            this.cancel.Theme = RazorUIMod.Theme.MSOffice2010_BLUE;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // target
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
            this.target.ColorTable = colortable3;
            this.target.Location = new System.Drawing.Point(240, 97);
            this.target.Name = "target";
            this.target.Size = new System.Drawing.Size(67, 23);
            this.target.TabIndex = 10;
            this.target.Text = "Target ";
            this.target.Theme = RazorUIMod.Theme.MSOffice2010_BLUE;
            this.target.Click += new System.EventHandler(this.target_Click);
            // 
            // delete
            // 
            colortable4.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            colortable4.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            colortable4.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            colortable4.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            colortable4.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            colortable4.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            colortable4.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            colortable4.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            colortable4.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            colortable4.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            colortable4.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            colortable4.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            colortable4.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            colortable4.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            colortable4.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            colortable4.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            colortable4.TextColor = System.Drawing.Color.White;
            this.delete.ColorTable = colortable4;
            this.delete.Location = new System.Drawing.Point(163, 97);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(67, 23);
            this.delete.TabIndex = 11;
            this.delete.Text = "Delete";
            this.delete.Theme = RazorUIMod.Theme.MSOffice2010_BLUE;
            this.delete.Visible = false;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // dispImg
            // 
            this.dispImg.Checked = true;
            this.dispImg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dispImg.Location = new System.Drawing.Point(10, 65);
            this.dispImg.Name = "dispImg";
            this.dispImg.Size = new System.Drawing.Size(192, 23);
            this.dispImg.TabIndex = 12;
            this.dispImg.Text = "Display image in titlebar";
            // 
            // AddCounter
            // 
            this.AcceptButton = this.Add;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(325, 136);
            this.ControlBox = false;
            this.Controls.Add(this.dispImg);
            this.Controls.Add(this.hue);
            this.Controls.Add(this.itemid);
            this.Controls.Add(this.format);
            this.Controls.Add(this.name);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.target);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddCounter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add a Counter";
            this.Load += new System.EventHandler(this.AddCounter_Load);
            this.ResumeLayout(false);

		}
		#endregion

		public string NameStr, FmtStr;
		public int ItemID, Hue;
		public bool DisplayImage;
		private void Add_Click(object sender, System.EventArgs e)
		{
			if ( name.Text.Trim().Length > 0 && format.Text.Trim().Length > 0 )
			{
				NameStr = name.Text.Trim();
				FmtStr = format.Text.Trim();
			}
			else
			{
				MessageBox.Show( this, Language.GetString( LocString.InvalidAbrev ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				if ( itemid.Text.StartsWith( "0x" ) )
					ItemID = Convert.ToUInt16( itemid.Text.Substring( 2 ).Trim(), 16 );
				else
					ItemID = Convert.ToUInt16( itemid.Text.Trim() );
			}
			catch
			{
				ItemID = 0;
			}

			if ( ItemID == 0 )
			{
				MessageBox.Show( this, Language.GetString( LocString.InvalidIID ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			Hue = Utility.ToInt32( hue.Text, -1 );

			if ( Hue < -1 || Hue > 0xFFFF )
			{
				MessageBox.Show( this, Language.GetString( LocString.InvalidHue ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				Hue = 0;
				return;
			}

			DisplayImage = dispImg.Checked;
		}

		private void cancel_Click(object sender, System.EventArgs e)
		{
			Targeting.CancelOneTimeTarget();
		}

		private void target_Click(object sender, System.EventArgs e)
		{
			if ( World.Player != null )
			{
				Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
				World.Player.SendMessage( MsgLevel.Force, LocString.SelItem2Count );
			}
		}

		private void OnTarget( bool loc, Serial serial, Point3D p, ushort graphic )
		{
			Engine.MainWindow.ShowMe();
			this.BringToFront();
			this.Show();
			this.Focus();
			if ( loc )
				return;

			Item item = World.FindItem( serial );
			if ( item != null )
			{
				itemid.Text = item.ItemID.Value.ToString();
				hue.Text = item.Hue == 0 ? "-1" : item.Hue.ToString();
			}
			else
			{
				itemid.Text = graphic.ToString();
				hue.Text = "-1";
			}
		}

		private void AddCounter_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames( this );
		}

		private void delete_Click(object sender, System.EventArgs e)
		{
			if ( MessageBox.Show( this, "Are you sure you want to delete this counter?", "Delete Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
			{
				this.DialogResult = DialogResult.Abort;
				this.Close();
			}
		}
	}
}
