using Assistant;
using System.Collections.Generic;
using System.Threading;

namespace RazorEnhanced
{
	public class Target
	{
		private int m_ptarget;

		//internal static bool TargetMessage = false;

		public static bool HasTarget()
		{
			return Assistant.Targeting.HasTarget;
		}

		public static void WaitForTarget(int delay) // Delay in MS
		{
			int subdelay = delay;
			while (Assistant.Targeting.HasTarget == false)
			{
				Thread.Sleep(2);
				subdelay -= 2;
				if (subdelay <= 0)
					break;
			}
		}

		public static void WaitForTarget(int delay, bool noshow)
		{
			if (!noshow)
				WaitForTarget(delay);
			else
			{
				int subdelay = delay;
				Assistant.Targeting.NoShowTarget = true;
				while (Assistant.Targeting.HasTarget == false)
				{
					Thread.Sleep(2);
					subdelay -= 2;
					if (subdelay <= 0)
						break;
				}
				Assistant.Targeting.NoShowTarget = false;
			}
		}

		public static void TargetExecute(int serial)
		{
			if (!CheckHealPoisonTarg(serial))
			{
				Assistant.Targeting.TargetByScript(serial);
			}
		}

		public static void TargetExecute(RazorEnhanced.Item item)
		{
			Assistant.Targeting.TargetByScript(item.Serial);
		}

		public static void TargetExecute(RazorEnhanced.Mobile mobile)
		{
			if (!CheckHealPoisonTarg(mobile.Serial))
			{
				Assistant.Targeting.TargetByScript(mobile.Serial);
			}
		}

		public static void TargetExecute(int x, int y, int z)
		{
			Assistant.Point3D location = new Assistant.Point3D(x, y, z);
			Assistant.Targeting.TargetByScript(location);
		}

		public static void TargetExecute(int x, int y, int z, int gfx)
		{
			Assistant.Point3D location = new Assistant.Point3D(x, y, z);
			Assistant.Targeting.TargetByScript(location, gfx);
		}

		public static void Cancel()
		{
			Assistant.Targeting.CancelClientTargetByScript();
			Assistant.Targeting.CancelOneTimeTargetByScript();
		}

		public static void Self()
		{
			TargetExecute(World.Player.Serial);
		}

		public static void SelfQueued()
		{
			Assistant.Targeting.TargetSelf(true);
		}

		public static void Last()
		{
			if (!CheckHealPoisonTarg(GetLast()))
				Assistant.Targeting.LastTarget();
		}

		public static void LastQueued()
		{
			Assistant.Targeting.LastTarget(true);
		}

		public static int GetLast()
		{
			return (int)Assistant.Targeting.GetLastTarger;
		}

		public static int GetLastAttack()
		{
			return (int)Assistant.Targeting.LastAttack;
		}

		public static void SetLast(RazorEnhanced.Mobile mob)
		{
			Assistant.Mobile mobile = World.FindMobile(mob.Serial);
			if (mobile != null)
				Assistant.Targeting.SetLastTargetWait(mobile, 0);
		}

		public static void SetLast(int serial)
		{
			Assistant.Mobile mobile = World.FindMobile(serial);
			if (mobile != null)
				Assistant.Targeting.SetLastTargetWait(mobile, 0);
		}

		public static void ClearQueue()
		{
			Assistant.Targeting.ClearQueue();
		}

		public static void ClearLast()
		{
			Assistant.Targeting.ClearLast();
		}

		public static void ClearLastandQueue()
		{
			Assistant.Targeting.ClearQueue();
			Assistant.Targeting.ClearLast();
		}

		public int PromptTarget(string message = "Select Item or Mobile")
		{
			m_ptarget = -1;
			Misc.SendMessage(message, 945);
			Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback(PromptTargetExex_Callback));

			while (!Targeting.HasTarget)
				Thread.Sleep(2);

			while (m_ptarget == -1 && Targeting.HasTarget)
				Thread.Sleep(30);

			Thread.Sleep(100);

			if (m_ptarget == -1)
				Misc.SendMessage("PromptTarget Cancelled", 945);

			return m_ptarget;
		}

