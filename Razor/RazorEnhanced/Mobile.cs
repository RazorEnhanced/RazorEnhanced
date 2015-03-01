using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

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

		public ushort Body { get { return m_AssistantMobile.Body; } }

		public string Direction
		{
			get { return m_AssistantMobile.Direction.ToString(); }
		}

		public bool Visible { get { return m_AssistantMobile.Visible; } }

		public bool Poisoned
		{
			get { return m_AssistantMobile.Poisoned; }
		}

		public bool Blessed { get { return m_AssistantMobile.Blessed; } }

		public bool IsGhost { get { return m_AssistantMobile.IsGhost; } }

		public bool Warmode { get { return m_AssistantMobile.Warmode; } }

		public bool Female { get { return m_AssistantMobile.Female; } }

		public byte Notoriety { get { return m_AssistantMobile.Notoriety; } }

		public int HitsMax { get { return m_AssistantMobile.HitsMax; } }

		public int Hits { get { return m_AssistantMobile.Hits; } }

		public int StamMax { get { return m_AssistantMobile.StamMax; } }

		public int Stam { get { return m_AssistantMobile.Stam; } }

		public int ManaMax { get { return m_AssistantMobile.ManaMax; } }

		public int Mana { get { return m_AssistantMobile.Mana; } }

		public byte Map { get { return m_AssistantMobile.Map; } }

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
	}
}


