using System;
using System.IO;
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

	/*	public Item FindItemByID(ItemID id)
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
        */
		public List<Item> Contains
		{
			get
			{
				List<Item> items = new List<Item>();
				foreach (Assistant.Item assistantItem in m_AssistantMobile.Contains)
				{
					RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
					items.Add(enhancedItem);
				}
				return items;
			}
		}

		public List<Property> Properties
		{
			get
			{
				List<Property> properties = new List<Property>();
				foreach (Assistant.ObjectPropertyList.OPLEntry entry in m_AssistantMobile.ObjPropList.Content)
				{
					Property property = new Property(entry);
					properties.Add(property);
				}
				return properties;
			}
		}
	}

	public class Mobiles
	{
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

        [Serializable]
		public class Filter
		{
			public bool Enabled = false;
			public List<int> Serials = new List<int>();
			public List<int> Bodies = new List<int>();
			public string Name = "";
			public List<int> Hues = new List<int>();
			public double RangeMin = -1;
			public double RangeMax = -1;
			public int Poisoned = -1;
            public int Blessed = -1;
            public int IsHuman = -1;
            public int IsGhost = -1;
            public int Female = -1;
            public int Warmode = -1;
            public int Friend = -1;
            public int Paralized = -1;
			public List<byte> Notorieties = new List<byte>();

			public Filter()
			{
			}
		}

		public static List<Mobile> ApplyFilter(Filter filter)
		{
			List<Mobile> result = new List<Mobile>();

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

                    if (filter.Warmode != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => m.Warmode == Convert.ToBoolean(filter.Warmode)).ToList();
                    }

                    if (filter.Poisoned != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => m.Poisoned == Convert.ToBoolean(filter.Poisoned)).ToList();
                    }

                    if (filter.Blessed != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => m.Blessed == Convert.ToBoolean(filter.Blessed)).ToList();
                    }

                    if (filter.IsHuman != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => m.IsHuman == Convert.ToBoolean(filter.IsHuman)).ToList();
                    }

                    if (filter.IsGhost != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => m.IsGhost == Convert.ToBoolean(filter.IsGhost)).ToList();
                    }

                    if (filter.Female != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => m.Female == Convert.ToBoolean(filter.Female)).ToList();
                    }

                    if (filter.Friend != -1)
                    {
                        assistantMobiles = assistantMobiles.Where((m) => RazorEnhanced.Friend.IsFriend(m.Serial) == Convert.ToBoolean(filter.Friend)).ToList();
                    }

                    if (filter.Paralized != -1)
                    {
                        // TODO PARALIZED FLAG
                        //assistantMobiles = assistantMobiles.Where((m) => m.Paralized == Convert.ToBoolean(filter.Paralized)).ToList();
                    }

					if (filter.Notorieties.Count > 0)
					{
						assistantMobiles = assistantMobiles.Where((m) => filter.Notorieties.Contains(m.Notoriety)).ToList();
					}

                    // Esclude Self dalla ricerca
                    assistantMobiles = assistantMobiles.Where((m) => m.Serial == World.Player.Serial).ToList();
				}
			}

			foreach (Assistant.Mobile assistantMobile in assistantMobiles)
			{
				RazorEnhanced.Mobile enhancedMobile = new RazorEnhanced.Mobile(assistantMobile);
				result.Add(enhancedMobile);
			}
			return result;
		}

		public static Mobile Select(List<Mobile> mobiles, string selector)
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
                        Mobile closest = null;
			            double closestDist = double.MaxValue;

                        foreach (Mobile m in mobiles)
			            {
                            if (m.Serial == World.Player.Serial)
                                continue;

				            double dist = Utility.DistanceSqrt(new Assistant.Point2D(m.Position.X, m.Position.Y) , World.Player.Position);

				            if (dist < closestDist || closest == null)
				            {
					            closestDist = dist;
					            closest = m;
				            }
			            }
                        result = closest;
						break;
					case "Farthest":
                        Mobile farthest = null;
                        double farthestDist = double.MinValue;

                        foreach (Mobile m in mobiles)
			            {
                            if (m.Serial == World.Player.Serial)
                                continue;

				            double dist = Utility.DistanceSqrt(new Assistant.Point2D(m.Position.X, m.Position.Y) , World.Player.Position);

                            if (dist > farthestDist || farthest == null)
				            {
                                farthestDist = dist;
                                farthest = m;
				            }
			            }
                        result = farthest;
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
        // USe

        public static void UseMobile(Mobile mobile)
        {
            Assistant.ClientCommunication.SendToServer(new DoubleClick(mobile.Serial));
        }
        public static void UseMobile(uint mobileserial)
        {
            Assistant.Mobile mobile = Assistant.World.FindMobile(mobileserial);
            if (mobile == null)
            {
                Misc.SendMessage("Script Error: UseMobile: Invalid Serial");
                return;
            }

            if (mobile.Serial.IsMobile)
                Assistant.ClientCommunication.SendToServer(new DoubleClick(mobile.Serial));
            else
                Misc.SendMessage("Script Error: UseMobile: (" + mobile.Serial.ToString() + ") is not a mobile");

        }

        // Single Click
        public static void SingleClick(Mobile mobile)
        {
            ClientCommunication.SendToServer(new SingleClick(mobile));
        }

        public static void SingleClick(int mobileserial)
        {
            Assistant.Mobile mobile = Assistant.World.FindMobile(mobileserial);
            if (mobile == null)
            {
                Misc.SendMessage("Script Error: SingleClick: Invalid Serial");
                return;
            }
            ClientCommunication.SendToServer(new SingleClick(mobile));
        }

		// Message
		public static void Message(Mobile mobile, int hue, string message)
		{
			Assistant.ClientCommunication.SendToClient(new UnicodeMessage(mobile.Serial, mobile.Body, MessageType.Regular, hue, 3, Language.CliLocName, mobile.Name, message));
		}

		public static void Message(int serial, int hue, string message)
		{
			Mobile mobile = FindBySerial(serial);
			Assistant.ClientCommunication.SendToClient(new UnicodeMessage(mobile.Serial, mobile.Body, MessageType.Regular, hue, 3, Language.CliLocName, mobile.Name, message));
		}

	}
}


