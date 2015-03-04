using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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

		public ArrayList Contains
		{
			get
			{
				ArrayList items = new ArrayList();
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

		public ArrayList Properties
		{
			get
			{
				ArrayList properties = new ArrayList();
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
				ArrayList properties = Properties;
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
				ArrayList properties = Properties;
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
				ArrayList properties = Properties;
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

		public static void WaitForContents(Item bag, int delay) // Delay in MS
		{
			int subdelay = delay;
			if (bag.IsCorpse || bag.IsContainer)
				while (!bag.Updated || subdelay < 0)
				{
					Thread.Sleep(2);
					subdelay -= 2;
				}
		}
	}
}
