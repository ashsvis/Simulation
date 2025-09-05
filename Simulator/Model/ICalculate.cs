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
        public ResultEventArgs(string propname, object value)
        {
            Result = value;
            Propname = propname;
        }

        public string? Propname { get; set; } 
        public object? Result { get; set; }

    }
}