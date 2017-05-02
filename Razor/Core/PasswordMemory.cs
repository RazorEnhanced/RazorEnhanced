using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Assistant
{
	internal class PasswordMemory
	{
		private static List<PasswordData> list = new List<PasswordData>();

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

		internal static void ProfileChangeInit()
		{
			list = RazorEnhanced.Settings.Password.RealAll();
		}

		internal static void ProfileChangeEnd()
		{
			RazorEnhanced.Settings.Password.InsertAll(list);
		}
		
	}
}