namespace Simulator.Model
{
    public class Element
    {
        public Element() { }
        public Type? Type { get; set; }
        public object? Instance { get; set; }
        public PointF Location { get; set; }
    }
}
