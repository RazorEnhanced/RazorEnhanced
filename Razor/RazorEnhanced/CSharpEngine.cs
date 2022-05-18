using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Scripting;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System.Linq;

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
            //parameters.CompilerOptions = "-langversion:8.0";
            parameters.CompilerOptions = "-parallel";
            parameters.IncludeDebugInformation = IncludeDebugInformation; // Build in debug or release
            return parameters;
        }
        class CompilerOptions : IProviderOptions
        {
            string _compilerVersion = "8.0";
            IDictionary<string, string> _compilerOptions = new Dictionary<string, string>() { };
            public string CompilerVersion { get => _compilerVersion; set { _compilerVersion = value; } }
            public bool WarnAsError => false;
            public bool UseAspNetSettings => true;
            public string CompilerFullPath => Path.Combine(Assistant.Engine.RootPath, "roslyn", "csc.exe");
            public int CompilerServerTimeToLive => 60 * 60; // 1h
            IDictionary<string, string> IProviderOptions.AllOptions { get => _compilerOptions; }
            IDictionary<string, string> Options { set { _compilerOptions = value; } } // For Debug
        }

        private List<string> GetReferenceAssemblies()
        {
            List<string> list = new();

            string assemblies_cfg_path = Path.Combine(Assistant.Engine.RootPath, "Scripts", "Assemblies.cfg");

            if (File.Exists(assemblies_cfg_path))
            {
                using StreamReader ip = new(assemblies_cfg_path);
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    if (line.Length > 0 && !line.StartsWith("#"))  // # means comment
                        list.Add(line);
                }
            }

            // Replace with full path all assemblis that are in razor path
            for (int i = 0; i < list.Count; i++)
            {
                string assembly_path = Path.Combine(Assistant.Engine.RootPath, list[i]);
                if (File.Exists(assembly_path))
                {
                    list[i] = assembly_path;
                }
            }

            // Adding Razor and Ultima.dll as default
            list.Add(Assistant.Engine.RootPath + Path.DirectorySeparatorChar + "RazorEnhanced.exe");
            list.Add(Assistant.Engine.RootPath + Path.DirectorySeparatorChar + "Ultima.dll");
            return list;
        }

        private bool ManageCompileResult(CompilerResults results, ref List<string> errorwarnings)
        {
            bool has_error = true;

            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    errorwarnings.Add(String.Format("Error ({0}) at line {1}: {2}", error.ErrorNumber, error.Line, error.ErrorText));
                }
            }
            else
            {
                has_error = false;

                if (results.Errors.HasWarnings)
                {
                    foreach (CompilerError warning in results.Errors)
                    {
                        errorwarnings.Add(String.Format("Warning ({0}) at line {1}: {2}", warning.ErrorNumber, warning.Line, warning.ErrorText));
                    }
                }
            }
            return has_error;
        }

        /// <summary>
        /// This function search for our custom directive //#import that allows import classes from other C# files
        /// The directive must be added anywhere before the namespace and can be used in C stile with &gt; &lt; or ""
        /// Using relative path with &gt; &lt; the base directory will the Scripts folder
        /// </summary>
        /// <param name="sourceFile">Full path of the source file</param>
        /// <param name="filesList">List of all files that must be compiled (it's a recursive list)</param>
        /// <param name="errorwarnings">List of error and warnings</param>
        private void FindAllIncludedCSharpScript(string sourceFile, ref List<string> filesList, ref List<string> errorwarnings)
        {
            const string directive = "//#import";

            if (!File.Exists(sourceFile))
            {
                errorwarnings.Add(string.Format("Error on directive {0}. Unable to find {1}", directive, sourceFile));
                return;
            }

            string basepath = Path.GetDirectoryName(sourceFile); // BasePath of the imported file
            filesList.Add(sourceFile);

            // Searching all the lines with the directive
            List<string> imports = new();
            foreach (string line in File.ReadAllLines(sourceFile))
            {
                if (line.Contains(directive))
                {
                    string file = line.Replace(directive, "").Trim();
                    imports.Add(file);
                }

                // If namespace directive is found stop searching
                if (line.ToLower().Contains("namespace")) { break; }
            }

            // If nothing is found return only the main file
            if (imports.Count == 0) { return; }

            // Parsing each line
            int lineCnt = 0;
            foreach (string line in imports)
            {
                string file = "";
                lineCnt++; // Count lines from 1
                if (line.StartsWith("<") && line.EndsWith(">"))
                {
                    // Relative path. Adding base folder
                    file = line.Substring(1, line.Length - 2); // Removes < >
                    file = Path.GetFullPath(Path.Combine(basepath, file)); // Basepath is Scripts folder
                }
                else if (line.StartsWith("\"") && line.EndsWith("\""))
                {
                    // Absolute path. Adding as is
                    file = line.Substring(1, line.Length - 2); // Removes " "
                    file = Path.GetFullPath(file); // This should resolve the relative ../ path
                }
                else
                {
                    errorwarnings.Add(string.Format("Error on RE Directive {0} at line {1}", directive, lineCnt));
                    break;
                }

                // I search if already exists in the filesList
                var match = filesList.FirstOrDefault(stringToCheck => stringToCheck.Contains(file));
                if (match == null)
                {
                    FindAllIncludedCSharpScript(file, ref filesList, ref errorwarnings);
                }
            }
        }

        /// <summary>
        /// This function checks for directive //#forcerelease
        /// If this directive is present, script will be builded in release instead of debug
        /// </summary>
        /// <param name="sourceFile">Filename of the main source file</param>
        /// <returns></returns>
        private bool CheckForceReleaseDirective(string sourceFile)
        {
            const string directive = "//#forcerelease";

            // Searching the directive in all lines untill "namespace"
            foreach (string line in File.ReadAllLines(sourceFile))
            {
                if (line.ToLower().Contains(directive))
                {
                    return true;
                }

                // If namespace directive is found stop searching
                if (line.ToLower().Contains("namespace")) { break; }
            }
            return false;
        }


        /*
        public bool CompileFromText(string source, out List<string> errorwarnings, out Assembly assembly)
        {
            CompilerOptions opt = new();
            CSharpCodeProvider provider = new(opt);


            string myTempFile = Path.Combine(Path.GetTempPath(), "re_script.cs");

            Misc.SendMessage("Compiling C# Script");
            CompilerParameters compileParameters = CompilerSettings(true); // When compiler is invoked from the editor it's always in debug mode
            CompilerResults results = provider.CompileAssemblyFromSource(compileParameters, source); // Compiling
            Misc.SendMessage("Compile Done");

            assembly = null;
            errorwarnings = new();
            bool has_error = ManageCompileResult(results, ref errorwarnings);
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
        */

        // https://medium.com/swlh/replace-codedom-with-roslyn-but-bin-roslyn-csc-exe-not-found-6a5dd9290bf2
        // https://stackoverflow.com/questions/20018979/how-can-i-target-a-specific-language-version-using-codedom
        // https://docs.microsoft.com/it-it/dotnet/api/microsoft.csharp.csharpcodeprovider.-ctor?view=net-5.0
        // https://github.com/aspnet/RoslynCodeDomProvider/blob/main/src/Microsoft.CodeDom.Providers.DotNetCompilerPlatform/Util/IProviderOptions.cs
        // https://josephwoodward.co.uk/2016/12/in-memory-c-sharp-compilation-using-roslyn
        public bool CompileFromFile(string path, bool debug, out List<string> errorwarnings, out Assembly assembly)
        {
            errorwarnings = new();
            assembly = null;

            // If debug is true I check for the force release directive
            if (debug == true)
            {
                debug = (CheckForceReleaseDirective(path) != true); // If flag is true then debug is false
            }

            List<string> filesList = new() { }; // List of files.
            FindAllIncludedCSharpScript(path, ref filesList, ref errorwarnings);
            if (errorwarnings.Count > 0)
            {
                return true;
            }

            if (debug)
            {
                Misc.SendMessage("Compiling C# Script [DEBUG] " + Path.GetFileName(path));
            }
            else
            {
                Misc.SendMessage("Compiling C# Script [RELEASE] " + Path.GetFileName(path));
            }

            DateTime start = DateTime.Now;

            CompilerOptions m_opt = new();
            CSharpCodeProvider m_provider = new(m_opt);
            CompilerParameters m_compileParameters = CompilerSettings(true); 

            m_compileParameters.IncludeDebugInformation = debug;
            CompilerResults results = m_provider.CompileAssemblyFromFile(m_compileParameters, filesList.ToArray()); // Compiling

            DateTime stop = DateTime.Now;
            Misc.SendMessage("Script compiled in " + (stop - start).TotalMilliseconds.ToString("F0") + " ms");

            bool has_error = ManageCompileResult(results, ref errorwarnings);
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
            int runMethodsFound = 0;
            foreach (Type mt in assembly.GetTypes())
            {
                if (mt != null)
                {
                    MethodInfo method = mt.GetMethod("Run", bf);
                    if (method != null) 
                    {
                        run = method;
                        runMethodsFound++;
                        if (runMethodsFound > 1)
                        {
                            string error = "Found more than one 'public void Run()' method in script.\nMust be only one Run method.";
                            Misc.SendMessage(error);
                            throw new Microsoft.Scripting.SyntaxErrorException(error, null, new SourceSpan(), 0, Severity.FatalError);
                        }
                    }
                }
            }

            // If Run method does not exists would be rised an exception later but better to throw a
            // SyntaxErrorException now and log it too
            if (run == null)
            {
                string error = "Required method 'public void Run()' missing from script.";
                Misc.SendMessage(error);
                throw new Microsoft.Scripting.SyntaxErrorException(error,null, new SourceSpan(), 0, Severity.FatalError);
            }

            // Creates an instance of the class runs the Run method
            object scriptInstance = Activator.CreateInstance(run.DeclaringType);

            run.Invoke(scriptInstance, null);
        }

    }
}
