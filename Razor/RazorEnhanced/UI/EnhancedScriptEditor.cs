using Assistant;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using FastColoredTextBoxNS;
using IronPython.Compiler;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading.Tasks;

namespace RazorEnhanced.UI
{
    internal partial class EnhancedScriptEditor : Form
    {
        private delegate void SetHighlightLineDelegate(int iline, Color color);

        private delegate void SetStatusLabelDelegate(string text, Color color);

        private delegate void SetRecordButtonDelegate(string text);

        private delegate string GetFastTextBoxTextDelegate();

        private delegate void SetTracebackDelegate(string text);

        private enum Command
        {
            None = 0,
            Line,
            Call,
            Return,
            Breakpoint
        }

        private static EnhancedScriptEditor m_EnhancedScriptEditor;
        internal static FastColoredTextBox EnhancedScriptEditorTextArea { get { return m_EnhancedScriptEditor.fastColoredTextBoxEditor; } }
        private static ConcurrentQueue<Command> m_Queue = new ConcurrentQueue<Command>();
        private static Command m_CurrentCommand = Command.None;
        private static readonly AutoResetEvent m_WaitDebug = new AutoResetEvent(false);

        private string Title {
            get
            {
                if (World.Player != null)
                {
                    if (m_Filename != String.Empty)
                        return String.Format("Enhanced Script Editor - ({0}) - {1} ({2})", m_Filename, World.Player.Name, World.ShardName);
                    else
                        return String.Format("Enhanced Script Editor - {0} ({1})", World.Player.Name, World.ShardName);
                }
                else
                    return "Enhanced Script Editor";
            }
        }

        private string m_Filename = String.Empty;
        private string m_Filepath = String.Empty;

        private readonly PythonEngine m_pe;

        private TraceBackFrame m_CurrentFrame;
        private FunctionCode m_CurrentCode;
        private string m_CurrentResult;
        private object m_CurrentPayload;
        private int m_ThreadID;

        private readonly List<int> m_Breakpoints = new List<int>();

        private volatile bool m_Breaktrace = false;
        private bool m_onclosing = false;

        private readonly FastColoredTextBoxNS.AutocompleteMenu m_popupMenu;

        internal static void Init(string filename)
        {
            m_EnhancedScriptEditor = new EnhancedScriptEditor(filename);
            m_EnhancedScriptEditor.Show();
        }

        internal static void End()
        {
            if (m_EnhancedScriptEditor != null)
            {
                if (ScriptRecorder.OnRecord)
                    ScriptRecorder.OnRecord = false;

                m_EnhancedScriptEditor.Stop();
            }
        }

        internal EnhancedScriptEditor(string filename)
        {
            InitializeComponent();
            //Automenu Section
            m_popupMenu = new AutocompleteMenu(fastColoredTextBoxEditor);
            m_popupMenu.Items.ImageList = imageList2;
            m_popupMenu.SearchPattern = @"[\w\.:=!<>]";
            m_popupMenu.AllowTabKey = true;
            m_popupMenu.ToolTipDuration = 5000;
            m_popupMenu.AppearInterval = 100;


            if (filename != null && Path.GetExtension(filename) == ".uos")
            {
                fastColoredTextBoxEditor.Language = FastColoredTextBoxNS.Language.Uos;
                fastColoredTextBoxEditor.AutoIndentExistingLines = true;
                InitUOSSyntaxHighlight();
            }
            else
            {
                fastColoredTextBoxEditor.Language = FastColoredTextBoxNS.Language.Python;
                InitPythonSyntaxHighlight();
            }

            // Always have to make these or Open() wont work from UOS to PY
            m_pe = new PythonEngine(this.SetErrorBox);
            m_pe.Engine.SetTrace(null);
            this.Text = Title;

            if (filename != null && File.Exists(filename))
            {
                m_Filepath = filename;
                m_Filename = Path.GetFileName(filename);
                this.Text = Title;
                fastColoredTextBoxEditor.Text = File.ReadAllText(filename);
            }
        }

        
        public void InitUOSSyntaxHighlight()
        {
            // keywords
            List<String> keywords = UOSteamEngine.Instance.AllKeywords();
            string[] syntax =
            {
                "and", "break", "continue", "elif", "else", "for", "if", "not", "or", "while", "true", "false",
                "endif", "endwhile", "endfor"
            };
            keywords.AddRange(syntax);
            String pattern = $@"\b({String.Join("|", keywords)})\b";
            this.fastColoredTextBoxEditor.SyntaxHighlighter.UosKeywordRegex = new Regex(pattern, RegexOptions.Compiled);

            // attributes
            List<String> aliases = UOSteamEngine.Instance.AllAliases();
            pattern = $@"\b({String.Join("|", aliases)})\b";
            this.fastColoredTextBoxEditor.SyntaxHighlighter.UosAttributeRegex = new Regex(pattern, RegexOptions.Compiled|RegexOptions.IgnoreCase);

            // Fill in autocomplete
            List<AutocompleteItem> items = new List<AutocompleteItem>();
            keywords = keywords.Distinct().ToList();
            keywords.Sort();
            foreach (var item in keywords)
            {
                items.Add(new AutocompleteItem(item) { ImageIndex = 0 });
            }
            m_popupMenu.Items.SetAutocompleteItems(items);

            //Increase the width for individual items so that the entire name is visible
            m_popupMenu.Items.MaximumSize = new Size(m_popupMenu.Items.Width + 400, m_popupMenu.Items.Height);
            m_popupMenu.Items.Width = m_popupMenu.Items.Width + 400;

            //
            ToolTipDescriptions tooltip;
            var methods = UOSteamEngine.Instance.AllKeywords().ToArray();
            var autodocMethods = new Dictionary<string, ToolTipDescriptions>();
            foreach (var method in keywords)
            {
                var methodName = method;
                var prms_name = new List<String>();
                var prms_type = new List<String>();
                var prms_name_type = new List<String>();
                var prms_name_type_desc = new List<String>();
                prms_name_type_desc.Add("Param1");
                prms_name_type_desc.Add("Param2");

                if (!autodocMethods.ContainsKey((string)method))
                {
                    //tooltip = new ToolTipDescriptions(method, prms_name_type_desc.ToArray(), method.returnType, method.itemDescription.Trim() + "\n");
                    tooltip = new ToolTipDescriptions((string)method, prms_name_type_desc.ToArray(), "ret", "desc" + "\n");
                    autodocMethods.Add((string)method, tooltip);
                }
            }



        }


