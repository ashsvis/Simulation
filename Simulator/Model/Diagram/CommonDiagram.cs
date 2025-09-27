using Simulator.Model.Interfaces;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Diagram
{
    public class CommonDiagram : FilterablePropertyBase, ILinkSupport, ILoadSave, IDraw
    {
        private readonly object[] getInputs;
        private readonly object[] getOutputs;
        private readonly (Guid, int, bool)[] getLinkSources;

        private Guid itemId;

        public void SetItemId(Guid id)
        {
            itemId = id;
        }

        private IVariable? varManager;
        public void SetVarManager(IVariable varManager)
        {
            this.varManager = varManager;
        }

        [Category(" Общие"), DisplayName("Идентификатор")]
        public Guid ItemId => itemId;

        public IVariable VarManager => varManager ??= new ProjectProxy();

        public CommonDiagram() : this(DiagramFunction.None)
        {
        }

        public CommonDiagram(DiagramFunction func)
        {
            getInputs = [];
            getOutputs = [];
            getLinkSources = [];
            Out = new object();
            var inputCount = 1;
            var outputCount = 1;
            if (func == DiagramFunction.Finish)
                outputCount = 0;
            if (func == DiagramFunction.Start)
                inputCount = 0;
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

        public object[] InputValues => [];

        public object[] OutputValues => [];

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

        public void Load(XElement? xtance)
        {
            var xinputs = xtance?.Element("Inputs");
            if (xinputs != null)
            {
                foreach (XElement item in xinputs.Elements("Input"))
                {
                    if (int.TryParse(item.Attribute("Index")?.Value, out int index))
                    {
                        var xsource = item.Element("Source");
                        if (xsource != null)
                        {
                            if (Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid guid) && guid != Guid.Empty)
                            {
                                var external = false;
                                if (bool.TryParse(xsource.Attribute("External")?.Value, out bool bval))
                                    external = bval;
                                if (int.TryParse(xsource.Attribute("PinIndex")?.Value, out int outputIndex))
                                    getLinkSources[index] = (guid, outputIndex, external);
                                else
                                    getLinkSources[index] = (guid, 0, external);
                            }
                        }
                    }
                }
            }
            var xoutputs = xtance?.Element("Outputs");
            if (xoutputs != null)
            {
                foreach (XElement item in xoutputs.Elements("Output"))
                {
                    if (int.TryParse(item.Attribute("Index")?.Value, out int index))
                    {

                    }
                }
            }
        }

        public void Save(XElement xtem)
        {
            var xtance = new XElement("Instance");

            XElement xinputs = new("Inputs");
            bool customInputs = false;
            for (var i = 0; i < getInputs.Length; i++)
            {
                (Guid id, int output, bool external) = getLinkSources[i];
                customInputs = true;
                XElement xinput = new("Input");
                xinputs.Add(xinput);
                xinput.Add(new XAttribute("Index", i));

                if (id != Guid.Empty)
                {
                    xinput.Add(new XElement("SourceId", id));
                    if (output > 0)
                        xinput.Add(new XElement("OutputIndex", output));
                }
            }
            if (customInputs)
                xtance.Add(xinputs);

            XElement xoutputs = new("Outputs");
            bool customOutputs = false;
            for (var i = 0; i < 1; i++)
            {
                customOutputs = true;
                XElement xoutput = new("Output");
                xoutputs.Add(xoutput);
                xoutput.Add(new XAttribute("Index", i));
            }
            if (customOutputs)
                xtance.Add(xoutputs);

            if (customInputs || customOutputs)
                xtem.Add(xtance);
        }

        public void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size, int index, bool selected, CustomDraw? customDraw = null)
        {
            var step = Element.Step;
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
            var y = -step + location.Y;
            var x = location.X + size.Width / 2f;
            if (getInputs.Length > 0)
            {
                // вход
                // вертикальная риска сверху, напротив входа
                graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y + step));
            }
            y = location.Y + size.Height;
            if (getOutputs.Length > 0)
            {
                // выход
                // вертикальная риска снизу, напротив выхода
                graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y + step));
            }
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
