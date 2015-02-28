using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
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

		private static Timer m_UnflagTimer;

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
			if (Config.GetBool("ForceSpellHue"))
			{
				switch (Flag)
				{
					case SpellFlag.Beneficial:
						return Config.GetInt("BeneficialSpellHue");
					case SpellFlag.Harmful:
						return Config.GetInt("HarmfulSpellHue");
					case SpellFlag.Neutral:
						return Config.GetInt("NeutralSpellHue");
					default:
						return def;
				}
			}
			else
			{
				return def;
			}
		}

		internal HKSubCat GetSubCat()
		{
			return GetID() < 678 ? HKSubCat.SpellOffset + Circle : HKSubCat.MysticismC;
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

		private void Cast()
		{
			if (Config.GetBool("SpellUnequip") && ClientCommunication.AllowBit(FeatureBit.UnequipBeforeCast))
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

			foreach (Counter c in Counter.List)
				c.Flag = false;

			if (Config.GetBool("HighlightReagents"))
			{
				foreach (string reagent in Reagents)
				{
					foreach (Counter c in Counter.List)
					{
						if (c.Enabled && c.Format.ToLower() == reagent)
						{
							c.Flag = true;
							break;
						}
					}
				}

				if (m_UnflagTimer != null)
					m_UnflagTimer.Stop();
				else
					m_UnflagTimer = new UnflagTimer();
				m_UnflagTimer.Start();
			}

			ClientCommunication.RequestTitlebarUpdate();
			ClientCommunication.PostSpellCast(this.Number);

			if (World.Player != null)
			{
				World.Player.LastSpell = GetID();
				LastCastTime = DateTime.Now;
				Targeting.SpellTargetID = 0;
			}
		}

		internal static DateTime LastCastTime = DateTime.MinValue;

		private class UnflagTimer : Timer
		{
			internal UnflagTimer()
				: base(TimeSpan.FromSeconds(30.0))
			{
			}

			protected override void OnTick()
			{
				foreach (Counter c in Counter.List)
					c.Flag = false;

				ClientCommunication.RequestTitlebarUpdate();
			}
		}

		private static Dictionary<string, Spell> m_SpellsByPower;
		private static Dictionary<int, Spell> m_SpellsByID;
		private static HotKeyCallbackState HotKeyCallback;

		static Spell()
		{
			string filename = Path.Combine(Directory.GetCurrentDirectory(), "spells.def");
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

			HotKeyCallback = new HotKeyCallbackState(OnHotKey);
			foreach (Spell s in m_SpellsByID.Values)
				HotKey.Add(HKCategory.Spells, s.GetSubCat(), s.Name, HotKeyCallback, (ushort)s.GetID());
			HotKey.Add(HKCategory.Spells, LocString.HealOrCureSelf, new HotKeyCallback(HealOrCureSelf));
			HotKey.Add(HKCategory.Spells, LocString.MiniHealOrCureSelf, new HotKeyCallback(MiniHealOrCureSelf));
		}

		internal static void HealOrCureSelf()
		{
			Spell s = null;

			if (!ClientCommunication.AllowBit(FeatureBit.BlockHealPoisoned))
			{
				if (World.Player.Hits + 30 < World.Player.HitsMax && World.Player.Mana >= 12)
					s = Get(4, 5); // greater heal
				else
					s = Get(1, 4); // mini heal
			}
			else
			{
				if (World.Player.Poisoned && ClientCommunication.AllowBit(FeatureBit.BlockHealPoisoned))
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
			}

			if (s != null)
			{
				if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
					Targeting.TargetSelf(true);
				ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
				s.Cast();
			}
		}

		internal static void MiniHealOrCureSelf()
		{
			Spell s = null;

			if (!ClientCommunication.AllowBit(FeatureBit.BlockHealPoisoned))
			{
				s = Get(1, 4); // mini heal
			}
			else
			{
				if (World.Player.Poisoned)
					s = Get(2, 3); // cure
				else
					s = Get(1, 4); // mini heal
			}

			if (s != null)
			{
				if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
					Targeting.TargetSelf(true);
				ClientCommunication.SendToServer(new CastSpellFromMacro((ushort)s.GetID()));
				s.Cast();
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
				//if ( Macros.MacroManager.AcceptActions )
				//	Macros.MacroManager.Action( new Macros.MacroCastSpellAction( s ) );
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
			return m_SpellsByPower[power];
		}

		internal static Spell Get(int num)
		{
			return m_SpellsByID[num];
		}

		internal static Spell Get(int circle, int num)
		{
			return Get(Spell.ToID(circle, num));
		}
	}
}
