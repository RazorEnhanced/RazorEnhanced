using System.Collections.Concurrent;
using System.Collections.Generic;
using Ultima;

namespace Assistant
{
    internal class World
    {
        private static readonly ConcurrentDictionary<Serial, Item> m_Items;
        private static readonly ConcurrentDictionary<Serial, Mobile> m_Mobiles;
        private static readonly ConcurrentDictionary<int, RazorEnhanced.Multi.MultiData> m_Multis;
        private static PlayerData m_Player;
        private static string m_ShardName, m_PlayerName, m_AccountName;
        private static readonly ConcurrentDictionary<ushort, string> m_Servers;

        static World()
        {
            m_Servers = new ConcurrentDictionary<ushort, string>();
            m_Items = new ConcurrentDictionary<Serial, Item>();
            m_Mobiles = new ConcurrentDictionary<Serial, Mobile>();
            m_Multis = new ConcurrentDictionary<int, RazorEnhanced.Multi.MultiData>();
            m_ShardName = "[None]";
        }

        internal static ConcurrentDictionary<ushort, string> Servers { get { return m_Servers; } }
        internal static ConcurrentDictionary<Serial, Item> Items { get { return m_Items; } }
        internal static ConcurrentDictionary<Serial, Mobile> Mobiles { get { return m_Mobiles; } }
        internal static ConcurrentDictionary<int, RazorEnhanced.Multi.MultiData> Multis { get { return m_Multis; } }

        internal static Item FindItem(Serial serial)
        {
            Item item;
            m_Items.TryGetValue(serial, out item);
            return item;
        }

        internal static List<Item> FindItems(int x, int y, int z)
        {
            List<Item> items = new List<Item>();
            foreach (var item in m_Items.Values)
            {
                if (item.Position.X == x && item.Position.Y == y)
                    items.Add(item);
            }
            return items;
        }


        internal static Mobile FindMobile(Serial serial)
        {
            Mobile mobile;
            m_Mobiles.TryGetValue(serial, out mobile);
            return mobile;
        }

        internal static List<UOEntity> FindAllEntityByID(ushort type, int color)
        {
            var typeId = new TypeID(type);
            List<UOEntity> entities = new List<UOEntity>();
            foreach (var item in World.Items.Values)
            {
                if (item.TypeID == typeId ) 
                    if (color == -1 || item.Hue == (ushort)color)
                        entities.Add(item);
            }
            foreach (var mobile in World.Mobiles.Values)
            {
                if (mobile.TypeID == typeId ) 
                    if (color == -1 || mobile.Hue == (ushort)color)
                        entities.Add(mobile);
            }

            return entities;
        }

        internal static List<CorpseItem> CorpsesInRange(int range)
        {
            List<CorpseItem> list = new List<CorpseItem>();

            if (World.Player == null)
                return list;

            foreach (Item m in World.Items.Values)
            {
                if (m.TypeID == 0x2006)
                {
                    CorpseItem corpse = m as CorpseItem;
                    if (corpse != null)
                    {
                        if (Utility.InRange(World.Player.Position, corpse.Position, range))
                        {
                            list.Add(corpse);
                        }
                    }
                }
            }

            return list;
        }

        internal static List<Mobile> MobilesInRange(int range)
        {
            List<Mobile> list = new List<Mobile>();

            if (World.Player == null)
                return list;

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (Utility.InRange(World.Player.Position, m.Position, World.Player.VisRange))
                    list.Add(m);
            }

            return list;
        }

        internal static List<Mobile> MobilesInRange()
        {
            if (Player == null)
                return MobilesInRange(18);
            else
                return MobilesInRange(Player.VisRange);
        }

        internal static void AddItem(Item item)
        {
            m_Items[item.Serial] = item;
        }

        internal static void AddMulti(Item item)
        {
            Ultima.MultiComponentList multiinfo = Ultima.Multis.GetComponents(item.TypeID);
            if (multiinfo.Count == 0)
                multiinfo = new Ultima.MultiComponentList(item.TypeID, item.Position.X, item.Position.Y);

            int stairSpace = 1;
            m_Multis[item.Serial] = new RazorEnhanced.Multi.MultiData(item.Position, new Point2D(item.Position.X + multiinfo.Min.X, item.Position.Y + multiinfo.Min.Y), new Point2D(item.Position.X + multiinfo.Max.X, item.Position.Y + multiinfo.Max.Y + stairSpace));
        }

        internal static void AddMobile(Mobile mob)
        {
            m_Mobiles[mob.Serial] = mob;
        }

        internal static void RemoveMobile(Mobile mob)
        {
            Mobile removed;
            m_Mobiles.TryRemove(mob.Serial, out removed);
        }

        internal static void RemoveItem(Item item)
        {
            if (item.IsMulti)
                RemoveMulti(item);

            m_Items.TryRemove(item.Serial, out Item removed);

            /*  while (m_Items.ContainsKey(item.Serial))
                {
                    if (!m_Items.TryRemove(item.Serial, out Item removed))
                        RazorEnhanced.AutoLoot.AddLog("Fail item remove" + item.Name);
                }*/
        }

        internal static void RemoveMulti(Item item)
        {
            m_Multis.TryRemove(item.Serial, out RazorEnhanced.Multi.MultiData removed);
        }

        internal static PlayerData Player
        {
            get { return m_Player; }
            set { m_Player = value; }
        }

        internal static string ShardName
        {
            get { return m_ShardName; }
            set { m_ShardName = value; }
        }

        internal static string OrigPlayerName
        {
            get { return m_PlayerName; }
            set { m_PlayerName = value; }
        }

        internal static string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
    }
}
