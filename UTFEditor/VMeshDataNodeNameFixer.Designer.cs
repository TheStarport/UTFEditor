namespace UTFEditor
{
    partial class VMeshDataNodeNameFixer
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonFixFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listBoxOptions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 66);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(519, 342);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(13, 13);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 43);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonFixFile
            // 
            this.buttonFixFile.Location = new System.Drawing.Point(455, 13);
            this.buttonFixFile.Name = "buttonFixFile";
            this.buttonFixFile.Size = new System.Drawing.Size(77, 43);
            this.buttonFixFile.TabIndex = 3;
            this.buttonFixFile.Text = "Fix File";
            this.buttonFixFile.UseVisualStyleBackColor = true;
            this.buttonFixFile.Click += new System.EventHandler(this.buttonFixFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // listBoxOptions
            // 
            this.listBoxOptions.FormattingEnabled = true;
            this.listBoxOptions.Items.AddRange(new object[] {
            "Scan Only",
            "Open File On Error",
            "Prompt to Fix Errors",
            "Automatically Fix Errors"});
            this.listBoxOptions.Location = new System.Drawing.Point(94, 13);
            this.listBoxOptions.Name = "listBoxOptions";
            this.listBoxOptions.ScrollAlwaysVisible = true;
            this.listBoxOptions.Size = new System.Drawing.Size(124, 43);
            this.listBoxOptions.TabIndex = 4;
            // 
            // VMeshDataNodeNameFixer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 420);
            this.Controls.Add(this.listBoxOptions);
            this.Controls.Add(this.buttonFixFile);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.richTextBox1);
            this.Name = "VMeshDataNodeNameFixer";
            this.ShowIcon = false;
            this.Text = "VMeshData Node Name Tester";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonFixFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox listBoxOptions;
    }
}