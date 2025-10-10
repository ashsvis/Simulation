using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class SUB : CommonAnalog, IManualChange
    {
        public SUB() : base(LogicFunction.Sub, 2)
        {
            SetValueToOut(0, 0.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a - b);
        }
    }
}
