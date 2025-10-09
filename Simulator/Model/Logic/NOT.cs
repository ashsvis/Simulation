using System.ComponentModel;

namespace Simulator.Model.Logic
{
    public class NOT : CommonLogic
    {
        public NOT() : base(LogicFunction.Not, 1) 
        {
            SetValueToOut(0, true);
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
