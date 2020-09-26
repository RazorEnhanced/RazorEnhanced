using System;
using System.Collections.Generic;
using System.Linq;
using Ultima;

namespace RazorEnhanced
{
	public class Statics
	{
		private static bool m_loaded = false;

		internal static void LoadMapData()
		{
			// Inizializza mappe
			Ultima.Map.InitializeMap("Felucca");
			Ultima.Map.InitializeMap("Trammel");
			Ultima.Map.InitializeMap("Ilshenar");
			Ultima.Map.InitializeMap("Malas");
			Ultima.Map.InitializeMap("Tokuno");
			Ultima.Map.InitializeMap("TerMur");
			m_loaded = true;
		}

		// Blocco info sul terreno
		public static int GetLandID(int x, int y, int map)
		{
			if (!m_loaded)
				LoadMapData();

			switch (map)
			{
				case 0:
					return Ultima.Map.Felucca.Tiles.GetLandTile(x, y).ID;
				case 1:
					return Ultima.Map.Trammel.Tiles.GetLandTile(x, y).ID;
				case 2:
					return Ultima.Map.Ilshenar.Tiles.GetLandTile(x, y).ID;
				case 3:
					return Ultima.Map.Malas.Tiles.GetLandTile(x, y).ID;
				case 4:
					return Ultima.Map.Tokuno.Tiles.GetLandTile(x, y).ID;
				case 5:
					return Ultima.Map.TerMur.Tiles.GetLandTile(x, y).ID;
				default:
					Scripts.SendMessageScriptError("Script Error: GetLandID Invalid Map!");
					return 0;
			}
		}

		public static int GetLandZ(int x, int y, int map)
		{
			if (!m_loaded)
				LoadMapData();

			switch (map)
			{
				case 0:
					return Ultima.Map.Felucca.Tiles.GetLandTile(x, y).Z;
				case 1:
					return Ultima.Map.Trammel.Tiles.GetLandTile(x, y).Z;
				case 2:
					return Ultima.Map.Ilshenar.Tiles.GetLandTile(x, y).Z;
				case 3:
					return Ultima.Map.Malas.Tiles.GetLandTile(x, y).Z;
				case 4:
					return Ultima.Map.Tokuno.Tiles.GetLandTile(x, y).Z;
				case 5:
					return Ultima.Map.TerMur.Tiles.GetLandTile(x, y).Z;
				default:
					Scripts.SendMessageScriptError("Script Error: GetLandZ Invalid Map!");
					return 0;
			}
		}
		public static string GetTileName(int itemid)
		{
			try{
				return TileData.ItemTable[itemid].Name;
			}
			catch (Exception e){
				Scripts.SendMessageScriptError("Script Error: GetTileName invalid tileID "+itemid);
				return "";
			}
		}

		public static string GetLandName(int itemid)
		{
			try{
				return TileData.LandTable[itemid].Name;
			}
			catch (Exception e){
				Scripts.SendMessageScriptError("Script Error: GetLandName invalid landID " + itemid);
				return "";
			}
		}

        public static int GetTileHeight(int itemid)
        {
            return TileData.ItemTable[itemid].Height;
        }

        public static bool GetTileFlag(int itemid, string flagname)
		{
			switch (flagname)
			{
				case "None":
					if (TileData.ItemTable[itemid].Flags == TileFlag.None)
						return true;
					else
						return false;

				case "Translucent": // The tile is rendered with partial alpha-transparency.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Translucent) != 0)
						return true;
					else
						return false;

				case "Wall": // The tile is a wall.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Wall) != 0)
						return true;
					else
						return false;

				case "Damaging": // The tile can cause damage when moved over.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Damaging) != 0)
						return true;
					else
						return false;

				case "Impassable": // The tile may not be moved over or through.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Impassable) != 0)
						return true;
					else
						return false;

				case "Surface": // The tile is a surface. It may be moved over, but not through.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Surface) != 0)
						return true;
					else
						return false;

				case "Bridge": // The tile is a stair, ramp, or ladder.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Bridge) != 0)
						return true;
					else
						return false;

				case "Window": // The tile is a window.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Window) != 0)
						return true;
					else
						return false;

				case "NoShoot": // The tile blocks line of sight.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.NoShoot) != 0)
						return true;
					else
						return false;

				case "Foliage": // The tile becomes translucent when walked behind. Boat masts also have this flag.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Foliage) != 0)
						return true;
					else
						return false;

				case "HoverOver": // Gargoyles can fly over
					if ((TileData.ItemTable[itemid].Flags & TileFlag.HoverOver) != 0)
						return true;
					else
						return false;

				case "Roof": // The tile is a slanted roof.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Roof) != 0)
						return true;
					else
						return false;

				case "Door": // The tile is a door. Tiles with this flag can be moved through by ghosts and GMs.
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Door) != 0)
						return true;
					else
						return false;

				case "Wet":
					if ((TileData.ItemTable[itemid].Flags & TileFlag.Wet) != 0)
						return true;
					else
						return false;

