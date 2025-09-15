namespace Simulator.View
{
    public struct Cell
    {
        public PointF Point;
        public int Kind;

        public override readonly string ToString()
        {
            return $"{Kind} ({Point.X},{Point.Y})";
        }
    }
}
