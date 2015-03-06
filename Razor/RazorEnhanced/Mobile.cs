using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Assistant;

namespace RazorEnhanced
{
	public class Mobile : EnhancedEntity
	{
		private Assistant.Mobile m_AssistantMobile;

		internal Mobile(Assistant.Mobile mobile)
			: base(mobile)
		{
			m_AssistantMobile = mobile;
		}

		public string Name { get { return m_AssistantMobile.Name; } }

		public int Body { get { return m_AssistantMobile.Body; } }

		public string Direction { get { return m_AssistantMobile.Direction.ToString(); } }

		public bool Visible { get { return m_AssistantMobile.Visible; } }

		public bool Poisoned { get { return m_AssistantMobile.Poisoned; } }

		public bool Blessed { get { return m_AssistantMobile.Blessed; } }

		public bool IsHuman { get { return m_AssistantMobile.IsHuman; } }

		public bool IsGhost { get { return m_AssistantMobile.IsGhost; } }

		public bool Warmode { get { return m_AssistantMobile.Warmode; } }

		public bool Female { get { return m_AssistantMobile.Female; } }

		public int Notoriety { get { return m_AssistantMobile.Notoriety; } }

		public int HitsMax { get { return m_AssistantMobile.HitsMax; } }

		public int Hits { get { return m_AssistantMobile.Hits; } }

		public int StamMax { get { return m_AssistantMobile.StamMax; } }

		public int Stam { get { return m_AssistantMobile.Stam; } }

		public int ManaMax { get { return m_AssistantMobile.ManaMax; } }

		public int Mana { get { return m_AssistantMobile.Mana; } }

		public int Map { get { return m_AssistantMobile.Map; } }

		public bool InParty { get { return Assistant.PacketHandlers.Party.Contains(m_AssistantMobile.Serial); } }

		private static Assistant.Layer GetAssistantLayer(string layer)
		{
			Assistant.Layer result = Assistant.Layer.Invalid;

			switch (layer)
			{
				case "RightHand":
					result = Assistant.Layer.RightHand;
					break;
				case "LeftHand":
					result = Assistant.Layer.LeftHand;
					break;
				case "Shoes":
					result = Assistant.Layer.Shoes;
					break;
				case "Pants":
					result = Assistant.Layer.Pants;
					break;
				case "Shirt":
					result = Assistant.Layer.Shirt;
					break;
				case "Head":
					result = Assistant.Layer.Head;
					break;
				case "Gloves":
					result = Assistant.Layer.Gloves;
					break;
				case "Ring":
					result = Assistant.Layer.Ring;
					break;
				case "Neck":
					result = Assistant.Layer.Neck;
					break;
				case "Hair":
					result = Assistant.Layer.Hair;
					break;
				case "Waist":
					result = Assistant.Layer.Waist;
					break;
				case "InnerTorso":
					result = Assistant.Layer.InnerTorso;
					break;
				case "Bracelet":
					result = Assistant.Layer.Bracelet;
					break;
				case "FacialHair":
					result = Assistant.Layer.FacialHair;
					break;
				case "MiddleTorso":
					result = Assistant.Layer.MiddleTorso;
					break;
				case "Earrings":
					result = Assistant.Layer.Earrings;
					break;
				case "Arms":
					result = Assistant.Layer.Arms;
					break;
				case "Cloak":
					result = Assistant.Layer.Cloak;
					break;
				case "OuterTorso":
					result = Assistant.Layer.OuterTorso;
					break;
				case "OuterLegs":
					result = Assistant.Layer.OuterLegs;
					break;
				case "InnerLegs":
					result = Assistant.Layer.InnerLegs;
					break;
				default:
					result = Assistant.Layer.Invalid;
					break;
			}

			return result;
		}

		public Item GetItemOnLayer(String layer)
		{
			Assistant.Layer assistantLayer = GetAssistantLayer(layer);

			Assistant.Item assistantItem = null;
			if (assistantLayer != Assistant.Layer.Invalid)
			{
				assistantItem = m_AssistantMobile.GetItemOnLayer(assistantLayer);
				if (assistantItem == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
					return enhancedItem;
				}
			}
			else
				return null;
		}

