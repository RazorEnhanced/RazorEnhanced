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



namespace RazorEnhanced
{
	internal class Scripts
	{
		private static TimeSpan m_Delay = TimeSpan.FromMilliseconds(100.0);

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
				m_Count = m_Assemblies.Count;
				m_Index = 0;
			}

			protected override void OnTick()
			{
				if (m_Index < m_Count)
				{
					if (m_ExitCode == null)
					{
						Assembly assembly = m_Assemblies[m_Index];
						m_ExitCode = RunAssemblyTask(assembly);
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

		private static List<Assembly> m_Assemblies = new List<Assembly>();
		internal static List<Assembly> Assemblies { get { return m_Assemblies; } }

		private static ScriptManagerTimer m_ScriptManager = new ScriptManagerTimer();

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

		internal static string[] GetReferenceAssemblies()
		{
			ArrayList refs = new ArrayList(1);

			refs.Add(Engine.ExePath);

			string path = Path.Combine(Directory.GetCurrentDirectory(), "Scripts/References.cfg");

			if (File.Exists(path))
			{
				using (StreamReader ip = new StreamReader(path))
				{
					string line;

					while ((line = ip.ReadLine()) != null)
					{
						if (line.Length > 0 && !line.StartsWith("#"))
							refs.Add(line);
					}
				}
			}

			return (string[])refs.ToArray(typeof(string));
		}

		private static Assembly Compile(string file)
		{
			CompilerParameters CompilerParams = new CompilerParameters();
			string outputDirectory = Directory.GetCurrentDirectory();

			CompilerParams.GenerateInMemory = false;
			CompilerParams.TreatWarningsAsErrors = false;
			CompilerParams.GenerateExecutable = false;
			CompilerParams.OutputAssembly = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", Path.GetFileNameWithoutExtension(file) + ".dll");
			CompilerParams.CompilerOptions = "/optimize";

			// string[] references = GetReferenceAssemblies();
			string[] references = new string[] { "System.dll", "Razor.exe" };
			CompilerParams.ReferencedAssemblies.AddRange(references);

			CSharpCodeProvider provider = new CSharpCodeProvider();
			CompilerResults compile = provider.CompileAssemblyFromFile(CompilerParams, file);

			if (compile.Errors.HasErrors)
			{
				string text = "Compile error: ";
				foreach (CompilerError ce in compile.Errors)
				{
					text += "rn" + ce.ToString();
				}
				throw new Exception(text);
			}

			Assembly assembly = compile.CompiledAssembly;
			return assembly;
		}

		private static Assembly Load(string file)
		{
			Assembly assembly = Assembly.LoadFile(file);
			return assembly;
		}

		internal static void Reset()
		{
			m_Assemblies.Clear();
		}

		internal static string CompileOrLoad(string script)
		{
			string status = "Idle";
			Assembly assembly = null;

			if (File.Exists(script))
			{
				string extension = Path.GetExtension(script).ToLower();
				string name = Path.GetFileNameWithoutExtension(script);

				try
				{
					if (extension == ".cs")
					{
						assembly = Compile(script);
					}
					else if (extension == ".dll")
					{
						assembly = Load(script);
					}
					if (assembly != null)
					{
						status = "Loaded";
						m_Assemblies.Add(assembly);
					}
				}
				catch (Exception ex)
				{
					status = "ERROR! " + ex.Message;
				}
			}
			return status;
		}

		internal static void InitializeAssemblies()
		{
			foreach (Assembly assembly in m_Assemblies)
			{
				Type[] types = assembly.GetTypes();
				for (int i = 0; i < types.Length; ++i)
				{
					MethodInfo m = types[i].GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public);

					if (m != null)
						m.Invoke(null, null);
				}
			}
		}

		private static int RunAssemblyTask(Assembly assembly)
		{
			Module module = assembly.GetModules()[0];

			if (module != null)
			{
				AssemblyName name = assembly.GetName();
				Type type = module.GetType(name.Name + "." + name.Name);

				if (type != null)
				{
					object instance = Activator.CreateInstance(type);
					MethodInfo methInfo = type.GetMethod("Run");
					if (methInfo != null && methInfo.ReturnType == typeof(int))
					{
						Task<int> task = new Task<int>(() => (int)methInfo.Invoke(instance, null));
						task.Start();
						int result = task.Result;
						return result;
					}
				}
			}

			return Int32.MinValue;
		}
	}
}
