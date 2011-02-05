using System;
using System.Windows.Forms;

namespace UTFEditor
{
	public partial class EditCmpRevData : Form
    {
        UTFForm parent;
        TreeNode node;
        CmpRevData data;
        int partNumber;

        bool on_orient = false, on_rotate = false;

		public EditCmpRevData(UTFForm parent, string type, TreeNode node)
        {
            this.parent = parent;
            this.node = node;
            InitializeComponent();
            this.Text = "Edit " + type + " Data";
            if (type == "Pris")
                checkBoxDegrees.Checked = checkBoxDegrees.Visible = false;
        }

        private void EditCmpRevData_Load(object sender, EventArgs e)
        {
            try
            {
                data = new CmpRevData(node.Tag as byte[]);
                if (data.Parts.Count == 0)
                    throw new Exception("No parts");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                Close();
                return;
            }

            labelPartCount.Text = String.Format("of {0}", data.Parts.Count);
            partUpDown.Maximum = data.Parts.Count;
            foreach (CmpRevData.Part part in data.Parts)
                comboBoxChildName.Items.Add(part.ChildName);
            comboBoxChildName.Select();
            partNumber = 0;
            DisplayPart();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                data.Parts[partNumber].ParentName = textBoxParentName.Text;
                comboBoxChildName.Items[partNumber] =
                data.Parts[partNumber].ChildName = comboBoxChildName.Text;
                data.Parts[partNumber].OriginX  = floatBoxOriginX.Value;
                data.Parts[partNumber].OriginY  = floatBoxOriginY.Value;
                data.Parts[partNumber].OriginZ  = floatBoxOriginZ.Value;
                data.Parts[partNumber].OffsetX  = floatBoxOffsetX.Value;
                data.Parts[partNumber].OffsetY  = floatBoxOffsetY.Value;
                data.Parts[partNumber].OffsetZ  = floatBoxOffsetZ.Value;
                data.Parts[partNumber].RotMatXX = floatBoxRotMatXX.Value;
                data.Parts[partNumber].RotMatXY = floatBoxRotMatXY.Value;
                data.Parts[partNumber].RotMatXZ = floatBoxRotMatXZ.Value;
                data.Parts[partNumber].RotMatYX = floatBoxRotMatYX.Value;
                data.Parts[partNumber].RotMatYY = floatBoxRotMatYY.Value;
                data.Parts[partNumber].RotMatYZ = floatBoxRotMatYZ.Value;
                data.Parts[partNumber].RotMatZX = floatBoxRotMatZX.Value;
                data.Parts[partNumber].RotMatZY = floatBoxRotMatZY.Value;
                data.Parts[partNumber].RotMatZZ = floatBoxRotMatZZ.Value;

                data.Parts[partNumber].AxisRotX = floatBoxAxisRotX.Value;
                data.Parts[partNumber].AxisRotY = floatBoxAxisRotY.Value;
                data.Parts[partNumber].AxisRotZ = floatBoxAxisRotZ.Value;

                data.Parts[partNumber].Min      = floatBoxMin.Value;
                data.Parts[partNumber].Max      = floatBoxMax.Value;
                if (checkBoxDegrees.Checked)
                {
                    data.Parts[partNumber].Min = (float)Utilities.DegreeToRadian(data.Parts[partNumber].Min);
                    data.Parts[partNumber].Max = (float)Utilities.DegreeToRadian(data.Parts[partNumber].Max);
                }

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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxChildName_SelectedIndexChanged(object sender, EventArgs e)
        {
            partUpDown.Value = comboBoxChildName.SelectedIndex + 1;
        }

        private void partUpDown_ValueChanged(object sender, EventArgs e)
        {
            partNumber = (int)partUpDown.Value - 1;
            DisplayPart();
        }

        private void DisplayPart()
        {
            textBoxParentName.Text = data.Parts[partNumber].ParentName;
            comboBoxChildName.Text = data.Parts[partNumber].ChildName;
            floatBoxOriginX.Value  = data.Parts[partNumber].OriginX;
            floatBoxOriginY.Value  = data.Parts[partNumber].OriginY;
            floatBoxOriginZ.Value  = data.Parts[partNumber].OriginZ;
            floatBoxOffsetX.Value  = data.Parts[partNumber].OffsetX;
            floatBoxOffsetY.Value  = data.Parts[partNumber].OffsetY;
            floatBoxOffsetZ.Value  = data.Parts[partNumber].OffsetZ;
            floatBoxRotMatXX.Value = data.Parts[partNumber].RotMatXX;
            floatBoxRotMatXY.Value = data.Parts[partNumber].RotMatXY;
            floatBoxRotMatXZ.Value = data.Parts[partNumber].RotMatXZ;
            floatBoxRotMatYX.Value = data.Parts[partNumber].RotMatYX;
            floatBoxRotMatYY.Value = data.Parts[partNumber].RotMatYY;
            floatBoxRotMatYZ.Value = data.Parts[partNumber].RotMatYZ;
            floatBoxRotMatZX.Value = data.Parts[partNumber].RotMatZX;
            floatBoxRotMatZY.Value = data.Parts[partNumber].RotMatZY;
            floatBoxRotMatZZ.Value = data.Parts[partNumber].RotMatZZ;

            floatBoxAxisRotX.Value = data.Parts[partNumber].AxisRotX;
            floatBoxAxisRotY.Value = data.Parts[partNumber].AxisRotY;
            floatBoxAxisRotZ.Value = data.Parts[partNumber].AxisRotZ;

            if (checkBoxDegrees.Checked)
            {
                floatBoxMin.Value = (float)Utilities.RadianToDegree(data.Parts[partNumber].Min);
                floatBoxMax.Value = (float)Utilities.RadianToDegree(data.Parts[partNumber].Max);
            }
            else
            {
                floatBoxMin.Value = data.Parts[partNumber].Min;
                floatBoxMax.Value = data.Parts[partNumber].Max;
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
            floatBoxPitch.Value = pitch;
            floatBoxYaw.Value = yaw;
            floatBoxRoll.Value = roll;
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
        
        private void floatBoxRot_Changed(object sender, EventArgs e)
        {
            if (!on_rotate)
                return;

            float pitch, yaw, roll;
            pitch = floatBoxPitch.Value;
            yaw   = floatBoxYaw.Value;
            roll  = floatBoxRoll.Value;

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
