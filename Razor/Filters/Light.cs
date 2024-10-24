namespace Assistant.Filters
{
    public class LightFilter : Filter
    {
        public static void Initialize()
        {
            Filter.Register(new LightFilter());
        }

        private LightFilter()
        {
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] { 0x4E, 0x4F }; }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.LightFilter); }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            if (Client.Instance.AllowBit(FeatureBit.LightFilter))
            {
                args.Block = true;
                if (World.Player != null)
                {
                    World.Player.LocalLightLevel = 0;
                    World.Player.GlobalLightLevel = 0;
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (Client.Instance.AllowBit(FeatureBit.LightFilter) && World.Player != null)
            {
                World.Player.LocalLightLevel = 0;
                World.Player.GlobalLightLevel = 0;

                Client.Instance.SendToClient(new GlobalLightLevel(0));
                Client.Instance.SendToClient(new PersonalLightLevel(World.Player));
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (Client.Instance.AllowBit(FeatureBit.LightFilter) && World.Player != null)
            {
                World.Player.LocalLightLevel = 6;
                World.Player.GlobalLightLevel = 2;

                Client.Instance.SendToClient(new GlobalLightLevel(26));
                Client.Instance.SendToClient(new PersonalLightLevel(World.Player));
            }
        }
    }
}
