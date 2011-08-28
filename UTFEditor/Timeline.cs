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
        public class Event
        {
            public List<float> At = new List<float>();
            public object Data;
            public string Text;

            public Event(params float[] ats)
            {
                At = ats.ToList<float>();
            }

            public Event(object data, params float[] ats)
            {
                At = ats.ToList<float>();
                Data = data;
            }

            public Event(object data, string text, params float[] ats)
            {
                At = ats.ToList<float>();
                Data = data;
                Text = text;
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

        public Event SelectedEvent
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        public List<Event> Items
        {
            get
            {
                return events;
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

            events.Add(new Event(null, "ABC", 0.1f));
            events.Add(new Event(null, 0.5f));
            events.Add(new Event(null, 0.8f));
            events.Add(new Event(null, 0.675f, 0.25f));
        }

        int eventCursorHeight, measureDistance, smallMeasureHeight, medMeasureHeight, largeMeasureHeight, leftMargin, effectiveWidth;

        private void UpdateDisplayVars()
        {
            int w = this.Width;
            int h = this.Height;

            if (w < h)
            {
                w = this.Height;
                h = this.Width;
            }

            eventCursorHeight = (int)Math.Round(h * 0.6);

            measureDistance = (int)Math.Floor(((float)w) / 100);

            smallMeasureHeight = (int)Math.Round(h * 0.2);
            medMeasureHeight = (int)Math.Round(h * 0.25);
            largeMeasureHeight = (int)Math.Round(h * 0.4);

            effectiveWidth = measureDistance * 100;

            leftMargin = (w - effectiveWidth) / 2;
        }

        private Point MakePoint(int x, int y)
        {
            if (this.Width > this.Height)
                return new Point(x, y);
            else
                return new Point(y, x);
        }

        private PointF MakePoint(float x, float y)
        {
            if (this.Width > this.Height)
                return new PointF(x, y);
            else
                return new PointF(y, x);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(this.BackColor);

            int w = this.Width;
            int h = this.Height;

            if (w < h)
            {
                w = this.Height;
                h = this.Width;
            }
            

            foreach (Event ev in events)
            {
                Pen evp = selected == ev ? selectedPen : eventPen;
                foreach (float f in ev.At)
                {
                    e.Graphics.DrawLine(evp, MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), h), MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), eventCursorHeight));
                    if (ev.Text != null)
                        e.Graphics.DrawString(ev.Text, font, selected == ev ? selectedBrush : eventBrush, MakePoint(f * effectiveWidth + leftMargin, eventCursorHeight));
                }
            }

            for (int a = leftMargin; a <= effectiveWidth + leftMargin; a += measureDistance)
            {
                bool bigline = (a - leftMargin) % (measureDistance * 10) == 0;
                bool medline = (a - leftMargin) % (measureDistance * 2) == 0;
                Point pt = MakePoint(a, bigline ? largeMeasureHeight : (medline ? medMeasureHeight : smallMeasureHeight));
                e.Graphics.DrawLine(bigline ? primaryPen : secondaryPen, MakePoint(a, 0), pt);
                if (bigline)
                    e.Graphics.DrawString((((float)a - leftMargin) / effectiveWidth).ToString("0.###"), font, primaryBrush, new PointF((float)pt.X, pt.Y * 1.05f), drawFormat);
                else if(medline)
                    e.Graphics.DrawString((((float)a - leftMargin) / effectiveWidth * 100 % 10).ToString("#"), fontSmall, primaryBrush, new PointF((float)pt.X, pt.Y * 1.05f), drawFormat);
            }

            if (cursor && mouseLoc != Int32.MinValue)
                e.Graphics.DrawLine(highlightPen, MakePoint(mouseLoc, 0), MakePoint(mouseLoc, h));
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            UpdateDisplayVars();

            Invalidate();
        }

        int mouseLoc = Int32.MinValue;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int loc = mouseLoc = this.Width > this.Height ? e.X : e.Y;

            if ((ModifierKeys & Keys.Shift) == Keys.None)
            {
                if (mouseLoc >= leftMargin - measureDistance / 2 && mouseLoc <= effectiveWidth + leftMargin + measureDistance / 2)
                {
                    int diff = (loc - leftMargin) % measureDistance;
                    if (diff > measureDistance / 2) diff -= measureDistance;

                    if (Math.Abs(diff) <= 0.2 * measureDistance) mouseLoc -= diff;
                }
            }

            if (held != null)
            {
                int timespan = DateTime.Now.Subtract(holdTime).Milliseconds;

                if (timespan > 200)
                {
                    held.At[heldAt] = ((float)mouseLoc - leftMargin) / effectiveWidth;
                }
            }

            Invalidate();
        }

        bool cursor;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            cursor = true;

            Cursor.Hide();

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            cursor = false;

            mouseLoc = Int32.MinValue;
            held = null;

            Cursor.Show();

            Invalidate();
        }

        DateTime holdTime;
        Event held;
        int heldAt;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            holdTime = DateTime.Now;

            int loc = this.Width > this.Height ? e.X : e.Y;

            float mindist = Single.MaxValue;
            selected = null;
            held = null;

            foreach (Event ev in events)
            {
                int t = 0;
                foreach (float f in ev.At)
                {
                    float testdist = Math.Abs(((float)loc - leftMargin) / effectiveWidth - f);
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
                if ((ModifierKeys & Keys.Control) != Keys.None)
                {
                    float loc = ((float)mouseLoc - leftMargin) / effectiveWidth;
                    Event newEv = new Event(loc);

                    OnItemAdded(new ItemAddEventArgs(newEv));
                }
                else
                    selected = held;
            }

            held = null;

            OnMouseMove(e);
        }

        public class ItemAddEventArgs : EventArgs
        {
            public Event Item;

            public ItemAddEventArgs(Event i)
            {
                Item = i;
            }
        }

        protected void OnItemAdded(ItemAddEventArgs e)
        {
            ItemAdded(this, e);
        }

        public delegate void ItemAddedEventHandler(object sender, ItemAddEventArgs e);

        public event ItemAddedEventHandler ItemAdded;

    }
}
