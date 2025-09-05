
namespace Simulator.Model.Logic
{
    public class NOT : ICalculate
    {
        private bool @out;

        public bool Inp { get; set; }
        public bool Out 
        {
            get => @out;
            set
            {
                if (@out != value) return;
                @out = value;
                OutputChanged?.Invoke(this, new ResultEventArgs(value));
            }
        }

        public event ResultEventHandler? OutputChanged;

        public void Calculate()
        {
            Out = !Inp;
        }
    }
}
