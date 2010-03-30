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
    public partial class EditFloatArrayForm : Form
    {
        TreeNode node;
        public EditFloatArrayForm(TreeNode node)
        {
            this.node = node;
            
            InitializeComponent();
            
            byte[] data = node.Tag as byte[];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i+=4)
            {
                sb.AppendLine(BitConverter.ToSingle(data, i).ToString());
            }
            textBox1.Text = sb.ToString();
            textBox1.Select(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<byte> dataBlock = new List<byte>();
                byte[] data = new byte[0];
                foreach (string value in textBox1.Text.Split('\r'))
                {
                    if (value.Trim().Length>0)
                        dataBlock.AddRange(BitConverter.GetBytes(Single.Parse(value.Trim())));
                }
                node.Tag = dataBlock.ToArray();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Invalid data");
            }
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
