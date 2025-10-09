namespace Simulator.Model.Common
{
    public class AnalogOutput : Output
    {
        public AnalogOutput(Guid itemId, int index, string? name = null) : base(itemId)
        {
            ValueSide = ValueDirect.Output;
            ValueKind = ValueKind.Analog;
            Index = index;
            Name = name;
        }

        public double Value 
        {
            get => (double)(Project.ReadValue(ItemId, Index, ValueDirect.Output, ValueKind.Analog)?.Value ?? 0.0);
            set => Project.WriteValue(ItemId, Index, ValueDirect.Output, ValueKind.Analog, value);
        }
    }
}
