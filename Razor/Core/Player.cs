using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Assistant
{
	internal enum LockType : byte
	{
		Up = 0,
		Down = 1,
		Locked = 2,
	}

	internal enum MsgLevel
	{
		Debug = 0,
		Info = 0,
		Warning = 1,
		Error = 2,
		Force = 3,
	}

	internal class Skill
	{
		internal static int Count = 58;

		private LockType m_Lock;
		private ushort m_Value;
		private ushort m_Base;
		private ushort m_Cap;
		private short m_Delta;
		private int m_Idx;

		internal Skill(int idx)
		{
			m_Idx = idx;
		}

		internal int Index { get { return m_Idx; } }

		internal LockType Lock
		{
			get { return m_Lock; }
			set { m_Lock = value; }
		}

		internal ushort FixedValue
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		internal ushort FixedBase
		{
			get { return m_Base; }
			set
			{
				m_Delta += (short)(value - m_Base);
				m_Base = value;
			}
		}

		internal ushort FixedCap
		{
			get { return m_Cap; }
			set { m_Cap = value; }
		}

		internal double Value
		{
			get { return m_Value / 10.0; }
			set { m_Value = (ushort)(value * 10.0); }
		}

		internal double Base
		{
			get { return m_Base / 10.0; }
			set { m_Base = (ushort)(value * 10.0); }
		}

		internal double Cap
		{
			get { return m_Cap / 10.0; }
			set { m_Cap = (ushort)(value * 10.0); }
		}

		internal double Delta
		{
			get { return m_Delta / 10.0; }
			set { m_Delta = (short)(value * 10); }
		}
	}

	internal enum Virtues
	{
		None = 0x0,
		Honor = 0x1,
		Sacrifice = 0x2,
		Valor = 0x3,
		Compassion = 0x4,
		Honesty = 0x5,
		Humility = 0x6,
		Justice = 0x7
	}
    public enum StatName
    {
        Strength = 0,
        Dexterity = 1,
        Intelligence = 2,
    };

	public enum SkillName
	{
		Alchemy = 0,
		Anatomy = 1,
		AnimalLore = 2,
		ItemID = 3,
		ArmsLore = 4,
		Parry = 5,
		Begging = 6,
		Blacksmith = 7,
		Fletching = 8,
		Peacemaking = 9,
		Camping = 10,
		Carpentry = 11,
		Cartography = 12,
		Cooking = 13,
		DetectHidden = 14,
		Discordance = 15,
		EvalInt = 16,
		Healing = 17,
		Fishing = 18,
		Forensics = 19,
		Herding = 20,
		Hiding = 21,
		Provocation = 22,
		Inscribe = 23,
		Lockpicking = 24,
		Magery = 25,
		MagicResist = 26,
		Tactics = 27,
		Snooping = 28,
		Musicianship = 29,
		Poisoning = 30,
		Archery = 31,
		SpiritSpeak = 32,
		Stealing = 33,
		Tailoring = 34,
		AnimalTaming = 35,
		TasteID = 36,
		Tinkering = 37,
		Tracking = 38,
		Veterinary = 39,
		Swords = 40,
		Macing = 41,
		Fencing = 42,
		Wrestling = 43,
		Lumberjacking = 44,
		Mining = 45,
		Meditation = 46,
		Stealth = 47,
		RemoveTrap = 48,
		Necromancy = 49,
		Focus = 50,
		Chivalry = 51,
		Bushido = 52,
		Ninjitsu = 53,
		SpellWeaving = 54,
		Mysticism = 55,
		Imbuing = 56,
		Throwing = 57
	}

	internal class PlayerData : Mobile
	{
		internal class MoveEntry
		{
			//public byte Seq;
			internal Direction Dir;

			//public int x;
			//public int y;
			internal Point3D Position;

			internal bool IsStep;

			internal bool FilterAck;
		}

		internal int VisRange = 31;
		internal int MultiVisRange { get { return VisRange + 5; } }

		private int m_MaxWeight = -1;

		private byte m_Expansion = 0;
		private int m_Race;

		private short m_FireResist, m_ColdResist, m_PoisonResist, m_EnergyResist, m_Luck;
		private ushort m_DamageMin, m_DamageMax;

		// KR Data
		private short m_HitChanceIncrease, m_SwingSpeedIncrease, m_DamageChanceIncrease, m_LowerReagentCost, m_HitPointsRegeneration, m_StaminaRegeneration, m_ManaRegeneration, m_ReflectPhysicalDamage, m_EnhancePotions, m_DefenseChanceIncrease;
		private short m_SpellDamageIncrease, m_FasterCastRecovery, m_FasterCasting, m_LowerManaCost, m_StrengthIncrease, m_DexterityIncrease, m_IntelligenceIncrease, m_HitPointsIncrease, m_StaminaIncrease, m_ManaIncrease, m_MaximumHitPointsIncrease;
		private short m_MaximumStaminaIncrease, m_MaximumManaIncrease;

		private ushort m_Str, m_Dex, m_Int;
		private LockType m_StrLock, m_DexLock, m_IntLock;
		private uint m_Gold;
		private ushort m_Weight;
		private Skill[] m_Skills;
		private ushort m_AR;
		private ushort m_StatCap;
		private byte m_Followers;
		private byte m_FollowersMax;
		private int m_Tithe;
		private sbyte m_LocalLight;
		private byte m_GlobalLight;
		private ushort m_Features;
		private byte m_Season;
		private int[] m_MapPatches = new int[10];

		private bool m_SkillsSent;

		//private Item m_Holding;
		//private ushort m_HoldAmt;
		private ConcurrentDictionary<byte, MoveEntry> m_MoveInfo;

		private Timer m_CriminalTime;
		private DateTime m_CriminalStart = DateTime.MinValue;
		private byte m_WalkSeq;

		internal static int FastWalkKey = 0;

		private List<BuffIcon> m_Buffs = new List<BuffIcon>();
		internal List<BuffIcon> Buffs { get { return m_Buffs; } }

		private List<SkillIcon> m_SkillEnabled = new List<SkillIcon>();
		internal List<SkillIcon> SkillEnabled { get { return m_SkillEnabled; } }


		internal PlayerData(Serial serial)
			: base(serial)
		{
			m_MoveInfo = new ConcurrentDictionary<byte, MoveEntry>();
			m_Skills = new Skill[Skill.Count];
			for (int i = 0; i < m_Skills.Length; i++)
				m_Skills[i] = new Skill(i);
		}

		internal ushort Str
		{
			get { return m_Str; }
			set { m_Str = value; }
		}

		internal ushort Dex
		{
			get { return m_Dex; }
			set { m_Dex = value; }
		}

		internal ushort Int
		{
			get { return m_Int; }
			set { m_Int = value; }
		}

		internal uint Gold
		{
			get { return m_Gold; }
			set { m_Gold = value; }
		}

		internal ushort Weight
		{
			get { return m_Weight; }
			set { m_Weight = value; }
		}

		internal ushort MaxWeight
		{
			get
			{
				if (m_MaxWeight == -1)
					return (ushort)((m_Str * 3.5) + 40);
				else
					return (ushort)m_MaxWeight;
			}
			set
			{
				m_MaxWeight = value;
			}
		}

		internal byte Expansion
		{
			get { return m_Expansion; }
			set { m_Expansion = value; }
		}

		internal int Race
		{
			get { return m_Race; }
			set { m_Race = value; }
		}

		internal short FireResistance
		{
			get { return m_FireResist; }
			set { m_FireResist = value; }
		}

		internal short ColdResistance
		{
			get { return m_ColdResist; }
			set { m_ColdResist = value; }
		}

		internal short PoisonResistance
		{
			get { return m_PoisonResist; }
			set { m_PoisonResist = value; }
		}

		internal short EnergyResistance
		{
			get { return m_EnergyResist; }
			set { m_EnergyResist = value; }
		}

		internal short Luck
		{
			get { return m_Luck; }
			set { m_Luck = value; }
		}

		internal ushort DamageMin
		{
			get { return m_DamageMin; }
			set { m_DamageMin = value; }
		}

		internal ushort DamageMax
		{
			get { return m_DamageMax; }
			set { m_DamageMax = value; }
		}

		internal LockType StrLock
		{
			get { return m_StrLock; }
			set { m_StrLock = value; }
		}

		internal LockType DexLock
		{
			get { return m_DexLock; }
			set { m_DexLock = value; }
		}

		internal LockType IntLock
		{
			get { return m_IntLock; }
			set { m_IntLock = value; }
		}

		internal ushort StatCap
		{
			get { return m_StatCap; }
			set { m_StatCap = value; }
		}

		internal ushort AR
		{
			get { return m_AR; }
			set { m_AR = value; }
		}

		internal byte Followers
		{
			get { return m_Followers; }
			set { m_Followers = value; }
		}

		internal byte FollowersMax
		{
			get { return m_FollowersMax; }
			set { m_FollowersMax = value; }
		}

		internal int Tithe
		{
			get { return m_Tithe; }
			set { m_Tithe = value; }
		}

		internal short HitChanceIncrease
		{
			get { return m_HitChanceIncrease; }
			set { m_HitChanceIncrease = value; }
		}

		internal short SwingSpeedIncrease
		{
			get { return m_SwingSpeedIncrease; }
			set { m_SwingSpeedIncrease = value; }
		}

		internal short DamageChanceIncrease
		{
			get { return m_DamageChanceIncrease; }
			set { m_DamageChanceIncrease = value; }
		}

		internal short LowerReagentCost
		{
			get { return m_LowerReagentCost; }
			set { m_LowerReagentCost = value; }
		}

		internal short HitPointsRegeneration
		{
			get { return m_HitPointsRegeneration; }
			set { m_HitPointsRegeneration = value; }
		}

		internal short StaminaRegeneration
		{
			get { return m_StaminaRegeneration; }
			set { m_StaminaRegeneration = value; }
		}

		internal short ManaRegeneration
		{
			get { return m_ManaRegeneration; }
			set { m_ManaRegeneration = value; }
		}

		internal short ReflectPhysicalDamage
		{
			get { return m_ReflectPhysicalDamage; }
			set { m_ReflectPhysicalDamage = value; }
		}

		internal short EnhancePotions
		{
			get { return m_EnhancePotions; }
			set { m_EnhancePotions = value; }
		}

		internal short DefenseChanceIncrease
		{
			get { return m_DefenseChanceIncrease; }
			set { m_DefenseChanceIncrease = value; }
		}

		internal short SpellDamageIncrease
		{
			get { return m_SpellDamageIncrease; }
			set { m_SpellDamageIncrease = value; }
		}

		internal short FasterCastRecovery
		{
			get { return m_FasterCastRecovery; }
			set { m_FasterCastRecovery = value; }
		}

		internal short FasterCasting
		{
			get { return m_FasterCasting; }
			set { m_FasterCasting = value; }
		}

		internal short LowerManaCost
		{
			get { return m_LowerManaCost; }
			set { m_LowerManaCost = value; }
		}

		internal short StrengthIncrease
		{
			get { return m_StrengthIncrease; }
			set { m_StrengthIncrease = value; }
		}

		internal short DexterityIncrease
		{
			get { return m_DexterityIncrease; }
			set { m_DexterityIncrease = value; }
		}

		internal short IntelligenceIncrease
		{
			get { return m_IntelligenceIncrease; }
			set { m_IntelligenceIncrease = value; }
		}

		internal short HitPointsIncrease
		{
			get { return m_HitPointsIncrease; }
			set { m_HitPointsIncrease = value; }
		}

		internal short StaminaIncrease
		{
			get { return m_StaminaIncrease; }
			set { m_StaminaIncrease = value; }
		}

		internal short ManaIncrease
		{
			get { return m_ManaIncrease; }
			set { m_ManaIncrease = value; }
		}

		internal short MaximumHitPointsIncrease
		{
			get { return m_MaximumHitPointsIncrease; }
			set { m_MaximumHitPointsIncrease = value; }
		}

		internal short MaximumStaminaIncrease
		{
			get { return m_MaximumStaminaIncrease; }
			set { m_MaximumStaminaIncrease = value; }
		}

		internal short MaximumManaIncrease
		{
			get { return m_MaximumManaIncrease; }
			set { m_MaximumManaIncrease = value; }
		}

		internal Skill[] Skills { get { return m_Skills; } }

		internal bool SkillsSent
		{
			get { return m_SkillsSent; }
			set { m_SkillsSent = value; }
		}

		internal byte WalkSequence { get { return m_WalkSeq; } }

		internal int CriminalTime
		{
			get
			{
				if (m_CriminalStart != DateTime.MinValue)
				{
					int sec = (int)(DateTime.Now - m_CriminalStart).TotalSeconds;
					if (sec > 300)
					{
						if (m_CriminalTime != null)
							m_CriminalTime.Stop();
						m_CriminalStart = DateTime.MinValue;
						return 0;
					}
					else
					{
						return sec;
					}
				}
				else
				{
					return 0;
				}
			}
		}

		internal void Resync()
		{
			m_OutstandingMoves = m_WalkSeq = 0;
			m_MoveInfo.Clear();
		}

		private int m_OutstandingMoves = 0;

		internal int OutstandingMoveReqs { get { return m_OutstandingMoves; } }

		internal MoveEntry GetMoveEntry(byte seq)
		{
			return m_MoveInfo[seq];
		}

		private static Timer m_OpenDoorReq = Timer.DelayedCallback(TimeSpan.FromSeconds(0.005), new TimerCallback(OpenDoor));

		private static void OpenDoor()
		{
			if (World.Player != null)
				Assistant.Client.Instance.SendToServer(new OpenDoorMacro());
		}

		private Serial m_LastDoor = Serial.Zero;
		private DateTime m_LastDoorTime = DateTime.MinValue;

		internal void MoveReq(Direction dir, byte seq)
		{
			m_OutstandingMoves++;
			FastWalkKey++;

			MoveEntry e = new MoveEntry();

			if (!m_MoveInfo.ContainsKey(seq))
				m_MoveInfo.TryAdd(seq, e);
			else
				m_MoveInfo[seq] = e;

			e.IsStep = (dir & Direction.Mask) == (Direction & Direction.Mask);
			e.Dir = dir;

			ProcessMove(dir);

			e.Position = Position;

			if (Body != 0x03DB && !IsGhost && ((int)(e.Dir & Direction.Mask)) % 2 == 0 && Engine.MainWindow.AutoOpenDoors.Checked && CheckHiddedOpenDoor())
			{
				int x = Position.X, y = Position.Y;
				Utility.Offset(e.Dir, ref x, ref y);

				int z = CalcZ;

				foreach (Item i in World.Items.Values)
				{
					if (i.Position.X == x && i.Position.Y == y)
						if (i.IsDoor)
							if (i.Position.Z - 15 <= z && i.Position.Z + 15 >= z)
								if (m_LastDoor != i.Serial || m_LastDoorTime + TimeSpan.FromSeconds(1) < DateTime.Now)
								{
									m_LastDoor = i.Serial;
									m_LastDoorTime = DateTime.Now;
									m_OpenDoorReq.Start();
									break;
								}
				}
			}

			e.FilterAck = false;

			m_WalkSeq = (byte)(seq >= 255 ? 1 : seq + 1);
		}

		internal bool CheckHiddedOpenDoor()
		{
			if (Visible)
				return true;
			else
			{
				if (RazorEnhanced.Settings.General.ReadBool("HiddedAutoOpenDoors"))
					return false;
				else
					return true;
			}
		}

		internal void ProcessMove(Direction dir)
		{
			if ((dir & Direction.Mask) == (this.Direction & Direction.Mask))
			{
				int x = Position.X, y = Position.Y;

				Utility.Offset(dir & Direction.Mask, ref x, ref y);

				int newZ = Position.Z;
				try { newZ = Assistant.Facet.ZTop(Map, x, y, newZ); }
				catch { }
				Position = new Point3D(x, y, newZ);
			}
			Direction = dir;
		}

		private int walkScriptRequest = 0;
		internal int WalkScriptRequest {
			get { return walkScriptRequest; }
			set
			{
                //Console.WriteLine("walkScriptRequest - {0}", value);
                //if ( (walkScriptRequest < 2) && (value >= 2))
                //	System.Threading.Thread.Sleep(500);
                walkScriptRequest = value;
			}
		}

		internal bool HasWalkEntry(byte seq)
		{
			return m_MoveInfo[seq] != null;
		}

		internal void MoveRej(byte seq, Direction dir, Point3D pos)
		{
			m_OutstandingMoves--;

			Direction = dir;
			Position = pos;
			Resync();
		}

        internal void CheckCorpseOpen()
        {
            if (Targeting.HasTarget)
            {
                // if not marked open, we'll get them when he isn't targeting
                return;
            }
            bool enabled = RazorEnhanced.Settings.General.ReadBool("AutoOpenCorpses");
            if (!enabled)
            {
                return;
            }
            int range = RazorEnhanced.Settings.General.ReadInt("CorpseRange");
            List<CorpseItem> list = World.CorpsesInRange(range);

            foreach (CorpseItem corpse in list)
            {
                if (! corpse.Opened)
                {
                    corpse.Opened = true;
                    Assistant.Client.Instance.SendToServer(new DoubleClick(corpse.Serial));
                    //System.Threading.Thread.Sleep(500);
                }
            }
        }

        internal bool MoveAck(byte seq)
		{
            CheckCorpseOpen();

            m_OutstandingMoves--;

			MoveEntry e;
			m_MoveInfo.TryGetValue(seq, out e);
			if (e != null)
			{
				if (e.IsStep && !IsGhost)
					StealthSteps.OnMove();

				return !e.FilterAck;
			}
			else
				return true;
		}

		private static bool m_ExternZ = false;
		internal static bool ExternalZ { get { return m_ExternZ; } set { m_ExternZ = value; } }

		//private sbyte m_CalcZ = 0;
		internal int CalcZ
		{
			get
			{
				if (!m_ExternZ || !DLLImport.Razor.IsCalibrated())
					return Assistant.Facet.ZTop(Map, Position.X, Position.Y, Position.Z);
				else
					return Position.Z;
			}
		}


		internal override void OnPositionChanging(Point3D newPos)
		{
			List<Mobile> mobiles = new List<Mobile>(World.Mobiles.Values);

			foreach (Mobile m in mobiles)
			{
				if (m != this)
				{
					if (!Utility.InRange(m.Position, newPos, VisRange))
						m.Remove();
					else
						Targeting.CheckLastTargetRange(m);
				}
			}

			List<Item> items = new List<Item>(World.Items.Values);
			foreach (Item item in items)
			{
				if (item.Deleted || item.Container != null)
					continue;

				int dist = Utility.Distance(item.GetWorldPosition(), newPos);
				if (item != DragDropManager.Holding && (dist > MultiVisRange || (!item.IsMulti && dist > VisRange)))
					item.Remove();
			}

			base.OnPositionChanging(newPos);
		}

		internal override void OnMapChange(byte old, byte cur)
		{
			List<Mobile> list = new List<Mobile>(World.Mobiles.Values);
			foreach (Mobile t in list)
			{
				if (t != this && t.Map != cur)
					t.Remove();
			}

			List<Item> itemlist = new List<Item>(World.Items.Values);

			foreach (Item i in itemlist)
			{
				if (i.RootContainer != World.Player)
					i.Remove();
			}

		/*	World.Items.Clear();
			for (int i = 0; i < Contains.Count; i++)
			{
				Item item = (Item)Contains[i];
				World.AddItem(item);
				item.Contains.Clear();
			}*/


			if (RazorEnhanced.Settings.General.ReadBool("AutoSearch") && Backpack != null)
				PlayerData.DoubleClick(Backpack);

	 		Assistant.UOAssist.PostMapChange(cur);
			m_HandCheck.Start();
		}

		protected override void OnNotoChange(byte old, byte cur)
		{
			if ((old == 3 || old == 4) && (cur != 3 && cur != 4))
			{
				// grey is turning off
				// SendMessage( "You are no longer a criminal." );
				if (m_CriminalTime != null)
					m_CriminalTime.Stop();
				m_CriminalStart = DateTime.MinValue;
			}
			else if ((cur == 3 || cur == 4) && (old != 3 && old != 4 && old != 0))
			{
				// grey is turning on
				ResetCriminalTimer();
			}
		}

		internal void ResetCriminalTimer()
		{
			if (m_CriminalStart == DateTime.MinValue || DateTime.Now - m_CriminalStart >= TimeSpan.FromSeconds(1))
			{
				m_CriminalStart = DateTime.Now;
				if (m_CriminalTime == null)
					m_CriminalTime = new CriminalTimer(this);
				m_CriminalTime.Start();
			}
		}

		private class CriminalTimer : Timer
		{
			private PlayerData m_Player;

			internal CriminalTimer(PlayerData player)
				: base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
			{
				m_Player = player;
			}

			protected override void OnTick()
			{
			}
		}

		internal void SendMessage(MsgLevel lvl, LocString loc, params object[] args)
		{
			SendMessage(lvl, Language.Format(loc, args));
		}

		internal void SendMessage(MsgLevel lvl, LocString loc)
		{
			SendMessage(lvl, Language.GetString(loc));
		}

		internal void SendMessage(LocString loc, params object[] args)
		{
			SendMessage(MsgLevel.Info, Language.Format(loc, args));
		}

		internal void SendMessage(LocString loc)
		{
			SendMessage(MsgLevel.Info, Language.GetString(loc));
		}

		/*internal void SendMessage( int hue, LocString loc, params object[] args )
		{
			SendMessage( hue, Language.Format( loc, args ) );
		}*/

		internal void SendMessage(MsgLevel lvl, string format, params object[] args)
		{
			SendMessage(lvl, String.Format(format, args));
		}

		internal void SendMessage(string format, params object[] args)
		{
			SendMessage(MsgLevel.Info, String.Format(format, args));
		}

		internal void SendMessage(string text)
		{
			SendMessage(MsgLevel.Info, text);
		}

		internal void SendMessage(MsgLevel lvl, string text)
		{
			if (lvl < (MsgLevel) RazorEnhanced.Settings.General.ReadInt("MessageLevel") || text.Length <= 0)
				return;

			int hue;
			switch (lvl)
			{
				case MsgLevel.Error:
				case MsgLevel.Warning:
					hue = Engine.MainWindow.WarningColor;
					break;

				default:
					hue = Engine.MainWindow.SysColor;
					break;
			}
	 		Assistant.Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3, Language.CliLocName, "System", text));

			PacketHandlers.SysMessages.Add(text.ToLower());

			if (PacketHandlers.SysMessages.Count >= 25)
				PacketHandlers.SysMessages.RemoveRange(0, 10);
		}

		internal uint CurrentGumpS, CurrentGumpI;
		internal bool HasGump;
		internal bool HasPrompt;
		internal bool HasContext;
		internal uint ContextID;
		internal uint PromptSenderSerial;
		internal uint PromptID;
		internal uint PromptType;
		internal List<int> CurrentGumpTile = new List<int>();
		internal List<string> CurrentGumpStrings = new List<string>();
		internal string CurrentGumpRawData;
		internal ConcurrentQueue<RazorEnhanced.Journal.JournalEntry> Journal = new ConcurrentQueue<RazorEnhanced.Journal.JournalEntry>();
		internal uint LastWeaponRight, LastWeaponLeft = 0;

		// Menu Old
		internal uint CurrentMenuS;
		internal ushort CurrentMenuI;
		internal bool HasMenu;
		internal string MenuQuestionText;

		// Special ability
		internal volatile bool HasSpecial = false;

		internal class MenuItem
		{
			private ushort m_modelID;
			public ushort ModelID { get { return m_modelID; } }

			private ushort m_modelColor;
			public ushort ModelColor { get { return m_modelColor; } }

			private string m_modelText;
			public string ModelText { get { return m_modelText; } }

			public MenuItem(ushort modelid, ushort modelcolor, string modeltext)
			{
				m_modelID = modelid;
				m_modelColor = modelcolor;
				m_modelText = modeltext;
			}
		}

		internal List<MenuItem> MenuEntry = new List<MenuItem>();

		// Query String
		internal bool HasQueryString;
		internal int QueryStringID;
		internal byte QueryStringType;
		internal byte QueryStringIndex;

		private ushort m_SpeechHue;
		internal ushort SpeechHue { get { return m_SpeechHue; } set { m_SpeechHue = value; } }

		internal sbyte LocalLightLevel { get { return m_LocalLight; } set { m_LocalLight = value; } }
		internal byte GlobalLightLevel { get { return m_GlobalLight; } set { m_GlobalLight = value; } }
		internal byte Season { get { return m_Season; } set { m_Season = value; } }
		internal ushort Features { get { return m_Features; } set { m_Features = value; } }
		internal int[] MapPatches { get { return m_MapPatches; } set { m_MapPatches = value; } }

		private int m_LastSkill = -1;
		internal int LastSkill { get { return m_LastSkill; } set { m_LastSkill = value; } }

		private Serial m_LastObj = Serial.Zero;
		internal Serial LastObject { get { return m_LastObj; } set { m_LastObj = value; } }

		private int m_LastSpell = -1;
		internal int LastSpell { get { return m_LastSpell; } set { m_LastSpell = value; } }


		//private UOEntity m_LastCtxM = null;
		//public UOEntity LastContextMenu { get { return m_LastCtxM; } set { m_LastCtxM = value; } }

		internal static bool DoubleClick(object clicked)
		{
			return DoubleClick(clicked, true);
		}

		internal static bool DoubleClick(object clicked, bool silent)
		{
			Serial s;
			if (clicked is Mobile)
				s = ((Mobile)clicked).Serial.Value;
			else if (clicked is Item)
				s = ((Item)clicked).Serial.Value;
			else if (clicked is Serial)
				s = ((Serial)clicked).Value;
			else
				s = Serial.Zero;

			if (s == Serial.Zero)
				return false;

			Item free = null, pack = World.Player.Backpack;
			if (s.IsItem && pack != null && RazorEnhanced.Settings.General.ReadBool("PotionEquip"))
			{
				Item i = World.FindItem(s);
				if (i != null && i.IsPotion && i.ItemID != 3853) // dont unequip for exploison potions
				{
					// dont worry about uneqipping RuneBooks or SpellBooks
					Item left = World.Player.GetItemOnLayer(Layer.LeftHand);
					Item right = World.Player.GetItemOnLayer(Layer.RightHand);

					if (left != null && (right != null || left.IsTwoHanded))
						free = left;
					else if (right != null && right.IsTwoHanded)
						free = right;

					if (free != null)
					{
						if (DragDropManager.HasDragFor(free.Serial))
							free = null;
						else
							DragDropManager.DragDrop(free, pack);
					}
				}
			}

			ActionQueue.DoubleClick(silent, s);

			if (free != null)
				DragDropManager.DragDrop(free, World.Player, free.Layer, true);

			if (s.IsItem)
				World.Player.m_LastObj = s;

			return false;
		}

		// Set last weapon on login
		private Timer m_HandCheck = Timer.DelayedCallback(TimeSpan.FromSeconds(3.0), new TimerCallback(HandCheck));

		private static void HandCheck()
		{
			if (World.Player == null)
				return;

			Item righthand = World.Player.GetItemOnLayer(Layer.RightHand);
			if (righthand != null)
				World.Player.LastWeaponRight = righthand.Serial;

			Item lefthand = World.Player.GetItemOnLayer(Layer.LeftHand);
			if (lefthand != null)
				World.Player.LastWeaponRight = lefthand.Serial;
		}


	}
}
