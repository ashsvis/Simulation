namespace Simulator.Model.Trigger
{
    public class RS : ICalculate
    {
        public bool S { get; set; }
        public bool R { get; set; }
        public bool Q { get; set; }
        public void Calculate()
        {
            Q = !R && (S || Q);
            R = false;
            S = false;
        }
    }
}
