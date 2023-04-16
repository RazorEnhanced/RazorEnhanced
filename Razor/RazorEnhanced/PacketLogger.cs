using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorEnhanced
{
    /// <summary>
    /// RazorEnhanced packet logger.
    /// </summary>
    public class PacketLogger                                                         
    {
        /// <summary>
        /// Set the RazorEnhanced packet logger. Calling it without a path it rester it to the default path.
        /// </summary>
        /// <param name="outputpath">(Optional) Custom output path (Default: reset to ./Desktop/Razor_Packets.log)</param>
        /// <returns>The path to the saved file.</returns>
        public static string SetOutputPath(string outputpath=null)
        {
            Assistant.PacketLogger.SharedInstance.OutputPath = outputpath != null ? outputpath : Assistant.PacketLogger.DEFAULT_LOG_OUTPATH;
            return Assistant.PacketLogger.SharedInstance.OutputPath;
        }

        /// <summary>
        /// Start the RazorEnhanced packet logger.
        /// </summary>
        /// <param name="outputpath">Custom output path (Default: ./Desktop/Razor_Packets.log)</param>
        /// <param name="appendLogs">True: Append logs - False: empty and override (Default: false)</param>
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
        ///{
        ///  'packetID': 0x0B,
        ///  'name': 'Damage 0x0B',
        ///  'showHexDump': true,
        ///  'fields':[
        ///    { 'name':'packetID', 'length':1, 'type':'packetID'},
        ///    { 'name':'Serial', 'length':4, 'type':'serial'},
        ///    { 'name':'Damage', 'length': 2, 'type':'int'},
        ///  ]
        ///}
        /// 
        /// </summary>
        /// <param name="packetTemplate">Remove a PacketTemplate, check <RE_ROOT>/Config/packets/ folder.</param>
        public static void AddTemplate(string packetTemplate)
        {
            Assistant.PacketLogger.SharedInstance.AddTemplate(packetTemplate);
        }
        /// <summary>
        /// Remove a custom template for RazorEnhanced packet logger.
        /// </summary>
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
    }
}
