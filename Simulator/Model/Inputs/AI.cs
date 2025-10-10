using Simulator.Model.Common;
using Simulator.Model.Fields;
using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using Simulator.Model.Mathematic;
using Simulator.View;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;

namespace Simulator.Model.Inputs
{
    public class AI : CommonAnalog, ICustomDraw, IChangeOrderDI, IContextMenu, IManualCommand
    {
        private (Guid, int, bool) linkSource = (Guid.Empty, 0, false);

        [Browsable(false)]
        public (Guid, int, bool) LinkSource => linkSource;

        public AI() : base(LogicFunction.AnaInp, 0, 1)
        {
            SetValueToOut(0, 0.0);
        }

        [Category("Настройки"), DisplayName("Текст"), Description("Наименование входа")]
        public string Description { get; set; } = "Аналоговый вход";

        [Category("Настройки"), DisplayName("Номер"), Description("Индекс входа")]
        public int Order { get; set; }

        public override void Calculate()
        {
            double output = (double)(GetOutputValue(0) ?? 0.0);
            if (linkSource.Item1 != Guid.Empty)
                output = (double)(Project.ReadValue(linkSource.Item1, 0, ValueDirect.Input, ValueKind.Analog)?.Value ?? 0.0);
            Project.WriteValue(ItemId, 0, ValueDirect.Output, ValueKind.Analog, output);
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

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangles(pen, [rect]);
            try
            {
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
                if (Project.Running && VisibleValues)
                {
                    var fp = CultureInfo.GetCultureInfo("en-US");
                    var val = (double)(GetOutputValue(0) ?? 0.0);
                    var textval = $"{val.ToString(fp)}";
                    var ms = graphics.MeasureString(textval, font);
                    graphics.DrawString(textval, font, fontbrush, new PointF(rect.Right, rect.Y + rect.Height / 2 - ms.Height));
                }

                var funcrect = new RectangleF(rect.X + rect.Height * 3, rect.Y, rect.Height, rect.Height / 3);
                var text = $"AI{Order}";
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

                if (Project.Running)
                {
                    var staterect = new RectangleF(rect.X + rect.Height * 3, rect.Y, rect.Height, rect.Height / 3);
                    staterect.Offset(0, rect.Height / 3);
                    var value = (double)(Project.ReadValue(ItemId, 0, ValueDirect.Output, ValueKind.Analog)?.Value ?? 0.0);
                    var fp = CultureInfo.GetCultureInfo("en-US");
                    graphics.DrawString(value.ToString(fp), font, brush, staterect, format);
                }
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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

        public object? GetValueFromOut(int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < Outputs.Length)
            {
                if (Outputs[outputIndex] is AnalogOutput output)
                    return output.Value;
                return null;
            }
            return null;
        }

        public override void AddMenuItems(ContextMenuStrip contextMenu)
        {
            ToolStripMenuItem item;
            item = new ToolStripMenuItem() { Text = "Связь с оборудованием...", Tag = this };
            item.Click += (s, e) =>
            {
                var dlg = new SelectLinkSourceForm(KindLinkSource.EquipmentOutputs, this.LinkSource);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    (Guid idSource, int pinOut) = dlg.Result;
                    if (idSource != Guid.Empty)
                    {
                        this.SetExternalLinkToInp(0, idSource, pinOut, true);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                    else if (idSource == Guid.Empty)
                    {
                        this.ResetExternalLinkToInp(0);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                }
            };
            contextMenu.Items.Add(item);
        }
    }
}
