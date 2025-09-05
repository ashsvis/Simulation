namespace Simulator.Model.Trigger
{
    public class RS : ICalculate
    {
        private bool q;

        public bool S { get; set; }
        public bool R { get; set; }
        public bool Q 
        { 
            get => q;
            set
            {
                if (q == value) return;
                q = value;
                ResultChanged?.Invoke(this, new ResultEventArgs(value, nameof(Q)));
            }
        }

        public event ResultEventHandler? ResultChanged;

        public void Calculate()
        {
            Q = !R && (S || Q);
            R = false;
            S = false;
        }
    }
}
