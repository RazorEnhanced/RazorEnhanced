using System.Collections.Generic;

namespace RazorEnhanced
{
	public class Multi
	{
		public class MultiData
		{
			private Assistant.Point3D m_position;
			internal Assistant.Point3D Position { get { return m_position; } }

			private Assistant.Point2D m_corner1;
			internal Assistant.Point2D Corner1 { get { return m_corner1; } }

			private Assistant.Point2D m_corner2;
			internal Assistant.Point2D Corner2 { get { return m_corner2; } }

			internal MultiData(Assistant.Point3D position, Assistant.Point2D corner1, Assistant.Point2D corner2)
			{
				m_position = position;
				m_corner1 = corner1;
				m_corner2 = corner2;
			}
		}
	}
}