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
    public partial class ReplaceAll : Form
    {
        public ReplaceAll()
        {
            InitializeComponent();
        }

        public string Find
        {
            get
            {
                return txtFind.Text;
            }
        }

        public string Replace
        {
            get
            {
                return txtReplace.Text;
            }
        }

        public bool WholeWord
        {
            get
            {
                return chkWholeWord.Checked;
            }
        }

        public bool MatchContent
        {
            get
            {
                return rdoMatchAll.Checked || rdoMatchContent.Checked;
            }
        }

        public bool MatchName
        {
            get
            {
                return rdoMatchAll.Checked || rdoMatchName.Checked;
            }
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            if (txtFind.Text.Length > 0 && txtReplace.Text.Length > 0)
                btnReplaceAll.Enabled = true;
            else
                btnReplaceAll.Enabled = false;
        }

        private void txtReplace_TextChanged(object sender, EventArgs e)
        {
            if (txtFind.Text.Length > 0 && txtReplace.Text.Length > 0)
                btnReplaceAll.Enabled = true;
            else
                btnReplaceAll.Enabled = false;
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
