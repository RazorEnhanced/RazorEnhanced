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
using System.Windows.Forms.VisualStyles;
using FastColoredTextBoxNS;

namespace RazorEnhanced.UI
{
	internal partial class EnhancedScriptEditor : Form
	{
		private delegate void SetHighlightLineDelegate(int iline, Color color);

		private delegate void SetStatusLabelDelegate(string text);

		private delegate string GetFastTextBoxTextDelegate();

		private delegate void SetTracebackDelegate(string text);

		private enum Command
		{
			None = 0,
			Line,
			Call,
			Return
		}

		private static Thread m_Thread;

		private static EnhancedScriptEditor m_EnhancedScriptEditor;
		private static ConcurrentQueue<Command> m_Queue = new ConcurrentQueue<Command>();
		private static Command m_CurrentCommand = Command.None;
		private static AutoResetEvent m_DebugContinue = new AutoResetEvent(false);

		private const string m_Title = "Enhanced Script Editor";
		private string m_Filename = "";
		private string m_Filepath = "";
		private static bool m_OnClosing = false;

		private ScriptEngine m_Engine;
		private ScriptSource m_Source;
		private ScriptScope m_Scope;

		private TraceBackFrame m_CurrentFrame;
		private FunctionCode m_CurrentCode;
		private string m_CurrentResult;
		private object m_CurrentPayload;

		private List<int> m_Breakpoints = new List<int>();

		private volatile bool m_Breaktrace = false;

	    private FastColoredTextBoxNS.AutocompleteMenu m_popupMenu;

		internal static void Init(string filename)
		{
			ScriptEngine engine = Python.CreateEngine();
			m_EnhancedScriptEditor = new EnhancedScriptEditor(engine, filename);
			m_EnhancedScriptEditor.Show();
		}

		internal static void End()
		{
			if (m_EnhancedScriptEditor != null)
			{
				m_OnClosing = true;
                m_EnhancedScriptEditor.Stop();
				//m_EnhancedScriptEditor.Close();
				//m_EnhancedScriptEditor.Dispose();
			}
		}

