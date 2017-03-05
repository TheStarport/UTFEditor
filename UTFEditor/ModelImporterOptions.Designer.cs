namespace UTFEditor
{
    partial class ModelImporterOptions
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
            this.chkWireframe = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstVertexType = new System.Windows.Forms.ListBox();
            this.chkRelocate = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUniqueName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkWireframe
            // 
            this.chkWireframe.AutoSize = true;
            this.chkWireframe.Location = new System.Drawing.Point(12, 95);
            this.chkWireframe.Name = "chkWireframe";
            this.chkWireframe.Size = new System.Drawing.Size(155, 21);
            this.chkWireframe.TabIndex = 0;
            this.chkWireframe.Text = "Generate wireframe";
            this.chkWireframe.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 314);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 31);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(175, 314);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 31);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstVertexType
            // 
            this.lstVertexType.FormattingEnabled = true;
            this.lstVertexType.ItemHeight = 16;
            this.lstVertexType.Items.AddRange(new object[] {
            "Normals",
            "Vertex Colors (starspheres)",
            "Vertex Colors and Normals",
            "Extra UVs (detail maps)",
            "Tangents and Binormals",
            "Extra UVs, Tangents and Binormals"});
            this.lstVertexType.Location = new System.Drawing.Point(12, 149);
            this.lstVertexType.Name = "lstVertexType";
            this.lstVertexType.Size = new System.Drawing.Size(313, 148);
            this.lstVertexType.TabIndex = 4;
            // 
            // chkRelocate
            // 
            this.chkRelocate.AutoSize = true;
            this.chkRelocate.Location = new System.Drawing.Point(12, 122);
            this.chkRelocate.Name = "chkRelocate";
            this.chkRelocate.Size = new System.Drawing.Size(312, 21);
            this.chkRelocate.TabIndex = 5;
            this.chkRelocate.Text = "Relocate parts according to pivot information";
            this.chkRelocate.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Unique Name";
            // 
            // txtUniqueName
            // 
            this.txtUniqueName.Location = new System.Drawing.Point(12, 29);
            this.txtUniqueName.Name = "txtUniqueName";
            this.txtUniqueName.Size = new System.Drawing.Size(312, 22);
            this.txtUniqueName.TabIndex = 7;
            // 
            // ModelImporterOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(337, 357);
            this.Controls.Add(this.txtUniqueName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkRelocate);
            this.Controls.Add(this.lstVertexType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkWireframe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ModelImporterOptions";
            this.Text = "Model Import Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkWireframe;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lstVertexType;
        private System.Windows.Forms.CheckBox chkRelocate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUniqueName;
    }
}