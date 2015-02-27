using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace Assistant
{
	internal class CounterLVIComparer : IComparer
	{
		private static CounterLVIComparer m_Instance;
		internal static CounterLVIComparer Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new CounterLVIComparer();
				return m_Instance;
			}
		}

		internal CounterLVIComparer()
		{
		}

		public int Compare(object a, object b)
		{
			return ((IComparable)(((ListViewItem)a).Tag)).CompareTo(((ListViewItem)b).Tag);
		}
	}

	internal class Counter : IComparable
	{
		private string m_Name;
		private string m_Fmt;
		private ushort m_ItemID;
		private int m_Hue;
		private int m_Count;
		private bool m_Enabled;
		private DateTime m_LastWarning;
		private bool m_Flag;
		private bool m_DispImg;
		private ListViewItem m_LVI;

		internal Counter(string name, string fmt, ushort iid, int hue, bool dispImg)
		{
			m_Name = name;
			m_Fmt = fmt;
			m_ItemID = iid;
			m_Hue = hue;
			m_LVI = new ListViewItem(new string[2]);
			m_LVI.SubItems[0].Text = ToString();
			m_LVI.Tag = this;
			m_LVI.Checked = m_Enabled = false;
			m_Count = 0;
			m_DispImg = dispImg;

			m_NeedXMLSave = true;
		}

		internal Counter(XmlElement node)
		{
			m_Name = GetText(node["name"], "");
			m_Fmt = GetText(node["format"], "");
			m_ItemID = (ushort)GetInt(GetText(node["itemid"], "0"), 0);
			m_Hue = GetInt(GetText(node["hue"], "-1"), -1);

			m_LVI = new ListViewItem(new string[2] { this.ToString(), "" });
			m_LVI.Tag = this;
			m_LVI.Checked = m_Enabled = false;

			m_DispImg = true;
		}

		internal void Save(XmlTextWriter xml)
		{
			xml.WriteStartElement("counter");

			xml.WriteStartElement("name");
			xml.WriteString(m_Name);
			xml.WriteEndElement();

			xml.WriteStartElement("format");
			xml.WriteString(m_Fmt);
			xml.WriteEndElement();

			xml.WriteStartElement("itemid");
			xml.WriteString(m_ItemID.ToString());
			xml.WriteEndElement();

			xml.WriteStartElement("hue");
			xml.WriteString(m_Hue.ToString());
			xml.WriteEndElement();

			xml.WriteEndElement();
		}

		internal string Name { get { return m_Name; } }
		internal string Format { get { return m_Fmt; } }
		internal ushort ItemID { get { return m_ItemID; } }
		internal int Hue { get { return m_Hue; } }
		internal bool Flag { get { return m_Flag; } set { m_Flag = value; } }
		internal ListViewItem ViewItem { get { return m_LVI; } }

		internal void Set(ushort iid, int hue, string name, string fmt, bool dispImg)
		{
			m_ItemID = iid;
			m_Hue = hue;
			m_Name = name;
			m_Fmt = fmt;
			m_DispImg = dispImg;

			m_LVI.SubItems[0].Text = ToString();
			m_NeedXMLSave = true;
		}

		internal string GetTitlebarString(bool dispImg)
		{
			StringBuilder sb = new StringBuilder();
			if (dispImg)
			{
				sb.AppendFormat("~I{0:X4}", m_ItemID);
				if (m_Hue > 0 && m_Hue < 0xFFFF)
					sb.Append(m_Hue.ToString("X4"));
				else
					sb.Append('~');
				sb.Append(": ");
			}

			if (m_Flag && Config.GetBool("HighlightReagents"))
				sb.AppendFormat("~^C00000{0}~#~", m_Count);
			else if (m_Count == 0 || m_Count < Config.GetInt("CounterWarnAmount"))
				sb.AppendFormat("~#FF0000{0}~#~", m_Count);
			else
				sb.AppendFormat("~#000000{0}~#~", m_Count);
			// sb.Append( m_Count.ToString() );

			return sb.ToString();
		}

		internal int Amount
		{
			get { return m_Count; }
			set
			{
				if (m_Count != value)
				{
					if (m_Enabled)
					{
						if (!SupressWarnings && m_LastWarning + TimeSpan.FromSeconds(1.0) < DateTime.Now &&
							World.Player != null && value < m_Count && Config.GetBool("CounterWarn") && value < Config.GetInt("CounterWarnAmount"))
						{
							World.Player.SendMessage(MsgLevel.Warning, LocString.CountLow, Name, value);
							m_LastWarning = DateTime.Now;
						}

						if (ClientCommunication.NotificationCount > 0)
						{
							int wp = 0;
							if (Format == "bm")
								wp = 1;
							else if (Format == "bp")
								wp = 2;
							else if (Format == "gl")
								wp = 3;
							else if (Format == "gs")
								wp = 4;
							else if (Format == "mr")
								wp = 5;
							else if (Format == "ns")
								wp = 6;
							else if (Format == "sa")
								wp = 7;
							else if (Format == "ss")
								wp = 8;
							else if (Format == "bw")
								wp = 100;
							else if (Format == "db")
								wp = 101;
							else if (Format == "gd")
								wp = 102;
							else if (Format == "nc")
								wp = 103;
							else if (Format == "pi")
								wp = 104;

							if (wp != 0)
								ClientCommunication.PostCounterUpdate(wp, value);
						}

						m_Count = value;
						if (m_Count < 0)
							m_Count = 0;

						//Engine.MainWindow.RefreshCounters();
						ClientCommunication.RequestTitlebarUpdate();
					}

					m_LVI.SubItems[1].Text = m_Count.ToString();
				}
			}
		}

		internal void SetEnabled(bool value)
		{
			m_Enabled = value;
			if (m_Enabled)
			{
				if (!SupressChecks)
					QuickRecount();
				m_LVI.SubItems[1].Text = m_Count.ToString();
			}
			else
			{
				m_LVI.SubItems[1].Text = "";
			}
		}

		internal bool Enabled
		{
			get { return m_Enabled; }
			set
			{
				if (m_Enabled != value)
				{
					m_LVI.Checked = value;
					SetEnabled(value);
				}
			}
		}

		internal bool DisplayImage
		{
			get { return m_DispImg; }
			set { m_DispImg = value; }
		}

		public int CompareTo(object comp)
		{
			if (!(comp is Counter))
				return 1;
			else if (this.Enabled && ((Counter)comp).Enabled)
				return this.Name == null ? 1 : ((Counter)comp).Name == null ? -1 : this.Name.CompareTo(((Counter)comp).Name);
			else if (!this.Enabled && ((Counter)comp).Enabled)
				return 1;
			else if (this.Enabled && !((Counter)comp).Enabled)
				return -1;
			else //if ( !this.Enabled && !((Counter)comp).Enabled )
				return this.Name == null ? 1 : ((Counter)comp).Name == null ? -1 : this.Name.CompareTo(((Counter)comp).Name);
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", Name, Format);
		}

		private static bool m_NeedXMLSave = false;
		private static List<Counter> m_List;
		private static bool m_SupressWarn, m_SupressChecks;
		private static Dictionary<Item, ushort> m_Cache;
		internal static List<Counter> List { get { return m_List; } }

		static Counter()
		{
			m_List = new List<Counter>();
			m_Cache = new Dictionary<Item, ushort>();
			Load();
		}

		internal static bool SupressWarnings
		{
			get { return m_SupressWarn; }
			set { m_SupressWarn = value; }
		}

		internal static bool SupressChecks
		{
			get { return m_SupressChecks; }
		}

		internal static void Load()
		{
			string file = Path.Combine(Config.GetUserDirectory(), "counters.xml");
			if (!File.Exists(file))
				return;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(file);

				XmlElement root = doc["counters"];
				if (root != null)
				{
					foreach (XmlElement node in root.GetElementsByTagName("counter"))
						m_List.Add(new Counter(node));
				}
			}
			catch
			{
				MessageBox.Show(Engine.ActiveWindow, Language.GetString(LocString.CounterFux), "Counters.xml Load Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}

			m_NeedXMLSave = false;
		}

		internal static void Save()
		{
			if (!m_NeedXMLSave)
				return;

			try
			{
				string file = Path.Combine(Config.GetUserDirectory(), "counters.xml");
				using (StreamWriter op = new StreamWriter(file))
				{
					XmlTextWriter xml = new XmlTextWriter(op);

					xml.Formatting = Formatting.Indented;
					xml.IndentChar = '\t';
					xml.Indentation = 1;

					xml.WriteStartDocument(true);

					xml.WriteStartElement("counters");

					foreach (Counter c in m_List)
						c.Save(xml);

					xml.WriteEndElement();
					xml.Close();
				}
				m_NeedXMLSave = false;
			}
			catch
			{
			}
		}

		internal static void SaveProfile(XmlTextWriter xml)
		{
			for (int i = 0; i < m_List.Count; i++)
			{
				Counter c = (Counter)m_List[i];
				if (c.Enabled)
				{
					xml.WriteStartElement("counter");
					xml.WriteAttributeString("name", c.Name);
					xml.WriteAttributeString("enabled", c.Enabled.ToString());
					xml.WriteAttributeString("image", c.DisplayImage.ToString());
					xml.WriteEndElement();
				}
			}
		}

		internal static void Default()
		{
			for (int i = 0; i < m_List.Count; i++)
			{
				Counter c = (Counter)m_List[i];

				if (c.Format == "bp" || c.Format == "bm" || c.Format == "gl" || c.Format == "gs" ||
					c.Format == "mr" || c.Format == "ns" || c.Format == "ss" || c.Format == "sa" ||
					c.Format == "aids")
				{
					c.Enabled = true;
				}
			}
		}

		internal static void DisableAll()
		{
			for (int i = 0; i < m_List.Count; i++)
				((Counter)m_List[i]).Enabled = false;
		}

		internal static void LoadProfile(XmlElement xml)
		{
			Reset();
			DisableAll();

			if (xml == null)
				return;

			foreach (XmlElement el in xml.GetElementsByTagName("counter"))
			{
				try
				{
					string name = el.GetAttribute("name");
					string en = el.GetAttribute("enabled");
					string img = el.GetAttribute("image");

					for (int i = 0; i < m_List.Count; i++)
					{
						Counter c = (Counter)m_List[i];

						if (c.Name == name)
						{
							c.Enabled = Convert.ToBoolean(en);
							try
							{
								c.DisplayImage = Convert.ToBoolean(img);
							}
							catch
							{
								c.DisplayImage = true;
							}

							break;
						}
					}
				}
				catch
				{
				}
			}
		}

		private static string GetText(XmlElement node, string defaultValue)
		{
			if (node == null)
				return defaultValue;

			return node.InnerText;
		}

		private static int GetInt(string value, int def)
		{
			try
			{
				return XmlConvert.ToInt32(value);
			}
			catch
			{
				try
				{
					return Convert.ToInt32(value);
				}
				catch
				{
					return def;
				}
			}
		}

		internal static void Register(Counter c)
		{
			m_List.Add(c);
			m_NeedXMLSave = true;
			Engine.MainWindow.RedrawCounters();
		}

		internal static void Uncount(Item item)
		{
			for (int i = 0; i < item.Contains.Count; i++)
				Uncount(item.Contains[i]);

			for (int i = 0; i < m_List.Count; i++)
			{
				Counter c = m_List[i];
				if (c.Enabled)
				{
					if (c.ItemID == item.ItemID && (c.Hue == item.Hue || c.Hue == -1 || c.Hue == 0xFFFF))
					{
						// if (m_Cache[item] != null)
						// {
						ushort rem = m_Cache[item];
						if (rem >= c.Amount)
							c.Amount = 0;
						else
							c.Amount -= rem;
						// m_Cache[item] = null;
						// }

						break;
					}
				}
			}
		}

		internal static void Count(Item item)
		{
			for (int i = 0; i < m_List.Count; i++)
			{
				Counter c = m_List[i];
				if (c.Enabled)
				{
					if (c.ItemID == item.ItemID && (c.Hue == item.Hue || c.Hue == 0xFFFF || c.Hue == -1))
					{
						ushort old = 0;
						if (m_Cache.ContainsKey(item))
						{
							ushort o = m_Cache[item];
							old = (ushort)o;
							if (old == item.Amount)
								break; // dont change result cause we dont need an update

							c.Amount += (item.Amount - old);
							m_Cache[item] = item.Amount;
							break;
						}
					}
				}
			}

			for (int c = 0; c < item.Contains.Count; c++)
				Count((Item)item.Contains[c]);
		}

		internal static void QuickRecount()
		{
			Reset();

			SupressWarnings = true;
			Item pack = World.Player == null ? null : World.Player.Backpack;
			if (pack != null)
				Count(pack);
			pack = World.Player == null ? null : World.Player.Quiver;
			if (pack != null)
				Count(pack);
			SupressWarnings = false;
		}

		internal static void FullRecount()
		{
			Reset();

			if (World.Player != null)
			{
				SupressWarnings = true;

				if (World.Player.Backpack != null)
				{
					while (World.Player.Backpack.Contains.Count > 0)
						((Item)World.Player.Backpack.Contains[0]).Remove();

					PacketHandlers.IgnoreGumps.Add(World.Player.Backpack);
					PlayerData.DoubleClick(World.Player.Backpack);
				}

				if (World.Player.Quiver != null)
				{
					while (World.Player.Quiver.Contains.Count > 0)
						((Item)World.Player.Quiver.Contains[0]).Remove();

					PacketHandlers.IgnoreGumps.Add(World.Player.Quiver);
					PlayerData.DoubleClick(World.Player.Quiver);
				}

				if (!Config.GetBool("AutoSearch"))
					World.Player.SendMessage(MsgLevel.Info, LocString.NoAutoCount);
				SupressWarnings = false;
			}
		}

		internal static void Reset()
		{
			SupressWarnings = true;
			m_Cache.Clear();

			for (int i = 0; i < m_List.Count; i++)
				((Counter)m_List[i]).Amount = 0;
			SupressWarnings = false;
		}

		internal static void Redraw(ListView list)
		{
			m_SupressChecks = true;
			list.BeginUpdate();
			list.Items.Clear();
			for (int i = 0; i < m_List.Count; i++)
				list.Items.Add(((Counter)m_List[i]).ViewItem);
			list.EndUpdate();
			m_SupressChecks = false;
		}
	}
}

