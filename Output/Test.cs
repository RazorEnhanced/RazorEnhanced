using System;
using System.Collections;

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
			//Misc.Pause(0.5);
			//Player.Walk("North");
			
			Items.Filter filter = new Items.Filter();
			ArrayList items = Items.ApplyFilter(filter);
			Misc.SendMessage(items.Count.ToString());
			
			return 0;
		}
	}
}
