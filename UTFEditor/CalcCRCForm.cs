using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class CalcCRCForm : Form
    {
        public CalcCRCForm()
        {           
            InitializeComponent();           
        }

		private string last_string = string.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
			if (textBox1.Text.Length == 0 || textBox1.Text == last_string)
				return;
			last_string = textBox1.Text;

			StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.Append("\r\n");
            sb.AppendFormat("    model crc = 0x{0:X8}\r\n", Utilities.FLModelCRC(textBox1.Text));
            sb.AppendFormat("    id hash = 0x{0:X8}\r\n", Utilities.CreateID(textBox1.Text));

            textBox3.Text = sb.ToString() + textBox3.Text;
			textBox1.SelectAll();
        }

		private void CalcCRCForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
		}
    }
}
