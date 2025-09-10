using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Simulator.Model.Logic
{
    public class CommonLogic : FilterablePropertyBase, IFunction
    {
        private bool @out = false;
        private readonly bool[] getInputs;
        private readonly bool[] getInverseInputs;
        private readonly GetLinkValueMethod?[] getLinkInputs;
        private readonly (Guid, int)[] getLinkSources;
        private readonly string[] getInputNames;
        private readonly string[] getOutputNames;
        private readonly LogicFunction logicFunction;

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
                else if (func == LogicFunction.Rs)
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
                else if (func == LogicFunction.Rs)
                {
                    getInputNames[0] = "S";
                    getInputNames[1] = "R";
                    getOutputNames[0] = "Q";
                }
            }

        }

        [Category(" Общие"), DisplayName("Функция")]
        public LogicFunction FuncName => logicFunction;

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
        public bool Out
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

        public event ResultCalculateEventHandler? ResultChanged;

        public virtual void Calculate()
        {
            bool result = (bool)InputValues[0] ^ getInverseInputs[0];
            if (logicFunction == LogicFunction.Rs)
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

        public void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length &&
                value != null && getLinkInputs[inputIndex] == null)
                getInputs[inputIndex] = (bool)value;
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
                xtance.Add(xoutputs);

            if (customInputs || customOutputs)
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
    }
}
