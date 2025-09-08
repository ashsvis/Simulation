using Simulator.Model;
using Simulator.View;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Simulator
{
    public partial class ChildForm : Form, IUpdateView
    {
        private readonly MainForm mainForm;

        private readonly List<Element> items = [];

        private Point firstMouseDown;
        private Point mousePosition;

        public ChildForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            mainForm.SimulationTick += MainForm_SimulationTick;
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
            mainForm.SimulationTick -= MainForm_SimulationTick;
        }

        private void MainForm_SimulationTick(object? sender, EventArgs e)
        {
            items.ForEach(item =>
            {
                try
                {
                    if (item.Instance is IFunction instance)
                        instance.Calculate();
                }
                catch
                {

                }
            });
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

        private void zoomPad_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Effect == DragDropEffects.Copy)
            {
                if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
                {
                    item.Instance = (IFunction?)Activator.CreateInstance(item.Type);
                    item.Location = PrepareMousePosition(zoomPad.PointToClient(new Point(e.X, e.Y)));
                    if (item.Instance is IFunction instance)
                        instance.ResultChanged += Item_ResultChanged;
                    items.Add(item);
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
                    item.Instance is IFunction function && pin is int ipin && !function.LinkedInputs[ipin])
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

        private void zoomPad_OnDraw(object sender, Simulator.View.ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            //graphics.SmoothingMode = SmoothingMode.HighQuality;
            //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var brush = new SolidBrush(zoomPad.BackColor);
            using var pen = new Pen(zoomPad.ForeColor, 0);
            // прорисовка связей
            foreach (var item in items)
            {
                if (item.Instance is IFunction function && function.LinkedInputs.Any(x => x == true))
                {
                    var n = 0;
                    foreach(var input in function.LinkedInputs)
                    {
                        if (input)
                        {
                            if (item.InputPins.TryGetValue(n, out PointF targetPinPoint))
                            {
                                var sourcePinPoint = item.OutputPoints[0];
                                graphics.DrawLine(Pens.Yellow, sourcePinPoint, targetPinPoint);
                            }
                        }
                        n++;
                    }
                }
            }
            // прорисовка элементов
            foreach (var item in items)
            {
                if (item.Instance is ICustomDraw inst)
                    item.Draw(graphics, zoomPad.ForeColor, zoomPad.BackColor, inst.CustomDraw);
                else
                    item.Draw(graphics, zoomPad.ForeColor, zoomPad.BackColor);
            }
            if (linkFirstPoint != null)
            {
                var mp = PrepareMousePosition(mousePosition);
                graphics.DrawLine(Pens.Red, (PointF)linkFirstPoint, mp);
            }
        }

        private Element? element;
        private int? pin;
        private bool? output;
        private PointF? linkFirstPoint;

        public event EventHandler? ElementSelected;

        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                linkFirstPoint = null;
                if (TryGetModule(e.Location, out element) &&
                    element != null && element.Instance != null)
                {
                    ElementSelected?.Invoke(element.Instance, EventArgs.Empty);
                }
                else if (TryGetFreeInputPin(e.Location, out element, out pin, out PointF? point, out output) &&
                    element != null && element.Instance != null)
                {
                    linkFirstPoint = point;
                    ElementSelected?.Invoke(element.Instance, EventArgs.Empty);
                }
                else
                    ElementSelected?.Invoke(null, EventArgs.Empty);
            }
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
                            element.Location = PointF.Add(element.Location, delta);
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
                var elementFirst = element;
                var pinFirst = pin;
                var outputFirst = output;
                var linkFirst = linkFirstPoint;
                if (TryGetPin(e.Location, out Element? elementSecond, out int? pinSecond, out PointF? linkSecondPoint, out bool? outputSecond) &&
                    elementSecond?.Instance is IFunction target && elementFirst?.Instance is IFunction source)
                {
                    if (elementFirst != elementSecond && outputFirst != outputSecond)
                    {
                        // создание связей между элементами
                        if (pinSecond != null && pinFirst != null && outputFirst == true && outputSecond == false)
                        {
                            target.SetValueLinkToInp((int)pinSecond, source.GetResultLink((int)pinFirst));
                            elementSecond.SetValueLinkPointToInp((int)pinSecond, elementFirst.GetResultLinkPoint((int)pinFirst));
                        }
                        else if (pinSecond != null && pinFirst != null && outputFirst == false && outputSecond == true)
                        {
                            source.SetValueLinkToInp((int)pinFirst, target.GetResultLink((int)pinSecond));
                            elementFirst.SetValueLinkPointToInp((int)pinFirst, elementSecond.GetResultLinkPoint((int)pinSecond));
                        }
                    }
                    else if (elementFirst == elementSecond && outputFirst == outputSecond && outputSecond == false && pinSecond is int ipin)
                    {
                        // отпускание после нажатия на этом же входе
                        var value = target.InputValues[ipin];
                        if (value is bool bvalue)
                            target.SetValueToInp(ipin, !bvalue);
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
    }
}