		internal EnhancedScriptEditor(ScriptEngine engine, string filename)
		{
			InitializeComponent();

            //Automenu Section
            m_popupMenu = new AutocompleteMenu(fastColoredTextBoxEditor);
            m_popupMenu.Items.ImageList = imageList2;
            m_popupMenu.SearchPattern = @"[\w\.:=!<>]";
            m_popupMenu.AllowTabKey = true;

            #region Keywords

            string[] keywords =
		    {
		        "and", "assert", "break", "class", "continue", "def", "del", "elif", "else", "except", "exec",
		        "finally", "for", "from", "global", "if", "import", "in", "is", "lambda", "not", "or", "pass", "print",
		        "raise", "return", "try", "while", "yield", "None", "True", "False", "as"
		    };

            #endregion

            #region Classes Autocomplete

            string[] classes =
		    {
		        "Player", "Spells", "Mobile", "Mobiles", "Item", "Items", "Misc", "Target", "Gumps", "Journal",
		        "AutoLoot", "Scavenger", "Organizer", "Restock", "SellAgent", "BuyAgent", "Dress", "Friend", "BandageHeal",
                "Statics"
		    };

            #endregion

            #region Methods Autocomplete

            string[] methodsPlayer =
		    {
		        "Player.BuffsExist", "Player.GetBuffDescription",
		        "Player.HeadMessage", "Player.InRangeMobile", "Player.InRangeItem", "Player.GetItemOnLayer",
		        "Player.UnEquipItemByLayer", "Player.EquipItem", "Player.CheckLayer", "Player.GetAssistantLayer",
		        "Player.GetSkillValue", "Player.GetSkillCap", "Player.GetSkillStatus", "Player.UseSkill", "Player.ChatSay",
		        "Player.ChatEmote", "Player.ChatWhisper",
		        "Player.ChatYell", "Player.ChatGuild", "Player.ChatAlliance", "Player.SetWarMode", "Player.Attack",
		        "Player.AttackLast", "Player.InParty", "Player.ChatParty",
		        "Player.PartyCanLoot", "Player.PartyInvite", "Player.PartyLeave", "Player.KickMember", "Player.InvokeVirtue",
		        "Player.Walk", "Player.PathFindTo", "Player.QuestButton",
		        "Player.GuildButton", "Player.WeaponPrimarySA", "Player.WeaponSecondarySA", "Player.WeaponClearSA",
		        "Player.WeaponStunSA", "Player.WeaponDisarmSA"
		    };

		    string[] methodsSpells =
		    {
                "Spells.CastMagery", "Spells.CastNecro", "Spells.CastChivalry", "Spells.CastBushido", "Spells.CastNinjitsu", "Spells.CastSpellweaving", "Spells.CastMysticism"
            };

		    string[] methodsMobiles =
		    {
		        "Mobile.GetItemOnLayer", "Mobile.GetAssistantLayer", "Mobiles.FindBySerial", "Mobiles.UseMobile",
		        "Mobiles.ApplyFilter", "Mobiles.Message", "Mobiles.WaitForProps", "Mobiles.GetPropValue",
		        "Mobiles.GetPropStringByIndex", "Mobiles.GetPropStringList"
		    };

		    string[] methodsItems =
		    {
		        "Items.FindBySerial", "Items.Move", "Items.DropItemOnGroundSelf", "Items.UseItem", "Items.WaitForProps",
		        "Items.WaitForProps", "Items.GetPropValue", "Items.GetPropStringByIndex", "Items.GetPropStringList",
		        "Items.WaitForContents",
		        "Items.Message", "Items.ApplyFilter", "Items.BackpackCount", "Items.ContainerCount"
		    };

		    string[] methodsMisc =
		    {
		        "Misc.SendMessage", "Misc.Resync", "Misc.Pause", "Misc.Beep", "Misc.Disconnect", "Misc.WaitForContext",
		        "Misc.ContextReply", "Misc.ReadSharedValue", "Misc.RemoveSharedValue", "Misc.CheckSharedValue",
		        "Misc.SetSharedValue",
		        "Misc.HasMenu", "Misc.CloseMenu", "Misc.MenuContains", "Misc.GetMenuTitle", "Misc.WaitForMenu",
		        "Misc.MenuResponse", "Misc.HasQueryString",
		        "Misc.WaitForQueryString", "Misc.QueryStringResponse", "Misc.NoOperation", "Misc.ScriptRun", "Misc.ScriptStop",
		        "Misc.ScriptStatus", "Misc.PetRename"
		    };

		    string[] methodsTarget =
		    {
                "Target.HasTarget", "Target.GetLast", "Target.GetLastAttack", "Target.WaitForTarget", "Target.TargetExecute", "Target.Cancel", "Target.Last", "Target.LastQueued",
                "Target.Self", "Target.SelfQueued", "Target.SetLast", "Target.ClearLast", "Target.ClearQueue", "Target.ClearLastandQueue", "Target.SetLastTargetFromList",
                "Target.PerformTargetFromList", "Target.AttackTargetFromList"
            };

		    string[] methodsJournal =
		    {
		        "Journal.Clear", "Journal.Search", "Journal.SearchByName", "Journal.SearchByColor",
		        "Journal.SearchByType", "Journal.GetLineText", "Journal.GetSpeechName", "Journal.WaitJournal"
		    };

		    string[] methodsAutoLoot =
		    {
                "AutoLoot.Status", "AutoLoot.Start", "AutoLoot.Stop", "AutoLoot.ChangeList", "Scavenger.RunOnce"
            };

		    string[] methodsScavenger =
		    {
                "Scavenger.Status", "Scavenger.Start", "Scavenger.Stop", "Scavenger.ChangeList", "Scavenger.RunOnce"
            };

            string[] methodsRestock =
            {
                "Restock.Status", "Restock.FStart", "Restock.FStop", "Restock.ChangeList"
            };

            string[] methodsSellAgent =
            {
                "SellAgent.Status", "SellAgent.Enable", "SellAgent.Disable", "SellAgent.ChangeList"
            };

            string[] methodsBuyAgent =
            {
                "BuyAgent.Status", "BuyAgent.Enable", "BuyAgent.Disable", "BuyAgent.ChangeList"
            };

		    string[] methodsDress =
		    {
                "Dress.DessStatus", "Dress.UnDressStatus", "Dress.DressFStart", "Dress.UnDressFStart", "Dress.DressFStop", "Dress.UnDressFStop", "Dress.ChangeList"
            };

		    string[] methodsFriend =
		    {
                "Friend.IsFriend", "Friend.ChangeList"
		    };

            string[] methodsBandageHeal =
            {
                "BandageHeal.Status", "BandageHeal.Start", "BandageHeal.Stop"
            };

		    string[] methodsStatics =
		    {
                "Statics.GetLandID", "Statics.GetLandZ", "Statics.GetStaticsTileInfo"
            };

		    string[] methods =
		        methodsPlayer.Union(methodsSpells)
		            .Union(methodsMobiles)
		            .Union(methodsItems)
		            .Union(methodsMisc)
		            .Union(methodsTarget)
		            .Union(methodsJournal)
		            .Union(methodsAutoLoot)
		            .Union(methodsScavenger)
		            .Union(methodsRestock)
		            .Union(methodsSellAgent)
		            .Union(methodsBuyAgent)
		            .Union(methodsDress)
		            .Union(methodsFriend)
		            .Union(methodsBandageHeal)
                    .Union(methodsStatics)
		            .ToArray();

            #endregion

            #region Props Autocomplete

		    string[] propsPlayer =
		    {
		        "Player.StatCap", "Player.AR", "Player.FireResistance", "Player.ColdResistance", "Player.EnergyResistance",
		        "Player.PoisonResistance",
		        "Player.Buffs", "Player.IsGhost", "Player.Female", "Player.Name", "Player.Bankbox",
		        "Player.Gold", "Player.Luck", "Player.Body",
		        "Player.Followers", "Player.FollowersMax", "Player.MaxWeight"
		    };

		    string[] propsGeneric =
		    {
		        "Serial", "Hue", "Name", "Body", "Color", "Direction", "Visible", "Poisoned", "YellowHits", "Paralized",
		        "Human", "WarMode", "Female", "Hits", "MaxHits", "Stam", "StamMax", "Mana", "ManaMax", "Backpack", "Mount",
		        "Quiver", "Notoriety", "Map", "InParty", "Properties", "Amount", "IsBagOfSending", "IsContainer", "IsCorpse",
		        "IsDoor", "IsInBank", "Movable", "OnGround", "ItemID", "RootContainer", "Durability", "MaxDurability",
		        "Contains", "Weight"
		    };

            string[] props = propsGeneric;

            #endregion

            List<AutocompleteItem> items = new List<AutocompleteItem>();

		    foreach (var item in keywords)
                items.Add(new AutocompleteItem(item) { ImageIndex = 0 });

		    foreach (var item in classes)
		        items.Add(new AutocompleteItem(item) { ImageIndex = 1 });

            foreach (var item in methods)
                items.Add(new MethodAutocompleteItemAdvance(item) { ImageIndex = 2 });

		    foreach (var item in propsPlayer)
		        items.Add(new MethodAutocompleteItemAdvance(item) { ImageIndex = 3 });

            foreach (var item in props)
                items.Add(new MethodAutocompleteItem(item) { ImageIndex = 3 });

            m_popupMenu.Items.SetAutocompleteItems(items);

            this.Text = m_Title;
			this.m_Engine = engine;
			this.m_Engine.SetTrace(null);

			if (filename != null)
			{
				m_Filepath = filename;
                m_Filename = Path.GetFileNameWithoutExtension(filename);
				this.Text = m_Title + " - " + m_Filename + ".cs";
				fastColoredTextBoxEditor.Text = File.ReadAllText(filename);
			}
		}

