using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RazorEnhanced
{
	public class Player
	{
		// Stats
		public static int Hits { get { return World.Player.Hits; } }

		public static int HitsMax { get { return World.Player.HitsMax; } }
		public static int Str { get { return World.Player.Str; } }
		public static int Mana { get { return World.Player.Mana; } }
		public static int ManaMax { get { return World.Player.ManaMax; } }
		public static int Int { get { return World.Player.Int; } }
		public static int Stam { get { return World.Player.Stam; } }
		public static int StamMax { get { return World.Player.StamMax; } }
		public static int Dex { get { return World.Player.Dex; } }
		public static int StatCap { get { return World.Player.StatCap; } }

		// Resistance
		public static int AR { get { return World.Player.AR; } }

		public static int FireResistance { get { return World.Player.FireResistance; } }
		public static int ColdResistance { get { return World.Player.ColdResistance; } }
		public static int EnergyResistance { get { return World.Player.EnergyResistance; } }
		public static int PoisonResistance { get { return World.Player.PoisonResistance; } }

		// KR Attribute
		public static int SwingSpeedIncrease { get { return World.Player.SwingSpeedIncrease; } }
		public static int DamageChanceIncrease { get { return World.Player.DamageChanceIncrease; } }
		public static int LowerReagentCost { get { return World.Player.LowerReagentCost; } }
		public static int HitPointsRegeneration { get { return World.Player.HitPointsRegeneration; } }
		public static int StaminaRegeneration { get { return World.Player.StaminaRegeneration; } }
		public static int ManaRegeneration { get { return World.Player.ManaRegeneration; } }
		public static int ReflectPhysicalDamage { get { return World.Player.ReflectPhysicalDamage; } }
		public static int EnhancePotions { get { return World.Player.EnhancePotions; } }
		public static int DefenseChanceIncrease { get { return World.Player.DefenseChanceIncrease; } }
		public static int SpellDamageIncrease { get { return World.Player.SpellDamageIncrease; } }
		public static int FasterCastRecovery { get { return World.Player.FasterCastRecovery; } }
		public static int FasterCasting { get { return World.Player.FasterCasting; } }
		public static int LowerManaCost { get { return World.Player.LowerManaCost; } }
		public static int StrengthIncrease { get { return World.Player.StrengthIncrease; } }
		public static int DexterityIncrease { get { return World.Player.DexterityIncrease; } }
		public static int IntelligenceIncrease { get { return World.Player.IntelligenceIncrease; } }
		public static int HitPointsIncrease { get { return World.Player.HitPointsIncrease; } }
		public static int StaminaIncrease { get { return World.Player.StaminaIncrease; } }
		public static int ManaIncrease { get { return World.Player.ManaIncrease; } }
		public static int MaximumHitPointsIncrease { get { return World.Player.MaximumHitPointsIncrease; } }
		public static int MaximumStaminaIncrease { get { return World.Player.MaximumStaminaIncrease; } }
		public static int MaximumManaIncrease { get { return World.Player.MaximumManaIncrease; } }

		// Flags
		public static bool IsGhost { get { return World.Player.IsGhost; } }

		public static bool Poisoned { get { return World.Player.Poisoned; } }
		public static bool YellowHits { get { return World.Player.Blessed; } }
		public static bool Visible { get { return World.Player.Visible; } }
		public static bool WarMode { get { return World.Player.Warmode; } }
		public static bool Paralized { get { return World.Player.Paralized; } }
		public static bool HasSpecial { get { return World.Player.HasSpecial; } }

		// Self
		public static bool Female { get { return World.Player.Female; } }

		public static String Name { get { return World.Player.Name; } }
		public static byte Notoriety { get { return World.Player.Notoriety; } }

		public static Item Backpack
		{
			get
			{
				Assistant.Item assistantBackpack = World.Player.Backpack;
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
				Assistant.Item assistantBank = World.Player.GetItemOnLayer(Layer.Bank);
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
				Assistant.Item assistantQuiver = World.Player.Quiver;
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
				Assistant.Item assistantMount = World.Player.GetItemOnLayer(Assistant.Layer.Mount);
				if (assistantMount == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedMount = new RazorEnhanced.Item(assistantMount);
					return enhancedMount;
				}
			}
		}

		public static int Gold { get { return Convert.ToInt32(World.Player.Gold); } }
		public static int Luck { get { return World.Player.Luck; } }
		public static int Body { get { return World.Player.Body; } }
		public static int Serial { get { return World.Player.Serial; } }

		// Follower
		public static int FollowersMax { get { return World.Player.FollowersMax; } }

		public static int Followers { get { return World.Player.Followers; } }

		// Weight
		public static int MaxWeight { get { return World.Player.MaxWeight; } }

		public static int Weight { get { return World.Player.Weight; } }

		// Position
		public static Point3D Position { get { return new Point3D(World.Player.Position); } }
		public static int Map { get { return World.Player.Map; } }
		public static string Direction
		{
			get
			{
				switch (World.Player.Direction & Assistant.Direction.Mask)
				{
					case Assistant.Direction.North: return "North";
					case Assistant.Direction.South: return "South";
					case Assistant.Direction.West: return "West";
					case Assistant.Direction.East: return "East";
					case Assistant.Direction.Right: return "Right";
					case Assistant.Direction.Left: return "Left";
					case Assistant.Direction.Down: return "Down";
					case Assistant.Direction.Up: return "Up";
					default: return "Undefined";
				}
			}
		}

		public int DistanceTo(Mobile m)
		{
			int x = Math.Abs(Position.X - m.Position.X);
			int y = Math.Abs(Position.Y - m.Position.Y);

			return x > y ? x : y;
		}

		public int DistanceTo(Item i)
		{
			int x = Math.Abs(Position.X - i.Position.X);
			int y = Math.Abs(Position.Y - i.Position.Y);

			return x > y ? x : y;
		}

		internal static string GetBuffDescription(BuffIcon icon)
		{
			string description = String.Empty;

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
					description = "Bload Oath (caster)";
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

				case BuffIcon.ConsecrateWeapon:
					description = "Consecrate Weapon";
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

				case BuffIcon.Weaken:
					description = "Weaken";
					break;
				case BuffIcon.VampiricEmbrace:
					description = "Vampiric Embrace";
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
				foreach (BuffIcon icon in World.Player.Buffs)
				{
					buffs.Add(GetBuffDescription(icon));
				}
				return buffs;
			}
		}

		public static bool BuffsExist(string buffname)
		{
			return World.Player.Buffs.Any(icon => GetBuffDescription(icon) == buffname);
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

				case "Talisman":
					result = Assistant.Layer.Talisman;
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

			Assistant.Item item = World.Player.GetItemOnLayer(assistantLayer);

			if (item != null)
			{
				ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
				ClientCommunication.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, World.Player.Backpack.Serial));
			}
			else
			{
				Scripts.SendMessageScriptError("Script Error: UnEquipItemByLayer: No item found on layer: " + layer);
			}
		}

		public static void EquipItem(int serial)
		{
			Assistant.Item item = World.FindItem((Assistant.Serial)serial);
			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: EquipItem: Item serial: (" + serial + ") not found");
				return;
			}

			if (item.Container == null && Assistant.Utility.Distance(item.GetWorldPosition(), World.Player.Position) > 3)
			{
				Scripts.SendMessageScriptError("Script Error: EquipItem: Item serial: (" + serial + ") too away");
				return;
			}
			ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount)); // Prende
			ClientCommunication.SendToServerWait(new EquipRequest(item.Serial, World.Player.Serial, item.Layer)); // Equippa
		}

		public static void EquipItem(Item item)
		{
			Assistant.Mobile player = World.Player;
			if (item.Container == null && Misc.DistanceSqrt(item.GetWorldPosition(), Position) > 3)
			{
				Scripts.SendMessageScriptError("Script Error: EquipItem: Item serial: (" + item.Serial + ") too away");
				return;
			}
			ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount)); // Prende
			ClientCommunication.SendToServerWait(new EquipRequest(item.Serial, World.Player.Serial, item.AssistantLayer)); // Equippa
		}

		public static void EquipUO3D(List<int> serials)
		{
			List<uint> serialstoequip = new List<uint>(); 
			foreach (int serial in serials)
				serialstoequip.Add((uint)serial);

			ClientCommunication.SendToServerWait(new EquipItemMacro(serialstoequip));

		}
		public static bool CheckLayer(String layer)
		{
			Assistant.Layer assistantLayer = GetAssistantLayer(layer);

			Assistant.Item item = World.Player.GetItemOnLayer(assistantLayer);

			if (item != null)
				return true;
			else
				return false;
		}

		public Item GetItemOnLayer(String layer)
		{
			Assistant.Layer assistantLayer = GetAssistantLayer(layer);

			Assistant.Item assistantItem = null;
			if (assistantLayer != Assistant.Layer.Invalid)
			{
				assistantItem = World.Player.GetItemOnLayer(assistantLayer);
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
					return World.Player.Skills[(int)SkillName.Alchemy].Value;

				case "Animal Lore":
					return World.Player.Skills[(int)SkillName.AnimalLore].Value;

				case "Item ID":
					return World.Player.Skills[(int)SkillName.ItemID].Value;

				case "Arms Lore":
					return World.Player.Skills[(int)SkillName.ArmsLore].Value;

				case "Parry":
					return World.Player.Skills[(int)SkillName.Parry].Value;

				case "Begging":
					return World.Player.Skills[(int)SkillName.Begging].Value;

				case "Blacksmith":
					return World.Player.Skills[(int)SkillName.Blacksmith].Value;

				case "Fletching":
					return World.Player.Skills[(int)SkillName.Fletching].Value;

				case "Peacemaking":
					return World.Player.Skills[(int)SkillName.Peacemaking].Value;

				case "Camping":
					return World.Player.Skills[(int)SkillName.Camping].Value;

				case "Carpentry":
					return World.Player.Skills[(int)SkillName.Carpentry].Value;

				case "Cartography":
					return World.Player.Skills[(int)SkillName.Cartography].Value;

				case "Cooking":
					return World.Player.Skills[(int)SkillName.Cooking].Value;

				case "Detect Hidden":
					return World.Player.Skills[(int)SkillName.DetectHidden].Value;

				case "Discordance":
					return World.Player.Skills[(int)SkillName.Discordance].Value;

				case "Eval Int":
					return World.Player.Skills[(int)SkillName.EvalInt].Value;

				case "Healing":
					return World.Player.Skills[(int)SkillName.Healing].Value;

				case "Fishing":
					return World.Player.Skills[(int)SkillName.Fishing].Value;

				case "Forensics":
					return World.Player.Skills[(int)SkillName.Forensics].Value;

				case "Herding":
					return World.Player.Skills[(int)SkillName.Herding].Value;

				case "Hiding":
					return World.Player.Skills[(int)SkillName.Hiding].Value;

				case "Provocation":
					return World.Player.Skills[(int)SkillName.Provocation].Value;

				case "Inscribe":
					return World.Player.Skills[(int)SkillName.Inscribe].Value;

				case "Lockpicking":
					return World.Player.Skills[(int)SkillName.Lockpicking].Value;

				case "Magery":
					return World.Player.Skills[(int)SkillName.Magery].Value;

				case "Magic Resist":
					return World.Player.Skills[(int)SkillName.MagicResist].Value;

				case "Tactics":
					return World.Player.Skills[(int)SkillName.Tactics].Value;

				case "Snooping":
					return World.Player.Skills[(int)SkillName.Snooping].Value;

				case "Musicianship":
					return World.Player.Skills[(int)SkillName.Musicianship].Value;

				case "Poisoning":
					return World.Player.Skills[(int)SkillName.Poisoning].Value;

				case "Archery":
					return World.Player.Skills[(int)SkillName.Archery].Value;

				case "Spirit Speak":
					return World.Player.Skills[(int)SkillName.SpiritSpeak].Value;

				case "Stealing":
					return World.Player.Skills[(int)SkillName.Stealing].Value;

				case "Tailoring":
					return World.Player.Skills[(int)SkillName.Tailoring].Value;

				case "Animal Taming":
					return World.Player.Skills[(int)SkillName.AnimalTaming].Value;

				case "Taste ID":
					return World.Player.Skills[(int)SkillName.TasteID].Value;

				case "Tinkering":
					return World.Player.Skills[(int)SkillName.Tinkering].Value;

				case "Tracking":
					return World.Player.Skills[(int)SkillName.Tracking].Value;

				case "Veterinary":
					return World.Player.Skills[(int)SkillName.Veterinary].Value;

				case "Swords":
					return World.Player.Skills[(int)SkillName.Swords].Value;

				case "Macing":
					return World.Player.Skills[(int)SkillName.Macing].Value;

				case "Fencing":
					return World.Player.Skills[(int)SkillName.Fencing].Value;

				case "Wrestling":
					return World.Player.Skills[(int)SkillName.Wrestling].Value;

				case "Lumberjacking":
					return World.Player.Skills[(int)SkillName.Lumberjacking].Value;

				case "Mining":
					return World.Player.Skills[(int)SkillName.Mining].Value;

				case "Meditation":
					return World.Player.Skills[(int)SkillName.Meditation].Value;

				case "Stealth":
					return World.Player.Skills[(int)SkillName.Stealth].Value;

				case "Remove Trap":
					return World.Player.Skills[(int)SkillName.RemoveTrap].Value;

				case "Necromancy":
					return World.Player.Skills[(int)SkillName.Necromancy].Value;

				case "Focus":
					return World.Player.Skills[(int)SkillName.Focus].Value;

				case "Chivalry":
					return World.Player.Skills[(int)SkillName.Chivalry].Value;

				case "Bushido":
					return World.Player.Skills[(int)SkillName.Bushido].Value;

				case "Ninjitsu":
					return World.Player.Skills[(int)SkillName.Ninjitsu].Value;

				case "Spell Weaving":
					return World.Player.Skills[(int)SkillName.SpellWeaving].Value;

				default:
					Scripts.SendMessageScriptError("Script Error: GetSkillValue: Invalid skill name: " + skillname);
					return 0;
			}
		}

		public static double GetSkillCap(string skillname)
		{
			switch (skillname)
			{
				case "Alchemy":
					return World.Player.Skills[(int)SkillName.Alchemy].Cap;

				case "Animal Lore":
					return World.Player.Skills[(int)SkillName.AnimalLore].Cap;

				case "Item ID":
					return World.Player.Skills[(int)SkillName.ItemID].Cap;

				case "ArmsLore":
					return World.Player.Skills[(int)SkillName.ArmsLore].Cap;

				case "Parry":
					return World.Player.Skills[(int)SkillName.Parry].Cap;

				case "Begging":
					return World.Player.Skills[(int)SkillName.Begging].Cap;

				case "Blacksmith":
					return World.Player.Skills[(int)SkillName.Blacksmith].Cap;

				case "Fletching":
					return World.Player.Skills[(int)SkillName.Fletching].Cap;

				case "Peacemaking":
					return World.Player.Skills[(int)SkillName.Peacemaking].Cap;

				case "Camping":
					return World.Player.Skills[(int)SkillName.Camping].Cap;

				case "Carpentry":
					return World.Player.Skills[(int)SkillName.Carpentry].Cap;

				case "Cartography":
					return World.Player.Skills[(int)SkillName.Cartography].Cap;

				case "Cooking":
					return World.Player.Skills[(int)SkillName.Cooking].Cap;

				case "Detect Hidden":
					return World.Player.Skills[(int)SkillName.DetectHidden].Cap;

				case "Discordance":
					return World.Player.Skills[(int)SkillName.Discordance].Cap;

				case "EvalInt":
					return World.Player.Skills[(int)SkillName.EvalInt].Cap;

				case "Healing":
					return World.Player.Skills[(int)SkillName.Healing].Cap;

				case "Fishing":
					return World.Player.Skills[(int)SkillName.Fishing].Cap;

				case "Forensics":
					return World.Player.Skills[(int)SkillName.Forensics].Cap;

				case "Herding":
					return World.Player.Skills[(int)SkillName.Herding].Cap;

				case "Hiding":
					return World.Player.Skills[(int)SkillName.Hiding].Cap;

				case "Provocation":
					return World.Player.Skills[(int)SkillName.Provocation].Cap;

				case "Inscribe":
					return World.Player.Skills[(int)SkillName.Inscribe].Cap;

				case "Lockpicking":
					return World.Player.Skills[(int)SkillName.Lockpicking].Cap;

				case "Magery":
					return World.Player.Skills[(int)SkillName.Magery].Cap;

				case "Magic Resist":
					return World.Player.Skills[(int)SkillName.MagicResist].Cap;

				case "Tactics":
					return World.Player.Skills[(int)SkillName.Tactics].Cap;

				case "Snooping":
					return World.Player.Skills[(int)SkillName.Snooping].Cap;

				case "Musicianship":
					return World.Player.Skills[(int)SkillName.Musicianship].Cap;

				case "Poisoning":
					return World.Player.Skills[(int)SkillName.Poisoning].Cap;

				case "Archery":
					return World.Player.Skills[(int)SkillName.Archery].Cap;

				case "Spirit Speak":
					return World.Player.Skills[(int)SkillName.SpiritSpeak].Cap;

				case "Stealing":
					return World.Player.Skills[(int)SkillName.Stealing].Cap;

				case "Tailoring":
					return World.Player.Skills[(int)SkillName.Tailoring].Cap;

				case "Animal Taming":
					return World.Player.Skills[(int)SkillName.AnimalTaming].Cap;

				case "Taste ID":
					return World.Player.Skills[(int)SkillName.TasteID].Cap;

				case "Tinkering":
					return World.Player.Skills[(int)SkillName.Tinkering].Cap;

				case "Tracking":
					return World.Player.Skills[(int)SkillName.Tracking].Cap;

				case "Veterinary":
					return World.Player.Skills[(int)SkillName.Veterinary].Cap;

				case "Swords":
					return World.Player.Skills[(int)SkillName.Swords].Cap;

				case "Macing":
					return World.Player.Skills[(int)SkillName.Macing].Cap;

				case "Fencing":
					return World.Player.Skills[(int)SkillName.Fencing].Cap;

				case "Wrestling":
					return World.Player.Skills[(int)SkillName.Wrestling].Cap;

				case "Lumberjacking":
					return World.Player.Skills[(int)SkillName.Lumberjacking].Cap;

				case "Mining":
					return World.Player.Skills[(int)SkillName.Mining].Cap;

				case "Meditation":
					return World.Player.Skills[(int)SkillName.Meditation].Cap;

				case "Stealth":
					return World.Player.Skills[(int)SkillName.Stealth].Cap;

				case "Remove Trap":
					return World.Player.Skills[(int)SkillName.RemoveTrap].Cap;

				case "Necromancy":
					return World.Player.Skills[(int)SkillName.Necromancy].Cap;

				case "Focus":
					return World.Player.Skills[(int)SkillName.Focus].Cap;

				case "Chivalry":
					return World.Player.Skills[(int)SkillName.Chivalry].Cap;

				case "Bushido":
					return World.Player.Skills[(int)SkillName.Bushido].Cap;

				case "Ninjitsu":
					return World.Player.Skills[(int)SkillName.Ninjitsu].Cap;

				case "Spell Weaving":
					return World.Player.Skills[(int)SkillName.SpellWeaving].Cap;

				default:
					Scripts.SendMessageScriptError("Script Error: GetSkillCap: Invalid skill name: " + skillname);
					return 0;
			}
		}

		public static int GetSkillStatus(string skillname)
		{
			switch (skillname)
			{
				case "Alchemy":
					return (int)World.Player.Skills[(int)SkillName.Alchemy].Lock;

				case "Animal Lore":
					return (int)World.Player.Skills[(int)SkillName.AnimalLore].Lock;

				case "Item ID":
					return (int)World.Player.Skills[(int)SkillName.ItemID].Lock;

				case "Arms Lore":
					return (int)World.Player.Skills[(int)SkillName.ArmsLore].Lock;

				case "Parry":
					return (int)World.Player.Skills[(int)SkillName.Parry].Lock;

				case "Begging":
					return (int)World.Player.Skills[(int)SkillName.Begging].Lock;

				case "Blacksmith":
					return (int)World.Player.Skills[(int)SkillName.Blacksmith].Lock;

				case "Fletching":
					return (int)World.Player.Skills[(int)SkillName.Fletching].Lock;

				case "Peacemaking":
					return (int)World.Player.Skills[(int)SkillName.Peacemaking].Lock;

				case "Camping":
					return (int)World.Player.Skills[(int)SkillName.Camping].Lock;

				case "Carpentry":
					return (int)World.Player.Skills[(int)SkillName.Carpentry].Lock;

				case "Cartography":
					return (int)World.Player.Skills[(int)SkillName.Cartography].Lock;

				case "Cooking":
					return (int)World.Player.Skills[(int)SkillName.Cooking].Lock;

				case "Detect Hidden":
					return (int)World.Player.Skills[(int)SkillName.DetectHidden].Lock;

				case "Discordance":
					return (int)World.Player.Skills[(int)SkillName.Discordance].Lock;

				case "Eval Int":
					return (int)World.Player.Skills[(int)SkillName.EvalInt].Lock;

				case "Healing":
					return (int)World.Player.Skills[(int)SkillName.Healing].Lock;

				case "Fishing":
					return (int)World.Player.Skills[(int)SkillName.Fishing].Lock;

				case "Forensics":
					return (int)World.Player.Skills[(int)SkillName.Forensics].Lock;

				case "Herding":
					return (int)World.Player.Skills[(int)SkillName.Herding].Lock;

				case "Hiding":
					return (int)World.Player.Skills[(int)SkillName.Hiding].Lock;

				case "Provocation":
					return (int)World.Player.Skills[(int)SkillName.Provocation].Lock;

				case "Inscribe":
					return (int)World.Player.Skills[(int)SkillName.Inscribe].Lock;

				case "Lockpicking":
					return (int)World.Player.Skills[(int)SkillName.Lockpicking].Lock;

				case "Magery":
					return (int)World.Player.Skills[(int)SkillName.Magery].Lock;

				case "Magic Resist":
					return (int)World.Player.Skills[(int)SkillName.MagicResist].Lock;

				case "Tactics":
					return (int)World.Player.Skills[(int)SkillName.Tactics].Lock;

				case "Snooping":
					return (int)World.Player.Skills[(int)SkillName.Snooping].Lock;

				case "Musicianship":
					return (int)World.Player.Skills[(int)SkillName.Musicianship].Lock;

				case "Poisoning":
					return (int)World.Player.Skills[(int)SkillName.Poisoning].Lock;

				case "Archery":
					return (int)World.Player.Skills[(int)SkillName.Archery].Lock;

				case "Spirit Speak":
					return (int)World.Player.Skills[(int)SkillName.SpiritSpeak].Lock;

				case "Stealing":
					return (int)World.Player.Skills[(int)SkillName.Stealing].Lock;

				case "Tailoring":
					return (int)World.Player.Skills[(int)SkillName.Tailoring].Lock;

				case "Animal Taming":
					return (int)World.Player.Skills[(int)SkillName.AnimalTaming].Lock;

				case "Taste ID":
					return (int)World.Player.Skills[(int)SkillName.TasteID].Lock;

				case "Tinkering":
					return (int)World.Player.Skills[(int)SkillName.Tinkering].Lock;

				case "Tracking":
					return (int)World.Player.Skills[(int)SkillName.Tracking].Lock;

				case "Veterinary":
					return (int)World.Player.Skills[(int)SkillName.Veterinary].Lock;

				case "Swords":
					return (int)World.Player.Skills[(int)SkillName.Swords].Lock;

				case "Macing":
					return (int)World.Player.Skills[(int)SkillName.Macing].Lock;

				case "Fencing":
					return (int)World.Player.Skills[(int)SkillName.Fencing].Lock;

				case "Wrestling":
					return (int)World.Player.Skills[(int)SkillName.Wrestling].Lock;

				case "Lumberjacking":
					return (int)World.Player.Skills[(int)SkillName.Lumberjacking].Lock;

				case "Mining":
					return (int)World.Player.Skills[(int)SkillName.Mining].Lock;

				case "Meditation":
					return (int)World.Player.Skills[(int)SkillName.Meditation].Lock;

				case "Stealth":
					return (int)World.Player.Skills[(int)SkillName.Stealth].Lock;

				case "Remove Trap":
					return (int)World.Player.Skills[(int)SkillName.RemoveTrap].Lock;

				case "Necromancy":
					return (int)World.Player.Skills[(int)SkillName.Necromancy].Lock;

				case "Focus":
					return (int)World.Player.Skills[(int)SkillName.Focus].Lock;

				case "Chivalry":
					return (int)World.Player.Skills[(int)SkillName.Chivalry].Lock;

				case "Bushido":
					return (int)World.Player.Skills[(int)SkillName.Bushido].Lock;

				case "Ninjitsu":
					return (int)World.Player.Skills[(int)SkillName.Ninjitsu].Lock;

				case "Spell Weaving":
					return (int)World.Player.Skills[(int)SkillName.SpellWeaving].Lock;

				default:
					Scripts.SendMessageScriptError("Script Error: GetSkillStatus: Invalid skill name: " + skillname);
					return 0;
			}
		}

		public static void UseSkill(string skillname, bool wait = true)
		{
			short skillID = 0;
			switch (skillname)
			{
				case "Animal Lore":
					skillID = (int)SkillName.AnimalLore;
					break;

				case "Item ID":
					skillID = (int)SkillName.ItemID;
					break;

				case "Arms Lore":
					skillID = (int)SkillName.ArmsLore;
					break;

				case "Begging":
					skillID = (int)SkillName.Begging;
					break;

				case "Peacemaking":
					skillID = (int)SkillName.Peacemaking;
					break;

				case "Cartography":
					skillID = (int)SkillName.Cartography;
					break;

				case "Detect Hidden":
					skillID = (int)SkillName.DetectHidden;
					break;

				case "Discordance":
					skillID = (int)SkillName.Discordance;					
					break;

				case "Eval Int":
					skillID = (int)SkillName.EvalInt;
					break;

				case "Forensics":
					skillID = (int)SkillName.Forensics;
					break;

				case "Hiding":
					skillID = (int)SkillName.Hiding;
					StealthSteps.Hide();
					break;

				case "Provocation":
					skillID = (int)SkillName.Provocation;
					break;

				case "Poisoning":
					skillID = (int)SkillName.Poisoning;
					break;

				case "Spirit Speak":
					skillID = (int)SkillName.SpiritSpeak;
					break;

				case "Stealing":
					skillID = (int)SkillName.Stealing;
					break;

				case "Animal Taming":
					skillID = (int)SkillName.AnimalTaming;
					break;

				case "Taste ID":
					skillID = (int)SkillName.TasteID;
					break;

				case "Tracking":
					skillID = (int)SkillName.Tracking;
					break;

				case "Meditation":
					skillID = (int)SkillName.Meditation;
					break;

				case "Stealth":
					skillID = (int)SkillName.Stealth;
					if (!World.Player.Visible) // Trigger stealth step counter
						StealthSteps.Hide();
					break;

				case "Remove Trap":
					skillID = (int)SkillName.RemoveTrap;
					break;

				case "Inscribe":
					skillID = (int)SkillName.Inscribe;
					break;

				case "Anatomy":
					skillID = (int)SkillName.Anatomy;
					break;

				case "Imbuing":
					skillID = (int)SkillName.Imbuing;
					break;
					
				default:
					Scripts.SendMessageScriptError("Script Error: UseSkill: Invalid skill name: " + skillname);
					break;
			}

			if (skillID != 0)
			{
				if (wait)
					ClientCommunication.SendToServerWait(new UseSkill(skillID));
				else
					ClientCommunication.SendToServer(new UseSkill(skillID));
			}
		}

		// Game Message
		public static void ChatSay(int hue, int num)
		{
			ChatSay(hue, num.ToString());
		}

		public static void ChatSay(int hue, string msg)
		{
			List<ushort> kw = EncodedSpeech.GetKeywords(msg);
			if (kw.Count == 1 && kw[0] == 0)
			{
				ClientCommunication.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Regular, hue, 3, Language.CliLocName, kw, msg));
			}
			else
			{
				ClientCommunication.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Encoded, hue, 3, Language.CliLocName, kw, msg));
			}
		}
		public static void ChatGuild(int num)
		{
			ChatGuild(num.ToString());
		}

		public static void ChatGuild(string msg)
		{
			ClientCommunication.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Guild, 1, 1, msg));
		}

		public static void ChatAlliance(int num)
		{
			ChatAlliance(num.ToString());
		}

		public static void ChatAlliance(string msg)
		{
			ClientCommunication.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Alliance, 1, 1, msg));
		}

		public static void ChatEmote(int hue, int num)
		{
			ChatEmote(hue, num.ToString());
		}

		public static void ChatEmote(int hue, string msg)
		{
			ClientCommunication.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Emote, hue, 1, msg));
		}

		public static void ChatWhisper(int hue, int num)
		{
			ChatWhisper(hue, num.ToString());
		}

		public static void ChatWhisper(int hue, string msg)
		{
			ClientCommunication.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Whisper, hue, 1, msg));
		}

		public static void ChatYell(int hue, int num)
		{
			ChatYell(hue, num.ToString());
		}

		public static void ChatYell(int hue, string msg)
		{
			ClientCommunication.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Yell, hue, 1, msg));
		}

		public static void ChatChannel(int num)
		{
			ChatChannel(num.ToString());
		}

		public static void ChatChannel(string msg)
		{
			ClientCommunication.SendToServerWait(new ChatAction(0x61, Language.CliLocName, msg));
		}
		
		// attack
		public static void SetWarMode(bool warflag)
		{
			ClientCommunication.SendToServerWait(new SetWarMode(warflag));
		}
		public static void Attack(Mobile m)
		{
			Attack(m.Serial);
		}

		public static void Attack(int serial)
		{
			if (World.FindMobile(serial) == null) // Mob non piu esistente
				return;

			Target.AttackMessage(serial, true);
			if (Targeting.LastAttack != serial)
			{
				ClientCommunication.SendToClientWait(new ChangeCombatant(serial));
				Targeting.LastAttack = (uint)serial;
			}
			ClientCommunication.SendToServerWait(new AttackReq(serial));
        }

		public static void AttackLast()
		{
			if (Targeting.LastAttack == 0) // Nessun last attack presente
				return;
			
			if (World.FindMobile(Targeting.LastAttack) == null) // Mob non piu esistente
				return;

			Target.AttackMessage((int)Targeting.LastAttack, true);

			ClientCommunication.SendToServerWait(new AttackReq(Targeting.LastAttack));
		}

		// Virtue
		public static void InvokeVirtue(string virtue)
		{
			switch (virtue)
			{
				case "Honor":
					ClientCommunication.SendToServerWait(new InvokeVirtue(1));
					break;

				case "Sacrifice":
					ClientCommunication.SendToServerWait(new InvokeVirtue(2));
					break;

				case "Valor":
					ClientCommunication.SendToServerWait(new InvokeVirtue(3));
					break;

				case "Compassion":
					ClientCommunication.SendToServerWait(new InvokeVirtue(4));
					break;

				case "Honesty":
					ClientCommunication.SendToServerWait(new InvokeVirtue(5));
					break;

				case "Humility":
					ClientCommunication.SendToServerWait(new InvokeVirtue(6));
					break;

				case "Justice":
					ClientCommunication.SendToServerWait(new InvokeVirtue(7));
					break;

				default:
					Scripts.SendMessageScriptError("Script Error - InvokeVirtue: Invalid virtue name: " + virtue);
					break;
			}
		}

		public static void ChatParty(string msg)
		{
			ClientCommunication.SendToServerWait(new SendPartyMessage(World.Player.Serial, msg));
		}

		public static void PartyInvite()
		{
			ClientCommunication.SendToServerWait(new PartyInvite());
		}

		public static void LeaveParty()
		{
			ClientCommunication.SendToServerWait(new PartyRemoveMember(World.Player.Serial));
		}

		public static void KickMember(int serial)
		{
			uint userial = Convert.ToUInt16(serial);
			ClientCommunication.SendToServerWait(new PartyRemoveMember(userial));
		}

		public static void PartyCanLoot(bool CanLoot)
		{
			if (CanLoot)
					ClientCommunication.SendToServerWait(new PartyCanLoot(0x1));
				else
					ClientCommunication.SendToServerWait(new PartyCanLoot(0x0));
		}

		// Moving
		public static bool Walk(string direction)  // Return true se walk ok false se rifiutato da server
		{
			Direction dir;
			switch (direction)
			{
				case "North":
					dir = Assistant.Direction.North;
					break;

				case "South":
					dir = Assistant.Direction.South;
					break;

				case "East":
					dir = Assistant.Direction.East;
					break;

				case "West":
					dir = Assistant.Direction.West;
					break;

				case "Up":
					dir = Assistant.Direction.Up;
					break;

				case "Down":
					dir = Assistant.Direction.Down;
					break;

				case "Left":
					dir = Assistant.Direction.Left;
					break;

				case "Right":
					dir = Assistant.Direction.Right;
					break;

				default:
					dir = Assistant.Direction.ValueMask;
					break;
			}

			int state = 0;
			if (dir != Assistant.Direction.ValueMask)
			{
				int timeout = 0;
				World.Player.WalkScriptRequest = 1;
				ClientCommunication.SendToServer(new WalkRequest(dir, World.Player.WalkSequence));
				while (World.Player.WalkScriptRequest < 2)
				{
					Thread.Sleep(10);
					timeout += 10;
					if (timeout > 2000)
						break;
				}
				state = World.Player.WalkScriptRequest;
				World.Player.WalkScriptRequest = 0;
			}
			if (state == 2)
				return true;
			else
				return false;
		}

		public static bool Run(string direction)    // Return true se walk ok false se rifiutato da server
		{
			Direction dir;
			switch (direction)
			{
				case "North":
					dir = Assistant.Direction.North;
					break;

				case "South":
					dir = Assistant.Direction.South;
					break;

				case "East":
					dir = Assistant.Direction.East;
					break;

				case "West":
					dir = Assistant.Direction.West;
					break;

				case "Up":
					dir = Assistant.Direction.Up;
					break;

				case "Down":
					dir = Assistant.Direction.Down;
					break;

				case "Left":
					dir = Assistant.Direction.Left;
					break;

				case "Right":
					dir = Assistant.Direction.Right;
					break;

				default:
					dir = Assistant.Direction.ValueMask;
					break;
			}
			int state = 0;
			if (dir != Assistant.Direction.ValueMask)
			{
				int timeout = 0;
				World.Player.WalkScriptRequest = 1;
				ClientCommunication.SendToServer(new WalkRequest(dir | Assistant.Direction.Running, World.Player.WalkSequence));
				while (World.Player.WalkScriptRequest < 2)
				{
					Thread.Sleep(10);
					timeout += 10;
					if (timeout > 2000)
						break;
				}
				state = World.Player.WalkScriptRequest;
				World.Player.WalkScriptRequest = 0;
			}
			if (state == 2)
				return true;
			else
				return false;
		}

		internal static void PathFindTo(Assistant.Point3D Location)
		{
			PathFindTo(Location.X, Location.Y, Location.Z);
		}

		public static void PathFindTo(int x, int y, int z)
		{
			UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();
			while (!UoWarper.UODLLHandleClass.Open(1))
			{
				Thread.Sleep(10);
			}
			UoWarper.UODLLHandleClass.Pathfind(x, y, z);

		}

		internal static void PathFindToPacket(Assistant.Point3D Location)
		{
			ClientCommunication.SendToClientWait(new PathFindTo(Location));
		}

		internal static void PathFindToPacket(int x, int y, int z)
		{
			Assistant.Point3D loc = new Assistant.Point3D(x, y, z);
			PathFindToPacket(loc);
        }

		// Fly
		public static void Fly(bool on)
		{
			if (on)
			{
				if (!World.Player.Flying)
					ClientCommunication.SendToServerWait(new ToggleFly());
			}
			else
			{
				if (World.Player.Flying)
					ClientCommunication.SendToServerWait(new ToggleFly());
			}
		}

		// Message
		public static void HeadMessage(int hue, int num)
		{
			HeadMessage(hue, num.ToString());
		}

		public static void HeadMessage(int hue, string message)
		{
			ClientCommunication.SendToClientWait(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, hue, 3, Language.CliLocName, World.Player.Name, message));
		}

		// Paperdool button click
		public static void QuestButton()
		{
			ClientCommunication.SendToServerWait(new QuestButton(World.Player.Serial));
		}

		public static void GuildButton()
		{
			ClientCommunication.SendToServerWait(new GuildButton(World.Player.Serial));
		}

		// Range
		public static bool InRangeMobile(Mobile mob, int range)
		{
			return Utility.InRange(new Assistant.Point2D(World.Player.Position.X, World.Player.Position.Y), new Assistant.Point2D(mob.Position.X, mob.Position.Y), range);
		}

		public static bool InRangeMobile(int mobserial, int range)
		{
			Assistant.Mobile mob = World.FindMobile(mobserial);
			if (mob != null)
			{
				return Utility.InRange(new Assistant.Point2D(World.Player.Position.X, World.Player.Position.Y), new Assistant.Point2D(mob.Position.X, mob.Position.Y), range);
			}
			else
				return false;
		}

		public static bool InRangeItem(Item mob, int range)
		{
			return Utility.InRange(new Assistant.Point2D(World.Player.Position.X, World.Player.Position.Y), new Assistant.Point2D(mob.Position.X, mob.Position.Y), range);
		}

		public static bool InRangeItem(int itemserial, int range)
		{
			Assistant.Item item = World.FindItem(itemserial);
			if (item != null)
			{
				return Utility.InRange(new Assistant.Point2D(World.Player.Position.X, World.Player.Position.Y), new Assistant.Point2D(item.Position.X, item.Position.Y), range);
			}
			else
				return false;
		}

		// Weapon SA
		public static void WeaponPrimarySA()
		{
			SpecialMoves.SetPrimaryAbility();
		}

		public static void WeaponSecondarySA()
		{
			SpecialMoves.SetSecondaryAbility();
		}

		public static void WeaponClearSA()
		{
			SpecialMoves.ClearAbilities();
		}

		public static void WeaponDisarmSA()
		{
			SpecialMoves.OnDisarm();
		}

		public static void WeaponStunSA()
		{
			SpecialMoves.OnStun();
		}

		// Props 

		public static List<string> GetPropStringList()
		{
			List<Assistant.ObjectPropertyList.OPLEntry> props = World.Player.ObjPropList.Content;

			return props.Select(prop => prop.ToString()).ToList();
		}

		public static string GetPropStringByIndex(int index)
		{
			string propstring = String.Empty;

			List<Assistant.ObjectPropertyList.OPLEntry> props = World.Player.ObjPropList.Content;
			if (props.Count > index)
				propstring = props[index].ToString();

			return propstring;
		}

		public static int GetPropValue(string name)
		{
			List<Assistant.ObjectPropertyList.OPLEntry> props = World.Player.ObjPropList.Content;

			foreach (Assistant.ObjectPropertyList.OPLEntry prop in props)
			{
				if (!prop.ToString().ToLower().Contains(name.ToLower()))
					continue;

				if (prop.Args == null)  // Props esiste ma non ha valore
					return 1;

				try
				{
					return (Convert.ToInt32(Language.ParsePropsCliloc(prop.Args)));
				}
				catch
				{
					return 1;  // errore di conversione ma esiste
				}
			}
			return 0;  // Non esiste
		}
	}
}