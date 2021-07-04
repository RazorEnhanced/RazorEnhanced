using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Assistant;

namespace RazorEnhanced
{
	internal partial class ToolBarForm : Form
	{
		internal ToolBarForm()
		{
			Name = "ToolBar";
			Text = "ToolBar";
			FormClosed += new FormClosedEventHandler(ToolBar.EnhancedToolbar_close);
			Move += new System.EventHandler(ToolBar.EnhancedToolbar_Move);
			ContextMenu = ToolBar.GeneraMenu();
			MouseDown += new System.Windows.Forms.MouseEventHandler(ToolBar.ToolbarForm_MouseDown);
			MouseMove += new System.Windows.Forms.MouseEventHandler(ToolBar.ToolbarForm_MouseMove);
			MouseUp += new System.Windows.Forms.MouseEventHandler(ToolBar.ToolbarForm_MouseUp);
			MouseClick += new System.Windows.Forms.MouseEventHandler(ToolBar.ToolbarForm_MouseClick);
			MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(ToolBar.ToolbarForm_MouseClick);
			ShowInTaskbar = false;
			TopMost = true;
			FormBorderStyle = FormBorderStyle.None;
			MinimumSize = new Size(1, 1);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = Color.FromArgb(187, 182, 137);
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			MaximizeBox = false;
			CausesValidation = false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!ToolBar.Lock)
				ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
			base.OnPaint(e);
		}
	}

	internal class ToolBar
	{
		private static int m_slot = 0;

		private static bool m_lock = false;
		internal static bool Lock
		{
			get { return m_lock; }
			set { m_lock = value; }
		}

		private static Form m_form;
		internal static Form ToolBarForm
		{
			get	{ return m_form; }
			set { m_form = value; }
		}

		// Piccola Orizzontale
		private static Label m_hitslabelSH = new Label();
		private static Label m_manalabelSH = new Label();
		private static Label m_staminalabelSH = new Label();
		private static Label m_weightlabelSH = new Label();
        private static Label m_tithelabelSH = new Label();

        private static Label m_followerlabelSH = new Label();

		// Piccola verticale
		private static Label m_strlabelSV = new Label();
		private static Label m_hitlabelSV = new Label();
		private static Label m_dexlabelSV = new Label();
		private static Label m_stamlabelSV = new Label();
		private static Label m_intlabelSV = new Label();
		private static Label m_manalabelSV = new Label();
		private static Label m_weightlabelSV = new Label();
        private static Label m_tithelabelSV = new Label();

        private static Label m_weightmaxlabelSV = new Label();
		private static Label m_followerlabelSV = new Label();

		// Grande orizzontale e verticale
		private static Label m_labelBarHitsBHV = new Label();
		private static Label m_labelTextHitsBHV = new Label();
		private static Label m_labelTextManaBHV = new Label();
		private static Label m_labelBarManaBHV = new Label();
		private static Label m_labelBarStaminaBHV = new Label();
		private static Label m_labelTextStaminaBHV = new Label();
		private static Label m_labelTextWeightBHV = new Label();
        private static Label m_labelTextTitheBHV = new Label();

        private static Label m_labelTextFollowerBHV = new Label();

		private static List<Panel> m_panellist = new List<Panel>();
		private static List<Label> m_panelcount = new List<Label>();

		[Serializable]
		public class ToolBarItem
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

			private bool m_Warning;
			internal bool Warning { get { return m_Warning; } }

			private int m_WarningLimit;
			public int WarningLimit { get { return m_WarningLimit; } }

			public ToolBarItem(string name, int graphics, int color, bool warning, int warninglimit)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
				m_Warning = warning;
				m_WarningLimit = warninglimit;
			}
		}

		internal static void UpdateHits(int maxhits, int hits)
		{
			if (m_form == null)
				return;

			if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
			{
				int percent = (int)(hits * 100 / (maxhits == 0 ? (ushort)1 : maxhits));

				m_labelTextHitsBHV.Text = "Hits: " + hits.ToString() + " / " + maxhits.ToString();
				m_labelBarHitsBHV.Size = Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical" ? new Size(percent, 10) : new Size(percent, 5);
				m_labelBarHitsBHV.BackColor = GetColor(percent);
			}
			else
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					m_strlabelSV.Text = "S: " + maxhits.ToString();
					m_hitlabelSV.Text = "H: " + hits.ToString();
				}
				else
				{
						m_hitslabelSH.Text = hits.ToString() + " / " + maxhits.ToString();
				}
			}
		}

		internal static void UpdateStam(int maxstam, int stam)
		{
			if (m_form == null)
				return;

			if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
			{
				int percent = (int)(stam * 100 / (maxstam == 0 ? (ushort)1 : maxstam));

				m_labelTextStaminaBHV.Text = "Stam: " + stam.ToString() + " / " + maxstam.ToString();
				m_labelBarStaminaBHV.Size = Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical" ? new Size(percent, 10) : new Size(percent, 5);
				m_labelBarStaminaBHV.BackColor = GetColor(percent);
			}
			else
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					m_dexlabelSV.Text = "D: " + maxstam.ToString();
					m_stamlabelSV.Text = "S: " + stam.ToString();
				}
				else
				{
						m_staminalabelSH.Text = stam.ToString() + " / " + maxstam.ToString();
				}
			}
		}

		internal static void UpdateMana(int maxmana, int mana)
		{
			if (m_form == null)
				return;

			if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
			{
				int percent = (int)(mana * 100 / (maxmana == 0 ? (ushort)1 : maxmana));

				m_labelTextManaBHV.Text = "Mana: " + mana.ToString() + " / " + maxmana.ToString();
				m_labelBarManaBHV.Size = Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical" ? new Size(percent, 10) : new Size(percent, 5);
				m_labelBarManaBHV.BackColor = GetColor(percent);
			}
			else
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					m_intlabelSV.Text = "I: " + maxmana.ToString();
					m_manalabelSV.Text = "M: " + mana.ToString();
				}
				else
				{
						m_manalabelSH.Text = mana.ToString() + " / " + maxmana.ToString();
				}
			}
		}

        internal static void UpdateTithe(int tithe)
        {
            if (m_form == null)
                return;

            if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
            {
                m_labelTextTitheBHV.Text = "Tithe: " + tithe.ToString();
            }
            else
            {
                if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
                {
                    m_tithelabelSV.Text = "T: " + tithe.ToString();
                }
                else
                {
                    m_tithelabelSH.Text = tithe.ToString();
                }
            }
        }

        internal static void UpdateWeight(int maxweight, int weight)
		{
			if (m_form == null)
				return;

			if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
			{
				m_labelTextWeightBHV.Text = "Weight: " + weight.ToString() + " / " + maxweight.ToString();
			}
			else
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					m_weightlabelSV.Text = "W: " + weight.ToString();
					m_weightmaxlabelSV.Text = "L: " + maxweight.ToString();
				}
				else
				{
					m_weightlabelSH.Text = weight.ToString() + " / " + maxweight.ToString();
				}
			}
		}

		internal static void UpdateFollower()
		{
			if (m_form == null)
				return;

			if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
			{
				m_labelTextFollowerBHV.Text = "Follower: " + World.Player.Followers.ToString() + " / " + World.Player.FollowersMax.ToString();
			}
			else
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					m_followerlabelSV.Text = "F: " + World.Player.Followers.ToString();
				}
				else
				{
					m_followerlabelSH.Text = World.Player.Followers.ToString() + " / " + World.Player.FollowersMax.ToString();
				}
			}
		}


		private static Color GetColor(int percent)
		{
			if (percent <= 10)
				return Color.DarkViolet;
			else if (percent > 10 && percent <= 30)
				return Color.DarkRed;
			else if (percent > 30 && percent <= 50)
				return Color.DarkOrange;
			else if (percent > 50 && percent <= 70)
				return Color.Goldenrod;
			else if (percent > 70 && percent <= 90)
				return Color.Gold;
			else
				return Color.ForestGreen;
		}

		internal static void Close()
		{
			if (m_form == null)
				return;

			Assistant.Engine.ToolBarX = m_form.Location.X;
			Assistant.Engine.ToolBarY = m_form.Location.Y;

			m_form.Close();
			m_form = null;
			m_slot = 0;
		}

		internal static void LockUnlock()
		{
			if (m_form == null)
				return;

			m_lock = !m_lock;
			m_form.ContextMenu = GeneraMenu();
			m_form.Refresh();
			Settings.General.WriteInt("PosXToolBar", m_form.Location.X);
			Settings.General.WriteInt("PosYToolBar", m_form.Location.Y);
		}

		internal static void Open()
		{
			if (Assistant.World.Player == null)
				return;

			if (Settings.General.ReadString("ToolBoxStyleComboBox") == "TitleBar")
			{
				TitleBar.UpdateTitleBar();
				return;
			}

			if (m_form == null)
				DrawToolBar();

			UpdateAll();
			UpdatePanelImage();
			UpdateCount();
			DLLImport.Win.ShowWindow(m_form.Handle, 8);
			m_form.Location = new Point(Settings.General.ReadInt("PosXToolBar"), Settings.General.ReadInt("PosYToolBar"));
			m_form.Opacity = ((double)Settings.General.ReadInt("ToolBarOpacity")) / 100.0; ;
		}

		internal static void UptateToolBarComboBox(int index, int slotlimit = 0)
		{
			if (slotlimit != 0)
				m_slot = slotlimit;

			List<RazorEnhanced.ToolBar.ToolBarItem> items = Settings.Toolbar.ReadItems();
			Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Clear();
			int i = 0;
			foreach (RazorEnhanced.ToolBar.ToolBarItem item in items)
			{
				Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Add("Slot " + i + ": " + item.Name);
				i++;
				if (i >= m_slot)
					break;
			}
			if (index > slotlimit && slotlimit != 0)
				Assistant.Engine.MainWindow.ToolBoxCountComboBox.SelectedIndex = slotlimit - 1;
			else
			{
				if (Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Count > index)
					Assistant.Engine.MainWindow.ToolBoxCountComboBox.SelectedIndex = index;
				else
					Assistant.Engine.MainWindow.ToolBoxCountComboBox.SelectedIndex = -1;
			}
		}

		internal static void UpdatePanelImage()
		{
			if (m_form == null)
				return;

			List<RazorEnhanced.ToolBar.ToolBarItem> items = Settings.Toolbar.ReadItems();

			for (int x = 0; x < m_slot; x++)
			{
				if (x > (m_panellist.Count - 1) || x > (items.Count - 1))
					return;

				if (items[x].Graphics != 0)
				{
					Bitmap m_itemimage = Ultima.Art.GetStatic(items[x].Graphics);

					if (m_itemimage == null) // Graph not exist
						continue;

					if (items[x].Color > 0)
					{
						int hue = items[x].Color;
						bool onlyHueGrayPixels = (hue & 0x8000) != 0;
						hue = (hue & 0x3FFF) - 1;
						Ultima.Hue m_hue = Ultima.Hues.GetHue(hue);
						m_hue.ApplyTo(m_itemimage, onlyHueGrayPixels);
					}
					m_itemimage = CropImage(m_itemimage);
					m_panellist[x].BackgroundImage = m_itemimage;

					if (Settings.General.ReadString("ToolBoxSizeComboBox") != "Big")
						m_panellist[x].BackgroundImageLayout = ImageLayout.None;

					m_panellist[x].Enabled = true;
					m_panelcount[x].Text = "0";
					m_panellist[x].BackColor = SystemColors.Control;
				}
				else
				{
					m_panellist[x].BackgroundImage = null;
					m_panellist[x].BackColor = Color.Transparent;
					m_panellist[x].Enabled = false;
					m_panelcount[x].Text = "";
				}
			}
		}

     	internal static void UpdateAll()
		{
			if (Assistant.World.Player != null)
			{
				UpdateHits(Assistant.World.Player.HitsMax, Assistant.World.Player.Hits);
				UpdateStam(Assistant.World.Player.StamMax, Assistant.World.Player.Stam);
				UpdateMana(Assistant.World.Player.ManaMax, Assistant.World.Player.Mana);
				UpdateWeight(Assistant.World.Player.MaxWeight, Assistant.World.Player.Weight);
                UpdateTithe(Assistant.World.Player.Tithe);

                UpdateFollower();
				UpdateCount();
			}
		}

		internal static void UpdateCount()
		{
			if (Assistant.World.Player == null)
				return;

			if (Engine.MainWindow.ToolBoxStyleComboBox.Text == "TitleBar")
			{
				TitleBar.UpdateTitleBar();
				return;
			}

			if (m_form == null)
				return;

			List<RazorEnhanced.ToolBar.ToolBarItem> items = Settings.Toolbar.ReadItems();

			for (int x = 0; x < m_slot; x++)
			{
				if (items[x].Graphics == 0)
					continue;

				int amount = Items.BackpackCount(items[x].Graphics, items[x].Color);
				Int32.TryParse(m_panelcount[x].Text, out int oldamount);
				m_panelcount[x].Text = amount.ToString();

				if (!items[x].Warning)
					continue;

				if (amount <= items[x].WarningLimit)
				{
					m_panellist[x].BackColor = Color.Orange;
					if (amount < oldamount)
					{
						RazorEnhanced.Misc.SendMessage("COUNTER WARNING: Item: " + items[x].Name + " under limit left: " + amount.ToString(), false);
					}
				}
				else
				{
					m_panellist[x].BackColor = SystemColors.Control;
				}
			}
		}

		//////////////// Load settings ////////////////
		internal static void LoadSettings()
		{
		//	Assistant.Engine.MainWindow.ToolBoxCountComboBox.Items.Clear();

			Assistant.Engine.MainWindow.ToolBoxSizeComboBox.Items.Clear();
			Assistant.Engine.MainWindow.ToolBoxSizeComboBox.Items.Add("Big");
			Assistant.Engine.MainWindow.ToolBoxSizeComboBox.Items.Add("Small");

			Assistant.Engine.MainWindow.ToolBoxStyleComboBox.Items.Clear();
			Assistant.Engine.MainWindow.ToolBoxStyleComboBox.Items.Add("TitleBar");
			Assistant.Engine.MainWindow.ToolBoxStyleComboBox.Items.Add("Horizontal");
			Assistant.Engine.MainWindow.ToolBoxStyleComboBox.Items.Add("Vertical");

			Assistant.Engine.MainWindow.LockToolBarCheckBox.Checked = m_lock = Settings.General.ReadBool("LockToolBarCheckBox");
			Assistant.Engine.MainWindow.AutoopenToolBarCheckBox.Checked = Settings.General.ReadBool("AutoopenToolBarCheckBox");
			Assistant.Engine.MainWindow.LocationToolBarLabel.Text = "X: " + Settings.General.ReadInt("PosXToolBar") + " - Y:" + Settings.General.ReadInt("PosYToolBar");
			Assistant.Engine.ToolBarX = Settings.General.ReadInt("PosXToolBar");
			Assistant.Engine.ToolBarY = Settings.General.ReadInt("PosYToolBar");

			Assistant.Engine.MainWindow.ToolBoxSizeComboBox.SelectedItem = Settings.General.ReadString("ToolBoxSizeComboBox");
			Assistant.Engine.MainWindow.ToolBoxStyleComboBox.SelectedItem = Settings.General.ReadString("ToolBoxStyleComboBox");
			Assistant.Engine.MainWindow.ToolBoxSlotsLabel.Text = Settings.General.ReadInt("ToolBoxSlotsTextBox").ToString();
			m_slot = Settings.General.ReadInt("ToolBoxSlotsTextBox");
			Assistant.Engine.MainWindow.ShowHitsToolBarCheckBox.Checked = Settings.General.ReadBool("ShowHitsToolBarCheckBox");
			Assistant.Engine.MainWindow.ShowStaminaToolBarCheckBox.Checked = Settings.General.ReadBool("ShowStaminaToolBarCheckBox");
			Assistant.Engine.MainWindow.ShowManaToolBarCheckBox.Checked = Settings.General.ReadBool("ShowManaToolBarCheckBox");
			Assistant.Engine.MainWindow.ShowWeightToolBarCheckBox.Checked = Settings.General.ReadBool("ShowWeightToolBarCheckBox");
			Assistant.Engine.MainWindow.ShowFollowerToolBarCheckBox.Checked = Settings.General.ReadBool("ShowFollowerToolBarCheckBox");
			List<RazorEnhanced.ToolBar.ToolBarItem> items = Settings.Toolbar.ReadItems();

			UptateToolBarComboBox(0);
		}


		////////////////////////////////////////////////////
		//////////////// DRAW TOOLBAR START ////////////////
		////////////////////////////////////////////////////

		internal static void DrawToolBar()
		{
			m_slot = Settings.General.ReadInt("ToolBoxSlotsTextBox");

            if (Settings.General.ReadString("ToolBoxSizeComboBox") == "Big")
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					DrawToolBarBV();
				}
				else
				{
					DrawToolBarBH();
				}
			}
			else
			{
				if (Settings.General.ReadString("ToolBoxStyleComboBox") == "Vertical")
				{
					DrawToolBarSV();
				}
				else
				{
					DrawToolBarSH();
				}
			}

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
			Settings.General.WriteBool("LockToolBarCheckBox", m_lock);
			Engine.MainWindow.LockToolBarCheckBox.Checked = m_lock;

		}
		private static void menuItemLock_Click(object sender, System.EventArgs e)
		{
			LockUnlock();
			Settings.General.WriteBool("LockToolBarCheckBox", m_lock);
			Engine.MainWindow.LockToolBarCheckBox.Checked = m_lock;
		}

		//////////////////////////////////////////////////////////////
		// Fine context menu
		//////////////////////////////////////////////////////////////

		//////////////////////////////////////////////////////////////
		// Form Dragmove start
		//////////////////////////////////////////////////////////////

		private static bool m_mouseDown;
		private static Point m_lastLocation;

		internal static void ToolbarForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_lock)
				return;

			m_mouseDown = true;
			m_lastLocation = e.Location;
		}

		internal static void ToolbarForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_lock)
				return;

			if (!m_mouseDown)
				return;

			ToolBarForm.Location = new Point(
				(ToolBarForm.Location.X - m_lastLocation.X) + e.X, (ToolBarForm.Location.Y - m_lastLocation.Y) + e.Y);

			ToolBarForm.Update();
		}

		internal static void ToolbarForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (!m_lock)
				m_mouseDown = false;
		}

		internal static void ToolbarForm_MouseClick(object sender, MouseEventArgs e)
		{
			DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
		}

		internal static void InitEvent()
		{
			foreach (Control control in m_form.Controls)
			{
				control.MouseDown += new System.Windows.Forms.MouseEventHandler(ToolbarForm_MouseDown);
				control.MouseMove += new System.Windows.Forms.MouseEventHandler(ToolbarForm_MouseMove);
				control.MouseUp += new System.Windows.Forms.MouseEventHandler(ToolbarForm_MouseUp);
				control.MouseClick += new System.Windows.Forms.MouseEventHandler(ToolbarForm_MouseClick);
				control.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(ToolBar.ToolbarForm_MouseClick);
			}
		}

		//////////////////////////////////////////////////////////////
		// Form DragMove fine
		//////////////////////////////////////////////////////////////

		//////////////////////////////////////////////////////////////
		// Inizio barre
		//////////////////////////////////////////////////////////////

		private static void DrawToolBarBV() // Grande Verticale
		{
			m_panellist = new List<Panel>();
			m_panelcount = new List<Label>();

			m_form = new ToolBarForm();
			m_form.SuspendLayout();

			// Sfondo e parametri offset
			Bitmap sfondotemporaneo = Assistant.Properties.Resources.BarraGrandeVerticaleBordoSopra;
			int height = Assistant.Properties.Resources.BarraGrandeVerticaleBordoSopra.Height + Assistant.Properties.Resources.BarraGrandeVerticaleBordoSotto.Height;
			int offsetstat = 9;
			int paneloffset = 18;
			height += (m_slot * 60) / 2; // Aggiungo spazio slot

			if (Settings.General.ReadBool("ShowHitsToolBarCheckBox"))
			{
				m_labelBarHitsBHV = new Label
				{
					BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
					Location = new Point(12, offsetstat + 16),
					Name = "labelBarHits",
					Size = new Size(100, 10),
					TabIndex = 0,
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_labelTextHitsBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextHits",
					Size = new Size(100, 16),
					TabIndex = 1,
					Text = "Hits: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelBarHitsBHV);
				m_form.Controls.Add(m_labelTextHitsBHV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			if (Settings.General.ReadBool("ShowManaToolBarCheckBox"))
			{
				m_labelBarManaBHV = new Label
				{
					BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
					Location = new Point(12, offsetstat + 16),
					Name = "labelBarMana",
					Size = new Size(100, 10),
					TabIndex = 2,
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_labelTextManaBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextMana",
					Size = new Size(100, 16),
					TabIndex = 3,
					Text = "Mana: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelBarManaBHV);
				m_form.Controls.Add(m_labelTextManaBHV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			if (Settings.General.ReadBool("ShowStaminaToolBarCheckBox"))
			{
				m_labelBarStaminaBHV = new Label
				{
					BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
					Location = new Point(12, offsetstat + 16),
					Name = "labelBarStamina",
					Size = new Size(100, 10),
					TabIndex = 4,
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_labelTextStaminaBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextStamina",
					Size = new Size(100, 16),
					TabIndex = 5,
					Text = "Stam: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelBarStaminaBHV);
				m_form.Controls.Add(m_labelTextStaminaBHV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			if (Settings.General.ReadBool("ShowWeightToolBarCheckBox"))
			{
				m_labelTextWeightBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat + 6),
					Name = "labelTextWeight",
					Size = new Size(100, 16),
					TabIndex = 5,
					Text = "Weight: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelTextWeightBHV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}
            //

            if (Settings.General.ReadBool("ShowTitheToolBarCheckBox"))
            {
                m_labelTextTitheBHV = new Label
                {
                    BackColor = System.Drawing.Color.Transparent,
                    Location = new Point(12, offsetstat + 6),
                    Name = "labelTextTithe",
                    Size = new Size(100, 16),
                    TabIndex = 5,
                    Text = "Tithe: 99999999",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };

                m_form.Controls.Add(m_labelTextTitheBHV);

                sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSpazioStat);

                height += 29;
                offsetstat += 30;
                paneloffset += 29;
            }

            //
            if (Settings.General.ReadBool("ShowFollowerToolBarCheckBox"))
			{
				m_labelTextFollowerBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat + 6),
					Name = "labelTextFollower",
					Size = new Size(100, 16),
					TabIndex = 5,
					Text = "Follower: 5/5",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelTextFollowerBHV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			for (int i = 0; i < m_slot; i += 2)
			{
				//Genero sfondo slot
				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleSlot);

				// Aggiungo panel dinamici
				Label labeltemp1 = new Label
				{
					AutoSize = true,
					Location = new Point(0, 29),
					Font =
						new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
							System.Drawing.GraphicsUnit.Point, ((byte) (0))),
					Name = "panel" + i + "count",
					Size = new Size(25, 13),
					TabIndex = 1,
					Text = "000"
				};

				Panel paneltemp1 = new Panel
				{
					BackColor = Color.Transparent,
					BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center,
					Location = new Point(11, paneloffset),
					Margin = new System.Windows.Forms.Padding(0),
					Name = "panel" + i + 1,
					Size = new Size(42, 42),
					TabIndex = 10,
				};

				Label labeltemp2 = new Label
				{
					AutoSize = true,
					Location = new Point(0, 29),
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Name = "panel" + i + "count",
					Size = new Size(25, 13),
					TabIndex = 1,
					Text = "000"
				};

				Panel paneltemp2 = new Panel
				{
					BackColor = Color.Transparent,
					BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center,
					Location = new Point(71, paneloffset),
					Margin = new System.Windows.Forms.Padding(0),
					Name = "panel" + i + 1,
					Size = new Size(42, 42),
					TabIndex = 10
				};

				m_panellist.Add(paneltemp1);
				paneltemp1.Controls.Add(labeltemp1);
				m_panelcount.Add(labeltemp1);

				paneltemp2.Controls.Add(labeltemp2);
				m_panellist.Add(paneltemp2);
				m_panelcount.Add(labeltemp2);

				m_form.Controls.Add(paneltemp1);
				m_form.Controls.Add(paneltemp2);

				paneloffset += 60;
			}

			m_form.ResumeLayout();
			m_form.ClientSize = new Size(Assistant.Properties.Resources.BarraGrandeVerticaleBordoSopra.Width, height);
			m_form.BackgroundImage = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraGrandeVerticaleBordoSotto);
			InitEvent();
		}

		private static void DrawToolBarBH() // Grande Orizzontale
		{
			m_panellist = new List<Panel>();
			m_panelcount = new List<Label>();

			m_form = new ToolBarForm();
			m_form.SuspendLayout();

			int width = Assistant.Properties.Resources.BarraGrandeOrizzontaBordoDestro.Width + Assistant.Properties.Resources.BigBarHorizontalSpaceStat.Width;
			int offsetstat = 10;
			int paneloffset = 18;


			// Generic Background
			Bitmap tempBackground = Assistant.Properties.Resources.BigBarHorizontalSpaceStat;

			if (Settings.General.ReadBool("ShowHitsToolBarCheckBox") || Settings.General.ReadBool("ShowStaminaToolBarCheckBox") || Settings.General.ReadBool("ShowManaToolBarCheckBox") ||
				Settings.General.ReadBool("ShowWeightToolBarCheckBox") || Settings.General.ReadBool("ShowFollowerToolBarCheckBox") || Settings.General.ReadBool("ShowTitheToolBarCheckBox"))
			{
				tempBackground = BackgroundAddHorizontal(tempBackground, Assistant.Properties.Resources.BarraGrandeOrizzontaleSpazioStat);
				width += 106;
				paneloffset += 106;
			}


			if (Settings.General.ReadBool("ShowHitsToolBarCheckBox"))
			{
				m_labelBarHitsBHV = new Label
				{
					BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
					Location = new Point(12, offsetstat + 16),
					Name = "labelBarHits",
					Size = new Size(100, 5),
					TabIndex = 0,
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_labelTextHitsBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextHits",
					Size = new Size(100, 16),
					TabIndex = 1,
					Text = "Hits: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelBarHitsBHV);
				m_form.Controls.Add(m_labelTextHitsBHV);

				offsetstat += 23;
			}

			if (Settings.General.ReadBool("ShowManaToolBarCheckBox"))
			{
				m_labelBarManaBHV = new Label
				{
					BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
					Location = new Point(12, offsetstat + 16),
					Name = "labelBarMana",
					Size = new Size(100, 5),
					TabIndex = 0,
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_labelTextManaBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextMana",
					Size = new Size(100, 16),
					TabIndex = 1,
					Text = "Mana: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelBarManaBHV);
				m_form.Controls.Add(m_labelTextManaBHV);

				offsetstat += 23;
			}

			if (Settings.General.ReadBool("ShowStaminaToolBarCheckBox"))
			{
				m_labelBarStaminaBHV = new Label
				{
					BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
					Location = new Point(12, offsetstat + 16),
					Name = "labelBarStamina",
					Size = new Size(100, 5),
					TabIndex = 0,
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_labelTextStaminaBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextStamina",
					Size = new Size(100, 16),
					TabIndex = 1,
					Text = "Stam: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelBarStaminaBHV);
				m_form.Controls.Add(m_labelTextStaminaBHV);

				offsetstat += 23;
			}

            if (Settings.General.ReadBool("ShowWeightToolBarCheckBox"))
			{
				m_labelTextWeightBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextStamina",
					Size = new Size(100, 16),
					TabIndex = 1,
					Text = "Weight: 150/150",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelTextWeightBHV);

				offsetstat += 18;
			}

            //
            if (Settings.General.ReadBool("ShowTitheToolBarCheckBox"))
            {
                m_labelTextTitheBHV = new Label
                {
                    BackColor = System.Drawing.Color.Transparent,
                    Location = new Point(12, offsetstat),
                    Name = "labelTextTithe",
                    Size = new Size(100, 16),
                    TabIndex = 1,
                    Text = "Tithe: 00000000",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };

                m_form.Controls.Add(m_labelTextTitheBHV);

                offsetstat += 18;
            }

            //


            if (Settings.General.ReadBool("ShowFollowerToolBarCheckBox"))
			{
				m_labelTextFollowerBHV = new Label
				{
					BackColor = System.Drawing.Color.Transparent,
					Location = new Point(12, offsetstat),
					Name = "labelTextFollower",
					Size = new Size(100, 16),
					TabIndex = 1,
					Text = "Follower: 5/5",
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter
				};

				m_form.Controls.Add(m_labelTextFollowerBHV);

				offsetstat += 18;
			}

			width += (m_slot * 60) / 2; // Aggiungo spazio slot

			m_form.ClientSize = new Size(width, Assistant.Properties.Resources.BarraGrandeOrizzontaBordoDestro.Height);

			for (int i = 0; i < m_slot; i += 2)
			{
				//Genero sfondo slot
				tempBackground = BackgroundAddHorizontal(tempBackground, Assistant.Properties.Resources.BarraGrandeOrizzontaleSlot);

				Label labeltemp1 = new Label
				{
					AutoSize = true,
					Location = new Point(0, 29),
					Font =
						new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
							System.Drawing.GraphicsUnit.Point, ((byte) (0))),
					Name = "panel" + i + "count",
					Size = new Size(25, 13),
					TabIndex = 1,
					Text = "000"
				};

				Panel paneltemp1 = new Panel
				{
					BackColor = Color.Transparent,
					BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center,
					Location = new Point(paneloffset, 11),
					Margin = new System.Windows.Forms.Padding(0),
					Name = "panel" + i + 1,
					Size = new Size(42, 42),
					TabIndex = 10
				};
				paneltemp1.Controls.Add(labeltemp1);

				Label labeltemp2 = new Label
				{
					AutoSize = true,
					Location = new Point(0, 29),
					Font =
						new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
							System.Drawing.GraphicsUnit.Point, ((byte) (0))),
					Name = "panel" + i + "count",
					Size = new Size(25, 13),
					TabIndex = 1,
					Text = "000"
				};

				Panel paneltemp2 = new Panel
				{
					BackColor = Color.Transparent,
					BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center,
					Location = new Point(paneloffset, 71),
					Margin = new System.Windows.Forms.Padding(0),
					Name = "panel" + i + 1,
					Size = new Size(42, 42),
					TabIndex = 10
				};
				paneltemp2.Controls.Add(labeltemp2);

				m_panellist.Add(paneltemp1);
				m_panelcount.Add(labeltemp1);
				m_form.Controls.Add(paneltemp1);

				m_panellist.Add(paneltemp2);
				m_panelcount.Add(labeltemp2);
				m_form.Controls.Add(paneltemp2);

				m_form.ResumeLayout();
				paneloffset += 60;
			}

			m_form.BackgroundImage = BackgroundAddHorizontal(tempBackground, Assistant.Properties.Resources.BarraGrandeOrizzontaBordoDestro);
			InitEvent();
		}

		private static void DrawToolBarSV() // Piccola Verticale
		{
			m_panellist = new List<Panel>();
			m_panelcount = new List<Label>();

			m_form = new ToolBarForm();
			m_form.SuspendLayout();

			// Sfondo e parametri offset
			Bitmap sfondotemporaneo = Assistant.Properties.Resources.BarraVerticaleBordoSopra;
			int height = Assistant.Properties.Resources.BarraVerticaleBordoSopra.Height + Assistant.Properties.Resources.BarraVerticaleBordoSotto.Height;
			int offsetstat = 8;
			int paneloffset = 12;
			height += (m_slot * 36); // Aggiungo spazio slot

			if (Settings.General.ReadBool("ShowHitsToolBarCheckBox"))
			{
				m_strlabelSV = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "S: 999"
				};
				m_form.Controls.Add(m_strlabelSV);

				m_hitlabelSV = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat + 13),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "H: 999"
				};
				m_form.Controls.Add(m_hitlabelSV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			if (Settings.General.ReadBool("ShowManaToolBarCheckBox"))
			{
				m_intlabelSV = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "I: 999"
				};
				m_form.Controls.Add(m_intlabelSV);

				m_manalabelSV = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat + 13),
					Name = "label1",
					Size = new Size(48, 12),
					TabIndex = 0,
					Text = "M: 200"
				};
				m_form.Controls.Add(m_manalabelSV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			if (Settings.General.ReadBool("ShowStaminaToolBarCheckBox"))
			{
				m_dexlabelSV = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "D: 999"
				};
				m_form.Controls.Add(m_dexlabelSV);

				m_stamlabelSV = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat + 13),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "S: 999"
				};
				m_form.Controls.Add(m_stamlabelSV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			if (Settings.General.ReadBool("ShowWeightToolBarCheckBox"))
			{
				m_weightlabelSV = new Label
				{
					AutoSize = true,
					Font = new Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "W: 999"
				};
				m_form.Controls.Add(m_weightlabelSV);

				m_weightmaxlabelSV = new Label
				{
					AutoSize = true,
					Font = new Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat + 13),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "W: 999"
				};
				m_form.Controls.Add(m_weightmaxlabelSV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

            //
            if (Settings.General.ReadBool("ShowTitheToolBarCheckBox"))
            {
                m_tithelabelSV = new Label
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Location = new Point(5, offsetstat),
                    Name = "label1",
                    Size = new Size(50, 12),
                    TabIndex = 0,
                    Text = "T: 99999999"
                };
                m_form.Controls.Add(m_tithelabelSV);

                sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSpazioStat);
                height += 29;
                offsetstat += 30;
                paneloffset += 29;
            }

            //
            if (Settings.General.ReadBool("ShowFollowerToolBarCheckBox"))
			{
				m_followerlabelSV = new Label
				{
					AutoSize = true,
					Font = new Font("Microsoft Sans Serif", 6.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(5, offsetstat + 7),
					Name = "label1",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "F: 5"
				};
				m_form.Controls.Add(m_followerlabelSV);

				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSpazioStat);

				height += 29;
				offsetstat += 30;
				paneloffset += 29;
			}

			for (int i = 0; i < m_slot; i++)
			{
				//Genero sfondo slot
				sfondotemporaneo = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleSlot);

				// Aggiungo panel dinamici

				Label labeltemp = new Label
				{
					AutoSize = true,
					Location = new Point(0, 18),
					Font =
						new Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
							GraphicsUnit.Point, ((byte) (0))),
					Name = "panel" + i + "count",
					Size = new Size(10, 20),
					TabIndex = 1,
					Text = "000"
				};

				Panel paneltemp = new Panel
				{
					BackgroundImageLayout = ImageLayout.Center,
					BackColor = Color.Transparent,
					Location = new Point(6, paneloffset),
					Margin = new Padding(0),
					Name = "panel" + i,
					Size = new Size(29, 29),
					TabIndex = 15
				};
				paneltemp.Controls.Add(labeltemp);

				m_panellist.Add(paneltemp);
				m_panelcount.Add(labeltemp);
				m_form.Controls.Add(paneltemp);
				m_form.ResumeLayout();

				paneloffset += 36;
			}

			m_form.ClientSize = new Size(Assistant.Properties.Resources.BarraVerticaleBordoSopra.Width, height);

			m_form.BackgroundImage = BackGroundAddVerticale(sfondotemporaneo, Assistant.Properties.Resources.BarraVerticaleBordoSotto);
			InitEvent();
		}

		private static void DrawToolBarSH() // Piccola Orizzontale
		{
			m_panellist = new List<Panel>();
			m_panelcount = new List<Label>();

			m_form = new ToolBarForm();
			m_form.SuspendLayout();

			int width = Assistant.Properties.Resources.BarraOrizzontaBordoDestro.Width + Assistant.Properties.Resources.BarraOrizzontaBordoSinistro.Width;
			int offsetstat = 5;
			int paneloffset = 11;

			// Genero Sfondo
			Bitmap sfondotemporaneo = Assistant.Properties.Resources.BarraOrizzontaBordoSinistro;

			if (Settings.General.ReadBool("ShowHitsToolBarCheckBox"))
			{
				Label m_hits_label = new Label
				{
					AutoSize = false,
					Width = 5,
					Height = 5,
					Font = new Font("Microsoft Sans Serif", 6.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat + 16, 3),
					Name = "h",
					Size = new Size(20, 12),
					TabIndex = 10,
					Text = "H",
					BackColor = Color.Transparent
				};
				m_form.Controls.Add(m_hits_label);

				m_hitslabelSH = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat, 14),
					Name = "hitslabel",
					Size = new Size(50, 12),
					TabIndex = 0,
					Text = "999/999"
				};
				m_form.Controls.Add(m_hitslabelSH);

				sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat);

				width += 51;
				offsetstat += 56;
				paneloffset += 51;
			}

			if (Settings.General.ReadBool("ShowManaToolBarCheckBox"))
			{
				Label m_mana_label = new Label
				{
					AutoSize = false,
					Width = 5,
					Height = 5,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat + 16, 3),
					Name = "m",
					Size = new Size(20, 12),
					TabIndex = 10,
					Text = "M",
					BackColor = Color.Transparent
				};
				m_form.Controls.Add(m_mana_label);

				m_manalabelSH = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat, 14),
					Name = "manalabel",
					Size = new Size(52, 12),
					TabIndex = 1,
					Text = "M: 999/999"
				};
				m_form.Controls.Add(m_manalabelSH);

				sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat);

				width += 51;
				offsetstat += 56;
				paneloffset += 51;
			}

			if (Settings.General.ReadBool("ShowStaminaToolBarCheckBox"))
			{
				Label m_stam_label = new Label
				{
					AutoSize = false,
					Width = 5,
					Height = 5,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat + 16, 3),
					Name = "s",
					Size = new Size(20, 12),
					TabIndex = 10,
					Text = "S",
					BackColor = Color.Transparent
				};
				m_form.Controls.Add(m_stam_label);

				m_staminalabelSH = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat, 14),
					Name = "stamlabel",
					Size = new Size(49, 12),
					TabIndex = 2,
					Text = "S: 999/999"
				};
				m_form.Controls.Add(m_staminalabelSH);

				sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat);

				width += 51;
				offsetstat += 56;
				paneloffset += 51;
			}
            //


            if (Settings.General.ReadBool("ShowWeightToolBarCheckBox"))
			{
				Label m_weight_label = new Label
				{
					AutoSize = false,
					Width = 5,
					Height = 5,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat + 12, 3),
					Name = "w",
					Size = new Size(20, 12),
					TabIndex = 10,
					Text = "W",
					BackColor = Color.Transparent
				};
				m_form.Controls.Add(m_weight_label);

				m_weightlabelSH = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat - 5, 14),
					Name = "weightlabel",
					Size = new Size(49, 12),
					TabIndex = 2,
					Text = "W: 999/999"
				};
				m_form.Controls.Add(m_weightlabelSH);

				sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat);

				width += 51;
				offsetstat += 49;
				paneloffset += 51;
			}
            if (Settings.General.ReadBool("ShowTitheToolBarCheckBox"))
            {
                Label m_tithe_label = new Label
                {
                    AutoSize = false,
                    Width = 5,
                    Height = 5,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Location = new Point(offsetstat + 12, 3),
                    Name = "t",
                    Size = new Size(20, 12),
                    TabIndex = 10,
                    Text = "T",
                    BackColor = Color.Transparent
                };
                m_form.Controls.Add(m_tithe_label);

                m_tithelabelSH = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Location = new Point(offsetstat - 5, 14),
                    Name = "tithelabel",
                    Size = new Size(49, 12),
                    TabIndex = 2,
                    Text = "T: 9999999"
                };
                m_form.Controls.Add(m_tithelabelSH);

                sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat);

                width += 51;
                offsetstat += 49;
                paneloffset += 51;
            }

            //

            if (Settings.General.ReadBool("ShowFollowerToolBarCheckBox"))
			{
				Label m_follower_label = new Label
				{
					AutoSize = false,
					Width = 5,
					Height = 5,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat + 7, 3),
					Name = "f",
					Size = new Size(20, 12),
					TabIndex = 10,
					Text = "F",
					BackColor = Color.Transparent
				};
				m_form.Controls.Add(m_follower_label);

				m_followerlabelSH = new Label
				{
					AutoSize = true,
					Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
					Location = new Point(offsetstat, 14),
					Name = "followerlabel",
					Size = new Size(49, 12),
					TabIndex = 2,
					Text = "F: 5"
				};
				m_form.Controls.Add(m_followerlabelSH);

				sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat);

				width += 51;
				offsetstat += 50;
				paneloffset += 51;
			}

			width += (m_slot * 36); // Aggiungo spazio slot

			m_form.ClientSize = new Size(width, Assistant.Properties.Resources.BarraOrizzontaleSpazioStat.Height);

			for (int i = 0; i < m_slot; i++)
			{
				//Genero sfondo slot
				sfondotemporaneo = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaleSlot);

				// Aggiungo panel dinamici

				Label labeltemp = new Label
				{
					AutoSize = true,
					Location = new Point(0, 18),
					Font =
						new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
							System.Drawing.GraphicsUnit.Point, ((byte) (0))),
					Name = "panel" + i + "count",
					Size = new Size(10, 20),
					TabIndex = 1,
					Text = "000"
				};


				Panel paneltemp = new Panel
				{
					BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center,
					Location = new Point(paneloffset, 6),
					Margin = new System.Windows.Forms.Padding(0),
					Name = "panel" + i,
					Size = new Size(29, 29),
					TabIndex = 15,
					BackColor = Color.Transparent
				};
				paneltemp.Controls.Add(labeltemp);

				m_panellist.Add(paneltemp);
				m_panelcount.Add(labeltemp);
				m_form.Controls.Add(paneltemp);

				m_form.ResumeLayout();

				paneloffset += 36;
			}

			m_form.BackgroundImage = BackgroundAddHorizontal(sfondotemporaneo, Assistant.Properties.Resources.BarraOrizzontaBordoDestro);
			InitEvent();
		}

		internal static void EnhancedToolbar_close(object sender, EventArgs e)
		{
			m_form = null;
			m_slot = 0;
        }

		internal static void EnhancedToolbar_Move(object sender, System.EventArgs e)
		{
			Point pt = m_form.Location;
				if (m_form.WindowState != FormWindowState.Minimized)
				{
					Assistant.Engine.MainWindow.LocationToolBarLabel.Text = "X: " + pt.X + " - Y:" + pt.Y;
					Assistant.Engine.ToolBarX = pt.X;
					Assistant.Engine.ToolBarY = pt.Y;
				}
		}

		private static Bitmap BackGroundAddVerticale(Image firstImage, Image secondImage)
		{
			int outputImageWidth = firstImage.Width;

			int outputImageHeight = firstImage.Height + secondImage.Height;

			Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			using (Graphics graphics = Graphics.FromImage(outputImage))
			{
				graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
					new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
				graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height), secondImage.Size),
					new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
			}

			return outputImage;
		}

		private static Bitmap BackgroundAddHorizontal(Image firstImage, Image secondImage)
		{
			int outputImageWidth = firstImage.Width + secondImage.Width;

			int outputImageHeight = firstImage.Height;

			Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			using (Graphics graphics = Graphics.FromImage(outputImage))
			{
				graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
					new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
				graphics.DrawImage(secondImage, new Rectangle(new Point(firstImage.Width, 0), secondImage.Size),
					new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
			}

			return outputImage;
		}

		private static Bitmap ResizeImage(System.Drawing.Bitmap value, int newWidth, int newHeight)
		{
			Bitmap resizedImage = new System.Drawing.Bitmap(newWidth, newHeight);
			using (Graphics graphics = Graphics.FromImage(resizedImage))
			{
				System.Drawing.Graphics.FromImage((System.Drawing.Image)resizedImage).DrawImage(value, 0, 0, newWidth, newHeight);
			}

			return (resizedImage);
		}


		public static Bitmap CropImage(Bitmap img)
		{
			Point min = new Point(int.MaxValue, int.MaxValue);
			Point max = new Point(int.MinValue, int.MinValue);

			for (int x = 0; x < img.Width; ++x)
			{
				for (int y = 0; y < img.Height; ++y)
				{
					Color pixelColor = img.GetPixel(x, y);
					if (pixelColor.A != 0)
					{
						if (x < min.X)
						{
							min.X = x;
						}
						if (y < min.Y)
						{
							min.Y = y;
					}

					if (x > max.X) max.X = x;
						if (y > max.Y) max.Y = y;
					}
				}
			}
			max.X += 2;
			max.Y += 2;

			// Create a new bitmap from the crop rectangle
			Rectangle cropRectangle = new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
			Bitmap newBitmap = new Bitmap(cropRectangle.Width, cropRectangle.Height);
			using (Graphics g = Graphics.FromImage(newBitmap))
			{
				g.DrawImage(img, 0, 0, cropRectangle, GraphicsUnit.Pixel);
			}
			return (newBitmap);
		}
	}
}
