using System.Threading;

namespace RazorEnhanced
{
	internal class CheckConnection
	{
		internal static bool ConnectionStart = false;
		internal static bool ConnectionSucccesfull = false;
		private static Thread m_checkconnectionthread = new Thread(CheckConnectionWorker);
		public static Thread CheckConnectionThread { get { return m_checkconnectionthread; } }


		internal static void CheckStart()
		{
			if (!m_checkconnectionthread.IsAlive)
			{
				m_checkconnectionthread = new Thread(CheckConnectionWorker);
				ConnectionStart = true;
				m_checkconnectionthread.Start();
            }

        }

		internal static void CheckConnectionWorker()
		{
			Thread.Sleep(90000);
			if (ConnectionSucccesfull)
			{
				ConnectionStart = false;
				ConnectionSucccesfull = false;
			}
			else
			{
				Misc.Disconnect();
				ConnectionStart = false;
				ConnectionSucccesfull = false;
			}

		}

		internal static void Abort()
		{
			try
			{
				m_checkconnectionthread.Abort();
			}
			catch { }
		}
	}
}