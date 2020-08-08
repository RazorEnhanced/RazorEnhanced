using Assistant;
namespace RazorEnhanced
{
	public class ScriptRecorder
	{
		private static bool m_onrecord = false;
		internal static bool OnRecord { get { return m_onrecord; } set { m_onrecord = value; } }

		private static void AddLog(string code)
		{
			if (UI.EnhancedScriptEditor.EnhancedScriptEditorTextArea != null)
				UI.EnhancedScriptEditor.EnhancedScriptEditorTextArea.Text = UI.EnhancedScriptEditor.EnhancedScriptEditorTextArea.Text +"\n" + code;
        }

		internal static void Record_AttackRequest(uint serial)
		{
			AddLog("Player.Attack(0x" + serial.ToString("X8") + ")");
        }

		internal static void Record_ClientDoubleClick(Assistant.Serial ser)
		{
			if (ser.IsItem)
				AddLog("Items.UseItem(0x" + ser.Value.ToString("X8") + ")");
			else
				AddLog("Mobiles.UseMobile(0x" + ser.Value.ToString("X8") + ")");
		}

		internal static void Record_DropRequest(Assistant.Item i, Assistant.Serial dest)
		{
			if (dest != 0xFFFFFFFF)
				AddLog("Items.Move(0x" + i.Serial.Value.ToString("X8") + ", 0x" + dest.Value.ToString("X8") + ", " + i.Amount + ")");
			else
				AddLog("Items.DropItemGroundSelf(0x" + i.Serial.Value.ToString("X8") + ", " + i.Amount + ")");	
		}

