using System;
using System.Collections.Generic;
using System.Linq;
using Assistant;
using Ultima;

namespace RazorEnhanced
{
		public class Tile
		{
			public Tile(int x, int y)
			{
				X = x;
				Y = y;
			}

			public int X { get; set; }
			public int Y { get; set; }

			public override bool Equals(Object obj)
			{
				var loc = obj as Tile;
				return X == loc.X && Y == loc.Y;
			}

			public override string ToString()
			{
				return $"{X};{Y}";
			}
		}

		public static class PathMove
		{
			/// <summary>
			/// This function will get the path from actual position to a X,Y coordinate of same map
			/// </summary>
			/// <param name="x">X coordinate of same map</param>
			/// <param name="y">Y coordinate of same map</param>
			/// <param name="scanMaxRange">Max range to scan a path (x, y) should be included in this max range</param>
			/// <returns></returns>
			public static List<Tile> GetPath(int x, int y, int scanMaxRange)
			{
				var playerPosition = Player.Position;
				var squareGrid = new SquareGrid(playerPosition.X, playerPosition.Y, scanMaxRange);

				if (!squareGrid.Tiles.Any(l => l.X == x && l.Y == y))
				{
					//Logging.Log($"Tiles does not contain goal: {playerPosition.X};{playerPosition.Y} to {x};{y}");
					return null;
				}

				var aStarSearch = new AStarSearch(squareGrid, new Tile(playerPosition.X, playerPosition.Y), new Tile(x, y), playerPosition.Z);
				var result = aStarSearch.FindPath();

				if (result == null)
				{
					//Logging.Log($"The result is null, path not found from {playerPosition.X};{playerPosition.Y} to {x};{y}");
					return null;
				}
				else
				{
					if (result.Count == 0)
						result.Add(new Tile(x, y));

					/*Logging.Log($"Path found from {playerPosition.X};{playerPosition.Y} to {x};{y}");
					foreach (var tile in result)
					{
						Logging.Log($"{tile.X},{tile.Y}"); 
					}*/
					return result;
				}

			}

			/// <summary>
			/// This function will get the path from actual position to a X,Y coordinate of same map, getting as scanMaxRange the max difference between positions + 2
			/// </summary>
			/// <param name="x">X coordinate of same map</param>
			/// <param name="y">Y coordinate of same map</param>
			/// <returns></returns>
			public static List<Tile> GetPath(int x, int y)
			{
				var position = Player.Position;

				var distanceX = Math.Abs(position.X - x);
				var distanceY = Math.Abs(position.Y - y);
				var scanMaxRange = Math.Max(distanceX, distanceY) + 2;

				return GetPath(x, y, scanMaxRange);
			}
		}

		internal static class TileExtensions
		{
			public static bool Ignored(this Ultima.Tile tile)
			{
				return (tile.ID == 2 || tile.ID == 0x1DB || (tile.ID >= 0x1AE && tile.ID <= 0x1B5));
			}
		}

		internal class SquareGrid
		{
			public const int BigCost = 1000000;

			private const int PersonHeight = 16;
			private const int StepHeight = 2;
			private const TileFlag ImpassableSurface = TileFlag.Impassable | TileFlag.Surface;

			// DIRS is directions        
			public static readonly Tile[] Dirs =
			{
			new Tile(1, 0), // to right of tile
            new Tile(0, -1), // below tile
            new Tile(-1, 0), // to left of tile
            new Tile(0, 1) // above tile            
        };

			public SquareGrid(int x, int y, int squareSize)
			{
				Tiles = new List<Tile>();

				for (var i = x - squareSize; i < x + squareSize; i++)
				{
					for (var j = y - squareSize; j < y + squareSize; j++)
					{
						Tiles.Add(new Tile(i, j));
					}
				}


			}

			public List<Tile> Tiles;

			// Check if a location is within the bounds of this grid.
			public bool InBounds(Tile id)
			{
				return Tiles.Any(x => x.Equals(id));
			}

			public int Cost(Point3D loc, Map map, Tile b, out int bZ)
			{
				int xForward = b.X, yForward = b.Y;
				var items = World.Items.Values.Where(x => x.OnGround);

				GetStartZ(loc, map, items.Where(x => x.Position.X == loc.X && x.Position.Y == loc.Y), out var startZ, out var startTop);
				var moveIsOk = Check(map, items.Where(x => x.Position.X == xForward && x.Position.Y == yForward), xForward, yForward, startTop, startZ, out var newZ);
				if (moveIsOk)
				{
					bZ = newZ;
					return 1;
				}

				bZ = startZ;
				return BigCost;
			}

