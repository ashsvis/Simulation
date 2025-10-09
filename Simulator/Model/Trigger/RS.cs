using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Trigger
{
    public class RS : CommonLogic, IEmbededMemory
    {
        public RS() : base(LogicFunction.Rs, 2) 
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
                return "RS";
            }
        }

        public override void Calculate()
        {
            InverseInputs[0] = false;
            InverseInputs[1] = false;
            InverseOutputs[0] = false;
            var s = (bool)InputValues[0];
            var r = (bool)InputValues[1];
            if (r)
                Out = false;
            else if (s)
                Out = true;
            Project.WriteValue(ItemId, 0, ValueDirect.Output, ValueKind.Digital, Out);
        }

        public new void Init()
        {
            Out = false;
            Project.WriteValue(ItemId, 0, ValueDirect.Output, ValueKind.Digital, Out);
        }
    }
}
