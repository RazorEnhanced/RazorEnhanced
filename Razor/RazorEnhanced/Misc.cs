using System;
using System.Media;
using System.Collections.Generic;
using Assistant;


namespace RazorEnhanced
{
	public class Misc
	{
		//General
		public static void Pause(int mseconds)
		{
            System.Threading.Thread.Sleep(mseconds);
		}

		public static void Resync()
		{
			Assistant.ClientCommunication.SendToServer(new ResyncReq());
		}

		public static double DistanceSqrt(Point3D a, Point3D b)
		{
			double distance = Math.Sqrt(((a.X - b.X) ^ 2) + (a.Y - b.Y) ^ 2);
			return distance;
		}

		// Sysmessage
		public static void SendMessage(int num)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, num.ToString());
		}

        public static void SendMessage(uint num)
        {
            if (Assistant.World.Player != null)
                Assistant.World.Player.SendMessage(MsgLevel.Info, num.ToString());
        }

		public static void SendMessage(string msg)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, msg);
		}

        public static void SendMessage(bool msg)
        {
            if (Assistant.World.Player != null)
                Assistant.World.Player.SendMessage(MsgLevel.Info, msg.ToString());
        }
        public static void Beep()
        {
            SystemSounds.Beep.Play();
        }
        // Login and logout
        public static void Disconnect()
        {
            Assistant.ClientCommunication.SendToServer(new Disconnect());
        }
        // Context Menu
        public static void ContextReply(int serial, int idx)
        {
            ClientCommunication.SendToServer(new ContextMenuRequest(serial));
            ClientCommunication.SendToServer(new ContextMenuResponse(serial, (ushort)idx));
        }
        public static void ContextReply(Mobile mob, int idx)
        {
            ClientCommunication.SendToServer(new ContextMenuRequest(mob.Serial));
            ClientCommunication.SendToServer(new ContextMenuResponse(mob.Serial, (ushort)idx));
        }
        public static void ContextReply(Item item, int idx)
        {
            ClientCommunication.SendToServer(new ContextMenuRequest(item.Serial));
            ClientCommunication.SendToServer(new ContextMenuResponse(item.Serial, (ushort)idx));
        }
	}
}
