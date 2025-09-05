namespace Simulator.Model.Logic
{
    public class AND : ICalculate
    {
        private bool @out;

        public bool Inp1 { get; set; }
        public bool Inp2 { get; set; }
        public bool Out 
        { 
            get => @out;
            set
            {
                if (@out == value) return;
                @out = value;
                ResultChanged?.Invoke(this, new ResultEventArgs(value, nameof(Out)));
            }
        }

        public event ResultEventHandler? ResultChanged;

        public void Calculate()
        {
            Out = Inp1 && Inp2;
        }
    }
}