			private static bool Check(Map map, IEnumerable<Assistant.Item> items, int x, int y, int startTop, int startZ, out int newZ)
			{
				newZ = 0;

				var tiles = map.Tiles.GetStaticTiles(x, y, true);
				var landTile = map.Tiles.GetLandTile(x, y);
				var landData = TileData.LandTable[landTile.ID & (TileData.LandTable.Length - 1)];
				var landBlocks = (landData.Flags & TileFlag.Impassable) != 0;
				var considerLand = !landTile.Ignored();

				int landZ = 0, landCenter = 0, landTop = 0;

				GetAverageZ(map, x, y, ref landZ, ref landCenter, ref landTop);

				var moveIsOk = false;

				var stepTop = startTop + StepHeight;
				var checkTop = startZ + PersonHeight;

				var ignoreDoors = Player.IsGhost;
				const bool ignoreSpellFields = true;

				int itemZ, itemTop, ourZ, ourTop, testTop;
				ItemData itemData;
				TileFlag flags;

				#region Tiles
				foreach (var tile in tiles)
				{
					itemData = TileData.ItemTable[tile.ID & (TileData.ItemTable.Length - 1)];

					flags = itemData.Flags;

					if ((flags & ImpassableSurface) != TileFlag.Surface)
					{
						continue;
					}

					itemZ = tile.Z;
					itemTop = itemZ;
					ourZ = itemZ + itemData.CalcHeight;
					ourTop = ourZ + PersonHeight;
					testTop = checkTop;

					if (moveIsOk)
					{
						var cmp = Math.Abs(ourZ - Player.Position.Z) - Math.Abs(newZ - Player.Position.Z); // TODO: Check this

						if (cmp > 0 || (cmp == 0 && ourZ > newZ))
						{
							continue;
						}
					}

					if (ourTop > testTop)
					{
						testTop = ourTop;
					}

					if (!itemData.Bridge)
					{
						itemTop += itemData.Height;
					}

					if (stepTop < itemTop)
					{
						continue;
					}

					var landCheck = itemZ;

					if (itemData.Height >= StepHeight)
					{
						landCheck += StepHeight;
					}
					else
					{
						landCheck += itemData.Height;
					}

					if (considerLand && landCheck < landCenter && landCenter > ourZ && testTop > landZ)
					{
						continue;
					}

					if (!IsOk(ignoreDoors, ignoreSpellFields, ourZ, testTop, tiles, items))
					{
						continue;
					}

					newZ = ourZ;
					moveIsOk = true;
				}
				#endregion

				#region Items

				foreach (var item in items)
				{
					itemData = TileData.ItemTable[item.ItemID & (TileData.ItemTable.Length - 1)];
					flags = itemData.Flags;

					if (item.Movable)
					{
						continue;
					}

					if ((flags & ImpassableSurface) != TileFlag.Surface)
					{
						continue;
					}

					itemZ = item.Position.Z;
					itemTop = itemZ;
					ourZ = itemZ + itemData.CalcHeight;
					ourTop = ourZ + PersonHeight;
					testTop = checkTop;

					if (moveIsOk)
					{
						var cmp = Math.Abs(ourZ - Player.Position.Z) - Math.Abs(newZ - Player.Position.Z);

						if (cmp > 0 || (cmp == 0 && ourZ > newZ))
						{
							continue;
						}
					}

					if (ourTop > testTop)
					{
						testTop = ourTop;
					}

					if (!itemData.Bridge)
					{
						itemTop += itemData.Height;
					}

					if (stepTop < itemTop)
					{
						continue;
					}

					var landCheck = itemZ;

					if (itemData.Height >= StepHeight)
					{
						landCheck += StepHeight;
					}
					else
					{
						landCheck += itemData.Height;
					}

					if (considerLand && landCheck < landCenter && landCenter > ourZ && testTop > landZ)
					{
						continue;
					}

					if (!IsOk(ignoreDoors, ignoreSpellFields, ourZ, testTop, tiles, items))
					{
						continue;
					}

					newZ = ourZ;
					moveIsOk = true;
				}

				#endregion

				if (!considerLand || landBlocks || stepTop < landZ)
				{
					return moveIsOk;
				}

				ourZ = landCenter;
				ourTop = ourZ + PersonHeight;
				testTop = checkTop;

				if (ourTop > testTop)
				{
					testTop = ourTop;
				}

				var shouldCheck = true;

				if (moveIsOk)
				{
					var cmp = Math.Abs(ourZ - Player.Position.Z) - Math.Abs(newZ - Player.Position.Z);

					if (cmp > 0 || (cmp == 0 && ourZ > newZ))
					{
						shouldCheck = false;
					}
				}

				if (!shouldCheck || !IsOk(ignoreDoors, ignoreSpellFields, ourZ, testTop, tiles, items))
				{
					return moveIsOk;
				}

				newZ = ourZ;
				return true;
			}

			private static bool IsOk(HuedTile tile, int ourZ, int ourTop)
			{
				var itemData = TileData.ItemTable[tile.ID & (TileData.ItemTable.Length - 1)];

				return tile.Z + itemData.CalcHeight <= ourZ || ourTop <= tile.Z || (itemData.Flags & ImpassableSurface) == 0;
			}

			private static bool IsOk(bool ignoreDoors, bool ignoreSpellFields, int ourZ, int ourTop, HuedTile[] tiles, IEnumerable<Assistant.Item> items)
			{
				return tiles.All(t => IsOk(t, ourZ, ourTop)) && items.All(i => IsOk(i, ourZ, ourTop, ignoreDoors, ignoreSpellFields));
			}

