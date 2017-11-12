using System;
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
		Unused_x9 = 0x09,
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

	internal class Item : UOEntity
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
		private List<Serial> m_Serials;

		private bool m_IsNew;
		private bool m_AutoStack;

		private byte[] m_HousePacket;
		private int m_HouseRev;

		private byte m_GridNum;

		private bool m_Updated;

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

		internal override void AfterLoad()
		{
			m_Items = new List<Item>();

			foreach (Serial serial in m_Serials)
			{
				Item item = World.FindItem(serial);

				if (item != null)
				{
					m_Items.Add(item);
				}
			}

			UpdateContainer();
		}

		internal Item(Serial serial)
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

					if (IsContainer && (!IsPouch || !RazorEnhanced.Settings.General.ReadBool("NoSearchPouches")) && RazorEnhanced.Settings.General.ReadBool("AutoSearch"))
					{
						PacketHandlers.IgnoreGumps.Add(this);
						PlayerData.DoubleClick(this);

						foreach (Item icheck in Contains)
						{
							if (icheck.IsContainer && (!icheck.IsPouch || !RazorEnhanced.Settings.General.ReadBool("NoSearchPouches")))
							{
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
			if (m_Items.Any(i => i == item))
			{
				return;
			}
			m_Items.Add(item);
		}

		private void RemoveItem(Item item)
		{
			m_Items.Remove(item);
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

		private Timer m_RemoveTimer = null;

		internal void RemoveRequest()
		{
			if (m_RemoveTimer == null)
				m_RemoveTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(0.25), new TimerCallback(Remove));
			else if (m_RemoveTimer.Running)
				m_RemoveTimer.Stop();

			m_RemoveTimer.Start();
		}

		internal bool CancelRemove()
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
		}

		internal override void Remove()
		{
			if (IsMulti)
				ClientCommunication.PostRemoveMulti(this);

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
				ClientCommunication.PostRemoveMulti(this);
				ClientCommunication.PostAddMulti(m_ItemID, newPos);
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

		internal bool IsContainer
		{
			get
			{
				if (IsCorpse)
					return false;

				if (m_Items.Count > 0)
					return true;

				if (ContainerDataTable.Table.Contains(m_ItemID.Value))
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

		internal bool IsDoor
		{
			get
			{
				ushort iid = m_ItemID.Value;
				if (iid >= 1653 && iid <= 1668) // Metal Door
					return true;
				else if (iid >= 8173 && iid <= 8188) // Metal Gate
					return true;
				else if (iid >= 1685 && iid <= 1700) // Rattan Door
					return true;
				else if (iid >= 1701 && iid <= 1716) // Dark Wood Door
					return true;
				else if (iid >= 1717 && iid <= 1732) // Wood Door
					return true;
				else if (iid >= 1749 && iid <= 1764) // Light Wood Door
					return true;
				else if (iid >= 1765 && iid <= 1780) // Wood and Metal Door
					return true;
				else if (iid >= 2084 && iid <= 2099) // Tall Wrought Iron Gate
					return true;
				else if (iid >= 2105 && iid <= 2120) // Light Wood Gate
					return true;
				else if (iid >= 2124 && iid <= 2139) // Short Wrought Iron Gate
					return true;
				else if (iid >= 2150 && iid <= 2165) // Dark Wood Gate
					return true;
				else if (iid >= 804 && iid <= 819) // Weathered Stone Secret Door
					return true;
				else if (iid >= 820 && iid <= 835) // Dark Wood Secret Door
					return true;
				else if (iid >= 836 && iid <= 851) // Light Wood Secret Door
					return true;
				else if (iid >= 852 && iid <= 867) // Grey Stone Secret Door
					return true;
				else if ((iid >= 9247 && iid <= 9248) || (iid >= 9251 && iid <= 9252)) // Japanese Doors
					return true;
				else if (iid >= 10765 && iid <= 10772) // Sliding Doors 1
					return true;
				else if (iid >= 10757 && iid <= 10764) // Sliding Doors 2
					return true;
				else if (iid >= 10773 && iid <= 10780) // Sliding Doors 3
					return true;
				else if ((iid >= 12716 && iid <= 12719) || (iid >= 11590 && iid <= 11593)) // Elvan Wood Door
					return true;
				else if ((iid >= 11619 && iid <= 11622) || (iid >= 12704 && iid <= 12707)) // Elvan White Wooden Door 1
					return true;
				else if ((iid >= 11623 && iid <= 11626) || (iid >= 12708 && iid <= 12710)) // Elvan Ornate Door
					return true;
				else if ((iid >= 11627 && iid <= 11630) || (iid >= 12712 && iid <= 12715)) // Elvan Kia Wood Door 2
					return true;
				else if ((iid >= 12700 && iid <= 12703) || (iid >= 12258 && iid <= 12261)) // Elvan Moon Door
					return true;
				else if (iid >= 13947 && iid <= 13962) // Crystal Door
					return true;
				else if (iid >= 13963 && iid <= 13978) // Shadow Door
					return true;
				else if (iid >= 16541 && iid <= 16546) // Gargish Carved Green Door
					return true;
				else if (iid >= 16652 && iid <= 16659) // Gargish Brown Door
					return true;
				else if (iid >= 16834 && iid <= 16841) // Sun Door
					return true;
				else if (iid >= 16847 && iid <= 16854) // Gargish Grey Door
					return true;
				else if (iid >= 17262 && iid <= 17277) // Gargish Set Door
					return true;
				else if (iid >= 18141 && iid <= 18148) // Ruined Door
					return true;
				else if (iid >= 19746 && iid <= 19753) // Gargish Blue Door
					return true;
				else if (iid >= 20680 && iid <= 20695) // Gargish Red Doors
					return true;
				else if (iid >= 20802 && iid <= 20809) // Gargish Prison Door
					return true;
				else if (iid >= 1733 && iid <= 1748) // Metal Doors (Dungeon)
					return true;
				else if (iid >= 788 && iid <= 803) // Hidden Door (brick)
					return true;
				else if (iid >= 1669 && iid <= 1684) // Barred Metal Gate
					return true;
				else if (iid >= 1781 && iid <= 1783) // Portcullis
					return true;
				else
					return false;
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

		internal void MakeHousePacket()
		{
			m_HousePacket = null;

			try
			{
				// 3 locations... which is right? all of them? wtf?
				//"Desktop/{0}/{1}/{2}/Multicache.dat", World.AccountName, World.ShardName, World.OrigPlayerName
				//"Desktop/{0}/{1}/{2}/Multicache.dat", World.AccountName, World.ShardName, World.Player.Name );
				//"Desktop/{0}/Multicache.dat", World.AccountName );
				string path = Ultima.Files.GetFilePath(String.Format("Desktop/{0}/{1}/{2}/Multicache.dat", World.AccountName, World.ShardName, World.OrigPlayerName));
				if (path == null || path == "" || !File.Exists(path))
					return;

				using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
				{
					string line;
					reader.ReadLine(); // ver
					int skip = 0;
					int count = 0;
					while ((line = reader.ReadLine()) != null)
					{
						if (count++ < skip || line == "" || line[0] == ';')
							continue;

						string[] split = line.Split(' ', '\t');
						if (split.Length <= 0)
							return;

						skip = 0;
						Serial ser = (uint)Utility.ToInt32(split[0], 0);
						int rev = Utility.ToInt32(split[1], 0);
						int lines = Utility.ToInt32(split[2], 0);

						if (ser == this.Serial)
						{
							m_HouseRev = rev;
							MultiTileEntry[] tiles = new MultiTileEntry[lines];
							count = 0;

							Ultima.MultiComponentList mcl = Ultima.Multis.GetComponents(m_ItemID);

							while ((line = reader.ReadLine()) != null && count < lines)
							{
								split = line.Split(' ', '\t');

								tiles[count] = new MultiTileEntry();
								tiles[count].m_ItemID = (ushort)Utility.ToInt32(split[0], 0);
								tiles[count].m_OffsetX = (short)(Utility.ToInt32(split[1], 0) + mcl.Center.X);
								tiles[count].m_OffsetX = (short)(Utility.ToInt32(split[2], 0) + mcl.Center.Y);
								tiles[count].m_OffsetX = (short)Utility.ToInt32(split[3], 0);

								count++;
							}

							m_HousePacket = new DesignStateDetailed(Serial, m_HouseRev, mcl.Min.X, mcl.Min.Y, mcl.Max.X, mcl.Max.Y, tiles).Compile();
							break;
						}
						else
						{
							skip = lines;
						}
						count = 0;
					}
				}
			}
			catch// ( Exception e )
			{
				//Engine.LogCrash( e );
			}
		}
	}
}