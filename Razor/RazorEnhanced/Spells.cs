using Assistant;

namespace RazorEnhanced
{
	public class Spells
	{
		// spell
		public static void CastMagery(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				// Primo circolo magery
				case "Clumsy":
					s = Spell.Get(1);
					break;

				case "Create Food":
					s = Spell.Get(2);
					break;

				case "Feeblemind":
					s = Spell.Get(3);
					break;

				case "Heal":
					s = Spell.Get(4);
					break;

				case "Magic Arrow":
					s = Spell.Get(5);
					break;

				case "Night Sight":
					s = Spell.Get(6);
					break;

				case "Reactive Armor":
					s = Spell.Get(7);
					break;

				case "Weaken":
					s = Spell.Get(8);
					break;
				// Secondo circolo magery
				case "Agility":
					s = Spell.Get(9);
					break;

				case "Cunning":
					s = Spell.Get(10);
					break;

				case "Cure":
					s = Spell.Get(11);
					break;

				case "Harm":
					s = Spell.Get(12);
					break;

				case "Magic Trap":
					s = Spell.Get(13);
					break;

				case "Magic Untrap":
					s = Spell.Get(14);
					break;

				case "Protection":
					s = Spell.Get(15);
					break;

				case "Strength":
					s = Spell.Get(16);
					break;
				// Terzo circolo magery
				case "Bless":
					s = Spell.Get(17);
					break;

				case "Fireball":
					s = Spell.Get(18);
					break;

				case "Magic Lock":
					s = Spell.Get(19);
					break;

				case "Poison":
					s = Spell.Get(20);
					break;

				case "Telekinesis":
					s = Spell.Get(21);
					break;

				case "Teleport":
					s = Spell.Get(22);
					break;

				case "Unlock":
					s = Spell.Get(23);
					break;

				case "Wall of Stone":
					s = Spell.Get(24);
					break;
				// Quarto circolo magery
				case "Arch Cure":
					s = Spell.Get(25);
					break;

				case "Arch Protection":
					s = Spell.Get(26);
					break;

				case "Curse":
					s = Spell.Get(27);
					break;

				case "Fire Field":
					s = Spell.Get(28);
					break;

				case "Greater Heal":
					s = Spell.Get(29);
					break;

				case "Lightning":
					s = Spell.Get(30);
					break;

				case "Mana Drain":
					s = Spell.Get(31);
					break;

				case "Recall":
					s = Spell.Get(32);
					break;
				// Quinto circolo magery
				case "Blade Spirits":
					s = Spell.Get(33);
					break;

				case "Dispel Field":
					s = Spell.Get(34);
					break;

				case "Incognito":
					s = Spell.Get(35);
					break;

				case "Magic Reflection":
					s = Spell.Get(36);
					break;

				case "Mind Blast":
					s = Spell.Get(37);
					break;

				case "Paralyze":
					s = Spell.Get(38);
					break;

				case "Poison Field":
					s = Spell.Get(39);
					break;

				case "Summon Creature":
					s = Spell.Get(40);
					break;
				// Sesto circolo magery
				case "Dispel":
					s = Spell.Get(41);
					break;

				case "Energy Bolt":
					s = Spell.Get(42);
					break;

				case "Explosion":
					s = Spell.Get(43);
					break;

				case "Invisibility":
					s = Spell.Get(44);
					break;

				case "Mark":
					s = Spell.Get(45);
					break;

				case "Mass Curse":
					s = Spell.Get(46);
					break;

				case "Paralyze Field":
					s = Spell.Get(47);
					break;

				case "Reveal":
					s = Spell.Get(48);
					break;
				// Settimo circolo magery
				case "Chain Lightning":
					s = Spell.Get(49);
					break;

				case "Energy Field":
					s = Spell.Get(50);
					break;

				case "Flamestrike":
					s = Spell.Get(51);
					break;

				case "Gate Travel":
					s = Spell.Get(52);
					break;

				case "Mana Vampire":
					s = Spell.Get(53);
					break;

				case "Mass Dispel":
					s = Spell.Get(54);
					break;

				case "Meteor Swarm":
					s = Spell.Get(55);
					break;

				case "Polymorph":
					s = Spell.Get(56);
					break;
				// Ottavo circolo magery
				case "Earthquake":
					s = Spell.Get(57);
					break;

				case "Energy Vortex":
					s = Spell.Get(58);
					break;

				case "Resurrection":
					s = Spell.Get(59);
					break;

				case "Summon Air Elemental":
					s = Spell.Get(60);
					break;

				case "Summon Daemon":
					s = Spell.Get(61);
					break;

				case "Summon Earth Elemental":
					s = Spell.Get(62);
					break;

				case "Summon Fire Elemental":
					s = Spell.Get(63);
					break;

				case "Summon Water Elemental":
					s = Spell.Get(64);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellMagery: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}

