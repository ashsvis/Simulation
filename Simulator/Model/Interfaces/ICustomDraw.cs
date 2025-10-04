namespace Simulator.Model.Interfaces
{
    public interface ICustomDraw
    {
        void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected);
    }
}