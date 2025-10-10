using Simulator.Model.Common;
using Simulator.Model.Logic;

namespace Simulator.Model.Interfaces
{
    public interface IFunction
    {
        string? Name { get; set; }
        LogicFunction FuncName { get; }
        Input[] Inputs { get; }
        Output[] Outputs { get; }
    }

    public delegate void ResultCalculateEventHandler(object sender, ResultCalculateEventArgs args);

    public class ResultCalculateEventArgs(string propname, object value) : EventArgs
    {
        public string? Propname { get; set; } = propname;
        public object? Result { get; set; } = value;

    }

    public delegate void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected);
}