		public static void CastNecro(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				case "Animate Dead":
					s = Spell.Get(101);
					break;

				case "Blood Oath":
					s = Spell.Get(102);
					break;

				case "Corpse Skin":
					s = Spell.Get(103);
					break;

				case "Curse Weapon":
					s = Spell.Get(104);
					break;

				case "Evil Omen":
					s = Spell.Get(105);
					break;

				case "Horrific Beast":
					s = Spell.Get(106);
					break;

				case "Lich Form":
					s = Spell.Get(107);
					break;

				case "Mind Rot":
					s = Spell.Get(108);
					break;

				case "Pain Spike":
					s = Spell.Get(109);
					break;

				case "Poison Strike":
					s = Spell.Get(110);
					break;

				case "Strangle":
					s = Spell.Get(111);
					break;

				case "Summon Familiar":
					s = Spell.Get(112);
					break;

				case "Vampiric Embrace":
					s = Spell.Get(113);
					break;

				case "Vengeful Spirit":
					s = Spell.Get(114);
					break;

				case "Wither":
					s = Spell.Get(115);
					break;

				case "Wraith Form":
					s = Spell.Get(116);
					break;

				case "Exorcism":
					s = Spell.Get(117);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellNecro: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}

		public static void CastChivalry(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				case "Cleanse By Fire":
					s = Spell.Get(201);
					break;

				case "Close Wounds":
					s = Spell.Get(202);
					break;

				case "Consecrate Weapon":
					s = Spell.Get(203);
					break;

				case "Dispel Evil":
					s = Spell.Get(204);
					break;

				case "Divine Fury":
					s = Spell.Get(205);
					break;

				case "Enemy Of One":
					s = Spell.Get(206);
					break;

				case "Holy Light":
					s = Spell.Get(207);
					break;

				case "Noble Sacrifice":
					s = Spell.Get(208);
					break;

				case "Remove Curse":
					s = Spell.Get(209);
					break;

				case "Sacred Journey":
					s = Spell.Get(210);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellChivalry: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}

		public static void CastBushido(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				case "Honorable Execution":
					s = Spell.Get(401);
					break;

				case "Confidence":
					s = Spell.Get(402);
					break;

				case "Evasion":
					s = Spell.Get(403);
					break;

				case "Counter Attack":
					s = Spell.Get(404);
					break;

				case "Lightning Strike":
					s = Spell.Get(405);
					break;

				case "Momentum Strike":
					s = Spell.Get(406);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellBushido: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}

		public static void CastNinjitsu(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				case "Focus Attack":
					s = Spell.Get(501);
					break;

				case "Death Strike":
					s = Spell.Get(502);
					break;

				case "Animal Form":
					s = Spell.Get(503);
					break;

				case "Ki Attack":
					s = Spell.Get(504);
					break;

				case "Surprise Attack":
					s = Spell.Get(505);
					break;

				case "Backstab":
					s = Spell.Get(506);
					break;

				case "Shadow jump":
					s = Spell.Get(507);
					break;

				case "Mirror Image":
					s = Spell.Get(508);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellNinjitsu: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}

		public static void CastSpellweaving(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				case "Arcane Circle":
					s = Spell.Get(601);
					break;

				case "Gift Of Renewal":
					s = Spell.Get(602);
					break;

				case "Immolating Weapon":
					s = Spell.Get(603);
					break;

				case "Attune Weapon":
					s = Spell.Get(604);
					break;

				case "Thunderstorm":
					s = Spell.Get(605);
					break;

				case "Natures Fury":
					s = Spell.Get(606);
					break;

				case "Summon Fey":
					s = Spell.Get(607);
					break;

				case "Summon Fiend":
					s = Spell.Get(608);
					break;

				case "Reaper Form":
					s = Spell.Get(609);
					break;

				case "Wildfire":
					s = Spell.Get(610);
					break;

				case "Essence Of Wind":
					s = Spell.Get(611);
					break;

				case "Dryad Allure":
					s = Spell.Get(612);
					break;

				case "Ethereal Voyage":
					s = Spell.Get(613);
					break;

				case "Word Of Death":
					s = Spell.Get(614);
					break;

				case "Gift Of Life":
					s = Spell.Get(615);
					break;

				case "Arcane Empowerment":
					s = Spell.Get(616);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellSpellweaving: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}

		public static void CastMysticism(string SpellName)
		{
			if (World.Player == null)
				return;

			Spell s = null;

			switch (SpellName)
			{
				case "Animated Weapon":
					s = Spell.Get(678);
					break;

				case "Healing Stone":
					s = Spell.Get(679);
					break;

				case "Purge":
					s = Spell.Get(680);
					break;

				case "Enchant":
					s = Spell.Get(681);
					break;

				case "Sleep":
					s = Spell.Get(682);
					break;

				case "Eagle Strike":
					s = Spell.Get(683);
					break;

				case "Stone Form":
					s = Spell.Get(684);
					break;

				case "Spell Trigger":
					s = Spell.Get(685);
					break;

				case "Mass Sleep":
					s = Spell.Get(686);
					break;

				case "Cleansing Winds":
					s = Spell.Get(687);
					break;

				case "Bombard":
					s = Spell.Get(688);
					break;

				case "Spell Plague":
					s = Spell.Get(689);
					break;

				case "Hail Storm":
					s = Spell.Get(690);
					break;

				case "Nether Cyclone":
					s = Spell.Get(691);
					break;

				case "Rising Colossus":
					s = Spell.Get(692);
					break;

				default:
					if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
						Misc.SendMessage("Script Error: CastSpellMysticism: Invalid spell name: " + SpellName);
					break;
			}
			if (s != null)
			{
				ClientCommunication.SendRecvWait();
				s.OnCast(new CastSpellFromMacro((ushort)s.GetID()));
			}
		}
	}
}