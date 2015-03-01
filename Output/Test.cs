using System;

namespace RazorEnhanced
{
	public class Test
	{
		public static void Main()
		{
		}			
			
		public static int Run()
		{
			//Player.InvokeVirtue("Honor");
			Player.Walk("North");
			Player.Pause(0.5);
			return 0;
		}
	}
}
