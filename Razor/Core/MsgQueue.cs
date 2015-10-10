using System;
using System.Collections.Generic;

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

		private class MessageTimer : Timer
		{
			internal MessageTimer()
				: base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
			{
			}

			protected override void OnTick()
			{
				if (m_Table.Count <= 0)
					return;

				List<KeyValuePair<string, MsgInfo>> list = new List<KeyValuePair<string, MsgInfo>>(m_Table);
				foreach (KeyValuePair<string, MsgInfo> pair in list)
				{
					string txt = pair.Key.ToString();
					MsgInfo msg = (MsgInfo)pair.Value;

					if (msg.NextSend <= DateTime.Now)
					{
						if (msg.Count > 0)
						{
							if (msg.Lang == "A")
								ClientCommunication.SendToClient(new AsciiMessage(msg.Serial, msg.Body, msg.Type, msg.Hue, msg.Font, msg.Name, msg.Count > 1 ? String.Format("{0} [{1}]", txt, msg.Count) : txt));
							else
								ClientCommunication.SendToClient(new UnicodeMessage(msg.Serial, msg.Body, msg.Type, msg.Hue, msg.Font, msg.Lang, msg.Name, msg.Count > 1 ? String.Format("{0} [{1}]", txt, msg.Count) : txt));
							msg.Count = 0;
							msg.NextSend = DateTime.Now + msg.Delay;
						}
						else
						{
							m_Table.Remove(pair.Key);
						}
					}
				}
			}
		}

		private static Timer m_Timer = new MessageTimer();
		private static Dictionary<string, MsgInfo> m_Table = new Dictionary<string, MsgInfo>();

		static MessageQueue()
		{
			m_Timer.Start();
		}

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
