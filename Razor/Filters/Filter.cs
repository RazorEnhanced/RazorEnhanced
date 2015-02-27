using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Assistant.Filters
{
	internal abstract class Filter
	{
		private static List<Filter> m_Filters = new List<Filter>();
		public static List<Filter> List { get { return m_Filters; } }

		internal static void Register(Filter filter)
		{
			m_Filters.Add(filter);
		}

		internal static void Load(XmlElement xml)
		{
			DisableAll();

			if (xml == null)
				return;

			foreach (XmlElement el in xml.GetElementsByTagName("filter"))
			{
				try
				{
					LocString name = (LocString)Convert.ToInt32(el.GetAttribute("name"));
					string enable = el.GetAttribute("enable");

					for (int i = 0; i < m_Filters.Count; i++)
					{
						Filter f = (Filter)m_Filters[i];
						if (f.Name == name)
						{
							if (Convert.ToBoolean(enable))
								f.OnEnable();
							break;
						}
					}
				}
				catch
				{
				}
			}
		}

		internal static void DisableAll()
		{
			for (int i = 0; i < m_Filters.Count; i++)
				((Filter)m_Filters[i]).OnDisable();
		}

		internal static void Save(XmlTextWriter xml)
		{
			for (int i = 0; i < m_Filters.Count; i++)
			{
				Filter f = (Filter)m_Filters[i];
				if (f.Enabled)
				{
					xml.WriteStartElement("filter");
					xml.WriteAttributeString("name", ((int)f.Name).ToString());
					xml.WriteAttributeString("enable", f.Enabled.ToString());
					xml.WriteEndElement();
				}
			}
		}

		internal static void Draw(CheckedListBox list)
		{
			list.BeginUpdate();
			list.Items.Clear();

			for (int i = 0; i < m_Filters.Count; i++)
			{
				Filter f = (Filter)m_Filters[i];
				list.Items.Add(f);
				list.SetItemChecked(i, f.Enabled);
			}
			list.EndUpdate();
		}

		internal abstract void OnFilter(PacketReader p, PacketHandlerEventArgs args);
		internal abstract byte[] PacketIDs { get; }
		internal abstract LocString Name { get; }

		internal bool Enabled { get { return m_Enabled; } }
		private bool m_Enabled;
		private PacketViewerCallback m_Callback;

		protected Filter()
		{
			m_Enabled = false;
			m_Callback = new PacketViewerCallback(this.OnFilter);
		}

		public override string ToString()
		{
			return Language.GetString(this.Name);
		}

		internal virtual void OnEnable()
		{
			m_Enabled = true;
			for (int i = 0; i < PacketIDs.Length; i++)
				PacketHandler.RegisterServerToClientViewer(PacketIDs[i], m_Callback);
		}

		internal virtual void OnDisable()
		{
			m_Enabled = false;
			for (int i = 0; i < PacketIDs.Length; i++)
				PacketHandler.RemoveServerToClientViewer(PacketIDs[i], m_Callback);
		}

		internal void OnCheckChanged(CheckState newValue)
		{
			if (Enabled && newValue == CheckState.Unchecked)
				OnDisable();
			else if (!Enabled && newValue == CheckState.Checked)
				OnEnable();
		}
	}
}

