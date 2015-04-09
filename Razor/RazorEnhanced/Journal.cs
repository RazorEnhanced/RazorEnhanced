using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using Assistant;

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
            World.Player.Journal.Clear();
            Misc.SendMessage("Journal Cleared");
        }

        public static void Dump()
        {

            foreach (JournalEntry entry in World.Player.Journal)
            {
                Misc.SendMessage(entry.Text);
                Misc.SendMessage(entry.Name);

            }
        }
	}
}