				default:
					Scripts.SendMessageScriptError("GetTileFlag: Invalid Flag to check");
					return false;
			}
		}

		public static bool GetLandFlag(int itemid, string flagname)
		{
			switch (flagname)
			{
				case "None":
					if (TileData.LandTable[itemid].Flags == TileFlag.None)
						return true;
					else
						return false;

				case "Translucent": // The tile is rendered with partial alpha-transparency.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Translucent) != 0)
						return true;
					else
						return false;

				case "Wall": // The tile is a wall.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Wall) != 0)
						return true;
					else
						return false;

				case "Damaging": // The tile can cause damage when moved over.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Damaging) != 0)
						return true;
					else
						return false;

				case "Impassable": // The tile may not be moved over or through.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Impassable) != 0)
						return true;
					else
						return false;

				case "Surface": // The tile is a surface. It may be moved over, but not through.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Surface) != 0)
						return true;
					else
						return false;

				case "Bridge": // The tile is a stair, ramp, or ladder.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Bridge) != 0)
						return true;
					else
						return false;

				case "Window": // The tile is a window.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Window) != 0)
						return true;
					else
						return false;

				case "NoShoot": // The tile blocks line of sight.
					if ((TileData.LandTable[itemid].Flags & TileFlag.NoShoot) != 0)
						return true;
					else
						return false;

				case "Foliage": // The tile becomes translucent when walked behind. Boat masts also have this flag.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Foliage) != 0)
						return true;
					else
						return false;

				case "HoverOver": // Gargoyles can fly over
					if ((TileData.LandTable[itemid].Flags & TileFlag.HoverOver) != 0)
						return true;
					else
						return false;

				case "Roof": // The tile is a slanted roof.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Roof) != 0)
						return true;
					else
						return false;

				case "Door": // The tile is a door. Tiles with this flag can be moved through by ghosts and GMs.
					if ((TileData.LandTable[itemid].Flags & TileFlag.Door) != 0)
						return true;
					else
						return false;

				case "Wet":
					if ((TileData.LandTable[itemid].Flags & TileFlag.Wet) != 0)
						return true;
					else
						return false;

				default:
					Scripts.SendMessageScriptError("GetLandFlag: Invalid Flag to check");
					return false;
			}
		}


		// Blocco info su statici
		public class TileInfo
		{
			private int m_StaticID;
			public int StaticID { get { return m_StaticID; } }

			private int m_StaticHue;
			public int StaticHue { get { return m_StaticHue; } }

			private int m_StaticZ;
			public int StaticZ { get { return m_StaticZ; } }

			public TileInfo(int id, int hue, int z)
			{
				m_StaticID = id;
				m_StaticHue = hue;
				m_StaticZ = z;
			}
		}

		public static TileInfo GetStaticsLandInfo(int x, int y, int map)
		{
			if (!m_loaded)
				LoadMapData();

			Ultima.Tile tile;
			TileInfo tileinfo = null;

			switch (map)
			{
				case 0:
					tile = Ultima.Map.Felucca.Tiles.GetLandTile(x, y);
					break;
				case 1:
					tile = Ultima.Map.Trammel.Tiles.GetLandTile(x, y);
					break;
				case 2:
					tile = Ultima.Map.Ilshenar.Tiles.GetLandTile(x, y);
					break;
				case 3:
					tile = Ultima.Map.Malas.Tiles.GetLandTile(x, y);
					break;
				case 4:
					tile = Ultima.Map.Tokuno.Tiles.GetLandTile(x, y);
					break;
				case 5:
					tile = Ultima.Map.TerMur.Tiles.GetLandTile(x, y);
					break;
				default:
					Scripts.SendMessageScriptError("Script Error: GetLandZ Invalid Map!");
					return tileinfo;
			}
			tileinfo = new TileInfo(tile.ID, 0, tile.Z);

			return tileinfo;
		}

		public static List<TileInfo> GetStaticsTileInfo(int x, int y, int map)
		{
			if (!m_loaded)
				LoadMapData();

			Ultima.HuedTile[] tiles;
			List<TileInfo> tileinfo = new List<TileInfo>();

			switch (map)
			{
				case 0:
					tiles = Ultima.Map.Felucca.Tiles.GetStaticTiles(x, y);
					break;
				case 1:
					tiles = Ultima.Map.Trammel.Tiles.GetStaticTiles(x, y);
					break;
				case 2:
					tiles = Ultima.Map.Ilshenar.Tiles.GetStaticTiles(x, y);
					break;
				case 3:
					tiles = Ultima.Map.Malas.Tiles.GetStaticTiles(x, y);
					break;
				case 4:
					tiles = Ultima.Map.Tokuno.Tiles.GetStaticTiles(x, y);
					break;
				case 5:
					tiles = Ultima.Map.TerMur.Tiles.GetStaticTiles(x, y);
					break;
				default:
					Scripts.SendMessageScriptError("Script Error: GetLandZ Invalid Map!");
					return tileinfo;
			}

			if (tiles != null && tiles.Length > 0)
			{
				foreach (Ultima.HuedTile tile in tiles)
				{
					tileinfo.Add(new TileInfo(tile.ID, tile.Hue, tile.Z));
				}
			}

			return tileinfo;
		}

		public static bool CheckDeedHouse(int x, int y)
		{
			List<Multi.MultiData> multidata = Assistant.World.Multis.Values.ToList();

			foreach (Multi.MultiData multi in multidata)
			{
				if (x >= multi.Corner1.X && x <= multi.Corner2.X + 1 && y >= multi.Corner1.Y && y <= multi.Corner2.Y + 1)
				{
					return true;
				}
			}
			return false;
		}
	}
}
