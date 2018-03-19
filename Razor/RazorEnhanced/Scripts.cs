using Assistant;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace RazorEnhanced
{
	internal class Scripts
	{
		internal static Thread ScriptEditorThread;

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

			if (Settings.General.ReadBool("ShowScriptMessageCheckBox"))
				ClientCommunication.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 945, 3, Language.CliLocName, "System", msg.ToString()));
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

				try
				{
					m_Source.Execute(m_Scope);
				}
				catch
				{
				}
			}

			internal void Stop()
			{
				if (!IsStopped)
					m_Thread.Abort();
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
					m_Engine = Python.CreateEngine();
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
						if ( (m_Thread.ThreadState & ThreadState.Running) != 0 || (m_Thread.ThreadState & ThreadState.WaitSleepJoin) != 0 || (m_Thread.ThreadState & ThreadState.AbortRequested) != 0 )
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
						if (m_Thread.ThreadState == ThreadState.Unstarted)
							return true;
						else
							return false;
					}
				}
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

			internal static ConcurrentDictionary<string, object> SharedScriptData = new ConcurrentDictionary<string, object>();
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

			private void OnTick(object state)
			{
				foreach (EnhancedScript script in m_EnhancedScripts.Values.ToList())
				{
					if (script.Run)
					{
						if (script.Loop)
						{
							if (script.IsStopped)
							{
								script.Reset();
							}

							if (script.IsUnstarted)
							{
								try
								{
									script.Start();
								}
								catch { }
							}
						}
						else
						{
							if (script.IsStopped)
							{
								script.Reset();
							}
							else if (script.IsUnstarted)
							{
								try
								{
									script.Start();
								}
								catch { }
							}
						}
					}
					else
					{
						if (script.IsRunning)
						{
							script.Stop();
						}

						if (script.IsStopped)
						{
							script.Reset();
						}
					}
				}

				if (World.Player != null && Assistant.Engine.Running) // Parte agent 
				{
					if (AutoLoot.AutoMode && !IsRunningThread(m_AutoLootThread))
					{
						m_AutoLootThread = new Thread(AutoLoot.AutoRun);
						m_AutoLootThread.Start();
					}

					if (Scavenger.AutoMode && !IsRunningThread(m_ScavengerThread))
					{
						m_ScavengerThread = new Thread(Scavenger.AutoRun);
						m_ScavengerThread.Start();
					}

					if (BandageHeal.AutoMode && !IsRunningThread(m_BandageHealThread))
					{
						m_BandageHealThread = new Thread(BandageHeal.AutoRun);
						m_BandageHealThread.Start();
					}

					if ((Scavenger.AutoMode || AutoLoot.AutoMode || Filters.AutoCarver) && !IsRunningThread(m_DragDropThread))
					{
						m_DragDropThread = new Thread(DragDropManager.AutoRun);
						m_DragDropThread.Start();
					}

					if (Filters.AutoCarver && !IsRunningThread(m_AutoCarverThread))
					{
						m_AutoCarverThread = new Thread(Filters.CarveAutoRun);
						m_AutoCarverThread.Start();
					}

					if (Filters.BoneCutter && !IsRunningThread(m_BoneCutterThread))
					{
						m_BoneCutterThread = new Thread(Filters.BoneCutterRun);
						m_BoneCutterThread.Start();
					}

					if (Filters.AutoModeRemount && !IsRunningThread(m_AutoRemountThread))
					{
						m_AutoRemountThread = new Thread(Filters.RemountAutoRun);
						m_AutoRemountThread.Start();
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