using Simulator.Model.Interfaces;
using Simulator.Model.Logic;
using System.ComponentModel;

namespace Simulator.Model.Mathematic
{
    public class ADD : CommonLogic
    {
        private readonly double[] getInputs;

        public ADD() : base(LogicFunction.Add, 2) 
        {
            getInputs = [];
            getInputs = new double[2];
            for (var i = 0; i < getOutputs.Length; i++) getOutputs[i] = 0.0;
        }

        public override void Init()
        {
            if (itemId == Guid.Empty) return;
            for (var i = 0; i < getLinkSources.Length; i++)
            {
                (Guid sourceId, _, _) = getLinkSources[i];
                if (sourceId == Guid.Empty)
                    Project.WriteValue(itemId, i, ValueSide.Input, ValueKind.Analog, getInputs[i]);
            }
        }

        [Category("Входы"), DisplayName("Управление")]
        public new double[] Inputs => getInputs;

        public override object Out
        {
            get => getOutputs.Length > 0 ? (double)(getOutputs[0] ?? 0.0) : 0.0;
            protected set
            {
                if (getOutputs.Length == 0) return;
                getOutputs[0] = value;
                OnResultChanged();
            }
        }

        public override void Calculate()
        {
            var a = (double)GetInputValue(0);
            var b = (double)GetInputValue(1);
            var @out = a + b;
            Out = @out;
            Project.WriteValue(itemId, 0, ValueSide.Output, ValueKind.Analog, Out);
        }

        public override object GetInputValue(int pin)
        {
            (Guid id, int pinout, bool _) = getLinkSources[pin];
            if (id != Guid.Empty)
                return (double)(Project.ReadValue(id, pinout, ValueSide.Output, ValueKind.Analog)?.Value ?? double.NaN);
            return (double)(Project.ReadValue(itemId, pin, ValueSide.Input, ValueKind.Analog)?.Value ?? double.NaN);
        }

        public override void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex < 0 || inputIndex >= getLinkSources.Length)
                return;
            if (value != null && !LinkedInputs[inputIndex])
            {
                getInputs[inputIndex] = (double)value;
                Project.WriteValue(itemId, inputIndex, ValueSide.Input, ValueKind.Analog, (double)value);
            }
        }
    }
}
