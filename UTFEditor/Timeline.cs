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
        int interval = 20;
        TimelineData events;
        KeyValuePair<float, object> selected = new KeyValuePair<float,object>(0, null);

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
                primaryPen.Brush = primaryBrush;
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
                secondaryPen.Brush = secondaryBrush;
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
                eventPen.Brush = eventBrush;
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
                highlightPen.Brush = highlightBrush;
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
                selectedPen.Brush = selectedBrush;
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
                playPen.Brush = playBrush;
            }
        }

        public KeyValuePair<float, object> SelectedItem
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                held = new KeyValuePair<float, object>(0, null);
            }
        }

        public TimelineData Items
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

                UpdateDisplayVars();
                Invalidate();
            }
        }

        private float timespan = 1.0f;

        public float Timespan
        {
            get
            {
                return timespan;
            }
            set
            {
                timespan = Math.Max(0, value);
                events.Interval = interval / 1000.0f / timespan;
            }
        }

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

            playback = new MultimediaTimer(interval);
            playback.Tick += new EventHandler(playback_Tick);

            events = new TimelineData(interval / 1000.0f / timespan);

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
            bool rev = false;

            if (w < h)
            {
                w = this.Height;
                h = this.Width;
                rev = true;
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

            if (rev)
                this.AutoScrollMinSize = new Size(0, w);
            else
                this.AutoScrollMinSize = new Size(w, 0);
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
            

            foreach (KeyValuePair<float, object> evts in events)
            {
                float f = evts.Key;
                object ev = evts.Value;

                string n;

                if (ev is ITimelineElement)
                {
                    ITimelineElement t = ev as ITimelineElement;
                    n = t.Name;

                    if (t.Length > 0)
                    {
                        SolidBrush evb = selected.Value == ev ? selectedBrush : eventBrush;

                        Color org = evb.Color;
                        evb.Color = Color.FromArgb(30, evb.Color);

                        e.Graphics.FillRectangle(evb,
                            new RectangleF(
                                MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), h - smallMeasureHeight),
                                new SizeF(MakePoint(t.Length / Timespan * effectiveWidth, h))
                            ));

                        evb.Color = org;
                    }
                }
                else
                    n = ev.ToString();

                Pen evp = selected.Value == ev ? selectedPen : eventPen;

                e.Graphics.DrawLine(evp, MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), h), MakePoint((int)Math.Round(f * effectiveWidth + leftMargin), eventCursorHeight));

                e.Graphics.DrawString(n, font, selected.Value == ev ? selectedBrush : eventBrush, MakePoint(f * effectiveWidth + leftMargin, eventCursorHeight));
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
                e.Graphics.DrawLine(playPen, MakePoint((int)Math.Round(PlaybackTime / 1000.0f / timespan * effectiveWidth) + leftMargin, 0), MakePoint((int)Math.Round(PlaybackTime / 1000.0f / timespan * effectiveWidth) + leftMargin, h));
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

            if (held.Value != null)
            {
                double timespan = DateTime.Now.Subtract(holdTime).TotalMilliseconds;

                if (timespan > 200)
                {
                    events.Remove(held);
                    held = new KeyValuePair<float, object>(((float)mouseLoc - leftMargin) / effectiveWidth, held.Value);
                    events.Add(held);
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
            held = new KeyValuePair<float,object>(0, null);

            Cursor.Show();

            Invalidate();
        }

        DateTime holdTime;
        KeyValuePair<float, object> held;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();

            holdTime = DateTime.Now;

            float loc = ((this.Width > this.Height ? e.X : e.Y) - leftMargin) / (float)effectiveWidth;

            held = new KeyValuePair<float, object>(0, null);

            foreach (KeyValuePair<float, object> evts in events)
            {
                if ((held.Value == null || Math.Abs(evts.Key - loc) < Math.Abs(evts.Key - held.Key)) && Math.Abs(evts.Key - loc) * effectiveWidth <= 10)
                {
                    held = evts;
                    break;
                }
            }

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            double timespan = DateTime.Now.Subtract(holdTime).TotalMilliseconds;

            KeyValuePair<float, object> lastSel = selected;

            if (timespan <= 200)
            {
                if ((ModifierKeys & Keys.Control) != Keys.None)
                {
                    float loc = ((float)mouseLoc - leftMargin) / effectiveWidth;

                    if ((ModifierKeys & Keys.Shift) != Keys.None)
                        OnTimeAdd(new TimeAddEventArgs(loc, selected.Value));
                    else
                        OnItemAdd(new ItemAddEventArgs(loc));
                }
                else
                    selected = held;
            }
            else
                selected = held;

            if (lastSel.Value != selected.Value || lastSel.Key != selected.Key)
                OnSelectionChanged();

            held = new KeyValuePair<float,object>(0, null);

            OnMouseMove(e);
        }

        public class SelectionChangedEventArgs : EventArgs
        {
            public KeyValuePair<float, object> Selection;

            public SelectionChangedEventArgs(KeyValuePair<float, object> sel)
            {
                Selection = sel;
            }
        }

        protected void OnSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, new SelectionChangedEventArgs(selected));
        }

        public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);

        public event SelectionChangedEventHandler SelectionChanged;

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

        public class TimeAddEventArgs : EventArgs
        {
            public float At;
            public object Object;

            public TimeAddEventArgs(float at, object obj)
            {
                At = at;
                Object = obj;
            }
        }

        protected void OnTimeAdd(TimeAddEventArgs e)
        {
            if (TimeAdd != null)
                TimeAdd(this, e);
        }

        public delegate void TimeAddEventHandler(object sender, TimeAddEventArgs e);

        public event TimeAddEventHandler TimeAdd;

        public void SelectNext()
        {
        }

        public void SelectPrevious()
        {
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
        private MultimediaTimer playback;
        DateTime startTime;
        Dictionary<object, List<float>> playElems;
        Random rand = new Random();

        public void PlayPause()
        {
            if (playback.Enabled)
            {
                playback.Stop();
                if (Stop != null)
                    Stop(this, new EventArgs());
            }
            else
            {
                PlaybackTime = 0;
                playElems = events.GetObjectsTimes();
                foreach (KeyValuePair<object, List<float>> k in playElems)
                {
                    if (k.Value.Count <= 1) continue;
                    float sel = k.Value[rand.Next(k.Value.Count - 1)];
                    k.Value.Clear();
                    k.Value.Add(sel);
                }
                playback.Start();
                startTime = DateTime.Now;
                if (Play != null)
                    Play(this, new EventArgs());
            }

            Invalidate();
        }

        public event EventHandler Play;
        public event EventHandler Stop;
        public event PlayEventEventHandler PlayEvent;

        private void playback_Tick(object sender, EventArgs e)
        {
            PlaybackTime += playback.Interval;

            if (PlaybackTime >= timespan * 1000)
            {
                playback.Stop();

                if (Stop != null)
                    Stop(this, new EventArgs());
            }

            if (PlayEvent != null)
            {
                List<KeyValuePair<float, object>> plevs = events[PlaybackTime];
                foreach (KeyValuePair<float, object> evt in plevs)
                {
                    if(playElems[evt.Value][0] == evt.Key)
                        PlayEvent(this, new PlayEventEventArgs(evt.Value));
                }
            }

            Invalidate();
        }

        public bool Playing
        {
            get
            {
                return playback.Enabled;
            }
        }

        public delegate void PlayEventEventHandler(object sender, PlayEventEventArgs e);

        public class PlayEventEventArgs : EventArgs
        {
            public object Event;

            public PlayEventEventArgs(object ev)
            {
                Event = ev;
            }
        }
    }

    class TimelineData : ICollection<KeyValuePair<float, object>>
    {
        private LinkedList<KeyValuePair<float, object>> lst = new LinkedList<KeyValuePair<float, object>>();

        private float interval;

        public float Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = Math.Max(0, value);
            }
        }

        public TimelineData(float i)
        {
            Interval = i;
        }

        public void Add(KeyValuePair<float, object> item)
        {
            LinkedListNode<KeyValuePair<float, object>> n = lst.First;
            while (n != null)
            {
                if (n.Value.Key > item.Key)
                {
                    lst.AddBefore(n, item);
                    return;
                }
                n = n.Next;
            }

            lst.AddLast(item);
        }

        public void Add(float key, object value)
        {
            Add(new KeyValuePair<float, object>(key, value));
        }

        public bool Remove(float key)
        {
            LinkedListNode<KeyValuePair<float, object>> n = lst.First;
            while (n != null)
            {
                LinkedListNode<KeyValuePair<float, object>> next = n.Next;

                if (key >= n.Value.Key && key <= n.Value.Key + interval)
                {
                    lst.Remove(n);
                    return true;
                }

                n = next;
            }

            return false;
        }

        public bool Remove(float key, object value)
        {
            return Remove(new KeyValuePair<float, object>(key, value));
        }

        public bool Remove(KeyValuePair<float, object> item)
        {
            LinkedListNode<KeyValuePair<float, object>> n = lst.First;
            while (n != null)
            {
                LinkedListNode<KeyValuePair<float, object>> next = n.Next;

                if (item.Value == n.Value.Value && item.Key >= n.Value.Key && item.Key <= n.Value.Key + interval)
                {
                    lst.Remove(n);
                    return true;
                }

                n = next;
            }

            return false;
        }

        public void Clear()
        {
            lst.Clear();
        }

        public bool Contains(KeyValuePair<float, object> item)
        {
            foreach (KeyValuePair<float, object> k in lst)
            {
                if (k.Key == item.Key && k.Value == item.Value)
                    return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<float, object>[] array, int arrayIndex)
        {
            int a = arrayIndex;
            foreach (KeyValuePair<float, object> k in lst)
            {
                array[a] = k;

                a++;
                if (a == array.Length) return;
            }
        }

        public int Count
        {
            get { return lst.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<KeyValuePair<float, object>> GetEnumerator()
        {
            return lst.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return lst.GetEnumerator();
        }

        public List<KeyValuePair<float, object>> this[float key]
        {
            get
            {
                List<KeyValuePair<float, object>> o = new List<KeyValuePair<float,object>>();

                LinkedListNode<KeyValuePair<float, object>> n = lst.First;
                while (n != null)
                {
                    if (key >= n.Value.Key && key <= n.Value.Key + interval)
                        o.Add(n.Value);

                    n = n.Next;
                }

                return o;
            }
        }

        public LinkedListNode<KeyValuePair<float, object>> GetNode(float key, object value)
        {
            LinkedListNode<KeyValuePair<float, object>> n = lst.First;
            while (n != null)
            {
                if (key >= n.Value.Key && key <= n.Value.Key + interval && value == n.Value.Value)
                    return n;

                n = n.Next;
            }

            return null;
        }

        public List<object> GetObjects()
        {
            List<object> o = new List<object>();
            foreach (KeyValuePair<float, object> k in lst)
            {
                if (!o.Contains(k.Value))
                    o.Add(k.Value);
            }

            return o;
        }

        public Dictionary<object, List<float>> GetObjectsTimes()
        {
            Dictionary<object, List<float>> o = new Dictionary<object, List<float>>();
            foreach (KeyValuePair<float, object> k in lst)
            {
                if (!o.ContainsKey(k.Value))
                {
                    o.Add(k.Value, (new float[] {k.Key}).ToList<float>() );
                }
                else
                {
                    o[k.Value].Add(k.Key);
                }
            }

            return o;
        }
    }

    public interface ITimelineElement
    {
        string Name
        {
            get;
        }

        float Length
        {
            get;
        }

        List<string> Tags
        {
            get;
        }
    }
}
