using System;
using System.Linq;
using Ultima;

namespace Assistant.MapUO
{
	internal class MultiTileEntry
	{
		internal ushort m_ItemID;
		internal short m_OffsetX;
		internal short m_OffsetY;
		internal short m_OffsetZ;
	}

	public class SupportFunc
	{
		static internal Ultima.Map GetMap(int mapNum)
		{
			switch (mapNum)
			{
				case 0:
					return Ultima.Map.InitializeMap("Felucca");
				case 1:
					return Ultima.Map.InitializeMap("Trammel");
				case 2:
					return Ultima.Map.InitializeMap("Ilshenar");
				case 3:
					return Ultima.Map.InitializeMap("Malas");
				case 4:
					return Ultima.Map.InitializeMap("Tokuno");
				case 5:
					return Ultima.Map.InitializeMap("TerMur");
				case 6:
					return Ultima.Map.InitializeMap("Custom");
				default:
					return Ultima.Map.InitializeMap("Felucca");
			}
		}

		static internal int MapStringToInt(string name)
		{
			if (name == null | string.IsNullOrEmpty(name))
			{
				return 0;
			}
			name = name.ToLower();
			switch (name)
			{
				case "felucca":
					return 0;
				case "trammel":
					return 1;
				case "ilshenar":
					return 2;
				case "malas":
					return 3;
				case "tokuno":
					return 4;
				case "terMur":
					return 5;
				case "custom":
					return 6;
				default:
					return 0;
			}
		}

		static internal string MapIntToString(int name)
		{
			switch (name)
			{
				case 0:
					return "Felucca";
				case 1:
					return "Trammel";
				case 2:
					return "Ilshenar";
				case 3:
					return "Malas";
				case 4:
					return "Tokuno";
				case 5:
					return "TerMur";
				case 6:
					return "Custom";
				default:
					return "Felucca";
			}
		}

		static internal HuedTile GetTileNear(int mapNum, int x, int y, int z)
		{
			try
			{
				Ultima.Map map = GetMap(mapNum);

				HuedTile[] tiles = map.Tiles.GetStaticTiles(x, y);
				if (tiles != null && tiles.Length > 0)
				{
					foreach (HuedTile tile in tiles)
					{
						if (tile.Z >= z - 5 && tile.Z <= z + 5)
						{
							return tile;
						}
					}
				}
			}
			catch
			{
			}

			return new HuedTile(0, 0, Convert.ToSByte(z));
		}

		private static void GetAverageZ(Ultima.Map map, int x, int y, ref int z, ref int avg, ref int top)
		{
			try
			{
				int zTop = map.Tiles.GetLandTile(x, y).Z;
				int zLeft = map.Tiles.GetLandTile(x, y + 1).Z;
				int zRight = map.Tiles.GetLandTile(x + 1, y).Z;
				int zBottom = map.Tiles.GetLandTile(x + 1, y + 1).Z;

				z = zTop;
				if (zLeft < z)
				{
					z = zLeft;
				}
				if (zRight < z)
				{
					z = zRight;
				}
				if (zBottom < z)
				{
					z = zBottom;
				}

				top = zTop;
				if (zLeft > top)
				{
					top = zLeft;
				}
				if (zRight > top)
				{
					top = zRight;
				}
				if (zBottom > top)
				{
					top = zBottom;
				}

				if (Math.Abs(zTop - zBottom) > Math.Abs(zLeft - zRight))
				{
					avg = Convert.ToInt32(Math.Floor((zLeft + zRight) / 2.0));
				}
				else
				{
					avg = Convert.ToInt32(Math.Floor((zTop + zBottom) / 2.0));
				}
			}
			catch
			{
			}
		}

		internal static sbyte ZTop(int mapNum, int xCheck, int yCheck, int oldZ)
		{
			try
			{
				Ultima.Map map = GetMap(mapNum);

				Tile landTile = map.Tiles.GetLandTile(xCheck, yCheck);
				int landZ = 0, landCenter = 0, zTop = 0;

				GetAverageZ(map, xCheck, yCheck, ref landZ, ref landCenter, ref zTop);

				if (zTop > oldZ)
					oldZ = zTop;

				bool isSet = false;
				HuedTile[] staticTiles = map.Tiles.GetStaticTiles(xCheck, yCheck);

				foreach (HuedTile tile in staticTiles)
				{
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					int calcTop = (tile.Z + id.CalcHeight);

					if (calcTop <= oldZ + 5 && (!isSet || calcTop > zTop) && ((id.Flags & TileFlag.Surface) != 0 || (id.Flags & TileFlag.Wet) != 0))
					{
						zTop = calcTop;
						isSet = true;
					}
				}

				return (sbyte)zTop;
			}
			catch
			{
				return (sbyte)oldZ;
			}
		}





