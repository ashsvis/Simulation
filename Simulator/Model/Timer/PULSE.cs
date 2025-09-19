using Simulator.Model.Logic;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;

namespace Simulator.Model.Timer
{
    public class PULSE : CommonLogic, ICustomDraw
    {
        public PULSE() : base(LogicFunction.Pulse, 1) 
        {
            OutputValues[0] = false;
        }

        [Browsable(false)]
        public override string FuncSymbol => "^-^"; // Импульс

        private DateTime time;
        private double waitTime = 1.0;

        [Category("Настройки"), DisplayName("Время"), Description("Время импульса, сек")]
        public double WaitTime 
        { 
            get => waitTime;
            set
            {
                if (Math.Abs(waitTime - value) < 0.0001) return;
                if (waitTime < 0.1) waitTime = 0.1;
                waitTime = value;
            }
        }
        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            if (!input && !Out)
            {
                time = DateTime.Now + TimeSpan.FromSeconds(WaitTime);
                Out = false;
            }
            else
                Out = time > DateTime.Now;
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
            graphics.DrawLines(pen, [
                new PointF(sym.Left, sym.Bottom), new PointF(sym.Left + w, sym.Bottom),
                new PointF(sym.Left + w, sym.Top), new PointF(sym.Right - w, sym.Top),
                new PointF(sym.Right - w, sym.Bottom), new PointF(sym.Right, sym.Bottom)]);

            // время импульса, текст по-центру, в нижней части рамки элемента
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            var text = $"{WaitTime:0.#}s";
            var ms = graphics.MeasureString(text, font);
            var pt = new PointF(rect.X + rect.Width / 2, rect.Y + (rect.Height - ms.Height) / 2);
            graphics.DrawString(text, font, fontbrush, pt, format);
            // индекс элемента в списке
            if (index != 0)
            {
                text = $"L{index}";
                ms = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, fontbrush, new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height - ms.Height), format);
            }
        }

        public override void Save(XElement xtem)
        {
            base.Save(xtem);
            XElement? xtance = xtem.Element("Instance");
            if (Math.Abs(WaitTime - 1.0) < 0.0001) return;
            if (xtance == null )
            {
                xtance = new XElement("Instance");
                xtem.Add(xtance);
            }
            xtance.Add(new XElement("WaitTime", WaitTime));
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            if (double.TryParse(xtance?.Element("WaitTime")?.Value, CultureInfo.GetCultureInfo("en-US"), out double value))
                WaitTime = value;
        }
    }
}
