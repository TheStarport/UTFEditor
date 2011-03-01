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
			this.trackBarScale = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxScale = new System.Windows.Forms.TextBox();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.modelView = new System.Windows.Forms.SplitContainer();
			this.viewPanelView = new System.Windows.Forms.DataGridView();
			this.colMPVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colMPElement = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colMPShading = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colMPColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colMPTexture = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.menu = new System.Windows.Forms.MenuStrip();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.frontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.navigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.leftfineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rightToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightfineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.upfineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.downfineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.anticlockwiseYaxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.anticlockwiseYaxisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.clockwiseYaxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clockwiseYaxisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.anticlockwiseXaxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.anticlockwiseXaxisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.clockwiseXaxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clockwiseXaxisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.anticlockwiseZaxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.anticlockwiseZaxisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.clockwiseZaxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clockwiseZaxisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.centerOnHardpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showViewPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colorDiag = new System.Windows.Forms.ColorDialog();
			this.lblHardpointHover = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.modelView.Panel2.SuspendLayout();
			this.modelView.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewPanelView)).BeginInit();
			this.menu.SuspendLayout();
			this.SuspendLayout();
			// 
			// trackBarScale
			// 
			this.trackBarScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.trackBarScale.AutoSize = false;
			this.trackBarScale.Location = new System.Drawing.Point(52, 11);
			this.trackBarScale.Maximum = 300;
			this.trackBarScale.Minimum = -300;
			this.trackBarScale.Name = "trackBarScale";
			this.trackBarScale.Size = new System.Drawing.Size(500, 25);
			this.trackBarScale.TabIndex = 2;
			this.trackBarScale.Value = 130;
			this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Zoom";
			// 
			// textBoxScale
			// 
			this.textBoxScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBoxScale.Location = new System.Drawing.Point(558, 12);
			this.textBoxScale.MaxLength = 8;
			this.textBoxScale.Name = "textBoxScale";
			this.textBoxScale.Size = new System.Drawing.Size(54, 20);
			this.textBoxScale.TabIndex = 3;
			this.textBoxScale.Text = "20";
			this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.lblHardpointHover);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.textBoxScale);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.trackBarScale);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1008, 43);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 687);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(1008, 43);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			this.toolStripContainer1.TopToolStripPanelVisible = false;
			// 
			// modelView
			// 
			this.modelView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelView.Location = new System.Drawing.Point(0, 24);
			this.modelView.Name = "modelView";
			// 
			// modelView.Panel1
			// 
			this.modelView.Panel1.BackColor = System.Drawing.Color.Black;
			this.modelView.Panel1.Margin = new System.Windows.Forms.Padding(0, 24, 0, 0);
			this.modelView.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.modelView_Paint);
			this.modelView.Panel1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.modelView_Panel1_PreviewKeyDown);
			this.modelView.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseMove);
			this.modelView.Panel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.modelView_Panel1_MouseDoubleClick);
			this.modelView.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseClick);
			this.modelView.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseDown);
			this.modelView.Panel1.Resize += new System.EventHandler(this.modelView_Panel1_Resize);
			this.modelView.Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelView_Panel1_MouseUp);
			// 
			// modelView.Panel2
			// 
			this.modelView.Panel2.Controls.Add(this.viewPanelView);
			this.modelView.Panel2MinSize = 0;
			this.modelView.Size = new System.Drawing.Size(1008, 663);
			this.modelView.SplitterDistance = 689;
			this.modelView.TabIndex = 0;
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
			this.viewPanelView.Size = new System.Drawing.Size(315, 663);
			this.viewPanelView.TabIndex = 3;
			this.viewPanelView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewPanelView_CellValueChanged);
			this.viewPanelView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.viewPanelView_SortCompare);
			this.viewPanelView.DoubleClick += new System.EventHandler(this.viewPanelView_DoubleClick);
			this.viewPanelView.CurrentCellDirtyStateChanged += new System.EventHandler(this.viewPanelView_CurrentCellDirtyStateChanged);
			// 
			// colMPVisible
			// 
			this.colMPVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colMPVisible.HeaderText = "Visible";
			this.colMPVisible.Name = "colMPVisible";
			this.colMPVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colMPVisible.Width = 43;
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
			this.colMPElement.Width = 51;
			// 
			// colMPShading
			// 
			this.colMPShading.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colMPShading.HeaderText = "Shading";
			this.colMPShading.Items.AddRange(new object[] {
            "Flat",
            "Wireframe"});
			this.colMPShading.Name = "colMPShading";
			this.colMPShading.Width = 52;
			// 
			// colMPColor
			// 
			this.colMPColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colMPColor.HeaderText = "Color (ARGB)";
			this.colMPColor.Name = "colMPColor";
			this.colMPColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colMPColor.Width = 76;
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
			this.colMPTexture.Width = 49;
			// 
			// menu
			// 
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.visibilityToolStripMenuItem,
            this.navigationToolStripMenuItem,
            this.resetAllToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.showViewPanelToolStripMenuItem});
			this.menu.Location = new System.Drawing.Point(0, 0);
			this.menu.Name = "menu";
			this.menu.Size = new System.Drawing.Size(1008, 24);
			this.menu.TabIndex = 8;
			this.menu.Text = "menu";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bottomToolStripMenuItem,
            this.topToolStripMenuItem,
            this.backToolStripMenuItem,
            this.frontToolStripMenuItem,
            this.rightToolStripMenuItem,
            this.leftToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// bottomToolStripMenuItem
			// 
			this.bottomToolStripMenuItem.Name = "bottomToolStripMenuItem";
			this.bottomToolStripMenuItem.ShortcutKeyDisplayString = "1";
			this.bottomToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.bottomToolStripMenuItem.Text = "Bottom";
			this.bottomToolStripMenuItem.Click += new System.EventHandler(this.bottomToolStripMenuItem_Click);
			// 
			// topToolStripMenuItem
			// 
			this.topToolStripMenuItem.Name = "topToolStripMenuItem";
			this.topToolStripMenuItem.ShortcutKeyDisplayString = "2";
			this.topToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.topToolStripMenuItem.Text = "Top";
			this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
			// 
			// backToolStripMenuItem
			// 
			this.backToolStripMenuItem.Name = "backToolStripMenuItem";
			this.backToolStripMenuItem.ShortcutKeyDisplayString = "3";
			this.backToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.backToolStripMenuItem.Text = "Back";
			this.backToolStripMenuItem.Click += new System.EventHandler(this.backToolStripMenuItem_Click);
			// 
			// frontToolStripMenuItem
			// 
			this.frontToolStripMenuItem.Name = "frontToolStripMenuItem";
			this.frontToolStripMenuItem.ShortcutKeyDisplayString = "4";
			this.frontToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.frontToolStripMenuItem.Text = "Front";
			this.frontToolStripMenuItem.Click += new System.EventHandler(this.frontToolStripMenuItem_Click);
			// 
			// rightToolStripMenuItem
			// 
			this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
			this.rightToolStripMenuItem.ShortcutKeyDisplayString = "5";
			this.rightToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.rightToolStripMenuItem.Text = "Right";
			this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
			// 
			// leftToolStripMenuItem
			// 
			this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
			this.leftToolStripMenuItem.ShortcutKeyDisplayString = "6";
			this.leftToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.leftToolStripMenuItem.Text = "Left";
			this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
			// 
			// visibilityToolStripMenuItem
			// 
			this.visibilityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundToolStripMenuItem,
            this.hardpointSizeToolStripMenuItem,
            this.zoomToolStripMenuItem});
			this.visibilityToolStripMenuItem.Name = "visibilityToolStripMenuItem";
			this.visibilityToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
			this.visibilityToolStripMenuItem.Text = "Visibility";
			// 
			// backgroundToolStripMenuItem
			// 
			this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackToolStripMenuItem,
            this.whiteToolStripMenuItem});
			this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
			this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.backgroundToolStripMenuItem.Text = "Background";
			// 
			// blackToolStripMenuItem
			// 
			this.blackToolStripMenuItem.Checked = true;
			this.blackToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
			this.blackToolStripMenuItem.ShortcutKeyDisplayString = "B";
			this.blackToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.blackToolStripMenuItem.Text = "Black";
			this.blackToolStripMenuItem.Click += new System.EventHandler(this.blackToolStripMenuItem_Click);
			// 
			// whiteToolStripMenuItem
			// 
			this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
			this.whiteToolStripMenuItem.ShortcutKeyDisplayString = "B";
			this.whiteToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
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
			this.hardpointSizeToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.hardpointSizeToolStripMenuItem.Text = "Hardpoint Size";
			// 
			// decreaseToolStripMenuItem1
			// 
			this.decreaseToolStripMenuItem1.Name = "decreaseToolStripMenuItem1";
			this.decreaseToolStripMenuItem1.ShortcutKeyDisplayString = "/";
			this.decreaseToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.decreaseToolStripMenuItem1.Text = "Decrease";
			this.decreaseToolStripMenuItem1.Click += new System.EventHandler(this.decreaseToolStripMenuItem1_Click);
			// 
			// increaseToolStripMenuItem1
			// 
			this.increaseToolStripMenuItem1.Name = "increaseToolStripMenuItem1";
			this.increaseToolStripMenuItem1.ShortcutKeyDisplayString = "*";
			this.increaseToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.increaseToolStripMenuItem1.Text = "Increase";
			this.increaseToolStripMenuItem1.Click += new System.EventHandler(this.increaseToolStripMenuItem1_Click);
			// 
			// toolStripHardpointSizeSet
			// 
			this.toolStripHardpointSizeSet.Name = "toolStripHardpointSizeSet";
			this.toolStripHardpointSizeSet.Size = new System.Drawing.Size(100, 23);
			this.toolStripHardpointSizeSet.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolStripHardpointSizeSet.TextChanged += new System.EventHandler(this.toolStripHardpointSizeSet_TextChanged);
			// 
			// zoomToolStripMenuItem
			// 
			this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inToolStripMenuItem,
            this.outToolStripMenuItem});
			this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			this.zoomToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.zoomToolStripMenuItem.Text = "Zoom";
			// 
			// inToolStripMenuItem
			// 
			this.inToolStripMenuItem.Name = "inToolStripMenuItem";
			this.inToolStripMenuItem.ShortcutKeyDisplayString = "+";
			this.inToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
			this.inToolStripMenuItem.Text = "In";
			this.inToolStripMenuItem.Click += new System.EventHandler(this.inToolStripMenuItem_Click);
			// 
			// outToolStripMenuItem
			// 
			this.outToolStripMenuItem.Name = "outToolStripMenuItem";
			this.outToolStripMenuItem.ShortcutKeyDisplayString = "-";
			this.outToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
			this.outToolStripMenuItem.Text = "Out";
			this.outToolStripMenuItem.Click += new System.EventHandler(this.outToolStripMenuItem_Click);
			// 
			// navigationToolStripMenuItem
			// 
			this.navigationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToolStripMenuItem,
            this.rotateToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.centerOnHardpointToolStripMenuItem});
			this.navigationToolStripMenuItem.Name = "navigationToolStripMenuItem";
			this.navigationToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
			this.navigationToolStripMenuItem.Text = "Navigation";
			// 
			// moveToolStripMenuItem
			// 
			this.moveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftToolStripMenuItem1,
            this.leftfineToolStripMenuItem,
            this.rightToolStripMenuItem1,
            this.rightfineToolStripMenuItem,
            this.upToolStripMenuItem,
            this.upfineToolStripMenuItem,
            this.downToolStripMenuItem,
            this.downfineToolStripMenuItem});
			this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
			this.moveToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.moveToolStripMenuItem.Text = "Move";
			// 
			// leftToolStripMenuItem1
			// 
			this.leftToolStripMenuItem1.Name = "leftToolStripMenuItem1";
			this.leftToolStripMenuItem1.ShortcutKeyDisplayString = "Arrow Left";
			this.leftToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
			this.leftToolStripMenuItem1.Text = "Left";
			this.leftToolStripMenuItem1.Click += new System.EventHandler(this.leftToolStripMenuItem1_Click);
			// 
			// leftfineToolStripMenuItem
			// 
			this.leftfineToolStripMenuItem.Name = "leftfineToolStripMenuItem";
			this.leftfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Left";
			this.leftfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.leftfineToolStripMenuItem.Text = "Left (fine)";
			this.leftfineToolStripMenuItem.Click += new System.EventHandler(this.leftfineToolStripMenuItem_Click);
			// 
			// rightToolStripMenuItem1
			// 
			this.rightToolStripMenuItem1.Name = "rightToolStripMenuItem1";
			this.rightToolStripMenuItem1.ShortcutKeyDisplayString = "Arrow Right";
			this.rightToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
			this.rightToolStripMenuItem1.Text = "Right";
			this.rightToolStripMenuItem1.Click += new System.EventHandler(this.rightToolStripMenuItem1_Click);
			// 
			// rightfineToolStripMenuItem
			// 
			this.rightfineToolStripMenuItem.Name = "rightfineToolStripMenuItem";
			this.rightfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Right";
			this.rightfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.rightfineToolStripMenuItem.Text = "Right (fine)";
			this.rightfineToolStripMenuItem.Click += new System.EventHandler(this.rightfineToolStripMenuItem_Click);
			// 
			// upToolStripMenuItem
			// 
			this.upToolStripMenuItem.Name = "upToolStripMenuItem";
			this.upToolStripMenuItem.ShortcutKeyDisplayString = "Arrow Up";
			this.upToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.upToolStripMenuItem.Text = "Up";
			this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
			// 
			// upfineToolStripMenuItem
			// 
			this.upfineToolStripMenuItem.Name = "upfineToolStripMenuItem";
			this.upfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Up";
			this.upfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.upfineToolStripMenuItem.Text = "Up (fine)";
			this.upfineToolStripMenuItem.Click += new System.EventHandler(this.upfineToolStripMenuItem_Click);
			// 
			// downToolStripMenuItem
			// 
			this.downToolStripMenuItem.Name = "downToolStripMenuItem";
			this.downToolStripMenuItem.ShortcutKeyDisplayString = "Arrow Down";
			this.downToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.downToolStripMenuItem.Text = "Down";
			this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
			// 
			// downfineToolStripMenuItem
			// 
			this.downfineToolStripMenuItem.Name = "downfineToolStripMenuItem";
			this.downfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Down";
			this.downfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.downfineToolStripMenuItem.Text = "Down (fine)";
			this.downfineToolStripMenuItem.Click += new System.EventHandler(this.downfineToolStripMenuItem_Click);
			// 
			// rotateToolStripMenuItem
			// 
			this.rotateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.anticlockwiseYaxisToolStripMenuItem,
            this.anticlockwiseYaxisToolStripMenuItem1,
            this.clockwiseYaxisToolStripMenuItem,
            this.clockwiseYaxisToolStripMenuItem1,
            this.anticlockwiseXaxisToolStripMenuItem,
            this.anticlockwiseXaxisToolStripMenuItem1,
            this.clockwiseXaxisToolStripMenuItem,
            this.clockwiseXaxisToolStripMenuItem1,
            this.anticlockwiseZaxisToolStripMenuItem,
            this.anticlockwiseZaxisToolStripMenuItem1,
            this.clockwiseZaxisToolStripMenuItem,
            this.clockwiseZaxisToolStripMenuItem1});
			this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
			this.rotateToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.rotateToolStripMenuItem.Text = "Rotate";
			// 
			// anticlockwiseYaxisToolStripMenuItem
			// 
			this.anticlockwiseYaxisToolStripMenuItem.Name = "anticlockwiseYaxisToolStripMenuItem";
			this.anticlockwiseYaxisToolStripMenuItem.ShortcutKeyDisplayString = "Pg Up";
			this.anticlockwiseYaxisToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.anticlockwiseYaxisToolStripMenuItem.Text = "15° anti-clockwise Y-axis";
			this.anticlockwiseYaxisToolStripMenuItem.Click += new System.EventHandler(this.anticlockwiseYaxisToolStripMenuItem_Click);
			// 
			// anticlockwiseYaxisToolStripMenuItem1
			// 
			this.anticlockwiseYaxisToolStripMenuItem1.Name = "anticlockwiseYaxisToolStripMenuItem1";
			this.anticlockwiseYaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Pg Up";
			this.anticlockwiseYaxisToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
			this.anticlockwiseYaxisToolStripMenuItem1.Text = "1° anti-clockwise Y-axis";
			this.anticlockwiseYaxisToolStripMenuItem1.Click += new System.EventHandler(this.anticlockwiseYaxisToolStripMenuItem1_Click);
			// 
			// clockwiseYaxisToolStripMenuItem
			// 
			this.clockwiseYaxisToolStripMenuItem.Name = "clockwiseYaxisToolStripMenuItem";
			this.clockwiseYaxisToolStripMenuItem.ShortcutKeyDisplayString = "Pg Dwn";
			this.clockwiseYaxisToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.clockwiseYaxisToolStripMenuItem.Text = "15° clockwise Y-axis";
			this.clockwiseYaxisToolStripMenuItem.Click += new System.EventHandler(this.clockwiseYaxisToolStripMenuItem_Click);
			// 
			// clockwiseYaxisToolStripMenuItem1
			// 
			this.clockwiseYaxisToolStripMenuItem1.Name = "clockwiseYaxisToolStripMenuItem1";
			this.clockwiseYaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Pg Dwn";
			this.clockwiseYaxisToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
			this.clockwiseYaxisToolStripMenuItem1.Text = "1° clockwise Y-axis";
			this.clockwiseYaxisToolStripMenuItem1.Click += new System.EventHandler(this.clockwiseYaxisToolStripMenuItem1_Click);
			// 
			// anticlockwiseXaxisToolStripMenuItem
			// 
			this.anticlockwiseXaxisToolStripMenuItem.Name = "anticlockwiseXaxisToolStripMenuItem";
			this.anticlockwiseXaxisToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Pg Up";
			this.anticlockwiseXaxisToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.anticlockwiseXaxisToolStripMenuItem.Text = "15° anti-clockwise X-axis";
			this.anticlockwiseXaxisToolStripMenuItem.Click += new System.EventHandler(this.anticlockwiseXaxisToolStripMenuItem_Click);
			// 
			// anticlockwiseXaxisToolStripMenuItem1
			// 
			this.anticlockwiseXaxisToolStripMenuItem1.Name = "anticlockwiseXaxisToolStripMenuItem1";
			this.anticlockwiseXaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Ctrl+Pg Up";
			this.anticlockwiseXaxisToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
			this.anticlockwiseXaxisToolStripMenuItem1.Text = "1° anti-clockwise X-axis";
			this.anticlockwiseXaxisToolStripMenuItem1.Click += new System.EventHandler(this.anticlockwiseXaxisToolStripMenuItem1_Click);
			// 
			// clockwiseXaxisToolStripMenuItem
			// 
			this.clockwiseXaxisToolStripMenuItem.Name = "clockwiseXaxisToolStripMenuItem";
			this.clockwiseXaxisToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Pg Dwn";
			this.clockwiseXaxisToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.clockwiseXaxisToolStripMenuItem.Text = "15° clockwise X-axis";
			this.clockwiseXaxisToolStripMenuItem.Click += new System.EventHandler(this.clockwiseXaxisToolStripMenuItem_Click);
			// 
			// clockwiseXaxisToolStripMenuItem1
			// 
			this.clockwiseXaxisToolStripMenuItem1.Name = "clockwiseXaxisToolStripMenuItem1";
			this.clockwiseXaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Ctrl+Pg Dwn";
			this.clockwiseXaxisToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
			this.clockwiseXaxisToolStripMenuItem1.Text = "1° clockwise X-axis";
			this.clockwiseXaxisToolStripMenuItem1.Click += new System.EventHandler(this.clockwiseXaxisToolStripMenuItem1_Click);
			// 
			// anticlockwiseZaxisToolStripMenuItem
			// 
			this.anticlockwiseZaxisToolStripMenuItem.Name = "anticlockwiseZaxisToolStripMenuItem";
			this.anticlockwiseZaxisToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Pg Up";
			this.anticlockwiseZaxisToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.anticlockwiseZaxisToolStripMenuItem.Text = "15° anti-clockwise Z-axis";
			this.anticlockwiseZaxisToolStripMenuItem.Click += new System.EventHandler(this.anticlockwiseZaxisToolStripMenuItem_Click);
			// 
			// anticlockwiseZaxisToolStripMenuItem1
			// 
			this.anticlockwiseZaxisToolStripMenuItem1.Name = "anticlockwiseZaxisToolStripMenuItem1";
			this.anticlockwiseZaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Alt+Pg Up";
			this.anticlockwiseZaxisToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
			this.anticlockwiseZaxisToolStripMenuItem1.Text = "1° anti-clockwise Z-axis";
			this.anticlockwiseZaxisToolStripMenuItem1.Click += new System.EventHandler(this.anticlockwiseZaxisToolStripMenuItem1_Click);
			// 
			// clockwiseZaxisToolStripMenuItem
			// 
			this.clockwiseZaxisToolStripMenuItem.Name = "clockwiseZaxisToolStripMenuItem";
			this.clockwiseZaxisToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Pg Dwn";
			this.clockwiseZaxisToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.clockwiseZaxisToolStripMenuItem.Text = "15° clockwise Z-axis";
			this.clockwiseZaxisToolStripMenuItem.Click += new System.EventHandler(this.clockwiseZaxisToolStripMenuItem_Click);
			// 
			// clockwiseZaxisToolStripMenuItem1
			// 
			this.clockwiseZaxisToolStripMenuItem1.Name = "clockwiseZaxisToolStripMenuItem1";
			this.clockwiseZaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Alt+Pg Dwn";
			this.clockwiseZaxisToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
			this.clockwiseZaxisToolStripMenuItem1.Text = "1° clockwise Z-axis";
			this.clockwiseZaxisToolStripMenuItem1.Click += new System.EventHandler(this.clockwiseZaxisToolStripMenuItem1_Click);
			// 
			// resetToolStripMenuItem
			// 
			this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			this.resetToolStripMenuItem.ShortcutKeyDisplayString = "Home";
			this.resetToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.resetToolStripMenuItem.Text = "Reset";
			this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
			// 
			// centerOnHardpointToolStripMenuItem
			// 
			this.centerOnHardpointToolStripMenuItem.Name = "centerOnHardpointToolStripMenuItem";
			this.centerOnHardpointToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Home";
			this.centerOnHardpointToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.centerOnHardpointToolStripMenuItem.Text = "Center on Hardpoint";
			this.centerOnHardpointToolStripMenuItem.Click += new System.EventHandler(this.centerOnHardpointToolStripMenuItem_Click);
			// 
			// resetAllToolStripMenuItem
			// 
			this.resetAllToolStripMenuItem.Name = "resetAllToolStripMenuItem";
			this.resetAllToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
			this.resetAllToolStripMenuItem.Text = "Reset All";
			this.resetAllToolStripMenuItem.Click += new System.EventHandler(this.resetAllToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortcutsToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// shortcutsToolStripMenuItem
			// 
			this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
			this.shortcutsToolStripMenuItem.ShortcutKeyDisplayString = "F1";
			this.shortcutsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.shortcutsToolStripMenuItem.Text = "Shortcuts...";
			this.shortcutsToolStripMenuItem.Click += new System.EventHandler(this.shortcutsToolStripMenuItem_Click);
			// 
			// showViewPanelToolStripMenuItem
			// 
			this.showViewPanelToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.showViewPanelToolStripMenuItem.Name = "showViewPanelToolStripMenuItem";
			this.showViewPanelToolStripMenuItem.Size = new System.Drawing.Size(116, 20);
			this.showViewPanelToolStripMenuItem.Text = "Toggle View Panel";
			this.showViewPanelToolStripMenuItem.Click += new System.EventHandler(this.showViewPanelToolStripMenuItem_Click);
			// 
			// lblHardpointHover
			// 
			this.lblHardpointHover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblHardpointHover.Location = new System.Drawing.Point(618, 10);
			this.lblHardpointHover.Name = "lblHardpointHover";
			this.lblHardpointHover.Size = new System.Drawing.Size(383, 23);
			this.lblHardpointHover.TabIndex = 4;
			this.lblHardpointHover.Text = "Hardpoint: <none>";
			this.lblHardpointHover.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ModelViewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 730);
			this.Controls.Add(this.modelView);
			this.Controls.Add(this.toolStripContainer1);
			this.Controls.Add(this.menu);
			this.Name = "ModelViewForm";
			this.ShowIcon = false;
			this.Text = "Model View";
			this.Load += new System.EventHandler(this.ModelViewForm_Load);
			this.Activated += new System.EventHandler(this.ModelViewForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelViewForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.modelView.Panel2.ResumeLayout(false);
			this.modelView.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.viewPanelView)).EndInit();
			this.menu.ResumeLayout(false);
			this.menu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxScale;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
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
		private System.Windows.Forms.ToolStripMenuItem navigationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem leftfineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem rightfineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem upfineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem downToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem downfineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem anticlockwiseYaxisToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem anticlockwiseYaxisToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem clockwiseYaxisToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clockwiseYaxisToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem anticlockwiseXaxisToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem anticlockwiseXaxisToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem clockwiseXaxisToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clockwiseXaxisToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem anticlockwiseZaxisToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem anticlockwiseZaxisToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem clockwiseZaxisToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clockwiseZaxisToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem centerOnHardpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shortcutsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showViewPanelToolStripMenuItem;
		private System.Windows.Forms.ColorDialog colorDiag;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colMPVisible;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMPElement;
		private System.Windows.Forms.DataGridViewComboBoxColumn colMPShading;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMPColor;
		private System.Windows.Forms.DataGridViewComboBoxColumn colMPTexture;
		private System.Windows.Forms.Label lblHardpointHover;
    }
}