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

		public Item Backpack
		{
			get
			{
				Assistant.Item assistantBackpack = Assistant.World.Player.Backpack;
				if (assistantBackpack == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedBackpack = new RazorEnhanced.Item(assistantBackpack);
					return enhancedBackpack;
				}
			}
		}

		public Item Bank
		{
			get
			{
				Assistant.Item assistantBank = Assistant.World.Player.GetItemOnLayer(Layer.Bank);
				if (assistantBank == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedBackpack = new RazorEnhanced.Item(assistantBank);
					return enhancedBackpack;
				}
			}
		}

		public Item Quiver
		{
			get
			{
				Assistant.Item assistantQuiver = Assistant.World.Player.Quiver;
				if (assistantQuiver == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedQuiver = new RazorEnhanced.Item(assistantQuiver);
					return enhancedQuiver;
				}
			}
		}

		public Item Mount
		{
			get
			{
				Assistant.Item assistantMount = Assistant.World.Player.GetItemOnLayer(Assistant.Layer.Mount);
				if (assistantMount == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedMount = new RazorEnhanced.Item(assistantMount);
					return enhancedMount;
				}
			}
		}

		public static int Gold { get { return Convert.ToInt32(Assistant.World.Player.Gold); } }
		public static int Luck { get { return Assistant.World.Player.Luck; } }
		public static int Body { get { return Assistant.World.Player.Body; } }
		public static int Serial { get { return Assistant.World.Player.Serial; } }
		// Follower
		public static int FollowersMax { get { return Assistant.World.Player.FollowersMax; } }
		public static int Followers { get { return Assistant.World.Player.Followers; } }
		// Weight
		public static int MaxWeight { get { return Assistant.World.Player.MaxWeight; } }
		public static int Weight { get { return Assistant.World.Player.Weight; } }
		// Position
		public static Point3D Position { get { return new Point3D(Assistant.World.Player.Position); } }
		// Layer

		internal static Assistant.Layer GetAssistantLayer(string layer)
		{
			Assistant.Layer result = Layer.Invalid;

			switch (layer)
			{
				case "RightHand":
					result = Assistant.Layer.RightHand;
					break;
				case "LeftHand":
					result = Assistant.Layer.LeftHand;
					break;
				case "Shoes":
					result = Assistant.Layer.Shoes;
					break;
				case "Pants":
					result = Assistant.Layer.Pants;
					break;
				case "Shirt":
					result = Assistant.Layer.Shirt;
					break;
				case "Head":
					result = Assistant.Layer.Head;
					break;
				case "Gloves":
					result = Assistant.Layer.Gloves;
					break;
				case "Ring":
					result = Assistant.Layer.Ring;
					break;
				case "Neck":
					result = Assistant.Layer.Neck;
					break;
				case "Hair":
					result = Assistant.Layer.Hair;
					break;
				case "Waist":
					result = Assistant.Layer.Waist;
					break;
				case "InnerTorso":
					result = Assistant.Layer.InnerTorso;
					break;
				case "Bracelet":
					result = Assistant.Layer.Bracelet;
					break;
				case "FacialHair":
					result = Assistant.Layer.FacialHair;
					break;
				case "MiddleTorso":
					result = Assistant.Layer.MiddleTorso;
					break;
				case "Earrings":
					result = Assistant.Layer.Earrings;
					break;
				case "Arms":
					result = Assistant.Layer.Arms;
					break;
				case "Cloak":
					result = Assistant.Layer.Cloak;
					break;
				case "OuterTorso":
					result = Assistant.Layer.OuterTorso;
					break;
				case "OuterLegs":
					result = Assistant.Layer.OuterLegs;
					break;
				case "InnerLegs":
					result = Assistant.Layer.InnerLegs;
					break;
				default:
					result = Assistant.Layer.Invalid;
					break;
			}

			return result;
		}

		public static void UnEquipItemByLayer(String layer)
		{

			Assistant.Layer assistantLayer = GetAssistantLayer(layer);

			Assistant.Item item = Assistant.World.Player.GetItemOnLayer(assistantLayer);

			if (item != null)
			{
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, World.Player.Backpack.Serial));
			}
			else
                Misc.SendMessage("Script Error: UnEquipItemByLayer: No item found on layer: " + layer);
		}


		public static void EquipItem(int serial)
		{
			Assistant.Item item = Assistant.World.FindItem((Assistant.Serial)serial);
			if (item == null)
			{
                Misc.SendMessage("Script Error: EquipItem: Item serial: (" + serial + ") not found");
				return;
			}

			if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), Assistant.World.Player.Position) > 3)
			{
                Misc.SendMessage("Script Error: EquipItem: Item serial: (" + serial + ") too away");
				return;
			}
			Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount)); // Prende
			Assistant.ClientCommunication.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player.Serial, item.Layer)); // Equippa
		}

		private static double GetDistance(Point3D a, Point3D b)
		{
			double distance = Math.Sqrt(((a.X - b.X) ^ 2) + (a.Y - b.Y) ^ 2);
			return distance;
		}

		public static void EquipItem(Item item)
		{
			Assistant.Mobile player = Assistant.World.Player;
			if (item.Container == null && GetDistance(item.GetWorldPosition(), Position) > 3)
			{
                Misc.SendMessage("Script Error: EquipItem: Item serial: (" + item.Serial + ") too away");
				return;
			}
			Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount)); // Prende
			Assistant.ClientCommunication.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player.Serial, item.AssistantLayer)); // Equippa
		}

		public static bool CheckLayer(String layer)
		{
			Assistant.Layer assistantLayer = GetAssistantLayer(layer);

			if (assistantLayer == Layer.Invalid)
				return false;
			else
			{
				Assistant.Item item = Assistant.World.Player.GetItemOnLayer(assistantLayer);
				if (item != null)
					return true;
				else
				{
                    Misc.SendMessage("Script Error: CheckLayer: Invalid layer name: " + layer);
					return false;
				}
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
                    Misc.SendMessage("Script Error: GetSkillValue: Invalid skill name: " + skillname);
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
                    Misc.SendMessage("Script Error: GetSkillCap: Invalid skill name: " + skillname);
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
                    Misc.SendMessage("Script Error: GetSkillStatus: Invalid skill name: " + skillname);
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
                    Misc.SendMessage("Script Error: UseSkill: Invalid skill name: " + skillname);
					break;
			}
		}

		// Game Message
		public static void ChatSay(int hue, int font, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Regular, hue, font, msg));
		}
		public static void ChatGuild(int hue, int font, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Guild, hue, font, msg));
		}
		public static void ChatAlliance(int hue, int font, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Alliance, hue, font, msg));
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
                    Misc.SendMessage("Script Error: CastSpellMagery: Invalid spell name: " + SpellName);
					break;
			}
		}

        public static void CastSpellNecro(string SpellName)
        {
            switch (SpellName)
            {
                case "AnimateDead":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(101));
                    break;
                case "BloodOath":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(102));
                    break;
                case "CorpseSkin":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(103));
                    break;
                case "CurseWeapon ":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(104));
                    break;
                case "EvilOmen":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(105));
                    break;
                case "HorrificBeast":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(106));
                    break;
                case "LichForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(107));
                    break;
                case "MindRot":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(108));
                    break;
                case "PainSpike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(109));
                    break;
                case "PoisonStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(110));
                    break;
                case "Strangle":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(111));
                    break;
                case "SummonFamiliar":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(112));
                    break;
                case "VampiricEmbrace":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(113));
                    break;
                case "VengefulSpirit":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(114));
                    break;
                case "Wither":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(115));
                    break;
                case "WraithForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(116));
                    break;
                case "Exorcism":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(117));
                    break;
                default:
                    Misc.SendMessage("Script Error: CastSpellNecro: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellChivalry(string SpellName)
        {
            switch (SpellName)
            {
                case "CleanseByFire":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(201));
                    break;
                case "CloseWounds":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(202));
                    break;
                case "ConsecrateWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(203));
                    break;
                case "DispelEvil":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(204));
                    break;
                case "DivineFury":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(205));
                    break;
                case "EnemyOfOne":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(206));
                    break;
                case "HolyLight":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(207));
                    break;
                case "NobleSacrifice":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(208));
                    break;
                case "RemoveCurse":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(209));
                    break;
                case "SacredJourney":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(210));
                    break;
                default:
                    Misc.SendMessage("Script Error: CastSpellChivalry: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellBushido(string SpellName)
        {
            switch (SpellName)
            {
                case "HonorableExecution":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(401));
                    break;
                case "Confidence":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(402));
                    break;
                case "Evasion":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(403));
                    break;
                case "CounterAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(404));
                    break;
                case "LightningStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(405));
                    break;
                case "MomentumStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(406));
                    break;
                default:
                    Misc.SendMessage("Script Error: CastSpellBushido: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellNinjitsu(string SpellName)
        {
            switch (SpellName)
            {
                case "FocusAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(501));
                    break;
                case "DeathStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(502));
                    break;
                case "AnimalForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(503));
                    break;
                case "KiAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(504));
                    break;
                case "SurpriseAttack":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(505));
                    break;
                case "Backstab":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(506));
                    break;
                case "Shadowjump":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(507));
                    break;
                case "MirrorImage":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(508));
                    break;
                default:
                    Misc.SendMessage("Script Error: CastSpellNinjitsu: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellSpellweaving(string SpellName)
        {
            switch (SpellName)
            {
                case "ArcaneCircle":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(601));
                    break;
                case "GiftOfRenewal":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(602));
                    break;
                case "ImmolatingWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(603));
                    break;
                case "AttuneWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(604));
                    break;
                case "Thunderstorm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(605));
                    break;
                case "NaturesFury":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(606));
                    break;
                case "SummonFey":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(607));
                    break;
                case "Summoniend":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(608));
                    break;
                case "ReaperForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(609));
                    break;
                case "Wildfire":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(610));
                    break;
                case "EssenceOfWind":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(611));
                    break;
                case "DryadAllure":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(612));
                    break;
                case "EtherealVoyage":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(613));
                    break;
                case "WordOfDeath":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(614));
                    break;
                case "GiftOfLife":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(615));
                    break;
                case "ArcaneEmpowerment":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(616));
                    break;
                default:
                    Misc.SendMessage("Script Error: CastSpellSpellweaving: Invalid spell name: " + SpellName);
                    break;
            }
        }
        public static void CastSpellMysticism(string SpellName)
        {
            switch (SpellName)
            {
                case "AnimatedWeapon":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(678));
                    break;
                case "HealingStone":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(679));
                    break;
                case "Purge":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(680));
                    break;
                case "Enchant":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(681));
                    break;
                case "Sleep":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(682));
                    break;
                case "EagleStrike":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(683));
                    break;
                case "StoneForm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(684));
                    break;
                case "SpellTrigger":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(685));
                    break;
                case "MassSleep":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(686));
                    break;
                case "CleansingWinds":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(687));
                    break;
                case "Bombard":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(688));
                    break;
                case "SpellPlague":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(689));
                    break;
                case "HailStorm":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(690));
                    break;
                case "NetherCyclone":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(691));
                    break;
                case "RisingColossus":
                    Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(692));
                    break;
                default:
                    Misc.SendMessage("Script Error: CastSpellMysticism: Invalid spell name: " + SpellName);
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
		// Virtue
		public static void InvokeVirtue(string virtue)
		{
			switch (virtue)
			{
				case "Honor":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(1));
					break;
				case "Sacrifice":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(2));
					break;
				case "Valor":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(3));
					break;
				case "Compassion":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(4));
					break;
				case "Honesty":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(5));
					break;
				case "Humility":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(6));
					break;
				case "Justice":
					Assistant.ClientCommunication.SendToServer(new InvokeVirtue(7));
					break;
				default:
                    Misc.SendMessage("Script Error - InvokeVirtue: Invalid virtue name: " + virtue);
					break;
			}
		}
		// party
		public static bool InParty { get { return Assistant.World.Player.InParty; } }
		public static void ChatParty(string msg)
		{
			if (InParty)
				Assistant.ClientCommunication.SendToServer(new SendPartyMessage(Assistant.World.Player.Serial, msg));
			else
                Misc.SendMessage("Script Error: ChatParty: you are not in a party");
		}
        public static void PartyInvite()
        {
            Assistant.ClientCommunication.SendToServer(new PartyInvite());
        }
        public static void LeaveParty()
        {
            Assistant.ClientCommunication.SendToServer(new PartyRemoveMember(World.Player.Serial));
        }
        public static void KickMember(int serial)
        {
            uint userial = Convert.ToUInt16(serial);
            Assistant.ClientCommunication.SendToServer(new PartyRemoveMember(userial));
        }
		public static void PartyCanLoot(bool CanLoot)
		{
			if (InParty)
				if (CanLoot)
					Assistant.ClientCommunication.SendToServer(new PartyCanLoot(0x1));
				else
					Assistant.ClientCommunication.SendToServer(new PartyCanLoot(0x0));
			else
                Misc.SendMessage("Script Error: ChatParty: you are not in a party");
		}

		// Moving
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

		internal static void PathFindTo(Assistant.Point3D Location)
		{
			ClientCommunication.SendToClient(new PathFindTo(Location));
		}

		public static void PathFindTo(int x, int y, int z)
		{
			Assistant.Point3D Location = new Assistant.Point3D(Assistant.Point3D.Zero);
			Location.X = x;
			Location.Y = y;
			Location.Z = z;
			ClientCommunication.SendToClient(new PathFindTo(Location));
		}


        //Props
        public static int GetPropByCliloc(uint serial, int code)
        {
            Assistant.Mobile assistantMobile = Assistant.World.FindMobile((Assistant.Serial)((uint)serial));
            if (assistantMobile == null)
            {
                Misc.SendMessage("Script Error: GetPropByCliloc: Mobile serial: (" + serial + ") not found");
                return 0;
            }
            else
            {
                RazorEnhanced.Mobile mobile = new RazorEnhanced.Mobile(assistantMobile);
                return GetPropExec(mobile, code, "GetPropByCliloc");
            }
        }

        public static int GetPropByCliloc(RazorEnhanced.Mobile assistantMobile, int code)
        {

            if (assistantMobile == null)
            {
                Misc.SendMessage("Script Error: GetPropByCliloc: mobile not found");
                return 0;
            }
            else
            {
                return GetPropExec(assistantMobile, code, "GetPropByCliloc");
            }
        }

        public static int GetPropByString(uint serial, string props)
        {
            Assistant.Mobile assistantMobile = Assistant.World.FindMobile((Assistant.Serial)((uint)serial));
            if (assistantMobile == null)
            {
                Misc.SendMessage("Script Error: GetPropByString: mobile serial: (" + serial + ") not found");
                return 0;
            }

            RazorEnhanced.Mobile mobile = new RazorEnhanced.Mobile(assistantMobile);

            switch (props)
            {
                case "Damage Increase":
                    {
                        if (GetPropExec(mobile, 1060401, "GetPropByString") != 0)
                            return GetPropExec(mobile, 1060401, "GetPropByString");
                        return GetPropExec(mobile, 1060402, "GetPropByString");
                    }
                case "Defense Chance Increase":
                    return GetPropExec(mobile, 1060408, "GetPropByString");
                case "Faster Cast Recovery":
                    return GetPropExec(mobile, 1060412, "GetPropByString");
                case "Enhance Potion":
                    return GetPropExec(mobile, 1060411, "GetPropByString");
                case "Faster Casting":
                    return GetPropExec(mobile, 1060413, "GetPropByString");
                case "Hit Chance Increase":
                    return GetPropExec(mobile, 1060415, "GetPropByString");
                case "Lower Mana Cost":
                    return GetPropExec(mobile, 1060433, "GetPropByString");
                case "Lower Reagent Cost":
                    return GetPropExec(mobile, 1060434, "GetPropByString");
                case "Mana Regeneration":
                    return GetPropExec(mobile, 1060440, "GetPropByString");
                case "Spell Damage Increase":
                    return GetPropExec(mobile, 1060483, "GetPropByString");
                case "Stamina Increase":
                    return GetPropExec(mobile, 1060484, "GetPropByString");
                case "Stamina Regeneration":
                    return GetPropExec(mobile, 1060443, "GetPropByString");
                case "Swing Speed Increase":
                    return GetPropExec(mobile, 1060486, "GetPropByString");
                case "Hit Point Increase":
                    return GetPropExec(mobile, 1060431, "GetPropByString");
                case "Hit Point Regeneration":
                    return GetPropExec(mobile, 1060444, "GetPropByString");

                default:
                    Misc.SendMessage("Script Error: GetPropByString: Invalid or not supported props string");
                    return 0;

            }
        }

        public static int GetPropByString(RazorEnhanced.Mobile mobile, string props)
        {
                switch (props)
                {
                    case "Damage Increase":
                        {
                            if (GetPropExec(mobile, 1060401, "GetPropByString") != 0)
                                return GetPropExec(mobile, 1060401, "GetPropByString");
                            return GetPropExec(mobile, 1060402, "GetPropByString");
                        }
                    case "Defense Chance Increase":
                        return GetPropExec(mobile, 1060408, "GetPropByString");
                    case "Faster Cast Recovery":
                        return GetPropExec(mobile, 1060412, "GetPropByString");
                    case "Enhance Potion":
                        return GetPropExec(mobile, 1060411, "GetPropByString");
                    case "Faster Casting":
                        return GetPropExec(mobile, 1060413, "GetPropByString");
                    case "Hit Chance Increase":
                        return GetPropExec(mobile, 1060415, "GetPropByString");
                    case "Lower Mana Cost":
                        return GetPropExec(mobile, 1060433, "GetPropByString");
                    case "Lower Reagent Cost":
                        return GetPropExec(mobile, 1060434, "GetPropByString");
                    case "Mana Regeneration":
                        return GetPropExec(mobile, 1060440, "GetPropByString");
                    case "Spell Damage Increase":
                        return GetPropExec(mobile, 1060483, "GetPropByString");
                    case "Stamina Increase":
                        return GetPropExec(mobile, 1060484, "GetPropByString");
                    case "Stamina Regeneration":
                        return GetPropExec(mobile, 1060443, "GetPropByString");
                    case "Swing Speed Increase":
                        return GetPropExec(mobile, 1060486, "GetPropByString");
                    case "Hit Point Increase":
                        return GetPropExec(mobile, 1060431, "GetPropByString");
                    case "Hit Point Regeneration":
                        return GetPropExec(mobile, 1060444, "GetPropByString");

                    default:
                        Misc.SendMessage("Script Error: GetPropByString: Invalid or not supported props string");
                        return 0;
                
            }
        }

        private static int GetPropExec(RazorEnhanced.Mobile mobile, int code, String Fcall)
        {
            List<Property> properties = mobile.Properties;
            foreach (Property property in properties)
            {
                int number = property.Number;
                string args = property.Args;
                if (number == code)
                {
                    if (args == null)  // Esiste prop ma senza valore
                        return 1;
                    else
                    {
                        try
                        {
                            return Convert.ToInt32(args);  // Ritorna valore
                        }
                        catch
                        {
                            Misc.SendMessage("Script Error: " + Fcall + ": Error to get value of Cliloc:" + code);
                            return 0;  // errore di conversione
                        }
                    }
                }
            }
            return 0;       // Prop inesistente sul item
        }
	}
}
