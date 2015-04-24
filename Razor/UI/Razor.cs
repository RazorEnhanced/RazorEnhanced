using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Assistant.Filters;
using Assistant.Macros;
using RazorEnhanced;
using RazorEnhanced.UI;

namespace Assistant
{
	internal class MainForm : System.Windows.Forms.Form
	{
		#region Class Variables

		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckedListBox filters;
		private System.Windows.Forms.ColumnHeader skillHDRName;
		private System.Windows.Forms.ColumnHeader skillHDRvalue;
		private System.Windows.Forms.ColumnHeader skillHDRbase;
		private System.Windows.Forms.ColumnHeader skillHDRdelta;
		private RazorButton resetDelta;
		private RazorButton setlocks;
		private RazorComboBox locks;
		private System.Windows.Forms.ListView skillList;
        private System.Windows.Forms.ColumnHeader skillHDRcap;
        private RazorCheckBox alwaysTop;
		private System.Windows.Forms.GroupBox groupBox4;
		private RazorButton newProfile;
		private RazorButton delProfile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox baseTotal;
		private System.Windows.Forms.TabPage emptyTab;
		private RazorButton skillCopySel;
        private RazorButton skillCopyAll;
		private System.Windows.Forms.TabPage generalTab;
		private System.Windows.Forms.TabPage toolbarTab;
		private System.Windows.Forms.TabPage skillsTab;
		private System.Windows.Forms.TabPage hotkeysTab;
		private RazorCheckBox chkCtrl;
		private RazorCheckBox chkAlt;
		private RazorCheckBox chkShift;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.TextBox key;
		private RazorButton setHK;
		private RazorButton unsetHK;
		private System.Windows.Forms.Label label2;
		private RazorCheckBox chkPass;
		private System.Windows.Forms.TabPage moreOptTab;
		private RazorCheckBox chkForceSpeechHue;
		private System.Windows.Forms.Label label3;
		private RazorTextBox txtSpellFormat;
		private RazorCheckBox chkForceSpellHue;
		private RazorCheckBox chkStealth;
        private System.Windows.Forms.TabPage mapsTab;
        private RazorButton dohotkey;
		private System.Windows.Forms.Label opacityLabel;
		private System.Windows.Forms.TrackBar opacity;
        private RazorCheckBox dispDelta;
		private RazorCheckBox openCorpses;
		private RazorTextBox corpseRange;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabPage macrosTab;
		private System.Windows.Forms.TreeView hotkeyTree;
		private System.Windows.Forms.TabPage screenshotTab;
		private System.Windows.Forms.TabPage statusTab;
		private RazorButton newMacro;
		private RazorButton delMacro;
		private System.Windows.Forms.GroupBox macroActGroup;
		private System.Windows.Forms.ListBox actionList;
		private RazorButton playMacro;
		private RazorButton recMacro;
        private RazorCheckBox loopMacro;
		private RazorCheckBox spamFilter;
		private System.Windows.Forms.PictureBox screenPrev;
		private System.Windows.Forms.ListBox screensList;
		private RazorButton setScnPath;
		private RazorRadioButton radioFull;
		private RazorRadioButton radioUO;
		private RazorCheckBox screenAutoCap;
		private RazorTextBox screenPath;
		private RazorButton capNow;
        private RazorCheckBox dispTime;
        private RazorCheckBox showWelcome;
		private System.Windows.Forms.ColumnHeader skillHDRlock;
		private System.ComponentModel.IContainer components;
		private RazorCheckBox queueTargets;
		private RazorRadioButton systray;
		private RazorRadioButton taskbar;
		private System.Windows.Forms.Label label11;
        private RazorCheckBox autoStackRes;
		private RazorButton setExHue;
		private RazorButton setMsgHue;
		private RazorButton setWarnHue;
		private RazorButton setSpeechHue;
		private RazorButton setBeneHue;
		private RazorButton setHarmHue;
		private RazorButton setNeuHue;
		private System.Windows.Forms.Label lblWarnHue;
		private System.Windows.Forms.Label lblMsgHue;
		private System.Windows.Forms.Label lblExHue;
		private System.Windows.Forms.Label lblBeneHue;
		private System.Windows.Forms.Label lblHarmHue;
		private System.Windows.Forms.Label lblNeuHue;
		private RazorCheckBox incomingCorpse;
        private RazorCheckBox incomingMob;
		private RazorComboBox profiles;
        private System.Windows.Forms.Label hkStatus;
		private System.Windows.Forms.TabPage moreMoreOptTab;
		private RazorCheckBox actionStatusMsg;
		private RazorTextBox txtObjDelay;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private RazorCheckBox QueueActions;
		private RazorCheckBox rangeCheckLT;
		private RazorTextBox ltRange;
        private System.Windows.Forms.Label label8;
		private RazorCheckBox logPackets;
		private RazorCheckBox filterSnoop;
		private RazorCheckBox smartLT;
		private RazorCheckBox showtargtext;
		private RazorCheckBox smartCPU;
		private System.Windows.Forms.Label waitDisp;
		private RazorButton setLTHilight;
		private RazorCheckBox lthilight;
		private RazorCheckBox rememberPwds;
		private RazorCheckBox blockDis;
		private System.Windows.Forms.Label label12;
        private RazorComboBox imgFmt;
		private System.Windows.Forms.TreeView macroTree;
		private ToolTip m_Tip;
		#endregion

		private int m_LastKV = 0;
		private bool m_ProfileConfirmLoad;
		private RazorCheckBox spellUnequip;
		private RazorCheckBox autoFriend;
		private RazorCheckBox alwaysStealth;
		private RazorCheckBox autoOpenDoors;
		private System.Windows.Forms.Label label17;
		private RazorComboBox msglvl;
		private RazorTextBox forceSizeX;
		private RazorTextBox forceSizeY;
		private System.Windows.Forms.Label label18;
        private RazorCheckBox gameSize;
        private RazorCheckBox potionEquip;
		private RazorCheckBox blockHealPoison;
		private RazorCheckBox negotiate;
        private System.Windows.Forms.PictureBox lockBox;
		private RazorCheckBox preAOSstatbar;
		private RazorCheckBox showHealthOH;
		private System.Windows.Forms.Label label10;
		private RazorTextBox healthFmt;
		private RazorComboBox clientPrio;
		private System.Windows.Forms.Label label9;
        private RazorCheckBox chkPartyOverhead;
		private RazorButton macroImport;
		private RazorButton exportMacro;
		private TabPage scriptingTab;
		private RazorButton xButton2;
		private RazorButton xButton3;
		private DataGridView dataGridViewScripting;
		private RazorButton razorButtonUp;
		private RazorButton razorButtonDown;
		internal RazorCheckBox razorCheckBoxAuto;
		private RazorButton razorButtonEdit;
		private Label labelFeatures;
		private Label labelStatus;
		private Panel panelUODlogo;
		private RazorButton razorButtonVisitUOD;
		private Label labelUOD;
		private RazorButton razorButtonCreateUODAccount;
		private RazorButton razorButtonWiki;
		private Panel panelLogo;
		private List<RazorEnhanced.Organizer.OrganizerItem> organizerItemList = new List<RazorEnhanced.Organizer.OrganizerItem>();
		private List<RazorEnhanced.SellAgent.SellAgentItem> sellItemList = new List<RazorEnhanced.SellAgent.SellAgentItem>();
		private List<RazorEnhanced.BuyAgent.BuyAgentItem> buyItemList = new List<RazorEnhanced.BuyAgent.BuyAgentItem>();
		private List<RazorEnhanced.Dress.DressItem> dressItemList = new List<RazorEnhanced.Dress.DressItem>();
		private TabPage EnhancedAgent;
		private TabControl tabControl1;
		private TabPage eautoloot;
		private GroupBox groupBox13;
		private ListBox autolootLogBox;
		private Label autolootContainerLabel;
		private RazorButton autoLootButtonListImport;
		private RazorButton autoLootButtonListExport;
		private GroupBox groupBox11;
		private RazorButton autolootItemPropsB;
		private RazorButton autolootItemEditB;
		private RazorButton autolootAddItemBTarget;
		private RazorButton autolootRemoveItemB;
		private RazorButton autolootAddItemBManual;
		private RazorButton autolootContainerButton;
		private RazorCheckBox autoLootCheckBox;
		private ListView autolootlistView;
		private ColumnHeader columnHeader4;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader ColumnHeader3;
		private TabPage escavenger;
		private Label label21;
		private RazorTextBox autoLootTextBoxDelay;
		private RazorButton autoLootButtonRemoveList;
		private RazorButton autolootButtonAddList;
		private RazorComboBox autolootListSelect;
		private Label label20;
		private RazorButton razorButtonResetIgnore;
		private RazorButton scavengerButtonRemoveList;
		private RazorButton scavengerButtonAddList;
		private RazorButton scavengerButtonImport;
		private RazorButton scavengerButtonExport;
		private RazorComboBox scavengerListSelect;
		private Label label22;
		private GroupBox groupBox12;
		private ListBox scavengerLogBox;
		private Label label23;
		private RazorTextBox scavengerDragDelay;
		private Label scavengerContainerLabel;
		private RazorButton scavengerButtonSetContainer;
		private RazorCheckBox scavengerCheckBox;
		private ListView scavengerListView;
		private ColumnHeader columnHeader5;
		private ColumnHeader columnHeader6;
		private ColumnHeader columnHeader7;
		private ColumnHeader columnHeader8;
		private GroupBox groupBox14;
		private RazorButton scavengerButtonEditProps;
		private RazorButton scavengerButtonEditItem;
		private RazorButton scavengerButtonAddTarget;
		private RazorButton scavengerButtonRemoveItem;
		private RazorButton scavengerButtonAddManual;
		private TabPage Organizer;
		private RazorButton organizerStopButton;
		private RazorButton organizerExecuteButton;
		private GroupBox groupBox16;
		private ListBox organizerLogBox;
		private Label label27;
		private RazorTextBox organizerDragDelay;
		private Label organizerDestinationLabel;
		private RazorButton organizerSetDestinationB;
		private Label organizerSourceLabel;
		private GroupBox groupBox15;
		private RazorButton organizerEditB;
		private RazorButton organizerAddTargetB;
		private RazorButton organizerRemoveB;
		private RazorButton organizerAddManualB;
		private RazorButton organizerSetSourceB;
		private ListView organizerListView;
		private ColumnHeader columnHeader9;
		private ColumnHeader columnHeader10;
		private ColumnHeader columnHeader11;
		private ColumnHeader columnHeader12;
		private ColumnHeader columnHeader13;
		private RazorButton organizerRemoveListB;
		private RazorButton organizerAddListB;
		private RazorButton organizerImportListB;
		private RazorComboBox organizerListSelect;
		private RazorButton organizerExportListB;
		private Label label24;
		private TabPage VendorBuy;
		private TabPage VendorSell;
		private GroupBox groupBox17;
		private RazorButton buyEditB;
		private RazorButton buyAddTargetB;
		private RazorButton buyRemoveB;
		private RazorButton buyAddManualB;
		private GroupBox groupBox18;
		private ListBox buyLogBox;
		private RazorCheckBox buyEnableCheckBox;
		private ListView buyListView;
		private ColumnHeader columnHeader14;
		private ColumnHeader columnHeader15;
		private ColumnHeader columnHeader16;
		private ColumnHeader columnHeader17;
		private RazorButton buyRemoveListButton;
		private RazorButton buyAddListButton;
		private RazorButton buyImportListButton;
		private RazorComboBox buyListSelect;
		private RazorButton buyExportListButton;
		private Label label25;
		private GroupBox groupBox19;
		private RazorButton sellEditButton;
		private RazorButton sellAddTargerButton;
		private RazorButton sellRemoveButton;
		private RazorButton sellAddManualButton;
		private GroupBox groupBox20;
		private ListBox sellLogBox;
		private RazorCheckBox sellEnableCheckBox;
		private ListView sellListView;
		private ColumnHeader columnHeader18;
		private ColumnHeader columnHeader19;
		private ColumnHeader columnHeader20;
		private ColumnHeader columnHeader21;
		private RazorButton sellRemoveListButton;
		private RazorButton sellAddListButton;
		private RazorButton sellImportListButton;
		private RazorComboBox sellListSelect;
		private RazorButton sellExportListButton;
		private Label label26;
		private RazorButton razorButton1;
		private Label sellBagLabel;
		private RazorButton sellSetBagButton;
		private ColumnHeader columnHeader22;
		private ColumnHeader columnHeader23;
		private TabPage Dress;
		private RazorCheckBox dressConflictCheckB;
		private Label dressBagLabel;
		private RazorButton dressSetBagB;
		private RazorButton undressExecuteButton;
		private RazorButton dressExecuteButton;
		private GroupBox groupBox22;
		private RazorButton dressAddTargetB;
		private RazorButton dressAddManualB;
		private RazorButton dressRemoveB;
		private RazorButton dressReadB;
		private Label label29;
		private RazorTextBox dressDragDelay;
		private GroupBox groupBox21;
		private ListBox dressLogBox;
		private ListView dressListView;
		private ColumnHeader columnHeader24;
		private ColumnHeader columnHeader25;
		private ColumnHeader columnHeader26;
		private ColumnHeader columnHeader27;
		private RazorButton dressRemoveListB;
		private RazorButton dressAddListB;
		private RazorButton dressImportListB;
		private RazorComboBox dressListSelect;
		private RazorButton dressExportListB;
		private Label label28;
		private NotifyIcon m_NotifyIcon;
		private OpenFileDialog openFileDialogscript;
		private System.Timers.Timer m_SystemTimer;
        private RazorButton dressStopButton;
        private System.Windows.Forms.Timer timerupdatestatus;
        private TabPage friends;
        private RazorButton btnMap;

		private bool m_CanClose = true;

