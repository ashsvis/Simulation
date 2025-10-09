using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Mathematic
{
    public class ADD : CommonLogic, IManualChange
    {
        public ADD() : base(LogicFunction.Add, 2) 
        {
            SetValueToOut(0, 0.0);
        }

        public override void Init()
        {
            if (itemId == Guid.Empty) return;
            //for (var i = 0; i < getLinkSources.Length; i++)
            //{
            //    (Guid sourceId, _, _) = getLinkSources[i];
            //    if (sourceId == Guid.Empty)
            //        Project.WriteValue(itemId, i, ValueSide.Input, ValueKind.Analog, getInputs[i]);
            //}
        }

        //[Category("Входы"), DisplayName("Управление")]
        //public new double[] Inputs => getInputs;

        //public  object Out
        //{
        //    get => getOutputs.Length > 0 ? (double)(getOutputs[0] ?? 0.0) : 0.0;
        //    protected set
        //    {
        //        if (getOutputs.Length == 0) return;
        //        getOutputs[0] = value;
        //        OnResultChanged();
        //    }
        //}

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var b = (double)(GetInputValue(1) ?? double.NaN);
            ((AnalogOutput)Outputs[0]).Value = a + b;

        }

        public new void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex < 0 || inputIndex >= Inputs.Length) return;
            SetValueToInp(inputIndex, value);
        }
    }
}
