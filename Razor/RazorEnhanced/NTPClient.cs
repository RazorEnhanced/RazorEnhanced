using System;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;

namespace RazorEnhanced
{
	public class NoServerFoundException : System.Exception
	{
		public NoServerFoundException() : base() { }
		public NoServerFoundException(string message) : base(message) { }
		public NoServerFoundException(string message,
				System.Exception inner)
			: base(message, inner) { }
		protected NoServerFoundException(SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) { }
	}

	class NetworkTime
	{
		/* For more info, see:
		 *  NTP (RFC-2030)
		 *  http://tools.ietf.org/html/rfc2030
		 */

		private const int requestTimeout = 3000;
		private const int timesForEachServer = 5;
		private const byte offTime = 40; //Transmit Time (see RFC-2030)
		private uint lastSrv;

		//NIST Servers
		public static string[] srvs = {
        "time.nist.gov",
        "pool.ntp.org",
        "europe.pool.ntp.org",
        "asia.pool.ntp.org",
        "oceania.pool.ntp.org",
        "north-america.pool.ntp.org",
        "south-america.pool.ntp.org",
        "africa.pool.ntp.org",
        "ntp1.inrim.it",
        "ntp2.inrim.it"
    };

		public NetworkTime()
		{
			Random rnd = new Random(DateTime.Now.Millisecond);
			lastSrv = (uint)rnd.Next(0, srvs.Length);
		}

		private IPAddress GetServer()
		{
			lastSrv = (uint)((lastSrv + 1) % srvs.Length);
			IPAddress[] address = Dns.GetHostEntry(srvs[lastSrv]).AddressList;
			if (address == null || address.Length == 0)
				throw new NoServerFoundException("no ip found");
			return address[0];
		}

		public DateTime GetDateTime() { return GetDateTime(false); }
		public DateTime GetDateTime(bool utc)
		{
			//Examine all servers until we find a server that responds
			for (int st = 0; st < srvs.Length * timesForEachServer; st++)
			{
				try
				{
					IPAddress ip = GetServer();
					IPEndPoint ipEndP = new IPEndPoint(ip, 123);

					Socket sk = new Socket(AddressFamily.InterNetwork,
										  SocketType.Dgram,
										  ProtocolType.Udp);
					sk.ReceiveTimeout = requestTimeout;

					sk.Connect(ipEndP);

					/* Request
					 * VN: 4 = NTP/SNTP version 4
					 * Mode: 3 = client
					 */
					byte[] data = new byte[48];
					data[0] = 0x23;
					for (int i = 1; i < 48; i++) data[i] = 0;
					sk.Send(data);

					/* Response
					 * we read the integer part and fraction part
					 * of transmit time (see RFC-2030)
					 */
					sk.Receive(data);
					byte[] integerPart = new byte[4];
					integerPart[0] = data[offTime + 3];
					integerPart[1] = data[offTime + 2];
					integerPart[2] = data[offTime + 1];
					integerPart[3] = data[offTime + 0];
					byte[] fractPart = new byte[4];
					fractPart[0] = data[offTime + 7];
					fractPart[1] = data[offTime + 6];
					fractPart[2] = data[offTime + 5];
					fractPart[3] = data[offTime + 4];
					long ms = (long)(
								(ulong)BitConverter.ToUInt32(integerPart, 0) * 1000
							 + ((ulong)BitConverter.ToUInt32(fractPart, 0) * 1000)
								/ 0x100000000L);
					sk.Close();

					/* DateTime*/
					DateTime date = new DateTime(1900, 1, 1);
					date += TimeSpan.FromTicks(ms * TimeSpan.TicksPerMillisecond);

					return utc ? date : date.ToLocalTime();

				}
				catch { }
			}

			throw new NoServerFoundException("no working server has been found");
		}
	}
}