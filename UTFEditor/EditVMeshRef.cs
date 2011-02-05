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
            this.Text = parent.GetVMeshRefName(node);
        }

        private void EditVMeshRef_Load(object sender, EventArgs e)
        {
            try
            {
                data = new VMeshRef(node.Tag as byte[]);
                textBoxHdrSize.Text = String.Format("0x{0:X}", data.HeaderSize);
                textBoxVMeshLibId.Text = String.Format("0x{0:X8}", data.VMeshLibId);
                textBoxVMeshLibName.Text = parent.FindVMeshName(data.VMeshLibId, false);
                textBoxStartMesh.Text = data.StartMesh.ToString();
                textBoxNumMeshes.Text = data.NumMeshes.ToString();
                
                textBoxStartVert.Text = data.StartVert.ToString();
                NumVert.Text = data.NumVert.ToString();

                textBoxStartIndex.Text = data.StartIndex.ToString();
                textBoxNumIndices.Text = data.NumIndex.ToString();

                textBoxBBMaxX.Text  = data.BoundingBoxMaxX.ToString("g");
                textBoxBBMaxY.Text  = data.BoundingBoxMaxY.ToString("g");
                textBoxBBMaxZ.Text  = data.BoundingBoxMaxZ.ToString("g");
                textBoxBBMinX.Text  = data.BoundingBoxMinX.ToString("g");
                textBoxBBMinY.Text  = data.BoundingBoxMinY.ToString("g");
                textBoxBBMinZ.Text  = data.BoundingBoxMinZ.ToString("g");
                textBoxCenterX.Text = data.CenterX.ToString("g");
                textBoxCenterY.Text = data.CenterY.ToString("g");
                textBoxCenterZ.Text = data.CenterZ.ToString("g");
                textBoxRadius.Text  = data.Radius.ToString("g");
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
