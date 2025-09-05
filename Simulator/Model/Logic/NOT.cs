using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class NOT : ICalculate
    {
        private bool @out = true;
        private GetLinkValueMethod? getInp;

        public NOT()
        {
        }

        [Category(" Общие"), DisplayName("Функция")]
        public string Function => "НЕ";

        [Category(" Общие"), DisplayName("Имя")]
        public string? Name { get; set; }

        [Category("Входы"), DisplayName("Вход")]
        public bool Inp { get; set; } = false;

        public void SetValueLinkToInp(GetLinkValueMethod? getInp)
        {
            this.getInp = getInp;
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

        [Browsable(false)]
        public bool[] Inputs => [false];

        [Browsable(false)]
        public bool[] Outputs => [true];

        public event ResultCalculateEventHandler? ResultChanged;

        public void Calculate()
        {
            Out = getInp != null ? !(bool)getInp() : !Inp;
        }

        public GetLinkValueMethod? GetResultLink()
        {
            return () => Out;
        }
    }

}
