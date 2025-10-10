using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class LML : CommonAnalog, IManualChange
    {
        public LML() : base(LogicFunction.LmL, 2)
        {
            SetValueToOut(0, 0.0);
            Inputs[1].Name = "Min";
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var min = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a < min ? min : a);
        }
    }
}
