using Simulator.Model.Interfaces;
using Simulator.View;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace Simulator.Model.Fields
{
    public class VALVE : CommonFields, ICustomDraw, ICalculate, IContextMenu
    {
        private (Guid, int, bool) openedLinkSource = (Guid.Empty, 0, false);
        private (Guid, int, bool) closedLinkSource = (Guid.Empty, 0, false);
        private (Guid, int, bool) commandLinkSource = (Guid.Empty, 0, false);

        private bool? isOpened;
        private bool? isClosed;
        private bool? isCommand;

        [Browsable(false)]
        public (Guid, int, bool) OpenedLinkSource => openedLinkSource;
        [Browsable(false)]
        public (Guid, int, bool) ClosedLinkSource => closedLinkSource;
        [Browsable(false)]
        public (Guid, int, bool) CommandLinkSource => commandLinkSource;

        [Category("Дизайн"), DisplayName("Ориентация")]
        public Orientation Orientation { get; set; }

        public VALVE() : base(FieldFunction.Valve) { }

        public override void CalculateTargets(PointF location, ref SizeF size,
            Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins, Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var height = step * 4;
            var width = step * 4;
            size = new SizeF(width, height);
            itargets.Clear();
            ipins.Clear();
            // входы
            var x = location.X;
            var y = location.Y + height / 2;
            // значение входа
            itargets.Add(0, RectangleF.Empty);
            ipins.Add(0, new PointF(x, y + step));
            // выход
            x = location.X + width;
            y = location.Y + height / 2;
            otargets.Clear();
            opins.Clear();
            // значение выхода
            otargets.Add(0, RectangleF.Empty);
            opins.Add(0, new PointF(x, y + step));
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
        {
            if (Orientation == Orientation.Horizontal)
            {
                var control = new RectangleF(rect.X + (rect.Height / 4), rect.Y, rect.Height / 2, rect.Height / 2);
                var valve = new RectangleF(rect.X, rect.Y + rect.Height / 2, rect.Width, rect.Height / 2);
                valve.Inflate(-0.5f, -0.5f);
                using var controlbrush = new SolidBrush(isCommand == true ? Color.Lime : Color.Black);
                graphics.FillRectangle(!Project.Running ? brush : isCommand == null ? Brushes.Magenta : controlbrush, control);
                graphics.DrawRectangles(pen, [control]);
                using GraphicsPath path = new();
                path.AddLines([
                        new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Right, valve.Bottom),
                    new PointF(valve.Right, valve.Top),
                    new PointF(valve.Left, valve.Bottom),
                    new PointF(valve.Left, valve.Top),
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Left + valve.Width / 2, control.Bottom),
                ]);
                using var valvebrush = new SolidBrush(isOpened == true ? Color.Lime : isClosed == true ? Color.Black : pen.Color);
                graphics.FillPath(!Project.Running ? brush : isOpened == null || isClosed == null || isOpened == true && isClosed == true ? Brushes.Magenta : valvebrush, path);
                graphics.DrawLines(pen,
                    [
                        new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Right, valve.Bottom),
                    new PointF(valve.Right, valve.Top),
                    new PointF(valve.Left, valve.Bottom),
                    new PointF(valve.Left, valve.Top),
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Left + valve.Width / 2, control.Bottom),
                ]);
                using var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.Left + rect.Width / 2, rect.Bottom), sf);
            }
            else
            {
                var control = new RectangleF(rect.X + rect.Width - rect.Height / 2, rect.Y + (rect.Height / 4), rect.Height / 2, rect.Height / 2);
                var valve = new RectangleF(rect.X, rect.Y, rect.Width / 2, rect.Height);
                valve.Inflate(-0.5f, -0.5f);
                using var controlbrush = new SolidBrush(isCommand == true ? Color.Lime : Color.Black);
                graphics.FillRectangle(!Project.Running ? brush : isCommand == null ? Brushes.Magenta : controlbrush, control);
                graphics.DrawRectangles(pen, [control]);
                using GraphicsPath path = new();
                path.AddLines(
                    [
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Right, valve.Bottom),
                    new PointF(valve.Left, valve.Bottom),
                    new PointF(valve.Right, valve.Top),
                    new PointF(valve.Left, valve.Top),
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(control.Left, valve.Top + valve.Height / 2),
                ]);
                using var valvebrush = new SolidBrush(isOpened == true ? Color.Lime : isClosed == true ? Color.Black : pen.Color);
                graphics.FillPath(!Project.Running ? brush : isOpened == null || isClosed == null || isOpened == true && isClosed == true ? Brushes.Magenta : valvebrush, path);
                graphics.DrawLines(pen,
                    [
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(valve.Right, valve.Bottom),
                    new PointF(valve.Left, valve.Bottom),
                    new PointF(valve.Right, valve.Top),
                    new PointF(valve.Left, valve.Top),
                    new PointF(valve.Left + valve.Width / 2, valve.Top + valve.Height / 2),
                    new PointF(control.Left, valve.Top + valve.Height / 2),
                ]);
                using var sf = new StringFormat();
                sf.Alignment = StringAlignment.Far;
                sf.LineAlignment = StringAlignment.Center;
                graphics.DrawString(Name, font, fontbrush, new PointF(rect.Left + valve.Width / 2, rect.Top + rect.Height / 2), sf);
            }
        }

        public void SetOpenedLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            openedLinkSource = (sourceId, outputPinIndex, byDialog);
        }

        public void ResetOpenedLinkToInp(int inputIndex)
        {
            openedLinkSource = (Guid.Empty, 0, false);
        }

        public void SetClosedLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            closedLinkSource = (sourceId, outputPinIndex, byDialog);
        }

        public void ResetClosedLinkToInp(int inputIndex)
        {
            closedLinkSource = (Guid.Empty, 0, false);
        }

        public void SetCommandLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            commandLinkSource = (sourceId, outputPinIndex, byDialog);
        }

        public void ResetCommandLinkToInp(int inputIndex)
        {
            commandLinkSource = (Guid.Empty, 0, false);
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            if (openedLinkSource.Item1 != Guid.Empty)
            {
                XElement xsource = new("OpenedState");
                xsource.Add(new XAttribute("Id", openedLinkSource.Item1));
                if (openedLinkSource.Item2 > 0)
                    xsource.Add(new XAttribute("PinIndex", openedLinkSource.Item2));
                xtance.Add(xsource);
            }
            if (closedLinkSource.Item1 != Guid.Empty)
            {
                XElement xsource = new("ClosedState");
                xsource.Add(new XAttribute("Id", closedLinkSource.Item1));
                if (closedLinkSource.Item2 > 0)
                    xsource.Add(new XAttribute("PinIndex", closedLinkSource.Item2));
                xtance.Add(xsource);
            }
            if (commandLinkSource.Item1 != Guid.Empty)
            {
                XElement xsource = new("CommandState");
                xsource.Add(new XAttribute("Id", commandLinkSource.Item1));
                if (commandLinkSource.Item2 > 0)
                    xsource.Add(new XAttribute("PinIndex", commandLinkSource.Item2));
                xtance.Add(xsource);
            }
            if (Orientation != Orientation.Horizontal)
            {
                XElement xsource = new("Design");
                xsource.Add(new XAttribute("Orientation", Orientation));
                if (Orientation != Orientation.Horizontal)
                    xtance.Add(xsource);
            }
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            var xsource = xtance?.Element("OpenedState");
            if (xsource != null)
            {
                if (Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid guid) && guid != Guid.Empty)
                {
                    if (int.TryParse(xsource.Attribute("PinIndex")?.Value, out int outputIndex))
                        openedLinkSource = (guid, outputIndex, true);
                    else
                        openedLinkSource = (guid, 0, true);
                }
            }
            xsource = xtance?.Element("ClosedState");
            if (xsource != null)
            {
                if (Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid guid) && guid != Guid.Empty)
                {
                    if (int.TryParse(xsource.Attribute("PinIndex")?.Value, out int outputIndex))
                        closedLinkSource = (guid, outputIndex, true);
                    else
                        closedLinkSource = (guid, 0, true);
                }
            }
            xsource = xtance?.Element("CommandState");
            if (xsource != null)
            {
                if (Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid guid) && guid != Guid.Empty)
                {
                    if (int.TryParse(xsource.Attribute("PinIndex")?.Value, out int outputIndex))
                        commandLinkSource = (guid, outputIndex, true);
                    else
                        commandLinkSource = (guid, 0, true);
                }
            }
            xsource = xtance?.Element("Design");
            if (xsource != null)
            {
                if (Enum.TryParse(typeof(Orientation), xsource.Attribute("Orientation")?.Value, out object? orientation))
                    Orientation = (Orientation)orientation;
            }
        }

        public void Calculate()
        {
            if (openedLinkSource.Item1 != Guid.Empty)
            {
                ValueItem? item = Project.ReadValue(openedLinkSource.Item1, 0, ValueSide.Input, ValueKind.Digital);
                isOpened = (bool?)item?.Value;
            }
            else
                isOpened = null;
            if (closedLinkSource.Item1 != Guid.Empty)
            {
                ValueItem? item = Project.ReadValue(closedLinkSource.Item1, 0, ValueSide.Input, ValueKind.Digital);
                isClosed = (bool?)item?.Value;
            }
            else
                isClosed = null;
            if (commandLinkSource.Item1 != Guid.Empty)
            {
                ValueItem? item = Project.ReadValue(commandLinkSource.Item1, 0, ValueSide.Output, ValueKind.Digital);
                isCommand = (bool?)item?.Value;
            }
            else
                isCommand = null;
        }

        public void ClearContextMenu(ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Clear();
        }

        public void AddMenuItems(ContextMenuStrip contextMenu)
        {
            ToolStripMenuItem item;
            item = new ToolStripMenuItem() { Text = "Связь для состояния ОТКРЫТО...", Tag = this };
            item.Click += (s, e) =>
            {
                var dlg = new SelectLinkSourceForm(KindLinkSource.EquipmentOutputs, this.OpenedLinkSource);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    (Guid idSource, int pinOut) = dlg.Result;
                    if (idSource != Guid.Empty)
                    {
                        this.SetOpenedLinkToInp(0, idSource, pinOut, true);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                    else if (idSource == Guid.Empty)
                    {
                        this.ResetOpenedLinkToInp(0);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                }
            };
            contextMenu.Items.Add(item);
            item = new ToolStripMenuItem() { Text = "Связь для состояния ЗАКРЫТО...", Tag = this };
            item.Click += (s, e) =>
            {
                var dlg = new SelectLinkSourceForm(KindLinkSource.EquipmentOutputs, this.ClosedLinkSource);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    (Guid idSource, int pinOut) = dlg.Result;
                    if (idSource != Guid.Empty)
                    {
                        this.SetClosedLinkToInp(0, idSource, pinOut, true);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                    else if (idSource == Guid.Empty)
                    {
                        this.ResetClosedLinkToInp(0);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                }
            };
            contextMenu.Items.Add(item);
            item = new ToolStripMenuItem() { Text = "Связь для управления...", Tag = this };
            item.Click += (s, e) =>
            {
                var dlg = new SelectLinkSourceForm(KindLinkSource.EquipmentInputs, this.CommandLinkSource);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    (Guid idSource, int pinInp) = dlg.Result;
                    if (idSource != Guid.Empty)
                    {
                        this.SetCommandLinkToInp(0, idSource, pinInp, true);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                    else if (idSource == Guid.Empty)
                    {
                        this.ResetCommandLinkToInp(0);
                        Project.Changed = true;
                        if (contextMenu.Tag is Action action) action.Invoke();
                    }
                }
            };
            contextMenu.Items.Add(item);
        }
    }
}
