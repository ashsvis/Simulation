using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Logic;

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
    }

    public class ADD : CommonAnalog, IManualChange
    {
        public ADD() : base(LogicFunction.Add, 2) 
        {
            SetValueToOut(0, 0.0);
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

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a + b);
        }

        //public new void SetValueToInp(int inputIndex, object? value)
        //{
        //    if (inputIndex < 0 || inputIndex >= Inputs.Length) return;
        //    SetValueToInp(inputIndex, value);
        //}
    }
}
