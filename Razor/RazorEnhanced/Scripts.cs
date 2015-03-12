using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

using Assistant;

namespace RazorEnhanced
{
	internal class Scripts
	{
		internal class EnhancedScript
		{
			internal void Start()
			{
				if (m_Thread == null || (m_Thread != null && m_Thread.ThreadState != ThreadState.Running && m_Thread.ThreadState != ThreadState.Unstarted && m_Thread.ThreadState != ThreadState.WaitSleepJoin))
				{
					ThreadState s;
					if (m_Thread != null)
						s = m_Thread.ThreadState;

					m_Thread = new Thread(AsyncRun);
					m_Thread.Start();
					while (!m_Thread.IsAlive)
					{
					}
				}
			}

			private void AsyncRun()
			{
				int exit = Int32.MinValue;
				exit = InvokeMethod<int>("Run");

				if (exit != 0)
				{
					Scripts.Auto = false;
					Assistant.Engine.MainWindow.SetCheckBoxAutoMode(false);
					Assistant.World.Player.SendMessage(LocString.EnhancedMacroError, exit);
				}
			}

			internal void Stop()
			{
				if (m_Thread != null)
				{
					m_Thread.Abort();
				}
			}

			private T InvokeMethod<T>(string method)
			{
				T result = default(T);
				try
				{
					m_Engine = Python.CreateEngine();
					ScriptSource source = m_Engine.CreateScriptSourceFromString(m_Text, Microsoft.Scripting.SourceCodeKind.Statements);
					ScriptScope scope = m_Engine.CreateScope();
					SetVariables(scope);
					source.Execute(scope);
					Func<T> Run = scope.GetVariable<Func<T>>("Run");
					result = Run();
				}
				catch
				{
				}

				return result;
			}

			private void SetVariables(ScriptScope scope)
			{
				scope.SetVariable("SendMessage", new Action<string>(Misc.SendMessage));
				scope.SetVariable("Pause", new Action<double>(Misc.Pause));

			}

			private string m_Text;
			internal string Text { get { return m_Text; } }

			private string m_Class;
			internal string Class { get { return m_Class; } }

			private TimeSpan m_Delay;
			internal TimeSpan Delay { get { return m_Delay; } }

			private Thread m_Thread;
			private ScriptEngine m_Engine;

			internal EnhancedScript(string text, string classname, TimeSpan delay, bool auto)
			{
				m_Text = text;
				m_Delay = delay;
				m_Class = classname;
			}
		}

		internal class ScriptTimer : Assistant.Timer
		{
			private Task<int> m_AutoLootTask;
			private Item.Filter m_CorpseFilter;

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
					if (m_AutoLootTask == null || (m_AutoLootTask != null && m_AutoLootTask.Status == TaskStatus.RanToCompletion))
					{
						m_AutoLootTask = new Task<int>(AutoLoot.Run);
						m_AutoLootTask.Start();
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

		internal static void Reset()
		{
			m_EnhancedScripts.Clear();
		}

		internal static EnhancedScript Search(string classname)
		{
			foreach (EnhancedScript script in m_EnhancedScripts)
			{
				if (script.Class == classname)
					return script;
			}
			return null;
		}

		internal static string LoadFromFile(string filename, TimeSpan delay)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(filename);
			string text = File.ReadAllText(filename);

			EnhancedScript script = new EnhancedScript(text, classname, delay, false);

			try
			{
				m_EnhancedScripts.Add(script);
			}
			catch (Exception ex)
			{
				status = "ERROR: " + ex.Message;
			}

			return status;
		}

		internal static string LoadFromText(string text, string classname, TimeSpan delay)
		{
			string status = "Loaded";

			EnhancedScript script = new EnhancedScript(text, classname, delay, false);

			try
			{
				m_EnhancedScripts.Add(script);
			}
			catch (Exception ex)
			{
				status = "ERROR: " + ex.Message;
			}

			return status;
		}
	}
}
