namespace Simulator.Model.Common
{
    public abstract class Input(Guid parentId)
    {
        public string? Name { get; set; }
        public int Index { get; set; }
        public ValueDirect ValueSide { get; set; }
        public ValueKind ValueKind { get; set; }
        public LinkSource? LinkSource { get; set; }
        public Guid ItemId { get; set; } = parentId;
    }
}
