using System.Drawing;

namespace RazorEnhanced.UI
{
    partial class EnhancedGridContainerViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedGridContainerViewer));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProps = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerial = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.useItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToBackpackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToContainerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToBankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToGroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnFilter = new System.Windows.Forms.ToolStripButton();
            this.btnSort = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnSortName = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSortSerial = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSortType = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSortHue = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSortAmount = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxAttributes = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnProps,
            this.columnSerial,
            this.columnType,
            this.columnHue,
            this.columnAmount});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.ForeColor = System.Drawing.Color.White;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(617, 502);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnProps
            // 
            this.columnProps.Text = "Props";
            this.columnProps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnSerial
            // 
            this.columnSerial.Text = "Serial";
            this.columnSerial.Width = 0;
            // 
            // columnType
            // 
            this.columnType.Text = "Type";
            this.columnType.Width = 0;
            // 
            // columnHue
            // 
            this.columnHue.Text = "Hue";
            this.columnHue.Width = 0;
            // 
            // columnAmount
            // 
            this.columnAmount.Text = "Amount";
            this.columnAmount.Width = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useItemToolStripMenuItem,
            this.moveToBackpackToolStripMenuItem,
            this.moveToContainerToolStripMenuItem,
            this.moveToBankToolStripMenuItem,
            this.moveToGroundToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 114);
            // 
            // useItemToolStripMenuItem
            // 
            this.useItemToolStripMenuItem.Name = "useItemToolStripMenuItem";
            this.useItemToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.useItemToolStripMenuItem.Text = "Use item";
            this.useItemToolStripMenuItem.Click += new System.EventHandler(this.useItemToolStripMenuItem_Click);
            // 
            // moveToBackpackToolStripMenuItem
            // 
            this.moveToBackpackToolStripMenuItem.Name = "moveToBackpackToolStripMenuItem";
            this.moveToBackpackToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.moveToBackpackToolStripMenuItem.Text = "Move to backpack";
            this.moveToBackpackToolStripMenuItem.Click += new System.EventHandler(this.moveToBackpackToolStripMenuItem_Click);
            // 
            // moveToContainerToolStripMenuItem
            // 
            this.moveToContainerToolStripMenuItem.Name = "moveToContainerToolStripMenuItem";
            this.moveToContainerToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.moveToContainerToolStripMenuItem.Text = "Move to container";
            this.moveToContainerToolStripMenuItem.Click += new System.EventHandler(this.moveToContainerToolStripMenuItem_Click);
            // 
            // moveToBankToolStripMenuItem
            // 
            this.moveToBankToolStripMenuItem.Name = "moveToBankToolStripMenuItem";
            this.moveToBankToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.moveToBankToolStripMenuItem.Text = "Move to bank";
            this.moveToBankToolStripMenuItem.Click += new System.EventHandler(this.moveToBankToolStripMenuItem_Click);
            // 
            // moveToGroundToolStripMenuItem
            // 
            this.moveToGroundToolStripMenuItem.Name = "moveToGroundToolStripMenuItem";
            this.moveToGroundToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.moveToGroundToolStripMenuItem.Text = "Move to ground ";
            this.moveToGroundToolStripMenuItem.Click += new System.EventHandler(this.moveToGroundToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6});
            this.statusStrip1.Location = new System.Drawing.Point(0, 527);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(876, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel1.Text = "{0}";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel2.Text = "items,";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel3.Text = "{0}";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(53, 17);
            this.toolStripStatusLabel4.Text = "selected,";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel5.Text = "{0}";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusLabel6.Text = "amount";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.btnFilter,
            this.btnSort});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(876, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(66, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnFilter.Image")));
            this.btnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(53, 22);
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnSort
            // 
            this.btnSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSortName,
            this.btnSortSerial,
            this.btnSortType,
            this.btnSortHue,
            this.btnSortAmount});
            this.btnSort.Image = ((System.Drawing.Image)(resources.GetObject("btnSort.Image")));
            this.btnSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(58, 22);
            this.btnSort.Text = "Sort";
            // 
            // btnSortName
            // 
            this.btnSortName.Name = "btnSortName";
            this.btnSortName.Size = new System.Drawing.Size(118, 22);
            this.btnSortName.Text = "Name";
            this.btnSortName.Click += new System.EventHandler(this.btnSortName_Click);
            // 
            // btnSortSerial
            // 
            this.btnSortSerial.Name = "btnSortSerial";
            this.btnSortSerial.Size = new System.Drawing.Size(118, 22);
            this.btnSortSerial.Text = "Serial";
            this.btnSortSerial.Click += new System.EventHandler(this.btnSortSerial_Click);
            // 
            // btnSortType
            // 
            this.btnSortType.Name = "btnSortType";
            this.btnSortType.Size = new System.Drawing.Size(118, 22);
            this.btnSortType.Text = "Type";
            this.btnSortType.Click += new System.EventHandler(this.btnSortType_Click);
            // 
            // btnSortHue
            // 
            this.btnSortHue.Name = "btnSortHue";
            this.btnSortHue.Size = new System.Drawing.Size(118, 22);
            this.btnSortHue.Text = "Hue";
            this.btnSortHue.Click += new System.EventHandler(this.btnSortHue_Click);
            // 
            // btnSortAmount
            // 
            this.btnSortAmount.Name = "btnSortAmount";
            this.btnSortAmount.Size = new System.Drawing.Size(118, 22);
            this.btnSortAmount.Text = "Amount";
            this.btnSortAmount.Click += new System.EventHandler(this.btnSortAmount_Click);
            // 
            // listBoxAttributes
            // 
            this.listBoxAttributes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.listBoxAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAttributes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxAttributes.ForeColor = System.Drawing.Color.White;
            this.listBoxAttributes.FormattingEnabled = true;
            this.listBoxAttributes.ItemHeight = 20;
            this.listBoxAttributes.Location = new System.Drawing.Point(0, 0);
            this.listBoxAttributes.Name = "listBoxAttributes";
            this.listBoxAttributes.Size = new System.Drawing.Size(255, 502);
            this.listBoxAttributes.TabIndex = 1;
            this.listBoxAttributes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxAttributes_DrawItem);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBoxAttributes);
            this.splitContainer1.Size = new System.Drawing.Size(876, 502);
            this.splitContainer1.SplitterDistance = 617;
            this.splitContainer1.TabIndex = 4;
            // 
            // EnhancedGridContainerViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 549);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "EnhancedGridContainerViewer";
            this.Text = "Enhanced Grid Container Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnhancedGridContainerViewer_FormClosing);
            this.Load += new System.EventHandler(this.EnhancedGridContainerViewer_Load);
            this.Shown += new System.EventHandler(this.EnhancedGridContainerViewer_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnProps;
        private System.Windows.Forms.ColumnHeader columnSerial;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton btnFilter;
        private System.Windows.Forms.ToolStripDropDownButton btnSort;
        private System.Windows.Forms.ToolStripMenuItem btnSortName;
        private System.Windows.Forms.ToolStripMenuItem btnSortSerial;
        private System.Windows.Forms.ToolStripMenuItem btnSortHue;
        private System.Windows.Forms.ToolStripMenuItem btnSortType;
        private System.Windows.Forms.ToolStripMenuItem btnSortAmount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem useItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToBackpackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToContainerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToBankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToGroundToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxAttributes;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnHue;
        private System.Windows.Forms.ColumnHeader columnAmount;
    }
}