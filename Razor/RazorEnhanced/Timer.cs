using System.Collections.Concurrent;
using System.Linq;
using Ultima;
using System.Timers;

namespace RazorEnhanced
{
	public class Timer
	{
		private static ConcurrentDictionary<string, System.Timers.Timer> m_timers = new ConcurrentDictionary<string, System.Timers.Timer>();

		public static void CreateTimer(string name, int delay)
		{
			if (m_timers.ContainsKey(name)) // Timer Exist
			{
				if (m_timers.TryGetValue(name, out System.Timers.Timer t)) // Get timer data
				{
					t.Close(); // stop timer
					t = null;
					m_timers.TryRemove(name, out System.Timers.Timer tt); // Remove timer
				}
			}

			System.Timers.Timer newtimer = new System.Timers.Timer();
			newtimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			newtimer.Interval = delay;
			newtimer.Enabled = true;
		}

		public static bool CheckTimer(string name)
		{
			if (m_timers.ContainsKey(name)) // Timer Exist
				return false;

			if (m_timers.TryGetValue(name, out System.Timers.Timer t)) // Get timer data
			{
				if (t != null)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		private static void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			System.Timers.Timer t = (System.Timers.Timer)source;
			t.Close();
			t = null;
		}
	}		
}