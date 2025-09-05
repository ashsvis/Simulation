using Simulator.Model;
using Simulator.View;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection;
using Module = Simulator.Model.Module;

namespace Simulator
{
    public partial class ChildForm : Form
    {
        private readonly MainForm mainForm;

        //private readonly Module module = new();
        private readonly List<Element> items = [];

        private Point firstMouseDown;
        private Point mousePosition;

        public ChildForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            mainForm.SimulationTick += MainForm_SimulationTick;

            //module.Items.Clear();
            //var not = new Model.Logic.NOT();
            //module.Items.Add(not);
            //var and = new Model.Logic.AND();
            //module.Items.Add(and);
            //var or = new Model.Logic.OR();
            //module.Items.Add(or);
            //var rs = new Model.Trigger.RS();
            //module.Items.Add(rs);

            //module.Items.ForEach(item => item.ResultChanged += Item_ResultChanged);

            //not.SetValueLinkToInp(rs.GetResultLink());
            //or.SetValueLinkToInp2(not.GetResultLink());
        }

        private void Item_ResultChanged(object sender, Model.ResultCalculateEventArgs args)
        {
            ElementSelected?.Invoke(sender, EventArgs.Empty);

            //Debug.WriteLine($"{Name}.{sender.GetType().Name}.{args.Propname} is {args.Result}");
        }

        private void ChildForm_Load(object sender, EventArgs e)
        {

        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            items.ForEach(item =>
            {
                if (item.Instance is ICalculate instance)
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
                    if (item.Instance is ICalculate instance)
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
                    item.Instance = Activator.CreateInstance(item.Type);
                    item.Location = PrepareMousePosition(zoomPad.PointToClient(new Point(e.X, e.Y)));
                    if (item.Instance is ICalculate instance)
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
                var rect = new RectangleF(item.Location, new SizeF(50f, 80f));
                if (rect.Contains(point))
                {
                    target = items[i];
                    return true;
                }
            }
            target = null;
            return false;
        }

        private void zoomPad_OnDraw(object sender, Simulator.View.ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            foreach (var item in items)
            {
                var rect = new RectangleF(item.Location, new SizeF(50f, 80f));
                graphics.FillRectangle(Brushes.White, rect);
                using var pen = new Pen(Color.Black, 0);
                graphics.DrawRectangles(pen, [rect]);
            }
        }

        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                Element? target;
                if (TryGetModule(e.Location, out target) &&
                    target != null && target.Instance != null)
                {
                    ElementSelected?.Invoke(target.Instance, EventArgs.Empty);
                }
                else
                    ElementSelected?.Invoke(null, EventArgs.Empty);
            }
        }

        public event EventHandler ElementSelected;

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void zoomPad_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
