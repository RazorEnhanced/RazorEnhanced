using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace RazorEnhanced
{
	public class Item : EnhancedEntity
	{
		private Assistant.Item m_AssistantItem;

		internal Item(Assistant.Item item)
			: base(item)
		{
			m_AssistantItem = item;
		}

		public bool Updated { get { return m_AssistantItem.Updated; } }

		public int ItemID { get { return m_AssistantItem.ItemID.Value; } }

		public int Amount { get { return m_AssistantItem.Amount; } }

		public string Direction { get { return m_AssistantItem.Direction.ToString(); } }

		public bool Visible { get { return m_AssistantItem.Visible; } }

		public bool Movable { get { return m_AssistantItem.Movable; } }

		public string Name { get { return m_AssistantItem.Name; } }

		public string Layer { get { return m_AssistantItem.Layer.ToString(); } }

		public object Container { get { return m_AssistantItem.Container; } }

		public object RootContainer { get { return m_AssistantItem.RootContainer; } }

		public bool IsChildOf(object parent)
		{
			return m_AssistantItem.IsChildOf(parent);
		}

		public int DistanceTo(Mobile m)
		{
			int x = Math.Abs(this.Position.X - m.Position.X);
			int y = Math.Abs(this.Position.Y - m.Position.Y);

			return x > y ? x : y;
		}

		public List<Item> Contains
		{
			get
			{
				List<Item> items = new List<Item>();
				foreach (Assistant.Item assistantItem in m_AssistantItem.Contains)
				{
					RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
					items.Add(enhancedItem);
				}
				return items;
			}
		}

		// possibly 4 bit x/y - 16x16?
		public byte GridNum { get { return m_AssistantItem.GridNum; } }

		public bool OnGround { get { return m_AssistantItem.OnGround; } }
		public bool IsContainer { get { return m_AssistantItem.IsContainer; } }

		public bool IsBagOfSending { get { return m_AssistantItem.IsBagOfSending; } }

		public bool IsInBank { get { return m_AssistantItem.IsInBank; } }

		public bool IsPouch { get { return m_AssistantItem.IsPouch; } }

		public bool IsCorpse { get { return m_AssistantItem.IsCorpse; } }

		public bool IsDoor { get { return m_AssistantItem.IsDoor; } }

		public bool IsResource { get { return m_AssistantItem.IsResource; } }

		public bool IsPotion { get { return m_AssistantItem.IsPotion; } }

		public bool IsVirtueShield { get { return m_AssistantItem.IsVirtueShield; } }

		public bool IsTwoHanded { get { return m_AssistantItem.IsTwoHanded; } }

		public override string ToString()
		{
			return m_AssistantItem.ToString();
		}

		public int Price { get { return m_AssistantItem.Price; } }

		public string BuyDesc { get { return m_AssistantItem.BuyDesc; } }

		public Point3D GetWorldPosition()
		{
			Assistant.Point3D assistantPoint = m_AssistantItem.GetWorldPosition();
			RazorEnhanced.Point3D enhancedPoint = new RazorEnhanced.Point3D(assistantPoint);
			return enhancedPoint;
		}

		internal Assistant.Layer AssistantLayer { get { return m_AssistantItem.Layer; } }

		public List<Property> Properties
		{
			get
			{
				List<Property> properties = new List<Property>();
				foreach (Assistant.ObjectPropertyList.OPLEntry entry in m_AssistantItem.ObjPropList.Content)
				{
					Property property = new Property(entry);
					properties.Add(property);
				}
				return properties;
			}
		}

		public int Weight
		{
			get
			{
				List<Property> properties = Properties;
				foreach (Property property in properties)
				{
					int number = property.Number;
					string args = property.Args;
					if (number == 1072788)
					{
						return 1;       // Peso 1 se cliloc è 1072788
					}
					if (number == 1072789)
						try
						{
							return Convert.ToInt32(args);  // Ritorna valore peso
						}
						catch
						{
							return 1;  // errore di conversione torna peso  1
						}
				}
				return 0;  // item senza peso
			}
		}

		public int Durability
		{
			get
			{
				List<Property> properties = Properties;
				foreach (Property property in properties)
				{
					int number = property.Number;
					if (number == 1060639)
					{
						string Text = property.Args;
						int step = 0;
						string Durability = "";

						for (int i = 0; i <= Text.Length - 1; i++)
						{
							if (step == 0)
								if (Char.IsNumber(Text[i]))
								{
									Durability = Durability + Text[i];
									step = 1;
									i++;
								}
							if (step == 1)
								if (Char.IsNumber(Text[i]))
								{
									Durability = Durability + Text[i];
								}
								else
									step = 2;
						}

						try
						{
							return Convert.ToInt32(Durability);  // Ritorna valore Dur
						}
						catch
						{
							return 0;  // errore di conversione torna 0
						}
					}
				}
				return 0; // item senza Dur
			}
		}

		public int MaxDurability
		{
			get
			{
				List<Property> properties = Properties;
				foreach (Property property in properties)
				{
					int number = property.Number;
					if (number == 1060639)
					{
						string Text = property.Args;
						string TempMaxDurability = "";
						int step = 0;
						string MaxDurability = "";
						for (int y = Text.Length - 1; y != 0; y--)
						{
							if (step == 0)
								if (Char.IsNumber(Text[y]))
								{
									TempMaxDurability = TempMaxDurability + Text[y];
									step = 1;
									y--;
								}
							if (step == 1)
								if (Char.IsNumber(Text[y]))
								{
									TempMaxDurability = TempMaxDurability + Text[y];
								}
								else
									step = 2;
						}
						for (int i = TempMaxDurability.Length - 1; i > -1; i--)
						{
							MaxDurability += TempMaxDurability[i];
						}
						try
						{
							return Convert.ToInt32(MaxDurability);  // Ritorna valore maxdur
						}
						catch
						{
							return 0;  // errore di conversione torna 0
						}
					}
				}
				return 0; // item senza maxdur
			}
		}
	}

	public class Items
	{
		public static void WaitForContents(Item bag, int delay) // Delay in MS
		{
			if (!bag.Updated)
			{
				RazorEnhanced.Items.UseItem(bag);
				int subdelay = delay;

				if (bag.IsCorpse || bag.IsContainer)
					while (!bag.Updated)
					{
						Thread.Sleep(2);
						subdelay -= 2;
						if (subdelay <= 0)
							break;
					}
			}
		}

		public class Filter
		{
			public bool Enabled = false;
			public List<int> Serials = new List<int>();
			public List<int> Graphics = new List<int>();
			public string Name = "";
			public List<int> Hues = new List<int>();
			public double RangeMin = -1;
			public double RangeMax = -1;
			public bool Movable = true;
			public List<string> Layers = new List<string>();
			public int OnGround = -1;
			public int IsCorpse = -1;
			public int IsContainer = -1;

			public Filter()
			{
			}
		}

		public static List<Item> ApplyFilter(Filter filter)
		{
			List<Item> result = new List<Item>();

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

					if (filter.OnGround != -1)
					{
						assistantItems = assistantItems.Where((i) => i.OnGround == Convert.ToBoolean(filter.OnGround)).ToList();
					}

					if (filter.IsContainer != -1)
					{
						assistantItems = assistantItems.Where((i) => i.IsContainer == Convert.ToBoolean(filter.IsContainer)).ToList();
					}

					if (filter.IsCorpse != -1)
					{
						assistantItems = assistantItems.Where((i) => i.IsCorpse == Convert.ToBoolean(filter.IsCorpse)).ToList();
					}					
				}
			}

			foreach (Assistant.Item assistantItem in assistantItems)
			{
				RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
				result.Add(enhancedItem);
			}

			return result;
		}

		public static Item Select(List<Item> items, string selector)
		{
			Item result = null;

			if (items.Count > 0)
			{
				switch (selector)
				{
					case "Random":
						result = items[Utility.Random(items.Count)] as Item;
						break;

					case "Nearest":
						Item nearest = items[0] as Item;
						if (nearest != null)
						{
							double minDist = Misc.DistanceSqrt(Player.Position, nearest.Position);
							for (int i = 0; i < items.Count; i++)
							{
								Item item = items[i] as Item;
								if (item != null)
								{
									double dist = Misc.DistanceSqrt(Player.Position, item.Position);
									if (dist < minDist)
									{
										nearest = item;
										minDist = dist;
									}
								}
							}
							result = nearest;
						}
						break;

					case "Farthest":
						Item farthest = items[0] as Item;
						if (farthest != null)
						{
							double maxDist = Misc.DistanceSqrt(Player.Position, farthest.Position);
							for (int i = 0; i < items.Count; i++)
							{
								Item item = items[i] as Item;
								if (item != null)
								{
									double dist = Misc.DistanceSqrt(Player.Position, item.Position);
									if (dist > maxDist)
									{
										farthest = item;
										maxDist = dist;
									}
								}
							}
							result = farthest;
						}
						break;

					case "Less":
						Item least = items[0] as Item;
						if (least != null)
						{
							int minAmount = least.Amount;
							for (int i = 0; i < items.Count; i++)
							{
								Item item = items[i] as Item;
								if (item != null)
								{
									int amount = item.Amount;
									if (amount < minAmount)
									{
										least = item;
										minAmount = amount;
									}
								}
							}
							result = least;
						}
						break;

					case "Most":
						Item most = items[0] as Item;
						if (most != null)
						{
							int maxAmount = most.Amount;
							for (int i = 0; i < items.Count; i++)
							{
								Item item = items[i] as Item;
								if (item != null)
								{
									int amount = item.Amount;
									if (amount > maxAmount)
									{
										most = item;
										maxAmount = amount;
									}
								}
							}
							result = most;
						}
						break;

					case "Weakest":
						Item weakest = items[0] as Item;
						if (weakest != null)
						{
							int minDur = weakest.Durability;
							for (int i = 0; i < items.Count; i++)
							{
								Item item = items[i] as Item;
								if (item != null)
								{
									int dur = item.Durability;
									if (dur < minDur)
									{
										weakest = item;
										minDur = dur;
									}
								}
							}
							result = weakest;
						}
						break;

					case "Strongest":
						Item strongest = items[0] as Item;
						if (strongest != null)
						{
							int maxDur = strongest.Amount;
							for (int i = 0; i < items.Count; i++)
							{
								Item item = items[i] as Item;
								if (item != null)
								{
									int dur = item.Durability;
									if (dur > maxDur)
									{
										strongest = item;
										maxDur = dur;
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

		public static void Lift(Item item, int amount)
		{
			if (item == null)
			{
				Misc.SendMessage("Script Error: Move: Source Item  not found");
				return;
			}
			if (amount == 0)
			{
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, amount));
			}
		}

		public static void DropFromHand(Item item, Item bag)
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
			Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
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

		public static void Move(int itemserial, int bagserial, int amount)
		{
			Assistant.Item bag = Assistant.World.FindItem(bagserial);
			Assistant.Item item = Assistant.World.FindItem(itemserial);
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

		public static void UseItem(int itemserial)
		{
			Assistant.Item item = Assistant.World.FindItem(itemserial);
			if (item == null)
			{
				Misc.SendMessage("Script Error: UseItem: Invalid Serial");
				return;
			}

			if (item.Serial.IsItem)
				Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
			else
				Misc.SendMessage("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
		}

		public static bool UseItemByID(int itemid, int color)
		{
			// Genero filtro item
			Items.Filter itemFilter = new Items.Filter();
			itemFilter.Enabled = true;
			itemFilter.Graphics.Add(itemid);

			if (color != -1)
				itemFilter.Hues.Add(color);

			List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

			foreach (Item found in containeritem)
			{
				if (!found.IsInBank && found.RootContainer == World.Player)
				{
					RazorEnhanced.Items.UseItem(found);
					return true;
				}
			}

			return false;
		}

		// Single Click
		public static void SingleClick(Item item)
		{
			ClientCommunication.SendToServer(new SingleClick(item));
		}

		public static void SingleClick(int itemserial)
		{
			Assistant.Item item = Assistant.World.FindItem(itemserial);
			if (item == null)
			{
				Misc.SendMessage("Script Error: SingleClick: Invalid Serial");
				return;
			}
			ClientCommunication.SendToServer(new SingleClick(item));
		}

		// Props
		public static int GetPropValue(int serial, string name)
		{
			Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			List<Assistant.ObjectPropertyList.OPLEntry> props = assistantItem.ObjPropList.Content;

			foreach (Assistant.ObjectPropertyList.OPLEntry prop in props)
			{
				if (prop.ToString().ToLower().Contains(name.ToLower()))
				{
					if (prop.Args == null)  // Props esiste ma non ha valore
						return 1;

					string propstring = prop.Args;
					bool subprops = false;
					int i = 0;

					if (propstring.Length > 7)
						subprops = true;

					try  // Etraggo il valore
					{
						string number = string.Empty;
						foreach (char str in propstring)
						{
							if (subprops)
							{
								if (i > 7)
									if (char.IsDigit(str))
										number += str.ToString();
							}
							else
							{
								if (char.IsDigit(str))
									number += str.ToString();
							}

							i++;
						}
						return (Convert.ToInt32(number));
					}
					catch
					{
						return 1;  // errore di conversione ma esiste
					}
				}
			}
			return 0;  // Non esiste
		}

		public static int GetPropValue(Item item, string name)
		{
			return GetPropValue(item.Serial, name);
		}

		// Message

		public static void Message(Item item, int hue, string message)
		{
			Assistant.ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
		}

		public static void Message(int serial, int hue, string message)
		{
			Item item = FindBySerial(serial);
			Assistant.ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
		}

		// Count
		public static int ContainerCount(int serial, int itemid, int color)
		{
			Item container = FindBySerial(serial);
			if (container != null)
				return ContainerCount(container, itemid, color);
			else
			{
				Misc.SendMessage("Script Error: ContainerCount: Invalid container");
				return 0;
			}
		}

		public static int ContainerCount(Item container, int itemid, int color)
		{
			int count = 0;
			if (container.IsContainer && container != null)
			{
				foreach (RazorEnhanced.Item itemcontenuti in container.Contains)
				{
					if (color == -1)
					{
						if (itemcontenuti.ItemID == itemid)
							count = count + itemcontenuti.Amount;
					}
					else
					{
						if (itemcontenuti.ItemID == itemid && itemcontenuti.Hue == color)
							count = count + itemcontenuti.Amount;
					}
				}
			}
			else
				Misc.SendMessage("Script Error: ContainerCount: Invalid container");

			return count;
		}

		public static int BackpackCount(int itemid, int color)
		{
			// Genero filtro item
			Items.Filter itemFilter = new Items.Filter();
			itemFilter.Enabled = true;
			itemFilter.Graphics.Add(itemid);

			if (color != -1)
				itemFilter.Hues.Add(color);

			List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

			int amount = 0;
			foreach (Item found in containeritem)
			{
				if (!found.IsInBank && found.RootContainer == World.Player)
					amount = amount + found.Amount;
			}

			return amount;
		}
	}
}