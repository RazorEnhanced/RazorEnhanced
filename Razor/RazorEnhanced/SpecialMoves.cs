using System;

namespace Assistant
{
	internal enum AOSAbility
	{
		Clear,
		ArmorIgnore,
		BleedAttack,
		ConcussionBlow,
		CrushingBlow,
		Disarm,
		Dismount,
		DoubleStrike,
		InfectiousStrike,
		MortalStrike,
		MovingShot,
		ParalyzingBlow,
		ShadowStrike,
		WhirlwindAttack,
		RidingSwipe,
		FrenziedWhirlwind,
		Block,
		DefenseMastery,
		NerveStrike,
		TalonStrike,
		Feint,
		DualWield,
		DoubleShot,
		ArmorPeirce,
		Bladeweave,
		ForceArrow,
		LightningArrow,
		PsychicAttack,
		SerpentArrow,
		ForceOfNature,
        Invalid
	}

	internal class SpecialMoves
	{
		private class AbilityInfo
		{
			private AOSAbility m_Ability;
			private int[][] m_Items;

			internal AbilityInfo(AOSAbility ab, params int[][] items)
			{
				m_Ability = (AOSAbility)ab;
				m_Items = items;
			}

			internal AOSAbility Ability { get { return m_Ability; } }

			internal bool HasItem(int item)
			{
				for (int a = 0; a < m_Items.Length; a++)
				{
					for (int b = 0; b < m_Items[a].Length; b++)
					{
						if (m_Items[a][b] == item)
							return true;
					}
				}

				return false;
			}
		}

		private static DateTime m_LastToggle = DateTime.MinValue;

		private static int[] HatchetID = new int[] { 0xF43, 0xF44 };
		private static int[] LongSwordID = new int[] { 0xF60, 0xF61 };
		private static int[] BroadswordID = new int[] { 0xF5E, 0xF5F };
		private static int[] KatanaID = new int[] { 0x13FE, 0x13FF };
		private static int[] BladedStaffID = new int[] { 0x26BD, 0x26C7 };
		private static int[] HammerPickID = new int[] { 0x143C, 0x143D };
		private static int[] WarAxeID = new int[] { 0x13AF, 0x13B0 };
		private static int[] KryssID = new int[] { 0x1400, 0x1401 };
		private static int[] SpearID = new int[] { 0xF62, 0xF63 };
		private static int[] CompositeBowID = new int[] { 0x26C2, 0x26CC };
		private static int[] CleaverID = new int[] { 0xEC2, 0xEC3 };
		private static int[] LargeBattleAxeID = new int[] { 0x13FA, 0x13FB };
		private static int[] BattleAxeID = new int[] { 0xF47, 0xF48 };
		private static int[] ExecAxeID = new int[] { 0xF45, 0xF46 };
		private static int[] CutlassID = new int[] { 0x1440, 0x1441 };
		private static int[] ScytheID = new int[] { 0x26BA, 0x26C4 };
		private static int[] WarMaceID = new int[] { 0x1406, 0x1407 };
		private static int[] PitchforkID = new int[] { 0xE87, 0xE88 };
		private static int[] WarForkID = new int[] { 0x1404, 0x1405 };
		private static int[] HalberdID = new int[] { 0x143E, 0x143F };
		private static int[] MaulID = new int[] { 0x143A, 0x143B };
		private static int[] MaceID = new int[] { 0xF5C, 0x45D };
		private static int[] GnarledStaffID = new int[] { 0x13F8, 0x13F9 };
		private static int[] QuarterStaffID = new int[] { 0xE89, 0xE8A };
		private static int[] LanceID = new int[] { 0x26C0, 0x26CA };
		private static int[] CrossbowID = new int[] { 0xF4F, 0xF50 };
		private static int[] VikingSwordID = new int[] { 0x13B9, 0x13BA };
		private static int[] AxeID = new int[] { 0xF49, 0xF4A };
		private static int[] ShepherdsCrookID = new int[] { 0xE81, 0xE82 };
		private static int[] SmithsHammerID = new int[] { 0x13EC, 0x13E4 };
		private static int[] WarHammerID = new int[] { 0x1438, 0x1439 };
		private static int[] ScepterID = new int[] { 0x26BC, 0x26C6 };
		private static int[] SledgeHammerID = new int[] { 0xFB4, 0xFB5 };
		private static int[] ButcherKnifeID = new int[] { 0x13F6, 0x13F7 };
		private static int[] PickaxeID = new int[] { 0xE85, 0xE86 };
		private static int[] SkinningKnifeID = new int[] { 0xEC4, 0xEC5 };
		private static int[] WandID = new int[] { 0xDF2, 0xDF3, 0xDF4, 0xDF5 };
		private static int[] BardicheID = new int[] { 0xF4D, 0xF4E };
		private static int[] ClubID = new int[] { 0x13B3, 0x13B4 };
		private static int[] ScimitarID = new int[] { 0x13B5, 0x13B6 };
		private static int[] HeavyCrossbowID = new int[] { 0x13FC, 0x13FD };
		private static int[] TwoHandedAxeID = new int[] { 0x1442, 0x1443 };
		private static int[] DoubleAxeID = new int[] { 0xF4B, 0xF4C };
		private static int[] CrescentBladeID = new int[] { 0x26C1, 0x26C2 };
		private static int[] DoubleBladedStaffID = new int[] { 0x26BF, 0x26C9 };
		private static int[] RepeatingCrossbowID = new int[] { 0x26C3, 0x26CD };
		private static int[] DaggerID = new int[] { 0xF51, 0xF52 };
		private static int[] PikeID = new int[] { 0x26BE, 0x26C8 };
		private static int[] BoneHarvesterID = new int[] { 0x26BB, 0x26C5 };
		private static int[] ShortSpearID = new int[] { 0x1402, 0x1403 };
		private static int[] BowID = new int[] { 0x13B1, 0x13B2 };
		private static int[] BlackStaffID = new int[] { 0xDF0, 0xDF1 };
		private static int[] FistsID = new int[] { 0 };

