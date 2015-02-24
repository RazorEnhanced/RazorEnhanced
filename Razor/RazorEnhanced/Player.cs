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
                    Player.SendMessage("Script Error: GetItemOnLayer: Invalid layer name: " + Layer);
                    return null;
			}
		}
		public static void UnEquipItemByLayer(String Layer)
		{
			switch (Layer)
			{
				case "RightHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount)); 
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial ));
                    }
                    else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "LeftHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Shoes":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Pants":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Shirt":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Head":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Gloves":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Ring":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Neck":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Waist":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "InnerTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Bracelet":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "MiddleTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Earrings":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Arms":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Cloak":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "OuterTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "OuterLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "InnerLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs) != null)
                    {
                        Assistant.Item item = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs);
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, World.Player.Backpack.Serial));
                    }
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				default:
                    Player.SendMessage("Script Error: UnEquipItemByLayer: Invalid layer name: " + Layer);
					break;
			}
		}

        public static void EquipItem(uint itemserial)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if (item == null)
            {
                Player.SendMessage("Script Error: EquipItem: Item serial: (" + itemserial + ") not found");
                return;
            }

            if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), Assistant.World.Player.Position) > 3)
            {
                Player.SendMessage("Script Error: EquipItem: Item serial: (" + itemserial + ") too away");
                return;
            }
            Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount)); // Prende
            Assistant.ClientCommunication.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player.Serial, item.Layer)); // Equippa
        }
        public static void EquipItem(Item item)
        {
            Assistant.Mobile aa = Assistant.World.Player;
            if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), Assistant.World.Player.Position) > 3)
            {
                Player.SendMessage("Script Error: EquipItem: Item serial: (" + item.Serial + ") too away");
                return;
            }
            Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount)); // Prende
            Assistant.ClientCommunication.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player.Serial, item.Layer)); // Equippa
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
                    Player.SendMessage("Script Error: CheckLayer: Invalid layer name: " + Layer);
                    return false;
            }
        }
        // Skill
        public static double GetSkillValue(string skillname)
        {
            switch (skillname)
            {
                case "Alchemy":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Alchemy)].Value;
                case "AnimalLore":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.AnimalLore)].Value;
                case "ItemID":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.ItemID)].Value;
                case "ArmsLore":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.ArmsLore)].Value;
                case "Parry":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Parry)].Value;
                case "Begging":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Begging)].Value;
                case "Blacksmith":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Blacksmith)].Value;
                case "Fletching":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fletching)].Value;
                case "Peacemaking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Peacemaking)].Value;
                case "Camping":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Camping)].Value;
                case "Carpentry":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Carpentry)].Value;
                case "Cartography":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Cartography)].Value;
                case "Cooking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Cooking)].Value;
                case "DetectHidden":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.DetectHidden)].Value;
                case "Discordance":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Discordance)].Value;
                case "EvalInt":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.EvalInt)].Value;
                case "Healing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Healing)].Value;
                case "Fishing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fishing)].Value;
                case "Forensics":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Forensics)].Value;
                case "Herding":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Herding)].Value;
                case "Hiding":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Hiding)].Value;
                case "Provocation":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Provocation)].Value;
                case "Inscribe":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Inscribe)].Value;
                case "Lockpicking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Lockpicking)].Value;
                case "Magery":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Magery)].Value;
                case "MagicResist":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.MagicResist)].Value;
                case "Tactics":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tactics)].Value;
                case "Snooping":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Snooping)].Value;
                case "Musicianship":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Musicianship)].Value;
                case "Poisoning":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Poisoning)].Value;
                case "Archery":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Archery)].Value;
                case "SpiritSpeak":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.SpiritSpeak)].Value;
                case "Stealing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Stealing)].Value;
                case "Tailoring":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tailoring)].Value;
                case "AnimalTaming":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.AnimalTaming)].Value;
                case "TasteID":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.TasteID)].Value;
                case "Tinkering":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tinkering)].Value;
                case "Tracking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tracking)].Value;
                case "Veterinary":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Veterinary)].Value;
                case "Swords":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Swords)].Value;
                case "Macing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Macing)].Value;
                case "Fencing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fencing)].Value;
                case "Wrestling":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Wrestling)].Value;
                case "Lumberjacking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Lumberjacking)].Value;
                case "Mining":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Mining)].Value;
                case "Meditation":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Meditation)].Value;
                case "Stealth":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Stealth)].Value;
                case "RemoveTrap":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.RemoveTrap)].Value;
                case "Necromancy":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Necromancy)].Value;
                case "Focus":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Focus)].Value;
                case "Chivalry":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Chivalry)].Value;
                case "Bushido":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Bushido)].Value;
                case "Ninjitsu":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Ninjitsu)].Value;
                case "SpellWeaving":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.SpellWeaving)].Value;
                default:
                    Player.SendMessage("Script Error: GetSkillValue: Invalid skill name: " + skillname);
                    return 0;
            }
        }
        public static double GetSkillCap(string skillname)
        {
            switch (skillname)
            {
                case "Alchemy":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Alchemy)].Cap;
                case "AnimalLore":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.AnimalLore)].Cap;
                case "ItemID":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.ItemID)].Cap;
                case "ArmsLore":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.ArmsLore)].Cap;
                case "Parry":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Parry)].Cap;
                case "Begging":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Begging)].Cap;
                case "Blacksmith":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Blacksmith)].Cap;
                case "Fletching":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fletching)].Cap;
                case "Peacemaking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Peacemaking)].Cap;
                case "Camping":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Camping)].Cap;
                case "Carpentry":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Carpentry)].Cap;
                case "Cartography":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Cartography)].Cap;
                case "Cooking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Cooking)].Cap;
                case "DetectHidden":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.DetectHidden)].Cap;
                case "Discordance":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Discordance)].Cap;
                case "EvalInt":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.EvalInt)].Cap;
                case "Healing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Healing)].Cap;
                case "Fishing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fishing)].Cap;
                case "Forensics":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Forensics)].Cap;
                case "Herding":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Herding)].Cap;
                case "Hiding":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Hiding)].Cap;
                case "Provocation":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Provocation)].Cap;
                case "Inscribe":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Inscribe)].Cap;
                case "Lockpicking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Lockpicking)].Cap;
                case "Magery":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Magery)].Cap;
                case "MagicResist":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.MagicResist)].Cap;
                case "Tactics":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tactics)].Cap;
                case "Snooping":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Snooping)].Cap;
                case "Musicianship":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Musicianship)].Cap;
                case "Poisoning":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Poisoning)].Cap;
                case "Archery":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Archery)].Cap;
                case "SpiritSpeak":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.SpiritSpeak)].Cap;
                case "Stealing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Stealing)].Cap;
                case "Tailoring":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tailoring)].Cap;
                case "AnimalTaming":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.AnimalTaming)].Cap;
                case "TasteID":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.TasteID)].Cap;
                case "Tinkering":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tinkering)].Cap;
                case "Tracking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tracking)].Cap;
                case "Veterinary":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Veterinary)].Cap;
                case "Swords":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Swords)].Cap;
                case "Macing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Macing)].Cap;
                case "Fencing":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fencing)].Cap;
                case "Wrestling":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Wrestling)].Cap;
                case "Lumberjacking":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Lumberjacking)].Cap;
                case "Mining":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Mining)].Cap;
                case "Meditation":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Meditation)].Cap;
                case "Stealth":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Stealth)].Cap;
                case "RemoveTrap":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.RemoveTrap)].Cap;
                case "Necromancy":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Necromancy)].Cap;
                case "Focus":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Focus)].Cap;
                case "Chivalry":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Chivalry)].Cap;
                case "Bushido":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Bushido)].Cap;
                case "Ninjitsu":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Ninjitsu)].Cap;
                case "SpellWeaving":
                    return Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.SpellWeaving)].Cap;
                default:
                    Player.SendMessage("Script Error: GetSkillCap: Invalid skill name: " + skillname);
                    return 0;
            }
        }
        public static int GetSkillStatus(string skillname)
        {
            switch (skillname)
            {
                case "Alchemy":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Alchemy)].Lock);
                case "AnimalLore":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.AnimalLore)].Lock);
                case "ItemID":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.ItemID)].Lock);
                case "ArmsLore":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.ArmsLore)].Lock);
                case "Parry":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Parry)].Lock);
                case "Begging":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Begging)].Lock);
                case "Blacksmith":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Blacksmith)].Lock);
                case "Fletching":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fletching)].Lock);
                case "Peacemaking":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Peacemaking)].Lock);
                case "Camping":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Camping)].Lock);
                case "Carpentry":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Carpentry)].Lock);
                case "Cartography":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Cartography)].Lock);
                case "Cooking":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Cooking)].Lock);
                case "DetectHidden":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.DetectHidden)].Lock);
                case "Discordance":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Discordance)].Lock);
                case "EvalInt":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.EvalInt)].Lock);
                case "Healing":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Healing)].Lock);
                case "Fishing":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fishing)].Lock);
                case "Forensics":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Forensics)].Lock);
                case "Herding":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Herding)].Lock);
                case "Hiding":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Hiding)].Lock);
                case "Provocation":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Provocation)].Lock);
                case "Inscribe":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Inscribe)].Lock);
                case "Lockpicking":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Lockpicking)].Lock);
                case "Magery":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Magery)].Lock);
                case "MagicResist":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.MagicResist)].Lock);
                case "Tactics":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tactics)].Lock);
                case "Snooping":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Snooping)].Lock);
                case "Musicianship":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Musicianship)].Lock);
                case "Poisoning":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Poisoning)].Lock);
                case "Archery":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Archery)].Lock);
                case "SpiritSpeak":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.SpiritSpeak)].Lock);
                case "Stealing":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Stealing)].Lock);
                case "Tailoring":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tailoring)].Lock);
                case "AnimalTaming":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.AnimalTaming)].Lock);
                case "TasteID":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.TasteID)].Lock);
                case "Tinkering":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tinkering)].Lock);
                case "Tracking":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Tracking)].Lock);
                case "Veterinary":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Veterinary)].Lock);
                case "Swords":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Swords)].Lock);
                case "Macing":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Macing)].Lock);
                case "Fencing":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Fencing)].Lock);
                case "Wrestling":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Wrestling)].Lock);
                case "Lumberjacking":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Lumberjacking)].Lock);
                case "Mining":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Mining)].Lock);
                case "Meditation":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Meditation)].Lock);
                case "Stealth":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Stealth)].Lock);
                case "RemoveTrap":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.RemoveTrap)].Lock);
                case "Necromancy":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Necromancy)].Lock);
                case "Focus":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Focus)].Lock);
                case "Chivalry":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Chivalry)].Lock);
                case "Bushido":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Bushido)].Lock);
                case "Ninjitsu":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.Ninjitsu)].Lock);
                case "SpellWeaving":
                    return Convert.ToInt16(Assistant.World.Player.Skills[Convert.ToInt16(Assistant.SkillName.SpellWeaving)].Lock);
                default:
                    Player.SendMessage("Script Error: GetSkillStatus: Invalid skill name: " + skillname);
                    return 0;
            }
        }
        public static void UseSkill(string skillname)
        {
            switch (skillname)
            {
                case "AnimalLore":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.AnimalLore)));
                    break;
                case "ItemID":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.ItemID)));
                    break;
                case "ArmsLore":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.ArmsLore)));
                    break;
                case "Begging":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Begging)));
                    break;
                case "Peacemaking":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Peacemaking)));
                    break;
                case "Cartography":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Cartography)));
                    break;
                case "DetectHidden":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.DetectHidden)));
                    break;
                case "Discordance":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Discordance)));
                    break;
                case "EvalInt":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.EvalInt)));
                    break;
                case "Forensics":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Forensics)));
                    break;
                case "Hiding":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Hiding)));
                    break;
                case "Provocation":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Provocation)));
                    break;
                case "Poisoning":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Poisoning)));
                    break;
                case "SpiritSpeak":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.SpiritSpeak)));
                    break;
                case "Stealing":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Stealing)));
                    break;
                case "AnimalTaming":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.AnimalTaming)));
                    break;
                case "TasteID":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.TasteID)));
                    break;
                case "Tracking":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Tracking)));
                    break;
                case "Meditation":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Meditation)));
                    break;
                case "Stealth":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Stealth)));
                    break;
                case "RemoveTrap":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.RemoveTrap)));
                    break;
                case "Inscribe":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Inscribe)));
                    break;
                case "Anatomy":
                    Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Anatomy)));
                    break;
                default:
                    Player.SendMessage("Script Error: UseSkill: Invalid skill name: " + skillname);
                    break;
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

        // Game Message
		public static void ChatSay(int hue, int font, string msg)
		{
            Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Regular, hue, font, msg));
		}
        public static void ChatEmote(int hue, int font, string msg)
        {
            Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Emote, hue, font, msg));
        }
        public static void ChatWhisper(int hue, int font, string msg)
        {
            Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Whisper, hue, font, msg));
        }
        public static void ChatYell(int hue, int font, string msg)
        {
            Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Yell, hue, font, msg));
        }

        // spell
        public static void CastSpellMagery(string SpellName)
        {
            switch (SpellName)
            {
                // Primo circolo magery
                case "Clumsy":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(1));
                    break;
                case "CreateFood":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(2));
                    break;
                case "Feeblemind":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(3));
                    break;
                case "Heal":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(4));
                    break;
                case "MagicArrow":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(5));
                    break;
                case "NightSight":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(6));
                    break;
                case "ReactiveArmor":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(7));
                    break;
                case "Weaken":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(8));
                    break;
                // Secondo circolo magery
                case "Agility":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(9));
                    break;
                case "Cunning":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(10));
                    break;
                case "Cure":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(11));
                    break;
                case "Harm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(12));
                    break;
                case "MagicTrap":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(13));
                    break;
                case "MagicUntrap":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(14));
                    break;
                case "Protection":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(15));
                    break;
                case "Strength":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(16));
                    break;
                // Terzo circolo magery
                case "Bless":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(17));
                    break;
                case "Fireball":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(18));
                    break;
                case "MagicLock":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(19));
                    break;
                case "Poison":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(20));
                    break;
                case "Telekinesis":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(21));
                    break;
                case "Teleport":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(22));
                    break;
                case "Unlock":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(23));
                    break;
                case "WallofStone":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(24));
                    break;
                // Quarto circolo magery
                case "ArchCure":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(25));
                    break;
                case "ArchProtection":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(26));
                    break;
                case "Curse":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(27));
                    break;
                case "FireField":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(28));
                    break;
                case "GreaterHeal":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(29));
                    break;
                case "Lightning":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(30));
                    break;
                case "ManaDrain":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(31));
                    break;
                case "Recall":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(32));
                    break;
                // Quinto circolo magery
                case "BladeSpirits":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(33));
                    break;
                case "DispelField":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(34));
                    break;
                case "Incognito":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(35));
                    break;
                case "MagicReflection":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(36));
                    break;
                case "MindBlast":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(37));
                    break;
                case "Paralyze":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(38));
                    break;
                case "PoisonField":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(39));
                    break;
                case "SummonCreature":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(40));
                    break;
                // Sesto circolo magery
                case "Dispel":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(41));
                    break;
                case "EnergyBolt":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(42));
                    break;
                case "Explosion":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(43));
                    break;
                case "Invisibility":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(44));
                    break;
                case "Mark":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(45));
                    break;
                case "MassCurse":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(46));
                    break;
                case "ParalyzeField":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(47));
                    break;
                case "Reveal":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(48));
                    break;
                // Settimo circolo magery
                case "ChainLightning":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(49));
                    break;
                case "EnergyField":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(50));
                    break;
                case "Flamestrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(51));
                    break;
                case "GateTravel":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(52));
                    break;
                case "ManaVampire":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(53));
                    break;
                case "MassDispel":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(54));
                    break;
                case "MeteorSwarm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(55));
                    break;
                case "Polymorph":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(56));
                    break;
                // Ottavo circolo magery
                case "Earthquake":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(57));
                    break;
                case "EnergyVortex":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(58));
                    break;
                case "Resurrection":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(59));
                    break;
                case "SummonAirElemental":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(60));
                    break;
                case "SummonDaemon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(61));
                    break;
                case "SummonEarthElemental":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(62));
                    break;
                case "SummonFireElemental":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(63));
                    break;
                case "SummonWaterElemental":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(64));
                    break;     
                default:
                    Player.SendMessage("Script Error: CastSpellMagery: Invalid spell name: " + SpellName);
                    break;
            } 
        }
        public static void CastSpellNecro(string SpellName)                    
        {
            switch (SpellName)
            {
                case "CurseWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(101));
                    break;
                case "PainSpike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(102));
                    break;
                case "CorpseSkin":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(103));
                    break;
                case "EvilOmen":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(104));
                    break;
                case "BloodOath":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(105));
                    break;
                case "WraithForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(106));
                    break;
                case "MindRot":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(107));
                    break;
                case "SummonFamiliar":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(108));
                    break;
                case "HorrificBeast":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(109));
                    break;
                case "AnimateDead":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(110));
                    break;
                case "PoisonStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(111));
                    break;
                case "Wither":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(112));
                    break;
                case "Strangle":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(113));
                    break;
                case "LichForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(114));
                    break;
                case "Exorcism":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(115));
                    break;
                case "VengefulSpirit":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(116));
                    break;
                case "VampiricEmbrace":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(117));
                    break;
                default:
                    Player.SendMessage("Script Error: CastSpellNecro: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellChivalry(string SpellName)
        {
            switch (SpellName)
            {
                case "CloseWounds":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(201));
                    break;
                case "RemoveCurse":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(202));
                    break;
                case "CleanseByFire":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(203));
                    break;
                case "ConsecrateWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(204));
                    break;
                case "SacredJourney":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(205));
                    break;
                case "DivineFury":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(206));
                    break;
                case "DispelEvil":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(207));
                    break;
                case "EnemyOfOne":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(208));
                    break;
                case "HolyLight":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(209));
                    break;
                case "NobleSacrifice":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(210));
                    break;        
                default:
                    Player.SendMessage("Script Error: CastSpellChivalry: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellBushido(string SpellName)
        {
            switch (SpellName)
            {
                case "HonorableExecution":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(145));
                    break;
                case "Confidence":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(146));
                    break;
                case "CounterAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(147));
                    break;
                case "LightningStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(148));
                    break;
                case "Evasion":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(149));
                    break;
                case "MomentumStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(150));
                    break;
                default:
                    Player.SendMessage("Script Error: CastSpellBushido: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellNinjitsu(string SpellName)
        {
            switch (SpellName)
            {
                case "AnimalForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(245));
                    break;
                case "Backstab":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(246));
                    break;
                case "SurpriseAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(247));
                    break;
                case "MirrorImage":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(248));
                    break;
                case "Shadowjump":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(249));
                    break;
                case "FocusAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(250));
                    break;
                case "KiAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(251));
                    break;
                case "DeathStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(252));
                    break;
                default:
                    Player.SendMessage("Script Error: CastSpellNinjitsu: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellSpellweaving(string SpellName)     // Sicuramente problemi sul send pacchetto in quanto gli id sono uguali ad altre spe
        {
            switch (SpellName)
            {
                case "ArcaneCircle":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(89));
                    break;
                case "Attunement":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(90));
                    break;
                case "GiftOfRenewal":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(91));
                    break;
                case "NaturesFury":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(92));
                    break;
                case "ImmolatingWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(93));
                    break;
                case "Thunderstorm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(94));
                    break;
                case "ArcaneEmpowerment":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(95));
                    break;
                case "EtherealVoyage":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(96));
                    break;
                case "ReaperForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(97));
                    break;
                case "GiftOfLife":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(98));
                    break;
                case "SummonFey":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(99));
                    break;
                case "SummonFiend":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(100));
                    break;
                case "DryadAllure":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(101));
                    break;
                case "EssenceOfWind":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(102));
                    break;
                case "Wildfire":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(103));
                    break;
                case "WordOfDeath":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(104));
                    break;
                default:
                    Player.SendMessage("Script Error: CastSpellSpellweaving: Invalid spell name: " + SpellName);
                    break;
            }
        }  

        // attack
        public static void SetWarMode(bool warflag)
        {
            Assistant.ClientCommunication.SendToServer(new SetWarMode(warflag));
        }

        public static void Attack(uint serial)
        {
            Assistant.ClientCommunication.SendToServer(new AttackReq(serial));
        }

		public static void Walk(string direction)
		{
			Direction dir;
			switch (direction)
			{
				case "North":
					dir = Direction.North;
					break;
				case "South":
					dir = Direction.South;
					break;
				case "East":
					dir = Direction.East;
					break;
				case "West":
					dir = Direction.West;
					break;
				case "Up":
					dir = Direction.Up;
					break;
				case "Down":
					dir = Direction.Down;
					break;
				case "Left":
					dir = Direction.Left;
					break;
				case "Right":
					dir = Direction.Right;
					break;
				default:
					dir = Direction.Mask;
					break;
			}

			if (dir != Direction.Mask)
			{
				ClientCommunication.SendToServer(new WalkRequest(dir, Assistant.World.Player.WalkSequence));
			}
		}

		public static void Pause(double seconds)
		{
			System.Threading.Thread.Sleep((int)(1000 * seconds));
		}
    
	}
}
