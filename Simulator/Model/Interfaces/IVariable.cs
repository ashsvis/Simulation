namespace Simulator.Model.Interfaces
{
    public interface IVariable
    {
        void Clear();
        int CountVariables(Guid moduleId);
        ValueItem? GetVariableByIndex(Guid moduleId, int itemIndex);
        ValueItem? ReadValue(Guid elementId, int pin, ValueSide side, ValueKind kind);
        void WriteValue(Guid elementId, int pin, ValueSide side, ValueKind kind, object? value);
    }
}