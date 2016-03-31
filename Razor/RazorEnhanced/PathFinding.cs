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
			int maxx = 0;
			int minx = 0;
			int maxy = 0;
			int miny = 0;

			//BaseGrid walkgrid = new StaticGrid(Math.Abs(startx - endx), Math.Abs(starty - endy));
			BaseGrid walkgrid = new StaticGrid(200, 200);

			Misc.SendMessage("Grid dim: " + Math.Abs(startx - endx) + " " + Math.Abs(starty - endy));
			if (startx > endx)
			{
				maxx = startx;
				minx = endx;
			}
			else
			{
				maxx = endx;
				minx = startx;
			}

			if (starty > endy)
			{
				maxy = starty;
				miny = endy;
			}
			else
			{
				maxy = endy;
				miny = starty;
			}

			Misc.SendMessage("Max x: " + maxx + " Min x: " + minx);
			Misc.SendMessage("Max y: " + maxy + " Min y: " + miny);

			for (int x = 0; x < 200; x++)
			{
				for (int y = 0; y < 200; y++)
				{
					walkgrid.SetWalkableAt(x, y,true);
				}
			}

			GridPos startPos = new GridPos(100, 100);
			GridPos endPos = new GridPos(startx - endx + 100, starty - endy + 100);
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

					int xx = step.x - oldstep.x;
					int yy = step.y - oldstep.y;
					Assistant.Point3D oldplayerpos = World.Player.Position;

					while (World.Player.Position.X != oldplayerpos.X + xx || World.Player.Position.Y != oldplayerpos.Y + yy)
					{
						Assistant.Point3D pp2 = World.Player.Position;
						Assistant.Direction dd2 = World.Player.Direction;

						if (xx >= 1 && yy >= 1)
						{
							Misc.SendMessage("Up");
							RazorEnhanced.Player.Walk("Up");
						}
						else if (xx <= -1 && yy == 0)
						{
							Misc.SendMessage("West");
							RazorEnhanced.Player.Walk("West");
						}
						else if (xx >= 1 && yy <= -1)
						{
							Misc.SendMessage("Left");
							RazorEnhanced.Player.Walk("Left");
						}
						else if (xx == 0 && yy <= -1)
						{
							Misc.SendMessage("South");
							RazorEnhanced.Player.Walk("South");
						}
						else if (xx <= -1 && yy <= -1)
						{
							Misc.SendMessage("Down");
							RazorEnhanced.Player.Walk("Down");
						}
						else if (xx >= 1 && yy == 0)
						{
							Misc.SendMessage("East");
							RazorEnhanced.Player.Walk("East");
						}
						else if (xx <= -1 && yy >= 1)
						{
							Misc.SendMessage("Right");
							RazorEnhanced.Player.Walk("Right");
						}
						else if (xx == 0 && yy >= 1)
						{
							Misc.SendMessage("North");
							RazorEnhanced.Player.Walk("North");
						}

						Misc.Pause(1000);
						/*while (pp2 == World.Player.Position && dd2 == World.Player.Direction)
						{
							Misc.Pause(10);
						}*/
					}

				}

				RazorEnhanced.Misc.SendMessage("step -> X: " + step.x + " Y: " + step.y);

				RazorEnhanced.Player.Walk("");
			}
		}
	}
}