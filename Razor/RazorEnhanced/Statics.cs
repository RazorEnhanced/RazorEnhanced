using System;
using System.Collections.Generic;
using System.Linq;
using Ultima;



namespace RazorEnhanced
{
    /// <summary>
    /// The Statics class provides access to informations about the Map, down to the individual tile.
    /// When using this function it's important to remember the distinction between Land and Tile:
    /// Land
    /// ----
    /// For a given (X,Y,map) there can be only 1 (0 zero) Land item, and has 1 specific Z coordinate.
    /// 
    /// Tile
    /// ----
    /// For a given (X,Y,map) there can be any number of Tile items.
    /// </summary>
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
        /// <summary>
        /// Land: Return the StaticID of the Land item, give the coordinates and map.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="map">
        ///     0 = Felucca
		///		1 = Trammel
        ///     2 = Ilshenar
		///	    3 = Malas
		///		4 = Tokuno
        ///		5 = TerMur
        /// </param>
        /// <returns>Return the StaticID of the Land tile </returns>
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

        /// <summary>
        /// Land: Return the Z coordinate (height) of the Land item, give the coordinates and map.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="map">
        ///     0 = Felucca
		///		1 = Trammel
        ///     2 = Ilshenar
		///	    3 = Malas
		///		4 = Tokuno
        ///		5 = TerMur
        /// </param>
        /// <returns></returns>

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

        /// <summary>
        /// Land: Get the name of a Land item given the StaticID.
        /// </summary>
        /// <param name="StaticID">Land item StaticID.</param>
        /// <returns>The name of the Land item.</returns>
        public static string GetLandName(int StaticID)
        {
            try
            {
                return TileData.LandTable[StaticID].Name;
            }
            catch (Exception)
            {
                Scripts.SendMessageScriptError("Script Error: GetLandName invalid landID " + StaticID);
                return "";
            }
        }

        /// <summary>
        /// Tile: Get the name of a Tile item given the StaticID.
        /// </summary>
        /// <param name="StaticID">Tile item StaticID.</param>
        /// <returns>The name of the Land item.</returns>
        public static string GetTileName(int StaticID)
		{
			try{
				return TileData.ItemTable[StaticID].Name;
			}
			catch (Exception){
				Scripts.SendMessageScriptError("Script Error: GetTileName invalid tileID "+StaticID);
				return "";
			}
		}

        /// <summary>
        /// Tile: Get hight of a Tile item, in Z coordinate reference.
        /// </summary>
        /// <param name="StaticID">Tile item StaticID.</param>
        /// <returns>Height of a Tile item.</returns>
        public static int GetTileHeight(int StaticID)
        {
            return TileData.ItemTable[StaticID].Height;
        }

        /// <summary>
        /// Tile: Check Flag value of a given Tile item.
        /// </summary>
        /// <param name="StaticID">StaticID of a Tile item.</param>
        /// <param name="flagname">
        ///     None
        ///     Translucent
        ///     Wall
        ///     Damaging
        ///     Impassable
        ///     Surface
        ///     Bridge
        ///     Window
        ///     NoShoot
        ///     Foliage
        ///     HoverOver
        ///     Roof
        ///     Door
        ///     Wet
        /// </param>
        /// <returns>True: if the Flag is active - False: otherwise</returns>
        public static bool GetTileFlag(int StaticID, string flagname)
		{
			switch (flagname)
			{
				case "None":
					if (TileData.ItemTable[StaticID].Flags == TileFlag.None)
						return true;
					else
						return false;

				case "Translucent": // The tile is rendered with partial alpha-transparency.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Translucent) != 0)
						return true;
					else
						return false;

				case "Wall": // The tile is a wall.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Wall) != 0)
						return true;
					else
						return false;

				case "Damaging": // The tile can cause damage when moved over.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Damaging) != 0)
						return true;
					else
						return false;

				case "Impassable": // The tile may not be moved over or through.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Impassable) != 0)
						return true;
					else
						return false;

				case "Surface": // The tile is a surface. It may be moved over, but not through.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Surface) != 0)
						return true;
					else
						return false;

				case "Bridge": // The tile is a stair, ramp, or ladder.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Bridge) != 0)
						return true;
					else
						return false;

				case "Window": // The tile is a window.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Window) != 0)
						return true;
					else
						return false;

				case "NoShoot": // The tile blocks line of sight.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.NoShoot) != 0)
						return true;
					else
						return false;

				case "Foliage": // The tile becomes translucent when walked behind. Boat masts also have this flag.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Foliage) != 0)
						return true;
					else
						return false;

				case "HoverOver": // Gargoyles can fly over
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.HoverOver) != 0)
						return true;
					else
						return false;

				case "Roof": // The tile is a slanted roof.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Roof) != 0)
						return true;
					else
						return false;

