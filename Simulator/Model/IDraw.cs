namespace Simulator.Model
{
    public interface IDraw
    {
        void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size, CustomDraw? customDraw = null);
    }
}