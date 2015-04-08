using System;
using System.Collections;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced
{
    public class GumpInspector
    {
        internal static void GumpResponseAddLog(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;

            AddLog("----------- Response Recevied START -----------");

            Assistant.Serial ser = p.ReadUInt32();
            AddLog("Gump Operation:" + ser.ToString());
            
            uint gid = p.ReadUInt32();
            AddLog("Gump ID: 0x" + gid.ToString("X8"));
            
            int bid = p.ReadInt32();
            AddLog("Gump Button:" + bid.ToString());

            int sc = p.ReadInt32();
            if (sc < 0 || sc > 2000)
            {
                AddLog("----------- Response Recevied END -----------");
                return;
            }

            int[] switches = new int[sc];
            for (int i = 0; i < sc; i++)
            {
                switches[i] = p.ReadInt32();
                AddLog("Switch ID: " + i + " Value: +" + switches[i].ToString());
            }

            int ec = p.ReadInt32();
            if (ec < 0 || ec > 2000)
            {
                AddLog("----------- Response Recevied END -----------");
                return;
            }

            for (int i = 0; i < ec; i++)
            {
                ushort id = p.ReadUInt16();
                ushort len = p.ReadUInt16();
                if (len >= 240)
                    return;
                string text = p.ReadUnicodeStringSafe(len);
                AddLog("Text ID: " + i + " String: +" + text);
            }
            AddLog("----------- Response Recevied END -----------");
        }

        internal static void GumpCloseAddLog(Packet p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;
            AddLog("----------- Close Recevied START -----------");
            ushort ext = p.ReadUInt16(); // Scarto primo uint
            uint gid = p.ReadUInt32();
            AddLog("Gump ID: 0x" + gid.ToString("X8"));
            int bid = p.ReadInt32();
            AddLog("Gump Close Button:" + bid.ToString());
            AddLog("----------- Close Recevied END -----------");
        }

        internal static void NewGumpStandardAddLog(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;

            AddLog("----------- New Recevied START -----------");
            
            Assistant.Serial ser = p.ReadUInt32();
            AddLog("Gump Operation:" + ser.ToString());

            uint gid = p.ReadUInt32();
            AddLog("Gump ID: 0x" + gid.ToString("X8"));

            AddLog("----------- New Recevied END -----------");
        }


        internal static void NewGumpCompressedAddLog(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;

            AddLog("----------- New Recevied START -----------");

            Assistant.Serial ser = p.ReadUInt32();
            AddLog("Gump Operation:" + ser.ToString());

            uint gid = p.ReadUInt32();
            AddLog("Gump ID: 0x" + gid.ToString("X8"));

            try
            {
                int x = p.ReadInt32(), y = p.ReadInt32();

                string layout = p.GetCompressedReader().ReadString();

                int numStrings = p.ReadInt32();
                if ( numStrings < 0 || numStrings > 256 )
                    numStrings = 0;
                ArrayList strings = new ArrayList( numStrings );
                PacketReader pComp = p.GetCompressedReader();
                int len = 0;
                while ( !pComp.AtEnd && (len=pComp.ReadInt16()) > 0 )
                   AddLog("Gump Text Data: " + pComp.ReadUnicodeString(len));
            }
            catch
            {
            }

            AddLog("----------- New Recevied END -----------");
        }
        internal static void AddLog(string addlog)
        {
            RazorEnhanced.UI.EnhancedGumpInspector.EnhancedGumpInspectorListBox.Invoke(new Action(() => RazorEnhanced.UI.EnhancedGumpInspector.EnhancedGumpInspectorListBox.Items.Add(addlog)));           
        }
    }
}