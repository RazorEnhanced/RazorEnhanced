using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Assistant;

namespace RazorEnhanced
{
	internal class Settings
	{
		private static string m_Save = "RazorEnhanced.settings";
		private static DataSet m_Dataset;
		internal static DataSet Dataset { get { return m_Dataset; } }

		internal static void Load()
		{
			if (m_Dataset != null)
				return;

			m_Dataset = new DataSet();
			string filename = Path.Combine(Directory.GetCurrentDirectory(), m_Save);

			if (File.Exists(filename))
			{
				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Open(filename, FileMode.Open);
					GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
					BinaryFormatter bin = new BinaryFormatter();
					m_Dataset = bin.Deserialize(decompress) as DataSet;
					decompress.Close();
					decompress.Dispose();
					stream.Close();
					stream.Dispose();

					foreach (DataRow row in m_Dataset.Tables["SCRIPTING"].Rows)
					{
						row["Checked"] = false;
						row["Flag"] = Assistant.Properties.Resources.yellow;
						row["Status"] = "Idle";
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error loading " + m_Save + ": " + ex);
				}
			}
			else
			{
				// Scripting
				DataTable scripting = new DataTable("SCRIPTING");
				scripting.Columns.Add("Checked", typeof(bool));
				scripting.Columns.Add("Filename", typeof(string));
				scripting.Columns.Add("Flag", typeof(Bitmap));
				scripting.Columns.Add("Status", typeof(string));
				m_Dataset.Tables.Add(scripting);



				// -------- AUTOLOOT ------------
				DataTable autoloot_lists = new DataTable("AUTOLOOT_LISTS");
				autoloot_lists.Columns.Add("Description", typeof(string));
				autoloot_lists.Columns.Add("Delay", typeof(int));
				autoloot_lists.Columns.Add("Bag", typeof(int));
				autoloot_lists.Columns.Add("Selected", typeof(bool));
				m_Dataset.Tables.Add(autoloot_lists);

				DataTable autoloot_items = new DataTable("AUTOLOOT_ITEMS");
				autoloot_items.Columns.Add("List", typeof(string));
				autoloot_items.Columns.Add("Item", typeof(RazorEnhanced.AutoLoot.AutoLootItem));
				m_Dataset.Tables.Add(autoloot_items);



				// ----------- SCAVENGER ----------
				DataTable scavenger_lists = new DataTable("SCAVENGER_LISTS");
				scavenger_lists.Columns.Add("Description", typeof(string));
				scavenger_lists.Columns.Add("Delay", typeof(int));
				scavenger_lists.Columns.Add("Bag", typeof(int));
				scavenger_lists.Columns.Add("Selected", typeof(bool));
				m_Dataset.Tables.Add(scavenger_lists);

				DataTable scavenger_items = new DataTable("SCAVENGER_ITEMS");
				scavenger_items.Columns.Add("List", typeof(string));
				scavenger_items.Columns.Add("Item", typeof(RazorEnhanced.Scavenger.ScavengerItem));
				m_Dataset.Tables.Add(scavenger_items);



				//Organizer
				DataTable organizer_lists = new DataTable("ORGANIZER_LISTS");
				organizer_lists.Columns.Add("Name", typeof(string));
				organizer_lists.Columns.Add("Source", typeof(uint));
				organizer_lists.Columns.Add("Destination", typeof(uint));
				organizer_lists.Columns.Add("List", typeof(List<Organizer.OrganizerItem>));
				m_Dataset.Tables.Add(organizer_lists);

				DataTable organizer_general = new DataTable("ORGANIZER_GENERAL");
				organizer_general.Columns.Add("Delay", typeof(int));
				organizer_general.Columns.Add("List", typeof(List<string>));
				organizer_general.Columns.Add("Selection", typeof(string));
				m_Dataset.Tables.Add(organizer_general);

				//Sell
				DataTable sell_general = new DataTable("SELL_GENERAL");
				sell_general.Columns.Add("List", typeof(List<string>));
				sell_general.Columns.Add("Selection", typeof(string));
				m_Dataset.Tables.Add(sell_general);

				DataTable sell_lists = new DataTable("SELL_LISTS");
				sell_lists.Columns.Add("Name", typeof(string));
				sell_lists.Columns.Add("HotBag", typeof(uint));
				sell_lists.Columns.Add("List", typeof(List<SellAgent.SellItem>));
				m_Dataset.Tables.Add(sell_lists);

				//Buy
				DataTable buy_general = new DataTable("BUY_GENERAL");
				buy_general.Columns.Add("List", typeof(List<string>));
				buy_general.Columns.Add("Selection", typeof(string));
				m_Dataset.Tables.Add(buy_general);

				DataTable buy_lists = new DataTable("BUY_LISTS");
				buy_lists.Columns.Add("Name", typeof(string));
				buy_lists.Columns.Add("List", typeof(List<RazorEnhanced.BuyAgent.BuyItem>));
				m_Dataset.Tables.Add(buy_lists);

				m_Dataset.AcceptChanges();
			}
		}

