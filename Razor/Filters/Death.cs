namespace Assistant.Filters
{
	internal class DeathFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register(new DeathFilter());
		}

		private DeathFilter()
		{
		}

		internal override byte[] PacketIDs { get { return new byte[] { 0x2C }; } }

		internal override LocString Name { get { return LocString.DeathStatus; } }

		internal override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
		{
			args.Block = true;
		}
	}
}