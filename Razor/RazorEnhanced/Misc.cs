using System;
using System.Collections.Generic;
using Assistant;


namespace RazorEnhanced
{
	public class Misc
	{
		//General
		public class Pause
		{
			private DateTime m_Previous;
			private TimeSpan m_Delay;

			public Pause(double seconds)
			{
				m_Previous = DateTime.Now;
				m_Delay  = TimeSpan.FromSeconds(seconds);
			}

			public bool IsWaiting
			{
				get
				{
					if (DateTime.Now >= m_Previous + m_Delay)
					{
						m_Previous = DateTime.Now;
						return false;
					}
					else
						return true;
				}
			}
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
