using System.Collections.Concurrent;
using System.Linq;
using Ultima;
using System.Timers;

namespace RazorEnhanced
{
	public class ScriptTimer : System.Timers.Timer
	{
		internal string Name;
	}

	public class Timer
	{
		private static ConcurrentDictionary<string, ScriptTimer> m_timers = new ConcurrentDictionary<string, ScriptTimer>();
        public static ConcurrentDictionary<string, ScriptTimer> Timers { get => m_timers; set => m_timers = value; }

        public static void Create(string name, int delay)
		{
			if (m_timers.ContainsKey(name)) // Timer Exist
			{
				if (m_timers.TryGetValue(name, out ScriptTimer t)) // Get timer data
				{
					t.Close(); // stop timer
					t = null;
					m_timers.TryRemove(name, out ScriptTimer tt); // Remove timer
				}
			}
			ScriptTimer newtimer = new ScriptTimer();
			newtimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			newtimer.Interval = delay;
			newtimer.Enabled = true;
			newtimer.Name = name;
			m_timers[name] = newtimer;
		}

		public static bool Check(string name)
		{
			if (m_timers.ContainsKey(name)) // Timer Exist
			{
				if (m_timers.TryGetValue(name, out ScriptTimer t)) // Get timer data
				{
					if (t != null)
						return true;
					else
						return false;
				}
				else
					return false;
			}
			return false;
		}

		private static void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			ScriptTimer t = (ScriptTimer)source;
			t.Close();
			m_timers.TryRemove(t.Name, out ScriptTimer tt); // Remove timer
			t = null;
		}
	}
}
