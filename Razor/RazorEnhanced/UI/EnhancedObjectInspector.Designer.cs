namespace RazorEnhanced.UI
{
	partial class EnhancedObjectInspector
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.objecttabPage = new System.Windows.Forms.TabPage();
            this.sharedobjectGridView = new System.Windows.Forms.DataGridView();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerstabPage = new System.Windows.Forms.TabPage();
            this.timerGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refreshtimer = new System.Windows.Forms.Timer(this.components);
            this.close = new RazorEnhanced.UI.RazorButton();
            this.tabControl1.SuspendLayout();
            this.objecttabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sharedobjectGridView)).BeginInit();
            this.timerstabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerGridView)).BeginInit();
            this.SuspendLayout();
            //
            // tabControl1
            //
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.objecttabPage);
            this.tabControl1.Controls.Add(this.timerstabPage);
            this.tabControl1.Location = new System.Drawing.Point(18, 18);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(562, 511);
            this.tabControl1.TabIndex = 0;
            //
            // objecttabPage
            //
            this.objecttabPage.Controls.Add(this.sharedobjectGridView);
            this.objecttabPage.Location = new System.Drawing.Point(4, 29);
            this.objecttabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.objecttabPage.Name = "objecttabPage";
            this.objecttabPage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.objecttabPage.Size = new System.Drawing.Size(554, 478);
            this.objecttabPage.TabIndex = 0;
            this.objecttabPage.Text = "Shared Objects";
            this.objecttabPage.UseVisualStyleBackColor = true;
            //
            // sharedobjectGridView
            //
            this.sharedobjectGridView.AllowUserToAddRows = false;
            this.sharedobjectGridView.AllowUserToDeleteRows = false;
            this.sharedobjectGridView.AllowUserToOrderColumns = true;
            this.sharedobjectGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sharedobjectGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.sharedobjectGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sharedobjectGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Alias,
            this.Value});
            this.sharedobjectGridView.Location = new System.Drawing.Point(9, 9);
            this.sharedobjectGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sharedobjectGridView.Name = "sharedobjectGridView";
            this.sharedobjectGridView.ReadOnly = true;
            this.sharedobjectGridView.RowHeadersVisible = false;
            this.sharedobjectGridView.RowHeadersWidth = 62;
            this.sharedobjectGridView.Size = new System.Drawing.Size(532, 449);
            this.sharedobjectGridView.TabIndex = 0;
            //
            // Alias
            //
            this.Alias.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Alias.HeaderText = "Alias";
            this.Alias.MinimumWidth = 8;
            this.Alias.Name = "Alias";
            this.Alias.ReadOnly = true;
            this.Alias.Width = 79;
            //
            // Value
            //
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.MinimumWidth = 8;
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            //
            // timerstabPage
            //
            this.timerstabPage.Controls.Add(this.timerGridView);
            this.timerstabPage.Location = new System.Drawing.Point(4, 29);
            this.timerstabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timerstabPage.Name = "timerstabPage";
            this.timerstabPage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timerstabPage.Size = new System.Drawing.Size(554, 478);
            this.timerstabPage.TabIndex = 1;
            this.timerstabPage.Text = "Timers";
            this.timerstabPage.UseVisualStyleBackColor = true;
            //
            // timerGridView
            //
            this.timerGridView.AllowUserToAddRows = false;
            this.timerGridView.AllowUserToDeleteRows = false;
            this.timerGridView.AllowUserToOrderColumns = true;
            this.timerGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.timerGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.timerGridView.Location = new System.Drawing.Point(10, 11);
            this.timerGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timerGridView.Name = "timerGridView";
            this.timerGridView.ReadOnly = true;
            this.timerGridView.RowHeadersVisible = false;
            this.timerGridView.RowHeadersWidth = 62;
            this.timerGridView.Size = new System.Drawing.Size(536, 449);
            this.timerGridView.TabIndex = 1;
            //
            // dataGridViewTextBoxColumn1
            //
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Alias";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 79;
            //
            // dataGridViewTextBoxColumn2
            //
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Duration";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            //
            // refreshtimer
            //
            this.refreshtimer.Enabled = true;
            this.refreshtimer.Interval = 1000;
            this.refreshtimer.Tick += new System.EventHandler(this.refreshtimer_Tick);
            //
            // close
            //
            this.close.Location = new System.Drawing.Point(158, 538);
            this.close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(112, 35);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            //
            // EnhancedObjectInspector
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 588);
            this.Controls.Add(this.close);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "EnhancedObjectInspector";
            this.Text = "Objects / Timers Inspector";
            this.Load += new System.EventHandler(this.EnhancedObjectInspector_Load);
            this.tabControl1.ResumeLayout(false);
            this.objecttabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sharedobjectGridView)).EndInit();
            this.timerstabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timerGridView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage objecttabPage;
		private System.Windows.Forms.TabPage timerstabPage;
		private System.Windows.Forms.DataGridView sharedobjectGridView;
		private System.Windows.Forms.DataGridView timerGridView;
		private RazorButton close;
		private System.Windows.Forms.Timer refreshtimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}