        public void InitPythonSyntaxHighlight() {
            //Dalamar: Trying to inject SyntaxHighlight (and Autocomplete) from AutoDoc
            //TODO: make it work
            // # Syntax Highlight
            List<String> itemList;
            List<String> escaped;
            String pattern;
            // ## Classes
            itemList = AutoDoc.GetClasses();
            escaped = new List<String>();
            foreach (var name in itemList)
            {
                escaped.Add(Regex.Escape(name));
            }
            pattern = $@"\b({String.Join("|", escaped)})\b";
            this.fastColoredTextBoxEditor.SyntaxHighlighter.RazorClassKeywordRegex = new Regex(pattern, RegexOptions.Compiled);

            // ## Properties
            itemList = AutoDoc.GetProperties();
            escaped = new List<String>();
            foreach (var name in itemList)
            {
                escaped.Add(Regex.Escape(name));
            }
            pattern = $@"\b({String.Join("|", escaped)})\b";
            this.fastColoredTextBoxEditor.SyntaxHighlighter.RazorPropsKeywordRegex = new Regex(pattern, RegexOptions.Compiled);

            // ## Functions
            itemList = AutoDoc.GetMethods();
            escaped = new List<String>();
            foreach (var name in itemList)
            {
                escaped.Add(Regex.Escape(name));
            }
            pattern = $@"\b({String.Join("|", escaped)})\b";
            this.fastColoredTextBoxEditor.SyntaxHighlighter.RazorFunctionsKeywordRegex = new Regex(pattern, RegexOptions.Compiled);

            #region Keywords

            string[] keywords =
            {
                "and", "assert", "break", "class", "continue", "def", "del", "elif", "else", "except", "exec",
                "finally", "for", "from", "global", "if", "import", "in", "is", "lambda", "not", "or", "pass", "print",
                "raise", "return", "try", "while", "yield", "None", "True", "False", "as", "sorted", "filter"
            };

            #endregion

            #region Classes Autocomplete



            //Dalamar: AutoDoc
            string[] classes = AutoDoc.GetClasses().ToArray();

            //Dalamar: AutoDoc
            string[] methods = AutoDoc.GetMethods(true, true, false).ToArray();

            #region Props Autocomplete

            string[] propsPlayer =
            {
                "Player.StatCap", "Player.AR", "Player.FireResistance", "Player.ColdResistance", "Player.EnergyResistance",
                "Player.PoisonResistance", "Player.StaticMount",
                "Player.Buffs", "Player.IsGhost", "Player.Female", "Player.Name", "Player.Bank",
                "Player.Gold", "Player.Luck", "Player.Body", "Player.HasSpecial",
                "Player.Followers", "Player.FollowersMax", "Player.MaxWeight", "Player.Str", "Player.Dex", "Player.Int",
            };

            string[] propsPositions =
            {
                "Position.X", "Position.Y", "Position.Z"
            };

            string[] propsWithCheck = AutoDoc.GetProperties(true).Union(propsPlayer).Union(propsPositions).ToArray();

            string[] propsGeneric =
            {
                "Serial", "Hue", "Name", "Body", "Color", "Direction", "Visible", "Poisoned", "YellowHits", "Paralized",
                "Human", "WarMode", "Female", "Hits", "HitsMax", "Stam", "StamMax", "Mana", "ManaMax", "Backpack", "Mount",
                "Quiver", "Notoriety", "Map", "InParty", "Properties", "Amount", "IsBagOfSending", "IsContainer", "IsCorpse",
                "IsDoor", "IsInBank", "Movable", "OnGround", "ItemID", "RootContainer", "Container", "Durability", "MaxDurability",
                "Contains", "Weight", "Position", "StaticID", "StaticHue", "StaticZ", "Flying",

                // Item filter part
                "Enabled", "Graphics", "Hues", "RangeMin", "RangeMax", "Layers",
                // Mobiles
                "Bodies", "Notorieties", "CheckIgnoreObject",
                // PathFind
                "DebugMessage", "StopIfStuck", "MaxRetry", "Timeout"
            };

            string[] props = AutoDoc.GetProperties().Union(propsGeneric).ToArray();

            #endregion
            #endregion


            ToolTipDescriptions tooltip;

            var autodocMethods = new Dictionary<string, ToolTipDescriptions>();
            foreach (var docitem in AutoDoc.GetPythonAPI().methods)
            {
                var method = (DocMethod)docitem;
                var methodName = method.itemClass + "." + method.itemName;
                var prms_name = new List<String>();
                var prms_type = new List<String>();
                var prms_name_type = new List<String>();
                var prms_name_type_desc = new List<String>();
                foreach (var prm in method.paramList)
                {
                    prms_name.Add(prm.itemName);
                    prms_type.Add(prm.itemType);


                    string name_type = $"{prm.itemName}: {prm.itemType}";
                    prms_name_type.Add(name_type);

                    string name_type_desc = name_type;
                    if (prm.itemDescription.Trim().Length > 0)
                    {
                        name_type_desc += $"\n    {prm.itemDescription.Trim()}";
                    }
                    prms_name_type_desc.Add(name_type_desc);
                }
                var methodSignNames = $"{methodName}({String.Join(",", prms_name)})";
                var methodSignTypes = $"{methodName}({String.Join(",", prms_type)})";
                var methodSignNameTypes = $"{methodName}({String.Join(",", prms_name_type)})";

                var methodKey = methodSignNames;

                if (!autodocMethods.ContainsKey(methodKey))
                {
                    tooltip = new ToolTipDescriptions(methodSignNames, prms_name_type_desc.ToArray(), method.returnType, method.itemDescription.Trim() + "\n");
                    autodocMethods.Add(methodKey, tooltip);
                }
                else
                {
                    autodocMethods[methodKey].Notes += "\n" + methodSignNameTypes;
                    if (method.itemDescription.Length > 0)
                    {
                        autodocMethods[methodKey].Notes += "\n    " + method.itemDescription.Trim() + "\n";
                    }
                }

            }

            var descriptionMethods = autodocMethods.ToDictionary(x => x.Key, x => x.Value);


            List<AutocompleteItem> items = new List<AutocompleteItem>();

            //Permette la creazione del menu con la singola keyword
            Array.Sort(keywords);
            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item) { ImageIndex = 0 });
            //Permette la creazione del menu con la singola classe
            Array.Sort(classes);
            foreach (var item in classes)
                items.Add(new AutocompleteItem(item) { ImageIndex = 1 });

