using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Trigger
{
    public class SR : CommonLogic
    {
        public SR() : base(LogicFunction.Sr, 2) 
        {
            InputNames[0] = "S";
            InputNames[1] = "R";
            OutputNames[0] = "Q";
        }

        [Browsable(false)]
        public override string FuncSymbol
        {
            get
            {
                return "SR";
            }
        }

        public override void Calculate()
        {
            InverseInputs[0] = false;
            InverseInputs[1] = false;
            InverseOutputs[0] = false;
            var s = (bool)InputValues[0];
            var r = (bool)InputValues[1];
            if (s)
                Out = true;
            else if (r)
                Out = false;
            Project.WriteBoolValue(ItemId, 0, Out);
        }
    }
}
