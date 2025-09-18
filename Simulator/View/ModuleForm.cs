using Simulator.Model;
using Simulator.View;
using System.Drawing.Drawing2D;

namespace Simulator
{
    public partial class ModuleForm : Form, IUpdateView
    {
        private readonly PanelForm panelForm;

        private readonly List<Element> items;
        private readonly List<Link> links;
        private Cell[,] grid = new Cell[0, 0];

        private Point firstMouseDown;
        private Point mousePosition;

        public ModuleForm(PanelForm panelForm, Module module)
        {
            InitializeComponent();
            this.panelForm = panelForm;
            Module = module;
            items = module.Items;
            links = module.Links;
            panelForm.SimulationTick += Module_SimulationTick;
        }

        private void Item_ResultChanged(object sender, ResultCalculateEventArgs args)
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
            items.ForEach(item =>
            {
                try
                {
                    if (item.Instance is Model.Logic.FE frontEdgeDetector)
                        frontEdgeDetector.Reset();
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

        public static PointF SnapToGrid(PointF pointF, int koeff = 1)
        {
            float gridStep = Element.Step * koeff;
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
                    item.Instance = Activator.CreateInstance(item.Type);
                    item.Location = SnapToGrid(PrepareMousePosition(zoomPad.PointToClient(new Point(e.X, e.Y))));
                    if (item.Instance is IFunction instance)
                        instance.ResultChanged += Item_ResultChanged;
                    if (item.Instance is ILinkSupport link)
                        link.SetItemId(item.Id);
                    items.ForEach(item => item.Selected = false);
                    links.ForEach(item => item.Select(false));
                    item.Selected = true;
                    items.Add(item);
                    Module.Changed = true;
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
                    item.Instance is ILinkSupport link && pin is int ipin && !link.LinkedInputs[ipin])
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
                foreach(var segment in link.Segments)
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

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            //graphics.SmoothingMode = SmoothingMode.HighQuality;
            //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
#if DEBUG

            // прорисовка узлов сетки
            //using var brush = new SolidBrush(Color.Gray);
            //using var font = new Font("Consolas", 3f);
            //for (var y = 0; y < grid.GetLength(0); y++)
            //{
            //    for (var x = 0; x < grid.GetLength(1); x++)
            //    {
            //        if (grid[y, x].Kind < -1)
            //        {
            //            graphics.FillEllipse(Brushes.Aqua, new RectangleF(
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
#endif

            // прорисовка связей
            using var linkpen = new Pen(zoomPad.ForeColor);
            foreach (var link in links.Where(x => !x.Selected))
            {
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
                                    rects.Add((new RectangleF(Math.Min(pt1.X, pt2.X), pt1.Y - 1f , Math.Abs(pt1.X - pt2.X), 3f), link.Id));
                                if (pt1.X == pt2.X || pt1.Y != pt2.Y)
                                    rects.Add((new RectangleF(pt1.X - 1f, Math.Min(pt1.Y, pt2.Y), 3f, Math.Abs(pt1.Y - pt2.Y)), link.Id));
                            }
                        }
                    }

                    List<(PointF, Guid)> results = [];
                    foreach ((var point, var id1) in list)
                    {
                        foreach((var rect, var id2) in rects)
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
                using var pen = new Pen(Color.Silver, 0);
                pen.DashStyle = DashStyle.Dash;
                graphics.DrawLine(pen, (PointF)linkFirstPoint, mp);
            }
        }

 
        /*
            
        private void BuildLinks()
        {
            // подготовка сетки с тенями от существующих элементов и связей
            grid = BuildGrid();
            // составление списка связей для построения
            //List<Link> links = [];
            links.Clear();
            foreach (var item in items)
            {
                if (item.Instance is ILinkSupport function && function.LinkedInputs.Any(x => x == true))
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
                                    links.Add(new Link(Guid.NewGuid(), sourcePinPoint, targetPinPoint));
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
                    link.BeginUpdate();
                    try
                    {
                        link.AddPoint(grid[y, x].Point);
                        while (wave > 1)
                        {
                            if (x > 0 && grid[y, x - 1].Kind == wave - 1)
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
                            else if (y > 0 && grid[y - 1, x].Kind == wave - 1)
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
                            wave--;
                        }
                        link.AddPoint(grid[y, x].Point);
                    }
                    finally
                    {
                        link.EndUpdate();
                    }
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

        */

        private Link? BuildLink(Guid sourceId, int sourcePin, PointF sourcePinPoint, Guid destinationId, int destinationPin, PointF destinationPinPoint)
        {
            // подготовка сетки с тенями от существующих элементов и связей
            grid = BuildGrid();
            var link = new Link(Guid.NewGuid(), sourceId, sourcePin, destinationId, destinationPin, sourcePinPoint, destinationPinPoint);
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
            if (tx < 0 || ty < 0) return null;

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
                link.BeginUpdate();
                try
                {
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
                finally
                {
                    link.EndUpdate();
                }
            }
            // очистка от предыдущей волны
            ClearGridFromWaves();
            return link;
        }

