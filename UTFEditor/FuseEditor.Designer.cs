namespace UTFEditor
{
    partial class FuseEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuseEditor));
            this.splitFuseEditor = new System.Windows.Forms.SplitContainer();
            this.btnNew = new System.Windows.Forms.Button();
            this.comboEvents = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.btnFlip = new System.Windows.Forms.Button();
            this.panelEffect = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboEffect = new System.Windows.Forms.ComboBox();
            this.chkEffectAttached = new System.Windows.Forms.CheckBox();
            this.grpEffectTimings = new System.Windows.Forms.GroupBox();
            this.btnEffectRemoveTiming = new System.Windows.Forms.Button();
            this.btnEffectAddTiming = new System.Windows.Forms.Button();
            this.lstEffectTimings = new System.Windows.Forms.ListBox();
            this.txtEffectAddTiming = new System.Windows.Forms.NumericUpDown();
            this.grpEffectHardpoints = new System.Windows.Forms.GroupBox();
            this.dataEffectHardpoints = new System.Windows.Forms.DataGridView();
            this.colEffectHardpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffectHardpointOri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffectHardpointPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEffectRemoveHardpoint = new System.Windows.Forms.Button();
            this.btnEffectAddHardpoint = new System.Windows.Forms.Button();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timeline1 = new UTFEditor.Timeline();
            this.splitFuseEditor.Panel1.SuspendLayout();
            this.splitFuseEditor.Panel2.SuspendLayout();
            this.splitFuseEditor.SuspendLayout();
            this.panelEffect.SuspendLayout();
            this.grpEffectTimings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEffectAddTiming)).BeginInit();
            this.grpEffectHardpoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataEffectHardpoints)).BeginInit();
            this.SuspendLayout();
            // 
            // splitFuseEditor
            // 
            this.splitFuseEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitFuseEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFuseEditor.Location = new System.Drawing.Point(0, 0);
            this.splitFuseEditor.Name = "splitFuseEditor";
            // 
            // splitFuseEditor.Panel1
            // 
            this.splitFuseEditor.Panel1.Controls.Add(this.btnNew);
            this.splitFuseEditor.Panel1.Controls.Add(this.comboEvents);
            this.splitFuseEditor.Panel1.Controls.Add(this.btnNext);
            this.splitFuseEditor.Panel1.Controls.Add(this.btnPrev);
            this.splitFuseEditor.Panel1.Controls.Add(this.btnPlayPause);
            this.splitFuseEditor.Panel1.Controls.Add(this.btnFlip);
            this.splitFuseEditor.Panel1.Controls.Add(this.timeline1);
            this.splitFuseEditor.Panel1MinSize = 150;
            // 
            // splitFuseEditor.Panel2
            // 
            this.splitFuseEditor.Panel2.Controls.Add(this.panelEffect);
            this.splitFuseEditor.Panel2.Controls.Add(this.comboType);
            this.splitFuseEditor.Panel2.Controls.Add(this.label1);
            this.splitFuseEditor.Size = new System.Drawing.Size(697, 706);
            this.splitFuseEditor.SplitterDistance = 200;
            this.splitFuseEditor.TabIndex = 0;
            this.splitFuseEditor.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitFuseEditor_SplitterMoved);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.Location = new System.Drawing.Point(135, 665);
            this.btnNew.Name = "btnNew";
            this.btnNew.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.btnNew.Size = new System.Drawing.Size(59, 28);
            this.btnNew.TabIndex = 13;
            this.btnNew.Text = "New";
            this.btnNew.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // comboEvents
            // 
            this.comboEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboEvents.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboEvents.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboEvents.FormattingEnabled = true;
            this.comboEvents.Location = new System.Drawing.Point(41, 14);
            this.comboEvents.Name = "comboEvents";
            this.comboEvents.Size = new System.Drawing.Size(123, 21);
            this.comboEvents.Sorted = true;
            this.comboEvents.TabIndex = 12;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(170, 11);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(24, 28);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = "»";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(11, 11);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(24, 28);
            this.btnPrev.TabIndex = 10;
            this.btnPrev.Text = "«";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlayPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlayPause.Image = global::UTFEditor.Properties.Resources.PlayHS;
            this.btnPlayPause.Location = new System.Drawing.Point(41, 665);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(24, 28);
            this.btnPlayPause.TabIndex = 9;
            this.btnPlayPause.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // btnFlip
            // 
            this.btnFlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlip.Image = ((System.Drawing.Image)(resources.GetObject("btnFlip.Image")));
            this.btnFlip.Location = new System.Drawing.Point(11, 665);
            this.btnFlip.Name = "btnFlip";
            this.btnFlip.Size = new System.Drawing.Size(24, 28);
            this.btnFlip.TabIndex = 8;
            this.btnFlip.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFlip.UseVisualStyleBackColor = true;
            this.btnFlip.Click += new System.EventHandler(this.btnFlip_Click);
            // 
            // panelEffect
            // 
            this.panelEffect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEffect.AutoScroll = true;
            this.panelEffect.Controls.Add(this.label4);
            this.panelEffect.Controls.Add(this.comboEffect);
            this.panelEffect.Controls.Add(this.chkEffectAttached);
            this.panelEffect.Controls.Add(this.grpEffectTimings);
            this.panelEffect.Controls.Add(this.grpEffectHardpoints);
            this.panelEffect.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelEffect.Location = new System.Drawing.Point(6, 35);
            this.panelEffect.Name = "panelEffect";
            this.panelEffect.Size = new System.Drawing.Size(474, 658);
            this.panelEffect.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Effect";
            // 
            // comboEffect
            // 
            this.comboEffect.FormattingEnabled = true;
            this.comboEffect.Location = new System.Drawing.Point(3, 16);
            this.comboEffect.MinimumSize = new System.Drawing.Size(250, 0);
            this.comboEffect.Name = "comboEffect";
            this.comboEffect.Size = new System.Drawing.Size(250, 21);
            this.comboEffect.TabIndex = 6;
            this.comboEffect.SelectedIndexChanged += new System.EventHandler(this.comboEffect_SelectedIndexChanged);
            this.comboEffect.TextUpdate += new System.EventHandler(this.comboEffect_TextUpdate);
            // 
            // chkEffectAttached
            // 
            this.chkEffectAttached.AutoSize = true;
            this.chkEffectAttached.Location = new System.Drawing.Point(3, 43);
            this.chkEffectAttached.MinimumSize = new System.Drawing.Size(69, 0);
            this.chkEffectAttached.Name = "chkEffectAttached";
            this.chkEffectAttached.Size = new System.Drawing.Size(69, 17);
            this.chkEffectAttached.TabIndex = 7;
            this.chkEffectAttached.Text = "Attached";
            this.chkEffectAttached.UseVisualStyleBackColor = true;
            // 
            // grpEffectTimings
            // 
            this.grpEffectTimings.Controls.Add(this.btnEffectRemoveTiming);
            this.grpEffectTimings.Controls.Add(this.btnEffectAddTiming);
            this.grpEffectTimings.Controls.Add(this.lstEffectTimings);
            this.grpEffectTimings.Controls.Add(this.txtEffectAddTiming);
            this.grpEffectTimings.Location = new System.Drawing.Point(3, 66);
            this.grpEffectTimings.MinimumSize = new System.Drawing.Size(190, 0);
            this.grpEffectTimings.Name = "grpEffectTimings";
            this.grpEffectTimings.Size = new System.Drawing.Size(190, 254);
            this.grpEffectTimings.TabIndex = 8;
            this.grpEffectTimings.TabStop = false;
            this.grpEffectTimings.Text = "Timings";
            // 
            // btnEffectRemoveTiming
            // 
            this.btnEffectRemoveTiming.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEffectRemoveTiming.Location = new System.Drawing.Point(122, 217);
            this.btnEffectRemoveTiming.Name = "btnEffectRemoveTiming";
            this.btnEffectRemoveTiming.Size = new System.Drawing.Size(62, 23);
            this.btnEffectRemoveTiming.TabIndex = 11;
            this.btnEffectRemoveTiming.Text = "Remove";
            this.btnEffectRemoveTiming.UseVisualStyleBackColor = true;
            this.btnEffectRemoveTiming.Click += new System.EventHandler(this.btnEffectRemoveTiming_Click);
            // 
            // btnEffectAddTiming
            // 
            this.btnEffectAddTiming.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEffectAddTiming.Location = new System.Drawing.Point(65, 216);
            this.btnEffectAddTiming.Name = "btnEffectAddTiming";
            this.btnEffectAddTiming.Size = new System.Drawing.Size(51, 23);
            this.btnEffectAddTiming.TabIndex = 10;
            this.btnEffectAddTiming.Text = "Add";
            this.btnEffectAddTiming.UseVisualStyleBackColor = true;
            this.btnEffectAddTiming.Click += new System.EventHandler(this.btnEffectAddTiming_Click);
            // 
            // lstEffectTimings
            // 
            this.lstEffectTimings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEffectTimings.FormattingEnabled = true;
            this.lstEffectTimings.Location = new System.Drawing.Point(6, 14);
            this.lstEffectTimings.Name = "lstEffectTimings";
            this.lstEffectTimings.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstEffectTimings.Size = new System.Drawing.Size(178, 199);
            this.lstEffectTimings.Sorted = true;
            this.lstEffectTimings.TabIndex = 9;
            this.lstEffectTimings.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstEffectTimings_KeyDown);
            // 
            // txtEffectAddTiming
            // 
            this.txtEffectAddTiming.DecimalPlaces = 2;
            this.txtEffectAddTiming.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txtEffectAddTiming.Location = new System.Drawing.Point(6, 219);
            this.txtEffectAddTiming.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtEffectAddTiming.Name = "txtEffectAddTiming";
            this.txtEffectAddTiming.Size = new System.Drawing.Size(53, 20);
            this.txtEffectAddTiming.TabIndex = 12;
            this.txtEffectAddTiming.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEffectAddTiming_KeyDown);
            // 
            // grpEffectHardpoints
            // 
            this.grpEffectHardpoints.Controls.Add(this.dataEffectHardpoints);
            this.grpEffectHardpoints.Controls.Add(this.btnEffectRemoveHardpoint);
            this.grpEffectHardpoints.Controls.Add(this.btnEffectAddHardpoint);
            this.grpEffectHardpoints.Location = new System.Drawing.Point(3, 326);
            this.grpEffectHardpoints.MinimumSize = new System.Drawing.Size(337, 0);
            this.grpEffectHardpoints.Name = "grpEffectHardpoints";
            this.grpEffectHardpoints.Size = new System.Drawing.Size(337, 309);
            this.grpEffectHardpoints.TabIndex = 9;
            this.grpEffectHardpoints.TabStop = false;
            this.grpEffectHardpoints.Text = "Hardpoints";
            // 
            // dataEffectHardpoints
            // 
            this.dataEffectHardpoints.AllowUserToAddRows = false;
            this.dataEffectHardpoints.AllowUserToDeleteRows = false;
            this.dataEffectHardpoints.AllowUserToResizeRows = false;
            this.dataEffectHardpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataEffectHardpoints.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataEffectHardpoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataEffectHardpoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEffectHardpoint,
            this.colEffectHardpointOri,
            this.colEffectHardpointPos});
            this.dataEffectHardpoints.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataEffectHardpoints.Location = new System.Drawing.Point(6, 19);
            this.dataEffectHardpoints.Name = "dataEffectHardpoints";
            this.dataEffectHardpoints.RowHeadersVisible = false;
            this.dataEffectHardpoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataEffectHardpoints.ShowCellErrors = false;
            this.dataEffectHardpoints.ShowCellToolTips = false;
            this.dataEffectHardpoints.ShowEditingIcon = false;
            this.dataEffectHardpoints.ShowRowErrors = false;
            this.dataEffectHardpoints.Size = new System.Drawing.Size(325, 255);
            this.dataEffectHardpoints.TabIndex = 7;
            // 
            // colEffectHardpoint
            // 
            this.colEffectHardpoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEffectHardpoint.HeaderText = "Hardpoint";
            this.colEffectHardpoint.Name = "colEffectHardpoint";
            this.colEffectHardpoint.ReadOnly = true;
            // 
            // colEffectHardpointOri
            // 
            this.colEffectHardpointOri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEffectHardpointOri.HeaderText = "Orientation Offset";
            this.colEffectHardpointOri.Name = "colEffectHardpointOri";
            this.colEffectHardpointOri.Width = 120;
            // 
            // colEffectHardpointPos
            // 
            this.colEffectHardpointPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEffectHardpointPos.HeaderText = "Position Offset";
            this.colEffectHardpointPos.Name = "colEffectHardpointPos";
            this.colEffectHardpointPos.Width = 120;
            // 
            // btnEffectRemoveHardpoint
            // 
            this.btnEffectRemoveHardpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEffectRemoveHardpoint.Location = new System.Drawing.Point(206, 280);
            this.btnEffectRemoveHardpoint.Name = "btnEffectRemoveHardpoint";
            this.btnEffectRemoveHardpoint.Size = new System.Drawing.Size(125, 23);
            this.btnEffectRemoveHardpoint.TabIndex = 6;
            this.btnEffectRemoveHardpoint.Text = "Remove";
            this.btnEffectRemoveHardpoint.UseVisualStyleBackColor = true;
            // 
            // btnEffectAddHardpoint
            // 
            this.btnEffectAddHardpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEffectAddHardpoint.Location = new System.Drawing.Point(6, 280);
            this.btnEffectAddHardpoint.Name = "btnEffectAddHardpoint";
            this.btnEffectAddHardpoint.Size = new System.Drawing.Size(125, 23);
            this.btnEffectAddHardpoint.TabIndex = 5;
            this.btnEffectAddHardpoint.Text = "Add";
            this.btnEffectAddHardpoint.UseVisualStyleBackColor = true;
            // 
            // comboType
            // 
            this.comboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboType.Enabled = false;
            this.comboType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(93, 5);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(387, 24);
            this.comboType.TabIndex = 1;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Event Type:";
            // 
            // timeline1
            // 
            this.timeline1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeline1.AutoScroll = true;
            this.timeline1.AutoScrollMinSize = new System.Drawing.Size(0, 612);
            this.timeline1.EventColor = System.Drawing.SystemColors.ControlText;
            this.timeline1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.timeline1.Location = new System.Drawing.Point(11, 47);
            this.timeline1.Name = "timeline1";
            this.timeline1.PlayColor = System.Drawing.Color.Green;
            this.timeline1.SecondaryBackColor = System.Drawing.SystemColors.ControlDark;
            this.timeline1.SecondaryForeColor = System.Drawing.SystemColors.ControlText;
            this.timeline1.SelectedColor = System.Drawing.Color.Red;
            this.timeline1.SelectedItem = new System.Collections.Generic.KeyValuePair<float,object>(0, null);
            this.timeline1.Size = new System.Drawing.Size(183, 612);
            this.timeline1.TabIndex = 4;
            this.timeline1.Text = "timeline1";
            this.timeline1.Timespan = 1F;
            this.timeline1.Zoom = 1F;
            this.timeline1.Stop += new System.EventHandler(this.timeline1_Stop);
            this.timeline1.ItemAdd += new UTFEditor.Timeline.ItemAddEventHandler(this.timeline1_ItemAdd);
            this.timeline1.SelectionChanged += new UTFEditor.Timeline.SelectionChangedEventHandler(this.timeline1_SelectionChanged);
            this.timeline1.Play += new System.EventHandler(this.timeline1_Play);
            // 
            // FuseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 706);
            this.Controls.Add(this.splitFuseEditor);
            this.KeyPreview = true;
            this.Name = "FuseEditor";
            this.Text = "FuseEditor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FuseEditor_KeyDown);
            this.splitFuseEditor.Panel1.ResumeLayout(false);
            this.splitFuseEditor.Panel2.ResumeLayout(false);
            this.splitFuseEditor.Panel2.PerformLayout();
            this.splitFuseEditor.ResumeLayout(false);
            this.panelEffect.ResumeLayout(false);
            this.panelEffect.PerformLayout();
            this.grpEffectTimings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEffectAddTiming)).EndInit();
            this.grpEffectHardpoints.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataEffectHardpoints)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitFuseEditor;
        private Timeline timeline1;
        private System.Windows.Forms.Button btnFlip;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel panelEffect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboEffect;
        private System.Windows.Forms.CheckBox chkEffectAttached;
        private System.Windows.Forms.GroupBox grpEffectTimings;
        private System.Windows.Forms.Button btnEffectRemoveTiming;
        private System.Windows.Forms.Button btnEffectAddTiming;
        private System.Windows.Forms.ListBox lstEffectTimings;
        private System.Windows.Forms.GroupBox grpEffectHardpoints;
        private System.Windows.Forms.DataGridView dataEffectHardpoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffectHardpoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffectHardpointOri;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffectHardpointPos;
        private System.Windows.Forms.Button btnEffectRemoveHardpoint;
        private System.Windows.Forms.Button btnEffectAddHardpoint;
        private System.Windows.Forms.ComboBox comboEvents;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.NumericUpDown txtEffectAddTiming;



    }
}