namespace Simulator.Model.Common
{
    public class LinkSource(Guid id, int pinIndex, bool external)
    {
        public Guid Id { get; } = id;
        public int PinIndex { get; } = pinIndex;
        public bool External { get; } = external;
    }
}
