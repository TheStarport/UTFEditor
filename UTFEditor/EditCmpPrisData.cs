using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class EditCmpPrisData : Form
    {
        UTFForm parent;
        TreeNode node;
        CmpPrisData data;
        int partNumber;

        public EditCmpPrisData(UTFForm parent, TreeNode node)
        {
            this.parent = parent;
            this.node = node;
            InitializeComponent();
        }

        private void EditCmpPrisData_Load(object sender, EventArgs e)
        {
            try
            {
                data = new CmpPrisData(node.Tag as byte[]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                Close();
                return;
            }

            partNumber = 0;
            DisplayPart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                data.Parts[partNumber].ParentName = this.textBoxParentName.Text;
                data.Parts[partNumber].ChildName = this.textBoxChildName.Text;
                data.Parts[partNumber].OriginX = Single.Parse(this.textBoxOriginX.Text);
                data.Parts[partNumber].OriginY = Single.Parse(this.textBoxOriginY.Text);
                data.Parts[partNumber].OriginZ = Single.Parse(this.textBoxOriginZ.Text);
                data.Parts[partNumber].OffsetX = Single.Parse(this.textBoxOffsetX.Text);
                data.Parts[partNumber].OffsetY = Single.Parse(this.textBoxOffsetY.Text);
                data.Parts[partNumber].OffsetZ = Single.Parse(this.textBoxOffsetZ.Text);
                data.Parts[partNumber].RotMatXX = Single.Parse(this.textBoxRotMatXX.Text);
                data.Parts[partNumber].RotMatXY = Single.Parse(this.textBoxRotMatXY.Text);
                data.Parts[partNumber].RotMatXZ = Single.Parse(this.textBoxRotMatXZ.Text);
                data.Parts[partNumber].RotMatYX = Single.Parse(this.textBoxRotMatYX.Text);
                data.Parts[partNumber].RotMatYY = Single.Parse(this.textBoxRotMatYY.Text);
                data.Parts[partNumber].RotMatYZ = Single.Parse(this.textBoxRotMatYZ.Text);
                data.Parts[partNumber].RotMatZX = Single.Parse(this.textBoxRotMatZX.Text);
                data.Parts[partNumber].RotMatZY = Single.Parse(this.textBoxRotMatZY.Text);
                data.Parts[partNumber].RotMatZZ = Single.Parse(this.textBoxRotMatZZ.Text);

                data.Parts[partNumber].AxisRotX = Single.Parse(this.textBoxAxisRotX.Text);
                data.Parts[partNumber].AxisRotY = Single.Parse(this.textBoxAxisRotY.Text);
                data.Parts[partNumber].AxisRotZ = Single.Parse(this.textBoxAxisRotZ.Text);

                data.Parts[partNumber].Unknown = Single.Parse(this.textBoxUnknown.Text);
                data.Parts[partNumber].Angle = Single.Parse(this.textBoxAngle.Text);

                string oldName = node.Name;
                object oldData = node.Tag;
                node.Tag = data.GetBytes();
                parent.NodeChanged(node, oldName, oldData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                Close();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonPrevPart_Click(object sender, EventArgs e)
        {
            partNumber--;
            DisplayPart();
        }

        private void buttonPart_Click(object sender, EventArgs e)
        {
            partNumber++;
            DisplayPart();
        }

        private void DisplayPart()
        {
            if (partNumber < 0)
                partNumber = 0;
            
            if (partNumber >= data.Parts.Count)
                partNumber = data.Parts.Count - 1;

            if (data.Parts.Count == 0)
            {
                this.textBox2.Text = "No parts available";
                return;
            }

            this.textBox2.Text = String.Format("{0:00} of {1:00}", partNumber+1, data.Parts.Count);
            this.textBoxParentName.Text = data.Parts[partNumber].ParentName;
            this.textBoxChildName.Text = data.Parts[partNumber].ChildName;
            this.textBoxOriginX.Text = String.Format("{0:0.000000}", data.Parts[partNumber].OriginX);
            this.textBoxOriginY.Text = String.Format("{0:0.000000}", data.Parts[partNumber].OriginY);
            this.textBoxOriginZ.Text = String.Format("{0:0.000000}", data.Parts[partNumber].OriginZ);
            this.textBoxOffsetX.Text = String.Format("{0:0.000000}", data.Parts[partNumber].OffsetX);
            this.textBoxOffsetY.Text = String.Format("{0:0.000000}", data.Parts[partNumber].OffsetY);
            this.textBoxOffsetZ.Text = String.Format("{0:0.000000}", data.Parts[partNumber].OffsetZ);
            this.textBoxRotMatXX.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatXX);
            this.textBoxRotMatXY.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatXY);
            this.textBoxRotMatXZ.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatXZ);
            this.textBoxRotMatYX.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatYX);
            this.textBoxRotMatYY.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatYY);
            this.textBoxRotMatYZ.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatYZ);
            this.textBoxRotMatZX.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatZX);
            this.textBoxRotMatZY.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatZY);
            this.textBoxRotMatZZ.Text = String.Format("{0:0.000000}", data.Parts[partNumber].RotMatZZ);

            this.textBoxAxisRotX.Text = String.Format("{0:0.000000}", data.Parts[partNumber].AxisRotX);
            this.textBoxAxisRotY.Text = String.Format("{0:0.000000}", data.Parts[partNumber].AxisRotY);
            this.textBoxAxisRotZ.Text = String.Format("{0:0.000000}", data.Parts[partNumber].AxisRotZ);

            this.textBoxUnknown.Text = String.Format("{0:0.000000}", data.Parts[partNumber].Unknown);
            this.textBoxAngle.Text = String.Format("{0:0.000000}", data.Parts[partNumber].Angle);

            double xaxisrot, zaxisrot, yaxisrot;
            if (data.Parts[partNumber].RotMatYX > 0.998)
            {
                yaxisrot = Math.Atan2(data.Parts[partNumber].RotMatXZ, data.Parts[partNumber].RotMatZZ);
                zaxisrot = Math.PI / 2;
                xaxisrot = 0;
            }
            else if (data.Parts[partNumber].RotMatYX < -0.998)
            {
                yaxisrot = Math.Atan2(data.Parts[partNumber].RotMatXZ, data.Parts[partNumber].RotMatZZ);
                zaxisrot = -Math.PI / 2;
                xaxisrot = 0;
            }
            else
            {
                xaxisrot = Math.Atan2(-data.Parts[partNumber].RotMatZX, data.Parts[partNumber].RotMatXX);
                yaxisrot = Math.Atan2(-data.Parts[partNumber].RotMatYZ, data.Parts[partNumber].RotMatYY);
                zaxisrot = Math.Asin(data.Parts[partNumber].RotMatYX);
            }

            this.textBoxRotAngleXAxis.Text = String.Format("{0:0.0}", Utilities.RadianToDegree(xaxisrot));
            this.textBoxRotAngleZAxis.Text = String.Format("{0:0.0}", Utilities.RadianToDegree(zaxisrot));
            this.textBoxRotAngleYAxis.Text = String.Format("{0:0.0}", Utilities.RadianToDegree(yaxisrot));
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;

                float result;
                if (!Single.TryParse(tb.Text, out result))
                {
                    tb.ForeColor = Color.Red;
                }
                else
                {
                    tb.ForeColor = TextBox.DefaultForeColor; ;
                }
            }
        }

        private void textBoxRotAngle_Changed(object sender, EventArgs e)
        {
            this.textBoxRotAngleXAxis.ForeColor = TextBox.DefaultForeColor;
            this.textBoxRotAngleZAxis.ForeColor = TextBox.DefaultForeColor;
            this.textBoxRotAngleYAxis.ForeColor = TextBox.DefaultForeColor;

            float heading, attitude, bank;
            if (!Single.TryParse(this.textBoxRotAngleXAxis.Text, out heading))
            {
                this.textBoxRotAngleXAxis.ForeColor = Color.Red;
                return;
            }

            if (!Single.TryParse(this.textBoxRotAngleZAxis.Text, out attitude))
            {
                this.textBoxRotAngleZAxis.ForeColor = Color.Red;
                return;
            }

            if (!Single.TryParse(this.textBoxRotAngleYAxis.Text, out bank))
            {
                this.textBoxRotAngleYAxis.ForeColor = Color.Red;
                return;
            }

            double ch = Math.Cos(Utilities.DegreeToRadian(heading));
            double sh = Math.Sin(Utilities.DegreeToRadian(heading));
            double ca = Math.Cos(Utilities.DegreeToRadian(attitude));
            double sa = Math.Sin(Utilities.DegreeToRadian(attitude));
            double cb = Math.Cos(Utilities.DegreeToRadian(bank));
            double sb = Math.Sin(Utilities.DegreeToRadian(bank));

            double m00 = ch * ca;
            double m01 = sh * sb - ch * sa * cb;
            double m02 = ch * sa * sb + sh * cb;
            double m10 = sa;
            double m11 = ca * cb;
            double m12 = -ca * sb;
            double m20 = -sh * ca;
            double m21 = sh * sa * cb + ch * sb;
            double m22 = -sh * sa * sb + ch * cb;

            data.Parts[partNumber].RotMatXX = (float)m00;
            data.Parts[partNumber].RotMatXY = (float)m01;
            data.Parts[partNumber].RotMatXZ = (float)m02;
            data.Parts[partNumber].RotMatYX = (float)m10;
            data.Parts[partNumber].RotMatYY = (float)m11;
            data.Parts[partNumber].RotMatYZ = (float)m12;
            data.Parts[partNumber].RotMatZX = (float)m20;
            data.Parts[partNumber].RotMatZY = (float)m21;
            data.Parts[partNumber].RotMatZZ = (float)m22;

            DisplayPart();
        }
    }
}
