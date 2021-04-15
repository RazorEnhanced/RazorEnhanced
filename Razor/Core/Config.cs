using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorEnhanced
{
    class Config
    {
        public static readonly string PATH_CONFIG_DOORS = "./Config/doors.json";
        public static Doors Doors;




        public static void load() {
            try
            {
                var json_txt = File.ReadAllText(PATH_CONFIG_DOORS);
                Doors = JsonConvert.DeserializeObject<Doors>(json_txt);
            }
            catch { }
        }

    }



    [Serializable]
    class DoorID { public int opened, closed; }
    [Serializable]
    class DoorsCategory { public String name; public List<DoorID> door; }
    [Serializable]
    class Doors { public List<DoorsCategory> doors;  }

}
