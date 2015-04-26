using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Assistant
{
	internal abstract class Agent
	{
		private static List<Agent> m_List = new List<Agent>();
		internal static List<Agent> List { get { return m_List; } }

		internal delegate void ItemCreatedEventHandler(Item item);
		internal delegate void MobileCreatedEventHandler(Mobile m);
		internal static event ItemCreatedEventHandler OnItemCreated;
		internal static event MobileCreatedEventHandler OnMobileCreated;

		internal static void InvokeMobileCreated(Mobile m)
		{
			if (OnMobileCreated != null)
				OnMobileCreated(m);
		}

		internal static void InvokeItemCreated(Item i)
		{
			if (OnItemCreated != null)
				OnItemCreated(i);
		}

		internal static void Add(Agent a)
		{
			m_List.Add(a);
		}

		internal static void ClearAll()
		{
			foreach (Agent agent in m_List)
				agent.Clear();
		}

		public override string ToString()
		{
			return Name;
		}

		internal abstract string Name { get; }
		internal abstract void Save(XmlTextWriter xml);
	    internal abstract void Load(XmlElement node);
		internal abstract void Clear();
		internal abstract void OnSelected(ListBox subList, params Button[] buttons);
		internal abstract void OnButtonPress(int num);
	}

	internal class SearchExemptionAgent : Agent
	{
		private static SearchExemptionAgent m_Instance;
		internal static int Count { get { return m_Instance.m_Items.Count; } }
		internal static SearchExemptionAgent Instance { get { return m_Instance; } }

		internal static void Initialize()
		{
			Agent.Add(m_Instance = new SearchExemptionAgent());
		}

		internal static bool IsExempt(Item item)
		{
			if (item == null || item.IsBagOfSending)
				return true;
			else
				return m_Instance == null ? false : m_Instance.CheckExempt(item);
		}

		internal static bool Contains(Item item)
		{
			return m_Instance == null ? false : m_Instance.m_Items.Contains(item.Serial) || m_Instance.m_Items.Contains(item.ItemID);
		}

		private ListBox m_SubList;
		private List<object> m_Items;

		internal SearchExemptionAgent()
		{
			m_Items = new List<object>();
		}

		internal override void Clear()
		{
			m_Items.Clear();
		}

		private bool CheckExempt(Item item)
		{
			if (m_Items.Count > 0)
			{
				if (m_Items.Contains(item.Serial))
					return true;
				else if (m_Items.Contains(item.ItemID))
					return true;
				else if (item.Container != null && item.Container is Item)
					return CheckExempt((Item)item.Container);
			}
			return false;
		}

		internal override string Name { get { return Language.GetString(LocString.AutoSearchEx); } }
		internal override void OnSelected(ListBox subList, params Button[] buttons)
		{
			m_SubList = subList;

			buttons[0].Text = Language.GetString(LocString.AddTarg);
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString(LocString.AddTargType);
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString(LocString.Remove);
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString(LocString.RemoveTarg);
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString(LocString.ClearList);
			buttons[4].Visible = true;

			m_SubList.BeginUpdate();
			m_SubList.Items.Clear();

			for (int i = 0; i < m_Items.Count; i++)
			{
				Item item = null;
				if (m_Items[i] is Serial)
					item = World.FindItem((Serial)m_Items[i]);
				if (item != null)
					m_SubList.Items.Add(item.ToString());
				else
					m_SubList.Items.Add(m_Items[i].ToString());
			}
			m_SubList.EndUpdate();
		}

		internal override void OnButtonPress(int num)
		{
			switch (num)
			{
				case 1:
					World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
					break;
				case 2:
					World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetType));
					break;
				case 3:
					if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count)
					{
						m_Items.RemoveAt(m_SubList.SelectedIndex);
						m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
					}
					break;
				case 4:
					World.Player.SendMessage(MsgLevel.Force, LocString.TargItemRem);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetRemove));
					break;
				case 5:
					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
			}
		}

		private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
		{
			Engine.MainWindow.ShowMe();
			if (!location && serial.IsItem)
			{
				m_Items.Add(serial);

				Item item = World.FindItem(serial);
				if (item != null)
				{
					ClientCommunication.SendToClient(new ContainerItem(item));
					m_SubList.Items.Add(item.ToString());
				}
				else
				{
					m_SubList.Items.Add(serial.ToString());
				}

				World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
			}
		}

		private void OnTargetType(bool location, Serial serial, Point3D loc, ushort gfx)
		{
			Engine.MainWindow.ShowMe();

			if (!serial.IsItem) return;

			m_Items.Add((ItemID)gfx);
			m_SubList.Items.Add(((ItemID)gfx).ToString());
			World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
		}

		private void OnTargetRemove(bool location, Serial serial, Point3D loc, ushort gfx)
		{
			Engine.MainWindow.ShowMe();
			if (!location && serial.IsItem)
			{
				for (int i = 0; i < m_Items.Count; i++)
				{
					if (m_Items[i] is Serial && (Serial)m_Items[i] == serial)
					{
						m_Items.RemoveAt(i);
						m_SubList.Items.RemoveAt(i);
						World.Player.SendMessage(MsgLevel.Force, LocString.ItemRemoved);

						Item item = World.FindItem(serial);
						if (item != null)
							ClientCommunication.SendToClient(new ContainerItem(item));
						return;
					}
				}

				World.Player.SendMessage(MsgLevel.Force, LocString.ItemNotFound);
			}
		}

		internal override void Save(XmlTextWriter xml)
		{
			foreach (object item in m_Items)
			{
				xml.WriteStartElement("item");
				if (item is Serial)
					xml.WriteAttributeString("serial", ((Serial)item).Value.ToString());
				else
					xml.WriteAttributeString("id", ((ItemID)item).Value.ToString());
				xml.WriteEndElement();
			}
		}

		internal override void Load(XmlElement node)
		{
			foreach (XmlElement el in node.GetElementsByTagName("item"))
			{
				try
				{
					string ser = el.GetAttribute("serial");
					string iid = el.GetAttribute("id");
					if (ser != null)
						m_Items.Add((Serial)Convert.ToUInt32(ser));
					else if (iid != null)
						m_Items.Add((ItemID)Convert.ToUInt16(iid));
				}
				catch
				{
				}
			}
		}
	}

	internal class ScavengerAgent : Agent
	{
		private static ScavengerAgent m_Instance = new ScavengerAgent();
		internal static ScavengerAgent Instance { get { return m_Instance; } }

		internal static bool Debug = false;

		internal static void Initialize()
		{
			Agent.Add(m_Instance);
		}

		private bool m_Enabled;
		private Serial m_Bag;
		private ListBox m_SubList;
		private Button m_EnButton;
		private List<ushort> m_ItemIDs;

		private List<Serial> m_Cache;
		private Item m_BagRef;

		internal ScavengerAgent()
		{
			m_ItemIDs = new List<ushort>();

		//	HotKey.Add(HKCategory.Agents, LocString.ClearScavCache, new HotKeyCallback(ClearCache));
			PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(OnSingleClick));

			Agent.OnItemCreated += new ItemCreatedEventHandler(CheckBagOPL);
		}

		private void CheckBagOPL(Item item)
		{
			if (item.Serial == m_Bag)
				item.ObjPropList.Add(Language.GetString(LocString.ScavengerHB));
		}

		private void OnSingleClick(PacketReader pvSrc, PacketHandlerEventArgs args)
		{
			Serial serial = pvSrc.ReadUInt32();
			if (m_Bag == serial)
			{
				ushort gfx = 0;
				Item c = World.FindItem(m_Bag);
				if (c != null)
					gfx = c.ItemID.Value;
				ClientCommunication.SendToClient(new UnicodeMessage(m_Bag, gfx, Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "", Language.GetString(LocString.ScavengerHB)));
			}
		}

		internal override void Clear()
		{
			m_ItemIDs.Clear();
			m_BagRef = null;
		}

		internal bool Enabled { get { return m_Enabled; } }
		internal override string Name { get { return Language.GetString(LocString.Scavenger); } }
		internal override void OnSelected(ListBox subList, params Button[] buttons)
		{
			buttons[0].Text = Language.GetString(LocString.AddTarg);
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString(LocString.Remove);
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString(LocString.SetHB);
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString(LocString.ClearList);
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString(LocString.ClearScavCache);
			buttons[4].Visible = true;
			m_EnButton = buttons[5];
			m_EnButton.Visible = true;
			UpdateEnableButton();

			m_SubList = subList;
			subList.BeginUpdate();
			subList.Items.Clear();

			for (int i = 0; i < m_ItemIDs.Count; i++)
				subList.Items.Add(m_ItemIDs[i]);
			subList.EndUpdate();
		}

		private void UpdateEnableButton()
		{
			m_EnButton.Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
		}

		internal override void OnButtonPress(int num)
		{
			DebugLog("User pressed button {0}", num);
			switch (num)
			{
				case 1:
					World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
					break;
				case 2:
					if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_ItemIDs.Count)
					{
						m_ItemIDs.RemoveAt(m_SubList.SelectedIndex);
						m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
					}
					break;
				case 3:
					World.Player.SendMessage(LocString.TargCont);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
					break;
				case 4:
					m_SubList.Items.Clear();
					m_ItemIDs.Clear();
					break;
				case 5:
					ClearCache();
					break;
				case 6:
					m_Enabled = !m_Enabled;
					UpdateEnableButton();
					break;
			}
		}

		private void ClearCache()
		{
			DebugLog("Clearing Cache of {0} items", m_Cache == null ? -1 : m_Cache.Count);
			if (m_Cache != null)
				m_Cache.Clear();
			if (World.Player != null)
				World.Player.SendMessage(MsgLevel.Force, "Scavenger agent item cache cleared.");
		}

		private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
		{
			Engine.MainWindow.ShowMe();

			if (location || !serial.IsItem)
				return;

			Item item = World.FindItem(serial);
			if (item == null)
				return;

			m_ItemIDs.Add(item.ItemID);
			m_SubList.Items.Add(item.ItemID);

			DebugLog("Added item {0}", item);

			World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
		}

		private void OnTargetBag(bool location, Serial serial, Point3D loc, ushort gfx)
		{
			Engine.MainWindow.ShowMe();

			if (location || !serial.IsItem)
				return;

			if (m_BagRef == null)
				m_BagRef = World.FindItem(m_Bag);
			if (m_BagRef != null)
			{
				m_BagRef.ObjPropList.Remove(Language.GetString(LocString.ScavengerHB));
				m_BagRef.OPLChanged();
			}

			DebugLog("Set bag to {0}", serial);
			m_Bag = serial;
			m_BagRef = World.FindItem(m_Bag);
			if (m_BagRef != null)
			{
				m_BagRef.ObjPropList.Add(Language.GetString(LocString.ScavengerHB));
				m_BagRef.OPLChanged();
			}

			World.Player.SendMessage(MsgLevel.Force, LocString.ContSet, m_Bag);
		}

		internal void Uncache(Serial s)
		{
			if (m_Cache != null)
				m_Cache.Remove(s);
		}

		internal void Scavenge(Item item)
		{
			DebugLog("Checking WorldItem {0} ...", item);
			if (!m_Enabled || !m_ItemIDs.Contains(item.ItemID) || World.Player.Backpack == null || World.Player.IsGhost || World.Player.Weight >= World.Player.MaxWeight)
			{
				DebugLog("... skipped.");
				return;
			}

			if (m_Cache == null)
				m_Cache = new List<Serial>(200);
			else if (m_Cache.Count >= 190)
				m_Cache.RemoveRange(0, 50);

			if (m_Cache.Contains(item.Serial))
			{
				DebugLog("Item was cached.");
				return;
			}

			Item bag = m_BagRef;
			if (bag == null || bag.Deleted)
				bag = m_BagRef = World.FindItem(m_Bag);
			if (bag == null || bag.Deleted || !bag.IsChildOf(World.Player.Backpack))
				bag = World.Player.Backpack;

			m_Cache.Add(item.Serial);
			DragDropManager.DragDrop(item, bag);
			DebugLog("Dragging to {0}!", bag);
		}

		private static void DebugLog(string str, params object[] args)
		{
			if (Debug)
			{
				using (System.IO.StreamWriter w = new System.IO.StreamWriter("Scavenger.log", true))
				{
					w.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
					w.Write(":: ");
					w.WriteLine(str, args);
					w.Flush();
				}
			}
		}

		internal override void Save(XmlTextWriter xml)
		{
			xml.WriteAttributeString("enabled", m_Enabled.ToString());

			if (m_Bag != Serial.Zero)
			{
				xml.WriteStartElement("bag");
				xml.WriteAttributeString("serial", m_Bag.ToString());
				xml.WriteEndElement();
			}

			for (int i = 0; i < m_ItemIDs.Count; i++)
			{
				xml.WriteStartElement("item");
				xml.WriteAttributeString("id", ((ItemID)m_ItemIDs[i]).Value.ToString());
				xml.WriteEndElement();
			}
		}

		internal override void Load(XmlElement node)
		{
			try
			{
				m_Enabled = Boolean.Parse(node.GetAttribute("enabled"));
			}
			catch
			{
				m_Enabled = false;
			}

			try
			{
				m_Bag = Serial.Parse(node["bag"].GetAttribute("serial"));
			}
			catch
			{
				m_Bag = Serial.Zero;
			}

			foreach (XmlElement el in node.GetElementsByTagName("item"))
			{
				try
				{
					string iid = el.GetAttribute("id");
					m_ItemIDs.Add((ItemID)Convert.ToUInt16(iid));
				}
				catch
				{
				}
			}
		}
	}

}

