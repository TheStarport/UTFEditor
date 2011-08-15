namespace UTFEditor
{
    partial class UTFForm
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
            this.treeView1 = new TreeViewMultiSelect();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemRenameNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImportData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExportData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditData = new System.Windows.Forms.ToolStripMenuItem();
            this.stringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intArrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floatArrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(296, 411);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeFlate);
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeFlate);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.treeView1_PreviewKeyDown);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRenameNode,
            this.toolStripMenuItemAddNode,
            this.toolStripMenuItemDeleteNode,
            this.toolStripMenuItemImportData,
            this.toolStripMenuItemExportData,
            this.toolStripMenuItemEdit,
            this.toolStripMenuItemView,
            this.toolStripMenuItemEditData,
            this.toolStripSeparator3,
            this.toolStripMenuItemExpandAll,
            this.toolStripMenuItemCollapseAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(184, 230);
            // 
            // toolStripMenuItemRenameNode
            // 
            this.toolStripMenuItemRenameNode.Name = "toolStripMenuItemRenameNode";
            this.toolStripMenuItemRenameNode.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemRenameNode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemRenameNode.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemRenameNode.Text = "Rename";
            this.toolStripMenuItemRenameNode.Click += new System.EventHandler(this.toolStripMenuItemRenameNode_Click);
            // 
            // toolStripMenuItemAddNode
            // 
            this.toolStripMenuItemAddNode.Name = "toolStripMenuItemAddNode";
            this.toolStripMenuItemAddNode.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemAddNode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemAddNode.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemAddNode.Text = "Add Node";
            this.toolStripMenuItemAddNode.Click += new System.EventHandler(this.toolStripMenuItemAddNode_Click);
            // 
            // toolStripMenuItemDeleteNode
            // 
            this.toolStripMenuItemDeleteNode.Name = "toolStripMenuItemDeleteNode";
            this.toolStripMenuItemDeleteNode.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemDeleteNode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemDeleteNode.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemDeleteNode.Text = "Delete Node";
            this.toolStripMenuItemDeleteNode.Click += new System.EventHandler(this.toolStripMenuItemDeleteNode_Click);
            // 
            // toolStripMenuItemImportData
            // 
            this.toolStripMenuItemImportData.Name = "toolStripMenuItemImportData";
            this.toolStripMenuItemImportData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.toolStripMenuItemImportData.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemImportData.Text = "Import Data";
            this.toolStripMenuItemImportData.Click += new System.EventHandler(this.toolStripMenuItemImportData_Click);
            // 
            // toolStripMenuItemExportData
            // 
            this.toolStripMenuItemExportData.Name = "toolStripMenuItemExportData";
            this.toolStripMenuItemExportData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.toolStripMenuItemExportData.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemExportData.Text = "Export Data";
            this.toolStripMenuItemExportData.Click += new System.EventHandler(this.toolStripMenuItemExportData_Click);
            // 
            // toolStripMenuItemEdit
            // 
            this.toolStripMenuItemEdit.Name = "toolStripMenuItemEdit";
            this.toolStripMenuItemEdit.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemEdit.Text = "Edit";
            this.toolStripMenuItemEdit.Click += new System.EventHandler(this.toolStripMenuItemEdit_Click);
            // 
            // toolStripMenuItemView
            // 
            this.toolStripMenuItemView.Name = "toolStripMenuItemView";
            this.toolStripMenuItemView.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemView.Text = "View";
            this.toolStripMenuItemView.Click += new System.EventHandler(this.toolStripMenuItemView_Click);
            // 
            // toolStripMenuItemEditData
            // 
            this.toolStripMenuItemEditData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stringToolStripMenuItem,
            this.intArrayToolStripMenuItem,
            this.floatArrayToolStripMenuItem});
            this.toolStripMenuItemEditData.Name = "toolStripMenuItemEditData";
            this.toolStripMenuItemEditData.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemEditData.Text = "Edit As";
            // 
            // stringToolStripMenuItem
            // 
            this.stringToolStripMenuItem.Name = "stringToolStripMenuItem";
            this.stringToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.stringToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.stringToolStripMenuItem.Text = "String";
            this.stringToolStripMenuItem.Click += new System.EventHandler(this.stringToolStripMenuItem_Click);
            // 
            // intArrayToolStripMenuItem
            // 
            this.intArrayToolStripMenuItem.Name = "intArrayToolStripMenuItem";
            this.intArrayToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.intArrayToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.intArrayToolStripMenuItem.Text = "Int Array";
            this.intArrayToolStripMenuItem.Click += new System.EventHandler(this.intArrayToolStripMenuItem_Click);
            // 
            // floatArrayToolStripMenuItem
            // 
            this.floatArrayToolStripMenuItem.Name = "floatArrayToolStripMenuItem";
            this.floatArrayToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.floatArrayToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.floatArrayToolStripMenuItem.Text = "Float Array";
            this.floatArrayToolStripMenuItem.Click += new System.EventHandler(this.floatArrayToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(180, 6);
            // 
            // toolStripMenuItemExpandAll
            // 
            this.toolStripMenuItemExpandAll.Name = "toolStripMenuItemExpandAll";
            this.toolStripMenuItemExpandAll.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemExpandAll.Text = "Expand All";
            this.toolStripMenuItemExpandAll.Click += new System.EventHandler(this.toolStripMenuItemExpandAll_Click);
            // 
            // toolStripMenuItemCollapseAll
            // 
            this.toolStripMenuItemCollapseAll.Name = "toolStripMenuItemCollapseAll";
            this.toolStripMenuItemCollapseAll.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemCollapseAll.Text = "Collapse All";
            this.toolStripMenuItemCollapseAll.Click += new System.EventHandler(this.toolStripMenuItemCollapseAll_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // UTFForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 411);
            this.Controls.Add(this.treeView1);
            this.Name = "UTFForm";
            this.ShowIcon = false;
            this.Text = "UTFFile";
            this.Activated += new System.EventHandler(this.UTFForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UTFForm_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewMultiSelect treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRenameNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeleteNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImportData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExportData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExpandAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCollapseAll;
        private System.Windows.Forms.ToolStripMenuItem stringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intArrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floatArrayToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdit;
    }
}

