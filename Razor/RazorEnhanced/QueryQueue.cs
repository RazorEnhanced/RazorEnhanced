using Assistant;
using System.Threading;
using System.Collections.Concurrent;

namespace RazorEnhanced
{
	internal class QueryQueue
	{
		internal static ConcurrentQueue<int> QueryStats = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> QueryMobsProps = new ConcurrentQueue<int>();
		private static Thread m_processqueuethread = new Thread(ProcessQueue);

		internal static void Start()
		{
			QueryStats = new ConcurrentQueue<int>();
			QueryMobsProps = new ConcurrentQueue<int>();
			if (!m_processqueuethread.IsAlive)
			{
				m_processqueuethread = new Thread(ProcessQueue);
				m_processqueuethread.Start();
			}

		}

		internal static void ProcessQueue()
		{
			while (true)
			{
				if (!QueryStats.IsEmpty)
				{
					int s = 0;
					QueryStats.TryDequeue(out s);
					ClientCommunication.SendToServer(new StatusQuery(s));
					Thread.Sleep(100);
				}

				if (World.Player.Expansion > 3)
				{
					if (!QueryMobsProps.IsEmpty)
					{
						int s = 0;
						QueryMobsProps.TryDequeue(out s);
						ClientCommunication.SendToServer(new QueryProperties(s));
						Thread.Sleep(100);
					}
				}
				Thread.Sleep(5);
			}
		}

		internal static void Abort()
		{
			try
			{
				if (m_processqueuethread != null && (m_processqueuethread.ThreadState == ThreadState.Running || m_processqueuethread.ThreadState == ThreadState.WaitSleepJoin))
					m_processqueuethread.Abort();
			}
			catch { }
		}
	}
}