using System;
using System.Collections.Generic;

namespace RazorEnhanced
{
	public class Player
	{
        // Stats
		public static int Hits { get { return Assistant.World.Player.Hits; } }
        public static int HitsMax { get { return Assistant.World.Player.HitsMax; } }
        public static int Str { get { return Assistant.World.Player.Str; } }
        public static int Mana { get { return Assistant.World.Player.Mana; } }
        public static int ManaMax { get { return Assistant.World.Player.ManaMax; } }
        public static int Int { get { return Assistant.World.Player.Int; } }
        public static int Stam { get { return Assistant.World.Player.Stam; } }
        public static int StamMax { get { return Assistant.World.Player.StamMax; } }
        public static int Dex { get { return Assistant.World.Player.Dex; } }
        public static int StatCap { get { return Assistant.World.Player.StatCap; } }
        // Resistance
        public static int AR { get { return Assistant.World.Player.AR; } }
        public static int FireResistance { get { return Assistant.World.Player.FireResistance; } }
        public static int ColdResistance { get { return Assistant.World.Player.ColdResistance; } }
        public static int EnergyResistance { get { return Assistant.World.Player.EnergyResistance; } }
        public static int PoisonResistance { get { return Assistant.World.Player.PoisonResistance; } }
        // Flags
        public static bool IsGhost { get { return Assistant.World.Player.IsGhost; } }
        public static bool Poisoned { get { return Assistant.World.Player.Poisoned; } }
        public static bool Blessed { get { return Assistant.World.Player.Blessed; } }
        public static bool Visible { get { return Assistant.World.Player.Visible; } }
        
        public static bool Warmode { get { return Assistant.World.Player.Warmode; } }
        // Self
        public static bool Female { get { return Assistant.World.Player.Female; } }
        public static String Name { get { return Assistant.World.Player.Name; } }
        public static Assistant.Item Backpack { get { return Assistant.World.Player.Backpack; } }
        public static Assistant.Item Quiver { get { return Assistant.World.Player.Quiver; } }
        public static int Gold { get { return Convert.ToInt32(Assistant.World.Player.Gold); } }
        public static int Luck { get { return Assistant.World.Player.Luck; } }
        public static int Body { get { return Assistant.World.Player.Body; } }
        public static bool InParty { get { return Assistant.World.Player.InParty; } }
        public static Assistant.Serial Serial { get { return Assistant.World.Player.Serial; } }
        // Follower
        public static int FollowersMax { get { return Assistant.World.Player.FollowersMax; } }
        public static int Followers { get { return Assistant.World.Player.Followers; } }
        // Weight
        public static int MaxWeight { get { return Assistant.World.Player.MaxWeight; } }
        public static int Weight { get { return Assistant.World.Player.Weight; } }
        // Position
        public static Assistant.Point3D Position { get { return Assistant.World.Player.Position; } }
	}
}
