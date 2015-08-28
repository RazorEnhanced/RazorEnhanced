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
    public class Geometry2
    {

        public enum SegmentIntersection
        {
            None = 0,
            // The segments are parallel and will never intersect
            Point = 1,
            // The segments physically intersect in one point
            ExtrapolatedPoint = 2,
            // The segments would physically intersect in one point if one or both segments were extended
            Overlapping = 3
            // The segments are parallel and overlap in a point or segment
        }

        public static SegmentIntersection SegmentIntersect(Point A, Point B, Point C, Point D, Point E, Point F)
        {

            // If one or both of the segments passed in is actually a point then just do a PointToSegmentDistance() calculation:
            if (A.Equals(B) || C.Equals(D))
            {
                if (A.Equals(B) && C.Equals(D))
                {
                    if (A.Equals(C))
                    {
                        E = A;
                        F = A;
                        return Geometry2.SegmentIntersection.Point;
                    }
                    else
                    {
                        return Geometry2.SegmentIntersection.None;
                    }
                }
                else if (A.Equals(B))
                {
                    if (Geometry2.PointToSegmentDistance(A.X, A.Y, C.X, C.Y, D.X, D.Y) == 0)
                    {
                        E = A;
                        F = A;
                        return Geometry2.SegmentIntersection.Point;
                    }
                }
                else if (C.Equals(D))
                {
                    if (Geometry2.PointToSegmentDistance(C.X, C.Y, A.X, A.Y, B.X, B.Y) == 0)
                    {
                        E = C;
                        F = C;
                        return Geometry2.SegmentIntersection.Point;
                    }
                }
                return Geometry2.SegmentIntersection.None;
            }

            //  We have two actual segments...let's do the calculations for Det1 and Det2:
            double Det1 = (A.Y - C.Y) * (D.X - C.X) - (A.X - C.X) * (D.Y - C.Y);
            double Det2 = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);

            // Non-Parallel Segments (they intersect or would intersect if extended)
            if (Det2 != 0)
            {
                double Det3 = (A.Y - C.Y) * (B.X - A.X) - (A.X - C.X) * (B.Y - A.Y);
                double Det4 = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);

                double r = Det1 / Det2;
                double s = Det3 / Det4;

                //   Compute the intersection point:
                E.X = (int)(A.X + r * (B.X - A.X));
                E.Y = (int)(A.Y + r * (B.Y - A.Y));
                F = E;

                if ((r >= 0 && r <= 1) && (s >= 0 && s <= 1))
                {
                    //       They physically intersect
                    return Geometry2.SegmentIntersection.Point;
                }
                else
                {
                    //      They would physically intersect if one or both segments were extended
                    return Geometry2.SegmentIntersection.ExtrapolatedPoint;
                }
                // Parallel Segments
            }
            else
            {
                // Non-Overlapping
                if (Det1 != 0)
                {
                    return Geometry2.SegmentIntersection.None;
                    // Overlapping (one point or a segment)
                }
                else
                {
                    //The parallel segments are the same
                    if ((A.Equals(C) && B.Equals(D)) || (A.Equals(D) && B.Equals(C)))
                    {
                        E = A;
                        F = B;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }

                    //The parallel segments overlap in exactly one point
                    if (B.Equals(C) || B.Equals(D))
                    {
                        E = B;
                        F = B;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    if (A.Equals(C) || A.Equals(D))
                    {
                        E = A;
                        F = A;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }

                    //   The parallel segments are overlapping in a segment
                    if (Geometry2.SegmentContainsPoint(A, B, C) && Geometry2.SegmentContainsPoint(C, D, B))
                    {
                        E = C;
                        F = B;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(A, B, D) && Geometry2.SegmentContainsPoint(D, C, B))
                    {
                        E = D;
                        F = B;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(B, A, C) && Geometry2.SegmentContainsPoint(C, D, A))
                    {
                        E = C;
                        F = A;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(B, A, D) && Geometry2.SegmentContainsPoint(D, C, A))
                    {
                        E = D;
                        F = A;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(C, D, A) && Geometry2.SegmentContainsPoint(A, B, D))
                    {
                        E = A;
                        F = D;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(C, D, B) && Geometry2.SegmentContainsPoint(B, A, D))
                    {
                        E = B;
                        F = D;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(D, C, A) && Geometry2.SegmentContainsPoint(A, B, C))
                    {
                        E = A;
                        F = C;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    else if (Geometry2.SegmentContainsPoint(D, C, B) && Geometry2.SegmentContainsPoint(B, A, C))
                    {
                        E = B;
                        F = C;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }

                    // One segment completely contains the other
                    if (Geometry2.SegmentContainsPoint(A, B, C) && Geometry2.SegmentContainsPoint(A, B, D))
                    {
                        E = C;
                        F = D;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }
                    if (Geometry2.SegmentContainsPoint(C, D, A) && Geometry2.SegmentContainsPoint(C, D, B))
                    {
                        E = A;
                        F = B;
                        return Geometry2.SegmentIntersection.Overlapping;
                    }

                    //  Segments are parallel but not touching
                    return Geometry2.SegmentIntersection.None;
                }
            }
        }

        public static float PointToPointDistance(float Ax, float Ay, float Bx, float By)
        {
            //  PointToPointDist = SquareRoot((Bx - Ax) ^ 2 + (By - Ay) ^ 2)
            return (float)Math.Sqrt(((Bx - Ax) * (Bx - Ax) + (By - Ay) * (By - Ay)));
        }

        public static float PointToSegmentDistance(float Px, float Py, float Ax, float Ay, float Bx, float By)
        {
            float q = 0;

            if ((Ax == Bx) & (Ay == By))
            {
                //    A and B passed in define a point, not a line.
                //   Point to Point Distance
                return PointToPointDistance(Px, Py, Ax, Ay);
            }
            else
            {
                //    Distance is the length of the line needed to connect the point to
                //  the(segment)such that the two lines would be perpendicular.

                //  q is the parameterized value needed to get to the intersection
                q = ((Px - Ax) * (Bx - Ax) + (Py - Ay) * (By - Ay)) / ((Bx - Ax) * (Bx - Ax) + (By - Ay) * (By - Ay));

                //   Limit q to 0 <= q <= 1
                //  If q is outside this range then the Point is somewhere past the 
                // endpoints of our segment.  By setting q = 0 or q = 1 we are 
                // measuring the actual distacne from the point to one of the 
                // endpoints(instead)
                if (q < 0)
                    q = 0;
                if (q > 1)
                    q = 1;

                //Distance()
                return PointToPointDistance(Px, Py, (1 - q) * Ax + q * Bx, (1 - q) * Ay + q * By);
            }
        }

        public static bool SegmentContainsPoint(Point A, Point B, Point C)
        {
            //Two Segments AB and CD have already been determined to have the 
            //same slope and that they overlap.
            //AB is the segment, and C is the point in question.
            //If AB contains C then return true, otherwise return false
            if (C.Equals(A) | C.Equals(B))
            {
                return true;
                // Project to the Y-Axis for vertical lines
            }
            else if (A.X == B.X)
            {
                int minY = Math.Min(A.Y, B.Y);
                int maxY = Math.Max(A.Y, B.Y);
                if (minY <= C.Y && C.Y <= maxY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                // Project to the X-Axis for anything else
            }
            else
            {
                int minX = Math.Min(A.X, B.X);
                int maxX = Math.Max(A.X, B.X);
                if (minX <= C.X && C.X <= maxX)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }

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
        private int OffsetZoom = 0;
        internal static float zoom = 1.5F;
        private Point drawPoint = new Point();
        private Font m_Font = new Font("Arial", 8 / zoom);

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

		private static Font m_BoldFont = new Font("Courier New", 8, FontStyle.Bold);
		private static Font m_SmallFont = new Font("Arial", 6);
		private static Font m_RegFont = new Font("Arial", 8);
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
        private void NameAndBar(Graphics gfx, Point pntPlayer)
        {
            PointF StringPointF = RotatePoint(pntPlayer, new Point(pntPlayer.X, pntPlayer.Y - 1));
            string Nome = World.Player.Name;
            Font Font = new Font("Arial", 9 / zoom);
            Brush TextCol = new SolidBrush(Color.Blue);

            gfx.DrawString(Nome, Font, Brushes.Black, ((StringPointF.X - 2.8f) / zoom), (StringPointF.Y / zoom) + 2.5f / zoom);
            gfx.DrawString(Nome, Font, TextCol, ((StringPointF.X - 2.8f) / zoom), (StringPointF.Y / zoom) + 1f / zoom);
 
            int HP = World.Player.Hits;
            int MaxHP = World.Player.HitsMax;
            int offsetbarre = 14;
            gfx.FillRectangle(Brushes.Red, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, 35 / zoom, 3 / zoom);
            int percent = Convert.ToInt32(HP * 100 / (MaxHP == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(MaxHP)));
            float imagepercent = (35 / zoom) * (percent / 100);
            gfx.FillRectangle(Brushes.Yellow, (StringPointF.X - 1) / zoom, (StringPointF.Y + offsetbarre) / zoom, imagepercent, 3 / zoom);
            offsetbarre += 4;            
        }

        private void Buildings(Graphics gfx, Point pntPlayer, Point mapOrigin, Point offset, Rectangle rect)
        {
            int Facet = World.Player.Map;
            Point BuildPoint = default(Point);
            PointF BuildPointF = default(PointF);
            GraphicsState TransState = gfx.Save();

//           if (Opzioni.CheckedListBox1.GetItemChecked(0) == true)
      //      {
                foreach (MapIcon.MapIconData build in MapIcon.IconAtlasList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
                }
        //    }
//
          //  if (Opzioni.CheckedListBox1.GetItemChecked(1) == true)
       //     {

                foreach (MapIcon.MapIconData build in MapIcon.IconcommonList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                       if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
               }
         //   }

           // if (Opzioni.CheckedListBox1.GetItemChecked(2) == true)
           // {
                foreach (MapIcon.MapIconData build in MapIcon.IconDungeonsList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
                //}
            }

          //  if (Opzioni.CheckedListBox1.GetItemChecked(3) == true)
          //  {
                foreach (MapIcon.MapIconData build in MapIcon.IconMLList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
               // }
            }

           // if (Opzioni.CheckedListBox1.GetItemChecked(4) == true)
           // {
                foreach (MapIcon.MapIconData build in MapIcon.IconNewHavenList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
              //  }
            }

      //      if (Opzioni.CheckedListBox1.GetItemChecked(5) == true)
           // {
                foreach (MapIcon.MapIconData build in MapIcon.IconOldHavenList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
                //}
            }

          //  if (Opzioni.CheckedListBox1.GetItemChecked(6) == true)
         //   {
                foreach (MapIcon.MapIconData build in MapIcon.IconPersonalList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
              //  }
            }

         //   if (Opzioni.CheckedListBox1.GetItemChecked(7) == true)
        //    {
                foreach (MapIcon.MapIconData build in MapIcon.IconRaresList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
               //}
            }

            //if (Opzioni.CheckedListBox1.GetItemChecked(8) == true)
        //    {
                foreach (MapIcon.MapIconData build in MapIcon.IconStealablesList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon =  MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
                //}
            }

      //      if (Opzioni.CheckedListBox1.GetItemChecked(9) == true)
         //   {
                foreach (MapIcon.MapIconData build in MapIcon.IconTokunoIslandsList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
               // }
            }

           // if (Opzioni.CheckedListBox1.GetItemChecked(10) == true)
          //  {
                foreach (MapIcon.MapIconData build in MapIcon.IconTreasureList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon =MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
              //}
            }

           // if (Opzioni.CheckedListBox1.GetItemChecked(11) == true)
         //   {
                foreach (MapIcon.MapIconData build in MapIcon.IconTreasurePFList)
                {
                    if (build.Facet == Facet | (build.Facet == 7 & (Facet == 1 | Facet == 0)))
                    {
                        BuildPoint = new Point((build.X) - (mapOrigin.X << 3) - offset.X, (build.Y) - (mapOrigin.Y << 3) - offset.Y);
                        //Calcolo Posizione Edificio
                        BuildPointF = RotatePoint(pntPlayer, BuildPoint);
                        //Se sta nel riquadro della mappa
                        if (!(BuildPointF.X <= 0 | BuildPointF.X > rect.Width | BuildPointF.Y <= 0 | BuildPointF.Y > rect.Height))
                        {
                            GraphicsState transState2 = gfx.Save();
                            gfx.TranslateTransform(BuildPoint.X, BuildPoint.Y);
                            //traslo sul punto dell'edificio
                            gfx.RotateTransform(-45);
                            Image Icon = MapIcon.IconImage[build.Icon];
                            gfx.DrawImage(Icon, -5f / zoom, -12f / zoom, Icon.Width / zoom, Icon.Height / zoom);
                            //disegno
                            gfx.Restore(transState2);
                            //reimposto la configurazione della grafica come prima
                        }
                    }
               // }
            }

            gfx.Restore(TransState);
        }


        public void PgOutRange(Graphics gfx, Point pntPlayer, Point drawPoint, Rectangle rect, MapNetworkIn.UserData user, PointF drawPointF, string nome)
        {
            gfx.ResetTransform();
            List<Point> intersections = SegmentToRectangleIntersect(pntPlayer, new Point((int)drawPointF.X, (int)drawPointF.Y), rect, Geometry2.SegmentIntersection.Point);
            foreach (Point pt in intersections)
            {
                SizeF stringsize = gfx.MeasureString(nome, m_Font);

                if (drawPointF.X < 0)
                {
                    drawPointF.X = pt.X;
                    // + stringsize.Width
                }
                if (drawPointF.Y < 0)
                {
                    drawPointF.X = pt.X - stringsize.Width;
                    drawPointF.Y = pt.Y;
                }
                if (drawPointF.X < 0 & drawPointF.Y < 0)
                {
                    drawPointF.X = pt.X - stringsize.Width;
                    drawPointF.Y = pt.Y;
                    //+ stringsize.Height
                }
                if (drawPointF.X > rect.Width)
                {
                    drawPointF.X = pt.X - stringsize.Width;
                    drawPointF.Y = pt.Y - stringsize.Height;
                }
                if (drawPointF.Y > rect.Height)
                {
                    drawPointF.X = pt.X;
                    drawPointF.Y = pt.Y - stringsize.Height;
                }

                /*   if (nome == "_MORTO_")
                   {
                       if (My.Settings.track_death_point == true)
                       {
                           gfx.DrawLine(new Pen(Brushes.White, 0), pntPlayer, drawPointF);
                       }
                       gfx.DrawImage(My.Resources.map_dead, drawPointF.X, drawPointF.Y, 14, 15);
                   }*/
                //  else
                //   {

                SolidBrush stato = default(SolidBrush);
                stato = new SolidBrush(Color.DarkGray);

                //     Dim rettangolo As Rectangle = GetRectangleAt(pt, 1)
                gfx.FillRectangle(new SolidBrush(stato.Color), drawPointF.X, drawPointF.Y, 1, 1);
                gfx.DrawString(nome, m_Font, new SolidBrush(Color.Black), drawPointF.X, drawPointF.Y + 2.5f);
                gfx.DrawString(nome, m_Font, new SolidBrush(stato.Color), drawPointF.X, drawPointF.Y + 1f);

                // }
            }
        }

        public void PgInRange(Graphics gfx, Point drawPoint, PointF drawPointF, MapNetworkIn.UserData user, string nome)
        {
            if (nome != "_MORTO_")
            {
                Font Font = new Font("Arial", 8 / zoom);
                Brush TextCol = new SolidBrush(Color.Blue);
                gfx.FillRectangle(TextCol, drawPoint.X - 1 / zoom, drawPoint.Y - 1 / zoom, 2f / zoom, 2f / zoom);
                gfx.TranslateTransform(drawPoint.X, drawPoint.Y);
                gfx.RotateTransform(-45);

                gfx.DrawString(nome, Font, Brushes.Black, -1f / zoom, 2f / zoom);
                gfx.DrawString(nome, Font, TextCol, -1f / zoom, 0.5f / zoom);

                Font.Dispose();
                TextCol.Dispose();


                Brush status = default(Brush);
                if (user.Flag == 1)
                {
                    status = Brushes.LimeGreen;
                }
                else if (user.Flag == 2)
                {
                    status = Brushes.Yellow;
                }
                else if (user.Flag == 3)
                {
                    status = Brushes.AliceBlue;
                }
                else if (user.Flag == 4)
                {
                    //  gfx.DrawImage(My.Resources.map_dead, drawPointF.X - 4, drawPointF.Y + 8, 14, 15)
                    status = Brushes.SteelBlue;
                }
                else
                {
                    status = Brushes.SteelBlue;
                }

                int offsetbarre = 14;
                gfx.FillRectangle(Brushes.Red, -1 / zoom, offsetbarre / zoom, 35 / zoom, 3 / zoom);
                int percent = Convert.ToInt32(user.Hits * 100 / (user.HitsMax == 0 ? Convert.ToUInt16(1) : Convert.ToUInt16(user.HitsMax)));
                float imagepercent = (35 / zoom) * (percent / 100);
                gfx.FillRectangle(status, -1 / zoom, offsetbarre / zoom, imagepercent, 3 / zoom);
                offsetbarre += 4;

            }
            else
            {
                // Disegna lapide del mio pg
                //    if (Player.HP > 0)
                //   {
                /*  if (user.nome == "_MORTO_")
                  {
                      int index = 0;
                      foreach (Users.Data.UserData data2 in userlist.ToList)
                      {
                          if (data2.nome == "_MORTO_")
                          {
                              userlist.RemoveAt(index);
                              break; // TODO: might not be correct. Was : Exit For
                          }
                          index += 1;
                      }
                  }*/
                /*    }
                    else
                    {
                        gfx.TranslateTransform(drawPoint.X, drawPoint.Y);
                            gfx.RotateTransform(-45);
                        gfx.DrawImage(My.Resources.map_dead, -6 / zoom, 0, 14 / zoom, 15 / zoom);
                    }*/
            }
        }


        /////////////////////////////////////////////////////////////////////
        ////////////////// FINE FUNZIONI DISEGNO ELEMENTI ///////////////////
        /////////////////////////////////////////////////////////////////////

        public void Coordinates(Graphics gfx, Point Focus, Ultima.Map map__1)
        {
            string locString = null;
                locString = String.Format("({0},{1})", Focus.X, Focus.Y);


            this.Text = "UOAM2 - " + World.Player.Name + " - [" + locString + "]";
        }

		internal void FullUpdate()
		{
			if (!Active || this.FocusMobile == null)
				return;

			if (m_Background != null)
            {
                m_Background.Dispose();
            }
            
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int w = (this.Width) >> 3;
            int h = (this.Height) >> 3;
            int xtrans = this.Width / 2;
            int ytrans = this.Height / 2;
		    Point PositionPg = new Point(World.Player.Position.X, World.Player.Position.Y);
		    Point focus = default(Point);
     	    focus = new Point(PositionPg.X, PositionPg.Y);

		    Point offset = new Point(focus.X & 7, focus.Y & 7);
		    Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));
		    Point pntPlayer = new Point((focus.X) - (mapOrigin.X << 3) - offset.X, (focus.Y) - (mapOrigin.Y << 3) - offset.Y);

		    int Facet = World.Player.Map;
		    if (Facet != CurrentFacet) {
			    map__1 = null;
			    map__1 = Assistant.Map.GetMap(Facet);
			    if (map__1 == null)
				    map__1 = Ultima.Map.Felucca;
			    CurrentFacet = Facet;
		    }

            m_Background = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

		    using (Graphics gfx = Graphics.FromImage(m_Background)) {
			    gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			    gfx.TranslateTransform(-pntPlayer.X, -pntPlayer.Y, MatrixOrder.Append);
			    if (zoom >= 1.5f) {
				    gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			    } else {
				    gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
			    }
			    gfx.PageUnit = GraphicsUnit.Pixel;
			    gfx.ScaleTransform(zoom, zoom, MatrixOrder.Append);
			    gfx.RotateTransform(45, MatrixOrder.Append);
			    gfx.TranslateTransform(pntPlayer.X, pntPlayer.Y, MatrixOrder.Append);


			    // Disegna mappa
			    gfx.DrawImage((map__1.GetImage(mapOrigin.X - (VarZoom >> 3), mapOrigin.Y - (VarZoom >> 3), (w + offset.X + OffsetZoom), (h + offset.Y + OffsetZoom), true)), -offset.X - VarZoom, -offset.Y - VarZoom);
			    gfx.ScaleTransform(1f, 1f, MatrixOrder.Append);

			    // Disegna guardie
				    //Guardlines(gfx, mapOrigin, offset, rect);

			    //  Disegno punti interesse
		        Buildings(gfx, pntPlayer, mapOrigin, offset, rect);

			    // Diesgna mio punto
			    Brush fontPunto = new SolidBrush(Color.Blue);
			    gfx.FillRectangle(fontPunto, (pntPlayer.X - 1f / zoom), (pntPlayer.Y - 1f / zoom), (2 / zoom), (2 / zoom));

			    gfx.ResetTransform();
			    gfx.ScaleTransform(zoom, zoom);


			    // Disegna Nome e Barra hp
			    NameAndBar(gfx, pntPlayer);

			    //  Disegna coordinate
			    //if (My.Settings.coordinates == true)
				    //Coordinates(gfx, focus, map__1);

			    gfx.ResetTransform();
			    gfx.Dispose();
		    }
        this.Refresh();
		}


        private List<Point> SegmentToRectangleIntersect(Point ptA, Point ptB, Rectangle RC, Geometry2.SegmentIntersection IntersectionType)
        {
            List<Point> pts = new List<Point>();
            Point ptC = default(Point);
            Point ptD = default(Point);
            Point ptE = default(Point);
            Point ptF = default(Point);
            Geometry2.SegmentIntersection SI = default(Geometry2.SegmentIntersection);

            ptC = new Point(RC.Left, RC.Top);
            ptD = new Point(RC.Right, RC.Top);
            SI = Geometry2.SegmentIntersect(ptA, ptB, ptC, ptD, ptE, ptF);
            if (SI == IntersectionType)
            {
                pts.Add(ptE);
            }

            ptC = new Point(RC.Right, RC.Top);
            ptD = new Point(RC.Right, RC.Bottom);
            SI = Geometry2.SegmentIntersect(ptA, ptB, ptC, ptD, ptE, ptF);
            if (SI == IntersectionType)
            {
                pts.Add(ptE);
            }

            ptC = new Point(RC.Right, RC.Bottom);
            ptD = new Point(RC.Left, RC.Bottom);
            SI = Geometry2.SegmentIntersect(ptA, ptB, ptC, ptD, ptE, ptF);
            if (SI == IntersectionType)
            {
                pts.Add(ptE);
            }

            ptC = new Point(RC.Left, RC.Bottom);
            ptD = new Point(RC.Left, RC.Top);
            SI = Geometry2.SegmentIntersect(ptA, ptB, ptC, ptD, ptE, ptF);
            if (SI == IntersectionType)
            {
                pts.Add(ptE);
            }

            return pts;
        }
        
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

            if (Active)
            {
                Graphics gfx = pe.Graphics;
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                gfx.DrawImageUnscaledAndClipped(m_Background, rect);

                int w = (this.Width) >> 3;
                int h = (this.Height) >> 3;
                int xtrans = this.Width / 2;
                int ytrans = this.Height / 2;
                Point Focus = new Point(World.Player.Position.X, World.Player.Position.Y);
                Point offset = new Point(Focus.X & 7, Focus.Y & 7);
                Point mapOrigin = new Point((Focus.X >> 3) - (w / 2), (Focus.Y >> 3) - (h / 2));
                Point pntPlayer = new Point((Focus.X) - (mapOrigin.X << 3) - offset.X, (Focus.Y) - (mapOrigin.Y << 3) - offset.Y);

                gfx.TranslateTransform(-pntPlayer.X, -pntPlayer.Y, MatrixOrder.Append);
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gfx.PageUnit = GraphicsUnit.Pixel;
                gfx.ScaleTransform(zoom, zoom, MatrixOrder.Append);
                gfx.RotateTransform(45, MatrixOrder.Append);
                gfx.TranslateTransform(pntPlayer.X, pntPlayer.Y, MatrixOrder.Append);
                gfx.ScaleTransform(1f, 1f, MatrixOrder.Append);

                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                GraphicsState transState = gfx.Save();


                //   Calcolo e disegno 
                foreach (MapUO.MapNetworkIn.UserData user in MapUO.MapNetwork.UData)
                {
                    string nome = user.Nome;
                    if (nome == null || nome.Length < 1)
                    {
                        nome = "(Not Seen)";
                    }
                    if (nome != null && nome.Length > 8)
                    {
                        nome = nome.Substring(0, 8);
                    }

                    drawPoint = new Point((user.X) - (mapOrigin.X << 3) - offset.X, (user.Y) - (mapOrigin.Y << 3) - offset.Y);
                    PointF drawPointF = RotatePoint(pntPlayer, drawPoint);

                    //Disegno PG
                    if (drawPointF.X <= 0 | drawPointF.X > rect.Width | drawPointF.Y <= 0 | drawPointF.Y > rect.Height)
                    {
                        PgOutRange(gfx, pntPlayer, drawPoint, rect, user, drawPointF, nome);
                    }
                    else
                    {
                        PgInRange(gfx, drawPoint, drawPointF, user, nome);
                    }
                    gfx.ResetTransform();
                    gfx.Restore(transState);
                }

                //Disegna Zoom
                gfx.ResetTransform();
                Font font = new Font("Arial", 9, FontStyle.Bold);
                gfx.DrawString("x" + zoom.ToString(), font, Brushes.Gold, new Point(rect.Width - 35, rect.Height - 20));
            }
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
	
	    internal static bool Format(Point p, Ultima.Map map, ref int xLong, ref int yLat, ref int xMins, ref int yMins, ref bool xEast, ref bool ySouth)
		{
			if (map == null)
				return false;

			int x = p.X, y = p.Y;
			int xCenter, yCenter;
			int xWidth, yHeight;

			if (!ComputeMapDetails(map, x, y, out xCenter, out yCenter, out xWidth, out yHeight))
				return false;

			double absLong = (double)((x - xCenter) * 360) / xWidth;
			double absLat = (double)((y - yCenter) * 360) / yHeight;

			if (absLong > 180.0)
				absLong = -180.0 + (absLong % 180.0);

			if (absLat > 180.0)
				absLat = -180.0 + (absLat % 180.0);

			bool east = (absLong >= 0), south = (absLat >= 0);

			if (absLong < 0.0)
				absLong = -absLong;

			if (absLat < 0.0)
				absLat = -absLat;

			xLong = (int)absLong;
			yLat = (int)absLat;

			xMins = (int)((absLong % 1.0) * 60);
			yMins = (int)((absLat % 1.0) * 60);

			xEast = east;
			ySouth = south;

			return true;
		}

		internal static bool ComputeMapDetails(Ultima.Map map, int x, int y, out int xCenter, out int yCenter, out int xWidth, out int yHeight)
		{
			xWidth = 5120; yHeight = 4096;

			if (map == Ultima.Map.Trammel || map == Ultima.Map.Felucca)
			{
				if (x >= 0 && y >= 0 && x < 5120 && y < map.Height)
				{
					xCenter = 1323; yCenter = 1624;
				}
				else if (x >= 5120 && y >= 2304 && x < 6144 && y < map.Height)
				{
					xCenter = 5936; yCenter = 3112;
				}
				else
				{
					xCenter = 0; yCenter = 0;
					return false;
				}
			}
			else if (x >= 0 && y >= 0 && x < map.Width && y < map.Height)
			{
				xCenter = 1323; yCenter = 1624;
			}
			else
			{
				xCenter = map.Width / 2; yCenter = map.Height / 2;
				return false;
			}

			return true;
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
