using Accord;
using Accord.Math;
using Assistant;
using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static RazorEnhanced.PacketLogger;
using static RazorEnhanced.PacketLogger.PacketTemplate;

namespace RazorEnhanced
{
    /// <summary>
    /// RazorEnhanced packet logger.
    /// </summary>
    public class PacketLogger
    {
        public readonly static Dictionary<PacketPath, string> PathToString = new Dictionary<PacketPath, string> {
            { PacketPath.ClientToServer, "ClientToServer" },
            { PacketPath.ServerToClient, "ServerToClient" },
            { PacketPath.RazorToServer, "RazorToServer" },
            { PacketPath.RazorToClient,  "RazorToClient" },
            { PacketPath.PacketVideo, "PacketVideo" },
        };

        public readonly static Dictionary<string, PacketPath> StringToPath = new Dictionary<string, PacketPath> {
            { "ClientToServer", PacketPath.ClientToServer},
            { "ServerToClient", PacketPath.ServerToClient},
            { "RazorToServer", PacketPath.RazorToServer },
            { "RazorToClient", PacketPath.RazorToClient },
            { "PacketVideo", PacketPath.PacketVideo },
        };



        /// <summary>
        /// Set the RazorEnhanced packet logger. Calling it without a path it rester it to the default path.
        /// </summary>
        /// <param name="outputpath">(Optional) Custom output path (Default: reset to ./Desktop/Razor_Packets.log)</param>
        /// <returns>The path to the saved file.</returns>
        public static string SetOutputPath(string outputpath = null)
        {
            Assistant.PacketLogger.SharedInstance.OutputPath = outputpath != null ? outputpath : Assistant.PacketLogger.DEFAULT_LOG_OUTPATH;
            return Assistant.PacketLogger.SharedInstance.OutputPath;
        }

        /// <summary>
        /// Start the RazorEnhanced packet logger.
        /// </summary>
        /// <param name="outputpath">Custom output path (Default: ./Desktop/Razor_Packets.log)</param>
        /// <param name="appendLogs">True: Append - False: Overwrite (Default: False)</param>
        /// <returns>The path to the saved file.</returns>
        public static string Start(string outputpath = null, bool appendLogs = false)
        {
            SetOutputPath(outputpath);
            return Assistant.PacketLogger.SharedInstance.StartRecording(appendLogs);
        }

        public static string Start(bool appendLogs = false)
        {
            return Assistant.PacketLogger.SharedInstance.StartRecording(appendLogs);
        }


        /// <summary>
        /// Stop the RazorEnhanced packet logger. 
        /// </summary>
        /// <returns>The path to the saved file.</returns>
        public static string Stop()
        {
            return Assistant.PacketLogger.SharedInstance.StopRecording();
        }
        

        /// <summary>
        /// Add a custom template for RazorEnhanced packet logger.
        /// 
        /// Example of "Damage" (0x0B) packet:
        /// {
        ///  'packetID': 0x0B,
        ///  'name': 'Damage 0x0B',
        ///  'showHexDump': true,
        ///  'fields':[
        ///    { 'name':'packetID', 'length':1, 'type':'packetID'},
        ///    { 'name':'Serial', 'length':4, 'type':'serial'},
        ///    { 'name':'Damage', 'length': 2, 'type':'int'},
        ///  ]
        /// }
        /// 
        /// </summary>
        /// <param name="packetTemplate">Add a PacketTemplate, check ./Config/packets/ folder.</param>
        public static void AddTemplate(string packetTemplate)
        {
            Assistant.PacketLogger.SharedInstance.AddTemplate(packetTemplate);
        }
        /// <summary>
        /// Remove a PacketTemplate for RazorEnhanced packet logger.
        /// </summary>
        /// <param name="packetID">Remove a spacific packetID. (Default: -1 Remove All)</param>
        public static void RemoveTemplate(int packetID = -1)
        {
            if (packetID == -1)
            {
                Assistant.PacketLogger.SharedInstance.RemoveTemplates();
                return;
            }
            Assistant.PacketLogger.SharedInstance.RemoveTemplate(packetID);

        }

