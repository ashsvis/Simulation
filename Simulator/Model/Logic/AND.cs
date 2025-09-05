using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class AND : ICalculate
    {
        private bool @out = false;
        private GetLinkValueMethod? getInp1;
        private GetLinkValueMethod? getInp2;

        public AND()
        {
        }

        [Category("1.Общие"), DisplayName("Функция")]
        public string Function => "И";

        [Category("1.Общие"), DisplayName("Имя")]
        public string? Name { get; set; }

        [Category("2.Входы"), DisplayName("Вход 1")]
        public bool Inp1 { get; set; } = false;

        [Category("2.Входы"), DisplayName("Вход 2")]
        public bool Inp2 { get; set; } = false;

        public void SetValueLinkToInp1(GetLinkValueMethod? getInp)
        {
            this.getInp1 = getInp;
        }

        public void SetValueLinkToInp2(GetLinkValueMethod? getInp)
        {
            this.getInp2 = getInp;
        }

        [Category("3.Выходы"), DisplayName("Выход")]
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

        public event ResultCalculateEventHandler? ResultChanged;

        public void Calculate()
        {
            Out = (getInp1 != null ? (bool)getInp1() : Inp1) && (getInp2 != null ? (bool)getInp2() : Inp2);
        }

        public GetLinkValueMethod? GetResultLink()
        {
            return () => Out;
        }
    }
}
