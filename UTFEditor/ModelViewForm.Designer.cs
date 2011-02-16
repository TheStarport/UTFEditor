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
			this.buttonCenter = new System.Windows.Forms.Button();
			this.spinnerLevel = new System.Windows.Forms.NumericUpDown();
			this.labelLevel = new System.Windows.Forms.Label();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.modelView = new System.Windows.Forms.SplitContainer();
			this.menu = new System.Windows.Forms.MenuStrip();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.frontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.brightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.minimumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.maximumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripBrightnessSet = new System.Windows.Forms.ToolStripTextBox();
			this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.solidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.centerOnHardpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLevel)).BeginInit();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.modelView.Panel1.SuspendLayout();
			this.modelView.SuspendLayout();
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
			this.trackBarScale.Size = new System.Drawing.Size(188, 25);
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
			this.textBoxScale.Location = new System.Drawing.Point(246, 12);
			this.textBoxScale.MaxLength = 8;
			this.textBoxScale.Name = "textBoxScale";
			this.textBoxScale.Size = new System.Drawing.Size(54, 20);
			this.textBoxScale.TabIndex = 3;
			this.textBoxScale.Text = "20";
			this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
			// 
			// buttonCenter
			// 
			this.buttonCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCenter.AutoSize = true;
			this.buttonCenter.Location = new System.Drawing.Point(531, 10);
			this.buttonCenter.Name = "buttonCenter";
			this.buttonCenter.Size = new System.Drawing.Size(81, 23);
			this.buttonCenter.TabIndex = 7;
			this.buttonCenter.Text = "Center on HP";
			this.buttonCenter.UseVisualStyleBackColor = true;
			this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
			// 
			// spinnerLevel
			// 
			this.spinnerLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.spinnerLevel.Location = new System.Drawing.Point(373, 13);
			this.spinnerLevel.Name = "spinnerLevel";
			this.spinnerLevel.Size = new System.Drawing.Size(54, 20);
			this.spinnerLevel.TabIndex = 5;
			this.spinnerLevel.ValueChanged += new System.EventHandler(this.spinnerLevel_ValueChanged);
			// 
			// labelLevel
			// 
			this.labelLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelLevel.AutoSize = true;
			this.labelLevel.Location = new System.Drawing.Point(334, 15);
			this.labelLevel.Name = "labelLevel";
			this.labelLevel.Size = new System.Drawing.Size(33, 13);
			this.labelLevel.TabIndex = 4;
			this.labelLevel.Text = "Level";
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.spinnerLevel);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.textBoxScale);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.labelLevel);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.trackBarScale);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.buttonCenter);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(624, 43);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 519);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(624, 43);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			this.toolStripContainer1.TopToolStripPanelVisible = false;
			// 
			// modelView
			// 
			this.modelView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelView.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.modelView.IsSplitterFixed = true;
			this.modelView.Location = new System.Drawing.Point(0, 0);
			this.modelView.Name = "modelView";
			this.modelView.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// modelView.Panel1
			// 
			this.modelView.Panel1.BackColor = System.Drawing.Color.Black;
			this.modelView.Panel1.Controls.Add(this.menu);
			this.modelView.Panel1.Margin = new System.Windows.Forms.Padding(0, 24, 0, 0);
			this.modelView.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.modelView_Paint);
			this.modelView.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseMove);
			this.modelView.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseClick);
			this.modelView.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseDown);
			this.modelView.Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelView_Panel1_MouseUp);
			this.modelView.Panel2Collapsed = true;
			this.modelView.Panel2MinSize = 0;
			this.modelView.Size = new System.Drawing.Size(624, 519);
			this.modelView.SplitterDistance = 450;
			this.modelView.SplitterWidth = 1;
			this.modelView.TabIndex = 0;
			this.modelView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.modelView_KeyDown);
			// 
			// menu
			// 
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.visibilityToolStripMenuItem,
            this.navigationToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menu.Location = new System.Drawing.Point(0, 0);
			this.menu.Name = "menu";
			this.menu.Size = new System.Drawing.Size(624, 24);
			this.menu.TabIndex = 0;
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
            this.brightnessToolStripMenuItem,
            this.backgroundToolStripMenuItem,
            this.shadingToolStripMenuItem,
            this.hardpointSizeToolStripMenuItem,
            this.zoomToolStripMenuItem});
			this.visibilityToolStripMenuItem.Name = "visibilityToolStripMenuItem";
			this.visibilityToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
			this.visibilityToolStripMenuItem.Text = "Visibility";
			// 
			// brightnessToolStripMenuItem
			// 
			this.brightnessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minimumToolStripMenuItem,
            this.decreaseToolStripMenuItem,
            this.increaseToolStripMenuItem,
            this.maximumToolStripMenuItem,
            this.toolStripBrightnessSet});
			this.brightnessToolStripMenuItem.Name = "brightnessToolStripMenuItem";
			this.brightnessToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.brightnessToolStripMenuItem.Text = "Brightness";
			// 
			// minimumToolStripMenuItem
			// 
			this.minimumToolStripMenuItem.Name = "minimumToolStripMenuItem";
			this.minimumToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Z";
			this.minimumToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.minimumToolStripMenuItem.Text = "Minimum";
			this.minimumToolStripMenuItem.Click += new System.EventHandler(this.minimumToolStripMenuItem_Click);
			// 
			// decreaseToolStripMenuItem
			// 
			this.decreaseToolStripMenuItem.Name = "decreaseToolStripMenuItem";
			this.decreaseToolStripMenuItem.ShortcutKeyDisplayString = "Z";
			this.decreaseToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.decreaseToolStripMenuItem.Text = "Decrease";
			this.decreaseToolStripMenuItem.Click += new System.EventHandler(this.decreaseToolStripMenuItem_Click);
			// 
			// increaseToolStripMenuItem
			// 
			this.increaseToolStripMenuItem.Name = "increaseToolStripMenuItem";
			this.increaseToolStripMenuItem.ShortcutKeyDisplayString = "A";
			this.increaseToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.increaseToolStripMenuItem.Text = "Increase";
			this.increaseToolStripMenuItem.Click += new System.EventHandler(this.increaseToolStripMenuItem_Click);
			// 
			// maximumToolStripMenuItem
			// 
			this.maximumToolStripMenuItem.Name = "maximumToolStripMenuItem";
			this.maximumToolStripMenuItem.ShortcutKeyDisplayString = "Shift+A";
			this.maximumToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.maximumToolStripMenuItem.Text = "Maximum";
			this.maximumToolStripMenuItem.Click += new System.EventHandler(this.maximumToolStripMenuItem_Click);
			// 
			// toolStripBrightnessSet
			// 
			this.toolStripBrightnessSet.Name = "toolStripBrightnessSet";
			this.toolStripBrightnessSet.Size = new System.Drawing.Size(100, 23);
			this.toolStripBrightnessSet.Text = "0.0";
			this.toolStripBrightnessSet.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolStripBrightnessSet.ToolTipText = "Brightness (0 to 1)";
			this.toolStripBrightnessSet.TextChanged += new System.EventHandler(this.toolStripBrightnessSet_TextChanged);
			// 
			// backgroundToolStripMenuItem
			// 
			this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackToolStripMenuItem,
            this.whiteToolStripMenuItem});
			this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
			this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
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
			// 
			// whiteToolStripMenuItem
			// 
			this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
			this.whiteToolStripMenuItem.ShortcutKeyDisplayString = "B";
			this.whiteToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.whiteToolStripMenuItem.Text = "White";
			// 
			// shadingToolStripMenuItem
			// 
			this.shadingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wireframeToolStripMenuItem,
            this.solidToolStripMenuItem});
			this.shadingToolStripMenuItem.Name = "shadingToolStripMenuItem";
			this.shadingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.shadingToolStripMenuItem.Text = "Shading";
			// 
			// wireframeToolStripMenuItem
			// 
			this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
			this.wireframeToolStripMenuItem.ShortcutKeyDisplayString = "W";
			this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.wireframeToolStripMenuItem.Text = "Wireframe";
			// 
			// solidToolStripMenuItem
			// 
			this.solidToolStripMenuItem.Checked = true;
			this.solidToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.solidToolStripMenuItem.Name = "solidToolStripMenuItem";
			this.solidToolStripMenuItem.ShortcutKeyDisplayString = "W";
			this.solidToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.solidToolStripMenuItem.Text = "Solid";
			// 
			// hardpointSizeToolStripMenuItem
			// 
			this.hardpointSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decreaseToolStripMenuItem1,
            this.increaseToolStripMenuItem1,
            this.toolStripHardpointSizeSet});
			this.hardpointSizeToolStripMenuItem.Name = "hardpointSizeToolStripMenuItem";
			this.hardpointSizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.hardpointSizeToolStripMenuItem.Text = "Hardpoint Size";
			// 
			// decreaseToolStripMenuItem1
			// 
			this.decreaseToolStripMenuItem1.Name = "decreaseToolStripMenuItem1";
			this.decreaseToolStripMenuItem1.ShortcutKeyDisplayString = "/";
			this.decreaseToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.decreaseToolStripMenuItem1.Text = "Decrease";
			// 
			// increaseToolStripMenuItem1
			// 
			this.increaseToolStripMenuItem1.Name = "increaseToolStripMenuItem1";
			this.increaseToolStripMenuItem1.ShortcutKeyDisplayString = "*";
			this.increaseToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.increaseToolStripMenuItem1.Text = "Increase";
			// 
			// toolStripHardpointSizeSet
			// 
			this.toolStripHardpointSizeSet.Name = "toolStripHardpointSizeSet";
			this.toolStripHardpointSizeSet.Size = new System.Drawing.Size(100, 23);
			// 
			// zoomToolStripMenuItem
			// 
			this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inToolStripMenuItem,
            this.outToolStripMenuItem});
			this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			this.zoomToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.zoomToolStripMenuItem.Text = "Zoom";
			// 
			// inToolStripMenuItem
			// 
			this.inToolStripMenuItem.Name = "inToolStripMenuItem";
			this.inToolStripMenuItem.ShortcutKeyDisplayString = "+";
			this.inToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
			this.inToolStripMenuItem.Text = "In";
			// 
			// outToolStripMenuItem
			// 
			this.outToolStripMenuItem.Name = "outToolStripMenuItem";
			this.outToolStripMenuItem.ShortcutKeyDisplayString = "-";
			this.outToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
			this.outToolStripMenuItem.Text = "Out";
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
			// 
			// leftfineToolStripMenuItem
			// 
			this.leftfineToolStripMenuItem.Name = "leftfineToolStripMenuItem";
			this.leftfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Left";
			this.leftfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.leftfineToolStripMenuItem.Text = "Left (fine)";
			// 
			// rightToolStripMenuItem1
			// 
			this.rightToolStripMenuItem1.Name = "rightToolStripMenuItem1";
			this.rightToolStripMenuItem1.ShortcutKeyDisplayString = "Arrow Right";
			this.rightToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
			this.rightToolStripMenuItem1.Text = "Right";
			// 
			// rightfineToolStripMenuItem
			// 
			this.rightfineToolStripMenuItem.Name = "rightfineToolStripMenuItem";
			this.rightfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Right";
			this.rightfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.rightfineToolStripMenuItem.Text = "Right (fine)";
			// 
			// upToolStripMenuItem
			// 
			this.upToolStripMenuItem.Name = "upToolStripMenuItem";
			this.upToolStripMenuItem.ShortcutKeyDisplayString = "Arrow Up";
			this.upToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.upToolStripMenuItem.Text = "Up";
			// 
			// upfineToolStripMenuItem
			// 
			this.upfineToolStripMenuItem.Name = "upfineToolStripMenuItem";
			this.upfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Up";
			this.upfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.upfineToolStripMenuItem.Text = "Up (fine)";
			// 
			// downToolStripMenuItem
			// 
			this.downToolStripMenuItem.Name = "downToolStripMenuItem";
			this.downToolStripMenuItem.ShortcutKeyDisplayString = "Arrow Down";
			this.downToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.downToolStripMenuItem.Text = "Down";
			// 
			// downfineToolStripMenuItem
			// 
			this.downfineToolStripMenuItem.Name = "downfineToolStripMenuItem";
			this.downfineToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Arrow Down";
			this.downfineToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.downfineToolStripMenuItem.Text = "Down (fine)";
			// 
			// rotateToolStripMenuItem
			// 
			this.rotateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.anticlockwiseYaxisToolStripMenuItem,
            this.anticlockwiseYaxisToolStripMenuItem1,
            this.clockwiseYaxisToolStripMenuItem,
            this.clockwiseYaxisToolStripMenuItem1});
			this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
			this.rotateToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.rotateToolStripMenuItem.Text = "Rotate";
			// 
			// anticlockwiseYaxisToolStripMenuItem
			// 
			this.anticlockwiseYaxisToolStripMenuItem.Name = "anticlockwiseYaxisToolStripMenuItem";
			this.anticlockwiseYaxisToolStripMenuItem.ShortcutKeyDisplayString = "Pg Up";
			this.anticlockwiseYaxisToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.anticlockwiseYaxisToolStripMenuItem.Text = "15° anti-clockwise Y-axis";
			// 
			// anticlockwiseYaxisToolStripMenuItem1
			// 
			this.anticlockwiseYaxisToolStripMenuItem1.Name = "anticlockwiseYaxisToolStripMenuItem1";
			this.anticlockwiseYaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Pg Up";
			this.anticlockwiseYaxisToolStripMenuItem1.Size = new System.Drawing.Size(269, 22);
			this.anticlockwiseYaxisToolStripMenuItem1.Text = "1° anti-clockwise Y-axis";
			// 
			// clockwiseYaxisToolStripMenuItem
			// 
			this.clockwiseYaxisToolStripMenuItem.Name = "clockwiseYaxisToolStripMenuItem";
			this.clockwiseYaxisToolStripMenuItem.ShortcutKeyDisplayString = "Pg Dwn";
			this.clockwiseYaxisToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.clockwiseYaxisToolStripMenuItem.Text = "15° clockwise Y-axis";
			// 
			// clockwiseYaxisToolStripMenuItem1
			// 
			this.clockwiseYaxisToolStripMenuItem1.Name = "clockwiseYaxisToolStripMenuItem1";
			this.clockwiseYaxisToolStripMenuItem1.ShortcutKeyDisplayString = "Shift+Pg Dwn";
			this.clockwiseYaxisToolStripMenuItem1.Size = new System.Drawing.Size(269, 22);
			this.clockwiseYaxisToolStripMenuItem1.Text = "1° clockwise Y-axis";
			// 
			// resetToolStripMenuItem
			// 
			this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			this.resetToolStripMenuItem.ShortcutKeyDisplayString = "Home";
			this.resetToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.resetToolStripMenuItem.Text = "Reset";
			// 
			// centerOnHardpointToolStripMenuItem
			// 
			this.centerOnHardpointToolStripMenuItem.Name = "centerOnHardpointToolStripMenuItem";
			this.centerOnHardpointToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Home";
			this.centerOnHardpointToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.centerOnHardpointToolStripMenuItem.Text = "Center on Hardpoint";
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
			// 
			// ModelViewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 562);
			this.Controls.Add(this.modelView);
			this.Controls.Add(this.toolStripContainer1);
			this.MainMenuStrip = this.menu;
			this.Name = "ModelViewForm";
			this.ShowIcon = false;
			this.Text = "Model View";
			this.Activated += new System.EventHandler(this.ModelViewForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelViewForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLevel)).EndInit();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.modelView.Panel1.ResumeLayout(false);
			this.modelView.Panel1.PerformLayout();
			this.modelView.ResumeLayout(false);
			this.menu.ResumeLayout(false);
			this.menu.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.NumericUpDown spinnerLevel;
        private System.Windows.Forms.Label labelLevel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer modelView;
		private System.Windows.Forms.MenuStrip menu;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bottomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem backToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem frontToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visibilityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem brightnessToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem minimumToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decreaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem increaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maximumToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox toolStripBrightnessSet;
		private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hardpointSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decreaseToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem increaseToolStripMenuItem1;
		private System.Windows.Forms.ToolStripTextBox toolStripHardpointSizeSet;
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
		private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shortcutsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shadingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem solidToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem centerOnHardpointToolStripMenuItem;
    }
}