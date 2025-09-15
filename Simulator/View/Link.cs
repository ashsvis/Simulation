
namespace Simulator.View
{
    public readonly struct Link
    {
        public readonly PointF SourcePoint => points.Count > 0 ? points[0] : PointF.Empty;
        public readonly PointF TargetPoint => points.Count > 1 ? points[^1] : PointF.Empty;

        private readonly List<PointF> points = [];
        
        public readonly float Length => points.Count > 1 ? Math.Abs(SourcePoint.X - TargetPoint.X) + Math.Abs(SourcePoint.Y - TargetPoint.Y) : 0f;

        public Link(params PointF[] points)
        {
            if (points == null || points.Length < 2)
                throw new ArgumentNullException();
            foreach (var point in points) 
            { 
                this.points.Add(point);
            }
        }

        public void Clear()
        {
            this.points.Clear();
        }

        public void AddPoint(PointF point)
        {
            this.points.Add(point);
        }

        public void Optimize()
        {
            //List<PointF> pts = [];
            //var groupsByX = points.GroupBy(point => point.X);
            //foreach (var groupByX in groupsByX)
            //{
            //    var groupsByY = groupByX.GroupBy(point => point.Y);
            //    foreach (var groupByY in groupsByY)
            //        pts.Add(new PointF(groupByX.Key, groupByY.Key));
            //}
        }

        public void Draw(Graphics graphics, Color foreColor)
        {
            using var pen = new Pen(foreColor);
            using var brush = new SolidBrush(Color.Gray);
            graphics.DrawLines(pen, [.. points]);
            if (points.Count > 2)
            {
                foreach (var point in points.Skip(1).Take(points.Count - 2))
                {
                    graphics.FillEllipse(brush, new RectangleF(
                        PointF.Subtract(point, new SizeF(0.5f, 0.5f)), new SizeF(1f, 1f)));
                }
            }
        }
    }
}
