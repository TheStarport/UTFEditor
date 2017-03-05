using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class ModelImporterOptions : Form
    {
        public bool Wireframe { get; private set; } = false;
        public bool Relocate { get; private set; } = false;
        public ModelImportVertexType VertexType { get; private set; } = ModelImportVertexType.Normals;
        public string UniqueName { get; private set; } = null;

        public ModelImporterOptions()
        {
            InitializeComponent();

            lstVertexType.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            VertexType = (ModelImportVertexType)lstVertexType.SelectedIndex;
            Wireframe = chkWireframe.Checked;
            Relocate = chkRelocate.Checked;
            UniqueName = txtUniqueName.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
