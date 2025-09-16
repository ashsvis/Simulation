namespace Simulator.Model
{
    public interface ILink
    {
        bool[] LinkedInputs { get; }
        object[] LinkedOutputs { get; }
        (Guid, int)[] InputLinkSources { get; }
        GetLinkValueMethod? GetResultLink(int outputIndex);
        void SetValueToInp(int inputIndex, object? value);
        void SetValueLinkToInp(int inputIndex, GetLinkValueMethod? getMethod, Guid sourceId, int outputPinIndex);
        void ResetValueLinkToInp(int inputIndex);
        void CalculateTargets(PointF location, ref SizeF size,
             Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins,
             Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins);
    }

    public delegate object GetLinkValueMethod();
}