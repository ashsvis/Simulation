using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Generator
{
    public class PULSE : CommonLogic, ICustomDraw
    {
        public PULSE() : base(LogicFunction.Pulse, 1) { }

        [Browsable(false)]
        public override string FuncSymbol => "^-^"; // Импульс

        private DateTime time;

        [Category("Настройки"), DisplayName("Время"), Description("Время импульса, сек")]
        public double WaitTime { get; set; } = 1.0;

        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            if (!input)
            {
                time = DateTime.Now + TimeSpan.FromSeconds(WaitTime);
                Out = false;
            }
            else
            {
                Out = time > DateTime.Now;
            }
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush)
        {
            rect.Inflate(-1, -1);
            graphics.FillRectangle(brush, rect);
            var sym = new RectangleF(rect.Location, new SizeF(rect.Width, rect.Height / 2));
            sym.Inflate(-6, -3);
            var w = sym.Width / 4;
            graphics.DrawLines(pen, [
                new PointF(sym.Left, sym.Bottom), new PointF(sym.Left + w, sym.Bottom), 
                new PointF(sym.Left + w, sym.Top), new PointF(sym.Right - w, sym.Top), 
                new PointF(sym.Right - w, sym.Bottom), new PointF(sym.Right, sym.Bottom)]);

            // время импульса, текст по-центру, в нижней части рамки элемента
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            var text = $"{WaitTime:0.#}s";
            var ms = graphics.MeasureString(text, font);
            var pt = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height - ms.Height);
            graphics.DrawString(text, font, fontbrush, pt, format);
        }
    }
}
