using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;

namespace Simulator.Model.Logic
{
    public class CommonLogic : FilterablePropertyBase, IFunction, ICalculate, ILoadSave, IDraw, IManualChange, IContextMenu, ILinkSupport
    {
        protected readonly LogicFunction logicFunction;

        private readonly Input[] inputs;
        private readonly Output[] outputs;

        [Browsable(false)]
        public Input[] Inputs => inputs;
        [Browsable(false)]
        public Output[] Outputs => outputs;

        protected Guid itemId;
        public virtual void SetItemId(Guid id)
        {
            itemId = id;
            for (var i = 0; i < inputs.Length; i++)
                inputs[i].ItemId = itemId;
            for (var i = 0; i < outputs.Length; i++)
                outputs[i].ItemId = itemId;
        }

        public virtual void Init()
        {
            if (itemId == Guid.Empty) return;
        }

        public CommonLogic() : this(LogicFunction.None, 1)
        { 
        }

        public CommonLogic(LogicFunction func, int inputCount, int outputCount = 1)
        {
            inputs = new Input[inputCount];
            for (var i = 0; i < inputs.Length; i++)
            {
                inputs[i] = new DigitalInput(itemId, i);
            }
            outputs = new Output[outputCount];
            for (var i = 0; i < outputs.Length; i++)
            {
                outputs[i] = new DigitalOutput(itemId, i);
            }
            logicFunction = func;
        }

        [Category(" Общие"), DisplayName("Функция")]
        public LogicFunction FuncName => logicFunction;