		// SA e ML e KR Weapon
		private static int[] DualShortAxesID = new int[] { 0x08FD, 0x4068 };
		private static int[] GargishBattleAxeID = new int[] { 0x48B0, 0x48B1 };
		private static int[] GargishAxeID = new int[] { 0x48B2, 0x48B3 };
		private static int[] OrnateAxeID = new int[] { 0x2D28, 0x2D34 };
		private static int[] BokutoID = new int[] { 0x27A8, 0x27F3 };
		private static int[] DaishoID = new int[] { 0x27A9, 0x27F4 };
		private static int[] DreadSwordID = new int[] { 0x090B, 0x4074 };
		private static int[] ElvenMacheteID = new int[] { 0x2D29, 0x2D35 };
		private static int[] GargishBardicheID = new int[] { 0x48B4, 0x48B5 };
		private static int[] GargishBoneHarvesterID = new int[] { 0x48C6, 0x48C7 };
		private static int[] GargishButcherKnifeID = new int[] { 0x48B6, 0x48B7 };
		private static int[] GargishCleaverID = new int[] { 0x48AE, 0x48AF };
		private static int[] GargishDaishoID = new int[] { 0x48D0, 0x48D1 };
		private static int[] GargishKatanaID = new int[] { 0x48BA, 0x48BB };
		private static int[] GargishScytheID = new int[] { 0x48C4, 0x48C5 };
		private static int[] GargishTalwarID = new int[] { 0x0908, 0x4075 };
		private static int[] GlassSwordID = new int[] { 0x090C, 0x4073 };
		private static int[] NoDachiID = new int[] { 0x27A2, 0x27ED };
		private static int[] RadiantScimitarID = new int[] { 0x2D27, 0x2D33 };
		private static int[] RuneBladeID = new int[] { 0x2D26, 0x2D32 };
		private static int[] WakizashiID = new int[] { 0x27A4, 0x27EF };
		private static int[] DiamondMaceID = new int[] { 0x2D24, 0x2D30 };
		private static int[] DiscMaceID = new int[] { 0x0903, 0x406E };
		private static int[] GargishGnarledStaffID = new int[] { 0x48B8, 0x48B9 };
		private static int[] GargishMaulID = new int[] { 0x48C2, 0x48C3 };
		private static int[] GargishTessenID = new int[] { 0x48CC, 0x48CD };
		private static int[] GargishWarHammerID = new int[] { 0x48C0, 0x48C1 };
		private static int[] GlassStaffID = new int[] { 0x0905, 0x4070 };
		private static int[] NunchakuID = new int[] { 0x27AE, 0x27F9 };
		private static int[] SerpentstoneStaffID = new int[] { 0x0906, 0x406F };
		private static int[] TessenID = new int[] { 0x27A3, 0x27EE };
		private static int[] TetsuboID = new int[] { 0x27A3, 0x27EE };
		private static int[] ElvenCompositeLongbowID = new int[] { 0x2D1E, 0x2D2A };
		private static int[] MagicalShortbowID = new int[] { 0x2D1F, 0x2D2B };
		private static int[] YumiID = new int[] { 0x27A5, 0x27F0 };
		private static int[] AssassinSpikeID = new int[] { 0x2D21, 0x2D2D };
		private static int[] BloodbladeID = new int[] { 0x08FE, 0x4072 };
		private static int[] DualPointedSpearID = new int[] { 0x0904, 0x406D };
		private static int[] ElvenSpellbladeID = new int[] { 0x2D20, 0x2D2C };
		private static int[] GargishDaggerID = new int[] { 0x0902, 0x406A };
		private static int[] GargishKryssID = new int[] { 0x48BC, 0x48BD };
		private static int[] GargishLanceID = new int[] { 0x48CA, 0x48CB };
		private static int[] GargishPikeID = new int[] { 0x48C8, 0x48C9 };
		private static int[] GargishTekagiID = new int[] { 0x48CE, 0x48CF };
		private static int[] GargishWarForkID = new int[] { 0x48BE, 0x48BF };
		private static int[] KamaID = new int[] { 0x27AD, 0x27F8 };
		private static int[] LajatangID = new int[] { 0x27A7, 0x27F2 };
		private static int[] LeafbladeID = new int[] { 0x2D22, 0x2D2E };
		private static int[] SaiID = new int[] { 0x27AF, 0x27FA };
		private static int[] ShortbladeID = new int[] { 0x0907, 0x4076 };
		private static int[] TekagiID = new int[] { 0x27AB, 0x27F6 };
		private static int[] WarCleaverID = new int[] { 0x2D23, 0x2D2F };
		private static int[] BoomerangID = new int[] { 0x2D23, 0x2D2F };


