using System;
using System.Drawing;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class EditHardpointData : Form
    {
        UTFForm parent;
        TreeNode node;
        HardpointData data;
        bool revolute;

        bool on_orient = false, on_rotate = false;

        public EditHardpointData(UTFForm parent, TreeNode node)
        {
            this.parent = parent;
            this.node = node;
            InitializeComponent();
            this.Text = node.Name + " - " + System.IO.Path.GetFileName(parent.fileName);
        }

        private void EditHardpointData_Load(object sender, EventArgs e)
        {
            try
            {
                data = new HardpointData(node);
                revolute = Utilities.StrIEq(node.Parent.Name, "Revolute");
                DisplayHardpoint();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                Close();
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            data.PosX     = floatBoxOriginX.Value;
            data.PosY     = floatBoxOriginY.Value;
            data.PosZ     = floatBoxOriginZ.Value;
            data.RotMatXX = floatBoxRotMatXX.Value;
            data.RotMatXY = floatBoxRotMatXY.Value;
            data.RotMatXZ = floatBoxRotMatXZ.Value;
            data.RotMatYX = floatBoxRotMatYX.Value;
            data.RotMatYY = floatBoxRotMatYY.Value;
            data.RotMatYZ = floatBoxRotMatYZ.Value;
            data.RotMatZX = floatBoxRotMatZX.Value;
            data.RotMatZY = floatBoxRotMatZY.Value;
            data.RotMatZZ = floatBoxRotMatZZ.Value;

            if (revolute)
            {
                if (floatBoxMin.Value > floatBoxMax.Value)
                {
                    float t = floatBoxMax.Value;
                    floatBoxMax.Value = floatBoxMin.Value;
                    floatBoxMin.Value = t;
                }

                data.AxisX = floatBoxAxisRotX.Value;
                data.AxisY = floatBoxAxisRotY.Value;
                data.AxisZ = floatBoxAxisRotZ.Value;
                data.Min   = floatBoxMin.Value;
                data.Max   = floatBoxMax.Value;
                if (checkBoxDegrees.Checked)
                {
                    data.Min = (float)Utilities.DegreeToRadian(data.Min);
                    data.Max = (float)Utilities.DegreeToRadian(data.Max);
                }
            }

            data.Write();
            parent.Modified(node);
            parent.RedrawModel();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DisplayHardpoint()
        {
            floatBoxOriginX.Value  = data.PosX;
            floatBoxOriginY.Value  = data.PosY;
            floatBoxOriginZ.Value  = data.PosZ;
            floatBoxRotMatXX.Value = data.RotMatXX;
            floatBoxRotMatXY.Value = data.RotMatXY;
            floatBoxRotMatXZ.Value = data.RotMatXZ;
            floatBoxRotMatYX.Value = data.RotMatYX;
            floatBoxRotMatYY.Value = data.RotMatYY;
            floatBoxRotMatYZ.Value = data.RotMatYZ;
            floatBoxRotMatZX.Value = data.RotMatZX;
            floatBoxRotMatZY.Value = data.RotMatZY;
            floatBoxRotMatZZ.Value = data.RotMatZZ;

            if (revolute)
            {
                floatBoxAxisRotX.Value = data.AxisX;
                floatBoxAxisRotY.Value = data.AxisY;
                floatBoxAxisRotZ.Value = data.AxisZ;
                floatBoxMin.Value = (float)Utilities.RadianToDegree(data.Min);
                floatBoxMax.Value = (float)Utilities.RadianToDegree(data.Max);
            }
            else
            {
                groupBoxRevolute.Visible = false;
            }

            DisplayRotation();
            on_orient = on_rotate = true;
        }

        private void DisplayRotation()
        {
            float pitch, yaw, roll;
            Utilities.OrientationToRotation(floatBoxRotMatXX.Value,
                                            floatBoxRotMatXY.Value,
                                            floatBoxRotMatXZ.Value,
                                            floatBoxRotMatYX.Value,
                                            floatBoxRotMatYY.Value,
                                            floatBoxRotMatYZ.Value,
                                            floatBoxRotMatZX.Value,
                                            floatBoxRotMatZY.Value,
                                            floatBoxRotMatZZ.Value,
                                            out pitch, out yaw, out roll);
            floatBoxRotAngleXAxis.Value = pitch;
            floatBoxRotAngleYAxis.Value = yaw;
            floatBoxRotAngleZAxis.Value = roll;
        }

        private void floatBoxOrient_Changed(object sender, EventArgs e)
        {
            if (on_orient)
            {
                on_rotate = false;
                DisplayRotation();
                on_rotate = true;
            }
        }
        
        private void floatBoxRotAngle_Changed(object sender, EventArgs e)
        {
            if (!on_rotate)
                return;

            float pitch, yaw, roll;
            pitch = floatBoxRotAngleXAxis.Value;
            yaw   = floatBoxRotAngleYAxis.Value;
            roll  = floatBoxRotAngleZAxis.Value;

            float[] ornt = new float[9];
            Utilities.RotationToOrientation(pitch, yaw, roll,
                                            out ornt[0], out ornt[1], out ornt[2],
                                            out ornt[3], out ornt[4], out ornt[5],
                                            out ornt[6], out ornt[7], out ornt[8]);

            on_orient = false;
            floatBoxRotMatXX.Value = ornt[0];
            floatBoxRotMatXY.Value = ornt[1];
            floatBoxRotMatXZ.Value = ornt[2];
            floatBoxRotMatYX.Value = ornt[3];
            floatBoxRotMatYY.Value = ornt[4];
            floatBoxRotMatYZ.Value = ornt[5];
            floatBoxRotMatZX.Value = ornt[6];
            floatBoxRotMatZY.Value = ornt[7];
            floatBoxRotMatZZ.Value = ornt[8];
            on_orient = true;
        }

        private void checkBoxDegrees_Click(object sender, EventArgs e)
        {
            if (checkBoxDegrees.Checked)
            {
                // It's become checked, convert radians to degrees.
                floatBoxMin.FormatString = floatBoxMax.FormatString = "0.##";
                floatBoxMin.Value = (float)Utilities.RadianToDegree(floatBoxMin.Value);
                floatBoxMax.Value = (float)Utilities.RadianToDegree(floatBoxMax.Value);
            }
            else
            {
                // It's become unchecked, convert degrees to radians.
                floatBoxMin.FormatString = floatBoxMax.FormatString = "g";
                floatBoxMin.Value = (float)Utilities.DegreeToRadian(floatBoxMin.Value);
                floatBoxMax.Value = (float)Utilities.DegreeToRadian(floatBoxMax.Value);
            }
        }
    }
}
