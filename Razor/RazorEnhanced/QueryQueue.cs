using Assistant;
using System.Threading;
using System.Collections.Concurrent;

namespace RazorEnhanced
{
	internal class QueryQueue
	{
		internal static ConcurrentQueue<int> QueryStats = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> QueryMobsProps = new ConcurrentQueue<int>();
		private static System.Timers.Timer m_timer = new System.Timers.Timer();

		internal static void Start()
		{
			QueryStats = new ConcurrentQueue<int>();
			QueryMobsProps = new ConcurrentQueue<int>();
			m_timer.Enabled = true;
			m_timer.Interval = 5;
			m_timer.Elapsed += ProcessQueue;
		}

		private static void ProcessQueue(object source, System.Timers.ElapsedEventArgs e)
		{
			if (!QueryStats.IsEmpty)
			{
				int s = 0;
				QueryStats.TryDequeue(out s);
				if (s != 0)
				{
					ClientCommunication.SendToServerWait(new StatusQuery(s));
					Thread.Sleep(100);
				}
			}

			if (World.Player.Expansion > 3)
			{
				if (!QueryMobsProps.IsEmpty)
				{
					int s = 0;
					QueryMobsProps.TryDequeue(out s);
					if (s != 0)
					{
						ClientCommunication.SendToServerWait(new QueryProperties(s));
						Thread.Sleep(100);
					}
				}
			}
		}

		internal static void Abort()
		{
			m_timer.Close();
		}
	}
}