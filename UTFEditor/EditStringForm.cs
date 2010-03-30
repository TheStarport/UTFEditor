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
    public partial class EditStringForm : Form
    {
        TreeNode node;
        public EditStringForm(TreeNode node)
        {
            this.node = node;
            InitializeComponent();
            byte[] data = node.Tag as byte[];
            textBox1.Text = System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            node.Tag = ASCIIEncoding.ASCII.GetBytes(textBox1.Text);
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
