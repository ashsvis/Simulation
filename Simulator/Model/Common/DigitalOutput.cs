namespace Simulator.Model.Common
{
    public class DigitalOutput : Output
    {
        public DigitalOutput(int index, string? name)
        {
            DirectKind = DirectKind.Output;
            ValueKind = ValueKind.Digital;
            Index = index;
            Name = name;
        }
    }
}
