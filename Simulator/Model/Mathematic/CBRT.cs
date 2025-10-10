using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class CBRT : CommonAnalog, IManualChange, ICustomDraw
    {
        public CBRT() : base(LogicFunction.Sbrt, 1)
        {
            SetValueToOut(0, 0.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            SetValueToOut(0, Math.Cbrt(a));
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            var step = Element.Step;
            var location = rect.Location;
            var height = step + 1 * step * 4 + step;
            var width = step + 1 * step * 4 + step;
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            // обозначение функции, текст по-центру, в верхней части рамки элемента
            using var smallfont = new Font(font.FontFamily, font.Size - 3f);
            graphics.DrawString("3", smallfont, fontbrush, new PointF(location.X + width / 2, location.Y), format);
            using var bigfont = new Font("Symbol", font.Size + 3f);
            var msn = graphics.MeasureString("", bigfont);
            graphics.DrawString("", bigfont, fontbrush, new PointF(location.X + width / 2 + 1f, location.Y + msn.Height * 0.1f), format);
            graphics.DrawLine(pen,
                new PointF(location.X + width / 2 + msn.Width / 3, location.Y + msn.Height * 0.15f),
                new PointF(location.X + width / 2 + msn.Width / 3 + 5f, location.Y + msn.Height * 0.15f));
            // индекс элемента в списке
            var text = $"L{index}";
            var msl = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, fontbrush, new PointF(location.X + width / 2, location.Y + height - msl.Height), format);
        }
    }
}
