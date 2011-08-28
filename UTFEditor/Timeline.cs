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
        private class Event
        {
            public List<float> At = new List<float>();

            public Event(ICollection<float> ats)
            {
                At = ats.ToList<float>();
            }
        }

        List<Event> events = new List<Event>();
        Event selected = null;

        SolidBrush primaryBrush;
        SolidBrush secondaryBrush;
        SolidBrush eventBrush;
        SolidBrush highlightBrush;
        SolidBrush selectedBrush;

        Pen primaryPen;
        Pen secondaryPen;
        Pen eventPen;
        Pen highlightPen;
        Pen selectedPen;

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

        public Color SelectedColor
        {
            get
            {
                return selectedBrush.Color;
            }
            set
            {
                if (value == null) return;
                selectedBrush.Color = value;
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
            selectedBrush = new SolidBrush(SystemColors.HotTrack);

            primaryPen = new Pen(primaryBrush, 1.5f);
            secondaryPen = new Pen(secondaryBrush, 1.0f);
            eventPen = new Pen(eventBrush, 1.5f);
            highlightPen = new Pen(highlightBrush, 2.0f);
            selectedPen = new Pen(selectedBrush, 2.0f);

            UpdateDisplayVars();

            this.Cursor = Cursors.Cross;

            events.Add(new Event(new float[] { 0.1f }));
            events.Add(new Event(new float[] { 0.5f }));
            events.Add(new Event(new float[] { 0.9f }));
            events.Add(new Event(new float[] { 0.675f }));
        }

        int eventCursorHeight, measureDistance, smallMeasureHeight, medMeasureHeight, largeMeasureHeight, leftMargin, effectiveWidth;

        private void UpdateDisplayVars()
        {
            int w = this.Width;
            int h = this.Height;

            eventCursorHeight = (int)Math.Round(h * 0.6);

            measureDistance = (int)Math.Floor(((float)w) / 100);

            smallMeasureHeight = (int)Math.Round(h * 0.2);
            medMeasureHeight = (int)Math.Round(h * 0.25);
            largeMeasureHeight = (int)Math.Round(h * 0.4);

            effectiveWidth = measureDistance * 100;

            leftMargin = (w - effectiveWidth) / 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(this.BackColor);
            

            foreach (Event ev in events)
            {
                Pen evp = selected == ev ? selectedPen : eventPen;
                foreach(float f in ev.At)
                    e.Graphics.DrawLine(evp, new Point((int)Math.Round(f * effectiveWidth + leftMargin), this.Height), new Point((int)Math.Round(f * effectiveWidth + leftMargin), largeMeasureHeight));
            }

            for (int a = leftMargin; a <= effectiveWidth + leftMargin; a += measureDistance)
            {
                bool bigline = (a - leftMargin) % (measureDistance * 10) == 0;
                bool medline = (a - leftMargin) % (measureDistance * 2) == 0;
                Point pt = new Point(a, bigline ? largeMeasureHeight : (medline ? medMeasureHeight : smallMeasureHeight));
                e.Graphics.DrawLine(bigline? primaryPen : secondaryPen, new Point(a, 0), pt);
                if (bigline)
                    e.Graphics.DrawString((((float)a - leftMargin) / effectiveWidth).ToString("0.###"), font, primaryBrush, new PointF(pt.X, pt.Y * 1.05f), drawFormat);
                else if(medline)
                    e.Graphics.DrawString((((float)a - leftMargin) / effectiveWidth * 100 % 10).ToString("#"), fontSmall, primaryBrush, new PointF(pt.X, pt.Y * 1.05f), drawFormat);
            }

            if (mouseX != Int32.MinValue)
                e.Graphics.DrawLine(highlightPen, new Point(mouseX, 0), new Point(mouseX, this.Height));
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            UpdateDisplayVars();

            Invalidate();
        }

        int mouseX = Int32.MinValue;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mouseX = e.X;

            if ((ModifierKeys & Keys.Control) == Keys.None)
            {
                if (mouseX >= leftMargin - measureDistance / 2 && mouseX <= effectiveWidth + leftMargin + measureDistance / 2)
                {
                    int diff = (e.X - leftMargin) % measureDistance;
                    if (diff > measureDistance / 2) diff -= measureDistance;

                    if (Math.Abs(diff) <= 0.2 * measureDistance) mouseX -= diff;
                }
            }

            if (held != null)
            {
                int timespan = DateTime.Now.Subtract(holdTime).Milliseconds;

                if (timespan > 200)
                {
                    held.At[heldAt] = ((float)mouseX - leftMargin) / effectiveWidth;
                }
            }

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            mouseX = Int32.MinValue;
            held = null;
        }

        DateTime holdTime;
        Event held;
        int heldAt;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            holdTime = DateTime.Now;

            float mindist = Single.MaxValue;
            selected = null;
            held = null;

            foreach (Event ev in events)
            {
                int t = 0;
                foreach (float f in ev.At)
                {
                    float testdist = Math.Abs(((float)e.X - leftMargin) / effectiveWidth - f);
                    if (testdist < mindist && testdist <= 0.005f)
                    {
                        held = ev;
                        heldAt = t;
                        mindist = testdist;
                    }

                    t++;
                }
            }

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int timespan = DateTime.Now.Subtract(holdTime).Milliseconds;

            if (timespan <= 200)
            {
                selected = held;
            }

            held = null;

            OnMouseMove(e);
        }
    }
}
