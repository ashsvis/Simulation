using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class MUL : CommonAnalog, IManualChange
    {
        public MUL() : base(LogicFunction.Mul, 2)
        {
            Inputs[1].Name = "*";
            SetValueToOut(0, 0.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a * b);
        }
    }
}