		// ------------- AUTOLOOT -----------------
		internal class AutoLoot
		{
			internal static bool ListExists(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{
					if (((string)row["Description"]).ToLower() == description.ToLower())
						return true;
				}

				return false;
			}

			internal static void ListInsert(string description, int delay, uint bag)
			{
				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["AUTOLOOT_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Bag"] = bag;
				newRow["Selected"] = true;
				m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int bag, bool selected)
			{

				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{
					if ((string)row["Description"] == description)
					{
						found = true;
						break;
					}
				}

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Bag"] = bag;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ListDelete(string description)
			{
				for (int i = m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[i];
					if ((string)row["List"] == description)
					{
						row.Delete();
					}
				}

				for (int i = m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static void ListsRead(out List<RazorEnhanced.AutoLoot.AutoLootList> lists)
			{
				List<RazorEnhanced.AutoLoot.AutoLootList> listsOut = new List<RazorEnhanced.AutoLoot.AutoLootList>();

				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{

					string description = (string)row["Description"];
					int delay = (int)row["Delay"];
					int bag = (int)row["Bag"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.AutoLoot.AutoLootList list = new RazorEnhanced.AutoLoot.AutoLootList(description, delay, bag, selected);
					listsOut.Add(list);
				}

				lists = listsOut;
			}

			internal static bool ItemExists(string list, RazorEnhanced.AutoLoot.AutoLootItem item)
			{
				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows)
				{
					if ((string)row["List"] == list && (RazorEnhanced.AutoLoot.AutoLootItem)row["Item"] == item)
						return true;
				}

				return false;
			}

			internal static void ItemInsert(string list, RazorEnhanced.AutoLoot.AutoLootItem item)
			{
				DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Add(row);

				Save();
			}

			internal static void ItemReplace(string list, int index, RazorEnhanced.AutoLoot.AutoLootItem item)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows)
				{
					if ((string)row["List"] == list)
					{
						count++;
						if (count == index)
						{
							row["Item"] = item;
						}
					}
				}

				Save();
			}

			internal static void ItemDelete(string list, RazorEnhanced.AutoLoot.AutoLootItem item)
			{
				for (int i = m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.AutoLoot.AutoLootItem)row["Item"] == item)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void ItemsRead(string list, out List<RazorEnhanced.AutoLoot.AutoLootItem> items)
			{
				List<RazorEnhanced.AutoLoot.AutoLootItem> itemsOut = new List<RazorEnhanced.AutoLoot.AutoLootItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							itemsOut.Add((RazorEnhanced.AutoLoot.AutoLootItem)row["Item"]);
						}
					}
				}