				case "Door": // The tile is a door. Tiles with this flag can be moved through by ghosts and GMs.
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Door) != 0)
						return true;
					else
						return false;

				case "Wet":
					if ((TileData.ItemTable[StaticID].Flags & TileFlag.Wet) != 0)
						return true;
					else
						return false;

				default:
					Scripts.SendMessageScriptError("GetTileFlag: Invalid Flag to check");
					return false;
			}
		}

        /// <summary>
        /// Land: Check Flag value of a given Land item.
        /// </summary>
        /// <param name="staticID">StaticID of a Land item.</param>
        /// <param name="flagname">
        ///     None
        ///     Translucent
        ///     Wall
        ///     Damaging
        ///     Impassable
        ///     Surface
        ///     Bridge
        ///     Window
        ///     NoShoot
        ///     Foliage
        ///     HoverOver
        ///     Roof
        ///     Door
        ///     Wet
        /// </param>
        /// <returns>True: if the Flag is active - False: otherwise</returns>
        public static bool GetLandFlag(int staticID, string flagname)
		{
			switch (flagname)
			{
				case "None":
					if (TileData.LandTable[staticID].Flags == TileFlag.None)
						return true;
					else
						return false;

				case "Translucent": // The tile is rendered with partial alpha-transparency.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Translucent) != 0)
						return true;
					else
						return false;

				case "Wall": // The tile is a wall.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Wall) != 0)
						return true;
					else
						return false;

				case "Damaging": // The tile can cause damage when moved over.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Damaging) != 0)
						return true;
					else
						return false;

				case "Impassable": // The tile may not be moved over or through.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Impassable) != 0)
						return true;
					else
						return false;

				case "Surface": // The tile is a surface. It may be moved over, but not through.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Surface) != 0)
						return true;
					else
						return false;

				case "Bridge": // The tile is a stair, ramp, or ladder.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Bridge) != 0)
						return true;
					else
						return false;

				case "Window": // The tile is a window.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Window) != 0)
						return true;
					else
						return false;

				case "NoShoot": // The tile blocks line of sight.
					if ((TileData.LandTable[staticID].Flags & TileFlag.NoShoot) != 0)
						return true;
					else
						return false;

				case "Foliage": // The tile becomes translucent when walked behind. Boat masts also have this flag.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Foliage) != 0)
						return true;
					else
						return false;

				case "HoverOver": // Gargoyles can fly over
					if ((TileData.LandTable[staticID].Flags & TileFlag.HoverOver) != 0)
						return true;
					else
						return false;

				case "Roof": // The tile is a slanted roof.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Roof) != 0)
						return true;
					else
						return false;

				case "Door": // The tile is a door. Tiles with this flag can be moved through by ghosts and GMs.
					if ((TileData.LandTable[staticID].Flags & TileFlag.Door) != 0)
						return true;
					else
						return false;

				case "Wet":
					if ((TileData.LandTable[staticID].Flags & TileFlag.Wet) != 0)
						return true;
					else
						return false;

				default:
					Scripts.SendMessageScriptError("GetLandFlag: Invalid Flag to check");
					return false;
			}
		}


        // Blocco info su statici
        /// <summary>
        /// The TileInfo class hold the values represeting Tile or Land items for a given X,Y coordinate.
        /// </summary>
        public class TileInfo
		{
			private readonly int m_StaticID;
			public int StaticID { get { return m_StaticID; } }

			private readonly int m_StaticHue;
			public int StaticHue { get { return m_StaticHue; } }

			private readonly int m_StaticZ;
			public int StaticZ { get { return m_StaticZ; } }

			public TileInfo(int id, int hue, int z)
			{
				m_StaticID = id;
				m_StaticHue = hue;
				m_StaticZ = z;
			}
		}

        /// <summary>
        /// Land: Return a TileInfo representing the Land item for a given X,Y, map.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="map">
        ///     0 = Felucca
		///		1 = Trammel
        ///     2 = Ilshenar
		///	    3 = Malas
		///		4 = Tokuno
        ///		5 = TerMur
        /// </param>
        /// <returns>A single TileInfo related a Land item.</returns>
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

        /// <summary>
        /// Tile: Return a list of TileInfo representing the Tile items for a given X,Y, map.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="map">
        ///     0 = Felucca
		///		1 = Trammel
        ///     2 = Ilshenar
		///	    3 = Malas
		///		4 = Tokuno
        ///		5 = TerMur
        /// </param>
        /// <returns>A list of TileInfo related to Tile items.</returns>
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

        /// <summary>
        /// Check if the given Tile is occupied by a private house.
        /// Need to be in-sight, on most servers the maximum distance is 18 tiles.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>True: The tile is occupied - False: otherwise</returns>
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
