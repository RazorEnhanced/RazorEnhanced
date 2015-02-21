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
        public static uint Backpack { get { return Assistant.World.Player.Backpack.Serial; } }
        public static uint BankBox { get { return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bank).Serial; } }
        public static uint Quiver { get { return Assistant.World.Player.Quiver.Serial; } }
        public static uint Mount { get { return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Mount).Serial; } }
        public static int Gold { get { return Convert.ToInt32(Assistant.World.Player.Gold); } }
        public static int Luck { get { return Assistant.World.Player.Luck; } }
        public static int Body { get { return Assistant.World.Player.Body; } }
        public static bool InParty { get { return Assistant.World.Player.InParty; } }
        public static uint Serial { get { return Assistant.World.Player.Serial; } }
        // Follower
        public static int FollowersMax { get { return Assistant.World.Player.FollowersMax; } }
        public static int Followers { get { return Assistant.World.Player.Followers; } }
        // Weight
        public static int MaxWeight { get { return Assistant.World.Player.MaxWeight; } }
        public static int Weight { get { return Assistant.World.Player.Weight; } }
        // Position
        public static Assistant.Point3D Position { get { return Assistant.World.Player.Position; } }
        // Layer
        public static uint GetItemOnLayer(String Layer)
        {
            switch (Layer)
            {
                case "RightHand":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand).Serial;
                case "LeftHand":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand).Serial;
                case "Shoes":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes).Serial;
                case "Pants":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants).Serial;
                case "Shirt":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt).Serial;
                case "Head":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head).Serial;
                case "Gloves":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves).Serial;
                case "Ring":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring).Serial;
                case "Neck":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck).Serial;
                case "Hair":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Hair).Serial;
                case "Waist":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist).Serial;
                case "InnerTorso":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso).Serial;
                case "Bracelet":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet).Serial;
                case "FacialHair":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.FacialHair).Serial;
                case "MiddleTorso":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso).Serial;
                case "Earrings":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings).Serial;
                case "Arms":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms).Serial;
                case "Cloak":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak).Serial;
                case "OuterTorso":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso).Serial;
                case "OuterLegs":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs).Serial;
                case "InnerLegs":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs).Serial;
                default:
                    // TODO: Sysmessage Invalid Layer name
                    return 0;
            }
        }
        public static void UnEquipItemByLayer(String Layer)
        {
            Assistant.Item backpack = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack);
            switch (Layer)
            {
                case "RightHand":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand)))
                            Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "LeftHand":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Shoes":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Pants":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Shirt":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Head":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Gloves":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Ring":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Neck":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Waist":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "InnerTorso":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Bracelet":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "MiddleTorso":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Earrings":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Arms":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "Cloak":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "OuterTorso":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "OuterLegs":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                case "InnerLegs":
                    if (Convert.ToBoolean(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs)))
                        Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
                    else
                        {
                            // TODO: Sysmessage Layer Empty
                        }
                    break;
                default:
                    // TODO: Sysmessage Invalid Layer name
                    break;
            }
        }
        public static void EquipItemOnLayer(uint itemserial, String Layer)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if ( item == null)
                {
                    // TODO: Sysmessage Invalid Item
                    return;
                }

            if ( item.Container == null && Assistant.Utility.Distance( item.GetWorldPosition(), Assistant.World.Player.Position ) > 3 )
				{ 
                    // TODO: Sysmessage Item too Away
                    return;
                }
            Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, item.Layer);
            switch (Layer)
            {
                case "RightHand":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.RightHand);
                    break;
                case "LeftHand":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.LeftHand);
                    break;
                case "Shoes":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Shoes);
                    break;
                case "Pants":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Pants);
                    break;
                case "Shirt":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Shirt);
                    break;
                case "Head":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Head);
                    break;
                case "Gloves":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Gloves);
                    break;
                case "Ring":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Ring);
                    break;
                case "Neck":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Neck);
                    break;
                case "Waist":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Waist);
                    break;
                case "InnerTorso":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.InnerTorso);
                    break;
                case "Bracelet":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Bracelet);
                    break;
                case "MiddleTorso":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.MiddleTorso);
                    break;
                case "Earrings":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Earrings);
                    break;
                case "Arms":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Arms);
                    break;
                case "Cloak":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.Cloak);
                    break;
                case "OuterTorso":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.OuterTorso);
                    break;
                case "OuterLegs":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.OuterLegs);
                    break;
                case "InnerLegs":
                    Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, Assistant.Layer.InnerLegs);
                    break;
                default:
                    // TODO: Sysmessage Invalid Layer name
                    break;
            }
        }
	}
}
