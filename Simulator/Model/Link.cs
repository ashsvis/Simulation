using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;

namespace Simulator.Model
{
    public readonly struct Link
    {
        private readonly Dictionary<int, RectangleF> htargets = [];
        private readonly Dictionary<int, RectangleF> vtargets = [];

        [Browsable(false)]
        public readonly PointF SourcePoint => points.Count > 0 ? points[0] : PointF.Empty;

        [Browsable(false)]
        public readonly PointF TargetPoint => points.Count > 1 ? points[^1] : PointF.Empty;

        private readonly List<PointF> points = [];

        private readonly bool[] busy = new bool[1];
        
        public readonly float Length => points.Count > 1 ? Math.Abs(SourcePoint.X - TargetPoint.X) + Math.Abs(SourcePoint.Y - TargetPoint.Y) : 0f;

        public Link(Guid id, params PointF[] points)
        {
            if (points == null || points.Length < 2)
                throw new ArgumentNullException(nameof(points), "Массив точек пуст или содержит меньше двух элементов");
            BeginUpdate();
            try
            {
                foreach (var point in points)
                {
                    //this.points.Add(point);
                    AddPoint(point);
                }
            }
            finally 
            { 
                EndUpdate();
            }
            Id = id;
        }

        public void Save(XElement xtem)
        {
            xtem.Add(new XElement("Id", Id));
            XElement xpoints = new("Points");
            foreach (var point in points)
            {
                XElement xpoint = new("Point");
                xpoint.Add(new XAttribute("X", point.X));
                xpoint.Add(new XAttribute("Y", point.Y));
                xpoints.Add(xpoint);
            }
            if (points.Count > 0)
                xtem.Add(xpoints);
            XElement xsegments = new("Segments");
            for (int i = 1; i < points.Count; i++)
            {
                XElement xsegment = new("Segment");
                xsegments.Add(xsegment);
                var pt1 = points[i - 1];
                var pt2 = points[i];
                if (pt1.X == pt2.X && pt1.Y != pt2.Y)
                {
                    // вертикальный сегмент
                    xsegment.Add(new XAttribute("Kind", LinkVector.Vertical));
                }
                else if (pt1.X != pt2.X && pt1.Y == pt2.Y)
                {
                    // горизонтальный сегмент
                    xsegment.Add(new XAttribute("Kind", LinkVector.Horizontal));
                }
                else
                {
                    // скрытый сегмент
                    xsegment.Add(new XAttribute("Kind", LinkVector.None));
                }
                //xsegment.Add(new XAttribute("X1", pt1.X));
                //xsegment.Add(new XAttribute("Y1", pt1.Y));
                //xsegment.Add(new XAttribute("X2", pt2.X));
                //xsegment.Add(new XAttribute("Y2", pt2.Y));
            }
            if (points.Count > 1)
                xtem.Add(xsegments);
        }

        public void Load(XElement xlink)
        {
            List<LinkVector> segments = [];
            var xsegments = xlink.Element("Segments");
            if (xsegments != null)
            {
                foreach (XElement xsegment in xsegments.Elements("Segment"))
                {
                    if (Enum.TryParse(xsegment?.Attribute("Kind")?.Value, out LinkVector linkVector))
                    {
                        segments.Add(linkVector);
                    }
                }
            }
        }

        public void BeginUpdate()
        {
            busy[0] = true;
            points.Clear();
        }

        public void AddPoint(PointF point)
        {
            if (!busy[0])
            {
                throw new ArgumentNullException(nameof(point), "Для добавления точек вначале вызовите метод BeginUpdate(), а в конце EndUpdate()");
            }
            if (!points.Contains(point))
                points.Add(point);
            if (points.Count == 1)
                points.Add(point);
        }

        public void EndUpdate()
        {
            if (points.Count > 0)
            {
                var lastpt = points[^1];
                points.Add(new PointF(lastpt.X, lastpt.Y));
            }
            CalculateSegmentTargets();
            busy[0] = false;
        }

        private void CalculateSegmentTargets()
        {
            htargets.Clear();
            vtargets.Clear();
            for (int i = 1; i < points.Count; i++)
            {
                var pt1 = points[i - 1];
                var pt2 = points[i];
                if (pt1.X == pt2.X)
                {
                    // вертикальный сегмент
                    vtargets.Add(i, new RectangleF(pt1.X - Element.Step / 2, Math.Min(pt1.Y, pt2.Y), Element.Step, Math.Abs(pt1.Y - pt2.Y)));
                }
                else if (pt1.Y == pt2.Y)
                {
                    // горизонтальный сегмент
                    htargets.Add(i, new RectangleF(Math.Min(pt1.X, pt2.X), pt2.Y - Element.Step / 2, Math.Abs(pt1.X - pt2.X), Element.Step));
                }
            }
        }

        [Browsable(false)]
        public (RectangleF,int, bool)[] Segments
        {
            get
            {
                List<(RectangleF, int, bool)> list = [];
                foreach (var key in vtargets.Keys)
                    list.Add((vtargets[key], key, true));
                foreach (var key in htargets.Keys)
                    list.Add((htargets[key], key, false));
                return [.. list.OrderBy(x => x.Item2)];
            }
        }

        public Guid Id { get; }

        public void Draw(Graphics graphics, Color foreColor)
        {
            if (busy[0]) return;

            if (points.Count > 1)
            {
                using var pen = new Pen(foreColor);
                graphics.DrawLines(pen, [.. points]);
            }
#if DEBUG
            using var brush = new SolidBrush(Color.Aqua);
            foreach (var point in points)
            {
                graphics.FillEllipse(brush, new RectangleF(
                    PointF.Subtract(point, new SizeF(0.5f, 0.5f)), new SizeF(1f, 1f)));
            }
            // области выбора
            using Pen tarpen = new(Color.FromArgb(80, Color.Aqua), 0);
            foreach (var key in vtargets.Keys)
            {
                var vtarget = vtargets[key];
                graphics.DrawRectangles(tarpen, [vtarget]);
            }
            foreach (var key in htargets.Keys)
            {
                var htarget = htargets[key];
                graphics.DrawRectangles(tarpen, [htarget]);
            }
#endif
        }

        public void OffsetSegment(int segmentIndex, bool segmentVertical, SizeF delta)
        {
            for (int i = 1; i < points.Count; i++)
            {
                if (segmentIndex == i)
                {
                    var pt1 = points[i - 1];
                    var pt2 = points[i];
                    if (pt1.X == pt2.X)
                    {
                        // вертикальный сегмент
                        if (segmentVertical)
                        {
                            points[i - 1] = PointF.Add(points[i - 1], new SizeF(delta.Width, 0));
                            points[i] = PointF.Add(points[i], new SizeF(delta.Width, 0));
                        }
                    }
                    else if (pt1.Y == pt2.Y)
                    {
                        // горизонтальный сегмент
                        if (!segmentVertical)
                        {
                            points[i - 1] = PointF.Add(points[i - 1], new SizeF(0, delta.Height));
                            points[i] = PointF.Add(points[i], new SizeF(0, delta.Height));
                        }
                    }
                    CalculateSegmentTargets();
                    break;
                }
            }
        }
    }
}
