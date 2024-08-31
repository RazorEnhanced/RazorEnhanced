using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace RazorEnhanced
{

    internal static class Shards
    {
        internal static readonly string m_Save = "RazorEnhanced.shards";
        internal static AllShards allShards = new AllShards();
        public static bool AllowBeta { get { return allShards.AllowBeta; } }
        public static bool ShowLauncher
        {
            get { return allShards.ShowLauncher; }
            set { allShards.ShowLauncher = value; Shard.Save(); }
        }
    }

    [Serializable]
    internal class AllShards
    {
        [JsonProperty("AllowBeta")]
        internal bool AllowBeta = new bool();
        [JsonProperty("ShowLauncher")]
        [DefaultValue(true)]
        internal bool ShowLauncher = new bool();
        [JsonProperty("Shards")]
        internal Dictionary<string, Shard> m_Shards = new Dictionary<string, Shard>();
    }


    [Serializable]
    internal class Shard
    {
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
        internal bool OSIEnc { get; set; }

        [JsonProperty("Selected")]
        internal bool Selected { get; set; }

        internal enum StartType
        {
            OSI,
            CUO
        }
        [JsonProperty("StartClientType")]
        internal StartType StartTypeSelected { get; set; }

        public Shard(string description, string clientpath, string clientfolder, string cuoClient, string host, uint port, bool patchenc, bool osienc, bool selected, StartType startType = StartType.OSI)
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
            StartTypeSelected = startType;
        }
        internal static void Load(bool tryBackup = true)
        {
            string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", Shards.m_Save);
            string backup = Path.Combine(Assistant.Engine.RootPath, "Backup", Shards.m_Save);
            Shards.allShards = new();
            if (File.Exists(filename))
            {
                var content = File.ReadAllText(filename);
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Populate
                    };
                    Shards.allShards = Newtonsoft.Json.JsonConvert.DeserializeObject<AllShards>(content, settings);
                }
                catch (Exception)
                {
                    RazorEnhanced.UI.RE_MessageBox.Show("Unable To Load Shards",
                            $"Shard file:\r\n{filename} failed\r\nLoading default servers",
                            ok: "Ok", no: null, cancel: null, backColor: null);
                }
            }
            if (Shards.allShards.m_Shards == null || Shards.allShards.m_Shards.Count == 0)
            {
                Shards.allShards = new AllShards();
                Insert("OSI Ultima Online", String.Empty, String.Empty, String.Empty, "login.ultimaonline.com", 7776, true, true);
                Insert("UO Demise", String.Empty, String.Empty, String.Empty, "login.uogdemise.com", 2593, true, false);
                Insert("UO Eventine", String.Empty, String.Empty, String.Empty, "shard.uoeventine.com", 2593, true, false);
                Insert("UO Forever", String.Empty, String.Empty, String.Empty, "play.uoforever.com", 2599, true, false);
                Insert("UO Wolvesbane", String.Empty, String.Empty, String.Empty, "play.wolvesbaneuo.com", 2593, true, false);
                Shards.ShowLauncher = true;
            }
        }

        internal static bool Exists(string description)
        {
            return Shards.allShards.m_Shards.ContainsKey(description.ToLower());
        }

        internal static void Insert(string description, string clientpath, string clientfolder, string cuoClient, string host, int port, bool patchenc, bool osienc)
        {
            foreach (var entry in Shards.allShards.m_Shards)
            {
                entry.Value.Selected = false;
            }

            Shard newEntry = new Shard(description, clientpath, clientfolder, cuoClient, host, (uint)port, patchenc, osienc, true);
            Shards.allShards.m_Shards[newEntry.Description] = newEntry;

            Save();
        }

        internal static void Update(string description, string clientpath, string clientfolder, string cuoClient, string host, uint port, bool patchenc, bool osienc, bool selected, StartType startType = StartType.OSI)
        {
            if (Shards.allShards.m_Shards.ContainsKey(description))
            {
                if (selected)
                {
                    foreach (Shard entry in Shards.allShards.m_Shards.Values)
                    {
                        entry.Selected = false;
                    }
                }
                Shards.allShards.m_Shards[description] = new Shard(description, clientpath, clientfolder, cuoClient, host, port, patchenc, osienc, selected, startType);
                Save();
            }
        }

        internal static void UpdateLast(string description)
        {
            foreach (var entry in Shards.allShards.m_Shards)
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
            if (Shards.allShards.m_Shards.ContainsKey(shardname))
            {
                Shard row = Shards.allShards.m_Shards[shardname];
                Shards.allShards.m_Shards.Remove(shardname);
                if (row.Selected && Shards.allShards.m_Shards.Count > 0)
                {
                    foreach (var entry in Shards.allShards.m_Shards)
                    {
                        entry.Value.Selected = true;
                        break;
                    }
                }
            }
            Save();
        }

        internal static List<RazorEnhanced.Shard> Read()
        {
            List<RazorEnhanced.Shard> returnList = new List<RazorEnhanced.Shard>();
            foreach (var entry in Shards.allShards.m_Shards)
            {
                returnList.Add(entry.Value);
            }
            return returnList;
        }

        internal static void Save()
        {
            string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", Shards.m_Save);
            try
            {
                string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(Shards.allShards, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filename, serialized);
            }
            catch (Exception ex)
            {
                var dialogResult = RazorEnhanced.UI.RE_MessageBox.Show("Unable To Save Shards",
                    $"Shard file:\r\n{filename}\r\nFailed with error:\r\n{ex}",
                    ok: "Ok", no: null, cancel: null, backColor: null);
            }
        }
    }
}
