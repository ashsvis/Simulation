namespace Simulator.Model.Interfaces
{
    public interface IManualCommand
    {
        void SetValueToOut(int outputIndex, object? value);
        object? GetValueFromOut(int outputIndex);
    }
}