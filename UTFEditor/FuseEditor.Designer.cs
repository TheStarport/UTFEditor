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
            this.timeline1 = new UTFEditor.Timeline();
            this.SuspendLayout();
            // 
            // timeline1
            // 
            this.timeline1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeline1.EventColor = System.Drawing.SystemColors.Control;
            this.timeline1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.timeline1.Location = new System.Drawing.Point(12, 12);
            this.timeline1.Name = "timeline1";
            this.timeline1.SecondaryForeColor = System.Drawing.SystemColors.Control;
            this.timeline1.Size = new System.Drawing.Size(1784, 126);
            this.timeline1.TabIndex = 0;
            // 
            // FuseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1808, 150);
            this.Controls.Add(this.timeline1);
            this.Name = "FuseEditor";
            this.Text = "FuseEditor";
            this.Load += new System.EventHandler(this.FuseEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Timeline timeline1;
    }
}