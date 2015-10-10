using Assistant;
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
		public static byte Notoriety { get { return Assistant.World.Player.Notoriety; } }

		public static Item Backpack
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

		public static Item Bank
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

		public static Item Quiver
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

		public static Item Mount
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

		internal static string GetBuffDescription(BuffIcon icon)
		{
			string description = "";

			switch (icon)
			{
				case BuffIcon.ActiveMeditation:
					description = "Meditation";
					break;
				case BuffIcon.Agility:
					description = "Agility";
					break;
				case BuffIcon.AnimalForm:
					description = "Animal Form";
					break;
				case BuffIcon.ArcaneEmpowerment:
					description = "Arcane Enpowerment";
					break;
				case BuffIcon.ArcaneEmpowermentNew:
					description = "Arcane Enpowerment (new)";
					break;
				case BuffIcon.ArchProtection:
					description = "Arch Protection";
					break;
				case BuffIcon.ArmorPierce:
					description = "Armor Pierce";
					break;
				case BuffIcon.AttuneWeapon:
					description = "Attunement";
					break;
				case BuffIcon.AuraOfNausea:
					description = "Aura of Nausea";
					break;
				case BuffIcon.Bleed:
					description = "Bleed";
					break;
				case BuffIcon.Bless:
					description = "Bless";
					break;
				case BuffIcon.Block:
					description = "Block";
					break;
				case BuffIcon.BloodOathCaster:
					description = "Bloath Oath (caster)";
					break;
				case BuffIcon.BloodOathCurse:
					description = "Bload Oath (curse)";
					break;
				case BuffIcon.BloodwormAnemia:
					description = "BloodWorm Anemia";
					break;
				case BuffIcon.CityTradeDeal:
					description = "City Trade Deal";
					break;
				case BuffIcon.Clumsy:
					description = "Clumsy";
					break;
				case BuffIcon.Confidence:
					description = "Confidence";
					break;
				case BuffIcon.CorpseSkin:
					description = "Corpse Skin";
					break;
				case BuffIcon.CounterAttack:
					description = "Counter Attack";
					break;
				case BuffIcon.CriminalStatus:
					description = "Criminal";
					break;
				case BuffIcon.Cunning:
					description = "Cunning";
					break;
				case BuffIcon.Curse:
					description = "Curse";
					break;
				case BuffIcon.CurseWeapon:
					description = "Curse Weapon";
					break;
				case BuffIcon.DeathStrike:
					description = "Death Strike";
					break;
				case BuffIcon.DefenseMastery:
					description = "Defense Mastery";
					break;
				case BuffIcon.Despair:
					description = "Despair";
					break;
				case BuffIcon.DespairTarget:
					description = "Despair (target)";
					break;
				case BuffIcon.DisarmNew:
					description = "Disarm (new)";
					break;
				case BuffIcon.Disguised:
					description = "Disguised";
					break;
				case BuffIcon.DismountPrevention:
					description = "Dismount Prevention";
					break;
				case BuffIcon.DivineFury:
					description = "Divine Fury";
					break;
				case BuffIcon.DragonSlasherFear:
					description = "Dragon Slasher Fear";
					break;
				case BuffIcon.Enchant:
					description = "Enchant";
					break;
				case BuffIcon.EnemyOfOne:
					description = "Enemy Of One";
					break;
				case BuffIcon.EnemyOfOneNew:
					description = "Enemy Of One (new)";
					break;
				case BuffIcon.EssenceOfWind:
					description = "Essence Of Wind";
					break;
				case BuffIcon.EtherealVoyage:
					description = "Ethereal Voyage";
					break;
				case BuffIcon.Evasion:
					description = "Evasion";
					break;
				case BuffIcon.EvilOmen:
					description = "Evil Omen";
					break;
				case BuffIcon.FactionLoss:
					description = "Faction Loss";
					break;
				case BuffIcon.FanDancerFanFire:
					description = "Fan Dancer Fan Fire";
					break;
				case BuffIcon.FeebleMind:
					description = "Feeble Mind";
					break;
				case BuffIcon.Feint:
					description = "Feint";
					break;
				case BuffIcon.ForceArrow:
					description = "Force Arrow";
					break;
				case BuffIcon.GargoyleBerserk:
					description = "Berserk";
					break;
				case BuffIcon.GargoyleFly:
					description = "Fly";
					break;
				case BuffIcon.GazeDespair:
					description = "Gaze Despair";
					break;
				case BuffIcon.GiftOfLife:
					description = "Gift Of Life";
					break;
				case BuffIcon.GiftOfRenewal:
					description = "Gift Of Renewal";
					break;
				case BuffIcon.HealingSkill:
					description = "Healing";
					break;
				case BuffIcon.HeatOfBattleStatus:
					description = "Heat Of Battle";
					break;
				case BuffIcon.HidingAndOrStealth:
					description = "Hiding";
					break;
				case BuffIcon.HiryuPhysicalResistance:
					description = "Hiryu Physical Malus";
					break;
				case BuffIcon.HitDualwield:
					description = "Hit Dual Wield";
					break;
				case BuffIcon.HitLowerAttack:
					description = "Hit Lower Attack";
					break;
				case BuffIcon.HitLowerDefense:
					description = "Hit Lower Defense";
					break;
				case BuffIcon.HonorableExecution:
					description = "Honorable Execution";
					break;
				case BuffIcon.Honored:
					description = "Honored";
					break;
				case BuffIcon.HorrificBeast:
					description = "Horrific Beast";
					break;
				case BuffIcon.HowlOfCacophony:
					description = "Hawl Of Cacophony";
					break;
				case BuffIcon.ImmolatingWeapon:
					description = "Immolating Weapon";
					break;
				case BuffIcon.Incognito:
					description = "Incognito";
					break;
				case BuffIcon.Inspire:
					description = "Inspire";
					break;
				case BuffIcon.Invigorate:
					description = "Invigorate";
					break;
				case BuffIcon.Invisibility:
					description = "Invisibility";
					break;
				case BuffIcon.LichForm:
					description = "Lich Form";
					break;
				case BuffIcon.LightningStrike:
					description = "Lighting Strike";
					break;
				case BuffIcon.MagicFish:
					description = "Magic Fish";
					break;
				case BuffIcon.MagicReflection:
					description = "Magic Reflection";
					break;
				case BuffIcon.ManaPhase:
					description = "Mana Phase";
					break;
				case BuffIcon.MassCurse:
					description = "Mass Curse";
					break;
				case BuffIcon.MedusaStone:
					description = "Medusa Stone";
					break;
				case BuffIcon.Mindrot:
					description = "Mind Rot";
					break;
				case BuffIcon.MomentumStrike:
					description = "Momentum Strike";
					break;
				case BuffIcon.MortalStrike:
					description = "Mortal Strike";
					break;
				case BuffIcon.NightSight:
					description = "Night Sight";
					break;
				case BuffIcon.NoRearm:
					description = "NoRearm";
					break;
				case BuffIcon.OrangePetals:
					description = "Orange Petals";
					break;
				case BuffIcon.PainSpike:
					description = "Pain Spike";
					break;
				case BuffIcon.Paralyze:
					description = "Paralyze";
					break;
				case BuffIcon.Perfection:
					description = "Perfection";
					break;
				case BuffIcon.Perseverance:
					description = "Perseverance";
					break;
				case BuffIcon.Poison:
					description = "Poison";
					break;
				case BuffIcon.PoisonResistanceImmunity:
					description = "Poison Resistance";
					break;
				case BuffIcon.Polymorph:
					description = "Polymorph";
					break;
				case BuffIcon.Protection:
					description = "Protection";
					break;
				case BuffIcon.PsychicAttack:
					description = "Psychic Attack";
					break;
				case BuffIcon.Rage:
					description = "Rage";
					break;
				case BuffIcon.RageFocusing:
					description = "Rage Focusing";
					break;
				case BuffIcon.RageFocusingTarget:
					description = "Rage Focusing (target)";
					break;
				case BuffIcon.ReactiveArmor:
					description = "Reactive Armor";
					break;
				case BuffIcon.ReaperForm:
					description = "Reaper Form";
					break;
				case BuffIcon.Resilience:
					description = "Resilience";
					break;
				case BuffIcon.RoseOfTrinsic:
					description = "Rose Of Trinsic";
					break;
				case BuffIcon.RotwormBloodDisease:
					description = "Rotworm Blood Disease";
					break;
				case BuffIcon.RuneBeetleCorruption:
					description = "Rune Beetle Corruption";
					break;
				case BuffIcon.SkillUseDelay:
					description = "Skill Use Delay";
					break;
				case BuffIcon.Sleep:
					description = "Sleep";
					break;
				case BuffIcon.SpellFocusing:
					description = "Spell Focusing";
					break;
				case BuffIcon.SpellFocusingTarget:
					description = "Spell Focusing (target)";
					break;
				case BuffIcon.SpellPlague:
					description = "Spell Plague";
					break;
				case BuffIcon.SplinteringEffect:
					description = "Splintering Effect";
					break;
				case BuffIcon.StoneForm:
					description = "Stone Form";
					break;
				case BuffIcon.Strangle:
					description = "Strangle";
					break;
				case BuffIcon.Strength:
					description = "Strength";
					break;
				case BuffIcon.Surge:
					description = "Surge";
					break;
				case BuffIcon.SwingSpeed:
					description = "Swing Speed";
					break;
				case BuffIcon.TalonStrike:
					description = "Talon Strike";
					break;
			}

			return description;
		}

		// Buff
		public static List<string> Buffs
		{
			get
			{
				List<string> buffs = new List<string>();
				foreach (ushort icon in Assistant.World.Player.Buffs)
				{
					buffs.Add(icon.ToString());
				}
				return buffs;
			}
		}
		public static bool BuffsExist(string buffname)
		{
			foreach (ushort icon in Assistant.World.Player.Buffs)
			{
				if (icon.ToString() == buffname)
					return true;
			}
			return false;
		}
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

		public static void EquipItem(Item item)
		{
			Assistant.Mobile player = Assistant.World.Player;
			if (item.Container == null && Misc.DistanceSqrt(item.GetWorldPosition(), Position) > 3)
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

		public Item GetItemOnLayer(String layer)
		{
			Assistant.Layer assistantLayer = GetAssistantLayer(layer);

			Assistant.Item assistantItem = null;
			if (assistantLayer != Assistant.Layer.Invalid)
			{
				assistantItem = Assistant.World.Player.GetItemOnLayer(assistantLayer);
				if (assistantItem == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
					return enhancedItem;
				}
			}
			else
				return null;
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
				case "Animal Lore":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.AnimalLore)));
					break;
				case "Item ID":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.ItemID)));
					break;
				case "Arms Lore":
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
				case "Detect Hidden":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.DetectHidden)));
					break;
				case "Discordance":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Discordance)));
					break;
				case "Eval Int":
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
				case "Spirit Speak":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.SpiritSpeak)));
					break;
				case "Stealing":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.Stealing)));
					break;
				case "Animal Taming":
					Assistant.ClientCommunication.SendToServer(new UseSkill(Convert.ToInt16(Assistant.SkillName.AnimalTaming)));
					break;
				case "Taste ID":
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
				case "Remove Trap":
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
		public static void ChatSay(int hue, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Regular, hue, 1, msg));
		}

		public static void ChatGuild(string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Guild, 1, 1, msg));
		}

		public static void ChatAlliance(string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Alliance, 1, 1, msg));
		}

		public static void ChatEmote(int hue, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Emote, hue, 1, msg));
		}

		public static void ChatWhisper(int hue, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Whisper, hue, 1, msg));
		}

		public static void ChatYell(int hue, string msg)
		{
			Assistant.ClientCommunication.SendToServer(new ClientAsciiMessage(Assistant.MessageType.Yell, hue, 1, msg));
		}

		// attack
		public static void SetWarMode(bool warflag)
		{
			Assistant.ClientCommunication.SendToServer(new SetWarMode(warflag));
		}

		public static void Attack(int serial)
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
		// Message

		public static void HeadMessage(int hue, string message)
		{
			Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, hue, 3, Language.CliLocName, World.Player.Name, message));
		}

		// Paperdool button click
		public static void QuestButton()
		{
			Assistant.ClientCommunication.SendToServer(new QuestButton(World.Player.Serial));
		}
		public static void GuildButton()
		{
			Assistant.ClientCommunication.SendToServer(new GuildButton(World.Player.Serial));
		}

		// open bank
		public static void OpenBank(string text)
		{
			List<ushort> kw = new List<ushort> { 16, 2 };
			ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, text));
		}

	}
}
