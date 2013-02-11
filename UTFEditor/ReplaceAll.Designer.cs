namespace UTFEditor
{
    partial class ReplaceAll
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
            this.txtFind = new System.Windows.Forms.TextBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.lblReplace = new System.Windows.Forms.Label();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.chkWholeWord = new System.Windows.Forms.CheckBox();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdoMatchContent = new System.Windows.Forms.RadioButton();
            this.rdoMatchAll = new System.Windows.Forms.RadioButton();
            this.rdoMatchName = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Location = new System.Drawing.Point(12, 25);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(260, 20);
            this.txtFind.TabIndex = 0;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // lblFind
            // 
            this.lblFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(12, 9);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(56, 13);
            this.lblFind.TabIndex = 1;
            this.lblFind.Text = "Find what:";
            // 
            // lblReplace
            // 
            this.lblReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new System.Drawing.Point(12, 56);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(72, 13);
            this.lblReplace.TabIndex = 3;
            this.lblReplace.Text = "Replace with:";
            // 
            // txtReplace
            // 
            this.txtReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReplace.Location = new System.Drawing.Point(12, 72);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(260, 20);
            this.txtReplace.TabIndex = 2;
            this.txtReplace.TextChanged += new System.EventHandler(this.txtReplace_TextChanged);
            // 
            // chkWholeWord
            // 
            this.chkWholeWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkWholeWord.AutoSize = true;
            this.chkWholeWord.Location = new System.Drawing.Point(12, 98);
            this.chkWholeWord.Name = "chkWholeWord";
            this.chkWholeWord.Size = new System.Drawing.Size(113, 17);
            this.chkWholeWord.TabIndex = 4;
            this.chkWholeWord.Text = "Match whole word";
            this.chkWholeWord.UseVisualStyleBackColor = true;
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReplaceAll.Enabled = false;
            this.btnReplaceAll.Location = new System.Drawing.Point(12, 154);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(111, 23);
            this.btnReplaceAll.TabIndex = 5;
            this.btnReplaceAll.Text = "Replace All";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(161, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdoMatchContent
            // 
            this.rdoMatchContent.AutoSize = true;
            this.rdoMatchContent.Location = new System.Drawing.Point(12, 121);
            this.rdoMatchContent.Name = "rdoMatchContent";
            this.rdoMatchContent.Size = new System.Drawing.Size(95, 17);
            this.rdoMatchContent.TabIndex = 7;
            this.rdoMatchContent.TabStop = true;
            this.rdoMatchContent.Text = "Match Content";
            this.rdoMatchContent.UseVisualStyleBackColor = true;
            // 
            // rdoMatchAll
            // 
            this.rdoMatchAll.AutoSize = true;
            this.rdoMatchAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoMatchAll.Checked = true;
            this.rdoMatchAll.Location = new System.Drawing.Point(144, 97);
            this.rdoMatchAll.Name = "rdoMatchAll";
            this.rdoMatchAll.Size = new System.Drawing.Size(128, 17);
            this.rdoMatchAll.TabIndex = 8;
            this.rdoMatchAll.TabStop = true;
            this.rdoMatchAll.Text = "Match Content/Name";
            this.rdoMatchAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoMatchAll.UseVisualStyleBackColor = true;
            // 
            // rdoMatchName
            // 
            this.rdoMatchName.AutoSize = true;
            this.rdoMatchName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoMatchName.Location = new System.Drawing.Point(186, 121);
            this.rdoMatchName.Name = "rdoMatchName";
            this.rdoMatchName.Size = new System.Drawing.Size(86, 17);
            this.rdoMatchName.TabIndex = 9;
            this.rdoMatchName.TabStop = true;
            this.rdoMatchName.Text = "Match Name";
            this.rdoMatchName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoMatchName.UseVisualStyleBackColor = true;
            // 
            // ReplaceAll
            // 
            this.AcceptButton = this.btnReplaceAll;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 189);
            this.Controls.Add(this.rdoMatchName);
            this.Controls.Add(this.rdoMatchAll);
            this.Controls.Add(this.rdoMatchContent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.chkWholeWord);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.txtFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ReplaceAll";
            this.Text = "Replace All";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.CheckBox chkWholeWord;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdoMatchContent;
        private System.Windows.Forms.RadioButton rdoMatchAll;
        private System.Windows.Forms.RadioButton rdoMatchName;
    }
}