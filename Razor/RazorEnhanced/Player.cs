using Assistant;
using Assistant.UI;
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


        public static void ToggleAlwaysRun()
        {
            if (Client.IsOSI){
                RazorEnhanced.UoWarper.UODLLHandleClass.ToggleAlwaysRun();
            }
            //TODO: check how to set "always run" on CUO

        }


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

        public static int StaticMount { get { return Filters.AutoRemountSerial; } }

        public static int Gold { get { return Convert.ToInt32(World.Player.Gold); } }
        public static int Luck { get { return World.Player.Luck; } }
        public static int Body { get { return World.Player.Body; } }
        public static int MobileID { get { return World.Player.Body; } }
        public static int Serial { get { return World.Player.Serial; } }

        // Follower
        public static int FollowersMax { get { return World.Player.FollowersMax; } }

        public static int Followers { get { return World.Player.Followers; } }

        // Weight
        public static int MaxWeight { get { return World.Player.MaxWeight; } }

        public static int Weight { get { return World.Player.Weight; } }

        // Party
        public bool InParty { get { return Assistant.PacketHandlers.Party.Contains(World.Player.Serial); } }

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

        public static int DistanceTo(Mobile m)
        {
            return Utility.Distance(Position.X, Position.Y, m.Position.X, m.Position.Y);
        }

        public static int DistanceTo(Item i)
        {
            return Utility.Distance(Position.X, Position.Y, i.Position.X, i.Position.Y);
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
                    description = "Lightning Strike";
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

                case BuffIcon.WraithForm:
                    description = "Wraith Form";
                    break;

                case BuffIcon.Rampage:
                    description = "Rampage";
                    break;

                case BuffIcon.Stagger:
                    description = "Stagger";
                    break;

                case BuffIcon.Toughness:
                    description = "Toughness";
                    break;

                case BuffIcon.Thrust:
                    description = "Thrust";
                    break;

                case BuffIcon.Pierce:
                    description = "Pierce";
                    break;

                case BuffIcon.PlayingTheOdds:
                    description = "Playing The Odds";
                    break;

                case BuffIcon.FocusedEye:
                    description = "Focused Eye";
                    break;

                case BuffIcon.Onslaught:
                    description = "Onslaught";
                    break;

                case BuffIcon.ElementalFury:
                    description = "Elemental Fury";
                    break;

                case BuffIcon.ElementalFuryDebuff:
                    description = "Elemental Fury Debuff";
                    break;

                case BuffIcon.CalledShot:
                    description = "Called Shot";
                    break;

                case BuffIcon.Knockout:
                    description = "Knockout";
                    break;

                case BuffIcon.SavingThrow:
                    description = "Saving Throw";
                    break;

                case BuffIcon.Conduit:
                    description = "Conduit";
                    break;

                case BuffIcon.EtherealBurst:
                    description = "Ethereal Burst";
                    break;

                case BuffIcon.MysticWeapon:
                    description = "Mystic Weapon";
                    break;

                case BuffIcon.ManaShield:
                    description = "Mana Shield";
                    break;

                case BuffIcon.AnticipateHit:
                    description = "Anticipate Hit";
                    break;

                case BuffIcon.Warcry:
                    description = "Warcry";
                    break;

                case BuffIcon.Shadow:
                    description = "Shadow";
                    break;

                case BuffIcon.WhiteTigerForm:
                    description = "White Tiger Form";
                    break;

                case BuffIcon.Bodyguard:
                    description = "Bodyguard";
                    break;

                case BuffIcon.HeightenedSenses:
                    description = "Heightened Senses";
                    break;

                case BuffIcon.Tolerance:
                    description = "Tolerance";
                    break;

                case BuffIcon.DeathRay:
                    description = "Death Ray";
                    break;

                case BuffIcon.DeathRayDebuff:
                    description = "Death Ray Debuff";
                    break;

                case BuffIcon.Intuition:
                    description = "Intuition";
                    break;

                case BuffIcon.EnchantedSummoning:
                    description = "Enchanted Summoning";
                    break;

                case BuffIcon.ShieldBash:
                    description = "Shield Bash";
                    break;

                case BuffIcon.Whispering:
                    description = "Whispering";
                    break;

                case BuffIcon.CombatTraining:
                    description = "Combat Training";
                    break;

                case BuffIcon.InjectedStrikeDebuff:
                    description = "Injected Strike Debuff";
                    break;

                case BuffIcon.InjectedStrike:
                    description = "Injected Strike";
                    break;

                case BuffIcon.UnknownTomato:
                    description = "Unknown Tomato";
                    break;

                case BuffIcon.PlayingTheOddsDebuff:
                    description = "Playing The Odds Debuff";
                    break;

                case BuffIcon.DragonTurtleDebuff:
                    description = "Dragon Turtle Debuff";
                    break;

                case BuffIcon.Boarding:
                    description = "Boarding";
                    break;

                case BuffIcon.Potency:
                    description = "Potency";
                    break;

                case BuffIcon.ThrustDebuff:
                    description = "Thrust Debuff";
                    break;

                case BuffIcon.FistsOfFury:
                    description = "Fists Of Fury";
                    break;

                case BuffIcon.BarrabHemolymphConcentrate:
                    description = "Barrab Hemolymph Concentrate";
                    break;

                case BuffIcon.JukariBurnPoiltice:
                    description = "Jukari Burn Poiltice";
                    break;

                case BuffIcon.KurakAmbushersEssence:
                    description = "Kurak Ambushers Essence";
                    break;

                case BuffIcon.BarakoDraftOfMight:
                    description = "Barako Draft Of Might";
                    break;

                case BuffIcon.UraliTranceTonic:
                    description = "Urali Trance Tonic,";
                    break;

                case BuffIcon.SakkhraProphylaxis:
                    description = "Sakkhra Prophylaxis";
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
            if (World.Player == null || World.Player.Buffs == null)
                return false;

            for (int i = 0; i < World.Player.Buffs.Count; i++)
            {
                if (GetBuffDescription(World.Player.Buffs[i]) == buffname)
                    return true;
            }

            return false;
        }

        // Special skill Icon
        public static bool SpellIsEnabled(string spell)
        {
            if (World.Player == null || World.Player.SkillEnabled == null)
                return false;

            if (!Enum.TryParse<SkillIcon>(spell.Replace(" ", ""), out SkillIcon l))
            {
                Scripts.SendMessageScriptError("Script Error: SpellEnable: Invalid spell name: " + spell);
                return false;
            }

            if (World.Player.SkillEnabled.Contains(l))
                return true;

            return false;
        }
        public static void UnEquipItemByLayer(String layer, bool wait = true)
        {
            if (!Enum.TryParse<Layer>(layer, out Layer l))
            {
                Scripts.SendMessageScriptError("Script Error: UnEquipItemByLayer: " + layer + " not valid");
                return;
            }

            Assistant.Item item = World.Player.GetItemOnLayer(l);

            if (item != null)
            {
                if (wait)
                {
                    Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
                    Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, World.Player.Backpack.Serial));
                }
                else
                {
                    Assistant.Client.Instance.SendToServer(new LiftRequest(item.Serial, item.Amount));
                    Assistant.Client.Instance.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, World.Player.Backpack.Serial));
                }
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
            Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount)); // Prende
            Assistant.Client.Instance.SendToServerWait(new EquipRequest(item.Serial, World.Player.Serial, item.Layer)); // Equippa
        }

        public static void EquipItem(Item item)
        {
            Assistant.Mobile player = World.Player;
            if (item.Container == 0 && Misc.DistanceSqrt(item.GetWorldPosition(), Position) > 3)
            {
                Scripts.SendMessageScriptError("Script Error: EquipItem: Item serial: (" + item.Serial + ") too away");
                return;
            }
            Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount)); // Prende
            Assistant.Client.Instance.SendToServerWait(new EquipRequest(item.Serial, World.Player.Serial, item.AssistantLayer)); // Equippa
        }

        public static void EquipUO3D(List<int> serials)
        {
            List<uint> serialstoequip = new List<uint>();
            foreach (int serial in serials)
                serialstoequip.Add((uint)serial);

            Assistant.Client.Instance.SendToServerWait(new EquipItemMacro(serialstoequip));

        }
        public static bool CheckLayer(String layer)
        {
            if (!Enum.TryParse<Layer>(layer, out Layer l))
            {
                Scripts.SendMessageScriptError("Script Error: CheckLayer: " + layer + " not valid");
                return false;
            }

            Assistant.Item item = World.Player.GetItemOnLayer(l);

            if (item != null)
                return true;
            else
                return false;
        }

        public static Item GetItemOnLayer(String layer)
        {
            if (!Enum.TryParse<Layer>(layer, out Layer l))
            {
                Scripts.SendMessageScriptError("Script Error: GetItemOnLayer: " + layer + " not valid");
                return null;
            }

            Assistant.Item assistantItem = null;
            if (l != Assistant.Layer.Invalid)
            {
                assistantItem = World.Player.GetItemOnLayer(l);
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
            if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetSkillValue: " + skillname + " not valid");
                return -1;
            }

            return World.Player.Skills[(int)skill].Value;
        }

        public static double GetRealSkillValue(string skillname)
        {
            if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetRealSkillValue: " + skillname + " not valid");
                return -1;
            }

            return World.Player.Skills[(int)skill].Base;
        }


        public static double GetSkillCap(string skillname)
        {
            if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetSkillCap: " + skillname + " not valid");
                return -1;
            }

            return World.Player.Skills[(int)skill].Cap;
        }

        public static int GetSkillStatus(string skillname)
        {
            if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetSkillStatus: " + skillname + " not valid");
                return -1;
            }

            return (int)World.Player.Skills[(int)skill].Lock;
        }

        public static void SetSkillStatus(string skillname, int status)
        {
            if (status < 0 || status > 2)
            {
                Scripts.SendMessageScriptError("Script Error: SetSkillStatus: status: " + status + " not valid");
                return;
            }

            if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: SetSkillStatus: " + skillname + " not valid");
                return;
            }

            LockType t = (LockType)status;

            Assistant.Client.Instance.SendToServer(new SetSkillLock(World.Player.Skills[(int)skill].Index, t));

            World.Player.Skills[(int)skill].Lock = t;
            Engine.MainWindow.SafeAction(s => s.UpdateSkill(World.Player.Skills[(int)skill]));

            Assistant.Client.Instance.SendToClient(new SkillUpdate(World.Player.Skills[(int)skill]));
        }

        public static int GetStatStatus(string statname)
        {
            if (!Enum.TryParse<StatName>(statname, out StatName stat))
            {
                Scripts.SendMessageScriptError("Script Error: GetStatStatus: " + statname + " not valid");
                return -1;
            }
            Scripts.SendMessageScriptError("Script Error: GetStatStatus: not implemented");
            return (int)-1;
        }

        public static void SetStatStatus(string statname, int status)
        {
            if (status < 0 || status > 2)
            {
                Scripts.SendMessageScriptError("Script Error: SetStatStatus: status: " + status + " not valid");
                return;
            }

            if (!Enum.TryParse<StatName>(statname, out StatName skill))
            {
                Scripts.SendMessageScriptError("Script Error: SetStatStatus: " + statname + " not valid");
                return;
            }

            LockType type = (LockType)status;

            Assistant.Client.Instance.SendToServer(new SetStatLock((int)skill, type));

            //World.Player.Skills[(int)skill].Lock = t;
            //Engine.MainWindow.SafeAction(s => s.UpdateSkill(World.Player.Skills[(int)skill]));

           // Assistant.Client.Instance.SendToClient(new SkillUpdate(World.Player.Skills[(int)skill]));
        }




        /// <summary>
        /// //////
        /// </summary>
        /// <param name="skillname"></param>
        /// <param name="target"></param>
        /// <param name="wait"></param>


        public static void UseSkill(string skillname, EnhancedEntity target, bool wait = true)
        {
            UseSkill(skillname, target.Serial, wait);
            return;
        }
        public static void UseSkill(string skillname, int targetSerial, bool wait=true)
        {
            if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: " + skillname + " not valid");
                return;
            }

            Assistant.Item itemTarget = Assistant.World.FindItem(targetSerial);
            Assistant.Mobile mobileTarget = Assistant.World.FindMobile(targetSerial);
            if (itemTarget == null && mobileTarget == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: Invalid Target");
                return;
            }

            if (itemTarget == null && !mobileTarget.Serial.IsMobile)
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: (" + targetSerial.ToString() + ") is not a mobile");
                return;
            }
            if (mobileTarget == null && !itemTarget.Serial.IsItem)
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: (" + targetSerial.ToString() + ") is not an item");
                return;
            }

            if (wait)
                Assistant.Client.Instance.SendToServerWait(new UseTargetedSkill((ushort)skill, (uint)targetSerial));
            else
                Assistant.Client.Instance.SendToServer(new UseTargetedSkill((ushort)skill, (uint)targetSerial));

            if (skill == SkillName.Hiding)
                StealthSteps.Hide();

            else if (skill == SkillName.Stealth)
            {
                if (!World.Player.Visible) // Trigger stealth step counter
                    StealthSteps.Hide();
            }

        }

        public static void UseSkill(string skillname)
        {
            UseSkillOnly(skillname, true);
        }
        public static void UseSkill(string skillname, bool wait)
        {
            UseSkillOnly(skillname, wait);
        }

        public static void UseSkillOnly(string skillname, bool wait)
		{
			if (!Enum.TryParse<SkillName>(skillname.Replace(" ", ""), out SkillName skill))
			{
				Scripts.SendMessageScriptError("Script Error: UseSkill: " + skillname + " not valid");
				return;
			}

			if (wait)
		 		Assistant.Client.Instance.SendToServerWait(new UseSkill((int)skill));
			else
		 		Assistant.Client.Instance.SendToServer(new UseSkill((int)skill));

			if (skill == SkillName.Hiding)
				StealthSteps.Hide();

			else if (skill == SkillName.Stealth)
			{
				if (!World.Player.Visible) // Trigger stealth step counter
					StealthSteps.Hide();
			}
		}
		// Map Message
		public static void MapSay(int num)
		{
			MapSay(num.ToString());
		}

		public static void MapSay(string msg)
		{
			if (msg != null && msg != string.Empty)
		 		Assistant.UOAssist.PostTextSend(msg);
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
		 		Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Regular, hue, 3, Language.CliLocName, kw, msg));
			}
			else
			{
		 		Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Encoded, hue, 3, Language.CliLocName, kw, msg));
			}
		}
		public static void ChatGuild(int num)
		{
			ChatGuild(num.ToString());
		}

		public static void ChatGuild(string msg)
		{
			if (Assistant.Client.Instance.ServerEncrypted) // is OSI
		 		Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Guild, 1, 1, "ENU", new List<ushort>(), msg));
			else
		 		Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Guild, 1, 1, msg));
		}

		public static void ChatAlliance(int num)
		{
			ChatAlliance(num.ToString());
		}

		public static void ChatAlliance(string msg)
		{
			if (Assistant.Client.Instance.ServerEncrypted) // is OSI
		 		Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Alliance, 1, 1, "ENU", new List<ushort>(), msg));
			else
		 		Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Alliance, 1, 1, msg));
		}

		public static void ChatEmote(int hue, int num)
		{
			ChatEmote(hue, num.ToString());
		}

		public static void ChatEmote(int hue, string msg)
		{
	 		Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Emote, hue, 1, msg));
		}

		public static void ChatWhisper(int hue, int num)
		{
			ChatWhisper(hue, num.ToString());
		}

		public static void ChatWhisper(int hue, string msg)
		{
	 		Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Whisper, hue, 1, msg));
		}

		public static void ChatYell(int hue, int num)
		{
			ChatYell(hue, num.ToString());
		}

		public static void ChatYell(int hue, string msg)
		{
	 		Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Yell, hue, 1, msg));
		}

		public static void ChatChannel(int num)
		{
			ChatChannel(num.ToString());
		}

		public static void ChatChannel(string msg)
		{
	 		Assistant.Client.Instance.SendToServerWait(new ChatAction(0x61, Language.CliLocName, msg));
		}

		// attack
		public static void SetWarMode(bool warflag)
		{
	 		Assistant.Client.Instance.SendToServerWait(new SetWarMode(warflag));
		}

		public static void Attack(Mobile m)
		{
			Attack(m.Serial);
		}

		public static void Attack(int serial)
		{
            // make sure its either an item or a mobile, else server will disconnect you
			if ((World.FindMobile(serial) == null) && (World.FindItem(serial) == null))
                return;

			Target.AttackMessage(serial, true);
			if (Targeting.LastAttack != serial)
			{
		 		Assistant.Client.Instance.SendToClientWait(new ChangeCombatant(serial));
				Targeting.LastAttack = (uint)serial;
			}
	 		Assistant.Client.Instance.SendToServerWait(new AttackReq(serial));
        }


        public static void AttackLast()
		{
			if (Targeting.LastAttack == 0) // Nessun last attack presente
				return;

            if ((World.FindMobile(Targeting.LastAttack) == null) && (World.FindItem(Targeting.LastAttack) == null))
                return;

			Target.AttackMessage((int)Targeting.LastAttack, true);

	 		Assistant.Client.Instance.SendToServerWait(new AttackReq(Targeting.LastAttack));
		}

		// Virtue
		public static void InvokeVirtue(string virtue)
		{
			if (!Enum.TryParse<Virtues>(virtue, out Virtues v))
			{
				Scripts.SendMessageScriptError("Script Error: InvokeVirtue: " + virtue + " not valid");
				return;
			}

	 		Assistant.Client.Instance.SendToServerWait(new InvokeVirtue((byte)v));
		}

		public static void ChatParty(string msg, int serial = 0)
		{
			if (serial != 0)
		 		Assistant.Client.Instance.SendToServerWait(new SendPartyMessagePrivate(serial, msg));
			else
		 		Assistant.Client.Instance.SendToServerWait(new SendPartyMessage(msg));
		}

		public static void PartyInvite()
		{
	 		Assistant.Client.Instance.SendToServerWait(new PartyInvite());
		}
        
        public static void PartyAccept(int serial = 0)
        {
            Assistant.Client.Instance.SendToServerWait(new AcceptParty(serial));
        }

		public static void LeaveParty()
		{
	 		Assistant.Client.Instance.SendToServerWait(new PartyRemoveMember(World.Player.Serial));
		}

		public static void KickMember(int serial)
		{
			uint userial = Convert.ToUInt16(serial);
	 		Assistant.Client.Instance.SendToServerWait(new PartyRemoveMember(userial));
		}

		public static void PartyCanLoot(bool CanLoot)
		{
			if (CanLoot)
			 		Assistant.Client.Instance.SendToServerWait(new PartyCanLoot(0x1));
				else
			 		Assistant.Client.Instance.SendToServerWait(new PartyCanLoot(0x0));
		}

		// Moving
		public static bool Walk(string direction, bool checkPosition = true)  // Return true se walk ok false se rifiutato da server
		{
			return Run(direction, checkPosition);
		}

		private static DateTime m_LastWalk = DateTime.MinValue;
		public static bool Run(string direction, bool checkPosition = true)    // Return true se walk ok false se rifiutato da server
		{
			if (!Enum.TryParse<Direction>(direction, out Direction dir))
			{
				Scripts.SendMessageScriptError("Script Error: Run: " + direction + " not valid");
				return false;
			}

            if (checkPosition && !Client.IsOSI)
            {
                TimeSpan t = DateTime.UtcNow - m_LastWalk;
                const double MaxSpeed = 0.2;
                if (t < TimeSpan.FromSeconds(MaxSpeed))
                {
                    TimeSpan wait = TimeSpan.FromSeconds(MaxSpeed) - t;
                    Thread.Sleep(wait);
                }
            }
			World.Player.WalkScriptRequest = 1;
			int timeout = 0;
			Client.Instance.RequestMove(dir);
			m_LastWalk = DateTime.UtcNow;
			// Waits until a move event is seen happenning
			Console.WriteLine("Move {0} Sent", direction);
            if (checkPosition && Client.IsOSI)
            {
                while (World.Player.WalkScriptRequest < 2)
                {
                	Thread.Sleep(10);
                	timeout += 20;
                	Console.WriteLine("Move Waiting {0} - {1}", timeout, direction);
                	if (timeout > 1000) //  Handle slower ping times
                	{
                		Console.WriteLine("Move Timeout {0}", direction);
                		break;
                	}
                }
                Console.WriteLine("Move {0} Complete {1}", direction, World.Player.WalkScriptRequest);
                if (World.Player.WalkScriptRequest == 2)
                {
                    Console.WriteLine("Move Success {0}", direction);
                    World.Player.WalkScriptRequest = 0;
                    return true;
                }
                else
                {
                	World.Player.WalkScriptRequest = 0;
                	return false;
                }
            }

            return true;
		}

		public static void PathFindTo(Assistant.Point3D Location)
		{
			PathFindTo(Location.X, Location.Y, Location.Z);
		}

		public static void PathFindTo(int x, int y, int z)
		{
			PathFindToPacket(x, y, z);
			//UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();
			//while (!UoWarper.UODLLHandleClass.Open())
			//{
			//	Thread.Sleep(50);
			//}
			//UoWarper.UODLLHandleClass.Pathfind(x, y, z);
		}

		internal static void PathFindToPacket(Assistant.Point3D location)
		{
			//UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();
			//while (!UoWarper.UODLLHandleClass.Open())
			//{
			//	Thread.Sleep(50);
			//}
			//UoWarper.UODLLHandleClass.Pathfind(location.X, location.Y, location.Z);
			Assistant.Client.Instance.PathFindTo(location);
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
			 		Assistant.Client.Instance.SendToServerWait(new ToggleFly());
			}
			else
			{
				if (World.Player.Flying)
			 		Assistant.Client.Instance.SendToServerWait(new ToggleFly());
			}
		}

		// Message
		public static void HeadMessage(int hue, int num)
		{
			HeadMessage(hue, num.ToString());
		}

		public static void HeadMessage(int hue, string message)
		{
	 		Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, hue, 3, Language.CliLocName, World.Player.Name, message));
		}

		// Paperdoll button click
		public static void QuestButton()
		{
	 		Assistant.Client.Instance.SendToServerWait(new QuestButton(World.Player.Serial));
		}

		public static void GuildButton()
		{
	 		Assistant.Client.Instance.SendToServerWait(new GuildButton(World.Player.Serial));
		}

		// Range
		public static bool InRangeMobile(Mobile mob, int range)
		{
			if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, mob.Position.X, mob.Position.Y) <= range)
				return true;
			else
				return false;
		}

		public static bool InRangeMobile(int mobserial, int range)
		{
			Assistant.Mobile mob = World.FindMobile(mobserial);
			if (mob != null)
			{
				if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, mob.Position.X, mob.Position.Y) <= range)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public static bool InRangeItem(Item i, int range)
		{
			if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, i.Position.X, i.Position.Y) <= range)
				return true;
			else
				return false;
		}

		public static bool InRangeItem(int itemserial, int range)
		{
			Assistant.Item item = World.FindItem(itemserial);
			if (item != null)
			{
				if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) <= range)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		// Weapon SA
		public static void WeaponPrimarySA()
		{
			SpecialMoves.SetPrimaryAbility(true);
		}

		public static void WeaponSecondarySA()
		{
			SpecialMoves.SetSecondaryAbility(true);
		}

		public static void WeaponClearSA()
		{
			SpecialMoves.ClearAbilities(true);
		}

		public static void WeaponDisarmSA()
		{
			SpecialMoves.OnDisarm(true);
		}

		public static void WeaponStunSA()
		{
			SpecialMoves.OnStun(true);
		}

        // Props

        // Layer to scan
        private static List<Assistant.Layer> m_layer_props = new List<Layer>
        {
            Layer.RightHand,
            Layer.LeftHand,
            Layer.Shoes,
            Layer.Pants,
            Layer.Shirt,
            Layer.Head,
            Layer.Gloves,
            Layer.Ring,
            Layer.Talisman,
            Layer.Neck,
            Layer.Waist,
            Layer.InnerTorso,
            Layer.Bracelet,
            Layer.Unused_xF,
            Layer.MiddleTorso,
            Layer.Earrings,
            Layer.Arms,
            Layer.Cloak,
            Layer.Backpack,
            Layer.OuterTorso,
            Layer.OuterLegs,
            Layer.InnerLegs
        };

        public static float SumAttribute(string attributename)
        {
            if (World.Player == null)
                return 0;

            float attributevalue = 0;

            foreach (Layer l in m_layer_props)
            {
                Assistant.Item itemtocheck = World.Player.GetItemOnLayer(l);
                if (itemtocheck == null) // Slot vuoto
                    continue;

                if (!itemtocheck.PropsUpdated)
                    RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);

                attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
            }
            return attributevalue;
        }

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
