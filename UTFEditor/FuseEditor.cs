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
            panelEffect.Visible = false;
        }

        private void btnFlip_Click(object sender, EventArgs e)
        {
            if (splitFuseEditor.Orientation == Orientation.Vertical)
                splitFuseEditor.Orientation = Orientation.Horizontal;
            else
                splitFuseEditor.Orientation = Orientation.Vertical;

            timeline1.Zoom = 1;
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

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            timeline1.PlayPause();
        }

        private void timeline1_Play(object sender, EventArgs e)
        {
            btnPlayPause.Image = global::UTFEditor.Properties.Resources.StopHS;
        }

        private void timeline1_Stop(object sender, EventArgs e)
        {
            btnPlayPause.Image = global::UTFEditor.Properties.Resources.PlayHS;
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelEffect.Visible = false;

            switch (comboType.SelectedItem.ToString())
            {
                case "Effect":
                    panelEffect.Visible = true;
                    break;
            }
        }

        private TimelineEvent current = null;

        private void timeline1_ItemAdd(object sender, Timeline.ItemAddEventArgs e)
        {

        }
    }

    public class TimelineEvent
    {
        public enum EType
        {
            EFFECT
        }

        public struct HardpointData
        {
            public string Name;
            public float X, Y, Z;
            public float RotX, RotY, RotZ;
        }

        public EType Type;

        // Type EFFECT
        public EffectData Effect;
        public bool Attached;
        public List<float> Timings = new List<float>();
        public List<HardpointData> Hardpoints = new List<HardpointData>();
    }

    public class EffectData
    {
        public enum EShape
        {
            SPHERE,
            CYLINDER,
            CONE,
            CUBE
        }

        public string Name;

        public EShape Shape;

        public float X, Y, Z;
        public float RotX, RotY, RotZ;
        public float StretchX, StretchY, StretchZ;
    }
}
