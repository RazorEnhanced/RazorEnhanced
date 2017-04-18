namespace Assistant.Filters
{
	internal class StaffNpcsFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register(new StaffNpcsFilter());
		}

		private StaffNpcsFilter()
		{
		}

		internal override byte[] PacketIDs { get { return new byte[] { 0x1A, 0xF3 }; } }

		internal override LocString Name { get { return LocString.StaffOnlyNpcs; } }

		internal override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
		{
			if (p.PacketID == 0xF3) 
			{
				
			}
			
		}

		internal override void OnEnable()
		{
			base.OnEnable();
			if (World.Player == null)
				return;

			foreach (Mobile m in World.Mobiles.Values)
			{
				if (m.Serial == World.Player.Serial)
					continue;

				if (!m.Visible)
					ClientCommunication.SendToClient(new RemoveObject(m));
			}
		}

		internal override void OnDisable()
		{
			base.OnDisable();

			if (World.Player == null)
				return;

			foreach (Mobile m in World.Mobiles.Values)
			{
				if (m.Serial == World.Player.Serial)
					continue;

				if (!m.Visible)
					ClientCommunication.SendToClient(new MobileUpdate(m));
			}
		}
	}
}