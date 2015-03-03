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

        // Sysmessage
        public static void SendMessage(int num)
        {
            Assistant.World.Player.SendMessage((Assistant.LocString)num);
        }

        public static void SendMessage(string msg)
        {
            Assistant.World.Player.SendMessage(msg);
        }

	}
}
