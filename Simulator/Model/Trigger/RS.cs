using System.ComponentModel;

namespace Simulator.Model.Trigger
{
    public class RS : ICalculate
    {
        private bool q = false;
        private GetLinkValueMethod? getS;
        private GetLinkValueMethod? getR;

        public RS()
        {
        }

        [Category(" Общие"), DisplayName("Функция")]
        public string Function => "RS-триггер";

        [Category(" Общие"), DisplayName("Имя")]
        public string? Name { get; set; }

        [Category("Входы"), DisplayName("Уст-ка (S)")]
        public bool S { get; set; } = false;

        [Category("Входы"), DisplayName("Сброс (R)")]
        public bool R { get; set; } = false;

        public void SetValueLinkToS(GetLinkValueMethod? getInp)
        {
            this.getS = getInp;
        }

        public void SetValueLinkToR(GetLinkValueMethod? getInp)
        {
            this.getR = getInp;
        }

        [Category("Выходы"), DisplayName("Ячейка (Q)")]
        public bool Q 
        { 
            get => q;
            private set
            {
                if (q == value) return;
                q = value;
                ResultChanged?.Invoke(this, new ResultCalculateEventArgs(nameof(Q), value));
            }
        }

        [Browsable(false)]
        public bool[] Inputs => [false, false];

        [Browsable(false)]
        public bool[] Outputs => [false];

        public event ResultCalculateEventHandler? ResultChanged;

        public void Calculate()
        {
            var r = getR != null ? (bool)getR() : R;
            var s = getS != null ? (bool)getS() : S;

            if (r || s)
            {
                Q = !r && (s || Q);
                R = false;
                S = false;
            }
        }

        public GetLinkValueMethod? GetResultLink()
        {
            return () => Q;
        }
    }
}
