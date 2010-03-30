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
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.Panel();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.textBoxPosX = new System.Windows.Forms.TextBox();
            this.textBoxPosY = new System.Windows.Forms.TextBox();
            this.textBoxPosZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxSolid
            // 
            this.checkBoxSolid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSolid.AutoSize = true;
            this.checkBoxSolid.Location = new System.Drawing.Point(13, 478);
            this.checkBoxSolid.Name = "checkBoxSolid";
            this.checkBoxSolid.Size = new System.Drawing.Size(49, 17);
            this.checkBoxSolid.TabIndex = 1;
            this.checkBoxSolid.Text = "Solid";
            this.checkBoxSolid.UseVisualStyleBackColor = true;
            this.checkBoxSolid.CheckedChanged += new System.EventHandler(this.checkBoxSolid_CheckedChanged);
            // 
            // trackBarScale
            // 
            this.trackBarScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarScale.AutoSize = false;
            this.trackBarScale.Location = new System.Drawing.Point(122, 474);
            this.trackBarScale.Maximum = 300;
            this.trackBarScale.Minimum = -300;
            this.trackBarScale.Name = "trackBarScale";
            this.trackBarScale.Size = new System.Drawing.Size(104, 25);
            this.trackBarScale.TabIndex = 3;
            this.trackBarScale.Value = 130;
            this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 479);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scale";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 458);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Press left mouse button to rotate; with shift to move.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(552, 442);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // textBoxScale
            // 
            this.textBoxScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxScale.Location = new System.Drawing.Point(232, 474);
            this.textBoxScale.MaxLength = 8;
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(54, 20);
            this.textBoxScale.TabIndex = 4;
            this.textBoxScale.Text = "20";
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // textBoxPosX
            // 
            this.textBoxPosX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPosX.Location = new System.Drawing.Point(391, 475);
            this.textBoxPosX.MaxLength = 8;
            this.textBoxPosX.Name = "textBoxPosX";
            this.textBoxPosX.Size = new System.Drawing.Size(54, 20);
            this.textBoxPosX.TabIndex = 6;
            this.textBoxPosX.Text = "0";
            this.textBoxPosX.TextChanged += new System.EventHandler(this.textBoxPosXYZ_TextChanged);
            // 
            // textBoxPosY
            // 
            this.textBoxPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPosY.Location = new System.Drawing.Point(451, 475);
            this.textBoxPosY.MaxLength = 8;
            this.textBoxPosY.Name = "textBoxPosY";
            this.textBoxPosY.Size = new System.Drawing.Size(54, 20);
            this.textBoxPosY.TabIndex = 7;
            this.textBoxPosY.Text = "0";
            this.textBoxPosY.TextChanged += new System.EventHandler(this.textBoxPosXYZ_TextChanged);
            // 
            // textBoxPosZ
            // 
            this.textBoxPosZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPosZ.Location = new System.Drawing.Point(511, 476);
            this.textBoxPosZ.MaxLength = 8;
            this.textBoxPosZ.Name = "textBoxPosZ";
            this.textBoxPosZ.Size = new System.Drawing.Size(54, 20);
            this.textBoxPosZ.TabIndex = 8;
            this.textBoxPosZ.Text = "0";
            this.textBoxPosZ.TextChanged += new System.EventHandler(this.textBoxPosXYZ_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(311, 478);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Center (X,Y,Z)";
            // 
            // ModelViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 502);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxPosZ);
            this.Controls.Add(this.textBoxPosY);
            this.Controls.Add(this.textBoxPosX);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarScale);
            this.Controls.Add(this.checkBoxSolid);
            this.Name = "ModelViewForm";
            this.ShowIcon = false;
            this.Text = "Model View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelViewForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSolid;
        private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pictureBox1;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.TextBox textBoxPosX;
        private System.Windows.Forms.TextBox textBoxPosY;
        private System.Windows.Forms.TextBox textBoxPosZ;
        private System.Windows.Forms.Label label3;
    }
}