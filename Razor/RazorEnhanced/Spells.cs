using Assistant;
using System.Collections.Generic;

namespace RazorEnhanced
{
	public class Spells
	{
		// spell
		public static void CastMagery(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;
			int id = 0;
			m_MagerySpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastMagery: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}

		public static void CastNecro(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_NecroSpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}


		public static void CastChivalry(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_ChivalrySpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastChivalry: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}

		public static void CastBushido(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_BushidoSpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastBushido: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}

		public static void CastNinjitsu(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_NinjitsuSpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastNinjitsu: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}

		public static void CastSpellweaving(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_SpellweavingSpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastSpellweaving: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}

		public static void CastMysticism(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_MysticismSpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastMysticism: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
		}

		public static void CastMastery(string SpellName, bool wait = true)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			int id = 0;
			m_MasterySpellName.TryGetValue(SpellName, out id);

			if (id > 0)
				s = Spell.Get(id);
			else
				Scripts.SendMessageScriptError("Script Error: CastMastery: Invalid spell name: " + SpellName);

			if (s != null)
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
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

		public static void CastLastSpell(bool wait = true)
		{
			if (World.Player.LastSpell != 0)
			{
				Spell s = Spell.Get(World.Player.LastSpell);

				if (s != null)
					s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
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
            { "Summon Water Elemental", 64 }
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
	}
}
