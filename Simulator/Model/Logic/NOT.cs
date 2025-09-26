using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class NOT : CommonLogic
    {
        public NOT() : base(LogicFunction.Not, 1) 
        {
            InverseInputs[0] = false;
            InverseOutputs[0] = true;
            Out = true;
        }

        [Browsable(false)]
        public override string FuncSymbol
        {
            get
            {
                return "1";
            }
        }

        public override void Calculate()
        {
            //InverseInputs[0] = false;
            //InverseOutputs[0] = true;
            //bool input = (bool)InputValues[0];
            ValueItem? val = varManager?.ReadValue(ItemId, 0, ValueSide.Input, ValueKind.Digital);
            bool input = (bool)(val?.Value ?? false);
            var changed = Out != !input;
            Out = !input;
            //if (changed)
            varManager?.WriteValue(ItemId, 0, ValueSide.Output, ValueKind.Digital, Out);
        }

    }
}
