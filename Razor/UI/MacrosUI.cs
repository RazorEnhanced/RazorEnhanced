using Newtonsoft.Json;
using RazorEnhanced;
using RazorEnhanced.Macros;
using RazorEnhanced.Macros.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        // Macro UI Controls
        private ListBox macroListBox;
        private Button btnMacroNew;
        private Button btnMacroDelete;
        private Button btnMacroSave;
        private Button btnMacroPlay;
        private Button btnMacroStop;
        private Button btnMacroRecord;
        private Button btnMacroStopRecord;
        private ListView macroActionsListView;
        private Label lblMacroStatus;
        private CheckBox chkMacroLoop;
        private TextBox txtMacroHotkey;
        private int? m_RecordFromActionIndex = null;

        private void InitializeMacroTab2()
        {

            // Macro List
            macroListBox = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(200, 300),
                Name = "macroListBox"
            };
            macroListBox.SelectedIndexChanged += MacroListBox_SelectedIndexChanged;

            // Buttons
            btnMacroNew = new Button
            {
                Text = "New",
                Location = new Point(10, 320),
                Size = new Size(60, 25)
            };
            btnMacroNew.Click += BtnMacroNew_Click;

            btnMacroDelete = new Button
            {
                Text = "Delete",
                Location = new Point(75, 320),
                Size = new Size(60, 25)
            };
            btnMacroDelete.Click += BtnMacroDelete_Click;

            btnMacroSave = new Button
            {
                Text = "Save",
                Location = new Point(140, 320), // Adjust as needed to be next to New and Delete
                Size = new Size(60, 25)
            };
            btnMacroSave.Click += BtnMacroSave_Click;

            macroListBox = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(200, 300),
                Name = "macroListBox"
            };
            macroListBox.SelectedIndexChanged += MacroListBox_SelectedIndexChanged;

            CreateMacroListBoxContextMenu();

            btnMacroRecord = new Button
            {
                Text = "Record",
                Location = new Point(220, 10),
                Size = new Size(80, 30),
                BackColor = Color.LightCoral
            };
            btnMacroRecord.Click += BtnMacroRecord_Click;

            btnMacroStopRecord = new Button
            {
                Text = "Stop Record",
                Location = new Point(220, 45),
                Size = new Size(80, 30),
                Enabled = false
            };
            btnMacroStopRecord.Click += BtnMacroStopRecord_Click;

            btnMacroPlay = new Button
            {
                Text = "Play",
                Location = new Point(310, 10),
                Size = new Size(60, 30),
                BackColor = Color.LightGreen
            };
            btnMacroPlay.Click += BtnMacroPlay_Click;

            btnMacroStop = new Button
            {
                Text = "Stop",
                Location = new Point(375, 10),
                Size = new Size(60, 30),
                Enabled = false
            };
            btnMacroStop.Click += BtnMacroStop_Click;

            // Actions ListView
            macroActionsListView = new ListView
            {
                Location = new Point(220, 85),
                Size = new Size(440, 230),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            macroActionsListView.Columns.Add("Action", 150);
            macroActionsListView.Columns.Add("Details", 280);
            macroActionsListView.DoubleClick += MacroActionsListView_DoubleClick;   

            CreateMacroActionsContextMenu();

            // Status
            lblMacroStatus = new Label
            {
                Location = new Point(220, 320),
                Size = new Size(440, 20),
                Text = "Ready"
            };

            // Loop checkbox
            chkMacroLoop = new CheckBox
            {
                Text = "Loop",
                Location = new Point(445, 10),
                Size = new Size(60, 25)
            };
            chkMacroLoop.CheckedChanged += ChkMacroLoop_CheckedChanged;

            // Add all controls to MacrosTab
            MacrosTab.Controls.AddRange(new Control[]
            {
        macroListBox,
        btnMacroNew,
        btnMacroDelete,
        btnMacroSave, 
        btnMacroRecord,
        btnMacroStopRecord,
        btnMacroPlay,
        btnMacroStop,
        macroActionsListView,
        lblMacroStatus,
        chkMacroLoop
            });



            // Sub to MacroManager events
            MacroManager.MacrosChanged += OnMacrosChanged;
            MacroManager.RecordingStateChanged += OnRecordingStateChanged;
            MacroManager.ActionRecorded += OnActionRecorded;

            MacroManager.LoadMacrosFromFiles();

            RefreshMacroList();


        }

        private MacroAction m_CopiedAction = null;

        private void CreateMacroActionsContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem insertSetAbilityItem = new ToolStripMenuItem("Set Ability");
            insertSetAbilityItem.Click += InsertSetAbilityMenuItem_Click;

            ToolStripMenuItem insertTargetResourceItem = new ToolStripMenuItem("Target Resource");
            insertTargetResourceItem.Click += InsertTargetResourceMenuItem_Click;

            ToolStripMenuItem insertBandageItem = new ToolStripMenuItem("Bandage");
            insertBandageItem.Click += InsertBandageMenuItem_Click;

            ToolStripMenuItem insertMovementActionItem = new ToolStripMenuItem("Movement");
            insertMovementActionItem.Click += InsertMovementActionMenuItem_Click;

            ToolStripMenuItem insertMessagingItem = new ToolStripMenuItem("Messaging");
            insertMessagingItem.Click += InsertMessagingMenuItem_Click;

            ToolStripMenuItem insertActionsMenuItem = new ToolStripMenuItem("Actions");

            ToolStripMenuItem insertMoveItemActionItem = new ToolStripMenuItem("Move Item");
            insertMoveItemActionItem.Click += InsertMoveItemActionMenuItem_Click;

            ToolStripMenuItem insertDoubleClickItem = new ToolStripMenuItem("Double Click");
            insertDoubleClickItem.Click += InsertDoubleClickMenuItem_Click;

            ToolStripMenuItem insertTargetItem = new ToolStripMenuItem("Target");
            insertTargetItem.Click += InsertTargetMenuItem_Click;

            ToolStripMenuItem insertAttackEntityItem = new ToolStripMenuItem("Attack");
            insertAttackEntityItem.Click += InsertAttackEntityMenuItem_Click;

            ToolStripMenuItem insertCastSpellItem = new ToolStripMenuItem("Cast Spell");
            insertCastSpellItem.Click += InsertCastSpellMenuItem_Click;

            ToolStripMenuItem insertUseSkillItem = new ToolStripMenuItem("Use Skill");
            insertUseSkillItem.Click += InsertUseSkillMenuItem_Click;

            ToolStripMenuItem insertUsePotionItem = new ToolStripMenuItem("Use Potion");
            insertUsePotionItem.Click += InsertUsePotionMenuItem_Click;

            ToolStripMenuItem insertInvokeVirtueItem = new ToolStripMenuItem("Invoke Virtue");
            insertInvokeVirtueItem.Click += InsertInvokeVirtueMenuItem_Click;

            ToolStripMenuItem insertRunOrganizerOnceItem = new ToolStripMenuItem("Run Organizer Once");
            insertRunOrganizerOnceItem.Click += InsertRunOrganizerOnceMenuItem_Click;

            ToolStripMenuItem insertToggleWarModeItem = new ToolStripMenuItem("Toggle War Mode");
            insertToggleWarModeItem.Click += InsertToggleWarModeMenuItem_Click;

            ToolStripMenuItem insertMountItem = new ToolStripMenuItem("Mount");
            insertMountItem.Click += InsertMountMenuItem_Click;

            ToolStripMenuItem insertFlyItem = new ToolStripMenuItem("Fly");
            insertFlyItem.Click += InsertFlyMenuItem_Click;

            ToolStripMenuItem insertUseEmoteItem = new ToolStripMenuItem("Use Emote");
            insertUseEmoteItem.Click += InsertUseEmoteMenuItem_Click;

            ToolStripMenuItem insertArmDisarmItem = new ToolStripMenuItem("Arm/Disarm");
            insertArmDisarmItem.Click += InsertArmDisarmMenuItem_Click;

            ToolStripMenuItem insertRenameMobileItem = new ToolStripMenuItem("Rename Mobile");
            insertRenameMobileItem.Click += InsertRenameMobileMenuItem_Click;

            ToolStripMenuItem insertPromptResponseItem = new ToolStripMenuItem("Prompt Response");
            insertPromptResponseItem.Click += InsertPromptResponseMenuItem_Click;

            ToolStripMenuItem insertGumpResponseItem = new ToolStripMenuItem("Gump Response");
            insertGumpResponseItem.Click += InsertGumpResponseMenuItem_Click;

            ToolStripMenuItem insertDropItem = new ToolStripMenuItem("Drop");
            insertDropItem.Click += InsertDropMenuItem_Click;

            insertActionsMenuItem.DropDownItems.Add(insertMessagingItem);
            insertActionsMenuItem.DropDownItems.Add(insertMovementActionItem);
            insertActionsMenuItem.DropDownItems.Add(insertDoubleClickItem);
            insertActionsMenuItem.DropDownItems.Add(insertTargetItem);
            insertActionsMenuItem.DropDownItems.Add(insertAttackEntityItem);
            insertActionsMenuItem.DropDownItems.Add(insertMoveItemActionItem);
            insertActionsMenuItem.DropDownItems.Add(insertCastSpellItem);
            insertActionsMenuItem.DropDownItems.Add(insertUseSkillItem);
            insertActionsMenuItem.DropDownItems.Add(insertUsePotionItem);
            insertActionsMenuItem.DropDownItems.Add(insertGumpResponseItem);
            insertActionsMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertActionsMenuItem.DropDownItems.Add(insertTargetResourceItem);
            insertActionsMenuItem.DropDownItems.Add(insertBandageItem);
            insertActionsMenuItem.DropDownItems.Add(insertDropItem);
            insertActionsMenuItem.DropDownItems.Add(insertToggleWarModeItem);
            insertActionsMenuItem.DropDownItems.Add(insertMountItem);
            insertActionsMenuItem.DropDownItems.Add(insertFlyItem);
            insertActionsMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertActionsMenuItem.DropDownItems.Add(insertSetAbilityItem);
            insertActionsMenuItem.DropDownItems.Add(insertInvokeVirtueItem);
            insertActionsMenuItem.DropDownItems.Add(insertRunOrganizerOnceItem);
            insertActionsMenuItem.DropDownItems.Add(insertUseEmoteItem);
            insertActionsMenuItem.DropDownItems.Add(insertArmDisarmItem);
            insertActionsMenuItem.DropDownItems.Add(insertRenameMobileItem);
            insertActionsMenuItem.DropDownItems.Add(insertPromptResponseItem);

            ToolStripMenuItem insertControlMenuItem = new ToolStripMenuItem("Control");

            ToolStripMenuItem insertPauseItem = new ToolStripMenuItem("Pause");
            insertPauseItem.Click += InsertPauseMenuItem_Click;

            ToolStripMenuItem insertResyncItem = new ToolStripMenuItem("Resync");
            insertResyncItem.Click += InsertResyncMenuItem_Click;

            ToolStripMenuItem insertCommentItem = new ToolStripMenuItem("Comment");
            insertCommentItem.Click += InsertCommentMenuItem_Click;

            ToolStripMenuItem insertUseContextMenuItem = new ToolStripMenuItem("Use Context Menu");
            insertUseContextMenuItem.Click += InsertUseContextMenuMenuItem_Click;

            ToolStripMenuItem insertClearJournalItem = new ToolStripMenuItem("Clear Journal");
            insertClearJournalItem.Click += InsertClearJournalMenuItem_Click;

            ToolStripMenuItem insertWaitForTargetItem = new ToolStripMenuItem("Wait For Target");
            insertWaitForTargetItem.Click += InsertWaitForTargetMenuItem_Click;

            ToolStripMenuItem insertWaitForGumpItem = new ToolStripMenuItem("Wait For Gump");
            insertWaitForGumpItem.Click += InsertWaitForGumpMenuItem_Click;

            ToolStripMenuItem insertDisconnectItem = new ToolStripMenuItem("Disconnect");
            insertDisconnectItem.Click += InsertDisconnectMenuItem_Click;

            insertControlMenuItem.DropDownItems.Add(insertPauseItem);
            insertControlMenuItem.DropDownItems.Add(insertResyncItem);
            insertControlMenuItem.DropDownItems.Add(insertCommentItem);
            insertControlMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertControlMenuItem.DropDownItems.Add(insertUseContextMenuItem);
            insertControlMenuItem.DropDownItems.Add(insertClearJournalItem);
            insertControlMenuItem.DropDownItems.Add(insertWaitForTargetItem);
            insertControlMenuItem.DropDownItems.Add(insertWaitForGumpItem);
            insertControlMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertControlMenuItem.DropDownItems.Add(insertDisconnectItem);

            ToolStripMenuItem insertMenuItem = new ToolStripMenuItem("Insert");

            ToolStripMenuItem saveMacroItem = new ToolStripMenuItem("Save Macro");
            saveMacroItem.Click += SaveMacroMenuItem_Click;

            ToolStripMenuItem moveUpItem = new ToolStripMenuItem("Move Up");
            moveUpItem.Click += MoveUpMenuItem_Click;

            ToolStripMenuItem moveDownItem = new ToolStripMenuItem("Move Down");
            moveDownItem.Click += MoveDownMenuItem_Click;

            ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy");
            copyItem.Click += CopyMenuItem_Click;

            ToolStripMenuItem pasteItem = new ToolStripMenuItem("Paste After");
            pasteItem.Click += PasteMenuItem_Click;

            ToolStripMenuItem insertIfItem = new ToolStripMenuItem("If Condition");
            insertIfItem.Click += InsertIfMenuItem_Click;

            ToolStripMenuItem insertElseIfItem = new ToolStripMenuItem("ElseIf Condition");
            insertElseIfItem.Click += InsertElseIfMenuItem_Click;

            ToolStripMenuItem insertElseItem = new ToolStripMenuItem("Else");
            insertElseItem.Click += InsertElseMenuItem_Click;

            ToolStripMenuItem insertEndIfItem = new ToolStripMenuItem("EndIf");
            insertEndIfItem.Click += InsertEndIfMenuItem_Click;

            ToolStripMenuItem insertWhileItem = new ToolStripMenuItem("While");
            insertWhileItem.Click += InsertWhileMenuItem_Click;

            ToolStripMenuItem insertEndWhileItem = new ToolStripMenuItem("EndWhile");
            insertEndWhileItem.Click += InsertEndWhileMenuItem_Click;

            ToolStripMenuItem insertForItem = new ToolStripMenuItem("For Loop");
            insertForItem.Click += InsertForMenuItem_Click;

            ToolStripMenuItem insertEndForItem = new ToolStripMenuItem("EndFor");
            insertEndForItem.Click += InsertEndForMenuItem_Click;

            ToolStripMenuItem insertSetAliasItem = new ToolStripMenuItem("Set Alias");
            insertSetAliasItem.Click += InsertSetAliasMenuItem_Click;

            ToolStripMenuItem insertRemoveAliasItem = new ToolStripMenuItem("Remove Alias");
            insertRemoveAliasItem.Click += InsertRemoveAliasMenuItem_Click;

            insertMenuItem.DropDownItems.Add(insertActionsMenuItem);
            insertMenuItem.DropDownItems.Add(insertControlMenuItem);
            insertMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertMenuItem.DropDownItems.Add(insertIfItem);
            insertMenuItem.DropDownItems.Add(insertElseIfItem);
            insertMenuItem.DropDownItems.Add(insertElseItem);
            insertMenuItem.DropDownItems.Add(insertEndIfItem);
            insertMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertMenuItem.DropDownItems.Add(insertWhileItem);
            insertMenuItem.DropDownItems.Add(insertEndWhileItem);
            insertMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertMenuItem.DropDownItems.Add(insertForItem);
            insertMenuItem.DropDownItems.Add(insertEndForItem);
            insertMenuItem.DropDownItems.Add(new ToolStripSeparator());
            insertMenuItem.DropDownItems.Add(insertSetAliasItem);
            insertMenuItem.DropDownItems.Add(insertRemoveAliasItem);


            ToolStripMenuItem removeMenuItem = new ToolStripMenuItem("Remove");

            ToolStripMenuItem removeLinesItem = new ToolStripMenuItem("Line(s)");
            removeLinesItem.Click += RemoveActionMenuItem_Click;

            ToolStripMenuItem removeAllItem = new ToolStripMenuItem("All");
            removeAllItem.Click += RemoveAllActionsMenuItem_Click;

            removeMenuItem.DropDownItems.Add(removeLinesItem);
            removeMenuItem.DropDownItems.Add(removeAllItem);

            ToolStripMenuItem editMoveItemActionItem = new ToolStripMenuItem("Edit Move Item");
            editMoveItemActionItem.Click += EditMoveItemActionMenuItem_Click;

            ToolStripMenuItem editPromptResponseItem = new ToolStripMenuItem("Edit Prompt Response");
            editPromptResponseItem.Click += EditPromptResponseMenuItem_Click;

            ToolStripMenuItem editSetAbilityActionItem = new ToolStripMenuItem("Edit Set Ability");
            editSetAbilityActionItem.Click += EditSetAbilityActionMenuItem_Click;

            ToolStripMenuItem editBandageActionItem = new ToolStripMenuItem("Edit Bandage");
            editBandageActionItem.Click += EditBandageActionMenuItem_Click;

            ToolStripMenuItem editMovementActionItem = new ToolStripMenuItem("Edit Movement");
            editMovementActionItem.Click += EditMovementActionMenuItem_Click;

            ToolStripMenuItem editMessagingItem = new ToolStripMenuItem("Edit Messaging");
            editMessagingItem.Click += EditMessagingMenuItem_Click;

            ToolStripMenuItem editUseContextMenuItem = new ToolStripMenuItem("Edit Use Context Menu");
            editUseContextMenuItem.Click += EditUseContextMenuMenuItem_Click;

            ToolStripMenuItem editArmDisarmItem = new ToolStripMenuItem("Edit Arm/Disarm");
            editArmDisarmItem.Click += EditArmDisarmMenuItem_Click;

            ToolStripMenuItem editDoubleClickItem = new ToolStripMenuItem("Edit Double Click");
            editDoubleClickItem.Click += EditDoubleClickMenuItem_Click;

            ToolStripMenuItem editTargetItem = new ToolStripMenuItem("Edit Target");
            editTargetItem.Click += EditTargetMenuItem_Click;

            ToolStripMenuItem editRunOrganizerOnceItem = new ToolStripMenuItem("Edit Run Organizer Once");
            editRunOrganizerOnceItem.Click += EditRunOrganizerOnceMenuItem_Click;

            ToolStripMenuItem editMountItem = new ToolStripMenuItem("Edit Mount");
            editMountItem.Click += EditMountMenuItem_Click;

            ToolStripMenuItem editAttackEntityItem = new ToolStripMenuItem("Edit Attack");
            editAttackEntityItem.Click += EditAttackEntityMenuItem_Click;

            ToolStripMenuItem editPauseItem = new ToolStripMenuItem("Edit Pause");
            editPauseItem.Click += EditPauseMenuItem_Click;

            ToolStripMenuItem editForItem = new ToolStripMenuItem("Edit For Loop");
            editForItem.Click += EditForMenuItem_Click;

            ToolStripMenuItem editCommentItem = new ToolStripMenuItem("Edit Comment");
            editCommentItem.Click += EditCommentMenuItem_Click;

            ToolStripMenuItem editIfItem = new ToolStripMenuItem("Edit If Condition");
            editIfItem.Click += EditIfMenuItem_Click;

            ToolStripMenuItem editElseIfItem = new ToolStripMenuItem("Edit ElseIf Condition");
            editElseIfItem.Click += EditElseIfMenuItem_Click;

            ToolStripMenuItem editWhileItem = new ToolStripMenuItem("Edit While Condition");
            editWhileItem.Click += EditWhileMenuItem_Click;

            ToolStripMenuItem editInvokeVirtueItem = new ToolStripMenuItem("Edit Invoke Virtue");
            editInvokeVirtueItem.Click += EditInvokeVirtueMenuItem_Click;

            ToolStripMenuItem editUseEmoteItem = new ToolStripMenuItem("Edit Use Emote");
            editUseEmoteItem.Click += EditUseEmoteMenuItem_Click;

            ToolStripMenuItem editWaitForTargetItem = new ToolStripMenuItem("Edit Wait For Target");
            editWaitForTargetItem.Click += EditWaitForTargetMenuItem_Click;

            ToolStripMenuItem editUsePotionItem = new ToolStripMenuItem("Edit Use Potion");
            editUsePotionItem.Click += EditUsePotionMenuItem_Click;

            ToolStripMenuItem editUseSkillItem = new ToolStripMenuItem("Edit Use Skill");
            editUseSkillItem.Click += EditUseSkillMenuItem_Click;

            ToolStripMenuItem editCastSpellItem = new ToolStripMenuItem("Edit Cast Spell");
            editCastSpellItem.Click += EditCastSpellMenuItem_Click;

            ToolStripMenuItem editToggleWarModeItem = new ToolStripMenuItem("Edit Toggle War Mode");
            editToggleWarModeItem.Click += EditToggleWarModeMenuItem_Click;

            ToolStripMenuItem editFlyItem = new ToolStripMenuItem("Edit Fly");
            editFlyItem.Click += EditFlyMenuItem_Click;

            ToolStripMenuItem editSetAliasItem = new ToolStripMenuItem("Edit Set Alias");
            editSetAliasItem.Click += EditSetAliasMenuItem_Click;

            ToolStripMenuItem editRemoveAliasItem = new ToolStripMenuItem("Edit Remove Alias");
            editRemoveAliasItem.Click += EditRemoveAliasMenuItem_Click;

            ToolStripMenuItem editRenameMobileItem = new ToolStripMenuItem("Edit Rename Mobile");
            editRenameMobileItem.Click += EditRenameMobileMenuItem_Click;

            ToolStripMenuItem editTargetResourceActionItem = new ToolStripMenuItem("Edit Target Resource");
            editTargetResourceActionItem.Click += EditTargetResourceActionMenuItem_Click;

            ToolStripMenuItem editWaitForGumpItem = new ToolStripMenuItem("Edit Wait For Gump");
            editWaitForGumpItem.Click += EditWaitForGumpMenuItem_Click;

            ToolStripMenuItem editGumpResponseItem = new ToolStripMenuItem("Edit Gump Response");
            editGumpResponseItem.Click += EditGumpResponseMenuItem_Click;

            ToolStripMenuItem editDropItem = new ToolStripMenuItem("Edit Drop");
            editDropItem.Click += EditDropMenuItem_Click;

            ToolStripMenuItem startRecordingFromHereItem = new ToolStripMenuItem("Start Recording From Here");
            startRecordingFromHereItem.Click += (s, e) => StartRecordingFromHereMenuItem_Click(s, e);


            contextMenu.Items.Insert(0, startRecordingFromHereItem);
            contextMenu.Items.Insert(1, new ToolStripSeparator());
            contextMenu.Items.Add(editDropItem);
            contextMenu.Items.Add(editMoveItemActionItem);
            contextMenu.Items.Add(editGumpResponseItem);
            contextMenu.Items.Add(editWaitForGumpItem);
            contextMenu.Items.Add(editPromptResponseItem);
            contextMenu.Items.Add(editSetAbilityActionItem);
            contextMenu.Items.Add(editTargetResourceActionItem);
            contextMenu.Items.Add(editBandageActionItem);
            contextMenu.Items.Add(editMessagingItem);
            contextMenu.Items.Add(editMovementActionItem);
            contextMenu.Items.Add(editWhileItem);
            contextMenu.Items.Add(editRenameMobileItem);
            contextMenu.Items.Add(editUseContextMenuItem);
            contextMenu.Items.Add(editDoubleClickItem);
            contextMenu.Items.Add(editTargetItem);
            contextMenu.Items.Add(editPauseItem);
            contextMenu.Items.Add(editCommentItem);
            contextMenu.Items.Add(editIfItem);
            contextMenu.Items.Add(editElseIfItem);
            contextMenu.Items.Add(editForItem);
            contextMenu.Items.Add(editRunOrganizerOnceItem);
            contextMenu.Items.Add(editMountItem);
            contextMenu.Items.Add(editInvokeVirtueItem);
            contextMenu.Items.Add(editToggleWarModeItem);
            contextMenu.Items.Add(editFlyItem);
            contextMenu.Items.Add(editUseEmoteItem);
            contextMenu.Items.Add(editWaitForTargetItem);
            contextMenu.Items.Add(editUsePotionItem);
            contextMenu.Items.Add(editUseSkillItem);
            contextMenu.Items.Add(editCastSpellItem);
            contextMenu.Items.Add(editSetAliasItem);
            contextMenu.Items.Add(editRemoveAliasItem);
            contextMenu.Items.Add(editAttackEntityItem);
            contextMenu.Items.Add(editArmDisarmItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(moveUpItem);
            contextMenu.Items.Add(moveDownItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(copyItem);
            contextMenu.Items.Add(pasteItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(insertMenuItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(removeMenuItem);
            contextMenu.Items.Add(saveMacroItem);

            // Enable/disable options based on selection
            contextMenu.Opening += (s, e) =>
            {
                bool singleSelection = macroActionsListView.SelectedIndices.Count == 1;
                bool hasSelection = macroActionsListView.SelectedIndices.Count > 0;
                bool hasMacroSelected = macroListBox.SelectedIndex >= 0;
                startRecordingFromHereItem.Enabled = singleSelection && hasMacroSelected;

                editDropItem.Visible = singleSelection && IsSelectedActionDrop();
                editMoveItemActionItem.Visible = singleSelection && IsSelectedActionMoveItem();
                editGumpResponseItem.Visible = singleSelection && IsSelectedActionGumpResponse();
                editWaitForGumpItem.Visible = singleSelection && IsSelectedActionWaitForGump();
                editPromptResponseItem.Visible = singleSelection && IsSelectedActionPromptResponse();
                editSetAbilityActionItem.Visible = singleSelection && IsSelectedActionSetAbility();
                editTargetResourceActionItem.Visible = singleSelection && IsSelectedActionTargetResource();
                editBandageActionItem.Visible = singleSelection && IsSelectedActionBandage();
                editMovementActionItem.Visible = singleSelection && IsSelectedActionMovement();
                editMessagingItem.Visible = singleSelection && IsSelectedActionMessaging();
                editWhileItem.Visible = singleSelection && IsSelectedActionWhile();
                editRenameMobileItem.Visible = singleSelection && IsSelectedActionRenameMobile();
                editUseContextMenuItem.Visible = singleSelection && IsSelectedActionUseContextMenu();
                editArmDisarmItem.Visible = singleSelection && IsSelectedActionArmDisarm();
                editDoubleClickItem.Visible = singleSelection && IsSelectedActionDoubleClick();
                editTargetItem.Visible = singleSelection && IsSelectedActionTarget();
                editRunOrganizerOnceItem.Visible = singleSelection && IsSelectedActionRunOrganizerOnce();
                editPauseItem.Visible = singleSelection && IsSelectedActionPause();
                editCommentItem.Visible = singleSelection && IsSelectedActionComment();
                editIfItem.Visible = singleSelection && IsSelectedActionIf();
                editElseIfItem.Visible = singleSelection && IsSelectedActionElseIf();
                editForItem.Visible = singleSelection && IsSelectedActionFor();
                editInvokeVirtueItem.Visible = singleSelection && IsSelectedActionInvokeVirtue();
                editToggleWarModeItem.Visible = singleSelection && IsSelectedActionToggleWarMode();
                editFlyItem.Visible = singleSelection && IsSelectedActionFly();
                editUseEmoteItem.Visible = singleSelection && IsSelectedActionUseEmote();
                editWaitForTargetItem.Visible = singleSelection && IsSelectedActionWaitForTarget();
                editUsePotionItem.Visible = singleSelection && IsSelectedActionUsePotion();
                editUseSkillItem.Visible = singleSelection && IsSelectedActionUseSkill();
                editCastSpellItem.Visible = singleSelection && IsSelectedActionCastSpell();
                editSetAliasItem.Visible = singleSelection && IsSelectedActionSetAlias();
                editRemoveAliasItem.Visible = singleSelection && IsSelectedActionRemoveAlias();
                editAttackEntityItem.Visible = singleSelection && IsSelectedActionAttackEntity();
                editMountItem.Visible = singleSelection && IsSelectedActionMount();

                copyItem.Enabled = singleSelection;
                pasteItem.Enabled = singleSelection && m_CopiedAction != null;
                insertMenuItem.Enabled = saveMacroItem.Enabled = hasMacroSelected;

                if (singleSelection && hasMacroSelected)
                {
                    var macros = MacroManager.GetMacros();
                    if (macroListBox.SelectedIndex < macros.Count)
                    {
                        var macro = macros[macroListBox.SelectedIndex];
                        int actionIndex = macroActionsListView.SelectedIndices[0];

                        moveUpItem.Enabled = actionIndex > 0;
                        moveDownItem.Enabled = actionIndex < macro.Actions.Count - 1;
                    }
                    else
                    {
                        moveUpItem.Enabled = false;
                        moveDownItem.Enabled = false;
                    }
                }
                else
                {
                    moveUpItem.Enabled = false;
                    moveDownItem.Enabled = false;
                }

                removeLinesItem.Enabled = hasSelection;

                if (hasMacroSelected)
                {
                    var macros = MacroManager.GetMacros();
                    if (macroListBox.SelectedIndex < macros.Count)
                    {
                        var macro = macros[macroListBox.SelectedIndex];
                        removeAllItem.Enabled = macro.Actions.Count > 0;
                    }
                    else
                    {
                        removeAllItem.Enabled = false;
                    }
                }
                else
                {
                    removeAllItem.Enabled = false;
                }

                removeMenuItem.Enabled = removeLinesItem.Enabled || removeAllItem.Enabled;
            };

            macroActionsListView.ContextMenuStrip = contextMenu;
        }

        private int GetInsertPosition()
        {
            // If no actions exist, insert at beginning
            if (macroActionsListView.Items.Count == 0)
                return 0;

            // If an action is selected, insert after it
            if (macroActionsListView.SelectedIndices.Count == 1)
                return macroActionsListView.SelectedIndices[0] + 1;

            // If multiple selected or none selected (but actions exist), insert at end
            return macroActionsListView.Items.Count;
        }
        private void DisplayMacroActions(Macro macro)
        {
            macroActionsListView.Items.Clear();
            int actionNum = 1;
            int indentLevel = 0;

            foreach (var action in macro.Actions)
            {
                // Decrease indent for ElseIf, Else, EndIf, and EndFor before displaying
                if (action is RazorEnhanced.Macros.Actions.ElseIfAction ||
                    action is RazorEnhanced.Macros.Actions.ElseAction ||
                    action is RazorEnhanced.Macros.Actions.EndIfAction ||
                    action is RazorEnhanced.Macros.Actions.EndForAction)
                {
                    indentLevel = Math.Max(0, indentLevel - 1);
                }

                string indent = new string(' ', indentLevel * 4);
                var item = new ListViewItem($"{actionNum}. {indent}{action.GetActionName()}");

                // Special formatting for each action type
                if (action is RazorEnhanced.Macros.Actions.ArmDisarmAction armDisarmAction)
                {
                    string handDisplay = armDisarmAction.Hand == "Both" ? "Both" : armDisarmAction.Hand == "Left" ? "Left Hand" : "Right Hand";
                    if (armDisarmAction.Mode == "Arm")
                        item.SubItems.Add($"Arm {handDisplay}: 0x{armDisarmAction.ItemSerial:X8}");
                    else
                        item.SubItems.Add($"Disarm {handDisplay}");
                    item.ForeColor = armDisarmAction.Mode == "Arm" ? Color.DarkGreen : Color.DarkRed;
                }
                else if (action is RazorEnhanced.Macros.Actions.AttackAction attackEntityAction)
                {
                    string display = "";
                    switch (attackEntityAction.Mode)
                    {
                        case AttackAction.AttackMode.LastTarget:
                            display = "Last Target";
                            break;
                        case AttackAction.AttackMode.Serial:
                            display = $"Serial 0x{attackEntityAction.Serial:X8}";
                            break;
                        case AttackAction.AttackMode.Alias:
                            display = $"Alias '{attackEntityAction.AliasName}'";
                            break;
                        case AttackAction.AttackMode.Nearest:
                            string notorietyStr = GetNotorietyDisplayName(attackEntityAction.Notoriety);
                            string rangeDisplay = attackEntityAction.Range == -1 ? "Any" : attackEntityAction.Range.ToString();
                            display = $"Nearest {notorietyStr} (Range: {rangeDisplay})";
                            break;
                        case AttackAction.AttackMode.Farthest:
                            string notorietyStr2 = GetNotorietyDisplayName(attackEntityAction.Notoriety);
                            string rangeDisplay2 = attackEntityAction.Range == -1 ? "Any" : attackEntityAction.Range.ToString();
                            display = $"Farthest {notorietyStr2} (Range: {rangeDisplay2})";
                            break;
                        case AttackAction.AttackMode.ByType:
                            string colorStr = attackEntityAction.Color == -1 ? "Any" : $"0x{attackEntityAction.Color:X4}";
                            string rangeDisplay3 = attackEntityAction.Range == -1 ? "Any" : attackEntityAction.Range.ToString();
                            display = $"{attackEntityAction.Selector}: 0x{attackEntityAction.Graphic:X4} ({colorStr}) Range: {rangeDisplay3}";
                            break;
                    }
                    item.SubItems.Add(display);
                    item.ForeColor = Color.DarkRed;
                }
                else if (action is RazorEnhanced.Macros.Actions.BandageAction bandageAction)
                {
                    string details = "";
                    switch (bandageAction.TargetMode)
                    {
                        case RazorEnhanced.Macros.Actions.BandageAction.BandageTargetMode.Self:
                            details = "Self";
                            break;
                        case RazorEnhanced.Macros.Actions.BandageAction.BandageTargetMode.Serial:
                            details = $"Serial: 0x{bandageAction.TargetSerial:X8}";
                            break;
                        case RazorEnhanced.Macros.Actions.BandageAction.BandageTargetMode.Alias:
                            details = $"Alias: '{bandageAction.TargetAlias}'";
                            break;
                    }
                    item.SubItems.Add(details);
                    item.ForeColor = Color.MediumVioletRed;
                }
                else if (action is RazorEnhanced.Macros.Actions.CastSpellAction castSpellAction)
                {
                    string spellName = RazorEnhanced.Macros.Actions.CastSpellAction.GetSpellNameByID(castSpellAction.SpellID);
                    string target = string.IsNullOrWhiteSpace(castSpellAction.TargetSerialOrAlias) ? "" : $" → Target: {castSpellAction.TargetSerialOrAlias}";
                    item.SubItems.Add($"{spellName} (ID: {castSpellAction.SpellID}){target}");
                    item.ForeColor = Color.MediumBlue;
                }
                else if (action is RazorEnhanced.Macros.Actions.ClearJournalAction)
                {
                    item.SubItems.Add("Clear journal buffer");
                    item.ForeColor = Color.SlateBlue;
                }
                else if (action is RazorEnhanced.Macros.Actions.CommentAction commentAction)
                {
                    item.SubItems.Add($"// {commentAction.Comment}");
                    item.ForeColor = Color.Green;
                }
                else if (action is RazorEnhanced.Macros.Actions.DisconnectAction)
                {
                    item.SubItems.Add("Disconnect from Server");
                    item.ForeColor = Color.DarkRed;
                }
                else if (action is RazorEnhanced.Macros.Actions.DoubleClickAction doubleClickAction)
                {
                    string display = "";
                    switch (doubleClickAction.Mode)
                    {
                        case DoubleClickAction.DoubleClickMode.Self:
                            display = "Self (Player)";
                            break;
                        case DoubleClickAction.DoubleClickMode.LastTarget:
                            display = "Last Target";
                            break;
                        case DoubleClickAction.DoubleClickMode.Serial:
                            display = $"Serial 0x{doubleClickAction.Serial:X8}";
                            break;
                        case DoubleClickAction.DoubleClickMode.Type:
                            string colorStr = doubleClickAction.Color == -1 ? "Any" : $"0x{doubleClickAction.Color:X4}";
                            display = $"{doubleClickAction.Selector}: 0x{doubleClickAction.Graphic:X4} ({colorStr})";
                            break;
                        case DoubleClickAction.DoubleClickMode.Alias:
                            display = $"Alias '{doubleClickAction.AliasName}'";
                            break;
                    }
                    item.SubItems.Add(display);
                    item.ForeColor = Color.DarkOliveGreen;
                }
                else if (action is RazorEnhanced.Macros.Actions.DropAction dropAction)
                {
                    string destDisplay = dropAction.Container == unchecked((int)0xFFFFFFFF)
                        ? "Ground"
                        : $"0x{dropAction.Container:X8}";
                    item.SubItems.Add($"Item: 0x{dropAction.Serial:X8} → {destDisplay}");
                    item.ForeColor = Color.SaddleBrown;
                }
                else if (action is RazorEnhanced.Macros.Actions.FlyAction flyAction)
                {
                    string modeText = flyAction.Flying ? "ON (Flying)" : "OFF (Ground)";
                    item.SubItems.Add($"Flying: {modeText}");
                    item.ForeColor = flyAction.Flying ? Color.DarkCyan : Color.Brown;
                }
                else if (action is RazorEnhanced.Macros.Actions.GumpResponseAction gumpResponseAction)
                {
                    string gumpIdDisplay = gumpResponseAction.GumpID == 0 ? "Any" : gumpResponseAction.GumpID.ToString();
                    string buttonIdDisplay = gumpResponseAction.ButtonID.ToString();
                    string switchesDisplay = gumpResponseAction.Switches != null && gumpResponseAction.Switches.Count > 0
                        ? string.Join(",", gumpResponseAction.Switches)
                        : "-";
                    string textIdsDisplay = gumpResponseAction.TextIDs != null && gumpResponseAction.TextIDs.Count > 0
                        ? string.Join(",", gumpResponseAction.TextIDs)
                        : "-";
                    string textEntriesDisplay = gumpResponseAction.TextEntries != null && gumpResponseAction.TextEntries.Count > 0
                        ? string.Join(" | ", gumpResponseAction.TextEntries)
                        : "-";

                    item.SubItems.Add(
                        $"GumpID: {gumpIdDisplay}, Button: {buttonIdDisplay}, Switches: {switchesDisplay}, TextIDs: {textIdsDisplay}, Text: {textEntriesDisplay}"
                    );
                    item.ForeColor = Color.DarkSeaGreen;
                }
                else if (action is RazorEnhanced.Macros.Actions.InvokeVirtueAction invokeVirtueAction)
                {
                    item.SubItems.Add($"Virtue: {invokeVirtueAction.VirtueName}");
                    item.ForeColor = Color.Gold;
                }
                else if (action is RazorEnhanced.Macros.Actions.MessagingAction messagingAction)
                {
                    string details = $"{messagingAction.Type} \"{messagingAction.Message}\" (Hue: {messagingAction.Hue})";
                    // Show target for Overhead type if set
                    if (messagingAction.Type == MessagingAction.MessageType.Overhead && !string.IsNullOrWhiteSpace(messagingAction.TargetSerialOrAlias))
                        details += $" → Target: {messagingAction.TargetSerialOrAlias}";
                    item.SubItems.Add(details);
                    item.ForeColor = Color.MediumVioletRed;
                }
                else if (action is RazorEnhanced.Macros.Actions.MountAction mountAction)
                {
                    if (mountAction.ShouldMount)
                    {
                        string serialDisplay = mountAction.MountSerial == 0 ? "Last Mount" : $"0x{mountAction.MountSerial:X8}";
                        item.SubItems.Add($"Mount: {serialDisplay}");
                    }
                    else
                    {
                        item.SubItems.Add("Dismount");
                    }
                    item.ForeColor = Color.SaddleBrown;
                }
                else if (action is RazorEnhanced.Macros.Actions.MoveItemAction moveItemAction)
                {
                    string details;
                    if (moveItemAction.TargetType == MoveItemAction.MoveTargetType.Entity)
                    {
                        details = $"Item: {moveItemAction.ItemSerialOrAlias} → Target: {moveItemAction.TargetSerialOrAlias}, Amount: {moveItemAction.Amount}";
                        if (moveItemAction.X > 0 || moveItemAction.Y > 0)
                            details += $" @ ({moveItemAction.X},{moveItemAction.Y})";
                    }
                    else
                    {
                        details = $"Item: {moveItemAction.ItemSerialOrAlias} → Ground ({moveItemAction.X},{moveItemAction.Y},{moveItemAction.Z}), Amount: {moveItemAction.Amount}";
                    }
                    item.SubItems.Add(details);
                    item.ForeColor = Color.SaddleBrown;
                }
                else if (action is RazorEnhanced.Macros.Actions.MovementAction movementAction)
                {
                    string details = "";
                    switch (movementAction.Type)
                    {
                        case MovementAction.MovementType.Walk:
                        case MovementAction.MovementType.Run:
                            details = $"{movementAction.Type} {movementAction.Direction}";
                            break;
                        case MovementAction.MovementType.Pathfind:
                            switch (movementAction.Mode)
                            {
                                case MovementAction.PathfindMode.Coordinates:
                                    details = $"Pathfind to ({movementAction.X}, {movementAction.Y}, {movementAction.Z})";
                                    break;
                                case MovementAction.PathfindMode.Serial:
                                    details = $"Pathfind to Serial: 0x{movementAction.Serial:X8}";
                                    break;
                                case MovementAction.PathfindMode.Alias:
                                    details = $"Pathfind to Alias: '{movementAction.AliasName}'";
                                    break;
                            }
                            break;
                    }
                    item.SubItems.Add(details);
                    item.ForeColor = Color.DarkSlateBlue;
                }
                else if (action is RazorEnhanced.Macros.Actions.PauseAction pauseAction)
                {
                    item.SubItems.Add($"{pauseAction.Milliseconds}ms");
                    item.ForeColor = Color.DarkGray;
                }
                else if (action is RazorEnhanced.Macros.Actions.PickUpAction pickUpAction)
                {
                    item.SubItems.Add($"Serial: 0x{pickUpAction.Serial:X8}, Amount: {pickUpAction.Amount}");
                    item.ForeColor = Color.SaddleBrown;
                }
                else if (action is RazorEnhanced.Macros.Actions.PromptResponseAction promptResponseAction)
                {
                    item.SubItems.Add($"Response: \"{promptResponseAction.Response}\"");
                    item.ForeColor = Color.DarkSeaGreen;
                }
                else if (action is RazorEnhanced.Macros.Actions.QueryStringResponseAction queryStringResponseAction)
                {
                    string acceptText = queryStringResponseAction.Accept ? "Accept" : "Decline";
                    item.SubItems.Add($"{acceptText}: \"{queryStringResponseAction.Response}\"");
                    item.ForeColor = Color.DarkSeaGreen;
                }
                else if (action is RazorEnhanced.Macros.Actions.RemoveAliasAction removeAliasAction)
                {
                    item.SubItems.Add($"Remove '{removeAliasAction.AliasName}'");
                    item.ForeColor = Color.DarkRed;
                }
                else if (action is RazorEnhanced.Macros.Actions.RenameMobileAction renameMobileAction)
                {
                    item.SubItems.Add($"Serial: 0x{renameMobileAction.Serial:X8}, Name: \"{renameMobileAction.Name}\"");
                    item.ForeColor = Color.DarkTurquoise;
                }
                else if (action is RazorEnhanced.Macros.Actions.ResyncAction)
                {
                    item.SubItems.Add("Resynchronize client");
                    item.ForeColor = Color.DarkGreen;
                }
                else if (action is RazorEnhanced.Macros.Actions.RunOrganizerOnceAction runOrganizerAction)
                {
                    string bagInfo = "";
                    if (runOrganizerAction.SourceBag != -1 || runOrganizerAction.DestinationBag != -1)
                    {
                        string source = runOrganizerAction.SourceBag == -1 ? "default" : $"0x{runOrganizerAction.SourceBag:X8}";
                        string dest = runOrganizerAction.DestinationBag == -1 ? "default" : $"0x{runOrganizerAction.DestinationBag:X8}";
                        bagInfo = $" ({source} → {dest})";
                    }
                    item.SubItems.Add($"List: {runOrganizerAction.OrganizerName}{bagInfo}");
                    item.ForeColor = Color.DarkSlateGray;
                }
                else if (action is RazorEnhanced.Macros.Actions.SetAbilityAction setAbilityAction)
                {
                    item.SubItems.Add($"Ability: {setAbilityAction.Ability}");
                    item.ForeColor = Color.DarkGoldenrod;
                }
                else if (action is RazorEnhanced.Macros.Actions.SetAliasAction setAliasAction)
                {
                    string serialDisplay = setAliasAction.UseFoundSerial
                        ? "'findfound'"
                        : (setAliasAction.Serial == 0 ? "0x00000000" : $"0x{setAliasAction.Serial:X8}");
                    item.SubItems.Add($"Set '{setAliasAction.AliasName}' = {serialDisplay}");
                    item.ForeColor = Color.DarkMagenta;
                }
                else if (action is RazorEnhanced.Macros.Actions.TargetAction targetAction)
                {
                    string display = "";
                    switch (targetAction.Mode)
                    {
                        case TargetAction.TargetMode.Self:
                            display = "Target Self";
                            break;
                        case TargetAction.TargetMode.LastTarget:
                            display = "Target Last";
                            break;
                        case TargetAction.TargetMode.Serial:
                            display = $"Serial 0x{targetAction.Serial:X8}";
                            break;
                        case TargetAction.TargetMode.Type:
                            string colorStr = targetAction.Color == -1 ? "Any" : $"0x{targetAction.Color:X4}";
                            display = $"{targetAction.Selector}: 0x{targetAction.Graphic:X4} ({colorStr})";
                            break;
                        case TargetAction.TargetMode.Alias:
                            display = $"Alias '{targetAction.AliasName}'";
                            break;
                        case TargetAction.TargetMode.Location:
                            display = $"Location ({targetAction.X}, {targetAction.Y}, {targetAction.Z})";
                            break;
                    }
                    item.SubItems.Add(display);
                    item.ForeColor = Color.Crimson;
                }
                else if (action is RazorEnhanced.Macros.Actions.ToggleWarModeAction toggleWarModeAction)
                {
                    string modeText = toggleWarModeAction.WarMode ? "ON (War)" : "OFF (Peace)";
                    item.SubItems.Add($"War Mode: {modeText}");
                    item.ForeColor = toggleWarModeAction.WarMode ? Color.Red : Color.Green;
                }
                else if (action is RazorEnhanced.Macros.Actions.UseContextMenuAction useContextMenuAction)
                {
                    string details;
                    if (!string.IsNullOrEmpty(useContextMenuAction.MenuName))
                        details = $"ContextMenu \"{useContextMenuAction.MenuName}\" on {useContextMenuAction.TargetSerialOrAlias}";
                    else
                        details = $"ContextMenu #{useContextMenuAction.MenuIndex} on {useContextMenuAction.TargetSerialOrAlias}";
                    item.SubItems.Add(details);
                    item.ForeColor = Color.DarkViolet;
                }
                else if (action is RazorEnhanced.Macros.Actions.UseEmoteAction useEmoteAction)
                {
                    item.SubItems.Add($"Emote: {useEmoteAction.EmoteName}");
                    item.ForeColor = Color.Orchid;
                }
                else if (action is RazorEnhanced.Macros.Actions.UsePotionAction usePotionAction)
                {
                    item.SubItems.Add($"Type: {usePotionAction.PotionType}");
                    item.ForeColor = Color.MediumPurple;
                }
                else if (action is RazorEnhanced.Macros.Actions.UseSkillAction useSkillAction)
                {
                    string details = $"Skill: {useSkillAction.SkillName}";
                    if (!string.IsNullOrWhiteSpace(useSkillAction.TargetSerialOrAlias))
                        details += $" → Target: {useSkillAction.TargetSerialOrAlias}";
                    item.SubItems.Add(details);
                    item.ForeColor = Color.DarkGoldenrod;
                }
                else if (action is RazorEnhanced.Macros.Actions.WaitForGumpAction waitForGumpAction)
                {
                    string gumpIdDisplay = waitForGumpAction.GumpID == 0 ? "Any" : waitForGumpAction.GumpID.ToString();
                    string timeoutDisplay = $"{waitForGumpAction.Timeout}ms";
                    item.SubItems.Add($"GumpID: {gumpIdDisplay}, Timeout: {timeoutDisplay}");
                    item.ForeColor = Color.DarkKhaki;
                }
                else if (action is RazorEnhanced.Macros.Actions.WaitForTargetAction waitForTargetAction)
                {
                    item.SubItems.Add($"Timeout: {waitForTargetAction.Timeout}ms");
                    item.ForeColor = Color.DarkKhaki;
                }
                else if (action is RazorEnhanced.Macros.Actions.IfAction ifAction)
                {
                    string display = FormatIfConditionDisplay(ifAction);
                    item.SubItems.Add(display);
                    item.ForeColor = Color.Blue;
                }
                else if (action is RazorEnhanced.Macros.Actions.ElseIfAction elseIfAction)
                {
                    string display = FormatElseIfConditionDisplay(elseIfAction);
                    item.SubItems.Add(display);
                    item.ForeColor = Color.DarkCyan;
                }
                else if (action is RazorEnhanced.Macros.Actions.ElseAction)
                {
                    item.SubItems.Add("");
                    item.ForeColor = Color.DarkOrange;
                }
                else if (action is RazorEnhanced.Macros.Actions.EndIfAction)
                {
                    item.SubItems.Add("");
                    item.ForeColor = Color.Blue;
                }
                else if (action is RazorEnhanced.Macros.Actions.WhileAction whileAction)
                {
                    string display = FormatWhileConditionDisplay(whileAction);
                    item.SubItems.Add(display);
                    item.ForeColor = Color.DarkBlue;
                }
                else if (action is RazorEnhanced.Macros.Actions.EndWhileAction)
                {
                    item.SubItems.Add("");
                    item.ForeColor = Color.DarkBlue;
                }
                else if (action is RazorEnhanced.Macros.Actions.ForAction forAction)
                {
                    item.SubItems.Add($"Iterations: {forAction.Iterations}");
                    item.ForeColor = Color.Purple;
                }
                else if (action is RazorEnhanced.Macros.Actions.EndForAction)
                {
                    item.SubItems.Add("");
                    item.ForeColor = Color.Purple;
                }
                else
                {
                    // Generic fallback for any other action types
                    item.SubItems.Add(action.Serialize());
                    item.ForeColor = Color.Black;
                }

                macroActionsListView.Items.Add(item);
                actionNum++;

                // Increase indent after If, ElseIf, Else, or For
                if (action is RazorEnhanced.Macros.Actions.IfAction ||
                    action is RazorEnhanced.Macros.Actions.ElseIfAction ||
                    action is RazorEnhanced.Macros.Actions.ElseAction ||
                    action is RazorEnhanced.Macros.Actions.ForAction)
                {
                    indentLevel++;
                }
            }
        }
        private void RemoveActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count == 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Get selected indices and sort them in reverse order
            // This prevents index shifting issues when removing multiple items
            var selectedIndices = new int[macroActionsListView.SelectedIndices.Count];
            macroActionsListView.SelectedIndices.CopyTo(selectedIndices, 0);
            Array.Sort(selectedIndices);
            Array.Reverse(selectedIndices);

            // Remove actions in reverse order
            foreach (int index in selectedIndices)
            {
                if (index >= 0 && index < macro.Actions.Count)
                {
                    macro.Actions.RemoveAt(index);
                }
            }

            // Refresh the display and save
            DisplayMacroActions(macro);
            MacroManager.SaveMacros(); // You'll need to make this public in MacroManager
        }
        private void RemoveAllActionsMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            if (macro.Actions.Count == 0) return;

            var result = MessageBox.Show(
                $"Remove all {macro.Actions.Count} action(s) from '{macro.Name}'?",
                "Confirm Remove All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                macro.Actions.Clear();
                DisplayMacroActions(macro);
                MacroManager.SaveMacros(); // You'll need to make this public in MacroManager
            }
        }
        private void OnMacrosChanged(object sender, EventArgs e)
        {
            RefreshMacroList();
        }
        private void OnRecordingStateChanged(object sender, EventArgs e)
        {
            UpdateRecordingState();
        }
        private void OnActionRecorded(object sender, EventArgs e)
        {
            RefreshCurrentMacroActions();
        }
        private void RefreshMacroList()
        {
            if (macroListBox.InvokeRequired)
            {
                macroListBox.Invoke(new Action(RefreshMacroList));
                return;
            }

            int selectedIndex = macroListBox.SelectedIndex;
            macroListBox.Items.Clear();

            // Subscribe to macro state changes
            foreach (var macro in MacroManager.GetMacros())
            {
                macroListBox.Items.Add(macro.Name);

                // Unsubscribe first to avoid duplicates
                macro.StateChanged -= OnMacroStateChanged;
                macro.StateChanged += OnMacroStateChanged;
            }

            // Restore selection
            if (selectedIndex >= 0 && selectedIndex < macroListBox.Items.Count)
            {
                macroListBox.SelectedIndex = selectedIndex;
            }
        }
        private void OnMacroStateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs>(OnMacroStateChanged), sender, e);
                return;
            }

            // The sender is the macro that changed state
            var macro = sender as Macro;
            if (macro == null) return;

            // Check if this is the currently selected macro
            if (macroListBox.SelectedIndex >= 0)
            {
                var macros = MacroManager.GetMacros();
                if (macroListBox.SelectedIndex < macros.Count)
                {
                    var selectedMacro = macros[macroListBox.SelectedIndex];

                    if (macro == selectedMacro)
                    {
                        // Update button states based on macro state
                        btnMacroPlay.Enabled = !macro.IsRunning && !MacroManager.IsRecording;
                        btnMacroStop.Enabled = macro.IsRunning;

                        lblMacroStatus.Text = macro.IsRunning ? "Macro running..." : "Ready";
                        lblMacroStatus.ForeColor = macro.IsRunning ? Color.Green : Color.Black;
                    }
                }
            }
        }
        private void RefreshCurrentMacroActions()
        {
            if (macroActionsListView.InvokeRequired)
            {
                macroActionsListView.Invoke(new Action(RefreshCurrentMacroActions));
                return;
            }

            if (macroListBox.SelectedIndex < 0) return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            DisplayMacroActions(macro);
        }
        private void UpdateRecordingState()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateRecordingState));
                return;
            }

            bool isRecording = MacroManager.IsRecording;
            btnMacroRecord.Enabled = !isRecording && macroListBox.SelectedIndex >= 0;
            btnMacroStopRecord.Enabled = isRecording;
            btnMacroPlay.Enabled = !isRecording && macroListBox.SelectedIndex >= 0;
            btnMacroNew.Enabled = !isRecording;
            btnMacroDelete.Enabled = !isRecording && macroListBox.SelectedIndex >= 0;
            btnMacroSave.Enabled = !isRecording && macroListBox.SelectedIndex >= 0;

            macroListBox.Enabled = !isRecording;




            lblMacroStatus.Text = isRecording ? "Recording... (Perform actions in-game)" : "Ready";
            lblMacroStatus.ForeColor = isRecording ? Color.Red : Color.Black;
        }
        private void MacroListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
            {
                btnMacroRecord.Enabled = false;
                btnMacroPlay.Enabled = false;
                btnMacroDelete.Enabled = false;
                btnMacroSave.Enabled = false;
                btnMacroStop.Enabled = false;
                macroActionsListView.Items.Clear();
                return;
            }

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            DisplayMacroActions(macro);
            chkMacroLoop.Checked = macro.Loop;

            btnMacroRecord.Enabled = !MacroManager.IsRecording;
            btnMacroPlay.Enabled = !MacroManager.IsRecording && !macro.IsRunning;
            btnMacroStop.Enabled = macro.IsRunning;
            btnMacroDelete.Enabled = !MacroManager.IsRecording;
            btnMacroSave.Enabled = !MacroManager.IsRecording;
        }
        private void ChkMacroLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            macro.Loop = chkMacroLoop.Checked;
        }
        private void BtnMacroNew_Click(object sender, EventArgs e)
        {
            // Gather all existing macro names (in memory and on disk)
            var macros = MacroManager.GetMacros();
            var existingNames = new HashSet<string>(macros.Select(m => m.Name), StringComparer.OrdinalIgnoreCase);

            // Also check for files in the Macros folder
            string macrosFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Macros");
            if (Directory.Exists(macrosFolder))
            {
                foreach (var file in Directory.GetFiles(macrosFolder, "*.macro"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    existingNames.Add(fileName);
                }
            }

            // Find the next available default name
            int macroNumber = 1;
            string defaultName;
            do
            {
                defaultName = $"New Macro {macroNumber:D2}";
                macroNumber++;
            } while (existingNames.Contains(defaultName));

            // Prompt user for macro name, pre-filled with the default
            string name = PromptForInput("Enter macro name:", "New Macro", defaultName);

            if (string.IsNullOrWhiteSpace(name))
                return;

            // Check for duplicates (in memory or on disk)
            if (existingNames.Contains(name))
            {
                MessageBox.Show("A macro with this name already exists (in memory or as a file). Please choose a different name.",
                    "Duplicate Macro Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create and add the macro
            var macro = new Macro { Name = name };
            MacroManager.AddMacro(macro);

            // Create a blank .macro file in the Macros folder
            try
            {
                if (!Directory.Exists(macrosFolder))
                    Directory.CreateDirectory(macrosFolder);

                // Sanitize macro name for file system
                string safeName = string.Join("_", macro.Name.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(macrosFolder, safeName + ".macro");

                // Only create if it doesn't exist
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create blank macro file:\n{ex.Message}", "New Macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Select the newly created macro
            macroListBox.SelectedIndex = macroListBox.Items.Count - 1;
        }
        private void BtnMacroDelete_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = MessageBox.Show(
                $"Delete macro '{macro.Name}'? This will also delete the .macro file if present.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Remove from MacroManager
                MacroManager.RemoveMacro(macro);

                // Delete the .macro file from the Macros folder
                string macrosFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Macros");
                string safeName = string.Join("_", macro.Name.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(macrosFolder, safeName + ".macro");
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete macro file:\n{ex.Message}", "Delete Macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                RefreshMacroList();
            }
        }
        private void BtnMacroRecord_Click(object sender, EventArgs e)
        {

            if (!Player.Connected)
            {
                MessageBox.Show("You must be connected to the server to record or play a macro.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (macroListBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a macro to record into, or create a new one.",
                    "No Macro Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Ask if they want to clear existing actions
            if (macro.Actions.Count > 0)
            {
                var result = MessageBox.Show(
                    $"Macro '{macro.Name}' has {macro.Actions.Count} action(s).\n\nDo you want to clear them and start fresh?",
                    "Clear Existing Actions?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Cancel) return;

                if (result == DialogResult.Yes)
                {
                    macro.Actions.Clear();
                    DisplayMacroActions(macro);
                }
            }

            MacroManager.StartRecording(macro);
        }
        private void BtnMacroStopRecord_Click(object sender, EventArgs e)
        {
            MacroManager.StopRecording();
        }
        private void BtnMacroPlay_Click(object sender, EventArgs e)
        {
            if (!Player.Connected)
            {
                MessageBox.Show("You must be connected to the server to record or play a macro.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (macroListBox.SelectedIndex < 0) return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            if (macro.Actions.Count == 0)
            {
                MessageBox.Show("This macro has no actions to play.",
                    "Empty Macro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            macro.Loop = chkMacroLoop.Checked;
            macro.Play();

            // Update button states immediately
            btnMacroStop.Enabled = true;
            btnMacroPlay.Enabled = false;
            lblMacroStatus.Text = "Macro running...";
            lblMacroStatus.ForeColor = Color.Green;
        }
        private void BtnMacroStop_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            macro.Stop();

            // Update button states immediately
            btnMacroStop.Enabled = false;
            btnMacroPlay.Enabled = true;
            lblMacroStatus.Text = "Ready";
            lblMacroStatus.ForeColor = Color.Black;
        }
        private void BtnMacroSave_Click(object sender, EventArgs e)
        {
            SaveMacroMenuItem_Click(sender, e);
        }
        private void SaveMacroMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            if (macro == null || string.IsNullOrWhiteSpace(macro.Name) || macro.Actions.Count == 0)
            {
                MessageBox.Show("No macro selected or macro is empty.", "Save Macro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Build the Macros folder path
            string mainFolder = AppDomain.CurrentDomain.BaseDirectory;
            string macrosFolder = System.IO.Path.Combine(mainFolder, "Macros");
            if (!System.IO.Directory.Exists(macrosFolder))
                System.IO.Directory.CreateDirectory(macrosFolder);

            // Sanitize macro name for file system
            string safeName = string.Join("_", macro.Name.Split(System.IO.Path.GetInvalidFileNameChars()));
            string filePath = System.IO.Path.Combine(macrosFolder, safeName + ".macro");

            try
            {
                using (var writer = new System.IO.StreamWriter(filePath, false))
                {
                    foreach (var action in macro.Actions)
                    {
                        writer.WriteLine(action.Serialize());
                    }
                }
                MessageBox.Show($"Macro saved to:\n{filePath}", "Save Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save macro:\n{ex.Message}", "Save Macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StartRecordingFromHereMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex > macro.Actions.Count)
                return;

            int insertIndex = actionIndex + 1;
            m_RecordFromActionIndex = insertIndex;

            var result = MessageBox.Show(
                $"You are about to start recording after action #{actionIndex + 1}.\n" +
                "Actions above and below will be kept. New actions will be inserted after this line, pushing the rest down.\n\n" +
                "Continue?",
                "Start Recording From Here",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            MacroManager.StartRecording(macro, insertIndex);

            if (insertIndex < macroActionsListView.Items.Count)
                macroActionsListView.Items[insertIndex].BackColor = Color.LightYellow;

            Misc.SendMessage($"Recording will insert new actions at line {insertIndex + 1}.", 88);
        }
        private void CreateMacroListBoxContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var saveMacroItem = new ToolStripMenuItem("Save Macro");
            saveMacroItem.Click += (s, e) => BtnMacroSave_Click(s, e);

            var deleteMacroItem = new ToolStripMenuItem("Delete Macro");
            deleteMacroItem.Click += (s, e) => BtnMacroDelete_Click(s, e);

            var renameMacroItem = new ToolStripMenuItem("Rename Macro");
            renameMacroItem.Click += (s, e) => RenameMacroMenuItem_Click(s, e);

            var reloadMacroItem = new ToolStripMenuItem("Reload Macros");
            reloadMacroItem.Click += (s, e) =>
            {
                MacroManager.LoadMacrosFromFiles();
                RefreshMacroList();
                Misc.SendMessage("Macros reloaded from .macro files.", 88);
            };

            contextMenu.Items.Add(renameMacroItem);
            contextMenu.Items.Add(saveMacroItem);
            contextMenu.Items.Add(deleteMacroItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(reloadMacroItem);

            macroListBox.ContextMenuStrip = contextMenu;

            // Only enable if a macro is selected
            contextMenu.Opening += (s, e) =>
            {
                bool hasSelection = macroListBox.SelectedIndex >= 0;
                saveMacroItem.Enabled = hasSelection;
                deleteMacroItem.Enabled = hasSelection;
                renameMacroItem.Enabled = hasSelection;
                reloadMacroItem.Enabled = true; // Always enabled
            };
        }
        private void RenameMacroMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            string oldName = macro.Name;

            // Prompt for new name, pre-filled with the current name
            string newName = PromptForInput("Enter new macro name:", "Rename Macro", oldName);

            if (string.IsNullOrWhiteSpace(newName) || newName.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                return;

            // Check for duplicates (in memory or on disk)
            var existingNames = new HashSet<string>(macros.Select(m => m.Name), StringComparer.OrdinalIgnoreCase);
            string macrosFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Macros");
            if (Directory.Exists(macrosFolder))
            {
                foreach (var file in Directory.GetFiles(macrosFolder, "*.macro"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    existingNames.Add(fileName);
                }
            }
            if (existingNames.Contains(newName))
            {
                MessageBox.Show("A macro with this name already exists (in memory or as a file). Please choose a different name.",
                    "Duplicate Macro Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Rename the file if it exists
            try
            {
                if (!Directory.Exists(macrosFolder))
                    Directory.CreateDirectory(macrosFolder);

                string oldSafeName = string.Join("_", oldName.Split(Path.GetInvalidFileNameChars()));
                string newSafeName = string.Join("_", newName.Split(Path.GetInvalidFileNameChars()));
                string oldFilePath = Path.Combine(macrosFolder, oldSafeName + ".macro");
                string newFilePath = Path.Combine(macrosFolder, newSafeName + ".macro");

                if (File.Exists(oldFilePath))
                {
                    if (File.Exists(newFilePath))
                    {
                        MessageBox.Show("A file with the new macro name already exists. Rename aborted.", "Rename Macro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    File.Move(oldFilePath, newFilePath);
                }
                else
                {
                    // If the old file doesn't exist, create a blank file for the new name
                    File.WriteAllText(newFilePath, string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to rename macro file:\n{ex.Message}", "Rename Macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update the macro name in memory
            macro.Name = newName;
            MacroManager.SaveMacros();
            RefreshMacroList();

            // Reselect the renamed macro
            int idx = macroListBox.Items.IndexOf(newName);
            if (idx >= 0)
                macroListBox.SelectedIndex = idx;

            Misc.SendMessage($"Macro renamed to '{newName}'", 88);
        }
        private string PromptForInput(string text, string caption, string defaultValue)
        {
            Form prompt = new Form
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label { Left = 20, Top = 20, Text = text, Width = 350 };
            TextBox textBox = new TextBox { Left = 20, Top = 50, Width = 350, Text = defaultValue };
            Button confirmation = new Button { Text = "OK", Left = 200, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            Button cancel = new Button { Text = "Cancel", Left = 290, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

            confirmation.Click += (s, ev) => { prompt.Close(); };
            cancel.Click += (s, ev) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
        private void MacroActionsListView_DoubleClick(object sender, EventArgs e)
        {
            // Must have exactly one action selected
            if (macroActionsListView.SelectedIndices.Count != 1)
                return;

            // Must have a macro selected
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count)
                return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            // Try to open the appropriate edit dialog based on action type
            OpenEditDialogForAction(action, macro, actionIndex);
        }
        private void OpenEditDialogForAction(MacroAction action, Macro macro, int actionIndex)
        {
            // Determine action type and open appropriate edit dialog
            if (action is RazorEnhanced.Macros.Actions.PauseAction pauseAction)
            {
                EditPauseAction(pauseAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.DropAction dropAction)
            {
                EditDropAction(dropAction, macro, actionIndex);
            }
            else if (action is ArmDisarmAction armDisarmAction)
            {
                EditArmDisarmAction(armDisarmAction, macro, actionIndex);
            }
            else if (action is MoveItemAction moveItemAction)
            {
                EditMoveItemAction(moveItemAction, macro, actionIndex);
            }
            else if (action is GumpResponseAction gumpResponseAction)
            {
                EditGumpResponseAction(gumpResponseAction, macro, actionIndex);
            }
            else if (action is WaitForGumpAction waitForGumpAction)
            {
                EditWaitForGumpAction(waitForGumpAction, macro, actionIndex);
            }
            else if (action is PromptResponseAction promptResponseAction)
            {
                EditPromptResponseAction(promptResponseAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.MovementAction movementAction)
            {
                EditMovementAction(movementAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.SetAbilityAction setAbilityAction)
            {
                EditSetAbilityAction(setAbilityAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.ForAction forAction)
            {
                EditForAction(forAction, macro, actionIndex);
            }
            else if (action is MessagingAction messagingAction)
            {
                EditMessagingAction(messagingAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.BandageAction bandageAction)
            {
                EditBandageAction(bandageAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.CommentAction commentAction)
            {
                EditCommentAction(commentAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.IfAction ifAction)
            {
                EditIfAction(ifAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.ElseIfAction elseIfAction)
            {
                EditElseIfAction(elseIfAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.WhileAction whileAction)
            {
                EditWhileAction(whileAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.RunOrganizerOnceAction runOrganizerAction)
            {
                EditRunOrganizerOnceAction(runOrganizerAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.InvokeVirtueAction invokeVirtueAction)
            {
                EditInvokeVirtueAction(invokeVirtueAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.UseEmoteAction useEmoteAction)
            {
                EditUseEmoteAction(useEmoteAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.ToggleWarModeAction toggleWarModeAction)
            {
                EditToggleWarModeAction(toggleWarModeAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.FlyAction flyAction)
            {
                EditFlyAction(flyAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.WaitForTargetAction waitForTargetAction)
            {
                EditWaitForTargetAction(waitForTargetAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.UsePotionAction usePotionAction)
            {
                EditUsePotionAction(usePotionAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.UseSkillAction useSkillAction)
            {
                EditUseSkillAction(useSkillAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.CastSpellAction castSpellAction)
            {
                EditCastSpellAction(castSpellAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.DoubleClickAction doubleClickAction)
            {
                EditDoubleClickAction(doubleClickAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.UseContextMenuAction useContextMenuAction)
            {
                EditUseContextMenuAction(useContextMenuAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.SetAliasAction setAliasAction)
            {
                EditSetAliasAction(setAliasAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.RemoveAliasAction removeAliasAction)
            {
                EditRemoveAliasAction(removeAliasAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.AttackAction attackEntityAction)
            {
                EditAttackEntityAction(attackEntityAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.TargetResourceAction targetResourceAction)
            {
                EditTargetResourceAction(targetResourceAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.MountAction mountAction)
            {
                EditMountAction(mountAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.RenameMobileAction renameMobileAction)
            {
                EditRenameMobileAction(renameMobileAction, macro, actionIndex);
            }
            else if (action is RazorEnhanced.Macros.Actions.TargetAction targetAction)
            {
                EditTargetAction(targetAction, macro, actionIndex);
            }
            // Non-editable actions: Else, EndIf, BandageSelf, Resync, ClearSystemMessages, LastTarget
            // They simply do nothing when double-clicked
        }
        private string GetConditionTypeDisplayName(IfAction.ConditionType type)
        {
            switch (type)
            {
                case IfAction.ConditionType.PlayerStats: return "PlayerStats";
                case IfAction.ConditionType.PlayerStatus: return "PlayerStatus";
                case IfAction.ConditionType.Find: return "Find"; // CHANGED FROM FindType
                case IfAction.ConditionType.InRange: return "InRange";
                case IfAction.ConditionType.InJournal: return "InJournal";
                case IfAction.ConditionType.BuffExists: return "BuffExists";
                case IfAction.ConditionType.TargetExists: return "TargetExists";
                case IfAction.ConditionType.Count: return "Count";
                case IfAction.ConditionType.Skill: return "Skill";
                default: return type.ToString();
            }
        }

        private IfAction.ConditionType GetConditionTypeFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "PlayerStats": return IfAction.ConditionType.PlayerStats;
                case "PlayerStatus": return IfAction.ConditionType.PlayerStatus;
                case "Find": return IfAction.ConditionType.Find; // CHANGED FROM FindType
                case "FindType": return IfAction.ConditionType.Find; // BACKWARD COMPATIBILITY
                case "InRange": return IfAction.ConditionType.InRange;
                case "InJournal": return IfAction.ConditionType.InJournal;
                case "BuffExists": return IfAction.ConditionType.BuffExists;
                case "TargetExists": return IfAction.ConditionType.TargetExists;
                case "Count": return IfAction.ConditionType.Count;
                case "Skill": return IfAction.ConditionType.Skill;
                default:
                    Enum.TryParse(displayName, out IfAction.ConditionType type);
                    return type;
            }
        }

        private void RefreshAndSelectAction(Macro macro, int actionIndex)
        {
            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (actionIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[actionIndex].Selected = true;
                macroActionsListView.EnsureVisible(actionIndex);
            }

            Misc.SendMessage("Action updated", 88);
        }

        private void SaveDefaultItemPresetsToFile(string filePath, Dictionary<string, (int graphic, int color)> presets)
        {
            try
            {
                // Convert to JSON-serializable format
                var jsonData = new Dictionary<string, Dictionary<string, int>>();

                foreach (var kvp in presets)
                {
                    jsonData[kvp.Key] = new Dictionary<string, int>
            {
                { "graphic", kvp.Value.graphic },
                { "color", kvp.Value.color }
            };
                }

                string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);

                // Create Data folder if it doesn't exist
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
                Misc.SendMessage($"Created default MacroItemsCount.json in Data folder", 88);
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error saving MacroItemsCount.json: {ex.Message}", 33);
            }
        }
        private Dictionary<string, (int graphic, int color)> LoadItemPresetsFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);

                // Deserialize to a dictionary with anonymous object structure
                var rawData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);

                if (rawData == null)
                    return null;

                var presets = new Dictionary<string, (int graphic, int color)>();

                foreach (var kvp in rawData)
                {
                    if (kvp.Value.ContainsKey("graphic") && kvp.Value.ContainsKey("color"))
                    {
                        int graphic = kvp.Value["graphic"];
                        int color = kvp.Value["color"];
                        presets[kvp.Key] = (graphic, color);
                    }
                }

                return presets;
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error loading MacroItemsCount.json: {ex.Message}", 33);
                return null;
            }
        }

        private void InsertIfMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var defaultIf = new IfAction();
            var result = ShowIfConditionDialog(defaultIf);

            if (result.success)
            {
                var ifAction = new IfAction(
                    result.type, result.op, result.value, result.graphic, result.color,
                    result.skillName, result.valueToken, result.booleanValue, result.presetName,
                    result.buffName, result.statType, result.statusType, result.rangeMode,
                    result.rangeSerial, result.rangeGraphic, result.rangeColor,
                    result.findEntityMode, result.findEntityLocation, result.findContainerSerial, result.findRange, result.findStoreSerial);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, ifAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted If condition at position {insertIndex + 1}", 88);
            }
        }
        private void EditIfMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.IfAction ifAction)
            {
                var result = ShowIfConditionDialog(ifAction);

                if (result.success)
                {
                    ifAction.Type = result.type;
                    ifAction.StatType = result.statType;
                    ifAction.StatusType = result.statusType;
                    ifAction.Op = result.op;
                    ifAction.Value = result.value;
                    ifAction.ValueToken = result.valueToken;
                    ifAction.BooleanValue = result.booleanValue;
                    ifAction.Graphic = result.graphic;
                    ifAction.Color = result.color;
                    ifAction.SkillName = result.skillName;
                    ifAction.PresetName = result.presetName;
                    ifAction.BuffName = result.buffName;
                    ifAction.RangeMode = result.rangeMode;
                    ifAction.RangeSerial = result.rangeSerial;
                    ifAction.RangeGraphic = result.rangeGraphic;
                    ifAction.RangeColor = result.rangeColor;
                    ifAction.FindEntityMode = result.findEntityMode;
                    ifAction.FindEntityLocation = result.findEntityLocation;
                    ifAction.FindContainerSerial = result.findContainerSerial;
                    ifAction.FindRange = result.findRange;
                    ifAction.FindStoreSerial = result.findStoreSerial;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("If condition updated", 88);
                }
            }
        }
        private void EditIfAction(IfAction ifAction, Macro macro, int actionIndex)
        {
            var result = ShowIfConditionDialog(ifAction);

            if (result.success)
            {
                ifAction.Type = result.type;
                ifAction.StatType = result.statType;
                ifAction.StatusType = result.statusType;
                ifAction.Op = result.op;
                ifAction.Value = result.value;
                ifAction.ValueToken = result.valueToken;
                ifAction.BooleanValue = result.booleanValue;
                ifAction.Graphic = result.graphic;
                ifAction.Color = result.color;
                ifAction.SkillName = result.skillName;
                ifAction.PresetName = result.presetName;
                ifAction.BuffName = result.buffName;
                ifAction.RangeMode = result.rangeMode;
                ifAction.RangeSerial = result.rangeSerial;
                ifAction.RangeGraphic = result.rangeGraphic;
                ifAction.RangeColor = result.rangeColor;
                ifAction.FindEntityMode = result.findEntityMode;
                ifAction.FindEntityLocation = result.findEntityLocation;
                ifAction.FindContainerSerial = result.findContainerSerial;
                ifAction.FindRange = result.findRange;
                ifAction.FindStoreSerial = result.findStoreSerial;

                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionIf()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.IfAction;
            }

            return false;
        }
        private string GetOperatorSymbol(IfAction.Operator op)
        {
            switch (op)
            {
                case IfAction.Operator.GreaterThan: return ">";
                case IfAction.Operator.LessThan: return "<";
                case IfAction.Operator.Equal: return "=";
                case IfAction.Operator.GreaterOrEqual: return ">=";
                case IfAction.Operator.LessOrEqual: return "<=";
                case IfAction.Operator.NotEqual: return "!=";
                default: return "?";
            }
        }
        private string FormatIfConditionDisplay(IfAction ifAction)
        {
            if (ifAction.Type == IfAction.ConditionType.PlayerStats)
            {
                string statName = ifAction.StatType.ToString();
                string displayValue = string.IsNullOrEmpty(ifAction.ValueToken) ? ifAction.Value.ToString() : ifAction.ValueToken;
                return $"{statName} {GetOperatorSymbol(ifAction.Op)} {displayValue}";
            }

            if (ifAction.Type == IfAction.ConditionType.PlayerStatus)
            {
                string prefix = ifAction.BooleanValue ? "" : "Not ";
                return $"{prefix}{ifAction.StatusType}";
            }

            if (ifAction.Type == IfAction.ConditionType.TargetExists)
            {
                string prefix = ifAction.BooleanValue ? "" : "Not ";
                return $"{prefix}TargetExists";
            }

            // CHANGED FROM FindType TO Find
            if (ifAction.Type == IfAction.ConditionType.Find)
            {
                string modeStr = ifAction.FindEntityMode == IfAction.FindMode.Item ? "Item" : "Mobile";
                string locationStr = "";

                if (ifAction.FindEntityMode == IfAction.FindMode.Item)
                {
                    switch (ifAction.FindEntityLocation)
                    {
                        case IfAction.FindLocation.Backpack:
                            locationStr = "Backpack";
                            break;
                        case IfAction.FindLocation.Container:
                            locationStr = $"Container 0x{ifAction.FindContainerSerial:X8}";
                            break;
                        case IfAction.FindLocation.Ground:
                            locationStr = $"Ground ({ifAction.FindRange} tiles)";
                            break;
                    }
                }
                else
                {
                    locationStr = $"Range {ifAction.FindRange}";
                }

                string colorStr = ifAction.Color == -1 ? "Any" : $"0x{ifAction.Color:X4}";
                return $"Find {modeStr}: 0x{ifAction.Graphic:X4} ({colorStr}) in {locationStr}";
            }

            if (ifAction.Type == IfAction.ConditionType.InJournal)
            {
                string prefix = ifAction.BooleanValue ? "" : "Not ";
                string searchText = string.IsNullOrEmpty(ifAction.ValueToken) ? "(empty)" : ifAction.ValueToken;
                return $"{prefix}InJournal: \"{searchText}\"";
            }

            if (ifAction.Type == IfAction.ConditionType.BuffExists)
            {
                string prefix = ifAction.BooleanValue ? "" : "Not ";
                string buffName = string.IsNullOrEmpty(ifAction.BuffName) ? "(none)" : ifAction.BuffName;
                return $"{prefix}BuffExists: {buffName}";
            }

            if (ifAction.Type == IfAction.ConditionType.Skill)
            {
                string displayValue = string.IsNullOrEmpty(ifAction.ValueToken) ? ifAction.Value.ToString() : ifAction.ValueToken;
                return $"{ifAction.SkillName} {GetOperatorSymbol(ifAction.Op)} {displayValue}";
            }

            if (ifAction.Type == IfAction.ConditionType.InRange)
            {
                string targetDesc;
                switch (ifAction.RangeMode)
                {
                    case IfAction.InRangeMode.LastTarget:
                        targetDesc = "Last Target";
                        break;
                    case IfAction.InRangeMode.Serial:
                        targetDesc = ifAction.RangeSerial == 0 ? "(no serial)" : $"Serial 0x{ifAction.RangeSerial:X8}";
                        break;
                    case IfAction.InRangeMode.ItemType:
                        string itemColorStr = ifAction.RangeColor == -1 ? "Any" : $"0x{ifAction.RangeColor:X4}";
                        targetDesc = $"ItemType 0x{ifAction.RangeGraphic:X4} ({itemColorStr})";
                        break;
                    case IfAction.InRangeMode.MobileType:
                        string mobileColorStr = ifAction.RangeColor == -1 ? "Any" : $"0x{ifAction.RangeColor:X4}";
                        targetDesc = $"MobileType 0x{ifAction.RangeGraphic:X4} ({mobileColorStr})";
                        break;
                    default:
                        targetDesc = "Unknown";
                        break;
                }
                return $"{targetDesc} InRange {GetOperatorSymbol(ifAction.Op)} {ifAction.Value}";
            }

            if (ifAction.Type == IfAction.ConditionType.Count)
            {
                string presetDisplay = string.IsNullOrEmpty(ifAction.PresetName) || ifAction.PresetName == "Custom"
                    ? ""
                    : $"{ifAction.PresetName} ";
                string colorStr = ifAction.Color == -1 ? "Any" : $"0x{ifAction.Color:X4}";
                return $"Count: {presetDisplay}(0x{ifAction.Graphic:X4}, Color: {colorStr}) {GetOperatorSymbol(ifAction.Op)} {ifAction.Value}";
            }

            // Fallback
            string value = string.IsNullOrEmpty(ifAction.ValueToken) ? ifAction.Value.ToString() : ifAction.ValueToken;
            return $"{ifAction.Type} {GetOperatorSymbol(ifAction.Op)} {value}";
        }
        private Dictionary<string, (int graphic, int color)> GetItemPresets()
        {
            // Define default presets
            var defaultPresets = new Dictionary<string, (int graphic, int color)>
    {
        { "Custom", (0x0, -1) },
        { "Gold", (0x0EED, 0) },
        { "Silver", (0x0EF0, 0) },
        { "Bandages", (0x0E21, 0) },
        { "Black Pearl", (0x0F7A, 0) },
        { "Blood Moss", (0x0F7B, 0) },
        { "Garlic", (0x0F84, 0) },
        { "Ginseng", (0x0F85, 0) },
        { "Mandrake Root", (0x0F86, 0) },
        { "Nightshade", (0x0F88, 0) },
        { "Sulfurous Ash", (0x0F8C, 0) },
        { "Spider's Silk", (0x0F8D, 0) },
        { "Nox Crystal", (0x0F8E, 0) },
        { "Pig Iron", (0x0F8A, 0) },
        { "Grave Dust", (0x0F8F, 0) },
        { "Bat Wing", (0x0F78, 0) },
        { "Daemon Blood", (0x0F7D, 0) },
        { "Dragon Blood", (0x4077, 0) },
        { "Fertile Dirt", (0x0F81, 0) },
        { "Daemon Bone", (0x0F80, 0) },
        { "Arrow", (0x0F3F, 0) },
        { "Bolt", (0x1BFB, 0) },
        { "Bolt of Cloth", (0x0F95, 0) },
        { "Cut Cloth", (0x1766, 0) },
        { "Uncut Cloth", (0x1767, 0) },
        { "Cotton Bale", (0x0DF9, 0) },
        { "Dark Yarn", (0x0E1D, 0) },
        { "Light Yarn", (0x0E1E, 0) },
        { "Spool of Thread", (0x0FA0, 0) },
        { "Ore", (0x19B9, 0) },
        { "Ingot", (0x1BF2, 0) },
        { "Granite", (0x1779, 0) },
        { "Leather", (0x1081, 0) },
        { "Hides", (0x1079, 0) },
        { "Empty Bottle", (0x0F0E, 0) },
        { "Heal Potion", (0x0F0C, 0) },
        { "Cure Potion", (0x0F07, 0) },
        { "Refresh Potion", (0x0F0B, 0) },
        { "Agility Potion", (0x0F08, 0) },
        { "Strength Potion", (0x0F09, 0) },
        { "Night Sight Potion", (0x0F06, 0) },
        { "Invisibility Potion", (0x0F0A, 0x48D) },
        { "Zoogi Fungus", (0x26B7, 0) },
        { "Translocation Powder", (0x26B8, 0) }
    };

            // Check for custom JSON file in Data folder
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string jsonFilePath = Path.Combine(dataFolder, "MacroItemsCount.json");

            // Try to load from JSON file
            var loadedPresets = LoadItemPresetsFromFile(jsonFilePath);

            if (loadedPresets != null && loadedPresets.Count > 0)
            {
                // Ensure "Custom" is always present
                if (!loadedPresets.ContainsKey("Custom"))
                {
                    loadedPresets["Custom"] = (0x0, -1);
                }

                Misc.SendMessage($"Loaded {loadedPresets.Count} item presets from MacroItemsCount.json", 88);
                return loadedPresets;
            }
            else
            {
                // File doesn't exist or failed to load - create it with defaults
                //SaveDefaultItemPresetsToFile(jsonFilePath, defaultPresets);
                return defaultPresets;
            }
        }

        private (bool success, IfAction.ConditionType type, IfAction.Operator op, int value,
                 int graphic, int color, string skillName, string valueToken, bool booleanValue,
                 string presetName, string buffName, IfAction.PlayerStatType statType,
                 IfAction.PlayerStatusType statusType, IfAction.InRangeMode rangeMode,
                 int rangeSerial, int rangeGraphic, int rangeColor,
                 IfAction.FindMode findEntityMode, IfAction.FindLocation findEntityLocation,
                 int findContainerSerial, int findRange, bool findStoreSerial) ShowIfConditionDialog(IfAction ifAction)
        {
            Form dialog = new Form
            {
                Width = 480,
                Height = 600,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Configure If/ElseIf/While Condition",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            // === CONDITION TYPE (Top = 20) ===
            Label lblType = new Label { Left = 20, Top = 20, Text = "Condition Type:", Width = 120 };
            ComboBox cmbType = new ComboBox
            {
                Left = 150,
                Top = 20,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };



            foreach (IfAction.ConditionType condType in Enum.GetValues(typeof(IfAction.ConditionType)))
            {
                cmbType.Items.Add(GetConditionTypeDisplayName(condType));
            }

            // === PLAYER STAT SELECTOR (Top = 60) ===
            Label lblStatType = new Label { Left = 20, Top = 60, Text = "Player Stat:", Width = 120 };
            ComboBox cmbStatType = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatType.Items.AddRange(new string[] { "Hit Points", "Mana", "Stamina", "Weight", "Str", "Dex", "Int" });

            // === PLAYER STATUS SELECTOR (Top = 60) ===
            Label lblStatusType = new Label { Left = 20, Top = 60, Text = "Player Status:", Width = 120 };
            ComboBox cmbStatusType = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatusType.Items.AddRange(new string[] { "Poisoned", "Paralyzed", "Hidden", "Mounted", "Is Alive", "Right Hand Equipped", "Left Hand Equipped" });

            // === INRANGE MODE SELECTOR (Top = 60) ===
            Label lblRangeMode = new Label { Left = 20, Top = 60, Text = "Range Check:", Width = 120 };
            ComboBox cmbRangeMode = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRangeMode.Items.AddRange(new string[] { "Last Target", "Specific Serial", "Item Type", "Mobile Type" });

            // === RANGE SERIAL (Top = 100) ===
            Label lblRangeSerial = new Label { Left = 20, Top = 100, Text = "Serial:", Width = 120 };
            TextBox txtRangeSerial = new TextBox
            {
                Left = 150,
                Top = 100,
                Width = 200,
                Text = ifAction.RangeSerial == 0 ? "" : $"0x{ifAction.RangeSerial:X8}"
            };

            Button btnTargetRangeSerial = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 98,
                Width = 80
            };

            // === RANGE GRAPHIC (Top = 140) ===
            Label lblRangeGraphic = new Label { Left = 20, Top = 140, Text = "Graphic (hex):", Width = 120 };
            TextBox txtRangeGraphic = new TextBox
            {
                Left = 150,
                Top = 140,
                Width = 200,
                Text = ifAction.RangeGraphic == 0 ? "" : $"0x{ifAction.RangeGraphic:X4}"
            };

            Button btnTargetRangeType = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 138,
                Width = 80
            };

            // === RANGE COLOR (Top = 180) ===
            Label lblRangeColor = new Label { Left = 20, Top = 180, Text = "Color (-1 = any):", Width = 120 };
            TextBox txtRangeColor = new TextBox { Left = 150, Top = 180, Width = 290, Text = ifAction.RangeColor.ToString() };

            // === NEW FIND MODE SELECTOR (Top = 60) ===
            Label lblFindMode = new Label { Left = 20, Top = 60, Text = "Find Mode:", Width = 120 };
            ComboBox cmbFindMode = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFindMode.Items.AddRange(new string[] { "Item", "Mobile" });
            cmbFindMode.SelectedIndex = (int)ifAction.FindEntityMode;

            // === FIND LOCATION SELECTOR (Top = 100) ===
            Label lblFindLocation = new Label { Left = 20, Top = 100, Text = "Find Location:", Width = 120 };
            ComboBox cmbFindLocation = new ComboBox
            {
                Left = 150,
                Top = 100,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFindLocation.Items.AddRange(new string[] { "Backpack", "Container", "Ground" });
            cmbFindLocation.SelectedIndex = (int)ifAction.FindEntityLocation;

            // === FIND CONTAINER SERIAL (Top = 140) ===
            Label lblFindContainer = new Label { Left = 20, Top = 140, Text = "Container Serial:", Width = 120 };
            TextBox txtFindContainer = new TextBox
            {
                Left = 150,
                Top = 140,
                Width = 200,
                Text = ifAction.FindContainerSerial == 0 ? "" : $"0x{ifAction.FindContainerSerial:X8}"
            };

            Button btnTargetContainer = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 138,
                Width = 80
            };

            // === FIND RANGE (Top = 180) ===
            Label lblFindRange = new Label { Left = 20, Top = 180, Text = "Range (tiles):", Width = 120 };
            TextBox txtFindRange = new TextBox { Left = 150, Top = 180, Width = 290, Text = ifAction.FindRange.ToString() };

            // === FIND GRAPHIC (Top = 220) ===
            Label lblFindGraphic = new Label { Left = 20, Top = 220, Text = "Graphic (hex):", Width = 120 };
            TextBox txtFindGraphic = new TextBox { Left = 150, Top = 220, Width = 200, Text = $"0x{ifAction.Graphic:X4}" };

            Button btnTargetFindType = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 218,
                Width = 80
            };

            // === FIND COLOR (Top = 260) ===
            Label lblFindColor = new Label { Left = 20, Top = 260, Text = "Color (-1 = any):", Width = 120 };
            TextBox txtFindColor = new TextBox { Left = 150, Top = 260, Width = 290, Text = ifAction.Color.ToString() };


            // === STORE SERIAL CHECKBOX (Top = 300) ===
            CheckBox chkFindStoreSerial = new CheckBox
            {
                Left = 150,
                Top = 300,
                Width = 290,
                Text = "Store found serial to 'findfound' alias",
                Checked = ifAction.FindStoreSerial
            };

            Label lblFindStoreNote = new Label
            {
                Left = 150,
                Top = 325,
                Width = 290,
                Height = 30,
                Text = "Use Target.SetAlias('findfound') to use the found item/mobile",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };


            // === BOOLEAN CHECK (Top = 100) ===
            Label lblBoolValue = new Label { Left = 20, Top = 100, Text = "Check For:", Width = 120 };
            ComboBox cmbBoolValue = new ComboBox
            {
                Left = 150,
                Top = 100,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbBoolValue.Items.AddRange(new string[] { "Is True", "Is False" });
            cmbBoolValue.SelectedIndex = ifAction.BooleanValue ? 0 : 1;

            // === OPERATOR (Top = 220) ===
            Label lblOp = new Label { Left = 20, Top = 220, Text = "Operator:", Width = 120 };
            ComboBox cmbOp = new ComboBox
            {
                Left = 150,
                Top = 220,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbOp.Items.AddRange(new string[] { ">", "<", "=", ">=", "<=", "!=" });

            // === MAX VALUE TOKEN (Top = 260) ===
            Label lblValueToken = new Label { Left = 20, Top = 260, Text = "Use Max Value:", Width = 120 };
            ComboBox cmbValueToken = new ComboBox
            {
                Left = 150,
                Top = 260,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbValueToken.Items.AddRange(new string[] { "None", "{maxhp}", "{maxstam}", "{maxmana}" });

            // === VALUE (Top = 300) ===
            Label lblValue = new Label { Left = 20, Top = 300, Text = "Value:", Width = 120 };
            TextBox txtValue = new TextBox
            {
                Left = 150,
                Top = 300,
                Width = 290,
                Text = ifAction.Value.ToString()
            };

            cmbValueToken.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbValueToken.SelectedItem != null)
                {
                    string selected = cmbValueToken.SelectedItem.ToString();
                    if (selected == "None")
                    {
                        txtValue.Enabled = true;
                        if (string.IsNullOrEmpty(ifAction.ValueToken))
                            txtValue.Text = ifAction.Value.ToString();
                    }
                    else
                    {
                        txtValue.Enabled = false;
                        txtValue.Text = selected;
                    }
                }
            };

            // === ITEM PRESET (Top = 60) ===
            Label lblPreset = new Label { Left = 20, Top = 60, Text = "Item Preset:", Width = 120 };
            ComboBox cmbPreset = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var itemPresets = GetItemPresets();
            cmbPreset.Items.AddRange(itemPresets.Keys.ToArray());

            string initialPreset = "Custom";
            if (!string.IsNullOrEmpty(ifAction.PresetName) && itemPresets.ContainsKey(ifAction.PresetName))
            {
                initialPreset = ifAction.PresetName;
            }
            else
            {
                foreach (var preset in itemPresets)
                {
                    if (preset.Value.graphic == ifAction.Graphic && preset.Value.color == ifAction.Color && preset.Key != "Custom")
                    {
                        initialPreset = preset.Key;
                        break;
                    }
                }
            }
            cmbPreset.SelectedItem = initialPreset;

            // === TARGET ITEM BUTTON (Top = 95) ===
            Button btnSelectItem = new Button
            {
                Text = "Target Item to Get Type",
                Left = 150,
                Top = 95,
                Width = 290
            };

            // === GRAPHIC (Top = 130) ===
            Label lblGraphic = new Label { Left = 20, Top = 130, Text = "Graphic (hex):", Width = 120 };
            TextBox txtGraphic = new TextBox { Left = 150, Top = 130, Width = 290, Text = $"0x{ifAction.Graphic:X4}" };

            // === COLOR (Top = 170) ===
            Label lblColor = new Label { Left = 20, Top = 170, Text = "Color (-1 = any):", Width = 120 };
            TextBox txtColor = new TextBox { Left = 150, Top = 170, Width = 290, Text = ifAction.Color.ToString() };

            // === SKILL SELECTOR (Top = 60) ===
            Label lblSkill = new Label { Left = 20, Top = 60, Text = "Skill Name:", Width = 120 };
            ComboBox cmbSkill = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSkill.Items.AddRange(GetAllSkills());
            if (!string.IsNullOrEmpty(ifAction.SkillName))
                cmbSkill.SelectedItem = ifAction.SkillName;

            // === JOURNAL TEXT (Top = 60) ===
            Label lblJournalText = new Label { Left = 20, Top = 60, Text = "Journal Text:", Width = 120 };
            TextBox txtJournalText = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                Text = ifAction.ValueToken ?? ""
            };

            Label lblJournalCheck = new Label { Left = 20, Top = 100, Text = "Check For:", Width = 120 };
            ComboBox cmbJournalCheck = new ComboBox
            {
                Left = 150,
                Top = 100,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbJournalCheck.Items.AddRange(new string[] { "Text Found", "Text Not Found" });
            cmbJournalCheck.SelectedIndex = ifAction.BooleanValue ? 0 : 1;

            // === BUFF SELECTOR (Top = 60) ===
            Label lblBuff = new Label { Left = 20, Top = 60, Text = "Buff Name:", Width = 120 };
            ComboBox cmbBuff = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Sorted = true
            };
            var allBuffs = new List<string>(Player.BuffsMapping.Values);
            allBuffs.Sort();
            cmbBuff.Items.AddRange(allBuffs.ToArray());
            if (!string.IsNullOrEmpty(ifAction.BuffName))
                cmbBuff.SelectedItem = ifAction.BuffName;

            Label lblBuffCheck = new Label { Left = 20, Top = 100, Text = "Check For:", Width = 120 };
            ComboBox cmbBuffCheck = new ComboBox
            {
                Left = 150,
                Top = 100,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbBuffCheck.Items.AddRange(new string[] { "Buff Active", "Buff Not Active" });
            cmbBuffCheck.SelectedIndex = ifAction.BooleanValue ? 0 : 1;

            // === PRESET CHANGE HANDLERS ===
            cmbPreset.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbPreset.SelectedItem != null)
                {
                    string selectedPreset = cmbPreset.SelectedItem.ToString();
                    if (itemPresets.TryGetValue(selectedPreset, out var preset))
                    {
                        if (selectedPreset != "Custom")
                        {
                            txtGraphic.Text = $"0x{preset.graphic:X4}";
                            txtColor.Text = preset.color.ToString();
                        }
                    }
                }
            };

            EventHandler detectCustom = (s, ev) =>
            {
                if (cmbPreset.SelectedItem?.ToString() == "Custom")
                    return;

                string graphicStr = txtGraphic.Text.Replace("0x", "").Replace("0X", "");
                int.TryParse(graphicStr, System.Globalization.NumberStyles.HexNumber, null, out int currentGraphic);
                int.TryParse(txtColor.Text, out int currentColor);

                if (cmbPreset.SelectedItem != null && itemPresets.TryGetValue(cmbPreset.SelectedItem.ToString(), out var selectedPreset))
                {
                    if (currentGraphic != selectedPreset.graphic || currentColor != selectedPreset.color)
                    {
                        cmbPreset.SelectedItem = "Custom";
                    }
                }
            };

            txtGraphic.TextChanged += detectCustom;
            txtColor.TextChanged += detectCustom;

            // === TARGET ITEM BUTTON HANDLER ===
            btnSelectItem.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item to get its type...", 88);

                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        var item = Items.FindBySerial((int)serial);
                        if (item != null)
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtGraphic.Text = $"0x{item.ItemID:X4}";
                                txtColor.Text = item.Hue.ToString();

                                bool foundPreset = false;
                                foreach (var preset in itemPresets)
                                {
                                    if (preset.Value.graphic == item.ItemID && preset.Value.color == item.Hue && preset.Key != "Custom")
                                    {
                                        cmbPreset.SelectedItem = preset.Key;
                                        foundPreset = true;
                                        break;
                                    }
                                }

                                if (!foundPreset)
                                {
                                    cmbPreset.SelectedItem = "Custom";
                                }

                                Misc.SendMessage($"Set to item: 0x{item.ItemID:X4}, Color: {item.Hue}", 88);
                            }));
                        }
                        else
                        {
                            Misc.SendMessage("Target must be an item, not a mobile.", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // === TARGET CONTAINER BUTTON HANDLER ===
            btnTargetContainer.Click += (s, ev) =>
            {
                
                Misc.SendMessage("Target a container...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {

                        int serialValue = (int)serial;
                        txtFindContainer.Text = $"0x{serialValue:X8}";

                        var item = Items.FindBySerial(serialValue);
                        if (item != null)
                        {
                            Misc.SendMessage($"Set container to: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                        }
                        else
                        {
                            Misc.SendMessage("Target must be a container.", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // === TARGET FIND TYPE BUTTON HANDLER ===
            btnTargetFindType.Click += (s, ev) =>
            {
                var selectedMode = (IfAction.FindMode)cmbFindMode.SelectedIndex;
                string modeStr = selectedMode == IfAction.FindMode.Item ? "item" : "mobile";
                Misc.SendMessage($"Target a {modeStr} to get its type...", 88);

                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            if (selectedMode == IfAction.FindMode.Item)
                            {
                                var item = Items.FindBySerial((int)serial);
                                if (item != null)
                                {
                                    txtFindGraphic.Text = $"0x{item.ItemID:X4}";
                                    txtFindColor.Text = item.Hue.ToString();
                                    Misc.SendMessage($"Set to item type: 0x{item.ItemID:X4}, Color: {item.Hue}", 88);
                                }
                                else
                                {
                                    Misc.SendMessage("Target must be an item.", 33);
                                }
                            }
                            else if (selectedMode == IfAction.FindMode.Mobile)
                            {
                                var mobile = Mobiles.FindBySerial((int)serial);
                                if (mobile != null)
                                {
                                    txtFindGraphic.Text = $"0x{mobile.Body:X4}";
                                    txtFindColor.Text = mobile.Color.ToString();
                                    Misc.SendMessage($"Set to mobile type: 0x{mobile.Body:X4}, Color: {mobile.Color}", 88);
                                }
                                else
                                {
                                    Misc.SendMessage("Target must be a mobile.", 33);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // === RANGE SERIAL TARGET BUTTON HANDLER ===
            btnTargetRangeSerial.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an entity for range check...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)serial;
                            txtRangeSerial.Text = $"0x{serialValue:X8}";

                            var mobile = Mobiles.FindBySerial(serialValue);
                            if (mobile != null)
                            {
                                Misc.SendMessage($"Set to mobile: {mobile.Name} (0x{serialValue:X8})", 88);
                            }
                            else
                            {
                                var item = Items.FindBySerial(serialValue);
                                if (item != null)
                                {
                                    Misc.SendMessage($"Set to item: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                                }
                                else
                                {
                                    Misc.SendMessage($"Set to serial: 0x{serialValue:X8}", 88);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // === RANGE TYPE TARGET BUTTON HANDLER ===
            btnTargetRangeType.Click += (s, ev) =>
            {
                var selectedMode = (IfAction.InRangeMode)cmbRangeMode.SelectedIndex;
                string modeStr = selectedMode == IfAction.InRangeMode.ItemType ? "item" : "mobile";
                Misc.SendMessage($"Target a {modeStr} to get its type...", 88);

                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            if (selectedMode == IfAction.InRangeMode.ItemType)
                            {
                                var item = Items.FindBySerial((int)serial);
                                if (item != null)
                                {
                                    txtRangeGraphic.Text = $"0x{item.ItemID:X4}";
                                    txtRangeColor.Text = item.Hue.ToString();
                                    Misc.SendMessage($"Set to item type: 0x{item.ItemID:X4}, Color: {item.Hue}", 88);
                                }
                                else
                                {
                                    Misc.SendMessage("Target must be an item.", 33);
                                }
                            }
                            else if (selectedMode == IfAction.InRangeMode.MobileType)
                            {
                                var mobile = Mobiles.FindBySerial((int)serial);
                                if (mobile != null)
                                {
                                    txtRangeGraphic.Text = $"0x{mobile.Body:X4}";
                                    txtRangeColor.Text = mobile.Color.ToString();
                                    Misc.SendMessage($"Set to mobile type: 0x{mobile.Body:X4}, Color: {mobile.Color}", 88);
                                }
                                else
                                {
                                    Misc.SendMessage("Target must be a mobile.", 33);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Button btnCreateList = new Button
            {
                Text = "Custom List",
                Left = 20,
                Top = 95,
                Width = 100,
                Visible = false // Only visible in Count mode and if file doesn't exist
            };

            // Handler for button click
            btnCreateList.Click += (s, ev) =>
            {
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                string jsonFilePath = Path.Combine(dataFolder, "MacroItemsCount.json");
                if (!Directory.Exists(dataFolder))
                    Directory.CreateDirectory(dataFolder);

                // Use the default presets from GetItemPresets
                var defaultPresets = GetItemPresets();
                SaveDefaultItemPresetsToFile(jsonFilePath, defaultPresets);

                btnCreateList.Enabled = false;
                btnCreateList.Text = "List Created!";
                Misc.SendMessage("Default MacroItemsCount.json created.", 88);
            };


            // === BUTTONS (Top = 520 with more margin) ===
            Button btnOK = new Button
            {
                Text = "OK",
                Left = 240,
                Width = 90,
                Top = 520,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 340,
                Width = 90,
                Top = 520,
                DialogResult = DialogResult.Cancel
            };

            // === VISIBILITY LOGIC ===
            Action updateVisibility = () =>
            {
                if (cmbType.SelectedItem == null)
                    return;

                IfAction.ConditionType selectedType = GetConditionTypeFromDisplayName(cmbType.SelectedItem.ToString());

                bool isPlayerStat = selectedType == IfAction.ConditionType.PlayerStats;
                bool isPlayerStatus = selectedType == IfAction.ConditionType.PlayerStatus;
                bool isTargetExists = selectedType == IfAction.ConditionType.TargetExists;
                bool isFind = selectedType == IfAction.ConditionType.Find;
                bool isCount = selectedType == IfAction.ConditionType.Count;
                bool isSkill = selectedType == IfAction.ConditionType.Skill;
                bool isInRange = selectedType == IfAction.ConditionType.InRange;
                bool isJournal = selectedType == IfAction.ConditionType.InJournal;
                bool isBuff = selectedType == IfAction.ConditionType.BuffExists;

                // Show player stat selector
                lblStatType.Visible = cmbStatType.Visible = isPlayerStat;

                // Show player status selector
                lblStatusType.Visible = cmbStatusType.Visible = isPlayerStatus;

                // Show boolean check (for PlayerStatus, TargetExists - positioned at 100)
                bool showBoolCheck = isPlayerStatus || isTargetExists;
                lblBoolValue.Visible = cmbBoolValue.Visible = showBoolCheck;

                // Show numeric fields (for PlayerStats, Skill, InRange, Count)
                bool showNumeric = isPlayerStat || isSkill || isInRange || isCount;
                lblOp.Visible = cmbOp.Visible = showNumeric;
                lblValue.Visible = txtValue.Visible = showNumeric;

                // Show max value token ONLY for HP/Mana/Stam
                bool showMaxToken = isPlayerStat &&
                    (cmbStatType.SelectedIndex == 0 || cmbStatType.SelectedIndex == 1 || cmbStatType.SelectedIndex == 2);
                lblValueToken.Visible = cmbValueToken.Visible = showMaxToken;

                // Set default values
                if (isInRange && (txtValue.Text == "" || txtValue.Text == "0" || txtValue.Text == "50" || txtValue.Text == "1"))
                {
                    txtValue.Text = "10";
                }
                if (isCount && (txtValue.Text == "" || txtValue.Text == "50" || txtValue.Text == "10"))
                {
                    txtValue.Text = "1";
                }
                if (isFind && (txtFindRange.Text == "" || txtFindRange.Text == "0" || txtFindRange.Text == "10"))
                {
                    txtFindRange.Text = "2";
                }

                // Show Find mode selector
                lblFindMode.Visible = cmbFindMode.Visible = isFind;

                // Show Find fields based on mode and location
                // In updateVisibility() action, update the Find section:

                // Show Find fields based on mode and location
                if (isFind)
                {
                    var findMode = (IfAction.FindMode)cmbFindMode.SelectedIndex;
                    bool isItemMode = findMode == IfAction.FindMode.Item;
                    bool isMobileMode = !isItemMode;

                    // MOBILE MODE: Force Ground location and hide location selector
                    if (isMobileMode)
                    {
                        cmbFindLocation.SelectedIndex = (int)IfAction.FindLocation.Ground;
                        lblFindLocation.Visible = cmbFindLocation.Visible = false;

                        // Show only range for mobiles
                        lblFindRange.Visible = txtFindRange.Visible = true;
                        lblFindContainer.Visible = txtFindContainer.Visible = btnTargetContainer.Visible = false;
                    }
                    else // ITEM MODE: Show location selector
                    {
                        lblFindLocation.Visible = cmbFindLocation.Visible = true;

                        var findLocation = (IfAction.FindLocation)cmbFindLocation.SelectedIndex;
                        bool isBackpackLocation = findLocation == IfAction.FindLocation.Backpack;
                        bool isContainerLocation = findLocation == IfAction.FindLocation.Container;
                        bool isGroundLocation = findLocation == IfAction.FindLocation.Ground;

                        // Container serial only for Container location
                        lblFindContainer.Visible = txtFindContainer.Visible = btnTargetContainer.Visible = isContainerLocation;

                        // Range only for Ground location (not for Backpack or Container)
                        lblFindRange.Visible = txtFindRange.Visible = isGroundLocation;
                    }

                    // Graphic/Color always visible for Find
                    lblFindGraphic.Visible = txtFindGraphic.Visible = btnTargetFindType.Visible = true;
                    lblFindColor.Visible = txtFindColor.Visible = true;

                    chkFindStoreSerial.Visible = true;
                    lblFindStoreNote.Visible = true;
                }
                else
                {
                    lblFindLocation.Visible = cmbFindLocation.Visible = false;
                    lblFindContainer.Visible = txtFindContainer.Visible = btnTargetContainer.Visible = false;
                    lblFindRange.Visible = txtFindRange.Visible = false;
                    lblFindGraphic.Visible = txtFindGraphic.Visible = btnTargetFindType.Visible = false;
                    lblFindColor.Visible = txtFindColor.Visible = false;

                    chkFindStoreSerial.Visible = false;
                    lblFindStoreNote.Visible = false;
                }

                // visible for count condition
                lblGraphic.Visible = txtGraphic.Visible = isCount;
                lblColor.Visible = txtColor.Visible = isCount;
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                string jsonFilePath = Path.Combine(dataFolder, "MacroItemsCount.json");
                btnCreateList.Visible = !File.Exists(jsonFilePath) && isCount;

                // Show preset fields (Count only)
                btnSelectItem.Visible = lblPreset.Visible = cmbPreset.Visible = isCount;

                // Show InRange mode selector and fields
                lblRangeMode.Visible = cmbRangeMode.Visible = isInRange;

                if (isInRange)
                {
                    var rangeMode = (IfAction.InRangeMode)cmbRangeMode.SelectedIndex;
                    bool isRangeLastTarget = rangeMode == IfAction.InRangeMode.LastTarget;
                    bool isRangeSerial = rangeMode == IfAction.InRangeMode.Serial;
                    bool isRangeItemType = rangeMode == IfAction.InRangeMode.ItemType;
                    bool isRangeMobileType = rangeMode == IfAction.InRangeMode.MobileType;
                    bool isRangeType = isRangeItemType || isRangeMobileType;

                    lblRangeSerial.Visible = txtRangeSerial.Visible = btnTargetRangeSerial.Visible = isRangeSerial;
                    lblRangeGraphic.Visible = txtRangeGraphic.Visible = btnTargetRangeType.Visible = isRangeType;
                    lblRangeColor.Visible = txtRangeColor.Visible = isRangeType;
                }
                else
                {
                    lblRangeSerial.Visible = txtRangeSerial.Visible = btnTargetRangeSerial.Visible = false;
                    lblRangeGraphic.Visible = txtRangeGraphic.Visible = btnTargetRangeType.Visible = false;
                    lblRangeColor.Visible = txtRangeColor.Visible = false;
                }

                // Show skill selector
                lblSkill.Visible = cmbSkill.Visible = isSkill;

                // Show journal fields
                lblJournalText.Visible = txtJournalText.Visible = isJournal;
                lblJournalCheck.Visible = cmbJournalCheck.Visible = isJournal;

                // Show buff fields
                lblBuff.Visible = cmbBuff.Visible = isBuff;
                lblBuffCheck.Visible = cmbBuffCheck.Visible = isBuff;



                var vissettings = GetConditionTypeFromDisplayName(cmbType.SelectedItem.ToString());

                switch (vissettings)
                {
                    case IfAction.ConditionType.PlayerStats:

                        dialog.Height = lblValueToken.Visible ? 300 : 260;

                        //"Player Stat: label and comboxbox
                        lblStatType.Top = 60;
                        cmbStatType.Top = 60;

                        //operator combobox and label
                        lblOp.Top = 100;
                        cmbOp.Top = 100;

                        //max value combobox and label
                        lblValueToken.Top = 140;
                        cmbValueToken.Top = 140;

                        //value textbox and label
                        lblValue.Top = lblValueToken.Visible ? 180 : 140;
                        txtValue.Top = lblValueToken.Visible ? 180 : 140;

                        //ok button
                        btnOK.Top = lblValueToken.Visible ? 220 : 180;

                        //cancelbutton
                        btnCancel.Top = lblValueToken.Visible ? 220 : 180;
                        break;
                    case IfAction.ConditionType.PlayerStatus:
                        dialog.Height = 220;

                        // Player Status comboxbox and label
                        lblStatusType.Top = 60;
                        cmbStatusType.Top = 60;

                        //boolean combobox and label
                        lblBoolValue.Top = 100;
                        cmbBoolValue.Top = 100;

                        //ok button
                        btnOK.Top = 140;
                        //cancelbutton
                        btnCancel.Top = 140;
                        break;
                    case IfAction.ConditionType.Find:
                        dialog.Height = lblFindLocation.Visible ? lblFindContainer.Visible ? 400 : lblFindRange.Visible ? 400 : 360 : 360;

                        //find mode combobox and label
                        lblFindMode.Top = 60;
                        cmbFindMode.Top = 60;

                        //find location combobox and label
                        lblFindLocation.Top = 100;
                        cmbFindLocation.Top = 100;

                        //find container serial comboxbox label and button
                        lblFindContainer.Top = 140;
                        txtFindContainer.Top = 140;
                        btnTargetContainer.Top = 138;

                        // find range textbox and label
                        lblFindRange.Top = lblFindLocation.Visible ? 140 : 100;
                        txtFindRange.Top = lblFindLocation.Visible ? 140 : 100;

                        // find graphic textbox and label
                        lblFindGraphic.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 180 : lblFindRange.Visible ? 180 : 140 : 140;
                        txtFindGraphic.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 180 : lblFindRange.Visible ? 180 : 140 : 140;
                        btnTargetFindType.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 178 : lblFindRange.Visible ? 178 : 138 : 138;

                        // find color textbox and label
                        lblFindColor.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 220 : lblFindRange.Visible ? 220 : 180 : 180;
                        txtFindColor.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 220 : lblFindRange.Visible ? 220 : 180 : 180;

                        //store serial checkbox
                        chkFindStoreSerial.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 260 : lblFindRange.Visible ? 260 : 220 : 220;
                        lblFindStoreNote.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 285 : lblFindRange.Visible ? 285 : 245 : 245;

                        //ok button
                        btnOK.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 320 : lblFindRange.Visible ? 320 : 280 : 280;
                        //cancelbutton
                        btnCancel.Top = lblFindLocation.Visible ? lblFindContainer.Visible ? 320 : lblFindRange.Visible ? 320 : 280 : 280;

                        break;
                    case IfAction.ConditionType.InRange:
                        dialog.Height = lblRangeSerial.Visible ? 300 : lblRangeGraphic.Visible ? 340 : 240;

                        //range mode combobox and label
                        lblRangeMode.Top = 60;
                        cmbRangeMode.Top = 60;

                        //range serial textbox label and target button
                        lblRangeSerial.Top = 100;
                        txtRangeSerial.Top = 100;
                        btnTargetRangeSerial.Top = 98;


                        //range graphic textbox label and target button
                        lblRangeGraphic.Top = lblRangeSerial.Visible ? 140 : 100;
                        txtRangeGraphic.Top = lblRangeSerial.Visible ? 140 : 100;
                        btnTargetRangeType.Top = lblRangeSerial.Visible ? 138 : 98;

                        //range color label and textbox
                        lblRangeColor.Top = lblRangeSerial.Visible ? 180 : 140;
                        txtRangeColor.Top = lblRangeSerial.Visible ? 180 : 140;

                        //operator combobox and label
                        lblOp.Top = lblRangeSerial.Visible ? 140 : lblRangeGraphic.Visible ? 180 : 100;
                        cmbOp.Top = lblRangeSerial.Visible ? 140 : lblRangeGraphic.Visible ? 180 : 100;

                        //value token combobox and label
                        lblValue.Top = lblRangeSerial.Visible ? 180 : lblRangeGraphic.Visible ? 220 : 140;
                        txtValue.Top = lblRangeSerial.Visible ? 180 : lblRangeGraphic.Visible ? 220 : 140;




                        //ok button
                        btnOK.Top = lblRangeSerial.Visible ? 220 : lblRangeGraphic.Visible ? 260 : 160;

                        //cancelbutton
                        btnCancel.Top = lblRangeSerial.Visible ? 220 : lblRangeGraphic.Visible ? 260 : 160;

                        break;
                    case IfAction.ConditionType.TargetExists:
                        dialog.Height = 180;

                        lblBoolValue.Top = 60;
                        cmbBoolValue.Top = 60;


                        //ok button
                        btnOK.Top = 100;

                        //cancelbutton
                        btnCancel.Top = 100;
                        break;
                    case IfAction.ConditionType.Count:
                        dialog.Height = 380;

                        //count preset combobox and label and button
                        lblPreset.Top = 60;
                        cmbPreset.Top = 60;
                        btnSelectItem.Top = 95;

                        btnCreateList.Top = 95;
                        //graphic textbox and label
                        lblGraphic.Top = 140;
                        txtGraphic.Top = 140;
                        txtGraphic.Width = 290;

                        //color textbox and label
                        lblColor.Top = 180;
                        txtColor.Top = 180;

                        //operator combobox and label
                        lblOp.Top = 220;
                        cmbOp.Top = 220;

                        //value textbox and label
                        lblValue.Top = 260;
                        txtValue.Top = 260;

                        //ok button
                        btnOK.Top = 300;

                        //cancelbutton
                        btnCancel.Top = 300;





                        break;
                    case IfAction.ConditionType.Skill:
                        dialog.Height = 260;

                        lblSkill.Top = 60;
                        cmbSkill.Top = 60;

                        //operator combobox and label
                        lblOp.Top = 100;
                        cmbOp.Top = 100;

                        //value textbox and label
                        lblValue.Top = 140;
                        txtValue.Top = 140;


                        //ok button
                        btnOK.Top = 180;

                        //cancelbutton
                        btnCancel.Top = 180;

                        break;
                    case IfAction.ConditionType.InJournal:
                        dialog.Height = 220;
                        lblJournalText.Top = 60;
                        txtJournalText.Top = 60;
                        lblJournalCheck.Top = 100;
                        cmbJournalCheck.Top = 100;
                        //ok button
                        btnOK.Top = 140;
                        //cancelbutton
                        btnCancel.Top = 140;
                        break;
                    case IfAction.ConditionType.BuffExists:
                        dialog.Height = 220;
                        lblBuff.Top = 60;
                        cmbBuff.Top = 60;
                        lblBuffCheck.Top = 100;
                        cmbBuffCheck.Top = 100;
                        //ok button
                        btnOK.Top = 140;
                        //cancelbutton
                        btnCancel.Top = 140;
                        break;
                    default:
                        dialog.Height = 600;

                        //Player Stat comboxbox and label
                        lblStatType.Top = 60;
                        cmbStatType.Top = 60;

                        // Player Status comboxbox and label
                        lblStatusType.Top = 60;
                        cmbStatusType.Top = 60;

                        //range mode combobox and label
                        lblRangeMode.Top = 60;
                        cmbRangeMode.Top = 60;

                        //boolean combobox and label
                        lblBoolValue.Top = 100;
                        cmbBoolValue.Top = 100;

                        //range serial textbox label and target button
                        lblRangeSerial.Top = 100;
                        txtRangeSerial.Top = 100;
                        btnTargetRangeSerial.Top = 98;

                        //count preset combobox and label and button
                        lblPreset.Top = 60;
                        cmbPreset.Top = 60;
                        btnSelectItem.Top = 95;

                        //find mode combobox and label
                        lblFindMode.Top = 60;
                        cmbFindMode.Top = 60;

                        //find location combobox and label
                        lblFindLocation.Top = 100;
                        cmbFindLocation.Top = 100;

                        //find container serial comboxbox label and button
                        lblFindContainer.Top = 140;
                        txtFindContainer.Top = 140;
                        btnTargetContainer.Top = 138;

                        // find range textbox and label
                        lblFindRange.Top = 180;
                        txtFindRange.Top = 180;

                        // find graphic textbox and label
                        lblFindGraphic.Top = 220;
                        txtFindGraphic.Top = 220;
                        btnTargetFindType.Top = 218;

                        // find color textbox and label
                        lblFindColor.Top = 260;
                        txtFindColor.Top = 260;

                        //store serial checkbox
                        chkFindStoreSerial.Top = 300;
                        lblFindStoreNote.Top = 325;


                        //range graphic textbox label and target button
                        lblRangeGraphic.Top = 140;
                        txtRangeGraphic.Top = 140;
                        btnTargetRangeType.Top = 138;

                        //graphic textbox and label
                        lblGraphic.Top = 130;
                        txtGraphic.Top = 130;
                        txtGraphic.Width = 290;
                        //color textbox and label
                        lblColor.Top = 170;
                        txtColor.Top = 170;


                        //Range combobox and label
                        lblRangeMode.Top = 60;
                        cmbRangeMode.Top = 60;

                        //serial range
                        lblRangeSerial.Top = 100;
                        txtRangeSerial.Top = 100;

                        // type range graphic set 
                        lblRangeGraphic.Top = 140;
                        txtRangeGraphic.Top = 140;
                        btnTargetRangeType.Top = 138;

                        //Range color set
                        lblRangeColor.Top = 180;
                        txtRangeColor.Top = 180;

                        //operator combobox and label
                        lblOp.Top = 220;
                        cmbOp.Top = 220;

                        //max value combobox and label
                        lblValueToken.Top = 260;
                        cmbValueToken.Top = 260;

                        //value textbox and label
                        lblValue.Top = 300;
                        txtValue.Top = 300;

                        //journal text textbox and label
                        lblJournalText.Top = 60;
                        txtJournalText.Top = 60;

                        //journal check combobox and label
                        lblJournalCheck.Top = 100;
                        cmbJournalCheck.Top = 100;

                        //buff combobox and label
                        lblBuff.Top = 60;
                        cmbBuff.Top = 60;

                        //buff check combobox and label
                        lblBuffCheck.Top = 100;
                        cmbBuffCheck.Top = 100;


                        //ok button
                        btnOK.Left = 240;
                        btnOK.Top = 520;

                        //cancelbutton
                        btnCancel.Left = 340;
                        btnCancel.Top = 520;
                        break;
                }

            };

            cmbType.SelectedIndexChanged += (s, ev) => updateVisibility();
            cmbStatType.SelectedIndexChanged += (s, ev) => updateVisibility();
            cmbRangeMode.SelectedIndexChanged += (s, ev) => updateVisibility();
            cmbFindMode.SelectedIndexChanged += (s, ev) => updateVisibility();
            cmbFindLocation.SelectedIndexChanged += (s, ev) => updateVisibility();

            // Set operator
            switch (ifAction.Op)
            {
                case IfAction.Operator.GreaterThan: cmbOp.SelectedIndex = 0; break;
                case IfAction.Operator.LessThan: cmbOp.SelectedIndex = 1; break;
                case IfAction.Operator.Equal: cmbOp.SelectedIndex = 2; break;
                case IfAction.Operator.GreaterOrEqual: cmbOp.SelectedIndex = 3; break;
                case IfAction.Operator.LessOrEqual: cmbOp.SelectedIndex = 4; break;
                case IfAction.Operator.NotEqual: cmbOp.SelectedIndex = 5; break;
            }

            cmbType.SelectedItem = GetConditionTypeDisplayName(ifAction.Type);

            // === SET INITIAL VALUES BEFORE SETTING TYPE ===
            cmbStatType.SelectedIndex = (int)ifAction.StatType;
            cmbStatusType.SelectedIndex = (int)ifAction.StatusType;
            cmbRangeMode.SelectedIndex = (int)ifAction.RangeMode;

            // Set value token
            if (!string.IsNullOrEmpty(ifAction.ValueToken))
                cmbValueToken.SelectedItem = ifAction.ValueToken;
            else
                cmbValueToken.SelectedIndex = 0;

            // Set type LAST
            updateVisibility();



            // === ADD ALL CONTROLS ===
            dialog.Controls.Add(btnCreateList);
            dialog.Controls.Add(lblType);
            dialog.Controls.Add(cmbType);
            dialog.Controls.Add(lblStatType);
            dialog.Controls.Add(cmbStatType);
            dialog.Controls.Add(lblStatusType);
            dialog.Controls.Add(cmbStatusType);
            dialog.Controls.Add(lblRangeMode);
            dialog.Controls.Add(cmbRangeMode);
            dialog.Controls.Add(lblRangeSerial);
            dialog.Controls.Add(txtRangeSerial);
            dialog.Controls.Add(btnTargetRangeSerial);
            dialog.Controls.Add(lblRangeGraphic);
            dialog.Controls.Add(txtRangeGraphic);
            dialog.Controls.Add(btnTargetRangeType);
            dialog.Controls.Add(lblRangeColor);
            dialog.Controls.Add(txtRangeColor);
            dialog.Controls.Add(lblFindMode);
            dialog.Controls.Add(cmbFindMode);
            dialog.Controls.Add(lblFindLocation);
            dialog.Controls.Add(cmbFindLocation);
            dialog.Controls.Add(lblFindContainer);
            dialog.Controls.Add(txtFindContainer);
            dialog.Controls.Add(btnTargetContainer);
            dialog.Controls.Add(lblFindRange);
            dialog.Controls.Add(txtFindRange);
            dialog.Controls.Add(lblFindGraphic);
            dialog.Controls.Add(txtFindGraphic);
            dialog.Controls.Add(btnTargetFindType);
            dialog.Controls.Add(lblFindColor);
            dialog.Controls.Add(txtFindColor);
            dialog.Controls.Add(lblBoolValue);
            dialog.Controls.Add(cmbBoolValue);
            dialog.Controls.Add(lblOp);
            dialog.Controls.Add(cmbOp);
            dialog.Controls.Add(lblValueToken);
            dialog.Controls.Add(cmbValueToken);
            dialog.Controls.Add(lblValue);
            dialog.Controls.Add(txtValue);
            dialog.Controls.Add(lblPreset);
            dialog.Controls.Add(cmbPreset);
            dialog.Controls.Add(btnSelectItem);
            dialog.Controls.Add(lblGraphic);
            dialog.Controls.Add(txtGraphic);
            dialog.Controls.Add(lblColor);
            dialog.Controls.Add(txtColor);
            dialog.Controls.Add(lblSkill);
            dialog.Controls.Add(cmbSkill);
            dialog.Controls.Add(lblJournalText);
            dialog.Controls.Add(txtJournalText);
            dialog.Controls.Add(lblJournalCheck);
            dialog.Controls.Add(cmbJournalCheck);
            dialog.Controls.Add(lblBuff);
            dialog.Controls.Add(cmbBuff);
            dialog.Controls.Add(lblBuffCheck);
            dialog.Controls.Add(cmbBuffCheck);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.Controls.Add(chkFindStoreSerial);
            dialog.Controls.Add(lblFindStoreNote);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                IfAction.ConditionType condType = GetConditionTypeFromDisplayName(cmbType.SelectedItem.ToString());

                IfAction.Operator op = IfAction.Operator.GreaterThan;
                switch (cmbOp.SelectedIndex)
                {
                    case 0: op = IfAction.Operator.GreaterThan; break;
                    case 1: op = IfAction.Operator.LessThan; break;
                    case 2: op = IfAction.Operator.Equal; break;
                    case 3: op = IfAction.Operator.GreaterOrEqual; break;
                    case 4: op = IfAction.Operator.LessOrEqual; break;
                    case 5: op = IfAction.Operator.NotEqual; break;
                }

                string valueToken = "";
                int value = 0;

                if (condType == IfAction.ConditionType.InJournal)
                {
                    valueToken = txtJournalText.Text;
                    value = 0;
                }
                else if (cmbValueToken.SelectedItem != null && cmbValueToken.SelectedItem.ToString() != "None")
                {
                    valueToken = cmbValueToken.SelectedItem.ToString();
                    value = 0;
                }
                else
                {
                    int.TryParse(txtValue.Text, out value);
                }

                int graphic = 0;
                int color = -1;

                // If Find mode, use Find graphic/color fields
                if (condType == IfAction.ConditionType.Find)
                {
                    string findGraphicStr = txtFindGraphic.Text.Replace("0x", "").Replace("0X", "");
                    int.TryParse(findGraphicStr, System.Globalization.NumberStyles.HexNumber, null, out graphic);
                    int.TryParse(txtFindColor.Text, out color);
                }
                else
                {
                    // Use old graphic/color fields for Count
                    string graphicStr = txtGraphic.Text.Replace("0x", "").Replace("0X", "");
                    int.TryParse(graphicStr, System.Globalization.NumberStyles.HexNumber, null, out graphic);
                    int.TryParse(txtColor.Text, out color);
                }

                string skillName = cmbSkill.SelectedItem?.ToString() ?? "";
                string buffName = cmbBuff.SelectedItem?.ToString() ?? "";

                bool boolValue;
                if (condType == IfAction.ConditionType.BuffExists)
                {
                    boolValue = (cmbBuffCheck.SelectedIndex == 0);
                }
                else if (condType == IfAction.ConditionType.InJournal)
                {
                    boolValue = (cmbJournalCheck.SelectedIndex == 0);
                }
                else
                {
                    boolValue = (cmbBoolValue.SelectedIndex == 0);
                }

                string presetName = cmbPreset.SelectedItem?.ToString() ?? "Custom";

                IfAction.PlayerStatType statType = IfAction.PlayerStatType.HitPoints;
                if (condType == IfAction.ConditionType.PlayerStats && cmbStatType.SelectedIndex >= 0)
                {
                    statType = (IfAction.PlayerStatType)cmbStatType.SelectedIndex;
                }

                IfAction.PlayerStatusType statusType = IfAction.PlayerStatusType.Poisoned;
                if (condType == IfAction.ConditionType.PlayerStatus && cmbStatusType.SelectedIndex >= 0)
                {
                    statusType = (IfAction.PlayerStatusType)cmbStatusType.SelectedIndex;
                }

                // Parse InRange fields
                IfAction.InRangeMode rangeMode = (IfAction.InRangeMode)cmbRangeMode.SelectedIndex;

                int rangeSerial = 0;
                string rangeSerialStr = txtRangeSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                if (!string.IsNullOrEmpty(rangeSerialStr))
                {
                    int.TryParse(rangeSerialStr, System.Globalization.NumberStyles.HexNumber, null, out rangeSerial);
                }

                int rangeGraphic = 0;
                string rangeGraphicStr = txtRangeGraphic.Text.Replace("0x", "").Replace("0X", "").Trim();
                if (!string.IsNullOrEmpty(rangeGraphicStr))
                {
                    int.TryParse(rangeGraphicStr, System.Globalization.NumberStyles.HexNumber, null, out rangeGraphic);
                }

                int.TryParse(txtRangeColor.Text, out int rangeColor);

                // Parse Find fields
                IfAction.FindMode findEntityMode = (IfAction.FindMode)cmbFindMode.SelectedIndex;
                IfAction.FindLocation findEntityLocation = (IfAction.FindLocation)cmbFindLocation.SelectedIndex;

                int findContainerSerial = 0;
                string findContainerStr = txtFindContainer.Text.Replace("0x", "").Replace("0X", "").Trim();
                if (!string.IsNullOrEmpty(findContainerStr))
                {
                    int.TryParse(findContainerStr, System.Globalization.NumberStyles.HexNumber, null, out findContainerSerial);
                }

                int findRange = 2;
                int.TryParse(txtFindRange.Text, out findRange);

                bool findStoreSerial = chkFindStoreSerial.Checked;

                return (true, condType, op, value, graphic, color, skillName, valueToken, boolValue, presetName, buffName, statType, statusType, rangeMode, rangeSerial, rangeGraphic, rangeColor, findEntityMode, findEntityLocation, findContainerSerial, findRange, findStoreSerial);
            }

            return (false, ifAction.Type, ifAction.Op, ifAction.Value, ifAction.Graphic, ifAction.Color,
                    ifAction.SkillName, ifAction.ValueToken, ifAction.BooleanValue, ifAction.PresetName,
                    ifAction.BuffName, ifAction.StatType, ifAction.StatusType, ifAction.RangeMode,
                    ifAction.RangeSerial, ifAction.RangeGraphic, ifAction.RangeColor,
                    ifAction.FindEntityMode, ifAction.FindEntityLocation, ifAction.FindContainerSerial, ifAction.FindRange, ifAction.FindStoreSerial);
        }


        private void InsertElseIfMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var defaultElseIf = new ElseIfAction();
            var result = ShowElseIfConditionDialog(defaultElseIf);

            if (result.success)
            {
                var elseIfAction = new ElseIfAction(
                    result.type, result.op, result.value, result.graphic, result.color,
                    result.skillName, result.valueToken, result.booleanValue, result.presetName,
                    result.buffName, result.statType, result.statusType, result.rangeMode,
                    result.rangeSerial, result.rangeGraphic, result.rangeColor,
                    result.findEntityMode, result.findEntityLocation, result.findContainerSerial, result.findRange, result.findStoreSerial);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, elseIfAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted ElseIf condition at position {insertIndex + 1}", 88);
            }
        }
        private void EditElseIfMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.ElseIfAction elseIfAction)
            {
                var result = ShowElseIfConditionDialog(elseIfAction);

                if (result.success)
                {
                    elseIfAction.Type = result.type;
                    elseIfAction.StatType = result.statType;
                    elseIfAction.StatusType = result.statusType;
                    elseIfAction.Op = result.op;
                    elseIfAction.Value = result.value;
                    elseIfAction.ValueToken = result.valueToken;
                    elseIfAction.BooleanValue = result.booleanValue;
                    elseIfAction.Graphic = result.graphic;
                    elseIfAction.Color = result.color;
                    elseIfAction.SkillName = result.skillName;
                    elseIfAction.PresetName = result.presetName;
                    elseIfAction.BuffName = result.buffName;
                    elseIfAction.RangeMode = result.rangeMode;
                    elseIfAction.RangeSerial = result.rangeSerial;
                    elseIfAction.RangeGraphic = result.rangeGraphic;
                    elseIfAction.RangeColor = result.rangeColor;
                    elseIfAction.FindEntityMode = result.findEntityMode;
                    elseIfAction.FindEntityLocation = result.findEntityLocation;
                    elseIfAction.FindContainerSerial = result.findContainerSerial;
                    elseIfAction.FindRange = result.findRange;
                    elseIfAction.FindStoreSerial = result.findStoreSerial;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("ElseIf condition updated", 88);
                }
            }
        }
        private void EditElseIfAction(ElseIfAction elseIfAction, Macro macro, int actionIndex)
        {
            var result = ShowElseIfConditionDialog(elseIfAction);

            if (result.success)
            {
                elseIfAction.Type = result.type;
                elseIfAction.StatType = result.statType;
                elseIfAction.StatusType = result.statusType;
                elseIfAction.Op = result.op;
                elseIfAction.Value = result.value;
                elseIfAction.ValueToken = result.valueToken;
                elseIfAction.BooleanValue = result.booleanValue;
                elseIfAction.Graphic = result.graphic;
                elseIfAction.Color = result.color;
                elseIfAction.SkillName = result.skillName;
                elseIfAction.PresetName = result.presetName;
                elseIfAction.BuffName = result.buffName;
                elseIfAction.RangeMode = result.rangeMode;
                elseIfAction.RangeSerial = result.rangeSerial;
                elseIfAction.RangeGraphic = result.rangeGraphic;
                elseIfAction.RangeColor = result.rangeColor;
                elseIfAction.FindEntityMode = result.findEntityMode;
                elseIfAction.FindEntityLocation = result.findEntityLocation;
                elseIfAction.FindContainerSerial = result.findContainerSerial;
                elseIfAction.FindRange = result.findRange;
                elseIfAction.FindStoreSerial = result.findStoreSerial;

                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionElseIf()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.ElseIfAction;
            }

            return false;
        }
        private string FormatElseIfConditionDisplay(ElseIfAction elseIfAction)
        {
            if (elseIfAction.Type == IfAction.ConditionType.PlayerStats)
            {
                string statName = elseIfAction.StatType.ToString();
                string displayValue = string.IsNullOrEmpty(elseIfAction.ValueToken) ? elseIfAction.Value.ToString() : elseIfAction.ValueToken;
                return $"{statName} {GetOperatorSymbol(elseIfAction.Op)} {displayValue}";
            }

            if (elseIfAction.Type == IfAction.ConditionType.PlayerStatus)
            {
                string prefix = elseIfAction.BooleanValue ? "" : "Not ";
                return $"{prefix}{elseIfAction.StatusType}";
            }

            if (elseIfAction.Type == IfAction.ConditionType.TargetExists)
            {
                string prefix = elseIfAction.BooleanValue ? "" : "Not ";
                return $"{prefix}TargetExists";
            }

            // CHANGED FROM FindType TO Find
            if (elseIfAction.Type == IfAction.ConditionType.Find)
            {
                string modeStr = elseIfAction.FindEntityMode == IfAction.FindMode.Item ? "Item" : "Mobile";
                string locationStr = "";

                if (elseIfAction.FindEntityMode == IfAction.FindMode.Item)
                {
                    switch (elseIfAction.FindEntityLocation)
                    {
                        case IfAction.FindLocation.Backpack:
                            locationStr = "Backpack";
                            break;
                        case IfAction.FindLocation.Container:
                            locationStr = $"Container 0x{elseIfAction.FindContainerSerial:X8}";
                            break;
                        case IfAction.FindLocation.Ground:
                            locationStr = $"Ground ({elseIfAction.FindRange} tiles)";
                            break;
                    }
                }
                else
                {
                    locationStr = $"Range {elseIfAction.FindRange}";
                }

                string colorStr = elseIfAction.Color == -1 ? "Any" : $"0x{elseIfAction.Color:X4}";
                return $"Find {modeStr}: 0x{elseIfAction.Graphic:X4} ({colorStr}) in {locationStr}";
            }

            if (elseIfAction.Type == IfAction.ConditionType.InJournal)
            {
                string prefix = elseIfAction.BooleanValue ? "" : "Not ";
                string searchText = string.IsNullOrEmpty(elseIfAction.ValueToken) ? "(empty)" : elseIfAction.ValueToken;
                return $"{prefix}InJournal: \"{searchText}\"";
            }

            if (elseIfAction.Type == IfAction.ConditionType.BuffExists)
            {
                string prefix = elseIfAction.BooleanValue ? "" : "Not ";
                string buffName = string.IsNullOrEmpty(elseIfAction.BuffName) ? "(none)" : elseIfAction.BuffName;
                return $"{prefix}BuffExists: {buffName}";
            }

            if (elseIfAction.Type == IfAction.ConditionType.Skill)
            {
                string displayValue = string.IsNullOrEmpty(elseIfAction.ValueToken) ? elseIfAction.Value.ToString() : elseIfAction.ValueToken;
                return $"{elseIfAction.SkillName} {GetOperatorSymbol(elseIfAction.Op)} {displayValue}";
            }

            if (elseIfAction.Type == IfAction.ConditionType.InRange)
            {
                string targetDesc;
                switch (elseIfAction.RangeMode)
                {
                    case IfAction.InRangeMode.LastTarget:
                        targetDesc = "Last Target";
                        break;
                    case IfAction.InRangeMode.Serial:
                        targetDesc = elseIfAction.RangeSerial == 0 ? "(no serial)" : $"Serial 0x{elseIfAction.RangeSerial:X8}";
                        break;
                    case IfAction.InRangeMode.ItemType:
                        string itemColorStr = elseIfAction.RangeColor == -1 ? "Any" : $"0x{elseIfAction.RangeColor:X4}";
                        targetDesc = $"ItemType 0x{elseIfAction.RangeGraphic:X4} ({itemColorStr})";
                        break;
                    case IfAction.InRangeMode.MobileType:
                        string mobileColorStr = elseIfAction.RangeColor == -1 ? "Any" : $"0x{elseIfAction.RangeColor:X4}";
                        targetDesc = $"MobileType 0x{elseIfAction.RangeGraphic:X4} ({mobileColorStr})";
                        break;
                    default:
                        targetDesc = "Unknown";
                        break;
                }
                return $"{targetDesc} InRange {GetOperatorSymbol(elseIfAction.Op)} {elseIfAction.Value}";
            }

            if (elseIfAction.Type == IfAction.ConditionType.Count)
            {
                string presetDisplay = string.IsNullOrEmpty(elseIfAction.PresetName) || elseIfAction.PresetName == "Custom"
                    ? ""
                    : $"{elseIfAction.PresetName} ";
                string colorStr = elseIfAction.Color == -1 ? "Any" : $"0x{elseIfAction.Color:X4}";
                return $"Count: {presetDisplay}(0x{elseIfAction.Graphic:X4}, Color: {colorStr}) {GetOperatorSymbol(elseIfAction.Op)} {elseIfAction.Value}";
            }

            // Fallback
            string value = string.IsNullOrEmpty(elseIfAction.ValueToken) ? elseIfAction.Value.ToString() : elseIfAction.ValueToken;
            return $"{elseIfAction.Type} {GetOperatorSymbol(elseIfAction.Op)} {value}";
        }

        private (bool success, IfAction.ConditionType type, IfAction.Operator op, int value,
    int graphic, int color, string skillName, string valueToken, bool booleanValue,
    string presetName, string buffName, IfAction.PlayerStatType statType,
    IfAction.PlayerStatusType statusType, IfAction.InRangeMode rangeMode,
    int rangeSerial, int rangeGraphic, int rangeColor,
    IfAction.FindMode findEntityMode, IfAction.FindLocation findEntityLocation,
    int findContainerSerial, int findRange, bool findStoreSerial) ShowElseIfConditionDialog(ElseIfAction elseIfAction)
        {
            // Create a temporary IfAction to pass to the dialog
            var tempIf = new IfAction
            {
                Type = elseIfAction.Type,
                StatType = elseIfAction.StatType,
                StatusType = elseIfAction.StatusType,
                Op = elseIfAction.Op,
                Value = elseIfAction.Value,
                ValueToken = elseIfAction.ValueToken,
                BooleanValue = elseIfAction.BooleanValue,
                Graphic = elseIfAction.Graphic,
                Color = elseIfAction.Color,
                SkillName = elseIfAction.SkillName,
                PresetName = elseIfAction.PresetName,
                BuffName = elseIfAction.BuffName,
                RangeMode = elseIfAction.RangeMode,
                RangeSerial = elseIfAction.RangeSerial,
                RangeGraphic = elseIfAction.RangeGraphic,
                RangeColor = elseIfAction.RangeColor,
                FindEntityMode = elseIfAction.FindEntityMode,
                FindEntityLocation = elseIfAction.FindEntityLocation,
                FindContainerSerial = elseIfAction.FindContainerSerial,
                FindRange = elseIfAction.FindRange,
                FindStoreSerial = elseIfAction.FindStoreSerial
            };

            var result = ShowIfConditionDialog(tempIf);

            if (result.success)
            {
                return result;
            }

            return (false, elseIfAction.Type, elseIfAction.Op, elseIfAction.Value, elseIfAction.Graphic, elseIfAction.Color,
                elseIfAction.SkillName, elseIfAction.ValueToken, elseIfAction.BooleanValue, elseIfAction.PresetName,
                elseIfAction.BuffName, elseIfAction.StatType, elseIfAction.StatusType, elseIfAction.RangeMode,
                elseIfAction.RangeSerial, elseIfAction.RangeGraphic, elseIfAction.RangeColor,
                elseIfAction.FindEntityMode, elseIfAction.FindEntityLocation, elseIfAction.FindContainerSerial, elseIfAction.FindRange, elseIfAction.FindStoreSerial);
        }

        private void InsertEndWhileMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var endWhileAction = new RazorEnhanced.Macros.Actions.EndWhileAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, endWhileAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted EndWhile at position {insertIndex + 1}", 88);
        }
        private void InsertDisconnectMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            var disconnectAction = new RazorEnhanced.Macros.Actions.DisconnectAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, disconnectAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Disconnect action at position {insertIndex + 1}", 88);
        }
        private void InsertElseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var elseAction = new RazorEnhanced.Macros.Actions.ElseAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, elseAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Else at position {insertIndex + 1}", 88);
        }
        private void InsertEndForMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var endForAction = new RazorEnhanced.Macros.Actions.EndForAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, endForAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted EndFor at position {insertIndex + 1}", 88);
        }
        private void InsertResyncMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var resyncAction = new RazorEnhanced.Macros.Actions.ResyncAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, resyncAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Resync action at position {insertIndex + 1}", 88);
        }
        private void InsertClearJournalMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var clearJournalAction = new RazorEnhanced.Macros.Actions.ClearJournalAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, clearJournalAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Clear Journal action at position {insertIndex + 1}", 88);
        }
        private void InsertEndIfMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var endIfAction = new RazorEnhanced.Macros.Actions.EndIfAction();

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, endIfAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted EndIf at position {insertIndex + 1}", 88);
        }
        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            // Store a reference to the action - we'll clone it when pasting
            m_CopiedAction = action;

            Misc.SendMessage($"Copied: {action.GetActionName()}", 88);
        }
        private void PasteMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            if (m_CopiedAction == null)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            // Clone the action using serialize/deserialize
            MacroAction newAction = CloneAction(m_CopiedAction);

            if (newAction == null)
            {
                MessageBox.Show("Failed to copy action.", "Paste Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            macro.Actions.Insert(actionIndex + 1, newAction);

            // Refresh and save
            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            // Select the newly pasted action
            if (actionIndex + 1 < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[actionIndex + 1].Selected = true;
                macroActionsListView.EnsureVisible(actionIndex + 1);
            }

            Misc.SendMessage($"Pasted: {newAction.GetActionName()}", 88);
        }
        private MacroAction CloneAction(MacroAction original)
        {
            if (original == null) return null;

            try
            {
                // Get the serialized data
                string serialized = original.Serialize();

                // Create a new instance of the same type using parameterless constructor
                var actionType = original.GetType();
                var newAction = (MacroAction)Activator.CreateInstance(actionType);

                // Deserialize into the new instance
                newAction.Deserialize(serialized);

                // Verify the clone has the same data (optional debug check)
                if (newAction.Serialize() != serialized)
                {
                    Misc.SendMessage($"Warning: Clone mismatch for {original.GetActionName()}", 33);
                }

                return newAction;
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error cloning action: {ex.Message}", 33);
                return null;
            }
        }
        private void MoveUpMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex <= 0 || actionIndex >= macro.Actions.Count)
                return; // Can't move up if already at top

            // Swap with previous action
            var temp = macro.Actions[actionIndex];
            macro.Actions[actionIndex] = macro.Actions[actionIndex - 1];
            macro.Actions[actionIndex - 1] = temp;

            // Refresh and save
            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            // Select the moved action (now one position up)
            macroActionsListView.Items[actionIndex - 1].Selected = true;
            macroActionsListView.EnsureVisible(actionIndex - 1);
        }
        private void MoveDownMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count - 1)
                return; // Can't move down if already at bottom

            // Swap with next action
            var temp = macro.Actions[actionIndex];
            macro.Actions[actionIndex] = macro.Actions[actionIndex + 1];
            macro.Actions[actionIndex + 1] = temp;

            // Refresh and save
            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            // Select the moved action (now one position down)
            macroActionsListView.Items[actionIndex + 1].Selected = true;
            macroActionsListView.EnsureVisible(actionIndex + 1);
        }


        private void InsertForMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Prompt for number of iterations
            string input = PromptForInput(
                "Enter number of loop iterations:",
                "Insert For Loop",
                "5"
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int iterations) || iterations <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive number for iterations.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var forAction = new RazorEnhanced.Macros.Actions.ForAction(iterations);

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, forAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted For Loop ({iterations} iterations) at position {insertIndex + 1}", 88);
        }
        private void EditForMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.ForAction forAction)
            {
                string input = PromptForInput(
                    "Enter number of loop iterations:",
                    "Edit For Loop",
                    forAction.Iterations.ToString()
                );

                if (string.IsNullOrWhiteSpace(input))
                    return;

                if (!int.TryParse(input, out int iterations) || iterations <= 0)
                {
                    MessageBox.Show(
                        "Please enter a valid positive number for iterations.",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                forAction.Iterations = iterations;
                forAction.Reset(); // Reset current iteration

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (actionIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                }

                Misc.SendMessage($"For Loop updated to {iterations} iterations", 88);
            }
        }
        private void EditForAction(RazorEnhanced.Macros.Actions.ForAction forAction, Macro macro, int actionIndex)
        {
            string input = PromptForInput(
                "Enter number of loop iterations:",
                "Edit For Loop",
                forAction.Iterations.ToString()
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int iterations) || iterations <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive number for iterations.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            forAction.Iterations = iterations;
            forAction.Reset();
            RefreshAndSelectAction(macro, actionIndex);
        }
        private bool IsSelectedActionFor()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.ForAction;
            }

            return false;
        }


        private void InsertPauseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Prompt for pause duration
            string input = PromptForInput("Enter pause duration in milliseconds:", "Insert Pause", "1000");

            if (string.IsNullOrWhiteSpace(input))
                return; // User cancelled

            if (!int.TryParse(input, out int milliseconds) || milliseconds <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive number for milliseconds.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Get insertion position (handles empty list)
            int insertIndex = GetInsertPosition();

            // Insert pause at calculated position
            var pauseAction = new RazorEnhanced.Macros.Actions.PauseAction(milliseconds);
            macro.Actions.Insert(insertIndex, pauseAction);

            // Refresh and save
            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            // Select the newly inserted pause
            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Pause at position {insertIndex + 1}", 88);
        }
        private void EditPauseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.PauseAction pauseAction)
            {
                // Prompt for new pause duration
                string input = PromptForInput(
                    "Enter pause duration in milliseconds:",
                    "Edit Pause",
                    pauseAction.Milliseconds.ToString()
                );

                if (string.IsNullOrWhiteSpace(input))
                    return; // User cancelled

                if (!int.TryParse(input, out int milliseconds) || milliseconds <= 0)
                {
                    MessageBox.Show(
                        "Please enter a valid positive number for milliseconds.",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // Update the pause duration
                pauseAction.Milliseconds = milliseconds;

                // Refresh and save
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                // Keep the same action selected
                if (actionIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                }
            }
        }
        private void EditPauseAction(RazorEnhanced.Macros.Actions.PauseAction pauseAction, Macro macro, int actionIndex)
        {
            string input = PromptForInput(
                "Enter pause duration in milliseconds:",
                "Edit Pause",
                pauseAction.Milliseconds.ToString()
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int milliseconds) || milliseconds <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive number for milliseconds.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            pauseAction.Milliseconds = milliseconds;
            RefreshAndSelectAction(macro, actionIndex);
        }
        private bool IsSelectedActionPause()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.PauseAction;
            }

            return false;
        }


        private void InsertSetAliasMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowSetAliasDialog("myalias", 0, false);

            if (result.success)
            {
                var setAliasAction = new RazorEnhanced.Macros.Actions.SetAliasAction(result.aliasName, result.serial, result.useFoundSerial);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, setAliasAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Set Alias: {result.aliasName} at position {insertIndex + 1}", 88);
            }
        }
        private void EditSetAliasMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.SetAliasAction setAliasAction)
            {
                var result = ShowSetAliasDialog(setAliasAction.AliasName, setAliasAction.Serial, setAliasAction.UseFoundSerial);

                if (result.success)
                {
                    setAliasAction.AliasName = result.aliasName;
                    setAliasAction.Serial = result.serial;
                    setAliasAction.UseFoundSerial = result.useFoundSerial;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Set Alias action updated", 88);
                }
            }
        }
        private void EditSetAliasAction(RazorEnhanced.Macros.Actions.SetAliasAction setAliasAction, Macro macro, int actionIndex)
        {
            var result = ShowSetAliasDialog(setAliasAction.AliasName, setAliasAction.Serial, setAliasAction.UseFoundSerial);

            if (result.success)
            {
                setAliasAction.AliasName = result.aliasName;
                setAliasAction.Serial = result.serial;
                setAliasAction.UseFoundSerial = result.useFoundSerial;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionSetAlias()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.SetAliasAction;
            }

            return false;
        }
        private (bool success, string aliasName, int serial, bool useFoundSerial) ShowSetAliasDialog(string aliasName, int serial, bool useFoundSerial)
        {
            Form dialog = new Form
            {
                Width = 450,
                Height = 320,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Set Alias",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Alias Name
            Label lblAliasName = new Label { Left = 20, Top = 20, Text = "Alias Name:", Width = 100 };
            TextBox txtAliasName = new TextBox
            {
                Left = 130,
                Top = 20,
                Width = 290,
                Text = aliasName ?? "myalias"
            };

            // Use 'findfound' checkbox
            CheckBox chkUseFound = new CheckBox
            {
                Left = 130,
                Top = 55,
                Width = 290,
                Text = "Use 'findfound' serial (from Find condition)",
                Checked = useFoundSerial
            };

            // Serial input
            Label lblSerial = new Label { Left = 20, Top = 90, Text = "Serial (hex):", Width = 100 };
            TextBox txtSerial = new TextBox
            {
                Left = 130,
                Top = 90,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}",
                Enabled = !useFoundSerial
            };

            // Target button
            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 340,
                Top = 88,
                Width = 80,
                Enabled = !useFoundSerial
            };

            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an entity to set the alias...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {

                            int serialValue = (int)targetSerial;
                            txtSerial.Text = $"0x{serialValue:X8}";

                            var mobile = Mobiles.FindBySerial(serialValue);
                            if (mobile != null)
                            {
                                Misc.SendMessage($"Set to mobile: {mobile.Name} (0x{serialValue:X8})", 88);
                            }
                            else
                            {
                                var item = Items.FindBySerial(serialValue);
                                if (item != null)
                                {
                                    Misc.SendMessage($"Set to item: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                                }
                                else
                                {
                                    Misc.SendMessage($"Set to serial: 0x{serialValue:X8}", 88);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Checkbox change handler
            chkUseFound.CheckedChanged += (s, ev) =>
            {
                bool useFound = chkUseFound.Checked;
                txtSerial.Enabled = !useFound;
                btnTarget.Enabled = !useFound;

                if (useFound)
                {
                    txtSerial.Text = "(will use 'findfound' at runtime)";
                }
                else
                {
                    txtSerial.Text = serial == 0 ? "" : $"0x{serial:X8}";
                }
            };

            // Info label
            Label lblInfo = new Label
            {
                Left = 130,
                Top = 125,
                Width = 290,
                Height = 80,
                Text = "Tip: Use 'findfound' to get the serial from a Find condition.\n\n" +
                       "Note: Setting 'found' alias will also update 'findfound' for UOSteam compatibility.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 230,
                Width = 90,
                Top = 230,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 330,
                Width = 90,
                Top = 230,
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblAliasName);
            dialog.Controls.Add(txtAliasName);
            dialog.Controls.Add(chkUseFound);
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblInfo);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Validate alias name
                if (string.IsNullOrWhiteSpace(txtAliasName.Text))
                {
                    MessageBox.Show("Alias name cannot be empty.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, aliasName, serial, useFoundSerial);
                }

                string newAliasName = txtAliasName.Text.Trim().ToLower();
                bool newUseFound = chkUseFound.Checked;
                int newSerial = 0;

                // Parse serial if not using findfound
                if (!newUseFound)
                {
                    string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                    if (!string.IsNullOrEmpty(serialStr))
                    {
                        if (!int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                        {
                            MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, aliasName, serial, useFoundSerial);
                        }
                    }
                }

                return (true, newAliasName, newSerial, newUseFound);
            }

            return (false, aliasName, serial, useFoundSerial);
        }


        private void InsertRemoveAliasMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowRemoveAliasDialog("myalias");

            if (result.success)
            {
                var removeAliasAction = new RazorEnhanced.Macros.Actions.RemoveAliasAction(result.aliasName);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, removeAliasAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Remove Alias: {result.aliasName} at position {insertIndex + 1}", 88);
            }
        }
        private void EditRemoveAliasMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.RemoveAliasAction removeAliasAction)
            {
                var result = ShowRemoveAliasDialog(removeAliasAction.AliasName);

                if (result.success)
                {
                    removeAliasAction.AliasName = result.aliasName;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Remove Alias action updated", 88);
                }
            }
        }
        private void EditRemoveAliasAction(RazorEnhanced.Macros.Actions.RemoveAliasAction removeAliasAction, Macro macro, int actionIndex)
        {
            var result = ShowRemoveAliasDialog(removeAliasAction.AliasName);

            if (result.success)
            {
                removeAliasAction.AliasName = result.aliasName;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionRemoveAlias()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.RemoveAliasAction;
            }

            return false;
        }
        private (bool success, string aliasName) ShowRemoveAliasDialog(string aliasName)
        {
            Form dialog = new Form
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Remove Alias",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblAliasName = new Label { Left = 20, Top = 20, Text = "Alias Name:", Width = 100 };
            TextBox txtAliasName = new TextBox
            {
                Left = 130,
                Top = 20,
                Width = 240,
                Text = aliasName ?? "myalias"
            };

            Label lblInfo = new Label
            {
                Left = 130,
                Top = 55,
                Width = 240,
                Height = 40,
                Text = "Removes the alias from shared values",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 180,
                Width = 90,
                Top = 110,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 280,
                Width = 90,
                Top = 110,
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblAliasName);
            dialog.Controls.Add(txtAliasName);
            dialog.Controls.Add(lblInfo);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtAliasName.Text))
                {
                    MessageBox.Show("Alias name cannot be empty.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, aliasName);
                }

                string newAliasName = txtAliasName.Text.Trim().ToLower();
                return (true, newAliasName);
            }

            return (false, aliasName);
        }


        private void InsertUseEmoteMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            string[] emotes = { "bow", "salute" };
            string selectedEmote = ShowEmoteDialog("bow", emotes);

            if (!string.IsNullOrEmpty(selectedEmote))
            {
                var useEmoteAction = new RazorEnhanced.Macros.Actions.UseEmoteAction(selectedEmote);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, useEmoteAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Use Emote: {selectedEmote} at position {insertIndex + 1}", 88);
            }
        }
        private void EditUseEmoteMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.UseEmoteAction useEmoteAction)
            {
                string[] emotes = { "bow", "salute" };
                string newEmote = ShowEmoteDialog(useEmoteAction.EmoteName, emotes);

                if (!string.IsNullOrEmpty(newEmote))
                {
                    useEmoteAction.EmoteName = newEmote;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage($"Emote changed to: {newEmote}", 88);
                }
            }
        }
        private void EditUseEmoteAction(RazorEnhanced.Macros.Actions.UseEmoteAction useEmoteAction, Macro macro, int actionIndex)
        {
            string[] emotes = { "bow", "salute" };
            string newEmote = ShowEmoteDialog(useEmoteAction.EmoteName, emotes);

            if (!string.IsNullOrEmpty(newEmote))
            {
                useEmoteAction.EmoteName = newEmote;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionUseEmote()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.UseEmoteAction;
            }

            return false;
        }
        private string ShowEmoteDialog(string currentEmote, string[] emotes)
        {
            Form dialog = new Form
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Select Emote",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblEmote = new Label
            {
                Left = 20,
                Top = 20,
                Text = "Choose emote:",
                Width = 250
            };

            ComboBox cmbEmote = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEmote.Items.AddRange(emotes);
            cmbEmote.SelectedItem = currentEmote;
            if (cmbEmote.SelectedIndex == -1) cmbEmote.SelectedIndex = 0; // Default to first emote

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 110,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 200,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.Cancel
            };

            btnOK.Click += (s, ev) => { dialog.Close(); };
            btnCancel.Click += (s, ev) => { dialog.Close(); };

            dialog.Controls.Add(lblEmote);
            dialog.Controls.Add(cmbEmote);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            return dialog.ShowDialog() == DialogResult.OK ? cmbEmote.SelectedItem.ToString() : string.Empty;
        }


        private void InsertCommentMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Prompt for comment text
            string input = PromptForInput(
                "Enter comment text (for documentation only):",
                "Insert Comment",
                "// Add your comment here"
            );

            if (input == null || input == string.Empty) // User cancelled (null) or provided empty string
                return;

            // Remove leading // if user added it
            string cleanComment = input.TrimStart().StartsWith("//")
                ? input.TrimStart().Substring(2).TrimStart()
                : input;

            // Get insertion position (handles empty list)
            int insertIndex = GetInsertPosition();

            // Insert Comment at calculated position
            var commentAction = new RazorEnhanced.Macros.Actions.CommentAction(cleanComment);
            macro.Actions.Insert(insertIndex, commentAction);

            // Refresh and save
            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            // Select the newly inserted action
            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Comment at position {insertIndex + 1}", 88);
        }
        private void EditCommentMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.CommentAction commentAction)
            {
                // Prompt for new comment text
                string input = PromptForInput(
                    "Enter comment text:",
                    "Edit Comment",
                    commentAction.Comment
                );

                if (input != null) // User didn't cancel
                {
                    commentAction.Comment = input;

                    // Refresh and save
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    // Keep the same action selected
                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Comment updated", 88);
                }
            }
        }
        private void EditCommentAction(RazorEnhanced.Macros.Actions.CommentAction commentAction, Macro macro, int actionIndex)
        {
            string input = PromptForInput(
                "Enter comment text:",
                "Edit Comment",
                commentAction.Comment
            );

            if (input != null)
            {
                commentAction.Comment = input;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionComment()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.CommentAction;
            }

            return false;
        }


        private void InsertToggleWarModeMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowToggleWarModeDialog(true);

            if (result.success)
            {
                var toggleWarModeAction = new RazorEnhanced.Macros.Actions.ToggleWarModeAction(result.warMode);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, toggleWarModeAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                string modeText = result.warMode ? "ON" : "OFF";
                Misc.SendMessage($"Inserted Toggle War Mode: {modeText} at position {insertIndex + 1}", 88);
            }
        }
        private void EditToggleWarModeMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.ToggleWarModeAction toggleWarModeAction)
            {
                var result = ShowToggleWarModeDialog(toggleWarModeAction.WarMode);

                if (result.success)
                {
                    toggleWarModeAction.WarMode = result.warMode;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    string modeText = result.warMode ? "ON" : "OFF";
                    Misc.SendMessage($"War Mode changed to: {modeText}", 88);
                }
            }
        }
        private void EditToggleWarModeAction(RazorEnhanced.Macros.Actions.ToggleWarModeAction toggleWarModeAction, Macro macro, int actionIndex)
        {
            var result = ShowToggleWarModeDialog(toggleWarModeAction.WarMode);

            if (result.success)
            {
                toggleWarModeAction.WarMode = result.warMode;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionToggleWarMode()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.ToggleWarModeAction;
            }

            return false;
        }
        private (bool success, bool warMode) ShowToggleWarModeDialog(bool currentWarMode)
        {
            Form dialog = new Form
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Toggle War Mode",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblWarMode = new Label
            {
                Left = 20,
                Top = 20,
                Text = "War Mode State:",
                Width = 250
            };

            ComboBox cmbWarMode = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbWarMode.Items.AddRange(new string[] { "ON (War)", "OFF (Peace)" });
            cmbWarMode.SelectedIndex = currentWarMode ? 0 : 1;

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 110,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 200,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.Cancel
            };

            btnOK.Click += (s, ev) => { dialog.Close(); };
            btnCancel.Click += (s, ev) => { dialog.Close(); };

            dialog.Controls.Add(lblWarMode);
            dialog.Controls.Add(cmbWarMode);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool warMode = cmbWarMode.SelectedIndex == 0;
                return (true, warMode);
            }

            return (false, currentWarMode);
        }

        private void InsertFlyMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowFlyDialog(true);

            if (result.success)
            {
                var flyAction = new RazorEnhanced.Macros.Actions.FlyAction(result.flying);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, flyAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                string modeText = result.flying ? "ON" : "OFF";
                Misc.SendMessage($"Inserted Fly: {modeText} at position {insertIndex + 1}", 88);
            }
        }
        private void EditFlyMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.FlyAction flyAction)
            {
                var result = ShowFlyDialog(flyAction.Flying);

                if (result.success)
                {
                    flyAction.Flying = result.flying;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    string modeText = result.flying ? "ON" : "OFF";
                    Misc.SendMessage($"Fly changed to: {modeText}", 88);
                }
            }
        }
        private void EditFlyAction(RazorEnhanced.Macros.Actions.FlyAction flyAction, Macro macro, int actionIndex)
        {
            var result = ShowFlyDialog(flyAction.Flying);

            if (result.success)
            {
                flyAction.Flying = result.flying;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionFly()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.FlyAction;
            }

            return false;
        }
        private (bool success, bool flying) ShowFlyDialog(bool currentFlying)
        {
            Form dialog = new Form
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Gargoyle Fly",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblFly = new Label
            {
                Left = 20,
                Top = 20,
                Text = "Flying State:",
                Width = 250
            };

            ComboBox cmbFly = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFly.Items.AddRange(new string[] { "ON (Flying)", "OFF (Ground)" });
            cmbFly.SelectedIndex = currentFlying ? 0 : 1;

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 110,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 200,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.Cancel
            };

            btnOK.Click += (s, ev) => { dialog.Close(); };
            btnCancel.Click += (s, ev) => { dialog.Close(); };

            dialog.Controls.Add(lblFly);
            dialog.Controls.Add(cmbFly);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool flying = cmbFly.SelectedIndex == 0;
                return (true, flying);
            }

            return (false, currentFlying);
        }


        private void InsertAttackEntityMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowAttackEntityDialog(
                AttackAction.AttackMode.LastTarget,
                0,
                "",
                AttackAction.NotorietyFilter.Any,
                -1,
                0,
                -1,
                "Nearest"
            );

            if (result.success)
            {
                var attackEntityAction = new RazorEnhanced.Macros.Actions.AttackAction(
                    result.mode,
                    result.serial,
                    result.aliasName,
                    result.notoriety,
                    result.range,
                    result.graphic,
                    result.color,
                    result.selector
                );

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, attackEntityAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Attack at position {insertIndex + 1}", 88);
            }
        }
        private void EditAttackEntityMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.AttackAction attackEntityAction)
            {
                var result = ShowAttackEntityDialog(
                    attackEntityAction.Mode,
                    attackEntityAction.Serial,
                    attackEntityAction.AliasName,
                    attackEntityAction.Notoriety,
                    attackEntityAction.Range,
                    attackEntityAction.Graphic,
                    attackEntityAction.Color,
                    attackEntityAction.Selector
                );

                if (result.success)
                {
                    attackEntityAction.Mode = result.mode;
                    attackEntityAction.Serial = result.serial;
                    attackEntityAction.AliasName = result.aliasName;
                    attackEntityAction.Notoriety = result.notoriety;
                    attackEntityAction.Range = result.range;
                    attackEntityAction.Graphic = result.graphic;
                    attackEntityAction.Color = result.color;
                    attackEntityAction.Selector = result.selector;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Attack action updated", 88);
                }
            }
        }
        private string GetNotorietyDisplayName(AttackAction.NotorietyFilter notoriety)
        {
            switch (notoriety)
            {
                case AttackAction.NotorietyFilter.Any: return "Any";
                case AttackAction.NotorietyFilter.Blue: return "Blue";
                case AttackAction.NotorietyFilter.Green: return "Green";
                case AttackAction.NotorietyFilter.Grey: return "Grey";
                case AttackAction.NotorietyFilter.GreyAggro: return "Criminal";
                case AttackAction.NotorietyFilter.Orange: return "Orange";
                case AttackAction.NotorietyFilter.Red: return "Red";
                case AttackAction.NotorietyFilter.Yellow: return "Yellow";
                case AttackAction.NotorietyFilter.Friendly: return "Friendly";
                case AttackAction.NotorietyFilter.NonFriendly: return "NonFriendly";
                default: return "Any";
            }
        }
        private void EditAttackEntityAction(RazorEnhanced.Macros.Actions.AttackAction attackEntityAction, Macro macro, int actionIndex)
        {
            var result = ShowAttackEntityDialog(
                attackEntityAction.Mode,
                attackEntityAction.Serial,
                attackEntityAction.AliasName,
                attackEntityAction.Notoriety,
                attackEntityAction.Range,
                attackEntityAction.Graphic,
                attackEntityAction.Color,
                attackEntityAction.Selector
            );

            if (result.success)
            {
                attackEntityAction.Mode = result.mode;
                attackEntityAction.Serial = result.serial;
                attackEntityAction.AliasName = result.aliasName;
                attackEntityAction.Notoriety = result.notoriety;
                attackEntityAction.Range = result.range;
                attackEntityAction.Graphic = result.graphic;
                attackEntityAction.Color = result.color;
                attackEntityAction.Selector = result.selector;

                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionAttackEntity()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.AttackAction;
            }

            return false;
        }
        private (bool success, AttackAction.AttackMode mode, int serial, string aliasName, AttackAction.NotorietyFilter notoriety, int range, int graphic, int color, string selector) ShowAttackEntityDialog(
    AttackAction.AttackMode mode, int serial, string aliasName, AttackAction.NotorietyFilter notoriety, int range, int graphic = 0, int color = -1, string selector = "Nearest")
        {
            Form dialog = new Form
            {
                Width = 450,
                Height = 450,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Attack",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Attack Mode selector
            Label lblMode = new Label { Left = 20, Top = 20, Text = "Attack Mode:", Width = 100 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 130,
                Top = 20,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Last Target", "Specific Serial", "Alias", "Nearest", "Farthest", "By Type" });
            cmbMode.SelectedIndex = (int)mode;

            // Serial input
            Label lblSerial = new Label { Left = 20, Top = 60, Text = "Serial (hex):", Width = 100 };
            TextBox txtSerial = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}"
            };

            Button btnTargetSerial = new Button
            {
                Text = "Target",
                Left = 340,
                Top = 58,
                Width = 80
            };

            // Alias input
            Label lblAlias = new Label { Left = 20, Top = 60, Text = "Alias Name:", Width = 100 };
            TextBox txtAlias = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 290,
                Text = aliasName ?? ""
            };

            Label lblAliasNote = new Label
            {
                Left = 130,
                Top = 95,
                Width = 290,
                Height = 30,
                Text = "Use 'findfound', 'enemy', or any custom alias",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Notoriety filter for Nearest/Farthest
            Label lblNotoriety = new Label { Left = 20, Top = 60, Text = "Notoriety Filter:", Width = 100 };
            ComboBox cmbNotoriety = new ComboBox
            {
                Left = 130,
                Top = 60,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNotoriety.Items.AddRange(new string[]
            {
        "Any",
        "Blue (Innocent)",
        "Green (Friend/Guild)",
        "Grey (Neutral)",
        "Grey (Criminal)",
        "Orange (Attacker)",
        "Red (Murderer)",
        "Yellow (Invulnerable)",
        "Friendly (Blue + Green)",
        "NonFriendly (Grey + Orange + Red)"
            });
            cmbNotoriety.SelectedIndex = (int)notoriety;

            // By Type: Graphic input
            Label lblGraphic = new Label { Left = 20, Top = 60, Text = "Graphic (hex):", Width = 100 };
            TextBox txtGraphic = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 200,
                Text = graphic == 0 ? "" : $"0x{graphic:X4}"
            };

            Button btnTargetType = new Button
            {
                Text = "Target",
                Left = 340,
                Top = 58,
                Width = 80
            };

            // By Type: Color input
            Label lblColor = new Label { Left = 20, Top = 100, Text = "Color (-1 = any):", Width = 100 };
            TextBox txtColor = new TextBox
            {
                Left = 130,
                Top = 100,
                Width = 290,
                Text = color.ToString()
            };

            // By Type: Selector
            Label lblSelector = new Label { Left = 20, Top = 140, Text = "Selector:", Width = 100 };
            ComboBox cmbSelector = new ComboBox
            {
                Left = 130,
                Top = 140,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSelector.Items.AddRange(new string[] { "Nearest", "Farthest", "Random" });
            cmbSelector.SelectedItem = selector;
            if (cmbSelector.SelectedIndex == -1) cmbSelector.SelectedIndex = 0;

            // Range (for Nearest/Farthest/ByType)
            Label lblRange = new Label { Left = 20, Top = 180, Text = "Range (-1 = any):", Width = 100 };
            TextBox txtRange = new TextBox
            {
                Left = 130,
                Top = 180,
                Width = 290,
                Text = range.ToString()
            };

            // Target Serial button handler
            btnTargetSerial.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a mobile to attack...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        var mobile = Mobiles.FindBySerial((int)targetSerial);
                        if (mobile != null)
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                int serialValue = (int)targetSerial;
                                txtSerial.Text = $"0x{serialValue:X8}";
                                Misc.SendMessage($"Set to mobile: 0x{serialValue:X8}", 88);
                            }));
                        }
                        else
                        {
                            Misc.SendMessage("Target must be a mobile.", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Target Type button handler (for By Type mode)
            btnTargetType.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a mobile to get its type...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        var mobile = Mobiles.FindBySerial((int)targetSerial);
                        if (mobile != null)
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtGraphic.Text = $"0x{mobile.Body:X4}";
                                txtColor.Text = mobile.Color.ToString();
                                Misc.SendMessage($"Set to mobile type: 0x{mobile.Body:X4}, Color: {mobile.Color}", 88);
                            }));
                        }
                        else
                        {
                            Misc.SendMessage("Target must be a mobile.", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 230,
                Width = 90,
                Top = 390,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 330,
                Width = 90,
                Top = 390,
                DialogResult = DialogResult.Cancel
            };

            // Visibility logic
            Action updateVisibility = () =>
            {
                var selectedMode = (AttackAction.AttackMode)cmbMode.SelectedIndex;

                bool isLastTarget = selectedMode == AttackAction.AttackMode.LastTarget;
                bool isSerial = selectedMode == AttackAction.AttackMode.Serial;
                bool isAlias = selectedMode == AttackAction.AttackMode.Alias;
                bool isNearestOrFarthest = selectedMode == AttackAction.AttackMode.Nearest ||
                                           selectedMode == AttackAction.AttackMode.Farthest;
                bool isByType = selectedMode == AttackAction.AttackMode.ByType;

                // Serial mode controls
                lblSerial.Visible = txtSerial.Visible = btnTargetSerial.Visible = isSerial;

                // Alias mode controls
                lblAlias.Visible = txtAlias.Visible = lblAliasNote.Visible = isAlias;

                // Notoriety (for Nearest/Farthest only)
                lblNotoriety.Visible = cmbNotoriety.Visible = isNearestOrFarthest;

                // By Type controls
                lblGraphic.Visible = txtGraphic.Visible = btnTargetType.Visible = isByType;
                lblColor.Visible = txtColor.Visible = isByType;
                lblSelector.Visible = cmbSelector.Visible = isByType;

                // Range (for Nearest/Farthest/ByType)
                bool showRange = isNearestOrFarthest || isByType;
                lblRange.Visible = txtRange.Visible = showRange;

                switch(selectedMode)
                {
                    case AttackAction.AttackMode.LastTarget:
                        dialog.Height = 140;
                        btnOK.Top = 60;
                        btnCancel.Top = 60;
                        break;
                    case AttackAction.AttackMode.Serial:
                        dialog.Height = 180;

                        btnOK.Top = 100;
                        btnCancel.Top = 100;
                        break;
                    case AttackAction.AttackMode.Alias:
                        dialog.Height = 220;

                        btnOK.Top = 140;
                        btnCancel.Top = 140;

                        break;
                    case AttackAction.AttackMode.Nearest:
                    case AttackAction.AttackMode.Farthest:
                        dialog.Height = 220;

                        // Range (for Nearest/Farthest/ByType)
                        lblRange.Top = 100;
                        txtRange.Top = 100;
                        btnOK.Top = 140;
                        btnCancel.Top = 140;
                        break;
                    case AttackAction.AttackMode.ByType:
                        dialog.Height = 300;

                        btnOK.Top = 220;
                        btnCancel.Top = 220;
                        break;
                    default:
                        dialog.Height = 450;

                        // Range (for Nearest/Farthest/ByType)
                        lblRange.Top = 180;
                        txtRange.Top = 180;
                        btnOK.Top = 390;
                        btnCancel.Top = 390;
                        break;
                }



            };

            cmbMode.SelectedIndexChanged += (s, ev) => updateVisibility();
            updateVisibility(); // Initial visibility



            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTargetSerial);
            dialog.Controls.Add(lblAlias);
            dialog.Controls.Add(txtAlias);
            dialog.Controls.Add(lblAliasNote);
            dialog.Controls.Add(lblNotoriety);
            dialog.Controls.Add(cmbNotoriety);
            dialog.Controls.Add(lblGraphic);
            dialog.Controls.Add(txtGraphic);
            dialog.Controls.Add(btnTargetType);
            dialog.Controls.Add(lblColor);
            dialog.Controls.Add(txtColor);
            dialog.Controls.Add(lblSelector);
            dialog.Controls.Add(cmbSelector);
            dialog.Controls.Add(lblRange);
            dialog.Controls.Add(txtRange);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newMode = (AttackAction.AttackMode)cmbMode.SelectedIndex;
                int newSerial = 0;
                string newAliasName = "";
                var newNotoriety = (AttackAction.NotorietyFilter)cmbNotoriety.SelectedIndex;
                int newRange = -1;
                int newGraphic = 0;
                int newColor = -1;
                string newSelector = "Nearest";

                // Parse based on mode
                switch (newMode)
                {
                    case AttackAction.AttackMode.Serial:
                        string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (!string.IsNullOrEmpty(serialStr))
                        {
                            if (!int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                            {
                                MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345).",
                                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Serial cannot be empty for Serial mode.",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                        }
                        break;

                    case AttackAction.AttackMode.Alias:
                        if (string.IsNullOrWhiteSpace(txtAlias.Text))
                        {
                            MessageBox.Show("Alias name cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                        }
                        newAliasName = txtAlias.Text.Trim().ToLower();
                        break;

                    case AttackAction.AttackMode.Nearest:
                    case AttackAction.AttackMode.Farthest:
                        if (!int.TryParse(txtRange.Text, out newRange))
                        {
                            MessageBox.Show("Invalid range value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                        }
                        break;

                    case AttackAction.AttackMode.ByType:
                        // Parse graphic
                        string graphicStr = txtGraphic.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (string.IsNullOrEmpty(graphicStr) || !int.TryParse(graphicStr, System.Globalization.NumberStyles.HexNumber, null, out newGraphic))
                        {
                            MessageBox.Show("Invalid graphic value. Please enter a hex value (e.g., 0x0001).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                        }

                        // Parse color
                        if (!int.TryParse(txtColor.Text, out newColor))
                        {
                            MessageBox.Show("Invalid color value. Please enter a number (-1 for any color).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                        }

                        // Parse range
                        if (!int.TryParse(txtRange.Text, out newRange))
                        {
                            MessageBox.Show("Invalid range value. Please enter a number (-1 for unlimited).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
                        }

                        newSelector = cmbSelector.SelectedItem.ToString();
                        break;
                }

                return (true, newMode, newSerial, newAliasName, newNotoriety, newRange, newGraphic, newColor, newSelector);
            }

            return (false, mode, serial, aliasName, notoriety, range, graphic, color, selector);
        }

        private void InsertInvokeVirtueMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            string[] virtues = { "Honor", "Sacrifice", "Valor", "Compassion", "Honesty", "Humility", "Justice", "Spirituality" };
            string selectedVirtue = ShowVirtueDialog("Honor", virtues);

            if (!string.IsNullOrEmpty(selectedVirtue))
            {
                var invokeVirtueAction = new RazorEnhanced.Macros.Actions.InvokeVirtueAction(selectedVirtue);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, invokeVirtueAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Invoke Virtue: {selectedVirtue} at position {insertIndex + 1}", 88);
            }
        }
        private void EditInvokeVirtueMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.InvokeVirtueAction invokeVirtueAction)
            {
                // Show virtue selection dialog
                string[] virtues = { "Honor", "Sacrifice", "Valor", "Compassion", "Honesty", "Humility", "Justice", "Spirituality" };
                string newVirtue = ShowVirtueDialog(invokeVirtueAction.VirtueName, virtues);

                if (!string.IsNullOrEmpty(newVirtue))
                {
                    invokeVirtueAction.VirtueName = newVirtue;

                    // Refresh and save
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    // Keep the same action selected
                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage($"Virtue changed to: {newVirtue}", 88);
                }
            }
        }
        private void EditInvokeVirtueAction(RazorEnhanced.Macros.Actions.InvokeVirtueAction invokeVirtueAction, Macro macro, int actionIndex)
        {
            string[] virtues = { "Honor", "Sacrifice", "Valor", "Compassion", "Honesty", "Humility", "Justice", "Spirituality" };
            string newVirtue = ShowVirtueDialog(invokeVirtueAction.VirtueName, virtues);

            if (!string.IsNullOrEmpty(newVirtue))
            {
                invokeVirtueAction.VirtueName = newVirtue;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionInvokeVirtue()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.InvokeVirtueAction;
            }

            return false;
        }
        private string ShowVirtueDialog(string currentVirtue, string[] virtues)
        {
            Form dialog = new Form
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Select Virtue",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblVirtue = new Label
            {
                Left = 20,
                Top = 20,
                Text = "Choose virtue:",
                Width = 250
            };

            ComboBox cmbVirtue = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbVirtue.Items.AddRange(virtues);
            cmbVirtue.SelectedItem = currentVirtue;
            if (cmbVirtue.SelectedIndex == -1) cmbVirtue.SelectedIndex = 0; // Default to first virtue

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 110,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 200,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.Cancel
            };

            btnOK.Click += (s, ev) => { dialog.Close(); };
            btnCancel.Click += (s, ev) => { dialog.Close(); };

            dialog.Controls.Add(lblVirtue);
            dialog.Controls.Add(cmbVirtue);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            return dialog.ShowDialog() == DialogResult.OK ? cmbVirtue.SelectedItem.ToString() : string.Empty;
        }


        private void InsertWaitForTargetMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            string input = PromptForInput(
                "Enter timeout duration in milliseconds:",
                "Insert Wait For Target",
                "5000"
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int timeout) || timeout <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive number for milliseconds.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var waitForTargetAction = new RazorEnhanced.Macros.Actions.WaitForTargetAction(timeout);

            int insertIndex = GetInsertPosition();
            macro.Actions.Insert(insertIndex, waitForTargetAction);

            DisplayMacroActions(macro);
            MacroManager.SaveMacros();

            if (insertIndex < macroActionsListView.Items.Count)
            {
                macroActionsListView.Items[insertIndex].Selected = true;
                macroActionsListView.EnsureVisible(insertIndex);
            }

            Misc.SendMessage($"Inserted Wait For Target: {timeout}ms at position {insertIndex + 1}", 88);
        }
        private void EditWaitForTargetMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.WaitForTargetAction waitForTargetAction)
            {
                // Prompt for new timeout duration
                string input = PromptForInput(
                    "Enter timeout duration in milliseconds:",
                    "Edit Wait For Target",
                    waitForTargetAction.Timeout.ToString()
                );

                if (string.IsNullOrWhiteSpace(input))
                    return; // User cancelled

                if (!int.TryParse(input, out int timeout) || timeout <= 0)
                {
                    MessageBox.Show(
                        "Please enter a valid positive number for milliseconds.",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // Update the timeout
                waitForTargetAction.Timeout = timeout;

                // Refresh and save
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                // Keep the same action selected
                if (actionIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                }
            }
        }
        private void EditWaitForTargetAction(RazorEnhanced.Macros.Actions.WaitForTargetAction waitForTargetAction, Macro macro, int actionIndex)
        {
            string input = PromptForInput(
                "Enter timeout duration in milliseconds:",
                "Edit Wait For Target",
                waitForTargetAction.Timeout.ToString()
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int timeout) || timeout <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive number for milliseconds.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            waitForTargetAction.Timeout = timeout;
            RefreshAndSelectAction(macro, actionIndex);
        }
        private bool IsSelectedActionWaitForTarget()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.WaitForTargetAction;
            }

            return false;
        }


        private void InsertUsePotionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            string[] potionTypes = { "Heal", "Cure", "Refresh", "Agility", "Strength", "Poison", "Explosion" };
            string selectedPotionType = ShowPotionDialog("Heal", potionTypes);

            if (!string.IsNullOrEmpty(selectedPotionType))
            {
                var usePotionAction = new RazorEnhanced.Macros.Actions.UsePotionAction(selectedPotionType);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, usePotionAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Use Potion: {selectedPotionType} at position {insertIndex + 1}", 88);
            }
        }
        private void EditUsePotionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.UsePotionAction usePotionAction)
            {
                // Show potion type selection dialog
                string[] potionTypes = { "Heal", "Cure", "Refresh", "Agility", "Strength", "Poison", "Explosion" };
                string newPotionType = ShowPotionDialog(usePotionAction.PotionType, potionTypes);

                if (!string.IsNullOrEmpty(newPotionType))
                {
                    usePotionAction.PotionType = newPotionType;

                    // Refresh and save
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    // Keep the same action selected
                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage($"Potion type changed to: {newPotionType}", 88);
                }
            }
        }
        private void EditUsePotionAction(RazorEnhanced.Macros.Actions.UsePotionAction usePotionAction, Macro macro, int actionIndex)
        {
            string[] potionTypes = { "Heal", "Cure", "Refresh", "Agility", "Strength", "Poison", "Explosion" };
            string newPotionType = ShowPotionDialog(usePotionAction.PotionType, potionTypes);

            if (!string.IsNullOrEmpty(newPotionType))
            {
                usePotionAction.PotionType = newPotionType;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionUsePotion()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.UsePotionAction;
            }

            return false;
        }
        private string ShowPotionDialog(string currentPotionType, string[] potionTypes)
        {
            Form dialog = new Form
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Select Potion Type",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblPotion = new Label
            {
                Left = 20,
                Top = 20,
                Text = "Choose potion type:",
                Width = 250
            };

            ComboBox cmbPotion = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPotion.Items.AddRange(potionTypes);
            cmbPotion.SelectedItem = currentPotionType;
            if (cmbPotion.SelectedIndex == -1) cmbPotion.SelectedIndex = 0; // Default to first potion

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 100,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 190,
                Width = 80,
                Top = 90,
                DialogResult = DialogResult.Cancel
            };

            btnOK.Click += (s, ev) => { dialog.Close(); };
            btnCancel.Click += (s, ev) => { dialog.Close(); };

            dialog.Controls.Add(lblPotion);
            dialog.Controls.Add(cmbPotion);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            return dialog.ShowDialog() == DialogResult.OK ? cmbPotion.SelectedItem.ToString() : string.Empty;
        }


        private void InsertUseSkillMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            string[] invokableSkills = GetInvokableSkills();
            var result = ShowUseSkillDialog("Hiding", "", invokableSkills);

            if (!string.IsNullOrEmpty(result.skillName))
            {
                var useSkillAction = new RazorEnhanced.Macros.Actions.UseSkillAction(result.skillName, result.targetSerialOrAlias);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, useSkillAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Use Skill: {result.skillName} at position {insertIndex + 1}", 88);
            }
        }
        private void EditUseSkillMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.UseSkillAction useSkillAction)
            {
                string[] invokableSkills = GetInvokableSkills();
                var result = ShowUseSkillDialog(useSkillAction.SkillName, useSkillAction.TargetSerialOrAlias, invokableSkills);

                if (!string.IsNullOrEmpty(result.skillName))
                {
                    useSkillAction.SkillName = result.skillName;
                    useSkillAction.TargetSerialOrAlias = result.targetSerialOrAlias;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage($"Skill changed to: {result.skillName}", 88);
                }
            }
        }
        private void EditUseSkillAction(RazorEnhanced.Macros.Actions.UseSkillAction useSkillAction, Macro macro, int actionIndex)
        {
            string[] invokableSkills = GetInvokableSkills();
            var result = ShowUseSkillDialog(useSkillAction.SkillName, useSkillAction.TargetSerialOrAlias, invokableSkills);

            if (!string.IsNullOrEmpty(result.skillName))
            {
                useSkillAction.SkillName = result.skillName;
                useSkillAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionUseSkill()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.UseSkillAction;
            }

            return false;
        }
        private string[] GetAllSkills()
        {
            var skillNames = new List<string>();

            try
            {
                // Get all skills from Ultima.Skills.SkillEntries
                if (Ultima.Skills.SkillEntries != null)
                {
                    foreach (var skillEntry in Ultima.Skills.SkillEntries)
                    {
                        if (skillEntry != null && !string.IsNullOrEmpty(skillEntry.Name))
                        {
                            // Use the skill name from the entry
                            // Remove spaces for consistency with existing format
                            string skillName = skillEntry.Name.Replace(" ", "");
                            skillNames.Add(skillName);
                        }
                    }
                }

                // If we got skills, return them sorted
                if (skillNames.Count > 0)
                {
                    skillNames.Sort();
                    return skillNames.ToArray();
                }

                // Fallback to hardcoded list if Ultima.Skills isn't available
                Misc.SendMessage("Warning: Could not load skills from Ultima.Skills, using defaults", 33);
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error loading skills: {ex.Message}, using defaults", 33);
            }

            // Fallback to original hardcoded list
            return new string[]
            {
        "Alchemy",
        "Anatomy",
        "AnimalLore",
        "AnimalTaming",
        "Archery",
        "ArmsLore",
        "Begging",
        "Blacksmith",
        "Bushido",
        "Camping",
        "Carpentry",
        "Cartography",
        "Cooking",
        "DetectHidden",
        "Discordance",
        "EvalInt",
        "Fencing",
        "Fishing",
        "Fletching",
        "Focus",
        "Forensics",
        "Healing",
        "Herding",
        "Hiding",
        "Imbuing",
        "Inscribe",
        "ItemID",
        "Lockpicking",
        "Lumberjacking",
        "Macefighting",
        "Magery",
        "MagicResist",
        "Meditation",
        "Mining",
        "Musicianship",
        "Necromancy",
        "Ninjitsu",
        "Parry",
        "Peacemaking",
        "Poisoning",
        "Provocation",
        "RemoveTrap",
        "Snooping",
        "SpiritSpeak",
        "Stealing",
        "Stealth",
        "Swords",
        "Tactics",
        "Tailoring",
        "TasteID",
        "Tinkering",
        "Tracking",
        "Veterinary",
        "Wrestling",
        "Chivalry",
        "Spellweaving",
        "Mysticism",
        "Throwing"
            };
        }
        private string[] GetInvokableSkills()
        {
            var skillNames = new List<string>();

            try
            {
                // Get invokable skills from Ultima.Skills.SkillEntries where IsAction is true
                if (Ultima.Skills.SkillEntries != null)
                {
                    foreach (var skillEntry in Ultima.Skills.SkillEntries)
                    {
                        if (skillEntry != null && !string.IsNullOrEmpty(skillEntry.Name) && skillEntry.IsAction)
                        {
                            // Remove spaces for consistency with existing format
                            string skillName = skillEntry.Name.Replace(" ", "");
                            skillNames.Add(skillName);
                        }
                    }
                }

                // If we got invokable skills, return them sorted
                if (skillNames.Count > 0)
                {
                    skillNames.Sort();
                    return skillNames.ToArray();
                }

                // Fallback to hardcoded list if Ultima.Skills isn't available or no IsAction skills found
                Misc.SendMessage("Warning: Could not load invokable skills from Ultima.Skills, using defaults", 33);
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error loading invokable skills: {ex.Message}, using defaults", 33);
            }

            // Fallback to hardcoded list of invokable skills (24 skills that can be used from macro)
            return new string[]
            {
        "Anatomy",
        "AnimalLore",
        "AnimalTaming",
        "ArmsLore",
        "Begging",
        "Cartography",
        "DetectHidden",
        "Discordance",
        "EvalInt",
        "Forensics",
        "Hiding",
        "Imbuing",
        "Inscribe",
        "ItemID",
        "Meditation",
        "Peacemaking",
        "Poisoning",
        "Provocation",
        "RemoveTrap",
        "SpiritSpeak",
        "Stealing",
        "Stealth",
        "TasteID",
        "Tracking"
            };
        }
        private (string skillName, string targetSerialOrAlias) ShowUseSkillDialog(string currentSkill, string currentTarget, string[] skills)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Select Skill and Target",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblSkill = new Label { Left = 20, Top = 20, Text = "Skill:", Width = 80 };
            ComboBox cmbSkill = new ComboBox
            {
                Left = 110,
                Top = 18,
                Width = 270,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Sorted = true
            };
            cmbSkill.Items.AddRange(skills);
            cmbSkill.SelectedItem = currentSkill;
            if (cmbSkill.SelectedIndex == -1 && cmbSkill.Items.Count > 0)
                cmbSkill.SelectedIndex = 0;

            Label lblTarget = new Label { Left = 20, Top = 60, Text = "Target (serial/alias):", Width = 120 };
            TextBox txtTarget = new TextBox
            {
                Left = 150,
                Top = 58,
                Width = 230,
                Text = currentTarget ?? ""
            };

            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 150,
                Top = 90,
                Width = 80
            };
            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item or mobile...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtTarget.Text = $"0x{(int)serial:X8}";
                        }));
                    }
                }));
            };

            Label lblHint = new Label
            {
                Left = 150,
                Top = 120,
                Width = 230,
                Height = 40,
                Text = "Leave blank for no target.\nYou can use a serial (0x...) or an alias (e.g. 'findfound').",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnOK = new Button { Text = "OK", Left = 180, Width = 80, Top = 170, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 270, Width = 80, Top = 170, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblSkill);
            dialog.Controls.Add(cmbSkill);
            dialog.Controls.Add(lblTarget);
            dialog.Controls.Add(txtTarget);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblHint);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK && cmbSkill.SelectedItem != null)
            {
                string selectedSkill = cmbSkill.SelectedItem.ToString();
                return (selectedSkill, txtTarget.Text.Trim());
            }
            return ("", currentTarget);
        }


        private void InsertCastSpellMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            var allSpells = RazorEnhanced.Macros.Actions.CastSpellAction.GetAllSpellsForUI();
            var result = ShowCastSpellDialog(42, "", allSpells);

            if (result.success)
            {
                var castSpellAction = new RazorEnhanced.Macros.Actions.CastSpellAction(result.spellID, null, result.targetSerialOrAlias);
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, castSpellAction);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Cast Spell: {result.spellID} at position {insertIndex + 1}", 88);
            }
        }
        private void EditCastSpellMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.CastSpellAction castSpellAction)
            {
                var allSpells = RazorEnhanced.Macros.Actions.CastSpellAction.GetAllSpellsForUI();
                var result = ShowCastSpellDialog(castSpellAction.SpellID, castSpellAction.TargetSerialOrAlias, allSpells);

                if (result.success)
                {
                    castSpellAction.SpellID = result.spellID;
                    castSpellAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }
                    Misc.SendMessage($"Spell changed to ID: {result.spellID}", 88);
                }
            }
        }
        private void EditCastSpellAction(RazorEnhanced.Macros.Actions.CastSpellAction castSpellAction, Macro macro, int actionIndex)
        {
            var allSpells = RazorEnhanced.Macros.Actions.CastSpellAction.GetAllSpellsForUI();
            var result = ShowCastSpellDialog(castSpellAction.SpellID, castSpellAction.TargetSerialOrAlias, allSpells);

            if (result.success)
            {
                castSpellAction.SpellID = result.spellID;
                castSpellAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionCastSpell()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.CastSpellAction;
            }

            return false;
        }
        private (bool success, int spellID, string targetSerialOrAlias) ShowCastSpellDialog(int currentSpellID, string currentTarget, Dictionary<string, int> spells)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Select Spell and Target",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblSpell = new Label { Left = 20, Top = 20, Text = "Spell:", Width = 80 };
            ComboBox cmbSpell = new ComboBox
            {
                Left = 110,
                Top = 18,
                Width = 270,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSpell.Items.AddRange(spells.Keys.ToArray());
            // Select current spell
            string currentSpellDisplay = spells.FirstOrDefault(x => x.Value == currentSpellID).Key;
            if (!string.IsNullOrEmpty(currentSpellDisplay))
                cmbSpell.SelectedItem = currentSpellDisplay;
            if (cmbSpell.SelectedIndex == -1 && cmbSpell.Items.Count > 0)
                cmbSpell.SelectedIndex = 0;

            Label lblTarget = new Label { Left = 20, Top = 60, Text = "Target (serial/alias):", Width = 120 };
            TextBox txtTarget = new TextBox
            {
                Left = 150,
                Top = 58,
                Width = 230,
                Text = currentTarget ?? ""
            };

            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 150,
                Top = 90,
                Width = 80
            };
            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item or mobile...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtTarget.Text = $"0x{(int)serial:X8}";
                        }));
                    }
                }));
            };

            Label lblHint = new Label
            {
                Left = 150,
                Top = 120,
                Width = 230,
                Height = 40,
                Text = "Leave blank for no target.\nYou can use a serial (0x...) or an alias (e.g. 'findfound').",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnOK = new Button { Text = "OK", Left = 180, Width = 80, Top = 170, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 270, Width = 80, Top = 170, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblSpell);
            dialog.Controls.Add(cmbSpell);
            dialog.Controls.Add(lblTarget);
            dialog.Controls.Add(txtTarget);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblHint);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK && cmbSpell.SelectedItem != null)
            {
                string selectedSpell = cmbSpell.SelectedItem.ToString();
                if (spells.TryGetValue(selectedSpell, out int spellID))
                {
                    return (true, spellID, txtTarget.Text.Trim());
                }
            }
            return (false, currentSpellID, currentTarget);
        }


        private void InsertMountMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowMountDialog(true, 0);

            if (result.success)
            {
                var mountAction = new RazorEnhanced.Macros.Actions.MountAction(result.shouldMount, result.mountSerial);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, mountAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                string modeText = result.shouldMount ? "Mount" : "Dismount";
                Misc.SendMessage($"Inserted {modeText} action at position {insertIndex + 1}", 88);
            }
        }
        private void EditMountMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.MountAction mountAction)
            {
                var result = ShowMountDialog(mountAction.ShouldMount, mountAction.MountSerial);

                if (result.success)
                {
                    mountAction.ShouldMount = result.shouldMount;
                    mountAction.MountSerial = result.mountSerial;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    string modeText = result.shouldMount ? "Mount" : "Dismount";
                    Misc.SendMessage($"Mount action changed to: {modeText}", 88);
                }
            }
        }
        private void EditMountAction(RazorEnhanced.Macros.Actions.MountAction mountAction, Macro macro, int actionIndex)
        {
            var result = ShowMountDialog(mountAction.ShouldMount, mountAction.MountSerial);

            if (result.success)
            {
                mountAction.ShouldMount = result.shouldMount;
                mountAction.MountSerial = result.mountSerial;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionMount()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.MountAction;
            }

            return false;
        }
        private (bool success, bool shouldMount, int mountSerial) ShowMountDialog(bool shouldMount, int mountSerial)
        {
            Form dialog = new Form
            {
                Width = 450,
                Height = 280,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Mount Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Mode selector
            Label lblMode = new Label { Left = 20, Top = 20, Text = "Action Mode:", Width = 100 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 130,
                Top = 20,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Mount", "Dismount" });
            cmbMode.SelectedIndex = shouldMount ? 0 : 1;

            // Mount Serial input
            Label lblSerial = new Label { Left = 20, Top = 60, Text = "Mount Serial (hex):", Width = 110 };
            TextBox txtSerial = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 200,
                Text = mountSerial == 0 ? "" : $"0x{mountSerial:X8}"
            };

            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 340,
                Top = 58,
                Width = 80
            };

            Label lblSerialNote = new Label
            {
                Left = 130,
                Top = 90,
                Width = 290,
                Height = 50,
                Text = "Leave empty (0x00000000) to mount last used mount.\nOtherwise, specify mount serial (works with pets and ethereal mounts).",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Target button handler - UPDATED to handle both items and mobiles
            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a mount (pet/creature or ethereal mount)...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)targetSerial;

                            // Check if it's a mobile (regular mount/pet)
                            var mobile = Mobiles.FindBySerial(serialValue);
                            if (mobile != null)
                            {
                                txtSerial.Text = $"0x{serialValue:X8}";
                                Misc.SendMessage($"Set to mount: {mobile.Name} (0x{serialValue:X8})", 88);
                                return;
                            }

                            // Check if it's an item (ethereal mount)
                            var item = Items.FindBySerial(serialValue);
                            if (item != null)
                            {
                                txtSerial.Text = $"0x{serialValue:X8}";
                                Misc.SendMessage($"Set to ethereal mount: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                                return;
                            }

                            // Unknown target
                            txtSerial.Text = $"0x{serialValue:X8}";
                            Misc.SendMessage($"Set to serial: 0x{serialValue:X8}", 88);
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 230,
                Width = 90,
                Top = 190,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 330,
                Width = 90,
                Top = 190,
                DialogResult = DialogResult.Cancel
            };

            // Visibility logic
            Action updateVisibility = () =>
            {
                bool isMount = cmbMode.SelectedIndex == 0;

                lblSerial.Visible = isMount;
                txtSerial.Visible = isMount;
                btnTarget.Visible = isMount;
                lblSerialNote.Visible = isMount;


                dialog.Height = lblSerial.Visible ? 230 : 140;

                btnOK.Top = lblSerial.Visible ? 150 : 60;
                btnCancel.Top = lblSerial.Visible ? 150  : 60;

            };

            cmbMode.SelectedIndexChanged += (s, ev) => updateVisibility();
            updateVisibility(); // Initial visibility



            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblSerialNote);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool newShouldMount = cmbMode.SelectedIndex == 0;
                int newSerial = 0;

                // Parse serial only if in Mount mode
                if (newShouldMount)
                {
                    string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                    if (!string.IsNullOrEmpty(serialStr))
                    {
                        if (!int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                        {
                            MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345) or leave empty for last mount.",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, shouldMount, mountSerial);
                        }
                    }
                }

                return (true, newShouldMount, newSerial);
            }

            return (false, shouldMount, mountSerial);
        }


        private void InsertRunOrganizerOnceMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowRunOrganizerOnceDialog("", -1, -1, -1);

            if (result.success)
            {
                var runOrganizerOnceAction = new RazorEnhanced.Macros.Actions.RunOrganizerOnceAction(
                    result.organizerName, result.sourceBag, result.destinationBag, result.dragDelay);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, runOrganizerOnceAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Run Organizer Once: '{result.organizerName}' at position {insertIndex + 1}", 88);
            }
        }
        private void EditRunOrganizerOnceMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.RunOrganizerOnceAction runOrganizerAction)
            {
                var result = ShowRunOrganizerOnceDialog(
                    runOrganizerAction.OrganizerName,
                    runOrganizerAction.SourceBag,
                    runOrganizerAction.DestinationBag,
                    runOrganizerAction.DragDelay
                );

                if (result.success)
                {
                    runOrganizerAction.OrganizerName = result.organizerName;
                    runOrganizerAction.SourceBag = result.sourceBag;
                    runOrganizerAction.DestinationBag = result.destinationBag;
                    runOrganizerAction.DragDelay = result.dragDelay;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Run Organizer Once action updated", 88);
                }
            }
        }
        private void EditRunOrganizerOnceAction(RazorEnhanced.Macros.Actions.RunOrganizerOnceAction runOrganizerAction, Macro macro, int actionIndex)
        {
            var result = ShowRunOrganizerOnceDialog(
                runOrganizerAction.OrganizerName,
                runOrganizerAction.SourceBag,
                runOrganizerAction.DestinationBag,
                runOrganizerAction.DragDelay
            );

            if (result.success)
            {
                runOrganizerAction.OrganizerName = result.organizerName;
                runOrganizerAction.SourceBag = result.sourceBag;
                runOrganizerAction.DestinationBag = result.destinationBag;
                runOrganizerAction.DragDelay = result.dragDelay;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionRunOrganizerOnce()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.RunOrganizerOnceAction;
            }

            return false;
        }
        private (bool success, string organizerName, int sourceBag, int destinationBag, int dragDelay) ShowRunOrganizerOnceDialog(
string organizerName, int sourceBag, int destinationBag, int dragDelay)
        {
            Form dialog = new Form
            {
                Width = 500,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Run Organizer Once",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Organizer List Name
            Label lblOrganizerName = new Label { Left = 20, Top = 20, Text = "Organizer List:", Width = 120 };
            ComboBox cmbOrganizerName = new ComboBox
            {
                Left = 150,
                Top = 20,
                Width = 310,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Populate with existing organizer lists
            var organizerLists = Settings.Organizer.ListsRead();
            foreach (var list in organizerLists)
            {
                cmbOrganizerName.Items.Add(list.Description);
            }

            if (!string.IsNullOrEmpty(organizerName) && cmbOrganizerName.Items.Contains(organizerName))
            {
                cmbOrganizerName.SelectedItem = organizerName;
            }
            else if (cmbOrganizerName.Items.Count > 0)
            {
                cmbOrganizerName.SelectedIndex = 0;
            }

            // Use Defaults checkbox
            CheckBox chkUseDefaults = new CheckBox
            {
                Left = 150,
                Top = 55,
                Width = 310,
                Text = "Use list defaults (bags and delay)",
                Checked = (sourceBag == -1 && destinationBag == -1 && dragDelay == -1)
            };

            // Source Bag
            Label lblSourceBag = new Label { Left = 20, Top = 95, Text = "Source Bag:", Width = 120 };
            TextBox txtSourceBag = new TextBox
            {
                Left = 150,
                Top = 95,
                Width = 220,
                Text = sourceBag == -1 ? "(use default)" : $"0x{sourceBag:X8}",
                Enabled = !chkUseDefaults.Checked
            };

            Button btnTargetSource = new Button
            {
                Text = "Target",
                Left = 380,
                Top = 93,
                Width = 80,
                Enabled = !chkUseDefaults.Checked
            };

            // Destination Bag
            Label lblDestBag = new Label { Left = 20, Top = 135, Text = "Destination Bag:", Width = 120 };
            TextBox txtDestBag = new TextBox
            {
                Left = 150,
                Top = 135,
                Width = 220,
                Text = destinationBag == -1 ? "(use default)" : $"0x{destinationBag:X8}",
                Enabled = !chkUseDefaults.Checked
            };

            Button btnTargetDest = new Button
            {
                Text = "Target",
                Left = 380,
                Top = 133,
                Width = 80,
                Enabled = !chkUseDefaults.Checked
            };

            // Drag Delay
            Label lblDragDelay = new Label { Left = 20, Top = 175, Text = "Drag Delay (ms):", Width = 120 };
            TextBox txtDragDelay = new TextBox
            {
                Left = 150,
                Top = 175,
                Width = 310,
                Text = dragDelay == -1 ? "(use default)" : dragDelay.ToString(),
                Enabled = !chkUseDefaults.Checked
            };

            Label lblNote = new Label
            {
                Left = 150,
                Top = 210,
                Width = 310,
                Height = 80,
                Text = "Leave settings as default (-1) to use the organizer list's configured values.\n\n" +
                       "Or specify custom values to override for this macro execution.\n\n" +
                       "Macro will pause until organizer completes.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Checkbox change handler
            chkUseDefaults.CheckedChanged += (s, ev) =>
            {
                bool useDefaults = chkUseDefaults.Checked;
                txtSourceBag.Enabled = !useDefaults;
                btnTargetSource.Enabled = !useDefaults;
                txtDestBag.Enabled = !useDefaults;
                btnTargetDest.Enabled = !useDefaults;
                txtDragDelay.Enabled = !useDefaults;

                if (useDefaults)
                {
                    txtSourceBag.Text = "(use default)";
                    txtDestBag.Text = "(use default)";
                    txtDragDelay.Text = "(use default)";
                }
                else
                {
                    txtSourceBag.Text = sourceBag == -1 ? "" : $"0x{sourceBag:X8}";
                    txtDestBag.Text = destinationBag == -1 ? "" : $"0x{destinationBag:X8}";
                    txtDragDelay.Text = dragDelay == -1 ? "600" : dragDelay.ToString();
                }
            };

            // Target Source Bag button handler
            btnTargetSource.Click += (s, ev) =>
            {
                Misc.SendMessage("Target source bag...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0 && targetSerial.IsItem)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)targetSerial;
                            txtSourceBag.Text = $"0x{serialValue:X8}";
                            Misc.SendMessage($"Set source bag to: 0x{serialValue:X8}", 88);
                        }));
                    }
                    else if (targetSerial.IsValid && targetSerial.IsMobile)
                    {
                        Misc.SendMessage("Target must be a container (item), not a mobile.", 33);
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Target Destination Bag button handler
            btnTargetDest.Click += (s, ev) =>
            {
                Misc.SendMessage("Target destination bag...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0 && targetSerial.IsItem)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)targetSerial;
                            txtDestBag.Text = $"0x{serialValue:X8}";
                            Misc.SendMessage($"Set destination bag to: 0x{serialValue:X8}", 88);
                        }));
                    }
                    else if (targetSerial.IsValid && targetSerial.IsMobile)
                    {
                        Misc.SendMessage("Target must be a container (item), not a mobile.", 33);
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 270,
                Width = 90,
                Top = 310,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 370,
                Width = 90,
                Top = 310,
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblOrganizerName);
            dialog.Controls.Add(cmbOrganizerName);
            dialog.Controls.Add(chkUseDefaults);
            dialog.Controls.Add(lblSourceBag);
            dialog.Controls.Add(txtSourceBag);
            dialog.Controls.Add(btnTargetSource);
            dialog.Controls.Add(lblDestBag);
            dialog.Controls.Add(txtDestBag);
            dialog.Controls.Add(btnTargetDest);
            dialog.Controls.Add(lblDragDelay);
            dialog.Controls.Add(txtDragDelay);
            dialog.Controls.Add(lblNote);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Validate organizer list selection
                if (cmbOrganizerName.SelectedItem == null)
                {
                    MessageBox.Show("Please select an organizer list.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, organizerName, sourceBag, destinationBag, dragDelay);
                }

                string newOrganizerName = cmbOrganizerName.SelectedItem.ToString();
                int newSourceBag = -1;
                int newDestBag = -1;
                int newDragDelay = -1;

                // Parse values only if not using defaults
                if (!chkUseDefaults.Checked)
                {
                    // Parse source bag
                    if (!txtSourceBag.Text.Contains("default"))
                    {
                        string sourceStr = txtSourceBag.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (!string.IsNullOrEmpty(sourceStr))
                        {
                            if (!int.TryParse(sourceStr, System.Globalization.NumberStyles.HexNumber, null, out newSourceBag))
                            {
                                MessageBox.Show("Invalid source bag serial.", "Invalid Input",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return (false, organizerName, sourceBag, destinationBag, dragDelay);
                            }
                        }
                    }

                    // Parse destination bag
                    if (!txtDestBag.Text.Contains("default"))
                    {
                        string destStr = txtDestBag.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (!string.IsNullOrEmpty(destStr))
                        {
                            if (!int.TryParse(destStr, System.Globalization.NumberStyles.HexNumber, null, out newDestBag))
                            {
                                MessageBox.Show("Invalid destination bag serial.", "Invalid Input",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return (false, organizerName, sourceBag, destinationBag, dragDelay);
                            }
                        }
                    }

                    // Parse drag delay
                    if (!txtDragDelay.Text.Contains("default"))
                    {
                        if (!int.TryParse(txtDragDelay.Text, out newDragDelay) || newDragDelay < 0)
                        {
                            MessageBox.Show("Invalid drag delay. Please enter a positive number in milliseconds.",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, organizerName, sourceBag, destinationBag, dragDelay);
                        }
                    }
                }

                return (true, newOrganizerName, newSourceBag, newDestBag, newDragDelay);
            }

            return (false, organizerName, sourceBag, destinationBag, dragDelay);
        }


        private void InsertTargetMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowTargetDialog(TargetAction.TargetMode.Self, 0, 0, -1, "Nearest", "", 0, 0, 0);

            if (result.success)
            {
                var targetAction = new RazorEnhanced.Macros.Actions.TargetAction(
                    result.mode, result.serial, result.graphic, result.color,
                    result.selector, result.aliasName, result.x, result.y, result.z);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, targetAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Target action at position {insertIndex + 1}", 88);
            }
        }
        private void EditTargetMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.TargetAction targetAction)
            {
                var result = ShowTargetDialog(
                    targetAction.Mode,
                    targetAction.Serial,
                    targetAction.Graphic,
                    targetAction.Color,
                    targetAction.Selector,
                    targetAction.AliasName,
                    targetAction.X,
                    targetAction.Y,
                    targetAction.Z
                );

                if (result.success)
                {
                    targetAction.Mode = result.mode;
                    targetAction.Serial = result.serial;
                    targetAction.Graphic = result.graphic;
                    targetAction.Color = result.color;
                    targetAction.Selector = result.selector;
                    targetAction.AliasName = result.aliasName;
                    targetAction.X = result.x;
                    targetAction.Y = result.y;
                    targetAction.Z = result.z;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Target action updated", 88);
                }
            }
        }
        private void EditTargetAction(RazorEnhanced.Macros.Actions.TargetAction targetAction, Macro macro, int actionIndex)
        {
            var result = ShowTargetDialog(
                targetAction.Mode,
                targetAction.Serial,
                targetAction.Graphic,
                targetAction.Color,
                targetAction.Selector,
                targetAction.AliasName,
                targetAction.X,
                targetAction.Y,
                targetAction.Z
            );

            if (result.success)
            {
                targetAction.Mode = result.mode;
                targetAction.Serial = result.serial;
                targetAction.Graphic = result.graphic;
                targetAction.Color = result.color;
                targetAction.Selector = result.selector;
                targetAction.AliasName = result.aliasName;
                targetAction.X = result.x;
                targetAction.Y = result.y;
                targetAction.Z = result.z;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionTarget()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.TargetAction;
            }

            return false;
        }

        private (bool success, TargetAction.TargetMode mode, int serial, int graphic, int color,
            string selector, string aliasName, int x, int y, int z) ShowTargetDialog(
            TargetAction.TargetMode mode, int serial, int graphic, int color,
            string selector, string aliasName, int x, int y, int z)
        {
            Form dialog = new Form
            {
                Width = 480,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Target Configuration",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // === TARGET MODE SELECTOR (Top = 20) ===
            Label lblMode = new Label { Left = 20, Top = 20, Text = "Target Mode:", Width = 100 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 130,
                Top = 20,
                Width = 310,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Serial", "Type", "Self", "Alias", "Location", "Last Target" });
            cmbMode.SelectedIndex = (int)mode;

            // === SERIAL MODE CONTROLS (Top = 60) ===
            Label lblSerial = new Label { Left = 20, Top = 60, Text = "Serial (hex):", Width = 100 };
            TextBox txtSerial = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}"
            };

            Button btnTargetSerial = new Button
            {
                Text = "Target",
                Left = 340,
                Top = 58,
                Width = 100
            };

            Label lblSerialNote = new Label
            {
                Left = 130,
                Top = 90,
                Width = 310,
                Height = 30,
                Text = "Target a specific entity by its serial number",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === TYPE MODE CONTROLS (Top = 60) ===
            Label lblGraphic = new Label { Left = 20, Top = 60, Text = "Graphic (hex):", Width = 100 };
            TextBox txtGraphic = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 200,
                Text = graphic == 0 ? "" : $"0x{graphic:X4}"
            };

            Button btnTargetType = new Button
            {
                Text = "Target",
                Left = 340,
                Top = 58,
                Width = 100
            };

            Label lblColor = new Label { Left = 20, Top = 100, Text = "Color (-1 = any):", Width = 100 };
            TextBox txtColor = new TextBox
            {
                Left = 130,
                Top = 100,
                Width = 310,
                Text = color.ToString()
            };

            Label lblSelector = new Label { Left = 20, Top = 140, Text = "Selector:", Width = 100 };
            ComboBox cmbSelector = new ComboBox
            {
                Left = 130,
                Top = 140,
                Width = 310,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSelector.Items.AddRange(new string[] { "Nearest", "Farthest", "Random" });
            cmbSelector.SelectedItem = selector ?? "Nearest";
            if (cmbSelector.SelectedIndex == -1) cmbSelector.SelectedIndex = 0;

            Label lblTypeNote = new Label
            {
                Left = 130,
                Top = 170,
                Width = 310,
                Height = 40,
                Text = "Target by item/mobile type. Selector chooses which matching entity to target.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === SELF MODE CONTROLS (Top = 60) ===
            Label lblSelfNote = new Label
            {
                Left = 130,
                Top = 60,
                Width = 310,
                Height = 50,
                Text = "Target yourself (the player character).\nNo additional configuration needed.",
                ForeColor = Color.DarkGreen,
                Font = new Font(Control.DefaultFont, FontStyle.Bold)
            };

            // === ALIAS MODE CONTROLS (Top = 60) ===
            Label lblAlias = new Label { Left = 20, Top = 60, Text = "Alias Name:", Width = 100 };
            TextBox txtAlias = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 310,
                Text = aliasName ?? ""
            };

            Label lblAliasNote = new Label
            {
                Left = 130,
                Top = 90,
                Width = 310,
                Height = 60,
                Text = "Use 'findfound' (from Find condition), 'enemy', 'friend', or any custom alias.\n\nAlias must be set before this action executes.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === LOCATION MODE CONTROLS (Top = 60) ===
            Label lblLocationNote = new Label
            {
                Left = 130,
                Top = 60,
                Width = 310,
                Height = 25,
                Text = "Target specific coordinates (ground/tile)",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnTargetLocation = new Button
            {
                Text = "Target Location",
                Left = 130,
                Top = 90,
                Width = 310
            };

            Label lblX = new Label { Left = 20, Top = 130, Text = "X:", Width = 30 };
            TextBox txtX = new TextBox { Left = 130, Top = 130, Width = 100, Text = x.ToString() };

            Label lblY = new Label { Left = 20, Top = 165, Text = "Y:", Width = 30 };
            TextBox txtY = new TextBox { Left = 130, Top = 165, Width = 100, Text = y.ToString() };

            Label lblZ = new Label { Left = 20, Top = 200, Text = "Z:", Width = 30 };
            TextBox txtZ = new TextBox { Left = 130, Top = 200, Width = 100, Text = z.ToString() };

            Label lblCoordNote = new Label
            {
                Left = 250,
                Top = 130,
                Width = 190,
                Height = 80,
                Text = "Target a location to get coordinates, or manually enter X, Y, Z values.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === LAST TARGET MODE CONTROLS (Top = 60) ===
            Label lblLastNote = new Label
            {
                Left = 130,
                Top = 60,
                Width = 310,
                Height = 50,
                Text = "Target the last entity you targeted.\nUses Target.Last() internally.",
                ForeColor = Color.DarkBlue,
                Font = new Font(Control.DefaultFont, FontStyle.Bold)
            };

            // === BUTTON HANDLERS ===

            // Serial targeting
            btnTargetSerial.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an entity...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)targetSerial;
                            txtSerial.Text = $"0x{serialValue:X8}";

                            var mobile = Mobiles.FindBySerial(serialValue);
                            if (mobile != null)
                            {
                                Misc.SendMessage($"Set to mobile: {mobile.Name} (0x{serialValue:X8})", 88);
                            }
                            else
                            {
                                var item = Items.FindBySerial(serialValue);
                                if (item != null)
                                {
                                    Misc.SendMessage($"Set to item: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                                }
                                else
                                {
                                    Misc.SendMessage($"Set to serial: 0x{serialValue:X8}", 88);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Type targeting
            btnTargetType.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item or mobile to get its type...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            var item = Items.FindBySerial((int)targetSerial);
                            if (item != null)
                            {
                                txtGraphic.Text = $"0x{item.ItemID:X4}";
                                txtColor.Text = item.Hue.ToString();
                                Misc.SendMessage($"Set to item type: 0x{item.ItemID:X4}, Color: {item.Hue}", 88);
                            }
                            else
                            {
                                var mobile = Mobiles.FindBySerial((int)targetSerial);
                                if (mobile != null)
                                {
                                    txtGraphic.Text = $"0x{mobile.Body:X4}";
                                    txtColor.Text = mobile.Color.ToString();
                                    Misc.SendMessage($"Set to mobile type: 0x{mobile.Body:X4}, Color: {mobile.Color}", 88);
                                }
                                else
                                {
                                    Misc.SendMessage("Could not find the targeted entity.", 33);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Location targeting
            btnTargetLocation.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a location or entity...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    dialog.Invoke(new Action(() =>
                    {
                        txtX.Text = p.X.ToString();
                        txtY.Text = p.Y.ToString();
                        txtZ.Text = p.Z.ToString();
                        Misc.SendMessage($"Set to coordinates: ({p.X}, {p.Y}, {p.Z})", 88);
                    }));
                }));
            };

            // === BUTTONS (Bottom of dialog) ===
            Button btnOK = new Button
            {
                Text = "OK",
                Left = 250,
                Width = 90,
                Top = 420,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 350,
                Width = 90,
                Top = 420,
                DialogResult = DialogResult.Cancel
            };

            // === VISIBILITY LOGIC ===
            Action updateVisibility = () =>
            {
                var selectedMode = (TargetAction.TargetMode)cmbMode.SelectedIndex;

                bool isSerial = selectedMode == TargetAction.TargetMode.Serial;
                bool isType = selectedMode == TargetAction.TargetMode.Type;
                bool isSelf = selectedMode == TargetAction.TargetMode.Self;
                bool isAlias = selectedMode == TargetAction.TargetMode.Alias;
                bool isLocation = selectedMode == TargetAction.TargetMode.Location;
                bool isLastTarget = selectedMode == TargetAction.TargetMode.LastTarget;

                // Serial mode
                lblSerial.Visible = txtSerial.Visible = btnTargetSerial.Visible = lblSerialNote.Visible = isSerial;

                // Type mode
                lblGraphic.Visible = txtGraphic.Visible = btnTargetType.Visible = isType;
                lblColor.Visible = txtColor.Visible = isType;
                lblSelector.Visible = cmbSelector.Visible = isType;
                lblTypeNote.Visible = isType;

                // Self mode
                lblSelfNote.Visible = isSelf;

                // Alias mode
                lblAlias.Visible = txtAlias.Visible = lblAliasNote.Visible = isAlias;

                // Location mode
                lblLocationNote.Visible = btnTargetLocation.Visible = isLocation;
                lblX.Visible = txtX.Visible = isLocation;
                lblY.Visible = txtY.Visible = isLocation;
                lblZ.Visible = txtZ.Visible = isLocation;
                lblCoordNote.Visible = isLocation;

                // Last Target mode
                lblLastNote.Visible = isLastTarget;

                var menumode = (TargetAction.TargetMode)cmbMode.SelectedIndex;

                switch(menumode)
                {
                    case TargetAction.TargetMode.Serial:
                        dialog.Height = 230;
                        btnOK.Top = 150;
                        btnCancel.Top = 150;
                        break;
                    case TargetAction.TargetMode.Type:
                        dialog.Height = 300;
                        btnOK.Top = 220;
                        btnCancel.Top = 220;
                        break;
                    case TargetAction.TargetMode.Self:
                        dialog.Height = 200;
                        btnOK.Top = 120;
                        btnCancel.Top = 120;
                        break;
                    case TargetAction.TargetMode.Alias:
                        dialog.Height = 260;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                    case TargetAction.TargetMode.Location:
                        dialog.Height = 320;
                        btnOK.Top = 240;
                        btnCancel.Top = 240;
                        break;
                    case TargetAction.TargetMode.LastTarget:
                        dialog.Height = 200;
                        btnOK.Top = 120;
                        btnCancel.Top = 120;
                        break;
                }
         
            };

            cmbMode.SelectedIndexChanged += (s, ev) => updateVisibility();



            // === ADD ALL CONTROLS TO DIALOG ===
            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);

            // Serial mode controls
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTargetSerial);
            dialog.Controls.Add(lblSerialNote);

            // Type mode controls
            dialog.Controls.Add(lblGraphic);
            dialog.Controls.Add(txtGraphic);
            dialog.Controls.Add(btnTargetType);
            dialog.Controls.Add(lblColor);
            dialog.Controls.Add(txtColor);
            dialog.Controls.Add(lblSelector);
            dialog.Controls.Add(cmbSelector);
            dialog.Controls.Add(lblTypeNote);

            // Self mode controls
            dialog.Controls.Add(lblSelfNote);

            // Alias mode controls
            dialog.Controls.Add(lblAlias);
            dialog.Controls.Add(txtAlias);
            dialog.Controls.Add(lblAliasNote);

            // Location mode controls
            dialog.Controls.Add(lblLocationNote);
            dialog.Controls.Add(btnTargetLocation);
            dialog.Controls.Add(lblX);
            dialog.Controls.Add(txtX);
            dialog.Controls.Add(lblY);
            dialog.Controls.Add(txtY);
            dialog.Controls.Add(lblZ);
            dialog.Controls.Add(txtZ);
            dialog.Controls.Add(lblCoordNote);

            // Last Target mode controls
            dialog.Controls.Add(lblLastNote);

            // Buttons
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);

            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            // Set initial visibility
            updateVisibility();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newMode = (TargetAction.TargetMode)cmbMode.SelectedIndex;
                int newSerial = 0;
                int newGraphic = 0;
                int newColor = -1;
                string newSelector = "Nearest";
                string newAliasName = "";
                int newX = 0, newY = 0, newZ = 0;

                // Validate and parse based on selected mode
                switch (newMode)
                {
                    case TargetAction.TargetMode.Serial:
                        string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (string.IsNullOrEmpty(serialStr))
                        {
                            MessageBox.Show("Serial cannot be empty for Serial mode.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        if (!int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                        {
                            MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        break;

                    case TargetAction.TargetMode.Type:
                        string graphicStr = txtGraphic.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (string.IsNullOrEmpty(graphicStr))
                        {
                            MessageBox.Show("Graphic cannot be empty for Type mode.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        if (!int.TryParse(graphicStr, System.Globalization.NumberStyles.HexNumber, null, out newGraphic))
                        {
                            MessageBox.Show("Invalid graphic value. Please enter a hex value (e.g., 0x0E21).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        if (!int.TryParse(txtColor.Text, out newColor))
                        {
                            MessageBox.Show("Invalid color value. Please enter a number (-1 for any color).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        newSelector = cmbSelector.SelectedItem.ToString();
                        break;

                    case TargetAction.TargetMode.Self:
                    case TargetAction.TargetMode.LastTarget:
                        // No validation needed
                        break;

                    case TargetAction.TargetMode.Alias:
                        if (string.IsNullOrWhiteSpace(txtAlias.Text))
                        {
                            MessageBox.Show("Alias name cannot be empty.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        newAliasName = txtAlias.Text.Trim().ToLower();
                        break;

                    case TargetAction.TargetMode.Location:
                        if (!int.TryParse(txtX.Text, out newX))
                        {
                            MessageBox.Show("Invalid X coordinate.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        if (!int.TryParse(txtY.Text, out newY))
                        {
                            MessageBox.Show("Invalid Y coordinate.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        if (!int.TryParse(txtZ.Text, out newZ))
                        {
                            MessageBox.Show("Invalid Z coordinate.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
                        }
                        break;
                }

                return (true, newMode, newSerial, newGraphic, newColor, newSelector, newAliasName, newX, newY, newZ);
            }

            return (false, mode, serial, graphic, color, selector, aliasName, x, y, z);
        }


        private void InsertDoubleClickMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowDoubleClickDialog(DoubleClickAction.DoubleClickMode.Serial, 0, 0, -1, "Nearest", "");

            if (result.success)
            {
                var doubleClickAction = new RazorEnhanced.Macros.Actions.DoubleClickAction(
                    result.mode, result.serial, result.graphic, result.color,
                    result.selector, result.aliasName);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, doubleClickAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Double Click action at position {insertIndex + 1}", 88);
            }
        }
        private void EditDoubleClickMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.DoubleClickAction doubleClickAction)
            {
                var result = ShowDoubleClickDialog(
                    doubleClickAction.Mode,
                    doubleClickAction.Serial,
                    doubleClickAction.Graphic,
                    doubleClickAction.Color,
                    doubleClickAction.Selector,
                    doubleClickAction.AliasName
                );

                if (result.success)
                {
                    doubleClickAction.Mode = result.mode;
                    doubleClickAction.Serial = result.serial;
                    doubleClickAction.Graphic = result.graphic;
                    doubleClickAction.Color = result.color;
                    doubleClickAction.Selector = result.selector;
                    doubleClickAction.AliasName = result.aliasName;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Double Click action updated", 88);
                }
            }
        }
        private void EditDoubleClickAction(RazorEnhanced.Macros.Actions.DoubleClickAction doubleClickAction, Macro macro, int actionIndex)
        {
            var result = ShowDoubleClickDialog(
                doubleClickAction.Mode,
                doubleClickAction.Serial,
                doubleClickAction.Graphic,
                doubleClickAction.Color,
                doubleClickAction.Selector,
                doubleClickAction.AliasName
            );

            if (result.success)
            {
                doubleClickAction.Mode = result.mode;
                doubleClickAction.Serial = result.serial;
                doubleClickAction.Graphic = result.graphic;
                doubleClickAction.Color = result.color;
                doubleClickAction.Selector = result.selector;
                doubleClickAction.AliasName = result.aliasName;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionDoubleClick()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.DoubleClickAction;
            }

            return false;
        }

        private (bool success, DoubleClickAction.DoubleClickMode mode, int serial, int graphic, int color, string selector, string aliasName) 
            ShowDoubleClickDialog( DoubleClickAction.DoubleClickMode mode, int serial, int graphic, int color, string selector, string aliasName)
        {
            Form dialog = new Form
            {
                Width = 480,
                Height = 450,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Double Click Configuration",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // === DOUBLE CLICK MODE SELECTOR (Top = 20) ===
            Label lblMode = new Label { Left = 20, Top = 20, Text = "Double Click Mode:", Width = 120 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 150,
                Top = 20,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Serial", "Type", "Self", "Alias", "Last Target" });
            cmbMode.SelectedIndex = (int)mode;

            // === SERIAL MODE CONTROLS (Top = 60) ===
            Label lblSerial = new Label { Left = 20, Top = 60, Text = "Serial (hex):", Width = 120 };
            TextBox txtSerial = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}"
            };

            Button btnTargetSerial = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 58,
                Width = 100
            };

            Label lblSerialNote = new Label
            {
                Left = 150,
                Top = 90,
                Width = 290,
                Height = 30,
                Text = "Double-click a specific item or mobile by its serial",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === TYPE MODE CONTROLS (Top = 60) ===
            Label lblGraphic = new Label { Left = 20, Top = 60, Text = "Graphic (hex):", Width = 120 };
            TextBox txtGraphic = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 200,
                Text = graphic == 0 ? "" : $"0x{graphic:X4}"
            };

            Button btnTargetType = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 58,
                Width = 100
            };

            Label lblColor = new Label { Left = 20, Top = 100, Text = "Color (-1 = any):", Width = 120 };
            TextBox txtColor = new TextBox
            {
                Left = 150,
                Top = 100,
                Width = 310,
                Text = color.ToString()
            };

            Label lblSelector = new Label { Left = 20, Top = 140, Text = "Selector:", Width = 120 };
            ComboBox cmbSelector = new ComboBox
            {
                Left = 150,
                Top = 140,
                Width = 310,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSelector.Items.AddRange(new string[] { "Nearest", "Farthest", "Random" });
            cmbSelector.SelectedItem = selector ?? "Nearest";
            if (cmbSelector.SelectedIndex == -1) cmbSelector.SelectedIndex = 0;

            Label lblTypeNote = new Label
            {
                Left = 150,
                Top = 170,
                Width = 310,
                Height = 40,
                Text = "Double-click by item/mobile type. Selector chooses which matching entity to use.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === SELF MODE CONTROLS (Top = 60) ===
            Label lblSelfNote = new Label
            {
                Left = 150,
                Top = 60,
                Width = 310,
                Height = 50,
                Text = "Double-click yourself (opens paperdoll).\nNo additional configuration needed.",
                ForeColor = Color.DarkGreen,
                Font = new Font(Control.DefaultFont, FontStyle.Bold)
            };

            // === ALIAS MODE CONTROLS (Top = 60) ===
            Label lblAlias = new Label { Left = 20, Top = 60, Text = "Alias Name:", Width = 120 };
            TextBox txtAlias = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 310,
                Text = aliasName ?? ""
            };

            Label lblAliasNote = new Label
            {
                Left = 150,
                Top = 90,
                Width = 310,
                Height = 60,
                Text = "Use 'findfound' (from Find condition), or any custom alias.\n\nAlias must be set before this action executes.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // === LAST TARGET MODE CONTROLS (Top = 60) ===
            Label lblLastNote = new Label
            {
                Left = 150,
                Top = 60,
                Width = 310,
                Height = 50,
                Text = "Double-click the last entity you targeted.\nUses the 'last' alias internally.",
                ForeColor = Color.DarkBlue,
                Font = new Font(Control.DefaultFont, FontStyle.Bold)
            };

            // === BUTTON HANDLERS ===

            // Serial targeting
            btnTargetSerial.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an entity to double-click...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)targetSerial;
                            txtSerial.Text = $"0x{serialValue:X8}";

                            var mobile = Mobiles.FindBySerial(serialValue);
                            if (mobile != null)
                            {
                                Misc.SendMessage($"Set to mobile: {mobile.Name} (0x{serialValue:X8})", 88);
                            }
                            else
                            {
                                var item = Items.FindBySerial(serialValue);
                                if (item != null)
                                {
                                    Misc.SendMessage($"Set to item: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                                }
                                else
                                {
                                    Misc.SendMessage($"Set to serial: 0x{serialValue:X8}", 88);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Type targeting
            btnTargetType.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item or mobile to get its type...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            var item = Items.FindBySerial((int)targetSerial);
                            if (item != null)
                            {
                                txtGraphic.Text = $"0x{item.ItemID:X4}";
                                txtColor.Text = item.Hue.ToString();
                                Misc.SendMessage($"Set to item type: 0x{item.ItemID:X4}, Color: {item.Hue}", 88);
                            }
                            else
                            {
                                var mobile = Mobiles.FindBySerial((int)targetSerial);
                                if (mobile != null)
                                {
                                    txtGraphic.Text = $"0x{mobile.Body:X4}";
                                    txtColor.Text = mobile.Color.ToString();
                                    Misc.SendMessage($"Set to mobile type: 0x{mobile.Body:X4}, Color: {mobile.Color}", 88);
                                }
                                else
                                {
                                    Misc.SendMessage("Could not find the targeted entity.", 33);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // === BUTTONS (Bottom of dialog) ===
            Button btnOK = new Button
            {
                Text = "OK",
                Left = 250,
                Width = 90,
                Top = 370,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 350,
                Width = 90,
                Top = 370,
                DialogResult = DialogResult.Cancel
            };

            // === VISIBILITY LOGIC ===
            Action updateVisibility = () =>
            {
                var selectedMode = (DoubleClickAction.DoubleClickMode)cmbMode.SelectedIndex;

                bool isSerial = selectedMode == DoubleClickAction.DoubleClickMode.Serial;
                bool isType = selectedMode == DoubleClickAction.DoubleClickMode.Type;
                bool isSelf = selectedMode == DoubleClickAction.DoubleClickMode.Self;
                bool isAlias = selectedMode == DoubleClickAction.DoubleClickMode.Alias;
                bool isLastTarget = selectedMode == DoubleClickAction.DoubleClickMode.LastTarget;

                // Serial mode
                lblSerial.Visible = txtSerial.Visible = btnTargetSerial.Visible = lblSerialNote.Visible = isSerial;

                // Type mode
                lblGraphic.Visible = txtGraphic.Visible = btnTargetType.Visible = isType;
                lblColor.Visible = txtColor.Visible = isType;
                lblSelector.Visible = cmbSelector.Visible = isType;
                lblTypeNote.Visible = isType;

                // Self mode
                lblSelfNote.Visible = isSelf;

                // Alias mode
                lblAlias.Visible = txtAlias.Visible = lblAliasNote.Visible = isAlias;

                // Last Target mode
                lblLastNote.Visible = isLastTarget;




                var menumode = (DoubleClickAction.DoubleClickMode)cmbMode.SelectedIndex;

                switch(menumode)
                {
                    case DoubleClickAction.DoubleClickMode.Serial:
                        dialog.Height = 210;

                        //buttons
                        btnOK.Top = 130;
                        btnCancel.Top = 130;

                        break;
                    case DoubleClickAction.DoubleClickMode.Type:
                        dialog.Height = 290;

                        //buttons
                        btnOK.Top = 210;
                        btnCancel.Top = 210;

                        break;
                    case DoubleClickAction.DoubleClickMode.Self:
                        dialog.Height = 190;

                        //buttons
                        btnOK.Top = 110;
                        btnCancel.Top = 110;

                        break;
                    case DoubleClickAction.DoubleClickMode.Alias:
                        dialog.Height = 230;

                        //buttons
                        btnOK.Top = 150;
                        btnCancel.Top = 150;

                        break;
                    case DoubleClickAction.DoubleClickMode.LastTarget:
                        dialog.Height = 190;

                        //buttons
                        btnOK.Top = 110;
                        btnCancel.Top = 110;

                        break;
                    default:
                        //menu
                        dialog.Height = 450;

                        //serial (hex)
                        lblSerial.Top = 60;
                        txtSerial.Top = 60;
                        btnTargetSerial.Top = 58;

                        //double-click a specific item or mobile by its serial.                
                        lblSerialNote.Top = 90;

                        //graphic: (hex)
                        lblGraphic.Top = 60;
                        txtGraphic.Top = 60;
                        btnTargetType.Top = 58;

                        //color (-1 = any)
                        lblColor.Top = 100;
                        txtColor.Top = 100;

                        //selector
                        lblSelector.Top = 140;
                        cmbSelector.Top = 140;
                        //double-click by item/mobile type selector chooses which matching entity to use.
                        lblTypeNote.Top = 170;

                        //double-click yourself (opens paperdoll) no additional configuration needed.
                        lblSelfNote.Top = 60;

                        //alias name:
                        lblAlias.Top = 60;
                        txtAlias.Top = 60;
                        //Use 'findfound' (from Find condition), or any custom alias.Alias must be set before this action executes.
                        lblAliasNote.Top = 90;

                        //last target label
                        lblLastNote.Top = 60;
                        //buttons
                        btnOK.Top = 370;
                        btnCancel.Top = 370;

                        break;
                }


            };

            cmbMode.SelectedIndexChanged += (s, ev) => updateVisibility();



            // === ADD ALL CONTROLS TO DIALOG ===
            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);

            // Serial mode controls
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTargetSerial);
            dialog.Controls.Add(lblSerialNote);

            // Type mode controls
            dialog.Controls.Add(lblGraphic);
            dialog.Controls.Add(txtGraphic);
            dialog.Controls.Add(btnTargetType);
            dialog.Controls.Add(lblColor);
            dialog.Controls.Add(txtColor);
            dialog.Controls.Add(lblSelector);
            dialog.Controls.Add(cmbSelector);
            dialog.Controls.Add(lblTypeNote);

            // Self mode controls
            dialog.Controls.Add(lblSelfNote);

            // Alias mode controls
            dialog.Controls.Add(lblAlias);
            dialog.Controls.Add(txtAlias);
            dialog.Controls.Add(lblAliasNote);

            // Last Target mode controls
            dialog.Controls.Add(lblLastNote);

            // Buttons
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);

            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            // Set initial visibility
            updateVisibility();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newMode = (DoubleClickAction.DoubleClickMode)cmbMode.SelectedIndex;
                int newSerial = 0;
                int newGraphic = 0;
                int newColor = -1;
                string newSelector = "Nearest";
                string newAliasName = "";

                // Validate and parse based on selected mode
                switch (newMode)
                {
                    case DoubleClickAction.DoubleClickMode.Serial:
                        string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (string.IsNullOrEmpty(serialStr))
                        {
                            MessageBox.Show("Serial cannot be empty for Serial mode.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName);
                        }
                        if (!int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                        {
                            MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName);
                        }
                        break;

                    case DoubleClickAction.DoubleClickMode.Type:
                        string graphicStr = txtGraphic.Text.Replace("0x", "").Replace("0X", "").Trim();
                        if (string.IsNullOrEmpty(graphicStr))
                        {
                            MessageBox.Show("Graphic cannot be empty for Type mode.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName);
                        }
                        if (!int.TryParse(graphicStr, System.Globalization.NumberStyles.HexNumber, null, out newGraphic))
                        {
                            MessageBox.Show("Invalid graphic value. Please enter a hex value (e.g., 0x0E21).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName);
                        }
                        if (!int.TryParse(txtColor.Text, out newColor))
                        {
                            MessageBox.Show("Invalid color value. Please enter a number (-1 for any color).",
                                "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName);
                        }
                        newSelector = cmbSelector.SelectedItem.ToString();
                        break;

                    case DoubleClickAction.DoubleClickMode.Self:
                    case DoubleClickAction.DoubleClickMode.LastTarget:
                        // No validation needed
                        break;

                    case DoubleClickAction.DoubleClickMode.Alias:
                        if (string.IsNullOrWhiteSpace(txtAlias.Text))
                        {
                            MessageBox.Show("Alias name cannot be empty.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return (false, mode, serial, graphic, color, selector, aliasName);
                        }
                        newAliasName = txtAlias.Text.Trim().ToLower();
                        break;
                }

                return (true, newMode, newSerial, newGraphic, newColor, newSelector, newAliasName);
            }

            return (false, mode, serial, graphic, color, selector, aliasName);
        }

        private void InsertArmDisarmMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Show dialog with default values: mode "Arm", serial 0, hand "Right"
            var result = ShowEditArmDisarmDialog("Arm", 0, "Right");

            if (result.success)
            {
                var armDisarmAction = new RazorEnhanced.Macros.Actions.ArmDisarmAction(result.mode, result.serial, result.hand);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, armDisarmAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted {result.mode} ({result.hand}) at position {insertIndex + 1}", 88);
            }
        }
        private void EditArmDisarmMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.ArmDisarmAction armDisarmAction)
            {
                var result = ShowEditArmDisarmDialog(armDisarmAction.Mode, armDisarmAction.ItemSerial, armDisarmAction.Hand);

                if (result.success)
                {
                    armDisarmAction.Mode = result.mode;
                    armDisarmAction.ItemSerial = result.serial;
                    armDisarmAction.Hand = result.hand;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage($"{result.mode} action updated", 88);
                }
            }
        }
        private void EditArmDisarmAction(RazorEnhanced.Macros.Actions.ArmDisarmAction armDisarmAction, Macro macro, int actionIndex)
        {
            var result = ShowEditArmDisarmDialog(armDisarmAction.Mode, armDisarmAction.ItemSerial, armDisarmAction.Hand);

            if (result.success)
            {
                armDisarmAction.Mode = result.mode;
                armDisarmAction.ItemSerial = result.serial;
                armDisarmAction.Hand = result.hand;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionArmDisarm()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.ArmDisarmAction;
            }

            return false;
        }
        private (bool success, string mode, int serial, string hand) ShowEditArmDisarmDialog(string mode, int serial, string hand)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Arm/Disarm Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Mode selector
            Label lblMode = new Label { Left = 20, Top = 20, Text = "Mode:", Width = 80 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 110,
                Top = 20,
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Arm", "Disarm" });
            cmbMode.SelectedItem = string.IsNullOrEmpty(mode) ? "Arm" : mode;

            // Serial input (only for Arm)
            Label lblSerial = new Label { Left = 20, Top = 60, Text = "Item Serial (hex):", Width = 120 };
            TextBox txtSerial = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}"
            };

            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 150,
                Top = 90,
                Width = 80
            };

            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item to arm...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        // Find the item to ensure it's valid (optional, but matches other dialogs)
                        var item = Items.FindBySerial((int)targetSerial);
                        if (item != null)
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtSerial.Text = $"0x{item.Serial:X8}";
                                Misc.SendMessage($"Set to item: 0x{item.Serial:X8}", 88);
                            }));
                        }
                        else
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtSerial.Text = $"0x{targetSerial:X8}";
                                Misc.SendMessage($"Set to serial: 0x{targetSerial:X8}", 88);
                            }));
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Hand selector
            Label lblHand = new Label { Left = 20, Top = 130, Text = "Hand:", Width = 120 };
            ComboBox cmbHand = new ComboBox
            {
                Left = 150,
                Top = 130,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Default options
            cmbHand.Items.AddRange(new string[] { "Left", "Right" });
            cmbHand.SelectedItem = hand;
            if (cmbHand.SelectedIndex == -1) cmbHand.SelectedIndex = 1; // Default to Right

            // Update hand options based on mode
            void UpdateHandOptions()
            {
                string selected = cmbHand.SelectedItem?.ToString() ?? "Right";
                cmbHand.Items.Clear();
                if (cmbMode.SelectedItem.ToString() == "Disarm")
                    cmbHand.Items.AddRange(new string[] { "Left", "Right", "Both" });
                else
                    cmbHand.Items.AddRange(new string[] { "Left", "Right" });

                cmbHand.SelectedItem = selected;
                if (cmbHand.SelectedIndex == -1) cmbHand.SelectedIndex = 1;
            }
            cmbMode.SelectedIndexChanged += (s, ev) => UpdateHandOptions();
            UpdateHandOptions();

            // Enable/disable serial controls based on mode
            void UpdateSerialControls()
            {
                bool isArm = cmbMode.SelectedItem.ToString() == "Arm";
                lblSerial.Enabled = txtSerial.Enabled = btnTarget.Enabled = isArm;
            }
            cmbMode.SelectedIndexChanged += (s, ev) => UpdateSerialControls();
            UpdateSerialControls();

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 150,
                Width = 80,
                Top = 180,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 240,
                Width = 80,
                Top = 180,
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblHand);
            dialog.Controls.Add(cmbHand);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string newMode = cmbMode.SelectedItem.ToString();
                int newSerial = 0;
                if (newMode == "Arm")
                {
                    string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                    if (string.IsNullOrEmpty(serialStr) || !int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                    {
                        MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345).",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return (false, newMode, serial, hand);
                    }
                }
                string newHand = cmbHand.SelectedItem?.ToString() ?? "Right";
                return (true, newMode, newSerial, newHand);
            }

            return (false, mode, serial, hand);
        }


        private void InsertUseContextMenuMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowEditUseContextMenuDialog("", -1, "");

            if (result.success)
            {
                RazorEnhanced.Macros.Actions.UseContextMenuAction action;
                if (!string.IsNullOrWhiteSpace(result.menuName))
                    action = new RazorEnhanced.Macros.Actions.UseContextMenuAction(result.targetSerialOrAlias, result.menuName);
                else
                    action = new RazorEnhanced.Macros.Actions.UseContextMenuAction(result.targetSerialOrAlias, result.menuIndex);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Use Context Menu at position {insertIndex + 1}", 88);
            }
        }
        private void EditUseContextMenuMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.UseContextMenuAction useContextMenuAction)
            {
                var result = ShowEditUseContextMenuDialog(useContextMenuAction.TargetSerialOrAlias, useContextMenuAction.MenuIndex, useContextMenuAction.MenuName);

                if (result.success)
                {
                    useContextMenuAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                    useContextMenuAction.MenuIndex = result.menuIndex;
                    useContextMenuAction.MenuName = result.menuName;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Use Context Menu action updated", 88);
                }
            }
        }
        private void EditUseContextMenuAction(RazorEnhanced.Macros.Actions.UseContextMenuAction useContextMenuAction, Macro macro, int actionIndex)
        {
            var result = ShowEditUseContextMenuDialog(useContextMenuAction.TargetSerialOrAlias, useContextMenuAction.MenuIndex, useContextMenuAction.MenuName);

            if (result.success)
            {
                useContextMenuAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                useContextMenuAction.MenuIndex = result.menuIndex;
                useContextMenuAction.MenuName = result.menuName;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionUseContextMenu()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.UseContextMenuAction;
            }

            return false;
        }
        private (bool success, string targetSerialOrAlias, int menuIndex, string menuName) ShowEditUseContextMenuDialog(string targetSerialOrAlias, int menuIndex, string menuName)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 290,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Use Context Menu Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTarget = new Label { Left = 20, Top = 20, Text = "Target (serial/alias):", Width = 120 };
            TextBox txtTarget = new TextBox
            {
                Left = 150,
                Top = 20,
                Width = 200,
                Text = targetSerialOrAlias ?? ""
            };

            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 150,
                Top = 50,
                Width = 80
            };

            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item or mobile for context menu...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtTarget.Text = $"0x{(int)targetSerial:X8}";
                            Misc.SendMessage($"Set to serial: 0x{(int)targetSerial:X8}", 88);
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Label lblMenuIndex = new Label { Left = 20, Top = 90, Text = "Menu Index:", Width = 120 };
            TextBox txtMenuIndex = new TextBox
            {
                Left = 150,
                Top = 90,
                Width = 200,
                Text = menuIndex >= 0 ? menuIndex.ToString() : ""
            };

            Label lblMenuName = new Label { Left = 20, Top = 130, Text = "Menu Name (optional):", Width = 120 };
            TextBox txtMenuName = new TextBox
            {
                Left = 150,
                Top = 130,
                Width = 200,
                Text = menuName ?? ""
            };

            Label lblHint = new Label
            {
                Left = 20,
                Top = 170,
                Width = 370,
                Height = 40,
                Text = "If Menu Name is set, it will be used instead of Menu Index.\nYou can use a serial (0x...) or an alias (e.g. 'findfound')."
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 150,
                Width = 80,
                Top = 210,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 240,
                Width = 80,
                Top = 210,
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblTarget);
            dialog.Controls.Add(txtTarget);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblMenuIndex);
            dialog.Controls.Add(txtMenuIndex);
            dialog.Controls.Add(lblMenuName);
            dialog.Controls.Add(txtMenuName);
            dialog.Controls.Add(lblHint);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string targetVal = txtTarget.Text.Trim();
                string menuNameVal = txtMenuName.Text.Trim();
                int newMenuIndex = -1;
                if (string.IsNullOrEmpty(menuNameVal))
                {
                    if (!int.TryParse(txtMenuIndex.Text, out newMenuIndex))
                    {
                        MessageBox.Show("Invalid menu index. Please enter a number or specify a menu name.",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return (false, targetSerialOrAlias, menuIndex, menuName);
                    }
                }

                return (true, targetVal, newMenuIndex, menuNameVal);
            }

            return (false, targetSerialOrAlias, menuIndex, menuName);
        }


        private void InsertRenameMobileMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowEditRenameMobileDialog(0, "");

            if (result.success)
            {
                var renameMobileAction = new RazorEnhanced.Macros.Actions.RenameMobileAction(result.serial, result.name);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, renameMobileAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Rename Mobile at position {insertIndex + 1}", 88);
            }
        }
        private void EditRenameMobileMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.RenameMobileAction renameMobileAction)
            {
                var result = ShowEditRenameMobileDialog(renameMobileAction.Serial, renameMobileAction.Name);

                if (result.success)
                {
                    renameMobileAction.Serial = result.serial;
                    renameMobileAction.Name = result.name;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Rename Mobile action updated", 88);
                }
            }
        }
        private void EditRenameMobileAction(RazorEnhanced.Macros.Actions.RenameMobileAction renameMobileAction, Macro macro, int actionIndex)
        {
            var result = ShowEditRenameMobileDialog(renameMobileAction.Serial, renameMobileAction.Name);

            if (result.success)
            {
                renameMobileAction.Serial = result.serial;
                renameMobileAction.Name = result.name;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionRenameMobile()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.RenameMobileAction;
            }

            return false;
        }
        private (bool success, int serial, string name) ShowEditRenameMobileDialog(int serial, string name)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Rename Mobile Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblSerial = new Label { Left = 20, Top = 20, Text = "Mobile Serial (hex):", Width = 120 };
            TextBox txtSerial = new TextBox
            {
                Left = 150,
                Top = 20,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}"
            };

            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 18,
                Width = 40
            };

            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a mobile to rename...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        var mobile = Mobiles.FindBySerial((int)targetSerial);
                        if (mobile != null)
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtSerial.Text = $"0x{mobile.Serial:X8}";
                                Misc.SendMessage($"Set to mobile: {mobile.Name} (0x{mobile.Serial:X8})", 88);
                            }));
                        }
                        else
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtSerial.Text = $"0x{targetSerial:X8}";
                                Misc.SendMessage($"Set to serial: 0x{targetSerial:X8}", 88);
                            }));
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Label lblName = new Label { Left = 20, Top = 60, Text = "New Name:", Width = 120 };
            TextBox txtName = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 200,
                Text = name ?? ""
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Left = 150,
                Width = 80,
                Top = 110,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 240,
                Width = 80,
                Top = 110,
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblName);
            dialog.Controls.Add(txtName);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                int newSerial = 0;
                if (string.IsNullOrEmpty(serialStr) || !int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial))
                {
                    MessageBox.Show("Invalid serial value. Please enter a hex serial (e.g., 0x00012345).",
                        "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, serial, name);
                }

                string newName = txtName.Text.Trim();
                if (string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show("Name cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, serial, name);
                }

                return (true, newSerial, newName);
            }

            return (false, serial, name);
        }

        private void InsertWhileMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            // Show a dialog to configure the While condition (reuse your If dialog)
            var defaultWhile = new WhileAction(); // WhileAction usually has the same fields as IfAction
            var result = ShowWhileConditionDialog(defaultWhile);

            if (result.success)
            {
                var whileAction = new RazorEnhanced.Macros.Actions.WhileAction(
                    result.type, result.op, result.value, result.graphic, result.color,
                    result.skillName, result.valueToken, result.booleanValue, result.presetName,
                    result.buffName, result.statType, result.statusType, result.rangeMode,
                    result.rangeSerial, result.rangeGraphic, result.rangeColor,
                    result.findEntityMode, result.findEntityLocation, result.findContainerSerial, result.findRange, result.findStoreSerial);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, whileAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted While at position {insertIndex + 1}", 88);
            }
        }
        private void EditWhileMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.WhileAction whileAction)
            {
                var result = ShowWhileConditionDialog(whileAction);

                if (result.success)
                {
                    whileAction.Type = result.type;
                    whileAction.StatType = result.statType;
                    whileAction.StatusType = result.statusType;
                    whileAction.Op = result.op;
                    whileAction.Value = result.value;
                    whileAction.ValueToken = result.valueToken;
                    whileAction.BooleanValue = result.booleanValue;
                    whileAction.Graphic = result.graphic;
                    whileAction.Color = result.color;
                    whileAction.SkillName = result.skillName;
                    whileAction.PresetName = result.presetName;
                    whileAction.BuffName = result.buffName;
                    whileAction.RangeMode = result.rangeMode;
                    whileAction.RangeSerial = result.rangeSerial;
                    whileAction.RangeGraphic = result.rangeGraphic;
                    whileAction.RangeColor = result.rangeColor;
                    whileAction.FindEntityMode = result.findEntityMode;
                    whileAction.FindEntityLocation = result.findEntityLocation;
                    whileAction.FindContainerSerial = result.findContainerSerial;
                    whileAction.FindRange = result.findRange;
                    whileAction.FindStoreSerial = result.findStoreSerial;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("While condition updated", 88);
                }
            }
        }
        private void EditWhileAction(RazorEnhanced.Macros.Actions.WhileAction whileAction, Macro macro, int actionIndex)
        {
            var result = ShowWhileConditionDialog(whileAction);

            if (result.success)
            {
                whileAction.Type = result.type;
                whileAction.StatType = result.statType;
                whileAction.StatusType = result.statusType;
                whileAction.Op = result.op;
                whileAction.Value = result.value;
                whileAction.ValueToken = result.valueToken;
                whileAction.BooleanValue = result.booleanValue;
                whileAction.Graphic = result.graphic;
                whileAction.Color = result.color;
                whileAction.SkillName = result.skillName;
                whileAction.PresetName = result.presetName;
                whileAction.BuffName = result.buffName;
                whileAction.RangeMode = result.rangeMode;
                whileAction.RangeSerial = result.rangeSerial;
                whileAction.RangeGraphic = result.rangeGraphic;
                whileAction.RangeColor = result.rangeColor;
                whileAction.FindEntityMode = result.findEntityMode;
                whileAction.FindEntityLocation = result.findEntityLocation;
                whileAction.FindContainerSerial = result.findContainerSerial;
                whileAction.FindRange = result.findRange;
                whileAction.FindStoreSerial = result.findStoreSerial;

                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionWhile()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.WhileAction;
            }

            return false;
        }
        private string FormatWhileConditionDisplay(RazorEnhanced.Macros.Actions.WhileAction whileAction)
        {
            if (whileAction.Type == IfAction.ConditionType.PlayerStats)
            {
                string statName = whileAction.StatType.ToString();
                string displayValue = string.IsNullOrEmpty(whileAction.ValueToken) ? whileAction.Value.ToString() : whileAction.ValueToken;
                return $"{statName} {GetOperatorSymbol(whileAction.Op)} {displayValue}";
            }

            if (whileAction.Type == IfAction.ConditionType.PlayerStatus)
            {
                string prefix = whileAction.BooleanValue ? "" : "Not ";
                return $"{prefix}{whileAction.StatusType}";
            }

            if (whileAction.Type == IfAction.ConditionType.TargetExists)
            {
                string prefix = whileAction.BooleanValue ? "" : "Not ";
                return $"{prefix}TargetExists";
            }

            if (whileAction.Type == IfAction.ConditionType.Find)
            {
                string modeStr = whileAction.FindEntityMode == IfAction.FindMode.Item ? "Item" : "Mobile";
                string locationStr = "";

                if (whileAction.FindEntityMode == IfAction.FindMode.Item)
                {
                    switch (whileAction.FindEntityLocation)
                    {
                        case IfAction.FindLocation.Backpack:
                            locationStr = "Backpack";
                            break;
                        case IfAction.FindLocation.Container:
                            locationStr = $"Container 0x{whileAction.FindContainerSerial:X8}";
                            break;
                        case IfAction.FindLocation.Ground:
                            locationStr = $"Ground ({whileAction.FindRange} tiles)";
                            break;
                    }
                }
                else
                {
                    locationStr = $"Range {whileAction.FindRange}";
                }

                string colorStr = whileAction.Color == -1 ? "Any" : $"0x{whileAction.Color:X4}";
                return $"Find {modeStr}: 0x{whileAction.Graphic:X4} ({colorStr}) in {locationStr}";
            }

            if (whileAction.Type == IfAction.ConditionType.InJournal)
            {
                string prefix = whileAction.BooleanValue ? "" : "Not ";
                string searchText = string.IsNullOrEmpty(whileAction.ValueToken) ? "(empty)" : whileAction.ValueToken;
                return $"{prefix}InJournal: \"{searchText}\"";
            }

            if (whileAction.Type == IfAction.ConditionType.BuffExists)
            {
                string prefix = whileAction.BooleanValue ? "" : "Not ";
                string buffName = string.IsNullOrEmpty(whileAction.BuffName) ? "(none)" : whileAction.BuffName;
                return $"{prefix}BuffExists: {buffName}";
            }

            if (whileAction.Type == IfAction.ConditionType.Skill)
            {
                string displayValue = string.IsNullOrEmpty(whileAction.ValueToken) ? whileAction.Value.ToString() : whileAction.ValueToken;
                return $"{whileAction.SkillName} {GetOperatorSymbol(whileAction.Op)} {displayValue}";
            }

            if (whileAction.Type == IfAction.ConditionType.InRange)
            {
                string targetDesc;
                switch (whileAction.RangeMode)
                {
                    case IfAction.InRangeMode.LastTarget:
                        targetDesc = "Last Target";
                        break;
                    case IfAction.InRangeMode.Serial:
                        targetDesc = whileAction.RangeSerial == 0 ? "(no serial)" : $"Serial 0x{whileAction.RangeSerial:X8}";
                        break;
                    case IfAction.InRangeMode.ItemType:
                        string itemColorStr = whileAction.RangeColor == -1 ? "Any" : $"0x{whileAction.RangeColor:X4}";
                        targetDesc = $"ItemType 0x{whileAction.RangeGraphic:X4} ({itemColorStr})";
                        break;
                    case IfAction.InRangeMode.MobileType:
                        string mobileColorStr = whileAction.RangeColor == -1 ? "Any" : $"0x{whileAction.RangeColor:X4}";
                        targetDesc = $"MobileType 0x{whileAction.RangeGraphic:X4} ({mobileColorStr})";
                        break;
                    default:
                        targetDesc = "Unknown";
                        break;
                }
                return $"{targetDesc} InRange {GetOperatorSymbol(whileAction.Op)} {whileAction.Value}";
            }

            if (whileAction.Type == IfAction.ConditionType.Count)
            {
                string presetDisplay = string.IsNullOrEmpty(whileAction.PresetName) || whileAction.PresetName == "Custom"
                    ? ""
                    : $"{whileAction.PresetName} ";
                string colorStr = whileAction.Color == -1 ? "Any" : $"0x{whileAction.Color:X4}";
                return $"Count: {presetDisplay}(0x{whileAction.Graphic:X4}, Color: {colorStr}) {GetOperatorSymbol(whileAction.Op)} {whileAction.Value}";
            }

            // Fallback
            string value = string.IsNullOrEmpty(whileAction.ValueToken) ? whileAction.Value.ToString() : whileAction.ValueToken;
            return $"{whileAction.Type} {GetOperatorSymbol(whileAction.Op)} {value}";
        }

        private (bool success, IfAction.ConditionType type, IfAction.Operator op, int value,
            int graphic, int color, string skillName, string valueToken, bool booleanValue,
            string presetName, string buffName, IfAction.PlayerStatType statType,
            IfAction.PlayerStatusType statusType, IfAction.InRangeMode rangeMode,
            int rangeSerial, int rangeGraphic, int rangeColor,
            IfAction.FindMode findEntityMode, IfAction.FindLocation findEntityLocation,
            int findContainerSerial, int findRange, bool findStoreSerial) ShowWhileConditionDialog(RazorEnhanced.Macros.Actions.WhileAction whileAction)
        {
            // Use a temporary IfAction to leverage the existing dialog and logic
            var tempIf = new IfAction
            {
                Type = whileAction.Type,
                StatType = whileAction.StatType,
                StatusType = whileAction.StatusType,
                Op = whileAction.Op,
                Value = whileAction.Value,
                ValueToken = whileAction.ValueToken,
                BooleanValue = whileAction.BooleanValue,
                Graphic = whileAction.Graphic,
                Color = whileAction.Color,
                SkillName = whileAction.SkillName,
                PresetName = whileAction.PresetName,
                BuffName = whileAction.BuffName,
                RangeMode = whileAction.RangeMode,
                RangeSerial = whileAction.RangeSerial,
                RangeGraphic = whileAction.RangeGraphic,
                RangeColor = whileAction.RangeColor,
                FindEntityMode = whileAction.FindEntityMode,
                FindEntityLocation = whileAction.FindEntityLocation,
                FindContainerSerial = whileAction.FindContainerSerial,
                FindRange = whileAction.FindRange,
                FindStoreSerial = whileAction.FindStoreSerial
            };

            // Use the same dialog as If/ElseIf
            var result = ShowIfConditionDialog(tempIf);

            // If user confirmed, return the values (all IfAction enums)
            if (result.success)
            {
                return result;
            }

            // If cancelled, return the current values from WhileAction (as IfAction enums)
            return (false, whileAction.Type, whileAction.Op, whileAction.Value, whileAction.Graphic, whileAction.Color,
                whileAction.SkillName, whileAction.ValueToken, whileAction.BooleanValue, whileAction.PresetName,
                whileAction.BuffName, whileAction.StatType, whileAction.StatusType, whileAction.RangeMode,
                whileAction.RangeSerial, whileAction.RangeGraphic, whileAction.RangeColor,
                whileAction.FindEntityMode, whileAction.FindEntityLocation, whileAction.FindContainerSerial, whileAction.FindRange, whileAction.FindStoreSerial);
        }


        private void InsertMessagingMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowMessagingDialog(new MessagingAction());
            if (result.success)
            {
                var action = new MessagingAction(result.type, result.message, result.hue, result.targetSerialOrAlias);
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Messaging action at position {insertIndex + 1}", 88);
            }
        }
        private void EditMessagingMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            if (macro.Actions[actionIndex] is MessagingAction messagingAction)
            {
                var result = ShowMessagingDialog(messagingAction);
                if (result.success)
                {
                    messagingAction.Type = result.type;
                    messagingAction.Message = result.message;
                    messagingAction.Hue = result.hue;
                    messagingAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Messaging action updated", 88);
                }
            }
        }
        private void EditMessagingAction(MessagingAction messagingAction, Macro macro, int actionIndex)
        {
            var result = ShowMessagingDialog(messagingAction);

            if (result.success)
            {
                messagingAction.Type = result.type;
                messagingAction.Message = result.message;
                messagingAction.Hue = result.hue;
                messagingAction.TargetSerialOrAlias = result.targetSerialOrAlias;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionMessaging()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is MessagingAction;
        }

        private (bool success, MessagingAction.MessageType type, string message, int hue, string targetSerialOrAlias)
            ShowMessagingDialog(MessagingAction action)
        {
            Form dialog = new Form
            {
                Width = 500,
                Height = 640,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Messaging Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Message Type
            Label lblType = new Label { Left = 20, Top = 20, Text = "Message Type:", Width = 120 };
            ComboBox cmbType = new ComboBox
            {
                Left = 150,
                Top = 20,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(Enum.GetNames(typeof(MessagingAction.MessageType)));
            cmbType.SelectedIndex = (int)action.Type;

            // Message
            Label lblMessage = new Label { Left = 20, Top = 60, Text = "Message:", Width = 120 };
            TextBox txtMessage = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 300,
                Text = action.Message ?? ""
            };

            // Hue
            Label lblHue = new Label { Left = 20, Top = 100, Text = "Hue:", Width = 120 };
            TextBox txtHue = new TextBox
            {
                Left = 150,
                Top = 100,
                Width = 100,
                Text = action.Hue.ToString()
            };

            // TargetSerialOrAlias (for Overhead)
            Label lblTarget = new Label { Left = 20, Top = 140, Text = "Target (serial/alias):", Width = 120 };
            TextBox txtTarget = new TextBox
            {
                Left = 150,
                Top = 140,
                Width = 230,
                Text = action.TargetSerialOrAlias ?? ""
            };
            Button btnTarget = new Button
            {
                Text = "Target",
                Left = 390,
                Top = 138,
                Width = 60
            };
            btnTarget.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a mobile or item for the overhead message...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtTarget.Text = $"0x{(int)targetSerial:X8}";
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Label lblHint = new Label
            {
                Left = 150,
                Top = 170,
                Width = 300,
                Height = 40,
                Text = "Leave blank for no target.\nYou can use a serial (0x...) or an alias (e.g. 'findfound').",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Info label
            Label lblInfo = new Label
            {
                Left = 20,
                Top = 260,
                Width = 440,
                Height = 40,
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // OK/Cancel
            Button btnOK = new Button
            {
                Text = "OK",
                Left = 250,
                Width = 90,
                Top = 300,
                DialogResult = DialogResult.OK
            };
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Left = 350,
                Width = 90,
                Top = 300,
                DialogResult = DialogResult.Cancel
            };

            // Visibility logic
            void UpdateVisibility()
            {
                var type = (MessagingAction.MessageType)cmbType.SelectedIndex;
                lblHue.Visible = txtHue.Visible =
                    (type == MessagingAction.MessageType.Say ||
                     type == MessagingAction.MessageType.Yell ||
                     type == MessagingAction.MessageType.Whisper ||
                     type == MessagingAction.MessageType.Emote ||
                     type == MessagingAction.MessageType.Overhead ||
                     type == MessagingAction.MessageType.System);

                lblTarget.Visible = txtTarget.Visible = btnTarget.Visible = (type == MessagingAction.MessageType.Overhead);
                lblHint.Visible = (type == MessagingAction.MessageType.Overhead);

                // Info text
                switch (type)
                {
                    case MessagingAction.MessageType.Say:
                        lblInfo.Text = "Say a message (optionally colored with Hue).";
                        dialog.Height = 260;
                        lblInfo.Top = 140;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                    case MessagingAction.MessageType.Yell:
                        lblInfo.Text = "Yell a message (optionally colored with Hue).";
                        dialog.Height = 260;
                        lblInfo.Top = 140;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                    case MessagingAction.MessageType.Whisper:
                        lblInfo.Text = "Whisper a message (optionally colored with Hue).";
                        dialog.Height = 260;
                        lblInfo.Top = 140;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                    case MessagingAction.MessageType.Emote:
                        lblInfo.Text = "Emote a message (optionally colored with Hue).";
                        dialog.Height = 260;
                        lblInfo.Top = 140;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                    case MessagingAction.MessageType.Overhead:
                        lblInfo.Text = "Show a message over a mobile or item (Hue and Target required).";
                        dialog.Height = 380;
                        lblInfo.Top = 220;
                        lblHint.Top = 180;
                        btnOK.Top = 260;
                        btnCancel.Top = 260;
                        break;
                    case MessagingAction.MessageType.System:
                        lblInfo.Text = "Send a system message (optionally colored with Hue).";
                        dialog.Height = 260;
                        lblInfo.Top = 140;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                    case MessagingAction.MessageType.General:
                        lblInfo.Text = "Send a message to the General chat channel.";
                        dialog.Height = 220;
                        lblInfo.Top = 100;
                        btnOK.Top = 140;
                        btnCancel.Top = 140;
                        break;
                    case MessagingAction.MessageType.Guild:
                        lblInfo.Text = "Send a message to the Guild chat channel.";
                        dialog.Height = 220;
                        lblInfo.Top = 100;
                        btnOK.Top = 140;
                        btnCancel.Top = 140;
                        break;
                    case MessagingAction.MessageType.Alliance:
                        lblInfo.Text = "Send a message to the Alliance chat channel.";
                        dialog.Height = 220;
                        lblInfo.Top = 100;
                        btnOK.Top = 140;
                        btnCancel.Top = 140;
                        break;
                    case MessagingAction.MessageType.Party:
                        lblInfo.Text = "Send a message to the Party chat channel.";
                        dialog.Height = 220;
                        lblInfo.Top = 100;
                        btnOK.Top = 140;
                        btnCancel.Top = 140;
                        break;
                }
            }

            cmbType.SelectedIndexChanged += (s, ev) => UpdateVisibility();
            UpdateVisibility();

            // Add controls
            dialog.Controls.Add(lblType);
            dialog.Controls.Add(cmbType);
            dialog.Controls.Add(lblMessage);
            dialog.Controls.Add(txtMessage);
            dialog.Controls.Add(lblHue);
            dialog.Controls.Add(txtHue);
            dialog.Controls.Add(lblTarget);
            dialog.Controls.Add(txtTarget);
            dialog.Controls.Add(btnTarget);
            dialog.Controls.Add(lblHint);
            dialog.Controls.Add(lblInfo);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var type = (MessagingAction.MessageType)cmbType.SelectedIndex;
                string message = txtMessage.Text;
                int hue = 0;
                int.TryParse(txtHue.Text, out hue);
                string targetSerialOrAlias = txtTarget.Text.Trim();

                // Basic validation
                if (string.IsNullOrWhiteSpace(message) &&
                    type != MessagingAction.MessageType.Emote &&
                    type != MessagingAction.MessageType.System)
                {
                    MessageBox.Show("Message cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, type, action.Message, action.Hue, action.TargetSerialOrAlias);
                }

                if (type == MessagingAction.MessageType.Overhead && string.IsNullOrWhiteSpace(targetSerialOrAlias))
                {
                    MessageBox.Show("Target is required for Overhead messages.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return (false, type, action.Message, action.Hue, action.TargetSerialOrAlias);
                }

                return (true, type, message, hue, targetSerialOrAlias);
            }

            return (false, action.Type, action.Message, action.Hue, action.TargetSerialOrAlias);
        }


        private void InsertMovementActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowMovementDialog(new MovementAction());
            if (result.success)
            {
                var action = result.action;
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Movement action at position {insertIndex + 1}", 88);
            }
        }
        private void EditMovementActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            if (macro.Actions[actionIndex] is MovementAction movementAction)
            {
                var result = ShowMovementDialog(movementAction);
                if (result.success)
                {
                    macro.Actions[actionIndex] = result.action;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Movement action updated", 88);
                }
            }
        }
        private void EditMovementAction(MovementAction movementAction, Macro macro, int actionIndex)
        {
            var result = ShowMovementDialog(movementAction);
            if (result.success)
            {
                macro.Actions[actionIndex] = result.action;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionMovement()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.MovementAction;
        }
        private (bool success, MovementAction action) ShowMovementDialog(MovementAction action)
        {
            Form dialog = new Form
            {
                Width = 450,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Movement Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Movement Type selector
            Label lblType = new Label { Left = 20, Top = 20, Text = "Movement Type:", Width = 120 };
            ComboBox cmbType = new ComboBox
            {
                Left = 150,
                Top = 20,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(Enum.GetNames(typeof(MovementAction.MovementType)));
            cmbType.SelectedIndex = (int)action.Type;

            // Direction for Walk/Run
            Label lblDirection = new Label { Left = 20, Top = 60, Text = "Direction:", Width = 120 };
            ComboBox cmbDirection = new ComboBox
            {
                Left = 150,
                Top = 60,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDirection.Items.AddRange(new string[] { "North", "Northeast", "East", "Southeast", "South", "Southwest", "West", "Northwest" });
            cmbDirection.SelectedItem = string.IsNullOrEmpty(action.Direction) ? "North" : action.Direction;

            // Pathfind fields (mirroring ShowPathfindToDialog)
            Label lblMode = new Label { Left = 20, Top = 100, Text = "Pathfind Mode:", Width = 120 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 150,
                Top = 100,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Coordinates", "Specific Serial", "Alias" });
            cmbMode.SelectedIndex = (int)action.Mode;

            // Coordinates section
            Label lblCoordNote = new Label
            {
                Left = 150,
                Top = 135,
                Width = 250,
                Height = 25,
                Text = "Set static coordinates to pathfind to",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnTargetCoords = new Button
            {
                Text = "Target Location/Entity",
                Left = 150,
                Top = 165,
                Width = 250
            };

            Label lblX = new Label { Left = 20, Top = 205, Text = "X:", Width = 30 };
            TextBox txtX = new TextBox { Left = 60, Top = 205, Width = 60, Text = action.X.ToString() };
            Label lblY = new Label { Left = 130, Top = 205, Text = "Y:", Width = 30 };
            TextBox txtY = new TextBox { Left = 170, Top = 205, Width = 60, Text = action.Y.ToString() };
            Label lblZ = new Label { Left = 240, Top = 205, Text = "Z:", Width = 30 };
            TextBox txtZ = new TextBox { Left = 280, Top = 205, Width = 60, Text = action.Z.ToString() };

            // Serial section
            Label lblSerial = new Label { Left = 20, Top = 140, Text = "Serial (hex):", Width = 120 };
            TextBox txtSerial = new TextBox
            {
                Left = 150,
                Top = 140,
                Width = 200,
                Text = action.Serial == 0 ? "" : $"0x{action.Serial:X8}"
            };

            Button btnTargetSerial = new Button
            {
                Text = "Target",
                Left = 350,
                Top = 138,
                Width = 80
            };

            Label lblSerialNote = new Label
            {
                Left = 150,
                Top = 170,
                Width = 250,
                Height = 40,
                Text = "Pathfind to this entity's location at runtime\n(follows the entity if it moves)",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Alias section
            Label lblAlias = new Label { Left = 20, Top = 140, Text = "Alias Name:", Width = 120 };
            TextBox txtAlias = new TextBox
            {
                Left = 150,
                Top = 140,
                Width = 250,
                Text = action.AliasName ?? ""
            };

            Label lblAliasNote = new Label
            {
                Left = 150,
                Top = 170,
                Width = 250,
                Height = 50,
                Text = "Use 'findfound' or custom alias.\nPathfind to the entity's current location at runtime.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Target Coordinates button handler
            btnTargetCoords.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a location or entity...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    dialog.Invoke(new Action(() =>
                    {
                        txtX.Text = p.X.ToString();
                        txtY.Text = p.Y.ToString();
                        txtZ.Text = p.Z.ToString();
                        Misc.SendMessage($"Set to coordinates: ({p.X}, {p.Y}, {p.Z})", 88);
                    }));
                }));
            };

            // Target Serial button handler
            btnTargetSerial.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an entity to pathfind to...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            int serialValue = (int)targetSerial;
                            txtSerial.Text = $"0x{serialValue:X8}";

                            var mobile = Mobiles.FindBySerial(serialValue);
                            if (mobile != null)
                            {
                                Misc.SendMessage($"Set to mobile: {mobile.Name} (0x{serialValue:X8})", 88);
                            }
                            else
                            {
                                var item = Items.FindBySerial(serialValue);
                                if (item != null)
                                {
                                    Misc.SendMessage($"Set to item: 0x{item.ItemID:X4} (0x{serialValue:X8})", 88);
                                }
                                else
                                {
                                    Misc.SendMessage($"Set to serial: 0x{serialValue:X8}", 88);
                                }
                            }
                        }));
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            Button btnOK = new Button { Text = "OK", Left = 200, Width = 80, Top = 320, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 300, Width = 80, Top = 320, DialogResult = DialogResult.Cancel };

            // Visibility logic
            void UpdateVisibility()
            {
                var type = (MovementAction.MovementType)cmbType.SelectedIndex;
                bool isPathfind = type == MovementAction.MovementType.Pathfind;
                lblDirection.Visible = cmbDirection.Visible = (type == MovementAction.MovementType.Walk || type == MovementAction.MovementType.Run);

                lblMode.Visible = cmbMode.Visible = isPathfind;

                // Hide all pathfind fields by default
                lblCoordNote.Visible = btnTargetCoords.Visible = lblX.Visible = txtX.Visible = lblY.Visible = txtY.Visible = lblZ.Visible = txtZ.Visible = false;
                lblSerial.Visible = txtSerial.Visible = btnTargetSerial.Visible = lblSerialNote.Visible = false;
                lblAlias.Visible = txtAlias.Visible = lblAliasNote.Visible = false;

                if (isPathfind)
                {
                    var mode = (MovementAction.PathfindMode)cmbMode.SelectedIndex;
                    if (mode == MovementAction.PathfindMode.Coordinates)
                    {
                        lblCoordNote.Visible = btnTargetCoords.Visible = lblX.Visible = txtX.Visible = lblY.Visible = txtY.Visible = lblZ.Visible = txtZ.Visible = true;
                    }
                    else if (mode == MovementAction.PathfindMode.Serial)
                    {
                        lblSerial.Visible = txtSerial.Visible = btnTargetSerial.Visible = lblSerialNote.Visible = true;
                    }
                    else if (mode == MovementAction.PathfindMode.Alias)
                    {
                        lblAlias.Visible = txtAlias.Visible = lblAliasNote.Visible = true;
                    }
                }


                var use = (MovementAction.MovementType)cmbType.SelectedIndex;

                switch (use)
                {
                    case MovementAction.MovementType.Walk:
                        dialog.Height = 180;
                        lblDirection.Top = 60;
                        cmbDirection.Top = 60;

                        btnOK.Top = 100;
                        btnCancel.Top = 100;
                        break;
                    case MovementAction.MovementType.Run:
                        dialog.Height = 180;
                        lblDirection.Top = 60;
                        cmbDirection.Top = 60;

                        btnOK.Top = 100;
                        btnCancel.Top = 100;
                        break;
                    case MovementAction.MovementType.Pathfind:
                        dialog.Height = lblAlias.Visible ? 260 : lblX.Visible ? 290 : lblSerial.Visible ? 250 : 400;

                        lblDirection.Top = 60;
                        cmbDirection.Top = 60;

                        lblMode.Top = 60;
                        cmbMode.Top = 60;

                        lblCoordNote.Top = 100;

                        btnTargetCoords.Top = 130;

                        lblSerial.Top = 100;
                        txtSerial.Top = 100;
                        btnTargetSerial.Top = 98;

                        lblSerialNote.Top = 130;

                        lblAlias.Top = 100;
                        txtAlias.Top = 100;
                        lblAliasNote.Top = 130;

                        lblX.Top = 170;
                        txtX.Top = 170;
                        lblY.Top = 170;
                        txtY.Top = 170;
                        lblZ.Top = 170;
                        txtZ.Top = 170;

                        btnOK.Top = lblAlias.Visible ? 180 : lblX.Visible ?  210: lblSerial.Visible ?  170 : 320;
                        btnCancel.Top = lblAlias.Visible ? 180 : lblX.Visible ? 210 : lblSerial.Visible ? 170 : 320;

                        break;
                    default:
                        dialog.Height = 400;
                        lblDirection.Top = 60;
                        cmbDirection.Top = 60;

                        lblMode.Top = 100; 
                        cmbMode.Top = 100;

                        lblCoordNote.Top = 135;

                        btnTargetCoords.Top = 165;

                        lblSerial.Top = 140;
                        txtSerial.Top = 140;
                        btnTargetSerial.Top = 138;

                        lblSerialNote.Top = 170;


                        lblAlias.Top = 140;
                        txtAlias.Top = 140;

                        lblAliasNote.Top = 170;

                        lblX.Top = 205;
                        txtX.Top = 205;
                        lblY.Top = 205;
                        txtY.Top = 205;
                        lblZ.Top = 205;
                        txtZ.Top = 205;

                        btnOK.Top = 320;
                        btnCancel.Top = 320;
                        break;
                }





            }
            cmbType.SelectedIndexChanged += (s, ev) => UpdateVisibility();
            cmbMode.SelectedIndexChanged += (s, ev) => UpdateVisibility();
            UpdateVisibility();

 
            dialog.Controls.Add(lblType);
            dialog.Controls.Add(cmbType);
            dialog.Controls.Add(lblDirection);
            dialog.Controls.Add(cmbDirection);
            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);
            dialog.Controls.Add(lblCoordNote);
            dialog.Controls.Add(btnTargetCoords);
            dialog.Controls.Add(lblX);
            dialog.Controls.Add(txtX);
            dialog.Controls.Add(lblY);
            dialog.Controls.Add(txtY);
            dialog.Controls.Add(lblZ);
            dialog.Controls.Add(txtZ);
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTargetSerial);
            dialog.Controls.Add(lblSerialNote);
            dialog.Controls.Add(lblAlias);
            dialog.Controls.Add(txtAlias);
            dialog.Controls.Add(lblAliasNote);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var type = (MovementAction.MovementType)cmbType.SelectedIndex;
                if (type == MovementAction.MovementType.Walk || type == MovementAction.MovementType.Run)
                {
                    string dir = cmbDirection.SelectedItem?.ToString() ?? "North";
                    return (true, new MovementAction(type, dir));
                }
                else if (type == MovementAction.MovementType.Pathfind)
                {
                    var mode = (MovementAction.PathfindMode)cmbMode.SelectedIndex;
                    int x = 0, y = 0, z = 0, serial = 0;
                    string alias = txtAlias.Text.Trim();
                    if (mode == MovementAction.PathfindMode.Coordinates)
                    {
                        int.TryParse(txtX.Text, out x);
                        int.TryParse(txtY.Text, out y);
                        int.TryParse(txtZ.Text, out z);
                    }
                    else if (mode == MovementAction.PathfindMode.Serial)
                    {
                        string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                        int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out serial);
                    }
                    else if (mode == MovementAction.PathfindMode.Alias)
                    {
                        alias = txtAlias.Text.Trim();
                    }
                    return (true, new MovementAction(mode, x, y, z, serial, alias));
                }
            }
            return (false, action);
        }


        private void InsertBandageMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowBandageDialog(new BandageAction());
            if (result.success)
            {
                var action = result.action;
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Bandage action at position {insertIndex + 1}", 88);
            }
        }
        private void EditBandageActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            if (macro.Actions[actionIndex] is BandageAction bandageAction)
            {
                var result = ShowBandageDialog(bandageAction);
                if (result.success)
                {
                    macro.Actions[actionIndex] = result.action;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Bandage action updated", 88);
                }
            }
        }
        private bool IsSelectedActionBandage()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.BandageAction;
        }
        private void EditBandageAction(BandageAction bandageAction, Macro macro, int actionIndex)
        {
            var result = ShowBandageDialog(bandageAction);
            if (result.success)
            {
                macro.Actions[actionIndex] = result.action;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private (bool success, BandageAction action) ShowBandageDialog(BandageAction action)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 260,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Bandage Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblMode = new Label { Left = 20, Top = 20, Text = "Target Mode:", Width = 100 };
            ComboBox cmbMode = new ComboBox
            {
                Left = 130,
                Top = 20,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(Enum.GetNames(typeof(BandageAction.BandageTargetMode)));
            cmbMode.SelectedIndex = (int)action.TargetMode;

            // Self mode
            Label lblSelfNote = new Label
            {
                Left = 130,
                Top = 60,
                Width = 250,
                Height = 40,
                Text = "Bandage yourself (uses BandageHeal agent settings if enabled).",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            // Serial mode
            Label lblSerial = new Label { Left = 20, Top = 60, Text = "Target Serial (hex):", Width = 100 };
            TextBox txtSerial = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 170,
                Text = action.TargetSerial == 0 ? "" : $"0x{action.TargetSerial:X8}"
            };
            Button btnTargetSerial = new Button
            {
                Text = "Target",
                Left = 300,
                Top = 58,
                Width = 80
            };
            btnTargetSerial.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a mobile to bandage...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    int serialValue = (int)targetSerial;

                    // Check if it's a mobile (regular mount/pet)
                    var mobile = Mobiles.FindBySerial(serialValue);
                    if (mobile != null)
                    {
                        txtSerial.Text = $"0x{serialValue:X8}";
                        Misc.SendMessage($"Set target: {mobile.Name} (0x{serialValue:X8})", 88);
                        return;
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };

            // Alias mode
            Label lblAlias = new Label { Left = 20, Top = 60, Text = "Alias Name:", Width = 100 };
            TextBox txtAlias = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 250,
                Text = action.TargetAlias ?? ""
            };
            Label lblAliasNote = new Label
            {
                Left = 130,
                Top = 90,
                Width = 250,
                Height = 40,
                Text = "Use 'findfound' or any custom alias.",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Button btnOK = new Button { Text = "OK", Left = 200, Width = 80, Top = 180, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 300, Width = 80, Top = 180, DialogResult = DialogResult.Cancel };


            // Visibility logic
            void UpdateVisibility()
            {
                var mode = (BandageAction.BandageTargetMode)cmbMode.SelectedIndex;
                lblSelfNote.Visible = (mode == BandageAction.BandageTargetMode.Self);
                lblSerial.Visible = txtSerial.Visible = btnTargetSerial.Visible = (mode == BandageAction.BandageTargetMode.Serial);
                lblAlias.Visible = txtAlias.Visible = lblAliasNote.Visible = (mode == BandageAction.BandageTargetMode.Alias);
          
            switch(mode)
                {
                    case BandageAction.BandageTargetMode.Self:
                        dialog.Height = 180;
                        btnOK.Top = 100;
                        btnCancel.Top = 100;
                        break;
                    case BandageAction.BandageTargetMode.Serial:
                        dialog.Height = 180;
                        btnOK.Top = 100;
                        btnCancel.Top = 100;
                        break;
                    case BandageAction.BandageTargetMode.Alias:
                        dialog.Height = 210;
                        btnOK.Top = 130;
                        btnCancel.Top = 130;
                        break;
                    default:
                        dialog.Height = 260;
                        btnOK.Top = 180;
                        btnCancel.Top = 180;
                        break;
                }
            
            }
            cmbMode.SelectedIndexChanged += (s, ev) => UpdateVisibility();
            UpdateVisibility();


            dialog.Controls.Add(lblMode);
            dialog.Controls.Add(cmbMode);
            dialog.Controls.Add(lblSelfNote);
            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTargetSerial);
            dialog.Controls.Add(lblAlias);
            dialog.Controls.Add(txtAlias);
            dialog.Controls.Add(lblAliasNote);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var mode = (BandageAction.BandageTargetMode)cmbMode.SelectedIndex;
                int serial = 0;
                string alias = txtAlias.Text.Trim();

                if (mode == BandageAction.BandageTargetMode.Serial)
                {
                    string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                    int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out serial);
                }
                else if (mode == BandageAction.BandageTargetMode.Alias)
                {
                    alias = txtAlias.Text.Trim();
                }

                var newAction = new BandageAction
                {
                    TargetMode = mode,
                    TargetSerial = serial,
                    TargetAlias = alias
                };
                return (true, newAction);
            }
            return (false, action);
        }



        private void InsertTargetResourceMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowTargetResourceDialog(new RazorEnhanced.Macros.Actions.TargetResourceAction());
            if (result.success)
            {
                var action = result.action;
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Target Resource action at position {insertIndex + 1}", 88);
            }
        }
        private void EditTargetResourceActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            if (macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.TargetResourceAction targetResourceAction)
            {
                var result = ShowTargetResourceDialog(targetResourceAction);
                if (result.success)
                {
                    macro.Actions[actionIndex] = result.action;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Target Resource action updated", 88);
                }
            }
        }
        private void EditTargetResourceAction(RazorEnhanced.Macros.Actions.TargetResourceAction targetResourceAction, Macro macro, int actionIndex)
        {
            var result = ShowTargetResourceDialog(targetResourceAction);
            if (result.success)
            {
                macro.Actions[actionIndex] = result.action;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionTargetResource()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.TargetResourceAction;
        }
        private (bool success, RazorEnhanced.Macros.Actions.TargetResourceAction action) ShowTargetResourceDialog(RazorEnhanced.Macros.Actions.TargetResourceAction action)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 260,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Target Resource Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblToolType = new Label { Left = 20, Top = 20, Text = "Tool Type (hex):", Width = 110 };
            TextBox txtToolType = new TextBox
            {
                Left = 130,
                Top = 20,
                Width = 120,
                Text = $"0x{action.ToolType:X4}"
            };
            Button btnTargetTool = new Button
            {
                Text = "Target",
                Left = 260,
                Top = 18,
                Width = 80
            };

            Label lblToolColor = new Label { Left = 20, Top = 60, Text = "Tool Color (-1 = any):", Width = 110 };
            TextBox txtToolColor = new TextBox
            {
                Left = 130,
                Top = 60,
                Width = 120,
                Text = action.ToolColor.ToString()
            };

            Label lblResource = new Label { Left = 20, Top = 100, Text = "Resource:", Width = 110 };
            ComboBox cmbResource = new ComboBox
            {
                Left = 130,
                Top = 100,
                Width = 210,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button btnOK = new Button { Text = "OK", Left = 200, Width = 80, Top = 190, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 300, Width = 80, Top = 190, DialogResult = DialogResult.Cancel };


            btnTargetTool.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a tool in your backpack...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        var item = Items.FindBySerial((int)targetSerial);
                        if (item != null)
                        {
                            dialog.Invoke(new Action(() =>
                            {
                                txtToolType.Text = $"0x{item.ItemID:X4}";
                                txtToolColor.Text = item.Hue.ToString();
                                Misc.SendMessage($"Set tool type: 0x{item.ItemID:X4}, Color: {item.Hue}", 88);
                            }));
                        }
                        else
                        {
                            Misc.SendMessage("Target must be a tool item.", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target cancelled.", 33);
                    }
                }));
            };


            foreach (var kvp in RazorEnhanced.Macros.Actions.TargetResourceAction.ResourcePresets)
                cmbResource.Items.Add($"{kvp.Key} ({kvp.Value})");
            cmbResource.Items.Add("Custom...");
            int presetIndex = action.ResourceNumber;
            if (presetIndex >= 0 && presetIndex < RazorEnhanced.Macros.Actions.TargetResourceAction.ResourcePresets.Count)
                cmbResource.SelectedIndex = presetIndex;
            else
                cmbResource.SelectedIndex = cmbResource.Items.Count - 1;

            Label lblResourceNum = new Label { Left = 20, Top = 140, Text = "Resource Number:", Width = 110 };
            TextBox txtResourceNum = new TextBox
            {
                Left = 130,
                Top = 140,
                Width = 120,
                Text = action.ResourceNumber.ToString(),
                Enabled = cmbResource.SelectedIndex == cmbResource.Items.Count - 1
            };

            cmbResource.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbResource.SelectedIndex == cmbResource.Items.Count - 1)
                {
                    txtResourceNum.Enabled = true;
                }
                else
                {
                    txtResourceNum.Enabled = false;
                    txtResourceNum.Text = RazorEnhanced.Macros.Actions.TargetResourceAction.ResourcePresets.Values.ElementAt(cmbResource.SelectedIndex).ToString();
                }
            };


            dialog.Controls.Add(lblToolType);
            dialog.Controls.Add(txtToolType);
            dialog.Controls.Add(btnTargetTool);
            dialog.Controls.Add(lblToolColor);
            dialog.Controls.Add(txtToolColor);
            dialog.Controls.Add(lblResource);
            dialog.Controls.Add(cmbResource);
            dialog.Controls.Add(lblResourceNum);
            dialog.Controls.Add(txtResourceNum);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int toolType = 0x0F39, toolColor = -1, resourceNum = 0;
                string toolTypeStr = txtToolType.Text.Replace("0x", "").Replace("0X", "").Trim();
                int.TryParse(toolTypeStr, System.Globalization.NumberStyles.HexNumber, null, out toolType);
                int.TryParse(txtToolColor.Text, out toolColor);

                if (cmbResource.SelectedIndex == cmbResource.Items.Count - 1)
                {
                    int.TryParse(txtResourceNum.Text, out resourceNum);
                }
                else
                {
                    resourceNum = RazorEnhanced.Macros.Actions.TargetResourceAction.ResourcePresets.Values.ElementAt(cmbResource.SelectedIndex);
                }

                var newAction = new RazorEnhanced.Macros.Actions.TargetResourceAction
                {
                    ToolType = toolType,
                    ToolColor = toolColor,
                    ResourceNumber = resourceNum
                };
                return (true, newAction);
            }
            return (false, action);
        }


        private void InsertSetAbilityMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowSetAbilityDialog("Primary");
            if (result.success)
            {
                var action = new RazorEnhanced.Macros.Actions.SetAbilityAction(result.ability);
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Set Ability action at position {insertIndex + 1}", 88);
            }
        }
        private void EditSetAbilityActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            if (macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.SetAbilityAction setAbilityAction)
            {
                var result = ShowSetAbilityDialog(setAbilityAction.Ability);
                if (result.success)
                {
                    setAbilityAction.Ability = result.ability;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Set Ability action updated", 88);
                }
            }
        }
        private void EditSetAbilityAction(RazorEnhanced.Macros.Actions.SetAbilityAction setAbilityAction, Macro macro, int actionIndex)
        {
            var result = ShowSetAbilityDialog(setAbilityAction.Ability);
            if (result.success)
            {
                setAbilityAction.Ability = result.ability;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionSetAbility()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.SetAbilityAction;
        }
        private (bool success, string ability) ShowSetAbilityDialog(string currentAbility)
        {
            Form dialog = new Form
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Set Ability Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblAbility = new Label { Left = 20, Top = 30, Text = "Ability:", Width = 80 };
            ComboBox cmbAbility = new ComboBox
            {
                Left = 110,
                Top = 28,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbAbility.Items.AddRange(new string[] { "Primary", "Secondary", "Stun", "Disarm", "Clear" });
            cmbAbility.SelectedItem = currentAbility;
            if (cmbAbility.SelectedIndex == -1) cmbAbility.SelectedIndex = 0;

            Button btnOK = new Button { Text = "OK", Left = 110, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 200, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblAbility);
            dialog.Controls.Add(cmbAbility);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string ability = cmbAbility.SelectedItem.ToString();
                return (true, ability);
            }
            return (false, currentAbility);
        }

        private void InsertPromptResponseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowPromptResponseDialog("", 10000);
            if (result.success)
            {
                var action = new PromptResponseAction(result.response, result.timeout);
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Prompt Response at position {insertIndex + 1}", 88);
            }
        }
        private void EditPromptResponseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            if (macro.Actions[actionIndex] is PromptResponseAction promptAction)
            {
                var result = ShowPromptResponseDialog(promptAction.Response, promptAction.Timeout);
                if (result.success)
                {
                    promptAction.Response = result.response;
                    promptAction.Timeout = result.timeout;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Prompt Response action updated", 88);
                }
            }
        }
        private void EditPromptResponseAction(PromptResponseAction promptAction, Macro macro, int actionIndex)
        {
            var result = ShowPromptResponseDialog(promptAction.Response, promptAction.Timeout);
            if (result.success)
            {
                promptAction.Response = result.response;
                promptAction.Timeout = result.timeout;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionPromptResponse()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is PromptResponseAction;
        }
        private (bool success, string response, int timeout) ShowPromptResponseDialog(string response, int timeout)
        {
            Form dialog = new Form
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Prompt Response",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblResponse = new Label { Left = 20, Top = 20, Text = "Response Text:", Width = 100 };
            TextBox txtResponse = new TextBox { Left = 130, Top = 20, Width = 220, Text = response ?? "" };

            Label lblTimeout = new Label { Left = 20, Top = 60, Text = "Timeout (ms):", Width = 100 };
            TextBox txtTimeout = new TextBox { Left = 130, Top = 60, Width = 100, Text = timeout.ToString() };

            Button btnOK = new Button { Text = "OK", Left = 180, Width = 80, Top = 110, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 270, Width = 80, Top = 110, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblResponse);
            dialog.Controls.Add(txtResponse);
            dialog.Controls.Add(lblTimeout);
            dialog.Controls.Add(txtTimeout);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int newTimeout = 10000;
                int.TryParse(txtTimeout.Text, out newTimeout);
                return (true, txtResponse.Text, newTimeout);
            }
            return (false, response, timeout);
        }


        private void InsertWaitForGumpMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowWaitForGumpDialog(-1, 10000);
            if (result.success)
            {
                var action = new WaitForGumpAction((uint)result.gumpId, result.timeout);
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Wait For Gump at position {insertIndex + 1}", 88);
            }
        }
        private void EditWaitForGumpMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1) return;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];
            if (actionIndex < 0 || actionIndex >= macro.Actions.Count) return;

            if (macro.Actions[actionIndex] is WaitForGumpAction waitForGumpAction)
            {
                var result = ShowWaitForGumpDialog((int)waitForGumpAction.GumpID, waitForGumpAction.Timeout);
                if (result.success)
                {
                    waitForGumpAction.GumpID = (uint)result.gumpId;
                    waitForGumpAction.Timeout = result.timeout;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Wait For Gump action updated", 88);
                }
            }
        }
        private void EditWaitForGumpAction(WaitForGumpAction waitForGumpAction, Macro macro, int actionIndex)
        {
            var result = ShowWaitForGumpDialog((int)waitForGumpAction.GumpID, waitForGumpAction.Timeout);
            if (result.success)
            {
                waitForGumpAction.GumpID = (uint)result.gumpId;
                waitForGumpAction.Timeout = result.timeout;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionWaitForGump()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];
            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is WaitForGumpAction;
        }
        private (bool success, int gumpId, int timeout) ShowWaitForGumpDialog(int gumpId, int timeout)
        {
            Form dialog = new Form
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Wait For Gump",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };


            Label lblGumpId = new Label { Left = 20, Top = 20, Text = "Gump ID (0 = any):", Width = 120 };
            TextBox txtGumpId = new TextBox { Left = 150, Top = 20, Width = 100, Text = gumpId.ToString() };

            Label lblTimeout = new Label { Left = 20, Top = 60, Text = "Timeout (ms):", Width = 120 };
            TextBox txtTimeout = new TextBox { Left = 150, Top = 60, Width = 100, Text = timeout.ToString() };

            Button btnOK = new Button { Text = "OK", Left = 180, Width = 80, Top = 110, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 270, Width = 80, Top = 110, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblGumpId);
            dialog.Controls.Add(txtGumpId);
            dialog.Controls.Add(lblTimeout);
            dialog.Controls.Add(txtTimeout);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int newGumpId = -1, newTimeout = 10000;
                int.TryParse(txtGumpId.Text, out newGumpId);
                int.TryParse(txtTimeout.Text, out newTimeout);
                return (true, newGumpId, newTimeout);
            }
            return (false, gumpId, timeout);
        }

        private void InsertGumpResponseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowGumpResponseDialog(0, 0, new List<int>(), new List<int>(), new List<string>());
            if (result.success)
            {
                var action = new GumpResponseAction((uint)result.gumpId, result.buttonId, result.switches, result.textIds, result.textEntries);
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Gump Response at position {insertIndex + 1}", 88);
            }
        }
        private void EditGumpResponseMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1) return;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];
            if (actionIndex < 0 || actionIndex >= macro.Actions.Count) return;

            if (macro.Actions[actionIndex] is GumpResponseAction gumpAction)
            {
                var result = ShowGumpResponseDialog((int)gumpAction.GumpID, gumpAction.ButtonID, gumpAction.Switches, gumpAction.TextIDs, gumpAction.TextEntries);
                if (result.success)
                {
                    gumpAction.GumpID = (uint)result.gumpId;
                    gumpAction.ButtonID = result.buttonId;
                    gumpAction.Switches = result.switches;
                    gumpAction.TextIDs = result.textIds;
                    gumpAction.TextEntries = result.textEntries;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Gump Response action updated", 88);
                }
            }
        }
        private void EditGumpResponseAction(GumpResponseAction gumpAction, Macro macro, int actionIndex)
        {
            var result = ShowGumpResponseDialog((int)gumpAction.GumpID, gumpAction.ButtonID, gumpAction.Switches, gumpAction.TextIDs, gumpAction.TextEntries);
            if (result.success)
            {
                gumpAction.GumpID = (uint)result.gumpId;
                gumpAction.ButtonID = result.buttonId;
                gumpAction.Switches = result.switches;
                gumpAction.TextIDs = result.textIds;
                gumpAction.TextEntries = result.textEntries;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionGumpResponse()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];
            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is GumpResponseAction;
        }
        private (bool success, int gumpId, int buttonId, List<int> switches, List<int> textIds, List<string> textEntries)
    ShowGumpResponseDialog(int gumpId, int buttonId, List<int> switches, List<int> textIds, List<string> textEntries)
        {
            Form dialog = new Form
            {
                Width = 500,
                Height = 350,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Gump Response",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblGumpId = new Label { Left = 20, Top = 20, Text = "Gump ID (-1 = any):", Width = 150 };
            TextBox txtGumpId = new TextBox { Left = 180, Top = 20, Width = 100, Text = gumpId == 0 ? "-1" : gumpId.ToString() };
            //TextBox txtGumpId = new TextBox { Left = 180, Top = 20, Width = 100, Text = gumpId == 0 ? "-1" : $"0x{gumpId:X8}" };

            Label lblButtonId = new Label { Left = 20, Top = 60, Text = "Button ID:", Width = 150 };
            TextBox txtButtonId = new TextBox { Left = 180, Top = 60, Width = 100, Text = buttonId.ToString() };

            Label lblSwitches = new Label { Left = 20, Top = 100, Text = "Switches (comma):", Width = 150 };
            TextBox txtSwitches = new TextBox { Left = 180, Top = 100, Width = 280, Text = string.Join(",", switches ?? new List<int>()) };

            Label lblTextIds = new Label { Left = 20, Top = 140, Text = "Text IDs (comma):", Width = 150 };
            TextBox txtTextIds = new TextBox { Left = 180, Top = 140, Width = 280, Text = string.Join(",", textIds ?? new List<int>()) };

            Label lblTextEntries = new Label { Left = 20, Top = 180, Text = "Text Entries (one per line):", Width = 150 };
            TextBox txtTextEntries = new TextBox { Left = 180, Top = 180, Width = 280, Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical, Text = string.Join(Environment.NewLine, textEntries ?? new List<string>()) };

            Button btnOK = new Button { Text = "OK", Left = 280, Width = 80, Top = 270, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 370, Width = 80, Top = 270, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblGumpId);
            dialog.Controls.Add(txtGumpId);
            dialog.Controls.Add(lblButtonId);
            dialog.Controls.Add(txtButtonId);
            dialog.Controls.Add(lblSwitches);
            dialog.Controls.Add(txtSwitches);
            dialog.Controls.Add(lblTextIds);
            dialog.Controls.Add(txtTextIds);
            dialog.Controls.Add(lblTextEntries);
            dialog.Controls.Add(txtTextEntries);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int newGumpId = ParseGumpId(txtGumpId.Text, gumpId);
                int newButtonId = 0;
                int.TryParse(txtButtonId.Text, out newButtonId);

                var newSwitches = txtSwitches.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s.Trim(), out int v) ? v : 0).ToList();

                var newTextIds = txtTextIds.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s.Trim(), out int v) ? v : 0).ToList();

                var newTextEntries = txtTextEntries.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                return (true, newGumpId, newButtonId, newSwitches, newTextIds, newTextEntries);
            }
            return (false, gumpId, buttonId, switches, textIds, textEntries);
        }

        private int ParseGumpId(string input, int fallback)
        {
            if (string.IsNullOrWhiteSpace(input))
                return fallback;
            input = input.Trim();
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(input.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int hexVal))
                    return hexVal;
            }
            else if (int.TryParse(input, out int decVal))
            {
                return decVal;
            }
            return fallback;
        }
        private void InsertMoveItemActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0) return;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowMoveItemActionDialog(new MoveItemAction());
            if (result.success)
            {
                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, result.action);
                DisplayMacroActions(macro);
                MacroManager.SaveMacros();
                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }
                Misc.SendMessage($"Inserted Move Item at position {insertIndex + 1}", 88);
            }
        }
        private void EditMoveItemActionMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1) return;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];
            if (actionIndex < 0 || actionIndex >= macro.Actions.Count) return;

            if (macro.Actions[actionIndex] is MoveItemAction moveItemAction)
            {
                var result = ShowMoveItemActionDialog(moveItemAction);
                if (result.success)
                {
                    macro.Actions[actionIndex] = result.action;
                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();
                    macroActionsListView.Items[actionIndex].Selected = true;
                    macroActionsListView.EnsureVisible(actionIndex);
                    Misc.SendMessage("Move Item action updated", 88);
                }
            }
        }
        private void EditMoveItemAction(MoveItemAction moveItemAction, Macro macro, int actionIndex)
        {
            var result = ShowMoveItemActionDialog(moveItemAction);
            if (result.success)
            {
                macro.Actions[actionIndex] = result.action;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionMoveItem()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;
            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;
            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];
            return actionIndex >= 0 && actionIndex < macro.Actions.Count &&
                   macro.Actions[actionIndex] is MoveItemAction;
        }
        private (bool success, MoveItemAction action) ShowMoveItemActionDialog(MoveItemAction action)
        {
            Form dialog = new Form
            {
                Width = 520,
                Height = 370,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Move Item",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTargetType = new Label { Left = 20, Top = 20, Text = "Target Type:", Width = 100 };
            ComboBox cmbTargetType = new ComboBox
            {
                Left = 130,
                Top = 20,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTargetType.Items.AddRange(new string[] { "Entity (Container/Mobile)", "Ground" });
            cmbTargetType.SelectedIndex = (int)action.TargetType;

            Label lblItemSerial = new Label { Left = 20, Top = 60, Text = "Item Serial/Alias:", Width = 120 };
            TextBox txtItemSerial = new TextBox
            {
                Left = 150,
                Top = 60,
                Width = 200,
                Text = action.ItemSerialOrAlias ?? ""
            };
            Button btnTargetItem = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 58,
                Width = 80
            };
            Label lblItemHint = new Label
            {
                Left = 150,
                Top = 85,
                Width = 300,
                Height = 20,
                Text = "Serial (0x...) or alias (e.g. 'findfound')",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Label lblAmount = new Label { Left = 20, Top = 110, Text = "Amount (-1 = all):", Width = 120 };
            TextBox txtAmount = new TextBox
            {
                Left = 150,
                Top = 110,
                Width = 120,
                Text = action.Amount.ToString()
            };

            // Entity fields
            Label lblTargetSerial = new Label { Left = 20, Top = 150, Text = "Target Serial/Alias:", Width = 120 };
            TextBox txtTargetSerial = new TextBox
            {
                Left = 150,
                Top = 150,
                Width = 200,
                Text = action.TargetSerialOrAlias ?? ""
            };
            Button btnTargetEntity = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 148,
                Width = 80
            };
            Label lblTargetHint = new Label
            {
                Left = 150,
                Top = 175,
                Width = 300,
                Height = 20,
                Text = "Serial (0x...) or alias (e.g. 'backpack')",
                ForeColor = Color.Gray,
                Font = new Font(Control.DefaultFont, FontStyle.Italic)
            };

            Label lblX = new Label { Left = 20, Top = 200, Text = "X (container/ground):", Width = 120 };
            TextBox txtX = new TextBox
            {
                Left = 150,
                Top = 200,
                Width = 60,
                Text = action.X.ToString()
            };

            Label lblY = new Label { Left = 220, Top = 200, Text = "Y (container/ground):", Width = 120 };
            TextBox txtY = new TextBox
            {
                Left = 340,
                Top = 200,
                Width = 60,
                Text = action.Y.ToString()
            };

            // Ground only
            Label lblZ = new Label { Left = 20, Top = 240, Text = "Z (ground only):", Width = 120 };
            TextBox txtZ = new TextBox
            {
                Left = 150,
                Top = 240,
                Width = 60,
                Text = action.Z.ToString()
            };

            btnTargetItem.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item to move...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtItemSerial.Text = $"0x{(int)serial:X8}";
                        }));
                    }
                }));
            };

            btnTargetEntity.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a container or mobile...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial serial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (serial.IsValid && serial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtTargetSerial.Text = $"0x{(int)serial:X8}";
                        }));
                    }
                }));
            };

            void UpdateVisibility()
            {
                bool isEntity = cmbTargetType.SelectedIndex == 0;
                lblTargetSerial.Visible = txtTargetSerial.Visible = btnTargetEntity.Visible = lblTargetHint.Visible = isEntity;
                lblX.Visible = txtX.Visible = lblY.Visible = txtY.Visible = true;
                lblZ.Visible = txtZ.Visible = !isEntity;
            }
            cmbTargetType.SelectedIndexChanged += (s, ev) => UpdateVisibility();
            UpdateVisibility();

            Button btnOK = new Button { Text = "OK", Left = 200, Width = 80, Top = 290, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 300, Width = 80, Top = 290, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblTargetType);
            dialog.Controls.Add(cmbTargetType);
            dialog.Controls.Add(lblItemSerial);
            dialog.Controls.Add(txtItemSerial);
            dialog.Controls.Add(btnTargetItem);
            dialog.Controls.Add(lblItemHint);
            dialog.Controls.Add(lblAmount);
            dialog.Controls.Add(txtAmount);
            dialog.Controls.Add(lblTargetSerial);
            dialog.Controls.Add(txtTargetSerial);
            dialog.Controls.Add(btnTargetEntity);
            dialog.Controls.Add(lblTargetHint);
            dialog.Controls.Add(lblX);
            dialog.Controls.Add(txtX);
            dialog.Controls.Add(lblY);
            dialog.Controls.Add(txtY);
            dialog.Controls.Add(lblZ);
            dialog.Controls.Add(txtZ);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newAction = new MoveItemAction();
                newAction.TargetType = (MoveItemAction.MoveTargetType)cmbTargetType.SelectedIndex;
                newAction.ItemSerialOrAlias = txtItemSerial.Text.Trim();
                int.TryParse(txtAmount.Text, out int amount);
                newAction.Amount = amount;

                if (newAction.TargetType == MoveItemAction.MoveTargetType.Entity)
                {
                    newAction.TargetSerialOrAlias = txtTargetSerial.Text.Trim();
                    int.TryParse(txtX.Text, out int x);
                    int.TryParse(txtY.Text, out int y);
                    newAction.X = x;
                    newAction.Y = y;
                    newAction.Z = 0;
                }
                else
                {
                    int.TryParse(txtX.Text, out int x);
                    int.TryParse(txtY.Text, out int y);
                    int.TryParse(txtZ.Text, out int z);
                    newAction.X = x;
                    newAction.Y = y;
                    newAction.Z = z;
                    newAction.TargetSerialOrAlias = "";
                }
                return (true, newAction);
            }
            return (false, action);
        }


        private void InsertDropMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];

            var result = ShowDropDialog(0, 0, 0, 0);

            if (result.success)
            {
                var dropAction = new RazorEnhanced.Macros.Actions.DropAction(result.serial, result.container, result.x, result.y, result.z);

                int insertIndex = GetInsertPosition();
                macro.Actions.Insert(insertIndex, dropAction);

                DisplayMacroActions(macro);
                MacroManager.SaveMacros();

                if (insertIndex < macroActionsListView.Items.Count)
                {
                    macroActionsListView.Items[insertIndex].Selected = true;
                    macroActionsListView.EnsureVisible(insertIndex);
                }

                Misc.SendMessage($"Inserted Drop action at position {insertIndex + 1}", 88);
            }
        }
        private void EditDropMenuItem_Click(object sender, EventArgs e)
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex < 0 || actionIndex >= macro.Actions.Count)
                return;

            var action = macro.Actions[actionIndex];

            if (action is RazorEnhanced.Macros.Actions.DropAction dropAction)
            {
                var result = ShowDropDialog(dropAction.Serial, dropAction.Container, dropAction.X, dropAction.Y);

                if (result.success)
                {
                    dropAction.Serial = result.serial;
                    dropAction.Container = result.container;
                    dropAction.X = result.x;
                    dropAction.Y = result.y;

                    DisplayMacroActions(macro);
                    MacroManager.SaveMacros();

                    if (actionIndex < macroActionsListView.Items.Count)
                    {
                        macroActionsListView.Items[actionIndex].Selected = true;
                        macroActionsListView.EnsureVisible(actionIndex);
                    }

                    Misc.SendMessage("Drop action updated", 88);
                }
            }
        }
        private void EditDropAction(RazorEnhanced.Macros.Actions.DropAction dropAction, Macro macro, int actionIndex)
        {
            var result = ShowDropDialog(dropAction.Serial, dropAction.Container, dropAction.X, dropAction.Y);

            if (result.success)
            {
                dropAction.Serial = result.serial;
                dropAction.Container = result.container;
                dropAction.X = result.x;
                dropAction.Y = result.y;
                RefreshAndSelectAction(macro, actionIndex);
            }
        }
        private bool IsSelectedActionDrop()
        {
            if (macroListBox.SelectedIndex < 0 || macroActionsListView.SelectedIndices.Count != 1)
                return false;

            var macros = MacroManager.GetMacros();
            if (macroListBox.SelectedIndex >= macros.Count) return false;

            var macro = macros[macroListBox.SelectedIndex];
            int actionIndex = macroActionsListView.SelectedIndices[0];

            if (actionIndex >= 0 && actionIndex < macro.Actions.Count)
            {
                return macro.Actions[actionIndex] is RazorEnhanced.Macros.Actions.DropAction;
            }

            return false;
        }
        private (bool success, int serial, int container, int x, int y, int z) ShowDropDialog(int serial, int container, int x, int y, int z = 0)
        {
            Form dialog = new Form
            {
                Width = 420,
                Height = 300,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Drop Action",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblSerial = new Label { Left = 20, Top = 20, Text = "Item Serial (hex):", Width = 120 };
            TextBox txtSerial = new TextBox
            {
                Left = 150,
                Top = 20,
                Width = 200,
                Text = serial == 0 ? "" : $"0x{serial:X8}"
            };
            Button btnTargetItem = new Button
            {
                Text = "Target",
                Left = 360,
                Top = 18,
                Width = 40
            };
            btnTargetItem.Click += (s, ev) =>
            {
                Misc.SendMessage("Target an item to drop...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    if (targetSerial.IsValid && targetSerial != 0)
                    {
                        dialog.Invoke(new Action(() =>
                        {
                            txtSerial.Text = $"0x{(int)targetSerial:X8}";
                        }));
                    }
                }));
            };

            Label lblContainer = new Label { Left = 20, Top = 60, Text = "Container Serial (hex, 0xFFFFFFFF = ground):", Width = 250 };
            TextBox txtContainer = new TextBox
            {
                Left = 270,
                Top = 60,
                Width = 120,
                Text = container == unchecked((int)0xFFFFFFFF) ? "0xFFFFFFFF" : (container == 0 ? "" : $"0x{container:X8}")
            };
            Button btnTargetContainer = new Button
            {
                Text = "Target",
                Left = 400,
                Top = 58,
                Width = 40
            };
            btnTargetContainer.Click += (s, ev) =>
            {
                Misc.SendMessage("Target a container or ground...", 88);
                Assistant.Targeting.OneTimeTarget(false, new Assistant.Targeting.TargetResponseCallback((bool loc, Assistant.Serial targetSerial, Assistant.Point3D p, ushort gfx) =>
                {
                    dialog.Invoke(new Action(() =>
                    {
                        if (targetSerial.IsValid && targetSerial != 0)
                            txtContainer.Text = $"0x{(int)targetSerial:X8}";
                        else
                            txtContainer.Text = "0xFFFFFFFF"; // ground
                    }));
                }));
            };

            Label lblX = new Label { Left = 20, Top = 100, Text = "X:", Width = 30 };
            TextBox txtX = new TextBox { Left = 60, Top = 100, Width = 60, Text = x.ToString() };

            Label lblY = new Label { Left = 140, Top = 100, Text = "Y:", Width = 30 };
            TextBox txtY = new TextBox { Left = 180, Top = 100, Width = 60, Text = y.ToString() };

            Label lblZ = new Label { Left = 260, Top = 100, Text = "Z:", Width = 30 };
            TextBox txtZ = new TextBox { Left = 300, Top = 100, Width = 60, Text = z.ToString() };

            Button btnOK = new Button { Text = "OK", Left = 200, Width = 80, Top = 180, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 300, Width = 80, Top = 180, DialogResult = DialogResult.Cancel };

            dialog.Controls.Add(lblSerial);
            dialog.Controls.Add(txtSerial);
            dialog.Controls.Add(btnTargetItem);
            dialog.Controls.Add(lblContainer);
            dialog.Controls.Add(txtContainer);
            dialog.Controls.Add(btnTargetContainer);
            dialog.Controls.Add(lblX);
            dialog.Controls.Add(txtX);
            dialog.Controls.Add(lblY);
            dialog.Controls.Add(txtY);
            dialog.Controls.Add(lblZ);
            dialog.Controls.Add(txtZ);
            dialog.Controls.Add(btnOK);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOK;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int newSerial = 0, newContainer = 0, newX = 0, newY = 0, newZ = 0;
                string serialStr = txtSerial.Text.Replace("0x", "").Replace("0X", "").Trim();
                if (!string.IsNullOrEmpty(serialStr))
                    int.TryParse(serialStr, System.Globalization.NumberStyles.HexNumber, null, out newSerial);

                string containerStr = txtContainer.Text.Replace("0x", "").Replace("0X", "").Trim();
                if (containerStr.ToUpper() == "FFFFFFFF")
                    newContainer = unchecked((int)0xFFFFFFFF);
                else if (!string.IsNullOrEmpty(containerStr))
                    int.TryParse(containerStr, System.Globalization.NumberStyles.HexNumber, null, out newContainer);

                int.TryParse(txtX.Text, out newX);
                int.TryParse(txtY.Text, out newY);
                int.TryParse(txtZ.Text, out newZ);

                return (true, newSerial, newContainer, newX, newY, newZ);
            }
            return (false, serial, container, x, y, z);
        }



    }
}