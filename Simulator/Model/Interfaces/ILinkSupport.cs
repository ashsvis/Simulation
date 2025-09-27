namespace Simulator.Model.Interfaces
{
    public interface ILinkSupport
    {
        bool[] LinkedInputs { get; }
        object[] LinkedOutputs { get; }
        (Guid, int, bool)[] InputLinkSources { get; }
        void UpdateInputLinkSources((Guid, int, bool) seek, Guid newId);
        object[] InputValues { get; }
        object[] OutputValues { get; }
        void SetValueLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog);
        void ResetValueLinkToInp(int inputIndex);
        void CalculateTargets(PointF location, ref SizeF size,
             Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins,
             Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins);

        Guid ItemId { get; }
        void SetItemId(Guid itemId);

        IVariable VarManager { get; }
        void SetVarManager(IVariable varManager);
    }
}