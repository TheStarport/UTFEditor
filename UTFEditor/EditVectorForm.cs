using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class EditVectorForm : Form
    {
        float ParseOrZero(string s)
        {
            if (!Single.TryParse(s, out float f))
                return 0.0f;

            return f;
        }

        public float X => ParseOrZero(textBox1.Text);
        public float Y => ParseOrZero(textBox2.Text);
        public float Z => ParseOrZero(textBox3.Text);
        
        public EditVectorForm(float x, float y, float z)
        {
            InitializeComponent();

            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
            textBox3.Text = z.ToString();
        }
    }
}