		private TracebackDelegate OnTraceback(TraceBackFrame frame, string result, object payload)
		{
			if (m_Breaktrace)
			{
				CheckCurrentCommand();

				if (m_CurrentCommand == Command.None)
				{
					SetTraceback("");
					m_DebugContinue.WaitOne();
				}
				else
				{
					UpdateCurrentState(frame, result, payload);
					int line = (int)m_CurrentFrame.f_lineno;

					if (m_Breakpoints.Contains(line))
					{
						TracebackBreakpoint();
					}
					else if (result == "call" && m_CurrentCommand == Command.Call)
					{
						TracebackCall();
					}
					else if (result == "line" && m_CurrentCommand == Command.Line)
					{
						TracebackLine();
					}
					else if (result == "return" && m_CurrentCommand == Command.Return)
					{
						TracebackReturn();
					}
				}

				return OnTraceback;
			}
			else
				return null;
		}

		private void TracebackCall()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Call {0}", m_CurrentCode.co_name));
			SetHighlightLine((int)m_CurrentFrame.f_lineno - 1, Color.LightGreen);
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
			ResetCurrentCommand();
		}

		private void TracebackReturn()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Return {0}", m_CurrentCode.co_name));
			SetHighlightLine((int)m_CurrentFrame.f_lineno - 1, Color.LightBlue);
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
			ResetCurrentCommand();
		}

		private void TracebackLine()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Line {0}", m_CurrentFrame.f_lineno));
			SetHighlightLine((int)m_CurrentFrame.f_lineno - 1, Color.Yellow);
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
			ResetCurrentCommand();
		}

		private void TracebackBreakpoint()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Breakpoint at line {0}", m_CurrentFrame.f_lineno));
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
			ResetCurrentCommand();
		}

		private void EnqueueCommand(Command command)
		{
			m_Queue.Enqueue(command);
			m_DebugContinue.Set();
		}

		private bool CheckCurrentCommand()
		{
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

		private void ResetCurrentCommand()
		{
			m_CurrentCommand = Command.None;
			m_DebugContinue.WaitOne();
		}

		private void Start(bool debug)
		{
			if (m_Thread == null ||
					(m_Thread != null && m_Thread.ThreadState != ThreadState.Running &&
					m_Thread.ThreadState != ThreadState.Unstarted &&
					m_Thread.ThreadState != ThreadState.WaitSleepJoin)
				)
			{
				m_Thread = new Thread(() => AsyncStart(debug));
				m_Thread.Start();
			}
		}

		private void AsyncStart(bool debug)
		{
			if (debug)
			{
                SetErrorBox("Starting Script in debug mode: " + m_Filename);
                SetStatusLabel("DEBUGGER ACTIVE");
			}
			else
			{
				SetErrorBox("Starting Script: " + m_Filename);
				SetStatusLabel("");
			}

			try
			{
				m_Breaktrace = debug;
				string text = GetFastTextBoxText();
				m_Source = m_Engine.CreateScriptSourceFromString(text);
				m_Scope = RazorEnhanced.Scripts.GetRazorScope(m_Engine);
				m_Engine.SetTrace(m_EnhancedScriptEditor.OnTraceback);
				m_Source.Execute(m_Scope);
				SetErrorBox("Script " + m_Filename + " run completed!");
			}
			catch (Exception ex)
			{
				if (!m_OnClosing)
				{
					if (ex is SyntaxErrorException)
					{
						SyntaxErrorException se = ex as SyntaxErrorException;
						SetErrorBox("Syntax Error:");
						SetErrorBox("--> LINE: " + se.Line);
						SetErrorBox("--> COLUMN: " + se.Column);
						SetErrorBox("--> SEVERITY: " + se.Severity);
						SetErrorBox("--> MESSAGE: " + se.Message);
						//MessageBox.Show("LINE: " + se.Line + "\nCOLUMN: " + se.Column + "\nSEVERITY: " + se.Severity + "\nMESSAGE: " + ex.Message, "Syntax Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						SetErrorBox("Generic Error:");
						SetErrorBox("--> MESSAGE: " + ex.Message);
						//MessageBox.Show("MESSAGE: " + ex.Message, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}

				if (m_Thread != null)
					m_Thread.Abort();
			}
		}

		private void Stop()
		{
			m_Breaktrace = false;
			m_DebugContinue.Set();

			for (int iline = 0; iline < fastColoredTextBoxEditor.LinesCount; iline++)
			{
				fastColoredTextBoxEditor[iline].BackgroundBrush = new SolidBrush(Color.White);
			}

			SetStatusLabel("");
			SetTraceback("");

			if (m_Thread != null && m_Thread.ThreadState != ThreadState.Stopped)
			{
				m_Thread.Abort();
				m_Thread = null;
                SetErrorBox("Stop Script: " + m_Filename);
            }
		}

		private void SetHighlightLine(int iline, Color background)
		{
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

		private void SetStatusLabel(string text)
		{
			if (this.InvokeRequired)
			{
				SetStatusLabelDelegate d = new SetStatusLabelDelegate(SetStatusLabel);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				this.toolStripStatusLabelScript.Text = text;
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
			string result = "";

			PythonDictionary locals = frame.f_locals as PythonDictionary;
			if (locals != null)
			{
				foreach (KeyValuePair<object, object> pair in locals)
				{
					if (!(pair.Key.ToString().StartsWith("__") && pair.Key.ToString().EndsWith("__")))
					{
						string line = pair.Key.ToString() + ": " + (pair.Value != null ? pair.Value.ToString() : "") + "\r\n";
						result += line;
					}
				}
			}

			return result;
		}

		private void SetTraceback(string text)
		{
			if (m_OnClosing)
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
			if (this.listBox1.InvokeRequired)
			{
				SetTracebackDelegate d = new SetTracebackDelegate(SetErrorBox);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				this.listBox1.Items.Add("- " + text);
				this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
			}
		}

		private void scintillaEditor_TextChanged(object sender, EventArgs e)
		{
			Stop();
		}

		private void EnhancedScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			Stop();
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			Start(false);
		}

		private void toolStripButtonDebug_Click(object sender, EventArgs e)
		{
			Start(true);
		}

		private void toolStripNextCall_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Call);
		}

		private void toolStripButtonNextLine_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Line);
		}

		private void toolStripButtonNextReturn_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Return);
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			Stop();
		}

		private void toolStripButtonAddBreakpoint_Click(object sender, EventArgs e)
		{
            AddBreakpoint();
		}

		private void toolStripButtonRemoveBreakpoints_Click(object sender, EventArgs e)
		{
            RemoveBreakpoint();
		}

		private void toolStripButtonOpen_Click(object sender, EventArgs e)
		{
		    Open();
		}

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
		{
			SaveAs();
        }

		private void toolStripButtonClose_Click(object sender, EventArgs e)
		{
		    Close();
		}

		private void toolStripButtonInspect_Click(object sender, EventArgs e)
		{
            InspectEntities();
		}

		private void InspectItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item assistantItem = Assistant.World.FindItem(serial);
			if (assistantItem != null && assistantItem.Serial.IsItem)
			{
				this.BeginInvoke((MethodInvoker)delegate
				{
					EnhancedItemInspector inspector = new EnhancedItemInspector(assistantItem);
					inspector.TopMost = true;
					inspector.Show();
				});
			}
			else
			{
				Assistant.Mobile assistantMobile = Assistant.World.FindMobile(serial);
				if (assistantMobile != null && assistantMobile.Serial.IsMobile)
				{
					this.BeginInvoke((MethodInvoker)delegate
					{
						EnhancedMobileInspector inspector = new EnhancedMobileInspector(assistantMobile);
						inspector.TopMost = true;
						inspector.Show();
					});
				}
			}
		}

		private void toolStripButtonGumps_Click(object sender, EventArgs e)
		{
		    InspectGumps();
		}

		private void gumpinspector_close(object sender, EventArgs e)
		{
			Assistant.Engine.MainWindow.GumpInspectorEnable = false;
		}

	    private void Open()
	    {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Script Files|*.py";
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                m_Filename = Path.GetFileNameWithoutExtension(open.FileName);
                m_Filepath = open.FileName;
                this.Text = m_Title + " - " + m_Filename + ".py";
                fastColoredTextBoxEditor.Text = File.ReadAllText(open.FileName);
            }
        }

	    private void Save()
	    {
            if (m_Filename != "")
            {
                this.Text = m_Title + " - " + m_Filename + ".py";
                File.WriteAllText(m_Filepath, fastColoredTextBoxEditor.Text);
                Scripts.EnhancedScript script = Scripts.Search(m_Filename + ".py");
                if (script != null)
                {
                    string fullpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Scripts", m_Filename + ".py");

                    if (File.Exists(fullpath) && Scripts.EnhancedScripts.ContainsKey(m_Filename + ".py"))
                    {
                        string text = File.ReadAllText(fullpath);
                        bool loop = script.Loop;
                        bool wait = script.Wait;
                        bool run = script.Run;
                        bool isRunning = script.IsRunning;

                        if (isRunning)
                            script.Stop();

                        Scripts.EnhancedScript reloaded = new Scripts.EnhancedScript(m_Filename + ".py", text, wait, loop, run);
                        reloaded.Create(null);
                        Scripts.EnhancedScripts[m_Filename + ".py"] = reloaded;

                        if (isRunning)
                            reloaded.Start();
                    }
                }
            }
            else
            {
                SaveAs();
            }
        }

		private void SaveAs()
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = "Script Files|*.py";
			save.RestoreDirectory = true;

			if (save.ShowDialog() == DialogResult.OK)
			{
				m_Filename = Path.GetFileNameWithoutExtension(save.FileName);
				this.Text = m_Title + " - " + m_Filename + ".py";
				m_Filepath = save.FileName;
				File.WriteAllText(save.FileName, fastColoredTextBoxEditor.Text);

				string filename = Path.GetFileName(save.FileName);
				Scripts.EnhancedScript script = Scripts.Search(filename);
				if (script != null)
				{
					string fullpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Scripts", filename);

					if (File.Exists(fullpath) && Scripts.EnhancedScripts.ContainsKey(filename))
					{
						string text = File.ReadAllText(fullpath);
						bool loop = script.Loop;
						bool wait = script.Wait;
						bool run = script.Run;
						bool isRunning = script.IsRunning;

						if (isRunning)
							script.Stop();

						Scripts.EnhancedScript reloaded = new Scripts.EnhancedScript(filename, text, wait, loop, run);
						reloaded.Create(null);
						Scripts.EnhancedScripts[filename] = reloaded;

						if (isRunning)
							reloaded.Start();
					}
				}
			}
		}

	    private void Close()
	    {
            DialogResult res = MessageBox.Show("Save current file?", "WARNING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Script Files|*.py";
                save.FileName = m_Filename;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(save.FileName, fastColoredTextBoxEditor.Text);
                }
                fastColoredTextBoxEditor.Text = "";
                m_Filename = "";
                m_Filepath = "";
                this.Text = m_Title;
            }
            else if (res == System.Windows.Forms.DialogResult.No)
            {
                fastColoredTextBoxEditor.Text = "";
                m_Filename = "";
                m_Filepath = "";
                this.Text = m_Title;
            }
        }

	    private void InspectEntities()
	    {
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(InspectItemTarget_Callback));
        }

	    private void InspectGumps()
	    {
            EnhancedGumpInspector ginspector = new EnhancedGumpInspector();
            ginspector.FormClosed += new FormClosedEventHandler(gumpinspector_close);
            ginspector.TopMost = true;
            ginspector.Show();
        }

	    private void AddBreakpoint()
	    {
            int iline = fastColoredTextBoxEditor.Selection.Start.iLine;

            if (!m_Breakpoints.Contains(iline))
            {
                m_Breakpoints.Add(iline);
                FastColoredTextBoxNS.Line line = fastColoredTextBoxEditor[iline];
                line.BackgroundBrush = new SolidBrush(Color.Red);
                fastColoredTextBoxEditor.Invalidate();
            }
        }

	    private void RemoveBreakpoint()
	    {
            int iline = fastColoredTextBoxEditor.Selection.Start.iLine;

            if (m_Breakpoints.Contains(iline))
            {
                m_Breakpoints.Remove(iline);
                FastColoredTextBoxNS.Line line = fastColoredTextBoxEditor[iline];
                line.BackgroundBrush = new SolidBrush(Color.White);
                fastColoredTextBoxEditor.Invalidate();
            }
        }
        
        /// <summary>
        /// Function to Shortcut with keyboard
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
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
                    Close();
                    return true;

                //Inspect Entities
                case (Keys.Control | Keys.I):
                    InspectEntities();
                    return true;

                //Inspect Gumps
                case (Keys.Control | Keys.G):
                    InspectGumps();
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

                //Debug - Next Call
                case (Keys.F9):
                    EnqueueCommand(Command.Call);
                    return true;

                //Debug - Next Line
                case (Keys.F10):
                    EnqueueCommand(Command.Line);
                    return true;

                //Debug - Next Return
                case (Keys.F11):
                    EnqueueCommand(Command.Return);
                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }

    /// <summary>
    /// This autocomplete item appears after dot
    /// </summary>
    public class MethodAutocompleteItemAdvance : MethodAutocompleteItem
    {
        string firstPart;
        string lastPart;

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
}