        [Browsable(false)]
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
                    LogicFunction.SelD => "SEL",
                    _ => "",
                };
            } 
        }

        [Category(" Общие"), DisplayName("Имя тега")]
        public string? Name { get; set; }

        [Browsable(false), Category("Диагностика"), DisplayName("Показывать значения")]
        public bool VisibleValues { get; set; } = true;

        public void UpdateInputLinkSources((Guid, int, bool) seek, Guid newId)
        {
            for (var i = 0; i < inputs.Length; i++)
            {
                var ls = inputs[i].LinkSource;
                if (ls == null) continue;
                if (ls.Id == seek.Item1 && ls.PinIndex == seek.Item2)
                {
                    inputs[i].LinkSource = new LinkSource(newId, seek.Item2, seek.Item3);
                }
            }
        }

        public virtual void Calculate()
        {
            bool result = (bool)(GetInputValue(0) ?? false) ^ GetInverseInputs(0);
            for (var i = 0; i < inputs.Length; i++)
            {
                var input = (bool)(GetInputValue(i) ?? false) ^ GetInverseInputs(i);
                result = logicFunction switch
                {
                    LogicFunction.And => result && input,
                    LogicFunction.Or => result || input,
                    LogicFunction.Xor => CalcXor(inputs),
                    _ => logicFunction == LogicFunction.None && result,
                };
            }
            SetValueToOut(0, result ^ GetInverseOutputs(0));
        }

        private static bool CalcXor(object[] inputValues)
        {
            return inputValues.Count(x => (bool)x == true) == 1;
        }

        public bool GetInverseInputs(int pin)
        {
            if (pin < inputs.Length && inputs[pin] is DigitalInput input)
                return input.Inverse;
            return false;
        }

        public bool GetInverseOutputs(int pin)
        {
            if (pin < outputs.Length && outputs[pin] is DigitalOutput output)
                return output.Inverse;
            return false;
        }

        public object? GetInputValue(int pin)
        {
            if (pin < inputs.Length)
            {
                if (inputs[pin] is DigitalInput dinput)
                    return dinput.Value;
                if (inputs[pin] is AnalogInput ainput)
                    return ainput.Value;
            }
            return null;
        }

        public object? GetOutputValue(int pin)
        {
            var item = Project.ReadValue(itemId, pin, ValueDirect.Output, ValueKind.Digital);
            if (item != null)
                return (bool)(item?.Value ?? false);
            item = Project.ReadValue(itemId, pin, ValueDirect.Output, ValueKind.Analog);
            if (item != null)
                return (double)(item?.Value ?? 0.0);
            return null;
        }

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

        public void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex < 0 || inputIndex >= inputs.Length) return;
            if (value != null)
            {
                if (inputs[inputIndex] is DigitalInput digital)
                    digital.Value = (bool)value;
                if (inputs[inputIndex] is AnalogInput analog)
                    analog.Value = (double)value;
            }
        }

        public void SetValueToOut(int outputIndex, object? value)
        {
            if (outputIndex < 0 || outputIndex >= outputs.Length) return;
            if (value != null)
            {
                if (outputs[outputIndex] is DigitalOutput digital)
                    digital.Value = (bool)value;
                if (outputs[outputIndex] is AnalogOutput analog)
                    analog.Value = (double)value;
            }
        }

        public virtual object? GetValueFromInp(int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= inputs.Length) return null;
            if (inputs[inputIndex] is DigitalInput digital)
                return digital.Value;
            if (inputs[inputIndex] is AnalogInput analog)
                return analog.Value;
            return null;
        }

        public virtual void Save(XElement xtance)
        {
            if (Inputs.Length > 0)
            {
                XElement xinputs = new("Inputs");
                for (var i = 0; i < inputs.Length; i++)
                {
                    XElement xinput = new("Input");
                    xinputs.Add(xinput);
                    xinput.Add(new XAttribute("Index", i));
                    var name = inputs[i].Name;
                    if (!string.IsNullOrWhiteSpace(name))
                        xinput.Add(new XAttribute("Name", name));
                    if (inputs[i] is DigitalInput input && input.Inverse)
                        xinput.Add(new XAttribute("Invert", true));
                    if (inputs[i].LinkSource == null)
                    {
                        if (inputs[i] is DigitalInput binput && binput.Value)
                            xinput.Add(new XAttribute("Value", binput.Value));
                        if (inputs[i] is AnalogInput ainput && ainput.Value != 0.0)
                            xinput.Add(new XAttribute("Value", ainput.Value));
                    }
                    else
                    {
                        var ls = inputs[i].LinkSource;
                        if (ls != null)
                        {
                            XElement xsource = new("Source");
                            xsource.Add(new XAttribute("Id", ls.Id));
                            if (ls.PinIndex > 0)
                                xsource.Add(new XAttribute("PinIndex", ls.PinIndex));
                            if (ls.External)
                                xsource.Add(new XAttribute("External", true));
                            xinput.Add(xsource);
                        }
                    }
                }
                xtance.Add(xinputs);
            }
            if (outputs.Length > 0)
            {
                XElement xoutputs = new("Outputs");
                for (var i = 0; i < outputs.Length; i++)
                {
                    XElement xoutput = new("Output");
                    xoutputs.Add(xoutput);
                    xoutput.Add(new XAttribute("Index", i));
                    var name = outputs[i].Name;
                    if (!string.IsNullOrWhiteSpace(name))
                        xoutput.Add(new XAttribute("Name", name));
                    if (outputs[i] is DigitalOutput output && output.Inverse)
                        xoutput.Add(new XAttribute("Invert", true));
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
                        if (name != null) inputs[index].Name = name;
                        if (bool.TryParse(item.Attribute("Invert")?.Value, out bool invert))
                        {
                            if (inputs[index] is DigitalInput input)
                                input.Inverse = invert;
                        }
                        var xsource = item.Element("Source");
                        if (xsource != null)
                        {
                            if (Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid guid) && guid != Guid.Empty)
                            {
                                var external = false;
                                if (bool.TryParse(xsource.Attribute("External")?.Value, out bool bval))
                                    external = bval;
                                if (int.TryParse(xsource.Attribute("PinIndex")?.Value, out int outputIndex))
                                    inputs[index].LinkSource = new LinkSource(guid, outputIndex, external);
                                else
                                    inputs[index].LinkSource = new LinkSource(guid, 0, external);
                            }
                        }
                        if (bool.TryParse(item.Attribute("Value")?.Value, out bool bvalue) && inputs[index] is DigitalInput dinput)
                        {
                            dinput.Value = bvalue;
                        }
                        else if (double.TryParse(item.Attribute("Value")?.Value, out double avalue) && inputs[index] is AnalogInput ainput)
                        {
                            ainput.Value = avalue;
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
                        if (name != null) outputs[index].Name = name;
                        if (bool.TryParse(item.Attribute("Invert")?.Value, out bool invert) && outputs[index] is DigitalOutput doutput)
                            doutput.Inverse = invert;
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
                var max = Math.Max(inputs.Length, outputs.Length);
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
                for (var i = 0; i < inputs.Length; i++)
                {
                    y += step * 2;
                    // горизонтальная риска слева, напротив входа
                    graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                    if (inputs[i] is DigitalInput input && input.Inverse)
                    {
                        var r = new RectangleF(x + step / 2, y - step / 2, step, step);
                        // рисуем кружок инверсии
                        graphics.FillEllipse(brush, r);
                        graphics.DrawEllipse(pen, r);
                    }
                    // наименование входа
                    var name = inputs[i].Name ?? "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        var ms = graphics.MeasureString(name, font);
                        graphics.DrawString(name, font, fontbrush, new PointF(x + step, y - ms.Height / 2));
                    }
                    // значение входа
                    var ls = inputs[i].LinkSource;
                    bool isLinked = ls != null;
                    bool isExternal = ls != null && ls.External;
                    if (Project.Running && VisibleValues)
                    {
                        var text = string.Empty;
                        var value = GetInputValue(i);
                        if (value is bool bval)
                            text = $"{bval}"[..1].ToUpper();
                        else if (value is double dval)
                        {
                            var fp = CultureInfo.GetCultureInfo("en-US");
                            text = Math.Round(dval, 4).ToString("0.####", fp);
                        }
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
                for (var i = 0; i < outputs.Length; i++)
                {
                    if (outputs.Length == 1)
                        y = height / 2 + location.Y;
                    else
                        y += step * 2;
                    if (outputs.Length > 0)
                    {
                        // горизонтальная риска справа, напротив выхода
                        graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                    }
                    if (outputs[i] is DigitalOutput output && output.Inverse && customDraw == null)
                    {
                        var r = new RectangleF(x - step / 2, y - step / 2, step, step);
                        // рисуем кружок инверсии
                        graphics.FillEllipse(brush, r);
                        graphics.DrawEllipse(pen, r);
                    }
                    // наименование выхода
                    var name = outputs.Length > 0 ? outputs[i].Name ?? ""  : "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        var ms = graphics.MeasureString(name, font);
                        graphics.DrawString(name, font, fontbrush, new PointF(x - ms.Width, y - ms.Height / 2));
                    }
                    // значение выхода
                    if (Project.Running && outputs.Length > 0 && VisibleValues)
                    {
                        var text = string.Empty;
                        var value = GetOutputValue(i);
                        if (value is bool bval)
                            text = $"{bval}"[..1].ToUpper();
                        else if (value is double dval)
                        {
                            var fp = CultureInfo.GetCultureInfo("en-US");
                            text = Math.Round(dval, 4).ToString("0.####", fp);
                        }
                        var ms = graphics.MeasureString(text, font);
                        graphics.DrawString(text, font, fontbrush, new PointF(x, y - ms.Height));
                    }
                    y += step * 2;
                }
                customDraw?.Invoke(graphics, rect, pen, brush, font, fontbrush, index, selected);
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
            var max = Math.Max(inputs.Length, outputs.Length);
            var height = step + max * step * 4 + step;
            size = new SizeF(width, height);
            // входы
            var y = step + location.Y;
            var x = -step + location.X;
            var n = 0;
            itargets.Clear();
            ipins.Clear();
            for (var i = 0; i < inputs.Length; i++)
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
            for (var i = 0; i < outputs.Length; i++)
            {
                if (outputs.Length == 1)
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
