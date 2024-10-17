using System;
using System.Text.RegularExpressions;

namespace Assistant.Filters
{
    public class VetRewardGumpFilter : Filter
    {
        // 1006046 = You have reward items available.  Click 'ok' below to get the selection menu or 'cancel' to be prompted upon your next login.
        //private static readonly string m_VetRewardStr = "{ xmfhtmlgump 52 35 420 55 1006046 1 1 }";

        public static void Initialize()
        {
            Filter.Register(new VetRewardGumpFilter());
        }

        private VetRewardGumpFilter()
        {
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] { 0xB0, 0xDD }; }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.VetRewardGump); }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            // completely skip this filter if we've been connected for more thn 1 minute
            if (Client.Instance.ConnectionStart + TimeSpan.FromMinutes(1.0) < DateTime.UtcNow)
                return;

            try
            {
                p.Seek(0, System.IO.SeekOrigin.Begin);
                byte packetID = p.ReadByte();

                p.MoveToData();

                uint ser = p.ReadUInt32();
                uint tid = p.ReadUInt32();
                int x = p.ReadInt32();
                int y = p.ReadInt32();

                // Decompression of Gump layout section
                PacketReader pr = p.GetCompressedReader();
                string layout = pr.ReadString();

                int numStrings = p.ReadInt32(); // Number of text lines
                if (numStrings < 0 || numStrings > 256)
                    numStrings = 0;

                // Parsing the uncompressed Gump Layout section
                // It is looking for all numbers and if one is a valid index for the cliloc, will be converted into string
                string[] numbers = Regex.Split(layout, @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int i = int.Parse(value);
                        if (i == 1006046)
                        {
                            args.Block = true;
                            RazorEnhanced.Gumps.CloseGump(tid);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
