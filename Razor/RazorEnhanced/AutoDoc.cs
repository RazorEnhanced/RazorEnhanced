using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace RazorEnhanced
{
    [Serializable]
    class DocSettings {
        public string version = Assembly.GetAssembly(typeof(DocSettings)).ImageRuntimeVersion;
        public string baseName = "RazorEnhanced.";
    }

    [Serializable]
    class DocContainer {
        public DocSettings settings = new DocSettings();

        public List<DocClass> classes = new List<DocClass>();
        public List<DocMethod> constructors = new List<DocMethod>();
        public List<DocProperty> properties = new List<DocProperty>();
        public List<DocMethod> methods = new List<DocMethod>();

        public void AddRange(DocContainer otherContainer) {
            classes.AddRange(otherContainer.classes);
            properties.AddRange(otherContainer.properties);
            methods.AddRange(otherContainer.methods);
            properties.AddRange(otherContainer.properties);
        }
    }


    [Serializable]
    class DocItem {
        public const String KindItem = "item";      // Generic or Unkown (TODO: Make it sensible)
        public const String KindClass = "class";      // Generic or Unkown (TODO: Make it sensible)
        public const String KindMethod = "method";
        public const String KindProperty = "property";

        public String xmlKey;
        public String itemKind;
        public String itemClass;
        public String itemName;
        public String itemDescription;
        public bool flagAutocomplete = false;

        public DocItem(String xmlKey, String className, String propertyName, String description) {
            this.xmlKey = xmlKey;
            this.itemKind = DocItem.KindItem;
            this.itemClass = className;
            this.itemName = propertyName;
            this.itemDescription = description;
        }
    }

    [Serializable]
    class DocClass : DocItem
    {
        public DocClass(String xmlKey, String className, String description) : base(xmlKey, className, className, description)
        {
            this.itemKind = DocItem.KindClass;
        }
    }

    [Serializable]
    class DocProperty : DocItem
    {
        public String propertyType;
        public DocProperty(String xmlKey, String className, String propertyName, String propertyType, String description) : base(xmlKey, className, propertyName, description)
        {
            this.itemKind = DocItem.KindProperty;
            this.propertyType = propertyType;
        }
    }

    [Serializable]
    class DocMethod : DocItem {
        public String returnType;
        public List<DocMethodParam> paramList;

        public DocMethod(String xmlKey, String className, String methodName, String returnType, String description, List<DocMethodParam> paramList) : base(xmlKey, className, methodName, description)
        {
            this.itemKind = DocItem.KindMethod;
            this.returnType = returnType;
            this.paramList = paramList;
        }
    }

    [Serializable]
    class DocMethodParam {
        public String itemName;
        public String itemType;
        public String itemDefault;
        public String itemDescription;

        public DocMethodParam(String paramName, String paramType, String description, String defaultValue = null) {
            this.itemName = paramName;
            this.itemType = paramType;
            this.itemDefault = defaultValue;
            this.itemDescription = description;
        }
    }




    /// <summary>
    /// Automatically generate the Python API
    /// DOING:
    /// 0a. Read Reflection
    /// 0b. Read Comments 
    /// 0c. Produce a JSON-compatible object.
    /// TODO:
    /// 1. Autocomplete/SyntaxHighlight for script editor
    /// 2. Wiki Documentation
    /// 3. razor.py 
    /// </summary>
    class AutoDoc
    {
        const String DEFAULT_JSON_PATH = "RazorEnhanced.json";
        const String DEFAULT_HTML_PATH = "./Docs/HTML/";
        const String DEFAULT_MD_PATH = "./Docs/";
        const String TAG_AUTOCOMPLETE = "@autocomplete";
        const String TAG_NODOC = "@nodoc";

        private static DocContainer cachedDocs;


        private static string html_main = @"
            <html>
                <header>
                    <link rel=stylesheet href='style.css' type='text/css'/>
                </header>
                <body>
                    {0}
                </body>
            </html>
        ";
        public static void ExportHTML(string path = null)
        {
            if (path == null) { path = DEFAULT_HTML_PATH; }
            var docs = GetPythonAPI();

            List<DocClass> classList = docs.classes;
            List<DocProperty> propsList = docs.properties;
            List<DocMethod> methodList = docs.methods;

            //Sort alphabetically
            //classList.Sort( (c1,c2) => c1.itemClass.CompareTo(c2.itemClass) );
            // Create main index.html
            var classListHtml = new List<string>();
            foreach (var cls in classList) {
                classListHtml.Add($"<li><a href='{cls.itemClass}.html'>{cls.itemClass}</a></li>");
            }
            var menu = $"<ul>\n{String.Join("\n", classListHtml)}\n</ul>";
            Directory.CreateDirectory(path);
            File.WriteAllText(path + "index.html", String.Format(html_main, menu));

            // Create per-class docs
            foreach (var cls in classList)
            {
                var className = cls.itemClass;
                var classProps = propsList.FindAll(doc => doc.itemClass == className);
                var classMethod = methodList.FindAll(doc => doc.itemClass == className);

                //Sort A-Z
                classProps.Sort((c1, c2) => c1.itemName.CompareTo(c2.itemName));
                classMethod.Sort((c1, c2) => c1.itemName.CompareTo(c2.itemName));

                // Get props
                var propsListHtml = new List<string>();
                foreach (var prop in classProps)
                {
                    var propName = $"<div class='property_name'>{className}.{prop.itemName}</div>";
                    var propDesc = $"<div class='property_desc'>{prop.itemDescription}</div>";
                    propsListHtml.Add($"{propName}\n{propDesc}\n");
                }

                // Get methods
                var methodListHtml = new List<string>();
                foreach (DocMethod method in classMethod)
                {
                    var argsSign = new List<String>();
                    var argsList = new List<String>();
                    foreach (DocMethodParam arg in method.paramList) {
                        var sign = $"<div class='arg_type'>{arg.itemType}</div> <div class='arg_name'>{arg.itemName}</div>";
                        argsSign.Add(sign);
                        argsList.Add($"<li>{sign} <div class='arg_desc'>{arg.itemDescription}</div></li>");
                    }
                    var argSignHtml = $"<div class='method_arg_list'>{String.Join(", ", argsSign)}</div>";
                    var argListHtml = $"<ul>\n{String.Join("\n", argsList)}</ul>";
                    var methodName = $"<div class='method_name'>{className}.{method.itemName}({argSignHtml})</div>";
                    var methodDesc = $"<div class='method_desc'>{method.itemDescription}</div>";

                    methodListHtml.Add($"<hr/>{methodName}<br/>{argListHtml}<br/>{methodDesc}<br/>");
                }

                // Assable page
                var propsHtml = String.Join("\n", propsListHtml);
                var methodsHtml = $@"<div class='method_list'>{String.Join("\n", methodListHtml)}</div>";

                var classHtml = $@"<div class='class_name'>{className}</div>";
                var classDescHtml = $@"<div class='class_desc'>{cls.itemDescription}</div>";

                var content = $@"{classHtml}{classDescHtml}{propsHtml}{methodsHtml}";
                File.WriteAllText(path + className + ".html", String.Format(html_main, content));
            }
        }


        public static void ExportMKDocs(string path = null)
        {
            if (path == null) { path = DEFAULT_MD_PATH; }
            var docs = GetPythonAPI();

            List<DocClass> classList = docs.classes;
            List<DocProperty> propsList = docs.properties;
            List<DocMethod> methodList = docs.methods;

            //Sort alphabetically
            //classList.Sort( (c1,c2) => c1.itemClass.CompareTo(c2.itemClass) );
            // Create main index.html
            var classListYaml = new List<string>();
            foreach (var cls in classList)
            {
                classListYaml.Add($"    - {cls.itemClass}: {cls.itemClass}.md");
            }

            String index;
            index = "site_name: Razor Enhaced\n";
            index += "docs_dir: ./MKDocs/\n";
            index += "nav:\n";
            index += "  - Classes:\n";
            index += String.Join("\n", classListYaml) + "\n";
            index += "theme:\n";
            index += "  name: readthedocs\n";
            index += "markdown_extensions:\n";
            index += "  - codehilite\n";
            //index += "      linenums: False\n";
            //index += "      use_pygments: True\n";


            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "MKDocs/");
            File.WriteAllText(path + "mkdocs.yml", index);

            // Create per-class docs
            foreach (var cls in classList)
            {
                var className = cls.itemClass;
                var classProps = propsList.FindAll(doc => doc.itemClass == className);
                var classMethod = methodList.FindAll(doc => doc.itemClass == className);

                //Sort A-Z
                classProps.Sort((c1, c2) => c1.itemName.CompareTo(c2.itemName));
                classMethod.Sort((c1, c2) => c1.itemName.CompareTo(c2.itemName));

                // Get props
                var propsListMD = new List<string>();
                foreach (DocProperty prop in classProps)
                {
                    var propName = $"### {className}.{prop.itemName} __{prop.propertyType}__";
                    var propDesc = prop.itemDescription != "" ? "\n" + prop.itemDescription : "";
                    propsListMD.Add($"{propName}{propDesc}");
                }

                // Get methods
                var methodListMD = new List<string>();
                foreach (DocMethod method in classMethod)
                {
                    var argsSign = new List<String>();
                    var argsList = new List<String>();
                    foreach (DocMethodParam arg in method.paramList)
                    {
                        var sign = $"{arg.itemName}";
                        argsSign.Add(sign);
                        argsList.Add($"- **{arg.itemName}**: {arg.itemType} {arg.itemDescription}");
                    }
                    var argListDM = $"{String.Join("\n", argsList)}";
                    var methodTitle = $"### {className}.{method.itemName}";
                    var methodName = $"```\n{className}.{method.itemName}({String.Join(", ", argsSign)}) -> {method.returnType}\n```";
                    var methodBody = $"\n{methodName}\n{argListDM}\n".Trim();
                    var methodDesc = method.itemDescription;

                    methodListMD.Add($"{methodTitle}\n{methodBody}\n{methodDesc}".Trim());
                }

                // Assable page
                var propsMD = String.Join("\n", propsListMD);
                var methodsMD = String.Join("\n", methodListMD);

                var classMD = $@"# {className}  ";
                var classDescMD = cls.itemDescription + "  \n\n";

                String content;
                content = classMD;
                content += classDescMD;
                content += $"## Properties  \n{propsMD} \n";
                content += $"## Methods  \n{methodsMD}";
                File.WriteAllText(path + "MKDocs/" + className + ".md", content);
            }
        }


        /// <summary>
        /// Export the API to disk. 
        /// See docs for more, lol.
        ///     1-
        ///     2-
        ///     3-
        /// 
        /// end.
        /// </summary>
        /// <param name="path">Define the output path</param>
        /// <param name="pretty">Output readable JSON (Default: True)</param>
        public static void ExportPythonAPI(string path = null, bool pretty = true) {
            if (path == null) { path = DEFAULT_JSON_PATH; }

            String json_txt;

            DocContainer docs = GetPythonAPI();
            if (pretty) {
                var options = new JsonSerializerSettings();
                options.Formatting = Newtonsoft.Json.Formatting.Indented;
                json_txt = JsonConvert.SerializeObject(docs, options);
            }
            else {
                json_txt = JsonConvert.SerializeObject(docs);
            }
            File.WriteAllText(path, json_txt);
        }

        public static bool JsonDocExists(){
            return File.Exists(DEFAULT_JSON_PATH);
        }

        public static void ImportPythonAPI(string path = null)
        {
            if (path == null) { path = DEFAULT_JSON_PATH; }

            if ( JsonDocExists() )
            {
                var json_txt = File.ReadAllText(DEFAULT_JSON_PATH);
                cachedDocs = JsonConvert.DeserializeObject<DocContainer>(json_txt);
            }
        }

        public static List<String> GetClasses()
        {
            var docs = GetPythonAPI();
            var names = new HashSet<String>();
            foreach (var doc in docs.classes)
            {
                names.Add(doc.itemName);
            }
            return new List<String>(names);
        }

        public static List<String> GetProperties(bool withClass = false)
        {
            var docs = GetPythonAPI();
            var names = new HashSet<String>();
            foreach (var doc in docs.properties)
            {
               names.Add( withClass ? doc.itemClass + "." + doc.itemName : doc.itemName);
            }
            
            return new List<String>(names);
        }

        public static List<String> GetMethods(bool withClass = false, bool withNames = false, bool withTypes = false )
        {
            var docs = GetPythonAPI();
            var names = new HashSet<String>();
            foreach (var method in docs.methods)
            {
                var methodName = withClass ? method.itemClass + "." + method.itemName : method.itemName;
                    
                if (withTypes || withNames)
                {
                    var prms = new List<String>();
                    foreach (var prm in method.paramList)
                    {
                        String prm_txt;
                        if (withTypes && withNames) { prm_txt = prm.itemType + " " + prm.itemName; }
                        else if (withTypes) { prm_txt = prm.itemType; }
                        else { prm_txt = prm.itemName; }
                        prms.Add(prm_txt);
                    }
                    names.Add($"{methodName}({String.Join(",", prms)})");
                }
                else {
                    names.Add(methodName);
                }
                    
            }
            return new List<String>(names);
        }

        /// <summary>
        /// Use reflection to generete the Python API List
        /// </summary>
        public static DocContainer GetPythonAPI()
        {
            if (cachedDocs != null) { return cachedDocs; }

            Misc.SendMessage("AutoDoc v0.01", 20);
            var docSections = new List<Type> {
                // Test
                typeof(AutoDoc),
                // API
                typeof(Misc),
                typeof(Item),
                typeof(Items),
                typeof(Mobile),
                typeof(Mobiles),
                typeof(Player),
                typeof(Spells),
                typeof(Gumps),
                typeof(Journal),
                typeof(Target),
                typeof(Statics),

                // API Agents
                typeof(AutoLoot),
                typeof(Scavenger),
                typeof(SellAgent),
                typeof(BuyAgent),
                typeof(Organizer),
                typeof(Dress),
                typeof(Friend),
                typeof(Restock),
                typeof(BandageHeal),
                typeof(PathFinding),
                typeof(DPSMeter),
                typeof(Timer),
                typeof(Vendor),

                // Other classes
                typeof(Point2D),
                typeof(Point3D),
                typeof(Tile),
                typeof(Property),
                typeof(PathFinding.Route),
                typeof(Items.Filter),
                typeof(Mobiles.Filter),
                typeof(HotKeyEvent),
            };
            

            BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            cachedDocs = new DocContainer();
            
            foreach (var docSection in docSections)
            {
                var classDocs = AutoDoc.ReadClass(docSection, flags);
                cachedDocs.AddRange(classDocs);
            }

            return cachedDocs;
        }

        public static String ParamType(ParameterInfo param)
        {
            var name = param.ParameterType.Name;

            if (param.ParameterType.HasElementType)
            {
                // The type is either an array, pointer, or reference
                if (param.ParameterType.IsArray)
                {
                    // Append the "[]" array brackets onto the element type
                    name += "[]";
                }
                else if (param.ParameterType.IsPointer)
                {
                    // Append the "*" pointer symbol to the element type
                    name += "*";
                }
                else if (param.ParameterType.IsByRef)
                {
                    // Append the "@" symbol to the element type
                    name += "@";
                }
            }
            else if (param.ParameterType.IsGenericParameter)
            {
                //TODO: do it properly ^_^
                name += "<T>";
                // Look up the index of the generic from the
                // dictionaries in Figure 5, appending "`" if
                // the parameter is from a type or "``" if the
                // parameter is from a method
            }

            return name;
        }

        public static bool HasTag(string tag, string text) {
            return text.Contains(tag);        
        }

        public static DocContainer ReadClass(Type type, BindingFlags flags)
        {
            var result = new DocContainer();

            var classKey = XMLKeyComposer.GetKey(type);
            var className = type.Name;
            var classSummary = XMLDocReader.GetDocumentation(type);
            result.classes.Add( new DocClass(classKey, className, classSummary) );


            // Methods
            var methods = type.GetMethods(flags);
            foreach (var method in methods)
            {
                // Skip: setter, getter, operator-overloader, etc
                if (method.IsSpecialName) { continue; }

                var methodKey = XMLKeyComposer.GetKey(method);
                var methodName = method.Name;
                var returnType = method.ReturnType.Name;
                var paramList = new List<DocMethodParam>();

                var documentation = XMLDocReader.GetDocumentation(method);
                var methodSummary = XMLDocReader.ExtractXML(documentation, "summary");
                if ( HasTag(TAG_NODOC, methodSummary) ) continue;

                var prms = method.GetParameters();
                foreach (var prm in prms)
                {
                    
                    var paramName = prm.Name;
                    var paramType = ParamType(prm);
                    var defaultValue = ( prm.DefaultValue != null ? prm.DefaultValue.ToString() : null ) ;
                    var paramSummary = XMLDocReader.GetDocumentation(prm);

                    var param = new DocMethodParam(paramName, paramType, defaultValue, paramSummary);
                    paramList.Add( param );
                }
                
                var mtd = new DocMethod(methodKey, className, methodName, returnType, methodSummary, paramList);
                if (HasTag(TAG_AUTOCOMPLETE, methodSummary)) mtd.flagAutocomplete = true;
                result.methods.Add(mtd);
            }

            // Properties
            var props = type.GetProperties(flags);
            foreach (var prop in props)
            {
                var propKey = XMLKeyComposer.GetKey(prop);
                var itemName = prop.Name;
                var propertyType = prop.PropertyType.Name;
                var propSummary = XMLDocReader.GetDocumentation(prop);

                var prt = new DocProperty(propKey, className, itemName, propertyType, propSummary);
                result.properties.Add(prt);
            }

            // Fields
            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                var fieldKey = XMLKeyComposer.GetKey(field);
                var itemName = field.Name;
                var fieldType = field.FieldType.Name;
                var fieldSummary = XMLDocReader.GetDocumentation(field);

                var prt = new DocProperty(fieldKey, className, itemName, fieldType, fieldSummary);
                result.properties.Add(prt);
            }

            return result;
        }
    }

    class XMLKeyComposer
    {
        /*
            <member name="M:RazorEnhanced.Items.Move(RazorEnhanced.Item,RazorEnhanced.Mobile,System.Int32)">
            <member name="M:RazorEnhanced.Items.Move(System.Int32,RazorEnhanced.Mobile,System.Int32)">
        */
        public static string GetKey(MethodInfo methodInfo)
        {
            var paramList = new List<String>();
            foreach (var prm in methodInfo.GetParameters())
            {
                paramList.Add(ComposeKey(prm.ParameterType.FullName, null));
            }

            string key = String.Format("M:{0}", ComposeKey(methodInfo.DeclaringType.FullName, methodInfo.Name));
            if (paramList.Count > 0) // 2 or more
            {
                key += String.Format("({0})", String.Join(",", paramList));
            }
            return key;
        }

        /*
         <member name = "T:RazorEnhanced.Items">
        </member>
        */
        public static string GetKey(ConstructorInfo constructorInfo)
        {
            var paramList = new List<String>();
            foreach (var prm in constructorInfo.GetParameters())
            {
                paramList.Add(ComposeKey(prm.ParameterType.FullName, prm.Name));
            }
            string key = String.Format("M:{0}({1})", ComposeKey(constructorInfo.DeclaringType.FullName, null), String.Join(",", paramList));
            return key;
        }
        
        /*
         <member name = "T:RazorEnhanced.Items"></member>
        */
        public static string GetKey(Type type)
        {
            string key = "T:" + ComposeKey(type.FullName, null);
            return key;
        }

        public static string GetKey(PropertyInfo propertyInfo)
        {
            string key = "P:" + ComposeKey(propertyInfo.DeclaringType.FullName, propertyInfo.Name);
            return key;
        }

        public static string GetKey(EventInfo eventInfo)
        {
            string key = "E:" + ComposeKey(eventInfo.DeclaringType.FullName, eventInfo.Name);
            return key;
        }

        public static string GetKey(FieldInfo fieldInfo)
        {
            string key = "F:" + ComposeKey(fieldInfo.DeclaringType.FullName, fieldInfo.Name);
            return key;
        }





        private static string ComposeKey(string typeFullNameString, string memberNameString)
        {
            string key = Regex.Replace(typeFullNameString, @"\[.*\]", string.Empty).Replace('+', '.');
            if (memberNameString != null)
            {
                key += "." + memberNameString;
            }
            return key;
        }
    }


    /// <summary>
    /// Author: Dalamar
    /// XML Doc reader
    /// Assembled following this article:
    /// https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/october/csharp-accessing-xml-documentation-via-reflection
    /// </summary>
    /// 
    class XMLDocReader{
        internal static HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();
        internal static Dictionary<string, string> loadedXmlDocumentation = new Dictionary<string, string>();

        public static void LoadXmlDocumentation(string xmlDocumentation)
        {
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlDocumentation)))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
                    {
                        string raw_name = xmlReader["name"];
                        loadedXmlDocumentation[raw_name] = xmlReader.ReadInnerXml();
                    }
                }
            }
        }

        internal static void LoadXmlDocumentation(Assembly assembly)
        {
            if (loadedAssemblies.Contains(assembly))
            {
                return; // Already loaded
            }
            string directoryPath = GetPath(assembly);
            string xmlFilePath = Path.Combine(directoryPath, assembly.GetName().Name + ".xml");
            if (File.Exists(xmlFilePath))
            {
                LoadXmlDocumentation(File.ReadAllText(xmlFilePath));
                loadedAssemblies.Add(assembly);
            }
        }

        public static string GetPath(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
        
       


        public static string GetDocumentation(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType.HasFlag(MemberTypes.Field))
            {
                return GetDocumentation(((FieldInfo)memberInfo));
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Property))
            {
                return GetDocumentation((PropertyInfo)memberInfo);
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Event))
            {
                return GetDocumentation((EventInfo)memberInfo);
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Constructor))
            {
                return GetDocumentation((ConstructorInfo)memberInfo);
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Method))
            {
                return GetDocumentation((MethodInfo)memberInfo);
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.TypeInfo) ||
                     memberInfo.MemberType.HasFlag(MemberTypes.NestedType))
            {
                return GetDocumentation((TypeInfo)memberInfo);
            }
            else
            {
                return "";
            }
        }

        public static string GetDocumentation(Type type)
        {
            LoadXmlDocumentation(type.Assembly);  //Autoload xml based.
            string key = XMLKeyComposer.GetKey(type);
            loadedXmlDocumentation.TryGetValue(key, out string documentation);
            return documentation ?? "";
        }




        public static string GetDocumentation(ParameterInfo parameterInfo)
        {
            string memberDocumentation = GetDocumentation(parameterInfo.Member);
            if (memberDocumentation != "")
            {
                var name = Regex.Escape(parameterInfo.Name);
                var pattern = new Regex( $"<param +name=\"{name}\">([^<]*)</param>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match match = pattern.Match(memberDocumentation);
                if (match.Success)
                {
                    var txt = match.Groups[1].Value;
                    return RemoveBaseIndentation(txt);
                }
            }
            return "";
        }

        private static Regex baseIndent = new Regex(@"\A\n?(\s+)\S", RegexOptions.Compiled);
        public static String RemoveBaseIndentation(string text) {
            var match = baseIndent.Match(text);
            if (match.Success)
            {
                var padding = match.Groups[1].Value;
                text = Regex.Replace(text, $@"\n{padding}", "\n");
            }
            return text.Trim();
        }
        
        public static String ExtractXML(string text, string tag) {
            tag = Regex.Escape(tag);
            var pattern = new Regex($"<{tag}>(.*)</{tag}>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = pattern.Match(text);
            if (match.Success)
            {
                var txt = match.Groups[1].Value;
                return RemoveBaseIndentation(txt);
            }
            return "";
        }


        /*
            <member name="M:RazorEnhanced.Items.Move(RazorEnhanced.Item,RazorEnhanced.Mobile,System.Int32)">
            <member name="M:RazorEnhanced.Items.Move(System.Int32,RazorEnhanced.Mobile,System.Int32)">
        */


        public static string GetDocumentation(MethodInfo methodInfo)
        {
            string key = XMLKeyComposer.GetKey(methodInfo);
            loadedXmlDocumentation.TryGetValue(key, out string documentation);
            return documentation ?? "";
        }

        public static string GetDocumentation(ConstructorInfo constructorInfo)
        {
            string key = XMLKeyComposer.GetKey(constructorInfo);
            loadedXmlDocumentation.TryGetValue(key, out string documentation);
            return documentation ?? "";
        }

        
        public static string GetDocumentation(PropertyInfo propertyInfo)
        {
            string key = XMLKeyComposer.GetKey(propertyInfo);
            loadedXmlDocumentation.TryGetValue(key, out string documentation);
            return documentation ?? "";
        }

        public static string GetDocumentation(EventInfo eventInfo)
        {
            string key = XMLKeyComposer.GetKey(eventInfo);
            loadedXmlDocumentation.TryGetValue(key, out string documentation);
            return documentation ?? "";
        }

        public static string GetDocumentation(FieldInfo fieldInfo)
        {
            string key = XMLKeyComposer.GetKey(fieldInfo);
            loadedXmlDocumentation.TryGetValue(key, out string documentation);
            return documentation ?? "";
        }



    }




}
