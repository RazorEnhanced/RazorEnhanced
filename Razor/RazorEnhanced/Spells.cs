using System;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced
{
	public class Spells
	{
		// spell
		public static void CastMagery(string SpellName)
		{
			switch (SpellName)
			{
				// Primo circolo magery
				case "Clumsy":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(1));
					break;
				case "Create Food":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(2));
					break;
				case "Feeblemind":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(3));
					break;
				case "Heal":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(4));
					break;
				case "Magic Arrow":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(5));
					break;
				case "Night Sight":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(6));
					break;
				case "Reactive Armor":
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
				case "Magic Trap":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(13));
					break;
				case "Magic Untrap":
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
				case "Magic Lock":
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
				case "Wall Of Stone":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(24));
					break;
				// Quarto circolo magery
				case "Arch Cure":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(25));
					break;
				case "Arch Protection":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(26));
					break;
				case "Curse":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(27));
					break;
				case "Fire Field":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(28));
					break;
				case "Greater Heal":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(29));
					break;
				case "Lightning":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(30));
					break;
				case "Mana Drain":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(31));
					break;
				case "Recall":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(32));
					break;
				// Quinto circolo magery
				case "Blade Spirits":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(33));
					break;
				case "Dispel Field":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(34));
					break;
				case "Incognito":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(35));
					break;
				case "Magic Reflection":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(36));
					break;
				case "Mind Blast":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(37));
					break;
				case "Paralyze":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(38));
					break;
				case "Poison Field":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(39));
					break;
				case "Summon Creature":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(40));
					break;
				// Sesto circolo magery
				case "Dispel":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(41));
					break;
				case "Energy Bolt":
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
				case "Mass Curse":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(46));
					break;
				case "Paralyze Field":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(47));
					break;
				case "Reveal":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(48));
					break;
				// Settimo circolo magery
				case "Chain Lightning":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(49));
					break;
				case "Energy Field":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(50));
					break;
				case "Flamestrike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(51));
					break;
				case "Gate Travel":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(52));
					break;
				case "Mana Vampire":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(53));
					break;
				case "Mass Dispel":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(54));
					break;
				case "Meteor Swarm":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(55));
					break;
				case "Polymorph":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(56));
					break;
				// Ottavo circolo magery
				case "Earthquake":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(57));
					break;
				case "Energy Vortex":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(58));
					break;
				case "Resurrection":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(59));
					break;
				case "Summon Air Elemental":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(60));
					break;
				case "Summon Daemon":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(61));
					break;
				case "Summon Earth Elemental":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(62));
					break;
				case "Summon Fire Elemental":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(63));
					break;
				case "Summon Water Elemental":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(64));
					break;
				default:
					Misc.SendMessage("Script Error: CastSpellMagery: Invalid spell name: " + SpellName);
					break;
			}
		}

		public static void CastNecro(string SpellName)
		{
			switch (SpellName)
			{
				case "Animate Dead":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(101));
					break;
				case "Blood Oath":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(102));
					break;
				case "Corpse Skin":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(103));
					break;
				case "Curse Weapon":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(104));
					break;
				case "Evil Omen":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(105));
					break;
				case "Horrific Beast":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(106));
					break;
				case "Lich Form":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(107));
					break;
				case "Mind Rot":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(108));
					break;
				case "Pain Spike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(109));
					break;
				case "Poison Strike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(110));
					break;
				case "Strangle":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(111));
					break;
				case "Summon Familiar":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(112));
					break;
				case "Vampiric Embrace":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(113));
					break;
				case "Vengeful Spirit":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(114));
					break;
				case "Wither":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(115));
					break;
				case "Wraith Form":
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
		public static void CastChivalry(string SpellName)
		{
			switch (SpellName)
			{
				case "Cleanse By Fire":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(201));
					break;
				case "Close Wounds":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(202));
					break;
				case "Consecrate Weapon":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(203));
					break;
				case "Dispel Evil":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(204));
					break;
				case "Divine Fury":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(205));
					break;
				case "Enemy Of One":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(206));
					break;
				case "Holy Light":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(207));
					break;
				case "Noble Sacrifice":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(208));
					break;
				case "Remove Curse":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(209));
					break;
				case "Sacred Journey":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(210));
					break;
				default:
					Misc.SendMessage("Script Error: CastSpellChivalry: Invalid spell name: " + SpellName);
					break;
			}
		}
		public static void CastBushido(string SpellName)
		{
			switch (SpellName)
			{
				case "Honorable Execution":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(401));
					break;
				case "Confidence":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(402));
					break;
				case "Evasion":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(403));
					break;
				case "Counter Attack":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(404));
					break;
				case "Lightning Strike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(405));
					break;
				case "Momentum Strike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(406));
					break;
				default:
					Misc.SendMessage("Script Error: CastSpellBushido: Invalid spell name: " + SpellName);
					break;
			}
		}
		public static void CastNinjitsu(string SpellName)
		{
			switch (SpellName)
			{
				case "Focus Attack":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(501));
					break;
				case "Death Strike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(502));
					break;
				case "Animal Form":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(503));
					break;
				case "Ki Attack":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(504));
					break;
				case "Surprise Attack":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(505));
					break;
				case "Backstab":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(506));
					break;
				case "Shadow jump":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(507));
					break;
				case "Mirror Image":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(508));
					break;
				default:
					Misc.SendMessage("Script Error: CastSpellNinjitsu: Invalid spell name: " + SpellName);
					break;
			}
		}
		public static void CastSpellweaving(string SpellName)
		{
			switch (SpellName)
			{
				case "Arcane Circle":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(601));
					break;
				case "Gift Of Renewal":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(602));
					break;
				case "Immolating Weapon":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(603));
					break;
				case "Attune Weapon":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(604));
					break;
				case "Thunderstorm":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(605));
					break;
				case "Natures Fury":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(606));
					break;
				case "Summon Fey":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(607));
					break;
				case "Summoniend":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(608));
					break;
				case "Reaper Form":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(609));
					break;
				case "Wildfire":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(610));
					break;
				case "Essence Of Wind":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(611));
					break;
				case "Dryad Allure":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(612));
					break;
				case "Ethereal Voyage":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(613));
					break;
				case "Word Of Death":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(614));
					break;
				case "Gift Of Life":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(615));
					break;
				case "Arcane Empowerment":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(616));
					break;
				default:
					Misc.SendMessage("Script Error: CastSpellSpellweaving: Invalid spell name: " + SpellName);
					break;
			}
		}
		public static void CastMysticism(string SpellName)
		{
			switch (SpellName)
			{
				case "Animated Weapon":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(678));
					break;
				case "Healing Stone":
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
				case "Eagle Strike":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(683));
					break;
				case "Stone Form":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(684));
					break;
				case "Spell Trigger":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(685));
					break;
				case "Mass Sleep":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(686));
					break;
				case "Cleansing Winds":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(687));
					break;
				case "Bombard":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(688));
					break;
				case "Spell Plague":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(689));
					break;
				case "Hail Storm":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(690));
					break;
				case "Nether Cyclone":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(691));
					break;
				case "Rising Colossus":
					Assistant.ClientCommunication.SendToServer(new CastSpellFromMacro(692));
					break;
				default:
					Misc.SendMessage("Script Error: CastSpellMysticism: Invalid spell name: " + SpellName);
					break;
			}
		}

	}
}
