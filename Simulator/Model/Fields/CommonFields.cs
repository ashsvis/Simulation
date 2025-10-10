using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Fields
{
    public class CommonFields : FilterablePropertyBase, ILoadSave, IDraw, ILinkSupport
    {
        private readonly Input[] inputs;
        private readonly Output[] outputs;

        [Browsable(false)]
        public Input[] Inputs => inputs;
        [Browsable(false)]
        public Output[] Outputs => outputs;

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
            var inputCount = 1;
            inputs = new Input[inputCount];
            for (var i = 0; i < inputs.Length; i++)
            {
                inputs[i] = new DigitalInput(itemId, i);
            }
            var outputCount = 1;
            outputs = new Output[outputCount];
            for (var i = 0; i < outputs.Length; i++)
            {
                outputs[i] = new DigitalOutput(itemId, i);
            }
        }

        public void UpdateInputLinkSources((Guid, int, bool) seek, Guid newId)
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                var ls = Inputs[i].LinkSource;
                if (ls != null &&  ls.Id == seek.Item1 && ls.PinIndex == seek.Item2)
                {
                    inputs[i].LinkSource = new LinkSource(newId, seek.Item2, seek.Item3);
                }
            }
        }

        [Category(" Общие"), DisplayName("Имя тега")]
        public string? Name { get; set; }

        public virtual void SetValueLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            if (inputIndex < 0 || inputIndex >= inputs.Length) return;
            if (sourceId == Guid.Empty)
                inputs[inputIndex].LinkSource = null;
            else
                inputs[inputIndex].LinkSource = new LinkSource(sourceId, outputPinIndex, byDialog);
        }

        public virtual void ResetValueLinkToInp(int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= inputs.Length) return;
            inputs[inputIndex].LinkSource = null;
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
            customDraw?.Invoke(graphics, rect, pen, brush, font, fontbrush, 0, selected);
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
            if (inputs.Length > 0)
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
            if (outputs.Length > 0)
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
