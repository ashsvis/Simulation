using Simulator.Model;
using Simulator.View;
using System.Drawing.Drawing2D;

namespace Simulator
{
    public partial class ModuleForm : Form, IUpdateView
    {
        private readonly PanelForm panelForm;

        private readonly List<Element> items;
        private Cell[,] grid = new Cell[0, 0];
        private List<Link> links = [];

        private Point firstMouseDown;
        private Point mousePosition;

        public ModuleForm(PanelForm panelForm, Module module)
        {
            InitializeComponent();
            this.panelForm = panelForm;
            Module = module;
            items = module.Items;

            BuildLinks();

            panelForm.SimulationTick += Module_SimulationTick;
        }

        private void Item_ResultChanged(object sender, Model.ResultCalculateEventArgs args)
        {
            zoomPad.Invalidate();
        }

        private void ChildForm_Load(object sender, EventArgs e)
        {

        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            items.ForEach(item =>
            {
                if (item.Instance is IFunction instance)
                    instance.ResultChanged -= Item_ResultChanged;
            });
            panelForm.SimulationTick -= Module_SimulationTick;
        }

        private void Module_SimulationTick(object? sender, EventArgs e)
        {
            items.ForEach(item =>
            {
                try
                {
                    if (item.Instance is ICalculate instance)
                        instance.Calculate();
                }
                catch
                {

                }
            });
            zoomPad.Invalidate();
            tsbSave.Enabled = Module.Changed;
        }

