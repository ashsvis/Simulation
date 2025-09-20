namespace Simulator.Model
{
    public interface ILinkSupport
    {
        bool[] LinkedInputs { get; }
        object[] LinkedOutputs { get; }
        (Guid, int)[] InputLinkSources { get; }
        void UpdateInputLinkSources((Guid, int) seek, Guid newId);
        object[] InputValues { get; }
        object[] OutputValues { get; }
        GetLinkValueMethod? GetResultLink(int outputIndex);
        void SetValueLinkToInp(int inputIndex, GetLinkValueMethod? getMethod, Guid sourceId, int outputPinIndex);
        void ResetValueLinkToInp(int inputIndex);
        void CalculateTargets(PointF location, ref SizeF size,
             Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins,
             Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins);

        Guid ItemId { get; }
        void SetItemId(Guid itemId);
    }

    public delegate object GetLinkValueMethod();
}