using System;

namespace RazorEnhanced
{
	public class Test
	{
		private static Misc.Pause m_Pause = new Misc.Pause(1.0);

		public static void Main()
		{
		}

		public static int Run()
		{
			//Player.InvokeVirtue("Honor");
			if (!m_Pause.IsWaiting)
				Player.Walk("North");
			
			return 0;
		}
	}
}