		[DllImport("User32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr wnd, bool reset);
		[DllImport("User32.dll")]
		private static extern IntPtr EnableMenuItem(IntPtr menu, uint item, uint options);

		internal Label WaitDisplay { get { return waitDisp; } }

		// Enhanced Toolbar
		private EnhancedToolbar enhancedToolbar;
		internal EnhancedToolbar ToolBar { get { return enhancedToolbar; } }

		// Scripting
		internal DataGridView ScriptDataGrid { get { return dataGridViewScripting; } }

		// AutoLoot
		internal RazorCheckBox AutolootCheckBox { get { return autoLootCheckBox; } }
		internal RazorTextBox AutolootLabelDelay { get { return autoLootTextBoxDelay; } }
		internal Label AutoLootContainerLabel { get { return autolootContainerLabel; } }
		internal ListBox AutoLootLogBox { get { return autolootLogBox; } }
		internal ListView AutoLootListView { get { return autolootlistView; } }
		internal RazorComboBox AutoLootListSelect { get { return autolootListSelect; } }

		// Scavenger
		internal RazorCheckBox ScavengerCheckBox { get { return scavengerCheckBox; } }
		internal RazorTextBox ScavengerDragDelay { get { return scavengerDragDelay; } }
		internal Label ScavengerContainerLabel { get { return scavengerContainerLabel; } }
		internal ListBox ScavengerLogBox { get { return scavengerLogBox; } }
		internal ListView ScavengerListView { get { return scavengerListView; } }
		internal RazorComboBox ScavengerListSelect { get { return scavengerListSelect; } }

		// Organizer
		internal RazorTextBox OrganizerDragDelay { get { return organizerDragDelay; } }
		internal Label OrganizerSourceLabel { get { return organizerSourceLabel; } }
		internal Label OrganizerDestinationLabel { get { return organizerDestinationLabel; } }
		internal ListBox OrganizerLogBox { get { return organizerLogBox; } }
		internal ListView OrganizerListView { get { return organizerListView; } }
		internal RazorComboBox OrganizerListSelect { get { return organizerListSelect; } }
		internal Button OrganizerExecute { get { return organizerExecuteButton; } }
		internal Button OrganizerStop { get { return organizerStopButton; } }

		// Sell Agent
		internal Label SellBagLabel { get { return sellBagLabel; } }
		internal RazorCheckBox SellCheckBox { get { return sellEnableCheckBox; } }
		internal ListBox SellLogBox { get { return sellLogBox; } }
		internal ListView SellListView { get { return sellListView; } }
		internal RazorComboBox SellListSelect { get { return sellListSelect; } }

		// Buy Agent
		internal RazorCheckBox BuyCheckBox { get { return buyEnableCheckBox; } }
		internal ListBox BuyLogBox { get { return buyLogBox; } }
		internal ListView BuyListView { get { return buyListView; } }
		internal RazorComboBox BuyListSelect { get { return buyListSelect; } }

		// Dress Agent
		internal CheckBox DressCheckBox { get { return dressConflictCheckB; } }
		internal ListView DressListView { get { return dressListView; } }
		internal ListBox DressLogBox { get { return dressLogBox; } }
		internal RazorTextBox DressDragDelay { get { return dressDragDelay; } }
		internal ComboBox DressListSelect { get { return dressListSelect; } }
		internal Label DressBagLabel { get { return dressBagLabel; } }

        internal Button DressExecuteButton { get { return dressExecuteButton; } }
        internal Button UnDressExecuteButton { get { return undressExecuteButton; } }
        internal Button DressStopButton { get { return dressStopButton; } }

		// GumpInspector Flag

		internal bool GumpInspectorEnable = false;

		private DataTable scriptTable;

		internal MainForm()
		{
			m_ProfileConfirmLoad = true;
			m_Tip = new ToolTip();
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_NotifyIcon.ContextMenu =
				new ContextMenu(new MenuItem[]
				{
					new MenuItem( "Show Razor", new EventHandler( DoShowMe ) ),
					new MenuItem( "Hide Razor", new EventHandler( HideMe ) ),
					new MenuItem( "-" ),
					new MenuItem( "Toggle Razor Visibility", new EventHandler( ToggleVisible ) ),
					new MenuItem( "-" ),
					new MenuItem( "Close Razor && UO", new EventHandler( OnClose ) ),
				});
			m_NotifyIcon.ContextMenu.MenuItems[0].DefaultItem = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            RazorEnhanced.UI.Office2010BlueTheme office2010BlueTheme1 = new RazorEnhanced.UI.Office2010BlueTheme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.playMacro = new RazorEnhanced.UI.RazorButton();
            this.tabs = new System.Windows.Forms.TabControl();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.clientPrio = new RazorEnhanced.UI.RazorComboBox();
            this.lockBox = new System.Windows.Forms.PictureBox();
            this.systray = new RazorEnhanced.UI.RazorRadioButton();
            this.taskbar = new RazorEnhanced.UI.RazorRadioButton();
            this.smartCPU = new RazorEnhanced.UI.RazorCheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.delProfile = new RazorEnhanced.UI.RazorButton();
            this.newProfile = new RazorEnhanced.UI.RazorButton();
            this.profiles = new RazorEnhanced.UI.RazorComboBox();
            this.showWelcome = new RazorEnhanced.UI.RazorCheckBox();
            this.opacity = new System.Windows.Forms.TrackBar();
            this.alwaysTop = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filters = new System.Windows.Forms.CheckedListBox();
            this.opacityLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.moreOptTab = new System.Windows.Forms.TabPage();
            this.preAOSstatbar = new RazorEnhanced.UI.RazorCheckBox();
            this.negotiate = new RazorEnhanced.UI.RazorCheckBox();
            this.setLTHilight = new RazorEnhanced.UI.RazorButton();
            this.lthilight = new RazorEnhanced.UI.RazorCheckBox();
            this.filterSnoop = new RazorEnhanced.UI.RazorCheckBox();
            this.corpseRange = new RazorEnhanced.UI.RazorTextBox();
            this.incomingCorpse = new RazorEnhanced.UI.RazorCheckBox();
            this.incomingMob = new RazorEnhanced.UI.RazorCheckBox();
            this.setHarmHue = new RazorEnhanced.UI.RazorButton();
            this.setNeuHue = new RazorEnhanced.UI.RazorButton();
            this.lblHarmHue = new System.Windows.Forms.Label();
            this.lblNeuHue = new System.Windows.Forms.Label();
            this.lblBeneHue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWarnHue = new System.Windows.Forms.Label();
            this.lblMsgHue = new System.Windows.Forms.Label();
            this.lblExHue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setBeneHue = new RazorEnhanced.UI.RazorButton();
            this.setSpeechHue = new RazorEnhanced.UI.RazorButton();
            this.setWarnHue = new RazorEnhanced.UI.RazorButton();
            this.setMsgHue = new RazorEnhanced.UI.RazorButton();
            this.setExHue = new RazorEnhanced.UI.RazorButton();
            this.autoStackRes = new RazorEnhanced.UI.RazorCheckBox();
            this.queueTargets = new RazorEnhanced.UI.RazorCheckBox();
            this.spamFilter = new RazorEnhanced.UI.RazorCheckBox();
            this.openCorpses = new RazorEnhanced.UI.RazorCheckBox();
            this.blockDis = new RazorEnhanced.UI.RazorCheckBox();
            this.txtSpellFormat = new RazorEnhanced.UI.RazorTextBox();
            this.chkForceSpellHue = new RazorEnhanced.UI.RazorCheckBox();
            this.chkForceSpeechHue = new RazorEnhanced.UI.RazorCheckBox();
            this.moreMoreOptTab = new System.Windows.Forms.TabPage();
            this.msglvl = new RazorEnhanced.UI.RazorComboBox();
            this.forceSizeX = new RazorEnhanced.UI.RazorTextBox();
            this.forceSizeY = new RazorEnhanced.UI.RazorTextBox();
            this.healthFmt = new RazorEnhanced.UI.RazorTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.showHealthOH = new RazorEnhanced.UI.RazorCheckBox();
            this.blockHealPoison = new RazorEnhanced.UI.RazorCheckBox();
            this.ltRange = new RazorEnhanced.UI.RazorTextBox();
            this.potionEquip = new RazorEnhanced.UI.RazorCheckBox();
            this.txtObjDelay = new RazorEnhanced.UI.RazorTextBox();
            this.QueueActions = new RazorEnhanced.UI.RazorCheckBox();
            this.spellUnequip = new RazorEnhanced.UI.RazorCheckBox();
            this.autoOpenDoors = new RazorEnhanced.UI.RazorCheckBox();
            this.alwaysStealth = new RazorEnhanced.UI.RazorCheckBox();
            this.autoFriend = new RazorEnhanced.UI.RazorCheckBox();
            this.chkStealth = new RazorEnhanced.UI.RazorCheckBox();
            this.rememberPwds = new RazorEnhanced.UI.RazorCheckBox();
            this.showtargtext = new RazorEnhanced.UI.RazorCheckBox();
            this.logPackets = new RazorEnhanced.UI.RazorCheckBox();
            this.rangeCheckLT = new RazorEnhanced.UI.RazorCheckBox();
            this.actionStatusMsg = new RazorEnhanced.UI.RazorCheckBox();
            this.smartLT = new RazorEnhanced.UI.RazorCheckBox();
            this.gameSize = new RazorEnhanced.UI.RazorCheckBox();
            this.chkPartyOverhead = new RazorEnhanced.UI.RazorCheckBox();
            this.toolbarTab = new System.Windows.Forms.TabPage();
            this.emptyTab = new System.Windows.Forms.TabPage();
            this.skillsTab = new System.Windows.Forms.TabPage();
            this.dispDelta = new RazorEnhanced.UI.RazorCheckBox();
            this.skillCopyAll = new RazorEnhanced.UI.RazorButton();
            this.skillCopySel = new RazorEnhanced.UI.RazorButton();
            this.baseTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.locks = new RazorEnhanced.UI.RazorComboBox();
            this.setlocks = new RazorEnhanced.UI.RazorButton();
            this.resetDelta = new RazorEnhanced.UI.RazorButton();
            this.skillList = new System.Windows.Forms.ListView();
            this.skillHDRName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRvalue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRbase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRdelta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRcap = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRlock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mapsTab = new System.Windows.Forms.TabPage();
            this.btnMap = new RazorEnhanced.UI.RazorButton();
            this.hotkeysTab = new System.Windows.Forms.TabPage();
            this.hkStatus = new System.Windows.Forms.Label();
            this.hotkeyTree = new System.Windows.Forms.TreeView();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.chkPass = new RazorEnhanced.UI.RazorCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.unsetHK = new RazorEnhanced.UI.RazorButton();
            this.setHK = new RazorEnhanced.UI.RazorButton();
            this.key = new System.Windows.Forms.TextBox();
            this.chkCtrl = new RazorEnhanced.UI.RazorCheckBox();
            this.chkAlt = new RazorEnhanced.UI.RazorCheckBox();
            this.chkShift = new RazorEnhanced.UI.RazorCheckBox();
            this.dohotkey = new RazorEnhanced.UI.RazorButton();
            this.macrosTab = new System.Windows.Forms.TabPage();
            this.macroTree = new System.Windows.Forms.TreeView();
            this.macroActGroup = new System.Windows.Forms.GroupBox();
            this.macroImport = new RazorEnhanced.UI.RazorButton();
            this.exportMacro = new RazorEnhanced.UI.RazorButton();
            this.waitDisp = new System.Windows.Forms.Label();
            this.loopMacro = new RazorEnhanced.UI.RazorCheckBox();
            this.recMacro = new RazorEnhanced.UI.RazorButton();
            this.actionList = new System.Windows.Forms.ListBox();
            this.delMacro = new RazorEnhanced.UI.RazorButton();
            this.newMacro = new RazorEnhanced.UI.RazorButton();
            this.screenshotTab = new System.Windows.Forms.TabPage();
            this.imgFmt = new RazorEnhanced.UI.RazorComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.capNow = new RazorEnhanced.UI.RazorButton();
            this.screenPath = new RazorEnhanced.UI.RazorTextBox();
            this.radioUO = new RazorEnhanced.UI.RazorRadioButton();
            this.radioFull = new RazorEnhanced.UI.RazorRadioButton();
            this.screenAutoCap = new RazorEnhanced.UI.RazorCheckBox();
            this.setScnPath = new RazorEnhanced.UI.RazorButton();
            this.screensList = new System.Windows.Forms.ListBox();
            this.screenPrev = new System.Windows.Forms.PictureBox();
            this.dispTime = new RazorEnhanced.UI.RazorCheckBox();
            this.statusTab = new System.Windows.Forms.TabPage();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.razorButtonWiki = new RazorEnhanced.UI.RazorButton();
            this.razorButtonCreateUODAccount = new RazorEnhanced.UI.RazorButton();
            this.labelUOD = new System.Windows.Forms.Label();
            this.razorButtonVisitUOD = new RazorEnhanced.UI.RazorButton();
            this.panelUODlogo = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelFeatures = new System.Windows.Forms.Label();
            this.scriptingTab = new System.Windows.Forms.TabPage();
            this.razorButtonEdit = new RazorEnhanced.UI.RazorButton();
            this.razorCheckBoxAuto = new RazorEnhanced.UI.RazorCheckBox();
            this.razorButtonUp = new RazorEnhanced.UI.RazorButton();
            this.razorButtonDown = new RazorEnhanced.UI.RazorButton();
            this.dataGridViewScripting = new System.Windows.Forms.DataGridView();
            this.xButton3 = new RazorEnhanced.UI.RazorButton();
            this.xButton2 = new RazorEnhanced.UI.RazorButton();
            this.EnhancedAgent = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.eautoloot = new System.Windows.Forms.TabPage();
            this.razorButtonResetIgnore = new RazorEnhanced.UI.RazorButton();
            this.label21 = new System.Windows.Forms.Label();
            this.autoLootTextBoxDelay = new RazorEnhanced.UI.RazorTextBox();
            this.autoLootButtonRemoveList = new RazorEnhanced.UI.RazorButton();
            this.autolootButtonAddList = new RazorEnhanced.UI.RazorButton();
            this.autoLootButtonListImport = new RazorEnhanced.UI.RazorButton();
            this.autolootListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.autoLootButtonListExport = new RazorEnhanced.UI.RazorButton();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.autolootLogBox = new System.Windows.Forms.ListBox();
            this.autolootContainerLabel = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.autolootItemPropsB = new RazorEnhanced.UI.RazorButton();
            this.autolootItemEditB = new RazorEnhanced.UI.RazorButton();
            this.autolootAddItemBTarget = new RazorEnhanced.UI.RazorButton();
            this.autolootRemoveItemB = new RazorEnhanced.UI.RazorButton();
            this.autolootAddItemBManual = new RazorEnhanced.UI.RazorButton();
            this.autolootContainerButton = new RazorEnhanced.UI.RazorButton();
            this.autoLootCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.autolootlistView = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.escavenger = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.scavengerButtonEditProps = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonEditItem = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonAddTarget = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonRemoveItem = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonAddManual = new RazorEnhanced.UI.RazorButton();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.scavengerLogBox = new System.Windows.Forms.ListBox();
            this.label23 = new System.Windows.Forms.Label();
            this.scavengerDragDelay = new RazorEnhanced.UI.RazorTextBox();
            this.scavengerContainerLabel = new System.Windows.Forms.Label();
            this.scavengerButtonSetContainer = new RazorEnhanced.UI.RazorButton();
            this.scavengerCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.scavengerListView = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scavengerButtonRemoveList = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonAddList = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonImport = new RazorEnhanced.UI.RazorButton();
            this.scavengerListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.scavengerButtonExport = new RazorEnhanced.UI.RazorButton();
            this.label22 = new System.Windows.Forms.Label();
            this.Organizer = new System.Windows.Forms.TabPage();
            this.organizerStopButton = new RazorEnhanced.UI.RazorButton();
            this.organizerExecuteButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.organizerLogBox = new System.Windows.Forms.ListBox();
            this.label27 = new System.Windows.Forms.Label();
            this.organizerDragDelay = new RazorEnhanced.UI.RazorTextBox();
            this.organizerDestinationLabel = new System.Windows.Forms.Label();
            this.organizerSetDestinationB = new RazorEnhanced.UI.RazorButton();
            this.organizerSourceLabel = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.organizerEditB = new RazorEnhanced.UI.RazorButton();
            this.organizerAddTargetB = new RazorEnhanced.UI.RazorButton();
            this.organizerRemoveB = new RazorEnhanced.UI.RazorButton();
            this.organizerAddManualB = new RazorEnhanced.UI.RazorButton();
            this.organizerSetSourceB = new RazorEnhanced.UI.RazorButton();
            this.organizerListView = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.organizerRemoveListB = new RazorEnhanced.UI.RazorButton();
            this.organizerAddListB = new RazorEnhanced.UI.RazorButton();
            this.organizerImportListB = new RazorEnhanced.UI.RazorButton();
            this.organizerListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.organizerExportListB = new RazorEnhanced.UI.RazorButton();
            this.label24 = new System.Windows.Forms.Label();
            this.VendorBuy = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.buyEditB = new RazorEnhanced.UI.RazorButton();
            this.buyAddTargetB = new RazorEnhanced.UI.RazorButton();
            this.buyRemoveB = new RazorEnhanced.UI.RazorButton();
            this.buyAddManualB = new RazorEnhanced.UI.RazorButton();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.buyLogBox = new System.Windows.Forms.ListBox();
            this.buyEnableCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.buyListView = new System.Windows.Forms.ListView();
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buyRemoveListButton = new RazorEnhanced.UI.RazorButton();
            this.buyAddListButton = new RazorEnhanced.UI.RazorButton();
            this.buyImportListButton = new RazorEnhanced.UI.RazorButton();
            this.buyListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.buyExportListButton = new RazorEnhanced.UI.RazorButton();
            this.label25 = new System.Windows.Forms.Label();
            this.VendorSell = new System.Windows.Forms.TabPage();
            this.razorButton1 = new RazorEnhanced.UI.RazorButton();
            this.sellBagLabel = new System.Windows.Forms.Label();
            this.sellSetBagButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.sellEditButton = new RazorEnhanced.UI.RazorButton();
            this.sellAddTargerButton = new RazorEnhanced.UI.RazorButton();
            this.sellRemoveButton = new RazorEnhanced.UI.RazorButton();
            this.sellAddManualButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.sellLogBox = new System.Windows.Forms.ListBox();
            this.sellEnableCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.sellListView = new System.Windows.Forms.ListView();
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sellRemoveListButton = new RazorEnhanced.UI.RazorButton();
            this.sellAddListButton = new RazorEnhanced.UI.RazorButton();
            this.sellImportListButton = new RazorEnhanced.UI.RazorButton();
            this.sellListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.sellExportListButton = new RazorEnhanced.UI.RazorButton();
            this.label26 = new System.Windows.Forms.Label();
            this.Dress = new System.Windows.Forms.TabPage();
            this.dressStopButton = new RazorEnhanced.UI.RazorButton();
            this.dressConflictCheckB = new RazorEnhanced.UI.RazorCheckBox();
            this.dressBagLabel = new System.Windows.Forms.Label();
            this.dressSetBagB = new RazorEnhanced.UI.RazorButton();
            this.undressExecuteButton = new RazorEnhanced.UI.RazorButton();
            this.dressExecuteButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.dressAddTargetB = new RazorEnhanced.UI.RazorButton();
            this.dressAddManualB = new RazorEnhanced.UI.RazorButton();
            this.dressRemoveB = new RazorEnhanced.UI.RazorButton();
            this.dressReadB = new RazorEnhanced.UI.RazorButton();
            this.label29 = new System.Windows.Forms.Label();
            this.dressDragDelay = new RazorEnhanced.UI.RazorTextBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.dressLogBox = new System.Windows.Forms.ListBox();
            this.dressListView = new System.Windows.Forms.ListView();
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dressRemoveListB = new RazorEnhanced.UI.RazorButton();
            this.dressAddListB = new RazorEnhanced.UI.RazorButton();
            this.dressImportListB = new RazorEnhanced.UI.RazorButton();
            this.dressListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.dressExportListB = new RazorEnhanced.UI.RazorButton();
            this.label28 = new System.Windows.Forms.Label();
            this.friends = new System.Windows.Forms.TabPage();
            this.m_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.openFileDialogscript = new System.Windows.Forms.OpenFileDialog();
            this.timerupdatestatus = new System.Windows.Forms.Timer(this.components);
            this.tabs.SuspendLayout();
            this.generalTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.moreOptTab.SuspendLayout();
            this.moreMoreOptTab.SuspendLayout();
            this.skillsTab.SuspendLayout();
            this.mapsTab.SuspendLayout();
            this.hotkeysTab.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.macrosTab.SuspendLayout();
            this.macroActGroup.SuspendLayout();
            this.screenshotTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).BeginInit();
            this.statusTab.SuspendLayout();
            this.scriptingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripting)).BeginInit();
            this.EnhancedAgent.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.eautoloot.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.escavenger.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.Organizer.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.VendorBuy.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.VendorSell.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.Dress.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.SuspendLayout();
            // 
            // playMacro
            // 
            office2010BlueTheme1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010BlueTheme1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010BlueTheme1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010BlueTheme1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010BlueTheme1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010BlueTheme1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010BlueTheme1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010BlueTheme1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010BlueTheme1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            office2010BlueTheme1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            office2010BlueTheme1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010BlueTheme1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010BlueTheme1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010BlueTheme1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010BlueTheme1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010BlueTheme1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010BlueTheme1.TextColor = System.Drawing.Color.White;
            this.playMacro.ColorTable = office2010BlueTheme1;
            this.playMacro.Location = new System.Drawing.Point(311, 18);
            this.playMacro.Name = "playMacro";
            this.playMacro.Size = new System.Drawing.Size(60, 20);
            this.playMacro.TabIndex = 9;
            this.playMacro.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.playMacro.Click += new System.EventHandler(this.playMacro_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.generalTab);
            this.tabs.Controls.Add(this.moreOptTab);
            this.tabs.Controls.Add(this.moreMoreOptTab);
            this.tabs.Controls.Add(this.toolbarTab);
            this.tabs.Controls.Add(this.emptyTab);
            this.tabs.Controls.Add(this.skillsTab);
            this.tabs.Controls.Add(this.mapsTab);
            this.tabs.Controls.Add(this.hotkeysTab);
            this.tabs.Controls.Add(this.macrosTab);
            this.tabs.Controls.Add(this.screenshotTab);
            this.tabs.Controls.Add(this.statusTab);
            this.tabs.Controls.Add(this.scriptingTab);
            this.tabs.Controls.Add(this.EnhancedAgent);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(674, 410);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabs.TabIndex = 0;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_IndexChanged);
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.clientPrio);
            this.generalTab.Controls.Add(this.lockBox);
            this.generalTab.Controls.Add(this.systray);
            this.generalTab.Controls.Add(this.taskbar);
            this.generalTab.Controls.Add(this.smartCPU);
            this.generalTab.Controls.Add(this.label11);
            this.generalTab.Controls.Add(this.groupBox4);
            this.generalTab.Controls.Add(this.showWelcome);
            this.generalTab.Controls.Add(this.opacity);
            this.generalTab.Controls.Add(this.alwaysTop);
            this.generalTab.Controls.Add(this.groupBox1);
            this.generalTab.Controls.Add(this.opacityLabel);
            this.generalTab.Controls.Add(this.label9);
            this.generalTab.Location = new System.Drawing.Point(4, 40);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(666, 366);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            // 
            // clientPrio
            // 
            this.clientPrio.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.clientPrio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientPrio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.clientPrio.Items.AddRange(new object[] {
            "Idle",
            "BelowNormal",
            "Normal",
            "AboveNormal",
            "High",
            "Realtime"});
            this.clientPrio.Location = new System.Drawing.Point(363, 134);
            this.clientPrio.Name = "clientPrio";
            this.clientPrio.Size = new System.Drawing.Size(88, 22);
            this.clientPrio.TabIndex = 60;
            // 
            // lockBox
            // 
            this.lockBox.Cursor = System.Windows.Forms.Cursors.Help;
            this.lockBox.Image = ((System.Drawing.Image)(resources.GetObject("lockBox.Image")));
            this.lockBox.Location = new System.Drawing.Point(490, 103);
            this.lockBox.Name = "lockBox";
            this.lockBox.Size = new System.Drawing.Size(16, 16);
            this.lockBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.lockBox.TabIndex = 56;
            this.lockBox.TabStop = false;
            this.lockBox.Visible = false;
            this.lockBox.Click += new System.EventHandler(this.lockBox_Click);
            // 
            // systray
            // 
            this.systray.Location = new System.Drawing.Point(363, 101);
            this.systray.Name = "systray";
            this.systray.Size = new System.Drawing.Size(99, 20);
            this.systray.TabIndex = 35;
            this.systray.Text = "System Tray";
            this.systray.CheckedChanged += new System.EventHandler(this.systray_CheckedChanged);
            // 
            // taskbar
            // 
            this.taskbar.Location = new System.Drawing.Point(301, 102);
            this.taskbar.Name = "taskbar";
            this.taskbar.Size = new System.Drawing.Size(63, 20);
            this.taskbar.TabIndex = 34;
            this.taskbar.Text = "Taskbar";
            this.taskbar.CheckedChanged += new System.EventHandler(this.taskbar_CheckedChanged);
            // 
            // smartCPU
            // 
            this.smartCPU.Location = new System.Drawing.Point(253, 50);
            this.smartCPU.Name = "smartCPU";
            this.smartCPU.Size = new System.Drawing.Size(241, 19);
            this.smartCPU.TabIndex = 53;
            this.smartCPU.Text = "Use smart CPU usage reduction";
            this.smartCPU.CheckedChanged += new System.EventHandler(this.smartCPU_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(251, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 15);
            this.label11.TabIndex = 33;
            this.label11.Text = "Show in:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.delProfile);
            this.groupBox4.Controls.Add(this.newProfile);
            this.groupBox4.Controls.Add(this.profiles);
            this.groupBox4.Location = new System.Drawing.Point(253, 173);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(253, 50);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Profiles";
            // 
            // delProfile
            // 
            this.delProfile.ColorTable = office2010BlueTheme1;
            this.delProfile.Location = new System.Drawing.Point(192, 17);
            this.delProfile.Name = "delProfile";
            this.delProfile.Size = new System.Drawing.Size(50, 20);
            this.delProfile.TabIndex = 2;
            this.delProfile.Text = "Delete";
            this.delProfile.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.delProfile.Click += new System.EventHandler(this.delProfile_Click);
            // 
            // newProfile
            // 
            this.newProfile.ColorTable = office2010BlueTheme1;
            this.newProfile.Location = new System.Drawing.Point(135, 17);
            this.newProfile.Name = "newProfile";
            this.newProfile.Size = new System.Drawing.Size(50, 20);
            this.newProfile.TabIndex = 1;
            this.newProfile.Text = "&New...";
            this.newProfile.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.newProfile.Click += new System.EventHandler(this.newProfile_Click);
            // 
            // profiles
            // 
            this.profiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.profiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.profiles.ItemHeight = 19;
            this.profiles.Location = new System.Drawing.Point(8, 14);
            this.profiles.MaxDropDownItems = 5;
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(122, 25);
            this.profiles.TabIndex = 0;
            this.profiles.SelectedIndexChanged += new System.EventHandler(this.profiles_SelectedIndexChanged);
            // 
            // showWelcome
            // 
            this.showWelcome.Location = new System.Drawing.Point(253, 24);
            this.showWelcome.Name = "showWelcome";
            this.showWelcome.Size = new System.Drawing.Size(244, 20);
            this.showWelcome.TabIndex = 26;
            this.showWelcome.Text = "Show Welcome Screen";
            this.showWelcome.CheckedChanged += new System.EventHandler(this.showWelcome_CheckedChanged);
            // 
            // opacity
            // 
            this.opacity.AutoSize = false;
            this.opacity.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.opacity.Location = new System.Drawing.Point(331, 242);
            this.opacity.Maximum = 100;
            this.opacity.Minimum = 10;
            this.opacity.Name = "opacity";
            this.opacity.Size = new System.Drawing.Size(312, 16);
            this.opacity.TabIndex = 22;
            this.opacity.TickFrequency = 0;
            this.opacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacity.Value = 100;
            this.opacity.Scroll += new System.EventHandler(this.opacity_Scroll);
            // 
            // alwaysTop
            // 
            this.alwaysTop.Location = new System.Drawing.Point(253, 75);
            this.alwaysTop.Name = "alwaysTop";
            this.alwaysTop.Size = new System.Drawing.Size(241, 20);
            this.alwaysTop.TabIndex = 3;
            this.alwaysTop.Text = "Use Smart Always on Top";
            this.alwaysTop.CheckedChanged += new System.EventHandler(this.alwaysTop_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.filters);
            this.groupBox1.Location = new System.Drawing.Point(3, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 350);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // filters
            // 
            this.filters.CheckOnClick = true;
            this.filters.IntegralHeight = false;
            this.filters.Location = new System.Drawing.Point(6, 16);
            this.filters.Name = "filters";
            this.filters.Size = new System.Drawing.Size(190, 328);
            this.filters.TabIndex = 0;
            this.filters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnFilterCheck);
            // 
            // opacityLabel
            // 
            this.opacityLabel.Location = new System.Drawing.Point(253, 242);
            this.opacityLabel.Name = "opacityLabel";
            this.opacityLabel.Size = new System.Drawing.Size(78, 16);
            this.opacityLabel.TabIndex = 23;
            this.opacityLabel.Text = "Opacity: 100%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(251, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 19);
            this.label9.TabIndex = 59;
            this.label9.Text = "Default Client Priority:";
            // 
            // moreOptTab
            // 
            this.moreOptTab.Controls.Add(this.preAOSstatbar);
            this.moreOptTab.Controls.Add(this.negotiate);
            this.moreOptTab.Controls.Add(this.setLTHilight);
            this.moreOptTab.Controls.Add(this.lthilight);
            this.moreOptTab.Controls.Add(this.filterSnoop);
            this.moreOptTab.Controls.Add(this.corpseRange);
            this.moreOptTab.Controls.Add(this.incomingCorpse);
            this.moreOptTab.Controls.Add(this.incomingMob);
            this.moreOptTab.Controls.Add(this.setHarmHue);
            this.moreOptTab.Controls.Add(this.setNeuHue);
            this.moreOptTab.Controls.Add(this.lblHarmHue);
            this.moreOptTab.Controls.Add(this.lblNeuHue);
            this.moreOptTab.Controls.Add(this.lblBeneHue);
            this.moreOptTab.Controls.Add(this.label4);
            this.moreOptTab.Controls.Add(this.lblWarnHue);
            this.moreOptTab.Controls.Add(this.lblMsgHue);
            this.moreOptTab.Controls.Add(this.lblExHue);
            this.moreOptTab.Controls.Add(this.label3);
            this.moreOptTab.Controls.Add(this.setBeneHue);
            this.moreOptTab.Controls.Add(this.setSpeechHue);
            this.moreOptTab.Controls.Add(this.setWarnHue);
            this.moreOptTab.Controls.Add(this.setMsgHue);
            this.moreOptTab.Controls.Add(this.setExHue);
            this.moreOptTab.Controls.Add(this.autoStackRes);
            this.moreOptTab.Controls.Add(this.queueTargets);
            this.moreOptTab.Controls.Add(this.spamFilter);
            this.moreOptTab.Controls.Add(this.openCorpses);
            this.moreOptTab.Controls.Add(this.blockDis);
            this.moreOptTab.Controls.Add(this.txtSpellFormat);
            this.moreOptTab.Controls.Add(this.chkForceSpellHue);
            this.moreOptTab.Controls.Add(this.chkForceSpeechHue);
            this.moreOptTab.Location = new System.Drawing.Point(4, 40);
            this.moreOptTab.Name = "moreOptTab";
            this.moreOptTab.Size = new System.Drawing.Size(666, 366);
            this.moreOptTab.TabIndex = 5;
            this.moreOptTab.Text = "Options";
            // 
            // preAOSstatbar
            // 
            this.preAOSstatbar.Location = new System.Drawing.Point(204, 13);
            this.preAOSstatbar.Name = "preAOSstatbar";
            this.preAOSstatbar.Size = new System.Drawing.Size(190, 20);
            this.preAOSstatbar.TabIndex = 57;
            this.preAOSstatbar.Text = "Use Pre-AOS status window";
            this.preAOSstatbar.CheckedChanged += new System.EventHandler(this.preAOSstatbar_CheckedChanged);
            // 
            // negotiate
            // 
            this.negotiate.Location = new System.Drawing.Point(204, 186);
            this.negotiate.Name = "negotiate";
            this.negotiate.Size = new System.Drawing.Size(224, 20);
            this.negotiate.TabIndex = 56;
            this.negotiate.Text = "Negotiate features with server";
            this.negotiate.CheckedChanged += new System.EventHandler(this.negotiate_CheckedChanged);
            // 
            // setLTHilight
            // 
            this.setLTHilight.ColorTable = office2010BlueTheme1;
            this.setLTHilight.Location = new System.Drawing.Point(142, 108);
            this.setLTHilight.Name = "setLTHilight";
            this.setLTHilight.Size = new System.Drawing.Size(32, 20);
            this.setLTHilight.TabIndex = 51;
            this.setLTHilight.Text = "Set";
            this.setLTHilight.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setLTHilight.Click += new System.EventHandler(this.setLTHilight_Click);
            // 
            // lthilight
            // 
            this.lthilight.Location = new System.Drawing.Point(7, 111);
            this.lthilight.Name = "lthilight";
            this.lthilight.Size = new System.Drawing.Size(131, 20);
            this.lthilight.TabIndex = 50;
            this.lthilight.Text = "Last Target Highlight:";
            this.lthilight.CheckedChanged += new System.EventHandler(this.lthilight_CheckedChanged);
            // 
            // filterSnoop
            // 
            this.filterSnoop.Location = new System.Drawing.Point(204, 143);
            this.filterSnoop.Name = "filterSnoop";
            this.filterSnoop.Size = new System.Drawing.Size(230, 20);
            this.filterSnoop.TabIndex = 49;
            this.filterSnoop.Text = "Filter Snooping Messages";
            this.filterSnoop.CheckedChanged += new System.EventHandler(this.filterSnoop_CheckedChanged);
            // 
            // corpseRange
            // 
            this.corpseRange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.corpseRange.BackColor = System.Drawing.Color.White;
            this.corpseRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.corpseRange.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.corpseRange.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.corpseRange.Location = new System.Drawing.Point(355, 100);
            this.corpseRange.Name = "corpseRange";
            this.corpseRange.Size = new System.Drawing.Size(24, 20);
            this.corpseRange.TabIndex = 23;
            this.corpseRange.TextChanged += new System.EventHandler(this.corpseRange_TextChanged);
            // 
            // incomingCorpse
            // 
            this.incomingCorpse.Location = new System.Drawing.Point(204, 208);
            this.incomingCorpse.Name = "incomingCorpse";
            this.incomingCorpse.Size = new System.Drawing.Size(226, 20);
            this.incomingCorpse.TabIndex = 48;
            this.incomingCorpse.Text = "Show Names of New/Incoming Corpses";
            this.incomingCorpse.CheckedChanged += new System.EventHandler(this.incomingCorpse_CheckedChanged);
            // 
            // incomingMob
            // 
            this.incomingMob.Location = new System.Drawing.Point(204, 165);
            this.incomingMob.Name = "incomingMob";
            this.incomingMob.Size = new System.Drawing.Size(244, 20);
            this.incomingMob.TabIndex = 47;
            this.incomingMob.Text = "Show Names of Incoming People/Creatures";
            this.incomingMob.CheckedChanged += new System.EventHandler(this.incomingMob_CheckedChanged);
            // 
            // setHarmHue
            // 
            this.setHarmHue.ColorTable = office2010BlueTheme1;
            this.setHarmHue.Enabled = false;
            this.setHarmHue.Location = new System.Drawing.Point(79, 177);
            this.setHarmHue.Name = "setHarmHue";
            this.setHarmHue.Size = new System.Drawing.Size(32, 20);
            this.setHarmHue.TabIndex = 42;
            this.setHarmHue.Text = "Set";
            this.setHarmHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setHarmHue.Click += new System.EventHandler(this.setHarmHue_Click);
            // 
            // setNeuHue
            // 
            this.setNeuHue.ColorTable = office2010BlueTheme1;
            this.setNeuHue.Enabled = false;
            this.setNeuHue.Location = new System.Drawing.Point(137, 177);
            this.setNeuHue.Name = "setNeuHue";
            this.setNeuHue.Size = new System.Drawing.Size(31, 20);
            this.setNeuHue.TabIndex = 43;
            this.setNeuHue.Text = "Set";
            this.setNeuHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setNeuHue.Click += new System.EventHandler(this.setNeuHue_Click);
            // 
            // lblHarmHue
            // 
            this.lblHarmHue.Location = new System.Drawing.Point(77, 160);
            this.lblHarmHue.Name = "lblHarmHue";
            this.lblHarmHue.Size = new System.Drawing.Size(45, 14);
            this.lblHarmHue.TabIndex = 46;
            this.lblHarmHue.Text = "Harmful";
            // 
            // lblNeuHue
            // 
            this.lblNeuHue.Location = new System.Drawing.Point(135, 160);
            this.lblNeuHue.Name = "lblNeuHue";
            this.lblNeuHue.Size = new System.Drawing.Size(42, 14);
            this.lblNeuHue.TabIndex = 45;
            this.lblNeuHue.Text = "Neutral";
            // 
            // lblBeneHue
            // 
            this.lblBeneHue.Location = new System.Drawing.Point(17, 160);
            this.lblBeneHue.Name = "lblBeneHue";
            this.lblBeneHue.Size = new System.Drawing.Size(55, 14);
            this.lblBeneHue.TabIndex = 44;
            this.lblBeneHue.Text = "Beneficial";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(384, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "tiles";
            // 
            // lblWarnHue
            // 
            this.lblWarnHue.Location = new System.Drawing.Point(7, 62);
            this.lblWarnHue.Name = "lblWarnHue";
            this.lblWarnHue.Size = new System.Drawing.Size(120, 16);
            this.lblWarnHue.TabIndex = 16;
            this.lblWarnHue.Text = "Warning Message Hue";
            // 
            // lblMsgHue
            // 
            this.lblMsgHue.Location = new System.Drawing.Point(7, 37);
            this.lblMsgHue.Name = "lblMsgHue";
            this.lblMsgHue.Size = new System.Drawing.Size(115, 17);
            this.lblMsgHue.TabIndex = 15;
            this.lblMsgHue.Text = "Razor Message Hue";
            // 
            // lblExHue
            // 
            this.lblExHue.Location = new System.Drawing.Point(7, 13);
            this.lblExHue.Name = "lblExHue";
            this.lblExHue.Size = new System.Drawing.Size(120, 16);
            this.lblExHue.TabIndex = 14;
            this.lblExHue.Text = "Search Exemption Hue";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Spell Format:";
            // 
            // setBeneHue
            // 
            this.setBeneHue.ColorTable = office2010BlueTheme1;
            this.setBeneHue.Enabled = false;
            this.setBeneHue.Location = new System.Drawing.Point(24, 177);
            this.setBeneHue.Name = "setBeneHue";
            this.setBeneHue.Size = new System.Drawing.Size(33, 20);
            this.setBeneHue.TabIndex = 41;
            this.setBeneHue.Text = "Set";
            this.setBeneHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setBeneHue.Click += new System.EventHandler(this.setBeneHue_Click);
            // 
            // setSpeechHue
            // 
            this.setSpeechHue.ColorTable = office2010BlueTheme1;
            this.setSpeechHue.Location = new System.Drawing.Point(142, 84);
            this.setSpeechHue.Name = "setSpeechHue";
            this.setSpeechHue.Size = new System.Drawing.Size(32, 20);
            this.setSpeechHue.TabIndex = 40;
            this.setSpeechHue.Text = "Set";
            this.setSpeechHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setSpeechHue.Click += new System.EventHandler(this.setSpeechHue_Click);
            // 
            // setWarnHue
            // 
            this.setWarnHue.ColorTable = office2010BlueTheme1;
            this.setWarnHue.Location = new System.Drawing.Point(142, 60);
            this.setWarnHue.Name = "setWarnHue";
            this.setWarnHue.Size = new System.Drawing.Size(32, 20);
            this.setWarnHue.TabIndex = 39;
            this.setWarnHue.Text = "Set";
            this.setWarnHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setWarnHue.Click += new System.EventHandler(this.setWarnHue_Click);
            // 
            // setMsgHue
            // 
            this.setMsgHue.ColorTable = office2010BlueTheme1;
            this.setMsgHue.Location = new System.Drawing.Point(142, 36);
            this.setMsgHue.Name = "setMsgHue";
            this.setMsgHue.Size = new System.Drawing.Size(32, 19);
            this.setMsgHue.TabIndex = 38;
            this.setMsgHue.Text = "Set";
            this.setMsgHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setMsgHue.Click += new System.EventHandler(this.setMsgHue_Click);
            // 
            // setExHue
            // 
            this.setExHue.ColorTable = office2010BlueTheme1;
            this.setExHue.Location = new System.Drawing.Point(142, 11);
            this.setExHue.Name = "setExHue";
            this.setExHue.Size = new System.Drawing.Size(32, 20);
            this.setExHue.TabIndex = 37;
            this.setExHue.Text = "Set";
            this.setExHue.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setExHue.Click += new System.EventHandler(this.setExHue_Click);
            // 
            // autoStackRes
            // 
            this.autoStackRes.Location = new System.Drawing.Point(204, 78);
            this.autoStackRes.Name = "autoStackRes";
            this.autoStackRes.Size = new System.Drawing.Size(228, 20);
            this.autoStackRes.TabIndex = 35;
            this.autoStackRes.Text = "Auto-Stack Ore/Fish/Logs at Feet";
            this.autoStackRes.CheckedChanged += new System.EventHandler(this.autoStackRes_CheckedChanged);
            // 
            // queueTargets
            // 
            this.queueTargets.Location = new System.Drawing.Point(204, 35);
            this.queueTargets.Name = "queueTargets";
            this.queueTargets.Size = new System.Drawing.Size(228, 20);
            this.queueTargets.TabIndex = 34;
            this.queueTargets.Text = "Queue LastTarget and TargetSelf";
            this.queueTargets.CheckedChanged += new System.EventHandler(this.queueTargets_CheckedChanged);
            // 
            // spamFilter
            // 
            this.spamFilter.Location = new System.Drawing.Point(204, 121);
            this.spamFilter.Name = "spamFilter";
            this.spamFilter.Size = new System.Drawing.Size(228, 20);
            this.spamFilter.TabIndex = 26;
            this.spamFilter.Text = "Filter repeating system messages";
            this.spamFilter.CheckedChanged += new System.EventHandler(this.spamFilter_CheckedChanged);
            // 
            // openCorpses
            // 
            this.openCorpses.Location = new System.Drawing.Point(204, 100);
            this.openCorpses.Name = "openCorpses";
            this.openCorpses.Size = new System.Drawing.Size(156, 20);
            this.openCorpses.TabIndex = 22;
            this.openCorpses.Text = "Open new corpses within";
            this.openCorpses.CheckedChanged += new System.EventHandler(this.openCorpses_CheckedChanged);
            // 
            // blockDis
            // 
            this.blockDis.Location = new System.Drawing.Point(204, 56);
            this.blockDis.Name = "blockDis";
            this.blockDis.Size = new System.Drawing.Size(184, 20);
            this.blockDis.TabIndex = 55;
            this.blockDis.Text = "Block dismount in war mode";
            this.blockDis.CheckedChanged += new System.EventHandler(this.blockDis_CheckedChanged);
            // 
            // txtSpellFormat
            // 
            this.txtSpellFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpellFormat.BackColor = System.Drawing.Color.White;
            this.txtSpellFormat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSpellFormat.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.txtSpellFormat.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.txtSpellFormat.Location = new System.Drawing.Point(81, 211);
            this.txtSpellFormat.Name = "txtSpellFormat";
            this.txtSpellFormat.Size = new System.Drawing.Size(106, 20);
            this.txtSpellFormat.TabIndex = 5;
            this.txtSpellFormat.TextChanged += new System.EventHandler(this.txtSpellFormat_TextChanged);
            // 
            // chkForceSpellHue
            // 
            this.chkForceSpellHue.Location = new System.Drawing.Point(7, 135);
            this.chkForceSpellHue.Name = "chkForceSpellHue";
            this.chkForceSpellHue.Size = new System.Drawing.Size(127, 20);
            this.chkForceSpellHue.TabIndex = 2;
            this.chkForceSpellHue.Text = "Override Spell Hues:";
            this.chkForceSpellHue.CheckedChanged += new System.EventHandler(this.chkForceSpellHue_CheckedChanged);
            // 
            // chkForceSpeechHue
            // 
            this.chkForceSpeechHue.Location = new System.Drawing.Point(7, 87);
            this.chkForceSpeechHue.Name = "chkForceSpeechHue";
            this.chkForceSpeechHue.Size = new System.Drawing.Size(131, 20);
            this.chkForceSpeechHue.TabIndex = 0;
            this.chkForceSpeechHue.Text = "Override Speech Hue";
            this.chkForceSpeechHue.CheckedChanged += new System.EventHandler(this.chkForceSpeechHue_CheckedChanged);
            // 
            // moreMoreOptTab
            // 
            this.moreMoreOptTab.Controls.Add(this.msglvl);
            this.moreMoreOptTab.Controls.Add(this.forceSizeX);
            this.moreMoreOptTab.Controls.Add(this.forceSizeY);
            this.moreMoreOptTab.Controls.Add(this.healthFmt);
            this.moreMoreOptTab.Controls.Add(this.label10);
            this.moreMoreOptTab.Controls.Add(this.label17);
            this.moreMoreOptTab.Controls.Add(this.label5);
            this.moreMoreOptTab.Controls.Add(this.label8);
            this.moreMoreOptTab.Controls.Add(this.label6);
            this.moreMoreOptTab.Controls.Add(this.label18);
            this.moreMoreOptTab.Controls.Add(this.showHealthOH);
            this.moreMoreOptTab.Controls.Add(this.blockHealPoison);
            this.moreMoreOptTab.Controls.Add(this.ltRange);
            this.moreMoreOptTab.Controls.Add(this.potionEquip);
            this.moreMoreOptTab.Controls.Add(this.txtObjDelay);
            this.moreMoreOptTab.Controls.Add(this.QueueActions);
            this.moreMoreOptTab.Controls.Add(this.spellUnequip);
            this.moreMoreOptTab.Controls.Add(this.autoOpenDoors);
            this.moreMoreOptTab.Controls.Add(this.alwaysStealth);
            this.moreMoreOptTab.Controls.Add(this.autoFriend);
            this.moreMoreOptTab.Controls.Add(this.chkStealth);
            this.moreMoreOptTab.Controls.Add(this.rememberPwds);
            this.moreMoreOptTab.Controls.Add(this.showtargtext);
            this.moreMoreOptTab.Controls.Add(this.logPackets);
            this.moreMoreOptTab.Controls.Add(this.rangeCheckLT);
            this.moreMoreOptTab.Controls.Add(this.actionStatusMsg);
            this.moreMoreOptTab.Controls.Add(this.smartLT);
            this.moreMoreOptTab.Controls.Add(this.gameSize);
            this.moreMoreOptTab.Controls.Add(this.chkPartyOverhead);
            this.moreMoreOptTab.Location = new System.Drawing.Point(4, 40);
            this.moreMoreOptTab.Name = "moreMoreOptTab";
            this.moreMoreOptTab.Size = new System.Drawing.Size(666, 366);
            this.moreMoreOptTab.TabIndex = 10;
            this.moreMoreOptTab.Text = "More Options";
            // 
            // msglvl
            // 
            this.msglvl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.msglvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.msglvl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.msglvl.Items.AddRange(new object[] {
            "Show All",
            "Warnings & Errors",
            "Errors Only",
            "None"});
            this.msglvl.Location = new System.Drawing.Point(118, 211);
            this.msglvl.Name = "msglvl";
            this.msglvl.Size = new System.Drawing.Size(88, 22);
            this.msglvl.TabIndex = 60;
            this.msglvl.SelectedIndexChanged += new System.EventHandler(this.msglvl_SelectedIndexChanged);
            // 
            // forceSizeX
            // 
            this.forceSizeX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSizeX.BackColor = System.Drawing.Color.White;
            this.forceSizeX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forceSizeX.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.forceSizeX.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.forceSizeX.Location = new System.Drawing.Point(375, 186);
            this.forceSizeX.Name = "forceSizeX";
            this.forceSizeX.Size = new System.Drawing.Size(30, 20);
            this.forceSizeX.TabIndex = 63;
            this.forceSizeX.TextChanged += new System.EventHandler(this.forceSizeX_TextChanged);
            // 
            // forceSizeY
            // 
            this.forceSizeY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSizeY.BackColor = System.Drawing.Color.White;
            this.forceSizeY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forceSizeY.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.forceSizeY.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.forceSizeY.Location = new System.Drawing.Point(417, 186);
            this.forceSizeY.Name = "forceSizeY";
            this.forceSizeY.Size = new System.Drawing.Size(30, 20);
            this.forceSizeY.TabIndex = 64;
            this.forceSizeY.TextChanged += new System.EventHandler(this.forceSizeY_TextChanged);
            // 
            // healthFmt
            // 
            this.healthFmt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.healthFmt.BackColor = System.Drawing.Color.White;
            this.healthFmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.healthFmt.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.healthFmt.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.healthFmt.Location = new System.Drawing.Point(159, 159);
            this.healthFmt.Name = "healthFmt";
            this.healthFmt.Size = new System.Drawing.Size(46, 20);
            this.healthFmt.TabIndex = 71;
            this.healthFmt.TextChanged += new System.EventHandler(this.healthFmt_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(7, 164);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 17);
            this.label10.TabIndex = 70;
            this.label10.Text = "Health Format:";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(7, 216);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(114, 18);
            this.label17.TabIndex = 59;
            this.label17.Text = "Razor messages:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 18);
            this.label5.TabIndex = 35;
            this.label5.Text = "Object delay:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(196, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 18);
            this.label8.TabIndex = 42;
            this.label8.Text = "tiles";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(197, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 18);
            this.label6.TabIndex = 36;
            this.label6.Text = "ms";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(372, 211);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(10, 19);
            this.label18.TabIndex = 66;
            this.label18.Text = "x";
            // 
            // showHealthOH
            // 
            this.showHealthOH.Location = new System.Drawing.Point(7, 143);
            this.showHealthOH.Name = "showHealthOH";
            this.showHealthOH.Size = new System.Drawing.Size(214, 20);
            this.showHealthOH.TabIndex = 69;
            this.showHealthOH.Text = "Show health above people/creatures";
            this.showHealthOH.CheckedChanged += new System.EventHandler(this.showHealthOH_CheckedChanged);
            // 
            // blockHealPoison
            // 
            this.blockHealPoison.Location = new System.Drawing.Point(238, 165);
            this.blockHealPoison.Name = "blockHealPoison";
            this.blockHealPoison.Size = new System.Drawing.Size(214, 20);
            this.blockHealPoison.TabIndex = 68;
            this.blockHealPoison.Text = "Block heal if target is poisoned";
            this.blockHealPoison.CheckedChanged += new System.EventHandler(this.blockHealPoison_CheckedChanged);
            // 
            // ltRange
            // 
            this.ltRange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltRange.BackColor = System.Drawing.Color.White;
            this.ltRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ltRange.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.ltRange.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.ltRange.Location = new System.Drawing.Point(159, 99);
            this.ltRange.Name = "ltRange";
            this.ltRange.Size = new System.Drawing.Size(32, 20);
            this.ltRange.TabIndex = 41;
            this.ltRange.TextChanged += new System.EventHandler(this.ltRange_TextChanged);
            // 
            // potionEquip
            // 
            this.potionEquip.Location = new System.Drawing.Point(238, 143);
            this.potionEquip.Name = "potionEquip";
            this.potionEquip.Size = new System.Drawing.Size(214, 20);
            this.potionEquip.TabIndex = 67;
            this.potionEquip.Text = "Auto Un/Re-equip hands for potions";
            this.potionEquip.CheckedChanged += new System.EventHandler(this.potionEquip_CheckedChanged);
            // 
            // txtObjDelay
            // 
            this.txtObjDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjDelay.BackColor = System.Drawing.Color.White;
            this.txtObjDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtObjDelay.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.txtObjDelay.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.txtObjDelay.Location = new System.Drawing.Point(159, 55);
            this.txtObjDelay.Name = "txtObjDelay";
            this.txtObjDelay.Size = new System.Drawing.Size(32, 20);
            this.txtObjDelay.TabIndex = 37;
            this.txtObjDelay.TextChanged += new System.EventHandler(this.txtObjDelay_TextChanged);
            // 
            // QueueActions
            // 
            this.QueueActions.Location = new System.Drawing.Point(7, 35);
            this.QueueActions.Name = "QueueActions";
            this.QueueActions.Size = new System.Drawing.Size(211, 20);
            this.QueueActions.TabIndex = 34;
            this.QueueActions.Text = "Auto-Queue Object Delay actions ";
            this.QueueActions.CheckedChanged += new System.EventHandler(this.QueueActions_CheckedChanged);
            // 
            // spellUnequip
            // 
            this.spellUnequip.Location = new System.Drawing.Point(238, 121);
            this.spellUnequip.Name = "spellUnequip";
            this.spellUnequip.Size = new System.Drawing.Size(214, 20);
            this.spellUnequip.TabIndex = 39;
            this.spellUnequip.Text = "Auto Unequip hands before casting";
            this.spellUnequip.CheckedChanged += new System.EventHandler(this.spellUnequip_CheckedChanged);
            // 
            // autoOpenDoors
            // 
            this.autoOpenDoors.Location = new System.Drawing.Point(238, 100);
            this.autoOpenDoors.Name = "autoOpenDoors";
            this.autoOpenDoors.Size = new System.Drawing.Size(190, 20);
            this.autoOpenDoors.TabIndex = 58;
            this.autoOpenDoors.Text = "Automatically open doors";
            this.autoOpenDoors.CheckedChanged += new System.EventHandler(this.autoOpenDoors_CheckedChanged);
            // 
            // alwaysStealth
            // 
            this.alwaysStealth.Location = new System.Drawing.Point(238, 78);
            this.alwaysStealth.Name = "alwaysStealth";
            this.alwaysStealth.Size = new System.Drawing.Size(190, 20);
            this.alwaysStealth.TabIndex = 57;
            this.alwaysStealth.Text = "Always show stealth steps ";
            this.alwaysStealth.CheckedChanged += new System.EventHandler(this.alwaysStealth_CheckedChanged);
            // 
            // autoFriend
            // 
            this.autoFriend.Location = new System.Drawing.Point(238, 35);
            this.autoFriend.Name = "autoFriend";
            this.autoFriend.Size = new System.Drawing.Size(190, 20);
            this.autoFriend.TabIndex = 56;
            this.autoFriend.Text = "Treat party members as \'Friends\'";
            this.autoFriend.CheckedChanged += new System.EventHandler(this.autoFriend_CheckedChanged);
            // 
            // chkStealth
            // 
            this.chkStealth.Location = new System.Drawing.Point(238, 56);
            this.chkStealth.Name = "chkStealth";
            this.chkStealth.Size = new System.Drawing.Size(190, 20);
            this.chkStealth.TabIndex = 12;
            this.chkStealth.Text = "Count stealth steps";
            this.chkStealth.CheckedChanged += new System.EventHandler(this.chkStealth_CheckedChanged);
            // 
            // rememberPwds
            // 
            this.rememberPwds.Location = new System.Drawing.Point(238, 13);
            this.rememberPwds.Name = "rememberPwds";
            this.rememberPwds.Size = new System.Drawing.Size(190, 20);
            this.rememberPwds.TabIndex = 54;
            this.rememberPwds.Text = "Remember passwords ";
            this.rememberPwds.CheckedChanged += new System.EventHandler(this.rememberPwds_CheckedChanged);
            // 
            // showtargtext
            // 
            this.showtargtext.Location = new System.Drawing.Point(7, 121);
            this.showtargtext.Name = "showtargtext";
            this.showtargtext.Size = new System.Drawing.Size(190, 20);
            this.showtargtext.TabIndex = 53;
            this.showtargtext.Text = "Show target flag on single click";
            this.showtargtext.CheckedChanged += new System.EventHandler(this.showtargtext_CheckedChanged);
            // 
            // logPackets
            // 
            this.logPackets.Location = new System.Drawing.Point(238, 208);
            this.logPackets.Name = "logPackets";
            this.logPackets.Size = new System.Drawing.Size(186, 22);
            this.logPackets.TabIndex = 50;
            this.logPackets.Text = "Enable packet logging";
            this.logPackets.CheckedChanged += new System.EventHandler(this.logPackets_CheckedChanged);
            // 
            // rangeCheckLT
            // 
            this.rangeCheckLT.Location = new System.Drawing.Point(7, 100);
            this.rangeCheckLT.Name = "rangeCheckLT";
            this.rangeCheckLT.Size = new System.Drawing.Size(151, 20);
            this.rangeCheckLT.TabIndex = 40;
            this.rangeCheckLT.Text = "Range check Last Target:";
            this.rangeCheckLT.CheckedChanged += new System.EventHandler(this.rangeCheckLT_CheckedChanged);
            // 
            // actionStatusMsg
            // 
            this.actionStatusMsg.Location = new System.Drawing.Point(7, 13);
            this.actionStatusMsg.Name = "actionStatusMsg";
            this.actionStatusMsg.Size = new System.Drawing.Size(211, 20);
            this.actionStatusMsg.TabIndex = 38;
            this.actionStatusMsg.Text = "Show Action-Queue status messages";
            this.actionStatusMsg.CheckedChanged += new System.EventHandler(this.actionStatusMsg_CheckedChanged);
            // 
            // smartLT
            // 
            this.smartLT.Location = new System.Drawing.Point(7, 78);
            this.smartLT.Name = "smartLT";
            this.smartLT.Size = new System.Drawing.Size(185, 20);
            this.smartLT.TabIndex = 52;
            this.smartLT.Text = "Use smart last target";
            this.smartLT.CheckedChanged += new System.EventHandler(this.smartLT_CheckedChanged);
            // 
            // gameSize
            // 
            this.gameSize.Location = new System.Drawing.Point(238, 186);
            this.gameSize.Name = "gameSize";
            this.gameSize.Size = new System.Drawing.Size(114, 19);
            this.gameSize.TabIndex = 65;
            this.gameSize.Text = "Force Game Size:";
            this.gameSize.CheckedChanged += new System.EventHandler(this.gameSize_CheckedChanged);
            // 
            // chkPartyOverhead
            // 
            this.chkPartyOverhead.Location = new System.Drawing.Point(7, 183);
            this.chkPartyOverhead.Name = "chkPartyOverhead";
            this.chkPartyOverhead.Size = new System.Drawing.Size(224, 20);
            this.chkPartyOverhead.TabIndex = 72;
            this.chkPartyOverhead.Text = "Show mana/stam above party members";
            this.chkPartyOverhead.CheckedChanged += new System.EventHandler(this.chkPartyOverhead_CheckedChanged);
            // 
            // toolbarTab
            // 
            this.toolbarTab.Location = new System.Drawing.Point(4, 40);
            this.toolbarTab.Name = "toolbarTab";
            this.toolbarTab.Size = new System.Drawing.Size(666, 366);
            this.toolbarTab.TabIndex = 1;
            this.toolbarTab.Text = "Enhanced Toolbar";
            // 
            // emptyTab
            // 
            this.emptyTab.Location = new System.Drawing.Point(4, 40);
            this.emptyTab.Name = "emptyTab";
            this.emptyTab.Size = new System.Drawing.Size(666, 366);
            this.emptyTab.TabIndex = 3;
            this.emptyTab.Text = " UNUSESD";
            this.emptyTab.Click += new System.EventHandler(this.dressTab_Click);
            // 
            // skillsTab
            // 
            this.skillsTab.Controls.Add(this.dispDelta);
            this.skillsTab.Controls.Add(this.skillCopyAll);
            this.skillsTab.Controls.Add(this.skillCopySel);
            this.skillsTab.Controls.Add(this.baseTotal);
            this.skillsTab.Controls.Add(this.label1);
            this.skillsTab.Controls.Add(this.locks);
            this.skillsTab.Controls.Add(this.setlocks);
            this.skillsTab.Controls.Add(this.resetDelta);
            this.skillsTab.Controls.Add(this.skillList);
            this.skillsTab.Location = new System.Drawing.Point(4, 40);
            this.skillsTab.Name = "skillsTab";
            this.skillsTab.Size = new System.Drawing.Size(666, 366);
            this.skillsTab.TabIndex = 2;
            this.skillsTab.Text = "Skills";
            // 
            // dispDelta
            // 
            this.dispDelta.Location = new System.Drawing.Point(402, 132);
            this.dispDelta.Name = "dispDelta";
            this.dispDelta.Size = new System.Drawing.Size(113, 20);
            this.dispDelta.TabIndex = 11;
            this.dispDelta.Text = "Display skill and stat changes";
            this.dispDelta.CheckedChanged += new System.EventHandler(this.dispDelta_CheckedChanged);
            // 
            // skillCopyAll
            // 
            this.skillCopyAll.ColorTable = office2010BlueTheme1;
            this.skillCopyAll.Location = new System.Drawing.Point(402, 100);
            this.skillCopyAll.Name = "skillCopyAll";
            this.skillCopyAll.Size = new System.Drawing.Size(115, 20);
            this.skillCopyAll.TabIndex = 9;
            this.skillCopyAll.Text = "Copy All";
            this.skillCopyAll.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.skillCopyAll.Click += new System.EventHandler(this.skillCopyAll_Click);
            // 
            // skillCopySel
            // 
            this.skillCopySel.ColorTable = office2010BlueTheme1;
            this.skillCopySel.Location = new System.Drawing.Point(402, 75);
            this.skillCopySel.Name = "skillCopySel";
            this.skillCopySel.Size = new System.Drawing.Size(115, 21);
            this.skillCopySel.TabIndex = 8;
            this.skillCopySel.Text = "Copy Selected";
            this.skillCopySel.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.skillCopySel.Click += new System.EventHandler(this.skillCopySel_Click);
            // 
            // baseTotal
            // 
            this.baseTotal.Location = new System.Drawing.Point(471, 161);
            this.baseTotal.Name = "baseTotal";
            this.baseTotal.ReadOnly = true;
            this.baseTotal.Size = new System.Drawing.Size(44, 20);
            this.baseTotal.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(401, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Base Total:";
            // 
            // locks
            // 
            this.locks.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.locks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locks.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Locked"});
            this.locks.Location = new System.Drawing.Point(483, 42);
            this.locks.Name = "locks";
            this.locks.Size = new System.Drawing.Size(37, 22);
            this.locks.TabIndex = 5;
            // 
            // setlocks
            // 
            this.setlocks.ColorTable = office2010BlueTheme1;
            this.setlocks.Location = new System.Drawing.Point(402, 42);
            this.setlocks.Name = "setlocks";
            this.setlocks.Size = new System.Drawing.Size(76, 20);
            this.setlocks.TabIndex = 4;
            this.setlocks.Text = "Set all locks:";
            this.setlocks.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setlocks.Click += new System.EventHandler(this.OnSetSkillLocks);
            // 
            // resetDelta
            // 
            this.resetDelta.ColorTable = office2010BlueTheme1;
            this.resetDelta.Location = new System.Drawing.Point(402, 13);
            this.resetDelta.Name = "resetDelta";
            this.resetDelta.Size = new System.Drawing.Size(115, 20);
            this.resetDelta.TabIndex = 3;
            this.resetDelta.Text = "Reset  +/-";
            this.resetDelta.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.resetDelta.Click += new System.EventHandler(this.OnResetSkillDelta);
            // 
            // skillList
            // 
            this.skillList.AutoArrange = false;
            this.skillList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.skillHDRName,
            this.skillHDRvalue,
            this.skillHDRbase,
            this.skillHDRdelta,
            this.skillHDRcap,
            this.skillHDRlock});
            this.skillList.FullRowSelect = true;
            this.skillList.Location = new System.Drawing.Point(7, 13);
            this.skillList.Name = "skillList";
            this.skillList.Size = new System.Drawing.Size(376, 260);
            this.skillList.TabIndex = 1;
            this.skillList.UseCompatibleStateImageBehavior = false;
            this.skillList.View = System.Windows.Forms.View.Details;
            this.skillList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnSkillColClick);
            this.skillList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skillList_MouseDown);
            // 
            // skillHDRName
            // 
            this.skillHDRName.Text = "Skill Name";
            this.skillHDRName.Width = 180;
            // 
            // skillHDRvalue
            // 
            this.skillHDRvalue.Text = "Value";
            // 
            // skillHDRbase
            // 
            this.skillHDRbase.Text = "Base";
            this.skillHDRbase.Width = 50;
            // 
            // skillHDRdelta
            // 
            this.skillHDRdelta.Text = "+/-";
            this.skillHDRdelta.Width = 40;
            // 
            // skillHDRcap
            // 
            this.skillHDRcap.Text = "Cap";
            this.skillHDRcap.Width = 40;
            // 
            // skillHDRlock
            // 
            this.skillHDRlock.Text = "Lock";
            this.skillHDRlock.Width = 55;
            // 
            // mapsTab
            // 
            this.mapsTab.Controls.Add(this.btnMap);
            this.mapsTab.Location = new System.Drawing.Point(4, 40);
            this.mapsTab.Name = "mapsTab";
            this.mapsTab.Size = new System.Drawing.Size(666, 366);
            this.mapsTab.TabIndex = 6;
            this.mapsTab.Text = "Enhanced Map";
            // 
            // btnMap
            // 
            this.btnMap.ColorTable = office2010BlueTheme1;
            this.btnMap.Location = new System.Drawing.Point(8, 17);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(102, 31);
            this.btnMap.TabIndex = 59;
            this.btnMap.Text = "Map UO";
            this.btnMap.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            // 
            // hotkeysTab
            // 
            this.hotkeysTab.Controls.Add(this.hkStatus);
            this.hotkeysTab.Controls.Add(this.hotkeyTree);
            this.hotkeysTab.Controls.Add(this.groupBox8);
            this.hotkeysTab.Controls.Add(this.dohotkey);
            this.hotkeysTab.Location = new System.Drawing.Point(4, 40);
            this.hotkeysTab.Name = "hotkeysTab";
            this.hotkeysTab.Size = new System.Drawing.Size(666, 366);
            this.hotkeysTab.TabIndex = 4;
            this.hotkeysTab.Text = "Hot Keys";
            // 
            // hkStatus
            // 
            this.hkStatus.Location = new System.Drawing.Point(366, 177);
            this.hkStatus.Name = "hkStatus";
            this.hkStatus.Size = new System.Drawing.Size(160, 15);
            this.hkStatus.TabIndex = 7;
            // 
            // hotkeyTree
            // 
            this.hotkeyTree.HideSelection = false;
            this.hotkeyTree.Location = new System.Drawing.Point(7, 13);
            this.hotkeyTree.Name = "hotkeyTree";
            this.hotkeyTree.Size = new System.Drawing.Size(345, 260);
            this.hotkeyTree.Sorted = true;
            this.hotkeyTree.TabIndex = 6;
            this.hotkeyTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.hotkeyTree_AfterSelect);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.chkPass);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.unsetHK);
            this.groupBox8.Controls.Add(this.setHK);
            this.groupBox8.Controls.Add(this.key);
            this.groupBox8.Controls.Add(this.chkCtrl);
            this.groupBox8.Controls.Add(this.chkAlt);
            this.groupBox8.Controls.Add(this.chkShift);
            this.groupBox8.Location = new System.Drawing.Point(366, 13);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(160, 124);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Hot Key";
            // 
            // chkPass
            // 
            this.chkPass.Location = new System.Drawing.Point(8, 68);
            this.chkPass.Name = "chkPass";
            this.chkPass.Size = new System.Drawing.Size(144, 20);
            this.chkPass.TabIndex = 9;
            this.chkPass.Text = "Pass to UO";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Key:";
            // 
            // unsetHK
            // 
            this.unsetHK.ColorTable = office2010BlueTheme1;
            this.unsetHK.Location = new System.Drawing.Point(8, 96);
            this.unsetHK.Name = "unsetHK";
            this.unsetHK.Size = new System.Drawing.Size(52, 20);
            this.unsetHK.TabIndex = 6;
            this.unsetHK.Text = "Unset";
            this.unsetHK.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.unsetHK.Click += new System.EventHandler(this.unsetHK_Click);
            // 
            // setHK
            // 
            this.setHK.ColorTable = office2010BlueTheme1;
            this.setHK.Location = new System.Drawing.Point(104, 96);
            this.setHK.Name = "setHK";
            this.setHK.Size = new System.Drawing.Size(48, 20);
            this.setHK.TabIndex = 5;
            this.setHK.Text = "Set";
            this.setHK.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setHK.Click += new System.EventHandler(this.setHK_Click);
            // 
            // key
            // 
            this.key.Location = new System.Drawing.Point(36, 43);
            this.key.Name = "key";
            this.key.ReadOnly = true;
            this.key.Size = new System.Drawing.Size(116, 20);
            this.key.TabIndex = 4;
            this.key.KeyUp += new System.Windows.Forms.KeyEventHandler(this.key_KeyUp);
            this.key.MouseDown += new System.Windows.Forms.MouseEventHandler(this.key_MouseDown);
            this.key.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.key_MouseWheel);
            // 
            // chkCtrl
            // 
            this.chkCtrl.Location = new System.Drawing.Point(8, 20);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(44, 20);
            this.chkCtrl.TabIndex = 1;
            this.chkCtrl.Text = "Ctrl";
            // 
            // chkAlt
            // 
            this.chkAlt.Location = new System.Drawing.Point(60, 20);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(36, 20);
            this.chkAlt.TabIndex = 2;
            this.chkAlt.Text = "Alt";
            // 
            // chkShift
            // 
            this.chkShift.Location = new System.Drawing.Point(104, 20);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(48, 20);
            this.chkShift.TabIndex = 3;
            this.chkShift.Text = "Shift";
            // 
            // dohotkey
            // 
            this.dohotkey.ColorTable = office2010BlueTheme1;
            this.dohotkey.Location = new System.Drawing.Point(366, 145);
            this.dohotkey.Name = "dohotkey";
            this.dohotkey.Size = new System.Drawing.Size(160, 20);
            this.dohotkey.TabIndex = 5;
            this.dohotkey.Text = "Execute Selected";
            this.dohotkey.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dohotkey.Click += new System.EventHandler(this.dohotkey_Click);
            // 
            // macrosTab
            // 
            this.macrosTab.Controls.Add(this.macroTree);
            this.macrosTab.Controls.Add(this.macroActGroup);
            this.macrosTab.Controls.Add(this.delMacro);
            this.macrosTab.Controls.Add(this.newMacro);
            this.macrosTab.Location = new System.Drawing.Point(4, 40);
            this.macrosTab.Name = "macrosTab";
            this.macrosTab.Size = new System.Drawing.Size(666, 366);
            this.macrosTab.TabIndex = 7;
            this.macrosTab.Text = "Macros";
            // 
            // macroTree
            // 
            this.macroTree.FullRowSelect = true;
            this.macroTree.HideSelection = false;
            this.macroTree.Location = new System.Drawing.Point(7, 12);
            this.macroTree.Name = "macroTree";
            this.macroTree.Size = new System.Drawing.Size(135, 231);
            this.macroTree.Sorted = true;
            this.macroTree.TabIndex = 4;
            this.macroTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.macroTree_AfterSelect);
            this.macroTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.macroTree_MouseDown);
            // 
            // macroActGroup
            // 
            this.macroActGroup.Controls.Add(this.macroImport);
            this.macroActGroup.Controls.Add(this.exportMacro);
            this.macroActGroup.Controls.Add(this.waitDisp);
            this.macroActGroup.Controls.Add(this.loopMacro);
            this.macroActGroup.Controls.Add(this.recMacro);
            this.macroActGroup.Controls.Add(this.playMacro);
            this.macroActGroup.Controls.Add(this.actionList);
            this.macroActGroup.Location = new System.Drawing.Point(150, 9);
            this.macroActGroup.Name = "macroActGroup";
            this.macroActGroup.Size = new System.Drawing.Size(376, 264);
            this.macroActGroup.TabIndex = 3;
            this.macroActGroup.TabStop = false;
            this.macroActGroup.Text = "Actions";
            this.macroActGroup.Visible = false;
            // 
            // macroImport
            // 
            this.macroImport.ColorTable = office2010BlueTheme1;
            this.macroImport.Location = new System.Drawing.Point(311, 106);
            this.macroImport.Name = "macroImport";
            this.macroImport.Size = new System.Drawing.Size(60, 20);
            this.macroImport.TabIndex = 7;
            this.macroImport.Text = "Import";
            this.macroImport.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.macroImport.Click += new System.EventHandler(this.macroImport_Click);
            // 
            // exportMacro
            // 
            this.exportMacro.ColorTable = office2010BlueTheme1;
            this.exportMacro.Location = new System.Drawing.Point(311, 81);
            this.exportMacro.Name = "exportMacro";
            this.exportMacro.Size = new System.Drawing.Size(60, 20);
            this.exportMacro.TabIndex = 6;
            this.exportMacro.Text = "Export";
            this.exportMacro.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.exportMacro.Click += new System.EventHandler(this.exportMacro_Click);
            // 
            // waitDisp
            // 
            this.waitDisp.Location = new System.Drawing.Point(308, 176);
            this.waitDisp.Name = "waitDisp";
            this.waitDisp.Size = new System.Drawing.Size(60, 43);
            this.waitDisp.TabIndex = 5;
            this.waitDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loopMacro
            // 
            this.loopMacro.Location = new System.Drawing.Point(311, 232);
            this.loopMacro.Name = "loopMacro";
            this.loopMacro.Size = new System.Drawing.Size(60, 20);
            this.loopMacro.TabIndex = 4;
            this.loopMacro.Text = "Loop";
            this.loopMacro.CheckedChanged += new System.EventHandler(this.loopMacro_CheckedChanged);
            // 
            // recMacro
            // 
            this.recMacro.ColorTable = office2010BlueTheme1;
            this.recMacro.Location = new System.Drawing.Point(311, 55);
            this.recMacro.Name = "recMacro";
            this.recMacro.Size = new System.Drawing.Size(60, 20);
            this.recMacro.TabIndex = 3;
            this.recMacro.Text = "Record";
            this.recMacro.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.recMacro.Click += new System.EventHandler(this.recMacro_Click);
            // 
            // actionList
            // 
            this.actionList.BackColor = System.Drawing.SystemColors.Window;
            this.actionList.HorizontalScrollbar = true;
            this.actionList.IntegralHeight = false;
            this.actionList.Location = new System.Drawing.Point(8, 16);
            this.actionList.Name = "actionList";
            this.actionList.Size = new System.Drawing.Size(288, 243);
            this.actionList.TabIndex = 0;
            this.actionList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.actionList_KeyDown);
            this.actionList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.actionList_MouseDown);
            // 
            // delMacro
            // 
            this.delMacro.ColorTable = office2010BlueTheme1;
            this.delMacro.Location = new System.Drawing.Point(82, 248);
            this.delMacro.Name = "delMacro";
            this.delMacro.Size = new System.Drawing.Size(60, 20);
            this.delMacro.TabIndex = 2;
            this.delMacro.Text = "Remove";
            this.delMacro.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.delMacro.Click += new System.EventHandler(this.delMacro_Click);
            // 
            // newMacro
            // 
            this.newMacro.ColorTable = office2010BlueTheme1;
            this.newMacro.Location = new System.Drawing.Point(7, 248);
            this.newMacro.Name = "newMacro";
            this.newMacro.Size = new System.Drawing.Size(60, 20);
            this.newMacro.TabIndex = 1;
            this.newMacro.Text = "New...";
            this.newMacro.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.newMacro.Click += new System.EventHandler(this.newMacro_Click);
            // 
            // screenshotTab
            // 
            this.screenshotTab.Controls.Add(this.imgFmt);
            this.screenshotTab.Controls.Add(this.label12);
            this.screenshotTab.Controls.Add(this.capNow);
            this.screenshotTab.Controls.Add(this.screenPath);
            this.screenshotTab.Controls.Add(this.radioUO);
            this.screenshotTab.Controls.Add(this.radioFull);
            this.screenshotTab.Controls.Add(this.screenAutoCap);
            this.screenshotTab.Controls.Add(this.setScnPath);
            this.screenshotTab.Controls.Add(this.screensList);
            this.screenshotTab.Controls.Add(this.screenPrev);
            this.screenshotTab.Controls.Add(this.dispTime);
            this.screenshotTab.Location = new System.Drawing.Point(4, 40);
            this.screenshotTab.Name = "screenshotTab";
            this.screenshotTab.Size = new System.Drawing.Size(666, 366);
            this.screenshotTab.TabIndex = 8;
            this.screenshotTab.Text = "Screen Shots";
            // 
            // imgFmt
            // 
            this.imgFmt.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.imgFmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imgFmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.imgFmt.Items.AddRange(new object[] {
            "jpg",
            "png",
            "bmp",
            "gif",
            "tif",
            "wmf",
            "exif",
            "emf"});
            this.imgFmt.Location = new System.Drawing.Point(94, 202);
            this.imgFmt.Name = "imgFmt";
            this.imgFmt.Size = new System.Drawing.Size(71, 22);
            this.imgFmt.TabIndex = 11;
            this.imgFmt.SelectedIndexChanged += new System.EventHandler(this.imgFmt_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(8, 205);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Image Format:";
            // 
            // capNow
            // 
            this.capNow.ColorTable = office2010BlueTheme1;
            this.capNow.Location = new System.Drawing.Point(314, 14);
            this.capNow.Name = "capNow";
            this.capNow.Size = new System.Drawing.Size(285, 20);
            this.capNow.TabIndex = 8;
            this.capNow.Text = "Take Screen Shot Now";
            this.capNow.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.capNow.Click += new System.EventHandler(this.capNow_Click);
            // 
            // screenPath
            // 
            this.screenPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenPath.BackColor = System.Drawing.Color.White;
            this.screenPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPath.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.screenPath.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.screenPath.Location = new System.Drawing.Point(7, 14);
            this.screenPath.Name = "screenPath";
            this.screenPath.Size = new System.Drawing.Size(196, 20);
            this.screenPath.TabIndex = 7;
            this.screenPath.TextChanged += new System.EventHandler(this.screenPath_TextChanged);
            // 
            // radioUO
            // 
            this.radioUO.Location = new System.Drawing.Point(11, 228);
            this.radioUO.Name = "radioUO";
            this.radioUO.Size = new System.Drawing.Size(87, 20);
            this.radioUO.TabIndex = 6;
            this.radioUO.Text = "UO Only";
            this.radioUO.CheckedChanged += new System.EventHandler(this.radioUO_CheckedChanged);
            // 
            // radioFull
            // 
            this.radioFull.Location = new System.Drawing.Point(102, 228);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(89, 20);
            this.radioFull.TabIndex = 5;
            this.radioFull.Text = "Full Screen";
            this.radioFull.CheckedChanged += new System.EventHandler(this.radioFull_CheckedChanged);
            // 
            // screenAutoCap
            // 
            this.screenAutoCap.Location = new System.Drawing.Point(11, 284);
            this.screenAutoCap.Name = "screenAutoCap";
            this.screenAutoCap.Size = new System.Drawing.Size(180, 20);
            this.screenAutoCap.TabIndex = 4;
            this.screenAutoCap.Text = "Auto Death Screen Capture";
            this.screenAutoCap.CheckedChanged += new System.EventHandler(this.screenAutoCap_CheckedChanged);
            // 
            // setScnPath
            // 
            this.setScnPath.ColorTable = office2010BlueTheme1;
            this.setScnPath.Location = new System.Drawing.Point(208, 16);
            this.setScnPath.Name = "setScnPath";
            this.setScnPath.Size = new System.Drawing.Size(22, 17);
            this.setScnPath.TabIndex = 3;
            this.setScnPath.Text = "...";
            this.setScnPath.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.setScnPath.Click += new System.EventHandler(this.setScnPath_Click);
            // 
            // screensList
            // 
            this.screensList.IntegralHeight = false;
            this.screensList.Location = new System.Drawing.Point(7, 40);
            this.screensList.Name = "screensList";
            this.screensList.Size = new System.Drawing.Size(223, 147);
            this.screensList.Sorted = true;
            this.screensList.TabIndex = 1;
            this.screensList.SelectedIndexChanged += new System.EventHandler(this.screensList_SelectedIndexChanged);
            this.screensList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screensList_MouseDown);
            // 
            // screenPrev
            // 
            this.screenPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPrev.Location = new System.Drawing.Point(246, 36);
            this.screenPrev.Name = "screenPrev";
            this.screenPrev.Size = new System.Drawing.Size(412, 322);
            this.screenPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenPrev.TabIndex = 0;
            this.screenPrev.TabStop = false;
            this.screenPrev.Click += new System.EventHandler(this.screenPrev_Click);
            // 
            // dispTime
            // 
            this.dispTime.Location = new System.Drawing.Point(11, 256);
            this.dispTime.Name = "dispTime";
            this.dispTime.Size = new System.Drawing.Size(180, 20);
            this.dispTime.TabIndex = 9;
            this.dispTime.Text = "Include Timestamp on images";
            this.dispTime.CheckedChanged += new System.EventHandler(this.dispTime_CheckedChanged);
            // 
            // statusTab
            // 
            this.statusTab.Controls.Add(this.panelLogo);
            this.statusTab.Controls.Add(this.razorButtonWiki);
            this.statusTab.Controls.Add(this.razorButtonCreateUODAccount);
            this.statusTab.Controls.Add(this.labelUOD);
            this.statusTab.Controls.Add(this.razorButtonVisitUOD);
            this.statusTab.Controls.Add(this.panelUODlogo);
            this.statusTab.Controls.Add(this.labelStatus);
            this.statusTab.Controls.Add(this.labelFeatures);
            this.statusTab.Location = new System.Drawing.Point(4, 40);
            this.statusTab.Name = "statusTab";
            this.statusTab.Size = new System.Drawing.Size(666, 366);
            this.statusTab.TabIndex = 9;
            this.statusTab.Text = "Help / Status";
            // 
            // panelLogo
            // 
            this.panelLogo.BackgroundImage = global::Assistant.Properties.Resources.razor_enhanced_png;
            this.panelLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelLogo.Location = new System.Drawing.Point(250, 155);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(48, 49);
            this.panelLogo.TabIndex = 7;
            // 
            // razorButtonWiki
            // 
            this.razorButtonWiki.ColorTable = office2010BlueTheme1;
            this.razorButtonWiki.Location = new System.Drawing.Point(304, 164);
            this.razorButtonWiki.Name = "razorButtonWiki";
            this.razorButtonWiki.Size = new System.Drawing.Size(145, 28);
            this.razorButtonWiki.TabIndex = 6;
            this.razorButtonWiki.Text = "Razor Enhanced wiki";
            this.razorButtonWiki.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonWiki.UseVisualStyleBackColor = true;
            this.razorButtonWiki.Click += new System.EventHandler(this.razorButtonWiki_Click);
            // 
            // razorButtonCreateUODAccount
            // 
            this.razorButtonCreateUODAccount.ColorTable = office2010BlueTheme1;
            this.razorButtonCreateUODAccount.Location = new System.Drawing.Point(250, 60);
            this.razorButtonCreateUODAccount.Name = "razorButtonCreateUODAccount";
            this.razorButtonCreateUODAccount.Size = new System.Drawing.Size(199, 28);
            this.razorButtonCreateUODAccount.TabIndex = 5;
            this.razorButtonCreateUODAccount.Text = "create your UOD account";
            this.razorButtonCreateUODAccount.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonCreateUODAccount.UseVisualStyleBackColor = true;
            this.razorButtonCreateUODAccount.Click += new System.EventHandler(this.razorButtonCreateUODAccount_Click);
            // 
            // labelUOD
            // 
            this.labelUOD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUOD.Location = new System.Drawing.Point(5, 175);
            this.labelUOD.Name = "labelUOD";
            this.labelUOD.Size = new System.Drawing.Size(213, 64);
            this.labelUOD.TabIndex = 4;
            this.labelUOD.Text = "To support the development of the Razor Enhanced project,  you can visit UODreams" +
    " shard and stay with us! You are welcome!";
            this.labelUOD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // razorButtonVisitUOD
            // 
            this.razorButtonVisitUOD.ColorTable = office2010BlueTheme1;
            this.razorButtonVisitUOD.Location = new System.Drawing.Point(250, 26);
            this.razorButtonVisitUOD.Name = "razorButtonVisitUOD";
            this.razorButtonVisitUOD.Size = new System.Drawing.Size(199, 28);
            this.razorButtonVisitUOD.TabIndex = 3;
            this.razorButtonVisitUOD.Text = "visit www.uodreams.com";
            this.razorButtonVisitUOD.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonVisitUOD.UseVisualStyleBackColor = true;
            this.razorButtonVisitUOD.Click += new System.EventHandler(this.razorButtonVisitUOD_Click);
            // 
            // panelUODlogo
            // 
            this.panelUODlogo.BackgroundImage = global::Assistant.Properties.Resources.uod_logo;
            this.panelUODlogo.Location = new System.Drawing.Point(8, 9);
            this.panelUODlogo.Name = "panelUODlogo";
            this.panelUODlogo.Size = new System.Drawing.Size(213, 163);
            this.panelUODlogo.TabIndex = 2;
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Location = new System.Drawing.Point(483, 9);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(175, 268);
            this.labelStatus.TabIndex = 1;
            // 
            // labelFeatures
            // 
            this.labelFeatures.Location = new System.Drawing.Point(8, 291);
            this.labelFeatures.Name = "labelFeatures";
            this.labelFeatures.Size = new System.Drawing.Size(650, 70);
            this.labelFeatures.TabIndex = 0;
            // 
            // scriptingTab
            // 
            this.scriptingTab.BackColor = System.Drawing.SystemColors.Control;
            this.scriptingTab.Controls.Add(this.razorButtonEdit);
            this.scriptingTab.Controls.Add(this.razorCheckBoxAuto);
            this.scriptingTab.Controls.Add(this.razorButtonUp);
            this.scriptingTab.Controls.Add(this.razorButtonDown);
            this.scriptingTab.Controls.Add(this.dataGridViewScripting);
            this.scriptingTab.Controls.Add(this.xButton3);
            this.scriptingTab.Controls.Add(this.xButton2);
            this.scriptingTab.Location = new System.Drawing.Point(4, 40);
            this.scriptingTab.Name = "scriptingTab";
            this.scriptingTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptingTab.Size = new System.Drawing.Size(666, 366);
            this.scriptingTab.TabIndex = 12;
            this.scriptingTab.Text = "Enhanced Scripting";
            // 
            // razorButtonEdit
            // 
            this.razorButtonEdit.ColorTable = office2010BlueTheme1;
            this.razorButtonEdit.Location = new System.Drawing.Point(442, 338);
            this.razorButtonEdit.Name = "razorButtonEdit";
            this.razorButtonEdit.Size = new System.Drawing.Size(52, 20);
            this.razorButtonEdit.TabIndex = 20;
            this.razorButtonEdit.Text = "Edit";
            this.razorButtonEdit.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonEdit.UseVisualStyleBackColor = true;
            this.razorButtonEdit.Click += new System.EventHandler(this.razorButtonEdit_Click);
            // 
            // razorCheckBoxAuto
            // 
            this.razorCheckBoxAuto.Location = new System.Drawing.Point(500, 338);
            this.razorCheckBoxAuto.Name = "razorCheckBoxAuto";
            this.razorCheckBoxAuto.Size = new System.Drawing.Size(78, 20);
            this.razorCheckBoxAuto.TabIndex = 19;
            this.razorCheckBoxAuto.Text = "Auto Mode";
            this.razorCheckBoxAuto.UseVisualStyleBackColor = true;
            this.razorCheckBoxAuto.CheckedChanged += new System.EventHandler(this.razorCheckBoxAuto_CheckedChanged);
            // 
            // razorButtonUp
            // 
            this.razorButtonUp.ColorTable = office2010BlueTheme1;
            this.razorButtonUp.Location = new System.Drawing.Point(361, 338);
            this.razorButtonUp.Name = "razorButtonUp";
            this.razorButtonUp.Size = new System.Drawing.Size(75, 20);
            this.razorButtonUp.TabIndex = 18;
            this.razorButtonUp.Text = "Move Up";
            this.razorButtonUp.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonUp.UseVisualStyleBackColor = true;
            this.razorButtonUp.Click += new System.EventHandler(this.razorButtonUp_Click);
            // 
            // razorButtonDown
            // 
            this.razorButtonDown.ColorTable = office2010BlueTheme1;
            this.razorButtonDown.Location = new System.Drawing.Point(274, 338);
            this.razorButtonDown.Name = "razorButtonDown";
            this.razorButtonDown.Size = new System.Drawing.Size(81, 19);
            this.razorButtonDown.TabIndex = 17;
            this.razorButtonDown.Text = "Move Down";
            this.razorButtonDown.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonDown.UseVisualStyleBackColor = true;
            this.razorButtonDown.Click += new System.EventHandler(this.razorButtonDown_Click);
            // 
            // dataGridViewScripting
            // 
            this.dataGridViewScripting.AllowUserToAddRows = false;
            this.dataGridViewScripting.AllowUserToDeleteRows = false;
            this.dataGridViewScripting.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewScripting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScripting.Location = new System.Drawing.Point(8, 6);
            this.dataGridViewScripting.Name = "dataGridViewScripting";
            this.dataGridViewScripting.Size = new System.Drawing.Size(650, 326);
            this.dataGridViewScripting.TabIndex = 16;
            this.dataGridViewScripting.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewScripting_CellContentClick);
            // 
            // xButton3
            // 
            this.xButton3.ColorTable = office2010BlueTheme1;
            this.xButton3.Location = new System.Drawing.Point(161, 338);
            this.xButton3.Name = "xButton3";
            this.xButton3.Size = new System.Drawing.Size(107, 20);
            this.xButton3.TabIndex = 15;
            this.xButton3.Text = "Remove Selected";
            this.xButton3.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.xButton3.Click += new System.EventHandler(this.xButton3_Click);
            // 
            // xButton2
            // 
            this.xButton2.ColorTable = office2010BlueTheme1;
            this.xButton2.Location = new System.Drawing.Point(70, 338);
            this.xButton2.Name = "xButton2";
            this.xButton2.Size = new System.Drawing.Size(85, 20);
            this.xButton2.TabIndex = 14;
            this.xButton2.Text = "Open Script";
            this.xButton2.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.xButton2.Click += new System.EventHandler(this.xButton2_Click);
            // 
            // EnhancedAgent
            // 
            this.EnhancedAgent.Controls.Add(this.tabControl1);
            this.EnhancedAgent.Location = new System.Drawing.Point(4, 40);
            this.EnhancedAgent.Name = "EnhancedAgent";
            this.EnhancedAgent.Padding = new System.Windows.Forms.Padding(3);
            this.EnhancedAgent.Size = new System.Drawing.Size(666, 366);
            this.EnhancedAgent.TabIndex = 14;
            this.EnhancedAgent.Text = "Enhanced Agents";
            this.EnhancedAgent.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.eautoloot);
            this.tabControl1.Controls.Add(this.escavenger);
            this.tabControl1.Controls.Add(this.Organizer);
            this.tabControl1.Controls.Add(this.VendorBuy);
            this.tabControl1.Controls.Add(this.VendorSell);
            this.tabControl1.Controls.Add(this.Dress);
            this.tabControl1.Controls.Add(this.friends);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(667, 367);
            this.tabControl1.TabIndex = 0;
            // 
            // eautoloot
            // 
            this.eautoloot.Controls.Add(this.razorButtonResetIgnore);
            this.eautoloot.Controls.Add(this.label21);
            this.eautoloot.Controls.Add(this.autoLootTextBoxDelay);
            this.eautoloot.Controls.Add(this.autoLootButtonRemoveList);
            this.eautoloot.Controls.Add(this.autolootButtonAddList);
            this.eautoloot.Controls.Add(this.autoLootButtonListImport);
            this.eautoloot.Controls.Add(this.autolootListSelect);
            this.eautoloot.Controls.Add(this.autoLootButtonListExport);
            this.eautoloot.Controls.Add(this.label20);
            this.eautoloot.Controls.Add(this.groupBox13);
            this.eautoloot.Controls.Add(this.autolootContainerLabel);
            this.eautoloot.Controls.Add(this.groupBox11);
            this.eautoloot.Controls.Add(this.autolootContainerButton);
            this.eautoloot.Controls.Add(this.autoLootCheckBox);
            this.eautoloot.Controls.Add(this.autolootlistView);
            this.eautoloot.Location = new System.Drawing.Point(4, 22);
            this.eautoloot.Name = "eautoloot";
            this.eautoloot.Padding = new System.Windows.Forms.Padding(3);
            this.eautoloot.Size = new System.Drawing.Size(659, 341);
            this.eautoloot.TabIndex = 0;
            this.eautoloot.Text = "Autoloot";
            this.eautoloot.UseVisualStyleBackColor = true;
            // 
            // razorButtonResetIgnore
            // 
            this.razorButtonResetIgnore.ColorTable = office2010BlueTheme1;
            this.razorButtonResetIgnore.Location = new System.Drawing.Point(558, 273);
            this.razorButtonResetIgnore.Name = "razorButtonResetIgnore";
            this.razorButtonResetIgnore.Size = new System.Drawing.Size(90, 20);
            this.razorButtonResetIgnore.TabIndex = 60;
            this.razorButtonResetIgnore.Text = "Reset Ignore";
            this.razorButtonResetIgnore.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButtonResetIgnore.Click += new System.EventHandler(this.razorButtonResetIgnore_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(436, 61);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(80, 13);
            this.label21.TabIndex = 59;
            this.label21.Text = "Loot Delay (ms)";
            // 
            // autoLootTextBoxDelay
            // 
            this.autoLootTextBoxDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoLootTextBoxDelay.BackColor = System.Drawing.Color.White;
            this.autoLootTextBoxDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoLootTextBoxDelay.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.autoLootTextBoxDelay.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.autoLootTextBoxDelay.Location = new System.Drawing.Point(383, 58);
            this.autoLootTextBoxDelay.Name = "autoLootTextBoxDelay";
            this.autoLootTextBoxDelay.Size = new System.Drawing.Size(45, 20);
            this.autoLootTextBoxDelay.TabIndex = 58;
            this.autoLootTextBoxDelay.TextChanged += new System.EventHandler(this.autoLootTextBoxDelay_TextChanged);
            // 
            // autoLootButtonRemoveList
            // 
            this.autoLootButtonRemoveList.ColorTable = office2010BlueTheme1;
            this.autoLootButtonRemoveList.Location = new System.Drawing.Point(366, 14);
            this.autoLootButtonRemoveList.Name = "autoLootButtonRemoveList";
            this.autoLootButtonRemoveList.Size = new System.Drawing.Size(90, 20);
            this.autoLootButtonRemoveList.TabIndex = 57;
            this.autoLootButtonRemoveList.Text = "Remove";
            this.autoLootButtonRemoveList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autoLootButtonRemoveList.Click += new System.EventHandler(this.autoLootButtonRemoveList_Click);
            // 
            // autolootButtonAddList
            // 
            this.autolootButtonAddList.ColorTable = office2010BlueTheme1;
            this.autolootButtonAddList.Location = new System.Drawing.Point(270, 14);
            this.autolootButtonAddList.Name = "autolootButtonAddList";
            this.autolootButtonAddList.Size = new System.Drawing.Size(90, 20);
            this.autolootButtonAddList.TabIndex = 56;
            this.autolootButtonAddList.Text = "Add";
            this.autolootButtonAddList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootButtonAddList.Click += new System.EventHandler(this.autoLootButtonAddList_Click);
            // 
            // autoLootButtonListImport
            // 
            this.autoLootButtonListImport.ColorTable = office2010BlueTheme1;
            this.autoLootButtonListImport.Location = new System.Drawing.Point(462, 14);
            this.autoLootButtonListImport.Name = "autoLootButtonListImport";
            this.autoLootButtonListImport.Size = new System.Drawing.Size(90, 20);
            this.autoLootButtonListImport.TabIndex = 49;
            this.autoLootButtonListImport.Text = "Import";
            this.autoLootButtonListImport.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autoLootButtonListImport.Click += new System.EventHandler(this.autoLootImport_Click);
            // 
            // autolootListSelect
            // 
            this.autolootListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autolootListSelect.FormattingEnabled = true;
            this.autolootListSelect.Location = new System.Drawing.Point(78, 12);
            this.autolootListSelect.Name = "autolootListSelect";
            this.autolootListSelect.Size = new System.Drawing.Size(183, 24);
            this.autolootListSelect.TabIndex = 55;
            this.autolootListSelect.SelectedIndexChanged += new System.EventHandler(this.autoLootListSelect_SelectedIndexChanged);
            // 
            // autoLootButtonListExport
            // 
            this.autoLootButtonListExport.ColorTable = office2010BlueTheme1;
            this.autoLootButtonListExport.Location = new System.Drawing.Point(558, 14);
            this.autoLootButtonListExport.Name = "autoLootButtonListExport";
            this.autoLootButtonListExport.Size = new System.Drawing.Size(90, 20);
            this.autoLootButtonListExport.TabIndex = 48;
            this.autoLootButtonListExport.Text = "Export";
            this.autoLootButtonListExport.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autoLootButtonListExport.Click += new System.EventHandler(this.autoLootButtonListExport_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 18);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(68, 13);
            this.label20.TabIndex = 54;
            this.label20.Text = "Autoloot List:";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.autolootLogBox);
            this.groupBox13.Location = new System.Drawing.Point(267, 84);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(278, 251);
            this.groupBox13.TabIndex = 53;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Autoloot Log";
            // 
            // autolootLogBox
            // 
            this.autolootLogBox.FormattingEnabled = true;
            this.autolootLogBox.Location = new System.Drawing.Point(7, 18);
            this.autolootLogBox.Name = "autolootLogBox";
            this.autolootLogBox.Size = new System.Drawing.Size(265, 225);
            this.autolootLogBox.TabIndex = 0;
            // 
            // autolootContainerLabel
            // 
            this.autolootContainerLabel.Location = new System.Drawing.Point(569, 84);
            this.autolootContainerLabel.Name = "autolootContainerLabel";
            this.autolootContainerLabel.Size = new System.Drawing.Size(82, 19);
            this.autolootContainerLabel.TabIndex = 50;
            this.autolootContainerLabel.Text = "0x00000000";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.autolootItemPropsB);
            this.groupBox11.Controls.Add(this.autolootItemEditB);
            this.groupBox11.Controls.Add(this.autolootAddItemBTarget);
            this.groupBox11.Controls.Add(this.autolootRemoveItemB);
            this.groupBox11.Controls.Add(this.autolootAddItemBManual);
            this.groupBox11.Location = new System.Drawing.Point(553, 104);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(100, 147);
            this.groupBox11.TabIndex = 51;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Loot List";
            // 
            // autolootItemPropsB
            // 
            this.autolootItemPropsB.ColorTable = office2010BlueTheme1;
            this.autolootItemPropsB.Location = new System.Drawing.Point(5, 94);
            this.autolootItemPropsB.Name = "autolootItemPropsB";
            this.autolootItemPropsB.Size = new System.Drawing.Size(90, 20);
            this.autolootItemPropsB.TabIndex = 49;
            this.autolootItemPropsB.Text = "Edit Props";
            this.autolootItemPropsB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootItemPropsB.Click += new System.EventHandler(this.autoLootItemProps_Click);
            // 
            // autolootItemEditB
            // 
            this.autolootItemEditB.ColorTable = office2010BlueTheme1;
            this.autolootItemEditB.Location = new System.Drawing.Point(5, 68);
            this.autolootItemEditB.Name = "autolootItemEditB";
            this.autolootItemEditB.Size = new System.Drawing.Size(90, 20);
            this.autolootItemEditB.TabIndex = 48;
            this.autolootItemEditB.Text = "Edit";
            this.autolootItemEditB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootItemEditB.Click += new System.EventHandler(this.autoLootItemEdit_Click);
            // 
            // autolootAddItemBTarget
            // 
            this.autolootAddItemBTarget.ColorTable = office2010BlueTheme1;
            this.autolootAddItemBTarget.Location = new System.Drawing.Point(5, 43);
            this.autolootAddItemBTarget.Name = "autolootAddItemBTarget";
            this.autolootAddItemBTarget.Size = new System.Drawing.Size(90, 20);
            this.autolootAddItemBTarget.TabIndex = 47;
            this.autolootAddItemBTarget.Text = "Add Target";
            this.autolootAddItemBTarget.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootAddItemBTarget.Click += new System.EventHandler(this.autoLootAddItemTarget_Click);
            // 
            // autolootRemoveItemB
            // 
            this.autolootRemoveItemB.ColorTable = office2010BlueTheme1;
            this.autolootRemoveItemB.Location = new System.Drawing.Point(5, 119);
            this.autolootRemoveItemB.Name = "autolootRemoveItemB";
            this.autolootRemoveItemB.Size = new System.Drawing.Size(90, 20);
            this.autolootRemoveItemB.TabIndex = 46;
            this.autolootRemoveItemB.Text = "Remove";
            this.autolootRemoveItemB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootRemoveItemB.Click += new System.EventHandler(this.autoLootRemoveItem_Click);
            // 
            // autolootAddItemBManual
            // 
            this.autolootAddItemBManual.ColorTable = office2010BlueTheme1;
            this.autolootAddItemBManual.Location = new System.Drawing.Point(5, 18);
            this.autolootAddItemBManual.Name = "autolootAddItemBManual";
            this.autolootAddItemBManual.Size = new System.Drawing.Size(90, 20);
            this.autolootAddItemBManual.TabIndex = 45;
            this.autolootAddItemBManual.Text = "Add Manual";
            this.autolootAddItemBManual.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootAddItemBManual.Click += new System.EventHandler(this.autoLootAddItemManual_Click);
            // 
            // autolootContainerButton
            // 
            this.autolootContainerButton.ColorTable = office2010BlueTheme1;
            this.autolootContainerButton.Location = new System.Drawing.Point(545, 58);
            this.autolootContainerButton.Name = "autolootContainerButton";
            this.autolootContainerButton.Size = new System.Drawing.Size(103, 20);
            this.autolootContainerButton.TabIndex = 49;
            this.autolootContainerButton.Text = "Target Container";
            this.autolootContainerButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.autolootContainerButton.Click += new System.EventHandler(this.autolootContainerButton_Click);
            // 
            // autoLootCheckBox
            // 
            this.autoLootCheckBox.Location = new System.Drawing.Point(274, 58);
            this.autoLootCheckBox.Name = "autoLootCheckBox";
            this.autoLootCheckBox.Size = new System.Drawing.Size(103, 22);
            this.autoLootCheckBox.TabIndex = 48;
            this.autoLootCheckBox.Text = "Enable autoloot";
            this.autoLootCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.autoLootCheckBox.CheckedChanged += new System.EventHandler(this.autoLootEnable_CheckedChanged);
            // 
            // autolootlistView
            // 
            this.autolootlistView.CheckBoxes = true;
            this.autolootlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.ColumnHeader3});
            this.autolootlistView.FullRowSelect = true;
            this.autolootlistView.GridLines = true;
            this.autolootlistView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.autolootlistView.HideSelection = false;
            this.autolootlistView.LabelWrap = false;
            this.autolootlistView.Location = new System.Drawing.Point(6, 51);
            this.autolootlistView.MultiSelect = false;
            this.autolootlistView.Name = "autolootlistView";
            this.autolootlistView.Size = new System.Drawing.Size(255, 284);
            this.autolootlistView.TabIndex = 47;
            this.autolootlistView.UseCompatibleStateImageBehavior = false;
            this.autolootlistView.View = System.Windows.Forms.View.Details;
            this.autolootlistView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.autolootlistView_ItemChecked);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "X";
            this.columnHeader4.Width = 22;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item Name";
            this.columnHeader1.Width = 105;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Graphics";
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Color";
            // 
            // escavenger
            // 
            this.escavenger.Controls.Add(this.groupBox14);
            this.escavenger.Controls.Add(this.groupBox12);
            this.escavenger.Controls.Add(this.label23);
            this.escavenger.Controls.Add(this.scavengerDragDelay);
            this.escavenger.Controls.Add(this.scavengerContainerLabel);
            this.escavenger.Controls.Add(this.scavengerButtonSetContainer);
            this.escavenger.Controls.Add(this.scavengerCheckBox);
            this.escavenger.Controls.Add(this.scavengerListView);
            this.escavenger.Controls.Add(this.scavengerButtonRemoveList);
            this.escavenger.Controls.Add(this.scavengerButtonAddList);
            this.escavenger.Controls.Add(this.scavengerButtonImport);
            this.escavenger.Controls.Add(this.scavengerListSelect);
            this.escavenger.Controls.Add(this.scavengerButtonExport);
            this.escavenger.Controls.Add(this.label22);
            this.escavenger.Location = new System.Drawing.Point(4, 22);
            this.escavenger.Name = "escavenger";
            this.escavenger.Padding = new System.Windows.Forms.Padding(3);
            this.escavenger.Size = new System.Drawing.Size(659, 341);
            this.escavenger.TabIndex = 1;
            this.escavenger.Text = "Scavenger";
            this.escavenger.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.scavengerButtonEditProps);
            this.groupBox14.Controls.Add(this.scavengerButtonEditItem);
            this.groupBox14.Controls.Add(this.scavengerButtonAddTarget);
            this.groupBox14.Controls.Add(this.scavengerButtonRemoveItem);
            this.groupBox14.Controls.Add(this.scavengerButtonAddManual);
            this.groupBox14.Location = new System.Drawing.Point(553, 104);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(100, 147);
            this.groupBox14.TabIndex = 71;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Item List";
            // 
            // scavengerButtonEditProps
            // 
            this.scavengerButtonEditProps.ColorTable = office2010BlueTheme1;
            this.scavengerButtonEditProps.Location = new System.Drawing.Point(5, 94);
            this.scavengerButtonEditProps.Name = "scavengerButtonEditProps";
            this.scavengerButtonEditProps.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonEditProps.TabIndex = 49;
            this.scavengerButtonEditProps.Text = "Edit Props";
            this.scavengerButtonEditProps.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonEditProps.Click += new System.EventHandler(this.scavengerEditProps_Click);
            // 
            // scavengerButtonEditItem
            // 
            this.scavengerButtonEditItem.ColorTable = office2010BlueTheme1;
            this.scavengerButtonEditItem.Location = new System.Drawing.Point(5, 68);
            this.scavengerButtonEditItem.Name = "scavengerButtonEditItem";
            this.scavengerButtonEditItem.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonEditItem.TabIndex = 48;
            this.scavengerButtonEditItem.Text = "Edit";
            this.scavengerButtonEditItem.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonEditItem.Click += new System.EventHandler(this.scavengerEditItem_Click);
            // 
            // scavengerButtonAddTarget
            // 
            this.scavengerButtonAddTarget.ColorTable = office2010BlueTheme1;
            this.scavengerButtonAddTarget.Location = new System.Drawing.Point(5, 43);
            this.scavengerButtonAddTarget.Name = "scavengerButtonAddTarget";
            this.scavengerButtonAddTarget.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonAddTarget.TabIndex = 47;
            this.scavengerButtonAddTarget.Text = "Add Target";
            this.scavengerButtonAddTarget.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonAddTarget.Click += new System.EventHandler(this.scavengerAddItemTarget_Click);
            // 
            // scavengerButtonRemoveItem
            // 
            this.scavengerButtonRemoveItem.ColorTable = office2010BlueTheme1;
            this.scavengerButtonRemoveItem.Location = new System.Drawing.Point(5, 119);
            this.scavengerButtonRemoveItem.Name = "scavengerButtonRemoveItem";
            this.scavengerButtonRemoveItem.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonRemoveItem.TabIndex = 46;
            this.scavengerButtonRemoveItem.Text = "Remove";
            this.scavengerButtonRemoveItem.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonRemoveItem.Click += new System.EventHandler(this.scavengerRemoveItem_Click);
            // 
            // scavengerButtonAddManual
            // 
            this.scavengerButtonAddManual.ColorTable = office2010BlueTheme1;
            this.scavengerButtonAddManual.Location = new System.Drawing.Point(5, 18);
            this.scavengerButtonAddManual.Name = "scavengerButtonAddManual";
            this.scavengerButtonAddManual.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonAddManual.TabIndex = 45;
            this.scavengerButtonAddManual.Text = "Add Manual";
            this.scavengerButtonAddManual.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonAddManual.Click += new System.EventHandler(this.scavengerAdItemdManual_Click);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.scavengerLogBox);
            this.groupBox12.Location = new System.Drawing.Point(267, 84);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(278, 251);
            this.groupBox12.TabIndex = 70;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Scavenger Log";
            // 
            // scavengerLogBox
            // 
            this.scavengerLogBox.FormattingEnabled = true;
            this.scavengerLogBox.Location = new System.Drawing.Point(7, 18);
            this.scavengerLogBox.Name = "scavengerLogBox";
            this.scavengerLogBox.Size = new System.Drawing.Size(265, 225);
            this.scavengerLogBox.TabIndex = 0;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(446, 61);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(82, 13);
            this.label23.TabIndex = 69;
            this.label23.Text = "Drag Delay (ms)";
            // 
            // scavengerDragDelay
            // 
            this.scavengerDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scavengerDragDelay.BackColor = System.Drawing.Color.White;
            this.scavengerDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scavengerDragDelay.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.scavengerDragDelay.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.scavengerDragDelay.Location = new System.Drawing.Point(396, 58);
            this.scavengerDragDelay.Name = "scavengerDragDelay";
            this.scavengerDragDelay.Size = new System.Drawing.Size(45, 20);
            this.scavengerDragDelay.TabIndex = 68;
            this.scavengerDragDelay.TextChanged += new System.EventHandler(this.scavengerDragDelay_TextChanged);
            // 
            // scavengerContainerLabel
            // 
            this.scavengerContainerLabel.Location = new System.Drawing.Point(572, 82);
            this.scavengerContainerLabel.Name = "scavengerContainerLabel";
            this.scavengerContainerLabel.Size = new System.Drawing.Size(82, 19);
            this.scavengerContainerLabel.TabIndex = 67;
            this.scavengerContainerLabel.Text = "0x00000000";
            // 
            // scavengerButtonSetContainer
            // 
            this.scavengerButtonSetContainer.ColorTable = office2010BlueTheme1;
            this.scavengerButtonSetContainer.Location = new System.Drawing.Point(552, 56);
            this.scavengerButtonSetContainer.Name = "scavengerButtonSetContainer";
            this.scavengerButtonSetContainer.Size = new System.Drawing.Size(96, 20);
            this.scavengerButtonSetContainer.TabIndex = 66;
            this.scavengerButtonSetContainer.Text = "Target Container";
            this.scavengerButtonSetContainer.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonSetContainer.Click += new System.EventHandler(this.scavengerSetContainer_Click);
            // 
            // scavengerCheckBox
            // 
            this.scavengerCheckBox.Location = new System.Drawing.Point(275, 56);
            this.scavengerCheckBox.Name = "scavengerCheckBox";
            this.scavengerCheckBox.Size = new System.Drawing.Size(116, 22);
            this.scavengerCheckBox.TabIndex = 65;
            this.scavengerCheckBox.Text = "Enable scavenger";
            this.scavengerCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.scavengerCheckBox.CheckedChanged += new System.EventHandler(this.scavengerEnableCheck_CheckedChanged);
            // 
            // scavengerListView
            // 
            this.scavengerListView.CheckBoxes = true;
            this.scavengerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.scavengerListView.FullRowSelect = true;
            this.scavengerListView.GridLines = true;
            this.scavengerListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.scavengerListView.HideSelection = false;
            this.scavengerListView.LabelWrap = false;
            this.scavengerListView.Location = new System.Drawing.Point(6, 51);
            this.scavengerListView.MultiSelect = false;
            this.scavengerListView.Name = "scavengerListView";
            this.scavengerListView.Size = new System.Drawing.Size(255, 284);
            this.scavengerListView.TabIndex = 64;
            this.scavengerListView.UseCompatibleStateImageBehavior = false;
            this.scavengerListView.View = System.Windows.Forms.View.Details;
            this.scavengerListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.scavengerListView_ItemChecked);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "X";
            this.columnHeader5.Width = 22;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Item Name";
            this.columnHeader6.Width = 105;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Graphics";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Color";
            // 
            // scavengerButtonRemoveList
            // 
            this.scavengerButtonRemoveList.ColorTable = office2010BlueTheme1;
            this.scavengerButtonRemoveList.Location = new System.Drawing.Point(371, 14);
            this.scavengerButtonRemoveList.Name = "scavengerButtonRemoveList";
            this.scavengerButtonRemoveList.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonRemoveList.TabIndex = 63;
            this.scavengerButtonRemoveList.Text = "Remove";
            this.scavengerButtonRemoveList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonRemoveList.Click += new System.EventHandler(this.scavengerRemoveList_Click);
            // 
            // scavengerButtonAddList
            // 
            this.scavengerButtonAddList.ColorTable = office2010BlueTheme1;
            this.scavengerButtonAddList.Location = new System.Drawing.Point(275, 14);
            this.scavengerButtonAddList.Name = "scavengerButtonAddList";
            this.scavengerButtonAddList.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonAddList.TabIndex = 62;
            this.scavengerButtonAddList.Text = "Add";
            this.scavengerButtonAddList.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonAddList.Click += new System.EventHandler(this.scavengerAddList_Click);
            // 
            // scavengerButtonImport
            // 
            this.scavengerButtonImport.ColorTable = office2010BlueTheme1;
            this.scavengerButtonImport.Location = new System.Drawing.Point(467, 14);
            this.scavengerButtonImport.Name = "scavengerButtonImport";
            this.scavengerButtonImport.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonImport.TabIndex = 59;
            this.scavengerButtonImport.Text = "Import";
            this.scavengerButtonImport.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonImport.Click += new System.EventHandler(this.scavengerButtonImport_Click);
            // 
            // scavengerListSelect
            // 
            this.scavengerListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scavengerListSelect.FormattingEnabled = true;
            this.scavengerListSelect.Location = new System.Drawing.Point(86, 12);
            this.scavengerListSelect.Name = "scavengerListSelect";
            this.scavengerListSelect.Size = new System.Drawing.Size(183, 24);
            this.scavengerListSelect.TabIndex = 61;
            this.scavengerListSelect.SelectedIndexChanged += new System.EventHandler(this.scavengertListSelect_SelectedIndexChanged);
            // 
            // scavengerButtonExport
            // 
            this.scavengerButtonExport.ColorTable = office2010BlueTheme1;
            this.scavengerButtonExport.Location = new System.Drawing.Point(563, 14);
            this.scavengerButtonExport.Name = "scavengerButtonExport";
            this.scavengerButtonExport.Size = new System.Drawing.Size(90, 20);
            this.scavengerButtonExport.TabIndex = 58;
            this.scavengerButtonExport.Text = "Export";
            this.scavengerButtonExport.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.scavengerButtonExport.Click += new System.EventHandler(this.scavengerButtonExport_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 18);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(81, 13);
            this.label22.TabIndex = 60;
            this.label22.Text = "Scavenger List:";
            // 
            // Organizer
            // 
            this.Organizer.Controls.Add(this.organizerStopButton);
            this.Organizer.Controls.Add(this.organizerExecuteButton);
            this.Organizer.Controls.Add(this.groupBox16);
            this.Organizer.Controls.Add(this.label27);
            this.Organizer.Controls.Add(this.organizerDragDelay);
            this.Organizer.Controls.Add(this.organizerDestinationLabel);
            this.Organizer.Controls.Add(this.organizerSetDestinationB);
            this.Organizer.Controls.Add(this.organizerSourceLabel);
            this.Organizer.Controls.Add(this.groupBox15);
            this.Organizer.Controls.Add(this.organizerSetSourceB);
            this.Organizer.Controls.Add(this.organizerListView);
            this.Organizer.Controls.Add(this.organizerRemoveListB);
            this.Organizer.Controls.Add(this.organizerAddListB);
            this.Organizer.Controls.Add(this.organizerImportListB);
            this.Organizer.Controls.Add(this.organizerListSelect);
            this.Organizer.Controls.Add(this.organizerExportListB);
            this.Organizer.Controls.Add(this.label24);
            this.Organizer.Location = new System.Drawing.Point(4, 22);
            this.Organizer.Name = "Organizer";
            this.Organizer.Padding = new System.Windows.Forms.Padding(3);
            this.Organizer.Size = new System.Drawing.Size(659, 341);
            this.Organizer.TabIndex = 2;
            this.Organizer.Text = "Organizer";
            this.Organizer.UseVisualStyleBackColor = true;
            // 
            // organizerStopButton
            // 
            this.organizerStopButton.ColorTable = office2010BlueTheme1;
            this.organizerStopButton.Location = new System.Drawing.Point(334, 58);
            this.organizerStopButton.Name = "organizerStopButton";
            this.organizerStopButton.Size = new System.Drawing.Size(61, 20);
            this.organizerStopButton.TabIndex = 75;
            this.organizerStopButton.Text = "Stop";
            this.organizerStopButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerStopButton.Click += new System.EventHandler(this.organizerStop_Click);
            // 
            // organizerExecuteButton
            // 
            this.organizerExecuteButton.ColorTable = office2010BlueTheme1;
            this.organizerExecuteButton.Location = new System.Drawing.Point(268, 58);
            this.organizerExecuteButton.Name = "organizerExecuteButton";
            this.organizerExecuteButton.Size = new System.Drawing.Size(61, 20);
            this.organizerExecuteButton.TabIndex = 74;
            this.organizerExecuteButton.Text = "Execute";
            this.organizerExecuteButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerExecuteButton.Click += new System.EventHandler(this.organizerExecute_Click);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.organizerLogBox);
            this.groupBox16.Location = new System.Drawing.Point(267, 84);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(278, 251);
            this.groupBox16.TabIndex = 73;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Organizer Log";
            // 
            // organizerLogBox
            // 
            this.organizerLogBox.FormattingEnabled = true;
            this.organizerLogBox.Location = new System.Drawing.Point(7, 18);
            this.organizerLogBox.Name = "organizerLogBox";
            this.organizerLogBox.Size = new System.Drawing.Size(265, 225);
            this.organizerLogBox.TabIndex = 0;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(446, 61);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(105, 13);
            this.label27.TabIndex = 72;
            this.label27.Text = "Drag Item Delay (ms)";
            // 
            // organizerDragDelay
            // 
            this.organizerDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.organizerDragDelay.BackColor = System.Drawing.Color.White;
            this.organizerDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.organizerDragDelay.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.organizerDragDelay.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.organizerDragDelay.Location = new System.Drawing.Point(400, 58);
            this.organizerDragDelay.Name = "organizerDragDelay";
            this.organizerDragDelay.Size = new System.Drawing.Size(45, 20);
            this.organizerDragDelay.TabIndex = 71;
            this.organizerDragDelay.TextChanged += new System.EventHandler(this.organizerDragDelay_TextChanged);
            // 
            // organizerDestinationLabel
            // 
            this.organizerDestinationLabel.Location = new System.Drawing.Point(564, 126);
            this.organizerDestinationLabel.Name = "organizerDestinationLabel";
            this.organizerDestinationLabel.Size = new System.Drawing.Size(82, 19);
            this.organizerDestinationLabel.TabIndex = 70;
            this.organizerDestinationLabel.Text = "0x00000000";
            // 
            // organizerSetDestinationB
            // 
            this.organizerSetDestinationB.ColorTable = office2010BlueTheme1;
            this.organizerSetDestinationB.Location = new System.Drawing.Point(558, 104);
            this.organizerSetDestinationB.Name = "organizerSetDestinationB";
            this.organizerSetDestinationB.Size = new System.Drawing.Size(90, 20);
            this.organizerSetDestinationB.TabIndex = 69;
            this.organizerSetDestinationB.Text = "Destination Cont";
            this.organizerSetDestinationB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerSetDestinationB.Click += new System.EventHandler(this.organizerSetDestination_Click);
            // 
            // organizerSourceLabel
            // 
            this.organizerSourceLabel.Location = new System.Drawing.Point(564, 82);
            this.organizerSourceLabel.Name = "organizerSourceLabel";
            this.organizerSourceLabel.Size = new System.Drawing.Size(82, 19);
            this.organizerSourceLabel.TabIndex = 67;
            this.organizerSourceLabel.Text = "0x00000000";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.organizerEditB);
            this.groupBox15.Controls.Add(this.organizerAddTargetB);
            this.groupBox15.Controls.Add(this.organizerRemoveB);
            this.groupBox15.Controls.Add(this.organizerAddManualB);
            this.groupBox15.Location = new System.Drawing.Point(553, 158);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(100, 123);
            this.groupBox15.TabIndex = 68;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Item List";
            // 
            // organizerEditB
            // 
            this.organizerEditB.ColorTable = office2010BlueTheme1;
            this.organizerEditB.Location = new System.Drawing.Point(5, 68);
            this.organizerEditB.Name = "organizerEditB";
            this.organizerEditB.Size = new System.Drawing.Size(90, 20);
            this.organizerEditB.TabIndex = 48;
            this.organizerEditB.Text = "Edit";
            this.organizerEditB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerEditB.Click += new System.EventHandler(this.organizerEdit_Click);
            // 
            // organizerAddTargetB
            // 
            this.organizerAddTargetB.ColorTable = office2010BlueTheme1;
            this.organizerAddTargetB.Location = new System.Drawing.Point(5, 43);
            this.organizerAddTargetB.Name = "organizerAddTargetB";
            this.organizerAddTargetB.Size = new System.Drawing.Size(90, 20);
            this.organizerAddTargetB.TabIndex = 47;
            this.organizerAddTargetB.Text = "Add Target";
            this.organizerAddTargetB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerAddTargetB.Click += new System.EventHandler(this.organizerAddTarget_Click);
            // 
            // organizerRemoveB
            // 
            this.organizerRemoveB.ColorTable = office2010BlueTheme1;
            this.organizerRemoveB.Location = new System.Drawing.Point(6, 94);
            this.organizerRemoveB.Name = "organizerRemoveB";
            this.organizerRemoveB.Size = new System.Drawing.Size(90, 20);
            this.organizerRemoveB.TabIndex = 46;
            this.organizerRemoveB.Text = "Remove";
            this.organizerRemoveB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerRemoveB.Click += new System.EventHandler(this.organizerRemoveItem_Click);
            // 
            // organizerAddManualB
            // 
            this.organizerAddManualB.ColorTable = office2010BlueTheme1;
            this.organizerAddManualB.Location = new System.Drawing.Point(5, 18);
            this.organizerAddManualB.Name = "organizerAddManualB";
            this.organizerAddManualB.Size = new System.Drawing.Size(90, 20);
            this.organizerAddManualB.TabIndex = 45;
            this.organizerAddManualB.Text = "Add Manual";
            this.organizerAddManualB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerAddManualB.Click += new System.EventHandler(this.organizerAddManual_Click);
            // 
            // organizerSetSourceB
            // 
            this.organizerSetSourceB.ColorTable = office2010BlueTheme1;
            this.organizerSetSourceB.Location = new System.Drawing.Point(558, 60);
            this.organizerSetSourceB.Name = "organizerSetSourceB";
            this.organizerSetSourceB.Size = new System.Drawing.Size(90, 20);
            this.organizerSetSourceB.TabIndex = 66;
            this.organizerSetSourceB.Text = "Source Cont";
            this.organizerSetSourceB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerSetSourceB.Click += new System.EventHandler(this.organizerSetSource_Click);
            // 
            // organizerListView
            // 
            this.organizerListView.CheckBoxes = true;
            this.organizerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13});
            this.organizerListView.FullRowSelect = true;
            this.organizerListView.GridLines = true;
            this.organizerListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.organizerListView.HideSelection = false;
            this.organizerListView.LabelWrap = false;
            this.organizerListView.Location = new System.Drawing.Point(6, 51);
            this.organizerListView.MultiSelect = false;
            this.organizerListView.Name = "organizerListView";
            this.organizerListView.Size = new System.Drawing.Size(255, 284);
            this.organizerListView.TabIndex = 65;
            this.organizerListView.UseCompatibleStateImageBehavior = false;
            this.organizerListView.View = System.Windows.Forms.View.Details;
            this.organizerListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.organizerListView_ItemChecked);
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "X";
            this.columnHeader9.Width = 22;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Item Name";
            this.columnHeader10.Width = 73;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Graphics";
            this.columnHeader11.Width = 55;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Color";
            this.columnHeader12.Width = 44;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Amount";
            this.columnHeader13.Width = 55;
            // 
            // organizerRemoveListB
            // 
            this.organizerRemoveListB.ColorTable = office2010BlueTheme1;
            this.organizerRemoveListB.Location = new System.Drawing.Point(369, 14);
            this.organizerRemoveListB.Name = "organizerRemoveListB";
            this.organizerRemoveListB.Size = new System.Drawing.Size(90, 20);
            this.organizerRemoveListB.TabIndex = 63;
            this.organizerRemoveListB.Text = "Remove";
            this.organizerRemoveListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerRemoveListB.Click += new System.EventHandler(this.organizerRemoveList_Click);
            // 
            // organizerAddListB
            // 
            this.organizerAddListB.ColorTable = office2010BlueTheme1;
            this.organizerAddListB.Location = new System.Drawing.Point(273, 14);
            this.organizerAddListB.Name = "organizerAddListB";
            this.organizerAddListB.Size = new System.Drawing.Size(90, 20);
            this.organizerAddListB.TabIndex = 62;
            this.organizerAddListB.Text = "Add";
            this.organizerAddListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerAddListB.Click += new System.EventHandler(this.organizerAddList_Click);
            // 
            // organizerImportListB
            // 
            this.organizerImportListB.ColorTable = office2010BlueTheme1;
            this.organizerImportListB.Location = new System.Drawing.Point(465, 14);
            this.organizerImportListB.Name = "organizerImportListB";
            this.organizerImportListB.Size = new System.Drawing.Size(90, 20);
            this.organizerImportListB.TabIndex = 59;
            this.organizerImportListB.Text = "Import";
            this.organizerImportListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerImportListB.Click += new System.EventHandler(this.organizerImportListB_Click);
            // 
            // organizerListSelect
            // 
            this.organizerListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.organizerListSelect.FormattingEnabled = true;
            this.organizerListSelect.Location = new System.Drawing.Point(83, 12);
            this.organizerListSelect.Name = "organizerListSelect";
            this.organizerListSelect.Size = new System.Drawing.Size(183, 24);
            this.organizerListSelect.TabIndex = 61;
            this.organizerListSelect.SelectedIndexChanged += new System.EventHandler(this.organizerListSelect_SelectedIndexChanged);
            // 
            // organizerExportListB
            // 
            this.organizerExportListB.ColorTable = office2010BlueTheme1;
            this.organizerExportListB.Location = new System.Drawing.Point(561, 14);
            this.organizerExportListB.Name = "organizerExportListB";
            this.organizerExportListB.Size = new System.Drawing.Size(90, 20);
            this.organizerExportListB.TabIndex = 58;
            this.organizerExportListB.Text = "Export";
            this.organizerExportListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.organizerExportListB.Click += new System.EventHandler(this.organizerExportListB_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 18);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(74, 13);
            this.label24.TabIndex = 60;
            this.label24.Text = "Organizer List:";
            // 
            // VendorBuy
            // 
            this.VendorBuy.Controls.Add(this.groupBox17);
            this.VendorBuy.Controls.Add(this.groupBox18);
            this.VendorBuy.Controls.Add(this.buyEnableCheckBox);
            this.VendorBuy.Controls.Add(this.buyListView);
            this.VendorBuy.Controls.Add(this.buyRemoveListButton);
            this.VendorBuy.Controls.Add(this.buyAddListButton);
            this.VendorBuy.Controls.Add(this.buyImportListButton);
            this.VendorBuy.Controls.Add(this.buyListSelect);
            this.VendorBuy.Controls.Add(this.buyExportListButton);
            this.VendorBuy.Controls.Add(this.label25);
            this.VendorBuy.Location = new System.Drawing.Point(4, 22);
            this.VendorBuy.Name = "VendorBuy";
            this.VendorBuy.Padding = new System.Windows.Forms.Padding(3);
            this.VendorBuy.Size = new System.Drawing.Size(659, 341);
            this.VendorBuy.TabIndex = 3;
            this.VendorBuy.Text = "Vendor Buy";
            this.VendorBuy.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.buyEditB);
            this.groupBox17.Controls.Add(this.buyAddTargetB);
            this.groupBox17.Controls.Add(this.buyRemoveB);
            this.groupBox17.Controls.Add(this.buyAddManualB);
            this.groupBox17.Location = new System.Drawing.Point(553, 84);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(100, 125);
            this.groupBox17.TabIndex = 74;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Item List";
            // 
            // buyEditB
            // 
            this.buyEditB.ColorTable = office2010BlueTheme1;
            this.buyEditB.Location = new System.Drawing.Point(5, 68);
            this.buyEditB.Name = "buyEditB";
            this.buyEditB.Size = new System.Drawing.Size(90, 20);
            this.buyEditB.TabIndex = 48;
            this.buyEditB.Text = "Edit";
            this.buyEditB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyEditB.Click += new System.EventHandler(this.buyEdit_Click);
            // 
            // buyAddTargetB
            // 
            this.buyAddTargetB.ColorTable = office2010BlueTheme1;
            this.buyAddTargetB.Location = new System.Drawing.Point(5, 43);
            this.buyAddTargetB.Name = "buyAddTargetB";
            this.buyAddTargetB.Size = new System.Drawing.Size(90, 20);
            this.buyAddTargetB.TabIndex = 47;
            this.buyAddTargetB.Text = "Add Target";
            this.buyAddTargetB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyAddTargetB.Click += new System.EventHandler(this.buyAddTarget_Click);
            // 
            // buyRemoveB
            // 
            this.buyRemoveB.ColorTable = office2010BlueTheme1;
            this.buyRemoveB.Location = new System.Drawing.Point(5, 94);
            this.buyRemoveB.Name = "buyRemoveB";
            this.buyRemoveB.Size = new System.Drawing.Size(90, 20);
            this.buyRemoveB.TabIndex = 46;
            this.buyRemoveB.Text = "Remove";
            this.buyRemoveB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyRemoveB.Click += new System.EventHandler(this.buyRemoveItem_Click);
            // 
            // buyAddManualB
            // 
            this.buyAddManualB.ColorTable = office2010BlueTheme1;
            this.buyAddManualB.Location = new System.Drawing.Point(5, 18);
            this.buyAddManualB.Name = "buyAddManualB";
            this.buyAddManualB.Size = new System.Drawing.Size(90, 20);
            this.buyAddManualB.TabIndex = 45;
            this.buyAddManualB.Text = "Add Manual";
            this.buyAddManualB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyAddManualB.Click += new System.EventHandler(this.buyAddManual_Click);
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.buyLogBox);
            this.groupBox18.Location = new System.Drawing.Point(267, 84);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(278, 251);
            this.groupBox18.TabIndex = 73;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Buy Log";
            // 
            // buyLogBox
            // 
            this.buyLogBox.FormattingEnabled = true;
            this.buyLogBox.Location = new System.Drawing.Point(7, 18);
            this.buyLogBox.Name = "buyLogBox";
            this.buyLogBox.Size = new System.Drawing.Size(265, 225);
            this.buyLogBox.TabIndex = 0;
            // 
            // buyEnableCheckBox
            // 
            this.buyEnableCheckBox.Location = new System.Drawing.Point(274, 58);
            this.buyEnableCheckBox.Name = "buyEnableCheckBox";
            this.buyEnableCheckBox.Size = new System.Drawing.Size(105, 22);
            this.buyEnableCheckBox.TabIndex = 72;
            this.buyEnableCheckBox.Text = "Enable Buy List";
            this.buyEnableCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buyEnableCheckBox.CheckedChanged += new System.EventHandler(this.buyEnableCheckB_CheckedChanged);
            // 
            // buyListView
            // 
            this.buyListView.CheckBoxes = true;
            this.buyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader23});
            this.buyListView.FullRowSelect = true;
            this.buyListView.GridLines = true;
            this.buyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.buyListView.HideSelection = false;
            this.buyListView.LabelWrap = false;
            this.buyListView.Location = new System.Drawing.Point(6, 51);
            this.buyListView.MultiSelect = false;
            this.buyListView.Name = "buyListView";
            this.buyListView.Size = new System.Drawing.Size(255, 284);
            this.buyListView.TabIndex = 70;
            this.buyListView.UseCompatibleStateImageBehavior = false;
            this.buyListView.View = System.Windows.Forms.View.Details;
            this.buyListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.buyagentListView_ItemChecked);
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "X";
            this.columnHeader14.Width = 21;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Item Name";
            this.columnHeader15.Width = 75;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Graphics";
            this.columnHeader16.Width = 54;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Amount";
            this.columnHeader17.Width = 49;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Color";
            this.columnHeader23.Width = 50;
            // 
            // buyRemoveListButton
            // 
            this.buyRemoveListButton.ColorTable = office2010BlueTheme1;
            this.buyRemoveListButton.Location = new System.Drawing.Point(369, 14);
            this.buyRemoveListButton.Name = "buyRemoveListButton";
            this.buyRemoveListButton.Size = new System.Drawing.Size(90, 20);
            this.buyRemoveListButton.TabIndex = 69;
            this.buyRemoveListButton.Text = "Remove";
            this.buyRemoveListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyRemoveListButton.Click += new System.EventHandler(this.buyRemoveList_Click);
            // 
            // buyAddListButton
            // 
            this.buyAddListButton.ColorTable = office2010BlueTheme1;
            this.buyAddListButton.Location = new System.Drawing.Point(273, 14);
            this.buyAddListButton.Name = "buyAddListButton";
            this.buyAddListButton.Size = new System.Drawing.Size(90, 20);
            this.buyAddListButton.TabIndex = 68;
            this.buyAddListButton.Text = "Add";
            this.buyAddListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyAddListButton.Click += new System.EventHandler(this.buyAddList_Click);
            // 
            // buyImportListButton
            // 
            this.buyImportListButton.ColorTable = office2010BlueTheme1;
            this.buyImportListButton.Location = new System.Drawing.Point(465, 14);
            this.buyImportListButton.Name = "buyImportListButton";
            this.buyImportListButton.Size = new System.Drawing.Size(90, 20);
            this.buyImportListButton.TabIndex = 65;
            this.buyImportListButton.Text = "Import";
            this.buyImportListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyImportListButton.Click += new System.EventHandler(this.buyImportListButton_Click);
            // 
            // buyListSelect
            // 
            this.buyListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buyListSelect.FormattingEnabled = true;
            this.buyListSelect.Location = new System.Drawing.Point(73, 12);
            this.buyListSelect.Name = "buyListSelect";
            this.buyListSelect.Size = new System.Drawing.Size(183, 24);
            this.buyListSelect.TabIndex = 67;
            this.buyListSelect.SelectedIndexChanged += new System.EventHandler(this.buyListSelect_SelectedIndexChanged);
            // 
            // buyExportListButton
            // 
            this.buyExportListButton.ColorTable = office2010BlueTheme1;
            this.buyExportListButton.Location = new System.Drawing.Point(561, 14);
            this.buyExportListButton.Name = "buyExportListButton";
            this.buyExportListButton.Size = new System.Drawing.Size(90, 20);
            this.buyExportListButton.TabIndex = 64;
            this.buyExportListButton.Text = "Export";
            this.buyExportListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.buyExportListButton.Click += new System.EventHandler(this.buyExportListButton_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 18);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(65, 13);
            this.label25.TabIndex = 66;
            this.label25.Text = "Vendor Buy:";
            // 
            // VendorSell
            // 
            this.VendorSell.Controls.Add(this.razorButton1);
            this.VendorSell.Controls.Add(this.sellBagLabel);
            this.VendorSell.Controls.Add(this.sellSetBagButton);
            this.VendorSell.Controls.Add(this.groupBox19);
            this.VendorSell.Controls.Add(this.groupBox20);
            this.VendorSell.Controls.Add(this.sellEnableCheckBox);
            this.VendorSell.Controls.Add(this.sellListView);
            this.VendorSell.Controls.Add(this.sellRemoveListButton);
            this.VendorSell.Controls.Add(this.sellAddListButton);
            this.VendorSell.Controls.Add(this.sellImportListButton);
            this.VendorSell.Controls.Add(this.sellListSelect);
            this.VendorSell.Controls.Add(this.sellExportListButton);
            this.VendorSell.Controls.Add(this.label26);
            this.VendorSell.Location = new System.Drawing.Point(4, 22);
            this.VendorSell.Name = "VendorSell";
            this.VendorSell.Padding = new System.Windows.Forms.Padding(3);
            this.VendorSell.Size = new System.Drawing.Size(659, 341);
            this.VendorSell.TabIndex = 4;
            this.VendorSell.Text = "Vendor Sell";
            this.VendorSell.UseVisualStyleBackColor = true;
            // 
            // razorButton1
            // 
            this.razorButton1.ColorTable = office2010BlueTheme1;
            this.razorButton1.Location = new System.Drawing.Point(553, 102);
            this.razorButton1.Name = "razorButton1";
            this.razorButton1.Size = new System.Drawing.Size(95, 20);
            this.razorButton1.TabIndex = 87;
            this.razorButton1.Text = "Clear Container";
            this.razorButton1.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.razorButton1.Click += new System.EventHandler(this.resetSellBag_Click);
            // 
            // sellBagLabel
            // 
            this.sellBagLabel.Location = new System.Drawing.Point(567, 81);
            this.sellBagLabel.Name = "sellBagLabel";
            this.sellBagLabel.Size = new System.Drawing.Size(72, 19);
            this.sellBagLabel.TabIndex = 86;
            this.sellBagLabel.Text = "0x00000000";
            // 
            // sellSetBagButton
            // 
            this.sellSetBagButton.ColorTable = office2010BlueTheme1;
            this.sellSetBagButton.Location = new System.Drawing.Point(551, 58);
            this.sellSetBagButton.Name = "sellSetBagButton";
            this.sellSetBagButton.Size = new System.Drawing.Size(100, 20);
            this.sellSetBagButton.TabIndex = 85;
            this.sellSetBagButton.Text = "Target Container";
            this.sellSetBagButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellSetBagButton.Click += new System.EventHandler(this.sellSetBag_Click);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.sellEditButton);
            this.groupBox19.Controls.Add(this.sellAddTargerButton);
            this.groupBox19.Controls.Add(this.sellRemoveButton);
            this.groupBox19.Controls.Add(this.sellAddManualButton);
            this.groupBox19.Location = new System.Drawing.Point(553, 128);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(100, 125);
            this.groupBox19.TabIndex = 84;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Item List";
            // 
            // sellEditButton
            // 
            this.sellEditButton.ColorTable = office2010BlueTheme1;
            this.sellEditButton.Location = new System.Drawing.Point(5, 68);
            this.sellEditButton.Name = "sellEditButton";
            this.sellEditButton.Size = new System.Drawing.Size(90, 20);
            this.sellEditButton.TabIndex = 48;
            this.sellEditButton.Text = "Edit";
            this.sellEditButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellEditButton.Click += new System.EventHandler(this.sellEdit_Click);
            // 
            // sellAddTargerButton
            // 
            this.sellAddTargerButton.ColorTable = office2010BlueTheme1;
            this.sellAddTargerButton.Location = new System.Drawing.Point(5, 43);
            this.sellAddTargerButton.Name = "sellAddTargerButton";
            this.sellAddTargerButton.Size = new System.Drawing.Size(90, 20);
            this.sellAddTargerButton.TabIndex = 47;
            this.sellAddTargerButton.Text = "Add Target";
            this.sellAddTargerButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellAddTargerButton.Click += new System.EventHandler(this.sellAddTarget_Click);
            // 
            // sellRemoveButton
            // 
            this.sellRemoveButton.ColorTable = office2010BlueTheme1;
            this.sellRemoveButton.Location = new System.Drawing.Point(5, 94);
            this.sellRemoveButton.Name = "sellRemoveButton";
            this.sellRemoveButton.Size = new System.Drawing.Size(90, 20);
            this.sellRemoveButton.TabIndex = 46;
            this.sellRemoveButton.Text = "Remove";
            this.sellRemoveButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellRemoveButton.Click += new System.EventHandler(this.sellRemove_Click);
            // 
            // sellAddManualButton
            // 
            this.sellAddManualButton.ColorTable = office2010BlueTheme1;
            this.sellAddManualButton.Location = new System.Drawing.Point(5, 18);
            this.sellAddManualButton.Name = "sellAddManualButton";
            this.sellAddManualButton.Size = new System.Drawing.Size(90, 20);
            this.sellAddManualButton.TabIndex = 45;
            this.sellAddManualButton.Text = "Add Manual";
            this.sellAddManualButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellAddManualButton.Click += new System.EventHandler(this.sellAddManual_Click);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.sellLogBox);
            this.groupBox20.Location = new System.Drawing.Point(267, 84);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(278, 251);
            this.groupBox20.TabIndex = 83;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Sell Log";
            // 
            // sellLogBox
            // 
            this.sellLogBox.FormattingEnabled = true;
            this.sellLogBox.Location = new System.Drawing.Point(7, 18);
            this.sellLogBox.Name = "sellLogBox";
            this.sellLogBox.Size = new System.Drawing.Size(265, 225);
            this.sellLogBox.TabIndex = 0;
            // 
            // sellEnableCheckBox
            // 
            this.sellEnableCheckBox.Location = new System.Drawing.Point(274, 58);
            this.sellEnableCheckBox.Name = "sellEnableCheckBox";
            this.sellEnableCheckBox.Size = new System.Drawing.Size(105, 22);
            this.sellEnableCheckBox.TabIndex = 82;
            this.sellEnableCheckBox.Text = "Enable Sell List";
            this.sellEnableCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sellEnableCheckBox.CheckedChanged += new System.EventHandler(this.sellEnableCheck_CheckedChanged);
            // 
            // sellListView
            // 
            this.sellListView.CheckBoxes = true;
            this.sellListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22});
            this.sellListView.FullRowSelect = true;
            this.sellListView.GridLines = true;
            this.sellListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.sellListView.HideSelection = false;
            this.sellListView.LabelWrap = false;
            this.sellListView.Location = new System.Drawing.Point(6, 51);
            this.sellListView.MultiSelect = false;
            this.sellListView.Name = "sellListView";
            this.sellListView.Size = new System.Drawing.Size(255, 284);
            this.sellListView.TabIndex = 81;
            this.sellListView.UseCompatibleStateImageBehavior = false;
            this.sellListView.View = System.Windows.Forms.View.Details;
            this.sellListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.sellagentListView_ItemChecked);
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "X";
            this.columnHeader18.Width = 22;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Item Name";
            this.columnHeader19.Width = 78;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Graphics";
            this.columnHeader20.Width = 54;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Amount";
            this.columnHeader21.Width = 50;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Color";
            this.columnHeader22.Width = 50;
            // 
            // sellRemoveListButton
            // 
            this.sellRemoveListButton.ColorTable = office2010BlueTheme1;
            this.sellRemoveListButton.Location = new System.Drawing.Point(369, 14);
            this.sellRemoveListButton.Name = "sellRemoveListButton";
            this.sellRemoveListButton.Size = new System.Drawing.Size(90, 20);
            this.sellRemoveListButton.TabIndex = 80;
            this.sellRemoveListButton.Text = "Remove";
            this.sellRemoveListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellRemoveListButton.Click += new System.EventHandler(this.sellRemoveList_Click);
            // 
            // sellAddListButton
            // 
            this.sellAddListButton.ColorTable = office2010BlueTheme1;
            this.sellAddListButton.Location = new System.Drawing.Point(273, 14);
            this.sellAddListButton.Name = "sellAddListButton";
            this.sellAddListButton.Size = new System.Drawing.Size(90, 20);
            this.sellAddListButton.TabIndex = 79;
            this.sellAddListButton.Text = "Add";
            this.sellAddListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellAddListButton.Click += new System.EventHandler(this.sellAddList_Click);
            // 
            // sellImportListButton
            // 
            this.sellImportListButton.ColorTable = office2010BlueTheme1;
            this.sellImportListButton.Location = new System.Drawing.Point(465, 14);
            this.sellImportListButton.Name = "sellImportListButton";
            this.sellImportListButton.Size = new System.Drawing.Size(90, 20);
            this.sellImportListButton.TabIndex = 76;
            this.sellImportListButton.Text = "Import";
            this.sellImportListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellImportListButton.Click += new System.EventHandler(this.sellImportListButton_Click);
            // 
            // sellListSelect
            // 
            this.sellListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sellListSelect.FormattingEnabled = true;
            this.sellListSelect.Location = new System.Drawing.Point(73, 12);
            this.sellListSelect.Name = "sellListSelect";
            this.sellListSelect.Size = new System.Drawing.Size(183, 24);
            this.sellListSelect.TabIndex = 78;
            this.sellListSelect.SelectedIndexChanged += new System.EventHandler(this.sellListSelect_SelectedIndexChanged);
            // 
            // sellExportListButton
            // 
            this.sellExportListButton.ColorTable = office2010BlueTheme1;
            this.sellExportListButton.Location = new System.Drawing.Point(561, 14);
            this.sellExportListButton.Name = "sellExportListButton";
            this.sellExportListButton.Size = new System.Drawing.Size(90, 20);
            this.sellExportListButton.TabIndex = 75;
            this.sellExportListButton.Text = "Export";
            this.sellExportListButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.sellExportListButton.Click += new System.EventHandler(this.sellExportListButton_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(3, 18);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(64, 13);
            this.label26.TabIndex = 77;
            this.label26.Text = "Vendor Sell:";
            // 
            // Dress
            // 
            this.Dress.Controls.Add(this.dressStopButton);
            this.Dress.Controls.Add(this.dressConflictCheckB);
            this.Dress.Controls.Add(this.dressBagLabel);
            this.Dress.Controls.Add(this.dressSetBagB);
            this.Dress.Controls.Add(this.undressExecuteButton);
            this.Dress.Controls.Add(this.dressExecuteButton);
            this.Dress.Controls.Add(this.groupBox22);
            this.Dress.Controls.Add(this.label29);
            this.Dress.Controls.Add(this.dressDragDelay);
            this.Dress.Controls.Add(this.groupBox21);
            this.Dress.Controls.Add(this.dressListView);
            this.Dress.Controls.Add(this.dressRemoveListB);
            this.Dress.Controls.Add(this.dressAddListB);
            this.Dress.Controls.Add(this.dressImportListB);
            this.Dress.Controls.Add(this.dressListSelect);
            this.Dress.Controls.Add(this.dressExportListB);
            this.Dress.Controls.Add(this.label28);
            this.Dress.Location = new System.Drawing.Point(4, 22);
            this.Dress.Name = "Dress";
            this.Dress.Padding = new System.Windows.Forms.Padding(3);
            this.Dress.Size = new System.Drawing.Size(659, 341);
            this.Dress.TabIndex = 5;
            this.Dress.Text = "Dress / Arm";
            this.Dress.UseVisualStyleBackColor = true;
            // 
            // dressStopButton
            // 
            this.dressStopButton.ColorTable = office2010BlueTheme1;
            this.dressStopButton.Location = new System.Drawing.Point(407, 58);
            this.dressStopButton.Name = "dressStopButton";
            this.dressStopButton.Size = new System.Drawing.Size(61, 20);
            this.dressStopButton.TabIndex = 91;
            this.dressStopButton.Text = "Stop";
            this.dressStopButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressStopButton.Click += new System.EventHandler(this.dressStopButton_Click);
            // 
            // dressConflictCheckB
            // 
            this.dressConflictCheckB.Location = new System.Drawing.Point(274, 84);
            this.dressConflictCheckB.Name = "dressConflictCheckB";
            this.dressConflictCheckB.Size = new System.Drawing.Size(241, 19);
            this.dressConflictCheckB.TabIndex = 90;
            this.dressConflictCheckB.Text = "Remove Conflict Item";
            this.dressConflictCheckB.CheckedChanged += new System.EventHandler(this.dressConflictCheckB_CheckedChanged);
            // 
            // dressBagLabel
            // 
            this.dressBagLabel.Location = new System.Drawing.Point(566, 154);
            this.dressBagLabel.Name = "dressBagLabel";
            this.dressBagLabel.Size = new System.Drawing.Size(82, 19);
            this.dressBagLabel.TabIndex = 89;
            this.dressBagLabel.Text = "0x00000000";
            // 
            // dressSetBagB
            // 
            this.dressSetBagB.ColorTable = office2010BlueTheme1;
            this.dressSetBagB.Location = new System.Drawing.Point(558, 127);
            this.dressSetBagB.Name = "dressSetBagB";
            this.dressSetBagB.Size = new System.Drawing.Size(88, 20);
            this.dressSetBagB.TabIndex = 88;
            this.dressSetBagB.Text = "Undress Bag";
            this.dressSetBagB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressSetBagB.Click += new System.EventHandler(this.dressSetBagB_Click);
            // 
            // undressExecuteButton
            // 
            this.undressExecuteButton.ColorTable = office2010BlueTheme1;
            this.undressExecuteButton.Location = new System.Drawing.Point(340, 58);
            this.undressExecuteButton.Name = "undressExecuteButton";
            this.undressExecuteButton.Size = new System.Drawing.Size(61, 20);
            this.undressExecuteButton.TabIndex = 87;
            this.undressExecuteButton.Text = "Undres";
            this.undressExecuteButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.undressExecuteButton.Click += new System.EventHandler(this.razorButton10_Click);
            // 
            // dressExecuteButton
            // 
            this.dressExecuteButton.ColorTable = office2010BlueTheme1;
            this.dressExecuteButton.Location = new System.Drawing.Point(274, 58);
            this.dressExecuteButton.Name = "dressExecuteButton";
            this.dressExecuteButton.Size = new System.Drawing.Size(61, 20);
            this.dressExecuteButton.TabIndex = 86;
            this.dressExecuteButton.Text = "Dress";
            this.dressExecuteButton.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressExecuteButton.Click += new System.EventHandler(this.dressExecuteButton_Click);
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.dressAddTargetB);
            this.groupBox22.Controls.Add(this.dressAddManualB);
            this.groupBox22.Controls.Add(this.dressRemoveB);
            this.groupBox22.Controls.Add(this.dressReadB);
            this.groupBox22.Location = new System.Drawing.Point(551, 186);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(100, 125);
            this.groupBox22.TabIndex = 85;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Item List";
            // 
            // dressAddTargetB
            // 
            this.dressAddTargetB.ColorTable = office2010BlueTheme1;
            this.dressAddTargetB.Location = new System.Drawing.Point(5, 68);
            this.dressAddTargetB.Name = "dressAddTargetB";
            this.dressAddTargetB.Size = new System.Drawing.Size(90, 20);
            this.dressAddTargetB.TabIndex = 48;
            this.dressAddTargetB.Text = "Add Target";
            this.dressAddTargetB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressAddTargetB.Click += new System.EventHandler(this.dressAddTargetB_Click);
            // 
            // dressAddManualB
            // 
            this.dressAddManualB.ColorTable = office2010BlueTheme1;
            this.dressAddManualB.Location = new System.Drawing.Point(5, 43);
            this.dressAddManualB.Name = "dressAddManualB";
            this.dressAddManualB.Size = new System.Drawing.Size(90, 20);
            this.dressAddManualB.TabIndex = 47;
            this.dressAddManualB.Text = "Add Clear Layer";
            this.dressAddManualB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressAddManualB.Click += new System.EventHandler(this.dressAddManualB_Click);
            // 
            // dressRemoveB
            // 
            this.dressRemoveB.ColorTable = office2010BlueTheme1;
            this.dressRemoveB.Location = new System.Drawing.Point(5, 94);
            this.dressRemoveB.Name = "dressRemoveB";
            this.dressRemoveB.Size = new System.Drawing.Size(90, 20);
            this.dressRemoveB.TabIndex = 46;
            this.dressRemoveB.Text = "Remove";
            this.dressRemoveB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressRemoveB.Click += new System.EventHandler(this.dressRemoveB_Click);
            // 
            // dressReadB
            // 
            this.dressReadB.ColorTable = office2010BlueTheme1;
            this.dressReadB.Location = new System.Drawing.Point(5, 18);
            this.dressReadB.Name = "dressReadB";
            this.dressReadB.Size = new System.Drawing.Size(90, 20);
            this.dressReadB.TabIndex = 45;
            this.dressReadB.Text = "Read Current";
            this.dressReadB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressReadB.Click += new System.EventHandler(this.dressReadB_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(521, 61);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(105, 13);
            this.label29.TabIndex = 76;
            this.label29.Text = "Drag Item Delay (ms)";
            // 
            // dressDragDelay
            // 
            this.dressDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dressDragDelay.BackColor = System.Drawing.Color.White;
            this.dressDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dressDragDelay.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            this.dressDragDelay.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            this.dressDragDelay.Location = new System.Drawing.Point(475, 58);
            this.dressDragDelay.Name = "dressDragDelay";
            this.dressDragDelay.Size = new System.Drawing.Size(45, 20);
            this.dressDragDelay.TabIndex = 75;
            this.dressDragDelay.TextChanged += new System.EventHandler(this.dressDragDelay_TextChanged);
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.dressLogBox);
            this.groupBox21.Location = new System.Drawing.Point(267, 109);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(278, 226);
            this.groupBox21.TabIndex = 74;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Organizer Log";
            // 
            // dressLogBox
            // 
            this.dressLogBox.FormattingEnabled = true;
            this.dressLogBox.Location = new System.Drawing.Point(7, 18);
            this.dressLogBox.Name = "dressLogBox";
            this.dressLogBox.Size = new System.Drawing.Size(265, 199);
            this.dressLogBox.TabIndex = 0;
            // 
            // dressListView
            // 
            this.dressListView.CheckBoxes = true;
            this.dressListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26,
            this.columnHeader27});
            this.dressListView.FullRowSelect = true;
            this.dressListView.GridLines = true;
            this.dressListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dressListView.HideSelection = false;
            this.dressListView.LabelWrap = false;
            this.dressListView.Location = new System.Drawing.Point(6, 51);
            this.dressListView.MultiSelect = false;
            this.dressListView.Name = "dressListView";
            this.dressListView.Size = new System.Drawing.Size(255, 284);
            this.dressListView.TabIndex = 64;
            this.dressListView.UseCompatibleStateImageBehavior = false;
            this.dressListView.View = System.Windows.Forms.View.Details;
            this.dressListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.dresslistView_ItemChecked);
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "X";
            this.columnHeader24.Width = 22;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Layer";
            this.columnHeader25.Width = 90;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "Name";
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "Serial";
            this.columnHeader27.Width = 75;
            // 
            // dressRemoveListB
            // 
            this.dressRemoveListB.ColorTable = office2010BlueTheme1;
            this.dressRemoveListB.Location = new System.Drawing.Point(366, 14);
            this.dressRemoveListB.Name = "dressRemoveListB";
            this.dressRemoveListB.Size = new System.Drawing.Size(90, 20);
            this.dressRemoveListB.TabIndex = 63;
            this.dressRemoveListB.Text = "Remove";
            this.dressRemoveListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressRemoveListB.Click += new System.EventHandler(this.dressRemoveListB_Click);
            // 
            // dressAddListB
            // 
            this.dressAddListB.ColorTable = office2010BlueTheme1;
            this.dressAddListB.Location = new System.Drawing.Point(270, 14);
            this.dressAddListB.Name = "dressAddListB";
            this.dressAddListB.Size = new System.Drawing.Size(90, 20);
            this.dressAddListB.TabIndex = 62;
            this.dressAddListB.Text = "Add";
            this.dressAddListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressAddListB.Click += new System.EventHandler(this.dressAddListB_Click);
            // 
            // dressImportListB
            // 
            this.dressImportListB.ColorTable = office2010BlueTheme1;
            this.dressImportListB.Location = new System.Drawing.Point(462, 14);
            this.dressImportListB.Name = "dressImportListB";
            this.dressImportListB.Size = new System.Drawing.Size(90, 20);
            this.dressImportListB.TabIndex = 59;
            this.dressImportListB.Text = "Import";
            this.dressImportListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressImportListB.Click += new System.EventHandler(this.dressImportListB_Click);
            // 
            // dressListSelect
            // 
            this.dressListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dressListSelect.FormattingEnabled = true;
            this.dressListSelect.Location = new System.Drawing.Point(78, 12);
            this.dressListSelect.Name = "dressListSelect";
            this.dressListSelect.Size = new System.Drawing.Size(183, 24);
            this.dressListSelect.TabIndex = 61;
            this.dressListSelect.SelectedIndexChanged += new System.EventHandler(this.dressListSelect_SelectedIndexChanged);
            // 
            // dressExportListB
            // 
            this.dressExportListB.ColorTable = office2010BlueTheme1;
            this.dressExportListB.Location = new System.Drawing.Point(558, 14);
            this.dressExportListB.Name = "dressExportListB";
            this.dressExportListB.Size = new System.Drawing.Size(90, 20);
            this.dressExportListB.TabIndex = 58;
            this.dressExportListB.Text = "Export";
            this.dressExportListB.Theme = RazorEnhanced.UI.Theme.MSOffice2010_BLUE;
            this.dressExportListB.Click += new System.EventHandler(this.dressExportListB_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 18);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(56, 13);
            this.label28.TabIndex = 60;
            this.label28.Text = "Dress List:";
            // 
            // friends
            // 
            this.friends.Location = new System.Drawing.Point(4, 22);
            this.friends.Name = "friends";
            this.friends.Padding = new System.Windows.Forms.Padding(3);
            this.friends.Size = new System.Drawing.Size(659, 341);
            this.friends.TabIndex = 6;
            this.friends.Text = "Friends List";
            this.friends.UseVisualStyleBackColor = true;
            // 
            // m_NotifyIcon
            // 
            this.m_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_NotifyIcon.Icon")));
            this.m_NotifyIcon.Text = "Razor Enhanced";
            this.m_NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // openFileDialogscript
            // 
            this.openFileDialogscript.Filter = "Script Files|*.py";
            this.openFileDialogscript.RestoreDirectory = true;
            // 
            // timerupdatestatus
            // 
            this.timerupdatestatus.Enabled = true;
            this.timerupdatestatus.Interval = 1000;
            this.timerupdatestatus.Tick += new System.EventHandler(this.timerupdatestatus_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(674, 410);
            this.Controls.Add(this.tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Razor Enhanced {0}";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabs.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.moreOptTab.ResumeLayout(false);
            this.moreOptTab.PerformLayout();
            this.moreMoreOptTab.ResumeLayout(false);
            this.moreMoreOptTab.PerformLayout();
            this.skillsTab.ResumeLayout(false);
            this.skillsTab.PerformLayout();
            this.mapsTab.ResumeLayout(false);
            this.hotkeysTab.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.macrosTab.ResumeLayout(false);
            this.macroActGroup.ResumeLayout(false);
            this.screenshotTab.ResumeLayout(false);
            this.screenshotTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).EndInit();
            this.statusTab.ResumeLayout(false);
            this.scriptingTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripting)).EndInit();
            this.EnhancedAgent.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.eautoloot.ResumeLayout(false);
            this.eautoloot.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.escavenger.ResumeLayout(false);
            this.escavenger.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.Organizer.ResumeLayout(false);
            this.Organizer.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.VendorBuy.ResumeLayout(false);
            this.VendorBuy.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.VendorSell.ResumeLayout(false);
            this.VendorSell.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.Dress.ResumeLayout(false);
            this.Dress.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == ClientCommunication.WM_UONETEVENT)
				msg.Result = (IntPtr)(ClientCommunication.OnMessage(this, (uint)msg.WParam.ToInt32(), msg.LParam.ToInt32()) ? 1 : 0);
			else if (msg.Msg >= (int)ClientCommunication.UOAMessage.First && msg.Msg <= (int)ClientCommunication.UOAMessage.Last)
				msg.Result = (IntPtr)ClientCommunication.OnUOAMessage(this, msg.Msg, msg.WParam.ToInt32(), msg.LParam.ToInt32());
			else
				base.WndProc(ref msg);
		}

		private void DisableCloseButton()
		{
			IntPtr menu = GetSystemMenu(this.Handle, false);
			EnableMenuItem(menu, 0xF060, 0x00000002); //menu, SC_CLOSE, MF_BYCOMMAND|MF_GRAYED
			m_CanClose = false;
		}

		private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
		{
            Assistant.Timer.Slice();
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			//ClientCommunication.SetCustomNotoHue( 0x2 );
			m_SystemTimer = new System.Timers.Timer(5);
			m_SystemTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
			Timer.SystemTimer = m_SystemTimer;

			this.Hide();
			Language.LoadControlNames(this);

			bool st = Config.GetBool("Systray");
			taskbar.Checked = this.ShowInTaskbar = !st;
			systray.Checked = m_NotifyIcon.Visible = st;

			//this.Text = String.Format( this.Text, Engine.Version );
			UpdateTitle();

			if (!ClientCommunication.InstallHooks(this.Handle)) // WaitForInputIdle done here
			{
				m_CanClose = true;
				SplashScreen.End();
				this.Close();
				System.Diagnostics.Process.GetCurrentProcess().Kill();
				return;
			}

			SplashScreen.Message = LocString.Welcome;
			InitConfig();

			this.Show();
			this.BringToFront();

			Engine.ActiveWindow = this;

			DisableCloseButton();

			tabs_IndexChanged(this, null); // load first tab

			m_ProfileConfirmLoad = false;
			Config.SetupProfilesList(profiles, Config.CurrentProfile.Name);
			m_ProfileConfirmLoad = true;

			showWelcome.Checked = Utility.ToInt32(Config.GetRegString(Microsoft.Win32.Registry.CurrentUser, "ShowWelcome"), 1) == 1;

			m_Tip.Active = true;
			SplashScreen.End();

			LoadSettings();

			enhancedToolbar = new EnhancedToolbar();
			enhancedToolbar.Show();
		}

		private void LoadSettings()
		{
			// Scripting
			scriptTable = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];
			dataGridViewScripting.Rows.Clear();
			dataGridViewScripting.DataSource = scriptTable;

			// ---------------- AUTOLOOT -----------------
			autoLootTextBoxDelay.Text = "100";
			autoLootCheckBox.Checked = false;
			RazorEnhanced.AutoLoot.RefreshLists();

			// ------------ SCAVENGER -------------------
			scavengerDragDelay.Text = "100";
			scavengerCheckBox.Checked = false;
			RazorEnhanced.Scavenger.RefreshLists();



			// ---------------- ORGANIZER ----------------
			organizerDragDelay.Text = "100";
			RazorEnhanced.Organizer.RefreshLists();
            organizerStopButton.Enabled = false;


			// ----------- SELL AGENT -----------------
			RazorEnhanced.SellAgent.RefreshLists();



			// ------------------- BUY AGENT ----------------------
			RazorEnhanced.BuyAgent.RefreshLists();



			// ------------------ DRESS AGENT -------------------------
			RazorEnhanced.Dress.RefreshLists();

		}

		private bool m_Initializing = false;

		internal void InitConfig()
		{
			m_Initializing = true;

			this.opacity.AutoSize = false;
			//this.opacity.Size = new System.Drawing.Size(156, 16);

			opacity.Value = Config.GetInt("Opacity");
			this.Opacity = ((float)opacity.Value) / 100.0;
			opacityLabel.Text = Language.Format(LocString.OpacityA1, opacity.Value);

			this.TopMost = alwaysTop.Checked = Config.GetBool("AlwaysOnTop");
			this.Location = new System.Drawing.Point(Config.GetInt("WindowX"), Config.GetInt("WindowY"));
			this.TopLevel = true;

			bool onScreen = false;
			foreach (Screen s in Screen.AllScreens)
			{
				if (s.Bounds.Contains(this.Location.X + this.Width, this.Location.Y + this.Height) ||
					s.Bounds.Contains(this.Location.X - this.Width, this.Location.Y - this.Height))
				{
					onScreen = true;
					break;
				}
			}

			if (!onScreen)
				this.Location = Point.Empty;

			spellUnequip.Checked = Config.GetBool("SpellUnequip");
			ltRange.Enabled = rangeCheckLT.Checked = Config.GetBool("RangeCheckLT");
			ltRange.Text = Config.GetInt("LTRange").ToString();

			incomingMob.Checked = Config.GetBool("ShowMobNames");
			incomingCorpse.Checked = Config.GetBool("ShowCorpseNames");
			QueueActions.Checked = Config.GetBool("QueueActions");
			queueTargets.Checked = Config.GetBool("QueueTargets");
			chkForceSpeechHue.Checked = setSpeechHue.Enabled = Config.GetBool("ForceSpeechHue");
			chkForceSpellHue.Checked = setBeneHue.Enabled = setNeuHue.Enabled = setHarmHue.Enabled = Config.GetBool("ForceSpellHue");
			if (Config.GetInt("LTHilight") != 0)
			{
				InitPreviewHue(lthilight, "LTHilight");
				//ClientCommunication.SetCustomNotoHue( Config.GetInt( "LTHilight" ) );
				lthilight.Checked = setLTHilight.Enabled = true;
			}
			else
			{
				//ClientCommunication.SetCustomNotoHue( 0 );
				lthilight.Checked = setLTHilight.Enabled = false;
			}

			txtSpellFormat.Text = Config.GetString("SpellFormat");
			txtObjDelay.Text = Config.GetInt("ObjectDelay").ToString();
			chkStealth.Checked = Config.GetBool("CountStealthSteps");

			spamFilter.Checked = Config.GetBool("FilterSpam");
			screenAutoCap.Checked = Config.GetBool("AutoCap");
			radioUO.Checked = !(radioFull.Checked = Config.GetBool("CapFullScreen"));
			screenPath.Text = Config.GetString("CapPath");
			dispTime.Checked = Config.GetBool("CapTimeStamp");
			blockDis.Checked = Config.GetBool("BlockDismount");
			alwaysStealth.Checked = Config.GetBool("AlwaysStealth");
			autoOpenDoors.Checked = Config.GetBool("AutoOpenDoors");

			msglvl.SelectedIndex = Config.GetInt("MessageLevel");

			try
			{
				imgFmt.SelectedItem = Config.GetString("ImageFormat");
			}
			catch
			{
				imgFmt.SelectedIndex = 0;
				Config.SetProperty("ImageFormat", "jpg");
			}

			InitPreviewHue(lblExHue, "ExemptColor");
			InitPreviewHue(lblMsgHue, "SysColor");
			InitPreviewHue(lblWarnHue, "WarningColor");
			InitPreviewHue(chkForceSpeechHue, "SpeechHue");
			InitPreviewHue(lblBeneHue, "BeneficialSpellHue");
			InitPreviewHue(lblHarmHue, "HarmfulSpellHue");
			InitPreviewHue(lblNeuHue, "NeutralSpellHue");

			taskbar.Checked = !(systray.Checked = Config.GetBool("Systray"));
			dispDelta.Checked = Config.GetBool("DisplaySkillChanges");
			corpseRange.Enabled = openCorpses.Checked = Config.GetBool("AutoOpenCorpses");
			corpseRange.Text = Config.GetInt("CorpseRange").ToString();

			actionStatusMsg.Checked = Config.GetBool("ActionStatusMsg");
			autoStackRes.Checked = Config.GetBool("AutoStack");

			rememberPwds.Checked = Config.GetBool("RememberPwds");
			filterSnoop.Checked = Config.GetBool("FilterSnoopMsg");

			preAOSstatbar.Checked = Config.GetBool("OldStatBar");
			showtargtext.Checked = Config.GetBool("LastTargTextFlags");
			smartLT.Checked = Config.GetBool("SmartLastTarget");

			smartCPU.Checked = Config.GetBool("SmartCPU");

			autoFriend.Checked = Config.GetBool("AutoFriend");

			try
			{
				clientPrio.SelectedItem = Config.GetString("ClientPrio");
			}
			catch
			{
				clientPrio.SelectedItem = "Normal";
			}

			forceSizeX.Text = Config.GetInt("ForceSizeX").ToString();
			forceSizeY.Text = Config.GetInt("ForceSizeY").ToString();

			gameSize.Checked = Config.GetBool("ForceSizeEnabled");

			potionEquip.Checked = Config.GetBool("PotionEquip");
			blockHealPoison.Checked = Config.GetBool("BlockHealPoison");

			negotiate.Checked = Config.GetBool("Negotiate");

			logPackets.Checked = Config.GetBool("LogPacketsByDefault");

			healthFmt.Enabled = showHealthOH.Checked = Config.GetBool("ShowHealth");
			healthFmt.Text = Config.GetString("HealthFmt");
			chkPartyOverhead.Checked = Config.GetBool("ShowPartyStats");

			if (smartCPU.Checked)
				ClientCommunication.ClientProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal;

			hotkeyTree.SelectedNode = null;

			m_Initializing = false;
			//Load macro list
		}

		private void tabs_IndexChanged(object sender, System.EventArgs e)
		{
			if (tabs == null)
				return;

			if (tabs.SelectedTab == generalTab)
			{
				Filters.Filter.Draw(filters);
			}
			else if (tabs.SelectedTab == skillsTab)
			{
				RedrawSkills();
			}
			else if (tabs.SelectedTab == hotkeysTab)
			{
				hotkeyTree.SelectedNode = null;
				HotKey.Status = hkStatus;
				if (hotkeyTree.TopNode != null)
					hotkeyTree.TopNode.Expand();
				else
					HotKey.RebuildList(hotkeyTree);
			}

			else if (tabs.SelectedTab == statusTab)
			{
				UpdateRazorStatus();
			}
			else if (tabs.SelectedTab == macrosTab)
			{
				RedrawMacros();

				if (MacroManager.Playing || MacroManager.Recording)
					OnMacroStart(MacroManager.Current);
				else
					OnMacroStop();

				if (MacroManager.Current != null)
					MacroManager.Current.DisplayTo(actionList);

				macroActGroup.Visible = macroTree.SelectedNode != null;
			}
			else if (tabs.SelectedTab == screenshotTab)
			{
				ReloadScreenShotsList();
			}
		}

		private Version m_Ver = System.Reflection.Assembly.GetCallingAssembly().GetName().Version;

		private uint m_OutPrev;
		private uint m_InPrev;

		private void UpdateRazorStatus()
		{
			if (!ClientCommunication.ClientRunning)
				Close();

			uint ps = m_OutPrev;
			uint pr = m_InPrev;
			m_OutPrev = ClientCommunication.TotalOut();
			m_InPrev = ClientCommunication.TotalIn();

			if (PacketHandlers.PlayCharTime < DateTime.Now && PacketHandlers.PlayCharTime + TimeSpan.FromSeconds(30) > DateTime.Now)
			{
				if (Config.GetBool("Negotiate"))
				{
					bool allAllowed = true;
					StringBuilder text = new StringBuilder();

					text.Append(Language.GetString(LocString.NegotiateTitle) + " ");

					for (uint i = 0; i < FeatureBit.MaxBit; i++)
					{
						if (!ClientCommunication.AllowBit(i))
						{
							allAllowed = false;

							text.Append(Language.GetString((LocString)(((int)LocString.FeatureDescBase) + i)));
							text.Append(" ");
							text.Append(Language.GetString(LocString.NotAllowed));
							text.Append(" - ");
						}
					}
					text = text.Remove(text.Length - 3, 3);

					if (allAllowed)
						text.Append(Language.GetString(LocString.AllFeaturesEnabled));

					labelFeatures.Visible = true;
					labelFeatures.Text = text.ToString();
				}
				else
				{
					labelFeatures.Visible = false;
				}
			}

			if (tabs.SelectedTab != statusTab)
				return;

			int time = 0;
			if (ClientCommunication.ConnectionStart != DateTime.MinValue)
				time = (int)((DateTime.Now - ClientCommunication.ConnectionStart).TotalSeconds);

			string status = Language.Format(LocString.RazorStatus1,
				m_Ver,
				Utility.FormatSize(System.GC.GetTotalMemory(false)),
				Utility.FormatSize(m_OutPrev), Utility.FormatSize((long)((m_OutPrev - ps))),
				Utility.FormatSize(m_InPrev), Utility.FormatSize((long)((m_InPrev - pr))),
				Utility.FormatTime(time),
				World.Player != null ? (uint)World.Player.Serial : 0,
				World.Player != null && World.Player.Backpack != null ? (uint)World.Player.Backpack.Serial : 0,
				World.Items.Count,
				World.Mobiles.Count);

			if (World.Player != null)
				status += String.Format("\r\nCoordinates\r\nX: {0}\r\nY: {1}\r\nZ: {2}", World.Player.Position.X, World.Player.Position.Y, World.Player.Position.Z);

			labelStatus.Text = status;
		}

		internal void UpdateSkill(Skill skill)
		{
			double Total = 0;
			for (int i = 0; i < Skill.Count; i++)
				Total += World.Player.Skills[i].Base;
			baseTotal.Text = String.Format("{0:F1}", Total);

			for (int i = 0; i < skillList.Items.Count; i++)
			{
				ListViewItem cur = skillList.Items[i];
				if (cur.Tag == skill)
				{
					cur.SubItems[1].Text = String.Format("{0:F1}", skill.Value);
					cur.SubItems[2].Text = String.Format("{0:F1}", skill.Base);
					cur.SubItems[3].Text = String.Format("{0}{1:F1}", (skill.Delta > 0 ? "+" : ""), skill.Delta);
					cur.SubItems[4].Text = String.Format("{0:F1}", skill.Cap);
					cur.SubItems[5].Text = skill.Lock.ToString()[0].ToString();
					SortSkills();
					return;
				}
			}
		}

		internal void RedrawSkills()
		{
			skillList.BeginUpdate();
			skillList.Items.Clear();
			double Total = 0;
			if (World.Player != null && World.Player.SkillsSent)
			{
				string[] items = new string[6];
				for (int i = 0; i < Skill.Count; i++)
				{
					Skill sk = World.Player.Skills[i];
					Total += sk.Base;
					items[0] = Language.Skill2Str(i);//((SkillName)i).ToString();
					items[1] = String.Format("{0:F1}", sk.Value);
					items[2] = String.Format("{0:F1}", sk.Base);
					items[3] = String.Format("{0}{1:F1}", (sk.Delta > 0 ? "+" : ""), sk.Delta);
					items[4] = String.Format("{0:F1}", sk.Cap);
					items[5] = sk.Lock.ToString()[0].ToString();

					ListViewItem lvi = new ListViewItem(items);
					lvi.Tag = sk;
					skillList.Items.Add(lvi);
				}

				//Config.SetProperty( "SkillListAsc", false );
				SortSkills();
			}
			skillList.EndUpdate();
			baseTotal.Text = String.Format("{0:F1}", Total);
		}

		private void OnFilterCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			((Filter)filters.Items[e.Index]).OnCheckChanged(e.NewValue);
		}

		private void incomingMob_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ShowMobNames", incomingMob.Checked);
		}

		private void incomingCorpse_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ShowCorpseNames", incomingCorpse.Checked);
		}

		private ContextMenu m_SkillMenu;
		private void skillList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ListView.SelectedListViewItemCollection items = skillList.SelectedItems;
				if (items.Count <= 0)
					return;
				Skill s = items[0].Tag as Skill;
				if (s == null)
					return;

				if (m_SkillMenu == null)
				{
					m_SkillMenu = new ContextMenu(new MenuItem[]
					{
						new MenuItem( Language.GetString( LocString.SetSLUp ), new EventHandler( onSetSkillLockUP ) ),
						new MenuItem( Language.GetString( LocString.SetSLDown ), new EventHandler( onSetSkillLockDOWN ) ),
						new MenuItem( Language.GetString( LocString.SetSLLocked ), new EventHandler( onSetSkillLockLOCKED ) ),
					});
				}

				for (int i = 0; i < 3; i++)
					m_SkillMenu.MenuItems[i].Checked = ((int)s.Lock) == i;

				m_SkillMenu.Show(skillList, new Point(e.X, e.Y));
			}
		}

		private void onSetSkillLockUP(object sender, EventArgs e)
		{
			SetLock(LockType.Up);
		}

		private void onSetSkillLockDOWN(object sender, EventArgs e)
		{
			SetLock(LockType.Down);
		}

		private void onSetSkillLockLOCKED(object sender, EventArgs e)
		{
			SetLock(LockType.Locked);
		}

		private void SetLock(LockType lockType)
		{
			ListView.SelectedListViewItemCollection items = skillList.SelectedItems;
			if (items.Count <= 0)
				return;
			Skill s = items[0].Tag as Skill;
			if (s == null)
				return;

			try
			{
				ClientCommunication.SendToServer(new SetSkillLock(s.Index, lockType));

				s.Lock = lockType;
				UpdateSkill(s);

				ClientCommunication.SendToClient(new SkillUpdate(s));
			}
			catch
			{
			}

		}

		private void OnSkillColClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			if (e.Column == Config.GetInt("SkillListCol"))
				Config.SetProperty("SkillListAsc", !Config.GetBool("SkillListAsc"));
			else
				Config.SetProperty("SkillListCol", e.Column);
			SortSkills();
		}

		private void SortSkills()
		{
			int col = Config.GetInt("SkillListCol");
			bool asc = Config.GetBool("SkillListAsc");

			if (col < 0 || col > 5)
				col = 0;

			skillList.BeginUpdate();
			if (col == 0 || col == 5)
			{
				skillList.ListViewItemSorter = null;
				skillList.Sorting = asc ? SortOrder.Ascending : SortOrder.Descending;
			}
			else
			{
				LVDoubleComparer.Column = col;
				LVDoubleComparer.Asc = asc;

				skillList.ListViewItemSorter = LVDoubleComparer.Instance;

				skillList.Sorting = SortOrder.None;
				skillList.Sort();
			}
			skillList.EndUpdate();
			skillList.Refresh();
		}

		private class LVDoubleComparer : IComparer
		{
			internal static readonly LVDoubleComparer Instance = new LVDoubleComparer();
			internal static int Column { set { Instance.m_Col = value; } }
			internal static bool Asc { set { Instance.m_Asc = value; } }

			private int m_Col;
			private bool m_Asc;

			private LVDoubleComparer()
			{
			}

			public int Compare(object x, object y)
			{
				if (x == null || !(x is ListViewItem))
					return m_Asc ? 1 : -1;
				else if (y == null || !(y is ListViewItem))
					return m_Asc ? -1 : 1;

				try
				{
					double dx = Convert.ToDouble(((ListViewItem)x).SubItems[m_Col].Text);
					double dy = Convert.ToDouble(((ListViewItem)y).SubItems[m_Col].Text);

					if (dx > dy)
						return m_Asc ? -1 : 1;
					else if (dx == dy)
						return 0;
					else //if ( dx > dy )
						return m_Asc ? 1 : -1;
				}
				catch
				{
					return ((ListViewItem)x).Text.CompareTo(((ListViewItem)y).Text) * (m_Asc ? 1 : -1);
				}
			}
		}

		private void OnResetSkillDelta(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			for (int i = 0; i < Skill.Count; i++)
				World.Player.Skills[i].Delta = 0;

			RedrawSkills();
		}

		private void OnSetSkillLocks(object sender, System.EventArgs e)
		{
			if (locks.SelectedIndex == -1 || World.Player == null)
				return;

			LockType type = (LockType)locks.SelectedIndex;

			for (short i = 0; i < Skill.Count; i++)
			{
				World.Player.Skills[i].Lock = type;
				ClientCommunication.SendToServer(new SetSkillLock(i, type));
			}
			ClientCommunication.SendToClient(new SkillsList());
			RedrawSkills();
		}

		private void OnDispSkillCheck(object sender, System.EventArgs e)
		{
			Config.SetProperty("DispSkillChanges", dispDelta.Checked);
		}
		private void alwaysTop_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AlwaysOnTop", this.TopMost = alwaysTop.Checked);
		}

		private void profiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (profiles.SelectedIndex < 0 || !m_ProfileConfirmLoad)
				return;

			string name = (string)profiles.Items[profiles.SelectedIndex];
			if (MessageBox.Show(this, Language.Format(LocString.ProfLoadQ, name), "Load?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Config.Save();
				if (!Config.LoadProfile(name))
				{
					MessageBox.Show(this, Language.GetString(LocString.ProfLoadE), "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else
				{
					InitConfig();
					if (World.Player != null)
						Config.SetProfileFor(World.Player);
				}
			}
			else
			{
				m_ProfileConfirmLoad = false;
				for (int i = 0; i < profiles.Items.Count; i++)
				{
					if ((string)profiles.Items[i] == Config.CurrentProfile.Name)
					{
						profiles.SelectedIndex = i;
						break;
					}
				}
				m_ProfileConfirmLoad = true;
			}
		}

		private void delProfile_Click(object sender, System.EventArgs e)
		{
			if (profiles.SelectedIndex < 0)
				return;
			string remove = (string)profiles.Items[profiles.SelectedIndex];

			if (remove == "default")
			{
				MessageBox.Show(this, Language.GetString(LocString.NoDelete), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string file = String.Format("Profiles/{0}.xml", remove);
			if (File.Exists(file))
				File.Delete(file);

			profiles.Items.Remove(remove);
			if (!Config.LoadProfile("default"))
			{
				Config.CurrentProfile.MakeDefault();
				Config.CurrentProfile.Name = "default";
			}
			InitConfig();

			m_ProfileConfirmLoad = false;
			for (int i = 0; i < profiles.Items.Count; i++)
			{
				if ((string)profiles.Items[i] == "default")
				{
					profiles.SelectedIndex = i;
					m_ProfileConfirmLoad = true;
					return;
				}
			}

			int sel = profiles.Items.Count;
			profiles.Items.Add("default");
			profiles.SelectedIndex = sel;
			m_ProfileConfirmLoad = true;
		}

		internal void SelectProfile(string name)
		{
			m_ProfileConfirmLoad = false;
			profiles.SelectedItem = name;
			m_ProfileConfirmLoad = true;
		}

		private void newProfile_Click(object sender, System.EventArgs e)
		{
			if (InputBox.Show(this, Language.GetString(LocString.EnterProfileName), Language.GetString(LocString.EnterAName)))
			{
				string str = InputBox.GetString();
				if (str == null || str == "" || str.IndexOfAny(Path.GetInvalidPathChars()) != -1 || str.IndexOfAny(m_InvalidNameChars) != -1)
				{
					MessageBox.Show(this, Language.GetString(LocString.InvalidChars), Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				m_ProfileConfirmLoad = false;
				int sel = profiles.Items.Count;
				string lwr = str.ToLower();
				for (int i = 0; i < profiles.Items.Count; i++)
				{
					if (lwr == ((string)profiles.Items[i]).ToLower())
					{
						if (MessageBox.Show(this, Language.GetString(LocString.ProfExists), "Load Profile?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							Config.Save();
							profiles.SelectedIndex = i;
							if (!Config.LoadProfile(str))
							{
								MessageBox.Show(this, Language.GetString(LocString.ProfLoadE), "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							}
							else
							{
								InitConfig();
								if (World.Player != null)
									Config.SetProfileFor(World.Player);
							}
						}

						m_ProfileConfirmLoad = true;
						return;
					}
				}

				Config.Save();
				Config.NewProfile(str);
				profiles.Items.Add(str);
				profiles.SelectedIndex = sel;
				InitConfig();
				if (World.Player != null)
					Config.SetProfileFor(World.Player);
				m_ProfileConfirmLoad = true;
			}
		}

		internal bool CanClose
		{
			get
			{
				return m_CanClose;
			}
			set
			{
				m_CanClose = value;
			}
		}

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!m_CanClose && ClientCommunication.ClientRunning)
			{
				DisableCloseButton();
				e.Cancel = true;
			}
		}

		private void skillCopySel_Click(object sender, System.EventArgs e)
		{
			if (skillList.SelectedItems == null || skillList.SelectedItems.Count <= 0)
				return;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < skillList.SelectedItems.Count; i++)
			{
				ListViewItem vi = skillList.SelectedItems[i];
				if (vi != null && vi.SubItems != null && vi.SubItems.Count > 4)
				{
					string name = vi.SubItems[0].Text;
					if (name != null && name.Length > 20)
						name = name.Substring(0, 16) + "...";

					sb.AppendFormat("{0,-20} {1,5:F1} {2,5:F1} {4:F1} {5,5:F1}\n",
						name,
						vi.SubItems[1].Text,
						vi.SubItems[2].Text,
						Utility.ToInt32(vi.SubItems[3].Text, 0) < 0 ? "" : "+",
						vi.SubItems[3].Text,
						vi.SubItems[4].Text);
				}
			}

			if (sb.Length > 0)
				Clipboard.SetDataObject(sb.ToString(), true);
		}

		private void skillCopyAll_Click(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Skill.Count; i++)
			{
				Skill sk = World.Player.Skills[i];
				sb.AppendFormat("{0,-20} {1,-5:F1} {2,-5:F1} {3}{4,-5:F1} {5,-5:F1}\n", (SkillName)i, sk.Value, sk.Base, sk.Delta > 0 ? "+" : "", sk.Delta, sk.Cap);
			}

			if (sb.Length > 0)
				Clipboard.SetDataObject(sb.ToString(), true);
		}

		private void hotkeyTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			ClearHKCtrls();

			if (e.Node == null || !(e.Node.Tag is KeyData))
				return;
			KeyData hk = (KeyData)e.Node.Tag;

			try
			{
				m_LastKV = hk.Key;
				switch (hk.Key)
				{
					case -1:
						key.Text = ("MouseWheel UP");
						break;
					case -2:
						key.Text = ("MouseWheel DOWN");
						break;
					case -3:
						key.Text = ("Mouse MID Button");
						break;
					case -4:
						key.Text = ("Mouse XButton 1");
						break;
					case -5:
						key.Text = ("Mouse XButton 2");
						break;
					default:
						if (hk.Key > 0 && hk.Key < 256)
							key.Text = (((Keys)hk.Key).ToString());
						else
							key.Text = ("");
						break;
				}
			}
			catch
			{
				key.Text = ">>ERROR<<";
			}

			chkCtrl.Checked = (hk.Mod & ModKeys.Control) != 0;
			chkAlt.Checked = (hk.Mod & ModKeys.Alt) != 0;
			chkShift.Checked = (hk.Mod & ModKeys.Shift) != 0;
			chkPass.Checked = hk.SendToUO;

			if ((hk.LocName >= (int)LocString.DrinkHeal && hk.LocName <= (int)LocString.DrinkAg && !ClientCommunication.AllowBit(FeatureBit.PotionHotkeys)) ||
				(hk.LocName >= (int)LocString.TargCloseRed && hk.LocName <= (int)LocString.TargCloseCriminal && !ClientCommunication.AllowBit(FeatureBit.ClosestTargets)) ||
				(((hk.LocName >= (int)LocString.TargRandRed && hk.LocName <= (int)LocString.TargRandNFriend) ||
				(hk.LocName >= (int)LocString.TargRandEnemyHuman && hk.LocName <= (int)LocString.TargRandCriminal)) && !ClientCommunication.AllowBit(FeatureBit.RandomTargets)))
			{
				LockControl(chkCtrl);
				LockControl(chkAlt);
				LockControl(chkShift);
				LockControl(chkPass);
				LockControl(key);
				LockControl(unsetHK);
				LockControl(setHK);
				LockControl(dohotkey);
			}
		}

		private KeyData GetSelectedHK()
		{
			if (hotkeyTree != null && hotkeyTree.SelectedNode != null)
				return hotkeyTree.SelectedNode.Tag as KeyData;
			else
				return null;
		}

		private void ClearHKCtrls()
		{
			m_LastKV = 0;
			key.Text = "";
			chkCtrl.Checked = false;
			chkAlt.Checked = false;
			chkShift.Checked = false;
			chkPass.Checked = false;

			UnlockControl(chkCtrl);
			UnlockControl(chkAlt);
			UnlockControl(chkShift);
			UnlockControl(chkPass);
			UnlockControl(key);
			UnlockControl(unsetHK);
			UnlockControl(setHK);
			UnlockControl(dohotkey);
		}

		private void setHK_Click(object sender, System.EventArgs e)
		{
			KeyData hk = GetSelectedHK();
			if (hk == null || m_LastKV == 0)
				return;

			ModKeys mod = ModKeys.None;
			if (chkCtrl.Checked)
				mod |= ModKeys.Control;
			if (chkAlt.Checked)
				mod |= ModKeys.Alt;
			if (chkShift.Checked)
				mod |= ModKeys.Shift;

			KeyData g = HotKey.Get(m_LastKV, mod);
			bool block = false;
			if (g != null && g != hk)
			{
				if (MessageBox.Show(this, Language.Format(LocString.KeyUsed, g.DispName, hk.DispName), "Hot Key Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					g.Key = 0;
					g.Mod = ModKeys.None;
					g.SendToUO = false;
				}
				else
				{
					block = true;
				}
			}

			if (!block)
			{
				hk.Key = m_LastKV;
				hk.Mod = mod;

				hk.SendToUO = chkPass.Checked;
			}
		}

		private void unsetHK_Click(object sender, System.EventArgs e)
		{
			KeyData hk = GetSelectedHK();
			if (hk == null)
				return;

			hk.Key = 0;
			hk.Mod = 0;
			hk.SendToUO = false;

			ClearHKCtrls();
		}

		private void key_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			m_LastKV = (int)e.KeyCode;
			key.Text = e.KeyCode.ToString();

			e.Handled = true;
		}

		private void key_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				m_LastKV = -1;
				key.Text = "MouseWheel UP";
			}
			else if (e.Delta < 0)
			{
				m_LastKV = -2;
				key.Text = "MouseWheel DOWN";
			}
		}

		private void key_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				m_LastKV = -3;
				key.Text = "Mouse MID Button";
			}
			else if (e.Button == MouseButtons.XButton1)
			{
				m_LastKV = -4;
				key.Text = "Mouse XButton 1";
			}
			else if (e.Button == MouseButtons.XButton2)
			{
				m_LastKV = -5;
				key.Text = "Mouse XButton 2";
			}
		}

		private void dohotkey_Click(object sender, System.EventArgs e)
		{
			KeyData hk = GetSelectedHK();
			if (hk != null && World.Player != null)
			{
				if (MacroManager.AcceptActions)
					MacroManager.Action(new HotKeyAction(hk));
				hk.Callback();
			}
		}

		private void queueTargets_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("QueueTargets", queueTargets.Checked);
		}

		private void chkForceSpeechHue_CheckedChanged(object sender, System.EventArgs e)
		{
			setSpeechHue.Enabled = chkForceSpeechHue.Checked;
			Config.SetProperty("ForceSpeechHue", chkForceSpeechHue.Checked);
		}

		private void lthilight_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!(setLTHilight.Enabled = lthilight.Checked))
			{
				Config.SetProperty("LTHilight", 0);
				ClientCommunication.SetCustomNotoHue(0);
				lthilight.BackColor = SystemColors.Control;
				lthilight.ForeColor = SystemColors.ControlText;
			}
		}

		private void chkForceSpellHue_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkForceSpellHue.Checked)
			{
				setBeneHue.Enabled = setHarmHue.Enabled = setNeuHue.Enabled = true;
				Config.SetProperty("ForceSpellHue", true);
			}
			else
			{
				setBeneHue.Enabled = setHarmHue.Enabled = setNeuHue.Enabled = false;
				Config.SetProperty("ForceSpellHue", false);
			}
		}

		private void txtSpellFormat_TextChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("SpellFormat", txtSpellFormat.Text.Trim());
		}

		private void InitPreviewHue(Control ctrl, string cfg)
		{
			int hueIdx = Config.GetInt(cfg);
			if (hueIdx > 0 && hueIdx < 3000)
				ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
			else
				ctrl.BackColor = SystemColors.Control;
			ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);
		}

		private bool SetHue(Control ctrl, string cfg)
		{
			HueEntry h = new HueEntry(Config.GetInt(cfg));

			if (h.ShowDialog(this) == DialogResult.OK)
			{
				int hueIdx = h.Hue;
				Config.SetProperty(cfg, hueIdx);
				if (hueIdx > 0 && hueIdx < 3000)
					ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
				else
					ctrl.BackColor = Color.White;
				ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);

				return true;
			}
			else
			{
				return false;
			}
		}

		private void setExHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblExHue, "ExemptColor");
		}

		private void setMsgHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblMsgHue, "SysColor");
		}

		private void setWarnHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblWarnHue, "WarningColor");
		}

		private void setSpeechHue_Click(object sender, System.EventArgs e)
		{
			SetHue(chkForceSpeechHue, "SpeechHue");
		}

		private void setLTHilight_Click(object sender, System.EventArgs e)
		{
			if (SetHue(lthilight, "LTHilight"))
				ClientCommunication.SetCustomNotoHue(Config.GetInt("LTHilight"));
		}

		private void setBeneHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblBeneHue, "BeneficialSpellHue");
		}

		private void setHarmHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblHarmHue, "HarmfulSpellHue");
		}

		private void setNeuHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblNeuHue, "NeutralSpellHue");
		}

		private void QueueActions_CheckedChanged(object sender, System.EventArgs e)
		{
			//txtObjDelay.Enabled = QueueActions.Checked;
			Config.SetProperty("QueueActions", QueueActions.Checked);
		}

		private void txtObjDelay_TextChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ObjectDelay", Utility.ToInt32(txtObjDelay.Text.Trim(), 500));
		}

		private void chkStealth_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("CountStealthSteps", chkStealth.Checked);
		}

		private void MainForm_Activated(object sender, System.EventArgs e)
		{
			DisableCloseButton();
			//this.TopMost = true;
		}

		private void MainForm_Deactivate(object sender, System.EventArgs e)
		{
			if (this.TopMost)
				this.TopMost = false;
		}

		private void MainForm_Resize(object sender, System.EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized && !this.ShowInTaskbar)
				this.Hide();
		}

		private bool IsNear(int a, int b)
		{
			return (a <= b + 5 && a >= b - 5);
		}

		private void MainForm_Move(object sender, System.EventArgs e)
		{
			// atempt to dock to the side of the screen.  Also try not to save the X/Y when we are minimized (which is -32000, -32000)
			System.Drawing.Point pt = this.Location;

			Rectangle screen = Screen.GetWorkingArea(this);
			if (this.WindowState != FormWindowState.Minimized && pt.X + this.Width / 2 >= screen.Left && pt.Y + this.Height / 2 >= screen.Top && pt.X <= screen.Right && pt.Y <= screen.Bottom)
			{
				if (IsNear(pt.X + this.Width, screen.Right))
					pt.X = screen.Right - this.Width;
				else if (IsNear(pt.X, screen.Left))
					pt.X = screen.Left;

				if (IsNear(pt.Y + this.Height, screen.Bottom))
					pt.Y = screen.Bottom - this.Height;
				else if (IsNear(pt.Y, screen.Top))
					pt.Y = screen.Top;

				this.Location = pt;
				Config.SetProperty("WindowX", (int)pt.X);
				Config.SetProperty("WindowY", (int)pt.Y);
			}
		}

		private void opacity_Scroll(object sender, System.EventArgs e)
		{
			int o = opacity.Value;
			Config.SetProperty("Opacity", o);
			opacityLabel.Text = String.Format("Opacity: {0}%", o);
			this.Opacity = ((double)o) / 100.0;
		}

		private void dispDelta_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("DisplaySkillChanges", dispDelta.Checked);
		}

		private void logPackets_CheckedChanged(object sender, System.EventArgs e)
		{
			if (logPackets.Checked)
			{
				if (m_Initializing || MessageBox.Show(this, Language.GetString(LocString.PacketLogWarn), "Caution!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
					Packet.Logging = true;
				else
					logPackets.Checked = false;
			}
			else
			{
				Packet.Logging = false;
			}
		}

		private void openCorpses_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AutoOpenCorpses", openCorpses.Checked);
			corpseRange.Enabled = openCorpses.Checked;
		}

		private void corpseRange_TextChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("CorpseRange", Utility.ToInt32(corpseRange.Text, 2));
		}

		private void showWelcome_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetRegString(Microsoft.Win32.Registry.CurrentUser, "ShowWelcome", (showWelcome.Checked ? 1 : 0).ToString());
		}

		private static char[] m_InvalidNameChars = new char[] { '/', '\\', ';', '?', ':', '*' };
		private void newMacro_Click(object sender, System.EventArgs e)
		{
			if (InputBox.Show(this, Language.GetString(LocString.NewMacro), Language.GetString(LocString.EnterAName)))
			{
				string name = InputBox.GetString();
				if (name == null || name == "" || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 || name.IndexOfAny(m_InvalidNameChars) != -1)
				{
					MessageBox.Show(this, Language.GetString(LocString.InvalidChars), Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				TreeNode node = GetMacroDirNode();
				string path = (node == null || !(node.Tag is string)) ? Config.GetUserDirectory("Macros") : (string)node.Tag;
				path = Path.Combine(path, name + ".macro");
				if (File.Exists(path))
				{
					MessageBox.Show(this, Language.GetString(LocString.MacroExists), Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				try
				{
					File.CreateText(path).Close();
				}
				catch
				{
					return;
				}

				Macro m = new Macro(path);
				MacroManager.Add(m);
				TreeNode newNode = new TreeNode(Path.GetFileNameWithoutExtension(m.Filename));
				newNode.Tag = m;
				if (node == null)
					macroTree.Nodes.Add(newNode);
				else
					node.Nodes.Add(newNode);
				macroTree.SelectedNode = newNode;
			}

			RedrawMacros();
		}

		internal Macro GetMacroSel()
		{
			if (macroTree.SelectedNode == null || !(macroTree.SelectedNode.Tag is Macro))
				return null;
			else
				return (Macro)macroTree.SelectedNode.Tag;
		}

		internal void playMacro_Click(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			if (MacroManager.Playing)
			{
				MacroManager.Stop();
				OnMacroStop();
			}
			else
			{
				Macro m = GetMacroSel();
				if (m == null || m.Actions.Count <= 0)
					return;

				actionList.SelectedIndex = 0;
				MacroManager.Play(m);
				playMacro.Text = "Stop";
				recMacro.Enabled = false;
				OnMacroStart(m);
			}
		}

		private void recMacro_Click(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			if (MacroManager.Recording)
			{
				MacroManager.Stop();
				//OnMacroStop();
			}
			else
			{
				Macro m = GetMacroSel();
				if (m == null)
					return;

				bool rec = true;
				if (m.Actions.Count > 0)
					rec = MessageBox.Show(this, Language.GetString(LocString.MacroConfRec), "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

				if (rec)
				{
					MacroManager.Record(m);
					OnMacroStart(m);
					recMacro.Text = "Stop";
					playMacro.Enabled = false;
				}
			}
		}

		internal void OnMacroStart(Macro m)
		{
			actionList.SelectedIndex = -1;
			macroTree.Enabled = actionList.Enabled = false;
			newMacro.Enabled = delMacro.Enabled = false;
			//macroList.SelectedItem = m;
			macroTree.SelectedNode = FindNode(macroTree.Nodes, m);
			macroTree.Update();
			macroTree.Refresh();
			m.DisplayTo(actionList);
		}

		internal void PlayMacro(Macro m)
		{
			playMacro.Text = "Stop";
			recMacro.Enabled = false;
		}

		internal void OnMacroStop()
		{
			recMacro.Text = "Record";
			recMacro.Enabled = true;
			playMacro.Text = "Play";
			playMacro.Enabled = true;
			actionList.SelectedIndex = -1;
			macroTree.Enabled = actionList.Enabled = true;
			newMacro.Enabled = delMacro.Enabled = true;
			RedrawMacros();
		}

		private ContextMenu m_MacroContextMenu = null;
		private void macroTree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.Clicks == 1)
			{
				if (m_MacroContextMenu == null)
				{
					m_MacroContextMenu = new ContextMenu(new MenuItem[]
						{
							new MenuItem( "Add Category", new EventHandler( Macro_AddCategory ) ),
							new MenuItem( "Delete Category", new EventHandler( Macro_DeleteCategory ) ),
							new MenuItem( "Move to Category", new EventHandler( Macro_Move2Category ) ),
							new MenuItem( "-" ),
							new MenuItem( "Refresh Macro List", new EventHandler( Macro_RefreshList ) ),
					});
				}

				Macro sel = GetMacroSel();

				m_MacroContextMenu.MenuItems[1].Enabled = sel == null;
				m_MacroContextMenu.MenuItems[2].Enabled = sel != null;

				m_MacroContextMenu.Show(this, new Point(e.X, e.Y));
			}

			//RedrawMacros();
		}

		private TreeNode GetMacroDirNode()
		{
			if (macroTree.SelectedNode == null)
			{
				return null;
			}
			else
			{
				if (macroTree.SelectedNode.Tag is string)
					return macroTree.SelectedNode;
				else if (macroTree.SelectedNode.Parent == null || !(macroTree.SelectedNode.Parent.Tag is string))
					return null;
				else
					return macroTree.SelectedNode.Parent;
			}
		}

		private void Macro_AddCategory(object sender, EventArgs args)
		{
			if (!InputBox.Show(this, Language.GetString(LocString.CatName)))
				return;

			string path = InputBox.GetString();
			if (path == null || path == "" || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 || path.IndexOfAny(m_InvalidNameChars) != -1)
			{
				MessageBox.Show(this, Language.GetString(LocString.InvalidChars), "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			TreeNode node = GetMacroDirNode();

			try
			{
				if (node == null || !(node.Tag is string))
					path = Path.Combine(Config.GetUserDirectory("Macros"), path);
				else
					path = Path.Combine((string)node.Tag, path);
				Engine.EnsureDirectory(path);
			}
			catch
			{
				MessageBox.Show(this, Language.Format(LocString.CanCreateDir, path), "Unabled to Create Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			TreeNode newNode = new TreeNode(String.Format("[{0}]", Path.GetFileName(path)));
			newNode.Tag = path;
			if (node == null)
				macroTree.Nodes.Add(newNode);
			else
				node.Nodes.Add(newNode);
			RedrawMacros();
			macroTree.SelectedNode = newNode;
		}

		private void Macro_DeleteCategory(object sender, EventArgs args)
		{
			string path = null;
			if (macroTree.SelectedNode != null)
				path = macroTree.SelectedNode.Tag as string;

			if (path == null)
				return;

			try
			{
				Directory.Delete(path);
			}
			catch
			{
				MessageBox.Show(this, Language.GetString(LocString.CantDelDir), "Unabled to Delete Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			TreeNode node = FindNode(macroTree.Nodes, path);
			if (node != null)
				node.Remove();
		}

		private void Macro_Move2Category(object sender, EventArgs args)
		{
			Macro sel = GetMacroSel();
			if (sel == null)
				return;

			if (!InputBox.Show(this, Language.GetString(LocString.CatName)))
				return;

			try
			{
				File.Move(sel.Filename, Path.Combine(Config.GetUserDirectory("Macros"), String.Format("{0}/{1}", InputBox.GetString(), Path.GetFileName(sel.Filename))));
			}
			catch
			{
				MessageBox.Show(this, Language.GetString(LocString.CantMoveMacro), "Unabled to Move Macro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			RedrawMacros();
			macroTree.SelectedNode = FindNode(macroTree.Nodes, sel);
		}

		private void Macro_RefreshList(object sender, EventArgs args)
		{
			RedrawMacros();
		}

		private static TreeNode FindNode(TreeNodeCollection nodes, object tag)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				TreeNode node = nodes[i];

				if (node.Tag == tag)
				{
					return node;
				}
				else if (node.Nodes.Count > 0)
				{
					node = FindNode(node.Nodes, tag);
					if (node != null)
						return node;
				}
			}

			return null;
		}

		private void RedrawMacros()
		{
			Macro ms = GetMacroSel();
			MacroManager.DisplayTo(macroTree);
			if (ms != null)
				macroTree.SelectedNode = FindNode(macroTree.Nodes, ms);
		}

		private void macroTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (MacroManager.Recording)
				return;

			Macro m = e.Node.Tag as Macro;
			macroActGroup.Visible = m != null;
			MacroManager.Select(m, actionList, playMacro, recMacro, loopMacro);
		}

		private void delMacro_Click(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel();
			if (m == null)
			{
				Macro_DeleteCategory(sender, e);
				return;
			}

			if (m == MacroManager.Current)
				return;

			if (m.Actions.Count <= 0 || MessageBox.Show(this, Language.Format(LocString.DelConf, m.ToString()), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					File.Delete(m.Filename);
				}
				catch
				{
					return;
				}

				MacroManager.Remove(m);

				TreeNode node = FindNode(macroTree.Nodes, m);
				if (node != null)
					node.Remove();
			}

			if (macroTree.Nodes.Count == 0)
				macroActGroup.Visible = false;
		}

		private void actionList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.Clicks == 1)
			{
				if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
					return;

				ContextMenu menu = new ContextMenu();
				menu.MenuItems.Add(Language.GetString(LocString.Reload), new EventHandler(onMacroReload));
				menu.MenuItems.Add(Language.GetString(LocString.Save), new EventHandler(onMacroSave));

				MacroAction a;
				try
				{
					a = actionList.SelectedItem as MacroAction;
				}
				catch
				{
					a = null;
				}

				if (a != null)
				{
					int pos = actionList.SelectedIndex;

					menu.MenuItems.Add("-");
					if (actionList.Items.Count > 1)
					{
						menu.MenuItems.Add(Language.GetString(LocString.MoveUp), new EventHandler(OnMacroActionMoveUp));
						if (pos <= 0)
							menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;
						menu.MenuItems.Add(Language.GetString(LocString.MoveDown), new EventHandler(OnMacroActionMoveDown));
						if (pos >= actionList.Items.Count - 1)
							menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;
						menu.MenuItems.Add("-");
					}
					menu.MenuItems.Add(Language.GetString(LocString.RemAct), new EventHandler(onMacroActionDelete));
					menu.MenuItems.Add("-");
					menu.MenuItems.Add(Language.GetString(LocString.BeginRec), new EventHandler(onMacroBegRecHere));
					menu.MenuItems.Add(Language.GetString(LocString.PlayFromHere), new EventHandler(onMacroPlayHere));

					MenuItem[] aMenus = a.GetContextMenuItems();
					if (aMenus != null && aMenus.Length > 0)
					{
						menu.MenuItems.Add("-");
						menu.MenuItems.AddRange(aMenus);
					}
				}

				menu.MenuItems.Add("-");
				menu.MenuItems.Add(Language.GetString(LocString.Constructs), new MenuItem[]
					{
						new MenuItem( Language.GetString( LocString.InsWait ), new EventHandler( onMacroInsPause ) ),
						new MenuItem( Language.GetString( LocString.InsLT ), new EventHandler( onMacroInsertSetLT ) ),
						new MenuItem( Language.GetString( LocString.InsComment ), new EventHandler( onMacroInsertComment ) ),
						new MenuItem( "-" ),
						new MenuItem( Language.GetString( LocString.InsIF ), new EventHandler( onMacroInsertIf ) ),
						new MenuItem( Language.GetString( LocString.InsELSE ), new EventHandler( onMacroInsertElse ) ),
						new MenuItem( Language.GetString( LocString.InsENDIF ), new EventHandler( onMacroInsertEndIf ) ),
						new MenuItem( "-" ),
						new MenuItem( Language.GetString( LocString.InsFOR ), new EventHandler( onMacroInsertFor ) ),
						new MenuItem( Language.GetString( LocString.InsENDFOR ), new EventHandler( onMacroInsertEndFor ) ),
				});

				menu.Show(actionList, new Point(e.X, e.Y));
			}
		}

		private void onMacroPlayHere(object sender, EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int sel = actionList.SelectedIndex + 1;
			if (sel < 0 || sel > m.Actions.Count)
				sel = m.Actions.Count;

			MacroManager.PlayAt(m, sel);
			playMacro.Text = "Stop";
			recMacro.Enabled = false;

			OnMacroStart(m);
		}

		private void onMacroInsertComment(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel();
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			if (InputBox.Show(Language.GetString(LocString.InsComment)))
			{
				m.Actions.Insert(a + 1, new MacroComment(InputBox.GetString()));
				RedrawActionList(m);
			}
		}

		private void onMacroInsertIf(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel();
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			MacroInsertIf ins = new MacroInsertIf(m, a);
			if (ins.ShowDialog(this) == DialogResult.OK)
				RedrawActionList(m);
		}

		private void onMacroInsertElse(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			m.Actions.Insert(a + 1, new ElseAction());
			RedrawActionList(m);
		}

		private void onMacroInsertEndIf(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			m.Actions.Insert(a + 1, new EndIfAction());
			RedrawActionList(m);
		}

		private void onMacroInsertFor(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel();
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			if (InputBox.Show(Language.GetString(LocString.NumIter)))
			{
				m.Actions.Insert(a + 1, new ForAction(InputBox.GetInt(1)));
				RedrawActionList(m);
			}
		}

		private void onMacroInsertEndFor(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel();
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			m.Actions.Insert(a + 1, new EndForAction());
			RedrawActionList(m);
		}

		private void OnMacroActionMoveUp(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int move = actionList.SelectedIndex;
			if (move > 0 && move < m.Actions.Count)
			{
				MacroAction a = (MacroAction)m.Actions[move - 1];
				m.Actions[move - 1] = m.Actions[move];
				m.Actions[move] = a;

				RedrawActionList(m);
				actionList.SelectedIndex = move - 1;
			}
		}

		private void OnMacroActionMoveDown(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int move = actionList.SelectedIndex;
			if (move + 1 < m.Actions.Count)
			{
				MacroAction a = (MacroAction)m.Actions[move + 1];
				m.Actions[move + 1] = m.Actions[move];
				m.Actions[move] = a;

				RedrawActionList(m);
				actionList.SelectedIndex = move + 1;
			}
		}

		private void RedrawActionList(Macro m)
		{
			int sel = actionList.SelectedIndex;
			m.DisplayTo(actionList);
			actionList.SelectedIndex = sel;
		}

		private void actionList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				onMacroActionDelete(sender, e);
		}

		private void onMacroActionDelete(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a < 0 || a >= m.Actions.Count)
				return;

			if (MessageBox.Show(this, Language.Format(LocString.DelConf, m.Actions[a].ToString()), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				m.Actions.RemoveAt(a);
				actionList.Items.RemoveAt(a);
			}
		}

		private void onMacroBegRecHere(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int sel = actionList.SelectedIndex + 1;
			if (sel < 0 || sel > m.Actions.Count)
				sel = m.Actions.Count;

			MacroManager.RecordAt(m, sel);
			recMacro.Text = "Stop";
			playMacro.Enabled = false;
			OnMacroStart(m);
		}

		private void onMacroInsPause(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			MacroInsertWait ins = new MacroInsertWait(m, a);
			if (ins.ShowDialog(this) == DialogResult.OK)
				RedrawActionList(m);
		}

		private void onMacroReload(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			m.Load();
			RedrawActionList(m);
		}

		private void onMacroSave(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			m.Save();
			RedrawActionList(m);
		}

		private void onMacroInsertSetLT(object sender, EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;

			int a = actionList.SelectedIndex;
			if (a >= m.Actions.Count) // -1 is valid, will insert @ top
				return;

			m.Actions.Insert(a + 1, new SetLastTargetAction());
			RedrawActionList(m);
		}

		private void loopMacro_CheckedChanged(object sender, System.EventArgs e)
		{
			Macro m = GetMacroSel(); ;
			if (m == null)
				return;
			m.Loop = loopMacro.Checked;
		}

		private void spamFilter_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("FilterSpam", spamFilter.Checked);
		}

		private void screenAutoCap_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AutoCap", screenAutoCap.Checked);
		}

		private void setScnPath_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folder = new FolderBrowserDialog();
			folder.Description = Language.GetString(LocString.SelSSFolder);
			folder.SelectedPath = Config.GetString("CapPath");
			folder.ShowNewFolderButton = true;

			if (folder.ShowDialog(this) == DialogResult.OK)
			{
				Config.SetProperty("CapPath", folder.SelectedPath);
				screenPath.Text = folder.SelectedPath;

				ReloadScreenShotsList();
			}
		}

		internal void ReloadScreenShotsList()
		{
			ScreenCapManager.DisplayTo(screensList);
			if (screenPrev.Image != null)
			{
				screenPrev.Image.Dispose();
				screenPrev.Image = null;
			}
		}

		private void radioFull_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioFull.Checked)
			{
				radioUO.Checked = false;
				Config.SetProperty("CapFullScreen", true);
			}
		}

		private void radioUO_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioUO.Checked)
			{
				radioFull.Checked = false;
				Config.SetProperty("CapFullScreen", false);
			}
		}

		private void screensList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (screenPrev.Image != null)
			{
				screenPrev.Image.Dispose();
				screenPrev.Image = null;
			}

			if (screensList.SelectedIndex == -1)
				return;

			string file = Path.Combine(Config.GetString("CapPath"), screensList.SelectedItem.ToString());
			if (!File.Exists(file))
			{
				MessageBox.Show(this, Language.Format(LocString.FileNotFoundA1, file), "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				screensList.Items.RemoveAt(screensList.SelectedIndex);
				screensList.SelectedIndex = -1;
				return;
			}

			using (Stream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				screenPrev.Image = Image.FromStream(reader);
			}
		}

		private void screensList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.Clicks == 1)
			{
				ContextMenu menu = new ContextMenu();
				menu.MenuItems.Add("Delete", new EventHandler(DeleteScreenCap));
				if (screensList.SelectedIndex == -1)
					menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;
				menu.MenuItems.Add("Delete ALL", new EventHandler(ClearScreensDirectory));
				menu.Show(screensList, new Point(e.X, e.Y));
			}
		}

		private void DeleteScreenCap(object sender, System.EventArgs e)
		{
			int sel = screensList.SelectedIndex;
			if (sel == -1)
				return;

			string file = Path.Combine(Config.GetString("CapPath"), (string)screensList.SelectedItem);
			if (MessageBox.Show(this, Language.Format(LocString.DelConf, file), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			screensList.SelectedIndex = -1;
			if (screenPrev.Image != null)
			{
				screenPrev.Image.Dispose();
				screenPrev.Image = null;
			}

			try
			{
				File.Delete(file);
				screensList.Items.RemoveAt(sel);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Unable to Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			ReloadScreenShotsList();
		}

		private void ClearScreensDirectory(object sender, System.EventArgs e)
		{
			string dir = Config.GetString("CapPath");
			if (MessageBox.Show(this, Language.Format(LocString.Confirm, dir), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			string[] files = Directory.GetFiles(dir, "*.jpg");
			StringBuilder sb = new StringBuilder();
			int failed = 0;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					File.Delete(files[i]);
				}
				catch
				{
					sb.AppendFormat("{0}\n", files[i]);
					failed++;
				}
			}

			if (failed > 0)
				MessageBox.Show(this, Language.Format(LocString.FileDelError, failed, failed != 1 ? "s" : "", sb.ToString()), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			ReloadScreenShotsList();
		}

		private void capNow_Click(object sender, System.EventArgs e)
		{
			ScreenCapManager.CaptureNow();
		}

		private void dispTime_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("CapTimeStamp", dispTime.Checked);
		}

		internal static void LaunchBrowser(string site)
		{
			try
			{
				System.Diagnostics.Process.Start(site);//"iexplore", site );
			}
			catch
			{
				MessageBox.Show(String.Format("Unable to open browser to '{0}'", site), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void taskbar_CheckedChanged(object sender, System.EventArgs e)
		{
			if (taskbar.Checked)
			{
				systray.Checked = false;
				Config.SetProperty("Systray", false);
				if (!this.ShowInTaskbar)
					MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void systray_CheckedChanged(object sender, System.EventArgs e)
		{
			if (systray.Checked)
			{
				taskbar.Checked = false;
				Config.SetProperty("Systray", true);
				if (this.ShowInTaskbar)
					MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		internal void UpdateTitle()
		{
			string str = Language.GetControlText(this.Name);
			if (str == null || str == "")
				str = "Razor Enhanced {0}";

			str = String.Format(str, Engine.Version);
			if (World.Player != null)
				this.Text = String.Format("{0} - {1} ({2})", str, World.Player.Name, World.ShardName);
			else
				this.Text = str;

			UpdateSystray();
		}

		internal void UpdateSystray()
		{
			if (m_NotifyIcon != null && m_NotifyIcon.Visible)
			{
				if (World.Player != null)
					m_NotifyIcon.Text = String.Format("Razor Enhanced - {0} ({1})", World.Player.Name, World.ShardName);
				else
					m_NotifyIcon.Text = "Razor Enhanced";
			}
		}

		private void DoShowMe(object sender, System.EventArgs e)
		{
			ShowMe();
		}

		internal void ShowMe()
		{
			// Fuck windows, seriously.

			ClientCommunication.BringToFront(this.Handle);
			if (Config.GetBool("AlwaysOnTop"))
				this.TopMost = true;
			if (WindowState != FormWindowState.Normal)
				WindowState = FormWindowState.Normal;
		}

		private void HideMe(object sender, System.EventArgs e)
		{
			//this.WindowState = FormWindowState.Minimized;
			this.TopMost = false;
			this.SendToBack();
			this.Hide();
		}

		private void NotifyIcon_DoubleClick(object sender, System.EventArgs e)
		{
			ShowMe();
		}

		private void ToggleVisible(object sender, System.EventArgs e)
		{
			if (this.Visible)
				HideMe(sender, e);
			else
				ShowMe();
		}

		private void OnClose(object sender, System.EventArgs e)
		{
			m_CanClose = true;
			this.Close();
		}

		private void actionStatusMsg_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ActionStatusMsg", actionStatusMsg.Checked);
		}

		private void autoStackRes_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AutoStack", autoStackRes.Checked);
			//setAutoStackDest.Enabled = autoStackRes.Checked;
		}

		private void screenPath_TextChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("CapPath", screenPath.Text);
		}

		private void rememberPwds_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rememberPwds.Checked && !Config.GetBool("RememberPwds"))
			{
				if (MessageBox.Show(this, Language.GetString(LocString.PWWarn), "Security Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					rememberPwds.Checked = false;
					return;
				}
			}
			Config.SetProperty("RememberPwds", rememberPwds.Checked);
		}

		//private void tabs_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		//{
		//	HotKey.KeyDown(e.KeyData);
		//}

		private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			HotKey.KeyDown(e.KeyData);
		}

		private void spellUnequip_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("SpellUnequip", spellUnequip.Checked);
		}

		private void rangeCheckLT_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("RangeCheckLT", ltRange.Enabled = rangeCheckLT.Checked);
		}

		private void ltRange_TextChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("LTRange", Utility.ToInt32(ltRange.Text, 11));
		}

		private void clientPrio_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = (string)clientPrio.SelectedItem;
			Config.SetProperty("ClientPrio", str);
			try
			{
				ClientCommunication.ClientProcess.PriorityClass = (System.Diagnostics.ProcessPriorityClass)Enum.Parse(typeof(System.Diagnostics.ProcessPriorityClass), str, true);
			}
			catch
			{
			}
		}

		private void filterSnoop_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("FilterSnoopMsg", filterSnoop.Checked);
		}

		private void preAOSstatbar_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("OldStatBar", preAOSstatbar.Checked);
			ClientCommunication.RequestStatbarPatch(preAOSstatbar.Checked);
			if (World.Player != null && !m_Initializing)
				MessageBox.Show(this, "Close and re-open your status bar for the change to take effect.", "Status Window Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void smartLT_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("SmartLastTarget", smartLT.Checked);
		}

		private void showtargtext_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("LastTargTextFlags", showtargtext.Checked);
		}

		private void smartCPU_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("SmartCPU", smartCPU.Checked);
			ClientCommunication.SetSmartCPU(smartCPU.Checked);
		}


		private void blockDis_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("BlockDismount", blockDis.Checked);
		}

		private void imgFmt_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (imgFmt.SelectedIndex != -1)
				Config.SetProperty("ImageFormat", imgFmt.SelectedItem);
			else
				Config.SetProperty("ImageFormat", "jpg");
		}
		private void autoFriend_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AutoFriend", autoFriend.Checked);
		}

		private void alwaysStealth_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AlwaysStealth", alwaysStealth.Checked);
		}

		private void autoOpenDoors_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("AutoOpenDoors", autoOpenDoors.Checked);
		}

		private void msglvl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("MessageLevel", msglvl.SelectedIndex);
		}

		private void screenPrev_Click(object sender, System.EventArgs e)
		{
			string file = screensList.SelectedItem as String;
			if (file != null)
				System.Diagnostics.Process.Start(Path.Combine(Config.GetString("CapPath"), file));
		}

		private Timer m_ResizeTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(1.0), new TimerCallback(ForceSize));

		private static void ForceSize()
		{
			int x, y;

			if (Config.GetBool("ForceSizeEnabled"))
			{
				x = Config.GetInt("ForceSizeX");
				y = Config.GetInt("ForceSizeY");

				if (x > 100 && x < 2000 && y > 100 && y < 2000)
					ClientCommunication.SetGameSize(x, y);
				else
					MessageBox.Show(Engine.MainWindow, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		private void gameSize_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ForceSizeEnabled", gameSize.Checked);
			forceSizeX.Enabled = forceSizeY.Enabled = gameSize.Checked;

			if (gameSize.Checked)
			{
				int x = Utility.ToInt32(forceSizeX.Text, 800);
				int y = Utility.ToInt32(forceSizeY.Text, 600);

				if (x < 100 || y < 100 || x > 2000 || y > 2000)
					MessageBox.Show(this, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				else
					ClientCommunication.SetGameSize(x, y);
			}
			else
			{
				ClientCommunication.SetGameSize(800, 600);
			}

			if (!m_Initializing)
				MessageBox.Show(this, Language.GetString(LocString.RelogRequired), "Relog Required", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		private void forceSizeX_TextChanged(object sender, System.EventArgs e)
		{
			int x = Utility.ToInt32(forceSizeX.Text, 600);
			if (x >= 100 && x <= 2000)
				Config.SetProperty("ForceSizeX", x);

			if (!m_Initializing)
			{
				if (x > 100 && x < 2000)
				{
					m_ResizeTimer.Stop();
					m_ResizeTimer.Start();
				}
			}
		}

		private void forceSizeY_TextChanged(object sender, System.EventArgs e)
		{
			int y = Utility.ToInt32(forceSizeY.Text, 600);
			if (y >= 100 && y <= 2000)
				Config.SetProperty("ForceSizeY", y);

			if (!m_Initializing)
			{
				if (y > 100 && y < 2000)
				{
					m_ResizeTimer.Stop();
					m_ResizeTimer.Start();
				}
			}
		}

		private void potionEquip_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("PotionEquip", potionEquip.Checked);
		}

		private void blockHealPoison_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("BlockHealPoison", blockHealPoison.Checked);
		}

		private void negotiate_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!m_Initializing)
			{
				Config.SetProperty("Negotiate", negotiate.Checked);
				ClientCommunication.SetNegotiate(negotiate.Checked);
			}
		}

		private void lockBox_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, Language.GetString(LocString.FeatureDisabledText), Language.GetString(LocString.FeatureDisabled), MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		private List<PictureBox> m_LockBoxes = new List<PictureBox>();

		internal void LockControl(Control locked)
		{
			if (locked != null)
			{
				if (locked.Parent != null && locked.Parent.Controls != null)
				{
					try
					{
						int y_off = (locked.Size.Height - 16) / 2;
						int x_off = 0;
						System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
						PictureBox newLockBox = new PictureBox();

						if (locked is TextBox)
							x_off += 5;
						else if (!(locked is CheckBox || locked is RadioButton))
							x_off = (locked.Size.Width - 16) / 2;

						newLockBox.Cursor = System.Windows.Forms.Cursors.Help;
						newLockBox.Image = ((System.Drawing.Image)(resources.GetObject("lockBox.Image")));
						newLockBox.Size = new System.Drawing.Size(16, 16);
						newLockBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
						newLockBox.Click += new System.EventHandler(this.lockBox_Click);

						newLockBox.TabIndex = locked.TabIndex;
						newLockBox.TabStop = locked.TabStop;
						newLockBox.Location = new System.Drawing.Point(locked.Location.X + x_off, locked.Location.Y + y_off);
						newLockBox.Name = locked.Name + "LockBox";
						newLockBox.Tag = locked;
						newLockBox.Visible = true;

						locked.Parent.Controls.Add(newLockBox);
						locked.Parent.Controls.SetChildIndex(newLockBox, 0);
						m_LockBoxes.Add(newLockBox);
					}
					catch
					{
					}
				}

				locked.Enabled = false;
			}
		}

		internal void UnlockControl(Control unlock)
		{
			if (unlock != null)
			{
				for (int i = 0; i < m_LockBoxes.Count; i++)
				{
					PictureBox box = m_LockBoxes[i];
					if (box == null)
						continue;

					if (box.Tag == unlock)
					{
						unlock.Enabled = true;
						if (box.Parent != null && box.Parent.Controls != null)
							box.Parent.Controls.Remove(box);

						m_LockBoxes.RemoveAt(i);
						break;
					}
				}
			}
		}

		internal void OnLogout()
		{
			OnMacroStop();

			labelFeatures.Visible = false;

			for (int i = 0; i < m_LockBoxes.Count; i++)
			{
				PictureBox box = m_LockBoxes[i];
				if (box == null)
					continue;

				box.Parent.Controls.Remove(box);
				if (box.Tag is Control)
					((Control)box.Tag).Enabled = true;
			}
			m_LockBoxes.Clear();
		}

		internal void UpdateControlLocks()
		{
			for (int i = 0; i < m_LockBoxes.Count; i++)
			{
				PictureBox box = m_LockBoxes[i];
				if (box == null)
					continue;

				box.Parent.Controls.Remove(box);
				if (box.Tag is Control)
					((Control)box.Tag).Enabled = true;
			}
			m_LockBoxes.Clear();

			if (!ClientCommunication.AllowBit(FeatureBit.SmartLT))
				LockControl(this.smartLT);

			if (!ClientCommunication.AllowBit(FeatureBit.RangeCheckLT))
				LockControl(this.rangeCheckLT);

			if (!ClientCommunication.AllowBit(FeatureBit.AutoOpenDoors))
				LockControl(this.autoOpenDoors);

			if (!ClientCommunication.AllowBit(FeatureBit.UnequipBeforeCast))
				LockControl(this.spellUnequip);

			if (!ClientCommunication.AllowBit(FeatureBit.AutoPotionEquip))
				LockControl(this.potionEquip);

			if (!ClientCommunication.AllowBit(FeatureBit.BlockHealPoisoned))
				LockControl(this.blockHealPoison);

			if (!ClientCommunication.AllowBit(FeatureBit.LoopingMacros))
				LockControl(this.loopMacro);

			if (!ClientCommunication.AllowBit(FeatureBit.OverheadHealth))
			{
				LockControl(this.showHealthOH);
				LockControl(this.healthFmt);
				LockControl(this.chkPartyOverhead);
			}
		}

		internal Assistant.MapUO.MapWindow MapWindow;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

		private void btnMap_Click(object sender, System.EventArgs e)
		{
			if (World.Player != null)
			{
				if (MapWindow == null)
					MapWindow = new Assistant.MapUO.MapWindow();
				//SetParent( MapWindow.Handle, ClientCommunication.FindUOWindow() );
				//MapWindow.Owner = (Form)Form.FromHandle( ClientCommunication.FindUOWindow() );
				MapWindow.Show();
				MapWindow.BringToFront();
			}
		}

		private void showHealthOH_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ShowHealth", healthFmt.Enabled = showHealthOH.Checked);
		}

		private void healthFmt_TextChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("HealthFmt", healthFmt.Text);
		}

		private void chkPartyOverhead_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetProperty("ShowPartyStats", chkPartyOverhead.Checked);
		}

		private void btcLabel_Click(object sender, EventArgs e)
		{

		}

		private void dressTab_Click(object sender, EventArgs e)
		{

		}

		private void exportMacro_Click(object sender, EventArgs e)
		{
			MessageBox.Show("TODO!", "TODO!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}

		private void macroImport_Click(object sender, EventArgs e)
		{
			MessageBox.Show("TODO!", "TODO!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}

		private void adveditorMacro_Click(object sender, EventArgs e)
		{
			MessageBox.Show("TODO!", "TODO!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}

		private void LoadAndInitializeScripts()
		{
			RazorEnhanced.Scripts.Auto = false;
			RazorEnhanced.Scripts.Reset();

			foreach (DataRow row in scriptTable.Rows)
			{
				if ((bool)row["Checked"])
				{
					string status = RazorEnhanced.Scripts.LoadFromFile((string)row["Filename"], TimeSpan.FromMilliseconds(100));
					if (status == "Loaded")
					{
						row["Flag"] = Properties.Resources.green;
					}
					else
					{
						row["Flag"] = Properties.Resources.red;
					}
					row["Status"] = status;
				}
				else
				{
					row["Flag"] = Properties.Resources.yellow;
					row["Status"] = "Idle";
				}
			}
		}

		private void xButton2_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialogscript.ShowDialog();

			if (result == DialogResult.OK) // Test result.
			{
				scriptTable.Rows.Add(false, openFileDialogscript.FileName, Properties.Resources.yellow, "Idle");
				RazorEnhanced.Settings.Save();

				dataGridViewScripting.DataSource = null;
				dataGridViewScripting.DataSource = scriptTable;
			}
		}

		private void xButton3_Click(object sender, EventArgs e)
		{
			for (int i = scriptTable.Rows.Count - 1; i >= 0; i--)
			{
				DataRow row = scriptTable.Rows[i];
				if ((bool)row["Checked"])
				{
					scriptTable.Rows.RemoveAt(i);
				}
			}

			RazorEnhanced.Settings.Save();
			LoadAndInitializeScripts();

			dataGridViewScripting.DataSource = null;
			dataGridViewScripting.DataSource = scriptTable;
		}

		private void MoveUp()
		{
			if (scriptTable.Rows.Count > 1)
			{
				if (dataGridViewScripting.SelectedRows.Count > 0)
				{
					int rowCount = dataGridViewScripting.Rows.Count;
					int index = dataGridViewScripting.SelectedCells[0].OwningRow.Index;

					if (index == 0)
					{
						return;
					}

					DataRow newRow = scriptTable.NewRow();
					// We "clone" the row
					newRow.ItemArray = scriptTable.Rows[index + 1].ItemArray;
					// We remove the old and insert the new
					scriptTable.Rows.RemoveAt(index + 1);
					scriptTable.Rows.InsertAt(newRow, index);

					dataGridViewScripting.Rows[index - 1].Selected = true;

					dataGridViewScripting.DataSource = null;
					dataGridViewScripting.DataSource = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];
				}
			}
		}

		private void MoveDown()
		{
			if (scriptTable.Rows.Count > 1)
			{
				if (dataGridViewScripting.SelectedRows.Count > 0)
				{
					int rowCount = dataGridViewScripting.Rows.Count;
					int index = dataGridViewScripting.SelectedCells[0].OwningRow.Index;

					if (index == (rowCount - 2)) // include the header row
					{
						return;
					}

					DataRow newRow = scriptTable.NewRow();
					// We "clone" the row
					newRow.ItemArray = scriptTable.Rows[index - 1].ItemArray;
					// We remove the old and insert the new
					scriptTable.Rows.RemoveAt(index - 1);
					scriptTable.Rows.InsertAt(newRow, index);

					dataGridViewScripting.Rows[index + 1].Selected = true;

					dataGridViewScripting.DataSource = null;
					dataGridViewScripting.DataSource = scriptTable;
				}
			}
		}

		private void dataGridViewScripting_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			dataGridViewScripting.EndEdit();
			scriptTable.Rows[e.RowIndex][e.ColumnIndex] = dataGridViewScripting[e.ColumnIndex, e.RowIndex].Value;
			scriptTable.AcceptChanges();
			LoadAndInitializeScripts();

			dataGridViewScripting.DataSource = null;
			dataGridViewScripting.DataSource = RazorEnhanced.Settings.Dataset.Tables["SCRIPTING"];
		}

		private void razorButtonDown_Click(object sender, EventArgs e)
		{
			MoveDown();
		}

		private void razorButtonUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void razorCheckBoxAuto_CheckedChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Scripts.Auto = razorCheckBoxAuto.Checked;
		}

		private void razorButtonEdit_Click(object sender, EventArgs e)
		{
			EnhancedScriptEditor.Init();
		}

		private void razorButtonVisitUOD_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.uodreams.com");
			System.Diagnostics.Debug.WriteLine(sender.ToString() + " - " + e.ToString());
		}

		private void razorButtonCreateUODAccount_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.gamesnet.it/register.php");
		}

		private void razorButtonWiki_Click(object sender, EventArgs e)
		{
            System.Diagnostics.Process.Start("http://razorenhanced.marcocarlotto.net/doku.php");
		}

		private void groupBox12_Enter(object sender, EventArgs e)
		{

		}


		// ------------ AUTOLOOT ----------------
		private void autoLootAddItemManual_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
			{
				EnhancedAutoLootAddItemManual ManualAddItem = new EnhancedAutoLootAddItemManual();
				ManualAddItem.TopMost = true;
				ManualAddItem.Show();
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autolootContainerButton_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(autoLootSetContainerTarget_Callback));
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootSetContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item autoLootBag = Assistant.World.FindItem(serial);

			if (autoLootBag == null)
				return;

			if (autoLootBag != null && autoLootBag.Serial.IsItem && autoLootBag.IsContainer && autoLootBag.RootContainer == Assistant.World.Player)
			{
				RazorEnhanced.Misc.SendMessage("Autoloot Container set to: " + autoLootBag.ToString());
				RazorEnhanced.AutoLoot.AddLog("Autoloot Container set to: " + autoLootBag.ToString());
				AutoLoot.AutoLootBag = (int)autoLootBag.Serial.Value;

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid Autoloot Container, set backpack");
				RazorEnhanced.AutoLoot.AddLog("Invalid Autoloot Container, set backpack");
				AutoLoot.AutoLootBag = (int)World.Player.Backpack.Serial.Value;
			}
			 this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, RazorEnhanced.AutoLoot.AutoLootDelay, serial, true);});
             this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.AutoLoot.RefreshLists(); });
		}

		private void autoLootAddItemTarget_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(autoLootItemTarget_Callback));
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item autoLootItem = Assistant.World.FindItem(serial);
			if (autoLootItem != null && autoLootItem.Serial.IsItem)
			{
				RazorEnhanced.Misc.SendMessage("Autoloot item added: " + autoLootItem.ToString());
                this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.AutoLoot.AddItemToList(autoLootItem.Name, autoLootItem.ItemID, autoLootItem.Hue); });

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid target");
			}
		}

		private void autoLootRemoveItem_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
			{
				if (autolootlistView.SelectedItems.Count == 1)
				{
					int index = autolootlistView.SelectedItems[0].Index;
					string selection = autolootListSelect.Text;

					if (RazorEnhanced.Settings.AutoLoot.ListExists(selection))
					{
						List<AutoLoot.AutoLootItem> items;
						RazorEnhanced.Settings.AutoLoot.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.AutoLoot.ItemDelete(selection, items[index]);
							RazorEnhanced.AutoLoot.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootItemEdit_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
			{
				if (autolootlistView.SelectedItems.Count == 1)
				{
					int index = autolootlistView.SelectedItems[0].Index;
					string selection = autolootListSelect.Text;

					if (RazorEnhanced.Settings.AutoLoot.ListExists(selection))
					{
						List<AutoLoot.AutoLootItem> items;
						RazorEnhanced.Settings.AutoLoot.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							AutoLoot.AutoLootItem item = items[index];
							EnhancedAutolootEditItem editItem = new EnhancedAutolootEditItem(selection, index, item);
							editItem.TopMost = true;
							editItem.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootItemProps_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
			{
				if (autolootlistView.SelectedItems.Count == 1)
				{
					int index = autolootlistView.SelectedItems[0].Index;
					string selection = autolootListSelect.Text;

					if (RazorEnhanced.Settings.AutoLoot.ListExists(selection))
					{
						List<AutoLoot.AutoLootItem> items;
						RazorEnhanced.Settings.AutoLoot.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							AutoLoot.AutoLootItem item = items[index];
							EnhancedAutolootEditItemProps editProp = new EnhancedAutolootEditItemProps(selection, index, item);
							editProp.TopMost = true;
							editProp.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}

		private void autoLootEnable_CheckedChanged(object sender, EventArgs e)
		{
            if (World.Player != null)
            {
                if (autolootListSelect.Text != "")
                {
                    if (autoLootCheckBox.Checked)
                    {
                        int delay = -1;
                        autolootListSelect.Enabled = false;
                        autolootButtonAddList.Enabled = false;
                        autoLootButtonListExport.Enabled = false;
                        autoLootButtonListImport.Enabled = false;
                        autoLootButtonRemoveList.Enabled = false;
                        autoLootTextBoxDelay.Enabled = false;
                        try
                        {
                            delay = Convert.ToInt32(autoLootTextBoxDelay.Text);
                        }
                        catch
                        {
                            RazorEnhanced.AutoLoot.AutoMode = false;
                            RazorEnhanced.AutoLoot.AddLog("ERROR: Loot item delay is not valid");
                            return;
                        }

                        if (delay < 0)
                        {
                            RazorEnhanced.AutoLoot.AutoMode = false;
                            RazorEnhanced.AutoLoot.AddLog("ERROR: Loot item delay is not valid");
                            return;
                        }

                        RazorEnhanced.AutoLoot.AutoMode = true;
                        RazorEnhanced.AutoLoot.AddLog("Autoloot Engine Start...");
                        RazorEnhanced.Misc.SendMessage("AUTOLOOT: Engine Start...");
                    }
                    else
                    {
                        autolootListSelect.Enabled = true;
                        autolootButtonAddList.Enabled = true;
                        autoLootButtonListExport.Enabled = true;
                        autoLootButtonListImport.Enabled = true;
                        autoLootButtonRemoveList.Enabled = true;
                        autoLootTextBoxDelay.Enabled = true;

                        // Stop autoloot
                        RazorEnhanced.AutoLoot.AutoMode = false;
                        RazorEnhanced.Misc.SendMessage("AUTOLOOT: Engine Stop...");
                        RazorEnhanced.AutoLoot.AddLog("Autoloot Engine Stop...");
                    }
                }
                else
                {
                    autoLootCheckBox.Checked = false;
                    RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
                }
            }
            else
            {
                autoLootCheckBox.Checked = false;
                RazorEnhanced.AutoLoot.AddLog("You are not logged in game!");
            }
		}

		private void autoLootListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int bag = 0;
			int delay = 0;
			RazorEnhanced.Settings.AutoLoot.ListDetailsRead(autolootListSelect.Text, out bag, out delay);
			RazorEnhanced.AutoLoot.AutoLootBag = bag;
			RazorEnhanced.AutoLoot.AutoLootDelay = delay;

			RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, RazorEnhanced.AutoLoot.AutoLootDelay, RazorEnhanced.AutoLoot.AutoLootBag, true);
			RazorEnhanced.AutoLoot.RefreshItems();

			if (autolootListSelect.Text != "")
				RazorEnhanced.AutoLoot.AddLog("Autoloot list changed to: " + autolootListSelect.Text);
		}

		private void autoLootTextBoxDelay_TextChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.AutoLoot.ListUpdate(autolootListSelect.Text, RazorEnhanced.AutoLoot.AutoLootDelay, RazorEnhanced.AutoLoot.AutoLootBag, true);
			RazorEnhanced.AutoLoot.RefreshLists();
		}

		private void autoLootButtonAddList_Click(object sender, EventArgs e)
		{
			EnhancedAutoLootAddList AddItemList = new EnhancedAutoLootAddList();
			AddItemList.TopMost = true;
			AddItemList.Show();
		}

		private void autoLootButtonRemoveList_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != null)
				RazorEnhanced.AutoLoot.AddLog("Autoloot list " + autolootListSelect.Text + " removed!");

			RazorEnhanced.AutoLoot.AutoLootBag = 0;
			RazorEnhanced.AutoLoot.RemoveList(autolootListSelect.Text);
		}

		private void autolootlistView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (autolootlistView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.AutoLoot.UpdateSelectedItems(item.Index);
			}
		}

		delegate void SetBoolCallback(bool check);

		internal void SetCheckBoxAutoMode(bool check)
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.razorCheckBoxAuto.InvokeRequired)
			{
				SetBoolCallback d = new SetBoolCallback(SetCheckBoxAutoMode);
				this.Invoke(d, new object[] { check });
			}
			else
			{
				this.razorCheckBoxAuto.Checked = check;
			}
		}

		private void razorButtonResetIgnore_Click(object sender, EventArgs e)
		{
			RazorEnhanced.AutoLoot.ResetIgnore();
		}

		private void autoLootButtonListExport_Click(object sender, EventArgs e)
		{
			if (autolootListSelect.Text != "")
				RazorEnhanced.ImportExport.ExportAutoloot(autolootListSelect.Text);
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");
		}
		private void autoLootImport_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ImportExport.ImportAutoloot();
		}
		// ------------ AUTOLOOT END ----------------




		// ------------ SCAVENGER ----------------
		private void scavengerRemoveItem_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
			{
				if (scavengerListView.SelectedItems.Count == 1)
				{
					int index = scavengerListView.SelectedItems[0].Index;
					string selection = scavengerListSelect.Text;

					if (RazorEnhanced.Settings.Scavenger.ListExists(selection))
					{
						List<Scavenger.ScavengerItem> items;
						RazorEnhanced.Settings.Scavenger.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.Scavenger.ItemDelete(selection, items[index]);
							RazorEnhanced.Scavenger.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		private void scavengerEditProps_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
			{
				if (scavengerListView.SelectedItems.Count == 1)
				{
					int index = scavengerListView.SelectedItems[0].Index;
					string selection = ScavengerListSelect.Text;

					if (RazorEnhanced.Settings.Scavenger.ListExists(selection))
					{
						List<Scavenger.ScavengerItem> items;
						RazorEnhanced.Settings.Scavenger.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							Scavenger.ScavengerItem item = items[index];
							EnhancedScavengerEditItemProps editProp = new EnhancedScavengerEditItemProps(selection, index, item);
							editProp.TopMost = true;
							editProp.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		private void scavengerEditItem_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
			{
				if (scavengerListView.SelectedItems.Count == 1)
				{
					int index = scavengerListView.SelectedItems[0].Index;
					string selection = scavengerListSelect.Text;

					if (RazorEnhanced.Settings.Scavenger.ListExists(selection))
					{
						List<Scavenger.ScavengerItem> items;
						RazorEnhanced.Settings.Scavenger.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							Scavenger.ScavengerItem item = items[index];
							EnhancedScavengerEditItem editItem = new EnhancedScavengerEditItem(selection, index, item);
							editItem.TopMost = true;
							editItem.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		private void scavengerAddItemTarget_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(ScavengerItemTarget_Callback));
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		private void ScavengerItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item autoLootItem = Assistant.World.FindItem(serial);
			if (autoLootItem != null && autoLootItem.Serial.IsItem)
			{
				RazorEnhanced.Misc.SendMessage("Autoloot item added: " + autoLootItem.ToString());
                this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Scavenger.AddItemToList(autoLootItem.Name, autoLootItem.ItemID, autoLootItem.Hue); });

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid target");
			}
		}

		private void scavengerAdItemdManual_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
			{
				EnhancedScavengerManualAdd manualAddItem = new EnhancedScavengerManualAdd();
				manualAddItem.TopMost = true;
				manualAddItem.Show();
			}
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		private void scavengerSetContainer_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(ScavengerItemContainerTarget_Callback));
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		private void ScavengerItemContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item scavengerBag = Assistant.World.FindItem(serial);

			if (scavengerBag == null)
				return;

			if (scavengerBag != null && scavengerBag.Serial.IsItem && scavengerBag.IsContainer && scavengerBag.RootContainer == Assistant.World.Player)
			{
				RazorEnhanced.Misc.SendMessage("Scavenger Container set to: " + scavengerBag.ToString());
				RazorEnhanced.Scavenger.AddLog("Scavenger Container set to: " + scavengerBag.ToString());
				Scavenger.ScavengerBag = (int)scavengerBag.Serial.Value;

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid Scavenger Container, set backpack");
				RazorEnhanced.Scavenger.AddLog("Invalid Scavenger Container, set backpack");
				Scavenger.ScavengerBag = (int)World.Player.Backpack.Serial.Value;
			}

			this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Settings.Scavenger.ListUpdate(scavengerListSelect.Text, RazorEnhanced.Scavenger.ScavengerDelay, serial, true); });
            this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Scavenger.RefreshLists(); });
		}

		private void scavengerAddList_Click(object sender, EventArgs e)
		{
			EnhancedScavengerAddList AddItemList = new EnhancedScavengerAddList();
			AddItemList.TopMost = true;
			AddItemList.Show();
		}

		private void scavengerRemoveList_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != null)
				RazorEnhanced.Scavenger.AddLog("Scavenger list " + scavengerListSelect.Text + " removed!");

			RazorEnhanced.Scavenger.ScavengerBag = 0;
			RazorEnhanced.Scavenger.RemoveList(scavengerListSelect.Text);
		}

		private void scavengertListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int bag = 0;
			int delay = 0;
			RazorEnhanced.Settings.Scavenger.ListDetailsRead(scavengerListSelect.Text, out bag, out delay);
			RazorEnhanced.Scavenger.ScavengerBag = bag;
			RazorEnhanced.Scavenger.ScavengerDelay = delay;

			RazorEnhanced.Settings.Scavenger.ListUpdate(scavengerListSelect.Text, RazorEnhanced.Scavenger.ScavengerDelay, RazorEnhanced.Scavenger.ScavengerBag, true);
			RazorEnhanced.Scavenger.RefreshItems();

			if (scavengerListSelect.Text != "")
				RazorEnhanced.Scavenger.AddLog("Scavenger list changed to: " + scavengerListSelect.Text);
		}

		private void scavengerEnableCheck_CheckedChanged(object sender, EventArgs e)
		{
            if (World.Player != null)
            {
                if (scavengerListSelect.Text != "")
                {
                    if (scavengerCheckBox.Checked)
                    {
                        int delay = -1;
                        ScavengerListSelect.Enabled = false;
                        scavengerButtonAddList.Enabled = false;
                        scavengerButtonRemoveList.Enabled = false;
                        scavengerButtonExport.Enabled = false;
                        scavengerButtonImport.Enabled = false;
                        scavengerDragDelay.Enabled = false;
                        try
                        {
                            delay = Convert.ToInt32(scavengerDragDelay.Text);
                        }
                        catch
                        {
                            RazorEnhanced.Scavenger.AddLog("ERROR: Drag item delay is not valid");
                            RazorEnhanced.Scavenger.AutoMode = false;
                            return;
                        }
                        if (delay < 0)
                        {
                            RazorEnhanced.Scavenger.AddLog("ERROR: Drag item delay is not valid");
                            RazorEnhanced.Scavenger.AutoMode = false;
                            return;
                        }

                        RazorEnhanced.Scavenger.AutoMode = false;
                        RazorEnhanced.Scavenger.AddLog("Fail to start Scavenger Engine...");
                        RazorEnhanced.Misc.SendMessage("SCAVENGER: Engine Stop...");
                        scavengerCheckBox.Checked = false;

                    }
                    else
                    {
                        ScavengerListSelect.Enabled = true;
                        scavengerButtonAddList.Enabled = true;
                        scavengerButtonRemoveList.Enabled = true;
                        scavengerButtonExport.Enabled = true;
                        scavengerButtonImport.Enabled = true;
                        scavengerDragDelay.Enabled = true;


                        RazorEnhanced.Scavenger.AutoMode = false;
                        RazorEnhanced.Scavenger.AddLog("Scavenger Engine Stop...");
                        RazorEnhanced.Misc.SendMessage("SCAVENGER: Engine Stop...");
                    }
                }
                else
                {
                    scavengerCheckBox.Checked = false;
                    RazorEnhanced.Scavenger.AddLog("Item list not selected!");
                }
            }
            else
            {
                scavengerCheckBox.Checked = false;
                RazorEnhanced.Scavenger.AddLog("You are not logged in game!");
            }
		}

		private void scavengerDragDelay_TextChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.Scavenger.ListUpdate(scavengerListSelect.Text, RazorEnhanced.Scavenger.ScavengerDelay, RazorEnhanced.Scavenger.ScavengerBag, true);
			RazorEnhanced.Scavenger.RefreshLists();
		}

		private void scavengerListView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (scavengerListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.Scavenger.UpdateSelectedItems(item.Index);
			}
		}
		private void scavengerButtonImport_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ImportExport.ImportScavenger();
		}

		private void scavengerButtonExport_Click(object sender, EventArgs e)
		{
			if (scavengerListSelect.Text != "")
				RazorEnhanced.ImportExport.ExportScavenger(scavengerListSelect.Text);
			else
				RazorEnhanced.Scavenger.AddLog("Item list not selected!");
		}

		// ------------ SCAVENGER END ----------------



		// ------------ ORGANIZER ----------------

		private void organizerAddList_Click(object sender, EventArgs e)
		{
			EnhancedOrganizerAddList addItemList = new EnhancedOrganizerAddList();
			addItemList.TopMost = true;
			addItemList.Show();
		}

		private void organizerRemoveList_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != null)
				RazorEnhanced.Organizer.AddLog("Organizer list " + organizerListSelect.Text + " removed!");

			RazorEnhanced.Organizer.OrganizerSource = 0;
			RazorEnhanced.Organizer.OrganizerDestination = 0;
			RazorEnhanced.Organizer.RemoveList(organizerListSelect.Text);
		}

		private void organizerAddManual_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
			{
				EnhancedOrganizerManualAdd manualAddItem = new EnhancedOrganizerManualAdd();
				manualAddItem.TopMost = true;
				manualAddItem.Show();
			}
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void organizerEdit_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
			{
				if (organizerListView.SelectedItems.Count == 1)
				{
					int index = organizerListView.SelectedItems[0].Index;
					string selection = organizerListSelect.Text;

					if (RazorEnhanced.Settings.Organizer.ListExists(selection))
					{
						List<Organizer.OrganizerItem> items;
						RazorEnhanced.Settings.Organizer.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							Organizer.OrganizerItem item = items[index];
							EnhancedOrganizerEditItem editItem = new EnhancedOrganizerEditItem(selection, index, item);
							editItem.TopMost = true;
							editItem.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void organizerRemoveItem_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
			{
				if (organizerListView.SelectedItems.Count == 1)
				{
					int index = organizerListView.SelectedItems[0].Index;
					string selection = organizerListSelect.Text;

					if (RazorEnhanced.Settings.Organizer.ListExists(selection))
					{
						List<Organizer.OrganizerItem> items;
						RazorEnhanced.Settings.Organizer.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.Organizer.ItemDelete(selection, items[index]);
							RazorEnhanced.Organizer.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void organizerSetSource_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OrganizerSourceContainerTarget_Callback));
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void OrganizerSourceContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item organizerBag = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (organizerBag == null)
			{
				RazorEnhanced.Misc.SendMessage("Invalid Source Container, set backpack");
				RazorEnhanced.Organizer.AddLog("Invalid Source Container, set backpack");
				RazorEnhanced.Organizer.OrganizerSource = (int)World.Player.Backpack.Serial.Value;
				return;
			}

			if (organizerBag != null && organizerBag.Serial.IsItem && organizerBag.IsContainer)
			{
				RazorEnhanced.Misc.SendMessage("Source Container set to: " + organizerBag.ToString());
				RazorEnhanced.Organizer.AddLog("Source Container set to: " + organizerBag.ToString());
				RazorEnhanced.Organizer.OrganizerSource = (int)organizerBag.Serial.Value;
			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid Source Container, set backpack");
				RazorEnhanced.Organizer.AddLog("Invalid Source Container, set backpack");
				RazorEnhanced.Organizer.OrganizerSource = (int)World.Player.Backpack.Serial.Value;
			}

			 this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, serial, RazorEnhanced.Organizer.OrganizerDestination, true); });
             this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Organizer.RefreshLists(); });
		}

		private void organizerSetDestination_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OrganizerDestinationContainerTarget_Callback));
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void OrganizerDestinationContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item organizerBag = Assistant.World.FindItem((Assistant.Serial)((uint)serial));

			if (organizerBag == null)
			{
				RazorEnhanced.Misc.SendMessage("Invalid Destination Container, set backpack");
				RazorEnhanced.Organizer.AddLog("Invalid Destination Container, set backpack");
				RazorEnhanced.Organizer.OrganizerDestination = (int)World.Player.Backpack.Serial.Value;
				return;
			}

			if (organizerBag != null && organizerBag.Serial.IsItem && organizerBag.IsContainer)
			{
				RazorEnhanced.Misc.SendMessage("Destination Container set to: " + organizerBag.ToString());
				RazorEnhanced.Organizer.AddLog("Destination Container set to: " + organizerBag.ToString());
				RazorEnhanced.Organizer.OrganizerDestination = (int)organizerBag.Serial.Value;

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid Destination Container, set backpack");
				RazorEnhanced.Organizer.AddLog("Invalid Destination Container, set backpack");
				RazorEnhanced.Organizer.OrganizerDestination = (int)World.Player.Backpack.Serial.Value;
			}

			 this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, RazorEnhanced.Organizer.OrganizerSource, serial, true); });
             this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Organizer.RefreshLists(); });
		}

		private void organizerListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int bagsource;
			int bagdestination;
			int delay;
			RazorEnhanced.Settings.Organizer.ListDetailsRead(organizerListSelect.Text, out bagsource, out bagdestination, out delay);
			RazorEnhanced.Organizer.OrganizerDelay = delay;
			RazorEnhanced.Organizer.OrganizerSource = bagsource;
			RazorEnhanced.Organizer.OrganizerDestination = bagdestination;

			RazorEnhanced.Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, RazorEnhanced.Organizer.OrganizerSource, RazorEnhanced.Organizer.OrganizerDestination, true);
			RazorEnhanced.Organizer.RefreshItems();

			if (organizerListSelect.Text != "")
				RazorEnhanced.Organizer.AddLog("Organizer list changed to: " + organizerListSelect.Text);
		}

		private void organizerAddTarget_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OrganizerItemTarget_Callback));
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}

		private void OrganizerItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item organizerItem = Assistant.World.FindItem(serial);
			if (organizerItem != null && organizerItem.Serial.IsItem)
			{
				RazorEnhanced.Misc.SendMessage("Organizer item added: " + organizerItem.ToString());
                this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Organizer.AddItemToList(organizerItem.Name, organizerItem.ItemID, -1, -1); });
			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid target");
			}
		}

		private void organizerDragDelay_TextChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.Organizer.ListUpdate(organizerListSelect.Text, RazorEnhanced.Organizer.OrganizerDelay, RazorEnhanced.Organizer.OrganizerSource, RazorEnhanced.Organizer.OrganizerDestination, true);
			RazorEnhanced.Organizer.RefreshLists();
		}

		private void organizerListView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (organizerListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.Organizer.UpdateSelectedItems(item.Index);
			}
		}

		private void organizerExecute_Click(object sender, EventArgs e)
		{
            if (World.Player != null)
            {
			    RazorEnhanced.Item sourceBag = RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
			    int sourceBagSerial;

			    RazorEnhanced.Item destinationBag = RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
			    int destinationBagSerial;

			    sourceBagSerial = Convert.ToInt32(Assistant.Engine.MainWindow.organizerSourceLabel.Text, 16);
			    sourceBag = RazorEnhanced.Items.FindBySerial(sourceBagSerial);
			    if (sourceBag != null)
			    {
				    RazorEnhanced.Organizer.AddLog("Source Container OK: 0x" + sourceBag.Serial.ToString("X8"));
				    RazorEnhanced.Organizer.OrganizerSource = sourceBag.Serial;
			    }
			    else
			    {
				    RazorEnhanced.Organizer.AddLog("Source Container Fail, switch packpack: 0x" + Assistant.World.Player.Backpack.Serial.Value.ToString("X8"));
				    RazorEnhanced.Organizer.OrganizerSource = Assistant.World.Player.Backpack.Serial;
			    }

			    destinationBagSerial = Convert.ToInt32(Assistant.Engine.MainWindow.organizerDestinationLabel.Text, 16);
			    destinationBag = RazorEnhanced.Items.FindBySerial(destinationBagSerial);
			    if (sourceBag != null)
			    {
				    RazorEnhanced.Organizer.AddLog("Destination Container OK: 0x" + destinationBag.Serial.ToString("X8"));
				    RazorEnhanced.Organizer.OrganizerDestination = destinationBag.Serial;
			    }
			    else
			    {
				    RazorEnhanced.Organizer.AddLog("Destination Container Fail, switch packpack: 0x" + Assistant.World.Player.Backpack.Serial.Value.ToString("X8"));
				    RazorEnhanced.Organizer.OrganizerDestination = Assistant.World.Player.Backpack.Serial;
			    }


                int delay = -1;
                try
                {
                    delay = Convert.ToInt32(organizerDragDelay.Text);
                }
                catch
                {
                    RazorEnhanced.Organizer.AddLog("ERROR: Drag item delay is not valid");
                    return;
                }
                if (delay < 0)
                {
                    RazorEnhanced.Organizer.AddLog("ERROR: Drag item delay is not valid");
                    return;
                }

                RazorEnhanced.Organizer.Start();
                RazorEnhanced.Organizer.AddLog("Organizer Engine Start...");
                RazorEnhanced.Misc.SendMessage("ORGANIZER: Engine Start...");
                organizerStopButton.Enabled = true;
                organizerExecuteButton.Enabled = false;
                organizerListSelect.Enabled = false;
                organizerAddListB.Enabled = false;
                organizerRemoveListB.Enabled = false;
                organizerExportListB.Enabled = false;
                organizerImportListB.Enabled = false;
                organizerDragDelay.Enabled = false;
			}
			else
			{
				RazorEnhanced.Organizer.AddLog("You are not logged in game!");
				organizerStopButton.Enabled = false;
				organizerExecuteButton.Enabled = true;
				organizerListSelect.Enabled = true;
				organizerAddListB.Enabled = true;
				organizerRemoveListB.Enabled = true;
				organizerExportListB.Enabled = true;
				organizerImportListB.Enabled = true;
				organizerDragDelay.Enabled = true;
			}
		}

		private void organizerStop_Click(object sender, EventArgs e)
		{
			RazorEnhanced.Organizer.ForceStop();

			RazorEnhanced.Organizer.AddLog("Organizer Engine force stop...");
			RazorEnhanced.Misc.SendMessage("ORGANIZER: Organizer Engine force stop...");
			organizerExecuteButton.Enabled = true;
			organizerListSelect.Enabled = true;
			organizerAddListB.Enabled = true;
			organizerRemoveListB.Enabled = true;
			organizerExportListB.Enabled = true;
			organizerImportListB.Enabled = true;
			organizerDragDelay.Enabled = true;
		}

		delegate void OrganizerFinishWorkCallback();

		internal void OrganizerFinishWork()
		{
			if (organizerExecuteButton.InvokeRequired ||
				organizerListSelect.InvokeRequired ||
				organizerAddListB.InvokeRequired ||
				organizerRemoveListB.InvokeRequired ||
				organizerExportListB.InvokeRequired ||
				organizerImportListB.InvokeRequired ||
				organizerDragDelay.InvokeRequired)
			{
				OrganizerFinishWorkCallback d = new OrganizerFinishWorkCallback(OrganizerFinishWork);
				this.Invoke(d, null);
			}
			else
			{
				organizerExecuteButton.Enabled = true;
				organizerListSelect.Enabled = true;
				organizerAddListB.Enabled = true;
				organizerRemoveListB.Enabled = true;
				organizerExportListB.Enabled = true;
				organizerImportListB.Enabled = true;
				organizerDragDelay.Enabled = true;
			}
		}
		private void organizerImportListB_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ImportExport.ImportOrganizer();
		}

		private void organizerExportListB_Click(object sender, EventArgs e)
		{
			if (organizerListSelect.Text != "")
				RazorEnhanced.ImportExport.ExportOrganizer(organizerListSelect.Text);
			else
				RazorEnhanced.Organizer.AddLog("Item list not selected!");
		}
		// ------------------ ORGANIZER END--------------------------





		// ------------------ SELL AGENT --------------------------
		private void sellListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			RazorEnhanced.SellAgent.SellBag = RazorEnhanced.Settings.SellAgent.BagRead(sellListSelect.Text);
			RazorEnhanced.SellAgent.RefreshItems();
			if (sellListSelect.Text != "")
				RazorEnhanced.SellAgent.AddLog("Sell Agent list changed to: " + sellListSelect.Text);
		}

		private void sellAddList_Click(object sender, EventArgs e)
		{
			EnhancedSellAgentAddList addItemList = new EnhancedSellAgentAddList();
			addItemList.TopMost = true;
			addItemList.Show();
		}

		private void sellRemoveList_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != null)
				RazorEnhanced.SellAgent.AddLog("Sell Agent list " + sellListSelect.Text + " removed!");

			RazorEnhanced.SellAgent.RemoveList(sellListSelect.Text);
		}

		private void sellAddManual_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
			{
				EnhancedSellAgentManualAdd ManualAddItem = new EnhancedSellAgentManualAdd();
				ManualAddItem.TopMost = true;
				ManualAddItem.Show();
			}
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void sellAddTarget_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(SellAgentItemTarget_Callback));
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void SellAgentItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item sellItem = Assistant.World.FindItem(serial);
			if (sellItem != null && sellItem.Serial.IsItem)
			{
				RazorEnhanced.Misc.SendMessage("Sell Agent item added: " + sellItem.ToString());
                this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.SellAgent.AddItemToList(sellItem.Name, sellItem.ItemID, 999, sellItem.Hue); });
			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid target");
			}
		}

		private void sellEdit_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
			{
				if (sellListView.SelectedItems.Count == 1)
				{
					int index = sellListView.SelectedItems[0].Index;
					string selection = sellListSelect.Text;

					if (RazorEnhanced.Settings.SellAgent.ListExists(selection))
					{
						List<RazorEnhanced.SellAgent.SellAgentItem> items;
						RazorEnhanced.Settings.SellAgent.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.SellAgent.SellAgentItem item = items[index];
							EnhancedSellAgentEditItem editItem = new EnhancedSellAgentEditItem(selection, index, item);
							editItem.TopMost = true;
							editItem.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void sellRemove_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
			{
				if (sellListView.SelectedItems.Count == 1)
				{
					int index = sellListView.SelectedItems[0].Index;
					string selection = sellListSelect.Text;

					if (RazorEnhanced.Settings.SellAgent.ListExists(selection))
					{
						List<RazorEnhanced.SellAgent.SellAgentItem> items;
						RazorEnhanced.Settings.SellAgent.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.SellAgent.ItemDelete(selection, items[index]);
							RazorEnhanced.SellAgent.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

        private void sellEnableCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (World.Player != null)
            {
                if (sellEnableCheckBox.Checked)
                {
                    if (sellListSelect.Text != "")
                    {
                        Assistant.Item bag = null;
                        int serialHotBag = Convert.ToInt32(sellBagLabel.Text, 16);
                        bag = Assistant.World.FindItem(serialHotBag);

                        if (bag != null)
                            if (bag.RootContainer != World.Player || !bag.IsContainer)
                            {
                                RazorEnhanced.SellAgent.AddLog("Invalid or not accessible Container!");
                                RazorEnhanced.Misc.SendMessage("Invalid or not accessible Container!");
                                sellEnableCheckBox.Checked = false;
                            }
                            else
                            {
                                sellListSelect.Enabled = false;
                                sellAddListButton.Enabled = false;
                                sellRemoveListButton.Enabled = false;
                                sellImportListButton.Enabled = false;
                                sellExportListButton.Enabled = false;
                                RazorEnhanced.SellAgent.AddLog("Apply item list " + sellListSelect.SelectedItem.ToString() + " filter ok!");
                                RazorEnhanced.Misc.SendMessage("Apply item list " + sellListSelect.SelectedItem.ToString() + " filter ok!");
                                RazorEnhanced.SellAgent.EnableSellFilter();
                            }
                        else
                        {
                            RazorEnhanced.SellAgent.AddLog("Invalid or not accessible Container!");
                            RazorEnhanced.Misc.SendMessage("Invalid or not accessible Container!");
                            sellEnableCheckBox.Checked = false;
                        }
                    }
                    else
                    {
                        sellEnableCheckBox.Checked = false;
                        RazorEnhanced.SellAgent.AddLog("Item list not selected!");
                    }
                }
                else
                {
                    sellListSelect.Enabled = true;
                    sellAddListButton.Enabled = true;
                    sellRemoveListButton.Enabled = true;
                    sellImportListButton.Enabled = true;
                    sellExportListButton.Enabled = true;
                    RazorEnhanced.SellAgent.AddLog("Remove item list " + sellListSelect.SelectedItem.ToString() + " filter ok!");
                    RazorEnhanced.Misc.SendMessage("Remove item list " + sellListSelect.SelectedItem.ToString() + " filter ok!");
                }
            }
            else
            {
                sellEnableCheckBox.Checked = false;
                RazorEnhanced.SellAgent.AddLog("You are not logged in game!");
            }
        }

		private void resetSellBag_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
			{
				RazorEnhanced.SellAgent.SellBag = 0;
                RazorEnhanced.Settings.SellAgent.ListUpdate(sellListSelect.Text, 0, true);
			}
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void sellSetBag_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(sellBagTarget_Callback));
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}

		private void sellBagTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item sellBag = Assistant.World.FindItem(serial);

			if (sellBag == null)
				return;

			if (sellBag != null && sellBag.Serial.IsItem && sellBag.IsContainer && sellBag.RootContainer == Assistant.World.Player)
			{
				RazorEnhanced.Misc.SendMessage("Container set to: " + sellBag.ToString());
				RazorEnhanced.SellAgent.AddLog("Container set to: " + sellBag.ToString());
				RazorEnhanced.SellAgent.SellBag = (int)sellBag.Serial.Value;

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid container, set backpack");
				RazorEnhanced.SellAgent.AddLog("Invalid container, set backpack");
				RazorEnhanced.SellAgent.SellBag = (int)World.Player.Backpack.Serial.Value;
			}

			this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Settings.SellAgent.ListUpdate(sellListSelect.Text, serial, true); });
            this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.SellAgent.RefreshLists(); });
		}

		private void sellagentListView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (sellListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.SellAgent.UpdateSelectedItems(item.Index);
			}
		}
		private void sellImportListButton_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ImportExport.ImportSell();
		}

		private void sellExportListButton_Click(object sender, EventArgs e)
		{
			if (sellListSelect.Text != "")
				RazorEnhanced.ImportExport.ExportSell(sellListSelect.Text);
			else
				RazorEnhanced.SellAgent.AddLog("Item list not selected!");
		}
		// ------------------ SELL AGENT END--------------------------



		// ------------------ BUY AGENT --------------------------
		private void buyListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.BuyAgent.ListUpdate(buyListSelect.Text, true);
			RazorEnhanced.BuyAgent.RefreshItems();

			if (sellListSelect.Text != "")
				RazorEnhanced.BuyAgent.AddLog("Buy Agent list changed to: " + buyListSelect.Text);
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}

		private void buyAddList_Click(object sender, EventArgs e)
		{
			EnhancedBuyAgentAddList AddItemList = new EnhancedBuyAgentAddList();
			AddItemList.TopMost = true;
			AddItemList.Show();
		}

		private void buyRemoveList_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != null)
				RazorEnhanced.BuyAgent.AddLog("Buy Agent list " + buyListSelect.Text + " removed!");

			RazorEnhanced.BuyAgent.RemoveList(buyListSelect.Text);
		}

		private void buyAddManual_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != "")
			{
				EnhancedBuyAgentManualAdd manualAddItem = new EnhancedBuyAgentManualAdd();
				manualAddItem.TopMost = true;
				manualAddItem.Show();
			}
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}

		private void buyAddTarget_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != "")
			{
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(BuyAgentItemTarget_Callback));
			}
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}

		private void BuyAgentItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item buyItem = Assistant.World.FindItem(serial);
			if (buyItem != null && buyItem.Serial.IsItem)
			{
				RazorEnhanced.Misc.SendMessage("Buy Agent item added: " + buyItem.ToString());
                this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.BuyAgent.AddItemToList(buyItem.Name, buyItem.ItemID, 999, buyItem.Hue); });
			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid target");
			}
		}

		private void buyEdit_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != "")
			{
				if (buyListView.SelectedItems.Count == 1)
				{
					int index = buyListView.SelectedItems[0].Index;
					string selection = buyListSelect.Text;

					if (RazorEnhanced.Settings.BuyAgent.ListExists(selection))
					{
						List<RazorEnhanced.BuyAgent.BuyAgentItem> items;
						RazorEnhanced.Settings.BuyAgent.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.BuyAgent.BuyAgentItem item = items[index];
							EnhancedBuyAgentEditItem editItem = new EnhancedBuyAgentEditItem(selection, index, item);
							editItem.TopMost = true;
							editItem.Show();
						}
					}
				}
			}
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}

		private void buyRemoveItem_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != "")
			{
				if (buyListView.SelectedItems.Count == 1 && buyListSelect.Text != "")
				{
					int index = buyListView.SelectedItems[0].Index;
					string selection = buyListSelect.Text;

					if (RazorEnhanced.Settings.BuyAgent.ListExists(selection))
					{
						List<RazorEnhanced.BuyAgent.BuyAgentItem> items;
						RazorEnhanced.Settings.BuyAgent.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.BuyAgent.ItemDelete(selection, items[index]);
							RazorEnhanced.BuyAgent.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}

		private void buyEnableCheckB_CheckedChanged(object sender, EventArgs e)
		{
            if (World.Player != null)
            {
                if (buyListSelect.Text != "")
                {
                    if (buyEnableCheckBox.Checked)
                    {
                        buyListSelect.Enabled = false;
                        buyAddListButton.Enabled = false;
                        buyRemoveListButton.Enabled = false;
                        buyImportListButton.Enabled = false;
                        buyExportListButton.Enabled = false;
                        RazorEnhanced.BuyAgent.AddLog("Apply item list " + buyListSelect.SelectedItem.ToString() + " filter ok!");
                        RazorEnhanced.Misc.SendMessage("Apply item list " + buyListSelect.SelectedItem.ToString() + " filter ok!");
                        RazorEnhanced.BuyAgent.EnableBuyFilter();
                    }
                    else
                    {
                        buyListSelect.Enabled = true;
                        buyAddListButton.Enabled = true;
                        buyRemoveListButton.Enabled = true;
                        buyImportListButton.Enabled = true;
                        buyExportListButton.Enabled = true;
                        RazorEnhanced.BuyAgent.AddLog("Remove item list " + buyListSelect.SelectedItem.ToString() + " filter ok!");
                        RazorEnhanced.Misc.SendMessage("Remove item list " + buyListSelect.SelectedItem.ToString() + " filter ok!");
                    }
                }
                else
                {
                    buyEnableCheckBox.Checked = false;
                    RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
                    return;
                }
            }
            else
            {
                buyEnableCheckBox.Checked = false;
                RazorEnhanced.BuyAgent.AddLog("You are not logged in game!");
                return;
            }
		}

		private void buyagentListView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (buyListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.BuyAgent.UpdateSelectedItems(item.Index);
			}
		}
		private void buyImportListButton_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ImportExport.ImportBuy();
		}
		private void buyExportListButton_Click(object sender, EventArgs e)
		{
			if (buyListSelect.Text != "")
				RazorEnhanced.ImportExport.ExportBuy(buyListSelect.Text);
			else
				RazorEnhanced.BuyAgent.AddLog("Item list not selected!");
		}
		// --------------- BUY AGENT END ---------

		// --------------- DRESS START ---------
		private void dressListSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int bag = 0;
			int delay = 0;
			bool conflict = false;
			RazorEnhanced.Settings.Dress.ListDetailsRead(dressListSelect.Text, out bag, out delay, out conflict);
			RazorEnhanced.Dress.DressBag = bag;
			RazorEnhanced.Dress.DressDelay = delay;
			RazorEnhanced.Dress.DressConflict = conflict;
			RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
			RazorEnhanced.Dress.RefreshItems();

			if (dressListSelect.Text != "")
				RazorEnhanced.Dress.AddLog("Dress list changed to: " + dressListSelect.Text);
		}
		private void dressAddListB_Click(object sender, EventArgs e)
		{
			EnhancedDressAddList AddItemList = new EnhancedDressAddList();
			AddItemList.TopMost = true;
			AddItemList.Show();
		}

		private void dressRemoveListB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != null)
				RazorEnhanced.Dress.AddLog("Dress list " + dressListSelect.Text + " removed!");

			RazorEnhanced.Dress.DressBag = 0;
			RazorEnhanced.Dress.DressDelay = 100;
			RazorEnhanced.Dress.DressConflict = false;
			RazorEnhanced.Dress.RemoveList(dressListSelect.Text);
		}

		private void dressDragDelay_TextChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
			RazorEnhanced.Dress.RefreshLists();
		}

		private void dressConflictCheckB_CheckedChanged(object sender, EventArgs e)
		{
			RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true);
			RazorEnhanced.Dress.RefreshLists();
		}

		private void dressReadB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != "")
				RazorEnhanced.Dress.ReadPlayerDress();
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void dressSetBagB_Click(object sender, EventArgs e)
		{

			if (dressListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(DressItemContainerTarget_Callback));
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void DressItemContainerTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item dressBag = Assistant.World.FindItem(serial);

			if (dressBag == null)
				return;

			if (dressBag != null && dressBag.Serial.IsItem && dressBag.IsContainer)
			{
				RazorEnhanced.Misc.SendMessage("Undress Container set to: " + dressBag.ToString());
				RazorEnhanced.Dress.AddLog("Undress Container set to: " + dressBag.ToString());
				RazorEnhanced.Dress.DressBag = (int)dressBag.Serial.Value;

			}
			else
			{
				RazorEnhanced.Misc.SendMessage("Invalid Undress Container, set backpack");
				RazorEnhanced.Dress.AddLog("Invalid Undress Container, set backpack");
				RazorEnhanced.Dress.DressBag = (int)World.Player.Backpack.Serial.Value;
			}

			this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Settings.Dress.ListUpdate(dressListSelect.Text, RazorEnhanced.Dress.DressDelay, RazorEnhanced.Dress.DressBag, RazorEnhanced.Dress.DressConflict, true); });
            this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Dress.RefreshLists(); });
		}
		private void dressRemoveB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != "")
			{
				if (dressListView.SelectedItems.Count == 1)
				{
					int index = dressListView.SelectedItems[0].Index;
					string selection = dressListSelect.Text;

					if (RazorEnhanced.Settings.Dress.ListExists(selection))
					{
						List<Dress.DressItem> items;
						RazorEnhanced.Settings.Dress.ItemsRead(selection, out items);
						if (index <= items.Count - 1)
						{
							RazorEnhanced.Settings.Dress.ItemDelete(selection, items[index]);
							RazorEnhanced.Dress.RefreshItems();
						}
					}
				}
			}
			else
				RazorEnhanced.AutoLoot.AddLog("Item list not selected!");

		}

		private void dresslistView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (dressListView.FocusedItem != null)
			{
				ListViewItem item = e.Item as ListViewItem;
				RazorEnhanced.Dress.UpdateSelectedItems(item.Index);
			}
		}

		private void dressAddTargetB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != "")
				Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(DressItemTarget_Callback));
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void DressItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item dressItem = Assistant.World.FindItem(serial);
            if (dressItem != null && dressItem.Serial.IsItem)
                this.BeginInvoke((MethodInvoker)delegate { RazorEnhanced.Dress.AddItemByTarger(dressItem); });
            else
                RazorEnhanced.Misc.SendMessage("Invalid target");
		}

		private void dressAddManualB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != "")
			{
				EnhancedDressAddUndressLayer ManualAddLayer = new EnhancedDressAddUndressLayer();
				ManualAddLayer.TopMost = true;
				ManualAddLayer.Show();
			}
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void dressImportListB_Click(object sender, EventArgs e)
		{
			RazorEnhanced.ImportExport.ImportDress();
		}

		private void dressExportListB_Click(object sender, EventArgs e)
		{
			if (dressListSelect.Text != "")
				RazorEnhanced.ImportExport.ExportDress(dressListSelect.Text);
			else
				RazorEnhanced.Dress.AddLog("Item list not selected!");
		}

		private void razorButton10_Click(object sender, EventArgs e)
		{
            if (World.Player != null)
			{
			    RazorEnhanced.Item undressbag = RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
			    int undressbagserial = RazorEnhanced.Dress.DressBag;

    		    undressbag = RazorEnhanced.Items.FindBySerial(undressbagserial);
			    if (undressbag != null)
			    {
				    RazorEnhanced.Dress.AddLog("Undress Container OK: 0x" + undressbag.Serial.ToString("X8"));
			    }
			    else
			    {
				    RazorEnhanced.Dress.AddLog("Undress Container Fail, switch packpack: 0x" + Assistant.World.Player.Backpack.Serial.Value.ToString("X8"));
				    RazorEnhanced.Dress.DressBag = Assistant.World.Player.Backpack.Serial;
			    }

                int delay = -1;
                try
                {
                    delay = Convert.ToInt32(dressDragDelay.Text);
                }
                catch
                {
                    RazorEnhanced.Dress.AddLog("ERROR: Drag item delay is not valid");
                    return;
                }
                if (delay < 0)
                {
                    RazorEnhanced.Dress.AddLog("ERROR: Drag item delay is not valid");
                    return;
                }

				RazorEnhanced.Dress.UndressStart();

				RazorEnhanced.Organizer.AddLog("Undress Engine Start...");
				RazorEnhanced.Misc.SendMessage("UNDRESS: Engine Start...");

                dressStopButton.Enabled = true;
				dressConflictCheckB.Enabled = false;
				dressExecuteButton.Enabled = false;
				undressExecuteButton.Enabled = false;
				dressAddListB.Enabled = false;
				dressRemoveListB.Enabled = false;
				dressExportListB.Enabled = false;
				dressImportListB.Enabled = false;
				dressDragDelay.Enabled = false;
			}
			else
			{
                RazorEnhanced.Dress.AddLog("You are not logged in game!");
                dressStopButton.Enabled = false;
				dressConflictCheckB.Enabled = true;
				dressExecuteButton.Enabled = true;
				undressExecuteButton.Enabled = true;
				dressAddListB.Enabled = true;
				dressRemoveListB.Enabled = true;
				dressExportListB.Enabled = true;
				dressImportListB.Enabled = true;
				dressDragDelay.Enabled = true;
			}
		}

		delegate void UndressFinishWorkCallback();

		internal void UndressFinishWork()
		{
			if (dressConflictCheckB.InvokeRequired ||
				dressExecuteButton.InvokeRequired ||
				undressExecuteButton.InvokeRequired ||
				dressAddListB.InvokeRequired ||
				dressRemoveListB.InvokeRequired ||
				organizerExportListB.InvokeRequired ||
				organizerImportListB.InvokeRequired ||
                dressStopButton.InvokeRequired ||
				organizerDragDelay.InvokeRequired)
			{
				UndressFinishWorkCallback d = new UndressFinishWorkCallback(UndressFinishWork);
				this.Invoke(d, null);
			}
			else
			{
                dressStopButton.Enabled = false;
				dressConflictCheckB.Enabled = true;
				dressExecuteButton.Enabled = true;
				undressExecuteButton.Enabled = true;
				dressAddListB.Enabled = true;
				dressRemoveListB.Enabled = true;
				dressExportListB.Enabled = true;
				dressImportListB.Enabled = true;
				dressDragDelay.Enabled = true;
			}
		}

		private void dressExecuteButton_Click(object sender, EventArgs e)
		{
            if (World.Player != null)
            {
			    RazorEnhanced.Item undressbag = RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
			    int undressbagserial = RazorEnhanced.Dress.DressBag;

			    undressbag = RazorEnhanced.Items.FindBySerial(undressbagserial);
			    if (undressbag != null)
			    {
				    RazorEnhanced.Dress.AddLog("Undress Container OK: 0x" + undressbag.Serial.ToString("X8"));
			    }
			    else
			    {
				    RazorEnhanced.Dress.AddLog("Undress Container Fail, switch packpack: 0x" + Assistant.World.Player.Backpack.Serial.Value.ToString("X8"));
				    RazorEnhanced.Dress.DressBag = Assistant.World.Player.Backpack.Serial;
			    }

                int delay = -1;
                try
                {
                    delay = Convert.ToInt32(dressDragDelay.Text);
                }
                catch
                {
                    RazorEnhanced.Dress.AddLog("ERROR: Drag item delay is not valid");
                    return;
                }
                if (delay < 0)
                {
                    RazorEnhanced.Dress.AddLog("ERROR: Drag item delay is not valid");
                    return;
                }

				RazorEnhanced.Dress.DressStart();

				RazorEnhanced.Organizer.AddLog("Dress Engine Start...");
				RazorEnhanced.Misc.SendMessage("DRESS: Engine Start...");

                dressStopButton.Enabled = true;
				dressConflictCheckB.Enabled = false;
				dressExecuteButton.Enabled = false;
				undressExecuteButton.Enabled = false;
				dressAddListB.Enabled = false;
				dressRemoveListB.Enabled = false;
				dressExportListB.Enabled = false;
				dressImportListB.Enabled = false;
				dressDragDelay.Enabled = false;
			}
			else
			{
                RazorEnhanced.Dress.AddLog("You are not logged in game!");
                dressStopButton.Enabled = false;
				dressConflictCheckB.Enabled = true;
				dressExecuteButton.Enabled = true;
				undressExecuteButton.Enabled = true;
				dressAddListB.Enabled = true;
				dressRemoveListB.Enabled = true;
				dressExportListB.Enabled = true;
				dressImportListB.Enabled = true;
				dressDragDelay.Enabled = true;
			}
		}

        private void dressStopButton_Click(object sender, EventArgs e)
        {
            RazorEnhanced.Dress.ForceStop();

            RazorEnhanced.Dress.AddLog("Dress / Undress Engine force stop...");
            RazorEnhanced.Misc.SendMessage("DRESS/UNDRESS: Engine force stop...");
            dressStopButton.Enabled = false;
            dressConflictCheckB.Enabled = true;
            dressExecuteButton.Enabled = true;
            undressExecuteButton.Enabled = true;
            dressAddListB.Enabled = true;
            dressRemoveListB.Enabled = true;
            dressExportListB.Enabled = true;
            dressImportListB.Enabled = true;
            dressDragDelay.Enabled = true;
        }
        // --------------- DRESS END ---------

        private void timerupdatestatus_Tick(object sender, EventArgs e)
        {
            UpdateRazorStatus();
        }
	}
}
