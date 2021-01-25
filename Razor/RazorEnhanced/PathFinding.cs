using System;
using System.Collections.Generic;
using System.Linq;
using Assistant;
using Ultima;

namespace RazorEnhanced
{

    // Pathfind Core
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

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
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
        public static List<Tile> GetPath(int x, int y, int scanMaxRange, bool ignoremob)
        {
            var playerPosition = Player.Position;
            var squareGrid = new SquareGrid(playerPosition.X, playerPosition.Y, scanMaxRange);

            if (!squareGrid.Tiles.Any(l => l.X == x && l.Y == y))
            {
                //Logging.Log($"Tiles does not contain goal: {playerPosition.X};{playerPosition.Y} to {x};{y}");
                return null;
            }

            var aStarSearch = new AStarSearch(squareGrid, new Tile(playerPosition.X, playerPosition.Y), new Tile(x, y), playerPosition.Z, ignoremob);
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
        public static List<Tile> GetPath(int x, int y, bool ignoremob)
        {
            var position = Player.Position;

            var distanceX = Math.Abs(position.X - x);
            var distanceY = Math.Abs(position.Y - y);
            var scanMaxRange = Math.Max(distanceX, distanceY) + 2;

            return GetPath(x, y, scanMaxRange, ignoremob);
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
            new Tile(1, 0),
            new Tile(-1, 0),
            new Tile(0, 1),
            new Tile(0, -1),
            new Tile(-1, -1),
            new Tile(1, 1),
            new Tile(-1, 1),
            new Tile(1, -1)
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

        public int Cost(Point3D loc, Map map, Tile b, bool ignoremob, out int bZ)
        {
            int xForward = b.X, yForward = b.Y;
            var items = World.Items.Values.Where(x => x.OnGround);
            var newZ = 0;
            GetStartZ(loc, map, items.Where(x => x.Position.X == loc.X && x.Position.Y == loc.Y), out var startZ, out var startTop);
            var moveIsOk = Check(map, items.Where(x => x.Position.X == xForward && x.Position.Y == yForward), xForward, yForward, startTop, startZ, ignoremob, out newZ);

            if (b.X > loc.X && b.Y > loc.Y) //Down
            {
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X == xForward && x.Position.Y == yForward - 1), xForward, yForward - 1, startTop, startZ, ignoremob, out newZ);
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X  == xForward - 1 && x.Position.Y == yForward), xForward - 1, yForward, startTop, startZ, ignoremob, out newZ);
            }
            else if (b.X < loc.X && b.Y < loc.Y) //UP
            {
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X == xForward && x.Position.Y  == yForward + 1), xForward, yForward + 1, startTop, startZ, ignoremob, out newZ);
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X  == xForward + 1 && x.Position.Y == yForward), xForward +1, yForward, startTop, startZ, ignoremob, out newZ);
            }
            else if (b.X > loc.X && b.Y < loc.Y) //Right
            {
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X == xForward && x.Position.Y == yForward + 1), xForward, yForward + 1, startTop, startZ, ignoremob, out newZ);
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X == xForward - 1 && x.Position.Y == yForward), xForward- 1, yForward, startTop, startZ, ignoremob, out newZ);
            }
            else if (b.X < loc.X && b.Y > loc.Y) //Left
            {
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X == xForward && x.Position.Y == yForward - 1), xForward, yForward - 1, startTop, startZ, ignoremob, out newZ);
                if (moveIsOk)
                    moveIsOk = Check(map, items.Where(x => x.Position.X == xForward + 1 && x.Position.Y == yForward), xForward + 1, yForward, startTop, startZ, ignoremob, out newZ);
            }

            if (moveIsOk)
            {
                bZ = newZ;
                return 1;
            }

            bZ = startZ;
            return BigCost;
        }

        private static bool Check(Map map, IEnumerable<Assistant.Item> items, int x, int y, int startTop, int startZ, bool ignoremob, out int newZ)
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

            var ignoreDoors = false;

            if (Player.IsGhost || Engine.MainWindow.AutoOpenDoors.Checked)
                ignoreDoors = true;

            const bool ignoreSpellFields = true;

            int itemZ, itemTop, ourZ, ourTop, testTop;
            ItemData itemData;
            TileFlag flags;

            // Check For mobiles
            if (!ignoremob)
            {
                var mobs = World.Mobiles.Values;
                List<Assistant.Mobile> result = new List<Assistant.Mobile>();
                foreach (Assistant.Mobile m in mobs)
                {
                    if (m.Position.X == x && m.Position.Y == y && m.Serial != Player.Serial)
                        result.Add(m);
                }
                if (result.Count > 0) // mob present at this spot.
                {
                    if (World.Player.Stam < World.Player.StamMax) // no max stam, avoid this location
                        return false;
                }
            }
            // Check for deed player house
            if (Statics.CheckDeedHouse(x, y))
            {
                return false;
            }

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
        public AStarSearch(SquareGrid graph, Tile start, Tile goal, int startZ, bool ignoremob)
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
                        var newCost = CostSoFar[new Tile(current.X, current.Y).ToString()] + graph.Cost(current, map, neighbor, ignoremob, out var neighborZ);
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

    public class PathFinding
    {
        public class Route
        {
            public int X = 0;
            public int Y = 0;
            public bool DebugMessage = false;
            public bool StopIfStuck = false;
            public bool IgnoreMobile = false;
            public bool UseResync = false;
            public int MaxRetry = -1;
            public float Timeout = -1;

            public Route()
            {
            }
        }


        // Dalamar: Exposed "RazorEnhanced.Tile" to Python via Pathfind in order to be able to create List[Tile] to feed directly to
        public static Tile Tile(int x, int y){
            return new Tile(x,y);
        }

        public static bool Go(Route r)
        {
            if ( r.StopIfStuck ) { r.MaxRetry = 1; }

            DateTime timeStart, timeEnd;
            timeStart = DateTime.Now;
            timeEnd = (r.Timeout < 0) ? timeStart.AddDays(1) : timeStart.AddSeconds(r.Timeout);

            float timeLeft;
            List<Tile> road;
            bool success;
            while ( r.MaxRetry == -1 || r.MaxRetry > 0 ) {
                road = PathMove.GetPath(r.X, r.Y, r.IgnoreMobile);
                timeLeft = (int) timeEnd.Subtract(DateTime.Now).TotalSeconds;
                success = RunPath(road, timeLeft, r.DebugMessage, r.UseResync);
                if (r.MaxRetry > 0) { r.MaxRetry -= 1; }
                if (success) { return true; }
                if (DateTime.Now.CompareTo(timeEnd) > 0) { return false; }
            }
            return false;
        }

        public static List<Tile> GetPath(int x, int y, bool ignoremob) {
            return PathMove.GetPath(x, y, ignoremob);
        }

        public static bool RunPath(List<Tile> path, float timeout=-1, bool debugMessage=false, bool useResync = true)
        {
            if (path == null) { return false; }
            DateTime timeStart, timeEnd;
            timeStart = DateTime.Now;
            timeEnd = (timeout < 0) ? timeStart.AddDays(1) : timeStart.AddSeconds(timeout);

            Tile dst = path.Last();
            foreach (Tile step in path)
            {
                if (Player.Position.X == dst.X && Player.Position.Y == dst.Y)
                {
                    Misc.SendMessage("PathFind: Destination reached", 66);
                    return true;
                }
                bool walkok = false;
                if (step.X > Player.Position.X && step.Y == Player.Position.Y) //East
                {
                    Rotate(Direction.East, debugMessage);
                    walkok = Run(Direction.East, debugMessage);
                }
                else if (step.X < Player.Position.X && step.Y == Player.Position.Y) // West
                {
                    Rotate(Direction.West, debugMessage);
                    walkok = Run(Direction.West, debugMessage);
                }
                else if (step.X == Player.Position.X && step.Y < Player.Position.Y) //North
                {
                    Rotate(Direction.North, debugMessage);
                    walkok = Run(Direction.North, debugMessage);
                }
                else if (step.X == Player.Position.X && step.Y > Player.Position.Y) //South
                {
                    Rotate(Direction.South, debugMessage);
                    walkok = Run(Direction.South, debugMessage);
                }
                else if (step.X > Player.Position.X && step.Y > Player.Position.Y) //Down
                {
                    Rotate(Direction.Down, debugMessage);
                    walkok = Run(Direction.Down, debugMessage);
                }
                else if (step.X < Player.Position.X && step.Y < Player.Position.Y) //UP
                {
                    Rotate(Direction.Up, debugMessage);
                    walkok = Run(Direction.Up, debugMessage);
                }
                else if (step.X > Player.Position.X && step.Y < Player.Position.Y) //Right
                {
                    Rotate(Direction.Right, debugMessage);
                    walkok = Run(Direction.Right, debugMessage);
                }
                else if (step.X < Player.Position.X && step.Y > Player.Position.Y) //Left
                {
                    Rotate(Direction.Left, debugMessage);
                    walkok = Run(Direction.Left, debugMessage);
                }
                else if (Player.Position.X == step.X && Player.Position.Y == step.Y) // no action
                    walkok = true;

                if (timeout >= 0 && DateTime.Now.CompareTo(timeEnd) > 0) {
                    if (debugMessage)
                        Misc.SendMessage("PathFind: RunPath run TIMEOUT", 33);
                    return false;
                }

                if (!walkok)
                {
                    if (debugMessage)
                        Misc.SendMessage("PathFind: Move action FAIL", 33);

                    if (useResync)
                    {
                        Misc.Resync();
                        Misc.Pause(200);
                    }

                    return false;
                }
                else
                {
                    if (debugMessage)
                        Misc.SendMessage("PathFind: Move action OK", 66);
                }
            }

            if (Player.Position.X == dst.X && Player.Position.Y == dst.Y)
            {
                Misc.SendMessage("PathFind: Destination reached", 66);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Rotate(Direction d, bool debug)
        {
            if ((World.Player.Direction & Direction.Mask) != d)
            {
                Player.Run(d.ToString());

                if (debug)
                    Misc.SendMessage("PathFind: Rotate in direction: " + d.ToString(), 55);
            }
        }

        private static bool Run(Direction d, bool debug)
        {
            if (debug)
                Misc.SendMessage("PathFind: Move to direction: " + d.ToString(), 55);

            return Player.Run(d.ToString());
        }
    }
}


