
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace Simulator.Model.Logic
{
    public class ASMBLY : CommonLogic, IAssembly
    {
        public ASMBLY() : base(LogicFunction.Assembly, 4, 4) { }

        [Browsable(false)]
        public override string FuncSymbol => "ASM"; // Детектор фронта

        [Browsable(false)]
        public Model.Module? InternalModule { get; set; }

        public override void Save(XElement xtem)
        {
            base.Save(xtem);
            XElement? xtance = xtem.Element("Instance");
            if (xtance == null)
            {
                xtance = new XElement("Instance");
                xtem.Add(xtance);
            }
            XElement xmodule = new("Module");
            xtance.Add(xmodule);
            InternalModule?.Save(xmodule);

        }

        public override void Load(XElement? xtem)
        {
            var xmodule = xtem?.Element("Module");
            if (xmodule == null) return;
            InternalModule = InternalModule ?? new Module();
            InternalModule.Load(xmodule);
        }
    }
}
