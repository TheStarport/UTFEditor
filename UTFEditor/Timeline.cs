using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UTFEditor
{
    class Timeline : ScrollableControl
    {
        public interface IEvent
        {
            List<float> At
            {
                get;
                set;
            }
        }

        List<IEvent> events = new List<IEvent>();
        IEvent selected = null;

        SolidBrush primaryBrush;
        SolidBrush secondaryBrush;
        SolidBrush eventBrush;
        SolidBrush highlightBrush;
        SolidBrush selectedBrush;
        SolidBrush backgroundBrush;
        SolidBrush playBrush;

        Pen primaryPen;
        Pen secondaryPen;
        Pen eventPen;
        Pen highlightPen;
        Pen selectedPen;
        Pen playPen;

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

        public Color SecondaryBackColor
        {
            get
            {
                return backgroundBrush.Color;
            }
            set
            {
                if (value == null) return;
                backgroundBrush.Color = value;
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

        public Color PlayColor
        {
            get
            {
                return playBrush.Color;
            }
            set
            {
                if (value == null) return;
                playBrush.Color = value;
            }
        }

        public IEvent SelectedEvent
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                held = null;
                heldAt = -1;
            }
        }

        public List<IEvent> Items
        {
            get
            {
                return events;
            }
        }

        float zoom = 1.0f;
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = Math.Max(1, value);
            }
        }

        public float Timespan = 5.0f;

        public new bool CanFocus
        {
            get
            {
                return true;
            }
        }

        public Timeline()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.AutoScroll = true;

            primaryBrush = new SolidBrush(SystemColors.ControlText);
            secondaryBrush = new SolidBrush(SystemColors.ControlText);
            eventBrush = new SolidBrush(SystemColors.ControlText);
            highlightBrush = new SolidBrush(SystemColors.Highlight);
            selectedBrush = new SolidBrush(SystemColors.HotTrack);
            backgroundBrush = new SolidBrush(SystemColors.ControlDark);
            playBrush = new SolidBrush(Color.Green);

            primaryPen = new Pen(primaryBrush, 1.5f);
            secondaryPen = new Pen(secondaryBrush, 1.0f);
            eventPen = new Pen(eventBrush, 1.5f);
            highlightPen = new Pen(highlightBrush, 2.0f);
            selectedPen = new Pen(selectedBrush, 2.0f);
            playPen = new Pen(playBrush, 2.0f);

            playback.Tick += new EventHandler(playback_Tick);

            UpdateDisplayVars();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if ((ModifierKeys & Keys.Control) != Keys.None)
            {
                zoom += e.Delta / 200.0f;
                zoom = Math.Max(1, zoom);

                UpdateDisplayVars();
                Invalidate();
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            UpdateDisplayVars();
            Invalidate();
        }

        int eventCursorHeight, backgroundCursorHeight, measureDistance, smallMeasureHeight, medMeasureHeight, largeMeasureHeight, leftMargin, effectiveWidth;

        private void UpdateDisplayVars()
        {
            int w = this.Width;
            int h = this.Height;
            int dw = this.AutoScrollPosition.X;

            if (w < h)
            {
                w = this.Height;
                h = this.Width;
                dw = this.AutoScrollPosition.Y;
                drawFormat.Alignment = StringAlignment.Near;
                drawFormat.LineAlignment = StringAlignment.Center;
            }
            else
            {
                drawFormat.Alignment = StringAlignment.Center;
                drawFormat.LineAlignment = StringAlignment.Near;
            }
            w = (int)Math.Round(w * zoom);

            if (w == 0) w = 1;
            if (h == 0) h = 1;

            eventCursorHeight = (int)Math.Round(h * 0.4);
            backgroundCursorHeight = (int)Math.Round(h * 0.38);

            measureDistance = (int)Math.Floor(((float)w) / 100);

            smallMeasureHeight = (int)Math.Round(h * 0.05);
            medMeasureHeight = (int)Math.Round(h * 0.1);
            largeMeasureHeight = (int)Math.Round(h * 0.2);

            effectiveWidth = measureDistance * 100;

            leftMargin = (w - effectiveWidth) / 2 + dw;

            if (w < h)
                this.AutoScrollMinSize = new Size((int)w, 0);
            else
                this.AutoScrollMinSize = new Size(0, (int)w);
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
                e.Graphics.FillRectangle(backgroundBrush, new Rectangle(backgroundCursorHeight, leftMargin, h, effectiveWidth));
                w = this.Height;
                h = this.Width;
            }
            else
                e.Graphics.FillRectangle(backgroundBrush, new Rectangle(leftMargin, backgroundCursorHeight, effectiveWidth, h));

            w = (int)Math.Round(w * zoom);
            

            foreach (IEvent ev in events)
            {
                Pen evp = selected == ev ? selectedPen : eventPen;
                foreach (float f in ev.At)
                {
                    e.Graphics.DrawLine(evp, MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), h), MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), eventCursorHeight));
                     e.Graphics.DrawString(ev.ToString(), font, selected == ev ? selectedBrush : eventBrush, MakePoint(f * effectiveWidth + leftMargin, eventCursorHeight));
                }
            }

            for (int a = leftMargin; a <= effectiveWidth + leftMargin; a += measureDistance)
            {
                bool bigline = (a - leftMargin) % (measureDistance * 10) == 0;
                bool medline = (a - leftMargin) % (measureDistance * 2) == 0;
                Point pt = MakePoint(a, bigline ? largeMeasureHeight : (medline ? medMeasureHeight : smallMeasureHeight));
                PointF ptxt = MakePoint((float)a, 3 + (bigline ? largeMeasureHeight : (medline ? medMeasureHeight : smallMeasureHeight)));
                e.Graphics.DrawLine(bigline ? primaryPen : secondaryPen, MakePoint(a, 0), pt);
                if (bigline)
                    e.Graphics.DrawString((((float)a - leftMargin) / effectiveWidth).ToString("0.###"), font, primaryBrush, ptxt, drawFormat);
                else if(medline)
                    e.Graphics.DrawString((((float)a - leftMargin) / effectiveWidth * 100 % 10).ToString("#"), fontSmall, primaryBrush, ptxt, drawFormat);
            }

            if (cursor && mouseLoc != Int32.MinValue)
                e.Graphics.DrawLine(highlightPen, MakePoint(mouseLoc, 0), MakePoint(mouseLoc, h));

            if (playback.Enabled)
                e.Graphics.DrawLine(playPen, MakePoint((int)Math.Round(PlaybackTime / 1000.0f / Timespan * effectiveWidth) + leftMargin, 0), MakePoint((int)Math.Round(PlaybackTime / 1000.0f / Timespan * effectiveWidth) + leftMargin, h));
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
                double timespan = DateTime.Now.Subtract(holdTime).TotalMilliseconds;

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
        IEvent held;
        int heldAt = -1;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();

            holdTime = DateTime.Now;

            int loc = this.Width > this.Height ? e.X : e.Y;

            float mindist = Single.MaxValue;
            selected = null;
            held = null;

            foreach (IEvent ev in events)
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
            double timespan = DateTime.Now.Subtract(holdTime).TotalMilliseconds;

            if (timespan <= 200)
            {
                if ((ModifierKeys & Keys.Control) != Keys.None)
                {
                    float loc = ((float)mouseLoc - leftMargin) / effectiveWidth;

                    OnItemAdd(new ItemAddEventArgs(loc));
                }
                else
                    selected = held;
            }
            else
                selected = held;

            held = null;

            OnMouseMove(e);
        }

        public class ItemAddEventArgs : EventArgs
        {
            public float At;

            public ItemAddEventArgs(float at)
            {
                At = at;
            }
        }

        protected void OnItemAdd(ItemAddEventArgs e)
        {
            if (ItemAdd != null)
                ItemAdd(this, e);
        }

        public delegate void ItemAddEventHandler(object sender, ItemAddEventArgs e);

        public event ItemAddEventHandler ItemAdd;

        private float CurrentAt()
        {
            float t = 0;
            if (selected != null && heldAt >= 0 && heldAt < selected.At.Count)
                t = selected.At[heldAt];
            else if (selected != null)
                t = selected.At[0];

            return t;
        }

        public void SelectNext()
        {
            float t = CurrentAt();
            float dist = Single.MaxValue;
            IEvent n = null;

            foreach (IEvent ev in this.events)
            {
                foreach (float f in ev.At)
                {
                    if (f > t && f - t < dist)
                    {
                        dist = f - t;
                        n = ev;
                    }
                }
            }

            if(n != null)
                selected = n;
        }

        public void SelectPrevious()
        {
            float t = CurrentAt();
            float dist = Single.MaxValue;
            IEvent n = null;

            foreach (IEvent ev in this.events)
            {
                foreach (float f in ev.At)
                {
                    if (f < t && t - f < dist)
                    {
                        dist = t - f;
                        n = ev;
                    }
                }
            }

            if (n != null)
                selected = n;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            e.SuppressKeyPress = true;
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Right:
                    SelectNext();
                    break;
                case Keys.Up:
                case Keys.Left:
                    SelectPrevious();
                    break;
                case Keys.Space:
                    PlayPause();
                    break;
                default:
                    e.SuppressKeyPress = false;
                    break;
            }
        }

        public int PlaybackTime = 0;
        private MultimediaTimer playback = new MultimediaTimer();
        DateTime startTime;

        public void PlayPause()
        {
            if (playback.Enabled)
            {
                playback.Stop();
            }
            else
            {
                PlaybackTime = 0;
                playback.Start();
                startTime = DateTime.Now;
            }
        }

        private void playback_Tick(object sender, EventArgs e)
        {
            PlaybackTime += 10;
            if (PlaybackTime >= Timespan * 1000)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.Subtract(startTime).TotalMilliseconds);
                playback.Stop();
            }
            Invalidate();
        }
    }
}
