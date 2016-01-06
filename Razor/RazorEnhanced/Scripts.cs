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
						Scripts.AutoMode = false;
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
			public Thread Thread { get { return m_Thread; } }

			private ScriptEngine m_Engine;
			private ScriptScope m_Scope;
			private ScriptSource m_Source;

			internal EnhancedScript(string filename, string text, TimeSpan delay)
			{
				m_Filename = filename;
				m_Text = text;
				m_Delay = delay;
			}

			internal static ConcurrentDictionary<string, object> SharedScriptData = new ConcurrentDictionary<string, object>();
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

			internal void Close()
			{
				AutoMode = false;

				if (
					m_AutoLootThread != null &&
					(m_AutoLootThread.ThreadState == ThreadState.Running ||
					m_AutoLootThread.ThreadState == ThreadState.Unstarted ||
					m_AutoLootThread.ThreadState == ThreadState.WaitSleepJoin)
				)
				{
					m_AutoLootThread.Abort();
				}

				if (
					m_ScavengerThread != null &&
					(m_ScavengerThread.ThreadState == ThreadState.Running ||
					m_ScavengerThread.ThreadState == ThreadState.Unstarted ||
					m_ScavengerThread.ThreadState == ThreadState.WaitSleepJoin)
				)
				{
					m_ScavengerThread.Abort();
				}

				if (
					m_BandageHealThread != null &&
					(m_BandageHealThread.ThreadState == ThreadState.Running ||
					m_BandageHealThread.ThreadState == ThreadState.Unstarted ||
					m_BandageHealThread.ThreadState == ThreadState.WaitSleepJoin)
				)
				{
					m_BandageHealThread.Abort();
				}

				if (
					m_DragDropThread != null &&
					(m_DragDropThread.ThreadState == ThreadState.Running ||
					m_DragDropThread.ThreadState == ThreadState.Unstarted ||
					m_DragDropThread.ThreadState == ThreadState.WaitSleepJoin)
				)
				{
					m_DragDropThread.Abort();
				}

				if (
					m_AutoCarverThread != null &&
					(m_AutoCarverThread.ThreadState == ThreadState.Running ||
					m_AutoCarverThread.ThreadState == ThreadState.Unstarted ||
					m_AutoCarverThread.ThreadState == ThreadState.WaitSleepJoin)
				)
				{
					m_AutoCarverThread.Abort();
				}

				if (
					m_AutoRemountThread != null &&
					(m_AutoRemountThread.ThreadState == ThreadState.Running ||
					m_AutoRemountThread.ThreadState == ThreadState.Unstarted ||
					m_AutoRemountThread.ThreadState == ThreadState.WaitSleepJoin)
				)
				{
					m_AutoRemountThread.Abort();
				}

				this.Stop();
			}

			protected override void OnTick()
			{
				Keys k = Keys.None;
				m_Keys.TryDequeue(out k);

				if (k != Keys.None)
				{
					string filename = RazorEnhanced.Settings.HotKey.FindScript(k);
					if (!m_EnhancedScripts.Any(s => s.Filename == filename))
					{
						string status = LoadFromFile(filename, TimeSpan.FromMilliseconds(100));
						if (status != "Loaded")
						{
							MessageBox.Show("Script not loaded!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							EnhancedScript script = Search(filename);
							if (script != null)
							{
								script.Start();
							}
						}
					}
				}

				Thread.Sleep(10);

				if (Scripts.AutoMode)
				{
					foreach (EnhancedScript script in m_EnhancedScripts)
					{
						script.Start();
					}
				}

				Thread.Sleep(10);

				foreach (EnhancedScript script in m_EnhancedScripts.ToArray())
				{
					if (script.Thread != null &&
						script.Thread.ThreadState != ThreadState.Running &&
						script.Thread.ThreadState != ThreadState.Unstarted &&
						script.Thread.ThreadState != ThreadState.WaitSleepJoin)
					{
						lock (m_Lock)
						{
							m_EnhancedScripts.Remove(script);
						}
					}

				}

				Thread.Sleep(5);

				if (AutoLoot.AutoMode && World.Player != null && Assistant.Engine.Running)
				{
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
						catch (Exception ex)
						{
							AutoLoot.AddLog("Error in AutoLoot Thread, Restart");
						}
					}
				}

				if (Scavenger.AutoMode && World.Player != null && Assistant.Engine.Running)
				{
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
						catch (Exception ex)
						{
							Scavenger.AddLog("Error in Scaveger Thread, Restart");
						}
					}
				}

				if (BandageHeal.AutoMode && World.Player != null && Assistant.Engine.Running)
				{
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
						catch (Exception ex)
						{
							BandageHeal.AddLog("Error in BandageHeal Thread, Restart");
						}
					}
				}

				if (World.Player != null && (Scavenger.AutoMode || AutoLoot.AutoMode) && Assistant.Engine.Running)
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
						catch (Exception ex)
						{
						}
					}
				}

				if (Filters.AutoCarver && World.Player != null && Assistant.Engine.Running)
				{
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
						catch (Exception ex)
						{
						}
					}
				}

				if (Filters.AutoModeRemount && World.Player != null && Assistant.Engine.Running)
				{
					if (m_AutoRemountThread == null ||
						(m_AutoRemountThread != null && m_AutoRemountThread.ThreadState != ThreadState.Running &&
						m_AutoRemountThread.ThreadState != ThreadState.Unstarted &&
						m_AutoRemountThread.ThreadState != ThreadState.WaitSleepJoin)
					)
					{
						try
						{
							m_AutoRemountThread = new Thread(Filters.AutoRemountEngine);
							m_AutoRemountThread.Start();
						}
						catch (Exception ex)
						{
						}
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

		private static List<EnhancedScript> m_EnhancedScripts = new List<EnhancedScript>();
		internal static List<EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

		private static ConcurrentQueue<Keys> m_Keys = new ConcurrentQueue<Keys>();

		private static object m_Lock = new object();

		public static void Initialize()
		{
			m_Timer.Start();
		}

		private static bool m_AutoMode = false;

		internal static void Init()
		{
			if (m_Timer != null)
				m_Timer.Stop();

			m_Timer = new ScriptTimer();
			m_Timer.Start();
		}

		internal static bool AutoMode
		{
			get { return m_AutoMode; }
			set
			{
				m_AutoMode = value;

				if (!m_AutoMode)
				{
					StopAll();
				}
				else
				{
					LoadAndInitializeScripts();
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
			lock (m_Lock)
			{
				m_EnhancedScripts.Clear();
			}
		}

		internal static void StopAll()
		{
			lock (m_Lock)
			{
				foreach (EnhancedScript script in m_EnhancedScripts.ToArray())
				{
					script.Stop();
				}
			}
		}

		internal static EnhancedScript Search(string filename)
		{
			foreach (EnhancedScript script in m_EnhancedScripts.ToArray())
			{
				if (script.Filename == filename)
					return script;
			}
			return null;
		}

		internal static void LoadAndInitializeScripts()
		{
			RazorEnhanced.Scripts.Reset();

			DataTable scriptTable = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];
			foreach (DataRow row in scriptTable.Rows)
			{
				if ((bool)row["Checked"])
				{
					string status = RazorEnhanced.Scripts.LoadFromFile((string)row["Filename"], TimeSpan.FromMilliseconds(100));
					if (status == "Loaded")
					{
						row["Flag"] = Assistant.Properties.Resources.green;
					}
					else
					{
						row["Flag"] = Assistant.Properties.Resources.red;
					}
					row["Status"] = status;
				}
				else
				{
					row["Flag"] = Assistant.Properties.Resources.yellow;
					row["Status"] = "Idle";
				}
			}
		}

		internal static string LoadFromFile(string filename, TimeSpan delay)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(filename);
			string text = null;

			if (File.Exists(filename))
				text = File.ReadAllText(filename);
			else
				return "ERROR: file not found";

			EnhancedScript script = new EnhancedScript(filename, text, delay);
			string result = script.Create(null);

			if (result == "Created")
			{
				lock (m_Lock)
				{
					m_EnhancedScripts.Add(script);
				}
			}
			else
			{
				status = "ERROR: " + result;
			}

			return status;
		}

		internal static void EnqueueKey(Keys k)
		{
			if (m_Keys != null)
			{
				m_Keys.Enqueue(k);
			}
		}
	}
}