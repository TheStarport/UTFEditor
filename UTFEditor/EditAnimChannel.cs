using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class EditAnimChannel : Form
    {
        TreeNode node;
        TreeNode nodeHeader;
        TreeNode nodeFrames;

        public EditAnimChannel(TreeNode node)
        {
            this.node = node;

            InitializeComponent();
        }

        private void EditAnimChannel_Load(object sender, EventArgs e)
        {
            try
            {
                nodeHeader = node.Nodes.Find("Header", true)[0];
                byte[] dataHeader = nodeHeader.Tag as byte[];

                int pos = 0;
                textBoxFrames.Text = Utilities.GetInt(dataHeader, ref pos).ToString();
                floatBoxInterval.Value = Utilities.GetFloat(dataHeader, ref pos);
                textBoxType.Text = Utilities.GetInt(dataHeader, ref pos).ToString();

                nodeFrames = node.Nodes.Find("Frames", true)[0];
                byte[] dataFrames = nodeFrames.Tag as byte[];
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dataFrames.Length; )
                {
                    sb.AppendLine(Utilities.GetFloat(dataFrames, ref i).ToString());
                }
                textBox1.Text = sb.ToString();
                //textBox1.Select(0, 0);
            }
            catch
            {
                MessageBox.Show(this, "Unable to parse Frames and Header nodes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                int frames = Int32.Parse(textBoxFrames.Text);
                float interval = floatBoxInterval.Value;
                int type = Int32.Parse(textBoxType.Text);

                List<byte> dataHeader = new List<byte>(12);
                dataHeader.AddRange(BitConverter.GetBytes(frames));
                dataHeader.AddRange(BitConverter.GetBytes(interval));
                dataHeader.AddRange(BitConverter.GetBytes(type));
                nodeHeader.Tag = dataHeader.ToArray();

                List<byte> dataBlock = new List<byte>();
                foreach (string value in textBox1.Lines)
                {
                    if (value.Trim().Length > 0)
                        dataBlock.AddRange(BitConverter.GetBytes(Single.Parse(value)));
                }
                nodeFrames.Tag = dataBlock.ToArray();

                if (frames * 8 != dataBlock.Count)
                    throw new Exception("Number of frames in channel data does not match channel header.");

                Close();
                (node.TreeView.FindForm() as UTFForm).Modified(node);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonGenData_Click(object sender, EventArgs e)
        {
            int frames;
            if (!Int32.TryParse(textBoxFrames.Text, out frames))
            {
                MessageBox.Show(this, "Frames is not a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            float time = floatBoxGenTime.Value / frames;
            float distance = floatBoxGenDistance.Value / frames;

            textBox1.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= frames; i++)
            {
                float curTime = time * i;
                float curDist = distance * i;
                sb.AppendFormat("{0:g}\r\n", curTime);
                sb.AppendFormat("{0:g}\r\n", curDist);
            }
            textBox1.Text = sb.ToString();
            //textBox1.Select(0, 0);
        }
    }
}
