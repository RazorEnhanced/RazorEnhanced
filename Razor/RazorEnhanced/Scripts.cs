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
					catch (Exception ex)
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

				if (AutoLoot.Auto)
				{
					if (m_AutoLootThread == null ||
						(m_AutoLootThread != null && m_AutoLootThread.ThreadState != ThreadState.Running &&
						m_AutoLootThread.ThreadState != ThreadState.Unstarted &&
						m_AutoLootThread.ThreadState != ThreadState.WaitSleepJoin)
					)
					{
						m_AutoLootThread = new Thread(AutoLoot.Run);
						m_AutoLootThread.Start();
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
			scope.SetVariable("AutoLoot", new RazorEnhanced.AutoLoot());
			scope.SetVariable("Items", new RazorEnhanced.Items());
			scope.SetVariable("Mobiles", new RazorEnhanced.Mobiles());
			scope.SetVariable("Player", new RazorEnhanced.Player());
			scope.SetVariable("Scavenger", new RazorEnhanced.Scavenger());

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
}
