namespace Simulator.Model
{
    public interface ICalculate
    {
        void Calculate();
        event ResultEventHandler? OutputChanged; 
    }

    public delegate void ResultEventHandler(object sender, ResultEventArgs args);

    public class ResultEventArgs : EventArgs
    {
        public ResultEventArgs(object value)
        {
            Result = value;
        }

        public object? Result { get; set; }

    }
}