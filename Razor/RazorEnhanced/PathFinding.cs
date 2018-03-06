using System.Collections.Generic;
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

            Misc.SendMessage("Grid dim: " + gridSize + "x" + gridSize);

            int minx = startx > endx ? endx : startx;
            int maxx = startx > endx ? startx : endx;
            int miny = starty > endy ? starty : endy;
            int maxy = starty > endy ? endy : starty;

            Misc.SendMessage("Min x: " + minx + " Max x: " + maxx);
            Misc.SendMessage("Min y: " + miny + " Max y: " + maxy);

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
}