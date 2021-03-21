using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class Shard
	{
		private static string m_Save = "RazorEnhanced.shards";

		private static DataSet m_Dataset;
		internal static DataSet Dataset { get { return m_Dataset; } }

		internal static void Load(bool tryBackup=true)
		{
			//if (m_Dataset != null)
			//	return;

			m_Dataset = new DataSet();
			string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", m_Save);
			string backup = Path.Combine(Assistant.Engine.RootPath, "Backup", m_Save);

			if (File.Exists(filename))
			{
				try
				{
					m_Dataset = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(File.ReadAllText(filename));
                    DataTableCollection tables = m_Dataset.Tables;

                    DataTable shards = m_Dataset.Tables["SHARDS"];
                    if (!shards.Columns.Contains("CUOClient"))
                    {
                        shards.Columns.Add("CUOClient", typeof(string));
                        foreach (DataRow row in shards.Rows)
                        {
                            row["CUOClient"] = string.Empty;
                        }
                    }

                    File.Copy(filename, backup, true);
				}
				catch (Exception e)
				{
					if (tryBackup)
					{
						MessageBox.Show("Error loading " + m_Save + ", Try to restore from backup!");
						File.Copy(backup, filename, true);
						Load(false);
					}
					else
					{
						throw;
					}
					return;
				}
			}
			else
			{
				// ----------- SHARDS ----------
				DataTable shards = new DataTable("SHARDS");
				shards.Columns.Add("Description", typeof(string)); // Key
				shards.Columns.Add("ClientPath", typeof(string));
				shards.Columns.Add("ClientFolder", typeof(string));
                shards.Columns.Add("CUOClient", typeof(string));
                shards.Columns.Add("Host", typeof(string));
				shards.Columns.Add("Port", typeof(long));
				shards.Columns.Add("PatchEnc", typeof(bool));
				shards.Columns.Add("OSIEnc", typeof(bool));
				shards.Columns.Add("Selected", typeof(bool));

				DataRow uod = shards.NewRow();
				uod.ItemArray = new object[] { "OSI Ultima Online", String.Empty, String.Empty, String.Empty, "login.ultimaonline.com", 7776, true, true, true };
				shards.Rows.Add(uod);

                DataRow eventine = shards.NewRow();
                eventine.ItemArray = new object[] { "UO Eventine", String.Empty, String.Empty, String.Empty, "shard.uoeventine.com", 2593, true, false, false };
                shards.Rows.Add(eventine);

                m_Dataset.Tables.Add(shards);


                m_Dataset.AcceptChanges();
			}
		}

		private string m_Description;
		internal string Description { get { return m_Description; } }


		internal string ClientPath { get; set; }
        internal string CUOClient { get; set; }

        //private string m_ClientFolder;
        internal string ClientFolder { get; set; }

		private string m_Host;
		internal string Host { get { return m_Host; } }

		private int m_Port;
		internal int Port { get { return m_Port; } }

		private bool m_PatchEnc;
		internal bool PatchEnc { get { return m_PatchEnc; } }

		private bool m_OSIEnc;
		internal bool OSIEnc { get { return m_OSIEnc; } }

		private bool m_Selected;
		internal bool Selected { get { return m_Selected; } }

		public Shard(string description, string clientpath, string clientfolder, string cuoClient, string host, int port, bool patchenc, bool osienc, bool selected)
		{
			m_Description = description;
			ClientPath = clientpath;
			ClientFolder = clientfolder;
            CUOClient = cuoClient;
			m_Host = host;
			m_Port = port;
			m_PatchEnc = patchenc;
			m_OSIEnc = osienc;
			m_Selected = selected;
		}

		internal static bool Exists(string description)
		{
			return m_Dataset.Tables["SHARDS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
		}

		internal static void Insert(string description, string clientpath, string clientfolder, string cuoClient, string host, int port, bool parchenc, bool osienc)
		{
			foreach (DataRow row in m_Dataset.Tables["SHARDS"].Rows)
			{
				row["Selected"] = false;
			}

			DataRow newRow = m_Dataset.Tables["SHARDS"].NewRow();
			newRow["Description"] = description;
			newRow["ClientPath"] = clientpath;
			newRow["ClientFolder"] = clientfolder;
            newRow["CUOClient"] = cuoClient;
			newRow["Host"] = host;
			newRow["Port"] = port;
			newRow["PatchEnc"] = parchenc;
			newRow["OSIEnc"] = osienc;
			newRow["Selected"] = true;
			m_Dataset.Tables["SHARDS"].Rows.Add(newRow);

			Save();
		}

		internal static void Update(string description, string clientpath, string clientfolder, string cuoClient, string host, int port, bool parchenc, bool osienc, bool selected)
		{
			bool found = m_Dataset.Tables["SHARDS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

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
                        row["CUOClient"] = cuoClient;
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
                string cuoClient = (string)row["CUOClient"];
                string host = (string)row["Host"];
                long testPort = (long)row["Port"];
                int port = (int)testPort;
                bool patchenc = (bool)row["PatchEnc"];
                bool osienc = (bool)row["OSIEnc"];
                bool selected = (bool)row["Selected"];

                RazorEnhanced.Shard shard = new RazorEnhanced.Shard(description, clientpath, clientfolder, cuoClient, host, port, patchenc, osienc, selected);
                shardsOut.Add(shard);
            }

			shards = shardsOut;
		}

		internal static void Save()
		{
			try
			{
				m_Dataset.AcceptChanges();

				string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", m_Save);
				File.WriteAllText(filename, Newtonsoft.Json.JsonConvert.SerializeObject(m_Dataset, Newtonsoft.Json.Formatting.Indented));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error writing " + m_Save + ": " + ex);
			}
		}
	}
}
