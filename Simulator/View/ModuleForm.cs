using Simulator.Model;
using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.View;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Simulator
{
    public partial class ModuleForm : Form, IUpdateView
    {
        private readonly PanelForm panelForm;

        private readonly List<Element> items;
        private readonly List<Link> links;

        private Point firstMouseDown;
        private Point mousePosition;
        private Rectangle? ribbon = null;
        private bool partSelection = false;

        private Element? element;
        private int? pin;
        private bool? output;
        private PointF? linkFirstPoint;

        private Link? link;
        private int? segmentIndex;
        private bool? segmentVertical;

        public Model.Module Module { get; }

        public event EventHandler? ElementSelected;

        private bool dragging = false;
        private bool segmentmoving = false;

        private readonly View.ZoomControl zoomPad;

        public ModuleForm(PanelForm panelForm, Model.Module module)
        {
            InitializeComponent();
            zoomPad = new ZoomControl() 
            { 
                Dock = DockStyle.Fill, 
                ForeColor = Color.FromArgb(253, 254, 255),
                BackColor = Color.FromArgb(63, 64, 65),
                AllowDrop = true,
            };
            zoomPad.ContextMenuStrip = cmZoomPad;
            zoomPad.DragEnter += zoomPad_DragEnter;
            zoomPad.DragOver += zoomPad_DragOver;
            zoomPad.DragDrop += zoomPad_DragDrop;
            zoomPad.OnDraw += zoomPad_OnDraw;
            zoomPad.MouseDown += zoomPad_MouseDown;
            zoomPad.MouseMove += zoomPad_MouseMove;
            zoomPad.MouseUp += zoomPad_MouseUp;
            Controls.Add(zoomPad);
            if (Properties.Settings.Default.DarkMode)
                ThemeManager.ApplyDarkTheme(this);
            this.panelForm = panelForm;
            Module = module;
            Project.Changed = false;
            items = Module.Elements;
            links = Module.Links;
            //items.ForEach(item =>
            //{
                //if (item.Instance is IFunction instance)
                //    instance.ResultChanged += Item_ResultChanged;
            //});
            panelForm.SimulationTick += Module_SimulationTick;
        }

        private void Item_ResultChanged(object sender, ResultCalculateEventArgs args)
        {
            zoomPad.Invalidate();
        }

        private void ModuleForm_Load(object sender, EventArgs e)
        {
            items.ForEach(item => item.Selected = false);
            links.ForEach(item => item.SetSelect(false));
            ElementSelected?.Invoke(Module, EventArgs.Empty);
            timerInterface.Enabled = true;
        }

        private void ModuleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerInterface.Enabled = false;
            //items.ForEach(item =>
            //{
            //    if (item.Instance is IFunction instance)
            //        instance.ResultChanged -= Item_ResultChanged;
            //});
            panelForm.SimulationTick -= Module_SimulationTick;
        }

        private void Module_SimulationTick(object? sender, EventArgs e)
        {
            zoomPad.Invalidate();
        }

        private void zoomPad_DragEnter(object? sender, DragEventArgs e)
        {
            if (!Project.Running && e.Data != null)
            {
                if (e.Data.GetDataPresent(typeof(Element)))
                    e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void zoomPad_DragOver(object? sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (Project.Running) return;
            if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
            {
                zoomPad.Invalidate();
            }
        }

        public static PointF SnapToGrid(PointF pointF, int koeff = 1)
        {
            if ((ModifierKeys & Keys.Alt) == Keys.Alt)
                return pointF;
            float gridStep = Element.Step * koeff;
            return new PointF(
                (float)Math.Round(pointF.X / gridStep) * gridStep,
                (float)Math.Round(pointF.Y / gridStep) * gridStep);
        }

        private void zoomPad_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (Project.Running) return;
            if (e.Effect == DragDropEffects.Copy)
            {
                if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
                {
                    item.Assign(Module.Elements, Module.Links);
                    item.Instance ??= Activator.CreateInstance(item.Type);
                    if (item.Instance is IBlock block)
                        block.ConnectToLibrary();
                    item.Location = SnapToGrid(PrepareMousePosition(zoomPad.PointToClient(new Point(e.X, e.Y))));
                    //if (item.Instance is IFunction instance)
                    //    instance.ResultChanged += Item_ResultChanged;
                    if (item.Instance is ILinkSupport link)
                    {
                        link.SetItemId(item.Id);
                    }
                    items.ForEach(item => item.Selected = false);
                    links.ForEach(item => item.SetSelect(false));
                    item.Selected = true;
                    items.Add(item);
                    Project.Changed = true;
                    zoomPad.Invalidate();
                    ElementSelected?.Invoke(item.Instance, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Перерасчёт позиции мыши при масштабировании и панарамировании
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private PointF PrepareMousePosition(PointF p)
        {
            PointF[] arr = [p];
            Matrix matrix = new();

            var zoom = (float)zoomPad.ZoomScale;
            var origin = zoomPad.Origin;

            matrix.Translate(origin.X, origin.Y);
            matrix.Scale(1 / zoom, 1 / zoom);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        private bool TryGetModule(Point location, out Element? target)
        {
            var point = PrepareMousePosition(location);
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                if (item.Bounds.Contains(point))
                {
                    target = items[i];
                    return true;
                }
            }
            target = null;
            return false;
        }

        private bool TryGetPin(Point location, out Element? target, out int? pin, out PointF? point, out bool? output)
        {
            pin = null;
            point = null;
            target = null;
            output = null;
            var pt = PrepareMousePosition(location);
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                if (item.TryGetInput(pt, out pin, out point))
                {
                    output = false;
                    target = items[i];
                    return true;
                }
                if (item.TryGetOutput(pt, out pin, out point))
                {
                    output = true;
                    target = items[i];
                    return true;
                }
            }
            return false;
        }

        private bool TryGetFreeInputPin(Point location, out Element? target, out int? pin, out PointF? point, out bool? output)
        {
            pin = null;
            point = null;
            target = null;
            output = null;
            var pt = PrepareMousePosition(location);
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                if (item.TryGetInput(pt, out pin, out point) &&
                    item.Instance is ILinkSupport link && pin is int ipin && link.Inputs[ipin].LinkSource == null)
                {
                    output = false;
                    target = items[i];
                    return true;
                }
                if (item.TryGetOutput(pt, out pin, out point))
                {
                    output = true;
                    target = items[i];
                    return true;
                }
            }
            return false;
        }

        private bool TryGetLinkSegment(Point location, out Link? target, out int? index, out bool? vertical)
        {
            var point = PrepareMousePosition(location);
            for (var i = links.Count - 1; i >= 0; i--)
            {
                var link = links[i];
                foreach (var segment in link.Segments)
                {
                    if (segment.Item1.Contains(point))
                    {
                        target = links[i];
                        index = segment.Item2;
                        vertical = segment.Item3;
                        return true;
                    }
                }
            }
            target = null;
            index = null;
            vertical = null;
            return false;
        }

        private void zoomPad_OnDraw(object? sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;

            graphics.SmoothingMode = SmoothingMode.HighSpeed;
            graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            using var font = new Font("Consolas", Element.Step + 2f);
            try
            {
                // прорисовка внешних связей для входов
                foreach (var item in items)
                {
                    if (item.Instance is ILinkSupport sup)
                    {
                        for (var i = 0; i < sup.Inputs.Length; i++)
                        {
                            var ls = sup.Inputs[i].LinkSource;
                            if (ls == null) continue;
                            if (!ls.External) continue;
                            ValueItem? val = Project.ReadValue(ls.Id, ls.PinIndex, ValueDirect.Output, ValueKind.Digital);
                            Color color = val != null && val.Value != null ? (bool)val.Value == true ? Color.LimeGreen : Color.OrangeRed : Color.Gray;
                            using var exlinkpen = new Pen(Color.FromArgb(255, color), 0);
                            using var exlinkbrush = new SolidBrush(Color.FromArgb(255, color));

                            var (moduleName, elementName, inputName) = Project.GetOutputByElementId(ls.Id, ls.PinIndex);

                            if (!string.IsNullOrWhiteSpace(moduleName + elementName))
                            {
                                var pt = item.InputPins[i];
                                graphics.DrawLine(exlinkpen, PointF.Subtract(pt, new SizeF(Element.Step, 0)), pt);
                                var text = $"{moduleName}.{elementName}.{inputName}";
                                var ms = graphics.MeasureString(text, font);
                                var rect = new RectangleF(pt.X - Element.Step - ms.Width, pt.Y - ms.Height, ms.Width, ms.Height);
                                graphics.DrawRectangles(exlinkpen, [rect]);
                                graphics.DrawString(text, font, exlinkbrush, rect.Location);
                            }
                        }
                    }
                }

                // подсчёт внешних ссылок в модулях проекта
                Dictionary<string, List<(Element, int)>> dict = [];
                foreach (var module in Project.Modules.Union(Project.Equipment))
                {
                    foreach (var item in module.Elements)
                    {
                        if (item.Instance is ILinkSupport sup)
                        {
                            for (var i = 0; i < sup.Inputs.Length; i++)
                            {
                                var ls = sup.Inputs[i].LinkSource;
                                if (ls == null) continue;
                                if (!ls.External) continue;

                                var key = $"{ls.Id}.{ls.PinIndex}";
                                if (!dict.ContainsKey(key)) dict.Add(key, []);
                                dict[key].Add((item, i));
                            }
                        }
                    }
                }

                if (dict.Count > 0)
                {
                    // прорисовка внешних связей для выходов
                    foreach (var item in items)
                    {
                        for (var pinout = 0; pinout < item.OutputPins.Count; pinout++)
                        {
                            var key = $"{item.Id}.{pinout}";
                            if (!dict.ContainsKey(key)) continue;
                            var pt = item.OutputPins[pinout];
                            StringBuilder sb = new();
                            foreach (var (element, pininp) in dict[key])
                            {
                                var (moduleName, elementName, inputName) = Project.GetInputByElementId(element.Id, pininp);
                                sb.AppendLine($"{moduleName}.{elementName}.{inputName}");
                            }
                            var text = sb.ToString();
                            var ms = graphics.MeasureString(text, font);
                            var rect = new RectangleF(pt.X + Element.Step, item.Bounds.Bottom + Element.Step, ms.Width, ms.Height);
                            ValueItem? val = Project.ReadValue(item.Id, pinout, ValueDirect.Output, ValueKind.Digital);
                            Color color = val != null && val.Value != null ? (bool)val.Value == true ? Color.LimeGreen : Color.OrangeRed : Color.Gray;
                            using var exlinkpen = new Pen(Color.FromArgb(255, color), 0);
                            using var exlinkbrush = new SolidBrush(Color.FromArgb(255, color));
                            graphics.DrawLine(exlinkpen, pt, new PointF(pt.X + Element.Step, pt.Y));
                            graphics.DrawLine(exlinkpen, new PointF(pt.X + Element.Step, pt.Y), 
                                new PointF(pt.X + Element.Step, item.Bounds.Bottom + Element.Step));
                            graphics.DrawRectangles(exlinkpen, [rect]);
                            graphics.DrawString(text, font, exlinkbrush, rect.Location);
                        }
                    }
                }
                // прорисовка локальных линий связей
                using var linkpen = new Pen(zoomPad.ForeColor);
                foreach (var link in links.Where(x => !x.Selected))
                {
                    Element? source = items.FirstOrDefault(x => x.Id == link.SourceId);
                    if (source?.Instance is ILinkSupport lsup && lsup != null && link.SourcePinIndex < lsup.Outputs.Length)
                    {
                        var pin = link.SourcePinIndex;
                        ValueItem? val = Project.ReadValue(lsup.ItemId, pin, ValueDirect.Output, ValueKind.Digital);
                        if (val != null && val.Value != null)
                            link.SetValue(val.Value);
                    }
                    link.Draw(graphics, zoomPad.ForeColor);
                }
                // прорисовка узлов на связях
                foreach (var group in links.GroupBy(x => x.SourceId))
                {
                    if (group.Count() > 1)
                    {
                        List<(PointF, Guid)> list = [];
                        List<(RectangleF, Guid)> rects = [];
                        foreach (var link in group)
                        {
                            foreach (var point in link.Points.Distinct().Skip(1))
                                list.Add((point, link.Id));
                            for (var i = 1; i < link.Points.Count; i++)
                            {
                                var pt1 = link.Points[i - 1];
                                var pt2 = link.Points[i];
                                if (pt1.X != pt2.X || pt1.Y != pt2.Y)
                                {
                                    if (pt1.X != pt2.X || pt1.Y == pt2.Y)
                                        rects.Add((new RectangleF(Math.Min(pt1.X, pt2.X) + 3f, pt1.Y - 1f, Math.Abs(pt1.X - pt2.X) - 6f, 3f), link.Id));
                                    if (pt1.X == pt2.X || pt1.Y != pt2.Y)
                                        rects.Add((new RectangleF(pt1.X - 1f, Math.Min(pt1.Y, pt2.Y) + 3f, 3f, Math.Abs(pt1.Y - pt2.Y) - 6f), link.Id));
                                }
                            }
                        }

                        List<(PointF, Guid)> results = [];
                        foreach ((var point, var id1) in list)
                        {
                            foreach ((var rect, var id2) in rects)
                            {
                                if (id1 != id2 && rect.Contains(point) && !results.Contains((point, id1)))
                                    results.Add((point, id1));
                            }
                        }
                        foreach ((var pt, var id) in results)
                        {
                            var link = links.First(x => x.Id == id);
                            using var br = new SolidBrush(link.Selected ? Color.Magenta : zoomPad.ForeColor);
                            graphics.FillEllipse(br, new RectangleF(
                                PointF.Subtract(pt, new SizeF(2.5f, 2.5f)), new SizeF(5f, 5f)));
                        }
                    }
                }

                // прорисовка элементов
                var np = 1;
                foreach (var item in items)
                {
                    item.Index = np++;
                    if (item.Instance is ICustomDraw inst)
                        item.Draw(graphics, zoomPad.ForeColor, zoomPad.BackColor, inst.CustomDraw);
                    else
                        item.Draw(graphics, zoomPad.ForeColor, zoomPad.BackColor);
                }

                // прорисовка выбранных связей 
                foreach (var link in links.Where(x => x.Selected))
                {
                    link.Draw(graphics, zoomPad.ForeColor);
                }

                // прорисовка "резиновой" линии
                if (linkFirstPoint != null)
                {
                    var mp = PrepareMousePosition(mousePosition);
                    using var pen = new Pen(Color.Magenta, 0);
                    pen.DashStyle = DashStyle.Dash;
                    graphics.DrawLine(pen, (PointF)linkFirstPoint, mp);
                }
                // прорисовка "резиновой" рамки 
                if (ribbon != null && linkFirstPoint == null && !dragging && !segmentmoving)
                {
                    using var pen = new Pen(Color.FromArgb(200, partSelection ? Color.Lime : Color.Blue), 0);
                    pen.DashStyle = DashStyle.Dash;
                    graphics.DrawRectangle(pen, PrepareRect((Rectangle)ribbon));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private Rectangle PrepareRect(Rectangle rectangle)
        {
            var pt1 = PrepareMousePosition(rectangle.Location);
            var size = rectangle.Size;
            var pt2 = PrepareMousePosition(Point.Add(rectangle.Location, size));
            return Rectangle.Ceiling(new RectangleF(pt1, new SizeF(pt2.X - pt1.X, pt2.Y - pt1.Y)));
        }

        private static Link? BuildLink(Guid sourceId, int sourcePin, PointF sourcePinPoint, Guid destinationId, int destinationPin, PointF destinationPinPoint)
        {
            var link = new Link(Guid.NewGuid(), sourceId, sourcePin, destinationId, destinationPin, sourcePinPoint, destinationPinPoint);
            return link;
        }

        private void zoomPad_MouseDown(object? sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
            linkFirstPoint = null;
            element = null;
            pin = null;
            output = null;
            dragging = false;
            segmentmoving = false;
            if (!Project.Running)
            {
                if (TryGetLinkSegment(e.Location, out link, out segmentIndex, out segmentVertical) &&
                    link != null && segmentIndex != null && segmentVertical != null)
                {
                    if (!((Link)link).Selected)
                        ((Link)link).SetSelect(true);
                    else if ((ModifierKeys & Keys.Control) == Keys.Control)
                        ((Link)link).SetSelect(false);
                    links.Where(item => item.Id != ((Link)link).Id).ToList().ForEach(item => item.SetSelect(false));
                    if ((ModifierKeys & Keys.Control) != Keys.Control)
                        items.ForEach(item => item.Selected = false);
                    if (e.Button == MouseButtons.Left)
                    {
                        segmentmoving = ((Link)link).Selected;
                    }
                    ElementSelected?.Invoke(link, EventArgs.Empty);
                }
                else if (TryGetModule(e.Location, out element) &&
                    element != null && element.Instance != null ||
                    TryGetPin(e.Location, out element, out pin, out linkFirstPoint, out output) &&
                    element != null && element.Instance != null && output == true)
                {
                    if (!element.Selected && (ModifierKeys & Keys.Control) != Keys.Control)
                    {
                        items.Where(item => item != element).ToList().ForEach(item => item.Selected = false);
                        links.ForEach(item => item.SetSelect(false));
                        // выделение выходных связей элемента
                        foreach (var item in items.Where(x => x.Selected))
                        {
                            foreach (var link in links.Where(x => x.SourceId == item.Id))
                                link.SetSelect(true);
                        }
                    }
                    // выбор собственно элемента
                    if (output == null)
                    {
                        if (!element.Selected)
                        {
                            element.Selected = true;
                            // выделение выходных связей элемента
                            foreach (var item in items.Where(x => x.Selected))
                            {
                                foreach (var link in links.Where(x => x.SourceId == item.Id))
                                    link.SetSelect(true);
                            }
                        }
                        else if ((ModifierKeys & Keys.Control) == Keys.Control)
                            element.Selected = false;
                    }
                    if (e.Button == MouseButtons.Left)
                    {
                        dragging = output == null && element.Selected;
                    }

                    ElementSelected?.Invoke(items.Any(x => x.Selected) ?
                        items.Where(x => x.Selected).Select(x => x.Instance).ToArray() : null, EventArgs.Empty);
                }
                else
                {
                    if (!(TryGetFreeInputPin(e.Location, out Element? element, out _, out _, out _) && element?.Instance is IManualChange _))
                        linkFirstPoint = null;
                    items.ForEach(item => item.Selected = false);
                    links.ForEach(item => item.SetSelect(false));
                    ElementSelected?.Invoke(Module, EventArgs.Empty);
                }
                if (e.Button == MouseButtons.Right)
                {
                    linkFirstPoint = null;
                    cmZoomPad.Items.Clear();
                    cmZoomPad.Tag = () => zoomPad.Invalidate();
                    ToolStripMenuItem item;
                    if (element?.Instance is ILinkSupport func)
                    {
                        if (TryGetPin(e.Location, out element, out pin, out _, out output) && output == false && pin != null)
                        {
                            var ls = func.Inputs[(int)pin].LinkSource;
                            if (ls == null || ls != null && ls.External)
                            {
                                item = new ToolStripMenuItem() { Text = "Связь по входу...", Tag = element };
                                item.Click += (s, e) =>
                                {
                                    var menuItem = (ToolStripMenuItem?)s;
                                    if (menuItem?.Tag is Element element && element.Instance is ILinkSupport fn)
                                    {
                                        (Guid, int, bool)? link = null;
                                        if (ls != null) link = (ls.Id, ls.PinIndex, ls.External);
                                        var dlg = new SelectLinkSourceForm(KindLinkSource.LogicOutputs, link, Module);
                                        if (dlg.ShowDialog() == DialogResult.OK)
                                        {
                                            (Guid idSource, int pinOut) = dlg.Result;
                                            fn.SetValueLinkToInp((int)pin, idSource, pinOut, true);
                                            Project.Changed = true;
                                            zoomPad.Invalidate();
                                            panelForm.RefreshPanels();
                                        }
                                    }
                                };
                                cmZoomPad.Items.Add(item);
                            }
                            else if (func.Inputs[(int)pin].LinkSource != null)
                            {
                                if (ls != null && !ls.External)
                                {
                                    item = new ToolStripMenuItem() { Text = "Удалить связь по входу", Tag = element };
                                    item.Click += (s, e) =>
                                    {
                                        var menuItem = (ToolStripMenuItem?)s;
                                        if (menuItem?.Tag is Element element && element.Instance is ILinkSupport fn)
                                        {
                                            fn.ResetValueLinkToInp((int)pin);
                                            links.RemoveAll(link => link.DestinationId == element.Id && link.DestinationPinIndex == (int)pin);
                                            Project.Changed = true;
                                            zoomPad.Invalidate();
                                        }
                                    };
                                    cmZoomPad.Items.Add(item);
                                }
                            }
                        }
                        else if (TryGetPin(e.Location, out element, out pin, out _, out output) && element != null && output == true && pin != null)
                        {
                            // выбор выхода по правой кнопке
                        }
                        else if (TryGetModule(e.Location, out element) && output == null)
                        {
                            if (element is IContextMenu context)
                            {
                                context.AddMenuItems(cmZoomPad);
                                if (element.Instance is IAddInput || element.Instance is IRemoveInput)
                                    cmZoomPad.Items.Add(new ToolStripSeparator());
                                if (element.Instance is IAddInput)
                                {
                                    item = new ToolStripMenuItem() { Text = "Добавить вход", Tag = element };
                                    item.Click += (s, e) =>
                                    {
                                        var menuItem = (ToolStripMenuItem?)s;
                                        if (menuItem?.Tag is Element element && element.Instance is IAddInput add)
                                        {
                                            add.AddInput(element);
                                            links?.ForEach(link =>
                                            {
                                                if (link.SourceId == element.Id)
                                                {
                                                    for (var i = 0; i < element.OutputPins.Count; i++)
                                                        link.UpdateSourcePoint(element.OutputPins[i]);
                                                }
                                            });
                                            Project.Changed = true;
                                            zoomPad.Invalidate();
                                        }
                                    };
                                    cmZoomPad.Items.Add(item);
                                }
                                if (element.Instance is IRemoveInput)
                                {
                                    item = new ToolStripMenuItem() { Text = "Удалить последний вход", Tag = element };
                                    item.Click += (s, e) =>
                                    {
                                        var menuItem = (ToolStripMenuItem?)s;
                                        if (menuItem?.Tag is Element element &&
                                           element.Instance is ILinkSupport sup &&
                                           element.Instance is IRemoveInput remove)
                                        {
                                            var ls = sup.Inputs[^1].LinkSource;
                                            (Guid sourceId, int pinOut, bool external) = ls != null 
                                                   ? (ls.Id, ls.PinIndex, ls.External) 
                                                   : (Guid.Empty, 0, false);
                                            links?.RemoveAll(link => link.SourceId == sourceId && 
                                                link.DestinationId == element.Id && link.DestinationPinIndex == sup.Inputs.Length - 1);
                                            remove.RemoveInput(element);
                                            links?.ForEach(link => 
                                                { 
                                                    if (link.SourceId == element.Id)
                                                    {
                                                        for (var i = 0; i < element.OutputPins.Count; i++)
                                                            link.UpdateSourcePoint(element.OutputPins[i]);
                                                    }
                                                });
                                            Project.Changed = true;
                                            zoomPad.Invalidate();
                                        }
                                    };
                                    cmZoomPad.Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            zoomPad.Invalidate();
        }

        private void zoomPad_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Project.Running)
            {
                if (TryGetFreeInputPin(e.Location, out Element? element, out _, out _, out _) && element?.Instance is IManualChange _)
                    Cursor = Cursors.Hand;
                else
                    Cursor = Cursors.Default;
            }
            else
            {
                if (ribbon != null)
                    Cursor = Cursors.Cross;
                else if (TryGetLinkSegment(e.Location, out _, out _, out bool? vertical))
                    Cursor = vertical == true ? Cursors.VSplit : Cursors.HSplit;
                else if (TryGetModule(e.Location, out _))
                    Cursor = Cursors.SizeAll;
                else if (TryGetFreeInputPin(e.Location, out Element? element, out _, out _, out _) && element?.Instance is IManualChange _)
                    Cursor = Cursors.Hand;
                else
                    Cursor = Cursors.Default;
            }
            if (e.Button == MouseButtons.Left)
            {
                var mp = PrepareMousePosition(mousePosition);
                var pt = PrepareMousePosition(e.Location);
                var delta = new SizeF(pt.X - mp.X, pt.Y - mp.Y);
                mousePosition = e.Location;
                if (!delta.IsEmpty)
                {
                    if (dragging)
                    {
                        foreach (var item in items.Where(x => x.Selected))
                        {
                            item.Location = PointF.Add(item.Location, delta);
                            foreach (var link in links.Where(x => x.Selected && x.DestinationId == item.Id))
                                link.Offset(delta);
                        }
                        Project.Changed = true;
                    }
                    if (segmentmoving && link != null && segmentIndex != null && segmentVertical != null)
                    {
                        link?.OffsetSegment((int)segmentIndex, (bool)segmentVertical, delta);
                        Project.Changed = true;
                    }
                    if (!Project.Running && !dragging)
                    {
                        ribbon = new Rectangle(Math.Min(firstMouseDown.X, mousePosition.X), Math.Min(firstMouseDown.Y, mousePosition.Y),
                            Math.Abs(firstMouseDown.X - mousePosition.X), Math.Abs(firstMouseDown.Y - mousePosition.Y));
                        partSelection = firstMouseDown.Y > mousePosition.Y && firstMouseDown.X > mousePosition.X;
                    }
                    zoomPad.Invalidate();
                }
            }
        }

        private void zoomPad_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
            {
                if (!Project.Running && ribbon != null) // выбор рамкой
                {
                    var rect = PrepareRect((Rectangle)ribbon);
                    if ((ModifierKeys & Keys.Control) != Keys.Control)
                    {
                        items.ForEach(item => item.Selected = false);
                        links.ForEach(item => item.SetSelect(false));
                    }
                    if (partSelection)
                    {
                        items.ForEach(item =>
                        {
                            if (rect.IntersectsWith(Rectangle.Ceiling(item.Bounds)))
                                item.Selected = true;
                        });
                    }
                    else
                    {
                        items.ForEach(item =>
                        {
                            var r = Rectangle.Ceiling(item.Bounds);
                            var u = Rectangle.Intersect(rect, r);
                            if (u.Equals(r))
                                item.Selected = true;
                        });
                    }
                    foreach (var link in links)
                    {
                        if (items.Any(x => x.Selected && x.Id == link.SourceId) && items.Any(x => x.Selected && x.Id == link.DestinationId))
                            link.SetSelect(true);
                    }
                    ElementSelected?.Invoke(items.Any(x => x.Selected) ?
                        items.Where(x => x.Selected).Select(x => x.Instance).ToArray() : null, EventArgs.Empty);

                    ribbon = null;
                }
                if (dragging )  // перетаскивание элемент(а,ов)
                {
                    dragging = false;
                    if (firstMouseDown != e.Location)
                    {
                        foreach (var item in items.Where(x => x.Selected))
                        {
                            item.Location = SnapToGrid(item.Location);
                            UpdateLinks(item);
                        }
                    }
                }
                if (segmentmoving)  // перемещение сегмента линии связи
                {
                    segmentmoving = false;
                    if (firstMouseDown != e.Location)
                    {
                        if (link != null)
                            ((Link)link).SnapPointsToGrid(SnapToGrid);
                    }
                }
                var elementFirst = element;
                var pinFirst = pin;
                var outputFirst = output;
                var linkFirst = linkFirstPoint;
                if (Project.Running && TryGetFreeInputPin(e.Location, out element, out pin, out linkFirstPoint, out output) &&
                    element != null && pin != null && element.Instance is IManualChange tar && output == false)
                {
                    linkFirstPoint = null;
                    var value = tar.GetValueFromInp((int)pin);
                    if (value is bool bval)
                        tar.SetValueToInp((int)pin, !bval);
                    else
                    {
                        var dlg = new ChangeValueDialog((double)(value ?? 0.0));
                        if (dlg.ShowDialog() == DialogResult.OK)
                            tar.SetValueToInp((int)pin, dlg.EnteredValue);
                    }
                }
                if (Project.Running && TryGetPin(e.Location, out element, out pin, out _, out output) &&
                    element != null && pin != null && element.Instance is IManualCommand comm && output == true)
                {
                    var value = comm.GetValueFromOut((int)pin) ?? false;
                    if (value is bool bval)
                        comm.SetValueToOut((int)pin, !bval);
                    else
                    {
                        var dlg = new ChangeValueDialog((double)(value ?? 0.0));
                        if (dlg.ShowDialog() == DialogResult.OK)
                            comm.SetValueToOut((int)pin, dlg.EnteredValue);
                    }
                }
                if (TryGetPin(e.Location, out Element? elementSecond, out int? pinSecond, out PointF? linkSecondPoint, out bool? outputSecond) &&
                     elementSecond?.Instance is ILinkSupport target && elementFirst?.Instance is ILinkSupport source)
                {
                    if (elementFirst != elementSecond && outputFirst != outputSecond &&
                        linkFirstPoint != null && linkSecondPoint != null)
                    {
                        // создание связей между элементами
                        if (pinSecond != null && pinFirst != null && outputFirst == true && outputSecond == false)
                        {
                            // от выхода ко входу
                            if (target.Inputs[(int)pinSecond].LinkSource == null)
                            {
                                target.SetValueLinkToInp((int)pinSecond, elementFirst.Id, (int)pinFirst, false);
                                var link = BuildLink(elementFirst.Id, (int)pinFirst, (PointF)linkFirstPoint, elementSecond.Id, (int)pinSecond, (PointF)linkSecondPoint);
                                if (link != null)
                                {
                                    elementFirst.Selected = false;
                                    ((Link)link).SetSelect(true);
                                    links.Add((Link)link);
                                    ((Link)link).UpdateSourcePoint(elementFirst.OutputPins[((Link)link).SourcePinIndex]);
                                    UpdateLinks(elementSecond);
                                    Project.Changed = true;
                                }
                            }
                        }
                        else if (pinSecond != null && pinFirst != null && outputFirst == false && outputSecond == true)
                        {
                            // от входа к выходу
                            if (source.Inputs[(int)pinFirst].LinkSource == null)
                            {
                                source.SetValueLinkToInp((int)pinFirst, elementSecond.Id, (int)pinSecond, false);
                                var link = BuildLink(elementSecond.Id, (int)pinSecond, (PointF)linkSecondPoint, elementFirst.Id, (int)pinFirst, (PointF)linkFirstPoint);
                                if (link != null)
                                {
                                    elementFirst.Selected = false;
                                    ((Link)link).SetSelect(true);
                                    links.Add((Link)link);
                                    ((Link)link).UpdateDestinationPoint(elementFirst.InputPins[((Link)link).DestinationPinIndex]);
                                    UpdateLinks(elementSecond);
                                    Project.Changed = true;
                                }
                            }
                        }
                    }
                }
                else
                    element = null;
                pin = null;
                output = null;
                linkFirstPoint = null;
                zoomPad.Invalidate();
            }
        }

        private void UpdateLinks(Element item)
        {
            foreach (var link in links.Where(x => x.DestinationId == item.Id))
                link.UpdateDestinationPoint(item.InputPins[link.DestinationPinIndex]);
            foreach (var link in links.Where(x => x.SourceId == item.Id))
                link.UpdateSourcePoint(item.OutputPins[link.SourcePinIndex]);
            foreach (var link in links.Where(x => x.DestinationId == item.Id || x.SourceId == item.Id))
                link.SnapPointsToGrid(SnapToGrid);
        }

        public void UpdateView()
        {
            zoomPad.Invalidate();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveModule();
        }

        private void SaveModule()
        {
            if (Project.Changed)
            {
                Project.Save();
                Project.Changed = false;
            }
        }

        private void ModuleForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (items.Any(x => x.Selected) &&
                        MessageBox.Show("Удалить выделенные элементы?", "Удаление элементов",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DeleteAllSelectedElements();
                        Project.Changed = true;
                        zoomPad.Invalidate();
                    }
                    else if (links.Any(x => x.Selected) &&
                        MessageBox.Show("Удалить выделенные связи?", "Удаление связей",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        foreach (var link in links.Where(x => x.Selected))
                        {
                            var item = items.FirstOrDefault(x => x.Id == link.DestinationId);
                            if (item?.Instance is ILinkSupport func)
                                func.ResetValueLinkToInp(link.DestinationPinIndex);
                        }
                        links.RemoveAll(link => link.Selected);
                        Project.Changed = true;
                        zoomPad.Invalidate();
                    }
                    break;
                case Keys.A:
                    if (e.Control)
                    {
                        items.ForEach(x => x.Selected = true);
                        links.ForEach(x => x.SetSelect(true));
                        zoomPad.Invalidate();
                    }
                    break;
                case Keys.X:
                    if (e.Control)
                    {
                        CutSelectedElementsAndLinkToClipboard();
                    }
                    break;
                case Keys.C:
                    if (e.Control)
                    {
                        CopySelectedElementsAndLinkToClipboard();
                    }
                    break;
                case Keys.V:
                    if (e.Control)
                    {
                        PasteElementsAndLinksFromClipboard();
                        zoomPad.Invalidate();
                    }
                    break;
                case Keys.S:
                    if (e.Control)
                    {
                        SaveModule();
                    }
                    break;
            }
        }

        internal void PasteElementsAndLinksFromClipboard()
        {
            if (Clipboard.ContainsData("XML Spreadsheet"))
            {
                var xmlStream = (MemoryStream?)Clipboard.GetData("XML Spreadsheet");
                if (xmlStream != null)
                {
                    items.ForEach(item => item.Selected = false);
                    links.ForEach(item => item.SetSelect(false));
                    List<Element> elements = [];
                    List<Link> elementlinks = [];
                    Dictionary<Guid, Guid> guids = [];
                    XDocument doc = XDocument.Load(xmlStream);
                    Module.LoadElements(doc.Root, elements);
                    var makedCopy = items.Any(x => elements.Any(y => y.Id == x.Id));
                    if (makedCopy)
                    {
                        // замена Id на новый, сохранение уникальности Id для копии элемента
                        // составление словаря замен
                        foreach (Element element in elements)
                        {
                            var newId = Guid.NewGuid();
                            guids.Add(element.Id, newId);
                            element.Id = newId;
                        }
                        // замена SourceId для входный связей из словаря замен
                        foreach (Element element in elements)
                        {
                            if (element.Instance is ILinkSupport link)
                            {
                                foreach (var input in link.Inputs)
                                {
                                    var ls = input.LinkSource;
                                    var seek = ls != null
                                           ? (ls.Id, ls.PinIndex, ls.External)
                                           : (Guid.Empty, 0, false);
                                    link.UpdateInputLinkSources(seek,
                                        guids.TryGetValue(seek.Item1, out Guid value) ? value : Guid.Empty);
                                }
                            }
                        }
                    }
                    // установление связей
                    Module.ConnectLinks(elements);
                    Model.Module.LoadVisualLinks(doc.Root, elementlinks);

                    foreach (Element element in elements)
                    {
                        element.Selected = true;
                        items.Add(element);
                    }
                    foreach (Link link in elementlinks)
                    {
                        if (makedCopy && (!guids.ContainsKey(link.SourceId) || !guids.ContainsKey(link.DestinationId))) continue;
                        link.SetSourceId(makedCopy ? guids[link.SourceId] : link.SourceId);
                        link.SetDestinationId(makedCopy ? guids[link.DestinationId] : link.DestinationId);
                        link.SetSelect(true);
                        links.Add(link);
                    }
                    Project.Changed = true;
                    UpdateView();
                }
            }
        }

        internal void CopySelectedElementsAndLinkToClipboard()
        {
            var root = new XElement("Clipboard");
            XDocument doc = new(new XComment("Выборка"), root);
            XElement xitems = new("Elements");
            root.Add(xitems);
            foreach (var item in items.Where(x => x.Selected))
            {
                var holder = new XElement("Element");
                item.Save(holder);
                xitems.Add(holder);
            }
            if (items.Any(x => x.Selected))
            {
                XElement xlinks = new("Links");
                root.Add(xlinks);
                foreach (var item in links.Where(x => x.Selected))
                {
                    var holder = new XElement("Link");
                    item.Save(holder);
                    xlinks.Add(holder);
                }
            }
            string xml = root.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            using var xmlStream = new MemoryStream(bytes);
            Clipboard.SetData("XML Spreadsheet", xmlStream);
        }

        internal void CutSelectedElementsAndLinkToClipboard()
        {
            CopySelectedElementsAndLinkToClipboard();
            DeleteAllSelectedElements();
        }

        private void DeleteAllSelectedElements()
        {
            List<Element> selected = [];
            foreach (var item in items)
            {
                if (item.Selected)
                {
                    selected.Add(item);
                    if (item.Instance is ILinkSupport func)
                        links?.RemoveAll(link => link.SourceId == item.Id || link.DestinationId == item.Id);
                }
            }
            foreach (var element in selected)
            {
                foreach (var item in items)
                {
                    if (item.Instance is ILinkSupport func)
                    {
                        var n = 0;
                        foreach (var input in func.Inputs)
                        {
                            var seek = input.LinkSource;
                            if (seek != null && seek.Id == element.Id)
                                func.ResetValueLinkToInp(n);
                            n++;
                        }
                    }
                }
            }
            items.RemoveAll(x => x.Selected);
            Project.Changed = true;
            ElementSelected?.Invoke(null, EventArgs.Empty);
            Project.RefreshPanels();
        }

        private void timerInterface_Tick(object? sender, EventArgs e)
        {
            // stub
        }

        private void cmZoomPad_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = Project.Running || cmZoomPad.Items.Count == 0;
        }
    }
}
