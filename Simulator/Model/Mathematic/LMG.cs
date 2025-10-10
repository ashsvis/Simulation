using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class LMG : CommonAnalog, IManualChange
    {
        public LMG() : base(LogicFunction.LmG, 2)
        {
            SetValueToOut(0, 0.0);
            Inputs[1].Name = "Max";
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var max = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a > max ? max : a);
        }
    }
}
