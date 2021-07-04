using Assistant;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Assistant.UI;


namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private void journalfilterdatagrid_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            System.Windows.Forms.DataGridViewCell cell = journalfilterdatagrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Value = cell.Value.ToString().ToLower();
            RazorEnhanced.Filters.CopyJournalFilterTable();
        }

        private void journalfilterdatagrid_DefaultValuesNeeded(object sender, System.Windows.Forms.DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "";
        }
    }
}

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
            /// <summary>
            /// Actual content of the Journal Line.
            /// </summary>
			public string Text { get { return m_Text; } }

			private string m_Type;
            /// <summary>
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
            /// </summary>
            public string Type { get { return m_Type; } }

			private int m_Color;
            /// <summary>
            /// Color of the text.
            /// </summary>
			public int Color { get { return m_Color; } }

			private string m_Name;
            /// <summary>
            /// Name of the source, can be a Mobile or an Item.
            /// </summary>
			public string Name { get { return m_Name; } }

			private int m_Serial;
            /// <summary>
            /// Name of the source, can be a Mobile or an Item.
            /// </summary>
            public int Serial { get { return m_Serial; } }

            private double m_Timestamp;
            /// <summary>
            /// Timestamp as UnixTime, the number of seconds elapsed since 01-Jan-1970.
            /// </summary>
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
        /// Get a copy of all Journal lines as JournalEntry. The list can be filtered to include *only* most recent events.
        /// </summary>
        /// <param name="afterTimestap">Timestap as UnixTime, the number of seconds elapsed since 01-Jan-1970. (default: -1, no filter)</param>
        /// <returns>List of JournalEntry</returns>
        public static List<JournalEntry> GetJournalEntry(double afterTimestap = -1)
        {
            var journalEntries = new List<JournalEntry>();
            if (World.Player.Journal == null) { return journalEntries; }

            journalEntries.AddRange(World.Player.Journal.Where(journalEntry => journalEntry.Timestamp > afterTimestap).Select(journalEntry => journalEntry.Copy()));
            return journalEntries;
        }

        /// <summary>
        /// Get a copy of all Journal lines as JournalEntry. The list can be filtered to include *only* most recent events.
        /// </summary>
        /// <param name="afterJournalEntry">A JournalEntry object (default: null, no filter)</param>
        /// <returns>List of JournalEntry</returns>
        public static List<JournalEntry> GetJournalEntry(JournalEntry afterJournalEntry = null)
        {
            var afterTimestap = afterJournalEntry == null ? -1 : afterJournalEntry.Timestamp;
            return GetJournalEntry(afterTimestap);
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
        /// Store a string that if matched, will block journal message ( case insensitive )
        /// </summary>
        /// <param name="text">Text to block. case insensitive, and will match if the incoming message contains the text</param>
        /// <returns>void</returns>
        public static void FilterText(string text)
        {            
            Engine.MainWindow.SafeAction(s => { s.JournalFilterDataGrid.Rows.Add(new object[] { text.ToLower() }); });
            Filters.CopyJournalFilterTable();
        }

        /// <summary>
        /// Remove a stored a string that if matched, would block journal message ( case insensitive )
        /// </summary>
        /// <param name="text">Text to no longer block. case insensitive</param>
        /// <returns>void</returns>
        public static void RemoveFilterText(string text)
        {
            text = text.ToLower();
            //System.Windows.Forms.DataGridViewCell cell = journalfilterdatagrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            for (int i = 0; i < Assistant.Engine.MainWindow.JournalFilterDataGrid.Rows.Count; i++)
            {
                System.Windows.Forms.DataGridViewRow gridRow = Assistant.Engine.MainWindow.JournalFilterDataGrid.Rows[i];
                if (gridRow.IsNewRow)
                {
                    continue;
                }

                if (text == gridRow.Cells[0].Value.ToString())
                {
                    Engine.MainWindow.SafeAction(s => { s.JournalFilterDataGrid.Rows.RemoveAt(i); });
                    break;
                }
            }
            Filters.CopyJournalFilterTable();
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
                        {
                            result = entrys.Name + ": " + entrys.Text;
                        }
                        else
                        {
                            result = entrys.Text;
                        }
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
                        {
                            result.Add(entrys.Name + ": " + entrys.Text);
                        }
                        else
                        {
                            result.Add(entrys.Text);
                        }
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
                        {
                            result.Add(entrys.Name + ": " + entrys.Text);
                        }
                        else
                        {
                            result.Add(entrys.Text);
                        }
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
                        {
                            result.Add(entrys.Name + ": " + entrys.Text);
                        }
                        else
                        {
                            result.Add(entrys.Text);
                        }
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
                        {
                            result.Add(entrys.Name + ": " + entrys.Text);
                        }
                        else
                        {
                            result.Add(entrys.Text);
                        }
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
                    {
                        return s; // found one of msgs list
                    }
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
