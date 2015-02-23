using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Diagnostics;
using Microsoft.CSharp;
using Assistant;
using System.Data;
using System.Threading.Tasks;
using PaxScript.Net;

namespace RazorEnhanced
{
	internal class Scripts
	{
		private static TimeSpan m_Delay = TimeSpan.FromMilliseconds(100.0);

		internal class EnhancedScript
		{
			private string m_File;
			internal string File { get { return m_File; } }

			private string m_Class;
			internal string Class { get { return m_Class; } }

			public EnhancedScript(string file, string classname)
			{
				m_File = file;
				m_Class = classname;
			}
		}

		private class ScriptManagerTimer : Assistant.Timer
		{
			private int m_Count;
			internal int Count { get { return m_Count; } }

			private int m_Index;
			internal int Index { get { return m_Index; } }

			private int? m_ExitCode = null;
			internal int? ExitCod { get { return m_ExitCode; } }

			public ScriptManagerTimer()
				: base(m_Delay, m_Delay)
			{
				m_Count = m_Scripts.Count;
				m_Index = 0;
			}

			protected override void OnTick()
			{
				if (m_Index < m_Count)
				{
					if (m_ExitCode == null)
					{
						EnhancedScript script = m_Scripts[m_Index];
						m_ExitCode = InvokeMethod<int>(script, "Run");
						System.Threading.Thread.Sleep(m_Delay.Milliseconds / 2);

						if (m_ExitCode != 0)
						{
							Stop();
							Assistant.Engine.MainWindow.razorCheckBoxAuto.Checked = false;
							Assistant.World.Player.SendMessage(LocString.EnhancedMacroError, m_ExitCode);
						}
						else
						{
							m_ExitCode = null;
							m_Index++;
						}
					}
				}
				else
				{
					if (m_Count <= 0)
					{
						Stop();
					}
					else
					{
						m_Index = 0;
					}
				}
			}
		}

		private static List<EnhancedScript> m_Scripts = new List<EnhancedScript>();
		internal static List<EnhancedScript> EnhancedScripts { get { return m_Scripts; } }

		private static ScriptManagerTimer m_ScriptManager;
		private static PaxScripter m_PaxScripter;

		public static void Initialize()
		{
			m_ScriptManager = new ScriptManagerTimer();
			m_PaxScripter = new PaxScripter();
			m_PaxScripter.OnChangeState += new ChangeStateHandler(paxScripter_OnChangeState);
		}

		private static void paxScripter_OnChangeState(PaxScripter sender, ChangeStateEventArgs e)
		{
			if (e.OldState == ScripterState.Init)
			{
			}
			else if (sender.HasErrors)
			{
				MessageBox.Show(sender.Error_List[0].Message);
			}
		}

		private static bool m_Auto = false;
		internal static bool Auto
		{
			get { return m_Auto; }
			set
			{
				if (m_Auto != value)
				{
					if (m_ScriptManager != null)
					{
						m_ScriptManager.Stop();
					}

					if (value)
					{
						m_ScriptManager = new ScriptManagerTimer();
						m_ScriptManager.Start();
					}

					m_Auto = value;
				}
			}
		}

		internal static void Reset()
		{
			m_Scripts.Clear();
			m_PaxScripter.Reset();
		}

		internal static void InitializeAssemblies()
		{
			foreach (EnhancedScript script in m_Scripts)
			{
				int exit = InvokeMethod<int>(script, "Initialize");
				if (exit != 0)
				{
					Assistant.Engine.MainWindow.razorCheckBoxAuto.Checked = false;
					Assistant.World.Player.SendMessage(LocString.EnhancedMacroError, exit);
					break;
				}
			}
		}

		internal static string Load(string file)
		{
			string status = "Loaded";
			string classname = Path.GetFileNameWithoutExtension(file);
			EnhancedScript script = new EnhancedScript(file, classname);

			try
			{
				m_Scripts.Add(script);
				m_PaxScripter.AddModule(script.Class);
				m_PaxScripter.AddCodeFromFile(script.Class, script.File);
			}
			catch (Exception ex)
			{
				status = "ERROR: " + ex.Message;
			}

			return status;
		}

		private static T InvokeMethod<T>(EnhancedScript script, string method)
		{
			T result = default(T);
			try
			{
				Task<T> task = new Task<T>(() => (T)m_PaxScripter.Invoke(RunMode.Run, null, "RazorEnhanced." + script.Class + "." + method));
				task.Start();
				result = task.Result;
			}
			catch
			{
			}

			return result;
		}
	}
}
