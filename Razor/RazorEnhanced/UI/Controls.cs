using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RazorEnhanced.UI
{
	public class RazorRadioButton : RadioButton
	{
		public RazorRadioButton()
		{
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			pevent.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			if (this.Checked)
			{
				pevent.Graphics.DrawImage(Assistant.Properties.Resources.RazorRadioButton_On, 0, 2, 16, 17);
			}
			else
			{
				pevent.Graphics.DrawImage(Assistant.Properties.Resources.RazorRadioButton_Off, 0, 2, 16, 17);
			}
		}
	}

	public class RazorTextBox : TextBox
	{
		public RazorTextBox()
		{
			BorderStyle = BorderStyle.FixedSingle;
			Location = new Point(-1, -1);
			Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			DefaultBorderColor = Color.FromArgb(31, 72, 161);
			FocusedBorderColor = Color.FromArgb(236, 199, 87);
			BackColor = Color.White;
			Padding = new Padding(1);
		}

		public Color DefaultBorderColor { get; set; }
		public Color FocusedBorderColor { get; set; }

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
		}
	}

	public class RazorComboBox : ComboBox
	{
		public RazorComboBox()
		{
			//	this.DropDownStyle = ComboBoxStyle.DropDownList;
			this.SetStyle(ControlStyles.UserPaint, true);
		}


		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			e.DrawBackground();
			var index = e.Index;
			if (index < 0 || index >= Items.Count) return;
			using (var brush = new SolidBrush(e.ForeColor))
			{
				Rectangle rec = new Rectangle(e.Bounds.Left, e.Bounds.Top + ((e.Bounds.Height - ItemHeight) / 2), e.Bounds.Width, ItemHeight - 1);
				e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(this.ForeColor), rec);
			}
			e.DrawFocusRectangle();
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			Pen Colore1Pen = new Pen(Color.FromArgb(31, 72, 161));
			Colore1Pen.Width = 1;
			Pen Colore2Pen = new Pen(Color.FromArgb(68, 135, 228));
			Colore2Pen.Width = 1;

			//Freccia selezione
			if (this.DropDownStyle == ComboBoxStyle.DropDown)
				pevent.Graphics.DrawImage(Assistant.Properties.Resources.RazorComboBox_Arrow, Width - 15, 2, 15, 22);
			else
				pevent.Graphics.DrawImage(Assistant.Properties.Resources.RazorComboBox_Arrow, Width - 15, 2, 15, 21);
			//angoli
			pevent.Graphics.DrawLine(Colore1Pen, 0, 2, 2, 0);
			pevent.Graphics.DrawLine(Colore2Pen, 1, 2, 2, 1);

			pevent.Graphics.DrawLine(Colore1Pen, 0, Height - 3, 2, Height - 1);
			pevent.Graphics.DrawLine(Colore2Pen, 1, Height - 2, 2, Height - 2);

			pevent.Graphics.DrawLine(Colore1Pen, Width - 2, 0, Width, 2);
			pevent.Graphics.DrawLine(Colore2Pen, Width - 2, 1, Width - 1, 2);

			pevent.Graphics.DrawLine(Colore1Pen, Width - 2, Height - 1, Width, Height - 3);
			pevent.Graphics.DrawLine(Colore2Pen, Width - 2, Height - 2, Width - 1, Height - 2);

			//Verticale sinistra e destra
			pevent.Graphics.DrawLine(Colore1Pen, 0, 3, 0, Height - 3);
			pevent.Graphics.DrawLine(Colore2Pen, 1, 3, 1, Height - 3);

			pevent.Graphics.DrawLine(Colore1Pen, Width - 1, 3, Width - 1, Height - 3);
			pevent.Graphics.DrawLine(Colore2Pen, Width - 2, 2, Width - 2, Height - 3);

			//Orizzontale sopra e sotto
			pevent.Graphics.DrawLine(Colore1Pen, 3, 0, Width - 3, 0);
			pevent.Graphics.DrawLine(Colore2Pen, 3, 1, Width - 3, 1);

			pevent.Graphics.DrawLine(Colore1Pen, 3, Height - 1, Width - 3, Height - 1);
			pevent.Graphics.DrawLine(Colore2Pen, 3, Height - 2, Width - 3, Height - 2);

			pevent.Graphics.DrawString(Text, this.Font, new SolidBrush(this.ForeColor), 3, 3, StringFormat.GenericDefault);
			base.OnPaint(pevent);
		}
	}

	public class RazorCheckBox : CheckBox
	{
		public RazorCheckBox()
		{
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			pevent.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			if (this.Checked)
			{
				pevent.Graphics.DrawImage(Assistant.Properties.Resources.RazorCheckBox_On, 0, 2, 16, 17);
			}
			else
			{
				pevent.Graphics.DrawImage(Assistant.Properties.Resources.RazorCheckBox_Off, 0, 2, 16, 17);
			}
		}
	}

	public partial class RazorButton : Button
	{
		#region Fields

		private Theme thm = Theme.MSOffice2010_BLUE;

		private enum MouseState { None = 1, Down = 2, Up = 3, Enter = 4, Leave = 5, Move = 6 }

		private MouseState MState = MouseState.None;

		#endregion

		#region Constructor

		public RazorButton()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor |
					  ControlStyles.Opaque |
					  ControlStyles.ResizeRedraw |
					  ControlStyles.OptimizedDoubleBuffer |
					  ControlStyles.CacheText | // We gain about 2% in painting by avoiding extra GetWindowText calls
					  ControlStyles.StandardClick,
					  true);

			this.colorTable = new Colortable();
		}

		#endregion

		#region Events

		#region Paint Transparent Background

		protected void PaintTransparentBackground(Graphics g, Rectangle clipRect)
		{
			// check if we have a parent
			if (this.Parent != null)
			{
				// convert the clipRects coordinates from ours to our parents
				clipRect.Offset(this.Location);

				PaintEventArgs e = new PaintEventArgs(g, clipRect);

				// save the graphics state so that if anything goes wrong 
				// we're not fubar
				GraphicsState state = g.Save();

				try
				{
					// move the graphics object so that we are drawing in 
					// the correct place
					g.TranslateTransform((float)-this.Location.X, (float)-this.Location.Y);

					// draw the parents background and foreground
					this.InvokePaintBackground(this.Parent, e);
					this.InvokePaint(this.Parent, e);

					return;
				}
				finally
				{
					// reset everything back to where they were before
					g.Restore(state);
					clipRect.Offset(-this.Location.X, -this.Location.Y);
				}
			}

			// we don't have a parent, so fill the rect with
			// the default control color
			g.FillRectangle(SystemBrushes.Control, clipRect);
		}

		#endregion

		#region Mouse Events

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			MState = MouseState.Down;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			MState = MouseState.Up;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs mevent)
		{
			MState = MouseState.Move;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			MState = MouseState.Leave;
			Invalidate();
		}

		#endregion

		#region Path

		public static GraphicsPath GetRoundedRect(RectangleF r, float radius)
		{
			GraphicsPath gp = new GraphicsPath();
			gp.StartFigure();
			r = new RectangleF(r.Left, r.Top, r.Width, r.Height);
			if (radius <= 0.0F || radius <= 0.0F)
			{
				gp.AddRectangle(r);
			}
			else
			{
				gp.AddArc((float)r.X, (float)r.Y, radius, radius, 180, 90);
				gp.AddArc((float)r.Right - radius, (float)r.Y, radius - 1, radius, 270, 90);
				gp.AddArc((float)r.Right - radius, ((float)r.Bottom) - radius - 1, radius - 1, radius, 0, 90);
				gp.AddArc((float)r.X, ((float)r.Bottom) - radius - 1, radius - 1, radius, 90, 90);
			}
			gp.CloseFigure();
			return gp;
		}

		#endregion

		#region Paint

		protected override void OnPaint(PaintEventArgs e)
		{
			this.PaintTransparentBackground(e.Graphics, e.ClipRectangle);

			#region Painting

			//now let's we begin painting
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			#endregion

			#region Color

			Color tTopColorBegin = this.colorTable.ButtonNormalColor1;
			Color tTopColorEnd = this.colorTable.ButtonNormalColor2;
			Color tBottomColorBegin = this.colorTable.ButtonNormalColor3;
			Color tBottomColorEnd = this.colorTable.ButtonNormalColor4;
			Color Textcol = this.colorTable.TextColor;


			if (!this.Enabled)
			{
				tTopColorBegin = Color.FromArgb((int)(tTopColorBegin.GetBrightness() * 255),
					(int)(tTopColorBegin.GetBrightness() * 255),
					(int)(tTopColorBegin.GetBrightness() * 255));
				tBottomColorEnd = Color.FromArgb((int)(tBottomColorEnd.GetBrightness() * 255),
					(int)(tBottomColorEnd.GetBrightness() * 255),
					(int)(tBottomColorEnd.GetBrightness() * 255));
			}
			else
			{
				if (MState == MouseState.None || MState == MouseState.Leave)
				{
					tTopColorBegin = this.colorTable.ButtonNormalColor1;
					tTopColorEnd = this.colorTable.ButtonNormalColor2;
					tBottomColorBegin = this.colorTable.ButtonNormalColor3;
					tBottomColorEnd = this.colorTable.ButtonNormalColor4;
					Textcol = this.colorTable.TextColor;
				}
				else if (MState == MouseState.Down)
				{
					tTopColorBegin = this.colorTable.ButtonSelectedColor1;
					tTopColorEnd = this.colorTable.ButtonSelectedColor2;
					tBottomColorBegin = this.colorTable.ButtonSelectedColor3;
					tBottomColorEnd = this.colorTable.ButtonSelectedColor4;
					Textcol = this.colorTable.SelectedTextColor;
				}
				else if (MState == MouseState.Move || MState == MouseState.Up)
				{
					tTopColorBegin = this.colorTable.ButtonMouseOverColor1;
					tTopColorEnd = this.colorTable.ButtonMouseOverColor2;
					tBottomColorBegin = this.colorTable.ButtonMouseOverColor3;
					tBottomColorEnd = this.colorTable.ButtonMouseOverColor4;
					Textcol = this.colorTable.HoverTextColor;
				}
			}


			#endregion

			#region Theme 2010

			if (thm == Theme.MSOffice2010_BLUE || thm == Theme.MSOffice2010_Green || thm == Theme.MSOffice2010_Yellow || thm == Theme.MSOffice2010_Publisher ||
				thm == Theme.MSOffice2010_RED || thm == Theme.MSOffice2010_WHITE || thm == Theme.MSOffice2010_Pink)
			{
				Paint2010Background(e, g, tTopColorBegin, tTopColorEnd, tBottomColorBegin, tBottomColorEnd);
				TEXTandIMAGE(e.ClipRectangle, g, Textcol);
			}

			#endregion
		}

		#endregion

		#region Paint 2010 Background

		protected void Paint2010Background(PaintEventArgs e, Graphics g, Color tTopColorBegin, Color tTopColorEnd, Color tBottomColorBegin, Color tBottomColorEnd)
		{
			int _roundedRadiusX = 6;

			Rectangle r = new Rectangle(e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
			Rectangle j = r;
			Rectangle r2 = r;
			r2.Inflate(-1, -1);
			Rectangle r3 = r2;
			r3.Inflate(-1, -1);

			//rectangle for gradient, half upper and lower
			RectangleF halfup = new RectangleF(r.Left, r.Top, r.Width, r.Height);
			RectangleF halfdown = new RectangleF(r.Left, r.Top + (r.Height / 2) - 1, r.Width, r.Height);

			//BEGIN PAINT BACKGROUND
			//for half upper, we paint using linear gradient
			using (GraphicsPath thePath = GetRoundedRect(r, _roundedRadiusX))
			{
				LinearGradientBrush lgb = new LinearGradientBrush(halfup, tBottomColorEnd, tBottomColorBegin, 90f, true);

				Blend blend = new Blend(4);
				blend.Positions = new float[] { 0, 0.18f, 0.35f, 1f };
				blend.Factors = new float[] { 0f, .4f, .9f, 1f };
				lgb.Blend = blend;
				g.FillPath(lgb, thePath);
				lgb.Dispose();

				//for half lower, we paint using radial gradient
				using (GraphicsPath p = new GraphicsPath())
				{
					p.AddEllipse(halfdown); //make it radial
					using (PathGradientBrush gradient = new PathGradientBrush(p))
					{
						gradient.WrapMode = WrapMode.Clamp;
						gradient.CenterPoint = new PointF(Convert.ToSingle(halfdown.Left + halfdown.Width / 2), Convert.ToSingle(halfdown.Bottom));
						gradient.CenterColor = tBottomColorEnd;
						gradient.SurroundColors = new Color[] { tBottomColorBegin };

						blend = new Blend(4);
						blend.Positions = new float[] { 0, 0.15f, 0.4f, 1f };
						blend.Factors = new float[] { 0f, .3f, 1f, 1f };
						gradient.Blend = blend;

						g.FillPath(gradient, thePath);
					}
				}
				//END PAINT BACKGROUND

				//BEGIN PAINT BORDERS
				using (GraphicsPath gborderDark = thePath)
				{
					using (Pen p = new Pen(tTopColorBegin, 1))
					{
						g.DrawPath(p, gborderDark);
					}
				}
				using (GraphicsPath gborderLight = GetRoundedRect(r2, _roundedRadiusX))
				{
					using (Pen p = new Pen(tTopColorEnd, 1))
					{
						g.DrawPath(p, gborderLight);
					}
				}
				using (GraphicsPath gborderMed = GetRoundedRect(r3, _roundedRadiusX))
				{
					SolidBrush bordermed = new SolidBrush(Color.FromArgb(50, tTopColorEnd));
					using (Pen p = new Pen(bordermed, 1))
					{
						g.DrawPath(p, gborderMed);
					}
				}
				//END PAINT BORDERS
			}
		}

		#endregion

		#region Paint TEXT AND IMAGE

		protected void TEXTandIMAGE(Rectangle Rec, Graphics g, Color textColor)
		{
			//BEGIN PAINT TEXT
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			#region Top

			if (this.TextAlign == ContentAlignment.TopLeft)
			{
				sf.LineAlignment = StringAlignment.Near;
				sf.Alignment = StringAlignment.Near;
			}
			else if (this.TextAlign == ContentAlignment.TopCenter)
			{
				sf.LineAlignment = StringAlignment.Near;
				sf.Alignment = StringAlignment.Center;
			}
			else if (this.TextAlign == ContentAlignment.TopRight)
			{
				sf.LineAlignment = StringAlignment.Near;
				sf.Alignment = StringAlignment.Far;
			}

			#endregion

			#region Middle

			else if (this.TextAlign == ContentAlignment.MiddleLeft)
			{
				sf.LineAlignment = StringAlignment.Center;
				sf.Alignment = StringAlignment.Near;
			}
			else if (this.TextAlign == ContentAlignment.MiddleCenter)
			{
				sf.LineAlignment = StringAlignment.Center;
				sf.Alignment = StringAlignment.Center;
			}
			else if (this.TextAlign == ContentAlignment.MiddleRight)
			{
				sf.LineAlignment = StringAlignment.Center;
				sf.Alignment = StringAlignment.Far;
			}

			#endregion

			#region Bottom

			else if (this.TextAlign == ContentAlignment.BottomLeft)
			{
				sf.LineAlignment = StringAlignment.Far;
				sf.Alignment = StringAlignment.Near;
			}
			else if (this.TextAlign == ContentAlignment.BottomCenter)
			{
				sf.LineAlignment = StringAlignment.Far;
				sf.Alignment = StringAlignment.Center;
			}
			else if (this.TextAlign == ContentAlignment.BottomRight)
			{
				sf.LineAlignment = StringAlignment.Far;
				sf.Alignment = StringAlignment.Far;
			}

			#endregion

			if (this.ShowKeyboardCues)
				sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
			else
				sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
			g.DrawString(this.Text, this.Font, new SolidBrush(textColor), Rec, sf);
		}

		#endregion

		#endregion

		#region Properties

		#region ColorTable

		Colortable colorTable = null;

		[DefaultValue(typeof(Colortable), "office2010Blue")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Colortable ColorTable
		{
			get
			{
				if (colorTable == null)
					colorTable = new Colortable();

				return colorTable;
			}
			set
			{

				if (value == null)
					value = Colortable.office2010Blue;

				colorTable = (Colortable)value;

				this.Invalidate();

			}
		}

		public Theme Theme
		{
			get
			{
				if (this.colorTable == Colortable.Office2010Green)
				{
					return Theme.MSOffice2010_Green;
				}
				else if (this.colorTable == Colortable.Office2010Red)
				{
					return Theme.MSOffice2010_RED;
				}
				else if (this.colorTable == Colortable.Office2010Pink)
				{
					return Theme.MSOffice2010_Pink;
				}
				else if (this.colorTable == Colortable.Office2010Yellow)
				{
					return Theme.MSOffice2010_Yellow;
				}
				else if (this.colorTable == Colortable.Office2010White)
				{
					return Theme.MSOffice2010_WHITE;
				}
				else if (this.colorTable == Colortable.Office2010Publisher)
				{
					return Theme.MSOffice2010_Publisher;
				}

				return Theme.MSOffice2010_BLUE;
			}

			set
			{
				this.thm = value;

				if (thm == Theme.MSOffice2010_Green)
				{
					this.colorTable = Colortable.Office2010Green;
				}
				else if (thm == Theme.MSOffice2010_RED)
				{
					this.colorTable = Colortable.Office2010Red;
				}
				else if (thm == Theme.MSOffice2010_Pink)
				{
					this.colorTable = Colortable.Office2010Pink;
				}
				else if (thm == Theme.MSOffice2010_WHITE)
				{
					this.colorTable = Colortable.Office2010White;
				}
				else if (thm == Theme.MSOffice2010_Yellow)
				{
					this.colorTable = Colortable.Office2010Yellow;
				}
				else if (thm == Theme.MSOffice2010_Publisher)
				{
					this.colorTable = Colortable.Office2010Publisher;
				}
				else
				{
					this.colorTable = Colortable.office2010Blue;
				}
			}
		}

		#endregion

		#region Background Image

		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		[Browsable(false)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		#endregion

		#endregion
	}

	#region ENUM

	public enum Theme
	{
		MSOffice2010_BLUE = 1,
		MSOffice2010_WHITE = 2,
		MSOffice2010_RED = 3,
		MSOffice2010_Green = 4,
		MSOffice2010_Pink = 5,
		MSOffice2010_Yellow = 6,
		MSOffice2010_Publisher = 7
	}

	#endregion

	#region COLOR TABLE

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class Colortable
	{
		#region Static Color Tables

		static office2010BlueTheme office2010blu = new office2010BlueTheme();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable office2010Blue
		{
			get { return office2010blu; }
		}

		static Office2010Green office2010gr = new Office2010Green();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable Office2010Green
		{
			get { return office2010gr; }
		}

		static Office2010Red office2010rd = new Office2010Red();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable Office2010Red
		{
			get { return office2010rd; }
		}

		static Office2010Pink office2010pk = new Office2010Pink();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable Office2010Pink
		{
			get { return office2010pk; }
		}

		static Office2010Yellow office2010yl = new Office2010Yellow();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable Office2010Yellow
		{
			get { return office2010yl; }
		}

		static Office2010White office2010wt = new Office2010White();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable Office2010White
		{
			get { return office2010wt; }
		}


		static Office2010Publisher office2010pb = new Office2010Publisher();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static Colortable Office2010Publisher
		{
			get { return office2010pb; }
		}


		#endregion

		#region Custom Properties

		Color textColor = Color.White;
		Color selectedTextColor = Color.FromArgb(30, 57, 91);
		Color OverTextColor = Color.FromArgb(30, 57, 91);
		Color borderColor = Color.FromArgb(31, 72, 161);
		Color innerborderColor = Color.FromArgb(68, 135, 228);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color TextColor
		{
			get { return textColor; }
			set { textColor = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color SelectedTextColor
		{
			get { return selectedTextColor; }
			set { selectedTextColor = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color HoverTextColor
		{
			get { return OverTextColor; }
			set { OverTextColor = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color BorderColor1
		{
			get { return borderColor; }
			set { borderColor = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color BorderColor2
		{
			get { return innerborderColor; }
			set { innerborderColor = value; }
		}

		#endregion

		#region Button Normal

		Color buttonNormalBegin = Color.FromArgb(31, 72, 161);
		Color buttonNormalMiddleBegin = Color.FromArgb(68, 135, 228);
		Color buttonNormalMiddleEnd = Color.FromArgb(41, 97, 181);
		Color buttonNormalEnd = Color.FromArgb(62, 125, 219);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonNormalColor1
		{
			get { return buttonNormalBegin; }
			set { buttonNormalBegin = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonNormalColor2
		{
			get { return buttonNormalMiddleBegin; }
			set { buttonNormalMiddleBegin = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonNormalColor3
		{
			get { return buttonNormalMiddleEnd; }
			set { buttonNormalMiddleEnd = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonNormalColor4
		{
			get { return buttonNormalEnd; }
			set { buttonNormalEnd = value; }
		}

		#endregion

		#region Button Selected

		Color buttonSelectedBegin = Color.FromArgb(236, 199, 87);
		Color buttonSelectedMiddleBegin = Color.FromArgb(252, 243, 215);
		Color buttonSelectedMiddleEnd = Color.FromArgb(255, 229, 117);
		Color buttonSelectedEnd = Color.FromArgb(255, 216, 107);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonSelectedColor1
		{
			get { return buttonSelectedBegin; }
			set { buttonSelectedBegin = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonSelectedColor2
		{
			get { return buttonSelectedMiddleBegin; }
			set { buttonSelectedMiddleBegin = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonSelectedColor3
		{
			get { return buttonSelectedMiddleEnd; }
			set { buttonSelectedMiddleEnd = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonSelectedColor4
		{
			get { return buttonSelectedEnd; }
			set { buttonSelectedEnd = value; }
		}

		#endregion

		#region Button Mouse Over

		Color buttonMouseOverBegin = Color.FromArgb(236, 199, 87);
		Color buttonMouseOverMiddleBegin = Color.FromArgb(252, 243, 215);
		Color buttonMouseOverMiddleEnd = Color.FromArgb(249, 225, 137);
		Color buttonMouseOverEnd = Color.FromArgb(251, 249, 224);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonMouseOverColor1
		{
			get { return buttonMouseOverBegin; }
			set { buttonMouseOverBegin = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonMouseOverColor2
		{
			get { return buttonMouseOverMiddleBegin; }
			set { buttonMouseOverMiddleBegin = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonMouseOverColor3
		{
			get { return buttonMouseOverMiddleEnd; }
			set { buttonMouseOverMiddleEnd = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Color ButtonMouseOverColor4
		{
			get { return buttonMouseOverEnd; }
			set { buttonMouseOverEnd = value; }
		}

		#endregion
	}

	#region Office 2010 Blue

	public class office2010BlueTheme : Colortable
	{
		public office2010BlueTheme()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.White;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(31, 72, 161);
			this.ButtonNormalColor2 = Color.FromArgb(68, 135, 228);
			this.ButtonNormalColor3 = Color.FromArgb(41, 97, 181);
			this.ButtonNormalColor4 = Color.FromArgb(62, 125, 219);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "office2010Blue";
		}
	}

	#endregion

	#region Office 2010 GREEN

	public class Office2010Green : Colortable
	{
		public Office2010Green()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.White;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(42, 126, 43);
			this.ButtonNormalColor2 = Color.FromArgb(94, 184, 67);
			this.ButtonNormalColor3 = Color.FromArgb(42, 126, 43);
			this.ButtonNormalColor4 = Color.FromArgb(94, 184, 67);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "Office2010Green";
		}
	}

	#endregion

	#region Office 2010 Red

	public class Office2010Red : Colortable
	{
		public Office2010Red()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.White;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(227, 77, 45);
			this.ButtonNormalColor2 = Color.FromArgb(245, 148, 64);
			this.ButtonNormalColor3 = Color.FromArgb(227, 77, 45);
			this.ButtonNormalColor4 = Color.FromArgb(245, 148, 64);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "Office2010Red";
		}
	}

	#endregion

	#region Office 2010 Pink

	public class Office2010Pink : Colortable
	{
		public Office2010Pink()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.White;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(175, 6, 77);
			this.ButtonNormalColor2 = Color.FromArgb(222, 52, 119);
			this.ButtonNormalColor3 = Color.FromArgb(175, 6, 77);
			this.ButtonNormalColor4 = Color.FromArgb(222, 52, 119);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "Office2010Pink";
		}
	}

	#endregion

	#region Office 2010 White

	public class Office2010White : Colortable
	{
		public Office2010White()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.Black;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(154, 154, 154);
			this.ButtonNormalColor2 = Color.FromArgb(255, 255, 255);
			this.ButtonNormalColor3 = Color.FromArgb(235, 235, 235);
			this.ButtonNormalColor4 = Color.FromArgb(255, 255, 255);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "Office2010White";
		}
	}

	#endregion

	#region Office 2010 Yellow

	public class Office2010Yellow : Colortable
	{
		public Office2010Yellow()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.White;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(252, 161, 8);
			this.ButtonNormalColor2 = Color.FromArgb(251, 191, 45);
			this.ButtonNormalColor3 = Color.FromArgb(252, 161, 8);
			this.ButtonNormalColor4 = Color.FromArgb(251, 191, 45);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "Office2010Yellow";
		}
	}

	#endregion

	#region Office 2010 Publisher

	public class Office2010Publisher : Colortable
	{
		public Office2010Publisher()
		{
			// Border Color

			this.BorderColor1 = Color.FromArgb(31, 72, 161);
			this.BorderColor2 = Color.FromArgb(68, 135, 228);

			// Button Text Color

			this.TextColor = Color.White;
			this.SelectedTextColor = Color.FromArgb(30, 57, 91);
			this.HoverTextColor = Color.FromArgb(30, 57, 91);

			// Button normal color

			this.ButtonNormalColor1 = Color.FromArgb(0, 126, 126);
			this.ButtonNormalColor2 = Color.FromArgb(31, 173, 167);
			this.ButtonNormalColor3 = Color.FromArgb(0, 126, 126);
			this.ButtonNormalColor4 = Color.FromArgb(31, 173, 167);

			// Button mouseover color

			this.ButtonMouseOverColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonMouseOverColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonMouseOverColor3 = Color.FromArgb(249, 225, 137);
			this.ButtonMouseOverColor4 = Color.FromArgb(251, 249, 224);

			// Button selected color

			this.ButtonSelectedColor1 = Color.FromArgb(236, 199, 87);
			this.ButtonSelectedColor2 = Color.FromArgb(252, 243, 215);
			this.ButtonSelectedColor3 = Color.FromArgb(255, 229, 117);
			this.ButtonSelectedColor4 = Color.FromArgb(255, 216, 107);
		}

		public override string ToString()
		{
			return "Office2010Publisher";
		}
	}

	#endregion
	#endregion
}
