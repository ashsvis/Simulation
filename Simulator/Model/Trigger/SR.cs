using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Trigger
{
    public class SR : CommonLogic, IEmbededMemory
    {
        public SR() : base(LogicFunction.Sr, 2) 
        {
            Inputs[0].Name = "S";
            Inputs[1].Name = "R";
            Outputs[0].Name = "Q";
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
            var s = (bool)(GetInputValue(0) ?? false);
            var r = (bool)(GetInputValue(1) ?? false);
            if (s)
                SetValueToOut(0, true);
            else if (r)
                SetValueToOut(0, false);
        }

        public new void Init()
        {
            SetValueToOut(0, false);
        }
    }
}