        private void ClearGridFromWaves()
        {
            for (var iy = 0; iy < grid.GetLength(0); iy++)
            {
                for (var ix = 0; ix < grid.GetLength(1); ix++)
                {
                    if (grid[iy, ix].Kind > 0 || grid[iy, ix].Kind == -2)
                        grid[iy, ix].Kind = 0;
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
                if (item.Instance is ILinkSupport link)
                {
                    var n = 0;
                    foreach (var isLinked in link.LinkedInputs)
                    {
                        if (item.InputPins.TryGetValue(n, out PointF targetPinPoint))
                            mustBeFree.Add(targetPinPoint);
                        n++;
                    }
                    n = 0;
                    foreach (var output in link.LinkedOutputs)
                    {
                        if (item.OutputPins.TryGetValue(n, out PointF sourcePinPoint))
                            mustBeFree.Add(sourcePinPoint);
                        n++;
                    }
                }
            }
            // заполнение тенями элементов
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    if (items.Select(item => new RectangleF(
                        item.Bounds.X, item.Bounds.Y, item.Bounds.Width + Element.Step, item.Bounds.Height + Element.Step))
                        .Any(rect => rect.Contains(grid[y, x].Point)))
                    {
                        if (!mustBeFree.Contains(grid[y, x].Point))
                            grid[y, x].Kind = -1;
                    }
                }
            }
            return grid;
        }

        private Element? element;
        private int? pin;
        private bool? output;
        private PointF? linkFirstPoint;

        private Link? link;
        private int? segmentIndex;
        private bool? segmentVertical;

        public Module Module { get; }

