namespace UTFEditor
{
    partial class ModelViewForm
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
            this.modelView = new System.Windows.Forms.SplitContainer();
            this.lblHardpointName = new System.Windows.Forms.Label();
            this.splitViewHardpoint = new System.Windows.Forms.SplitContainer();
            this.viewPanelView = new System.Windows.Forms.DataGridView();
            this.colMPVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMPElement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMPShading = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colMPColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMPTexture = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.hardpointPanelView = new System.Windows.Forms.DataGridView();
            this.colHPVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHPRevolute = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHPColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editHardpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editHardpointsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addHardpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fuseComposerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerOnHardpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardpointSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripHardpointSizeSet = new System.Windows.Forms.ToolStripTextBox();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opaqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showViewPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDiag = new System.Windows.Forms.ColorDialog();
            this.centersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.modelView)).BeginInit();
            this.modelView.Panel1.SuspendLayout();
            this.modelView.Panel2.SuspendLayout();
            this.modelView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitViewHardpoint)).BeginInit();
            this.splitViewHardpoint.Panel1.SuspendLayout();
            this.splitViewHardpoint.Panel2.SuspendLayout();
            this.splitViewHardpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewPanelView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hardpointPanelView)).BeginInit();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelView
            // 
            this.modelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelView.Location = new System.Drawing.Point(0, 27);
            this.modelView.Name = "modelView";
            // 
            // modelView.Panel1
            // 
            this.modelView.Panel1.BackColor = System.Drawing.Color.Black;
            this.modelView.Panel1.Controls.Add(this.lblHardpointName);
            this.modelView.Panel1.Margin = new System.Windows.Forms.Padding(0, 24, 0, 0);
            this.modelView.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.modelView_Paint);
            this.modelView.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseClick);
            this.modelView.Panel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.modelView_Panel1_MouseDoubleClick);
            this.modelView.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseDown);
            this.modelView.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseMove);
            this.modelView.Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelView_Panel1_MouseUp);
            this.modelView.Panel1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.modelView_Panel1_PreviewKeyDown);
            this.modelView.Panel1.Resize += new System.EventHandler(this.modelView_Panel1_Resize);
            // 
            // modelView.Panel2
            // 
            this.modelView.Panel2.Controls.Add(this.splitViewHardpoint);
            this.modelView.Panel2MinSize = 0;
            this.modelView.Size = new System.Drawing.Size(1008, 703);
            this.modelView.SplitterDistance = 688;
            this.modelView.TabIndex = 0;
            // 
            // lblHardpointName
            // 
            this.lblHardpointName.AutoSize = true;
            this.lblHardpointName.BackColor = System.Drawing.Color.White;
            this.lblHardpointName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHardpointName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHardpointName.Location = new System.Drawing.Point(0, 0);
            this.lblHardpointName.Name = "lblHardpointName";
            this.lblHardpointName.Size = new System.Drawing.Size(102, 24);
            this.lblHardpointName.TabIndex = 0;
            this.lblHardpointName.Text = "HpName01";
            this.lblHardpointName.Visible = false;
            // 
            // splitViewHardpoint
            // 
            this.splitViewHardpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitViewHardpoint.Location = new System.Drawing.Point(0, 0);
            this.splitViewHardpoint.Name = "splitViewHardpoint";
            this.splitViewHardpoint.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitViewHardpoint.Panel1
            // 
            this.splitViewHardpoint.Panel1.Controls.Add(this.viewPanelView);
            // 
            // splitViewHardpoint.Panel2
            // 
            this.splitViewHardpoint.Panel2.Controls.Add(this.hardpointPanelView);
            this.splitViewHardpoint.Panel2Collapsed = true;
            this.splitViewHardpoint.Size = new System.Drawing.Size(316, 703);
            this.splitViewHardpoint.SplitterDistance = 320;
            this.splitViewHardpoint.TabIndex = 5;
            // 
            // viewPanelView
            // 
            this.viewPanelView.AllowUserToAddRows = false;
            this.viewPanelView.AllowUserToDeleteRows = false;
            this.viewPanelView.AllowUserToResizeRows = false;
            this.viewPanelView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewPanelView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMPVisible,
            this.colMPElement,
            this.colMPShading,
            this.colMPColor,
            this.colMPTexture});
            this.viewPanelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanelView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.viewPanelView.Location = new System.Drawing.Point(0, 0);
            this.viewPanelView.Margin = new System.Windows.Forms.Padding(0);
            this.viewPanelView.MultiSelect = false;
            this.viewPanelView.Name = "viewPanelView";
            this.viewPanelView.RowHeadersVisible = false;
            this.viewPanelView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.viewPanelView.Size = new System.Drawing.Size(316, 703);
            this.viewPanelView.TabIndex = 3;
            this.viewPanelView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewPanelView_CellValueChanged);
            this.viewPanelView.CurrentCellDirtyStateChanged += new System.EventHandler(this.viewPanelView_CurrentCellDirtyStateChanged);
            this.viewPanelView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.viewPanelView_SortCompare);
            this.viewPanelView.DoubleClick += new System.EventHandler(this.viewPanelView_DoubleClick);
            // 
            // colMPVisible
            // 
            this.colMPVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMPVisible.HeaderText = "Visible";
            this.colMPVisible.Name = "colMPVisible";
            this.colMPVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMPVisible.Width = 49;
            // 
            // colMPElement
            // 
            this.colMPElement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMPElement.HeaderText = "Element";
            this.colMPElement.MinimumWidth = 50;
            this.colMPElement.Name = "colMPElement";
            this.colMPElement.ReadOnly = true;
            this.colMPElement.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMPElement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMPElement.Width = 59;
            // 
            // colMPShading
            // 
            this.colMPShading.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMPShading.HeaderText = "Shading";
            this.colMPShading.Items.AddRange(new object[] {
            "Flat",
            "Wireframe"});
            this.colMPShading.Name = "colMPShading";
            this.colMPShading.Width = 59;
            // 
            // colMPColor
            // 
            this.colMPColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMPColor.HeaderText = "Color (ABGR)";
            this.colMPColor.Name = "colMPColor";
            this.colMPColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMPColor.Width = 86;
            // 
            // colMPTexture
            // 
            this.colMPTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMPTexture.HeaderText = "Texture";
            this.colMPTexture.Items.AddRange(new object[] {
            "TextureColor",
            "Texture",
            "Color",
            "None"});
            this.colMPTexture.Name = "colMPTexture";
            this.colMPTexture.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMPTexture.Width = 54;
            // 
            // hardpointPanelView
            // 
            this.hardpointPanelView.AllowUserToAddRows = false;
            this.hardpointPanelView.AllowUserToDeleteRows = false;
            this.hardpointPanelView.AllowUserToResizeRows = false;
            this.hardpointPanelView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hardpointPanelView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHPVisible,
            this.colHPName,
            this.colHPRevolute,
            this.colHPColor});
            this.hardpointPanelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hardpointPanelView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.hardpointPanelView.Location = new System.Drawing.Point(0, 0);
            this.hardpointPanelView.Margin = new System.Windows.Forms.Padding(0);
            this.hardpointPanelView.MultiSelect = false;
            this.hardpointPanelView.Name = "hardpointPanelView";
            this.hardpointPanelView.RowHeadersVisible = false;
            this.hardpointPanelView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.hardpointPanelView.Size = new System.Drawing.Size(112, 37);
            this.hardpointPanelView.TabIndex = 0;
            this.hardpointPanelView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.hardpointPanelView_CellValueChanged);
            this.hardpointPanelView.CurrentCellDirtyStateChanged += new System.EventHandler(this.hardpointPanelView_CurrentCellDirtyStateChanged);
            this.hardpointPanelView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.hardpointPanelView_SortCompare);
            this.hardpointPanelView.DoubleClick += new System.EventHandler(this.hardpointPanelView_DoubleClick);
            // 
            // colHPVisible
            // 
            this.colHPVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colHPVisible.HeaderText = "Visible";
            this.colHPVisible.Name = "colHPVisible";
            this.colHPVisible.Width = 49;
            // 
            // colHPName
            // 
            this.colHPName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colHPName.HeaderText = "Hardpoint";
            this.colHPName.Name = "colHPName";
            this.colHPName.ReadOnly = true;
            this.colHPName.Width = 88;
            // 
            // colHPRevolute
            // 
            this.colHPRevolute.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colHPRevolute.HeaderText = "Revolute";
            this.colHPRevolute.Name = "colHPRevolute";
            this.colHPRevolute.ReadOnly = true;
            this.colHPRevolute.Width = 61;
            // 
            // colHPColor
            // 
            this.colHPColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colHPColor.HeaderText = "Color";
            this.colHPColor.Name = "colHPColor";
            this.colHPColor.Width = 63;
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.visibilityToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.showViewPanelToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1008, 27);
            this.menu.TabIndex = 8;
            this.menu.Text = "menu";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetAllToolStripMenuItem1,
            this.editHardpointsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // resetAllToolStripMenuItem1
            // 
            this.resetAllToolStripMenuItem1.Name = "resetAllToolStripMenuItem1";
            this.resetAllToolStripMenuItem1.Size = new System.Drawing.Size(166, 24);
            this.resetAllToolStripMenuItem1.Text = "Reset Settings";
            this.resetAllToolStripMenuItem1.Click += new System.EventHandler(this.resetAllToolStripMenuItem_Click);
            // 
            // editHardpointsToolStripMenuItem
            // 
            this.editHardpointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editHardpointsToolStripMenuItem1,
            this.addHardpointsToolStripMenuItem,
            this.fuseComposerToolStripMenuItem});
            this.editHardpointsToolStripMenuItem.Name = "editHardpointsToolStripMenuItem";
            this.editHardpointsToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.editHardpointsToolStripMenuItem.Text = "Mode";
            // 
            // editHardpointsToolStripMenuItem1
            // 
            this.editHardpointsToolStripMenuItem1.Name = "editHardpointsToolStripMenuItem1";
            this.editHardpointsToolStripMenuItem1.Size = new System.Drawing.Size(177, 24);
            this.editHardpointsToolStripMenuItem1.Text = "Edit Hardpoints";
            this.editHardpointsToolStripMenuItem1.Click += new System.EventHandler(this.hardpointEditToolStripMenuItem_Click);
            // 
            // addHardpointsToolStripMenuItem
            // 
            this.addHardpointsToolStripMenuItem.Name = "addHardpointsToolStripMenuItem";
            this.addHardpointsToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.addHardpointsToolStripMenuItem.Text = "Add Hardpoints";
            this.addHardpointsToolStripMenuItem.Click += new System.EventHandler(this.addHardpointsToolStripMenuItem_Click);
            // 
            // fuseComposerToolStripMenuItem
            // 
            this.fuseComposerToolStripMenuItem.Name = "fuseComposerToolStripMenuItem";
            this.fuseComposerToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.fuseComposerToolStripMenuItem.Text = "Fuse Composer";
            this.fuseComposerToolStripMenuItem.Click += new System.EventHandler(this.fuseComposerToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bottomToolStripMenuItem,
            this.topToolStripMenuItem,
            this.backToolStripMenuItem,
            this.frontToolStripMenuItem,
            this.rightToolStripMenuItem,
            this.leftToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.centerOnHardpointToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(50, 23);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // bottomToolStripMenuItem
            // 
            this.bottomToolStripMenuItem.Name = "bottomToolStripMenuItem";
            this.bottomToolStripMenuItem.ShortcutKeyDisplayString = "1";
            this.bottomToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.bottomToolStripMenuItem.Text = "Bottom";
            this.bottomToolStripMenuItem.Click += new System.EventHandler(this.bottomToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.ShortcutKeyDisplayString = "2";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.topToolStripMenuItem.Text = "Top";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // backToolStripMenuItem
            // 
            this.backToolStripMenuItem.Name = "backToolStripMenuItem";
            this.backToolStripMenuItem.ShortcutKeyDisplayString = "3";
            this.backToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.backToolStripMenuItem.Text = "Back";
            this.backToolStripMenuItem.Click += new System.EventHandler(this.backToolStripMenuItem_Click);
            // 
            // frontToolStripMenuItem
            // 
            this.frontToolStripMenuItem.Name = "frontToolStripMenuItem";
            this.frontToolStripMenuItem.ShortcutKeyDisplayString = "4";
            this.frontToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.frontToolStripMenuItem.Text = "Front";
            this.frontToolStripMenuItem.Click += new System.EventHandler(this.frontToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.ShortcutKeyDisplayString = "5";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.rightToolStripMenuItem.Text = "Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.ShortcutKeyDisplayString = "6";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.leftToolStripMenuItem.Text = "Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeyDisplayString = "Home";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // centerOnHardpointToolStripMenuItem
            // 
            this.centerOnHardpointToolStripMenuItem.Name = "centerOnHardpointToolStripMenuItem";
            this.centerOnHardpointToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Home";
            this.centerOnHardpointToolStripMenuItem.Size = new System.Drawing.Size(290, 24);
            this.centerOnHardpointToolStripMenuItem.Text = "Center on Hardpoint";
            this.centerOnHardpointToolStripMenuItem.Click += new System.EventHandler(this.centerOnHardpointToolStripMenuItem_Click);
            // 
            // visibilityToolStripMenuItem
            // 
            this.visibilityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundToolStripMenuItem,
            this.hardpointSizeToolStripMenuItem,
            this.zoomToolStripMenuItem,
            this.sURToolStripMenuItem});
            this.visibilityToolStripMenuItem.Name = "visibilityToolStripMenuItem";
            this.visibilityToolStripMenuItem.Size = new System.Drawing.Size(71, 23);
            this.visibilityToolStripMenuItem.Text = "Visibility";
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackToolStripMenuItem,
            this.whiteToolStripMenuItem});
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.backgroundToolStripMenuItem.Text = "Background";
            // 
            // blackToolStripMenuItem
            // 
            this.blackToolStripMenuItem.Checked = true;
            this.blackToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
            this.blackToolStripMenuItem.ShortcutKeyDisplayString = "B";
            this.blackToolStripMenuItem.Size = new System.Drawing.Size(133, 24);
            this.blackToolStripMenuItem.Text = "Black";
            this.blackToolStripMenuItem.Click += new System.EventHandler(this.blackToolStripMenuItem_Click);
            // 
            // whiteToolStripMenuItem
            // 
            this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
            this.whiteToolStripMenuItem.ShortcutKeyDisplayString = "B";
            this.whiteToolStripMenuItem.Size = new System.Drawing.Size(133, 24);
            this.whiteToolStripMenuItem.Text = "White";
            this.whiteToolStripMenuItem.Click += new System.EventHandler(this.whiteToolStripMenuItem_Click);
            // 
            // hardpointSizeToolStripMenuItem
            // 
            this.hardpointSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decreaseToolStripMenuItem1,
            this.increaseToolStripMenuItem1,
            this.toolStripHardpointSizeSet});
            this.hardpointSizeToolStripMenuItem.Name = "hardpointSizeToolStripMenuItem";
            this.hardpointSizeToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.hardpointSizeToolStripMenuItem.Text = "Hardpoint Size";
            // 
            // decreaseToolStripMenuItem1
            // 
            this.decreaseToolStripMenuItem1.Name = "decreaseToolStripMenuItem1";
            this.decreaseToolStripMenuItem1.ShortcutKeyDisplayString = "/";
            this.decreaseToolStripMenuItem1.Size = new System.Drawing.Size(162, 24);
            this.decreaseToolStripMenuItem1.Text = "Decrease";
            this.decreaseToolStripMenuItem1.Click += new System.EventHandler(this.decreaseToolStripMenuItem1_Click);
            // 
            // increaseToolStripMenuItem1
            // 
            this.increaseToolStripMenuItem1.Name = "increaseToolStripMenuItem1";
            this.increaseToolStripMenuItem1.ShortcutKeyDisplayString = "*";
            this.increaseToolStripMenuItem1.Size = new System.Drawing.Size(162, 24);
            this.increaseToolStripMenuItem1.Text = "Increase";
            this.increaseToolStripMenuItem1.Click += new System.EventHandler(this.increaseToolStripMenuItem1_Click);
            // 
            // toolStripHardpointSizeSet
            // 
            this.toolStripHardpointSizeSet.Name = "toolStripHardpointSizeSet";
            this.toolStripHardpointSizeSet.Size = new System.Drawing.Size(100, 25);
            this.toolStripHardpointSizeSet.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripHardpointSizeSet.TextChanged += new System.EventHandler(this.toolStripHardpointSizeSet_TextChanged);
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inToolStripMenuItem,
            this.outToolStripMenuItem});
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.zoomToolStripMenuItem.Text = "Zoom";
            // 
            // inToolStripMenuItem
            // 
            this.inToolStripMenuItem.Name = "inToolStripMenuItem";
            this.inToolStripMenuItem.ShortcutKeyDisplayString = "+";
            this.inToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.inToolStripMenuItem.Text = "In";
            this.inToolStripMenuItem.Click += new System.EventHandler(this.inToolStripMenuItem_Click);
            // 
            // outToolStripMenuItem
            // 
            this.outToolStripMenuItem.Name = "outToolStripMenuItem";
            this.outToolStripMenuItem.ShortcutKeyDisplayString = "-";
            this.outToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.outToolStripMenuItem.Text = "Out";
            this.outToolStripMenuItem.Click += new System.EventHandler(this.outToolStripMenuItem_Click);
            // 
            // sURToolStripMenuItem
            // 
            this.sURToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hiddenToolStripMenuItem,
            this.wireframeToolStripMenuItem,
            this.transparentToolStripMenuItem,
            this.opaqueToolStripMenuItem,
            this.centersToolStripMenuItem});
            this.sURToolStripMenuItem.Name = "sURToolStripMenuItem";
            this.sURToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.sURToolStripMenuItem.Text = "SUR";
            // 
            // hiddenToolStripMenuItem
            // 
            this.hiddenToolStripMenuItem.Checked = true;
            this.hiddenToolStripMenuItem.CheckOnClick = true;
            this.hiddenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hiddenToolStripMenuItem.Name = "hiddenToolStripMenuItem";
            this.hiddenToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.hiddenToolStripMenuItem.Text = "Hidden";
            this.hiddenToolStripMenuItem.Click += new System.EventHandler(this.hiddenToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.CheckOnClick = true;
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // transparentToolStripMenuItem
            // 
            this.transparentToolStripMenuItem.CheckOnClick = true;
            this.transparentToolStripMenuItem.Name = "transparentToolStripMenuItem";
            this.transparentToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.transparentToolStripMenuItem.Text = "Transparent";
            this.transparentToolStripMenuItem.Click += new System.EventHandler(this.transparentToolStripMenuItem_Click);
            // 
            // opaqueToolStripMenuItem
            // 
            this.opaqueToolStripMenuItem.CheckOnClick = true;
            this.opaqueToolStripMenuItem.Name = "opaqueToolStripMenuItem";
            this.opaqueToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.opaqueToolStripMenuItem.Text = "Opaque";
            this.opaqueToolStripMenuItem.Click += new System.EventHandler(this.opaqueToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortcutsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(49, 23);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // shortcutsToolStripMenuItem
            // 
            this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
            this.shortcutsToolStripMenuItem.ShortcutKeyDisplayString = "F1";
            this.shortcutsToolStripMenuItem.Size = new System.Drawing.Size(171, 24);
            this.shortcutsToolStripMenuItem.Text = "Shortcuts...";
            this.shortcutsToolStripMenuItem.Click += new System.EventHandler(this.shortcutsToolStripMenuItem_Click);
            // 
            // showViewPanelToolStripMenuItem
            // 
            this.showViewPanelToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.showViewPanelToolStripMenuItem.Checked = true;
            this.showViewPanelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showViewPanelToolStripMenuItem.Name = "showViewPanelToolStripMenuItem";
            this.showViewPanelToolStripMenuItem.Size = new System.Drawing.Size(131, 23);
            this.showViewPanelToolStripMenuItem.Text = "Toggle View Panel";
            this.showViewPanelToolStripMenuItem.Click += new System.EventHandler(this.showViewPanelToolStripMenuItem_Click);
            // 
            // centersToolStripMenuItem
            // 
            this.centersToolStripMenuItem.Name = "centersToolStripMenuItem";
            this.centersToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.centersToolStripMenuItem.Text = "Centers";
            this.centersToolStripMenuItem.Click += new System.EventHandler(this.centersToolStripMenuItem_Click);
            // 
            // ModelViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.modelView);
            this.Controls.Add(this.menu);
            this.Name = "ModelViewForm";
            this.ShowIcon = false;
            this.Text = "x";
            this.Activated += new System.EventHandler(this.ModelViewForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelViewForm_FormClosing);
            this.Load += new System.EventHandler(this.ModelViewForm_Load);
            this.modelView.Panel1.ResumeLayout(false);
            this.modelView.Panel1.PerformLayout();
            this.modelView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.modelView)).EndInit();
            this.modelView.ResumeLayout(false);
            this.splitViewHardpoint.Panel1.ResumeLayout(false);
            this.splitViewHardpoint.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitViewHardpoint)).EndInit();
            this.splitViewHardpoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewPanelView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hardpointPanelView)).EndInit();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
		private System.Windows.Forms.SplitContainer modelView;
		private System.Windows.Forms.DataGridView viewPanelView;
		private System.Windows.Forms.MenuStrip menu;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bottomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem backToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem frontToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visibilityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hardpointSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decreaseToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem increaseToolStripMenuItem1;
		private System.Windows.Forms.ToolStripTextBox toolStripHardpointSizeSet;
		private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerOnHardpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shortcutsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showViewPanelToolStripMenuItem;
		private System.Windows.Forms.ColorDialog colorDiag;
		private System.Windows.Forms.SplitContainer splitViewHardpoint;
		private System.Windows.Forms.DataGridView hardpointPanelView;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colHPVisible;
		private System.Windows.Forms.DataGridViewTextBoxColumn colHPName;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colHPRevolute;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHPColor;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editHardpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editHardpointsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addHardpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fuseComposerToolStripMenuItem;
        private System.Windows.Forms.Label lblHardpointName;
        private System.Windows.Forms.ToolStripMenuItem sURToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opaqueToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMPVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMPElement;
        private System.Windows.Forms.DataGridViewComboBoxColumn colMPShading;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMPColor;
        private System.Windows.Forms.DataGridViewComboBoxColumn colMPTexture;
        private System.Windows.Forms.ToolStripMenuItem centersToolStripMenuItem;
    }
}