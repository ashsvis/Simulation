
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Logic
{
    public class ASMBLY : CommonLogic, IAssembly
    {
        public ASMBLY() : base(LogicFunction.Assembly, 4, 4) { }

        [Browsable(false)]
        public override string FuncSymbol => "ASM"; // Детектор фронта

        [Browsable(false)]
        public Model.Module? ModuleInternal { get; set; }
        public override void Calculate()
        {
            if (ModuleInternal != null) 
            {
                foreach (var item in ModuleInternal.Elements)
                {
                    if (item.Instance is Model.Inputs.DI di)
                    {
                        var index = di.Order;
                        if (index >= 0 && index < InputValues.Length)
                        {
                            bool value = (bool)InputValues[index];
                            di.SetValueToOut(0, value);
                        }
                    }
                }
                ModuleInternal.GetCalculationMethod().Invoke();
                foreach (var item in ModuleInternal.Elements)
                {
                    if (item.Instance is Model.Outputs.DO @do)
                    {
                        var index = @do.Order;
                        if (index >= 0 && index < OutputValues.Length)
                        {
                            bool value = ((bool?)@do.GetValueFromInp(0)) ?? false;
                            OutputValues[index] = value;
                        }
                    }
                }
            }
        }

        public override void Save(XElement xtance)
        {
            XElement xmodule = new("Module");
            xtance.Add(xmodule);
            ModuleInternal?.Save(xmodule);

        }

        public override void Load(XElement? xtem)
        {
            var xmodule = xtem?.Element("Module");
            if (xmodule == null) return;
            ModuleInternal = ModuleInternal ?? new Module();
            ModuleInternal.Load(xmodule);
        }
    }
}