			private static bool IsOk(Assistant.Item item, int ourZ, int ourTop, bool ignoreDoors, bool ignoreSpellFields)
			{
				var itemID = item.ItemID & (TileData.ItemTable.Length - 1);
				var itemData = TileData.ItemTable[itemID];

				if ((itemData.Flags & ImpassableSurface) == 0)
				{
					return true;
				}

				if (((itemData.Flags & TileFlag.Door) != 0 || itemID == 0x692 || itemID == 0x846 || itemID == 0x873 ||
					 (itemID >= 0x6F5 && itemID <= 0x6F6)) && ignoreDoors)
				{
					return true;
				}

				if ((itemID == 0x82 || itemID == 0x3946 || itemID == 0x3956) && ignoreSpellFields)
				{
					return true;
				}

				return item.Position.Z + itemData.CalcHeight <= ourZ || ourTop <= item.Position.Z;
			}

			private static void GetStartZ(Point3D loc, Map map, IEnumerable<Assistant.Item> itemList, out int zLow, out int zTop)
			{
				int xCheck = loc.X, yCheck = loc.Y;
				var landTile = map.Tiles.GetLandTile(xCheck, yCheck);
				var landData = TileData.LandTable[landTile.ID & (TileData.LandTable.Length - 1)];
				var landBlocks = (landData.Flags & TileFlag.Impassable) != 0;


				int landZ = 0, landCenter = 0, landTop = 0;

				GetAverageZ(map, xCheck, yCheck, ref landZ, ref landCenter, ref landTop);

				var considerLand = !landTile.Ignored();

				var zCenter = zLow = zTop = 0;
				var isSet = false;

				if (considerLand && !landBlocks && loc.Z >= landCenter)
				{
					zLow = landZ;
					zCenter = landCenter;
					zTop = landTop;
					isSet = true;
				}

				var staticTiles = map.Tiles.GetStaticTiles(xCheck, yCheck, true);

				foreach (var tile in staticTiles)
				{
					var tileData = TileData.ItemTable[tile.ID & (TileData.ItemTable.Length - 1)];
					var calcTop = (tile.Z + tileData.CalcHeight);

					if (isSet && calcTop < zCenter)
					{
						continue;
					}

					if ((tileData.Flags & TileFlag.Surface) == 0)
					{
						continue;
					}

					if (loc.Z < calcTop)
					{
						continue;
					}

					zLow = tile.Z;
					zCenter = calcTop;

					var top = tile.Z + tileData.Height;

					if (!isSet || top > zTop)
					{
						zTop = top;
					}

					isSet = true;
				}

				foreach (var item in itemList)
				{
					var itemData = TileData.ItemTable[item.ItemID & (TileData.ItemTable.Length - 1)];

					var calcTop = item.Position.Z + itemData.CalcHeight;

					if (isSet && calcTop < zCenter)
					{
						continue;
					}

					if ((itemData.Flags & TileFlag.Surface) == 0)
					{
						continue;
					}

					if (loc.Z < calcTop)
					{
						continue;
					}

					zLow = item.Position.Z;
					zCenter = calcTop;

					var top = item.Position.Z + itemData.Height;

					if (!isSet || top > zTop)
					{
						zTop = top;
					}

					isSet = true;
				}

				if (!isSet)
				{
					zLow = zTop = loc.Z;
				}
				else if (loc.Z > zTop)
				{
					zTop = loc.Z;
				}
			}

			private static void GetAverageZ(Map map, int x, int y, ref int z, ref int avg, ref int top)
			{
				var zTop = map.Tiles.GetLandTile(x, y).Z;
				var zLeft = map.Tiles.GetLandTile(x, y + 1).Z;
				var zRight = map.Tiles.GetLandTile(x + 1, y).Z;
				var zBottom = map.Tiles.GetLandTile(x + 1, y + 1).Z;

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
					avg = FloorAverage(zLeft, zRight);
				else
					avg = FloorAverage(zTop, zBottom);
			}

			private static int FloorAverage(int a, int b)
			{
				var v = a + b;

				if (v < 0)
					--v;

				return (v / 2);
			}

			// Check the tiles that are next to, above, below, or diagonal to
			// this tile, and return them if they're within the game bounds and passable
			public IEnumerable<Tile> Neighbors(Point3D id)
			{
				foreach (var dir in Dirs)
				{
					var next = new Tile(id.X + dir.X, id.Y + dir.Y);
					if (InBounds(next))
					{
						yield return next;
					}
				}
			}
		}

		internal class AStarSearch
		{
			public Dictionary<string, Tile> CameFrom = new Dictionary<string, Tile>();
			public Dictionary<string, int> CostSoFar = new Dictionary<string, int>();

			private readonly Tile _start;
			private readonly Tile _goal;

			public static int Heuristic(Tile a, Tile b)
			{
				return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
			}

