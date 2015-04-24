using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
	public class Friend
	{
		[Serializable]
		public class FriendPlayer
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Serial;
			public int Serial { get { return m_Serial; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

            public FriendPlayer(string name, int serial, bool selected)
			{
				m_Name = name;
                m_Serial = serial;
				m_Selected = selected;
			}
		}

		internal class FriendList
		{
			private string m_Description;
			internal string Description { get { return m_Description; } }

			private int m_Bag;
			internal int Bag { get { return m_Bag; } }

			private bool m_Selected;
			internal bool Selected { get { return m_Selected; } }

            public FriendList(string description, int bag, bool selected)
			{
				m_Description = description;
				m_Bag = bag;
				m_Selected = selected;
			}
		}		
	}

}