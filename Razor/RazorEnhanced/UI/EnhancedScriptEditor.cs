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
		private static AutoResetEvent m_WaitDebug = new AutoResetEvent(false);

		private string m_Title {
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

		private PythonEngine m_pe;
		private ScriptEngine m_Engine;
		private ScriptSource m_Source;
		private ScriptScope m_Scope;

		private TraceBackFrame m_CurrentFrame;
		private FunctionCode m_CurrentCode;
		private string m_CurrentResult;
		private object m_CurrentPayload;
		private int m_ThreadID;

		private List<int> m_Breakpoints = new List<int>();

		private volatile bool m_Breaktrace = false;
		private bool m_onclosing = false;

		private FastColoredTextBoxNS.AutocompleteMenu m_popupMenu;

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
            InitSyntaxtHighlight();

            //Automenu Section
            m_popupMenu = new AutocompleteMenu(fastColoredTextBoxEditor);
			m_popupMenu.Items.ImageList = imageList2;
			m_popupMenu.SearchPattern = @"[\w\.:=!<>]";
			m_popupMenu.AllowTabKey = true;
			m_popupMenu.ToolTipDuration = 5000;
			m_popupMenu.AppearInterval = 100;

			#region Keywords

			string[] keywords =
			{
				"and", "assert", "break", "class", "continue", "def", "del", "elif", "else", "except", "exec",
				"finally", "for", "from", "global", "if", "import", "in", "is", "lambda", "not", "or", "pass", "print",
				"raise", "return", "try", "while", "yield", "None", "True", "False", "as", "sorted", "filter"
			};

            #endregion

            #region Classes Autocomplete

			string[] old_classes =
			{
				"Player", "Spells", "Mobile", "Mobiles", "Item", "Items", "Misc", "Target", "Gumps", "Journal",
				"AutoLoot", "Scavenger", "Organizer", "Restock", "SellAgent", "BuyAgent", "Dress", "Friend", "BandageHeal",
				"Statics", "DPSMeter", "PathFinding", "Timer", "Vendor"
			};

            #endregion

            //Dalamar: AutoDoc
            string[] classes = AutoDoc.GetClasses().ToArray();



            #region Methods Autocomplete

            string[] methodsPlayer =
			{
				"Player.BuffsExist", "Player.GetBuffDescription", "Player.SpellIsEnabled",
				"Player.HeadMessage", "Player.InRangeMobile", "Player.InRangeItem", "Player.GetItemOnLayer",
				"Player.UnEquipItemByLayer", "Player.EquipItem", "Player.CheckLayer", "Player.GetAssistantLayer", "Player.EquipUO3D",
				"Player.GetSkillValue", "Player.GetSkillCap", "Player.SetSkillStatus", "Player.GetSkillStatus", "Player.GetRealSkillValue", "Player.UseSkill", "Player.ChatSay",
				"Player.ChatEmote", "Player.ChatWhisper","Player.ChatChannel",
				"Player.ChatYell", "Player.ChatGuild", "Player.ChatAlliance", "Player.SetWarMode", "Player.Attack",
				"Player.AttackLast", "Player.InParty", "Player.ChatParty",
				"Player.PartyCanLoot", "Player.PartyInvite", "Player.PartyAccept", "Player.PartyLeave", "Player.KickMember", "Player.InvokeVirtue",
				"Player.Walk", "Player.Run", "Player.PathFindTo", "Player.GetPropValue", "Player.GetPropStringByIndex", "GetPropStringList",
                "Player.SumAttribute", "Player.QuestButton",
				"Player.GuildButton", "Player.WeaponPrimarySA", "Player.WeaponSecondarySA", "Player.WeaponClearSA",
				"Player.WeaponStunSA", "Player.WeaponDisarmSA, Player.HasSpecial", "Player.Flying",
                "Player.ToggleAlwaysRun",
            };

			string[] methodsSpells =
			{
                "Spells.CastMagery", "Spells.CastNecro", "Spells.CastChivalry", "Spells.CastBushido", "Spells.CastNinjitsu",
                "Spells.CastSpellweaving", "Spells.CastMysticism", "Spells.CastMastery", "Spells.CastCleric", "Spells.CastDruid",
                "Spells.Interrupt", "Spells.CastLastSpell"
			};

			string[] methodsMobiles =
			{
				"Mobile.GetItemOnLayer", "Mobile.GetAssistantLayer", "Mobiles.FindBySerial", "Mobiles.UseMobile", "Mobiles.SingleClick",
				"Mobiles.Filter", "Mobiles.ApplyFilter", "Mobiles.Select", "Mobiles.Message", "Mobiles.WaitForProps", "Mobiles.GetPropValue",
				"Mobiles.GetPropStringByIndex", "Mobiles.GetPropStringList", "Mobiles.Flying", "Mobiles.ContextExist", "Mobiles.WaitForStats",
                "Mobiles.GetTrackingInfo"
			};

			string[] methodsItems =
			{
				"Items.FindBySerial", "Items.Move", "Items.MoveOnGround", "Items.DropItemGroundSelf", "Items.UseItem", "Items.SingleClick",
				"Items.WaitForProps", "Items.GetPropValue", "Items.GetPropStringByIndex", "Items.GetPropStringList",
				"Items.WaitForContents", "Items.Message", "Items.Filter", "Items.ApplyFilter", "Items.Select", "Items.BackpackCount",
                "Items.ContainerCount", "Items.UseItemByID", "Items.Hide", "Items.ContextExist", "Items.FindByID"
			};

			string[] methodsMisc =
			{
				"Misc.SendMessage", "Misc.SendToClient", "Misc.ResetPrompt", "Misc.HasPrompt", "Misc.WaitForPrompt", "Misc.CancelPrompt", "Misc.ResponsePrompt",
                "Misc.NoOperation", "Misc.Resync", "Misc.Pause", "Misc.Beep", "Misc.Disconnect", "Misc.WaitForContext",
				"Misc.ContextReply", "Misc.MouseMove", "Misc.MouseLocation",
                "Misc.ReadSharedValue", "Misc.RemoveSharedValue", "Misc.CheckSharedValue",
				"Misc.SetSharedValue", "Misc.ScriptStopAll", "Misc.ShardName",
				"Misc.HasMenu", "Misc.CloseMenu", "Misc.MenuContain", "Misc.GetMenuTitle", "Misc.WaitForMenu",
				"Misc.MenuResponse", "Misc.HasQueryString",
				"Misc.WaitForQueryString", "Misc.QueryStringResponse", "Misc.NoOperation", "Misc.ScriptRun", "Misc.ScriptStop",
				"Misc.ScriptStatus", "Misc.PetRename", "Misc.FocusUOWindow",
				"Misc.IgnoreObject", "Misc.CheckIgnoreObject", "Misc.ClearIgnore", "Misc.UnIgnoreObject",
				"Misc.CurrentScriptDirectory", "Misc.CaptureNow", "Misc.GetMapInfo",
                "Misc.CloseBackpack", "Misc.NextContPosition", "Misc.GetContPosition"
            };

			string[] methodsTarget =
			{
				"Target.HasTarget", "Target.GetLast", "Target.GetLastAttack", "Target.WaitForTarget", "Target.TargetExecute", "Target.TargetExecuteRelative",
                "Target.PromptGroundTarget", "Target.PromptTarget", "Target.Cancel", "Target.Last", "Target.LastQueued", "Target.Self", "Target.SelfQueued",
                "Target.SetLast", "Target.ClearLast", "Target.ClearQueue", "Target.ClearLastandQueue", "Target.SetLastTargetFromList", "Target.GetTargetFromList",
				"Target.PerformTargetFromList", "Target.AttackTargetFromList"
			};

			string[] methodsGumps =
			{
				"Gumps.CurrentGump", "Gumps.HasGump", "Gumps.CloseGump", "Gumps.ResetGump", "Gumps.WaitForGump", "Gumps.SendAction",
				"Gumps.SendAdvancedAction", "Gumps.LastGumpGetLine", "Gumps.LastGumpGetLineList", "Gumps.LastGumpTextExist",
				"Gumps.LastGumpTextExistByLine", "Gumps.LastGumpRawData"
			};

			string[] methodsJournal =
			{
				"Journal.Clear", "Journal.Search", "Journal.SearchByName", "Journal.SearchByColor",
				"Journal.SearchByType", "Journal.GetLineText", "Journal.GetSpeechName", "Journal.WaitJournal",
                "Journal.WaitByName",
                "Journal.GetTextBySerial", "Journal.GetTextByColor", "Journal.GetTextByName", "Journal.GetTextByType"
			};

			string[] methodsAutoLoot =
			{
				"AutoLoot.Status", "AutoLoot.Start", "AutoLoot.Stop", "AutoLoot.ChangeList", "AutoLoot.RunOnce", "AutoLoot.GetList", "AutoLoot.GetLootBag", "AutoLoot.SetNoOpenCorpse"
            };

			string[] methodsScavenger =
			{
				"Scavenger.Status", "Scavenger.Start", "Scavenger.Stop", "Scavenger.ChangeList", "Scavenger.RunOnce"
			};

			string[] methodsOrganizer =
			{
				"Organizer.Status", "Organizer.FStart", "Organizer.FStop", "Organizer.ChangeList", "Organizer.RunOnce"
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
				"Dress.DressStatus", "Dress.UnDressStatus", "Dress.DressFStart", "Dress.UnDressFStart", "Dress.DressFStop", "Dress.UnDressFStop", "Dress.ChangeList"
			};

			string[] methodsFriend =
			{
				"Friend.IsFriend", "Friend.ChangeList", "Friend.GetList", "Friend.AddPlayer"
			};

			string[] methodsBandageHeal =
			{
				"BandageHeal.Status", "BandageHeal.Start", "BandageHeal.Stop"
			};

			string[] methodsDPSMeter =
			{
				"DPSMeter.Status", "DPSMeter.Start", "DPSMeter.Stop", "DPSMeter.Pause", "DPSMeter.GetDamage"
			};

			string[] methodsPathFinding =
            {
				"PathFinding.Go", "PathFinding.Route", "Pathfinding.GetPath", "Pathfinding.RunPath", "Pathfinding.Tile",
            };
            string[] methodsVendor =
            {
                "Vendor.Buy",
                "Vendor.BuyList",
            };

            string[] methodsTimer =
{
				"Timer.Check", "Timer.Create"
			};

			string[] methodsStatics =
			{
				"Statics.GetLandID", "Statics.GetLandZ", "Statics.GetStaticsTileInfo", "Statics.GetLandName", "Statics.GetTileName", "Statics.GetTileFlag", "Statics.GetLandFlag", "Statics.GetStaticsLandInfo", "Statics.CheckDeedHouse"
			};

			string[] methodsGeneric =
			{
				"GetItemOnLayer", "GetAssistantLayer", "DistanceTo"
			};
            #endregion
            //Dalamar: AutoDoc
            string[] methods = AutoDoc.GetMethods(true, true, false).ToArray();


			string[] old_methods =
				methodsPlayer.Union(methodsSpells)
					.Union(methodsMobiles)
					.Union(methodsItems)
					.Union(methodsMisc)
					.Union(methodsTarget)
					.Union(methodsGumps)
					.Union(methodsJournal)
					.Union(methodsAutoLoot)
					.Union(methodsScavenger)
					.Union(methodsOrganizer)
					.Union(methodsRestock)
					.Union(methodsSellAgent)
					.Union(methodsBuyAgent)
					.Union(methodsDress)
					.Union(methodsFriend)
					.Union(methodsBandageHeal)
					.Union(methodsStatics)
					.Union(methodsDPSMeter)
					.Union(methodsPathFinding)
					.Union(methodsTimer)
                    .Union(methodsVendor)
                    .ToArray();




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

			#region Methods Descriptions

			ToolTipDescriptions tooltip;

			#region Description Player

			Dictionary<string, ToolTipDescriptions> descriptionPlayer = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Player.SpellIsEnabled(string)", new string[] { "string SpellName" }, "bool", "Get a bool value if specific spell is enabled (red Icon) or not\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.SpellIsEnabled", tooltip);

            tooltip = new ToolTipDescriptions("Player.ToggleAlwaysRun()", new string[] { "string SkillName" }, "void", "Toggles the Always Run in the UI, but you can only tell on/off from journal");
            descriptionPlayer.Add("Player.ToggleAlwaysRun", tooltip);

            tooltip = new ToolTipDescriptions("Player.BuffsExist(string)", new string[] { "string BuffName" }, "bool", "Get a bool value if specific buff exist or not\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.BuffsExist", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetBuffDescription(BuffIcon)", new string[] { "BuffIcon Name" }, "string", "Get description of a specific BuffIcon");
			descriptionPlayer.Add("Player.GetBuffDescription", tooltip);

			tooltip = new ToolTipDescriptions("Player.HeadMessage(int, string or int)", new string[] { "int MessageColor", "string Message or int number" }, "void", "Display a message over self character with specified color");
			descriptionPlayer.Add("Player.HeadMessage", tooltip);

			tooltip = new ToolTipDescriptions("Player.InRangeMobile(Mobile or int, int)", new string[] { "Mobile MobileToCheck or int SerialMobileToCheck", "int range" }, "bool", "Retrieves a bool value if specific mobile is in a certain range");
			descriptionPlayer.Add("Player.InRangeMobile", tooltip);

			tooltip = new ToolTipDescriptions("Player.InRangeItem(Item or int, int)", new string[] { "Item ItemToCheck or int SerialItemToCheck", "int range" }, "bool", "Retrieves a bool value if specific item is in a certain range");
			descriptionPlayer.Add("Player.InRangeItem", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetItemOnLayer(string)", new string[] { "string LayerName" }, "Item", "Retrieves a item value of item equipped on specific layer\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.GetItemOnLayer", tooltip);

			tooltip = new ToolTipDescriptions("Player.UnEquipItemByLayer(string)", new string[] { "string LayerName" }, "void", "Unequip an item on a specific layer\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.UnEquipItemByLayer", tooltip);

			tooltip = new ToolTipDescriptions("Player.EquipItem(Item or int)", new string[] { "Item ItemInstance or int SerialItem" }, "void", "Equip an item on a layer");
			descriptionPlayer.Add("Player.EquipItem", tooltip);

			tooltip = new ToolTipDescriptions("Player.EquipUO3D(List<int>)", new string[] { "List<int> SerialtoEquip" }, "void", "Equip a list o item by UO3D Packet");
			descriptionPlayer.Add("Player.EquipUO3D", tooltip);

			tooltip = new ToolTipDescriptions("Player.CheckLayer(string)", new string[] { "string LayerName" }, "bool", "Retrieves current status of a certain layer\n\tTrue: busy, False: free\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.CheckLayer", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetAssistantLayer(string)", new string[] { "string LayerName" }, "Layer", "Retrives HexID from the Layer's name");
			descriptionPlayer.Add("Player.GetAssistantLayer", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetSkillValue(string)", new string[] { "string SkillName" }, "dobule", "Get current value of a specific skill\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.GetSkillValue", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetSkillCap(string)", new string[] { "string SkillName" }, "double", "Get current value of a specific skillcap\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.GetSkillCap", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetSkillStatus(string)", new string[] { "string SkillName" }, "int", "Get lock status for a certain skill\n\tUP: 0, DOWN: 1, LOCKED: 2\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.GetSkillStatus", tooltip);

			tooltip = new ToolTipDescriptions("Player.SetSkillStatus(string, int)", new string[] { "string SkillName, int Status" }, "void", "Set status for a certain skill\n\tUP: 0, DOWN: 1, LOCKED: 2\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.SetSkillStatus", tooltip);


			tooltip = new ToolTipDescriptions("Player.GetRealSkillValue(string)", new string[] { "string SkillName" }, "int", "Get real value of a specific skill\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.GetRealSkillValue", tooltip);

			tooltip = new ToolTipDescriptions("Player.UseSkill(string)", new string[] { "string SkillName" }, "void", "Use a specific skill\n\tCheck the wiki for the possible strings");
			descriptionPlayer.Add("Player.UseSkill", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatSay(int, string or int)", new string[] { "int MessageColor", "string Message or int number" }, "void", "Send a message in say with a specific color");
			descriptionPlayer.Add("Player.ChatSay", tooltip);

			tooltip = new ToolTipDescriptions("Player.MapSay(string or int)", new string[] { "string Message or int number" }, "void", "Send a message in map application");
			descriptionPlayer.Add("Player.MapSay", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatEmote(int, string or int)", new string[] { "int MessageColor", "string Message or int number" }, "void", "Send a message in emote with a specific color");
			descriptionPlayer.Add("Player.ChatEmote", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatWhisper(int, string or int)", new string[] { "int MessageColor", "string Message or int number" }, "void", "Send a message in wishper with a specific color");
			descriptionPlayer.Add("Player.ChatWhisper", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatChannel(string or int)", new string[] { "string Message or int number" }, "void", "Send a message in chat channel.");
			descriptionPlayer.Add("Player.ChatChannel", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatYell(int, string or int)", new string[] { "int MessageColor", "string Message or int number" }, "void", "Send a message in yell with a specific color");
			descriptionPlayer.Add("Player.ChatYell", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatGuild(string or int)", new string[] { "string Message or int number" }, "void", "Send a message in guild chat");
			descriptionPlayer.Add("Player.ChatGuild", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatAlliance(string or int)", new string[] { "string Message or int number" }, "void", "Send a message in alliance chat");
			descriptionPlayer.Add("Player.ChatAlliance", tooltip);

			tooltip = new ToolTipDescriptions("Player.SetWarMode(bool)", new string[] { "bool WarStatus" }, "void", "Set character warmode status\n\t True: set Warmode ON, False: set Warmode OFF");
			descriptionPlayer.Add("Player.SetWarMode", tooltip);

			tooltip = new ToolTipDescriptions("Player.Attack(int or mobile)", new string[] { "int TargetSerial ot Mobile mobiletoattack" }, "void", "Force character to atttack a specific serial or mobile object");
			descriptionPlayer.Add("Player.Attack", tooltip);

			tooltip = new ToolTipDescriptions("Player.AttackLast()", new string[] { "none" }, "void", "Force character to attack last target");
			descriptionPlayer.Add("Player.AttackLast", tooltip);

			tooltip = new ToolTipDescriptions("Player.ChatParty(string, optional int)", new string[] { "string Message, optional int serial" }, "void", "Send a message to party chat, if specific a serial send private message");
			descriptionPlayer.Add("Player.ChatParty", tooltip);

			tooltip = new ToolTipDescriptions("Player.PartyCanLoot(bool)", new string[] { "bool Flag" }, "void", "Set player party CanLoot flag\n\tTrue: Members can loot me, False: Member can't loot me");
			descriptionPlayer.Add("Player.PartyCanLoot", tooltip);

            tooltip = new ToolTipDescriptions("Player.PartyInvite()", new string[] { "none" }, "void", "Open a target prompt to invite new members");
            descriptionPlayer.Add("Player.PartyInvite", tooltip);

            tooltip = new ToolTipDescriptions("Player.PartyAccept(int serial = 0)", new string[] { "int Serial = 0" }, "void", "Accept party invites. Serial: Specify the serial of the Party Leader who sent the invitation. Useful in case of multiple invitations.");
            descriptionPlayer.Add("Player.PartyAccept", tooltip);

            tooltip = new ToolTipDescriptions("Player.PartyLeave()", new string[] { "none" }, "void", "Leave from party");
			descriptionPlayer.Add("Player.PartyLeave", tooltip);

			tooltip = new ToolTipDescriptions("Player.KickMember(int)", new string[] { "int SerialPersonToKick" }, "void", "Kick a member from party by serial\n\tOnly for party leader");
			descriptionPlayer.Add("Player.KickMember", tooltip);

			tooltip = new ToolTipDescriptions("Player.InvokeVirtue(string)", new string[] { "string VirtueName" }, "void", "Invoke a chracter virtue by name");
			descriptionPlayer.Add("Player.InvokeVirtue", tooltip);

			tooltip = new ToolTipDescriptions("Player.Walk(string)", new string[] { "string Direction" }, "int", "Move character in a specific direction\n\tCheck the wiki for the possible strings\n\t Return true for success move, false if fail");
			descriptionPlayer.Add("Player.Walk", tooltip);

			tooltip = new ToolTipDescriptions("Player.Run(string)", new string[] { "string Direction" }, "int", "Move character (run speed) in a specific direction\n\tCheck the wiki for the possible strings\n\t Return true for success move, false if fail");
			descriptionPlayer.Add("Player.Run", tooltip);

			tooltip = new ToolTipDescriptions("Player.PathFindTo(int, int, int)", new string[] { "int X, int Y, int Z" }, "void", "Client pathfinder to specific location with XYZ coordinates");
			descriptionPlayer.Add("Player.PathFindTo", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetPropValue(string)", new string[] { "string PropName" }, "int", "Get property value of player");
			descriptionPlayer.Add("Player.GetPropValue", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetPropStringByIndex(int)", new string[] { "int PropIndex" }, "string", "Get property name by index, if any property\n\tin selected index, return empty");
			descriptionPlayer.Add("Player.GetPropStringByIndex", tooltip);

			tooltip = new ToolTipDescriptions("Player.GetPropStringList()", new string[] { "none" }, "List<string>", "Get a list with all property name, if there are no\n\tproperties, list is empty");
			descriptionPlayer.Add("Player.GetPropStringList", tooltip);

            tooltip = new ToolTipDescriptions("Player.SumAttribute(string)", new string[] { "string Attribute" }, "float", "Scan the current layers and compute the value of the Attribute requested");
            descriptionPlayer.Add("Player.SumAttribute", tooltip);

            tooltip = new ToolTipDescriptions("Player.QuestButton()", new string[] { "none" }, "void", "Open quest menu linked to paperdoll quest button");
			descriptionPlayer.Add("Player.QuestButton", tooltip);

			tooltip = new ToolTipDescriptions("Player.GuildButton()", new string[] { "none" }, "void", "Open guild menu linked to paperdoll guild button");
			descriptionPlayer.Add("Player.GuildButton", tooltip);

			tooltip = new ToolTipDescriptions("Player.WeaponPrimarySA()", new string[] { "none" }, "void", "Set on Weapon Primary Ability");
			descriptionPlayer.Add("Player.WeaponPrimarySA", tooltip);

			tooltip = new ToolTipDescriptions("Player.WeaponSecondarySA()", new string[] { "none" }, "void", "Set on Weapon Secondary Ability");
			descriptionPlayer.Add("Player.WeaponSecondarySA", tooltip);

			tooltip = new ToolTipDescriptions("Player.WeaponClearSA()", new string[] { "none" }, "void", "Clear ability if active");
			descriptionPlayer.Add("Player.WeaponClearSA", tooltip);

			tooltip = new ToolTipDescriptions("Player.WeaponStunSA()", new string[] { "none" }, "void", "Set on No Weapon Stun");
			descriptionPlayer.Add("Player.WeaponStunSA", tooltip);

			tooltip = new ToolTipDescriptions("Player.WeaponDisarmSA()", new string[] { "none" }, "void", "Set on No Weapon Disarm");
			descriptionPlayer.Add("Player.WeaponDisarmSA", tooltip);

			tooltip = new ToolTipDescriptions("Player.Fly()", new string[] { "bool OnOF" }, "void", "Enable or disable Gargoyle Fly");
			descriptionPlayer.Add("Player.Fly", tooltip);

			#endregion

			#region Description Spells

			Dictionary<string, ToolTipDescriptions> descriptionSpells = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Spells.CastMagery(string, mobile)", new string[] { "string SpellName, Mobile target" }, "void", "Cast a magery spell by spell name and (optionally) target mobile\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastMagery", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastNecro(string)", new string[] { "string SpellName" }, "void", "Cast a necro spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastNecro", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastChivalry(string)", new string[] { "string SpellName" }, "void", "Cast a chivalry spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastChivalry", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastBushido(string)", new string[] { "string SpellName" }, "void", "Cast a bushido spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastBushido", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastNinjitsu(string)", new string[] { "string SpellName" }, "void", "Cast a ninjitsu spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastNinjitsu", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastSpellweaving(string)", new string[] { "string SpellName" }, "void", "Cast a spellweaving spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastSpellweaving", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastMysticism(string)", new string[] { "string SpellName" }, "void", "Cast a mysticism spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastMysticism", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastMastery(string)", new string[] { "string SpellName" }, "void", "Cast a Mastery spell by spell name\n\tCheck the wiki for the possible strings");
			descriptionSpells.Add("Spells.CastMastery", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastCleric(string)", new string[] { "string SpellName" }, "void", "Cast a Cleric spell by spell name\n\tCheck the wiki for the possible strings");
            descriptionSpells.Add("Spells.CastCleric", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastDruid(string)", new string[] { "string SpellName" }, "void", "Cast a Druid spell by spell name\n\tCheck the wiki for the possible strings");
            descriptionSpells.Add("Spells.CastDruid", tooltip);

            tooltip = new ToolTipDescriptions("Spells.Interrupt()", new string[] { "none" }, "void", "Block current casting action.");
			descriptionSpells.Add("Spells.Interrupt", tooltip);

			tooltip = new ToolTipDescriptions("Spells.CastLastSpell()", new string[] { "target(optional)" }, "void", "Cast Last spell optionally supply a target.");
			descriptionSpells.Add("Spells.CastLastSpell", tooltip);

			#endregion

			#region Description Mobiles

			Dictionary<string, ToolTipDescriptions> descriptionMobiles = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Mobiles.FindBySerial(int)", new string[] { "int MobileSerial" }, "Mobile", "Find mobile instance by specific serial");
			descriptionMobiles.Add("Mobiles.FindBySerial", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.GetTrackingInfo()", new string[] { "none" }, "TrackingStruct", "Return the most recent tracking info");
            descriptionMobiles.Add("Mobiles.GetTrackingInfo", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.UseMobile(Mobile or int)", new string[] { "Mobile MobileIstance or int MobileSerial" }, "void", "Use (double click) specific mobile");
			descriptionMobiles.Add("Mobiles.UseMobile", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.SingleClick(Mobile or int)", new string[] { "Mobile MobileIstance or int MobileSerial" }, "void", "Perform a single click on specific mobile");
			descriptionMobiles.Add("Mobiles.SingleClick", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.Filter()", new string[] { "none" }, "Filter", "Create a new instance for a mobile filter");
			descriptionMobiles.Add("Mobiles.Filter", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.ApplyFilter(Filter)", new string[] { "Filter MobileFilter" }, "List<Mobile>", "Search mobiles by filter");
			descriptionMobiles.Add("Mobiles.ApplyFilter", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.Select(<List>Mobile, string)", new string[] { "<List>Mobile, string selectortype" }, "Mobile", "Select a specific mobile from a list by using a selector type\n\tSee wiki for selecto type");
			descriptionMobiles.Add("Mobiles.Select", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.Message(Mobile or int, int, string)", new string[] { "Mobile MobileIstance or int MobileSerial", "int ColorMessage", "string Message" }, "void", "Display a message with a certain color over a specified mobile");
			descriptionMobiles.Add("Mobiles.Message", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.WaitForProps(Mobile or int, int)", new string[] { "Mobile MobileIstance or int MobileSerial", "int TimeoutProps" }, "void", "Wait to retrieves properties of a specific mobile within a certain timeout");
			descriptionMobiles.Add("Mobiles.WaitForProps", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.WaitForStats(Mobile or int, int)", new string[] { "Mobile MobileIstance or int MobileSerial", "int TimeoutProps" }, "void", "Wait to retrieves stats (Whitout open bar) of a specific mobile within a certain timeout");
			descriptionMobiles.Add("Mobiles.WaitForStats", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.GetPropValue(Mobile or int, string)", new string[] { "Mobile MobileIstance or int MobileSerial", "string PropName" }, "int", "Get value of a specific property from a certain mobile");
			descriptionMobiles.Add("Mobiles.GetPropValue", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.GetPropStringByIndex(Mobile or int, int)", new string[] { "Mobile MobileIstance or int MobileSerial", "int PropIndex" }, "string", "Get string name of a property by index,\n\tif there's no property in selected index, return empty");
			descriptionMobiles.Add("Mobiles.GetPropStringByIndex", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.GetPropStringList(Mobile or int)", new string[] { "Mobile MobileIstance or int MobileSerial" }, "List<string>", "Get list of all properties name of a specific mobile, if list is empty, returns empty");
			descriptionMobiles.Add("Mobiles.GetPropStringList", tooltip);

			tooltip = new ToolTipDescriptions("Mobiles.ContextExist(serial or mobile, string)", new string[] { "int serial or mobile, string nametosearch" }, "int", "Check if mobile have this context. Return Context ID or -1 if not valid item or not present");
			descriptionMobiles.Add("Mobiles.ContextExist", tooltip);

			#endregion

			#region Description Items

			Dictionary<string, ToolTipDescriptions> descriptionItems = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Items.FindBySerial(int)", new string[] { "int ItemSerial" }, "Item", "Find item instance by specific serial");
			descriptionItems.Add("Items.FindBySerial", tooltip);

            tooltip = new ToolTipDescriptions("Items.Move(Item or int, Item or Mobile or int, int, (optional)(int, int))", new string[] { "Item Source or int SourceItemSerial", "Item DestinationItem or Mobile DestinationMobile or int DestinationSerial", "int AmountToMove", "int X", "int Y" }, "void", "Move a item with a certain amount to specific destination\n\tIf amount is set to 0 or bigger value of the amount, move the entire stack\n\tIs also possible to declare coordinates where item needs to be positioned\n\tinto the container");
            descriptionItems.Add("Items.Move", tooltip);

            tooltip = new ToolTipDescriptions("Items.MoveOnGround(Item or int, int, int, int, int)", new string[] { "Item ItemInstance or ItemSerial", "int amount", "int X", "int Y", "int Z" }, "void", "Move an item with a specific amount to the ground in specified coordinates\n\tIf amount is set to 0 or bigger value of the amount, move the entire stack");
            descriptionItems.Add("Items.MoveOnGround", tooltip);

            tooltip = new ToolTipDescriptions("Items.DropItemGroundSelf(Item, int)", new string[] { "Item ItemInstance", "int Amount" }, "void", "Drop on character feets specified item with certain amount.\n\tIf amount is set to 0 or bigger value of the amount, move the entire stack");
			descriptionItems.Add("Items.DropItemGroundSelf", tooltip);

			tooltip = new ToolTipDescriptions("Items.UseItem(Item or int, (optional)int target)", new string[] { "Item ItemInstance or int ItemSerial, TargetSerial" }, "void", "Use (double click) specified item and optionally target targetSerial.");
			descriptionItems.Add("Items.UseItem", tooltip);

			tooltip = new ToolTipDescriptions("Items.SingleClick(Item or int)", new string[] { "Item ItemInstance or int ItemSerial" }, "void", "Perform a single click on a specific item");
			descriptionItems.Add("Items.SingleClick", tooltip);

			tooltip = new ToolTipDescriptions("Items.WaitForProps(Item or int, int)", new string[] { "Item ItemInstance or int ItemSerial", "int TimeoutProps" }, "void", "Wait to retrieves property of a specific item for a certain time");
			descriptionItems.Add("Items.WaitForProps", tooltip);

			tooltip = new ToolTipDescriptions("Items.GetPropValue(Item or int, string)", new string[] { "Item ItemInstance or int ItemSerial", "string PropName" }, "int", "Get value of item property");
			descriptionItems.Add("Items.GetPropValue", tooltip);

			tooltip = new ToolTipDescriptions("Items.GetPropStringByIndex(Item or int, int)", new string[] { "Item ItemInstance or int ItemSerial", "int PropIndex" }, "string", "Get name of property by index, if no property in selected index, return empty");
			descriptionItems.Add("Items.GetPropStringByIndex", tooltip);

			tooltip = new ToolTipDescriptions("Items.GetPropStringList(Item or int)", new string[] { "Item ItemInstance or int ItemSerial" }, "List<string>", "Get list of all property names on specific item, if no property, returns empty list");
			descriptionItems.Add("Items.GetPropStringList", tooltip);

			tooltip = new ToolTipDescriptions("Items.WaitForContents(Item or int, int)", new string[] { "Item ItemInstance or int ItemSerial", "int TimeoutContents" }, "void", "Force a item to open and wait for a response for item inside");
			descriptionItems.Add("Items.WaitForContents", tooltip);

			tooltip = new ToolTipDescriptions("Items.Message(Item or int, int, string)", new string[] { "Item ItemInstance or int ItemSerial", "int MessageColor", "string Message" }, "void", "Display a message with specific color over the item");
			descriptionItems.Add("Items.Message", tooltip);

			tooltip = new ToolTipDescriptions("Items.Filter()", new string[] { "none" }, "Filter", "Create a new instance for item filter");
			descriptionItems.Add("Items.Filter", tooltip);

			tooltip = new ToolTipDescriptions("Items.ApplyFilter(Filter)", new string[] { "Filter ItemFilter" }, "List<Item>", "Search items by filter");
			descriptionItems.Add("Items.ApplyFilter", tooltip);

			tooltip = new ToolTipDescriptions("Items.Select(<List>Item, string)", new string[] { "<List>Item, string selectortype" }, "Item", "Select a specific item from a list by using a selector type\n\tSee wiki for selecto type");
			descriptionMobiles.Add("Items.Select", tooltip);

			tooltip = new ToolTipDescriptions("Items.BackpackCount(int, int)", new string[] { "int ItemID", "int Color" }, "int", "Returns amount of specific item (by ItemID) and color in backpack and subcontainer\n\tColor -1 is wildcard for all color");
			descriptionItems.Add("Items.BackpackCount", tooltip);

			tooltip = new ToolTipDescriptions("Items.ContainerCount(item or int, int, int)", new string[] { "Item Container or int ContainerSerial", "int ItemID", "int Color" }, "List<Item>", "Returns amount of specific item (by ItemID) and color in a specific container\n\tColor -1 is wildcard for all color");
			descriptionItems.Add("Items.ContainerCount", tooltip);

			tooltip = new ToolTipDescriptions("Items.UseItemByID(int, int)", new string[] { "int ItemID", "int Color" }, "void", "Use item whit specific ID\n\tColor -1 is wildcard for all color");
			descriptionItems.Add("Items.UseItemByID", tooltip);

			tooltip = new ToolTipDescriptions("Items.Hide(int or item)", new string[] { "int serial or item itemtohide"}, "void", "Use to hide a item in screen");
			descriptionItems.Add("Items.Hide", tooltip);

			tooltip = new ToolTipDescriptions("Items.ContextExist(serial or item, string)", new string[] { "int serial or item, string nametosearch" }, "int", "Check if item have this context. Return Context ID or -1 if not valid item or not present");
			descriptionItems.Add("Items.ContextExist", tooltip);

			tooltip = new ToolTipDescriptions("Items.FindByID(int, int, int)", new string[] { "Int itemid, int color, int serialcontainer" }, "int", "Find item serial by specific item ID, color and Container.\n\tCan use -1 on color for no chose color\n\tcan use -1 on container for serach in all item in memory.");
			descriptionItems.Add("Items.FindByID", tooltip);

			#endregion

			#region Description Misc

			Dictionary<string, ToolTipDescriptions> descriptionMisc = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Misc.SendMessage(string or int or bool, (optional)int)", new string[] { "string Message or int Value or bool Status", "int Color" }, "void", "Send a system message\n\tIf pass color, it colors the message");
            descriptionMisc.Add("Misc.SendMessage", tooltip);

            tooltip = new ToolTipDescriptions("Misc.SendToClient(string)", new string[] { "string Message"}, "void", "Send a string to the client\nctrl characters can be sent with ^(u) where u is ctrl-u");
            descriptionMisc.Add("Misc.SendToClient", tooltip);

            tooltip = new ToolTipDescriptions("Misc.Resync()", new string[] { "none" }, "void", "Resync game data");
			descriptionMisc.Add("Misc.Resync", tooltip);

            tooltip = new ToolTipDescriptions("Misc.CloseBackpack()", new string[] { "none" }, "void", "Close Player Backpack");
            descriptionMisc.Add("Misc.CloseBackpack", tooltip);

            tooltip = new ToolTipDescriptions("Misc.NextContPosition()", new string[] { "int X, int Y" }, "void", "The next gump will open at X, Y");
            descriptionMisc.Add("Misc.NextContPosition", tooltip);

            tooltip = new ToolTipDescriptions("Misc.GetContPosition()", new string[] { "Point" }, "void", "Return the co-ordinates of the most recent gump");
            descriptionMisc.Add("Misc.GetContPosition", tooltip);

            tooltip = new ToolTipDescriptions("Misc.Pause(int)", new string[] { "int Delay" }, "void", "Pause script for N milliseconds");
			descriptionMisc.Add("Misc.Pause", tooltip);

			tooltip = new ToolTipDescriptions("Misc.Beep()", new string[] { "none" }, "void", "Play beep system sound");
			descriptionMisc.Add("Misc.Beep", tooltip);

			tooltip = new ToolTipDescriptions("Misc.Disconnect()", new string[] { "none" }, "void", "Force client to disconnect");
			descriptionMisc.Add("Misc.Disconnect", tooltip);

			tooltip = new ToolTipDescriptions("Misc.WaitForContext(int or Mobile or Item, int)", new string[] { "int Serial or Mobile MobileInstance or Item ItemInstance", "int Timeout" }, "void", "Wait a server response for a context menu request");
			descriptionMisc.Add("Misc.WaitForContext", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ContextReply(int or Mobile or Item, int or string)", new string[] { "int Serial or Mobile MobileInstance or Item ItemInstance", "int MenuID or MenuText" }, "void", "Response to a context menu on mobile or item. \n\tMenuID is base zero if use number, if use string is menu text");
			descriptionMisc.Add("Misc.ContextReply", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ResetPrompt()", new string[] { "none" }, "void", "Reset the prompt response");
            descriptionMisc.Add("Misc.ResetPrompt", tooltip);

            tooltip = new ToolTipDescriptions("Misc.HasPrompt()", new string[] { "none" }, "bool", "Player has a prompt waiting");
            descriptionMisc.Add("Misc.HasPrompt", tooltip);

            tooltip = new ToolTipDescriptions("Misc.WaitForPrompt(int)", new string[] { "int delay" }, "void", "Wait for player prompt");
            descriptionMisc.Add("Misc.WaitForPrompt", tooltip);

            tooltip = new ToolTipDescriptions("Misc.CancelPrompt()", new string[] { "none" }, "void", "cancel the player prompt");
            descriptionMisc.Add("Misc.CancelPrompt", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ResponsePrompt(string)", new string[] { "string reply" }, "void", "Respond to the outstanding player prompt");
            descriptionMisc.Add("Misc.ResponsePrompt", tooltip);

            tooltip = new ToolTipDescriptions("Misc.MouseLocation()", new string[] { "none" }, "point", "Returns the X/Y co-ordinates of the mouse location relative to the window origin");
            descriptionMisc.Add("Misc.MouseLocation", tooltip);

            tooltip = new ToolTipDescriptions("Misc.MouseMove()", new string[] { "int X, int Y" }, "void", "Moves the mouse cursor to the X/Y relative to window origin");
            descriptionMisc.Add("Misc.MouseMove", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ReadSharedValue(string)", new string[] { "string NameOfValue" }, "object", "Read a shared value, if value not exist return null");
			descriptionMisc.Add("Misc.ReadSharedValue", tooltip);

			tooltip = new ToolTipDescriptions("Misc.RemoveSharedValue(string)", new string[] { "string NameOfValue" }, "void", "Remove a shared value");
			descriptionMisc.Add("Misc.RemoveSharedValue", tooltip);

			tooltip = new ToolTipDescriptions("Misc.CheckSharedValue(string)", new string[] { "string NameOfValue" }, "bool", "Get a True or False if value exist");
			descriptionMisc.Add("Misc.CheckSharedValue", tooltip);

			tooltip = new ToolTipDescriptions("Misc.SetSharedValue(string, object)", new string[] { "string NameOfValue", "object ValueToSet" }, "void", "Set a value by specific name, if value exist, it replace the value");
			descriptionMisc.Add("Misc.SetSharedValue", tooltip);

			tooltip = new ToolTipDescriptions("Misc.HasMenu()", new string[] { "none" }, "bool", "Return status of menu\n\tTrue: menu opened, False: menu closed");
			descriptionMisc.Add("Misc.HasMenu", tooltip);

			tooltip = new ToolTipDescriptions("Misc.CloseMenu()", new string[] { "none" }, "void", "Close opened menu");
			descriptionMisc.Add("Misc.CloseMenu", tooltip);

			tooltip = new ToolTipDescriptions("Misc.MenuContain(string)", new string[] { "string TextToSearch" }, "bool", "Search in opened menu if contains a specific text");
			descriptionMisc.Add("Misc.MenuContain", tooltip);

			tooltip = new ToolTipDescriptions("Misc.GetMenuTitle()", new string[] { "none" }, "string", "Return title for opened menu");
			descriptionMisc.Add("Misc.GetMenuTitle", tooltip);

			tooltip = new ToolTipDescriptions("Misc.WaitForMenu(int)", new string[] { "int Timeout" }, "void", "Pause script until server send menu, delay in Milliseconds");
			descriptionMisc.Add("Misc.WaitForMenu", tooltip);

			tooltip = new ToolTipDescriptions("Misc.MenuResponse(string)", new string[] { "string SubmitName" }, "void", "Perform a menu response by subitem name\n\tIf item not exist, close menu");
			descriptionMisc.Add("Misc.MenuResponse", tooltip);

			tooltip = new ToolTipDescriptions("Misc.HasQueryString()", new string[] { "none" }, "bool", "Check if have a query string menu opened");
			descriptionMisc.Add("Misc.HasQueryString", tooltip);

			tooltip = new ToolTipDescriptions("Misc.WaitForQueryString(int)", new string[] { "int Timeout" }, "void", "Pause script until server send query string request, delay in Milliseconds");
			descriptionMisc.Add("Misc.WaitForQueryString", tooltip);

			tooltip = new ToolTipDescriptions("Misc.QueryStringResponse(bool, string)", new string[] { "bool YesCancelStatus", "string StringToResponse" }, "void", "Perform a query string response by ok or cancel button and specific response text");
			descriptionMisc.Add("Misc.QueryStringResponse", tooltip);

			tooltip = new ToolTipDescriptions("Misc.NoOperation()", new string[] { "none" }, "void", "Do nothing");
			descriptionMisc.Add("Misc.NoOperation", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ScriptRun(string)", new string[] { "string ScriptFilename" }, "void", "Run a script by filename\n\tScript must be present in script grid");
			descriptionMisc.Add("Misc.ScriptRun", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ScriptStop(string)", new string[] { "string ScriptFilename" }, "void", "Stop a script by filename\n\tScritp must be present in script grid");
			descriptionMisc.Add("Misc.ScriptStop", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ScriptStopAll(string)", new string[] { "none" }, "void", "Stop all script running.");
			descriptionMisc.Add("Misc.ScriptStopAll", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ScriptStatus(string)", new string[] { "string ScriptFilename" }, "bool", "Get status of a script if is running or not\n\tScript must be present in script grid");
			descriptionMisc.Add("Misc.ScriptStatus", tooltip);

			tooltip = new ToolTipDescriptions("Misc.PetRename(Mobile or int, string)", new string[] { "Mobile MobileInstance or int MobileSerial", "string NewName" }, "void", "Rename a specific pet.\n\tMust be tamed");
			descriptionMisc.Add("Misc.PetRename", tooltip);

			tooltip = new ToolTipDescriptions("Misc.FocusUOWindow()", new string[] { "none" }, "void", "Set UoClient window in focus or restore if minimized");
			descriptionMisc.Add("Misc.FocusUOWindow", tooltip);

			tooltip = new ToolTipDescriptions("Misc.IgnoreObject()", new string[] { "int serial or Item itemtoignore or Mobile mobtoignore" }, "void", "Add a object to ignore list.");
			descriptionMisc.Add("Misc.IgnoreObject", tooltip);

			tooltip = new ToolTipDescriptions("Misc.CheckIgnoreObject()", new string[] { "int serial or Item itemtocheck or Mobile mobtocheck" }, "bool", "check if object is present in ignore list.");
			descriptionMisc.Add("Misc.CheckIgnoreObject", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ClearIgnore()", new string[] { "none" }, "void", "Clear ignore list.");
			descriptionMisc.Add("Misc.ClearIgnore", tooltip);

			tooltip = new ToolTipDescriptions("Misc.UnIgnoreObject()", new string[] { "int serial or Item itemtounignore or Mobile mobtounignore" }, "void", "Remove a object from ignore list.");
			descriptionMisc.Add("Misc.UnIgnoreObject", tooltip);

			tooltip = new ToolTipDescriptions("Misc.ShardName()", new string[] { "none" }, "string", "Get currect shard name you play.");
			descriptionMisc.Add("Misc.ShardName", tooltip);

            tooltip = new ToolTipDescriptions("Misc.CaptureNow()", new string[] { "none" }, "void", "Screen capture UO window");
            descriptionMisc.Add("Misc.CaptureNow", tooltip);

            tooltip = new ToolTipDescriptions("Misc.GetMapInfo()", new string[] { "none" }, "MapInfo", "Return information about map serial");
            descriptionMisc.Add("Misc.GetMapInfo", tooltip);
            #endregion

            #region Description Target

            Dictionary<string, ToolTipDescriptions> descriptionTarget = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Target.HasTarget()", new string[] { "none" }, "bool", "Get status of target if exists or not");
			descriptionTarget.Add("Target.HasTarget", tooltip);

			tooltip = new ToolTipDescriptions("Target.GetLast()", new string[] { "none" }, "int", "Get serial number of last target");
			descriptionTarget.Add("Target.GetLast", tooltip);

			tooltip = new ToolTipDescriptions("Target.GetLastAttack()", new string[] { "none" }, "int", "Get serial number of last attack target");
			descriptionTarget.Add("Target.GetLastAttack", tooltip);

			tooltip = new ToolTipDescriptions("Target.WaitForTarget(int, optional bool)", new string[] { "int TimeoutTarget, optional bool NoShowTarget" }, "none", "Pause script to wait server to send target request\n\tTimeout is in Milliseconds, and can select if show or not target cursor in game (false show)");
			descriptionTarget.Add("Target.WaitForTarget", tooltip);

			tooltip = new ToolTipDescriptions("Target.TargetExecute(int or Item or Mobile or (int, int, int, (optional)int))", new string[] { "int Serial or Item ItemInstance or Mobile MobileInstance or ( int X, int Y, int Z, int TileID )" }, "void", "Send target execute to specific serial, item, mobile\n\tIn case of X Y Z coordinates, can be defined a tileid");
			descriptionTarget.Add("Target.TargetExecute", tooltip);

			tooltip = new ToolTipDescriptions("Target.TargetExecuteRelative(Mobile or int, int)", new string[] { "int Serial or Mobile Mobiletarget, int offset)" }, "void", "Send target execute to ground by mobile position whit offset distance.\n\tDistance is calculate in base of mobile direction");
			descriptionTarget.Add("Target.TargetExecuteRelative", tooltip);

			tooltip = new ToolTipDescriptions("Target.PromptTarget()", new string[] { "none or string message" }, "int", "Pick the serial from item or mobile\n\tCan specific string in parameters for prompt message");
			descriptionTarget.Add("Target.PromptTarget", tooltip);

			tooltip = new ToolTipDescriptions("Target.PromptGroundTarget()", new string[] { "none or string message" }, "Point3D", "Pick coords of ground targetted\n\tCan specific string in parameters for prompt message");
			descriptionTarget.Add("Target.PromptGroundTarget", tooltip);

			tooltip = new ToolTipDescriptions("Target.Cancel()", new string[] { "none" }, "void", "Cancel target cursor");
			descriptionTarget.Add("Target.Cancel", tooltip);

			tooltip = new ToolTipDescriptions("Target.Last()", new string[] { "none" }, "void", "Target last object or mobile targetted");
			descriptionTarget.Add("Target.Last", tooltip);

			tooltip = new ToolTipDescriptions("Target.LastQueued()", new string[] { "none" }, "void", "Queue next target to Last");
			descriptionTarget.Add("Target.LastQueued", tooltip);

			tooltip = new ToolTipDescriptions("Target.Self()", new string[] { "none" }, "void", "Target self");
			descriptionTarget.Add("Target.Self", tooltip);

			tooltip = new ToolTipDescriptions("Target.SelfQueued()", new string[] { "none" }, "void", "Queue Next target to Self");
			descriptionTarget.Add("Target.SelfQueued", tooltip);

			tooltip = new ToolTipDescriptions("Target.SetLast(Mobile or int)", new string[] { "Mobile MobileTarget or int TargetSerial" }, "void", "Force set last target to specific mobile, by mobile instance or serial");
			descriptionTarget.Add("Target.SetLast", tooltip);

			tooltip = new ToolTipDescriptions("Target.ClearLast()", new string[] { "none" }, "void", "Clear Last Target");
			descriptionTarget.Add("Target.ClearLast", tooltip);

			tooltip = new ToolTipDescriptions("Target.ClearQueue()", new string[] { "none" }, "void", "Clear Queue Target");
			descriptionTarget.Add("Target.ClearQueue", tooltip);

			tooltip = new ToolTipDescriptions("Target.ClearLastandQueue()", new string[] { "none" }, "void", "Clear Last and Queue Target");
			descriptionTarget.Add("Target.ClearLastandQueue", tooltip);

			tooltip = new ToolTipDescriptions("Target.SetLastTargetFromList(string)", new string[] { "string TargetFilterName" }, "bool", "Set Last Target from GUI Filter selector");
			descriptionTarget.Add("Target.SetLastTargetFromList", tooltip);

			tooltip = new ToolTipDescriptions("Target.PerformTargetFromList(string)", new string[] { "string TargetFilterName" }, "bool", "Execute Target from GUI Filter selector");
			descriptionTarget.Add("Target.PerformTargetFromList", tooltip);

			tooltip = new ToolTipDescriptions("Target.AttackTargetFromList(string)", new string[] { "string TargetFilterName" }, "bool", "Attack Target from GUI Filter selector");
			descriptionTarget.Add("Target.AttackTargetFromList", tooltip);

			tooltip = new ToolTipDescriptions("Target.GetTargetFromList(string)", new string[] { "string TargetFilterName" }, "Mobile", "Get Mobile object from GUI Filter selector\n\tIf no mobile found return null");
			descriptionTarget.Add("Target.GetTargetFromList", tooltip);

			#endregion																											   IsFriend

			#region Description Gumps

			Dictionary<string, ToolTipDescriptions> descriptionGumps = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Gumps.CurrentGump()", new string[] { "none" }, "uint", "Return a integet with ID of last gump opened and still open");
			descriptionGumps.Add("Gumps.CurrentGump", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.HasGump()", new string[] { "none" }, "bool", "Get status to check if have a gump opened or not");
			descriptionGumps.Add("Gumps.HasGump", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.CloseGump(uint)", new string[] { "uint GumpID" }, "void", "Close a specific Gump");
			descriptionGumps.Add("Gumps.CloseGump", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.ResetGump()", new string[] { "none" }, "void", "Clean gump status");
			descriptionGumps.Add("Gumps.ResetGump", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.WaitForGump(uint, int)", new string[] { "uint GumpID", "int TimeoutGump" }, "void", "Pause script to wait server to send gump after operation for call gump\n\tTimeout is in Milliseconds");
			descriptionGumps.Add("Gumps.WaitForGump", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.SendAction(uint, int)", new string[] { "uint GumpID", "int ButtonID" }, "void", "Send a gump response by GumpID and ButtonID");
			descriptionGumps.Add("Gumps.SendAction", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.SendAdvancedAction(uint, int, List<int>, (optional)List<int>, (optional)List<string>)", new string[] { "uint GumpID", "int ButtonID", "List<int> Switches", "List<int> TextID", "List<string> Texts" }, "void", "Send a gump response by GumpID and ButtonID and advanced switch in gumps\n\tYou can add a switch list with all parameters need setted in gump windows\n\tCan be also choose to send text to be filled in gump");
			descriptionGumps.Add("Gumps.SendAdvancedAction", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.LastGumpGetLine(int)", new string[] { "int LineNumber" }, "string", "Get the text in gump by line number, Gump must be still open to get data");
			descriptionGumps.Add("Gumps.LastGumpGetLine", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.LastGumpGetLineList()", new string[] { "none" }, "List<string>", "Get all texts in gump. Gump must be still open for get data");
			descriptionGumps.Add("Gumps.LastGumpGetLineList", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.LastGumpTextExist(string)", new string[] { "string TextToSearch" }, "bool", "Search text inside a gump text\n\tTrue: found, False: not found\n\tGump must be still open to get data");
			descriptionGumps.Add("Gumps.LastGumpTextExist", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.LastGumpTextExistByLine(int, string)", new string[] { "int LineNumber", "string TextToSearch" }, "bool", "Search text inside a gump text by line number\n\tTrue: found, False: not found\n\tGump must be still open to get data");
			descriptionGumps.Add("Gumps.LastGumpTextExistByLine", tooltip);

			tooltip = new ToolTipDescriptions("Gumps.LastGumpRawData()", new string[] { "none" }, "string", "Get last gump RawData structure.");
			descriptionGumps.Add("Gumps.LastGumpRawData", tooltip);

			#endregion

			#region Description Journal

			Dictionary<string, ToolTipDescriptions> descriptionJournal = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Journal.Clear()", new string[] { "none" }, "void", "Clear data in journal buffer");
			descriptionJournal.Add("Journal.Clear", tooltip);

			tooltip = new ToolTipDescriptions("Journal.Search(string)", new string[] { "string TextToSearch" }, "bool", "Search a text in all journal buffer, and text is case sensitive\n\tTrue: text found, False: text not found");
			descriptionJournal.Add("Journal.Search", tooltip);

			tooltip = new ToolTipDescriptions("Journal.SearchByName(string, string)", new string[] { "string TextToSearch", "string SenderName" }, "bool", "Search a text in all journal buffer by sender name, and text and sender name are case sensitive\n\tTrue: text found, False: text not found");
			descriptionJournal.Add("Journal.SearchByName", tooltip);

			tooltip = new ToolTipDescriptions("Journal.SearchByColor(string, int)", new string[] { "string TextToSearch", "int ColorToSearch" }, "bool", "Search a text in all journal buffer by color, and text is case sensitive\n\tTrue: text found, False: text not found");
			descriptionJournal.Add("Journal.SearchByColor", tooltip);

			tooltip = new ToolTipDescriptions("Journal.SearchByType(string, string)", new string[] { "string TextToSearch", "string MessageType" }, "bool", "Search a text in all journal buffer by type, and text and type are case sensitive\n\tTrue: text found, False: text not found\n\tCheck the wiki for the possible strings");
			descriptionJournal.Add("Journal.SearchByType", tooltip);

			tooltip = new ToolTipDescriptions("Journal.GetLineText(string)", new string[] { "string TextToSearch" }, "string", "Search and get last line containing searched text, and text is case sensitive\n\tReturns: matched entry or empty string (if not found)");
			descriptionJournal.Add("Journal.GetLineText", tooltip);

			tooltip = new ToolTipDescriptions("Journal.GetSpeechName()", new string[] { "none" }, "List<string>", "Get a list of all players name and object speech");
			descriptionJournal.Add("Journal.GetSpeechName", tooltip);

			tooltip = new ToolTipDescriptions("Journal.GetTextBySerial(int)", new string[] { "int serial" }, "List<string>", "Get a list of all line speeched by specific serial");
			descriptionJournal.Add("Journal.GetTextBySerial", tooltip);

			tooltip = new ToolTipDescriptions("Journal.GetTextByColor(int)", new string[] { "int color" }, "List<string>", "Get a list of all line speeched by specific color");
			descriptionJournal.Add("Journal.GetTextByColor", tooltip);

			tooltip = new ToolTipDescriptions("Journal.GetTextByName(string)", new string[] { "string name" }, "List<string>", "Get a list of all line speeched by specific name");
			descriptionJournal.Add("Journal.GetTextByName", tooltip);

			tooltip = new ToolTipDescriptions("Journal.GetTextByType(string)", new string[] { "string type" }, "List<string>", "Get a list of all line speeched by specific type\n\tCheck on wiki for type list");
			descriptionJournal.Add("Journal.GetTextByType", tooltip);

			tooltip = new ToolTipDescriptions("Journal.WaitJournal(string, int)", new string[] { "string TextToSearch", "int TimeoutJournal" }, "void", "Pause script and wait until text is present in journal, and text is case sensitive\n\tTimeout in Milliseconds");
			descriptionJournal.Add("Journal.WaitJournal", tooltip);

            tooltip = new ToolTipDescriptions("Journal.WaitByName(string, int)", new string[] { "string NameToSearchFor", "int TimeoutJournal" }, "void", "Pause script and wait until the named person says something in journal, and text is case sensitive\n\tTimeout in Milliseconds");
            descriptionJournal.Add("Journal.WaitByName", tooltip);


            #endregion

            #region Description AutoLoot

            Dictionary<string, ToolTipDescriptions> descriptionAutoLoot = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("AutoLoot.Status()", new string[] { "none" }, "bool", "Get status of autoloot engine\n\tTrue: is running, False: is not running");
			descriptionAutoLoot.Add("AutoLoot.Status", tooltip);

			tooltip = new ToolTipDescriptions("AutoLoot.Start()", new string[] { "none" }, "void", "Start autoloot engine");
			descriptionAutoLoot.Add("AutoLoot.Start", tooltip);

			tooltip = new ToolTipDescriptions("AutoLoot.Stop()", new string[] { "none" }, "void", "Stop autoloot engine");
			descriptionAutoLoot.Add("AutoLoot.Stop", tooltip);

			tooltip = new ToolTipDescriptions("AutoLoot.ChangeList(string)", new string[] { "string ListName" }, "void", "Change list of autoloot item. List must exist in autoloot GUI configuration");
			descriptionAutoLoot.Add("AutoLoot.ChangeList", tooltip);

			tooltip = new ToolTipDescriptions("AutoLoot.RunOnce(string, double, Filter)", new string[] { "string AutoLootItemListName", "double DelayGrabInMs", "Filter FilterToSearch" }, "void", "Start autoloot with certain parameters. AutoLootitem is a list type for item\n\tdelay in seconds to grab and filter for search on ground");
			descriptionAutoLoot.Add("AutoLoot.RunOnce", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.GetList(string)", new string[] { "string AutoLootItemListName" }, "List<AutoLootItem>", "Returns a list of the autoloot items in the loot list name passed in\n\tInvalid name specified will return None");
            descriptionAutoLoot.Add("AutoLoot.GetList", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.GetLootBag()", new string[] { "none" }, "int", "Returns the Serial of the assigned loot bag");
            descriptionAutoLoot.Add("AutoLoot.GetLootBag", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.SetNoOpenCorpse(True|False)", new string[] { "bool True/False" }, "bool", "Temporarily changes the Autoloot open corpse setting");
            descriptionAutoLoot.Add("AutoLoot.SetNoOpenCorpse", tooltip);

            #endregion

            #region Description Scavenger

            Dictionary<string, ToolTipDescriptions> descriptionScavenger = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Scavenger.Status()", new string[] { "none" }, "bool", "Get status of scavenger engine\n\tTrue: is running, False: is not running");
			descriptionScavenger.Add("Scavenger.Status", tooltip);

			tooltip = new ToolTipDescriptions("Scavenger.Start()", new string[] { "none" }, "void", "Start scavenger engine");
			descriptionScavenger.Add("Scavenger.Start", tooltip);

			tooltip = new ToolTipDescriptions("Scavenger.Stop()", new string[] { "none" }, "void", "Stop scavenger engine");
			descriptionScavenger.Add("Scavenger.Stop", tooltip);

			tooltip = new ToolTipDescriptions("Scavenger.ChangeList(string)", new string[] { "string ListName" }, "void", "Change list of scavenger item. List must exist in scavenger GUI configuration");
			descriptionScavenger.Add("Scavenger.ChangeList", tooltip);

			tooltip = new ToolTipDescriptions("Scavenger.RunOnce(ScavengerItem, double, Filter)()", new string[] { "ScavengerItem ItemList", "double DelayGrabInMs", "Filter FilterToSearch" }, "void", "Start scavenger with certain parameters. ScavengerItem is a list type for item\n\tdelay in seconds to grab and filter for search on ground");
			descriptionScavenger.Add("Scavenger.RunOnce", tooltip);

			#endregion

			#region Description Organizer

			Dictionary<string, ToolTipDescriptions> descriptionOrganizer = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Organizer.Status()", new string[] { "none" }, "bool", "Get status of organizer engine\n\tTrue: is running, False: is not running");
			descriptionOrganizer.Add("Organizer.Status", tooltip);

			tooltip = new ToolTipDescriptions("Organizer.FStart()", new string[] { "none" }, "void", "Start organizer engine");
			descriptionOrganizer.Add("Organizer.FStart", tooltip);

			tooltip = new ToolTipDescriptions("Organizer.FStop()", new string[] { "none" }, "void", "Stop organizer engine");
			descriptionOrganizer.Add("Organizer.FStop", tooltip);

			tooltip = new ToolTipDescriptions("Organizer.ChangeList(string)", new string[] { "strign ListName" }, "void", "Change list of organizer item. List must be exist in organizer GUI configuration");
			descriptionOrganizer.Add("Organizer.ChangeList", tooltip);

            tooltip = new ToolTipDescriptions("Organizer.RunOnce(int source, int dest, int dragDelay, string organizerName)", new string[] { "int source, int list, int dragDelay, string organizerName" }, "void", "Run organizer once with specific parameters");
            descriptionOrganizer.Add("Organizer.RunOnce", tooltip);

            #endregion

            #region Description Restock

            Dictionary<string, ToolTipDescriptions> descriptionRestock = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Restock.Status()", new string[] { "none" }, "bool", "Get status of restock engine\n\tTrue: is running, False: is not running");
			descriptionRestock.Add("Restock.Status", tooltip);

			tooltip = new ToolTipDescriptions("Restock.FStart()", new string[] { "none" }, "void", "Start restock engine");
			descriptionRestock.Add("Restock.FStart", tooltip);

			tooltip = new ToolTipDescriptions("Restock.FStop()", new string[] { "none" }, "void", "Stop restock engine");
			descriptionRestock.Add("Restock.FStop", tooltip);

			tooltip = new ToolTipDescriptions("Restock.ChangeList(string)", new string[] { "strign ListName" }, "void", "Change list of restock item. List must be exist in restock GUI configuration");
			descriptionRestock.Add("Restock.ChangeList", tooltip);

			#endregion

			#region Description SellAgent

			Dictionary<string, ToolTipDescriptions> descriptionSellAgent = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("SellAgent.Status()", new string[] { "none" }, "bool", "Get status of vendor sell filter\n\tTrue: enabled, False: disabled");
			descriptionSellAgent.Add("SellAgent.Status", tooltip);

			tooltip = new ToolTipDescriptions("SellAgent.Enable()", new string[] { "none" }, "void", "Enable vendor sell filter");
			descriptionSellAgent.Add("SellAgent.Enable", tooltip);

			tooltip = new ToolTipDescriptions("SellAgent.Disable()", new string[] { "none" }, "void", "Disable vendor sell filter");
			descriptionSellAgent.Add("SellAgent.Disable", tooltip);

			tooltip = new ToolTipDescriptions("SellAgent.ChangeList(string)", new string[] { "string ListName" }, "void", "Change list of vendor sell filter, List must be exist in vendor sell GUI configuration");
			descriptionSellAgent.Add("SellAgent.ChangeList", tooltip);

			#endregion

			#region Description BuyAgent

			Dictionary<string, ToolTipDescriptions> descriptionBuyAgent = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("BuyAgent.Status()", new string[] { "none" }, "bool", "Get status of vendor buy filter\n\tTrue: enabled, False: disabled");
			descriptionBuyAgent.Add("BuyAgent.Status", tooltip);

			tooltip = new ToolTipDescriptions("BuyAgent.Enable()", new string[] { "none" }, "void", "Enable vendor buy filter");
			descriptionBuyAgent.Add("BuyAgent.Enable", tooltip);

			tooltip = new ToolTipDescriptions("BuyAgent.Disable()", new string[] { "none" }, "void", "Disable vendor buy filter");
			descriptionBuyAgent.Add("BuyAgent.Disable", tooltip);

			tooltip = new ToolTipDescriptions("BuyAgent.ChangeList(string)", new string[] { "string ListName" }, "void", "Change list of vendor Buy filter, List must be exist in vendor Buy GUI configuration");
			descriptionBuyAgent.Add("BuyAgent.ChangeList", tooltip);

			#endregion

			#region Description Dress

			Dictionary<string, ToolTipDescriptions> descriptionDress = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Dress.DressStatus()", new string[] { "none" }, "bool", "Get status of dress engine\n\tTrue: is running, False: is not running");
			descriptionDress.Add("Dress.DressStatus", tooltip);

			tooltip = new ToolTipDescriptions("Dress.UnDressStatus()", new string[] { "none" }, "bool", "Get status of undress engine\n\tTrue: is running, False: is not running");
			descriptionDress.Add("Dress.UnDressStatus", tooltip);

			tooltip = new ToolTipDescriptions("Dress.DressFStart()", new string[] { "none" }, "void", "Start dress engine");
			descriptionDress.Add("Dress.DressFStart", tooltip);

			tooltip = new ToolTipDescriptions("Dress.UnDressFStart()", new string[] { "none" }, "void", "Start undress engine");
			descriptionDress.Add("Dress.UnDressFStart", tooltip);

			tooltip = new ToolTipDescriptions("Dress.DressFStop()", new string[] { "none" }, "void", "Stop dress engine");
			descriptionDress.Add("Dress.DressFStop", tooltip);

			tooltip = new ToolTipDescriptions("Dress.UnDressFStop()", new string[] { "none" }, "void", "Stop undress engine");
			descriptionDress.Add("Dress.UnDressFStop", tooltip);

			tooltip = new ToolTipDescriptions("Dress.ChangeList(string)", new string[] { "string ListName" }, "void", "Change item list of dress engine, List must be exist in dress / undress GUI configuration");
			descriptionDress.Add("Dress.ChangeList", tooltip);

			#endregion

			#region Description Friend

			Dictionary<string, ToolTipDescriptions> descriptionFriend = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Friend.IsFriend(int)", new string[] { "int SerialToSearch" }, "bool", "Check if serial is in friend list or not, if partyinclude option is active on GUI, search also in party\n\tTrue: found, False: not found");
			descriptionFriend.Add("Friend.IsFriend", tooltip);

			tooltip = new ToolTipDescriptions("Friend.ChangeList(string)", new string[] { "string ListName" }, "void", "Change friend list, List must be exist in friend list GUI configuration");
			descriptionFriend.Add("Friend.ChangeList", tooltip);

			tooltip = new ToolTipDescriptions("Friend.GetList(string)", new string[] { "string ListName" }, "<list> int", "Retrive list of serial in list, List must be exist in friend list GUI configuration");
			descriptionFriend.Add("Friend.GetList", tooltip);

			tooltip = new ToolTipDescriptions("Friend.AddPlayer(string, string, int)", new string[] { "string ListName, string PlayerName, int PlayerSerial" }, "void", "Add a player to the friends list as if added manually");
			descriptionFriend.Add("Friend.AddPlayer", tooltip);

			#endregion

			#region Description BandageHeal

			Dictionary<string, ToolTipDescriptions> descriptionBandageHeal = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("BandageHeal.Status()", new string[] { "none" }, "bool", "Get status of bandage heal engine\n\tTrue: is running, False: is not running");
			descriptionBandageHeal.Add("BandageHeal.Status", tooltip);

			tooltip = new ToolTipDescriptions("BandageHeal.Start()", new string[] { "none" }, "void", "Start bandage heal engine");
			descriptionBandageHeal.Add("BandageHeal.Start", tooltip);

			tooltip = new ToolTipDescriptions("BandageHeal.Stop()", new string[] { "none" }, "void", "Stop bandage heal engine");
			descriptionBandageHeal.Add("BandageHeal.Stop", tooltip);

			#endregion

			#region Description PathFinding

			Dictionary<string, ToolTipDescriptions> descriptionPathFinding = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("PathFinding.Route()", new string[] { "none" }, "new Route()", "Create a new instance for inizialize pathfinder");
			descriptionPathFinding.Add("PathFinding.Route", tooltip);

            tooltip = new ToolTipDescriptions("PathFinding.Go(Route)", new string[] { "Route PathfindingInstance" }, "void", "Start Pathfind movement");
            descriptionPathFinding.Add("PathFinding.Go", tooltip);

            tooltip = new ToolTipDescriptions("PathFinding.GetPath(int x, int y, bool avoidMobs = true )", new string[] { " (x, y) <int,int> destination world coordinates; avoidMobs <bool> find path without stepping on mobiles" }, "List<Tile> path", "Computes and returns a list of Tile (Tile.X, tile.Y) which rapresents the step-by-step path to the destination (x, y)");
            descriptionPathFinding.Add("PathFinding.GetPath", tooltip);

            tooltip = new ToolTipDescriptions("PathFinding.RunPath(List<Tile> path, float timeout = -1, bool debugMessage = false, bool useResync = true )", new string[] { " List<Tile> path (ex: generated by GetPath()\nfloat timeout: terminates after number of seconds\nbool debugMessage show true/false\n bool useResync Resync in case of walk faliure" }, "bool true: arrived; false: issue", "Computes and returns a list of Tile (Tile.X, tile.Y) which rapresents the step-by-step path to the destination ");
            descriptionPathFinding.Add("PathFinding.RunPath", tooltip);

            tooltip = new ToolTipDescriptions("PathFinding.Tile(int x, int y )", new string[] { " (x, y) <int,int> world coordinates " }, "new Tile()", "Returns a Tile object compabile with RunPath( List<Tile> path, ... ). Useful for custom made paths.");
            descriptionPathFinding.Add("PathFinding.Tile", tooltip);

            #endregion

            #region Description Timer

            Dictionary<string, ToolTipDescriptions> descriptionTimer = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Timer.Create()", new string[] { "string TimerName, int mstime" }, "void", "Create a timer object whit specific name and duration in ms");
            descriptionTimer.Add("Timer.Create", tooltip);

			tooltip = new ToolTipDescriptions("Timer.Check()", new string[] { "string TimerName" }, "bool", "Check if a timer object is expired or not, \n\t True if not expired, false if expired");
            descriptionTimer.Add("Timer.Check", tooltip);

            #endregion

            #region Description Vendor

            Dictionary<string, ToolTipDescriptions> descriptionVendor = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Vendor.Buy(vendorSerial, ItemId, amount)", new string[] { "int VendorSerial, int ItemID, int Amount" }, "void", "Send item buy request to specified vendor");
            descriptionVendor.Add("Vendor.Buy", tooltip);

            tooltip = new ToolTipDescriptions("Vendor.BuyList()", new string[] { "void" }, "List<BuyItems>", "Returns the Data associated with the last vendor buy list requested");
            descriptionVendor.Add("Vendor.BuyList", tooltip);

            #endregion

            #region Description DPSMeter

            Dictionary<string, ToolTipDescriptions> descriptionDPSMeter = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("DPSMeter.Status()", new string[] { "none" }, "bool", "Get status of DPS Meter engine\n\tTrue: is running, False: is not running");
			descriptionBandageHeal.Add("DPSMeter.Status", tooltip);

			tooltip = new ToolTipDescriptions("DPSMeter.Start()", new string[] { "none" }, "void", "Start Dps Meter engine");
			descriptionBandageHeal.Add("DPSMeter.Start", tooltip);

			tooltip = new ToolTipDescriptions("DPSMeter.Stop()", new string[] { "none" }, "void", "Stop Dps Meter engine");
			descriptionBandageHeal.Add("DPSMeter.Stop", tooltip);

			tooltip = new ToolTipDescriptions("DPSMeter.Pause()", new string[] { "none" }, "void", "Pause Dps Meter engine");
			descriptionBandageHeal.Add("DPSMeter.Pause", tooltip);

			tooltip = new ToolTipDescriptions("DPSMeter.GetDamage()", new string[] { "int serial" }, "int", "Get damage recorded by specific serial.");
			descriptionBandageHeal.Add("DPSMeter.GetDamage", tooltip);

			#endregion

			#region Description Statics

			Dictionary<string, ToolTipDescriptions> descriptionStatics = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("Statics.GetLandID(int, int, int)", new string[] { "int X", "int Y", "int MapValue" }, "int", "Get ID of tile in X, Y coordinates");
			descriptionStatics.Add("Statics.GetLandID", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetLandZ(int, int, int)", new string[] { "int X", "int Y", "int MapValue" }, "int", "Get Z level of tile in X, Y coordinates");
			descriptionStatics.Add("Statics.GetLandZ", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetStaticsTileInfo(int, int, int)", new string[] { "int X", "int Y", "int MapValue" }, "List<TileInfo>", "Get static tiles info in a certain map at X, Y coordinates");
			descriptionStatics.Add("Statics.GetStaticsTileInfo", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetLandName(int)", new string[] { "int LandID" }, "List<TileInfo>", "Get name given a spacific landID ( see: GetLandID )");
			descriptionStatics.Add("Statics.GetLandName", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetTileName(int)", new string[] { "int TileID" }, "List<TileInfo>", "Get name given a spacific tileID ( see: GetStaticsLandInfo )");
			descriptionStatics.Add("Statics.GetTileName", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetStaticsLandInfo(int, int, int)", new string[] { "int X", "int Y", "int MapValue" }, "List<TileInfo>", "Get land tile info in a certain map at X, Y coordinates");
			descriptionStatics.Add("Statics.GetStaticsLandInfo", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetTileFlag(int, string)", new string[] { "int itemID", "string flagname" }, "bool", "Get true or false if flag is present for specific static itemID\n\tSee wiki for flag name list.");
			descriptionStatics.Add("Statics.GetTileFlag", tooltip);

			tooltip = new ToolTipDescriptions("Statics.GetLandFlag(int, string)", new string[] { "int itemID", "string flagname" }, "bool", "Get true or false if flag is present for specific land itemID\n\tSee wiki for flag name list.");
			descriptionStatics.Add("Statics.GetLandFlag", tooltip);

			tooltip = new ToolTipDescriptions("Statics.CheckDeedHouse(int, int)", new string[] { "int X", "int Y" }, "bool", "Get true or false if deed house placed in specific coords.");
			descriptionStatics.Add("Statics.CheckDeedHouse", tooltip);

			#endregion

			#region Description Generics

			Dictionary<string, ToolTipDescriptions> descriptionGenerics = new Dictionary<string, ToolTipDescriptions>();

			tooltip = new ToolTipDescriptions("GetItemOnLayer(string)", new string[] { "string LayerName" }, "Item", "Retrieves a item value of item equipped on specific layer\n\tCheck the wiki for the possible strings\n\tWorks only on Mobile Instances");
			descriptionGenerics.Add("GetItemOnLayer", tooltip);

			tooltip = new ToolTipDescriptions("GetAssistantLayer(string)", new string[] { "string LayerName" }, "Layer", "Retrives HexID from the Layer's name\n\tWorks only on Mobile Instances");
			descriptionGenerics.Add("GetAssistantLayer", tooltip);

			tooltip = new ToolTipDescriptions("DistanceTo(Mobile)", new string[] { "Mobile MobileInstance" }, "int", "Return a value about distance from the mobile\n\tWorks only on Item Instances");
			descriptionGenerics.Add("DistanceTo", tooltip);

            #endregion
            #endregion

            Dictionary<string, ToolTipDescriptions> old_descriptionMethods =
				descriptionPlayer
				.Union(descriptionSpells)
				.Union(descriptionMobiles)
				.Union(descriptionItems)
				.Union(descriptionMisc)
				.Union(descriptionTarget)
				.Union(descriptionGumps)
				.Union(descriptionJournal)
				.Union(descriptionAutoLoot)
				.Union(descriptionScavenger)
				.Union(descriptionOrganizer)
				.Union(descriptionRestock)
				.Union(descriptionSellAgent)
				.Union(descriptionBuyAgent)
				.Union(descriptionDress)
				.Union(descriptionFriend)
				.Union(descriptionBandageHeal)
				.Union(descriptionStatics)
				.Union(descriptionDPSMeter)
				.Union(descriptionPathFinding)
				.Union(descriptionTimer)
                .Union(descriptionVendor)
				.ToDictionary(x => x.Key, x => x.Value);



            var autodocMethods = new Dictionary<string, ToolTipDescriptions>();
            foreach (var docitem in AutoDoc.GetPythonAPI().methods ) {
                var method = (DocMethod)docitem;
                var methodName = method.itemClass + "." + method.itemName;
                var prms_name = new List<String>();
                var prms_type = new List<String>();
                var prms_name_type = new List<String>();
                foreach (var prm in method.paramList) {
                    prms_name.Add(prm.itemName);
                    prms_type.Add(prm.itemType);
                    prms_name_type.Add(prm.itemType + " " + prm.itemName);
                }
                var methodSignNames = $"{methodName}({String.Join(",", prms_name)})";
                var methodSignTypes = $"{methodName}({String.Join(",", prms_type)})";
                var methodSignNameTypes = $"{methodName}({String.Join(",", prms_name_type)})";

                var methodKey = methodSignNames;
                tooltip = new ToolTipDescriptions(methodSignNames, prms_name_type.ToArray() , method.returnType, method.itemDescription.Trim()+"\n");
                if (autodocMethods.ContainsKey(methodKey))
                {
                    autodocMethods[methodKey].Notes += "\n"+ methodSignNameTypes;
                    if (method.itemDescription.Length > 0) {
                        autodocMethods[methodKey].Notes += "\n" + method.itemDescription.Trim()+"\n---";
                    }
                }
                else {
                    autodocMethods.Add(methodKey, tooltip);
                }

            }

            var descriptionMethods = descriptionGenerics.Union(autodocMethods).ToDictionary(x => x.Key, x => x.Value);




            //Dalamar
            //Remove this, it's just debug
            //REMOVE: begin

            /*
            //Classes
            var classes_diff = new List<String>(old_classes);
            foreach (var cls in classes) {
                //if found, remove because =. if not add as new methods
                if (!classes_diff.Remove(cls))
                {
                    classes_diff.Add("(new)" + cls);
                }
            }
            File.WriteAllText("classes_diff.csv", String.Join("\n", classes_diff));

            //Methods
            var methods_diff = new List<String>(old_methods);
            foreach (var mtd in methods)
            {
                //if found, remove because =. if not add as new methods
                if (!methods_diff.Remove(mtd))
                {
                    methods_diff.Add("(new)" + mtd);
                }
            }
            File.WriteAllText("methods_diff.csv", String.Join("\n", methods_diff));

            //Tooltips
            var desc_diff = new List<String>(old_descriptionMethods.Keys);
            foreach (var dsc in descriptionMethods.Keys)
            {
                //if found, remove because =. if not add as new methods
                if (!desc_diff.Remove(dsc))
                {
                    desc_diff.Add("(new)" + dsc);
                }
            }
            File.WriteAllText("tooltip_diff.csv", String.Join("\n", desc_diff));
            //REMOVE: end
            */

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

			//Metodi generici per instanze di tipo Item o Mobile
			Array.Sort(methodsGeneric);
			foreach (var item in methodsGeneric)
			{
				descriptionGenerics.TryGetValue(item, out ToolTipDescriptions element);

				if (element != null)
				{
					items.Add(new MethodAutocompleteItem(item)
					{
						ImageIndex = 3,
						ToolTipTitle = element.Title,
						ToolTipText = element.ToolTipDescription()
					});
				}
				else
				{
					items.Add(new MethodAutocompleteItem(item)
					{
						ImageIndex = 3
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

			this.Text = m_Title;

			m_pe = new PythonEngine();
			m_Engine = m_pe.engine;
			m_Scope = m_pe.scope;
			m_Engine.SetTrace(null);



			if (filename != null && File.Exists(filename))
			{
				m_Filepath = filename;
				m_Filename = Path.GetFileName(filename);
				this.Text = m_Title;
				fastColoredTextBoxEditor.Text = File.ReadAllText(filename);
			}
		}

        private void InitSyntaxtHighlight() {
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
                var checkUOS = text.Substring(0, 2);   // you want it to be UOS it better start with UOS style comment
                if (checkUOS == "//")
                {
                    string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    UOSteamEngine uosteam = new UOSteamEngine();
                    uosteam.Execute(lines);
                    SetErrorBox("Script " + m_Filename + " run completed!");
                    SetStatusLabel("IDLE", Color.DarkTurquoise);
                }
                else
                {

                    m_Engine.SetTrace(m_EnhancedScriptEditor.OnTraceback);

                    /*Dalamar: BEGIN "fix python env" */
                    //EXECUTION OF THE SCRIPT
                    //Refactoring option, the whole block can be replaced by:
                    //
                    //m_pe.Execute(text);

                    m_Source = m_Engine.CreateScriptSourceFromString(text);

                    // "+": USE PythonCompilerOptions in order to initialize Python modules correctly, without it the Python env is half broken
                    PythonCompilerOptions pco = (PythonCompilerOptions)m_Engine.GetCompilerOptions(m_Scope);
                    pco.ModuleName = "__main__";
                    pco.Module |= ModuleOptions.Initialize;
                    CompiledCode compiled = m_Source.Compile(pco);
                    compiled.Execute(m_Scope);

                    // "-": DONT execute directly, unless you are not planning to import external modules.
                    //m_Source.Execute(m_Scope);

                    /*Dalamar: END*/

                    SetErrorBox("Script " + m_Filename + " run completed!");
                    SetStatusLabel("IDLE", Color.DarkTurquoise);
                }
            }
            catch (IronPython.Runtime.Exceptions.SystemExitException ex)
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
                    ExceptionOperations eo = m_Engine.GetService<ExceptionOperations>();
                    string error = eo.FormatException(ex);
                    SetErrorBox("--> MESSAGE: " + error);
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

		private void toolStripButtonNextBreakpoint_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Breakpoint);
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
			CloseAndSave();
		}

		private void toolStripButtonInspect_Click(object sender, EventArgs e)
		{
			InspectEntities();
		}

		private void toolStripInspectGump_Click(object sender, EventArgs e)
		{
			InspectGumps();
		}

		private void toolStripRecord_Click(object sender, EventArgs e)
		{
			ScriptRecord();
		}

		private static void gumpinspector_close(object sender, EventArgs e)
		{
			Assistant.Engine.MainWindow.GumpInspectorEnable = false;
		}

		private void toolStripButtonSearch_Click(object sender, EventArgs e)
		{
			fastColoredTextBoxEditor.Focus();
			SendKeys.SendWait("^f");
		}

		private void Open()
		{
			OpenFileDialog open = new OpenFileDialog
			{
				Filter = "Script Files|*.py;*.txt;*.uos",
				RestoreDirectory = true
			};
			if (open.ShowDialog() == DialogResult.OK)
			{
				if (open.FileName != null && File.Exists(open.FileName))
				{
					m_Filename = Path.GetFileName(open.FileName);
					m_Filepath = open.FileName;
					this.Text = m_Title;
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

		private void SavaData(string path, string text)
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
				this.Text = m_Title;

				SavaData(m_Filepath, fastColoredTextBoxEditor.Text);

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
				Filter = "Python Files|*.py|Script Files|*.txt|UOSteam Files|*.uos",
				RestoreDirectory = true
			};
			save.InitialDirectory = Path.Combine(Assistant.Engine.RootPath, "Scripts");
			if (save.ShowDialog() == DialogResult.OK)
			{
				m_Filename = Path.GetFileName(save.FileName);
				this.Text = m_Title;
				m_Filepath = save.FileName;
				m_Filename = Path.GetFileName(save.FileName);
				SavaData(save.FileName, fastColoredTextBoxEditor.Text);
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
				this.Text = m_Title;
				return true;
			}

			if (fastColoredTextBoxEditor.Text == String.Empty) // Not ask to save empty text
				return true;

			DialogResult res = MessageBox.Show("Save current file?", "WARNING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
			if (res == System.Windows.Forms.DialogResult.Yes)
			{
				if (m_Filename != null && m_Filename != String.Empty)
				{
					SavaData(m_Filepath, fastColoredTextBoxEditor.Text);
					ReloadAfterSave();
				}
				else
				{
					SaveFileDialog save = new SaveFileDialog
					{
						Filter = "Script Files|*.py|Script Files|*.txt",
						FileName = m_Filename
					};

					if (save.ShowDialog() == DialogResult.OK)
					{
						if (save.FileName != null && save.FileName != string.Empty && fastColoredTextBoxEditor.Text != null)
						{
							SavaData(save.FileName, fastColoredTextBoxEditor.Text);
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
				this.Text = m_Title;
				return true;
			}
			else if (res == System.Windows.Forms.DialogResult.No)
			{
				fastColoredTextBoxEditor.Text = String.Empty;
				m_Filename = String.Empty;
				m_Filepath = String.Empty;
				this.Text = m_Title;
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
			ginspector.FormClosed += new FormClosedEventHandler(gumpinspector_close);
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

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fastColoredTextBoxEditor.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fastColoredTextBoxEditor.Paste();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fastColoredTextBoxEditor.Cut();
		}

		private void commentSelectLineToolStripMenuItem_Click(object sender, EventArgs e)
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

		private void unCommentLineToolStripMenuItem_Click(object sender, EventArgs e)
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

		private void messagelistBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (messagelistBox.SelectedItems == null) // Nothing selected
				return;

			if (e.Control && e.KeyCode == Keys.C)
			{
				Utility.ClipBoardCopy(String.Join(Environment.NewLine, messagelistBox.SelectedItems.Cast<string>()));
			}
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			messagelistBox.Items.Clear();
		}

		private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (messagelistBox.SelectedItems == null) // Nothing selected
				return;

			Utility.ClipBoardCopy(String.Join(Environment.NewLine, messagelistBox.SelectedItems.Cast<string>()));
		}

		private void toolStripInspectAlias_Click(object sender, EventArgs e)
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
			string complete_description = String.Empty;

			complete_description += "Parameters: ";

			foreach (string parameter in Parameters)
				complete_description += "\n\t" + parameter;

			complete_description += "\nReturns: " + Returns;

			complete_description += "\nDescription:";

            if (Description.Length > 0)
            {
                complete_description += "\n" + Description.Trim();
            }

            if (Notes.Length > 0){
                complete_description += "\n---" + Notes;
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

	/// <summary>
	/// This autocomplete item appears after dot
	/// </summary>
	public class SubPropertiesAutocompleteItem : MethodAutocompleteItem
	{
		string firstPart;
		string lastPart;

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
