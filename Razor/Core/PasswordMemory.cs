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
			public string IP { get; set;}

			public string User { get; set; }

			public string Password { get; set; }

			public PasswordData(string ip, string user, string password)
			{
				IP = ip;
				User = user;
				Password = password;
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
