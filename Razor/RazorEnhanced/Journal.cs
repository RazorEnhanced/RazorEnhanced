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

            public JournalEntry(string text, string type, int color)
			{
                m_Text = text;
                m_Type = type;
				m_Color = color;
			}
    	}
        public static void Clear()
        {
            World.Player.Journal.Clear();
            Misc.SendMessage("Journal Cleared");
        }
	}
}
