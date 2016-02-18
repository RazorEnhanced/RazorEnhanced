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
		internal enum RunMode
		{
			None,
			RunOnce,
			Loop,
		}

		internal class EnhancedScript
		{
			internal void Start()
			{
				if (!IsRunning && IsUnstarted)
				{
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
			}

			private void AsyncStart()
			{
				if (m_Source != null)
				{
					try
					{
						m_Source.Execute(m_Scope);
					}
					catch (Exception ex)
					{
					}
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
				string result = "";
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
						if (m_Thread.ThreadState == ThreadState.Running || m_Thread.ThreadState == ThreadState.WaitSleepJoin || m_Thread.ThreadState == ThreadState.AbortRequested)
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
						if (m_Thread.ThreadState == ThreadState.Stopped || m_Thread.ThreadState == ThreadState.Aborted)
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

			internal EnhancedScript(string filename, string text, bool wait, bool loop, bool run)
			{
				m_Filename = filename;
				m_Text = text;
				m_Wait = wait;
				m_Loop = loop;
				m_Run = run;

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

				if (thread != null && (thread.ThreadState == ThreadState.Running || /*thread.ThreadState == ThreadState.Unstarted ||*/ thread.ThreadState == ThreadState.WaitSleepJoin))
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
								script.Start();
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
								script.Start();
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

				if (AutoLoot.AutoMode && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_AutoLootThread))
				{
					try
					{
						m_AutoLootThread = new Thread(AutoLoot.AutoRun);
						m_AutoLootThread.Start();
					}
					catch (Exception ex)
					{
						AutoLoot.AddLog("Error in AutoLoot Thread, Restart");
					}
				}

				if (Scavenger.AutoMode && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_ScavengerThread))
				{
					try
					{
						m_ScavengerThread = new Thread(Scavenger.AutoRun);
						m_ScavengerThread.Start();
					}
					catch (Exception ex)
					{
						Scavenger.AddLog("Error in Scaveger Thread, Restart");
					}
				}

				if (BandageHeal.AutoMode && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_BandageHealThread))
				{
					try
					{
						m_BandageHealThread = new Thread(BandageHeal.AutoRun);
						m_BandageHealThread.Start();
					}
					catch (Exception ex)
					{
						BandageHeal.AddLog("Error in BandageHeal Thread, Restart");
					}
				}

				if (World.Player != null && (Scavenger.AutoMode || AutoLoot.AutoMode || Filters.AutoCarver) && Assistant.Engine.Running && !IsRunningThread(m_DragDropThread))
				{
					try
					{
						m_DragDropThread = new Thread(DragDropManager.AutoRun);
						m_DragDropThread.Start();
					}
					catch (Exception ex)
					{
					}
				}

				if (Filters.AutoCarver && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_AutoCarverThread))
				{
					try
					{
						m_AutoCarverThread = new Thread(Filters.CarveAutoRun);
						m_AutoCarverThread.Start();
					}
					catch (Exception ex)
					{
					}
				}

				if (Filters.BoneCutter && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_BoneCutterThread))
				{
					try
					{
						m_BoneCutterThread = new Thread(Filters.BoneCutterRun);
						m_BoneCutterThread.Start();
					}
					catch (Exception ex)
					{
					}
				}

				if (Filters.AutoModeRemount && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_AutoRemountThread))
				{
					try
					{
						m_AutoRemountThread = new Thread(Filters.RemountAutoRun);
						m_AutoRemountThread.Start();
					}
					catch (Exception ex)
					{
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

			scope.SetVariable("AutoLoot", new RazorEnhanced.AutoLoot());
			scope.SetVariable("Scavenger", new RazorEnhanced.Scavenger());
			scope.SetVariable("SellAgent", new RazorEnhanced.SellAgent());
			scope.SetVariable("BuyAgent", new RazorEnhanced.BuyAgent());
			scope.SetVariable("Organizer", new RazorEnhanced.Organizer());
			scope.SetVariable("Dress", new RazorEnhanced.Dress());
			scope.SetVariable("Friend", new RazorEnhanced.Friend());
			scope.SetVariable("Restock", new RazorEnhanced.Restock());
			scope.SetVariable("BandageHeal", new RazorEnhanced.BandageHeal());

			return scope;
		}

		internal static EnhancedScript Search(string filename)
		{
			foreach (KeyValuePair<string, EnhancedScript> pair in m_EnhancedScripts)
			{ 
				if (pair.Key == filename)
					return pair.Value;
			}

			return null;
		}
	}
}