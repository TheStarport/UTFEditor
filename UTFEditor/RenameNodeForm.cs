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
    public partial class RenameNodeForm : Form
    {
        UTFForm parent;
        TreeNode node;
        public RenameNodeForm(UTFForm parent, TreeNode node)
        {
            this.parent = parent;
            this.node = node;
            InitializeComponent();
            textBox1.Text = node.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldName = node.Name;
            object oldData = node.Tag;
            this.node.Text = textBox1.Text;
            this.node.Name = textBox1.Text;
            parent.NodeChanged(node, oldName, oldData);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {   
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                button1_Click(sender, null);
        }
    }
}
