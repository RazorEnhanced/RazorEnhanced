using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
  public class Organizer
	{
		[Serializable]
		public class OrganizerItem
		{
			
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

            private int m_amount;
            public int Amount { get { return m_amount; } }
			
            public OrganizerItem(string name, int graphics, int color, int amount)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
                m_amount = amount;
			}
		}
  }
}
