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
				exit = InvokeMethod<int>("Run", action);
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

		internal class ScriptTimer : Assistant.Timer
		{
			RazorEnhanced.Item.Filter m_CorpseFilter;

			internal ScriptTimer()
				: base(m_TimerDelay, m_TimerDelay)
			{
				// Genero filtro per corpi
				m_CorpseFilter = new RazorEnhanced.Item.Filter();
				m_CorpseFilter.RangeMax = 3;
				m_CorpseFilter.Movable = false;
				m_CorpseFilter.IsCorpse = true;
				m_CorpseFilter.OnGround = true;
				m_CorpseFilter.Enabled = true;
			}

			protected override void OnTick()
			{
				if (Scripts.Auto)
				{
					foreach (EnhancedScript script in m_EnhancedScripts)
					{
						int exit = script.Execute(RunMode.Run);

						if (exit != 0)
						{
							Scripts.Auto = false;
							Assistant.Engine.MainWindow.SetCheckBoxAutoMode(false);
							Assistant.World.Player.SendMessage(LocString.EnhancedMacroError, exit);
						}
						else
							Thread.Sleep(script.Delay);
					}
				}

				if (AutoLoot.Auto)
				{
					AutoLoot.Engine(Assistant.Engine.MainWindow.AutoLootItemList, Assistant.Engine.MainWindow.AutoLootDelayLabel, m_CorpseFilter);
					Thread.Sleep(25);
				}
			}
		}

		internal static TimeSpan m_TimerDelay = TimeSpan.FromMilliseconds(100);

		internal static ScriptTimer m_Timer = new ScriptTimer();

		private static List<EnhancedScript> m_EnhancedScripts = new List<EnhancedScript>();
		internal static List<EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

		private static PaxScripter m_PaxScripter;
		internal static PaxScripter Engine { get { return m_PaxScripter; } }

		public static void Initialize()
		{
			m_PaxScripter = new PaxScripter();
			m_PaxScripter.OnChangeState += new ChangeStateHandler(paxScripter_OnChangeState);
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
			}
		}

		internal static void Reset()
		{
			Auto = false;
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
