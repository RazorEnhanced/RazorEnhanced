using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Assistant.MapUO
{
	internal class Region
	{
		private int m_Height;
		private int m_Width;
		private int m_X;
		private int m_Y;

		internal Region(string line)
		{
			string[] textArray1 = line.Split(new char[] { ' ' });
			this.m_X = int.Parse(textArray1[0]);
			this.m_Y = int.Parse(textArray1[1]);
			this.m_Width = int.Parse(textArray1[2]);
			this.m_Height = int.Parse(textArray1[3]);
		}

		internal Region(int x, int y, int width, int height)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Width = width;
			this.m_Height = height;
		}

		internal static Region[] Load(string path)
		{
			if (!File.Exists(path))
			{
				return new Region[0];
			}
			List<Region> list = new List<Region>();
			try
			{
				using (StreamReader reader1 = new StreamReader(path))
				{
					string text;
					while ((text = reader1.ReadLine()) != null)
					{
						if ((text.Length != 0) && !text.StartsWith("#"))
						{
							list.Add(new Region(text));
						}
					}
				}
			}
			catch
			{
			}
			return list.ToArray();
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

		internal int Length
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		internal int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

	}
}