        private void zoomPad_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.GetDataPresent(typeof(Element)))
                    e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void zoomPad_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
            {
                zoomPad.Invalidate();
            }
        }

        public static PointF SnapToGrid(PointF pointF)
        {
            float gridStep = Element.Step * 2;
            return new PointF(
                (float)Math.Round(pointF.X / gridStep) * gridStep,
                (float)Math.Round(pointF.Y / gridStep) * gridStep);
        }

        private void zoomPad_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Effect == DragDropEffects.Copy)
            {
                if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
                {
                    item.Instance = (IFunction?)Activator.CreateInstance(item.Type);
                    item.Location = SnapToGrid(PrepareMousePosition(zoomPad.PointToClient(new Point(e.X, e.Y))));
                    if (item.Instance is IFunction instance)
                        instance.ResultChanged += Item_ResultChanged;
                    items.ForEach(item => item.Selected = false);
                    item.Selected = true;
                    items.Add(item);
                    Module.Changed = true;
                    BuildLinks();
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
                    item.Instance is ILink function && pin is int ipin && !function.LinkedInputs[ipin])
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

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // прорисовка связей
            using var linkpen = new Pen(Color.FromArgb(100, zoomPad.ForeColor));
            foreach (var link in links)
            {
                link.Draw(graphics, zoomPad.ForeColor);
            }
            //foreach (var item in items)
            //{
            //    if (item.Instance is IFunction function && function.LinkedInputs.Any(x => x == true))
            //    {
            //        var n = 0;
            //        foreach (var isLinked in function.LinkedInputs)
            //        {
            //            if (isLinked)
            //            {
            //                if (item.InputPins.TryGetValue(n, out PointF targetPinPoint))
            //                {
            //                    (Guid sourceId, int outputIndex) = function.InputLinkSources[n];
            //                    var source = items.FirstOrDefault(x => x.Id == sourceId);
            //                    if (source != null)
            //                    {
            //                        var sourcePinPoint = source.OutputPins[outputIndex];
            //                        //DrawLink(graphics, zoomPad.ForeColor, sourcePinPoint, targetPinPoint, source.Bounds, item.Bounds);

            //                        graphics.DrawLine(linkpen, sourcePinPoint, targetPinPoint);
            //                    }
            //                }
            //            }
            //            n++;
            //        }
            //    }
            //}
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
            // прорисовка узлов сетки
            //using var brush = new SolidBrush(Color.Gray);
            //using var font = new Font("Consolas", 3f);
            //for (var y = 0; y < grid.GetLength(0); y++)
            //{
            //    for (var x = 0; x < grid.GetLength(1); x++)
            //    {
            //        if (grid[y, x].Kind < -1)
            //        {
            //            graphics.FillEllipse(Brushes.Yellow, new RectangleF(
            //                PointF.Subtract(grid[y, x].Point, new SizeF(0.5f, 0.5f)), new SizeF(1f, 1f)));
            //        }
            //        else if (grid[y, x].Kind == -1)
            //        {
            //            graphics.FillEllipse(Brushes.Red, new RectangleF(
            //                PointF.Subtract(grid[y, x].Point, new SizeF(0.5f, 0.5f)), new SizeF(1f, 1f)));
            //        }
            //        else if (grid[y, x].Kind == 0)
            //        {
            //            graphics.FillEllipse(Brushes.Gray, new RectangleF(
            //                PointF.Subtract(grid[y, x].Point, new SizeF(0.5f, 0.5f)), new SizeF(1f, 1f)));
            //        }
            //        else if (grid[y, x].Kind > 0)
            //        {
            //            graphics.DrawString(grid[y, x].Kind.ToString(), font, brush, grid[y, x].Point);
            //        }
            //    }
            //}
            // прорисовка "резиновой" линии
            if (linkFirstPoint != null)
            {
                var mp = PrepareMousePosition(mousePosition);
                using var pen = new Pen(Color.Silver, 0);
                pen.DashStyle = DashStyle.Dash;
                graphics.DrawLine(pen, (PointF)linkFirstPoint, mp);
            }
        }

        private void BuildLinks()
        {
            // подготовка сетки с тенями от существующих элентов и связей
            grid = BuildGrid();
            // составление списка связей для построения
            links = [];
            foreach (var item in items)
            {
                if (item.Instance is ILink function && function.LinkedInputs.Any(x => x == true))
                {
                    var n = 0;
                    foreach (var isLinked in function.LinkedInputs)
                    {
                        if (isLinked)
                        {
                            if (item.InputPins.TryGetValue(n, out PointF targetPinPoint))
                            {
                                (Guid sourceId, int outputIndex) = function.InputLinkSources[n];
                                var source = items.FirstOrDefault(x => x.Id == sourceId);
                                if (source != null)
                                {
                                    var sourcePinPoint = source.OutputPins[outputIndex];
                                    links.Add(new Link(sourcePinPoint, targetPinPoint));
                                }
                            }
                        }
                        n++;
                    }
                }
            }
            foreach (var link in links.OrderBy(link => link.Length))
            {
                // помещение затравки волны в сетку
                var tpt = link.SourcePoint;
                var spt = link.TargetPoint;
                var tx = -1;
                var ty = -1;
                for (var y = 0; y < grid.GetLength(0); y++)
                {
                    for (var x = 0; x < grid.GetLength(1); x++)
                    {
                        if (spt == grid[y, x].Point)
                            grid[y, x].Kind = 1;
                        if (tpt == grid[y, x].Point)
                        {
                            tx = x;
                            ty = y;
                        }
                    }
                }
                if (tx < 0 || ty < 0) continue;
                // генерация волны
                var changed = true;
                var wave = 1;
                while (changed) 
                {
                    changed = false;
                    for (var y = 0; y < grid.GetLength(0); y++)
                    {
                        for (var x = 0; x < grid.GetLength(1); x++)
                        {
                            if (grid[y, x].Kind == wave)
                            {
                                if (y > 0 && grid[y - 1, x].Kind == 0)
                                {
                                    grid[y - 1, x].Kind = wave + 1;
                                    changed = true;
                                }
                                if (y < grid.GetLength(0) - 1 && grid[y + 1, x].Kind == 0)
                                {
                                    grid[y + 1, x].Kind = wave + 1;
                                    changed = true;
                                }
                                if (x > 0 && grid[y, x - 1].Kind == 0)
                                {
                                    grid[y, x - 1].Kind = wave + 1;
                                    changed = true;
                                }
                                if (x < grid.GetLength(1) - 1 && grid[y, x + 1].Kind == 0)
                                {
                                    grid[y, x + 1].Kind = wave + 1;
                                    changed = true;
                                }
                                if (changed && grid[ty, tx].Kind > 0)
                                    goto exit;
                            }
                        }
                    }
                    wave++;
                }
            exit: if (changed) // путь найден
                {
                    var vector = LinkVector.Horizontal;
                    // следуем путём
                    var x = tx;
                    var y = ty;
                    wave = grid[y, x].Kind;
                    grid[y, x].Kind = -2;
                    link.Clear();
                    link.AddPoint(grid[y, x].Point);
                    while (wave > 1)
                    {
                        if (y > 0 && grid[y - 1, x].Kind == wave - 1)
                        {
                            if (vector != LinkVector.Vertical)
                                link.AddPoint(grid[y, x].Point);
                            y--;
                            grid[y, x].Kind = -2;
                            vector = LinkVector.Vertical;
                        }
                        else if (y < grid.GetLength(0) - 1 && grid[y + 1, x].Kind == wave - 1)
                        {
                            if (vector != LinkVector.Vertical)
                                link.AddPoint(grid[y, x].Point);
                            y++;
                            grid[y, x].Kind = -2;
                            vector = LinkVector.Vertical;
                        }
                        else if (x > 0 && grid[y, x - 1].Kind == wave - 1)
                        {
                            if (vector != LinkVector.Horizontal)
                                link.AddPoint(grid[y, x].Point);
                            x--;
                            grid[y, x].Kind = -2;
                            vector = LinkVector.Horizontal;
                        }
                        else if (x < grid.GetLength(1) - 1 && grid[y, x + 1].Kind == wave - 1)
                        {
                            if (vector != LinkVector.Horizontal)
                                link.AddPoint(grid[y, x].Point);
                            x++;
                            grid[y, x].Kind = -2;
                            vector = LinkVector.Horizontal;
                        }
                        wave--;
                    }
                    link.AddPoint(grid[y, x].Point);
                }
                // очистка от предыдущей волны
                for (var iy = 0; iy < grid.GetLength(0); iy++)
                {
                    for (var ix = 0; ix < grid.GetLength(1); ix++)
                    {
                        if (grid[iy, ix].Kind > 0 || grid[iy, ix].Kind == -2)
                            grid[iy, ix].Kind = 0;
                    }
                }
            }
        }

        private Cell[,] BuildGrid()
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;
            foreach (var item in items)
            {
                if (item.Bounds.X < minX) minX = item.Bounds.X;
                if (item.Bounds.Y < minY) minY = item.Bounds.Y;
                if (item.Bounds.X > maxX) maxX = item.Bounds.X;
                if (item.Bounds.Y > maxY) maxY = item.Bounds.Y;

                var bottomRightPoint = new PointF(item.Bounds.X + item.Bounds.Width, item.Bounds.Y + item.Bounds.Height);
                if (bottomRightPoint.X < minX) minX = bottomRightPoint.X;
                if (bottomRightPoint.Y < minY) minY = bottomRightPoint.Y;
                if (bottomRightPoint.X > maxX) maxX = bottomRightPoint.X;
                if (bottomRightPoint.Y > maxY) maxY = bottomRightPoint.Y;
            }
            minX -= Element.Step * 10;
            minY -= Element.Step * 10;
            maxX += Element.Step * 10;
            maxY += Element.Step * 10;

            int lengthY = (int)((maxY - minY) / Element.Step) + 1;
            int lengthX = (int)((maxX - minX) / Element.Step) + 1;
            if (lengthY < 0) lengthY = 0;
            if (lengthX < 0) lengthX = 0;
            // создание пустой сетки
            Cell[,] grid = new Cell[lengthY, lengthX];
            for (int y = 0; y < lengthY; y++)
                for (int x = 0; x < lengthX; x++)
                    grid[y, x] = new Cell { Point = new PointF(minX + x * Element.Step, minY + y * Element.Step) };
            // эти точки не должны заполняться тенью
            List<PointF> mustBeFree = [];
            foreach (var item in items)
            {
                if (item.Instance is ILink link)
                {
                    var n = 0;
                    foreach (var isLinked in link.LinkedInputs)
                    {
                        if (item.InputPins.TryGetValue(n, out PointF targetPinPoint))
                            mustBeFree.Add(targetPinPoint);
                        n++;
                    }
                    if (item.Instance is IFunction func)
                    {
                        n = 0;
                        foreach (var output in func.OutputValues)
                        {
                            if (item.OutputPins.TryGetValue(n, out PointF sourcePinPoint))
                                mustBeFree.Add(sourcePinPoint);
                            n++;
                        }
                    }
                }
            }
            // заполнение тенями элементов
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    if (items.Select(item => new RectangleF(
                        item.Bounds.X - Element.Step, item.Bounds.Y,
                        item.Bounds.Width + Element.Step * 3, item.Bounds.Height + Element.Step))
                        .Any(rect => rect.Contains(grid[y, x].Point)))
                    {
                        if (!mustBeFree.Contains(grid[y, x].Point))
                            grid[y, x].Kind = -1;
                    }
                }
            }
            return grid;
        }

        // рисовка связи
        private static void DrawLink(Graphics graphics, Color color, PointF sourcePinPoint, PointF targetPinPoint,
            RectangleF sourceBounds, RectangleF targetBounds)
        {
            //graphics.DrawLine(Pens.Yellow, sourcePinPoint, targetPinPoint);
            var rect = new RectangleF(
                Math.Min(sourcePinPoint.X, targetPinPoint.X),
                Math.Min(sourcePinPoint.Y, targetPinPoint.Y),
                Math.Abs(sourcePinPoint.X - targetPinPoint.X),
                Math.Abs(sourcePinPoint.Y - targetPinPoint.Y));

            //using var rectpen = new Pen(Color.Silver, 0);
            //rectpen.DashStyle = DashStyle.Dot;
            //graphics.DrawRectangles(rectpen, [rect]);

            using var linepen = new Pen(color, 1);
            if (sourcePinPoint.X < targetPinPoint.X)
            {
                var top = sourcePinPoint.Y < targetPinPoint.Y
                ? sourcePinPoint.X < targetPinPoint.X ? rect.Top : rect.Bottom
                : sourcePinPoint.X < targetPinPoint.X ? rect.Bottom : rect.Top;
                var bottom = sourcePinPoint.Y < targetPinPoint.Y
                    ? sourcePinPoint.X < targetPinPoint.X ? rect.Bottom : rect.Top
                    : sourcePinPoint.X < targetPinPoint.X ? rect.Top : rect.Bottom;
                var leftshift = SnapToGrid(new PointF(sourcePinPoint.X + (targetBounds.Left - (sourceBounds.Right + Element.Step * 2)) / 2, 0)).X;
                graphics.DrawLines(linepen, [
                    new PointF(rect.Left, top),
                    new PointF(leftshift, top),
                    new PointF(leftshift, bottom),
                    new PointF(rect.Right, bottom),
                ]);
            }
            else
            {
                var top = sourcePinPoint.Y > targetPinPoint.Y ? rect.Top : rect.Bottom;
                var bottom = sourcePinPoint.Y > targetPinPoint.Y ? rect.Bottom : rect.Top;
                var leftshift = rect.Right + Element.Step * 2;
                var middle = SnapToGrid(new PointF(0, sourceBounds.Top - targetBounds.Bottom > 0
                    ? targetBounds.Bottom + (sourceBounds.Top - targetBounds.Bottom) / 2
                    : sourceBounds.Bottom + (targetBounds.Top - sourceBounds.Bottom) / 2)).Y;
                var rightshift = rect.Left - Element.Step * 2;
                graphics.DrawLines(linepen, [
                    new PointF(rect.Right, bottom),
                    new PointF(leftshift, bottom),
                    new PointF(leftshift, middle),
                    new PointF(rightshift, middle),
                    new PointF(rightshift, top),
                    new PointF(rect.Left, top),
                ]);
            }
        }

        private Element? element;
        private int? pin;
        private bool? output;
        private PointF? linkFirstPoint;

        public Module Module { get; }

        public event EventHandler? ElementSelected;

        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
            linkFirstPoint = null;
            element = null;
            pin = null;
            output = null;
            if (TryGetModule(e.Location, out element) &&
                element != null && element.Instance != null ||
                TryGetFreeInputPin(e.Location, out element, out pin, out linkFirstPoint, out output) &&
                element != null && element.Instance != null && output == false ||
                TryGetPin(e.Location, out element, out pin, out linkFirstPoint, out output) &&
                element != null && element.Instance != null && output == true)
            {
                element.Selected = true;
                items.Where(item => item != element).ToList().ForEach(item => item.Selected = false);
                ElementSelected?.Invoke(element.Instance, EventArgs.Empty);
            }
            else
            {
                items.ForEach(item => item.Selected = false);
                ElementSelected?.Invoke(null, EventArgs.Empty);
            }
            if (e.Button == MouseButtons.Right)
            {
                linkFirstPoint = null;
                cmsContextMenu.Items.Clear();
                ToolStripMenuItem item; 
                if (element?.Instance is ILink func)
                {
                    if (output == false && pin != null && func.LinkedInputs[(int)pin])
                    {
                        item = new ToolStripMenuItem() { Text = "Удалить связь по входу", Tag = element };
                        item.Click += (s, e) =>
                        {
                            var menuItem = (ToolStripMenuItem?)s;
                            if (menuItem?.Tag is Element element && element.Instance is ILink fn)
                            {
                                fn.ResetValueLinkToInp((int)pin);
                                Module.Changed = true;
                                BuildLinks();
                            }
                        };
                        cmsContextMenu.Items.Add(item);
                    }
                    else if (output == true && items.Select(x => x.Instance as ILink)
                        .Any(x => x != null && x.InputLinkSources.Any(y => y.Item1 == element.Id)))
                    {
                        item = new ToolStripMenuItem() { Text = "Удалить связи по выходу", Tag = element };
                        item.Click += (s, e) =>
                        {
                            var menuItem = (ToolStripMenuItem?)s;
                            if (menuItem?.Tag is Element element && element.Instance is IFunction fn)
                            {
                                foreach (var instance in items.Select(x => x.Instance as ILink)
                                        .Where(instance => instance != null && instance.InputLinkSources.Any(y => y.Item1 == element.Id)))
                                {
                                    if (instance == null) continue;
                                    var n = 0;
                                    foreach (var source in instance.InputLinkSources)
                                    {
                                        if (source.Item1 == element.Id)
                                        {
                                            instance.ResetValueLinkToInp(n);
                                        }
                                        n++;
                                    }
                                }
                                Module.Changed = true;
                                BuildLinks();
                            }
                        };
                        cmsContextMenu.Items.Add(item);
                    }
                    else if (output == null)
                    {
                        item = new ToolStripMenuItem() { Text = "Удалить элемент", Tag = element };
                        item.Click += (s, e) =>
                        {
                            var menuItem = (ToolStripMenuItem?)s;
                            if (menuItem?.Tag is Element element)
                            {
                                DeleteOneElement(element);
                                zoomPad.Invalidate();
                            }
                        };
                        cmsContextMenu.Items.Add(item);
                    }
                }
            }
            zoomPad.Invalidate();
        }

        private void DeleteOneElement(Element element)
        {
            foreach (var item in items)
            {
                if (item.Instance is ILink func)
                {
                    var n = 0;
                    foreach (var islinked in func.LinkedInputs)
                    {
                        if (islinked)
                        {
                            var linkSources = func.InputLinkSources;
                            (Guid id, int index) = linkSources[n];
                            if (items.FirstOrDefault(x => x.Id == id) == element)
                                func.ResetValueLinkToInp(n);
                        }
                        n++;
                    }
                }
            }
            items.Remove(element);
            Module.Changed = true;
            BuildLinks();
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            if (TryGetModule(e.Location, out _))
                Cursor = Cursors.SizeAll;
            else if (TryGetFreeInputPin(e.Location, out _, out _, out _, out _))
                Cursor = Cursors.Hand;
            else
                Cursor = Cursors.Default;
            if (e.Button == MouseButtons.Left)
            {
                var mp = PrepareMousePosition(mousePosition);
                var pt = PrepareMousePosition(e.Location);
                var delta = new SizeF(pt.X - mp.X, pt.Y - mp.Y);
                mousePosition = e.Location;
                if (!delta.IsEmpty)
                {
                    if (element != null)
                    {
                        if (pin == null)
                        {
                            element.Location = PointF.Add(element.Location, delta);
                            Module.Changed = true;
                        }
                        else
                        {

                        }
                    }
                    zoomPad.Invalidate();
                }
            }
        }

        private void zoomPad_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (element != null)
                {
                    element.Location = SnapToGrid(element.Location);
                }
                var elementFirst = element;
                var pinFirst = pin;
                var outputFirst = output;
                var linkFirst = linkFirstPoint;
                if (TryGetPin(e.Location, out Element? elementSecond, out int? pinSecond, out PointF? linkSecondPoint, out bool? outputSecond) &&
                    elementSecond?.Instance is ILink target && elementFirst?.Instance is ILink source)
                {
                    if (elementFirst != elementSecond && outputFirst != outputSecond)
                    {
                        // создание связей между элементами
                        if (pinSecond != null && pinFirst != null && outputFirst == true && outputSecond == false)
                        {
                            target.SetValueLinkToInp((int)pinSecond, source.GetResultLink((int)pinFirst), elementFirst.Id, (int)pinFirst);
                            Module.Changed = true;
                        }
                        else if (pinSecond != null && pinFirst != null && outputFirst == false && outputSecond == true)
                        {
                            source.SetValueLinkToInp((int)pinFirst, target.GetResultLink((int)pinSecond), elementSecond.Id, (int)pinSecond);
                            Module.Changed = true;
                        }
                    }
                    //else if (elementFirst == elementSecond && outputFirst == outputSecond && outputSecond == false && pinSecond is int ipin)
                    //{
                    //    // отпускание после нажатия на этом же входе
                    //    var value = target.InputValues[ipin];
                    //    if (value is bool bvalue)
                    //    {
                    //        target.SetValueToInp(ipin, !bvalue);
                    //        Module.Changed = true;
                    //    }
                    //}
                }
                else
                    element = null;
                pin = null;
                output = null;
                linkFirstPoint = null;
                BuildLinks();
                zoomPad.Invalidate();
            }
        }

        public void UpdateView()
        {
            zoomPad.Invalidate();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            Project.Save();
        }

        private void tsmiAddModule_Click(object sender, EventArgs e)
        {
            panelForm.AddModuleToProject();
        }

        private void tsbDeleteModule_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Этот модуль будет удалён безвозвратно! Удалить?",
                "Удаление текущего модуля", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                panelForm.RemoveModuleFromProject(Module);
            }
        }

        private void ModuleForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) 
            { 
                case Keys.Delete:
                    if (items.Any(x => x.Selected) &&
                        MessageBox.Show("Удалить выделенные элементы?", "Удаление элементов", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DeleteAllSelectedElements();
                        zoomPad.Invalidate();
                    }
                    break;
            }
        }

        private void DeleteAllSelectedElements()
        {
            foreach (var item in items)
            {
                if (item.Instance is ILink func)
                {
                    var n = 0;
                    foreach (var islinked in func.LinkedInputs)
                    {
                        if (islinked)
                        {
                            var linkSources = func.InputLinkSources;
                            (Guid id, int index) = linkSources[n];
                            var source = items.FirstOrDefault(x => x.Id == id);
                            if (source != null && source.Selected)
                                func.ResetValueLinkToInp(n);
                        }
                        n++;
                    }
                }
            }
            items.RemoveAll(x => x.Selected);
            Module.Changed = true;
            BuildLinks();
        }
    }

    public enum LinkVector
    {
        None,
        Horizontal, 
        Vertical,
    }
}
