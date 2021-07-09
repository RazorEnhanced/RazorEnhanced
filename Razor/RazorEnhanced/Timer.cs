using System.Collections.Concurrent;
using System.Linq;
using Ultima;
using System.Timers;
using System;

namespace RazorEnhanced
{
    public class ScriptTimer : System.Timers.Timer
    {
        internal string Name;
        internal string Message;
        internal DateTime m_dueTime;

        public ScriptTimer() : base() => this.Elapsed += this.ElapsedAction;
        protected new void Dispose()
        {
            this.Elapsed -= this.ElapsedAction;
            base.Dispose();
        }

        public double TimeLeft => (this.m_dueTime - DateTime.Now).TotalMilliseconds;
        public new void Start()
        {
            this.m_dueTime = DateTime.Now.AddMilliseconds(this.Interval);
            base.Start();
        }

        private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.AutoReset)
                this.m_dueTime = DateTime.Now.AddMilliseconds(this.Interval);
        }
    }


    /// <summary>
    /// Timer are normally used to display messages after a certain period of time. 
    /// They are also often used to keep track of the maximum amount of time for an action to complete.
    /// </summary>
	public class Timer
	{
		private static ConcurrentDictionary<string, ScriptTimer> m_timers = new ConcurrentDictionary<string, ScriptTimer>();

        /// <summary>
        /// @nodoc
        /// List of active timers.
        /// </summary>
        public static ConcurrentDictionary<string, ScriptTimer> Timers { get => m_timers; set => m_timers = value; }


        
		public static void Create(string name, int delay)
		{
			Create(name, delay, String.Empty);
		}

        /// <summary>
        /// Create a timer with the provided name that will expire in ms_timer time (in milliseconds)
        /// </summary>
        /// <param name="name">Timer name.</param>
        /// <param name="delay">Delay in milliseconds.</param>
        /// <param name="message">Message displayed at timeouit.</param>
		public static void Create(string name, int delay, string message)
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
			newtimer.Message = message;
            newtimer.Start();
			m_timers[name] = newtimer;
		}

        /// <summary>
        /// Check if a timer object is expired or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true if not expired, false if expired</returns>
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

        /// <summary>
        /// Get remaining time for a named timer
        /// </summary>
        /// <param name="name">Timer name</param>
        /// <returns>Returns the milliseconds remaining for a timer.</returns>
        public static int Remaining(string name)
        {
            if (m_timers.ContainsKey(name)) // Timer Exist
            {
                if (m_timers.TryGetValue(name, out ScriptTimer t)) // Get timer data
                {
                    if (t != null)
                        return (int)t.TimeLeft;
                    else
                        return -1;
                }
                else
                    return -1;
            }
            return -1;
        }


        private static void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			ScriptTimer t = (ScriptTimer)source;
			if (t.Message != String.Empty) // If timer have a end Message
				Misc.SendMessage(t.Message);

			t.Close();
			m_timers.TryRemove(t.Name, out ScriptTimer tt); // Remove timer
        }
    }
}
