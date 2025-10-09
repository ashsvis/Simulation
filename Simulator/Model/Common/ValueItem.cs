namespace Simulator.Model.Common
{
    public class ValueItem
    {
        public ValueKind Kind { get; set; }
        public ValueDirect Side { get; set; }
        public object? Value { get; set; }
        public int Pin { get; set; }
        public Guid ElementId { get; set; }

        public override string ToString()
        {
            return Kind switch
            {
                ValueKind.Analog => $"{Value}",
                ValueKind.Digital => $"{Value ?? false}"[..1].ToUpper(),
                _ => string.Empty,
            };
        }
    }
}
