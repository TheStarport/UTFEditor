namespace UTFEditor
{
    partial class EditAnimChannel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAnimChannel));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxType = new System.Windows.Forms.TextBox();
            this.buttonGenData = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFrames = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.floatBoxGenTime = new System.Windows.Forms.FloatBox();
            this.floatBoxGenDistance = new System.Windows.Forms.FloatBox();
            this.floatBoxInterval = new System.Windows.Forms.FloatBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(175, 415);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(94, 415);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(226, 210);
            this.textBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 235);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel data (time-distance)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.floatBoxGenTime);
            this.groupBox2.Controls.Add(this.floatBoxGenDistance);
            this.groupBox2.Controls.Add(this.floatBoxInterval);
            this.groupBox2.Controls.Add(this.textBoxType);
            this.groupBox2.Controls.Add(this.buttonGenData);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxFrames);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 156);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel header";
            // 
            // textBoxType
            // 
            this.textBoxType.Location = new System.Drawing.Point(97, 68);
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.Size = new System.Drawing.Size(135, 20);
            this.textBoxType.TabIndex = 5;
            // 
            // buttonGenData
            // 
            this.buttonGenData.Location = new System.Drawing.Point(167, 102);
            this.buttonGenData.Name = "buttonGenData";
            this.buttonGenData.Size = new System.Drawing.Size(65, 48);
            this.buttonGenData.TabIndex = 10;
            this.buttonGenData.Text = "Generate Data";
            this.buttonGenData.UseVisualStyleBackColor = true;
            this.buttonGenData.Click += new System.EventHandler(this.buttonGenData_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Distance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Interval";
            // 
            // textBoxFrames
            // 
            this.textBoxFrames.Location = new System.Drawing.Point(97, 16);
            this.textBoxFrames.Name = "textBoxFrames";
            this.textBoxFrames.Size = new System.Drawing.Size(135, 20);
            this.textBoxFrames.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Frames";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Total time (sec)";
            // 
            // floatBoxGenTime
            // 
            this.floatBoxGenTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.floatBoxGenTime.Location = new System.Drawing.Point(97, 128);
            this.floatBoxGenTime.Name = "floatBoxGenTime";
            this.floatBoxGenTime.Size = new System.Drawing.Size(60, 20);
            this.floatBoxGenTime.TabIndex = 12;
            this.floatBoxGenTime.Text = "5";
            this.floatBoxGenTime.Value = 5F;
            // 
            // floatBoxGenDistance
            // 
            this.floatBoxGenDistance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.floatBoxGenDistance.Location = new System.Drawing.Point(97, 102);
            this.floatBoxGenDistance.Name = "floatBoxGenDistance";
            this.floatBoxGenDistance.Size = new System.Drawing.Size(60, 20);
            this.floatBoxGenDistance.TabIndex = 12;
            this.floatBoxGenDistance.Text = "100";
            this.floatBoxGenDistance.Value = 100F;
            // 
            // floatBoxInterval
            // 
            this.floatBoxInterval.ForeColor = System.Drawing.SystemColors.ControlText;
            this.floatBoxInterval.Location = new System.Drawing.Point(97, 42);
            this.floatBoxInterval.Name = "floatBoxInterval";
            this.floatBoxInterval.Size = new System.Drawing.Size(135, 20);
            this.floatBoxInterval.TabIndex = 11;
            // 
            // EditAnimChannel
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(262, 447);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditAnimChannel";
            this.ShowIcon = false;
            this.Text = "Edit Anim Channel";
            this.Load += new System.EventHandler(this.EditAnimChannel_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFrames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGenData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FloatBox floatBoxGenTime;
        private System.Windows.Forms.FloatBox floatBoxGenDistance;
        private System.Windows.Forms.FloatBox floatBoxInterval;
    }
}