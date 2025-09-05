namespace Simulator.Model
{
    public interface ICalculate
    {
        string? Name { get; set; }
        string Function { get; }
        bool[] Inputs { get; }
        bool[] Outputs { get; }
        void Calculate();
        GetLinkValueMethod? GetResultLink();
        event ResultCalculateEventHandler? ResultChanged; 
    }

    public delegate object GetLinkValueMethod();
    public delegate void ResultCalculateEventHandler(object sender, ResultCalculateEventArgs args);

    public class ResultCalculateEventArgs(string propname, object value) : EventArgs
    {
        public string? Propname { get; set; } = propname;
        public object? Result { get; set; } = value;

    }
}