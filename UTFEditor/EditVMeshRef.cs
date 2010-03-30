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
    public partial class EditVMeshRef : Form
    {
        UTFForm parent;
        VMeshRef data;
        TreeNode node;

        public EditVMeshRef(UTFForm parent, TreeNode node)
        {
            this.parent = parent;
            this.node = node;
            InitializeComponent();
        }

        private void EditVMeshRef_Load(object sender, EventArgs e)
        {
            try
            {
                data = new VMeshRef(node.Tag as byte[]);
                textBoxHdrSize.Text = String.Format("0x{0:X}", data.HeaderSize);
                textBoxHdrSize.ReadOnly = true;
                textBoxVMeshLibId.Text = String.Format("0x{0:X8}", data.VMeshLibId);
                textBoxVMeshLibId.ReadOnly = true;
                textBoxStartMesh.Text = data.StartMesh.ToString();
                textBoxStartMesh.ReadOnly = true;
                textBoxNumMeshes.Text = data.NumMeshes.ToString();
                textBoxNumMeshes.ReadOnly = true;
                
                textBoxStartVert.Text = data.StartVert.ToString();
                textBoxStartVert.ReadOnly = true;
                NumVert.Text = data.NumVert.ToString();
                NumVert.ReadOnly = true;

                textBoxStartRefVertices.Text = data.StartRefVert.ToString();
                textBoxStartRefVertices.ReadOnly = true;
                textBoxNumRefVertices.Text = data.NumRefVert.ToString();
                textBoxNumRefVertices.ReadOnly = true;

                textBoxBBMaxX.Text = String.Format("{0:0.000000}", data.BoundingBoxMaxX);
                textBoxBBMaxY.Text = String.Format("{0:0.000000}", data.BoundingBoxMaxY);
                textBoxBBMaxZ.Text = String.Format("{0:0.000000}", data.BoundingBoxMaxZ);
                textBoxBBMinX.Text = String.Format("{0:0.000000}", data.BoundingBoxMinX);
                textBoxBBMinY.Text = String.Format("{0:0.000000}", data.BoundingBoxMinY);
                textBoxBBMinZ.Text = String.Format("{0:0.000000}", data.BoundingBoxMinZ);
                textBoxCenterX.Text = String.Format("{0:0.000000}", data.CenterX);
                textBoxCenterY.Text = String.Format("{0:0.000000}", data.CenterY);
                textBoxCenterZ.Text = String.Format("{0:0.000000}", data.CenterZ);
                textBoxRadius.Text = String.Format("{0:0.000000}", data.Radius);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                Close();
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                data.BoundingBoxMaxX = Single.Parse(textBoxBBMaxX.Text);
                data.BoundingBoxMaxY = Single.Parse(textBoxBBMaxY.Text);
                data.BoundingBoxMaxZ = Single.Parse(textBoxBBMaxZ.Text);
                data.BoundingBoxMinX = Single.Parse(textBoxBBMinX.Text);
                data.BoundingBoxMinY = Single.Parse(textBoxBBMinY.Text);
                data.BoundingBoxMinZ = Single.Parse(textBoxBBMinZ.Text);
                data.CenterX = Single.Parse(textBoxCenterX.Text);
                data.CenterY = Single.Parse(textBoxCenterY.Text);
                data.CenterZ = Single.Parse(textBoxCenterZ.Text);
                data.Radius = Single.Parse(textBoxRadius.Text);

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

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;

                if (tb.Text.StartsWith("0x"))
                {
                    uint result;
                    if (!UInt32.TryParse(tb.Text, System.Globalization.NumberStyles.AllowHexSpecifier,
                        System.Globalization.CultureInfo.CurrentCulture, out result))
                    {
                        tb.ForeColor = Color.Red;
                    }
                    else
                    {
                        tb.ForeColor = TextBox.DefaultForeColor;
                    }
                }
                else
                {
                    float result;
                    if (!Single.TryParse(tb.Text, out result))
                    {
                        tb.ForeColor = Color.Red;
                    }
                    else
                    {
                        tb.ForeColor = TextBox.DefaultForeColor;
                    }
                }
            }
        }
    }
}