		/*internal static void Record_ClientSingleClick(Assistant.Serial ser)
		{
			int serint = ser;
			if (ser.IsItem)
				AddLog("Items.SingleClick(0x" + serint.ToString("X8") + ")");
			else
				AddLog("Mobiles.SingleClick(0x" + serint.ToString("X8") + ")");
		}
		*/
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
			else if (type == 2) // Cast Spell
			{
				Spell s = Spell.Get(id);
				if (id >= 1 && id <= 64)
					AddLog("Spells.CastMagery(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 101 && id <= 117)
					AddLog("Spells.CastNecro(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 201 && id <= 210)
					AddLog("Spells.CastChivalry(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 401 && id <= 406)
					AddLog("Spells.CastBushido(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 501 && id <= 508)
					AddLog("Spells.CastNinjitsu(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 601 && id <= 616)
					AddLog("Spells.CastSpellweaving(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 678 && id <= 693)
					AddLog("Spells.CastMysticism(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				else if (id >= 701 && id <= 745)
				{
					if (id == 732)
						AddLog("Spells.CastMastery(\"Called Shot\")");
					else if (id == 715)
						AddLog("Spells.CastMastery(\"Enchanted Summoning\")");
					else
						AddLog("Spells.CastMastery(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
				}
				else
					AddLog("ERROR Spell not listed " + id);
			}
			else // InvokeVirtue
			{
				switch (id)
				{
					case 1:
						AddLog("Player.InvokeVirtue(\"Honor\")");
						break;
					case 2:
						AddLog("Player.InvokeVirtue(\"Sacrifice\")");
						break;
					case 3:
						AddLog("Player.InvokeVirtue(\"Valor\")");
						break;
					case 4:
						AddLog("Player.InvokeVirtue(\"Compassion\")");
						break;
					case 5:
						AddLog("Player.InvokeVirtue(\"Honesty\")");
						break;
					case 6:
						AddLog("Player.InvokeVirtue(\"Humility\")");
						break;
					case 7:
						AddLog("Player.InvokeVirtue(\"Justice\")");
						break;
					case 8:
						AddLog("Player.InvokeVirtue(\"Spirituality\")");
						break;
				}
			}
        }

		internal static void Record_EquipRequest(Assistant.Item item, Assistant.Layer l, Assistant.Mobile m)
		{
			if (m == World.Player)
				AddLog("Player.EquipItem(0x"+ item.Serial.Value.ToString("X8") + ")");
			else
				AddLog("Player.UnEquipItemByLayer("+ l.ToString() + ")");
		}

		internal static void Record_RenameMobile(int serial, string name)
		{
			AddLog("Misc.PerRename(0x" + serial.ToString("X8") + ", " + name + " )");
		}

		internal static void Record_AsciiPromptResponse(uint type, string text)
		{
			AddLog("Misc.WaitForPrompt(10000)");
			if (type == 0)
				AddLog("Misc.WaitForPrompt(10000)");
			else
				AddLog("Misc.ResponsePrompt(\"" + text + "\")");
		}

		internal static void Record_UnicodeSpeech(MessageType type, string text, int hue)
		{
			switch (type)
			{
				case MessageType.Guild:
					AddLog("Player.ChatGuild(\"" + text + "\")");
					break;
				case MessageType.Alliance:
					AddLog("Player.ChatAlliance(\"" + text + "\")");
					break;
				case MessageType.Emote:
					AddLog("Player.ChatEmote(" + hue + ", \"" + text + "\")");
					break;
				case MessageType.Whisper:
					AddLog("Player.ChatWhisper(" + hue + ", \"" + text + "\")");
					break;
				case MessageType.Yell:
					AddLog("Player.ChatYell(" + hue + ", \"" + text + "\")");
					break;
				default:
					AddLog("Player.ChatSay(" + hue + ", \"" + text + "\")");
					break;
			}
		}

		internal static void Record_GumpsResponse(uint id, int operation)
		{
			AddLog("Gumps.WaitForGump("+id+", 10000)");
			AddLog("Gumps.SendAction(" + id + ", "+ operation + ")");
		}

		internal static void Record_SADisarm()
		{
			AddLog("Player.WeaponDisarmSA( )");
		}

		internal static void Record_SAStun()
		{
			AddLog("Player.WeaponStunSA( )");
		}

		internal static void Record_ContextMenuResponse(int serial, ushort idx)
		{
			AddLog("Misc.WaitForContext(0x" + serial.ToString("X8") + ", 10000)");
			AddLog("Misc.ContextReply(0x" + serial.ToString("X8") + ", " + idx + ")");
		}

		internal static void Record_ResponseStringQuery(byte yesno, string text)
		{
			AddLog("Misc.WaitForQueryString(10000)");
			if (yesno != 0)
				AddLog("Misc.QueryStringResponse(True, " + text + ")");
			else
				AddLog("Misc.QueryStringResponse(False, " + text + ")");
		}

		internal static void Record_MenuResponse(int index)
		{
			AddLog("Misc.WaitForMenu(10000)");
			string text = string.Empty;
            try
			{
				text = World.Player.MenuEntry[index-1].ModelText;
			}
			catch { }
			AddLog("Misc.MenuResponse(\"" + text+ "\")");
		}

		internal static void Record_Movement(Direction dir)
		{
			if ((dir & Direction.Running) == Direction.Running)
			{
				switch (World.Player.Direction & Direction.Mask)
				{
					case Direction.North: AddLog("Player.Run(\"North\")"); break;
					case Direction.South: AddLog("Player.Run(\"South\")"); break;
					case Direction.West: AddLog("Player.Run(\"West\")"); break;
					case Direction.East: AddLog("Player.Run(\"East\")"); break;
					case Direction.Right: AddLog("Player.Run(\"Right\")"); break;
					case Direction.Left: AddLog("Player.Run(\"Left\")"); break;
					case Direction.Down: AddLog("Player.Run(\"Down\")"); break;
					case Direction.Up: AddLog("Player.Run(\"Up\")"); break;
					default: break;
				}
			}
			else
			{
				switch (World.Player.Direction & Direction.Mask)
				{
					case Direction.North: AddLog("Player.Walk(\"North\")"); break;
					case Direction.South: AddLog("Player.Walk(\"South\")"); break;
					case Direction.West: AddLog("Player.Walk(\"West\")"); break;
					case Direction.East: AddLog("Player.Walk(\"East\")"); break;
					case Direction.Right: AddLog("Player.Walk(\"Right\")"); break;
					case Direction.Left: AddLog("Player.Walk(\"Left\")"); break;
					case Direction.Down: AddLog("Player.Walk(\"Down\")"); break;
					case Direction.Up: AddLog("Player.Walk(\"Up\")"); break;
					default: break;
				}
			}
		}
	

		internal static void Record_Target(TargetInfo info)
		{
			AddLog("Target.WaitForTarget(10000, False)");
			if (info.X == 0xFFFF && info.X == 0xFFFF && (info.Serial <= 0 || info.Serial >= 0x80000000))
			{
				AddLog("Target.Cancel( )");
				return;
			}

			if (info.Serial == 0)
			{
				if (info.Gfx == 0)
					AddLog("Target.TargetExecute(" + info.X + ", " + info.Y + " ," + info.Z + ")");
				else
					AddLog("Target.TargetExecute(" + info.X + ", " + info.Y + " ," + info.Z + " ," + info.Gfx + ")");
			}
			else
				AddLog("Target.TargetExecute(" + info.Serial + ")");

		}
	}
}