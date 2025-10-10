using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class POW : CommonAnalog, IManualChange, ICustomDraw
    {
        public POW() : base(LogicFunction.Pow, 2)
        {
            SetValueToOut(0, 0.0);
            Inputs[0].Name = "X";
            Inputs[1].Name = "Y";
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, Math.Pow(a, b));
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            var step = Element.Step;
            var location = rect.Location;
            var height = step + 2 * step * 4 + step;
            var width = step + 1 * step * 4 + step;
            // обозначение функции, текст по-центру, в верхней части рамки элемента
            using var bigfont = new Font(font.FontFamily, font.Size + 3f);
            using var smallfont = new Font(font.FontFamily, font.Size - 1f);
            var msn = graphics.MeasureString("X1", bigfont);
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            graphics.DrawString("X", bigfont, fontbrush, new PointF(location.X + width / 2, location.Y + msn.Height * 0.1f), format);
            graphics.DrawString("Y", smallfont, fontbrush, new PointF(location.X + msn.Width * 1.3f, location.Y), format);
            // входы
            var y = step + location.Y;
            var x = -step + location.X;
            for (var i = 0; i < Inputs.Length; i++)
            {
                y += step * 2;
                // наименование входа
                var name = Inputs[i].Name ?? "";
                if (!string.IsNullOrEmpty(name))
                {
                    var ms = graphics.MeasureString(name, smallfont);
                    graphics.DrawString(name, smallfont, fontbrush, new PointF(x + step, y - ms.Height / 2));
                }
                y += step * 2;
            }
            // индекс элемента в списке
            var text = $"L{index}";
            var msl = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, fontbrush, new PointF(location.X + width / 2, location.Y + height - msl.Height), format);
        }
    }
}
