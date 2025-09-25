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
            InverseInputs[0] = false;
            InverseOutputs[0] = true;
            bool input = (bool)InputValues[0];
            Out = !input;
            Project.WriteBoolValue($"{ItemId}\t{0}", Out);
        }

    }
}
