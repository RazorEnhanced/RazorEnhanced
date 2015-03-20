using System;
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
				DataTable scripting = new DataTable("SCRIPTING");
				scripting.Columns.Add("Checked", typeof(bool));
				scripting.Columns.Add("Filename", typeof(string));
				scripting.Columns.Add("Flag", typeof(Bitmap));
				scripting.Columns.Add("Status", typeof(string));
				m_Dataset.Tables.Add(scripting);

                // Autoloot
				DataTable autoloot_lists = new DataTable("AUTOLOOT_LISTS");
				autoloot_lists.Columns.Add("Name", typeof(string));
				autoloot_lists.Columns.Add("List", typeof(List<AutoLoot.AutoLootItem>));
				m_Dataset.Tables.Add(autoloot_lists);

				DataTable autoloot_general = new DataTable("AUTOLOOT_GENERAL");
				autoloot_general.Columns.Add("Label", typeof(string));
				autoloot_general.Columns.Add("List", typeof(List<string>));
				autoloot_general.Columns.Add("Selection", typeof(string));
                autoloot_general.Columns.Add("Bag", typeof(string));
				m_Dataset.Tables.Add(autoloot_general);

                
                //Scavenger
                DataTable scavenger_lists = new DataTable("SCAVENGER_LISTS");
                scavenger_lists.Columns.Add("Name", typeof(string));
                scavenger_lists.Columns.Add("List", typeof(List<Scavenger.ScavengerItem>));
                m_Dataset.Tables.Add(scavenger_lists);

                DataTable scavenger_general = new DataTable("SCAVENGER_GENERAL");
                scavenger_general.Columns.Add("Label", typeof(string));
                scavenger_general.Columns.Add("List", typeof(List<string>));
                scavenger_general.Columns.Add("Selection", typeof(string));
                scavenger_general.Columns.Add("Bag", typeof(string));
                m_Dataset.Tables.Add(scavenger_general);

                //Organizer
                DataTable organizer_lists = new DataTable("ORGANIZER_LISTS");
                organizer_lists.Columns.Add("Name", typeof(string));
                organizer_lists.Columns.Add("SourceBag", typeof(string));
                organizer_lists.Columns.Add("DestinationBag", typeof(string));
                organizer_lists.Columns.Add("List", typeof(List<Organizer.OrganizerItem>));
                m_Dataset.Tables.Add(organizer_lists);

                DataTable organizer_general = new DataTable("ORGANIZER_GENERAL");
                organizer_general.Columns.Add("Label", typeof(string));
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
                sell_lists.Columns.Add("HotBag", typeof(string));
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

                //Dress
                DataTable dress_general = new DataTable("DRESS_GENERAL");
                dress_general.Columns.Add("Label", typeof(string));
                dress_general.Columns.Add("List", typeof(List<string>));
                dress_general.Columns.Add("Selection", typeof(string));
                m_Dataset.Tables.Add(dress_general);

                DataTable dress_lists = new DataTable("DRESS_LISTS");
                dress_lists.Columns.Add("Name", typeof(string));
                dress_lists.Columns.Add("UndressBag", typeof(string));
                dress_lists.Columns.Add("List", typeof(List<RazorEnhanced.Dress.DressItem>));
                dress_lists.Columns.Add("Conflict", typeof(bool));
                m_Dataset.Tables.Add(dress_lists);

				m_Dataset.AcceptChanges();
			}
		}

        // Autoloot
		internal static bool LoadAutoLootItemList(string name, out List<AutoLoot.AutoLootItem> list)
		{
			bool exit = false;
			List<AutoLoot.AutoLootItem> result = new List<AutoLoot.AutoLootItem>();

			foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
			{
				if ((string)row["Name"] == name)
				{
					result = row["List"] as List<AutoLoot.AutoLootItem>;
					exit = true;
				}
			}

			list = result;
			return exit;
		}

		internal static void SaveAutoLootItemList(string name, List<AutoLoot.AutoLootItem> list)
		{
			DataRow row = m_Dataset.Tables["AUTOLOOT_LISTS"].NewRow();
			row["Name"] = name;
			row["List"] = list;
			m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Add(row);
			Save();
		}

		internal static void ClearAutoLootItemList()
		{
			m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Clear();
			Save();
		}

        internal static bool LoadAutoLootGeneral(out string label, out List<string> list, out string selection, out string bag)
		{
			bool exit = false;

			string labelOut = "";
			List<string> listOut = new List<string>();
			string selectionOut = "";
            string bagOut = "0x0000000";

			if (m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows.Count == 1)
			{
				DataRow row = m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows[0];
				{
					labelOut = (string)row["Label"];
					listOut = row["List"] as List<string>;
					selectionOut = (string)row["Selection"];
                    bagOut = (string)row["Bag"];
					exit = true;
				}
			}

			label = labelOut;
			list = listOut;
			selection = selectionOut;
            bag = bagOut;

			return exit;
		}

		internal static void SaveAutoLootIGeneral(string label, List<string> list, string selection, string bag)
		{
			m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows.Clear();
			DataRow row = m_Dataset.Tables["AUTOLOOT_GENERAL"].NewRow();
			row["Label"] = label;
			row["List"] = list;
			row["Selection"] = selection;
            row["Bag"] = bag;
			m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows.Add(row);
			Save();
		}

        //Scavenger
        internal static bool LoadScavengerItemList(string name, out List<Scavenger.ScavengerItem> list)
        {
            bool exit = false;
            List<Scavenger.ScavengerItem> result = new List<Scavenger.ScavengerItem>();

            foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
            {
                if ((string)row["Name"] == name)
                {
                    result = row["List"] as List<Scavenger.ScavengerItem>;
                    exit = true;
                }
            }

            list = result;
            return exit;
        }

        internal static void SaveScavengerItemList(string name, List<Scavenger.ScavengerItem> list)
        {
            DataRow row = m_Dataset.Tables["SCAVENGER_LISTS"].NewRow();
            row["Name"] = name;
            row["List"] = list;
            m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Add(row);
            Save();
        }

        internal static bool LoadScavengerGeneral(out string label, out List<string> list, out string selection, out string bag)
        {
            bool exit = false;

            string labelOut = "";
            List<string> listOut = new List<string>();
            string selectionOut = "";
            string BagOut = "0x0000000";

            if (m_Dataset.Tables["SCAVENGER_GENERAL"].Rows.Count == 1)
            {
                DataRow row = m_Dataset.Tables["SCAVENGER_GENERAL"].Rows[0];
                {
                    labelOut = (string)row["Label"];
                    listOut = row["List"] as List<string>;
                    selectionOut = (string)row["Selection"];
                    BagOut = (string)row["Bag"];
                    exit = true;
                }
            }

            label = labelOut;
            list = listOut;
            selection = selectionOut;
            bag = BagOut;

            return exit;
        }

        internal static void SaveScavengerGeneral(string label, List<string> list, string selection, string bag)
        {
            m_Dataset.Tables["SCAVENGER_GENERAL"].Rows.Clear();
            DataRow row = m_Dataset.Tables["SCAVENGER_GENERAL"].NewRow();
            row["Label"] = label;
            row["List"] = list;
            row["Selection"] = selection;
            row["Bag"] = bag;
            m_Dataset.Tables["SCAVENGER_GENERAL"].Rows.Add(row);
            Save();
        }

        //Organizer
        internal static bool LoadOrganizerItemList(string name, out List<Organizer.OrganizerItem> list, out string SourceBag, out string DestinationBag)
        {
            bool exit = false;
            string SourceBagOut = "0x0000000";
            string DestinationBagOut = "0x0000000";
            List<Organizer.OrganizerItem> result = new List<Organizer.OrganizerItem>();

            foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
            {
                if ((string)row["Name"] == name)
                {
                    SourceBagOut = (string)row["SourceBag"];
                    DestinationBagOut = (string)row["DestinationBag"];
                    result = row["List"] as List<Organizer.OrganizerItem>;
                    exit = true;
                }
            }
            SourceBag = SourceBagOut;
            DestinationBag = DestinationBagOut;
            list = result;
            return exit;
        }

        internal static void SaveOrganizerItemList(string name, List<Organizer.OrganizerItem> list, string SourceBag, string DestinationBag)
        {
            DataRow row = m_Dataset.Tables["ORGANIZER_LISTS"].NewRow();
            row["Name"] = name;
            row["List"] = list;
            row["SourceBag"] = SourceBag;
            row["DestinationBag"] = DestinationBag;
            m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Add(row);
            Save();
        }

        internal static bool LoadOrganizerGeneral(out string label, out List<string> list, out string selection)
        {
            bool exit = false;

            string labelOut = "";
            List<string> listOut = new List<string>();
            string selectionOut = "";

            if (m_Dataset.Tables["ORGANIZER_GENERAL"].Rows.Count == 1)
            {
                DataRow row = m_Dataset.Tables["ORGANIZER_GENERAL"].Rows[0];
                {
                    labelOut = (string)row["Label"];
                    listOut = row["List"] as List<string>;
                    selectionOut = (string)row["Selection"];
                    exit = true;
                }
            }

            label = labelOut;
            list = listOut;
            selection = selectionOut;

            return exit;
        }

        internal static void SaveOrganizerGeneral(string label, List<string> list, string selection)
        {
            m_Dataset.Tables["ORGANIZER_GENERAL"].Rows.Clear();
            DataRow row = m_Dataset.Tables["ORGANIZER_GENERAL"].NewRow();
            row["Label"] = label;
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

        internal static bool LoadSellItemList(string name, out List<SellAgent.SellItem> list, out string HotBag)
        {
            bool exit = false;
            string HotBagOut = "0x0000000";
            List<SellAgent.SellItem> result = new List<SellAgent.SellItem>();

            foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
            {
                if ((string)row["Name"] == name)
                {
                    HotBagOut = (string)row["HotBag"];
                    result = row["List"] as List<SellAgent.SellItem>;
                    exit = true;
                }
            }
            list = result;
            HotBag = HotBagOut;
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

        
        internal static bool LoadDressItemList(string name, out List<RazorEnhanced.Dress.DressItem> list, out string undressbag, out bool conflict)
        {
            bool exit = false;
            List<RazorEnhanced.Dress.DressItem> result = new List<RazorEnhanced.Dress.DressItem>();
            string undressbagOut = "";
            bool conflictOut = false;

            foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
            {
                if ((string)row["Name"] == name)
                {
                    undressbagOut = (string)row["UndressBag"];
                    conflictOut = (bool)row["Conflict"];
                    result = row["List"] as List<RazorEnhanced.Dress.DressItem>;
                    exit = true;
                }
            }
            list = result;
            undressbag = undressbagOut;
            conflict = conflictOut;
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