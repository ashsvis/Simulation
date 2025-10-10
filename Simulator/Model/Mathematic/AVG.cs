using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class AVG : CommonAnalog, IManualChange
    {
        public AVG() : base(LogicFunction.Avg, 2)
        {
            SetValueToOut(0, 0.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, new double[] { a, b }.Average());
        }
    }
}
