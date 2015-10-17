using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Assistant.Map
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

		private Ultima.Map m_Map = default(Ultima.Map);
		private int m_CurrentFacet = 9;
		private int m_VarZoom = 0;
		private float m_OffsetZoom = 0;
		internal static float Zoom = 1.5F;
		private Point m_DrawPoint = new Point();
		private Font m_Font = new Font("Arial", 8 / Zoom);
		private Color m_UserColor = Color.Blue;
		private int m_UserFacetTemp = 1;
		private bool m_TrackFriend = false;
		private bool m_TiltMap = true;
		private bool m_RemoveUser = false;
		internal static DateTime m_TimeAfterDeath;
		internal static bool m_BoolDeathPoint;
		private Point m_OffsetBuild;
		private bool m_FreeView = false;
		private bool m_GoToLocs = false;
		private Point m_PosTemp;
		private Point m_Offset;
		private Point m_Actual;
		private Point m_GoToLocPosition;
		private string m_UsernameTemp;
		private bool m_Guardlines = true;
		private bool m_Builds = true;
		private string m_LabelDescBuild;
		private bool m_ShowLabelBuild = true;
		private Rectangle m_LabelRectBuild;
		private bool m_Coordinates = true;
		private float m_ZoomValue;

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

		public UOMapControl()
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

			string name = "";
			if (World.Player != null)
				name = World.Player.Name;

			Font Font = new Font("Arial", 9 / Zoom);
			Brush TextCol = new SolidBrush(m_UserColor);
			int offsetbarre = 14;

			gfx.DrawString(name, Font, Brushes.Black, ((StringPointF.X - 2.8f) / Zoom), ((StringPointF.Y / 1) + 2.5f) / Zoom);
			gfx.DrawString(name, Font, TextCol, ((StringPointF.X - 2.8f) / Zoom), ((StringPointF.Y / 1) + 1f) / Zoom);

			if (RazorEnhanced.Settings.General.ReadBool("MapHpBarCheckBox"))
			{
				Brush status = default(Brush);
				if (World.Player != null)
				{
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
				}

				gfx.FillRectangle(Brushes.Red, (StringPointF.X - 1) / Zoom, (StringPointF.Y + offsetbarre) / Zoom, 35 / Zoom, 3 / Zoom);
				int percent = Convert.ToInt32(World.Player.Hits * 100 / (World.Player.HitsMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(World.Player.HitsMax)));
				float imagepercent = (35 / Zoom) * (percent / 100);
				gfx.FillRectangle(status, (StringPointF.X - 1) / Zoom, (StringPointF.Y + offsetbarre) / Zoom, imagepercent, 3 / Zoom);
				offsetbarre += 4;
			}

			if (RazorEnhanced.Settings.General.ReadBool("MapManaBarCheckBox"))
			{
				gfx.FillRectangle(Brushes.Blue, (StringPointF.X - 1) / Zoom, (StringPointF.Y + offsetbarre) / Zoom, 35 / Zoom, 3 / Zoom);
				int percent = Convert.ToInt32(World.Player.Mana * 100 / (World.Player.ManaMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(World.Player.ManaMax)));
				float imagepercent = (35 / Zoom) * (percent / 100);
				gfx.FillRectangle(Brushes.Blue, (StringPointF.X - 1) / Zoom, (StringPointF.Y + offsetbarre) / Zoom, imagepercent, 3 / Zoom);
				offsetbarre += 4;
			}

			if (RazorEnhanced.Settings.General.ReadBool("MapStaminaBarCheckBox"))
			{
				gfx.FillRectangle(Brushes.Orange, (StringPointF.X - 1) / Zoom, (StringPointF.Y + offsetbarre) / Zoom, 35 / Zoom, 3 / Zoom);
				int percent = Convert.ToInt32(World.Player.Stam * 100 / (World.Player.StamMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(World.Player.StamMax)));
				float imagepercent = (35 / Zoom) * (percent / 100);
				gfx.FillRectangle(Brushes.Orange, (StringPointF.X - 1) / Zoom, (StringPointF.Y + offsetbarre) / Zoom, imagepercent, 3 / Zoom);
				offsetbarre += 4;
			}

			Font.Dispose();
			TextCol.Dispose();
		}


		public void Buildings(Graphics gfx, Point pntPlayer, Point mapOrigin, Point offset, Rectangle rect)
		{
			// Dim x As Stopwatch = Stopwatch.StartNew

			int facet = 0x7F; // internal
			if (World.Player != null)
				facet = World.Player.Map;

			GraphicsState TransState = gfx.Save();

			int index = 0;
			foreach (List<MapIcon.MapIconData> CurrentList in MapIcon.AllListOfBuilds)
			{
				// if (Opzioni.CheckedListBox1.GetItemChecked(index))
				//{
				foreach (MapIcon.MapIconData build in CurrentList)
				{
					if (build.Facet == facet | (build.Facet == 7 & (facet == 1 | facet == 0)))
					{
						Point BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
						//Calcolo Posizione Edificio
						PointF BuildPointF = RotatePoint(pntPlayer, BuildPoint);

						//Se sta nel riquadro della mappa
						if (!(BuildPointF.X <= -m_OffsetBuild.X | BuildPointF.X > rect.Width + m_OffsetBuild.X | BuildPointF.Y <= -m_OffsetBuild.Y | BuildPointF.Y > rect.Height + m_OffsetBuild.Y))
						{

							GraphicsState transState2 = gfx.Save();
							gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
							//traslo sul punto dell'edificio
							if (m_TiltMap)
								gfx.RotateTransform(-45);
							Image Icon = MapIcon.IconImage[build.Icon];

							gfx.DrawImage(Icon, (-Icon.Width / 2) / Zoom, (-Icon.Height / 2) / Zoom, Icon.Width / Zoom, Icon.Height / Zoom);
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

		internal void PgOutRange(Graphics gfx, Point pntPlayer, Point drawPoint, Rectangle rect, MapNetworkIn.UserData user, PointF drawPointF, string name)
		{
			gfx.ResetTransform();

			List<Point> intersections = Geometry2.SegmentToRectangleIntersect(pntPlayer, new Point((int)drawPointF.X, (int)drawPointF.Y), rect, Geometry2.SegmentIntersection.Point);

			foreach (Point pt in intersections)
			{
				SizeF stringsize = gfx.MeasureString(name, m_Font);
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

				if (name == "_DEATH_")
				{
					gfx.DrawLine(new Pen(Brushes.White, 0), pntPlayer, p_);
					gfx.DrawImage(Properties.Resources.map_deadOut, p_.X, p_.Y, 14, 15);
				}
				else if (name == "_MARKER_")
				{
					gfx.DrawLine(Pens.White, pntPlayer, p_);
				}
				else
				{
					int facet = 0x7F; // internal

					if (World.Player != null)
					{
						if (m_TrackFriend == false)
						{
							facet = World.Player.Map;
						}
						else
						{
							facet = m_UserFacetTemp;
						}
					}

					SolidBrush status = default(SolidBrush);
					if (user.Facet != facet)
					{
						status = new SolidBrush(Color.DarkGray);
					}
					else
					{
						status = new SolidBrush(m_UserColor);
					}

					gfx.FillRectangle(new SolidBrush(status.Color), Geometry2.GetRectangleAt(pt, 1));
					gfx.DrawString(name, m_Font, new SolidBrush(Color.Black), p_.X, p_.Y + 2.5f);
					gfx.DrawString(name, m_Font, new SolidBrush(m_UserColor), p_.X, p_.Y + 1f);
				}
			}
		}


		public void PgInRange(Graphics gfx, Point drawPoint, PointF drawPointF, MapNetworkIn.UserData user, string name, Point pntPlayer)
		{
			if (name == "_MARKER_")
			{
				Pen pen_ = new Pen(Brushes.White, 1.2f / Zoom);
				pen_.DashStyle = DashStyle.Dot;
				gfx.DrawLine(pen_, pntPlayer, drawPoint);
				gfx.FillEllipse(Brushes.White, drawPoint.X - 2.5f / Zoom, drawPoint.Y - 2.5f / Zoom, 5 / Zoom, 5 / Zoom);
			}
			else if (name == "_DEATH_")
			{
				if (((DateTime.Now - m_TimeAfterDeath).TotalMinutes >= 5 & World.Player.Hits > 0) | World.Player.IsGhost)
				{
					if (user.Name == "_DEATH_")
					{
						m_RemoveUser = true;
						//  ImDead = false;
						// RemoveDeathPointToolStripMenuItem.Enabled = false;
					}
				}
				else
				{
					gfx.DrawLine(new Pen(Brushes.White, 0), pntPlayer, new Point(drawPoint.X + 3, drawPoint.Y + 5));
					gfx.TranslateTransform(drawPoint.X, drawPoint.Y);
					if (m_TiltMap)
						gfx.RotateTransform(-45);
					gfx.DrawImage(Properties.Resources.map_deadIn, -6 / Zoom, 0, 14 / Zoom, 15 / Zoom);
				}
			}
			else
			{
				Font font = new Font("Arial", 9 / Zoom);
				Brush textCol = new SolidBrush(m_UserColor);
				gfx.FillRectangle(textCol, drawPoint.X - 1 / Zoom, drawPoint.Y - 1 / Zoom, 2f / Zoom, 2f / Zoom);
				gfx.TranslateTransform(drawPoint.X, drawPoint.Y);
				if (m_TiltMap)
					gfx.RotateTransform(-45);

				gfx.DrawString(name, font, Brushes.Black, -1f / Zoom, 2f / Zoom);
				gfx.DrawString(name, font, textCol, -1f / Zoom, 0.5f / Zoom);
				font.Dispose();
				textCol.Dispose();
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


					gfx.FillRectangle(Brushes.Red, -1 / Zoom, offsetbarre / Zoom, 35 / Zoom, 3 / Zoom);
					int percent = Convert.ToInt32(user.Hits * 100 / (user.HitsMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.HitsMax)));
					float imagepercent = (35 / Zoom) * (percent / 100);
					gfx.FillRectangle(status, -1 / Zoom, offsetbarre / Zoom, imagepercent, 3 / Zoom);
					offsetbarre += 4;
				}
				if (RazorEnhanced.Settings.General.ReadBool("MapManaBarCheckBox"))
				{
					gfx.FillRectangle(Brushes.Blue, -1 / Zoom, offsetbarre / Zoom, 35 / Zoom, 3 / Zoom);
					int percent = Convert.ToInt32(user.Mana * 100 / (user.ManaMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.ManaMax)));
					float imagepercent = (35 / Zoom) * (percent / 100);
					gfx.FillRectangle(Brushes.Blue, -1 / Zoom, offsetbarre / Zoom, imagepercent, 3 / Zoom);
					offsetbarre += 4;
				}

				if (RazorEnhanced.Settings.General.ReadBool("MapStaminaBarCheckBox"))
				{
					gfx.FillRectangle(Brushes.Orange, -1 / Zoom, offsetbarre / Zoom, 35 / Zoom, 3 / Zoom);
					int percent = Convert.ToInt32(user.Stamina * 100 / (user.StaminaMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.StaminaMax)));
					float imagepercent = (35 / Zoom) * (percent / 100);
					gfx.FillRectangle(Brushes.Orange, -1 / Zoom, offsetbarre / Zoom, imagepercent, 3 / Zoom);
					offsetbarre += 4;
				}
			}
		}

		public void Coordinates(Graphics gfx, Point focus, Ultima.Map map)
		{
			int xLong = 0;
			int yLat = 0;
			int xMins = 0;
			int yMins = 0;
			bool xEast = false;
			bool ySouth = false;
			string locString = null;

			locString = String.Format("{0}°{1}'{2} {3}°{4}'{5} | (X: {6}, Y: {7})", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W", focus.X, focus.Y);

			string name = " -- ";
			if (World.Player != null)
				name = World.Player.Name;

			this.Text = "UOAM2 - " + name + " - [" + locString + "]";
		}

		public void Guardlines(Graphics gfx, Point mapOrigin, Point offset, Rectangle rect)
		{
			int Facet = 0x7F; // internal
			if (World.Player != null)
				Facet = World.Player.Map;

			Pen pen = new Pen(Brushes.Green, 0);
			Rectangle RegionPoint = default(Rectangle);

			foreach (Region region in Map.Region.RegionLists)
			{
				RegionPoint = new Rectangle((region.X) - (mapOrigin.X << 3) - offset.X, (region.Y) - (mapOrigin.Y << 3) - offset.Y, region.Width, region.Height);
				if (!(RegionPoint.X <= -m_OffsetBuild.X | RegionPoint.X > rect.Width + m_OffsetBuild.X | RegionPoint.Y <= -m_OffsetBuild.Y | RegionPoint.Y > rect.Height + m_OffsetBuild.Y) & region.Facet == Facet)
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
					if (Zoom > 0.5f)
					{
						Zoom = Zoom - 0.5f;
					}
				}
				else
				{
					if (Zoom < 4f)
					{
						Zoom = Zoom + 0.5f;
					}
				}
				ZoomLevel();
				m_ZoomValue = Zoom;
				FullUpdate(true);
			}
		}


		public void ZoomLevel()
		{
			Size PicDim = new Size(this.Width, this.Height);


			if (Zoom == 0.5f)
			{
				m_VarZoom = 720;
				m_OffsetZoom = 180;
			}
			else if (Zoom == 1f)
			{
				m_VarZoom = 400;
				m_OffsetZoom = 80;
			}
			else if (Zoom == 1.5f)
			{
				m_VarZoom = 200;
				m_OffsetZoom = 40;
			}
			else if (Zoom == 2f)
			{
				m_VarZoom = 120;
				m_OffsetZoom = 30;
			}
			else if (Zoom == 2.5f)
			{
				m_VarZoom = 80;
				m_OffsetZoom = 15;
			}
			else if (Zoom == 3f)
			{
				m_VarZoom = 40;
				m_OffsetZoom = 7.5f;
			}
			else if (Zoom == 3.5f)
			{
				m_VarZoom = 40;
				m_OffsetZoom = 7.5f;
			}
			else if (Zoom == 4f)
			{
				m_VarZoom = 0;
				m_OffsetZoom = 0;
			}

			m_OffsetBuild = new Point(PicDim.Width * 2 / 350, PicDim.Height * 2 / 350);

			if (m_Map != null)
				SupportFunc.ClearCache(m_Map);
		}

		internal void picturebox1_DoubleClick(object sender, EventArgs e)
		{
			if (MapWindow.MapWindowForm.FormBorderStyle == FormBorderStyle.None)
			{
				MapWindow.MapWindowForm.FormBorderStyle = FormBorderStyle.Sizable;
				MapWindow.MapWindowForm.TopMost = false;
			}
			else
			{
				MapWindow.MapWindowForm.FormBorderStyle = FormBorderStyle.None;
				MapWindow.MapWindowForm.TopMost = true;

			}
			FullUpdate(true);
		}

		internal void PictureBox1_MouseClick(System.Object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				MapWindow.MapContextMenu.Visible = true;
				MapWindow.MapContextMenu.Location = Cursor.Position;
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

			if (World.Player != null)
				PositionPg = new Point(World.Player.Position.X, World.Player.Position.Y);

			Point focus = default(Point);
			if (m_FreeView == false & m_TrackFriend == false & m_GoToLocs == false)
			{
				focus = new Point(PositionPg.X, PositionPg.Y);
			}
			else
			{
				//   FreeView
				if (m_FreeView)
				{
					if (m_TrackFriend)
						m_TrackFriend = false;
					if (MapNetwork.UData != null)
					{
						if (m_PosTemp.X == PositionPg.X & m_PosTemp.Y == PositionPg.Y)
						{
							focus = new Point(PositionPg.X + m_Actual.X, PositionPg.Y + m_Actual.Y);
						}
						else
						{
							focus = new Point(m_PosTemp.X + m_Actual.X, m_PosTemp.Y + m_Actual.Y);
						}
						m_PosTemp = focus;
						m_Actual.X = m_Actual.Y = 0;
						if (World.Player.Poisoned)
							SupportFunc.AddMyUser(1);
						else
							SupportFunc.AddMyUser(0);
					}

				}

				//   Tracking
				if (m_TrackFriend)
				{
					//if (FreeViewToolStripMenuItem.Enabled == true)
					//   FreeViewToolStripMenuItem.Enabled = false;

					if (MapNetwork.UData != null)
					{
						foreach (MapNetworkIn.UserData user in MapNetwork.UData)
						{
							if (user.Name == m_UsernameTemp)
							{
								m_PosTemp = new Point(user.X, user.Y);
							}
						}
						focus = m_PosTemp;
						//Pos_temp = New Point(focus.X, focus.Y)
						if (World.Player.Poisoned)
							SupportFunc.AddMyUser(1);
						else
							SupportFunc.AddMyUser(0);
					}
				}
			}

			//   GotoLoc
			if (m_GoToLocs)
			{
				if (MapNetwork.UData != null)
				{
					m_PosTemp = m_GoToLocPosition;
					focus = new Point(m_PosTemp.X, m_PosTemp.Y);
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
			if (World.Player != null && World.Player.HitsMax != 0)
			{
				if (m_TrackFriend == false)
				{
					Facet = World.Player.Map;
				}
				else
				{
					Facet = m_UserFacetTemp;
				}
			}
			else
			{
				Facet = 10;
			}

			if (Facet != m_CurrentFacet)
			{
				// map__1 = Nothing
				if (m_Map != null)
					SupportFunc.ClearCache(m_Map);
				m_Map = SupportFunc.GetMap(Facet);
				if (m_Map == null)
					m_Map = Ultima.Map.InitializeMap("Felucca");
				m_CurrentFacet = Facet;
			}

			m_Background = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

			using (Graphics gfx = Graphics.FromImage(m_Background))
			{
				gfx.SmoothingMode = SmoothingMode.AntiAlias;
				gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				gfx.TranslateTransform(-pntPlayer.X, -pntPlayer.Y, MatrixOrder.Append);
				if (Zoom >= 1f)
				{
					gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				}
				else
				{
					gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
				}
				gfx.PageUnit = GraphicsUnit.Pixel;
				gfx.ScaleTransform(Zoom, Zoom, MatrixOrder.Append);
				if (m_TiltMap)
					gfx.RotateTransform(45, MatrixOrder.Append);
				gfx.TranslateTransform(pntPlayer.X, pntPlayer.Y, MatrixOrder.Append);

				//   Disegna mappa
				gfx.DrawImage(m_Map.GetImage(mapOrigin.X - (m_VarZoom >> 3), mapOrigin.Y - (m_VarZoom >> 3), (int)(w + offset.X + m_OffsetZoom), (int)(h + offset.Y + m_OffsetZoom), true), -offset.X - m_VarZoom, -offset.Y - m_VarZoom);
				//  Opzioni.log("Map:" + x.ElapsedMilliseconds.ToString)
				gfx.ScaleTransform(1f, 1f, MatrixOrder.Append);

				//   Disegna guardie
				if (m_Guardlines)
					Guardlines(gfx, mapOrigin, offset, rect);

				//   Disegno punti interesse
				if (m_Builds & Zoom >= 1f)
					Buildings(gfx, pntPlayer, mapOrigin, offset, rect);

				if (m_ShowLabelBuild)
				{
					GraphicsState TransState = gfx.Save();

					Font string_font = new Font("Arial", 9 / Zoom, FontStyle.Regular);
					SizeF stringsize = gfx.MeasureString(m_LabelDescBuild, string_font);


					Point rectbuild = new Point((m_LabelRectBuild.X) - (mapOrigin.X << 3) - offset.X, (m_LabelRectBuild.Y) - (mapOrigin.Y << 3) - offset.Y);

					gfx.TranslateTransform(rectbuild.X, rectbuild.Y);
					if (m_TiltMap == true)
						gfx.RotateTransform(-45);

					Rectangle rett = new Rectangle((int)((-stringsize.Width / 2) - 1 / Zoom), (int)((-stringsize.Height / 2) + 4.5f / Zoom), (int)(stringsize.Width + 1 / Zoom), (int)(stringsize.Height + 1 / Zoom));
					gfx.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.DarkGray)), rett);
					gfx.DrawString(m_LabelDescBuild, string_font, Brushes.Gold, (-stringsize.Width / 2) - 1 / Zoom, -1 / Zoom);
					string_font.Dispose();

					gfx.Restore(TransState);

					string_font.Dispose();
				}

				//   Diesgna mio punto
				if (m_TrackFriend == false & m_FreeView == false & m_GoToLocs == false)
				{
					Brush fontPunto = new SolidBrush(m_UserColor);
					gfx.FillRectangle(fontPunto, (pntPlayer.X - 1f / Zoom), (pntPlayer.Y - 1f / Zoom), (2 / Zoom), (2 / Zoom));
					fontPunto.Dispose();

					gfx.ResetTransform();
					gfx.ScaleTransform(Zoom, Zoom);

					NameAndBar(gfx, pntPlayer);
				}
				else if (m_FreeView & m_GoToLocs)
				{
					Pen _pen = new Pen(Brushes.Silver, 1.2f / Zoom);
					gfx.DrawLine(_pen, (pntPlayer.X - 5f / Zoom), (pntPlayer.Y - 5f / Zoom), (pntPlayer.X + 5f / Zoom), (pntPlayer.Y + 5f / Zoom));
					gfx.DrawLine(_pen, (pntPlayer.X + 5f / Zoom), (pntPlayer.Y - 5f / Zoom), (pntPlayer.X - 5f / Zoom), (pntPlayer.Y + 5f / Zoom));

					gfx.ResetTransform();
					gfx.ScaleTransform(Zoom, Zoom);

					PointF StringPointF = RotatePoint(pntPlayer, new Point(pntPlayer.X, pntPlayer.Y - 1));
					Font Font = new Font("Arial", 9 / Zoom);
					gfx.DrawString(m_PosTemp.ToString(), Font, Brushes.Gold, ((StringPointF.X - 1) / Zoom), (StringPointF.Y / Zoom) + 10f / Zoom);
				}
				//   Disegna coordinate
				if (m_Coordinates)
					Coordinates(gfx, focus, m_Map);

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

			if (MapNetwork.Connected & MapNetwork.UData != null)
			{
				//  If Connected = True Or (Player.HP = 0 And Player.Shard <> "") Or DropOrPickupMakerToolStripMenuItem.Checked = True Then
				int w = (this.Width) >> 3;
				int h = (this.Height) >> 3;
				int xtrans = this.Width / 2;
				int ytrans = this.Height / 2;
				Point Focus = default(Point);
				if (m_TrackFriend == false & m_FreeView == false & m_GoToLocs == false)
				{
					Focus = new Point(World.Player.Position.X, World.Player.Position.Y);
				}
				else
				{
					Focus = m_PosTemp;
				}

				Point offset = new Point(Focus.X & 7, Focus.Y & 7);
				Point mapOrigin = new Point((Focus.X >> 3) - (w / 2), (Focus.Y >> 3) - (h / 2));
				Point pntPlayer = new Point((Focus.X) - (mapOrigin.X << 3) - offset.X, (Focus.Y) - (mapOrigin.Y << 3) - offset.Y);

				gfx.TranslateTransform(-pntPlayer.X, -pntPlayer.Y, MatrixOrder.Append);
				gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				gfx.PageUnit = GraphicsUnit.Pixel;
				gfx.ScaleTransform(Zoom, Zoom, MatrixOrder.Append);
				if (m_TiltMap)
					gfx.RotateTransform(45, MatrixOrder.Append);
				gfx.TranslateTransform(pntPlayer.X, pntPlayer.Y, MatrixOrder.Append);
				gfx.ScaleTransform(1f, 1f, MatrixOrder.Append);
				gfx.SmoothingMode = SmoothingMode.AntiAlias;
				gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

				//   Calcolo e disegno 
				foreach (MapNetworkIn.UserData user in MapNetwork.UData)
				{
					GraphicsState transState = gfx.Save();
					string nome = user.Name;
					if (nome == null | nome.Length < 1)
						nome = "(Not Seen)";
					if (nome != null & nome.Length > 8)
						nome = nome.Substring(0, 8);
					Point drawPoint = new Point((user.X) - (mapOrigin.X << 3) - offset.X, (user.Y) - (mapOrigin.Y << 3) - offset.Y);
					PointF drawPointF = RotatePoint(pntPlayer, drawPoint);
					//   Disegno PG

					if (drawPointF.X <= -m_OffsetBuild.X | drawPointF.X > rect.Width + m_OffsetBuild.X | drawPointF.Y <= -m_OffsetBuild.Y | drawPointF.Y > rect.Height + m_OffsetBuild.Y)
					{
						if (nome == "_MARKER_" & m_FreeView == true)
						{
							pntPlayer = new Point((World.Player.Position.X) - (mapOrigin.X << 3) - offset.X, (World.Player.Position.Y) - (mapOrigin.Y << 3) - offset.Y);
						}
						PgOutRange(gfx, pntPlayer, drawPoint, rect, user, drawPointF, nome);
					}
					else
					{
						if (nome == "_MARKER_" & m_FreeView == true)
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

			if (m_RemoveUser)
			{
				SupportFunc.RemoveFakeUser("_DEATH_");
				m_RemoveUser = false;
			}

			gfx.ResetTransform();

			//   Disegna Zoom
			Font font = new Font("Arial", 9, FontStyle.Bold);

			gfx.DrawString(Zoom.ToString() + "x", font, Brushes.Gold, new Point(rect.Width - 35, rect.Height - 20));
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
