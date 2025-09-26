using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;

namespace Simulator.Model.Timer
{
    public class ONDLY : CommonLogic, ICustomDraw
    {
        public ONDLY() : base(LogicFunction.OnDelay, 1) 
        {
            OutputValues[0] = false;
        }

        [Browsable(false)]
        public override string FuncSymbol => "_^"; // Задержка срабатывания

        private DateTime time;

        [Category("Настройки"), DisplayName("Задержка"), Description("Время задержки, сек")]
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
            Project.WriteBoolValue(ItemId, 0, Out);
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
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
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.X + rect.Height / 2, rect.Y - msn.Height), format);
            }
            rect.Inflate(-1, -1);
            
            graphics.FillRectangle(brush, rect);
            var sym = new RectangleF(rect.Location, new SizeF(rect.Width, rect.Height / 3));
            sym.Inflate(-6, -6);
            sym.Offset(0, sym.Height);
            graphics.DrawLine(pen, new PointF(sym.Left, sym.Top + sym.Height / 3), new PointF(sym.Right, sym.Top + sym.Height / 3));
            graphics.DrawLine(pen, new PointF(sym.Left, sym.Top), new PointF(sym.Left, sym.Top + sym.Height - 1));

            // время импульса, текст по-центру, в нижней части рамки элемента
            var text = $"{WaitTime:0.#}s";
            var ms = graphics.MeasureString(text, font);
            var pt = new PointF(rect.X + rect.Width / 2, rect.Y + (rect.Height - ms.Height) / 2);
            graphics.DrawString(text, font, fontbrush, pt, format);

            using var symfont = new Font(font.FontFamily, font.Size - 4, FontStyle.Italic);
            var symms = graphics.MeasureString("t1", symfont);
            graphics.DrawString("t1", symfont, fontbrush, new PointF(sym.Left, sym.Top + sym.Height / 3 - symms.Height), format);
            // индекс элемента в списке
            if (index != 0)
            {
                text = $"L{index}";
                ms = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, fontbrush, new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height - ms.Height), format);
            }
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            if (Math.Abs(WaitTime - 1.0) < 0.0001) return;
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
