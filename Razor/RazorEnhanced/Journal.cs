using Assistant;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System;
using System.Diagnostics;

namespace RazorEnhanced
{
    public class Journal
    {
        public class JournalEntry
        {
            private string m_Text;
            public string Text { get { return m_Text; } }

            private string m_Type;
            public string Type { get { return m_Type; } }

            private int m_Color;
            public int Color { get { return m_Color; } }

            private string m_Name;
            public string Name { get { return m_Name; } }

            private int m_Serial;
            public int Serial { get { return m_Serial; } }

            private double m_Timestamp;
            public double Timestamp { get { return m_Timestamp; } }

            public JournalEntry(string text, string type, int color, string name, int serial, double timestamp = -1)
            {
                m_Text = text;
                m_Type = type;
                m_Color = color;
                m_Name = name;
                m_Serial = serial;
                m_Timestamp = (timestamp == -1) ? Journal.Timestamp(DateTime.Now) : timestamp;
            }
        }

        static double Timestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return diff.TotalSeconds;
        }

        private static JournalPointerList m_pointerRegistry = new JournalPointerList();

        public class JournalPointerList
        {
            const string DEFAULT = "_default";

            private List<JournalEntry> pointers;
            private Dictionary<int, Dictionary<string, JournalEntry>> byThread;
            public JournalPointerList()
            {
                pointers = new List<JournalEntry>();
                byThread = new Dictionary<int, Dictionary<string, JournalEntry>>();
            }

            private bool ExistPointer(int pid, string name)
            {
                return (byThread.ContainsKey(pid) && byThread[pid].ContainsKey(name));
            }

            public void Cleanup(int pid)
            {
                foreach (JournalEntry je in byThread[pid].Values ) {
                    pointers.Remove(je);
                }
                byThread.Remove(pid);
            }

            public JournalEntry GetPointer(string name = null)
            {
                if (name == null) { name = DEFAULT; }
                int pid = Thread.CurrentThread.ManagedThreadId;
                if (!ExistPointer(pid, name))
                {
                    return SetPointer(name);
                }

                return byThread[pid][name];
            }

            public JournalEntry SetPointer(string name = null)
            {
                if (name == null) { name = DEFAULT; }
                int pid = Thread.CurrentThread.ManagedThreadId;
                if (!byThread.ContainsKey(pid))
                {
                    byThread[pid] = new Dictionary<string, JournalEntry>();
                }
                byThread[pid][name] = World.Player.Journal.Last();

                return byThread[pid][name];
            }
        }




        public static void Clear()
		{
            //TODO: scan the registry and cleanup up to the oldest pointer.
			ConcurrentQueue<JournalEntry> Journal = new ConcurrentQueue<JournalEntry>();
			Interlocked.Exchange(ref World.Player.Journal, Journal);
		}

		public static bool Search(string text)
		{
			try
			{
				return World.Player.Journal.Any(entrys => entrys.Text.Contains(text));
			}
			catch
			{
				return false;
			}
		}

		public static bool SearchByName(string text, string name)
		{
			try
			{
				return World.Player.Journal.Where(entrys => entrys.Name == name).Any(entrys => entrys.Text.Contains(text));
			}
			catch
			{
				return false;
			}
		}

		public static bool SearchByColor(string text, int color)
		{
			try
			{
				return World.Player.Journal.Where(entrys => entrys.Color == color).Any(entrys => entrys.Text.Contains(text));
			}
			catch
			{
				return false;
			}
		}

		public static bool SearchByType(string text, string type)
		{
			try
			{
				return World.Player.Journal.Where(entrys => entrys.Type.ToString() == type).Any(entrys => entrys.Text.Contains(text));
			}
			catch
			{
				return false;
			}
		}

		public static string GetLineText(string text, bool addname = false)
		{
			string result = string.Empty;
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Text.Contains(text))
					{
						if (addname)
							result = entrys.Name + ": " + entrys.Text;
						else
							result = entrys.Text;
					}
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static List<string> GetTextBySerial(int serial)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Serial == serial)
					{
						result.Add(entrys.Text);
					}
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static List<string> GetTextByColor(int color, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Color == color)
					{
						if (addname)
							result.Add(entrys.Name + ": " + entrys.Text);
						else
							result.Add(entrys.Text);
					}
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static List<string> GetTextByName(string name)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Name == name)
					{
						result.Add(entrys.Text);
					}
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static List<string> GetTextByType(string type, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Type == type)
					{
						if (addname)
							result.Add(entrys.Name + ": " + entrys.Text);
						else
							result.Add(entrys.Text);
					}
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static List<string> GetSpeechName()
		{
			List<string> result = new List<string>();
			try
			{
				result.AddRange(from entrys in World.Player.Journal where !string.IsNullOrEmpty(entrys.Name) select entrys.Name);
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static void WaitJournal(string text, int delay)
		{
			int subdelay = delay;
			while (Search(text) == false && subdelay > 0)
			{
				Thread.Sleep(10);
				subdelay -= 10;
			}
		}

		public static string WaitJournal(List<string> msgs, int delay)
		{
			int subdelay = delay;
			while (subdelay > 0)
			{
				foreach(string s in msgs)
				{
					if (Search(s))
						return s; // found one of msgs list
				}

				Thread.Sleep(10);
				subdelay -= 10;
			}
			return string.Empty; // found one of msgs list

		}
         public static bool WaitByName(string name, int delay)
        {
            int subdelay = delay;
            while (subdelay > 0)
            {
                foreach (JournalEntry entrys in World.Player.Journal)
                {
                    if (entrys.Name == name)
                    {
                        return true;
                    }
                }
                Thread.Sleep(10);
                subdelay -= 10;
            }
            return false;
        }
    }
}
