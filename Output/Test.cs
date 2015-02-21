using System;
using RazorEnhanced;

namespace Test
{
	public class Test
	{
		public static void Run()
		{
			System.Windows.Forms.MessageBox.Show("Hits: " + RazorEnhanced.Player.Hits.ToString());
		}
	}
}
