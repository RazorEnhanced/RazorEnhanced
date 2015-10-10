namespace Assistant.Filters
{
	internal class LightFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register(new LightFilter());
		}

		private LightFilter()
		{
		}

		internal override byte[] PacketIDs { get { return new byte[] { 0x4E, 0x4F }; } }

		internal override LocString Name { get { return LocString.LightFilter; } }

		internal override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
		{
			args.Block = true;
			if (World.Player != null)
			{
				World.Player.LocalLightLevel = 0;
				World.Player.GlobalLightLevel = 0;
			}
		}

		internal override void OnEnable()
		{
			base.OnEnable();

			if (World.Player != null)
			{
				World.Player.LocalLightLevel = 0;
				World.Player.GlobalLightLevel = 0;

				ClientCommunication.SendToClient(new GlobalLightLevel(0));
				ClientCommunication.SendToClient(new PersonalLightLevel(World.Player));
			}
		}

	}
}
