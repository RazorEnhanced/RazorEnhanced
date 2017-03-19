using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Assistant
{
	internal class Spell
	{
		internal enum SpellFlag
		{
			None = '?',
			Beneficial = 'B',
			Harmful = 'H',
			Neutral = 'N',
		}

		readonly internal SpellFlag Flag;
		readonly internal int Circle;
		readonly internal int Number;
		readonly internal string WordsOfPower;
		readonly internal string[] Reagents;

		internal Spell(char flag, int n, int c, string power, string[] reags)
		{
			Flag = (SpellFlag)flag;
			Number = n;
			Circle = c;
			WordsOfPower = power;
			Reagents = reags;
		}

		internal int Name
		{
			get
			{
				if (Circle <= 8) // Mage
					return 3002011 + ((Circle - 1) * 8) + Number - 1;
				else if (Circle == 10) // Necr
					return 1060509 + Number - 1;
				else if (Circle == 20) // Chiv
					return 1060585 + Number - 1;
				else if (Circle == 40) // Bush
					return 1060595 + Number - 1;
				else if (Circle == 50) // Ninj
					return 1060610 + Number - 1;
				else if (Circle == 60)
				{
					if (Number < 78) // Spellweaving
						return 1071026 + Number - 1;
					else // Mysticism
						return 1031678 + Number - 78;
				}
				else if (Circle == 70) // Mastery
				{
					switch (Number)
					{
						case 1:
							return 1115612;
						case 2:
							return 1115613;
						case 3:
							return 1115614;
						case 4:
							return 1115615;
						case 5:
							return 1115616;
						case 6:
							return 1115617;
						case 7:
							return 1155896;
						case 8:
							return 1155897;
						case 9:
							return 1155898;
						case 10:
							return 1155899;
						case 11:
							return 1155900;
						case 12:
							return 1155901;
						case 13:
							return 1155902;
						case 14:
							return 1155903;
						case 15:
							return 1155905;
						case 16:
							return 1155906;
						case 17:
							return 1155908;
						case 18:
							return 1155909;
						case 19:
							return 1155910;
						case 20:
							return 1155911;
						case 21:
							return 1155912;
						case 22:
							return 1155913;
						case 23:
							return 1155914;
						case 24:
							return 1155915;
						case 25:
							return 1155916;
						case 26:
							return 1155917;
						case 27:
							return 1155918;
						case 28:
							return 1155919;
						case 29:
							return 1155920;
						case 30:
							return 1155921;
						case 31:
							return 1155923;
						case 32:
							return 1155924;
						case 33:
							return 1155925;
						case 34:
							return 1155926;
						case 35:
							return 1155927;
						case 36:
							return 1155929;
						case 37:
							return 1155930;
						case 38:
							return 1155932;
						case 39:
							return 1155939;
					}
					return -1;
				}
                else
					return -1;
			}
		}

		public override string ToString()
		{
			return String.Format("{0} (#{1})", Language.GetString(this.Name), GetID());
		}

		internal int GetID()
		{
			return ToID(Circle, Number);
		}

		internal int GetHue(int def)
		{
			if (RazorEnhanced.Settings.General.ReadBool("ForceSpellHue"))
			{
				switch (Flag)
				{
					case SpellFlag.Beneficial:
						return RazorEnhanced.Settings.General.ReadInt("BeneficialSpellHue");

					case SpellFlag.Harmful:
						return RazorEnhanced.Settings.General.ReadInt("HarmfulSpellHue");

					case SpellFlag.Neutral:
						return RazorEnhanced.Settings.General.ReadInt("NeutralSpellHue");

					default:
						return def;
				}
			}
			else
			{
				return def;
			}
		}

		internal void OnCast(PacketReader p)
		{
			Cast();
			ClientCommunication.SendToServer(p);
		}

		internal void OnCast(Packet p)
		{
			Cast();
			ClientCommunication.SendToServer(p);
		}

		internal void OnCastByScript(Packet p)
		{
			Cast();
			ClientCommunication.SendToServerWait(p);
		}

		private void Cast()
		{
			if (RazorEnhanced.Settings.General.ReadBool("SpellUnequip"))
			{
				Item pack = World.Player.Backpack;
				if (pack != null)
				{
					// dont worry about uneqipping RuneBooks or SpellBooks
					Item item = World.Player.GetItemOnLayer(Layer.RightHand);
#if DEBUG
					if (item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA && !item.IsVirtueShield)
#else
					if ( item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA )
#endif
					{
						DragDropManager.Drag(item, item.Amount);
						DragDropManager.Drop(item, pack);
					}

					item = World.Player.GetItemOnLayer(Layer.LeftHand);
#if DEBUG
					if (item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA && !item.IsVirtueShield)
#else
					if ( item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA )
#endif
					{
						DragDropManager.Drag(item, item.Amount);
						DragDropManager.Drop(item, pack);
					}
				}
			}

			ClientCommunication.PostSpellCast(this.Number);

			if (World.Player != null)
			{
				World.Player.LastSpell = GetID();
				LastCastTime = DateTime.Now;
				Targeting.SpellTargetID = 0;
			}
		}

		internal static DateTime LastCastTime = DateTime.MinValue;

		private static Dictionary<string, Spell> m_SpellsByPower;
		private static Dictionary<int, Spell> m_SpellsByID;

		static Spell()
		{
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Definitions/spells.def");
			m_SpellsByPower = new Dictionary<string, Spell>(64 + 10 + 16);
			m_SpellsByID = new Dictionary<int, Spell>(64 + 10 + 16);

			if (!File.Exists(filename))
			{
				MessageBox.Show(Engine.ActiveWindow, Language.GetString(LocString.NoSpells), "Spells.def", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			using (StreamReader reader = new StreamReader(filename))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (line.Length <= 0 || line[0] == '#')
						continue;
					string[] split = line.Split('|');

					try
					{
						if (split.Length >= 5)
						{
							string[] reags = new string[split.Length - 5];
							for (int i = 5; i < split.Length; i++)
								reags[i - 5] = split[i].ToLower().Trim();
							Spell s = new Spell(split[0].Trim()[0], Convert.ToInt32(split[1].Trim()), Convert.ToInt32(split[2].Trim()), /*split[3].Trim(),*/ split[4].Trim(), reags);

							m_SpellsByID[s.GetID()] = s;

							if (s.WordsOfPower != null && s.WordsOfPower.Trim().Length > 0)
								m_SpellsByPower[s.WordsOfPower] = s;
						}
					}
					catch
					{
					}
				}
			}
		}

		internal static void HealOrCureSelf()
		{
			Spell s = null;

			if (World.Player.Poisoned)
			{
				s = Get(2, 3); // cure
			}
			else if (World.Player.Hits + 2 < World.Player.HitsMax)
			{
				if (World.Player.Hits + 30 < World.Player.HitsMax && World.Player.Mana >= 12)
					s = Get(4, 5); // greater heal
				else
					s = Get(1, 4); // mini heal
			}
			else
			{
				if (World.Player.Mana >= 12)
					s = Get(4, 5); // greater heal
				else
					s = Get(1, 4); // mini heal
			}

			if (RazorEnhanced.Settings.General.ReadBool("BlockBigHealCheckBox"))
			{
				if (World.Player.Hits < World.Player.HitsMax || World.Player.Poisoned)
				{
					if (s != null)
					{
						if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
							Targeting.TargetSelf(true);
						ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
						s.Cast();
					}
				}
			}
			else
			{
				if (s != null)
				{
					if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
						Targeting.TargetSelf(true);
					ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
					s.Cast();
				}
			}
		}

		internal static void MiniHealOrCureSelf()
		{
			Spell s = null;

			s = World.Player.Poisoned ? Get(2, 3) : Get(1, 4);

			if (RazorEnhanced.Settings.General.ReadBool("BlockMiniHealCheckBox"))
			{
				if (World.Player.Hits < World.Player.HitsMax || World.Player.Poisoned)
				{
					if (s != null)
					{
						if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
							Targeting.TargetSelf(true);
						ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
						s.Cast();
					}
				}
			}
			else
			{
				if (s != null)
				{
					if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
						Targeting.TargetSelf(true);
					ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
					s.Cast();
				}
			}

		}

		internal static void HealOrCureSelfChiva()
		{
			Spell s = null;

			s = Get(20, World.Player.Poisoned ? 1 : 2);

			if (RazorEnhanced.Settings.General.ReadBool("BlockChivalryHealCheckBox"))
			{
				if (World.Player.Hits < World.Player.HitsMax || World.Player.Poisoned)
				{
					if (s != null)
					{
						if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
							Targeting.TargetSelf(true);
						ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
						s.Cast();
					}
				}
			}
			else
			{
				if (s != null)
				{
					if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
						Targeting.TargetSelf(true);
					ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
					s.Cast();
				}
			}

		}

		internal static void Initialize()
		{
			// no code, this is here to make sure out static ctor is init'd by the core
		}

		internal static void OnHotKey(ref object state)
		{
			ushort id = (ushort)state;
			Spell s = Spell.Get(id);
			if (s != null)
			{
				s.OnCast(new CastSpellFromMacro(id));
			}
		}

		internal static int ToID(int circle, int num)
		{
			if (circle < 10)
				return ((circle - 1) * 8) + num;
			else
				return (circle * 10) + num;
		}

		internal static Spell Get(string power)
		{
			Spell spell = null;
			m_SpellsByPower.TryGetValue(power, out spell);
			return spell;
		}

		internal static Spell Get(int num)
		{
			Spell spell = null;
			m_SpellsByID.TryGetValue(num, out spell);
			return spell;
		}

		internal static Spell Get(int circle, int num)
		{
			return Get(Spell.ToID(circle, num));
		}
	}
}