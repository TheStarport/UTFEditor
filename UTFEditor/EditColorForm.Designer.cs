namespace UTFEditor
{
    partial class EditColorForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.spinnerR = new System.Windows.Forms.NumericUpDown();
            this.spinnerG = new System.Windows.Forms.NumericUpDown();
            this.spinnerB = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxHex = new System.Windows.Forms.TextBox();
            this.buttonColor = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.floatBoxR = new System.Windows.Forms.FloatBox();
            this.floatBoxG = new System.Windows.Forms.FloatBox();
            this.floatBoxB = new System.Windows.Forms.FloatBox();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerB)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Float";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Red";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Green";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(231, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Blue";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Integer";
            // 
            // spinnerR
            // 
            this.spinnerR.Location = new System.Drawing.Point(58, 51);
            this.spinnerR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinnerR.Name = "spinnerR";
            this.spinnerR.Size = new System.Drawing.Size(70, 20);
            this.spinnerR.TabIndex = 8;
            // 
            // spinnerG
            // 
            this.spinnerG.Location = new System.Drawing.Point(134, 51);
            this.spinnerG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinnerG.Name = "spinnerG";
            this.spinnerG.Size = new System.Drawing.Size(70, 20);
            this.spinnerG.TabIndex = 9;
            // 
            // spinnerB
            // 
            this.spinnerB.Location = new System.Drawing.Point(210, 51);
            this.spinnerB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinnerB.Name = "spinnerB";
            this.spinnerB.Size = new System.Drawing.Size(70, 20);
            this.spinnerB.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hex";
            // 
            // textBoxHex
            // 
            this.textBoxHex.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxHex.Location = new System.Drawing.Point(58, 77);
            this.textBoxHex.MaxLength = 6;
            this.textBoxHex.Name = "textBoxHex";
            this.textBoxHex.Size = new System.Drawing.Size(70, 20);
            this.textBoxHex.TabIndex = 12;
            this.textBoxHex.TextChanged += new System.EventHandler(this.textBoxHex_TextChanged);
            this.textBoxHex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxHex_KeyDown);
            this.textBoxHex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxHex_KeyPress);
            this.textBoxHex.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxHex_Validating);
            // 
            // buttonColor
            // 
            this.buttonColor.BackColor = System.Drawing.Color.Black;
            this.buttonColor.Location = new System.Drawing.Point(295, 25);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(72, 72);
            this.buttonColor.TabIndex = 13;
            this.buttonColor.UseVisualStyleBackColor = false;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(217, 115);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(72, 23);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(295, 115);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 23);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // colorDialog1
            // 
            this.colorDialog1.FullOpen = true;
            // 
            // floatBoxR
            // 
            this.floatBoxR.ForeColor = System.Drawing.SystemColors.ControlText;
            this.floatBoxR.Location = new System.Drawing.Point(58, 25);
            this.floatBoxR.Name = "floatBoxR";
            this.floatBoxR.Size = new System.Drawing.Size(70, 20);
            this.floatBoxR.TabIndex = 2;
            this.floatBoxR.TextChanged += new System.EventHandler(this.floatBoxRGB_TextChanged);
            // 
            // floatBoxG
            // 
            this.floatBoxG.ForeColor = System.Drawing.SystemColors.ControlText;
            this.floatBoxG.Location = new System.Drawing.Point(134, 25);
            this.floatBoxG.Name = "floatBoxG";
            this.floatBoxG.Size = new System.Drawing.Size(70, 20);
            this.floatBoxG.TabIndex = 4;
            this.floatBoxG.TextChanged += new System.EventHandler(this.floatBoxRGB_TextChanged);
            // 
            // floatBoxB
            // 
            this.floatBoxB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.floatBoxB.Location = new System.Drawing.Point(210, 25);
            this.floatBoxB.Name = "floatBoxB";
            this.floatBoxB.Size = new System.Drawing.Size(70, 20);
            this.floatBoxB.TabIndex = 6;
            this.floatBoxB.TextChanged += new System.EventHandler(this.floatBoxRGB_TextChanged);
            // 
            // EditColorForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(379, 150);
            this.Controls.Add(this.floatBoxB);
            this.Controls.Add(this.floatBoxG);
            this.Controls.Add(this.floatBoxR);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonColor);
            this.Controls.Add(this.textBoxHex);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.spinnerB);
            this.Controls.Add(this.spinnerG);
            this.Controls.Add(this.spinnerR);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditColorForm";
            this.ShowIcon = false;
            this.Text = "Edit Color";
            ((System.ComponentModel.ISupportInitialize)(this.spinnerR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown spinnerR;
        private System.Windows.Forms.NumericUpDown spinnerG;
        private System.Windows.Forms.NumericUpDown spinnerB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxHex;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FloatBox floatBoxR;
        private System.Windows.Forms.FloatBox floatBoxG;
        private System.Windows.Forms.FloatBox floatBoxB;
    }
}