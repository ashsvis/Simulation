using Simulator.Model.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Simulator.Model.Logic
{
    public class CommonLogic : FilterablePropertyBase, IFunction, ICalculate, ILinkSupport, ILoadSave, IDraw, IManualChange, IContextMenu
    {
        private readonly bool[] getInputs;
        private readonly bool[] getInverseInputs;
        protected readonly (Guid, int, bool)[] getLinkSources;
        private readonly string[] getInputNames;
        private readonly object[] getOutputs;
        private readonly bool[] getInverseOutputs;
        private readonly string[] getOutputNames;
        private readonly LogicFunction logicFunction;

        private Guid itemId;
        public void SetItemId(Guid id)
        {
            itemId = id;
            for (var i = 0; i < getLinkSources.Length; i++) 
            {
                (Guid sourceId, _, _) = getLinkSources[i];
                if (sourceId == Guid.Empty)
                    Project.WriteValue(id, i, ValueSide.Input, ValueKind.Digital, getInputs[i]);
            }
        }

        public void Init()
        {
            if (itemId == Guid.Empty) return;
            for (var i = 0; i < getLinkSources.Length; i++)
            {
                (Guid sourceId, _, _) = getLinkSources[i];
                if (sourceId == Guid.Empty)
                    Project.WriteValue(itemId, i, ValueSide.Input, ValueKind.Digital, getInputs[i]);
            }
        }

        public CommonLogic() : this(LogicFunction.None, 1)
        { 
        }

        public CommonLogic(LogicFunction func, int inputCount, int outputCount = 1)
        {
            logicFunction = func;
            getInputs = [];
            getInverseInputs = [];
            getLinkSources = [];
            getInputNames = [];
            getOutputs = [];
            getInverseOutputs = [];
            getOutputNames = [];
            if (inputCount > 0)
            {
                getInputs = new bool[inputCount];
                getInverseInputs = new bool[inputCount];
                getLinkSources = new (Guid, int, bool)[inputCount];
                getInputNames = new string[inputCount];
            }
            getOutputs = new object[outputCount];
            for (var i = 0; i < outputCount; i++) getOutputs[i] = false;
            getInverseOutputs = new bool[outputCount];
            getOutputNames = new string[outputCount];
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
                    LogicFunction.Or => "1",
                    LogicFunction.Xor => "=1",
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

        [Category("Выходы"), DisplayName("Выход 1")]
        [DynamicPropertyFilter(nameof(FuncName), "Not,And,Or,Xor,Rs,Sr,Fe,Pulse,OnDelay,OffDelay")]
        public virtual bool Out
        {
            get => getOutputs.Length > 0 && (bool)(getOutputs[0] ?? false);
            protected set
            {
                if (getOutputs.Length == 0 || (bool)(getOutputs[0] ?? false) == value) return;
                getOutputs[0] = value;
                ResultChanged?.Invoke(this, new ResultCalculateEventArgs(nameof(Out), value));
            }
        }

        [Category("Выходы"), DisplayName("Состояние")]
        [DynamicPropertyFilter(nameof(FuncName), "Assembly")]
        public object[] Outputs => getOutputs;

        [Category("Выходы"), DisplayName(" Инверсия"), DefaultValue(false)]
        [DynamicPropertyFilter(nameof(FuncName), "And,Or")]
        public bool[] InverseOutputs => getInverseOutputs;

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
                    (Guid id, int pinout, bool external) = getLinkSources[i];
                    if (id != Guid.Empty)
                    {
                        ValueItem? value = Project.ReadValue(id, pinout, ValueSide.Output, ValueKind.Digital);
                        if (value != null && value.Value != null)
                            list.Add(value.Value);
                        else
                        {
                            ValueItem? val = Project.ReadValue(itemId, i, ValueSide.Input, ValueKind.Digital);
                            list.Add(val?.Value ?? false);
                        }
                    }
                    else
                    {
                        ValueItem? value = Project.ReadValue(itemId, i, ValueSide.Input, ValueKind.Digital);
                        list.Add(value?.Value ?? false);
                    }
                }
                return [.. list];
            }
        }

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
        public object[] OutputValues => getOutputs;

        [Browsable(false)]
        public bool[] LinkedInputs 
        { 
            get 
            {
                List<bool> list = [];
                for (var i = 0; i < getInputs.Length; i++)
                {
                    (Guid id, _, _) = getLinkSources[i];
                    list.Add(id != Guid.Empty);
                }
                return [.. list];
            } 
        }

        [Browsable(false)]
        public object[] LinkedOutputs => [Out];

        public event ResultCalculateEventHandler? ResultChanged;

        public virtual void Calculate()
        {
            bool result = GetInputValue(0) ^ getInverseInputs[0];
            for (var i = 0; i < InputValues.Length; i++)
            {
                var input = GetInputValue(i) ^ getInverseInputs[i];
                result = logicFunction switch
                {
                    LogicFunction.And => result && input,
                    LogicFunction.Or => result || input,
                    LogicFunction.Xor => CalcXor(InputValues),
                    _ => logicFunction == LogicFunction.None && result,
                };
            }
            var @out = result ^ getInverseOutputs[0];
            Out = @out;
            Project.WriteValue(itemId, 0, ValueSide.Output, ValueKind.Digital, Out);
        }

        protected bool GetInputValue(int pin)
        {
            (Guid id, int pinout, bool external) = getLinkSources[pin];
            if (id != Guid.Empty)
                return (bool)(Project.ReadValue(id, pinout, ValueSide.Output, ValueKind.Digital)?.Value ?? false);
            return (bool)(Project.ReadValue(itemId, pin, ValueSide.Input, ValueKind.Digital)?.Value ?? false);
        }

        private static bool CalcXor(object[] inputValues)
        {
            return inputValues.Count(x => (bool)x == true) == 1;
        }

        /// <summary>
        /// Для создания связи записывается ссылка на метод,
        /// который потом вызывается для получения актуального значения
        /// </summary>
        /// <param name="inputIndex">номер входа</param>
        /// <param name="getMethod">Ссылка на метод, записываемая в целевом элементе, для этого входа</param>
        public virtual void SetValueLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog)
        {
            if (inputIndex < 0 || inputIndex >= getLinkSources.Length)
                throw new ArgumentOutOfRangeException(nameof(inputIndex));
            getLinkSources[inputIndex] = (sourceId, outputPinIndex, byDialog);
        }

        public virtual void ResetValueLinkToInp(int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= getLinkSources.Length)
                throw new ArgumentOutOfRangeException(nameof(inputIndex));
            getLinkSources[inputIndex] = (Guid.Empty, 0, false);
        }

        public void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex < 0 || inputIndex >= getLinkSources.Length)
                throw new ArgumentOutOfRangeException(nameof(inputIndex));
            if (value != null && !LinkedInputs[inputIndex])
            {
                getInputs[inputIndex] = (bool)value;
                Project.WriteValue(itemId, inputIndex, ValueSide.Input, ValueKind.Digital, (bool)value);
            }
        }

        public object? GetValueFromInp(int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= getLinkSources.Length)
                throw new ArgumentOutOfRangeException(nameof(inputIndex));
            (Guid id, int pin, bool external) = getLinkSources[inputIndex];
            if (id != Guid.Empty)
            { 
                ValueItem? valtem = Project.ReadValue(id, pin, ValueSide.Output, ValueKind.Digital);
                return valtem?.Value;
            }
            else
            {
                ValueItem? valtem = Project.ReadValue(itemId, inputIndex, ValueSide.Input, ValueKind.Digital);
                return valtem?.Value;
            }
        }

        public virtual void Save(XElement xtance)
        {
            if (Inputs.Length > 0)
            {
                XElement xinputs = new("Inputs");
                for (var i = 0; i < Inputs.Length; i++)
                {
                    (Guid id, int output, bool external) = getLinkSources[i];
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
                        XElement xsource = new("Source");
                        xsource.Add(new XAttribute("Id", id));
                        if (output > 0)
                            xsource.Add(new XAttribute("PinIndex", output));
                        if (external)
                            xsource.Add(new XAttribute("External", external));
                        xinput.Add(xsource);
                    }
                }
                xtance.Add(xinputs);
            }
            if (OutputNames.Length > 0)
            {
                XElement xoutputs = new("Outputs");
                for (var i = 0; i < InverseOutputs.Length; i++)
                {
                    XElement xoutput = new("Output");
                    xoutputs.Add(xoutput);
                    xoutput.Add(new XAttribute("Index", i));
                    if (!string.IsNullOrWhiteSpace(OutputNames[i]))
                        xoutput.Add(new XAttribute("Name", OutputNames[i]));
                    if (InverseOutputs[i])
                        xoutput.Add(new XAttribute("Invert", InverseOutputs[i]));
                }
                xtance.Add(xoutputs);
            }
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
                        if (bool.TryParse(item.Attribute("Value")?.Value, out bool value))
                        {
                            getInputs[index] = value;
                            Project.WriteValue(itemId, index, ValueSide.Input, ValueKind.Digital, value);
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
                        var name = item.Attribute("Name")?.Value;
                        if (name != null) OutputNames[index] = name;
                        if (bool.TryParse(item.Attribute("Invert")?.Value, out bool invert))
                            InverseOutputs[index] = invert;
                    }
                }
            }
        }

        public void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size, 
            int index, bool selected, CustomDraw? customDraw = null)
        {
            try
            {
                using var brush = new SolidBrush(Color.FromArgb(255, backColor));
                using var pen = new Pen(selected ? Color.Magenta : foreColor, 1f);
                using var font = new Font("Consolas", Element.Step + 2f);
                using var fontbrush = new SolidBrush(selected ? Color.Magenta : foreColor);
                using var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                var max = Math.Max(InverseInputs.Length, InverseOutputs.Length);
                var step = Element.Step;
                var height = step + max * step * 4 + step;
                var width = step + 1 * step * 4 + step;
                var rect = new RectangleF(location, size);
                if (customDraw == null)
                {
                    graphics.FillRectangle(brush, rect);
                    graphics.DrawRectangles(pen, [rect]);
                    // обозначение функции, текст по-центру, в верхней части рамки элемента
                    var named = !string.IsNullOrEmpty(Name);
                    if (named)
                    {
                        var msn = graphics.MeasureString(Name, font);
                        graphics.DrawString(Name, font, fontbrush, new PointF(location.X + width / 2, location.Y - msn.Height), format);
                    }
                }
                graphics.DrawString(FuncSymbol, font, fontbrush, new PointF(location.X + width / 2, location.Y), format);
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
                    // значение входа
                    bool isLinked = this is ILinkSupport link && link.LinkedInputs[i];
                    bool isExternal = this is ILinkSupport link1 && link1.InputLinkSources[i].Item3;
                    if (Project.Running && VisibleValues)
                    {
                        var text = $"{GetInputValue(i)}"[..1].ToUpper();
                        var ms = graphics.MeasureString(text, font);
                        using var iformat = new StringFormat();
                        iformat.Alignment = StringAlignment.Near;
                        using var br = new SolidBrush(isLinked ? Color.Gray : foreColor);
                        graphics.DrawString(text, font, br, new PointF(x - ms.Width + step, y - ms.Height), iformat);
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
                    if (InverseOutputs[i] && customDraw == null)
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
                    if (Project.Running && OutputNames.Length > 0 && VisibleValues && this is ILinkSupport link)
                    {
                        var text = $"{Project.ReadValue(itemId, i, ValueSide.Output, ValueKind.Digital)?.Value ?? false}"[..1].ToUpper();
                        var ms = graphics.MeasureString(text, font);
                        graphics.DrawString(text, font, fontbrush, new PointF(x, y - ms.Height));
                    }
                    y += step * 2;
                }
                customDraw?.Invoke(graphics, rect, pen, brush, font, fontbrush, index);
                // индекс элемента в списке
                if (customDraw == null && index != 0)
                {
                    var text = $"L{index}";
                    var ms = graphics.MeasureString(text, font);
                    graphics.DrawString(text, font, fontbrush, new PointF(location.X + width / 2, location.Y + height - ms.Height), format);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public virtual void CalculateTargets(PointF location, ref SizeF size,
             Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins,
             Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            var step = Element.Step;
            var width = step + 1 * step * 4 + step;
            var max = Math.Max(LinkedInputs.Length, LinkedOutputs.Length);
            var height = step + max * step * 4 + step;
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

        public void ClearContextMenu(ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Clear();
        }

        public virtual void AddMenuItems(ContextMenuStrip contextMenu)
        {
            // stub
        }
    }
}
