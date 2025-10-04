using Simulator.Model.Interfaces;

namespace Simulator.Model.Diagram
{
    public class FINISH : CommonDiagram, ICustomDraw
    {
        public FINISH() : base(DiagramFunction.Finish) { }

        public override void CalculateTargets(PointF location, ref SizeF size,
            Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins, Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var height = step * 4;
            var width = step * 10;
            size = new SizeF(width, height);
            // входы
            var y = -step + location.Y;
            var x = location.X + width / 2f;
            itargets.Clear();
            ipins.Clear();
            // значение входа
            var ms = new SizeF(step * 2, step * 2);
            itargets.Add(0, new RectangleF(new PointF(x - ms.Width + step, y - ms.Height + step), ms));
            ipins.Add(0, new PointF(x, y));
            otargets.Clear();
            opins.Clear();
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            var rleft = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height);
            graphics.DrawArc(pen, rleft, 90f, 180f);
            var rright = new RectangleF(rect.X + rect.Width - rect.Height, rect.Y, rect.Height, rect.Height);
            graphics.DrawArc(pen, rright, -90f, 180f);
            var cen = new RectangleF(rect.X + rect.Height / 2f, rect.Y, rect.Width - rect.Height, rect.Height);
            graphics.DrawLines(pen, [cen.Location, new PointF(cen.Right, cen.Top)]);
            graphics.DrawLines(pen, [new PointF(cen.Left, cen.Bottom), new PointF(cen.Right, cen.Bottom)]);
            using var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            graphics.DrawString("Конец", font, fontbrush, rect, sf);
        }
    }
}
