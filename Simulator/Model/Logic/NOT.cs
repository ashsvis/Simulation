namespace Simulator.Model.Logic
{
    public class NOT : ICalculate
    {
        private bool @out = true;

        public bool Inp { get; set; }
        public bool Out 
        {
            get => @out;
            private set
            {
                if (@out == value) return;
                @out = value;
                ResultChanged?.Invoke(this, new ResultEventArgs(nameof(Out), value));
            }
        }

        public event ResultEventHandler? ResultChanged;

        public void Calculate()
        {
            Out = !Inp;
        }
    }
}
