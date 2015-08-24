using System;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

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

        internal static List<MapIconData> IconTreasurePFList = new List<MapIconData>();
        internal static List<MapIconData> IconTreasureList = new List<MapIconData>();
        internal static List<MapIconData> IconTokunoIslandsList = new List<MapIconData>();
        internal static List<MapIconData> IconStealablesList = new List<MapIconData>();
        internal static List<MapIconData> IconRaresList = new List<MapIconData>();
        internal static List<MapIconData> IconPersonalList = new List<MapIconData>();
        internal static List<MapIconData> IconOldHavenList = new List<MapIconData>();
        internal static List<MapIconData> IconNewHavenList = new List<MapIconData>();
        internal static List<MapIconData> IconMLList = new List<MapIconData>();
        internal static List<MapIconData> IconDungeonsList = new List<MapIconData>();
        internal static List<MapIconData> IconcommonList = new List<MapIconData>();
        internal static List<MapIconData> IconAtlasList = new List<MapIconData>();

        internal static List<MapIconData> ParseDataFile(string filename)
        {
            bool sintaxerror = false;
            List<MapIconData> lista = new List<MapIconData>();
            MapNetwork.AddLog("- Try parsing: " + filename);
            if (File.Exists(filename))
            {
                StreamReader reader = File.OpenText(filename);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split('\t');
                    if (items.Length != 5)
                    {
                        sintaxerror = true;
                        break;
                    }

                    if (!File.Exists("Icon//" + items[0] + ".gif"))
                    {
                        MapNetwork.AddLog("- Missing Icon file : " + items[0] + ".gif");
                        continue;
                    }

                    Image imagefile = Image.FromFile("Icon//" + items[0] + ".gif");
                    short x = short.Parse(items[1]);
                    short y = short.Parse(items[2]);
                    short facet = short.Parse(items[3]);
                    string desc = items[4];
                    lista.Add(new MapIconData(imagefile, x, y, facet, desc));
                }
                reader.Close();
                if (sintaxerror)
                    MapNetwork.AddLog("- Sintax Error in file: " + filename);
                else
                    MapNetwork.AddLog("- Done parsing: " + filename);
            }
            else
                MapNetwork.AddLog("- Fail parsing: " + filename);

            return lista;
        }
	}
}
