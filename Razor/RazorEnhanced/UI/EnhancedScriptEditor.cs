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
		internal static FastColoredTextBox EnhancedScriptEditorTextArea { get { return m_EnhancedScriptEditor.fastColoredTextBoxEditor; } }
		private static ConcurrentQueue<Command> m_Queue = new ConcurrentQueue<Command>();
		private static Command m_CurrentCommand = Command.None;
		private static AutoResetEvent m_DebugContinue = new AutoResetEvent(false);

		private const string m_Title = "Enhanced Script Editor";
		private string m_Filename = "";
		private string m_Filepath = "";
		private static bool m_OnClosing = false;
		private static bool m_OnRecord = false;

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
				if (m_OnRecord)
				{
					m_OnRecord = false;
					ScriptRecorder.OnRecord = false;
                }
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
			m_popupMenu.ToolTipDuration = 5000;
			m_popupMenu.AppearInterval = 100;

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
		        "Player.Walk", "Player.PathFindTo", "Player.GetPropValue", "Player.GetPropStringByIndex", "GetPropStringList", "Player.QuestButton",
		        "Player.GuildButton", "Player.WeaponPrimarySA", "Player.WeaponSecondarySA", "Player.WeaponClearSA",
		        "Player.WeaponStunSA", "Player.WeaponDisarmSA"
		    };

		    string[] methodsSpells =
		    {
                "Spells.CastMagery", "Spells.CastNecro", "Spells.CastChivalry", "Spells.CastBushido", "Spells.CastNinjitsu", "Spells.CastSpellweaving", "Spells.CastMysticism"
            };

		    string[] methodsMobiles =
		    {
		        "Mobile.GetItemOnLayer", "Mobile.GetAssistantLayer", "Mobiles.FindBySerial", "Mobiles.UseMobile", "Mobiles.SingleClick",
		        "Mobiles.Filter", "Mobiles.ApplyFilter", "Mobiles.Message", "Mobiles.WaitForProps", "Mobiles.GetPropValue",
		        "Mobiles.GetPropStringByIndex", "Mobiles.GetPropStringList"
		    };

		    string[] methodsItems =
		    {
		        "Items.FindBySerial", "Items.Move", "Items.DropItemOnGroundSelf", "Items.UseItem", "Items.SingleClick",
                "Items.WaitForProps", "Items.GetPropValue", "Items.GetPropStringByIndex", "Items.GetPropStringList",
		        "Items.WaitForContents", "Items.Message", "Items.Filter", "Items.ApplyFilter", "Items.BackpackCount", "Items.ContainerCount"
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
                "Target.HasTarget", "Target.GetLast", "Target.GetLastAttack", "Target.WaitForTarget", "Target.TargetExecute", "Target.PromptTarget", "Target.Cancel", "Target.Last", "Target.LastQueued",
                "Target.Self", "Target.SelfQueued", "Target.SetLast", "Target.ClearLast", "Target.ClearQueue", "Target.ClearLastandQueue", "Target.SetLastTargetFromList",
                "Target.PerformTargetFromList", "Target.AttackTargetFromList"
            };

		    string[] methodsGumps =
		    {
		        "Gumps.CurrentGump", "Gumps.HasGump", "Gumps.CloseGump", "Gumps.WaitForGump", "Gumps.SendAction",
		        "Gumps.SendAdvancedAction", "Gumps.LastGumpGetLine", "Gumps.LastGumpGetLineList", "Gumps.LastGumpTextExist",
		        "Gumps.LastGumpTextExistByLine"
		    };

		    string[] methodsJournal =
		    {
		        "Journal.Clear", "Journal.Search", "Journal.SearchByName", "Journal.SearchByColor",
		        "Journal.SearchByType", "Journal.GetLineText", "Journal.GetSpeechName", "Journal.WaitJournal"
		    };

		    string[] methodsAutoLoot =
		    {
                "AutoLoot.Status", "AutoLoot.Start", "AutoLoot.Stop", "AutoLoot.ChangeList", "AutoLoot.RunOnce"
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
                    .Union(methodsGumps)
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
		        "Player.Followers", "Player.FollowersMax", "Player.MaxWeight", "Player.Str", "Player.Dex", "Player.Int"
		    };

            string[] propsPositions =
            {
                "Position.X", "Position.Y", "Position.Z"
            };

		    string[] propsWithCheck = propsPlayer.Union(propsPositions).ToArray();

		    string[] propsGeneric =
		    {
		        "Serial", "Hue", "Name", "Body", "Color", "Direction", "Visible", "Poisoned", "YellowHits", "Paralized",
		        "Human", "WarMode", "Female", "Hits", "MaxHits", "Stam", "StamMax", "Mana", "ManaMax", "Backpack", "Mount",
		        "Quiver", "Notoriety", "Map", "InParty", "Properties", "Amount", "IsBagOfSending", "IsContainer", "IsCorpse",
		        "IsDoor", "IsInBank", "Movable", "OnGround", "ItemID", "RootContainer", "Durability", "MaxDurability",
		        "Contains", "Weight", "Position", "StaticID", "StaticHue", "StaticZ"
            };

            string[] props = propsGeneric;

            #endregion

            #region Methods Descriptions

            ToolTipDescriptions tooltip;

            #region Description Player

            Dictionary<string, ToolTipDescriptions> descriptionPlayer = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Player.BuffsExist(string)", new string[] { "string BuffName" }, "bool", "Ammaccabanana");
            descriptionPlayer.Add("Player.BuffsExist", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetBuffDescription(BuffIcon)", new string[] { "BuffIcon Name" }, "string", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetBuffDescription", tooltip);

            tooltip = new ToolTipDescriptions("Player.HeadMessage(int, string)", new string[] { "int MessageColor", "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.HeadMessage", tooltip);

            tooltip = new ToolTipDescriptions("Player.InRangeMobile(Mobile or int, int)", new string[] { "Mobile MobileToCheck or int SerialMobileToCheck", "int range" }, "bool", "Ammaccabanana");
            descriptionPlayer.Add("Player.InRangeMobile", tooltip);

            tooltip = new ToolTipDescriptions("Player.InRangeItem(Item or int, int)", new string[] { "Item ItemToCheck or int SerialItemToCheck", "int range" }, "bool", "Ammaccabanana");
            descriptionPlayer.Add("Player.InRangeItem", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetItemOnLayer(string)", new string[] { "string LayerName" }, "Item", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetItemOnLayer", tooltip);

            tooltip = new ToolTipDescriptions("Player.UnEquipItemByLayer(string)", new string[] { "string LayerName" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.UnEquipItemByLayer", tooltip);

            tooltip = new ToolTipDescriptions("Player.EquipItem(Item or int)", new string[] { "Item ItemInstance or int SerialItem" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.EquipItem", tooltip);

            tooltip = new ToolTipDescriptions("Player.CheckLayer(string)", new string[] { "string LayerName" }, "bool", "Ammaccabanana");
            descriptionPlayer.Add("Player.CheckLayer", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetAssistantLayer(string)", new string[] { "string LayerName" }, "Layer", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetAssistantLayer", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetSkillValue(string)", new string[] { "string SkillName" }, "dobule", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetSkillValue", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetSkillCap(string)", new string[] { "string SkillName" }, "double", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetSkillCap", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetSkillStatus(string)", new string[] { "string SkillName" }, "int", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetSkillStatus", tooltip);

            tooltip = new ToolTipDescriptions("Player.UseSkill(string)", new string[] { "string SkillName" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.UseSkill", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatSay(int, string)", new string[] { "int MessageColor", "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatSay", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatEmote(int, string)", new string[] { "int MessageColor", "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatEmote", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatWhisper(int, string)", new string[] { "int MessageColor", "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatWhisper", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatYell(int, string)", new string[] { "int MessageColor", "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatYell", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatGuild(string)", new string[] { "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatGuild", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatAlliance(string)", new string[] { "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatAlliance", tooltip);

            tooltip = new ToolTipDescriptions("Player.SetWarMode(bool)", new string[] { "bool WarStatus" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.SetWarMode", tooltip);

            tooltip = new ToolTipDescriptions("Player.Attack(int)", new string[] { "int TargetSerial" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.Attack", tooltip);

            tooltip = new ToolTipDescriptions("Player.AttackLast()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.AttackLast", tooltip);

            tooltip = new ToolTipDescriptions("Player.InParty()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionPlayer.Add("Player.InParty", tooltip);

            tooltip = new ToolTipDescriptions("Player.ChatParty(string)", new string[] { "string Message" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.ChatParty", tooltip);

            tooltip = new ToolTipDescriptions("Player.PartyCanLoot(bool)", new string[] { "bool Flag" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.PartyCanLoot", tooltip);

            tooltip = new ToolTipDescriptions("Player.PartyInvite()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.PartyInvite", tooltip);

            tooltip = new ToolTipDescriptions("Player.PartyLeave()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.PartyLeave", tooltip);

            tooltip = new ToolTipDescriptions("Player.KickMember(int)", new string[] { "int SerialPersonToKick" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.KickMember", tooltip);

            tooltip = new ToolTipDescriptions("Player.InvokeVirtue(string)", new string[] { "string VirtueName" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.InvokeVirtue", tooltip);

            tooltip = new ToolTipDescriptions("Player.Walk(string)", new string[] { "string Direction" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.Walk", tooltip);

            tooltip = new ToolTipDescriptions("Player.PathFindTo(Point3D or (int, int, int))", new string[] { "Point3D Coords or ( int X, int Y, int Z )" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.PathFindTo", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetPropValue(string)", new string[] { "string PropName" }, "int", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetPropValue", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetPropStringByIndex(int)", new string[] { "int PropIndex" }, "string", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetPropStringByIndex", tooltip);

            tooltip = new ToolTipDescriptions("Player.GetPropStringList()", new string[] { "none" }, "List<string>", "Ammaccabanana");
            descriptionPlayer.Add("Player.GetPropStringList", tooltip);

            tooltip = new ToolTipDescriptions("Player.QuestButton()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.QuestButton", tooltip);

            tooltip = new ToolTipDescriptions("Player.GuildButton()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.GuildButton", tooltip);

            tooltip = new ToolTipDescriptions("Player.WeaponPrimarySA()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.WeaponPrimarySA", tooltip);

            tooltip = new ToolTipDescriptions("Player.WeaponSecondarySA()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.WeaponSecondarySA", tooltip);

            tooltip = new ToolTipDescriptions("Player.WeaponClearSA()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.WeaponClearSA", tooltip);

            tooltip = new ToolTipDescriptions("Player.WeaponStunSA()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.WeaponStunSA", tooltip);

            tooltip = new ToolTipDescriptions("Player.WeaponDisarmSA()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionPlayer.Add("Player.WeaponDisarmSA", tooltip);

            #endregion

            #region Description Spells

            Dictionary<string, ToolTipDescriptions> descriptionSpells = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Spells.CastMagery(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastMagery", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastNecro(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastNecro", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastChivalry(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastChivalry", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastBushido(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastBushido", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastNinjitsu(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastNinjitsu", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastSpellweaving(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastSpellweaving", tooltip);

            tooltip = new ToolTipDescriptions("Spells.CastMysticism(string)", new string[] { "string SpellName" }, "void", "Ammaccabanana");
            descriptionSpells.Add("Spells.CastMysticism", tooltip);

            #endregion

            #region Description Mobiles

            Dictionary<string, ToolTipDescriptions> descriptionMobiles = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Mobiles.FindBySerial(int)", new string[] { "int MobileSerial" }, "Mobile", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.FindBySerial", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.UseMobile(Mobile or int)", new string[] { "Mobile MobileIstance or int MobileSerial" }, "void", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.UseMobile", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.SingleClick(Mobile or int)", new string[] { "Mobile MobileIstance or int MobileSerial" }, "void", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.SingleClick", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.Filter()", new string[] { "none" }, "Filter", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.Filter", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.ApplyFilter(Filter)", new string[] { "Filter MobileFilter" }, "List<Mobile>", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.ApplyFilter", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.Message(Mobile or int, int string)", new string[] { "Mobile MobileIstance or int MobileSerial", "int ColorMessage", "string Message" }, "void", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.Message", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.WaitForProps(Mobile or int, int)", new string[] { "Mobile MobileIstance or int MobileSerial", "int TimeoutProps" }, "void", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.WaitForProps", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.GetPropValue(Mobile or int, string)", new string[] { "Mobile MobileIstance or int MobileSerial", "string PropName" }, "int", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.GetPropValue", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.GetPropStringByIndex(Mobile or int, int)", new string[] { "Mobile MobileIstance or int MobileSerial", "int PropIndex" }, "string", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.GetPropStringByIndex", tooltip);

            tooltip = new ToolTipDescriptions("Mobiles.GetPropStringList(Mobile or int)", new string[] { "Mobile MobileIstance or int MobileSerial" }, "List<string>", "Ammaccabanana");
            descriptionMobiles.Add("Mobiles.GetPropStringList", tooltip);

            #endregion

            #region Description Items

            Dictionary<string, ToolTipDescriptions> descriptionItems = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Items.FindBySerial(int)", new string[] { "int ItemSerial" }, "Item", "Ammaccabanana");
            descriptionItems.Add("Items.FindBySerial", tooltip);

            tooltip = new ToolTipDescriptions("Items.Move(Item or int, Item or Mobile or int, int)", new string[] { "Item Source or int SourceItemSerial", "Item DestinationItem or Mobile DestinationMobile or int DestinationSerial", "int AmountToMove" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.Move", tooltip);

            tooltip = new ToolTipDescriptions("Items.DropItemGroundSelf(Item, int)", new string[] { "Item ItemInstance", "int Amount" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.DropItemGroundSelf", tooltip);

            tooltip = new ToolTipDescriptions("Items.UseItem(Item or int)", new string[] { "Item ItemInstance or int ItemSerial" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.UseItem", tooltip);

            tooltip = new ToolTipDescriptions("Items.SingleClick(Item or int)", new string[] { "Item ItemInstance or int ItemSerial" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.SingleClick", tooltip);

            tooltip = new ToolTipDescriptions("Items.WaitForProps(Item or int, int)", new string[] { "Item ItemInstance or int ItemSerial", "int TimeoutProps" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.WaitForProps", tooltip);

            tooltip = new ToolTipDescriptions("Items.GetPropValue(Item or int, string)", new string[] { "Item ItemInstance or int ItemSerial", "string PropName" }, "int", "Ammaccabanana");
            descriptionItems.Add("Items.GetPropValue", tooltip);

            tooltip = new ToolTipDescriptions("Items.GetPropStringByIndex(Item or int, int)", new string[] { "Item ItemInstance or int ItemSerial", "int PropIndex" }, "string", "Ammaccabanana");
            descriptionItems.Add("Items.GetPropStringByIndex", tooltip);

            tooltip = new ToolTipDescriptions("Items.GetPropStringList(Item or int)", new string[] { "Item ItemInstance or int ItemSerial" }, "List<string>", "Ammaccabanana");
            descriptionItems.Add("Items.GetPropStringList", tooltip);

            tooltip = new ToolTipDescriptions("Items.WaitForContents(Item or int, int)", new string[] { "Item ItemInstance or int ItemSerial", "int TimeoutContents" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.WaitForContents", tooltip);

            tooltip = new ToolTipDescriptions("Items.Message(Item or int, int, string)", new string[] { "Item ItemInstance or int ItemSerial", "int MessageColor", "string Message" }, "void", "Ammaccabanana");
            descriptionItems.Add("Items.Message", tooltip);

            tooltip = new ToolTipDescriptions("Items.Filter()", new string[] { "none" }, "Filter", "Ammaccabanana");
            descriptionItems.Add("Items.Filter", tooltip);

            tooltip = new ToolTipDescriptions("Items.ApplyFilter(Filter)", new string[] { "Filter ItemFilter" }, "List<Item>", "Ammaccabanana");
            descriptionItems.Add("Items.ApplyFilter", tooltip);

            tooltip = new ToolTipDescriptions("Items.BackpackCount(int, int)", new string[] { "int ItemID", "int Color" }, "int", "Ammaccabanana");
            descriptionItems.Add("Items.BackpackCount", tooltip);

            tooltip = new ToolTipDescriptions("Items.ContainerCount(item or int, int, int)", new string[] { "Item Container or int ContainerSerial", "int ItemID", "int Color" }, "List<Item>", "Ammaccabanana");
            descriptionItems.Add("Items.ContainerCount", tooltip);
            
            #endregion

            #region Description Misc

            Dictionary<string, ToolTipDescriptions> descriptionMisc = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Misc.SendMessage(string or int or bool, (optional)int)", new string[] { "string Message or int Value or bool Status", "int Color" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.SendMessage", tooltip);

            tooltip = new ToolTipDescriptions("Misc.Resync()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.Resync", tooltip);

            tooltip = new ToolTipDescriptions("Misc.Pause(int)", new string[] { "int Delay" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.Pause", tooltip);

            tooltip = new ToolTipDescriptions("Misc.Beep()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.Beep", tooltip);

            tooltip = new ToolTipDescriptions("Misc.Disconnect()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.Disconnect", tooltip);

            tooltip = new ToolTipDescriptions("Misc.WaitForContext(int or Mobile or Item, int)", new string[] { "int Serial or Mobile MobileInstance or Item ItemInstance", "int Timeout" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.WaitForContext", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ContextReply(int or Mobile or Item, int)", new string[] { "int Serial or Mobile MobileInstance or Item ItemInstance", "int MenuID" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.ContextReply", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ReadSharedValue(string)", new string[] { "string NameOfValue" }, "object", "Ammaccabanana");
            descriptionMisc.Add("Misc.ReadSharedValue", tooltip);

            tooltip = new ToolTipDescriptions("Misc.RemoveSharedValue(string)", new string[] { "string NameOfValue" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.RemoveSharedValue", tooltip);

            tooltip = new ToolTipDescriptions("Misc.CheckSharedValue(string)", new string[] { "string NameOfValue" }, "bool", "Ammaccabanana");
            descriptionMisc.Add("Misc.CheckSharedValue", tooltip);

            tooltip = new ToolTipDescriptions("Misc.SetSharedValue(string, object)", new string[] { "string NameOfValue", "object ValueToSet" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.SetSharedValue", tooltip);

            tooltip = new ToolTipDescriptions("Misc.HasMenu()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionMisc.Add("Misc.HasMenu", tooltip);

            tooltip = new ToolTipDescriptions("Misc.CloseMenu()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.CloseMenu", tooltip);

            tooltip = new ToolTipDescriptions("Misc.MenuContains(string)", new string[] { "string TextToSearch" }, "bool", "Ammaccabanana");
            descriptionMisc.Add("Misc.MenuContains", tooltip);

            tooltip = new ToolTipDescriptions("Misc.GetMenuTitle()", new string[] { "none" }, "string", "Ammaccabanana");
            descriptionMisc.Add("Misc.GetMenuTitle", tooltip);

            tooltip = new ToolTipDescriptions("Misc.WaitForMenu(int)", new string[] { "int Timeout" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.WaitForMenu", tooltip);

            tooltip = new ToolTipDescriptions("Misc.MenuResponse(string)", new string[] { "string SubmitName" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.MenuResponse", tooltip);

            tooltip = new ToolTipDescriptions("Misc.HasQueryString()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionMisc.Add("Misc.HasQueryString", tooltip);

            tooltip = new ToolTipDescriptions("Misc.WaitForQueryString(int)", new string[] { "int Timeout" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.WaitForQueryString", tooltip);

            tooltip = new ToolTipDescriptions("Misc.QueryStringResponse(bool, string)", new string[] { "bool YesCancelStatus", "string StringToResponse" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.QueryStringResponse", tooltip);

            tooltip = new ToolTipDescriptions("Misc.NoOperation()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.NoOperation", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ScriptRun(string)", new string[] { "string ScriptFilename" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.ScriptRun", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ScriptStop(string)", new string[] { "string ScriptFilename" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.ScriptStop", tooltip);

            tooltip = new ToolTipDescriptions("Misc.ScriptStatus(string)", new string[] { "string ScriptFilename" }, "bool", "Ammaccabanana");
            descriptionMisc.Add("Misc.ScriptStatus", tooltip);

            tooltip = new ToolTipDescriptions("Misc.PetRename(Mobile or int, string)", new string[] { "Mobile MobileInstance or int MobileSerial", "string NewName" }, "void", "Ammaccabanana");
            descriptionMisc.Add("Misc.PetRename", tooltip);

            #endregion

            #region Description Target

            Dictionary<string, ToolTipDescriptions> descriptionTarget = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Target.HasTarget()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionTarget.Add("Target.HasTarget", tooltip);

            tooltip = new ToolTipDescriptions("Target.GetLast()", new string[] { "none" }, "int", "Ammaccabanana");
            descriptionTarget.Add("Target.GetLast", tooltip);

            tooltip = new ToolTipDescriptions("Target.GetLastAttack()", new string[] { "none" }, "int", "Ammaccabanana");
            descriptionTarget.Add("Target.GetLastAttack", tooltip);

            tooltip = new ToolTipDescriptions("Target.WaitForTarget(int)", new string[] { "int TimeoutTarget" }, "int", "Ammaccabanana");
            descriptionTarget.Add("Target.WaitForTarget", tooltip);

            tooltip = new ToolTipDescriptions("Target.TargetExecute(int or Item or Mobile or (int, int, int, (optional)int))", new string[] { "int Serial or Item ItemInstance or Mobile MobileInstance or ( int X, int Y, int Z, int TileID )" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.TargetExecute", tooltip);

            tooltip = new ToolTipDescriptions("Target.PromptTarget()", new string[] { "none" }, "int", "Ammaccabanana");
            descriptionTarget.Add("Target.PromptTarget", tooltip);

            tooltip = new ToolTipDescriptions("Target.Cancel()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.Cancel", tooltip);

            tooltip = new ToolTipDescriptions("Target.Last()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.Last", tooltip);

            tooltip = new ToolTipDescriptions("Target.LastQueued()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.LastQueued", tooltip);

            tooltip = new ToolTipDescriptions("Target.Self()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.Self", tooltip);

            tooltip = new ToolTipDescriptions("Target.SelfQueued()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.SelfQueued", tooltip);

            tooltip = new ToolTipDescriptions("Target.SetLast(Mobile or int)", new string[] { "Mobile MobileTarget or int TargetSerial" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.SetLast", tooltip);

            tooltip = new ToolTipDescriptions("Target.ClearLast()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.ClearLast", tooltip);

            tooltip = new ToolTipDescriptions("Target.ClearQueue()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.ClearQueue", tooltip);

            tooltip = new ToolTipDescriptions("Target.ClearLastandQueue()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionTarget.Add("Target.ClearLastandQueue", tooltip);

            tooltip = new ToolTipDescriptions("Target.SetLastTargetFromList(string)", new string[] { "string TargetFilterName" }, "bool", "Ammaccabanana");
            descriptionTarget.Add("Target.SetLastTargetFromList", tooltip);

            tooltip = new ToolTipDescriptions("Target.PerformTargetFromList(string)", new string[] { "string TargetFilterName" }, "bool", "Ammaccabanana");
            descriptionTarget.Add("Target.PerformTargetFromList", tooltip);

            tooltip = new ToolTipDescriptions("Target.AttackTargetFromList(string)", new string[] { "string TargetFilterName" }, "bool", "Ammaccabanana");
            descriptionTarget.Add("Target.AttackTargetFromList", tooltip);

            #endregion

            #region Description Gumps

            Dictionary<string, ToolTipDescriptions> descriptionGumps = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Gumps.CurrentGump()", new string[] { "none" }, "uint", "Ammaccabanana");
            descriptionGumps.Add("Gumps.CurrentGump", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.HasGump()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionGumps.Add("Gumps.HasGump", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.CloseGump(uint)", new string[] { "uint GumpID" }, "void", "Ammaccabanana");
            descriptionGumps.Add("Gumps.CloseGump", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.WaitForGump(uint, int)", new string[] { "uint GumpID", "int TimeoutGump" }, "void", "Ammaccabanana");
            descriptionGumps.Add("Gumps.WaitForGump", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.SendAction(uint, int)", new string[] { "uint GumpID", "int ButtonID" }, "void", "Ammaccabanana");
            descriptionGumps.Add("Gumps.SendAction", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.SendAdvancedAction(uint, int, List<int>, (optional)List<int>, (optional)List<string>)", new string[] { "uint GumpID", "int ButtonID", "List<int> Switches", "List<int> TextID", "List<string> Texts" }, "void", "Ammaccabanana");
            descriptionGumps.Add("Gumps.SendAdvancedAction", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.LastGumpGetLine(int)", new string[] { "int LineNumber" }, "string", "Ammaccabanana");
            descriptionGumps.Add("Gumps.LastGumpGetLine", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.LastGumpGetLineList()", new string[] { "none" }, "List<string>", "Ammaccabanana");
            descriptionGumps.Add("Gumps.LastGumpGetLineList", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.LastGumpTextExist(string)", new string[] { "string TextToSearch" }, "bool", "Ammaccabanana");
            descriptionGumps.Add("Gumps.LastGumpTextExist", tooltip);

            tooltip = new ToolTipDescriptions("Gumps.LastGumpTextExistByLine(int, string)", new string[] { "int LineNumber", "string TextToSearch" }, "bool", "Ammaccabanana");
            descriptionGumps.Add("Gumps.LastGumpTextExistByLine", tooltip);

            #endregion

            #region Description Journal

            Dictionary<string, ToolTipDescriptions> descriptionJournal = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Journal.Clear()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionJournal.Add("Journal.Clear", tooltip);

            tooltip = new ToolTipDescriptions("Journal.Search(string)", new string[] { "string TextToSearch" }, "bool", "Ammaccabanana");
            descriptionJournal.Add("Journal.Search", tooltip);

            tooltip = new ToolTipDescriptions("Journal.SearchByName(string, string)", new string[] { "string TextToSearch", "string SenderName" }, "bool", "Ammaccabanana");
            descriptionJournal.Add("Journal.SearchByName", tooltip);

            tooltip = new ToolTipDescriptions("Journal.SearchByColor(string, int)", new string[] { "string TextToSearch", "int ColorToSearch" }, "bool", "Ammaccabanana");
            descriptionJournal.Add("Journal.SearchByColor", tooltip);

            tooltip = new ToolTipDescriptions("Journal.SearchByType(string, string)", new string[] { "string TextToSearch", "string MessageType" }, "bool", "Ammaccabanana");
            descriptionJournal.Add("Journal.SearchByType", tooltip);

            tooltip = new ToolTipDescriptions("Journal.GetLineText(string)", new string[] { "string TextToSearch" }, "string", "Ammaccabanana");
            descriptionJournal.Add("Journal.GetLineText", tooltip);

            tooltip = new ToolTipDescriptions("Journal.GetSpeechName()", new string[] { "none" }, "List<string>", "Ammaccabanana");
            descriptionJournal.Add("Journal.GetSpeechName", tooltip);

            tooltip = new ToolTipDescriptions("Journal.WaitJournal(string, int)", new string[] { "string TextToSearch", "int TimeoutJournal" }, "void", "Ammaccabanana");
            descriptionJournal.Add("Journal.WaitJournal", tooltip);

            #endregion

            #region Description AutoLoot

            Dictionary<string, ToolTipDescriptions> descriptionAutoLoot = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("AutoLoot.Status()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionAutoLoot.Add("AutoLoot.Status", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.Start()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionAutoLoot.Add("AutoLoot.Start", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.Stop()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionAutoLoot.Add("AutoLoot.Stop", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.ChangeList(string)", new string[] { "string ListName" }, "void", "Ammaccabanana");
            descriptionAutoLoot.Add("AutoLoot.ChangeList", tooltip);

            tooltip = new ToolTipDescriptions("AutoLoot.RunOnce(AutoLootItem, double, Filter)", new string[] { "AutoLootItem ItemList", "double DelayGrabInMs", "Filter FilterToSearch" }, "void", "Ammaccabanana");
            descriptionAutoLoot.Add("AutoLoot.RunOnce", tooltip);

            #endregion

            #region Description Scavenger

            Dictionary<string, ToolTipDescriptions> descriptionScavenger = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Scavenger.Status()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionScavenger.Add("Scavenger.Status", tooltip);

            tooltip = new ToolTipDescriptions("Scavenger.Start()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionScavenger.Add("Scavenger.Start", tooltip);

            tooltip = new ToolTipDescriptions("Scavenger.Stop()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionScavenger.Add("Scavenger.Stop", tooltip);

            tooltip = new ToolTipDescriptions("Scavenger.ChangeList(string)", new string[] { "string ListName" }, "void", "Ammaccabanana");
            descriptionScavenger.Add("Scavenger.ChangeList", tooltip);

            tooltip = new ToolTipDescriptions("Scavenger.RunOnce(ScavengerItem, double, Filter)()", new string[] { "ScavengerItem ItemList", "double DelayGrabInMs", "Filter FilterToSearch" }, "void", "Ammaccabanana");
            descriptionScavenger.Add("Scavenger.RunOnce", tooltip);

            #endregion

            #region Description Restock

            Dictionary<string, ToolTipDescriptions> descriptionRestock = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Restock.Status()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionRestock.Add("Restock.Status", tooltip);

            tooltip = new ToolTipDescriptions("Restock.FStart()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionRestock.Add("Restock.FStart", tooltip);

            tooltip = new ToolTipDescriptions("Restock.FStop()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionRestock.Add("Restock.FStop", tooltip);

            tooltip = new ToolTipDescriptions("Restock.ChangeList(string)", new string[] { "strign ListName" }, "void", "Ammaccabanana");
            descriptionRestock.Add("Restock.ChangeList", tooltip);

            #endregion

            #region Description SellAgent

            Dictionary<string, ToolTipDescriptions> descriptionSellAgent = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("SellAgent.Status()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionSellAgent.Add("SellAgent.Status", tooltip);

            tooltip = new ToolTipDescriptions("SellAgent.Enable()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionSellAgent.Add("SellAgent.Enable", tooltip);

            tooltip = new ToolTipDescriptions("SellAgent.Disable()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionSellAgent.Add("SellAgent.Disable", tooltip);

            tooltip = new ToolTipDescriptions("SellAgent.ChangeList(string)", new string[] { "string ListName" }, "void", "Ammaccabanana");
            descriptionSellAgent.Add("SellAgent.ChangeList", tooltip);

            #endregion

            #region Description BuyAgent

            Dictionary<string, ToolTipDescriptions> descriptionBuyAgent = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("SellAgent.Status()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionBuyAgent.Add("BuyAgent.Status", tooltip);

            tooltip = new ToolTipDescriptions("BuyAgent.Enable()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionBuyAgent.Add("BuyAgent.Enable", tooltip);

            tooltip = new ToolTipDescriptions("BuyAgent.Disable()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionBuyAgent.Add("BuyAgent.Disable", tooltip);

            tooltip = new ToolTipDescriptions("BuyAgent.ChangeList(string)", new string[] { "string ListName" }, "void", "Ammaccabanana");
            descriptionBuyAgent.Add("BuyAgent.ChangeList", tooltip);

            #endregion

            #region Description Dress

            Dictionary<string, ToolTipDescriptions> descriptionDress = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Dress.DessStatus()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionDress.Add("Dress.DessStatus", tooltip);

            tooltip = new ToolTipDescriptions("Dress.UnDressStatus()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionDress.Add("Dress.UnDressStatus", tooltip);

            tooltip = new ToolTipDescriptions("Dress.DressFStart()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionDress.Add("Dress.DressFStart", tooltip);

            tooltip = new ToolTipDescriptions("Dress.UnDressFStart()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionDress.Add("Dress.UnDressFStart", tooltip);

            tooltip = new ToolTipDescriptions("Dress.DressFStop()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionDress.Add("Dress.DressFStop", tooltip);

            tooltip = new ToolTipDescriptions("Dress.UnDressFStop()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionDress.Add("Dress.UnDressFStop", tooltip);

            tooltip = new ToolTipDescriptions("Dress.ChangeList(string)", new string[] { "string ListName" }, "void", "Ammaccabanana");
            descriptionDress.Add("Dress.ChangeList", tooltip);

            #endregion

            #region Description Friend

            Dictionary<string, ToolTipDescriptions> descriptionFriend = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Friend.IsFriend(int)", new string[] { "int SerialToSearch" }, "bool", "Ammaccabanana");
            descriptionFriend.Add("Friend.IsFriend", tooltip);

            tooltip = new ToolTipDescriptions("Friend.ChangeList(string)", new string[] { "string ListName" }, "void", "Ammaccabanana");
            descriptionFriend.Add("Friend.ChangeList", tooltip);
            
            #endregion

            #region Description BandageHeal

            Dictionary<string, ToolTipDescriptions> descriptionBandageHeal = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("BandageHeal.Status()", new string[] { "none" }, "bool", "Ammaccabanana");
            descriptionBandageHeal.Add("BandageHeal.Status", tooltip);

            tooltip = new ToolTipDescriptions("BandageHeal.Start()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionBandageHeal.Add("BandageHeal.Start", tooltip);

            tooltip = new ToolTipDescriptions("BandageHeal.Stop()", new string[] { "none" }, "void", "Ammaccabanana");
            descriptionBandageHeal.Add("BandageHeal.Stop", tooltip);

            #endregion

            #region Description Statics

            Dictionary<string, ToolTipDescriptions> descriptionStatics = new Dictionary<string, ToolTipDescriptions>();

            tooltip = new ToolTipDescriptions("Statics.GetLandID(int, int, int)", new string[] { "int X", "int Y", "int MapValue" }, "int", "Ammaccabanana");
            descriptionStatics.Add("Statics.GetLandID", tooltip);

            tooltip = new ToolTipDescriptions("Statics.GetLandZ(int, int, int)", new string[]  { "int X", "int Y", "int MapValue" }, "int", "Ammaccabanana");
            descriptionStatics.Add("Statics.GetLandZ", tooltip);

            tooltip = new ToolTipDescriptions("Statics.GetStaticsTileInfo(int, int, int)", new string[] { "int X", "int Y", "int MapValue" }, "List<TileInfo>", "Ammaccabanana");
            descriptionStatics.Add("Statics.GetStaticsTileInfo", tooltip);

            #endregion

            Dictionary<string, ToolTipDescriptions> descriptionMethods =
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
                .Union(descriptionRestock)
                .Union(descriptionSellAgent)
                .Union(descriptionBuyAgent)
                .Union(descriptionDress)
                .Union(descriptionFriend)
                .Union(descriptionBandageHeal)
                .Union(descriptionStatics)
                .ToDictionary(x => x.Key, x => x.Value);

            #endregion

            List<AutocompleteItem> items = new List<AutocompleteItem>();

            //Permette la creazione del menu con la singola keyword
            Array.Sort(keywords);
		    foreach (var item in keywords)
		        items.Add(new AutocompleteItem(item) {ImageIndex = 0});
		    //Permette la creazione del menu con la singola classe
            Array.Sort(classes);
		    foreach (var item in classes)
		        items.Add(new AutocompleteItem(item) {ImageIndex = 1});

		    //Permette di creare il menu solo per i metodi della classe digitata
            Array.Sort(methods);
		    foreach (var item in methods)
		    {
                ToolTipDescriptions element;
                descriptionMethods.TryGetValue(item, out element);

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
		        items.Add(new SubPropertiesAutocompleteItem(item) {ImageIndex = 4});

		    //Props generiche divise tra quelle Mobiles e Items, che possono
            //Appartenere a variabili istanziate di una certa classe
            //Qui sta alla cura dell'utente capire se una props va bene o no
            //Per quella istanza
            Array.Sort(props);
		    foreach (var item in props)
		        items.Add(new MethodAutocompleteItem(item) {ImageIndex = 3});

            m_popupMenu.Items.SetAutocompleteItems(items);

            //Aumenta la larghezza per i singoli item, in modo che l'intero nome sia visibile
            m_popupMenu.Items.MaximumSize = new Size(m_popupMenu.Items.Width + 20, m_popupMenu.Items.Height);
            m_popupMenu.Items.Width = m_popupMenu.Items.Width + 20;

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
			if(m_OnClosing)
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
			if (m_OnClosing)
				return;

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
			End();
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

        private void toolStripInspectGump_Click(object sender, EventArgs e)
        {
            InspectGumps();
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

		private void toolStripRecord_Click(object sender, EventArgs e)
		{
		    ScriptRecord();
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

	    private void ScriptRecord()
	    {
            if (ScriptRecorder.OnRecord && !m_OnRecord)
            {
                SetErrorBox("RECORDER ERROR: Other Editor are on Record");
                return;
            }

            if (m_Thread == null ||
                    (m_Thread != null && m_Thread.ThreadState != ThreadState.Running &&
                    m_Thread.ThreadState != ThreadState.Unstarted &&
                    m_Thread.ThreadState != ThreadState.WaitSleepJoin)
                )
            {
                if (m_OnRecord)
                {
                    SetErrorBox("RECORDER: Stop Record");
                    m_OnRecord = false;
                    ScriptRecorder.OnRecord = false;
                    SetStatusLabel("");
                    return;
                }
                else
                {
                    SetErrorBox("RECORDER: Start Record");
                    m_OnRecord = true;
                    ScriptRecorder.OnRecord = true;
                    SetStatusLabel("ON RECORD");
                    return;
                }
            }
            else
            {
                SetErrorBox("RECORDER ERROR: Can't Record if script is running");
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

	public class ToolTipDescriptions
    {
        public string Title;
        public string[] Parameters;
	    public string Returns;
	    public string Description;

        public ToolTipDescriptions(string title, string[] parameter, string returns, string description)
        {
            Title = title;
            Parameters = parameter;
            Returns = returns;
            Description = description;
        }

	    public string ToolTipDescription()
	    {
	        string complete_description = "";

	        complete_description += "Parameters: ";

	        foreach (string parameter in Parameters)
	            complete_description += "\n\t" + parameter;

	        complete_description += "\nReturns: " + Returns;

	        complete_description += "\nDescription:";

	        complete_description += "\n\t" + Description;

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