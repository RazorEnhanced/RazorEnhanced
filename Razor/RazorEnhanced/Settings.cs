using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Text;
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


                // ----------- RESTOCK ----------
                DataTable restock_lists = new DataTable("RESTOCK_LISTS");
                restock_lists.Columns.Add("Description", typeof(string));
                restock_lists.Columns.Add("Delay", typeof(int));
                restock_lists.Columns.Add("Source", typeof(int));
                restock_lists.Columns.Add("Destination", typeof(int));
                restock_lists.Columns.Add("Selected", typeof(bool));
                m_Dataset.Tables.Add(restock_lists);

                DataTable restock_items = new DataTable("RESTOCK_ITEMS");
                restock_items.Columns.Add("List", typeof(string));
                restock_items.Columns.Add("Item", typeof(RazorEnhanced.Restock.RestockItem));
                m_Dataset.Tables.Add(restock_items);


                // ----------- TARGET ----------             
                DataTable targets = new DataTable("TARGETS");
                targets.Columns.Add("Name", typeof(string));
                targets.Columns.Add("TargetGUIObject", typeof(RazorEnhanced.TargetGUI.TargetGUIObject));
                m_Dataset.Tables.Add(targets);


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


                // ----------- FILTER GRAPH CHANGE ----------
                DataTable filter_graph = new DataTable("FILTER_GRAPH");
                filter_graph.Columns.Add("Graph", typeof(RazorEnhanced.Filters.GraphChangeData));
                m_Dataset.Tables.Add(filter_graph);


                // ----------- TOOLBAR ITEM ----------
                DataTable toolbar_items = new DataTable("TOOLBAR_ITEMS");
                toolbar_items.Columns.Add("Item", typeof(RazorEnhanced.ToolBar.ToolBarItem));

                for (int i = 0; i < 14; i++)  // Popolo di slot vuoti al primo avvio
                {
                    DataRow emptytoolbar = toolbar_items.NewRow();
                    RazorEnhanced.ToolBar.ToolBarItem emptyitem = new RazorEnhanced.ToolBar.ToolBarItem("Empty", 0x0000, 0x0000, false, 0);
                    emptytoolbar.ItemArray = new object[] { emptyitem };
                    toolbar_items.Rows.Add(emptytoolbar);
                }
                m_Dataset.Tables.Add(toolbar_items);

                // ----------- SAVE PASSWORD ----------
                DataTable password = new DataTable("PASSWORD");
                password.Columns.Add("IP", typeof(string));
                password.Columns.Add("User", typeof(string));
                password.Columns.Add("Password", typeof(string));
                m_Dataset.Tables.Add(password);


                // ----------- HOTKEYS ----------
                DataTable hotkey = new DataTable("HOTKEYS");
                hotkey.Columns.Add("Group", typeof(string));
                hotkey.Columns.Add("Name", typeof(string));
                hotkey.Columns.Add("Key", typeof(Keys));

                // Parametri primo avvio HotKey
               
                DataRow hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "General", "Resync", Keys.None };
                hotkey.Rows.Add(hotkeyrow);
                
                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "General", "Take Screen Shot", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "General", "Ping Server", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "General", "Accept Party", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "General", "Decline Party", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Actions", "Unmount", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Actions", "Grab Item", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Actions", "Drop Item", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Use", "Last Item", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Use", "Left Hand", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Use", "Right Hand", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Show Names", "All", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Show Names", "Corpses", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Show Names", "Mobiles", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Show Names", "Items", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Pet Commands", "Come", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Pet Commands", "Follow", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Pet Commands", "Guard", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Pet Commands", "Kill", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Pet Commands", "Stay", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Pet Commands", "Stop", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Autoloot Start", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Autoloot Stop", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Scavenger Start", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Scavenger Stop", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Organizer Start", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Organizer Stop", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Sell Agent Enable", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Sell Agent Disable", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Buy Agent Enable", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Buy Agent Disable", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Dress Start", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Dress Stop", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Undress", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Restock Start", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Restock Stop", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Bandage Heal Enable", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Agents", "Bandage Heal Disable", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Abilities", "Primary", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Abilities", "Secondary", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Abilities", "Stun", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Abilities", "Disarm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Attack", "Attack Last Target", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Bandage", "Self", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Bandage", "Last", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Bandage", "Use Only", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Agility", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Cure", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Explosion", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Refresh", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Strenght", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Potions", "Nightsight", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Enchanted Apple", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Orange Petals", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Wrath Grapes", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Rose Of Trinsic", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Smoke Bomb", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Spell Stone", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Other", "Healing Stone", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Hands", "Clear Left", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Hands", "Clear Right", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Clumsy", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Identidication", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Feebleming", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Weakness", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Magic Arrow", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Harm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Fireball", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Greater Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Lightning", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Equip Wands", "Mana Drain", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Last Used", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Animal Lore", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Item ID", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Arms Lore", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Begging", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Peacemaking", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Cartography", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Detect Hidden", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Eval Int", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Forensics", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Hiding", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Provocation", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Spirit Speak", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Stealing", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Animal Taming", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Taste ID", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Tracking", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Meditation", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Stealth", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "RemoveTrap", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Inscribe", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "Skills", "Anatomy", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Mini Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Big Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Chivarly Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Create Food", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Feeblemind", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Arrow", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Night Sight", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Reactive Armor", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Weaken", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Agility", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Cunning", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Cure", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Harm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Trap", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Untrap", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Protection", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Strength", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Bless", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Fireball", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Lock", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Poison", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Telekinesis", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Teleport", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Unlock", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Wall of Stone", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Arch Cure", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Arch Protection", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Curse", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Greater Heal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Lightning", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mana Drain", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Recall", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Blade Spirits", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Dispel Field", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Incognito", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Reflection", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mind Blast", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Paralyze", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Poison Field", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Creature", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Dispel", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Energy Bolt", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Explosion", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Invisibility", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mark", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mass Curse", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Paralyze Field", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Reveal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Chain Lightning", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Energy Field", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Flamestrike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Gate Travel", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mana Vampire", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mass Dispel", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Meteor Swarm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Polymorph", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Earthquake", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Energy Vortex", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Resurrection", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Air Elemental", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Daemon", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Earth Elemental", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Fire Elemental", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Water Elemental", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Animate Dead", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Blood Oath", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Corpse Skin", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Curse Weapon", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Evil Omen", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Horrific Beast", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Lich Form", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Mind Rot", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Pain Spike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Poison Strike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Strangle", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Summon Familiar", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Vampiric Embrace", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Vengeful Spirit", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Wither", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Wraith Form", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Exorcism", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Honorable Execution", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Confidence", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Evasion", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Counter Attack", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Lightning Strike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Momentum Strike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Focus Attack", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Death Strike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Animal Form", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Ki Attack", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Surprise Attack", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Backstab", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Shadow jump", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Mirror Image", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Arcane Circle", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Gift Of Renewal", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Immolating Weapon", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Thunderstorm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Natures Fury", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Summon Fey", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Summoniend", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Reaper Form", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Wildfire", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Essence Of Wind", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Dryad Allure", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Ethereal Voyage", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Word Of Death", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow(); 
                hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Gift Of Life", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Animated Weapon", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Healing Stone", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Purge", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Enchant", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Eagle Strike", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "StoneForm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Spell Trigger", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Mass Sleep", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Cleansing Winds", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Bombard", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Spell Plague", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Hail Storm", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Nether Cyclone", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Rising Colossus", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Cleanse By Fire", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Close Wounds", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Consecrate Weapon", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Dispel Evil", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Divine Fury", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Enemy Of One", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Holy Light", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Noble Sacrifice", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Remove Curse", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Sacred Journey", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Honor", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Sacrifice", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Compassion", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Valor", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Honesty", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Humility", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "UseVirtue", "Justice", Keys.None };
                hotkey.Rows.Add(hotkeyrow);

                m_Dataset.Tables.Add(hotkey);

                // ----------- GENERAL SETTINGS ----------
                DataTable general = new DataTable("GENERAL");

                // Parametri Tab (Agent --> Heal)
                general.Columns.Add("BandageHealcountdownCheckBox", typeof(bool));
                general.Columns.Add("BandageHealtargetComboBox", typeof(string));
                general.Columns.Add("BandageHealtargetLabel", typeof(int));
                general.Columns.Add("BandageHealcustomCheckBox", typeof(bool)); 
                general.Columns.Add("BandageHealcustomIDTextBox", typeof(int)); 
                general.Columns.Add("BandageHealcustomcolorTextBox", typeof(int)); 
                general.Columns.Add("BandageHealdexformulaCheckBox", typeof(bool)); 
                general.Columns.Add("BandageHealdelayTextBox", typeof(int));
                general.Columns.Add("BandageHealhpTextBox", typeof(int)); 
                general.Columns.Add("BandageHealpoisonCheckBox", typeof(bool)); 
                general.Columns.Add("BandageHealmortalCheckBox", typeof(bool));
                general.Columns.Add("BandageHealhiddedCheckBox", typeof(bool));  

                // Parametri Tab (Enhanced Filters)
                general.Columns.Add("HighlightTargetCheckBox", typeof(bool));
                general.Columns.Add("FlagsHighlightCheckBox", typeof(bool));
                general.Columns.Add("ShowStaticFieldCheckBox", typeof(bool));
                general.Columns.Add("BlockTradeRequestCheckBox", typeof(bool));
                general.Columns.Add("BlockPartyInviteCheckBox", typeof(bool));
                general.Columns.Add("MobFilterCheckBox", typeof(bool));
                general.Columns.Add("AutoCarverCheckBox", typeof(bool));
                general.Columns.Add("BoneCutterCheckBox", typeof(bool));
                general.Columns.Add("AutoCarverBladeLabel", typeof(int));
                general.Columns.Add("BoneBladeLabel", typeof(int));
                
                // Parametri Tab (Enhanced ToolBar)
                general.Columns.Add("LockToolBarCheckBox", typeof(bool));
                general.Columns.Add("AutoopenToolBarCheckBox", typeof(bool));
                general.Columns.Add("PosXToolBar", typeof(int));
                general.Columns.Add("PosYToolBar", typeof(int));

                // Parametri Tab (Screenshot)
                general.Columns.Add("CapPath", typeof(string));
                general.Columns.Add("ImageFormat", typeof(string));
                general.Columns.Add("CapFullScreen", typeof(bool));
                general.Columns.Add("CapTimeStamp", typeof(bool));
                general.Columns.Add("AutoCap", typeof(bool));

                // Parametri Tab (Filtri veccchi)
                general.Columns.Add("1001", typeof(bool));
                general.Columns.Add("1387", typeof(bool));
                general.Columns.Add("1002", typeof(bool));
                general.Columns.Add("1003", typeof(bool));
                general.Columns.Add("1004", typeof(bool));
                general.Columns.Add("1005", typeof(bool));
                general.Columns.Add("1006", typeof(bool));
                general.Columns.Add("1007", typeof(bool));
                general.Columns.Add("1008", typeof(bool));
                general.Columns.Add("1321", typeof(bool));
                general.Columns.Add("1507", typeof(bool));
                general.Columns.Add("1478", typeof(bool));
                general.Columns.Add("1009", typeof(bool));

                // Parametri Tab (General)
                general.Columns.Add("SmartCPU", typeof(bool));
                general.Columns.Add("AlwaysOnTop", typeof(bool));
                general.Columns.Add("RememberPwds", typeof(bool));
                general.Columns.Add("Systray", typeof(bool));
                general.Columns.Add("ForceSizeEnabled", typeof(bool));
                general.Columns.Add("ForceSizeX", typeof(int));
                general.Columns.Add("ForceSizeY", typeof(int));
                general.Columns.Add("ClientPrio", typeof(string));
                general.Columns.Add("Opacity", typeof(int));
                general.Columns.Add("WindowX", typeof(int));
                general.Columns.Add("WindowY", typeof(int));

                // Parametri Tab (Skill)
                general.Columns.Add("DisplaySkillChanges", typeof(bool));

                // Parametri Tab (Options)
                general.Columns.Add("ActionStatusMsg", typeof(bool));
                general.Columns.Add("QueueActions", typeof(bool));
                general.Columns.Add("ObjectDelay", typeof(int));
                general.Columns.Add("SmartLastTarget", typeof(bool));
                general.Columns.Add("RangeCheckLT", typeof(bool));
                general.Columns.Add("LTRange", typeof(int));
                general.Columns.Add("LastTargTextFlags", typeof(bool));
                general.Columns.Add("ShowHealth", typeof(bool));
                general.Columns.Add("HealthFmt", typeof(string));
                general.Columns.Add("ShowPartyStats", typeof(bool));
                general.Columns.Add("OldStatBar", typeof(bool));
                general.Columns.Add("QueueTargets", typeof(bool));
                general.Columns.Add("BlockDismount", typeof(bool));
                general.Columns.Add("AutoStack", typeof(bool));
                general.Columns.Add("AutoOpenCorpses", typeof(bool));
                general.Columns.Add("CorpseRange", typeof(int));
                general.Columns.Add("FilterSpam", typeof(bool));
                general.Columns.Add("FilterSnoopMsg", typeof(bool));
                general.Columns.Add("ShowMobNames", typeof(bool));
                general.Columns.Add("Negotiate", typeof(bool));
                general.Columns.Add("ShowCorpseNames", typeof(bool));
                general.Columns.Add("CountStealthSteps", typeof(bool));
                general.Columns.Add("AlwaysStealth", typeof(bool));
                general.Columns.Add("AutoOpenDoors", typeof(bool));
                general.Columns.Add("SpellUnequip", typeof(bool));
                general.Columns.Add("PotionEquip", typeof(bool));
                general.Columns.Add("ForceSpeechHue", typeof(bool));
                general.Columns.Add("ForceSpellHue", typeof(bool));
                general.Columns.Add("SpellFormat", typeof(string));
                general.Columns.Add("MessageLevel", typeof(int));

                // Parametri Tab (Options -> Hues)
                general.Columns.Add("LTHilight", typeof(int));
                general.Columns.Add("NeutralSpellHue", typeof(int));
                general.Columns.Add("HarmfulSpellHue", typeof(int));
                general.Columns.Add("BeneficialSpellHue", typeof(int));
                general.Columns.Add("SpeechHue", typeof(int));
                general.Columns.Add("ExemptColor", typeof(int));
                general.Columns.Add("WarningColor", typeof(int));
                general.Columns.Add("SysColor", typeof(int));

                // Parametri Tab (HotKey)
                general.Columns.Add("HotKeyEnable", typeof(bool));
                general.Columns.Add("HotKeyMasterKey", typeof(Keys));

                // Composizione Parematri base primo avvio
                object[] generalstartparam = new object[] { 
                    // Parametri primo avvio per tab agent Bandage heal
                    false, "Self", 0, false, 0, 0, false, 1000, 100, false, false, false,

                    // Parametri primo avvio per tab Enhanced Filters
                    false, false, false, false, false, false, false, false, 0, 0,
                    
                    // Parametri primo avvio per tab Enhanced ToolBar
                    false, false, 10, 10,

                    // Parametri primo avvio per tab screenshot
                    Directory.GetCurrentDirectory(), "jpg", false, false, false,
                    
                    // Parametri primo avvio per vecchi filtri
                    false, false, false, false, false, false, false, false, false, false, false, false, false,

                    // Parametri primo avvio tab general
                    false, false, false, false, false, 800, 600, "Normal", 100, 400, 400,

                    // Parametri primo avvio tab skill
                    false,
                    
                    // Parametri primo avvio tab Options
                    false, false, 600, false, false, 12, false, false, "[{0}%]", false, false, false, false, false, false, 2, false, false, false, false, false, false, false, false, false, false, false, false, @"{power} [{spell}]", 0,

                    // Parametri primo avvio tab Options -> Hues
                    (int)0, (int)0x03B1, (int)0x0025, (int)0x0005, (int)0x03B1, (int)0x0480, (int)0x0025, (int)0x03B1,

                    // Parametri primo avvio tab HotKey
                    false, Keys.None

                };

                DataRow generalsettings = general.NewRow();
                generalsettings.ItemArray = generalstartparam;
                general.Rows.Add(generalsettings);

                m_Dataset.Tables.Add(general);
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


        // ------------- RESTOCK  -----------------

        internal class Restock
        {
            internal static bool ListExists(string description)
            {
                foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
                {
                    if (((string)row["Description"]).ToLower() == description.ToLower())
                        return true;
                }

                return false;
            }

            internal static void ListInsert(string description, int delay, int source, int destination)
            {
                foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
                {
                    row["Selected"] = false;
                }

                DataRow newRow = m_Dataset.Tables["RESTOCK_LISTS"].NewRow();
                newRow["Description"] = description;
                newRow["Delay"] = delay;
                newRow["Source"] = source;
                newRow["Destination"] = destination;
                newRow["Selected"] = true;
                m_Dataset.Tables["RESTOCK_LISTS"].Rows.Add(newRow);

                Save();
            }

            internal static void ListUpdate(string description, int delay, int source, int destination, bool selected)
            {

                bool found = false;
                foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
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
                        foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
                        {
                            row["Selected"] = false;
                        }
                    }

                    foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
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
                for (int i = m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].Rows[i];
                    if ((string)row["List"] == description)
                    {
                        row.Delete();
                    }
                }

                for (int i = m_Dataset.Tables["RESTOCK_LISTS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["RESTOCK_LISTS"].Rows[i];
                    if ((string)row["Description"] == description)
                    {
                        row.Delete();
                        break;
                    }
                    row["Selected"] = false;
                }

                Save();
            }

            internal static void ListsRead(out List<RazorEnhanced.Restock.RestockList> lists)
            {
                List<RazorEnhanced.Restock.RestockList> listsOut = new List<RazorEnhanced.Restock.RestockList>();

                foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
                {

                    string description = (string)row["Description"];
                    int delay = (int)row["Delay"];
                    int source = (int)row["Source"];
                    int destination = (int)row["Destination"];
                    bool selected = (bool)row["Selected"];

                    RazorEnhanced.Restock.RestockList list = new RazorEnhanced.Restock.RestockList(description, delay, source, destination, selected);
                    listsOut.Add(list);
                }

                lists = listsOut;
            }

            internal static bool ItemExists(string list, RazorEnhanced.Restock.RestockItem item)
            {
                foreach (DataRow row in m_Dataset.Tables["RESTOCK_ITEMS"].Rows)
                {
                    if ((string)row["List"] == list && (RazorEnhanced.Restock.RestockItem)row["Item"] == item)
                        return true;
                }

                return false;
            }

            internal static void ItemInsert(string list, RazorEnhanced.Restock.RestockItem item)
            {
                DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].NewRow();
                row["List"] = list;
                row["Item"] = item;
                m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Add(row);

                Save();
            }

            internal static void ItemReplace(string list, int index, RazorEnhanced.Restock.RestockItem item)
            {
                int count = -1;
                foreach (DataRow row in m_Dataset.Tables["RESTOCK_ITEMS"].Rows)
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

            internal static void ItemDelete(string list, RazorEnhanced.Restock.RestockItem item)
            {
                for (int i = m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].Rows[i];
                    if ((string)row["List"] == list && (RazorEnhanced.Restock.RestockItem)row["Item"] == item)
                    {
                        row.Delete();
                        break;
                    }
                }

                Save();
            }

            internal static void ItemsRead(string list, out List<RazorEnhanced.Restock.RestockItem> items)
            {
                List<RazorEnhanced.Restock.RestockItem> itemsOut = new List<RazorEnhanced.Restock.RestockItem>();

                if (ListExists(list))
                {
                    foreach (DataRow row in m_Dataset.Tables["RESTOCK_ITEMS"].Rows)
                    {
                        if ((string)row["List"] == list)
                        {
                            itemsOut.Add((RazorEnhanced.Restock.RestockItem)row["Item"]);
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
                foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
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
        // ------------- RESTOCK END-----------------


        // ------------- GRAPH FILTER  -----------------

        internal class GraphFilter
        {
            internal static List<RazorEnhanced.Filters.GraphChangeData> ReadAll()
            {
                List<RazorEnhanced.Filters.GraphChangeData> graphdatalist = new List<RazorEnhanced.Filters.GraphChangeData>();

                foreach (DataRow row in m_Dataset.Tables["FILTER_GRAPH"].Rows)
                {
                    RazorEnhanced.Filters.GraphChangeData graphdata = (RazorEnhanced.Filters.GraphChangeData)row["Graph"];
                    graphdatalist.Add(graphdata);
                }

                return graphdatalist;
            }

            internal static void Insert(int graphreal, int graphnew)
            {
                RazorEnhanced.Filters.GraphChangeData graphdata = new RazorEnhanced.Filters.GraphChangeData(true, graphreal, graphnew);

                DataRow row = m_Dataset.Tables["FILTER_GRAPH"].NewRow();
                row["Graph"] = graphdata;
                m_Dataset.Tables["FILTER_GRAPH"].Rows.Add(row);

                Save();
            }

            internal static void Replace(int index, RazorEnhanced.Filters.GraphChangeData graphdata)
            {
                int count = -1;
                foreach (DataRow row in m_Dataset.Tables["FILTER_GRAPH"].Rows)
                {
                        count++;
                        if (count == index)
                        {
                            row["Graph"] = graphdata;
                        }
                }

                Save();
            }

            internal static bool Exist(int graphreal)
            {
                for (int i = m_Dataset.Tables["FILTER_GRAPH"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["FILTER_GRAPH"].Rows[i];
                    RazorEnhanced.Filters.GraphChangeData graphdata = (RazorEnhanced.Filters.GraphChangeData)row["Graph"];
                    if (graphdata.GraphReal == graphreal)
                    {
                        return true;
                    }
                }
                return false;
            }

            internal static void Delete(int graphreal)
            {
                for (int i = m_Dataset.Tables["FILTER_GRAPH"].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = m_Dataset.Tables["FILTER_GRAPH"].Rows[i];
                    RazorEnhanced.Filters.GraphChangeData graphdata = (RazorEnhanced.Filters.GraphChangeData)row["Graph"];
                    if (graphdata.GraphReal == graphreal)
                    {
                        row.Delete();
                        break;
                    }
                }

                Save();
            }

        }
        // ------------- GRAPH FILTER END-----------------


        // ------------- TARGET SETTINGS START -----------------
        internal class Target
        {
            internal static List<TargetGUI.TargetGUIObjectList> ReadAll()
            {
                List<TargetGUI.TargetGUIObjectList> list = new List<TargetGUI.TargetGUIObjectList>();
                foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
                {
                    string name = (string)row["Name"];
                    TargetGUI.TargetGUIObject target = (TargetGUI.TargetGUIObject)row["TargetGUIObject"];
                    list.Add(new TargetGUI.TargetGUIObjectList(name, target));
                }
                return list;
            }

            internal static bool TargetExist(string targetid)
            {
                foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
                {
                    if ((string)row["Name"] == targetid)
                        return true;
                }
                return false;
            }

            internal static void TargetSave(string targetid, TargetGUI.TargetGUIObject target)
            {
                if (TargetExist(targetid))
                {
                    TargetDelete(targetid);
                }

                DataRow row = m_Dataset.Tables["TARGETS"].NewRow();
                row["Name"] = targetid;
                row["TargetGUIObject"] = target;
                m_Dataset.Tables["TARGETS"].Rows.Add(row);

                Save();
            }

            internal static void TargetDelete(string targetid)
            {
                if (TargetExist(targetid))
                {
                    foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
                    {
                        if ((string)row["Name"] == targetid)
                        {
                            row.Delete();
                            break;
                        }
                    }
                }
                Save();
            }
            internal static TargetGUI.TargetGUIObject TargetRead(string targetid)
            {
                foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
                {
                    if ((string)row["Name"] == targetid)
                        return (TargetGUI.TargetGUIObject)row["TargetGUIObject"];
                }
                return null;
            }
        }


        // ------------- TARGET SETTINGS END -----------------
        

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
		// ------------- LAUNCHER END -----------------

        // ------------- TOOLBAR -----------------
        internal class Toolbar
        {
            internal static List<RazorEnhanced.ToolBar.ToolBarItem> ReadItems()
            {
                List<RazorEnhanced.ToolBar.ToolBarItem> itemsOut = new List<RazorEnhanced.ToolBar.ToolBarItem>();

                foreach (DataRow row in m_Dataset.Tables["TOOLBAR_ITEMS"].Rows)
                {
                    RazorEnhanced.ToolBar.ToolBarItem item = (RazorEnhanced.ToolBar.ToolBarItem)row["Item"];
                    itemsOut.Add(item);
                }
                return itemsOut;
            }

            internal static RazorEnhanced.ToolBar.ToolBarItem ReadSelectedItem(int index)
            {
                return (RazorEnhanced.ToolBar.ToolBarItem)m_Dataset.Tables["TOOLBAR_ITEMS"].Rows[index]["Item"];
            }

            internal static void UpdateItem(int index, string name, string graphics, string color, bool warning, string warninglimit)
            {
                int convgraphics = 0;
                int convcolor = 0;
                int convwarninglimit = 0;

                try
                {
                    convgraphics = Convert.ToInt32(graphics, 16);
                }
                catch
                { }

                if (color == "-1")
                {
                    convcolor = -1;
                }
                else
                {
                    try
                    {
                        convcolor = Convert.ToInt32(color, 16);
                    }
                    catch
                    { }
                }

                try
                {
                    convwarninglimit = Convert.ToInt32(warninglimit);
                }
                catch
                { }

                RazorEnhanced.ToolBar.ToolBarItem item = new RazorEnhanced.ToolBar.ToolBarItem(name, convgraphics, convcolor, warning, convwarninglimit);
                m_Dataset.Tables["TOOLBAR_ITEMS"].Rows[index]["Item"] = item;
                Save();
            }
        }

        // ------------- TOOLBAR END -----------------

        // ------------- PASSWORD START -----------------
        internal class Password
        {
            internal static void Insert(string IP, string user, string password)
            {
                foreach (DataRow row in m_Dataset.Tables["PASSWORD"].Rows)
                {
                    string ip = (string)row["IP"];
                    string username = (string)row["User"];
                    if (ip == IP && username == user)
                        row.Delete();
                }

                DataRow newRow = m_Dataset.Tables["PASSWORD"].NewRow();
                newRow["IP"] = IP;
                newRow["User"] = user;
                newRow["Password"] = password;
                m_Dataset.Tables["PASSWORD"].Rows.Add(newRow);

                Save();
            }

            internal static List<Assistant.PasswordMemory.PasswordData> RealAll()
            {
                List<Assistant.PasswordMemory.PasswordData> dataOut = new List<Assistant.PasswordMemory.PasswordData>();

                foreach (DataRow row in m_Dataset.Tables["PASSWORD"].Rows)
                {
                    string ip = (string)row["IP"];
                    string user = (string)row["User"];
                    string password = (string)row["Password"];

                    Assistant.PasswordMemory.PasswordData data = new Assistant.PasswordMemory.PasswordData(ip, user, password);
                    dataOut.Add(data);
                }
                return dataOut;
            }
        }

        // ------------- PASSWORD END -----------------

        // ------------- HOTKEYS START -----------------
        internal class HotKey
        {
            internal static List<RazorEnhanced.HotKey.HotKeyData> ReadGroup(string gname)
            {
                List<RazorEnhanced.HotKey.HotKeyData> keydataOut = new List<RazorEnhanced.HotKey.HotKeyData>();

                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((string)row["Group"] == gname)
                    {
                        string name = (string)row["Name"];
                        Keys key = (Keys)row["Key"];
                        keydataOut.Add(new RazorEnhanced.HotKey.HotKeyData(name, key));
                    }
                }
                return keydataOut;
            }

            internal static void UpdateKey(string name, Keys key)
            {
                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((string)row["Name"] == name)
                    {
                        row["Key"] = key;
                        break;
                    }

                }
                Save();
            }

            internal static void UnassignKey(Keys key)
            {
                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((Keys)row["Key"] == key)
                    {
                        row["Key"] = Keys.None;
                    }
                }

                if (RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey") == key)
                {
                    RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", Keys.None);
                    Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + RazorEnhanced.HotKey.m_Masterkey.ToString();
                }

                Save();
            }

            internal static bool AssignedKey(Keys key)
            {
                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((Keys)row["Key"] == key)
                    {
                        return true;
                    }
                }
                if (RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey") == key)
                    return true;

                return false;
            }

            internal static Keys FindKey(string name)
            {
                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((string)row["Name"] == name)
                    {
                        return (Keys)row["Key"];
                    }
                }
                return Keys.None;
            }

            internal static string FindString(Keys key)
            {
                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((Keys)row["Key"] == key)
                    {
                        return (String)row["Name"];
                    }
                }
                return null;
            }

            internal static string FindGroup(Keys key)
            {
                foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
                {
                    if ((Keys)row["Key"] == key)
                    {
                        return (String)row["Group"];
                    }
                }
                return null;
            }

            internal static List<RazorEnhanced.HotKey.HotKeyData> AgentListKey(string agentdatatable)
            {
                List<RazorEnhanced.HotKey.HotKeyData> listsOut = new List<RazorEnhanced.HotKey.HotKeyData>();

                foreach (DataRow row in m_Dataset.Tables[agentdatatable].Rows)
                {
                    string description = (string)row["Description"];
                    Keys key = (Keys)row["HotKey"];

                    RazorEnhanced.HotKey.HotKeyData list = new RazorEnhanced.HotKey.HotKeyData(description, key);
                    listsOut.Add(list);
                }

                return listsOut;
            }
        }
        // ------------- HOTKEYS END -----------------

        // ------------- GENERAL SETTINGS START -----------------
        internal class General
		{
              // EnhancedFilterTab
            internal static void EnhancedFilterLoadAll(out bool HighlightTargetCheckBox, out bool FlagsHighlightCheckBox, out bool ShowStaticFieldCheckBox, out bool BlockTradeRequestCheckBox, out bool BlockPartyInviteCheckBox, out bool MobFilterCheckBox, out bool AutoCarverCheckBox, out bool BoneCutterCheckBox, out int AutoCarverBladeLabel, out int BoneBladeLabel)
            {
                bool HighlightTargetCheckBoxOut = false;
                bool FlagsHighlightCheckBoxOut = false;
                bool ShowStaticFieldCheckBoxOut = false;
                bool BlockTradeRequestCheckBoxOut = false;
                bool BlockPartyInviteCheckBoxOut = false;
                bool MobFilterCheckBoxOut = false;
                bool AutoCarverCheckBoxOut = false;
                bool BoneCutterCheckBoxOut = false;
                int AutoCarverBladeLabelOut = 0;
                int BoneBladeLabelOut = 0;
                

                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    HighlightTargetCheckBoxOut = (bool)row["HighlightTargetCheckBox"];
                    FlagsHighlightCheckBoxOut = (bool)row["FlagsHighlightCheckBox"];
                    ShowStaticFieldCheckBoxOut = (bool)row["ShowStaticFieldCheckBox"];
                    BlockTradeRequestCheckBoxOut = (bool)row["BlockTradeRequestCheckBox"];
                    BlockPartyInviteCheckBoxOut = (bool)row["BlockPartyInviteCheckBox"];
                    MobFilterCheckBoxOut = (bool)row["MobFilterCheckBox"];
                    AutoCarverCheckBoxOut = (bool)row["AutoCarverCheckBox"];
                    BoneCutterCheckBoxOut = (bool)row["BoneCutterCheckBox"];
                    AutoCarverBladeLabelOut = (int)row["AutoCarverBladeLabel"];
                    BoneBladeLabelOut = (int)row["BoneBladeLabel"];
                }

                HighlightTargetCheckBox = HighlightTargetCheckBoxOut;
                FlagsHighlightCheckBox = FlagsHighlightCheckBoxOut;
                ShowStaticFieldCheckBox = ShowStaticFieldCheckBoxOut;
                BlockTradeRequestCheckBox = BlockTradeRequestCheckBoxOut;
                BlockPartyInviteCheckBox = BlockPartyInviteCheckBoxOut;
                MobFilterCheckBox = MobFilterCheckBoxOut;
                AutoCarverCheckBox = AutoCarverCheckBoxOut;
                BoneCutterCheckBox = BoneCutterCheckBoxOut;
                AutoCarverBladeLabel = AutoCarverBladeLabelOut;
                BoneBladeLabel = BoneBladeLabelOut;
            }

            // Enhanced Toolbar Tab
            internal static void EnhancedToolBarLoadAll(out bool LockToolBarCheckBox, out bool AutoopenToolBarCheckBox, out int PosXToolBar, out int PosYToolBar)
            {
                bool LockToolBarCheckBoxOut = false;
                bool AutoopenToolBarCheckOut = false;
                int PosXToolBarOut = 10;
                int PosYToolBarOut = 10;

                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    LockToolBarCheckBoxOut = (bool)row["LockToolBarCheckBox"];
                    AutoopenToolBarCheckOut = (bool)row["AutoopenToolBarCheckBox"];
                    PosXToolBarOut = (int)row["PosXToolBar"];
                    PosYToolBarOut = (int)row["PosYToolBar"];
                }

                LockToolBarCheckBox = LockToolBarCheckBoxOut;
                AutoopenToolBarCheckBox = AutoopenToolBarCheckOut;
                PosXToolBar = PosXToolBarOut;
                PosYToolBar = PosYToolBarOut;
            }

            internal static bool ReadBool(string name)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row =  m_Dataset.Tables["GENERAL"].Rows[0];
                    return (bool)row[name];
                }
                return false;
            }

            internal static void WriteBool(string name, bool value)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    row[name] = value;
                    Save();
                }
            }

            internal static string ReadString(string name)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    return (string)row[name];
                }
                return "";
            }

            internal static void WriteString(string name, string value)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    row[name] = value;
                    Save();
                }
            }
            internal static int ReadInt(string name)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    return (int)row[name];
                }
                return 1;
            }

            internal static void WriteInt(string name, int value)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    row[name] = value;
                    Save();
                }
            }

            internal static Keys ReadKey(string name)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    return (Keys)row[name];
                }
                return Keys.None;
            }

            internal static void WriteKey(string name, Keys k)
            {
                if (m_Dataset.Tables["GENERAL"].Rows.Count > 0)
                {
                    DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
                    row[name] = k;
                    Save();
                }
            }
        }

        // ------------- GENERAL SETTINGS END -----------------

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