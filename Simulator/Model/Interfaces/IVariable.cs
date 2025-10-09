using Simulator.Model.Common;

namespace Simulator.Model.Interfaces
{
    public interface IVariable
    {
        ValueItem? ReadValue(Guid elementId, int pin, ValueDirect side, ValueKind kind);
        void WriteValue(Guid elementId, int pin, ValueDirect side, ValueKind kind, object? value);
    }
}