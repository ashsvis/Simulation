using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class FE : CommonLogic, ICustomDraw
    {
        public FE() : base(LogicFunction.Fe, 1) 
        {
            ((DigitalOutput)Outputs[0]).Value = false;
        }

        [Browsable(false)]
        public override string FuncSymbol => "FE"; // Детектор фронта

        private DateTime time;
        private readonly double waitTime = 0.2;
        private bool @out;

        public override void Calculate()
        {
            bool input = (bool)(GetInputValue(0) ?? false);
            bool output = (bool)(GetOutputValue(0) ?? false);
            if (!input && !output)
            {
                time = DateTime.Now + TimeSpan.FromSeconds(waitTime);
                @out = false;
            }
            else
                @out = time > DateTime.Now;
            SetValueToOut(0, @out);
        }

        public void Reset()
        {
            if ((bool)(GetOutputValue(0) ?? false))
                time = DateTime.Now;
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            // обозначение функции, текст по-центру, в верхней части рамки элемента
            var named = !string.IsNullOrEmpty(Name);
            if (named)
            {
                var msn = graphics.MeasureString(Name, font);
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.X + rect.Width - rect.Height / 2, rect.Y - msn.Height), format);
            }
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
                graphics.DrawString(text, font, fontbrush, new PointF(rect.X + rect.Width / 2, rect.Y +rect.Height - ms.Height), format);
            }
        }
    }
}
