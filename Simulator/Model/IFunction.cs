using Simulator.Model.Logic;

namespace Simulator.Model
{
    public interface IFunction
    {
        string? Name { get; set; }
        LogicFunction FuncName { get; }
        string FuncSymbol { get; }
        bool[] InverseInputs { get; }
        bool[] InverseOutputs { get; }
        string[] InputNames { get; }
        string[] OutputNames { get; }
        object[] InputValues { get; }
        object[] OutputValues { get; }
        bool VisibleValues { get; set; }

        event ResultCalculateEventHandler? ResultChanged;
    }

    public delegate void ResultCalculateEventHandler(object sender, ResultCalculateEventArgs args);

    public class ResultCalculateEventArgs(string propname, object value) : EventArgs
    {
        public string? Propname { get; set; } = propname;
        public object? Result { get; set; } = value;

    }

    public delegate void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush);
}