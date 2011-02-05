using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    /// <summary>
    /// A text box that only accepts float values.
    /// </summary>
    public class FloatBox : TextBox
    {
        public FloatBox()
        {
            Text = "0";
        }

        [Browsable(false)]
        [DefaultValue("0")]
        public override string Text
        {
            get 
            { 
		        return base.Text;
	        }
            set 
	        {
                float num;
                if (Single.TryParse(value, out num))
                {
                    base.Text = value;
                    ForeColor = DefaultForeColor;
                }
	        }
        }

        private string format = "g";

        [Category("Appearance")]
        [Description("The format to display the value.")]
        [DefaultValue("g")]
        public string FormatString
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        [Category("Appearance")]
        [Description("The value of the control.")]
        [DefaultValue(0f)]
        public float Value
        {
            get
            {
                float value;
                Single.TryParse(Text, out value);
                return value;
            }
            set
            {
                Text = value.ToString(format);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            // Since we can't seem to get the caret position, if there's an
            // actual selection, just replace it.
            if (SelectionLength == 0 && !UTFEditor.Utilities.ValidFloatChar(Text, e.KeyChar, SelectionStart))
                e.Handled = true;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Only generate the event on valid data.
            if (ValidFloat())
                base.OnTextChanged(e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!ValidFloat())
                e.Cancel = true;
        }

        private bool ValidFloat()
        {
            float val;
            if (TextLength == 0 || Single.TryParse(Text, out val))
            {
                ForeColor = DefaultForeColor;
                return true;
            }
            ForeColor = Color.Red;
            return false;
        }
    }
}
