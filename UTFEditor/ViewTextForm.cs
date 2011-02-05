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
    public partial class ViewTextForm : Form
    {
        public ViewTextForm(string title, string text)
        {
            InitializeComponent();
            this.Text = title;
            if (text[0] == '{')
                richTextBox.Rtf = text;
            else
                richTextBox.Text = text;
            // Resize the form to the longest line, but no bigger than default.
            string[] lines = richTextBox.Lines;
            string max = "";
            foreach (string line in lines)
                if (line.Length > max.Length)
                    max = line;
            Size size = TextRenderer.MeasureText(max, richTextBox.Font);
            size.Width += new VScrollBar().Size.Width;
            if (size.Width < richTextBox.Size.Width)
                this.Size = new Size(this.Size.Width - richTextBox.Size.Width + size.Width, this.Size.Height);
        }

        private void checkBoxWrap_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox.WordWrap = checkBoxWrap.Checked;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = this.Text;
            saveFile.Filter = "Text (*.txt)|*.txt";
            saveFile.DefaultExt = "txt";
            if (saveFile.ShowDialog(this) == DialogResult.OK)
                richTextBox.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
