using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal void RedrawSkills()
		{
			skillList.BeginUpdate();
			skillList.Items.Clear();
			double Total = 0;
			if (World.Player != null && World.Player.SkillsSent)
			{
				string[] items = new string[6];
				for (int i = 0; i < Skill.Count; i++)
				{
					Skill sk = World.Player.Skills[i];
					Total += sk.Base;
					items[0] = Language.Skill2Str(i);//((SkillName)i).ToString();
					items[1] = String.Format("{0:F1}", sk.Value);
					items[2] = String.Format("{0:F1}", sk.Base);
					items[3] = String.Format("{0}{1:F1}", (sk.Delta > 0 ? "+" : String.Empty), sk.Delta);
					items[4] = String.Format("{0:F1}", sk.Cap);
					items[5] = sk.Lock.ToString()[0].ToString();

					ListViewItem lvi = new ListViewItem(items)
					{
						Tag = sk
					};
					skillList.Items.Add(lvi);
				}

				//Config.SetProperty( "SkillListAsc", false );
				SortSkills();
			}
			skillList.EndUpdate();
			baseTotal.Text = String.Format("{0:F1}", Total);
		}

		private ContextMenu m_SkillMenu;

		private void skillList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ListView.SelectedListViewItemCollection items = skillList.SelectedItems;
				if (items.Count <= 0)
					return;

				Skill s = items[0].Tag as Skill;
				if (s == null)
					return;

				if (m_SkillMenu == null)
				{
					m_SkillMenu = new ContextMenu(new MenuItem[]
					{
						new MenuItem( Language.GetString( LocString.SetSLUp ), new EventHandler( onSetSkillLockUP ) ),
						new MenuItem( Language.GetString( LocString.SetSLDown ), new EventHandler( onSetSkillLockDOWN ) ),
						new MenuItem( Language.GetString( LocString.SetSLLocked ), new EventHandler( onSetSkillLockLOCKED ) ),
					});
				}

				for (int i = 0; i < 3; i++)
					m_SkillMenu.MenuItems[i].Checked = ((int)s.Lock) == i;

				m_SkillMenu.Show(skillList, new Point(e.X, e.Y));
			}
		}

		private void onSetSkillLockUP(object sender, EventArgs e)
		{
			SetLock(LockType.Up);
		}

		private void onSetSkillLockDOWN(object sender, EventArgs e)
		{
			SetLock(LockType.Down);
		}

		private void onSetSkillLockLOCKED(object sender, EventArgs e)
		{
			SetLock(LockType.Locked);
		}

		private void SetLock(LockType lockType)
		{
			ListView.SelectedListViewItemCollection items = skillList.SelectedItems;
			if (items.Count <= 0)
				return;

			Skill s = null;
			for (int i = 0; i < items.Count; i++)
			{
				s = items[i].Tag as Skill;
				if (s == null)
					continue;

				try
				{
			 		Assistant.Client.Instance.SendToServer(new SetSkillLock(s.Index, lockType));

					s.Lock = lockType;
					UpdateSkill(s);

			 		Assistant.Client.Instance.SendToClient(new SkillUpdate(s));
				}
				catch
				{
				}
			}
		}

		internal void UpdateSkill(Skill skill)
		{
			double Total = 0;
			for (int i = 0; i < Skill.Count; i++)
				Total += World.Player.Skills[i].Base;
			baseTotal.Text = String.Format("{0:F1}", Total);
			for (int i = 0; i < skillList.Items.Count; i++)
			{
				ListViewItem cur = skillList.Items[i];
				if (cur.Tag == skill)
				{
					cur.SubItems[1].Text = String.Format("{0:F1}", skill.Value);
					cur.SubItems[2].Text = String.Format("{0:F1}", skill.Base);
					cur.SubItems[3].Text = String.Format("{0}{1:F1}", (skill.Delta > 0 ? "+" : String.Empty), skill.Delta);
					cur.SubItems[4].Text = String.Format("{0:F1}", skill.Cap);
					cur.SubItems[5].Text = skill.Lock.ToString()[0].ToString();
					SortSkills();
					return;
				}
			}
		}

		private void OnSkillColClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			if (e.Column == RazorEnhanced.Settings.General.ReadInt("SkillListCol"))
				RazorEnhanced.Settings.General.WriteBool("SkillListAsc", !RazorEnhanced.Settings.General.ReadBool("SkillListAsc"));
			else
				RazorEnhanced.Settings.General.WriteInt("SkillListCol", e.Column);
			SortSkills();
		}

		private void SortSkills()
		{
			int col = RazorEnhanced.Settings.General.ReadInt("SkillListCol");
			bool asc = RazorEnhanced.Settings.General.ReadBool("SkillListAsc");

			if (col < 0 || col > 5)
				col = 0;

			skillList.BeginUpdate();
			if (col == 0 || col == 5)
			{
				skillList.ListViewItemSorter = null;
				skillList.Sorting = asc ? SortOrder.Ascending : SortOrder.Descending;
			}
			else
			{
				LVDoubleComparer.Column = col;
				LVDoubleComparer.Asc = asc;

				skillList.ListViewItemSorter = LVDoubleComparer.Instance;

				skillList.Sorting = SortOrder.None;
				skillList.Sort();
			}
			skillList.EndUpdate();
			skillList.Refresh();
		}

		private class LVDoubleComparer : IComparer
		{
			internal static readonly LVDoubleComparer Instance = new LVDoubleComparer();
			internal static int Column { set { Instance.m_Col = value; } }
			internal static bool Asc { set { Instance.m_Asc = value; } }

			private int m_Col;
			private bool m_Asc;

			private LVDoubleComparer()
			{
			}

			public int Compare(object x, object y)
			{
				if (x == null || !(x is ListViewItem))
					return m_Asc ? 1 : -1;
				else if (y == null || !(y is ListViewItem))
					return m_Asc ? -1 : 1;

				try
				{
					double dx = Convert.ToDouble(((ListViewItem)x).SubItems[m_Col].Text);
					double dy = Convert.ToDouble(((ListViewItem)y).SubItems[m_Col].Text);

					if (dx > dy)
						return m_Asc ? -1 : 1;
					else if (dx == dy)
						return 0;
					else //if ( dx > dy )
						return m_Asc ? 1 : -1;
				}
				catch
				{
					return ((ListViewItem)x).Text.CompareTo(((ListViewItem)y).Text) * (m_Asc ? 1 : -1);
				}
			}
		}

		private void OnResetSkillDelta(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			for (int i = 0; i < Skill.Count; i++)
				World.Player.Skills[i].Delta = 0;

			RedrawSkills();
		}

		private void OnSetSkillLocks(object sender, System.EventArgs e)
		{
			if (locks.SelectedIndex == -1 || World.Player == null)
				return;

			LockType type = (LockType)locks.SelectedIndex;

			for (short i = 0; i < Skill.Count; i++)
			{
				World.Player.Skills[i].Lock = type;
		 		Assistant.Client.Instance.SendToServer(new SetSkillLock(i, type));
			}
	 		Assistant.Client.Instance.SendToClient(new SkillsList());
			RedrawSkills();
		}

		private void OnDispSkillCheck(object sender, System.EventArgs e)
		{
			RazorEnhanced.Settings.General.WriteBool("DispSkillChanges", dispDelta.Checked);
		}

		private void skillCopySel_Click(object sender, System.EventArgs e)
		{
			if (skillList.SelectedItems == null || skillList.SelectedItems.Count <= 0)
				return;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < skillList.SelectedItems.Count; i++)
			{
				ListViewItem vi = skillList.SelectedItems[i];
				if (vi != null && vi.SubItems != null && vi.SubItems.Count > 4)
				{
					string name = vi.SubItems[0].Text;
					if (name != null && name.Length > 20)
						name = name.Substring(0, 16) + "...";

					sb.AppendFormat("{0,-20} {1,5:F1} {2,5:F1} {4:F1} {5,5:F1}\n",
						name,
						vi.SubItems[1].Text,
						vi.SubItems[2].Text,
						Utility.ToInt32(vi.SubItems[3].Text, 0) < 0 ? String.Empty : "+",
						vi.SubItems[3].Text,
						vi.SubItems[4].Text);
				}
			}

			if (sb.Length > 0)
				Clipboard.SetDataObject(sb.ToString(), true);
		}

		private void skillCopyAll_Click(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Skill.Count; i++)
			{
				Skill sk = World.Player.Skills[i];
				sb.AppendFormat("{0,-20} {1,-5:F1} {2,-5:F1} {3}{4,-5:F1} {5,-5:F1}\n", (SkillName)i, sk.Value, sk.Base, sk.Delta > 0 ? "+" : String.Empty, sk.Delta, sk.Cap);
			}

			if (sb.Length > 0)
				Clipboard.SetDataObject(sb.ToString(), true);
		}
	}
}
