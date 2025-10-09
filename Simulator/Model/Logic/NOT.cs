using Simulator.Model.Common;
using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class NOT : CommonLogic
    {
        public NOT() : base(LogicFunction.Not, 1) 
        {
            ((DigitalOutput)Outputs[0]).Value = true;
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
            bool input = (bool)(GetInputValue(0) ?? false);
            SetValueToOut(0, !input);
        }

    }
}
