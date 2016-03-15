using Assistant;
namespace RazorEnhanced
{
	public class ScriptRecorder
	{
		private static bool m_onrecord = true;
		internal static bool OnRecord { get { return m_onrecord; } set { m_onrecord = value; } }

		private static void AddLog(string code)
		{
			RazorEnhanced.AutoLoot.AddLog(code);
		}

		internal static void Record_AttackRequest(uint serial)
		{
			AddLog("Player.Attack(0x" + serial.ToString("X8") + ")");
        }

		internal static void Record_ClientDoubleClick(Assistant.Serial ser)
		{
			int serint = ser;
			if (ser.IsItem)
				AddLog("Items.UseItem(0x" + serint.ToString("X8") + ")");
			else
				AddLog("Mobiles.UseMobile(0x" + serint.ToString("X8") + ")");
		}

		internal static void Record_DropRequest(Assistant.Serial i, Assistant.Serial dest)
		{
			int iint = i;
			int destint = dest;
			Assistant.Item item = Assistant.World.FindItem(i);
			if (item != null)
			{
				if (dest != 0xFFFFFFFF)
					AddLog("Items.Move(0x" + iint.ToString("X8") + ", 0x" + destint.ToString("X8") + ", " + item.Amount + ")");
				else
					AddLog("Items.DropItemGroundSelf(0x" + iint.ToString("X8") + ", " + item.Amount + ")");
			}
		}

		internal static void Record_ClientSingleClick(Assistant.Serial ser)
		{
			int serint = ser;
			if (ser.IsItem)
				AddLog("Items.SingleClick(0x" + serint.ToString("X8") + ")");
			else
				AddLog("Mobiles.SingleClick(0x" + serint.ToString("X8") + ")");
		}

		internal static void Record_ClientTextCommand(int type, int id)
		{
			if (type == 1) // Use Skill
			{
				switch (id)
				{
					case 2:
						AddLog("Player.UseSkill(\"Animal Lore\")");
						break;

					case 3:
						AddLog("Player.UseSkill(\"Item ID\")");
						break;

					case 4:
						AddLog("Player.UseSkill(\"Arms Lore\")");
						break;

					case 6:
						AddLog("Player.UseSkill(\"Begging\")");
						break;

					case 9:
						AddLog("Player.UseSkill(\"Peacemaking\")");
						break;

					case 12:
						AddLog("Player.UseSkill(\"Cartography\")");
						break;

					case 14:
						AddLog("Player.UseSkill(\"Detect Hidden\")");
						break;

					case 15:
						AddLog("Player.UseSkill(\"Discordance\")");
						break;

					case 16:
						AddLog("Player.UseSkill(\"Eval Int\")");
						break;

					case 19:
						AddLog("Player.UseSkill(\"Forensics\")");
						break;

					case 21:
						AddLog("Player.UseSkill(\"Hiding\")");
						break;

					case 22:
						AddLog("Player.UseSkill(\"Provocation\")");
						break;

					case 30:
						AddLog("Player.UseSkill(\"Poisoning\")");
						break;

					case 32:
						AddLog("Player.UseSkill(\"Spirit Speak\")");
						break;

					case 33:
						AddLog("Player.UseSkill(\"Stealing\")");
						break;

					case 35:
						AddLog("Player.UseSkill(\"Animal Taming\")");
						break;

					case 36:
						AddLog("Player.UseSkill(\"Taste ID\")");
						break;

					case 38:
						AddLog("Player.UseSkill(\"Tracking\")");
						break;

					case 46:
						AddLog("Player.UseSkill(\"Meditation\")");
						break;

					case 47:
						AddLog("Player.UseSkill(\"Stealth\")");
						break;

					case 48:
						AddLog("Player.UseSkill(\"Remove Trap\")");
						break;

					case 23:
						AddLog("Player.UseSkill(\"Inscribe\")");
						break;

					case 1:
						AddLog("Player.UseSkill(\"Anatomy\")");
						break;

					default:
						break;
				}

			}
			else  // Cast Spell
			{
				Spell s = Spell.Get(id);
				if (id >= 1 && id <= 64)
					AddLog("Spells.CastMagery(\""+ Language.GetString(s.Name) + "\")");
				else if (id >= 101 && id <= 117)
					AddLog("Spells.CastNecro(\"" + Language.GetString(s.Name) + "\")");
				else if (id >= 201 && id <= 201)
					AddLog("Spells.CastChivalry(\"" + Language.GetString(s.Name) + "\")");
				else if (id >= 401 && id <= 406)
					AddLog("Spells.CastBushido(\"" + Language.GetString(s.Name) + "\")");
				else if (id >= 501 && id <= 508)
					AddLog("Spells.CastNinjitsu(\"" + Language.GetString(s.Name) + "\")");
				else if (id >= 601 && id <= 616)
					AddLog("Spells.CastSpellweaving(\"" + Language.GetString(s.Name) + "\")");
				else if (id >= 678 && id <= 692)
					AddLog("Spells.CastMysticism(\"" + Language.GetString(s.Name) + "\")");
			}
		}

		internal static void Record_EquipRequest(Assistant.Item item, Assistant.Layer l, Assistant.Mobile m)
		{
			if (m == World.Player)
				AddLog("Player.EquipItem(0x"+ item.Serial.Value.ToString("X8") + ")");
			else
				AddLog("Player.UnEquipItemByLayer("+ l.ToString() + ")");
		}

		

	}
}