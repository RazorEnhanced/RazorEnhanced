using System;
using System.Collections.Generic;
using Assistant;

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
		public static Item Backpack { get { return Assistant.World.Player.Backpack; } }
		public static Item BankBox { get { return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bank); } }
		public static Item Quiver { get { return Assistant.World.Player.Quiver; } }
		public static Item Mount { get { return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Mount); } }
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
		public static Item GetItemOnLayer(String Layer)
		{
			switch (Layer)
			{
				case "RightHand":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand);
				case "LeftHand":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand);
				case "Shoes":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes);
				case "Pants":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants);
				case "Shirt":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt);
				case "Head":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head);
				case "Gloves":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves);
				case "Ring":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring);
				case "Neck":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck);
				case "Hair":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Hair);
				case "Waist":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist);
				case "InnerTorso":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso);
				case "Bracelet":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet);
				case "FacialHair":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.FacialHair);
				case "MiddleTorso":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso);
				case "Earrings":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings);
				case "Arms":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms);
				case "Cloak":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak);
				case "OuterTorso":
                    return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso);
				case "OuterLegs":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs);
				case "InnerLegs":
					return Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs);
				default:
                    Player.SendMessage("Invalid layer name: " + Layer);
                    return null;
			}
		}
		public static void UnEquipItemByLayer(String Layer)
		{
			switch (Layer)
			{
				case "RightHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "LeftHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Shoes":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Pants":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Shirt":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Head":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Gloves":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Ring":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Neck":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Waist":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "InnerTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Bracelet":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "MiddleTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Earrings":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Arms":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "Cloak":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "OuterTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "OuterLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				case "InnerLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("No item found on layer: " + Layer);
					break;
				default:
                    Player.SendMessage("Invalid layer name: " + Layer);
					break;
			}
		}

        public static void EquipItem(uint itemserial)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if (item == null)
            {
                Player.SendMessage("Item serial: (" + itemserial + ") not found");
                return;
            }

            if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), Assistant.World.Player.Position) > 3)
            {
                Player.SendMessage("Item serial: (" + itemserial + ") too away");
                return;
            }
            Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, item.Layer);
        }
        public static void EquipItem(Item item)
        {
            if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), Assistant.World.Player.Position) > 3)
            {
                Player.SendMessage("Item serial: (" + item.Serial + ") too away");
                return;
            }
            Assistant.DragDropManager.DragDrop(item, Assistant.World.Player, item.Layer);
        }

        public static bool CheckLayer(String Layer)
        {
            switch (Layer)
            {
                case "RightHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand) != null)
                        return true;
                    else
                        return false;
                case "LeftHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand) != null)
                        return true;
                    else
                        return false;
                case "Shoes":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes) != null)
                        return true;
                    else
                        return false;
                case "Pants":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants) != null)
                        return true;
                    else
                        return false;
                case "Shirt":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt) != null)
                        return true;
                    else
                        return false;
                case "Head":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head) != null)
                        return true;
                    else
                        return false;
                case "Gloves":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves) != null)
                        return true;
                    else
                        return false;
                case "Ring":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring) != null)
                        return true;
                    else
                        return false;
                case "Neck":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck) != null)
                        return true;
                    else
                        return false;
                case "Waist":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist) != null)
                        return true;
                    else
                        return false;
                case "InnerTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso) != null)
                        return true;
                    else
                        return false;
                case "Bracelet":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet) != null)
                        return true;
                    else
                        return false;
                case "MiddleTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso) != null)
                        return true;
                    else
                        return false;
                case "Earrings":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings) != null)
                        return true;
                    else
                        return false;
                case "Arms":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms) != null)
                        return true;
                    else
                        return false;
                case "Cloak":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak) != null)
                        return true;
                    else
                        return false;
                case "OuterTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso) != null)
                        return true;
                    else
                        return false;
                case "OuterLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs) != null)
                        return true;
                    else
                        return false;
                case "InnerLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs) != null)
                        return true;
                    else
                        return false;
                default:
                    Player.SendMessage("Invalid layer name: " + Layer);
                    return false;
            }
        }
        
        // Sysmessage
		public static void SendMessage(Assistant.LocString loc)
		{
			Assistant.World.Player.SendMessage(loc);
		}

		public static void SendMessage(string msg)
		{
			Assistant.World.Player.SendMessage(msg);
		}
	}
}