		private static AbilityInfo[] m_Primary = new AbilityInfo[]
		{
			new AbilityInfo( AOSAbility.ArmorIgnore, HatchetID, LongSwordID, BladedStaffID, HammerPickID, WarAxeID, KryssID, SpearID, CompositeBowID, DiscMaceID, GargishKryssID, ShortbladeID ),
			new AbilityInfo( AOSAbility.ArmorPeirce, YumiID ),
            new AbilityInfo( AOSAbility.BleedAttack,  CleaverID, BattleAxeID, ExecAxeID, CutlassID, ScytheID, PitchforkID, WarForkID, GargishBattleAxeID, GargishCleaverID, GargishScytheID, GlassSwordID, BloodbladeID, GargishWarForkID ),
			new AbilityInfo( AOSAbility.Block, NunchakuID ),
            new AbilityInfo( AOSAbility.ConcussionBlow, MaceID, GnarledStaffID, CrossbowID, DiamondMaceID, GargishGnarledStaffID ),
			new AbilityInfo( AOSAbility.CrushingBlow, VikingSwordID, AxeID, BroadswordID, ShepherdsCrookID, SmithsHammerID, MaulID, WarMaceID, ScepterID, SledgeHammerID, GargishAxeID, DreadSwordID, NoDachiID, SerpentstoneStaffID ),
			new AbilityInfo( AOSAbility.DefenseMastery, ElvenMacheteID, LajatangID ),
			new AbilityInfo( AOSAbility.Disarm, FistsID, OrnateAxeID, RuneBladeID, GargishLanceID, WarCleaverID ),
			new AbilityInfo( AOSAbility.Dismount, WandID, LanceID ),
			new AbilityInfo( AOSAbility.DoubleStrike, PickaxeID, TwoHandedAxeID, DoubleAxeID, ScimitarID, KatanaID, CrescentBladeID, QuarterStaffID, DoubleBladedStaffID, RepeatingCrossbowID, DualShortAxesID, GargishKatanaID, GargishMaulID, GlassStaffID, NunchakuID, DualPointedSpearID ),
			new AbilityInfo( AOSAbility.DualWield, GargishTekagiID, SaiID, TekagiID ),
            new AbilityInfo( AOSAbility.InfectiousStrike, ButcherKnifeID, DaggerID, GargishButcherKnifeID, AssassinSpikeID ),
			new AbilityInfo( AOSAbility.LightningArrow, MagicalShortbowID ),
            new AbilityInfo( AOSAbility.Feint, BokutoID, DaishoID, GargishDaishoID, GargishTessenID, TessenID, LeafbladeID ),
			new AbilityInfo( AOSAbility.FrenziedWhirlwind, WakizashiID ),
			new AbilityInfo( AOSAbility.ForceArrow, ElvenCompositeLongbowID ),
			//new AbilityInfo( AOSAbility.MortalStrike ), // not primary for anything
			new AbilityInfo( AOSAbility.MovingShot, HeavyCrossbowID ),
			new AbilityInfo( AOSAbility.ParalyzingBlow, BardicheID, BoneHarvesterID, PikeID, BowID, GargishBardicheID, GargishBoneHarvesterID, GargishPikeID ),
			new AbilityInfo( AOSAbility.PsychicAttack, ElvenSpellbladeID ),
            new AbilityInfo( AOSAbility.ShadowStrike, SkinningKnifeID, ClubID, ShortSpearID, GargishDaggerID ),
			new AbilityInfo( AOSAbility.WhirlwindAttack, LargeBattleAxeID, HalberdID, WarHammerID, BlackStaffID, GargishTalwarID, RadiantScimitarID, GargishWarHammerID, KamaID )   
		};

