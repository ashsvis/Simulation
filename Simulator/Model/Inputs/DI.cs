using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Simulator.Model.Inputs
{
    public class DI : CommonLogic, ICustomDraw, IChangeOrderDI, IManualCommand
    {
        public DI() : base(LogicFunction.DigInp, 0, 1)
        {
            OutputValues[0] = false;
        }

        [Category("Настройки"), DisplayName("Текст"), Description("Наименование входа")]
        public string Description { get; set; } = "Дискретный вход";

        [Category("Настройки"), DisplayName("Номер"), Description("Индекс входа")]
        public int Order { get; set; }

        public override void Calculate()
        {
            //bool output = (bool)(OutputValues[0] ?? false);
            bool output = (bool)(Project.ReadValue(ItemId, 0, ValueSide.Input, ValueKind.Digital)?.Value ?? false);
            //Out = output;
            //Project.WriteBoolValue(ItemId, 0, Out);
            //Project.WriteValue(ItemId, 0, ValueSide.Output, ValueKind.Digital, output);
        }

        public override void CalculateTargets(PointF location, ref SizeF size,
            Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins, Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var width = step * 24;
            var height = step * 6;
            size = new SizeF(width, height);
            itargets.Clear();
            ipins.Clear();
            // выходы
            var y = step + location.Y;
            var x = width + location.X;
            otargets.Clear();
            opins.Clear();
            y = height / 2 + location.Y;
            // значение выхода
            var ms = new SizeF(step * 2, step * 2);
            otargets.Add(0, new RectangleF(new PointF(x, y - ms.Height), ms));
            var pt = new PointF(x + step, y);
            opins.Add(0, pt);
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
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.X + rect.Width - rect.Height / 2, rect.Y - msn.Height), format);
            }

            // горизонтальная риска справа, напротив выхода
            graphics.DrawLine(pen, new PointF(rect.Right, rect.Y + rect.Height / 2),
                new PointF(rect.Right + Element.Step, rect.Y + rect.Height / 2));
            // значение выхода
            if (VisibleValues)
            {
                var textval = $"{Project.ReadValue(ItemId, 0, ValueSide.Output, ValueKind.Digital)?.Value ?? false}"[..1].ToUpper();
                var ms = graphics.MeasureString(textval, font);
                graphics.DrawString(textval, font, fontbrush, new PointF(rect.Right, rect.Y + rect.Height / 2 - ms.Height));
            }

            var funcrect = new RectangleF(rect.X + rect.Height * 3, rect.Y, rect.Height, rect.Height / 3);
            var text = $"DI{Order}";
            format.LineAlignment = StringAlignment.Center;
            using var lampFont = new Font(font.FontFamily, font.Size);
            graphics.DrawString(text, lampFont, fontbrush, funcrect, format);

            // индекс элемента в списке
            if (index != 0)
            {
                var labelrect = new RectangleF(rect.X + rect.Height * 3, rect.Bottom - rect.Height / 3, rect.Height, rect.Height / 3);
                text = $"L{index}";
                var ms = graphics.MeasureString(text, font);
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(text, font, fontbrush, labelrect, format);
            }

            var staterect = new RectangleF(rect.X + rect.Height * 3, rect.Y, rect.Height, rect.Height / 3);
            staterect.Offset(0, rect.Height / 3);
            var value = (bool)(Project.ReadValue(ItemId, 0, ValueSide.Output, ValueKind.Digital)?.Value ?? false);
            using var statebrush = new SolidBrush(value ? Color.Lime : Color.Red);
            graphics.DrawString(value ? "\"1\"" : "\"0\"", font, statebrush, staterect, format);

            var descrect = new RectangleF(rect.X, rect.Y, rect.Height * 3, rect.Height);
            graphics.FillRectangle(brush, descrect);
            graphics.DrawRectangles(pen, [descrect]);
            using var textFont = new Font("Arial Narrow", font.Size);
            graphics.DrawString(Description, textFont, fontbrush, descrect, format);
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
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

        public void SetValueToOut(int outputIndex, object? value)
        {
            if (outputIndex >= 0 && outputIndex < OutputValues.Length)
            {
                //OutputValues[outputIndex] = (bool)(value ?? false);
                Project.WriteValue(ItemId, outputIndex, ValueSide.Output, ValueKind.Digital, (bool)(value ?? false));
            }
        }

        public object? GetValueFromOut(int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < OutputValues.Length)
            {
                ValueItem? value = Model.Project.ReadValue(ItemId, outputIndex, ValueSide.Output, ValueKind.Digital);
                return value?.Value;
            }
            return null;
        }
    }
}
