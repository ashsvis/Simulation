using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Mathematic;

namespace Simulator.Model.Compare
{
    public class EQ : CommonAnalog, IManualChange
    {
        public EQ() : base(LogicFunction.Eq, 2)
        {
            Outputs[0] = new DigitalOutput(ItemId, 0);
            SetValueToOut(0, false);
            Inputs[1].Name = "SP";
        }

        public override void SetItemId(Guid id)
        {
            itemId = id;
            for (var i = 0; i < Inputs.Length; i++)
            {
                Inputs[i].ItemId = itemId;
                SetValueToInp(i, 0.0);
            }
            Outputs[0].ItemId = itemId;
            SetValueToOut(0, false);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var sp = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, Math.Abs(a - sp) < 0.0001);
        }
    }
    public class LT : CommonAnalog, IManualChange
    {
        public LT() : base(LogicFunction.Lt, 2)
        {
            Outputs[0] = new DigitalOutput(ItemId, 0);
            SetValueToOut(0, false);
            Inputs[1].Name = "SP";
        }

        public override void SetItemId(Guid id)
        {
            itemId = id;
            for (var i = 0; i < Inputs.Length; i++)
            {
                Inputs[i].ItemId = itemId;
                SetValueToInp(i, 0.0);
            }
            Outputs[0].ItemId = itemId;
            SetValueToOut(0, false);
        }

        public override void Calculate()
        {
            var a = (double)(GetInputValue(0) ?? double.NaN);
            var sp = (double)(GetInputValue(1) ?? double.NaN);
            SetValueToOut(0, a < sp);
        }
    }
}
