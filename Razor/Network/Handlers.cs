using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace Assistant
{
	internal class PacketHandlers
	{
		private static List<Item> m_IgnoreGumps = new List<Item>();
		internal static List<Item> IgnoreGumps { get { return m_IgnoreGumps; } }

		public static void Initialize()
		{
			//Client -> Server handlers
			PacketHandler.RegisterClientToServerViewer(0x00, new PacketViewerCallback(CreateCharacter));
			PacketHandler.RegisterClientToServerViewer(0x02, new PacketViewerCallback(MovementRequest));
			PacketHandler.RegisterClientToServerFilter(0x05, new PacketFilterCallback(AttackRequest));	
			PacketHandler.RegisterClientToServerViewer(0x06, new PacketViewerCallback(ClientDoubleClick));
			PacketHandler.RegisterClientToServerViewer(0x07, new PacketViewerCallback(LiftRequest));
			PacketHandler.RegisterClientToServerViewer(0x08, new PacketViewerCallback(DropRequest));
			PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(ClientSingleClick));
			PacketHandler.RegisterClientToServerViewer(0x12, new PacketViewerCallback(ClientTextCommand));
			PacketHandler.RegisterClientToServerViewer(0x13, new PacketViewerCallback(EquipRequest));
			PacketHandler.RegisterClientToServerViewer(0x22, new PacketViewerCallback(ResyncRequest));
			// 0x29 - UOKR confirm drop.  0 bytes payload (just a single byte, 0x29, no length or data)
			PacketHandler.RegisterClientToServerViewer(0x3A, new PacketViewerCallback(SetSkillLock));
			PacketHandler.RegisterClientToServerViewer(0x5D, new PacketViewerCallback(PlayCharacter));
			PacketHandler.RegisterClientToServerViewer(0x75, new PacketViewerCallback(RenameMobile));
			PacketHandler.RegisterClientToServerViewer(0x7D, new PacketViewerCallback(MenuResponse));
			PacketHandler.RegisterClientToServerFilter(0x80, new PacketFilterCallback(ServerListLogin));
			PacketHandler.RegisterClientToServerFilter(0x91, new PacketFilterCallback(GameLogin));
			PacketHandler.RegisterClientToServerViewer(0x95, new PacketViewerCallback(HueResponse));
			PacketHandler.RegisterClientToServerViewer(0x9A, new PacketViewerCallback(ClientAsciiPromptResponse));
			PacketHandler.RegisterClientToServerViewer(0xA0, new PacketViewerCallback(PlayServer));
			PacketHandler.RegisterClientToServerViewer(0xAC, new PacketViewerCallback(ResponseStringQuery));
            PacketHandler.RegisterClientToServerViewer(0xB1, new PacketViewerCallback(ClientGumpResponse));
			PacketHandler.RegisterClientToServerFilter(0xBF, new PacketFilterCallback(ExtendedClientCommand));
			//PacketHandler.RegisterClientToServerViewer( 0xD6, new PacketViewerCallback( BatchQueryProperties ) );
			PacketHandler.RegisterClientToServerViewer(0xC2, new PacketViewerCallback(UnicodePromptSend));
			PacketHandler.RegisterClientToServerViewer(0xD7, new PacketViewerCallback(ClientEncodedPacket));
			PacketHandler.RegisterClientToServerViewer(0xF8, new PacketViewerCallback(CreateCharacter));

			//Server -> Client handlers
			PacketHandler.RegisterServerToClientViewer(0x0B, new PacketViewerCallback(Damage));
			PacketHandler.RegisterServerToClientViewer(0x11, new PacketViewerCallback(MobileStatus));
			PacketHandler.RegisterServerToClientViewer(0x16, new PacketViewerCallback(SAMobileStatus));
			PacketHandler.RegisterServerToClientViewer(0x17, new PacketViewerCallback(NewMobileStatus));
			PacketHandler.RegisterServerToClientViewer(0x1A, new PacketViewerCallback(WorldItem));
			PacketHandler.RegisterServerToClientViewer(0x1B, new PacketViewerCallback(LoginConfirm));
			PacketHandler.RegisterServerToClientFilter(0x1C, new PacketFilterCallback(AsciiSpeech));
			PacketHandler.RegisterServerToClientViewer(0x1D, new PacketViewerCallback(RemoveObject));
			PacketHandler.RegisterServerToClientFilter(0x20, new PacketFilterCallback(MobileUpdate));
			PacketHandler.RegisterServerToClientViewer(0x21, new PacketViewerCallback(MovementRej));
			PacketHandler.RegisterServerToClientViewer(0x22, new PacketViewerCallback(MovementAck));
			PacketHandler.RegisterServerToClientViewer(0x24, new PacketViewerCallback(BeginContainerContent));
			PacketHandler.RegisterServerToClientFilter(0x25, new PacketFilterCallback(ContainerContentUpdate));
			PacketHandler.RegisterServerToClientViewer(0x27, new PacketViewerCallback(LiftReject));
			PacketHandler.RegisterServerToClientViewer(0x28, new PacketViewerCallback(DropReject));
			PacketHandler.RegisterServerToClientViewer(0x29, new PacketViewerCallback(DropAccepted));
			PacketHandler.RegisterServerToClientViewer(0x2D, new PacketViewerCallback(MobileStatInfo));
			PacketHandler.RegisterServerToClientFilter(0x2E, new PacketFilterCallback(EquipmentUpdate));
			PacketHandler.RegisterServerToClientViewer(0x3A, new PacketViewerCallback(Skills));
			PacketHandler.RegisterServerToClientFilter(0x3C, new PacketFilterCallback(ContainerContent));
			PacketHandler.RegisterServerToClientViewer(0x4E, new PacketViewerCallback(PersonalLight));
			PacketHandler.RegisterServerToClientViewer(0x4F, new PacketViewerCallback(GlobalLight));
			PacketHandler.RegisterServerToClientViewer(0x6F, new PacketViewerCallback(TradeRequest));
			PacketHandler.RegisterServerToClientViewer(0x72, new PacketViewerCallback(ServerSetWarMode));
			//PacketHandler.RegisterServerToClientViewer(0x73, new PacketViewerCallback(PingResponse));
			PacketHandler.RegisterServerToClientViewer(0x76, new PacketViewerCallback(ServerChange));
			PacketHandler.RegisterServerToClientFilter(0x77, new PacketFilterCallback(MobileMoving));
			PacketHandler.RegisterServerToClientFilter(0x78, new PacketFilterCallback(MobileIncoming));
			PacketHandler.RegisterServerToClientViewer(0x7C, new PacketViewerCallback(SendMenu));
			PacketHandler.RegisterServerToClientViewer(0x97, new PacketViewerCallback(MovementDemand));
			PacketHandler.RegisterServerToClientViewer(0x9A, new PacketViewerCallback(AsciiPromptResponse));
			PacketHandler.RegisterServerToClientViewer(0xA1, new PacketViewerCallback(HitsUpdate));
			PacketHandler.RegisterServerToClientViewer(0xA2, new PacketViewerCallback(ManaUpdate));
			PacketHandler.RegisterServerToClientViewer(0xA3, new PacketViewerCallback(StamUpdate));
			PacketHandler.RegisterServerToClientViewer(0xA8, new PacketViewerCallback(ServerList));
			PacketHandler.RegisterServerToClientViewer(0xA9, new PacketViewerCallback(CharList));
			PacketHandler.RegisterServerToClientViewer(0xAB, new PacketViewerCallback(DisplayStringQuery));
			PacketHandler.RegisterServerToClientViewer(0xAF, new PacketViewerCallback(DeathAnimation));
			PacketHandler.RegisterServerToClientFilter(0xAE, new PacketFilterCallback(UnicodeSpeech));
			PacketHandler.RegisterServerToClientViewer(0xB0, new PacketViewerCallback(SendGump));
			PacketHandler.RegisterServerToClientViewer(0xB9, new PacketViewerCallback(Features));
			PacketHandler.RegisterServerToClientViewer(0xBC, new PacketViewerCallback(ChangeSeason));
			PacketHandler.RegisterServerToClientViewer(0xBF, new PacketViewerCallback(ExtendedPacket));
			PacketHandler.RegisterServerToClientFilter(0xC1, new PacketFilterCallback(OnLocalizedMessage));
			PacketHandler.RegisterServerToClientViewer(0xC2, new PacketViewerCallback(UnicodePromptRecevied));
			PacketHandler.RegisterServerToClientFilter(0xC8, new PacketFilterCallback(SetUpdateRange));
			PacketHandler.RegisterServerToClientFilter(0xCC, new PacketFilterCallback(OnLocalizedMessageAffix));
			PacketHandler.RegisterServerToClientViewer(0xD6, new PacketViewerCallback(EncodedPacket));//0xD6 "encoded" packets
			PacketHandler.RegisterServerToClientViewer(0xD8, new PacketViewerCallback(CustomHouseInfo));
			PacketHandler.RegisterServerToClientFilter(0xDC, new PacketFilterCallback(ServOPLHash));
			PacketHandler.RegisterServerToClientViewer(0xDD, new PacketViewerCallback(CompressedGump));
			PacketHandler.RegisterServerToClientViewer(0xDF, new PacketViewerCallback(BuffDebuff));
			PacketHandler.RegisterServerToClientViewer(0xF0, new PacketViewerCallback(RunUOProtocolExtention)); // Special RunUO protocol extentions (for KUOC/Razor)
			PacketHandler.RegisterServerToClientViewer(0xF3, new PacketViewerCallback(SAWorldItem));
			PacketHandler.RegisterServerToClientViewer(0xF6, new PacketViewerCallback(MoveBoatHS));
		}

		private static void DisplayStringQuery(PacketReader p, PacketHandlerEventArgs args)
		{
			World.Player.QueryStringID = p.ReadInt32();
			World.Player.QueryStringType = p.ReadByte();
			World.Player.QueryStringIndex = p.ReadByte();

			if (RazorEnhanced.Misc.BlockGump)
				args.Block = true;

			World.Player.HasQueryString = true;
		}

		private static void SetUpdateRange(Packet p, PacketHandlerEventArgs args)
		{
			if (Engine.ClientMajor >= 7 && Engine.ClientBuild < 49)
				args.Block = true;
			else
				World.Player.VisRange = (int)p.ReadByte();
		}

		private static void EncodedPacket(PacketReader p, PacketHandlerEventArgs args)
		{
			ushort id = p.ReadUInt16();

			switch (id)
			{
				case 1: // object property list
					{
						Serial s = p.ReadUInt32();

						if (s.IsItem)
						{
							Item item = World.FindItem(s);
							if (item == null)
							{
								World.AddItem(item = new Item(s));
							}

							item.ReadPropertyList(p);
						/*	if (item.ModifiedOPL)
							{
								args.Block = true;
								ClientCommunication.SendToClient(item.ObjPropList.BuildPacket());
							}*/

							item.PropsUpdated = true;
						}
						else if (s.IsMobile)
						{
							Mobile m = World.FindMobile(s);
							if (m == null)
								World.AddMobile(m = new Mobile(s));

							m.ReadPropertyList(p);
						/*	if (m.ModifiedOPL)
							{
								args.Block = true;
								ClientCommunication.SendToClient(m.ObjPropList.BuildPacket());
							}*/

							m.PropsUpdated = true;
						}
						break;
					}
			}
		}

		private static void ServOPLHash(Packet p, PacketHandlerEventArgs args)
		{
		/*	Serial s = p.ReadUInt32();
			int hash = p.ReadInt32();

			if (s.IsItem)
			{
				Item item = World.FindItem(s);
				if (item != null && item.OPLHash != hash)
				{
					item.OPLHash = hash;
					p.Seek(-4, SeekOrigin.Current);
					p.Write((uint)item.OPLHash);
				}
			}
			else if (s.IsMobile)
			{
				Mobile m = World.FindMobile(s);
				if (m != null && m.OPLHash != hash)
				{
					m.OPLHash = hash;
					p.Seek(-4, SeekOrigin.Current);
					p.Write((uint)m.OPLHash);
				}
			}*/
		}

		private static void ClientSingleClick(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial ser = p.ReadUInt32();

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_ClientSingleClick(ser);

			// if you modify this, don't forget to modify the allnames hotkey
			if (RazorEnhanced.Settings.General.ReadBool("LastTargTextFlags"))
			{
				Mobile m = World.FindMobile(ser);
				if (m != null)
					Targeting.CheckTextFlags(m);
			}
		}

		private static void ClientDoubleClick(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial ser = p.ReadUInt32();

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_ClientDoubleClick(ser);

			if (ser.IsItem)
			{
				Item item = World.FindItem(ser);
				if (item != null)
				{
					item.Updated = false;
					World.Player.LastObject = ser;
				}
			}

			if (RazorEnhanced.Settings.General.ReadBool("BlockDismount") && World.Player != null && ser == World.Player.Serial && World.Player.Warmode && World.Player.GetItemOnLayer(Layer.Mount) != null)
			{ // mount layer = 0x19
				World.Player.SendMessage(LocString.DismountBlocked);
				args.Block = true;
				return;
			}

			if (RazorEnhanced.Settings.General.ReadBool("QueueActions"))
				args.Block = !PlayerData.DoubleClick(ser, false);
		}

		private static void DeathAnimation(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial killed = p.ReadUInt32();
			Mobile m = World.FindMobile(killed);

			if (!RazorEnhanced.Settings.General.ReadBool("AutoCap"))
				return;

			if (m != null && ((m.Body >= 0x0190 && m.Body <= 0x0193) || (m.Body >= 0x025D && m.Body <= 0x0260)) && Utility.Distance(World.Player.Position, m.Position) <= 12)
				ScreenCapManager.DeathCapture();
		}

		private static void ExtendedClientCommand(Packet p, PacketHandlerEventArgs args)
		{
			ushort ext = p.ReadUInt16();
			switch (ext)
			{
				case 0x09: // Sa disarm
					{
						if (RazorEnhanced.ScriptRecorder.OnRecord)
							RazorEnhanced.ScriptRecorder.Record_SADisarm();
						break;
					}
				case 0x0A: // Sa Stun
					{
						if (RazorEnhanced.ScriptRecorder.OnRecord)
							RazorEnhanced.ScriptRecorder.Record_SAStun();
						break;
					}
			/*	case 0x10: // query object properties
					{
						break;
					}*/
				case 0x15: // context menu response
					{
						UOEntity ent = null;
						Serial ser = p.ReadUInt32();
						ushort idx = p.ReadUInt16();

						if (ser.IsMobile)
							ent = World.FindMobile(ser);
						else if (ser.IsItem)
							ent = World.FindItem(ser);

						if (RazorEnhanced.ScriptRecorder.OnRecord)
							RazorEnhanced.ScriptRecorder.Record_ContextMenuResponse(ser, idx);

						World.Player.HasContext = false;
						World.Player.ContextID = 0;

						break;
					}
				case 0x1C:// cast spell
					{
						Serial ser = Serial.MinusOne;
						if (p.ReadUInt16() == 1)
							ser = p.ReadUInt32();
						ushort sid = p.ReadUInt16();

						if (RazorEnhanced.ScriptRecorder.OnRecord)
							RazorEnhanced.ScriptRecorder.Record_ClientTextCommand(2, sid);

						Spell s = Spell.Get(sid);
						if (s != null)
						{
							s.OnCast(p);
							args.Block = true;
						}
						break;
					}
			/*	case 0x24:
					{
						// for the cheatx0r part 2...  anything outside this range indicates some haxing, just hide it with 0x30s
						byte b = p.ReadByte();
						if (b < 0x25 || b >= 0x5E + 0x25)
						{
							p.Seek(-1, SeekOrigin.Current);
							p.Write((byte)0x30);
						}
						//using ( StreamWriter w = new StreamWriter( "bf24.txt", true ) )
						//	w.WriteLine( "{0} : 0x{1:X2}", DateTime.Now.ToString( "HH:mm:ss.ffff" ), b );
						break;
					}*/
			}
		}

		private static void ClientTextCommand(PacketReader p, PacketHandlerEventArgs args)
		{
			int type = p.ReadByte();
			string command = p.ReadString();

			switch (type)
			{
				case 0x24: // Use skill
					{
						int skillIndex;

						try { skillIndex = Convert.ToInt32(command.Split(' ')[0]); }
						catch { break; }

						if (RazorEnhanced.ScriptRecorder.OnRecord)
							RazorEnhanced.ScriptRecorder.Record_ClientTextCommand(1, skillIndex);

						if (World.Player != null)
							World.Player.LastSkill = skillIndex;

						if (skillIndex == (int)SkillName.Stealth && !World.Player.Visible)
							StealthSteps.Hide();
						break;
					}
				case 0x27: // Cast spell from book
					{
						try
						{
							string[] split = command.Split(' ');

							if (split.Length > 0)
							{
								ushort spellID = Convert.ToUInt16(split[0]);
								Serial serial = Convert.ToUInt32(split.Length > 1 ? Utility.ToInt32(split[1], -1) : -1);

								if (RazorEnhanced.ScriptRecorder.OnRecord)
									RazorEnhanced.ScriptRecorder.Record_ClientTextCommand(2, spellID);

								Spell s = Spell.Get(spellID);
								if (s != null)
								{
									s.OnCast(p);
									args.Block = true;
								}
							}
						}
						catch
						{
						}
						break;
					}
				case 0x56: // Cast spell from macro
					{
						try
						{
							ushort spellID = Convert.ToUInt16(command);

							if (RazorEnhanced.ScriptRecorder.OnRecord)
								RazorEnhanced.ScriptRecorder.Record_ClientTextCommand(2, spellID);

							Spell s = Spell.Get(spellID);
							if (s != null)
							{
								s.OnCast(p);
								args.Block = true;
							}
						}
						catch
						{
						}
						break;
					}
				case 0xF4: // Invoke Virtue
					{
						if (RazorEnhanced.ScriptRecorder.OnRecord)
						{
							int virtueid = 0;
							try
							{
								virtueid = Convert.ToInt32(command.Split(' ')[0]);
								RazorEnhanced.ScriptRecorder.Record_ClientTextCommand(3, virtueid);
							}
							catch { break; }
						}
						break;
					}

			}
		}

		internal static DateTime PlayCharTime = DateTime.MinValue;

		private static void CreateCharacter(PacketReader p, PacketHandlerEventArgs args)
		{
			p.Seek(1 + 4 + 4 + 1, SeekOrigin.Begin); // skip begining crap
			World.OrigPlayerName = p.ReadStringSafe(30);

			if (Engine.MainWindow != null)
				Engine.MainWindow.UpdateControlLocks();

			PlayCharTime = DateTime.Now;
		}

		private static void PlayCharacter(PacketReader p, PacketHandlerEventArgs args)
		{
			p.ReadUInt32(); //0xedededed
			World.OrigPlayerName = p.ReadStringSafe(30);

			if (Engine.MainWindow != null)
				Engine.MainWindow.UpdateControlLocks();

			PlayCharTime = DateTime.Now;
		}

		private static void RenameMobile(PacketReader p, PacketHandlerEventArgs args)
		{
			if (RazorEnhanced.ScriptRecorder.OnRecord)
			{
				Serial ser = p.ReadUInt32();
				string name = p.ReadStringSafe(30);
				RazorEnhanced.ScriptRecorder.Record_RenameMobile((int)ser, name);
			}
		}

		private static void ServerList(PacketReader p, PacketHandlerEventArgs args)
		{
			p.ReadByte(); //unknown
			ushort numServers = p.ReadUInt16();

			for (int i = 0; i < numServers; ++i)
			{
				ushort num = p.ReadUInt16();
				World.Servers[num] = p.ReadString(32);
				p.ReadByte(); // full %
				p.ReadSByte(); // time zone
				p.ReadUInt32(); // ip
			}
		}

		private static void CharList(PacketReader p, PacketHandlerEventArgs args)
		{
		/*	byte ccount = p.ReadByte();
			for (int i = 0; i < ccount; ++i)
			{
				string name = p.ReadString(30);
				p.ReadString(30); // pass
				//RazorEnhanced.AutoLoot.AddLog(name);
			}
			byte cicount = p.ReadByte();
			for (int i = 0; i < cicount; ++i)
			{
				p.ReadByte(); // index
				string name = p.ReadString(32);
				p.ReadString(32); // build name
				p.ReadUInt32(); //x
				p.ReadUInt32(); //y
				p.ReadUInt32(); // z
				p.ReadUInt32(); // map
				p.ReadUInt32(); // cliloc
				p.ReadUInt32(); // 0
			//	RazorEnhanced.AutoLoot.AddLog(name);
			}
			uint flags = p.ReadUInt32();
			RazorEnhanced.AutoLoot.AddLog(flags.ToString("X4"));
			if ((flags & 0x08) != 0)
			{
				RazorEnhanced.AutoLoot.AddLog("presente");
			}*/
		}

		private static void PlayServer(PacketReader p, PacketHandlerEventArgs args)
		{
			ushort index = p.ReadUInt16();

			World.ShardName = World.Servers[index] as string ?? "[Unknown]";
		}

		private static void ResponseStringQuery(PacketReader p, PacketHandlerEventArgs args)
		{
			World.Player.HasQueryString = false;

			if (!RazorEnhanced.ScriptRecorder.OnRecord)
				return;

			p.ReadUInt32(); //  Serial
			p.ReadByte(); // Parent ID
			p.ReadByte(); // Button
			byte yesno = p.ReadByte();
			int textlenght = p.ReadInt16();
			string text = p.ReadStringSafe(textlenght);
			RazorEnhanced.ScriptRecorder.Record_ResponseStringQuery(yesno, text);
		}
		
		private static void LiftRequest(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();
			ushort amount = p.ReadUInt16();

			Item item = World.FindItem(serial);
			ushort iid = 0;

			if (item != null)
				iid = item.ItemID.Value;

			RazorEnhanced.DragDropManager.HoldingItem = true;
			if (RazorEnhanced.Settings.General.ReadBool("QueueActions"))
			{
				if (item == null)
				{
					World.AddItem(item = new Item(serial));
					item.Amount = amount;
				}

				DragDropManager.Drag(item, amount, true);
				args.Block = true;
			}
		}

		private static void LiftReject(PacketReader p, PacketHandlerEventArgs args)
		{
			RazorEnhanced.DragDropManager.HoldingItem = false;

			if (RazorEnhanced.AutoLoot.AutoMode)
				args.Block = true;

			if (!DragDropManager.LiftReject())
			{
				args.Block = true;
			}
		}

		private static void DropReject(PacketReader p, PacketHandlerEventArgs args)
		{
			RazorEnhanced.DragDropManager.HoldingItem = false;
		}

		private static void DropAccepted(PacketReader p, PacketHandlerEventArgs args)
		{
			RazorEnhanced.DragDropManager.HoldingItem = false;
		}

		private static void EquipRequest(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial iser = p.ReadUInt32(); // item being dropped serial
			Layer layer = (Layer)p.ReadByte();
			Serial mser = p.ReadUInt32();

			Item item = World.FindItem(iser);

			if (item == null)
				return;

			Mobile m = World.FindMobile(mser);
			if (m == null)
				return;

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_EquipRequest(item, layer, m);

			RazorEnhanced.DragDropManager.HoldingItem = false;

			// Aggiornamento icone spellgrid
			if (item.Layer == Layer.RightHand || item.Layer == Layer.LeftHand || item.Layer == Layer.FirstValid)
				RazorEnhanced.SpellGrid.UpdateSAIcon();

			if (RazorEnhanced.Settings.General.ReadBool("QueueActions"))
				args.Block = DragDropManager.Drop(item, m, layer);
		}

		private static void DropRequest(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial iser = p.ReadUInt32();
			int x = p.ReadInt16();
			int y = p.ReadInt16();
			int z = p.ReadSByte();
			if (Engine.UsePostKRPackets)
				/* grid num */
				p.ReadByte();
			Point3D newPos = new Point3D(x, y, z);
			Serial dser = p.ReadUInt32();

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_DropRequest(iser, dser);

			RazorEnhanced.DragDropManager.HoldingItem = false;

			Item i = World.FindItem(iser);
			if (i == null)
				return;

			Item dest = World.FindItem(dser);
			if (dest != null && dest.IsContainer && World.Player != null && (dest.IsChildOf(World.Player.Backpack) || dest.IsChildOf(World.Player.Quiver)))
				i.IsNew = true;

			if (RazorEnhanced.Settings.General.ReadBool("QueueActions"))
				args.Block = DragDropManager.Drop(i, dser, newPos);
		}

		private static void MovementRej(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			if (World.Player.WalkScriptRequest == 1)
				World.Player.WalkScriptRequest = 3;

			byte seq = p.ReadByte();
			int x = p.ReadUInt16();
			int y = p.ReadUInt16();
			Direction dir = (Direction)p.ReadByte();
			sbyte z = p.ReadSByte();

			World.Player.MoveRej(seq, dir, new Point3D(x, y, z));
		}

		private static void MovementAck(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			if (World.Player.WalkScriptRequest == 1)
				World.Player.WalkScriptRequest = 2;

			byte oldNoto = World.Player.Notoriety;

			byte seq = p.ReadByte();
			World.Player.Notoriety = p.ReadByte();

			args.Block |= !World.Player.MoveAck(seq);
		}

		private static void MovementRequest(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			Direction dir = (Direction)p.ReadByte();
			byte seq = p.ReadByte();

			World.Player.MoveReq(dir, seq);

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_Movement(dir);
		}

		internal static byte[] HandleRPVContainerContentUpdate(Packet p)
		{
			// This function will ignore the item if the container item has not been sent to the client yet.
			// We can do this because we can't really count on getting all of the container info anyway.
			// (So we'd need to request the container be updated, so why bother with the extra stuff required to find the container once its been sent?)
			bool isPostKR = false, decided = false;

			Serial serial = p.ReadUInt32();
			ushort itemid = p.ReadUInt16();
			itemid = (ushort)(itemid + p.ReadSByte()); // signed, itemID offset
			ushort amount = p.ReadUInt16();
			if (amount == 0)
				amount = 1;
			Point3D pos = new Point3D(p.ReadUInt16(), p.ReadUInt16(), 0);
			byte gridPos = 0;
			if (!decided)
			{
				byte nextByte = p.ReadByte();

				isPostKR = ((nextByte & 0x40) == 0);
				decided = true;

				if (isPostKR == Engine.UsePostKRPackets)
					return p.Compile(); // not need to change anything

				p.Seek(-1, SeekOrigin.Current);
			}

			if (isPostKR)
				gridPos = p.ReadByte();

			Serial cser = p.ReadUInt32();
			ushort hue = p.ReadUInt16();

			Item i = World.FindItem(serial);
			if (i == null)
			{
				if (!serial.IsItem)
					return p.Compile();

				World.AddItem(i = new Item(serial));
				i.IsNew = i.AutoStack = true;
			}

			i.ItemID = itemid;
			i.Amount = amount;
			i.Position = pos;
			i.GridNum = gridPos;
			i.Hue = hue;
			i.Container = cser;

			if (i.IsNew)
				Item.UpdateContainers();

			return new ContainerItem(i, Engine.UsePostKRPackets).Compile();
		}

		private static void ContainerContentUpdate(Packet p, PacketHandlerEventArgs args)
		{
			// This function will ignore the item if the container item has not been sent to the client yet.
			// We can do this because we can't really count on getting all of the container info anyway.
			// (So we'd need to request the container be updated, so why bother with the extra stuff required to find the container once its been sent?)
			Serial serial = p.ReadUInt32();
			ushort itemid = p.ReadUInt16();
			itemid = (ushort)(itemid + p.ReadSByte()); // signed, itemID offset
			ushort amount = p.ReadUInt16();
			if (amount == 0)
				amount = 1;
			Point3D pos = new Point3D(p.ReadUInt16(), p.ReadUInt16(), 0);
			byte gridPos = 0;
			if (Engine.UsePostKRPackets)
				gridPos = p.ReadByte();
			Serial cser = p.ReadUInt32();

			if (cser.IsItem)
			{
				Item container = World.FindItem(cser);
				if (container != null)
					container.Updated = true;
			}

			ushort hue = p.ReadUInt16();

			Item i = World.FindItem(serial);
			if (i == null)
			{
				if (!serial.IsItem)
					return;

				World.AddItem(i = new Item(serial));
				i.IsNew = i.AutoStack = true;
			}
			else
			{
				i.CancelRemove();
			}

			if (serial != DragDropManager.Pending)
			{
				if (!DragDropManager.EndHolding(serial))
					return;
			}

			i.ItemID = itemid;
			i.Amount = amount;
			i.Position = pos;
			i.GridNum = gridPos;
			i.Hue = hue;

			i.Container = cser;
			if (i.IsNew)
				Item.UpdateContainers();
		}

		private static void BeginContainerContent(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial ser = p.ReadUInt32();
			if (!ser.IsItem)
				return;
			Item item = World.FindItem(ser);
			if (item != null)
			{
				if (m_IgnoreGumps.Contains(item))
				{
					m_IgnoreGumps.Remove(item);
					args.Block = true;
				}
			}
			else
			{
				World.AddItem(new Item(ser));
				Item.UpdateContainers();
			}
			item = World.FindItem(ser);
			if (item != null)
				item.Updated = true;
		}

		internal static byte[] HandleRPVContainerContent(Packet p)
		{
			bool isPostKR = false, decided = false; ;
			int count = p.ReadUInt16();

			List<Item> list = new List<Item>();

			for (int i = 0; i < count; i++)
			{
				Serial serial = p.ReadUInt32();
				// serial is purposely not checked to be valid, sometimes buy lists dont have "valid" item serials (and we are okay with that).
				Item item = World.FindItem(serial);
				if (item == null)
				{
					World.AddItem(item = new Item(serial));
					item.IsNew = true;
					item.AutoStack = false;
				}
				else
				{
					item.CancelRemove();
				}

				//if ( !DragDropManager.EndHolding( serial ) )
				//	continue;

				item.ItemID = p.ReadUInt16();
				item.ItemID = (ushort)(item.ItemID + p.ReadSByte());// signed, itemID offset
				item.Amount = p.ReadUInt16();
				if (item.Amount == 0)
					item.Amount = 1;
				item.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), 0);

				if (!decided)
				{
					byte nextByte = p.ReadByte();

					isPostKR = ((nextByte & 0x40) == 0);
					decided = true;

					if (isPostKR == Engine.UsePostKRPackets)
						return p.Compile(); // not need to change anything

					p.Seek(-1, SeekOrigin.Current);
				}

				if (isPostKR)
					item.GridNum = p.ReadByte();

				Serial cont = p.ReadUInt32();
				item.Hue = p.ReadUInt16();

				item.Container = cont; // must be done after hue is set (for counters)

				list.Add(item);

			}
			Item.UpdateContainers();

			return new ContainerContent(list, Engine.UsePostKRPackets).Compile();
		}

		private static void ContainerContent(Packet p, PacketHandlerEventArgs args)
		{
			int count = p.ReadUInt16();

			List<Item> updated = new List<Item>();

			for (int i = 0; i < count; i++)
			{
				Serial serial = p.ReadUInt32();
				// serial is purposely not checked to be valid, sometimes buy lists dont have "valid" item serials (and we are okay with that).
				Item item = World.FindItem(serial);
				if (item == null)
				{
					World.AddItem(item = new Item(serial));
					item.IsNew = true;
					item.AutoStack = false;
				}
				else
				{
					item.CancelRemove();
				}

				if (!DragDropManager.EndHolding(serial))
					continue;

				item.ItemID = p.ReadUInt16();
				item.ItemID = (ushort)(item.ItemID + p.ReadSByte());// signed, itemID offset
				item.Amount = p.ReadUInt16();
				if (item.Amount == 0)
					item.Amount = 1;
				item.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), 0);
				if (Engine.UsePostKRPackets)
					item.GridNum = p.ReadByte();
				Serial cont = p.ReadUInt32();

				if (cont.IsItem)
				{
					Item container = World.FindItem(cont);
					if (container != null && !updated.Contains(container))
						updated.Add(container);
				}

				item.Hue = p.ReadUInt16();

				item.Container = cont; // must be done after hue is set (for counters)
			}

			foreach (Item container in updated)
				container.Updated = true;

			Item.UpdateContainers();
		}

		private static void EquipmentUpdate(Packet p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();

			Item i = World.FindItem(serial);
			bool isNew = false;
			if (i == null)
			{
				World.AddItem(i = new Item(serial));
				isNew = true;
				Item.UpdateContainers();
			}
			else
			{
				i.CancelRemove();
			}

			if (!DragDropManager.EndHolding(serial))
				return;

			ushort iid = p.ReadUInt16();
			i.ItemID = (ushort)(iid + p.ReadSByte()); // signed, itemID offset
			i.Layer = (Layer)p.ReadByte();

			Serial ser = p.ReadUInt32();// cont must be set after hue (for counters)
			if (ser.IsItem)
			{
				Item container = World.FindItem(ser);
				if (container != null)
					container.Updated = true;
			}

			i.Hue = p.ReadUInt16();

			i.Container = ser;

			// Apply flag color if enabled
			p = RazorEnhanced.Filters.EquipmentUpdateColorize(p, i);

			if (i.Layer != Layer.Backpack || !isNew || ser != World.Player.Serial)
				return;

			m_IgnoreGumps.Add(i);
			PlayerData.DoubleClick(i);
		}

		private static void SetSkillLock(PacketReader p, PacketHandlerEventArgs args)
		{
			int i = p.ReadUInt16();

			if (i < 0 || i >= Skill.Count)
				return;

			Skill skill = World.Player.Skills[i];

			skill.Lock = (LockType)p.ReadByte();
			Engine.MainWindow.UpdateSkill(skill);
		}

		private static void Skills(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null || World.Player.Skills == null || Engine.MainWindow == null)
				return;

			byte type = p.ReadByte();

			switch (type)
			{
				case 0x02://list (with caps, 3.0.8 and up)
					{
						int i;
						while ((i = p.ReadUInt16()) > 0)
						{
							if (i > 0 && i <= Skill.Count)
							{
								Skill skill = World.Player.Skills[i - 1];

								if (skill == null)
									continue;

								skill.FixedValue = p.ReadUInt16();
								skill.FixedBase = p.ReadUInt16();
								skill.Lock = (LockType)p.ReadByte();
								skill.FixedCap = p.ReadUInt16();
								if (!World.Player.SkillsSent)
									skill.Delta = 0;
								ClientCommunication.PostSkillUpdate(i - 1, skill.FixedBase);
							}
							else
							{
								p.Seek(7, SeekOrigin.Current);
							}
						}

						World.Player.SkillsSent = true;
						Engine.MainWindow.RedrawSkills();
						break;
					}

				case 0x00: // list (without caps, older clients)
					{
						int i;
						while ((i = p.ReadUInt16()) > 0)
						{
							if (i > 0 && i <= Skill.Count)
							{
								Skill skill = World.Player.Skills[i - 1];

								if (skill == null)
									continue;

								skill.FixedValue = p.ReadUInt16();
								skill.FixedBase = p.ReadUInt16();
								skill.Lock = (LockType)p.ReadByte();
								skill.FixedCap = 100;//p.ReadUInt16();
								if (!World.Player.SkillsSent)
									skill.Delta = 0;

								ClientCommunication.PostSkillUpdate(i - 1, skill.FixedBase);
							}
							else
							{
								p.Seek(5, SeekOrigin.Current);
							}
						}

						World.Player.SkillsSent = true;
						Engine.MainWindow.RedrawSkills();
						break;
					}

				case 0xDF: //change (with cap, new clients)
					{
						int i = p.ReadUInt16();

						if (i >= 0 && i < Skill.Count)
						{
							Skill skill = World.Player.Skills[i];

							if (skill == null)
								break;

							ushort old = skill.FixedBase;
							skill.FixedValue = p.ReadUInt16();
							skill.FixedBase = p.ReadUInt16();
							skill.Lock = (LockType)p.ReadByte();
							skill.FixedCap = p.ReadUInt16();
							Engine.MainWindow.UpdateSkill(skill);

							if (RazorEnhanced.Settings.General.ReadBool("DisplaySkillChanges") && skill.FixedBase != old)
								World.Player.SendMessage(MsgLevel.Force, LocString.SkillChanged, (SkillName)i, skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value, skill.FixedBase - old > 0 ? "+" : "", ((double)(skill.FixedBase - old)) / 10.0);
							ClientCommunication.PostSkillUpdate(i, skill.FixedBase);
						}
						break;
					}

				case 0xFF: //change (without cap, older clients)
					{
						int i = p.ReadUInt16();

						if (i >= 0 && i < Skill.Count)
						{
							Skill skill = World.Player.Skills[i];

							if (skill == null)
								break;

							ushort old = skill.FixedBase;
							skill.FixedValue = p.ReadUInt16();
							skill.FixedBase = p.ReadUInt16();
							skill.Lock = (LockType)p.ReadByte();
							skill.FixedCap = 100;
							Engine.MainWindow.UpdateSkill(skill);
							if (RazorEnhanced.Settings.General.ReadBool("DisplaySkillChanges") && skill.FixedBase != old)
								World.Player.SendMessage(MsgLevel.Force, LocString.SkillChanged, (SkillName)i, skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value, ((double)(skill.FixedBase - old)) / 10.0, skill.FixedBase - old > 0 ? "+" : "");
							ClientCommunication.PostSkillUpdate(i, skill.FixedBase);
						}
						break;
					}
			}
		}

		private static void LoginConfirm(PacketReader p, PacketHandlerEventArgs args)
		{
			World.Items.Clear();
			World.Mobiles.Clear(); 

			if (Engine.ClientMajor >= 7)
				UseNewStatus = true;
			else
				UseNewStatus = false;

			Serial serial = p.ReadUInt32();

			PlayerData m = new PlayerData(serial);
			m.Name = World.OrigPlayerName;

			Mobile test = World.FindMobile(serial);
			if (test != null)
				test.Remove();

			World.AddMobile(World.Player = m);

			PlayerData.ExternalZ = false;

			p.ReadUInt32(); // always 0?
			m.Body = p.ReadUInt16();
			m.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16());
			m.Direction = (Direction)p.ReadByte();
			m.Resync();

			ClientCommunication.SendToServer(new StatusQuery(m.Serial));

			ClientCommunication.PostLogin((int)serial.Value);
			Engine.MainWindow.UpdateTitle(); // update player name & shard name
			ClientCommunication.BeginCalibratePosition();

			// Carico profili se linkati
			string profilename = RazorEnhanced.Profiles.IsLinked(serial);
			if (profilename != null && RazorEnhanced.Profiles.LastUsed() != profilename)
			{
				RazorEnhanced.Profiles.ProfileChange(profilename);
				Engine.MainWindow.ProfilesComboBox.SelectedIndex = Engine.MainWindow.ProfilesComboBox.FindString(profilename);
			}

			// Apertura automatica toolbar se abilitata
			if (RazorEnhanced.Settings.General.ReadBool("AutoopenToolBarCheckBox") && RazorEnhanced.ToolBar.ToolBarForm == null)
				RazorEnhanced.ToolBar.Open();

			// Apertura automatica spellgrit se abilitata
			if (RazorEnhanced.Settings.General.ReadBool("GridOpenLoginCheckBox") && RazorEnhanced.SpellGrid.SpellGridForm == null)
				RazorEnhanced.SpellGrid.Open();

			// Avvio automatico script selezionati come autostart
			RazorEnhanced.Scripts.AutoStart();
        }

		private static void MobileMoving(Packet p, PacketHandlerEventArgs args)
		{
			Mobile m = World.FindMobile(p.ReadUInt32());

			if (m == null)
				return;

			m.Body = p.ReadUInt16();

			// Chiamata funzione cambio grafica mob.
			if (Engine.MainWindow.MobFilterCheckBox.Checked)
				p = RazorEnhanced.Filters.GraphChange(p, m.Body);

			m.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte());

			if (World.Player != null && !Utility.InRange(World.Player.Position, m.Position, World.Player.VisRange))
			{
				m.Remove();
				return;
			}

			Targeting.CheckLastTargetRange(m);

			m.Direction = (Direction)p.ReadByte();

			m.Hue = p.ReadUInt16();
			m.ProcessPacketFlags(p.ReadByte());

			// Apply color flag on mob if enabled
			p = RazorEnhanced.Filters.MobileColorize(p, m);

			m.Notoriety = p.ReadByte();

			if (m == World.Player)
			{
				ClientCommunication.BeginCalibratePosition();
			}
		}

		private static void AsciiPromptResponse(PacketReader p, PacketHandlerEventArgs args)
		{
			//int serial = (int)p.ReadUInt32();
			//int id = (int)p.ReadUInt32();
			//int type = (int)p.ReadUInt32();
			//string message = p.ReadString();

			if (World.Player != null)
				World.Player.HasPrompt = false;
		}

		private static void UnicodePromptSend(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			uint serial = p.ReadUInt32();
			uint id = p.ReadUInt32();
			uint type = p.ReadUInt32();

			World.Player.HasPrompt = false;
			World.Player.PromptSenderSerial = serial;
			World.Player.PromptID = id;
			World.Player.PromptType = type;

			//string lang = p.ReadStringSafe(4);
			//string message = p.ReadUnicodeStringSafe();
		}

		private static void UnicodePromptRecevied(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			uint serial = p.ReadUInt32();
			uint id = p.ReadUInt32();
			uint type = p.ReadUInt32();

			World.Player.HasPrompt = false;
			World.Player.PromptSenderSerial = serial;
			World.Player.PromptID = id;
			World.Player.PromptType = type;

			//string lang = p.ReadStringSafe(4);
			//string message = p.ReadUnicodeStringSafe();
		}

		private static readonly int[] HealthHues = new int[] { 428, 333, 37, 44, 49, 53, 158, 263, 368, 473, 578 };

		private static void HitsUpdate(PacketReader p, PacketHandlerEventArgs args)
		{
			Mobile m = World.FindMobile(p.ReadUInt32());

			if (m == null)
				return;

			int oldPercent = (int)(m.Hits * 100 / (m.HitsMax == 0 ? (ushort)1 : m.HitsMax));

			m.HitsMax = p.ReadUInt16();
			m.Hits = p.ReadUInt16();

			if (m == World.Player)
			{
				// Update hits toolbar
				RazorEnhanced.ToolBar.UpdateHits(m.HitsMax, m.Hits);

				ClientCommunication.PostHitsUpdate();
			}

			if (!RazorEnhanced.Settings.General.ReadBool("ShowHealth"))
				return;

			int percent = (int)(m.Hits * 100 / (m.HitsMax == 0 ? (ushort)1 : m.HitsMax));

			// Limit to people who are on screen and check the previous value so we dont get spammed.
			if (oldPercent != percent && World.Player != null && Utility.Distance(World.Player.Position, m.Position) <= 12)
			{
				try
				{
					m.OverheadMessageFrom(HealthHues[((percent + 5) / 10) % HealthHues.Length],
						Language.Format(LocString.sStatsA1, m.Name),
						RazorEnhanced.Settings.General.ReadString("HealthFmt"), percent);
					RazorEnhanced.Filters.ProcessMessage(m);
				}
				catch
				{
				}
			}
		}

		private static void StamUpdate(PacketReader p, PacketHandlerEventArgs args)
		{
			Mobile m = World.FindMobile(p.ReadUInt32());

			if (m == null)
				return;

			int oldPercent = (int)(m.Stam * 100 / (m.StamMax == 0 ? (ushort)1 : m.StamMax));

			m.StamMax = p.ReadUInt16();
			m.Stam = p.ReadUInt16();

			if (m == World.Player)
			{
				// Update Stam Toolbar
				RazorEnhanced.ToolBar.UpdateStam(m.StamMax, m.Stam);

				ClientCommunication.PostStamUpdate();
			}

			if (m == World.Player || !RazorEnhanced.Settings.General.ReadBool("ShowPartyStats"))
				return;

			int stamPercent = (int)(m.Stam * 100 / (m.StamMax == 0 ? (ushort)1 : m.StamMax));
			int manaPercent = (int)(m.Mana * 100 / (m.ManaMax == 0 ? (ushort)1 : m.ManaMax));

			// Limit to people who are on screen and check the previous value so we dont get spammed.
			if (oldPercent != stamPercent && World.Player != null && Utility.Distance(World.Player.Position, m.Position) <= 12)
			{
				try
				{
					m.OverheadMessageFrom(0x63,
						Language.Format(LocString.sStatsA1, m.Name),
						RazorEnhanced.Settings.General.ReadString("PartyStatFmt"), manaPercent, stamPercent);
				}
				catch
				{
				}
			}
		}

		private static void ManaUpdate(PacketReader p, PacketHandlerEventArgs args)
		{
			Mobile m = World.FindMobile(p.ReadUInt32());

			if (m == null)
				return;

			int oldPercent = (int)(m.Mana * 100 / (m.ManaMax == 0 ? (ushort)1 : m.ManaMax));

			m.ManaMax = p.ReadUInt16();
			m.Mana = p.ReadUInt16();

			if (m == World.Player)
			{
				// Update Mana toolbar
				RazorEnhanced.ToolBar.UpdateMana(m.ManaMax, m.Mana);

				ClientCommunication.PostManaUpdate();
			}

			if (m == World.Player || !RazorEnhanced.Settings.General.ReadBool("ShowPartyStats"))
				return;

			int stamPercent = (int)(m.Stam * 100 / (m.StamMax == 0 ? (ushort)1 : m.StamMax));
			int manaPercent = (int)(m.Mana * 100 / (m.ManaMax == 0 ? (ushort)1 : m.ManaMax));

			// Limit to people who are on screen and check the previous value so we dont get spammed.
			if (oldPercent != manaPercent && World.Player != null && Utility.Distance(World.Player.Position, m.Position) <= 12)
			{
				try
				{
					m.OverheadMessageFrom(0x63,
						Language.Format(LocString.sStatsA1, m.Name),
						RazorEnhanced.Settings.General.ReadString("PartyStatFmt"), manaPercent, stamPercent);
				}
				catch
				{
				}
			}
		}

		private static void MobileStatInfo(PacketReader pvSrc, PacketHandlerEventArgs args)
		{
			Mobile m = World.FindMobile(pvSrc.ReadUInt32());
			if (m == null)
				return;
			PlayerData p = World.Player;

			m.HitsMax = pvSrc.ReadUInt16();
			m.Hits = pvSrc.ReadUInt16();

			m.ManaMax = pvSrc.ReadUInt16();
			m.Mana = pvSrc.ReadUInt16();

			m.StamMax = pvSrc.ReadUInt16();
			m.Stam = pvSrc.ReadUInt16();

			if (m == World.Player)
			{
				ClientCommunication.PostHitsUpdate();
				ClientCommunication.PostStamUpdate();
				ClientCommunication.PostManaUpdate();
			}
		}

		internal static bool UseNewStatus = false;

		private static void SAMobileStatus(PacketReader p, PacketHandlerEventArgs args)
		{
			uint serial = p.ReadUInt32();
            Mobile m = World.FindMobile(serial);

			if (m == null)
			{
				World.AddMobile(m = new Mobile(serial));
				ClientCommunication.SendToServer(new StatusQuery(serial));
				ClientCommunication.SendToServer(new QueryProperties(serial));
			}

			UseNewStatus = true;

			if (p.ReadUInt16() == 0)
				return;
			// 00 01 Poison
			// 00 02 Yellow Health Bar

			ushort id = p.ReadUInt16();

			// 00 Off
			// 01 On
			// For Poison: Poison Level + 1

			byte flag = p.ReadByte();

			switch (id)
			{
				case 1:
					m.Poisoned = (flag != 0);
					break;
				case 2:
					m.Blessed = (flag != 0);
					break;
			}

			if (m == World.Player)
			{
				if (Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked)
					RazorEnhanced.Filters.ApplyColor(m);
			}
			else
			{
				if (Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked)
					RazorEnhanced.Filters.ApplyColor(m);
			}
		}

		private static void NewMobileStatus(PacketReader p, PacketHandlerEventArgs args)
		{
			uint serial = p.ReadUInt32();
            Mobile m = World.FindMobile(serial);

			if (m == null)
			{
				World.AddMobile(m = new Mobile(serial));
				ClientCommunication.SendToServer(new StatusQuery(serial));
				ClientCommunication.SendToServer(new QueryProperties(serial));
			}

			UseNewStatus = true;

			// 00 01
			p.ReadUInt16();

			// 00 01 Poison
			// 00 02 Yellow Health Bar

			ushort id = p.ReadUInt16();

			// 00 Off
			// 01 On
			// For Poison: Poison Level + 1

			byte flag = p.ReadByte();

			switch (id)
			{
				case 1:
					m.Poisoned = (flag != 0);
					break;
				case 2:
					m.Blessed = (flag != 0);
					break;
			}

			if (m == World.Player)
			{
				if (Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked)
					RazorEnhanced.Filters.ApplyColor(m);
			}
			else
			{
				if (Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked)
					RazorEnhanced.Filters.ApplyColor(m);
			}
		}

		private static void Damage(PacketReader p, PacketHandlerEventArgs args)
		{
			if (RazorEnhanced.DPSMeter.Enabled)
				RazorEnhanced.DPSMeter.AddDamage(p.ReadUInt32(), p.ReadUInt16());
		}

		private static void MobileStatus(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();
			Mobile m = World.FindMobile(serial);
			if (m == null)
			{
				World.AddMobile(m = new Mobile(serial));
			}

			m.Name = p.ReadString(30);

			m.Hits = p.ReadUInt16();
			m.HitsMax = p.ReadUInt16();

			p.ReadBoolean();//CanBeRenamed

			byte type = p.ReadByte();

			if (m != World.Player || type == 0x00)
				return;

			PlayerData player = (PlayerData)m;

			player.Female = p.ReadBoolean();
			player.Expansion = type;

			int oStr = player.Str, oDex = player.Dex, oInt = player.Int;

			player.Str = p.ReadUInt16();
			player.Dex = p.ReadUInt16();
			player.Int = p.ReadUInt16();

			if (player.Str != oStr && oStr != 0 && RazorEnhanced.Settings.General.ReadBool("DisplaySkillChanges"))
				World.Player.SendMessage(MsgLevel.Force, LocString.StrChanged, player.Str - oStr > 0 ? "+" : "", player.Str - oStr, player.Str);

			if (player.Dex != oDex && oDex != 0 && RazorEnhanced.Settings.General.ReadBool("DisplaySkillChanges"))
				World.Player.SendMessage(MsgLevel.Force, LocString.DexChanged, player.Dex - oDex > 0 ? "+" : "", player.Dex - oDex, player.Dex);

			if (player.Int != oInt && oInt != 0 && RazorEnhanced.Settings.General.ReadBool("DisplaySkillChanges"))
				World.Player.SendMessage(MsgLevel.Force, LocString.IntChanged, player.Int - oInt > 0 ? "+" : "", player.Int - oInt, player.Int);

			player.Stam = p.ReadUInt16();
			player.StamMax = p.ReadUInt16();
			player.Mana = p.ReadUInt16();
			player.ManaMax = p.ReadUInt16();

			player.Gold = p.ReadUInt32();
			player.AR = p.ReadUInt16(); // ar / physical resist
			player.Weight = p.ReadUInt16();

			if (type >= 0x03)
			{
				if (type > 0x04)
				{
					player.MaxWeight = p.ReadUInt16();
					player.Race = p.ReadByte();
				}

				player.StatCap = p.ReadUInt16();

				if (type > 0x03)
				{
					player.Followers = p.ReadByte();
					player.FollowersMax = p.ReadByte();

					player.FireResistance = p.ReadInt16();
					player.ColdResistance = p.ReadInt16();
					player.PoisonResistance = p.ReadInt16();
					player.EnergyResistance = p.ReadInt16();

					player.Luck = p.ReadInt16();

					player.DamageMin = p.ReadUInt16();
					player.DamageMax = p.ReadUInt16();

					player.Tithe = p.ReadInt32();
				}

				if (type > 0x05)        // KR Data
				{


					player.HitChanceIncrease = p.ReadInt16();
					player.SwingSpeedIncrease = p.ReadInt16();
					player.DamageChanceIncrease = p.ReadInt16();
					player.LowerReagentCost = p.ReadInt16();
					player.HitPointsRegeneration = p.ReadInt16();
					player.StaminaRegeneration = p.ReadInt16();
					player.ManaRegeneration = p.ReadInt16();
					player.ReflectPhysicalDamage = p.ReadInt16();
					player.EnhancePotions = p.ReadInt16();
					player.DefenseChanceIncrease = p.ReadInt16();
					player.SpellDamageIncrease = p.ReadInt16();
					player.FasterCastRecovery = p.ReadInt16();
					player.FasterCasting = p.ReadInt16();
					player.LowerManaCost = p.ReadInt16();
					player.StrengthIncrease = p.ReadInt16();
					player.DexterityIncrease = p.ReadInt16();
					player.IntelligenceIncrease = p.ReadInt16();
					player.HitPointsIncrease = p.ReadInt16();
					player.StaminaIncrease = p.ReadInt16();
					player.ManaIncrease = p.ReadInt16();
					player.MaximumHitPointsIncrease = p.ReadInt16();
					player.MaximumStaminaIncrease = p.ReadInt16();
					player.MaximumManaIncrease = p.ReadInt16();
				}
			}

			// Update All toolbar
			RazorEnhanced.ToolBar.UpdateAll();

			ClientCommunication.PostHitsUpdate();
			ClientCommunication.PostStamUpdate();
			ClientCommunication.PostManaUpdate();

			Engine.MainWindow.UpdateTitle(); // update player name
		}

		private static void MobileUpdate(Packet p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			Serial serial = p.ReadUInt32();
			Mobile m = World.FindMobile(serial);

			if (m == null)
			{
				World.AddMobile(m = new Mobile(serial));
				if (!ClientCommunication.ServerEncrypted)
				{
					ClientCommunication.SendToServer(new QueryProperties(serial));
					ClientCommunication.SendToServer(new StatusQuery(serial));
				}
            }

			bool wasHidden = !m.Visible;

			m.Body = (ushort)(p.ReadUInt16() + p.ReadSByte());

			m.Hue = p.ReadUInt16();
			m.ProcessPacketFlags(p.ReadByte());

			// Apply flag color on mob if enabled
			p = RazorEnhanced.Filters.MobileColorize(p, m);

			if (m == World.Player)
			{
				ClientCommunication.BeginCalibratePosition();

				World.Player.Resync();

				if (!wasHidden && !m.Visible)
				{
					if (RazorEnhanced.Settings.General.ReadBool("AlwaysStealth"))
						StealthSteps.Hide();
				}
				else if (wasHidden && m.Visible)
				{
					StealthSteps.Unhide();
				}
			}

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();
			p.ReadUInt16(); //always 0?
			m.Direction = (Direction)p.ReadByte();
			m.Position = new Point3D(x, y, p.ReadSByte());
			Item.UpdateContainers();
		}

		private static void MobileIncoming(Packet p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			Serial serial = p.ReadUInt32();
			ushort body = p.ReadUInt16();

			// Chiamata funzione cambio grafica mob.
			if (Engine.MainWindow.MobFilterCheckBox.Checked)
				p = RazorEnhanced.Filters.GraphChange(p, body);

			Point3D position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte());

			if (World.Player.Position != Point3D.Zero && !Utility.InRange(World.Player.Position, position, World.Player.VisRange))
				return;

			Mobile m = World.FindMobile(serial);
			if (m == null)
			{
				World.AddMobile(m = new Mobile(serial));
				if (!ClientCommunication.ServerEncrypted)
				{
					ClientCommunication.SendToServer(new QueryProperties(serial));
					ClientCommunication.SendToServer(new StatusQuery(serial));
				}
			}

			bool wasHidden = !m.Visible;

			if (m != World.Player && RazorEnhanced.Settings.General.ReadBool("ShowMobNames"))
				ClientCommunication.SendToServer(new SingleClick(m));
			if (RazorEnhanced.Settings.General.ReadBool("LastTargTextFlags"))
				Targeting.CheckTextFlags(m);

			int ltHue = RazorEnhanced.Settings.General.ReadInt("LTHilight");
			

			m.Body = body;

			if (m != World.Player || World.Player.OutstandingMoveReqs == 0)
				m.Position = position;
			m.Direction = (Direction)p.ReadByte();

			m.Hue = p.ReadUInt16();
			m.ProcessPacketFlags(p.ReadByte());

			// Apply color flag on mob if enabled
			p = RazorEnhanced.Filters.MobileColorize(p, m);

			m.Notoriety = p.ReadByte();

			if (m == World.Player)
			{
				ClientCommunication.BeginCalibratePosition();

				if (!wasHidden && !m.Visible)
				{
					if (RazorEnhanced.Settings.General.ReadBool("AlwaysStealth"))
						StealthSteps.Hide();
				}
				else if (wasHidden && m.Visible)
				{
					StealthSteps.Unhide();
				}
			}

			while (true)
			{
				serial = p.ReadUInt32();
				if (!serial.IsItem)
				{
					break;
				}
				Item item = World.FindItem(serial);
				bool isNew = false;
				if (item == null)
				{
					isNew = true;
					World.AddItem(item = new Item(serial));
				}
				if (DragDropManager.EndHolding(serial))
				{
					item.Container = m;
					ushort num = p.ReadUInt16();
					if (Engine.UseNewMobileIncoming)
					{
						item.ItemID = num;
					}
					else
					{
						item.ItemID = (ushort)(num & 16383);
					}
					item.Layer = (Layer)p.ReadByte();
					if (Engine.UseNewMobileIncoming || (num & 32768) != 0)
					{
						item.Hue = p.ReadUInt16();
						
						// Colorize item 
						p = RazorEnhanced.Filters.MobileIncomingItemColorize(p, m, true);
					}
					else
					{
						item.Hue = 0;
						// Colorize item 
						RazorEnhanced.Filters.MobileIncomingItemColorize(p, m, false, item);
					}
					if ((item.Layer == Layer.Backpack & isNew) && m == World.Player && m != null)
					{
						PacketHandlers.m_IgnoreGumps.Add(item);
						PlayerData.DoubleClick(item);
					}
				}
			}
			Item.UpdateContainers();
			RazorEnhanced.Filters.ProcessMessage(m);
		}

		private static void RemoveObject(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();

			if (serial.IsMobile)
			{
				Mobile m = World.FindMobile(serial);
				if (m != null && m != World.Player)
					m.Remove();
			}
			else if (serial.IsItem)
			{
				Item i = World.FindItem(serial);
				if (i != null)
				{
					if (DragDropManager.Holding == i)
					{
						i.Container = null;
					}
					else
					{
						i.RemoveRequest();
					}
				}
			}
		}

		private static void ServerChange(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player != null)
				World.Player.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16());
		}

		private static void WorldItem(PacketReader p, PacketHandlerEventArgs args)
		{
			uint serial = p.ReadUInt32();
			Item item = World.FindItem(serial & 0x7FFFFFFF);
			bool isNew = false;
			if (item == null)
			{
				World.AddItem(item = new Item(serial & 0x7FFFFFFF));
				isNew = true;
			}
			else
			{
				item.CancelRemove();
			}

			if (!DragDropManager.EndHolding(serial))
				return;

			item.Container = null;

			ushort itemID = p.ReadUInt16();
			item.ItemID = (ushort)(itemID & 0x7FFF);

			if ((serial & 0x80000000) != 0)
				item.Amount = p.ReadUInt16();
			else
				item.Amount = 1;

			if ((itemID & 0x8000) != 0)
				item.ItemID = (ushort)(item.ItemID + p.ReadSByte());

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();

			if ((x & 0x8000) != 0)
				item.Direction = p.ReadByte();
			else
				item.Direction = 0;

			short z = p.ReadSByte();

			item.Position = new Point3D(x & 0x7FFF, y & 0x3FFF, z);

			if ((y & 0x8000) != 0)
				item.Hue = p.ReadUInt16();
			else
				item.Hue = 0;

			byte flags = 0;
			if ((y & 0x4000) != 0)
				flags = p.ReadByte();

			item.ProcessPacketFlags(flags);

			if (isNew && World.Player != null)
			{
				if (item.ItemID == 0x2006) // corpse itemid = 0x2006
				{
					if (RazorEnhanced.Settings.General.ReadBool("ShowCorpseNames"))
						ClientCommunication.SendToServer(new SingleClick(item));
					if (RazorEnhanced.Settings.General.ReadBool("AutoOpenCorpses") && Utility.InRange(item.Position, World.Player.Position, RazorEnhanced.Settings.General.ReadInt("CorpseRange")) && World.Player != null && World.Player.Visible)
						Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
				}
				else if (item.IsMulti)
				{
					World.AddMulti(item);
					ClientCommunication.PostAddMulti(item.ItemID, item.Position);
				}
			}
			Item.UpdateContainers();

			// Filtro muri
			if (!Assistant.Engine.MainWindow.ShowStaticFieldCheckBox.Checked)
				return;

			switch (item.ItemID)
			{
				case 0x0080:
				case 0x0082:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x3B1;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Wall Of Stone]"));
					return;
				case 0x3996:
				case 0x398C:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x0845;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Fire Field]"));
					return;
				case 0x3915:
				case 0x3920:
                case 0x3922:
                    args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x016A;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Poison Field]"));
					return;
				case 0x3967:
				case 0x3979:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x00DA;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Paralyze Field]"));
					return;
				case 0x3946:
				case 0x3956:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x0125;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Energy Field]"));
					return;
			}
		}

		private static void SAWorldItem(PacketReader p, PacketHandlerEventArgs args)
		{
			/*
			New World Item Packet
			PacketID: 0xF3
			PacketLen: 24
			Format:

				BYTE - 0xF3 packetId
				WORD - 0x01
				BYTE - ArtDataID: 0x00 if the item uses art from TileData table, 0x02 if the item uses art from MultiData table)
				DWORD - item Serial
				WORD - item ID
				BYTE - item direction (same as old)
				WORD - amount
				WORD - amount
				WORD - X
				WORD - Y
				SBYTE - Z
				BYTE - item light
				WORD - item Hue
				BYTE - item flags (same as old packet)
			*/

			// Post-7.0.9.0
			/*
			New World Item Packet
			PacketID: 0xF3
			PacketLen: 26
			Format:

				BYTE - 0xF3 packetId
				WORD - 0x01
				BYTE - ArtDataID: 0x00 if the item uses art from TileData table, 0x02 if the item uses art from MultiData table)
				DWORD - item Serial
				WORD - item ID
				BYTE - item direction (same as old)
				WORD - amount
				WORD - amount
				WORD - X
				WORD - Y
				SBYTE - Z
				BYTE - item light
				WORD - item Hue
				BYTE - item flags (same as old packet)
				WORD ???
			*/

			ushort _unk1 = p.ReadUInt16();

			byte _artDataID = p.ReadByte();

			uint serial = p.ReadUInt32();
			Item item = World.FindItem(serial);
			bool isNew = false;
			if (item == null)
			{
				World.AddItem(item = new Item(serial));
				isNew = true;
			}
			else
			{
				item.CancelRemove();
			}

			if (!DragDropManager.EndHolding(serial))
				return;

			item.Container = null;

			ushort itemID = p.ReadUInt16();
			item.ItemID = (ushort)(_artDataID == 0x02 ? itemID | 0x4000 : itemID);

			item.Direction = p.ReadByte();

			ushort _amount = p.ReadUInt16();
			item.Amount = _amount = p.ReadUInt16();

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();
			short z = p.ReadSByte();

			item.Position = new Point3D(x, y, z);

			byte _light = p.ReadByte();

			item.Hue = p.ReadUInt16();

			byte flags = p.ReadByte();

			item.ProcessPacketFlags(flags);

			if (Engine.UsePostHSChanges)
			{
				p.ReadUInt16();
			}

			if (isNew && World.Player != null)
			{
				if (item.ItemID == 0x2006)// corpse itemid = 0x2006
				{
					if (RazorEnhanced.Settings.General.ReadBool("ShowCorpseNames"))
						ClientCommunication.SendToServer(new SingleClick(item));
					if (RazorEnhanced.Settings.General.ReadBool("AutoOpenCorpses") && Utility.InRange(item.Position, World.Player.Position, RazorEnhanced.Settings.General.ReadInt("CorpseRange")) && World.Player != null && World.Player.Visible)
						Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
				}
				else if (item.IsMulti)
				{
					World.AddMulti(item);
					ClientCommunication.PostAddMulti(item.ItemID, item.Position);
				}
			}

			Item.UpdateContainers();

			// Filtro muri
			if (!Assistant.Engine.MainWindow.ShowStaticFieldCheckBox.Checked)
				return;

			switch (item.ItemID)
			{
				case 0x0080:
				case 0x0082:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x3B1;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Wall Of Stone]"));
					return;
				case 0x3996:
				case 0x398C:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x0845;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Fire Field]"));
					return;
				case 0x3915:
				case 0x3920:
				case 0x3922:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x016A;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Poison Field]"));
					return;
				case 0x3967:
				case 0x3979:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x00DA;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Paralyze Field]"));
					return;
				case 0x3946:
				case 0x3956:
					args.Block = true;
					item.ItemID = 0x28A8;
					item.Hue = 0x0125;
					ClientCommunication.SendToClient(new WorldItem(item));
					if (RazorEnhanced.Settings.General.ReadBool("ShowMessageFieldCheckBox"))
						ClientCommunication.SendToClient(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, 10, 3, Language.CliLocName, item.Name, "[Energy Field]"));
					return;
			}
		}

		private static void MoveBoatHS(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();
			p.ReadByte();
			p.ReadByte();
			p.ReadByte();
			Point3D position = new Point3D((int)p.ReadUInt16(), (int)p.ReadUInt16(), (int)p.ReadInt16());
			UOEntity uOEntity = World.FindItem(serial);
			if (uOEntity != null)
			{
				uOEntity.Position = position;
			}
			int num = (int)p.ReadInt16();
			int i = 0;
			while (i < num)
			{
				serial = p.ReadUInt32();
				position = new Point3D((int)p.ReadUInt16(), (int)p.ReadUInt16(), (int)p.ReadInt16());
				if (serial.IsMobile)
				{
					uOEntity = World.FindMobile(serial);
					if (uOEntity != null)
					{
						uOEntity.Position = position;
					}
				}
				if (serial.IsItem)
				{
					uOEntity = World.FindItem(serial);
					if (uOEntity != null)
					{
						uOEntity.Position = position;
					}
				}
				i++;
			}
		}

		internal static List<string> SysMessages = new List<string>(21);

		internal static void HandleSpeech(Packet p, PacketHandlerEventArgs args, Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name, string text)
		{
			if (World.Player == null)
				return;
			World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(text, type.ToString(), hue, name));          // Journal buffer
			if (World.Player.Journal.Count > 100)
			{
				RazorEnhanced.Journal.JournalEntry ra;
				World.Player.Journal.TryDequeue(out ra);
			}

			if (!ser.IsValid || ser == World.Player.Serial || ser.IsItem)
			{
				SysMessages.Add(text.ToLower());

				if (SysMessages.Count >= 25)
					SysMessages.RemoveRange(0, 10);
			}

			if (type == MessageType.Spell)
			{
				Spell s = Spell.Get(text.Trim());
				bool replaced = false;
				if (s != null)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder(RazorEnhanced.Settings.General.ReadString("SpellFormat"));
					sb.Replace(@"{power}", s.WordsOfPower);
					string spell = Language.GetString(s.Name);
					sb.Replace(@"{spell}", spell);
					sb.Replace(@"{name}", spell);

					string newText = sb.ToString();

					if (!string.IsNullOrEmpty(newText) && newText != text)
					{
						ClientCommunication.SendToClient(new AsciiMessage(ser, body, MessageType.Spell, s.GetHue(hue), font, name, newText));
						replaced = true;
						args.Block = true;
					}
				}

				if (!replaced && RazorEnhanced.Settings.General.ReadBool("ForceSpellHue"))
				{
					p.Seek(10, SeekOrigin.Begin);
					if (s != null)
						p.Write((ushort)s.GetHue(hue));
					else
						p.Write((ushort)RazorEnhanced.Settings.General.ReadInt("NeutralSpellHue"));
				}
			}
			else if (ser.IsMobile && type == MessageType.Label)
			{
				Mobile m = World.FindMobile(ser);
				if (m != null && m.Name.IndexOf(text) != 5 && m != World.Player && !(text.StartsWith("(") && text.EndsWith(")")))
					m.Name = text;
			}
			else
			{
				if (ser == Serial.MinusOne && name == "System")
				{
					if (RazorEnhanced.Settings.General.ReadBool("FilterSnoopMsg") && text.IndexOf(World.Player.Name) == -1 && text.StartsWith("You notice") && text.IndexOf("attempting to peek into") != -1 && text.IndexOf("belongings") != -1)
					{
						args.Block = true;
						return;
					}
					else if (text.StartsWith("You've committed a criminal act") || text.StartsWith("You are now a criminal"))
					{
						World.Player.ResetCriminalTimer();
					}
				}

				if ((type == MessageType.Emote || type == MessageType.Regular || type == MessageType.Whisper || type == MessageType.Yell) && ser.IsMobile && ser != World.Player.Serial)
				{
					if (RazorEnhanced.Settings.General.ReadBool("ForceSpeechHue"))
					{
						p.Seek(10, SeekOrigin.Begin);
						p.Write((ushort)RazorEnhanced.Settings.General.ReadInt("SpeechHue"));
					}
				}

				if (RazorEnhanced.Settings.General.ReadBool("FilterSpam") && (ser == Serial.MinusOne || ser == Serial.Zero))
				{
					if (!MessageQueue.Enqueue(ser, body, type, hue, font, lang, name, text))
					{
						args.Block = true;
						return;
					}
				}
			}
		}

		internal static void AsciiSpeech(Packet p, PacketHandlerEventArgs args)
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			string name = p.ReadStringSafe(30);
			string text = p.ReadStringSafe();

			if (World.Player != null && serial == Serial.Zero && body == 0 && type == MessageType.Regular && hue == 0xFFFF && font == 0xFFFF && name == "SYSTEM")
			{
				args.Block = true;

				p.Seek(3, SeekOrigin.Begin);
				p.WriteAsciiFixed("", (int)p.Length - 3);

				if (World.ShardName.Contains("Demise"))  // Use old Version
					DLLImport.Razor.DoFeatures(World.Player.Features, true);
				else
					DLLImport.Razor.DoFeatures(World.Player.Features, false);
			}
			else
			{
				HandleSpeech(p, args, serial, body, type, hue, font, "A", name, text);
			}
		}

		internal static void UnicodeSpeech(Packet p, PacketHandlerEventArgs args)
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			string lang = p.ReadStringSafe(4);
			string name = p.ReadStringSafe(30);
			string text = p.ReadUnicodeStringSafe();

			HandleSpeech(p, args, serial, body, type, hue, font, lang, name, text);
		}

		private static void OnLocalizedMessage(Packet p, PacketHandlerEventArgs args)
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			int num = p.ReadInt32();
			string name = p.ReadStringSafe(30);
			string ext_str = p.ReadUnicodeStringLE();

			if ((num >= 3002011 && num < 3002011 + 64) || // reg spells
				(num >= 1060509 && num < 1060509 + 16) || // necro
				(num >= 1060585 && num < 1060585 + 10) || // chiv
				(num >= 1060493 && num < 1060493 + 10) || // chiv
				(num >= 1060595 && num < 1060595 + 6) || // bush
				(num >= 1060610 && num < 1060610 + 8)) // ninj
			{
				type = MessageType.Spell;
			}

			try
			{
				string text = Language.ClilocFormat(num, ext_str);
				HandleSpeech(p, args, serial, body, type, hue, font, Language.CliLocName.ToUpper(), name, text);
			}
			catch (Exception e)
			{
				Engine.LogCrash(new Exception(String.Format("Exception in Ultima.dll cliloc: {0}, {1}", num, ext_str), e));
			}
		}

		private static void OnLocalizedMessageAffix(Packet p, PacketHandlerEventArgs phea)
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			int num = p.ReadInt32();
			byte affixType = p.ReadByte();
			string name = p.ReadStringSafe(30);
			string affix = p.ReadStringSafe();
			string args = p.ReadUnicodeStringSafe();

			if ((num >= 3002011 && num < 3002011 + 64) || // reg spells
				(num >= 1060509 && num < 1060509 + 16) || // necro
				(num >= 1060585 && num < 1060585 + 10) || // chiv
				(num >= 1060493 && num < 1060493 + 10) || // chiv
				(num >= 1060595 && num < 1060595 + 6) || // bush
				(num >= 1060610 && num < 1060610 + 8)     // ninj
				)
			{
				type = MessageType.Spell;
			}

			string text;
			if ((affixType & 1) != 0) // prepend
				text = String.Format("{0}{1}", affix, Language.ClilocFormat(num, args));
			else // 0 == append, 2 = system
				text = String.Format("{0}{1}", Language.ClilocFormat(num, args), affix);
			HandleSpeech(p, phea, serial, body, type, hue, font, Language.CliLocName.ToUpper(), name, text);
		}

		private static void SendGump(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			World.Player.CurrentGumpS = p.ReadUInt32();
			World.Player.CurrentGumpI = p.ReadUInt32();
			World.Player.HasGump = true;
			RazorEnhanced.GumpInspector.NewGumpStandardAddLog(World.Player.CurrentGumpS, World.Player.CurrentGumpI);
		}

		private static void ClientGumpResponse(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			Serial ser = p.ReadUInt32();
			uint tid = p.ReadUInt32();
			int bid = p.ReadInt32();
			List<int> switchesid = new List<int>();
			List<string> texts = new List<string>();

			RazorEnhanced.GumpInspector.GumpResponseAddLogMain(ser, tid, bid);

			World.Player.HasGump = false;
			World.Player.CurrentGumpI = 0;
			World.Player.CurrentGumpStrings.Clear();

			int sc = p.ReadInt32();
			if (sc < 0 || sc > 2000)
				return;
			int[] switches = new int[sc];
			for (int i = 0; i < sc; i++)
			{
				switches[i] = p.ReadInt32();
				switchesid.Add(switches[i]);
			}
			RazorEnhanced.GumpInspector.GumpResponseAddLogSwitchID(switchesid);
			int ec = p.ReadInt32();
			if (ec < 0 || ec > 2000)
				return;
			GumpTextEntry[] entries = new GumpTextEntry[ec];
			for (int i = 0; i < ec; i++)
			{
				ushort id = p.ReadUInt16();
				ushort len = p.ReadUInt16();
				if (len >= 240)
					return;
				string text = p.ReadUnicodeStringSafe(len);
				entries[i] = new GumpTextEntry(id, text);
				texts.Add(entries[i].Text);
			}
			RazorEnhanced.GumpInspector.GumpResponseAddLogTextID(texts);
			RazorEnhanced.GumpInspector.GumpResponseAddLogEnd();

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_GumpsResponse(tid, bid);
		}

		private static void ChangeSeason(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player != null)
				World.Player.Season = p.ReadByte();
		}

		private static void ExtendedPacket(PacketReader p, PacketHandlerEventArgs args)
		{
			ushort type = p.ReadUInt16();

			switch (type)
			{
				case 0x04: // close gump
					{
						// int serial, int tid
						if (World.Player != null)
						{
							World.Player.HasGump = false;
							World.Player.CurrentGumpI = 0;
							World.Player.CurrentGumpStrings.Clear();
							RazorEnhanced.GumpInspector.GumpCloseAddLog(p, args);
						}
						break;
					}
				case 0x06: // party messages
					{
						OnPartyMessage(p, args);
						break;
					}
				case 0x08: // map change
					{
						if (World.Player != null)
						{
							World.Player.Map = p.ReadByte();
						}
						break;
					}
				case 0x10: // Equip Info
					{
						uint attrib = 0;
						if (World.Player != null)
						{
							uint serial = p.ReadUInt32();
							uint info = p.ReadUInt32();
							uint owner = p.ReadUInt32();
							if (owner == 0xFFFFFFFD)
							{
								ushort nameLengh = p.ReadUInt16();
								string ownername = p.ReadString(nameLengh);
								World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(ownername, "System", 1, World.FindItem(serial).Name));          // Journal buffer
								if (World.Player.Journal.Count > 100)
								{
									RazorEnhanced.Journal.JournalEntry ra;
									World.Player.Journal.TryDequeue(out ra);
								}

								attrib = p.ReadUInt32();
							}
							else
								attrib = owner;

							if (attrib != 0xFFFFFFFC)
							{
								while (attrib != 0xFFFFFFFF)
								{
									try
									{
										ushort charge = p.ReadUInt16();
										World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(charge.ToString(), "System", 1, World.FindItem(serial).Name));          // Journal buffer
										if (World.Player.Journal.Count > 100)
										{
											RazorEnhanced.Journal.JournalEntry ra;
											World.Player.Journal.TryDequeue(out ra);
										}

										World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(Language.GetCliloc((int)attrib), "System", 1, World.FindItem(serial).Name));          // Journal buffer
										if (World.Player.Journal.Count > 100)
										{
											RazorEnhanced.Journal.JournalEntry ra;
											World.Player.Journal.TryDequeue(out ra);
										}
										attrib = p.ReadUInt32();
									}
									catch { }
								}
							}
							else
							{
								World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry("Unidentified", "System", 1, World.FindItem(serial).Name));          // Journal buffer
								if (World.Player.Journal.Count > 100)
								{
									RazorEnhanced.Journal.JournalEntry ra;
									World.Player.Journal.TryDequeue(out ra);
								}
							}
						}
						break;
					}
				case 0x14: // context menu
					{
						p.ReadByte(); // unknown, always 0x00
						int subcommand = p.ReadByte(); // 0x01 for 2D, 0x02 for KR
						UOEntity ent = null;
						Serial ser = p.ReadUInt32();
						if (ser.IsMobile)
							ent = World.FindMobile(ser);
						else if (ser.IsItem)
							ent = World.FindItem(ser);

						if (ent != null)
						{
							byte count = p.ReadByte();

							try
							{
								ent.ContextMenu.Clear();

								for (int i = 0; i < count; i++)
								{
									if (subcommand == 2)
									{
										int num = p.ReadInt32();
										ushort idx = p.ReadUInt16();
										p.ReadUInt16(); // Flags 0x00 = enabled, 0x01 = disabled, 0x04 = highlighted
										ent.ContextMenu.Add(idx, num);
									}
									else
									{
										ushort idx = p.ReadUInt16();
										int num = p.ReadUInt16() + 3000000;
										ushort flags = p.ReadUInt16();
										ushort color = 0;

										if ((flags & 0x02) != 0)
											color = p.ReadUInt16();

										ent.ContextMenu.Add(idx, num);
									}
								}
							}
							catch
							{
							}
						}
						World.Player.HasContext = true;
						World.Player.ContextID = ser;
                        break;
					}
				case 0x18: // map patches
					{
						if (World.Player != null)
						{
							int count = p.ReadInt32() * 2;
							try
							{
								World.Player.MapPatches = new int[count];
								for (int i = 0; i < count; i++)
									World.Player.MapPatches[i] = p.ReadInt32();
							}
							catch
							{
							}
						}
						break;
					}
				case 0x19: //  stat locks
					{
						if (p.ReadByte() == 0x02)
						{
							Mobile m = World.FindMobile(p.ReadUInt32());
							if (World.Player == m && m != null)
							{
								p.ReadByte();// 0?

								byte locks = p.ReadByte();

								World.Player.StrLock = (LockType)((locks >> 4) & 3);
								World.Player.DexLock = (LockType)((locks >> 2) & 3);
								World.Player.IntLock = (LockType)(locks & 3);
							}
						}
						break;
					}
				case 0x1D: // Custom House "General Info"
					{
						Item i = World.FindItem(p.ReadUInt32());
						if (i != null)
							i.HouseRevision = p.ReadInt32();
						break;
					}
				case 0x21: // Special ability execute
					{
						RazorEnhanced.SpellGrid.UpdateSAHighLight(0);
						World.Player.HasSpecial = SpecialMoves.HasPrimary = SpecialMoves.HasSecondary = false;
						break;
					}
			}
		}

		internal static int SpecialPartySent = 0;
		internal static int SpecialPartyReceived = 0;

		private static void RunUOProtocolExtention(PacketReader p, PacketHandlerEventArgs args)
		{
			args.Block = true;

			switch (p.ReadByte())
			{
				case 1: // Custom Party information
					{
						Serial serial;

						PacketHandlers.SpecialPartyReceived++;

						while ((serial = p.ReadUInt32()) > 0)
						{
							Mobile mobile = World.FindMobile(serial);

							short x = p.ReadInt16();
							short y = p.ReadInt16();
							byte map = p.ReadByte();

							if (mobile == null)
							{
								World.AddMobile(mobile = new Mobile(serial));
								mobile.Visible = false;
								if (!mobile.PropsUpdated)
									ClientCommunication.SendToServer(new QueryProperties(mobile.Serial));
								ClientCommunication.SendToServer(new StatusQuery(mobile.Serial));
							}

							if (mobile.Name == null || mobile.Name.Length <= 0)
								mobile.Name = "(Not Seen)";

							if (!m_Party.Contains(serial))
								m_Party.Add(serial);

							mobile.Position = map == World.Player.Map ? new Point3D(x, y, mobile.Position.Z) : Point3D.Zero;
						}

						break;
					}
				case 0xFE: // Begin Handshake/Features Negotiation
					{
						ulong features = p.ReadRawUInt64();

						if (DLLImport.Razor.HandleNegotiate(features))
						{
							ClientCommunication.SendToServer(new RazorNegotiateResponse());
							Engine.MainWindow.UpdateControlLocks();

						}
						break;
					}
			}
		}

		private static List<Serial> m_Party = new List<Serial>();
		internal static List<Serial> Party { get { return m_Party; } }
		private static Timer m_PartyDeclineTimer = null;
		internal static Serial PartyLeader = Serial.Zero;

		private static void OnPartyMessage(PacketReader p, PacketHandlerEventArgs args)
		{
			switch (p.ReadByte())
			{
				case 0x01: // List
					{
						m_Party.Clear();

						int count = p.ReadByte();
						for (int i = 0; i < count; i++)
						{
							Serial s = p.ReadUInt32();
							if (World.Player == null || s != World.Player.Serial)
								m_Party.Add(s);
						}

						break;
					}
				case 0x02: // Remove Member/Re-list
					{
						m_Party.Clear();
						int count = p.ReadByte();
						Serial remSerial = p.ReadUInt32(); // the serial of who was removed

						if (World.Player != null)
						{
							Mobile rem = World.FindMobile(remSerial);
							if (rem != null && !Utility.InRange(World.Player.Position, rem.Position, World.Player.VisRange))
								rem.Remove();
						}

						for (int i = 0; i < count; i++)
						{
							Serial s = p.ReadUInt32();
							if (World.Player == null || s != World.Player.Serial)
								m_Party.Add(s);
						}

						break;
					}
				case 0x03: // text message
				case 0x04: // 3 = private, 4 = public
					{
						Serial from = p.ReadUInt32();
						string text = p.ReadUnicodeStringSafe();
						World.Player.Journal.Enqueue(new RazorEnhanced.Journal.JournalEntry(text, "Party", 0, "null"));          // Journal buffer
						if (World.Player.Journal.Count > 100)
						{
							RazorEnhanced.Journal.JournalEntry ra;
							World.Player.Journal.TryDequeue(out ra);
						}
						break;
					}
				case 0x07: // party invite
					{
						//Serial leader = p.ReadUInt32();
						PartyLeader = p.ReadUInt32();
						if (Assistant.Engine.MainWindow.BlockPartyInviteCheckBox.Checked)                           // AutoDecline Party
						{
							ClientCommunication.SendToServer(new DeclineParty(PacketHandlers.PartyLeader));
						}

						if (RazorEnhanced.Friend.AutoacceptParty && RazorEnhanced.Friend.IsFriend(PartyLeader))     // Autoaccept party from friend
						{
							if (PacketHandlers.PartyLeader != Serial.Zero)
							{
								Assistant.Mobile leader = World.FindMobile(PartyLeader);
								if (leader != null)
								{
									RazorEnhanced.Friend.AddLog("AutoAccept party from: " + leader.Name + " (0x" + leader.Serial.Value.ToString("X8") + ")");
									if (RazorEnhanced.Settings.General.ReadBool("ShowAgentMessageCheckBox"))
										RazorEnhanced.Misc.SendMessage("AutoAccept party from: " + leader.Name + " (0x" + leader.Serial.Value.ToString("X8") + ")");
								}
								else
								{
                                    RazorEnhanced.Friend.AddLog("AutoAccept party from: [NO NAME] (0x" + PartyLeader.Value.ToString("X8") + ")");
									if (RazorEnhanced.Settings.General.ReadBool("ShowAgentMessageCheckBox"))
										RazorEnhanced.Misc.SendMessage("AutoAccept party from: [NO NAME] (0x" + PartyLeader.Value.ToString("X8") + ")");
								}
								ClientCommunication.SendToServer(new AcceptParty(PacketHandlers.PartyLeader));
								PacketHandlers.PartyLeader = Serial.Zero;
							}
						}
						else
						{
							if (m_PartyDeclineTimer == null)
								m_PartyDeclineTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(10.0), new TimerCallback(PartyAutoDecline));
							m_PartyDeclineTimer.Start();
						}
						break;
					}
			}
		}

		private static void PartyAutoDecline()
		{
			PartyLeader = Serial.Zero;
		}

		private static void ClientEncodedPacket(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();
			ushort packetID = p.ReadUInt16();
			switch (packetID)
			{
				case 0x19: // set ability
					{
						int ability = 0;
						if (p.ReadByte() == 0)
						{
							ability = p.ReadInt32();
							RazorEnhanced.SpellGrid.UpdateSAHighLight(ability);
							World.Player.HasSpecial = true;
						}
						break;
					}
			}
		}

		private static string m_LastPW = "";

		private static void ServerListLogin(Packet p, PacketHandlerEventArgs args)
		{
			m_LastPW = "";
			if (!RazorEnhanced.Settings.General.ReadBool("RememberPwds"))
				return;

			World.AccountName = p.ReadStringSafe(30);
			string pass = p.ReadStringSafe(30);

			if (string.IsNullOrEmpty(pass))
			{
				pass = RazorEnhanced.Settings.Password.GetPassword(World.AccountName, ClientCommunication.LastConnection.ToString());
				if (!string.IsNullOrEmpty(pass))
				{
					p.Seek(31, SeekOrigin.Begin);
					p.WriteAsciiFixed(pass, 30);
					m_LastPW = pass;
				}
			}
			else
			{
				RazorEnhanced.Settings.Password.AddUpdateUser(World.AccountName, pass, ClientCommunication.LastConnection.ToString());
			}
		}

		private static void GameLogin(Packet p, PacketHandlerEventArgs args)
		{
			p.ReadInt32(); //authID

			World.AccountName = p.ReadString(30);
			string password = p.ReadString(30);

			if (password == "" && m_LastPW != "" && RazorEnhanced.Settings.General.ReadBool("RememberPwds"))
			{
				p.Seek(35, SeekOrigin.Begin);
				p.WriteAsciiFixed(m_LastPW, 30);
				m_LastPW = "";
			}

			RazorEnhanced.Scripts.Init();
		}

		private static void MenuResponse(PacketReader pvSrc, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			uint serial = pvSrc.ReadUInt32();
			pvSrc.ReadUInt16(); //menuID
			ushort index = pvSrc.ReadUInt16();
			pvSrc.ReadUInt16(); //itemID
			ushort hue = pvSrc.ReadUInt16();

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_MenuResponse(index);

			World.Player.HasMenu = false;
		}

		private static void SendMenu(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			World.Player.CurrentMenuS = p.ReadUInt32();
			World.Player.CurrentMenuI = p.ReadUInt16();

			byte m_questionlenght = p.ReadByte();
			World.Player.MenuQuestionText = p.ReadStringSafe(m_questionlenght);

			byte m_objectcount = p.ReadByte();

			World.Player.MenuEntry.Clear();
            for (int x = 0; x < m_objectcount; x ++)
			{
				ushort m_modelID = 	p.ReadUInt16(); // Model ID
				ushort m_modelColor = p.ReadUInt16(); // Model ID
				byte m_modelTextLenght = p.ReadByte();
				string m_modelText = p.ReadStringSafe(m_modelTextLenght);

				PlayerData.MenuItem m_entry = new PlayerData.MenuItem(m_modelID, m_modelColor, m_modelText);
				World.Player.MenuEntry.Add(m_entry);
			}

			if (RazorEnhanced.Misc.BlockMenu)
				args.Block = true;

			World.Player.HasMenu = true;
		}

		private static void HueResponse(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial serial = p.ReadUInt32();
			ushort iid = p.ReadUInt16();
			ushort hue = p.ReadUInt16();

			if (serial == Serial.MinusOne)
			{
				if (HueEntry.Callback != null)
					HueEntry.Callback(hue);
				args.Block = true;
			}
		}

		private static void ClientAsciiPromptResponse(PacketReader p, PacketHandlerEventArgs args)
		{
			if (!RazorEnhanced.ScriptRecorder.OnRecord)
				return;

			p.ReadUInt32(); // sender serial
			p.ReadUInt32(); // Prompt ID
			uint type = p.ReadUInt32(); // type
			string text = p.ReadUnicodeStringSafe();
			RazorEnhanced.ScriptRecorder.Record_AsciiPromptResponse(type, text);
		}

		private static void ResyncRequest(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player != null)
				World.Player.Resync();
		}

		private static void Features(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player != null)
				World.Player.Features = p.ReadUInt16();
		}

		private static void PersonalLight(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null || args.Block)
				return;

			p.ReadUInt32(); // serial
			World.Player.LocalLightLevel = p.ReadSByte();
		}

		private static void GlobalLight(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player != null && !args.Block)
				World.Player.GlobalLightLevel = p.ReadByte();
		}

		private static void MovementDemand(PacketReader p, PacketHandlerEventArgs args)
		{
			World.Player.ProcessMove((Direction)p.ReadByte());
		}

		private static void ServerSetWarMode(PacketReader p, PacketHandlerEventArgs args)
		{
			World.Player.Warmode = p.ReadBoolean();
		}

		private static void CustomHouseInfo(PacketReader p, PacketHandlerEventArgs args)
		{
			p.ReadByte(); // compression
			p.ReadByte(); // Unknown

			Item i = World.FindItem(p.ReadUInt32());
			if (i != null)
			{
				i.HouseRevision = p.ReadInt32();
				i.HousePacket = p.CopyBytes(0, p.Length);
			}
		}

		private static void CompressedGump(PacketReader p, PacketHandlerEventArgs args)
		{
			if (World.Player == null)
				return;

			List<string> stringlist = new List<string>();
			World.Player.HasGump = true;
			
			World.Player.CurrentGumpS = p.ReadUInt32();
			World.Player.CurrentGumpI = p.ReadUInt32();
			try
			{
				int x = p.ReadInt32(), y = p.ReadInt32();

				string layout = p.GetCompressedReader().ReadString();

				int numStrings = p.ReadInt32();
				if (numStrings < 0 || numStrings > 256)
					numStrings = 0;

				// Split on one or more non-digit characters.
				World.Player.CurrentGumpStrings.Clear();

				string[] numbers = Regex.Split(layout, @"\D+");
				foreach (string value in numbers)
				{
					if (!string.IsNullOrEmpty(value))
					{
						int i = int.Parse(value);
						if ((i >= 500000 && i <= 503405) || (i >= 1000000 && i <= 1155584) || (i >= 3000000 && i <= 3011032))
						{
							World.Player.CurrentGumpStrings.Add(Language.GetString(i));
							stringlist.Add(Language.GetString(i));
						}
					}
				}

				PacketReader pComp = p.GetCompressedReader();
				int len = 0;
				int x1 = 0;
				string[] stringlistparse = new string[numStrings];

				while (!pComp.AtEnd && (len = pComp.ReadInt16()) > 0)
				{
					string tempstring = pComp.ReadUnicodeString(len);
					stringlistparse[x1] = tempstring;
					//World.Player.CurrentGumpStrings.Add(tempstring);
					x1++;
				}

				if (TryParseGump(layout, out string[] gumpPieces))
				{
					stringlist.AddRange(ParseGumpString(gumpPieces, stringlistparse));
				}
				RazorEnhanced.GumpInspector.NewGumpCompressedAddLog(World.Player.CurrentGumpS, World.Player.CurrentGumpI, stringlist);
				World.Player.CurrentGumpStrings.AddRange(stringlist);

				World.Player.CurrentGumpRawData = layout; // Get raw data of current gump
				}
			catch { }
		}

		private static List<string> ParseGumpString(string[] gumpPieces, string[] gumpLines)
		{
			List <string> testipresenti = new List<string>();
			for (int i = 0; i < gumpPieces.Length; i++)
			{
				string[] gumpParams = Regex.Split(gumpPieces[i], @"\s+");
				switch (gumpParams[0].ToLower())
				{

					case "croppedtext":
						testipresenti.Add(gumpLines[int.Parse(gumpParams[6])]);
						//RazorEnhanced.AutoLoot.AddLog("croppedtext " + gumpLines[int.Parse(gumpParams[6])]);
						// CroppedText [x] [y] [width] [height] [color] [text-id]
						// Adds a text field to the gump. gump is similar to the text command, but the text is cropped to the defined area.
						//gump.AddControl(new CroppedText(gump, gumpParams, gumpLines), currentGUMPPage);
						//(gump.LastControl as CroppedText).Hue = 1;
						break;

					case "htmlgump":
						testipresenti.Add(gumpLines[int.Parse(gumpParams[5])]);
						//RazorEnhanced.AutoLoot.AddLog("htmlgump " + gumpLines[int.Parse(gumpParams[5])]);
						// HtmlGump [x] [y] [width] [height] [text-id] [background] [scrollbar]
						// Defines a text-area where Html-commands are allowed.
						// [background] and [scrollbar] can be 0 or 1 and define whether the background is transparent and a scrollbar is displayed.
						//	gump.AddControl(new HtmlGumpling(gump, gumpParams, gumpLines), currentGUMPPage);
						break;

					case "text":
						testipresenti.Add(gumpLines[int.Parse(gumpParams[4])]);
						//RazorEnhanced.AutoLoot.AddLog("text " + gumpLines[int.Parse(gumpParams[4])]);
						// Text [x] [y] [color] [text-id]
						// Defines the position and color of a text (data) entry.
						//gump.AddControl(new TextLabel(gump, gumpParams, gumpLines), currentGUMPPage);
						break;
				}
			}

			return testipresenti;
		}

		private static bool TryParseGump(string gumpData, out string[] pieces)
		{
			List<string> i = new List<string>();
			int dataIndex = 0;
			while (dataIndex < gumpData.Length)
			{
				if (gumpData.Substring(dataIndex) == "\0")
				{
					break;
				}
				else
				{
					int begin = gumpData.IndexOf("{", dataIndex);
					int end = gumpData.IndexOf("}", dataIndex + 1);
					if ((begin != -1) && (end != -1))
					{
						string sub = gumpData.Substring(begin + 1, end - begin - 1).Trim();
						i.Add(sub);
						dataIndex = end;
					}
					else
					{
						break;
					}
				}
			}

			pieces = i.ToArray();
			return (pieces.Length > 0);
		}

		private static void BuffDebuff(PacketReader p, PacketHandlerEventArgs args)
		{
			Serial ser = p.ReadUInt32();
			ushort icon = p.ReadUInt16();
			ushort action = p.ReadUInt16();

			if (Enum.IsDefined(typeof(BuffIcon), icon))
			{
				BuffIcon buff = (BuffIcon)icon;
				switch (action)
				{
					case 0x01: // show
						if (World.Player != null && !World.Player.Buffs.Contains(buff))
						{
							World.Player.Buffs.Add(buff);
						}
						break;

					case 0x0: // remove
						if (World.Player != null && World.Player.Buffs.Contains(buff))
						{
							World.Player.Buffs.Remove(buff);
						}
						break;
				}

				// Highlight for bloodOath
				if (Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked && buff == BuffIcon.BloodOathCurse)
					RazorEnhanced.Filters.ApplyColor(World.Player, Convert.ToBoolean(action));
			}
		}

		private static void AttackRequest(Packet p, PacketHandlerEventArgs args)
		{
			uint serialbersaglio = p.ReadUInt32();

			if (RazorEnhanced.ScriptRecorder.OnRecord)
				RazorEnhanced.ScriptRecorder.Record_AttackRequest(serialbersaglio);

			if (RazorEnhanced.Friend.PreventAttack)
			{
				if (RazorEnhanced.Friend.IsFriend((int)serialbersaglio))
				{
					Assistant.Mobile bersaglio = World.FindMobile(serialbersaglio);
					if (bersaglio != null)
					{
						RazorEnhanced.Friend.AddLog("Can't attack a friend player: " + bersaglio.Name + " (0x" + bersaglio.Serial.Value.ToString("X8") + ")");
						RazorEnhanced.Misc.SendMessage("Can't attack a friend player: " + bersaglio.Name + " (0x" + bersaglio.Serial.Value.ToString("X8") + ")");
					}
					args.Block = true;
					return;
				}
			}
			Targeting.LastAttack = serialbersaglio;
        }

		private static void TradeRequest(PacketReader p, PacketHandlerEventArgs args)
		{
			if (Assistant.Engine.MainWindow.BlockTradeRequestCheckBox.Checked)
			{
				args.Block = true;
			}
		}
	}
}