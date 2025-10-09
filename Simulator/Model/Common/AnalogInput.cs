namespace Simulator.Model.Common
{
    public class AnalogInput : Input
    {
        public AnalogInput(int index, string? name)
        {
            DirectKind = DirectKind.Input;
            ValueKind = ValueKind.Analog;
            Index = index;
            Name = name;
        }
    }
}
