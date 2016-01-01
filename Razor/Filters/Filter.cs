using System.Collections.Generic;
using System.Windows.Forms;

namespace Assistant.Filters
{
	internal abstract class Filter
	{
		private static List<Filter> m_Filters = new List<Filter>();
		internal static List<Filter> List { get { return m_Filters; } }

		internal static void Register(Filter filter)
		{
			m_Filters.Add(filter);
		}

		internal static void Load()
		{
			DisableAll();
			for (int i = 0; i < m_Filters.Count; i++)
			{
				Filter f = m_Filters[i];
				if (RazorEnhanced.Settings.General.ReadBool(((int)f.Name).ToString()))
					f.OnEnable();
			}
		}

		internal static void DisableAll()
		{
			for (int i = 0; i < m_Filters.Count; i++)
				m_Filters[i].OnDisable();
		}

		internal static void Save()
		{
			for (int i = 0; i < m_Filters.Count; i++)
			{
				Filter f = m_Filters[i];
				if (f.Enabled)
				{
					RazorEnhanced.Settings.General.WriteBool(((int)f.Name).ToString(), f.Enabled);
				}
			}
		}

		internal static void Draw(CheckedListBox list)
		{
			list.BeginUpdate();
			list.Items.Clear();

			for (int i = 0; i < m_Filters.Count; i++)
			{
				Filter f = m_Filters[i];
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
			{
				OnDisable();
				RazorEnhanced.Settings.General.WriteBool(((int)this.Name).ToString().ToString(), false);
			}
			else if (!Enabled && newValue == CheckState.Checked)
			{
				OnEnable();
				RazorEnhanced.Settings.General.WriteBool(((int)this.Name).ToString().ToString(), true);
			}
		}
	}
}