        /// <summary>
        /// Add the packetID to the blacklist. Packets in the backlist will not be logged. (See PacketLogger.DiscardAll() ) 
        /// </summary>
        /// <param name="packetID">PacketID to blacklist</param>
        public static void AddBlacklist(int packetID)
        {
            Assistant.PacketLogger.SharedInstance.AddBlacklist(packetID);
        }

        /// <summary>
        /// Add the packetID to the whitelist. Packets in the whitelist are always. (See PacketLogger.DiscardAll() ) 
        /// </summary>
        /// <param name="packetID">PacketID to whitelist</param>
        public static void AddWhitelist(int packetID)
        {
            Assistant.PacketLogger.SharedInstance.AddWhitelist(packetID);
        }

        /// <summary>
        /// Packet logger will discard all packets, except the one in the whitelist.  (See PacketLogger.AddWhitelist() ) 
        /// </summary>
        /// <param name="discardAll">True: Log only the packet in the whitelist - False: Log everything, but the packets in the blacklist</param>
        public static void DiscardAll(bool discardAll)
        {
            Assistant.PacketLogger.SharedInstance.DiscardAll = discardAll;
        }

        /// <summary>
        /// Packet logger will show the headers of discarded packets.
        /// </summary>
        /// <param name="showHeader">True: Always show headers - False: Hide everything.</param>
        public static void DiscardShowHeader(bool showHeader)
        {
            Assistant.PacketLogger.SharedInstance.DiscardShowHeader = showHeader;
        }

        /// <summary>
        /// Reset the packet logger to defaults.
        /// </summary>
        public static void Reset()
        {
            Assistant.PacketLogger.SharedInstance.Reset();
        }

        /// <summary>
        /// Packet logger will discard all packets, except the one in the whitelist.  (See PacketLogger.AddWhitelist() ) 
        /// If the packetPath is not set or not resognized, the function simply returns the current active paths.
        /// </summary>
        /// <param name="packetPath">
        /// Possible values:
        ///    ClientToServer
        ///    ServerToClient
        ///    RazorToServer
        ///    RazorToClient
        ///    PacketVideo
        /// </param>
        /// <returns>List of strings of currently active packet paths.</returns>
        public static string[] ListenPacketPath(string packetPath = "", bool active = true)
        {
            
            var compareKeys = StringToPath.Keys.ToList();
            var compareLower = StringToPath.Keys.Select(x => x.ToLower()).ToList();
            var matchPath = Regex.Replace(packetPath.ToLower(), "[^a-z]", "");
            var found = compareLower.IndexOf(matchPath);

            if (found != -1)
            {
                var originalKey = compareKeys[found];
                PacketPath path = StringToPath[originalKey];
                Assistant.PacketLogger.SharedInstance.ListenPacketPath(path, active);
            }
            else {
                Misc.SendMessage($"PacketLogger:ListenPacketPath: packetPath not recognized '{packetPath}'");
            }

            PacketPath[] activePaths = Assistant.PacketLogger.SharedInstance.ActivePacketPaths();
            var activeKeys = StringToPath.Where(entry => activePaths.Contains(entry.Value)).Select(entry => entry.Key);

            return activeKeys.ToArray();
        }

        /// <summary>
        /// Rapresents a general purpose template system for packets. 
        /// The templates allow to format packets in the logger, making them readable.
        /// 
        /// Example of "Damage" (0x0B) packet:
        /// 
        /// {
        ///  'packetID': 0x0B,
        ///  'name': 'Damage 0x0B',
        ///  'showHexDump': true,
        ///  'fields':[
        ///    { 'name':'packetID', 'length':1, 'type':'packetID'},
        ///    { 'name':'Serial', 'length':4, 'type':'serial'},
        ///    { 'name':'Damage', 'length': 2, 'type':'int'},
        ///  ]
        /// }
        ///
        /// </summary>
        public class PacketTemplate
        {
            /// <summary>
            /// Template version,optional
            /// </summary>
            public int version = 1;
            /// <summary>
            /// packetID, mandatory. 
            /// </summary>
            public int packetID = -1;
            /// <summary>
            /// A readable name for the packet, optional but useful.
            /// </summary>
            public string name = "";
            /// <summary>
            /// If showHexDump is true the packet logger will show also the hex dump.
            /// </summary>
            public bool showHexDump = false;
            /// <summary>
            /// Advanced settings for PacketReader. Ask Crezdba about DLLImport.Razor.IsDynLength(buff[0])
            /// </summary>
            public bool dynamicLength = false;
            /// <summary>
            /// List of fields present in this Packet.
            /// </summary>
            public List<FieldTemplate> fields;


            
        }

