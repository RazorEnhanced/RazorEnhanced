using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Assistant;
using PaxScript.Net;


namespace RazorEnhanced
{
	internal class Scripts
	{
		internal class ScriptWorker
		{
			private ConcurrentQueue<RunMode> m_Actions = new ConcurrentQueue<RunMode>();
			internal ConcurrentQueue<RunMode> Actions { get { return m_Actions; } }

			private EnhancedScript m_Script;
			internal EnhancedScript Script { get { return m_Script; } }

			private TimeSpan m_Delay;
			internal TimeSpan Delay { get { return m_Delay; } }

			private bool m_AutoMode;
			internal bool AutoMode { get { return m_AutoMode; } }

			internal ScriptWorker(EnhancedScript script, TimeSpan delay, bool autoMode)
			{
				m_Script = script;
				m_Delay = delay;
				m_AutoMode = autoMode;
			}

			// This method will be called when the thread is started.
			internal void AsyncRun()
			{
				int exit = Int32.MinValue;

				while (!_shouldStop)
				{
					Thread.Sleep(m_Delay);

					RunMode action;
					if (m_Actions.Count > 0 && m_Actions.TryDequeue(out action))
					{
						exit = InvokeMethod<int>(m_Script, "Run", action);

						if (this.AutoMode)
						{
							if (exit != 0)
								_shouldStop = true;
						}
						else
						{
							if (exit != Int32.MinValue)
								_shouldStop = true;
						}
					}
				}

				if (exit != 0)
				{
					Assistant.Engine.MainWindow.razorCheckBoxAuto.Checked = false;
					Assistant.World.Player.SendMessage(LocString.EnhancedMacroError, exit);
				}
			}
			internal void RequestStop()
			{
				_shouldStop = true;
			}

			// Volatile is used as hint to the compiler that this data
			// member will be accessed by multiple threads.
			private volatile bool _shouldStop;
		}

		internal class EnhancedScript
		{
			private string m_File;
			internal string File { get { return m_File; } }

			private string m_Class;
			internal string Class { get { return m_Class; } }

			internal EnhancedScript(string file, string classname)
			{
				m_File = file;
				m_Class = classname;
			}
		}

		private static List<ScriptWorker> m_Workers = new List<ScriptWorker>();
		internal static List<ScriptWorker> Workers { get { return m_Workers; } }

		private static List<Task> m_Tasks = new List<Task>();
		internal static List<Task> Tasks { get { return m_Tasks; } }

		private static PaxScripter m_PaxScripter;

		public static void Initialize()
		{
			m_PaxScripter = new PaxScripter();
			m_PaxScripter.OnChangeState += new ChangeStateHandler(paxScripter_OnChangeState);
		}

		private static void paxScripter_OnChangeState(PaxScripter sender, ChangeStateEventArgs e)
		{
			if (e.OldState == ScripterState.Init)
			{
			}
			else if (sender.HasErrors)
			{
				MessageBox.Show(sender.Error_List[0].Message);
			}
		}

		private static bool m_Auto = false;
		internal static bool Auto
		{
			get { return m_Auto; }
			set
			{
				if (m_Tasks.Count != m_Workers.Count)
				{
					MessageBox.Show("Thread count different from Worker count", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (value)
				{
					for (int i = 0; i < m_Tasks.Count; i++)
					{
						Task task = m_Tasks[i];
						ScriptWorker worker = m_Workers[i];

						// Start the worker task.
						task.Start();
						// Loop until worker thread activates.
						while (task.Status == TaskStatus.Running)
						{
							Thread.Sleep(1);
						}

						Misc.SendMessage("Script " + worker.Script.Class + " started.");
					}
				}
				else
				{
					for (int i = 0; i < m_Tasks.Count; i++)
					{
						Task task = m_Tasks[i];
						ScriptWorker worker = m_Workers[i];

						// Request that the worker task stop itself:
						worker.RequestStop();

						Misc.SendMessage("Script " + worker.Script.Class + " stopped.");
					}
				}

				m_Auto = value;
			}
		}

		internal static void Reset()
		{
			Auto = false;
			m_Workers.Clear();
			m_Tasks.Clear();
			m_PaxScripter.Reset();
		}

		private static TimeSpan m_AutoDelay = TimeSpan.FromMilliseconds(25);

		internal static string Load(string file)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(file);

			EnhancedScript script = new EnhancedScript(file, classname);

			ScriptWorker worker = new ScriptWorker(script, m_AutoDelay, true);
			worker.Actions.Enqueue(RunMode.Run);

			Task task = new Task(worker.AsyncRun);

			try
			{
				m_Workers.Add(worker);
				m_Tasks.Add(task);
				m_PaxScripter.AddModule(script.Class);
				m_PaxScripter.AddCodeFromFile(script.Class, script.File);
			}
			catch (Exception ex)
			{
				status = "ERROR: " + ex.Message;
			}

			return status;
		}

		private static T InvokeMethod<T>(EnhancedScript script, string method, RunMode action)
		{
			T result = default(T);
			try
			{
				result = (T)m_PaxScripter.Invoke(action, null, "RazorEnhanced." + script.Class + "." + method);
			}
			catch
			{
			}

			return result;
		}

		internal static void OnContainerUpdated(Assistant.Item container)
		{
		}
	}
}
