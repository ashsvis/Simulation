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

        public string? Name { get; set; }
        public bool S { get; set; } = false;
        public bool R { get; set; } = false;

        public void SetValueLinkToS(GetLinkValueMethod? getInp)
        {
            this.getS = getInp;
        }

        public void SetValueLinkToR(GetLinkValueMethod? getInp)
        {
            this.getR = getInp;
        }

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
