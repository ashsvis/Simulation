namespace Simulator.Model
{
    public interface IManualCommand
    {
        void SetValueToOut(int outputIndex, object? value);
        object? GetValueFromOut(int outputIndex);
    }
}