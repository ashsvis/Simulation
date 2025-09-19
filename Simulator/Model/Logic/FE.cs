using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class FE : CommonLogic, ICustomDraw
    {
        public FE() : base(LogicFunction.Fe, 1) 
        {
            OutputValues[0] = false;
        }

        [Browsable(false)]
        public override string FuncSymbol => "FE"; // Детектор фронта

        private DateTime time;
        private readonly double waitTime = 0.2;

        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            if (!input && !Out)
            {
                time = DateTime.Now + TimeSpan.FromSeconds(waitTime);
                Out = false;
            }
            else
                Out = time > DateTime.Now;
        }

        public void Reset()
        {
            if (Out)
                time = DateTime.Now;
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            rect.Inflate(-1, -1);
            graphics.FillRectangle(brush, rect);
            var sym = new RectangleF(rect.Location, new SizeF(rect.Width, rect.Height / 3));
            sym.Inflate(-6, -3);
            var w = sym.Width / 4;
            sym.Offset(w, 0);
            graphics.DrawLines(pen, [
                new PointF(sym.Left, sym.Bottom), new PointF(sym.Left + w, sym.Bottom),
                new PointF(sym.Left + w, sym.Top), new PointF(sym.Left + w * 2, sym.Top)]);
            // индекс элемента в списке
            if (index != 0)
            {
                var text = $"L{index}";
                var ms = graphics.MeasureString(text, font);
                using var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(text, font, fontbrush, new PointF(rect.X + rect.Width / 2, rect.Y +rect.Height - ms.Height), format);
            }
        }
    }
}
