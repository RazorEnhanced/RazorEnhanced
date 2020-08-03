using Assistant;
using JsonData;
using System.Collections.Generic;
using System.Threading;

namespace RazorEnhanced
{
	public class Target
	{
		private int m_ptarget;
		private RazorEnhanced.Point3D m_pgtarget;

		public static bool HasTarget()
		{
			return Assistant.Targeting.HasTarget;
		}

		public static void WaitForTarget(int delay, bool noshow = false)
		{
			int subdelay = delay;
			Assistant.Targeting.NoShowTarget = noshow;
			while (Assistant.Targeting.HasTarget == false)
			{
				Thread.Sleep(2);
				subdelay -= 2;
				if (subdelay <= 0)
					break;
			}
			Assistant.Targeting.NoShowTarget = false;
		}

		public static void TargetExecute(int serial)
		{
			if (!CheckHealPoisonTarg(serial))
			{
				Assistant.Targeting.Target(serial, true);
			}
		}

		public static void TargetExecute(RazorEnhanced.Item item)
		{
			Assistant.Targeting.Target(item.Serial, true);
		}

		public static void TargetExecute(RazorEnhanced.Mobile mobile)
		{
			if (!CheckHealPoisonTarg(mobile.Serial))
			{
				Assistant.Targeting.Target(mobile.Serial, true);
			}
		}

		public static void TargetExecuteRelative(int serial, int offset)
		{
			Mobile m = Mobiles.FindBySerial(serial);
			if (m != null)
				TargetExecuteRelative(m, offset);
		}

		public static void TargetExecuteRelative(Mobile m, int offset)
		{
			Assistant.Point2D relpos = new Assistant.Point2D();
			switch (m.Direction)
				{
				case "North":
					relpos.X = m.Position.X;
					relpos.Y = m.Position.Y - offset;
					break;
				case "South":
					relpos.X = m.Position.X;
					relpos.Y = m.Position.Y + offset;
					break;
				case "West":
					relpos.X = m.Position.X - offset;
					relpos.Y = m.Position.Y;
					break;
				case "East":
					relpos.X = m.Position.X + offset;
					relpos.Y = m.Position.Y;
					break;
				case "Up":
					relpos.X = m.Position.X - offset;
					relpos.Y = m.Position.Y - offset;
					break;
				case "Down":
					relpos.X = m.Position.X + offset;
					relpos.Y = m.Position.Y + offset;
					break;
				case "Left":
					relpos.X = m.Position.X - offset;
					relpos.Y = m.Position.Y + offset;
					break;
				case "Right":
					relpos.X = m.Position.X + offset;
					relpos.Y = m.Position.Y - offset;
					break;
			}
			Assistant.Point3D location = new Assistant.Point3D(relpos.X, relpos.Y, Statics.GetLandZ(relpos.X, relpos.Y, Player.Map));
			Assistant.Targeting.Target(location, true);
		}

		public static void TargetExecute(int x, int y, int z)
		{
			Assistant.Point3D location = new Assistant.Point3D(x, y, z);
			Assistant.Targeting.Target(location, true);
		}

		public static void TargetExecute(int x, int y, int z, int gfx)
		{
			Assistant.Point3D location = new Assistant.Point3D(x, y, z);
			Assistant.Targeting.Target(location, gfx, true);
		}

		public static void Cancel()
		{
			//Assistant.Targeting.CancelClientTarget(true);
			Assistant.Targeting.CancelOneTimeTarget(true);
		}

		public static void Self()
		{
			if (World.Player != null)
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
				SetLast(mob.Serial);
		}

		public static void SetLast(int serial, bool wait = true)
		{
			TargetMessage(serial, wait); // Process message for highlight
			Assistant.Targeting.SetLastTarget(serial, 0, wait);
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
			Misc.SendMessage(message, 945, true);
			Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback(PromptTargetExex_Callback));

			while (!Targeting.HasTarget)
				Thread.Sleep(30);

			while (m_ptarget == -1 && Targeting.HasTarget)
				Thread.Sleep(30);

			Thread.Sleep(100);

			if (m_ptarget == -1)
				Misc.SendMessage("Prompt Target Cancelled", 945, true);

			return m_ptarget;
		}

