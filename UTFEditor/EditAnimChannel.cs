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
                textBoxFrames.Text = BitConverter.ToInt32(dataHeader, pos).ToString(); pos += 4;
                textBoxFrameUnknown1.Text = BitConverter.ToSingle(dataHeader, pos).ToString(); pos += 4;
                textBoxFrameUnknown2.Text = BitConverter.ToInt32(dataHeader, pos).ToString(); pos += 4;

                textBoxGenTime.Text = "5.0";
                textBoxGenDistance.Text = "100.0";

                nodeFrames = node.Nodes.Find("Frames", true)[0];
                byte[] dataFrames = nodeFrames.Tag as byte[];
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dataFrames.Length; i += 4)
                {
                    sb.AppendLine(BitConverter.ToSingle(dataFrames, i).ToString());
                }
                textBox1.Text = sb.ToString();
                textBox1.Select(0, 0);
            }
            catch
            {
                MessageBox.Show(this, "Error 'Unable to parse Frames and Header nodes", "Error");
                Close();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                int frames = Int32.Parse(textBoxFrames.Text);
                float unknown1 = Single.Parse(textBoxFrameUnknown1.Text);
                int unknown2 = Int32.Parse(textBoxFrameUnknown2.Text);

                List<byte> dataHeader = new List<byte>();
                dataHeader.AddRange(BitConverter.GetBytes(frames));
                dataHeader.AddRange(BitConverter.GetBytes(unknown1));
                dataHeader.AddRange(BitConverter.GetBytes(unknown2));
                nodeHeader.Tag = dataHeader.ToArray();

                List<byte> dataBlock = new List<byte>();
                foreach (string value in textBox1.Text.Split('\r'))
                {
                    if (value.Trim().Length > 0)
                        dataBlock.AddRange(BitConverter.GetBytes(Single.Parse(value.Trim())));
                }
                nodeFrames.Tag = dataBlock.ToArray();

                if (frames != (dataBlock.Count / 8))
                    throw new Exception("Number of frames in channel data does not match channel header");

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message+"'", "Error");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonGenData_Click(object sender, EventArgs e)
        {
            float frames = Single.Parse(textBoxFrames.Text);
            float time = Single.Parse(textBoxGenTime.Text);
            float distance = Single.Parse(textBoxGenDistance.Text);

            textBox1.Clear();
            StringBuilder sb = new StringBuilder();
            for (float i = 1; i <= frames; i++)
            {
                float curTime = (time / (frames)) * i;
                float curDist = (distance / (frames)) * i;
                sb.AppendFormat("{0:0.000000}\r\n", curTime);
                sb.AppendFormat("{0:0.000000}\r\n", curDist);
            }
            textBox1.Text = sb.ToString();
            textBox1.Select(0, 0);
        }
    }
}
