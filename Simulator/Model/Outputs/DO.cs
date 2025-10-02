using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using Simulator.View;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Simulator.Model.Outputs
{
    public class DO : CommonLogic, ICustomDraw, IChangeOrderDO, IContextMenu
    {
        private (Guid, int, bool) linkSource = (Guid.Empty, 0, false);

        public (Guid, int, bool) LinkSource => linkSource;

        public DO() : base(LogicFunction.DigOut, 1, 0)
        {
        }

        [Category("Настройки"), DisplayName("Текст"), Description("Наименование выхода")]
        public string Description { get; set; } = "Дискретный выход";

        [Category("Настройки"), DisplayName("Номер"), Description("Индекс выхода")]
        public int Order { get; set; }

        public override void Calculate()
        {
            bool input = GetInputValue(0);
            Project.WriteValue(ItemId, 0, ValueSide.Input, ValueKind.Digital, input);
            if (linkSource.Item1 != Guid.Empty)
            {
                Project.WriteValue(linkSource.Item1, 0, ValueSide.Output, ValueKind.Digital, input);
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
            try
            {
                using var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                // обозначение функции, текст по-центру, в верхней части рамки элемента
                var named = !string.IsNullOrEmpty(Name);
                if (named)
                {
                    var msn = graphics.MeasureString(Name, font);
                    graphics.DrawString(Name, font, fontbrush, new PointF(rect.X + rect.Height / 2, rect.Y - msn.Height), format);
                }

                var funcrect = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height / 3);
                var text = $"DO{Order}";
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
                if (Project.Running)
                {
                    var staterect = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height / 3);
                    staterect.Offset(0, rect.Height / 3);
                    bool value = GetInputValue(0);
                    using var statebrush = new SolidBrush(value ? Color.Lime : Color.Red);
                    graphics.DrawString(value ? "\"1\"" : "\"0\"", font, statebrush, staterect, format);
                }
                var descrect = new RectangleF(rect.X + rect.Height, rect.Y, rect.Height * 3, rect.Height);
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

        public override void AddMenuItems(ContextMenuStrip contextMenu)
        {
            ToolStripMenuItem item;
            item = new ToolStripMenuItem() { Text = "Связь с оборудованием...", Tag = this };
            item.Click += (s, e) =>
            {
                var dlg = new SelectLinkSourceForm(KindLinkSource.EquipmentInputs, this.LinkSource);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    (Guid idSource, int pinInp) = dlg.Result;
                    if (idSource != Guid.Empty)
                    {
                        this.SetExternalLinkToInp(0, idSource, pinInp, true);
                        Project.Changed = true;
                    }
                    else if (idSource == Guid.Empty)
                    {
                        this.ResetExternalLinkToInp(0);
                        Project.Changed = true;
                    }
                }
            };
            contextMenu.Items.Add(item);
        }
    }
}
