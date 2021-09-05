using System;
using System.Collections.Generic;
using Assistant;

namespace Assistant.Filters
{
    public class SoundFilter : Filter
    {


        public static void Initialize()
        {
            foreach (var entry in RazorEnhanced.ConfigFiles.FilterSounds)
            {
                Filter.Register(new SoundFilter(entry.Key, entry.Value));
            }
        }

        public static ushort[] GetRange(ushort min, ushort max)
        {
            if (max < min)
                return new ushort[0];

            ushort[] range = new ushort[max - min + 1];
            for (ushort i = min; i <= max; i++)
                range[i - min] = i;
            return range;
        }

        private readonly string m_Name;
        private readonly ushort[] m_Sounds;

        private SoundFilter(LocString name, params ushort[] blockSounds)
        {
            m_Name = Language.GetString(name);
            m_Sounds = blockSounds;
        }
        private SoundFilter(string name, params ushort[] blockSounds)
        {
            m_Name = name;
            m_Sounds = blockSounds;
        }


        public override byte[] PacketIDs
        {
            get { return new byte[] {0x54}; }
        }

        public override string Name
        {
            get { return m_Name; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            p.ReadByte(); // flags

            ushort sound = p.ReadUInt16();
            if (RazorEnhanced.Sound.Logging)
            {
                RazorEnhanced.Misc.SendMessage(string.Format("Play Sound 0x{0:x} - {0}", sound), false);
            }
            for (int i = 0; i < m_Sounds.Length; i++)
            {
                if (m_Sounds[i] == sound)
                {
                    args.Block = true;
                    return;
                }
            }
        }
    }
}
