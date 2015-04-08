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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedScriptEditor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDebug = new System.Windows.Forms.ToolStripButton();
            this.toolStripNextCall = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNextLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNextReturn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAddBreakpoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveBreakpoints = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonInspect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelScript = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scintillaEditor = new ScintillaNET.Scintilla();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintillaEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripButtonClose,
            this.toolStripSeparator1,
            this.toolStripButtonPlay,
            this.toolStripButtonDebug,
            this.toolStripNextCall,
            this.toolStripButtonNextLine,
            this.toolStripButtonNextReturn,
            this.toolStripButtonStop,
            this.toolStripSeparator2,
            this.toolStripButtonAddBreakpoint,
            this.toolStripButtonRemoveBreakpoints,
            this.toolStripSeparator3,
            this.toolStripButtonInspect,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(978, 25);
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
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::Assistant.Properties.Resources.document_save_5;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.Image = global::Assistant.Properties.Resources.document_close_2;
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(56, 22);
            this.toolStripButtonClose.Text = "Close";
            this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
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
            this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButtonPlay_Click);
            // 
            // toolStripButtonDebug
            // 
            this.toolStripButtonDebug.Image = global::Assistant.Properties.Resources.bug;
            this.toolStripButtonDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDebug.Name = "toolStripButtonDebug";
            this.toolStripButtonDebug.Size = new System.Drawing.Size(62, 22);
            this.toolStripButtonDebug.Text = "Debug";
            this.toolStripButtonDebug.Click += new System.EventHandler(this.toolStripButtonDebug_Click);
            // 
            // toolStripNextCall
            // 
            this.toolStripNextCall.Image = global::Assistant.Properties.Resources.media_seek_forward_3;
            this.toolStripNextCall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripNextCall.Name = "toolStripNextCall";
            this.toolStripNextCall.Size = new System.Drawing.Size(74, 22);
            this.toolStripNextCall.Text = "Next Call";
            this.toolStripNextCall.Click += new System.EventHandler(this.toolStripNextCall_Click);
            // 
            // toolStripButtonNextLine
            // 
            this.toolStripButtonNextLine.Image = global::Assistant.Properties.Resources.media_playback_pause_3;
            this.toolStripButtonNextLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNextLine.Name = "toolStripButtonNextLine";
            this.toolStripButtonNextLine.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonNextLine.Text = "Next Line";
            this.toolStripButtonNextLine.Click += new System.EventHandler(this.toolStripButtonNextLine_Click);
            // 
            // toolStripButtonNextReturn
            // 
            this.toolStripButtonNextReturn.Image = global::Assistant.Properties.Resources.media_skip_forward_3;
            this.toolStripButtonNextReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNextReturn.Name = "toolStripButtonNextReturn";
            this.toolStripButtonNextReturn.Size = new System.Drawing.Size(89, 22);
            this.toolStripButtonNextReturn.Text = "Next Return";
            this.toolStripButtonNextReturn.Click += new System.EventHandler(this.toolStripButtonNextReturn_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Image = global::Assistant.Properties.Resources.media_playback_stop_3;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAddBreakpoint
            // 
            this.toolStripButtonAddBreakpoint.Image = global::Assistant.Properties.Resources.bug_add;
            this.toolStripButtonAddBreakpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddBreakpoint.Name = "toolStripButtonAddBreakpoint";
            this.toolStripButtonAddBreakpoint.Size = new System.Drawing.Size(84, 22);
            this.toolStripButtonAddBreakpoint.Text = "Breakpoint";
            this.toolStripButtonAddBreakpoint.Click += new System.EventHandler(this.toolStripButtonAddBreakpoint_Click);
            // 
            // toolStripButtonRemoveBreakpoints
            // 
            this.toolStripButtonRemoveBreakpoints.Image = global::Assistant.Properties.Resources.bug_delete;
            this.toolStripButtonRemoveBreakpoints.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveBreakpoints.Name = "toolStripButtonRemoveBreakpoints";
            this.toolStripButtonRemoveBreakpoints.Size = new System.Drawing.Size(70, 22);
            this.toolStripButtonRemoveBreakpoints.Text = "Remove";
            this.toolStripButtonRemoveBreakpoints.Click += new System.EventHandler(this.toolStripButtonRemoveBreakpoints_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonInspect
            // 
            this.toolStripButtonInspect.Image = global::Assistant.Properties.Resources.applications_utilities;
            this.toolStripButtonInspect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInspect.Name = "toolStripButtonInspect";
            this.toolStripButtonInspect.Size = new System.Drawing.Size(65, 22);
            this.toolStripButtonInspect.Text = "Inspect";
            this.toolStripButtonInspect.Click += new System.EventHandler(this.toolStripButtonInspect_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "GumpInspector";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelScript});
            this.statusStrip1.Location = new System.Drawing.Point(0, 443);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(978, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelScript
            // 
            this.toolStripStatusLabelScript.Name = "toolStripStatusLabelScript";
            this.toolStripStatusLabelScript.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scintillaEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxDebug);
            this.splitContainer1.Size = new System.Drawing.Size(978, 418);
            this.splitContainer1.SplitterDistance = 728;
            this.splitContainer1.TabIndex = 3;
            // 
            // scintillaEditor
            // 
            this.scintillaEditor.ConfigurationManager.Language = "python";
            this.scintillaEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaEditor.Location = new System.Drawing.Point(0, 0);
            this.scintillaEditor.Name = "scintillaEditor";
            this.scintillaEditor.Size = new System.Drawing.Size(728, 418);
            this.scintillaEditor.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintillaEditor.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.scintillaEditor.TabIndex = 0;
            this.scintillaEditor.TextChanged += new System.EventHandler(this.scintillaEditor_TextChanged);
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDebug.Location = new System.Drawing.Point(0, 0);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.ReadOnly = true;
            this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebug.Size = new System.Drawing.Size(246, 418);
            this.textBoxDebug.TabIndex = 0;
            // 
            // EnhancedScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 465);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scintillaEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
		private System.Windows.Forms.ToolStripButton toolStripButtonSave;
		private System.Windows.Forms.ToolStripButton toolStripButtonClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButtonStop;
		private System.Windows.Forms.ToolStripButton toolStripButtonAddBreakpoint;
		private System.Windows.Forms.ToolStripButton toolStripButtonRemoveBreakpoints;
		private System.Windows.Forms.ToolStripButton toolStripNextCall;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButtonNextReturn;
		private System.Windows.Forms.ToolStripButton toolStripButtonNextLine;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelScript;
		private System.Windows.Forms.ToolStripButton toolStripButtonPlay;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButtonInspect;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private ScintillaNET.Scintilla scintillaEditor;
		private System.Windows.Forms.TextBox textBoxDebug;
		private System.Windows.Forms.ToolStripButton toolStripButtonDebug;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
	}
}