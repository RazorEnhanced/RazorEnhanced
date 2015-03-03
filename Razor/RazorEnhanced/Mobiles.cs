using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Assistant;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
	public class Mobiles
	{
		public class Filter
		{
			public bool Enabled = false;
			public ArrayList Serials = new ArrayList();
			public ArrayList Bodies = new ArrayList();
			public string Name = "";
			public ArrayList Hues = new ArrayList();
			public double RangeMin = -1;
			public double RangeMax = -1;
			public bool Poisoned = false;
			public bool Blessed = false;
			public bool IsHuman = false;
			public bool IsGhost = false;
			public bool Female = false;
			public bool Warmode = false;
			public ArrayList Notorieties = new ArrayList();

			public Filter()
			{
			}
		}

		public static ArrayList ApplyFilter(Filter filter)
		{

			ArrayList result = new ArrayList();

			List<Assistant.Mobile> assistantMobiles = Assistant.World.Mobiles.Values.ToList();

			if (filter.Enabled)
			{
				if (filter.Serials.Count > 0)
				{
					assistantMobiles = assistantMobiles.Where((m) => filter.Serials.Contains((int)m.Serial.Value)).ToList();
				}
				else
				{
					if (filter.Name != "")
					{
						Regex rgx = new Regex(filter.Name, RegexOptions.IgnoreCase);
						List<Assistant.Mobile> list = new List<Assistant.Mobile>();
						foreach (Assistant.Mobile i in assistantMobiles)
						{
							if (rgx.IsMatch(i.Name))
							{
								list.Add(i);
							}
						}
						assistantMobiles = list;
					}

					if (filter.Bodies.Count > 0)
					{
						assistantMobiles = assistantMobiles.Where((m) => filter.Bodies.Contains(m.Body)).ToList();
					}

					if (filter.Hues.Count > 0)
					{
						assistantMobiles = assistantMobiles.Where((i) => filter.Hues.Contains(i.Hue)).ToList();
					}

					if (filter.RangeMin != -1)
					{
						assistantMobiles = assistantMobiles.Where((m) =>
							Utility.DistanceSqrt
							(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(m.Position.X, m.Position.Y)) >= filter.RangeMin
						).ToList();
					}

					if (filter.RangeMax != -1)
					{
						assistantMobiles = assistantMobiles.Where((m) =>
							Utility.DistanceSqrt
							(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(m.Position.X, m.Position.Y)) <= filter.RangeMax
						).ToList();
					}

					assistantMobiles = assistantMobiles.Where((m) => m.Poisoned == filter.Poisoned).ToList();
					assistantMobiles = assistantMobiles.Where((m) => m.Blessed == filter.Blessed).ToList();
					assistantMobiles = assistantMobiles.Where((m) => m.IsHuman == filter.IsHuman).ToList();
					assistantMobiles = assistantMobiles.Where((m) => m.IsGhost == filter.IsGhost).ToList();
					assistantMobiles = assistantMobiles.Where((m) => m.Female == filter.Female).ToList();
					assistantMobiles = assistantMobiles.Where((m) => m.Warmode == filter.Warmode).ToList();

					if (filter.Notorieties.Count > 0)
					{
						assistantMobiles = assistantMobiles.Where((m) => filter.Notorieties.Contains(m.Notoriety)).ToList();
					}
				}
			}

			foreach (Assistant.Mobile assistantMobile in assistantMobiles)
			{
				RazorEnhanced.Mobile enhancedMobile = new RazorEnhanced.Mobile(assistantMobile);
				result.Add(enhancedMobile);
			}
			return result;
		}

		public static Mobile Select(ArrayList mobiles, string selector)
		{
			Mobile result = null;

			if (mobiles.Count > 0)
			{
				switch (selector)
				{
					case "Random":
						result = (Mobile)mobiles[Utility.Random(mobiles.Count)];
						break;
					case "Nearest":
						Mobile nearest = (Mobile)mobiles[0];
						double minDist = Misc.DistanceSqrt(Player.Position, nearest.Position);
						for (int i = 0; i < mobiles.Count; i++)
						{
							Mobile mob = (Mobile)mobiles[i];
							double dist = Misc.DistanceSqrt(Player.Position, mob.Position);
							if (dist < minDist)
							{
								nearest = mob;
								minDist = dist;
							}
						}
						result = nearest;
						break;
					case "Farthest":
						Mobile farthest = (Mobile)mobiles[0];
						double maxDist = Misc.DistanceSqrt(Player.Position, farthest.Position);
						for (int i = 0; i < mobiles.Count; i++)
						{
							Mobile mob = (Mobile)mobiles[i];
							double dist = Misc.DistanceSqrt(Player.Position, mob.Position);
							if (dist > maxDist)
							{
								farthest = mob;
								maxDist = dist;
							}
						}
						result = farthest;
						break;
					case "Weakest":
						Mobile weakest = (Mobile)mobiles[0];
						int minHits = weakest.Hits;
						for (int i = 0; i < mobiles.Count; i++)
						{
							Mobile mob = (Mobile)mobiles[i];
							int wounds = mob.Hits;
							if (wounds < minHits)
							{
								weakest = mob;
								minHits = wounds;
							}
						}
						result = weakest;
						break;
					case "Strongest":
						Mobile strongest = (Mobile)mobiles[0];
						int maxHits = strongest.Hits;
						for (int i = 0; i < mobiles.Count; i++)
						{
							Mobile mob = (Mobile)mobiles[i];
							int wounds = mob.Hits;
							if (wounds > maxHits)
							{
								strongest = mob;
								maxHits = wounds;
							}
						}
						result = strongest;
						break;
				}
			}

			return result;
		}

		public static Mobile FindBySerial(int serial)
		{

			Assistant.Mobile assistantMobile = Assistant.World.FindMobile((Assistant.Serial)((uint)serial));
			if (assistantMobile == null)
			{
				Misc.SendMessage("Script Error: FindBySerial: Mobile serial: (" + serial + ") not found");
				return null;
			}
			else
			{
				RazorEnhanced.Mobile enhancedMobile = new RazorEnhanced.Mobile(assistantMobile);
				return enhancedMobile;
			}
		}
	}
}
