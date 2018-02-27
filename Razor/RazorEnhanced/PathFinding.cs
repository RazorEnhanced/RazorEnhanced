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
                            walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), false);
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
                                    walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), false);
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
         /*   var draw = PathFinder.FindPath(walkgrid, startPos, endPos);

            PathFinderAStar3D.Point3D stepp = draw.next.position;
            while (draw.next != null)
            {
                stepp = draw.next.position;
                if (stepp.EqualsSS(endPos))
                    break;
                bmp.SetPixel(stepp.X, stepp.Y, walked);
                draw = PathFinder.FindPath(walkgrid, stepp, endPos);
            }*/

            //bmp.Save("C:\\Cose\\test.png");

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

                    //SE
                    if (newStepX == 1 && newStepY == 1)
                    {
                        if (walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X + 1, oldstep.Y,
                            coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X + 1, pp2.Y, Player.Map)).Key)))
                        {

                            Console.WriteLine("East e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "East")
                            {
                                RazorEnhanced.Player.Walk("East");
                                while (RazorEnhanced.Player.Direction != "East")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("East");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "South")
                            {
                                RazorEnhanced.Player.Walk("South");
                                while (RazorEnhanced.Player.Direction != "South")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("South");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                        }
                        else if (walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X, oldstep.Y + 1,
                            coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X, pp2.Y + 1, Player.Map)).Key)))
                        {
                            Console.WriteLine("South e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "South")
                            {
                                RazorEnhanced.Player.Walk("South");
                                while (RazorEnhanced.Player.Direction != "South")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("South");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "East")
                            {
                                RazorEnhanced.Player.Walk("East");
                                while (RazorEnhanced.Player.Direction != "East")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("East");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else
                        {
                            walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                            PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                            skippedMovement = true;
                        }
                    }
                    //NW
                    else if (newStepX == -1 && newStepY == -1)
                    {
                        if (
                            walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X - 1, oldstep.Y,
                                coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X - 1, pp2.Y, Player.Map)).Key)))
                        {
                            Console.WriteLine("West e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "West")
                            {
                                RazorEnhanced.Player.Walk("West");
                                while (RazorEnhanced.Player.Direction != "West")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("West");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "North")
                            {
                                RazorEnhanced.Player.Walk("North");
                                while (RazorEnhanced.Player.Direction != "North")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("North");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else if (
                            walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X, oldstep.Y - 1,
                                coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X, pp2.Y - 1, Player.Map)).Key)))
                        {
                            Console.WriteLine("North e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "North")
                            {
                                RazorEnhanced.Player.Walk("North");
                                while (RazorEnhanced.Player.Direction != "North")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("North");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "West")
                            {
                                RazorEnhanced.Player.Walk("West");
                                while (RazorEnhanced.Player.Direction != "West")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("West");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else
                        {
                            walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                            PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                            skippedMovement = true;
                        }
                    }
                    //NE
                    else if (newStepX == 1 && newStepY == -1)
                    {
                        if (
                            walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X + 1, oldstep.Y,
                                coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X + 1, pp2.Y, Player.Map)).Key)))
                        {
                            Console.WriteLine("East e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "East")
                            {
                                RazorEnhanced.Player.Walk("East");
                                while (RazorEnhanced.Player.Direction != "East")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("East");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "North")
                            {
                                RazorEnhanced.Player.Walk("North");
                                while (RazorEnhanced.Player.Direction != "North")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("North");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else if (
                            walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X, oldstep.Y - 1,
                                coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X, pp2.Y - 1, Player.Map)).Key)))
                        {
                            Console.WriteLine("North e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "North")
                            {
                                RazorEnhanced.Player.Walk("North");
                                while (RazorEnhanced.Player.Direction != "North")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("North");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "East")
                            {
                                RazorEnhanced.Player.Walk("East");
                                while (RazorEnhanced.Player.Direction != "East")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("East");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else
                        {
                            walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                            PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                            skippedMovement = true;
                        }
                    }
                    //SW
                    else if (newStepX == -1 && newStepY == 1)
                    {
                        if (
                            walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X - 1, oldstep.Y,
                                coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X - 1, pp2.Y, Player.Map)).Key)))
                        {
                            Console.WriteLine("West e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "West")
                            {
                                RazorEnhanced.Player.Walk("West");
                                while (RazorEnhanced.Player.Direction != "West")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("West");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "South")
                            {
                                RazorEnhanced.Player.Walk("South");
                                while (RazorEnhanced.Player.Direction != "South")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("South");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else if (
                            walkgrid.PositionIsFree(new PathFinderAStar3D.Point3D(oldstep.X, oldstep.Y + 1,
                                coordsZ.First(z => z.Value == (short)Statics.GetLandZ(pp2.X, pp2.Y + 1, Player.Map)).Key)))
                        {
                            Console.WriteLine("South e' libera!");

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "South")
                            {
                                RazorEnhanced.Player.Walk("South");
                                while (RazorEnhanced.Player.Direction != "South")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("South");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);

                            //Reset della posizione corrente
                            pp2 = Assistant.World.Player.Position;

                            //Switcha la direzione
                            if (RazorEnhanced.Player.Direction != "West")
                            {
                                RazorEnhanced.Player.Walk("West");
                                while (RazorEnhanced.Player.Direction != "West")
                                    Misc.Pause(10);
                            }

                            //Si muove
                            RazorEnhanced.Player.Run("West");
                            while (pp2 == Assistant.World.Player.Position)
                                Misc.Pause(10);
                        }
                        else
                        {
                            walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(step.X, step.Y, step.Z), true);
                            PathList = PathFinder.FindPath(walkgrid, oldstep, endPos);
                            skippedMovement = true;
                        }
                    }
                    else
                    {

                        int xx1 = step.X - oldstep.X;
                        int yy1 = step.Y - oldstep.Y;
                        Assistant.Point3D oldplayerpos = Assistant.World.Player.Position;

                        while (Assistant.World.Player.Position.X != oldplayerpos.X + xx1 ||
                               Assistant.World.Player.Position.Y != oldplayerpos.Y + yy1)
                        {
                            pp2 = Assistant.World.Player.Position;
                            dd2 = Assistant.World.Player.Direction;

                            if (xx1 <= -1 && yy1 <= -1)
                                RazorEnhanced.Player.Run("Up");
                            else if (xx1 <= -1 && yy1 == 0)
                                RazorEnhanced.Player.Run("West");
                            else if (xx1 <= -1 && yy1 >= 1)
                                RazorEnhanced.Player.Run("Left");
                            else if (xx1 == 0 && yy1 >= 1)
                                RazorEnhanced.Player.Run("South");
                            else if (xx1 >= 1 && yy1 >= 1)
                                RazorEnhanced.Player.Run("Down");
                            else if (xx1 >= 1 && yy1 == 0)
                                RazorEnhanced.Player.Run("East");
                            else if (xx1 >= 1 && yy1 <= -1)
                                RazorEnhanced.Player.Run("Right");
                            else if (xx1 == 0 && yy1 <= -1)
                                RazorEnhanced.Player.Run("North");

                            while (pp2 == Assistant.World.Player.Position && dd2 == Assistant.World.Player.Direction)
                            {
                                Misc.Pause(10);
                            }
                        }
                    }
                }
                else
                {
                    int xx1 = step.X - oldstep.X;
                    int yy1 = step.Y - oldstep.Y;
                    Assistant.Point3D oldplayerpos = Assistant.World.Player.Position;

                    Console.WriteLine("Posizione Corrente Step - X: " + oldplayerpos.X + " - Y:  " + oldplayerpos.Y);
                    Console.WriteLine("Prossimo Step - X: " + (int)(oldplayerpos.X + xx1) + " - Y:  " + (int)(oldplayerpos.Y + yy1));

                    while (Assistant.World.Player.Position.X != oldplayerpos.X + xx1 ||
                           Assistant.World.Player.Position.Y != oldplayerpos.Y + yy1)
                    {
                        Assistant.Point3D pp2 = Assistant.World.Player.Position;
                        Assistant.Direction dd2 = Assistant.World.Player.Direction;

                        if (xx1 <= -1 && yy1 <= -1)
                            RazorEnhanced.Player.Run("Up");
                        else if (xx1 <= -1 && yy1 == 0)
                            RazorEnhanced.Player.Run("West");
                        else if (xx1 <= -1 && yy1 >= 1)
                            RazorEnhanced.Player.Run("Left");
                        else if (xx1 == 0 && yy1 >= 1)
                            RazorEnhanced.Player.Run("South");
                        else if (xx1 >= 1 && yy1 >= 1)
                            RazorEnhanced.Player.Run("Down");
                        else if (xx1 >= 1 && yy1 == 0)
                            RazorEnhanced.Player.Run("East");
                        else if (xx1 >= 1 && yy1 <= -1)
                            RazorEnhanced.Player.Run("Right");
                        else if (xx1 == 0 && yy1 <= -1)
                            RazorEnhanced.Player.Run("North");

                        while (pp2 == Assistant.World.Player.Position && dd2 == Assistant.World.Player.Direction)
                        {
                            Misc.Pause(10);
                        }
                    }
                }

                Console.WriteLine("Prossimo Step - X: " + step.X + " - Y:  " + step.Y);

                if(PathList == null)
                {
                    Misc.SendMessage("Riavvio dio");
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
    }
}