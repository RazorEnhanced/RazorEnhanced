using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public class Config
    {
        #region Defines
        public static readonly string PATH_CONFIG = "Config";
        public static readonly string PATH_DATA = "Data";
        public static readonly string CONFIG_DOORS = "doors.json";
        public static readonly string CONFIG_FOODS = "foods.json";
        public static readonly string CONFIG_REGIONS = "regions.json";
        public static readonly string CONFIG_WANDS = "wands.json";
        public static readonly string CONFIG_MAPS = "maps.json";
        public static readonly string CONFIG_SOUNDFILTERS = "soundfilters.json";
        public static readonly string CONFIG_PF_BYPASS = "pf_bypass.json";
        #endregion

        //Dalamar: See if you like it (data/config loading)
        public static string ConfigPath(string configfile, bool readData = true)
        {
            string path = Path.Combine(Assistant.Engine.RootPath, PATH_DATA, configfile);
            if (!readData || !File.Exists(path))
            {
                path = Path.Combine(Assistant.Engine.RootPath, PATH_CONFIG, configfile);
            }

            return path;
        }


        public static ConfigFiles.ConfigData Load(string configfile, Type configModel)
        {
            // Load from ./Data/ if present, otherwise ./Config/ ( in this order )
            var fullpath = ConfigPath(configfile);
            var config = ConfigFiles.Load(fullpath, configModel);
            if (config != null) { return config; } // Load success!

            // Load failed.
            var msg = String.Format("Coudn't load config file:\n{0}\n\n Please refer to Discord channel for assistance, more details:\n http://razorenhanced.net/", fullpath);
            Assistant.Utility.Logger.Error(msg);

            fullpath = ConfigPath(configfile, readData: false);  // Fallback: load Config
            config = ConfigFiles.Load(fullpath, configModel);
            if (config == null)
            {
                var result = RazorEnhanced.UI.RE_MessageBox.Show("Config File Failure",
                    "Failed to load original Config file",
                    ok: "Ok", no: null, cancel: null, backColor: null);
                Application.Exit();
            }

            return config; // Return config on success
        }

        internal static void buildFacet(ConfigFiles.RegionByArea.Facet facet, ConfigFiles.Regions.Facet oldFacet)
        {

            {
                foreach (KeyValuePair<string, List<ConfigFiles.Regions.Rectangle>> pair in oldFacet.Towns)
                {
                    ConfigFiles.RegionByArea.Area area = new ConfigFiles.RegionByArea.Area(pair.Key);
                    foreach (ConfigFiles.Regions.Rectangle aRect in pair.Value)
                    {
                        area.rect.Add(new System.Drawing.Rectangle(aRect.X, aRect.Y, aRect.Width, aRect.Height));
                    }
                    area.zoneName = "Towns";
                    area.guarded = true;
                    facet.areas.Add(area);
                }
            }
            {
                foreach (KeyValuePair<string, List<ConfigFiles.Regions.Rectangle>> pair in oldFacet.Dungeons)
                {
                    ConfigFiles.RegionByArea.Area area = new ConfigFiles.RegionByArea.Area(pair.Key);
                    foreach (ConfigFiles.Regions.Rectangle aRect in pair.Value)
                    {
                        area.rect.Add(new System.Drawing.Rectangle(aRect.X, aRect.Y, aRect.Width, aRect.Height));
                    }
                    area.zoneName = "Dungeons";
                    area.guarded = false;
                    facet.areas.Add(area);
                }
            }
            {
                foreach (KeyValuePair<string, List<ConfigFiles.Regions.Rectangle>> pair in oldFacet.Guarded)
                {
                    ConfigFiles.RegionByArea.Area area = new ConfigFiles.RegionByArea.Area(pair.Key);
                    foreach (ConfigFiles.Regions.Rectangle aRect in pair.Value)
                    {
                        area.rect.Add(new System.Drawing.Rectangle(aRect.X, aRect.Y, aRect.Width, aRect.Height));
                    }
                    area.zoneName = "Guarded";
                    area.guarded = true;
                    facet.areas.Add(area);
                }
            }
            {
                foreach (KeyValuePair<string, List<ConfigFiles.Regions.Rectangle>> pair in oldFacet.Forest)
                {
                    ConfigFiles.RegionByArea.Area area = new ConfigFiles.RegionByArea.Area(pair.Key);
                    foreach (ConfigFiles.Regions.Rectangle aRect in pair.Value)
                    {
                        area.rect.Add(new System.Drawing.Rectangle(aRect.X, aRect.Y, aRect.Width, aRect.Height));
                    }
                    area.zoneName = "Forest";
                    area.guarded = false;
                    facet.areas.Add(area);
                }
            }
        }


        public static void LoadAll()
        {
            // TODO: add more
            ConfigFiles.PF_Bypass.Data = (ConfigFiles.PF_Bypass)Load(CONFIG_PF_BYPASS, typeof(ConfigFiles.PF_Bypass));
            ConfigFiles.Doors.Data = (ConfigFiles.Doors)Load(CONFIG_DOORS, typeof(ConfigFiles.Doors));
            ConfigFiles.Foods.Data = (ConfigFiles.Foods)Load(CONFIG_FOODS, typeof(ConfigFiles.Foods));
            ConfigFiles.Regions.Data = (ConfigFiles.Regions)Load(CONFIG_REGIONS, typeof(ConfigFiles.Regions));
            //
            ConfigFiles.RegionByArea.AllFacets.Add(0, new ConfigFiles.RegionByArea.Facet("Felucca"));
            buildFacet(ConfigFiles.RegionByArea.AllFacets[0], ConfigFiles.Regions.Data.Felucca);
            //
            ConfigFiles.RegionByArea.AllFacets.Add(1, new ConfigFiles.RegionByArea.Facet("Trammel"));
            buildFacet(ConfigFiles.RegionByArea.AllFacets[1], ConfigFiles.Regions.Data.Trammel);
            //
            ConfigFiles.RegionByArea.AllFacets.Add(2, new ConfigFiles.RegionByArea.Facet("Ilshenar"));
            buildFacet(ConfigFiles.RegionByArea.AllFacets[2], ConfigFiles.Regions.Data.Ilshenar);
            //
            ConfigFiles.RegionByArea.AllFacets.Add(3, new ConfigFiles.RegionByArea.Facet("Malas"));
            buildFacet(ConfigFiles.RegionByArea.AllFacets[3], ConfigFiles.Regions.Data.Malas);
            //
            ConfigFiles.RegionByArea.AllFacets.Add(4, new ConfigFiles.RegionByArea.Facet("Tokuno"));
            buildFacet(ConfigFiles.RegionByArea.AllFacets[4], ConfigFiles.Regions.Data.Tokuno);
            //
            //ConfigFiles.RegionByArea.AllFacets.Add(5, new ConfigFiles.RegionByArea.Facet("TerMur"));
            //buildFacet(ConfigFiles.RegionByArea.AllFacets[5], ConfigFiles.Regions.Data.TerMur);
            //

            ConfigFiles.Wands.Data = (ConfigFiles.Wands)Load(CONFIG_WANDS, typeof(ConfigFiles.Wands));

            ConfigFiles.Maps.Data = (ConfigFiles.Maps)Load(CONFIG_MAPS, typeof(ConfigFiles.Maps));

            string path = ConfigPath(CONFIG_SOUNDFILTERS);
            try
            {
                ConfigFiles.FilterSounds = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ushort[]>>(File.ReadAllText(path));
            }
            catch (Exception)
            {
                var result = RazorEnhanced.UI.RE_MessageBox.Show("SoundFilter File Failure",
                    "Failed to load Sound Filter file: {path}\r\nSounds may not behave as you expect",
                    ok: "Ok", no: null, cancel: null, backColor: null);
            }
        }


        public class Foods
        {
            //Lazy caching
            private static List<int> m_FishIDs;

            //Dalamar: example of high level function
            public static bool IsFish(int ItemID)
            {
                //Lazy caching
                if (m_FishIDs == null)
                {
                    var shared_foods = ConfigFiles.Foods.Data;
                    var fish_ids_txt = shared_foods.Fish.Values.ToList();
                    m_FishIDs = fish_ids_txt.Select(x => int.Parse(x)).ToList();
                }
                return m_FishIDs.Contains(ItemID);
            }
        }

        public class Doors
        {
            public static int[] OpenDoors()
            {
                var result = new List<int>();
                //TODO: filter only open doors
                return result.ToArray();
            }
        }
    }


    //other possible names: Serializables, Models, ConfigFiles, DataStorage
    public class ConfigFiles
    {
        public static Dictionary<string, ushort[]> FilterSounds = new Dictionary<string, ushort[]>();

        public static ConfigData Load(string fullpath, Type configModel)
        {
            try
            {
                var json_text = File.ReadAllText(fullpath);
                return (ConfigData)JsonConvert.DeserializeObject(json_text, configModel);
            }
            catch (Exception e)
            {
                //TODO: show sensible output
                var msg = String.Format("ConfigData.Load FAILED:\nPATH: {0}\nMESSAGE: {1}\n", fullpath, e.Message);
                Console.Out.Write(msg);
                Misc.SendMessage(msg, 138);
            }
            return null;
        }


        //Serializable classes
        // common class/loader
        public class ConfigData
        {
            public static ConfigData Data = new ConfigData();
        }

        //wands.json
        public class Wands : ConfigData
        {
            new public static Wands Data = new Wands();
            public List<int> WandIDs;
        }

        //maps.json
        public class Maps : ConfigData
        {
            new public static Maps Data = new Maps();
            public List<uint> MapIDs;
        }

        //regions.json
        public class RegionByArea : ConfigData
        {
            public class Area
            {
                public string zoneName;
                public string areaName;
                public List<System.Drawing.Rectangle> rect;
                public bool guarded;

                public Area(string name)
                {
                    areaName = name;
                    rect = new List<System.Drawing.Rectangle>();
                    guarded = true;
                }
            }

            public class Facet
            {
                public string facetName;
                public List<Area> areas;

                public Facet(string name)
                {
                    facetName = name;
                    areas = new List<Area>();
                }

            }

            public static Dictionary<int, Facet> AllFacets = new Dictionary<int, Facet>();
        }

        public class Regions : ConfigData
        {
            new public static Regions Data = new Regions();

            public Facet Trammel;
            public Facet Felucca;
            public Facet Ilshenar;
            public Facet Malas;
            public Facet Tokuno;
            public Facet TerMur; //Not implementeds


            public class Facet { public Dictionary<string, List<Rectangle>> Towns, Dungeons, Guarded, Forest; };
            public class Rectangle { public int X, Y, Z, Width, Height; public bool Guarded = true; }
        }

        //foods.json
        public class Foods : ConfigData
        {
            new public static Foods Data = new Foods();

            public Dictionary<string, string> Fish;
            public Dictionary<string, string> FruitsAndVegetables;
            public Dictionary<string, string> Meat;
        }

        //doors.json
        public class Doors : ConfigData
        {
            new public static Doors Data = new Doors();
            public Dictionary<string, List<DoorID>> DoorType;
            public class DoorID { public int Open, Close; }

        }

        //pathfinding.json
        public class PF_Bypass : ConfigData
        {
            new public static PF_Bypass Data = new PF_Bypass();
            public List<int> BypassIDs;

        }

        //Keyboard key mappers
        internal class KeyMap : Config
        {
            private Dictionary<int, int> _keyMap = new Dictionary<int, int>();
            private readonly string _filePath;
            private readonly FileSystemWatcher _fileWatcher;
            private readonly object _lock = new object();

            internal Dictionary<int, int> Data
            {
                get
                {
                    lock (_lock)
                    { return _keyMap; }
                }
            }

            internal KeyMap()
            {
                string mapName = "WindowsKeyMap.json";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    mapName = "LinuxKeyMap.json";
                }
                _filePath = ConfigPath(mapName);

                LoadKeyMap();

                _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_filePath), Path.GetFileName(_filePath))
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                _fileWatcher.Changed += OnFileChanged;

            }
            private void LoadKeyMap()
            {
                lock (_lock)
                {
                    _keyMap.Clear();

                    if (File.Exists(_filePath))
                    {
                        string jsonData = File.ReadAllText(_filePath);
                        _keyMap = JsonConvert.DeserializeObject<Dictionary<int, int>>(jsonData);
                    }
                }

            }
            private void OnFileChanged(object sender, FileSystemEventArgs e)
            {
                LoadKeyMap();
            }

        }
    }

}
