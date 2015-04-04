using System;
using System.Collections.Generic;

namespace RazorEnhanced
{
	internal class Shard
	{
		private string m_Description;
		internal string Description { get { return m_Description; } }

		private string m_ClientPath;
		internal string ClientPath { get { return m_ClientPath; } }

		private string m_ClientFolder;
		internal string ClientFolder { get { return m_ClientFolder; } }

		private string m_Host;
		internal string Host { get { return m_Host; } }

		private int m_Port;
		internal int Port { get { return m_Port; } }

		private bool m_PatchEnc;
		internal bool PatchEnc { get { return m_PatchEnc; } }

		private bool m_OSIEnc;
		internal bool OSIEnc { get { return m_OSIEnc; } }

		private bool m_Selected;
		internal bool Selected { get { return m_Selected; } }

		public Shard(string description, string clientpath, string clientfolder, string host, int port, bool patchenc, bool osienc, bool selected)
		{
			m_Description = description;
			m_ClientPath = clientpath;
			m_ClientFolder = clientfolder;
			m_Host = host;
			m_Port = port;
			m_PatchEnc = patchenc;
			m_OSIEnc = osienc;
			m_Selected = selected;
		}

	}
}
