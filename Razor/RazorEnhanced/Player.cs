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
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.RightHand), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "LeftHand":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.LeftHand), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Shoes":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shoes), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Pants":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Pants), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Shirt":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Shirt), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Head":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Head), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Gloves":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Gloves), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Ring":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Ring), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Neck":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Neck), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Waist":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Waist), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "InnerTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Bracelet":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Bracelet), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "MiddleTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.MiddleTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Earrings":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Earrings), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Arms":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Arms), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "Cloak":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Cloak), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "OuterTorso":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterTorso), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "OuterLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.OuterLegs), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
					else
                        Player.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + Layer);
					break;
				case "InnerLegs":
                    if (Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs) != null)
						Assistant.DragDropManager.DragDrop(Assistant.World.Player.GetItemOnLayer(Assistant.Layer.InnerLegs), Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Backpack));
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
            Assistant.ClientCommunication.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player.Serial, Assistant.Layer.Ring));
        }
        public static void EquipItem(Item item)
        {
            if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), Assistant.World.Player.Position) > 3)
            {
                Player.SendMessage("Script Error: EquipItem: Item serial: (" + item.Serial + ") too away");
                return;
            }
            Assistant.ClientCommunication.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player.Serial, Assistant.Layer.Ring));
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
	}
}