        /// <summary>
        /// Class representing the fields inside a packet template.
        /// Example of "Damage" (0x0B) packet:
        /// 
        /// {
        ///  'packetID': 0x0B,
        ///  'name': 'Damage 0x0B',
        ///  'showHexDump': true,
        ///  'fields':[
        ///    { 'name':'packetID', 'length':1, 'type':'packetID'},
        ///    { 'name':'Serial', 'length':4, 'type':'serial'},
        ///    { 'name':'Damage', 'length': 2, 'type':'int'},
        ///  ]
        /// }
        /// </summary>
        public class FieldTemplate
        {
            /// <summary>
            /// Dysplay Name of the field.
            /// </summary>
            public string name = "";
            /// <summary>
            /// Length in bytes. length > 0 maybe a mandatory for some FieldType.
            /// </summary>
            public int length = -1;
            /// <summary>
            /// Type of field. See FieldType for details on each type.
            /// </summary>
            public string type = FieldType.DUMP;
            /// <summary>
            /// List of subfields present in this Field.
            /// </summary>
            public List<FieldTemplate> fields;
            /// <summary>
            /// A subpacket Field.
            /// </summary>
            public PacketTemplate subpacket;

        }

        /// <summary>
        /// Type of Fields available for FieldTemplate 
        /// Example of "Damage" (0x0B) packet:
        /// 
        /// {
        ///  'packetID': 0x0B,
        ///  'name': 'Damage 0x0B',
        ///  'showHexDump': true,
        ///  'fields':[
        ///    { 'name':'packetID', 'length':1, 'type':'packetID'},
        ///    { 'name':'Serial', 'length':4, 'type':'serial'},
        ///    { 'name':'Damage', 'length': 2, 'type':'int'},
        ///  ]
        /// }
        /// </summary>
        public class FieldType
        {

            /// <summary>
            /// Common type present in every packet, packetID, length is fixed to 1 byte.
            ///            
            /// Example:
            /// {'name':'packetID', 'type':'packetID'}
            /// </summary>
            public static readonly string PACKETID = "packetID";

            /// <summary>
            /// Serial type, length is fixed to 4 bytes and is displayed as 0x hex.
            ///       
            /// Example:
            /// {'name':'Target Serial', 'type':'serial'}
            /// </summary>
            public static readonly string SERIAL = "serial";

            /// <summary>
            /// ModelID type like Item.ItemdID, Mobile.Body, etc.
            /// Length is fixed to 2 bytes and is displayed as 0x hex.
            ///       
            /// Example:
            /// {'name':'Item ID', 'type':'modelID'}
            /// {'name':'Mobile Body', 'type':'modelID'}
            /// {'name':'Static ID', 'type':'modelID'}
            /// </summary>
            public static readonly string MODELID = "modelID";

            /// <summary>
            /// Boolean type, length is fixed to 1 byte.
            ///       
            /// Example:
            /// {'name':'Paralized', 'type':'bool'}
            /// </summary>
            public static readonly string BOOL = "bool";

            /// <summary>
            /// Integers type used for positive and negative integers.
            /// Length is mandatory and can range between 1 and 4 bytes.
            ///       
            /// Example:
            /// {'name':'Z Level', 'type':'int', 'length': 2}
            /// </summary>
            public static readonly string INT = "int";

            /// <summary>
            /// Unsigned integers type used for positive integers.
            /// Length is mandatory and can range between 1 and 4 bytes.
            ///       
            /// Example:
            /// {'name':'Z Level', 'type':'uint', 'length': 2}
            /// </summary>
            public static readonly string UINT = "uint";

            /// <summary>
            /// Hex type is equivalent to unsigned integers but the contents is displayed as 0x hex.
            /// Length is mandatory and can range between 1 and 4 bytes.
            ///       
            /// Example:
            /// {'name':'Hue', 'type':'hex', 'length': 2}
            /// </summary>
            public static readonly string HEX = "hex";

