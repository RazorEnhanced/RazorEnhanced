using System;
using System.Collections.Generic;

namespace Assistant
{
	internal delegate void PacketViewerCallback(PacketReader p, PacketHandlerEventArgs args);

	internal delegate void PacketFilterCallback(Packet p, PacketHandlerEventArgs args);

	public class PacketHandlerEventArgs
	{
		private bool m_Block;

		internal bool Block
		{
			get { return m_Block; }
			set { m_Block = value; }
		}

		internal PacketHandlerEventArgs()
		{
			Reinit();
		}

		internal void Reinit()
		{
			m_Block = false;
		}
	}

	internal class PacketHandler
	{
		private static Dictionary<int, List<PacketViewerCallback>> m_ClientViewers;
		private static Dictionary<int, List<PacketViewerCallback>> m_ServerViewers;

		private static Dictionary<int, List<PacketFilterCallback>> m_ClientFilters;
		private static Dictionary<int, List<PacketFilterCallback>> m_ServerFilters;

		static PacketHandler()
		{
			m_ClientViewers = new Dictionary<int, List<PacketViewerCallback>>();
			m_ServerViewers = new Dictionary<int, List<PacketViewerCallback>>();

			m_ClientFilters = new Dictionary<int, List<PacketFilterCallback>>();
			m_ServerFilters = new Dictionary<int, List<PacketFilterCallback>>();
		}

		internal static void RegisterClientToServerViewer(int packetID, PacketViewerCallback callback)
		{
			if (!m_ClientViewers.ContainsKey(packetID))
				m_ClientViewers.Add(packetID, new List<PacketViewerCallback>());

			if (!m_ClientViewers[packetID].Contains(callback))
			{
				List<PacketViewerCallback> list = m_ClientViewers[packetID];
				list.Add(callback);
				m_ClientViewers[packetID] = list;
			}
		}

		internal static void RegisterServerToClientViewer(int packetID, PacketViewerCallback callback)
		{
			if (!m_ServerViewers.ContainsKey(packetID))
				m_ServerViewers.Add(packetID, new List<PacketViewerCallback>());

			if (!m_ServerViewers[packetID].Contains(callback))
			{
				List<PacketViewerCallback> list = m_ServerViewers[packetID];
				list.Add(callback);
				m_ServerViewers[packetID] = list;
			}
		}

		internal static void RemoveClientToServerViewer(int packetID, PacketViewerCallback callback)
		{
			if (m_ClientViewers.ContainsKey(packetID))
				m_ClientViewers[packetID].Remove(callback);
		}

		internal static void RemoveServerToClientViewer(int packetID, PacketViewerCallback callback)
		{
			if (m_ServerViewers.ContainsKey(packetID))
				m_ServerViewers[packetID].Remove(callback);
		}

		internal static void RegisterClientToServerFilter(int packetID, PacketFilterCallback callback)
		{
			if (!m_ClientFilters.ContainsKey(packetID))
				m_ClientFilters.Add(packetID, new List<PacketFilterCallback>());

			if (!m_ClientFilters[packetID].Contains(callback))
			{
				List<PacketFilterCallback> list = m_ClientFilters[packetID];
				list.Add(callback);
				m_ClientFilters[packetID] = list;
			}
		}

		internal static void RegisterServerToClientFilter(int packetID, PacketFilterCallback callback)
		{
			if (!m_ServerFilters.ContainsKey(packetID))
				m_ServerFilters.Add(packetID, new List<PacketFilterCallback>());

			if (!m_ServerFilters[packetID].Contains(callback))
			{
				List<PacketFilterCallback> list = m_ServerFilters[packetID];
				list.Add(callback);
				m_ServerFilters[packetID] = list;
			}
		}

		internal static void RemoveClientToServerFilter(int packetID, PacketFilterCallback callback)
		{
			if (m_ClientFilters.ContainsKey(packetID))
				m_ClientFilters[packetID].Remove(callback);
		}

		internal static void RemoveServerToClientFilter(int packetID, PacketFilterCallback callback)
		{
			if (m_ServerFilters.ContainsKey(packetID))
				m_ServerFilters[packetID].Remove(callback);
		}

		internal static bool OnServerPacket(int id, PacketReader pr, Packet p)
		{
			bool result = false;
			if (pr != null)
			{
				List<PacketViewerCallback> list;
				m_ServerViewers.TryGetValue(id, out list);
				if (list != null && list.Count > 0)
					result = ProcessViewers(list, pr);
			}

			if (p != null)
			{
                List<PacketFilterCallback> list;
				m_ServerFilters.TryGetValue(id, out list);
				if (list != null && list.Count > 0)
					result |= ProcessFilters(list, p);
			}

			return result;
		}

		internal static bool OnClientPacket(int id, PacketReader pr, Packet p)
		{
			bool result = false;
			if (pr != null)
			{
				List<PacketViewerCallback> list;
				m_ClientViewers.TryGetValue(id, out list);
				if (list != null && list.Count > 0)
					result = ProcessViewers(list, pr);
			}

			if (p != null)
			{
				List<PacketFilterCallback> list;
				m_ClientFilters.TryGetValue(id, out list);
				if (list != null && list.Count > 0)
					result |= ProcessFilters(list, p);
			}

			return result;
		}

		internal static bool HasClientViewer(int packetID)
		{
			List<PacketViewerCallback> list;
			m_ClientViewers.TryGetValue(packetID, out list);
			return list != null && list.Count > 0;
		}

		internal static bool HasServerViewer(int packetID)
		{
			List<PacketViewerCallback> list;
			m_ServerViewers.TryGetValue(packetID, out list);
			return list != null && list.Count > 0;
		}

		internal static bool HasClientFilter(int packetID)
		{
			List<PacketFilterCallback> list;
			m_ClientFilters.TryGetValue(packetID, out list);
			return (list != null && list.Count > 0) || false;
		}

		internal static bool HasServerFilter(int packetID)
		{
			List<PacketFilterCallback> list;
			m_ServerFilters.TryGetValue(packetID, out list);
			return (list != null && list.Count > 0) || false;
		}

		private static PacketHandlerEventArgs m_Args = new PacketHandlerEventArgs();

		private static bool ProcessViewers(List<PacketViewerCallback> list, PacketReader p)
		{
			m_Args.Reinit();

			if (list == null)
				return m_Args.Block;

			foreach (PacketViewerCallback t in list)
			{
				p.MoveToData();

				try
				{
					t(p, m_Args);
				}
				catch (Exception e)
				{
					Engine.LogCrash(e);
				}
			}

			return m_Args.Block;
		}

		private static bool ProcessFilters(List<PacketFilterCallback> list, Packet p)
		{
			m_Args.Reinit();

			if (list == null)
				return m_Args.Block;

			foreach (PacketFilterCallback t in list)
			{
				p.MoveToData();

				try
				{
					t(p, m_Args);
				}
				catch (Exception e)
				{
					Engine.LogCrash(e);
				}
			}

			return m_Args.Block;
		}
	}
}
