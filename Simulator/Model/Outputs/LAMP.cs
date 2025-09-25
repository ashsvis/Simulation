using Simulator.Model.Logic;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Outputs
{
    public class LAMP : CommonLogic, ICustomDraw
    {
        private bool @out;

        public LAMP() : base(LogicFunction.Lamp, 1, 0) 
        {
        }

        [Category("Настройки"), DisplayName("Цвет"), Description("Цвет свечения лампы")]
        public Color Color { get; set; } = Color.Red;

        [Category("Настройки"), DisplayName("Текст"), Description("Наименование лампы")]
        public string Description { get; set; } = "Индикаторная лампа";

        public override void Calculate()
        {
            bool input = (bool)InputValues[0];
            @out = input;
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
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            // обозначение функции, текст по-центру, в верхней части рамки элемента
            var named = !string.IsNullOrEmpty(Name);
            if (named)
            {
                var msn = graphics.MeasureString(Name, font);
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.X + rect.Height / 2, rect.Y - msn.Height), format);
            }
            var text = "Лампа";
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
            var lamprect = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height);
            lamprect.Inflate(-rect.Height / 3, -rect.Height / 3);
            lamprect.Offset(0, -2);
            if (@out)
            {
                using var fill = new SolidBrush(Color);
                graphics.FillEllipse(fill, lamprect);
            }
            else 
            {
                using var stroke = new Pen(Color);
                graphics.DrawEllipse(stroke, lamprect);
            }
            var descrect = new RectangleF(rect.X + rect.Height, rect.Y, rect.Height * 3, rect.Height);
            graphics.DrawRectangles(pen, [descrect]);
            format.LineAlignment = StringAlignment.Center;
            using var textFont = new Font("Arial Narrow", font.Size);
            graphics.DrawString(Description, textFont, fontbrush, descrect, format);
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            xtance.Add(new XElement("Description", Description));
            xtance.Add(new XElement("Color", Color.Name));
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            Description = $"{xtance?.Element("Description")?.Value}";
            try
            {
                Color = Color.FromName($"{xtance?.Element("Color")?.Value}");
            }
            catch { }
        }
    }
}
