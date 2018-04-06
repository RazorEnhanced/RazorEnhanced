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

		public object Container { get { return m_AssistantItem.Container; } }

		public object RootContainer { get { return m_AssistantItem.RootContainer; } }

		public bool PropsUpdated { get { return m_AssistantItem.PropsUpdated; } }

		public bool ContainerUpdated { get { return m_AssistantItem.Updated; } }

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
				List<Assistant.Item> itempresent = new List<Assistant.Item>(m_AssistantItem.Contains);
				foreach (Assistant.Item assistantItem in itempresent)
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
			public bool Enabled = false;
			public List<int> Serials = new List<int>();
			public List<int> Graphics = new List<int>();
			public string Name = String.Empty;
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
			List<Assistant.Item> assistantItems = new List<Assistant.Item>(World.Items.Values.ToList());

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
						if (World.Player == null)
							return result;

						assistantItems = assistantItems.Where((i) =>
							Utility.DistanceSqrt
							(new Assistant.Point2D(World.Player.Position.X, World.Player.Position.Y), new Assistant.Point2D(i.Position.X, i.Position.Y)) >= filter.RangeMin
						).ToList();
					}

					if (filter.RangeMax != -1)
					{
						if (World.Player == null)
							return result;

						assistantItems = assistantItems.Where((i) =>
							Utility.DistanceSqrt
							(new Assistant.Point2D(World.Player.Position.X, World.Player.Position.Y), new Assistant.Point2D(i.Position.X, i.Position.Y)) <= filter.RangeMax
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
						int maxDur = strongest.Amount;
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
				Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, amount));
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
			Assistant.ClientCommunication.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
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

		public static void Move(int source, int destination, int amount, int x, int y)
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
			}

			if (serialdestination == 0)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Destination not found");
				return;
			}

			Assistant.Point3D loc = Assistant.Point3D.MinusOne;
			if (x != -1 && y != -1)
				loc = new Assistant.Point3D(x, y, 0);

			if (amount == 0)
			{
				DragDropManager.HoldingItem = true;
				Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
				Thread.Sleep(80);
				Assistant.ClientCommunication.SendToServerWait(new DropRequest(item.Serial, loc, serialdestination));
				DragDropManager.HoldingItem = false;
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				DragDropManager.HoldingItem = true;
				Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, amount));
				Thread.Sleep(80);
				Assistant.ClientCommunication.SendToServerWait(new DropRequest(item.Serial, loc, serialdestination));
				DragDropManager.HoldingItem = false;
			}
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

			Assistant.Point3D loc = Assistant.Point3D.MinusOne;
			if (x != -1 && y != -1)
				loc = new Assistant.Point3D(x, y, z);

			int amounttodrop = amount;
			if ((item.Amount < amount) || (amount == 0))
                amounttodrop = item.Amount;

			DragDropManager.HoldingItem = true;
			Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, amounttodrop));
			Thread.Sleep(80);
			Assistant.ClientCommunication.SendToServerWait(new DropRequest(item.Serial, loc, Assistant.Serial.MinusOne));
			DragDropManager.HoldingItem = false;
		}

		public static void DropItemGroundSelf(Item item, int amount)
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

		public static void DropItemGroundSelf(int serialitem, int amount)
		{
			Item i = FindBySerial(serialitem);
			DropItemGroundSelf(i, amount);
        }

		// Use item
		public static void UseItemOnMobile(Item i, Mobile m)
		{
			Assistant.ClientCommunication.SendToServerWait(new UseItemOnTarget(i.Serial, m.Serial));
		}

		public static void UseItemOnMobile(int itemserial, Mobile m)
		{
			Assistant.ClientCommunication.SendToServerWait(new UseItemOnTarget(itemserial, m.Serial));
		}

		public static void UseItemOnMobile(Item i, int targetserial)
		{
			Assistant.ClientCommunication.SendToServerWait(new UseItemOnTarget(i.Serial, targetserial));
		}

		public static void UseItemOnMobile(int itemserial, int targetserial, bool wait = true)
		{
			if (wait)
				ClientCommunication.SendToServerWait(new UseItemOnTarget(itemserial, targetserial));
			else
				ClientCommunication.SendToServer(new UseItemOnTarget(itemserial, targetserial));
		}


		public static void UseItem(Item item)
		{
			if (item != null)
				UseItem(item.Serial);
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
				Assistant.ClientCommunication.SendToServerWait(new DoubleClick(item.Serial));
			}
			else
			{
				Scripts.SendMessageScriptError("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
			}
		}

		public static bool UseItemByID(int itemid, int color)
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
			ClientCommunication.SendToServerWait(new SingleClick(item));
		}

		public static void SingleClick(int itemserial)
		{
			Assistant.Item item = Assistant.World.FindItem(itemserial);
			if (item != null)
			{
				Scripts.SendMessageScriptError("Script Error: SingleClick: Invalid Serial");
				return;
			}
			ClientCommunication.SendToServerWait(new SingleClick(item));
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

			ClientCommunication.SendToServerWait(new QueryProperties(i.Serial));
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

		public static int GetPropValue(int serial, string name)
		{
			Assistant.Item assistantItem = World.FindItem((uint)serial);

			if (assistantItem != null)
			{
				List<ObjectPropertyList.OPLEntry> props = new List<ObjectPropertyList.OPLEntry>(assistantItem.ObjPropList.Content);

				foreach (ObjectPropertyList.OPLEntry prop in props)
				{
					if (!prop.ToString().ToLower().Contains(name.ToLower()))
						continue;


					if (prop.Args == null)  // Props esiste ma non ha valore
						return 1;

					try
					{
						return (Convert.ToInt32(Language.ParsePropsCliloc(prop.Args)));
					}
					catch
					{
						return 1;  // errore di conversione ma esiste
					}
				}
			}
			return 0;  // Non esiste o null item
		}

		public static int GetPropValue(Item item, string name)
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

			ClientCommunication.SendToClientWait(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
		}

		public static void Message(int serial, int hue, string message)
		{
			Item item = FindBySerial(serial);

			if (item == null) //Intem 
				return;

			// Prevent spamm message on left bottom screen
			if (World.Player == null || Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) > 11)
				return;

			ClientCommunication.SendToClientWait(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
		}

		// Count
		public static int ContainerCount(int serial, int itemid, int color)
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

		public static int ContainerCount(Item container, int itemid, int color)
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
				ClientCommunication.SendToClientWait(new RemoveObject(serial));
			}
		}

        public static int BackpackCount(int itemid, int color)
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