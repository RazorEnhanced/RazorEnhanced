using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Assistant;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
	public class Items
	{
		public class Filter
		{
			public bool Enabled = false;
			public ArrayList Serials = new ArrayList();
			public ArrayList Graphics = new ArrayList();
			public string Name = "";
			public ArrayList Hues = new ArrayList();
			public double RangeMin = -1;
			public double RangeMax = -1;
			public bool Movable = true;
			public ArrayList Layers = new ArrayList();
			public bool OnGround = false;
			public bool IsCorpse = false;
			public bool IsContainer = false;

			public Filter()
			{
			}
		}

		public static ArrayList ApplyFilter(Filter filter)
		{
			ArrayList result = new ArrayList();

			List<Assistant.Item> assistantItems = Assistant.World.Items.Values.ToList();

			if (filter.Enabled)
			{
				if (filter.Serials.Count > 0)
				{
					assistantItems = assistantItems.Where((i) => filter.Serials.Contains((int)i.Serial.Value)).ToList();
				}
				else
				{
					if (filter.Name != "")
					{
						Regex rgx = new Regex(filter.Name, RegexOptions.IgnoreCase);
						List<Assistant.Item> list = new List<Assistant.Item>();
						foreach (Assistant.Item i in assistantItems)
						{
							if (rgx.IsMatch(i.Name))
							{
								list.Add(i);
							}
						}
						assistantItems = list;
					}

					if (filter.Graphics.Count > 0)
					{
						assistantItems = assistantItems.Where((i) => filter.Graphics.Contains(i.ItemID.Value)).ToList();
					}

					if (filter.Hues.Count > 0)
					{
						assistantItems = assistantItems.Where((i) => filter.Hues.Contains(i.Hue)).ToList();
					}

					if (filter.RangeMin != -1)
					{
						assistantItems = assistantItems.Where((i) =>
							Utility.DistanceSqrt
							(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(i.Position.X, i.Position.Y)) >= filter.RangeMin
						).ToList();
					}

					if (filter.RangeMax != -1)
					{
						assistantItems = assistantItems.Where((i) =>
							Utility.DistanceSqrt
							(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(i.Position.X, i.Position.Y)) <= filter.RangeMax
						).ToList();
					}

					assistantItems = assistantItems.Where((i) => i.Movable == filter.Movable).ToList();

					if (filter.Layers.Count > 0)
					{
						List<Assistant.Layer> list = new List<Assistant.Layer>();

						foreach (string text in filter.Layers)
						{
							Assistant.Layer layer = RazorEnhanced.Player.GetAssistantLayer(text);
							if (layer != Assistant.Layer.Invalid)
							{
								list.Add(layer);
							}
						}

						assistantItems = assistantItems.Where((i) => list.Contains(i.Layer)).ToList();
					}

					assistantItems = assistantItems.Where((i) => i.OnGround == filter.OnGround).ToList();
					assistantItems = assistantItems.Where((i) => i.IsContainer == filter.IsContainer).ToList();
					assistantItems = assistantItems.Where((i) => i.IsCorpse == filter.IsCorpse).ToList();
				}
			}

			foreach (Assistant.Item assistantItem in assistantItems)
			{
				RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
				result.Add(enhancedItem);
			}

			return result;
		}

		public static Item Select(ArrayList items, string selector)
		{
			Item result = null;

			if (items.Count > 0)
			{
				switch (selector)
				{
					case "Random":
						result = (Item)items[Utility.Random(items.Count)];
						break;
					case "Nearest":
						Item nearest = (Item)items[0];
						double minDist = Misc.DistanceSqrt(Player.Position, nearest.Position);
						for (int i = 0; i < items.Count; i++)
						{
							Item item = (Item)items[i];
							double dist = Misc.DistanceSqrt(Player.Position, item.Position);
							if (dist < minDist)
							{
								nearest = item;
								minDist = dist;
							}
						}
						result = nearest;
						break;
					case "Farthest":
						Item farthest = (Item)items[0];
						double maxDist = Misc.DistanceSqrt(Player.Position, farthest.Position);
						for (int i = 0; i < items.Count; i++)
						{
							Item item = (Item)items[i];
							double dist = Misc.DistanceSqrt(Player.Position, item.Position);
							if (dist > maxDist)
							{
								farthest = item;
								maxDist = dist;
							}
						}
						result = farthest;
						break;
					case "Less":
						Item least = (Item)items[0];
						int minAmount = least.Amount;
						for (int i = 0; i < items.Count; i++)
						{
							Item item = (Item)items[i];
							int amount = item.Amount;
							if (amount < minAmount)
							{
								least = item;
								minAmount = amount;
							}
						}
						result = least;
						break;
					case "Most":
						Item most = (Item)items[0];
						int maxAmount = most.Amount;
						for (int i = 0; i < items.Count; i++)
						{
							Item item = (Item)items[i];
							int amount = item.Amount;
							if (amount > maxAmount)
							{
								most = item;
								maxAmount = amount;
							}
						}
						result = most;
						break;
					case "Weakest":
						Item weakest = (Item)items[0];
						int minDur = weakest.Durability;
						for (int i = 0; i < items.Count; i++)
						{
							Item item = (Item)items[i];
							int dur = item.Durability;
							if (dur < minDur)
							{
								weakest = item;
								minDur = dur;
							}
						}
						result = weakest;
						break;
					case "Strongest":
						Item strongest = (Item)items[0];
						int maxDur = strongest.Amount;
						for (int i = 0; i < items.Count; i++)
						{
							Item item = (Item)items[i];
							int dur = item.Durability;
							if (dur > maxDur)
							{
								strongest = item;
								maxDur = dur;
							}
						}
						result = strongest;
						break;
				}
			}

			return result;
		}

		public static Item FindBySerial(int serial)
		{

			Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (assistantItem == null)
			{
				Misc.SendMessage("Script Error: FindBySerial: Item serial: (" + serial + ") not found");
				return null;
			}
			else
			{
				RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
				return enhancedItem;
			}
		}
		public static void Move(Item item, Item bag, int amount)
		{
			if (item == null)
			{
				Misc.SendMessage("Script Error: Move: Source Item  not found");
				return;
			}
			if (bag == null)
			{
				Misc.SendMessage("Script Error: Move: Destination Item not found");
				return;
			}
			if (!bag.IsContainer)
			{
				Misc.SendMessage("Script Error: Move: Destination Item is not a container");
				return;
			}
			if (amount == 0)
			{
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
			}
		}

		public static void DropItemGroundSelf(Item item, int amount)
		{
			if (item == null)
			{
				Misc.SendMessage("Script Error: DropItemGroundSelf: Item not found");
				return;
			}
			if (amount == 0)
			{
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, World.Player.Position, Assistant.Serial.Zero));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, World.Player.Position, Assistant.Serial.Zero));
			}
		}
		public static void UseItem(Item item)
		{
			Assistant.ClientCommunication.SendToServer(new DoubleClick((Assistant.Serial)item.Serial));
		}
		public static void UseItem(uint itemserial)
		{
			Assistant.Item item = Assistant.World.FindItem(itemserial);
			if (item.Serial.IsItem)
				Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
			else
				Misc.SendMessage("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");

		}
		public static void UseItemByID(UInt16 ItemID)                   // Da verificare il findbyid dove cerca
		{
			Assistant.Item item = World.Player.FindItemByID(ItemID);
			if (item.Serial.IsItem)
			{
				Misc.SendMessage("Script Error: UseItemByID: No item whit ID:(" + ItemID.ToString() + ") found!");
				return;
			}
			if (item.Serial.IsItem)
				Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
			else
				Misc.SendMessage("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
		}

		// Props
		public static int GetPropByCliloc(RazorEnhanced.Item item, int code)
		{

			return GetPropExec(item, code, "GetPropByCliloc");
		}
		public static int GetPropByString(RazorEnhanced.Item item, string props)
		{
			switch (props)
			{
				case "Balanced":
					return GetPropExec(item, 1072792, "GetPropByString");
				case "Cold Resist":
					return GetPropExec(item, 1060445, "GetPropByString");
				case "Damage Increase":
					{
						if (GetPropExec(item, 1060401, "GetPropByString") != 0)
							return GetPropExec(item, 1060401, "GetPropByString");
						return GetPropExec(item, 1060402, "GetPropByString");
					}
				case "Defense Chance Increase":
					return GetPropExec(item, 1060408, "GetPropByString");
				case "Dexterity Bonus":
					return GetPropExec(item, 1060409, "GetPropByString");
				case "Energy Resist":
					return GetPropExec(item, 1060446, "GetPropByString");
				case "Faster Cast Recovery":
					return GetPropExec(item, 1060412, "GetPropByString");
				case "Enhance Potion":
					return GetPropExec(item, 1060411, "GetPropByString");
				case "Energy Damage":
					return GetPropExec(item, 1060407, "GetPropByString");
				case "Poison Damage":
					return GetPropExec(item, 1060406, "GetPropByString");
				case "Fire Damage":
					return GetPropExec(item, 1060405, "GetPropByString");
				case "Cold Damage":
					return GetPropExec(item, 1060404, "GetPropByString");
				case "Physical Damage":
					return GetPropExec(item, 1060403, "GetPropByString");
				case "Faster Casting":
					return GetPropExec(item, 1060413, "GetPropByString");
				case "Gold Increase":
					return GetPropExec(item, 1060414, "GetPropByString");
				case "Fire Resist":
					return GetPropExec(item, 1060447, "GetPropByString");
				case "Hit Chance Increase":
					return GetPropExec(item, 1060415, "GetPropByString");
				case "Hit Energy Area":
					return GetPropExec(item, 1060418, "GetPropByString");
				case "Hit Dispel":
					return GetPropExec(item, 1060417, "GetPropByString");
				case "Hit Cold Area":
					return GetPropExec(item, 1060416, "GetPropByString");
				case "Hit Fire Area":
					return GetPropExec(item, 1060419, "GetPropByString");
				case "Hit Fireball":
					return GetPropExec(item, 1060420, "GetPropByString");
				case "Hit Life Leech":
					return GetPropExec(item, 1060422, "GetPropByString");
				case "Hit Point Increase":
					return GetPropExec(item, 1060431, "GetPropByString");
				case "Hit Point Regeneration":
					return GetPropExec(item, 1060444, "GetPropByString");
				case "Hit Stamina Leech":
					return GetPropExec(item, 1060430, "GetPropByString");
				case "Hit Poison Area":
					return GetPropExec(item, 1060429, "GetPropByString");
				case "Hit Physical Area":
					return GetPropExec(item, 1060428, "GetPropByString");
				case "Hit Mana Leech":
					return GetPropExec(item, 1060427, "GetPropByString");
				case "Hit Magic Arrow":
					return GetPropExec(item, 1060426, "GetPropByString");
				case "Hit Lower Defence":
					return GetPropExec(item, 1060425, "GetPropByString");
				case "Hit Lower Attack":
					return GetPropExec(item, 1060424, "GetPropByString");
				case "Hit Lightning":
					return GetPropExec(item, 1060423, "GetPropByString");
				case "Hit Harm":
					return GetPropExec(item, 1060421, "GetPropByString");
				case "Intelligence Bonus":
					return GetPropExec(item, 1060432, "GetPropByString");
				case "Lower Mana Cost":
					return GetPropExec(item, 1060433, "GetPropByString");
				case "Lower Reagent Cost":
					return GetPropExec(item, 1060434, "GetPropByString");
				case "Lower Requirements":
					return GetPropExec(item, 1060435, "GetPropByString");
				case "Luck":
					return GetPropExec(item, 1060436, "GetPropByString");
				case "Mana Increase":
					return GetPropExec(item, 1060439, "GetPropByString");
				case "Mana Regeneration":
					return GetPropExec(item, 1060440, "GetPropByString");
				case "Physical Resist":
					return GetPropExec(item, 1060448, "GetPropByString");
				case "Poison Resist":
					return GetPropExec(item, 1060449, "GetPropByString");
				case "Nighr Sight":
					return GetPropExec(item, 1060441, "GetPropByString");
				case "Spell Channeling":
					return GetPropExec(item, 1060482, "GetPropByString");
				case "Spell Damage Increase":
					return GetPropExec(item, 1060483, "GetPropByString");
				case "Splintering Weapon":
					return GetPropExec(item, 1112857, "GetPropByString");
				case "Stamina Increase":
					return GetPropExec(item, 1060484, "GetPropByString");
				case "Stamina Regeneration":
					return GetPropExec(item, 1060443, "GetPropByString");
				case "Swing Speed Increase":
					return GetPropExec(item, 1060486, "GetPropByString");
				case "Velocity":
					return GetPropExec(item, 1072792, "GetPropByString");
				case "Self Repair":
					return GetPropExec(item, 1060450, "GetPropByString");
				case "Reflect Physical Damage":
					return GetPropExec(item, 1060442, "GetPropByString");
				case "Night Sight":
					return GetPropExec(item, 1060441, "GetPropByString");
				case "Mage Armor":
					return GetPropExec(item, 1060437, "GetPropByString");
				case "Strenght Bonus":
					return GetPropExec(item, 1060485, "GetPropByString");
				case "Water Elemental Slayer":
					return GetPropExec(item, 1060482, "GetPropByString");
				case "Troll Slayer":
					return GetPropExec(item, 1060480, "GetPropByString");
				case "Undead Slayer":
					return GetPropExec(item, 1060479, "GetPropByString");
				case "Terathan Slayer":
					return GetPropExec(item, 1060478, "GetPropByString");
				case "Spider Slayer":
					return GetPropExec(item, 1060477, "GetPropByString");
				case "Snow Elemental Slayer":
					return GetPropExec(item, 1060476, "GetPropByString");
				case "Snake Slayer":
					return GetPropExec(item, 1060475, "GetPropByString");
				case "Scorpion Slayer":
					return GetPropExec(item, 1060474, "GetPropByString");
				case "Reptile Slayer":
					return GetPropExec(item, 1060473, "GetPropByString");
				case "Repond Slayer":
					return GetPropExec(item, 1060472, "GetPropByString");
				case "Poison Elemental Slayer":
					return GetPropExec(item, 1060471, "GetPropByString");
				case "Orc Slayer":
					return GetPropExec(item, 1060470, "GetPropByString");
				case "Ophidian Slayer":
					return GetPropExec(item, 1060469, "GetPropByString");
				case "Ogre Slayer":
					return GetPropExec(item, 1060468, "GetPropByString");
				case "Lizardman Slayer":
					return GetPropExec(item, 1060467, "GetPropByString");
				case "Gargoyle Slayer":
					return GetPropExec(item, 1060466, "GetPropByString");
				case "Fire Elemental Slayer":
					return GetPropExec(item, 1060465, "GetPropByString");
				case "Elemental Slayer":
					return GetPropExec(item, 1060464, "GetPropByString");
				case "Earth Elemental Slayer":
					return GetPropExec(item, 1060463, "GetPropByString");
				case "Dragon Slayer":
					return GetPropExec(item, 1060462, "GetPropByString");
				case "Demon Slayer":
					{
						if (GetPropExec(item, 1060460, "GetPropByString") != 0)
							return GetPropExec(item, 1060460, "GetPropByString");
						return GetPropExec(item, 1060461, "GetPropByString");
					}
				case "Blood Elemental Slayer":
					return GetPropExec(item, 1060459, "GetPropByString");
				case "Arachnid Slayer":
					return GetPropExec(item, 1060458, "GetPropByString");
				case "Air Elemental Slayer":
					return GetPropExec(item, 1060457, "GetPropByString");
				case "Magic Arrow Charges":
					return GetPropExec(item, 1060492, "GetPropByString");
				case "Lightning Charges":
					return GetPropExec(item, 1060491, "GetPropByString");
				case "Healing Charges":
					return GetPropExec(item, 1060490, "GetPropByString");
				case "Harm Charges":
					return GetPropExec(item, 1060489, "GetPropByString");
				case "Greater Healing Charges":
					return GetPropExec(item, 1060488, "GetPropByString");
				case "Fireball Charges":
					return GetPropExec(item, 1060487, "GetPropByString");
				default:
					Misc.SendMessage("Script Error: GetPropByString: Invalid or not supported props string");
					return 0;
			}
		}

		public static int GetPropByCliloc(uint serial, int code)
		{
			Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (assistantItem == null)
			{
				Misc.SendMessage("Script Error: GetPropByCliloc: Item serial: (" + serial + ") not found");
				return 0;
			}
			else
			{
				RazorEnhanced.Item item = new RazorEnhanced.Item(assistantItem);

				return GetPropExec(item, code, "GetPropByCliloc");

			}
		}
		public static int GetPropByString(uint serial, string props)
		{
			Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (assistantItem == null)
			{
				Misc.SendMessage("Script Error: GetPropByString: Item serial: (" + serial + ") not found");
				return 0;
			}
			else
			{
				RazorEnhanced.Item item = new RazorEnhanced.Item(assistantItem);
				switch (props)
				{
					case "Balanced":
						return GetPropExec(item, 1072792, "GetPropByString");
					case "Cold Resist":
						return GetPropExec(item, 1060445, "GetPropByString");
					case "Damage Increase":
						{
							if (GetPropExec(item, 1060401, "GetPropByString") != 0)
								return GetPropExec(item, 1060401, "GetPropByString");
							return GetPropExec(item, 1060402, "GetPropByString");
						}
					case "Defense Chance Increase":
						return GetPropExec(item, 1060408, "GetPropByString");
					case "Dexterity Bonus":
						return GetPropExec(item, 1060409, "GetPropByString");
					case "Energy Resist":
						return GetPropExec(item, 1060446, "GetPropByString");
					case "Faster Cast Recovery":
						return GetPropExec(item, 1060412, "GetPropByString");
					case "Enhance Potion":
						return GetPropExec(item, 1060411, "GetPropByString");
					case "Energy Damage":
						return GetPropExec(item, 1060407, "GetPropByString");
					case "Poison Damage":
						return GetPropExec(item, 1060406, "GetPropByString");
					case "Fire Damage":
						return GetPropExec(item, 1060405, "GetPropByString");
					case "Cold Damage":
						return GetPropExec(item, 1060404, "GetPropByString");
					case "Physical Damage":
						return GetPropExec(item, 1060403, "GetPropByString");
					case "Faster Casting":
						return GetPropExec(item, 1060413, "GetPropByString");
					case "Gold Increase":
						return GetPropExec(item, 1060414, "GetPropByString");
					case "Fire Resist":
						return GetPropExec(item, 1060447, "GetPropByString");
					case "Hit Chance Increase":
						return GetPropExec(item, 1060415, "GetPropByString");
					case "Hit Energy Area":
						return GetPropExec(item, 1060418, "GetPropByString");
					case "Hit Dispel":
						return GetPropExec(item, 1060417, "GetPropByString");
					case "Hit Cold Area":
						return GetPropExec(item, 1060416, "GetPropByString");
					case "Hit Fire Area":
						return GetPropExec(item, 1060419, "GetPropByString");
					case "Hit Fireball":
						return GetPropExec(item, 1060420, "GetPropByString");
					case "Hit Life Leech":
						return GetPropExec(item, 1060422, "GetPropByString");
					case "Hit Point Increase":
						return GetPropExec(item, 1060431, "GetPropByString");
					case "Hit Point Regeneration":
						return GetPropExec(item, 1060444, "GetPropByString");
					case "Hit Stamina Leech":
						return GetPropExec(item, 1060430, "GetPropByString");
					case "Hit Poison Area":
						return GetPropExec(item, 1060429, "GetPropByString");
					case "Hit Physical Area":
						return GetPropExec(item, 1060428, "GetPropByString");
					case "Hit Mana Leech":
						return GetPropExec(item, 1060427, "GetPropByString");
					case "Hit Magic Arrow":
						return GetPropExec(item, 1060426, "GetPropByString");
					case "Hit Lower Defence":
						return GetPropExec(item, 1060425, "GetPropByString");
					case "Hit Lower Attack":
						return GetPropExec(item, 1060424, "GetPropByString");
					case "Hit Lightning":
						return GetPropExec(item, 1060423, "GetPropByString");
					case "Hit Harm":
						return GetPropExec(item, 1060421, "GetPropByString");
					case "Intelligence Bonus":
						return GetPropExec(item, 1060432, "GetPropByString");
					case "Lower Mana Cost":
						return GetPropExec(item, 1060433, "GetPropByString");
					case "Lower Reagent Cost":
						return GetPropExec(item, 1060434, "GetPropByString");
					case "Lower Requirements":
						return GetPropExec(item, 1060435, "GetPropByString");
					case "Luck":
						return GetPropExec(item, 1060436, "GetPropByString");
					case "Mana Increase":
						return GetPropExec(item, 1060439, "GetPropByString");
					case "Mana Regeneration":
						return GetPropExec(item, 1060440, "GetPropByString");
					case "Physical Resist":
						return GetPropExec(item, 1060448, "GetPropByString");
					case "Poison Resist":
						return GetPropExec(item, 1060449, "GetPropByString");
					case "Nighr Sight":
						return GetPropExec(item, 1060441, "GetPropByString");
					case "Spell Channeling":
						return GetPropExec(item, 1060482, "GetPropByString");
					case "Spell Damage Increase":
						return GetPropExec(item, 1060483, "GetPropByString");
					case "Splintering Weapon":
						return GetPropExec(item, 1112857, "GetPropByString");
					case "Stamina Increase":
						return GetPropExec(item, 1060484, "GetPropByString");
					case "Stamina Regeneration":
						return GetPropExec(item, 1060443, "GetPropByString");
					case "Swing Speed Increase":
						return GetPropExec(item, 1060486, "GetPropByString");
					case "Velocity":
						return GetPropExec(item, 1072792, "GetPropByString");
					case "Self Repair":
						return GetPropExec(item, 1060450, "GetPropByString");
					case "Reflect Physical Damage":
						return GetPropExec(item, 1060442, "GetPropByString");
					case "Night Sight":
						return GetPropExec(item, 1060441, "GetPropByString");
					case "Mage Armor":
						return GetPropExec(item, 1060437, "GetPropByString");
					case "Strenght Bonus":
						return GetPropExec(item, 1060485, "GetPropByString");
					case "Water Elemental Slayer":
						return GetPropExec(item, 1060482, "GetPropByString");
					case "Troll Slayer":
						return GetPropExec(item, 1060480, "GetPropByString");
					case "Undead Slayer":
						return GetPropExec(item, 1060479, "GetPropByString");
					case "Terathan Slayer":
						return GetPropExec(item, 1060478, "GetPropByString");
					case "Spider Slayer":
						return GetPropExec(item, 1060477, "GetPropByString");
					case "Snow Elemental Slayer":
						return GetPropExec(item, 1060476, "GetPropByString");
					case "Snake Slayer":
						return GetPropExec(item, 1060475, "GetPropByString");
					case "Scorpion Slayer":
						return GetPropExec(item, 1060474, "GetPropByString");
					case "Reptile Slayer":
						return GetPropExec(item, 1060473, "GetPropByString");
					case "Repond Slayer":
						return GetPropExec(item, 1060472, "GetPropByString");
					case "Poison Elemental Slayer":
						return GetPropExec(item, 1060471, "GetPropByString");
					case "Orc Slayer":
						return GetPropExec(item, 1060470, "GetPropByString");
					case "Ophidian Slayer":
						return GetPropExec(item, 1060469, "GetPropByString");
					case "Ogre Slayer":
						return GetPropExec(item, 1060468, "GetPropByString");
					case "Lizardman Slayer":
						return GetPropExec(item, 1060467, "GetPropByString");
					case "Gargoyle Slayer":
						return GetPropExec(item, 1060466, "GetPropByString");
					case "Fire Elemental Slayer":
						return GetPropExec(item, 1060465, "GetPropByString");
					case "Elemental Slayer":
						return GetPropExec(item, 1060464, "GetPropByString");
					case "Earth Elemental Slayer":
						return GetPropExec(item, 1060463, "GetPropByString");
					case "Dragon Slayer":
						return GetPropExec(item, 1060462, "GetPropByString");
					case "Demon Slayer":
						{
							if (GetPropExec(item, 1060460, "GetPropByString") != 0)
								return GetPropExec(item, 1060460, "GetPropByString");
							return GetPropExec(item, 1060461, "GetPropByString");
						}
					case "Blood Elemental Slayer":
						return GetPropExec(item, 1060459, "GetPropByString");
					case "Arachnid Slayer":
						return GetPropExec(item, 1060458, "GetPropByString");
					case "Air Elemental Slayer":
						return GetPropExec(item, 1060457, "GetPropByString");
					case "Magic Arrow Charges":
						return GetPropExec(item, 1060492, "GetPropByString");
					case "Lightning Charges":
						return GetPropExec(item, 1060491, "GetPropByString");
					case "Healing Charges":
						return GetPropExec(item, 1060490, "GetPropByString");
					case "Harm Charges":
						return GetPropExec(item, 1060489, "GetPropByString");
					case "Greater Healing Charges":
						return GetPropExec(item, 1060488, "GetPropByString");
					case "Fireball Charges":
						return GetPropExec(item, 1060487, "GetPropByString");
					default:
						Misc.SendMessage("Script Error: GetPropByString: Invalid or not supported props string");
						return 0;
				}
			}
		}

		private static int GetPropExec(RazorEnhanced.Item item, int code, String Fcall)
		{
			ArrayList properties = item.Properties;
			foreach (Property property in properties)
			{
				int number = property.Number;
				string args = property.Args;
				if (number == code)
				{
					if (args == null)  // Esiste prop ma senza valore
						return 1;
					else
					{
						try
						{
							return Convert.ToInt32(args);  // Ritorna valore
						}
						catch
						{
							Misc.SendMessage("Script Error: " + Fcall + ": Error to get value of Cliloc:" + code);
							return 0;  // errore di conversione
						}
					}
				}
			}
			return 0;       // Prop inesistente sul item
		}
	}
}
