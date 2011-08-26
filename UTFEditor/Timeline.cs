using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UTFEditor
{
    class Timeline : Panel
    {

        List<float> times = new List<float>();

        SolidBrush primaryBrush;
        SolidBrush secondaryBrush;
        SolidBrush eventBrush;
        SolidBrush highlightBrush;

        Pen primaryPen;
        Pen secondaryPen;
        Pen eventPen;
        Pen highlightPen;

        Font font = new Font("Arial", 8);
        Font fontSmall = new Font("Arial", 7);

        StringFormat drawFormat = new StringFormat();

        public new Color ForeColor
        {
            get
            {
                return primaryBrush.Color;
            }
            set
            {
                if (value == null) return;
                primaryBrush.Color = value;
            }
        }

        public Color SecondaryForeColor
        {
            get
            {
                return secondaryBrush.Color;
            }
            set
            {
                if (value == null) return;
                secondaryBrush.Color = value;
            }
        }

        public Color EventColor
        {
            get
            {
                return eventBrush.Color;
            }
            set
            {
                if (value == null) return;
                eventBrush.Color = value;
            }
        }

        public Color HighlightColor
        {
            get
            {
                return highlightBrush.Color;
            }
            set
            {
                if (value == null) return;
                highlightBrush.Color = value;
            }
        }

        public Timeline()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            drawFormat.Alignment = StringAlignment.Center;

            primaryBrush = new SolidBrush(SystemColors.ControlText);
            secondaryBrush = new SolidBrush(SystemColors.ControlText);
            eventBrush = new SolidBrush(SystemColors.ControlText);
            highlightBrush = new SolidBrush(SystemColors.Highlight);

            primaryPen = new Pen(primaryBrush, 1.5f);
            secondaryPen = new Pen(secondaryBrush, 1.0f);
            eventPen = new Pen(eventBrush, 1.5f);
            highlightPen = new Pen(highlightBrush, 2.0f);

            times.Add(0.1f);
            times.Add(0.3f);
            times.Add(0.8f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(this.BackColor);

            int w = this.Width;
            int h = this.Height;

            int maxline = (int)Math.Round(h * 0.5);

            foreach (float f in times)
            {
                e.Graphics.DrawLine(eventPen, new Point((int)Math.Round(f * w + w), h), new Point((int)Math.Round(f * w + w), maxline));
            }

            int dist = (int)Math.Floor(((float)w) / 100);

            int minline = (int)Math.Round(h * 0.2);
            int minlinemed = (int)Math.Round(h * 0.25);
            int minlinebig = (int)Math.Round(h * 0.4);

            int maxdist = dist * 100;

            int newmin = (w - maxdist) / 2;

            Font font = new Font("Arial", 8);

            for (int a = newmin; a <= maxdist + newmin; a += dist)
            {
                bool bigline = (a - newmin) % (dist * 10) == 0;
                bool medline = (a - newmin) % (dist * 2) == 0;
                Point pt = new Point(a, bigline ? minlinebig : (medline ? minlinemed : minline));
                e.Graphics.DrawLine(bigline? primaryPen : secondaryPen, new Point(a, 0), pt);
                if (bigline)
                    e.Graphics.DrawString((((float)a - newmin) / maxdist).ToString("0.###"), font, primaryBrush, new PointF(pt.X, pt.Y * 1.05f), drawFormat);
                else if(medline)
                    e.Graphics.DrawString((((float)a - newmin) / maxdist * 100 % 10).ToString("#"), fontSmall, primaryBrush, new PointF(pt.X, pt.Y * 1.05f), drawFormat);
            }

            if (mousePos.X != -1 && mousePos.Y != -1)
                e.Graphics.DrawLine(highlightPen, new Point(mousePos.X, 0), new Point(mousePos.X, h));
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            Invalidate();
        }

        Point mousePos = new Point(-1, -1);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mousePos = new Point(e.X, e.Y);

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            mousePos = new Point(-1, -1);
        }
    }
}
