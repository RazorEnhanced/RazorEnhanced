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



namespace RazorEnhanced
{
	internal class Scripts
	{
		internal static AppDomain m_Domain = AppDomain.CreateDomain("scripts");

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

			string[] references = GetReferenceAssemblies();
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
			AppDomain.Unload(m_Domain);
			m_Domain = AppDomain.CreateDomain("scripts");
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
						m_Domain.Load(assembly.GetName());
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
			foreach (Assembly assembly in m_Domain.GetAssemblies())
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

		private static int? RunAssembly(Assembly assembly)
		{
			Module module = assembly.GetModules()[0];
			MethodInfo methInfo = null;

			if (module != null)
			{
				foreach (Type mt in module.GetTypes())
				{

					if (mt != null)
					{
						methInfo = mt.GetMethod("Run");
					}

					if (methInfo != null && methInfo.ReturnType == typeof(int))
					{
						return (int)methInfo.Invoke(null, null);
					}
				}
			}

			return null;
		}
	}
}
