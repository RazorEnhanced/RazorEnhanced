using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assistant
{
	[Flags]
	internal enum Direction : byte
	{
		North = 0x0,
		Right = 0x1,
		East = 0x2,
		Down = 0x3,
		South = 0x4,
		Left = 0x5,
		West = 0x6,
		Mask = 0x7,
		Up = 0x7,
		Running = 0x80,
		ValueMask = 0x87
	}

	public class Mobile : UOEntity
	{
		private static List<ushort> m_HumanBodies = new List<ushort>() { 183, 184, 185, 186, 400, 401, 402, 403, 605, 606, 607, 608, 666, 667, 694, 744, 745, 747, 748, 750, 751, 970, 695};

		private ushort m_Body;
		private Direction m_Direction;
		private string m_Name;

		private byte m_Notoriety;

		private bool m_Visible;
		private bool m_Female;
		private bool m_Poisoned;
		private bool m_Blessed; // Yellow Hits
		private bool m_Warmode;
		private bool m_Paralized;
		private bool m_Flying;

		// Stats & Props
		private bool m_PropsUpdated = false;
		private bool m_StatsUpdated = false;

		private ushort m_HitsMax, m_Hits;
		protected ushort m_StamMax, m_Stam, m_ManaMax, m_Mana;

		private List<Item> m_Items;
		private byte m_Map;

		internal Mobile(Serial serial)
			: base(serial)
		{
			m_Items = new List<Item>();
			m_Map = World.Player == null ? (byte)0 : World.Player.Map;
			m_Visible = true;
		}

		internal string Name
		{
			get
			{
				if (m_Name == null)
					return "";
				else
					return m_Name;
			}
			set
			{
				if (value != null)
				{
					string trim = value.Trim();
					if (trim.Length > 0)
						m_Name = trim;
				}
			}
		}

		internal ushort Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}

		internal bool PropsUpdated
		{
			get { return m_PropsUpdated; }
			set { m_PropsUpdated = value; }
		}

		internal bool StatsUpdated
		{
			get { return m_StatsUpdated; }
			set { m_StatsUpdated = value; }
		}

		internal Direction Direction
		{
			get { return m_Direction; }
			set	{ m_Direction = value; }
		}

		internal bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}

		internal bool Poisoned
		{
			get { return m_Poisoned; }
			set { m_Poisoned = value; }
		}

		internal bool Blessed
		{
			get { return m_Blessed; }
			set { m_Blessed = value; }
		}

		internal bool IsHuman
		{
			get
			{
				if (m_HumanBodies.Contains(this.Body))
					return true;
				else
					return false;
			}
		}

		internal bool IsGhost
		{
			get
			{
				if (StatsUpdated && m_Hits == 0)
					return true;

				return m_Body == 402
					|| m_Body == 403
					|| m_Body == 607
					|| m_Body == 608
					|| m_Body == 694
					|| m_Body == 695
					|| m_Body == 970;
			}
		}

		internal bool Warmode
		{
			get { return m_Warmode; }
			set { m_Warmode = value; }
		}

		internal bool Female
		{
			get { return m_Female; }
			set { m_Female = value; }
		}

		internal bool Paralized
		{
			get { return m_Paralized; }
			set { m_Paralized = value; }
		}

		internal bool Flying
		{
			get { return m_Flying; }
			set { m_Flying = value; }
		}

		internal byte Notoriety
		{
			get { return m_Notoriety; }
			set
			{
				if (value != Notoriety)
				{
					OnNotoChange(m_Notoriety, value);
					m_Notoriety = value;
				}
			}
		}

		protected virtual void OnNotoChange(byte old, byte cur)
		{
		}

		// grey, blue, green, 'canbeattacked'
		private static uint[] m_NotoHues = new uint[8]
		{
			// hue color #30
			0x000000, // black		unused 0
			0x30d0e0, // blue		0x0059 1
			0x60e000, // green		0x003F 2
			0x9090b2, // greyish	0x03b2 3
			0x909090, // grey		   "   4
			0xd88038, // orange		0x0090 5
			0xb01000, // red		0x0022 6
			0xe0e000, // yellow		0x0035 7
		};

		internal uint GetNotorietyColor()
		{
			if (m_Notoriety < 0 || m_Notoriety >= m_NotoHues.Length)
				return m_NotoHues[0];
			else
				return m_NotoHues[m_Notoriety];
		}

		internal byte GetStatusCode()
		{
			if (m_Poisoned)
				return 1;
			else
				return 0;
		}

		internal ushort HitsMax
		{
			get { return m_HitsMax; }
			set { m_HitsMax = value; }
		}

		internal ushort Hits
		{
			get { return m_Hits; }
			set { m_Hits = value; }
		}

		internal ushort Stam
		{
			get { return m_Stam; }
			set { m_Stam = value; }
		}

		internal ushort StamMax
		{
			get { return m_StamMax; }
			set { m_StamMax = value; }
		}

		internal ushort Mana
		{
			get { return m_Mana; }
			set { m_Mana = value; }
		}

		internal ushort ManaMax
		{
			get { return m_ManaMax; }
			set { m_ManaMax = value; }
		}

		internal byte Map
		{
			get { return m_Map; }
			set
			{
				if (m_Map != value)
				{
					OnMapChange(m_Map, value);
					m_Map = value;
				}
			}
		}

		internal virtual void OnMapChange(byte old, byte cur)
		{
		}

		internal void AddItem(Item item)
		{
			m_Items.Add(item);
		}

		internal void RemoveItem(Item item)
		{
			m_Items.Remove(item);
		}

		internal override void Remove()
		{
			List<Item> rem = new List<Item>(m_Items);
			m_Items.Clear();

			foreach (Item r in rem)
				r.Remove();

			if (!InParty)
			{
				base.Remove();
				World.RemoveMobile(this);
			}
			else
			{
				Visible = false;
			}
		}

		internal bool InParty
		{
			get
			{
				return PacketHandlers.Party.Contains(this.Serial);
			}
		}

		internal Item GetItemOnLayer(Layer layer)
		{
			for (int i = 0; i < m_Items.Count; i++)
			{
				Item item = (Item)m_Items[i];
				if (item.Layer == layer)
					return item;
			}
			return null;
		}

		internal Item Backpack
		{
			get
			{
				return GetItemOnLayer(Layer.Backpack);
			}
		}

		internal Item Quiver
		{
			get
			{
				Item item = GetItemOnLayer(Layer.Cloak);

				if (item != null && item.IsContainer)
					return item;
				else
					return null;
			}
		}

		internal Item FindItemByID(ItemID id)
		{
			return this.Contains.FirstOrDefault(item => item.ItemID == id);
		}

		internal int GetPacketFlags()
		{
			int flags = 0x0;

			if (m_Paralized)
				flags |= 0x01;

			if (m_Female)
				flags |= 0x02;

			if (m_Poisoned)
				flags |= 0x04;

			if (m_Blessed)
				flags |= 0x08;

			if (m_Warmode)
				flags |= 0x40;

			if (!m_Visible)
				flags |= 0x80;

			return flags;
		}

		internal void ProcessPacketFlags(byte flags)
		{
			bool needrefresh = false;
			if (!PacketHandlers.UseNewStatus)
			{
				if (Poisoned != ((flags & 0x04) != 0))
					needrefresh = true;
				m_Poisoned = (flags & 0x04) != 0;
			}
			else
				m_Flying = (flags & 0x04) != 0;

			if (Blessed != ((flags & 0x08) != 0))
				needrefresh = true;
			m_Blessed = (flags & 0x08) != 0;

			m_Female = (flags & 0x02) != 0;
			m_Warmode = (flags & 0x40) != 0;
			m_Visible = (flags & 0x80) == 0;

			if (Paralized != ((flags & 0x01) != 0))
				needrefresh = true;

			m_Paralized = (flags & 0x01) != 0;

			if (!needrefresh) // Non richiede aggiornamento colori in quanto flag non cambiati
				return;

			if (this == World.Player)
			{
				if (Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked)
					RazorEnhanced.Filters.ApplyColor(this);
			}
			else
			{
				if (Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked)
					RazorEnhanced.Filters.ApplyColor(this);
			}
		}

		internal List<Item> Contains { get { return m_Items; } }

		internal void OverheadMessageFrom(int hue, string from, string format, params object[] args)
		{
			OverheadMessageFrom(hue, from, String.Format(format, args));
		}

		internal void OverheadMessageFrom(int hue, string from, string text)
		{
			Assistant.Client.Instance.SendToClient(new UnicodeMessage(Serial, m_Body, MessageType.Regular, hue, 3, Language.CliLocName, from, text));
		}

		internal void OverheadMessage(string text)
		{
			OverheadMessage(Engine.MainWindow.SysColor, text);
		}

		internal void OverheadMessage(string format, params object[] args)
		{
			OverheadMessage(Engine.MainWindow.SysColor, String.Format(format, args));
		}

		internal void OverheadMessage(int hue, string format, params object[] args)
		{
			OverheadMessage(hue, String.Format(format, args));
		}

		internal void OverheadMessage(int hue, string text)
		{
			OverheadMessageFrom(hue, "Razor", text);
		}

		internal void OverheadMessage(LocString str)
		{
			OverheadMessage(Language.GetString(str));
		}

		internal void OverheadMessage(LocString str, params object[] args)
		{
			OverheadMessage(Language.Format(str, args));
		}

		private Point2D m_ButtonPoint = Point2D.Zero;

		internal Point2D ButtonPoint
		{
			get { return m_ButtonPoint; }
			set { m_ButtonPoint = value; }
		}
	}
}