		private static AbilityInfo[] m_Secondary = new AbilityInfo[]
		{
			
            new AbilityInfo( AOSAbility.ArmorPeirce, SaiID ),
			new AbilityInfo( AOSAbility.ArmorIgnore, LargeBattleAxeID, BroadswordID, KatanaID, GargishKatanaID, LeafbladeID ),
			new AbilityInfo( AOSAbility.Bladeweave, ElvenMacheteID, RadiantScimitarID, RuneBladeID, WarCleaverID ),
			new AbilityInfo( AOSAbility.BleedAttack, WarMaceID, WarAxeID, ElvenSpellbladeID ),
			new AbilityInfo( AOSAbility.ConcussionBlow, LongSwordID, BattleAxeID, HalberdID, MaulID, QuarterStaffID, LanceID, GargishBattleAxeID, DreadSwordID, GargishMaulID, GargishLanceID ),
			new AbilityInfo( AOSAbility.CrushingBlow, WarHammerID, OrnateAxeID, DiamondMaceID, GargishWarHammerID ),
            new AbilityInfo( AOSAbility.DefenseMastery, KamaID ),
			new AbilityInfo( AOSAbility.Disarm, ButcherKnifeID, PickaxeID, SkinningKnifeID, HatchetID, WandID, ShepherdsCrookID, MaceID, WarForkID, GargishButcherKnifeID, DiscMaceID, DualPointedSpearID, GargishWarForkID ),
			new AbilityInfo( AOSAbility.Dismount, BardicheID, AxeID, BladedStaffID, ClubID, PitchforkID, HeavyCrossbowID, GargishAxeID, GargishBardicheID, GargishTalwarID, SerpentstoneStaffID ),
			new AbilityInfo( AOSAbility.DualWield, TessenID ),
            new AbilityInfo( AOSAbility.DoubleStrike, DaishoID, GargishDaishoID, WakizashiID),
			new AbilityInfo( AOSAbility.DoubleShot, YumiID ),
			new AbilityInfo( AOSAbility.FrenziedWhirlwind, LajatangID ),
            new AbilityInfo( AOSAbility.InfectiousStrike, CleaverID, PikeID, KryssID, DoubleBladedStaffID, DualShortAxesID, GargishCleaverID, GargishDaggerID, GargishKryssID, GargishPikeID ),
			new AbilityInfo( AOSAbility.MortalStrike, ExecAxeID, BoneHarvesterID, CrescentBladeID, HammerPickID, ScepterID, ShortSpearID, CrossbowID, BowID, GargishBoneHarvesterID, GlassSwordID, GlassStaffID, ShortbladeID ),
			new AbilityInfo( AOSAbility.MovingShot, CompositeBowID, RepeatingCrossbowID ),
			new AbilityInfo( AOSAbility.ParalyzingBlow, VikingSwordID, ScimitarID, ScytheID, GnarledStaffID, BlackStaffID, SpearID, FistsID, GargishScytheID, GargishGnarledStaffID, BloodbladeID ),
			new AbilityInfo( AOSAbility.PsychicAttack, MagicalShortbowID ),
			new AbilityInfo( AOSAbility.RidingSwipe, NoDachiID ),
			new AbilityInfo( AOSAbility.SerpentArrow, ElvenCompositeLongbowID ),
            new AbilityInfo( AOSAbility.ShadowStrike, TwoHandedAxeID, CutlassID, SmithsHammerID, DaggerID, SledgeHammerID, AssassinSpikeID ),
			new AbilityInfo( AOSAbility.TalonStrike, GargishTekagiID, TekagiID ),
            new AbilityInfo( AOSAbility.WhirlwindAttack, DoubleAxeID ),
			new AbilityInfo( AOSAbility.NerveStrike, BokutoID )
		};

