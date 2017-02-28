using Assistant;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

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

			public JournalEntry(string text, string type, int color, string name)
			{
				m_Text = text;
				m_Type = type;
				m_Color = color;
				m_Name = name;
			}
		}

		public static void Clear()
		{
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

		public static string GetLineText(string text)
		{
			string result = "";
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Text.Contains(text))
						result = entrys.Text;
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
	}
}