namespace Simulator.Model
{
    public interface ILink
    {
        bool[] LinkedInputs { get; }
        (Guid, int)[] InputLinkSources { get; }
        GetLinkValueMethod? GetResultLink(int outputIndex);
        void SetValueToInp(int inputIndex, object? value);
        void SetValueLinkToInp(int inputIndex, GetLinkValueMethod? getMethod, Guid sourceId, int outputPinIndex);
        void ResetValueLinkToInp(int inputIndex);
    }

    public delegate object GetLinkValueMethod();
}