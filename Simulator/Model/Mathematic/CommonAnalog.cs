using Simulator.Model.Common;
using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Mathematic
{
    public class CommonAnalog : CommonLogic
    {
        public CommonAnalog(LogicFunction func, int inputCount, int outputCount = 1) : base(func, inputCount, outputCount)
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Inputs[i] = new AnalogInput(itemId, i);
            }
            for (var i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = new AnalogOutput(itemId, i);
            }
        }

        [Browsable(false)]
        public override string FuncSymbol
        {
            get
            {
                return logicFunction switch
                {
                    LogicFunction.Add => "ADD",
                    LogicFunction.Sub => "SUB",
                    LogicFunction.Mul => "MUL",
                    LogicFunction.Div => "DIV",
                    LogicFunction.Mod => "MOD",
                    LogicFunction.Abs => "ABS",
                    LogicFunction.Neg => "NEG",
                    LogicFunction.Min => "MIN",
                    LogicFunction.Max => "MAX",
                    LogicFunction.Avg => "AVG",
                    LogicFunction.Rond => "ROND",
                    LogicFunction.Trnc => "TRNC",
                    LogicFunction.Sqrt => "SQRT",
                    LogicFunction.Sbrt => "SBRT",
                    LogicFunction.Pow => "POW",
                    LogicFunction.Exp => "EXP",
                    LogicFunction.Ln => "LN",
                    LogicFunction.Log => "LOG",
                    LogicFunction.Lmt => "LMT",
                    _ => "",
                };
            }
        }

        public override void SetItemId(Guid id)
        {
            itemId = id;
            for (var i = 0; i < Inputs.Length; i++)
            {
                Inputs[i].ItemId = itemId;
                SetValueToInp(i, 0.0);
            }
            for (var i = 0; i < Outputs.Length; i++)
            {
                Outputs[i].ItemId = itemId;
                SetValueToOut(i, 0.0);
            }
        }
    }
}
