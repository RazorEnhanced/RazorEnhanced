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
			
			Item.Filter filter = new Item.Filter();
			ArrayList items = Item.ApplyFilter(filter);
			Misc.SendMessage(items.Count.ToString());
			
			return 0;
		}
	}
}
