using System;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Reflection;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace Assistant.MapUO
{
    internal class MapIcon
	{
        public class MapIconData
        {
            private Image m_icon;
            public Image Icon
            {
                get { return m_icon; }
            }

            private short m_x;
            public short X
            {
                get { return m_x; }
            }

            private short m_y;
            public short Y
            {
                get { return m_y; }
            }

            private short m_facet;
            public short Facet
            {
                get { return m_facet; }
                set { m_facet = value; }
            }

            private string m_desc;
            public string Desc
            {
                get { return m_desc; }
            }

            public MapIconData(Image icon, short x, short y, short facet, string desc)
			{
                m_icon = icon;
                m_x = x;
                m_y = y;
                m_facet = facet;
                m_desc = desc;
			}
        }

        internal static List<MapIconData> IconList = new List<MapIconData>();
        internal static void ParseDataFile(string filename)
        {
            StreamReader reader = File.OpenText(filename);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                Image imagefile = Image.FromFile("Icon//" + items[0] + ".gif");
                short x = short.Parse(items[1]);
                short y = short.Parse(items[2]);
                short facet = short.Parse(items[3]);
                string desc = items[4];
                IconList.Add(new MapIconData(imagefile, x, y, facet, desc));
            }
            reader.Close();
            MapNetwork.AddLog("- Done Parsing Datafile: " + filename);
        }
	}
}
