using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class EditIntArrayForm : Form
    {
        TreeNode node;
        public EditIntArrayForm(TreeNode node, bool hex)
        {
            this.node = node;
            
            InitializeComponent();
            
            byte[] data = node.Tag as byte[];
            StringBuilder sb = new StringBuilder(data.Length * 3 / 2);
            string format = (hex) ? "0x{0:X}" : "{0}";
            for (int i = 0; i < data.Length; i += 4)
            {
                sb.AppendLine(String.Format(format, BitConverter.ToUInt32(data, i)));
            }
            textBox1.Text = sb.ToString();
            textBox1.Select(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int line = 0;
            try
            {
                List<byte> dataBlock = new List<byte>(textBox1.Lines.Length * 4);
                foreach (string value in textBox1.Lines)
                {
                    string num = value.Trim();
                    if (num.Length > 0)
                    {
                        if (num.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            dataBlock.AddRange(BitConverter.GetBytes(Int32.Parse(num.Substring(2), NumberStyles.AllowHexSpecifier)));
                        else
                            dataBlock.AddRange(BitConverter.GetBytes(Int32.Parse(num, NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands)));
                    }
                    ++line;
                }
                node.Tag = dataBlock.ToArray();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Invalid data");
                textBox1.Select(textBox1.GetFirstCharIndexFromLine(line), 0);
                textBox1.Select();
            }
        }
    }
}
