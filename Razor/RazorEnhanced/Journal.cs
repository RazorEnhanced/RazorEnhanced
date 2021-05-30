using Assistant;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace RazorEnhanced
{
    /// <summary>
    /// The Journal class provides access to the message Journal.
    /// </summary>
	public class Journal
	{


        /// <summary>
        /// The JournalEntry class rapresents a line in the Journal.
        /// </summary>
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

            private readonly static DateTime UnixTimeBegin = new DateTime(1970, 1, 1);


            public JournalEntry Copy() {
                return new JournalEntry(this);
            }

            public JournalEntry(string text, string type, int color, string name, int serial)
            {
                m_Text = text;
                m_Type = type;
                m_Color = color;
                m_Name = name;
                m_Serial = serial;
                m_Timestamp = DateTime.Now.Subtract(UnixTimeBegin).TotalSeconds;
            }

            public JournalEntry(JournalEntry from)
            {
                m_Text = from.Text;
                m_Type = from.Type;
                m_Color = from.Color;
                m_Name = from.Name;
                m_Serial = from.Serial;
                m_Timestamp = from.Timestamp;
            }
        }


        /// <summary>
        /// Get a copy of all Journal lines as JournalEntry. The list can be limited to most recent events.
        /// </summary>
        /// <param name="afterTimestap"></param>
        /// <returns></returns>
        public static List<JournalEntry> GetJournalEntry(double afterTimestap = -1)
        {
            var journalEntries = new List<JournalEntry>();
            if (World.Player.Journal == null) { return journalEntries; }

            journalEntries.AddRange(World.Player.Journal.Where( journalEntry => journalEntry.Timestamp > afterTimestap ).Select(journalEntry => journalEntry.Copy() ));
            return journalEntries;
        }

        /// <summary>
        /// Removes all entry from the Jorunal. This operation is Global for all Razor.
        /// </summary>
		public static void Clear()
		{
			ConcurrentQueue<JournalEntry> Journal = new ConcurrentQueue<JournalEntry>();
			Interlocked.Exchange(ref World.Player.Journal, Journal);
		}

        /// <summary>
        /// Search in the Journal for the occurrence of text. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <returns>True: Text is found - False: otherwise</returns>
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

        /// <summary>
        /// Search in the Journal for the occurrence of text, for a given soruce. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <param name="name">Name of the source.</param>
        /// <returns>True: Text is found - False: otherwise</returns>
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

        /// <summary>
        /// Search in the Journal for the occurrence of text, for a given color. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <param name="color">Color of the message.</param>
        /// <returns>True: Text is found - False: otherwise</returns>
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

        /// <summary>
        /// Search in the Journal for the occurrence of text, for a given type. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <param name="type">
        ///     Regular
        ///     System
        ///     Emote
        ///     Label
        ///     Focus
        ///     Whisper
        ///     Yell
        ///     Spell
        ///     Guild
        ///     Alliance
        ///     Party
        ///     Encoded
        ///     Special
        /// </param>
        /// <returns>True: Text is found - False: otherwise</returns>
        public static bool SearchByType(string text, string type)
        {
            try
            {
                foreach (var entry in World.Player.Journal)
                {
                    if (entry.Type.ToString() == type && entry.Text.Contains(text))
                    {
                        return true;
                    }
                }
                //return World.Player.Journal.Where(entrys => entrys.Type.ToString() == type).Any(entrys => entrys.Text.Contains(text));
            }
            catch
            {
                // Do nothing
            }
            return false;
        }

        /// <summary>
        /// Search and return the most recent line Journal containing the given text. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <param name="addname">Prepend source name. (default: False)</param>
        /// <returns>Return the full line - Empty string if not found.</returns>
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

        /// <summary>
        /// Returns all the lines present in the Journal for a given serial.
        /// </summary>
        /// <param name="serial">Serial of the soruce.</param>
        /// <param name="addname">Prepend source name. (default: False)</param>
        /// <returns>A list of Journal as lines of text.</returns>
		public static List<string> GetTextBySerial(int serial, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Serial == serial)
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

        /// <summary>
        /// Returns all the lines present in the Journal for a given color.
        /// </summary>
        /// <param name="color">Color of the soruce.</param>
        /// <param name="addname">Prepend source name. (default: False)</param>
        /// <returns>A list of Journal as lines of text.</returns>
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

        /// <summary>
        /// Returns all the lines present in the Journal for a given source name. (case sensitive)
        /// </summary>
        /// <param name="name">Name of the soruce.</param>
        /// <param name="addname">Prepend source name. (default: False)</param>
        /// <returns>A list of Journal as lines of text.</returns>
		public static List<string> GetTextByName(string name, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in World.Player.Journal)
				{
					if (entrys.Name == name)
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

        /// <summary>
        /// Returns all the lines present in the Journal for a given type. (case sensitive)
        /// </summary>
        /// <param name="type">
        ///     Regular
        ///     System
        ///     Emote
        ///     Label
        ///     Focus
        ///     Whisper
        ///     Yell
        ///     Spell
        ///     Guild
        ///     Alliance
        ///     Party
        ///     Encoded
        ///     Special
        /// </param>
        /// <param name="addname">Prepend source name. (default: False)</param>
        /// <returns>A list of Journal as lines of text.</returns>
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

        /// <summary>
        /// Get list of speakers.
        /// </summary>
        /// <returns>List of speakers as text.</returns>
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

        /// <summary>
        /// Pause script and wait for maximum amount of time, for a specific text to appear in Journal. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <param name="delay">Maximum pause in milliseconds.</param>
        /// <returns>True: Text is found - False: otherwise</returns>
		public static bool WaitJournal(string text, int delay)
		{
			int subdelay = delay;
			while (Search(text) == false && subdelay > 0)
			{
				Thread.Sleep(10);
				subdelay -= 10;
			}
            return Search(text);
		}


        /// <summary>
        /// Pause script and wait for maximum amount of time, for any of the text in the list to appear in Journal. (case sensitive)
        /// </summary>
        /// <param name="msgs">List of text to search.</param>
        /// <param name="delay">Maximum pause in milliseconds.</param>
        /// <returns>Return the first line in the journal. Empty string: otherwise</returns>
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

        /// <summary>
        /// Pause script and wait for maximum amount of time, for a specific soruce to appear in Jorunal. (case sensitive)
        /// </summary>
        /// <param name="name">Name of the soruce.</param>
        /// <param name="delay">Maximum pause in milliseconds.</param>
        /// <returns></returns>
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