		internal static void AddMyUser(short flag)
		{

			bool found = false;
			foreach (MapNetworkIn.UserData user in MapNetwork.UData)
			{
				if (user.Nome == World.Player.Name)
				{
					found = true;
					UpdateUserProperties(user.Nome, "coordinates", (short)World.Player.Position.X, (short)World.Player.Position.Y, World.Player.Map, (short)World.Player.Hits, (short)World.Player.Stam, (short)World.Player.Mana,
					(short)World.Player.HitsMax, (short)World.Player.StamMax, (short)World.Player.ManaMax, flag, RazorEnhanced.Settings.General.ReadInt("MapChatColor"));

					UpdateUserProperties(user.Nome, "stats", 0, 0, 0, (short)World.Player.Hits, (short)World.Player.Stam, (short)World.Player.Mana, (short)World.Player.HitsMax, (short)World.Player.StamMax,
					(short)World.Player.ManaMax, 0, RazorEnhanced.Settings.General.ReadInt("MapChatColor"));

					UpdateUserProperties(user.Nome, "flags", (short)World.Player.Position.X, (short)World.Player.Position.Y, World.Player.Map, 0, 0, 0, 0, 0,
					0, flag, RazorEnhanced.Settings.General.ReadInt("MapChatColor"));
					return;
				}
			}

			if (!found)
				MapNetwork.UData.Add(new MapNetworkIn.UserData(World.Player.Name, (short)World.Player.Position.Y, World.Player.Map,
					0, 0, 0,
					0, 0, 0,
					(short)World.Player.Hits, (short)World.Player.HitsMax, (short)World.Player.Stam, (short)World.Player.StamMax,
					(short)World.Player.HitsMax, (short)World.Player.StamMax, (short)World.Player.ManaMax, flag));
		}



		internal static void UpdateUserProperties(string user, string type, short X, short Y, short Mappa, short hp, short stam, short mana, short maxhp, short maxstam,
		short maxmana, short flag, int col)
		{
			MapNetworkIn.UserData obj = MapNetwork.UData.FirstOrDefault(abc => abc.Nome == user);
			if (obj != null)
			{
				switch (type)
				{
					case "coordinates":
						obj.X = X;
						obj.Y = Y;
						obj.Facet = Mappa;
						break;
					case "stats":
						obj.Hits = hp;
						obj.Stamina = stam;
						obj.Mana = mana;
						obj.HitsMax = maxhp;
						obj.ManaMax = maxstam;
						obj.StaminaMax = maxmana;
						break;
					case "flags":
						obj.Flag = flag;
						break;
					case "panic":
						obj.PanicPointX = X;
						obj.PanicPointY = Y;
						obj.PanicPointFacet = Mappa;
						break;
					case "death":
						obj.DeathPointX = X;
						obj.DeathPointY = Y;
						obj.DeathPointFacet = 1;  // Da sistemare
						break;
				}
			}
		}
		public static void ClearCache(Ultima.Map Map)
		{
			if (Map.Tiles != null)
				Map.Tiles.Dispose();
			if (Map.Tiles.StaticIndexInit == true)
				Map.Tiles.StaticIndexInit = false;
			if (Map.m_Cache != null)
				Map.m_Cache = null;
			if (Map.m_Tiles != null)
				Map.m_Tiles = null;
			if (Map.m_Cache_NoStatics != null)
				Map.m_Cache_NoStatics = null;
			if (Map.m_Cache_NoPatch != null)
				Map.m_Cache_NoPatch = null;
			if (Map.m_Cache_NoStatics_NoPatch != null)
				Map.m_Cache_NoStatics_NoPatch = null;
			GC.Collect();
			GC.WaitForFullGCComplete();
		}

		public static void RemoveFakeUser(string fakeuser)
		{
			int index = 0;
			foreach (MapNetworkIn.UserData user in MapNetwork.UData)
			{
				if (user.Nome == fakeuser)
				{
					MapNetwork.UData.RemoveAt(index);
					if (fakeuser == "_DEATH_")
						MapUO.UOMapControl.m_booldeathpoint = false;
					break;
				}
				index += 1;
			}
		}
		public void AddFakeUser(string nome, int x, int y, int facet)
		{
			MapNetwork.UData.Add(new MapNetworkIn.UserData(nome, (short)x, (short)y, (short)facet, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0));
			if (nome == "_DEATH_")
			{
				MapUO.UOMapControl.m_timeafterdeath = DateTime.Now;
			}
		}


	}
}