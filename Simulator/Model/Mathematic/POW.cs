using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class POW : CommonAnalog, IManualChange
    {
        public POW() : base(LogicFunction.Pow, 2)
        {
            SetValueToOut(0, 0.0);
            Inputs[0].Name = "X";
            Inputs[1].Name = "Y";
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, Math.Pow(a, b));
        }
    }
}
