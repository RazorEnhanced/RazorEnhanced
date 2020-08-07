using Assistant;
using System.Collections.Generic;

namespace RazorEnhanced
{
    public class Spells
    {
        // spell
        public static void CastMagery(string SpellName)
        {
            CastOnlyMagery(SpellName, true);
        }
        public static void CastMagery(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_MagerySpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMagery: Invalid spell name: " + SpellName);
        }

        public static void CastOnlyMagery(string SpellName, bool wait)
        {
            bool success = CastOnlyGeneric(m_MagerySpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMagery: Invalid spell name: " + SpellName);
        }
        public static void CastMagery(string SpellName, Mobile m, bool wait = true)
        {
            CastMagery(SpellName, (uint)m.Serial, wait);
        }

        public static void CastNecro(string SpellName)
        {
            CastOnlyNecro(SpellName);
        }
        public static void CastNecro(string SpellName, Mobile m, bool wait = true)
        {
            CastNecro(SpellName, (uint)m.Serial, wait);
        }

        public static void CastNecro(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_NecroSpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
        }

        public static void CastOnlyNecro(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_NecroSpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
        }


        public static void CastChivalry(string SpellName)
        {
            CastOnlyChivalry(SpellName);
        }
        public static void CastChivalry(string SpellName, Mobile m, bool wait = true)
        {
            CastChivalry(SpellName, (uint)m.Serial, wait);
        }

        public static void CastChivalry(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_ChivalrySpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastChivalry: Invalid spell name: " + SpellName);
        }

        public static void CastOnlyChivalry(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_ChivalrySpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastChivalry: Invalid spell name: " + SpellName);
        }

        public static void CastBushido(string SpellName)
        {
            CastOnlyBushido(SpellName);
        }
        //public static void CastBushido(string SpellName, Mobile m, bool wait = true)
        //{
        //    CastBushido(SpellName, (uint)m.Serial, wait);
        //}

        //public static void CastBushido(string SpellName, uint target, bool wait = true)
        //{
        //    bool success = CastTargetedGeneric(m_BushidoSpellName, SpellName, target, wait);
        //    if (!success)
        //        Scripts.SendMessageScriptError("Script Error: CastBushido: Invalid spell name: " + SpellName);
        //}

        public static void CastOnlyBushido(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_BushidoSpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastBushido: Invalid spell name: " + SpellName);
        }

        public static void CastNinjitsu(string SpellName)
        {
            CastOnlyNinjitsu(SpellName);
        }
        public static void CastNinjitsu(string SpellName, Mobile m, bool wait = true)
        {
            CastNinjitsu(SpellName, (uint)m.Serial, wait);
        }

        public static void CastNinjitsu(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_NinjitsuSpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNinjitsu: Invalid spell name: " + SpellName);
        }

        public static void CastOnlyNinjitsu(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_NinjitsuSpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNinjitsu: Invalid spell name: " + SpellName);
        }

        public static void CastSpellweaving(string SpellName)
        {
            CastOnlySpellweaving(SpellName);
        }
        public static void CastSpellweaving(string SpellName, Mobile m, bool wait = true)
        {
            CastSpellweaving(SpellName, (uint)m.Serial, wait);
        }

        public static void CastSpellweaving(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_SpellweavingSpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastSpellweaving: Invalid spell name: " + SpellName);
        }

        public static void CastOnlySpellweaving(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_SpellweavingSpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastSpellweaving: Invalid spell name: " + SpellName);
        }

        public static void CastMysticism(string SpellName)
        {
            CastOnlyMysticism(SpellName);
        }
        public static void CastMysticism(string SpellName, Mobile m, bool wait = true)
        {
            CastMysticism(SpellName, (uint)m.Serial, wait);
        }

        public static void CastMysticism(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_MysticismSpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMysticism: Invalid spell name: " + SpellName);
        }

        public static void CastOnlyMysticism(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_MysticismSpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMysticism: Invalid spell name: " + SpellName);
        }

        public static void CastMastery(string SpellName)
        {
            CastOnlyMastery(SpellName);
        }
        public static void CastMastery(string SpellName, Mobile m, bool wait = true)
        {
            CastMastery(SpellName, (uint)m.Serial, wait);
        }

        public static void CastMastery(string SpellName, uint target, bool wait = true)
        {
            bool success = CastTargetedGeneric(m_MasterySpellName, SpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMastery: Invalid spell name: " + SpellName);
        }

        public static void CastOnlyMastery(string SpellName, bool wait = true)
        {
            bool success = CastOnlyGeneric(m_MasterySpellName, SpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMastery: Invalid spell name: " + SpellName);
        }
        internal static bool CastOnlyGeneric(Dictionary<string, int> conversion, string SpellName, bool wait)
        {
            if (World.Player == null)
                return true;

            Spell s = null;

            int id = 0;
            conversion.TryGetValue(SpellName, out id);

            if (id > 0)
                s = Spell.Get(id);
            else
                return false;

            if (s != null)
                s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
            return true;
        }

        internal static bool CastTargetedGeneric(Dictionary<string, int> conversion,
            string SpellName, uint target, bool wait)
        {
            if (World.Player == null)
                return true;

            Spell s = null;

            int id = 0;
            conversion.TryGetValue(SpellName, out id);

            if (id > 0)
                s = Spell.Get(id);
            else
                return false;

            if (s != null)
                s.OnCast(new CastTargetedSpell((ushort)s.GetID(), target), wait);
            return true;
        }

        public static void CastCleric(string SpellName)
        {
            CastOnlyCleric(SpellName);
        }
        public static void CastCleric(string SpellName, Mobile m, bool wait = true)
        {
            CastCleric(SpellName, (uint)m.Serial, wait);
        }

        public static void CastCleric(string SpellName, uint target, bool wait = true)
        {
            if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
            {

                bool success = CastTargetedGeneric(m_ClericSpellName, SpellName, target, wait);
                if (!success)
                    Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
            }
            else
            {
                CastOnlyCleric(SpellName, wait);
            }
        }

        public static void CastOnlyCleric(string SpellName, bool wait = true)
        {
            if (World.Player == null)
                return;

            string spell = null;
            m_ClericSpellNameText.TryGetValue(SpellName, out spell);

            if (spell == null)
                Scripts.SendMessageScriptError("Script Error: CastCleric: Invalid spell name: " + SpellName);
            else
            {
                if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
                {
                    bool success = CastOnlyGeneric(m_ClericSpellName, SpellName, wait);
                    if (!success)
                        Scripts.SendMessageScriptError("Script Error: CastCleric: Invalid spell name: " + SpellName);
                }
                else
                {
                    Player.ChatSay(5, spell);
                }
            }
        }
        // felix
        public static void CastDruid(string SpellName)
        {
            CastOnlyDruid(SpellName);
        }
        public static void CastDruid(string SpellName, Mobile m, bool wait = true)
        {
            CastDruid(SpellName, (uint)m.Serial, wait);
        }

        public static void CastDruid(string SpellName, uint target, bool wait = true)
        {
            if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
            {
                bool success = CastTargetedGeneric(m_DruidSpellName, SpellName, target, wait);
                if (!success)
                    Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
            }
            else
            {
                CastOnlyDruid(SpellName, wait);
            }
        }



        public static void CastOnlyDruid(string SpellName, bool wait = true)
        {
            if (World.Player == null)
                return;

            string spell = null;
            m_DruidSpellNameText.TryGetValue(SpellName, out spell);

            if (spell == null)
                Scripts.SendMessageScriptError("Script Error: CastDruid: Invalid spell name: " + SpellName);
            else
            {
                if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
                {
                    bool success = CastOnlyGeneric(m_DruidSpellName, SpellName, wait);
                    if (!success)
                        Scripts.SendMessageScriptError("Script Error: CasDruid: Invalid spell name: " + SpellName);
                }
                else
                {
                    Player.ChatSay(8, spell);
                }
            }
        }

        public static void Interrupt()
        {
            Assistant.Item item = FindUsedLayer();
            if (item != null)
            {
                Assistant.Point3D loc = Assistant.Point3D.MinusOne;
                Assistant.Client.Instance.SendToServerWait(new LiftRequest(item, 1));
                Assistant.Client.Instance.SendToServerWait(new EquipRequest(item.Serial, Assistant.World.Player, item.Layer)); // Equippa
            }
        }

        public static void CastLastSpell()
        {
            CastLastSpellInternal(true);
        }
        public static void CastLastSpellInternal(bool wait)
        {
            if (World.Player.LastSpell != 0)
            {
                Spell s = Spell.Get(World.Player.LastSpell);

                if (s != null)
                    s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
            }
        }

        public static void CastLastSpell(Mobile m, bool wait = true)
        {
            CastLastSpell((uint)m.Serial, wait);
        }

        public static void CastLastSpellLastTarget()
        {
            int lastTarget = Target.GetLast();
            Mobile m = Mobiles.FindBySerial(lastTarget);
            if (m != null)
            {
                CastLastSpell((uint)lastTarget, false);
            }
            else
            {
                Scripts.SendMessageScriptError("Last Target no longer exists");
            }
        }


        public static void CastLastSpell(uint target, bool wait = true)
        {
            if (World.Player.LastSpell != 0)
            {
                Spell s = Spell.Get(World.Player.LastSpell);

                if (s != null)
                    s.OnCast(new CastTargetedSpell((ushort)s.GetID(), target), wait);
            }
        }

        internal static Assistant.Item FindUsedLayer()
        {
            Assistant.Item layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Shoes);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Pants);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Shirt);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Head);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Gloves);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Ring);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Neck);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Waist);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.InnerTorso);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Bracelet);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.MiddleTorso);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Earrings);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Arms);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Cloak);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.OuterTorso);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.OuterLegs);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.InnerLegs);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
            if (layeritem != null)
                return layeritem;

            return null;
        }

        //////////////////////////////////////////////////////////////
        // Dizionari
        //////////////////////////////////////////////////////////////

        private static Dictionary<string, int> m_MagerySpellName = new Dictionary<string, int>
        {
			// Primo circolo magery
			{ "Clumsy", 1 },
            { "Create Food", 2 },
            { "Feeblemind", 3 },
            { "Heal", 4 },
            { "Magic Arrow", 5 },
            { "Night Sight", 6 },
            { "Reactive Armor", 7 },
            { "Weaken", 8 },

			// Secondo circolo magery
			{ "Agility", 9 },
            { "Cunning", 10 },
            { "Cure", 11 },
            { "Harm", 12 },
            { "Magic Trap", 13 },
            { "Magic Untrap", 14 },
            { "Protection", 15 },
            { "Strength", 16 },

			// Terzo circolo magery
			{ "Bless", 17 },
            { "Fireball", 18 },
            { "Magic Lock", 19 },
            { "Poison", 20 },
            { "Telekinesis", 21 },
            { "Teleport", 22 },
            { "Unlock", 23 },
            { "Wall of Stone", 24 },

			// Quarto circolo magery
			{ "Arch Cure", 25 },
            { "Arch Protection", 26 },
            { "Curse", 27 },
            { "Fire Field", 28 },
            { "Greater Heal", 29 },
            { "Lightning", 30 },
            { "Mana Drain", 31 },
            { "Recall", 32 },

			// Quinto circolo magery
			{ "Blade Spirits", 33 },
            { "Dispel Field", 34 },
            { "Incognito", 35 },
            { "Magic Reflection", 36 },
            { "Mind Blast", 37 },
            { "Paralyze", 38 },
            { "Poison Field", 39 },
            { "Summon Creature", 40 },

			// Sesto circolo magery
			{ "Dispel", 41 },
            { "Energy Bolt", 42 },
            { "Explosion", 43 },
            { "Invisibility", 44 },
            { "Mark", 45 },
            { "Mass Curse", 46 },
            { "Paralyze Field", 47 },
            { "Reveal", 48 },

			// Settimo circolo magery
			{ "Chain Lightning", 49 },
            { "Energy Field", 50 },
            { "Flamestrike", 51 },
            { "Gate Travel", 52 },
            { "Mana Vampire", 53 },
            { "Mass Dispel", 54 },
            { "Meteor Swarm", 55 },
            { "Polymorph", 56 },

			// Ottavo circolo magery
			{ "Earthquake", 57 },
            { "Energy Vortex", 58 },
            { "Resurrection", 59 },
            { "Summon Air Elemental", 60 },
            { "Air Elemental", 60 },
            { "Summon Daemon", 61 },
            { "Summon Earth Elemental", 62 },
            { "Earth Elemental", 62 },
            { "Summon Fire Elemental", 63 },
            { "Fire Elemental", 63 },
            { "Water Elemental", 64 },
            { "Summon Water Elemental", 64 },
            { "Ice Blast", 65 },
            { "Lightning Wave", 66 },
            { "Dust Storm", 67 },
            { "Mass Calm", 68 },
            { "Protean", 69},
            { "Poison Dart", 70},
            { "Void Matter", 71},
            { "Tidal Rush", 72},
            { "Crushing Boulder", 73},
            { "Noxious Fumes", 74},
        };

		private static Dictionary<string, int> m_NecroSpellName = new Dictionary<string, int>
		{
			{ "Animate Dead", 101 },
			{ "Blood Oath", 102 },
			{ "Corpse Skin", 103 },
			{ "Curse Weapon", 104 },
			{ "Evil Omen", 105 },
			{ "Horrific Beast", 106 },
			{ "Lich Form", 107 },
			{ "Mind Rot", 108 },
			{ "Pain Spike", 109 },
			{ "Poison Strike", 110 },
			{ "Strangle", 111 },
			{ "Summon Familiar", 112 },
			{ "Vampiric Embrace", 113 },
			{ "Vengeful Spirit", 114 },
			{ "Wither", 115 },
			{ "Wraith Form", 116 },
			{ "Exorcism", 117 }
		};

		private static Dictionary<string, int> m_ChivalrySpellName = new Dictionary<string, int>
		{
			{ "Cleanse By Fire", 201 },
			{ "Close Wounds", 202 },
			{ "Consecrate Weapon", 203 },
			{ "Dispel Evil", 204 },
			{ "Divine Fury", 205 },
			{ "Enemy Of One", 206 },
			{ "Holy Light", 207 },
			{ "Noble Sacrifice", 208 },
			{ "Remove Curse", 209 },
			{ "Sacred Journey", 210 }
		};

		private static Dictionary<string, int> m_BushidoSpellName = new Dictionary<string, int>
		{
			{ "Honorable Execution", 401 },
			{ "Confidence", 402 },
			{ "Evasion", 403 },
			{ "Counter Attack", 404 },
			{ "Lightning Strike", 405 },
			{ "Momentum Strike", 406 }
		};

		private static Dictionary<string, int> m_NinjitsuSpellName = new Dictionary<string, int>
		{
			{ "Focus Attack", 501 },
			{ "Death Strike", 502 },
			{ "Animal Form", 503 },
			{ "Ki Attack", 504 },
			{ "Surprise Attack", 505 },
			{ "Backstab", 506 },
			{ "Shadow jump", 507 },  // Keep old compaibility whit old script
			{ "Shadowjump", 507 },
			{ "Mirror Image", 508 },
		};

		private static Dictionary<string, int> m_SpellweavingSpellName = new Dictionary<string, int>
		{
			{ "Arcane Circle", 601 },
			{ "Gift Of Renewal", 602 },
			{ "Immolating Weapon", 603 },
			{ "Attunement", 604 },
			{ "Attune Weapon", 604 }, // Keep old compaibility whit old script
			{ "Thunderstorm", 605 },
			{ "Natures Fury", 606 },
			{ "Summon Fey", 607 },
			{ "Summon Fiend", 608 },
			{ "Reaper Form", 609 },
			{ "Wildfire", 610 },
			{ "Essence Of Wind", 611 },
			{ "Dryad Allure", 612 },
			{ "Ethereal Voyage", 613 },
			{ "Word Of Death", 614 },
			{ "Gift Of Life", 615 },
			{ "Arcane Empowerment", 616 }
		};

		private static Dictionary<string, int> m_MysticismSpellName = new Dictionary<string, int>
		{
			{ "Nether Bolt", 678 },
			{ "Healing Stone", 679 },
			{ "Purge Magic", 680 },
			{ "Enchant", 681 },
			{ "Sleep", 682 },
			{ "Eagle Strike", 683 },
			{ "Animated Weapon", 684 },
			{ "Stone Form", 685 },
			{ "Spell Trigger", 686 },
			{ "Mass Sleep", 687 },
			{ "Cleansing Winds", 688 },
			{ "Bombard", 689 },
			{ "Spell Plague", 690 },
			{ "Hail Storm", 691 },
			{ "Nether Cyclone", 692 },
			{ "Rising Colossus", 693 },
		};

		private static Dictionary<string, int> m_MasterySpellName = new Dictionary<string, int>
		{
			{ "Inspire", 701 },
			{ "Invigorate", 702 },
			{ "Resilience", 703 },
			{ "Perseverance", 704 },
			{ "Tribulation", 705 },
			{ "Despair", 706 },
			{ "Death Ray", 707 },
			{ "Ethereal Blast", 708 },
			{ "Nether Blast", 709 },
			{ "Mystic Weapon", 710 },
			{ "Command Undead", 711 },
			{ "Conduit", 712 },
			{ "Mana Shield", 713 },
			{ "Summon Reaper", 714 },
			{ "Enchanted Summoning", 715 },
			{ "Anticipate Hit", 716 },
			{ "Warcry", 717 },
			{ "Intuition", 718 },
			{ "Rejuvenate", 719 },
			{ "Holy Fist", 720 },
			{ "Shadow", 721 },
			{ "White Tiger Form", 722 },
			{ "Flaming Shot", 723 },
			{ "Playing The Odds", 724 },
			{ "Thrust", 725 },
			{ "Pierce", 726 },
			{ "Stagger", 727 },
			{ "Toughness", 728 },
			{ "Onslaught", 729 },
			{ "Focused Eye", 730 },
			{ "Elemental Fury", 731 },
			{ "Called Shot", 732 },
			{ "Saving Throw", 733 },
			{ "Shield Bash", 734 },
			{ "Bodyguard", 735 },
			{ "Heighten Senses", 736 },
			{ "Tolerance", 737 },
			{ "Injected Strike", 738 },
			{ "Potency", 739 },
			{ "Rampage", 740 },
			{ "Fists Of Fury", 741 },
			{ "Knockout", 742 },
			{ "Whispering", 743 },
			{ "Combat Training", 744 },
			{ "Boarding", 745 },
		};

        private static Dictionary<string, int> m_ClericSpellName = new Dictionary<string, int>
        {
            { "Angelic Faith", 342 },
            {"Banish Evil", 343 },
            { "Dampen Spirit", 344 },
            { "Divine Focus", 345 },
            { "Hammer of Faith", 346 },
            { "Purge", 347 },
            { "Restoration", 348 },
            { "Sacred Boon", 349 },
            { "Sacrifice", 350 },
            { "Smite", 351 },
            { "Touch of Life", 352 },
            { "Trial by Fire", 353 },
        };
        private static Dictionary<string, string> m_ClericSpellNameText = new Dictionary<string, string>
        {
            { "Angelic Faith", "[cs AngelicFaith" },
            {"Banish Evil", "[cs BanishEvil" },
            { "Dampen Spirit", "[cs DampenSpirit" },
            { "Divine Focus", "[cs DivineFocus" },
            { "Hammer of Faith", "[cs HammerofFaith" },
            { "Purge", "[cs Purge" },
            { "Restoration", "[cs Restoration" },
            { "Sacred Boon", "[cs SacredBoon" },
            { "Sacrifice", "[cs Sacrifice" },
            { "Smite", "[cs Smite" },
            { "Touch of Life", "[cs TouchofLife" },
            { "Trial by Fire", "[cs TrialbyFire" },
        };

        private static Dictionary<string, int> m_DruidSpellName = new Dictionary<string, int>
        {
            { "Shield of Earth", 302 },
            { "Hollow Reed", 303 },
            { "Pack of Beast", 304 },
            { "Spring of Life", 305 },
            { "Grasping Roots", 306 },
            { "Circle of Thorns", 307 },
            { "Swarm of Insects", 308 },
            { "Volcanic Eruption", 309 },
            { "Treefellow", 310 },
            { "Deadly Spores", 311 },
            { "Enchanted Grove", 312 },
            { "Lure Stone", 313 },
            { "Hurricane", 314 },
            { "Mushroom Gateway", 315 },
            { "Restorative Soil", 316 },
            { "FireFly", 317 },
            { "Forest Kin", 318 },
            { "BarkSkin", 319 },
            { "ManaSpring", 320 },
            { "Hibernate", 321 },
        };

        private static Dictionary<string, string> m_DruidSpellNameText = new Dictionary<string, string>
        {
            { "Shield of Earth", "[cs ShieldofEarth" },
            { "Hollow Reed", "[cs HollowReed" },
            { "Pack of Beasts", "[cs PackofBeasts" },
            { "Spring of Life", "[cs SpringofLife" },
            { "Grasping Roots", "[cs graspingroots" },
            { "Circle of Thorns", "[cs circleofthorns" },
            { "Swarm of Insects", "[cs SwarmofInsects" },
            { "Volcanic Eruption", "[cs VolcanicEruption" },
            { "Treefellow", "[cs treefellow" },
            { "Deadly Spores", "[cs deadlyspores" },
            { "Enchanted Grove", "[cs EnchantedGrove" },
            { "Lure Stone", "[cs LureStone" },
            { "Hurricane", "[cs Hurricane" },
            { "Mushroom Gateway", "[cs MushroomGateway" },
            { "Restorative Soil", "[cs RestorativeSoil" },
            { "FireFly", "[cs firefly" },
            { "Forest Kin", "[cs forestkin" },
            { "BarkSkin", "[cs barkskin" },
            { "ManaSpring", "[cs manaspring" },
            { "Hibernate", "[cs hibernate" },
        };


    }
}
