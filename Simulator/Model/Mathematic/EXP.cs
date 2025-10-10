using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class EXP : CommonAnalog, IManualChange, ICustomDraw
    {
        public EXP() : base(LogicFunction.Exp, 1)
        {
            SetValueToOut(0, 0.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            SetValueToOut(0, Math.Exp(a));
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            var step = Element.Step;
            var location = rect.Location;
            var height = step + 1 * step * 4 + step;
            var width = step + 1 * step * 4 + step;
            // обозначение функции, текст по-центру, в верхней части рамки элемента
            using var bigfont = new Font(font.FontFamily, font.Size + 10f);
            using var smallfont = new Font(font.FontFamily, font.Size - 1f);
            var msn = graphics.MeasureString("X1", bigfont);
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            graphics.DrawString("e", bigfont, fontbrush, new PointF(location.X + width / 2, location.Y), format);
            graphics.DrawString("x", smallfont, fontbrush, new PointF(location.X + width / 2 + msn.Width * 0.2f, location.Y), format);
            // индекс элемента в списке
            var text = $"L{index}";
            var msl = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, fontbrush, new PointF(location.X + width / 2, location.Y + height - msl.Height), format);
        }
    }
}
