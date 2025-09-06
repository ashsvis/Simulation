using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class OR : ICalculate
    {
        private bool @out = false;
        private GetLinkValueMethod? getInp1;
        private GetLinkValueMethod? getInp2;

        public OR()
        {
        }

        [Category(" Общие"), DisplayName("Функция")]
        public string FuncName => "Or";

        [Category(" Общие"), DisplayName("Имя")]
        public string? Name { get; set; }

        [Browsable(false), Category("Диагностика"), DisplayName("Показывать значения")]
        public bool VisibleValues { get; set; } = true;

        [Category("Входы"), DisplayName("Вход 1")]
        public bool Inp1 { get; set; } = false;

        [Category("Входы"), DisplayName("Вход 2")]
        public bool Inp2 { get; set; } = false;

        public void SetValueLinkToInp1(GetLinkValueMethod? getInp)
        {
            this.getInp1 = getInp;
        }
        
        public void SetValueLinkToInp2(GetLinkValueMethod? getInp)
        {
            this.getInp2 = getInp;
        }

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
        public bool InverseInp1 { get; set; } = false;

        [Category("Инверсия"), DisplayName("входа 2")]
        public bool InverseInp2 { get; set; } = false;
        [Category("Инверсия"), DisplayName("выхода")]
        public bool InverseOut { get; set; } = false;

        [Browsable(false)]
        public bool[] InverseInputs => [InverseInp1, InverseInp2];
        [Browsable(false)]
        public bool[] InverseOutputs => [InverseOut];

        [Browsable(false)]
        public string[] InputNames => [string.Empty, string.Empty];

        [Browsable(false)]
        public string[] OutputNames => [string.Empty];

        [Browsable(false)]
        public object[] InputValues =>
            [
                getInp1 != null ? (bool)getInp1() : Inp1,
                getInp2 != null ? (bool)getInp2() : Inp2
            ];

        [Browsable(false)]
        public object[] OutputValues => [Out];

        [Browsable(false)]
        public string FuncSymbol => "1";

        public event ResultCalculateEventHandler? ResultChanged;

        public void Calculate()
        {
            var inp1 = getInp1 != null ? (bool)getInp1() : Inp1;
            var inp2 = getInp2 != null ? (bool)getInp2() : Inp2;
            Out = ((inp1 ^ InverseInp1) || (inp2 ^ InverseInp2)) ^ InverseOut;
        }

        public GetLinkValueMethod? GetResultLink()
        {
            return () => Out;
        }
    }
}