			// Conduct the A* search
			public AStarSearch(SquareGrid graph, Tile start, Tile goal, int startZ)
			{
				// start is current sprite Location
				_start = start;
				// goal is sprite destination eg tile user clicked on
				_goal = goal;

				var frontier = new PriorityQueue<Point3D>();
				// Add the starting location to the frontier with a priority of 0
				frontier.Enqueue(new Point3D(start.X, start.Y, startZ), 0);

				CameFrom.Add(start.ToString(), start); // is set to start, None in example
				CostSoFar.Add(start.ToString(), 0);

				Map map = null;
				switch (Player.Map)
				{
					case 0:
						map = Ultima.Map.Felucca;
						break;
					case 1:
						map = Ultima.Map.Trammel;
						break;
					case 2:
						map = Ultima.Map.Ilshenar;
						break;
					case 3:
						map = Ultima.Map.Malas;
						break;
					case 4:
						map = Ultima.Map.Tokuno;
						break;
					case 5:
						map = Ultima.Map.TerMur;
						break;
					default:
						Scripts.SendMessageScriptError("Script Error: GetLandID Invalid Map!");
						break;
				}

				if (map != null)
				{
					while (frontier.Count > 0)
					{
						// Get the Location from the frontier that has the lowest
						// priority, then remove that Location from the frontier
						var current = frontier.Dequeue();

						// If we're at the goal Location, stop looking.
						if (current.X == goal.X && current.Y == goal.Y)
						{
							break;
						}

						// Neighbors will return a List of valid tile Locations
						// that are next to, diagonal to, above or below current
						foreach (var neighbor in graph.Neighbors(current))
						{
							var newCost = CostSoFar[new Tile(current.X, current.Y).ToString()] + graph.Cost(current, map, neighbor, out var neighborZ);
							if (newCost < SquareGrid.BigCost)
							{
								if (!CostSoFar.ContainsKey(neighbor.ToString()) || newCost < CostSoFar[neighbor.ToString()])
								{
									// If we're replacing the previous cost, remove it
									if (CostSoFar.ContainsKey(neighbor.ToString()))
									{
										CostSoFar.Remove(neighbor.ToString());
										CameFrom.Remove(neighbor.ToString());
									}

									CostSoFar.Add(neighbor.ToString(), newCost);
									CameFrom.Add(neighbor.ToString(), new Tile(current.X, current.Y));
									var priority = newCost + Heuristic(neighbor, goal);
									var neighborTile = new Point3D(neighbor.X, neighbor.Y, neighborZ);
									frontier.Enqueue(neighborTile, priority);
								}
							}
						}
					}
				}
			}

			// Return a List of Locations representing the found path
			public List<Tile> FindPath()
			{
				var path = new List<Tile>();
				var current = _goal;
				// path.Add(current);

				while (!current.Equals(_start))
				{
					if (!CameFrom.ContainsKey(current.ToString()))
					{
						return null;
					}
					path.Add(current);
					current = CameFrom[current.ToString()];
				}
				// path.Add(start);
				path.Reverse();
				return path;
			}
		}

		internal class PriorityQueue<T>
		{
			// From Red Blob: I'm using an unsorted array for this example, but ideally this
			// would be a binary heap. Find a binary heap class:
			// * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
			// * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
			// * http://xfleury.github.io/graphsearch.html
			// * http://stackoverflow.com/questions/102398/priority-queue-in-net

			private readonly List<KeyValuePair<T, float>> _elements = new List<KeyValuePair<T, float>>();

			public int Count => _elements.Count;

			public void Enqueue(T item, float priority)
			{
				_elements.Add(new KeyValuePair<T, float>(item, priority));
			}

			// Returns the Location that has the lowest priority
			public T Dequeue()
			{
				var bestIndex = 0;

				for (var i = 0; i < _elements.Count; i++)
				{
					if (_elements[i].Value < _elements[bestIndex].Value)
					{
						bestIndex = i;
					}
				}

				var bestItem = _elements[bestIndex].Key;
				_elements.RemoveAt(bestIndex);
				return bestItem;
			}
		}
}



