using System.Collections.Generic;
using Ultima;
using EpPathFinding.cs;
using Assistant;
using System;
using System.IO;
using System.Text;


namespace RazorEnhanced
{
	public class PathFinding
	{
		private static bool m_loaded = false;
		private static Dictionary<short, TileFlag> m_landdata =	new Dictionary<short, TileFlag>();
		private static Dictionary<short, TileFlag> m_tiledata = new Dictionary<short, TileFlag>();

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

		public static bool Passable(int x, int y)
		{
			if (!m_loaded)
				LoadTileLandData();

			TileFlag templandflag = m_landdata[(short)Statics.GetLandID(x, y, Player.Map)];

			if ((templandflag & TileFlag.Impassable) != 0 || (templandflag & TileFlag.Wall) != 0 || (templandflag & TileFlag.Wet) != 0)
			{
				return false;
			}
			else
			{
				List<int> staticitemsid = Statics.GetStaticsTileInfoPathfind(x, y, Player.Map);
				if (staticitemsid.Count > 0)
				{
					foreach (int itemid in staticitemsid)
					{
						if ((TileData.LandTable[itemid].Flags & TileFlag.Impassable) == 0 || (TileData.LandTable[itemid].Flags & TileFlag.Door) == 0 || (TileData.LandTable[itemid].Flags & TileFlag.Wall) == 0 || (TileData.LandTable[itemid].Flags & TileFlag.Damaging) == 0 || (TileData.LandTable[itemid].Flags & TileFlag.Wet) == 0 || (TileData.LandTable[itemid].Flags & TileFlag.Window) == 0)
						{
							return false;
						}
					}
					return true;
				}
				else
					return true;
			}
		}

		public static void TestRoute(int startx, int starty, int endx, int endy)
		{
            //Calcolo l'asse più lungo
		    int xAxis = Math.Abs(startx - endx);
		    int yAxis = Math.Abs(starty - endy);
		    int range = xAxis > yAxis ? xAxis + 20 : yAxis + 20;
            int gridSize = range * 2;

            BaseGrid walkgrid = new StaticGrid(gridSize, gridSize);

			Misc.SendMessage("Grid dim: " + gridSize + "x" + gridSize);

            int minx = startx > endx ? endx : startx;
            int maxx = startx > endx ? startx : endx;
            int miny = starty > endy ? starty : endy;
            int maxy = starty > endy ? endy : starty;

			Misc.SendMessage("Min x: " + minx + " Max x: " + maxx);
			Misc.SendMessage("Min y: " + miny + " Max y: " + maxy);

		    int mediumXPoint = (startx + endx)/2;
		    int mediumYPoint = (starty + endy)/2;

		    int countNonPass = 0;
		    int countPass = 0;
			for (int x = 0, xx = mediumXPoint - range; x < gridSize; x++, xx++)
			{
				for (int y = 0, yy = mediumYPoint - range; y < gridSize; y++, yy++)
				{
				    bool isPassable = Passable(xx, yy);
				    if (!isPassable)
				        countNonPass++;
				    else
				        countPass++;
					walkgrid.SetWalkableAt(x, y, isPassable);
				}
			}

            Misc.SendMessage("Amount non passabili: " + countNonPass);
            Misc.SendMessage("Amount passabili: " + countPass);

            GridPos startPos = new GridPos(range, range);
			GridPos endPos = new GridPos(startx - endx + range, starty - endy + range);
			JumpPointParam jpParam = new JumpPointParam(walkgrid, startPos, endPos, false, false, false);
			List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam);

			GridPos oldstep = new GridPos(-1, -1);
			foreach (GridPos step in resultPathList)
			{
				if (oldstep.x == -1 && oldstep.y == -1)
				{
					oldstep = step;
				}
				else
				{
                    int xx = oldstep.x - step.x;
					int yy = oldstep.y - step.y;
					Assistant.Point3D oldplayerpos = World.Player.Position;

					while (World.Player.Position.X != oldplayerpos.X + xx || World.Player.Position.Y != oldplayerpos.Y + yy)
					{
						Assistant.Point3D pp2 = World.Player.Position;
						Assistant.Direction dd2 = World.Player.Direction;

						if (xx <= -1 && yy <= -1)
							RazorEnhanced.Player.Run("Up");
						else if (xx <= -1 && yy == 0)
							RazorEnhanced.Player.Run("West");
						else if (xx <= -1 && yy >= 1)
							RazorEnhanced.Player.Run("Left");
						else if (xx == 0 && yy >= 1)
							RazorEnhanced.Player.Run("South");
						else if (xx >= 1 && yy >= 1)
							RazorEnhanced.Player.Run("Down");
						else if (xx >= 1 && yy == 0)
							RazorEnhanced.Player.Run("East");
						else if (xx >= 1 && yy <= -1)
							RazorEnhanced.Player.Run("Right");
						else if (xx == 0 && yy <= -1)
							RazorEnhanced.Player.Run("North");
                        
						while (pp2 == World.Player.Position && dd2 == World.Player.Direction)
						{
							Misc.Pause(10);
						}
					}
				    oldstep = step;
				}

				RazorEnhanced.Misc.SendMessage("step -> X: " + step.x + " Y: " + step.y);
			}
		}
	}
}