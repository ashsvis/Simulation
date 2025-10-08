namespace Simulator.Model.Interfaces
{
    public interface IManualChange
    {
        void SetValueToInp(int inputIndex, object? value);
        object? GetValueFromInp(int inputIndex);
        void Init();
        object GetInputValue(int pin);
    }
}