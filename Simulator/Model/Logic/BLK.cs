
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Logic
{
    public class BLK : CommonLogic, IBlock
    {
        public BLK() : base(LogicFunction.Block, 4, 4) { }

        public BLK(Guid id) : base(LogicFunction.Block, 4, 4) 
        { 
            LibraryId = id;
        }

        [Browsable(false)]
        public override string FuncSymbol => "BLK"; // Функциональный блок с внутренним модулем

        [Browsable(false)]
        public Guid LibraryId { get; set; }

        [Browsable(false)]
        private Model.Module? Internal { get; set; }

        [Category("Библиотека"), DisplayName("Имя")]
        public string LibraryName { get; private set; } = string.Empty;

        [Category("Библиотека"), DisplayName("Описание")]
        public string LibraryDescription { get; private set; } = string.Empty;

        public override void Calculate()
        {
            if (Internal != null) 
            {
                // перезапись состояний входов сборки на элементы DI внутреннего модуля
                foreach (var item in Internal.Elements)
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
                Internal.GetCalculationMethod().Invoke();
                // перезапись состояний выходов сборки из элементов DO внутреннего модуля
                foreach (var item in Internal.Elements)
                {
                    if (item.Instance is Model.Outputs.DO @do)
                    {
                        var index = @do.Order;
                        if (index >= 0 && index < OutputValues.Length)
                        {
                            bool value = ((bool?)@do.GetValueFromInp(0)) ?? false;
                            OutputValues[index] = value;
                            Project.WriteBoolValue($"{ItemId}\t{index}", value);
                        }
                    }
                }
            }
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            xtance.Add(new XElement("LibraryId", LibraryId));
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            if (Guid.TryParse(xtance?.Element("LibraryId")?.Value, out Guid id))
            {
                LibraryId = id;
                ConnectToLibrary();
            }
        }

        public void ConnectToLibrary()
        {
            var lib = Project.Blocks.FirstOrDefault(x => x.Id == LibraryId);
            if (lib != null)
            {
                Internal = lib.DeepCopy();
                LibraryName = lib.Name;
                LibraryDescription = lib.Description;
                foreach (var item in Internal.Elements)
                {
                    if (item.Instance is Model.Inputs.DI di)
                    {
                        if (di.Order < InputNames.Length)
                            InputNames[di.Order] = di.Name ?? "";
                    }
                    if (item.Instance is Model.Outputs.DO @do)
                    {
                        if (@do.Order < OutputNames.Length)
                            OutputNames[@do.Order] = @do.Name ?? "";
                    }
                }
            }
        }
    }
}
