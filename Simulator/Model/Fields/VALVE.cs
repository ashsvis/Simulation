using Simulator.Model.Interfaces;
using System.Drawing.Drawing2D;

namespace Simulator.Model.Fields
{
    public class VALVE : CommonFields, ICustomDraw
    {
        public VALVE() : base(FieldFunction.Valve) { }

        public override void CalculateTargets(PointF location, ref SizeF size,
            Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins, Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var height = step * 4;
            var width = step * 4;
            size = new SizeF(width, height);
            itargets.Clear();
            ipins.Clear();
            // входы
            var x = location.X;
            var y = location.Y + height / 2;
            // значение входа
            var ms = new SizeF(step * 2, step * 2);
            itargets.Add(0, new RectangleF(new PointF(x + step, y - step * 2), ms));
            ipins.Add(0, new PointF(x, y + step));
            // выход
            x = location.X + width;
            y = location.Y + height / 2;
            otargets.Clear();
            opins.Clear();
            // значение выхода
            ms = new SizeF(step * 4, step * 2);
            otargets.Add(0, new RectangleF(new PointF(x- ms.Width, y), ms));
            opins.Add(0, new PointF(x, y + step));
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
        {
            var control = new RectangleF(rect.X + (rect.Height / 4), rect.Y, rect.Height / 2, rect.Height / 2);
            var valve = new RectangleF(rect.X, rect.Y + rect.Height / 2, rect.Width, rect.Height / 2);
            valve.Inflate(-0.5f, -0.5f);
            using var controlbrush = new SolidBrush(Color.Yellow);
            graphics.FillRectangle(controlbrush, control);
            graphics.DrawRectangles(pen, [control]);
            using GraphicsPath path = new();
            path.AddLines([
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Right, valve.Bottom),
                    new PointF(valve.Right, valve.Top),
                    new PointF(valve.Left, valve.Bottom),
                    new PointF(valve.Left, valve.Top),
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Left + valve.Width / 2, control.Bottom),
                ]);
            using var valvebrush = new SolidBrush(Color.Yellow);
            graphics.FillPath(valvebrush, path);
            graphics.DrawLines(pen,
                [
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Right, valve.Bottom),
                    new PointF(valve.Right, valve.Top),
                    new PointF(valve.Left, valve.Bottom),
                    new PointF(valve.Left, valve.Top),
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Left + valve.Width / 2, control.Bottom),
                ]);
        }
    }
}
