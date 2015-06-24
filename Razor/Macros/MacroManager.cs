using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Assistant.Macros
{
	internal class MacroManager
	{
		private static List<Macro> m_List;
		private static Macro m_Current, m_PrevPlay;
		private static MacroTimer m_Timer;

		public static void Initialize()
		{
			//HotKey.Add(HKCategory.Macros, LocString.StopCurrent, new HotKeyCallback(HotKeyStop));

            string path = Directory.GetCurrentDirectory();
			Recurse(null, path);
			/*string[] macros = Directory.GetFiles( path, "*.macro" );
			for (int i=0;i<macros.Length;i++)
				Add( new Macro( macros[i] ) );*/
		}

		static MacroManager()
		{
			m_List = new List<Macro>();
			m_Timer = new MacroTimer();
		}

		internal static void Save()
		{
            Engine.EnsureDirectory(Directory.GetCurrentDirectory());
			for (int i = 0; i < m_List.Count; i++)
				((Macro)m_List[i]).Save();
		}

		internal static List<Macro> List { get { return m_List; } }
		internal static bool Recording { get { return m_Current != null && m_Current.Recording; } }
		internal static bool Playing { get { return m_Current != null && m_Current.Playing && m_Timer != null && m_Timer.Running; } }
		internal static Macro Current { get { return m_Current; } }
		internal static bool AcceptActions { get { return Recording || (Playing && m_Current.Waiting); } }
		//public static bool IsWaiting{ get{ return Playing && m_Current != null && m_Current.Waiting; } }

		internal static void Add(Macro m)
		{
			//HotKey.Add(HKCategory.Macros, HKSubCat.None, Language.Format(LocString.PlayA1, m), new HotKeyCallbackState(HotKeyPlay), m);
			m_List.Add(m);
		}

		internal static void Remove(Macro m)
		{
			//HotKey.Remove(Language.Format(LocString.PlayA1, m));
			m_List.Remove(m);
		}

		internal static void RecordAt(Macro m, int at)
		{
			if (m_Current != null)
				m_Current.Stop();
			m_Current = m;
			m_Current.RecordAt(at);
		}

		internal static void Record(Macro m)
		{
			if (m_Current != null)
				m_Current.Stop();
			m_Current = m;
			m_Current.Record();
		}

		internal static void PlayAt(Macro m, int at)
		{
			if (m_Current != null)
			{
				if (m_Current.Playing && m_Current.Loop && !m.Loop)
					m_PrevPlay = m_Current;
				else
					m_PrevPlay = null;

				m_Current.Stop();
			}
			else
			{
				m_PrevPlay = null;
			}

			LiftAction.LastLift = null;
			m_Current = m;
			m_Current.PlayAt(at);

			m_Timer.Macro = m_Current;
			m_Timer.Start();

			//if (Engine.MainWindow.WaitDisplay != null)
			//	Engine.MainWindow.WaitDisplay.Text = "";
		}

		private static void HotKeyPlay(ref object state)
		{
			HotKeyPlay(state as Macro);
		}

		internal static void HotKeyPlay(Macro m)
		{
			if (m != null)
			{
				Play(m);
				World.Player.SendMessage(LocString.PlayingA1, m);
				//Engine.MainWindow.PlayMacro(m);
			}
		}

		internal static void Play(Macro m)
		{
			if (m_Current != null)
			{
				if (m_Current.Playing && m_Current.Loop && !m.Loop)
					m_PrevPlay = m_Current;
				else
					m_PrevPlay = null;

				m_Current.Stop();
			}
			else
			{
				m_PrevPlay = null;
			}

			LiftAction.LastLift = null;
			m_Current = m;
			m_Current.Play();

			m_Timer.Macro = m_Current;
			m_Timer.Start();

		//	if (Engine.MainWindow.WaitDisplay != null)
			//	Engine.MainWindow.WaitDisplay.Text = "";
		}

		private static void HotKeyStop()
		{
			Stop();
		}

		internal static void Stop()
		{
			Stop(false);
		}

		internal static void Stop(bool restartPrev)
		{
			m_Timer.Stop();
			if (m_Current != null)
			{
				m_Current.Stop();
				m_Current = null;
			}
			ClientCommunication.PostMacroStop();

			/*if (Engine.MainWindow.WaitDisplay != null)
				Engine.MainWindow.WaitDisplay.Text = "";

			Engine.MainWindow.OnMacroStop();*/

			//if ( restartPrev )
			//	Play( m_PrevPlay );
			m_PrevPlay = null;
		}

		internal static void DisplayTo(TreeView tree)
		{
			tree.BeginUpdate();
			tree.Nodes.Clear();
            Recurse(tree.Nodes, Directory.GetCurrentDirectory());
			tree.EndUpdate();
			tree.Refresh();
			tree.Update();
		}

		private static void Recurse(TreeNodeCollection nodes, string path)
		{
			try
			{
				string[] macros = Directory.GetFiles(path, "*.macro");
				for (int i = 0; i < macros.Length; i++)
				{
					Macro m = null;
					for (int j = 0; j < m_List.Count; j++)
					{
						Macro check = (Macro)m_List[j];

						if (check.Filename == macros[i])
						{
							m = check;
							break;
						}
					}

					if (m == null)
						Add(m = new Macro(macros[i]));

					if (nodes != null)
					{
						TreeNode node = new TreeNode(Path.GetFileNameWithoutExtension(m.Filename));
						node.Tag = m;
						nodes.Add(node);
					}
				}
			}
			catch
			{
			}

			try
			{
				string[] dirs = Directory.GetDirectories(path);
				for (int i = 0; i < dirs.Length; i++)
				{
					if (dirs[i] != "" && dirs[i] != "." && dirs[i] != "..")
					{
						if (nodes != null)
						{
							TreeNode node = new TreeNode(String.Format("[{0}]", Path.GetFileName(dirs[i])));
							node.Tag = dirs[i];
							nodes.Add(node);
							Recurse(node.Nodes, dirs[i]);
						}
						else
						{
							Recurse(null, dirs[i]);
						}
					}
				}
			}
			catch
			{
			}
		}

		internal static void Select(Macro m, ListBox actionList, Button play, Button rec, CheckBox loop)
		{
			if (m == null)
				return;

			m.DisplayTo(actionList);

			if (Recording)
			{
				play.Enabled = false;
				play.Text = "Play";
				rec.Enabled = true;
				rec.Text = "Stop";
			}
			else
			{
				play.Enabled = true;
				if (m.Playing)
				{
					play.Text = "Stop";
					rec.Enabled = false;
				}
				else
				{
					play.Text = "Play";
					rec.Enabled = true;
				}
				rec.Text = "Record";
				loop.Checked = m.Loop;
			}
		}

		internal static bool Action(MacroAction a)
		{
			if (m_Current != null)
				return m_Current.Action(a);
			else
				return false;
		}

		private class MacroTimer : Timer
		{
			private Macro m_Macro;

			internal MacroTimer()
				: base(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(50))
			{
			}

			internal Macro Macro { get { return m_Macro; } set { m_Macro = value; } }

			protected override void OnTick()
			{
				try
				{
					if (m_Macro == null || World.Player == null)
					{
						this.Stop();
						MacroManager.Stop();
					}
					else if (!m_Macro.ExecNext())
					{
						this.Stop();
						MacroManager.Stop(true);
						World.Player.SendMessage(LocString.MacroFinished, m_Macro);
					}
				}
				catch
				{
					this.Stop();
					MacroManager.Stop();
				}
			}
		}
	}
}

