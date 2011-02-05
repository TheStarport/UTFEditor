using System;
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
            textBox1.Text = Encoding.ASCII.GetString(data);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text + "\u0000";
            node.Tag = Encoding.ASCII.GetBytes(text);
        }
    }
}
