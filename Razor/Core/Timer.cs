using System;
using System.Collections.Generic;

namespace Assistant
{
	internal class MinHeap
	{
		private List<IComparable> m_List;
		private int m_Size;

		internal MinHeap()
			: this(1)
		{
		}

		internal MinHeap(int capacity)
		{
			m_List = new List<IComparable>(capacity + 1);
			m_Size = 0;
			m_List.Add(null); // 0th index is never used, always null
		}

		internal MinHeap(ICollection<IComparable> c)
			: this(c.Count)
		{
			foreach (IComparable o in c)
				m_List.Add(o);
			m_Size = c.Count;
			Heapify();
		}

		internal void Heapify()
		{
			for (int i = m_Size / 2; i > 0; i--)
				PercolateDown(i);
		}

		private void PercolateDown(int hole)
		{
			IComparable tmp = m_List[hole];
			int child;

			for (; hole * 2 <= m_Size; hole = child)
			{
				child = hole * 2;
				if (child != m_Size && (m_List[child + 1]).CompareTo(m_List[child]) < 0)
					child++;

				if (tmp.CompareTo(m_List[child]) >= 0)
					m_List[hole] = m_List[child];
				else
					break;
			}

			m_List[hole] = tmp;
		}

		internal IComparable Peek()
		{
			return m_List[1] as IComparable;
		}

		internal IComparable Pop()
		{
			IComparable top = Peek();

			m_List[1] = m_List[m_Size--];
			PercolateDown(1);

			return top;
		}

		internal void Remove(IComparable o)
		{
			for (int i = 1; i <= m_Size; i++)
			{
				if (m_List[i] == o)
				{
					m_List[i] = m_List[m_Size--];
					PercolateDown(i);
					// TODO: Do we ever need to shrink?
					return;
				}
			}
		}

		internal void Clear()
		{
			int capacity = m_List.Count / 2;
			if (capacity < 2)
				capacity = 2;
			m_Size = 0;
			m_List = new List<IComparable>(capacity) {null};
		}

		internal void Add(IComparable o)
		{
			// PercolateUp
			int hole = ++m_Size;

			// Grow the list if needed
			while (m_List.Count <= m_Size)
				m_List.Add(null);

			for (; hole > 1 && o.CompareTo(m_List[hole / 2]) < 0; hole /= 2)
				m_List[hole] = m_List[hole / 2];
			m_List[hole] = o;
		}

		internal void AddMultiple(ICollection<IComparable> col)
		{
			if (col == null || col.Count <= 0)
				return;

			foreach (IComparable o in col)
			{
				int hole = ++m_Size;

				// Grow the list as needed
				while (m_List.Count <= m_Size)
					m_List.Add(null);

				m_List[hole] = o;
			}

			Heapify();
		}

		internal int Count { get { return m_Size; } }

		internal bool IsEmpty { get { return Count <= 0; } }

		internal List<IComparable> GetRawList()
		{
			List<IComparable> copy = new List<IComparable>(m_Size);
			for (int i = 1; i <= m_Size; i++)
				copy.Add(m_List[i]);
			return copy;
		}
	}

	internal delegate void TimerCallback();

	internal delegate void TimerCallbackState(object state);

	internal abstract class Timer : IComparable
	{
		private DateTime m_Next;
		private TimeSpan m_Delay;
		private TimeSpan m_Interval;
		private bool m_Running;
		private int m_Index, m_Count;

		protected abstract void OnTick();

		internal Timer(TimeSpan delay)
			: this(delay, TimeSpan.Zero, 1)
		{
		}

		internal Timer(TimeSpan interval, int count)
			: this(interval, interval, count)
		{
		}

		internal Timer(TimeSpan delay, TimeSpan interval)
			: this(delay, interval, 0)
		{
		}

		internal Timer(TimeSpan delay, TimeSpan interval, int count)
		{
			m_Delay = delay;
			m_Interval = interval;
			m_Count = count;
		}

