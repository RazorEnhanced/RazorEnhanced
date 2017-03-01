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
			foreach (Filter f in m_Filters)
			{
				if (RazorEnhanced.Settings.General.ReadBool(((int)f.Name).ToString()))
					f.OnEnable();
			}
		}

		internal static void DisableAll()
		{
			foreach (Filter t in m_Filters)
				t.OnDisable();
		}

		internal static void Save()
		{
			foreach (Filter f in m_Filters)
			{
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
			foreach (byte t in PacketIDs)
				PacketHandler.RegisterServerToClientViewer(t, m_Callback);
		}

		internal virtual void OnDisable()
		{
			m_Enabled = false;
			foreach (byte t in PacketIDs)
				PacketHandler.RemoveServerToClientViewer(t, m_Callback);
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