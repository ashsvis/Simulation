namespace Simulator.View
{
    public readonly struct Link
    {
        public readonly PointF SourcePoint => points.Count > 0 ? points[0] : PointF.Empty;
        public readonly PointF TargetPoint => points.Count > 1 ? points[^1] : PointF.Empty;

        private readonly List<PointF> points = [];

        public Link(params PointF[] points)
        {
            if (points == null || points.Length < 2)
                throw new ArgumentNullException();
            foreach (var point in points) 
            { 
                this.points.Add(point);
            }
        }
    }
}
