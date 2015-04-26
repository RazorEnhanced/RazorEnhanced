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



				// ----------- ORGANIZER ----------
				DataTable organizer_lists = new DataTable("ORGANIZER_LISTS");
				organizer_lists.Columns.Add("Description", typeof(string));
				organizer_lists.Columns.Add("Delay", typeof(int));
				organizer_lists.Columns.Add("Source", typeof(int));
				organizer_lists.Columns.Add("Destination", typeof(int));
				organizer_lists.Columns.Add("Selected", typeof(bool));
				m_Dataset.Tables.Add(organizer_lists);

				DataTable organizer_items = new DataTable("ORGANIZER_ITEMS");
				organizer_items.Columns.Add("List", typeof(string));
				organizer_items.Columns.Add("Item", typeof(RazorEnhanced.Organizer.OrganizerItem));
				m_Dataset.Tables.Add(organizer_items);



				// ----------- SELL AGENT ----------
				DataTable sell_lists = new DataTable("SELL_LISTS");
				sell_lists.Columns.Add("Description", typeof(string));
				sell_lists.Columns.Add("Bag", typeof(int));
				sell_lists.Columns.Add("Selected", typeof(bool));
				m_Dataset.Tables.Add(sell_lists);

				DataTable sell_items = new DataTable("SELL_ITEMS");
				sell_items.Columns.Add("List", typeof(string));
				sell_items.Columns.Add("Item", typeof(RazorEnhanced.SellAgent.SellAgentItem));
				m_Dataset.Tables.Add(sell_items);




				// ----------- BUY AGENT ----------
				DataTable buy_lists = new DataTable("BUY_LISTS");
				buy_lists.Columns.Add("Description", typeof(string));
				buy_lists.Columns.Add("Selected", typeof(bool));
				m_Dataset.Tables.Add(buy_lists);

				DataTable buy_items = new DataTable("BUY_ITEMS");
				buy_items.Columns.Add("List", typeof(string));
				buy_items.Columns.Add("Item", typeof(RazorEnhanced.BuyAgent.BuyAgentItem));
				m_Dataset.Tables.Add(buy_items);

                // ----------- DRESS ----------
                DataTable dress_lists = new DataTable("DRESS_LISTS");
                dress_lists.Columns.Add("Description", typeof(string));
                dress_lists.Columns.Add("Bag", typeof(int));
                dress_lists.Columns.Add("Delay", typeof(int));
                dress_lists.Columns.Add("Conflict", typeof(bool));
                dress_lists.Columns.Add("Selected", typeof(bool));
                m_Dataset.Tables.Add(dress_lists);

                DataTable dress_items = new DataTable("DRESS_ITEMS");
                dress_items.Columns.Add("List", typeof(string));
                dress_items.Columns.Add("Item", typeof(RazorEnhanced.Dress.DressItem));
                m_Dataset.Tables.Add(dress_items);

                // ----------- FRIEND ----------
                DataTable friend_lists = new DataTable("FRIEND_LISTS");
                friend_lists.Columns.Add("Description", typeof(string));
                friend_lists.Columns.Add("IncludeParty", typeof(bool));
                friend_lists.Columns.Add("PreventAttack", typeof(bool));
                friend_lists.Columns.Add("AutoacceptParty", typeof(bool));
                friend_lists.Columns.Add("Selected", typeof(bool));
                m_Dataset.Tables.Add(friend_lists);

                DataTable friend_player = new DataTable("FRIEND_PLAYERS");
                friend_player.Columns.Add("List", typeof(string));
                friend_player.Columns.Add("Player", typeof(RazorEnhanced.Friend.FriendPlayer));
                m_Dataset.Tables.Add(friend_player);

				// ----------- SHARDS ----------
				DataTable shards = new DataTable("SHARDS");
				shards.Columns.Add("Description", typeof(string)); // Key
				shards.Columns.Add("ClientPath", typeof(string));
				shards.Columns.Add("ClientFolder", typeof(string));
				shards.Columns.Add("Host", typeof(string));
				shards.Columns.Add("Port", typeof(int));
				shards.Columns.Add("PatchEnc", typeof(bool));
				shards.Columns.Add("OSIEnc", typeof(bool));
				shards.Columns.Add("Selected", typeof(bool));

				DataRow uod = shards.NewRow();
				uod.ItemArray = new object[] { "UODreams", "", "", "login.uodreams.com", 2593, true, false, true };
				shards.Rows.Add(uod);



				m_Dataset.Tables.Add(shards);




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

			internal static void ListInsert(string description, int delay, int bag)
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
            internal static void ListDetailsRead(string listname, out int bag, out int delay)
            {
                int bagOut = 0;
                int delayOut = 0;  
                foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
                {
                    if ((string)row["Description"] == listname)
                    {
                        bagOut = (int)row["Bag"];
                        delayOut = (int)row["Delay"];
                    }
                }
                bag = bagOut;
                delay = delayOut;
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

			internal static void ListInsert(string description, int delay, int bag)
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

            internal static void ListDetailsRead(string listname, out int bag, out int delay)
            {
                int bagOut = 0;
                int delayOut = 0;
                foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
                {
                    if ((string)row["Description"] == listname)
                    {
                        bagOut = (int)row["Bag"];
                        delayOut = (int)row["Delay"];
                    }
                }
                bag = bagOut;
                delay = delayOut;
            }
		}
		// ------------- SCAVENGER END-----------------




		// ------------- ORGANIZER -----------------

		internal class Organizer
		{
			internal static bool ListExists(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
				{
					if (((string)row["Description"]).ToLower() == description.ToLower())
						return true;
				}

				return false;
			}

			internal static void ListInsert(string description, int delay, int source, int destination)
			{
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["ORGANIZER_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Source"] = source;
				newRow["Destination"] = destination;
				newRow["Selected"] = true;
				m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int source, int destination, bool selected)
			{

				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
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
						foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Source"] = source;
							row["Destination"] = destination;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ListDelete(string description)
			{
				for (int i = m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[i];
					if ((string)row["List"] == description)
					{
						row.Delete();
					}
				}

				for (int i = m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static void ListsRead(out List<RazorEnhanced.Organizer.OrganizerList> lists)
			{
				List<RazorEnhanced.Organizer.OrganizerList> listsOut = new List<RazorEnhanced.Organizer.OrganizerList>();

				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
				{

					string description = (string)row["Description"];
					int delay = (int)row["Delay"];
					int source = (int)row["Source"];
					int destination = (int)row["Destination"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.Organizer.OrganizerList list = new RazorEnhanced.Organizer.OrganizerList(description, delay, source, destination, selected);
					listsOut.Add(list);
				}

				lists = listsOut;
			}

			internal static bool ItemExists(string list, RazorEnhanced.Organizer.OrganizerItem item)
			{
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_ITEMS"].Rows)
				{
					if ((string)row["List"] == list && (RazorEnhanced.Organizer.OrganizerItem)row["Item"] == item)
						return true;
				}

				return false;
			}

			internal static void ItemInsert(string list, RazorEnhanced.Organizer.OrganizerItem item)
			{
				DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Add(row);

				Save();
			}

			internal static void ItemReplace(string list, int index, RazorEnhanced.Organizer.OrganizerItem item)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_ITEMS"].Rows)
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

			internal static void ItemDelete(string list, RazorEnhanced.Organizer.OrganizerItem item)
			{
				for (int i = m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.Organizer.OrganizerItem)row["Item"] == item)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void ItemsRead(string list, out List<RazorEnhanced.Organizer.OrganizerItem> items)
			{
				List<RazorEnhanced.Organizer.OrganizerItem> itemsOut = new List<RazorEnhanced.Organizer.OrganizerItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["ORGANIZER_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							itemsOut.Add((RazorEnhanced.Organizer.OrganizerItem)row["Item"]);
						}
					}
				}

				items = itemsOut;
			}

            internal static void ListDetailsRead(string listname, out int bags, out int bagd, out int delay)
            {
                int bagsOut = 0;
                int bagdOut = 0;
                int delayOut = 0;
                foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
                {
                    if ((string)row["Description"] == listname)
                    {
                        bagsOut = (int)row["Source"];
                        bagdOut = (int)row["Destination"];
                        delayOut = (int)row["Delay"];
                    }
                }
                bags = bagsOut;
                bagd = bagdOut;
                delay = delayOut;
            }
		}
		// ------------- ORGANIZER END-----------------



		// ------------- SELL AGENT ----------------

		internal class SellAgent
		{
			internal static bool ListExists(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
				{
					if (((string)row["Description"]).ToLower() == description.ToLower())
						return true;
				}

				return false;
			}

			internal static void ListInsert(string description, int bag)
			{
				foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["SELL_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Bag"] = bag;
				newRow["Selected"] = true;
				m_Dataset.Tables["SELL_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int bag, bool selected)
			{

				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
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
						foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
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
				for (int i = m_Dataset.Tables["SELL_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SELL_ITEMS"].Rows[i];
					if ((string)row["List"] == description)
					{
						row.Delete();
					}
				}

				for (int i = m_Dataset.Tables["SELL_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SELL_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static void ListsRead(out List<RazorEnhanced.SellAgent.SellAgentList> lists)
			{
				List<RazorEnhanced.SellAgent.SellAgentList> listsOut = new List<RazorEnhanced.SellAgent.SellAgentList>();

				foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int bag = (int)row["Bag"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.SellAgent.SellAgentList list = new RazorEnhanced.SellAgent.SellAgentList(description, bag, selected);
					listsOut.Add(list);
				}

				lists = listsOut;
			}

			internal static bool ItemExists(string list, RazorEnhanced.SellAgent.SellAgentItem item)
			{
				foreach (DataRow row in m_Dataset.Tables["SELL_ITEMS"].Rows)
				{
					if ((string)row["List"] == list && (RazorEnhanced.SellAgent.SellAgentItem)row["Item"] == item)
						return true;
				}

				return false;
			}
            internal static int BagRead(string listname)
            {
                foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
                {
                    if ((string)row["Description"] == listname)
                    {
                        return (int)row["Bag"];
                    }
                }

                return 0;
            }

			internal static void ItemInsert(string list, RazorEnhanced.SellAgent.SellAgentItem item)
			{
				DataRow row = m_Dataset.Tables["SELL_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["SELL_ITEMS"].Rows.Add(row);

				Save();
			}

			internal static void ItemReplace(string list, int index, RazorEnhanced.SellAgent.SellAgentItem item)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["SELL_ITEMS"].Rows)
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

			internal static void ItemDelete(string list, RazorEnhanced.SellAgent.SellAgentItem item)
			{
				for (int i = m_Dataset.Tables["SELL_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SELL_ITEMS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.SellAgent.SellAgentItem)row["Item"] == item)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void ItemsRead(string list, out List<RazorEnhanced.SellAgent.SellAgentItem> items)
			{
				List<RazorEnhanced.SellAgent.SellAgentItem> itemsOut = new List<RazorEnhanced.SellAgent.SellAgentItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["SELL_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							itemsOut.Add((RazorEnhanced.SellAgent.SellAgentItem)row["Item"]);
						}
					}
				}

				items = itemsOut;
			}
		}
		// ------------- SELL AGENT END-----------------



		// ------------- BUY AGENT ----------------

		internal class BuyAgent
		{
			internal static bool ListExists(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
				{
					if (((string)row["Description"]).ToLower() == description.ToLower())
						return true;
				}

				return false;
			}

			internal static void ListInsert(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["BUY_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Selected"] = true;
				m_Dataset.Tables["BUY_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, bool selected)
			{

				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
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
						foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ListDelete(string description)
			{
				for (int i = m_Dataset.Tables["BUY_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["BUY_ITEMS"].Rows[i];
					if ((string)row["List"] == description)
					{
						row.Delete();
					}
				}

				for (int i = m_Dataset.Tables["BUY_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["BUY_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static void ListsRead(out List<RazorEnhanced.BuyAgent.BuyAgentList> lists)
			{
				List<RazorEnhanced.BuyAgent.BuyAgentList> listsOut = new List<RazorEnhanced.BuyAgent.BuyAgentList>();

				foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.BuyAgent.BuyAgentList list = new RazorEnhanced.BuyAgent.BuyAgentList(description, selected);
					listsOut.Add(list);
				}

				lists = listsOut;
			}

			internal static bool ItemExists(string list, RazorEnhanced.BuyAgent.BuyAgentItem item)
			{
				foreach (DataRow row in m_Dataset.Tables["BUY_ITEMS"].Rows)
				{
					if ((string)row["List"] == list && (RazorEnhanced.BuyAgent.BuyAgentItem)row["Item"] == item)
						return true;
				}

				return false;
			}

			internal static void ItemInsert(string list, RazorEnhanced.BuyAgent.BuyAgentItem item)
			{
				DataRow row = m_Dataset.Tables["BUY_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["BUY_ITEMS"].Rows.Add(row);

				Save();
			}

			internal static void ItemReplace(string list, int index, RazorEnhanced.BuyAgent.BuyAgentItem item)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["BUY_ITEMS"].Rows)
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

			internal static void ItemDelete(string list, RazorEnhanced.BuyAgent.BuyAgentItem item)
			{
				for (int i = m_Dataset.Tables["BUY_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["BUY_ITEMS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.BuyAgent.BuyAgentItem)row["Item"] == item)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void ItemsRead(string list, out List<RazorEnhanced.BuyAgent.BuyAgentItem> items)
			{
				List<RazorEnhanced.BuyAgent.BuyAgentItem> itemsOut = new List<RazorEnhanced.BuyAgent.BuyAgentItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["BUY_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							itemsOut.Add((RazorEnhanced.BuyAgent.BuyAgentItem)row["Item"]);
						}
					}
				}

				items = itemsOut;
			}
		}
		// ------------- BUY AGENT END-----------------

        // ------------- DRESS ----------------

        internal class Dress
        {
            internal static bool ListExists(string description)
            {
                foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
                {
                    if (((string)row["Description"]).ToLower() == description.ToLower())
                        return true;
                }

                return false;
            }

            internal static void ListInsert(string description, int delay, int bag, bool conflict)
            {
                foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
                {
                    row["Selected"] = false;
                }

                DataRow newRow = m_Dataset.Tables["DRESS_LISTS"].NewRow();
                newRow["Description"] = description;
                newRow["Delay"] = delay;
                newRow["Bag"] = bag;
                newRow["Conflict"] = conflict;
                newRow["Selected"] = true;
                m_Dataset.Tables["DRESS_LISTS"].Rows.Add(newRow);

                Save();
            }

            internal static void ListUpdate(string description, int delay, int bag, bool conflict, bool selected)
            {

                bool found = false;
                foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
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
                        foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
                        {
                            row["Selected"] = false;
                        }
                    }

                    foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
                    {
                        if ((string)row["Description"] == description)
                        {
                            row["Delay"] = delay;
                            row["Bag"] = bag;
                            row["Conflict"] = conflict;
                            row["Selected"] = selected;
                            break;
                        }
                    }

                    Save();
                }
            }

            internal static void ListDelete(string description)
            {
                for (int i = m_Dataset.Tables["DRESS_ITEMS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["DRESS_ITEMS"].Rows[i];
                    if ((string)row["List"] == description)
                    {
                        row.Delete();
                    }
                }

                for (int i = m_Dataset.Tables["DRESS_LISTS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["DRESS_LISTS"].Rows[i];
                    if ((string)row["Description"] == description)
                    {
                        row.Delete();
                        break;
                    }
                    row["Selected"] = false;
                }

                Save();
            }

            internal static void ListsRead(out List<RazorEnhanced.Dress.DressList> lists)
            {
                List<RazorEnhanced.Dress.DressList> listsOut = new List<RazorEnhanced.Dress.DressList>();

                foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
                {
                    string description = (string)row["Description"];
                    int delay = (int)row["Delay"];
                    int bag = (int)row["Bag"];
                    bool conflict = (bool)row["Conflict"];
                    bool selected = (bool)row["Selected"];

                    RazorEnhanced.Dress.DressList list = new RazorEnhanced.Dress.DressList(description, delay, bag, conflict, selected);
                    listsOut.Add(list);
                }

                lists = listsOut;
            }

            internal static void ItemsRead(string list, out List<RazorEnhanced.Dress.DressItem> items)
            {
                List<RazorEnhanced.Dress.DressItem> itemsOut = new List<RazorEnhanced.Dress.DressItem>();

                if (ListExists(list))
                {
                    foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
                    {
                        if ((string)row["List"] == list)
                        {
                            itemsOut.Add((RazorEnhanced.Dress.DressItem)row["Item"]);
                        }
                    }
                }

                items = itemsOut;
            }

            internal static void ListDetailsRead(string listname, out int bag, out int delay, out bool conflict)
            {
                int bagOut = 0;
                int delayOut = 0;
                bool conflictOut = false;
                foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
                {
                    if ((string)row["Description"] == listname)
                    {
                        bagOut = (int)row["Bag"];
                        delayOut = (int)row["Delay"];
                        conflictOut = (bool)row["Conflict"];
                    }
                }
                bag = bagOut;
                delay = delayOut;
                conflict = conflictOut;

            }

            internal static void ItemClear(string list)
            {
                if (ListExists(list))
                {
                    foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
                    {
                        if ((string)row["List"] == list)
                            row.Delete();
                    }
                }
                Save();
            }
            internal static void ItemInsert(string list, RazorEnhanced.Dress.DressItem item)
            {
                DataRow row = m_Dataset.Tables["DRESS_ITEMS"].NewRow();
                row["List"] = list;
                row["Item"] = item;
                m_Dataset.Tables["DRESS_ITEMS"].Rows.Add(row);

                Save();
            }
            internal static void ItemDelete(string list, RazorEnhanced.Dress.DressItem item)
            {
                for (int i = m_Dataset.Tables["DRESS_ITEMS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["DRESS_ITEMS"].Rows[i];
                    if ((string)row["List"] == list && (RazorEnhanced.Dress.DressItem)row["Item"] == item)
                    {
                        row.Delete();
                        break;
                    }
                }

                Save();
            }
            internal static void ItemReplace(string list, int index, RazorEnhanced.Dress.DressItem item)
            {
                int count = -1;
                foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
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

            internal static void ItemInsertByLayer(string list, RazorEnhanced.Dress.DressItem item)
            {
                bool found = false;
                foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
                {
                    if ((string)row["List"] == list)
                    {
                        RazorEnhanced.Dress.DressItem itemtoscan;
                        itemtoscan = (RazorEnhanced.Dress.DressItem)row["Item"];
                        if (itemtoscan.Layer == item.Layer)
                        {
                            RazorEnhanced.Dress.AddLog("Item repaced");
                            row["Item"] = item;
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    RazorEnhanced.Dress.AddLog("New item added");
                    ItemInsert(list, item);
                }
                Save();
            }
        }

        // ------------- DRESS END-----------------

        // ------------- FRIEND START -----------------

        internal class Friend
        {
            internal static bool ListExists(string description)
            {
                foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
                {
                    if (((string)row["Description"]).ToLower() == description.ToLower())
                        return true;
                }

                return false;
            }


            internal static void ListInsert(string description, bool includeparty, bool preventattack, bool autoacceptparty)
            {
                foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
                {
                    row["Selected"] = false;
                }

                DataRow newRow = m_Dataset.Tables["FRIEND_LISTS"].NewRow();
                newRow["Description"] = description;
                newRow["IncludeParty"] = includeparty;
                newRow["PreventAttack"] = preventattack;
                newRow["AutoacceptParty"] = autoacceptparty;
                newRow["Selected"] = true;
                m_Dataset.Tables["FRIEND_LISTS"].Rows.Add(newRow);

                Save();
            }

            internal static void ListUpdate(string description, bool includeparty, bool preventattack, bool autoacceptparty, bool selected)
            {

                bool found = false;
                foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
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
                        foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
                        {
                            row["Selected"] = false;
                        }
                    }

                    foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
                    {
                        if ((string)row["Description"] == description)
                        {
                            row["Description"] = description;
                            row["IncludeParty"] = includeparty;
                            row["PreventAttack"] = preventattack;
                            row["AutoacceptParty"] = autoacceptparty;
                            row["Selected"] = selected;
                            break;
                        }
                    }

                    Save();
                }
            }

            internal static void ListDelete(string description)
            {
                for (int i = m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].Rows[i];
                    if ((string)row["List"] == description)
                    {
                        row.Delete();
                    }
                }

                for (int i = m_Dataset.Tables["FRIEND_LISTS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["FRIEND_LISTS"].Rows[i];
                    if ((string)row["Description"] == description)
                    {
                        row.Delete();
                        break;
                    }
                    row["Selected"] = false;
                }
                Save();
            }

            internal static void ListsRead(out List<RazorEnhanced.Friend.FriendList> lists)
            {
                List<RazorEnhanced.Friend.FriendList> listsOut = new List<RazorEnhanced.Friend.FriendList>();

                foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
                {
                    string description = (string)row["Description"];
                    bool includeparty = (bool)row["IncludeParty"];
                    bool preventattack = (bool)row["PreventAttack"];
                    bool autoacceptparty = (bool)row["AutoacceptParty"];
                    bool selected = (bool)row["Selected"];

                    RazorEnhanced.Friend.FriendList list = new RazorEnhanced.Friend.FriendList(description, autoacceptparty, preventattack, includeparty, selected);
                    listsOut.Add(list);
                }
                lists = listsOut;
            }



            internal static bool PlayerExists(string list, RazorEnhanced.Friend.FriendPlayer player)
            {
                foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
                {
                    RazorEnhanced.Friend.FriendPlayer dacercare =(RazorEnhanced.Friend.FriendPlayer)row["Player"];
                    if ((string)row["List"] == list && dacercare.Serial == player.Serial)
                        return true;
                }

                return false;
            }

            internal static void PlayerInsert(string list, RazorEnhanced.Friend.FriendPlayer player)
            {
                DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].NewRow();
                row["List"] = list;
                row["Player"] = player;
                m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Add(row);

                Save();
            }

            internal static void PlayerReplace(string list, int index, RazorEnhanced.Friend.FriendPlayer player)
            {
                int count = -1;
                foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
                {
                    if ((string)row["List"] == list)
                    {
                        count++;
                        if (count == index)
                        {
                            row["Player"] = player;
                        }
                    }
                }

                Save();
            }

            internal static void PlayerDelete(string list, RazorEnhanced.Friend.FriendPlayer player)
            {
                for (int i = m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].Rows[i];
                    if ((string)row["List"] == list && (RazorEnhanced.Friend.FriendPlayer)row["Player"] == player)
                    {
                        row.Delete();
                        break;
                    }
                }

                Save();
            }

            internal static void PlayersRead(string list, out List<RazorEnhanced.Friend.FriendPlayer> players)
            {
                List<RazorEnhanced.Friend.FriendPlayer> playersOut = new List<RazorEnhanced.Friend.FriendPlayer>();

                if (ListExists(list))
                {
                    foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
                    {
                        if ((string)row["List"] == list)
                        {
                            playersOut.Add((RazorEnhanced.Friend.FriendPlayer)row["Player"]);
                        }
                    }
                }

                players = playersOut;
            }

            internal static void ListDetailsRead(string listname, out bool includeparty, out bool preventattack, out bool autoacceptparty)
            {
                bool includepartyOut = false;
                bool preventattackOut = false;
                bool autoacceptpartyOut = false;

                foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
                {
                    if ((string)row["Description"] == listname)
                    {
                        includepartyOut = (bool)row["IncludeParty"];
                        preventattackOut = (bool)row["PreventAttack"];
                        autoacceptpartyOut = (bool)row["AutoacceptParty"];
                    }
                }
                includeparty = includepartyOut;
                preventattack = preventattackOut;
                autoacceptparty = autoacceptpartyOut;
            }
        }
        // ------------- FRIEND END-----------------
        
		// ------------- SHARDS -----------------
		internal class Shards
		{
			internal static bool Exists(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
				{
					if (((string)row["Description"]).ToLower() == description.ToLower())
						return true;
				}

				return false;
			}

			internal static void Insert(string description, string clientpath, string clientfolder, string host, string port, bool parchenc, bool osienc)
			{
				foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["SHARDS"].NewRow();
				newRow["Description"] = description;
				newRow["ClientPath"] = clientpath;
				newRow["ClientFolder"] = clientfolder;
				newRow["Host"] = host;
				newRow["Port"] = port;
				newRow["PatchEnc"] = parchenc;
				newRow["OSIEnc"] = osienc;
				newRow["Selected"] = true;
				m_Dataset.Tables["SHARDS"].Rows.Add(newRow);

				Save();
			}

			internal static void Update(string description, string clientpath, string clientfolder, string host, int port, bool parchenc, bool osienc, bool selected)
			{
				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
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
						foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Description"] = description;
							row["ClientPath"] = clientpath;
							row["ClientFolder"] = clientfolder;
							row["Host"] = host;
							row["Port"] = port;
							row["PatchEnc"] = parchenc;
							row["OSIEnc"] = osienc;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

            internal static void UpdateLast(string description)
            {
                foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
                {
                    if ((string)row["Description"] == description)
                    {
                        row["Selected"] = true;
                    }
                    else
                        row["Selected"] = false;
                }
                Save();
            }

			internal static void Delete(string shardname)
			{
                bool last = true;
				for (int i = m_Dataset.Tables["SHARDS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SHARDS"].Rows[i];
					if ((string)row["Description"] == shardname)
					{
						row.Delete();
					}
                    else
                    {
                        if (last)
                        {
                            row["Selected"] = true;
                            last = false;
                        }
                        else
                            row["Selected"] = false;
                    }
				}

				Save();
			}

			internal static void Read(out List<RazorEnhanced.Shard> shards)
			{
				List<RazorEnhanced.Shard> shardsOut = new List<RazorEnhanced.Shard>();

				foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
				{
					string description = (string)row["Description"];
					string clientpath = (string)row["ClientPath"];
					string clientfolder = (string)row["ClientFolder"];
					string host = (string)row["Host"];
					int port = (int)row["Port"];
					bool patchenc = (bool)row["PatchEnc"];
					bool osienc = (bool)row["OSIEnc"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.Shard shard = new RazorEnhanced.Shard(description, clientpath, clientfolder, host, port, patchenc, osienc, selected);
					shardsOut.Add(shard);
				}

				shards = shardsOut;
			}
		}
		// ------------- LAUNCHER END-----------------


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