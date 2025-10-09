namespace Simulator.Model.Common
{
    public class DigitalInput : Input
    {
        public DigitalInput(int index, string? name) 
        {
            DirectKind = DirectKind.Input;
            ValueKind = ValueKind.Digital;
            Index = index;
            Name = name;
        }
    }
}
