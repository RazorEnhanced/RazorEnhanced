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
		        "Contains", "Weight", "Position"
            };

            string[] props = propsGeneric;

            #endregion

            #region Methods Descriptions

            #region Description Player

            Dictionary<string, ToolTipDescriptions> descriptionPlayer = new Dictionary<string, ToolTipDescriptions>();
            descriptionPlayer.Add("Player.BuffsExist", new ToolTipDescriptions("Player.BuffsExist(string)", "Parameters:\n\tstring BuffName\nReturns: bool"));
            descriptionPlayer.Add("Player.GetBuffDescription", new ToolTipDescriptions("Player.BuffsExist(BuffIcon)", "Parameters:\n\tBuffIcon name\nReturns: string"));
            descriptionPlayer.Add("Player.HeadMessage", new ToolTipDescriptions("Player.HeadMessage(int, string)", "Parameters:\n\tint MessageColor\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.InRangeMobile", new ToolTipDescriptions("Player.InRangeMobile(Mobile or int, int)", "Parameters:\n\tMobile mobiletocheck or int serialmobiletocheck\n\tint range\nReturns: bool"));
            descriptionPlayer.Add("Player.InRangeItem", new ToolTipDescriptions("Player.InRangeItem(Item or int, int)", "Parameters:\n\tItem itemtocheck or int serialitemtocheck\n\tint range\nReturns: bool"));
            descriptionPlayer.Add("Player.GetItemOnLayer", new ToolTipDescriptions("Player.GetItemOnLayer(string)", "Parameters:\n\tstring LayerName\nReturns: Item"));
            descriptionPlayer.Add("Player.UnEquipItemByLayer", new ToolTipDescriptions("Player.UnEquipItemByLayer(string)", "Parameters:\n\tstring LayerName\nReturns: void"));
            descriptionPlayer.Add("Player.EquipItem", new ToolTipDescriptions("Player.EquipItem(Item or int)", "Parameters:\n\tItem item or int serialitem\nReturns: void"));
            descriptionPlayer.Add("Player.CheckLayer", new ToolTipDescriptions("Player.CheckLayer(string)", "Parameters:\n\tstring LayerName\nReturns: bool"));
            descriptionPlayer.Add("Player.GetAssistantLayer", new ToolTipDescriptions("Player.GetAssistantLayer(string)", "Parameters:\n\tstring LayerName\nReturns: Layer"));
            descriptionPlayer.Add("Player.GetSkillValue", new ToolTipDescriptions("Player.GetSkillValue(string)", "Parameters:\n\tstring SkillName\nReturns: double"));
            descriptionPlayer.Add("Player.GetSkillCap", new ToolTipDescriptions("Player.GetSkillCap(string)", "Parameters:\n\tstring SkillName\nReturns: double"));
            descriptionPlayer.Add("Player.GetSkillStatus", new ToolTipDescriptions("Player.GetSkillStatus(string)", "Parameters:\n\tstring SkillName\nReturns: int"));
            descriptionPlayer.Add("Player.UseSkill", new ToolTipDescriptions("Player.UseSkill(string)", "Parameters:\n\tstring SkillName\nReturns: void"));
            descriptionPlayer.Add("Player.ChatSay", new ToolTipDescriptions("Player.ChatSay(int, string)", "Parameters:\n\tint MessageColor\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.ChatEmote", new ToolTipDescriptions("Player.ChatEmote(int, string)", "Parameters:\n\tint MessageColor\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.ChatWhisper", new ToolTipDescriptions("Player.ChatWhisper(int, string)", "Parameters:\n\tint MessageColor\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.ChatYell", new ToolTipDescriptions("Player.ChatYell(int, string)", "Parameters:\n\tint MessageColor\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.ChatGuild", new ToolTipDescriptions("Player.ChatGuild(string)", "Parameters:\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.ChatAlliance", new ToolTipDescriptions("Player.ChatAlliance(string)", "Parameters:\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.SetWarMode", new ToolTipDescriptions("Player.SetWarMode(bool)", "Parameters:\n\tbool warstatus\nReturns: void"));
            descriptionPlayer.Add("Player.Attack", new ToolTipDescriptions("Player.Attack(int)", "Parameters:\n\tint targetserial\nReturns: void"));
            descriptionPlayer.Add("Player.AttackLast", new ToolTipDescriptions("Player.AttackLast()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.InParty", new ToolTipDescriptions("Player.InParty()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionPlayer.Add("Player.ChatParty", new ToolTipDescriptions("Player.ChatParty(string)", "Parameters:\n\tstring Message\nReturns: void"));
            descriptionPlayer.Add("Player.PartyCanLoot", new ToolTipDescriptions("Player.PartyCanLoot(bool)", "Parameters:\n\tbool flag\nReturns: void"));
            descriptionPlayer.Add("Player.PartyInvite", new ToolTipDescriptions("Player.PartyInvite()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.PartyLeave", new ToolTipDescriptions("Player.PartyLeave()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.KickMember", new ToolTipDescriptions("Player.KickMember(int)", "Parameters:\n\tint SerialPersonToKick\nReturns: void"));
            descriptionPlayer.Add("Player.InvokeVirtue", new ToolTipDescriptions("Player.InvokeVirtue(string)", "Parameters:\n\tstring Virtuename\nReturns: void"));
            descriptionPlayer.Add("Player.Walk", new ToolTipDescriptions("Player.Walk(string)", "Parameters:\n\tstring Direction\nReturns: void"));
            descriptionPlayer.Add("Player.PathFindTo", new ToolTipDescriptions("Player.PathFindTo(Point3D or (int, int, int))", "Parameters:\n\tPoint3D coords or\n\tint x, int y, int z\nReturns: void"));
            descriptionPlayer.Add("Player.GetPropValue", new ToolTipDescriptions("Player.GetPropValue(string)", "Parameters:\n\tstring PropName\nReturns: int"));
            descriptionPlayer.Add("Player.GetPropStringByIndex", new ToolTipDescriptions("Player.GetPropStringByIndex(int)", "Parameters:\n\tint PropIndex\nReturns: string"));
            descriptionPlayer.Add("Player.GetPropStringList", new ToolTipDescriptions("Player.GetPropStringList()", "Parameters:\n\tnone\nReturns: List<string>"));
            descriptionPlayer.Add("Player.QuestButton", new ToolTipDescriptions("Player.QuestButton()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.GuildButton", new ToolTipDescriptions("Player.GuildButton()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.WeaponPrimarySA", new ToolTipDescriptions("Player.WeaponPrimarySA()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.WeaponSecondarySA", new ToolTipDescriptions("Player.WeaponSecondarySA()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.WeaponClearSA", new ToolTipDescriptions("Player.WeaponClearSA()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.WeaponStunSA", new ToolTipDescriptions("Player.WeaponStunSA()", "Parameters:\n\tnone\nReturns: void"));
            descriptionPlayer.Add("Player.WeaponDisarmSA", new ToolTipDescriptions("Player.WeaponDisarmSA()", "Parameters:\n\tnone\nReturns: void"));

            #endregion

            #region Description Spells

            Dictionary<string, ToolTipDescriptions> descriptionSpells = new Dictionary<string, ToolTipDescriptions>();
            descriptionSpells.Add("Spells.CastMagery", new ToolTipDescriptions("Spells.CastMagery(string)", "Parameters:\n\tstring Spellname\nReturns: void"));
            descriptionSpells.Add("Spells.CastNecro", new ToolTipDescriptions("Spells.CastNecro(string)", "Parameters:\n\tstring Spellname\nReturns: void"));
            descriptionSpells.Add("Spells.CastChivalry", new ToolTipDescriptions("Spells.CastChivalry(string)", "Parameters:\n\tstring Spellname\nReturns: void"));
            descriptionSpells.Add("Spells.CastBushido", new ToolTipDescriptions("Spells.CastBushido(string)", "Parameters:\n\tstring Spellname\nReturns: void"));
            descriptionSpells.Add("Spells.CastNinjitsu", new ToolTipDescriptions("Spells.CastNinjitsu(string)", "Parameters:\n\tstring Spellname\nReturns: void"));
            descriptionSpells.Add("Spells.CastSpellweaving", new ToolTipDescriptions("Spells.CastSpellweaving(string)", "Parameters:\n\tstring Spellname\nReturns: void"));
            descriptionSpells.Add("Spells.CastMysticism", new ToolTipDescriptions("Spells.CastMysticism(string)", "Parameters:\n\tstring Spellname\nReturns: void"));

            #endregion

            #region Description Mobiles

            Dictionary<string, ToolTipDescriptions> descriptionMobiles = new Dictionary<string, ToolTipDescriptions>();
            descriptionMobiles.Add("Mobiles.FindBySerial", new ToolTipDescriptions("Mobiles.FindBySerial(int)", "Parameters:\n\tint Mobileserial\nReturns: Mobile"));
            descriptionMobiles.Add("Mobiles.UseMobile", new ToolTipDescriptions("Mobiles.UseMobile(Mobile or int)", "Parameters:\n\tMobile mobile or int serialmobile\nReturns: void"));
            descriptionMobiles.Add("Mobiles.SingleClick", new ToolTipDescriptions("Mobiles.SingleClick(Mobile or int)", "Parameters:\n\tMobile mobile or int serialmobile\nReturns: void"));
            descriptionMobiles.Add("Mobiles.Filter", new ToolTipDescriptions("Mobiles.Filter()", "Parameters:\n\tnone\nReturns: Filter"));
            descriptionMobiles.Add("Mobiles.ApplyFilter", new ToolTipDescriptions("Mobiles.ApplyFilter(filter)", "Parameters:\n\tFilter filtermobile\nReturns: List<Mobiles>"));
            descriptionMobiles.Add("Mobiles.Message", new ToolTipDescriptions("Mobiles.Message(Mobile or int, int string)", "Parameters:\n\tMobile mobile or int serialmobile\n\tint Colormessage\n\tstring Message\nReturns: void"));
            descriptionMobiles.Add("Mobiles.WaitForProps", new ToolTipDescriptions("Mobiles.WaitForProps(Mobile or int, int)", "Parameters:\n\tMobile mobile or int serialmobile\n\tint timeoutProps\nReturns: void"));
            descriptionMobiles.Add("Mobiles.GetPropValue", new ToolTipDescriptions("Mobiles.GetPropValue(Mobile or int, string)", "Parameters:\n\tMobile mobile or int serialmobile\n\tstring PropName\nReturns: int"));
            descriptionMobiles.Add("Mobiles.GetPropStringByIndex", new ToolTipDescriptions("Mobiles.GetPropStringByIndex(Mobile or int, int)", "Parameters:\n\tMobile mobile or int serialmobile\n\tint PropIndex\nReturns: string"));
            descriptionMobiles.Add("Mobiles.GetPropStringList", new ToolTipDescriptions("Mobiles.GetPropStringList(Mobile or int)", "Parameters:\n\tMobile mobile or int serialmobile\nReturns: List<string>"));

            #endregion

            #region Description Items

            Dictionary<string, ToolTipDescriptions> descriptionItems = new Dictionary<string, ToolTipDescriptions>();
            descriptionItems.Add("Items.FindBySerial", new ToolTipDescriptions("Items.FindBySerial(int)", "Parameters:\n\tint ItemSerial\nReturns: Item"));
            descriptionItems.Add("Items.Move", new ToolTipDescriptions("Items.Move(Item or int, Item or Mobile or int, int)", "Parameters:\n\tItem source or int sourceItem\n\tItem destinationItem or Mobile destinationMobile or int destinationSerial\n\tint amountToMove\nReturns: void"));
            descriptionItems.Add("Items.DropItemOnGroundSelf", new ToolTipDescriptions("Items.DropItemOnGroundSelf(Item, int)", "Parameters:\n\tItem item\n\tint amount\nReturns: void"));
            descriptionItems.Add("Items.UseItem", new ToolTipDescriptions("Items.UseItem(Item or int)", "Parameters:\n\tItem item or int ItemSerial\nReturns: void"));
            descriptionItems.Add("Items.SingleClick", new ToolTipDescriptions("Items.SingleClick(Item or int)", "Parameters:\n\tItem item or int ItemSerial\nReturns: void"));
            descriptionItems.Add("Items.WaitForProps", new ToolTipDescriptions("Items.WaitForProps(Item or int, int)", "Parameters:\n\tItem item or int serialitem\n\tint timeoutProps\nReturns: void"));
            descriptionItems.Add("Items.GetPropValue", new ToolTipDescriptions("Items.GetPropValue(Item or int, string)", "Parameters:\n\tItem item or int serialitem\n\tstring PropName\nReturns: int"));
            descriptionItems.Add("Items.GetPropStringByIndex", new ToolTipDescriptions("Items.GetPropStringByIndex(Item or int, int)", "Parameters:\n\tItem item or int serialitem\n\tint PropIndex\nReturns: string"));
            descriptionItems.Add("Items.GetPropStringList", new ToolTipDescriptions("Items.GetPropStringList(Item or int)", "Parameters:\n\tItem item or int serialitem\nReturns: List<string>"));
            descriptionItems.Add("Items.WaitForContents", new ToolTipDescriptions("Items.WaitForContents(Item or int, int)", "Parameters:\n\tItem itemtouse or int serialitemtouse\n\tint timeoutContentWait\nReturns: void"));
            descriptionItems.Add("Items.Message", new ToolTipDescriptions("Items.Message(Item or int, int, string)", "Parameters:\n\tItem item or int serialitem\n\tint messageColor\n\tstring message\nReturns: void"));
            descriptionItems.Add("Items.Filter", new ToolTipDescriptions("Items.Filter()", "Parameters:\n\tnone\nReturns: Filter"));
            descriptionItems.Add("Items.ApplyFilter", new ToolTipDescriptions("Items.ApplyFilter(Filter)", "Parameters:\n\tFilter filterItem\nReturns: List<Item>"));
            descriptionItems.Add("Items.BackpackCount", new ToolTipDescriptions("Items.BackpackCount(int, int)", "Parameters:\n\tint itemID\n\tint color\nReturns: int"));
            descriptionItems.Add("Items.ContainerCount", new ToolTipDescriptions("Items.ContainerCount(item or int, int, int)", "Parameters:\n\tItem container or int serialContainer\n\tint itemID\n\tint color\nReturns: List<Item>"));

            #endregion

            #region Description Misc

            Dictionary<string, ToolTipDescriptions> descriptionMisc = new Dictionary<string, ToolTipDescriptions>();
            descriptionMisc.Add("Misc.SendMessage", new ToolTipDescriptions("Misc.SendMessage(string or int or bool, color)", "Parameters:\n\tstring Message or int Value or bool Status\n\tint color (optional)\nReturns: void"));
            descriptionMisc.Add("Misc.Resync", new ToolTipDescriptions("Misc.Resync()", "Parameters:\n\tnone\nReturns: void"));
            descriptionMisc.Add("Misc.Pause", new ToolTipDescriptions("Misc.Pause(int)", "Parameters:\n\tint pauseTime\nReturns: void"));
            descriptionMisc.Add("Misc.Beep", new ToolTipDescriptions("Misc.Beep()", "Parameters:\n\tnone\nReturns: void"));
            descriptionMisc.Add("Misc.Disconnect", new ToolTipDescriptions("Misc.Disconnect()", "Parameters:\n\tnone\nReturns: void"));
            descriptionMisc.Add("Misc.WaitForContext", new ToolTipDescriptions("Misc.WaitForContext(int or Mobile or Item, int)", "Parameters:\n\tint serial or Mobile mobile or Item item\n\tint timeout\nReturns: void"));
            descriptionMisc.Add("Misc.ContextReply", new ToolTipDescriptions("Misc.ContextReply(int or Mobile or Item, int)", "Parameters:\n\tint serial or Mobile mobile or Item item\n\tint menuID\nReturns: void"));
            descriptionMisc.Add("Misc.ReadSharedValue", new ToolTipDescriptions("Misc.ReadSharedValue(string)", "Parameters:\n\tstring nameOfValue\nReturns: object"));
            descriptionMisc.Add("Misc.RemoveSharedValue", new ToolTipDescriptions("Misc.RemoveSharedValue(string)", "Parameters:\n\tstring nameOfValue\nReturns: void"));
            descriptionMisc.Add("Misc.CheckSharedValue", new ToolTipDescriptions("Misc.CheckSharedValue(string)", "Parameters:\n\tstring nameOfValue\nReturns: bool"));
            descriptionMisc.Add("Misc.SetSharedValue", new ToolTipDescriptions("Misc.SetSharedValue(string, object)", "Parameters:\n\tstring nameOfValue\n\tobject valueToSet\nReturns: void"));
            descriptionMisc.Add("Misc.HasMenu", new ToolTipDescriptions("Misc.HasMenu()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionMisc.Add("Misc.CloseMenu", new ToolTipDescriptions("Misc.CloseMenu()", "Parameters:\n\tnone\nReturns: void"));
            descriptionMisc.Add("Misc.MenuContains", new ToolTipDescriptions("Misc.MenuContains(string)", "Parameters:\n\tstring texttosearch\nReturns: bool"));
            descriptionMisc.Add("Misc.GetMenuTitle", new ToolTipDescriptions("Misc.GetMenuTitle()", "Parameters:\n\tnone\nReturns: string"));
            descriptionMisc.Add("Misc.WaitForMenu", new ToolTipDescriptions("Misc.WaitForMenu(int)", "Parameters:\n\tint timeout\nReturns: void"));
            descriptionMisc.Add("Misc.MenuResponse", new ToolTipDescriptions("Misc.MenuResponse(string)", "Parameters:\n\tstring submitname\nReturns: void"));
            descriptionMisc.Add("Misc.HasQueryString", new ToolTipDescriptions("Misc.HasQueryString()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionMisc.Add("Misc.WaitForQueryString", new ToolTipDescriptions("Misc.WaitForQueryString(int)", "Parameters:\n\tint timeout\nReturns: void"));
            descriptionMisc.Add("Misc.QueryStringResponse", new ToolTipDescriptions("Misc.QueryStringResponse(string)", "Parameters:\n\tbool yescancelstatus\n\tstring stringtoresponse\nReturns: void"));
            descriptionMisc.Add("Misc.NoOperation", new ToolTipDescriptions("Misc.NoOperation()", "Parameters:\n\tnone\nReturns: void"));
            descriptionMisc.Add("Misc.ScriptRun", new ToolTipDescriptions("Misc.ScriptRun(string)", "Parameters:\n\tstring scriptfilename\nReturns: void"));
            descriptionMisc.Add("Misc.ScriptStop", new ToolTipDescriptions("Misc.ScriptStop(string)", "Parameters:\n\tstring scriptfilename\nReturns: void"));
            descriptionMisc.Add("Misc.ScriptStatus", new ToolTipDescriptions("Misc.ScriptStatus(string)", "Parameters:\n\tstring scriptfilename\nReturns: bool"));
            descriptionMisc.Add("Misc.PetRename", new ToolTipDescriptions("Misc.PetRename(Mobile or int, string)", "Parameters:\n\tMobile mobile or int serialmobile\n\tstring newname\nReturns: void"));

            #endregion

            #region Description Target

            Dictionary<string, ToolTipDescriptions> descriptionTarget = new Dictionary<string, ToolTipDescriptions>();
            descriptionTarget.Add("Target.HasTarget", new ToolTipDescriptions("Target.HasTarget()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionTarget.Add("Target.GetLast", new ToolTipDescriptions("Target.GetLast()", "Parameters:\n\tnone\nReturns: int"));
            descriptionTarget.Add("Target.GetLastAttack", new ToolTipDescriptions("Target.GetLastAttack()", "Parameters:\n\tnone\nReturns: int"));
            descriptionTarget.Add("Target.WaitForTarget", new ToolTipDescriptions("Target.WaitForTarget(int)", "Parameters:\n\tint timeoutDelay\nReturns: int"));
            descriptionTarget.Add("Target.TargetExecute", new ToolTipDescriptions("Target.TargetExecute(int or Item or Mobile or (int, int, int))", "Parameters:\n\tint serial or Item item or Mobile mobile\n\tor int x, int y, int z\nReturns: void"));
            descriptionTarget.Add("Target.PromptTarget", new ToolTipDescriptions("Target.PromptTarget()", "Parameters:\n\tnone\nReturns: int"));
            descriptionTarget.Add("Target.Cancel", new ToolTipDescriptions("Target.Cancel()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.Last", new ToolTipDescriptions("Target.Last()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.LastQueued", new ToolTipDescriptions("Target.LastQueued()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.Self", new ToolTipDescriptions("Target.Self()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.SelfQueued", new ToolTipDescriptions("Target.SelfQueued()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.SetLast", new ToolTipDescriptions("Target.SetLast(Mobile or int)", "Parameters:\n\tMobile mobileTarget or int serialTarget\nReturns: void"));
            descriptionTarget.Add("Target.ClearLast", new ToolTipDescriptions("Target.ClearLast()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.ClearQueue", new ToolTipDescriptions("Target.ClearQueue()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.ClearLastandQueue", new ToolTipDescriptions("Target.ClearLastandQueue()", "Parameters:\n\tnone\nReturns: void"));
            descriptionTarget.Add("Target.SetLastTargetFromList", new ToolTipDescriptions("Target.SetLastTargetFromList(string)", "Parameters:\n\tstring targetFilterName\nReturns: bool"));
            descriptionTarget.Add("Target.PerformTargetFromList", new ToolTipDescriptions("Target.PerformTargetFromList(string)", "Parameters:\n\tstring targetFilterName\nReturns: bool"));
            descriptionTarget.Add("Target.AttackTargetFromList", new ToolTipDescriptions("Target.AttackTargetFromList(string)", "Parameters:\n\tstring targetFilterName\nReturns: bool"));

            #endregion

            #region Description Gumps

            Dictionary<string, ToolTipDescriptions> descriptionGumps = new Dictionary<string, ToolTipDescriptions>();
            descriptionGumps.Add("Gumps.CurrentGump", new ToolTipDescriptions("Gumps.CurrentGump()", "Parameters:\n\tnone\nReturns: uint"));
            descriptionGumps.Add("Gumps.HasGump", new ToolTipDescriptions("Gumps.HasGump()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionGumps.Add("Gumps.CloseGump", new ToolTipDescriptions("Gumps.CloseGump(uint)", "Parameters:\n\tuint gumpID\nReturns: void"));
            descriptionGumps.Add("Gumps.WaitForGump", new ToolTipDescriptions("Gumps.WaitForGump(uint, int)", "Parameters:\n\tuint gumpID\n\tint timeout\nReturns: void"));
            descriptionGumps.Add("Gumps.SendAction", new ToolTipDescriptions("Gumps.SendAction(uint, int)", "Parameters:\n\tuint gumpID\n\tint buttonID\nReturns: void"));
            descriptionGumps.Add("Gumps.SendAdvancedAction", new ToolTipDescriptions("Gumps.SendAdvancedAction(uint, int, list<int>, (optional)list<int>, (optional)list<string>)", "Parameters:\n\tuint gumpID\n\tint buttonID\n\tlist<int> switches\n\tlist<int> textID\n\tlist<string> texts\nReturns: void"));
            descriptionGumps.Add("Gumps.LastGumpGetLine", new ToolTipDescriptions("Gumps.LastGumpGetLine(int)", "Parameters:\n\tint lineNumber\nReturns: string"));
            descriptionGumps.Add("Gumps.LastGumpGetLineList", new ToolTipDescriptions("Gumps.LastGumpGetLineList()", "Parameters:\n\tnone\nReturns: list<string>"));
            descriptionGumps.Add("Gumps.LastGumpTextExist", new ToolTipDescriptions("Gumps.LastGumpTextExist(string)", "Parameters:\n\tstring texttosearch\nReturns: bool"));
            descriptionGumps.Add("Gumps.LastGumpTextExistByLine", new ToolTipDescriptions("Gumps.LastGumpTextExistByLine(int, string)", "Parameters:\n\tint lineNumber\n\ttstring texttosearch\nReturns: bool"));

            #endregion

            #region Description Journal

            Dictionary<string, ToolTipDescriptions> descriptionJournal = new Dictionary<string, ToolTipDescriptions>();
            descriptionJournal.Add("Journal.Clear", new ToolTipDescriptions("Journal.Clear()", "Parameters:\n\tnone\nReturns: void"));
            descriptionJournal.Add("Journal.Search", new ToolTipDescriptions("Journal.Search(string)", "Parameters:\n\tstring TextToSearch\nReturns: bool"));
            descriptionJournal.Add("Journal.SearchByName", new ToolTipDescriptions("Journal.SearchByName(string, string)", "Parameters:\n\tstring TextToSearch\n\tstring senderName\nReturns: bool"));
            descriptionJournal.Add("Journal.SearchByColor", new ToolTipDescriptions("Journal.SearchByColor(string, int)", "Parameters:\n\tstring TextToSearch\n\tstring colorToSearch\nReturns: bool"));
            descriptionJournal.Add("Journal.SearchByType", new ToolTipDescriptions("Journal.SearchByType(string, string)", "Parameters:\n\tstring TextToSearch\n\tstring messageType\nReturns: bool"));
            descriptionJournal.Add("Journal.GetLineText", new ToolTipDescriptions("Journal.GetLineText(string)", "Parameters:\n\tstring TextToSearch\nReturns: string"));
            descriptionJournal.Add("Journal.GetSpeechName", new ToolTipDescriptions("Journal.GetSpeechName()", "Parameters:\n\tnone\nReturns: list<string>"));
            descriptionJournal.Add("Journal.WaitJournal", new ToolTipDescriptions("Journal.WaitJournal(string, int)", "Parameters:\n\tstring TextToSearch\n\tint timeoutTime\nReturns: void"));

            #endregion

            #region Description AutoLoot

            Dictionary<string, ToolTipDescriptions> descriptionAutoLoot = new Dictionary<string, ToolTipDescriptions>();
            descriptionAutoLoot.Add("AutoLoot.Status", new ToolTipDescriptions("AutoLoot.Status()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionAutoLoot.Add("AutoLoot.Start", new ToolTipDescriptions("AutoLoot.Start()", "Parameters:\n\tnone\nReturns: void"));
            descriptionAutoLoot.Add("AutoLoot.Stop", new ToolTipDescriptions("AutoLoot.Stop()", "Parameters:\n\tnone\nReturns: void"));
            descriptionAutoLoot.Add("AutoLoot.ChangeList", new ToolTipDescriptions("AutoLoot.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));
            descriptionAutoLoot.Add("AutoLoot.RunOnce", new ToolTipDescriptions("AutoLoot.RunOnce(AutoLootItem, double, Filter)", "Parameters:\n\tAutoLootItem itemList\n\tdouble DelayGrabInMs\n\tFilter FilterToSearch\nReturns: void"));

            #endregion

            #region Description Scavenger

            Dictionary<string, ToolTipDescriptions> descriptionScavenger = new Dictionary<string, ToolTipDescriptions>();
            descriptionScavenger.Add("Scavenger.Status", new ToolTipDescriptions("Scavenger.Status()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionScavenger.Add("Scavenger.Start", new ToolTipDescriptions("Scavenger.Start()", "Parameters:\n\tnone\nReturns: void"));
            descriptionScavenger.Add("Scavenger.Stop", new ToolTipDescriptions("Scavenger.Stop()", "Parameters:\n\tnone\nReturns: void"));
            descriptionScavenger.Add("Scavenger.ChangeList", new ToolTipDescriptions("Scavenger.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));
            descriptionScavenger.Add("Scavenger.RunOnce", new ToolTipDescriptions("Scavenger.RunOnce(ScavengerItem, double, Filter)", "Parameters:\n\tScavengerItem itemList\n\tdouble DelayGrabInMs\n\tFilter FilterToSearch\nReturns: void"));

            #endregion

            #region Description Restock

            Dictionary<string, ToolTipDescriptions> descriptionRestock = new Dictionary<string, ToolTipDescriptions>();
            descriptionRestock.Add("Restock.Status", new ToolTipDescriptions("Restock.Status()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionRestock.Add("Restock.FStart", new ToolTipDescriptions("Restock.FStart()", "Parameters:\n\tnone\nReturns: void"));
            descriptionRestock.Add("Restock.FStop", new ToolTipDescriptions("Restock.FStop()", "Parameters:\n\tnone\nReturns: void"));
            descriptionRestock.Add("Restock.ChangeList", new ToolTipDescriptions("Restock.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));

            #endregion

            #region Description SellAgent

            Dictionary<string, ToolTipDescriptions> descriptionSellAgent = new Dictionary<string, ToolTipDescriptions>();
            descriptionSellAgent.Add("SellAgent.Status", new ToolTipDescriptions("SellAgent.Status()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionSellAgent.Add("SellAgent.Enable", new ToolTipDescriptions("SellAgent.Enable()", "Parameters:\n\tnone\nReturns: void"));
            descriptionSellAgent.Add("SellAgent.Disable", new ToolTipDescriptions("SellAgent.Disable()", "Parameters:\n\tnone\nReturns: void"));
            descriptionSellAgent.Add("SellAgent.ChangeList", new ToolTipDescriptions("SellAgent.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));

            #endregion

            #region Description BuyAgent

            Dictionary<string, ToolTipDescriptions> descriptionBuyAgent = new Dictionary<string, ToolTipDescriptions>();
            descriptionBuyAgent.Add("BuyAgent.Status", new ToolTipDescriptions("BuyAgent.Status()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionBuyAgent.Add("BuyAgent.Enable", new ToolTipDescriptions("BuyAgent.Enable()", "Parameters:\n\tnone\nReturns: void"));
            descriptionBuyAgent.Add("BuyAgent.Disable", new ToolTipDescriptions("BuyAgent.Disable()", "Parameters:\n\tnone\nReturns: void"));
            descriptionBuyAgent.Add("BuyAgent.ChangeList", new ToolTipDescriptions("BuyAgent.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));

            #endregion

            #region Description Dress

            Dictionary<string, ToolTipDescriptions> descriptionDress = new Dictionary<string, ToolTipDescriptions>();
            descriptionDress.Add("Dress.DessStatus", new ToolTipDescriptions("Dress.DessStatus()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionDress.Add("Dress.UnDressStatus", new ToolTipDescriptions("Dress.UnDressStatus()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionDress.Add("Dress.DressFStart", new ToolTipDescriptions("Dress.DressFStart()", "Parameters:\n\tnone\nReturns: void"));
            descriptionDress.Add("Dress.UnDressFStart", new ToolTipDescriptions("Dress.UnDressFStart()", "Parameters:\n\tnone\nReturns: void"));
            descriptionDress.Add("Dress.DressFStop", new ToolTipDescriptions("Dress.DressFStop()", "Parameters:\n\tnone\nReturns: void"));
            descriptionDress.Add("Dress.UnDressFStop", new ToolTipDescriptions("Dress.UnDressFStop()", "Parameters:\n\tnone\nReturns: void"));
            descriptionDress.Add("Dress.ChangeList", new ToolTipDescriptions("Dress.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));

            #endregion

            #region Description Friend

            Dictionary<string, ToolTipDescriptions> descriptionFriend = new Dictionary<string, ToolTipDescriptions>();
            descriptionDress.Add("Friend.IsFriend", new ToolTipDescriptions("Friend.IsFriend(int)", "Parameters:\n\tint SerialToSearch\nReturns: bool"));
            descriptionFriend.Add("Friend.ChangeList", new ToolTipDescriptions("Friend.ChangeList(string)", "Parameters:\n\tstring listName\nReturns: void"));

            #endregion

            #region Description BandageHeal

            Dictionary<string, ToolTipDescriptions> descriptionBandageHeal = new Dictionary<string, ToolTipDescriptions>();
            descriptionBandageHeal.Add("BandageHeal.Status", new ToolTipDescriptions("BandageHeal.Status()", "Parameters:\n\tnone\nReturns: bool"));
            descriptionBandageHeal.Add("BandageHeal.Start", new ToolTipDescriptions("BandageHeal.Start()", "Parameters:\n\tnone\nReturns: void"));
            descriptionBandageHeal.Add("BandageHeal.Stop", new ToolTipDescriptions("BandageHeal.Stop()", "Parameters:\n\tnone\nReturns: void"));

            #endregion

            #region Description Statics

            Dictionary<string, ToolTipDescriptions> descriptionStatics = new Dictionary<string, ToolTipDescriptions>();
            descriptionStatics.Add("Statics.GetLandID", new ToolTipDescriptions("Statics.GetLandID()", "Parameters:\n\t\nReturns: "));
            descriptionStatics.Add("Statics.GetLandZ", new ToolTipDescriptions("Statics.GetLandZ()", "Parameters:\n\t\nReturns: "));
            descriptionStatics.Add("Statics.GetStaticsTileInfo", new ToolTipDescriptions("Statics.GetStaticsTileInfo()", "Parameters:\n\t\nReturns: "));

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
                items.Add(new AutocompleteItem(item) { ImageIndex = 0 });

            //Permette la creazione del menu con la singola classe
            Array.Sort(classes);
            foreach (var item in classes)
		        items.Add(new AutocompleteItem(item) { ImageIndex = 1 });

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
		                ToolTipText = element.Description
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

		private void toolStripRecord_Click(object sender, EventArgs e)
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

		private void toolStripInspectGump_Click(object sender, EventArgs e)
		{
			InspectGumps();
		}
	}

	public class ToolTipDescriptions
    {
        public string Title;
        public string Description;

        public ToolTipDescriptions(string title, string description)
        {
            Title = title;
            Description = description;
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