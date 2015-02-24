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
			Player.SendMessage("Hits: " + Player.Hits.ToString());
			return 0;
		}
	}
}