		private void PromptTargetExex_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			m_ptarget = serial;
		}

		// Check Poison 
		private static bool CheckHealPoisonTarg(Assistant.Serial ser)
		{
			if (World.Player == null)
				return false;

			if (!RazorEnhanced.Settings.General.ReadBool("BlockHealPoison"))
				return false;

			if (ser.IsMobile && (World.Player.LastSpell == Spell.ToID(1, 4) || World.Player.LastSpell == Spell.ToID(4, 5) || World.Player.LastSpell == 202))
			{
				Assistant.Mobile m = World.FindMobile(ser);

				if (m != null && m.Poisoned)
				{
					World.Player.SendMessage(MsgLevel.Warning, "Heal blocked, Target is poisoned!");
					return true;
				}
				else if (m != null && m.Blessed)
				{
					World.Player.SendMessage(MsgLevel.Warning, "Heal blocked, Target is mortelled!");
					return true;
				}
				return false;
			}
			else
				return false;
		}

		// Funzioni target per richiamare i target della gui

		private static string GetPlayerName(int s)
		{
			Assistant.Mobile mob = World.FindMobile(s);
			return mob != null ? mob.Name : string.Empty;
		}

		private static int[] m_NotoHues = new int[8]
		{
			1, // black		unused 0
			0x059, // blue		0x0059 1
			0x03F, // green		0x003F 2
			0x3B2, // greyish	0x03b2 3
			0x3B2, // grey		   "   4
			0x090, // orange		0x0090 5
			0x022, // red		0x0022 6
			0x035, // yellow		0x0035 7
		};

		private static int GetPlayerColor(Mobile mob)
		{
			return m_NotoHues[mob.Notoriety];
		}
		public static void SetLastTargetFromList(string targetid)
		{
			TargetGUI.TargetGUIObject targetdata = Settings.Target.TargetRead(targetid);
			if (targetdata != null)
			{
				Mobiles.Filter filter = targetdata.Filter;
				string selector = targetdata.Selector;

				List<Mobile> filterresult;
				filterresult = Mobiles.ApplyFilter(filter);

				Mobile mobtarget = Mobiles.Select(filterresult, selector);
				if (mobtarget == null)
					return;

				TargetMessage(mobtarget.Serial); // Process message for highlight

				RazorEnhanced.Target.SetLast(mobtarget);
			}
		}

		internal static void SetLastTargetFromListHotKey(string targetid)
		{
			TargetGUI.TargetGUIObject targetdata = Settings.Target.TargetRead(targetid);

			if (targetdata == null)
				return;

			Mobiles.Filter filter = targetdata.Filter;
			string selector = targetdata.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);

			if (mobtarget == null)
				return;

			TargetMessage(mobtarget.Serial); // Process message for highlight

			Assistant.Mobile mobile = World.FindMobile(mobtarget.Serial);
			if (mobile != null)
				Targeting.SetLastTargetWait(mobile, 0);
		}

		public static void PerformTargetFromList(string targetid)
		{
			TargetGUI.TargetGUIObject targetdata = Settings.Target.TargetRead(targetid);

			if (targetdata == null)
				return;

			Mobiles.Filter filter = targetdata.Filter;
			string selector = targetdata.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);

			if (mobtarget == null)
				return;

			TargetMessage(mobtarget.Serial); // Process message for highlight

			TargetExecute(mobtarget.Serial);
			SetLast(mobtarget);
		}

		public static void AttackTargetFromList(string targetid)
		{
			TargetGUI.TargetGUIObject targetdata = Settings.Target.TargetRead(targetid);

			if (targetdata == null)
				return;

			Mobiles.Filter filter = targetdata.Filter;
			string selector = targetdata.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);

			if (mobtarget == null)
				return;

			AttackMessage(mobtarget.Serial); // Process message for highlight

			RazorEnhanced.Player.Attack(mobtarget.Serial); // Real attack
		}

		internal static void TargetMessage(int serial)
		{
			if (Assistant.Engine.MainWindow.ShowHeadTargetCheckBox.Checked)
			{
				if (Friend.IsFriend(serial))
					Assistant.ClientCommunication.SendToClientWait(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 63, 3, Language.CliLocName, World.Player.Name, "Target: [" + GetPlayerName(serial) + "]"));
				else
					Assistant.ClientCommunication.SendToClientWait(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, GetPlayerColor(Mobiles.FindBySerial(serial)), 3, Language.CliLocName, World.Player.Name, "Target: [" + GetPlayerName(serial) + "]"));
			}

			if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
				Mobiles.Message(serial, 10, "* Target *");
		}

		internal static void AttackMessage(int serial)
		{
			if (Assistant.Engine.MainWindow.ShowHeadTargetCheckBox.Checked)
			{
				if (Friend.IsFriend(serial))
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 63, 3, Language.CliLocName, World.Player.Name, "Attack: [" + GetPlayerName(serial) + "]"));
				else
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, GetPlayerColor(Mobiles.FindBySerial(serial)), 3, Language.CliLocName, World.Player.Name, "Attack: [" + GetPlayerName(serial) + "]"));
			}

			if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
				Mobiles.Message(serial, 10, "* Target *");
		}

	}
}