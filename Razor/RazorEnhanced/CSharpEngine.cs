using Microsoft.CSharp;
using Microsoft.Scripting;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RazorEnhanced
{
    class CSharpEngine
    {
        private static CSharpEngine m_instance = null;
        public static CSharpEngine Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CSharpEngine();
                }
                return m_instance;
            }
        }

        private CSharpEngine()
        {
        }

        private CompilerParameters CompilerSettings(bool IncludeDebugInformation)
        {
            CompilerParameters parameters = new CompilerParameters();
            List<string> assemblies = GetReferenceAssemblies();
            foreach (string assembly in assemblies)
            {
                parameters.ReferencedAssemblies.Add(assembly);
            }

            parameters.GenerateInMemory = true; // True - memory generation, false - external file generation
            parameters.GenerateExecutable = false; // True - exe file generation, false - dll file generation
            parameters.TreatWarningsAsErrors = false; // Set whether to treat all warnings as errors.
            parameters.WarningLevel = 4; // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/errors-warnings
            //parameters.CompilerOptions = "/optimize"; // Set compiler argument to optimize output.
            parameters.IncludeDebugInformation = IncludeDebugInformation; // Build in debug or release
            return parameters;
        }


        private List<string> GetReferenceAssemblies()
        {
            List<string> list = new List<string>();

            string path = Path.Combine(Assistant.Engine.RootPath, "Scripts", "Assemblies.cfg");

            if (File.Exists(path))
            {
                using (StreamReader ip = new StreamReader(path))
                {
                    string line;

                    while ((line = ip.ReadLine()) != null)
                    {
                        if (line.Length > 0 && !line.StartsWith("#"))
                            list.Add(line);
                    }
                }
            }

            list.Add(Assistant.Engine.RootPath + "\\" + "RazorEnhanced.exe");
            list.Add(Assistant.Engine.RootPath + "\\" + "Ultima.dll");
            return list;
        }

        private bool ManageCompileResult(CompilerResults results, out StringBuilder errorwarnings)
        {
            bool has_error = true;

            StringBuilder sb = new StringBuilder();
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) at line {1}: {2}", error.ErrorNumber, error.Line, error.ErrorText));
                }
            }
            else
            {
                has_error = false;

                if (results.Errors.HasWarnings)
                {
                    foreach (CompilerError warning in results.Errors)
                    {
                        sb.AppendLine(String.Format("Warning ({0}) at line {1}: {2}", warning.ErrorNumber, warning.Line, warning.ErrorText));
                    }
                }
            }
            errorwarnings = sb;
            return has_error;
        }

        public bool CompileFromText(string source, out StringBuilder errorwarnings, out Assembly assembly)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compileParameters = CompilerSettings(true); // When compiler is invoked from the editor it's always in debug mode
            CompilerResults results = provider.CompileAssemblyFromSource(compileParameters, source); // Compiling

            assembly = null;
            bool has_error = ManageCompileResult(results, out errorwarnings);
            if (has_error)
            {
                var error = results.Errors[0];
                var a = new SourceLocation(0, error.Line, error.Column);
                throw new SyntaxErrorException(error.ErrorText, results.PathToAssembly, error.ErrorNumber, "", new SourceSpan(a, a), 0, Severity.Error);
            }
            else
            {
                assembly = results.CompiledAssembly;
            }
            return has_error;
        }

        public bool CompileFromFile(string path, bool debug, out StringBuilder errorwarnings, out Assembly assembly)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compileParameters = CompilerSettings(debug);
            CompilerResults results = provider.CompileAssemblyFromFile(compileParameters, path); // Compiling

            assembly = null;
            bool has_error = ManageCompileResult(results, out errorwarnings);
            if (!has_error)
            {
                assembly = results.CompiledAssembly;
            }
            return has_error;
        }

        public void Execute(Assembly assembly)
        {
            // This is important for methods visibility. Check if all of these flags are really needed.
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;

            MethodInfo run = null;

            // Search trough all methods and finds Run then calls it
            foreach (Type mt in assembly.GetTypes())
            {
                if (mt != null)
                {
                    run = mt.GetMethod("Run", bf);
                    if (run != null) 
                    {
                        break;
                    }
                }
            }

            // If Run method does not exists would be rised an exception later but better to throw a
            // SyntaxErrorException now and log it too
            if (run == null)
            {
                string error = "Required method 'public void Run() missing from script.";
                Misc.SendMessage(error);
                throw new Microsoft.Scripting.SyntaxErrorException(error,null, new SourceSpan(), 0, Severity.FatalError);
            }

            // Creates an instance of the class runs the Run method
            object scriptInstance = Activator.CreateInstance(run.DeclaringType);
            run.Invoke(scriptInstance, null);
        }

    }
}
