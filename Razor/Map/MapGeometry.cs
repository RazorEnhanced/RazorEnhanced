using System;
using System.Collections.Generic;
using System.Drawing;

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
		internal static Rectangle GetRectangleAt(Point CenterPoint, int ExpandBy)
		{
			Rectangle rc = new Rectangle(CenterPoint, new Size(1, 1));
			rc.Inflate(ExpandBy, ExpandBy);
			return rc;
		}

		internal static List<Point> SegmentToRectangleIntersect(Point ptA, Point ptB, Rectangle RC, Geometry2.SegmentIntersection IntersectionType)
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

	}
}