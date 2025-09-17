namespace Simulator.Model
{
    public interface IManualChange
    {
        void SetValueToInp(int inputIndex, object? value);
        object? GetValueFromInp(int inputIndex);
    }
}