		private static void ToggleWarPeace()
		{
			ClientCommunication.SendToServer(new SetWarMode(!World.Player.Warmode));
		}

		internal static void OnStun()
		{
			if (m_LastToggle + TimeSpan.FromSeconds(0.5) < DateTime.Now)
			{
				m_LastToggle = DateTime.Now;
				ClientCommunication.SendToServer(new StunRequest());
			}
		}

		internal static void OnDisarm()
		{
			if (m_LastToggle + TimeSpan.FromSeconds(0.5) < DateTime.Now)
			{
				m_LastToggle = DateTime.Now;
				ClientCommunication.SendToServer(new DisarmRequest());
			}
		}

		private static AOSAbility GetAbility(int item, AbilityInfo[] list)
		{
			for (int a = 0; a < list.Length; a++)
			{
				if (list[a].HasItem(item))
					return list[a].Ability;
			}

			return AOSAbility.Invalid;
		}

		internal static void SetPrimaryAbility()
		{
			Item right = World.Player.GetItemOnLayer(Layer.RightHand);
			Item left = World.Player.GetItemOnLayer(Layer.LeftHand);

			AOSAbility a = AOSAbility.Invalid;
			if (right != null)
				a = GetAbility(right.ItemID.Value, m_Primary);

			if (a == AOSAbility.Invalid && left != null)
				a = GetAbility(left.ItemID.Value, m_Primary);

			if (a == AOSAbility.Invalid)
				a = GetAbility(FistsID[0], m_Primary);

			if (a != AOSAbility.Invalid)
			{
				ClientCommunication.SendToServer(new UseAbility(a));
				ClientCommunication.SendToClient(ClearAbility.Instance);
				World.Player.SendMessage(LocString.SettingAOSAb, a);
			}
		}

		internal static void SetSecondaryAbility()
		{
			Item right = World.Player.GetItemOnLayer(Layer.RightHand);
			Item left = World.Player.GetItemOnLayer(Layer.LeftHand);

			AOSAbility a = AOSAbility.Invalid;
			if (right != null)
				a = GetAbility(right.ItemID.Value, m_Secondary);

			if (a == AOSAbility.Invalid && left != null)
				a = GetAbility(left.ItemID.Value, m_Secondary);

			if (a == AOSAbility.Invalid)
				a = GetAbility(FistsID[0], m_Secondary);

			if (a != AOSAbility.Invalid)
			{
				ClientCommunication.SendToServer(new UseAbility(a));
				ClientCommunication.SendToClient(ClearAbility.Instance);
				World.Player.SendMessage(LocString.SettingAOSAb, a);
			}
		}

		internal static void ClearAbilities()
		{
			ClientCommunication.SendToServer(new UseAbility(AOSAbility.Clear));
			ClientCommunication.SendToClient(ClearAbility.Instance);
			World.Player.SendMessage(LocString.AOSAbCleared);
		}
	}
}