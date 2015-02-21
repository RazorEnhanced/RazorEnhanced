using System;
using RazorEnhanced;

namespace Test
{
	public class Test
	{
		public Test()
		{
		}

		public int Run()
		{
			Player.SendMessage("Hits: " + Player.Hits.ToString());
			return 0;
		}
	}
}
