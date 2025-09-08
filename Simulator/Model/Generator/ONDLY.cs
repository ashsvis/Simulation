using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Generator
{
    public class ONDLY : CommonLogic, ICustomDraw
    {
        public ONDLY() : base(LogicFunction.OnDelay, 1) { }

        [Browsable(false)]
        public override string FuncSymbol => "_^"; // Задержка срабатывания

        private DateTime time;

        [Category("Настройки"), DisplayName("Задержка"), Description("Время задержки, сек")] // Удержание
        public double WaitTime { get; set; } = 1.0;

        private bool state;

        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            if (!input && !state)
            {
                time = DateTime.Now + TimeSpan.FromSeconds(WaitTime);
                state = false;
            }
            else
                state = time > DateTime.Now;
            Out = !state && input;
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush)
        {
            rect.Inflate(-1, -1);
            
            graphics.FillRectangle(brush, rect);
            var sym = new RectangleF(rect.Location, new SizeF(rect.Width, rect.Height / 2));
            sym.Inflate(-6, -6);
            sym.Offset(0, sym.Height);
            graphics.DrawLine(pen, new PointF(sym.Left, sym.Top + sym.Height / 2), new PointF(sym.Right, sym.Top + sym.Height / 2));
            graphics.DrawLine(pen, new PointF(sym.Left, sym.Top), new PointF(sym.Left, sym.Top + sym.Height));

            // время импульса, текст по-центру, в нижней части рамки элемента
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            var text = $"{WaitTime:0.#}s";
            var ms = graphics.MeasureString(text, font);
            var pt = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height - ms.Height);
            graphics.DrawString(text, font, fontbrush, pt, format);

            using var symfont = new Font(font.FontFamily, font.Size - 3, FontStyle.Italic);
            var symms = graphics.MeasureString("t1", symfont);
            graphics.DrawString("t1", symfont, fontbrush, new PointF(sym.Left, sym.Top + sym.Height / 2 - symms.Height * 1.2f), format);
        }
    }
}
