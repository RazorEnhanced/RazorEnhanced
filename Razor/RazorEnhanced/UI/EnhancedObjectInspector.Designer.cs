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
			this.close = new RazorEnhanced.UI.RazorButton();
			this.refreshtimer = new System.Windows.Forms.Timer(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControl1.SuspendLayout();
			this.objecttabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sharedobjectGridView)).BeginInit();
			this.timerstabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timerGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.objecttabPage);
			this.tabControl1.Controls.Add(this.timerstabPage);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(266, 332);
			this.tabControl1.TabIndex = 0;
			// 
			// objecttabPage
			// 
			this.objecttabPage.Controls.Add(this.sharedobjectGridView);
			this.objecttabPage.Location = new System.Drawing.Point(4, 22);
			this.objecttabPage.Name = "objecttabPage";
			this.objecttabPage.Padding = new System.Windows.Forms.Padding(3);
			this.objecttabPage.Size = new System.Drawing.Size(258, 306);
			this.objecttabPage.TabIndex = 0;
			this.objecttabPage.Text = "Shared Objects";
			this.objecttabPage.UseVisualStyleBackColor = true;
			// 
			// sharedobjectGridView
			// 
			this.sharedobjectGridView.AllowUserToAddRows = false;
			this.sharedobjectGridView.AllowUserToDeleteRows = false;
			this.sharedobjectGridView.AllowUserToOrderColumns = true;
			this.sharedobjectGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.sharedobjectGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Alias,
            this.Value});
			this.sharedobjectGridView.Location = new System.Drawing.Point(6, 6);
			this.sharedobjectGridView.Name = "sharedobjectGridView";
			this.sharedobjectGridView.ReadOnly = true;
			this.sharedobjectGridView.RowHeadersVisible = false;
			this.sharedobjectGridView.Size = new System.Drawing.Size(245, 292);
			this.sharedobjectGridView.TabIndex = 0;
			// 
			// Alias
			// 
			this.Alias.HeaderText = "Alias";
			this.Alias.Name = "Alias";
			this.Alias.ReadOnly = true;
			this.Alias.Width = 120;
			// 
			// Value
			// 
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			this.Value.ReadOnly = true;
			// 
			// timerstabPage
			// 
			this.timerstabPage.Controls.Add(this.timerGridView);
			this.timerstabPage.Location = new System.Drawing.Point(4, 22);
			this.timerstabPage.Name = "timerstabPage";
			this.timerstabPage.Padding = new System.Windows.Forms.Padding(3);
			this.timerstabPage.Size = new System.Drawing.Size(258, 306);
			this.timerstabPage.TabIndex = 1;
			this.timerstabPage.Text = "Timers";
			this.timerstabPage.UseVisualStyleBackColor = true;
			// 
			// timerGridView
			// 
			this.timerGridView.AllowUserToAddRows = false;
			this.timerGridView.AllowUserToDeleteRows = false;
			this.timerGridView.AllowUserToOrderColumns = true;
			this.timerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.timerGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
			this.timerGridView.Location = new System.Drawing.Point(7, 7);
			this.timerGridView.Name = "timerGridView";
			this.timerGridView.ReadOnly = true;
			this.timerGridView.RowHeadersVisible = false;
			this.timerGridView.Size = new System.Drawing.Size(245, 292);
			this.timerGridView.TabIndex = 1;
			// 
			// close
			// 
			this.close.Location = new System.Drawing.Point(105, 350);
			this.close.Name = "close";
			this.close.Size = new System.Drawing.Size(75, 23);
			this.close.TabIndex = 3;
			this.close.Text = "Close";
			this.close.UseVisualStyleBackColor = true;
			this.close.Click += new System.EventHandler(this.close_Click);
			// 
			// refreshtimer
			// 
			this.refreshtimer.Enabled = true;
			this.refreshtimer.Interval = 1000;
			this.refreshtimer.Tick += new System.EventHandler(this.refreshtimer_Tick);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Alias";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 120;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.HeaderText = "Duration";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// EnhancedObjectInspector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 382);
			this.Controls.Add(this.close);
			this.Controls.Add(this.tabControl1);
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
		private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		private System.Windows.Forms.DataGridView timerGridView;
		private RazorButton close;
		private System.Windows.Forms.Timer refreshtimer;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
	}
}