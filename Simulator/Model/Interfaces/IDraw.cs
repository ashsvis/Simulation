namespace Simulator.Model.Interfaces
{
    public interface IDraw
    {
        void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size, int index, bool selected, CustomDraw? customDraw = null);
    }
}