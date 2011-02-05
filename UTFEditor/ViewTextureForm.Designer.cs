namespace UTFEditor
{
    partial class ViewTextureForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxFlip = new System.Windows.Forms.CheckBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.spinnerLevel = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxTransparent = new System.Windows.Forms.CheckBox();
            this.checkBoxStretch = new System.Windows.Forms.CheckBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerLevel)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::UTFEditor.Properties.Resources.backgroundpattern;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxFlip
            // 
            this.checkBoxFlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxFlip.AutoSize = true;
            this.checkBoxFlip.Location = new System.Drawing.Point(202, 25);
            this.checkBoxFlip.Name = "checkBoxFlip";
            this.checkBoxFlip.Size = new System.Drawing.Size(42, 17);
            this.checkBoxFlip.TabIndex = 5;
            this.checkBoxFlip.Text = "&Flip";
            this.checkBoxFlip.UseVisualStyleBackColor = true;
            this.checkBoxFlip.CheckedChanged += new System.EventHandler(this.checkBoxFlip_CheckedChanged);
            // 
            // labelSize
            // 
            this.labelSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSize.Location = new System.Drawing.Point(184, 8);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(60, 13);
            this.labelSize.TabIndex = 4;
            this.labelSize.Text = "2048x2048";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // spinnerLevel
            // 
            this.spinnerLevel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.spinnerLevel.Location = new System.Drawing.Point(130, 6);
            this.spinnerLevel.Name = "spinnerLevel";
            this.spinnerLevel.Size = new System.Drawing.Size(36, 20);
            this.spinnerLevel.TabIndex = 3;
            this.spinnerLevel.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinnerLevel.ValueChanged += new System.EventHandler(this.spinnerLevel_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.Location = new System.Drawing.Point(91, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Level";
            // 
            // checkBoxTransparent
            // 
            this.checkBoxTransparent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTransparent.AutoSize = true;
            this.checkBoxTransparent.Checked = true;
            this.checkBoxTransparent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTransparent.Location = new System.Drawing.Point(12, 25);
            this.checkBoxTransparent.Name = "checkBoxTransparent";
            this.checkBoxTransparent.Size = new System.Drawing.Size(83, 17);
            this.checkBoxTransparent.TabIndex = 1;
            this.checkBoxTransparent.Text = "&Transparent";
            this.checkBoxTransparent.UseVisualStyleBackColor = true;
            this.checkBoxTransparent.Click += new System.EventHandler(this.checkBoxTransparent_Click);
            // 
            // checkBoxStretch
            // 
            this.checkBoxStretch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStretch.AutoSize = true;
            this.checkBoxStretch.Location = new System.Drawing.Point(12, 7);
            this.checkBoxStretch.Name = "checkBoxStretch";
            this.checkBoxStretch.Size = new System.Drawing.Size(60, 17);
            this.checkBoxStretch.TabIndex = 0;
            this.checkBoxStretch.Text = "&Stretch";
            this.checkBoxStretch.UseVisualStyleBackColor = true;
            this.checkBoxStretch.CheckedChanged += new System.EventHandler(this.checkBoxStretch_CheckedChanged);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.checkBoxStretch);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.checkBoxFlip);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.spinnerLevel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.labelSize);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.checkBoxTransparent);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(256, 49);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 256);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(256, 49);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // ViewTextureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 305);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ViewTextureForm";
            this.ShowIcon = false;
            this.Text = "ViewTextureForm";
            this.Load += new System.EventHandler(this.ViewTextureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerLevel)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.NumericUpDown spinnerLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxStretch;
        private System.Windows.Forms.CheckBox checkBoxTransparent;
        private System.Windows.Forms.CheckBox checkBoxFlip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    }
}