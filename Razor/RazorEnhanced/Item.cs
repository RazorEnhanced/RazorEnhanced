using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

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

		public int ItemID {
			get {
				if (m_AssistantItem != null)
					return m_AssistantItem.ItemID.Value;
				else
					return 0;
				}
		}

		public int Amount { get { return m_AssistantItem.Amount; } }

		public string Direction { get { return m_AssistantItem.Direction.ToString(); } }

		public bool Visible { get { return m_AssistantItem.Visible; } }

		public bool Movable { get { return m_AssistantItem.Movable; } }

		public string Name { get { return m_AssistantItem.Name; } }

		public string Layer { get { return m_AssistantItem.Layer.ToString(); } }

		public int Container
		{
			get
			{
				if (m_AssistantItem.Container is Assistant.Item)
					return (m_AssistantItem.Container as Assistant.Item).Serial;
				else if (m_AssistantItem.Container is Assistant.Mobile)
					return (m_AssistantItem.Container as Assistant.Mobile).Serial;
				else
					return 0;
			}
		}

		public int RootContainer
		{
			get
			{
				if (m_AssistantItem.RootContainer is Assistant.Item)
					return (m_AssistantItem.RootContainer as Assistant.Item).Serial;
				else if (m_AssistantItem.RootContainer is Assistant.Mobile)
					return (m_AssistantItem.RootContainer as Assistant.Mobile).Serial;
				else
					return 0;
			}
		}

		public bool PropsUpdated { get { return m_AssistantItem.PropsUpdated; } }

		public bool ContainerUpdated { get { return m_AssistantItem.Updated; } }

		public bool IsChildOf(object parent)
		{
			return m_AssistantItem.IsChildOf(parent);
		}

		public int DistanceTo(Mobile m)
		{
			return Utility.Distance(Position.X, Position.Y, m.Position.X, m.Position.Y);
		}

		public List<Item> Contains
		{
			get
			{
				List<Item> items = new List<Item>();
				for (int i = 0; i < m_AssistantItem.Contains.Count; i++)
				{
					RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(m_AssistantItem.Contains[i]);
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
        public bool IsLootable { get { return m_AssistantItem.IsLootable; } }

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
					switch (number)
					{
						case 1072788:
							return 1;       // Peso 1 se cliloc è 1072788
						case 1072789:
							try
							{
								return Convert.ToInt32(args);  // Ritorna valore peso
							}
							catch
							{
								return 1;  // errore di conversione torna peso  1
							}
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
					if (number != 1060639)
						continue;

					string Text = property.Args;
					int step = 0;
					string Durability = String.Empty;

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
					if (number != 1060639)
						continue;

					string Text = property.Args;
					string TempMaxDurability = String.Empty;
					int step = 0;
					string MaxDurability = String.Empty;
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
				return 0; // item senza maxdur
			}
		}
	}

	public class Items
	{
		public static void WaitForContents(int serialbag, int delay) // Delay in MS
		{
			Item bag = FindBySerial(serialbag);
			if (bag != null)
				WaitForContents(bag, delay);
		}

		public static void WaitForContents(Item bag, int delay) // Delay in MS
		{
			if (!bag.IsCorpse && !bag.IsContainer)
				return;

			RazorEnhanced.Items.UseItem(bag);

			if (bag.Updated)
				return;

			int subdelay = delay;
			while (!bag.Updated)
			{
				Thread.Sleep(2);
				subdelay -= 2;
				if (subdelay <= 0)
					break;
			}
		}

		public class Filter
		{
			public bool Enabled = true;
			public List<int> Serials = new List<int>();
			public List<int> Graphics = new List<int>();
			public string Name = String.Empty;
			public List<int> Hues = new List<int>();
			public double RangeMin = -1;
			public double RangeMax = -1;
			public int Movable = -1;
			public bool CheckIgnoreObject = false;
			public List<string> Layers = new List<string>();
			public int OnGround = -1;
			public int IsCorpse = -1;
			public int IsContainer = -1;
            public int IsDoor = -1;

			public Filter()
			{
			}
		}

		public static List<Item> ApplyFilter(Filter filter)
		{
			List<Item> result = new List<Item>();
			List<Assistant.Item> assistantItems = new List<Assistant.Item>(World.Items.Values.ToList());

			try
			{
				if (filter.Enabled)
				{
					if (filter.Serials.Count > 0)
					{
						assistantItems = assistantItems.Where((i) => filter.Serials.Contains((int)i.Serial.Value)).ToList();
					}
					else
					{
						if (filter.Name != String.Empty)
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
								Utility.Distance(World.Player.Position.X, World.Player.Position.Y, i.Position.X, i.Position.Y) >= filter.RangeMin
							).ToList();
						}

						if (filter.RangeMax != -1)
						{
							assistantItems = assistantItems.Where((i) =>
								Utility.Distance(World.Player.Position.X, World.Player.Position.Y, i.Position.X, i.Position.Y) <= filter.RangeMax
							).ToList();
						}

                        if (filter.Movable >= 0)
                        {
                            assistantItems = assistantItems.Where((i) => i.Movable == (filter.Movable>0)).ToList();
                        }

						if (filter.Layers.Count > 0)
						{
							List<Assistant.Layer> list = new List<Assistant.Layer>();

							foreach (string text in filter.Layers)
							{
								Enum.TryParse<Layer>(text, out Layer l);
								if (l != Assistant.Layer.Invalid)
								{
									list.Add(l);
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

						if (filter.IsDoor != -1)
                        {
                            assistantItems = assistantItems.Where((i) => i.IsDoor == Convert.ToBoolean(filter.IsDoor)).ToList();
                        }

                        if (filter.CheckIgnoreObject)
						{
							assistantItems = assistantItems.Where((i) => Misc.CheckIgnoreObject(i.Serial) != true).ToList();
						}
					}
				}

			}
			catch { }

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

			if (items.Count <= 0)
				return null;

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
						foreach (Item t in items)
						{
							if (t == null)
								continue;

							double dist = Misc.DistanceSqrt(Player.Position, t.Position);

							if (!(dist < minDist))
								continue;

							nearest = t;
							minDist = dist;
						}
						result = nearest;
					}
					break;

				case "Farthest":
					Item farthest = items[0] as Item;
					if (farthest != null)
					{
						double maxDist = Misc.DistanceSqrt(Player.Position, farthest.Position);
						foreach (Item t in items)
						{
							if (t == null)
								continue;

							double dist = Misc.DistanceSqrt(Player.Position, t.Position);
							if (dist > maxDist)
							{
								farthest = t;
								maxDist = dist;
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
						foreach (Item t in items)
						{
							if (t == null)
								continue;

							int amount = t.Amount;
							if (amount < minAmount)
							{
								least = t;
								minAmount = amount;
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
						foreach (Item t in items)
						{
							if (t == null)
								continue;

							int amount = t.Amount;

							if (amount <= maxAmount)
								continue;

							most = t;
							maxAmount = amount;
						}
						result = most;
					}
					break;

				case "Weakest":
					Item weakest = items[0] as Item;
					if (weakest != null)
					{
						int minDur = weakest.Durability;
						foreach (Item t in items)
						{
							if (t == null)
								continue;

							int dur = t.Durability;

							if (dur >= minDur)
								continue;

							weakest = t;
							minDur = dur;
						}
						result = weakest;
					}
					break;

				case "Strongest":
					Item strongest = items[0] as Item;
					if (strongest != null)
					{
						int maxDur = strongest.Durability;
						foreach (Item t in items)
						{
							if (t == null)
								continue;

							int dur = t.Durability;

							if (dur <= maxDur)
								continue;
							strongest = t;
							maxDur = dur;
						}
						result = strongest;
					}
					break;
			}

			return result;
		}

		public static Item FindBySerial(int serial)
		{
			Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (assistantItem == null)
				return null;
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
				Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
				return;
			}
			if (amount == 0)
			{
				Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, amount));
			}
		}

		public static void DropFromHand(Item item, Item bag)
		{
			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
				return;
			}
			if (bag == null)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Destination Item not found");
				return;
			}
			if (!bag.IsContainer)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Destination Item is not a container");
				return;
			}
			Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
		}

		public static void Move(Item source, Mobile destination, int amount)
		{
			Move(source.Serial, destination.Serial, amount, -1, -1);
		}

		public static void Move(int source, Mobile destination, int amount)
		{
			Move(source, destination.Serial, amount, -1, -1);
		}

		public static void Move(Item source, int destination, int amount)
		{
			Move(source.Serial, destination, amount, -1, -1);
		}

		public static void Move(int source, Item destination, int amount)
		{
			Move(source, destination.Serial, amount, -1, -1);
		}

		public static void Move(Item source, Item destination, int amount)
		{
			Move(source.Serial, destination.Serial, amount, -1, -1);
        }

		public static void Move(int source, int destination, int amount)
		{
			Move(source, destination, amount, -1, -1);
		}

		public static void Move(Item source, Mobile destination, int amount, int x, int y)
		{
			Move(source.Serial, destination.Serial, amount, x, y);
		}

		public static void Move(int source, Mobile destination, int amount, int x, int y)
		{
			Move(source, destination.Serial, amount, x, y);
		}

		public static void Move(Item source, int destination, int amount, int x, int y)
		{
			Move(source.Serial, destination, amount, x, y);
		}

		public static void Move(int source, Item destination, int amount, int x, int y)
		{
			Move(source, destination.Serial, amount, x, y);
		}

		public static void Move(Item source, Item destination, int amount, int x, int y)
		{
			Move(source.Serial, destination.Serial, amount, x, y);
		}

		/*public static void Move(int source, int destination, int amount, int x, int y)
		{
			Assistant.Item bag = Assistant.World.FindItem(destination);
			Assistant.Item item = Assistant.World.FindItem(source);
			int serialdestination = 0;

			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
				return;
			}

			if (bag != null)
			{
				serialdestination = bag.Serial;
			}
			else
			{
				Assistant.Mobile mbag = Assistant.World.FindMobile(destination);
				if (mbag != null)
				{
					serialdestination = mbag.Serial;
				}
				else
				{
					Scripts.SendMessageScriptError("Script Error: Move: Destination not found");
					return;
				}
			}

			Assistant.Point3D loc = Assistant.Point3D.MinusOne;
			if (x != -1 && y != -1)
				loc = new Assistant.Point3D(x, y, 0);

			if (amount == 0)
			{
				Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
				Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, loc, serialdestination));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, amount));
				Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, loc, serialdestination));
			}
		}*/

		public static void Move(int source, int destination, int amount, int x, int y)
		{
			Assistant.Item bag = Assistant.World.FindItem(destination);
			Assistant.Item item = Assistant.World.FindItem(source);
			Assistant.Mobile mbag = null;

			int serialdestination = 0;
			bool isMobile = false;
			bool onLocation = false;
			int newamount = 0;

			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
				return;
			}

			if (bag != null)
			{
				serialdestination = bag.Serial;
			}
			else
			{
				mbag = Assistant.World.FindMobile(destination);
				if (mbag != null)
				{
					isMobile = true;
					serialdestination = mbag.Serial;
				}
			}

			if (serialdestination == 0)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Destination not found");
				return;
			}

			Assistant.Point3D loc = Assistant.Point3D.MinusOne;
			if (x != -1 && y != -1)
			{
				onLocation = true;
				loc = new Assistant.Point3D(x, y, 0);
			}

			// calcolo amount
			if (amount == 0)
			{
				newamount = item.Amount;
			}
			else
			{
				if (item.Amount < amount)
					newamount = item.Amount;
				else
					newamount = amount;
			}

			if (isMobile)
				Assistant.DragDropManager.DragDrop(item, newamount, mbag.Serial);
			else if (onLocation)
				Assistant.DragDropManager.DragDrop(item, newamount, bag, loc);
			else
				Assistant.DragDropManager.DragDrop(item, newamount, bag);
		}

		public static void MoveOnGround(Item source, int amount, int x, int y, int z)
		{
			MoveOnGround(source.Serial, amount, x, y, z);
        }

        public static void MoveOnGround(int source, int amount, int x, int y, int z)
		{
			Assistant.Item item = Assistant.World.FindItem(source);

			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: MoveOnGround: Source Item  not found");
				return;
			}

			Assistant.Point3D loc = new Assistant.Point3D(x, y, z);

			int amounttodrop = amount;
			if ((item.Amount < amount) || (amount == 0))
                amounttodrop = item.Amount;

			Assistant.DragDropManager.DragDrop(item, loc, amounttodrop);
			//Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, amounttodrop));
			//Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, loc, Assistant.Serial.MinusOne));
		}

		public static void DropItemGroundSelf(Item item, int amount = 0)
		{
			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: DropItemGroundSelf: Item not found");
				return;
			}

			int amounttodrop = amount;
			if ((item.Amount < amount) || (amount == 0))
                amounttodrop = item.Amount;

			MoveOnGround(item.Serial, amount, Player.Position.X, Player.Position.Y, Player.Position.Z);
		}

		public static void DropItemGroundSelf(int serialitem, int amount = 0)
		{
			Item i = FindBySerial(serialitem);
			DropItemGroundSelf(i, amount);
        }

		// Use item

		public static void UseItem(Item item)
		{
			if (item != null)
				UseItem(item.Serial);
		}

        public static void UseItem(Item item, EnhancedEntity target)
        {
            if (item == null || target == null)
                return;

            UseItem(item.Serial, target.Serial, true);
        }
        public static void UseItem(int item, EnhancedEntity target)
        {
            if (target == null)
                return;

            UseItem(item, target.Serial, true);
        }
        public static void UseItem(Item item, int target)
        {
            if (item == null)
                return;

            UseItem(item.Serial, target, true);
        }

        public static void UseItem(int itemSerial, int targetSerial)
        {
            UseItem(itemSerial, targetSerial, true);
        }

            public static void UseItem(int itemSerial, int targetSerial, bool wait)
        {
            Assistant.Item item = Assistant.World.FindItem(itemSerial);
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Use Serial");
                return;
            }

            Assistant.Item itemTarget = Assistant.World.FindItem(targetSerial);
            Assistant.Mobile mobileTarget = Assistant.World.FindMobile(targetSerial);
            if (itemTarget == null && mobileTarget == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Target Serial");
                return;
            }

            if (!item.Serial.IsItem)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + item.Serial.ToString() + ") is not an item");
                return;
            }
            if (itemTarget == null && !mobileTarget.Serial.IsMobile)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + targetSerial.ToString() + ") is not a mobile");
                return;
            }
            if (mobileTarget == null && !itemTarget.Serial.IsItem)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + targetSerial.ToString() + ") is not an item");
                return;
            }

            if (wait)
                Assistant.Client.Instance.SendToServerWait(new UseTargetedItem((uint)itemSerial, (uint)targetSerial));
            else
                Assistant.Client.Instance.SendToServer(new UseTargetedItem((uint)itemSerial, (uint)targetSerial));
        }

        public static void UseItem(int itemserial)
		{
			Assistant.Item item = Assistant.World.FindItem(itemserial);
			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Serial");
				return;
			}

			if (item.Serial.IsItem)
			{
				Assistant.Client.Instance.SendToServerWait(new DoubleClick(item.Serial));
			}
			else
			{
				Scripts.SendMessageScriptError("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
			}
		}

		public static bool UseItemByID(int itemid, int color = -1)
		{
			// Genero filtro item
			Items.Filter itemFilter = new Items.Filter
			{
				Enabled = true
			};
			itemFilter.Graphics.Add(itemid);

			if (color != -1)
				itemFilter.Hues.Add(color);

			List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

			foreach (Item found in containeritem)
			{
				if (!found.IsInBank && found.RootContainer == World.Player.Serial)
				{
					RazorEnhanced.Items.UseItem(found);
					return true;
				}
			}

			return false;
		}

		// Find item by id
		public static Item FindByID(int itemid, int color, int container)
		{
			if (container != -1)  // search in specific container
			{
				Item cont = FindBySerial(container);
				if (cont == null) // not valid serial or container not found
				{
					Scripts.SendMessageScriptError("Script Error: FindByID: Container serial not found");
					return null;
				}
				foreach (Item i in cont.Contains)
				{
					if (i.ItemID == itemid) // check item id
					{
						if (color != -1) // color -1 ALL
						{
							if (i.Hue == color)
								return i;
						}
						else
						{
							return i;
						}
					}
					else if (i.IsContainer)
					{
						FindByID(itemid, color, i.Serial); // recall for sub container
					}
				}
				return null; // Return null if no item found
			}
			else  // Search in world
			{
				Items.Filter itemFilter = new Items.Filter
				{
					Enabled = true
				};
				itemFilter.Graphics.Add(itemid);

				if (color != -1)
					itemFilter.Hues.Add(color);

				List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

				foreach (Item found in containeritem)  // Return frist one found
					return found;

				return null;
			}
		}

		// Single Click
		public static void SingleClick(Item item)
		{
	 		Assistant.Client.Instance.SendToServerWait(new SingleClick(item));
		}

		public static void SingleClick(int itemserial)
		{
			Assistant.Item item = Assistant.World.FindItem(itemserial);
			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: SingleClick: Invalid Serial");
				return;
			}
	 		Assistant.Client.Instance.SendToServerWait(new SingleClick(item));
		}

		// Props
		public static void WaitForProps(Item i, int delay)
		{
			WaitForProps(i.Serial, delay);
		}

		public static void WaitForProps(int itemserial, int delay) // Delay in MS
		{
			if (World.Player != null && World.Player.Expansion <= 3) //  Expansion <= 3. Non esistono le props
				return;

			Assistant.Item i = Assistant.World.FindItem(itemserial);

			if (i == null)
				return;

			if (i.PropsUpdated)
				return;

	 		Assistant.Client.Instance.SendToServerWait(new QueryProperties(i.Serial));
			int subdelay = delay;

			while (!i.PropsUpdated)
			{
				Thread.Sleep(2);
				subdelay -= 2;
				if (subdelay <= 0)
					break;
			}
		}

		public static List<string> GetPropStringList(int serial)
		{
			List<string> propstringlist = new List<string>();
			Assistant.Item assistantItem = Assistant.World.FindItem((uint)serial);

			if (assistantItem == null)
				return propstringlist;

			List<Assistant.ObjectPropertyList.OPLEntry> props = assistantItem.ObjPropList.Content;
			foreach (Assistant.ObjectPropertyList.OPLEntry prop in props)
			{
				propstringlist.Add(prop.ToString());
			}
			return propstringlist;
		}

		public static List<string> GetPropStringList(Item item)
		{
			return GetPropStringList(item.Serial);
        }

		public static string GetPropStringByIndex(int serial, int index)
		{
			string propstring = String.Empty;
			Assistant.Item assistantItem = Assistant.World.FindItem((uint)serial);

			if (assistantItem == null)
				return propstring;

			List<Assistant.ObjectPropertyList.OPLEntry> props = assistantItem.ObjPropList.Content;
			if (props.Count > index)
				propstring = props[index].ToString();
			return propstring;
		}

		public static string GetPropStringByIndex(Item item, int index)
		{
			return GetPropStringByIndex(item.Serial, index);
		}


		// Special case "Total Resist" so that items can be collected based on total resist
			public static float GetTotalResistProp(int serial)
		{
			Assistant.Item assistantItem = World.FindItem((uint)serial);

			float totalResist = 0;
            if (assistantItem != null)
            {
                try
                {
                    if (assistantItem != null && assistantItem.ObjPropList != null && assistantItem.ObjPropList.Content != null)
                    {
                        for (int i = 0; i < assistantItem.ObjPropList.Content.Count; i++)
                        {
                            if (assistantItem.ObjPropList.Content[i].ToString().ToLower().Contains("resist"))
                            {
                                if (assistantItem.ObjPropList.Content[i].Args != null)
                                {
                                    float addIt = 0;
                                    try
                                    {
                                        addIt = Convert.ToSingle(Language.ParsePropsCliloc(assistantItem.ObjPropList.Content[i].Args), CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        addIt = 1;  // Conversion error
                                    }
                                    totalResist += addIt;
                                }
                            }
                        }
                    }
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    // Do nothing. This occurs when looting or claiming a corpse while processing is still going on
                }
            }
			return totalResist;
		}

        public static float GetPropValue(int serial, string name)
        {
            if (name.ToLower().Contains("total") && name.ToLower().Contains("resist"))
                return GetTotalResistProp(serial);

            Assistant.Item assistantItem = World.FindItem((uint)serial);
            try
            {
                if (assistantItem != null && assistantItem.ObjPropList != null && assistantItem.ObjPropList.Content != null)
                {
                    var content = assistantItem.ObjPropList.Content;
                    if (content != null)
                    {
                        for (int i = 0; i < content.Count; i++)
                        {
                            if (!content[i].ToString().ToLower().Contains(name.ToLower())) // Props Name not match
                                continue;

                            if (content[i].Args == null)  // Props exist but not have value
                                return 1;

                            try
                            {
                                return Convert.ToSingle(Language.ParsePropsCliloc(content[i].Args), CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                return 1;  // Conversion error
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing because sometimes the content[i] no longer existed so fall through
                // and return 0
            }
            return 0;  // Item not exist or props not exist
        }

		public static float GetPropValue(Item item, string name)
		{
			if (item == null)
				return 0;

			return GetPropValue(item.Serial, name);
		}

		// Message

		public static void Message(Item item, int hue, string message)
		{
			// Prevent spamm message on left bottom screen
			if (World.Player == null || Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) > 11)
				return;

	 		Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
		}

		public static void Message(int serial, int hue, string message)
		{
			Item item = FindBySerial(serial);

			if (item == null) //Intem
				return;

			// Prevent spamm message on left bottom screen
			if (World.Player == null || Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) > 11)
				return;

	 		Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
		}

		// Count
		public static int ContainerCount(int serial, int itemid, int color = -1)
		{
			Item container = FindBySerial(serial);
			if (container != null)
				return ContainerCount(container, itemid, color);
			else
			{
				Scripts.SendMessageScriptError("Script Error: ContainerCount: Invalid container");
				return 0;
			}
		}

		public static int ContainerCount(Item container, int itemid, int color = -1)
		{
			int count = 0;
			if (container != null && container.IsContainer)
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
			{
				Scripts.SendMessageScriptError("Script Error: ContainerCount: Invalid container");
			}
			return count;
		}

		public static void Hide(Item item)
		{
			Hide(item.Serial);
		}

		public static void Hide(int serial)
		{
			Assistant.Item item = World.FindItem(serial);
			if (item != null)
			{
				item.Visible = false;
		 		Assistant.Client.Instance.SendToClientWait(new RemoveObject(serial));
			}
		}

        public static int BackpackCount(int itemid, int color = -1)
		{
			List<Assistant.Item> items = new List<Assistant.Item>(World.Items.Values.ToList());
			if (color == -1)
				items = items.Where((i) => i.RootContainer == World.Player && i.ItemID == itemid && i.IsInBank == false).ToList();
			else
				items = items.Where((i) => i.RootContainer == World.Player && i.ItemID == itemid && i.Hue == color && i.IsInBank == false).ToList();

			int amount = 0;
			foreach (Assistant.Item i in items)
					amount = amount + i.Amount;

			return amount;
		}

		// Context

		public static int ContextExist(Item i, string name)
		{
			return ContextExist(i.Serial, name);
		}

		public static int ContextExist(int serial, string name)
		{
			Assistant.Item item = World.FindItem(serial);
			if (item == null) // Se item non valido
				return -1;

			Misc.WaitForContext(serial, 1500);

			foreach (KeyValuePair<ushort, int> entry in item.ContextMenu)
			{
				string menuname = string.Empty;
				menuname = Language.GetCliloc(entry.Value);
				if (menuname.ToLower() == name.ToLower())
				{
					return entry.Key;
				}
			}

			return -1; // Se non trovata
		}
	}
}
