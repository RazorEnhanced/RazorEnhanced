using Accord.Math;
using Accord;
using Newtonsoft.Json;
using RazorEnhanced;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Assistant
{
    public class PacketLogger
    {
        //internal static readonly string DEFAULT_LOG_DIR = Misc.ScriptDirectory() + "\\log\\";
        public static readonly string DEFAULT_LOG_DIR = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string DEFAULT_LOG_FILE = "Razor_Packets.log";
        public static readonly string DEFAULT_LOG_OUTPATH = Path.Combine(DEFAULT_LOG_DIR, DEFAULT_LOG_FILE);

        // Class/Static Methods
        public readonly static PacketLogger SharedInstance = new PacketLogger(DEFAULT_LOG_OUTPATH);

        private Dictionary<int, PacketTemplate> m_PacketTemplates = new Dictionary<int, PacketTemplate>();

        public bool DiscardAll = false;
        private List<int> m_PacketWhitelist = new List<int>();
        private List<int> m_PacketBlacklist = new List<int>();

        private bool m_Active = false;

        private string m_BaseDir;
        private string m_FileName;
        private string m_OutputPath;

        

        public bool Active { get => m_Active; }

        public string BaseDir { get => m_BaseDir; }
        public string FileName { get => m_FileName; }
        public string OutputPath {
            get => m_OutputPath;
            set {
                if (m_Active) { StopRecording(); }
                m_OutputPath = value;
                m_FileName = Path.GetFileName(m_OutputPath);
                m_BaseDir = Path.GetDirectoryName(m_OutputPath);
            }
        }

        public static string PacketPathText(PacketPath path)
        {
            switch (path)
            {
                case PacketPath.ClientToServer: return "Client -> Server";
                case PacketPath.ServerToClient: return "Server -> Client";
                case PacketPath.RazorToServer: return "Razor -> Server";
                case PacketPath.RazorToClient: return "Razor -> Client";
                case PacketPath.PacketVideo: return "PacketVideo -> Client";
            }
            return "Unknown -> Unknown";
        }

        public PacketLogger(string OutputPath)
        {
            this.OutputPath = OutputPath;
        }
        
        public string StartRecording(bool appendLogs = false)
        {
            if (m_Active) return m_OutputPath;
            m_Active = true;
            

            Directory.CreateDirectory(BaseDir);
            if (appendLogs == false)
            {
                File.Create(m_OutputPath).Dispose(); // create-truncate file
            }

            using (StreamWriter sw = new StreamWriter(m_OutputPath, true))
            {
                sw.AutoFlush = true;
                sw.WriteLine("\n\n");
                sw.WriteLine(">>>>>>>>>> Logging START {0} >>>>>>>>>>", DateTime.Now);
                sw.WriteLine("\n\n");
            }

            return m_OutputPath;
        }

        public string StopRecording()
        {
            if (!m_Active) return m_OutputPath;
            m_Active = false;

            using (StreamWriter sw = new StreamWriter(m_OutputPath, true))
            {
                sw.AutoFlush = true;
                sw.WriteLine("\n\n");
                sw.WriteLine("<<<<<<<<<< Logging END {0} <<<<<<<<<<", DateTime.Now);
                sw.WriteLine("\n\n");
            }
            return m_OutputPath;
        }

        public void LogString(string line)
        {
            if (!m_Active)
                return;

            try
            {
                using (StreamWriter sw = new StreamWriter(m_OutputPath, true))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(line);
                }
            }
            catch
            {
            }
        }

        public void AddBlacklist(int packetID)
        {
            m_PacketBlacklist.Add(packetID);
        }

        public void RemoveBlacklist(int packetID = -1)
        {
            if (packetID == -1)
            {
                m_PacketBlacklist.Clear();
                return;
            }
            m_PacketBlacklist.Remove(packetID);
        }

        public void AddWhitelist(int packetID)
        {
            m_PacketWhitelist.Add(packetID);
        }

        public void RemoveWhitelist(int packetID = -1)
        {
            if (packetID == -1)
            {
                m_PacketWhitelist.Clear();
                return;
            }
            m_PacketWhitelist.Remove(packetID);
        }


        public void AddTemplate(string jsonTemplate)
        {
            var packetTemplate = JsonConvert.DeserializeObject<PacketTemplate>(jsonTemplate);
            AddTemplate(packetTemplate);
        }

        public void AddTemplate(PacketTemplate packetTemplate)
        {
            m_PacketTemplates[packetTemplate.packetID] = packetTemplate;
        }

        public void RemoveTemplate(int packetID)
        {
            m_PacketTemplates.Remove(packetID);
        }

        public void RemoveTemplate(string packetTemplate)
        {
            var template = JsonConvert.DeserializeObject<PacketTemplate>(packetTemplate);
            RemoveTemplate(template.packetID);
        }

        public void RemoveTemplates()
        {
            m_PacketTemplates.Clear();
        }

        protected bool DisplayTemplate(StreamWriter sw, byte[] packetData)
        {
            var packetID = packetData[0];
            if (!m_PacketTemplates.Keys.Contains(packetID)){
                return false;
            }

            PacketTemplate template = m_PacketTemplates[packetID];
            var packet = template.parse(packetData);
            var jsonDump = JsonConvert.SerializeObject(packet, Formatting.Indented);
            sw.WriteLine(jsonDump);
            return !template.showHexDump;
        }


        public void LogPacketData(PacketPath path, byte[] packetData, bool blocked = false)
        {
            if (!m_Active) return;
            var packetLen = packetData.Length;
            if (packetLen == 0) return;

            int packetID = packetData[0];
            if (DiscardAll && !m_PacketWhitelist.Contains(packetID)) { return; }
            if (m_PacketBlacklist.Contains(packetID)) { return; }

            var pathStr = PacketPathText(path);

            try
            {
                using (StreamWriter sw = new StreamWriter(m_OutputPath, true))
                {
                    sw.AutoFlush = true;

                    sw.WriteLine("{0}: {1}{2}0x{3:X2} (Length: {4})", DateTime.Now.ToString("HH:mm:ss.ffff"), pathStr, blocked ? " [BLOCKED] " : " ", packetID, packetLen);
                    
                    var showHexDump = !DisplayTemplate(sw, packetData);
                    if (showHexDump) { 
                        Stream packetStream = new MemoryStream(packetData);
                        Utility.FormatBuffer(sw, packetStream, (int)packetStream.Length);
                    }
                    sw.WriteLine();
                }
            }
            catch
            {
            }
        }

        public void Reset()
        {
            StopRecording();
            m_PacketBlacklist.Clear();
            m_PacketWhitelist.Clear();
            m_PacketTemplates.Clear();
            OutputPath = DEFAULT_LOG_OUTPATH;
        }

    }




    /* PACKET TEMPLATE */
    /*
        {
           version: 1,
           packetID: 0x01,
           name: "Test",
           fields:[
               {name:"", length:""},
               {name:"", type:""},
               {name:"", type:"", length:"" },
               {name:"", type:"", fields:[
                   {name:"", type:"", },
                   {name:"", type:"", },
               ]},
               {name:"", type:""},
           ]
        }
        */

    /*
     {
        version: 1,
        packetID: 0x0B,
        name: "Damage",
        fields:[
            {name:"packetID", length:1, type="hex"},
            {name:"Serial", length:4, type="hex"},
            {name:"Damage", length:2, type="num"},
        ]
     }
     */

    public class PacketTemplate
    {
        public int version = 1;
        public int packetID;
        public string name = "";
        public bool dynamicLength = false;
        public bool showHexDump = false;
        public List<PacketTemplateField> fields;

        public Dictionary<string, dynamic> parse(byte[] packetData)
        {
            var packetReader = new PacketReader(packetData, dynamicLength);
            return parse(packetReader);
        }

        public Dictionary<string, dynamic> parse(PacketReader packetReader)
        {
            Dictionary<string, dynamic> fieldsObject = new Dictionary<string, dynamic>();
            foreach (var field in fields)
            {
                var fieldObj = field.parse(packetReader);
                if (fieldObj != null)
                {
                    fieldsObject.Add(field.name, fieldObj);
                }
            }
            Dictionary<string, dynamic> packetObject = new Dictionary<string, dynamic>{
                //{ "version", version },
                { "name", name },
                { "packetID", "0x{0:X2}".Format(packetID) },
                { "fields", fieldsObject }
            };
            return packetObject;
        }
    }

    public class PacketTemplateField
    {
        public string name = "";
        public int length = -1;
        public string type = "hex";
        public List<PacketTemplateField> fields;
        public PacketTemplate subpacket;

        

        public dynamic parse(PacketReader packetReader)
        {

            if (subpacket != null)
            {
                return subpacket.parse(packetReader);
            }

            if (fields != null)
            {
                var fieldsObjects = fields.Apply(field => field.parse(packetReader));
                if (type == "for")
                {
                    var i = 0;
                    while (true)
                    {
                        if (packetReader.Position >= packetReader.Length) break;
                        if (length > 0 && i > length) break;
                        var fieldObjs = fields.Apply(field => field.parse(packetReader));
                        fieldsObjects.Concatenate(fieldObjs);
                        i++;
                    }
                }                                        
                return fieldsObjects;

            }

            if (type == "int")
            {
                switch (length)
                {
                    case 1: return packetReader.ReadByte();
                    case 2: return packetReader.ReadInt16();
                    case 3: return (((Int32)packetReader.ReadInt16()) << 8) | packetReader.ReadByte();
                    case 4: return packetReader.ReadInt32();
                }
            }


            if (type == "serial")
            {
                return "0x{0:X8}".Format(packetReader.ReadUInt32());
            }

            if (type == "ID")
            {
                return "0x{0:X4}".Format(packetReader.ReadUInt16());
            }

            if (type == "packetID")
            {
                return "0x{0:X2}".Format(packetReader.ReadByte());
            }

            if (type == "uint")
            {
                switch (length)
                {
                    case 1: return packetReader.ReadByte();
                    case 2: return packetReader.ReadUInt16();
                    case 3: return ((packetReader.ReadUInt16()) << 8) | packetReader.ReadByte();
                    case 4: return packetReader.ReadUInt32();
                }
            }

            if (type == "hex")
            {
                switch (length)
                {
                    case 1: return "0x{0:X2}".Format(packetReader.ReadByte());
                    case 2: return "0x{0:X4}".Format(packetReader.ReadUInt16());
                    case 3: return "0x{0:X6}".Format(((packetReader.ReadUInt16()) << 8) + packetReader.ReadByte());
                    case 4: return "0x{0:X8}".Format(packetReader.ReadUInt32());
                }
            }

            if (type == "text")
            {
                return packetReader.ReadString(length);
            }

            if (type == "utf8")
            {
                return packetReader.ReadString(length);
            }

            if (type == "boolean")
            {
                return packetReader.ReadBoolean();
            }

            if (type == "skip")
            {
                packetReader.Seek(length, SeekOrigin.Current);
                return $"skip:{length}";
            }

            if (type == "dump")
            {
                var data = packetReader.CopyBytes(packetReader.Position, length);
                packetReader.Seek(length, SeekOrigin.Current);
                var hexlist = BitConverter.ToString(data).Split('-').ToList();
                var charPerLines = 16;
                var newLines = (int)Math.Floor(hexlist.Count() / (float)charPerLines);
                for (int i = 0; i < newLines; i++)
                {
                    var pos = (newLines - i) * charPerLines;
                    hexlist.Insert(pos, "\n");
                }
                return String.Join(" ", hexlist.ToArray());
            }


            return "(null)";
        }

    }


}
