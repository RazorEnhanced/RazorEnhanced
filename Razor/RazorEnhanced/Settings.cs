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
                m_Dataset.Tables.Add(scavenger_general);
                

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

		internal static bool LoadAutoLootGeneral(out string label, out List<string> list, out string selection)
		{
			bool exit = false;

			string labelOut = "";
			List<string> listOut = new List<string>();
			string selectionOut = "";

			if (m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows.Count == 1)
			{
				DataRow row = m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows[0];
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

		internal static void SaveAutoLootIGeneral(string label, List<string> list, string selection)
		{
			m_Dataset.Tables["AUTOLOOT_GENERAL"].Rows.Clear();
			DataRow row = m_Dataset.Tables["AUTOLOOT_GENERAL"].NewRow();
			row["Label"] = label;
			row["List"] = list;
			row["Selection"] = selection;
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

        internal static bool LoadScavengerGeneral(out string label, out List<string> list, out string selection)
        {
            bool exit = false;

            string labelOut = "";
            List<string> listOut = new List<string>();
            string selectionOut = "";

            if (m_Dataset.Tables["SCAVENGER_GENERAL"].Rows.Count == 1)
            {
                DataRow row = m_Dataset.Tables["SCAVENGER_GENERAL"].Rows[0];
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

        internal static void SaveScavengerGeneral(string label, List<string> list, string selection)
        {
            m_Dataset.Tables["SCAVENGER_GENERAL"].Rows.Clear();
            DataRow row = m_Dataset.Tables["SCAVENGER_GENERAL"].NewRow();
            row["Label"] = label;
            row["List"] = list;
            row["Selection"] = selection;
            m_Dataset.Tables["SCAVENGER_GENERAL"].Rows.Add(row);
            Save();
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