namespace Simulator.Model.Logic
{
    public class OR : ICalculate
    {
        public bool Inp1 { get; set; }
        public bool Inp2 { get; set; }
        public bool Out { get; set; }
        public void Calculate()
        {
            Out = Inp1 || Inp2;
        }
    }
}
