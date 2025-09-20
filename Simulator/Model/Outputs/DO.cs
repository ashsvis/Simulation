using Simulator.Model.Logic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Simulator.Model.Outputs
{
    public class DO : CommonLogic, ICustomDraw, IChangeOrderDO
    {
        private bool @out;

        public DO() : base(LogicFunction.DigOut, 1, 0)
        {
        }

        [Category("Настройки"), DisplayName("Текст"), Description("Наименование выхода")]
        public string Description { get; set; } = "Дискретный выход";

        [Category("Настройки"), DisplayName("Номер"), Description("Индекс выхода")]
        public int Order { get; set; }

        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            if (@out != input)
            {
                @out = input;
                Debug.WriteLine($"{Name}={@out}");
            }
        }

        public override void CalculateTargets(PointF location, ref SizeF size,
            Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins, Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var width = step * 24;
            var height = step * 6;
            size = new SizeF(width, height);
            // входы
            var y = step + location.Y;
            var x = -step + location.X;
            itargets.Clear();
            ipins.Clear();
            y += step * 2;
            // значение входа
            var ms = new SizeF(step * 2, step * 2);
            itargets.Add(0, new RectangleF(new PointF(x - ms.Width + step, y - ms.Height), ms));
            ipins.Add(0, new PointF(x, y));
            otargets.Clear();
            opins.Clear();
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);

            var funcrect = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height / 3);
            var text = $"DO{Order}";
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            using var lampFont = new Font(font.FontFamily, font.Size);
            graphics.DrawString(text, lampFont, fontbrush, funcrect, format);

            // индекс элемента в списке
            if (index != 0)
            {
                var labelrect = new RectangleF(rect.X, rect.Bottom - rect.Height / 3, rect.Height, rect.Height / 3);
                text = $"L{index}";
                var ms = graphics.MeasureString(text, font);
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(text, font, fontbrush, labelrect, format);
            }

            var staterect = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height / 3);
            staterect.Offset(0, rect.Height / 3);
            using var statebrush = new SolidBrush(@out ? Color.Lime : Color.Red);
            graphics.DrawString(@out ? "\"1\"" : "\"0\"", font, statebrush, staterect, format);

            var descrect = new RectangleF(rect.X + rect.Height, rect.Y, rect.Height * 3, rect.Height);
            graphics.DrawRectangles(pen, [descrect]);
            using var textFont = new Font("Arial Narrow", font.Size);
            graphics.DrawString(Description, textFont, fontbrush, descrect, format);
        }

        public override void Save(XElement xtance)
        {
            xtance.Add(new XElement("Order", Order));
            xtance.Add(new XElement("Description", Description));
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            if (int.TryParse(xtance?.Element("Order")?.Value, out int order))
                Order = order;
            Description = $"{xtance?.Element("Description")?.Value}";
        }
    }
}
