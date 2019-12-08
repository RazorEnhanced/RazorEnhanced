using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace Assistant
{
	internal class FeatureBit
	{
		internal static readonly uint WeatherFilter = 1 << 0; // Weather Filter
		internal static readonly uint LightFilter = 1 << 1;// Light Filter
		internal static readonly uint SmartLT = 1 << 2; // Smart Last Target
		internal static readonly uint RangeCheckLT = 1 << 3;// Range Check Last Target
		internal static readonly uint AutoOpenDoors = 1 << 4; // Automatically Open Doors
		internal static readonly uint UnequipBeforeCast = 1 << 5; // Unequip Weapon on spell cast
		internal static readonly uint AutoPotionEquip = 1 << 6; // Un/re-equip weapon on potion use
		internal static readonly uint BlockHealPoisoned = 1 << 7; // Block heal If poisoned/Macro If Poisoned condition/Heal or Cure self
		internal static readonly uint LoopingMacros = 1 << 8; // Disallow looping or recursive macros
		internal static readonly uint UseOnceAgent = 1 << 9;// The use once agent
		internal static readonly uint RestockAgent = 1 << 10;// The restock agent
		internal static readonly uint SellAgent = 1 << 11;// The sell agent
		internal static readonly uint BuyAgent = 1 << 12;// The buy agent
		internal static readonly uint PotionHotkeys = 1 << 13;// All potion hotkeys
		internal static readonly uint RandomTargets = 1 << 14;// All random target hotkeys (not target next, last target, target self)
		internal static readonly uint ClosestTargets = 1 << 15; // All closest target hotkeys
		internal static readonly uint OverheadHealth = 1 << 16;// Health and Mana/Stam messages shown over player's heads
		internal static readonly uint AutolootAgent = 1 << 17; // The autoloot agent
		internal static readonly uint BoneCutterAgent = 1 << 18; // The bone cutter agent
		internal static readonly uint AdvancedMacros = 1 << 19; // Advanced macro engine
		internal static readonly uint AutoRemount = 1 << 20; // Auto remount after dismount
		internal static readonly uint AutoBandage = 1 << 21; // Auto bandage friends, self, last and mount option
		internal static readonly uint EnemyTargetShare = 1 << 22; // Enemy target share on guild, party or alliance chat
		internal static readonly uint FilterSeason = 1 << 23; // Season Filter
		internal static readonly uint SpellTargetShare = 1 << 24; // Spell target share on guild, party or alliance chat

		internal static readonly uint MaxBit = 24;
	}

	public abstract class Client
	{
		public static Client Instance;
		public static bool IsOSI;

		internal const int WM_USER = 0x400;
		public enum Loader_Error
		{
			SUCCESS = 0,
			NO_OPEN_EXE,
			NO_MAP_EXE,
			NO_READ_EXE_DATA,

			NO_RUN_EXE,
			NO_ALLOC_MEM,

			NO_WRITE,
			NO_VPROTECT,
			NO_READ,

			UNKNOWN_ERROR = 99
		};

		public abstract bool InstallHooks(IntPtr mainWindow);

		internal static void Init(bool isOSI)
		{
			IsOSI = isOSI;
			Instance = new OSIClient();		
		}

		private ulong m_Features = 0;

		public bool AllowBit(uint bit)
		{
			return (m_Features & (1U << (int)bit)) == 0;
		}

		public void SetFeatures(ulong features)
		{
			m_Features = features;
		}

		public abstract bool ServerEncrypted { get; set; }
		public abstract bool ClientEncrypted { get; set; }

		public abstract bool Ready { get;  }
		public abstract Process ClientProcess { get; }

		public abstract DateTime ConnectionStart { get; }
		public abstract IPAddress LastConnection { get; }
		public abstract  bool ClientRunning { get; }



		public abstract void SendToServer(Packet p);
		public abstract void SendToServerWait(Packet p);
		public abstract void SendToServer(PacketReader pr);
		public abstract void SendToClientWait(Packet p);
		public abstract void SendToClient(Packet p);
		public abstract void SendToClient(PacketReader pr);
		public abstract void ForceSendToClient(Packet p);
		public abstract void ForceSendToServer(Packet p);

		public abstract void InitSendFlush();
		public abstract Packet MakePacketFrom(PacketReader pr);
		public abstract void PostTextSend(string text);

		public abstract void SetCustomNotoHue(int hue);
		public abstract void RequestStatbarPatch(bool preAOS);
		public abstract void SetSmartCPU(bool enabled);
		public abstract void SetGameSize(int x, int y);
		public abstract void SetNegotiate(bool negotiate);

		public abstract void SetTitleStr(string str);
		public abstract bool OnMessage(MainForm razor, uint wParam, int lParam);

		public abstract void PostRemoveMulti(Item item);
		public abstract void PostAddMulti(ItemID iid, Point3D Position);
		public abstract void PostMapChange(int map);
		public abstract void PostSkillUpdate(int skill, int val);

		public abstract void PostStamUpdate();
		public abstract void PostManaUpdate();
		public abstract void PostHitsUpdate();
		public abstract void PostLogin(int serial);

		public abstract void SetConnectionInfo(IPAddress addr, int port);
		public abstract Loader_Error LaunchClient(string client);
		public abstract bool Attach(int pid);
		public abstract void Close();

		public abstract void BeginCalibratePosition();
		public abstract void PostSpellCast(int spell);

		public abstract int OnUOAMessage(MainForm razor, int Msg, int wParam, int lParam);
	}
}