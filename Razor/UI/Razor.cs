using Assistant.Filters;
using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using AutoUpdaterDotNET;
using System.IO;
using JsonData;

namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox baseTotal;
        private System.Windows.Forms.TabPage emptyTab;
        private RazorButton skillCopySel;
        private RazorButton skillCopyAll;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TabPage toolbarTab;
        private System.Windows.Forms.TabPage skillsTab;
        private System.Windows.Forms.TabPage moreOptTab;
        private RazorCheckBox chkForceSpeechHue;
        private System.Windows.Forms.Label label3;
        private RazorTextBox txtSpellFormat;
        private RazorCheckBox chkForceSpellHue;
        private System.Windows.Forms.Label opacityLabel;
        private System.Windows.Forms.TrackBar opacity;
        private RazorCheckBox dispDelta;
        private RazorCheckBox openCorpses;
        private RazorTextBox corpseRange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage screenshotTab;
        private System.Windows.Forms.TabPage statusTab;
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
        private System.Windows.Forms.ColumnHeader skillHDRlock;
        private System.ComponentModel.IContainer components;
        private RazorCheckBox queueTargets;
        private RazorRadioButton systray;
        private RazorRadioButton taskbar;
        private System.Windows.Forms.Label label11;
        private RazorCheckBox autoStackRes;
        private RazorButton setMsgHue;
        private RazorButton setWarnHue;
        private RazorButton setSpeechHue;
        private RazorButton setBeneHue;
        private RazorButton setHarmHue;
        private RazorButton setNeuHue;
        private System.Windows.Forms.Label lblWarnHue;
        private System.Windows.Forms.Label lblMsgHue;
        private System.Windows.Forms.Label lblBeneHue;
        private System.Windows.Forms.Label lblHarmHue;
        private System.Windows.Forms.Label lblNeuHue;
        private RazorCheckBox incomingCorpse;
        private RazorCheckBox incomingMob;
        private System.Windows.Forms.TabPage enhancedFilterTab;
        private RazorCheckBox filterSnoop;
        private RazorCheckBox smartCPU;
        private RazorButton setLTHilight;
        private RazorCheckBox lthilight;
        private RazorCheckBox blockDis;
        private System.Windows.Forms.Label label12;
        private RazorComboBox imgFmt;
        private ToolTip m_Tip;

        #endregion Class Variables
        private RazorCheckBox preAOSstatbar;
        private RazorComboBox clientPrio;
        private System.Windows.Forms.Label label9;
        private TabPage scriptingTab;
        private RazorButton buttonAddScript;
        private RazorButton buttonRemoveScript;
        private RazorButton buttonScriptUp;
        private RazorButton buttonScriptDown;
        private RazorButton buttonScriptEditor;
        private Label labelStatus;
        private RazorButton razorButtonWiki;
        private List<RazorEnhanced.Organizer.OrganizerItem> organizerItemList = new List<RazorEnhanced.Organizer.OrganizerItem>();
        private List<RazorEnhanced.SellAgent.SellAgentItem> sellItemList = new List<RazorEnhanced.SellAgent.SellAgentItem>();
        private List<RazorEnhanced.BuyAgent.BuyAgentItem> buyItemList = new List<RazorEnhanced.BuyAgent.BuyAgentItem>();
        private List<RazorEnhanced.Dress.DressItemNew> dressItemList = new List<RazorEnhanced.Dress.DressItemNew>();
        private TabPage EnhancedAgent;
        private TabControl tabControl1;
        private TabPage eautoloot;
        private GroupBox groupBox13;
        private ListBox autolootLogBox;
        private Label autolootContainerLabel;
        private RazorButton autolootItemPropsB;
        private RazorButton autolootAddItemBTarget;
        private RazorButton autolootContainerButton;
        private RazorCheckBox autoLootCheckBox;
        private TabPage escavenger;
        private Label label21;
        private RazorAgentNumOnlyTextBox autoLootTextBoxDelay;
        private RazorButton autoLootButtonRemoveList;
        private RazorButton autolootButtonAddList;
        private RazorComboBox autolootListSelect;
        private Label label20;
        private RazorButton scavengerButtonRemoveList;
        private RazorButton scavengerButtonAddList;
        private RazorComboBox scavengerListSelect;
        private Label label22;
        private GroupBox groupBox12;
        private ListBox scavengerLogBox;
        private Label label23;
        private RazorAgentNumOnlyTextBox scavengerDragDelay;
        private Label scavengerContainerLabel;
        private RazorButton scavengerButtonSetContainer;
        private RazorCheckBox scavengerCheckBox;
        private RazorButton scavengerButtonEditProps;
        private RazorButton scavengerButtonAddTarget;
        private TabPage organizer;
        private GroupBox groupBox16;
        private ListBox organizerLogBox;
        private Label label27;
        private RazorAgentNumOnlyTextBox organizerDragDelay;
        private Label organizerDestinationLabel;
        private RazorButton organizerSetDestinationB;
        private Label organizerSourceLabel;
        private RazorButton organizerAddTargetB;
        private RazorButton organizerSetSourceB;
        private RazorButton organizerRemoveListB;
        private RazorButton organizerAddListB;
        private RazorComboBox organizerListSelect;
        private Label label24;
        private TabPage VendorBuy;
        private TabPage VendorSell;
        private RazorButton buyAddTargetB;
        private GroupBox groupBox18;
        private ListBox buyLogBox;
        private RazorCheckBox buyEnableCheckBox;
        private RazorButton buyRemoveListButton;
        private RazorButton buyAddListButton;
        private RazorComboBox buyListSelect;
        private Label label25;
        private RazorButton sellAddTargerButton;
        private GroupBox groupBox20;
        private ListBox sellLogBox;
        private RazorCheckBox sellEnableCheckBox;
        private RazorButton sellRemoveListButton;
        private RazorButton sellAddListButton;
        private RazorComboBox sellListSelect;
        private Label label26;
        private Label sellBagLabel;
        private RazorButton sellSetBagButton;
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
        private RazorButton dressClearListB;
        private RazorButton dressReadB;
        private Label label29;
        private RazorAgentNumOnlyTextBox dressDragDelay;
        private GroupBox groupBox21;
        private ListBox dressLogBox;
        private ListView dressListView;
        private ColumnHeader columnHeader24;
        private ColumnHeader columnHeader25;
        private ColumnHeader columnHeader26;
        private ColumnHeader columnHeader27;
        private RazorButton dressRemoveListB;
        private RazorButton dressAddListB;
        private RazorComboBox dressListSelect;
        private Label label28;
        private NotifyIcon m_NotifyIcon;
        private OpenFileDialog openFileDialogscript;
        private System.Timers.Timer m_SystemTimer;
        private RazorButton dressStopButton;
        private System.Windows.Forms.Timer timerupdatestatus;
        private TabPage friends;
        private GroupBox friendloggroupBox;
        private ListBox friendLogBox;
        private RazorCheckBox friendIncludePartyCheckBox;
        private RazorCheckBox friendAttackCheckBox;
        private RazorCheckBox friendPartyCheckBox;
        private ListView friendlistView;
        private ColumnHeader columnHeader28;
        private ColumnHeader columnHeader29;
        private ColumnHeader columnHeader30;
        private RazorButton friendButtonRemoveList;
        private RazorButton friendButtonAddList;
        private RazorComboBox friendListSelect;
        private Label labelfriend;
        private GroupBox friendGroupBox;
        private RazorButton friendAddTargetButton;
        private RazorButton friendRemoveButton;
        private RazorButton friendAddButton;
        private TabPage restock;
        private GroupBox groupBox2;
        private ListBox restockLogBox;
        private Label label13;
        private RazorAgentNumOnlyTextBox restockDragDelay;
        private Label restockDestinationLabel;
        private RazorButton restockSetDestinationButton;
        private Label restockSourceLabel;
        private RazorButton restockAddTargetButton;
        private RazorButton restockSetSourceButton;
        private RazorButton restockRemoveListB;
        private RazorButton restockAddListB;
        private RazorComboBox restockListSelect;
        private Label label7;
        private TabPage bandageheal;
        private GroupBox groupBox6;
        private RazorCheckBox bandagehealcountdownCheckBox;
        private RazorCheckBox bandagehealhiddedCheckBox;
        private RazorCheckBox bandagehealmortalCheckBox;
        private RazorCheckBox bandagehealpoisonCheckBox;
        private Label label33;
        private RazorAgentNumOnlyTextBox bandagehealhpTextBox;
        private Label label32;
        private RazorCheckBox bandagehealdexformulaCheckBox;
        private RazorAgentNumHexTextBox bandagehealcustomcolorTextBox;
        private Label label30;
        private RazorAgentNumHexTextBox bandagehealcustomIDTextBox;
        private Label label19;
        private RazorCheckBox bandagehealcustomCheckBox;
        private Label bandagehealtargetLabel;
        private Label label15;
        private RazorButton bandagehealsettargetButton;
        private RazorComboBox bandagehealtargetComboBox;
        private Label label14;
        private RazorCheckBox bandagehealenableCheckBox;
        private GroupBox groupBox5;
        private ListBox bandagehealLogBox;
        private RazorCheckBox rememberPwds;
        private RazorCheckBox gameSize;
        private RazorTextBox forceSizeX;
        private RazorTextBox forceSizeY;
        private RazorCheckBox chkStealth;
        private RazorCheckBox autoOpenDoors;
        private RazorCheckBox spellUnequip;
        private RazorCheckBox potionEquip;
        private Label label17;
        private RazorComboBox msglvl;
        private RazorCheckBox actionStatusMsg;
        private RazorCheckBox QueueActions;
        private RazorTextBox txtObjDelay;
        private Label label5;
        private Label label6;
        private RazorCheckBox smartLT;
        private RazorCheckBox rangeCheckLT;
        private RazorTextBox ltRange;
        private Label label8;
        private RazorCheckBox showtargtext;
        private RazorCheckBox showHealthOH;
        private RazorTextBox healthFmt;
        private Label label10;
        private RazorCheckBox chkPartyOverhead;
        private GroupBox groupBox23;
        private RazorCheckBox mobfilterCheckBox;
        private GroupBox groupBox10;
        private Label autocarverbladeLabel;
        private Label label34;
        private RazorButton autocarverrazorButton;
        private RazorCheckBox autocarverCheckBox;
        private GroupBox groupBox9;
        private Label bonebladeLabel;
        private Label label16;
        private RazorButton boneCutterrazorButton;
        private RazorCheckBox bonecutterCheckBox;
        private RazorCheckBox showstaticfieldCheckBox;
        private RazorCheckBox flagsHighlightCheckBox;
        private RazorCheckBox highlighttargetCheckBox;
        private GroupBox groupBox24;
        private RazorCheckBox blockpartyinviteCheckBox;
        private RazorCheckBox blocktraderequestCheckBox;
        private RazorButton openToolBarButton;
        private GroupBox groupBox25;
        private RazorCheckBox lockToolBarCheckBox;
        private RazorCheckBox autoopenToolBarCheckBox;
        private Label locationToolBarLabel;
        private RazorButton closeToolBarButton;
        private GroupBox groupBox26;
        private Label label38;
        private RazorTextBox toolboxcountNameTextBox;
        private Label label37;
        private RazorButton toolboxcountClearButton;
        private RazorButton toolboxcountTargetButton;
        private RazorAgentNumOnlyTextBox toolboxcountWarningTextBox;
        private Label label36;
        private RazorCheckBox toolboxcountHueWarningCheckBox;
        private RazorTextBox toolboxcountHueTextBox;
        private Label label35;
        private RazorAgentNumHexTextBox toolboxcountGraphTextBox;
        private Label label18;
        private RazorComboBox toolboxcountComboBox;
        private TabPage enhancedHotKeytabPage;
        private TreeView hotkeytreeView;
        private RazorHotKeyTextBox hotkeytextbox;
        private GroupBox groupBox27;
        private RazorButton hotkeyClearButton;
        private RazorButton hotkeySetButton;
        private Label label39;
        private GroupBox groupBox28;
        private RazorButton hotkeyMasterClearButton;
        private RazorButton hotkeyMasterSetButton;
        private Label label42;
        private Label hotkeyKeyMasterLabel;
        private RazorHotKeyTextBox hotkeyKeyMasterTextBox;
        private Label hotkeyStatusLabel;
        private RazorCheckBox hotkeypassCheckBox;
        private GroupBox groupBox8;
        private RazorButton hotkeyMDisableButton;
        private RazorButton hotkeyMEnableButton;
        private GroupBox groupBox29;
        private RazorButton profilesDeleteButton;
        private RazorButton profilesAddButton;
        private RazorComboBox profilesComboBox;
        private RazorButton profilesCloneButton;
        private RazorButton profilesRenameButton;
        private RazorButton profilesUnlinkButton;
        private RazorButton profilesLinkButton;
        private Label profilelinklabel;

        private bool m_CanClose = true;
        private GroupBox groupBox32;
        private RazorAgentNumOnlyTextBox remountedelay;
        private RazorAgentNumOnlyTextBox remountdelay;
        private Label label48;
        private Label label40;
        private Label remountseriallabel;
        private Label label47;
        private RazorButton remountsetbutton;
        private RazorCheckBox remountcheckbox;
        private Button buttonScriptPlay;
        private Button buttonScriptStop;
        private TextBox textBoxDelay;
        private RazorCheckBox showheadtargetCheckBox;
        private RazorCheckBox notshowlauncher;
        private RazorCheckBox blockhealpoisonCheckBox;
        private GroupBox groupBox4;
        private Label label43;
        private RazorComboBox toolboxsizeComboBox;
        private Label label41;
        private RazorCheckBox showfollowerToolBarCheckBox;
        private RazorCheckBox showweightToolBarCheckBox;
        private RazorCheckBox showmanaToolBarCheckBox;
        private RazorCheckBox showstaminaToolBarCheckBox;
        private RazorCheckBox showhitsToolBarCheckBox;
        private RazorComboBox toolboxstyleComboBox;
        private Label label2;
        private Label toolbarslot_label;
        private RazorCheckBox colorflagsHighlightCheckBox;
        private RazorCheckBox blockchivalryhealCheckBox;
        private RazorCheckBox blockbighealCheckBox;
        private RazorCheckBox blockminihealCheckBox;
        private ScriptListView scriptlistView;
        private ColumnHeader filename;
        private ColumnHeader status;
        private ColumnHeader loop;
        private ColumnHeader wait;
        private ColumnHeader hotkey;
        private ColumnHeader heypass;
        private ColumnHeader columnHeader62;
        private GroupBox groupBox30;
        private RazorCheckBox scriptwaitmodecheckbox;
        private RazorCheckBox scriptloopmodecheckbox;
        private Label scriptfilelabel;
        private GroupBox groupBox31;
        private RazorCheckBox showagentmessageCheckBox;
        private RazorCheckBox showmessagefieldCheckBox;
        private RazorCheckBox showscriptmessageCheckBox;
        private Button buttonScriptRefresh;
        private RazorButton FriendGuildRemoveButton;
        private RazorButton FriendGuildAddButton;
        private ListView friendguildListView;
        private ColumnHeader columnHeader63;
        private ColumnHeader columnHeader64;
        private RazorCheckBox MINfriendCheckBox;
        private RazorCheckBox COMfriendCheckBox;
        private RazorCheckBox TBfriendCheckBox;
        private RazorCheckBox SLfriendCheckBox;
        private GroupBox groupBox34;
        private GroupBox groupBox33;
        private RazorButton toolbarremoveslotButton;
        private RazorButton toolbaraddslotButton;
        private RazorCheckBox autoLootnoopenCheckBox;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private GroupBox groupBox37;
        private RazorButton gridhslotremove_button;
        private RazorButton gridhslotadd_button;
        private Label gridhslot_textbox;
        private Label label53;
        private RazorButton gridvslotremove_button;
        private RazorButton gridvslotadd_button;
        private Label gridvslot_textbox;
        private Label label49;
        private GroupBox groupBox36;
        private RazorComboBox gridborder_ComboBox;
        private Label label44;
        private RazorComboBox gridspell_ComboBox;
        private Label label52;
        private RazorComboBox gridgroup_ComboBox;
        private Label label51;
        private Label label45;
        private RazorComboBox gridslot_ComboBox;
        private GroupBox groupBox35;
        private RazorCheckBox gridlock_CheckBox;
        private RazorCheckBox gridopenlogin_CheckBox;
        private Label gridlocation_label;
        private RazorButton gridclose_button;
        private RazorButton gridopen_button;
        private GroupBox groupBox39;
        private TrackBar toolbar_trackBar;
        private Label toolbar_opacity_label;
        private GroupBox groupBox38;
        private TrackBar spellgrid_trackBar;
        private Label spellgrid_opacity_label;
        private TabControl toolbarstab;
        private GroupBox uomodgroupbox;
        private RazorCheckBox uomodFPSCheckBox;
        private RazorCheckBox uomodpaperdollCheckBox;
        private RazorCheckBox uomodglobalsoundCheckBox;
        private Label labelHotride;
        private RazorAgentNumOnlyTextBox bandagehealmaxrangeTextBox;
        private Label label46;
        private RazorAgentNumOnlyTextBox bandagehealdelayTextBox;
        private Label label31;
        private RazorButton openchangelogButton;
        private RazorButton discordrazorButton;
        private DataGridView vendorsellGridView;
        private ContextMenuStrip datagridMenuStrip;
        private ToolStripMenuItem deleteRowToolStripMenuItem;
        private Label label50;
        private GroupBox groupBox19;
        private DataGridView scavengerdataGridView;
        private DataGridViewCheckBoxColumn VendorSellX;
        private DataGridViewTextBoxColumn VendorSellItemName;
        private DataGridViewTextBoxColumn VendorSellGraphics;
        private DataGridViewTextBoxColumn VendorSellAmount;
        private DataGridViewTextBoxColumn VendorSellColor;
        private GroupBox groupBox41;
        private Label label54;
        private DataGridViewCheckBoxColumn ScavengerX;
        private DataGridViewTextBoxColumn ScavengerItemName;
        private DataGridViewTextBoxColumn ScavenerGraphics;
        private DataGridViewTextBoxColumn ScavengerColor;
        private DataGridViewTextBoxColumn ScavengerProp;
        private DataGridView autolootdataGridView;
        private GroupBox groupBox14;
        private Label label55;
        private DataGridView vendorbuydataGridView;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView organizerdataGridView;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private Button organizerStopButton;
        private GroupBox groupBox11;
        private Label label57;
        private Label label56;
        private Button organizerExecuteButton;
        private DataGridView restockdataGridView;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private GroupBox groupBox3;
        private Label label59;
        private Label label58;
        private Button restockExecuteButton;
        private Button restockStopButton;
        private Label label60;
        private RazorAgentNumOnlyTextBox autoLootTextBoxMaxRange;
        private Label label61;
        private RazorAgentNumOnlyTextBox scavengerRange;
        private RazorCheckBox hiddedAutoOpenDoors;
        private RazorCheckBox uo3dEquipUnEquip;
        private RazorCheckBox nosearchpouches;
        private RazorCheckBox autosearchcontainers;
        private TabPage videoTab;
        private RazorTextBox videoPathTextBox;
        private RazorButton videoPathButton;
        private ListBox videolistBox;
        private GroupBox groupBox40;
        private GroupBox videosettinggroupBox;
        private Button videorecbutton;
        private Button videostopbutton;
        private Label label62;
        private RazorTextBox videoFPSTextBox;
        private GroupBox groupBox15;
        private Label videoRecStatuslabel;
        private Label label64;
        private RazorComboBox videoCodecComboBox;
        private Label label63;
        private RazorCheckBox scriptautostartcheckbox;
        private ColumnHeader autostart;
        private Accord.Controls.VideoSourcePlayer videoSourcePlayer;
        private RazorComboBox gridscript_ComboBox;
        private Label label65;
        private TabPage DPStabPage;
        private RazorButton DPSMeterStopButton;
        private RazorButton DPSMeterStartButton;
        private RazorButton DPSMeterClearButton;
        private Label DPSMeterStatusLabel;
        private Label label67;
        private RazorButton DPSMeterPauseButton;
        private DataGridView DpsMeterGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private GroupBox filtergroup;
        private RazorButton DPSMeterApplyFilterButton;
        private RazorTextBox DPSmetername;
        private Label label70;
        private RazorAgentNumHexTextBox DPSmeterserial;
        private Label label69;
        private Label label68;
        private RazorAgentNumOnlyTextBox DPSmetermaxdamage;
        private Label label66;
        private RazorAgentNumOnlyTextBox DPSmetermindamage;
        private RazorButton DPSMeterClearFilterButton;
        private System.Drawing.Point windowspt;

        // GumpInspector Flag
        internal bool GumpInspectorEnable = false;

        private RazorCheckBox colorflagsselfHighlightCheckBox;
        private ContextMenuStrip scriptgridMenuStrip;
        private ToolStripMenuItem modifyToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripMenuItem flagsToolStripMenuItem;
        private ToolStripMenuItem loopModeToolStripMenuItem;
        private ToolStripMenuItem waitBeforeInterruptToolStripMenuItem;
        private ToolStripMenuItem autoStartAtLoginToolStripMenuItem;
        private ToolStripMenuItem playToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.Timer timertitlestatusbar;
        private RazorCheckBox chknorunStealth;
        private RazorCheckBox filterPoison;
        private Label label71;
        private GroupBox groupBox17;
        private Label label72;
        private RazorTextBox enhancedmappathTextBox;
        private Button setpathmapbutton;
        private OpenFileDialog openmaplocation;
        private RazorCheckBox autolootautostartCheckBox;
        private RazorCheckBox scavengerautostartCheckBox;
        private RazorCheckBox filterNPC;
        private DataGridView graphfilterdatagrid;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private RazorCheckBox scripterrorlogCheckBox;
        private RazorButton autoLootButtonListClone;
        private RazorButton scavengerButtonClone;
        private RazorButton organizerCloneListB;
        private RazorButton buyCloneButton;
        private RazorButton sellCloneListButton;
        private RazorButton restockCloneListB;
        private GroupBox groupBox42;
        private RazorTextBox scriptSearchTextBox;
        private RazorButton buttonScriptEditorNew;
        private GroupBox groupBox43;
        private RazorButton targetaddButton;
        private RazorTextBox targetaddTextBox;
        private ListBox targetlistBox;
        private GroupBox groupBox45;
        private DataGridView targethueGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private RazorCheckBox targetcoloCheckBox;
        private GroupBox groupBox44;
        private DataGridView targetbodydataGridView;
        private RazorCheckBox targetbodyCheckBox;
        private GroupBox groupBox46;
        private GroupBox groupBox47;
        private RazorRadioButton paralizedBoth;
        private RazorRadioButton paralizedOff;
        private RazorRadioButton paralizedOn;
        private GroupBox groupBox49;
        private RazorRadioButton friendOn;
        private RazorRadioButton friendBoth;
        private RazorRadioButton friendOff;
        private GroupBox groupBox50;
        private RazorRadioButton warmodeOn;
        private RazorRadioButton warmodeBoth;
        private RazorRadioButton warmodeOff;
        private GroupBox groupBox51;
        private RazorRadioButton ghostOn;
        private RazorRadioButton ghostBoth;
        private RazorRadioButton ghostOff;
        private GroupBox groupBox52;
        private RazorRadioButton humanOn;
        private RazorRadioButton humanOff;
        private RazorRadioButton humanBoth;
        private GroupBox groupBox53;
        private RazorRadioButton blessedOn;
        private RazorRadioButton blessedOff;
        private RazorRadioButton blessedBoth;
        private GroupBox groupBox54;
        private RazorRadioButton poisonedOn;
        private RazorRadioButton poisonedOff;
        private RazorRadioButton poisonedBoth;
        private GroupBox groupBox48;
        private Label label73;
        private Label label74;
        private RazorTextBox targetRangeMaxTextBox;
        private Label label75;
        private RazorTextBox targetRangeMinTextBox;
        private GroupBox groupBox55;
        private RazorTextBox targetNameTextBox;
        private GroupBox groupBox57;
        private GroupBox groupBox56;
        private RazorComboBox targetSelectorComboBox;
        private RazorCheckBox targetBlueCheckBox;
        private RazorCheckBox targetYellowCheckBox;
        private RazorCheckBox targetRedCheckBox;
        private RazorCheckBox targetOrangeCheckBox;
        private RazorCheckBox targetCriminalCheckBox;
        private RazorCheckBox targetGreyCheckBox;
        private RazorCheckBox targetGreenCheckBox;
        private RazorButton targetsaveButton;
        private RazorButton targetTestButton;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private RazorButton targetremoveButton;
        private Label label76;
        private RazorButton targetChoseHue;
        private RazorButton targetChoseBody;
        private RazorCheckBox bandagehealAutostartCheckBox;
        private RazorCheckBox bandagehealusetarget;
        private RazorButton paypalButton;
        private RazorCheckBox bandagehealusetext;
        private RazorTextBox bandagehealusetextSelfContent;
        private RazorTextBox bandagehealusetextContent;
        private Label label77;
        private Label label78;
        private RazorButton advertisementLink;
        private PictureBox advertisement;
        private DataGridViewCheckBoxColumn AutolootColumnX;
        private DataGridViewTextBoxColumn AutolootColumnItemName;
        private DataGridViewTextBoxColumn AutolootColumnItemID;
        private DataGridViewTextBoxColumn AutolootColumnColor;
        private DataGridViewTextBoxColumn LootBagColumnID;
        private DataGridViewTextBoxColumn AutolootColumnProps;
        private RazorCheckBox allowHiddenLooting;
        private RazorCheckBox druidClericPackets;
		private RazorCheckBox buyCompareNameCheckBox;
        private RazorCheckBox showtitheToolBarCheckBox;
        private Label label79;
        private RazorCheckBox scriptshowStartStopCheckBox;

        internal MainForm()
        {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabs = new System.Windows.Forms.TabControl();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.label79 = new System.Windows.Forms.Label();
            this.paypalButton = new RazorEnhanced.UI.RazorButton();
            this.openchangelogButton = new RazorEnhanced.UI.RazorButton();
            this.notshowlauncher = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.profilesCloneButton = new RazorEnhanced.UI.RazorButton();
            this.profilesRenameButton = new RazorEnhanced.UI.RazorButton();
            this.profilesUnlinkButton = new RazorEnhanced.UI.RazorButton();
            this.profilesLinkButton = new RazorEnhanced.UI.RazorButton();
            this.profilelinklabel = new System.Windows.Forms.Label();
            this.profilesDeleteButton = new RazorEnhanced.UI.RazorButton();
            this.profilesAddButton = new RazorEnhanced.UI.RazorButton();
            this.profilesComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.forceSizeY = new RazorEnhanced.UI.RazorTextBox();
            this.forceSizeX = new RazorEnhanced.UI.RazorTextBox();
            this.gameSize = new RazorEnhanced.UI.RazorCheckBox();
            this.rememberPwds = new RazorEnhanced.UI.RazorCheckBox();
            this.clientPrio = new RazorEnhanced.UI.RazorComboBox();
            this.systray = new RazorEnhanced.UI.RazorRadioButton();
            this.taskbar = new RazorEnhanced.UI.RazorRadioButton();
            this.smartCPU = new RazorEnhanced.UI.RazorCheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.opacity = new System.Windows.Forms.TrackBar();
            this.alwaysTop = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filters = new System.Windows.Forms.CheckedListBox();
            this.opacityLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.moreOptTab = new System.Windows.Forms.TabPage();
            this.druidClericPackets = new RazorEnhanced.UI.RazorCheckBox();
            this.allowHiddenLooting = new RazorEnhanced.UI.RazorCheckBox();
            this.filterNPC = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.setpathmapbutton = new System.Windows.Forms.Button();
            this.label72 = new System.Windows.Forms.Label();
            this.enhancedmappathTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblHarmHue = new System.Windows.Forms.Label();
            this.lblNeuHue = new System.Windows.Forms.Label();
            this.lblBeneHue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWarnHue = new System.Windows.Forms.Label();
            this.lblMsgHue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.filterPoison = new RazorEnhanced.UI.RazorCheckBox();
            this.chknorunStealth = new RazorEnhanced.UI.RazorCheckBox();
            this.nosearchpouches = new RazorEnhanced.UI.RazorCheckBox();
            this.autosearchcontainers = new RazorEnhanced.UI.RazorCheckBox();
            this.uo3dEquipUnEquip = new RazorEnhanced.UI.RazorCheckBox();
            this.hiddedAutoOpenDoors = new RazorEnhanced.UI.RazorCheckBox();
            this.chkPartyOverhead = new RazorEnhanced.UI.RazorCheckBox();
            this.healthFmt = new RazorEnhanced.UI.RazorTextBox();
            this.showHealthOH = new RazorEnhanced.UI.RazorCheckBox();
            this.showtargtext = new RazorEnhanced.UI.RazorCheckBox();
            this.ltRange = new RazorEnhanced.UI.RazorTextBox();
            this.rangeCheckLT = new RazorEnhanced.UI.RazorCheckBox();
            this.smartLT = new RazorEnhanced.UI.RazorCheckBox();
            this.txtObjDelay = new RazorEnhanced.UI.RazorTextBox();
            this.QueueActions = new RazorEnhanced.UI.RazorCheckBox();
            this.actionStatusMsg = new RazorEnhanced.UI.RazorCheckBox();
            this.msglvl = new RazorEnhanced.UI.RazorComboBox();
            this.potionEquip = new RazorEnhanced.UI.RazorCheckBox();
            this.spellUnequip = new RazorEnhanced.UI.RazorCheckBox();
            this.autoOpenDoors = new RazorEnhanced.UI.RazorCheckBox();
            this.chkStealth = new RazorEnhanced.UI.RazorCheckBox();
            this.preAOSstatbar = new RazorEnhanced.UI.RazorCheckBox();
            this.setLTHilight = new RazorEnhanced.UI.RazorButton();
            this.lthilight = new RazorEnhanced.UI.RazorCheckBox();
            this.filterSnoop = new RazorEnhanced.UI.RazorCheckBox();
            this.corpseRange = new RazorEnhanced.UI.RazorTextBox();
            this.incomingCorpse = new RazorEnhanced.UI.RazorCheckBox();
            this.incomingMob = new RazorEnhanced.UI.RazorCheckBox();
            this.setHarmHue = new RazorEnhanced.UI.RazorButton();
            this.setNeuHue = new RazorEnhanced.UI.RazorButton();
            this.setBeneHue = new RazorEnhanced.UI.RazorButton();
            this.setSpeechHue = new RazorEnhanced.UI.RazorButton();
            this.setWarnHue = new RazorEnhanced.UI.RazorButton();
            this.setMsgHue = new RazorEnhanced.UI.RazorButton();
            this.autoStackRes = new RazorEnhanced.UI.RazorCheckBox();
            this.queueTargets = new RazorEnhanced.UI.RazorCheckBox();
            this.spamFilter = new RazorEnhanced.UI.RazorCheckBox();
            this.openCorpses = new RazorEnhanced.UI.RazorCheckBox();
            this.blockDis = new RazorEnhanced.UI.RazorCheckBox();
            this.txtSpellFormat = new RazorEnhanced.UI.RazorTextBox();
            this.chkForceSpellHue = new RazorEnhanced.UI.RazorCheckBox();
            this.chkForceSpeechHue = new RazorEnhanced.UI.RazorCheckBox();
            this.enhancedFilterTab = new System.Windows.Forms.TabPage();
            this.uomodgroupbox = new System.Windows.Forms.GroupBox();
            this.uomodpaperdollCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.uomodglobalsoundCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.uomodFPSCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox32 = new System.Windows.Forms.GroupBox();
            this.remountedelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.remountdelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.remountseriallabel = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.remountsetbutton = new RazorEnhanced.UI.RazorButton();
            this.remountcheckbox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.colorflagsselfHighlightCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showagentmessageCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showmessagefieldCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.colorflagsHighlightCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.blockchivalryhealCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.blockbighealCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.blockminihealCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.blockhealpoisonCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showheadtargetCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.blockpartyinviteCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.blocktraderequestCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.highlighttargetCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.flagsHighlightCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showstaticfieldCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.graphfilterdatagrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobfilterCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.autocarverbladeLabel = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.autocarverrazorButton = new RazorEnhanced.UI.RazorButton();
            this.autocarverCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.bonebladeLabel = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.boneCutterrazorButton = new RazorEnhanced.UI.RazorButton();
            this.bonecutterCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.scriptingTab = new System.Windows.Forms.TabPage();
            this.scriptshowStartStopCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox42 = new System.Windows.Forms.GroupBox();
            this.scriptSearchTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.buttonScriptEditorNew = new RazorEnhanced.UI.RazorButton();
            this.buttonScriptRefresh = new System.Windows.Forms.Button();
            this.buttonAddScript = new RazorEnhanced.UI.RazorButton();
            this.buttonRemoveScript = new RazorEnhanced.UI.RazorButton();
            this.buttonScriptDown = new RazorEnhanced.UI.RazorButton();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.buttonScriptUp = new RazorEnhanced.UI.RazorButton();
            this.buttonScriptEditor = new RazorEnhanced.UI.RazorButton();
            this.buttonScriptStop = new System.Windows.Forms.Button();
            this.buttonScriptPlay = new System.Windows.Forms.Button();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.scriptautostartcheckbox = new RazorEnhanced.UI.RazorCheckBox();
            this.scriptwaitmodecheckbox = new RazorEnhanced.UI.RazorCheckBox();
            this.scriptloopmodecheckbox = new RazorEnhanced.UI.RazorCheckBox();
            this.scriptfilelabel = new System.Windows.Forms.Label();
            this.scripterrorlogCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showscriptmessageCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.scriptlistView = new RazorEnhanced.UI.ScriptListView();
            this.columnHeader62 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.filename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.loop = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.autostart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wait = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hotkey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.heypass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EnhancedAgent = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.eautoloot = new System.Windows.Forms.TabPage();
            this.autoLootButtonListClone = new RazorEnhanced.UI.RazorButton();
            this.autolootautostartCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.label60 = new System.Windows.Forms.Label();
            this.autoLootTextBoxMaxRange = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.autolootItemPropsB = new RazorEnhanced.UI.RazorButton();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label55 = new System.Windows.Forms.Label();
            this.autolootContainerLabel = new System.Windows.Forms.Label();
            this.autolootContainerButton = new RazorEnhanced.UI.RazorButton();
            this.autolootAddItemBTarget = new RazorEnhanced.UI.RazorButton();
            this.autolootdataGridView = new System.Windows.Forms.DataGridView();
            this.AutolootColumnX = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AutolootColumnItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutolootColumnItemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutolootColumnColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LootBagColumnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutolootColumnProps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.autoLootnoopenCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.autoLootTextBoxDelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.autoLootButtonRemoveList = new RazorEnhanced.UI.RazorButton();
            this.autolootButtonAddList = new RazorEnhanced.UI.RazorButton();
            this.autolootListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.autolootLogBox = new System.Windows.Forms.ListBox();
            this.autoLootCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.escavenger = new System.Windows.Forms.TabPage();
            this.scavengerButtonClone = new RazorEnhanced.UI.RazorButton();
            this.scavengerautostartCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.label61 = new System.Windows.Forms.Label();
            this.groupBox41 = new System.Windows.Forms.GroupBox();
            this.label54 = new System.Windows.Forms.Label();
            this.scavengerContainerLabel = new System.Windows.Forms.Label();
            this.scavengerButtonSetContainer = new RazorEnhanced.UI.RazorButton();
            this.scavengerdataGridView = new System.Windows.Forms.DataGridView();
            this.ScavengerX = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ScavengerItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScavenerGraphics = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScavengerColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScavengerProp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.scavengerLogBox = new System.Windows.Forms.ListBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.scavengerRange = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.scavengerButtonEditProps = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonAddTarget = new RazorEnhanced.UI.RazorButton();
            this.scavengerDragDelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.scavengerCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.scavengerButtonRemoveList = new RazorEnhanced.UI.RazorButton();
            this.scavengerButtonAddList = new RazorEnhanced.UI.RazorButton();
            this.scavengerListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.organizer = new System.Windows.Forms.TabPage();
            this.organizerCloneListB = new RazorEnhanced.UI.RazorButton();
            this.organizerExecuteButton = new System.Windows.Forms.Button();
            this.organizerStopButton = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label57 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.organizerSetSourceB = new RazorEnhanced.UI.RazorButton();
            this.organizerSetDestinationB = new RazorEnhanced.UI.RazorButton();
            this.organizerSourceLabel = new System.Windows.Forms.Label();
            this.organizerDestinationLabel = new System.Windows.Forms.Label();
            this.organizerdataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.organizerLogBox = new System.Windows.Forms.ListBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.organizerAddTargetB = new RazorEnhanced.UI.RazorButton();
            this.organizerDragDelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.organizerRemoveListB = new RazorEnhanced.UI.RazorButton();
            this.organizerAddListB = new RazorEnhanced.UI.RazorButton();
            this.organizerListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.VendorBuy = new System.Windows.Forms.TabPage();
            this.buyCompareNameCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.buyCloneButton = new RazorEnhanced.UI.RazorButton();
            this.vendorbuydataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.buyLogBox = new System.Windows.Forms.ListBox();
            this.label25 = new System.Windows.Forms.Label();
            this.buyAddTargetB = new RazorEnhanced.UI.RazorButton();
            this.buyEnableCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.buyRemoveListButton = new RazorEnhanced.UI.RazorButton();
            this.buyAddListButton = new RazorEnhanced.UI.RazorButton();
            this.buyListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.VendorSell = new System.Windows.Forms.TabPage();
            this.sellCloneListButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.sellSetBagButton = new RazorEnhanced.UI.RazorButton();
            this.label50 = new System.Windows.Forms.Label();
            this.sellBagLabel = new System.Windows.Forms.Label();
            this.vendorsellGridView = new System.Windows.Forms.DataGridView();
            this.VendorSellX = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.VendorSellItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorSellGraphics = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorSellAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorSellColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.sellLogBox = new System.Windows.Forms.ListBox();
            this.label26 = new System.Windows.Forms.Label();
            this.sellAddTargerButton = new RazorEnhanced.UI.RazorButton();
            this.sellEnableCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.sellRemoveListButton = new RazorEnhanced.UI.RazorButton();
            this.sellAddListButton = new RazorEnhanced.UI.RazorButton();
            this.sellListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.Dress = new System.Windows.Forms.TabPage();
            this.dressStopButton = new RazorEnhanced.UI.RazorButton();
            this.dressConflictCheckB = new RazorEnhanced.UI.RazorCheckBox();
            this.dressBagLabel = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.dressAddTargetB = new RazorEnhanced.UI.RazorButton();
            this.dressAddManualB = new RazorEnhanced.UI.RazorButton();
            this.dressRemoveB = new RazorEnhanced.UI.RazorButton();
            this.dressClearListB = new RazorEnhanced.UI.RazorButton();
            this.dressReadB = new RazorEnhanced.UI.RazorButton();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.dressLogBox = new System.Windows.Forms.ListBox();
            this.dressSetBagB = new RazorEnhanced.UI.RazorButton();
            this.undressExecuteButton = new RazorEnhanced.UI.RazorButton();
            this.dressExecuteButton = new RazorEnhanced.UI.RazorButton();
            this.dressDragDelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.dressListView = new System.Windows.Forms.ListView();
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label28 = new System.Windows.Forms.Label();
            this.dressRemoveListB = new RazorEnhanced.UI.RazorButton();
            this.dressAddListB = new RazorEnhanced.UI.RazorButton();
            this.dressListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.friends = new System.Windows.Forms.TabPage();
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.FriendGuildAddButton = new RazorEnhanced.UI.RazorButton();
            this.FriendGuildRemoveButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox33 = new System.Windows.Forms.GroupBox();
            this.MINfriendCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.SLfriendCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.TBfriendCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.COMfriendCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.friendguildListView = new System.Windows.Forms.ListView();
            this.columnHeader63 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader64 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.friendGroupBox = new System.Windows.Forms.GroupBox();
            this.friendAddTargetButton = new RazorEnhanced.UI.RazorButton();
            this.friendRemoveButton = new RazorEnhanced.UI.RazorButton();
            this.friendAddButton = new RazorEnhanced.UI.RazorButton();
            this.friendloggroupBox = new System.Windows.Forms.GroupBox();
            this.friendLogBox = new System.Windows.Forms.ListBox();
            this.friendIncludePartyCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.friendAttackCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.friendPartyCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.friendlistView = new System.Windows.Forms.ListView();
            this.columnHeader28 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader29 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader30 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelfriend = new System.Windows.Forms.Label();
            this.friendButtonRemoveList = new RazorEnhanced.UI.RazorButton();
            this.friendButtonAddList = new RazorEnhanced.UI.RazorButton();
            this.friendListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.restock = new System.Windows.Forms.TabPage();
            this.restockCloneListB = new RazorEnhanced.UI.RazorButton();
            this.restockExecuteButton = new System.Windows.Forms.Button();
            this.restockStopButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.restockSetSourceButton = new RazorEnhanced.UI.RazorButton();
            this.restockSourceLabel = new System.Windows.Forms.Label();
            this.restockDestinationLabel = new System.Windows.Forms.Label();
            this.restockSetDestinationButton = new RazorEnhanced.UI.RazorButton();
            this.restockdataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.restockLogBox = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.restockAddTargetButton = new RazorEnhanced.UI.RazorButton();
            this.restockDragDelay = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.restockRemoveListB = new RazorEnhanced.UI.RazorButton();
            this.restockAddListB = new RazorEnhanced.UI.RazorButton();
            this.restockListSelect = new RazorEnhanced.UI.RazorComboBox();
            this.bandageheal = new System.Windows.Forms.TabPage();
            this.bandagehealAutostartCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label78 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.bandagehealusetextContent = new RazorEnhanced.UI.RazorTextBox();
            this.bandagehealusetextSelfContent = new RazorEnhanced.UI.RazorTextBox();
            this.bandagehealusetext = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealusetarget = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealmaxrangeTextBox = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.bandagehealcountdownCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealhiddedCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealmortalCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealpoisonCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.bandagehealhpTextBox = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.bandagehealdelayTextBox = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.bandagehealdexformulaCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealcustomcolorTextBox = new RazorEnhanced.UI.RazorAgentNumHexTextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.bandagehealcustomIDTextBox = new RazorEnhanced.UI.RazorAgentNumHexTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.bandagehealcustomCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.bandagehealtargetLabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.bandagehealsettargetButton = new RazorEnhanced.UI.RazorButton();
            this.bandagehealtargetComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.bandagehealLogBox = new System.Windows.Forms.ListBox();
            this.bandagehealenableCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.toolbarTab = new System.Windows.Forms.TabPage();
            this.toolbarstab = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox39 = new System.Windows.Forms.GroupBox();
            this.toolbar_trackBar = new System.Windows.Forms.TrackBar();
            this.toolbar_opacity_label = new System.Windows.Forms.Label();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.lockToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.autoopenToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.locationToolBarLabel = new System.Windows.Forms.Label();
            this.closeToolBarButton = new RazorEnhanced.UI.RazorButton();
            this.openToolBarButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.showtitheToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.toolbarremoveslotButton = new RazorEnhanced.UI.RazorButton();
            this.toolbaraddslotButton = new RazorEnhanced.UI.RazorButton();
            this.toolbarslot_label = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.toolboxsizeComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.showfollowerToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showweightToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showmanaToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showstaminaToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.showhitsToolBarCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.toolboxstyleComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label38 = new System.Windows.Forms.Label();
            this.toolboxcountNameTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.toolboxcountClearButton = new RazorEnhanced.UI.RazorButton();
            this.toolboxcountTargetButton = new RazorEnhanced.UI.RazorButton();
            this.toolboxcountWarningTextBox = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.toolboxcountHueWarningCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.toolboxcountHueTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.toolboxcountGraphTextBox = new RazorEnhanced.UI.RazorAgentNumHexTextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.toolboxcountComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox38 = new System.Windows.Forms.GroupBox();
            this.spellgrid_trackBar = new System.Windows.Forms.TrackBar();
            this.spellgrid_opacity_label = new System.Windows.Forms.Label();
            this.groupBox37 = new System.Windows.Forms.GroupBox();
            this.gridhslotremove_button = new RazorEnhanced.UI.RazorButton();
            this.gridhslotadd_button = new RazorEnhanced.UI.RazorButton();
            this.gridhslot_textbox = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.gridvslotremove_button = new RazorEnhanced.UI.RazorButton();
            this.gridvslotadd_button = new RazorEnhanced.UI.RazorButton();
            this.gridvslot_textbox = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.groupBox36 = new System.Windows.Forms.GroupBox();
            this.gridscript_ComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label65 = new System.Windows.Forms.Label();
            this.gridborder_ComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label44 = new System.Windows.Forms.Label();
            this.gridspell_ComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label52 = new System.Windows.Forms.Label();
            this.gridgroup_ComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label51 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.gridslot_ComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.groupBox35 = new System.Windows.Forms.GroupBox();
            this.gridlock_CheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.gridopenlogin_CheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.gridlocation_label = new System.Windows.Forms.Label();
            this.gridclose_button = new RazorEnhanced.UI.RazorButton();
            this.gridopen_button = new RazorEnhanced.UI.RazorButton();
            this.emptyTab = new System.Windows.Forms.TabPage();
            this.targetTestButton = new RazorEnhanced.UI.RazorButton();
            this.targetsaveButton = new RazorEnhanced.UI.RazorButton();
            this.groupBox57 = new System.Windows.Forms.GroupBox();
            this.targetYellowCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.targetRedCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.targetOrangeCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.targetCriminalCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.targetGreyCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.targetGreenCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.targetBlueCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox56 = new System.Windows.Forms.GroupBox();
            this.targetSelectorComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.groupBox55 = new System.Windows.Forms.GroupBox();
            this.targetNameTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.groupBox48 = new System.Windows.Forms.GroupBox();
            this.label73 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.targetRangeMaxTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.label75 = new System.Windows.Forms.Label();
            this.targetRangeMinTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.groupBox46 = new System.Windows.Forms.GroupBox();
            this.groupBox47 = new System.Windows.Forms.GroupBox();
            this.paralizedBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.paralizedOff = new RazorEnhanced.UI.RazorRadioButton();
            this.paralizedOn = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox49 = new System.Windows.Forms.GroupBox();
            this.friendOn = new RazorEnhanced.UI.RazorRadioButton();
            this.friendBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.friendOff = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox50 = new System.Windows.Forms.GroupBox();
            this.warmodeOn = new RazorEnhanced.UI.RazorRadioButton();
            this.warmodeBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.warmodeOff = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox51 = new System.Windows.Forms.GroupBox();
            this.ghostOn = new RazorEnhanced.UI.RazorRadioButton();
            this.ghostBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.ghostOff = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox52 = new System.Windows.Forms.GroupBox();
            this.humanOn = new RazorEnhanced.UI.RazorRadioButton();
            this.humanOff = new RazorEnhanced.UI.RazorRadioButton();
            this.humanBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox53 = new System.Windows.Forms.GroupBox();
            this.blessedOn = new RazorEnhanced.UI.RazorRadioButton();
            this.blessedOff = new RazorEnhanced.UI.RazorRadioButton();
            this.blessedBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox54 = new System.Windows.Forms.GroupBox();
            this.poisonedOn = new RazorEnhanced.UI.RazorRadioButton();
            this.poisonedOff = new RazorEnhanced.UI.RazorRadioButton();
            this.poisonedBoth = new RazorEnhanced.UI.RazorRadioButton();
            this.groupBox45 = new System.Windows.Forms.GroupBox();
            this.targetChoseHue = new RazorEnhanced.UI.RazorButton();
            this.targethueGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetcoloCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox44 = new System.Windows.Forms.GroupBox();
            this.targetChoseBody = new RazorEnhanced.UI.RazorButton();
            this.targetbodydataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetbodyCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.groupBox43 = new System.Windows.Forms.GroupBox();
            this.label76 = new System.Windows.Forms.Label();
            this.targetremoveButton = new RazorEnhanced.UI.RazorButton();
            this.targetaddButton = new RazorEnhanced.UI.RazorButton();
            this.targetaddTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.targetlistBox = new System.Windows.Forms.ListBox();
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
            this.enhancedHotKeytabPage = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.hotkeyMasterClearButton = new RazorEnhanced.UI.RazorButton();
            this.hotkeyKeyMasterTextBox = new RazorEnhanced.UI.RazorHotKeyTextBox();
            this.hotkeyMasterSetButton = new RazorEnhanced.UI.RazorButton();
            this.label42 = new System.Windows.Forms.Label();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.hotkeyMDisableButton = new RazorEnhanced.UI.RazorButton();
            this.hotkeyMEnableButton = new RazorEnhanced.UI.RazorButton();
            this.hotkeyKeyMasterLabel = new System.Windows.Forms.Label();
            this.hotkeyStatusLabel = new System.Windows.Forms.Label();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.hotkeypassCheckBox = new RazorEnhanced.UI.RazorCheckBox();
            this.hotkeyClearButton = new RazorEnhanced.UI.RazorButton();
            this.hotkeySetButton = new RazorEnhanced.UI.RazorButton();
            this.label39 = new System.Windows.Forms.Label();
            this.hotkeytextbox = new RazorEnhanced.UI.RazorHotKeyTextBox();
            this.hotkeytreeView = new System.Windows.Forms.TreeView();
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
            this.videoTab = new System.Windows.Forms.TabPage();
            this.videoRecStatuslabel = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.videoSourcePlayer = new Accord.Controls.VideoSourcePlayer();
            this.videosettinggroupBox = new System.Windows.Forms.GroupBox();
            this.videoCodecComboBox = new RazorEnhanced.UI.RazorComboBox();
            this.label63 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.videoFPSTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.videorecbutton = new System.Windows.Forms.Button();
            this.videostopbutton = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.videolistBox = new System.Windows.Forms.ListBox();
            this.videoPathButton = new RazorEnhanced.UI.RazorButton();
            this.videoPathTextBox = new RazorEnhanced.UI.RazorTextBox();
            this.DPStabPage = new System.Windows.Forms.TabPage();
            this.filtergroup = new System.Windows.Forms.GroupBox();
            this.DPSMeterClearFilterButton = new RazorEnhanced.UI.RazorButton();
            this.DPSMeterApplyFilterButton = new RazorEnhanced.UI.RazorButton();
            this.DPSmetername = new RazorEnhanced.UI.RazorTextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.DPSmeterserial = new RazorEnhanced.UI.RazorAgentNumHexTextBox();
            this.label69 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.DPSmetermaxdamage = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.label66 = new System.Windows.Forms.Label();
            this.DPSmetermindamage = new RazorEnhanced.UI.RazorAgentNumOnlyTextBox();
            this.DpsMeterGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DPSMeterStatusLabel = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.DPSMeterPauseButton = new RazorEnhanced.UI.RazorButton();
            this.DPSMeterStopButton = new RazorEnhanced.UI.RazorButton();
            this.DPSMeterStartButton = new RazorEnhanced.UI.RazorButton();
            this.DPSMeterClearButton = new RazorEnhanced.UI.RazorButton();
            this.statusTab = new System.Windows.Forms.TabPage();
            this.advertisementLink = new RazorEnhanced.UI.RazorButton();
            this.advertisement = new System.Windows.Forms.PictureBox();
            this.label71 = new System.Windows.Forms.Label();
            this.labelHotride = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.discordrazorButton = new RazorEnhanced.UI.RazorButton();
            this.razorButtonWiki = new RazorEnhanced.UI.RazorButton();
            this.m_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.openFileDialogscript = new System.Windows.Forms.OpenFileDialog();
            this.timerupdatestatus = new System.Windows.Forms.Timer(this.components);
            this.datagridMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptgridMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loopModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waitBeforeInterruptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoStartAtLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timertitlestatusbar = new System.Windows.Forms.Timer(this.components);
            this.openmaplocation = new System.Windows.Forms.OpenFileDialog();
            this.tabs.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.groupBox29.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.moreOptTab.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.enhancedFilterTab.SuspendLayout();
            this.uomodgroupbox.SuspendLayout();
            this.groupBox32.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphfilterdatagrid)).BeginInit();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.scriptingTab.SuspendLayout();
            this.groupBox42.SuspendLayout();
            this.groupBox31.SuspendLayout();
            this.groupBox30.SuspendLayout();
            this.EnhancedAgent.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.eautoloot.SuspendLayout();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autolootdataGridView)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.escavenger.SuspendLayout();
            this.groupBox41.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scavengerdataGridView)).BeginInit();
            this.groupBox12.SuspendLayout();
            this.organizer.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.organizerdataGridView)).BeginInit();
            this.groupBox16.SuspendLayout();
            this.VendorBuy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vendorbuydataGridView)).BeginInit();
            this.groupBox18.SuspendLayout();
            this.VendorSell.SuspendLayout();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vendorsellGridView)).BeginInit();
            this.groupBox20.SuspendLayout();
            this.Dress.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.friends.SuspendLayout();
            this.groupBox34.SuspendLayout();
            this.groupBox33.SuspendLayout();
            this.friendGroupBox.SuspendLayout();
            this.friendloggroupBox.SuspendLayout();
            this.restock.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.restockdataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.bandageheal.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.toolbarTab.SuspendLayout();
            this.toolbarstab.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox39.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolbar_trackBar)).BeginInit();
            this.groupBox25.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox38.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spellgrid_trackBar)).BeginInit();
            this.groupBox37.SuspendLayout();
            this.groupBox36.SuspendLayout();
            this.groupBox35.SuspendLayout();
            this.emptyTab.SuspendLayout();
            this.groupBox57.SuspendLayout();
            this.groupBox56.SuspendLayout();
            this.groupBox55.SuspendLayout();
            this.groupBox48.SuspendLayout();
            this.groupBox46.SuspendLayout();
            this.groupBox47.SuspendLayout();
            this.groupBox49.SuspendLayout();
            this.groupBox50.SuspendLayout();
            this.groupBox51.SuspendLayout();
            this.groupBox52.SuspendLayout();
            this.groupBox53.SuspendLayout();
            this.groupBox54.SuspendLayout();
            this.groupBox45.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targethueGridView)).BeginInit();
            this.groupBox44.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetbodydataGridView)).BeginInit();
            this.groupBox43.SuspendLayout();
            this.skillsTab.SuspendLayout();
            this.enhancedHotKeytabPage.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox28.SuspendLayout();
            this.groupBox27.SuspendLayout();
            this.screenshotTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).BeginInit();
            this.videoTab.SuspendLayout();
            this.groupBox40.SuspendLayout();
            this.videosettinggroupBox.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.DPStabPage.SuspendLayout();
            this.filtergroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DpsMeterGridView)).BeginInit();
            this.statusTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advertisement)).BeginInit();
            this.datagridMenuStrip.SuspendLayout();
            this.scriptgridMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.generalTab);
            this.tabs.Controls.Add(this.moreOptTab);
            this.tabs.Controls.Add(this.enhancedFilterTab);
            this.tabs.Controls.Add(this.scriptingTab);
            this.tabs.Controls.Add(this.EnhancedAgent);
            this.tabs.Controls.Add(this.toolbarTab);
            this.tabs.Controls.Add(this.emptyTab);
            this.tabs.Controls.Add(this.skillsTab);
            this.tabs.Controls.Add(this.enhancedHotKeytabPage);
            this.tabs.Controls.Add(this.screenshotTab);
            this.tabs.Controls.Add(this.videoTab);
            this.tabs.Controls.Add(this.DPStabPage);
            this.tabs.Controls.Add(this.statusTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.ItemSize = new System.Drawing.Size(90, 18);
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.Padding = new System.Drawing.Point(9, 3);
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(674, 410);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabs.TabIndex = 0;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_IndexChanged);
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.label79);
            this.generalTab.Controls.Add(this.paypalButton);
            this.generalTab.Controls.Add(this.openchangelogButton);
            this.generalTab.Controls.Add(this.notshowlauncher);
            this.generalTab.Controls.Add(this.groupBox29);
            this.generalTab.Controls.Add(this.forceSizeY);
            this.generalTab.Controls.Add(this.forceSizeX);
            this.generalTab.Controls.Add(this.gameSize);
            this.generalTab.Controls.Add(this.rememberPwds);
            this.generalTab.Controls.Add(this.clientPrio);
            this.generalTab.Controls.Add(this.systray);
            this.generalTab.Controls.Add(this.taskbar);
            this.generalTab.Controls.Add(this.smartCPU);
            this.generalTab.Controls.Add(this.label11);
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
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(431, 102);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(14, 13);
            this.label79.TabIndex = 70;
            this.label79.Text = "X";
            // 
            // paypalButton
            // 
            this.paypalButton.BackColor = System.Drawing.SystemColors.Control;
            this.paypalButton.BackgroundImage = global::Assistant.Properties.Resources.PayPal;
            this.paypalButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.paypalButton.FlatAppearance.BorderSize = 0;
            this.paypalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paypalButton.Location = new System.Drawing.Point(542, 53);
            this.paypalButton.Name = "paypalButton";
            this.paypalButton.Size = new System.Drawing.Size(95, 33);
            this.paypalButton.TabIndex = 69;
            this.paypalButton.UseVisualStyleBackColor = false;
            this.paypalButton.Click += new System.EventHandler(this.paypalButton_Click);
            // 
            // openchangelogButton
            // 
            this.openchangelogButton.Location = new System.Drawing.Point(542, 24);
            this.openchangelogButton.Name = "openchangelogButton";
            this.openchangelogButton.Size = new System.Drawing.Size(95, 23);
            this.openchangelogButton.TabIndex = 68;
            this.openchangelogButton.Text = "Changelog";
            this.openchangelogButton.UseVisualStyleBackColor = true;
            this.openchangelogButton.Click += new System.EventHandler(this.openchangelogButton_Click);
            // 
            // notshowlauncher
            // 
            this.notshowlauncher.Location = new System.Drawing.Point(253, 124);
            this.notshowlauncher.Name = "notshowlauncher";
            this.notshowlauncher.Size = new System.Drawing.Size(241, 22);
            this.notshowlauncher.TabIndex = 67;
            this.notshowlauncher.Text = "Don\'t Show Launcher Window";
            this.notshowlauncher.CheckedChanged += new System.EventHandler(this.notshowlauncher_CheckedChanged);
            // 
            // groupBox29
            // 
            this.groupBox29.Controls.Add(this.profilesCloneButton);
            this.groupBox29.Controls.Add(this.profilesRenameButton);
            this.groupBox29.Controls.Add(this.profilesUnlinkButton);
            this.groupBox29.Controls.Add(this.profilesLinkButton);
            this.groupBox29.Controls.Add(this.profilelinklabel);
            this.groupBox29.Controls.Add(this.profilesDeleteButton);
            this.groupBox29.Controls.Add(this.profilesAddButton);
            this.groupBox29.Controls.Add(this.profilesComboBox);
            this.groupBox29.Location = new System.Drawing.Point(253, 216);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Size = new System.Drawing.Size(390, 98);
            this.groupBox29.TabIndex = 66;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "Profiles";
            // 
            // profilesCloneButton
            // 
            this.profilesCloneButton.Location = new System.Drawing.Point(321, 44);
            this.profilesCloneButton.Name = "profilesCloneButton";
            this.profilesCloneButton.Size = new System.Drawing.Size(63, 21);
            this.profilesCloneButton.TabIndex = 9;
            this.profilesCloneButton.Text = "Clone";
            this.profilesCloneButton.Click += new System.EventHandler(this.profilesCloneButton_Click);
            // 
            // profilesRenameButton
            // 
            this.profilesRenameButton.Location = new System.Drawing.Point(252, 44);
            this.profilesRenameButton.Name = "profilesRenameButton";
            this.profilesRenameButton.Size = new System.Drawing.Size(63, 21);
            this.profilesRenameButton.TabIndex = 8;
            this.profilesRenameButton.Text = "Rename";
            this.profilesRenameButton.Click += new System.EventHandler(this.profilesRenameButton_Click);
            // 
            // profilesUnlinkButton
            // 
            this.profilesUnlinkButton.Location = new System.Drawing.Point(75, 70);
            this.profilesUnlinkButton.Name = "profilesUnlinkButton";
            this.profilesUnlinkButton.Size = new System.Drawing.Size(63, 21);
            this.profilesUnlinkButton.TabIndex = 7;
            this.profilesUnlinkButton.Text = "UnLink";
            this.profilesUnlinkButton.Click += new System.EventHandler(this.profilesUnlinkButton_Click);
            // 
            // profilesLinkButton
            // 
            this.profilesLinkButton.Location = new System.Drawing.Point(6, 70);
            this.profilesLinkButton.Name = "profilesLinkButton";
            this.profilesLinkButton.Size = new System.Drawing.Size(63, 21);
            this.profilesLinkButton.TabIndex = 6;
            this.profilesLinkButton.Text = "Link";
            this.profilesLinkButton.Click += new System.EventHandler(this.profilesLinkButton_Click);
            // 
            // profilelinklabel
            // 
            this.profilelinklabel.AutoSize = true;
            this.profilelinklabel.Location = new System.Drawing.Point(7, 50);
            this.profilelinklabel.Name = "profilelinklabel";
            this.profilelinklabel.Size = new System.Drawing.Size(83, 13);
            this.profilelinklabel.TabIndex = 5;
            this.profilelinklabel.Text = "Linked to: None";
            // 
            // profilesDeleteButton
            // 
            this.profilesDeleteButton.Location = new System.Drawing.Point(321, 18);
            this.profilesDeleteButton.Name = "profilesDeleteButton";
            this.profilesDeleteButton.Size = new System.Drawing.Size(63, 21);
            this.profilesDeleteButton.TabIndex = 4;
            this.profilesDeleteButton.Text = "Delete";
            this.profilesDeleteButton.Click += new System.EventHandler(this.profilesDeleteButton_Click);
            // 
            // profilesAddButton
            // 
            this.profilesAddButton.Location = new System.Drawing.Point(252, 18);
            this.profilesAddButton.Name = "profilesAddButton";
            this.profilesAddButton.Size = new System.Drawing.Size(63, 21);
            this.profilesAddButton.TabIndex = 3;
            this.profilesAddButton.Text = "Add";
            this.profilesAddButton.Click += new System.EventHandler(this.profilesAddButton_Click);
            // 
            // profilesComboBox
            // 
            this.profilesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profilesComboBox.FormattingEnabled = true;
            this.profilesComboBox.Location = new System.Drawing.Point(6, 19);
            this.profilesComboBox.Name = "profilesComboBox";
            this.profilesComboBox.Size = new System.Drawing.Size(240, 21);
            this.profilesComboBox.TabIndex = 0;
            this.profilesComboBox.SelectedIndexChanged += new System.EventHandler(this.profilesComboBox_SelectedIndexChanged);
            // 
            // forceSizeY
            // 
            this.forceSizeY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSizeY.BackColor = System.Drawing.Color.White;
            this.forceSizeY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forceSizeY.Location = new System.Drawing.Point(450, 99);
            this.forceSizeY.Name = "forceSizeY";
            this.forceSizeY.Size = new System.Drawing.Size(50, 20);
            this.forceSizeY.TabIndex = 64;
            this.forceSizeY.TextChanged += new System.EventHandler(this.forceSizeY_TextChanged);
            // 
            // forceSizeX
            // 
            this.forceSizeX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSizeX.BackColor = System.Drawing.Color.White;
            this.forceSizeX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forceSizeX.Location = new System.Drawing.Point(375, 99);
            this.forceSizeX.Name = "forceSizeX";
            this.forceSizeX.Size = new System.Drawing.Size(50, 20);
            this.forceSizeX.TabIndex = 63;
            this.forceSizeX.TextChanged += new System.EventHandler(this.forceSizeX_TextChanged);
            // 
            // gameSize
            // 
            this.gameSize.Location = new System.Drawing.Point(253, 99);
            this.gameSize.Name = "gameSize";
            this.gameSize.Size = new System.Drawing.Size(114, 22);
            this.gameSize.TabIndex = 65;
            this.gameSize.Text = "Force Game Size:";
            this.gameSize.CheckedChanged += new System.EventHandler(this.gameSize_CheckedChanged);
            // 
            // rememberPwds
            // 
            this.rememberPwds.Location = new System.Drawing.Point(253, 74);
            this.rememberPwds.Name = "rememberPwds";
            this.rememberPwds.Size = new System.Drawing.Size(190, 22);
            this.rememberPwds.TabIndex = 54;
            this.rememberPwds.Text = "Remember passwords ";
            this.rememberPwds.CheckedChanged += new System.EventHandler(this.rememberPwds_CheckedChanged);
            // 
            // clientPrio
            // 
            this.clientPrio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientPrio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.clientPrio.Items.AddRange(new object[] {
            "Idle",
            "BelowNormal",
            "Normal",
            "AboveNormal",
            "High",
            "Realtime"});
            this.clientPrio.Location = new System.Drawing.Point(363, 186);
            this.clientPrio.Name = "clientPrio";
            this.clientPrio.Size = new System.Drawing.Size(88, 23);
            this.clientPrio.TabIndex = 60;
            this.clientPrio.SelectedIndexChanged += new System.EventHandler(this.clientPrio_SelectedIndexChanged);
            // 
            // systray
            // 
            this.systray.Location = new System.Drawing.Point(373, 156);
            this.systray.Name = "systray";
            this.systray.Size = new System.Drawing.Size(99, 20);
            this.systray.TabIndex = 35;
            this.systray.Text = "System Tray";
            this.systray.CheckedChanged += new System.EventHandler(this.systray_CheckedChanged);
            // 
            // taskbar
            // 
            this.taskbar.Location = new System.Drawing.Point(301, 154);
            this.taskbar.Name = "taskbar";
            this.taskbar.Size = new System.Drawing.Size(66, 20);
            this.taskbar.TabIndex = 34;
            this.taskbar.Text = "Taskbar";
            this.taskbar.CheckedChanged += new System.EventHandler(this.taskbar_CheckedChanged);
            // 
            // smartCPU
            // 
            this.smartCPU.Location = new System.Drawing.Point(253, 24);
            this.smartCPU.Name = "smartCPU";
            this.smartCPU.Size = new System.Drawing.Size(241, 22);
            this.smartCPU.TabIndex = 53;
            this.smartCPU.Text = "Use smart CPU usage reduction";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(251, 156);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 15);
            this.label11.TabIndex = 33;
            this.label11.Text = "Show in:";
            // 
            // opacity
            // 
            this.opacity.AutoSize = false;
            this.opacity.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.opacity.Location = new System.Drawing.Point(331, 334);
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
            this.alwaysTop.Location = new System.Drawing.Point(253, 49);
            this.alwaysTop.Name = "alwaysTop";
            this.alwaysTop.Size = new System.Drawing.Size(241, 22);
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
            this.opacityLabel.Location = new System.Drawing.Point(253, 334);
            this.opacityLabel.Name = "opacityLabel";
            this.opacityLabel.Size = new System.Drawing.Size(78, 16);
            this.opacityLabel.TabIndex = 23;
            this.opacityLabel.Text = "Opacity: 100%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(251, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 19);
            this.label9.TabIndex = 59;
            this.label9.Text = "Default Client Priority:";
            // 
            // moreOptTab
            // 
            this.moreOptTab.Controls.Add(this.druidClericPackets);
            this.moreOptTab.Controls.Add(this.allowHiddenLooting);
            this.moreOptTab.Controls.Add(this.filterNPC);
            this.moreOptTab.Controls.Add(this.groupBox17);
            this.moreOptTab.Controls.Add(this.label10);
            this.moreOptTab.Controls.Add(this.label8);
            this.moreOptTab.Controls.Add(this.label5);
            this.moreOptTab.Controls.Add(this.label6);
            this.moreOptTab.Controls.Add(this.label17);
            this.moreOptTab.Controls.Add(this.lblHarmHue);
            this.moreOptTab.Controls.Add(this.lblNeuHue);
            this.moreOptTab.Controls.Add(this.lblBeneHue);
            this.moreOptTab.Controls.Add(this.label4);
            this.moreOptTab.Controls.Add(this.lblWarnHue);
            this.moreOptTab.Controls.Add(this.lblMsgHue);
            this.moreOptTab.Controls.Add(this.label3);
            this.moreOptTab.Controls.Add(this.filterPoison);
            this.moreOptTab.Controls.Add(this.chknorunStealth);
            this.moreOptTab.Controls.Add(this.nosearchpouches);
            this.moreOptTab.Controls.Add(this.autosearchcontainers);
            this.moreOptTab.Controls.Add(this.uo3dEquipUnEquip);
            this.moreOptTab.Controls.Add(this.hiddedAutoOpenDoors);
            this.moreOptTab.Controls.Add(this.chkPartyOverhead);
            this.moreOptTab.Controls.Add(this.healthFmt);
            this.moreOptTab.Controls.Add(this.showHealthOH);
            this.moreOptTab.Controls.Add(this.showtargtext);
            this.moreOptTab.Controls.Add(this.ltRange);
            this.moreOptTab.Controls.Add(this.rangeCheckLT);
            this.moreOptTab.Controls.Add(this.smartLT);
            this.moreOptTab.Controls.Add(this.txtObjDelay);
            this.moreOptTab.Controls.Add(this.QueueActions);
            this.moreOptTab.Controls.Add(this.actionStatusMsg);
            this.moreOptTab.Controls.Add(this.msglvl);
            this.moreOptTab.Controls.Add(this.potionEquip);
            this.moreOptTab.Controls.Add(this.spellUnequip);
            this.moreOptTab.Controls.Add(this.autoOpenDoors);
            this.moreOptTab.Controls.Add(this.chkStealth);
            this.moreOptTab.Controls.Add(this.preAOSstatbar);
            this.moreOptTab.Controls.Add(this.setLTHilight);
            this.moreOptTab.Controls.Add(this.lthilight);
            this.moreOptTab.Controls.Add(this.filterSnoop);
            this.moreOptTab.Controls.Add(this.corpseRange);
            this.moreOptTab.Controls.Add(this.incomingCorpse);
            this.moreOptTab.Controls.Add(this.incomingMob);
            this.moreOptTab.Controls.Add(this.setHarmHue);
            this.moreOptTab.Controls.Add(this.setNeuHue);
            this.moreOptTab.Controls.Add(this.setBeneHue);
            this.moreOptTab.Controls.Add(this.setSpeechHue);
            this.moreOptTab.Controls.Add(this.setWarnHue);
            this.moreOptTab.Controls.Add(this.setMsgHue);
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
            // druidClericPackets
            // 
            this.druidClericPackets.AutoSize = true;
            this.druidClericPackets.Location = new System.Drawing.Point(436, 329);
            this.druidClericPackets.Name = "druidClericPackets";
            this.druidClericPackets.Size = new System.Drawing.Size(189, 17);
            this.druidClericPackets.TabIndex = 83;
            this.druidClericPackets.Text = "Use packets for Druid/Cleric spells";
            this.druidClericPackets.UseVisualStyleBackColor = true;
            this.druidClericPackets.CheckedChanged += new System.EventHandler(this.druidClericPackets_CheckedChanged);
            // 
            // allowHiddenLooting
            // 
            this.allowHiddenLooting.Location = new System.Drawing.Point(204, 117);
            this.allowHiddenLooting.Name = "allowHiddenLooting";
            this.allowHiddenLooting.Size = new System.Drawing.Size(190, 22);
            this.allowHiddenLooting.TabIndex = 82;
            this.allowHiddenLooting.Text = "Allow looting while hidden";
            this.allowHiddenLooting.CheckedChanged += new System.EventHandler(this.hiddenLooting_CheckedChanged);
            // 
            // filterNPC
            // 
            this.filterNPC.Location = new System.Drawing.Point(204, 199);
            this.filterNPC.Name = "filterNPC";
            this.filterNPC.Size = new System.Drawing.Size(216, 22);
            this.filterNPC.TabIndex = 81;
            this.filterNPC.Text = "Filter Orc / Lizard / Ratmen Messages";
            this.filterNPC.CheckedChanged += new System.EventHandler(this.filterNPC_CheckedChanged);
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.setpathmapbutton);
            this.groupBox17.Controls.Add(this.label72);
            this.groupBox17.Controls.Add(this.enhancedmappathTextBox);
            this.groupBox17.Location = new System.Drawing.Point(7, 285);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(187, 58);
            this.groupBox17.TabIndex = 80;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Map Integration";
            // 
            // setpathmapbutton
            // 
            this.setpathmapbutton.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
            this.setpathmapbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.setpathmapbutton.Location = new System.Drawing.Point(157, 22);
            this.setpathmapbutton.Name = "setpathmapbutton";
            this.setpathmapbutton.Size = new System.Drawing.Size(20, 20);
            this.setpathmapbutton.TabIndex = 10;
            this.setpathmapbutton.UseVisualStyleBackColor = true;
            this.setpathmapbutton.Click += new System.EventHandler(this.setpathmapbutton_Click);
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(6, 25);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(32, 13);
            this.label72.TabIndex = 1;
            this.label72.Text = "Path:";
            // 
            // enhancedmappathTextBox
            // 
            this.enhancedmappathTextBox.Location = new System.Drawing.Point(41, 22);
            this.enhancedmappathTextBox.Name = "enhancedmappathTextBox";
            this.enhancedmappathTextBox.ReadOnly = true;
            this.enhancedmappathTextBox.Size = new System.Drawing.Size(110, 20);
            this.enhancedmappathTextBox.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(453, 184);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 17);
            this.label10.TabIndex = 73;
            this.label10.Text = "Health Format:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(452, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 18);
            this.label8.TabIndex = 72;
            this.label8.Text = "Tiles:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(452, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 18);
            this.label5.TabIndex = 70;
            this.label5.Text = "Object delay:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(568, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 18);
            this.label6.TabIndex = 71;
            this.label6.Text = "ms";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(7, 228);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(92, 18);
            this.label17.TabIndex = 68;
            this.label17.Text = "Razor messages:";
            // 
            // lblHarmHue
            // 
            this.lblHarmHue.Location = new System.Drawing.Point(77, 142);
            this.lblHarmHue.Name = "lblHarmHue";
            this.lblHarmHue.Size = new System.Drawing.Size(45, 14);
            this.lblHarmHue.TabIndex = 46;
            this.lblHarmHue.Text = "Harmful";
            // 
            // lblNeuHue
            // 
            this.lblNeuHue.Location = new System.Drawing.Point(135, 142);
            this.lblNeuHue.Name = "lblNeuHue";
            this.lblNeuHue.Size = new System.Drawing.Size(42, 14);
            this.lblNeuHue.TabIndex = 45;
            this.lblNeuHue.Text = "Neutral";
            // 
            // lblBeneHue
            // 
            this.lblBeneHue.Location = new System.Drawing.Point(17, 142);
            this.lblBeneHue.Name = "lblBeneHue";
            this.lblBeneHue.Size = new System.Drawing.Size(55, 14);
            this.lblBeneHue.TabIndex = 44;
            this.lblBeneHue.Text = "Beneficial";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(384, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "tiles";
            // 
            // lblWarnHue
            // 
            this.lblWarnHue.Location = new System.Drawing.Point(7, 44);
            this.lblWarnHue.Name = "lblWarnHue";
            this.lblWarnHue.Size = new System.Drawing.Size(139, 16);
            this.lblWarnHue.TabIndex = 16;
            this.lblWarnHue.Text = "Warning Message Hue";
            // 
            // lblMsgHue
            // 
            this.lblMsgHue.Location = new System.Drawing.Point(7, 19);
            this.lblMsgHue.Name = "lblMsgHue";
            this.lblMsgHue.Size = new System.Drawing.Size(139, 17);
            this.lblMsgHue.TabIndex = 15;
            this.lblMsgHue.Text = "Razor Message Hue";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Spell Format:";
            // 
            // filterPoison
            // 
            this.filterPoison.Location = new System.Drawing.Point(204, 178);
            this.filterPoison.Name = "filterPoison";
            this.filterPoison.Size = new System.Drawing.Size(216, 22);
            this.filterPoison.TabIndex = 79;
            this.filterPoison.Text = "Filter Poison Messages";
            this.filterPoison.CheckedChanged += new System.EventHandler(this.filterPoison_CheckedChanged);
            // 
            // chknorunStealth
            // 
            this.chknorunStealth.Location = new System.Drawing.Point(204, 283);
            this.chknorunStealth.Name = "chknorunStealth";
            this.chknorunStealth.Size = new System.Drawing.Size(190, 22);
            this.chknorunStealth.TabIndex = 78;
            this.chknorunStealth.Text = "No run if Stealthed";
            this.chknorunStealth.CheckedChanged += new System.EventHandler(this.chknorunStealth_CheckedChanged);
            // 
            // nosearchpouches
            // 
            this.nosearchpouches.Location = new System.Drawing.Point(455, 306);
            this.nosearchpouches.Name = "nosearchpouches";
            this.nosearchpouches.Size = new System.Drawing.Size(185, 22);
            this.nosearchpouches.TabIndex = 77;
            this.nosearchpouches.Text = "Ignore pouches";
            this.nosearchpouches.CheckedChanged += new System.EventHandler(this.nosearchpouches_CheckedChanged);
            // 
            // autosearchcontainers
            // 
            this.autosearchcontainers.Location = new System.Drawing.Point(436, 285);
            this.autosearchcontainers.Name = "autosearchcontainers";
            this.autosearchcontainers.Size = new System.Drawing.Size(214, 22);
            this.autosearchcontainers.TabIndex = 76;
            this.autosearchcontainers.Text = "Auto search new containers";
            this.autosearchcontainers.CheckedChanged += new System.EventHandler(this.autosearchcontainers_CheckedChanged);
            // 
            // uo3dEquipUnEquip
            // 
            this.uo3dEquipUnEquip.Location = new System.Drawing.Point(436, 264);
            this.uo3dEquipUnEquip.Name = "uo3dEquipUnEquip";
            this.uo3dEquipUnEquip.Size = new System.Drawing.Size(214, 22);
            this.uo3dEquipUnEquip.TabIndex = 75;
            this.uo3dEquipUnEquip.Text = "Use UO3D Equip and UnEquip";
            this.uo3dEquipUnEquip.CheckedChanged += new System.EventHandler(this.uo3dEquipUnEquip_CheckedChanged);
            // 
            // hiddedAutoOpenDoors
            // 
            this.hiddedAutoOpenDoors.Location = new System.Drawing.Point(222, 325);
            this.hiddedAutoOpenDoors.Name = "hiddedAutoOpenDoors";
            this.hiddedAutoOpenDoors.Size = new System.Drawing.Size(190, 22);
            this.hiddedAutoOpenDoors.TabIndex = 74;
            this.hiddedAutoOpenDoors.Text = "Disable if hidden";
            this.hiddedAutoOpenDoors.CheckedChanged += new System.EventHandler(this.hiddedAutoOpenDoors_CheckedChanged);
            // 
            // chkPartyOverhead
            // 
            this.chkPartyOverhead.Location = new System.Drawing.Point(436, 202);
            this.chkPartyOverhead.Name = "chkPartyOverhead";
            this.chkPartyOverhead.Size = new System.Drawing.Size(226, 21);
            this.chkPartyOverhead.TabIndex = 72;
            this.chkPartyOverhead.Text = "Show mana/stam above party members";
            this.chkPartyOverhead.CheckedChanged += new System.EventHandler(this.chkPartyOverhead_CheckedChanged);
            // 
            // healthFmt
            // 
            this.healthFmt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.healthFmt.BackColor = System.Drawing.Color.White;
            this.healthFmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.healthFmt.Location = new System.Drawing.Point(537, 181);
            this.healthFmt.Name = "healthFmt";
            this.healthFmt.Size = new System.Drawing.Size(113, 20);
            this.healthFmt.TabIndex = 71;
            this.healthFmt.TextChanged += new System.EventHandler(this.healthFmt_TextChanged);
            // 
            // showHealthOH
            // 
            this.showHealthOH.Location = new System.Drawing.Point(436, 159);
            this.showHealthOH.Name = "showHealthOH";
            this.showHealthOH.Size = new System.Drawing.Size(222, 22);
            this.showHealthOH.TabIndex = 69;
            this.showHealthOH.Text = "Show health above people/creatures";
            this.showHealthOH.CheckedChanged += new System.EventHandler(this.showHealthOH_CheckedChanged);
            // 
            // showtargtext
            // 
            this.showtargtext.Location = new System.Drawing.Point(436, 138);
            this.showtargtext.Name = "showtargtext";
            this.showtargtext.Size = new System.Drawing.Size(190, 22);
            this.showtargtext.TabIndex = 53;
            this.showtargtext.Text = "Show target flag on single click";
            this.showtargtext.CheckedChanged += new System.EventHandler(this.showtargtext_CheckedChanged);
            // 
            // ltRange
            // 
            this.ltRange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltRange.BackColor = System.Drawing.Color.White;
            this.ltRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ltRange.Location = new System.Drawing.Point(491, 117);
            this.ltRange.Name = "ltRange";
            this.ltRange.Size = new System.Drawing.Size(22, 20);
            this.ltRange.TabIndex = 41;
            this.ltRange.TextChanged += new System.EventHandler(this.ltRange_TextChanged);
            // 
            // rangeCheckLT
            // 
            this.rangeCheckLT.Location = new System.Drawing.Point(436, 94);
            this.rangeCheckLT.Name = "rangeCheckLT";
            this.rangeCheckLT.Size = new System.Drawing.Size(185, 22);
            this.rangeCheckLT.TabIndex = 40;
            this.rangeCheckLT.Text = "Range check Last Target:";
            this.rangeCheckLT.CheckedChanged += new System.EventHandler(this.rangeCheckLT_CheckedChanged);
            // 
            // smartLT
            // 
            this.smartLT.Location = new System.Drawing.Point(436, 75);
            this.smartLT.Name = "smartLT";
            this.smartLT.Size = new System.Drawing.Size(185, 22);
            this.smartLT.TabIndex = 52;
            this.smartLT.Text = "Use smart last target";
            this.smartLT.CheckedChanged += new System.EventHandler(this.smartLT_CheckedChanged);
            // 
            // txtObjDelay
            // 
            this.txtObjDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjDelay.BackColor = System.Drawing.Color.White;
            this.txtObjDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtObjDelay.Location = new System.Drawing.Point(530, 54);
            this.txtObjDelay.Name = "txtObjDelay";
            this.txtObjDelay.Size = new System.Drawing.Size(32, 20);
            this.txtObjDelay.TabIndex = 37;
            this.txtObjDelay.TextChanged += new System.EventHandler(this.txtObjDelay_TextChanged);
            // 
            // QueueActions
            // 
            this.QueueActions.Location = new System.Drawing.Point(436, 33);
            this.QueueActions.Name = "QueueActions";
            this.QueueActions.Size = new System.Drawing.Size(222, 22);
            this.QueueActions.TabIndex = 34;
            this.QueueActions.Text = "Auto-Queue Object Delay actions ";
            this.QueueActions.CheckedChanged += new System.EventHandler(this.QueueActions_CheckedChanged);
            // 
            // actionStatusMsg
            // 
            this.actionStatusMsg.Location = new System.Drawing.Point(436, 12);
            this.actionStatusMsg.Name = "actionStatusMsg";
            this.actionStatusMsg.Size = new System.Drawing.Size(222, 22);
            this.actionStatusMsg.TabIndex = 38;
            this.actionStatusMsg.Text = "Show Action-Queue status messages";
            this.actionStatusMsg.CheckedChanged += new System.EventHandler(this.actionStatusMsg_CheckedChanged);
            // 
            // msglvl
            // 
            this.msglvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.msglvl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.msglvl.Items.AddRange(new object[] {
            "Show All",
            "Warnings & Errors",
            "Errors Only",
            "None"});
            this.msglvl.Location = new System.Drawing.Point(106, 224);
            this.msglvl.Name = "msglvl";
            this.msglvl.Size = new System.Drawing.Size(88, 23);
            this.msglvl.TabIndex = 69;
            this.msglvl.SelectedIndexChanged += new System.EventHandler(this.msglvl_SelectedIndexChanged);
            // 
            // potionEquip
            // 
            this.potionEquip.Location = new System.Drawing.Point(436, 243);
            this.potionEquip.Name = "potionEquip";
            this.potionEquip.Size = new System.Drawing.Size(214, 22);
            this.potionEquip.TabIndex = 67;
            this.potionEquip.Text = "Auto Un/Re-equip hands for potions";
            this.potionEquip.CheckedChanged += new System.EventHandler(this.potionEquip_CheckedChanged);
            // 
            // spellUnequip
            // 
            this.spellUnequip.Location = new System.Drawing.Point(436, 222);
            this.spellUnequip.Name = "spellUnequip";
            this.spellUnequip.Size = new System.Drawing.Size(214, 22);
            this.spellUnequip.TabIndex = 39;
            this.spellUnequip.Text = "Auto Unequip hands before casting";
            this.spellUnequip.CheckedChanged += new System.EventHandler(this.spellUnequip_CheckedChanged);
            // 
            // autoOpenDoors
            // 
            this.autoOpenDoors.Location = new System.Drawing.Point(204, 304);
            this.autoOpenDoors.Name = "autoOpenDoors";
            this.autoOpenDoors.Size = new System.Drawing.Size(190, 22);
            this.autoOpenDoors.TabIndex = 59;
            this.autoOpenDoors.Text = "Automatically open doors";
            this.autoOpenDoors.CheckedChanged += new System.EventHandler(this.autoOpenDoors_CheckedChanged);
            // 
            // chkStealth
            // 
            this.chkStealth.Location = new System.Drawing.Point(204, 262);
            this.chkStealth.Name = "chkStealth";
            this.chkStealth.Size = new System.Drawing.Size(190, 22);
            this.chkStealth.TabIndex = 12;
            this.chkStealth.Text = "Count stealth steps";
            this.chkStealth.CheckedChanged += new System.EventHandler(this.chkStealth_CheckedChanged);
            // 
            // preAOSstatbar
            // 
            this.preAOSstatbar.Location = new System.Drawing.Point(204, 12);
            this.preAOSstatbar.Name = "preAOSstatbar";
            this.preAOSstatbar.Size = new System.Drawing.Size(190, 22);
            this.preAOSstatbar.TabIndex = 57;
            this.preAOSstatbar.Text = "Use Pre-AOS status window";
            this.preAOSstatbar.CheckedChanged += new System.EventHandler(this.preAOSstatbar_CheckedChanged);
            // 
            // setLTHilight
            // 
            this.setLTHilight.Location = new System.Drawing.Point(152, 90);
            this.setLTHilight.Name = "setLTHilight";
            this.setLTHilight.Size = new System.Drawing.Size(32, 20);
            this.setLTHilight.TabIndex = 51;
            this.setLTHilight.Text = "Set";
            this.setLTHilight.Click += new System.EventHandler(this.setLTHilight_Click);
            // 
            // lthilight
            // 
            this.lthilight.Location = new System.Drawing.Point(7, 93);
            this.lthilight.Name = "lthilight";
            this.lthilight.Size = new System.Drawing.Size(139, 22);
            this.lthilight.TabIndex = 50;
            this.lthilight.Text = "Last Target Highlight:";
            this.lthilight.CheckedChanged += new System.EventHandler(this.lthilight_CheckedChanged);
            // 
            // filterSnoop
            // 
            this.filterSnoop.Location = new System.Drawing.Point(204, 157);
            this.filterSnoop.Name = "filterSnoop";
            this.filterSnoop.Size = new System.Drawing.Size(208, 22);
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
            this.corpseRange.Location = new System.Drawing.Point(355, 96);
            this.corpseRange.Name = "corpseRange";
            this.corpseRange.Size = new System.Drawing.Size(23, 20);
            this.corpseRange.TabIndex = 23;
            this.corpseRange.TextChanged += new System.EventHandler(this.corpseRange_TextChanged);
            // 
            // incomingCorpse
            // 
            this.incomingCorpse.Location = new System.Drawing.Point(204, 241);
            this.incomingCorpse.Name = "incomingCorpse";
            this.incomingCorpse.Size = new System.Drawing.Size(226, 22);
            this.incomingCorpse.TabIndex = 48;
            this.incomingCorpse.Text = "Show Names of New/Incoming Corpses";
            this.incomingCorpse.CheckedChanged += new System.EventHandler(this.incomingCorpse_CheckedChanged);
            // 
            // incomingMob
            // 
            this.incomingMob.Location = new System.Drawing.Point(204, 220);
            this.incomingMob.Name = "incomingMob";
            this.incomingMob.Size = new System.Drawing.Size(244, 22);
            this.incomingMob.TabIndex = 47;
            this.incomingMob.Text = "Show Names of Incoming People/Creatures";
            this.incomingMob.CheckedChanged += new System.EventHandler(this.incomingMob_CheckedChanged);
            // 
            // setHarmHue
            // 
            this.setHarmHue.Enabled = false;
            this.setHarmHue.Location = new System.Drawing.Point(83, 159);
            this.setHarmHue.Name = "setHarmHue";
            this.setHarmHue.Size = new System.Drawing.Size(32, 20);
            this.setHarmHue.TabIndex = 42;
            this.setHarmHue.Text = "Set";
            this.setHarmHue.Click += new System.EventHandler(this.setHarmHue_Click);
            // 
            // setNeuHue
            // 
            this.setNeuHue.Enabled = false;
            this.setNeuHue.Location = new System.Drawing.Point(140, 159);
            this.setNeuHue.Name = "setNeuHue";
            this.setNeuHue.Size = new System.Drawing.Size(31, 20);
            this.setNeuHue.TabIndex = 43;
            this.setNeuHue.Text = "Set";
            this.setNeuHue.Click += new System.EventHandler(this.setNeuHue_Click);
            // 
            // setBeneHue
            // 
            this.setBeneHue.Location = new System.Drawing.Point(28, 159);
            this.setBeneHue.Name = "setBeneHue";
            this.setBeneHue.Size = new System.Drawing.Size(33, 20);
            this.setBeneHue.TabIndex = 41;
            this.setBeneHue.Text = "Set";
            this.setBeneHue.Click += new System.EventHandler(this.setBeneHue_Click);
            // 
            // setSpeechHue
            // 
            this.setSpeechHue.Location = new System.Drawing.Point(152, 66);
            this.setSpeechHue.Name = "setSpeechHue";
            this.setSpeechHue.Size = new System.Drawing.Size(32, 20);
            this.setSpeechHue.TabIndex = 40;
            this.setSpeechHue.Text = "Set";
            this.setSpeechHue.Click += new System.EventHandler(this.setSpeechHue_Click);
            // 
            // setWarnHue
            // 
            this.setWarnHue.Location = new System.Drawing.Point(152, 42);
            this.setWarnHue.Name = "setWarnHue";
            this.setWarnHue.Size = new System.Drawing.Size(32, 20);
            this.setWarnHue.TabIndex = 39;
            this.setWarnHue.Text = "Set";
            this.setWarnHue.Click += new System.EventHandler(this.setWarnHue_Click);
            // 
            // setMsgHue
            // 
            this.setMsgHue.Location = new System.Drawing.Point(152, 18);
            this.setMsgHue.Name = "setMsgHue";
            this.setMsgHue.Size = new System.Drawing.Size(32, 19);
            this.setMsgHue.TabIndex = 38;
            this.setMsgHue.Text = "Set";
            this.setMsgHue.Click += new System.EventHandler(this.setMsgHue_Click);
            // 
            // autoStackRes
            // 
            this.autoStackRes.Location = new System.Drawing.Point(204, 75);
            this.autoStackRes.Name = "autoStackRes";
            this.autoStackRes.Size = new System.Drawing.Size(228, 22);
            this.autoStackRes.TabIndex = 35;
            this.autoStackRes.Text = "Auto-Stack Ore/Fish/Logs at Feet";
            this.autoStackRes.CheckedChanged += new System.EventHandler(this.autoStackRes_CheckedChanged);
            // 
            // queueTargets
            // 
            this.queueTargets.Location = new System.Drawing.Point(204, 33);
            this.queueTargets.Name = "queueTargets";
            this.queueTargets.Size = new System.Drawing.Size(228, 22);
            this.queueTargets.TabIndex = 34;
            this.queueTargets.Text = "Queue LastTarget and TargetSelf";
            this.queueTargets.CheckedChanged += new System.EventHandler(this.queueTargets_CheckedChanged);
            // 
            // spamFilter
            // 
            this.spamFilter.Location = new System.Drawing.Point(204, 136);
            this.spamFilter.Name = "spamFilter";
            this.spamFilter.Size = new System.Drawing.Size(228, 22);
            this.spamFilter.TabIndex = 26;
            this.spamFilter.Text = "Filter repeating system messages";
            this.spamFilter.CheckedChanged += new System.EventHandler(this.spamFilter_CheckedChanged);
            // 
            // openCorpses
            // 
            this.openCorpses.Location = new System.Drawing.Point(204, 96);
            this.openCorpses.Name = "openCorpses";
            this.openCorpses.Size = new System.Drawing.Size(156, 22);
            this.openCorpses.TabIndex = 22;
            this.openCorpses.Text = "Open new corpses within";
            this.openCorpses.CheckedChanged += new System.EventHandler(this.openCorpses_CheckedChanged);
            // 
            // blockDis
            // 
            this.blockDis.Location = new System.Drawing.Point(204, 54);
            this.blockDis.Name = "blockDis";
            this.blockDis.Size = new System.Drawing.Size(184, 22);
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
            this.txtSpellFormat.Location = new System.Drawing.Point(106, 196);
            this.txtSpellFormat.Name = "txtSpellFormat";
            this.txtSpellFormat.Size = new System.Drawing.Size(88, 20);
            this.txtSpellFormat.TabIndex = 5;
            this.txtSpellFormat.TextChanged += new System.EventHandler(this.txtSpellFormat_TextChanged);
            // 
            // chkForceSpellHue
            // 
            this.chkForceSpellHue.Location = new System.Drawing.Point(7, 117);
            this.chkForceSpellHue.Name = "chkForceSpellHue";
            this.chkForceSpellHue.Size = new System.Drawing.Size(139, 22);
            this.chkForceSpellHue.TabIndex = 2;
            this.chkForceSpellHue.Text = "Override Spell Hues:";
            this.chkForceSpellHue.CheckedChanged += new System.EventHandler(this.chkForceSpellHue_CheckedChanged);
            // 
            // chkForceSpeechHue
            // 
            this.chkForceSpeechHue.Location = new System.Drawing.Point(7, 69);
            this.chkForceSpeechHue.Name = "chkForceSpeechHue";
            this.chkForceSpeechHue.Size = new System.Drawing.Size(139, 22);
            this.chkForceSpeechHue.TabIndex = 0;
            this.chkForceSpeechHue.Text = "Override Speech Hue";
            this.chkForceSpeechHue.CheckedChanged += new System.EventHandler(this.chkForceSpeechHue_CheckedChanged);
            // 
            // enhancedFilterTab
            // 
            this.enhancedFilterTab.Controls.Add(this.uomodgroupbox);
            this.enhancedFilterTab.Controls.Add(this.groupBox32);
            this.enhancedFilterTab.Controls.Add(this.groupBox24);
            this.enhancedFilterTab.Controls.Add(this.groupBox23);
            this.enhancedFilterTab.Controls.Add(this.groupBox10);
            this.enhancedFilterTab.Controls.Add(this.groupBox9);
            this.enhancedFilterTab.Location = new System.Drawing.Point(4, 40);
            this.enhancedFilterTab.Name = "enhancedFilterTab";
            this.enhancedFilterTab.Size = new System.Drawing.Size(666, 366);
            this.enhancedFilterTab.TabIndex = 10;
            this.enhancedFilterTab.Text = "Filters";
            // 
            // uomodgroupbox
            // 
            this.uomodgroupbox.Controls.Add(this.uomodpaperdollCheckBox);
            this.uomodgroupbox.Controls.Add(this.uomodglobalsoundCheckBox);
            this.uomodgroupbox.Controls.Add(this.uomodFPSCheckBox);
            this.uomodgroupbox.Location = new System.Drawing.Point(192, 285);
            this.uomodgroupbox.Name = "uomodgroupbox";
            this.uomodgroupbox.Size = new System.Drawing.Size(283, 65);
            this.uomodgroupbox.TabIndex = 69;
            this.uomodgroupbox.TabStop = false;
            this.uomodgroupbox.Text = "UoMod (Client > 7.0.0.0)";
            // 
            // uomodpaperdollCheckBox
            // 
            this.uomodpaperdollCheckBox.Location = new System.Drawing.Point(119, 15);
            this.uomodpaperdollCheckBox.Name = "uomodpaperdollCheckBox";
            this.uomodpaperdollCheckBox.Size = new System.Drawing.Size(158, 22);
            this.uomodpaperdollCheckBox.TabIndex = 61;
            this.uomodpaperdollCheckBox.Text = "Show Paperdoll Slot";
            this.uomodpaperdollCheckBox.CheckedChanged += new System.EventHandler(this.uomodpaperdollCheckBox_CheckedChanged);
            // 
            // uomodglobalsoundCheckBox
            // 
            this.uomodglobalsoundCheckBox.Location = new System.Drawing.Point(6, 38);
            this.uomodglobalsoundCheckBox.Name = "uomodglobalsoundCheckBox";
            this.uomodglobalsoundCheckBox.Size = new System.Drawing.Size(99, 22);
            this.uomodglobalsoundCheckBox.TabIndex = 60;
            this.uomodglobalsoundCheckBox.Text = "Global Sound";
            this.uomodglobalsoundCheckBox.CheckedChanged += new System.EventHandler(this.uomodglobalsoundCheckBox_CheckedChanged);
            // 
            // uomodFPSCheckBox
            // 
            this.uomodFPSCheckBox.Location = new System.Drawing.Point(6, 15);
            this.uomodFPSCheckBox.Name = "uomodFPSCheckBox";
            this.uomodFPSCheckBox.Size = new System.Drawing.Size(99, 22);
            this.uomodFPSCheckBox.TabIndex = 59;
            this.uomodFPSCheckBox.Text = "Increase FPS";
            this.uomodFPSCheckBox.CheckedChanged += new System.EventHandler(this.uomodFPSCheckBox_CheckedChanged);
            // 
            // groupBox32
            // 
            this.groupBox32.Controls.Add(this.remountedelay);
            this.groupBox32.Controls.Add(this.remountdelay);
            this.groupBox32.Controls.Add(this.label48);
            this.groupBox32.Controls.Add(this.label40);
            this.groupBox32.Controls.Add(this.remountseriallabel);
            this.groupBox32.Controls.Add(this.label47);
            this.groupBox32.Controls.Add(this.remountsetbutton);
            this.groupBox32.Controls.Add(this.remountcheckbox);
            this.groupBox32.Location = new System.Drawing.Point(491, 186);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Size = new System.Drawing.Size(165, 118);
            this.groupBox32.TabIndex = 68;
            this.groupBox32.TabStop = false;
            this.groupBox32.Text = "Auto Remount";
            // 
            // remountedelay
            // 
            this.remountedelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remountedelay.BackColor = System.Drawing.Color.White;
            this.remountedelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.remountedelay.Location = new System.Drawing.Point(93, 89);
            this.remountedelay.Name = "remountedelay";
            this.remountedelay.Size = new System.Drawing.Size(58, 20);
            this.remountedelay.TabIndex = 68;
            this.remountedelay.Leave += new System.EventHandler(this.remountedelay_Leave);
            // 
            // remountdelay
            // 
            this.remountdelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remountdelay.BackColor = System.Drawing.Color.White;
            this.remountdelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.remountdelay.Location = new System.Drawing.Point(93, 64);
            this.remountdelay.Name = "remountdelay";
            this.remountdelay.Size = new System.Drawing.Size(58, 20);
            this.remountdelay.TabIndex = 67;
            this.remountdelay.Leave += new System.EventHandler(this.remountdelay_Leave);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(6, 91);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(79, 13);
            this.label48.TabIndex = 66;
            this.label48.Text = "Ethereal Delay:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(6, 70);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(70, 13);
            this.label40.TabIndex = 65;
            this.label40.Text = "Mount Delay:";
            // 
            // remountseriallabel
            // 
            this.remountseriallabel.AutoSize = true;
            this.remountseriallabel.Location = new System.Drawing.Point(90, 48);
            this.remountseriallabel.Name = "remountseriallabel";
            this.remountseriallabel.Size = new System.Drawing.Size(66, 13);
            this.remountseriallabel.TabIndex = 64;
            this.remountseriallabel.Text = "0x00000000";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(6, 48);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(69, 13);
            this.label47.TabIndex = 63;
            this.label47.Text = "Mount Serial:";
            // 
            // remountsetbutton
            // 
            this.remountsetbutton.Location = new System.Drawing.Point(73, 19);
            this.remountsetbutton.Name = "remountsetbutton";
            this.remountsetbutton.Size = new System.Drawing.Size(75, 21);
            this.remountsetbutton.TabIndex = 62;
            this.remountsetbutton.Text = "Set Mount";
            this.remountsetbutton.UseVisualStyleBackColor = true;
            this.remountsetbutton.Click += new System.EventHandler(this.remountsetbutton_Click);
            // 
            // remountcheckbox
            // 
            this.remountcheckbox.Location = new System.Drawing.Point(6, 19);
            this.remountcheckbox.Name = "remountcheckbox";
            this.remountcheckbox.Size = new System.Drawing.Size(62, 22);
            this.remountcheckbox.TabIndex = 61;
            this.remountcheckbox.Text = "Enable";
            this.remountcheckbox.CheckedChanged += new System.EventHandler(this.remountcheckbox_CheckedChanged);
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.colorflagsselfHighlightCheckBox);
            this.groupBox24.Controls.Add(this.showagentmessageCheckBox);
            this.groupBox24.Controls.Add(this.showmessagefieldCheckBox);
            this.groupBox24.Controls.Add(this.colorflagsHighlightCheckBox);
            this.groupBox24.Controls.Add(this.blockchivalryhealCheckBox);
            this.groupBox24.Controls.Add(this.blockbighealCheckBox);
            this.groupBox24.Controls.Add(this.blockminihealCheckBox);
            this.groupBox24.Controls.Add(this.blockhealpoisonCheckBox);
            this.groupBox24.Controls.Add(this.showheadtargetCheckBox);
            this.groupBox24.Controls.Add(this.blockpartyinviteCheckBox);
            this.groupBox24.Controls.Add(this.blocktraderequestCheckBox);
            this.groupBox24.Controls.Add(this.highlighttargetCheckBox);
            this.groupBox24.Controls.Add(this.flagsHighlightCheckBox);
            this.groupBox24.Controls.Add(this.showstaticfieldCheckBox);
            this.groupBox24.Location = new System.Drawing.Point(8, 14);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(178, 336);
            this.groupBox24.TabIndex = 67;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Misc";
            // 
            // colorflagsselfHighlightCheckBox
            // 
            this.colorflagsselfHighlightCheckBox.Location = new System.Drawing.Point(6, 86);
            this.colorflagsselfHighlightCheckBox.Name = "colorflagsselfHighlightCheckBox";
            this.colorflagsselfHighlightCheckBox.Size = new System.Drawing.Size(145, 22);
            this.colorflagsselfHighlightCheckBox.TabIndex = 71;
            this.colorflagsselfHighlightCheckBox.Text = "Color Flag Self Highlight";
            this.colorflagsselfHighlightCheckBox.CheckedChanged += new System.EventHandler(this.colorflagsselfHighlightCheckBox_CheckedChanged);
            // 
            // showagentmessageCheckBox
            // 
            this.showagentmessageCheckBox.Location = new System.Drawing.Point(6, 313);
            this.showagentmessageCheckBox.Name = "showagentmessageCheckBox";
            this.showagentmessageCheckBox.Size = new System.Drawing.Size(166, 22);
            this.showagentmessageCheckBox.TabIndex = 70;
            this.showagentmessageCheckBox.Text = "Show Agent Message";
            this.showagentmessageCheckBox.CheckedChanged += new System.EventHandler(this.showagentmessageCheckBox_CheckedChanged);
            // 
            // showmessagefieldCheckBox
            // 
            this.showmessagefieldCheckBox.Location = new System.Drawing.Point(6, 131);
            this.showmessagefieldCheckBox.Name = "showmessagefieldCheckBox";
            this.showmessagefieldCheckBox.Size = new System.Drawing.Size(157, 22);
            this.showmessagefieldCheckBox.TabIndex = 69;
            this.showmessagefieldCheckBox.Text = "Show Filed Message";
            this.showmessagefieldCheckBox.CheckedChanged += new System.EventHandler(this.showmessagefieldCheckBox_CheckedChanged);
            // 
            // colorflagsHighlightCheckBox
            // 
            this.colorflagsHighlightCheckBox.Location = new System.Drawing.Point(6, 63);
            this.colorflagsHighlightCheckBox.Name = "colorflagsHighlightCheckBox";
            this.colorflagsHighlightCheckBox.Size = new System.Drawing.Size(145, 22);
            this.colorflagsHighlightCheckBox.TabIndex = 68;
            this.colorflagsHighlightCheckBox.Text = "Color Flag Highlight";
            this.colorflagsHighlightCheckBox.CheckedChanged += new System.EventHandler(this.colorflagsHighlightCheckBox_CheckedChanged);
            // 
            // blockchivalryhealCheckBox
            // 
            this.blockchivalryhealCheckBox.Location = new System.Drawing.Point(6, 290);
            this.blockchivalryhealCheckBox.Name = "blockchivalryhealCheckBox";
            this.blockchivalryhealCheckBox.Size = new System.Drawing.Size(166, 22);
            this.blockchivalryhealCheckBox.TabIndex = 67;
            this.blockchivalryhealCheckBox.Text = "Block ChivaHeal if no need";
            this.blockchivalryhealCheckBox.CheckedChanged += new System.EventHandler(this.blockchivalryhealCheckBox_CheckedChanged);
            // 
            // blockbighealCheckBox
            // 
            this.blockbighealCheckBox.Location = new System.Drawing.Point(6, 267);
            this.blockbighealCheckBox.Name = "blockbighealCheckBox";
            this.blockbighealCheckBox.Size = new System.Drawing.Size(157, 22);
            this.blockbighealCheckBox.TabIndex = 66;
            this.blockbighealCheckBox.Text = "Block BigHeal if no need";
            this.blockbighealCheckBox.CheckedChanged += new System.EventHandler(this.blockbighealCheckBox_CheckedChanged);
            // 
            // blockminihealCheckBox
            // 
            this.blockminihealCheckBox.Location = new System.Drawing.Point(6, 244);
            this.blockminihealCheckBox.Name = "blockminihealCheckBox";
            this.blockminihealCheckBox.Size = new System.Drawing.Size(157, 22);
            this.blockminihealCheckBox.TabIndex = 65;
            this.blockminihealCheckBox.Text = "Block MiniHeal if no need";
            this.blockminihealCheckBox.CheckedChanged += new System.EventHandler(this.blockminihealCheckBox_CheckedChanged);
            // 
            // blockhealpoisonCheckBox
            // 
            this.blockhealpoisonCheckBox.Location = new System.Drawing.Point(6, 221);
            this.blockhealpoisonCheckBox.Name = "blockhealpoisonCheckBox";
            this.blockhealpoisonCheckBox.Size = new System.Drawing.Size(166, 22);
            this.blockhealpoisonCheckBox.TabIndex = 64;
            this.blockhealpoisonCheckBox.Text = "Block Heal if Poison/Mortal";
            this.blockhealpoisonCheckBox.CheckedChanged += new System.EventHandler(this.blockhealpoisonCheckBox_CheckedChanged);
            // 
            // showheadtargetCheckBox
            // 
            this.showheadtargetCheckBox.Location = new System.Drawing.Point(6, 198);
            this.showheadtargetCheckBox.Name = "showheadtargetCheckBox";
            this.showheadtargetCheckBox.Size = new System.Drawing.Size(141, 22);
            this.showheadtargetCheckBox.TabIndex = 63;
            this.showheadtargetCheckBox.Text = "Show Target on Head";
            this.showheadtargetCheckBox.CheckedChanged += new System.EventHandler(this.showheadtargetCheckBox_CheckedChanged);
            // 
            // blockpartyinviteCheckBox
            // 
            this.blockpartyinviteCheckBox.Location = new System.Drawing.Point(6, 175);
            this.blockpartyinviteCheckBox.Name = "blockpartyinviteCheckBox";
            this.blockpartyinviteCheckBox.Size = new System.Drawing.Size(141, 22);
            this.blockpartyinviteCheckBox.TabIndex = 62;
            this.blockpartyinviteCheckBox.Text = "Block Party Invite";
            this.blockpartyinviteCheckBox.CheckedChanged += new System.EventHandler(this.blockpartyinviteCheckBox_CheckedChanged);
            // 
            // blocktraderequestCheckBox
            // 
            this.blocktraderequestCheckBox.Location = new System.Drawing.Point(6, 153);
            this.blocktraderequestCheckBox.Name = "blocktraderequestCheckBox";
            this.blocktraderequestCheckBox.Size = new System.Drawing.Size(141, 22);
            this.blocktraderequestCheckBox.TabIndex = 61;
            this.blocktraderequestCheckBox.Text = "Block Trade Request";
            this.blocktraderequestCheckBox.CheckedChanged += new System.EventHandler(this.blocktraderequestCheckBox_CheckedChanged);
            // 
            // highlighttargetCheckBox
            // 
            this.highlighttargetCheckBox.Location = new System.Drawing.Point(6, 19);
            this.highlighttargetCheckBox.Name = "highlighttargetCheckBox";
            this.highlighttargetCheckBox.Size = new System.Drawing.Size(145, 22);
            this.highlighttargetCheckBox.TabIndex = 58;
            this.highlighttargetCheckBox.Text = "Text Current Target";
            this.highlighttargetCheckBox.CheckedChanged += new System.EventHandler(this.highlighttargetCheckBox_CheckedChanged);
            // 
            // flagsHighlightCheckBox
            // 
            this.flagsHighlightCheckBox.Location = new System.Drawing.Point(6, 41);
            this.flagsHighlightCheckBox.Name = "flagsHighlightCheckBox";
            this.flagsHighlightCheckBox.Size = new System.Drawing.Size(132, 22);
            this.flagsHighlightCheckBox.TabIndex = 59;
            this.flagsHighlightCheckBox.Text = "Text Flags Highlight";
            this.flagsHighlightCheckBox.CheckedChanged += new System.EventHandler(this.flagsHighlightCheckBox_CheckedChanged);
            // 
            // showstaticfieldCheckBox
            // 
            this.showstaticfieldCheckBox.Location = new System.Drawing.Point(6, 108);
            this.showstaticfieldCheckBox.Name = "showstaticfieldCheckBox";
            this.showstaticfieldCheckBox.Size = new System.Drawing.Size(118, 22);
            this.showstaticfieldCheckBox.TabIndex = 60;
            this.showstaticfieldCheckBox.Text = "Show Static Field";
            this.showstaticfieldCheckBox.CheckedChanged += new System.EventHandler(this.showstaticfieldCheckBox_CheckedChanged);
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.graphfilterdatagrid);
            this.groupBox23.Controls.Add(this.mobfilterCheckBox);
            this.groupBox23.Location = new System.Drawing.Point(192, 14);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(283, 265);
            this.groupBox23.TabIndex = 66;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Mobile Graphics Change Filter";
            // 
            // graphfilterdatagrid
            // 
            this.graphfilterdatagrid.AllowDrop = true;
            this.graphfilterdatagrid.AllowUserToResizeRows = false;
            this.graphfilterdatagrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.graphfilterdatagrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn4,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn17,
            this.dataGridViewTextBoxColumn18,
            this.dataGridViewTextBoxColumn19});
            this.graphfilterdatagrid.Location = new System.Drawing.Point(6, 47);
            this.graphfilterdatagrid.Name = "graphfilterdatagrid";
            this.graphfilterdatagrid.RowHeadersVisible = false;
            this.graphfilterdatagrid.RowHeadersWidth = 62;
            this.graphfilterdatagrid.Size = new System.Drawing.Size(271, 208);
            this.graphfilterdatagrid.TabIndex = 69;
            this.graphfilterdatagrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.graphfilterdatagrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.graphfilterdatagrid_CellEndEdit);
            this.graphfilterdatagrid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.graphfilterdatagrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.graphfilterdatagrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.graphfilterdatagrid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.graphfilterdatagrid_DefaultValuesNeeded);
            this.graphfilterdatagrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.graphfilterdatagrid.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.graphfilterdatagrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.graphfilterdatagrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // dataGridViewCheckBoxColumn4
            // 
            this.dataGridViewCheckBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn4.FalseValue = "False";
            this.dataGridViewCheckBoxColumn4.HeaderText = "X";
            this.dataGridViewCheckBoxColumn4.IndeterminateValue = "False";
            this.dataGridViewCheckBoxColumn4.MinimumWidth = 8;
            this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
            this.dataGridViewCheckBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn4.TrueValue = "True";
            this.dataGridViewCheckBoxColumn4.Width = 22;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn16.HeaderText = "Old Graphic";
            this.dataGridViewTextBoxColumn16.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Width = 75;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn17.HeaderText = "New Graphic";
            this.dataGridViewTextBoxColumn17.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn17.Width = 75;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn18.HeaderText = "New Color";
            this.dataGridViewTextBoxColumn18.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn18.Width = 75;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.HeaderText = "Props";
            this.dataGridViewTextBoxColumn19.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.Visible = false;
            this.dataGridViewTextBoxColumn19.Width = 150;
            // 
            // mobfilterCheckBox
            // 
            this.mobfilterCheckBox.Location = new System.Drawing.Point(6, 19);
            this.mobfilterCheckBox.Name = "mobfilterCheckBox";
            this.mobfilterCheckBox.Size = new System.Drawing.Size(79, 22);
            this.mobfilterCheckBox.TabIndex = 61;
            this.mobfilterCheckBox.Text = "Enable";
            this.mobfilterCheckBox.CheckedChanged += new System.EventHandler(this.mobfilterCheckBox_CheckedChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.autocarverbladeLabel);
            this.groupBox10.Controls.Add(this.label34);
            this.groupBox10.Controls.Add(this.autocarverrazorButton);
            this.groupBox10.Controls.Add(this.autocarverCheckBox);
            this.groupBox10.Location = new System.Drawing.Point(490, 14);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(166, 80);
            this.groupBox10.TabIndex = 65;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Auto Carver";
            // 
            // autocarverbladeLabel
            // 
            this.autocarverbladeLabel.AutoSize = true;
            this.autocarverbladeLabel.Location = new System.Drawing.Point(78, 48);
            this.autocarverbladeLabel.Name = "autocarverbladeLabel";
            this.autocarverbladeLabel.Size = new System.Drawing.Size(66, 13);
            this.autocarverbladeLabel.TabIndex = 64;
            this.autocarverbladeLabel.Text = "0x00000000";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 48);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(66, 13);
            this.label34.TabIndex = 63;
            this.label34.Text = "Blade Serial:";
            // 
            // autocarverrazorButton
            // 
            this.autocarverrazorButton.Location = new System.Drawing.Point(74, 18);
            this.autocarverrazorButton.Name = "autocarverrazorButton";
            this.autocarverrazorButton.Size = new System.Drawing.Size(75, 21);
            this.autocarverrazorButton.TabIndex = 62;
            this.autocarverrazorButton.Text = "Set Blade";
            this.autocarverrazorButton.UseVisualStyleBackColor = true;
            this.autocarverrazorButton.Click += new System.EventHandler(this.autocarverrazorButton_Click);
            // 
            // autocarverCheckBox
            // 
            this.autocarverCheckBox.Location = new System.Drawing.Point(6, 19);
            this.autocarverCheckBox.Name = "autocarverCheckBox";
            this.autocarverCheckBox.Size = new System.Drawing.Size(79, 22);
            this.autocarverCheckBox.TabIndex = 61;
            this.autocarverCheckBox.Text = "Enable";
            this.autocarverCheckBox.CheckedChanged += new System.EventHandler(this.autocarverCheckBox_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.bonebladeLabel);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Controls.Add(this.boneCutterrazorButton);
            this.groupBox9.Controls.Add(this.bonecutterCheckBox);
            this.groupBox9.Location = new System.Drawing.Point(490, 100);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(166, 80);
            this.groupBox9.TabIndex = 62;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Bone Cutter";
            // 
            // bonebladeLabel
            // 
            this.bonebladeLabel.AutoSize = true;
            this.bonebladeLabel.Location = new System.Drawing.Point(78, 48);
            this.bonebladeLabel.Name = "bonebladeLabel";
            this.bonebladeLabel.Size = new System.Drawing.Size(66, 13);
            this.bonebladeLabel.TabIndex = 64;
            this.bonebladeLabel.Text = "0x00000000";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 48);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 13);
            this.label16.TabIndex = 63;
            this.label16.Text = "Blade Serial:";
            // 
            // boneCutterrazorButton
            // 
            this.boneCutterrazorButton.Location = new System.Drawing.Point(74, 19);
            this.boneCutterrazorButton.Name = "boneCutterrazorButton";
            this.boneCutterrazorButton.Size = new System.Drawing.Size(75, 21);
            this.boneCutterrazorButton.TabIndex = 62;
            this.boneCutterrazorButton.Text = "Set Blade";
            this.boneCutterrazorButton.UseVisualStyleBackColor = true;
            this.boneCutterrazorButton.Click += new System.EventHandler(this.boneCutterrazorButton_Click);
            // 
            // bonecutterCheckBox
            // 
            this.bonecutterCheckBox.Location = new System.Drawing.Point(6, 19);
            this.bonecutterCheckBox.Name = "bonecutterCheckBox";
            this.bonecutterCheckBox.Size = new System.Drawing.Size(79, 22);
            this.bonecutterCheckBox.TabIndex = 61;
            this.bonecutterCheckBox.Text = "Enable";
            this.bonecutterCheckBox.CheckedChanged += new System.EventHandler(this.bonecutterCheckBox_CheckedChanged);
            // 
            // scriptingTab
            // 
            this.scriptingTab.BackColor = System.Drawing.SystemColors.Control;
            this.scriptingTab.Controls.Add(this.scriptshowStartStopCheckBox);
            this.scriptingTab.Controls.Add(this.groupBox42);
            this.scriptingTab.Controls.Add(this.groupBox31);
            this.scriptingTab.Controls.Add(this.groupBox30);
            this.scriptingTab.Controls.Add(this.scripterrorlogCheckBox);
            this.scriptingTab.Controls.Add(this.showscriptmessageCheckBox);
            this.scriptingTab.Controls.Add(this.scriptlistView);
            this.scriptingTab.Location = new System.Drawing.Point(4, 40);
            this.scriptingTab.Name = "scriptingTab";
            this.scriptingTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptingTab.Size = new System.Drawing.Size(666, 366);
            this.scriptingTab.TabIndex = 12;
            this.scriptingTab.Text = "Scripting";
            // 
            // scriptshowStartStopCheckBox
            // 
            this.scriptshowStartStopCheckBox.Location = new System.Drawing.Point(488, 342);
            this.scriptshowStartStopCheckBox.Name = "scriptshowStartStopCheckBox";
            this.scriptshowStartStopCheckBox.Size = new System.Drawing.Size(175, 22);
            this.scriptshowStartStopCheckBox.TabIndex = 76;
            this.scriptshowStartStopCheckBox.Text = "Show Start/Stop Message";
            this.scriptshowStartStopCheckBox.CheckedChanged += new System.EventHandler(this.scriptshowStartStopCheckBox_CheckedChanged);
            // 
            // groupBox42
            // 
            this.groupBox42.Controls.Add(this.scriptSearchTextBox);
            this.groupBox42.Location = new System.Drawing.Point(482, 249);
            this.groupBox42.Name = "groupBox42";
            this.groupBox42.Size = new System.Drawing.Size(175, 54);
            this.groupBox42.TabIndex = 75;
            this.groupBox42.TabStop = false;
            this.groupBox42.Text = "Search";
            // 
            // scriptSearchTextBox
            // 
            this.scriptSearchTextBox.Location = new System.Drawing.Point(6, 22);
            this.scriptSearchTextBox.Name = "scriptSearchTextBox";
            this.scriptSearchTextBox.Size = new System.Drawing.Size(157, 20);
            this.scriptSearchTextBox.TabIndex = 0;
            this.scriptSearchTextBox.Text = "Name to search..";
            this.scriptSearchTextBox.TextChanged += new System.EventHandler(this.scriptSearchTextBox_TextChanged);
            // 
            // groupBox31
            // 
            this.groupBox31.Controls.Add(this.buttonScriptEditorNew);
            this.groupBox31.Controls.Add(this.buttonScriptRefresh);
            this.groupBox31.Controls.Add(this.buttonAddScript);
            this.groupBox31.Controls.Add(this.buttonRemoveScript);
            this.groupBox31.Controls.Add(this.buttonScriptDown);
            this.groupBox31.Controls.Add(this.textBoxDelay);
            this.groupBox31.Controls.Add(this.buttonScriptUp);
            this.groupBox31.Controls.Add(this.buttonScriptEditor);
            this.groupBox31.Controls.Add(this.buttonScriptStop);
            this.groupBox31.Controls.Add(this.buttonScriptPlay);
            this.groupBox31.Location = new System.Drawing.Point(482, 112);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(175, 137);
            this.groupBox31.TabIndex = 50;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "Script Operation";
            // 
            // buttonScriptEditorNew
            // 
            this.buttonScriptEditorNew.Location = new System.Drawing.Point(7, 72);
            this.buttonScriptEditorNew.Name = "buttonScriptEditorNew";
            this.buttonScriptEditorNew.Size = new System.Drawing.Size(75, 21);
            this.buttonScriptEditorNew.TabIndex = 74;
            this.buttonScriptEditorNew.Text = "New";
            this.buttonScriptEditorNew.UseVisualStyleBackColor = true;
            this.buttonScriptEditorNew.Click += new System.EventHandler(this.buttonScriptEditorNew_Click);
            // 
            // buttonScriptRefresh
            // 
            this.buttonScriptRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonScriptRefresh.Image = ((System.Drawing.Image)(resources.GetObject("buttonScriptRefresh.Image")));
            this.buttonScriptRefresh.Location = new System.Drawing.Point(66, 98);
            this.buttonScriptRefresh.Name = "buttonScriptRefresh";
            this.buttonScriptRefresh.Size = new System.Drawing.Size(44, 27);
            this.buttonScriptRefresh.TabIndex = 73;
            this.buttonScriptRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonScriptRefresh.UseVisualStyleBackColor = true;
            this.buttonScriptRefresh.Click += new System.EventHandler(this.buttonScriptRefresh_Click);
            // 
            // buttonAddScript
            // 
            this.buttonAddScript.Location = new System.Drawing.Point(7, 19);
            this.buttonAddScript.Name = "buttonAddScript";
            this.buttonAddScript.Size = new System.Drawing.Size(75, 21);
            this.buttonAddScript.TabIndex = 14;
            this.buttonAddScript.Text = "Add";
            this.buttonAddScript.Click += new System.EventHandler(this.buttonScriptAdd_Click);
            // 
            // buttonRemoveScript
            // 
            this.buttonRemoveScript.Location = new System.Drawing.Point(94, 19);
            this.buttonRemoveScript.Name = "buttonRemoveScript";
            this.buttonRemoveScript.Size = new System.Drawing.Size(75, 21);
            this.buttonRemoveScript.TabIndex = 15;
            this.buttonRemoveScript.Text = "Remove";
            this.buttonRemoveScript.Click += new System.EventHandler(this.buttonScriptRemove_Click);
            // 
            // buttonScriptDown
            // 
            this.buttonScriptDown.Location = new System.Drawing.Point(7, 45);
            this.buttonScriptDown.Name = "buttonScriptDown";
            this.buttonScriptDown.Size = new System.Drawing.Size(75, 21);
            this.buttonScriptDown.TabIndex = 17;
            this.buttonScriptDown.Text = "Move Down";
            this.buttonScriptDown.UseVisualStyleBackColor = true;
            this.buttonScriptDown.Click += new System.EventHandler(this.buttonScriptDown_Click);
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(116, 204);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(42, 20);
            this.textBoxDelay.TabIndex = 23;
            this.textBoxDelay.Text = "100";
            this.textBoxDelay.Visible = false;
            this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxEngineDelay_TextChanged);
            // 
            // buttonScriptUp
            // 
            this.buttonScriptUp.Location = new System.Drawing.Point(94, 45);
            this.buttonScriptUp.Name = "buttonScriptUp";
            this.buttonScriptUp.Size = new System.Drawing.Size(75, 21);
            this.buttonScriptUp.TabIndex = 18;
            this.buttonScriptUp.Text = "Move Up";
            this.buttonScriptUp.UseVisualStyleBackColor = true;
            this.buttonScriptUp.Click += new System.EventHandler(this.buttonScriptUp_Click);
            // 
            // buttonScriptEditor
            // 
            this.buttonScriptEditor.Location = new System.Drawing.Point(94, 71);
            this.buttonScriptEditor.Name = "buttonScriptEditor";
            this.buttonScriptEditor.Size = new System.Drawing.Size(75, 21);
            this.buttonScriptEditor.TabIndex = 20;
            this.buttonScriptEditor.Text = "Edit";
            this.buttonScriptEditor.UseVisualStyleBackColor = true;
            this.buttonScriptEditor.Click += new System.EventHandler(this.buttonOpenEditor_Click);
            // 
            // buttonScriptStop
            // 
            this.buttonScriptStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonScriptStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonScriptStop.Image")));
            this.buttonScriptStop.Location = new System.Drawing.Point(10, 98);
            this.buttonScriptStop.Name = "buttonScriptStop";
            this.buttonScriptStop.Size = new System.Drawing.Size(44, 27);
            this.buttonScriptStop.TabIndex = 22;
            this.buttonScriptStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonScriptStop.UseVisualStyleBackColor = true;
            this.buttonScriptStop.Click += new System.EventHandler(this.buttonScriptStop_Click);
            // 
            // buttonScriptPlay
            // 
            this.buttonScriptPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonScriptPlay.Image = ((System.Drawing.Image)(resources.GetObject("buttonScriptPlay.Image")));
            this.buttonScriptPlay.Location = new System.Drawing.Point(122, 98);
            this.buttonScriptPlay.Name = "buttonScriptPlay";
            this.buttonScriptPlay.Size = new System.Drawing.Size(44, 27);
            this.buttonScriptPlay.TabIndex = 21;
            this.buttonScriptPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonScriptPlay.UseVisualStyleBackColor = true;
            this.buttonScriptPlay.Click += new System.EventHandler(this.buttonScriptPlay_Click);
            // 
            // groupBox30
            // 
            this.groupBox30.Controls.Add(this.scriptautostartcheckbox);
            this.groupBox30.Controls.Add(this.scriptwaitmodecheckbox);
            this.groupBox30.Controls.Add(this.scriptloopmodecheckbox);
            this.groupBox30.Controls.Add(this.scriptfilelabel);
            this.groupBox30.Location = new System.Drawing.Point(482, 6);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Size = new System.Drawing.Size(175, 100);
            this.groupBox30.TabIndex = 49;
            this.groupBox30.TabStop = false;
            this.groupBox30.Text = "Script Info";
            // 
            // scriptautostartcheckbox
            // 
            this.scriptautostartcheckbox.Location = new System.Drawing.Point(6, 76);
            this.scriptautostartcheckbox.Name = "scriptautostartcheckbox";
            this.scriptautostartcheckbox.Size = new System.Drawing.Size(138, 22);
            this.scriptautostartcheckbox.TabIndex = 51;
            this.scriptautostartcheckbox.Text = "AutoStart at Login";
            this.scriptautostartcheckbox.CheckedChanged += new System.EventHandler(this.scriptautostartcheckbox_CheckedChanged);
            // 
            // scriptwaitmodecheckbox
            // 
            this.scriptwaitmodecheckbox.Location = new System.Drawing.Point(6, 56);
            this.scriptwaitmodecheckbox.Name = "scriptwaitmodecheckbox";
            this.scriptwaitmodecheckbox.Size = new System.Drawing.Size(138, 22);
            this.scriptwaitmodecheckbox.TabIndex = 50;
            this.scriptwaitmodecheckbox.Text = "Wait before interrupt";
            this.scriptwaitmodecheckbox.CheckedChanged += new System.EventHandler(this.scriptwaitmodecheckbox_CheckedChanged);
            // 
            // scriptloopmodecheckbox
            // 
            this.scriptloopmodecheckbox.Location = new System.Drawing.Point(6, 36);
            this.scriptloopmodecheckbox.Name = "scriptloopmodecheckbox";
            this.scriptloopmodecheckbox.Size = new System.Drawing.Size(103, 22);
            this.scriptloopmodecheckbox.TabIndex = 49;
            this.scriptloopmodecheckbox.Text = "Loop Mode";
            this.scriptloopmodecheckbox.CheckedChanged += new System.EventHandler(this.scriptloopmodecheckbox_CheckedChanged);
            // 
            // scriptfilelabel
            // 
            this.scriptfilelabel.AutoSize = true;
            this.scriptfilelabel.Location = new System.Drawing.Point(4, 20);
            this.scriptfilelabel.Name = "scriptfilelabel";
            this.scriptfilelabel.Size = new System.Drawing.Size(29, 13);
            this.scriptfilelabel.TabIndex = 0;
            this.scriptfilelabel.Text = "File: ";
            // 
            // scripterrorlogCheckBox
            // 
            this.scripterrorlogCheckBox.Location = new System.Drawing.Point(488, 302);
            this.scripterrorlogCheckBox.Name = "scripterrorlogCheckBox";
            this.scripterrorlogCheckBox.Size = new System.Drawing.Size(160, 22);
            this.scripterrorlogCheckBox.TabIndex = 74;
            this.scripterrorlogCheckBox.Text = "Log Script Error";
            this.scripterrorlogCheckBox.CheckedChanged += new System.EventHandler(this.scripterrorlogCheckBox_CheckedChanged);
            // 
            // showscriptmessageCheckBox
            // 
            this.showscriptmessageCheckBox.Location = new System.Drawing.Point(488, 322);
            this.showscriptmessageCheckBox.Name = "showscriptmessageCheckBox";
            this.showscriptmessageCheckBox.Size = new System.Drawing.Size(175, 22);
            this.showscriptmessageCheckBox.TabIndex = 72;
            this.showscriptmessageCheckBox.Text = "Show Script Error Message";
            this.showscriptmessageCheckBox.CheckedChanged += new System.EventHandler(this.showscriptmessageCheckBox_CheckedChanged);
            // 
            // scriptlistView
            // 
            this.scriptlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader62,
            this.filename,
            this.status,
            this.loop,
            this.autostart,
            this.wait,
            this.hotkey,
            this.heypass});
            this.scriptlistView.FullRowSelect = true;
            this.scriptlistView.GridLines = true;
            this.scriptlistView.HideSelection = false;
            this.scriptlistView.LabelWrap = false;
            this.scriptlistView.Location = new System.Drawing.Point(8, 6);
            this.scriptlistView.MultiSelect = false;
            this.scriptlistView.Name = "scriptlistView";
            this.scriptlistView.Size = new System.Drawing.Size(468, 354);
            this.scriptlistView.TabIndex = 48;
            this.scriptlistView.UseCompatibleStateImageBehavior = false;
            this.scriptlistView.View = System.Windows.Forms.View.Details;
            this.scriptlistView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.scriptlistView_ColumnClick);
            this.scriptlistView.SelectedIndexChanged += new System.EventHandler(this.scriptlistView_SelectedIndexChanged);
            this.scriptlistView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.scriptlistView_MouseClick);
            this.scriptlistView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.scriptlistView_MouseDoubleClick);
            // 
            // columnHeader62
            // 
            this.columnHeader62.Text = "";
            this.columnHeader62.Width = 0;
            // 
            // filename
            // 
            this.filename.Text = "Filename";
            this.filename.Width = 150;
            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.status.Width = 67;
            // 
            // loop
            // 
            this.loop.Text = "Loop";
            this.loop.Width = 37;
            // 
            // autostart
            // 
            this.autostart.DisplayIndex = 5;
            this.autostart.Text = "A.S.";
            this.autostart.Width = 37;
            // 
            // wait
            // 
            this.wait.DisplayIndex = 4;
            this.wait.Text = "Wait";
            this.wait.Width = 37;
            // 
            // hotkey
            // 
            this.hotkey.Text = "HotKey";
            this.hotkey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hotkey.Width = 80;
            // 
            // heypass
            // 
            this.heypass.Text = "KeyPass";
            this.heypass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EnhancedAgent
            // 
            this.EnhancedAgent.Controls.Add(this.tabControl1);
            this.EnhancedAgent.Location = new System.Drawing.Point(4, 40);
            this.EnhancedAgent.Name = "EnhancedAgent";
            this.EnhancedAgent.Padding = new System.Windows.Forms.Padding(3);
            this.EnhancedAgent.Size = new System.Drawing.Size(666, 366);
            this.EnhancedAgent.TabIndex = 14;
            this.EnhancedAgent.Text = "Agents";
            this.EnhancedAgent.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.eautoloot);
            this.tabControl1.Controls.Add(this.escavenger);
            this.tabControl1.Controls.Add(this.organizer);
            this.tabControl1.Controls.Add(this.VendorBuy);
            this.tabControl1.Controls.Add(this.VendorSell);
            this.tabControl1.Controls.Add(this.Dress);
            this.tabControl1.Controls.Add(this.friends);
            this.tabControl1.Controls.Add(this.restock);
            this.tabControl1.Controls.Add(this.bandageheal);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(667, 367);
            this.tabControl1.TabIndex = 0;
            // 
            // eautoloot
            // 
            this.eautoloot.Controls.Add(this.autoLootButtonListClone);
            this.eautoloot.Controls.Add(this.autolootautostartCheckBox);
            this.eautoloot.Controls.Add(this.label60);
            this.eautoloot.Controls.Add(this.autoLootTextBoxMaxRange);
            this.eautoloot.Controls.Add(this.autolootItemPropsB);
            this.eautoloot.Controls.Add(this.groupBox14);
            this.eautoloot.Controls.Add(this.autolootAddItemBTarget);
            this.eautoloot.Controls.Add(this.autolootdataGridView);
            this.eautoloot.Controls.Add(this.autoLootnoopenCheckBox);
            this.eautoloot.Controls.Add(this.label21);
            this.eautoloot.Controls.Add(this.autoLootTextBoxDelay);
            this.eautoloot.Controls.Add(this.autoLootButtonRemoveList);
            this.eautoloot.Controls.Add(this.autolootButtonAddList);
            this.eautoloot.Controls.Add(this.autolootListSelect);
            this.eautoloot.Controls.Add(this.label20);
            this.eautoloot.Controls.Add(this.groupBox13);
            this.eautoloot.Controls.Add(this.autoLootCheckBox);
            this.eautoloot.Location = new System.Drawing.Point(4, 22);
            this.eautoloot.Name = "eautoloot";
            this.eautoloot.Padding = new System.Windows.Forms.Padding(3);
            this.eautoloot.Size = new System.Drawing.Size(659, 341);
            this.eautoloot.TabIndex = 0;
            this.eautoloot.Text = "Autoloot";
            this.eautoloot.UseVisualStyleBackColor = true;
            // 
            // autoLootButtonListClone
            // 
            this.autoLootButtonListClone.Location = new System.Drawing.Point(424, 12);
            this.autoLootButtonListClone.Name = "autoLootButtonListClone";
            this.autoLootButtonListClone.Size = new System.Drawing.Size(70, 21);
            this.autoLootButtonListClone.TabIndex = 67;
            this.autoLootButtonListClone.Text = "Clone";
            this.autoLootButtonListClone.Click += new System.EventHandler(this.autoLootButtonListClone_Click);
            // 
            // autolootautostartCheckBox
            // 
            this.autolootautostartCheckBox.Location = new System.Drawing.Point(275, 73);
            this.autolootautostartCheckBox.Name = "autolootautostartCheckBox";
            this.autolootautostartCheckBox.Size = new System.Drawing.Size(126, 22);
            this.autolootautostartCheckBox.TabIndex = 66;
            this.autolootautostartCheckBox.Text = "Autostart OnLogin";
            this.autolootautostartCheckBox.CheckedChanged += new System.EventHandler(this.autolootautostartCheckBox_CheckedChanged);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(464, 68);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(62, 13);
            this.label60.TabIndex = 65;
            this.label60.Text = "Max Range";
            // 
            // autoLootTextBoxMaxRange
            // 
            this.autoLootTextBoxMaxRange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoLootTextBoxMaxRange.BackColor = System.Drawing.Color.White;
            this.autoLootTextBoxMaxRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoLootTextBoxMaxRange.Location = new System.Drawing.Point(411, 65);
            this.autoLootTextBoxMaxRange.Name = "autoLootTextBoxMaxRange";
            this.autoLootTextBoxMaxRange.Size = new System.Drawing.Size(45, 20);
            this.autoLootTextBoxMaxRange.TabIndex = 64;
            this.autoLootTextBoxMaxRange.Leave += new System.EventHandler(this.autoLootTextBoxMaxRange_Leave);
            // 
            // autolootItemPropsB
            // 
            this.autolootItemPropsB.Location = new System.Drawing.Point(558, 66);
            this.autolootItemPropsB.Name = "autolootItemPropsB";
            this.autolootItemPropsB.Size = new System.Drawing.Size(90, 21);
            this.autolootItemPropsB.TabIndex = 49;
            this.autolootItemPropsB.Text = "Edit Props";
            this.autolootItemPropsB.Click += new System.EventHandler(this.autoLootItemProps_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label55);
            this.groupBox14.Controls.Add(this.autolootContainerLabel);
            this.groupBox14.Controls.Add(this.autolootContainerButton);
            this.groupBox14.Location = new System.Drawing.Point(9, 42);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(252, 47);
            this.groupBox14.TabIndex = 63;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "AutoLoot Bag";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(6, 21);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(36, 13);
            this.label55.TabIndex = 90;
            this.label55.Text = "Serial:";
            // 
            // autolootContainerLabel
            // 
            this.autolootContainerLabel.Location = new System.Drawing.Point(48, 21);
            this.autolootContainerLabel.Name = "autolootContainerLabel";
            this.autolootContainerLabel.Size = new System.Drawing.Size(82, 19);
            this.autolootContainerLabel.TabIndex = 50;
            this.autolootContainerLabel.Text = "0x00000000";
            // 
            // autolootContainerButton
            // 
            this.autolootContainerButton.Location = new System.Drawing.Point(157, 16);
            this.autolootContainerButton.Name = "autolootContainerButton";
            this.autolootContainerButton.Size = new System.Drawing.Size(89, 21);
            this.autolootContainerButton.TabIndex = 49;
            this.autolootContainerButton.Text = "Set Bag";
            this.autolootContainerButton.Click += new System.EventHandler(this.autolootContainerButton_Click);
            // 
            // autolootAddItemBTarget
            // 
            this.autolootAddItemBTarget.Location = new System.Drawing.Point(558, 39);
            this.autolootAddItemBTarget.Name = "autolootAddItemBTarget";
            this.autolootAddItemBTarget.Size = new System.Drawing.Size(90, 21);
            this.autolootAddItemBTarget.TabIndex = 47;
            this.autolootAddItemBTarget.Text = "Add Item";
            this.autolootAddItemBTarget.Click += new System.EventHandler(this.autoLootAddItemTarget_Click);
            // 
            // autolootdataGridView
            // 
            this.autolootdataGridView.AllowDrop = true;
            this.autolootdataGridView.AllowUserToResizeRows = false;
            this.autolootdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.autolootdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AutolootColumnX,
            this.AutolootColumnItemName,
            this.AutolootColumnItemID,
            this.AutolootColumnColor,
            this.LootBagColumnID,
            this.AutolootColumnProps});
            this.autolootdataGridView.Location = new System.Drawing.Point(9, 95);
            this.autolootdataGridView.Name = "autolootdataGridView";
            this.autolootdataGridView.RowHeadersVisible = false;
            this.autolootdataGridView.RowHeadersWidth = 62;
            this.autolootdataGridView.Size = new System.Drawing.Size(392, 238);
            this.autolootdataGridView.TabIndex = 62;
            this.autolootdataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.autolootdataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.autolootdataGridView_CellEndEdit);
            this.autolootdataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.autolootdataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.autolootdataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.autolootdataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.autolootdataGridView_DefaultValuesNeeded);
            this.autolootdataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.autolootdataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.autolootdataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.autolootdataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // AutolootColumnX
            // 
            this.AutolootColumnX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AutolootColumnX.FalseValue = "False";
            this.AutolootColumnX.HeaderText = "X";
            this.AutolootColumnX.IndeterminateValue = "False";
            this.AutolootColumnX.MinimumWidth = 8;
            this.AutolootColumnX.Name = "AutolootColumnX";
            this.AutolootColumnX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AutolootColumnX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.AutolootColumnX.TrueValue = "True";
            this.AutolootColumnX.Width = 22;
            // 
            // AutolootColumnItemName
            // 
            this.AutolootColumnItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AutolootColumnItemName.HeaderText = "Item Name";
            this.AutolootColumnItemName.MinimumWidth = 8;
            this.AutolootColumnItemName.Name = "AutolootColumnItemName";
            this.AutolootColumnItemName.Width = 206;
            // 
            // AutolootColumnItemID
            // 
            this.AutolootColumnItemID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AutolootColumnItemID.HeaderText = "Graphics";
            this.AutolootColumnItemID.MinimumWidth = 8;
            this.AutolootColumnItemID.Name = "AutolootColumnItemID";
            this.AutolootColumnItemID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AutolootColumnItemID.Width = 54;
            // 
            // AutolootColumnColor
            // 
            this.AutolootColumnColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AutolootColumnColor.HeaderText = "Color";
            this.AutolootColumnColor.MinimumWidth = 8;
            this.AutolootColumnColor.Name = "AutolootColumnColor";
            this.AutolootColumnColor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AutolootColumnColor.Width = 54;
            // 
            // LootBagColumnID
            // 
            this.LootBagColumnID.HeaderText = "Bag";
            this.LootBagColumnID.MaxInputLength = 65535;
            this.LootBagColumnID.MinimumWidth = 8;
            this.LootBagColumnID.Name = "LootBagColumnID";
            this.LootBagColumnID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LootBagColumnID.Width = 54;
            // 
            // AutolootColumnProps
            // 
            this.AutolootColumnProps.HeaderText = "Props";
            this.AutolootColumnProps.MinimumWidth = 8;
            this.AutolootColumnProps.Name = "AutolootColumnProps";
            this.AutolootColumnProps.Visible = false;
            this.AutolootColumnProps.Width = 150;
            // 
            // autoLootnoopenCheckBox
            // 
            this.autoLootnoopenCheckBox.Location = new System.Drawing.Point(275, 55);
            this.autoLootnoopenCheckBox.Name = "autoLootnoopenCheckBox";
            this.autoLootnoopenCheckBox.Size = new System.Drawing.Size(126, 22);
            this.autoLootnoopenCheckBox.TabIndex = 61;
            this.autoLootnoopenCheckBox.Text = "No Open Corpse";
            this.autoLootnoopenCheckBox.CheckedChanged += new System.EventHandler(this.autoLootnoopenCheckBox_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(464, 44);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 13);
            this.label21.TabIndex = 59;
            this.label21.Text = "Delay (ms)";
            // 
            // autoLootTextBoxDelay
            // 
            this.autoLootTextBoxDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoLootTextBoxDelay.BackColor = System.Drawing.Color.White;
            this.autoLootTextBoxDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoLootTextBoxDelay.Location = new System.Drawing.Point(411, 41);
            this.autoLootTextBoxDelay.Name = "autoLootTextBoxDelay";
            this.autoLootTextBoxDelay.Size = new System.Drawing.Size(45, 20);
            this.autoLootTextBoxDelay.TabIndex = 58;
            this.autoLootTextBoxDelay.Leave += new System.EventHandler(this.autoLootTextBoxDelay_Leave);
            // 
            // autoLootButtonRemoveList
            // 
            this.autoLootButtonRemoveList.Location = new System.Drawing.Point(347, 12);
            this.autoLootButtonRemoveList.Name = "autoLootButtonRemoveList";
            this.autoLootButtonRemoveList.Size = new System.Drawing.Size(71, 21);
            this.autoLootButtonRemoveList.TabIndex = 57;
            this.autoLootButtonRemoveList.Text = "Remove";
            this.autoLootButtonRemoveList.Click += new System.EventHandler(this.autoLootButtonRemoveList_Click);
            // 
            // autolootButtonAddList
            // 
            this.autolootButtonAddList.Location = new System.Drawing.Point(270, 12);
            this.autolootButtonAddList.Name = "autolootButtonAddList";
            this.autolootButtonAddList.Size = new System.Drawing.Size(71, 21);
            this.autolootButtonAddList.TabIndex = 56;
            this.autolootButtonAddList.Text = "Add";
            this.autolootButtonAddList.Click += new System.EventHandler(this.autoLootButtonAddList_Click);
            // 
            // autolootListSelect
            // 
            this.autolootListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autolootListSelect.FormattingEnabled = true;
            this.autolootListSelect.Location = new System.Drawing.Point(78, 12);
            this.autolootListSelect.Name = "autolootListSelect";
            this.autolootListSelect.Size = new System.Drawing.Size(183, 21);
            this.autolootListSelect.TabIndex = 55;
            this.autolootListSelect.SelectedIndexChanged += new System.EventHandler(this.autoLootListSelect_SelectedIndexChanged);
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
            this.groupBox13.Location = new System.Drawing.Point(407, 94);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(242, 239);
            this.groupBox13.TabIndex = 53;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Autoloot Log";
            // 
            // autolootLogBox
            // 
            this.autolootLogBox.FormattingEnabled = true;
            this.autolootLogBox.Location = new System.Drawing.Point(6, 19);
            this.autolootLogBox.Name = "autolootLogBox";
            this.autolootLogBox.Size = new System.Drawing.Size(232, 186);
            this.autolootLogBox.TabIndex = 0;
            // 
            // autoLootCheckBox
            // 
            this.autoLootCheckBox.Location = new System.Drawing.Point(275, 37);
            this.autoLootCheckBox.Name = "autoLootCheckBox";
            this.autoLootCheckBox.Size = new System.Drawing.Size(126, 22);
            this.autoLootCheckBox.TabIndex = 48;
            this.autoLootCheckBox.Text = "Enable autoloot";
            this.autoLootCheckBox.CheckedChanged += new System.EventHandler(this.autoLootEnable_CheckedChanged);
            // 
            // escavenger
            // 
            this.escavenger.Controls.Add(this.scavengerButtonClone);
            this.escavenger.Controls.Add(this.scavengerautostartCheckBox);
            this.escavenger.Controls.Add(this.label61);
            this.escavenger.Controls.Add(this.groupBox41);
            this.escavenger.Controls.Add(this.scavengerdataGridView);
            this.escavenger.Controls.Add(this.groupBox12);
            this.escavenger.Controls.Add(this.label23);
            this.escavenger.Controls.Add(this.label22);
            this.escavenger.Controls.Add(this.scavengerRange);
            this.escavenger.Controls.Add(this.scavengerButtonEditProps);
            this.escavenger.Controls.Add(this.scavengerButtonAddTarget);
            this.escavenger.Controls.Add(this.scavengerDragDelay);
            this.escavenger.Controls.Add(this.scavengerCheckBox);
            this.escavenger.Controls.Add(this.scavengerButtonRemoveList);
            this.escavenger.Controls.Add(this.scavengerButtonAddList);
            this.escavenger.Controls.Add(this.scavengerListSelect);
            this.escavenger.Location = new System.Drawing.Point(4, 22);
            this.escavenger.Name = "escavenger";
            this.escavenger.Padding = new System.Windows.Forms.Padding(3);
            this.escavenger.Size = new System.Drawing.Size(659, 341);
            this.escavenger.TabIndex = 1;
            this.escavenger.Text = "Scavenger";
            this.escavenger.UseVisualStyleBackColor = true;
            // 
            // scavengerButtonClone
            // 
            this.scavengerButtonClone.Location = new System.Drawing.Point(423, 11);
            this.scavengerButtonClone.Name = "scavengerButtonClone";
            this.scavengerButtonClone.Size = new System.Drawing.Size(68, 21);
            this.scavengerButtonClone.TabIndex = 77;
            this.scavengerButtonClone.Text = "Clone";
            this.scavengerButtonClone.Click += new System.EventHandler(this.scavengerButtonClone_Click);
            // 
            // scavengerautostartCheckBox
            // 
            this.scavengerautostartCheckBox.Location = new System.Drawing.Point(275, 67);
            this.scavengerautostartCheckBox.Name = "scavengerautostartCheckBox";
            this.scavengerautostartCheckBox.Size = new System.Drawing.Size(126, 22);
            this.scavengerautostartCheckBox.TabIndex = 76;
            this.scavengerautostartCheckBox.Text = "Autostart OnLogin";
            this.scavengerautostartCheckBox.CheckedChanged += new System.EventHandler(this.scavengerautostartCheckBox_CheckedChanged);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(469, 71);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(62, 13);
            this.label61.TabIndex = 75;
            this.label61.Text = "Max Range";
            // 
            // groupBox41
            // 
            this.groupBox41.Controls.Add(this.label54);
            this.groupBox41.Controls.Add(this.scavengerContainerLabel);
            this.groupBox41.Controls.Add(this.scavengerButtonSetContainer);
            this.groupBox41.Location = new System.Drawing.Point(9, 42);
            this.groupBox41.Name = "groupBox41";
            this.groupBox41.Size = new System.Drawing.Size(257, 47);
            this.groupBox41.TabIndex = 73;
            this.groupBox41.TabStop = false;
            this.groupBox41.Text = "Scavenger Bag";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(6, 21);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(36, 13);
            this.label54.TabIndex = 89;
            this.label54.Text = "Serial:";
            // 
            // scavengerContainerLabel
            // 
            this.scavengerContainerLabel.Location = new System.Drawing.Point(48, 21);
            this.scavengerContainerLabel.Name = "scavengerContainerLabel";
            this.scavengerContainerLabel.Size = new System.Drawing.Size(82, 19);
            this.scavengerContainerLabel.TabIndex = 67;
            this.scavengerContainerLabel.Text = "0x00000000";
            // 
            // scavengerButtonSetContainer
            // 
            this.scavengerButtonSetContainer.Location = new System.Drawing.Point(161, 16);
            this.scavengerButtonSetContainer.Name = "scavengerButtonSetContainer";
            this.scavengerButtonSetContainer.Size = new System.Drawing.Size(90, 21);
            this.scavengerButtonSetContainer.TabIndex = 66;
            this.scavengerButtonSetContainer.Text = "Set Bag";
            this.scavengerButtonSetContainer.Click += new System.EventHandler(this.scavengerSetContainer_Click);
            // 
            // scavengerdataGridView
            // 
            this.scavengerdataGridView.AllowDrop = true;
            this.scavengerdataGridView.AllowUserToResizeRows = false;
            this.scavengerdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scavengerdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScavengerX,
            this.ScavengerItemName,
            this.ScavenerGraphics,
            this.ScavengerColor,
            this.ScavengerProp});
            this.scavengerdataGridView.Location = new System.Drawing.Point(9, 95);
            this.scavengerdataGridView.Name = "scavengerdataGridView";
            this.scavengerdataGridView.RowHeadersVisible = false;
            this.scavengerdataGridView.RowHeadersWidth = 62;
            this.scavengerdataGridView.Size = new System.Drawing.Size(356, 238);
            this.scavengerdataGridView.TabIndex = 72;
            this.scavengerdataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.scavengerdataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.scavengerdataGridView_CellEndEdit);
            this.scavengerdataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.scavengerdataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.scavengerdataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.scavengerdataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.scavengerdataGridView_DefaultValuesNeeded);
            this.scavengerdataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.scavengerdataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.scavengerdataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.scavengerdataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // ScavengerX
            // 
            this.ScavengerX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ScavengerX.FalseValue = "False";
            this.ScavengerX.HeaderText = "X";
            this.ScavengerX.IndeterminateValue = "False";
            this.ScavengerX.MinimumWidth = 8;
            this.ScavengerX.Name = "ScavengerX";
            this.ScavengerX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ScavengerX.TrueValue = "True";
            this.ScavengerX.Width = 22;
            // 
            // ScavengerItemName
            // 
            this.ScavengerItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ScavengerItemName.HeaderText = "Item Name";
            this.ScavengerItemName.MinimumWidth = 8;
            this.ScavengerItemName.Name = "ScavengerItemName";
            this.ScavengerItemName.Width = 206;
            // 
            // ScavenerGraphics
            // 
            this.ScavenerGraphics.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ScavenerGraphics.HeaderText = "Graphics";
            this.ScavenerGraphics.MinimumWidth = 8;
            this.ScavenerGraphics.Name = "ScavenerGraphics";
            this.ScavenerGraphics.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ScavenerGraphics.Width = 54;
            // 
            // ScavengerColor
            // 
            this.ScavengerColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ScavengerColor.HeaderText = "Color";
            this.ScavengerColor.MinimumWidth = 8;
            this.ScavengerColor.Name = "ScavengerColor";
            this.ScavengerColor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ScavengerColor.Width = 54;
            // 
            // ScavengerProp
            // 
            this.ScavengerProp.HeaderText = "Prop";
            this.ScavengerProp.MinimumWidth = 8;
            this.ScavengerProp.Name = "ScavengerProp";
            this.ScavengerProp.Visible = false;
            this.ScavengerProp.Width = 150;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.scavengerLogBox);
            this.groupBox12.Location = new System.Drawing.Point(371, 94);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(278, 239);
            this.groupBox12.TabIndex = 70;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Scavenger Log";
            // 
            // scavengerLogBox
            // 
            this.scavengerLogBox.FormattingEnabled = true;
            this.scavengerLogBox.Location = new System.Drawing.Point(7, 18);
            this.scavengerLogBox.Name = "scavengerLogBox";
            this.scavengerLogBox.Size = new System.Drawing.Size(265, 186);
            this.scavengerLogBox.TabIndex = 0;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(469, 45);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(56, 13);
            this.label23.TabIndex = 69;
            this.label23.Text = "Delay (ms)";
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
            // scavengerRange
            // 
            this.scavengerRange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scavengerRange.BackColor = System.Drawing.Color.White;
            this.scavengerRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scavengerRange.Location = new System.Drawing.Point(416, 68);
            this.scavengerRange.Name = "scavengerRange";
            this.scavengerRange.Size = new System.Drawing.Size(45, 20);
            this.scavengerRange.TabIndex = 74;
            this.scavengerRange.Leave += new System.EventHandler(this.scavengerRange_Leave);
            // 
            // scavengerButtonEditProps
            // 
            this.scavengerButtonEditProps.Location = new System.Drawing.Point(563, 66);
            this.scavengerButtonEditProps.Name = "scavengerButtonEditProps";
            this.scavengerButtonEditProps.Size = new System.Drawing.Size(90, 21);
            this.scavengerButtonEditProps.TabIndex = 49;
            this.scavengerButtonEditProps.Text = "Edit Props";
            this.scavengerButtonEditProps.Click += new System.EventHandler(this.scavengerEditProps_Click);
            // 
            // scavengerButtonAddTarget
            // 
            this.scavengerButtonAddTarget.Location = new System.Drawing.Point(563, 39);
            this.scavengerButtonAddTarget.Name = "scavengerButtonAddTarget";
            this.scavengerButtonAddTarget.Size = new System.Drawing.Size(90, 21);
            this.scavengerButtonAddTarget.TabIndex = 47;
            this.scavengerButtonAddTarget.Text = "Add Item";
            this.scavengerButtonAddTarget.Click += new System.EventHandler(this.scavengerAddItemTarget_Click);
            // 
            // scavengerDragDelay
            // 
            this.scavengerDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scavengerDragDelay.BackColor = System.Drawing.Color.White;
            this.scavengerDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scavengerDragDelay.Location = new System.Drawing.Point(416, 42);
            this.scavengerDragDelay.Name = "scavengerDragDelay";
            this.scavengerDragDelay.Size = new System.Drawing.Size(45, 20);
            this.scavengerDragDelay.TabIndex = 68;
            this.scavengerDragDelay.Leave += new System.EventHandler(this.scavengerDragDelay_Leave);
            // 
            // scavengerCheckBox
            // 
            this.scavengerCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.scavengerCheckBox.Location = new System.Drawing.Point(275, 47);
            this.scavengerCheckBox.Name = "scavengerCheckBox";
            this.scavengerCheckBox.Size = new System.Drawing.Size(115, 22);
            this.scavengerCheckBox.TabIndex = 65;
            this.scavengerCheckBox.Text = "Enable scavenger";
            this.scavengerCheckBox.CheckedChanged += new System.EventHandler(this.scavengerEnableCheck_CheckedChanged);
            // 
            // scavengerButtonRemoveList
            // 
            this.scavengerButtonRemoveList.Location = new System.Drawing.Point(349, 12);
            this.scavengerButtonRemoveList.Name = "scavengerButtonRemoveList";
            this.scavengerButtonRemoveList.Size = new System.Drawing.Size(68, 21);
            this.scavengerButtonRemoveList.TabIndex = 63;
            this.scavengerButtonRemoveList.Text = "Remove";
            this.scavengerButtonRemoveList.Click += new System.EventHandler(this.scavengerRemoveList_Click);
            // 
            // scavengerButtonAddList
            // 
            this.scavengerButtonAddList.Location = new System.Drawing.Point(275, 12);
            this.scavengerButtonAddList.Name = "scavengerButtonAddList";
            this.scavengerButtonAddList.Size = new System.Drawing.Size(68, 21);
            this.scavengerButtonAddList.TabIndex = 62;
            this.scavengerButtonAddList.Text = "Add";
            this.scavengerButtonAddList.Click += new System.EventHandler(this.scavengerAddList_Click);
            // 
            // scavengerListSelect
            // 
            this.scavengerListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scavengerListSelect.FormattingEnabled = true;
            this.scavengerListSelect.Location = new System.Drawing.Point(91, 12);
            this.scavengerListSelect.Name = "scavengerListSelect";
            this.scavengerListSelect.Size = new System.Drawing.Size(175, 21);
            this.scavengerListSelect.TabIndex = 61;
            this.scavengerListSelect.SelectedIndexChanged += new System.EventHandler(this.scavengertListSelect_SelectedIndexChanged);
            // 
            // organizer
            // 
            this.organizer.Controls.Add(this.organizerCloneListB);
            this.organizer.Controls.Add(this.organizerExecuteButton);
            this.organizer.Controls.Add(this.organizerStopButton);
            this.organizer.Controls.Add(this.groupBox11);
            this.organizer.Controls.Add(this.organizerdataGridView);
            this.organizer.Controls.Add(this.groupBox16);
            this.organizer.Controls.Add(this.label27);
            this.organizer.Controls.Add(this.label24);
            this.organizer.Controls.Add(this.organizerAddTargetB);
            this.organizer.Controls.Add(this.organizerDragDelay);
            this.organizer.Controls.Add(this.organizerRemoveListB);
            this.organizer.Controls.Add(this.organizerAddListB);
            this.organizer.Controls.Add(this.organizerListSelect);
            this.organizer.Location = new System.Drawing.Point(4, 22);
            this.organizer.Name = "organizer";
            this.organizer.Padding = new System.Windows.Forms.Padding(3);
            this.organizer.Size = new System.Drawing.Size(659, 341);
            this.organizer.TabIndex = 2;
            this.organizer.Text = "Organizer";
            this.organizer.UseVisualStyleBackColor = true;
            // 
            // organizerCloneListB
            // 
            this.organizerCloneListB.Location = new System.Drawing.Point(425, 12);
            this.organizerCloneListB.Name = "organizerCloneListB";
            this.organizerCloneListB.Size = new System.Drawing.Size(70, 21);
            this.organizerCloneListB.TabIndex = 92;
            this.organizerCloneListB.Text = "Clone";
            this.organizerCloneListB.Click += new System.EventHandler(this.organizerCloneListB_Click);
            // 
            // organizerExecuteButton
            // 
            this.organizerExecuteButton.BackgroundImage = global::Assistant.Properties.Resources.playagent;
            this.organizerExecuteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.organizerExecuteButton.FlatAppearance.BorderSize = 0;
            this.organizerExecuteButton.Location = new System.Drawing.Point(283, 58);
            this.organizerExecuteButton.Name = "organizerExecuteButton";
            this.organizerExecuteButton.Size = new System.Drawing.Size(30, 30);
            this.organizerExecuteButton.TabIndex = 91;
            this.organizerExecuteButton.UseVisualStyleBackColor = true;
            this.organizerExecuteButton.Click += new System.EventHandler(this.organizerExecute_Click);
            // 
            // organizerStopButton
            // 
            this.organizerStopButton.BackgroundImage = global::Assistant.Properties.Resources.stopagent;
            this.organizerStopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.organizerStopButton.FlatAppearance.BorderSize = 0;
            this.organizerStopButton.Location = new System.Drawing.Point(319, 58);
            this.organizerStopButton.Name = "organizerStopButton";
            this.organizerStopButton.Size = new System.Drawing.Size(30, 30);
            this.organizerStopButton.TabIndex = 1;
            this.organizerStopButton.UseVisualStyleBackColor = true;
            this.organizerStopButton.Click += new System.EventHandler(this.organizerStop_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label57);
            this.groupBox11.Controls.Add(this.label56);
            this.groupBox11.Controls.Add(this.organizerSetSourceB);
            this.groupBox11.Controls.Add(this.organizerSetDestinationB);
            this.groupBox11.Controls.Add(this.organizerSourceLabel);
            this.groupBox11.Controls.Add(this.organizerDestinationLabel);
            this.groupBox11.Location = new System.Drawing.Point(9, 42);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(252, 65);
            this.groupBox11.TabIndex = 90;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Organizer Bags";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(6, 41);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(63, 13);
            this.label57.TabIndex = 91;
            this.label57.Text = "Destination:";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(6, 17);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(44, 13);
            this.label56.TabIndex = 90;
            this.label56.Text = "Source:";
            // 
            // organizerSetSourceB
            // 
            this.organizerSetSourceB.Location = new System.Drawing.Point(156, 12);
            this.organizerSetSourceB.Name = "organizerSetSourceB";
            this.organizerSetSourceB.Size = new System.Drawing.Size(90, 21);
            this.organizerSetSourceB.TabIndex = 66;
            this.organizerSetSourceB.Text = "Set Source";
            this.organizerSetSourceB.Click += new System.EventHandler(this.organizerSetSource_Click);
            // 
            // organizerSetDestinationB
            // 
            this.organizerSetDestinationB.Location = new System.Drawing.Point(156, 37);
            this.organizerSetDestinationB.Name = "organizerSetDestinationB";
            this.organizerSetDestinationB.Size = new System.Drawing.Size(90, 21);
            this.organizerSetDestinationB.TabIndex = 69;
            this.organizerSetDestinationB.Text = "Set Dest";
            this.organizerSetDestinationB.Click += new System.EventHandler(this.organizerSetDestination_Click);
            // 
            // organizerSourceLabel
            // 
            this.organizerSourceLabel.Location = new System.Drawing.Point(75, 17);
            this.organizerSourceLabel.Name = "organizerSourceLabel";
            this.organizerSourceLabel.Size = new System.Drawing.Size(82, 19);
            this.organizerSourceLabel.TabIndex = 67;
            this.organizerSourceLabel.Text = "0x00000000";
            // 
            // organizerDestinationLabel
            // 
            this.organizerDestinationLabel.Location = new System.Drawing.Point(75, 41);
            this.organizerDestinationLabel.Name = "organizerDestinationLabel";
            this.organizerDestinationLabel.Size = new System.Drawing.Size(82, 19);
            this.organizerDestinationLabel.TabIndex = 70;
            this.organizerDestinationLabel.Text = "0x00000000";
            // 
            // organizerdataGridView
            // 
            this.organizerdataGridView.AllowDrop = true;
            this.organizerdataGridView.AllowUserToResizeRows = false;
            this.organizerdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.organizerdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn7});
            this.organizerdataGridView.Location = new System.Drawing.Point(9, 113);
            this.organizerdataGridView.Name = "organizerdataGridView";
            this.organizerdataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.organizerdataGridView.RowHeadersVisible = false;
            this.organizerdataGridView.RowHeadersWidth = 62;
            this.organizerdataGridView.Size = new System.Drawing.Size(357, 220);
            this.organizerdataGridView.TabIndex = 89;
            this.organizerdataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.organizerdataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.organizerdataGridView_CellEndEdit);
            this.organizerdataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.organizerdataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.organizerdataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.organizerdataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.organizerdataGridView_DefaultValuesNeeded);
            this.organizerdataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.organizerdataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.organizerdataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.organizerdataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.FalseValue = "False";
            this.dataGridViewCheckBoxColumn2.HeaderText = "X";
            this.dataGridViewCheckBoxColumn2.IndeterminateValue = "False";
            this.dataGridViewCheckBoxColumn2.MinimumWidth = 8;
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn2.ToolTipText = "Check This for enable item in list";
            this.dataGridViewCheckBoxColumn2.TrueValue = "True";
            this.dataGridViewCheckBoxColumn2.Width = 22;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.ToolTipText = "Here the item name";
            this.dataGridViewTextBoxColumn5.Width = 153;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.HeaderText = "Graphics";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.ToolTipText = "Here Graphics item ID";
            this.dataGridViewTextBoxColumn6.Width = 54;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn8.HeaderText = "Color";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn8.ToolTipText = "Here item color, use -1 for all color";
            this.dataGridViewTextBoxColumn8.Width = 54;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn7.ToolTipText = "Here amount to move, use -1 for all item";
            this.dataGridViewTextBoxColumn7.Width = 54;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.organizerLogBox);
            this.groupBox16.Location = new System.Drawing.Point(373, 84);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(278, 251);
            this.groupBox16.TabIndex = 73;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Organizer Log";
            // 
            // organizerLogBox
            // 
            this.organizerLogBox.FormattingEnabled = true;
            this.organizerLogBox.Location = new System.Drawing.Point(6, 19);
            this.organizerLogBox.Name = "organizerLogBox";
            this.organizerLogBox.Size = new System.Drawing.Size(265, 212);
            this.organizerLogBox.TabIndex = 0;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(425, 55);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(105, 13);
            this.label27.TabIndex = 72;
            this.label27.Text = "Drag Item Delay (ms)";
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
            // organizerAddTargetB
            // 
            this.organizerAddTargetB.Location = new System.Drawing.Point(561, 52);
            this.organizerAddTargetB.Name = "organizerAddTargetB";
            this.organizerAddTargetB.Size = new System.Drawing.Size(90, 20);
            this.organizerAddTargetB.TabIndex = 47;
            this.organizerAddTargetB.Text = "Add Item";
            this.organizerAddTargetB.Click += new System.EventHandler(this.organizerAddTarget_Click);
            // 
            // organizerDragDelay
            // 
            this.organizerDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.organizerDragDelay.BackColor = System.Drawing.Color.White;
            this.organizerDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.organizerDragDelay.Location = new System.Drawing.Point(379, 52);
            this.organizerDragDelay.Name = "organizerDragDelay";
            this.organizerDragDelay.Size = new System.Drawing.Size(45, 20);
            this.organizerDragDelay.TabIndex = 71;
            this.organizerDragDelay.Leave += new System.EventHandler(this.organizerDragDelay_Leave);
            // 
            // organizerRemoveListB
            // 
            this.organizerRemoveListB.Location = new System.Drawing.Point(349, 12);
            this.organizerRemoveListB.Name = "organizerRemoveListB";
            this.organizerRemoveListB.Size = new System.Drawing.Size(70, 21);
            this.organizerRemoveListB.TabIndex = 63;
            this.organizerRemoveListB.Text = "Remove";
            this.organizerRemoveListB.Click += new System.EventHandler(this.organizerRemoveList_Click);
            // 
            // organizerAddListB
            // 
            this.organizerAddListB.Location = new System.Drawing.Point(273, 12);
            this.organizerAddListB.Name = "organizerAddListB";
            this.organizerAddListB.Size = new System.Drawing.Size(70, 21);
            this.organizerAddListB.TabIndex = 62;
            this.organizerAddListB.Text = "Add";
            this.organizerAddListB.Click += new System.EventHandler(this.organizerAddList_Click);
            // 
            // organizerListSelect
            // 
            this.organizerListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.organizerListSelect.FormattingEnabled = true;
            this.organizerListSelect.Location = new System.Drawing.Point(89, 12);
            this.organizerListSelect.Name = "organizerListSelect";
            this.organizerListSelect.Size = new System.Drawing.Size(172, 21);
            this.organizerListSelect.TabIndex = 61;
            this.organizerListSelect.SelectedIndexChanged += new System.EventHandler(this.organizerListSelect_SelectedIndexChanged);
            // 
            // VendorBuy
            // 
            this.VendorBuy.Controls.Add(this.buyCompareNameCheckBox);
            this.VendorBuy.Controls.Add(this.buyCloneButton);
            this.VendorBuy.Controls.Add(this.vendorbuydataGridView);
            this.VendorBuy.Controls.Add(this.groupBox18);
            this.VendorBuy.Controls.Add(this.label25);
            this.VendorBuy.Controls.Add(this.buyAddTargetB);
            this.VendorBuy.Controls.Add(this.buyEnableCheckBox);
            this.VendorBuy.Controls.Add(this.buyRemoveListButton);
            this.VendorBuy.Controls.Add(this.buyAddListButton);
            this.VendorBuy.Controls.Add(this.buyListSelect);
            this.VendorBuy.Location = new System.Drawing.Point(4, 22);
            this.VendorBuy.Name = "VendorBuy";
            this.VendorBuy.Padding = new System.Windows.Forms.Padding(3);
            this.VendorBuy.Size = new System.Drawing.Size(659, 341);
            this.VendorBuy.TabIndex = 3;
            this.VendorBuy.Text = "Vendor Buy";
            this.VendorBuy.UseVisualStyleBackColor = true;
            // 
            // buyCompareNameCheckBox
            // 
            this.buyCompareNameCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buyCompareNameCheckBox.Location = new System.Drawing.Point(373, 65);
            this.buyCompareNameCheckBox.Name = "buyCompareNameCheckBox";
            this.buyCompareNameCheckBox.Size = new System.Drawing.Size(135, 22);
            this.buyCompareNameCheckBox.TabIndex = 90;
            this.buyCompareNameCheckBox.Text = "Compare Item Name";
            this.buyCompareNameCheckBox.CheckedChanged += new System.EventHandler(this.buyCompareNameCheckBox_CheckedChanged);
            // 
            // buyCloneButton
            // 
            this.buyCloneButton.Location = new System.Drawing.Point(419, 12);
            this.buyCloneButton.Name = "buyCloneButton";
            this.buyCloneButton.Size = new System.Drawing.Size(67, 21);
            this.buyCloneButton.TabIndex = 89;
            this.buyCloneButton.Text = "Clone";
            this.buyCloneButton.Click += new System.EventHandler(this.buyCloneButton_Click);
            // 
            // vendorbuydataGridView
            // 
            this.vendorbuydataGridView.AllowDrop = true;
            this.vendorbuydataGridView.AllowUserToResizeRows = false;
            this.vendorbuydataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vendorbuydataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.vendorbuydataGridView.Location = new System.Drawing.Point(6, 54);
            this.vendorbuydataGridView.Name = "vendorbuydataGridView";
            this.vendorbuydataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.vendorbuydataGridView.RowHeadersVisible = false;
            this.vendorbuydataGridView.RowHeadersWidth = 62;
            this.vendorbuydataGridView.Size = new System.Drawing.Size(357, 274);
            this.vendorbuydataGridView.TabIndex = 88;
            this.vendorbuydataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.vendorbuydataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.vendorbuydataGridView_CellEndEdit);
            this.vendorbuydataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.vendorbuydataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.vendorbuydataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.vendorbuydataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.vendorbuydataGridView_DefaultValuesNeeded);
            this.vendorbuydataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.vendorbuydataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.vendorbuydataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.vendorbuydataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.FalseValue = "False";
            this.dataGridViewCheckBoxColumn1.HeaderText = "X";
            this.dataGridViewCheckBoxColumn1.IndeterminateValue = "False";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 8;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.ToolTipText = "Check This for enable item in list";
            this.dataGridViewCheckBoxColumn1.TrueValue = "True";
            this.dataGridViewCheckBoxColumn1.Width = 22;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.ToolTipText = "Here the item name";
            this.dataGridViewTextBoxColumn1.Width = 153;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "Graphics";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.ToolTipText = "Here Graphics item ID";
            this.dataGridViewTextBoxColumn2.Width = 54;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.ToolTipText = "Here Item Amount to sell";
            this.dataGridViewTextBoxColumn3.Width = 54;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.HeaderText = "Color";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.ToolTipText = "Here item color, use -1 for all color";
            this.dataGridViewTextBoxColumn4.Width = 54;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.buyLogBox);
            this.groupBox18.Location = new System.Drawing.Point(373, 93);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(278, 235);
            this.groupBox18.TabIndex = 73;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Buy Log";
            // 
            // buyLogBox
            // 
            this.buyLogBox.FormattingEnabled = true;
            this.buyLogBox.Location = new System.Drawing.Point(7, 18);
            this.buyLogBox.Name = "buyLogBox";
            this.buyLogBox.Size = new System.Drawing.Size(265, 186);
            this.buyLogBox.TabIndex = 0;
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
            // buyAddTargetB
            // 
            this.buyAddTargetB.Location = new System.Drawing.Point(561, 39);
            this.buyAddTargetB.Name = "buyAddTargetB";
            this.buyAddTargetB.Size = new System.Drawing.Size(90, 21);
            this.buyAddTargetB.TabIndex = 45;
            this.buyAddTargetB.Text = "Add Item";
            this.buyAddTargetB.Click += new System.EventHandler(this.buyAddTarget_Click);
            // 
            // buyEnableCheckBox
            // 
            this.buyEnableCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buyEnableCheckBox.Location = new System.Drawing.Point(373, 44);
            this.buyEnableCheckBox.Name = "buyEnableCheckBox";
            this.buyEnableCheckBox.Size = new System.Drawing.Size(135, 22);
            this.buyEnableCheckBox.TabIndex = 72;
            this.buyEnableCheckBox.Text = "Enable Buy List";
            this.buyEnableCheckBox.CheckedChanged += new System.EventHandler(this.buyEnableCheckB_CheckedChanged);
            // 
            // buyRemoveListButton
            // 
            this.buyRemoveListButton.Location = new System.Drawing.Point(346, 12);
            this.buyRemoveListButton.Name = "buyRemoveListButton";
            this.buyRemoveListButton.Size = new System.Drawing.Size(67, 21);
            this.buyRemoveListButton.TabIndex = 69;
            this.buyRemoveListButton.Text = "Remove";
            this.buyRemoveListButton.Click += new System.EventHandler(this.buyRemoveList_Click);
            // 
            // buyAddListButton
            // 
            this.buyAddListButton.Location = new System.Drawing.Point(273, 12);
            this.buyAddListButton.Name = "buyAddListButton";
            this.buyAddListButton.Size = new System.Drawing.Size(67, 21);
            this.buyAddListButton.TabIndex = 68;
            this.buyAddListButton.Text = "Add";
            this.buyAddListButton.Click += new System.EventHandler(this.buyAddList_Click);
            // 
            // buyListSelect
            // 
            this.buyListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buyListSelect.FormattingEnabled = true;
            this.buyListSelect.Location = new System.Drawing.Point(78, 12);
            this.buyListSelect.Name = "buyListSelect";
            this.buyListSelect.Size = new System.Drawing.Size(183, 21);
            this.buyListSelect.TabIndex = 67;
            this.buyListSelect.SelectedIndexChanged += new System.EventHandler(this.buyListSelect_SelectedIndexChanged);
            // 
            // VendorSell
            // 
            this.VendorSell.Controls.Add(this.sellCloneListButton);
            this.VendorSell.Controls.Add(this.groupBox19);
            this.VendorSell.Controls.Add(this.vendorsellGridView);
            this.VendorSell.Controls.Add(this.groupBox20);
            this.VendorSell.Controls.Add(this.label26);
            this.VendorSell.Controls.Add(this.sellAddTargerButton);
            this.VendorSell.Controls.Add(this.sellEnableCheckBox);
            this.VendorSell.Controls.Add(this.sellRemoveListButton);
            this.VendorSell.Controls.Add(this.sellAddListButton);
            this.VendorSell.Controls.Add(this.sellListSelect);
            this.VendorSell.Location = new System.Drawing.Point(4, 22);
            this.VendorSell.Name = "VendorSell";
            this.VendorSell.Padding = new System.Windows.Forms.Padding(3);
            this.VendorSell.Size = new System.Drawing.Size(659, 341);
            this.VendorSell.TabIndex = 4;
            this.VendorSell.Text = "Vendor Sell";
            this.VendorSell.UseVisualStyleBackColor = true;
            // 
            // sellCloneListButton
            // 
            this.sellCloneListButton.Location = new System.Drawing.Point(419, 12);
            this.sellCloneListButton.Name = "sellCloneListButton";
            this.sellCloneListButton.Size = new System.Drawing.Size(67, 21);
            this.sellCloneListButton.TabIndex = 90;
            this.sellCloneListButton.Text = "Clone";
            this.sellCloneListButton.Click += new System.EventHandler(this.sellCloneListButton_Click);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.sellSetBagButton);
            this.groupBox19.Controls.Add(this.label50);
            this.groupBox19.Controls.Add(this.sellBagLabel);
            this.groupBox19.Location = new System.Drawing.Point(6, 42);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(255, 42);
            this.groupBox19.TabIndex = 89;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Sell Bag";
            // 
            // sellSetBagButton
            // 
            this.sellSetBagButton.Location = new System.Drawing.Point(157, 14);
            this.sellSetBagButton.Name = "sellSetBagButton";
            this.sellSetBagButton.Size = new System.Drawing.Size(90, 21);
            this.sellSetBagButton.TabIndex = 85;
            this.sellSetBagButton.Text = "Set Bag";
            this.sellSetBagButton.Click += new System.EventHandler(this.sellSetBag_Click);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(6, 19);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(36, 13);
            this.label50.TabIndex = 88;
            this.label50.Text = "Serial:";
            // 
            // sellBagLabel
            // 
            this.sellBagLabel.Location = new System.Drawing.Point(47, 19);
            this.sellBagLabel.Name = "sellBagLabel";
            this.sellBagLabel.Size = new System.Drawing.Size(72, 19);
            this.sellBagLabel.TabIndex = 86;
            this.sellBagLabel.Text = "0x00000000";
            // 
            // vendorsellGridView
            // 
            this.vendorsellGridView.AllowDrop = true;
            this.vendorsellGridView.AllowUserToResizeRows = false;
            this.vendorsellGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vendorsellGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VendorSellX,
            this.VendorSellItemName,
            this.VendorSellGraphics,
            this.VendorSellAmount,
            this.VendorSellColor});
            this.vendorsellGridView.Location = new System.Drawing.Point(6, 90);
            this.vendorsellGridView.Name = "vendorsellGridView";
            this.vendorsellGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.vendorsellGridView.RowHeadersVisible = false;
            this.vendorsellGridView.RowHeadersWidth = 62;
            this.vendorsellGridView.Size = new System.Drawing.Size(357, 238);
            this.vendorsellGridView.TabIndex = 87;
            this.vendorsellGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.vendorsellGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.vendorsellGridView_CellEndEdit);
            this.vendorsellGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.vendorsellGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.vendorsellGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.vendorsellGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.vendorsellGridView_DefaultValuesNeeded);
            this.vendorsellGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.vendorsellGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.vendorsellGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.vendorsellGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // VendorSellX
            // 
            this.VendorSellX.FalseValue = "False";
            this.VendorSellX.HeaderText = "X";
            this.VendorSellX.IndeterminateValue = "False";
            this.VendorSellX.MinimumWidth = 8;
            this.VendorSellX.Name = "VendorSellX";
            this.VendorSellX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VendorSellX.ToolTipText = "Check This for enable item in list";
            this.VendorSellX.TrueValue = "True";
            this.VendorSellX.Width = 22;
            // 
            // VendorSellItemName
            // 
            this.VendorSellItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VendorSellItemName.HeaderText = "Item Name";
            this.VendorSellItemName.MinimumWidth = 8;
            this.VendorSellItemName.Name = "VendorSellItemName";
            this.VendorSellItemName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VendorSellItemName.ToolTipText = "Here the item name";
            this.VendorSellItemName.Width = 153;
            // 
            // VendorSellGraphics
            // 
            this.VendorSellGraphics.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VendorSellGraphics.HeaderText = "Graphics";
            this.VendorSellGraphics.MinimumWidth = 8;
            this.VendorSellGraphics.Name = "VendorSellGraphics";
            this.VendorSellGraphics.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VendorSellGraphics.ToolTipText = "Here Graphics item ID";
            this.VendorSellGraphics.Width = 54;
            // 
            // VendorSellAmount
            // 
            this.VendorSellAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VendorSellAmount.HeaderText = "Amount";
            this.VendorSellAmount.MinimumWidth = 8;
            this.VendorSellAmount.Name = "VendorSellAmount";
            this.VendorSellAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VendorSellAmount.ToolTipText = "Here Item Amount to sell";
            this.VendorSellAmount.Width = 54;
            // 
            // VendorSellColor
            // 
            this.VendorSellColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VendorSellColor.HeaderText = "Color";
            this.VendorSellColor.MinimumWidth = 8;
            this.VendorSellColor.Name = "VendorSellColor";
            this.VendorSellColor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VendorSellColor.ToolTipText = "Here item color, use -1 for all color";
            this.VendorSellColor.Width = 54;
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.sellLogBox);
            this.groupBox20.Location = new System.Drawing.Point(373, 72);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(278, 256);
            this.groupBox20.TabIndex = 83;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Sell Log";
            // 
            // sellLogBox
            // 
            this.sellLogBox.FormattingEnabled = true;
            this.sellLogBox.Location = new System.Drawing.Point(7, 18);
            this.sellLogBox.Name = "sellLogBox";
            this.sellLogBox.Size = new System.Drawing.Size(265, 212);
            this.sellLogBox.TabIndex = 0;
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
            // sellAddTargerButton
            // 
            this.sellAddTargerButton.Location = new System.Drawing.Point(561, 49);
            this.sellAddTargerButton.Name = "sellAddTargerButton";
            this.sellAddTargerButton.Size = new System.Drawing.Size(90, 21);
            this.sellAddTargerButton.TabIndex = 47;
            this.sellAddTargerButton.Text = "Add Item";
            this.sellAddTargerButton.Click += new System.EventHandler(this.sellAddTarget_Click);
            // 
            // sellEnableCheckBox
            // 
            this.sellEnableCheckBox.Location = new System.Drawing.Point(273, 49);
            this.sellEnableCheckBox.Name = "sellEnableCheckBox";
            this.sellEnableCheckBox.Size = new System.Drawing.Size(105, 22);
            this.sellEnableCheckBox.TabIndex = 82;
            this.sellEnableCheckBox.Text = "Enable Sell List";
            this.sellEnableCheckBox.CheckedChanged += new System.EventHandler(this.sellEnableCheck_CheckedChanged);
            // 
            // sellRemoveListButton
            // 
            this.sellRemoveListButton.Location = new System.Drawing.Point(346, 12);
            this.sellRemoveListButton.Name = "sellRemoveListButton";
            this.sellRemoveListButton.Size = new System.Drawing.Size(67, 21);
            this.sellRemoveListButton.TabIndex = 80;
            this.sellRemoveListButton.Text = "Remove";
            this.sellRemoveListButton.Click += new System.EventHandler(this.sellRemoveList_Click);
            // 
            // sellAddListButton
            // 
            this.sellAddListButton.Location = new System.Drawing.Point(273, 12);
            this.sellAddListButton.Name = "sellAddListButton";
            this.sellAddListButton.Size = new System.Drawing.Size(67, 21);
            this.sellAddListButton.TabIndex = 79;
            this.sellAddListButton.Text = "Add";
            this.sellAddListButton.Click += new System.EventHandler(this.sellAddList_Click);
            // 
            // sellListSelect
            // 
            this.sellListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sellListSelect.FormattingEnabled = true;
            this.sellListSelect.Location = new System.Drawing.Point(78, 12);
            this.sellListSelect.Name = "sellListSelect";
            this.sellListSelect.Size = new System.Drawing.Size(183, 21);
            this.sellListSelect.TabIndex = 78;
            this.sellListSelect.SelectedIndexChanged += new System.EventHandler(this.sellListSelect_SelectedIndexChanged);
            // 
            // Dress
            // 
            this.Dress.Controls.Add(this.dressStopButton);
            this.Dress.Controls.Add(this.dressConflictCheckB);
            this.Dress.Controls.Add(this.dressBagLabel);
            this.Dress.Controls.Add(this.groupBox22);
            this.Dress.Controls.Add(this.label29);
            this.Dress.Controls.Add(this.groupBox21);
            this.Dress.Controls.Add(this.dressSetBagB);
            this.Dress.Controls.Add(this.undressExecuteButton);
            this.Dress.Controls.Add(this.dressExecuteButton);
            this.Dress.Controls.Add(this.dressDragDelay);
            this.Dress.Controls.Add(this.dressListView);
            this.Dress.Controls.Add(this.label28);
            this.Dress.Controls.Add(this.dressRemoveListB);
            this.Dress.Controls.Add(this.dressAddListB);
            this.Dress.Controls.Add(this.dressListSelect);
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
            this.dressStopButton.Location = new System.Drawing.Point(407, 58);
            this.dressStopButton.Name = "dressStopButton";
            this.dressStopButton.Size = new System.Drawing.Size(61, 20);
            this.dressStopButton.TabIndex = 91;
            this.dressStopButton.Text = "Stop";
            this.dressStopButton.Click += new System.EventHandler(this.dressStopButton_Click);
            // 
            // dressConflictCheckB
            // 
            this.dressConflictCheckB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dressConflictCheckB.Location = new System.Drawing.Point(274, 84);
            this.dressConflictCheckB.Name = "dressConflictCheckB";
            this.dressConflictCheckB.Size = new System.Drawing.Size(224, 22);
            this.dressConflictCheckB.TabIndex = 90;
            this.dressConflictCheckB.Text = "Remove Conflict Item";
            this.dressConflictCheckB.CheckedChanged += new System.EventHandler(this.dressConflictCheckB_CheckedChanged);
            // 
            // dressBagLabel
            // 
            this.dressBagLabel.Location = new System.Drawing.Point(572, 142);
            this.dressBagLabel.Name = "dressBagLabel";
            this.dressBagLabel.Size = new System.Drawing.Size(82, 19);
            this.dressBagLabel.TabIndex = 89;
            this.dressBagLabel.Text = "0x00000000";
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.dressAddTargetB);
            this.groupBox22.Controls.Add(this.dressAddManualB);
            this.groupBox22.Controls.Add(this.dressRemoveB);
            this.groupBox22.Controls.Add(this.dressClearListB);
            this.groupBox22.Controls.Add(this.dressReadB);
            this.groupBox22.Location = new System.Drawing.Point(555, 164);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(100, 149);
            this.groupBox22.TabIndex = 85;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Item List";
            // 
            // dressAddTargetB
            // 
            this.dressAddTargetB.Location = new System.Drawing.Point(5, 68);
            this.dressAddTargetB.Name = "dressAddTargetB";
            this.dressAddTargetB.Size = new System.Drawing.Size(90, 20);
            this.dressAddTargetB.TabIndex = 48;
            this.dressAddTargetB.Text = "Add Target";
            this.dressAddTargetB.Click += new System.EventHandler(this.dressAddTargetB_Click);
            // 
            // dressAddManualB
            // 
            this.dressAddManualB.Location = new System.Drawing.Point(5, 43);
            this.dressAddManualB.Name = "dressAddManualB";
            this.dressAddManualB.Size = new System.Drawing.Size(90, 20);
            this.dressAddManualB.TabIndex = 47;
            this.dressAddManualB.Text = "Add Clear Layer";
            this.dressAddManualB.Click += new System.EventHandler(this.dressAddManualB_Click);
            // 
            // dressRemoveB
            // 
            this.dressRemoveB.Location = new System.Drawing.Point(5, 94);
            this.dressRemoveB.Name = "dressRemoveB";
            this.dressRemoveB.Size = new System.Drawing.Size(90, 20);
            this.dressRemoveB.TabIndex = 46;
            this.dressRemoveB.Text = "Remove";
            this.dressRemoveB.Click += new System.EventHandler(this.dressRemoveB_Click);
            // 
            // dressClearListB
            // 
            this.dressClearListB.Location = new System.Drawing.Point(5, 120);
            this.dressClearListB.Name = "dressClearListB";
            this.dressClearListB.Size = new System.Drawing.Size(90, 20);
            this.dressClearListB.TabIndex = 111;
            this.dressClearListB.Text = "ClearList";
            this.dressClearListB.Click += new System.EventHandler(this.dressClearListB_Click);
            // 
            // dressReadB
            // 
            this.dressReadB.Location = new System.Drawing.Point(5, 18);
            this.dressReadB.Name = "dressReadB";
            this.dressReadB.Size = new System.Drawing.Size(90, 20);
            this.dressReadB.TabIndex = 45;
            this.dressReadB.Text = "Read Current";
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
            this.dressLogBox.Size = new System.Drawing.Size(265, 173);
            this.dressLogBox.TabIndex = 0;
            // 
            // dressSetBagB
            // 
            this.dressSetBagB.Location = new System.Drawing.Point(560, 115);
            this.dressSetBagB.Name = "dressSetBagB";
            this.dressSetBagB.Size = new System.Drawing.Size(88, 20);
            this.dressSetBagB.TabIndex = 88;
            this.dressSetBagB.Text = "Undress Bag";
            this.dressSetBagB.Click += new System.EventHandler(this.dressSetBagB_Click);
            // 
            // undressExecuteButton
            // 
            this.undressExecuteButton.Location = new System.Drawing.Point(340, 58);
            this.undressExecuteButton.Name = "undressExecuteButton";
            this.undressExecuteButton.Size = new System.Drawing.Size(61, 20);
            this.undressExecuteButton.TabIndex = 87;
            this.undressExecuteButton.Text = "Undres";
            this.undressExecuteButton.Click += new System.EventHandler(this.razorButton10_Click);
            // 
            // dressExecuteButton
            // 
            this.dressExecuteButton.Location = new System.Drawing.Point(274, 58);
            this.dressExecuteButton.Name = "dressExecuteButton";
            this.dressExecuteButton.Size = new System.Drawing.Size(61, 20);
            this.dressExecuteButton.TabIndex = 86;
            this.dressExecuteButton.Text = "Dress";
            this.dressExecuteButton.Click += new System.EventHandler(this.dressExecuteButton_Click);
            // 
            // dressDragDelay
            // 
            this.dressDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dressDragDelay.BackColor = System.Drawing.Color.White;
            this.dressDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dressDragDelay.Location = new System.Drawing.Point(475, 58);
            this.dressDragDelay.Name = "dressDragDelay";
            this.dressDragDelay.Size = new System.Drawing.Size(45, 20);
            this.dressDragDelay.TabIndex = 75;
            this.dressDragDelay.Leave += new System.EventHandler(this.dressDragDelay_Leave);
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
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 18);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(56, 13);
            this.label28.TabIndex = 60;
            this.label28.Text = "Dress List:";
            // 
            // dressRemoveListB
            // 
            this.dressRemoveListB.Location = new System.Drawing.Point(366, 12);
            this.dressRemoveListB.Name = "dressRemoveListB";
            this.dressRemoveListB.Size = new System.Drawing.Size(90, 21);
            this.dressRemoveListB.TabIndex = 63;
            this.dressRemoveListB.Text = "Remove";
            this.dressRemoveListB.Click += new System.EventHandler(this.dressRemoveListB_Click);
            // 
            // dressAddListB
            // 
            this.dressAddListB.Location = new System.Drawing.Point(270, 12);
            this.dressAddListB.Name = "dressAddListB";
            this.dressAddListB.Size = new System.Drawing.Size(90, 21);
            this.dressAddListB.TabIndex = 62;
            this.dressAddListB.Text = "Add";
            this.dressAddListB.Click += new System.EventHandler(this.dressAddListB_Click);
            // 
            // dressListSelect
            // 
            this.dressListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dressListSelect.FormattingEnabled = true;
            this.dressListSelect.Location = new System.Drawing.Point(78, 12);
            this.dressListSelect.Name = "dressListSelect";
            this.dressListSelect.Size = new System.Drawing.Size(183, 21);
            this.dressListSelect.TabIndex = 61;
            this.dressListSelect.SelectedIndexChanged += new System.EventHandler(this.dressListSelect_SelectedIndexChanged);
            // 
            // friends
            // 
            this.friends.Controls.Add(this.groupBox34);
            this.friends.Controls.Add(this.groupBox33);
            this.friends.Controls.Add(this.friendguildListView);
            this.friends.Controls.Add(this.friendGroupBox);
            this.friends.Controls.Add(this.friendloggroupBox);
            this.friends.Controls.Add(this.friendIncludePartyCheckBox);
            this.friends.Controls.Add(this.friendAttackCheckBox);
            this.friends.Controls.Add(this.friendPartyCheckBox);
            this.friends.Controls.Add(this.friendlistView);
            this.friends.Controls.Add(this.labelfriend);
            this.friends.Controls.Add(this.friendButtonRemoveList);
            this.friends.Controls.Add(this.friendButtonAddList);
            this.friends.Controls.Add(this.friendListSelect);
            this.friends.Location = new System.Drawing.Point(4, 22);
            this.friends.Name = "friends";
            this.friends.Padding = new System.Windows.Forms.Padding(3);
            this.friends.Size = new System.Drawing.Size(659, 341);
            this.friends.TabIndex = 6;
            this.friends.Text = "Friends";
            this.friends.UseVisualStyleBackColor = true;
            // 
            // groupBox34
            // 
            this.groupBox34.Controls.Add(this.FriendGuildAddButton);
            this.groupBox34.Controls.Add(this.FriendGuildRemoveButton);
            this.groupBox34.Location = new System.Drawing.Point(547, 230);
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.Size = new System.Drawing.Size(106, 75);
            this.groupBox34.TabIndex = 82;
            this.groupBox34.TabStop = false;
            this.groupBox34.Text = "Guild Friend";
            // 
            // FriendGuildAddButton
            // 
            this.FriendGuildAddButton.Location = new System.Drawing.Point(9, 19);
            this.FriendGuildAddButton.Name = "FriendGuildAddButton";
            this.FriendGuildAddButton.Size = new System.Drawing.Size(90, 20);
            this.FriendGuildAddButton.TabIndex = 80;
            this.FriendGuildAddButton.Text = "Add Manual";
            this.FriendGuildAddButton.Click += new System.EventHandler(this.FriendGuildAddButton_Click);
            // 
            // FriendGuildRemoveButton
            // 
            this.FriendGuildRemoveButton.Location = new System.Drawing.Point(9, 45);
            this.FriendGuildRemoveButton.Name = "FriendGuildRemoveButton";
            this.FriendGuildRemoveButton.Size = new System.Drawing.Size(90, 20);
            this.FriendGuildRemoveButton.TabIndex = 81;
            this.FriendGuildRemoveButton.Text = "Remove";
            this.FriendGuildRemoveButton.Click += new System.EventHandler(this.FriendGuildRemoveButton_Click);
            // 
            // groupBox33
            // 
            this.groupBox33.Controls.Add(this.MINfriendCheckBox);
            this.groupBox33.Controls.Add(this.SLfriendCheckBox);
            this.groupBox33.Controls.Add(this.TBfriendCheckBox);
            this.groupBox33.Controls.Add(this.COMfriendCheckBox);
            this.groupBox33.Location = new System.Drawing.Point(547, 147);
            this.groupBox33.Name = "groupBox33";
            this.groupBox33.Size = new System.Drawing.Size(106, 77);
            this.groupBox33.TabIndex = 78;
            this.groupBox33.TabStop = false;
            this.groupBox33.Text = "Faction Friend";
            // 
            // MINfriendCheckBox
            // 
            this.MINfriendCheckBox.Location = new System.Drawing.Point(50, 47);
            this.MINfriendCheckBox.Name = "MINfriendCheckBox";
            this.MINfriendCheckBox.Size = new System.Drawing.Size(47, 22);
            this.MINfriendCheckBox.TabIndex = 81;
            this.MINfriendCheckBox.Text = "MIN";
            this.MINfriendCheckBox.CheckedChanged += new System.EventHandler(this.MINfriendCheckBox_CheckedChanged);
            // 
            // SLfriendCheckBox
            // 
            this.SLfriendCheckBox.Location = new System.Drawing.Point(5, 20);
            this.SLfriendCheckBox.Name = "SLfriendCheckBox";
            this.SLfriendCheckBox.Size = new System.Drawing.Size(41, 22);
            this.SLfriendCheckBox.TabIndex = 78;
            this.SLfriendCheckBox.Text = "SL";
            this.SLfriendCheckBox.CheckedChanged += new System.EventHandler(this.SLfriendCheckBox_CheckedChanged);
            // 
            // TBfriendCheckBox
            // 
            this.TBfriendCheckBox.Location = new System.Drawing.Point(5, 47);
            this.TBfriendCheckBox.Name = "TBfriendCheckBox";
            this.TBfriendCheckBox.Size = new System.Drawing.Size(41, 22);
            this.TBfriendCheckBox.TabIndex = 79;
            this.TBfriendCheckBox.Text = "TB";
            this.TBfriendCheckBox.CheckedChanged += new System.EventHandler(this.TBfriendCheckBox_CheckedChanged);
            // 
            // COMfriendCheckBox
            // 
            this.COMfriendCheckBox.Location = new System.Drawing.Point(50, 20);
            this.COMfriendCheckBox.Name = "COMfriendCheckBox";
            this.COMfriendCheckBox.Size = new System.Drawing.Size(50, 22);
            this.COMfriendCheckBox.TabIndex = 80;
            this.COMfriendCheckBox.Text = "CoM";
            this.COMfriendCheckBox.CheckedChanged += new System.EventHandler(this.COMfriendCheckBox_CheckedChanged);
            // 
            // friendguildListView
            // 
            this.friendguildListView.CheckBoxes = true;
            this.friendguildListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader63,
            this.columnHeader64});
            this.friendguildListView.FullRowSelect = true;
            this.friendguildListView.GridLines = true;
            this.friendguildListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.friendguildListView.HideSelection = false;
            this.friendguildListView.LabelWrap = false;
            this.friendguildListView.Location = new System.Drawing.Point(6, 198);
            this.friendguildListView.MultiSelect = false;
            this.friendguildListView.Name = "friendguildListView";
            this.friendguildListView.Size = new System.Drawing.Size(255, 135);
            this.friendguildListView.TabIndex = 77;
            this.friendguildListView.UseCompatibleStateImageBehavior = false;
            this.friendguildListView.View = System.Windows.Forms.View.Details;
            this.friendguildListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.friendGuildListView_Checked);
            // 
            // columnHeader63
            // 
            this.columnHeader63.Text = "X";
            this.columnHeader63.Width = 22;
            // 
            // columnHeader64
            // 
            this.columnHeader64.Text = "Guild";
            this.columnHeader64.Width = 223;
            // 
            // friendGroupBox
            // 
            this.friendGroupBox.Controls.Add(this.friendAddTargetButton);
            this.friendGroupBox.Controls.Add(this.friendRemoveButton);
            this.friendGroupBox.Controls.Add(this.friendAddButton);
            this.friendGroupBox.Location = new System.Drawing.Point(547, 41);
            this.friendGroupBox.Name = "friendGroupBox";
            this.friendGroupBox.Size = new System.Drawing.Size(106, 100);
            this.friendGroupBox.TabIndex = 76;
            this.friendGroupBox.TabStop = false;
            this.friendGroupBox.Text = "User Friend";
            // 
            // friendAddTargetButton
            // 
            this.friendAddTargetButton.Location = new System.Drawing.Point(9, 46);
            this.friendAddTargetButton.Name = "friendAddTargetButton";
            this.friendAddTargetButton.Size = new System.Drawing.Size(90, 20);
            this.friendAddTargetButton.TabIndex = 50;
            this.friendAddTargetButton.Text = "Add Target";
            this.friendAddTargetButton.Click += new System.EventHandler(this.friendAddTargetButton_Click);
            // 
            // friendRemoveButton
            // 
            this.friendRemoveButton.Location = new System.Drawing.Point(9, 72);
            this.friendRemoveButton.Name = "friendRemoveButton";
            this.friendRemoveButton.Size = new System.Drawing.Size(90, 20);
            this.friendRemoveButton.TabIndex = 49;
            this.friendRemoveButton.Text = "Remove";
            this.friendRemoveButton.Click += new System.EventHandler(this.friendRemoveButton_Click);
            // 
            // friendAddButton
            // 
            this.friendAddButton.Location = new System.Drawing.Point(9, 21);
            this.friendAddButton.Name = "friendAddButton";
            this.friendAddButton.Size = new System.Drawing.Size(90, 20);
            this.friendAddButton.TabIndex = 48;
            this.friendAddButton.Text = "Add Manual";
            this.friendAddButton.Click += new System.EventHandler(this.friendAddButton_Click);
            // 
            // friendloggroupBox
            // 
            this.friendloggroupBox.Controls.Add(this.friendLogBox);
            this.friendloggroupBox.Location = new System.Drawing.Point(270, 131);
            this.friendloggroupBox.Name = "friendloggroupBox";
            this.friendloggroupBox.Size = new System.Drawing.Size(271, 204);
            this.friendloggroupBox.TabIndex = 74;
            this.friendloggroupBox.TabStop = false;
            this.friendloggroupBox.Text = "Friend Log";
            // 
            // friendLogBox
            // 
            this.friendLogBox.FormattingEnabled = true;
            this.friendLogBox.Location = new System.Drawing.Point(6, 19);
            this.friendLogBox.Name = "friendLogBox";
            this.friendLogBox.Size = new System.Drawing.Size(259, 134);
            this.friendLogBox.TabIndex = 0;
            // 
            // friendIncludePartyCheckBox
            // 
            this.friendIncludePartyCheckBox.Location = new System.Drawing.Point(270, 101);
            this.friendIncludePartyCheckBox.Name = "friendIncludePartyCheckBox";
            this.friendIncludePartyCheckBox.Size = new System.Drawing.Size(234, 22);
            this.friendIncludePartyCheckBox.TabIndex = 68;
            this.friendIncludePartyCheckBox.Text = "Include party member in Friend List";
            this.friendIncludePartyCheckBox.CheckedChanged += new System.EventHandler(this.friendIncludePartyCheckBox_CheckedChanged);
            // 
            // friendAttackCheckBox
            // 
            this.friendAttackCheckBox.Location = new System.Drawing.Point(270, 76);
            this.friendAttackCheckBox.Name = "friendAttackCheckBox";
            this.friendAttackCheckBox.Size = new System.Drawing.Size(241, 22);
            this.friendAttackCheckBox.TabIndex = 67;
            this.friendAttackCheckBox.Text = "Prevent attacking friends in warmode";
            this.friendAttackCheckBox.CheckedChanged += new System.EventHandler(this.friendAttackCheckBox_CheckedChanged);
            // 
            // friendPartyCheckBox
            // 
            this.friendPartyCheckBox.Location = new System.Drawing.Point(270, 51);
            this.friendPartyCheckBox.Name = "friendPartyCheckBox";
            this.friendPartyCheckBox.Size = new System.Drawing.Size(241, 22);
            this.friendPartyCheckBox.TabIndex = 66;
            this.friendPartyCheckBox.Text = "Autoaccept party from Friends";
            this.friendPartyCheckBox.CheckedChanged += new System.EventHandler(this.friendPartyCheckBox_CheckedChanged);
            // 
            // friendlistView
            // 
            this.friendlistView.CheckBoxes = true;
            this.friendlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader28,
            this.columnHeader29,
            this.columnHeader30});
            this.friendlistView.FullRowSelect = true;
            this.friendlistView.GridLines = true;
            this.friendlistView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.friendlistView.HideSelection = false;
            this.friendlistView.LabelWrap = false;
            this.friendlistView.Location = new System.Drawing.Point(6, 51);
            this.friendlistView.MultiSelect = false;
            this.friendlistView.Name = "friendlistView";
            this.friendlistView.Size = new System.Drawing.Size(255, 135);
            this.friendlistView.TabIndex = 64;
            this.friendlistView.UseCompatibleStateImageBehavior = false;
            this.friendlistView.View = System.Windows.Forms.View.Details;
            this.friendlistView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.friendlistView_PlayerChecked);
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "X";
            this.columnHeader28.Width = 22;
            // 
            // columnHeader29
            // 
            this.columnHeader29.Text = "Name";
            this.columnHeader29.Width = 145;
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "Serial";
            this.columnHeader30.Width = 75;
            // 
            // labelfriend
            // 
            this.labelfriend.AutoSize = true;
            this.labelfriend.Location = new System.Drawing.Point(6, 18);
            this.labelfriend.Name = "labelfriend";
            this.labelfriend.Size = new System.Drawing.Size(58, 13);
            this.labelfriend.TabIndex = 60;
            this.labelfriend.Text = "Friend List:";
            // 
            // friendButtonRemoveList
            // 
            this.friendButtonRemoveList.Location = new System.Drawing.Point(366, 12);
            this.friendButtonRemoveList.Name = "friendButtonRemoveList";
            this.friendButtonRemoveList.Size = new System.Drawing.Size(90, 21);
            this.friendButtonRemoveList.TabIndex = 63;
            this.friendButtonRemoveList.Text = "Remove";
            this.friendButtonRemoveList.Click += new System.EventHandler(this.friendButtonRemoveList_Click);
            // 
            // friendButtonAddList
            // 
            this.friendButtonAddList.Location = new System.Drawing.Point(270, 12);
            this.friendButtonAddList.Name = "friendButtonAddList";
            this.friendButtonAddList.Size = new System.Drawing.Size(90, 21);
            this.friendButtonAddList.TabIndex = 62;
            this.friendButtonAddList.Text = "Add";
            this.friendButtonAddList.Click += new System.EventHandler(this.friendButtonAddList_Click);
            // 
            // friendListSelect
            // 
            this.friendListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.friendListSelect.FormattingEnabled = true;
            this.friendListSelect.Location = new System.Drawing.Point(78, 12);
            this.friendListSelect.Name = "friendListSelect";
            this.friendListSelect.Size = new System.Drawing.Size(183, 21);
            this.friendListSelect.TabIndex = 61;
            this.friendListSelect.SelectedIndexChanged += new System.EventHandler(this.friendListSelect_SelectedIndexChanged);
            // 
            // restock
            // 
            this.restock.Controls.Add(this.restockCloneListB);
            this.restock.Controls.Add(this.restockExecuteButton);
            this.restock.Controls.Add(this.restockStopButton);
            this.restock.Controls.Add(this.groupBox3);
            this.restock.Controls.Add(this.restockdataGridView);
            this.restock.Controls.Add(this.groupBox2);
            this.restock.Controls.Add(this.label13);
            this.restock.Controls.Add(this.label7);
            this.restock.Controls.Add(this.restockAddTargetButton);
            this.restock.Controls.Add(this.restockDragDelay);
            this.restock.Controls.Add(this.restockRemoveListB);
            this.restock.Controls.Add(this.restockAddListB);
            this.restock.Controls.Add(this.restockListSelect);
            this.restock.Location = new System.Drawing.Point(4, 22);
            this.restock.Name = "restock";
            this.restock.Padding = new System.Windows.Forms.Padding(3);
            this.restock.Size = new System.Drawing.Size(659, 341);
            this.restock.TabIndex = 7;
            this.restock.Text = "Restock";
            this.restock.UseVisualStyleBackColor = true;
            // 
            // restockCloneListB
            // 
            this.restockCloneListB.Location = new System.Drawing.Point(416, 11);
            this.restockCloneListB.Name = "restockCloneListB";
            this.restockCloneListB.Size = new System.Drawing.Size(67, 21);
            this.restockCloneListB.TabIndex = 94;
            this.restockCloneListB.Text = "Clone";
            this.restockCloneListB.Click += new System.EventHandler(this.restockCloneListB_Click);
            // 
            // restockExecuteButton
            // 
            this.restockExecuteButton.BackgroundImage = global::Assistant.Properties.Resources.playagent;
            this.restockExecuteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.restockExecuteButton.FlatAppearance.BorderSize = 0;
            this.restockExecuteButton.Location = new System.Drawing.Point(283, 58);
            this.restockExecuteButton.Name = "restockExecuteButton";
            this.restockExecuteButton.Size = new System.Drawing.Size(30, 30);
            this.restockExecuteButton.TabIndex = 93;
            this.restockExecuteButton.UseVisualStyleBackColor = true;
            this.restockExecuteButton.Click += new System.EventHandler(this.restockExecuteButton_Click);
            // 
            // restockStopButton
            // 
            this.restockStopButton.BackgroundImage = global::Assistant.Properties.Resources.stopagent;
            this.restockStopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.restockStopButton.FlatAppearance.BorderSize = 0;
            this.restockStopButton.Location = new System.Drawing.Point(319, 58);
            this.restockStopButton.Name = "restockStopButton";
            this.restockStopButton.Size = new System.Drawing.Size(30, 30);
            this.restockStopButton.TabIndex = 92;
            this.restockStopButton.UseVisualStyleBackColor = true;
            this.restockStopButton.Click += new System.EventHandler(this.restockStopButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label59);
            this.groupBox3.Controls.Add(this.label58);
            this.groupBox3.Controls.Add(this.restockSetSourceButton);
            this.groupBox3.Controls.Add(this.restockSourceLabel);
            this.groupBox3.Controls.Add(this.restockDestinationLabel);
            this.groupBox3.Controls.Add(this.restockSetDestinationButton);
            this.groupBox3.Location = new System.Drawing.Point(9, 42);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(252, 65);
            this.groupBox3.TabIndex = 91;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Restock Bags";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(6, 42);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(63, 13);
            this.label59.TabIndex = 92;
            this.label59.Text = "Destination:";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(6, 18);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(44, 13);
            this.label58.TabIndex = 91;
            this.label58.Text = "Source:";
            // 
            // restockSetSourceButton
            // 
            this.restockSetSourceButton.Location = new System.Drawing.Point(156, 12);
            this.restockSetSourceButton.Name = "restockSetSourceButton";
            this.restockSetSourceButton.Size = new System.Drawing.Size(90, 21);
            this.restockSetSourceButton.TabIndex = 76;
            this.restockSetSourceButton.Text = "Set Bag";
            this.restockSetSourceButton.Click += new System.EventHandler(this.restockSetSourceButton_Click);
            // 
            // restockSourceLabel
            // 
            this.restockSourceLabel.Location = new System.Drawing.Point(71, 18);
            this.restockSourceLabel.Name = "restockSourceLabel";
            this.restockSourceLabel.Size = new System.Drawing.Size(82, 19);
            this.restockSourceLabel.TabIndex = 77;
            this.restockSourceLabel.Text = "0x00000000";
            // 
            // restockDestinationLabel
            // 
            this.restockDestinationLabel.Location = new System.Drawing.Point(71, 42);
            this.restockDestinationLabel.Name = "restockDestinationLabel";
            this.restockDestinationLabel.Size = new System.Drawing.Size(82, 19);
            this.restockDestinationLabel.TabIndex = 80;
            this.restockDestinationLabel.Text = "0x00000000";
            // 
            // restockSetDestinationButton
            // 
            this.restockSetDestinationButton.Location = new System.Drawing.Point(156, 38);
            this.restockSetDestinationButton.Name = "restockSetDestinationButton";
            this.restockSetDestinationButton.Size = new System.Drawing.Size(90, 21);
            this.restockSetDestinationButton.TabIndex = 79;
            this.restockSetDestinationButton.Text = "Set Bag";
            this.restockSetDestinationButton.Click += new System.EventHandler(this.restockSetDestinationButton_Click);
            // 
            // restockdataGridView
            // 
            this.restockdataGridView.AllowDrop = true;
            this.restockdataGridView.AllowUserToResizeRows = false;
            this.restockdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.restockdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn3,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
            this.restockdataGridView.Location = new System.Drawing.Point(9, 113);
            this.restockdataGridView.Name = "restockdataGridView";
            this.restockdataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.restockdataGridView.RowHeadersVisible = false;
            this.restockdataGridView.RowHeadersWidth = 62;
            this.restockdataGridView.Size = new System.Drawing.Size(357, 220);
            this.restockdataGridView.TabIndex = 90;
            this.restockdataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
            this.restockdataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.restockdataGridView_CellEndEdit);
            this.restockdataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.restockdataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.restockdataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.restockdataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.restockdataGridView_DefaultValuesNeeded);
            this.restockdataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.restockdataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.restockdataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.restockdataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.FalseValue = "False";
            this.dataGridViewCheckBoxColumn3.HeaderText = "X";
            this.dataGridViewCheckBoxColumn3.IndeterminateValue = "False";
            this.dataGridViewCheckBoxColumn3.MinimumWidth = 8;
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            this.dataGridViewCheckBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn3.ToolTipText = "Check This for enable item in list";
            this.dataGridViewCheckBoxColumn3.TrueValue = "True";
            this.dataGridViewCheckBoxColumn3.Width = 22;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn9.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn9.ToolTipText = "Here the item name";
            this.dataGridViewTextBoxColumn9.Width = 153;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn10.HeaderText = "Graphics";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn10.ToolTipText = "Here Graphics item ID";
            this.dataGridViewTextBoxColumn10.Width = 54;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn11.HeaderText = "Color";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn11.ToolTipText = "Here item color, use -1 for all color";
            this.dataGridViewTextBoxColumn11.Width = 54;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn12.HeaderText = "Limit";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn12.ToolTipText = "Here amount limit to move";
            this.dataGridViewTextBoxColumn12.Width = 54;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.restockLogBox);
            this.groupBox2.Location = new System.Drawing.Point(373, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 251);
            this.groupBox2.TabIndex = 83;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Restock Log";
            // 
            // restockLogBox
            // 
            this.restockLogBox.FormattingEnabled = true;
            this.restockLogBox.Location = new System.Drawing.Point(7, 18);
            this.restockLogBox.Name = "restockLogBox";
            this.restockLogBox.Size = new System.Drawing.Size(265, 212);
            this.restockLogBox.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(419, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 13);
            this.label13.TabIndex = 82;
            this.label13.Text = "Drag Item Delay (ms)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 66;
            this.label7.Text = "Restock List:";
            // 
            // restockAddTargetButton
            // 
            this.restockAddTargetButton.Location = new System.Drawing.Point(558, 49);
            this.restockAddTargetButton.Name = "restockAddTargetButton";
            this.restockAddTargetButton.Size = new System.Drawing.Size(90, 21);
            this.restockAddTargetButton.TabIndex = 47;
            this.restockAddTargetButton.Text = "Add Item";
            this.restockAddTargetButton.Click += new System.EventHandler(this.restockAddTargetButton_Click);
            // 
            // restockDragDelay
            // 
            this.restockDragDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.restockDragDelay.BackColor = System.Drawing.Color.White;
            this.restockDragDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.restockDragDelay.Location = new System.Drawing.Point(373, 51);
            this.restockDragDelay.Name = "restockDragDelay";
            this.restockDragDelay.Size = new System.Drawing.Size(45, 20);
            this.restockDragDelay.TabIndex = 81;
            this.restockDragDelay.Leave += new System.EventHandler(this.restockDragDelay_Leave);
            // 
            // restockRemoveListB
            // 
            this.restockRemoveListB.Location = new System.Drawing.Point(343, 12);
            this.restockRemoveListB.Name = "restockRemoveListB";
            this.restockRemoveListB.Size = new System.Drawing.Size(67, 21);
            this.restockRemoveListB.TabIndex = 69;
            this.restockRemoveListB.Text = "Remove";
            this.restockRemoveListB.Click += new System.EventHandler(this.restockRemoveListB_Click);
            // 
            // restockAddListB
            // 
            this.restockAddListB.Location = new System.Drawing.Point(270, 12);
            this.restockAddListB.Name = "restockAddListB";
            this.restockAddListB.Size = new System.Drawing.Size(67, 21);
            this.restockAddListB.TabIndex = 68;
            this.restockAddListB.Text = "Add";
            this.restockAddListB.Click += new System.EventHandler(this.restockAddListB_Click);
            // 
            // restockListSelect
            // 
            this.restockListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.restockListSelect.FormattingEnabled = true;
            this.restockListSelect.Location = new System.Drawing.Point(78, 12);
            this.restockListSelect.Name = "restockListSelect";
            this.restockListSelect.Size = new System.Drawing.Size(183, 21);
            this.restockListSelect.TabIndex = 67;
            this.restockListSelect.SelectedIndexChanged += new System.EventHandler(this.restockListSelect_SelectedIndexChanged);
            // 
            // bandageheal
            // 
            this.bandageheal.Controls.Add(this.bandagehealAutostartCheckBox);
            this.bandageheal.Controls.Add(this.groupBox6);
            this.bandageheal.Controls.Add(this.groupBox5);
            this.bandageheal.Controls.Add(this.bandagehealenableCheckBox);
            this.bandageheal.Location = new System.Drawing.Point(4, 22);
            this.bandageheal.Name = "bandageheal";
            this.bandageheal.Padding = new System.Windows.Forms.Padding(3);
            this.bandageheal.Size = new System.Drawing.Size(659, 341);
            this.bandageheal.TabIndex = 8;
            this.bandageheal.Text = "Bandage Heal";
            this.bandageheal.UseVisualStyleBackColor = true;
            // 
            // bandagehealAutostartCheckBox
            // 
            this.bandagehealAutostartCheckBox.Location = new System.Drawing.Point(533, 15);
            this.bandagehealAutostartCheckBox.Name = "bandagehealAutostartCheckBox";
            this.bandagehealAutostartCheckBox.Size = new System.Drawing.Size(118, 22);
            this.bandagehealAutostartCheckBox.TabIndex = 75;
            this.bandagehealAutostartCheckBox.Text = "Autostart OnLogin";
            this.bandagehealAutostartCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealAutostartCheckBox_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label78);
            this.groupBox6.Controls.Add(this.label77);
            this.groupBox6.Controls.Add(this.bandagehealusetextContent);
            this.groupBox6.Controls.Add(this.bandagehealusetextSelfContent);
            this.groupBox6.Controls.Add(this.bandagehealusetext);
            this.groupBox6.Controls.Add(this.bandagehealusetarget);
            this.groupBox6.Controls.Add(this.bandagehealmaxrangeTextBox);
            this.groupBox6.Controls.Add(this.label46);
            this.groupBox6.Controls.Add(this.bandagehealcountdownCheckBox);
            this.groupBox6.Controls.Add(this.bandagehealhiddedCheckBox);
            this.groupBox6.Controls.Add(this.bandagehealmortalCheckBox);
            this.groupBox6.Controls.Add(this.bandagehealpoisonCheckBox);
            this.groupBox6.Controls.Add(this.label33);
            this.groupBox6.Controls.Add(this.bandagehealhpTextBox);
            this.groupBox6.Controls.Add(this.label32);
            this.groupBox6.Controls.Add(this.bandagehealdelayTextBox);
            this.groupBox6.Controls.Add(this.label31);
            this.groupBox6.Controls.Add(this.bandagehealdexformulaCheckBox);
            this.groupBox6.Controls.Add(this.bandagehealcustomcolorTextBox);
            this.groupBox6.Controls.Add(this.label30);
            this.groupBox6.Controls.Add(this.bandagehealcustomIDTextBox);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.bandagehealcustomCheckBox);
            this.groupBox6.Controls.Add(this.bandagehealtargetLabel);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.bandagehealsettargetButton);
            this.groupBox6.Controls.Add(this.bandagehealtargetComboBox);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Location = new System.Drawing.Point(304, 43);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(347, 295);
            this.groupBox6.TabIndex = 74;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Settings";
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(210, 249);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(36, 13);
            this.label78.TabIndex = 96;
            this.label78.Text = "Other:";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(207, 223);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(28, 13);
            this.label77.TabIndex = 95;
            this.label77.Text = "Self:";
            // 
            // bandagehealusetextContent
            // 
            this.bandagehealusetextContent.Enabled = false;
            this.bandagehealusetextContent.Location = new System.Drawing.Point(254, 245);
            this.bandagehealusetextContent.Name = "bandagehealusetextContent";
            this.bandagehealusetextContent.Size = new System.Drawing.Size(82, 20);
            this.bandagehealusetextContent.TabIndex = 94;
            this.bandagehealusetextContent.Text = "[band";
            this.bandagehealusetextContent.Leave += new System.EventHandler(this.bandagehealusetext_Content_Leave);
            // 
            // bandagehealusetextSelfContent
            // 
            this.bandagehealusetextSelfContent.Enabled = false;
            this.bandagehealusetextSelfContent.Location = new System.Drawing.Point(254, 219);
            this.bandagehealusetextSelfContent.Name = "bandagehealusetextSelfContent";
            this.bandagehealusetextSelfContent.Size = new System.Drawing.Size(82, 20);
            this.bandagehealusetextSelfContent.TabIndex = 94;
            this.bandagehealusetextSelfContent.Text = "[bandself";
            this.bandagehealusetextSelfContent.Leave += new System.EventHandler(this.bandagehealusetextSelf_Content_Leave);
            // 
            // bandagehealusetext
            // 
            this.bandagehealusetext.AutoSize = true;
            this.bandagehealusetext.Location = new System.Drawing.Point(184, 193);
            this.bandagehealusetext.Name = "bandagehealusetext";
            this.bandagehealusetext.Size = new System.Drawing.Size(128, 17);
            this.bandagehealusetext.TabIndex = 93;
            this.bandagehealusetext.Text = "Send text for self heal";
            this.bandagehealusetext.UseVisualStyleBackColor = true;
            this.bandagehealusetext.CheckedChanged += new System.EventHandler(this.bandagehealusetext_CheckedChanged);
            // 
            // bandagehealusetarget
            // 
            this.bandagehealusetarget.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealusetarget.Location = new System.Drawing.Point(184, 162);
            this.bandagehealusetarget.Name = "bandagehealusetarget";
            this.bandagehealusetarget.Size = new System.Drawing.Size(155, 22);
            this.bandagehealusetarget.TabIndex = 92;
            this.bandagehealusetarget.Text = "Use Normal Target";
            this.bandagehealusetarget.CheckedChanged += new System.EventHandler(this.bandagehealusetarget_CheckedChanged);
            // 
            // bandagehealmaxrangeTextBox
            // 
            this.bandagehealmaxrangeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bandagehealmaxrangeTextBox.BackColor = System.Drawing.Color.White;
            this.bandagehealmaxrangeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bandagehealmaxrangeTextBox.Location = new System.Drawing.Point(77, 270);
            this.bandagehealmaxrangeTextBox.Name = "bandagehealmaxrangeTextBox";
            this.bandagehealmaxrangeTextBox.Size = new System.Drawing.Size(29, 20);
            this.bandagehealmaxrangeTextBox.TabIndex = 91;
            this.bandagehealmaxrangeTextBox.Leave += new System.EventHandler(this.bandagehealmaxrangeTextBox_Leave);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(7, 273);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(65, 13);
            this.label46.TabIndex = 90;
            this.label46.Text = "Max Range:";
            // 
            // bandagehealcountdownCheckBox
            // 
            this.bandagehealcountdownCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealcountdownCheckBox.Location = new System.Drawing.Point(10, 246);
            this.bandagehealcountdownCheckBox.Name = "bandagehealcountdownCheckBox";
            this.bandagehealcountdownCheckBox.Size = new System.Drawing.Size(155, 22);
            this.bandagehealcountdownCheckBox.TabIndex = 89;
            this.bandagehealcountdownCheckBox.Text = "Show Heal Countdown";
            this.bandagehealcountdownCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealcountdownCheckBox_CheckedChanged);
            // 
            // bandagehealhiddedCheckBox
            // 
            this.bandagehealhiddedCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealhiddedCheckBox.Location = new System.Drawing.Point(10, 218);
            this.bandagehealhiddedCheckBox.Name = "bandagehealhiddedCheckBox";
            this.bandagehealhiddedCheckBox.Size = new System.Drawing.Size(155, 22);
            this.bandagehealhiddedCheckBox.TabIndex = 88;
            this.bandagehealhiddedCheckBox.Text = "Block heal if Hidden";
            this.bandagehealhiddedCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealhiddedCheckBox_CheckedChanged);
            // 
            // bandagehealmortalCheckBox
            // 
            this.bandagehealmortalCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealmortalCheckBox.Location = new System.Drawing.Point(10, 190);
            this.bandagehealmortalCheckBox.Name = "bandagehealmortalCheckBox";
            this.bandagehealmortalCheckBox.Size = new System.Drawing.Size(155, 22);
            this.bandagehealmortalCheckBox.TabIndex = 87;
            this.bandagehealmortalCheckBox.Text = "Block heal if Mortalled";
            this.bandagehealmortalCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealmortalCheckBox_CheckedChanged);
            // 
            // bandagehealpoisonCheckBox
            // 
            this.bandagehealpoisonCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealpoisonCheckBox.Location = new System.Drawing.Point(10, 162);
            this.bandagehealpoisonCheckBox.Name = "bandagehealpoisonCheckBox";
            this.bandagehealpoisonCheckBox.Size = new System.Drawing.Size(155, 22);
            this.bandagehealpoisonCheckBox.TabIndex = 86;
            this.bandagehealpoisonCheckBox.Text = "Block heal if Poisoned";
            this.bandagehealpoisonCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealpoisonCheckBox_CheckedChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(135, 139);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(34, 13);
            this.label33.TabIndex = 85;
            this.label33.Text = "% hits";
            // 
            // bandagehealhpTextBox
            // 
            this.bandagehealhpTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bandagehealhpTextBox.BackColor = System.Drawing.Color.White;
            this.bandagehealhpTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bandagehealhpTextBox.Location = new System.Drawing.Point(76, 136);
            this.bandagehealhpTextBox.Name = "bandagehealhpTextBox";
            this.bandagehealhpTextBox.Size = new System.Drawing.Size(53, 20);
            this.bandagehealhpTextBox.TabIndex = 84;
            this.bandagehealhpTextBox.Leave += new System.EventHandler(this.bandagehealhpTextBox_Leave);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(7, 138);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(64, 13);
            this.label32.TabIndex = 83;
            this.label32.Text = "Start Below:";
            // 
            // bandagehealdelayTextBox
            // 
            this.bandagehealdelayTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bandagehealdelayTextBox.BackColor = System.Drawing.Color.White;
            this.bandagehealdelayTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bandagehealdelayTextBox.Location = new System.Drawing.Point(234, 103);
            this.bandagehealdelayTextBox.Name = "bandagehealdelayTextBox";
            this.bandagehealdelayTextBox.Size = new System.Drawing.Size(53, 20);
            this.bandagehealdelayTextBox.TabIndex = 82;
            this.bandagehealdelayTextBox.Leave += new System.EventHandler(this.bandagehealdelayTextBox_Leave);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(153, 106);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(75, 13);
            this.label31.TabIndex = 81;
            this.label31.Text = "Custom Delay:";
            // 
            // bandagehealdexformulaCheckBox
            // 
            this.bandagehealdexformulaCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealdexformulaCheckBox.Location = new System.Drawing.Point(10, 102);
            this.bandagehealdexformulaCheckBox.Name = "bandagehealdexformulaCheckBox";
            this.bandagehealdexformulaCheckBox.Size = new System.Drawing.Size(155, 22);
            this.bandagehealdexformulaCheckBox.TabIndex = 80;
            this.bandagehealdexformulaCheckBox.Text = "Use DEX formula delay";
            this.bandagehealdexformulaCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealdexformulaCheckBox_CheckedChanged);
            // 
            // bandagehealcustomcolorTextBox
            // 
            this.bandagehealcustomcolorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bandagehealcustomcolorTextBox.BackColor = System.Drawing.Color.White;
            this.bandagehealcustomcolorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bandagehealcustomcolorTextBox.Enabled = false;
            this.bandagehealcustomcolorTextBox.Location = new System.Drawing.Point(286, 76);
            this.bandagehealcustomcolorTextBox.Name = "bandagehealcustomcolorTextBox";
            this.bandagehealcustomcolorTextBox.Size = new System.Drawing.Size(53, 20);
            this.bandagehealcustomcolorTextBox.TabIndex = 79;
            this.bandagehealcustomcolorTextBox.Leave += new System.EventHandler(this.bandagehealcustomcolorTextBox_Leave);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(246, 79);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(34, 13);
            this.label30.TabIndex = 78;
            this.label30.Text = "Color:";
            // 
            // bandagehealcustomIDTextBox
            // 
            this.bandagehealcustomIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bandagehealcustomIDTextBox.BackColor = System.Drawing.Color.White;
            this.bandagehealcustomIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bandagehealcustomIDTextBox.Enabled = false;
            this.bandagehealcustomIDTextBox.Location = new System.Drawing.Point(180, 75);
            this.bandagehealcustomIDTextBox.Name = "bandagehealcustomIDTextBox";
            this.bandagehealcustomIDTextBox.Size = new System.Drawing.Size(53, 20);
            this.bandagehealcustomIDTextBox.TabIndex = 77;
            this.bandagehealcustomIDTextBox.Leave += new System.EventHandler(this.bandagehealcustomIDTextBox_Leave);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(153, 78);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(21, 13);
            this.label19.TabIndex = 76;
            this.label19.Text = "ID:";
            // 
            // bandagehealcustomCheckBox
            // 
            this.bandagehealcustomCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealcustomCheckBox.Location = new System.Drawing.Point(10, 74);
            this.bandagehealcustomCheckBox.Name = "bandagehealcustomCheckBox";
            this.bandagehealcustomCheckBox.Size = new System.Drawing.Size(137, 22);
            this.bandagehealcustomCheckBox.TabIndex = 75;
            this.bandagehealcustomCheckBox.Text = "Use Custom Bandage";
            this.bandagehealcustomCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealcustomCheckBox_CheckedChanged);
            // 
            // bandagehealtargetLabel
            // 
            this.bandagehealtargetLabel.AutoSize = true;
            this.bandagehealtargetLabel.Location = new System.Drawing.Point(73, 49);
            this.bandagehealtargetLabel.Name = "bandagehealtargetLabel";
            this.bandagehealtargetLabel.Size = new System.Drawing.Size(93, 13);
            this.bandagehealtargetLabel.TabIndex = 4;
            this.bandagehealtargetLabel.Text = "Null (0x00000000)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "Target:";
            // 
            // bandagehealsettargetButton
            // 
            this.bandagehealsettargetButton.Location = new System.Drawing.Point(213, 14);
            this.bandagehealsettargetButton.Name = "bandagehealsettargetButton";
            this.bandagehealsettargetButton.Size = new System.Drawing.Size(75, 23);
            this.bandagehealsettargetButton.TabIndex = 2;
            this.bandagehealsettargetButton.Text = "Set Target";
            this.bandagehealsettargetButton.UseVisualStyleBackColor = true;
            this.bandagehealsettargetButton.Click += new System.EventHandler(this.bandagehealsettargetButton_Click);
            // 
            // bandagehealtargetComboBox
            // 
            this.bandagehealtargetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bandagehealtargetComboBox.FormattingEnabled = true;
            this.bandagehealtargetComboBox.Location = new System.Drawing.Point(76, 15);
            this.bandagehealtargetComboBox.Name = "bandagehealtargetComboBox";
            this.bandagehealtargetComboBox.Size = new System.Drawing.Size(121, 21);
            this.bandagehealtargetComboBox.TabIndex = 1;
            this.bandagehealtargetComboBox.SelectedIndexChanged += new System.EventHandler(this.bandagehealtargetComboBox_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Heal Target:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.bandagehealLogBox);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(278, 332);
            this.groupBox5.TabIndex = 54;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Bandage Heal Log";
            // 
            // bandagehealLogBox
            // 
            this.bandagehealLogBox.FormattingEnabled = true;
            this.bandagehealLogBox.Location = new System.Drawing.Point(7, 18);
            this.bandagehealLogBox.Name = "bandagehealLogBox";
            this.bandagehealLogBox.Size = new System.Drawing.Size(265, 264);
            this.bandagehealLogBox.TabIndex = 0;
            // 
            // bandagehealenableCheckBox
            // 
            this.bandagehealenableCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bandagehealenableCheckBox.Location = new System.Drawing.Point(304, 15);
            this.bandagehealenableCheckBox.Name = "bandagehealenableCheckBox";
            this.bandagehealenableCheckBox.Size = new System.Drawing.Size(147, 22);
            this.bandagehealenableCheckBox.TabIndex = 73;
            this.bandagehealenableCheckBox.Text = "Enable Bandage Heal";
            this.bandagehealenableCheckBox.CheckedChanged += new System.EventHandler(this.bandagehealenableCheckBox_CheckedChanged);
            // 
            // toolbarTab
            // 
            this.toolbarTab.Controls.Add(this.toolbarstab);
            this.toolbarTab.Location = new System.Drawing.Point(4, 40);
            this.toolbarTab.Name = "toolbarTab";
            this.toolbarTab.Size = new System.Drawing.Size(666, 366);
            this.toolbarTab.TabIndex = 1;
            this.toolbarTab.Text = "Toolbars";
            // 
            // toolbarstab
            // 
            this.toolbarstab.Controls.Add(this.tabPage2);
            this.toolbarstab.Controls.Add(this.tabPage3);
            this.toolbarstab.Location = new System.Drawing.Point(3, 3);
            this.toolbarstab.Name = "toolbarstab";
            this.toolbarstab.SelectedIndex = 0;
            this.toolbarstab.Size = new System.Drawing.Size(660, 363);
            this.toolbarstab.TabIndex = 62;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.groupBox39);
            this.tabPage2.Controls.Add(this.groupBox25);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox26);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(652, 337);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Counter / Stat Bar";
            // 
            // groupBox39
            // 
            this.groupBox39.Controls.Add(this.toolbar_trackBar);
            this.groupBox39.Controls.Add(this.toolbar_opacity_label);
            this.groupBox39.Location = new System.Drawing.Point(351, 183);
            this.groupBox39.Name = "groupBox39";
            this.groupBox39.Size = new System.Drawing.Size(295, 50);
            this.groupBox39.TabIndex = 65;
            this.groupBox39.TabStop = false;
            this.groupBox39.Text = "Opacity";
            // 
            // toolbar_trackBar
            // 
            this.toolbar_trackBar.AutoSize = false;
            this.toolbar_trackBar.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.toolbar_trackBar.Location = new System.Drawing.Point(54, 24);
            this.toolbar_trackBar.Maximum = 100;
            this.toolbar_trackBar.Minimum = 10;
            this.toolbar_trackBar.Name = "toolbar_trackBar";
            this.toolbar_trackBar.Size = new System.Drawing.Size(235, 16);
            this.toolbar_trackBar.TabIndex = 62;
            this.toolbar_trackBar.TickFrequency = 0;
            this.toolbar_trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolbar_trackBar.Value = 100;
            this.toolbar_trackBar.Scroll += new System.EventHandler(this.toolbar_trackBar_Scroll);
            // 
            // toolbar_opacity_label
            // 
            this.toolbar_opacity_label.Location = new System.Drawing.Point(8, 24);
            this.toolbar_opacity_label.Name = "toolbar_opacity_label";
            this.toolbar_opacity_label.Size = new System.Drawing.Size(40, 16);
            this.toolbar_opacity_label.TabIndex = 63;
            this.toolbar_opacity_label.Text = "100%";
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.lockToolBarCheckBox);
            this.groupBox25.Controls.Add(this.autoopenToolBarCheckBox);
            this.groupBox25.Controls.Add(this.locationToolBarLabel);
            this.groupBox25.Controls.Add(this.closeToolBarButton);
            this.groupBox25.Controls.Add(this.openToolBarButton);
            this.groupBox25.Location = new System.Drawing.Point(6, 6);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(121, 159);
            this.groupBox25.TabIndex = 59;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "General";
            // 
            // lockToolBarCheckBox
            // 
            this.lockToolBarCheckBox.Location = new System.Drawing.Point(6, 71);
            this.lockToolBarCheckBox.Name = "lockToolBarCheckBox";
            this.lockToolBarCheckBox.Size = new System.Drawing.Size(99, 22);
            this.lockToolBarCheckBox.TabIndex = 63;
            this.lockToolBarCheckBox.Text = "Lock ToolBar";
            this.lockToolBarCheckBox.CheckedChanged += new System.EventHandler(this.lockToolBarCheckBox_CheckedChanged);
            // 
            // autoopenToolBarCheckBox
            // 
            this.autoopenToolBarCheckBox.Location = new System.Drawing.Point(6, 93);
            this.autoopenToolBarCheckBox.Name = "autoopenToolBarCheckBox";
            this.autoopenToolBarCheckBox.Size = new System.Drawing.Size(112, 22);
            this.autoopenToolBarCheckBox.TabIndex = 62;
            this.autoopenToolBarCheckBox.Text = "Open On Login";
            this.autoopenToolBarCheckBox.CheckedChanged += new System.EventHandler(this.autoopenToolBarCheckBox_CheckedChanged);
            // 
            // locationToolBarLabel
            // 
            this.locationToolBarLabel.AutoSize = true;
            this.locationToolBarLabel.Location = new System.Drawing.Point(6, 118);
            this.locationToolBarLabel.Name = "locationToolBarLabel";
            this.locationToolBarLabel.Size = new System.Drawing.Size(42, 13);
            this.locationToolBarLabel.TabIndex = 61;
            this.locationToolBarLabel.Text = "X:0 Y:0";
            // 
            // closeToolBarButton
            // 
            this.closeToolBarButton.Location = new System.Drawing.Point(6, 45);
            this.closeToolBarButton.Name = "closeToolBarButton";
            this.closeToolBarButton.Size = new System.Drawing.Size(90, 21);
            this.closeToolBarButton.TabIndex = 59;
            this.closeToolBarButton.Text = "Close";
            this.closeToolBarButton.Click += new System.EventHandler(this.closeToolBarButton_Click);
            // 
            // openToolBarButton
            // 
            this.openToolBarButton.Location = new System.Drawing.Point(6, 19);
            this.openToolBarButton.Name = "openToolBarButton";
            this.openToolBarButton.Size = new System.Drawing.Size(90, 21);
            this.openToolBarButton.TabIndex = 58;
            this.openToolBarButton.Text = "Open";
            this.openToolBarButton.Click += new System.EventHandler(this.openToolBarButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.showtitheToolBarCheckBox);
            this.groupBox4.Controls.Add(this.toolbarremoveslotButton);
            this.groupBox4.Controls.Add(this.toolbaraddslotButton);
            this.groupBox4.Controls.Add(this.toolbarslot_label);
            this.groupBox4.Controls.Add(this.label43);
            this.groupBox4.Controls.Add(this.toolboxsizeComboBox);
            this.groupBox4.Controls.Add(this.label41);
            this.groupBox4.Controls.Add(this.showfollowerToolBarCheckBox);
            this.groupBox4.Controls.Add(this.showweightToolBarCheckBox);
            this.groupBox4.Controls.Add(this.showmanaToolBarCheckBox);
            this.groupBox4.Controls.Add(this.showstaminaToolBarCheckBox);
            this.groupBox4.Controls.Add(this.showhitsToolBarCheckBox);
            this.groupBox4.Controls.Add(this.toolboxstyleComboBox);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(351, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(295, 168);
            this.groupBox4.TabIndex = 61;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Layout";
            // 
            // showtitheToolBarCheckBox
            // 
            this.showtitheToolBarCheckBox.AutoSize = true;
            this.showtitheToolBarCheckBox.Location = new System.Drawing.Point(181, 145);
            this.showtitheToolBarCheckBox.Name = "showtitheToolBarCheckBox";
            this.showtitheToolBarCheckBox.Size = new System.Drawing.Size(80, 17);
            this.showtitheToolBarCheckBox.TabIndex = 80;
            this.showtitheToolBarCheckBox.Text = "Show Tithe";
            this.showtitheToolBarCheckBox.UseVisualStyleBackColor = true;
            this.showtitheToolBarCheckBox.CheckedChanged += new System.EventHandler(this.showtitheToolBarCheckBox_CheckedChanged);
            // 
            // toolbarremoveslotButton
            // 
            this.toolbarremoveslotButton.Location = new System.Drawing.Point(86, 85);
            this.toolbarremoveslotButton.Name = "toolbarremoveslotButton";
            this.toolbarremoveslotButton.Size = new System.Drawing.Size(20, 20);
            this.toolbarremoveslotButton.TabIndex = 79;
            this.toolbarremoveslotButton.Text = "-";
            this.toolbarremoveslotButton.Click += new System.EventHandler(this.toolbarremoveslotButton_Click);
            // 
            // toolbaraddslotButton
            // 
            this.toolbaraddslotButton.Location = new System.Drawing.Point(61, 85);
            this.toolbaraddslotButton.Name = "toolbaraddslotButton";
            this.toolbaraddslotButton.Size = new System.Drawing.Size(20, 20);
            this.toolbaraddslotButton.TabIndex = 71;
            this.toolbaraddslotButton.Text = "+";
            this.toolbaraddslotButton.Click += new System.EventHandler(this.toolbaraddslotButton_Click);
            // 
            // toolbarslot_label
            // 
            this.toolbarslot_label.AutoSize = true;
            this.toolbarslot_label.Location = new System.Drawing.Point(42, 89);
            this.toolbarslot_label.Name = "toolbarslot_label";
            this.toolbarslot_label.Size = new System.Drawing.Size(13, 13);
            this.toolbarslot_label.TabIndex = 78;
            this.toolbarslot_label.Text = "0";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(6, 89);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(33, 13);
            this.label43.TabIndex = 71;
            this.label43.Text = "Slots:";
            // 
            // toolboxsizeComboBox
            // 
            this.toolboxsizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolboxsizeComboBox.FormattingEnabled = true;
            this.toolboxsizeComboBox.Location = new System.Drawing.Point(45, 52);
            this.toolboxsizeComboBox.Name = "toolboxsizeComboBox";
            this.toolboxsizeComboBox.Size = new System.Drawing.Size(112, 21);
            this.toolboxsizeComboBox.TabIndex = 77;
            this.toolboxsizeComboBox.SelectedIndexChanged += new System.EventHandler(this.toolboxsizeComboBox_SelectedIndexChanged);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(6, 58);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(30, 13);
            this.label41.TabIndex = 76;
            this.label41.Text = "Size:";
            // 
            // showfollowerToolBarCheckBox
            // 
            this.showfollowerToolBarCheckBox.Location = new System.Drawing.Point(181, 121);
            this.showfollowerToolBarCheckBox.Name = "showfollowerToolBarCheckBox";
            this.showfollowerToolBarCheckBox.Size = new System.Drawing.Size(99, 22);
            this.showfollowerToolBarCheckBox.TabIndex = 75;
            this.showfollowerToolBarCheckBox.Text = "Show Follower";
            this.showfollowerToolBarCheckBox.CheckedChanged += new System.EventHandler(this.showfollowerToolBarCheckBox_CheckedChanged);
            // 
            // showweightToolBarCheckBox
            // 
            this.showweightToolBarCheckBox.Location = new System.Drawing.Point(181, 95);
            this.showweightToolBarCheckBox.Name = "showweightToolBarCheckBox";
            this.showweightToolBarCheckBox.Size = new System.Drawing.Size(99, 22);
            this.showweightToolBarCheckBox.TabIndex = 74;
            this.showweightToolBarCheckBox.Text = "Show Weight";
            this.showweightToolBarCheckBox.CheckedChanged += new System.EventHandler(this.showweightToolBarCheckBox_CheckedChanged);
            // 
            // showmanaToolBarCheckBox
            // 
            this.showmanaToolBarCheckBox.Location = new System.Drawing.Point(181, 69);
            this.showmanaToolBarCheckBox.Name = "showmanaToolBarCheckBox";
            this.showmanaToolBarCheckBox.Size = new System.Drawing.Size(99, 22);
            this.showmanaToolBarCheckBox.TabIndex = 73;
            this.showmanaToolBarCheckBox.Text = "Show Mana";
            this.showmanaToolBarCheckBox.CheckedChanged += new System.EventHandler(this.showmanaToolBarCheckBox_CheckedChanged);
            // 
            // showstaminaToolBarCheckBox
            // 
            this.showstaminaToolBarCheckBox.Location = new System.Drawing.Point(181, 43);
            this.showstaminaToolBarCheckBox.Name = "showstaminaToolBarCheckBox";
            this.showstaminaToolBarCheckBox.Size = new System.Drawing.Size(99, 22);
            this.showstaminaToolBarCheckBox.TabIndex = 72;
            this.showstaminaToolBarCheckBox.Text = "Show Stamina";
            this.showstaminaToolBarCheckBox.CheckedChanged += new System.EventHandler(this.showstaminaToolBarCheckBox_CheckedChanged);
            // 
            // showhitsToolBarCheckBox
            // 
            this.showhitsToolBarCheckBox.Location = new System.Drawing.Point(181, 17);
            this.showhitsToolBarCheckBox.Name = "showhitsToolBarCheckBox";
            this.showhitsToolBarCheckBox.Size = new System.Drawing.Size(99, 22);
            this.showhitsToolBarCheckBox.TabIndex = 64;
            this.showhitsToolBarCheckBox.Text = "Show Hits";
            this.showhitsToolBarCheckBox.CheckedChanged += new System.EventHandler(this.showhitsToolBarCheckBox_CheckedChanged);
            // 
            // toolboxstyleComboBox
            // 
            this.toolboxstyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolboxstyleComboBox.FormattingEnabled = true;
            this.toolboxstyleComboBox.Location = new System.Drawing.Point(45, 19);
            this.toolboxstyleComboBox.Name = "toolboxstyleComboBox";
            this.toolboxstyleComboBox.Size = new System.Drawing.Size(112, 21);
            this.toolboxstyleComboBox.TabIndex = 71;
            this.toolboxstyleComboBox.SelectedIndexChanged += new System.EventHandler(this.toolboxstyleComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Style:";
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.label38);
            this.groupBox26.Controls.Add(this.toolboxcountNameTextBox);
            this.groupBox26.Controls.Add(this.label37);
            this.groupBox26.Controls.Add(this.toolboxcountClearButton);
            this.groupBox26.Controls.Add(this.toolboxcountTargetButton);
            this.groupBox26.Controls.Add(this.toolboxcountWarningTextBox);
            this.groupBox26.Controls.Add(this.label36);
            this.groupBox26.Controls.Add(this.toolboxcountHueWarningCheckBox);
            this.groupBox26.Controls.Add(this.toolboxcountHueTextBox);
            this.groupBox26.Controls.Add(this.label35);
            this.groupBox26.Controls.Add(this.toolboxcountGraphTextBox);
            this.groupBox26.Controls.Add(this.label18);
            this.groupBox26.Controls.Add(this.toolboxcountComboBox);
            this.groupBox26.Location = new System.Drawing.Point(130, 6);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(214, 203);
            this.groupBox26.TabIndex = 60;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Item Count";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(131, 102);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(44, 13);
            this.label38.TabIndex = 70;
            this.label38.Text = "-1 for all";
            // 
            // toolboxcountNameTextBox
            // 
            this.toolboxcountNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolboxcountNameTextBox.BackColor = System.Drawing.Color.White;
            this.toolboxcountNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolboxcountNameTextBox.Location = new System.Drawing.Point(64, 49);
            this.toolboxcountNameTextBox.Name = "toolboxcountNameTextBox";
            this.toolboxcountNameTextBox.Size = new System.Drawing.Size(144, 20);
            this.toolboxcountNameTextBox.TabIndex = 69;
            this.toolboxcountNameTextBox.TextChanged += new System.EventHandler(this.toolboxcountNameTextBox_TextChanged);
            this.toolboxcountNameTextBox.Leave += new System.EventHandler(this.toolboxcountNameTextBox_Leave);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 52);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(38, 13);
            this.label37.TabIndex = 68;
            this.label37.Text = "Name:";
            // 
            // toolboxcountClearButton
            // 
            this.toolboxcountClearButton.Location = new System.Drawing.Point(131, 177);
            this.toolboxcountClearButton.Name = "toolboxcountClearButton";
            this.toolboxcountClearButton.Size = new System.Drawing.Size(77, 21);
            this.toolboxcountClearButton.TabIndex = 67;
            this.toolboxcountClearButton.Text = "Clear Slot";
            this.toolboxcountClearButton.Click += new System.EventHandler(this.toolboxcountClearButton_Click);
            // 
            // toolboxcountTargetButton
            // 
            this.toolboxcountTargetButton.Location = new System.Drawing.Point(9, 177);
            this.toolboxcountTargetButton.Name = "toolboxcountTargetButton";
            this.toolboxcountTargetButton.Size = new System.Drawing.Size(77, 21);
            this.toolboxcountTargetButton.TabIndex = 64;
            this.toolboxcountTargetButton.Text = "Get Data";
            this.toolboxcountTargetButton.Click += new System.EventHandler(this.toolboxcountTargetButton_Click);
            // 
            // toolboxcountWarningTextBox
            // 
            this.toolboxcountWarningTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolboxcountWarningTextBox.BackColor = System.Drawing.Color.White;
            this.toolboxcountWarningTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolboxcountWarningTextBox.Location = new System.Drawing.Point(64, 148);
            this.toolboxcountWarningTextBox.Name = "toolboxcountWarningTextBox";
            this.toolboxcountWarningTextBox.Size = new System.Drawing.Size(61, 20);
            this.toolboxcountWarningTextBox.TabIndex = 66;
            this.toolboxcountWarningTextBox.TextChanged += new System.EventHandler(this.toolboxcountWarningTextBox_TextChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 151);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(50, 13);
            this.label36.TabIndex = 65;
            this.label36.Text = "Warning:";
            // 
            // toolboxcountHueWarningCheckBox
            // 
            this.toolboxcountHueWarningCheckBox.Location = new System.Drawing.Point(9, 125);
            this.toolboxcountHueWarningCheckBox.Name = "toolboxcountHueWarningCheckBox";
            this.toolboxcountHueWarningCheckBox.Size = new System.Drawing.Size(99, 22);
            this.toolboxcountHueWarningCheckBox.TabIndex = 64;
            this.toolboxcountHueWarningCheckBox.Text = "Show Warning";
            this.toolboxcountHueWarningCheckBox.CheckedChanged += new System.EventHandler(this.toolboxcountHueWarningCheckBox_CheckedChanged);
            // 
            // toolboxcountHueTextBox
            // 
            this.toolboxcountHueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolboxcountHueTextBox.BackColor = System.Drawing.Color.White;
            this.toolboxcountHueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolboxcountHueTextBox.Location = new System.Drawing.Point(64, 99);
            this.toolboxcountHueTextBox.Name = "toolboxcountHueTextBox";
            this.toolboxcountHueTextBox.Size = new System.Drawing.Size(61, 20);
            this.toolboxcountHueTextBox.TabIndex = 4;
            this.toolboxcountHueTextBox.TextChanged += new System.EventHandler(this.toolboxcountHueTextBox_TextChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 102);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(34, 13);
            this.label35.TabIndex = 3;
            this.label35.Text = "Color:";
            // 
            // toolboxcountGraphTextBox
            // 
            this.toolboxcountGraphTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolboxcountGraphTextBox.BackColor = System.Drawing.Color.White;
            this.toolboxcountGraphTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolboxcountGraphTextBox.Location = new System.Drawing.Point(64, 73);
            this.toolboxcountGraphTextBox.Name = "toolboxcountGraphTextBox";
            this.toolboxcountGraphTextBox.Size = new System.Drawing.Size(61, 20);
            this.toolboxcountGraphTextBox.TabIndex = 2;
            this.toolboxcountGraphTextBox.TextChanged += new System.EventHandler(this.toolboxcountGraphTextBox_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 76);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "Graphics:";
            // 
            // toolboxcountComboBox
            // 
            this.toolboxcountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolboxcountComboBox.FormattingEnabled = true;
            this.toolboxcountComboBox.Location = new System.Drawing.Point(6, 19);
            this.toolboxcountComboBox.Name = "toolboxcountComboBox";
            this.toolboxcountComboBox.Size = new System.Drawing.Size(202, 21);
            this.toolboxcountComboBox.TabIndex = 0;
            this.toolboxcountComboBox.SelectedIndexChanged += new System.EventHandler(this.toolboxcountComboBox_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.groupBox38);
            this.tabPage3.Controls.Add(this.groupBox37);
            this.tabPage3.Controls.Add(this.groupBox36);
            this.tabPage3.Controls.Add(this.groupBox35);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(652, 337);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Spell Grid";
            // 
            // groupBox38
            // 
            this.groupBox38.Controls.Add(this.spellgrid_trackBar);
            this.groupBox38.Controls.Add(this.spellgrid_opacity_label);
            this.groupBox38.Location = new System.Drawing.Point(427, 103);
            this.groupBox38.Name = "groupBox38";
            this.groupBox38.Size = new System.Drawing.Size(219, 48);
            this.groupBox38.TabIndex = 66;
            this.groupBox38.TabStop = false;
            this.groupBox38.Text = "Opacity";
            // 
            // spellgrid_trackBar
            // 
            this.spellgrid_trackBar.AutoSize = false;
            this.spellgrid_trackBar.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.spellgrid_trackBar.Location = new System.Drawing.Point(43, 20);
            this.spellgrid_trackBar.Maximum = 100;
            this.spellgrid_trackBar.Minimum = 10;
            this.spellgrid_trackBar.Name = "spellgrid_trackBar";
            this.spellgrid_trackBar.Size = new System.Drawing.Size(170, 16);
            this.spellgrid_trackBar.TabIndex = 62;
            this.spellgrid_trackBar.TickFrequency = 0;
            this.spellgrid_trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.spellgrid_trackBar.Value = 100;
            this.spellgrid_trackBar.Scroll += new System.EventHandler(this.spellgrid_trackBar_Scroll);
            // 
            // spellgrid_opacity_label
            // 
            this.spellgrid_opacity_label.Location = new System.Drawing.Point(6, 20);
            this.spellgrid_opacity_label.Name = "spellgrid_opacity_label";
            this.spellgrid_opacity_label.Size = new System.Drawing.Size(36, 16);
            this.spellgrid_opacity_label.TabIndex = 63;
            this.spellgrid_opacity_label.Text = "100%";
            // 
            // groupBox37
            // 
            this.groupBox37.Controls.Add(this.gridhslotremove_button);
            this.groupBox37.Controls.Add(this.gridhslotadd_button);
            this.groupBox37.Controls.Add(this.gridhslot_textbox);
            this.groupBox37.Controls.Add(this.label53);
            this.groupBox37.Controls.Add(this.gridvslotremove_button);
            this.groupBox37.Controls.Add(this.gridvslotadd_button);
            this.groupBox37.Controls.Add(this.gridvslot_textbox);
            this.groupBox37.Controls.Add(this.label49);
            this.groupBox37.Location = new System.Drawing.Point(427, 6);
            this.groupBox37.Name = "groupBox37";
            this.groupBox37.Size = new System.Drawing.Size(129, 91);
            this.groupBox37.TabIndex = 65;
            this.groupBox37.TabStop = false;
            this.groupBox37.Text = "Layout";
            // 
            // gridhslotremove_button
            // 
            this.gridhslotremove_button.Location = new System.Drawing.Point(90, 47);
            this.gridhslotremove_button.Name = "gridhslotremove_button";
            this.gridhslotremove_button.Size = new System.Drawing.Size(20, 20);
            this.gridhslotremove_button.TabIndex = 83;
            this.gridhslotremove_button.Text = "-";
            this.gridhslotremove_button.Click += new System.EventHandler(this.gridhslotremove_button_Click);
            // 
            // gridhslotadd_button
            // 
            this.gridhslotadd_button.Location = new System.Drawing.Point(65, 47);
            this.gridhslotadd_button.Name = "gridhslotadd_button";
            this.gridhslotadd_button.Size = new System.Drawing.Size(20, 20);
            this.gridhslotadd_button.TabIndex = 80;
            this.gridhslotadd_button.Text = "+";
            this.gridhslotadd_button.Click += new System.EventHandler(this.gridhslotadd_button_Click);
            // 
            // gridhslot_textbox
            // 
            this.gridhslot_textbox.AutoSize = true;
            this.gridhslot_textbox.Location = new System.Drawing.Point(48, 51);
            this.gridhslot_textbox.Name = "gridhslot_textbox";
            this.gridhslot_textbox.Size = new System.Drawing.Size(13, 13);
            this.gridhslot_textbox.TabIndex = 82;
            this.gridhslot_textbox.Text = "0";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(6, 51);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(44, 13);
            this.label53.TabIndex = 81;
            this.label53.Text = "Slots H:";
            // 
            // gridvslotremove_button
            // 
            this.gridvslotremove_button.Location = new System.Drawing.Point(90, 21);
            this.gridvslotremove_button.Name = "gridvslotremove_button";
            this.gridvslotremove_button.Size = new System.Drawing.Size(20, 20);
            this.gridvslotremove_button.TabIndex = 79;
            this.gridvslotremove_button.Text = "-";
            this.gridvslotremove_button.Click += new System.EventHandler(this.gridvslotremove_button_Click);
            // 
            // gridvslotadd_button
            // 
            this.gridvslotadd_button.Location = new System.Drawing.Point(65, 21);
            this.gridvslotadd_button.Name = "gridvslotadd_button";
            this.gridvslotadd_button.Size = new System.Drawing.Size(20, 20);
            this.gridvslotadd_button.TabIndex = 71;
            this.gridvslotadd_button.Text = "+";
            this.gridvslotadd_button.Click += new System.EventHandler(this.gridvslotadd_button_Click);
            // 
            // gridvslot_textbox
            // 
            this.gridvslot_textbox.AutoSize = true;
            this.gridvslot_textbox.Location = new System.Drawing.Point(48, 25);
            this.gridvslot_textbox.Name = "gridvslot_textbox";
            this.gridvslot_textbox.Size = new System.Drawing.Size(13, 13);
            this.gridvslot_textbox.TabIndex = 78;
            this.gridvslot_textbox.Text = "0";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(6, 25);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(43, 13);
            this.label49.TabIndex = 71;
            this.label49.Text = "Slots V:";
            // 
            // groupBox36
            // 
            this.groupBox36.Controls.Add(this.gridscript_ComboBox);
            this.groupBox36.Controls.Add(this.label65);
            this.groupBox36.Controls.Add(this.gridborder_ComboBox);
            this.groupBox36.Controls.Add(this.label44);
            this.groupBox36.Controls.Add(this.gridspell_ComboBox);
            this.groupBox36.Controls.Add(this.label52);
            this.groupBox36.Controls.Add(this.gridgroup_ComboBox);
            this.groupBox36.Controls.Add(this.label51);
            this.groupBox36.Controls.Add(this.label45);
            this.groupBox36.Controls.Add(this.gridslot_ComboBox);
            this.groupBox36.Location = new System.Drawing.Point(133, 6);
            this.groupBox36.Name = "groupBox36";
            this.groupBox36.Size = new System.Drawing.Size(288, 177);
            this.groupBox36.TabIndex = 64;
            this.groupBox36.TabStop = false;
            this.groupBox36.Text = "Grid Item";
            // 
            // gridscript_ComboBox
            // 
            this.gridscript_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridscript_ComboBox.FormattingEnabled = true;
            this.gridscript_ComboBox.Location = new System.Drawing.Point(75, 136);
            this.gridscript_ComboBox.Name = "gridscript_ComboBox";
            this.gridscript_ComboBox.Size = new System.Drawing.Size(202, 21);
            this.gridscript_ComboBox.TabIndex = 78;
            this.gridscript_ComboBox.SelectedIndexChanged += new System.EventHandler(this.gridscript_ComboBox_SelectedIndexChanged);
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(6, 141);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(34, 13);
            this.label65.TabIndex = 77;
            this.label65.Text = "Script";
            // 
            // gridborder_ComboBox
            // 
            this.gridborder_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridborder_ComboBox.FormattingEnabled = true;
            this.gridborder_ComboBox.Location = new System.Drawing.Point(75, 107);
            this.gridborder_ComboBox.Name = "gridborder_ComboBox";
            this.gridborder_ComboBox.Size = new System.Drawing.Size(202, 21);
            this.gridborder_ComboBox.TabIndex = 76;
            this.gridborder_ComboBox.SelectedIndexChanged += new System.EventHandler(this.gridborder_ComboBox_SelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(6, 112);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(44, 13);
            this.label44.TabIndex = 75;
            this.label44.Text = "Border: ";
            // 
            // gridspell_ComboBox
            // 
            this.gridspell_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridspell_ComboBox.FormattingEnabled = true;
            this.gridspell_ComboBox.Location = new System.Drawing.Point(75, 77);
            this.gridspell_ComboBox.Name = "gridspell_ComboBox";
            this.gridspell_ComboBox.Size = new System.Drawing.Size(202, 21);
            this.gridspell_ComboBox.TabIndex = 74;
            this.gridspell_ComboBox.SelectedIndexChanged += new System.EventHandler(this.gridspell_ComboBox_SelectedIndexChanged);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(6, 82);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(68, 13);
            this.label52.TabIndex = 73;
            this.label52.Text = "Abilitie/Spell:";
            // 
            // gridgroup_ComboBox
            // 
            this.gridgroup_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridgroup_ComboBox.FormattingEnabled = true;
            this.gridgroup_ComboBox.Location = new System.Drawing.Point(75, 47);
            this.gridgroup_ComboBox.Name = "gridgroup_ComboBox";
            this.gridgroup_ComboBox.Size = new System.Drawing.Size(202, 21);
            this.gridgroup_ComboBox.TabIndex = 72;
            this.gridgroup_ComboBox.SelectedIndexChanged += new System.EventHandler(this.gridgroup_ComboBox_SelectedIndexChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(6, 23);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(28, 13);
            this.label51.TabIndex = 71;
            this.label51.Text = "Slot:";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(6, 52);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(39, 13);
            this.label45.TabIndex = 68;
            this.label45.Text = "Group:";
            // 
            // gridslot_ComboBox
            // 
            this.gridslot_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridslot_ComboBox.FormattingEnabled = true;
            this.gridslot_ComboBox.Location = new System.Drawing.Point(75, 17);
            this.gridslot_ComboBox.Name = "gridslot_ComboBox";
            this.gridslot_ComboBox.Size = new System.Drawing.Size(202, 21);
            this.gridslot_ComboBox.TabIndex = 0;
            this.gridslot_ComboBox.SelectedIndexChanged += new System.EventHandler(this.gridslot_ComboBox_SelectedIndexChanged);
            // 
            // groupBox35
            // 
            this.groupBox35.Controls.Add(this.gridlock_CheckBox);
            this.groupBox35.Controls.Add(this.gridopenlogin_CheckBox);
            this.groupBox35.Controls.Add(this.gridlocation_label);
            this.groupBox35.Controls.Add(this.gridclose_button);
            this.groupBox35.Controls.Add(this.gridopen_button);
            this.groupBox35.Location = new System.Drawing.Point(6, 6);
            this.groupBox35.Name = "groupBox35";
            this.groupBox35.Size = new System.Drawing.Size(121, 145);
            this.groupBox35.TabIndex = 63;
            this.groupBox35.TabStop = false;
            this.groupBox35.Text = "General";
            // 
            // gridlock_CheckBox
            // 
            this.gridlock_CheckBox.Location = new System.Drawing.Point(6, 71);
            this.gridlock_CheckBox.Name = "gridlock_CheckBox";
            this.gridlock_CheckBox.Size = new System.Drawing.Size(99, 22);
            this.gridlock_CheckBox.TabIndex = 63;
            this.gridlock_CheckBox.Text = "Lock Grid";
            this.gridlock_CheckBox.CheckedChanged += new System.EventHandler(this.gridlock_CheckBox_CheckedChanged);
            // 
            // gridopenlogin_CheckBox
            // 
            this.gridopenlogin_CheckBox.Location = new System.Drawing.Point(6, 93);
            this.gridopenlogin_CheckBox.Name = "gridopenlogin_CheckBox";
            this.gridopenlogin_CheckBox.Size = new System.Drawing.Size(112, 22);
            this.gridopenlogin_CheckBox.TabIndex = 62;
            this.gridopenlogin_CheckBox.Text = "Open On Login";
            this.gridopenlogin_CheckBox.CheckedChanged += new System.EventHandler(this.gridopenlogin_CheckBox_CheckedChanged);
            // 
            // gridlocation_label
            // 
            this.gridlocation_label.AutoSize = true;
            this.gridlocation_label.Location = new System.Drawing.Point(6, 118);
            this.gridlocation_label.Name = "gridlocation_label";
            this.gridlocation_label.Size = new System.Drawing.Size(42, 13);
            this.gridlocation_label.TabIndex = 61;
            this.gridlocation_label.Text = "X:0 Y:0";
            // 
            // gridclose_button
            // 
            this.gridclose_button.Location = new System.Drawing.Point(6, 45);
            this.gridclose_button.Name = "gridclose_button";
            this.gridclose_button.Size = new System.Drawing.Size(90, 21);
            this.gridclose_button.TabIndex = 59;
            this.gridclose_button.Text = "Close";
            this.gridclose_button.Click += new System.EventHandler(this.gridclose_button_Click);
            // 
            // gridopen_button
            // 
            this.gridopen_button.Location = new System.Drawing.Point(6, 19);
            this.gridopen_button.Name = "gridopen_button";
            this.gridopen_button.Size = new System.Drawing.Size(90, 21);
            this.gridopen_button.TabIndex = 58;
            this.gridopen_button.Text = "Open";
            this.gridopen_button.Click += new System.EventHandler(this.gridopen_button_Click);
            // 
            // emptyTab
            // 
            this.emptyTab.Controls.Add(this.targetTestButton);
            this.emptyTab.Controls.Add(this.targetsaveButton);
            this.emptyTab.Controls.Add(this.groupBox57);
            this.emptyTab.Controls.Add(this.groupBox56);
            this.emptyTab.Controls.Add(this.groupBox55);
            this.emptyTab.Controls.Add(this.groupBox48);
            this.emptyTab.Controls.Add(this.groupBox46);
            this.emptyTab.Controls.Add(this.groupBox45);
            this.emptyTab.Controls.Add(this.groupBox44);
            this.emptyTab.Controls.Add(this.groupBox43);
            this.emptyTab.Location = new System.Drawing.Point(4, 40);
            this.emptyTab.Name = "emptyTab";
            this.emptyTab.Size = new System.Drawing.Size(666, 366);
            this.emptyTab.TabIndex = 3;
            this.emptyTab.Text = "Targetting";
            // 
            // targetTestButton
            // 
            this.targetTestButton.Location = new System.Drawing.Point(375, 274);
            this.targetTestButton.Name = "targetTestButton";
            this.targetTestButton.Size = new System.Drawing.Size(100, 23);
            this.targetTestButton.TabIndex = 57;
            this.targetTestButton.Text = "Test Target";
            this.targetTestButton.UseVisualStyleBackColor = true;
            this.targetTestButton.Click += new System.EventHandler(this.targetTestButton_Click);
            // 
            // targetsaveButton
            // 
            this.targetsaveButton.Location = new System.Drawing.Point(573, 333);
            this.targetsaveButton.Name = "targetsaveButton";
            this.targetsaveButton.Size = new System.Drawing.Size(88, 23);
            this.targetsaveButton.TabIndex = 5;
            this.targetsaveButton.Text = "Save Change";
            this.targetsaveButton.UseVisualStyleBackColor = true;
            this.targetsaveButton.Click += new System.EventHandler(this.targetsaveButton_Click);
            // 
            // groupBox57
            // 
            this.groupBox57.Controls.Add(this.targetYellowCheckBox);
            this.groupBox57.Controls.Add(this.targetRedCheckBox);
            this.groupBox57.Controls.Add(this.targetOrangeCheckBox);
            this.groupBox57.Controls.Add(this.targetCriminalCheckBox);
            this.groupBox57.Controls.Add(this.targetGreyCheckBox);
            this.groupBox57.Controls.Add(this.targetGreenCheckBox);
            this.groupBox57.Controls.Add(this.targetBlueCheckBox);
            this.groupBox57.Location = new System.Drawing.Point(375, 110);
            this.groupBox57.Name = "groupBox57";
            this.groupBox57.Size = new System.Drawing.Size(100, 157);
            this.groupBox57.TabIndex = 56;
            this.groupBox57.TabStop = false;
            this.groupBox57.Text = "Notoriety Color";
            // 
            // targetYellowCheckBox
            // 
            this.targetYellowCheckBox.AutoSize = true;
            this.targetYellowCheckBox.ForeColor = System.Drawing.Color.DarkSalmon;
            this.targetYellowCheckBox.Location = new System.Drawing.Point(6, 134);
            this.targetYellowCheckBox.Name = "targetYellowCheckBox";
            this.targetYellowCheckBox.Size = new System.Drawing.Size(57, 17);
            this.targetYellowCheckBox.TabIndex = 77;
            this.targetYellowCheckBox.Text = "Yellow";
            this.targetYellowCheckBox.UseVisualStyleBackColor = true;
            // 
            // targetRedCheckBox
            // 
            this.targetRedCheckBox.AutoSize = true;
            this.targetRedCheckBox.ForeColor = System.Drawing.Color.Red;
            this.targetRedCheckBox.Location = new System.Drawing.Point(6, 114);
            this.targetRedCheckBox.Name = "targetRedCheckBox";
            this.targetRedCheckBox.Size = new System.Drawing.Size(46, 17);
            this.targetRedCheckBox.TabIndex = 76;
            this.targetRedCheckBox.Text = "Red";
            this.targetRedCheckBox.UseVisualStyleBackColor = true;
            // 
            // targetOrangeCheckBox
            // 
            this.targetOrangeCheckBox.AutoSize = true;
            this.targetOrangeCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.targetOrangeCheckBox.Location = new System.Drawing.Point(6, 95);
            this.targetOrangeCheckBox.Name = "targetOrangeCheckBox";
            this.targetOrangeCheckBox.Size = new System.Drawing.Size(61, 17);
            this.targetOrangeCheckBox.TabIndex = 75;
            this.targetOrangeCheckBox.Text = "Orange";
            this.targetOrangeCheckBox.UseVisualStyleBackColor = true;
            // 
            // targetCriminalCheckBox
            // 
            this.targetCriminalCheckBox.AutoSize = true;
            this.targetCriminalCheckBox.ForeColor = System.Drawing.Color.DimGray;
            this.targetCriminalCheckBox.Location = new System.Drawing.Point(6, 76);
            this.targetCriminalCheckBox.Name = "targetCriminalCheckBox";
            this.targetCriminalCheckBox.Size = new System.Drawing.Size(85, 17);
            this.targetCriminalCheckBox.TabIndex = 74;
            this.targetCriminalCheckBox.Text = "Grey (Aggro)";
            this.targetCriminalCheckBox.UseVisualStyleBackColor = true;
            // 
            // targetGreyCheckBox
            // 
            this.targetGreyCheckBox.AutoSize = true;
            this.targetGreyCheckBox.ForeColor = System.Drawing.Color.DimGray;
            this.targetGreyCheckBox.Location = new System.Drawing.Point(6, 57);
            this.targetGreyCheckBox.Name = "targetGreyCheckBox";
            this.targetGreyCheckBox.Size = new System.Drawing.Size(48, 17);
            this.targetGreyCheckBox.TabIndex = 73;
            this.targetGreyCheckBox.Text = "Grey";
            this.targetGreyCheckBox.UseVisualStyleBackColor = true;
            // 
            // targetGreenCheckBox
            // 
            this.targetGreenCheckBox.AutoSize = true;
            this.targetGreenCheckBox.ForeColor = System.Drawing.Color.Green;
            this.targetGreenCheckBox.Location = new System.Drawing.Point(6, 38);
            this.targetGreenCheckBox.Name = "targetGreenCheckBox";
            this.targetGreenCheckBox.Size = new System.Drawing.Size(55, 17);
            this.targetGreenCheckBox.TabIndex = 72;
            this.targetGreenCheckBox.Text = "Green";
            this.targetGreenCheckBox.UseVisualStyleBackColor = true;
            // 
            // targetBlueCheckBox
            // 
            this.targetBlueCheckBox.AutoSize = true;
            this.targetBlueCheckBox.ForeColor = System.Drawing.Color.Blue;
            this.targetBlueCheckBox.Location = new System.Drawing.Point(6, 19);
            this.targetBlueCheckBox.Name = "targetBlueCheckBox";
            this.targetBlueCheckBox.Size = new System.Drawing.Size(47, 17);
            this.targetBlueCheckBox.TabIndex = 71;
            this.targetBlueCheckBox.Text = "Blue";
            this.targetBlueCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox56
            // 
            this.groupBox56.Controls.Add(this.targetSelectorComboBox);
            this.groupBox56.Location = new System.Drawing.Point(140, 321);
            this.groupBox56.Name = "groupBox56";
            this.groupBox56.Size = new System.Drawing.Size(200, 40);
            this.groupBox56.TabIndex = 55;
            this.groupBox56.TabStop = false;
            this.groupBox56.Text = "Selector";
            // 
            // targetSelectorComboBox
            // 
            this.targetSelectorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetSelectorComboBox.FormattingEnabled = true;
            this.targetSelectorComboBox.Location = new System.Drawing.Point(8, 14);
            this.targetSelectorComboBox.Name = "targetSelectorComboBox";
            this.targetSelectorComboBox.Size = new System.Drawing.Size(182, 21);
            this.targetSelectorComboBox.TabIndex = 11;
            // 
            // groupBox55
            // 
            this.groupBox55.Controls.Add(this.targetNameTextBox);
            this.groupBox55.Location = new System.Drawing.Point(346, 321);
            this.groupBox55.Name = "groupBox55";
            this.groupBox55.Size = new System.Drawing.Size(180, 40);
            this.groupBox55.TabIndex = 54;
            this.groupBox55.TabStop = false;
            this.groupBox55.Text = "Target (Char/Mob) Name";
            // 
            // targetNameTextBox
            // 
            this.targetNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetNameTextBox.BackColor = System.Drawing.Color.White;
            this.targetNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetNameTextBox.Location = new System.Drawing.Point(10, 15);
            this.targetNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.targetNameTextBox.Name = "targetNameTextBox";
            this.targetNameTextBox.Size = new System.Drawing.Size(162, 20);
            this.targetNameTextBox.TabIndex = 13;
            // 
            // groupBox48
            // 
            this.groupBox48.Controls.Add(this.label73);
            this.groupBox48.Controls.Add(this.label74);
            this.groupBox48.Controls.Add(this.targetRangeMaxTextBox);
            this.groupBox48.Controls.Add(this.label75);
            this.groupBox48.Controls.Add(this.targetRangeMinTextBox);
            this.groupBox48.Location = new System.Drawing.Point(374, 6);
            this.groupBox48.Name = "groupBox48";
            this.groupBox48.Size = new System.Drawing.Size(101, 94);
            this.groupBox48.TabIndex = 53;
            this.groupBox48.TabStop = false;
            this.groupBox48.Text = "Range";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(9, 77);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(85, 13);
            this.label73.TabIndex = 4;
            this.label73.Text = "Set -1 for no limit";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(9, 48);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(30, 13);
            this.label74.TabIndex = 3;
            this.label74.Text = "Max:";
            // 
            // targetRangeMaxTextBox
            // 
            this.targetRangeMaxTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetRangeMaxTextBox.BackColor = System.Drawing.Color.White;
            this.targetRangeMaxTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetRangeMaxTextBox.Location = new System.Drawing.Point(43, 45);
            this.targetRangeMaxTextBox.Name = "targetRangeMaxTextBox";
            this.targetRangeMaxTextBox.Size = new System.Drawing.Size(47, 20);
            this.targetRangeMaxTextBox.TabIndex = 2;
            this.targetRangeMaxTextBox.Text = "-1";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(9, 22);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(27, 13);
            this.label75.TabIndex = 1;
            this.label75.Text = "Min:";
            // 
            // targetRangeMinTextBox
            // 
            this.targetRangeMinTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetRangeMinTextBox.BackColor = System.Drawing.Color.White;
            this.targetRangeMinTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetRangeMinTextBox.Location = new System.Drawing.Point(43, 19);
            this.targetRangeMinTextBox.Name = "targetRangeMinTextBox";
            this.targetRangeMinTextBox.Size = new System.Drawing.Size(47, 20);
            this.targetRangeMinTextBox.TabIndex = 0;
            this.targetRangeMinTextBox.Text = "-1";
            // 
            // groupBox46
            // 
            this.groupBox46.Controls.Add(this.groupBox47);
            this.groupBox46.Controls.Add(this.groupBox49);
            this.groupBox46.Controls.Add(this.groupBox50);
            this.groupBox46.Controls.Add(this.groupBox51);
            this.groupBox46.Controls.Add(this.groupBox52);
            this.groupBox46.Controls.Add(this.groupBox53);
            this.groupBox46.Controls.Add(this.groupBox54);
            this.groupBox46.Location = new System.Drawing.Point(481, 6);
            this.groupBox46.Name = "groupBox46";
            this.groupBox46.Size = new System.Drawing.Size(180, 311);
            this.groupBox46.TabIndex = 52;
            this.groupBox46.TabStop = false;
            this.groupBox46.Text = "Flags";
            // 
            // groupBox47
            // 
            this.groupBox47.Controls.Add(this.paralizedBoth);
            this.groupBox47.Controls.Add(this.paralizedOff);
            this.groupBox47.Controls.Add(this.paralizedOn);
            this.groupBox47.Location = new System.Drawing.Point(6, 263);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.Size = new System.Drawing.Size(163, 37);
            this.groupBox47.TabIndex = 97;
            this.groupBox47.TabStop = false;
            this.groupBox47.Text = "Paralized";
            // 
            // paralizedBoth
            // 
            this.paralizedBoth.Checked = true;
            this.paralizedBoth.Location = new System.Drawing.Point(107, 15);
            this.paralizedBoth.Name = "paralizedBoth";
            this.paralizedBoth.Size = new System.Drawing.Size(50, 20);
            this.paralizedBoth.TabIndex = 59;
            this.paralizedBoth.TabStop = true;
            this.paralizedBoth.Text = "Both";
            // 
            // paralizedOff
            // 
            this.paralizedOff.Location = new System.Drawing.Point(60, 15);
            this.paralizedOff.Name = "paralizedOff";
            this.paralizedOff.Size = new System.Drawing.Size(41, 20);
            this.paralizedOff.TabIndex = 58;
            this.paralizedOff.Text = "No";
            // 
            // paralizedOn
            // 
            this.paralizedOn.Location = new System.Drawing.Point(6, 15);
            this.paralizedOn.Name = "paralizedOn";
            this.paralizedOn.Size = new System.Drawing.Size(48, 20);
            this.paralizedOn.TabIndex = 57;
            this.paralizedOn.Text = "Yes";
            // 
            // groupBox49
            // 
            this.groupBox49.Controls.Add(this.friendOn);
            this.groupBox49.Controls.Add(this.friendBoth);
            this.groupBox49.Controls.Add(this.friendOff);
            this.groupBox49.Location = new System.Drawing.Point(6, 222);
            this.groupBox49.Name = "groupBox49";
            this.groupBox49.Size = new System.Drawing.Size(163, 37);
            this.groupBox49.TabIndex = 95;
            this.groupBox49.TabStop = false;
            this.groupBox49.Text = "Friend";
            // 
            // friendOn
            // 
            this.friendOn.Location = new System.Drawing.Point(6, 15);
            this.friendOn.Name = "friendOn";
            this.friendOn.Size = new System.Drawing.Size(48, 20);
            this.friendOn.TabIndex = 65;
            this.friendOn.Text = "Yes";
            // 
            // friendBoth
            // 
            this.friendBoth.Checked = true;
            this.friendBoth.Location = new System.Drawing.Point(107, 15);
            this.friendBoth.Name = "friendBoth";
            this.friendBoth.Size = new System.Drawing.Size(50, 20);
            this.friendBoth.TabIndex = 67;
            this.friendBoth.TabStop = true;
            this.friendBoth.Text = "Both";
            // 
            // friendOff
            // 
            this.friendOff.Location = new System.Drawing.Point(60, 14);
            this.friendOff.Name = "friendOff";
            this.friendOff.Size = new System.Drawing.Size(41, 20);
            this.friendOff.TabIndex = 66;
            this.friendOff.Text = "No";
            // 
            // groupBox50
            // 
            this.groupBox50.Controls.Add(this.warmodeOn);
            this.groupBox50.Controls.Add(this.warmodeBoth);
            this.groupBox50.Controls.Add(this.warmodeOff);
            this.groupBox50.Location = new System.Drawing.Point(6, 179);
            this.groupBox50.Name = "groupBox50";
            this.groupBox50.Size = new System.Drawing.Size(163, 37);
            this.groupBox50.TabIndex = 94;
            this.groupBox50.TabStop = false;
            this.groupBox50.Text = "Warmode";
            // 
            // warmodeOn
            // 
            this.warmodeOn.Location = new System.Drawing.Point(6, 15);
            this.warmodeOn.Name = "warmodeOn";
            this.warmodeOn.Size = new System.Drawing.Size(48, 20);
            this.warmodeOn.TabIndex = 69;
            this.warmodeOn.Text = "Yes";
            // 
            // warmodeBoth
            // 
            this.warmodeBoth.Checked = true;
            this.warmodeBoth.Location = new System.Drawing.Point(107, 15);
            this.warmodeBoth.Name = "warmodeBoth";
            this.warmodeBoth.Size = new System.Drawing.Size(50, 20);
            this.warmodeBoth.TabIndex = 71;
            this.warmodeBoth.TabStop = true;
            this.warmodeBoth.Text = "Both";
            // 
            // warmodeOff
            // 
            this.warmodeOff.Location = new System.Drawing.Point(60, 15);
            this.warmodeOff.Name = "warmodeOff";
            this.warmodeOff.Size = new System.Drawing.Size(41, 20);
            this.warmodeOff.TabIndex = 70;
            this.warmodeOff.Text = "No";
            // 
            // groupBox51
            // 
            this.groupBox51.Controls.Add(this.ghostOn);
            this.groupBox51.Controls.Add(this.ghostBoth);
            this.groupBox51.Controls.Add(this.ghostOff);
            this.groupBox51.Location = new System.Drawing.Point(6, 139);
            this.groupBox51.Name = "groupBox51";
            this.groupBox51.Size = new System.Drawing.Size(163, 37);
            this.groupBox51.TabIndex = 92;
            this.groupBox51.TabStop = false;
            this.groupBox51.Text = "Ghost";
            // 
            // ghostOn
            // 
            this.ghostOn.Location = new System.Drawing.Point(6, 14);
            this.ghostOn.Name = "ghostOn";
            this.ghostOn.Size = new System.Drawing.Size(48, 20);
            this.ghostOn.TabIndex = 73;
            this.ghostOn.Text = "Yes";
            // 
            // ghostBoth
            // 
            this.ghostBoth.Checked = true;
            this.ghostBoth.Location = new System.Drawing.Point(107, 14);
            this.ghostBoth.Name = "ghostBoth";
            this.ghostBoth.Size = new System.Drawing.Size(50, 20);
            this.ghostBoth.TabIndex = 75;
            this.ghostBoth.TabStop = true;
            this.ghostBoth.Text = "Both";
            // 
            // ghostOff
            // 
            this.ghostOff.Location = new System.Drawing.Point(60, 14);
            this.ghostOff.Name = "ghostOff";
            this.ghostOff.Size = new System.Drawing.Size(41, 20);
            this.ghostOff.TabIndex = 74;
            this.ghostOff.Text = "No";
            // 
            // groupBox52
            // 
            this.groupBox52.Controls.Add(this.humanOn);
            this.groupBox52.Controls.Add(this.humanOff);
            this.groupBox52.Controls.Add(this.humanBoth);
            this.groupBox52.Location = new System.Drawing.Point(6, 98);
            this.groupBox52.Name = "groupBox52";
            this.groupBox52.Size = new System.Drawing.Size(163, 37);
            this.groupBox52.TabIndex = 91;
            this.groupBox52.TabStop = false;
            this.groupBox52.Text = "Human";
            // 
            // humanOn
            // 
            this.humanOn.Location = new System.Drawing.Point(6, 15);
            this.humanOn.Name = "humanOn";
            this.humanOn.Size = new System.Drawing.Size(48, 20);
            this.humanOn.TabIndex = 77;
            this.humanOn.Text = "Yes";
            // 
            // humanOff
            // 
            this.humanOff.Location = new System.Drawing.Point(60, 15);
            this.humanOff.Name = "humanOff";
            this.humanOff.Size = new System.Drawing.Size(41, 20);
            this.humanOff.TabIndex = 78;
            this.humanOff.Text = "No";
            // 
            // humanBoth
            // 
            this.humanBoth.Checked = true;
            this.humanBoth.Location = new System.Drawing.Point(107, 15);
            this.humanBoth.Name = "humanBoth";
            this.humanBoth.Size = new System.Drawing.Size(50, 20);
            this.humanBoth.TabIndex = 79;
            this.humanBoth.TabStop = true;
            this.humanBoth.Text = "Both";
            // 
            // groupBox53
            // 
            this.groupBox53.Controls.Add(this.blessedOn);
            this.groupBox53.Controls.Add(this.blessedOff);
            this.groupBox53.Controls.Add(this.blessedBoth);
            this.groupBox53.Location = new System.Drawing.Point(6, 58);
            this.groupBox53.Name = "groupBox53";
            this.groupBox53.Size = new System.Drawing.Size(163, 37);
            this.groupBox53.TabIndex = 90;
            this.groupBox53.TabStop = false;
            this.groupBox53.Text = "Yellow Hits";
            // 
            // blessedOn
            // 
            this.blessedOn.Location = new System.Drawing.Point(6, 14);
            this.blessedOn.Name = "blessedOn";
            this.blessedOn.Size = new System.Drawing.Size(48, 20);
            this.blessedOn.TabIndex = 81;
            this.blessedOn.Text = "Yes";
            // 
            // blessedOff
            // 
            this.blessedOff.Location = new System.Drawing.Point(60, 14);
            this.blessedOff.Name = "blessedOff";
            this.blessedOff.Size = new System.Drawing.Size(41, 20);
            this.blessedOff.TabIndex = 82;
            this.blessedOff.Text = "No";
            // 
            // blessedBoth
            // 
            this.blessedBoth.Checked = true;
            this.blessedBoth.Location = new System.Drawing.Point(107, 14);
            this.blessedBoth.Name = "blessedBoth";
            this.blessedBoth.Size = new System.Drawing.Size(50, 20);
            this.blessedBoth.TabIndex = 83;
            this.blessedBoth.TabStop = true;
            this.blessedBoth.Text = "Both";
            // 
            // groupBox54
            // 
            this.groupBox54.Controls.Add(this.poisonedOn);
            this.groupBox54.Controls.Add(this.poisonedOff);
            this.groupBox54.Controls.Add(this.poisonedBoth);
            this.groupBox54.Location = new System.Drawing.Point(6, 19);
            this.groupBox54.Name = "groupBox54";
            this.groupBox54.Size = new System.Drawing.Size(163, 37);
            this.groupBox54.TabIndex = 89;
            this.groupBox54.TabStop = false;
            this.groupBox54.Text = "Poisoned";
            // 
            // poisonedOn
            // 
            this.poisonedOn.Location = new System.Drawing.Point(6, 13);
            this.poisonedOn.Name = "poisonedOn";
            this.poisonedOn.Size = new System.Drawing.Size(48, 20);
            this.poisonedOn.TabIndex = 85;
            this.poisonedOn.Text = "Yes";
            // 
            // poisonedOff
            // 
            this.poisonedOff.Location = new System.Drawing.Point(60, 13);
            this.poisonedOff.Name = "poisonedOff";
            this.poisonedOff.Size = new System.Drawing.Size(41, 20);
            this.poisonedOff.TabIndex = 86;
            this.poisonedOff.Text = "No";
            // 
            // poisonedBoth
            // 
            this.poisonedBoth.Checked = true;
            this.poisonedBoth.Location = new System.Drawing.Point(107, 13);
            this.poisonedBoth.Name = "poisonedBoth";
            this.poisonedBoth.Size = new System.Drawing.Size(50, 20);
            this.poisonedBoth.TabIndex = 87;
            this.poisonedBoth.TabStop = true;
            this.poisonedBoth.Text = "Both";
            // 
            // groupBox45
            // 
            this.groupBox45.Controls.Add(this.targetChoseHue);
            this.groupBox45.Controls.Add(this.targethueGridView);
            this.groupBox45.Controls.Add(this.targetcoloCheckBox);
            this.groupBox45.Location = new System.Drawing.Point(257, 6);
            this.groupBox45.Name = "groupBox45";
            this.groupBox45.Size = new System.Drawing.Size(111, 313);
            this.groupBox45.TabIndex = 51;
            this.groupBox45.TabStop = false;
            this.groupBox45.Text = "Color Filter";
            // 
            // targetChoseHue
            // 
            this.targetChoseHue.Location = new System.Drawing.Point(7, 284);
            this.targetChoseHue.Name = "targetChoseHue";
            this.targetChoseHue.Size = new System.Drawing.Size(95, 23);
            this.targetChoseHue.TabIndex = 71;
            this.targetChoseHue.Text = "Target Hue ID";
            this.targetChoseHue.UseVisualStyleBackColor = true;
            this.targetChoseHue.Click += new System.EventHandler(this.targetChoseHue_Click);
            // 
            // targethueGridView
            // 
            this.targethueGridView.AllowDrop = true;
            this.targethueGridView.AllowUserToResizeRows = false;
            this.targethueGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.targethueGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn21});
            this.targethueGridView.Location = new System.Drawing.Point(7, 43);
            this.targethueGridView.Name = "targethueGridView";
            this.targethueGridView.RowHeadersVisible = false;
            this.targethueGridView.RowHeadersWidth = 62;
            this.targethueGridView.Size = new System.Drawing.Size(95, 233);
            this.targethueGridView.TabIndex = 70;
            this.targethueGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.targethueGridView_CellEndEdit);
            this.targethueGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_MouseDown);
            this.targethueGridView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_MouseMove);
            this.targethueGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.targethueGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.targethueGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.targethueGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.targetfilter_DefaultValuesNeeded);
            this.targethueGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.targethueGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn21.HeaderText = "Hue";
            this.dataGridViewTextBoxColumn21.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.Width = 75;
            // 
            // targetcoloCheckBox
            // 
            this.targetcoloCheckBox.AutoSize = true;
            this.targetcoloCheckBox.Location = new System.Drawing.Point(7, 20);
            this.targetcoloCheckBox.Name = "targetcoloCheckBox";
            this.targetcoloCheckBox.Size = new System.Drawing.Size(59, 17);
            this.targetcoloCheckBox.TabIndex = 0;
            this.targetcoloCheckBox.Text = "Enable";
            this.targetcoloCheckBox.UseVisualStyleBackColor = true;
            this.targetcoloCheckBox.CheckedChanged += new System.EventHandler(this.targetcoloCheckBox_CheckedChanged);
            // 
            // groupBox44
            // 
            this.groupBox44.Controls.Add(this.targetChoseBody);
            this.groupBox44.Controls.Add(this.targetbodydataGridView);
            this.groupBox44.Controls.Add(this.targetbodyCheckBox);
            this.groupBox44.Location = new System.Drawing.Point(140, 6);
            this.groupBox44.Name = "groupBox44";
            this.groupBox44.Size = new System.Drawing.Size(111, 313);
            this.groupBox44.TabIndex = 50;
            this.groupBox44.TabStop = false;
            this.groupBox44.Text = "Body Filter";
            // 
            // targetChoseBody
            // 
            this.targetChoseBody.Location = new System.Drawing.Point(7, 284);
            this.targetChoseBody.Name = "targetChoseBody";
            this.targetChoseBody.Size = new System.Drawing.Size(95, 23);
            this.targetChoseBody.TabIndex = 58;
            this.targetChoseBody.Text = "Target Body ID";
            this.targetChoseBody.UseVisualStyleBackColor = true;
            this.targetChoseBody.Click += new System.EventHandler(this.targetChoseBody_Click);
            // 
            // targetbodydataGridView
            // 
            this.targetbodydataGridView.AllowDrop = true;
            this.targetbodydataGridView.AllowUserToResizeRows = false;
            this.targetbodydataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.targetbodydataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn20});
            this.targetbodydataGridView.Location = new System.Drawing.Point(7, 43);
            this.targetbodydataGridView.Name = "targetbodydataGridView";
            this.targetbodydataGridView.RowHeadersVisible = false;
            this.targetbodydataGridView.RowHeadersWidth = 62;
            this.targetbodydataGridView.Size = new System.Drawing.Size(95, 233);
            this.targetbodydataGridView.TabIndex = 70;
            this.targetbodydataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.targetbodydataGridView_CellEndEdit);
            this.targetbodydataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseUp);
            this.targetbodydataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridView_CurrentCellDirtyStateChanged);
            this.targetbodydataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridView_DataError);
            this.targetbodydataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.targetfilter_DefaultValuesNeeded);
            this.targetbodydataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridView_DragDrop);
            this.targetbodydataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.GridView_DragOver);
            this.targetbodydataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.targetbodydataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn20.HeaderText = "Body ID";
            this.dataGridViewTextBoxColumn20.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn20.Width = 75;
            // 
            // targetbodyCheckBox
            // 
            this.targetbodyCheckBox.AutoSize = true;
            this.targetbodyCheckBox.Location = new System.Drawing.Point(7, 20);
            this.targetbodyCheckBox.Name = "targetbodyCheckBox";
            this.targetbodyCheckBox.Size = new System.Drawing.Size(59, 17);
            this.targetbodyCheckBox.TabIndex = 0;
            this.targetbodyCheckBox.Text = "Enable";
            this.targetbodyCheckBox.UseVisualStyleBackColor = true;
            this.targetbodyCheckBox.CheckedChanged += new System.EventHandler(this.targetbodyCheckBox_CheckedChanged);
            // 
            // groupBox43
            // 
            this.groupBox43.Controls.Add(this.label76);
            this.groupBox43.Controls.Add(this.targetremoveButton);
            this.groupBox43.Controls.Add(this.targetaddButton);
            this.groupBox43.Controls.Add(this.targetaddTextBox);
            this.groupBox43.Controls.Add(this.targetlistBox);
            this.groupBox43.Location = new System.Drawing.Point(8, 6);
            this.groupBox43.Name = "groupBox43";
            this.groupBox43.Size = new System.Drawing.Size(126, 355);
            this.groupBox43.TabIndex = 49;
            this.groupBox43.TabStop = false;
            this.groupBox43.Text = "Shortcut";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(6, 22);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(38, 13);
            this.label76.TabIndex = 5;
            this.label76.Text = "Name:";
            // 
            // targetremoveButton
            // 
            this.targetremoveButton.Location = new System.Drawing.Point(7, 45);
            this.targetremoveButton.Name = "targetremoveButton";
            this.targetremoveButton.Size = new System.Drawing.Size(55, 23);
            this.targetremoveButton.TabIndex = 5;
            this.targetremoveButton.Text = "Remove";
            this.targetremoveButton.UseVisualStyleBackColor = true;
            this.targetremoveButton.Click += new System.EventHandler(this.targetremoveButton_Click);
            // 
            // targetaddButton
            // 
            this.targetaddButton.Location = new System.Drawing.Point(65, 45);
            this.targetaddButton.Name = "targetaddButton";
            this.targetaddButton.Size = new System.Drawing.Size(55, 23);
            this.targetaddButton.TabIndex = 4;
            this.targetaddButton.Text = "Add";
            this.targetaddButton.UseVisualStyleBackColor = true;
            this.targetaddButton.Click += new System.EventHandler(this.targetaddButton_Click);
            // 
            // targetaddTextBox
            // 
            this.targetaddTextBox.Location = new System.Drawing.Point(44, 19);
            this.targetaddTextBox.Name = "targetaddTextBox";
            this.targetaddTextBox.Size = new System.Drawing.Size(76, 20);
            this.targetaddTextBox.TabIndex = 1;
            // 
            // targetlistBox
            // 
            this.targetlistBox.FormattingEnabled = true;
            this.targetlistBox.Location = new System.Drawing.Point(7, 72);
            this.targetlistBox.Name = "targetlistBox";
            this.targetlistBox.Size = new System.Drawing.Size(113, 225);
            this.targetlistBox.TabIndex = 0;
            this.targetlistBox.SelectedIndexChanged += new System.EventHandler(this.targetlistBox_SelectedIndexChanged);
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
            this.dispDelta.Location = new System.Drawing.Point(517, 144);
            this.dispDelta.Name = "dispDelta";
            this.dispDelta.Size = new System.Drawing.Size(113, 22);
            this.dispDelta.TabIndex = 11;
            this.dispDelta.Text = "Display Changes";
            this.dispDelta.CheckedChanged += new System.EventHandler(this.dispDelta_CheckedChanged);
            // 
            // skillCopyAll
            // 
            this.skillCopyAll.Location = new System.Drawing.Point(517, 109);
            this.skillCopyAll.Name = "skillCopyAll";
            this.skillCopyAll.Size = new System.Drawing.Size(131, 20);
            this.skillCopyAll.TabIndex = 9;
            this.skillCopyAll.Text = "Copy All";
            this.skillCopyAll.Click += new System.EventHandler(this.skillCopyAll_Click);
            // 
            // skillCopySel
            // 
            this.skillCopySel.Location = new System.Drawing.Point(517, 81);
            this.skillCopySel.Name = "skillCopySel";
            this.skillCopySel.Size = new System.Drawing.Size(131, 21);
            this.skillCopySel.TabIndex = 8;
            this.skillCopySel.Text = "Copy Selected";
            this.skillCopySel.Click += new System.EventHandler(this.skillCopySel_Click);
            // 
            // baseTotal
            // 
            this.baseTotal.Location = new System.Drawing.Point(586, 175);
            this.baseTotal.Name = "baseTotal";
            this.baseTotal.ReadOnly = true;
            this.baseTotal.Size = new System.Drawing.Size(44, 20);
            this.baseTotal.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(516, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Base Total:";
            // 
            // locks
            // 
            this.locks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locks.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Locked"});
            this.locks.Location = new System.Drawing.Point(598, 45);
            this.locks.Name = "locks";
            this.locks.Size = new System.Drawing.Size(50, 23);
            this.locks.TabIndex = 5;
            // 
            // setlocks
            // 
            this.setlocks.Location = new System.Drawing.Point(517, 46);
            this.setlocks.Name = "setlocks";
            this.setlocks.Size = new System.Drawing.Size(76, 20);
            this.setlocks.TabIndex = 4;
            this.setlocks.Text = "Set all locks:";
            this.setlocks.Click += new System.EventHandler(this.OnSetSkillLocks);
            // 
            // resetDelta
            // 
            this.resetDelta.Location = new System.Drawing.Point(517, 13);
            this.resetDelta.Name = "resetDelta";
            this.resetDelta.Size = new System.Drawing.Size(131, 20);
            this.resetDelta.TabIndex = 3;
            this.resetDelta.Text = "Reset  +/-";
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
            this.skillList.HideSelection = false;
            this.skillList.Location = new System.Drawing.Point(7, 13);
            this.skillList.Name = "skillList";
            this.skillList.Size = new System.Drawing.Size(492, 345);
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
            // enhancedHotKeytabPage
            // 
            this.enhancedHotKeytabPage.Controls.Add(this.groupBox8);
            this.enhancedHotKeytabPage.Controls.Add(this.groupBox28);
            this.enhancedHotKeytabPage.Controls.Add(this.groupBox27);
            this.enhancedHotKeytabPage.Controls.Add(this.hotkeytreeView);
            this.enhancedHotKeytabPage.Location = new System.Drawing.Point(4, 40);
            this.enhancedHotKeytabPage.Name = "enhancedHotKeytabPage";
            this.enhancedHotKeytabPage.Padding = new System.Windows.Forms.Padding(3);
            this.enhancedHotKeytabPage.Size = new System.Drawing.Size(666, 366);
            this.enhancedHotKeytabPage.TabIndex = 15;
            this.enhancedHotKeytabPage.Text = "HotKey";
            this.enhancedHotKeytabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.hotkeyMasterClearButton);
            this.groupBox8.Controls.Add(this.hotkeyKeyMasterTextBox);
            this.groupBox8.Controls.Add(this.hotkeyMasterSetButton);
            this.groupBox8.Controls.Add(this.label42);
            this.groupBox8.Location = new System.Drawing.Point(502, 105);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(156, 84);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Master Key";
            // 
            // hotkeyMasterClearButton
            // 
            this.hotkeyMasterClearButton.Location = new System.Drawing.Point(92, 50);
            this.hotkeyMasterClearButton.Name = "hotkeyMasterClearButton";
            this.hotkeyMasterClearButton.Size = new System.Drawing.Size(53, 23);
            this.hotkeyMasterClearButton.TabIndex = 5;
            this.hotkeyMasterClearButton.Text = "Clear";
            this.hotkeyMasterClearButton.UseVisualStyleBackColor = true;
            this.hotkeyMasterClearButton.Click += new System.EventHandler(this.hotkeyMasterClearButton_Click);
            // 
            // hotkeyKeyMasterTextBox
            // 
            this.hotkeyKeyMasterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hotkeyKeyMasterTextBox.BackColor = System.Drawing.Color.White;
            this.hotkeyKeyMasterTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyKeyMasterTextBox.Location = new System.Drawing.Point(41, 19);
            this.hotkeyKeyMasterTextBox.Name = "hotkeyKeyMasterTextBox";
            this.hotkeyKeyMasterTextBox.ReadOnly = true;
            this.hotkeyKeyMasterTextBox.Size = new System.Drawing.Size(104, 20);
            this.hotkeyKeyMasterTextBox.TabIndex = 5;
            this.hotkeyKeyMasterTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HotKey_MouseDown);
            this.hotkeyKeyMasterTextBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HotKey_MouseRoll);
            // 
            // hotkeyMasterSetButton
            // 
            this.hotkeyMasterSetButton.Location = new System.Drawing.Point(10, 50);
            this.hotkeyMasterSetButton.Name = "hotkeyMasterSetButton";
            this.hotkeyMasterSetButton.Size = new System.Drawing.Size(53, 23);
            this.hotkeyMasterSetButton.TabIndex = 7;
            this.hotkeyMasterSetButton.Text = "Set";
            this.hotkeyMasterSetButton.UseVisualStyleBackColor = true;
            this.hotkeyMasterSetButton.Click += new System.EventHandler(this.hotkeyMasterSetButton_Click);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(7, 22);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(28, 13);
            this.label42.TabIndex = 6;
            this.label42.Text = "Key:";
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.hotkeyMDisableButton);
            this.groupBox28.Controls.Add(this.hotkeyMEnableButton);
            this.groupBox28.Controls.Add(this.hotkeyKeyMasterLabel);
            this.groupBox28.Controls.Add(this.hotkeyStatusLabel);
            this.groupBox28.Location = new System.Drawing.Point(502, 7);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(156, 92);
            this.groupBox28.TabIndex = 3;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "General";
            // 
            // hotkeyMDisableButton
            // 
            this.hotkeyMDisableButton.Location = new System.Drawing.Point(92, 58);
            this.hotkeyMDisableButton.Name = "hotkeyMDisableButton";
            this.hotkeyMDisableButton.Size = new System.Drawing.Size(53, 23);
            this.hotkeyMDisableButton.TabIndex = 8;
            this.hotkeyMDisableButton.Text = "Disable";
            this.hotkeyMDisableButton.UseVisualStyleBackColor = true;
            this.hotkeyMDisableButton.Click += new System.EventHandler(this.hotkeyDisableButton_Click);
            // 
            // hotkeyMEnableButton
            // 
            this.hotkeyMEnableButton.Location = new System.Drawing.Point(10, 58);
            this.hotkeyMEnableButton.Name = "hotkeyMEnableButton";
            this.hotkeyMEnableButton.Size = new System.Drawing.Size(53, 23);
            this.hotkeyMEnableButton.TabIndex = 9;
            this.hotkeyMEnableButton.Text = "Enable";
            this.hotkeyMEnableButton.UseVisualStyleBackColor = true;
            this.hotkeyMEnableButton.Click += new System.EventHandler(this.hotkeyEnableButton_Click);
            // 
            // hotkeyKeyMasterLabel
            // 
            this.hotkeyKeyMasterLabel.AutoSize = true;
            this.hotkeyKeyMasterLabel.Location = new System.Drawing.Point(7, 38);
            this.hotkeyKeyMasterLabel.Name = "hotkeyKeyMasterLabel";
            this.hotkeyKeyMasterLabel.Size = new System.Drawing.Size(101, 13);
            this.hotkeyKeyMasterLabel.TabIndex = 4;
            this.hotkeyKeyMasterLabel.Text = "ON/OFF Key: None";
            // 
            // hotkeyStatusLabel
            // 
            this.hotkeyStatusLabel.AutoSize = true;
            this.hotkeyStatusLabel.Location = new System.Drawing.Point(7, 16);
            this.hotkeyStatusLabel.Name = "hotkeyStatusLabel";
            this.hotkeyStatusLabel.Size = new System.Drawing.Size(82, 13);
            this.hotkeyStatusLabel.TabIndex = 3;
            this.hotkeyStatusLabel.Text = "Status: Enabled";
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.hotkeypassCheckBox);
            this.groupBox27.Controls.Add(this.hotkeyClearButton);
            this.groupBox27.Controls.Add(this.hotkeySetButton);
            this.groupBox27.Controls.Add(this.label39);
            this.groupBox27.Controls.Add(this.hotkeytextbox);
            this.groupBox27.Location = new System.Drawing.Point(502, 195);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(156, 107);
            this.groupBox27.TabIndex = 2;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Modify Key";
            // 
            // hotkeypassCheckBox
            // 
            this.hotkeypassCheckBox.Location = new System.Drawing.Point(10, 43);
            this.hotkeypassCheckBox.Name = "hotkeypassCheckBox";
            this.hotkeypassCheckBox.Size = new System.Drawing.Size(103, 22);
            this.hotkeypassCheckBox.TabIndex = 49;
            this.hotkeypassCheckBox.Text = "Pass Key to UO";
            this.hotkeypassCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hotkeyClearButton
            // 
            this.hotkeyClearButton.Location = new System.Drawing.Point(92, 71);
            this.hotkeyClearButton.Name = "hotkeyClearButton";
            this.hotkeyClearButton.Size = new System.Drawing.Size(53, 23);
            this.hotkeyClearButton.TabIndex = 4;
            this.hotkeyClearButton.Text = "Clear";
            this.hotkeyClearButton.UseVisualStyleBackColor = true;
            this.hotkeyClearButton.Click += new System.EventHandler(this.hotkeyClearButton_Click);
            // 
            // hotkeySetButton
            // 
            this.hotkeySetButton.Location = new System.Drawing.Point(10, 71);
            this.hotkeySetButton.Name = "hotkeySetButton";
            this.hotkeySetButton.Size = new System.Drawing.Size(53, 23);
            this.hotkeySetButton.TabIndex = 3;
            this.hotkeySetButton.Text = "Set";
            this.hotkeySetButton.UseVisualStyleBackColor = true;
            this.hotkeySetButton.Click += new System.EventHandler(this.hotkeySetButton_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(7, 20);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(28, 13);
            this.label39.TabIndex = 2;
            this.label39.Text = "Key:";
            // 
            // hotkeytextbox
            // 
            this.hotkeytextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hotkeytextbox.BackColor = System.Drawing.Color.White;
            this.hotkeytextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeytextbox.Location = new System.Drawing.Point(41, 17);
            this.hotkeytextbox.Name = "hotkeytextbox";
            this.hotkeytextbox.ReadOnly = true;
            this.hotkeytextbox.Size = new System.Drawing.Size(104, 20);
            this.hotkeytextbox.TabIndex = 1;
            this.hotkeytextbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HotKey_MouseDown);
            this.hotkeytextbox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HotKey_MouseRoll);
            // 
            // hotkeytreeView
            // 
            this.hotkeytreeView.Location = new System.Drawing.Point(9, 7);
            this.hotkeytreeView.Name = "hotkeytreeView";
            this.hotkeytreeView.Size = new System.Drawing.Size(487, 353);
            this.hotkeytreeView.TabIndex = 0;
            this.hotkeytreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.hotkeytreeView_AfterSelect);
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
            this.imgFmt.Size = new System.Drawing.Size(71, 23);
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
            this.capNow.Location = new System.Drawing.Point(314, 14);
            this.capNow.Name = "capNow";
            this.capNow.Size = new System.Drawing.Size(285, 20);
            this.capNow.TabIndex = 8;
            this.capNow.Text = "Take Screen Shot Now";
            this.capNow.Click += new System.EventHandler(this.capNow_Click);
            // 
            // screenPath
            // 
            this.screenPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenPath.BackColor = System.Drawing.Color.White;
            this.screenPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPath.Location = new System.Drawing.Point(7, 14);
            this.screenPath.Name = "screenPath";
            this.screenPath.Size = new System.Drawing.Size(253, 20);
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
            this.screenAutoCap.Size = new System.Drawing.Size(180, 22);
            this.screenAutoCap.TabIndex = 4;
            this.screenAutoCap.Text = "Auto Death Screen Capture";
            this.screenAutoCap.CheckedChanged += new System.EventHandler(this.screenAutoCap_CheckedChanged);
            // 
            // setScnPath
            // 
            this.setScnPath.Location = new System.Drawing.Point(208, 16);
            this.setScnPath.Name = "setScnPath";
            this.setScnPath.Size = new System.Drawing.Size(22, 17);
            this.setScnPath.TabIndex = 3;
            this.setScnPath.Text = "...";
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
            this.dispTime.Size = new System.Drawing.Size(180, 22);
            this.dispTime.TabIndex = 9;
            this.dispTime.Text = "Include Timestamp on images";
            this.dispTime.CheckedChanged += new System.EventHandler(this.dispTime_CheckedChanged);
            // 
            // videoTab
            // 
            this.videoTab.BackColor = System.Drawing.SystemColors.Control;
            this.videoTab.Controls.Add(this.videoRecStatuslabel);
            this.videoTab.Controls.Add(this.label64);
            this.videoTab.Controls.Add(this.groupBox40);
            this.videoTab.Controls.Add(this.videosettinggroupBox);
            this.videoTab.Controls.Add(this.videorecbutton);
            this.videoTab.Controls.Add(this.videostopbutton);
            this.videoTab.Controls.Add(this.groupBox15);
            this.videoTab.Location = new System.Drawing.Point(4, 40);
            this.videoTab.Name = "videoTab";
            this.videoTab.Padding = new System.Windows.Forms.Padding(3);
            this.videoTab.Size = new System.Drawing.Size(666, 366);
            this.videoTab.TabIndex = 16;
            this.videoTab.Text = "Video Recorder";
            // 
            // videoRecStatuslabel
            // 
            this.videoRecStatuslabel.AutoSize = true;
            this.videoRecStatuslabel.ForeColor = System.Drawing.Color.Green;
            this.videoRecStatuslabel.Location = new System.Drawing.Point(185, 334);
            this.videoRecStatuslabel.Name = "videoRecStatuslabel";
            this.videoRecStatuslabel.Size = new System.Drawing.Size(24, 13);
            this.videoRecStatuslabel.TabIndex = 95;
            this.videoRecStatuslabel.Text = "Idle";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(120, 334);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(63, 13);
            this.label64.TabIndex = 94;
            this.label64.Text = "Rec Status:";
            // 
            // groupBox40
            // 
            this.groupBox40.Controls.Add(this.videoSourcePlayer);
            this.groupBox40.Location = new System.Drawing.Point(259, 6);
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.Size = new System.Drawing.Size(399, 352);
            this.groupBox40.TabIndex = 64;
            this.groupBox40.TabStop = false;
            this.groupBox40.Text = "Playback";
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.Location = new System.Drawing.Point(7, 20);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(386, 321);
            this.videoSourcePlayer.TabIndex = 0;
            this.videoSourcePlayer.Text = "videoSourcePlayer";
            this.videoSourcePlayer.VideoSource = null;
            // 
            // videosettinggroupBox
            // 
            this.videosettinggroupBox.Controls.Add(this.videoCodecComboBox);
            this.videosettinggroupBox.Controls.Add(this.label63);
            this.videosettinggroupBox.Controls.Add(this.label62);
            this.videosettinggroupBox.Controls.Add(this.videoFPSTextBox);
            this.videosettinggroupBox.Location = new System.Drawing.Point(10, 250);
            this.videosettinggroupBox.Name = "videosettinggroupBox";
            this.videosettinggroupBox.Size = new System.Drawing.Size(243, 66);
            this.videosettinggroupBox.TabIndex = 63;
            this.videosettinggroupBox.TabStop = false;
            this.videosettinggroupBox.Text = "Video Settings";
            // 
            // videoCodecComboBox
            // 
            this.videoCodecComboBox.FormattingEnabled = true;
            this.videoCodecComboBox.Items.AddRange(new object[] {
            "Default",
            "MPEG4",
            "WMV1",
            "WMV2",
            "MSMPEG4v2",
            "MSMPEG4v3",
            "H263P",
            "FLV1",
            "MPEG2",
            "Raw",
            "FFV1",
            "FFVHUFF",
            "H264",
            "H265",
            "Theora",
            "VP8",
            "VP9"});
            this.videoCodecComboBox.Location = new System.Drawing.Point(122, 27);
            this.videoCodecComboBox.Name = "videoCodecComboBox";
            this.videoCodecComboBox.Size = new System.Drawing.Size(110, 21);
            this.videoCodecComboBox.TabIndex = 63;
            this.videoCodecComboBox.SelectedIndexChanged += new System.EventHandler(this.videoCodecComboBox_SelectedIndexChanged);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(81, 31);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(41, 13);
            this.label63.TabIndex = 62;
            this.label63.Text = "Codec:";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(7, 31);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(33, 13);
            this.label62.TabIndex = 61;
            this.label62.Text = "FPS: ";
            // 
            // videoFPSTextBox
            // 
            this.videoFPSTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoFPSTextBox.BackColor = System.Drawing.Color.White;
            this.videoFPSTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.videoFPSTextBox.Location = new System.Drawing.Point(42, 28);
            this.videoFPSTextBox.Name = "videoFPSTextBox";
            this.videoFPSTextBox.Size = new System.Drawing.Size(33, 20);
            this.videoFPSTextBox.TabIndex = 60;
            this.videoFPSTextBox.TextChanged += new System.EventHandler(this.videoFPSTextBox_TextChanged);
            // 
            // videorecbutton
            // 
            this.videorecbutton.BackgroundImage = global::Assistant.Properties.Resources.record;
            this.videorecbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.videorecbutton.FlatAppearance.BorderSize = 0;
            this.videorecbutton.Location = new System.Drawing.Point(43, 325);
            this.videorecbutton.Name = "videorecbutton";
            this.videorecbutton.Size = new System.Drawing.Size(30, 30);
            this.videorecbutton.TabIndex = 93;
            this.videorecbutton.UseVisualStyleBackColor = true;
            this.videorecbutton.Click += new System.EventHandler(this.videorecbutton_Click);
            // 
            // videostopbutton
            // 
            this.videostopbutton.BackgroundImage = global::Assistant.Properties.Resources.stopagent;
            this.videostopbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.videostopbutton.FlatAppearance.BorderSize = 0;
            this.videostopbutton.Location = new System.Drawing.Point(79, 325);
            this.videostopbutton.Name = "videostopbutton";
            this.videostopbutton.Size = new System.Drawing.Size(30, 30);
            this.videostopbutton.TabIndex = 92;
            this.videostopbutton.UseVisualStyleBackColor = true;
            this.videostopbutton.Click += new System.EventHandler(this.videostopbutton_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.videolistBox);
            this.groupBox15.Controls.Add(this.videoPathButton);
            this.groupBox15.Controls.Add(this.videoPathTextBox);
            this.groupBox15.Location = new System.Drawing.Point(8, 6);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(245, 238);
            this.groupBox15.TabIndex = 62;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "File";
            // 
            // videolistBox
            // 
            this.videolistBox.IntegralHeight = false;
            this.videolistBox.Location = new System.Drawing.Point(11, 41);
            this.videolistBox.Name = "videolistBox";
            this.videolistBox.Size = new System.Drawing.Size(223, 183);
            this.videolistBox.Sorted = true;
            this.videolistBox.TabIndex = 8;
            this.videolistBox.SelectedIndexChanged += new System.EventHandler(this.videoList_SelectedIndexChanged);
            this.videolistBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.videoList_MouseDown);
            // 
            // videoPathButton
            // 
            this.videoPathButton.Location = new System.Drawing.Point(212, 17);
            this.videoPathButton.Name = "videoPathButton";
            this.videoPathButton.Size = new System.Drawing.Size(22, 17);
            this.videoPathButton.TabIndex = 9;
            this.videoPathButton.Text = "...";
            this.videoPathButton.Click += new System.EventHandler(this.videoPathButton_Click);
            // 
            // videoPathTextBox
            // 
            this.videoPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPathTextBox.BackColor = System.Drawing.Color.White;
            this.videoPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.videoPathTextBox.Location = new System.Drawing.Point(11, 15);
            this.videoPathTextBox.Name = "videoPathTextBox";
            this.videoPathTextBox.Size = new System.Drawing.Size(195, 20);
            this.videoPathTextBox.TabIndex = 10;
            // 
            // DPStabPage
            // 
            this.DPStabPage.Controls.Add(this.filtergroup);
            this.DPStabPage.Controls.Add(this.DpsMeterGridView);
            this.DPStabPage.Controls.Add(this.DPSMeterStatusLabel);
            this.DPStabPage.Controls.Add(this.label67);
            this.DPStabPage.Controls.Add(this.DPSMeterPauseButton);
            this.DPStabPage.Controls.Add(this.DPSMeterStopButton);
            this.DPStabPage.Controls.Add(this.DPSMeterStartButton);
            this.DPStabPage.Controls.Add(this.DPSMeterClearButton);
            this.DPStabPage.Location = new System.Drawing.Point(4, 40);
            this.DPStabPage.Name = "DPStabPage";
            this.DPStabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DPStabPage.Size = new System.Drawing.Size(666, 366);
            this.DPStabPage.TabIndex = 17;
            this.DPStabPage.Text = "DPS Meter";
            this.DPStabPage.UseVisualStyleBackColor = true;
            // 
            // filtergroup
            // 
            this.filtergroup.Controls.Add(this.DPSMeterClearFilterButton);
            this.filtergroup.Controls.Add(this.DPSMeterApplyFilterButton);
            this.filtergroup.Controls.Add(this.DPSmetername);
            this.filtergroup.Controls.Add(this.label70);
            this.filtergroup.Controls.Add(this.DPSmeterserial);
            this.filtergroup.Controls.Add(this.label69);
            this.filtergroup.Controls.Add(this.label68);
            this.filtergroup.Controls.Add(this.DPSmetermaxdamage);
            this.filtergroup.Controls.Add(this.label66);
            this.filtergroup.Controls.Add(this.DPSmetermindamage);
            this.filtergroup.Location = new System.Drawing.Point(372, 53);
            this.filtergroup.Name = "filtergroup";
            this.filtergroup.Size = new System.Drawing.Size(288, 167);
            this.filtergroup.TabIndex = 66;
            this.filtergroup.TabStop = false;
            this.filtergroup.Text = "Filter";
            // 
            // DPSMeterClearFilterButton
            // 
            this.DPSMeterClearFilterButton.Location = new System.Drawing.Point(144, 129);
            this.DPSMeterClearFilterButton.Name = "DPSMeterClearFilterButton";
            this.DPSMeterClearFilterButton.Size = new System.Drawing.Size(63, 21);
            this.DPSMeterClearFilterButton.TabIndex = 68;
            this.DPSMeterClearFilterButton.Text = "Clear";
            this.DPSMeterClearFilterButton.Click += new System.EventHandler(this.DPSMeterClearFilterButton_Click);
            // 
            // DPSMeterApplyFilterButton
            // 
            this.DPSMeterApplyFilterButton.Location = new System.Drawing.Point(213, 129);
            this.DPSMeterApplyFilterButton.Name = "DPSMeterApplyFilterButton";
            this.DPSMeterApplyFilterButton.Size = new System.Drawing.Size(63, 21);
            this.DPSMeterApplyFilterButton.TabIndex = 67;
            this.DPSMeterApplyFilterButton.Text = "Apply";
            this.DPSMeterApplyFilterButton.Click += new System.EventHandler(this.DPSMeterApplyFilterButton_Click);
            // 
            // DPSmetername
            // 
            this.DPSmetername.Location = new System.Drawing.Point(48, 93);
            this.DPSmetername.Name = "DPSmetername";
            this.DPSmetername.Size = new System.Drawing.Size(228, 20);
            this.DPSmetername.TabIndex = 8;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(6, 96);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(38, 13);
            this.label70.TabIndex = 7;
            this.label70.Text = "Name:";
            // 
            // DPSmeterserial
            // 
            this.DPSmeterserial.Location = new System.Drawing.Point(48, 58);
            this.DPSmeterserial.Name = "DPSmeterserial";
            this.DPSmeterserial.Size = new System.Drawing.Size(100, 20);
            this.DPSmeterserial.TabIndex = 6;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(6, 61);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(36, 13);
            this.label69.TabIndex = 5;
            this.label69.Text = "Serial:";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(144, 26);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(73, 13);
            this.label68.TabIndex = 3;
            this.label68.Text = "Damage Max:";
            // 
            // DPSmetermaxdamage
            // 
            this.DPSmetermaxdamage.Location = new System.Drawing.Point(220, 23);
            this.DPSmetermaxdamage.Name = "DPSmetermaxdamage";
            this.DPSmetermaxdamage.Size = new System.Drawing.Size(56, 20);
            this.DPSmetermaxdamage.TabIndex = 2;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(6, 26);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(70, 13);
            this.label66.TabIndex = 1;
            this.label66.Text = "Damage Min:";
            // 
            // DPSmetermindamage
            // 
            this.DPSmetermindamage.Location = new System.Drawing.Point(82, 23);
            this.DPSmetermindamage.Name = "DPSmetermindamage";
            this.DPSmetermindamage.Size = new System.Drawing.Size(56, 20);
            this.DPSmetermindamage.TabIndex = 0;
            // 
            // DpsMeterGridView
            // 
            this.DpsMeterGridView.AllowDrop = true;
            this.DpsMeterGridView.AllowUserToResizeRows = false;
            this.DpsMeterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DpsMeterGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15});
            this.DpsMeterGridView.Location = new System.Drawing.Point(8, 6);
            this.DpsMeterGridView.Name = "DpsMeterGridView";
            this.DpsMeterGridView.RowHeadersVisible = false;
            this.DpsMeterGridView.RowHeadersWidth = 62;
            this.DpsMeterGridView.Size = new System.Drawing.Size(355, 352);
            this.DpsMeterGridView.TabIndex = 65;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn13.HeaderText = "Serial";
            this.dataGridViewTextBoxColumn13.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn13.Width = 70;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn14.HeaderText = "Name";
            this.dataGridViewTextBoxColumn14.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn14.Width = 200;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn15.HeaderText = "Damage";
            this.dataGridViewTextBoxColumn15.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn15.Width = 60;
            // 
            // DPSMeterStatusLabel
            // 
            this.DPSMeterStatusLabel.AutoSize = true;
            this.DPSMeterStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DPSMeterStatusLabel.ForeColor = System.Drawing.Color.Green;
            this.DPSMeterStatusLabel.Location = new System.Drawing.Point(409, 343);
            this.DPSMeterStatusLabel.Name = "DPSMeterStatusLabel";
            this.DPSMeterStatusLabel.Size = new System.Drawing.Size(28, 13);
            this.DPSMeterStatusLabel.TabIndex = 64;
            this.DPSMeterStatusLabel.Text = "Idle";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(369, 343);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(40, 13);
            this.label67.TabIndex = 63;
            this.label67.Text = "Status:";
            // 
            // DPSMeterPauseButton
            // 
            this.DPSMeterPauseButton.Location = new System.Drawing.Point(516, 17);
            this.DPSMeterPauseButton.Name = "DPSMeterPauseButton";
            this.DPSMeterPauseButton.Size = new System.Drawing.Size(63, 21);
            this.DPSMeterPauseButton.TabIndex = 61;
            this.DPSMeterPauseButton.Text = "Pause";
            this.DPSMeterPauseButton.Click += new System.EventHandler(this.DPSMeterPauseButton_Click);
            // 
            // DPSMeterStopButton
            // 
            this.DPSMeterStopButton.Location = new System.Drawing.Point(447, 17);
            this.DPSMeterStopButton.Name = "DPSMeterStopButton";
            this.DPSMeterStopButton.Size = new System.Drawing.Size(63, 21);
            this.DPSMeterStopButton.TabIndex = 60;
            this.DPSMeterStopButton.Text = "Stop";
            this.DPSMeterStopButton.Click += new System.EventHandler(this.DPSMeterStopButton_Click);
            // 
            // DPSMeterStartButton
            // 
            this.DPSMeterStartButton.Location = new System.Drawing.Point(378, 17);
            this.DPSMeterStartButton.Name = "DPSMeterStartButton";
            this.DPSMeterStartButton.Size = new System.Drawing.Size(63, 21);
            this.DPSMeterStartButton.TabIndex = 59;
            this.DPSMeterStartButton.Text = "Start";
            this.DPSMeterStartButton.Click += new System.EventHandler(this.DPSMeterStartButton_Click);
            // 
            // DPSMeterClearButton
            // 
            this.DPSMeterClearButton.Location = new System.Drawing.Point(585, 17);
            this.DPSMeterClearButton.Name = "DPSMeterClearButton";
            this.DPSMeterClearButton.Size = new System.Drawing.Size(63, 21);
            this.DPSMeterClearButton.TabIndex = 58;
            this.DPSMeterClearButton.Text = "Clear";
            this.DPSMeterClearButton.Click += new System.EventHandler(this.DPSMeterClearButton_Click);
            // 
            // statusTab
            // 
            this.statusTab.Controls.Add(this.advertisementLink);
            this.statusTab.Controls.Add(this.advertisement);
            this.statusTab.Controls.Add(this.label71);
            this.statusTab.Controls.Add(this.labelHotride);
            this.statusTab.Controls.Add(this.labelStatus);
            this.statusTab.Controls.Add(this.discordrazorButton);
            this.statusTab.Controls.Add(this.razorButtonWiki);
            this.statusTab.Location = new System.Drawing.Point(4, 40);
            this.statusTab.Name = "statusTab";
            this.statusTab.Size = new System.Drawing.Size(666, 366);
            this.statusTab.TabIndex = 9;
            this.statusTab.Text = "Help / Status";
            // 
            // advertisementLink
            // 
            this.advertisementLink.Location = new System.Drawing.Point(340, 240);
            this.advertisementLink.Name = "advertisementLink";
            this.advertisementLink.Size = new System.Drawing.Size(120, 37);
            this.advertisementLink.TabIndex = 12;
            this.advertisementLink.Text = "Eventine";
            this.advertisementLink.UseVisualStyleBackColor = true;
            this.advertisementLink.Click += new System.EventHandler(this.advertisement_Click);
            // 
            // advertisement
            // 
            this.advertisement.Image = ((System.Drawing.Image)(resources.GetObject("advertisement.Image")));
            this.advertisement.InitialImage = ((System.Drawing.Image)(resources.GetObject("advertisement.InitialImage")));
            this.advertisement.Location = new System.Drawing.Point(3, 3);
            this.advertisement.Name = "advertisement";
            this.advertisement.Size = new System.Drawing.Size(474, 231);
            this.advertisement.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.advertisement.TabIndex = 11;
            this.advertisement.TabStop = false;
            // 
            // label71
            // 
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.Location = new System.Drawing.Point(8, 323);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(650, 38);
            this.label71.TabIndex = 10;
            this.label71.Text = "Many thanks also for developer of UO.DLL and ULTIMA.DLL";
            // 
            // labelHotride
            // 
            this.labelHotride.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHotride.Location = new System.Drawing.Point(8, 291);
            this.labelHotride.Name = "labelHotride";
            this.labelHotride.Size = new System.Drawing.Size(650, 38);
            this.labelHotride.TabIndex = 8;
            this.labelHotride.Text = "Many thanks to Hotride for his  FPS multiclient patch! Hotride is the author of O" +
    "penGL OrionUO Client project (you can point your browser to the link http://foru" +
    "m.orion-client.online for more info)";
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Location = new System.Drawing.Point(483, 9);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(175, 268);
            this.labelStatus.TabIndex = 1;
            // 
            // discordrazorButton
            // 
            this.discordrazorButton.Location = new System.Drawing.Point(174, 240);
            this.discordrazorButton.Name = "discordrazorButton";
            this.discordrazorButton.Size = new System.Drawing.Size(135, 37);
            this.discordrazorButton.TabIndex = 9;
            this.discordrazorButton.Text = "Razor Enhanced Discord";
            this.discordrazorButton.UseVisualStyleBackColor = true;
            this.discordrazorButton.Click += new System.EventHandler(this.discordrazorButton_Click);
            // 
            // razorButtonWiki
            // 
            this.razorButtonWiki.Location = new System.Drawing.Point(3, 240);
            this.razorButtonWiki.Name = "razorButtonWiki";
            this.razorButtonWiki.Size = new System.Drawing.Size(145, 37);
            this.razorButtonWiki.TabIndex = 6;
            this.razorButtonWiki.Text = "Razor Enhanced wiki";
            this.razorButtonWiki.UseVisualStyleBackColor = true;
            this.razorButtonWiki.Click += new System.EventHandler(this.razorButtonWiki_Click);
            // 
            // m_NotifyIcon
            // 
            this.m_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_NotifyIcon.Icon")));
            this.m_NotifyIcon.Text = "Razor Enhanced";
            this.m_NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // openFileDialogscript
            // 
            this.openFileDialogscript.Filter = "Script Files|*.py;*.txt";
            this.openFileDialogscript.InitialDirectory = "C:\\Users\\credz\\AppData\\Local\\Microsoft\\VisualStudio\\16.0_1b6dbd30\\ProjectAssembli" +
    "es\\zro-nw7h01\\Scripts";
            this.openFileDialogscript.RestoreDirectory = true;
            // 
            // timerupdatestatus
            // 
            this.timerupdatestatus.Enabled = true;
            this.timerupdatestatus.Interval = 1000;
            this.timerupdatestatus.Tick += new System.EventHandler(this.timerupdatestatus_Tick);
            // 
            // datagridMenuStrip
            // 
            this.datagridMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.datagridMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteRowToolStripMenuItem});
            this.datagridMenuStrip.Name = "datagridMenuStrip";
            this.datagridMenuStrip.Size = new System.Drawing.Size(134, 26);
            this.datagridMenuStrip.Click += new System.EventHandler(this.datagridMenuStrip_Click);
            // 
            // deleteRowToolStripMenuItem
            // 
            this.deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            this.deleteRowToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.deleteRowToolStripMenuItem.Text = "Delete Row";
            // 
            // scriptgridMenuStrip
            // 
            this.scriptgridMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.scriptgridMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem,
            this.flagsToolStripMenuItem,
            this.playToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.scriptgridMenuStrip.Name = "scriptgridMenuStrip";
            this.scriptgridMenuStrip.Size = new System.Drawing.Size(113, 92);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.openToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.modifyToolStripMenuItem.Text = "Modify";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // flagsToolStripMenuItem
            // 
            this.flagsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loopModeToolStripMenuItem,
            this.waitBeforeInterruptToolStripMenuItem,
            this.autoStartAtLoginToolStripMenuItem});
            this.flagsToolStripMenuItem.Name = "flagsToolStripMenuItem";
            this.flagsToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.flagsToolStripMenuItem.Text = "Flags";
            // 
            // loopModeToolStripMenuItem
            // 
            this.loopModeToolStripMenuItem.Name = "loopModeToolStripMenuItem";
            this.loopModeToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.loopModeToolStripMenuItem.Text = "Loop Mode";
            this.loopModeToolStripMenuItem.Click += new System.EventHandler(this.loopModeToolStripMenuItem_Click);
            // 
            // waitBeforeInterruptToolStripMenuItem
            // 
            this.waitBeforeInterruptToolStripMenuItem.Name = "waitBeforeInterruptToolStripMenuItem";
            this.waitBeforeInterruptToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.waitBeforeInterruptToolStripMenuItem.Text = "Wait Before Interrupt";
            this.waitBeforeInterruptToolStripMenuItem.Click += new System.EventHandler(this.waitBeforeInterruptToolStripMenuItem_Click);
            // 
            // autoStartAtLoginToolStripMenuItem
            // 
            this.autoStartAtLoginToolStripMenuItem.Name = "autoStartAtLoginToolStripMenuItem";
            this.autoStartAtLoginToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.autoStartAtLoginToolStripMenuItem.Text = "AutoStart at Login";
            this.autoStartAtLoginToolStripMenuItem.Click += new System.EventHandler(this.autoStartAtLoginToolStripMenuItem_Click);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // timertitlestatusbar
            // 
            this.timertitlestatusbar.Enabled = true;
            this.timertitlestatusbar.Interval = 200;
            this.timertitlestatusbar.Tick += new System.EventHandler(this.timertitlestatusbar_Tick);
            // 
            // openmaplocation
            // 
            this.openmaplocation.DefaultExt = "exe";
            this.openmaplocation.FileName = "EnhancedMap.exe";
            this.openmaplocation.Filter = "Executable Files|*.exe";
            this.openmaplocation.RestoreDirectory = true;
            this.openmaplocation.Title = "Select Enhanced Map";
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
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabs.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.groupBox29.ResumeLayout(false);
            this.groupBox29.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.moreOptTab.ResumeLayout(false);
            this.moreOptTab.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.enhancedFilterTab.ResumeLayout(false);
            this.uomodgroupbox.ResumeLayout(false);
            this.groupBox32.ResumeLayout(false);
            this.groupBox32.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.graphfilterdatagrid)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.scriptingTab.ResumeLayout(false);
            this.groupBox42.ResumeLayout(false);
            this.groupBox42.PerformLayout();
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.groupBox30.ResumeLayout(false);
            this.groupBox30.PerformLayout();
            this.EnhancedAgent.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.eautoloot.ResumeLayout(false);
            this.eautoloot.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autolootdataGridView)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.escavenger.ResumeLayout(false);
            this.escavenger.PerformLayout();
            this.groupBox41.ResumeLayout(false);
            this.groupBox41.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scavengerdataGridView)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.organizer.ResumeLayout(false);
            this.organizer.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.organizerdataGridView)).EndInit();
            this.groupBox16.ResumeLayout(false);
            this.VendorBuy.ResumeLayout(false);
            this.VendorBuy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vendorbuydataGridView)).EndInit();
            this.groupBox18.ResumeLayout(false);
            this.VendorSell.ResumeLayout(false);
            this.VendorSell.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vendorsellGridView)).EndInit();
            this.groupBox20.ResumeLayout(false);
            this.Dress.ResumeLayout(false);
            this.Dress.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.friends.ResumeLayout(false);
            this.friends.PerformLayout();
            this.groupBox34.ResumeLayout(false);
            this.groupBox33.ResumeLayout(false);
            this.friendGroupBox.ResumeLayout(false);
            this.friendloggroupBox.ResumeLayout(false);
            this.restock.ResumeLayout(false);
            this.restock.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.restockdataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.bandageheal.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.toolbarTab.ResumeLayout(false);
            this.toolbarstab.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox39.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolbar_trackBar)).EndInit();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox38.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spellgrid_trackBar)).EndInit();
            this.groupBox37.ResumeLayout(false);
            this.groupBox37.PerformLayout();
            this.groupBox36.ResumeLayout(false);
            this.groupBox36.PerformLayout();
            this.groupBox35.ResumeLayout(false);
            this.groupBox35.PerformLayout();
            this.emptyTab.ResumeLayout(false);
            this.groupBox57.ResumeLayout(false);
            this.groupBox57.PerformLayout();
            this.groupBox56.ResumeLayout(false);
            this.groupBox55.ResumeLayout(false);
            this.groupBox55.PerformLayout();
            this.groupBox48.ResumeLayout(false);
            this.groupBox48.PerformLayout();
            this.groupBox46.ResumeLayout(false);
            this.groupBox47.ResumeLayout(false);
            this.groupBox49.ResumeLayout(false);
            this.groupBox50.ResumeLayout(false);
            this.groupBox51.ResumeLayout(false);
            this.groupBox52.ResumeLayout(false);
            this.groupBox53.ResumeLayout(false);
            this.groupBox54.ResumeLayout(false);
            this.groupBox45.ResumeLayout(false);
            this.groupBox45.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targethueGridView)).EndInit();
            this.groupBox44.ResumeLayout(false);
            this.groupBox44.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetbodydataGridView)).EndInit();
            this.groupBox43.ResumeLayout(false);
            this.groupBox43.PerformLayout();
            this.skillsTab.ResumeLayout(false);
            this.skillsTab.PerformLayout();
            this.enhancedHotKeytabPage.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            this.screenshotTab.ResumeLayout(false);
            this.screenshotTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).EndInit();
            this.videoTab.ResumeLayout(false);
            this.videoTab.PerformLayout();
            this.groupBox40.ResumeLayout(false);
            this.videosettinggroupBox.ResumeLayout(false);
            this.videosettinggroupBox.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.DPStabPage.ResumeLayout(false);
            this.DPStabPage.PerformLayout();
            this.filtergroup.ResumeLayout(false);
            this.filtergroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DpsMeterGridView)).EndInit();
            this.statusTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advertisement)).EndInit();
            this.datagridMenuStrip.ResumeLayout(false);
            this.scriptgridMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == 1025)
            {
                msg.Result = (IntPtr)(Assistant.Client.Instance.OnMessage(this, (uint)msg.WParam.ToInt32(), msg.LParam.ToInt32()) ? 1 : 0);
                return;
            }
            if (msg.Msg >= 1224 && msg.Msg <= 1338)
            {
                msg.Result = (IntPtr)Assistant.UOAssist.OnUOAMessage(this, msg.Msg, msg.WParam.ToInt32(), msg.LParam.ToInt32());
                return;
            }
            base.WndProc(ref msg);
        }

        private void DisableCloseButton()
        {
            IntPtr menu = DLLImport.Win.GetSystemMenu(this.Handle, false);
            DLLImport.Win.EnableMenuItem(menu, 0xF060, 0x00000002); //menu, SC_CLOSE, MF_BYCOMMAND|MF_GRAYED
            m_CanClose = false;
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            Assistant.Timer.Slice();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            m_SystemTimer = new System.Timers.Timer(5);
            m_SystemTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            Timer.SystemTimer = m_SystemTimer;

            this.Hide();

            bool st = RazorEnhanced.Settings.General.ReadBool("Systray");
            taskbar.Checked = this.ShowInTaskbar = !st;
            systray.Checked = m_NotifyIcon.Visible = st;

            UpdateTitle();

            if (!Assistant.Client.Instance.InstallHooks(this.Handle)) // WaitForInputIdle done here
            {
                m_CanClose = true;
                //SplashScreen.End();
                this.Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            InitConfig();

            this.Show();
            this.BringToFront();

            Engine.ActiveWindow = this;

            DisableCloseButton();

            tabs_IndexChanged(this, null); // load first tab

            m_Tip.Active = true;
            //SplashScreen.End();

            // AutoUpdater
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.ReportErrors = true;
            AutoUpdater.Start("http://www.razorenhanced.net/download/RazorAutoUpdater.xml");
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (Client.IsOSI)
            {
                if (args != null)
                {
                    if (args.IsUpdateAvailable)
                    {
                        DialogResult dialogResult;

                        dialogResult =
                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {
                                        args.InstalledVersion
                                    }. Do you want to update the application now?", @"Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                        if (dialogResult.Equals(DialogResult.Yes))
                        {
                            try
                            {
                                if (AutoUpdater.DownloadUpdate())
                                {
                                    Application.Exit();
                                    Thread.Sleep(2000); // attesa uscita
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                            @"There is a problem reaching update server please check your internet connection and try again later.",
                            @"Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        internal void LoadSettings()
        {
            // -------------- SCRIPTING --------------------
            scriptTable = Settings.Dataset.Tables["SCRIPTING"];
            ReloadScriptTable();

            // ---------------- AUTOLOOT -----------------
            AutoLoot.RefreshLists();
            autolootautostartCheckBox.Checked = Settings.General.ReadBool("AutolootAutostartCheckBox");

            // ------------ SCAVENGER -------------------
            Scavenger.RefreshLists();
            scavengerautostartCheckBox.Checked = Settings.General.ReadBool("ScavengerAutostartCheckBox");

            // ---------------- ORGANIZER ----------------
            Organizer.RefreshLists();

            // ----------- SELL AGENT -----------------
            SellAgent.RefreshLists();

            // ------------------- BUY AGENT ----------------------
            BuyAgent.RefreshLists();

            // ------------------ DRESS AGENT -------------------------
            RazorEnhanced.Dress.RefreshLists();

            // ------------------ FRIEND -------------------------
            Friend.RefreshLists();

            // ------------------ RESTOCK -------------------------
            Restock.RefreshLists();

            // ------------------ BANDAGE HEAL --------------------
            BandageHeal.LoadSettings();

            // ------------------ ENHANCED FILTERS --------------------
            RazorEnhanced.Filters.LoadSettings();

            // ------------------ ENHANCED TOOLBAR --------------------
            RazorEnhanced.ToolBar.LoadSettings();
            toolbar_trackBar.Value = RazorEnhanced.Settings.General.ReadInt("ToolBarOpacity");

            // ------------------ ENHANCED SPELLGRID --------------------
            RazorEnhanced.SpellGrid.LoadSettings();
            spellgrid_trackBar.Value = RazorEnhanced.Settings.General.ReadInt("GridOpacity");

            // ------------------ TARGETS --------------------
            targetSelectorComboBox.DataSource = TargetGUI.Selectors;
            targetcoloCheckBox.Checked = targethueGridView.Enabled = false;
            targetbodyCheckBox.Checked = targetbodydataGridView.Enabled = false;
            JsonData.Filter.RefreshTargetShortCut(targetlistBox);
            if (targetlistBox.Items.Count > 0)
                EnableTargetGUI();
            else
                DisableTargetGUI();

            // ------------------ HOTKEY --------------------
            //HotKey.Init();

            // ------------------ DPS METER --------------------
            DPSMeterStopButton.Enabled = DPSMeterPauseButton.Enabled = false;
            DPSMeter.Clear();
            DpsMeterGridView.Rows.Clear();

            // ------------------ PARAMETRI GENERALI -------------------
            imgFmt.SelectedItem = RazorEnhanced.Settings.General.ReadString("ImageFormat");

            screenPath.Text = RazorEnhanced.Settings.General.ReadString("CapPath");
            radioUO.Checked = !(radioFull.Checked = RazorEnhanced.Settings.General.ReadBool("CapFullScreen"));
            imgFmt.SelectedItem = RazorEnhanced.Settings.General.ReadString("ImageFormat");
            dispTime.Checked = RazorEnhanced.Settings.General.ReadBool("CapTimeStamp");
            screenAutoCap.Checked = RazorEnhanced.Settings.General.ReadBool("AutoCap");
            Filters.Filter.Load();
            Filters.Filter.Draw(filters);
            if (Assistant.Client.IsOSI)
            {
                smartCPU.Checked = RazorEnhanced.Settings.General.ReadBool("SmartCPU");
            }
            else
            {
                smartCPU.Checked = false;
            }

            this.TopMost = alwaysTop.Checked = RazorEnhanced.Settings.General.ReadBool("AlwaysOnTop");
            rememberPwds.Checked = RazorEnhanced.Settings.General.ReadBool("RememberPwds");

            forceSizeX.Text = RazorEnhanced.Settings.General.ReadInt("ForceSizeX").ToString();
            forceSizeY.Text = RazorEnhanced.Settings.General.ReadInt("ForceSizeY").ToString();
            gameSize.Checked = RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled");

            notshowlauncher.Checked = RazorEnhanced.Settings.General.ReadBool("NotShowLauncher");
            forceSizeX.Enabled = forceSizeY.Enabled = gameSize.Checked;
            taskbar.Checked = !(systray.Checked = RazorEnhanced.Settings.General.ReadBool("Systray"));
            clientPrio.SelectedItem = RazorEnhanced.Settings.General.ReadString("ClientPrio");
            opacity.AutoSize = false;
            opacity.Value = RazorEnhanced.Settings.General.ReadInt("Opacity");
            this.Opacity = ((float)opacity.Value) / 100.0;
            opacityLabel.Text = String.Format("Opacity: {0}%", opacity.Value);
            msglvl.SelectedIndex = RazorEnhanced.Settings.General.ReadInt("MessageLevel");

            this.Location = new System.Drawing.Point(RazorEnhanced.Settings.General.ReadInt("WindowX"), RazorEnhanced.Settings.General.ReadInt("WindowY"));
            Assistant.Engine.MainWindowX = RazorEnhanced.Settings.General.ReadInt("WindowX");
            Assistant.Engine.MainWindowY = RazorEnhanced.Settings.General.ReadInt("WindowY");

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

            dispDelta.Checked = RazorEnhanced.Settings.General.ReadBool("DisplaySkillChanges");
            actionStatusMsg.Checked = RazorEnhanced.Settings.General.ReadBool("ActionStatusMsg");
            QueueActions.Checked = RazorEnhanced.Settings.General.ReadBool("QueueActions");
            txtObjDelay.Text = RazorEnhanced.Settings.General.ReadInt("ObjectDelay").ToString();
            smartLT.Checked = RazorEnhanced.Settings.General.ReadBool("SmartLastTarget");
            ltRange.Enabled = rangeCheckLT.Checked = RazorEnhanced.Settings.General.ReadBool("RangeCheckLT");
            ltRange.Text = RazorEnhanced.Settings.General.ReadInt("LTRange").ToString();
            showtargtext.Checked = RazorEnhanced.Settings.General.ReadBool("LastTargTextFlags");
            healthFmt.Enabled = showHealthOH.Checked = RazorEnhanced.Settings.General.ReadBool("ShowHealth");
            healthFmt.Text = RazorEnhanced.Settings.General.ReadString("HealthFmt");
            chkPartyOverhead.Checked = RazorEnhanced.Settings.General.ReadBool("ShowPartyStats");
            preAOSstatbar.Checked = RazorEnhanced.Settings.General.ReadBool("OldStatBar");
            queueTargets.Checked = RazorEnhanced.Settings.General.ReadBool("QueueTargets");
            blockDis.Checked = RazorEnhanced.Settings.General.ReadBool("BlockDismount");
            autoStackRes.Checked = RazorEnhanced.Settings.General.ReadBool("AutoStack");
            corpseRange.Enabled = openCorpses.Checked = RazorEnhanced.Settings.General.ReadBool("AutoOpenCorpses");
            corpseRange.Text = RazorEnhanced.Settings.General.ReadInt("CorpseRange").ToString();
            allowHiddenLooting.Checked = hiddedAutoOpenDoors.Enabled = RazorEnhanced.Settings.General.ReadBool("AutoOpenCorpses");
            allowHiddenLooting.Checked = RazorEnhanced.Settings.General.ReadBool("AllowHiddenLooting");

            spamFilter.Checked = RazorEnhanced.Settings.General.ReadBool("FilterSpam");
            filterSnoop.Checked = RazorEnhanced.Settings.General.ReadBool("FilterSnoopMsg");
            filterPoison.Checked = RazorEnhanced.Settings.General.ReadBool("FilterPoison");
            filterNPC.Checked = RazorEnhanced.Settings.General.ReadBool("FilterNPC");
            incomingMob.Checked = RazorEnhanced.Settings.General.ReadBool("ShowMobNames");
            incomingCorpse.Checked = RazorEnhanced.Settings.General.ReadBool("ShowCorpseNames");
            chkStealth.Checked = RazorEnhanced.Settings.General.ReadBool("CountStealthSteps");
            autoOpenDoors.Checked = hiddedAutoOpenDoors.Enabled = RazorEnhanced.Settings.General.ReadBool("AutoOpenDoors");
            hiddedAutoOpenDoors.Checked = RazorEnhanced.Settings.General.ReadBool("HiddedAutoOpenDoors");
            spellUnequip.Checked = RazorEnhanced.Settings.General.ReadBool("SpellUnequip");
            potionEquip.Checked = RazorEnhanced.Settings.General.ReadBool("PotionEquip");
            uo3dEquipUnEquip.Checked = RazorEnhanced.Settings.General.ReadBool("UO3dEquipUnEquip");
            autosearchcontainers.Checked = RazorEnhanced.Settings.General.ReadBool("AutoSearch");
            nosearchpouches.Checked = RazorEnhanced.Settings.General.ReadBool("NoSearchPouches");
            druidClericPackets.Checked = RazorEnhanced.Settings.General.ReadBool("DruidClericPackets");
            chknorunStealth.Checked = RazorEnhanced.Settings.General.ReadBool("ChkNoRunStealth");
            enhancedmappathTextBox.Text = Settings.General.ReadString("EnhancedMapPath");

            chkForceSpeechHue.Checked = setSpeechHue.Enabled = RazorEnhanced.Settings.General.ReadBool("ForceSpeechHue");
            chkForceSpellHue.Checked = setBeneHue.Enabled = setNeuHue.Enabled = setHarmHue.Enabled = RazorEnhanced.Settings.General.ReadBool("ForceSpellHue");
            if (RazorEnhanced.Settings.General.ReadInt("LTHilight") != 0)
            {
                InitPreviewHue(lthilight, "LTHilight");
                lthilight.Checked = setLTHilight.Enabled = true;
            }
            else
            {
                lthilight.Checked = setLTHilight.Enabled = false;
            }
            InitPreviewHue(lblMsgHue, "SysColor");
            InitPreviewHue(lblWarnHue, "WarningColor");
            InitPreviewHue(chkForceSpeechHue, "SpeechHue");
            InitPreviewHue(lblBeneHue, "BeneficialSpellHue");
            InitPreviewHue(lblHarmHue, "HarmfulSpellHue");
            InitPreviewHue(lblNeuHue, "NeutralSpellHue");

            txtSpellFormat.Text = RazorEnhanced.Settings.General.ReadString("SpellFormat");

            // Script
            showscriptmessageCheckBox.Checked = Settings.General.ReadBool("ShowScriptMessageCheckBox");
            scripterrorlogCheckBox.Checked = Scripts.ScriptErrorLog = Settings.General.ReadBool("ScriptErrorLog");
            scriptshowStartStopCheckBox.Checked = Scripts.ScriptStartStopMessage = Settings.General.ReadBool("ScriptStartStopMessage");

            // UoMod
            if (Engine.ClientMajor >= 7) //&& Engine.ClientBuild < 49)
            {
                uomodFPSCheckBox.Checked = RazorEnhanced.Settings.General.ReadBool("UoModFPS");
                uomodpaperdollCheckBox.Checked = RazorEnhanced.Settings.General.ReadBool("UoModPaperdoll");
                uomodglobalsoundCheckBox.Checked = RazorEnhanced.Settings.General.ReadBool("UoModSound");
            }
            else
            {
                uomodFPSCheckBox.Enabled = false;
                uomodpaperdollCheckBox.Enabled = false;
                uomodglobalsoundCheckBox.Enabled = false;
            }

            // Video Recorder
            videoPathTextBox.Text = Settings.General.ReadString("VideoPath");
            videoFPSTextBox.Text = Settings.General.ReadInt("VideoFPS").ToString();
            videoCodecComboBox.SelectedIndex = Settings.General.ReadInt("VideoFormat");
        }

        public void SetBandSelfState()
        {
            bandagehealusetextSelfContent.Enabled = bandagehealusetext.Checked;
            bandagehealusetextContent.Enabled = bandagehealusetext.Checked;
            BandageHealUseTarget.Enabled = !bandagehealusetext.Checked;
        }

        public void DisableSmartCpu(string newText)
        {
            this.smartCPU.Text = newText;
            this.smartCPU.Enabled = false;
            this.smartCPU.Checked = false;
        }

        private bool m_Initializing = false;
        internal bool Initializing
        {
            get { return m_Initializing; }
            set { m_Initializing = value; }
        }

        internal void InitConfig()
        {
            m_Initializing = true;
            LoadSettings();
            RazorEnhanced.Profiles.Refresh();

            // Init mappe ultima.dll
            Ultima.Map.InitializeMap("Felucca");
            Ultima.Map.InitializeMap("Trammel");
            Ultima.Map.InitializeMap("Ilshenar");
            Ultima.Map.InitializeMap("Malas");
            Ultima.Map.InitializeMap("Tokuno");
            Ultima.Map.InitializeMap("TerMur");

            m_Initializing = false;
        }

        public void removeVideoTab()
        {
            this.tabs.Controls.Remove(this.videoTab);
        }

        private void tabs_IndexChanged(object sender, System.EventArgs e)
        {
            if (tabs == null)
                return;
            else if (tabs.SelectedTab == skillsTab)
            {
                RedrawSkills();
            }
            else if (tabs.SelectedTab == statusTab)
            {
                UpdateRazorStatus();
            }
            else if (tabs.SelectedTab == screenshotTab)
            {
                ReloadScreenShotsList();
            }
            else if (tabs.SelectedTab == scriptingTab)
            {
                UpdateScriptGrid();
            }
            else if (tabs.SelectedTab == videoTab)
            {
                if (!tabs.SelectedTab.Enabled)
                {
                    //DialogResult dialogResult = MessageBox.Show("Microsoft Visual C++ 2015 Redistributable is not installed, is necessary for prevent crash or missing function, Want open offical download site?", "Missing Dependency", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //if (dialogResult == DialogResult.Yes)
                    //{
                    //	if (Environment.Is64BitOperatingSystem)
                    //		System.Diagnostics.Process.Start("https://download.microsoft.com/download/6/A/A/6AA4EDFF-645B-48C5-81CC-ED5963AEAD48/vc_redist.x64.exe");
                    //	else
                    //		System.Diagnostics.Process.Start("https://download.microsoft.com/download/6/A/A/6AA4EDFF-645B-48C5-81CC-ED5963AEAD48/vc_redist.x86.exe");
                    //}

                }
                else
                    ReloadVideoList();
            }
        }

        private Version m_Ver = System.Reflection.Assembly.GetCallingAssembly().GetName().Version;

        private uint m_OutPrev;
        private uint m_InPrev;

        private void UpdateRazorStatus()
        {
            if (!Assistant.Client.Instance.ClientRunning)
                Close();

            if (tabs.SelectedTab != statusTab)
                return;

            uint ps = m_OutPrev;
            uint pr = m_InPrev;
            m_OutPrev = Client.Instance.TotalDataOut(); // DLLImport.Razor.TotalOut();
            m_InPrev = Client.Instance.TotalDataIn(); //  DLLImport.Razor.TotalIn();

            int time = 0;
            if (Assistant.Client.Instance.ConnectionStart != DateTime.MinValue)
                time = (int)((DateTime.Now - Assistant.Client.Instance.ConnectionStart).TotalSeconds);

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
            if (!m_CanClose && Assistant.Client.Instance.ClientRunning)
            {
                DisableCloseButton();
                e.Cancel = true;
            }
        }

        private void MainForm_Activated(object sender, System.EventArgs e)
        {
            DisableCloseButton();
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

        private void MainForm_Move(object sender, System.EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                windowspt = this.Location;
                Assistant.Engine.MainWindowX = this.Location.X;
                Assistant.Engine.MainWindowY = this.Location.Y;
            }
        }


        internal void UpdateTitle()
        {
            string str = Language.GetControlText(this.Name);
            if (str == null || str == String.Empty)
                str = "Razor Enhanced {0}";

            str = String.Format(str, Engine.Version);
            if (World.Player != null)
                this.Text = String.Format("{1} ({2}) - {0}", str, World.Player.Name, World.ShardName);
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
            Client.BringToFront(this.Handle);
            if (RazorEnhanced.Settings.General.ReadBool("AlwaysOnTop"))
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

        private void razorButtonWiki_Click(object sender, EventArgs e)
        {
            ProcessStartInfo p = new ProcessStartInfo("http://www.razorenhanced.net/dokuwiki");
            try
            {
                Process.Start(p);
            }
            catch { }
        }

        private void advertisement_Click(object sender, EventArgs e)
        {
            ProcessStartInfo p = new ProcessStartInfo("https://www.uoeventine.com/");
            try
            {
                Process.Start(p);
            }
            catch { }
        }


        private void discordrazorButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo p = new ProcessStartInfo("https://discord.gg/P3Q7mKT");
            try
            {
                Process.Start(p);
            }
            catch { }
        }

        private void openchangelogButton_Click(object sender, EventArgs e)
        {
            EnhancedChangeLog changelogform = new EnhancedChangeLog
            {
                TopMost = true
            };
            changelogform.Show();
        }

        // ----------------- FEATURE START -------------------

        public void UpdateControlLocks()
        {
            if (!Assistant.Client.Instance.AllowBit(FeatureBit.AutolootAgent))
            {
                autoLootCheckBox.Enabled = false;
                autoLootCheckBox.Checked = false;
                if (RazorEnhanced.AutoLoot.Status())
                    RazorEnhanced.AutoLoot.Stop();
            }
            else
            {
                if (!autoLootCheckBox.Enabled)
                    autoLootCheckBox.Enabled = true;
            }

            if (!Assistant.Client.Instance.AllowBit(FeatureBit.RangeCheckLT))
            {
                rangeCheckLT.Checked = false;
                rangeCheckLT.Enabled = false;
                Settings.General.WriteBoolNoSave("RangeCheckLT", false);
            }
            else
            {
                if (!rangeCheckLT.Enabled)
                    rangeCheckLT.Enabled = true;
            }


            /*	if (Client.AllowBit(FeatureBit.AutoOpenDoors))
				{
					RazorEnhanced.AutoLoot.AddLog(Client.AllowBit(FeatureBit.LightFilter).ToString());

					//RazorEnhanced.AutoLoot.AddLog(Client.AllowBit(FeatureBit.AutoOpenDoors).ToString());
					autoOpenDoors.Checked = false;
					autoOpenDoors.Enabled = false;
					Settings.General.WriteBoolNoSave("AutoOpenDoors", false);
				}
				else
				{
					if (!autoOpenDoors.Enabled)
						autoOpenDoors.Enabled = true;
				}*/


            if (!Assistant.Client.Instance.AllowBit(FeatureBit.UnequipBeforeCast))
            {
                spellUnequip.Checked = false;
                spellUnequip.Enabled = false;
                Settings.General.WriteBoolNoSave("SpellUnequip", false);
            }
            else
            {
                if (!spellUnequip.Enabled)
                    spellUnequip.Enabled = true;
            }

            if (!Assistant.Client.Instance.AllowBit(FeatureBit.AutoPotionEquip))
            {
                potionEquip.Checked = false;
                potionEquip.Enabled = false;
                Settings.General.WriteBoolNoSave("PotionEquip", false);
            }
            else
            {
                if (!potionEquip.Enabled)
                    potionEquip.Enabled = true;
            }

            if (!Assistant.Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
            {
                blockhealpoisonCheckBox.Checked = false;
                blockhealpoisonCheckBox.Enabled = false;
                Settings.General.WriteBoolNoSave("BlockHealPoison", false);
            }
            else
            {
                if (!blockhealpoisonCheckBox.Enabled)
                    blockhealpoisonCheckBox.Enabled = true;
            }

            /*	if (!Client.AllowBit(FeatureBit.SellAgent))
				{
					sellEnableCheckBox.Enabled = false;
					sellEnableCheckBox.Checked = false;
				}
				else
				{
					if (!sellEnableCheckBox.Enabled)
						sellEnableCheckBox.Enabled = true;
				}*/


            /*if (!Client.AllowBit(FeatureBit.BuyAgent))
			{
				buyEnableCheckBox.Enabled = false;
				buyEnableCheckBox.Checked = false;
			}
			else
			{
				if (!buyEnableCheckBox.Enabled)
					buyEnableCheckBox.Enabled = true;
			}*/

            if (!Assistant.Client.Instance.AllowBit(FeatureBit.OverheadHealth))
            {
                chkPartyOverhead.Checked = false;
                chkPartyOverhead.Enabled = false;
                Settings.General.WriteBoolNoSave("ShowPartyStats", false);
            }
            else
            {
                if (!chkPartyOverhead.Enabled)
                    chkPartyOverhead.Enabled = true;
            }

            if (!Assistant.Client.Instance.AllowBit(FeatureBit.BoneCutterAgent))
            {
                bonecutterCheckBox.Checked = false;
                bonecutterCheckBox.Enabled = false;
                Settings.General.WriteBoolNoSave("BoneCutterCheckBox", false);
            }
            else
            {
                if (!bonecutterCheckBox.Enabled)
                    bonecutterCheckBox.Enabled = true;
            }

            if (!Assistant.Client.Instance.AllowBit(FeatureBit.AutoRemount))
            {
                remountcheckbox.Checked = false;
                remountcheckbox.Enabled = false;
                Settings.General.WriteBoolNoSave("RemountCheckbox", false);
            }
            else
            {
                if (!remountcheckbox.Enabled)
                    remountcheckbox.Enabled = true;
            }


            if (!Assistant.Client.Instance.AllowBit(FeatureBit.AutoBandage))
            {
                bandagehealenableCheckBox.Enabled = false;
                bandagehealenableCheckBox.Checked = false;
                if (RazorEnhanced.BandageHeal.Status())
                    RazorEnhanced.BandageHeal.Stop();
            }
            else
            {
                if (!bandagehealenableCheckBox.Enabled)
                    bandagehealenableCheckBox.Enabled = true;
            }

        }
        // ----------------- FEATURE END -------------------

        // ----------------- UO MOD START -------------------
        private void uomodFPSCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uomodFPSCheckBox.Focused)
            {
                if (uomodFPSCheckBox.Checked)
                {
                    if (Engine.ClientBuild > 49)
                        MessageBox.Show(this, "Enable this option can make client unstable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UoMod.EnableDisable(true, (int)UoMod.PATCH_TYPE.PT_FPS);
                }
                else
                    UoMod.EnableDisable(false, (int)UoMod.PATCH_TYPE.PT_FPS);

                RazorEnhanced.Settings.General.WriteBool("UoModFPS", uomodFPSCheckBox.Checked);
            }
        }

        private void uomodpaperdollCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uomodpaperdollCheckBox.Focused)
            {
                if (uomodpaperdollCheckBox.Checked)
                {
                    if (Engine.ClientBuild > 49)
                        MessageBox.Show(this, "Enable this option can make client unstable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UoMod.EnableDisable(true, (int)UoMod.PATCH_TYPE.PT_PAPERDOLL_SLOTS);
                }
                else
                    UoMod.EnableDisable(false, (int)UoMod.PATCH_TYPE.PT_PAPERDOLL_SLOTS);

                RazorEnhanced.Settings.General.WriteBool("UoModPaperdoll", uomodpaperdollCheckBox.Checked);
            }
        }

        private void uomodglobalsoundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uomodglobalsoundCheckBox.Focused)
            {
                if (uomodglobalsoundCheckBox.Checked)
                {
                    if (Engine.ClientBuild > 49)
                        MessageBox.Show(this, "Enable this option can make client unstable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UoMod.EnableDisable(true, (int)UoMod.PATCH_TYPE.PT_GLOBAL_SOUND);
                }
                else
                    UoMod.EnableDisable(false, (int)UoMod.PATCH_TYPE.PT_GLOBAL_SOUND);

                RazorEnhanced.Settings.General.WriteBool("UoModSound", uomodglobalsoundCheckBox.Checked);
            }
        }

        private void paypalButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo p = new ProcessStartInfo("https://www.paypal.me/credzba");
            Process.Start(p);
        }


        private void hiddenLooting_CheckedChanged(object sender, EventArgs e)
        {
            if (allowHiddenLooting.Focused)
                RazorEnhanced.Settings.General.WriteBool("AllowHiddenLooting", allowHiddenLooting.Checked);

        }

        private void scriptlistView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ScriptGridOpen();
        }

        // ----------------- UO MOD END -------------------
    }
}

