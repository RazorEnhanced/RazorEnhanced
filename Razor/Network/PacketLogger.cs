using Accord.Math;
using Accord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static RazorEnhanced.PacketLogger;

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

        private List<PacketPath> m_PacketPaths = new List<PacketPath> { PacketPath.ClientToServer, PacketPath.ServerToClient };

        public bool DiscardAll = false;
        public bool DiscardShowHeader = false;
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
        public PacketPath[] ActivePacketPaths() { return m_PacketPaths.ToArray(); }



        public PacketLogger(string OutputPath)
        {
            this.OutputPath = OutputPath;
        }
        
        public string StartRecording(bool appendLogs = false)
        {
            if (Assistant.Client.Instance.AllowBit(FeatureBit.PacketAgent))
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

        public void ListenPacketPath(PacketPath path, bool active=true)
        {
            if (active && !m_PacketPaths.Contains(path))
            {
                m_PacketPaths.Add(path);
            }
            else if(!active && m_PacketPaths.Contains(path))
            {
                m_PacketPaths.Remove(path);
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
            var packet = RazorEnhanced.PacketLogger.TemplateParser.parse(template,packetData);
            var jsonDump = JsonConvert.SerializeObject(packet, Formatting.Indented);
            sw.WriteLine(jsonDump);
            return !template.showHexDump;
        }
        
        public void LogPacketData(PacketPath path, byte[] packetData, bool blocked = false)
        {
            EventManager.Instance.didRecievePacket(path, packetData);

            if (!m_Active) return;
            if (!m_PacketPaths.Contains(path)) { return; }

            var packetLen = packetData.Length;
            if (packetLen == 0) return;

            int packetID = packetData[0];
            var shouldDiscardW = DiscardAll && !m_PacketWhitelist.Contains(packetID);
            var shouldDiscardB = !DiscardAll && m_PacketBlacklist.Contains(packetID);
            var shouldDiscard = shouldDiscardW || shouldDiscardB;
            if (shouldDiscard && !DiscardShowHeader) { return; }

            var pathStr = _packetPathOutput(path);

            try
            {
                using (StreamWriter sw = new StreamWriter(m_OutputPath, true))
                {
                    sw.AutoFlush = true;

                    sw.WriteLine("{0}: {1}{2}0x{3:X2} (Length: {4})", DateTime.Now.ToString("HH:mm:ss.ffff"), pathStr, blocked ? " [BLOCKED] " : " ", packetID, packetLen);
                    if (shouldDiscard) { return; }
                    
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

        private string _packetPathOutput(PacketPath path)
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



        public void Reset()
        {
            StopRecording();
            m_PacketBlacklist.Clear();
            m_PacketWhitelist.Clear();
            m_PacketTemplates.Clear();
            m_PacketPaths.Clear();
            m_PacketPaths.Add(PacketPath.ClientToServer);
            m_PacketPaths.Add(PacketPath.ServerToClient);
            OutputPath = DEFAULT_LOG_OUTPATH;
        }




    }

}
