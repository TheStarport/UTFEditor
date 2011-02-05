using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class EditColorForm : Form
    {
        TreeNode node;

        public EditColorForm(TreeNode node)
        {
            InitializeComponent();
            spinnerR.TextChanged += new EventHandler(spinnerRGB_TextChanged);
            spinnerG.TextChanged += new EventHandler(spinnerRGB_TextChanged);
            spinnerB.TextChanged += new EventHandler(spinnerRGB_TextChanged);

            this.node = node;
            byte[] data = node.Tag as byte[];
            float r = 0, g = 0, b = 0;
            try
            {
                int pos = 0;
                r = Utilities.GetFloat(data, ref pos);
                if (r < 0) r = 0; else if (r > 1) r = 1;
                g = Utilities.GetFloat(data, ref pos);
                if (g < 0) g = 0; else if (g > 1) g = 1;
                b = Utilities.GetFloat(data, ref pos);
                if (b < 0) b = 0; else if (b > 1) b = 1;
            }
            catch { }

            floatBoxR.Value = r;
            floatBoxG.Value = g;
            floatBoxB.Value = b;

            int ri = (int)(r * 255);
            int gi = (int)(g * 255);
            int bi = (int)(b * 255);
            spinnerR.Value = ri;
            spinnerG.Value = gi;
            spinnerB.Value = bi;
            textBoxHex.Text = String.Format("{0:X2}{1:X2}{2:X2}", ri, gi, bi);

            buttonColor.BackColor = Color.FromArgb(ri, gi, bi);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            List<byte> data = new List<byte>(12);
            data.AddRange(BitConverter.GetBytes(floatBoxR.Value));
            data.AddRange(BitConverter.GetBytes(floatBoxG.Value));
            data.AddRange(BitConverter.GetBytes(floatBoxB.Value));
            node.Tag = data.ToArray();
        }

        bool in_update = false;

        private void floatBoxRGB_TextChanged(object sender, EventArgs e)
        {
            if (!in_update)
            {
                in_update = true;
                UpdateInteger();
                UpdateColor();
                in_update = false;
            }
        }

        private void spinnerRGB_TextChanged(object sender, EventArgs e)
        {
            if (!in_update)
            {
                in_update = true;
                UpdateFloat();
                UpdateColor();
                in_update = false;
            }
        }

        private void textBoxHex_TextChanged(object sender, EventArgs e)
        {
            if (!in_update)
            {
                in_update = true;
                string hex = (sender as TextBox).Text;
                int r, g, b;
                try
                {
                    r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                    spinnerR.Value = r;
                    spinnerG.Value = g;
                    spinnerB.Value = b;
                    UpdateFloat();
                    buttonColor.BackColor = Color.FromArgb(r, g, b);
                    textBoxHex.ForeColor = TextBox.DefaultForeColor;
                }
                catch
                {
                    textBoxHex.ForeColor = Color.Red;
                }
                in_update = false;
            }
        }

        private void ItoF(NumericUpDown ival, FloatBox fval)
        {
            fval.Value = (float)ival.Value / 255;
        }

        private void UpdateFloat()
        {
            ItoF(spinnerR, floatBoxR);
            ItoF(spinnerG, floatBoxG);
            ItoF(spinnerB, floatBoxB);
        }

        private void FtoI(FloatBox fval, NumericUpDown ival)
        {
            float c = fval.Value;
            if (c < 0) 
                c = 0;
            else if (c > 1)
                c = 1;
            ival.Value = (int)(c * 255);
        }

        private void UpdateInteger()
        {
            FtoI(floatBoxR, spinnerR);
            FtoI(floatBoxG, spinnerG);
            FtoI(floatBoxB, spinnerB);
        }

        private void UpdateColor()
        {
            int ri = (int)spinnerR.Value;
            int gi = (int)spinnerG.Value;
            int bi = (int)spinnerB.Value;
            textBoxHex.Text = String.Format("{0:X2}{1:X2}{2:X2}", ri, gi, bi);
            buttonColor.BackColor = Color.FromArgb(ri, gi, bi);
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonColor.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                in_update = true;
                spinnerR.Value = colorDialog1.Color.R;
                spinnerG.Value = colorDialog1.Color.G;
                spinnerB.Value = colorDialog1.Color.B;
                UpdateFloat();
                in_update = false;
                buttonColor.BackColor = colorDialog1.Color;
            }
        }

        Stack<char> history = new Stack<char>(6);

        private void textBoxHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') ||
                (e.KeyChar >= 'A' && e.KeyChar <= 'F') ||
                (e.KeyChar >= 'a' && e.KeyChar <= 'f'))
            {
                // Force overwrite, but remember current character first,
                // using backspace as an undo.
                if (textBoxHex.SelectionStart != textBoxHex.TextLength)
                    history.Push(textBoxHex.Text[textBoxHex.SelectionStart]);
                textBoxHex.SelectionLength = 1;
            }
            else if (e.KeyChar == '\b')
            {
                if (textBoxHex.SelectionStart > 0)
                {
                    int pos = --textBoxHex.SelectionStart;
                    if (history.Count > 0)
                    {
                        // Can't write individual string characters, how dumb is that?
                        StringBuilder sb = new StringBuilder(textBoxHex.Text);
                        sb[pos] = history.Pop();
                        textBoxHex.Text = sb.ToString();
                        textBoxHex.SelectionStart = pos;
                    }
                    e.Handled = true;
                }
            }
            else if (e.KeyChar != 26 && // Ctrl+Z - Undo
                     e.KeyChar !=  3 && // Ctrl+C - Copy
                     e.KeyChar != 22 && // Ctrl+V - Paste
                     e.KeyChar != 24)   // Ctrl+X - Cut
            {
                e.Handled = true;
            }
        }

        private void textBoxHex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (!((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
                       (e.KeyCode >= Keys.A && e.KeyCode <= Keys.F) ||
                       e.KeyCode == Keys.Back))
            {
                history.Clear();
            }
        }

        private void textBoxHex_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            //int val;
            //if (tb.TextLength != 6 || !int.TryParse(tb.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out val))
            if (tb.ForeColor == Color.Red)
                e.Cancel = true;
        }
    }
}
