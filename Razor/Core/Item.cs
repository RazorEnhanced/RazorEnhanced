using ConcurrentCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assistant
{
	internal enum Layer : byte
	{
		Invalid = 0x00,

		FirstValid = 0x01,

		RightHand = 0x01,
		LeftHand = 0x02,
		Shoes = 0x03,
		Pants = 0x04,
		Shirt = 0x05,
		Head = 0x06,
		Gloves = 0x07,
		Ring = 0x08,
		Talisman = 0x09,
		Neck = 0x0A,
		Hair = 0x0B,
		Waist = 0x0C,
		InnerTorso = 0x0D,
		Bracelet = 0x0E,
		Unused_xF = 0x0F,
		FacialHair = 0x10,
		MiddleTorso = 0x11,
		Earrings = 0x12,
		Arms = 0x13,
		Cloak = 0x14,
		Backpack = 0x15,
		OuterTorso = 0x16,
		OuterLegs = 0x17,
		InnerLegs = 0x18,

		LastUserValid = 0x18,

		Mount = 0x19,
		ShopBuy = 0x1A,
		ShopResale = 0x1B,
		ShopSell = 0x1C,
		Bank = 0x1D,

		LastValid = 0x1D
	}

	public class Item : UOEntity
	{
		private ItemID m_ItemID;
		private ushort m_Amount;
		private byte m_Direction;

		private bool m_Visible;
		private bool m_Movable;

		private bool m_PropsUpdated;

		private Layer m_Layer;
		private string m_Name;
		private object m_Parent;
		private int m_Price;
		private string m_BuyDesc;
		private List<Item> m_Items;

		private bool m_IsNew;
		private bool m_AutoStack;

		private byte[] m_HousePacket;
		private int m_HouseRev;

		private byte m_GridNum;

		private bool m_Updated;

        private static readonly object LockingVar = new object();
		internal bool Updated
		{
			get { return m_Updated; }
			set
			{
				if (this.IsContainer || this.IsCorpse)
				{
					m_Updated = value;
				}
			}
		}

        public static Item Factory(Serial serial, UInt32 itemID)
        {
            // during drag operation item may be removed from World
            if (itemID == 0 && DragDropManager.Holding != null && DragDropManager.Holding.Serial == serial)
            {
                // Dropping this item, but already deleted so use ItemID from dead one
                // Because the itemID of dragged items is not on a drop packet
                itemID = DragDropManager.Holding.ItemID;
            }
            Item item = null;
            switch (itemID)
            {
                case 0x14EC:
                    item = new MapItem(serial);
                    break;
                case 0x2006:
                    item = new CorpseItem(serial);
                    break;
                case 0:
                    item = new Item(serial);
                    break;
                default:
                    item = new Item(serial);
                    break;
            }
            if (item != null)
            {
                item.ItemID = (ushort)itemID;
            }
            return item;
        }
		protected Item(Serial serial)
			: base(serial)
		{
			m_Items = new List<Item>();

			m_Visible = true;
			m_Movable = true;
		}

		internal ItemID ItemID
		{
			get { return m_ItemID; }
			set { m_ItemID = value; }
		}

		internal bool PropsUpdated
		{
			get { return m_PropsUpdated; }
			set { m_PropsUpdated = value; }
		}

		internal ushort Amount
		{
			get { return m_Amount; }
			set { m_Amount = value; }
		}

		internal byte Direction
		{
			get { return m_Direction; }
			set { m_Direction = value; }
		}

		internal bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}

		internal bool Movable
		{
			get { return m_Movable; }
			set { m_Movable = value; }
		}

		internal string Name
		{
			get
			{
				if (m_Name != null && m_Name != "")
				{
					return m_Name;
				}
				else
				{
					return m_ItemID.ToString();
				}
			}
			set
			{
				if (value != null)
					m_Name = value.Trim();
				else
					m_Name = null;
			}
		}

		internal Layer Layer
		{
			get
			{
				if ((m_Layer < Layer.FirstValid || m_Layer > Layer.LastValid) &&
					((this.ItemID.ItemData.Flags & Ultima.TileFlag.Wearable) != 0 ||
					(this.ItemID.ItemData.Flags & Ultima.TileFlag.Armor) != 0 ||
					(this.ItemID.ItemData.Flags & Ultima.TileFlag.Weapon) != 0
					))
				{
					m_Layer = (Layer)this.ItemID.ItemData.Quality;
				}
				return m_Layer;
			}
			set
			{
				m_Layer = value;
			}
		}

		internal Item FindItemByID(ItemID id)
		{
			return FindItemByID(id, true);
		}

		internal Item FindItemByID(ItemID id, bool recurse)
		{
			foreach (Item t in m_Items)
			{
				Item item = t;
				if (item.ItemID == id)
				{
					return item;
				}
				else if (recurse)
				{
					item = item.FindItemByID(id, true);
					if (item != null)
						return item;
				}
			}
			return null;
		}

		internal int GetCount(ushort iid)
		{
			int count = 0;
			foreach (Item item in m_Items)
			{
				if (item.ItemID == iid)
					count += item.Amount;
				// fucking osi blank scrolls
				else if ((item.ItemID == 0x0E34 && iid == 0x0EF3) || (item.ItemID == 0x0EF3 && iid == 0x0E34))
					count += item.Amount;
				count += item.GetCount(iid);
			}

			return count;
		}

		internal object Container
		{
			get
			{
				if (m_Parent is Serial && UpdateContainer())
					m_NeedContUpdate.Remove(this);
				return m_Parent;
			}
			set
			{
				if ((m_Parent != null && m_Parent.Equals(value))
					|| (value is Serial && m_Parent is UOEntity && ((UOEntity)m_Parent).Serial == (Serial)value)
					|| (m_Parent is Serial && value is UOEntity && ((UOEntity)value).Serial == (Serial)m_Parent))
				{
					return;
				}

				if (m_Parent is Mobile)
					((Mobile)m_Parent).RemoveItem(this);
				else if (m_Parent is Item)
					((Item)m_Parent).RemoveItem(this);

				if (value is Mobile)
					m_Parent = ((Mobile)value).Serial;
				else if (value is Item)
					m_Parent = ((Item)value).Serial;
				else
					m_Parent = value;

				if (!UpdateContainer() && m_NeedContUpdate != null)
					m_NeedContUpdate.Add(this);
			}
		}

		internal bool UpdateContainer()
		{
			if (!(m_Parent is Serial) || Deleted)
				return true;

			object o = null;
			Serial contSer = (Serial)m_Parent;
			if (contSer.IsItem)
				o = World.FindItem(contSer);
			else if (contSer.IsMobile)
				o = World.FindMobile(contSer);

			if (o == null)
				return false;

			m_Parent = o;

			if (m_Parent is Item)
				((Item)m_Parent).AddItem(this);
			else if (m_Parent is Mobile)
				((Mobile)m_Parent).AddItem(this);

			if (World.Player != null && (IsChildOf(World.Player.Backpack) || IsChildOf(World.Player.Quiver)))
			{
				if (m_IsNew)
				{
					if (m_AutoStack)
						AutoStackResource();

                    if (RazorEnhanced.Settings.General.ReadBool("AutoSearch")
                        && IsContainer
                        && !(IsPouch && RazorEnhanced.Settings.General.ReadBool("NoSearchPouches"))
                        && !this.IsBagOfSending
                        )
                    {
                        PacketHandlers.IgnoreGumps.Add(this);
                        PlayerData.DoubleClick(this);

                        for (int c = 0; c < Contains.Count; c++)
                        {
                            Item icheck = (Item)Contains[c];
                            if (icheck.IsContainer)
                            {
                                if (icheck.IsPouch && RazorEnhanced.Settings.General.ReadBool("NoSearchPouches"))
                                    continue;
                                if (icheck.IsBagOfSending)
                                    continue;
								PacketHandlers.IgnoreGumps.Add(icheck);
								PlayerData.DoubleClick(icheck);
							}
						}
					}
				}
			}
			m_AutoStack = m_IsNew = false;

			return true;
		}

		private static List<Item> m_NeedContUpdate = new List<Item>();

		internal static void UpdateContainers()
		{
			int i = 0;
			while (i < m_NeedContUpdate.Count)
			{
				if ((m_NeedContUpdate[i]).UpdateContainer())
					m_NeedContUpdate.RemoveAt(i);
				else
					i++;
			}
		}

		private static List<Serial> m_AutoStackCache = new List<Serial>();

		internal void AutoStackResource()
		{
			if (!IsResource || !RazorEnhanced.Settings.General.ReadBool("AutoStack") || m_AutoStackCache.Contains(Serial))
				return;

			foreach (Item check in World.Items.Values)
			{
				if (check.Container == null && check.ItemID == ItemID && check.Hue == Hue && Utility.InRange(World.Player.Position, check.Position, 2))
				{
					DragDropManager.DragDrop(this, check);
					m_AutoStackCache.Add(Serial);
					return;
				}
			}

			DragDropManager.DragDrop(this, World.Player.Position);
			m_AutoStackCache.Add(Serial);
		}

		internal object RootContainer
		{
			get
			{
				int die = 100;
				object cont = this.Container;
				while (cont != null && cont is Item && die-- > 0)
					cont = ((Item)cont).Container;

				return cont;
			}
		}

		internal bool IsChildOf(object parent)
		{
			Serial parentSerial = 0;
			if (parent is Mobile)
				return parent == RootContainer;
			else if (parent is Item)
				parentSerial = ((Item)parent).Serial;
			else
				return false;

			object check = this;
			int die = 100;
			while (check != null && check is Item && die-- > 0)
			{
				if (((Item)check).Serial == parentSerial)
					return true;
				else
					check = ((Item)check).Container;
			}

			return false;
		}

		internal Point3D GetWorldPosition()
		{
			int die = 100;
			object root = this.Container;
			while (root != null && root is Item && ((Item)root).Container != null && die-- > 0)
				root = ((Item)root).Container;

			if (root is Item)
				return ((Item)root).Position;
			else if (root is Mobile)
				return ((Mobile)root).Position;
			else
				return this.Position;
		}

		private void AddItem(Item item)
		{
			for (int i = 0; i < m_Items.Count; i++)
			{
				if (m_Items[i] == item)
					return;
			}

			m_Items.Add(item);
		}

		private void RemoveItem(Item item)
		{
			try
			{
				m_Items.Remove(item);
			}
			catch { }
		}

		internal byte GetPacketFlags()
		{
			byte flags = 0;

			if (!m_Visible)
			{
				flags |= 0x80;
			}

			if (m_Movable)
			{
				flags |= 0x20;
			}

			return flags;
		}

		internal int DistanceTo(Mobile m)
		{
			int x = Math.Abs(this.Position.X - m.Position.X);
			int y = Math.Abs(this.Position.Y - m.Position.Y);

			return x > y ? x : y;
		}

		internal void ProcessPacketFlags(byte flags)
		{
			m_Visible = ((flags & 0x80) == 0);
			m_Movable = ((flags & 0x20) != 0);
		}

		//private Timer m_RemoveTimer = null;

	/*	internal void RemoveRequest()
		{
			if (m_RemoveTimer == null)
				m_RemoveTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(0.25), new TimerCallback(Remove));
			else if (m_RemoveTimer.Running)
				m_RemoveTimer.Stop();

			m_RemoveTimer.Start();
		}*/

		/*internal bool CancelRemove()
		{
			if (m_RemoveTimer != null && m_RemoveTimer.Running)
			{
				m_RemoveTimer.Stop();
				return true;
			}
			else
			{
				return false;
			}
		}*/

		internal override void Remove()
		{
			if (IsMulti)
			 	Assistant.UOAssist.PostRemoveMulti(this);

			List<Item> rem = new List<Item>(m_Items);
			m_Items.Clear();

			foreach (Item r in rem)
				r.Remove();

			if (m_Parent is Mobile)
				((Mobile)m_Parent).RemoveItem(this);
			else if (m_Parent is Item)
				((Item)m_Parent).RemoveItem(this);

			World.RemoveItem(this);
			base.Remove();
		}

		internal override void OnPositionChanging(Point3D newPos)
		{
			if (IsMulti && this.Position != Point3D.Zero && newPos != Point3D.Zero && this.Position != newPos)
			{
			 	Assistant.UOAssist.PostRemoveMulti(this);
			 	Assistant.UOAssist.PostAddMulti(m_ItemID, newPos);
			}
			base.OnPositionChanging(newPos);
		}

		internal List<Item> Contains { get { return m_Items; } }

		// possibly 4 bit x/y - 16x16?
		internal byte GridNum
		{
			get { return m_GridNum; }
			set { m_GridNum = value; }
		}

		internal bool OnGround { get { return Container == null; } }


        internal static ConcurrentHashSet<int> LoadContainersData()
        {
            lock (LockingVar)
            {
                string pathName = Path.Combine(Assistant.Engine.RootPath, "Data", "ContainersData.json");
                if (File.Exists(pathName))
                {
                    string containersData = File.ReadAllText(pathName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentHashSet<int>>(containersData);
                }
                pathName = Path.Combine(Assistant.Engine.RootPath, "Config", "ContainersData.json");
                if (File.Exists(pathName))
                {
                    string containersData = File.ReadAllText(pathName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentHashSet<int>>(containersData);
                }
            }
                return new ConcurrentHashSet<int>();
        }

        internal static ConcurrentHashSet<int> m_containerID = LoadContainersData();

		internal bool IsContainer
		{
			get
			{
				if (IsCorpse)
					return false;

				if (m_Items.Count > 0)
					return true;

				if (m_containerID.Contains(m_ItemID.Value))
					return true;
				else
					return false;
			}
		}

		internal bool IsBagOfSending
		{
			get
			{
				return Hue >= 0x0400 && m_ItemID.Value == 0xE76;
			}
		}

		internal bool IsInBank
		{
			get
			{
				if (m_Parent is Item)
					return ((Item)m_Parent).IsInBank;
				else if (m_Parent is Mobile)
					return this.Layer == Layer.Bank;
				else
					return false;
			}
		}

		internal bool IsNew
		{
			get { return m_IsNew; }
			set { m_IsNew = value; }
		}

		internal bool AutoStack
		{
			get { return m_AutoStack; }
			set { m_AutoStack = value; }
		}

		internal bool IsMulti
		{
			get { return m_ItemID.Value >= 0x4000; }
		}

		internal bool IsPouch
		{
			get { return m_ItemID.Value == 0x0E79; }
		}

		internal bool IsCorpse
		{
			get { return m_ItemID.Value == 0x2006 || (m_ItemID.Value >= 0x0ECA && m_ItemID.Value <= 0x0ED2); }
		}

        internal static ConcurrentHashSet<int> LoadDoorData()
        {
            lock (LockingVar)
            {

                string pathName = Path.Combine(Assistant.Engine.RootPath, "Data", "DoorData.json");
                if (File.Exists(pathName))
                {
                    string doorData = File.ReadAllText(pathName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentHashSet<int>>(doorData);
                }
                pathName = Path.Combine(Assistant.Engine.RootPath, "Config", "DoorData.json");
                if (File.Exists(pathName))
                {
                    string doorData = File.ReadAllText(pathName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentHashSet<int>>(doorData);
                }
            }
            return new ConcurrentHashSet<int>();
        }

        internal static ConcurrentHashSet<int> DoorData = LoadDoorData();

        internal bool IsDoor
		{
            get
            {
                ushort iid = m_ItemID.Value;
                return DoorData.Contains(iid);
            }
		}


        internal static ConcurrentHashSet<int> LoadNotLootableData()
        {
            lock (LockingVar)
            {

                string pathName = Path.Combine(Assistant.Engine.RootPath, "Data", "NotLootableData.json");
                if (File.Exists(pathName))
                {
                    string notLootableData = File.ReadAllText(pathName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentHashSet<int>>(notLootableData);
                }
                pathName = Path.Combine(Assistant.Engine.RootPath, "Config", "NotLootableData.json");
                if (File.Exists(pathName))
                {
                    string notLootableData = File.ReadAllText(pathName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentHashSet<int>>(notLootableData);
                }
            }
            return new ConcurrentHashSet<int>();
        }

        //hair beards and horns
        static ConcurrentHashSet<int> NotLootable = LoadNotLootableData();
        internal  bool IsLootable
        {
            // Eventine owner found looting items was trying to loot hair and beards.
            // This caused big delay, so I will introduce this "lootable" property
            // but for now all its going to do is return true for anything
            // except beards and hair
            get
            {
                ushort iid = m_ItemID.Value;
                return !NotLootable.Contains(iid);
            }
        }

        internal bool IsResource
		{
			get
			{
				ushort iid = m_ItemID.Value;
				return (iid >= 0x19B7 && iid <= 0x19BA) || // ore
					(iid >= 0x09CC && iid <= 0x09CF) || // fishes
					(iid >= 0x1BDD && iid <= 0x1BE2) || // logs
					iid == 0x1779 || // granite / stone
					iid == 0x11EA || iid == 0x11EB // sand
				;
			}
		}

		internal bool IsPotion
		{
			get
			{
				return (m_ItemID.Value >= 0x0F06 && m_ItemID.Value <= 0x0F0D) ||
					m_ItemID.Value == 0x2790 || m_ItemID.Value == 0x27DB; // Ninja belt (works like a potion)
			}
		}

		internal bool IsVirtueShield
		{
			get
			{
				ushort iid = m_ItemID.Value;
				return (iid >= 0x1bc3 && iid <= 0x1bc5); // virtue shields
			}
		}

		internal bool IsTwoHanded
		{
			get
			{
				ushort iid = m_ItemID.Value;
				return (
						// everything in layer 2 except shields is 2handed
						Layer == Layer.LeftHand &&
						!((iid >= 0x1b72 && iid <= 0x1b7b) || IsVirtueShield) // shields
					) ||

					// and all of these layer 1 weapons:
					(iid == 0x13fc || iid == 0x13fd) || // hxbow
					(iid == 0x13AF || iid == 0x13b2) || // war axe & bow
					(iid >= 0x0F43 && iid <= 0x0F50) || // axes & xbow
					(iid == 0x1438 || iid == 0x1439) || // war hammer
					(iid == 0x1442 || iid == 0x1443) || // 2handed axe
					(iid == 0x1402 || iid == 0x1403) || // short spear
					(iid == 0x26c1 || iid == 0x26cb) || // aos gay blade
					(iid == 0x26c2 || iid == 0x26cc) || // aos gay bow
					(iid == 0x26c3 || iid == 0x26cd) // aos gay xbow
				;
			}
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", this.Name, this.Serial);
		}

		internal int Price
		{
			get { return m_Price; }
			set { m_Price = value; }
		}

		internal string BuyDesc
		{
			get { return m_BuyDesc; }
			set { m_BuyDesc = value; }
		}

		internal int HouseRevision
		{
			get { return m_HouseRev; }
			set { m_HouseRev = value; }
		}

		internal byte[] HousePacket
		{
			get { return m_HousePacket; }
			set { m_HousePacket = value; }
		}
	}

    internal class CorpseItem : Item
    {
        // Used for the open corpse option to ensure it is only openned first time
        internal bool Opened { get; set; }
        internal CorpseItem(Serial serial)
            : base(serial)
        {
            Opened = false;
        }

    }

    internal class MapItem : Item
    {

        private RazorEnhanced.Point2D m_PinPosition;
        internal RazorEnhanced.Point2D PinPosition
        {
            get { return m_PinPosition; }
            set
            {
                m_PinPosition = value;
                FixUpLocation();
            }
        }

        public RazorEnhanced.Point2D m_MapOrigin;
        internal RazorEnhanced.Point2D MapOrigin
        {
            get { return m_MapOrigin; }
            set
            {
                m_MapOrigin = value;
                FixUpLocation();
            }
        }

        public RazorEnhanced.Point2D m_MapEnd;
        internal RazorEnhanced.Point2D MapEnd
        {
            get { return m_MapEnd; }
            set { m_MapEnd = value; }
        }


        public int m_Multiplier;
        internal int Multiplier
        {
            get { return m_Multiplier; }
            set { m_Multiplier = value; }
        }


        public ushort m_Facet;
        internal ushort Facet
        {
            get { return m_Facet; }
            set { m_Facet = value; }
        }
        int m_FakePropIndex;
        internal MapItem(Serial serial)
            : base(serial)
        {
            m_PinPosition = new RazorEnhanced.Point2D();
            m_MapOrigin = new RazorEnhanced.Point2D();
            m_MapEnd = new RazorEnhanced.Point2D();
            m_Facet = 0;
            m_FakePropIndex = 0;

        }
        void FixUpLocation()
        {
            // This has issues with the fakeIndex do for now quit doing it
            try
            {
                if (Multiplier == 0)
                {
                    return;
                }
                int xCoord = m_MapOrigin.X + (Multiplier * m_PinPosition.X);
                int yCoord = m_MapOrigin.Y + (Multiplier * m_PinPosition.Y);
                string location = String.Format("Location({0}, {1})",
                    xCoord,
                    yCoord
                    );
                // The m_FakePropIndex at this point was beyond the end of the array
                m_ObjPropList.Content[m_FakePropIndex] = new Assistant.ObjectPropertyList.OPLEntry(1042971, location);
            }
            catch (Exception e)
            {
                // shold do something, but ...
            }

        }
        override internal void ReadPropertyList(PacketReader p)
        {
            base.ReadPropertyList(p);
            string location = String.Format("Location({0}, {1}",
                m_MapOrigin.X + (2 * m_PinPosition.X),
                m_MapOrigin.Y + (2 * m_PinPosition.Y)
                );
            m_FakePropIndex = m_ObjPropList.Content.Count;
            // Fake+0 = coordinates
            m_ObjPropList.Content.Add(new Assistant.ObjectPropertyList.OPLEntry(1042971, ""));
            // Fake+1 = THB number
            m_ObjPropList.Content.Add(new Assistant.ObjectPropertyList.OPLEntry(1042971, ""));
            // Fake+2 = THB Name
            m_ObjPropList.Content.Add(new Assistant.ObjectPropertyList.OPLEntry(1042971, ""));
            FixUpLocation();
        }
    }
}
