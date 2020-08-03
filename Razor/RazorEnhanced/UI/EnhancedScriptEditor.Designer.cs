namespace RazorEnhanced.UI
{
	partial class EnhancedScriptEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedScriptEditor));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonDebug = new System.Windows.Forms.ToolStripButton();
			this.toolStripNextCall = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonNextLine = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonNextReturn = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonNextBreakpoint = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonAddBreakpoint = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRemoveBreakpoints = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonGumps = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonInspect = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.inspectaliasbutton = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelScript = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.fastColoredTextBoxEditor = new FastColoredTextBoxNS.FastColoredTextBox();
			this.textareaMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.commentSelectLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unCommentLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.messagelistBox = new System.Windows.Forms.ListBox();
			this.logboxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.textBoxDebug = new System.Windows.Forms.TextBox();
			this.imageList2 = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxEditor)).BeginInit();
			this.textareaMenuStrip.SuspendLayout();
			this.logboxMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripButtonClose,
            this.toolStripButtonSearch,
            this.toolStripSeparator1,
            this.toolStripButtonPlay,
            this.toolStripButtonStop,
            this.toolStripSeparator4,
            this.toolStripButtonDebug,
            this.toolStripNextCall,
            this.toolStripButtonNextLine,
            this.toolStripButtonNextReturn,
            this.toolStripButtonNextBreakpoint,
            this.toolStripButtonAddBreakpoint,
            this.toolStripButtonRemoveBreakpoints,
            this.toolStripSeparator3,
            this.toolStripButtonGumps,
            this.toolStripSeparator2,
            this.toolStripButtonInspect,
            this.toolStripButton2,
            this.inspectaliasbutton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1404, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButtonOpen
			// 
			this.toolStripButtonOpen.Image = global::Assistant.Properties.Resources.document_open_7;
			this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonOpen.Name = "toolStripButtonOpen";
			this.toolStripButtonOpen.Size = new System.Drawing.Size(56, 22);
			this.toolStripButtonOpen.Text = "Open";
			this.toolStripButtonOpen.ToolTipText = "Open ( CTRL + O )";
			this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
			// 
			// toolStripButtonSave
			// 
			this.toolStripButtonSave.Image = global::Assistant.Properties.Resources.document_save_5;
			this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSave.Name = "toolStripButtonSave";
			this.toolStripButtonSave.Size = new System.Drawing.Size(51, 22);
			this.toolStripButtonSave.Text = "Save";
			this.toolStripButtonSave.ToolTipText = "Save ( CTRL + S )";
			this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
			// 
			// toolStripButtonSaveAs
			// 
			this.toolStripButtonSaveAs.Image = global::Assistant.Properties.Resources.document_save_5;
			this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
			this.toolStripButtonSaveAs.Size = new System.Drawing.Size(67, 22);
			this.toolStripButtonSaveAs.Text = "Save As";
			this.toolStripButtonSaveAs.ToolTipText = "Save As ( CTRL + SHIFT + S )";
			this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
			// 
			// toolStripButtonClose
			// 
			this.toolStripButtonClose.Image = global::Assistant.Properties.Resources.document_close_2;
			this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonClose.Name = "toolStripButtonClose";
			this.toolStripButtonClose.Size = new System.Drawing.Size(51, 22);
			this.toolStripButtonClose.Text = "New";
			this.toolStripButtonClose.ToolTipText = "Close ( CTRL + E )";
			this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
			// 
			// toolStripButtonSearch
			// 
			this.toolStripButtonSearch.Image = global::Assistant.Properties.Resources.search2;
			this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSearch.Name = "toolStripButtonSearch";
			this.toolStripButtonSearch.Size = new System.Drawing.Size(62, 22);
			this.toolStripButtonSearch.Text = "Search";
			this.toolStripButtonSearch.ToolTipText = "Search ( CTRL + F )";
			this.toolStripButtonSearch.Click += new System.EventHandler(this.toolStripButtonSearch_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonPlay
			// 
			this.toolStripButtonPlay.Image = global::Assistant.Properties.Resources.media_playback_start_3;
			this.toolStripButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlay.Name = "toolStripButtonPlay";
			this.toolStripButtonPlay.Size = new System.Drawing.Size(49, 22);
			this.toolStripButtonPlay.Text = "Play";
			this.toolStripButtonPlay.ToolTipText = "Play ( F6 )";
			this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButtonPlay_Click);
			// 
			// toolStripButtonStop
			// 
			this.toolStripButtonStop.Image = global::Assistant.Properties.Resources.media_playback_stop_3;
			this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonStop.Name = "toolStripButtonStop";
			this.toolStripButtonStop.Size = new System.Drawing.Size(51, 22);
			this.toolStripButtonStop.Text = "Stop";
			this.toolStripButtonStop.ToolTipText = "Stop ( F4 )";
			this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonDebug
			// 
			this.toolStripButtonDebug.Image = global::Assistant.Properties.Resources.bug;
			this.toolStripButtonDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonDebug.Name = "toolStripButtonDebug";
			this.toolStripButtonDebug.Size = new System.Drawing.Size(99, 22);
			this.toolStripButtonDebug.Text = "Debug Mode!";
			this.toolStripButtonDebug.ToolTipText = "Debug ( F5 )";
			this.toolStripButtonDebug.Click += new System.EventHandler(this.toolStripButtonDebug_Click);
			// 
			// toolStripNextCall
			// 
			this.toolStripNextCall.Image = global::Assistant.Properties.Resources.media_seek_forward_3;
			this.toolStripNextCall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripNextCall.Name = "toolStripNextCall";
			this.toolStripNextCall.Size = new System.Drawing.Size(74, 22);
			this.toolStripNextCall.Text = "Next Call";
			this.toolStripNextCall.ToolTipText = "Next Call ( F12 )";
			this.toolStripNextCall.Click += new System.EventHandler(this.toolStripNextCall_Click);
			// 
			// toolStripButtonNextLine
			// 
			this.toolStripButtonNextLine.Image = global::Assistant.Properties.Resources.media_playback_pause_3;
			this.toolStripButtonNextLine.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNextLine.Name = "toolStripButtonNextLine";
			this.toolStripButtonNextLine.Size = new System.Drawing.Size(76, 22);
			this.toolStripButtonNextLine.Text = "Next Line";
			this.toolStripButtonNextLine.ToolTipText = "Next Line ( F10 )";
			this.toolStripButtonNextLine.Click += new System.EventHandler(this.toolStripButtonNextLine_Click);
			// 
			// toolStripButtonNextReturn
			// 
			this.toolStripButtonNextReturn.Image = global::Assistant.Properties.Resources.media_skip_forward_3;
			this.toolStripButtonNextReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNextReturn.Name = "toolStripButtonNextReturn";
			this.toolStripButtonNextReturn.Size = new System.Drawing.Size(89, 22);
			this.toolStripButtonNextReturn.Text = "Next Return";
			this.toolStripButtonNextReturn.ToolTipText = "Next Return ( F11 )";
			this.toolStripButtonNextReturn.Click += new System.EventHandler(this.toolStripButtonNextReturn_Click);
			// 
			// toolStripButtonNextBreakpoint
			// 
			this.toolStripButtonNextBreakpoint.Image = global::Assistant.Properties.Resources.bug_go;
			this.toolStripButtonNextBreakpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNextBreakpoint.Name = "toolStripButtonNextBreakpoint";
			this.toolStripButtonNextBreakpoint.Size = new System.Drawing.Size(111, 22);
			this.toolStripButtonNextBreakpoint.Text = "Next Breakpoint";
			this.toolStripButtonNextBreakpoint.ToolTipText = "Next Breakpoint ( F9 )";
			this.toolStripButtonNextBreakpoint.Click += new System.EventHandler(this.toolStripButtonNextBreakpoint_Click);
			// 
			// toolStripButtonAddBreakpoint
			// 
			this.toolStripButtonAddBreakpoint.Image = global::Assistant.Properties.Resources.bug_add;
			this.toolStripButtonAddBreakpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAddBreakpoint.Name = "toolStripButtonAddBreakpoint";
			this.toolStripButtonAddBreakpoint.Size = new System.Drawing.Size(49, 22);
			this.toolStripButtonAddBreakpoint.Text = "Add";
			this.toolStripButtonAddBreakpoint.ToolTipText = "Breakpoint ( F7 )";
			this.toolStripButtonAddBreakpoint.Click += new System.EventHandler(this.toolStripButtonAddBreakpoint_Click);
			// 
			// toolStripButtonRemoveBreakpoints
			// 
			this.toolStripButtonRemoveBreakpoints.Image = global::Assistant.Properties.Resources.bug_delete;
			this.toolStripButtonRemoveBreakpoints.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRemoveBreakpoints.Name = "toolStripButtonRemoveBreakpoints";
			this.toolStripButtonRemoveBreakpoints.Size = new System.Drawing.Size(70, 22);
			this.toolStripButtonRemoveBreakpoints.Text = "Remove";
			this.toolStripButtonRemoveBreakpoints.ToolTipText = "Remove ( F8 )";
			this.toolStripButtonRemoveBreakpoints.Click += new System.EventHandler(this.toolStripButtonRemoveBreakpoints_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonGumps
			// 
			this.toolStripButtonGumps.Image = global::Assistant.Properties.Resources.record;
			this.toolStripButtonGumps.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonGumps.Name = "toolStripButtonGumps";
			this.toolStripButtonGumps.Size = new System.Drawing.Size(64, 22);
			this.toolStripButtonGumps.Text = "Record";
			this.toolStripButtonGumps.ToolTipText = "Record ( CTRL + R )";
			this.toolStripButtonGumps.Click += new System.EventHandler(this.toolStripRecord_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonInspect
			// 
			this.toolStripButtonInspect.Image = global::Assistant.Properties.Resources.applications_utilities;
			this.toolStripButtonInspect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonInspect.Name = "toolStripButtonInspect";
			this.toolStripButtonInspect.Size = new System.Drawing.Size(106, 22);
			this.toolStripButtonInspect.Text = "Inspect Entities";
			this.toolStripButtonInspect.ToolTipText = "Inspect Entities ( CTRL + I )";
			this.toolStripButtonInspect.Click += new System.EventHandler(this.toolStripButtonInspect_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.Image = global::Assistant.Properties.Resources.gump;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(106, 22);
			this.toolStripButton2.Text = "Inspect Gumps";
			this.toolStripButton2.ToolTipText = "Inspect Gumps ( CTRL + G )";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripInspectGump_Click);
			// 
			// inspectaliasbutton
			// 
			this.inspectaliasbutton.Image = global::Assistant.Properties.Resources.search2;
			this.inspectaliasbutton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.inspectaliasbutton.Name = "inspectaliasbutton";
			this.inspectaliasbutton.Size = new System.Drawing.Size(93, 22);
			this.inspectaliasbutton.Text = "Inspect Alias";
			this.inspectaliasbutton.ToolTipText = "Inspect Alias";
			this.inspectaliasbutton.Click += new System.EventHandler(this.toolStripInspectAlias_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelScript,
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 610);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1404, 22);
			this.statusStrip1.TabIndex = 2;
			// 
			// toolStripStatusLabelScript
			// 
			this.toolStripStatusLabelScript.Name = "toolStripStatusLabelScript";
			this.toolStripStatusLabelScript.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.fastColoredTextBoxEditor);
			this.splitContainer1.Panel1.Controls.Add(this.messagelistBox);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(1404, 585);
			this.splitContainer1.SplitterDistance = 1043;
			this.splitContainer1.TabIndex = 3;
			// 
			// fastColoredTextBoxEditor
			// 
			this.fastColoredTextBoxEditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fastColoredTextBoxEditor.AutoScrollMinSize = new System.Drawing.Size(27, 14);
			this.fastColoredTextBoxEditor.BackBrush = null;
			this.fastColoredTextBoxEditor.CharHeight = 14;
			this.fastColoredTextBoxEditor.CharWidth = 8;
			this.fastColoredTextBoxEditor.CommentPrefix = "#";
			this.fastColoredTextBoxEditor.ContextMenuStrip = this.textareaMenuStrip;
			this.fastColoredTextBoxEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fastColoredTextBoxEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fastColoredTextBoxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fastColoredTextBoxEditor.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.fastColoredTextBoxEditor.IsReplaceMode = false;
			this.fastColoredTextBoxEditor.Language = FastColoredTextBoxNS.Language.Python;
			this.fastColoredTextBoxEditor.LeftBracket = '(';
			this.fastColoredTextBoxEditor.LeftBracket2 = '[';
			this.fastColoredTextBoxEditor.Location = new System.Drawing.Point(0, 0);
			this.fastColoredTextBoxEditor.Name = "fastColoredTextBoxEditor";
			this.fastColoredTextBoxEditor.Paddings = new System.Windows.Forms.Padding(0);
			this.fastColoredTextBoxEditor.RightBracket = ')';
			this.fastColoredTextBoxEditor.RightBracket2 = ']';
			this.fastColoredTextBoxEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fastColoredTextBoxEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxEditor.ServiceColors")));
			this.fastColoredTextBoxEditor.Size = new System.Drawing.Size(1043, 464);
			this.fastColoredTextBoxEditor.TabIndex = 0;
			this.fastColoredTextBoxEditor.Zoom = 100;
			// 
			// textareaMenuStrip
			// 
			this.textareaMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.toolStripMenuItem1,
            this.commentSelectLineToolStripMenuItem,
            this.unCommentLineToolStripMenuItem});
			this.textareaMenuStrip.Name = "textareaMenuStrip";
			this.textareaMenuStrip.Size = new System.Drawing.Size(223, 120);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(219, 6);
			// 
			// commentSelectLineToolStripMenuItem
			// 
			this.commentSelectLineToolStripMenuItem.Name = "commentSelectLineToolStripMenuItem";
			this.commentSelectLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.commentSelectLineToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.commentSelectLineToolStripMenuItem.Text = "Comment Line";
			this.commentSelectLineToolStripMenuItem.Click += new System.EventHandler(this.commentSelectLineToolStripMenuItem_Click);
			// 
			// unCommentLineToolStripMenuItem
			// 
			this.unCommentLineToolStripMenuItem.Name = "unCommentLineToolStripMenuItem";
			this.unCommentLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.unCommentLineToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.unCommentLineToolStripMenuItem.Text = "UnComment Line";
			this.unCommentLineToolStripMenuItem.Click += new System.EventHandler(this.unCommentLineToolStripMenuItem_Click);
			// 
			// messagelistBox
			// 
			this.messagelistBox.ContextMenuStrip = this.logboxMenuStrip;
			this.messagelistBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.messagelistBox.FormattingEnabled = true;
			this.messagelistBox.HorizontalScrollbar = true;
			this.messagelistBox.Location = new System.Drawing.Point(0, 464);
			this.messagelistBox.Name = "messagelistBox";
			this.messagelistBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.messagelistBox.Size = new System.Drawing.Size(1043, 121);
			this.messagelistBox.TabIndex = 1;
			this.messagelistBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.messagelistBox_KeyUp);
			// 
			// logboxMenuStrip
			// 
			this.logboxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.copyToolStripMenuItem1});
			this.logboxMenuStrip.Name = "logboxMenuStrip";
			this.logboxMenuStrip.Size = new System.Drawing.Size(154, 48);
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem1
			// 
			this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
			this.copyToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
			this.copyToolStripMenuItem1.Text = "Copy";
			this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.textBoxDebug);
			this.splitContainer2.Panel1MinSize = 0;
			this.splitContainer2.Panel2MinSize = 0;
			this.splitContainer2.Size = new System.Drawing.Size(357, 585);
			this.splitContainer2.SplitterDistance = 556;
			this.splitContainer2.TabIndex = 0;
			// 
			// textBoxDebug
			// 
			this.textBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxDebug.Location = new System.Drawing.Point(0, 0);
			this.textBoxDebug.Multiline = true;
			this.textBoxDebug.Name = "textBoxDebug";
			this.textBoxDebug.ReadOnly = true;
			this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxDebug.Size = new System.Drawing.Size(357, 556);
			this.textBoxDebug.TabIndex = 0;
			// 
			// imageList2
			// 
			this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
			this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList2.Images.SetKeyName(0, "Key_16x.png");
			this.imageList2.Images.SetKeyName(1, "Class_yellow_16x.png");
			this.imageList2.Images.SetKeyName(2, "Method_purple_16x.png");
			this.imageList2.Images.SetKeyName(3, "Method_black_16x.png");
			this.imageList2.Images.SetKeyName(4, "Field_blue_16x.png");
			this.imageList2.Images.SetKeyName(5, "Field_black_16x.png");
			// 
			// EnhancedScriptEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1404, 632);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "EnhancedScriptEditor";
			this.Text = "Enhanced Script Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnhancedScriptEditor_FormClosing);
			this.Load += new System.EventHandler(this.EnhancedScriptEditor_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxEditor)).EndInit();
			this.textareaMenuStrip.ResumeLayout(false);
			this.logboxMenuStrip.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
		private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
		private System.Windows.Forms.ToolStripButton toolStripButtonClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButtonStop;
		private System.Windows.Forms.ToolStripButton toolStripButtonAddBreakpoint;
		private System.Windows.Forms.ToolStripButton toolStripButtonRemoveBreakpoints;
		private System.Windows.Forms.ToolStripButton toolStripNextCall;
		private System.Windows.Forms.ToolStripButton toolStripButtonNextReturn;
		private System.Windows.Forms.ToolStripButton toolStripButtonNextLine;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelScript;
		private System.Windows.Forms.ToolStripButton toolStripButtonPlay;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButtonInspect;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox textBoxDebug;
		private System.Windows.Forms.ToolStripButton toolStripButtonDebug;
		private System.Windows.Forms.ToolStripButton toolStripButtonGumps;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxEditor;
		private System.Windows.Forms.ToolStripButton toolStripButtonSave;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ListBox messagelistBox;
        private System.Windows.Forms.ImageList imageList2;
		private System.Windows.Forms.ToolStripButton inspectaliasbutton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton toolStripButtonNextBreakpoint;
		private System.Windows.Forms.ContextMenuStrip textareaMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem commentSelectLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unCommentLineToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip logboxMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
		private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
	}
}