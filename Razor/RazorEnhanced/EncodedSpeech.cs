using Assistant;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace RazorEnhanced
{ 
	public class EncodedSpeech
	{
		[DllImport("Kernel32", EntryPoint = "_lread")] 
        private static extern unsafe int lread(IntPtr hFile, void* lpBuffer, int wBytes);

		private static Queue m_DataStores = new Queue();

		public class SpeechEntry : IComparable
		{
			public short m_KeywordID;
			public string[] m_Keywords;


			public SpeechEntry(int idKeyword, string keyword)
			{
				m_KeywordID = (short)idKeyword;
				m_Keywords = keyword.Split(new char[] { '*' });
			}


			public int CompareTo(object x)
			{
				if ((x == null) || (x.GetType() != typeof(SpeechEntry)))
				{
					return -1;
				}
				if (x != this)
				{
					SpeechEntry entry = (SpeechEntry)x;
					if (m_KeywordID < entry.m_KeywordID)
					{
						return -1;
					}
					if (m_KeywordID > entry.m_KeywordID)
					{
						return 1;
					}
				}
				return 0;
			}
		}


		private static SpeechEntry[] m_Speech;

		private static unsafe int NativeRead(FileStream fs, void* pBuffer, int bytes)
		{
			return lread(fs.Handle, pBuffer, bytes);
		}

		private static unsafe int NativeRead(FileStream fs, byte[] buffer, int offset, int length)
		{
			fixed (byte* numRef = buffer)
			{
				return NativeRead(fs, (void*)(numRef + offset), length);
			}
		}

		internal static unsafe void LoadSpeechTable()
		{
			string path = Ultima.Files.GetFilePath("Speech.mul");
			if (!File.Exists(path))
			{
				m_Speech = new SpeechEntry[0];
			}
			else
			{
				byte[] buffer = new byte[0x400];
				fixed (byte* numRef = buffer)
				{
					ArrayList list = new ArrayList();
					FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
					int num = 0;
					while ((num = NativeRead(fs, (void*)numRef, 4)) > 0)
					{
						int idKeyword = numRef[1] | (numRef[0] << 8);
						int bytes = numRef[3] | (numRef[2] << 8);
						if (bytes > 0)
						{
							NativeRead(fs, (void*)numRef, bytes);
							list.Add(new SpeechEntry(idKeyword, new string((sbyte*)numRef, 0, bytes)));
						}
					}
					fs.Close();
					m_Speech = (SpeechEntry[])list.ToArray(typeof(SpeechEntry));
				}
			}
		}
		internal static List<ushort> GetKeywords(string text)
         { 
             if (m_Speech == null) 
             { 
                 LoadSpeechTable(); 
             } 
            text = text.ToLower(); 
            ArrayList dataStore = GetDataStore(); 
            SpeechEntry[] speech = m_Speech; 
            int length = speech.Length; 
            for (int i = 0; i<length; i++) 
             { 
                 SpeechEntry entry = speech[i]; 
                 if (IsMatch(text, entry.m_Keywords))
				{ 
                     dataStore.Add(entry); 
                 } 
             } 
             dataStore.Sort(); 
             SpeechEntry[] keywords = (SpeechEntry[])dataStore.ToArray(typeof(SpeechEntry)); 
             ReleaseDataStore(dataStore);

			int numk = keywords.Length & 15;
			List<ushort> keynumber = new List<ushort>();
			bool flag = false;
			int index = 0;
			while (index < keywords.Length)
			{
				EncodedSpeech.SpeechEntry entry = keywords[index];
				int keywordID = entry.m_KeywordID;
				if (flag)
				{
					keynumber.Add((byte)(keywordID >> 4));
					numk = keywordID & 15;
				}
				else
				{
					keynumber.Add((byte)((numk << 4) | ((keywordID >> 8) & 15)));
					keynumber.Add((byte)keywordID);
				}
				index++;
				flag = !flag;
			}
			if (!flag)
			{
				keynumber.Add((byte)(numk << 4));
			}

			return keynumber; 
         }

		private static ArrayList GetDataStore()
		{
			if (m_DataStores.Count > 0)
			{
				return (ArrayList)m_DataStores.Dequeue();
			}
			return new ArrayList();
		}


		private static void ReleaseDataStore(ArrayList list)
		{
			if (list.Count > 0)
			{
				list.Clear();
			}
			m_DataStores.Enqueue(list);
		}

		private static bool IsMatch(string input, string[] split)
		{
			int startIndex = 0;
			for (int i = 0; i < split.Length; i++)
			{
				if (split[i].Length > 0)
				{
					int index = input.IndexOf(split[i], startIndex);
					if ((index > 0) && (i == 0))
					{
						return false;
					}
					if (index < 0)
					{
						return false;
					}
					startIndex = index + split[i].Length;
				}
			}
			return ((split[split.Length - 1].Length <= 0) || (startIndex == input.Length));
		}

		internal sealed class ClientUniMessage : Packet
		{
			internal ClientUniMessage(MessageType type, int hue, int font, string lang, string text, SpeechEntry[] keys)
				: base(0xAD)
			{
				if (lang == null || lang == "") lang = "ENU";
				if (text == null) text = "";

				this.EnsureCapacity(50 + (text.Length * 2) + (keys == null ? 0 : keys.Length));
				if (keys == null || keys.Length <= 0)
					Write((byte)type);
				else
					Write((byte)(type | MessageType.Encoded));
				Write((short)hue);
				Write((short)font);
				WriteAsciiFixed(lang, 4);


				if (keys != null || (keys.Length <= 0))
				{
					WriteBigUniNull(text);
				}
				else
				{
                    Write((byte)(keys.Length >> 4));
					int num = keys.Length & 15;
					bool flag = false;
					int index = 0;
					while (index < keys.Length)
					{
						SpeechEntry entry = keys[index];
						int keywordID = entry.m_KeywordID;
						if (flag)
						{
							Write((byte)(keywordID >> 4));
							num = keywordID & 15;
						}
						else
						{
							Write((byte)((num << 4) | ((keywordID >> 8) & 15)));
							Write((byte)keywordID);
						}
						index++;
						flag = !flag;
					}
					if (!flag)
					{
						Write((byte)(num << 4));
					}
					WriteUTF8Null(text);
				}
			}
		}
	}
}