        public event EventHandler? ElementSelected;

        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
            linkFirstPoint = null;
            element = null;
            pin = null;
            output = null;
            if (TryGetLinkSegment(e.Location, out link, out segmentIndex, out segmentVertical) &&
                link != null && segmentIndex != null && segmentVertical != null)
            {
                ((Link)link).Select(true);
                links.Where(item => item.Id != ((Link)link).Id).ToList().ForEach(item => item.Select(false));
                items.ForEach(item => item.Selected = false);
                ElementSelected?.Invoke(link, EventArgs.Empty);
            }
            else if (TryGetModule(e.Location, out element) &&
                element != null && element.Instance != null ||
                TryGetPin(e.Location, out element, out pin, out linkFirstPoint, out output) &&
                element != null && element.Instance != null && output == true)
            {
                element.Selected = true;
                items.Where(item => item != element).ToList().ForEach(item => item.Selected = false);
                links.ForEach(item => item.Select(false));
                // выделение выходных связей элемента
                foreach (var item in items.Where(x => x.Selected))
                {
                    foreach (var link in links.Where(x => x.SourceId == item.Id))
                        link.Select(true);
                }
                ElementSelected?.Invoke(element.Instance, EventArgs.Empty);
            }
            else
            {
                if (!(TryGetFreeInputPin(e.Location, out Element? element, out _, out _, out _) && element?.Instance is IManualChange _))
                    linkFirstPoint = null;
                items.ForEach(item => item.Selected = false);
                links.ForEach(item => item.Select(false));
                ElementSelected?.Invoke(null, EventArgs.Empty);
            }
            if (e.Button == MouseButtons.Right)
            {
                linkFirstPoint = null;
                cmsContextMenu.Items.Clear();
                ToolStripMenuItem item; 
                if (element?.Instance is ILinkSupport func)
                {
                    if (output == false && pin != null && func.LinkedInputs[(int)pin])
                    {
                        item = new ToolStripMenuItem() { Text = "Удалить связь по входу", Tag = element };
                        item.Click += (s, e) =>
                        {
                            var menuItem = (ToolStripMenuItem?)s;
                            if (menuItem?.Tag is Element element && element.Instance is ILinkSupport fn)
                            {
                                fn.ResetValueLinkToInp((int)pin);
                                links.RemoveAll(link => link.DestinationId == element.Id && link.DestinationPinIndex == (int)pin);
                                Module.Changed = true;
                            }
                        };
                        cmsContextMenu.Items.Add(item);
                    }
                    else if (output == true && items.Select(x => x.Instance as ILinkSupport)
                        .Any(x => x != null && x.InputLinkSources.Any(y => y.Item1 == element.Id)))
                    {
                        item = new ToolStripMenuItem() { Text = "Удалить связи по выходу", Tag = element };
                        item.Click += (s, e) =>
                        {
                            var menuItem = (ToolStripMenuItem?)s;
                            if (menuItem?.Tag is Element element && element.Instance is IFunction fn)
                            {
                                foreach (var instance in items.Select(x => x.Instance as ILinkSupport)
                                        .Where(instance => instance != null && instance.InputLinkSources.Any(y => y.Item1 == element.Id)))
                                {
                                    if (instance == null) continue;
                                    var n = 0;
                                    foreach (var source in instance.InputLinkSources)
                                    {
                                        if (source.Item1 == element.Id)
                                        {
                                            instance.ResetValueLinkToInp(n);
                                            links.RemoveAll(link => link.SourceId == element.Id && link.SourcePinIndex == n);
                                        }
                                        n++;
                                    }
                                }
                                Module.Changed = true;
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
                if (item.Instance is ILinkSupport func)
                {
                    var n = 0;
                    foreach (var islinked in func.LinkedInputs)
                    {
                        if (islinked)
                        {
                            var linkSources = func.InputLinkSources;
                            (Guid id, int index) = linkSources[n];
                            if (items.FirstOrDefault(x => x.Id == id) == element)
                            {
                                func.ResetValueLinkToInp(n);
                            }
                        }
                        n++;
                    }
                }
            }
            links.RemoveAll(link => link.SourceId == element.Id || link.DestinationId == element.Id);
            items.Remove(element);
            Module.Changed = true;
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            if (TryGetLinkSegment(e.Location, out _, out _, out bool? vertical))
                Cursor = vertical == true ? Cursors.VSplit : Cursors.HSplit;
            else if (TryGetModule(e.Location, out _))
                Cursor = Cursors.SizeAll;
            else if (TryGetFreeInputPin(e.Location, out Element? element, out _, out _, out _) && element?.Instance is IManualChange _)
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
                    }
                    else if (link != null && segmentIndex != null && segmentVertical != null)
                    {
                        link?.OffsetSegment((int)segmentIndex, (bool)segmentVertical, delta);
                        Module.Changed = true;
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
                    foreach (var link in links.Where(x => x.SourceId == element.Id))
                    {
                        link.UpdateSourcePoint(element.OutputPins[link.SourcePinIndex]);
                    }
                    foreach (var link in links.Where(x => x.DestinationId == element.Id))
                    {
                        link.UpdateDestinationPoint(element.InputPins[link.DestinationPinIndex]);
                    }
                }
                else if (link != null)
                {
                    ((Link)link).SnapPointsToGrid(SnapToGrid);
                }
                var elementFirst = element;
                var pinFirst = pin;
                var outputFirst = output;
                var linkFirst = linkFirstPoint;
                if (TryGetFreeInputPin(e.Location, out element, out pin, out linkFirstPoint, out output) &&
                    element != null && pin != null && element.Instance is IManualChange tar && output == false)
                {
                    var value = tar.GetValueFromInp((int)pin);
                    if (value is bool bval)
                        tar.SetValueToInp((int)pin, !bval);
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
                            if (!target.LinkedInputs[(int)pinSecond])
                            {
                                target.SetValueLinkToInp((int)pinSecond, source.GetResultLink((int)pinFirst), elementFirst.Id, (int)pinFirst);
                                var link = BuildLink(elementFirst.Id, (int)pinFirst, (PointF)linkFirstPoint, elementSecond.Id, (int)pinSecond, (PointF)linkSecondPoint);
                                if (link != null)
                                {
                                    elementFirst.Selected = false;
                                    ((Link)link).Select(true);
                                    links.Add((Link)link);
                                    ((Link)link).UpdateSourcePoint(elementFirst.OutputPins[((Link)link).SourcePinIndex]);
                                    Module.Changed = true;
                                }
                            }
                        }
                        else if (pinSecond != null && pinFirst != null && outputFirst == false && outputSecond == true)
                        {
                            // от входа к выходу
                            if (!source.LinkedInputs[(int)pinFirst])
                            {
                                source.SetValueLinkToInp((int)pinFirst, target.GetResultLink((int)pinSecond), elementSecond.Id, (int)pinSecond);
                                var link = BuildLink(elementSecond.Id, (int)pinSecond, (PointF)linkSecondPoint, elementFirst.Id, (int)pinFirst, (PointF)linkFirstPoint);
                                if (link != null)
                                {
                                    elementFirst.Selected = false;
                                    ((Link)link).Select(true);
                                    links.Add((Link)link);
                                    ((Link)link).UpdateDestinationPoint(elementFirst.InputPins[((Link)link).DestinationPinIndex]);
                                    Module.Changed = true;
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
                        zoomPad.Invalidate();
                    }
                    break;
            }
        }

        private void DeleteAllSelectedElements()
        {
            foreach (var item in items)
            {
                if (item.Selected && item.Instance is ILinkSupport func)
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
                            {
                                func.ResetValueLinkToInp(n);
                            }
                        }
                        n++;
                    }
                    links.RemoveAll(link => link.SourceId == item.Id || link.DestinationId == item.Id);
                }
            }
            items.RemoveAll(x => x.Selected);
            Module.Changed = true;
        }
    }
}
