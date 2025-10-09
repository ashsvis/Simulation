namespace Simulator.Model.Common
{
    public class DigitalOutput : Output
    {
        public DigitalOutput(Guid itemId, int index, bool inverse = false, string? name = null) : base(itemId)
        {
            ValueSide = ValueDirect.Output;
            ValueKind = ValueKind.Digital;
            Index = index;
            Name = name;
            Inverse = inverse;
        }

        public bool Value 
        {
            get => (bool)(Project.ReadValue(ItemId, Index, ValueDirect.Output, ValueKind.Digital)?.Value ?? false);
            set => Project.WriteValue(ItemId, Index, ValueDirect.Output, ValueKind.Digital, value);
        }

        public bool Inverse { get; set; }
    }
}
