using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class CommonLogic : FilterablePropertyBase, IFunction
    {
        private bool @out = false;
        private readonly bool[] getInputs;
        private readonly bool[] getInverseInputs;
        private readonly GetLinkValueMethod?[] getLinkInputs;
        private readonly IFunction?[] getLinkOutputSources;
        private readonly string[] getInputNames;
        private readonly LogicFunction logicFunction;

        public CommonLogic() : this(LogicFunction.None, 1)
        { 
        }

        public CommonLogic(LogicFunction func, int inputCount)
        {
            logicFunction = func;
            getInputs = [];
            getInverseInputs = [];
            getLinkInputs = [];
            getLinkOutputSources = [];
            getInputNames = [];
            if (inputCount > 0)
            {
                if (func == LogicFunction.Not)
                    inputCount = 1;
                else if (func == LogicFunction.Xor || func == LogicFunction.Rs)
                    inputCount = 2;
                getInputs = new bool[inputCount];
                getInverseInputs = new bool[inputCount];
                getLinkInputs = new GetLinkValueMethod?[inputCount];
                getLinkOutputSources = new IFunction?[inputCount];
                getInputNames = new string[inputCount];
                if (func == LogicFunction.Not)
                {
                    InverseOut = true;
                }
                else if (func == LogicFunction.Rs)
                {
                    getInputNames[0] = "S";
                    getInputNames[1] = "R";
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
        public string[] OutputNames => [string.Empty];

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
        public IFunction[] LinkedInputSources
        {
            get
            {
                List<IFunction> list = [];
                for (var i = 0; i < getLinkOutputSources.Length; i++)
                {
                    if (getLinkOutputSources[i] is IFunction function)
                        list.Add(function);
                    else
                        list.Add(null);
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
                    LogicFunction.Xor => result ^ input,
                    LogicFunction.Rs => CalcRsTrigger(result, input, @out),
                    _ => logicFunction == LogicFunction.Not && !result,
                };
            }
            if (logicFunction == LogicFunction.Rs)
                InverseOut = false;
            Out = result ^ InverseOut;
        }

        private static bool CalcRsTrigger(bool s, bool r, bool q)
        {
            return !r && (s || q);
        }

        public GetLinkValueMethod? GetResultLink(int outputIndex)
        {
            return () => Out;
        }

        public void SetValueLinkToInp(int inputIndex, IFunction source, GetLinkValueMethod? getInp)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                getLinkInputs[inputIndex] = getInp;
                getLinkOutputSources[inputIndex] = source;
            }
        }

        public void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length &&
                value != null && getLinkInputs[inputIndex] == null)
                getInputs[inputIndex] = (bool)value;
        }
    }
}
