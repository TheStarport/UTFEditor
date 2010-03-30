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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemRenameNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImportData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExportData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditData = new System.Windows.Forms.ToolStripMenuItem();
            this.stringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intArrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floatArrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExportStructure = new System.Windows.Forms.ToolStripMenuItem();
            this.vMeshDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vMeshRefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vWireDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemPlaySound = new System.Windows.Forms.ToolStripMenuItem();
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
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(296, 411);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRenameNode,
            this.toolStripMenuItemAddNode,
            this.toolStripMenuItemDeleteNode,
            this.toolStripMenuItemImportData,
            this.toolStripMenuItemExportData,
            this.toolStripMenuItemEditData,
            this.toolStripSeparator1,
            this.toolStripMenuItemExportStructure,
            this.toolStripSeparator2,
            this.toolStripMenuItemPlaySound,
            this.toolStripSeparator3,
            this.toolStripMenuItemExpandAll,
            this.toolStripMenuItemCollapseAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 264);
            // 
            // toolStripMenuItemRenameNode
            // 
            this.toolStripMenuItemRenameNode.Name = "toolStripMenuItemRenameNode";
            this.toolStripMenuItemRenameNode.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemRenameNode.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemRenameNode.Text = "Rename";
            this.toolStripMenuItemRenameNode.Click += new System.EventHandler(this.toolStripMenuItemRenameNode_Click);
            // 
            // toolStripMenuItemAddNode
            // 
            this.toolStripMenuItemAddNode.Name = "toolStripMenuItemAddNode";
            this.toolStripMenuItemAddNode.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemAddNode.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemAddNode.Text = "Add Node";
            this.toolStripMenuItemAddNode.Click += new System.EventHandler(this.toolStripMenuItemAddNode_Click);
            // 
            // toolStripMenuItemDeleteNode
            // 
            this.toolStripMenuItemDeleteNode.Name = "toolStripMenuItemDeleteNode";
            this.toolStripMenuItemDeleteNode.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemDeleteNode.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemDeleteNode.Text = "Delete Node";
            this.toolStripMenuItemDeleteNode.Click += new System.EventHandler(this.toolStripMenuItemDeleteNode_Click);
            // 
            // toolStripMenuItemImportData
            // 
            this.toolStripMenuItemImportData.Name = "toolStripMenuItemImportData";
            this.toolStripMenuItemImportData.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemImportData.Text = "Import Data";
            this.toolStripMenuItemImportData.Click += new System.EventHandler(this.toolStripMenuItemImportData_Click);
            // 
            // toolStripMenuItemExportData
            // 
            this.toolStripMenuItemExportData.Name = "toolStripMenuItemExportData";
            this.toolStripMenuItemExportData.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemExportData.Text = "Export Data";
            this.toolStripMenuItemExportData.Click += new System.EventHandler(this.toolStripMenuItemExportData_Click);
            // 
            // toolStripMenuItemEditData
            // 
            this.toolStripMenuItemEditData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stringToolStripMenuItem,
            this.intArrayToolStripMenuItem,
            this.floatArrayToolStripMenuItem});
            this.toolStripMenuItemEditData.Name = "toolStripMenuItemEditData";
            this.toolStripMenuItemEditData.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemEditData.Text = "Edit Data";
            // 
            // stringToolStripMenuItem
            // 
            this.stringToolStripMenuItem.Name = "stringToolStripMenuItem";
            this.stringToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.stringToolStripMenuItem.Text = "String";
            this.stringToolStripMenuItem.Click += new System.EventHandler(this.stringToolStripMenuItem_Click);
            // 
            // intArrayToolStripMenuItem
            // 
            this.intArrayToolStripMenuItem.Name = "intArrayToolStripMenuItem";
            this.intArrayToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.intArrayToolStripMenuItem.Text = "Int Array";
            this.intArrayToolStripMenuItem.Click += new System.EventHandler(this.intArrayToolStripMenuItem_Click);
            // 
            // floatArrayToolStripMenuItem
            // 
            this.floatArrayToolStripMenuItem.Name = "floatArrayToolStripMenuItem";
            this.floatArrayToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.floatArrayToolStripMenuItem.Text = "Float Array";
            this.floatArrayToolStripMenuItem.Click += new System.EventHandler(this.floatArrayToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // toolStripMenuItemExportStructure
            // 
            this.toolStripMenuItemExportStructure.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vMeshDataToolStripMenuItem,
            this.vMeshRefToolStripMenuItem,
            this.vWireDataToolStripMenuItem});
            this.toolStripMenuItemExportStructure.Name = "toolStripMenuItemExportStructure";
            this.toolStripMenuItemExportStructure.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemExportStructure.Text = "Export Structure";
            // 
            // vMeshDataToolStripMenuItem
            // 
            this.vMeshDataToolStripMenuItem.Name = "vMeshDataToolStripMenuItem";
            this.vMeshDataToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.vMeshDataToolStripMenuItem.Text = "VMeshData";
            this.vMeshDataToolStripMenuItem.Click += new System.EventHandler(this.vMeshDataToolStripMenuItem_Click);
            // 
            // vMeshRefToolStripMenuItem
            // 
            this.vMeshRefToolStripMenuItem.Name = "vMeshRefToolStripMenuItem";
            this.vMeshRefToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.vMeshRefToolStripMenuItem.Text = "VMeshRef";
            this.vMeshRefToolStripMenuItem.Click += new System.EventHandler(this.vMeshRefToolStripMenuItem_Click);
            // 
            // vWireDataToolStripMenuItem
            // 
            this.vWireDataToolStripMenuItem.Name = "vWireDataToolStripMenuItem";
            this.vWireDataToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.vWireDataToolStripMenuItem.Text = "VWireData";
            this.vWireDataToolStripMenuItem.Click += new System.EventHandler(this.vWireDataToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // toolStripMenuItemPlaySound
            // 
            this.toolStripMenuItemPlaySound.Name = "toolStripMenuItemPlaySound";
            this.toolStripMenuItemPlaySound.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemPlaySound.Text = "Play Sound";
            this.toolStripMenuItemPlaySound.Click += new System.EventHandler(this.toolStripMenuItemPlaySound_Click);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UTFForm_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRenameNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeleteNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImportData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExportData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExportStructure;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPlaySound;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExpandAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCollapseAll;
        private System.Windows.Forms.ToolStripMenuItem stringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intArrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floatArrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vMeshDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vMeshRefToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vWireDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

