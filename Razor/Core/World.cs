using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Assistant
{
    internal class World
    {
        private static readonly ConcurrentDictionary<Serial, Item> m_Items;
        private static long m_ItemUpdateCount;
        private static readonly ConcurrentDictionary<Serial, Mobile> m_Mobiles;
        private static long m_MobileUpdateCount;
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
        internal static long itemUpdateCounter { get { return m_ItemUpdateCount; } }
        internal static ConcurrentDictionary<Serial, Mobile> Mobiles { get { return m_Mobiles; } }
        internal static long mobileUpdateCounter { get { return m_MobileUpdateCount; } }
        internal static ConcurrentDictionary<int, RazorEnhanced.Multi.MultiData> Multis { get { return m_Multis; } }

        internal static Item FindItem(Serial serial)
        {
            Item item;
            m_Items.TryGetValue(serial, out item);
            return item;
        }

        internal static Mobile FindMobile(Serial serial)
        {
            Mobile mobile;
            m_Mobiles.TryGetValue(serial, out mobile);
            return mobile;
        }

        internal static List<CorpseItem> CorpsesInRange(int range)
        {
            List<CorpseItem> list = new List<CorpseItem>();

            if (World.Player == null)
                return list;

            foreach (Item m in World.Items.Values)
            {
                if (m.ItemID == 0x2006)
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
            m_ItemUpdateCount++;
        }

        internal static void AddMulti(Item item)
        {           
            Ultima.MultiComponentList multiinfo = Ultima.Multis.GetComponents(item.ItemID);
            if (multiinfo.Count == 0)
                multiinfo = new Ultima.MultiComponentList(item.ItemID, item.Position.X, item.Position.Y);

            int stairSpace = 1;
            m_Multis[item.Serial] = new RazorEnhanced.Multi.MultiData(item.Position, new Point2D(item.Position.X + multiinfo.Min.X, item.Position.Y + multiinfo.Min.Y), new Point2D(item.Position.X + multiinfo.Max.X, item.Position.Y + multiinfo.Max.Y + stairSpace));
        }

        internal static void AddMobile(Mobile mob)
        {
            m_Mobiles[mob.Serial] = mob;
            m_MobileUpdateCount++;
        }

        internal static void RemoveMobile(Mobile mob)
        {
            if (m_Mobiles.TryRemove(mob.Serial, out Mobile removed) == true)
            {
                m_MobileUpdateCount++;
            }
        }

        internal static void RemoveItem(Item item)
        {
            if (item.IsMulti)
                RemoveMulti(item);

            if (m_Items.TryRemove(item.Serial, out Item removed) == true)
            {
                m_ItemUpdateCount++;
            }

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
