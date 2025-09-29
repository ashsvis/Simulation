using Simulator.Model.Interfaces;
using System.ComponentModel;

namespace Simulator.Model
{
    public class ProjectProxy : IVariable
    {
        public ValueItem? ReadValue(Guid elementId, int pin, ValueSide side, ValueKind kind)
        {
            return Project.ReadValue(elementId, pin, side, kind);
        }

        public void WriteValue(Guid elementId, int pin, ValueSide side, ValueKind kind, object? value)
        {
            Project.WriteValue(elementId, pin, side, kind, value);
        }
    }
}
