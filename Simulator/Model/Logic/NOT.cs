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
            bool input = (bool)GetInputValue(0);
            Out = !input;
            Project.WriteValue(ItemId, 0, ValueSide.Output, ValueKind.Digital, Out);
        }

    }
}
