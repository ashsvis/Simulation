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

        public string? Name { get; set; }
        public bool Inp1 { get; set; } = false;
        public bool Inp2 { get; set; } = false;

        public void SetValueLinkToInp1(GetLinkValueMethod? getInp)
        {
            this.getInp1 = getInp;
        }

        public void SetValueLinkToInp2(GetLinkValueMethod? getInp)
        {
            this.getInp2 = getInp;
        }

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