				items = itemsOut;
			}
		}
		// ------------- AUTOLOOT END-----------------




		// ------------- SCAVENGER -----------------
		internal class Scavenger
		{
			internal static bool ListExists(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{
					if (((string)row["Description"]).ToLower() == description.ToLower())
						return true;
				}

				return false;
			}

			internal static void ListInsert(string description, int delay, uint bag)
			{
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["SCAVENGER_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Bag"] = bag;
				newRow["Selected"] = true;
				m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int bag, bool selected)
			{
				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{
					if ((string)row["Description"] == description)
					{
						found = true;
						break;
					}
				}

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Bag"] = bag;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ListDelete(string description)
			{
				for (int i = m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[i];
					if ((string)row["List"] == description)
					{
						row.Delete();
					}
				}

				for (int i = m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static void ListsRead(out List<RazorEnhanced.Scavenger.ScavengerList> lists)
			{
				List<RazorEnhanced.Scavenger.ScavengerList> listsOut = new List<RazorEnhanced.Scavenger.ScavengerList>();

				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{

					string description = (string)row["Description"];
					int delay = (int)row["Delay"];
					int bag = (int)row["Bag"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.Scavenger.ScavengerList list = new RazorEnhanced.Scavenger.ScavengerList(description, delay, bag, selected);
					listsOut.Add(list);
				}

				lists = listsOut;
			}

			internal static bool ItemExists(string list, RazorEnhanced.Scavenger.ScavengerItem item)
			{
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_ITEMS"].Rows)
				{
					if ((string)row["List"] == list && (RazorEnhanced.Scavenger.ScavengerItem)row["Item"] == item)
						return true;
				}

				return false;
			}

			internal static void ItemInsert(string list, RazorEnhanced.Scavenger.ScavengerItem item)
			{
				DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Add(row);

				Save();
			}

			internal static void ItemReplace(string list, int index, RazorEnhanced.Scavenger.ScavengerItem item)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_ITEMS"].Rows)
				{
					if ((string)row["List"] == list)
					{
						count++;
						if (count == index)
						{
							row["Item"] = item;
						}
					}
				}

				Save();
			}

			internal static void ItemDelete(string list, RazorEnhanced.Scavenger.ScavengerItem item)
			{
				for (int i = m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.Scavenger.ScavengerItem)row["Item"] == item)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void ItemsRead(string list, out List<RazorEnhanced.Scavenger.ScavengerItem> items)
			{
				List<RazorEnhanced.Scavenger.ScavengerItem> itemsOut = new List<RazorEnhanced.Scavenger.ScavengerItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["SCAVENGER_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							itemsOut.Add((RazorEnhanced.Scavenger.ScavengerItem)row["Item"]);
						}
					}
				}

				items = itemsOut;
			}
		}
		// ------------- SCAVENGER END-----------------






		//Organizer
		internal static bool LoadOrganizerItemList(string name, out List<Organizer.OrganizerItem> list, out uint source, out uint destination)
		{
			bool exit = false;
			uint sourceOut = 0;
			uint destinationOut = 0;
			List<Organizer.OrganizerItem> result = new List<Organizer.OrganizerItem>();

			foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
			{
				if ((string)row["Name"] == name)
				{
					sourceOut = (uint)row["Source"];
					destinationOut = (uint)row["Destination"];
					result = row["List"] as List<Organizer.OrganizerItem>;
					exit = true;
				}
			}
			source = sourceOut;
			destination = destinationOut;
			list = result;
			return exit;
		}

		internal static void SaveOrganizerItemList(string name, List<Organizer.OrganizerItem> list, uint source, uint destination)
		{
			m_Dataset.Tables["ORGANIZER_LISTS"].Clear();
			DataRow row = m_Dataset.Tables["ORGANIZER_LISTS"].NewRow();
			row["Name"] = name;
			row["List"] = list;
			row["Source"] = source;
			row["Destination"] = destination;
			m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Add(row);
			Save();
		}

		internal static bool LoadOrganizerGeneral(out int delay, out List<string> list, out string selection)
		{
			bool exit = false;

			int delayOut = 0;
			List<string> listOut = new List<string>();
			string selectionOut = "";

			if (m_Dataset.Tables["ORGANIZER_GENERAL"].Rows.Count == 1)
			{
				DataRow row = m_Dataset.Tables["ORGANIZER_GENERAL"].Rows[0];
				{
					delayOut = (int)row["Delay"];
					listOut = row["List"] as List<string>;
					selectionOut = (string)row["Selection"];
					exit = true;
				}
			}

			delay = delayOut;
			list = listOut;
			selection = selectionOut;

			return exit;
		}

		internal static void SaveOrganizerGeneral(int delay, List<string> list, string selection)
		{
			m_Dataset.Tables["ORGANIZER_GENERAL"].Rows.Clear();
			DataRow row = m_Dataset.Tables["ORGANIZER_GENERAL"].NewRow();
			row["Delay"] = delay;
			row["List"] = list;
			row["Selection"] = selection;
			m_Dataset.Tables["ORGANIZER_GENERAL"].Rows.Add(row);
			Save();
		}

		// sell agent
		internal static void SaveSellGeneral(List<string> list, string selection)
		{
			m_Dataset.Tables["SELL_GENERAL"].Rows.Clear();
			DataRow row = m_Dataset.Tables["SELL_GENERAL"].NewRow();
			row["List"] = list;
			row["Selection"] = selection;
			m_Dataset.Tables["SELL_GENERAL"].Rows.Add(row);
			Save();
		}

		internal static void SaveSellItemList(string name, List<SellAgent.SellItem> list, string hotbag)
		{
			m_Dataset.Tables["SELL_LISTS"].Rows.Clear();
			DataRow row = m_Dataset.Tables["SELL_LISTS"].NewRow();
			row["Name"] = name;
			row["HotBag"] = hotbag;
			row["List"] = list;
			m_Dataset.Tables["SELL_LISTS"].Rows.Add(row);
			Save();
		}

		internal static bool LoadSellGeneral(out List<string> list, out string selection)
		{
			bool exit = false;

			List<string> listOut = new List<string>();
			string selectionOut = "";

			if (m_Dataset.Tables["SELL_GENERAL"].Rows.Count == 1)
			{
				DataRow row = m_Dataset.Tables["SELL_GENERAL"].Rows[0];
				{
					listOut = row["List"] as List<string>;
					selectionOut = (string)row["Selection"];
					exit = true;
				}
			}
			list = listOut;
			selection = selectionOut;

			return exit;
		}

		internal static bool LoadSellItemList(string name, out List<SellAgent.SellItem> list, out uint hotBag)
		{
			bool exit = false;
			uint hotBagOut = 0;
			List<SellAgent.SellItem> result = new List<SellAgent.SellItem>();

			foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
			{
				if ((string)row["Name"] == name)
				{
					hotBagOut = (uint)row["HotBag"];
					result = row["List"] as List<SellAgent.SellItem>;
					exit = true;
				}
			}
			list = result;
			hotBag = hotBagOut;
			return exit;
		}

		// Buy Agent
		internal static void SaveBuyGeneral(List<string> list, string selection)
		{
			m_Dataset.Tables["BUY_GENERAL"].Rows.Clear();
			DataRow row = m_Dataset.Tables["BUY_GENERAL"].NewRow();
			row["List"] = list;
			row["Selection"] = selection;
			m_Dataset.Tables["BUY_GENERAL"].Rows.Add(row);
			Save();
		}

		internal static void SaveBuyItemList(string name, List<RazorEnhanced.BuyAgent.BuyItem> list)
		{
			m_Dataset.Tables["BUY_LISTS"].Rows.Clear();
			DataRow row = m_Dataset.Tables["BUY_LISTS"].NewRow();
			row["Name"] = name;
			row["List"] = list;
			m_Dataset.Tables["BUY_LISTS"].Rows.Add(row);
			Save();
		}

		internal static bool LoadBuyGeneral(out List<string> list, out string selection)
		{
			bool exit = false;

			List<string> listOut = new List<string>();
			string selectionOut = "";

			if (m_Dataset.Tables["BUY_GENERAL"].Rows.Count == 1)
			{
				DataRow row = m_Dataset.Tables["BUY_GENERAL"].Rows[0];
				{
					listOut = row["List"] as List<string>;
					selectionOut = (string)row["Selection"];
					exit = true;
				}
			}
			list = listOut;
			selection = selectionOut;

			return exit;
		}

		internal static bool LoadBuyItemList(string name, out List<RazorEnhanced.BuyAgent.BuyItem> list)
		{
			bool exit = false;
			List<RazorEnhanced.BuyAgent.BuyItem> result = new List<RazorEnhanced.BuyAgent.BuyItem>();

			foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
			{
				if ((string)row["Name"] == name)
				{
					result = row["List"] as List<RazorEnhanced.BuyAgent.BuyItem>;
					exit = true;
				}
			}
			list = result;
			return exit;
		}

		internal static void Save()
		{
			try
			{
				m_Dataset.AcceptChanges();

				string filename = Path.Combine(Directory.GetCurrentDirectory(), m_Save);

				m_Dataset.RemotingFormat = SerializationFormat.Binary;
				m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
				Stream stream = File.Create(filename);
				GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
				BinaryFormatter bin = new BinaryFormatter();
				bin.Serialize(compress, m_Dataset);
				compress.Close();
				compress.Dispose();
				stream.Close();
				stream.Dispose();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error writing " + m_Save + ": " + ex);
			}
		}
	}
}