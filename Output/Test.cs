using System;

namespace Test
{
	public class Test
	{
		public static void Run()
		{
			System.Windows.Forms.MessageBox.Show("Hits: " + Assistant.World.Player.Hits.ToString());
		}
	}
}
