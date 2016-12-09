using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Assistant;

namespace RazorEnhanced
{
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
			BackColor = Color.FromArgb(187, 182, 137);
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			MaximizeBox = false;
			CausesValidation = false;
		}

	/*	protected override void OnPaint(PaintEventArgs e)
		{
			if (!ToolBar.Lock)
				ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
			base.OnPaint(e);
		}*/
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
		internal static Form SpellGridForm
		{
			get	{return m_form;} set {m_form = value;}
		}

		internal static void SpellGrid_close(object sender, EventArgs e)
		{
			m_form = null;
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
			if (m_form != null)
			{
				m_form.Close();
				m_form = null;
			}
		}

		internal static void LockUnlock()
		{
			if (m_form != null)
			{
				if (m_lock)
					m_lock = false;
				else
					m_lock = true;
				m_form.ContextMenu = GeneraMenu();
				m_form.Refresh();
			}
		}

		//////////////////////////////////////////////////////////////
		// Form Dragmove start
		//////////////////////////////////////////////////////////////

		private static bool m_mouseDown;
		private static Point m_lastLocation;

		internal static void SpellGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if (!m_lock)
			{
				m_mouseDown = true;
				m_lastLocation = e.Location;
			}
		}

		internal static void SpellGrid_MouseMove(object sender, MouseEventArgs e)
		{
			if (!m_lock)
			{
				if (m_mouseDown)
				{
					m_form.Location = new Point(
						(m_form.Location.X - m_lastLocation.X) + e.X, (m_form.Location.Y - m_lastLocation.Y) + e.Y);

					m_form.Update();
				}
			}
		}

		internal static void SpellGrid_MouseUp(object sender, MouseEventArgs e)
		{
			if (!m_lock)
				m_mouseDown = false;
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
				control.MouseClick += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseClick);
				control.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SpellGrid_MouseClick);
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
	}
}