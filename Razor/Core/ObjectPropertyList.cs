using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistant
{
	internal class ObjectPropertyList
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

		internal int Hash
		{
			get { return m_Hash; }
			set { m_Hash = value; }
		}

		internal int ServerHash { get { return m_Hash; } }

		internal ObjectPropertyList(UOEntity owner)
		{
			m_Owner = owner;

			m_StringNums.AddRange(m_DefaultStringNums);
		}

		internal void Read(PacketReader p)
		{
			string argsold = String.Empty;
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

				if (args == argsold)
					continue;
				else
				{
					argsold = args;
					m_Content.Add(new OPLEntry(num, args));
				}
			}

			
		}

		private static byte[] m_Buffer = new byte[0];

		internal void Add(int number, string format, object arg0)
		{
			Add(number, String.Format(format, arg0));
		}

		internal void Add(int number, string format, object arg0, object arg1)
		{
			Add(number, String.Format(format, arg0, arg1));
		}

		internal void Add(int number, string format, object arg0, object arg1, object arg2)
		{
			Add(number, String.Format(format, arg0, arg1, arg2));
		}

		internal void Add(int number, string format, params object[] args)
		{
			Add(number, String.Format(format, args));
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

		private int GetStringNumber()
		{
			if (m_StringNums.Count > 0)
			{
				int num = (int)m_StringNums[0];
				m_StringNums.RemoveAt(0);
				return num;
			}
			else
			{
				return 1049644;
			}
		}

		private const string RazorHTMLFormat = " <CENTER><BASEFONT COLOR=#FF0000>{0}</BASEFONT></CENTER> ";

		internal void Add(string text)
		{
			Add(GetStringNumber(), String.Format(RazorHTMLFormat, text));
		}

		internal void Add(string format, string arg0)
		{
			Add(GetStringNumber(), String.Format(format, arg0));
		}

		internal void Add(string format, string arg0, string arg1)
		{
			Add(GetStringNumber(), String.Format(format, arg0, arg1));
		}

		internal void Add(string format, string arg0, string arg1, string arg2)
		{
			Add(GetStringNumber(), String.Format(format, arg0, arg1, arg2));
		}

		internal void Add(string format, params object[] args)
		{
			Add(GetStringNumber(), String.Format(format, args));
		}
	}

	internal class OPLInfo : Packet
	{
		internal OPLInfo(Serial ser, int hash)
			: base(0xDC, 9)
		{
			Write((uint)ser);
			Write((int)hash);
		}
	}
}