            //Permette di creare il menu solo per i metodi della classe digitata
            Array.Sort(methods);
            foreach (var item in methods)
            {
                descriptionMethods.TryGetValue(item, out ToolTipDescriptions element);

                if (element != null)
                {
                    items.Add(new MethodAutocompleteItemAdvance(item)
                    {
                        ImageIndex = 2,
                        ToolTipTitle = element.Title,
                        ToolTipText = element.ToolTipDescription()
                    });
                }
                else
                {
                    items.Add(new MethodAutocompleteItemAdvance(item)
                    {
                        ImageIndex = 2
                    });
                }
            }

            //Permette di creare il menu per le props solo sulla classe Player
            Array.Sort(propsWithCheck);
            foreach (var item in propsWithCheck)
                items.Add(new SubPropertiesAutocompleteItem(item) { ImageIndex = 4 });

            //Props generiche divise tra quelle Mobiles e Items, che possono
            //Appartenere a variabili istanziate di una certa classe
            //Qui sta alla cura dell'utente capire se una props va bene o no
            //Per quella istanza
            Array.Sort(props);
            foreach (var item in props)
                items.Add(new MethodAutocompleteItem(item) { ImageIndex = 5 });

            m_popupMenu.Items.SetAutocompleteItems(items);

            //Aumenta la larghezza per i singoli item, in modo che l'intero nome sia visibile
            m_popupMenu.Items.MaximumSize = new Size(m_popupMenu.Items.Width + 20, m_popupMenu.Items.Height);
            m_popupMenu.Items.Width = m_popupMenu.Items.Width + 20;

        }


        private TracebackDelegate OnTraceback(TraceBackFrame frame, string result, object payload)
        {
            if (m_Breaktrace)
            {
                m_WaitDebug.WaitOne();
                CheckCurrentCommand();

                if (m_CurrentCommand != Command.None)
                {
                    UpdateCurrentState(frame, result, payload);
                    int line = (int)m_CurrentFrame.f_lineno;

                    switch (m_CurrentCommand)
                    {
                        case Command.Breakpoint:

                            if (m_Breakpoints.Contains(line))
                                TracebackBreakpoint();
                            else
                                EnqueueCommand(Command.Breakpoint);
                            break;

                        case Command.Call:

                            if (result == "call")
                                TracebackCall();
                            else
                                EnqueueCommand(Command.Call);
                            break;

                        case Command.Line:

                            if (result == "line")
                                TracebackLine();
                            else
                                EnqueueCommand(Command.Line);
                            break;

                        case Command.Return:

                            if (result == "return")
                                TracebackReturn();
                            else
                                EnqueueCommand(Command.Return);
                            break;
                    }
                }

                return OnTraceback;
            }
            else
                return null;
        }

        private void TracebackCall()
        {
            SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Call {0}", m_CurrentCode.co_name), Color.YellowGreen);
            SetHighlightLine((int)m_CurrentFrame.f_lineno - 1, Color.LightGreen);
            string locals = GetLocalsText(m_CurrentFrame);
            SetTraceback(locals);
        }

        private void TracebackReturn()
        {
            SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Return {0}", m_CurrentCode.co_name), Color.YellowGreen);
            SetHighlightLine((int)m_CurrentFrame.f_lineno - 1, Color.LightBlue);
            string locals = GetLocalsText(m_CurrentFrame);
            SetTraceback(locals);
        }

        private void TracebackLine()
        {
            SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Line {0}", (int)m_CurrentFrame.f_lineno), Color.YellowGreen);
            SetHighlightLine((int)m_CurrentFrame.f_lineno - 1, Color.Yellow);
            string locals = GetLocalsText(m_CurrentFrame);
            SetTraceback(locals);
        }

        private void TracebackBreakpoint()
        {
            SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Breakpoint at line {0}", (int)m_CurrentFrame.f_lineno), Color.YellowGreen);
            string locals = GetLocalsText(m_CurrentFrame);
            SetTraceback(locals);
        }

        private void EnqueueCommand(Command command)
        {
            m_Queue.Enqueue(command);
            m_WaitDebug.Set();
        }

        private bool CheckCurrentCommand()
        {
            m_CurrentCommand = Command.None;
            bool result = m_Queue.TryDequeue(out m_CurrentCommand);
            return result;
        }

        private void UpdateCurrentState(TraceBackFrame frame, string result, object payload)
        {
            m_CurrentFrame = frame;
            m_CurrentCode = frame.f_code;
            m_CurrentResult = result;
            m_CurrentPayload = payload;
        }

        private void Start(bool debug)
        {
            if (World.Player == null)
            {
                SetErrorBox("Starting ERROR: Can't start script if not logged in game.");
                return;
            }

            if (Scripts.ScriptEditorThread == null ||
                    (Scripts.ScriptEditorThread != null && Scripts.ScriptEditorThread.ThreadState != ThreadState.Running &&
                    Scripts.ScriptEditorThread.ThreadState != ThreadState.Unstarted &&
                    Scripts.ScriptEditorThread.ThreadState != ThreadState.WaitSleepJoin)
                )
            {
                Scripts.ScriptEditorThread = new Thread(() => AsyncStart(debug));
                Scripts.ScriptEditorThread.Start();
                m_ThreadID = Scripts.ScriptEditorThread.ManagedThreadId;
            }
            else
                SetErrorBox("Starting ERROR: Can't start script if another editor is running.");
        }
        private void AsyncStart(bool debug)
        {
            if (ScriptRecorder.OnRecord)
            {
                SetErrorBox("Starting ERROR: Can't start script if record mode is ON.");
                return;
            }

            if (debug)
            {
                SetErrorBox("Starting Script in debug mode: " + m_Filename);
                SetStatusLabel("DEBUGGER ACTIVE", Color.YellowGreen);
            }
            else
            {
                SetErrorBox("Starting Script: " + m_Filename);
                SetStatusLabel("SCRIPT RUNNING", Color.Green);
            }

            try
            {
                if (debug)
                {
                    m_Breaktrace = true;
                }
                else
                {
                    m_Breaktrace = false;
                }

                m_Queue = new ConcurrentQueue<Command>();

                string text = GetFastTextBoxText();
                if (text.Length >= 4 && text.Substring(0, 4).ToUpper() == "//C#")
                {
                    if (m_Filepath == "")
                    {
                        SetErrorBox("Due to a limitation, C# scripts must be saved before run it");
                        throw new Exception();
                    } 
                    else
                    {
                        Save();
                        SetErrorBox(m_Filename + " saved");
                    }

                    CSharpEngine csharpEngine = CSharpEngine.Instance;

                    // Changed the logic: Now scripts are not executed as a text tring. Text will be saved and executed as a file.
                    // This change simplify alot the management of the #import directive. This behaviour should change in future maybe with a new editor
                    // 
                    // If compile error occurs a SyntaxErrorException is thrown
                    //bool compileErrors = csharpEngine.CompileFromText(text, out List<string> compileMessages, out Assembly assembly);
                    bool compileErrors = csharpEngine.CompileFromFile(m_Filepath, true, out List<string> compileMessages, out Assembly assembly);

                    if (compileMessages.Count > 0)
                    {
                        SetErrorBox("C# compile warning:");
                        foreach (string str in compileMessages)
                        {
                            SetErrorBox(str);
                        }
                    }
                    if (assembly != null)
                    {
                        csharpEngine.Execute(assembly);
                    }
                    else
                    {
                        throw new Exception();
                    }

                    SetErrorBox("Script " + m_Filename + " run completed!");
                    SetStatusLabel("IDLE", Color.DarkTurquoise);
                } 
                else if ((text.Length >= 2) && ((text.Substring(0, 2) == "//") || (text.Substring(0, 5).ToUpper() == "//UOS")) )   // you want it to be UOS it better start with UOS style comment
                {
                    // Deprecation of // 
                    if ((text.Substring(0, 2) == "//") && !(text.Substring(0, 5).ToUpper() == "//UOS"))
                    {
                        string message = "WARNING: // header for UOS scripts is going to be deprecated. Please use //UOS instead";
                        SetErrorBox(message);
                        Misc.SendMessage(message);
                    }
                    string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    UOSteamEngine uosteam = UOSteamEngine.Instance;
                    uosteam.Execute(lines);
                    SetErrorBox("Script " + m_Filename + " run completed!");
                    SetStatusLabel("IDLE", Color.DarkTurquoise);
                }
                else
                {

                    m_pe.Engine.SetTrace(m_EnhancedScriptEditor.OnTraceback);
                    m_pe.Execute(text);


                    SetErrorBox("Script " + m_Filename + " run completed!");
                    SetStatusLabel("IDLE", Color.DarkTurquoise);
                }
            }
            catch (IronPython.Runtime.Exceptions.SystemExitException )
            {
                Stop();
                // sys.exit - terminate the thread
            }
            catch (Exception ex)
            {
                if (ex is SyntaxErrorException)
                {
                    SyntaxErrorException se = ex as SyntaxErrorException;
                    SetErrorBox("Syntax Error:");
                    SetErrorBox("--> LINE: " + se.Line);
                    SetErrorBox("--> COLUMN: " + se.Column);
                    SetErrorBox("--> SEVERITY: " + se.Severity);
                    SetErrorBox("--> MESSAGE: " + se.Message);
                }
                else
                {
                    SetErrorBox("Generic Error:");
                    ExceptionOperations eo = m_pe.Engine.GetService<ExceptionOperations>();
                    string error = eo.FormatException(ex);
                    error = error.Trim();
                    error = Regex.Replace(error, "\n\n", "\n");     //remove empty lines
                    foreach (var line in error.Split('\n') ) {
                        SetErrorBox(line);
                    }
                }
                SetStatusLabel("IDLE", Color.DarkTurquoise);
            }

            if (Scripts.ScriptEditorThread != null)
                Scripts.ScriptEditorThread.Abort();
        }

        private void Stop()
        {
            if (ScriptRecorder.OnRecord)
                return;

            m_Breaktrace = false;
            m_Queue = new ConcurrentQueue<Command>();
            m_Breakpoints.Clear();

            for (int iline = 0; iline < fastColoredTextBoxEditor.LinesCount; iline++)
            {
                fastColoredTextBoxEditor[iline].BackgroundBrush = new SolidBrush(Color.White);
            }
            fastColoredTextBoxEditor.Invalidate();

            SetStatusLabel("IDLE", Color.DarkTurquoise);
            SetTraceback(String.Empty);

            if (Scripts.ScriptEditorThread != null && Scripts.ScriptEditorThread.ThreadState != ThreadState.Stopped && m_ThreadID == Scripts.ScriptEditorThread.ManagedThreadId)
            {
                try
                {
                    Scripts.ScriptEditorThread.Abort();
                }
                catch { }
                SetErrorBox("Script stopped: " + m_Filename);
                Scripts.ScriptEditorThread = null;
            }
        }

        private void SetHighlightLine(int iline, Color background)
        {
            if (this.m_onclosing)
                return;

            if (this.fastColoredTextBoxEditor.InvokeRequired)
            {
                SetHighlightLineDelegate d = new SetHighlightLineDelegate(SetHighlightLine);
                this.Invoke(d, new object[] { iline, background });
            }
            else
            {
                for (int i = 0; i < fastColoredTextBoxEditor.LinesCount; i++)
                {
                    if (m_Breakpoints.Contains(i))
                        fastColoredTextBoxEditor[i].BackgroundBrush = new SolidBrush(Color.Red);
                    else
                        fastColoredTextBoxEditor[i].BackgroundBrush = new SolidBrush(Color.White);
                }

                this.fastColoredTextBoxEditor[iline].BackgroundBrush = new SolidBrush(background);
                this.fastColoredTextBoxEditor.Invalidate();
            }
        }

        private void SetStatusLabel(string text, Color color)
        {
            if (this.m_onclosing || this.Disposing)
                return;
            try
            {
                if (this.InvokeRequired)
                {
                    SetStatusLabelDelegate d = new SetStatusLabelDelegate(SetStatusLabel);
                    this.Invoke(d, new object[] { text, color });
                }
                else
                {
                    this.toolStripStatusLabelScript.Text = "--> " + text;
                    this.statusStrip1.BackColor = color;

                }
            }
            catch { }
        }

        private void SetRecordButton(string text)
        {
            if (this.m_onclosing || this.Disposing)
                return;

            if (this.InvokeRequired)
            {
                SetRecordButtonDelegate d = new SetRecordButtonDelegate(SetRecordButton);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                toolStripButtonGumps.Text = text;
            }
        }

        private string GetFastTextBoxText()
        {
            if (this.fastColoredTextBoxEditor.InvokeRequired)
            {
                GetFastTextBoxTextDelegate d = new GetFastTextBoxTextDelegate(GetFastTextBoxText);
                return (string)this.Invoke(d, null);
            }
            else
            {
                return fastColoredTextBoxEditor.Text;
            }
        }

        private string GetLocalsText(TraceBackFrame frame)
        {
            string result = String.Empty;

            PythonDictionary locals = frame.f_locals as PythonDictionary;
            if (locals != null)
            {
                foreach (KeyValuePair<object, object> pair in locals)
                {
                    if (!(pair.Key.ToString().StartsWith("__") && pair.Key.ToString().EndsWith("__")))
                    {
                        string line = pair.Key.ToString() + ": " + (pair.Value != null ? pair.Value.ToString() : String.Empty) + "\r\n";
                        result += line;
                    }
                }
            }

            return result;
        }

        private void SetTraceback(string text)
        {
            if (this.m_onclosing)
                return;

            if (this.textBoxDebug.InvokeRequired)
            {
                SetTracebackDelegate d = new SetTracebackDelegate(SetTraceback);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxDebug.Text = text;
            }
        }

        private void SetErrorBox(string text)
        {
            if (this.m_onclosing)
                return;

            try
            {
                if (this.messagelistBox.InvokeRequired)
                {
                    SetTracebackDelegate d = new SetTracebackDelegate(SetErrorBox);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.messagelistBox.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] - " + text);
                    this.messagelistBox.TopIndex = this.messagelistBox.Items.Count - 1;
                }
            }
            catch
            { }
        }

        private void EnhancedScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_EnhancedScriptEditor.m_onclosing = true;
            Stop();
            End();
            if (!CloseAndSave())
                e.Cancel = true;
            m_EnhancedScriptEditor.m_onclosing = false;
        }

        private void ToolStripButtonPlay_Click(object sender, EventArgs e)
        {
            Start(false);
        }

        private void ToolStripButtonDebug_Click(object sender, EventArgs e)
        {
            Start(true);
        }

        private void ToolStripNextCall_Click(object sender, EventArgs e)
        {
            EnqueueCommand(Command.Call);
        }

        private void ToolStripButtonNextLine_Click(object sender, EventArgs e)
        {
            EnqueueCommand(Command.Line);
        }

        private void ToolStripButtonNextReturn_Click(object sender, EventArgs e)
        {
            EnqueueCommand(Command.Return);
        }

        private void ToolStripButtonNextBreakpoint_Click(object sender, EventArgs e)
        {
            EnqueueCommand(Command.Breakpoint);
        }

        private void ToolStripButtonStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void ToolStripButtonAddBreakpoint_Click(object sender, EventArgs e)
        {
            AddBreakpoint();
        }

        private void ToolStripButtonRemoveBreakpoints_Click(object sender, EventArgs e)
        {
            RemoveBreakpoint();
        }

        private void ToolStripButtonOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void ToolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void ToolStripButtonClose_Click(object sender, EventArgs e)
        {
            CloseAndSave();
        }

        private void ToolStripButtonInspect_Click(object sender, EventArgs e)
        {
            InspectEntities();
        }

        private void ToolStripInspectGump_Click(object sender, EventArgs e)
        {
            InspectGumps();
        }

        private void ToolStripRecord_Click(object sender, EventArgs e)
        {
            ScriptRecord();
        }

        private static void Gumpinspector_close(object sender, EventArgs e)
        {
            Assistant.Engine.MainWindow.GumpInspectorEnable = false;
        }

        private void ToolStripButtonSearch_Click(object sender, EventArgs e)
        {
            fastColoredTextBoxEditor.Focus();
            SendKeys.SendWait("^f");
        }
        private void ToolStripButtonWiki_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo("http://www.razorenhanced.net/dokuwiki");
            try
            {
                System.Diagnostics.Process.Start(p);
            }
            catch { }
        }
        private void Open()
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Script Files|*.py;*.txt;*.uos;*.cs",
                RestoreDirectory = true
            };
            if (open.ShowDialog() == DialogResult.OK)
            {
                if (open.FileName != null && File.Exists(open.FileName))
                {
                    m_Filename = Path.GetFileName(open.FileName);
                    m_Filepath = open.FileName;
                    this.Text = Title;
                    if (m_Filename != null && Path.GetExtension(m_Filename) == ".uos")
                    {
                        fastColoredTextBoxEditor.Language = FastColoredTextBoxNS.Language.Uos;
                        fastColoredTextBoxEditor.AutoIndentExistingLines = true;
                        InitUOSSyntaxHighlight();
                    }
                    else
                    {
                        fastColoredTextBoxEditor.Language = FastColoredTextBoxNS.Language.Python;
                        InitPythonSyntaxHighlight();
                    }

                    fastColoredTextBoxEditor.Text = File.ReadAllText(open.FileName);
                }
            }
        }

        private void ReloadAfterSave()
        {
            Scripts.EnhancedScript script = Scripts.Search(m_Filename);
            if (script != null)
            {
                string fullpath = Path.Combine(Assistant.Engine.RootPath, "Scripts", m_Filename);

                if (File.Exists(fullpath) && Scripts.EnhancedScripts.ContainsKey(m_Filename))
                {
                    //string text = File.ReadAllText(fullpath);
                    //bool loop = script.Loop;
                    //bool wait = script.Wait;
                    //bool run = script.Run;
                    //bool autostart = script.AutoStart;
                    bool isRunning = script.IsRunning;

                    if (isRunning)
                        script.Stop();

                    //Scripts.EnhancedScript reloaded = new Scripts.EnhancedScript(m_Filename, text, wait, loop, run, autostart);
                    //reloaded.Create(null);
                    Scripts.EnhancedScripts[m_Filename].FileChangeDate = DateTime.MinValue;

                    if (isRunning)
                        script.Start();
                }
            }
        }

        private void SavaData()
        {
            try // Avoid crash if for some reasons file are unaccessible.
            {
                File.WriteAllText(m_Filepath, fastColoredTextBoxEditor.Text);
            }
            catch { }
        }

        private void Save()
        {
            if (m_Filename != String.Empty)
            {
                this.Text = Title;

                SavaData();

                ReloadAfterSave();
            }
            else
            {
                SaveAs();
            }
        }

        private void SaveAs()
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "Python Files|*.py|Script Files|*.txt|UOSteam Files|*.uos|C# Files|*.cs",
                RestoreDirectory = true
            };
            save.InitialDirectory = Path.Combine(Assistant.Engine.RootPath, "Scripts");
            if (save.ShowDialog() == DialogResult.OK)
            {
                m_Filename = Path.GetFileName(save.FileName);
                this.Text = Title;
                m_Filepath = save.FileName;
                m_Filename = Path.GetFileName(save.FileName);
                SavaData();
                ReloadAfterSave();
            }
        }

        private bool CloseAndSave()
        {
            if (File.Exists(m_Filepath) && File.ReadAllText(m_Filepath) == fastColoredTextBoxEditor.Text)
            {
                fastColoredTextBoxEditor.Text = String.Empty;
                m_Filename = String.Empty;
                m_Filepath = String.Empty;
                this.Text = Title;
                return true;
            }

            if (fastColoredTextBoxEditor.Text == String.Empty) // Not ask to save empty text
                return true;

            DialogResult res = MessageBox.Show("Save current file?", "WARNING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                if (m_Filename != null && m_Filename != String.Empty)
                {
                    SavaData();
                    ReloadAfterSave();
                }
                else
                {
                    SaveFileDialog save = new SaveFileDialog
                    {
                        Filter = "Script Files|*.py|Script Files|*.txt|C# Files|*.cs",
                        FileName = m_Filename
                    };

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        if (save.FileName != null && save.FileName != string.Empty && fastColoredTextBoxEditor.Text != null)
                        {
                            SavaData();
                            m_Filename = save.FileName;
                            ReloadAfterSave();
                        }
                    }
                    else
                        return false;
                }

                fastColoredTextBoxEditor.Text = String.Empty;
                m_Filename = String.Empty;
                m_Filepath = String.Empty;
                this.Text = Title;
                return true;
            }
            else if (res == System.Windows.Forms.DialogResult.No)
            {
                fastColoredTextBoxEditor.Text = String.Empty;
                m_Filename = String.Empty;
                m_Filepath = String.Empty;
                this.Text = Title;
                return true;
            }
            else if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                return false;
            }
            return true;
        }

        private void AddBreakpoint()
        {
            int iline = fastColoredTextBoxEditor.Selection.Start.iLine;

            if (!m_Breakpoints.Contains(iline))
            {
                m_Breakpoints.Add(iline + 1);
                FastColoredTextBoxNS.Line line = fastColoredTextBoxEditor[iline];
                line.BackgroundBrush = new SolidBrush(Color.Red);
                fastColoredTextBoxEditor.Invalidate();
            }
        }

        private void RemoveBreakpoint()
        {
            int iline = fastColoredTextBoxEditor.Selection.Start.iLine;

            if (m_Breakpoints.Contains(iline + 1))
            {
                m_Breakpoints.Remove(iline + 1);
                FastColoredTextBoxNS.Line line = fastColoredTextBoxEditor[iline];
                line.BackgroundBrush = new SolidBrush(Color.White);
                fastColoredTextBoxEditor.Invalidate();
            }
        }

        private void InspectEntities()
        {
            Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback(Commands.GetInfoTarget_Callback));
        }

        internal static void InspectGumps()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is EnhancedGumpInspector af)
                {
                    af.Focus();
                    return;
                }
            }
            EnhancedGumpInspector ginspector = new EnhancedGumpInspector();
            ginspector.FormClosed += new FormClosedEventHandler(Gumpinspector_close);
            ginspector.TopMost = true;
            ginspector.Show();
        }

        private void ScriptRecord()
        {
            if (Scripts.ScriptEditorThread == null ||
                    (Scripts.ScriptEditorThread != null && Scripts.ScriptEditorThread.ThreadState != ThreadState.Running &&
                    Scripts.ScriptEditorThread.ThreadState != ThreadState.Unstarted &&
                    Scripts.ScriptEditorThread.ThreadState != ThreadState.WaitSleepJoin)
                )
            {
                if (ScriptRecorder.OnRecord)
                {
                    SetErrorBox("RECORDER: Stop Record");
                    ScriptRecorder.OnRecord = false;
                    SetStatusLabel("IDLE", Color.DarkTurquoise);
                    SetRecordButton("Record");
                    return;
                }
                else
                {
                    SetErrorBox("RECORDER: Start Record");
                    ScriptRecorder.OnRecord = true;
                    SetStatusLabel("ON RECORD", Color.Red);
                    SetRecordButton("Stop Record");
                    return;
                }
            }
            else
            {
                SetErrorBox("RECORDER ERROR: Can't Record if script is running");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                //Open File
                case (Keys.Control | Keys.O):
                    Open();
                    return true;

                //Save File
                case (Keys.Control | Keys.S):
                    Save();
                    return true;

                //Save As File
                case (Keys.Control | Keys.Shift | Keys.S):
                    SaveAs();
                    return true;

                //Close File
                case (Keys.Control | Keys.E):
                    CloseAndSave();
                    return true;

                //Inspect Entities
                case (Keys.Control | Keys.I):
                    InspectEntities();
                    return true;

                //Inspect Gumps
                case (Keys.Control | Keys.G):
                    InspectGumps();
                    return true;

                case (Keys.Control | Keys.R):
                    ScriptRecord();
                    return true;

                //Start with Debug
                case (Keys.F5):
                    Start(true);
                    return true;

                //Start without Debug
                case (Keys.F6):
                    Start(false);
                    return true;

                //Stop
                case (Keys.F4):
                    Stop();
                    return true;

                //Add Breakpoint
                case (Keys.F7):
                    AddBreakpoint();
                    return true;

                //Remove Breakpoint
                case (Keys.F8):
                    RemoveBreakpoint();
                    return true;

                //Next Breakpoint
                case (Keys.F9):
                    EnqueueCommand(Command.Breakpoint);
                    return true;

                //Debug - Next Line
                case (Keys.F10):
                    EnqueueCommand(Command.Line);
                    return true;

                //Debug - Next Call
                case (Keys.F11):
                    EnqueueCommand(Command.Call);
                    return true;

                //Debug - Next Return
                case (Keys.F12):
                    EnqueueCommand(Command.Return);
                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void EnhancedScriptEditor_Load(object sender, EventArgs e)
        {
            toolStripStatusLabelScript.Width = this.Width - 20;
            SetStatusLabel("IDLE", Color.DarkTurquoise);
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBoxEditor.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBoxEditor.Paste();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBoxEditor.Cut();
        }

        private void CommentSelectLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fastColoredTextBoxEditor.SelectedText)) // No selection
                return;

            string[] lines = fastColoredTextBoxEditor.SelectedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None );

            fastColoredTextBoxEditor.SelectedText = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                fastColoredTextBoxEditor.SelectedText += "#" + lines[i];
                if (i < lines.Count() -1)
                    fastColoredTextBoxEditor.SelectedText += "\r\n";
            }
        }

        private void UnCommentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fastColoredTextBoxEditor.SelectedText)) // No selection
                return;

            string[] lines = fastColoredTextBoxEditor.SelectedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            fastColoredTextBoxEditor.SelectedText = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                fastColoredTextBoxEditor.SelectedText += lines[i].TrimStart('#');
                if (i < lines.Count() - 1)
                    fastColoredTextBoxEditor.SelectedText += "\r\n";
            }
        }

        private void MessagelistBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (messagelistBox.SelectedItems == null) // Nothing selected
                return;

            if (e.Control && e.KeyCode == Keys.C)
            {
                Utility.ClipBoardCopy(String.Join(Environment.NewLine, messagelistBox.SelectedItems.Cast<string>()));
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            messagelistBox.Items.Clear();
        }

        private void CopyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (messagelistBox.SelectedItems == null) // Nothing selected
                return;

            Utility.ClipBoardCopy(String.Join(Environment.NewLine, messagelistBox.SelectedItems.Cast<string>()));
        }

        private void ToolStripInspectAlias_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is EnhancedObjectInspector af)
                {
                    af.Focus();
                    return;
                }
            }
            new EnhancedObjectInspector().Show();
        }
    }

    public class ToolTipDescriptions
    {
        public string Title;
        public string[] Parameters;
        public string Returns;
        public string Description;
        public string Notes;

        public ToolTipDescriptions(string title, string[] parameter, string returns, string description, string notes="")
        {
            Title = title;
            Parameters = parameter;
            Returns = returns;
            Description = description;
            Notes = notes;

        }

        public string ToolTipDescription()
        {
            string complete_description = "";

            //Description
            if (Description.Trim().Length > 0)
            {
                complete_description += Description.Trim() + "\n";
            }

            //Parameters
            complete_description += "\nParameters: ";
            if (Parameters.Length > 0)
            {
                complete_description += "\n" + String.Join("\n", Parameters.Select(text=>"- "+text));
            }
            else {
                complete_description += "None";
            }
            complete_description += "\n";

            //Return
            complete_description += $"\nReturns: {Returns}";

            //Notes
            if (Notes.Length > 0){
                complete_description += "\n---\n" + Notes;
            }
            return complete_description;
        }
    }

    #region Custom Items per Autocomplete

    /// <summary>
    /// This autocomplete item appears after dot
    /// </summary>
    public class MethodAutocompleteItemAdvance : MethodAutocompleteItem
    {
        readonly string firstPart;
        readonly string lastPart;

        public MethodAutocompleteItemAdvance(string text)
            : base(text)
        {
            var i = text.LastIndexOf('.');
            if (i < 0)
                firstPart = text;
            else
            {
                firstPart = text.Substring(0, i);
                lastPart = text.Substring(i + 1);
            }
        }

        public override CompareResult Compare(string fragmentText)
        {
            int i = fragmentText.LastIndexOf('.');

            if (i < 0)
            {
                if (firstPart.StartsWith(fragmentText) && string.IsNullOrEmpty(lastPart))
                    return CompareResult.VisibleAndSelected;
                //if (firstPart.ToLower().Contains(fragmentText.ToLower()))
                //  return CompareResult.Visible;
            }
            else
            {
                var fragmentFirstPart = fragmentText.Substring(0, i);
                var fragmentLastPart = fragmentText.Substring(i + 1);


                if (firstPart != fragmentFirstPart)
                    return CompareResult.Hidden;

                if (lastPart != null && lastPart.StartsWith(fragmentLastPart))
                    return CompareResult.VisibleAndSelected;

                if (lastPart != null && lastPart.ToLower().Contains(fragmentLastPart.ToLower()))
                    return CompareResult.Visible;

            }

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            if (lastPart == null)
                return firstPart;

            return firstPart + "." + lastPart;
        }

        public override string ToString()
        {
            if (lastPart == null)
                return firstPart;

            return lastPart;
        }
    }

    /// <summary>
    /// This autocomplete item appears after dot
    /// </summary>
    public class SubPropertiesAutocompleteItem : MethodAutocompleteItem
    {
        readonly string firstPart;
        readonly string lastPart;

        public SubPropertiesAutocompleteItem(string text)
            : base(text)
        {
            var i = text.LastIndexOf('.');
            if (i < 0)
                firstPart = text;
            else
            {
                var keywords = text.Split('.');
                if (keywords.Length >= 2)
                {
                    firstPart = keywords[keywords.Length - 2];
                    lastPart = keywords[keywords.Length - 1];
                }
                else
                {
                    firstPart = text.Substring(0, i);
                    lastPart = text.Substring(i + 1, text.Length);
                }
            }
        }

        public override CompareResult Compare(string fragmentText)
        {
            int i = fragmentText.LastIndexOf('.');

            if (i < 0)
            {
                if (firstPart.StartsWith(fragmentText) && string.IsNullOrEmpty(lastPart))
                    return CompareResult.VisibleAndSelected;
                //if (firstPart.ToLower().Contains(fragmentText.ToLower()))
                //  return CompareResult.Visible;
            }
            else
            {
                var keywords = fragmentText.Split('.');
                if (keywords.Length >= 2)
                {
                    var fragmentFirstPart = keywords[keywords.Length - 2];
                    var fragmentLastPart = keywords[keywords.Length - 1];


                    if (firstPart != fragmentFirstPart)
                        return CompareResult.Hidden;

                    if (lastPart != null && lastPart.StartsWith(fragmentLastPart))
                        return CompareResult.VisibleAndSelected;

                    if (lastPart != null && lastPart.ToLower().Contains(fragmentLastPart.ToLower()))
                        return CompareResult.Visible;
                }
                else
                {
                    var fragmentFirstPart = fragmentText.Substring(0, i);
                    var fragmentLastPart = fragmentText.Substring(i + 1);


                    if (firstPart != fragmentFirstPart)
                        return CompareResult.Hidden;

                    if (lastPart != null && lastPart.StartsWith(fragmentLastPart))
                        return CompareResult.VisibleAndSelected;

                    if (lastPart != null && lastPart.ToLower().Contains(fragmentLastPart.ToLower()))
                        return CompareResult.Visible;
                }

            }

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            if (lastPart == null)
                return firstPart;

            return firstPart + "." + lastPart;
        }

        public override string ToString()
        {
            if (lastPart == null)
                return firstPart;

            return lastPart;
        }
    }

    #endregion
}
