using System;
using System.Collections.Generic;
using System.Drawing;
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

		private string m_group = "Empty";
		public string Group
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

		private static bool m_dicloaded = false;
		internal static Dictionary<string, int> SpellIconAbilities = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconMagery = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconMastery = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconBushido = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconChivalry = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconNecromancy = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconNinjitsu = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconMysticism = new Dictionary<string, int>();
		internal static Dictionary<string, int> SpellIconSpellweaving = new Dictionary<string, int>();

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

			Settings.General.WriteInt("PosXGrid", m_form.Location.X);
			Settings.General.WriteInt("PosYGrid", m_form.Location.Y);
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
			ClientCommunication.ShowWindow(m_form.Handle, 8);
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

		internal static void UpdateSAHighLight(int ID)
		{
			foreach (PanelGrid p in m_panellist)
			{
				if (p.Group == "Abilities")
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
				if (p.Group == "Abilities")
				{
					if (p.Spell == "Primary")
					{
						if (p.AbilityID != primaryAbilityID)
						{
							p.AbilityID = primaryAbilityID;
						//	RazorEnhanced.AutoLoot.AddLog("ID" + primaryAbilityID.ToString());
						//	RazorEnhanced.AutoLoot.AddLog(SpecialMoves.GetPrimaryIcon(primaryAbilityID).ToString());
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
				case "Magery":
					RazorEnhanced.Spells.CastMagery(pl.Spell, true);
					break;
				case "Abilities":
					if (pl.Spell == "Primary")
						Assistant.SpecialMoves.SetPrimaryAbility();
					else
						Assistant.SpecialMoves.SetSecondaryAbility();
					break;
				case "Bushido":
					RazorEnhanced.Spells.CastBushido(pl.Spell, true);
					break;
				case "Chivalry":
					RazorEnhanced.Spells.CastChivalry(pl.Spell, true);
					break;
				case "Necromancy":
					RazorEnhanced.Spells.CastNecro(pl.Spell, true);
					break;
				case "Ninjitsu":
					RazorEnhanced.Spells.CastNinjitsu(pl.Spell, true);
					break;
				case "Mysticism":
					RazorEnhanced.Spells.CastMysticism(pl.Spell, true);
					break;
				case "Spellweaving":
					RazorEnhanced.Spells.CastSpellweaving(pl.Spell, true);
					break;
				case "Mastery":
					RazorEnhanced.Spells.CastMastery(pl.Spell, true);
					break;
				default:
					break;
			}

			ClientCommunication.SetForegroundWindow(ClientCommunication.FindUOWindow());
		}

		internal static void SpellGrid_MouseClick(object sender, MouseEventArgs e)
		{
            ClientCommunication.SetForegroundWindow(ClientCommunication.FindUOWindow());
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

		internal static void UpdatePanelImage()
		{
			if (m_form == null)
				return;

			List<SpellGridItem> items = Settings.SpellGrid.ReadItems();

			for (int x = 0; x < m_vslot * m_hslot; x++)
			{
				int imageid = 0;

				m_panellist[x].BorderColor = items[x].Color;

				switch (items[x].Group)
				{
					case "Magery":
						SpellIconMagery.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Abilities":
						SpellIconAbilities.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Mastery":
						SpellIconMastery.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Bushido":
						SpellIconBushido.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Chivalry":
						SpellIconChivalry.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Necromancy":
						SpellIconNecromancy.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Ninjitsu":
						SpellIconNinjitsu.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Mysticism":
						SpellIconMysticism.TryGetValue(items[x].Spell, out imageid);
						break;
					case "Spellweaving":
						SpellIconSpellweaving.TryGetValue(items[x].Spell, out imageid);
						break;
					default:
						imageid = 0;
						break;
				}

				if (imageid != 0)
				{
					Bitmap image = Ultima.Gumps.GetGump(imageid);
					m_panellist[x].BackgroundImage = image;
					m_panellist[x].Enabled = true;
					m_panellist[x].Spell = items[x].Spell;
				}
				else
					m_panellist[x].Enabled = false;

				m_panellist[x].Group = items[x].Group;
				
			}
		}

		//////////////////////////////////////////////////////////////
		// Context Menu
		//////////////////////////////////////////////////////////////

		internal static ContextMenu GeneraMenu()
		{
			ContextMenu cm = new ContextMenu();
			MenuItem menuItem = new MenuItem();
			menuItem.Text = "Close";
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

			Engine.MainWindow.GridGroupComboBox.Items.Clear();
			Engine.MainWindow.GridGroupComboBox.Items.Add("Empty");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Abilities");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Bushido");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Chivalry");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Necromancy");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Ninjitsu");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Magery");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Mysticism");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Spellweaving");
			Engine.MainWindow.GridGroupComboBox.Items.Add("Mastery");

			//////////////////////////////////////////////////////////////
			// Dizionari
			//////////////////////////////////////////////////////////////

			if (!m_dicloaded)
			{
				// Abilities
				SpellIconAbilities.Add("Primary", 0x5204);
				SpellIconAbilities.Add("Secondary", 0x5206);

				// Mastery
				SpellIconMastery.Add("Inspire", 0x945);
				SpellIconMastery.Add("Invigorate", 0x946);
				SpellIconMastery.Add("Resilience", 0x947);
				SpellIconMastery.Add("Perseverance", 0x948);
				SpellIconMastery.Add("Tribulation", 0x949);
				SpellIconMastery.Add("Despair", 0x946A);
				SpellIconMastery.Add("Death Ray", 0x9B8B);
				SpellIconMastery.Add("Ethereal Blast", 0x9B8C);
				SpellIconMastery.Add("Nether Blast", 0x9B8D);
				SpellIconMastery.Add("Mystic Weapon", 0x9B8E);
				SpellIconMastery.Add("Command Undead", 0x9B8F);
				SpellIconMastery.Add("Conduit", 0x9B90);
				SpellIconMastery.Add("Mana Shield", 0x9B91);
				SpellIconMastery.Add("Summon Reaper", 0x9B92);
				SpellIconMastery.Add("Enchanted Summoning", 0x9B93);
				SpellIconMastery.Add("Anticipate Hit", 0x9B94);
				SpellIconMastery.Add("Warcry", 0x9B95);
				SpellIconMastery.Add("Intuition", 0x9B96);
				SpellIconMastery.Add("Rejuvenate", 0x9B97);
				SpellIconMastery.Add("Holy Fist", 0x9B98);
				SpellIconMastery.Add("Shadow", 0x9B99);
				SpellIconMastery.Add("White Tiger Form", 0x9B9A);
				SpellIconMastery.Add("Flaming Shot", 0x9B9B);
				SpellIconMastery.Add("Playing The Odds", 0x9B9C);
				SpellIconMastery.Add("Thrust", 0x9B9D);
				SpellIconMastery.Add("Pierce", 0x9B9E);
				SpellIconMastery.Add("Stagger", 0x9B9F);
				SpellIconMastery.Add("Toughness", 0x9BA0);
				SpellIconMastery.Add("Onslaught", 0x9BA1);
				SpellIconMastery.Add("Focused Eye", 0x9BA2);
				SpellIconMastery.Add("Elemental Fury", 0x9BA3);
				SpellIconMastery.Add("Called Shot", 0x9BA4);
				SpellIconMastery.Add("Saving Throw", 0x9BA5);
				SpellIconMastery.Add("Shield Bash", 0x9BA6);
				SpellIconMastery.Add("Bodyguard", 0x9BA7);
				SpellIconMastery.Add("Heighten Senses", 0x9BA8);
				SpellIconMastery.Add("Tolerance", 0x9BA9);
				SpellIconMastery.Add("Injected Strike", 0x9BAA);
				SpellIconMastery.Add("Potency", 0x9BAB);
				SpellIconMastery.Add("Rampage", 0x9BAC);
				SpellIconMastery.Add("Fists Of Fury", 0x9BAD);
				SpellIconMastery.Add("Knockout", 0x9BAE);
				SpellIconMastery.Add("Whispering", 0x9BAF);
				SpellIconMastery.Add("Combat Training", 0x9BB0);
				SpellIconMastery.Add("Boarding", 0x9BB1);

				// Mysticism
				SpellIconMysticism.Add("Nether Bolt", 0x5DC0);
                SpellIconMysticism.Add("Healing Stone", 0x5DC1);
				SpellIconMysticism.Add("Purge Magic", 0x5DC2);
				SpellIconMysticism.Add("Enchant", 0x5DC3);
				SpellIconMysticism.Add("Sleep", 0x5DC4);
				SpellIconMysticism.Add("Eagle Strike", 0x5DC5);
				SpellIconMysticism.Add("Animated Weapon", 0x5DC6);
                SpellIconMysticism.Add("Stone Form", 0x5DC7);
				SpellIconMysticism.Add("Spell Trigger", 0x5DC8);
				SpellIconMysticism.Add("Mass Sleep", 0x5DC9);
				SpellIconMysticism.Add("Cleansing Winds", 0x5DCA);
				SpellIconMysticism.Add("Bombard", 0x5DCB);
				SpellIconMysticism.Add("Spell Plague", 0x5DCC);
				SpellIconMysticism.Add("Hail Storm", 0x5DCD);
				SpellIconMysticism.Add("Nether Cyclone", 0x5DCE);
				SpellIconMysticism.Add("Rising Colossus", 0x5DCF);

				// SpellWeaving
				SpellIconSpellweaving.Add("Arcane Circle", 0x59D8);
                SpellIconSpellweaving.Add("Gift Of Renewal", 0x59D9);
                SpellIconSpellweaving.Add("Immolating Weapon", 0x59DA);
				SpellIconSpellweaving.Add("Attune Weapon", 0x59DB);
				SpellIconSpellweaving.Add("Thunderstorm", 0x59DC);
				SpellIconSpellweaving.Add("Natures Fury", 0x59DD);
				SpellIconSpellweaving.Add("Summon Fey", 0x59DE);
				SpellIconSpellweaving.Add("Summon Fiend", 0x59DF);
				SpellIconSpellweaving.Add("Reaper Form", 0x59E0);
				SpellIconSpellweaving.Add("Wildfire", 0x59E1);
				SpellIconSpellweaving.Add("Essence Of Wind", 0x59E2);
                SpellIconSpellweaving.Add("Dryad Allure", 0x59E3);
				SpellIconSpellweaving.Add("Ethereal Voyage", 0x59E4);
				SpellIconSpellweaving.Add("Word Of Death", 0x59E5);
				SpellIconSpellweaving.Add("Gift Of Life", 0x59E6);
				SpellIconSpellweaving.Add("Arcane Empowerment", 0x59E7);

				// Ninjitsu
				SpellIconNinjitsu.Add("Focus Attack", 0x5320);
				SpellIconNinjitsu.Add("Death Strike", 0x5321);
				SpellIconNinjitsu.Add("Animal Form", 0x5322);
				SpellIconNinjitsu.Add("Ki Attack", 0x5323);
				SpellIconNinjitsu.Add("Surprise Attack", 0x5324);
				SpellIconNinjitsu.Add("Backstab", 0x5325);
				SpellIconNinjitsu.Add("Shadow jump", 0x5326);
				SpellIconNinjitsu.Add("Mirror Image", 0x5327);

				// Bushido
				SpellIconBushido.Add("Honorable Execution", 0x5420);
				SpellIconBushido.Add("Confidence", 0x5421);
				SpellIconBushido.Add("Evasion", 0x5422);
				SpellIconBushido.Add("Counter Attack", 0x5423);
				SpellIconBushido.Add("Lightning Strike", 0x5424);
				SpellIconBushido.Add("Momentum Strike", 0x5425);

				// Chivalry
				SpellIconChivalry.Add("Cleanse By Fire", 0x5100);
                SpellIconChivalry.Add("Close Wounds", 0x5101);
				SpellIconChivalry.Add("Consecrate Weapon", 0x5102);
				SpellIconChivalry.Add("Dispel Evil", 0x5103);
				SpellIconChivalry.Add("Divine Fury", 0x5104);
				SpellIconChivalry.Add("Enemy Of One", 0x5105);
				SpellIconChivalry.Add("Holy Light", 0x5106);
				SpellIconChivalry.Add("Noble Sacrifice", 0x5107);
				SpellIconChivalry.Add("Remove Curse", 0x5108);
				SpellIconChivalry.Add("Sacred Journey", 0x5109);

				// Necromancy
				SpellIconNecromancy.Add("Animate Dead", 0x5000);
				SpellIconNecromancy.Add("Blood Oath", 0x5001);
				SpellIconNecromancy.Add("Corpse Skin", 0x5002);
				SpellIconNecromancy.Add("Curse Weapon", 0x503);
				SpellIconNecromancy.Add("Evil Omen", 0x5004);
				SpellIconNecromancy.Add("Horrific Beast", 0x5005);
				SpellIconNecromancy.Add("Lich Form", 0x5006);
				SpellIconNecromancy.Add("Mind Rot", 0x5007);
				SpellIconNecromancy.Add("Pain Spike", 0x5008);
				SpellIconNecromancy.Add("Poison Strike", 0x5009);
				SpellIconNecromancy.Add("Strangle", 0x500A);
				SpellIconNecromancy.Add("Summon Familiar", 0x500B);
				SpellIconNecromancy.Add("Vampiric Embrace", 0x500C);
				SpellIconNecromancy.Add("Vengeful Spirit", 0x500D);
				SpellIconNecromancy.Add("Wither", 0x500E);
				SpellIconNecromancy.Add("Wraith Form", 0x500F);
				SpellIconNecromancy.Add("Exorcism", 0x5010);

				// Magery
				SpellIconMagery.Add("Clumsy", 0x8c0);
				SpellIconMagery.Add("Create Food", 0x8c1);
				SpellIconMagery.Add("Feeblemind", 0x8c2);
				SpellIconMagery.Add("Heal", 0x8c3);
				SpellIconMagery.Add("Magic Arrow", 0x8c4);
				SpellIconMagery.Add("Night Sight", 0x8c5);
				SpellIconMagery.Add("Reactive Armor", 0x8c6);
				SpellIconMagery.Add("Weaken", 0x8c7);
				SpellIconMagery.Add("Agility", 0x8c8);
				SpellIconMagery.Add("Cunning", 0x8c9);
				SpellIconMagery.Add("Cure", 0x8cA);
				SpellIconMagery.Add("Harm", 0x8cB);
				SpellIconMagery.Add("Magic Trap", 0x8cC);
				SpellIconMagery.Add("Magic Untrap", 0x8cD);
				SpellIconMagery.Add("Protection", 0x8cE);
				SpellIconMagery.Add("Strength", 0x8cF);
				SpellIconMagery.Add("Bless", 0x8D0);
				SpellIconMagery.Add("Fireball", 0x8D1);
				SpellIconMagery.Add("Magic Lock", 0x8D2);
				SpellIconMagery.Add("Poison", 0x8D3);
				SpellIconMagery.Add("Telekinesis", 0x8D4);
				SpellIconMagery.Add("Teleport", 0x8D5);
				SpellIconMagery.Add("Unlock", 0x8D6);
				SpellIconMagery.Add("Wall of Stone", 0x8D7);
				SpellIconMagery.Add("Arch Cure", 0x8D8);
				SpellIconMagery.Add("Arch Protection", 0x8D9);
				SpellIconMagery.Add("Curse", 0x8DA);
				SpellIconMagery.Add("Fire Field", 0x8DB);
				SpellIconMagery.Add("Greater Heal", 0x8DC);
				SpellIconMagery.Add("Lightning", 0x8DD);
				SpellIconMagery.Add("Mana Drain", 0x8DE);
				SpellIconMagery.Add("Recall", 0x8DF);
				SpellIconMagery.Add("Blade Spirits", 0x8E0);
				SpellIconMagery.Add("Dispel Field", 0x8E1);
				SpellIconMagery.Add("Incognito", 0x8E2);
				SpellIconMagery.Add("Magic Reflection", 0x8E3);
				SpellIconMagery.Add("Mind Blast", 0x8E4);
				SpellIconMagery.Add("Paralyze", 0x8E5);
				SpellIconMagery.Add("Poison Field", 0x8E6);
				SpellIconMagery.Add("Summon Creature", 0x8E7);
				SpellIconMagery.Add("Dispel", 0x8E8);
				SpellIconMagery.Add("Energy Bolt", 0x8E9);
				SpellIconMagery.Add("Explosion", 0x8EA);
				SpellIconMagery.Add("Invisibility", 0x8EB);
				SpellIconMagery.Add("Mark", 0x8EC);
				SpellIconMagery.Add("Mass Curse", 0x8ED);
				SpellIconMagery.Add("Paralyze Field", 0x8EE);
				SpellIconMagery.Add("Reveal", 0x8EF);
				SpellIconMagery.Add("Chain Lightning", 0x8F0);
				SpellIconMagery.Add("Energy Field", 0x8F1);
				SpellIconMagery.Add("Flamestrike", 0x8F2);
				SpellIconMagery.Add("Gate Travel", 0x8F3);
				SpellIconMagery.Add("Mana Vampire", 0x8F4);
				SpellIconMagery.Add("Mass Dispel", 0x8F5);
				SpellIconMagery.Add("Meteor Swarm", 0x8F6);
				SpellIconMagery.Add("Polymorph", 0x8F7);
				SpellIconMagery.Add("Earthquake", 0x8F8);
				SpellIconMagery.Add("Energy Vortex", 0x8F9);
				SpellIconMagery.Add("Resurrection", 0x8FA);
				SpellIconMagery.Add("Summon Air Elemental", 0x8FB);
				SpellIconMagery.Add("Summon Daemon", 0x8FC);
				SpellIconMagery.Add("Summon Earth Elemental", 0x8FD);
				SpellIconMagery.Add("Summon Fire Elemental", 0x8FE);
				SpellIconMagery.Add("Summon Water Elemental", 0x8FF);

				m_dicloaded = true;
            }

			// Color Picked Combobox
			Engine.MainWindow.GridBorderComboBox.Items.Clear();
			Type colorType = typeof(System.Drawing.Color);
			PropertyInfo[] propInfoList = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
			foreach (PropertyInfo c in propInfoList)
			{
				Engine.MainWindow.GridBorderComboBox.Items.Add(c.Name);
			}

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
	}
}