using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using Simulator.Model.Mathematic;

namespace Simulator.Model.Selector
{
    public class SELA : CommonAnalog, IManualChange
    {
        public SELA() : base(LogicFunction.SelA, 3)
        {
            Inputs[0] = new DigitalInput(ItemId, 0);
            SetValueToOut(0, 0.0);
            Inputs[0].Name = "A/B";
            Inputs[1].Name = "A";
            Inputs[2].Name = "B";
        }

        public override void Init()
        {
            base.Init();
            SetValueToInp(0, false);
            SetValueToInp(1, 0.0);
            SetValueToInp(2, 0.0);
        }

        public override void SetItemId(Guid id)
        {
            itemId = id;
            Inputs[0].ItemId = itemId;
            SetValueToInp(0, false);
            for (var i = 1; i < Inputs.Length; i++)
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
            var sel = (bool)(GetInputValue(0) ?? false);
            var a = (double)(GetInputValue(1) ?? double.NaN);
            var b = (double)(GetInputValue(2) ?? double.NaN);
            SetValueToOut(0, sel ? b : a);
        }
    }
}
