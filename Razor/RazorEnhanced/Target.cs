using System;
using System.IO;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;

namespace RazorEnhanced
{
	public class Target
	{
        private int m_ptarget;
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
        public static void TargetExecute(int serial) 
        {
            Assistant.Targeting.Target(serial);
        }

        public static void TargetExecute(RazorEnhanced.Item item)
        {
            Assistant.Targeting.Target(item);
        }
        public static void TargetExecute(RazorEnhanced.Mobile mobile)
        {
            Assistant.Targeting.Target(mobile);
        }

        public static void TargetExecute(Point3D location) 
        {
            Assistant.Targeting.Target(location);
        }
        public static void TargetExecute(int x, int y, int z)
        {
            Assistant.Point3D location = new Assistant.Point3D(x, y, z);
            Assistant.Targeting.Target(location);
        }

        public static void Cancel()
        {
            Assistant.Targeting.CancelOneTimeTarget();
        }

        public static void Self()
        {
            Assistant.Targeting.Target(World.Player);
        }

        public static void Last()
        {
            Assistant.Targeting.LastTarget();
        }

        public static int GetLast()
        {
            return (int)Assistant.Targeting.GetLastTarger;
        }
        public static void SetLast(RazorEnhanced.Mobile mob)
        {
            Assistant.Mobile mobile = World.FindMobile(mob.Serial);
            Assistant.Targeting.SetLastTargetTo(mobile);
        }
        public static void SetLast(int serial)
        {
            Assistant.Mobile mobile = World.FindMobile(serial);
            if (mobile!= null)
                Assistant.Targeting.SetLastTargetTo(mobile);
        }

        public int PromptTarget()
        {
            m_ptarget = -1;
            Misc.SendMessage("Select Item or Mobile");
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(PromptTargetExex_Callback));
            
            while (m_ptarget == -1)
                Thread.Sleep(30);

            return m_ptarget;
        }

        private void PromptTargetExex_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
        {
            m_ptarget = serial;
        }
	}
}
