using Simulator.Model.Logic;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Outputs
{
    public class DO : CommonLogic, ICustomDraw
    {
        public DO() : base(LogicFunction.DigOut, 1, 0)
        {
        }

        [Category("Настройки"), DisplayName("Текст"), Description("Наименование выхода")]
        public string Description { get; set; } = "Дискретный выход";

        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            Out = input;
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
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            var text = " DO";
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            using var lampFont = new Font(font.FontFamily, font.Size - 2f);
            graphics.DrawString(text, lampFont, fontbrush, new PointF(rect.X + rect.Height / 2, rect.Y), format);
            // индекс элемента в списке
            if (index != 0)
            {
                text = $"L{index}";
                var ms = graphics.MeasureString(text, font);
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(text, font, fontbrush, new PointF(rect.X + rect.Height / 2, rect.Y + rect.Height - ms.Height), format);
            }
            format.LineAlignment = StringAlignment.Center;
            var staterect = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height);
            staterect.Inflate(0, -rect.Height / 3);
            staterect.Offset(0, -2);
            using var statebrush = new SolidBrush(Out ? Color.Lime : Color.Red);
            graphics.DrawString(Out ? "\"1\"" : "\"0\"", font, statebrush, staterect, format);
            var descrect = new RectangleF(rect.X + rect.Height, rect.Y, rect.Height * 3, rect.Height);
            graphics.DrawRectangles(pen, [descrect]);
            using var textFont = new Font("Arial Narrow", font.Size);
            graphics.DrawString(Description, textFont, fontbrush, descrect, format);
        }

        public override void Save(XElement xtem)
        {
            base.Save(xtem);
            XElement? xtance = xtem.Element("Instance");
            if (xtance == null)
            {
                xtance = new XElement("Instance");
                xtem.Add(xtance);
            }
            xtance.Add(new XElement("Description", Description));
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            Description = $"{xtance?.Element("Description")?.Value}";
        }
    }
}
