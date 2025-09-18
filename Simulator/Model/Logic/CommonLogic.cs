using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Logic
{
    public class CommonLogic : FilterablePropertyBase, IFunction, ICalculate, ILinkSupport, ILoadSave, IDraw, IManualChange
    {
        private bool @out = false;
        private readonly bool[] getInputs;
        private readonly bool[] getInverseInputs;
        private readonly GetLinkValueMethod?[] getLinkInputs;
        private readonly (Guid, int)[] getLinkSources;
        private readonly string[] getInputNames;
        private readonly string[] getOutputNames;
        private readonly LogicFunction logicFunction;

        private Guid itemId;

        public void SetItemId(Guid id)
        {
            itemId = id;
        }

        public CommonLogic() : this(LogicFunction.None, 1)
        { 
        }

        public CommonLogic(LogicFunction func, int inputCount, int outputCount = 1)
        {
            logicFunction = func;
            getInputs = [];
            getInverseInputs = [];
            getLinkInputs = [];
            getLinkSources = [];
            getInputNames = [];
            getOutputNames = [];
            if (inputCount > 0)
            {
                if (func == LogicFunction.Not)
                    inputCount = 1;
                else if (func == LogicFunction.Rs || func == LogicFunction.Sr)
                    inputCount = 2;
                getInputs = new bool[inputCount];
                getInverseInputs = new bool[inputCount];
                getLinkInputs = new GetLinkValueMethod?[inputCount];
                getLinkSources = new (Guid, int)[inputCount];
                getInputNames = new string[inputCount];
                getOutputNames = new string[outputCount];
                if (func == LogicFunction.Not)
                {
                    InverseOut = true;
                }
                else if (func == LogicFunction.Rs || func == LogicFunction.Sr)
                {
                    getInputNames[0] = "S";
                    getInputNames[1] = "R";
                    getOutputNames[0] = "Q";
                }
            }

        }

        [Category(" Общие"), DisplayName("Функция")]
        public LogicFunction FuncName => logicFunction;

        [Category(" Общие"), DisplayName("Идентификатор")]
        public Guid ItemId => itemId;

        [Browsable(false)]
        public virtual string FuncSymbol 
        { 
            get 
            {
                return logicFunction switch
                {
                    LogicFunction.And => "&",
                    LogicFunction.Not or LogicFunction.Or => "1",
                    LogicFunction.Xor => "=1",
                    LogicFunction.Rs => "RS",
                    LogicFunction.Sr => "SR",
                    _ => "",
                };
            } 
        }

        [Category(" Общие"), DisplayName("Имя")]
        public string? Name { get; set; }

        [Browsable(false), Category("Диагностика"), DisplayName("Показывать значения")]
        public bool VisibleValues { get; set; } = true;

        [Category("Входы"), DisplayName("Управление")]
        public bool[] Inputs => getInputs;

        [Category("Входы"), DisplayName("Инверсия")]
        [DynamicPropertyFilter(nameof(FuncName), "And,Or")]
        public bool[] InverseInputs => getInverseInputs;

        [Category("Выходы"), DisplayName("Выход")]
        [DynamicPropertyFilter(nameof(FuncName), "Not,And,Or,Xor,Rs,Sr,Fe,Pulse,OnDelay,OffDelay")]
        public virtual bool Out
        {
            get => @out;
            protected set
            {
                if (@out == value) return;
                @out = value;
                ResultChanged?.Invoke(this, new ResultCalculateEventArgs(nameof(Out), value));
            }
        }

        [Category("Выходы"), DisplayName(" Инверсия"), DefaultValue(false)]
        [DynamicPropertyFilter(nameof(FuncName), "And,Or")]
        public bool InverseOut { get; set; } = false;

        [Browsable(false)]
        public bool[] InverseOutputs => [InverseOut];

        [Browsable(false)]
        public string[] InputNames => getInputNames;

        [Browsable(false)]
        public string[] OutputNames => getOutputNames;

        [Browsable(false)]
        public object[] InputValues
        {
            get
            {
                List<object> list = [];
                for (var i = 0; i < getInputs.Length; i++)
                {
                    if (getLinkInputs[i] is GetLinkValueMethod method)
                    {
                        bool value = (bool)method();
                        list.Add(value);
                    }
                    else
                        list.Add(getInputs[i]);
                }
                return [.. list];
            }
        }

        [Browsable(false)]
        public (Guid, int)[] InputLinkSources => getLinkSources;

        [Browsable(false)]
        public object[] OutputValues => [Out];

        [Browsable(false)]
        public bool[] LinkedInputs 
        { 
            get 
            {
                List<bool> list = [];
                for (var i = 0; i < getInputs.Length; i++)
                {
                    if (getLinkInputs[i] is GetLinkValueMethod _)
                        list.Add(true);
                    else
                        list.Add(false);
                }
                return [.. list];
            } 
        }

        [Browsable(false)]
        public object[] LinkedOutputs => [Out];


        public event ResultCalculateEventHandler? ResultChanged;

        public virtual void Calculate()
        {
            bool result = (bool)InputValues[0] ^ getInverseInputs[0];
            if (logicFunction == LogicFunction.Rs || logicFunction == LogicFunction.Sr)
            {
                getInverseInputs[0] = false;
                getInverseInputs[1] = false;
            }
            for (var i = 1; i < InputValues.Length; i++)
            {
                var input = (bool)InputValues[i] ^ getInverseInputs[i];
                result = logicFunction switch
                {
                    LogicFunction.And => result && input,
                    LogicFunction.Or => result || input,
                    LogicFunction.Xor => CalcXor(InputValues),
                    LogicFunction.Rs => CalcRsTrigger(result, input, @out),
                    LogicFunction.Sr => CalcSrTrigger(result, input, @out),
                    _ => logicFunction == LogicFunction.Not && !result,
                };
            }
            if (logicFunction == LogicFunction.Rs)
                InverseOut = false;
            Out = result ^ InverseOut;
        }

        private static bool CalcXor(object[] inputValues)
        {
            return inputValues.Count(x => (bool)x == true) == 1;
        }

        private static bool CalcRsTrigger(bool s, bool r, bool q)
        {
            return !r && (s || q);
        }

        private static bool CalcSrTrigger(bool s, bool r, bool q)
        {
            return s || (!r && q); 
        }

        public GetLinkValueMethod? GetResultLink(int outputIndex)
        {
            return () => Out;
        }

        /// <summary>
        /// Для создания связи записывается ссылка на метод,
        /// который потом вызывается для получения актуального значения
        /// </summary>
        /// <param name="inputIndex">номер входа</param>
        /// <param name="getMethod">Ссылка на метод, записываемая в целевом элементе, для этого входа</param>
        public void SetValueLinkToInp(int inputIndex, GetLinkValueMethod? getMethod, Guid sourceId, int outputPinIndex)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                getLinkInputs[inputIndex] = getMethod;
                getLinkSources[inputIndex] = (sourceId, outputPinIndex);
            }
        }

        public void ResetValueLinkToInp(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                getLinkInputs[inputIndex] = null;
                getLinkSources[inputIndex] = (Guid.Empty, 0);
            }
        }

        public void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length &&
                value != null && getLinkInputs[inputIndex] == null)
                getInputs[inputIndex] = (bool)value;
        }

        public object? GetValueFromInp(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                if (getLinkInputs[inputIndex] != null)
                {
                    var method = getLinkInputs[inputIndex];
                    return method?.Invoke();
                }
                else
                    return getInputs[inputIndex];
            }
            return null;
        }

        public virtual void Save(XElement xtem)
        {
            var xtance = new XElement("Instance");

            if (!string.IsNullOrWhiteSpace(Name))
                xtance.Add(new XAttribute("Name", Name));
            XElement xinputs = new("Inputs");
            bool customInputs = false;
            for (var i = 0; i < Inputs.Length; i++)
            {
                (Guid id, int output) = getLinkSources[i];
                if (string.IsNullOrWhiteSpace(InputNames[i]) &&
                    !InverseInputs[i] && !Inputs[i] &&
                    id == Guid.Empty) continue;
                customInputs = true;
                XElement xinput = new("Input");
                xinputs.Add(xinput);
                xinput.Add(new XAttribute("Index", i));

                if (!string.IsNullOrWhiteSpace(InputNames[i]))
                    xinput.Add(new XAttribute("Name", InputNames[i]));
                if (InverseInputs[i])
                    xinput.Add(new XAttribute("Invert", InverseInputs[i]));
                if (Inputs[i])
                    xinput.Add(new XAttribute("Value", Inputs[i]));
                if (id != Guid.Empty)
                {
                    xinput.Add(new XElement("SourceId", id));
                    if (output > 0)
                        xinput.Add(new XElement("OutputIndex", output));
                }
            }
            if (customInputs)
                xtance.Add(xinputs);
            if (OutputNames.Length > 0)
            {
                XElement xoutputs = new("Outputs");
                bool customOutputs = false;
                for (var i = 0; i < InverseOutputs.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(OutputNames[i]) &&
                        !InverseOutputs[i]) continue;
                    customOutputs = true;
                    XElement xoutput = new("Output");
                    xoutputs.Add(xoutput);
                    xoutput.Add(new XAttribute("Index", i));
                    if (!string.IsNullOrWhiteSpace(OutputNames[i]))
                        xoutput.Add(new XAttribute("Name", OutputNames[i]));
                    if (InverseOutputs[i])
                        xoutput.Add(new XAttribute("Invert", InverseOutputs[i]));
                }
                if (customOutputs)
                {
                    xtance.Add(xoutputs);
                    xtem.Add(xtance);
                }
            }
            if (customInputs)
                xtem.Add(xtance);
        }

        public virtual void Load(XElement? xtance)
        {
            var instname = xtance?.Attribute("Name")?.Value;
            if (instname != null) 
                Name = instname;
            var xinputs = xtance?.Element("Inputs");
            if (xinputs != null)
            {
                foreach (XElement item in xinputs.Elements("Input"))
                {
                    if (int.TryParse(item.Attribute("Index")?.Value, out int index))
                    {
                        var name = item.Attribute("Name")?.Value;
                        if (name != null) InputNames[index] = name;
                        if (bool.TryParse(item.Attribute("Invert")?.Value, out bool invert))
                            InverseInputs[index] = invert;
                        if (Guid.TryParse(item.Element("SourceId")?.Value, out Guid guid) && guid != Guid.Empty)
                        {
                            if (int.TryParse(item.Element("OutputIndex")?.Value, out int outputIndex))
                                getLinkSources[index] = (guid, outputIndex);
                            else
                                getLinkSources[index] = (guid, 0);
                        }
                        else if (bool.TryParse(item.Attribute("Value")?.Value, out bool value))
                            getInputs[index] = value;
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
                        var name = item.Attribute("Name")?.Value;
                        if (name != null) OutputNames[index] = name;
                        if (bool.TryParse(item.Attribute("Invert")?.Value, out bool invert))
                            InverseOutputs[index] = invert;
                    }
                }
            }
        }

        public void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size, int index, bool selected, CustomDraw? customDraw = null)
        {
            using var brush = new SolidBrush(Color.FromArgb(255, backColor));
            using var pen = new Pen(selected ? Color.Magenta : foreColor, 1f);
            using var font = new Font("Consolas", Element.Step + 2f);
            using var fontbrush = new SolidBrush(selected ? Color.Magenta : foreColor);
            var named = !string.IsNullOrEmpty(Name);
            var max = Math.Max(InverseInputs.Length, InverseOutputs.Length);
            var step = Element.Step;
            var height = step + max * step * 4 + step;
            var width = step + 1 * step * 4 + step;
            var rect = new RectangleF(location, size);
            if (customDraw == null)
            {
                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangles(pen, [rect]);
            }
            // обозначение функции, текст по-центру, в верхней части рамки элемента
            using var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            var msn = graphics.MeasureString(Name, font);
            if (named)
                graphics.DrawString(Name, font, fontbrush, new PointF(location.X + width / 2, location.Y - msn.Height), format);
            graphics.DrawString(FuncSymbol, font, fontbrush, new PointF(location.X + width / 2, location.Y), format);
            customDraw?.Invoke(graphics, rect, pen, brush, font, fontbrush, index);
            // входы
            var y = step + location.Y;
            var x = -step + location.X;
            for (var i = 0; i < InverseInputs.Length; i++)
            {
                y += step * 2;
                // горизонтальная риска слева, напротив входа
                graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                if (InverseInputs[i])
                {
                    var r = new RectangleF(x + step / 2, y - step / 2, step, step);
                    // рисуем кружок инверсии
                    graphics.FillEllipse(brush, r);
                    graphics.DrawEllipse(pen, r);
                }
                // наименование входа
                if (!string.IsNullOrEmpty(InputNames[i]))
                {
                    var ms = graphics.MeasureString(InputNames[i], font);
                    graphics.DrawString(InputNames[i], font, fontbrush, new PointF(x + step, y - ms.Height / 2));
                }
                // значение входа - отображаются только не связанные (свободные) входы
                if (VisibleValues && this is ILinkSupport link && !link.LinkedInputs[i])
                {
                    var value = link.InputValues[i];
                    var text = value != null && value.GetType() == typeof(bool) ? (bool)value ? "T" : "F" : $"{value}";
                    var ms = graphics.MeasureString(text, font);
                    using var iformat = new StringFormat();
                    iformat.Alignment = StringAlignment.Near;
                    graphics.DrawString(text, font, fontbrush, new PointF(x - ms.Width + step, y - ms.Height), iformat);
                }
                y += step * 2;
            }
            // выходы
            y = step + location.Y;
            x = width + location.X;
            for (var i = 0; i < InverseOutputs.Length; i++)
            {
                if (InverseOutputs.Length == 1)
                    y = height / 2 + location.Y;
                else
                    y += step * 2;
                if (OutputNames.Length > 0)
                {
                    // горизонтальная риска справа, напротив выхода
                    graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                }
                if (InverseOutputs[i])
                {
                    var r = new RectangleF(x - step / 2, y - step / 2, step, step);
                    // рисуем кружок инверсии
                    graphics.FillEllipse(brush, r);
                    graphics.DrawEllipse(pen, r);
                }
                // наименование выхода
                if (OutputNames.Length > 0 && !string.IsNullOrEmpty(OutputNames[i]))
                {
                    var ms = graphics.MeasureString(OutputNames[i], font);
                    graphics.DrawString(OutputNames[i], font, fontbrush, new PointF(x - ms.Width, y - ms.Height / 2));
                }
                // значение выхода
                if (OutputNames.Length > 0 && VisibleValues && this is ILinkSupport link)
                {
                    var value = link.OutputValues[i];
                    var text = value != null && value.GetType() == typeof(bool) ? (bool)value ? "T" : "F" : $"{value}";
                    var ms = graphics.MeasureString(text, font);
                    graphics.DrawString(text, font, fontbrush, new PointF(x, y - ms.Height));
                }
                y += step * 2;
            }
            // индекс элемента в списке
            if (customDraw == null && index != 0)
            {
                var text = $"L{index}";
                var ms = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, fontbrush, new PointF(location.X + width / 2, location.Y + height - ms.Height), format);
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
            max = Math.Max(LinkedInputs.Length, LinkedOutputs.Length);
            height = step + max * step * 4 + step;
            size = new SizeF(width, height);
            // входы
            var y = step + location.Y;
            var x = -step + location.X;
            var n = 0;
            itargets.Clear();
            ipins.Clear();
            for (var i = 0; i < LinkedInputs.Length; i++)
            {
                y += step * 2;
                // значение входа
                var ms = new SizeF(step * 2, step * 2);
                itargets.Add(n, new RectangleF(new PointF(x - ms.Width + step, y - ms.Height), ms));
                ipins.Add(n, new PointF(x, y));
                y += step * 2;
                n++;
            }
            // выходы
            y = step + location.Y;
            x = width + location.X;
            n = 0;
            otargets.Clear();
            opins.Clear();
            for (var i = 0; i < OutputNames.Length; i++)
            {
                if (OutputNames.Length == 1)
                    y = height / 2 + location.Y;
                else
                    y += step * 2;
                // значение выхода
                var ms = new SizeF(step * 2, step * 2);
                otargets.Add(n, new RectangleF(new PointF(x, y - ms.Height), ms));
                var pt = new PointF(x + step, y);
                opins.Add(n, pt);
                y += step * 2;
                n++;
            }
        }
    }
}
