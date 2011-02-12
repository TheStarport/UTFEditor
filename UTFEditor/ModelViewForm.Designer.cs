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
			this.checkBoxSolid = new System.Windows.Forms.CheckBox();
			this.trackBarScale = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxScale = new System.Windows.Forms.TextBox();
			this.buttonReset = new System.Windows.Forms.Button();
			this.buttonCenter = new System.Windows.Forms.Button();
			this.spinnerLevel = new System.Windows.Forms.NumericUpDown();
			this.labelLevel = new System.Windows.Forms.Label();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.modelView = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLevel)).BeginInit();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.modelView.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkBoxSolid
			// 
			this.checkBoxSolid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxSolid.AutoSize = true;
			this.checkBoxSolid.Checked = true;
			this.checkBoxSolid.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSolid.Location = new System.Drawing.Point(12, 14);
			this.checkBoxSolid.Name = "checkBoxSolid";
			this.checkBoxSolid.Size = new System.Drawing.Size(49, 17);
			this.checkBoxSolid.TabIndex = 0;
			this.checkBoxSolid.Text = "Solid";
			this.checkBoxSolid.UseVisualStyleBackColor = true;
			this.checkBoxSolid.CheckedChanged += new System.EventHandler(this.checkBoxSolid_CheckedChanged);
			// 
			// trackBarScale
			// 
			this.trackBarScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.trackBarScale.AutoSize = false;
			this.trackBarScale.Location = new System.Drawing.Point(136, 11);
			this.trackBarScale.Maximum = 300;
			this.trackBarScale.Minimum = -300;
			this.trackBarScale.Name = "trackBarScale";
			this.trackBarScale.Size = new System.Drawing.Size(104, 25);
			this.trackBarScale.TabIndex = 2;
			this.trackBarScale.Value = 130;
			this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(96, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Scale";
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
			// buttonReset
			// 
			this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonReset.AutoSize = true;
			this.buttonReset.Location = new System.Drawing.Point(442, 10);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(59, 23);
			this.buttonReset.TabIndex = 6;
			this.buttonReset.Text = "Reset";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// buttonCenter
			// 
			this.buttonCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCenter.AutoSize = true;
			this.buttonCenter.Location = new System.Drawing.Point(507, 10);
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
			this.spinnerLevel.Size = new System.Drawing.Size(30, 20);
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
			this.toolStripContainer1.ContentPanel.Controls.Add(this.checkBoxSolid);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.buttonReset);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(600, 43);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 450);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(600, 43);
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
			this.modelView.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.modelView_Paint);
			this.modelView.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseMove);
			this.modelView.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseClick);
			this.modelView.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelView_MouseDown);
			this.modelView.Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelView_Panel1_MouseUp);
			this.modelView.Panel2Collapsed = true;
			this.modelView.Panel2MinSize = 0;
			this.modelView.Size = new System.Drawing.Size(600, 450);
			this.modelView.SplitterDistance = 450;
			this.modelView.SplitterWidth = 1;
			this.modelView.TabIndex = 0;
			this.modelView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.modelView_KeyDown);
			// 
			// ModelViewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 493);
			this.Controls.Add(this.modelView);
			this.Controls.Add(this.toolStripContainer1);
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
			this.modelView.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSolid;
        private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.NumericUpDown spinnerLevel;
        private System.Windows.Forms.Label labelLevel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer modelView;
    }
}