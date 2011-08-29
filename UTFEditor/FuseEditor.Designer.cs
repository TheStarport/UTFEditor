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
            this.btnFlip = new System.Windows.Forms.Button();
            this.comboEvents = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.timeline1 = new UTFEditor.Timeline();
            this.splitFuseEditor.Panel1.SuspendLayout();
            this.splitFuseEditor.SuspendLayout();
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
            this.splitFuseEditor.Panel1.Controls.Add(this.btnFlip);
            this.splitFuseEditor.Panel1.Controls.Add(this.comboEvents);
            this.splitFuseEditor.Panel1.Controls.Add(this.btnNext);
            this.splitFuseEditor.Panel1.Controls.Add(this.btnPrev);
            this.splitFuseEditor.Panel1.Controls.Add(this.timeline1);
            this.splitFuseEditor.Panel1MinSize = 150;
            this.splitFuseEditor.Size = new System.Drawing.Size(697, 783);
            this.splitFuseEditor.SplitterDistance = 200;
            this.splitFuseEditor.TabIndex = 0;
            this.splitFuseEditor.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitFuseEditor_SplitterMoved);
            // 
            // btnFlip
            // 
            this.btnFlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlip.Image = ((System.Drawing.Image)(resources.GetObject("btnFlip.Image")));
            this.btnFlip.Location = new System.Drawing.Point(12, 737);
            this.btnFlip.Name = "btnFlip";
            this.btnFlip.Size = new System.Drawing.Size(24, 28);
            this.btnFlip.TabIndex = 8;
            this.btnFlip.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFlip.UseVisualStyleBackColor = true;
            this.btnFlip.Click += new System.EventHandler(this.btnFlip_Click);
            // 
            // comboEvents
            // 
            this.comboEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboEvents.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboEvents.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboEvents.FormattingEnabled = true;
            this.comboEvents.Location = new System.Drawing.Point(72, 744);
            this.comboEvents.Name = "comboEvents";
            this.comboEvents.Size = new System.Drawing.Size(93, 21);
            this.comboEvents.Sorted = true;
            this.comboEvents.TabIndex = 7;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(171, 741);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(24, 28);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "»";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(42, 737);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(24, 28);
            this.btnPrev.TabIndex = 5;
            this.btnPrev.Text = "«";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // timeline1
            // 
            this.timeline1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeline1.AutoScroll = true;
            this.timeline1.AutoScrollMinSize = new System.Drawing.Size(0, 723);
            this.timeline1.EventColor = System.Drawing.SystemColors.ControlText;
            this.timeline1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.timeline1.Location = new System.Drawing.Point(12, 12);
            this.timeline1.Name = "timeline1";
            this.timeline1.SecondaryBackColor = System.Drawing.SystemColors.ControlDark;
            this.timeline1.SecondaryForeColor = System.Drawing.SystemColors.ControlText;
            this.timeline1.SelectedColor = System.Drawing.Color.Red;
            this.timeline1.SelectedEvent = null;
            this.timeline1.Size = new System.Drawing.Size(183, 723);
            this.timeline1.TabIndex = 4;
            this.timeline1.Text = "timeline1";
            this.timeline1.Zoom = 1F;
            // 
            // FuseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 783);
            this.Controls.Add(this.splitFuseEditor);
            this.KeyPreview = true;
            this.Name = "FuseEditor";
            this.Text = "FuseEditor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FuseEditor_KeyDown);
            this.splitFuseEditor.Panel1.ResumeLayout(false);
            this.splitFuseEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitFuseEditor;
        private System.Windows.Forms.ComboBox comboEvents;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private Timeline timeline1;
        private System.Windows.Forms.Button btnFlip;



    }
}