using System;
using System.Collections.Generic;
using Assistant;


namespace RazorEnhanced
{
	public class Misc
	{
		//General
		public static void Pause(double seconds)
		{
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
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
				Assistant.World.Player.SendMessage(MsgLevel.Info, (Assistant.LocString)num);
		}

		public static void SendMessage(string msg)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, msg);
		}

	}
}
