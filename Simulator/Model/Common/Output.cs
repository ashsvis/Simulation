namespace Simulator.Model.Common
{
    public abstract class Output
    {
        public string? Name { get; set; }
        public int Index { get; set; }
        public DirectKind DirectKind { get; set; }
        public ValueKind ValueKind { get; set; }
        public LinkSource[] LinkTargets { get; set; } = [];
    }
}
