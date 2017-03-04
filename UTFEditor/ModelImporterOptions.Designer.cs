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
            this.SuspendLayout();
            // 
            // chkWireframe
            // 
            this.chkWireframe.AutoSize = true;
            this.chkWireframe.Location = new System.Drawing.Point(12, 12);
            this.chkWireframe.Name = "chkWireframe";
            this.chkWireframe.Size = new System.Drawing.Size(159, 21);
            this.chkWireframe.TabIndex = 0;
            this.chkWireframe.Text = "Generate Wireframe";
            this.chkWireframe.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 164);
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
            this.btnCancel.Location = new System.Drawing.Point(170, 164);
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
            this.lstVertexType.Location = new System.Drawing.Point(12, 39);
            this.lstVertexType.Name = "lstVertexType";
            this.lstVertexType.Size = new System.Drawing.Size(308, 116);
            this.lstVertexType.TabIndex = 4;
            // 
            // ModelImporterOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(332, 207);
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
    }
}