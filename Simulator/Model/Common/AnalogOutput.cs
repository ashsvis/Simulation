namespace Simulator.Model.Common
{
    public class AnalogOutput : Output
    {
        public AnalogOutput(int index, string? name)
        {
            DirectKind = DirectKind.Output;
            ValueKind = ValueKind.Analog;
            Index = index;
            Name = name;
        }
    }
}
