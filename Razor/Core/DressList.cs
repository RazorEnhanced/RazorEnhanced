using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;

namespace Assistant
{
	internal class DressList
	{
		private static List<DressList> m_List = new List<DressList>();

		internal static void Redraw(ListBox box)
		{
			box.Items.Clear();

			box.Items.AddRange(m_List.ToArray());
		}

		internal static void ClearAll()
		{
			while (m_List.Count > 0)
				Remove(m_List[0]);
		}

		internal static DressList Find(string name)
		{
			foreach (DressList dress in m_List)
			{
				if (dress.Name == name)
					return dress;
			}

			return null;
		}

		internal static Item FindUndressBag(Item item)
		{
			Item undressBag = World.Player.Backpack;

			foreach (DressList dress in m_List)
			{
				if (dress != null && (dress.Items.Contains(item.Serial) || dress.Items.Contains(item.ItemID)))
				{
					if (dress.m_UndressBag.IsValid)
					{
						Item bag = World.FindItem(dress.m_UndressBag);
						if (bag != null && (bag.RootContainer == World.Player || (bag.RootContainer == null && Utility.InRange(bag.GetWorldPosition(), World.Player.Position, 2))))
							undressBag = bag;
					}
					break;
				}
			}

			return undressBag;
		}

		internal static void Load(XmlElement xml)
		{
			ClearAll();

			if (xml == null)
				return;

			foreach (XmlElement el in xml.GetElementsByTagName("list"))
			{
				try
				{
					string name = el.GetAttribute("name");
					DressList list = new DressList(name);
					Add(list);

					try
					{
						list.m_UndressBag = Serial.Parse(el.GetAttribute("undressbag"));
					}
					catch
					{
						list.m_UndressBag = Serial.Zero;
					}

					foreach (XmlElement el2 in el.GetElementsByTagName("item"))
					{
						try
						{
							string ser = el2.GetAttribute("serial");
							list.Items.Add((Serial)Convert.ToUInt32(ser));
						}
						catch
						{
							try
							{
								string iid = el2.GetAttribute("id");
								list.Items.Add((ItemID)Convert.ToUInt16(iid));
							}
							catch
							{
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		internal static void Save(XmlTextWriter xml)
		{
			foreach (DressList list in m_List)
			{
				xml.WriteStartElement("list");
				xml.WriteAttributeString("name", list.Name);

				if (list.m_UndressBag.IsItem)
					xml.WriteAttributeString("undressbag", list.m_UndressBag.ToString());

				foreach (object o in list.Items)
				{
					xml.WriteStartElement("item");
					if (o is Serial)
						xml.WriteAttributeString("serial", ((Serial)o).Value.ToString());
					else if (o is ItemID)
						xml.WriteAttributeString("id", ((ItemID)o).Value.ToString());
					xml.WriteEndElement();
				}
				xml.WriteEndElement();
			}
		}

		internal static void Add(DressList list)
		{
			m_List.Add(list);
			HotKey.Add(HKCategory.Dress, HKSubCat.None, String.Format("Dress: {0}", list.Name), new HotKeyCallback(list.Dress));
			HotKey.Add(HKCategory.Dress, HKSubCat.None, String.Format("Undress: {0}", list.Name), new HotKeyCallback(list.Undress));
			HotKey.Add(HKCategory.Dress, HKSubCat.None, String.Format("Toggle: {0}", list.Name), new HotKeyCallback(list.Toggle));
		}

		internal static void Remove(DressList list)
		{
			m_List.Remove(list);
			HotKey.Remove(String.Format("Dress: {0}", list.Name));
			HotKey.Remove(String.Format("Undress: {0}", list.Name));
			HotKey.Remove(String.Format("Toggle: {0}", list.Name));
		}

		private List<object> m_Items;
		private Serial m_UndressBag;
		private string m_Name;

		internal DressList(string name)
		{
			m_Name = name;
			m_Items = new List<object>();
			m_UndressBag = Serial.Zero;
		}

		internal DressList(XmlElement xml)
		{
			m_Name = xml.GetAttribute("name");
		}

		public override string ToString()
		{
			return m_Name;
		}

		internal string Name { get { return m_Name; } }
		internal List<object> Items { get { return m_Items; } }

		internal void SetUndressBag(Serial serial)
		{
			if (serial == World.Player.Backpack.Serial || !serial.IsItem)
				m_UndressBag = Serial.Zero;
			else
				m_UndressBag = serial;
		}

		internal void Toggle()
		{
			if (World.Player == null)
				return;

			int worn = 0;
			int total = m_Items.Count;

			for (int i = 0; i < m_Items.Count; i++)
			{
				Item item = null;
				if (m_Items[i] is Serial)
					item = World.FindItem((Serial)m_Items[i]);
				else if (m_Items[i] is ItemID)
					item = World.Player.FindItemByID((ItemID)m_Items[i]);

				if (item == null)
					total--;
				else if (item.Container == World.Player)
					worn++;
			}

			if (m_Items.Count == 1)
			{
				if (worn != 0)
					Undress();
				else
					Dress();
			}
			else
			{
				if (worn > total / 2)
					Undress();
				else
					Dress();
			}
		}

		internal void Undress()
		{
			if (World.Player == null)
				return;

			int count = 0;
			Item undressBag = World.Player.Backpack;
			if (undressBag == null)
			{
				World.Player.SendMessage(LocString.NoBackpack);
				return;
			}

			if (Macros.MacroManager.AcceptActions)
				Macros.MacroManager.Action(new Macros.UnDressAction(m_Name));

			if (m_UndressBag.IsValid)
			{
				Item bag = World.FindItem(m_UndressBag);
				if (bag != null && (bag.RootContainer == World.Player || (bag.RootContainer == null && Utility.InRange(bag.GetWorldPosition(), World.Player.Position, 2))))
					undressBag = bag;
				else
					World.Player.SendMessage(LocString.UndressBagRange);
			}

			for (int i = 0; i < m_Items.Count; i++)
			{
				Item item = null;
				if (m_Items[i] is Serial)
					item = World.FindItem((Serial)m_Items[i]);
				else if (m_Items[i] is ItemID)
					item = World.Player.FindItemByID((ItemID)m_Items[i]);

				if (item == null || DragDropManager.CancelDragFor(item.Serial) || item.Container != World.Player)
				{
					continue;
				}
				else
				{
					DragDropManager.DragDrop(item, undressBag);
					count++;
				}
			}

			World.Player.SendMessage(LocString.UndressQueued, count);
		}

		internal static Layer GetLayerFor(Item item)
		{
			Layer layer = item.Layer;
			if (layer == Layer.Invalid || layer > Layer.LastUserValid)
				layer = (Layer)item.ItemID.ItemData.Quality;

			return layer;
		}

		internal void Dress()
		{
			if (World.Player == null)
				return;

			int skipped = 0, gone = 0, done = 0;
			List<Item> list = new List<Item>();
			bool remConflicts = Config.GetBool("UndressConflicts");

			if (World.Player.Backpack == null)
			{
				World.Player.SendMessage(LocString.NoBackpack);
				return;
			}

			if (Macros.MacroManager.AcceptActions)
				Macros.MacroManager.Action(new Macros.DressAction(m_Name));

			for (int i = 0; i < m_Items.Count; i++)
			{
				Item item = null;
				if (m_Items[i] is Serial)
				{
					item = World.FindItem((Serial)m_Items[i]);
					if (item == null)
						gone++;
					else
						list.Add(item);
				}
				else if (m_Items[i] is ItemID)
				{
					ItemID id = (ItemID)m_Items[i];

					// search to make sure they are not already wearing this...
					item = World.Player.FindItemByID(id);
					if (item != null)
					{
						skipped++;
					}
					else
					{
						item = World.Player.Backpack.FindItemByID(id);
						if (item == null)
							gone++;
						else
							list.Add(item);
					}
				}
			}

			foreach (Item item in list)
			{
				if (item.Container == World.Player)
				{
					skipped++;
				}
				else if (item.IsChildOf(World.Player.Backpack) || item.RootContainer == null)
				{
					Layer layer = GetLayerFor(item);
					if (layer == Layer.Invalid || layer > Layer.LastUserValid)
						continue;

					if (remConflicts)
					{
						Item conflict = World.Player.GetItemOnLayer(layer);
						if (conflict != null)
							DragDropManager.DragDrop(conflict, FindUndressBag(conflict));

						// try to also undress conflicting hand(s)
						if (layer == Layer.RightHand)
							conflict = World.Player.GetItemOnLayer(Layer.LeftHand);
						else if (layer == Layer.LeftHand)
							conflict = World.Player.GetItemOnLayer(Layer.RightHand);
						else
							conflict = null;

						if (conflict != null && (conflict.IsTwoHanded || item.IsTwoHanded))
							DragDropManager.DragDrop(conflict, FindUndressBag(conflict));
					}
					DragDropManager.DragDrop(item, World.Player, layer);
					done++;
				}
			}

			if (done > 0)
				World.Player.SendMessage(LocString.DressQueued, done);
			if (skipped > 0)
				World.Player.SendMessage(LocString.AlreadyDressed, skipped);
			if (gone > 0)
				World.Player.SendMessage(LocString.ItemsNotFound, gone);
		}
	}
}
