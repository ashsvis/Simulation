using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model
{
    public readonly struct Link
    {
        private readonly Dictionary<int, RectangleF> htargets = [];
        private readonly Dictionary<int, RectangleF> vtargets = [];

        [Browsable(false)]
        public PointF SourcePoint => points.Count > 0 ? points[0] : PointF.Empty;

        [Browsable(false)]
        public readonly PointF TargetPoint => points.Count > 1 ? points[^1] : PointF.Empty;

        private readonly List<PointF> points = [];

        [Browsable(false)]
        public readonly List<PointF> Points => points;

        private readonly bool[] busy = new bool[1];
        private readonly bool[] selected = new bool[1];
        private readonly object[] value = new object[1];
        private readonly Guid[] guids = new Guid[2];

        public Link(Guid id, Guid sourceId, int sourcePin, Guid destinationId, int destinationPin, params PointF[] points)
        {
            if (points == null || points.Length < 2)
                throw new ArgumentNullException(nameof(points), "Массив точек пуст или содержит меньше двух элементов");
            BeginUpdate();
            try
            {
                foreach (var point in points)
                {
                    AddPoint(point);
                }
            }
            finally 
            { 
                EndUpdate();
            }
            Id = id;
            guids[0] = sourceId;
            guids[1] = destinationId;
            SourcePinIndex = sourcePin;
            DestinationPinIndex = destinationPin;
        }

        public void Save(XElement xtem)
        {
            xtem.Add(new XElement("Id", Id));
            var xsource = new XElement("Source");
            xsource.Add(new XAttribute("Id", SourceId));
            xsource.Add(new XAttribute("PinIndex", SourcePinIndex));
            xtem.Add(xsource);
            var xdest = new XElement("Destination");
            xdest.Add(new XAttribute("Id", DestinationId));
            xdest.Add(new XAttribute("PinIndex", DestinationPinIndex));
            xtem.Add(xdest);
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
        }

        public void EndUpdate()
        {
            var pt1 = points[0];
            var pt2 = points[^1];
            points.Insert(0, pt1);
            points.Add(pt2);

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


        [Category(" Общие"), DisplayName("Идентификатор")]
        public Guid Id { get; }

        [Category("Источник"), DisplayName("Идентификатор")]
        public Guid SourceId => guids[0];

        public void SetSourceId(Guid id)
        {
            guids[0] = id;
        }

        [Category("Источник"), DisplayName("Выход"), Description("Индекс выхода источника")]
        public int SourcePinIndex { get; }

        [Category("Приёмник"), DisplayName("Идентификатор")]
        public Guid DestinationId => guids[1];

        public void SetDestinationId(Guid id)
        {
            guids[1] = id;
        }

        [Category("Приёмник"), DisplayName("Вход"), Description("Индекс входа приёмника")]
        public int DestinationPinIndex { get; }

        [Browsable(false)]
        public bool Selected => selected[0];

        public void SetSelect(bool value)
        {
            selected[0] = value;
        }

        [Browsable(false)]
        public object Value => value[0];

        public void SetValue(object val)
        {
            value[0] = val;
        }

        public void Draw(Graphics graphics, Color foreColor)
        {
            if (busy[0]) return;

            if (points.Count > 1)
            {
                using var pen = new Pen(selected[0] 
                    ? Color.Magenta 
                    : (value[0] is bool v) ? (v == true) ? Color.Lime : Color.Red : foreColor);
                graphics.DrawLines(pen, [SourcePoint, points[0]]);
                graphics.DrawLines(pen, [.. points]);
                graphics.DrawLines(pen, [points[^1], TargetPoint]);
            }
#if DEBUG
            //using var brush = new SolidBrush(Color.Aqua);
            //foreach (var point in points)
            //{
            //    graphics.FillEllipse(brush, new RectangleF(
            //        PointF.Subtract(point, new SizeF(0.5f, 0.5f)), new SizeF(1f, 1f)));
            //}
            //// области выбора
            //using Pen tarpen = new(Color.FromArgb(80, Color.Aqua), 0);
            //foreach (var key in vtargets.Keys)
            //{
            //    var vtarget = vtargets[key];
            //    graphics.DrawRectangles(tarpen, [vtarget]);
            //}
            //foreach (var key in htargets.Keys)
            //{
            //    var htarget = htargets[key];
            //    graphics.DrawRectangles(tarpen, [htarget]);
            //}
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
                            if (segmentIndex == 1)
                                points.Insert(0, pt1);
                            if (segmentIndex == points.Count - 1)
                                points.Add(pt2);
                        }
                    }
                    else if (pt1.Y == pt2.Y)
                    {
                        // горизонтальный сегмент
                        if (!segmentVertical)
                        {
                            points[i - 1] = PointF.Add(points[i - 1], new SizeF(0, delta.Height));
                            points[i] = PointF.Add(points[i], new SizeF(0, delta.Height));
                            if (segmentIndex == 1)
                                points.Insert(0, pt1);
                            if (segmentIndex == points.Count - 1)
                                points.Add(pt2);
                        }
                    }
                    CalculateSegmentTargets();
                    break;
                }
            }
        }

        public void SnapPointsToGrid(Func<PointF, int, PointF> snapToGridMethod)
        {
            for (var i = 0; i < points.Count; i++)
                points[i] = snapToGridMethod(points[i], 1);
            SegmentOptimization();
            CalculateSegmentTargets();
        }

        private void SegmentOptimization()
        {
            var pt1 = points[0];
            var pt2 = points[^1];
            // объединение повторяющихся сегментов
            var k = 2;
            while (k < points.Count)
            {
                if (points[k - 2].Y == points[k - 1].Y && points[k - 1].Y == points[k].Y ||
                    points[k - 2].X == points[k - 1].X && points[k - 1].X == points[k].X)
                    points.RemoveAt(k - 1);
                else
                    k++;
            }
            points.Insert(0, pt1);
            points.Add(pt2);
        }

        public void UpdateSourcePoint(PointF point)
        {
            if (points.Count > 4)
            {
                points.RemoveAt(0);
                points.RemoveAt(0);
                points.RemoveAt(0);
            }
            points.Insert(0, new PointF(points[0].X, point.Y));
            points.Insert(0, point);

            SegmentOptimization();
            CalculateSegmentTargets();
        }

        public void UpdateDestinationPoint(PointF point)
        {
            if (points.Count > 4)
            {
                points.RemoveAt(points.Count - 1);
                points.RemoveAt(points.Count - 1);
                points.RemoveAt(points.Count - 1);
            }
            points.Add(new PointF(points[^1].X, point.Y));
            points.Add(point);

            SegmentOptimization();
            CalculateSegmentTargets();
        }

        public void Offset(SizeF delta)
        {
            for (var i = 0; i < points.Count; i++)
                points[i] = PointF.Add(points[i], delta);
        }
    }
}