            /// <summary>
            /// Text reads bytes as text.
            /// Length is mandatory.
            ///       
            /// Example:
            /// {'name':'Name', 'type':'text', 'length': 20}
            /// </summary>
            public static readonly string TEXT = "text";

            /// <summary>
            /// Text reads bytes as UTF8 text.
            /// Length is mandatory.
            ///       
            /// Example:
            /// {'name':'Pet name', 'type':'utf8', 'length': 40}
            /// </summary>
            public static readonly string UTF8 = "utf8";

            /// <summary>
            /// Skip a certain amount of data.
            /// Length is mandatory.
            ///       
            /// Example:
            /// {'name':'unused', 'type':'skip', 'length': 40}
            /// </summary>
            public static readonly string SKIP = "skip";

            /// <summary>
            /// Dump a certain amount of data as raw bytes-by-bytes HEX 
            /// Length is mandatory.
            ///       
            /// Example:
            /// {'name':'unused', 'type':'dump', 'length': 40}
            /// </summary>
            public static readonly string DUMP = "dump";

            /// <summary>
            /// A special field which denotes the beginning of a subpacket. 
            /// 'length' is ignored, 'type' is optional, 'subpacket' is mandatory.
            /// 
            /// Example:
            /// {'name':'action', 'type':'subpacket',
            ///   'subpacket':{
            ///     'name':'my subpacket'
            ///     'fields':[
            ///         ...
            ///     ]
            ///   }
            /// 
            /// }
            /// </summary>
            public static readonly string SUBPACKET = "subpacket";

            /// <summary>
            /// A special field which has subfields, useful for displaying stucts. 
            /// 'length' is ignored, 'type' is optional, 'fields' is mandatory.
            /// 
            /// Example:
            /// {'name':'Player Position', 'type':'fields',
            ///   'fields':[
            ///          {'name':'X', 'type':'uint', 'length': 2}
            ///          {'name':'Y', 'type':'uint', 'length': 2}
            ///          {'name':'Z', 'type':'uint', 'length': 1}
            ///    ]
            /// }
            /// </summary>
            public static readonly string FIELDS = "fields";

            /// <summary>
            /// A special field which has subfields, useful for displaying stucts. 
            /// 'fields' is mandatory, 'type' must be set to 'for'.
            /// 'length' > 0: subfields are collected in sequence, a fixed number of times. 
            /// 'length' <= 0: subfields are collected in sequence, until the end of the packet.
            /// 
            /// Example:
            /// {'name':'House tiles', 'type':'for',
            ///   'fields':[
            ///          {'name':'X', 'type':'uint', 'length': 2}
            ///          {'name':'Y', 'type':'uint', 'length': 2}
            ///          {'name':'Z', 'type':'uint', 'length': 1}
            ///          {'name':'staticID', 'type':'modelID'}
            ///    ]
            /// }
            /// </summary>
            public static readonly string FIELDSFOR = "for";


            /// <summary>
            /// List of valid types
            /// </summary>
            public static readonly string[] VALID_TYPES = new string[] { PACKETID, SERIAL, MODELID, BOOL, INT, UINT, HEX, TEXT, UTF8, SKIP, DUMP, SUBPACKET, FIELDS, FIELDSFOR };

            /// <summary>
            /// Check if the name of type is a valid Template filed type.
            /// </summary>
            /// <param name="typename">Name of the types</param>
            /// <returns>True: is resognized. - False: not recognized.</returns>
            public static bool IsValid(string typename)
            {
                return VALID_TYPES.Contains(typename);
            }
        }
    
        /// <summary>
        /// Given a PacketTemplate and some packet data[] it produces a structured object based on the template.
        /// </summary>
        public class TemplateParser
        {
            /// <summary>
            /// Format and structure some packet data according to a given template.
            /// </summary>
            /// <returns>A serializable object representing the parsed packet.</returns>
            public static Dictionary<string, dynamic> parse(PacketTemplate template, byte[] packetData)
            {
                var packetReader = new PacketReader(packetData, template.dynamicLength);
                return parsePacket(template, packetReader);
            }

