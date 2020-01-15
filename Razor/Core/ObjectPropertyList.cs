using System.Collections.Generic;
using System.Linq;

namespace Assistant
{
	public class ObjectPropertyList
	{
		internal class OPLEntry
		{
			internal int Number = 0;
			internal string Args = null;

			internal OPLEntry(int num)
				: this(num, null)
			{
			}

			internal OPLEntry(int num, string args)
			{
				Number = num;
				Args = args;
			}

			public override string ToString()
			{
				int number = this.Number;
				string args = Assistant.Language.ParseSubCliloc(this.Args);

				string content;
				if (args == null)
					content = Assistant.Language.GetCliloc(number);
				else
					content = Assistant.Language.ClilocFormat(this.Number, args);

				return content;
			}
		}

		private List<int> m_StringNums = new List<int>();

		private int m_Hash = 0;
		private List<OPLEntry> m_Content = new List<OPLEntry>();
		internal List<OPLEntry> Content { get { return m_Content; } }

		private UOEntity m_Owner = null;

		internal ObjectPropertyList(UOEntity owner)
		{
			m_Owner = owner;

			m_StringNums.AddRange(m_DefaultStringNums);
		}

		internal void Read(PacketReader p)
		{
			m_Content.Clear();

			p.Seek(5, System.IO.SeekOrigin.Begin); // seek to packet data

			p.ReadUInt32(); // serial
			p.ReadByte(); // 0
			p.ReadByte(); // 0
			m_Hash = p.ReadInt32();

			m_StringNums.Clear();
			m_StringNums.AddRange(m_DefaultStringNums);

			while (p.Position < p.Length)
			{
				int num = p.ReadInt32();
				if (num == 0)
					break;

				m_StringNums.Remove(num);

				short bytes = p.ReadInt16();
				string args = string.Empty;
				if (bytes > 0)
					args = p.ReadUnicodeStringBE(bytes >> 1);

				if (m_Content.Any(e => e.Number == num))
					continue;
				else
					m_Content.Add(new OPLEntry(num, args));
			}
		}

		private static int[] m_DefaultStringNums = new int[]
		{
			1042971, // ~1_NOTHING~
			1070722, // ~1_NOTHING~
			1063483, // ~1_MATERIAL~ ~2_ITEMNAME~
			1076228, // ~1_DUMMY~ ~2_DUMMY~
			1060847, // ~1_val~ ~2_val~
			1050039, // ~1_NUMBER~ ~2_ITEMNAME~
			// these are ugly:
			//1062613, // "~1_NAME~" (orange)
			//1049644, // [~1_stuff~]
		};
	}
}
