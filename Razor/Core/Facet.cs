using System;
using Ultima;

namespace Assistant
{
	internal class MultiTileEntry
	{
	}

	internal class Facet
	{
		internal static Ultima.Map GetMap(int mapNum)
		{
			switch (mapNum)
			{
				case 1: return Ultima.Map.Trammel;
				case 2: return Ultima.Map.Ilshenar;
				case 3: return Ultima.Map.Malas;
				case 4: return Ultima.Map.Tokuno;
				case 5: return Ultima.Map.TerMur;
				case 0:
				default: return Ultima.Map.Felucca;
			}
		}

		internal static int Parse(string name)
		{
			if (string.IsNullOrEmpty(name))
				return 0;

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
				case "samurai":
				case "tokuno":
					return 4;
				case "termur":
					return 5;
				default:
					return 0;
			}
		}

		internal static HuedTile GetTileNear(int mapNum, int x, int y, int z)
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
							return tile;
					}
				}
			}
			catch
			{
			}

			return new HuedTile(0, 0, (sbyte)z);
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
					z = zLeft;
				if (zRight < z)
					z = zRight;
				if (zBottom < z)
					z = zBottom;

				top = zTop;
				if (zLeft > top)
					top = zLeft;
				if (zRight > top)
					top = zRight;
				if (zBottom > top)
					top = zBottom;

				if (Math.Abs(zTop - zBottom) > Math.Abs(zLeft - zRight))
					avg = (int)Math.Floor((zLeft + zRight) / 2.0);
				else
					avg = (int)Math.Floor((zTop + zBottom) / 2.0);
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
	}
}
