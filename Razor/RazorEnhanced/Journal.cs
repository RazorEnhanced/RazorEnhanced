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

        bool m_Active;
        internal bool Active { 
            get { return m_Active; } 
            set {
                m_Active = value;
                if (value)
                {
                    if (GlobalJournal != null)
                        m_journal = new ConcurrentQueue<RazorEnhanced.Journal.JournalEntry>(GlobalJournal.m_journal); // clone global list at activate
                }
                else
                {
                    Clear();
                }
            } 
        }

        internal static void Enqueue(RazorEnhanced.Journal.JournalEntry entry)
        {
            bool needsCleanup = false;
            foreach (WeakReference<Journal> j in allInstances)
            {
                if (j == null)
                    continue;
                Journal journal;
                j.TryGetTarget(out journal);
                if (journal != null)
                {
                    if (journal.Active)
                        journal.enqueue(entry);
                }
                else
                    needsCleanup = true;
            }
            if (needsCleanup)

                allInstances.RemoveAll(wr => wr.TryGetTarget(out var el) && el == null);
        }

        internal ConcurrentQueue<RazorEnhanced.Journal.JournalEntry> m_journal;
        internal int m_MaxJournalEntries;

        internal static List<WeakReference<Journal>> allInstances = new List<WeakReference<Journal>>();
        internal static Journal GlobalJournal = new Journal(100);

        public Journal(int size=100)
        {
            m_MaxJournalEntries = size;
            Active = true;
            allInstances.Add(new WeakReference<Journal>(this));

        }

        ~Journal()
        {
        }

        internal void enqueue(RazorEnhanced.Journal.JournalEntry entry)
        {
            m_journal.Enqueue(entry);
            if (m_journal.Count > m_MaxJournalEntries)
            {
                RazorEnhanced.Journal.JournalEntry ra;
                m_journal.TryDequeue(out ra);
            }
        }


    /// <summary>
    /// The JournalEntry class rapresents a line in the Journal.
    /// </summary>
    public class JournalEntry
		{
			private readonly string m_Text;
            /// <summary>
            /// Actual content of the Journal Line.
            /// </summary>
			public string Text { get { return m_Text; } }

			private readonly string m_Type;
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

			private readonly int m_Color;
            /// <summary>
            /// Color of the text.
            /// </summary>
			public int Color { get { return m_Color; } }

			private readonly string m_Name;
            /// <summary>
            /// Name of the source, can be a Mobile or an Item.
            /// </summary>
			public string Name { get { return m_Name; } }

			private readonly int m_Serial;
            /// <summary>
            /// Name of the source, can be a Mobile or an Item.
            /// </summary>
            public int Serial { get { return m_Serial; } }

            private readonly double m_Timestamp;
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
        public List<JournalEntry> GetJournalEntry(double afterTimestap = -1)
        {
            var journalEntries = new List<JournalEntry>();
            if (m_journal == null) { return journalEntries; }

            journalEntries.AddRange(m_journal.Where(journalEntry => journalEntry.Timestamp > afterTimestap).Select(journalEntry => journalEntry.Copy()));
            return journalEntries;
        }

        /// <summary>
        /// Get a copy of all Journal lines as JournalEntry. The list can be filtered to include *only* most recent events.
        /// </summary>
        /// <param name="afterJournalEntry">A JournalEntry object (default: null, no filter)</param>
        /// <returns>List of JournalEntry</returns>
        public List<JournalEntry> GetJournalEntry(JournalEntry afterJournalEntry = null)
        {
            var afterTimestap = afterJournalEntry == null ? -1 : afterJournalEntry.Timestamp;
            return GetJournalEntry(afterTimestap);
        }

        /// <summary>
        /// Removes all entry from the Jorunal. 
        /// </summary>
		public void Clear()
		{
			ConcurrentQueue<JournalEntry> Journal = new ConcurrentQueue<JournalEntry>();
			Interlocked.Exchange(ref m_journal, Journal);
		}

        /// <summary>
        /// Removes all matching entry from the Jorunal. 
        /// </summary>
        public void Clear(string toBeRemoved)
        {
            ConcurrentQueue<JournalEntry> journal = new ConcurrentQueue<JournalEntry>();
            foreach (var entry in m_journal)
            {
                if (!entry.Text.Contains(toBeRemoved))
                    journal.Enqueue(entry);
            }
            Interlocked.Exchange(ref m_journal, journal);
        }


        /// <summary>
        /// Search in the Journal for the occurrence of text. (case sensitive)
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <returns>True: Text is found - False: otherwise</returns>
		public bool Search(string text)
		{
			try
			{
				return m_journal.Any(entrys => entrys.Text.Contains(text));
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
        public void FilterText(string text)
        {            
            Engine.MainWindow.SafeAction(s => { s.JournalFilterDataGrid.Rows.Add(new object[] { text.ToLower() }); });
            Filters.CopyJournalFilterTable();
        }

        /// <summary>
        /// Remove a stored a string that if matched, would block journal message ( case insensitive )
        /// </summary>
        /// <param name="text">Text to no longer block. case insensitive</param>
        /// <returns>void</returns>
        public void RemoveFilterText(string text)
        {
            text = text.ToLower();
            //System.Windows.Forms.DataGridViewCell cell = journalfilterdatagrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            for (int i = 0; i < Assistant.Engine.MainWindow.JournalFilterDataGrid.Rows.Count; i++)
            {
                System.Windows.Forms.DataGridViewRow gridRow = Assistant.Engine.MainWindow.JournalFilterDataGrid.Rows[i];
                if (gridRow.IsNewRow)
                    continue;

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
		public bool SearchByName(string text, string name)
		{
			try
			{
				return m_journal.Where(entrys => entrys.Name.ToLower().Contains(name.ToLower())).Any(entrys => entrys.Text.ToLower().Contains(text.ToLower()));
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
		public bool SearchByColor(string text, int color)
		{
			try
			{
				return m_journal.Where(entrys => entrys.Color == color).Any(entrys => entrys.Text.Contains(text));
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
        public bool SearchByType(string text, string type)
        {
            try
            {
                foreach (var entry in m_journal)
                {
                    if (entry.Type.ToString() == type && entry.Text.Contains(text))
                    {
                        return true;
                    }
                }
                //return m_journal.Where(entrys => entrys.Type.ToString() == type).Any(entrys => entrys.Text.Contains(text));
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
		public string GetLineText(string text, bool addname = false)
		{
			string result = string.Empty;
			try
			{
				foreach (JournalEntry entrys in m_journal)
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
		public List<string> GetTextBySerial(int serial, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in m_journal)
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
        public List<string> GetTextByColor(int color, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in m_journal)
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
		public List<string> GetTextByName(string name, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in m_journal)
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
		public List<string> GetTextByType(string type, bool addname = false)
		{
			List<string> result = new List<string>();
			try
			{
				foreach (JournalEntry entrys in m_journal)
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
		public List<string> GetSpeechName()
		{
			List<string> result = new List<string>();
			try
			{
				result.AddRange(from entrys in m_journal where !string.IsNullOrEmpty(entrys.Name) select entrys.Name);
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
		public bool WaitJournal(string text, int delay)
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
		public string WaitJournal(List<string> msgs, int delay)
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
        public bool WaitByName(string name, int delay)
        {
            int subdelay = delay;
            while (subdelay > 0)
            {
                foreach (JournalEntry entrys in m_journal)
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
