using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Assistant
{
	internal class PasswordMemory
	{
		private class Entry
		{
			internal Entry() { }
			internal Entry(string u, string p, IPAddress a) { User = u; Pass = p; Address = a; }
			internal string User;
			internal string Pass;
			internal IPAddress Address;
		}

		[Serializable]
		internal class PasswordData
		{
			private string m_IP;
			public string IP { get { return m_IP; } }

			private string m_User;
			public string User { get { return m_User; } }

			private string m_Password;
			public string Password { get { return m_Password; } }

			public PasswordData(string ip, string user, string password)
			{
				m_IP = ip;
				m_User = user;
				m_Password = password;
			}
		}

		private static List<Entry> m_List = new List<Entry>();

		internal static string Encrypt(string source)
		{
			byte[] buff = ASCIIEncoding.ASCII.GetBytes(source);
			int kidx = 0;
			string key = ClientCommunication.GetWindowsUserName();
			if (key == String.Empty)
				return String.Empty;
			StringBuilder sb = new StringBuilder(source.Length * 2 + 2);
			sb.Append("1+");
			for (int i = 0; i < buff.Length; i++)
			{
				sb.AppendFormat("{0:X2}", (byte)(buff[i] ^ ((byte)key[kidx++])));
				if (kidx >= key.Length)
					kidx = 0;
			}
			return sb.ToString();
		}

		internal static string Decrypt(string source)
		{
			byte[] buff = null;

			if (source.Length > 2 && source[0] == '1' && source[1] == '+')
			{
				buff = new byte[(source.Length - 2) / 2];
				string key = ClientCommunication.GetWindowsUserName();
				if (key == String.Empty)
					return String.Empty;
				int kidx = 0;
				for (int i = 2; i < source.Length; i += 2)
				{
					byte c;
					try
					{
						c = Convert.ToByte(source.Substring(i, 2), 16);
					}
					catch
					{
						continue;
					}
					buff[(i - 2) / 2] = (byte)(c ^ ((byte)key[kidx++]));
					if (kidx >= key.Length)
						kidx = 0;
				}
			}
			else
			{
				byte key = (byte)(source.Length / 2);
				buff = new byte[key];

				for (int i = 0; i < source.Length; i += 2)
				{
					byte c;
					try
					{
						c = Convert.ToByte(source.Substring(i, 2), 16);
					}
					catch
					{
						continue;
					}
					buff[i / 2] = (byte)(c ^ key++);
				}
			}
			return ASCIIEncoding.ASCII.GetString(buff);
		}

		internal static void Load()
		{
			ClearAll();

			List<PasswordData> allpassword = RazorEnhanced.Settings.Password.RealAll();

			foreach (PasswordData password in allpassword)
			{
				if (password.Password == null)
					continue;
				m_List.Add(new Entry(password.User, password.Password, IPAddress.Parse(password.IP)));
			}
		}

		internal static void Save()
		{
			List<PasswordData> pdata = new List<PasswordData>();
			foreach (Entry e in m_List)
			{
				if (e.Pass != String.Empty)
					pdata.Add(new PasswordData(e.Address.ToString(), e.User, e.Pass));
			}

			RazorEnhanced.Settings.Password.Insert(pdata);
		}

		internal static void ClearAll()
		{
			m_List.Clear();
		}

		internal static void Add(string user, string pass, IPAddress addr)
		{
			if (pass == "")
				return;

			user = user.ToLower();
			for (int i = 0; i < m_List.Count; i++)
			{
				Entry e = m_List[i];
				if (e.User == user && e.Address.Equals(addr))
				{
					e.Pass = Encrypt(pass);
					return;
				}
			}

			m_List.Add(new Entry(user, Encrypt(pass), addr));
		}

		internal static string Find(string user, IPAddress addr)
		{
			user = user.ToLower();
			for (int i = 0; i < m_List.Count; i++)
			{

				Entry e = m_List[i];
				if (e.User == user && e.Address.Equals(addr))
					return Decrypt(e.Pass);
			}

			return String.Empty;
		}
	}
}

