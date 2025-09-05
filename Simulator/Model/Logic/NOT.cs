namespace Simulator.Model.Logic
{
    public class NOT : ICalculate
    {
        private bool @out = true;
        private GetLinkValueMethod? getInp;

        public string? Name { get; set; }
        public bool Inp { get; set; } = false;

        public void SetValueLinkToInp(GetLinkValueMethod? getInp)
        {
            this.getInp = getInp;
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
            Out = getInp != null ? !(bool)getInp() : !Inp;
        }

        public GetLinkValueMethod? GetResultLink()
        {
            return () => Out;
        }
    }

}
