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

		public static void CastNecro(string SpellName)
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
		public static void CastChivalry(string SpellName)
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
		public static void CastBushido(string SpellName)
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
		public static void CastNinjitsu(string SpellName)
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
		public static void CastSpellweaving(string SpellName)
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
		public static void CastMysticism(string SpellName)
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

	}
}