            /// <summary>
            /// @nodoc
            /// </summary>
            protected static Dictionary<string, dynamic> parsePacket(PacketTemplate template, PacketReader packetReader)
            {
                var packetID = packetReader.ReadByte();
                packetReader.Seek(-1, SeekOrigin.Current);
                Dictionary<string, dynamic> fieldsObject = new Dictionary<string, dynamic>();
                foreach (var field in template.fields)
                {
                    var fieldObj = parseField(field, packetReader);
                    if (fieldObj != null)
                    {
                        fieldsObject.Add(field.name, fieldObj);
                    }
                }
                Dictionary<string, dynamic> packetObject = new Dictionary<string, dynamic>{
                        //{ "version", version },
                        { "name", template.name },
                        { "packetID", "0x{0:X2}".Format(packetID) },
                        { "fields", fieldsObject }
                    };
                return packetObject;
            }


            /// <summary>
            /// @nodoc
            /// </summary>
            protected static dynamic parseField(FieldTemplate field, PacketReader packetReader)
            {

                if (field.subpacket != null)
                {
                    field.type = FieldType.SUBPACKET;
                    Dictionary<string, dynamic> subpacketObject = new Dictionary<string, dynamic>{
                            { field.name, parsePacket(field.subpacket,packetReader) }
                        };
                    return subpacketObject;
                }

                if (field.fields != null)
                {
                    var subfieldObjects = field.fields.Apply(field => parseField(field, packetReader));
                    if (field.type == FieldType.FIELDSFOR)
                    {
                        var i = 0;
                        while (true)
                        {
                            if (packetReader.Position >= packetReader.Length) break;
                            if (field.length > 0 && i > field.length) break;
                            var fieldObjs = field.fields.Apply(field => parseField(field, packetReader));
                            subfieldObjects.Concatenate(fieldObjs);
                            i++;
                        }
                    }
                    else
                    {
                        field.type = FieldType.FIELDS;
                    }
                    Dictionary<string, dynamic> fieldObjects = new Dictionary<string, dynamic>{
                                { field.name, subfieldObjects },
                            };

                    return fieldObjects;

                }

                if (field.type == FieldType.INT)
                {
                    switch (field.length)
                    {
                        case 1: return packetReader.ReadByte();
                        case 2: return packetReader.ReadInt16();
                        case 3: return (((Int32)packetReader.ReadInt16()) << 8) | packetReader.ReadByte();
                        case 4: return packetReader.ReadInt32();
                    }
                }


                if (field.type == FieldType.SERIAL)
                {
                    return "0x{0:X8}".Format(packetReader.ReadUInt32());
                }

                if (field.type == FieldType.MODELID)
                {
                    return "0x{0:X4}".Format(packetReader.ReadUInt16());
                }

                if (field.type == FieldType.PACKETID)
                {
                    return "0x{0:X2}".Format(packetReader.ReadByte());
                }

                if (field.type == FieldType.UINT)
                {
                    switch (field.length)
                    {
                        case 1: return packetReader.ReadByte();
                        case 2: return packetReader.ReadUInt16();
                        case 3: return ((packetReader.ReadUInt16()) << 8) | packetReader.ReadByte();
                        case 4: return packetReader.ReadUInt32();
                    }
                }

                if (field.type == FieldType.HEX)
                {
                    switch (field.length)
                    {
                        case 1: return "0x{0:X2}".Format(packetReader.ReadByte());
                        case 2: return "0x{0:X4}".Format(packetReader.ReadUInt16());
                        case 3: return "0x{0:X6}".Format(((packetReader.ReadUInt16()) << 8) + packetReader.ReadByte());
                        case 4: return "0x{0:X8}".Format(packetReader.ReadUInt32());
                    }
                }

                if (field.type == FieldType.TEXT)
                {
                    return packetReader.ReadString(field.length);
                }

                if (field.type == FieldType.UTF8)
                {
                    return packetReader.ReadString(field.length);
                }

                if (field.type == FieldType.BOOL)
                {
                    return packetReader.ReadBoolean();
                }

                if (field.type == FieldType.SKIP)
                {
                    packetReader.Seek(field.length, SeekOrigin.Current);
                    return $"skip:{field.length}";
                }

                if (field.type == FieldType.DUMP)
                {
                    var data = packetReader.CopyBytes(packetReader.Position, field.length);
                    packetReader.Seek(field.length, SeekOrigin.Current);
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
}
