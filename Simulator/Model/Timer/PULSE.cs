using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

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
            bool @out;
            if (!input && !Out)
            {
                time = DateTime.Now + TimeSpan.FromSeconds(WaitTime);
                @out = false;
            }
            else
                @out = time > DateTime.Now;
            Out = @out;
            Project.WriteValue(ItemId, 0, ValueSide.Output, ValueKind.Digital, Out);
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
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.X + rect.Height / 2, rect.Y - msn.Height), format);
            }
            rect.Inflate(-1, -1);
            graphics.FillRectangle(brush, rect);
            if (Project.Running && Out)
            {
                var kf = (time - DateTime.Now).TotalMilliseconds / (WaitTime * 1000);
                var r = rect;
                r.Width = (float)(rect.Width * kf);
                r.X += rect.Width - r.Width;
                using var br = new SolidBrush(Color.FromArgb(100, Color.Lime));
                graphics.FillRectangle(br, r);
            }
            var sym = new RectangleF(rect.Location, new SizeF(rect.Width, rect.Height / 3));
            sym.Inflate(-6, -3);
            var w = sym.Width / 4;
            graphics.DrawLines(pen, [
                new PointF(sym.Left, sym.Bottom), new PointF(sym.Left + w, sym.Bottom),
                new PointF(sym.Left + w, sym.Top), new PointF(sym.Right - w, sym.Top),
                new PointF(sym.Right - w, sym.Bottom), new PointF(sym.Right, sym.Bottom)]);

            // время импульса, текст по-центру, в нижней части рамки элемента
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
