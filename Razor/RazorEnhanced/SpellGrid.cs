using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Assistant;
using System.Reflection;

namespace RazorEnhanced
{
	internal partial class PanelGrid : Panel
	{
		private bool m_abilityenabled = false;
		public bool AbilityEnabled
		{
			get { return m_abilityenabled; }
			set { m_abilityenabled = value; }
		}

		private int m_abilityID = 0;
		public int AbilityID
		{
			get { return m_abilityID; }
			set { m_abilityID = value; }
		}

		private string m_spell = "Empty";
		public string Spell
		{
			get { return m_spell; }
			set { m_spell = value; }
		}

		private SpellGrid.GroupType m_group = SpellGrid.GroupType.Empty;
		public SpellGrid.GroupType Group
		{
			get { return m_group; }
			set { m_group = value; }
		}

		private Color m_bordercolor = Color.Transparent;
		public Color BorderColor
		{
			get { return m_bordercolor; }
			set { m_bordercolor = value; }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (BorderColor != Color.Transparent)
				ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
			base.OnPaint(e);
		}
	}

	internal partial class SpellGridForm : Form
	{
		internal SpellGridForm()
		{
			FormClosed += new FormClosedEventHandler(SpellGrid.SpellGrid_close);
			Move += new System.EventHandler(SpellGrid.SpellGrid_Move);
			ContextMenu = SpellGrid.GeneraMenu();
			MouseDown += new System.Windows.Forms.MouseEventHandler(SpellGrid.SpellGrid_MouseDown);
			MouseMove += new System.Windows.Forms.MouseEventHandler(SpellGrid.SpellGrid_MouseMove);
			MouseUp += new System.Windows.Forms.MouseEventHandler(SpellGrid.SpellGrid_MouseUp);
			MouseClick += new System.Windows.Forms.MouseEventHandler(SpellGrid.SpellGrid_MouseClick);
			MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SpellGrid.SpellGrid_MouseClick);
			ShowInTaskbar = false;
			TopMost = true;
			FormBorderStyle = FormBorderStyle.None;
			MinimumSize = new Size(1, 1);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			TransparencyKey = Color.YellowGreen;
			BackColor = Color.YellowGreen;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			MaximizeBox = false;
			CausesValidation = false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!SpellGrid.Lock)
				ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
			base.OnPaint(e);
		}
	}

	internal class SpellGrid
	{
		[Serializable]
		public class SpellGridItem
		{
			private string m_Group;
			public string Group { get { return m_Group; } }

			private string m_Spell;
			public string Spell { get { return m_Spell; } }

			private Color m_Color;
			public Color Color { get { return m_Color; } }

			private Color m_Border;
			internal Color Border { get { return m_Border; } }

			public SpellGridItem(string group, string spell, Color color, Color border)
			{
				m_Group = group;
				m_Spell = spell;
				m_Color = color;
				m_Border = border;
			}
		}

		internal enum GroupType : int
		{
			Empty,
			Magery,
			Abilities,
			Bushido,
			Chivalry,
			Necromancy,
			Ninjitsu,
			Mysticism,
			Spellweaving,
			Mastery,
            Cleric,
            Druid,
			Script,
			Skills
		}

		private static Form m_form;
		private static bool m_lock = false;
		internal static bool Lock
		{
			get	{return m_lock;} set {m_lock = value;}
		}

		private static int m_vslot = 0;
		internal static int VSlot
		{
			get { return m_vslot; }
			set { m_vslot = value; }
		}

		private static int m_hslot = 0;
		internal static int HSlot
		{
			get { return m_hslot; }
			set { m_hslot = value; }
		}
		internal static Form SpellGridForm
		{
			get	{return m_form;} set {m_form = value;}
		}

		internal static void SpellGrid_close(object sender, EventArgs e)
		{
			m_form = null;
			m_hslot = m_vslot = 0;
        }

		internal static void SpellGrid_Move(object sender, System.EventArgs e)
		{
			System.Drawing.Point pt = m_form.Location;
			if (m_form.WindowState != FormWindowState.Minimized)
			{
				Assistant.Engine.MainWindow.GridLocationLabel.Text = "X: " + pt.X + " - Y:" + pt.Y;
				Assistant.Engine.GridX = pt.X;
				Assistant.Engine.GridY = pt.Y;
			}
		}

		internal static void Close()
		{
			if (m_form == null)
				return;

			Engine.GridX = m_form.Location.X;
			Engine.GridY = m_form.Location.Y;
			m_form.Close();
			m_form = null;
			m_hslot = m_vslot = 0;
		}

		internal static void Open()
		{
			if (Assistant.World.Player == null)
				return;

			m_vslot = RazorEnhanced.Settings.General.ReadInt("GridVSlot");
			m_hslot = RazorEnhanced.Settings.General.ReadInt("GridHSlot");
			if (m_form == null)
				DrawSpellGrid();

			UpdatePanelImage();
			DLLImport.Win.ShowWindow(m_form.Handle, 8);
			m_form.Location = new System.Drawing.Point(Settings.General.ReadInt("PosXGrid"), Settings.General.ReadInt("PosYGrid"));
			m_form.Opacity =((double)RazorEnhanced.Settings.General.ReadInt("GridOpacity")) / 100.0;
			m_form.Refresh();
        }

		internal static void LockUnlock()
		{
			if (m_form == null)
				return;

			m_lock = !m_lock;
			m_form.ContextMenu = GeneraMenu();
			m_form.Refresh();
			Settings.General.WriteInt("PosXGrid", m_form.Location.X);
			Settings.General.WriteInt("PosYGrid", m_form.Location.Y);
		}

		internal static void UpdateBox()
		{
			List<SpellGridItem> items = Settings.SpellGrid.ReadItems();

			int i = 0;
			int oldindex = Engine.MainWindow.GridSlotComboBox.SelectedIndex;
			Engine.MainWindow.GridSlotComboBox.Items.Clear();
            foreach (SpellGridItem item in items)
			{
				Engine.MainWindow.GridSlotComboBox.Items.Add("Slot: " + i);
				if (i == (m_hslot * m_vslot) - 1)
					break;
				i++;
				if (i > 99)
					break;
			}

			Engine.MainWindow.GridSlotComboBox.SelectedIndex = oldindex < i ? oldindex : 0;

		}

		private static Bitmap ColorizeIcon(Bitmap icon)
		{
			if (icon == null)
				return null;

			Bitmap mImage = new Bitmap(icon.Width, icon.Height);
			float[][] coeff = new float[][] {
						new float[] { 0, 0, 0, 0, 0 },
						new float[] { 0, 1, 0, 0, 0 },
						new float[] { 0, 0, 1, 0, 0 },
						new float[] { 0, 0, 0, 1, 0 },
						new float[] { 1, 0, 0, 0, 1 }};


			ColorMatrix cm = new ColorMatrix(coeff);
			var ia = new ImageAttributes();
			ia.SetColorMatrix(new ColorMatrix(coeff));
			using (var gr = Graphics.FromImage(mImage))
			{
				gr.DrawImage(icon, new Rectangle(0, 0, mImage.Width, mImage.Height),
					0, 0, mImage.Width, mImage.Height, GraphicsUnit.Pixel, ia);
			}
			return mImage;
		}
		internal static void UpdateSkillHighLight(SkillIcon ID, bool enable)
		{
			foreach (PanelGrid p in m_panellist)
			{
				if (p.Group == GroupType.Mastery || p.Group == GroupType.Bushido || p.Group == GroupType.Ninjitsu)
				{
					if (Enum.TryParse<SkillIcon>(p.Spell.Replace(" ", ""), out SkillIcon l))
					{
						if (ID == l)
						{
							if (enable)
								p.BackgroundImage = ColorizeIcon((Bitmap)p.BackgroundImage);
							else
								p.BackgroundImage = Ultima.Gumps.GetGump(GetImageID(p.Group, p.Spell));
						}
					}
				}
			}
		}

		internal static void UpdateSAHighLight(int ID)
		{
			foreach (PanelGrid p in m_panellist)
			{
				if (p.Group == GroupType.Abilities)
				{
					if (ID == 0)
					{
						p.AbilityEnabled = false;
						if (p.Spell == "Primary")
							p.BackgroundImage = Ultima.Gumps.GetGump(SpecialMoves.GetPrimaryIcon(p.AbilityID));
						else
							p.BackgroundImage = Ultima.Gumps.GetGump(SpecialMoves.GetSecondaryIcon(p.AbilityID));
					}
					else
					{
						if (p.AbilityID == ID)
						{
							p.AbilityEnabled = true;
							p.BackgroundImage = ColorizeIcon((Bitmap)p.BackgroundImage);
						}
						else
						{
							if (p.AbilityEnabled)
							{
								if (p.Spell == "Primary")
									p.BackgroundImage = Ultima.Gumps.GetGump(SpecialMoves.GetPrimaryIcon(p.AbilityID));
								else
									p.BackgroundImage = Ultima.Gumps.GetGump(SpecialMoves.GetSecondaryIcon(p.AbilityID));
							}
						}

					}

				}
			}
		}

		internal static void UpdateSAIcon()
		{
			if (Assistant.World.Player == null || m_form == null)
				return;

			Assistant.Item wep = null;
			Assistant.Item right = World.Player.GetItemOnLayer(Layer.RightHand);
			Assistant.Item left = World.Player.GetItemOnLayer(Layer.LeftHand);

			if (right != null)
				wep = right;
			else
			{
				if (left != null)
					wep = left;
			}

			int primaryAbilityID = SpecialMoves.GetPrimaryAbility(wep);
			int secondaryAbilityID = SpecialMoves.GetSecondaryAbility(wep);

			foreach (PanelGrid p in m_panellist)
			{
				if (p.Group == GroupType.Abilities)
				{
					if (p.Spell == "Primary")
					{
						if (p.AbilityID != primaryAbilityID)
						{
							p.AbilityID = primaryAbilityID;
							p.BackgroundImage = Ultima.Gumps.GetGump(SpecialMoves.GetPrimaryIcon(primaryAbilityID));
						}
					}
					else if (p.Spell == "Secondary")
					{
						if (p.AbilityID != secondaryAbilityID)
						{
							p.AbilityID = secondaryAbilityID;
							p.BackgroundImage = Ultima.Gumps.GetGump(SpecialMoves.GetSecondaryIcon(secondaryAbilityID));
						}
					}
				}
			}
		}

		//////////////////////////////////////////////////////////////
		// Form Dragmove start
		//////////////////////////////////////////////////////////////

		private static bool m_mouseDown;
		private static Point m_lastLocation;

		internal static void SpellGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_lock)
				return;

			m_mouseDown = true;
			m_lastLocation = e.Location;
		}

		internal static void SpellGrid_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_lock)
				return;

			if (!m_mouseDown)
				return;

			m_form.Location = new Point(
				(m_form.Location.X - m_lastLocation.X) + e.X, (m_form.Location.Y - m_lastLocation.Y) + e.Y);

			m_form.Update();
		}

		internal static void SpellGrid_MouseUp(object sender, MouseEventArgs e)
		{
			if (!m_lock)
				m_mouseDown = false;
		}

		internal static void SpellGrid_MouseClick_Control(object sender, MouseEventArgs e)
		{
			PanelGrid pl = (PanelGrid)sender;
			switch (pl.Group)
			{
				case GroupType.Magery:
					Spells.CastOnlyMagery(pl.Spell, false);
					break;
				case GroupType.Abilities:
					if (pl.Spell == "Primary")
						SpecialMoves.SetPrimaryAbility(false);
					else
						SpecialMoves.SetSecondaryAbility(false);
					break;
				case GroupType.Bushido:
					Spells.CastOnlyBushido(pl.Spell, false);
					break;
				case GroupType.Chivalry:
					Spells.CastOnlyChivalry(pl.Spell, false);
					break;
				case GroupType.Necromancy:
					Spells.CastOnlyNecro(pl.Spell, false);
					break;
				case GroupType.Ninjitsu:
					Spells.CastOnlyNinjitsu(pl.Spell, false);
					break;
				case GroupType.Mysticism:
					Spells.CastOnlyMysticism(pl.Spell, false);
					break;
				case GroupType.Spellweaving:
					Spells.CastOnlySpellweaving(pl.Spell, false);
					break;
				case GroupType.Mastery:
					Spells.CastOnlyMastery(pl.Spell, false);
					break;
                case GroupType.Cleric:
                    Spells.CastOnlyMastery(pl.Spell, false);
                    break;
                case GroupType.Druid:
                    Spells.CastOnlyMastery(pl.Spell, false);
                    break;
                case GroupType.Script:
					Misc.ScriptRun(pl.Spell);
					break;
				case GroupType.Skills:
					Player.UseSkillOnly(pl.Spell, false);
					break;
				default:
					break;
			}

			DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
		}

		internal static void SpellGrid_MouseClick(object sender, MouseEventArgs e)
		{
			DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
		}

		internal static void InitEvent()
		{
			foreach (Control control in m_form.Controls)
			{
				control.MouseDown += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseDown);
				control.MouseMove += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseMove);
				control.MouseUp += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseUp);
				control.MouseClick += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseClick_Control);
				control.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseClick_Control);
			}
		}

		////////////////////////////////////////////////////
		/////////////// DRAW SPELLGRID START ///////////////
		////////////////////////////////////////////////////

		private static List<PanelGrid> m_panellist = new List<PanelGrid>();

		internal static void DrawSpellGrid()
		{
			m_panellist = new List<PanelGrid>();
			m_form = new SpellGridForm
			{
				ClientSize = new System.Drawing.Size(m_hslot * 44 + m_hslot * 3, m_vslot * 44 + m_vslot * 3)
			};

			int paneloffsetX = 1;
			int paneloffsetY = 1;
			for (int i = 0; i < m_vslot; i += 1)
			{
				for (int x = 0; x < m_hslot; x += 1)
				{
					// Aggiungo panel dinamici
					PanelGrid paneltemp = new PanelGrid
					{
						BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center,
						Location = new System.Drawing.Point(paneloffsetX, paneloffsetY),
						Margin = new System.Windows.Forms.Padding(0),
						Size = new System.Drawing.Size(44, 44),
						TabIndex = i,
						BackColor = Color.Transparent
					};


					m_panellist.Add(paneltemp);
					m_form.Controls.Add(paneltemp);
					paneloffsetX += 45;
				}
				paneloffsetX = 1;
				paneloffsetY += 45;
            }
			InitEvent();
		}

		private static int GetImageID(GroupType t, string s)
		{
			int imageid = 0;

			switch (t)
			{
				case GroupType.Magery:
					SpellIconMagery.TryGetValue(s, out imageid);
					break;
				case GroupType.Abilities:
					SpellIconAbilities.TryGetValue(s, out imageid);
					break;
				case GroupType.Mastery:
					SpellIconMastery.TryGetValue(s, out imageid);
					break;
				case GroupType.Bushido:
					SpellIconBushido.TryGetValue(s, out imageid);
					break;
				case GroupType.Chivalry:
					SpellIconChivalry.TryGetValue(s, out imageid);
					break;
				case GroupType.Necromancy:
					SpellIconNecromancy.TryGetValue(s, out imageid);
					break;
				case GroupType.Ninjitsu:
					SpellIconNinjitsu.TryGetValue(s, out imageid);
					break;
				case GroupType.Mysticism:
					SpellIconMysticism.TryGetValue(s, out imageid);
					break;
				case GroupType.Spellweaving:
					SpellIconSpellweaving.TryGetValue(s, out imageid);
					break;
				case GroupType.Script:
					imageid = -1;
					break;
				case GroupType.Skills:
					imageid = -2;
					break;
				default:
					imageid = 0;
					break;
			}

			return imageid;
		}

		internal static void UpdatePanelImage()
		{
			if (m_form == null)
				return;

			List<SpellGridItem> items = Settings.SpellGrid.ReadItems();

			for (int x = 0; x < m_vslot * m_hslot; x++)
			{
				if (x > (m_panellist.Count -1)|| x > (items.Count -1))
					return;

				int imageid = 0;

				GroupType g = GroupType.Empty;

				if (Enum.TryParse<GroupType>(items[x].Group, out g))
					imageid = GetImageID(g, items[x].Spell);

				m_panellist[x].BorderColor = items[x].Color;

				switch (imageid)
				{
					case 0:
						m_panellist[x].Enabled = false;
						break;

					case -1:  // Script
						if (items[x].Spell != string.Empty)
						{
							m_panellist[x].BackgroundImage = CreateBitmap(items[x].Spell.Substring(0, items[x].Spell.LastIndexOf(".")));
							m_panellist[x].Enabled = true;
						}
						break;

					case -2:  // Skill
						m_panellist[x].BackgroundImage = SkillsIcon[items[x].Spell];
						m_panellist[x].Enabled = true;
						break;

					default:
						m_panellist[x].BackgroundImage = Ultima.Gumps.GetGump(imageid);
						m_panellist[x].Enabled = true;
						break;
				}

				m_panellist[x].Spell = items[x].Spell;

				m_panellist[x].Group = g;
			}
		}

		internal static Bitmap CreateBitmap(string text)
		{
			Bitmap bmp = new Bitmap(44, 44);

			RectangleF rectf = new RectangleF(0, 0, 44, 44);

			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.Black);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.DrawString(text, new Font("Tahoma", 10), Brushes.Yellow, rectf);

			g.Flush();
			return bmp;
		}
		//////////////////////////////////////////////////////////////
		// Context Menu
		//////////////////////////////////////////////////////////////

		internal static ContextMenu GeneraMenu()
		{
			ContextMenu cm = new ContextMenu();
			MenuItem menuItem = new MenuItem
			{
				Text = "Close"
			};
			menuItem.Click += new System.EventHandler(menuItemClose_Click);
			cm.MenuItems.Add(menuItem);

			menuItem = new MenuItem();
			if (m_lock)
			{
				menuItem.Text = "UnLock";
				menuItem.Click += new System.EventHandler(menuItemUnLock_Click);
			}
			else
			{
				menuItem.Text = "Lock";
				menuItem.Click += new System.EventHandler(menuItemLock_Click);
			}
			cm.MenuItems.Add(menuItem);

			return cm;
		}

		private static void menuItemClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private static void menuItemUnLock_Click(object sender, System.EventArgs e)
		{
			LockUnlock();
			Settings.General.WriteBool("LockGridCheckBox", m_lock);
			Engine.MainWindow.GridLockCheckBox.Checked = m_lock;
		}

		private static void menuItemLock_Click(object sender, System.EventArgs e)
		{
			LockUnlock();
			Settings.General.WriteBool("LockGridCheckBox", m_lock);
			Engine.MainWindow.GridLockCheckBox.Checked = m_lock;
		}

		//////////////// Load settings ////////////////
		internal static void LoadSettings()
		{
			Engine.MainWindow.GridSlotComboBox.Items.Clear();
			Engine.MainWindow.GridLockCheckBox.Checked = m_lock = Settings.General.ReadBool("LockGridCheckBox");
			Engine.MainWindow.GridOpenLoginCheckBox.Checked = Settings.General.ReadBool("GridOpenLoginCheckBox");
			Engine.MainWindow.GridLocationLabel.Text = "X: " + Settings.General.ReadInt("PosXGrid") + " - Y:" + RazorEnhanced.Settings.General.ReadInt("PosYGrid");
			Engine.GridX = Settings.General.ReadInt("PosXGrid");
			Engine.GridY = Settings.General.ReadInt("PosYGrid");
			m_vslot = Settings.General.ReadInt("GridVSlot");
			m_hslot = Settings.General.ReadInt("GridHSlot");
			Engine.MainWindow.GridVSlotLabel.Text = m_vslot.ToString();
			Engine.MainWindow.GridHSlotLabel.Text = m_hslot.ToString();

			Engine.MainWindow.GridGroupComboBox.DataSource = m_group_list;

			// Color Picked Combobox
			Engine.MainWindow.GridBorderComboBox.Items.Clear();
			Type colorType = typeof(System.Drawing.Color);
			PropertyInfo[] propInfoList = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
			foreach (PropertyInfo c in propInfoList)
				Engine.MainWindow.GridBorderComboBox.Items.Add(c.Name);

			// Carico Item
			List<SpellGridItem> items = Settings.SpellGrid.ReadItems();

			int i = 0;
			foreach (SpellGridItem item in items)
			{
				Engine.MainWindow.GridSlotComboBox.Items.Add("Slot " + i);
				if (i == (m_hslot * m_vslot) -1 )
					break;
				i++;
				if (i > 99)
					break;
            }
			Engine.MainWindow.GridSlotComboBox.SelectedIndex = 0;
		}

		//////////////////////////////////////////////////////////////
		// Dizionari
		//////////////////////////////////////////////////////////////

		internal static List<string> m_group_list = new List<string>
		{
			"Empty",
			"Abilities",
			"Bushido",
			"Chivalry",
			"Necromancy",
			"Ninjitsu",
			"Magery",
			"Mysticism",
			"Spellweaving",
			"Mastery",
			"Script",
			"Skills"
		};

		internal static Dictionary<string, int> SpellIconAbilities = new Dictionary<string, int>
		{
			{ "Primary", 0x5204 },
			{ "Secondary", 0x5206 }
		};

		internal static Dictionary<string, int> SpellIconMastery = new Dictionary<string, int>
		{
			{ "Inspire", 0x945},
			{ "Invigorate", 0x946},
			{ "Resilience", 0x947},
			{ "Perseverance", 0x948},
			{ "Tribulation", 0x949},
			{ "Despair", 0x946A},
			{ "Death Ray", 0x9B8B},
			{ "Ethereal Blast", 0x9B8C},
			{ "Nether Blast", 0x9B8D},
			{ "Mystic Weapon", 0x9B8E},
			{ "Command Undead", 0x9B8F},
			{ "Conduit", 0x9B90},
			{ "Mana Shield", 0x9B91},
			{ "Summon Reaper", 0x9B92},
			{ "Enchanted Summoning", 0x9B93},
			{ "Anticipate Hit", 0x9B94},
			{ "Warcry", 0x9B95},
			{ "Intuition", 0x9B96},
			{ "Rejuvenate", 0x9B97},
			{ "Holy Fist", 0x9B98},
			{ "Shadow", 0x9B99},
			{ "White Tiger Form", 0x9B9A},
			{ "Flaming Shot", 0x9B9B},
			{ "Playing The Odds", 0x9B9C},
			{ "Thrust", 0x9B9D},
			{ "Pierce", 0x9B9E},
			{ "Stagger", 0x9B9F},
			{ "Toughness", 0x9BA0},
			{ "Onslaught", 0x9BA1},
			{ "Focused Eye", 0x9BA2},
			{ "Elemental Fury", 0x9BA3},
			{ "Called Shot", 0x9BA4},
			{ "Saving Throw", 0x9BA5},
			{ "Shield Bash", 0x9BA6},
			{ "Bodyguard", 0x9BA7},
			{ "Heighten Senses", 0x9BA8},
			{ "Tolerance", 0x9BA9},
			{ "Injected Strike", 0x9BAA},
			{ "Potency", 0x9BAB},
			{ "Rampage", 0x9BAC},
			{ "Fists Of Fury", 0x9BAD},
			{ "Knockout", 0x9BAE},
			{ "Whispering", 0x9BAF},
			{ "Combat Training", 0x9BB0},
			{ "Boarding", 0x9BB1}
		};

		internal static Dictionary<string, int> SpellIconMysticism = new Dictionary<string, int>
		{
			{ "Nether Bolt", 0x5DC0},
			{ "Healing Stone", 0x5DC1},
			{ "Purge Magic", 0x5DC2},
			{ "Enchant", 0x5DC3},
			{ "Sleep", 0x5DC4},
			{ "Eagle Strike", 0x5DC5},
			{ "Animated Weapon", 0x5DC6},
			{ "Stone Form", 0x5DC7},
			{ "Spell Trigger", 0x5DC8},
			{ "Mass Sleep", 0x5DC9},
			{ "Cleansing Winds", 0x5DCA},
			{ "Bombard", 0x5DCB},
			{ "Spell Plague", 0x5DCC},
			{ "Hail Storm", 0x5DCD},
			{ "Nether Cyclone", 0x5DCE},
			{ "Rising Colossus", 0x5DCF},
		};

		internal static Dictionary<string, int> SpellIconSpellweaving = new Dictionary<string, int>
		{
			{ "Arcane Circle", 0x59D8},
			{ "Gift Of Renewal", 0x59D9},
			{ "Immolating Weapon", 0x59DA},
			{ "Attunement", 0x59DB},
			{ "Thunderstorm", 0x59DC},
			{ "Natures Fury", 0x59DD},
			{ "Summon Fey", 0x59DE},
			{ "Summon Fiend", 0x59DF},
			{ "Reaper Form", 0x59E0},
			{ "Wildfire", 0x59E1},
			{ "Essence Of Wind", 0x59E2},
			{ "Dryad Allure", 0x59E3},
			{ "Ethereal Voyage", 0x59E4},
			{ "Word Of Death", 0x59E5},
			{ "Gift Of Life", 0x59E6},
			{ "Arcane Empowerment", 0x59E7}
		};

		internal static Dictionary<string, int> SpellIconNinjitsu = new Dictionary<string, int>
		{
			{ "Focus Attack", 0x5320},
			{ "Death Strike", 0x5321},
			{ "Animal Form", 0x5322},
			{ "Ki Attack", 0x5323},
			{ "Surprise Attack", 0x5324},
			{ "Backstab", 0x5325},
			{ "Shadow jump", 0x5326},
			{ "Mirror Image", 0x5327}
		};

		internal static Dictionary<string, int> SpellIconBushido = new Dictionary<string, int>
		{
			{ "Honorable Execution", 0x5420},
			{ "Confidence", 0x5421},
			{ "Evasion", 0x5422},
			{ "Counter Attack", 0x5423},
			{ "Lightning Strike", 0x5424},
			{ "Momentum Strike", 0x5425}
		};

		internal static Dictionary<string, int> SpellIconChivalry = new Dictionary<string, int>
		{
			{ "Cleanse By Fire", 0x5100},
			{ "Close Wounds", 0x5101},
			{ "Consecrate Weapon", 0x5102},
			{ "Dispel Evil", 0x5103},
			{ "Divine Fury", 0x5104},
			{ "Enemy Of One", 0x5105},
			{ "Holy Light", 0x5106},
			{ "Noble Sacrifice", 0x5107},
			{ "Remove Curse", 0x5108},
			{ "Sacred Journey", 0x5109}
		};

		internal static Dictionary<string, int> SpellIconNecromancy = new Dictionary<string, int>
		{
			{ "Animate Dead", 0x5000},
			{ "Blood Oath", 0x5001},
			{ "Corpse Skin", 0x5002},
			{ "Curse Weapon", 0x503},
			{ "Evil Omen", 0x5004},
			{ "Horrific Beast", 0x5005},
			{ "Lich Form", 0x5006},
			{ "Mind Rot", 0x5007},
			{ "Pain Spike", 0x5008},
			{ "Poison Strike", 0x5009},
			{ "Strangle", 0x500A},
			{ "Summon Familiar", 0x500B},
			{ "Vampiric Embrace", 0x500C},
			{ "Vengeful Spirit", 0x500D},
			{ "Wither", 0x500E},
			{ "Wraith Form", 0x500F},
			{ "Exorcism", 0x5010}
		};

		internal static Dictionary<string, int> SpellIconMagery = new Dictionary<string, int>
		{
			{ "Clumsy", 0x8c0},
			{ "Create Food", 0x8c1},
			{ "Feeblemind", 0x8c2},
			{ "Heal", 0x8c3},
			{ "Magic Arrow", 0x8c4},
			{ "Night Sight", 0x8c5},
			{ "Reactive Armor", 0x8c6},
			{ "Weaken", 0x8c7},
			{ "Agility", 0x8c8},
			{ "Cunning", 0x8c9},
			{ "Cure", 0x8cA},
			{ "Harm", 0x8cB},
			{ "Magic Trap", 0x8cC},
			{ "Magic Untrap", 0x8cD},
			{ "Protection", 0x8cE},
			{ "Strength", 0x8cF},
			{ "Bless", 0x8D0},
			{ "Fireball", 0x8D1},
			{ "Magic Lock", 0x8D2},
			{ "Poison", 0x8D3},
			{ "Telekinesis", 0x8D4},
			{ "Teleport", 0x8D5},
			{ "Unlock", 0x8D6},
			{ "Wall of Stone", 0x8D7},
			{ "Arch Cure", 0x8D8},
			{ "Arch Protection", 0x8D9},
			{ "Curse", 0x8DA},
			{ "Fire Field", 0x8DB},
			{ "Greater Heal", 0x8DC},
			{ "Lightning", 0x8DD},
			{ "Mana Drain", 0x8DE},
			{ "Recall", 0x8DF},
			{ "Blade Spirits", 0x8E0},
			{ "Dispel Field", 0x8E1},
			{ "Incognito", 0x8E2},
			{ "Magic Reflection", 0x8E3},
			{ "Mind Blast", 0x8E4},
			{ "Paralyze", 0x8E5},
			{ "Poison Field", 0x8E6},
			{ "Summon Creature", 0x8E7},
			{ "Dispel", 0x8E8},
			{ "Energy Bolt", 0x8E9},
			{ "Explosion", 0x8EA},
			{ "Invisibility", 0x8EB},
			{ "Mark", 0x8EC},
			{ "Mass Curse", 0x8ED},
			{ "Paralyze Field", 0x8EE},
			{ "Reveal", 0x8EF},
			{ "Chain Lightning", 0x8F0},
			{ "Energy Field", 0x8F1},
			{ "Flamestrike", 0x8F2},
			{ "Gate Travel", 0x8F3},
			{ "Mana Vampire", 0x8F4},
			{ "Mass Dispel", 0x8F5},
			{ "Meteor Swarm", 0x8F6},
			{ "Polymorph", 0x8F7},
			{ "Earthquake", 0x8F8},
			{ "Energy Vortex", 0x8F9},
			{ "Resurrection", 0x8FA},
            { "Air Elemental", 0x8FB},
            { "Summon Daemon", 0x8FC},
            { "Earth Elemental", 0x8FD},
            { "Fire Elemental", 0x8FE},
            { "Water Elemental", 0x8FF}
		};

		internal static Dictionary<string, Bitmap> SkillsIcon = new Dictionary<string, Bitmap>
		{
			{ "Anatomy", Assistant.Properties.Resources.Skill_Icon_Anatomy},
			{ "Animal Lore", Assistant.Properties.Resources.Skill_Icon_AnimalLore},
			{ "Animal Taming", Assistant.Properties.Resources.Skill_Icon_AnimalTaming},
			{ "ArmsLore", Assistant.Properties.Resources.Skill_Icon_ArmsLore},
			{ "Begging", Assistant.Properties.Resources.Skill_Icon_Begging},
			{ "Cartography", Assistant.Properties.Resources.Skill_Icon_Cartography},
			{ "Detect Hidden", Assistant.Properties.Resources.Skill_Icon_DetectingHidden},
			{ "Discordance", Assistant.Properties.Resources.Skill_Icon_Discordance},
			{ "EvalInt", Assistant.Properties.Resources.Skill_Icon_EvalInt},
			{ "Forensics", Assistant.Properties.Resources.Skill_Icon_ForesicEvalutation},
			{ "Hiding", Assistant.Properties.Resources.Skill_Icon_Hiding},
			{ "Imbuing", Assistant.Properties.Resources.Skill_Icon_Imbuing},
			{ "Inscribe", Assistant.Properties.Resources.Skill_Icon_Inscription},
			{ "Item ID", Assistant.Properties.Resources.Skill_Icon_ItemID},
			{ "Meditation", Assistant.Properties.Resources.Skill_Icon_Meditation},
			{ "Peacemaking", Assistant.Properties.Resources.Skill_Icon_Peacemaking},
			{ "Poisoning", Assistant.Properties.Resources.Skill_Icon_Poisoning},
			{ "Provocation", Assistant.Properties.Resources.Skill_Icon_Provocation},
			{ "Remove Trap", Assistant.Properties.Resources.Skill_Icon_RemoveTrap},
			{ "Spirit Speak", Assistant.Properties.Resources.Skill_Icon_SpiritSpeak},
			{ "Stealing", Assistant.Properties.Resources.Skill_Icon_Stealing},
			{ "Stealth", Assistant.Properties.Resources.Skill_Icon_Stealth},
			{ "Taste ID", Assistant.Properties.Resources.Skill_Icon_TasteID},
			{ "Tracking", Assistant.Properties.Resources.Skill_Icon_Tracking}
		};
	}
}
