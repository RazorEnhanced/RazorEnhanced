using Assistant;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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
				if (!(this.State == ThreadState.Running || this.State == ThreadState.WaitSleepJoin))
				{
					m_Thread = new Thread(AsyncStart);
					m_Thread.Start();
					while (!m_Thread.IsAlive)
					{
					}
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
					catch
					{
					}
				}
			}

			internal void Stop()
			{
				if (m_Thread.ThreadState != ThreadState.Stopped)
				{
					m_Thread.Abort();
				}

				m_Thread = new Thread(AsyncStart);
				m_Run = false;
				Engine.MainWindow.UpdateScriptGrid(m_Filename, false);
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
			internal string Filename { get { return m_Filename; } }

			private string m_Text;
			internal string Text { get { return m_Text; } }

			private Thread m_Thread;

			private volatile bool m_Wait;
			internal bool Wait { get { return m_Wait; } }

			private volatile bool m_Loop;
			internal bool Loop
			{
				get { return m_Loop; }
				set { m_Loop = value; }
			}

			private volatile bool m_Run;
			internal bool Run
			{
				get { return m_Run; }
				set { m_Run = value; }
			}

			private object m_Lock = new object();

			internal ThreadState State
			{
				get
				{
					lock (m_Lock)
					{
						return m_Thread.ThreadState;
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
		}

		internal class ScriptTimer : Assistant.Timer
		{
			private Thread m_AutoLootThread;
			private Thread m_ScavengerThread;
			private Thread m_BandageHealThread;
			private Thread m_AutoCarverThread;
			private Thread m_DragDropThread;
			private Thread m_AutoRemountThread;

			internal ScriptTimer()
				: base(m_TimerDelay, m_TimerDelay)
			{
			}

			private bool IsRunningThread(Thread thread)
			{
				if (thread == null)
					return false;

				if (thread != null && (thread.ThreadState == ThreadState.Running || thread.ThreadState == ThreadState.Unstarted || thread.ThreadState == ThreadState.WaitSleepJoin))
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

			protected override void OnTick()
			{
				foreach (EnhancedScript script in m_EnhancedScripts)
				{
					if (script.Run)
					{
						if (script.Loop)
						{
							if (script.State == ThreadState.Stopped)
							{
								script.Start();
							}
							else if (!(script.State == ThreadState.Running || script.State == ThreadState.WaitSleepJoin))
							{
								script.Start();
							}
						}
						else
						{
							if (script.State == ThreadState.Stopped)
							{
								script.Stop();
							}
							else if (!(script.State == ThreadState.Running || script.State == ThreadState.WaitSleepJoin))
							{
								script.Start();
							}
						}
					}
					else
					{
						script.Stop();
					}
				}

				if (AutoLoot.AutoMode && World.Player != null && Assistant.Engine.Running && !IsRunningThread(m_AutoLootThread))
				{
					try
					{ 
						m_AutoLootThread = new Thread(AutoLoot.AutoRun);
						m_AutoLootThread.Start();
					}
					catch
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
					catch
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
					catch
					{
						BandageHeal.AddLog("Error in BandageHeal Thread, Restart");
					}
				}

				if (World.Player != null && (Scavenger.AutoMode || AutoLoot.AutoMode) && Assistant.Engine.Running && !IsRunningThread(m_DragDropThread))
				{
					try
					{
						m_DragDropThread = new Thread(DragDropManager.AutoRun);
						m_DragDropThread.Start();
					}
					catch
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
					catch
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
					catch
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

		private static ConcurrentStack<EnhancedScript> m_EnhancedScripts = new ConcurrentStack<EnhancedScript>();
		internal static ConcurrentStack<EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

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
			scope.SetVariable("Pets", new RazorEnhanced.Pets());

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
			foreach (EnhancedScript script in m_EnhancedScripts)
			{
				if (script.Filename == filename)
					return script;
			}
			return null;
		}
	}
}