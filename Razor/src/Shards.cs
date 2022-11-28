using Newtonsoft.Json;
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
    [Serializable]
    public class Shard
    {
        [JsonIgnore]
        private static readonly string m_Save = "RazorEnhanced.shards";

        [JsonIgnore]
        private static Dictionary<string, Shard> m_Dataset;

        [JsonIgnore]
        internal static Dictionary<string, Shard> Dataset { get { return m_Dataset; } }

        [JsonProperty("Description")]
        internal string Description { get; set; }

        [JsonProperty("ClientPath")]
        internal string ClientPath { get; set; }
        
        [JsonProperty("CUOClient")] 
        internal string CUOClient { get; set; }

        [JsonProperty("ClientFolder")]
        internal string ClientFolder { get; set; }

        [JsonProperty("Host")]
        internal string Host { get; set; }
        
        [JsonProperty("Port")] 
        internal uint Port { get; set; }

        [JsonProperty("PatchEnc")]
        internal bool PatchEnc { get; set; }

        [JsonProperty("OSIEnc")]
        internal bool OSIEnc { get; set;}

        [JsonProperty("Selected")]
        internal bool Selected { get; set; }

        public Shard(string description, string clientpath, string clientfolder, string cuoClient, string host, uint port, bool patchenc, bool osienc, bool selected)
        {
            Description = description;
            ClientPath = clientpath;
            ClientFolder = clientfolder;
            CUOClient = cuoClient;
            Host = host;
            Port = port;
            PatchEnc = patchenc;
            OSIEnc = osienc;
            Selected = selected;
        }
        internal static void Load(bool tryBackup = true)
        {
            //if (m_Dataset != null)
            //  return;

            m_Dataset = new Dictionary<string, Shard>();
            string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", m_Save);
            string backup = Path.Combine(Assistant.Engine.RootPath, "Backup", m_Save);

            if (File.Exists(filename))
            {
                try
                {
                    Dictionary<string, List<Shard>> keyValuePairs = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<Shard>>>(File.ReadAllText(filename));
                    File.Copy(filename, backup, true);
                    foreach (var entry in keyValuePairs["SHARDS"])
                    {
                        m_Dataset[entry.Description] = entry;
                    }
                }
                catch (Exception)
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
                Insert("UO Eventine", String.Empty, String.Empty, String.Empty, "shard.uoeventine.com", 2593, true, false);
                Insert("OSI Ultima Online", String.Empty, String.Empty, String.Empty, "login.ultimaonline.com", 7776, true, true );
            }
        }

        internal static bool Exists(string description)
        {
            return m_Dataset.ContainsKey(description.ToLower());
        }

        internal static void Insert(string description, string clientpath, string clientfolder, string cuoClient, string host, int port, bool patchenc, bool osienc)
        {
            foreach (var entry in m_Dataset)
            {
                entry.Value.Selected = false;
            }

            Shard newEntry = new Shard(description, clientpath, clientfolder, cuoClient, host, (uint)port, patchenc, osienc, true);
            m_Dataset[newEntry.Description] = newEntry;

            Save();
        }

        internal static void Update(string description, string clientpath, string clientfolder, string cuoClient, string host, uint port, bool patchenc, bool osienc, bool selected)
        {
            if (m_Dataset.ContainsKey(description))
            {
                if (selected)
                {
                    foreach (Shard entry in m_Dataset.Values)
                    {
                        entry.Selected = false;
                    }
                }
                m_Dataset[description] = new Shard(description, clientpath, clientfolder, cuoClient, host, port, patchenc, osienc, selected);
                Save();
            }
        }

        internal static void UpdateLast(string description)
        {
            foreach (var entry in m_Dataset)
            {
                if (entry.Value.Description == description)
                    entry.Value.Selected = true;
                else
                    entry.Value.Selected = false;
            }
            Save();
        }

        internal static void Delete(string shardname)
        {
            if (m_Dataset.ContainsKey(shardname))
            {
                Shard row = m_Dataset[shardname];
                m_Dataset.Remove(shardname);
                if (row.Selected && m_Dataset.Count > 0)
                {
                    m_Dataset.FirstOrDefault().Value.Selected = true;
                }
            }
            Save();
        }

        internal static void Read(out List<RazorEnhanced.Shard> shards)
        {
            List<RazorEnhanced.Shard> shardsOut = new List<RazorEnhanced.Shard>();

            foreach (var row in m_Dataset)
            {
                var entry = row.Value;
                RazorEnhanced.Shard shard = new RazorEnhanced.Shard(entry.Description, entry.ClientPath, 
                    entry.ClientFolder, entry.CUOClient, entry.Host, entry.Port, entry.PatchEnc, entry.OSIEnc, entry.Selected);
                shardsOut.Add(shard);
            }

            shards = shardsOut;
        }

        internal static void Save()
        {
            try
            {
                Dictionary<string, List<Shard>> keyValuePairs = new Dictionary<string, List<Shard>>();
                keyValuePairs["SHARDS"] = new List<Shard>();                
                foreach (var entry in m_Dataset.Values)
                {
                    keyValuePairs["SHARDS"].Add(entry);
                }
                string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", m_Save);
                string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(keyValuePairs, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filename, serialized);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing " + m_Save + ": " + ex);
            }
        }
    }
}
