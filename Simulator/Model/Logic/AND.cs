using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class AND : IFunction
    {
        private bool @out = false;
        private readonly bool[] getInputs = [false, false];
        private readonly bool[] getInverseInputs = [false, false];
        private readonly GetLinkValueMethod?[] getLinkInputs = [null, null];

        public AND()
        {
        }

        [Category(" Общие"), DisplayName("Функция")]
        public string FuncName => "And";

        [Category(" Общие"), DisplayName("Имя")]
        public string? Name { get; set; }

        [Browsable(false), Category("Диагностика"), DisplayName("Показывать значения")]
        public bool VisibleValues { get; set; } = true;

        [Category("Входы"), DisplayName("Вход 1")]
        public bool Inp1 { get => getInputs[0]; set => getInputs[0] = value; }

        [Category("Входы"), DisplayName("Вход 2")]
        public bool Inp2 { get => getInputs[1]; set => getInputs[1] = value; }

        [Category("Выходы"), DisplayName("Выход")]
        public bool Out 
        { 
            get => @out;
            private set
            {
                if (@out == value) return;
                @out = value;
                ResultChanged?.Invoke(this, new ResultCalculateEventArgs(nameof(Out), value));
            }
        }

        [Category("Инверсия"), DisplayName("входа 1")]
        public bool InverseInp1 { get => getInverseInputs[0]; set => getInverseInputs[0] = value; }

        [Category("Инверсия"), DisplayName("входа 2")]
        public bool InverseInp2 { get => getInverseInputs[1]; set => getInverseInputs[1] = value; }
        [Category("Инверсия"), DisplayName("выхода")]
        public bool InverseOut { get; set; } = false;

        [Browsable(false)]
        public bool[] InverseInputs => [getInverseInputs[0], getInverseInputs[1]];

        [Browsable(false)]
        public bool[] InverseOutputs => [InverseOut];

        [Browsable(false)]
        public string[] InputNames => [string.Empty, string.Empty];

        [Browsable(false)]
        public string[] OutputNames => [string.Empty];

        [Browsable(false)]
        public object[] InputValues 
        { 
            get 
            {
                List<object> list = [];
                for (var i = 0; i < getInputs.Length; i++)
                    list.Add(getLinkInputs[i] ?? (object)getInputs[i]);
                return [.. list];
            }
        }
            
        [Browsable(false)]
        public object[] OutputValues => [Out];

        [Browsable(false)]
        public string FuncSymbol => "&";

        public event ResultCalculateEventHandler? ResultChanged;

        public void Calculate()
        {
            bool result = (bool)InputValues[0] ^ getInverseInputs[0];
            for (var i = 1; i < InputValues.Length; i++)
            {
                var input = (bool)InputValues[i] ^ getInverseInputs[i];
                result = result && input;
            }
            Out = result ^ InverseOut;
        }

        public GetLinkValueMethod? GetResultLink(int outputIndex)
        {
            return () => Out;
        }

        public void SetValueLinkToInp(int inputIndex, GetLinkValueMethod? getInp)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                getLinkInputs[inputIndex] = getInp;
            }
        }
    }
}
