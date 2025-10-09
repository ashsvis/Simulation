namespace Simulator.Model.Common
{
    public class AnalogInput : Input
    {
        public AnalogInput(Guid itemId, int index, string? name = null) : base(itemId)
        {
            ValueSide = ValueDirect.Input;
            ValueKind = ValueKind.Analog;
            Index = index;
            Name = name;
        }

        public double Value 
        {
            get
            {
                if (LinkSource == null)
                    return (double)(Project.ReadValue(ItemId, Index, ValueDirect.Input, ValueKind.Analog)?.Value ?? 0.0);
                return (double)(Project.ReadValue(LinkSource.Id, LinkSource.PinIndex, ValueDirect.Output, ValueKind.Analog)?.Value ?? 0.0);
            }
            set
            {
                if (LinkSource == null)
                    Project.WriteValue(ItemId, Index, ValueDirect.Input, ValueKind.Analog, value);
            }
        }
    }
}
