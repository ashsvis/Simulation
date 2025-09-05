namespace Simulator.Model
{
    public interface ICalculate
    {
        void Calculate();
        event ResultEventHandler? ResultChanged; 
    }

    public delegate void ResultEventHandler(object sender, ResultEventArgs args);

    public class ResultEventArgs : EventArgs
    {
        public ResultEventArgs(object value, string propname)
        {
            Result = value;
            Propname = propname;
        }

        public object? Result { get; set; }
        public string? Propname { get; set; } 

    }
}