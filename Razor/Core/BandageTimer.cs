using System;

namespace Assistant
{
	internal class BandageTimer
	{
		private static int m_Count;
		private static Timer m_Timer;

		private static int[] m_ClilocNums = new int[]
			{
				500955,
				500962,
				500963,
				500964,
				500965,
				500966,
				500967,
				500968,
				500969,
				503252,
				503253,
				503254,
				503255,
				503256,
				503257,
				503258,
				503259,
				503260,
				503261,
				1010058,
				1010648,
				1010650,
				1060088,
				1060167,
			};

		static BandageTimer()
		{
			m_Timer = new InternalTimer();
		}

		internal static void OnLocalizedMessage(int num)
		{
			if (Running)
			{
				if (num == 500955 || (num >= 500962 && num <= 500969) || (num >= 503252 && num <= 503261) || num == 1010058 || num == 1010648 || num == 1010650 || num == 1060088 || num == 1060167)
					Stop();
			}
		}

		internal static void OnAsciiMessage(string msg)
		{
			if (Running)
			{
				foreach (int cliloc in m_ClilocNums)
				{
					if (Language.GetCliloc(cliloc) == msg)
					{
						Stop();
						break;
					}
				}
			}
		}

		internal static int Count
		{
			get
			{
				return m_Count;
			}
		}

		internal static bool Running
		{
			get
			{
				return m_Timer.Running;
			}
		}

		internal static void Start()
		{
			m_Count = 0;

			if (m_Timer.Running)
				m_Timer.Stop();
			m_Timer.Start();
			ClientCommunication.RequestTitlebarUpdate();
		}

		internal static void Stop()
		{
			m_Timer.Stop();
			ClientCommunication.RequestTitlebarUpdate();
		}

		private class InternalTimer : Timer
		{
			internal InternalTimer()
				: base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
			{
			}

			protected override void OnTick()
			{
				m_Count++;
				if (m_Count > 30)
					Stop();
				ClientCommunication.RequestTitlebarUpdate();
			}
		}
	}
}