		public Item Backpack
		{
			get
			{
				Assistant.Item assistantBackpack = m_AssistantMobile.Backpack;
				if (assistantBackpack == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedBackpack = new RazorEnhanced.Item(assistantBackpack);
					return enhancedBackpack;
				}
			}
		}

		public Item Quiver
		{
			get
			{
				Assistant.Item assistantQuiver = m_AssistantMobile.Quiver;
				if (assistantQuiver == null)
					return null;
				else
				{
					RazorEnhanced.Item enhancedQuiver = new RazorEnhanced.Item(assistantQuiver);
					return enhancedQuiver;
				}
			}
		}

		public Item FindItemByID(ItemID id)
		{
			Assistant.Item assitantItem = m_AssistantMobile.FindItemByID((ushort)id.Value);
			if (assitantItem == null)
				return null;
			else
			{
				RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assitantItem);
				return enhancedItem;
			}
		}

		public ArrayList Contains
		{
			get
			{
				ArrayList items = new ArrayList();
				foreach (Assistant.Item assistantItem in m_AssistantMobile.Contains)
				{
					RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
					items.Add(enhancedItem);
				}
				return items;
			}
		}

		public ArrayList Properties
		{
			get
			{
				ArrayList properties = new ArrayList();
				foreach (Assistant.ObjectPropertyList.OPLEntry entry in m_AssistantMobile.ObjPropList.Content)
				{
					Property property = new Property(entry);
					properties.Add(property);
				}
				return properties;
			}
		}

		//find 
		public static Mobile FindBySerial(int serial)
		{

			Assistant.Mobile assistantMobile = Assistant.World.FindMobile((Assistant.Serial)((uint)serial));
			if (assistantMobile == null)
			{
				Misc.SendMessage("Script Error: FindBySerial: Item serial: (" + serial + ") not found");
				return null;
			}
			else
			{
				RazorEnhanced.Mobile enhancedMobile = new RazorEnhanced.Mobile(assistantMobile);
				return enhancedMobile;
			}
		}

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
						result = mobiles[Utility.Random(mobiles.Count)] as Mobile;
						break;
					case "Nearest":
						Mobile nearest = mobiles[0] as Mobile;
						if (nearest != null)
						{
							double minDist = Misc.DistanceSqrt(Player.Position, nearest.Position);
							for (int i = 0; i < mobiles.Count; i++)
							{
								Mobile mob = mobiles[i] as Mobile;
								if (mob != null)
								{
									double dist = Misc.DistanceSqrt(Player.Position, mob.Position);
									if (dist < minDist)
									{
										nearest = mob;
										minDist = dist;
									}
								}
							}
							result = nearest;
						}
						break;
					case "Farthest":
						Mobile farthest = mobiles[0] as Mobile;
						if (farthest != null)
						{
							double maxDist = Misc.DistanceSqrt(Player.Position, farthest.Position);
							for (int i = 0; i < mobiles.Count; i++)
							{
								Mobile mob = mobiles[i] as Mobile;
								if (mob != null)
								{
									double dist = Misc.DistanceSqrt(Player.Position, mob.Position);
									if (dist > maxDist)
									{
										farthest = mob;
										maxDist = dist;
									}
								}
							}
							result = farthest;
						}
						break;
					case "Weakest":
						Mobile weakest = mobiles[0] as Mobile;
						if (weakest != null)
						{
							int minHits = weakest.Hits;
							for (int i = 0; i < mobiles.Count; i++)
							{
								Mobile mob = mobiles[i] as Mobile;
								if (mob != null)
								{
									int wounds = mob.Hits;
									if (wounds < minHits)
									{
										weakest = mob;
										minHits = wounds;
									}
								}
							}
							result = weakest;
						}
						break;
					case "Strongest":
						Mobile strongest = mobiles[0] as Mobile;
						if (strongest != null)
						{
							int maxHits = strongest.Hits;
							for (int i = 0; i < mobiles.Count; i++)
							{
								Mobile mob = mobiles[i] as Mobile;
								if (mob != null)
								{
									int wounds = mob.Hits;
									if (wounds > maxHits)
									{
										strongest = mob;
										maxHits = wounds;
									}
								}
							}
							result = strongest;
						}
						break;
				}
			}

			return result;
		}
	}
}


