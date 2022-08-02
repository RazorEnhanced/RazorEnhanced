using Assistant;
using Assistant.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RazorEnhanced
{
    /// <summary>
    /// The Player class represent the currently logged in character.
    /// </summary>
    public class Player
    {
        internal static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // Stats
        /// <summary>
        /// Current hit points.
        /// </summary>
        public static int Hits { get { return World.Player.Hits; } }
        /// <summary>
        /// Maximum hit points.
        /// </summary>
        public static int HitsMax { get { return World.Player.HitsMax; } }
        /// <summary>
        /// Stats value for Strenght.
        /// </summary>
        public static int Str { get { return World.Player.Str; } }
        /// <summary>
        /// Current mana.
        /// </summary>
        public static int Mana { get { return World.Player.Mana; } }
        /// <summary>
        /// Maximum mana.
        /// </summary>
        public static int ManaMax { get { return World.Player.ManaMax; } }
        /// <summary>
        /// Stats value for Intelligence.
        /// </summary>
        public static int Int { get { return World.Player.Int; } }
        /// <summary>
        /// Current stamina.
        /// </summary>
        public static int Stam { get { return World.Player.Stam; } }
        /// <summary>
        /// Maximum stamina.
        /// </summary>
        public static int StamMax { get { return World.Player.StamMax; } }
        /// <summary>
        /// Stats value for Dexterity.
        /// </summary>
        public static int Dex { get { return World.Player.Dex; } }
        /// <summary>
        /// Get the stats cap.
        /// </summary>
        public static int StatCap { get { return World.Player.StatCap; } }


        //KR: Resistance and modifiers
        /// <summary>
        /// Resistance to Phisical damage.
        /// </summary>
        public static int AR { get { return World.Player.AR; } }
        /// <summary>
        /// Resistance to Fire damage.
        /// </summary>

        public static int FireResistance { get { return World.Player.FireResistance; } }
        /// <summary>
        /// Resistance to Cold damage.
        /// </summary>
        public static int ColdResistance { get { return World.Player.ColdResistance; } }
        /// <summary>
        /// Resistance to Energy damage.
        /// </summary>
        public static int EnergyResistance { get { return World.Player.EnergyResistance; } }
        /// <summary>
        /// Resistance to Poison damage.
        /// </summary>
        public static int PoisonResistance { get { return World.Player.PoisonResistance; } }

        // KR Attribute
        /// <summary>
        /// Get total Swing Speed Increase.
        /// </summary>
        public static int SwingSpeedIncrease { get { return World.Player.SwingSpeedIncrease; } }
        /// <summary>
        /// Get total Damage Chance Increase.
        /// </summary>
        public static int DamageChanceIncrease { get { return World.Player.DamageChanceIncrease; } }
        /// <summary>
        /// Get total Lower Reagent Cost.
        /// </summary>
        public static int LowerReagentCost { get { return World.Player.LowerReagentCost; } }
        /// <summary>
        /// Get total Hit Points Regeneration.
        /// </summary>
        public static int HitPointsRegeneration { get { return World.Player.HitPointsRegeneration; } }
        /// <summary>
        /// Get total Stamina Regeneration.
        /// </summary>
        public static int StaminaRegeneration { get { return World.Player.StaminaRegeneration; } }
        /// <summary>
        /// Get total Mana Regeneration.
        /// </summary>
        public static int ManaRegeneration { get { return World.Player.ManaRegeneration; } }
        /// <summary>
        /// Get total Reflect Physical Damage.
        /// </summary>
        public static int ReflectPhysicalDamage { get { return World.Player.ReflectPhysicalDamage; } }
        /// <summary>
        /// Get total Enhance Potions.
        /// </summary>
        public static int EnhancePotions { get { return World.Player.EnhancePotions; } }
        /// <summary>
        /// Get total Defense Chance Increase.
        /// </summary>
        public static int DefenseChanceIncrease { get { return World.Player.DefenseChanceIncrease; } }
        /// <summary>
        /// Get total Spell Damage Increase.
        /// </summary>
        public static int SpellDamageIncrease { get { return World.Player.SpellDamageIncrease; } }
        /// <summary>
        /// Get total Faster Cast Recovery.
        /// </summary>
        public static int FasterCastRecovery { get { return World.Player.FasterCastRecovery; } }
        /// <summary>
        /// Get total Faster Casting.
        /// </summary>
        public static int FasterCasting { get { return World.Player.FasterCasting; } }
        /// <summary>
        /// Get total Lower Mana Cost.
        /// </summary>
        public static int LowerManaCost { get { return World.Player.LowerManaCost; } }
        /// <summary>
        /// Get total Strength Increase.
        /// </summary>
        public static int StrengthIncrease { get { return World.Player.StrengthIncrease; } }
        /// <summary>
        /// Get total Dexterity Increase.
        /// </summary>
        public static int DexterityIncrease { get { return World.Player.DexterityIncrease; } }
        /// <summary>
        /// Get total Intelligence Increase.
        /// </summary>
        public static int IntelligenceIncrease { get { return World.Player.IntelligenceIncrease; } }
        /// <summary>
        /// Get total Hit Points Increase.
        /// </summary>
        public static int HitPointsIncrease { get { return World.Player.HitPointsIncrease; } }
        /// <summary>
        /// Get total Stamina Increase.
        /// </summary>
        public static int StaminaIncrease { get { return World.Player.StaminaIncrease; } }
        /// <summary>
        /// Get total Mana Increase.
        /// </summary>
        public static int ManaIncrease { get { return World.Player.ManaIncrease; } }
        /// <summary>
        /// Get total Maximum Hit Points Increase.
        /// </summary>
        public static int MaximumHitPointsIncrease { get { return World.Player.MaximumHitPointsIncrease; } }
        /// <summary>
        /// Get total Maximum Stamina Increase.
        /// </summary>
        public static int MaximumStaminaIncrease { get { return World.Player.MaximumStaminaIncrease; } }

        /// <summary>
        /// Display a fake tracking arrow
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="display">True = On, False = off</param>
        /// <param name="target">object serial targeted</param>
        public static void TrackingArrow(ushort x, ushort y, bool display, uint target=0)
        {
            if (target == 0)
                target = (uint)Player.Serial;
            Client.Instance.SendToClient(new TrackingArrow(target, display, x, y));
        }


        // Flags
        /// <summary>
        /// Player is a Ghost
        /// </summary>
        public static bool IsGhost { get { return World.Player.IsGhost; } }

        /// <summary>
        /// Player is Poisoned
        /// </summary>
        public static bool Poisoned { get { return World.Player.Poisoned; } }
        /// <summary>
        /// Player HP bar is not blue, but yellow.
        /// </summary>
        public static bool YellowHits { get { return World.Player.Blessed; } }
        /// <summary>
        /// Player is visible, false if hidden.
        /// </summary>
        public static bool Visible { get { return World.Player.Visible; } }
        /// <summary>
        /// Player has war mode active.
        /// </summary>
        public static bool WarMode { get { return World.Player.Warmode; } }
        /// <summary>
        /// Player is Paralized. True also while frozen because of casting of spells.
        /// </summary>
        public static bool Paralized { get { return World.Player.Paralized; } }

        /// <summary>
        /// Player have a special abilities active.
        /// </summary>
        public static bool HasSpecial { get { return World.Player.HasSpecial; } }
        public static bool HasPrimarySpecial { get { return SpecialMoves.HasPrimary; } }
        public static bool HasSecondarySpecial { get { return SpecialMoves.HasSecondary; } }
        public static uint PrimarySpecial { get { return SpecialMoves.PrimaryGumpId; } }
        public static uint SecondarySpecial { get { return SpecialMoves.SecondaryGumpId; } }


        // Self
        /// <summary>
        /// Player is a female.
        /// </summary>
        public static bool Female { get { return World.Player.Female; } }

        /// <summary>
        /// Player name.
        /// </summary>
        public static String Name { get { return World.Player.Name; } }
        /// <summary>
        /// Player notoriety
        ///     1: blue, innocent
        ///     2: green, friend
        ///     3: gray, neutral
        ///     4: gray, criminal
        ///     5: orange, enemy
        ///     6: red, hostile 
        ///     6: yellow, invulnerable
        /// </summary>
        public static byte Notoriety { get { return World.Player.Notoriety; } }

        /// <summary>
        /// Get the name of the area in which the Player is currently in. (Ex: Britain, Destard, Vesper, Moongate, etc)
        /// Regions are defined inside by Config/regions.json.
        /// </summary>
        /// <returns>Name of the area. Unknown if not recognized.</returns>
        public static string Area()
        {
            ConfigFiles.RegionByArea.Area area = Area(Player.Map, Player.Position.X, Player.Position.Y);
            if (area == null)
                return "Unknown";

            return area.areaName;
        }

        /// <summary>
        /// Get the type of zone in which the Player is currently in.
        /// Regions are defined inside by Config/regions.json.
        /// </summary>
        /// <returns>
        ///     Towns
        ///     Dungeons
        ///     Guarded
        ///     Forest
        ///     Unknown
        /// </returns>
        public static string Zone()
        {
            ConfigFiles.RegionByArea.Area area = Area(Player.Map, Player.Position.X, Player.Position.Y);
            if (area == null)
                return "Unknown";

            return area.zoneName;
        }

        internal static ConfigFiles.RegionByArea.Area Area(int map, int x, int y)
        {
            if (ConfigFiles.RegionByArea.AllFacets.ContainsKey(map))
            {
                ConfigFiles.RegionByArea.Facet facet = ConfigFiles.RegionByArea.AllFacets[Player.Map];
                foreach (ConfigFiles.RegionByArea.Area area in facet.areas)
                {
                    foreach (System.Drawing.Rectangle rect in area.rect)
                    {
                        if (rect.Contains((Int32)x, (Int32)y))
                            return area;
                    }
                }
            }
            return null;
        }




        /// <summary>
        /// Toggle on/off the awlays run flag. 
        /// NOTE: Works only on OSI client.
        /// </summary>
        public static void ToggleAlwaysRun()
        {
            if (Client.IsOSI)
            {
                RazorEnhanced.UoWarper.UODLLHandleClass.ToggleAlwaysRun();
            }
            //TODO: check how to set "always run" on CUO

        }


        /// <summary>
        /// Player backpack, as Item object.
        /// </summary>
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

        /// <summary>
        /// Player bank chest, as Item object.
        /// </summary>
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

        /// <summary>
        /// Player quiver, as Item object.
        /// </summary>
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

        /// <summary>
        /// Player current Mount, as Item object.
        /// NOTE: On some server the Serial return by this function doesn't match the mount serial.
        /// </summary>
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

        /// <summary>
        /// Retrieves serial of mount set in Filter/Mount GUI.
        /// </summary>
        public static int StaticMount { get { return Filters.AutoRemountSerial; } }

        /// <summary>
        /// Player total gold, in the backpack.
        /// </summary>
        public static int Gold { get { return Convert.ToInt32(World.Player.Gold); } }
        /// <summary>
        /// Player total luck.
        /// </summary>
        public static int Luck { get { return World.Player.Luck; } }
        /// <summary>
        /// Player Body or MobileID (see: Mobile.Body)
        /// </summary>
        public static int Body { get { return World.Player.Body; } }
        /// <summary>
        /// Player MobileID or Body (see: Mobile.MobileID)
        /// </summary>
        public static int MobileID { get { return World.Player.Body; } }

        /// <summary>
        /// Player unique Serial.
        /// </summary>
        public static int Serial { get { return World.Player.Serial; } }

        // Follower
        /// <summary>
        /// Player maximum amount of pet/followers.
        /// </summary>
        public static int FollowersMax { get { return World.Player.FollowersMax; } }
        /// <summary>
        /// Player current amount of pet/followers.
        /// </summary>
        public static int Followers { get { return World.Player.Followers; } }

        // Weight
        /// <summary>
        /// Player maximum weight.
        /// </summary>
        public static int MaxWeight { get { return World.Player.MaxWeight; } }
        /// <summary>
        /// Player current weight.
        /// </summary>
        public static int Weight { get { return World.Player.Weight; } }

        // Party
        /// <summary>
        /// Player is in praty.
        /// </summary>
        public bool InParty { get { return Assistant.PacketHandlers.Party.Contains(World.Player.Serial); } }

        // Position
        /// <summary>
        /// Current Player position as Point3D object. 
        /// </summary>
        public static Point3D Position { get { return new Point3D(World.Player.Position); } }

        /// <summary>
        /// Player current map, or facet.
        /// </summary>
        public static int Map { get { return World.Player.Map; } }

        /// <summary>
        /// Player current direction, as text.
        /// </summary>
        public static string Direction
        {
            get
            {
                switch (World.Player.Direction & Assistant.Direction.mask)
                {
                    case Assistant.Direction.north: return "North";
                    case Assistant.Direction.south: return "South";
                    case Assistant.Direction.west: return "West";
                    case Assistant.Direction.east: return "East";
                    case Assistant.Direction.right: return "Right";
                    case Assistant.Direction.left: return "Left";
                    case Assistant.Direction.down: return "Down";
                    case Assistant.Direction.up: return "Up";
                    default: return "Undefined";
                }
            }
        }

        /// <summary>
        /// Returns the distance between the Player and a Mobile or an Item.
        /// </summary>
        /// <param name="target">The other Mobile or Item</param>
        /// <returns>Distance in number of tiles.</returns>
        public static int DistanceTo(Mobile target)
        {
            return Utility.Distance(Position.X, Position.Y, target.Position.X, target.Position.Y);
        }

        public static int DistanceTo(Item target)
        {
            return Utility.Distance(Position.X, Position.Y, target.Position.X, target.Position.Y);
        }

        internal static Dictionary<BuffIcon, string> GetBuffsMapping()
        {

            Dictionary<BuffIcon, string> buffs = new Dictionary<BuffIcon, string>
            {
                [BuffIcon.ActiveMeditation] = "Meditation",
                [BuffIcon.Agility] = "Agility",
                [BuffIcon.ActiveMeditation] = "Meditation",
                [BuffIcon.Agility] = "Agility",
                [BuffIcon.AnimalForm] = "Animal Form",
                [BuffIcon.ArcaneEmpowerment] = "Arcane Enpowerment",
                [BuffIcon.ArcaneEmpowermentNew] = "Arcane Enpowerment (new)",
                [BuffIcon.ArchProtection] = "Arch Protection",
                [BuffIcon.ArmorPierce] = "Armor Pierce",
                [BuffIcon.AttuneWeapon] = "Attunement",
                [BuffIcon.AuraOfNausea] = "Aura of Nausea",
                [BuffIcon.Bleed] = "Bleed",
                [BuffIcon.Bless] = "Bless",
                [BuffIcon.Block] = "Block",
                [BuffIcon.BloodOathCaster] = "Bload Oath (caster)",
                [BuffIcon.BloodOathCurse] = "Bload Oath (curse)",
                [BuffIcon.BloodwormAnemia] = "BloodWorm Anemia",
                [BuffIcon.CityTradeDeal] = "City Trade Deal",
                [BuffIcon.Clumsy] = "Clumsy",
                [BuffIcon.Confidence] = "Confidence",
                [BuffIcon.CorpseSkin] = "Corpse Skin",
                [BuffIcon.CounterAttack] = "Counter Attack",
                [BuffIcon.CriminalStatus] = "Criminal",
                [BuffIcon.Cunning] = "Cunning",
                [BuffIcon.Curse] = "Curse",
                [BuffIcon.CurseWeapon] = "Curse Weapon",
                [BuffIcon.DeathStrike] = "Death Strike",
                [BuffIcon.DefenseMastery] = "Defense Mastery",
                [BuffIcon.Despair] = "Despair",
                [BuffIcon.Tribulation] = "Tribulation",
                [BuffIcon.DespairTarget] = "Despair (target)",
                [BuffIcon.TribulationTarget] = "TribulationTarget",
                [BuffIcon.DisarmNew] = "Disarm (new)",
                [BuffIcon.Disguised] = "Disguised",
                [BuffIcon.DismountPrevention] = "Dismount Prevention",
                [BuffIcon.DivineFury] = "Divine Fury",
                [BuffIcon.DragonSlasherFear] = "Dragon Slasher Fear",
                [BuffIcon.Enchant] = "Enchant",
                [BuffIcon.EnemyOfOne] = "Enemy Of One",
                [BuffIcon.EnemyOfOneNew] = "Enemy Of One (new)",
                [BuffIcon.EssenceOfWind] = "Essence Of Wind",
                [BuffIcon.EtherealVoyage] = "Ethereal Voyage",
                [BuffIcon.Evasion] = "Evasion",
                [BuffIcon.EvilOmen] = "Evil Omen",
                [BuffIcon.FactionLoss] = "Faction Loss",
                [BuffIcon.FanDancerFanFire] = "Fan Dancer Fan Fire",
                [BuffIcon.FeebleMind] = "Feeble Mind",
                [BuffIcon.Feint] = "Feint",
                [BuffIcon.ForceArrow] = "Force Arrow",
                [BuffIcon.GargoyleBerserk] = "Berserk",
                [BuffIcon.GargoyleFly] = "Fly",
                [BuffIcon.GazeDespair] = "Gaze Despair",
                [BuffIcon.GiftOfLife] = "Gift Of Life",
                [BuffIcon.GiftOfRenewal] = "Gift Of Renewal",
                [BuffIcon.HealingSkill] = "Healing",
                [BuffIcon.HeatOfBattleStatus] = "Heat Of Battle",
                [BuffIcon.HidingAndOrStealth] = "Hiding",
                [BuffIcon.HiryuPhysicalResistance] = "Hiryu Physical Malus",
                [BuffIcon.HitDualwield] = "Hit Dual Wield",
                [BuffIcon.HitLowerAttack] = "Hit Lower Attack",
                [BuffIcon.HitLowerDefense] = "Hit Lower Defense",
                [BuffIcon.HonorableExecution] = "Honorable Execution",
                [BuffIcon.Honored] = "Honored",
                [BuffIcon.HorrificBeast] = "Horrific Beast",
                [BuffIcon.HowlOfCacophony] = "Hawl Of Cacophony",
                [BuffIcon.ImmolatingWeapon] = "Immolating Weapon",
                [BuffIcon.Incognito] = "Incognito",
                [BuffIcon.Inspire] = "Inspire",
                [BuffIcon.Invigorate] = "Invigorate",
                [BuffIcon.Invisibility] = "Invisibility",
                [BuffIcon.LichForm] = "Lich Form",
                [BuffIcon.LightningStrike] = "Lightning Strike",
                [BuffIcon.MagicFish] = "Magic Fish",
                [BuffIcon.MagicReflection] = "Magic Reflection",
                [BuffIcon.ManaPhase] = "Mana Phase",
                [BuffIcon.MassCurse] = "Mass Curse",
                [BuffIcon.MedusaStone] = "Medusa Stone",
                [BuffIcon.Mindrot] = "Mind Rot",
                [BuffIcon.MomentumStrike] = "Momentum Strike",
                [BuffIcon.MortalStrike] = "Mortal Strike",
                [BuffIcon.NightSight] = "Night Sight",
                [BuffIcon.NoRearm] = "NoRearm",
                [BuffIcon.OrangePetals] = "Orange Petals",
                [BuffIcon.PainSpike] = "Pain Spike",
                [BuffIcon.Paralyze] = "Paralyze",
                [BuffIcon.Perfection] = "Perfection",
                [BuffIcon.Perseverance] = "Perseverance",
                [BuffIcon.Poison] = "Poison",
                [BuffIcon.PoisonResistanceImmunity] = "Poison Resistance",
                [BuffIcon.Polymorph] = "Polymorph",
                [BuffIcon.Protection] = "Protection",
                [BuffIcon.PsychicAttack] = "Psychic Attack",
                [BuffIcon.ConsecrateWeapon] = "Consecrate Weapon",
                [BuffIcon.Rage] = "Rage",
                [BuffIcon.RageFocusing] = "Rage Focusing",
                [BuffIcon.RageFocusingTarget] = "Rage Focusing (target)",
                [BuffIcon.ReactiveArmor] = "Reactive Armor",
                [BuffIcon.ReaperForm] = "Reaper Form",
                [BuffIcon.Resilience] = "Resilience",
                [BuffIcon.RoseOfTrinsic] = "Rose Of Trinsic",
                [BuffIcon.RotwormBloodDisease] = "Rotworm Blood Disease",
                [BuffIcon.RuneBeetleCorruption] = "Rune Beetle Corruption",
                [BuffIcon.SkillUseDelay] = "Skill Use Delay",
                [BuffIcon.Sleep] = "Sleep",
                [BuffIcon.SpellFocusing] = "Spell Focusing",
                [BuffIcon.SpellFocusingTarget] = "Spell Focusing (target)",
                [BuffIcon.SpellPlague] = "Spell Plague",
                [BuffIcon.SplinteringEffect] = "Splintering Effect",
                [BuffIcon.StoneForm] = "Stone Form",
                [BuffIcon.Strangle] = "Strangle",
                [BuffIcon.Strength] = "Strength",
                [BuffIcon.Surge] = "Surge",
                [BuffIcon.SwingSpeed] = "Swing Speed",
                [BuffIcon.TalonStrike] = "Talon Strike",
                [BuffIcon.Weaken] = "Weaken",
                [BuffIcon.VampiricEmbrace] = "Vampiric Embrace",
                [BuffIcon.WraithForm] = "Wraith Form",
                [BuffIcon.Rampage] = "Rampage",
                [BuffIcon.Stagger] = "Stagger",
                [BuffIcon.Toughness] = "Toughness",
                [BuffIcon.Thrust] = "Thrust",
                [BuffIcon.Pierce] = "Pierce",
                [BuffIcon.PlayingTheOdds] = "Playing The Odds",
                [BuffIcon.FocusedEye] = "Focused Eye",
                [BuffIcon.Onslaught] = "Onslaught",
                [BuffIcon.ElementalFury] = "Elemental Fury",
                [BuffIcon.ElementalFuryDebuff] = "Elemental Fury Debuff",
                [BuffIcon.CalledShot] = "Called Shot",
                [BuffIcon.Knockout] = "Knockout",
                [BuffIcon.SavingThrow] = "Saving Throw",
                [BuffIcon.Conduit] = "Conduit",
                [BuffIcon.EtherealBurst] = "Ethereal Burst",
                [BuffIcon.MysticWeapon] = "Mystic Weapon",
                [BuffIcon.ManaShield] = "Mana Shield",
                [BuffIcon.AnticipateHit] = "Anticipate Hit",
                [BuffIcon.Warcry] = "Warcry",
                [BuffIcon.Shadow] = "Shadow",
                [BuffIcon.WhiteTigerForm] = "White Tiger Form",
                [BuffIcon.Bodyguard] = "Bodyguard",
                [BuffIcon.HeightenedSenses] = "Heightened Senses",
                [BuffIcon.Tolerance] = "Tolerance",
                [BuffIcon.DeathRay] = "Death Ray",
                [BuffIcon.DeathRayDebuff] = "Death Ray Debuff",
                [BuffIcon.Intuition] = "Intuition",
                [BuffIcon.EnchantedSummoning] = "Enchanted Summoning",
                [BuffIcon.ShieldBash] = "Shield Bash",
                [BuffIcon.Whispering] = "Whispering",
                [BuffIcon.CombatTraining] = "Combat Training",
                [BuffIcon.InjectedStrikeDebuff] = "Injected Strike Debuff",
                [BuffIcon.InjectedStrike] = "Injected Strike",
                [BuffIcon.UnknownTomato] = "Unknown Tomato",
                [BuffIcon.PlayingTheOddsDebuff] = "Playing The Odds Debuff",
                [BuffIcon.DragonTurtleDebuff] = "Dragon Turtle Debuff",
                [BuffIcon.Boarding] = "Boarding",
                [BuffIcon.Potency] = "Potency",
                [BuffIcon.ThrustDebuff] = "Thrust Debuff",
                [BuffIcon.FistsOfFury] = "Fists Of Fury",
                [BuffIcon.BarrabHemolymphConcentrate] = "Barrab Hemolymph Concentrate",
                [BuffIcon.JukariBurnPoiltice] = "Jukari Burn Poiltice",
                [BuffIcon.KurakAmbushersEssence] = "Kurak Ambushers Essence",
                [BuffIcon.BarakoDraftOfMight] = "Barako Draft Of Might",
                [BuffIcon.UraliTranceTonic] = "Urali Trance Tonic,",
                [BuffIcon.SakkhraProphylaxis] = "Sakkhra Prophylaxis",
            };

            return buffs;
        }

        internal static Dictionary<BuffIcon, string> BuffsMapping = GetBuffsMapping();

        internal static string GetBuffDescription(BuffIcon icon)
        {
            string description = String.Empty;

            if (BuffsMapping.ContainsKey(icon))
            {
                description = BuffsMapping[icon];
            }

            return description;
        }




        // Buff
        /// <summary>
        /// List of Player active buffs:
        ///    Meditation
        ///    Agility
        ///    Animal Form
        ///    Arcane Enpowerment
        ///    Arcane Enpowerment (new)
        ///    Arch Protection
        ///    Armor Pierce
        ///    Attunement
        ///    Aura of Nausea
        ///    Bleed
        ///    Bless
        ///    Block
        ///    Bload Oath (caster)
        ///    Bload Oath (curse)
        ///    BloodWorm Anemia
        ///    City Trade Deal
        ///    Clumsy
        ///    Confidence
        ///    Corpse Skin
        ///    Counter Attack
        ///    Criminal
        ///    Cunning
        ///    Curse
        ///    Curse Weapon
        ///    Death Strike
        ///    Defense Mastery
        ///    Despair
        ///    Despair (target)
        ///    Disarm (new)
        ///    Disguised
        ///    Dismount Prevention
        ///    Divine Fury
        ///    Dragon Slasher Fear
        ///    Enchant
        ///    Enemy Of One
        ///    Enemy Of One (new)
        ///    Essence Of Wind
        ///    Ethereal Voyage
        ///    Evasion
        ///    Evil Omen
        ///    Faction Loss
        ///    Fan Dancer Fan Fire
        ///    Feeble Mind
        ///    Feint
        ///    Force Arrow
        ///    Berserk
        ///    Fly
        ///    Gaze Despair
        ///    Gift Of Life
        ///    Gift Of Renewal
        ///    Healing
        ///    Heat Of Battle
        ///    Hiding
        ///    Hiryu Physical Malus
        ///    Hit Dual Wield
        ///    Hit Lower Attack
        ///    Hit Lower Defense
        ///    Honorable Execution
        ///    Honored
        ///    Horrific Beast
        ///    Hawl Of Cacophony
        ///    Immolating Weapon
        ///    Incognito
        ///    Inspire
        ///    Invigorate
        ///    Invisibility
        ///    Lich Form
        ///    Lighting Strike
        ///    Magic Fish
        ///    Magic Reflection
        ///    Mana Phase
        ///    Mass Curse
        ///    Medusa Stone
        ///    Mind Rot
        ///    Momentum Strike
        ///    Mortal Strike
        ///    Night Sight
        ///    NoRearm
        ///    Orange Petals
        ///    Pain Spike
        ///    Paralyze
        ///    Perfection
        ///    Perseverance
        ///    Poison
        ///    Poison Resistance
        ///    Polymorph
        ///    Protection
        ///    Psychic Attack
        ///    Consecrate Weapon
        ///    Rage
        ///    Rage Focusing
        ///    Rage Focusing (target)
        ///    Reactive Armor
        ///    Reaper Form
        ///    Resilience
        ///    Rose Of Trinsic
        ///    Rotworm Blood Disease
        ///    Rune Beetle Corruption
        ///    Skill Use Delay
        ///    Sleep
        ///    Spell Focusing
        ///    Spell Focusing (target)
        ///    Spell Plague
        ///    Splintering Effect
        ///    Stone Form
        ///    Strangle
        ///    Strength
        ///    Surge
        ///    Swing Speed
        ///    Talon Strike
        ///    Vampiric Embrace
        ///    Weaken
        ///    Wraith Form
        /// </summary>
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

        /// <summary>
        /// Check if a buff is active, by buff name.
        /// </summary>
        /// <param name="buffname">
        ///    Meditation
        ///    Agility
        ///    Animal Form
        ///    Arcane Enpowerment
        ///    Arcane Enpowerment (new)
        ///    Arch Protection
        ///    Armor Pierce
        ///    Attunement
        ///    Aura of Nausea
        ///    Bleed
        ///    Bless
        ///    Block
        ///    Bload Oath (caster)
        ///    Bload Oath (curse)
        ///    BloodWorm Anemia
        ///    City Trade Deal
        ///    Clumsy
        ///    Confidence
        ///    Corpse Skin
        ///    Counter Attack
        ///    Criminal
        ///    Cunning
        ///    Curse
        ///    Curse Weapon
        ///    Death Strike
        ///    Defense Mastery
        ///    Despair
        ///    Despair (target)
        ///    Disarm (new)
        ///    Disguised
        ///    Dismount Prevention
        ///    Divine Fury
        ///    Dragon Slasher Fear
        ///    Enchant
        ///    Enemy Of One
        ///    Enemy Of One (new)
        ///    Essence Of Wind
        ///    Ethereal Voyage
        ///    Evasion
        ///    Evil Omen
        ///    Faction Loss
        ///    Fan Dancer Fan Fire
        ///    Feeble Mind
        ///    Feint
        ///    Force Arrow
        ///    Berserk
        ///    Fly
        ///    Gaze Despair
        ///    Gift Of Life
        ///    Gift Of Renewal
        ///    Healing
        ///    Heat Of Battle
        ///    Hiding
        ///    Hiryu Physical Malus
        ///    Hit Dual Wield
        ///    Hit Lower Attack
        ///    Hit Lower Defense
        ///    Honorable Execution
        ///    Honored
        ///    Horrific Beast
        ///    Hawl Of Cacophony
        ///    Immolating Weapon
        ///    Incognito
        ///    Inspire
        ///    Invigorate
        ///    Invisibility
        ///    Lich Form
        ///    Lighting Strike
        ///    Magic Fish
        ///    Magic Reflection
        ///    Mana Phase
        ///    Mass Curse
        ///    Medusa Stone
        ///    Mind Rot
        ///    Momentum Strike
        ///    Mortal Strike
        ///    Night Sight
        ///    NoRearm
        ///    Orange Petals
        ///    Pain Spike
        ///    Paralyze
        ///    Perfection
        ///    Perseverance
        ///    Poison
        ///    Poison Resistance
        ///    Polymorph
        ///    Protection
        ///    Psychic Attack
        ///    Consecrate Weapon
        ///    Rage
        ///    Rage Focusing
        ///    Rage Focusing (target)
        ///    Reactive Armor
        ///    Reaper Form
        ///    Resilience
        ///    Rose Of Trinsic
        ///    Rotworm Blood Disease
        ///    Rune Beetle Corruption
        ///    Skill Use Delay
        ///    Sleep
        ///    Spell Focusing
        ///    Spell Focusing (target)
        ///    Spell Plague
        ///    Splintering Effect
        ///    Stone Form
        ///    Strangle
        ///    Strength
        ///    Surge
        ///    Swing Speed
        ///    Talon Strike
        ///    Vampiric Embrace
        ///    Weaken
        ///    Wraith Form
        /// </param>
        /// <returns>True: if the buff is active - False: otherwise.</returns>
        public static bool BuffsExist(string buffname)
        {
            if (World.Player == null || World.Player.Buffs == null)
                return false;

            // if exact match use it
            for (int i = 0; i < World.Player.Buffs.Count; i++)
            {
                if (GetBuffDescription(World.Player.Buffs[i]) == buffname)
                    return true;
            }

            // try to guess correct spelling
            string useBuffname = GuessBuffName(buffname);
            for (int i = 0; i < World.Player.Buffs.Count; i++)
            {
                if (GetBuffDescription(World.Player.Buffs[i]) == useBuffname)
                    return true;
            }

            return false;
        }

        // Special skill Icon
        /// <summary>
        /// Check if spell is active using the spell name (for spells that have this function).
        /// </summary>
        /// <param name="spellname">Name of the spell.</param>
        /// <returns>True: the spell is enabled - False: otherwise,.</returns>
        public static bool SpellIsEnabled(string spellname)
        {
            if (World.Player == null || World.Player.SkillEnabled == null)
                return false;

            if (!Enum.TryParse<SkillIcon>(spellname.Replace(" ", ""), out SkillIcon l))
            {
                Scripts.SendMessageScriptError("Script Error: SpellEnable: Invalid spell name: " + spellname);
                return false;
            }

            if (World.Player.SkillEnabled.Contains(l))
                return true;

            return false;
        }


        /// <summary>
        /// Unequip the Item associated with a specific Layer.
        /// </summary>
        /// <param name="layer">
        /// Layers:
        ///    RightHand
        ///    LeftHand
        ///    Shoes
        ///    Pants
        ///    Shirt
        ///    Head
        ///    Gloves
        ///    Ring
        ///    Neck
        ///    Hair
        ///    Waist
        ///    InnerTorso
        ///    Bracelet
        ///    FacialHair
        ///    MiddleTorso
        ///    Earrings
        ///    Arms
        ///    Cloak
        ///    OuterTorso
        ///    OuterLegs
        ///    InnerLegs
        ///    Talisman
        /// </param>
        /// <param name="wait">Wait for confirmation from the server.</param>
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

        /// <summary>
        /// Equip an Item
        /// </summary>
        /// <param name="serial">Serial or Item to equip.</param>
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
        /// <summary>
        /// Equip a list of item by using UO3D packet.
        /// </summary>
        /// <param name="serials">List of Serials of Item to equip.</param>
        public static void EquipUO3D(List<int> serials)
        {
            List<uint> serialstoequip = new List<uint>();
            foreach (int serial in serials)
                serialstoequip.Add((uint)serial);

            Assistant.Client.Instance.SendToServerWait(new EquipItemMacro(serialstoequip));

        }

        /// <summary>
        /// Check if a Layer is equipped by the Item.
        /// 
        /// </summary>
        /// <param name="layer">
        /// Layers:
        ///    RightHand
        ///    LeftHand
        ///    Shoes
        ///    Pants
        ///    Shirt
        ///    Head
        ///    Gloves
        ///    Ring
        ///    Neck
        ///    Hair
        ///    Waist
        ///    InnerTorso
        ///    Bracelet
        ///    FacialHair
        ///    MiddleTorso
        ///    Earrings
        ///    Arms
        ///    Cloak
        ///    OuterTorso
        ///    OuterLegs
        ///    InnerLegs
        ///    Talisman
        /// </param>
        /// <returns>True: the Layer is occupied by an Item - False: otherwise.</returns>
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

        /// <summary>
        /// Returns the Item associated with a Mobile Layer.
        /// </summary>
        /// <param name="layer">
        /// Layers:
        ///    RightHand
        ///    LeftHand
        ///    Shoes
        ///    Pants
        ///    Shirt
        ///    Head
        ///    Gloves
        ///    Ring
        ///    Neck
        ///    Hair
        ///    Waist
        ///    InnerTorso
        ///    Bracelet
        ///    FacialHair
        ///    MiddleTorso
        ///    Earrings
        ///    Arms
        ///    Cloak
        ///    OuterTorso
        ///    OuterLegs
        ///    InnerLegs
        ///    Talisman
        /// </param>
        /// <returns>Item for the layer. Return null if not found or Layer invalid.</returns>
        public static Item GetItemOnLayer(String layer)
        {
            if (!Enum.TryParse<Layer>(layer, out Layer l))
            {
                Scripts.SendMessageScriptError("Script Error: GetItemOnLayer: " + layer + " not valid");
                return null;
            }

            if (l != Assistant.Layer.Invalid)
            {
                Assistant.Item assistantItem = World.Player.GetItemOnLayer(l);
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

        internal static string GuessSkillName(string originalName)
        {
            int distance = 99;
            string closest = "";

            foreach (string skill in Enum.GetNames(typeof(SkillName)))
            {
                int computeDistance = UOAssist.LevenshteinDistance(skill, originalName);
                if (computeDistance < distance)
                {
                    distance = computeDistance;
                    closest = skill;
                }
            }

            if (distance < 99)
                return closest;
            return originalName;

        }
        internal static string GuessBuffName(string originalName)
        {
            int distance = 99;
            string closest = "";
            List<string> buffsList = BuffsMapping.Values.ToList();
            foreach (string buff in buffsList)
            {
                int computeDistance = UOAssist.LevenshteinDistance(buff, originalName);
                if (computeDistance < distance)
                {
                    distance = computeDistance;
                    closest = buff;
                }
            }

            if (distance < 99)
                return closest;
            return originalName;

        }
        // Skill
        /// <summary>
        /// Get the value of the skill, with modifiers, for the given the skill name.
        /// </summary>
        /// <param name="skillname">
        ///    Alchemy
        ///    Anatomy
        ///    Animal Lore
        ///    Item ID
        ///    Arms Lore
        ///    Parry
        ///    Begging
        ///    Blacksmith
        ///    Fletching
        ///    Peacemaking
        ///    Camping
        ///    Carpentry
        ///    Cartography
        ///    Cooking
        ///    Detect Hidden
        ///    Discordance
        ///    EvalInt
        ///    Healing
        ///    Fishing
        ///    Forensics
        ///    Herding
        ///    Hiding
        ///    Provocation
        ///    Inscribe
        ///    Lockpicking
        ///    Magery
        ///    Magic Resist
        ///    Mysticism
        ///    Tactics
        ///    Snooping
        ///    Musicianship
        ///    Poisoning
        ///    Archery
        ///    Spirit Speak
        ///    Stealing
        ///    Tailoring
        ///    Animal Taming
        ///    Taste ID
        ///    Tinkering
        ///    Tracking
        ///    Veterinary
        ///    Swords
        ///    Macing
        ///    Fencing
        ///    Wrestling
        ///    Lumberjacking
        ///    Mining
        ///    Meditation
        ///    Stealth
        ///    Remove Trap
        ///    Necromancy
        ///    Focus
        ///    Chivalry
        ///    Bushido
        ///    Ninjitsu
        ///    Spell Weaving
        ///    Imbuing
        /// </param>
        /// <returns>Value of the skill.</returns>
        public static double GetSkillValue(string skillname)
        {
            string guessedSkillName = GuessSkillName(skillname);
            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetSkillValue: " + skillname + " not valid");
                return -1;
            }

            return World.Player.Skills[(int)skill].Value;
        }

        /// <summary>
        /// Get the base/real value of the skill for the given the skill name.
        /// </summary>
        /// <param name="skillname">
        ///    Alchemy
        ///    Anatomy
        ///    Animal Lore
        ///    Item ID
        ///    Arms Lore
        ///    Parry
        ///    Begging
        ///    Blacksmith
        ///    Fletching
        ///    Peacemaking
        ///    Camping
        ///    Carpentry
        ///    Cartography
        ///    Cooking
        ///    Detect Hidden
        ///    Discordance
        ///    EvalInt
        ///    Healing
        ///    Fishing
        ///    Forensics
        ///    Herding
        ///    Hiding
        ///    Provocation
        ///    Inscribe
        ///    Lockpicking
        ///    Magery
        ///    Magic Resist
        ///    Mysticism
        ///    Tactics
        ///    Snooping
        ///    Musicianship
        ///    Poisoning
        ///    Archery
        ///    Spirit Speak
        ///    Stealing
        ///    Tailoring
        ///    Animal Taming
        ///    Taste ID
        ///    Tinkering
        ///    Tracking
        ///    Veterinary
        ///    Swords
        ///    Macing
        ///    Fencing
        ///    Wrestling
        ///    Lumberjacking
        ///    Mining
        ///    Meditation
        ///    Stealth
        ///    Remove Trap
        ///    Necromancy
        ///    Focus
        ///    Chivalry
        ///    Bushido
        ///    Ninjitsu
        ///    Spell Weaving
        ///    Imbuing
        /// </param>
        /// <returns>Value of the skill.</returns>
        public static double GetRealSkillValue(string skillname)
        {
            string guessedSkillName = GuessSkillName(skillname);
            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetRealSkillValue: " + skillname + " not valid");
                return -1;
            }

            return World.Player.Skills[(int)skill].Base;
        }

        /// <summary>
        /// Get the skill cap for the given the skill name.
        /// </summary>
        /// <param name="skillname">
        ///    Alchemy
        ///    Anatomy
        ///    Animal Lore
        ///    Item ID
        ///    Arms Lore
        ///    Parry
        ///    Begging
        ///    Blacksmith
        ///    Fletching
        ///    Peacemaking
        ///    Camping
        ///    Carpentry
        ///    Cartography
        ///    Cooking
        ///    Detect Hidden
        ///    Discordance
        ///    EvalInt
        ///    Healing
        ///    Fishing
        ///    Forensics
        ///    Herding
        ///    Hiding
        ///    Provocation
        ///    Inscribe
        ///    Lockpicking
        ///    Magery
        ///    Magic Resist
        ///    Mysticism
        ///    Tactics
        ///    Snooping
        ///    Musicianship
        ///    Poisoning
        ///    Archery
        ///    Spirit Speak
        ///    Stealing
        ///    Tailoring
        ///    Animal Taming
        ///    Taste ID
        ///    Tinkering
        ///    Tracking
        ///    Veterinary
        ///    Swords
        ///    Macing
        ///    Fencing
        ///    Wrestling
        ///    Lumberjacking
        ///    Mining
        ///    Meditation
        ///    Stealth
        ///    Remove Trap
        ///    Necromancy
        ///    Focus
        ///    Chivalry
        ///    Bushido
        ///    Ninjitsu
        ///    Spell Weaving
        ///    Imbuing
        /// </param>
        /// <returns>Value of the skill cap.</returns>
        public static double GetSkillCap(string skillname)
        {
            string guessedSkillName = GuessSkillName(skillname);
            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetSkillCap: " + skillname + " not valid");
                return -1;
            }

            return World.Player.Skills[(int)skill].Cap;
        }

        /// <summary>
        /// Get lock status for a specific skill. 
        /// </summary>
        /// <param name="skillname">
        ///    Alchemy
        ///    Anatomy
        ///    Animal Lore
        ///    Item ID
        ///    Arms Lore
        ///    Parry
        ///    Begging
        ///    Blacksmith
        ///    Fletching
        ///    Peacemaking
        ///    Camping
        ///    Carpentry
        ///    Cartography
        ///    Cooking
        ///    Detect Hidden
        ///    Discordance
        ///    EvalInt
        ///    Healing
        ///    Fishing
        ///    Forensics
        ///    Herding
        ///    Hiding
        ///    Provocation
        ///    Inscribe
        ///    Lockpicking
        ///    Magery
        ///    Magic Resist
        ///    Mysticism
        ///    Tactics
        ///    Snooping
        ///    Musicianship
        ///    Poisoning
        ///    Archery
        ///    Spirit Speak
        ///    Stealing
        ///    Tailoring
        ///    Animal Taming
        ///    Taste ID
        ///    Tinkering
        ///    Tracking
        ///    Veterinary
        ///    Swords
        ///    Macing
        ///    Fencing
        ///    Wrestling
        ///    Lumberjacking
        ///    Mining
        ///    Meditation
        ///    Stealth
        ///    Remove Trap
        ///    Necromancy
        ///    Focus
        ///    Chivalry
        ///    Bushido
        ///    Ninjitsu
        ///    Spell Weaving
        ///    Imbuing
        /// </param>
        /// <returns>
        /// Lock status:
        ///      0: Up     
        ///      1: Down 
        ///      2: Locked 
        ///     -1: Error
        /// </returns>
        public static int GetSkillStatus(string skillname)
        {
            string guessedSkillName = GuessSkillName(skillname);
            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: GetSkillStatus: " + skillname + " not valid");
                return -1;
            }

            return (int)World.Player.Skills[(int)skill].Lock;
        }

        /// <summary>
        /// Set lock status for a specific skill. 
        /// </summary>
        /// <param name="skillname">
        ///    Alchemy
        ///    Anatomy
        ///    Animal Lore
        ///    Item ID
        ///    Arms Lore
        ///    Parry
        ///    Begging
        ///    Blacksmith
        ///    Fletching
        ///    Peacemaking
        ///    Camping
        ///    Carpentry
        ///    Cartography
        ///    Cooking
        ///    Detect Hidden
        ///    Discordance
        ///    EvalInt
        ///    Healing
        ///    Fishing
        ///    Forensics
        ///    Herding
        ///    Hiding
        ///    Provocation
        ///    Inscribe
        ///    Lockpicking
        ///    Magery
        ///    Magic Resist
        ///    Mysticism
        ///    Tactics
        ///    Snooping
        ///    Musicianship
        ///    Poisoning
        ///    Archery
        ///    Spirit Speak
        ///    Stealing
        ///    Tailoring
        ///    Animal Taming
        ///    Taste ID
        ///    Tinkering
        ///    Tracking
        ///    Veterinary
        ///    Swords
        ///    Macing
        ///    Fencing
        ///    Wrestling
        ///    Lumberjacking
        ///    Mining
        ///    Meditation
        ///    Stealth
        ///    Remove Trap
        ///    Necromancy
        ///    Focus
        ///    Chivalry
        ///    Bushido
        ///    Ninjitsu
        ///    Spell Weaving
        ///    Imbuing
        /// </param>
        /// <param name="status">
        /// Lock status:
        ///      0: Up     
        ///      1: Down 
        ///      2: Locked 
        /// </param>
        public static void SetSkillStatus(string skillname, int status)
        {
            string guessedSkillName = GuessSkillName(skillname);
            if (status < 0 || status > 2)
            {
                Scripts.SendMessageScriptError("Script Error: SetSkillStatus: status: " + status + " not valid");
                return;
            }

            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
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

        /// <summary> 
        /// Get lock status for a specific stats. 
        /// </summary>
        /// <param name="statname">
        ///     Strength
        ///     Dexterity
        ///     Intelligence
        /// </param>
        /// <returns>
        /// Lock status:
        ///      0: Up     
        ///      1: Down 
        ///      2: Locked 
        /// </returns>
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


        /// <summary>
        /// Set lock status for a specific skill. 
        /// </summary>
        /// <param name="statname">
        ///     Strength
        ///     Dexterity
        ///     Intelligence
        /// </param>
        /// <param name="status">
        /// Lock status:
        ///      0: Up     
        ///      1: Down 
        ///      2: Locked 
        /// </param>
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
        /// Use a specific skill, and optionally apply that skill to the target specified.
        /// </summary>
        /// <param name="skillname">
        ///    Alchemy
        ///    Anatomy
        ///    Animal Lore
        ///    Item ID
        ///    Arms Lore
        ///    Parry
        ///    Begging
        ///    Blacksmith
        ///    Fletching
        ///    Peacemaking
        ///    Camping
        ///    Carpentry
        ///    Cartography
        ///    Cooking
        ///    Detect Hidden
        ///    Discordance
        ///    EvalInt
        ///    Healing
        ///    Fishing
        ///    Forensics
        ///    Herding
        ///    Hiding
        ///    Provocation
        ///    Inscribe
        ///    Lockpicking
        ///    Magery
        ///    Magic Resist
        ///    Mysticism
        ///    Tactics
        ///    Snooping
        ///    Musicianship
        ///    Poisoning
        ///    Archery
        ///    Spirit Speak
        ///    Stealing
        ///    Tailoring
        ///    Animal Taming
        ///    Taste ID
        ///    Tinkering
        ///    Tracking
        ///    Veterinary
        ///    Swords
        ///    Macing
        ///    Fencing
        ///    Wrestling
        ///    Lumberjacking
        ///    Mining
        ///    Meditation
        ///    Stealth
        ///    Remove Trap
        ///    Necromancy
        ///    Focus
        ///    Chivalry
        ///    Bushido
        ///    Ninjitsu
        ///    Spell Weaving
        ///    Imbuing
        /// </param>
        /// <param name="target">Optional: Serial, Mobile or Item to target. (default: null)</param>
        /// <param name="wait">Optional: True: wait for confirmation from the server (default: False)</param>
        public static void UseSkill(string skillname, int target, bool wait = true)
        {
            string guessedSkillName = GuessSkillName(skillname);
            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: " + skillname + " not valid");
                return;
            }

            Assistant.Item itemTarget = Assistant.World.FindItem(target);
            Assistant.Mobile mobileTarget = Assistant.World.FindMobile(target);
            if (itemTarget == null && mobileTarget == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: Invalid Target");
                return;
            }

            if (itemTarget == null && !mobileTarget.Serial.IsMobile)
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: (" + target.ToString() + ") is not a mobile");
                return;
            }
            if (mobileTarget == null && !itemTarget.Serial.IsItem)
            {
                Scripts.SendMessageScriptError("Script Error: UseSkill: (" + target.ToString() + ") is not an item");
                return;
            }

            if (wait)
                Assistant.Client.Instance.SendToServerWait(new UseTargetedSkill((ushort)skill, (uint)target));
            else
                Assistant.Client.Instance.SendToServer(new UseTargetedSkill((ushort)skill, (uint)target));

            if (skill == SkillName.Hiding)
                StealthSteps.Hide();

            else if (skill == SkillName.Stealth)
            {
                if (!World.Player.Visible) // Trigger stealth step counter
                    StealthSteps.Hide();
            }

        }

        public static void UseSkill(string skillname, Item target, bool wait = true)
        {
            UseSkill(skillname, target.Serial, wait);
            return;
        }

        public static void UseSkill(string skillname, Mobile target, bool wait = true)
        {
            UseSkill(skillname, target.Serial, wait);
            return;
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
            string guessedSkillName = GuessSkillName(skillname);
            if (!Enum.TryParse<SkillName>(guessedSkillName, out SkillName skill))
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

        /// <summary>
        /// Send message in the Map chat.
        /// </summary>
        /// <param name="msg">Message to send</param>
        public static void MapSay(string msg)
        {
            if (msg != null && msg != string.Empty)
                Assistant.UOAssist.PostTextSend(msg);
        }

        public static void MapSay(int msg)
        {
            MapSay(msg.ToString());
        }


        // Game Message
        /// <summary>
        /// Send message in game.
        /// </summary>
        /// <param name="color">Color of the text</param>
        /// <param name="msg">Message to send.</param>
        public static void ChatSay(int color, string msg)
        {
            List<ushort> kw = EncodedSpeech.GetKeywords(msg);
            if (kw.Count == 1 && kw[0] == 0)
            {
                Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Regular, color, 3, Language.CliLocName, kw, msg));
            }
            else
            {
                Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Encoded, color, 3, Language.CliLocName, kw, msg));
            }
        }

        public static void ChatSay(int color, int msg)
        {
            ChatSay(color, msg.ToString());
        }


        // Game Message
        /// <summary>
        /// Send message in game using 1153 for color.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public static void ChatSay(string msg)
        {
            ChatSay(1153, msg);
        }


        /// <summary>
        /// Send message to the guild chat.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public static void ChatGuild(string msg)
        {
            if (Assistant.Client.Instance.ServerEncrypted) // is OSI
                Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Guild, 1, 1, "ENU", new List<ushort>(), msg));
            else
                Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Guild, 1, 1, msg));
        }

        public static void ChatGuild(int msg)
        {
            ChatGuild(msg.ToString());
        }

        /// <summary>
        /// Send message to the alliace chat.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public static void ChatAlliance(string msg)
        {
            if (Assistant.Client.Instance.ServerEncrypted) // is OSI
                Assistant.Client.Instance.SendToServerWait(new ClientUniMessage(Assistant.MessageType.Alliance, 1, 1, "ENU", new List<ushort>(), msg));
            else
                Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Alliance, 1, 1, msg));

        }

        public static void ChatAlliance(int msg)
        {
            ChatAlliance(msg.ToString());
        }

        /// <summary>
        /// Send an emote in game.
        /// </summary>
        /// <param name="color">Color of the text</param>
        /// <param name="msg">Message to send.</param>
        public static void ChatEmote(int color, string msg)
        {
            Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Emote, color, 1, msg));
        }

        public static void EmoteAction(string action)
        {
            Assistant.Client.Instance.SendToServer(new EmoteAction(action));
        }

        public static void ChatEmote(int color, int msg)
        {
            ChatEmote(color, msg.ToString());
        }


        /// <summary>
        /// Send an wishper message.
        /// </summary>
        /// <param name="color">Color of the text</param>
        /// <param name="msg">Message to send.</param>
        public static void ChatWhisper(int color, string msg)
        {
            Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Whisper, color, 1, msg));
        }

        public static void ChatWhisper(int color, int msg)
        {
            ChatWhisper(color, msg.ToString());
        }


        /// <summary>
        /// Send an yell message.
        /// </summary>
        /// <param name="color">Color of the text</param>
        /// <param name="msg">Message to send.</param>
        public static void ChatYell(int color, string msg)
        {
            Assistant.Client.Instance.SendToServerWait(new ClientAsciiMessage(Assistant.MessageType.Yell, color, 1, msg));
        }
        public static void ChatYell(int color, int msg)
        {
            ChatYell(color, msg.ToString());
        }


        /// <summary>
        /// Send an chat channel message.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public static void ChatChannel(string msg)
        {
            Assistant.Client.Instance.SendToServerWait(new ChatAction(0x61, Language.CliLocName, msg));
        }

        public static void ChatChannel(int msg)
        {
            ChatChannel(msg.ToString());
        }



        /// <summary>
        /// Send message in arty chat. If a recepient_serial is specified, the message is private.
        /// </summary>
        /// <param name="msg">Text to send.</param>
        /// <param name="recepient_serial">Optional: Target of private message.</param>
        public static void ChatParty(string msg, int recepient_serial = 0)
        {
            if (recepient_serial != 0)
                Assistant.Client.Instance.SendToServerWait(new SendPartyMessagePrivate(recepient_serial, msg));
            else
                Assistant.Client.Instance.SendToServerWait(new SendPartyMessage(msg));
        }


        //Party

        /// <summary>
        /// Invite a person to a party. Prompt for a in-game Target.
        /// </summary>
        public static void PartyInvite()
        {
            Assistant.Client.Instance.SendToServerWait(new PartyInvite());
        }

        /// <summary>
        /// Accept an incoming party offer. In case of multiple party oebnding invitation, from_serial is specified, 
        /// </summary>
        /// <param name="from_serial">Optional: Serial to accept party from.( in case of multiple offers )</param>
        /// <param name="force">True: Accept the party invite even you are already in a party.</param>
        /// <returns>True: if you are now in a party - False: otherwise.</returns>
        public static bool PartyAccept(int from_serial = 0, bool force = false)
        {
            //Dalamar: about "force" option. On some shard "dobule party" is considered a feature as double the change of dropping artys
            if (!force && World.Player.InParty)
            {
                Misc.SendMessage("Player.PartyAccept: You are already in a party.");
                return true;
            }
            Assistant.Client.Instance.SendToServerWait(new AcceptParty(from_serial));
            return World.Player.InParty;
        }

        /// <summary>
        /// Leaves a party.
        /// </summary>
        /// <param name="force">True: Leave the party invite even you notin any party.</param>
        public static void LeaveParty(bool force = false)
        {
            if (!force && !World.Player.InParty)
            {
                Misc.SendMessage("Player.LeaveParty: You are not in a party.");
                return;
            }
            Assistant.Client.Instance.SendToServerWait(new PartyRemoveMember(World.Player.Serial));
        }

        /// <summary>
        /// Kick a member from party by serial. Only for party leader
        /// </summary>
        /// <param name="serial">Serial of the Mobile to remove.</param>
        public static void KickMember(int serial)
        {
            uint userial = Convert.ToUInt16(serial);
            Assistant.Client.Instance.SendToServerWait(new PartyRemoveMember(userial));
        }

        /// <summary>
        /// Set the Party loot permissions.
        /// </summary>
        /// <param name="CanLoot"></param>
        public static void PartyCanLoot(bool CanLoot)
        {
            if (CanLoot)
                Assistant.Client.Instance.SendToServerWait(new PartyCanLoot(0x1));
            else
                Assistant.Client.Instance.SendToServerWait(new PartyCanLoot(0x0));
        }


        // attack
        /// <summary>
        /// Set war Mode on on/off. 
        /// </summary>
        /// <param name="warflag">True: War - False: Peace</param>
        public static void SetWarMode(bool warflag)
        {
            Assistant.Client.Instance.SendToServerWait(new SetWarMode(warflag));
        }


        /// <summary>
        /// Attack a Mobile.
        /// </summary>
        /// <param name="serial">Serial or Mobile to attack.</param>
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

        public static void Attack(Mobile mobile)
        {
            Attack(mobile.Serial);
        }



        /// <summary>
        /// Attack last target.
        /// </summary>
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
        /// <summary>
        /// Invoke a virtue by name.
        /// </summary>
        /// <param name="virtue">
        ///    Honor
        ///    Sacrifice
        ///    Valor
        ///    Compassion
        ///    Honesty
        ///    Humility
        ///    Justice
        /// </param>
        public static void InvokeVirtue(string virtue)
        {
            if (!Enum.TryParse<Virtues>(virtue, out Virtues v))
            {
                Scripts.SendMessageScriptError("Script Error: InvokeVirtue: " + virtue + " not valid");
                return;
            }

            Assistant.Client.Instance.SendToServerWait(new InvokeVirtue((byte)v));
        }


        /// <summary>
        /// Run one step in the specified direction and wait for the confirmation of the new position by the server.
        /// If the character is not facing the direction, the first step only "turn" the Player in the required direction.
        /// 
        /// 
        /// Info:
        /// Walking:  5 tiles/sec (~200ms between each step)
        /// Running: 10 tiles/sec (~100ms between each step)
        /// </summary>
        /// <param name="direction">
        ///    North
        ///    South
        ///    East
        ///    West
        ///    Up
        ///    Down
        ///    Left
        ///    Right 
        /// </param>
        /// <returns>True: Destination reached - False: Coudn't reach the destination.</returns>
        public static bool Run(string direction)    // Return true if walk ok false if server refuse
        {
            return Move(direction, true);
        }
        public static bool Walk(string direction)    // Return true if walk ok false if server refuse
        {
            return Move(direction, false);
        }

        /// <summary>
        /// Run one step in the specified direction and wait for the confirmation of the new position by the server.
        /// If the character is not facing the direction, the first step only "turn" the Player in the required direction.
        /// 
        /// 
        /// Info:
        /// Walking:  5 tiles/sec (~200ms between each step)
        /// Running: 10 tiles/sec (~100ms between each step)
        /// </summary>
        /// <param name="direction">
        ///    North
        ///    South
        ///    East
        ///    West
        ///    Up
        ///    Down
        ///    Left
        ///    Right 
        /// </param>
        /// <param name="run">True: True - use run api, false use walk api</param>
        /// <returns>True: Destination reached - False: Coudn't reach the destination.</returns>
        internal static bool Move(string direction, bool run=true)    // Return true if walk ok false refused by the server
        {
            if (!Enum.TryParse<Direction>(direction.ToLower(), out Direction dir))
            {
                Scripts.SendMessageScriptError("Script Error: Run: " + direction + " not valid");
                return false;
            }

            World.Player.WalkScriptRequest = 1;
            int timeout = 0;
            bool result = false;
            if (run)
                result = Client.Instance.RequestRun(dir);
            else
                result = Client.Instance.RequestWalk(dir);

            Logger.Debug("Move {0} Sent", direction);

            return result;
        }

        /// <summary>
        /// Go to the given coordinates using Client-provided pathfinding.
        /// </summary>
        /// <param name="x">X map coordinates or Point3D</param>
        /// <param name="y">Y map coordinates</param>
        /// <param name="z">Z map coordinates</param>
        public static void PathFindTo(int x, int y, int z)
        {
            PathFindToPacket(x, y, z);
        }

        public static void PathFindTo(Assistant.Point3D Location)
        {
            PathFindTo(Location.X, Location.Y, Location.Z);
        }

        internal static void PathFindToPacket(Assistant.Point3D location)
        {
            Assistant.Client.Instance.PathFindTo(location);
        }

        internal static void PathFindToPacket(int x, int y, int z)
        {
            Assistant.Point3D loc = new Assistant.Point3D(x, y, z);
            PathFindToPacket(loc);
        }

        // Fly
        /// <summary>
        /// Enable or disable Gargoyle Flying.
        /// </summary>
        /// <param name="status">True: Gargoyle Fly ON - False: Gargoyle fly OFF</param>
        public static void Fly(bool status)
        {
            if (status)
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
        /// <summary>
        /// Display a message above the Player. Visible only by the Player.
        /// </summary>
        /// <param name="color">Color of the Text.</param>
        /// <param name="msg">Text of the message.</param>
        public static void HeadMessage(int color, string msg)
        {
            Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, color, 3, Language.CliLocName, World.Player.Name, msg));
        }

        public static void HeadMessage(int color, int msg)
        {
            HeadMessage(color, msg.ToString());
        }


        /// <summary>
        /// Open Player's Paperdoll
        /// </summary>
        // Open Paperdoll
        public static void OpenPaperDoll()
        {
            Assistant.Client.Instance.SendToServerWait(new DisplayPaperdoll(World.Player.Serial));
        } 

        /// <summary>
        /// Press the Quest menu button in the paperdoll.
        /// </summary>
        // Paperdoll button click
        public static void QuestButton()
        {
            Assistant.Client.Instance.SendToServerWait(new QuestButton(World.Player.Serial));
        }

        /// <summary>
        /// Press the Guild menu button in the paperdoll.
        /// </summary>
        public static void GuildButton()
        {
            Assistant.Client.Instance.SendToServerWait(new GuildButton(World.Player.Serial));
        }

        // Range
        /// <summary>
        /// Check if the Mobile is within a certain range (&lt;=).
        /// </summary>
        /// <param name="mobile">Serial or Mobile object.</param>
        /// <param name="range">Maximum distance in tiles.</param>
        /// <returns>True: Mobile is in range - False: otherwise.</returns>
        public static bool InRangeMobile(int mobile, int range)
        {
            Assistant.Mobile mob = World.FindMobile(mobile);
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

        public static bool InRangeMobile(Mobile mobile, int range)
        {
            if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, mobile.Position.X, mobile.Position.Y) <= range)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the Item is within a certain range (&lt;=).
        /// </summary>
        /// <param name="item">Serial or Item object.</param>
        /// <param name="range">Maximum distance in tiles.</param>
        /// <returns>True: Item is in range - False: otherwise.</returns>
        public static bool InRangeItem(int item, int range)
        {
            Assistant.Item itm = World.FindItem(item);
            if (itm != null)
            {
                if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, itm.Position.X, itm.Position.Y) <= range)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static bool InRangeItem(Item item, int range)
        {
            if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) <= range)
                return true;
            else
                return false;
        }


        // Equip Last Weapon
        /// <summary>
        /// Equip the last used weapon 
        /// </summary>
        public static void EquipLastWeapon()
        {
            Assistant.Client.Instance.SendToServer(new EquipLastWeapon());
        }

        // Weapon SA
        /// <summary>
        /// Toggle on/off the primary Special Ability of the weapon. 
        /// </summary>
        public static void WeaponPrimarySA()
        {
            SpecialMoves.SetPrimaryAbility(true);
        }
        /// <summary>
        /// Toggle on/off the secondary Special Ability of the weapon. 
        /// </summary>
        public static void WeaponSecondarySA()
        {
            SpecialMoves.SetSecondaryAbility(true);
        }

        /// <summary>
        /// Disable any active Special Ability of the weapon.
        /// </summary>
        public static void WeaponClearSA()
        {
            SpecialMoves.ClearAbilities(true);
        }

        /// <summary>
        /// Toggle Disarm Ability.
        /// </summary>
        public static void WeaponDisarmSA()
        {
            SpecialMoves.OnDisarm(true);
        }

        /// <summary>
        /// Toggle Stun Ability.
        /// </summary>
        public static void WeaponStunSA()
        {
            SpecialMoves.OnStun(true);
        }

        // Props

        // Layer to scan
        private static readonly List<Assistant.Layer> m_layer_props = new List<Layer>
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

        /// <summary>
        /// Scan all the equipped Item, returns the total value of a specific property. (ex: Lower Reagent Cost )
        /// NOTE: This function is slow.
        /// </summary>
        /// <param name="attributename">Name of the property.</param>
        /// <returns>The total value as number.</returns>
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

                attributevalue += RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
            }
            return attributevalue;
        }


        /// <summary>
        /// Get the list of Properties of the Player, as list of lines of the tooltip.
        /// </summary>
        /// <returns>List of text lines.</returns>
        public static List<string> GetPropStringList()
        {
            List<Assistant.ObjectPropertyList.OPLEntry> props = World.Player.ObjPropList.Content;

            return props.Select(prop => prop.ToString()).ToList();
        }


        /// <summary>
        /// Get a single line of Properties of the Player, from the tooltip, as text. 
        /// </summary>
        /// <param name="index">Line number, start from 0.</param>
        /// <returns>Single line of text.</returns>
        public static string GetPropStringByIndex(int index)
        {
            string propstring = String.Empty;

            List<Assistant.ObjectPropertyList.OPLEntry> props = World.Player.ObjPropList.Content;
            if (props.Count > index)
                propstring = props[index].ToString();

            return propstring;
        }


        /// <summary>
        /// Get the numeric value of a specific Player property, from the tooltip.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns>
        ///     n: value of the propery 
        ///     0: property not found.
        ///     1: property found, but not numeric.
        /// </returns>
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
