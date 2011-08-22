namespace UTFEditor
{
    partial class AddHardpoints
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
            this.comboPresets = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpHpList = new System.Windows.Forms.GroupBox();
            this.chkRevolute = new System.Windows.Forms.CheckBox();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnRemoveHp = new System.Windows.Forms.Button();
            this.btnAddHp = new System.Windows.Forms.Button();
            this.txtAddHp = new System.Windows.Forms.TextBox();
            this.lstHps = new System.Windows.Forms.CheckedListBox();
            this.btnSavePreset = new System.Windows.Forms.Button();
            this.grpAddHps = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPrevType = new System.Windows.Forms.Button();
            this.btnNextType = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.txtHp = new System.Windows.Forms.TextBox();
            this.chkSet = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.grpHpList.SuspendLayout();
            this.grpAddHps.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboPresets
            // 
            this.comboPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboPresets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboPresets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboPresets.DropDownHeight = 130;
            this.comboPresets.FormattingEnabled = true;
            this.comboPresets.IntegralHeight = false;
            this.comboPresets.Location = new System.Drawing.Point(55, 12);
            this.comboPresets.MaxDropDownItems = 20;
            this.comboPresets.Name = "comboPresets";
            this.comboPresets.Size = new System.Drawing.Size(170, 21);
            this.comboPresets.Sorted = true;
            this.comboPresets.TabIndex = 0;
            this.comboPresets.SelectionChangeCommitted += new System.EventHandler(this.comboPresets_SelectionChangeCommitted);
            this.comboPresets.SelectedIndexChanged += new System.EventHandler(this.comboPresets_SelectedIndexChanged);
            this.comboPresets.Leave += new System.EventHandler(this.comboPresets_Leave);
            this.comboPresets.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboPresets_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Preset";
            // 
            // grpHpList
            // 
            this.grpHpList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpHpList.Controls.Add(this.chkRevolute);
            this.grpHpList.Controls.Add(this.btnMoveDown);
            this.grpHpList.Controls.Add(this.btnMoveUp);
            this.grpHpList.Controls.Add(this.btnRemoveHp);
            this.grpHpList.Controls.Add(this.btnAddHp);
            this.grpHpList.Controls.Add(this.txtAddHp);
            this.grpHpList.Controls.Add(this.lstHps);
            this.grpHpList.Location = new System.Drawing.Point(12, 156);
            this.grpHpList.Name = "grpHpList";
            this.grpHpList.Size = new System.Drawing.Size(260, 417);
            this.grpHpList.TabIndex = 2;
            this.grpHpList.TabStop = false;
            this.grpHpList.Text = "Hardpoint Types List";
            // 
            // chkRevolute
            // 
            this.chkRevolute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRevolute.AutoSize = true;
            this.chkRevolute.Location = new System.Drawing.Point(112, 393);
            this.chkRevolute.Name = "chkRevolute";
            this.chkRevolute.Size = new System.Drawing.Size(69, 17);
            this.chkRevolute.TabIndex = 9;
            this.chkRevolute.Text = "Revolute";
            this.chkRevolute.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.Enabled = false;
            this.btnMoveDown.Location = new System.Drawing.Point(173, 362);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDown.TabIndex = 8;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.Enabled = false;
            this.btnMoveUp.Location = new System.Drawing.Point(100, 362);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(67, 23);
            this.btnMoveUp.TabIndex = 7;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnRemoveHp
            // 
            this.btnRemoveHp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveHp.Enabled = false;
            this.btnRemoveHp.Location = new System.Drawing.Point(6, 362);
            this.btnRemoveHp.Name = "btnRemoveHp";
            this.btnRemoveHp.Size = new System.Drawing.Size(88, 23);
            this.btnRemoveHp.TabIndex = 6;
            this.btnRemoveHp.Text = "Remove";
            this.btnRemoveHp.UseVisualStyleBackColor = true;
            this.btnRemoveHp.Click += new System.EventHandler(this.btnRemoveHp_Click);
            // 
            // btnAddHp
            // 
            this.btnAddHp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddHp.Location = new System.Drawing.Point(187, 389);
            this.btnAddHp.Name = "btnAddHp";
            this.btnAddHp.Size = new System.Drawing.Size(66, 23);
            this.btnAddHp.TabIndex = 5;
            this.btnAddHp.Text = "Add";
            this.btnAddHp.UseVisualStyleBackColor = true;
            this.btnAddHp.Click += new System.EventHandler(this.btnAddHp_Click);
            // 
            // txtAddHp
            // 
            this.txtAddHp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddHp.Location = new System.Drawing.Point(6, 391);
            this.txtAddHp.Name = "txtAddHp";
            this.txtAddHp.Size = new System.Drawing.Size(100, 20);
            this.txtAddHp.TabIndex = 4;
            // 
            // lstHps
            // 
            this.lstHps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstHps.FormattingEnabled = true;
            this.lstHps.Location = new System.Drawing.Point(6, 19);
            this.lstHps.Name = "lstHps";
            this.lstHps.Size = new System.Drawing.Size(248, 334);
            this.lstHps.TabIndex = 3;
            this.lstHps.SelectedIndexChanged += new System.EventHandler(this.lstHps_SelectedIndexChanged);
            // 
            // btnSavePreset
            // 
            this.btnSavePreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavePreset.Location = new System.Drawing.Point(231, 10);
            this.btnSavePreset.Name = "btnSavePreset";
            this.btnSavePreset.Size = new System.Drawing.Size(41, 23);
            this.btnSavePreset.TabIndex = 7;
            this.btnSavePreset.Text = "Save";
            this.btnSavePreset.UseVisualStyleBackColor = true;
            this.btnSavePreset.Click += new System.EventHandler(this.btnSavePreset_Click);
            // 
            // grpAddHps
            // 
            this.grpAddHps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAddHps.Controls.Add(this.tableLayoutPanel1);
            this.grpAddHps.Enabled = false;
            this.grpAddHps.Location = new System.Drawing.Point(12, 39);
            this.grpAddHps.Name = "grpAddHps";
            this.grpAddHps.Size = new System.Drawing.Size(260, 111);
            this.grpAddHps.TabIndex = 10;
            this.grpAddHps.TabStop = false;
            this.grpAddHps.Text = "Add Controls";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnPrevType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNextType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnNext, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtHp, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkSet, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(254, 92);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnPrevType
            // 
            this.btnPrevType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevType.Location = new System.Drawing.Point(3, 3);
            this.btnPrevType.Name = "btnPrevType";
            this.btnPrevType.Size = new System.Drawing.Size(184, 24);
            this.btnPrevType.TabIndex = 0;
            this.btnPrevType.Text = "Prev Hardpoint Type";
            this.btnPrevType.UseVisualStyleBackColor = true;
            this.btnPrevType.Click += new System.EventHandler(this.btnPrevType_Click);
            // 
            // btnNextType
            // 
            this.btnNextType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextType.Location = new System.Drawing.Point(3, 63);
            this.btnNextType.Name = "btnNextType";
            this.btnNextType.Size = new System.Drawing.Size(184, 26);
            this.btnNextType.TabIndex = 1;
            this.btnNextType.Text = "Next Hardpoint Type";
            this.btnNextType.UseVisualStyleBackColor = true;
            this.btnNextType.Click += new System.EventHandler(this.btnNextType_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(193, 33);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(58, 24);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // txtHp
            // 
            this.txtHp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHp.Location = new System.Drawing.Point(3, 33);
            this.txtHp.Name = "txtHp";
            this.txtHp.ReadOnly = true;
            this.txtHp.Size = new System.Drawing.Size(184, 20);
            this.txtHp.TabIndex = 4;
            // 
            // chkSet
            // 
            this.chkSet.AutoSize = true;
            this.chkSet.Enabled = false;
            this.chkSet.Location = new System.Drawing.Point(193, 63);
            this.chkSet.Name = "chkSet";
            this.chkSet.Size = new System.Drawing.Size(53, 17);
            this.chkSet.TabIndex = 6;
            this.chkSet.TabStop = false;
            this.chkSet.Text = "Exists";
            this.chkSet.UseVisualStyleBackColor = true;
            this.chkSet.CheckedChanged += new System.EventHandler(this.chkSet_CheckedChanged);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(12, 579);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(260, 26);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // AddHardpoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 617);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpAddHps);
            this.Controls.Add(this.btnSavePreset);
            this.Controls.Add(this.grpHpList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboPresets);
            this.MaximizeBox = false;
            this.Name = "AddHardpoints";
            this.ShowIcon = false;
            this.Text = "Add Hardpoints";
            this.TopMost = true;
            this.grpHpList.ResumeLayout(false);
            this.grpHpList.PerformLayout();
            this.grpAddHps.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboPresets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpHpList;
        private System.Windows.Forms.Button btnRemoveHp;
        private System.Windows.Forms.Button btnAddHp;
        private System.Windows.Forms.TextBox txtAddHp;
        private System.Windows.Forms.CheckedListBox lstHps;
        private System.Windows.Forms.Button btnSavePreset;
        private System.Windows.Forms.GroupBox grpAddHps;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnPrevType;
        private System.Windows.Forms.Button btnNextType;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox txtHp;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox chkRevolute;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.CheckBox chkSet;
    }
}