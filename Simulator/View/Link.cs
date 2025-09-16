
namespace Simulator.View
{
    public readonly struct Link
    {
        public readonly PointF SourcePoint => points.Count > 0 ? points[0] : PointF.Empty;
        public readonly PointF TargetPoint => points.Count > 1 ? points[^1] : PointF.Empty;

        private readonly List<PointF> points = [];

        private readonly bool[] busy = new bool[1];
        
        public readonly float Length => points.Count > 1 ? Math.Abs(SourcePoint.X - TargetPoint.X) + Math.Abs(SourcePoint.Y - TargetPoint.Y) : 0f;

        public Link(params PointF[] points)
        {
            if (points == null || points.Length < 2)
                throw new ArgumentNullException(nameof(points), "Массив точек пуст или содержит меньше двух элементов");
            foreach (var point in points) 
            { 
                this.points.Add(point);
            }
        }

        public void BeginUpdate()
        {
            busy[0] = true;
            this.points.Clear();
        }

        public void AddPoint(PointF point)
        {
            if (!busy[0])
            {
                throw new ArgumentNullException(nameof(point), "Для добавления точек вначале вызовите метод BeginUpdate(), а в конце EndUpdate()");
            }
            if (!points.Contains(point))
                this.points.Add(point);
        }

        public void EndUpdate()
        {
            busy[0] = false;
        }

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
#endif
        }
    }
}