		internal void Start()
		{
			if (!m_Running)
			{
				m_Index = 0;
				m_Next = DateTime.Now + m_Delay;
				m_Running = true;
				m_Heap.Add(this);
				ChangedNextTick(true);
			}
		}

		internal void Stop()
		{
			if (!m_Running)
				return;

			m_Running = false;
			m_Heap.Remove(this);
			//ChangedNextTick();
		}

		public int CompareTo(object obj)
		{
			if (obj is Timer)
				return this.TimeUntilTick.CompareTo(((Timer)obj).TimeUntilTick);
			else
				return -1;
		}

		internal TimeSpan TimeUntilTick
		{
			get { return m_Running ? m_Next - DateTime.Now : TimeSpan.MaxValue; }
		}

		internal bool Running { get { return m_Running; } }

		internal TimeSpan Delay
		{
			get { return m_Delay; }
			set { m_Delay = value; }
		}

		internal TimeSpan Interval
		{
			get { return m_Interval; }
			set { m_Interval = value; }
		}

		private static MinHeap m_Heap = new MinHeap();
		private static System.Timers.Timer m_SystemTimer;

		internal static System.Timers.Timer SystemTimer
		{
			get { return m_SystemTimer; }
			set
			{
				if (m_SystemTimer != value)
				{
					if (m_SystemTimer != null)
						m_SystemTimer.Stop();
					m_SystemTimer = value;
					ChangedNextTick();
				}
			}
		}

		private static void ChangedNextTick()
		{
			ChangedNextTick(false);
		}

		private static void ChangedNextTick(bool allowImmediate)
		{
			if (m_SystemTimer == null)
				return;

			m_SystemTimer.Stop();

			if (!m_Heap.IsEmpty)
			{
				int interval = (int)Math.Round(((Timer)m_Heap.Peek()).TimeUntilTick.TotalMilliseconds);
				if (allowImmediate && interval <= 0)
				{
					Slice();
				}
				else
				{
					if (interval <= 0)
						interval = 1;

					m_SystemTimer.Interval = interval;
					m_SystemTimer.Start();
				}
			}
		}

		internal static void Slice()
		{
			int breakCount = 100;
			List<IComparable> readd = new List<IComparable>();

			while (!m_Heap.IsEmpty && ((Timer)m_Heap.Peek()).TimeUntilTick < TimeSpan.Zero)
			{
				if (breakCount-- <= 0)
					break;

				Timer t = (Timer)m_Heap.Pop();

				if (t != null && t.Running)
				{
					t.OnTick();

					if (t.Running && (t.m_Count == 0 || (++t.m_Index) < t.m_Count))
					{
						t.m_Next = DateTime.Now + t.m_Interval;
						readd.Add(t);
					}
					else
					{
						t.Stop();
					}
				}
			}

			m_Heap.AddMultiple(readd);

			ChangedNextTick();
		}

		private class OneTimeTimer : Timer
		{
			private TimerCallback m_Call;

			internal OneTimeTimer(TimeSpan d, TimerCallback call)
				: base(d)
			{
				m_Call = call;
			}

			protected override void OnTick()
			{
				m_Call();
			}
		}

		internal static Timer DelayedCallback(TimeSpan delay, TimerCallback call)
		{
			return new OneTimeTimer(delay, call);
		}

		private class OneTimeTimerState : Timer
		{
			private TimerCallbackState m_Call;
			private object m_State;

			internal OneTimeTimerState(TimeSpan d, TimerCallbackState call, object state)
				: base(d)
			{
				m_Call = call;
				m_State = state;
			}

			protected override void OnTick()
			{
				m_Call(m_State);
			}
		}

		internal static Timer DelayedCallbackState(TimeSpan delay, TimerCallbackState call, object state)
		{
			return new OneTimeTimerState(delay, call, state);
		}
	}
}
