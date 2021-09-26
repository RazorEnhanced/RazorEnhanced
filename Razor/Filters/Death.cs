using System;
using Assistant;

namespace Assistant.Filters
{
    public class DeathFilter : Filter
    {
        public static void Initialize()
        {
            Filter.Register(new DeathFilter());
        }

        private DeathFilter()
        {
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0x2C}; }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.DeathStatus); }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            args.Block = true;
        }
    }
}
