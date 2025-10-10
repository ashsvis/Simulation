using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class LOG : CommonAnalog, IManualChange
    {
        public LOG() : base(LogicFunction.Log, 1)
        {
            SetValueToOut(0, 0.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            SetValueToOut(0, Math.Log10(a));
        }
    }
}
