namespace Simulator.Model.Common
{
    public class DigitalInput : Input
    {
        public DigitalInput(Guid itemId, int index, bool inverse = false, string? name = null) : base(itemId)
        {
            ValueSide = ValueDirect.Input;
            ValueKind = ValueKind.Digital;
            Index = index;
            Name = name;
            Inverse = inverse;
        }

        public bool Value 
        {
            get
            {
                if (LinkSource == null)
                    return (bool)(Project.ReadValue(ItemId, Index, ValueDirect.Input, ValueKind.Digital)?.Value ?? false);
                return (bool)(Project.ReadValue(LinkSource.Id, LinkSource.PinIndex, ValueDirect.Output, ValueKind.Digital)?.Value ?? false);
            }
            set
            {
                if (LinkSource == null)
                    Project.WriteValue(ItemId, Index, ValueDirect.Input, ValueKind.Digital, value);
            }
        }

        public bool Inverse { get; set; }
    }
}
