using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Logic;

namespace Simulator.Model.Selector
{
    public class SELD : CommonLogic, IManualChange
    {
        public SELD() : base(LogicFunction.SelD, 3)
        {
            SetValueToOut(0, false);
            Inputs[0].Name = "A/B";
            Inputs[1].Name = "A";
            Inputs[2].Name = "B";
        }

        public override void Calculate()
        {
            var sel = (bool)(GetInputValue(0) ?? false);
            var a = (bool)(GetInputValue(1) ?? false);
            var b = (bool)(GetInputValue(2) ?? false);
            SetValueToOut(0, sel ? b : a);
        }
    }
}