/*using System.Collections.Generic;
using Ultima;
using Assistant;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

using PathFinderAStar3D;

namespace RazorEnhanced
{
    public class PathFinding
    {
        public enum DiagonalDirection
        {
            NONE        = 0,
            SOUTHEAST   = 1,
            SOUTHWEST   = 2,
            NORTHEAST   = 3,
            NORTHWEST   = 4
        }

        public enum Direction
        {
            NONE        = 0,
            UP          = 1,
            WEST        = 2,
            LEFT        = 3,
            SOUTH       = 4,
            DOWN        = 5,
            EAST        = 6,
            RIGHT       = 7,
            NORTH       = 8
        }

        public class WalkableTiles
        {
            public int x;
            public int y;
            public bool IsWalkable;
            public bool StartPoint;
            public bool EndPoint;
        }

        public class RoadTiles
        {
            public int x;
            public int y;
        }

        private static bool m_loaded = false;
        private static Dictionary<short, TileFlag> m_landdata = new Dictionary<short, TileFlag>();
        private static Dictionary<short, TileFlag> m_tiledata = new Dictionary<short, TileFlag>();
        private static List<Multi.MultiData> multidata = new List<Multi.MultiData>();

        private static void LoadTileLandData()
        {
            m_loaded = true;
            // Inizializzo dati da file
            TileData.Initialize();

            // Parso le info della land in un dizionario 
            foreach (LandData landinfo in TileData.LandTable)
            {
                if (!m_landdata.ContainsKey(landinfo.TextureID))
                    m_landdata.Add(landinfo.TextureID, landinfo.Flags);
            }
        }

        public static Dictionary<int, bool> CheckStatic(int x, int y)
        {
            Dictionary<int, bool> zLands = new Dictionary<int, bool>();
            short landId = (short)Statics.GetLandID(x, y, Player.Map);
            short zValue = (short)Statics.GetLandZ(x, y, Player.Map);

            if (m_landdata.ContainsKey(landId))
            {
                TileFlag templandflag = m_landdata[landId];

                if ((templandflag & TileFlag.Impassable) != 0 || (templandflag & TileFlag.Wall) != 0 ||
                    (templandflag & TileFlag.Wet) != 0)
                {
                    zLands.Add(zValue, true);
                }
                else
                {
                    zLands.Add(zValue, false);
                }
            }

            Dictionary<int, bool> zThings = new Dictionary<int, bool>();
            List<Statics.TileInfo> staticitemsid = Statics.GetStaticsTileInfo(x, y, Player.Map);

            if (staticitemsid.Count > 0)
            {
                foreach (Statics.TileInfo tile in staticitemsid)
                {
                    if (Math.Abs(tile.StaticZ - Player.Position.Z) < 15)
                    {
                        if (TileData.ItemTable[tile.StaticID].Flags != TileFlag.None)
                        {
                            if ((TileData.ItemTable[tile.StaticID].Flags & TileFlag.Impassable) != 0 ||
                                (TileData.ItemTable[tile.StaticID].Flags & TileFlag.Door) != 0 ||
                                (TileData.ItemTable[tile.StaticID].Flags & TileFlag.Wall) != 0 ||
                                (TileData.ItemTable[tile.StaticID].Flags & TileFlag.Damaging) != 0 ||
                                (TileData.ItemTable[tile.StaticID].Flags & TileFlag.Wet) != 0 ||
                                (TileData.ItemTable[tile.StaticID].Flags & TileFlag.Window) != 0)
                            {
                                if (zThings.ContainsKey(tile.StaticZ))
                                    zThings[tile.StaticZ] = true;
                                else
                                    zThings.Add(tile.StaticZ, true);
                            }
                            else if ((TileData.ItemTable[tile.StaticID].Flags & TileFlag.Roof) != 0 ||
                                     (TileData.ItemTable[tile.StaticID].Flags & TileFlag.Surface) != 0)
                            {
                                if (!zThings.ContainsKey(tile.StaticZ))
                                    zThings.Add(tile.StaticZ, false);
                            }
                        }
                    }
                }
            }

            if (zThings.Count == 0)
                foreach (var zLand in zLands)
                    zThings.Add(zLand.Key, zLand.Value);
            return zThings;
        }

        public static Dictionary<int, bool> CheckDynamic(int x, int y)
        {
            Dictionary<int, bool> zThings = new Dictionary<int, bool>();
            List<Assistant.Item> assistantItems = Assistant.World.Items.Values.ToList();
            assistantItems = assistantItems.Where((i) => i.Position.X == x && i.Position.Y == y).ToList();

            if (assistantItems.Count > 0)
            {
                foreach (Assistant.Item i in assistantItems)
                {
                    if (TileData.ItemTable[i.ItemID].Flags != TileFlag.None)
                    {
                        if ((TileData.ItemTable[i.ItemID].Flags & TileFlag.Impassable) != 0 ||
                            (TileData.ItemTable[i.ItemID].Flags & TileFlag.Door) != 0 ||
                            (TileData.ItemTable[i.ItemID].Flags & TileFlag.Wall) != 0 ||
                            (TileData.ItemTable[i.ItemID].Flags & TileFlag.Damaging) != 0 ||
                            (TileData.ItemTable[i.ItemID].Flags & TileFlag.Wet) != 0 ||
                            (TileData.ItemTable[i.ItemID].Flags & TileFlag.Window) != 0)
                        {
                            if (zThings.ContainsKey(i.Position.Z))
                                zThings[i.Position.Z] = true;
                            else
                                zThings.Add(i.Position.Z, true);
                        }
                        else
                        {
                            if (!zThings.ContainsKey(i.Position.Z))
                                zThings.Add(i.Position.Z, false);
                        }
                    }
                    else
                    {
                        if (!zThings.ContainsKey(i.Position.Z))
                            zThings.Add(i.Position.Z, false);
                    }
                }
            }

            return zThings;
        }

        public static bool CheckHouse(int x, int y)
        {
            foreach (Multi.MultiData multi in multidata)
            {
                if (x >= multi.Corner1.X && x <= multi.Corner2.X + 1 && y >= multi.Corner1.Y && y <= multi.Corner2.Y + 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static void TestGrid(int startx, int starty, int startz)
        {
            if (!m_loaded)
                LoadTileLandData();

            multidata = Assistant.World.Multis.Values.ToList();

            //Riempie la griglia

            int xx = startx;
            int yy = starty;

            //Controllo che non sia una casa
            if (CheckHouse(xx, yy))
            {
                Console.WriteLine("C'è una casa");
            }
            else
            {
                Dictionary<int, bool> zResults = CheckStatic(xx, yy);

                if (zResults.Count > 0)
                {
                    foreach (var res in zResults)
                        Console.WriteLine(xx + " - " + yy + " ---- " + res.Key + " : " + res.Value);
                }

                zResults = CheckDynamic(xx, yy);

                if (zResults.Count > 0)
                {
                    foreach (var res in zResults)
                        Console.WriteLine(xx + " - " + yy + " ---- " + res.Key + " : " + res.Value);
                }
            }
        }

        public static void TestRoute(int endx, int endy, int endz)
        {
            if (!m_loaded)
                LoadTileLandData();

            multidata = Assistant.World.Multis.Values.ToList();

            int startx = World.Player.Position.X, starty = World.Player.Position.Y, startz = World.Player.Position.Z;

            //Calcolo l'asse più lungo
            int xAxis = Math.Abs(startx - endx);
            int yAxis = Math.Abs(starty - endy);
            int range = xAxis > yAxis ? xAxis + 20 : yAxis + 20;
            int gridSize = range * 2;
            Dictionary<int, int> coordsX = new Dictionary<int, int>();
            Dictionary<int, int> coordsY = new Dictionary<int, int>();
            Dictionary<int, int> coordsZ = new Dictionary<int, int>();

            //Crea la griglia 3D
            WorldGrid walkgrid = new WorldGrid(gridSize, gridSize, 256);

            Misc.SendMessage("Grid dim: " + gridSize + "x" + gridSize, true);

            int minx = startx > endx ? endx : startx;
            int maxx = startx > endx ? startx : endx;
            int miny = starty > endy ? starty : endy;
            int maxy = starty > endy ? endy : starty;

            Misc.SendMessage("Min x: " + minx + " Max x: " + maxx, true);
            Misc.SendMessage("Min y: " + miny + " Max y: " + maxy, true);

            int mediumXPoint = (startx + endx) / 2;
            int mediumYPoint = (starty + endy) / 2;

            for (int x = 0, xx = mediumXPoint - range; x < walkgrid.Right; x++, xx++)
                coordsX.Add(x, xx);
            for (int y = 0, yy = mediumYPoint - range; y < walkgrid.Top; y++, yy++)
                coordsY.Add(y, yy);
            for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
                coordsZ.Add(z, zz);
                        
            var passable = Color.Yellow;
            var blocked = Color.Red;
            var walked = Color.Gray;
            var start = Color.Green;
            var end = Color.Blue;

            Bitmap bmp = new Bitmap(gridSize, gridSize);            

            //Riempie la griglia
            for (int x = 0, xx = mediumXPoint - range; x < walkgrid.Right; x++, xx++)
            {
                for (int y = 0, yy = mediumYPoint - range; y < walkgrid.Top; y++, yy++)
                {
                    var isWalkable = false;
                    //Controllo che non sia una casa
                    if (CheckHouse(xx, yy))
                    {
                        //Se in queste x-y c'è una casa, setta tutta la Z a False
                        for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
                        {
                            walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
                            if (zz == startz)
                                isWalkable = true;
                        }
                    }
                    else
                    {
                        Dictionary<int, bool> zResults = CheckStatic(xx, yy);

                        if (zResults.Count > 0)
                        {
                            //Se in queste x-y c'è roba statica, setta tutta la Z a False
                            for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
                            {                                    
                                if (zResults.ContainsKey(zz))
                                { 
                                   walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), zResults[zz]);
                                    if (zz == startz)
                                        isWalkable = zResults[zz];
                                }
                                else
                                { 
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
                                    if (zz == startz)
                                        isWalkable = true;
                                }
                            }
                        }
                        else
                        {
                            //Se in queste x-y c'è roba statica, setta tutta la Z come non passabile
                            for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
                            {
                                walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
                                if (zz == startz)
                                    isWalkable = true;
                            }
                        }

                        zResults = CheckDynamic(xx, yy);

                        if (zResults.Count > 0)
                        {
                            //Se in queste x-y c'è roba statica, setta tutta la Z come non passabile
                            for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
                            {
                                if (zResults.ContainsKey(zz))
                                { 
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), zResults[zz]);
                                    if (zz == startz)
                                        isWalkable = zResults[zz];
                                }
                                else
                                { 
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
                                    if (zz == startz)
                                        isWalkable = true;
                                }
                            }
                        }
                    }

                    if(isWalkable)                    
                        bmp.SetPixel(x, y, blocked);                    
                    else
                        bmp.SetPixel(x, y, passable);
                }
            }

            var StartX = coordsX.First(x => x.Value == startx).Key;
            var StartY = coordsY.First(y => y.Value == starty).Key;
            var StartZ = coordsZ.First(z => z.Value == startz).Key;
            PathFinderAStar3D.Point3D startPos = new PathFinderAStar3D.Point3D(StartX, StartY, StartZ);

            var EndX = coordsX.First(x => x.Value == endx).Key;
            var EndY = coordsY.First(y => y.Value == endy).Key;
            var EndZ = coordsZ.First(z => z.Value == endz).Key;
            PathFinderAStar3D.Point3D endPos = new PathFinderAStar3D.Point3D(EndX, EndY, EndZ);

            bmp.SetPixel(startPos.X, startPos.Y, start);
            bmp.SetPixel(endPos.X, endPos.Y, end);            

            bool skippedMovement = false;
            var draw = PathFinder.FindPath(walkgrid, startPos, endPos);

            PathFinderAStar3D.Point3D stepp = draw.next.position;
            while (draw.next != null)
            {
                stepp = draw.next.position;
                if (stepp.EqualsSS(endPos))
                    break;
                bmp.SetPixel(stepp.X, stepp.Y, walked);
                draw = PathFinder.FindPath(walkgrid, stepp, endPos);
            }
            bmp.Save("C:\\Cose\\test.png");

            //Calcolo il percorso
            SearchNode PathList = PathFinder.FindPath(walkgrid, startPos, endPos);
            Console.WriteLine("Posizione Corrente - X: " + PathList.position.X + " - Y:  " + PathList.position.Y);
            PathFinderAStar3D.Point3D oldstep = PathList.position;

            while (PathList.next != null)
            {
                PathFinderAStar3D.Point3D step = PathList.next.position;

                int newStepX = step.X - oldstep.X;
                int newStepY = step.Y - oldstep.Y;
                
                for (int x = oldstep.X - 5, xx = mediumXPoint - 5; x < oldstep.X + 5; x++, xx++)
                {
                    for (int y = oldstep.Y - 5, yy = mediumYPoint - 5; y < oldstep.Y + 5; y++, yy++)
                    {
                        Dictionary<int, bool> zResult = CheckDynamic(xx, yy);
                        if (zResult.Count > 0)
                        {
                            //Se in queste x-y c'è roba statica, setta tutta la Z a False
                            for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
                            {
                                if (zResult.ContainsKey(zz))
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), zResult[zz]);
                                else
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
                            }

                            PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                        }
                    }
                }

                if (oldstep.X != step.X && oldstep.Y != step.Y && IsSurrounded(walkgrid, oldstep, newStepX, newStepY))
                {
                    //Movimento diagonale
                    Assistant.Point3D pp2 = Assistant.World.Player.Position;
                    Assistant.Direction dd2 = Assistant.World.Player.Direction;

                    Assistant.Point2D newStep = new Assistant.Point2D(newStepX, newStepY);

                    //Calcolo il tipo di direzione diagonale da fare
                    switch(GetDiagonalDirection(newStepX, newStepY))
                    {
                        case DiagonalDirection.SOUTHEAST:
                            if (CheckPosition(pp2, oldstep, walkgrid, +1, 0, coordsZ))
                                TurnCorner(pp2, "East", "South");
                            else if (CheckPosition(pp2, oldstep, walkgrid, 0, +1, coordsZ))
                                TurnCorner(pp2, "South", "East");
                            else
                            {
                                walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                                PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                                skippedMovement = true;
                            }
                            break;

                        case DiagonalDirection.SOUTHWEST:
                            if (CheckPosition(pp2, oldstep, walkgrid, -1, 0, coordsZ))
                                TurnCorner(pp2, "West", "South");
                            else if (CheckPosition(pp2, oldstep, walkgrid, 0, -1, coordsZ))
                                TurnCorner(pp2, "South", "West");
                            else
                            {
                                walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                                PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                                skippedMovement = true;
                            }
                            break;

                        case DiagonalDirection.NORTHEAST:
                            if (CheckPosition(pp2, oldstep, walkgrid, +1, 0, coordsZ))
                                TurnCorner(pp2, "East", "North");
                            else if (CheckPosition(pp2, oldstep, walkgrid, 0, -1, coordsZ))
                                TurnCorner(pp2, "North", "East");
                            else
                            {
                                walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                                PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                                skippedMovement = true;
                            }
                            break;

                        case DiagonalDirection.NORTHWEST:
                            if (CheckPosition(pp2, oldstep, walkgrid, -1, 0, coordsZ))
                                TurnCorner(pp2, "West", "North");
                            else if (CheckPosition(pp2, oldstep, walkgrid, 0, -1, coordsZ))
                                TurnCorner(pp2, "North", "West");
                            else
                            {
                                walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                                PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                                skippedMovement = true;
                            }
                            break;

                        case DiagonalDirection.NONE:
                        default:
                            MoveStraightForward(step.X - oldstep.X, step.Y - oldstep.Y);
                            break;
                    }
                }
                else
                    MoveStraightForward(step.X - oldstep.X, step.Y - oldstep.Y);


                if(PathList == null)
                {
                    TestRoute(endx, endy, endz);
                    break;
                }

                if (!skippedMovement)
                {
                    oldstep = step;
                    PathList = PathList.next;
                }
                else
                    skippedMovement = false;

                //Rruvai!
                if(oldstep.EqualsSS(endPos))
                    break;

                //Controllo se è cambiato il multidata
                if (multidata != Assistant.World.Multis.Values.ToList())
                {
                    multidata = Assistant.World.Multis.Values.ToList();

                    for (int x = 0, xx = mediumXPoint - range; x < walkgrid.Right; x++, xx++)
                    {
                        for (int y = 0, yy = mediumYPoint - range; y < walkgrid.Top; y++, yy++)
                        {
                            //Controllo che non sia una casa
                            if (CheckHouse(xx, yy))
                            {
                                //Se in queste x-y c'è una casa, setta tutta la Z a False
                                for (int z = 0; z < walkgrid.Back; z++)
                                {
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
                                }
                            }
                        }
                    }

                    PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                }
            }

            Console.WriteLine("Posizione Finale - X: " + PathList.position.X + " - Y:  " + PathList.position.Y);
        }

        private static bool IsSurrounded(WorldGrid grid, PathFinderAStar3D.Point3D coords, int xEdit, int yEdit)
        {
            if (!grid.PositionIsFree(new PathFinderAStar3D.Point3D(coords.X + xEdit, coords.Y, coords.Z)))
                return true;
            else if (!grid.PositionIsFree(new PathFinderAStar3D.Point3D(coords.X, coords.Y + yEdit, coords.Z)))
                return true;
            else
                return false;
        }

        private static bool CheckPosition(Assistant.Point3D playerPos, PathFinderAStar3D.Point3D oldStep, WorldGrid walkGrid, int x, int y, Dictionary<int, int> coordsZ)
        {
            int coordZ = coordsZ.First(z => z.Value == (short)Statics.GetLandZ(playerPos.X + x, playerPos.Y + y, Player.Map)).Key;

            PathFinderAStar3D.Point3D position = new PathFinderAStar3D.Point3D(oldStep.X + x, oldStep.Y + y, coordZ);

            if(walkGrid.PositionIsFree(position))
                return true;
            else
                return false;
        }

        private static Direction GetDirection(int x, int y)
        {
            if (x <= -1 && y <= -1)
                return Direction.UP;
            else if (x <= -1 && y == 0)
                return Direction.WEST;
            else if (x <= -1 && y >= 1)
                return Direction.LEFT;
            else if (x == 0 && y >= 1)
                return Direction.SOUTH;
            else if (x >= 1 && y >= 1)
                return Direction.DOWN;
            else if (x >= 1 && y == 0)
                return Direction.EAST;
            else if (x >= 1 && y <= -1)
                return Direction.RIGHT;
            else if (x == 0 && y <= -1)
                return Direction.NORTH;
            else
                return Direction.NONE;
        }

        private static DiagonalDirection GetDiagonalDirection(int x, int y)
        {
            if(x == 1 && y == 1)
                return DiagonalDirection.SOUTHEAST;
            else if (x == -1 && y == 1)
                return DiagonalDirection.SOUTHWEST;
            else if (x == 1 && y == -1)
                return DiagonalDirection.NORTHEAST;
            else if (x == -1 && y == -1)
                return DiagonalDirection.NORTHWEST;
            else
                return DiagonalDirection.NONE;
        }

        private static void MoveStraightForward(int x, int y)
        {
            Assistant.Point3D oldplayerpos = Assistant.World.Player.Position;
            while (Assistant.World.Player.Position.X != oldplayerpos.X + x ||
                   Assistant.World.Player.Position.Y != oldplayerpos.Y + y)
            {
                Assistant.Point3D pp2 = Assistant.World.Player.Position;
                Assistant.Direction dd2 = Assistant.World.Player.Direction;

                switch(GetDirection(x, y))
                {
                    case Direction.UP:
                        RazorEnhanced.Player.Run("Up");
                        break;
                    case Direction.WEST:
                        RazorEnhanced.Player.Run("West");
                        break;
                    case Direction.LEFT:
                        RazorEnhanced.Player.Run("Left");
                        break;
                    case Direction.SOUTH:
                        RazorEnhanced.Player.Run("South");
                        break;
                    case Direction.DOWN:
                        RazorEnhanced.Player.Run("Down");
                        break;
                    case Direction.EAST:
                        RazorEnhanced.Player.Run("East");
                        break;
                    case Direction.RIGHT:
                        RazorEnhanced.Player.Run("Right");
                        break;
                    case Direction.NORTH:
                        RazorEnhanced.Player.Run("North");
                        break;

                    case Direction.NONE:
                    default:
                        break;
                }

                while (pp2 == Assistant.World.Player.Position && dd2 == Assistant.World.Player.Direction)
                    Misc.Pause(10);
            }
        }

        private static void RotateDirection(string direction)
        {
            //Switcha la direzione
            if (RazorEnhanced.Player.Direction != direction)
            {
                RazorEnhanced.Player.Walk(direction);
                while (RazorEnhanced.Player.Direction != direction)
                    Misc.Pause(10);
            }
        }

        private static Assistant.Point3D MoveToDirection(Assistant.Point3D playerPos, string direction)
        {
            //Si muove
            RazorEnhanced.Player.Run(direction);
            while (playerPos == Assistant.World.Player.Position)
                Misc.Pause(10);
            return Assistant.World.Player.Position;
        }

        private static void TurnCorner(Assistant.Point3D playerPos, string direction1, string direction2)
        {
            //Ruota la direzione
            RotateDirection(direction1);
            //Si muove in questa direzione
            playerPos = MoveToDirection(playerPos, direction1);

            //Ruota la direzione
            RotateDirection(direction2);
            //Si muove in questa direzione
            playerPos = MoveToDirection(playerPos, direction2);
        }
    }
}*/
