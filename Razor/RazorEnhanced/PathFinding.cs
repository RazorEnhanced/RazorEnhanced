using System.Collections.Generic;
using Ultima;
using Assistant;
using System;
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
			Dictionary<int, bool> zThings = new Dictionary<int, bool>();
			short landId = (short)Statics.GetLandID(x, y, Player.Map);
			short zValue = (short)Statics.GetLandZ(x, y, Player.Map);

			if (m_landdata.ContainsKey(landId))
			{
				TileFlag templandflag = m_landdata[landId];

				if ((templandflag & TileFlag.Impassable) != 0 || (templandflag & TileFlag.Wall) != 0 ||
					(templandflag & TileFlag.Wet) != 0)
				{
					zThings.Add(zValue, true);
				}
				else
				{
					zThings.Add(zValue, false);
				}
			}

			List<Statics.TileInfo> staticitemsid = Statics.GetStaticsTileInfo(x, y, Player.Map);

			if (staticitemsid.Count > 0)
			{
				foreach (Statics.TileInfo tile in staticitemsid)
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
						zThings.Add(i.Position.Z, false);
					}
				}
			}

			return zThings;
		}

		public static bool CheckHouse(int x, int y)
		{
			List<Multi.MultiData> multidata = Assistant.World.Multis.Values.ToList();

			foreach (Multi.MultiData multi in multidata)
			{
				if (x >= multi.Corner1.X && x <= multi.Corner2.X && y >= multi.Corner1.Y && y <= multi.Corner2.Y)
				{
					return true;
				}
			}
			return false;
		}

		public static void TestRoute(int startx, int starty, int startz, int endx, int endy, int endz)
		{
			if (!m_loaded)
				LoadTileLandData();

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

			//Riempie la griglia
			for (int x = 0, xx = mediumXPoint - range; x < walkgrid.Right; x++, xx++)
			{
				for (int y = 0, yy = mediumYPoint - range; y < walkgrid.Top; y++, yy++)
				{
					//Controllo che non sia una casa
					if (CheckHouse(xx, yy))
					{
						//Se in queste x-y c'è una casa, setta tutta la Z a False
						for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
						{
							walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), false);
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
									walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), zResults[zz]);
								else
									walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
							}
						}
						else
						{
							//Se in queste x-y c'è roba statica, setta tutta la Z a False
							for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
							{
								walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
							}
						}

						zResults = CheckDynamic(xx, yy);

						if (zResults.Count > 0)
						{
							//Se in queste x-y c'è roba statica, setta tutta la Z a False
							for (int z = 0, zz = -127; z < walkgrid.Back; z++, zz++)
							{
								if (zResults.ContainsKey(zz))
									walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), zResults[zz]);
								else
									walkgrid.MarkPosition(new PathFinderAStar3D.Point3D(x, y, z), true);
							}
						}
					}
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

			//Calcola il percorso
			SearchNode PathList = PathFinder.FindPath(walkgrid, startPos, endPos);

			//Va avanti con gli step
			if (PathList.next != null)
			{
				PathFinderAStar3D.Point3D oldstep = PathList.position;
				PathFinderAStar3D.Point3D step = PathList.next.position;

				int xx1 = step.X - oldstep.X;
				int yy1 = step.Y - oldstep.Y;
				Assistant.Point3D oldplayerpos = Assistant.World.Player.Position;

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
				walkgrid = null;

				TestRoute(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y, Assistant.World.Player.Position.Z, endx, endy, endz);
			}

			/*if (resultPathList.Count > 1)
			{
				GridPos oldstep = resultPathList[0];
				GridPos step = resultPathList[1];

				int xx1 = step.x - oldstep.x;
				int yy1 = step.y - oldstep.y;
				Assistant.Point3D oldplayerpos = World.Player.Position;

				while (World.Player.Position.X != oldplayerpos.X + xx1 ||
					   World.Player.Position.Y != oldplayerpos.Y + yy1)
				{
					Assistant.Point3D pp2 = World.Player.Position;
					Assistant.Direction dd2 = World.Player.Direction;

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

					while (pp2 == World.Player.Position && dd2 == World.Player.Direction)
					{
						Misc.Pause(10);
					}
				}

				walkgrid = null;*/

		}
	}
	}