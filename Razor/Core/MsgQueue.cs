using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace Assistant
{
	internal class MessageQueue
	{
		private class MsgInfo
		{
			internal MsgInfo(Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name)
			{
				Serial = ser; Body = body; Type = type; Hue = hue; Font = font; Lang = lang; Name = name;
			}

			internal TimeSpan Delay;
			internal DateTime NextSend;
			internal int Count;

			//ser, body, type, hue, font, lang, name
			internal Serial Serial;

			internal ushort Body, Hue, Font;
			internal MessageType Type;
			internal string Lang, Name;
		}

		private static void OnTick(object state)
		{
			if (m_Table.Count <= 0)
				return;

			ConcurrentBag<KeyValuePair<string, MsgInfo>> list = new ConcurrentBag<KeyValuePair<string, MsgInfo>>(m_Table);
			foreach (KeyValuePair<string, MsgInfo> pair in list)
			{
				string txt = pair.Key.ToString();
				MsgInfo msg = (MsgInfo)pair.Value;

				if (msg.NextSend > DateTime.Now)
					continue;

				if (msg.Count > 0)
				{
					if (msg.Lang == "A")
				 		Assistant.Client.Instance.SendToClientWait(new AsciiMessage(msg.Serial, msg.Body, msg.Type, msg.Hue, msg.Font, msg.Name, msg.Count > 1 ? String.Format("{0} [{1}]", txt, msg.Count) : txt));
					else
				 		Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(msg.Serial, msg.Body, msg.Type, msg.Hue, msg.Font, msg.Lang, msg.Name, msg.Count > 1 ? String.Format("{0} [{1}]", txt, msg.Count) : txt));
					msg.Count = 0;
					msg.NextSend = DateTime.Now + msg.Delay;
				}
				else
				{
					MsgInfo removed;
                    m_Table.TryRemove(pair.Key, out removed);
				}
			}
		}

		private static System.Threading.Timer m_Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTick), null, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1));
		private static ConcurrentDictionary<string, MsgInfo> m_Table = new ConcurrentDictionary<string, MsgInfo>();

		internal static bool Enqueue(Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name, string text)
		{
			MsgInfo m = null;
			if (m_Table.ContainsKey(text))
				m = m_Table[text] as MsgInfo;

			if (m == null)
			{
				m_Table[text] = m = new MsgInfo(ser, body, type, hue, font, lang, name);

				m.Count = 0;

				m.Delay = TimeSpan.FromSeconds((((int)(text.Length / 50)) + 1) * 3.5);

				m.NextSend = DateTime.Now + m.Delay;

				return true;
			}
			else
			{
				m.Count++;

				return false;
			}
		}
	}
}
