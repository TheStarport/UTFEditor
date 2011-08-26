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

            this.Cursor = Cursors.Cross;

            events.Add(new Event(new float[] { 0.1f }));
            events.Add(new Event(new float[] { 0.5f }));
            events.Add(new Event(new float[] { 0.9f }));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(this.BackColor);

            int w = this.Width;
            int h = this.Height;

            int maxline = (int)Math.Round(h * 0.6);

            int dist = (int)Math.Floor(((float)w) / 100);

            int minline = (int)Math.Round(h * 0.2);
            int minlinemed = (int)Math.Round(h * 0.25);
            int minlinebig = (int)Math.Round(h * 0.4);

            int maxdist = dist * 100;

            int newmin = (w - maxdist) / 2;

            foreach (Event ev in events)
            {
                Pen evp = selected == ev ? selectedPen : eventPen;
                foreach(float f in ev.At)
                    e.Graphics.DrawLine(evp, new Point((int)Math.Round(f * maxdist + newmin), h), new Point((int)Math.Round(f * maxdist + newmin), maxline));
            }

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

            if (mouseX != Int32.MinValue)
                e.Graphics.DrawLine(highlightPen, new Point(mouseX, 0), new Point(mouseX, h));
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            Invalidate();
        }

        int mouseX = Int32.MinValue;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mouseX = e.X;

            if ((ModifierKeys & Keys.Control) == Keys.None)
            {

                int dist = (int)Math.Floor(((float)this.Width) / 100);

                int maxdist = dist * 100;

                int newmin = (this.Width - maxdist) / 2;
                if (mouseX >= newmin - dist / 2 && mouseX <= maxdist + newmin + dist / 2)
                {

                    int diff = (e.X - newmin) % dist;
                    if (diff > dist / 2) diff -= dist;

                    if (Math.Abs(diff) <= 0.2 * dist) mouseX -= diff;
                }
            }

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            mouseX = Int32.MinValue;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {

            int dist = (int)Math.Floor(((float)this.Width) / 100);

            int maxdist = dist * 100;

            int newmin = (this.Width - maxdist) / 2;

            base.OnMouseDown(e);

            float mindist = Single.MaxValue;
            selected = null;

            foreach (Event ev in events)
            {
                foreach (float f in ev.At)
                {
                    float testdist = Math.Abs(((float)e.X - newmin) / maxdist - f);
                    if (testdist < mindist && testdist <= 0.05f)
                    {
                        selected = ev;
                        mindist = testdist;
                    }
                }
            }
        }
    }
}
