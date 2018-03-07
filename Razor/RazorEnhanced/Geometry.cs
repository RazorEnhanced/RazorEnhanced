namespace RazorEnhanced
{
	public struct Point2D
	{
		private Assistant.Point2D m_AssistantPoint2D;

		internal static readonly Point2D Zero = new Point2D(new Assistant.Point2D(0, 0));
		internal static readonly Point2D MinusOne = new Point2D(new Assistant.Point2D(-1, -1));

		internal Point2D(Assistant.Point2D point2D)
		{
			m_AssistantPoint2D = point2D;
		}

		public int X { get { return m_AssistantPoint2D.X; } }
		public int Y { get { return m_AssistantPoint2D.Y; } }

		public override string ToString()
		{
			return m_AssistantPoint2D.ToString();
		}
	}

	public struct Point3D
	{
		private Assistant.Point3D m_AssistantPoint3D;

		internal static readonly Point3D Zero = new Point3D(new Assistant.Point3D(0, 0, 0));
		internal static readonly Point3D MinusOne = new Point3D(new Assistant.Point3D(-1, -1, 0));

		internal Point3D(Assistant.Point3D point3D)
		{
			m_AssistantPoint3D = point3D;
		}

		internal Point3D(int x, int y, int z)
		{
			m_AssistantPoint3D = new Assistant.Point3D(x, y, z);
		}

		public int X { get { return m_AssistantPoint3D.X; } }
		public int Y { get { return m_AssistantPoint3D.Y; } }
		public int Z { get { return m_AssistantPoint3D.Z; } }

		public override string ToString()
		{
			return m_AssistantPoint3D.ToString();
		}
	}
}