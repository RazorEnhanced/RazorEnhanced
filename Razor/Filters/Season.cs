using System;
using Assistant;

namespace Assistant.Filters
{
    public class SeasonFilter : Filter
    {
        public static void Initialize()
        {
            Filter season = new SeasonFilter();;
            Filter.Register(season);
        }

        private SeasonFilter()
        {
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0xBC}; }
        }

        public override LocString Name
        {
            get { return LocString.Season; }
        }


        public override void OnDisable()
        {
            base.OnDisable();
            if (Assistant.Client.Instance.Ready)
            {
                if (World.Player != null)
                {
                    Client.Instance.ForceSendToClient(new SeasonChange(World.Player.Season, true));
                }
            }
        }
        public override void OnEnable()
        {
            base.OnEnable();
            if (Assistant.Client.Instance.Ready)
            {
                if (World.Player != null)
                {
                    Client.Instance.ForceSendToClient(new SeasonChange(0, true));
                }
            }
        }


        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            if (Client.Instance.AllowBit(FeatureBit.WeatherFilter))
            {
                args.Block = true;
                Client.Instance.ForceSendToClient(new SeasonChange(0, true));
            }
        }
    }
}
