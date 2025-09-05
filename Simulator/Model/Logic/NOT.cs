namespace Simulator.Model.Logic
{
    public class NOT : ICalculate
    {
        public bool Inp { get; set; }
        public bool Out { get; set; }
        public void Calculate()
        {
            Out = !Inp;
        }
    }
}
