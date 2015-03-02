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

				DataTable autoloot = new DataTable("AUTOLOOT");
				autoloot.Columns.Add("Item", typeof(AutoLoot.AutoLootItem));
				m_Dataset.Tables.Add(autoloot);

				m_Dataset.AcceptChanges();
			}
		}

		internal static List<AutoLoot.AutoLootItem> LoadAutoLootItemList()
		{
			List<AutoLoot.AutoLootItem> result = new List<AutoLoot.AutoLootItem>();
			foreach (DataRow row in m_Dataset.Tables["AUTOLOOT"].Rows)
			{
				AutoLoot.AutoLootItem item = row["Item"] as AutoLoot.AutoLootItem;
				if (item != null)
				{
					result.Add(item);
				}
			}

			return result;
		}

		internal static void SaveAutoLootItemList(List<AutoLoot.AutoLootItem> list)
		{
			m_Dataset.Tables["AUTOLOOT"].Rows.Clear();
			foreach (AutoLoot.AutoLootItem item in list)
			{
				DataRow row = m_Dataset.Tables["AUTOLOOT"].NewRow();
				row["Item"] = item;
				m_Dataset.Tables["AUTOLOOT"].Rows.Add(row);
			}

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