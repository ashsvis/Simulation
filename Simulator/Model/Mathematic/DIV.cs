using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Mathematic
{
    public class DIV : CommonAnalog, IManualChange
    {
        public DIV() : base(LogicFunction.Div, 2)
        {
            Inputs[1].Name = "/";
            SetValueToInp(1, 1.0);
            SetValueToOut(0, 0.0);
        }

        public override void Init()
        {
            base.Init();
            SetValueToInp(1, 1.0);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a / b);
        }
    }
}
