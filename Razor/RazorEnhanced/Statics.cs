using System.Collections.Generic;

namespace RazorEnhanced
{
	public class Statics
	{
		private static bool m_loaded = false;

		private static void LoadMapData()
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
	}
}