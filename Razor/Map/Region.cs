using System.Collections.Generic;
using System.IO;

namespace Assistant.MapUO
{
	internal class Region
	{
		private int m_Height;
		private int m_Width;
		private int m_X;
		private int m_Y;
		private int m_Z;

		private int m_Facet;
		internal Region(string line)
		{
			string[] textArray1 = line.Split('\t');
			this.m_X = int.Parse(textArray1[0]);
			this.m_Y = int.Parse(textArray1[1]);
			this.m_Z = int.Parse(textArray1[2]);
			this.m_Width = int.Parse(textArray1[3]);
			this.m_Height = int.Parse(textArray1[4]);
			this.m_Facet = int.Parse(textArray1[5]);
		}

		internal Region(int x, int y, int z, int width, int height, int facet)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = z;
			this.m_Width = width;
			this.m_Height = height;
			this.m_Facet = facet;
		}


		static internal List<Region> RegionLists = new List<Region>();
		static internal void Load(string path)
		{
			try
			{
				using (StreamReader reader = new StreamReader(path))
				{
					string line = null;
					while ((InlineAssignHelper(ref line, reader.ReadLine())) != null)
					{
						if ((line.Length != 0) && !line.StartsWith("#"))
						{
							RegionLists.Add(new Region(line));
						}
					}
				}
			}
			catch
			{
			}
		}

		internal int X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		internal int Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}
		internal int z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		internal int Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		internal int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}


		internal int Facet
		{
			get { return m_Facet; }
			set { m_Facet = value; }
		}
		private static T InlineAssignHelper<T>(ref T target, T value)
		{
			target = value;
			return value;
		}

	}
}

