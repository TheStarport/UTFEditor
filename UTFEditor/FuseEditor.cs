using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace UTFEditor
{
    public partial class FuseEditor : Form
    {
        public FuseEditor()
        {
            InitializeComponent();
            panelEffect.Visible = false;

            comboType.Items.Clear();

            IEnumerable<TimelineEvent.EType> eTypes = EnumExtend.EnumToList<TimelineEvent.EType>();
            foreach (TimelineEvent.EType t in eTypes)
                comboType.Items.Add(t.DescString());
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
                case "start_effect":
                    panelEffect.Visible = true;
                    break;
            }
        }

        private TimelineEvent current = null;

        private void timeline1_ItemAdd(object sender, Timeline.ItemAddEventArgs e)
        {
            current = new TimelineEvent();

            current.Timings.Add(e.At);
            timeline1.Items.Add(e.At, current);

            RefreshUI();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            current = new TimelineEvent();

            RefreshUI();
        }

        private void RefreshUI()
        {
            bool e = current != null;

            comboType.Enabled = e;
            if(e) comboType.SelectedIndex = comboType.Items.IndexOf(current.Type.DescString());
        }
    }

    public class TimelineEvent
    {
        public enum EType
        {
            [Description("start_effect")]
            EFFECT
        }

        public struct HardpointData
        {
            public string Name;
            public float X, Y, Z;
            public float RotX, RotY, RotZ;
        }

        public EType Type = EType.EFFECT;

        // Type EFFECT
        public EffectData Effect;
        public bool Attached = false;
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

        public struct EffectDataKeyframe
        {
            public float Time;

            public float X, Y, Z;
            public float RotX, RotY, RotZ;
            public float StretchX, StretchY, StretchZ;
        }

        public string Name;

        public EShape Shape;

        public List<EffectDataKeyframe> Keyframes = new List<EffectDataKeyframe>();
    }

    // Largely based from code by Luke Foust
    // http://blog.spontaneouspublicity.com/post/2008/01/17/Associating-Strings-with-enums-in-C.aspx

    public static class EnumExtend
    {
        public static string DescString(this Enum e)
        {
            FieldInfo fi = e.GetType().GetField(e.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return e.ToString();
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}
