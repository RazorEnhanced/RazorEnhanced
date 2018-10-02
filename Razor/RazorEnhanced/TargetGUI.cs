using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class TargetGUI
	{
		[Serializable]
		public class TargetGUIObject
		{
			private string m_Selector;
			public string Selector { get { return m_Selector; } }

			private Mobiles.Filter m_Filter;
			public Mobiles.Filter Filter { get { return m_Filter; } }

			public TargetGUIObject(string selector, Mobiles.Filter filter)
			{
				m_Selector = selector;
				m_Filter = filter;
			}
		}

		// Selector List
		internal static List<string> Selectors = new List<string>
		{
			"Random",
			"Nearest",
			"Farthest",
			"Weakest",
			"Strongest",
			"Next"
		};

		internal static void RefreshTargetShortCut(ListBox t)
		{
			t.Items.Clear();
			List<string> shortcutlist = Settings.Target.ReadAllShortCut();
			foreach (string shortcut in shortcutlist)
			{
				t.Items.Add(shortcut);
			}
			if (t.Items.Count > 0)
				t.SelectedIndex = 0;
		}
	}
}