		private void PromptTargetExex_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			m_ptarget = serial;
		}

		public Point3D PromptGroundTarget(string message = "Select Ground Position")
		{
			m_pgtarget = Point3D.MinusOne;

			Misc.SendMessage(message, 945, true);
			Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback(PromptGroundTargetExex_Callback));

			while (!Targeting.HasTarget)
				Thread.Sleep(30);

			while (m_pgtarget.X == -1 && Targeting.HasTarget)
				Thread.Sleep(30);

			Thread.Sleep(100);

			if (m_pgtarget.X == -1)
				Misc.SendMessage("Prompt Gorund Target Cancelled", 945, true);

			return m_pgtarget;
		}

        private void PromptGroundTargetExex_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
        {
            if (!loc)
            {
                Mobile target = Mobiles.FindBySerial(serial);
                if (target == null)
                {
                    m_pgtarget = Point3D.MinusOne;
}
                else {
                    m_pgtarget = target.Position;
                }
            }
            else
                m_pgtarget = new Point3D(pt.X, pt.Y, pt.Z);
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
			if (mob == null)
				return 0;

			return m_NotoHues[mob.Notoriety];
		}
		public static void SetLastTargetFromList(string targetid)
		{
			TargetGUI targetdata = Settings.Target.TargetRead(targetid);
			if (targetdata != null)
			{
				Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();			
				string selector = targetdata.TargetGuiObject.Selector;

				List<Mobile> filterresult;
				filterresult = Mobiles.ApplyFilter(filter);

				Mobile mobtarget = Mobiles.Select(filterresult, selector);
				if (mobtarget == null)
					return;

				RazorEnhanced.Target.SetLast(mobtarget);
			}
		}

		public static Mobile GetTargetFromList(string targetid)
		{
			TargetGUI targetdata = Settings.Target.TargetRead(targetid);
			if (targetdata == null)
				return null;

			
			Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
			string selector = targetdata.TargetGuiObject.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);
			if (mobtarget == null)
				return null;

			return mobtarget;
		}

		internal static void SetLastTargetFromListHotKey(string targetid)
		{
			TargetGUI targetdata = Settings.Target.TargetRead(targetid);

			if (targetdata == null)
				return;

			Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
			string selector = targetdata.TargetGuiObject.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);

			if (mobtarget == null)
				return;

			TargetMessage(mobtarget.Serial, false); // Process message for highlight

			Assistant.Mobile mobile = World.FindMobile(mobtarget.Serial);
			if (mobile != null)
				Targeting.SetLastTarget(mobile.Serial, 0, false);
		}

		public static void PerformTargetFromList(string targetid)
		{
			TargetGUI targetdata = Settings.Target.TargetRead(targetid);

			if (targetdata == null)
				return;

			Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
			string selector = targetdata.TargetGuiObject.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);

			if (mobtarget == null)
				return;

			TargetExecute(mobtarget.Serial);
			SetLast(mobtarget);
		}

		public static void AttackTargetFromList(string targetid)
		{
			TargetGUI targetdata = Settings.Target.TargetRead(targetid);

			if (targetdata == null)
				return;

			Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
			string selector = targetdata.TargetGuiObject.Selector;

			List<Mobile> filterresult;
			filterresult = Mobiles.ApplyFilter(filter);

			Mobile mobtarget = Mobiles.Select(filterresult, selector);

			if (mobtarget == null)
				return;

			AttackMessage(mobtarget.Serial, true); // Process message for highlight
			if (Targeting.LastAttack != mobtarget.Serial)
			{
		 		Assistant.Client.Instance.SendToClientWait(new ChangeCombatant(mobtarget.Serial));
				Targeting.LastAttack = (uint)mobtarget.Serial;
			}
	 		Assistant.Client.Instance.SendToServerWait(new AttackReq(mobtarget.Serial)); // Real attack
		}

		internal static void TargetMessage(int serial, bool wait)
		{
			if (Assistant.Engine.MainWindow.ShowHeadTargetCheckBox.Checked)
			{
				if (Friend.IsFriend(serial))
					Mobiles.Message(World.Player.Serial, 63, "Target: [" + GetPlayerName(serial) + "]", wait);
				else
					Mobiles.Message(World.Player.Serial, GetPlayerColor(Mobiles.FindBySerial(serial)), "Target: [" + GetPlayerName(serial) + "]", wait);
			}

			if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
				Mobiles.Message(serial, 10, "* Target *", wait);
		}

		internal static void AttackMessage(int serial, bool wait)
		{
			if (Assistant.Engine.MainWindow.ShowHeadTargetCheckBox.Checked)
			{
				if (Friend.IsFriend(serial))
					Mobiles.Message(World.Player.Serial, 63, "Attack: [" + GetPlayerName(serial) + "]", wait);
				else
					Mobiles.Message(World.Player.Serial, GetPlayerColor(Mobiles.FindBySerial(serial)), "Attack: [" + GetPlayerName(serial) + "]", wait);
			}

			if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
				Mobiles.Message(serial, 10, "* Target *", wait);
		}

	}
}