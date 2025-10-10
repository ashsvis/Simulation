using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class LMT : CommonAnalog, IManualChange
    {
        public LMT() : base(LogicFunction.Lmt, 3)
        {
            SetValueToOut(0, 0.0);
            Inputs[1].Name = "Min";
            Inputs[2].Name = "Max";
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var min = (double)(GetInputValue(1) ?? double.NaN);
            var max = (double)(GetInputValue(2) ?? double.NaN);
            SetValueToOut(0, min > max ? double.NaN : a < min ? min : a > max ? max : a);
        }
    }
}
