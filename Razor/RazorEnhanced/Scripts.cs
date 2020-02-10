using Assistant;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Scripting;

namespace RazorEnhanced
{
	internal class Scripts
	{
		internal static Thread ScriptEditorThread;
		internal static bool ScriptErrorLog = false;
		internal static bool ScriptStartStopMessage = false;

		internal enum RunMode
		{
			None,
			RunOnce,
			Loop,
		}

		internal static void SendMessageScriptError(string msg)
		{
			if (Assistant.World.Player == null)
				return;

			if (RazorEnhanced.Settings.General.ReadBool("ShowScriptMessageCheckBox"))
		 		Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 945, 3, Language.CliLocName, "System", msg.ToString()));
		}

		internal class EnhancedScript
		{
			internal void Start()
			{
				if (IsRunning || !IsUnstarted)
					return;

				try
				{
					m_Thread.Start();
					while (!m_Thread.IsAlive)
					{
					}

					m_Run = true;
				}
				catch { }
			}

			private void AsyncStart()
			{
				if (m_Source == null)
					return;

				if (World.Player == null)
					return;

				try
				{
					m_Source.Execute(m_Scope);
				}
				catch (Exception ex)
				{
					if (ex is System.Threading.ThreadAbortException)
						return;

					if (ScriptErrorLog) // enabled log of error
					{
						StringBuilder log = new StringBuilder();
						log.Append(Environment.NewLine + "============================ START REPORT ============================ " + Environment.NewLine);

						DateTime dt = DateTime.Now;
						log.Append("---> Time: " + String.Format("{0:F}", dt) + Environment.NewLine);
						log.Append(Environment.NewLine);

						if (ex is SyntaxErrorException)
						{
							SyntaxErrorException se = ex as SyntaxErrorException;
							log.Append("----> Syntax Error:" + Environment.NewLine);
							log.Append("-> LINE: " + se.Line + Environment.NewLine);
							log.Append("-> COLUMN: " + se.Column + Environment.NewLine);
							log.Append("-> SEVERITY: " + se.Severity + Environment.NewLine);
							log.Append("-> MESSAGE: " + se.Message + Environment.NewLine);
						}
						else
						{
							log.Append("----> Generic Error:" + Environment.NewLine);
							ExceptionOperations eo = m_Engine.GetService<ExceptionOperations>();
							string error = eo.FormatException(ex);
							log.Append(error);
						}

						log.Append(Environment.NewLine);
						log.Append("============================ END REPORT ============================ ");
						log.Append(Environment.NewLine);

						try // For prevent crash in case of file are busy or inaccessible
						{
							File.AppendAllText(Assistant.Engine.RootPath + "\\" + m_Filename + ".ERROR", log.ToString());
						}
						catch { }
						log.Clear();
					}
				}
			}

			internal void Stop()
			{
				if (!IsStopped)
					try
					{
						if (m_Thread.ThreadState != ThreadState.AbortRequested)
						{
							m_Thread.Abort();
						}
					}
					catch { }
			}

			internal void Reset()
			{
				m_Thread = new Thread(AsyncStart);
				m_Run = false;
			}

			internal string Create(TracebackDelegate traceFunc)
			{
				string result = String.Empty;
				try
				{
					// none of this adds to the python path but I am leaving it here because there HAS to be a way to do this
					//Environment.SetEnvironmentVariable("IRONPYTHONPATH", Misc.CurrentScriptDirectory());
					m_Engine = Python.CreateEngine();
					//m_Engine.GetService<ExceptionOperations>().FormatException(exception);
					//ICollection<string> paths = m_Engine.GetSearchPaths();
					//paths.Add(Misc.CurrentScriptDirectory());
					//m_Engine.SetSearchPaths(paths);
					//
					m_Source = m_Engine.CreateScriptSourceFromString(m_Text);
					m_Scope = GetRazorScope(m_Engine);

					if (traceFunc != null)
						m_Engine.SetTrace(traceFunc);

					result = "Created";
				}
				catch (Exception ex)
				{
					result = ex.Message;
				}

				return result;
			}

			internal string Status
			{
				get
				{
					switch (m_Thread.ThreadState)
					{
						case ThreadState.Aborted:
						case ThreadState.AbortRequested:
							return "Stopping";

						case ThreadState.WaitSleepJoin:
						case ThreadState.Running:
							return "Running";

						default:
							return "Stopped";
					}
				}
			}

			private string m_Filename;
			internal string Filename
			{
				get
				{
					lock (m_Lock)
					{
						return m_Filename;
					}
				}
			}

			private string m_Text;
			internal string Text
			{
				get
				{
					lock (m_Lock)
					{
						return m_Text;
					}
				}
			}

			private Thread m_Thread;

			private bool m_Wait;
			internal bool Wait
			{
				get
				{
					lock (m_Lock)
					{
						return m_Wait;
					}
				}
			}

			private bool m_Loop;
			internal bool Loop
			{
				get
				{
					lock (m_Lock)
					{
						return m_Loop;
					}
				}
				set
				{
					lock (m_Lock)
					{
						m_Loop = value;
					}
				}
			}

			private bool m_AutoStart;
			internal bool AutoStart
			{
				get
				{
					lock (m_Lock)
					{
						return m_AutoStart;
					}
				}
				set
				{
					lock (m_Lock)
					{
						m_AutoStart = value;
					}
				}
			}

			private bool m_Run;
			internal bool Run
			{
				get
				{
					lock (m_Lock)
					{
						return m_Run;
					}
				}
				set
				{
					lock (m_Lock)
					{
						m_Run = value;
					}
				}
			}

			private object m_Lock = new object();

			internal bool IsRunning
			{
				get
				{
					lock (m_Lock)
					{
						if ( (m_Thread.ThreadState & (ThreadState.Unstarted | ThreadState.Stopped)) == 0)
						//if (m_Thread.ThreadState == ThreadState.Running || m_Thread.ThreadState == ThreadState.WaitSleepJoin || m_Thread.ThreadState == ThreadState.AbortRequested)
							return true;
						else
							return false;
					}
				}
			}

			internal bool IsStopped
			{
				get
				{
					lock (m_Lock)
					{
						if ( (m_Thread.ThreadState & ThreadState.Stopped) != 0 || (m_Thread.ThreadState & ThreadState.Aborted) != 0 )
							//if (m_Thread.ThreadState == ThreadState.Stopped || m_Thread.ThreadState == ThreadState.Aborted)
							return true;
						else
							return false;
					}
				}
			}

			internal bool IsUnstarted
			{
				get
				{
					lock (m_Lock)
					{
						if ((m_Thread.ThreadState & ThreadState.Unstarted) != 0)
					//	if (m_Thread.ThreadState == ThreadState.Unstarted)
							return true;
						else
							return false;
					}
				}
			}

			private bool m_StopMessage = false;
			internal bool StopMessage
			{
				get { return m_StopMessage; }
				set { m_StopMessage = value; }
			}

			private bool m_StartMessage = true;
			internal bool StartMessage
			{
				get { return m_StartMessage; }
				set { m_StartMessage = value; }
			}

			private ScriptEngine m_Engine;
			private ScriptScope m_Scope;
			private ScriptSource m_Source;

			internal EnhancedScript(string filename, string text, bool wait, bool loop, bool run, bool autostart)
			{
				m_Filename = filename;
				m_Text = text;
				m_Wait = wait;
				m_Loop = loop;
				m_Run = run;
				m_AutoStart = autostart;
				m_Thread = new Thread(AsyncStart);
			}
		}

		internal class ScriptTimer
		{
			private System.Threading.Timer m_Timer;

			private Thread m_AutoLootThread;
			private Thread m_ScavengerThread;
			private Thread m_BandageHealThread;
			private Thread m_AutoCarverThread;
			private Thread m_BoneCutterThread;
			private Thread m_DragDropThread;
			private Thread m_AutoRemountThread;

			internal ScriptTimer()
			{
			}

			public void Start()
			{
				m_Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTick), null, m_TimerDelay, m_TimerDelay);
			}

			public void Stop()
			{
                m_Timer.Change(Timeout.Infinite, Timeout.Infinite);

				foreach (EnhancedScript script in m_EnhancedScripts.Values.ToList())
				{
					if (script.IsRunning)
					{
						script.Stop();
					}
				}
			}

			private bool IsRunningThread(Thread thread)
			{
				if (thread == null)
					return false;

				if (thread != null && ( (thread.ThreadState & ThreadState.Running) != 0 || (thread.ThreadState & ThreadState.WaitSleepJoin) != 0 || (thread.ThreadState & ThreadState.AbortRequested) != 0 ))
					return true;
				else
					return false;
			}

			internal void Close()
			{
				try
				{
					if (IsRunningThread(m_AutoLootThread))
					{
						m_AutoLootThread.Abort();
					}

					if (IsRunningThread(m_ScavengerThread))
					{
						m_ScavengerThread.Abort();
					}

					if (IsRunningThread(m_BandageHealThread))
					{
						m_BandageHealThread.Abort();
					}

					if (IsRunningThread(m_DragDropThread))
					{
						m_DragDropThread.Abort();
					}

					if (IsRunningThread(m_AutoCarverThread))
					{
						m_AutoCarverThread.Abort();
					}

					if (IsRunningThread(m_AutoRemountThread))
					{
						m_AutoRemountThread.Abort();
					}

					UI.EnhancedScriptEditor.End();

					this.Stop();
				}
				catch
				{ }
			}

			private void OnTick(object state)
			{
				foreach (EnhancedScript script in m_EnhancedScripts.Values.ToList())
				{
					if (script.Run)
					{
						if (ScriptStartStopMessage && script.StartMessage)
						{
							Misc.SendMessage("Script <" + script.Filename + "> started.", 70, false);
							script.StartMessage = false;
							script.StopMessage = true;
						}

						if (script.Loop)
						{
							if (script.IsStopped)
								script.Reset();

							if (script.IsUnstarted)
								script.Start();
						}
						else
						{
							if (script.IsStopped)
								script.Reset();
							else if (script.IsUnstarted)
								script.Start();
						}
					}
					else
					{
						if (ScriptStartStopMessage && script.StopMessage)
						{
							Misc.SendMessage("Script <" + script.Filename + "> stopped.", 70, false);
							script.StartMessage = true;
							script.StopMessage = false;
						}

						if (script.IsRunning)
							script.Stop();

						if (script.IsStopped)
							script.Reset();
					}
				}

				if (World.Player != null && Client.Running) // Parte agent
				{
					if (AutoLoot.AutoMode && !IsRunningThread(m_AutoLootThread))
					{
						try
						{
							m_AutoLootThread = new Thread(AutoLoot.AutoRun);
							m_AutoLootThread.Start();
						}
						catch { }
					}

					if (Scavenger.AutoMode && !IsRunningThread(m_ScavengerThread))
					{
						try
						{
							m_ScavengerThread = new Thread(Scavenger.AutoRun);
							m_ScavengerThread.Start();
						}
						catch { }
					}

					if (BandageHeal.AutoMode && !IsRunningThread(m_BandageHealThread))
					{
						try
						{
							m_BandageHealThread = new Thread(BandageHeal.AutoRun);
							m_BandageHealThread.Start();
						}
						catch { }
					}

					if ((Scavenger.AutoMode || AutoLoot.AutoMode || Filters.AutoCarver) && !IsRunningThread(m_DragDropThread))
					{
						try
						{
							m_DragDropThread = new Thread(DragDropManager.AutoRun);
							m_DragDropThread.Start();
						}
						catch { }
					}

					if (Filters.AutoCarver && !IsRunningThread(m_AutoCarverThread))
					{
						try
						{
							m_AutoCarverThread = new Thread(Filters.CarveAutoRun);
							m_AutoCarverThread.Start();
						}
						catch { }
					}

					if (Filters.BoneCutter && !IsRunningThread(m_BoneCutterThread))
					{
						try
						{
							m_BoneCutterThread = new Thread(Filters.BoneCutterRun);
							m_BoneCutterThread.Start();
						}
						catch { }
					}

					if (Filters.AutoModeRemount && !IsRunningThread(m_AutoRemountThread))
					{
						try
						{
							m_AutoRemountThread = new Thread(Filters.RemountAutoRun);
							m_AutoRemountThread.Start();
						}
						catch { }
					}
				}
			}
		}

		private static TimeSpan m_TimerDelay = TimeSpan.FromMilliseconds(100);

		internal static TimeSpan TimerDelay
		{
			get { return m_TimerDelay; }
			set
			{
				if (value != m_TimerDelay)
				{
					m_TimerDelay = value;
					Init();
				}
			}
		}

		private static ScriptTimer m_Timer = new ScriptTimer();
		internal static ScriptTimer Timer { get { return m_Timer; } }

		private static ConcurrentDictionary<string, EnhancedScript> m_EnhancedScripts = new ConcurrentDictionary<string, EnhancedScript>();
		internal static ConcurrentDictionary<string, EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

		public static void Initialize()
		{
			m_Timer.Start();
		}

		internal static void Init()
		{
			if (m_Timer != null)
				m_Timer.Stop();

			m_Timer = new ScriptTimer();
			m_Timer.Start();
		}

		internal static ScriptScope GetRazorScope(ScriptEngine engine)
		{
			Dictionary<string, object> globals = new Dictionary<string, object>();

			ScriptScope scope = engine.CreateScope(globals);
			scope.SetVariable("Misc", new RazorEnhanced.Misc());
			scope.SetVariable("Items", new RazorEnhanced.Items());
			scope.SetVariable("Mobiles", new RazorEnhanced.Mobiles());
			scope.SetVariable("Player", new RazorEnhanced.Player());
			scope.SetVariable("Spells", new RazorEnhanced.Spells());
			scope.SetVariable("Gumps", new RazorEnhanced.Gumps());
			scope.SetVariable("Journal", new RazorEnhanced.Journal());
			scope.SetVariable("Target", new RazorEnhanced.Target());
			scope.SetVariable("Statics", new RazorEnhanced.Statics());

			scope.SetVariable("AutoLoot", new RazorEnhanced.AutoLoot());
			scope.SetVariable("Scavenger", new RazorEnhanced.Scavenger());
			scope.SetVariable("SellAgent", new RazorEnhanced.SellAgent());
			scope.SetVariable("BuyAgent", new RazorEnhanced.BuyAgent());
			scope.SetVariable("Organizer", new RazorEnhanced.Organizer());
			scope.SetVariable("Dress", new RazorEnhanced.Dress());
			scope.SetVariable("Friend", new RazorEnhanced.Friend());
			scope.SetVariable("Restock", new RazorEnhanced.Restock());
			scope.SetVariable("BandageHeal", new RazorEnhanced.BandageHeal());
			scope.SetVariable("PathFinding", new RazorEnhanced.PathFinding());
			scope.SetVariable("DPSMeter", new RazorEnhanced.DPSMeter());
			scope.SetVariable("Timer", new RazorEnhanced.Timer());
			return scope;
		}

		internal static EnhancedScript Search(string filename)
		{
			foreach (KeyValuePair<string, EnhancedScript> pair in m_EnhancedScripts)
			{
				if (pair.Key.ToLower() == filename.ToLower())
					return pair.Value;
			}

			return null;
		}

		// Autostart
		internal static void AutoStart()
		{
			foreach (EnhancedScript script in m_EnhancedScripts.Values.ToList())
			{
				if (!script.IsRunning && script.AutoStart)
					script.Start();
			}
		}
	}
}
