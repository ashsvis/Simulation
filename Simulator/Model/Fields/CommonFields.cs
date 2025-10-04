using Simulator.Model.Interfaces;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Fields
{
    public class CommonFields : FilterablePropertyBase, ILinkSupport, ILoadSave, IDraw
    {
        private readonly object[] getInputs;
        private readonly object[] getOutputs;
        private readonly (Guid, int, bool)[] getLinkSources;

        private Guid itemId;

        public void SetItemId(Guid id)
        {
            itemId = id;
        }

        [Browsable(false)]
        public Guid ItemId => itemId;

        public CommonFields() : this(FieldFunction.None)
        {
        }

        public CommonFields(FieldFunction func)
        {
            getInputs = [];
            getOutputs = [];
            getLinkSources = [];
            Out = new object();
            var inputCount = 1;
            var outputCount = 1;
            getInputs = new object[inputCount];
            getOutputs = new object[outputCount];
            getLinkSources = new (Guid, int, bool)[inputCount];
        }

        [Browsable(false)]
        public bool[] LinkedInputs
        {
            get
            {
                List<bool> list = [];
                for (var i = 0; i < getInputs.Length; i++)
                {
                    (Guid id, int _, bool _) = getLinkSources[i];
                    list.Add(id != Guid.Empty);
                }
                return [.. list];
            }
        }

        [Browsable(false)]
        public object[] LinkedOutputs => getOutputs;

        [Browsable(false)]
        public (Guid, int, bool)[] InputLinkSources => getLinkSources;

        public void UpdateInputLinkSources((Guid, int, bool) seek, Guid newId)
        {
            for (var i = 0; i < getLinkSources.Length; i++)
            {
                (Guid id, int input, bool external) = getLinkSources[i];
                if (id == seek.Item1 && input == seek.Item2)
                {
                    getLinkSources[i].Item1 = newId;
                }
            }
        }

        [Browsable(false)]
        public object Out { get; set; }

        [Browsable(false)]
        public object[] InputValues => [];

        [Browsable(false)]
        public object[] OutputValues => [];

        [Category(" Общие"), DisplayName("Имя тега")]
        public string? Name { get; set; }

        public void ResetValueLinkToInp(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < getLinkSources.Length)
                getLinkSources[inputIndex] = (Guid.Empty, 0, false);
        }

        public void SetValueLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            if (inputIndex >= 0 && inputIndex < getLinkSources.Length)
                getLinkSources[inputIndex] = (sourceId, outputPinIndex, byDialog);
        }

        public virtual void Load(XElement? xtance)
        {
            var instname = xtance?.Attribute("Name")?.Value;
            if (instname != null)
                Name = instname;
        }

        public virtual void Save(XElement xtem)
        {
            // stub
        }

        public void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size, int index, bool selected, CustomDraw? customDraw = null)
        {
            using var brush = new SolidBrush(backColor);
            using var pen = new Pen(selected ? Color.Magenta : foreColor, 1f);
            using var font = new Font("Consolas", Element.Step + 2f);
            using var fontbrush = new SolidBrush(foreColor);
            var rect = new RectangleF(location, size);
            if (customDraw == null)
            {
                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangles(pen, [rect]);
            }
            customDraw?.Invoke(graphics, rect, pen, brush, font, fontbrush, 0);
            
            //var step = Element.Step;
            //var y = -step + location.Y;
            //var x = location.X + size.Width / 2f;
            //if (getInputs.Length > 0)
            //{
            //    // вход
            //    // вертикальная риска сверху, напротив входа
            //    graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y + step));
            //}
            //y = location.Y + size.Height;
            //if (getOutputs.Length > 0)
            //{
            //    // выход
            //    // вертикальная риска снизу, напротив выхода
            //    graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y + step));
            //}
        }

        public virtual void CalculateTargets(PointF location, ref SizeF size,
             Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins,
             Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var max = 1;
            var height = step + max * step * 4 + step;
            var width = step + 1 * step * 4 + step;
            size = new SizeF(width, height);
            // вход
            var y = location.Y;
            var x = location.X + width / 2f;
            var n = 0;
            itargets.Clear();
            ipins.Clear();
            if (getInputs.Length > 0)
            {
                // значение входа
                var ms = new SizeF(step * 2, step * 2);
                itargets.Add(n, new RectangleF(new PointF(x - ms.Width / 2, y - ms.Height), ms));
                ipins.Add(n, new PointF(x, y - step));
            }
            // выход
            y = location.Y + height;
            n = 0;
            otargets.Clear();
            opins.Clear();
            if (getOutputs.Length > 0)
            {
                // значение выхода
                var ms = new SizeF(step * 2, step * 2);
                otargets.Add(n, new RectangleF(new PointF(x - ms.Width / 2, y), ms));
                var pt = new PointF(x, y + step);
                opins.Add(n, pt);
            }
        }
    }
}
