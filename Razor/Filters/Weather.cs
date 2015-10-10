namespace Assistant.Filters
{
	internal class WeatherFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register(new WeatherFilter());
		}

		private WeatherFilter()
		{
		}

		internal override byte[] PacketIDs { get { return new byte[] { 0x65 }; } }

		internal override LocString Name { get { return LocString.Weather; } }

		internal override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
		{
			args.Block = true;
		}
	}
}

