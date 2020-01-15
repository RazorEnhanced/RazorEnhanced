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

		public override byte[] PacketIDs { get { return new byte[] { 0x20, 0x78, 0x77 }; } }

		public override LocString Name { get { return LocString.StaffOnlyNpcs; } }

		public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
		{
			if (p.PacketID == 0x20) // Mobile update
			{
				if (World.Player == null)
					return;

                bool visible = true;
				uint serial = p.ReadUInt32(); // Serial

				if (serial == World.Player.Serial)
					return;

				p.ReadUInt16();
				p.ReadSByte();  // Body

				p.ReadUInt16(); // Hue

				byte flags = p.ReadByte();
				visible = (flags & 0x80) == 0;

				if (!visible)
					args.Block = true;
			}
			else if (p.PacketID == 0x78)  // Mobile Incoming
			{
				if (World.Player == null)
					return;

				bool visible = true;
				uint serial = p.ReadUInt32(); // Serial

				if (serial == World.Player.Serial)
					return;

				p.ReadUInt16(); // Body

				p.ReadUInt16(); //x
				p.ReadUInt16(); //y
				p.ReadSByte(); //z

				p.ReadByte(); // Direction

				p.ReadUInt16(); // Hue

				byte flags = p.ReadByte();
				visible = (flags & 0x80) == 0;

				if (!visible)
					args.Block = true;
			}
			else if (p.PacketID == 0x77)  // Mobile Moving
			{
				if (World.Player == null)
					return;

				bool visible = true;

				uint serial = p.ReadUInt32(); // Serial

				if (serial == World.Player.Serial)
					return;

				p.ReadUInt16(); // Body

				p.ReadUInt16(); //x
				p.ReadUInt16(); //y
				p.ReadSByte(); //z

				p.ReadByte(); // Direction

				p.ReadUInt16(); // Hue

				byte flags = p.ReadByte();
				visible = (flags & 0x80) == 0;

				if (!visible)
					args.Block = true;
			}
		}

		public override void OnEnable()
		{
			base.OnEnable();
			if (World.Player == null)
				return;

			foreach (Mobile m in World.Mobiles.Values)
			{
				if (m.Serial == World.Player.Serial)
					continue;

				if (!m.Visible)
			 		Assistant.Client.Instance.SendToClient(new RemoveObject(m));
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();

			if (World.Player == null)
				return;

			foreach (Mobile m in World.Mobiles.Values)
			{
				if (m.Serial == World.Player.Serial)
					continue;

				if (!m.Visible)
			 		Assistant.Client.Instance.SendToClient(new MobileUpdate(m));
			}
		}
	}
}
