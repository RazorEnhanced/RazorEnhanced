using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace Assistant
{
    internal enum MessageType
    {
        Regular = 0x00,
        System = 0x01,
        Emote = 0x02,
        Label = 0x06,
        Focus = 0x07,
        Whisper = 0x08,
        Yell = 0x09,
        Spell = 0x0A,
        Guild = 0x0D,
        Alliance = 0x0E,
        Encoded = 0xC0,

        Special = 0x20,
    }

    internal sealed class EmoteAction : Packet
    {
        public EmoteAction(string action) : base(0x12)
        {
            EnsureCapacity(1 + action.Length);
            Write((byte)0xC7);
            WriteAsciiNull(action);
        }
    }

    internal sealed class QueryPartyLocs : Packet
    {
        internal QueryPartyLocs()
            : base(0xF0)
        {
            EnsureCapacity(4);
            Write((byte)0x00);
        }
    }

    internal sealed class AcceptParty : Packet
    {
        internal AcceptParty(Serial leader)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort)0x06); // party command
            Write((byte)0x08); // accept
            Write((uint)leader);
        }
    }

    internal sealed class DeclineParty : Packet
    {
        internal DeclineParty(Serial leader)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort)0x06); // party command
            Write((byte)0x09); // decline
            Write((uint)leader);
        }
    }

    internal sealed class ContainerContent : Packet
    {
        internal ContainerContent(List<Item> items)
            : this(items, Engine.UsePostKRPackets)
        {
        }

        internal ContainerContent(List<Item> items, bool useKR)
            : base(0x3C)
        {
            Write((ushort)items.Count);

            foreach (Item item in items)
            {
                Write((uint)item.Serial);
                Write((ushort)item.ItemID);
                Write((sbyte)0);
                Write((ushort)item.Amount);
                Write((ushort)item.Position.X);
                Write((ushort)item.Position.Y);

                if (useKR)
                    Write((byte)item.GridNum);

                if (item.Container is Item)
                    Write((uint)((Item)item.Container).Serial);
                else
                    Write((uint)0);
                Write((ushort)item.Hue);
            }
        }
    }

    internal sealed class ContainerItem : Packet
    {
        internal ContainerItem(Item item)
            : this(item, Engine.UsePostKRPackets)
        {
        }

        internal ContainerItem(Item item, bool isKR)
            : base(0x25, 20)
        {
            if (isKR)
                EnsureCapacity(21);

            Write(item.Serial);

            Write(item.ItemID);
            Write((byte)0);
            Write(item.Amount);
            Write((ushort)item.Position.X);
            Write((ushort)item.Position.Y);

            if (isKR)
                Write(item.GridNum);

            object cont = item.Container;
            if (cont is UOEntity)
                Write((uint)((UOEntity)item.Container).Serial);
            else if (cont is uint)
                Write((uint)cont);
            else if (cont is Serial)
                Write((Serial)item.Container);
            else
                Write((uint)0x7FFFFFFF);

            Write(item.Hue);
        }
    }

    internal sealed class SingleClick : Packet
    {
        internal SingleClick(object clicked)
            : base(0x09, 5)
        {
            if (clicked is Mobile)
                Write(((Mobile)clicked).Serial);
            else if (clicked is Item)
                Write(((Item)clicked).Serial);
            else if (clicked is Serial)
                Write(((Serial)clicked).Value);
            else
                Write((uint)0);
        }
    }

    internal sealed class DoubleClick : Packet
    {
        internal DoubleClick(Serial clicked)
            : base(0x06, 5)
        {
            Write((uint)clicked.Value);
        }
    }

    internal sealed class Target : Packet
    {
        internal Target(uint tid)
            : this(tid, false, 0)
        {
        }

        internal Target(uint tid, byte flags)
            : this(tid, false, flags)
        {
        }

        internal Target(uint tid, bool ground)
            : this(tid, ground, 0)
        {
        }

        internal Target(uint tid, bool ground, byte flags)
            : base(0x6C, 19)
        {
            Write(ground);
            Write(tid);
            Write(flags);
            Fill();
        }
    }

    internal sealed class TargetResponse : Packet
    {
        internal TargetResponse(TargetInfo info)
            : base(0x6C, 19)
        {
            Write((byte)info.Type);
            Write((uint)info.TargID);
            Write((byte)info.Flags);
            Write((uint)info.Serial);
            Write((ushort)info.X);
            Write((ushort)info.Y);
            Write((short)info.Z);
            Write((ushort)info.Gfx);
        }

        internal TargetResponse(uint id, Mobile m)
            : base(0x6C, 19)
        {
            Write((byte)0x00); // target object
            Write((uint)id);
            Write((byte)0); // flags
            Write((uint)m.Serial);
            Write((ushort)m.Position.X);
            Write((ushort)m.Position.Y);
            Write((short)m.Position.Z);
            Write((ushort)m.Body);
        }

        internal TargetResponse(uint id, Item item)
            : base(0x6C, 19)
        {
            Write((byte)0x00); // target object
            Write((uint)id);
            Write((byte)0); // flags
            Write((uint)item.Serial);
            Write((ushort)item.Position.X);
            Write((ushort)item.Position.Y);
            Write((short)item.Position.Z);
            Write((ushort)item.ItemID);
        }
    }

    internal sealed class TargetCancelResponse : Packet
    {
        internal TargetCancelResponse(uint id)
            : base(0x6C, 19)
        {
            Write((byte)0);
            Write((uint)id);
            Write((byte)0);
            Write((uint)0);
            Write((ushort)0xFFFF);
            Write((ushort)0xFFFF);
            Write((short)0);
            Write((ushort)0);
        }
    }

    internal sealed class AttackReq : Packet
    {
        internal AttackReq(Serial serial)
            : base(0x05, 5)
        {
            Write((uint)serial);
        }
    }

    internal sealed class CancelTarget : Packet
    {
        internal CancelTarget(uint id)
            : base(0x6C, 19)
        {
            Write((byte)0);
            Write((uint)id);
            Write((byte)3);
            Fill();
        }
    }

    internal sealed class SkillsQuery : Packet
    {
        internal SkillsQuery(Mobile m)
            : base(0x34, 10)
        {
            Write((uint)0xEDEDEDED); // que el fuck, osi
            Write((byte)0x05);
            Write(m.Serial);
        }
    }

    internal sealed class StatusQuery : Packet
    {
        internal StatusQuery(Serial s)
            : base(0x34, 10)
        {
            Write((uint)0xEDEDEDED);
            Write((byte)0x04);
            Write(s);
        }
    }

    internal sealed class StatLockInfo : Packet
    {
        internal StatLockInfo(PlayerData m)
            : base(0xBF)
        {
            this.EnsureCapacity(12);

            Write((short)0x19);
            Write((byte)2);
            Write((int)m.Serial);
            Write((byte)0);

            int lockBits = 0;

            lockBits |= (int)m.StrLock << 4;
            lockBits |= (int)m.DexLock << 2;
            lockBits |= (int)m.IntLock;

            Write((byte)lockBits);
        }
    }

    internal sealed class SkillsList : Packet
    {
        internal SkillsList()
            : base(0x3A)
        {
            EnsureCapacity(3 + 1 + Skill.Count * 9 + 2);

            Write((byte)0x02);
            for (int i = 0; i < Skill.Count; i++)
            {
                Write((short)(i + 1));
                Write(World.Player.Skills[i].FixedValue);
                Write(World.Player.Skills[i].FixedBase);
                Write((byte)World.Player.Skills[i].Lock);
                Write(World.Player.Skills[i].FixedCap);
            }
            Write((short)0);
        }
    }

    internal sealed class SkillUpdate : Packet
    {
        internal SkillUpdate(Skill s)
            : base(0x3A)
        {
            EnsureCapacity(3 + 1 + 9);

            Write((byte)0xDF);

            Write((short)s.Index);
            Write((ushort)s.FixedValue);
            Write((ushort)s.FixedBase);
            Write((byte)s.Lock);
            Write((ushort)s.FixedCap);
        }
    }

    internal sealed class SetSkillLock : Packet
    {
        internal SetSkillLock(int skill, LockType type)
            : base(0x3A)
        {
            EnsureCapacity(6);
            Write((short)skill);
            Write((byte)type);
        }
    }

    internal sealed class SetStatLock : Packet
    {
        internal SetStatLock(int statType, LockType type)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 1);
            Write((short)0x1A);
            Write((byte)(statType));
            Write((byte)(type));
        }
    }



    internal sealed class AsciiMessage : Packet
    {
        internal AsciiMessage(Serial serial, int graphic, MessageType type, int hue, int font, string name, string text)
            : base(0x1C)
        {
            if (name == null) name = "";
            if (text == null) text = "";

            if (hue == 0)
                hue = 0x3B2;

            this.EnsureCapacity(45 + text.Length);

            Write((uint)serial);
            Write((short)graphic);
            Write((byte)type);
            Write((short)hue);
            Write((short)font);
            WriteAsciiFixed(name, 30);
            WriteAsciiNull(text);
        }
    }

    internal sealed class ClientAsciiMessage : Packet
    {
        internal ClientAsciiMessage(MessageType type, int hue, int font, string str)
            : base(0x03)
        {
            EnsureCapacity(1 + 2 + 1 + 2 + 2 + str.Length + 1);

            Write((byte)type);
            Write((short)hue);
            Write((short)font);
            WriteAsciiNull(str);
        }
    }

    internal sealed class UnicodeMessage : Packet
    {
        internal UnicodeMessage(Serial serial, int graphic, MessageType type, int hue, int font, string lang, string name, string text)
            : base(0xAE)
        {
            if (string.IsNullOrEmpty(lang)) lang = "ENU";
            if (name == null) name = "";
            if (text == null) text = "";

            if (hue == 0)
                hue = 0x3B2;

            this.EnsureCapacity(50 + (text.Length * 2));

            Write((uint)serial);
            Write((ushort)graphic);
            Write((byte)type);
            Write((ushort)hue);
            Write((ushort)font);
            WriteAsciiFixed(lang.ToUpper(), 4);
            WriteAsciiFixed(name, 30);
            WriteBigUniNull(text);
        }
    }

    internal sealed class ClientUniMessage : Packet
    {
        internal ClientUniMessage(MessageType type, int hue, int font, string lang, List<ushort> keys, string text)
            : base(0xAD)
        {
            if (string.IsNullOrEmpty(lang)) lang = "ENU";
            if (text == null) text = "";

            this.EnsureCapacity(50 + (text.Length * 2) + (keys == null ? 0 : keys.Count + 1));
            if (keys == null || keys.Count <= 1)
                Write((byte)type);
            else
                Write((byte)(type | MessageType.Encoded));
            Write((short)hue);
            Write((short)font);
            WriteAsciiFixed(lang, 4);
            if (keys != null && keys.Count > 1)
            {
                Write((ushort)keys[0]);
                for (int i = 1; i < keys.Count; i++)
                    Write((byte)keys[i]);
                WriteUTF8Null(text);
            }
            else
            {
                WriteBigUniNull(text);
            }
        }
    }

    internal sealed class LiftRequest : Packet
    {
        internal LiftRequest(Serial ser, int amount)
            : base(0x07, 7)
        {
            this.Write(ser.Value);
            this.Write((ushort)amount);
        }

        internal LiftRequest(Item i, int amount)
            : this(i.Serial, amount)
        {
        }

        internal LiftRequest(Item i)
            : this(i.Serial, i.Amount)
        {
        }
    }

    internal sealed class LiftRej : Packet
    {
        internal LiftRej()
            : this(5) // reason = Inspecific
        {
        }

        internal LiftRej(byte reason)
            : base(0x27, 2)
        {
            Write(reason);
        }
    }

    internal sealed class EquipRequest : Packet
    {
        internal EquipRequest(Serial item, Mobile to, Layer layer)
            : base(0x13, 10)
        {
            Write(item);
            Write((byte)layer);
            Write(to.Serial);
        }

        internal EquipRequest(Serial item, Serial to, Layer layer)
            : base(0x13, 10)
        {
            Write(item);
            Write((byte)layer);
            Write(to);
        }
    }

    internal sealed class DropRequest : Packet
    {
        internal DropRequest(Item item, Serial destSer)
            : base(0x08, 14)
        {
            if (Engine.UsePostKRPackets)
                EnsureCapacity(15);

            Write(item.Serial);
            Write((short)(-1));
            Write((short)(-1));
            Write((sbyte)0);
            if (Engine.UsePostKRPackets)
                Write((byte)0);
            Write(destSer);
        }

        internal DropRequest(Item item, Item to)
            : this(item, to.Serial)
        {
        }

        internal DropRequest(Serial item, Point3D pt, Serial dest)
            : base(0x08, 14)
        {
            if (Engine.UsePostKRPackets)
                EnsureCapacity(15);

            Write(item);
            Write((ushort)pt.X);
            Write((ushort)pt.Y);
            Write((sbyte)pt.Z);
            if (Engine.UsePostKRPackets)
                Write((byte)0);
            Write(dest);
        }

        internal DropRequest(Item item, Point3D pt, Serial destSer)
            : this(item.Serial, pt, destSer)
        {
        }
    }

    internal class SellListItem
    {
        internal Serial Serial;
        internal ushort Amount;

        internal SellListItem(Serial s, ushort a)
        {
            Serial = s;
            Amount = a;
        }
    }

    internal sealed class VendorSellResponse : Packet
    {
        internal VendorSellResponse(Mobile vendor, List<SellListItem> list)
            : base(0x9F)
        {
            EnsureCapacity(1 + 2 + 4 + 2 + list.Count * 6);

            Write((uint)vendor.Serial);
            Write((ushort)list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                SellListItem sli = (SellListItem)list[i];
                Write((uint)sli.Serial);
                Write((ushort)sli.Amount);
            }
        }
    }

    internal sealed class MobileStatusExtended : Packet
    {
        internal MobileStatusExtended(PlayerData m)
            : base(0x11)
        {
            string name = m.Name;
            if (name == null) name = "";

            this.EnsureCapacity(88);

            Write((uint)m.Serial);
            WriteAsciiFixed(name, 30);

            Write((short)m.Hits);
            Write((short)m.HitsMax);

            Write(false); // cannot edit name

            Write((byte)0x03); // no aos info

            Write(m.Female);

            Write((short)m.Str);
            Write((short)m.Dex);
            Write((short)m.Int);

            Write((short)m.Stam);
            Write((short)m.StamMax);

            Write((short)m.Mana);
            Write((short)m.ManaMax);

            Write((int)m.Gold);
            Write((short)m.AR);
            Write((short)m.Weight);
            Write((short)m.StatCap);
            Write((byte)m.Followers);
            Write((byte)m.FollowersMax);
        }
    }

    internal sealed class MobileStatusCompact : Packet
    {
        internal MobileStatusCompact(Mobile m)
            : base(0x11)
        {
            string name = m.Name;
            if (name == null) name = "";

            this.EnsureCapacity(88);

            Write((uint)m.Serial);
            WriteAsciiFixed(name, 30);

            Write((short)m.Hits);
            Write((short)m.HitsMax);

            Write(false); // cannot edit name

            Write((byte)0x00); // no aos info
        }
    }

    internal sealed class MoveRequest : Packet
    {
        internal MoveRequest(byte seq, Direction dir, bool run)
            : base(0x02, 7)
        {
            if (run)
            {
                dir |= Direction.running;
            }
            Write((byte)dir);
            Write((byte)seq);
            //Write( (uint)Utility.Random( 0x7FFFFFFF ) ); // fastwalk key (unused)
            Write((uint)0);
            //if (PlayerData.FastWalkKey < 5)
            //    Write((uint)0xBAADF00D);
            //else
            //    Write((uint)0);
            //PlayerData.FastWalkKey++;
        }
    }

    internal sealed class MoveReject : Packet
    {
        internal MoveReject(byte seq, Mobile m)
            : base(0x21, 8)
        {
            Write((byte)seq);
            Write((short)m.Position.X);
            Write((short)m.Position.Y);
            Write((byte)m.Direction);
            Write((sbyte)m.Position.Z);
        }
    }

    internal sealed class MoveAcknowledge : Packet
    {
        internal MoveAcknowledge(byte seq, byte noto)
            : base(0x22, 3)
        {
            Write((byte)seq);
            Write((byte)noto);
        }
    }

    internal sealed class GumpTextEntry
    {
        internal GumpTextEntry(ushort id, string s)
        {
            EntryID = id;
            Text = s;
        }

        internal ushort EntryID;
        internal string Text;
    }

    internal sealed class GumpResponse : Packet
    {
        internal GumpResponse(uint serial, uint tid, int bid, int[] switches, GumpTextEntry[] entries)
            : base(0xB1)
        {
            int txtlenght = 0;
            foreach (GumpTextEntry t in entries) // Calculate dynamic lenght of all text
                txtlenght += t.Text.Length;

            EnsureCapacity(3 + 4 + 4 + 4 + 4 + switches.Length * 4 + 4 + entries.Length * 4 + txtlenght * 2);

            Write((uint)serial);
            Write((uint)tid);
            Write((int)bid);

            Write((int)switches.Length);
            foreach (int t in switches)
                Write((int)t);

            Write((int)entries.Length);
            foreach (GumpTextEntry t in entries)
            {
                Write((short)t.EntryID);
                Write((short)t.Text.Length);
                WriteBigUniFixed(t.Text, t.Text.Length);
            }
        }
    }

    internal sealed class UseSkill : Packet
    {
        internal UseSkill(int sk)
            : base(0x12)
        {
            string cmd = String.Format("{0} 0", sk);
            EnsureCapacity(4 + cmd.Length + 1);
            Write((byte)0x24);
            WriteAsciiNull(cmd);
        }

        internal UseSkill(SkillName sk)
            : this((int)sk)
        {
        }
    }

    internal sealed class TrackingArrow : Packet
    {
        internal TrackingArrow(Serial target, bool display, ushort mx, ushort my)
            : base(0xBA, 12)
        {
            EnsureCapacity(12);
            Write(display ? (byte)1 : (byte)0);
            Write((ushort)(mx));
            Write((ushort)(my));
            Write((uint)target);
        }
    }

    internal sealed class ExtCastSpell : Packet
    {
        internal ExtCastSpell(Serial book, ushort spell)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 2 + 4 + 2);
            Write((short)0x1C);
            Write((short)(book.IsItem ? 1 : 2));
            if (book.IsItem)
                Write((uint)book);
            Write((short)spell);
        }
    }

    internal sealed class CastSpellFromBook : Packet
    {
        internal CastSpellFromBook(Serial book, ushort spell)
            : base(0x12)
        {
            string cmd;
            cmd = book.IsItem ? String.Format("{0} {1}", spell, book.Value) : String.Format("{0}", spell);
            EnsureCapacity(3 + 1 + cmd.Length + 1);
            Write((byte)0x27);
            WriteAsciiNull(cmd);
        }
    }

    internal sealed class CastSpellFromMacro : Packet
    {
        internal CastSpellFromMacro(ushort spell)
            : base(0x12)
        {
            string cmd = spell.ToString();
            EnsureCapacity(3 + 1 + cmd.Length + 1);
            Write((byte)0x56);
            WriteAsciiNull(cmd);
        }
    }

    internal sealed class UseTargetedItem : Packet
    {
        internal UseTargetedItem(uint useItem, uint target)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4 + 4);
            Write((ushort)0x2C); // use targeted item
            Write(useItem); // use item
            Write((uint)target); // on target
        }
    }

    internal sealed class CastTargetedSpell : Packet
    {
        internal CastTargetedSpell(ushort spell, uint target)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 2 + 4);
            Write((ushort)0x2D); // cast targeted spell
            Write(spell); // spell id
            Write((uint)target);
        }
    }

    internal sealed class UseTargetedSkill : Packet
    {
        internal UseTargetedSkill(ushort useSkill, uint target)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 2 + 4);
            Write((ushort)0x2E); // use skill
            Write(useSkill); // skill
            Write((uint)target); // on target
        }
    }
    internal sealed class TargeByResource : Packet
    {
        internal TargeByResource(uint Serial, uint ResourceType)
            : base(0xBF)
        {
            EnsureCapacity(11);
            Write((ushort)0x30); // command
            Write((uint)Serial); // Tools
            Write((ushort)ResourceType); // Resource type
        }
    }

    internal sealed class DisarmRequest : Packet
    {
        internal DisarmRequest()
            : base(0xBF)
        {
            EnsureCapacity(3);
            Write((ushort)0x09);
        }
    }

    internal sealed class StunRequest : Packet
    {
        internal StunRequest()
            : base(0xBF)
        {
            EnsureCapacity(3);
            Write((ushort)0x0A);
        }
    }

    internal sealed class UseItemOnTarget : Packet
    {
        internal UseItemOnTarget(int iserial, int tserial)
            : base(0xBF)
        {
            EnsureCapacity(12);
            Write((ushort)0x2C); // Command
            Write((uint)iserial);
            Write((uint)tserial);

        }
    }

    // Doesn't seem to work right, puts up a fixed gump, ignores text 
    internal sealed class DisplaySignGump : Packet
    {
        internal DisplaySignGump()
            : base(0x8B)
        {
            string text = "Text Test";
            string caption = "Caption Test";
            int gumpid = 990099;
            EnsureCapacity(3 + 4 + 2 + 2 + text.Length + 2 + caption.Length);
            Write((int)World.Player.Serial);
            Write((ushort)gumpid);
            Write((ushort)text.Length);
            WriteAsciiNull(text);
            Write((ushort)caption.Length);
            WriteAsciiNull(caption);
        }
    }

    // Doesn't seem to work right, puts up a fixed gump, ignores text 
    internal sealed class GenericGump : Packet
    {
        internal GenericGump(uint gumpid, uint serial, uint x, uint y,
            string gumpDefinition, List<string> gumpStrings)
            : base(0xDD)
        {
            string compGumpEntries = "";
            string compGumpStrings = "";
            uint gumpId = gumpid;
            uint gumpSerial = serial;
            uint gumpX = x;
            uint gumpY = y;
            //uint packedLength = 182;
            //uint linesCount = 0;
            //uint uncompGumpStringsLength = 543;
            EnsureCapacity(4 + 4 + 4 + 4 + 4 + 4 + compGumpEntries.Length + 4 + 4 + 4 + 4 + compGumpStrings.Length);
            Write((uint)gumpSerial);
            Write((uint)gumpId);
            Write((uint)gumpX);
            Write((uint)gumpY);

            byte[] dest = new byte[gumpDefinition.Length]; // compressed SHOULD be smalled than uncompressed
            int destLen = dest.Length;
            bool worked = (DLLImport.ZLib.compress(dest, ref destLen, System.Text.Encoding.ASCII.GetBytes(gumpDefinition), gumpDefinition.Length) == ZLibError.Z_OK);
            Write((uint)destLen + 4);
            Write((uint)gumpDefinition.Length);
            Write((byte[])dest, 0, destLen);
            Write((uint)gumpStrings.Count);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                foreach (string s in gumpStrings)
                {
                    short len = (short)s.Length;
                    byte[] lenBytes = BitConverter.GetBytes(len);
                    ms.WriteByte(lenBytes[1]);
                    ms.WriteByte(lenBytes[0]);
                    char[] charArray = new char[len];
                    s.CopyTo(0, charArray, 0, len);
                    byte[] bytes = System.Text.Encoding.BigEndianUnicode.GetBytes(s);
                    ms.Write(bytes, 0, bytes.Length);
                }
                //ms.Flush();
                byte[] textBuffer = ms.ToArray();
                int compressedSize = textBuffer.Length + 10;
                byte[] compressedData = new byte[compressedSize + 10]; // compressed SHOULD be smalled than uncompressed
                ZLibError compResult2 = DLLImport.ZLib.compress(compressedData, ref compressedSize, textBuffer, textBuffer.Length);
                bool worked2 = (compResult2 == ZLibError.Z_OK);

                Write((uint)compressedSize + 4);
                Write((uint)textBuffer.Length);
                Write((byte[])compressedData, 0, compressedSize);
            }
        }
    }


    internal sealed class CloseGump : Packet
    {
        internal CloseGump(uint typeID, uint buttonID)
            : base(0xBF)
        {
            EnsureCapacity(13);

            Write((short)0x04);
            Write((int)typeID);
            Write((int)buttonID);
        }

        internal CloseGump(uint typeID)
            : base(0xBF)
        {
            EnsureCapacity(13);

            Write((short)0x04);
            Write((int)typeID);
            Write((int)0);
        }
    }

    internal sealed class ChangeCombatant : Packet
    {
        internal ChangeCombatant(Serial ser)
            : base(0xAA, 5)
        {
            Write((uint)ser);
        }

        internal ChangeCombatant(Mobile m)
            : this(m.Serial)
        {
        }
    }

    internal sealed class UseAbility : Packet
    {
        // ints are 'encoded' with a leading bool, if true then the number is 0, if false then followed by all 4 bytes (lame :-)
        internal UseAbility(AOSAbility abilityIndex)
            : base(0xD7)
        {
            EnsureCapacity(15);

            Write((uint)World.Player.Serial);
            Write((ushort)0x19);
            Write((uint)0);
            Write((byte)abilityIndex);
            Write((byte)0x0A);
        }
    }
    internal sealed class EquipLastWeapon : Packet
    {
        internal EquipLastWeapon()
            : base(0xD7)
        {
            EnsureCapacity(10);
            Write((uint)World.Player.Serial);
            Write((ushort)0x1E);
            Write((byte)0x0A);
        }
    }


    internal sealed class ClearAbility : Packet
    {
        internal static readonly Packet Instance = new ClearAbility();

        internal ClearAbility()
            : base(0xBF)
        {
            EnsureCapacity(5);

            Write((short)0x21);
        }
    }

    internal sealed class PingPacket : Packet
    {
        internal PingPacket(byte seq)
            : base(0x73, 2)
        {
            Write(seq);
        }
    }

    internal sealed class MobileIncoming : Packet
    {
        internal MobileIncoming(Mobile m)
            : base(0x78)
        {
            int count = m.Contains.Count;
            int ltHue = Engine.MainWindow.LTHilight;
            int hue = m.Hue;

            if (ltHue != 0 && Targeting.IsLastTarget(m))
                hue = ltHue;
            else   // Inizio controllo flag
            {
                if (m.Poisoned) // Caso Poison
                    hue = (int)RazorEnhanced.Filters.HighLightColor.Poison;
                else if (m.Blessed) // Caso Mortal
                    hue = (int)RazorEnhanced.Filters.HighLightColor.Mortal;
                else if (m.Paralized) // Caso Paral
                    hue = (int)RazorEnhanced.Filters.HighLightColor.Paralized;
            }

            EnsureCapacity(3 + 4 + 2 + 2 + 2 + 1 + 1 + 2 + 1 + 1 + 4 + count * (4 + 2 + 1 + 2));
            Write((uint)m.Serial);
            Write((ushort)m.Body);
            Write((ushort)m.Position.X);
            Write((ushort)m.Position.Y);
            Write((sbyte)m.Position.Z);
            Write((byte)m.Direction);
            Write((ushort)hue);
            Write((byte)m.GetPacketFlags());
            Write((byte)m.Notoriety);

            for (int i = 0; i < count; ++i)
            {
                Item item = (Item)m.Contains[i];
                Write((uint)item.Serial);
                Write((ushort)item.ItemID);
                Write((byte)item.Layer);
                if (ltHue != 0 && Targeting.IsLastTarget(m))
                    Write((ushort)ltHue);
                else   // Inizio controllo flag
                {
                    if (m.Poisoned) // Caso Poison
                        hue = (int)RazorEnhanced.Filters.HighLightColor.Poison;
                    else if (m.Blessed) // Caso Mortal
                        hue = (int)RazorEnhanced.Filters.HighLightColor.Mortal;
                    else if (m.Paralized) // Caso Paral
                        hue = (int)RazorEnhanced.Filters.HighLightColor.Paralized;
                    else
                        Write((ushort)item.Hue);
                }
            }
            Write((uint)0); // terminate
        }
    }

    internal class VendorBuyItem
    {
        internal VendorBuyItem(Serial ser, int amount, int price)
        {
            Serial = ser;
            Amount = amount;
            Price = price;
        }

        internal readonly Serial Serial;
        internal int Amount;
        internal int Price;

        internal int TotalCost { get { return Amount * Price; } }
    }

    internal sealed class VendorBuyResponse : Packet
    {
        internal VendorBuyResponse(Serial vendor, List<VendorBuyItem> list)
            : base(0x3B)
        {
            EnsureCapacity(1 + 2 + 4 + 1 + list.Count * 7);

            Write(vendor);
            Write((byte)0x02); // flag

            foreach (VendorBuyItem vbi in list)
            {
                Write((byte)0x1A); // layer?
                Write(vbi.Serial);
                Write((ushort)vbi.Amount);
            }
        }
    }

    internal sealed class MenuResponse : Packet
    {
        internal MenuResponse(uint serial, ushort menuid, ushort index, ushort itemid, ushort hue)
            : base(0x7D, 13)
        {
            Write((uint)serial);
            Write(menuid);
            Write(index);
            Write(itemid);
            Write(hue);
        }
    }

    internal sealed class HuePicker : Packet
    {
        internal HuePicker()
            : this(Serial.MinusOne, 0x0FAB)
        {
        }

        internal HuePicker(ItemID itemid)
            : this(Serial.MinusOne, itemid)
        {
        }

        internal HuePicker(Serial target, ItemID itemid)
            : base(0x95, 9)
        {
            //BYTE[4] itemID of dye target
            //BYTE[2] ignored on send, model on return
            //BYTE[2] model on send, color on return from client (default on server send is 0x0FAB)
            Write((uint)target);
            Write((ushort)0);
            Write((ushort)itemid);
        }

        internal HuePicker(Serial target, ItemID model, ushort color)
            : base(0x95, 9)
        {
            //BYTE[4] itemID of dye target
            //BYTE[2] ignored on send, model on return
            //BYTE[2] model on send, color on return from client (default on server send is 0x0FAB)
            Write((uint)target);
            Write((ushort)model);
            Write((ushort)color);
        }

    }

    internal sealed class WalkRequest : Packet
    {
        internal WalkRequest(Direction dir, byte seq)
            : base(0x02, 7)
        {
            Write((byte)dir);
            Write(seq);
            Write((int)-1); // key
        }
    }

    internal sealed class ResyncReq : Packet
    {
        internal ResyncReq()
            : base(0x22, 3)
        {
            Write((ushort)0);
        }
    }
    public sealed class ACKTalk : Packet
    {
        public ACKTalk() : base(0x03)
        {
            EnsureCapacity(40);
            Write((byte)0x20);
            Write((byte)0x00);
            Write((byte)0x34);
            Write((byte)0x00);
            Write((byte)0x03);
            Write((byte)0xdb);
            Write((byte)0x13);
            Write((byte)0x14);
            Write((byte)0x3f);
            Write((byte)0x45);
            Write((byte)0x2c);
            Write((byte)0x58);
            Write((byte)0x0f);
            Write((byte)0x5d);
            Write((byte)0x44);
            Write((byte)0x2e);
            Write((byte)0x50);
            Write((byte)0x11);
            Write((byte)0xdf);
            Write((byte)0x75);
            Write((byte)0x5c);
            Write((byte)0xe0);
            Write((byte)0x3e);
            Write((byte)0x71);
            Write((byte)0x4f);
            Write((byte)0x31);
            Write((byte)0x34);
            Write((byte)0x05);
            Write((byte)0x4e);
            Write((byte)0x18);
            Write((byte)0x1e);
            Write((byte)0x72);
            Write((byte)0x0f);
            Write((byte)0x59);
            Write((byte)0xad);
            Write((byte)0xf5);
            Write((byte)0x00);
        }
    }

    internal sealed class WorldItem : Packet
    {
        internal WorldItem(Item item)
            : base(0x1A)
        {
            this.EnsureCapacity(20);

            // 14 base length
            // +2 - Amount
            // +2 - Hue
            // +1 - Flags

            uint serial = (uint)item.Serial;
            ushort itemID = item.ItemID;
            ushort amount = item.Amount;
            int x = item.Position.X;
            int y = item.Position.Y;
            ushort hue = item.Hue;
            byte flags = item.GetPacketFlags();
            byte direction = item.Direction;

            if (amount != 0)
                serial |= 0x80000000;
            else
                serial &= 0x7FFFFFFF;
            Write((uint)serial);
            ushort maskedItemID = (ushort)(itemID & 0x7FFF);
            Write(maskedItemID);
            if (amount != 0)
                Write((ushort)amount);

            x &= 0x7FFF;
            if (direction != 0)
                x |= 0x8000;
            Write((ushort)x);

            y &= 0x3FFF;
            if (hue != 0)
                y |= 0x8000;
            if (flags != 0)
                y |= 0x4000;

            Write((ushort)y);
            if (direction != 0)
                Write((byte)direction);
            Write((sbyte)item.Position.Z);
            if (hue != 0)
                Write((ushort)hue);
            if (flags != 0)
                Write((byte)flags);
        }
    }
    internal sealed class SAWorldItem : Packet
    {
        internal SAWorldItem(Item item)
            : base(0xF3)
        {
            this.EnsureCapacity(26);

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

            uint serial = (uint)item.Serial;
            ushort itemID = item.ItemID;
            ushort amount = item.Amount;
            int x = item.Position.X;
            int y = item.Position.Y;
            byte light = item.Light;
            ushort hue = item.Hue;
            byte flags = item.GetPacketFlags();
            byte direction = item.Direction;

            //if (amount != 0)
            //    serial |= 0x80000000;
            //else
            serial &= 0x7FFFFFFF;
            //Write((ushort)0x00); // ??
            //Write((ushort)0x01);


            byte artDataId = item.ArtID;
            //if ((0x4000 & itemID) == 0x4000)
            //    artDataId = 2;
            Write((byte)artDataId);

            Write((uint)serial);
            Write((ushort)(itemID & 0x7FFF));
            Write((byte)direction); // graph inc ?

            Write((ushort)amount);
            Write((ushort)0); // unknown

            x &= 0x7FFF;
            Write((ushort)x);

            y &= 0x3FFF;
            Write((ushort)y);

            Write((sbyte)item.Position.Z);

            Write((byte)light);

            Write((ushort)hue);
            Write((byte)flags);
            Write((ushort)0);
        }
    }

    internal sealed class EquipmentItem : Packet
    {
        internal EquipmentItem(Item item, Serial owner)
            : this(item, item.Hue, owner)
        {
        }

        internal EquipmentItem(Item item, ushort hue, Serial owner)
            : base(0x2E, 15)
        {
            Write(item.Serial);
            Write(item.ItemID);
            Write((byte)0x00);
            Write((byte)item.Layer);
            Write(owner);
            Write(hue);
        }
    }

    internal sealed class ForceWalk : Packet
    {
        internal ForceWalk(Direction d)
            : base(0x97, 2)
        {
            Write((byte)d);
        }
    }

    internal sealed class PathFindTo : Packet
    {
        internal PathFindTo(Point3D loc)
            : base(0x38, 7 * 20)
        {
            for (int i = 0; i < 20; i++)
            {
                if (i != 0)
                    Write((byte)0x38);
                Write((ushort)loc.X);
                Write((ushort)loc.Y);
                Write((short)loc.Z);
            }
        }
    }

    internal sealed class LoginConfirm : Packet
    {
        internal LoginConfirm(Mobile m)
            : base(0x1B, 37)
        {
            Write((int)m.Serial);
            Write((int)0);
            Write((short)m.Body);
            Write((short)m.Position.X);
            Write((short)m.Position.Y);
            Write((short)m.Position.Z);
            Write((byte)m.Direction);
            Write((byte)0);
            Write((int)-1);

            Write((short)0);
            Write((short)0);
            Write((short)6144);
            Write((short)4096);
        }
    }

    internal sealed class LoginComplete : Packet
    {
        internal LoginComplete()
            : base(0x55, 1)
        {
        }
    }

    internal sealed class DeathStatus : Packet
    {
        internal DeathStatus(bool dead)
            : base(0x2C, 2)
        {
            Write((byte)(dead ? 0 : 2));
        }
    }

    internal sealed class CurrentTime : Packet
    {
        internal CurrentTime()
            : base(0x5B, 4)
        {
            DateTime now = DateTime.Now;

            Write((byte)now.Hour);
            Write((byte)now.Minute);
            Write((byte)now.Second);
        }
    }

    internal sealed class MapChange : Packet
    {
        internal MapChange(byte map)
            : base(0xBF)
        {
            this.EnsureCapacity(6);

            Write((short)0x08);
            Write((byte)map);
        }
    }

    internal sealed class SeasonChange : Packet
    {
        internal SeasonChange(int season, bool playSound)
            : base(0xBC, 3)
        {
            Write((byte)season);
            Write((bool)playSound);
        }
    }

    internal sealed class SupportedFeatures : Packet
    {
        //private static int m_Value = 0x801F;
        internal SupportedFeatures(ushort val)
            : base(0xB9, 3)
        {
            Write((ushort)val); // 0x01 = T2A, 0x02 = LBR
        }
    }

    internal sealed class MapPatches : Packet
    {
        internal MapPatches(int[] patches)
            : base(0xBF)
        {
            EnsureCapacity(9 + (4 * patches.Length));

            Write((short)0x0018);

            Write((int)(patches.Length / 2));

            foreach (int t in patches)
                Write((int)t);

        }
    }

    internal sealed class MobileAttributes : Packet
    {
        internal MobileAttributes(PlayerData m)
            : base(0x2D, 17)
        {
            Write(m.Serial);

            Write((short)m.HitsMax);
            Write((short)m.Hits);

            Write((short)m.ManaMax);
            Write((short)m.Mana);

            Write((short)m.StamMax);
            Write((short)m.Stam);
        }
    }

    internal sealed class NewMobileAnimation : Packet
    {
        internal NewMobileAnimation(Mobile m, int action, int frameCount, int delay)
            : base(0xE2, 10)
        {
            Write((int)m.Serial);
            Write((short)action);
            Write((short)frameCount);
            Write((byte)delay);
        }
    }

    internal sealed class SetWarMode : Packet
    {
        internal SetWarMode(bool mode)
            : base(0x72, 5)
        {
            Write(mode);
            Write((byte)0x00);
            Write((byte)0x32);
            Write((byte)0x00);
            //Fill();
        }
    }

    internal sealed class OpenDoorMacro : Packet
    {
        internal OpenDoorMacro()
            : base(0x12)
        {
            EnsureCapacity(5);
            Write((byte)0x58);
            Write((byte)0);
        }
    }

    internal sealed class PersonalLightLevel : Packet
    {
        internal PersonalLightLevel(PlayerData m)
            : base(0x4E, 6)
        {
            Write((int)m.Serial);
            Write((sbyte)m.LocalLightLevel);
        }
    }

    internal sealed class GlobalLightLevel : Packet
    {
        internal GlobalLightLevel(int level)
            : base(0x4F, 2)
        {
            Write((sbyte)level);
        }
    }

    internal sealed class DisplayPaperdoll : Packet
    {
        internal DisplayPaperdoll(int serial)
            : base(0x06, 5)
        {
            Write((uint)serial | 0x80000000);
        }
    }

    internal sealed class RemoveObject : Packet
    {
        internal RemoveObject(UOEntity ent)
            : base(0x1D, 5)
        {
            Write((uint)ent.Serial);
        }

        internal RemoveObject(Serial s)
            : base(0x1D, 5)
        {
            Write((uint)s);
        }
    }
    internal sealed class CloseContainer : Packet
    {
        internal CloseContainer(Serial s)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4 + 4);
            Write((ushort)0x16);
            Write((uint)0x0C);
            Write((uint)s);
        }
    }

    internal sealed class ContextMenuRequest : Packet
    {
        internal ContextMenuRequest(Serial entity)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4);
            Write((ushort)0x13);
            Write((uint)entity);
        }
    }

    internal sealed class QueryProperties : Packet
    {
        internal QueryProperties(Serial entity)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4);
            Write((ushort)0x10);
            Write((uint)entity);
        }
    }

    internal sealed class MegaCliloc : Packet
    {
        internal MegaCliloc(List<Serial> entity)
            : base(0xD6)
        {
            EnsureCapacity(1 + 2 + (4 * entity.Count));
            foreach (var i in entity)
                Write((uint)i);
        }
    }

    internal sealed class ObjectProperties : Packet
    {
        internal ObjectProperties(Serial serial, ObjectPropertyList propertyList)
            : base(0xD6)
        {
            int numEntries = propertyList.Content.Count;
            EnsureCapacity(1 + 2 + 2 + 4 + 2 + 4 + (4 * numEntries));
            Write((UInt16)1); // packet id
            Write((UInt32)serial); // serial
            Write((UInt16)0);
            Write((Int32)0);
            foreach (var i in propertyList.Content)
            {
                Write((Int32)i.Number); // cliloc
                ushort len = (ushort)(i.Args.Length * 2);
                Write(len);
                if (i.Args.Length > 0)
                {
                    WriteLittleUniFixed(i.Args, i.Args.Length);
                }
            }
           Write((uint)0);
        }

    }

    internal sealed class ContextMenuResponse : Packet
    {
        internal ContextMenuResponse(Serial entity, ushort idx)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4 + 2);

            Write((ushort)0x15);
            Write((uint)entity);
            Write((ushort)idx);
        }
    }

    internal sealed class SetUpdateRange : Packet
    {
        internal SetUpdateRange(int range)
            : base(0xC8, 2)
        {
            Write((byte)range);
        }
    }

    internal sealed class RazorNegotiateResponse : Packet
    {
        internal RazorNegotiateResponse()
            : base(0xF0)
        {
            EnsureCapacity(1 + 2 + 1);

            Write((byte)0xFF);
        }
    }

    internal sealed class DesignStateGeneral : Packet
    {
        internal DesignStateGeneral(Item house)
            : base(0xBF)
        {
            EnsureCapacity(13);

            Write((ushort)0x1D);
            Write((uint)house.Serial);
            Write((int)house.HouseRevision);
        }
    }

    internal sealed class StringQueryResponse : Packet
    {
        internal StringQueryResponse(int serial, byte type, byte index, bool ok, string resp)
            : base(0xAC)
        {
            if (resp == null)
                resp = String.Empty;

            this.EnsureCapacity(1 + 2 + 4 + 1 + 1 + 1 + 2 + resp.Length + 1);

            Write((int)serial);
            Write((byte)type);
            Write((byte)index);
            Write((bool)ok);
            Write((short)(resp.Length + 1));
            WriteAsciiNull(resp);
        }
    }

    internal class DesignStateDetailed : Packet
    {
        internal const int MaxItemsPerStairBuffer = 750;

        private static readonly byte[] m_InflatedBuffer = new byte[0x2000];
        private static readonly byte[] m_DeflatedBuffer = new byte[0x2000];

        internal static void Clear(byte[] buffer, int size)
        {
            for (int i = 0; i < size; ++i)
                buffer[i] = 0;
        }
    }

    internal sealed class MobileUpdate : Packet
    {
        internal MobileUpdate(Mobile m) : base(0x20, 19)
        {
            Write((int)m.Serial);
            Write((short)m.Body);
            Write((byte)0);
            int ltHue = Engine.MainWindow.LTHilight;
            if (ltHue != 0 && Targeting.IsLastTarget(m))
                Write((short)(ltHue | 0x8000));
            else
                Write((short)m.Hue);
            Write((byte)m.GetPacketFlags());
            Write((short)m.Position.X);
            Write((short)m.Position.Y);
            Write((short)0);
            Write((byte)m.Direction);
            Write((sbyte)m.Position.Z);
        }
    }

    // Nuovi pacchetti Enhanced

    internal sealed class InvokeVirtue : Packet
    {
        internal InvokeVirtue(byte virtueID)
            : base(0x12)
        {
            EnsureCapacity(6);
            Write((byte)0xF4);
            WriteAsciiNull(virtueID.ToString());
            Write((byte)0x00);
        }
    }

    internal sealed class SendPartyMessage : Packet
    {
        internal SendPartyMessage(string message)
            : base(0xBF)
        {
            EnsureCapacity(2 + 2 + 2 + message.Length + 1);
            Write((ushort)0x06);    // Command
            Write((byte)0x04);      // Party command message to all
            WriteBigUniNull(message);
        }
    }

    internal sealed class SendPartyMessagePrivate : Packet
    {
        internal SendPartyMessagePrivate(int serial, string message)
            : base(0xBF)
        {
            EnsureCapacity(2 + 2 + 2 + message.Length + 1);
            Write((ushort)0x06);   // Command
            Write((byte)0x03);       // Party command message pricate
            Write((uint)serial);    // serial to send private message
            WriteBigUniNull(message);
        }
    }

    internal sealed class PartyCanLoot : Packet
    {
        internal PartyCanLoot(byte canloot)
            : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 2 + 2 + 1);
            Write((ushort)0x06);   // Command
            Write((byte)0x06);       // Party command
            Write((byte)canloot);
        }
    }

    internal sealed class PartyInvite : Packet
    {
        internal PartyInvite()
            : base(0xBF)
        {
            EnsureCapacity(6);
            Write((ushort)0x06);   // Command
            Write((byte)0x01);       // Party command open target for new member
        }
    }

    internal sealed class PartyRemoveMember : Packet
    {
        internal PartyRemoveMember(uint serial)
            : base(0xBF)
        {
            EnsureCapacity(2 + 2 + 2 + 4);
            Write((ushort)0x06);   // Command
            Write((byte)0x02);       // remove member
            Write((uint)serial);
        }
    }

    internal sealed class ToggleFly : Packet
    {
        internal ToggleFly()
            : base(0xBF)
        {
            EnsureCapacity(2 + 2 + 2 + 4);
            Write((ushort)0x32);   // Command
            Write((ushort)0x01);
            Write((int)0x00);
        }
    }

    internal sealed class QuestButton : Packet
    {
        internal QuestButton(uint serial)
            : base(0xD7)
        {
            EnsureCapacity(2 + 4 + 2 + 2);
            Write((uint)serial);
            Write((ushort)0x32);
            Write((byte)0x0A);
        }
    }

    internal sealed class GuildButton : Packet
    {
        internal GuildButton(uint serial)
            : base(0xD7)
        {
            EnsureCapacity(2 + 4 + 2 + 2);
            Write((uint)serial);
            Write((ushort)0x28);
            Write((byte)0x0A);
        }
    }

    internal sealed class Disconnect : Packet
    {
        internal Disconnect(bool toServer = false)
            : base(0xD1, 2)
        {
            var payload = toServer ? 0x00 : 0x01;
            Write((byte)payload);
        }
    }

    internal sealed class LogoffNotification : Packet
    {
        internal LogoffNotification()
            : base(0x01, 2)
        {
            Write(0xFFFFFFFF);
        }
    }

    internal sealed class ClosedStatusGump : Packet
    {
        internal ClosedStatusGump()
            : base(0xBF, 2)
        {
            Write(0x0009); //Packet Length
            Write(0x000c); //Subcommand 0x0c: Closed Status Gump
            Write(World.Player.Serial);
        }
    }


    internal sealed class PromptResponse : Packet
    {
        internal PromptResponse(uint serial, uint promptid, uint operation, string lang, string text)
            : base(0xC2)
        {
            if (text != "")
                EnsureCapacity(2 + 4 + 4 + 4 + 4 + (text.Length * 2));
            else
            {
                EnsureCapacity(18);
            }

            Write((uint)serial);
            Write((uint)promptid);
            Write((uint)operation);

            if (string.IsNullOrEmpty(lang))
                lang = "ENU";

            WriteAsciiFixed(lang.ToUpper(), 4);

            if (text != "")
                WriteLittleUniNull(text);
        }
    }

    internal sealed class RenameRequest : Packet
    {
        internal RenameRequest(uint serial, string name)
            : base(0x75, 65)
        {
            Write((uint)serial);
            WriteAsciiFixed(name, 30);
        }
    }

    internal sealed class EquipItemMacro : Packet
    {
        internal EquipItemMacro(List<uint> serials)
            : base(0xEC)
        {
            EnsureCapacity(2 + 2 + (serials.Count * 4));

            Write((byte)serials.Count);
            foreach (uint serial in serials)
                Write((uint)serial);
        }
    }

    internal sealed class UnEquipItemMacro : Packet
    {
        internal UnEquipItemMacro(List<ushort> layers)
            : base(0xED)
        {
            EnsureCapacity(2 + 2 + (layers.Count * 2));

            Write((byte)layers.Count);
            foreach (ushort layer in layers)
                Write((ushort)layer);
        }
    }


    internal sealed class ChatAction : Packet
    {
        internal ChatAction(ushort action, string lang, string text) // Channel message 0x61
            : base(0xB3)
        {
            if (string.IsNullOrEmpty(lang)) lang = "ENU";
            if (text == null) text = "";

            this.EnsureCapacity(2 + 4 + 2 + (text.Length * 2));

            WriteAsciiFixed(lang.ToUpper(), 4);
            Write(action);
            WriteBigUniNull(text);
        }
    }

}
