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
		internal class EnhancedScript
		{
			internal int Execute(RunMode action)
			{
				int exit = Int32.MinValue;

				RunMethod("Run", action);

				return exit;
			}

			private T InvokeMethod<T>(string method, RunMode action)
			{
				T result = default(T);
				try
				{
					result = (T)m_PaxScripter.Invoke(action, null, "RazorEnhanced." + m_Class + "." + method);
					m_LineNUmber = m_PaxScripter.CurrentLineNumber;
				}
				catch
				{
				}

				return result;
			}

			private void RunMethod(string method, RunMode action)
			{
				try
				{
					m_PaxScripter.Run(action, null, "RazorEnhanced." + m_Class + "." + method);
					m_LineNUmber = m_PaxScripter.CurrentLineNumber;
				}
				catch
				{
				}

			}

			private int m_LineNUmber;
			internal int LineNumber { get { return m_LineNUmber; } }

			private string m_Text;
			internal string Text { get { return m_Text; } }

			private string m_Class;
			internal string Class { get { return m_Class; } }

			private TimeSpan m_Delay;
			internal TimeSpan Delay { get { return m_Delay; } }

			internal EnhancedScript(string text, string classname, TimeSpan delay, bool auto)
			{
				m_Text = text;
				m_Delay = delay;
				m_Class = classname;
			}
		}

		private static List<EnhancedScript> m_EnhancedScripts = new List<EnhancedScript>();
		internal static List<EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

		private static PaxScripter m_PaxScripter;
		internal static PaxScripter Engine { get { return m_PaxScripter; } }

		public static void Initialize()
		{
			m_PaxScripter = new PaxScripter();
			m_PaxScripter.OnChangeState += new ChangeStateHandler(paxScripter_OnChangeState);
		}

		private static bool m_AutoMode = false;
		internal static bool AutoMode
		{
			get { return m_AutoMode; }
			set
			{
				if (m_AutoMode == value)
					return;

				m_AutoMode = value;
				if (m_AutoMode)
					m_Timer.Start();
				else
					m_Timer.Stop();
			}
		}

		internal static TimeSpan m_TimerDelay = TimeSpan.FromMilliseconds(100);

		internal class ScriptTimer : Assistant.Timer
		{
			internal ScriptTimer()
				: base(m_TimerDelay, m_TimerDelay)
			{
			}

			protected override void OnTick()
			{
				foreach (EnhancedScript script in m_EnhancedScripts)
				{
					int exit = script.Execute(RunMode.Run);

					if (exit != 0)
					{
						Scripts.AutoMode = false;
						Assistant.Engine.MainWindow.SetCheckBoxAutoMode(false);
						Assistant.World.Player.SendMessage(LocString.EnhancedMacroError, exit);
					}
					else
						Thread.Sleep(script.Delay);
				}

				if (AutoLoot.Auto)
				{
					AutoLoot.Engine();
					Thread.Sleep(100);
				}
			}
		}

		internal static ScriptTimer m_Timer = new ScriptTimer();

		internal static void Reset()
		{
			AutoMode = false;
			m_EnhancedScripts.Clear();
			m_PaxScripter.Reset();
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
				m_PaxScripter.AddModule(script.Class);
				m_PaxScripter.AddCode(script.Class, script.Text);
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
				m_PaxScripter.AddModule(script.Class);
				m_PaxScripter.AddCode(script.Class, script.Text);
			}
			catch (Exception ex)
			{
				status = "ERROR: " + ex.Message;
			}

			return status;
		}

		private static void paxScripter_OnChangeState(PaxScripter sender, ChangeStateEventArgs e)
		{
			if (e.NewState == ScripterState.Error)
			{
				string msg = "SCRIPT ERRORS:" + Environment.NewLine;
				foreach (ScriptError err in m_PaxScripter.Error_List)
				{
					msg += "Line: " + err.LineNumber.ToString() + " - Error: " + err.Message + Environment.NewLine;
				}
				MessageBox.Show(msg);
			}
		}
	}
}
