namespace Simulator.Model
{
    public interface ICustomDraw
    {
        void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush);
    }

    public delegate PointF GetLinkPointMethod();
}