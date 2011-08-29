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
    public partial class FuseEditor : Form
    {
        public FuseEditor()
        {
            InitializeComponent();
        }

        private void btnFlip_Click(object sender, EventArgs e)
        {
            if (splitFuseEditor.Orientation == Orientation.Vertical)
                splitFuseEditor.Orientation = Orientation.Horizontal;
            else
                splitFuseEditor.Orientation = Orientation.Vertical;
        }

        private void splitFuseEditor_SplitterMoved(object sender, SplitterEventArgs e)
        {
            timeline1.Focus();
        }

        private void FuseEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                e.SuppressKeyPress = true;
                switch (e.KeyCode)
                {
                    case Keys.Space:
                        timeline1.Focus();
                        break;
                    case Keys.Down:
                    case Keys.Right:
                        timeline1.SelectNext();
                        break;
                    case Keys.Up:
                    case Keys.Left:
                        timeline1.SelectPrevious();
                        break;
                    default:
                        e.SuppressKeyPress = false;
                        break;
                }
            }
        }
    }
}
