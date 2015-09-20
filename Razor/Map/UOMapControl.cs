using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Assistant.MapUO
{
	internal class UOMapControl : Panel
	{
		delegate void UpdateMapCallback();

		private bool m_Active;
		private Point prevPoint;
		private Mobile m_Focus;
		private const double RotateAngle = Math.PI / 4 + Math.PI;
		private Bitmap m_Background;
		private DateTime LastRefresh;


        private Point TempPosDeath = default(Point);
        private Ultima.Map map__1 = default(Ultima.Map);
        private int CurrentFacet = 9;
        private int VarZoom = 0;
        private float OffsetZoom = 0;
        internal static float zoom = 1.5F;
        private Point drawPoint = new Point();
        private Font m_Font = new Font("Arial", 8 / zoom);
        private Color m_usercolor = Color.Blue;
        private int m_userfacettemp = 1;
        private bool m_track_friend = false;
        private bool m_tiltmap = true;
        private bool m_removeuser = false;
        internal static DateTime m_timeafterdeath;
        internal static bool m_booldeathpoint;
        private Point m_offsetbuild;
        private bool m_freeview = false;
        private bool m_gotolocs = false;
        private Point m_pos_temp;
        private Point m_offset;
        private Point m_effettivo;
        private Point m_gotoloc_position;
        private string m_usernametemp;
        private bool m_guardlines = true;
        private bool m_builds = true;
        private string m_labeldescbuild;
        private bool m_showlabelbuild = true;
        private Rectangle m_labelrectbuild;
        private bool m_coordinates = true;
        private float m_zoomvalue;
        private Single m_zoom;

        private static Font m_BoldFont = new Font("Courier New", 8, FontStyle.Bold);
        private static Font m_SmallFont = new Font("Arial", 6);
        private static Font m_RegFont = new Font("Arial", 8);

		internal Mobile FocusMobile
		{
			get
			{
				if (m_Focus == null || m_Focus.Deleted || !PacketHandlers.Party.Contains(m_Focus.Serial))
				{
					/*if ( World.Player == null )
						return new Mobile( Serial.Zero );
					else*/
					return World.Player;
				}
				return m_Focus;
			}
			set { m_Focus = value; }
		}

		internal UOMapControl()
		{
			m_Active = true;
			this.DoubleBuffered = true;
			this.prevPoint = new Point(0, 0);
			this.BorderStyle = BorderStyle.Fixed3D;
		}


		public override void Refresh()
		{
			TimeSpan now = DateTime.Now - LastRefresh;
			if (now.TotalMilliseconds <= 100)
				return;
			LastRefresh = DateTime.Now;
			base.Refresh();
		}

		private PointF AdjustPoint(PointF center, PointF pos)
		{
			PointF newp = new PointF(center.X - pos.X, center.Y - pos.Y);
			float dis = (float)Distance(center, pos);
			dis += dis * 0.50f;
			float slope = 0;
			if (newp.X != 0)
				slope = (float)newp.Y / (float)newp.X;
			else
				return new PointF(0 + center.X, -1f * (newp.Y + (newp.Y * 0.25f)) + center.Y);
			slope *= -1;
			//Both of these algorithms oddly produce the same results.
			//float x = dis / (float)(Math.Sqrt(1f + Math.Pow(slope, 2)));
			float x = newp.X + (newp.X * 0.5f);
			// if (newp.X > 0)
			x *= -1;
			float y = (-1) * slope * x;

			PointF def = new PointF(x + center.X, y + center.Y);

			return def;
		}
		internal double Distance(PointF center, PointF pos)
		{

			PointF newp = new PointF(center.X - pos.X, center.Y - pos.Y);
			double distX = Math.Pow(newp.X, 2);
			double distY = Math.Pow(newp.Y, 2);
			return Math.Sqrt(distX + distY);
		}
		private PointF RotatePoint(PointF center, PointF pos)
		{
			PointF newp = new PointF(center.X - pos.X, center.Y - pos.Y);
			double x = newp.X * Math.Cos(RotateAngle) - newp.Y * Math.Sin(RotateAngle);
			double y = newp.X * Math.Sin(RotateAngle) + newp.Y * Math.Sin(RotateAngle);
			return AdjustPoint(center, new PointF((float)(x) + center.X, (float)(y) + center.Y));
		}

        //////////////////////////////////////////////////////////////////
        /////////////// FUNZIONI DI DISEGNO ELEMENTI /////////////////////
        //////////////////////////////////////////////////////////////////

        public void NameAndBar(Graphics gfx, Point pntPlayer)
        {
            PointF StringPointF = RotatePoint(pntPlayer, new Point(pntPlayer.X, pntPlayer.Y));
            string Nome = World.Player.Name;
            Font Font = new Font("Arial", 9 / zoom);
            Brush TextCol = new SolidBrush(m_usercolor);
            int offsetbarre = 14;

            gfx.DrawString(Nome, Font, Brushes.Black, ((StringPointF.X - 2.8f) / zoom), ((StringPointF.Y / 1) + 2.5f) / zoom);
            gfx.DrawString(Nome, Font, TextCol, ((StringPointF.X - 2.8f) / zoom), ((StringPointF.Y / 1) + 1f) / zoom);

            if (RazorEnhanced.Settings.General.ReadBool("MapHpBarCheckBox"))
            {
                Brush status = default(Brush);
                if (World.Player.Poisoned)
                {
                    status = Brushes.LimeGreen;
                }
         /*       else if (World.Player.YellowHits)
                {
                    status = Brushes.Yellow;
                }
                else if (World.Player.Paralyzed)
                {
                    status = Brushes.AliceBlue;
                }*/
                else
                {
                    status = Brushes.SteelBlue;
                }
                gfx.FillRectangle(Brushes.Red, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, 35 / zoom, 3 / zoom);
                int percent = Convert.ToInt32(World.Player.Hits * 100 / (World.Player.HitsMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(World.Player.HitsMax)));
                float imagepercent = (35 / zoom) * (percent / 100);
                gfx.FillRectangle(status, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, imagepercent, 3 / zoom);
                offsetbarre += 4;
            }

            if (RazorEnhanced.Settings.General.ReadBool("MapManaBarCheckBox"))
            {
                gfx.FillRectangle(Brushes.Blue, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, 35 / zoom, 3 / zoom);
                int percent = Convert.ToInt32(World.Player.Mana * 100 / (World.Player.ManaMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(World.Player.ManaMax)));
                float imagepercent = (35 / zoom) * (percent / 100);
                gfx.FillRectangle(Brushes.Blue, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, imagepercent, 3 / zoom);
                offsetbarre += 4;
            }

            if (RazorEnhanced.Settings.General.ReadBool("MapStaminaBarCheckBox"))
            {
                gfx.FillRectangle(Brushes.Orange, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, 35 / zoom, 3 / zoom);
                int percent = Convert.ToInt32(World.Player.Stam * 100 / (World.Player.StamMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(World.Player.StamMax)));
                float imagepercent = (35 / zoom) * (percent / 100);
                gfx.FillRectangle(Brushes.Orange, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, imagepercent, 3 / zoom);
                offsetbarre += 4;
            }

            Font.Dispose();
            TextCol.Dispose();
        }


        public void Buildings(Graphics gfx, Point pntPlayer, Point mapOrigin, Point offset, Rectangle rect)
        {
            // Dim x As Stopwatch = Stopwatch.StartNew
            int Facet = World.Player.Map;
            GraphicsState TransState = gfx.Save();

            int index = 0;
            foreach (List<MapIcon.MapIconData> CurrentList in MapIcon.AllListOfBuilds)
            {
              // if (Opzioni.CheckedListBox1.GetItemChecked(index))
                //{
                    foreach (MapIcon.MapIconData build in CurrentList)
                    {
                        if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                        {
                            Point BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                            //Calcolo Posizione Edificio
                            PointF BuildPointF = RotatePoint(pntPlayer, BuildPoint);

                            //Se sta nel riquadro della mappa
                            if (!(BuildPointF.X <= -m_offsetbuild.X | BuildPointF.X > rect.Width + m_offsetbuild.X | BuildPointF.Y <= -m_offsetbuild.Y | BuildPointF.Y > rect.Height + m_offsetbuild.Y))
                            {

                                GraphicsState transState2 = gfx.Save();
                                gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                                //traslo sul punto dell'edificio
                                if (m_tiltmap)
                                    gfx.RotateTransform(-45);
                                Image Icon = MapIcon.IconImage[build.Icon];

                                gfx.DrawImage(Icon, (-Icon.Width / 2) / zoom, (-Icon.Height / 2) / zoom, Icon.Width / zoom, Icon.Height / zoom);
                                //disegno
                                gfx.Restore(transState2);
                            }
                        }
                  //  }
                }
                index += 1;
            }
            gfx.Restore(TransState);
        }


        public void PgOutRange(Graphics gfx, Point pntPlayer, Point drawPoint, Rectangle rect, MapNetworkIn.UserData user, PointF drawPointF, string nome)
        {
            gfx.ResetTransform();

            List<Point> intersections = Geometry2.SegmentToRectangleIntersect(pntPlayer, new Point((int)drawPointF.X, (int)drawPointF.Y), rect, Geometry2.SegmentIntersection.Point);

            foreach (Point pt in intersections)
            {
                SizeF stringsize = gfx.MeasureString(nome, m_Font);
                Point p_ = new Point(pt.X, pt.Y);

                if (drawPointF.X < 0)
                {
                    p_.X = pt.X;
                }
                if (drawPointF.X > rect.Width)
                {
                    p_.X = pt.X - (int)stringsize.Width;
                }
                if (drawPointF.Y < 0)
                {
                    p_.Y = pt.Y;
                }
                if (drawPointF.Y > rect.Height)
                {
                    p_.Y = pt.Y - (int)stringsize.Height;
                }

                if (nome == "_DEATH_")
                {
                    gfx.DrawLine(new Pen(Brushes.White, 0), pntPlayer, p_);
                    gfx.DrawImage(Properties.Resources.map_deadOut, p_.X, p_.Y, 14, 15);
                }
                else if (nome == "_MARKER_")
                {
                    gfx.DrawLine(Pens.White, pntPlayer, p_);
                }
                else
                {
                    int Facet = 0;
                    if (m_track_friend == false)
                    {
                        Facet = World.Player.Map;
                    }
                    else
                    {
                        Facet = m_userfacettemp;
                    }
                    SolidBrush stato = default(SolidBrush);
                    if (user.Facet != Facet)
                    {
                        stato = new SolidBrush(Color.DarkGray);
                    }
                    else
                    {
                        stato = new SolidBrush(m_usercolor);
                    }

                    gfx.FillRectangle(new SolidBrush(stato.Color), Geometry2.GetRectangleAt(pt, 1));
                    gfx.DrawString(nome, m_Font, new SolidBrush(Color.Black), p_.X, p_.Y + 2.5f);
                    gfx.DrawString(nome, m_Font, new SolidBrush(m_usercolor), p_.X, p_.Y + 1f);
                }
            }
        }


        public void PgInRange(Graphics gfx, Point drawPoint, PointF drawPointF, MapNetworkIn.UserData user, string nome, Point pntPlayer)
        {
            if (nome == "_MARKER_")
            {
                Pen pen_ = new Pen(Brushes.White, 1.2f / zoom);
                pen_.DashStyle = DashStyle.Dot;
                gfx.DrawLine(pen_, pntPlayer, drawPoint);
                gfx.FillEllipse(Brushes.White, drawPoint.X - 2.5f / zoom, drawPoint.Y - 2.5f / zoom, 5 / zoom, 5 / zoom);
            }
            else if (nome == "_DEATH_")
            {
                if (((DateTime.Now - m_timeafterdeath).TotalMinutes >= 5 & World.Player.Hits > 0) | World.Player.IsGhost)
                {
                    if (user.Nome == "_DEATH_")
                    {
                        m_removeuser = true;
                      //  ImDead = false;
                       // RemoveDeathPointToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    gfx.DrawLine(new Pen(Brushes.White, 0), pntPlayer, new Point(drawPoint.X + 3, drawPoint.Y + 5));
                    gfx.TranslateTransform(drawPoint.X, drawPoint.Y);
                    if (m_tiltmap)
                        gfx.RotateTransform(-45);
                    gfx.DrawImage(Properties.Resources.map_deadIn, -6 / zoom, 0, 14 / zoom, 15 / zoom);
                }
            }
            else
            {
                Font Font = new Font("Arial", 9 / zoom);
                Brush TextCol = new SolidBrush(m_usercolor);
                gfx.FillRectangle(TextCol, drawPoint.X - 1 / zoom, drawPoint.Y - 1 / zoom, 2f / zoom, 2f / zoom);
                gfx.TranslateTransform(drawPoint.X, drawPoint.Y);
                if (m_tiltmap)
                    gfx.RotateTransform(-45);

                gfx.DrawString(nome, Font, Brushes.Black, -1f / zoom, 2f / zoom);
                gfx.DrawString(nome, Font, TextCol, -1f / zoom, 0.5f / zoom);
                Font.Dispose();
                TextCol.Dispose();
                int offsetbarre = 14;

                if (RazorEnhanced.Settings.General.ReadBool("MapHpBarCheckBox"))
                {
                    Brush status = default(Brush);
                    short _Flag = user.Flag;
                    if (_Flag == 1)
                    {
                        status = Brushes.LimeGreen;
                    }
                    else if (_Flag == 2)
                    {
                        status = Brushes.Yellow;
                    }
                    else if (_Flag == 3)
                    {
                        status = Brushes.AliceBlue;
                    }
                    else if (_Flag == 4)
                    {
                        status = Brushes.SteelBlue;
                    }
                    else
                    {
                        status = Brushes.SteelBlue;
                    }

                    
                    gfx.FillRectangle(Brushes.Red, -1 / zoom, offsetbarre / zoom, 35 / zoom, 3 / zoom);
                    int percent = Convert.ToInt32(user.Hits * 100 / (user.HitsMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.HitsMax)));
                    float imagepercent = (35 / zoom) * (percent / 100);
                    gfx.FillRectangle(status, -1 / zoom, offsetbarre / zoom, imagepercent, 3 / zoom);
                    offsetbarre += 4;
                }
                if (RazorEnhanced.Settings.General.ReadBool("MapManaBarCheckBox"))
                {
                    gfx.FillRectangle(Brushes.Blue, -1 / zoom, offsetbarre / zoom, 35 / zoom, 3 / zoom);
                    int percent = Convert.ToInt32(user.Mana * 100 / (user.ManaMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.ManaMax)));
                    float imagepercent = (35 / zoom) * (percent / 100);
                    gfx.FillRectangle(Brushes.Blue, -1 / zoom, offsetbarre / zoom, imagepercent, 3 / zoom);
                    offsetbarre += 4;
                }

                if (RazorEnhanced.Settings.General.ReadBool("MapStaminaBarCheckBox"))
                {
                    gfx.FillRectangle(Brushes.Orange, -1 / zoom, offsetbarre / zoom, 35 / zoom, 3 / zoom);
                    int percent = Convert.ToInt32(user.Stamina * 100 / (user.StaminaMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.StaminaMax)));
                    float imagepercent = (35 / zoom) * (percent / 100);
                    gfx.FillRectangle(Brushes.Orange, -1 / zoom, offsetbarre / zoom, imagepercent, 3 / zoom);
                    offsetbarre += 4;
                }
            }
        }


        public void Coordinates(Graphics gfx, Point Focus, Ultima.Map map__1)
        {
            int xLong = 0;
            int yLat = 0;
            int xMins = 0;
            int yMins = 0;
            bool xEast = false;
            bool ySouth = false;
            string locString = null;
           
                locString = String.Format("{0}°{1}'{2} {3}°{4}'{5} | (X: {6}, Y: {7})", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W", Focus.X, Focus.Y);
        

            this.Text = "UOAM2 - " + World.Player.Name + " - [" + locString + "]";
        }


        public void Guardlines(Graphics gfx, Point mapOrigin, Point offset, Rectangle rect)
        {
            int Facet = World.Player.Map;
            Pen pen = new Pen(Brushes.Green, 0);
            Rectangle RegionPoint = default(Rectangle);
            foreach (Assistant.MapUO.Region region in Assistant.MapUO.Region.RegionLists)
            {
                RegionPoint = new Rectangle((region.X) - (mapOrigin.X << 3) - offset.X, (region.Y) - (mapOrigin.Y << 3) - offset.Y, region.Width, region.Height);
                if (!(RegionPoint.X <= -m_offsetbuild.X | RegionPoint.X > rect.Width + m_offsetbuild.X | RegionPoint.Y <= -m_offsetbuild.Y | RegionPoint.Y > rect.Height + m_offsetbuild.Y) & region.Facet == Facet)
                {
                    gfx.DrawRectangle(pen, RegionPoint.X, RegionPoint.Y, region.Width, region.Height);
                }
            }
            pen.Dispose();
        }


        /////////////////////////////////////////////////////////////////////
        ////////////////// FINE FUNZIONI DISEGNO ELEMENTI ///////////////////
        /////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////
        ////////////////////////// FUNZIONI EVENTI ///////////////////////
        //////////////////////////////////////////////////////////////////

        internal void PictureBox1_MouseWheel(System.Object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (e.Delta <= 0)
                {
                    if (zoom > 0.5f)
                    {
                        zoom = zoom - 0.5f;
                    }
                }
                else
                {
                    if (zoom < 4f)
                    {
                        zoom = zoom + 0.5f;
                    }
                }
                ZoomLevel();
                m_zoomvalue = zoom;
                FullUpdate(true);
            }
        }


        public void ZoomLevel()
        {
            Size PicDim = new Size(this.Width, this.Height);


            if (zoom == 0.5f)
            {
                VarZoom = 720;
                OffsetZoom = 180;
            }
            else if(zoom == 1f)
            {
                    VarZoom = 400;
                    OffsetZoom = 80;
            }
            else if(zoom == 1.5f)
            {
                    VarZoom = 200;
                    OffsetZoom = 40;
            }
            else if(zoom == 2f)
            {
                    VarZoom = 120;
                    OffsetZoom = 30;
            }
            else if(zoom == 2.5f)
            {
                    VarZoom = 80;
                    OffsetZoom = 15;
            }
            else if(zoom == 3f)
            {
                    VarZoom = 40;
                    OffsetZoom = 7.5f;
            }
            else if(zoom == 3.5f)
            {
                    VarZoom = 40;
                    OffsetZoom = 7.5f;
            }
            else if(zoom == 4f)
            {
                    VarZoom = 0;
                    OffsetZoom = 0;
            }

            m_offsetbuild = new Point(PicDim.Width * 2 / 350, PicDim.Height * 2 / 350);

            if (map__1 != null)
                SupportFunc.ClearCache(map__1);
        }

        internal void picturebox1_DoubleClick(object sender, EventArgs e)
        {
            if (MapUO.MapWindow.UoMapWindowStatic.FormBorderStyle == FormBorderStyle.None)
            {
                MapUO.MapWindow.UoMapWindowStatic.FormBorderStyle = FormBorderStyle.Sizable;
                MapUO.MapWindow.UoMapWindowStatic.TopMost = false;            
            }
            else
            {
                MapUO.MapWindow.UoMapWindowStatic.FormBorderStyle = FormBorderStyle.None;
                MapUO.MapWindow.UoMapWindowStatic.TopMost = true;
                
            }
            FullUpdate(true);
        }

        internal void PictureBox1_MouseClick(System.Object sender, MouseEventArgs e)
        {
           if (e.Button == MouseButtons.Right)
           {
               MapUO.MapWindow.UoMenuStatic.Visible = true;
               MapUO.MapWindow.UoMenuStatic.Location = Cursor.Position;
           }
        }

        //////////////////////////////////////////////////////////////////
        ///////////////////// FINE FUNZIONI EVENTI ///////////////////////
        //////////////////////////////////////////////////////////////////

        public void FullUpdate(bool SkipTimerControl = false)
        {
            if (this.Width < 1 || this.Height < 1)
                return;

            if (m_Background != null)
                m_Background.Dispose();

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int w = (this.Width) >> 3;
            int h = (this.Height) >> 3;
            int xtrans = this.Width / 2;
            int ytrans = this.Height / 2;
            Point PositionPg = default(Point);

            PositionPg = new Point(World.Player.Position.X, World.Player.Position.Y);


                Point focus = default(Point);
                if (m_freeview == false & m_track_friend == false & m_gotolocs == false)
                {
                    focus = new Point(PositionPg.X, PositionPg.Y);
                }
                else
                {
                    //   FreeView
                    if (m_freeview)
                    {
                        if (m_track_friend)
                            m_track_friend = false;
                        if (MapNetwork.UData != null)
                        {
                            if (m_pos_temp.X == PositionPg.X & m_pos_temp.Y == PositionPg.Y)
                            {
                                focus = new Point(PositionPg.X + m_effettivo.X, PositionPg.Y + m_effettivo.Y);
                            }
                            else
                            {
                                focus = new Point(m_pos_temp.X + m_effettivo.X, m_pos_temp.Y + m_effettivo.Y);
                            }
                            m_pos_temp = focus;
                            m_effettivo.X = m_effettivo.Y = 0;
                            if (World.Player.Poisoned)
                                SupportFunc.AddMyUser(1);
                            else
                                SupportFunc.AddMyUser(0);
                        }

                    }

                    //   Tracking
                    if (m_track_friend)
                    {
                        //if (FreeViewToolStripMenuItem.Enabled == true)
                         //   FreeViewToolStripMenuItem.Enabled = false;
                    
                        if (MapNetwork.UData != null)
                        {
                            foreach (MapNetworkIn.UserData user in MapNetwork.UData)
                            {
                                if (user.Nome == m_usernametemp)
                                {
                                    m_pos_temp = new Point(user.X, user.Y);
                                }
                            }
                            focus = m_pos_temp;
                            //Pos_temp = New Point(focus.X, focus.Y)
                            if (World.Player.Poisoned)
                                SupportFunc.AddMyUser(1);
                            else
                                SupportFunc.AddMyUser(0);
                        }
                    }
                }

                //   GotoLoc
                if (m_gotolocs)
                {
                    if (MapNetwork.UData != null)
                    {
                        m_pos_temp = m_gotoloc_position;
                        focus = new Point(m_pos_temp.X, m_pos_temp.Y);
                        if (World.Player.Poisoned)
                            SupportFunc.AddMyUser(1);
                        else
                            SupportFunc.AddMyUser(0);
                    }
                }


                Point offset = new Point(focus.X & 7, focus.Y & 7);
                Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));
                Point pntPlayer = new Point((focus.X) - (mapOrigin.X << 3) - offset.X, (focus.Y) - (mapOrigin.Y << 3) - offset.Y);

                int Facet = 0;
                if (World.Player.HitsMax != 0)
                {
                    if (m_track_friend == false)
                    {
                        Facet = World.Player.Map;
                    }
                    else
                    {
                        Facet = m_userfacettemp;
                    }
                }
                else
                {
                    Facet = 10;
                }

                if (Facet != CurrentFacet)
                {
                    // map__1 = Nothing
                    if (map__1 != null)
                        SupportFunc.ClearCache(map__1);
                    map__1 = SupportFunc.GetMap(Facet);
                    if (map__1 == null)
                        map__1 = Ultima.Map.InitializeMap("Felucca");
                    CurrentFacet = Facet;
                }

                m_Background = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                using (Graphics gfx = Graphics.FromImage(m_Background))
                {
                    gfx.SmoothingMode = SmoothingMode.AntiAlias;
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    gfx.TranslateTransform(-pntPlayer.X, -pntPlayer.Y, MatrixOrder.Append);
                    if (zoom >= 1f)
                    {
                        gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    }
                    else
                    {
                        gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    }
                    gfx.PageUnit = GraphicsUnit.Pixel;
                    gfx.ScaleTransform(zoom, zoom, MatrixOrder.Append);
                    if (m_tiltmap)
                        gfx.RotateTransform(45, MatrixOrder.Append);
                    gfx.TranslateTransform(pntPlayer.X, pntPlayer.Y, MatrixOrder.Append);

                    //   Disegna mappa
                    gfx.DrawImage(map__1.GetImage(mapOrigin.X - (VarZoom >> 3), mapOrigin.Y - (VarZoom >> 3), (int)(w + offset.X + OffsetZoom), (int)(h + offset.Y + OffsetZoom), true), -offset.X - VarZoom, -offset.Y - VarZoom);
                    //  Opzioni.log("Map:" + x.ElapsedMilliseconds.ToString)
                    gfx.ScaleTransform(1f, 1f, MatrixOrder.Append);

                    //   Disegna guardie
                    if (m_guardlines)
                        Guardlines(gfx, mapOrigin, offset, rect);

                    //   Disegno punti interesse
                    if (m_builds & zoom >= 1f)
                        Buildings(gfx, pntPlayer, mapOrigin, offset, rect);

                    if (m_showlabelbuild)
                    {
                        GraphicsState TransState = gfx.Save();

                        Font string_font = new Font("Arial", 9 / zoom, FontStyle.Regular);
                        SizeF stringsize = gfx.MeasureString(m_labeldescbuild, string_font);

                      
                        Point rectbuild = new Point((m_labelrectbuild.X) - (mapOrigin.X << 3) - offset.X, (m_labelrectbuild.Y) - (mapOrigin.Y << 3) - offset.Y);

                        gfx.TranslateTransform(rectbuild.X, rectbuild.Y);
                        if (m_tiltmap == true)
                            gfx.RotateTransform(-45);

                        Rectangle rett = new Rectangle((int)((-stringsize.Width / 2) - 1 / zoom), (int)((-stringsize.Height / 2) + 4.5f / zoom), (int)(stringsize.Width + 1 / zoom), (int)(stringsize.Height + 1 / zoom));
                        gfx.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.DarkGray)), rett);
                        gfx.DrawString(m_labeldescbuild, string_font, Brushes.Gold, (-stringsize.Width / 2) - 1 / zoom, -1 / zoom);
                        string_font.Dispose();

                        gfx.Restore(TransState);

                        string_font.Dispose();
                    }

                    //   Diesgna mio punto
                    if (m_track_friend == false & m_freeview == false & m_gotolocs == false)
                    {
                        Brush fontPunto = new SolidBrush(m_usercolor);
                        gfx.FillRectangle(fontPunto, (pntPlayer.X - 1f / zoom), (pntPlayer.Y - 1f / zoom), (2 / zoom), (2 / zoom));
                        fontPunto.Dispose();

                        gfx.ResetTransform();
                        gfx.ScaleTransform(zoom, zoom);

                        NameAndBar(gfx, pntPlayer);
                    }
                    else if (m_freeview & m_gotolocs)
                    {
                        Pen _pen = new Pen(Brushes.Silver, 1.2f / zoom);
                        gfx.DrawLine(_pen, (pntPlayer.X - 5f / zoom), (pntPlayer.Y - 5f / zoom), (pntPlayer.X + 5f / zoom), (pntPlayer.Y + 5f / zoom));
                        gfx.DrawLine(_pen, (pntPlayer.X + 5f / zoom), (pntPlayer.Y - 5f / zoom), (pntPlayer.X - 5f / zoom), (pntPlayer.Y + 5f / zoom));

                        gfx.ResetTransform();
                        gfx.ScaleTransform(zoom, zoom);

                        PointF StringPointF = RotatePoint(pntPlayer, new Point(pntPlayer.X, pntPlayer.Y - 1));
                        Font Font = new Font("Arial", 9 / zoom);
                        gfx.DrawString(m_pos_temp.ToString(), Font, Brushes.Gold, ((StringPointF.X - 1) / zoom), (StringPointF.Y / zoom) + 10f / zoom);
                    }
                    //   Disegna coordinate
                    if (m_coordinates)
                        Coordinates(gfx, focus, map__1);

                    gfx.ResetTransform();

                    //   Draw PanicRect
                  //  if (My.Settings.panicadvice == true & Alternate & PanicTimer.Enabled == true)
                   //     PanicRectangle(gfx);
                }

               // LastRefresh_2 = DateTime.Now;
                this.Refresh();

            }
    
	
		protected override void OnPaint(PaintEventArgs e)
		{
            Graphics gfx = e.Graphics;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            gfx.DrawImageUnscaledAndClipped(m_Background, rect);

            if (MapUO.MapNetwork.Connected & MapNetwork.UData != null)
            {
                //  If Connected = True Or (Player.HP = 0 And Player.Shard <> "") Or DropOrPickupMakerToolStripMenuItem.Checked = True Then
                int w = (this.Width) >> 3;
                int h = (this.Height) >> 3;
                int xtrans = this.Width / 2;
                int ytrans = this.Height / 2;
                Point Focus = default(Point);
                if (m_track_friend == false & m_freeview == false & m_gotolocs == false)
                {
                    Focus = new Point(World.Player.Position.X, World.Player.Position.Y);
                }
                else
                {
                    Focus = m_pos_temp;
                }

                Point offset = new Point(Focus.X & 7, Focus.Y & 7);
                Point mapOrigin = new Point((Focus.X >> 3) - (w / 2), (Focus.Y >> 3) - (h / 2));
                Point pntPlayer = new Point((Focus.X) - (mapOrigin.X << 3) - offset.X, (Focus.Y) - (mapOrigin.Y << 3) - offset.Y);

                gfx.TranslateTransform(-pntPlayer.X, -pntPlayer.Y, MatrixOrder.Append);
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gfx.PageUnit = GraphicsUnit.Pixel;
                gfx.ScaleTransform(zoom, zoom, MatrixOrder.Append);
                if (m_tiltmap)
                    gfx.RotateTransform(45, MatrixOrder.Append);
                gfx.TranslateTransform(pntPlayer.X, pntPlayer.Y, MatrixOrder.Append);
                gfx.ScaleTransform(1f, 1f, MatrixOrder.Append);
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                //   Calcolo e disegno 
                foreach (MapNetworkIn.UserData user in MapNetwork.UData)
                {
                    GraphicsState transState = gfx.Save();
                    string nome = user.Nome;
                    if (nome == null | nome.Length < 1)
                        nome = "(Not Seen)";
                    if (nome != null & nome.Length > 8)
                        nome = nome.Substring(0, 8);
                    Point drawPoint = new Point((user.X) - (mapOrigin.X << 3) - offset.X, (user.Y) - (mapOrigin.Y << 3) - offset.Y);
                    PointF drawPointF = RotatePoint(pntPlayer, drawPoint);
                    //   Disegno PG

                    if (drawPointF.X <= -m_offsetbuild.X | drawPointF.X > rect.Width + m_offsetbuild.X | drawPointF.Y <= -m_offsetbuild.Y | drawPointF.Y > rect.Height + m_offsetbuild.Y)
                    {
                        if (nome == "_MARKER_" & m_freeview == true)
                        {
                            pntPlayer = new Point((World.Player.Position.X) - (mapOrigin.X << 3) - offset.X, (World.Player.Position.Y) - (mapOrigin.Y << 3) - offset.Y);
                        }
                        PgOutRange(gfx, pntPlayer, drawPoint, rect, user, drawPointF, nome);
                    }
                    else
                    {
                        if (nome == "_MARKER_" & m_freeview == true)
                        {
                            pntPlayer = new Point((World.Player.Position.X) - (mapOrigin.X << 3) - offset.X, (World.Player.Position.Y) - (mapOrigin.Y << 3) - offset.Y);
                        }
                        PgInRange(gfx, drawPoint, drawPointF, user, nome, pntPlayer);
                    }

                    gfx.ResetTransform();

                    //   Draw PanicLine
                //    if (My.Settings.panicadvice == true & Alternate & PanicTimer.Enabled == true)
                 //       PanicLine(gfx, pntPlayer, drawPointF);

                    gfx.Restore(transState);
                }
            }

            if (m_removeuser)
            {
                SupportFunc.RemoveFakeUser("_DEATH_");
                m_removeuser = false;
            }

            gfx.ResetTransform();

            //   Disegna Zoom
            Font font = new Font("Arial", 9, FontStyle.Bold);

            gfx.DrawString(zoom.ToString() + "x", font, Brushes.Gold, new Point(rect.Width - 35, rect.Height - 20));
		}

		private Point MousePointToMapPoint(Point p)
		{
			double rad = (Math.PI / 180) * 45;
			int w = (this.Width) >> 3;
			int h = (this.Height) >> 3;
			Point3D focus = this.FocusMobile.Position;

			Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));
			Point pnt1 = new Point((mapOrigin.X << 3) + (p.X), (mapOrigin.Y << 3) + (p.Y));
			Point check = new Point(pnt1.X - focus.X, pnt1.Y - focus.Y);
			check = RotatePoint(new Point((int)(check.X * 0.695), (int)(check.Y * 0.68)), rad, 1);
			return new Point(check.X + focus.X, check.Y + focus.Y);
		}

		private Point RotatePoint(Point p, double angle, double dist)
		{
			int x = (int)((p.X * Math.Cos(angle) + p.Y * Math.Sin(angle)) * dist);
			int y = (int)((-p.X * Math.Sin(angle) + p.Y * Math.Cos(angle)) * dist);

			return new Point(x, y);
		}

		protected override void Dispose(bool disposing)
		{
			m_Background.Dispose();
			m_Background = null;
			base.Dispose(disposing);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			FullUpdate();
		}

        internal void UpdateMap()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    UpdateMapCallback d = new UpdateMapCallback(UpdateMap);
                    this.Invoke(d, new object[0]);
                }
                else
                {
                    this.Refresh();
                }
            }
            catch { }
        }

		internal bool Active
		{
			get { return m_Active; }
			set
			{
				m_Active = value;
				if (value)
				{
					FullUpdate();
				}
			}
		}
	}
}
