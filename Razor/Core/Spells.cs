using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Assistant.UI;

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
        internal Spell(char flag, int n, int c, string power)
        {
            Flag = (SpellFlag)flag;
            Number = n;
            Circle = c;
            WordsOfPower = power;
        }


        internal int Name
		{
			get
			{
                if (Circle <= 8) // Mage
                    return 3002011 + ((Circle - 1) * 8) + Number - 1;
                else if (Circle == 9) // Extended Mage Spells
                    return 2100 + Number - 1;
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
                    if (Number >= 1 && Number <= 6)
                        return 1115612 + Number - 1;
                    else if (Number >= 7 && Number <= 45)
                        return 1155896 + Number - 7;
                    else
                        return -1;
                }
                else if (Circle == 80) // Druid
                {
                    return 2120 + Number - 1;
                }
                else if (Circle == 90) // Cleric
                {
                    return 2140 + Number - 1;
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
						return Engine.MainWindow.BeneficialSpellHue;

					case SpellFlag.Harmful:
						return Engine.MainWindow.HarmfulSpellHue;

					case SpellFlag.Neutral:
						return Engine.MainWindow.NeutralSpellHue;

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
	 		Assistant.Client.Instance.SendToServer(p);
		}

		internal void OnCast(Packet p, bool wait)
		{
			Cast();
			if (wait)
		 		Assistant.Client.Instance.SendToServerWait(p);
			else
		 		Assistant.Client.Instance.SendToServer(p);
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

	 		Assistant.UOAssist.PostSpellCast(this.Number);

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
			//string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Definitions/spells.def");
			string filename = Path.Combine( Assistant.Engine.RootPath, "Definitions/spells.def");
			m_SpellsByPower = new Dictionary<string, Spell>(64 + 10 + 16);
			m_SpellsByID = new Dictionary<int, Spell>(64 + 10 + 16);

			if (!File.Exists(filename))
			{
				//Engine.MainWindow.SafeAction({ MessageBox.Show(Engine.ActiveWindow, Language.GetString(LocString.NoSpells), "Spells.def", MessageBoxButtons.OK, MessageBoxIcon.Warning); });
				MessageBox.Show(Language.GetString(LocString.NoSpells), "Spells.def", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
				 		Assistant.Client.Instance.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
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
			 		Assistant.Client.Instance.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
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
				 		Assistant.Client.Instance.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
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
			 		Assistant.Client.Instance.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
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
				 		Assistant.Client.Instance.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
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
			 		Assistant.Client.Instance.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
					s.Cast();
				}
			}

		}

		internal static void Initialize()
		{
			// no code, this is here to make sure out static ctor is init'd by the core
		}

        internal static int ToID(int circle, int num)
        {
            if (circle < 10)
                return ((circle - 1) * 8) + num;
            if (circle == 80)
                return 301 + num;
            if (circle == 90)
                return 341 + num;

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
