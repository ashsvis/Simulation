namespace Simulator.Model.Interfaces
{
    public interface IVariable
    {
        ValueItem? ReadValue(Guid elementId, int pin, ValueSide side, ValueKind kind);
        void WriteValue(Guid elementId, int pin, ValueSide side, ValueKind kind, object? value);
    }
}