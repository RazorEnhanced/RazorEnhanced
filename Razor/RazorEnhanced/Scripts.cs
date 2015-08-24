using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime.Exceptions;

using Assistant;
using IronPython.Runtime;

namespace RazorEnhanced
{
	internal class Scripts
	{
		internal class EnhancedScript
		{
			internal void Start()
			{
				if (m_Thread == null ||
					(m_Thread != null && m_Thread.ThreadState != ThreadState.Running &&
					m_Thread.ThreadState != ThreadState.Unstarted &&
					m_Thread.ThreadState != ThreadState.WaitSleepJoin)
				)
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
				if (m_Source != null && m_Source != null)
				{
					try
					{
						m_Source.Execute(m_Scope);
					}
					catch 
					{
						Scripts.Auto = false;
						Assistant.Engine.MainWindow.SetCheckBoxAutoMode(false);
					}
				}
			}

			internal void Stop()
			{
				if (m_Thread != null && m_Thread.ThreadState != ThreadState.Stopped)
				{
					m_Thread.Abort();
				}
			}

			internal string Create(TracebackDelegate traceFunc)
			{
				string result = "Created";
				try
				{
					m_Engine = Python.CreateEngine();
					m_Source = m_Engine.CreateScriptSourceFromString(m_Text);
					m_Scope = GetRazorScope(m_Engine);

					if (traceFunc != null)
						m_Engine.SetTrace(traceFunc);
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

			private TimeSpan m_Delay;
			internal TimeSpan Delay { get { return m_Delay; } }

			private Thread m_Thread;
			private ScriptEngine m_Engine;
			private ScriptScope m_Scope;
			private ScriptSource m_Source;

			internal EnhancedScript(string filename, string text, TimeSpan delay)
			{
				m_Filename = filename;
				m_Text = text;
				m_Delay = delay;
			}
		}

		internal class ScriptTimer : Assistant.Timer
		{
			private Thread m_AutoLootThread;
			private Thread m_ScavengerThread;
            private Thread m_BandageHealThread;
            private Thread m_AutoCarverThread;
            private Thread m_DragDropThread;

			internal ScriptTimer()
				: base(m_TimerDelay, m_TimerDelay)
			{
			}

			protected override void OnTick()
			{
				if (Scripts.Auto)
				{
					foreach (EnhancedScript script in m_EnhancedScripts)
					{
						script.Start();
					}
				}


                if (AutoLoot.AutoMode && World.Player != null)
				{
                    Thread.Sleep(5);
					if (m_AutoLootThread == null ||
						(m_AutoLootThread != null && m_AutoLootThread.ThreadState != ThreadState.Running &&
						m_AutoLootThread.ThreadState != ThreadState.Unstarted &&
						m_AutoLootThread.ThreadState != ThreadState.WaitSleepJoin)
					)
					{
                        try
                        {
                            m_AutoLootThread = new Thread(AutoLoot.Engine);
                            m_AutoLootThread.Start();
                        }
                        catch
                        {
                            AutoLoot.AddLog("Error in AutoLoot Thread, Restart");
                        }
					}   
				}

                if (Scavenger.AutoMode && World.Player != null)
				{
                    Thread.Sleep(5);
					if (m_ScavengerThread == null ||
						(m_ScavengerThread != null && m_ScavengerThread.ThreadState != ThreadState.Running &&
						m_ScavengerThread.ThreadState != ThreadState.Unstarted &&
						m_ScavengerThread.ThreadState != ThreadState.WaitSleepJoin)
					)
					{
                        try
                        {
                            m_ScavengerThread = new Thread(Scavenger.Engine);
                            m_ScavengerThread.Start();
                        }
                        catch
                        {
                            Scavenger.AddLog("Error in Scaveger Thread, Restart");
                        }
					}
				}

                if (BandageHeal.AutoMode && World.Player != null)
                {
                    Thread.Sleep(5);
                    if (m_BandageHealThread == null ||
                        (m_BandageHealThread != null && m_BandageHealThread.ThreadState != ThreadState.Running &&
                        m_BandageHealThread.ThreadState != ThreadState.Unstarted &&
                        m_BandageHealThread.ThreadState != ThreadState.WaitSleepJoin)
                    )
                    {
                        try
                        {
                            m_BandageHealThread = new Thread(BandageHeal.Engine);
                            m_BandageHealThread.Start();
                        }
                        catch
                        {
                            BandageHeal.AddLog("Error in BandageHeal Thread, Restart");
                        }
                    }
                }

                if (Filters.AutoCarver && World.Player != null)
                {
                    Thread.Sleep(5);
                    if (m_AutoCarverThread == null ||
                        (m_AutoCarverThread != null && m_AutoCarverThread.ThreadState != ThreadState.Running &&
                        m_AutoCarverThread.ThreadState != ThreadState.Unstarted &&
                        m_AutoCarverThread.ThreadState != ThreadState.WaitSleepJoin)
                    )
                    {
                        try
                        {
                            m_AutoCarverThread = new Thread(Filters.AutoCarverEngine);
                            m_AutoCarverThread.Start();
                        }
                        catch
                        { }
                    }
                }

                if (World.Player != null && (Scavenger.AutoMode || AutoLoot.AutoMode))
                {
                    if (m_DragDropThread == null ||
                           (m_DragDropThread != null && m_DragDropThread.ThreadState != ThreadState.Running &&
                           m_DragDropThread.ThreadState != ThreadState.Unstarted &&
                           m_DragDropThread.ThreadState != ThreadState.WaitSleepJoin)
                       )
                    {
                        try
                        {
                            m_DragDropThread = new Thread(DragDropManager.Engine);
                            m_DragDropThread.Start();
                        }
                        catch
                        { }
                    }
                }
			}
		}

		internal static TimeSpan m_TimerDelay = TimeSpan.FromMilliseconds(100);

		internal static ScriptTimer m_Timer = new ScriptTimer();

		private static List<EnhancedScript> m_EnhancedScripts = new List<EnhancedScript>();
		internal static List<EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

		public static void Initialize()
		{
			m_Timer.Start();
		}

		private static bool m_Auto = false;
		internal static bool Auto
		{
			get { return m_Auto; }
			set
			{
				if (m_Auto == value)
					return;

				m_Auto = value;

				if (!m_Auto)
				{
					foreach (EnhancedScript script in m_EnhancedScripts)
					{
						script.Stop();
					}
				}
			}
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

		internal static void Reset()
		{
			m_EnhancedScripts.Clear();
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

		internal static string LoadFromFile(string filename, TimeSpan delay)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(filename);
			string text = File.ReadAllText(filename);

			EnhancedScript script = new EnhancedScript(filename, text, delay);
			string result = script.Create(null);

			if (result == "Created")
			{
				m_EnhancedScripts.Add(script);
			}
			else
			{
				status = "ERROR: " + result;
			}

			return status;
		}
	}

    // Blocco HotKey
    internal class RunningThreads
    {
        private string m_Filename;
        internal string Filename { 
            get { return m_Filename; }
            set { m_Filename = value; }
        }

        private Thread m_Thread;
        internal Thread Thread
        {
            get { return m_Thread; }
            set { m_Thread = value; }
        }

        internal RunningThreads(string filename, Thread thread)
		{
			m_Filename = filename;
            m_Thread = thread;
		}

    }

    internal class EnhancedScriptHotKey
    {
        internal static List<RunningThreads> RunningThreadsList = new List<RunningThreads>();


        internal static void HotKeyStart(string filename)
        {
            Thread m_Thread;
            bool m_run = true;
            Thread m_oldthread = null;
            foreach (RunningThreads t in RunningThreadsList)
            {
                if (t.Filename == filename)
                {
                    m_run = false;
                    m_oldthread = t.Thread;
                    break;
                }
            }

            if (m_run)
            {
                m_Thread = new Thread(new ParameterizedThreadStart(HotKeyStartThread));
                m_Thread.Start(filename);
                RunningThreadsList.Add(new RunningThreads(filename, m_Thread));
            }
            else
            {
                if (m_oldthread != null && m_oldthread.ThreadState != ThreadState.Stopped)
                {
                    m_oldthread.Abort();
                    HotKeyScriptDone(filename);
                    m_Thread = new Thread(new ParameterizedThreadStart(HotKeyStartThread));
                    m_Thread.Start(filename);
                    RunningThreadsList.Add(new RunningThreads(filename, m_Thread));
                }
            }

        }

        private static void HotKeyStartThread(object parameter)
        {
            ScriptEngine m_Engine;
            ScriptScope m_Scope;
            ScriptSource m_Source;
            try
            {
                m_Engine = Python.CreateEngine();
                m_Source = m_Engine.CreateScriptSourceFromString(File.ReadAllText((string)parameter));
                m_Scope = Scripts.GetRazorScope(m_Engine);
                m_Source.Execute(m_Scope);
                HotKeyScriptDone((string)parameter);
            }
            catch { }
        }
        private static void HotKeyScriptDone(string filename)
        {
            int i = 0;
            foreach (RunningThreads t in RunningThreadsList)
            {
                if (t.Filename == filename)
                {
                    break;
                }
                i++;
            }
            try
            {
                RunningThreadsList.RemoveAt(i);
            }
            catch { }
        }

        internal static void HotKeyStopAll()
        {
            foreach (RunningThreads t in RunningThreadsList)
            {
                if (t.Thread != null && t.Thread.ThreadState != ThreadState.Stopped)
                {
                    t.Thread.Abort();
                }
            }
            RunningThreadsList.Clear();
        }
    }
}
