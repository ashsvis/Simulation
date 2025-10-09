using Simulator.Model.Common;

namespace Simulator.Model.Interfaces
{
    public interface ILinkSupport
    {
        void UpdateInputLinkSources((Guid, int, bool) seek, Guid newId);
        Input[] Inputs { get; }
        Output[] Outputs { get; }
        void SetValueLinkToInp(int inputIndex, Guid sourceId, int outputPinIndex, bool byDialog);
        void ResetValueLinkToInp(int inputIndex);
        void CalculateTargets(PointF location, ref SizeF size,
             Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins,
             Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins);

        Guid ItemId { get; }
        void SetItemId(Guid itemId);
    }
}