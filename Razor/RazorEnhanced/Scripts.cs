using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Reflection;
using Assistant;
using System.Threading.Tasks;
using PaxScript.Net;
using System.Threading;

namespace RazorEnhanced
{
	internal class Scripts
	{
		private static TimeSpan m_Delay = TimeSpan.FromMilliseconds(25.0);

		internal class ScriptWorker
		{
			private EnhancedScript m_Script;
			internal EnhancedScript Script { get { return m_Script; } }

			internal ScriptWorker(EnhancedScript script)
			{
				m_Script = script;
			}

			// This method will be called when the thread is started.
			internal void AsyncRun()
			{
				int exit = 0;

				while (!_shouldStop)
				{
					Thread.Sleep(m_Delay);
					exit = InvokeMethod<int>(m_Script, "Run");
					if (exit != 0)
						_shouldStop = true;
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

		private static List<Thread> m_Threads = new List<Thread>();
		internal static List<Thread> Threads { get { return m_Threads; } }

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
				if (m_Threads.Count != m_Workers.Count)
				{
					MessageBox.Show("Thread count different from Worker count", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (value)
				{
					for (int i = 0; i < m_Threads.Count; i++)
					{
						Thread thread = m_Threads[i];
						ScriptWorker worker = m_Workers[i];

						// Start the worker thread.
						thread.Start();
						// Loop until worker thread activates.
						while (!thread.IsAlive) ;

						Misc.SendMessage("Script " + worker.Script.Class + " started.");
					}
				}
				else
				{
					for (int i = 0; i < m_Threads.Count; i++)
					{
						Thread thread = m_Threads[i];
						ScriptWorker worker = m_Workers[i];

						// Request that the worker thread stop itself:
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
			m_Threads.Clear();
			m_PaxScripter.Reset();
		}

		internal static string Load(string file)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(file);
			EnhancedScript script = new EnhancedScript(file, classname);
			ScriptWorker worker = new ScriptWorker(script);
			Thread thread = new Thread(worker.AsyncRun);

			try
			{
				m_Workers.Add(worker);
				m_Threads.Add(thread);
				m_PaxScripter.AddModule(script.Class);
				m_PaxScripter.AddCodeFromFile(script.Class, script.File);
			}
			catch (Exception ex)
			{
				status = "ERROR: " + ex.Message;
			}

			return status;
		}

		private static T InvokeMethod<T>(EnhancedScript script, string method)
		{
			T result = default(T);
			try
			{
				result = (T)m_PaxScripter.Invoke(RunMode.Run, null, "RazorEnhanced." + script.Class + "." + method);
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
