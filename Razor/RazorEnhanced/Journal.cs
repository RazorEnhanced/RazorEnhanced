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
            //Misc.SendMessage("Journal Cleared");
        }
        public static bool Search(string text)
        {
            foreach (JournalEntry entrys in World.Player.Journal)
            {
                if (entrys.Text.Contains(text))
                    return true;
            }
            return false;
        }
        public static bool SearchByName(string text, string name)
        {
            foreach (JournalEntry entrys in World.Player.Journal)
            {
                if (entrys.Name == name)
                    if (entrys.Text.Contains(text))
                        return true;
            }
            return false;
        }
        public static bool SearchByColor(string text, int color)
        {
            foreach (JournalEntry entrys in World.Player.Journal)
            {
                if (entrys.Color == color)
                    if (entrys.Text.Contains(text))
                        return true;
            }
            return false;
        }
        public static bool SearchByType(string text, string type)
        {
            foreach (JournalEntry entrys in World.Player.Journal)
            {
                if (entrys.Type.ToString() == type)
                    if (entrys.Text.Contains(text))
                        return true;
            }
            return false;
        }

        public static string GetLineText(string text)
        {
            string result = "";
            foreach (JournalEntry entrys in World.Player.Journal)
            {
                if (entrys.Text.Contains(text))
                    result = entrys.Text;
            }
            return result;
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
