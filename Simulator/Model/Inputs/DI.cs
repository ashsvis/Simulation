using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Inputs
{
    public class DI : CommonLogic, ICustomDraw, IChangeOrderDI, IManualCommand
    {
        private (Guid, int, bool) linkSource = (Guid.Empty, 0, false);

        public (Guid, int, bool) LinkSource => linkSource;

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
            if (linkSource.Item1 != Guid.Empty)
            {
                ValueItem? item = Project.ReadValue(linkSource.Item1, 0, ValueSide.Input, ValueKind.Digital);
                if (item != null)
                {
                    OutputValues[0] = item.Value ?? false;
                    Project.WriteValue(ItemId, 0, ValueSide.Output, ValueKind.Digital, item.Value);
                }
            }
        }

        /// <summary>
        /// Для создания связи записывается ссылка на метод,
        /// который потом вызывается для получения актуального значения
        /// </summary>
        /// <param name="inputIndex">номер входа</param>
        /// <param name="getMethod">Ссылка на метод, записываемая в целевом элементе, для этого входа</param>
        public void SetExternalLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            linkSource = (sourceId, outputPinIndex, byDialog);
        }

        public void ResetExternalLinkToInp(int inputIndex)
        {
            linkSource = (Guid.Empty, 0, false);
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
            if (linkSource.Item1 != Guid.Empty)
            {
                rect.Inflate(2, 2);
                graphics.DrawRectangles(pen, [rect]);
            }
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            xtance.Add(new XElement("Order", Order));
            xtance.Add(new XElement("Description", Description));
            if (linkSource.Item1 != Guid.Empty)
            {
                XElement xsource = new("External");
                xsource.Add(new XAttribute("Id", linkSource.Item1));
                if (linkSource.Item2 > 0)
                    xsource.Add(new XAttribute("PinIndex", linkSource.Item2));
                xtance.Add(xsource);
            }
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            if (int.TryParse(xtance?.Element("Order")?.Value, out int order))
                Order = order;
            Description = $"{xtance?.Element("Description")?.Value}";
            var xsource = xtance?.Element("External");
            if (xsource != null)
            {
                if (Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid guid) && guid != Guid.Empty)
                {
                    if (int.TryParse(xsource.Attribute("PinIndex")?.Value, out int outputIndex))
                        linkSource = (guid, outputIndex, true);
                    else
                        linkSource = (guid, 0, true);
                }
            }
        }

        public void SetValueToOut(int outputIndex, object? value)
        {
            if (outputIndex >= 0 && outputIndex < OutputValues.Length)
            {
                Project.WriteValue(ItemId, outputIndex, ValueSide.Output, ValueKind.Digital, (bool)(value ?? false));
            }
        }

        public object? GetValueFromOut(int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < OutputValues.Length)
            {
                ValueItem? value = Project.ReadValue(ItemId, outputIndex, ValueSide.Output, ValueKind.Digital);
                return value?.Value;
            }
            return null;
        }
    }
}
