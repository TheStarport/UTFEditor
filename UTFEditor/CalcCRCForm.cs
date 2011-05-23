using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class CalcCRCForm : Form
    {
        TreeNode node;
        public CalcCRCForm()
        {           
            InitializeComponent();           
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            string st = textBox1.Text + "\r\n";
            st += String.Format(" model crc = {0:X8}\r\n", Utilities.FLModelCRC(textBox1.Text));
            st += String.Format(" id hash= {0:X8}\r\n", Utilities.CreateID(textBox1.Text));

            textBox3.Text = st + textBox3.Text;
        }
    }
}
