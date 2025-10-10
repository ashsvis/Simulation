using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;

namespace Simulator.Model.Mathematic
{
    public class Numeric : CommonAnalog, IContextMenu, IManualCommand
    {
        public Numeric() : base(LogicFunction.Numeric, 0, 1)
        {
        }

        [Browsable(false)]
        public double Value { get; set; }

        public override void Calculate()
        {
            Project.WriteValue(ItemId, 0, ValueDirect.Output, ValueKind.Analog, Value);
        }

        public object? GetValueFromOut(int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < Outputs.Length)
            {
                //if (Outputs[outputIndex] is AnalogOutput output)
                //    return output.Value;
                return Value;
            }
            return null;
        }

        public new void SetValueToOut(int outputIndex, object? value)
        {
            if (outputIndex != 0) return;
            if (value != null)
            {
                if (Outputs[outputIndex] is AnalogOutput analog)
                {
                    if (Value != (double)value)
                    {
                        Value = (double)value;
                        Project.WriteValue(ItemId, 0, ValueDirect.Output, ValueKind.Analog, Value);
                        analog.Value = (double)value;
                        if (!Project.Running)
                            Project.Changed = true;
                    }
                }
            }
        }

        public override void Init()
        {
            // leave this empty
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            xtance.Add(new XElement("Value", Value));
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            if (double.TryParse(xtance?.Element("Value")?.Value, CultureInfo.GetCultureInfo("en-US"), out double value))
            {
                Value = value;
                SetValueToOut(0, Value);
                ((AnalogOutput)Outputs[0]).Value = value;
            }
        }

        public override void Draw(Graphics graphics, Color foreColor, Color backColor, PointF location, SizeF size,
            int index, bool selected, CustomDraw? customDraw = null)
        {
            base.Draw(graphics, foreColor, backColor, location, size, index, selected, customDraw);
            var fp = CultureInfo.GetCultureInfo("en-US");
            var text = Math.Round(Value, 4).ToString("0.####", fp);
            using var font = new Font("Consolas", Element.Step + 2f);
            using var fontbrush = new SolidBrush(selected ? Color.Magenta : foreColor);
            var ms = graphics.MeasureString(text, font);
            var step = Element.Step;
            var height = step + 1 * step * 4 + step;
            var width = step + 1 * step * 4 + step;
            var x = width + location.X;
            var y = height / 2 + location.Y;
            graphics.DrawString(text, font, fontbrush, new PointF(x, y - ms.Height));

        }
    }
}
